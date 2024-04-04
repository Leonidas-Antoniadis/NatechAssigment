using DataLayer.Models;
using Microsoft.Extensions.Configuration;
using Natech.BussinessLayer.Interfaces;
using Natech.BussinessLayer.Models;
using Natech.Common.Extensions;
using Natech.Common.Models;
using Natech.DataLayer.Interface;
using NatechAssignment.Models;
using NatechAssignment.Services.Interfaces;

namespace Natech.BussinessLayer
{
    public class GeolocationManager : IGeolocationManager
    {
        private readonly ICommunicationService _communicationService;
        private readonly IRepository<Geolocation> _repository;

        private readonly GeolocationConfig _geolocationConfig;

        private static readonly List<Batch> _batches = new List<Batch>();
        public GeolocationManager(ICommunicationService communicationService, IRepository<Geolocation> repository, IConfiguration configuration)
        {
            _communicationService = communicationService;
            _repository = repository;
            _geolocationConfig = configuration.GetGeolocationConfig();
        }

        public async Task<Result<Location>> FetchGeolocation(string ip)
        {
            var url = $"{_geolocationConfig.Url}{_geolocationConfig.Key}&{nameof(ip)}={ip}";

            var result = await _communicationService.GetAsync<IpBaseResponse>(url);

            if (!result.Success)
                return Result<Location>.CreateFailure(result.Error);

            return Result<Location>.CreateSuccess(result.Data.Data.Geolocation);
        }

        public async Task<Result<string>> BatchProcessIPs(List<string> ips)
        {
            string batchId = Guid.NewGuid().ToString();

            var task = Task.Run(async () =>
            {
                try
                {
                    var ipProcessingStatus = ips.Select(ip => new { Key = ip, Value = false })
                                .ToDictionary(pair => pair.Key, pair => pair.Value);

                    var batch = new Batch
                    {
                        Id = batchId,
                        StartTime = DateTime.Now,
                        IpProcessingStatus = ipProcessingStatus
                    };

                    _batches.Add(batch);

                    foreach (var ip in ips)
                    {
                        var result = await FetchGeolocation(ip);
                        await Task.Delay(10000);

                        if (result.Success)
                            //logerr

                            batch.IpProcessingStatus[ip] = true;
                    }

                    _batches.Remove(batch);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);//logger
                }
            });

            return Result<string>.CreateSuccess(batchId);
        }

        public async Task<Result<BatchProgress>> GetBatchProgress(string batchId)
        {
            var batch = _batches.FirstOrDefault(x => x.Id == batchId);

            if (batch == null)
                return Result<BatchProgress>.CreateFailure(new Error(404, "Batch not found"));

            int totalCount = batch.IpProcessingStatus.Count;
            int processedCount = batch.IpProcessingStatus.Count(kv => kv.Value);
            int progress = processedCount * 100 / totalCount;

            TimeSpan elapsedTime = DateTime.Now - batch.StartTime;
            int remainingCount = batch.IpProcessingStatus.Count(kv => !kv.Value);

            double averageTimePerProcessedIPMilliseconds = processedCount > 0 ? elapsedTime.TotalMilliseconds / processedCount : 0;
            double estimatedRemainingTimeMilliseconds = averageTimePerProcessedIPMilliseconds * remainingCount;

            var estimatedCompletionTime = DateTime.Now.AddMilliseconds(estimatedRemainingTimeMilliseconds);

            var batchProgress = new BatchProgress
            {
                ExptectedTime = estimatedCompletionTime,
                Progress = $"{progress}/100"
            };

            return Result<BatchProgress>.CreateSuccess(batchProgress);
        }
    }
}
