using FFBC.Models;
using FFBC.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FFBC.Pages;

public class ListEventsModel : PageModel
{
    private readonly IEventStore _eventStore;

    public ListEventsModel(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public IEnumerable<Event> Events { get; private set; } = [];

    public void OnGet()
    {
        Events = _eventStore.GetAll().OrderBy(e => e.Date);
    }
}
