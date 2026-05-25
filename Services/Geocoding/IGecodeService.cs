using WiperCheck.Models.Utilities;
using WiperCheck.Models.Forms;

namespace WiperCheck.Services.Geocoding;

public interface IGeocodeService
{
    Task<GeocodeLocation> GetCoordinates(Address address);
}