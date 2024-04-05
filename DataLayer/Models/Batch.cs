using System.ComponentModel.DataAnnotations;

namespace Natech.DataLayer.Models
{
    public class Batch
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public int TotalItems { get; set; }

        [Required]
        public int Progress { get; set; }
        public DateTime? EstimateFinish { get; set; }
        public string BatchResult { get; set; }
    }
}
