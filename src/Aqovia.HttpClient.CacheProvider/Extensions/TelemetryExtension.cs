using System;
using System.Collections.Generic;
using System.Text;

namespace Aqovia.HttpClient.CacheProvider.Extensions
{
    public static class TelemetryExtension
    {
        public static void AddIfNotExist(this IDictionary<string, string> dictionary, string key, string value)
        {
            if (dictionary.ContainsKey(key))
                dictionary.Remove(key);

            dictionary.Add(key, value);
        }


    }
}
