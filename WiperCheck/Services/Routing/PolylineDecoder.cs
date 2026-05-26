using WiperCheck.Models.Utilities;

namespace WiperCheck.Services.Routing;


public static class PolylineDecoder
{
    public static List<GeocodeLocation> Decode(string encodedPolyline)
    {
        var coordinates = new List<GeocodeLocation>();
        var index = 0;
        var lat = 0;
        var lng = 0;

        while (index < encodedPolyline.Length)
        {
            lat += DecodeNext(encodedPolyline, ref index);
            lng += DecodeNext(encodedPolyline, ref index);

            coordinates.Add(new GeocodeLocation(
                lat / 1e5,
                lng / 1e5
            ));
        }

        return coordinates;
    }

    private static int DecodeNext(string encoded, ref int index)
    {
        var result = 0;
        var shift = 0;
        char chunk;

        do
        {
            chunk = encoded[index++];
            var value = chunk - 63;
            result |= (value & 0x1F) << shift;
            shift += 5;
        } while (chunk - 63 >= 0x20);

        return (result & 1) != 0 ? ~(result >> 1) : result >> 1;
    }
}