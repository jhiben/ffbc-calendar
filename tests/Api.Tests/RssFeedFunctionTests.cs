using FFBC.Functions;
using FFBC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace FFBC.Tests;

public class RssFeedFunctionTests
{
    [Fact]
    public async Task Run_ReturnsRssXml_WithChannelMetadata()
    {
        var events = new List<Event>
        {
            new() { Date = new DateTime(2026, 3, 25), Title = "Rallye des Spirous", Town = "Spy", Country = "Belgium" }
        };
        var store = new StubEventStore(events);
        var function = new RssFeedFunction(store, NullLogger<RssFeedFunction>.Instance);
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("example.com");

        var result = function.Run(httpContext.Request);

        var (status, body) = await ExecuteResult(result);
        Assert.Equal(200, status);
        Assert.Contains("<rss", body);
        Assert.Contains("FFBC Calendar Events", body);
        Assert.Contains("Rallye des Spirous", body);
    }

    [Fact]
    public async Task Run_ReturnsItemsOrderedByDate()
    {
        var events = new List<Event>
        {
            new() { Date = new DateTime(2026, 5, 1), Title = "Later Event", Town = "Namur" },
            new() { Date = new DateTime(2026, 3, 1), Title = "Earlier Event", Town = "Spy" }
        };
        var store = new StubEventStore(events);
        var function = new RssFeedFunction(store, NullLogger<RssFeedFunction>.Instance);
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("example.com");

        var result = function.Run(httpContext.Request);

        var (_, body) = await ExecuteResult(result);
        var earlierIndex = body.IndexOf("Earlier Event", StringComparison.Ordinal);
        var laterIndex = body.IndexOf("Later Event", StringComparison.Ordinal);
        Assert.True(earlierIndex < laterIndex, "Earlier event should appear before later event in RSS feed");
    }

    [Fact]
    public async Task Run_ReturnsEmptyFeed_WhenNoEvents()
    {
        var store = new StubEventStore([]);
        var function = new RssFeedFunction(store, NullLogger<RssFeedFunction>.Instance);
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("example.com");

        var result = function.Run(httpContext.Request);

        var (status, body) = await ExecuteResult(result);
        Assert.Equal(200, status);
        Assert.Contains("<rss", body);
        Assert.Contains("FFBC Calendar Events", body);
        Assert.DoesNotContain("<item>", body);
    }

    [Fact]
    public async Task Run_IncludesDescriptionFromTownCountryAndNotes()
    {
        var events = new List<Event>
        {
            new()
            {
                Date = new DateTime(2026, 3, 25),
                Title = "Test Ride",
                Town = "Spy",
                Country = "Belgium",
                Notes = "Some notes"
            }
        };
        var store = new StubEventStore(events);
        var function = new RssFeedFunction(store, NullLogger<RssFeedFunction>.Instance);
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("example.com");

        var result = function.Run(httpContext.Request);

        var (_, body) = await ExecuteResult(result);
        Assert.Contains("Spy", body);
        Assert.Contains("Belgium", body);
        Assert.Contains("Some notes", body);
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
