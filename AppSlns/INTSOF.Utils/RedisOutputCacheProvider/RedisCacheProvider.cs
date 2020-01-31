using System;
using System.Web.Caching;

namespace Intsof.RedisOutputCacheProvider
{
    public class RedisCacheProvider
    {
        internal static ProviderConfiguration configuration;
        internal static object configurationCreationLock = new object();
        internal IOutputCacheConnection cache;
        
        public void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            
            // If configuration exists then use it otherwise read from config file and create one
            if (configuration == null)
            {
                lock (configurationCreationLock)
                {
                    if (configuration == null)
                    {
                        configuration = ProviderConfiguration.ProviderConfigurationForOutputCache(config);
                    }
                }
            }
        }

        public object Get(string key)
        {
            try
            {
                GetAccessToCacheStore();
                return cache.Get(key);
            }
            catch(Exception e)
            {
                LogUtility.LogError("Error in Get: " + e.Message);
            }
            return null;
        }

        public String StringGet(string key)
        {
            try
            {
                GetAccessToCacheStore();
                return cache.StringGet(key);
            }
            catch (Exception e)
            {
                LogUtility.LogError("Error in Get: " + e.Message);
            }
            return null;
        }

        public void Set(string key, object entry, DateTime utcExpiry)
        {
            try
            {
                GetAccessToCacheStore();
                cache.Set(key, entry, utcExpiry);
            }
            catch (Exception e)
            {
                LogUtility.LogError("Error in Set: " + e.Message);
            }
        }

        public void StringSet(string key, string entry, DateTime utcExpiry)
        {
            try
            {
                GetAccessToCacheStore();
                cache.StringSet(key, entry, utcExpiry);
            }
            catch (Exception e)
            {
                LogUtility.LogError("Error in Set: " + e.Message);
            }
        }


        public void Remove(string key)
        {
            try
            {
                GetAccessToCacheStore();
                cache.Remove(key);
            }
            catch (Exception e)
            {
                LogUtility.LogError("Error in Remove: " + e.Message);
            }
        }
        
        public void RemoveAll(String wildCardKey)
        {
            try
            {
                GetAccessToCacheStore();
                cache.RemoveAll(wildCardKey);
            }
            catch (Exception e)
            {
                LogUtility.LogError("Error in Remove All: " + e.Message);
            }
        }
        
        private void GetAccessToCacheStore()
        {
            if (cache == null)
            {
                cache = new RedisOutputCacheConnectionWrapper(configuration);
            }
        }
    }
}