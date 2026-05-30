namespace WiperCheck.Models.Responses.ReverseGeocodeAPI;

public class ReverseGeocodeApiResponse
{
    public Geocoding geocoding { get; set; }
    public string type { get; set; }
    public Features[] features { get; set; }
    public double[] bbox { get; set; }
}

public class Geocoding
{
    public string version { get; set; }
    public string attribution { get; set; }
    public Query query { get; set; }
    public Engine engine { get; set; }
    public long timestamp { get; set; }
}



public class Lang
{
    public string name { get; set; }
    public string iso6391 { get; set; }
    public string iso6393 { get; set; }
    public string via { get; set; }
    public bool defaulted { get; set; }
}

public class Engine
{
    public string name { get; set; }
    public string author { get; set; }
    public string version { get; set; }
}

public class Features
{
    public string type { get; set; }
    public Geometry geometry { get; set; }
    public Properties properties { get; set; }
    public double[] bbox { get; set; }
}

public class Geometry
{
    public string type { get; set; }
    public double[] coordinates { get; set; }
}

public class Properties
{
    public string id { get; set; }
    public string gid { get; set; }
    public string layer { get; set; }
    public string source { get; set; }
    public string source_id { get; set; }
    public string name { get; set; }
    public string street { get; set; }
    public double confidence { get; set; }
    public double distance { get; set; }
    public string accuracy { get; set; }
    public string country { get; set; }
    public string country_gid { get; set; }
    public string country_a { get; set; }
    public string region { get; set; }
    public string region_gid { get; set; }
    public string region_a { get; set; }
    public string county { get; set; }
    public string county_gid { get; set; }
    public string county_a { get; set; }
    public string locality { get; set; }
    public string locality_gid { get; set; }
    public string neighbourhood { get; set; }
    public string neighbourhood_gid { get; set; }
    public string continent { get; set; }
    public string continent_gid { get; set; }
    public string label { get; set; }
}


