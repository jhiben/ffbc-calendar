using System.Text.Json;
using FFBC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FFBC.Functions;

public class EventDetailsFunction
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly IEventDetailsService _eventDetailsService;
    private readonly ILogger<EventDetailsFunction> _logger;

    public EventDetailsFunction(IEventDetailsService eventDetailsService, ILogger<EventDetailsFunction> logger)
    {
        _eventDetailsService = eventDetailsService;
        _logger = logger;
    }

    [Function("GetEventDetails")]
    public async Task<IResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "events/{eventId}/details")] HttpRequest req,
        string eventId)
    {
        if (string.IsNullOrWhiteSpace(eventId))
        {
            return Results.NotFound();
        }

        try
        {
            var details = await _eventDetailsService.GetEventDetailsAsync(eventId);
            if (details is null)
            {
                return Results.NotFound();
            }

            return Results.Json(details, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve event details for {EventId}", eventId);
            return Results.Problem("Failed to retrieve event details", statusCode: 500);
        }
    }
}
