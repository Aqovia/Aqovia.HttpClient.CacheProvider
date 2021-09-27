using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Aqovia.HttpClient.CacheProvider.Cache
{
   public interface ICacheOutput
   {


       T Get<T>(string key);

        void Remove(string key); 
        void RemoveStartsWith(string key);

        bool Contains(string key);

        void Add(string key, object o, DateTimeOffset expiration, string dependsOnKey = null, IOperationHolder<DependencyTelemetry> operation =null);

        IEnumerable<string> AllKeys { get; }
         
    }
}
