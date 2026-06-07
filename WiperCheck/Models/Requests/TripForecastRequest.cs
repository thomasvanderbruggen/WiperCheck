using WiperCheck.Models.Forms;

namespace WiperCheck.Models.Requests;

public class TripForecastRequest
{
    public Address StartAddress { get; set; }
    public Address EndAddress { get; set; }
    public DateTime EarliestDeparture { get; set; }
    public DateTime LatestDeparture { get; set; }
}