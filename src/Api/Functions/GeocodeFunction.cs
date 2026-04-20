using FFBC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FFBC.Functions;

public class GeocodeFunction
{
    private readonly IGeocodingService _geocodingService;
    private readonly ILogger<GeocodeFunction> _logger;

    public GeocodeFunction(IGeocodingService geocodingService, ILogger<GeocodeFunction> logger)
    {
        _geocodingService = geocodingService;
        _logger = logger;
    }

    [Function("Geocode")]
    public async Task<IResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "geocode/{postalCode}")] HttpRequest req,
        string postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode))
        {
            return Results.NotFound();
        }

        try
        {
            var country = req.Query["country"].FirstOrDefault();
            var coords = await _geocodingService.GeocodeAsync(postalCode, null, country);

            if (coords is null)
            {
                return Results.NotFound();
            }

            return Results.Json(new { latitude = coords.Value.Latitude, longitude = coords.Value.Longitude });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to geocode postal code {PostalCode}", postalCode);
            return Results.Problem("Failed to geocode", statusCode: 500);
        }
    }
}
