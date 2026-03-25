using FFBC.Models;
using FFBC.Pages;
using FFBC.Services;

namespace FFBC.Tests;

public class MapTests
{
    [Fact]
    public async Task OnGetAsync_WithGeocodableEvents_PopulatesMarkersJson()
    {
        var events = new[]
        {
            new Event { Date = new DateTime(2026, 5, 1), Title = "Test Ride", PostalCode = "5000", Town = "Namur" }
        };
        var model = BuildModel(events, new StubGeocodingService((50.4669, 4.8719)));

        await model.OnGetAsync();

        Assert.True(model.HasEvents);
        Assert.Contains("Test Ride", model.MarkersJson);
        Assert.Contains("50.4669", model.MarkersJson);
        Assert.Empty(model.UnmappedEvents);
    }

    [Fact]
    public async Task OnGetAsync_WithNoPostalCode_AddsToUnmappedEvents()
    {
        var events = new[]
        {
            new Event { Date = new DateTime(2026, 5, 1), Title = "No Location Ride" }
        };
        var model = BuildModel(events, new StubGeocodingService(null));

        await model.OnGetAsync();

        Assert.True(model.HasEvents);
        Assert.Equal("[]", model.MarkersJson);
        Assert.Single(model.UnmappedEvents);
        Assert.Equal("No Location Ride", model.UnmappedEvents[0].Title);
    }

    [Fact]
    public async Task OnGetAsync_WithGeocodingFailure_AddsToUnmappedEvents()
    {
        var events = new[]
        {
            new Event { Date = new DateTime(2026, 5, 1), Title = "Failed Geocode", PostalCode = "9999", Town = "Unknown" }
        };
        var model = BuildModel(events, new StubGeocodingService(null));

        await model.OnGetAsync();

        Assert.True(model.HasEvents);
        Assert.Equal("[]", model.MarkersJson);
        Assert.Single(model.UnmappedEvents);
    }

    [Fact]
    public async Task OnGetAsync_WithNoEvents_SetsHasEventsFalse()
    {
        var model = BuildModel([], new StubGeocodingService(null));

        await model.OnGetAsync();

        Assert.False(model.HasEvents);
        Assert.Equal("[]", model.MarkersJson);
        Assert.Empty(model.UnmappedEvents);
    }

    [Fact]
    public async Task OnGetAsync_MixedEvents_SeparatesMappedAndUnmapped()
    {
        var events = new[]
        {
            new Event { Date = new DateTime(2026, 5, 1), Title = "Mapped", PostalCode = "5000", Town = "Namur" },
            new Event { Date = new DateTime(2026, 6, 1), Title = "Unmapped" }
        };
        var geocoder = new StubGeocodingService((50.4669, 4.8719));
        var model = BuildModel(events, geocoder);

        await model.OnGetAsync();

        Assert.True(model.HasEvents);
        Assert.Contains("Mapped", model.MarkersJson);
        Assert.DoesNotContain("Unmapped", model.MarkersJson);
        Assert.Single(model.UnmappedEvents);
        Assert.Equal("Unmapped", model.UnmappedEvents[0].Title);
    }

    private static MapModel BuildModel(IEnumerable<Event> events, IGeocodingService geocodingService)
    {
        var store = new InMemoryEventStore(events);
        return new MapModel(store, geocodingService);
    }

    private sealed class StubGeocodingService : IGeocodingService
    {
        private readonly (double Lat, double Lng)? _result;

        public StubGeocodingService((double, double)? result)
        {
            _result = result;
        }

        public Task<(double Latitude, double Longitude)?> GeocodeAsync(string postalCode, string? town)
        {
            return Task.FromResult(_result);
        }
    }
}
