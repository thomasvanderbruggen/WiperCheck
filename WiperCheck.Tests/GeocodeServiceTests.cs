using Microsoft.Extensions.Configuration;
using WiperCheck.Models.Forms;
using WiperCheck.Services.Geocoding;

namespace WiperCheck.Tests;

public class GeocodeServiceTests
{
    [Fact]
    public async Task GetCoordinates_WithValidAddress_ReturnsCorrectCoordinates()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<GeocodeServiceTests>()
            .Build();
        
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://api.openrouteservice.org/geocode/search/structured");

        var service = new GeocodeService(httpClient, config);

        var testAddress = new Address
        {
            Street = "1600 Pennsylvania Avenue NW",
            City = "Washington",
            State = "DC",
            ZipCode = "20500"
        };
        var result = await service.GetCoordinates(testAddress);

        // Assert that the result is not null and contains the expected city name
        Assert.NotNull(result);
        Assert.Contains("Washington", result.DisplayName);
        
        // Assert that the result has the expected latitude and longitude, within 0.01 degrees
        Assert.Equal(38.89, result.Latitude, 0.01);  
        Assert.Equal(-77.03, result.Longitude, 0.01);
    }
}