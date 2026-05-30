namespace WiperCheck.Models.Utilities;

public record GeocodeLocation
{
    public string DisplayName { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public GeocodeLocation(string displayName, double latitude, double longitude)
    {
        DisplayName = displayName;
        Latitude = latitude;
        Longitude = longitude;
    }

    public GeocodeLocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}