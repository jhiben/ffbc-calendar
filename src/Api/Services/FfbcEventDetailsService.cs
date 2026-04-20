using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using FFBC.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FFBC.Services;

public partial class FfbcEventDetailsService : IEventDetailsService
{
    private const string PopupApiUrl = "https://www.velo-liberte.be/wp-content/themes/zapedah-child/assets/scripts/ajax_request_popup_calendar.php";
    private const string CacheKeyPrefix = "event-details::";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(2);

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<FfbcEventDetailsService> _logger;

    public FfbcEventDetailsService(
        IHttpClientFactory httpClientFactory,
        IMemoryCache memoryCache,
        ILogger<FfbcEventDetailsService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<EventDetails?> GetEventDetailsAsync(string eventId)
    {
        if (string.IsNullOrWhiteSpace(eventId))
        {
            return null;
        }

        var cacheKey = $"{CacheKeyPrefix}{eventId}";

        if (_memoryCache.TryGetValue<EventDetails>(cacheKey, out var cachedDetails) && cachedDetails is not null)
        {
            return cachedDetails;
        }

        try
        {
            var details = await FetchDetailsFromApiAsync(eventId);
            if (details is not null)
            {
                _memoryCache.Set(cacheKey, details, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CacheDuration
                });
            }
            return details;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Failed to fetch event details for ID {EventId}", eventId);
            return null;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "Timeout fetching event details for ID {EventId}", eventId);
            return null;
        }
    }

    private async Task<EventDetails?> FetchDetailsFromApiAsync(string eventId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, PopupApiUrl)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["id"] = eventId,
                ["ajax"] = "1"
            })
        };

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
        request.Headers.Add("X-Requested-With", "XMLHttpRequest");

        var client = _httpClientFactory.CreateClient("ffbc");
        using var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Popup API returned status {StatusCode} for event ID {EventId}",
                response.StatusCode, eventId);
            return null;
        }

        var html = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(html))
        {
            return null;
        }

        return ParseEventDetails(eventId, html);
    }

    internal static EventDetails? ParseEventDetails(string eventId, string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var details = new EventDetails
        {
            EventId = eventId,
            HtmlContent = html
        };

        var content = document.DocumentNode;

        var registrationNode = content.SelectSingleNode("//div[contains(@class, 'calendar-details-subscription')]//a[@href]");
        if (registrationNode is not null)
        {
            details.RegistrationUrl = registrationNode.GetAttributeValue("href", null);
        }

        var placeNode = content.SelectSingleNode("//div[contains(@class, 'calendar-details-place')]");
        if (placeNode is not null)
        {
            ParsePlace(placeNode, details);
        }

        var clubNode = content.SelectSingleNode("//div[contains(@class, 'calendar-details-club')]");
        if (clubNode is not null)
        {
            var clubText = NormalizeText(clubNode.InnerText);
            details.Club = StripLabel(clubText, "Club organisateur :");
        }

        var activityRows = content.SelectNodes("//div[contains(@class, 'calendar-activities-row')]");
        if (activityRows is not null)
        {
            var activities = new List<Activity>();
            foreach (var row in activityRows)
            {
                var activity = ParseActivity(row);
                if (activity is not null)
                {
                    activities.Add(activity);
                }
            }
            details.Activities = activities;
        }

        var noteNode = content.SelectSingleNode("//div[contains(@class, 'calendar-details-note')]");
        if (noteNode is not null)
        {
            var noteText = NormalizeText(noteNode.InnerText);
            details.Notes = StripLabel(noteText, "Remarque :");
        }

        var websiteNode = content.SelectSingleNode("//div[contains(@class, 'calendar-details-website')]//a[@href]");
        if (websiteNode is not null)
        {
            details.Website = websiteNode.GetAttributeValue("href", null);
        }

        return details;
    }

    private static void ParsePlace(HtmlNode placeNode, EventDetails details)
    {
        var mapsLink = placeNode.SelectSingleNode(".//a[contains(@href, 'google.com/maps')]");
        if (mapsLink is not null)
        {
            details.MapsUrl = HtmlEntity.DeEntitize(mapsLink.GetAttributeValue("href", null));
        }

        var innerHtml = placeNode.InnerHtml;
        var parts = innerHtml.Split(new[] { "<br>", "<br/>", "<br />" }, StringSplitOptions.RemoveEmptyEntries);

        var textParts = new List<string>();
        foreach (var part in parts)
        {
            var cleaned = HtmlEntity.DeEntitize(StripHtmlTags(part)).Trim();
            if (string.IsNullOrWhiteSpace(cleaned)
                || cleaned.Contains("Lieu de départ", StringComparison.OrdinalIgnoreCase)
                || cleaned.Contains("Itinéraire", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            textParts.Add(cleaned);
        }

        if (textParts.Count > 0)
        {
            details.StartLocation = textParts[0];
        }
        if (textParts.Count > 1)
        {
            details.Address = textParts[1];
        }
    }

    private static Activity? ParseActivity(HtmlNode row)
    {
        var typeNode = row.SelectSingleNode(".//div[contains(@class, 'calendar-activities-row-type')]");
        if (typeNode is null) return null;

        var typeText = NormalizeText(typeNode.InnerText);

        return new Activity
        {
            Type = StripLabel(typeText, "Type :") ?? string.Empty,
            Distance = ExtractField(row, "calendar-activities-row-km"),
            Elevation = ExtractField(row, "calendar-activities-row-denivele", "D+ :"),
            Difficulty = ExtractField(row, "calendar-activities-row-dificulty"),
            Time = ExtractField(row, "calendar-activities-row-date"),
            Ravito = ExtractField(row, "calendar-activities-row-ravito", "Ravito :"),
            Price = ExtractField(row, "calendar-activities-row-price-ffbc", "Tarif :")
        };
    }

    private static string? ExtractField(HtmlNode row, string cssClass, string? labelToStrip = null)
    {
        var node = row.SelectSingleNode($".//div[contains(@class, '{cssClass}')]");
        if (node is null) return null;

        var text = NormalizeText(node.InnerText);
        if (labelToStrip is not null)
        {
            text = StripLabel(text, labelToStrip);
        }
        return text;
    }

    internal static string? StripLabel(string? text, string label)
    {
        if (text is null) return null;

        var idx = text.IndexOf(label, StringComparison.OrdinalIgnoreCase);
        if (idx >= 0)
        {
            text = text[(idx + label.Length)..].Trim();
        }
        return string.IsNullOrWhiteSpace(text) ? null : text;
    }

    private static string StripHtmlTags(string html)
    {
        return HtmlTagRegex().Replace(html, string.Empty);
    }

    private static string? NormalizeText(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        var normalized = HtmlEntity.DeEntitize(text);
        normalized = WhitespaceRegex().Replace(normalized, " ").Trim();

        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();

    [GeneratedRegex(@"<[^>]+>")]
    private static partial Regex HtmlTagRegex();
}
