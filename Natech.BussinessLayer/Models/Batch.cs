namespace Natech.BussinessLayer.Models
{
    public class Batch
    {
        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public Dictionary<string, bool> IpProcessingStatus { get; set; }
    }
}
