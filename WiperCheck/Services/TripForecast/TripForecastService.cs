using WiperCheck.Models.Forms;
using WiperCheck.Models.Requests;
using WiperCheck.Models.Responses;
using WiperCheck.Models.Utilities.Routing;
using WiperCheck.Models.Utilities.Weather;
using WiperCheck.Services.Geocoding;
using WiperCheck.Services.Routing;
using WiperCheck.Services.Weather;
using WiperCheck.ViewModels.TripForecast;

namespace WiperCheck.Services.TripForecast;

public class TripForecastService
{
    private readonly GeocodeService _geocodeService;
    private readonly RoutingService _routingService;
    private readonly WeatherService _weatherService;
    public TripForecastService(
        GeocodeService geocodeService,
        RoutingService routingService,
        WeatherService weatherService)
    {
        _geocodeService = geocodeService;
        _routingService = routingService;
        _weatherService = weatherService;
    }

    public async Task<TripForecastResult> GetTripForecast(TripForecastRequest request)
    {
        var start =  _geocodeService.GetCoordinates(request.StartAddress);
        var end = _geocodeService.GetCoordinates(request.EndAddress);
        
        var geoResults = await Task.WhenAll(start, end);

        var startGeo = geoResults[0];
        var endGeo = geoResults[1];
        var route = await _routingService.GetRoute(startGeo, endGeo);

        var weatherTasks = route.Steps.Select<RouteStep, Task<WeatherResult>>(step => _weatherService.GetWeather(step.Location, request.DepartureTime.AddSeconds(step.Duration)));
        var reverseGeoTasks = route.Steps.Select<RouteStep, Task<Address>>(step => _geocodeService.GetAddress(step.Location));
        
        var weatherResults = await Task.WhenAll(weatherTasks);
        var addresses = await Task.WhenAll(reverseGeoTasks);
        
        
        
        var tfr = new TripForecastResult
        {
            TotalDistanceMiles = route.TotalDistanceMiles, TotalDurationSeconds = route.TotalTimeSeconds, 
            Waypoints = route.Steps.Select((waypoint, index) =>
            {
                var weather = weatherResults[index];
                weather.Coordinate = weather.Coordinate with
                {
                    DisplayName = $"{addresses[index].City}, {addresses[index].State}"
                };
                return new WaypointForecast
                {
                    WaypointIndex = index,
                    WeatherResult = weather
                };
            }).ToList()
        };

        return tfr;
    }
}