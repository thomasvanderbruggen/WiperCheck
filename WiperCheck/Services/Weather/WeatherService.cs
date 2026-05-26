using System.Numerics;
using Microsoft.Extensions.Options;
using WiperCheck.Models.Forms;
using WiperCheck.Models.Responses;
using WiperCheck.Models.Utilities;
using WiperCheck.Models.Utilities.Weather;
using WiperCheck.Services.DateTime;

namespace WiperCheck.Services.Weather;

public class WeatherService(HttpClient httpClient, IConfiguration config, IDateTimeProvider dateTimeProvider) : IWeatherService
{
    public async Task<WeatherResult> GetWeather(GeocodeLocation location, System.DateTime arrivalTime)
    {
        var response = await httpClient.GetAsync(BuildQueryString(location));
        response.EnsureSuccessStatusCode();
        
        var apiResult = await response.Content.ReadFromJsonAsync<WeatherApiResponse>();

        if (apiResult is null)
        {
            throw new InvalidOperationException("Weather API returned an empty response.");
        }
    
        return MapToWeatherResult(location, apiResult, arrivalTime);
    }

    internal WeatherResult MapToWeatherResult(GeocodeLocation location, WeatherApiResponse apiResult, System.DateTime arrivalTime)
    {
        var hourIndex = (arrivalTime - dateTimeProvider.Today).TotalHours;
        var hourly = apiResult.Hourly;
        var weatherCode = GetValueAtRoundedIndex(hourly.WeatherCode, hourIndex);

        return new WeatherResult
        {
            // Vitals
            Coordinate = location,
            UtcForecastTime = arrivalTime,
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