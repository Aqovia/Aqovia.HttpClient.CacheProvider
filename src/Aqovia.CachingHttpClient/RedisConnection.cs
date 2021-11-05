using System;
using System.Threading;
using StackExchange.Redis;

namespace Aqovia.CachingHttpClient
{
    public class RedisConnection
    {
        public static Lazy<ConnectionMultiplexer> Create(string connectionString)
        {
            return new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString),
                LazyThreadSafetyMode.PublicationOnly);
        }
    }
}