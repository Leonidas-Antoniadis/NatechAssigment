using Microsoft.AspNetCore.Mvc;
using Natech.BussinessLayer.Interfaces;
using Natech.Services.Interfaces;
using NatechAssignment.Responses;


namespace NatechAssignment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeolocationController : ControllerBase
    {
        private readonly IGeolocationManager _geolocationManager;
        private readonly IBatchService _batchService;

        public GeolocationController(IGeolocationManager geolocationManager, IBatchService batchService)
        {
            _geolocationManager = geolocationManager;
            _batchService = batchService;
        }

        //All the mappings could have been made with an AutoMapper

        [HttpGet("{ip}")]
        public async Task<IActionResult> Get(string ip)
        {
            var result = await _geolocationManager.FetchFrom(ip);

            if (!result.Success)
                return StatusCode(result.Error.ErrorCode, result.Error);

            var geolocation = new GeolocationResponse
            {
                CountryCode = result.Data.Country.Ioc,
                CountryName = result.Data.Country.Name,
                IP = ip,
                Latitude = result.Data.Latitude.ToString(),
                Longitude = result.Data.Longitude.ToString(),
                TimeZone = result.Data.Country.Timezones.FirstOrDefault()
            };

            return Ok(geolocation);
        }

        [HttpPost()]
        public async Task<IActionResult> SearchForMultiple(List<string> ips)
        {
            var result = await _geolocationManager.SearchForMultiple(ips);

            if (!result.Success)
                return StatusCode(result.Error.ErrorCode, result.Error);

            var response = new MultipleIpsResponse
            {
                BatchId = result.Data.ToString(),
                Url = nameof(GetBatchProgress)
            };

            return Ok(response);
        }

        [HttpGet("GetBatchProgress/{batchId}")]
        public async Task<IActionResult> GetBatchProgress(int batchId)
        {
            var result = await _batchService.GetBatchResult(batchId);

            if (!result.Success)
                return StatusCode(result.Error.ErrorCode, result.Error);

            var response = new BatchProgressResponse
            {
                Progress = result.Data.Progress,
                ExptectedTime = result.Data.ExptectedTime.ToString("dd/MM/yyyy HH:mm:ss"),
            };

            response.GeolocationResponses = new List<GeolocationResponse>();

            result.Data.CompletedItems.ForEach(x => response.GeolocationResponses.Add(new GeolocationResponse
            {
                CountryCode = x.CountryCode,
                CountryName = x.CountryName,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                TimeZone = x.TimeZone,
                IP = x.IP
            }));

            return Ok(response);
        }
    }
}
