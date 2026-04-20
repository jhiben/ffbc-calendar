using System.Text.Json;
using FFBC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace FFBC.Functions;

public class GeocodeFunction
{
    private readonly IGeocodingService _geocodingService;

    public GeocodeFunction(IGeocodingService geocodingService)
    {
        _geocodingService = geocodingService;
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

        var country = req.Query["country"].FirstOrDefault();
        var coords = await _geocodingService.GeocodeAsync(postalCode, null, country);

        if (coords is null)
        {
            return Results.NotFound();
        }

        return Results.Json(new { latitude = coords.Value.Latitude, longitude = coords.Value.Longitude });
    }
}
