namespace Natech.BussinessLayer.Models
{
    public class BatchProgress
    {
        public string Progress { get; set; }
        public DateTime ExptectedTime { get; set; }
        public List<GeolocationBatchResult> CompletedIps { get; set; }
    }
}
