using System.Globalization;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using FFBC.Models;
using FFBC.Options;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FFBC.Services;

public class FfbcWebEventStore : IEventStore
{
    private static readonly CultureInfo FrenchBelgianCulture = CultureInfo.GetCultureInfo("fr-BE");
    private static readonly Regex PostalCodeRegex = new(@"^(?:(.+)\s-\s)?(?:([BF])-)?(\d{4,5})\s+(.+)$", RegexOptions.Compiled);

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<FfbcWebEventStore> _logger;
    private readonly FfbcEventStoreOptions _options;

    public FfbcWebEventStore(
        IHttpClientFactory httpClientFactory,
        IMemoryCache memoryCache,
        IOptions<FfbcEventStoreOptions> options,
        ILogger<FfbcWebEventStore> logger)
    {
        _httpClientFactory = httpClientFactory;
        _memoryCache = memoryCache;
        _logger = logger;
        _options = options.Value;
    }

    public IReadOnlyList<Event> GetAll()
    {
        var cacheKey = BuildCacheKey(_options);
        if (_memoryCache.TryGetValue<IReadOnlyList<Event>>(cacheKey, out var cachedEvents) && cachedEvents is not null)
        {
            return cachedEvents;
        }

        try
        {
            var events = FetchAndParse();
            if (events is null)
            {
                return [];
            }

            _memoryCache.Set(
                cacheKey,
                events,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.CacheDurationMinutes)
                });

            return events;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch FFBC events from {PostUrl}", _options.PostUrl);
            return [];
        }
    }

    public Event? GetByDateAndTitle(DateTime date, string title) =>
        GetAll().FirstOrDefault(e => e.Date.Date == date.Date
            && string.Equals(e.Title, title, StringComparison.OrdinalIgnoreCase));

    private IReadOnlyList<Event>? FetchAndParse()
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, _options.PostUrl)
        {
            Content = new FormUrlEncodedContent(BuildPayload(_options))
        };

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
        request.Headers.Add("X-Requested-With", "XMLHttpRequest");

        var client = _httpClientFactory.CreateClient("ffbc");
        using var response = client.SendAsync(request).GetAwaiter().GetResult();
        response.EnsureSuccessStatusCode();

        var html = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        if (!TryParseEvents(html, out var events))
        {
            _logger.LogWarning("FFBC response could not be parsed into calendar rows");
            return null;
        }

        return events;
    }

    public static string BuildCacheKey(FfbcEventStoreOptions options) =>
        $"ffbc-events::{options.BaseUrl}::{options.Year}::{options.Challenge}::{options.VttRoute}::{options.Province}::{options.Date}";

    public static IReadOnlyDictionary<string, string> BuildPayload(FfbcEventStoreOptions options) =>
        new Dictionary<string, string>
        {
            ["ajax"] = "1",
            ["baseUrl"] = options.BaseUrl,
            ["year_selected"] = options.Year.ToString(CultureInfo.InvariantCulture),
            ["challenge"] = options.Challenge,
            ["vtt_route"] = options.VttRoute,
            ["province"] = options.Province,
            ["date"] = options.Date
        };

    internal static bool TryParseEvents(string html, out IReadOnlyList<Event> events)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var rows = document.DocumentNode.SelectNodes("//div[contains(concat(' ', normalize-space(@class), ' '), ' calendar-row ')]");
        if (rows is null)
        {
            events = [];
            return false;
        }

        var parsedEvents = new List<Event>();

        foreach (var row in rows)
        {
            var eventId = row.SelectSingleNode(".//div[contains(@class, 'calendar-row-informations')]")?.GetAttributeValue("id", null);
            var title = HtmlEntity.DeEntitize(row.SelectSingleNode(".//div[contains(@class, 'calendar-row-name')]")?.InnerText ?? string.Empty).Trim();
            var dateText = HtmlEntity.DeEntitize(row.SelectSingleNode(".//div[contains(@class, 'calendar-row-date')]")?.InnerText ?? string.Empty).Trim();
            var placeText = HtmlEntity.DeEntitize(row.SelectSingleNode(".//div[contains(@class, 'calendar-row-place')]")?.InnerText ?? string.Empty).Trim();
            var challengeText = HtmlEntity.DeEntitize(row.SelectSingleNode(".//div[contains(@class, 'calendar-row-challenge')]")?.InnerText ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(title) || !TryParseFrenchDate(dateText, out var date))
            {
                continue;
            }

            var normalizedPlace = NormalizeWhitespace(placeText);
            var normalizedChallenge = NormalizeWhitespace(challengeText);
            ParsePlace(normalizedPlace, out var town, out var postalCode, out var country, out var province);

            var notesParts = new[] { normalizedPlace, normalizedChallenge }
                .Where(static part => !string.IsNullOrWhiteSpace(part));

            parsedEvents.Add(new Event
            {
                EventId = eventId,
                Date = date,
                Title = NormalizeWhitespace(title),
                Notes = string.Join(" | ", notesParts),
                Town = town,
                PostalCode = postalCode,
                Country = country,
                Province = province,
                Challenge = string.IsNullOrWhiteSpace(normalizedChallenge) ? null : normalizedChallenge
            });
        }

        events = parsedEvents;
        return parsedEvents.Count > 0;
    }

    internal static void ParsePlace(string placeText, out string? town, out string? postalCode, out string? country, out string? province)
    {
        if (string.IsNullOrWhiteSpace(placeText))
        {
            town = null;
            postalCode = null;
            country = null;
            province = null;
            return;
        }

        var match = PostalCodeRegex.Match(placeText);
        if (match.Success)
        {
            province = match.Groups[1].Success ? match.Groups[1].Value.Trim() : null;
            var prefix = match.Groups[2].Value;
            postalCode = match.Groups[3].Value;
            town = match.Groups[4].Value.Trim();
            country = prefix == "F" || (prefix.Length == 0 && postalCode.Length == 5) ? "France" : "Belgium";
        }
        else
        {
            postalCode = null;
            town = placeText;
            country = null;
            province = null;
        }
    }

    private static string NormalizeWhitespace(string value) =>
        string.Join(' ', value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

    private static bool TryParseFrenchDate(string value, out DateTime date)
    {
        var normalized = NormalizeWhitespace(value);
        return DateTime.TryParseExact(
            normalized,
            "dddd dd MMMM yyyy",
            FrenchBelgianCulture,
            DateTimeStyles.None,
            out date);
    }
}
