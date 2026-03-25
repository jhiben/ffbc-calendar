using FFBC.Models;
using FFBC.Pages;
using FFBC.Services;

namespace FFBC.Tests;

public class CalendarTests
{
    private static CalendarModel BuildModel(IEnumerable<Event> events)
        => new CalendarModel(new InMemoryEventStore(events));

    [Fact]
    public void OnGet_EventAppearsInCorrectWeekRowAndDayCell()
    {
        // April 2026: April 5 is a Sunday (column 0, second row after leading padding)
        var events = new[]
        {
            new Event { Date = new DateTime(2026, 4, 5), Title = "Spring Ride" }
        };
        var model = BuildModel(events);

        model.OnGet(2026, 4);

        // Find the week row containing April 5
        var weekWithEvent = model.Weeks.Single(row => row.Any(d => d.Date?.Day == 5));
        var cell = weekWithEvent.Single(d => d.Date?.Day == 5);

        Assert.Single(cell.Events);
        Assert.Equal("Spring Ride", cell.Events[0].Title);
        // April 5 is a Sunday → index 0 in its row
        Assert.Equal(0, weekWithEvent.ToList().IndexOf(cell));
    }

    [Fact]
    public void OnGet_WithNoParameters_DefaultsToCurrentYearAndMonth()
    {
        var model = BuildModel([]);

        model.OnGet(null, null);

        Assert.Equal(DateTime.Today.Year, model.DisplayYear);
        Assert.Equal(DateTime.Today.Month, model.DisplayMonth);
    }

    [Fact]
    public void OnGet_PreviousAndNextMonthValuesAreCorrect_IncludingYearBoundaryWrap()
    {
        var model = BuildModel([]);

        // January: previous should be December of prior year
        model.OnGet(2026, 1);

        Assert.Equal(2025, model.PrevYear);
        Assert.Equal(12, model.PrevMonth);
        Assert.Equal(2026, model.NextYear);
        Assert.Equal(2, model.NextMonth);

        // December: next should be January of next year
        model.OnGet(2026, 12);

        Assert.Equal(2026, model.PrevYear);
        Assert.Equal(11, model.PrevMonth);
        Assert.Equal(2027, model.NextYear);
        Assert.Equal(1, model.NextMonth);
    }
}
