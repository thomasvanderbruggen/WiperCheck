namespace WiperCheck.Models.Utilities.Routing;

public class RouteResult(
    List<GeocodeLocation> polyline,
    List<RouteStep> steps,
    double totalDistanceMiles,
    double totalTimeMinutes)
{
    public List<GeocodeLocation> Polyline { get; set; } = polyline;
    public List<RouteStep> Steps { get; set; } = steps;
    public double TotalDistanceMiles { get; set; } = totalDistanceMiles;
    public double TotalTimeSeconds { get; set; } = totalTimeMinutes;
}