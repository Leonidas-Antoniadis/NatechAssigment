using Microsoft.Extensions.Logging;
using NatechAssignment.Models;
using NatechAssignment.Services.Interfaces;
using System.Text.Json;

namespace NatechAssignment.Services
{
    public class CommunicationService : ICommunicationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CommunicationService> _logger;

        public CommunicationService(IHttpClientFactory httpClientFactory, ILogger<CommunicationService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<Result<T>> GetAsync<T>(string url)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                    return Result<T>.CreateSuccess(JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync()));

                else
                    return Result<T>.CreateFailure(new Error((int)response.StatusCode, response.ReasonPhrase));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong{ex.Message} on the get call: {url}");
                return Result<T>.CreateFailure(new Error(500, ex.Message));
            }
        }
    }
}
