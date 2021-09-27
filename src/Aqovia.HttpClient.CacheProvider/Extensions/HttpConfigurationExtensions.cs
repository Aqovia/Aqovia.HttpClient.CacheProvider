using System.Web.Http;
using Aqovia.HttpClient.CacheProvider.Configurations;

namespace Aqovia.HttpClient.CacheProvider.Extensions
{
    public static class HttpConfigurationExtensions
    {
        public static CacheOutputConfiguration CacheOutputConfiguration(this HttpConfiguration config)
        {
            return new CacheOutputConfiguration(config);
        }
    }
}