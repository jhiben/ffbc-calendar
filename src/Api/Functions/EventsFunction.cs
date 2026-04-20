using System.Text.Json;
using FFBC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FFBC.Functions;

public class EventsFunction
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly IEventStore _eventStore;
    private readonly ILogger<EventsFunction> _logger;

    public EventsFunction(IEventStore eventStore, ILogger<EventsFunction> logger)
    {
        _eventStore = eventStore;
        _logger = logger;
    }

    [Function("GetEvents")]
    public IResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "events")] HttpRequest req)
    {
        try
        {
            var events = _eventStore.GetAll()
                .OrderBy(e => e.Date)
                .ToList();

            return Results.Json(events, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve events");
            return Results.Problem("Failed to retrieve events", statusCode: 500);
        }
    }
}
