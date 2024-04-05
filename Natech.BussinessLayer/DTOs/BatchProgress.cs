namespace Natech.BussinessLayer.DTOs
{
    public class BatchProgress
    {
        public string Progress { get; set; }
        public DateTime ExptectedTime { get; set; }
        public List<GeolocationBatchResult> CompletedItems { get; set; }
    }
}
