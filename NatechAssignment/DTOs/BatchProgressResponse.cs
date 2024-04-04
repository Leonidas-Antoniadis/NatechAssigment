namespace NatechAssignment.DTOs
{
    public class BatchProgressResponse
    {
        public string Progress { get; set; }
        public string ExptectedTime { get; set; }
        public List<GeolocationResponse> GeolocationResponses { get; set; }
    }
}
