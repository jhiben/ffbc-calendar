namespace FFBC.Models;

public class Event
{
    public DateTime Date { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? Town { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
}
