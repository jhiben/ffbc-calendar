using System.Text.Json;
using FFBC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace FFBC.Functions;

public class EventsFunction
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly IEventStore _eventStore;

    public EventsFunction(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    [Function("GetEvents")]
    public IResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "events")] HttpRequest req)
    {
        var events = _eventStore.GetAll()
            .OrderBy(e => e.Date)
            .ToList();

        return Results.Json(events, JsonOptions);
    }
}
