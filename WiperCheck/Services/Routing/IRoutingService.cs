using WiperCheck.Models.Utilities;
using WiperCheck.Models.Utilities.Routing;

namespace WiperCheck.Services.Routing;

public interface IRoutingService
{
    Task<RouteResult> GetRoute(GeocodeLocation start, GeocodeLocation end);
}