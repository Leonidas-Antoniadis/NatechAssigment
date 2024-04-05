using Microsoft.Extensions.Logging;
using Natech.BussinessLayer;
using Natech.BussinessLayer.DTOs;
using Natech.Common.DTOs;
using Natech.DataLayer.Interface;
using Natech.DataLayer.Models;
using Natech.Services.Interfaces;
using System.Text.Json;

namespace Natech.Services
{
    public class BatchService : IBatchService
    {
        private readonly IRepository<Batch> _repository;
        private readonly ILogger<GeolocationService> _logger;

        public BatchService(IRepository<Batch> repository, ILogger<GeolocationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<Batch>> StartBatch(int TotalItems)
        {
            try
            {
                var batch = new Batch
                {
                    TotalItems = TotalItems,
                    Progress = 0,
                    StartTime = DateTime.Now
                };

                await _repository.AddAsync(batch);
                await _repository.SaveChangesAsync();

                return Result<Batch>.CreateSuccess(batch);
            }
            catch (Exception e)
            {
                return Result<Batch>.CreateFailure(new Error(500, e.Message));
            }
        }

        public async Task<Result<bool>> UpdateBatchProgress(int batchId, string resultToAdd)
        {
            try
            {
                var batch = await _repository.FindAsync(batchId);

                if (batch.Progress < batch.TotalItems)
                    batch.Progress++;
                else
                    return Result<bool>.CreateFailure(new Error(400, "Already finished"));

                var elapsedTime = DateTime.Now - batch.StartTime;
                var estimatedRemainingTime = CalculateEstimatedRemainingTime(batch.Progress, elapsedTime, batch.TotalItems - batch.Progress);
                batch.EstimateFinish = DateTime.Now.Add(estimatedRemainingTime);

                batch.BatchResult = resultToAdd;

                _repository.UpdateAsync(batch);
                await _repository.SaveChangesAsync();

                return Result<bool>.CreateSuccess(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Result<bool>.CreateFailure(new Error(500, e.Message));
            }
        }

        public async Task<Result<BatchProgress>> GetBatchResult(int batchId)
        {
            try
            {
                var batch = await _repository.FindAsync(batchId);

                if (batch == null)
                    return Result<BatchProgress>.CreateFailure(new Error(404, "Batch not found"));

                var batchProgress = new BatchProgress
                {
                    ExptectedTime = (DateTime)batch.EstimateFinish,
                    CompletedItems = !string.IsNullOrEmpty(batch.BatchResult) ? JsonSerializer.Deserialize<List<GeolocationBatchResult>>(batch.BatchResult) : new List<GeolocationBatchResult>(),
                    Progress = GetProgressString(batch)
                };

                return Result<BatchProgress>.CreateSuccess(batchProgress);
            }
            catch (Exception e)
            {
                return Result<BatchProgress>.CreateFailure(new Error(500, e.Message));
            }
        }

        private string GetProgressString(Batch batch)
        {
            if (batch.TotalItems == 0)
                return "0/0";

            double percentage = (double)batch.Progress / batch.TotalItems * 100;
            return $"{batch.Progress}/{batch.TotalItems} ({percentage:F0}%)";
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
    }
}
