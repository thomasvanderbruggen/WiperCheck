using WiperCheck.Models.Responses;
using WiperCheck.Models.Utilities;

namespace WiperCheck.Services.Weather;

public interface IWeatherService
{
    Task<WeatherApiResponse> GetForecast(GeocodeLocation location);
}
