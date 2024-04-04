using Natech.BussinessLayer.Models;
using NatechAssignment.Models;

namespace Natech.BussinessLayer.Interfaces
{
    public interface IGeolocationManager
    {
        Task<Result<Location>> FetchGeolocation(string ip);
        Task<Result<int>> FetchMultipleIps(List<string> ips);
        Task<Result<BatchProgress>> GetBatchResult(int batchId);
    }
}
