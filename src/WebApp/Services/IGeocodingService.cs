namespace FFBC.Services;

public interface IGeocodingService
{
    Task<(double Latitude, double Longitude)?> GeocodeAsync(string postalCode, string? town);
}
