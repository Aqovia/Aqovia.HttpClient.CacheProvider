using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Caching;
using System.Linq;
using Aqovia.HttpClient.CacheProvider.Extensions;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Newtonsoft.Json;

namespace Aqovia.HttpClient.CacheProvider.Cache
{
    public class InMemoryCache : ICacheOutput
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;
        public virtual T Get<T>(string key) 
        {
            var obj = Cache.Get(key);
            return (T)obj;
        }

        public virtual void Remove(string key)
        {
            lock (Cache)
            {
                Cache.Remove(key);
            }
        }

        public virtual void RemoveStartsWith(string key)
        {
            lock (Cache)
            {
                Cache.Remove(key);
            }
        }
        public virtual bool Contains(string key)
        {
            return Cache.Contains(key);
        }

        public virtual void Add(string key, object obj, DateTimeOffset expiration, string dependsOnKey = null,IOperationHolder<DependencyTelemetry> operation =null)
        {
            var cachePolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = expiration
            };

            if (!string.IsNullOrWhiteSpace(dependsOnKey))
            {
                cachePolicy.ChangeMonitors.Add(
                    Cache.CreateCacheEntryChangeMonitor(new[] { dependsOnKey })
                );
            }
            lock (Cache)
            { 
                operation?.Telemetry.Properties.AddIfNotExist(key,JsonConvert.SerializeObject(obj));
                Cache.Add(key, obj, cachePolicy);
            }
        }



        public virtual IEnumerable<string> AllKeys => Cache.Select(x => x.Key); 
    }
}
