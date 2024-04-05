using Natech.BussinessLayer.DTOs;
using Natech.Common.DTOs;
using Natech.DataLayer.Models;

namespace Natech.Services.Interfaces
{
    public interface IBatchService
    {
        Task<Result<Batch>> StartBatch(int TotalItems);
        Task<Result<bool>> UpdateBatchProgress(int batchId, string resultToAdd);
        Task<Result<BatchProgress>> GetBatchResult(int batchId);
    }
}
