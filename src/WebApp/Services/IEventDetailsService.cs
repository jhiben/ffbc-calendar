using FFBC.Models;

namespace FFBC.Services;

/// <summary>
/// Service for fetching enriched event details from the FFBC popup API.
/// </summary>
public interface IEventDetailsService
{
    /// <summary>
    /// Fetches additional event details by event ID.
    /// </summary>
    /// <param name="eventId">The event ID to fetch details for.</param>
    /// <returns>Event details if successfully fetched, null otherwise.</returns>
    Task<EventDetails?> GetEventDetailsAsync(string eventId);
}
