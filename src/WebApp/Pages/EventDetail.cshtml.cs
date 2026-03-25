using System.Globalization;
using FFBC.Models;
using FFBC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FFBC.Pages;

public class EventDetailModel : PageModel
{
    private readonly IEventStore _eventStore;

    public EventDetailModel(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public Event? EventItem { get; private set; }
    public bool EventNotFound { get; private set; }

    public IActionResult OnGet(string? date, string? title)
    {
        if (string.IsNullOrWhiteSpace(date) || string.IsNullOrWhiteSpace(title))
        {
            return RedirectToPage("/ListEvents");
        }

        if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
        {
            return RedirectToPage("/ListEvents");
        }

        EventItem = _eventStore.GetByDateAndTitle(parsedDate, title);
        EventNotFound = EventItem is null;

        return Page();
    }
}
