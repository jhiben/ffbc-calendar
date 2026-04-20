using System.Text.Json;
using FFBC.Functions;
using FFBC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace FFBC.Tests;

public class EventsFunctionTests
{
    [Fact]
    public async Task Run_ReturnsJsonOfEventsOrderedByDate()
    {
        var events = new List<Event>
        {
            new() { Date = new DateTime(2026, 5, 1), Title = "Later" },
            new() { Date = new DateTime(2026, 3, 1), Title = "Earlier" }
        };
        var store = new StubEventStore(events);
        var function = new EventsFunction(store, NullLogger<EventsFunction>.Instance);
        var httpContext = new DefaultHttpContext();

        var result = function.Run(httpContext.Request);

        var (status, body) = await ExecuteResult(result);
        Assert.Equal(200, status);

        var returned = JsonSerializer.Deserialize<List<JsonElement>>(body);
        Assert.NotNull(returned);
        Assert.Equal(2, returned!.Count);
        Assert.Equal("Earlier", returned[0].GetProperty("title").GetString());
        Assert.Equal("Later", returned[1].GetProperty("title").GetString());
    }

    [Fact]
    public async Task Run_ReturnsEmptyArray_WhenNoEvents()
    {
        var store = new StubEventStore([]);
        var function = new EventsFunction(store, NullLogger<EventsFunction>.Instance);
        var httpContext = new DefaultHttpContext();

        var result = function.Run(httpContext.Request);

        var (status, body) = await ExecuteResult(result);
        Assert.Equal(200, status);

        var returned = JsonSerializer.Deserialize<List<JsonElement>>(body);
        Assert.NotNull(returned);
        Assert.Empty(returned!);
    }

    private static async Task<(int StatusCode, string Body)> ExecuteResult(IResult result)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.RequestServices = new ServiceCollection().AddLogging().BuildServiceProvider();
        httpContext.Response.Body = new MemoryStream();
        await result.ExecuteAsync(httpContext);
        httpContext.Response.Body.Position = 0;
        var body = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
        return (httpContext.Response.StatusCode, body);
    }
}
