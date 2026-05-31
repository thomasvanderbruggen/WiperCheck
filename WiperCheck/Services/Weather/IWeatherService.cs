using WiperCheck.Models.Utilities;
using WiperCheck.Models.Utilities.Weather;

namespace WiperCheck.Services.Weather;

public interface IWeatherService
{
    Task<WeatherResult> GetWeather(GeocodeLocation location, DateTime arrivalTime);
}