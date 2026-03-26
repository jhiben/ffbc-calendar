namespace FFBC.Models;

/// <summary>
/// Holds enriched event details fetched from the FFBC popup API.
/// </summary>
public class EventDetails
{
    /// <summary>
    /// The event ID used to fetch these details.
    /// </summary>
    public string EventId { get; set; } = string.Empty;

    /// <summary>
    /// Raw HTML content from the popup API response.
    /// </summary>
    public string? HtmlContent { get; set; }

    /// <summary>
    /// Registration URL for pre-registration.
    /// </summary>
    public string? RegistrationUrl { get; set; }

    /// <summary>
    /// Start location name (e.g. venue or building name).
    /// </summary>
    public string? StartLocation { get; set; }

    /// <summary>
    /// Full address of the start location.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Google Maps directions URL for the start location.
    /// </summary>
    public string? MapsUrl { get; set; }

    /// <summary>
    /// Organizing club name and number.
    /// </summary>
    public string? Club { get; set; }

    /// <summary>
    /// Available activities/routes for the event.
    /// </summary>
    public IReadOnlyList<Activity> Activities { get; set; } = [];

    /// <summary>
    /// Additional remarks or notes from the organizer.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Event website URL.
    /// </summary>
    public string? Website { get; set; }
}

/// <summary>
/// A single activity/route within an event (e.g. a specific VTT distance).
/// </summary>
public class Activity
{
    /// <summary>
    /// Activity type (e.g. VTT, GRAVEL, MARCHE).
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Distance in kilometers (e.g. "60 Km").
    /// </summary>
    public string? Distance { get; set; }

    /// <summary>
    /// Elevation gain (D+), e.g. "500m" or "N.A".
    /// </summary>
    public string? Elevation { get; set; }

    /// <summary>
    /// Difficulty level, e.g. "N.A" or a numeric rating.
    /// </summary>
    public string? Difficulty { get; set; }

    /// <summary>
    /// Time window for departure (e.g. "07h00 - 10h00").
    /// </summary>
    public string? Time { get; set; }

    /// <summary>
    /// Number of refreshment stops or "N.A".
    /// </summary>
    public string? Ravito { get; set; }

    /// <summary>
    /// Pricing information (e.g. "FFBC : 4,50€ / NON FFBC : 6,00€").
    /// </summary>
    public string? Price { get; set; }
}
