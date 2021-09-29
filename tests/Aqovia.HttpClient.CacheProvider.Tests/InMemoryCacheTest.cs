using Aqovia.HttpClient.CacheProvider.Cache;
using System;
using Xunit;
using Xbehave;
using System.Collections.Generic;
using System.Linq;
using Aqovia.HttpClient.CacheProvider.Tests.Abstraction;

namespace Aqovia.HttpClient.CacheProvider.Tests
{
    public class InMemoryCacheTest
    { 
        private IEnumerable<string> allKeys = null; 
        private ICacheOutput cache = new InMemoryCache();
        [Scenario]
        public void AddCache_And_Return_Allkeys_FromCache()
        {
            
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
            "Ensure caches are cleared".x(_=> {
                cache.RemoveStartsWith("base");
                allKeys = cache.AllKeys;
                Assert.True(allKeys.Count() == 0);
            });
            
        }
       
        [Scenario]
        public void GetValueFromCache()
        {
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
            "Ensure caches are cleared".x(_ => {
                cache.RemoveStartsWith("key");
                allKeys = cache.AllKeys;
                Assert.True(allKeys.Count() == 0);

            });
        }

        [Scenario]
        public void AddModelAndReadFromCache()
        {
            "When Cache key-values are added to InMemoryCache".x(_ =>
            {
                cache.Add("profile", Data.Profiles[0], DateTime.Now.AddSeconds(60));
            });
            "Then the value should return from cache".x(_ => {
               var cacheResponse = cache.Get<object>("profile");
               Assert.True(cacheResponse.GetType()==typeof(Profile));
                Assert.Equal(Data.Profiles[0], cacheResponse);
            });
            "Ensure caches are cleared".x(_ => {
                cache.RemoveStartsWith("profile");
                allKeys = cache.AllKeys;
                Assert.True(allKeys.Count() == 0);

            });
        }

    }
}
