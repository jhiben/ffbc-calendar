using FFBC.Models;
using FFBC.Services;

namespace FFBC.Tests;

internal sealed class StubEventStore(IReadOnlyList<Event> events) : IEventStore
{
    public IReadOnlyList<Event> GetAll() => events;

    public Event? GetByDateAndTitle(DateTime date, string title) =>
        events.FirstOrDefault(e => e.Date == date && e.Title == title);
}

internal sealed class StubEventDetailsService(EventDetails? details) : IEventDetailsService
{
    public Task<EventDetails?> GetEventDetailsAsync(string eventId) =>
        Task.FromResult(details);
}

internal sealed class StubGeocodingService((double Latitude, double Longitude)? coords) : IGeocodingService
{
    public Task<(double Latitude, double Longitude)?> GeocodeAsync(string postalCode, string? town, string? country) =>
        Task.FromResult(coords);
}
