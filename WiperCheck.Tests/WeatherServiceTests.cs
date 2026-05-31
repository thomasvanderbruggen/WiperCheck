using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Moq;
using WiperCheck.Models.Responses;
using WiperCheck.Models.Utilities;
using WiperCheck.Models.Utilities.Weather;
using WiperCheck.Services.DateTimeProvider;
using WiperCheck.Services.Weather;

namespace WiperCheck.Tests;

public class WeatherServiceTests
{
    private readonly WeatherApiResponse _apiResponse;
    public WeatherServiceTests()
    {
        _apiResponse = JsonSerializer.Deserialize<WeatherApiResponse>(File.ReadAllText("Fixtures/ChicagoWeatherReport.json")) ?? throw new Exception("Failed to deserialize fixture");
    }
    // [Fact]
    // public void GetWeather_ReturnsCorrectWeather()
    // {
    //    
    //     var mockDateProvider = new Mock<IDateTimeProvider>();
    //     mockDateProvider.Setup(p => p.Today).Returns(new DateTime(2026, 5, 26));
    //
    //     var service = new WeatherService(new HttpClient(), new ConfigurationBuilder().Build(), mockDateProvider.Object);
    //     
    //     var wr = service.MapToWeatherResult(new GeocodeLocation("Chicago", 41.85, -87.65), _apiResponse, new DateTime(2026, 5, 26, 15, 0, 0), new DateTime(2026, 5, 26));
    //     
    //     Assert.Equal(15, wr.UtcForecastTime.Hour);
    //     Assert.Equal(WeatherCondition.Overcast, wr.Condition);
    //     Assert.Equal(73, wr.FeelsLikeF);
    //     Assert.Equal("Chicago", wr.Coordinate.DisplayName);
    // }
}