using FFBC.Models;
using FFBC.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FFBC.Pages;

public record CalendarDay(DateTime? Date, IReadOnlyList<Event> Events);

public class CalendarModel : PageModel
{
    private readonly IEventStore _eventStore;

    public CalendarModel(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public int DisplayYear { get; private set; }
    public int DisplayMonth { get; private set; }

    public int PrevYear { get; private set; }
    public int PrevMonth { get; private set; }
    public int NextYear { get; private set; }
    public int NextMonth { get; private set; }

    // Each inner list is one week row (always 7 elements; null Date = padding cell)
    public IReadOnlyList<IReadOnlyList<CalendarDay>> Weeks { get; private set; } = [];

    public void OnGet(int? year, int? month)
    {
        var today = DateTime.Today;
        DisplayYear = year ?? today.Year;
        DisplayMonth = month ?? today.Month;

        // Clamp to valid range
        DisplayMonth = Math.Clamp(DisplayMonth, 1, 12);

        // Previous / next month with year wrap
        var prev = new DateTime(DisplayYear, DisplayMonth, 1).AddMonths(-1);
        PrevYear = prev.Year;
        PrevMonth = prev.Month;

        var next = new DateTime(DisplayYear, DisplayMonth, 1).AddMonths(1);
        NextYear = next.Year;
        NextMonth = next.Month;

        // Build lookup: day-of-month → events
        var eventsInMonth = _eventStore.GetAll()
            .Where(e => e.Date.Year == DisplayYear && e.Date.Month == DisplayMonth)
            .GroupBy(e => e.Date.Day)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<Event>)g.OrderBy(e => e.Date).ToList());

        // Build the week grid
        var firstOfMonth = new DateTime(DisplayYear, DisplayMonth, 1);
        int daysInMonth = DateTime.DaysInMonth(DisplayYear, DisplayMonth);
        int startOffset = (int)firstOfMonth.DayOfWeek; // 0 = Sunday

        var cells = new List<CalendarDay>();

        // Leading padding cells
        for (int i = 0; i < startOffset; i++)
            cells.Add(new CalendarDay(null, []));

        // Actual days
        for (int d = 1; d <= daysInMonth; d++)
        {
            var date = new DateTime(DisplayYear, DisplayMonth, d);
            var dayEvents = eventsInMonth.TryGetValue(d, out var ev) ? ev : [];
            cells.Add(new CalendarDay(date, dayEvents));
        }

        // Trailing padding to complete the last week
        while (cells.Count % 7 != 0)
            cells.Add(new CalendarDay(null, []));

        // Chunk into week rows
        var weeks = new List<IReadOnlyList<CalendarDay>>();
        for (int i = 0; i < cells.Count; i += 7)
            weeks.Add(cells.GetRange(i, 7));

        Weeks = weeks;
    }
}
