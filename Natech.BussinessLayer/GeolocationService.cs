using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Natech.BussinessLayer.Interfaces;
using Natech.BussinessLayer.Models;
using Natech.Common.Extensions;
using Natech.Common.Interfaces;
using Natech.Common.Models;
using Natech.Services.Interfaces;
using NatechAssignment.Models;
using System.Text.Json;

namespace Natech.BussinessLayer
{
    public class GeolocationService : IGeolocationManager
    {
        private readonly IHttpClientService _httpClient;
        private readonly ILogger<GeolocationService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IBatchService _batchService;

        private readonly GeolocationConfig _geolocationConfig;

        public GeolocationService(IHttpClientService httpClient, IConfiguration configuration, ILogger<GeolocationService> logger, IServiceProvider serviceProvider, IBatchService batchService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _geolocationConfig = configuration.GetGeolocationConfig();
            _serviceProvider = serviceProvider;
            _batchService = batchService;
        }

        public async Task<Result<Location>> FetchFrom(string ip)
        {
            var url = $"{_geolocationConfig.Url}{_geolocationConfig.Key}&{nameof(ip)}={ip}";

            var result = await _httpClient.GetAsync<IpBaseResponse>(url);

            if (!result.Success)
                return Result<Location>.CreateFailure(result.Error);

            return Result<Location>.CreateSuccess(result.Data.Data.Geolocation);
        }

        public async Task<Result<int>> SearchForMultiple(List<string> ips)
        {
            var resultBatch = await _batchService.StartBatch(ips.Count);

            _ = Task.Run(async () =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var batchService = scope.ServiceProvider.GetRequiredService<IBatchService>();

                    var results = new List<GeolocationBatchResult>();

                    foreach (var ip in ips)
                    {
                        var result = await FetchFrom(ip);

                        if (!result.Success)
                            _logger.LogError($"{result.Error.ErrorCode}:{result.Error.ErrorMessage}");

                        var resultToAdd = new GeolocationBatchResult
                        {
                            CountryCode = result.Data.Country.Ioc,
                            CountryName = result.Data.Country.Name,
                            IP = ip,
                            Latitude = result.Data.Latitude.ToString(),
                            Longitude = result.Data.Longitude.ToString(),
                            TimeZone = result.Data.Country.Timezones.FirstOrDefault()
                        };
                        results.Add(resultToAdd);

                        var updateResult = await batchService.UpdateBatchProgress(resultBatch.Data.Id, JsonSerializer.Serialize(results));

                        if (!updateResult.Data)
                            _logger.LogError($"Ip failed to update reason {updateResult.Error.ErrorMessage}");

                        await Task.Delay(10000);
                    }

                    scope.Dispose();
                }
            });

            return Result<int>.CreateSuccess(resultBatch.Data.Id);
        }
    }
}
