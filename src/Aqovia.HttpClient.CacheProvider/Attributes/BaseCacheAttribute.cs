using Aqovia.HttpClient.CacheProvider.Extensions;
using System.Net.Http;
using System.Web.Http;
using Aqovia.HttpClient.CacheProvider.Cache;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace Aqovia.HttpClient.CacheProvider
{
    public abstract class BaseCacheAttribute : ActionFilterAttribute
    {
        // cache repository
        protected ICacheOutput InxnCache;

        protected virtual void EnsureCache(HttpConfiguration config, HttpRequestMessage req)
        {
            InxnCache = config.CacheOutputConfiguration().GetCacheOutputProvider(req);
        }
    }
}