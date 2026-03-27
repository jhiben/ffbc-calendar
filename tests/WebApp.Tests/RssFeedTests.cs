using FFBC.Models;
using FFBC.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;

namespace FFBC.Tests;

public class RssFeedTests : IClassFixture<WebApplicationFactory<Program>>
{
    private static readonly Event[] SeedEvents =
    [
        new Event
        {
            Date = new DateTime(2026, 3, 25),
            Title = "Rallye des Spirous",
            Town = "Spy",
            PostalCode = "5190",
            Country = "Belgium",
            Province = "Namur",
            Notes = "Brevet à Dénivelé"
        },
        new Event
        {
            Date = new DateTime(2026, 4, 12),
            Title = "La Florancy",
            Town = "Messancy",
            PostalCode = "6780",
            Country = "Belgium",
            Province = "Luxembourg"
        }
    ];

    private readonly HttpClient _client;

    public RssFeedTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IEventStore));
                if (descriptor is not null)
                    services.Remove(descriptor);

                services.AddSingleton<IEventStore>(new InMemoryEventStore(SeedEvents));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task FeedEndpoint_Returns200_WithRssContentType()
    {
        var response = await _client.GetAsync("/feed.xml");

        response.EnsureSuccessStatusCode();
        Assert.Equal("application/rss+xml", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task FeedXml_ContainsChannelMetadata()
    {
        var xml = await _client.GetStringAsync("/feed.xml");
        var doc = XDocument.Parse(xml);

        var channel = doc.Root!.Element("channel")!;
        Assert.Equal("FFBC Calendar Events", channel.Element("title")!.Value);
        Assert.Equal("Upcoming FFBC-licensed mountain bike events", channel.Element("description")!.Value);
        Assert.NotNull(channel.Element("link"));
    }

    [Fact]
    public async Task FeedXml_ContainsItemsWithExpectedElements()
    {
        var xml = await _client.GetStringAsync("/feed.xml");
        var doc = XDocument.Parse(xml);

        var items = doc.Descendants("item").ToList();
        Assert.Equal(2, items.Count);

        // Items ordered by date — first is Rallye des Spirous
        var first = items[0];
        Assert.Equal("Rallye des Spirous", first.Element("title")!.Value);
        Assert.Contains("Spy", first.Element("description")!.Value);
        Assert.Contains("Belgium", first.Element("description")!.Value);
        Assert.NotNull(first.Element("link"));
        Assert.NotNull(first.Element("pubDate"));
        Assert.Contains("EventDetail", first.Element("link")!.Value);
    }

    [Fact]
    public async Task FeedXml_WithEmptyStore_ReturnsValidFeedWithNoItems()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IEventStore));
                if (descriptor is not null)
                    services.Remove(descriptor);

                services.AddSingleton<IEventStore>(new InMemoryEventStore([]));
            });
        });

        using var client = factory.CreateClient();
        var xml = await client.GetStringAsync("/feed.xml");
        var doc = XDocument.Parse(xml);

        var channel = doc.Root!.Element("channel")!;
        Assert.Equal("FFBC Calendar Events", channel.Element("title")!.Value);
        Assert.Empty(doc.Descendants("item"));
    }
}
