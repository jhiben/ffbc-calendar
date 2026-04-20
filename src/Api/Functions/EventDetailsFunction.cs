using System.Text.Json;
using FFBC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace FFBC.Functions;

public class EventDetailsFunction
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly IEventDetailsService _eventDetailsService;

    public EventDetailsFunction(IEventDetailsService eventDetailsService)
    {
        _eventDetailsService = eventDetailsService;
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

        var details = await _eventDetailsService.GetEventDetailsAsync(eventId);
        if (details is null)
        {
            return Results.NotFound();
        }

        return Results.Json(details, JsonOptions);
    }
}
