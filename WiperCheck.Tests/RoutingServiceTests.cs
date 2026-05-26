using System.Text.Json;
using WiperCheck.Models.Responses;
using WiperCheck.Services.Routing;

namespace WiperCheck.Tests;

public class RoutingServiceTests
{
    private readonly RoutingApiResponse _apiResponse;
    
    public RoutingServiceTests()
    {
        _apiResponse = JsonSerializer.Deserialize<RoutingApiResponse>(File.ReadAllText("Fixtures/WhiteHouseToIndependenceHall.json")) ?? throw new Exception("Failed to deserialize fixture");
    }

    [Fact]
    public async Task MapToRouteResult_ValidOrsResponse_MapsPolylineCorrectly()
    {
        var result = RoutingService.MapToRouteResult(_apiResponse);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Polyline);
        Assert.Equal(38.8977, result.Polyline[0].Latitude, .01);
    }
    [Fact] 
    public async Task MapToResult_AddsWaypointsCorrectly()
    {
        var result = RoutingService.MapToRouteResult(_apiResponse);
        
        Assert.Equal(5, result.Steps.Count);
    }
}