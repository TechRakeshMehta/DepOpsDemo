using System;

namespace Intsof.RedisOutputCacheProvider
{
    internal interface IRedisClientConnection
    {
        bool Expiry(string key, int timeInSeconds);
        object Eval(string script, string[] keyArgs, object[] valueArgs);
        string GetLockId(object rowDataFromRedis);
        bool IsLocked(object rowDataFromRedis);

        bool KeyExists(string key);

        void RemoveAll(string keyPattern);

        void Set(string key, byte[] data, DateTime utcExpiry);

        void StringSet(string key, string data, DateTime utcExpiry);

        byte[] Get(string key);
        string StringGet(string key);

        void Remove(string key);
        byte[] GetOutputCacheDataFromResult(object rowDataFromRedis);
    }
}
