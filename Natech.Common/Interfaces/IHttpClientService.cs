using Natech.Common.DTOs;

namespace Natech.Common.Interfaces
{
    public interface IHttpClientService
    {
        Task<Result<T>> GetAsync<T>(string url);
    }
}
