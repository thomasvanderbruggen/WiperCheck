using WiperCheck.Models.Forms;
using Forward = WiperCheck.Models.Responses.GeocodeAPI;
using WiperCheck.Models.Utilities;

namespace WiperCheck.Services.Geocoding;

public class GeocodeService(HttpClient httpClient, IConfiguration config) : IGeocodeService
{
    private readonly string _apiKey = config["ORS:ApiKey"] ?? throw new InvalidOperationException("ORS API Key is missing from configuration!");

    public async Task<GeocodeLocation> GetCoordinates(Address address)
    {
        var response = await httpClient.GetAsync(BuildQueryString(address, _apiKey));
        response.EnsureSuccessStatusCode();

        var apiResult = await response.Content.ReadFromJsonAsync<Forward.GeocodeApiResponse>();

        var feature = ValidateApiResult(apiResult);
        
        return new GeocodeLocation(feature.Properties.Label, feature.Geometry.Coordinates[1], feature.Geometry.Coordinates[0]);

    }

    public async Task<Address> GetAddress(GeocodeLocation location)
    {
        var response = await httpClient.GetAsync(BuildQueryString(location, _apiKey));
        response.EnsureSuccessStatusCode();
        
        var apiResult = await response.Content.ReadFromJsonAsync<Forward.GeocodeApiResponse>();

        var feature = ValidateApiResult(apiResult);
        var address = new Address(); 
        
        address.Street = feature.Properties.Street;
        address.City = feature.Properties.Locality ?? feature.Properties.County; 
        address.State = feature.Properties.Region; 
        
        return address;

    }
    
    private static Forward.Feature ValidateApiResult(Forward.GeocodeApiResponse? apiResult)
    {
        if (apiResult == null)
        {
            throw new InvalidOperationException("ORS API returned an empty or invalid response.!");
        }

        if (apiResult?.Features == null || apiResult.Features.Length == 0)
        {
            throw new InvalidOperationException("No coordinates found for entered city.");
        }

        // if (apiResult.Features.Length > 1)
        // {
        //     throw new InvalidOperationException("Multiple coordinates found for entered city, please refine your search.");
        // }
        
        var feature = apiResult.Features[0];

        if (feature?.Geometry == null)
        {
            throw new InvalidOperationException("ORS API returned an invalid response: missing Geometry.");
        }

        return feature;
    }


    private static string BuildQueryString(Address address, string apiKey)
    {
        var street = Uri.EscapeDataString(address.Street ?? string.Empty);
        var city = Uri.EscapeDataString(address.City ?? string.Empty);
        var state = Uri.EscapeDataString(address.State ?? string.Empty);
        var zip = Uri.EscapeDataString(address.ZipCode ?? string.Empty);

        var combined = $"search/structured?api_key={apiKey}&address={street}&locality={city}&region={state}&postalcode={zip}";
        return combined;
    }

    private static string BuildQueryString(GeocodeLocation location, string apiKey)
    {
        return $"reverse?api_key={apiKey}&point.lat={location.Latitude}&point.lon={location.Longitude}";
    }
}