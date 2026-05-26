namespace WiperCheck.Models.Utilities.Routing;

public class RouteStep
{
    public double Duration { get; set; }
    public GeocodeLocation Location { get; set; }
    
    public RouteStep(double duration, GeocodeLocation location)
    {
        Duration = duration;
        Location = location;
    } 
}