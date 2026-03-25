using FFBC.Models;
using FFBC.Pages;
using FFBC.Services;

namespace FFBC.Tests;

public class ListEventsTests
{
    private static ListEventsModel BuildModel(IEnumerable<Event> events)
    {
        var store = new InMemoryEventStore(events);
        return new ListEventsModel(store);
    }

    [Fact]
    public void OnGet_ReturnsEventsInAscendingDateOrder()
    {
        var events = new[]
        {
            new Event { Date = new DateTime(2026, 5, 1), Title = "Later Ride" },
            new Event { Date = new DateTime(2026, 3, 1), Title = "Earlier Ride" },
            new Event { Date = new DateTime(2026, 4, 1), Title = "Middle Ride" },
        };
        var model = BuildModel(events);

        model.OnGet();

        var result = model.Events.ToList();
        Assert.Equal(3, result.Count);
        Assert.Equal(new DateTime(2026, 3, 1), result[0].Date);
        Assert.Equal(new DateTime(2026, 4, 1), result[1].Date);
        Assert.Equal(new DateTime(2026, 5, 1), result[2].Date);
    }

    [Fact]
    public void OnGet_WithNoEvents_ReturnsEmptyList()
    {
        var model = BuildModel([]);

        model.OnGet();

        Assert.Empty(model.Events);
    }
}
