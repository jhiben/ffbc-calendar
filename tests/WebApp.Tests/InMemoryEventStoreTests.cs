using FFBC.Models;
using FFBC.Services;

namespace FFBC.Tests;

public class InMemoryEventStoreTests
{
    private static readonly Event[] SeedEvents =
    [
        new Event { Date = new DateTime(2026, 3, 25), Title = "Rallye des Spirous" },
        new Event { Date = new DateTime(2026, 4, 25), Title = "La Florancy" }
    ];

    [Fact]
    public void GetByDateAndTitle_ReturnsMatchingEvent()
    {
        var store = new InMemoryEventStore(SeedEvents);

        var result = store.GetByDateAndTitle(new DateTime(2026, 3, 25), "Rallye des Spirous");

        Assert.NotNull(result);
        Assert.Equal("Rallye des Spirous", result!.Title);
    }

    [Fact]
    public void GetByDateAndTitle_ReturnsNull_WhenNoMatch()
    {
        var store = new InMemoryEventStore(SeedEvents);

        var result = store.GetByDateAndTitle(new DateTime(2026, 3, 25), "Nonexistent");

        Assert.Null(result);
    }

    [Fact]
    public void GetByDateAndTitle_IsCaseInsensitive()
    {
        var store = new InMemoryEventStore(SeedEvents);

        var result = store.GetByDateAndTitle(new DateTime(2026, 3, 25), "rallye des spirous");

        Assert.NotNull(result);
        Assert.Equal("Rallye des Spirous", result!.Title);
    }

    [Fact]
    public void GetByDateAndTitle_ReturnsNull_WhenDateDoesNotMatch()
    {
        var store = new InMemoryEventStore(SeedEvents);

        var result = store.GetByDateAndTitle(new DateTime(2026, 5, 1), "Rallye des Spirous");

        Assert.Null(result);
    }
}
