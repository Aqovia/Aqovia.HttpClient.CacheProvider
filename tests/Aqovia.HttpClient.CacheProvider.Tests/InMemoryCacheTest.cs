using Aqovia.HttpClient.CacheProvider.Cache;
using System;
using Xunit;
using Xbehave;
using System.Collections.Generic;
using System.Linq;

namespace Aqovia.HttpClient.CacheProvider.Tests
{
    public class InMemoryCacheTest
    {
        [Scenario]
        public void GetValueFromCache()
        {
            ICacheOutput cache = new InMemoryCache();
            string value = "abc";
            string cacheResponse = "";
            "When Cache key-values are added to InMemoryCache".x(_ =>
            {

                cache.Add("key", value, DateTime.Now.AddSeconds(60));
            });
            "Then the value should return from cache".x(_ => {
                cacheResponse = cache.Get<string>("key");
                Assert.Equal(value,cacheResponse);
            });

        }
        [Scenario]
        public void Return_Allkeys_FromCache()
        {
            ICacheOutput cache = new InMemoryCache();
            IEnumerable<string> allKeys = null; 
            "When Cache key-values are added to InMemoryCache".x(_ =>
            {

                cache.Add("base", "abc", DateTime.Now.AddSeconds(60));
                cache.Add("key1", "abc", DateTime.Now.AddSeconds(60), "base");
                cache.Add("key2", "abc", DateTime.Now.AddSeconds(60), "base");
                cache.Add("key3", "abc", DateTime.Now.AddSeconds(60), "base");
                allKeys = cache.AllKeys;
            });
            "Then it should return all keys".x(_=> {
                Assert.NotNull(allKeys);
                Assert.Equal(new[] { "base", "key1", "key2", "key3" }, allKeys.OrderBy(_=>_).ToArray());
            });
            
        }
    }
}
