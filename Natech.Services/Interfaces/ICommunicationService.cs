using NatechAssignment.Models;

namespace NatechAssignment.Services.Interfaces
{
    public interface ICommunicationService
    {
        Task<Result<T>> GetAsync<T>(string url);
    }
}
