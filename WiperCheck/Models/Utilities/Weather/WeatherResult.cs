namespace WiperCheck.Models.Utilities.Weather;

public record WeatherResult
{
    public GeocodeLocation Coordinate { get; init; }
    public DateTime UtcForecastTime { get; init; }
    public string TimeZone { get; init; }
    public long UtcOffsetSeconds { get; init; }
    
    public int TemperatureF { get; init; }
    public int FeelsLikeF { get; init; }
    
    public double PrecipitationProbability { get; init; }
    public double PrecipitationAmount { get; init; }
    public PrecipitationType PrecipitationType { get; init; }
    
    public int CloudCoverPercent { get; init; }
    public int VisibilityFt { get; init; }
    public int WindSpeedMph { get; init; }
    public int WindDirectionDegrees { get; init; }
    public int WindGustsMph { get; init; }
    
    public WeatherCondition Condition { get; init; }
    public bool HasExtremeWeatherWarning { get; init; }
    public string? ExtremeWeatherDescription { get; init; }
}

public enum PrecipitationType
{
    None,
    Rain,
    Drizzle,
    Sleet,
    Snow,
    Hail,
    Thunderstorm
}

public enum WeatherCondition
{
    Clear,
    PartlyCloudy,
    Overcast,
    Foggy,
    Drizzle,
    Rain,
    Snow,
    Sleet,
    Thunderstorm,
    Hail
}