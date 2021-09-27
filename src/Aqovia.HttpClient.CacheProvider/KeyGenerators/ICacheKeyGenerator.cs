using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aqovia.HttpClient.CacheProvider.KeyGenerators
{
    public interface ICacheKeyGenerator
    {
        string MakeCacheKey(HttpActionContext context, MediaTypeHeaderValue mediaType, bool excludeQueryString = false);
        string MakeCacheKey(ActionExecutingContext context, MediaTypeHeaderValue mediaType, bool excludeQueryString = false);
    }
}
