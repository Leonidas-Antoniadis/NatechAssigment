using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Natech.BussinessLayer.Interfaces;
using Natech.BussinessLayer.Models;
using Natech.Common.Extensions;
using Natech.Common.Models;
using Natech.DataLayer.Interface;
using Natech.DataLayer.Models;
using NatechAssignment.Models;
using NatechAssignment.Services.Interfaces;
using System.Text.Json;

namespace Natech.BussinessLayer
{
    public class GeolocationManager : IGeolocationManager
    {
        private readonly ICommunicationService _communicationService;
        private readonly IRepository<Batch> _repository;
        private readonly ILogger<GeolocationManager> _logger;

        private readonly IServiceProvider _serviceProvider;

        private readonly GeolocationConfig _geolocationConfig;
        public GeolocationManager(ICommunicationService communicationService,
            IRepository<Batch> repository, IConfiguration configuration, ILogger<GeolocationManager> logger, IServiceProvider serviceProvider)
        {
            _communicationService = communicationService;
            _repository = repository;
            _logger = logger;
            _geolocationConfig = configuration.GetGeolocationConfig();
            _serviceProvider = serviceProvider;
        }

        public async Task<Result<Location>> FetchGeolocation(string ip)
        {
            var url = $"{_geolocationConfig.Url}{_geolocationConfig.Key}&{nameof(ip)}={ip}";

            var result = await _communicationService.GetAsync<IpBaseResponse>(url);

            if (!result.Success)
                return Result<Location>.CreateFailure(result.Error);

            return Result<Location>.CreateSuccess(result.Data.Data.Geolocation);
        }

        public async Task<Result<int>> FetchMultipleIps(List<string> ips)
        {
            try
            {
                var batch = new Batch
                {
                    TotalItems = ips.Count,
                    Progress = 0,
                    StartTime = DateTime.Now
                };

                await _repository.AddAsync(batch);
                await _repository.SaveChangesAsync();

                BackgroundTask(batch, ips);

                return Result<int>.CreateSuccess(batch.Id);
            }
            catch (Exception e)
            {
                return Result<int>.CreateFailure(new Error(500, e.Message));
            }
        }

        public async Task<Result<BatchProgress>> GetBatchResult(int batchId)
        {
            try
            {
                var batch = await _repository.GetByIdAsync(batchId);

                if (batch == null)
                    return Result<BatchProgress>.CreateFailure(new Error(404, "Batch not found"));

                var batchProgress = new BatchProgress
                {
                    ExptectedTime = batch.EstimateFinish,
                    CompletedIps = JsonSerializer.Deserialize<List<GeolocationBatchResult>>(batch.BatchResult),
                    Progress = GetProgressString(batch)
                };

                return Result<BatchProgress>.CreateSuccess(batchProgress);
            }
            catch (Exception e)
            {
                return Result<BatchProgress>.CreateFailure(new Error(500, e.Message));
            }
        }

        private TimeSpan CalculateEstimatedRemainingTime(int progress, TimeSpan elapsedTime, int remainingCount)
        {
            if (progress == 0)
                return TimeSpan.MaxValue;
            else
            {
                double averageTimePerIpMilliseconds = elapsedTime.TotalMilliseconds / progress;
                double estimatedRemainingTimeMilliseconds = averageTimePerIpMilliseconds * remainingCount;

                return TimeSpan.FromMilliseconds(estimatedRemainingTimeMilliseconds);
            }
        }

        private async Task BackgroundTask(Batch batch, List<string> ips)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            {
                var repository = scope.ServiceProvider.GetRequiredService<IRepository<Batch>>();

                try
                {
                    var ipsLocationResult = new List<GeolocationBatchResult>();

                    foreach (var ip in ips)
                    {
                        var result = await FetchGeolocation(ip);

                        if (!result.Success)
                            _logger.LogError($"{result.Error.ErrorCode}:{result.Error.ErrorMessage}");

                        ipsLocationResult.Add(new GeolocationBatchResult
                        {
                            CountryCode = result.Data.Country.Ioc,
                            CountryName = result.Data.Country.Name,
                            IP = ip,
                            Latitude = result.Data.Latitude.ToString(),
                            Longitude = result.Data.Longitude.ToString(),
                            TimeZone = result.Data.Country.Timezones.FirstOrDefault()
                        });

                        batch.Progress++;

                        var elapsedTime = DateTime.Now - batch.StartTime;
                        var estimatedRemainingTime = CalculateEstimatedRemainingTime(batch.Progress, elapsedTime, ips.Count - batch.Progress);
                        batch.EstimateFinish = DateTime.Now.Add(estimatedRemainingTime);
                        batch.BatchResult = JsonSerializer.Serialize(ipsLocationResult);

                        repository.UpdateAsync(batch);
                        await repository.SaveChangesAsync();

                        await Task.Delay(10000);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"{e.Message}");
                }
            }
            scope.Dispose();
        }

        private string GetProgressString(Batch batch)
        {
            if (batch.TotalItems == 0)
                return "0/0";

            double percentage = (double)batch.Progress / batch.TotalItems * 100;
            return $"{batch.Progress}/{batch.TotalItems} ({percentage:F0}%)";
        }
    }
}
