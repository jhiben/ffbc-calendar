using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace FFBC.Services;

public class NominatimGeocodingService : IGeocodingService
{
    private const string NominatimBaseUrl = "https://nominatim.openstreetmap.org/search";
    private const string CacheKeyPrefix = "geocode::";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<NominatimGeocodingService> _logger;

    public NominatimGeocodingService(
        IHttpClientFactory httpClientFactory,
        IMemoryCache memoryCache,
        ILogger<NominatimGeocodingService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<(double Latitude, double Longitude)?> GeocodeAsync(string postalCode, string? town, string? country)
    {
        var cacheKey = $"{CacheKeyPrefix}{postalCode}";
        if (_memoryCache.TryGetValue<(double, double)?>(cacheKey, out var cached))
        {
            return cached;
        }

        var result = await FetchCoordinatesAsync(postalCode, town, country);

        _memoryCache.Set(cacheKey, result, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        });

        return result;
    }

    private async Task<(double Latitude, double Longitude)?> FetchCoordinatesAsync(string postalCode, string? town, string? country)
    {
        var resolvedCountry = country ?? "Belgium";
        var url = $"{NominatimBaseUrl}?postalcode={Uri.EscapeDataString(postalCode)}&country={Uri.EscapeDataString(resolvedCountry)}&format=json&limit=1";

        var client = _httpClientFactory.CreateClient("nominatim");

        try
        {
            using var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var results = doc.RootElement;

            if (results.GetArrayLength() == 0)
            {
                _logger.LogDebug("No geocoding results for postal code {PostalCode}", postalCode);
                return null;
            }

            var first = results[0];
            if (first.TryGetProperty("lat", out var latEl) &&
                first.TryGetProperty("lon", out var lonEl) &&
                double.TryParse(latEl.GetString(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var lat) &&
                double.TryParse(lonEl.GetString(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var lon))
            {
                return (lat, lon);
            }

            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Geocoding failed for postal code {PostalCode}", postalCode);
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Failed to parse geocoding response for postal code {PostalCode}", postalCode);
            return null;
        }
    }
}
