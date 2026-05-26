namespace WiperCheck.ViewModels.TripForecast;

public class TripForecastResult
{
    public double TotalDistanceMiles { get; set; }
    public double TotalDurationSeconds { get; set; }
    public List<WaypointForecast> Waypoints { get; set; }
}