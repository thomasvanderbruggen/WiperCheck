using WiperCheck.Models.Forms;
using WiperCheck.Models.Utilities;

namespace WiperCheck.Services.Geocoding;

public class GeocodeService : IGeocodeService
{
    private readonly HttpClient _httpClient;

    public GeocodeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GeocodeLocation> GetCoordinates(Address address)
    {
        var response = await _httpClient.GetAsync($""); 
        return new GeocodeLocation("Default", 0, 0); 
    }
}