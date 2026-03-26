using System.Globalization;
using FFBC.Models;
using FFBC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FFBC.Pages;

public class EventDetailModel : PageModel
{
    private readonly IEventStore _eventStore;
    private readonly IEventDetailsService _eventDetailsService;

    public EventDetailModel(IEventStore eventStore, IEventDetailsService eventDetailsService)
    {
        _eventStore = eventStore;
        _eventDetailsService = eventDetailsService;
    }

    public Event? EventItem { get; private set; }
    public EventDetails? EnrichedDetails { get; private set; }
    public bool EventNotFound { get; private set; }

    public async Task<IActionResult> OnGetAsync(string? date, string? title)
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

        // Fetch enriched details if event has an ID
        if (EventItem?.EventId is not null)
        {
            EnrichedDetails = await _eventDetailsService.GetEventDetailsAsync(EventItem.EventId);
        }

        return Page();
    }
}
