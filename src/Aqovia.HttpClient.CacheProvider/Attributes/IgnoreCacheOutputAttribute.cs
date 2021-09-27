using System;

namespace Aqovia.HttpClient.CacheProvider
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class IgnoreCacheOutputAttribute : Attribute
    {
    }
}