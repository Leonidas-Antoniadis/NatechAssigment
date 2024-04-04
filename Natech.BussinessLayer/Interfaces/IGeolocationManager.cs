using Natech.BussinessLayer.Models;
using NatechAssignment.Models;

namespace Natech.BussinessLayer.Interfaces
{
    public interface IGeolocationManager
    {
        Task<Result<Location>> FetchGeolocation(string ip);
        Task<Result<string>> BatchProcessIPs(List<string> ips);
        Task<Result<BatchProgress>> GetBatchProgress(string batchId);
    }
}
