using Microsoft.Extensions.Configuration;
using Natech.Common.DTOs;

namespace Natech.Common.Extensions
{
    public static class Configuration
    {
        public static GeolocationConfig GetGeolocationConfig(this IConfiguration configuration)
            => configuration.GetSection("Geolocation").Get<GeolocationConfig>() ?? new GeolocationConfig();
    }
}
