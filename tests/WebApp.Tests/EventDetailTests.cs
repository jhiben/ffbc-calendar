using FFBC.Models;
using FFBC.Pages;
using FFBC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FFBC.Tests;

public class EventDetailTests
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
            Challenge = "Brevet à Dénivelé",
            Notes = "Namur - 5190 Spy | Brevet à Dénivelé"
        },
        new Event
        {
            Date = new DateTime(2026, 4, 25),
            Title = "La Florancy",
            Town = "MESSANCY",
            PostalCode = "6780",
            Country = "Belgium",
            Province = "Luxembourg"
        }
    ];

    private static EventDetailModel BuildModel(IEnumerable<Event>? events = null)
    {
        var store = new InMemoryEventStore(events ?? SeedEvents);
        var eventDetailsService = new NullEventDetailsService();
        return new EventDetailModel(store, eventDetailsService);
    }

    [Fact]
    public async Task OnGetAsync_WithValidDateAndTitle_ReturnsPageWithEvent()
    {
        var model = BuildModel();

        var result = await model.OnGetAsync("2026-03-25", "Rallye des Spirous");

        Assert.IsType<PageResult>(result);
        Assert.NotNull(model.EventItem);
        Assert.Equal("Rallye des Spirous", model.EventItem!.Title);
        Assert.False(model.EventNotFound);
    }

    [Fact]
    public async Task OnGetAsync_WithNonExistentEvent_SetsEventNotFound()
    {
        var model = BuildModel();

        var result = await model.OnGetAsync("2026-03-25", "Nonexistent Ride");

        Assert.IsType<PageResult>(result);
        Assert.Null(model.EventItem);
        Assert.True(model.EventNotFound);
    }

    [Fact]
    public async Task OnGetAsync_WithMissingDate_RedirectsToListEvents()
    {
        var model = BuildModel();

        var result = await model.OnGetAsync(null, "Rallye des Spirous");

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/ListEvents", redirect.PageName);
    }

    [Fact]
    public async Task OnGetAsync_WithMissingTitle_RedirectsToListEvents()
    {
        var model = BuildModel();

        var result = await model.OnGetAsync("2026-03-25", null);

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/ListEvents", redirect.PageName);
    }

    [Fact]
    public async Task OnGetAsync_WithInvalidDateFormat_RedirectsToListEvents()
    {
        var model = BuildModel();

        var result = await model.OnGetAsync("not-a-date", "Rallye des Spirous");

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/ListEvents", redirect.PageName);
    }

    [Fact]
    public async Task OnGetAsync_TitleMatchIsCaseInsensitive()
    {
        var model = BuildModel();

        var result = await model.OnGetAsync("2026-03-25", "rallye des spirous");

        Assert.IsType<PageResult>(result);
        Assert.NotNull(model.EventItem);
        Assert.Equal("Rallye des Spirous", model.EventItem!.Title);
    }

    /// <summary>
    /// Test implementation of IEventDetailsService that returns null.
    /// </summary>
    private sealed class NullEventDetailsService : IEventDetailsService
    {
        public Task<EventDetails?> GetEventDetailsAsync(string eventId) => Task.FromResult<EventDetails?>(null);
    }
}
