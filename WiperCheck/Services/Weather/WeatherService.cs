using System.Numerics;
using WiperCheck.Models.Responses;
using WiperCheck.Models.Utilities;
using WiperCheck.Models.Utilities.Weather;
using WiperCheck.Services.DateTimeProvider;

namespace WiperCheck.Services.Weather;

public class WeatherService(HttpClient httpClient, IConfiguration config, IDateTimeProvider dateTimeProvider) : IWeatherService
{
    public async Task<WeatherResult> GetWeather(GeocodeLocation location, DateTime arrivalUtc)
    {
        var requestUtc = DateTime.UtcNow;
        var response = await httpClient.GetAsync(BuildQueryString(location));
        response.EnsureSuccessStatusCode();
        
        var apiResult = await response.Content.ReadFromJsonAsync<WeatherApiResponse>();

        if (apiResult is null)
        {
            throw new InvalidOperationException("Weather API returned an empty response.");
        }
    
        return MapToWeatherResult(location, apiResult, arrivalUtc, requestUtc);
    }
    
    public async Task<long> GetUtcOffsetAsync(GeocodeLocation location)
    {
        var response = await httpClient.GetAsync(BuildQueryString(location));
        response.EnsureSuccessStatusCode();
        var apiResult = await response.Content.ReadFromJsonAsync<WeatherApiResponse>();
        return apiResult?.UtcOffsetSeconds 
               ?? throw new InvalidOperationException("Could not determine UTC offset for departure location.");
    }

    internal WeatherResult MapToWeatherResult(GeocodeLocation location, WeatherApiResponse apiResult, DateTime arrivalUtc, DateTime requestUtc)
    {
        // The API response's hourly forecast array starts at midnight of the day the HTTP request was made in the requested location's timezone. 
        // We need to convert from Utc to local time to get the correct forecast for the requested arrival time. 
        var requestAtWaypointTime = requestUtc.AddSeconds(apiResult.UtcOffsetSeconds);
        var forecastArrayStart = requestAtWaypointTime.Date; 
        
        var localArrivalTime = arrivalUtc.AddSeconds(apiResult.UtcOffsetSeconds);
        
        var hourIndex = (localArrivalTime - forecastArrayStart).TotalHours;
        
        var hourly = apiResult.Hourly;
        var weatherCode = GetValueAtRoundedIndex(hourly.WeatherCode, hourIndex);

        return new WeatherResult
        {
            // Vitals
            Coordinate = location,
            UtcForecastTime = arrivalUtc,
            TimeZone = apiResult.Timezone ?? string.Empty,
            UtcOffsetSeconds = apiResult.UtcOffsetSeconds,

            // Weather Forecasts
            TemperatureF = (int)Math.Round(GetWeightedAverage(hourly?.Temperature2M, hourIndex)),
            FeelsLikeF = (int)Math.Round(GetWeightedAverage(hourly?.ApparentTemperature, hourIndex)),
            PrecipitationProbability = GetWeightedAverage(hourly?.PrecipitationProbability, hourIndex),
            PrecipitationAmount = GetWeightedAverage(hourly?.Precipitation, hourIndex),
            PrecipitationType = WmoWeatherInterpreter.GetPrecipitationType(weatherCode),
            CloudCoverPercent = (int)Math.Round(GetWeightedAverage(hourly?.CloudCover, hourIndex)),
            VisibilityFt = (int)Math.Round(GetWeightedAverage(hourly?.Visibility, hourIndex)),
            WindSpeedMph = (int)Math.Round(GetWeightedAverage(hourly?.WindSpeed10M, hourIndex)),
            WindDirectionDegrees = (int)Math.Round(GetWeightedAverage(hourly?.WindDirection10M, hourIndex)),
            WindGustsMph = (int)Math.Round(GetWeightedAverage(hourly?.WindGusts10M, hourIndex)),
            Condition = WmoWeatherInterpreter.GetCondition(weatherCode),
            HasExtremeWeatherWarning = WmoWeatherInterpreter.IsExtreme(weatherCode, GetWeightedAverage(hourly?.WindGusts10M, hourIndex)),
            ExtremeWeatherDescription = WmoWeatherInterpreter.GetExtremeDescription(weatherCode, GetWeightedAverage(hourly?.WindGusts10M, hourIndex))
        };
    }

    private static double GetWeightedAverage<T>(T?[]? values, double hourIndex)
        where T : struct, INumber<T>
    {
        if (values is null || values.Length == 0)
        {
            return 0;
        }

        int floorIndex = (int)Math.Floor(hourIndex);
        int ceilIndex = (int)Math.Ceiling(hourIndex);

        if (floorIndex < 0 || ceilIndex >= values.Length)
        {
            return 0;
        }

        var floorValue = values[floorIndex];
        var ceilValue = values[ceilIndex];

        if (floorValue is null || ceilValue is null)
        {
            return 0;
        }

        // Exact hour, no interpolation needed
        if (floorIndex == ceilIndex)
        {
            return double.CreateChecked(floorValue.Value);
        }

        double floorWeight = ceilIndex - hourIndex;
        double ceilWeight = 1 - floorWeight;

        return (double.CreateChecked(floorValue.Value) * floorWeight)
            + (double.CreateChecked(ceilValue.Value) * ceilWeight);
    }

    private static T? GetValueAtRoundedIndex<T>(T?[]? values, double hourIndex)
        where T : struct
    {
        if (values is null || values.Length == 0)
        {
            return null;
        }

        int index = (int)Math.Round(hourIndex);

        if (index < 0 || index >= values.Length)
        {
            return null;
        }

        return values[index];
    }

    private static string BuildQueryString(GeocodeLocation location)
    {
        var latitude = Uri.EscapeDataString(location.Latitude.ToString() ?? string.Empty);
        var longitude = Uri.EscapeDataString(location.Longitude.ToString() ?? string.Empty); 
    
        return $"?latitude={latitude}&longitude={longitude}&hourly=temperature_2m,apparent_temperature,precipitation_probability,precipitation,wind_speed_10m,wind_direction_10m,cloud_cover,visibility,weather_code,wind_gusts_10m&wind_speed_unit=mph&temperature_unit=fahrenheit&precipitation_unit=inch&timezone=auto";
    }

}