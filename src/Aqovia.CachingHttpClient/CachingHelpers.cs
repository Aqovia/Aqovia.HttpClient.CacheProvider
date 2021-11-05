using System;
using System.Net.Http;
using CacheCow.Client;
using CacheCow.Client.RedisCacheStore;
using CacheCow.Common;

namespace Aqovia.CachingHttpClient
{
    public static class CachingHelpers
    {
        public static ICacheStore CreateInMemoryCacheStore(TimeSpan? minExpiry = null) =>
            new InMemoryCacheStore(minExpiry ?? TimeSpan.FromHours(6)); // 6 Hours is default CacheCow TimeSpan

        public static ICacheStore CreateRedisCacheStore(string redisConnectionString, TimeSpan? minExpiry = null)
        {
            var store = new RedisStore(RedisConnection.Create(redisConnectionString).Value);
            if (minExpiry != null)
                store.MinExpiry = minExpiry.Value;
            return store;
        }

        public static HttpClient GetClient(string redisConnectionString, TimeSpan? minExpiry = null) =>
            CreateRedisCacheStore(redisConnectionString, minExpiry).CreateClient();

        public static HttpClient GetClient(TimeSpan? minExpiry = null) =>
            CreateInMemoryCacheStore(minExpiry).CreateClient();
    }
}