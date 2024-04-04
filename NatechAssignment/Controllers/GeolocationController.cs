using Microsoft.AspNetCore.Mvc;
using Natech.BussinessLayer.Interfaces;
using NatechAssignment.DTOs;

namespace NatechAssignment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeolocationController : ControllerBase
    {
        private readonly IGeolocationManager _geolocationManager;

        public GeolocationController(IGeolocationManager geolocationManager)
        {
            _geolocationManager = geolocationManager;
        }

        //All the mappings could have been made with an AutoMapper

        [HttpGet("{ip}")]
        public async Task<IActionResult> Get(string ip)
        {
            var result = await _geolocationManager.FetchGeolocation(ip);

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
        public async Task<IActionResult> Get(List<string> ips)
        {
            var result = await _geolocationManager.FetchMultipleIps(ips);

            if (!result.Success)
                return StatusCode(result.Error.ErrorCode, result.Error);

            var response = new MultipleIpsResponse
            {
                BatchId = result.Data.ToString(),
                Url = "BatchProgress"
            };

            return Ok(response);
        }

        [HttpGet("BatchProgress/{batchId}")]
        public async Task<IActionResult> GetBatchProgress(int batchId)
        {
            var result = await _geolocationManager.GetBatchResult(batchId);

            if (!result.Success)
                return StatusCode(result.Error.ErrorCode, result.Error);

            var response = new BatchProgressResponse
            {
                Progress = result.Data.Progress,
                ExptectedTime = result.Data.ExptectedTime.ToString("dd/MM/yyyy HH:mm:ss"),
            };

            response.GeolocationResponses = new List<GeolocationResponse>();

            result.Data.CompletedIps.ForEach(x => response.GeolocationResponses.Add(new GeolocationResponse
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
