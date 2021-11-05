using System;
using CacheCow.Client;
using CacheCow.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Aqovia.CachingHttpClient.Extensions
{
    public static class AspNetCoreExtensions
    {
        public static IServiceCollection AddCachingHttpClient(this IServiceCollection services, CachingConfiguration cachingOptions)
        {
            var cacheStore = string.IsNullOrWhiteSpace(cachingOptions.RedisConnectionString)
                ? CachingHelpers.CreateInMemoryCacheStore(cachingOptions.MinExpiry)
                : CachingHelpers.CreateRedisCacheStore(cachingOptions.RedisConnectionString, cachingOptions.MinExpiry);
            services.AddSingleton<ICacheStore>(cacheStore);
            services.AddSingleton<System.Net.Http.HttpClient>(cacheStore.CreateClient());
            return services;
        }
    }

    public class CachingConfiguration
    {
        public string RedisConnectionString { get; set; }
        public TimeSpan? MinExpiry { get; set; }
    }
}