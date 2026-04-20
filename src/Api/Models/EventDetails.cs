namespace FFBC.Models;

/// <summary>
/// Holds enriched event details fetched from the FFBC popup API.
/// </summary>
public class EventDetails
{
    public string EventId { get; set; } = string.Empty;
    public string? HtmlContent { get; set; }
    public string? RegistrationUrl { get; set; }
    public string? StartLocation { get; set; }
    public string? Address { get; set; }
    public string? MapsUrl { get; set; }
    public string? Club { get; set; }
    public IReadOnlyList<Activity> Activities { get; set; } = [];
    public string? Notes { get; set; }
    public string? Website { get; set; }
}

/// <summary>
/// A single activity/route within an event.
/// </summary>
public class Activity
{
    public string Type { get; set; } = string.Empty;
    public string? Distance { get; set; }
    public string? Elevation { get; set; }
    public string? Difficulty { get; set; }
    public string? Time { get; set; }
    public string? Ravito { get; set; }
    public string? Price { get; set; }
}
