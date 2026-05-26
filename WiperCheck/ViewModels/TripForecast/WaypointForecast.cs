using WiperCheck.Models.Utilities.Weather;

namespace WiperCheck.ViewModels.TripForecast;

public class WaypointForecast
{
    public int WaypointIndex { get; set; }
    public WeatherResult WeatherResult { get; set; }
}