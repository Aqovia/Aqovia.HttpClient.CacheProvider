using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace Aqovia.HttpClient.CacheProvider.Demo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            config.Properties.TryAdd("RedisCacheConnectionString",
                ConfigurationManager.AppSettings["RedisCacheStoreConnectionString"]);

            config.Properties.TryAdd("AppInsightInstrumentationKey",
                ConfigurationManager.AppSettings["AppInsightInstrumentationKey"]);
        }
    }
}
