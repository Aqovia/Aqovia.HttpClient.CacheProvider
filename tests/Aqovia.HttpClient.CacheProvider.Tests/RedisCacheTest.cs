using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aqovia.HttpClient.CacheProvider.Cache;
using Aqovia.HttpClient.CacheProvider.Tests.Abstraction;
using Newtonsoft.Json;
using Xbehave;
using Xunit;

namespace Aqovia.HttpClient.CacheProvider.Tests
{
   public class RedisCacheTest
    {
        private IEnumerable<string> allKeys = null;
        private ICacheOutput cache = new RedisCache("127.0.0.1:6379,ssl=false");
       
        [Scenario(Skip = "Skipped due to not unable to connect RedisCache.")]
        public void AddCache_And_Return_Allkeys_FromRedisCache()
        {

            "When Cache key-values are added to RedisCache".x(_ =>
            {

                cache.Add("base", "abc", DateTime.Now.AddSeconds(60));
                cache.Add("key1", "abc", DateTime.Now.AddSeconds(60), "base");
                allKeys = cache.AllKeys;
            });
            "Then it should return all keys".x(_ => {
                Assert.NotNull(allKeys);
                Assert.True(allKeys.Any());
            });
            "Ensure caches are cleared".x(_ => {
                cache.RemoveStartsWith("base");
                var baseKey = cache.Get<string>("base");
                Assert.Null(baseKey);
                cache.RemoveStartsWith("key1");
                var key1 = cache.Get<string>("key1");
                Assert.Null(key1);
            });

        }

        [Scenario(Skip = "Skipped due to not unable to connect RedisCache.")]
        public void GetValueFromRedisCache()
        {
            string value = "abc";
            string cacheResponse = "";
            "When Cache key-values are added to RedisCache".x(_ =>
            {
                cache.Add("key", value, DateTime.Now.AddSeconds(60));
            });
            "Then the value should return from cache".x(_ => {
                cacheResponse = cache.Get<string>("key");
                Assert.Equal(value, cacheResponse);
            });
            "Ensure caches are cleared".x(_ => {
                cache.RemoveStartsWith("key");
                cacheResponse = cache.Get<string>("key");
                Assert.Null(cacheResponse);

            });
        }

        [Scenario(Skip = "Skipped due to not unable to connect RedisCache.")]
        public void AddModelAndReadFromRedisCache()
        {
            "When Cache key-values are added to RedisCache".x(_ =>
            {
                cache.Add("profile", Data.Profiles[0], DateTime.Now.AddSeconds(60));
            });
            "Then the value should return from cache".x(_ => {
                var cacheResponse = cache.Get<Profile>("profile");
                Assert.True(Data.Profiles[0].Id == cacheResponse.Id);
                Assert.True(Data.Profiles[0].CompanyId == cacheResponse.CompanyId);
                Assert.True(Data.Profiles[0].Name == cacheResponse.Name);
            });
            "Ensure caches are cleared".x(_ => {
                cache.RemoveStartsWith("profile");
                var cacheResponse = cache.Get<object>("profile");
                Assert.Null(cacheResponse);

            });
        }
    }
}
