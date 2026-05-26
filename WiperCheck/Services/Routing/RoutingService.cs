using WiperCheck.Models.Forms;
using WiperCheck.Models.Requests;
using WiperCheck.Models.Responses;
using WiperCheck.Models.Utilities;
using WiperCheck.Models.Utilities.Routing;

namespace WiperCheck.Services.Routing;

public class RoutingService(HttpClient httpClient, IConfiguration config) : IRoutingService
{
    private readonly string _apiKey = config["ORS:ApiKey"] ?? throw new InvalidOperationException("ORS API Key is missing from configuration!");

    public async Task<RouteResult> GetRoute(GeocodeLocation start, GeocodeLocation end)
    {
        var requestBody = new DirectionsRequest
        {
            Coordinates = new List<List<double>>
            {
                new List<double> { start.Longitude, start.Latitude },
                new List<double> { end.Longitude, end.Latitude }
            },
            Units = "mi"
        };
        var response = await httpClient.PostAsJsonAsync("v2/directions/driving-car", requestBody);
        response.EnsureSuccessStatusCode();
        
        var apiResult = await response.Content
                            .ReadFromJsonAsync<RoutingApiResponse>()
                        ?? throw new InvalidOperationException("ORS returned an empty routing response.");
        
        
        return MapToRouteResult(apiResult);
    }

    internal static RouteResult MapToRouteResult(RoutingApiResponse apiResult)
    {
        var polylineDecoded = PolylineDecoder.Decode(apiResult.Routes[0].Geometry);

        var totalDistTravelled = 0.0; 
        var currWayPointDist = 0.0; // Every 3600 seconds, we need to create a waypoint

        // Create waypoints' list and populate with waypoint for starting location
        var waypoints = new List<RouteStep>();
        waypoints.Add(new RouteStep(0, polylineDecoded[0]));
        
        foreach (var step in apiResult.Routes[0].Segments[0].Steps)
        {
            currWayPointDist += step.Duration; 
            totalDistTravelled += step.Duration;

            while (currWayPointDist >= 3600)
            {
                var remainder = currWayPointDist - 3600; 
                
                // Divide by remainder and invert percentage to see how far along we are in the current segment's waypoint
                var percentIntoWaypoint = 1 - (remainder / totalDistTravelled );
                int polylineIndex = (int) Math.Round(step.WayPoints[1] * percentIntoWaypoint);

                waypoints.Add(new RouteStep(totalDistTravelled - remainder, polylineDecoded[polylineIndex]));
                currWayPointDist = remainder;
            }
        }
        // Add waypoint for ending location
        waypoints.Add(new RouteStep(totalDistTravelled, polylineDecoded[polylineDecoded.Count - 1]));
        
        return new RouteResult(
            polylineDecoded, waypoints, apiResult.Routes[0].Summary.Distance, apiResult.Routes[0].Summary.Duration 
            );
    }
}