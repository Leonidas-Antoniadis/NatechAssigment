namespace Natech.DataLayer.Models
{
    public class Batch
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public int TotalItems { get; set; }
        public int Progress { get; set; }
        public DateTime EstimateFinish { get; set; }
        public string BatchResult { get; set; }
    }
}
