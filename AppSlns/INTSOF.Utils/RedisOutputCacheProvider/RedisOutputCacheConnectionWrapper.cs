using System;

namespace Intsof.RedisOutputCacheProvider
{
    internal class RedisOutputCacheConnectionWrapper : IOutputCacheConnection
    {
        internal static RedisSharedConnection sharedConnection;
        static object lockForSharedConnection = new object();
        internal static RedisUtility redisUtility;

        internal IRedisClientConnection redisConnection;
        ProviderConfiguration configuration;
        
        public RedisOutputCacheConnectionWrapper(ProviderConfiguration configuration)
        {
            this.configuration = configuration;

            // only single object of RedisSharedConnection will be created and then reused
            if (sharedConnection == null)
            {
                lock (lockForSharedConnection)
                {
                    if (sharedConnection == null)
                    {
                        sharedConnection = new RedisSharedConnection(configuration);
                        redisUtility = new RedisUtility(configuration);
                    }
                }
            }
            redisConnection = new StackExchangeClientConnection(configuration, redisUtility, sharedConnection);
        }

/*-------Start of Add operation-----------------------------------------------------------------------------------------------------------------------------------------------*/
        // KEYS = { key }
        // ARGV = { page data, expiry time in miliseconds } 
        // retArray = { page data from cache or new }
        static readonly string addScript = (@"
                    local retVal = redis.call('GET',KEYS[1])
                    if retVal == false then
                       redis.call('PSETEX',KEYS[1],ARGV[2],ARGV[1])
                       retVal = ARGV[1]
                    end
                    return retVal
                    ");
        static readonly string deleteScript = (@"
                    ""return redis.call('del', 'defaultKey', unpack(redis.call('keys', KEYS[1])))"" 0 prefix:*");

        public object Add(string key, object entry, DateTime utcExpiry)
        {
            key = GetKeyForRedis(key);
            TimeSpan expiryTime = utcExpiry - DateTime.UtcNow;
            string[] keyArgs = new string[] { key };
            object[] valueArgs = new object[] { redisUtility.GetBytesFromObject(entry), (long) expiryTime.TotalMilliseconds };

            object rowDataFromRedis = redisConnection.Eval(addScript, keyArgs, valueArgs);
            return redisUtility.GetObjectFromBytes(redisConnection.GetOutputCacheDataFromResult(rowDataFromRedis));
        }

        public void RemoveAll(string wildCardkey)
        {
            wildCardkey = GetKeyForRedis(wildCardkey);
            redisConnection.RemoveAll(wildCardkey);
        }

        public void Set(string key, object entry, DateTime utcExpiry)
        {
            key = GetKeyForRedis(key);
            byte[] data = redisUtility.GetBytesFromObject(entry);
            redisConnection.Set(key, data, utcExpiry);
        }

        public object Get(string key)
        {
            key = GetKeyForRedis(key);
            byte[] data = redisConnection.Get(key);
            return redisUtility.GetObjectFromBytes(data);
        }

        public void Remove(string key)
        {
            key = GetKeyForRedis(key);
            redisConnection.Remove(key);
        }

        private string GetKeyForRedis(string key)
        {
            return configuration.ApplicationName + "_" + key;
        }

        public void StringSet(string key, string entry, DateTime utcExpiry)
        {
            key = GetKeyForRedis(key);
            redisConnection.StringSet(key, entry, utcExpiry);
        }

        public string StringGet(string key)
        {
            key = GetKeyForRedis(key);
            return redisConnection.StringGet(key);
        }
    }
}
