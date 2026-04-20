using System.Text.Json;
using FFBC.Functions;
using FFBC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace FFBC.Tests;

public class EventDetailsFunctionTests
{
    [Fact]
    public async Task Run_ReturnsDetails_WhenFound()
    {
        var details = new EventDetails { EventId = "123", Club = "Test Club" };
        var service = new StubEventDetailsService(details);
        var function = new EventDetailsFunction(service, NullLogger<EventDetailsFunction>.Instance);
        var httpContext = new DefaultHttpContext();

        var result = await function.Run(httpContext.Request, "123");

        var (status, body) = await ExecuteResult(result);
        Assert.Equal(200, status);

        var json = JsonSerializer.Deserialize<JsonElement>(body);
        Assert.Equal("123", json.GetProperty("eventId").GetString());
        Assert.Equal("Test Club", json.GetProperty("club").GetString());
    }

    [Fact]
    public async Task Run_ReturnsNotFound_WhenNull()
    {
        var service = new StubEventDetailsService(null);
        var function = new EventDetailsFunction(service, NullLogger<EventDetailsFunction>.Instance);
        var httpContext = new DefaultHttpContext();

        var result = await function.Run(httpContext.Request, "999");

        var (status, _) = await ExecuteResult(result);
        Assert.Equal(404, status);
    }

    [Fact]
    public async Task Run_ReturnsNotFound_WhenEventIdIsEmpty()
    {
        var service = new StubEventDetailsService(null);
        var function = new EventDetailsFunction(service, NullLogger<EventDetailsFunction>.Instance);
        var httpContext = new DefaultHttpContext();

        var result = await function.Run(httpContext.Request, "  ");

        var (status, _) = await ExecuteResult(result);
        Assert.Equal(404, status);
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
