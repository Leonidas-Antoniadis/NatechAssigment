using System.Text.Json.Serialization;

namespace Natech.BussinessLayer.DTOs
{
    public class IpBaseDataReponse
    {
        [JsonPropertyName("location")]
        public Location Geolocation { get; set; }
    }
}
