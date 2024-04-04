using System.Text.Json.Serialization;

namespace Natech.BussinessLayer.Models
{
    public class IpBaseDataReponse
    {
        [JsonPropertyName("location")]
        public Location Geolocation { get; set; }
    }
}
