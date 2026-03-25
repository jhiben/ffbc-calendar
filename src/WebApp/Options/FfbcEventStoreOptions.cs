namespace FFBC.Options;

public class FfbcEventStoreOptions
{
    public const string SectionName = "FfbcEventStore";

    public string PostUrl { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string Challenge { get; set; } = string.Empty;
    public string VttRoute { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Date { get; set; } = string.Empty;
    public int CacheDurationMinutes { get; set; } = 120;
}