namespace FFBC.Options;

public class FfbcEventStoreOptions
{
    public const string SectionName = "FfbcEventStore";

    public string PostUrl { get; set; } = "https://www.velo-liberte.be/wp-content/themes/zapedah-child/assets/scripts/ajax_request_calendrier.php";
    public string BaseUrl { get; set; } = "https://www.velo-liberte.be/wp-content/themes/zapedah-child";
    public string Province { get; set; } = "Luxembourg";
    public string Challenge { get; set; } = string.Empty;
    public string VttRoute { get; set; } = "vtt/marche";
    public int Year { get; set; } = DateTime.UtcNow.Year;
    public string Date { get; set; } = $"{DateTime.UtcNow.Year}-03-25";
    public int CacheDurationMinutes { get; set; } = 120;
}
