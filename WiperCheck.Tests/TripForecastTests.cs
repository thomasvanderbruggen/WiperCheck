using Microsoft.Extensions.Configuration;
using Moq;
using WiperCheck.Models.Forms;
using WiperCheck.Models.Requests;
using WiperCheck.Services.DateTimeProvider;
using WiperCheck.Services.Geocoding;
using WiperCheck.Services.Routing;
using WiperCheck.Services.TripForecast;
using WiperCheck.Services.Weather;

namespace WiperCheck.Tests;

public class TripForecastTests
{
    [Fact]
    public async Task GetForecastAsync_ValidRequest_ReturnsTripForecast()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<TripForecastTests>()
            .Build(); 
        
        var mockDateProvider = new Mock<IDateTimeProvider>();
        mockDateProvider.Setup(p => p.Today).Returns(DateTime.Today);
        
        var geocodeHttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.openrouteservice.org/geocode/")
        };

        var routingHttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.openrouteservice.org/")
        };
        routingHttpClient.DefaultRequestHeaders.Add("Authorization", configuration["ORS:ApiKey"]);
        
        var weatherHttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.open-meteo.com/v1/forecast/")
        };
        
        var geocodeService = new GeocodeService(geocodeHttpClient, configuration);
        var routingService = new RoutingService(routingHttpClient, configuration);
        var weatherService = new WeatherService(weatherHttpClient, configuration, mockDateProvider.Object);
        
        var tripForecastService = new TripForecastService(geocodeService, routingService, weatherService);
        
        // Arrange
        var request = new TripForecastRequest
        {
            StartAddress = new Address
            {
                Street = "400 Robert D. Ray Dr",
                City = "Des Moines",
                State = "IA",
                ZipCode = "50309"
            },
            EndAddress = new Address
            {
                Street = "121 N LaSalle St",
                City = "Chicago",
                State = "IL",
                ZipCode = "60602"
            },
            EarliestDeparture = DateTime.Today.AddHours(8),
            LatestDeparture = DateTime.Today.AddHours(10)
        };

        // Act
        var results = await tripForecastService.GetTripForecast(request);

        // Assert
        Assert.NotNull(results);
        Assert.Equal(3, results.Count); // 8 AM, 9 AM, 10 AM
        Assert.Equal(1, results[0].Rank);
        var best = results[0].Trip;
        Assert.NotEmpty(best.Waypoints);
        Assert.All(best.Waypoints, w => Assert.NotNull(w.WeatherResult));
        Assert.True(best.TotalDistanceMiles > 0);
    }
}