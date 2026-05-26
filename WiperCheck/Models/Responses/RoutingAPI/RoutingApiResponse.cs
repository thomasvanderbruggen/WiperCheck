namespace WiperCheck.Models.Responses 

{
    using System;
    using System.Text.Json.Serialization;

    public class RoutingApiResponse
    {
        [JsonPropertyName("bbox")]
        public double[] Bbox { get; set; }

        [JsonPropertyName("routes")]
        public Route[] Routes { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; }
    }

    public class Metadata
    {
        [JsonPropertyName("attribution")]
        public string Attribution { get; set; }

        [JsonPropertyName("service")]
        public string Service { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("query")]
        public Query Query { get; set; }

        [JsonPropertyName("engine")]
        public Engine Engine { get; set; }
    }

    public class Engine
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("build_date")]
        public DateTimeOffset BuildDate { get; set; }

        [JsonPropertyName("graph_date")]
        public DateTimeOffset GraphDate { get; set; }

        [JsonPropertyName("osm_date")]
        public DateTimeOffset OsmDate { get; set; }
    }

    public class Query
    {
        [JsonPropertyName("coordinates")]
        public double[][] Coordinates { get; set; }

        [JsonPropertyName("profile")]
        public string Profile { get; set; }

        [JsonPropertyName("profileName")]
        public string ProfileName { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }
    }

    public class Route
    {
        [JsonPropertyName("summary")]
        public Summary Summary { get; set; }

        [JsonPropertyName("segments")]
        public Segment[] Segments { get; set; }

        [JsonPropertyName("bbox")]
        public double[] Bbox { get; set; }

        [JsonPropertyName("geometry")]
        public string Geometry { get; set; }

        [JsonPropertyName("way_points")]
        public long[] WayPoints { get; set; }
    }

    public class Segment
    {
        [JsonPropertyName("distance")]
        public double Distance { get; set; }

        [JsonPropertyName("duration")]
        public double Duration { get; set; }

        [JsonPropertyName("steps")]
        public Step[] Steps { get; set; }
    }

    public class Step
    {
        [JsonPropertyName("distance")]
        public double Distance { get; set; }

        [JsonPropertyName("duration")]
        public double Duration { get; set; }

        [JsonPropertyName("type")]
        public long Type { get; set; }

        [JsonPropertyName("instruction")]
        public string Instruction { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("way_points")]
        public long[] WayPoints { get; set; }
    }

    public class Summary
    {
        [JsonPropertyName("distance")]
        public double Distance { get; set; }

        [JsonPropertyName("duration")]
        public double Duration { get; set; }
    }
}
