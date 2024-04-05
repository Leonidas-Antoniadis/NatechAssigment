using System.Text.Json.Serialization;

namespace Natech.BussinessLayer.Models
{
    public class IpBaseResponse
    {
        [JsonPropertyName("data")]
        public IpBaseDataReponse Data { get; set; }
    }
}
