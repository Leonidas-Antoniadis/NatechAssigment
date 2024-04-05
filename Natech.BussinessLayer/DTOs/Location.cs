using System.Text.Json.Serialization;

namespace Natech.BussinessLayer.DTOs
{
    public class Location
    {
        [JsonPropertyName("geonames_id")]
        public int GeonamesId { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("zip")]
        public string Zip { get; set; }

        [JsonPropertyName("country")]
        public Country Country { get; set; }
    }
}
