using System.Text.Json.Serialization;

namespace WiperCheck.Models.Requests;

public class DirectionsRequest
{
    [JsonPropertyName("coordinates")]
    public List<List<double>> Coordinates { get; set; }

    [JsonPropertyName("units")] 
    public string Units { get; set; }
}