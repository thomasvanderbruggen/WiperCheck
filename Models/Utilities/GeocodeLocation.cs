namespace WiperCheck.Models.Utilities;

public class GeocodeLocation
{
    public string DisplayName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public GeocodeLocation(string displayName, double latitude, double longitude)
    {
        DisplayName = displayName;
        Latitude = latitude;
        Longitude = longitude;
    }
}