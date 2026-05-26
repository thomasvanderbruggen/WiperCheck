namespace WiperCheck.Models.Responses.GeocodeAPI;

using System;
using System.Text.Json.Serialization;

public class GeocodeApiResponse
{
    [JsonPropertyName("geocoding")]
    public Geocoding Geocoding { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("features")]
    public Feature[] Features { get; set; }

    [JsonPropertyName("bbox")]
    public double[] Bbox { get; set; }
}

public class Feature
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("geometry")]
    public Geometry Geometry { get; set; }

    [JsonPropertyName("properties")]
    public Properties Properties { get; set; }
}

public class Geometry
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("coordinates")]
    public double[] Coordinates { get; set; }
}

public class Properties
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("gid")]
    public string Gid { get; set; }

    [JsonPropertyName("layer")]
    public string Layer { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("source_id")]
    public string SourceId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("housenumber")]
    public string Housenumber { get; set; }

    [JsonPropertyName("street")]
    public string Street { get; set; }

    [JsonPropertyName("postalcode")]
    public string Postalcode { get; set; }

    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }

    [JsonPropertyName("match_type")]
    public string MatchType { get; set; }

    [JsonPropertyName("accuracy")]
    public string Accuracy { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("country_gid")]
    public string CountryGid { get; set; }

    [JsonPropertyName("country_a")]
    public string CountryA { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }

    [JsonPropertyName("region_gid")]
    public string RegionGid { get; set; }

    [JsonPropertyName("region_a")]
    public string RegionA { get; set; }

    [JsonPropertyName("county")]
    public string County { get; set; }

    [JsonPropertyName("county_gid")]
    public string CountyGid { get; set; }

    [JsonPropertyName("county_a")]
    public string CountyA { get; set; }

    [JsonPropertyName("locality")]
    public string Locality { get; set; }

    [JsonPropertyName("locality_gid")]
    public string LocalityGid { get; set; }

    [JsonPropertyName("continent")]
    public string Continent { get; set; }

    [JsonPropertyName("continent_gid")]
    public string ContinentGid { get; set; }

    [JsonPropertyName("label")]
    public string Label { get; set; }
}

public class Geocoding
{
    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("attribution")]
    public Uri Attribution { get; set; }

    [JsonPropertyName("query")]
    public Query Query { get; set; }

    [JsonPropertyName("engine")]
    public Engine Engine { get; set; }

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
}

public class Engine
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("author")]
    public string Author { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }
}

public class Query
{
    [JsonPropertyName("parsed_text")]
    public ParsedText ParsedText { get; set; }

    [JsonPropertyName("size")]
    public double Size { get; set; }

    [JsonPropertyName("private")]
    public bool Private { get; set; }

    [JsonPropertyName("lang")]
    public Lang Lang { get; set; }

    [JsonPropertyName("querySize")]
    public double QuerySize { get; set; }
}

public class Lang
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("iso6391")]
    public string Iso6391 { get; set; }

    [JsonPropertyName("iso6393")]
    public string Iso6393 { get; set; }

    [JsonPropertyName("via")]
    public string Via { get; set; }

    [JsonPropertyName("defaulted")]
    public bool Defaulted { get; set; }
}

public class ParsedText
{
    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("postalcode")]
    public string Postalcode { get; set; }

    [JsonPropertyName("housenumber")]
    public string Housenumber { get; set; }

    [JsonPropertyName("street")]
    public string Street { get; set; }
}
