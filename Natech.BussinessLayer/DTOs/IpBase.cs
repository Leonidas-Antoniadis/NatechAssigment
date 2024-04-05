using System.Text.Json.Serialization;

namespace Natech.BussinessLayer.DTOs
{
    public class IpBaseResponse
    {
        [JsonPropertyName("data")]
        public IpBaseDataReponse Data { get; set; }
    }
}
