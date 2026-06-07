using WiperCheck.Models.Forms;
using WiperCheck.Models.Requests;
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

    public async Task<List<RankedTripResult>> GetTripForecast(TripForecastRequest request)
    {
        var requestUtc = DateTime.UtcNow;

        // Geocode start + end in parallel
        var geoResults = await Task.WhenAll(
            _geocodeService.GetCoordinates(request.StartAddress),
            _geocodeService.GetCoordinates(request.EndAddress));
        var startGeo = geoResults[0];
        var endGeo = geoResults[1];

        // Single route call using the earliest departure
        var route = await _routingService.GetRoute(startGeo, endGeo);

        // One weather API call per waypoint location + reverse geocode, all in parallel
        var forecastTasks = route.Steps.Select(step => _weatherService.GetForecast(step.Location));
        var reverseGeoTasks = route.Steps.Select(step => _geocodeService.GetAddress(step.Location));
        var forecastResponses = await Task.WhenAll(forecastTasks);
        var addresses = await Task.WhenAll(reverseGeoTasks);

        long utcOffsetSeconds = forecastResponses[0].UtcOffsetSeconds;
        var earliestDepartureUtc = request.EarliestDeparture.AddSeconds(-utcOffsetSeconds);

        int departureCount = (int)(request.LatestDeparture - request.EarliestDeparture).TotalHours + 1;

        var variations = new List<RankedTripResult>(departureCount);

        for (int i = 0; i < departureCount; i++)
        {
            var departureLocal = request.EarliestDeparture.AddHours(i);
            var departureUtc = earliestDepartureUtc.AddHours(i);

            var waypoints = route.Steps.Select((step, j) =>
            {
                var arrivalUtc = departureUtc.AddSeconds(step.Duration);
                var weather = _weatherService.MapToWeatherResult(step.Location, forecastResponses[j], arrivalUtc, requestUtc);
                weather = weather with
                {
                    Coordinate = weather.Coordinate with
                    {
                        DisplayName = $"{addresses[j].City}, {addresses[j].State}"
                    }
                };
                return new WaypointForecast { WaypointIndex = j, WeatherResult = weather };
            }).ToList();

            var trip = new TripForecastResult
            {
                TotalDistanceMiles = route.TotalDistanceMiles,
                TotalDurationSeconds = route.TotalTimeSeconds,
                DepartureTime = departureLocal,
                Waypoints = waypoints
            };

            variations.Add(new RankedTripResult
            {
                Trip = trip,
                DepartureTime = departureLocal,
                PenaltyScore = ScoreTrip(trip),
                HasExtremeWarning = waypoints.Any(w => w.WeatherResult.HasExtremeWeatherWarning),
                WorstWaypoint = GetWorstWaypoint(waypoints)
            });
        }

        return variations
            .OrderBy(r => r.PenaltyScore)
            .Select((r, index) => { r.Rank = index + 1; return r; })
            .ToList();
    }

    private static double ScoreWaypoint(WeatherResult w)
    {
        const double PrecipProbWeight = 1.0;
        const double PrecipAmountWeight = 100.0;
        const double GustThresholdMph = 30.0;
        const double GustWeight = 2.0;
        const double VisibilityThreshFt = 26400.0;
        const double VisibilityWeight = 15.0;
        const double ExtremePenalty = 500.0;

        return w.PrecipitationProbability * PrecipProbWeight
            + w.PrecipitationAmount * PrecipAmountWeight
            + Math.Max(0, w.WindGustsMph - GustThresholdMph) * GustWeight
            + Math.Max(0, VisibilityThreshFt - w.VisibilityFt) / 5280.0 * VisibilityWeight
            + (w.HasExtremeWeatherWarning ? ExtremePenalty : 0);
    }

    private static double ScoreTrip(TripForecastResult trip) =>
        trip.Waypoints.Sum(w => ScoreWaypoint(w.WeatherResult));

    private static WaypointForecast GetWorstWaypoint(List<WaypointForecast> waypoints) =>
        waypoints.MaxBy(w => ScoreWaypoint(w.WeatherResult))!;
}
