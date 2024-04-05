using System.Text.Json.Serialization;

namespace Natech.BussinessLayer.Models
{
    public class Country
    {
        [JsonPropertyName("alpha2")]
        public string Alpha2 { get; set; }

        [JsonPropertyName("alpha3")]
        public string Alpha3 { get; set; }

        [JsonPropertyName("calling_codes")]
        public List<string> CallingCodes { get; set; }

        [JsonPropertyName("emoji")]
        public string Emoji { get; set; }

        [JsonPropertyName("ioc")]
        public string Ioc { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("name_translated")]
        public string NameTranslated { get; set; }

        [JsonPropertyName("timezones")]
        public List<string> Timezones { get; set; }

        [JsonPropertyName("is_in_european_union")]
        public bool IsInEuropeanUnion { get; set; }

        [JsonPropertyName("fips")]
        public string Fips { get; set; }

        [JsonPropertyName("geonames_id")]
        public int GeonamesId { get; set; }

        [JsonPropertyName("hasc_id")]
        public string HascId { get; set; }

        [JsonPropertyName("wikidata_id")]
        public string WikidataId { get; set; }
    }
}