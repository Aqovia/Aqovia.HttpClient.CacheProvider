using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Aqovia.HttpClient.CacheProvider.Extensions;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using StackExchange.Redis;

namespace Aqovia.HttpClient.CacheProvider.Cache
{
    public class RedisCache : ICacheOutput
    { 
        private readonly IDatabase Cache;
        static string _redisConnectionString = "127.0.0.1:6379,ssl=false"; 
        public RedisCache(string redisConnectionString)
        {
            _redisConnectionString = redisConnectionString;
            Cache = GetDatabase();
        }

        private static Lazy<ConnectionMultiplexer> _lazyConnection = CreateConnection(_redisConnectionString);

        public static ConnectionMultiplexer Connection => _lazyConnection.Value;

        private static Lazy<ConnectionMultiplexer> CreateConnection(string connectionstring)
        {
            return new Lazy<ConnectionMultiplexer>(() =>
            { 
                return ConnectionMultiplexer.Connect(connectionstring);
            }, LazyThreadSafetyMode.PublicationOnly);
        }

        private static long _lastReconnectTicks = DateTimeOffset.MinValue.UtcTicks;
        private static DateTimeOffset _firstErrorTime = DateTimeOffset.MinValue;
        private static DateTimeOffset _previousErrorTime = DateTimeOffset.MinValue;

        private static readonly object ReconnectLock = new object();

        // In general, let StackExchange.Redis handle most reconnects,
        // so limit the frequency of how often ForceReconnect() will
        // actually reconnect.
        private static TimeSpan ReconnectMinFrequency => TimeSpan.FromSeconds(60);

        // If errors continue for longer than the below threshold, then the
        // multiplexer seems to not be reconnecting, so ForceReconnect() will
        // re-create the multiplexer.
        private static TimeSpan ReconnectErrorThreshold => TimeSpan.FromSeconds(30);

        private static int RetryMaxAttempts => 5;


        private static void CloseConnection(Lazy<ConnectionMultiplexer> oldConnection)
        {
            if (oldConnection == null)
                return;

            try
            {
                oldConnection.Value.Close();
            }
            catch (Exception)
            {
                // Example error condition: if accessing oldConnection.Value causes a connection attempt and that fails.
            }
        }

        /// <summary>
        /// Force a new ConnectionMultiplexer to be created.
        /// NOTES:
        ///     1. Users of the ConnectionMultiplexer MUST handle ObjectDisposedExceptions, which can now happen as a result of calling ForceReconnect().
        ///     2. Don't call ForceReconnect for Timeouts, just for RedisConnectionExceptions or SocketExceptions.
        ///     3. Call this method every time you see a connection exception. The code will:
        ///         a. wait to reconnect for at least the "ReconnectErrorThreshold" time of repeated errors before actually reconnecting
        ///         b. not reconnect more frequently than configured in "ReconnectMinFrequency"
        /// </summary>
        private static void ForceReconnect()
        {
            var utcNow = DateTimeOffset.UtcNow;
            var previousTicks = Interlocked.Read(ref _lastReconnectTicks);
            var previousReconnectTime = new DateTimeOffset(previousTicks, TimeSpan.Zero);
            var elapsedSinceLastReconnect = utcNow - previousReconnectTime;

            // If multiple threads call ForceReconnect at the same time, we only want to honor one of them.
            if (elapsedSinceLastReconnect < ReconnectMinFrequency)
                return;

            lock (ReconnectLock)
            {
                utcNow = DateTimeOffset.UtcNow;
                elapsedSinceLastReconnect = utcNow - previousReconnectTime;

                if (_firstErrorTime == DateTimeOffset.MinValue)
                {
                    // We haven't seen an error since last reconnect, so set initial values.
                    _firstErrorTime = utcNow;
                    _previousErrorTime = utcNow;
                    return;
                }

                if (elapsedSinceLastReconnect < ReconnectMinFrequency)
                    return; // Some other thread made it through the check and the lock, so nothing to do.

                var elapsedSinceFirstError = utcNow - _firstErrorTime;
                var elapsedSinceMostRecentError = utcNow - _previousErrorTime;

                var shouldReconnect =
                    elapsedSinceFirstError >= ReconnectErrorThreshold // Make sure we gave the multiplexer enough time to reconnect on its own if it could.
                    && elapsedSinceMostRecentError <= ReconnectErrorThreshold; // Make sure we aren't working on stale data (e.g. if there was a gap in errors, don't reconnect yet).

                // Update the previousErrorTime timestamp to be now (e.g. this reconnect request).
                _previousErrorTime = utcNow;

                if (!shouldReconnect)
                    return;

                _firstErrorTime = DateTimeOffset.MinValue;
                _previousErrorTime = DateTimeOffset.MinValue;

                var oldConnection = _lazyConnection;
                CloseConnection(oldConnection);
                _lazyConnection = CreateConnection(_redisConnectionString);
                Interlocked.Exchange(ref _lastReconnectTicks, utcNow.UtcTicks);
            }
        }


        public static readonly RetryPolicy RedisRetryPolicy = Policy
            .Handle<RedisConnectionException>()
            .Or<SocketException>()
            .Or<ObjectDisposedException>()
            .Retry(RetryMaxAttempts, (exception, _) =>
            {
                if (exception is ObjectDisposedException) return;
                ForceReconnect();
            });

        private IEnumerable<string> _allKeys;

        public static IDatabase GetDatabase()
        {
            return RedisRetryPolicy.Execute(() => Connection.GetDatabase());
        }

        public static System.Net.EndPoint[] GetEndPoints()
        {
            return RedisRetryPolicy.Execute(() => Connection.GetEndPoints());
        }
        public static IServer GetServer(string host, int port)
        {
            return RedisRetryPolicy.Execute(() => Connection.GetServer(host, port));
        }

        public T Get<T>(string key) 
        {
            var result = RedisRetryPolicy.Execute(() => Cache.StringGet(key));
            result = result.IsNullOrEmpty ? new RedisValue("") : result;
            return  JsonConvert.DeserializeObject < T > (result);
        }

        public void Remove(string key)
        {
            RedisRetryPolicy.Execute(() => Cache.KeyDelete(key));
        }

        public void RemoveStartsWith(string key)
        {
            Remove(key);
        }

        public bool Contains(string key)
        {
            return RedisRetryPolicy.Execute(() => Cache.KeyExists(key));
        }

        public void Add(string key, object obj, DateTimeOffset expiration, string dependsOnKey = null,IOperationHolder<DependencyTelemetry> operation =null)
        {
            operation?.Telemetry.Properties.AddIfNotExist(key, JsonConvert.SerializeObject(obj));
            var redisValue =new RedisValue(JsonConvert.SerializeObject(obj));
            RedisRetryPolicy.Execute(() => Cache.StringSet(key, redisValue , expiration.Offset, When.Always));
        }

        IEnumerable<string> ICacheOutput.AllKeys
        {
            get
            {
                var endpoint = (System.Net.DnsEndPoint)GetEndPoint()[0];
                IServer server = GetServer(endpoint.Host, endpoint.Port);
                return server.Keys().Select(_ => _.ToString());
            }
        }
        private static System.Net.EndPoint[] GetEndPoint()
        {
            return RedisRetryPolicy.Execute(() => Connection.GetEndPoints());
        }
        public TelemetryClient TelemetryClient { get; set; }
    }
}
