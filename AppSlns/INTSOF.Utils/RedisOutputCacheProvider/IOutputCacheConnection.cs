using System;

namespace Intsof.RedisOutputCacheProvider
{
    internal interface IOutputCacheConnection
    {
        void StringSet(string key, string entry, DateTime utcExpiry);
        void Set(string key, object entry, DateTime utcExpiry);
        object Get(string key);
        string StringGet(string key);
        void Remove(string key);
        void RemoveAll(string wildCardKey);
    }
}
