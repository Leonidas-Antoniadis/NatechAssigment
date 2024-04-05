using Natech.BussinessLayer.Models;
using NatechAssignment.Models;

namespace Natech.BussinessLayer.Interfaces
{
    public interface IGeolocationManager
    {
        Task<Result<Location>> FetchFrom(string ip);
        Task<Result<int>> SearchForMultiple(List<string> ips);
    }
}
