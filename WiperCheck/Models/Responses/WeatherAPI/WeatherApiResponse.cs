namespace WiperCheck.Models.Responses
{
    using System.Text.Json.Serialization;

    public class WeatherApiResponse
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("generationtime_ms")]
        public double GenerationtimeMs { get; set; }

        [JsonPropertyName("utc_offset_seconds")]
        public long UtcOffsetSeconds { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("timezone_abbreviation")]
        public string TimezoneAbbreviation { get; set; }

        [JsonPropertyName("elevation")]
        public double Elevation { get; set; }

        [JsonPropertyName("hourly_units")]
        public HourlyUnits HourlyUnits { get; set; }

        [JsonPropertyName("hourly")]
        public Hourly Hourly { get; set; }
    }

    public class Hourly
    {
        [JsonPropertyName("time")]
        public DateTime?[] Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public double?[] Temperature2M { get; set; }

        [JsonPropertyName("apparent_temperature")]
        public double?[] ApparentTemperature { get; set; }

        [JsonPropertyName("precipitation_probability")]
        public int?[] PrecipitationProbability { get; set; }

        [JsonPropertyName("precipitation")]
        public double?[] Precipitation { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public double?[] WindSpeed10M { get; set; }

        [JsonPropertyName("wind_direction_10m")]
        public int?[] WindDirection10M { get; set; }
        
        [JsonPropertyName("wind_gusts_10m")]
        public double?[] WindGusts10M { get; set; }

        [JsonPropertyName("cloud_cover")]
        public int?[] CloudCover { get; set; }

        [JsonPropertyName("visibility")]
        public double?[] Visibility { get; set; }

        [JsonPropertyName("weather_code")]
        public int?[] WeatherCode { get; set; }
    }

    public class HourlyUnits
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public string Temperature2M { get; set; }

        [JsonPropertyName("apparent_temperature")]
        public string ApparentTemperature { get; set; }

        [JsonPropertyName("precipitation_probability")]
        public string PrecipitationProbability { get; set; }

        [JsonPropertyName("precipitation")]
        public string Precipitation { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public string WindSpeed10M { get; set; }

        [JsonPropertyName("wind_direction_10m")]
        public string WindDirection10M { get; set; }

        [JsonPropertyName("cloud_cover")]
        public string CloudCover { get; set; }

        [JsonPropertyName("visibility")]
        public string Visibility { get; set; }

        [JsonPropertyName("weather_code")]
        public string WeatherCode { get; set; }
    }
}
