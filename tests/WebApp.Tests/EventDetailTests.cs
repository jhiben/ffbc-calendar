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
        return new EventDetailModel(store);
    }

    [Fact]
    public void OnGet_WithValidDateAndTitle_ReturnsPageWithEvent()
    {
        var model = BuildModel();

        var result = model.OnGet("2026-03-25", "Rallye des Spirous");

        Assert.IsType<PageResult>(result);
        Assert.NotNull(model.EventItem);
        Assert.Equal("Rallye des Spirous", model.EventItem!.Title);
        Assert.False(model.EventNotFound);
    }

    [Fact]
    public void OnGet_WithNonExistentEvent_SetsEventNotFound()
    {
        var model = BuildModel();

        var result = model.OnGet("2026-03-25", "Nonexistent Ride");

        Assert.IsType<PageResult>(result);
        Assert.Null(model.EventItem);
        Assert.True(model.EventNotFound);
    }

    [Fact]
    public void OnGet_WithMissingDate_RedirectsToListEvents()
    {
        var model = BuildModel();

        var result = model.OnGet(null, "Rallye des Spirous");

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/ListEvents", redirect.PageName);
    }

    [Fact]
    public void OnGet_WithMissingTitle_RedirectsToListEvents()
    {
        var model = BuildModel();

        var result = model.OnGet("2026-03-25", null);

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/ListEvents", redirect.PageName);
    }

    [Fact]
    public void OnGet_WithInvalidDateFormat_RedirectsToListEvents()
    {
        var model = BuildModel();

        var result = model.OnGet("not-a-date", "Rallye des Spirous");

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/ListEvents", redirect.PageName);
    }

    [Fact]
    public void OnGet_TitleMatchIsCaseInsensitive()
    {
        var model = BuildModel();

        var result = model.OnGet("2026-03-25", "rallye des spirous");

        Assert.IsType<PageResult>(result);
        Assert.NotNull(model.EventItem);
        Assert.Equal("Rallye des Spirous", model.EventItem!.Title);
    }
}
