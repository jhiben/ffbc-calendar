using System.Text.Json;
using System.Text.Json.Serialization;
using FFBC.Models;
using FFBC.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FFBC.Pages;

public class MapModel : PageModel
{
    private readonly IEventStore _eventStore;
    private readonly IGeocodingService _geocodingService;

    public MapModel(IEventStore eventStore, IGeocodingService geocodingService)
    {
        _eventStore = eventStore;
        _geocodingService = geocodingService;
    }

    public string MarkersJson { get; private set; } = "[]";
    public IReadOnlyList<Event> UnmappedEvents { get; private set; } = [];
    public bool HasEvents { get; private set; }

    public async Task OnGetAsync()
    {
        var events = _eventStore.GetAll();
        HasEvents = events.Count > 0;

        var markers = new List<MarkerData>();
        var unmapped = new List<Event>();

        foreach (var ev in events)
        {
            if (string.IsNullOrWhiteSpace(ev.PostalCode))
            {
                unmapped.Add(ev);
                continue;
            }

            var coords = await _geocodingService.GeocodeAsync(ev.PostalCode, ev.Town, ev.Country);
            if (coords is null)
            {
                unmapped.Add(ev);
                continue;
            }

            markers.Add(new MarkerData
            {
                Lat = coords.Value.Latitude,
                Lng = coords.Value.Longitude,
                Title = ev.Title,
                Date = ev.Date.ToString("ddd, MMM d yyyy"),
                Town = ev.Town ?? string.Empty
            });
        }

        MarkersJson = JsonSerializer.Serialize(markers, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        UnmappedEvents = unmapped;
    }

    public class MarkerData
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Town { get; set; } = string.Empty;
    }
}
