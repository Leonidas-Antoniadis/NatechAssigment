using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class Geolocation
    {
        public int Id { get; set; }

        [Required]
        public string IP { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string TimeZone { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
