namespace WiperCheck.Models.Utilities.Weather;
public static class WmoWeatherInterpreter
{
    public static WeatherCondition GetCondition(int? weatherCode) => weatherCode switch
    {
        0 => WeatherCondition.Clear,
        1 or 2 => WeatherCondition.PartlyCloudy,
        3 => WeatherCondition.Overcast,
        45 or 48 => WeatherCondition.Foggy,
        51 or 53 or 55 => WeatherCondition.Drizzle,
        61 or 63 or 65 => WeatherCondition.Rain,
        66 or 67 => WeatherCondition.Sleet,
        71 or 73 or 75 or 77 => WeatherCondition.Snow,
        80 or 81 or 82 => WeatherCondition.Rain,
        85 or 86 => WeatherCondition.Snow,
        95 => WeatherCondition.Thunderstorm,
        96 or 99 => WeatherCondition.Hail,
        _ => WeatherCondition.Clear
    };

    public static PrecipitationType GetPrecipitationType(int? weatherCode) => weatherCode switch
    {
        51 or 53 or 55 or 61 or 63 or 65 or 80 or 81 or 82 => PrecipitationType.Rain,
        56 or 57 or 66 or 67 => PrecipitationType.Sleet,
        71 or 73 or 75 or 77 or 85 or 86 => PrecipitationType.Snow,
        95 => PrecipitationType.Thunderstorm,
        96 or 99 => PrecipitationType.Hail,
        _ => PrecipitationType.None
    };

    public static bool IsExtreme(int? weatherCode, double? windGustsMph) =>
        weatherCode is 95 or 96 or 99 || windGustsMph >= 58;

    public static string? GetExtremeDescription(int? weatherCode, double windGustsMph)
    {
        if (weatherCode == 96 || weatherCode == 99) return "Hail expected";
        if (weatherCode == 95) return "Thunderstorm warning";
        if (windGustsMph >= 58) return $"Dangerous wind gusts: {windGustsMph:F0} mph";
        return null;
    }
}