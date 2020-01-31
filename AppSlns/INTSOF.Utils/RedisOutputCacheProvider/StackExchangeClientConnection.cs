using System;
using System.Diagnostics;
using System.Web.SessionState;
using StackExchange.Redis;

namespace Intsof.RedisOutputCacheProvider
{
    internal class StackExchangeClientConnection : IRedisClientConnection
    {

        ProviderConfiguration _configuration;
        RedisUtility _redisUtility;
        RedisSharedConnection _sharedConnection;

        public StackExchangeClientConnection(ProviderConfiguration configuration, RedisUtility redisUtility, RedisSharedConnection sharedConnection)
        {
            _configuration = configuration;
            _redisUtility = redisUtility;
            _sharedConnection = sharedConnection;
        }

        // This is used just by tests
        public IDatabase RealConnection
        {
            get { return _sharedConnection.Connection; }
        }

        public bool Expiry(string key, int timeInSeconds)
        {
            TimeSpan timeSpan = new TimeSpan(0, 0, timeInSeconds);
            RedisKey redisKey = key;
            return (bool)RetryLogic(() => RealConnection.KeyExpire(redisKey,timeSpan));
        }

        public object Eval(string script, string[] keyArgs, object[] valueArgs)
        {
            RedisKey[] redisKeyArgs = new RedisKey[keyArgs.Length];
            RedisValue[] redisValueArgs = new RedisValue[valueArgs.Length];
            
            int i = 0;
            foreach (string key in keyArgs)
            {
                redisKeyArgs[i] = key;
                i++;
            }

            i = 0;
            foreach (object val in valueArgs)
            {
                if (val.GetType() == typeof(byte[]))
                {
                    // User data is always in bytes
                    redisValueArgs[i] = (byte[])val;
                }
                else
                {
                    // Internal data like session timeout and indexes are stored as strings
                    redisValueArgs[i] = val.ToString();
                }
                i++;
            }
            return RetryLogic(() => RealConnection.ScriptEvaluate(script, redisKeyArgs, redisValueArgs));
        }

        private object OperationExecutor(Func<object> redisOperation)
        {
            try
            {
                return redisOperation.Invoke();
            }
            catch (ObjectDisposedException)
            {
                // Try once as this can be caused by force reconnect by closing multiplexer
                return redisOperation.Invoke();
            }
            catch (RedisConnectionException)
            {
                // Try once after reconnect
                _sharedConnection.ForceReconnect();
                return redisOperation.Invoke();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("NOSCRIPT"))
                {
                    // Second call should pass if it was script not found issue
                    return redisOperation.Invoke();
                }
                throw;
            }
        }

        /// <summary>
        /// If retry timout is provide than we will retry first time after 20 ms and after that every 1 sec till retry timout is expired or we get value.
        /// </summary>
        private object RetryLogic(Func<object> redisOperation)
        {
            int timeToSleepBeforeRetryInMiliseconds = 20;
            DateTime startTime = DateTime.Now;
            while (true)
            {
                try
                {
                    return OperationExecutor(redisOperation);
                }
                catch (Exception e)
                {
                    TimeSpan passedTime = DateTime.Now - startTime;
                    if (_configuration.RetryTimeout < passedTime)
                    {
                        LogUtility.LogError($"Exception: {e.Message}");
                        throw;
                    }
                    else
                    {
                        int remainingTimeout = (int)(_configuration.RetryTimeout.TotalMilliseconds - passedTime.TotalMilliseconds);
                        // if remaining time is less than 1 sec than wait only for that much time and than give a last try
                        if (remainingTimeout < timeToSleepBeforeRetryInMiliseconds)
                        {
                            timeToSleepBeforeRetryInMiliseconds = remainingTimeout;
                        }
                    }

                    // First time try after 20 msec after that try after 1 second
                    System.Threading.Thread.Sleep(timeToSleepBeforeRetryInMiliseconds);
                    timeToSleepBeforeRetryInMiliseconds = 1000;
                }
            }
        }

        public bool IsLocked(object rowDataFromRedis)
        {
            RedisResult rowDataAsRedisResult = (RedisResult)rowDataFromRedis;
            RedisResult[] lockScriptReturnValueArray = (RedisResult[])rowDataAsRedisResult;
            Debug.Assert(lockScriptReturnValueArray != null);
            Debug.Assert(lockScriptReturnValueArray[3] != null);
            return (bool)lockScriptReturnValueArray[3];
        }

        public string GetLockId(object rowDataFromRedis)
        {
            return StackExchangeClientConnection.GetLockIdStatic(rowDataFromRedis);
        }

        internal static string GetLockIdStatic(object rowDataFromRedis)
        {
            RedisResult rowDataAsRedisResult = (RedisResult)rowDataFromRedis;
            RedisResult[] lockScriptReturnValueArray = (RedisResult[])rowDataAsRedisResult;
            Debug.Assert(lockScriptReturnValueArray != null);
            return (string)lockScriptReturnValueArray[0];
        }


        public void Set(string key, byte[] data, DateTime utcExpiry)
        {
            RedisKey redisKey = key;
            RedisValue redisValue = data;
            TimeSpan timeSpanForExpiry = utcExpiry - DateTime.UtcNow;
            OperationExecutor(() => RealConnection.StringSet(redisKey, redisValue, timeSpanForExpiry));
        }

        public void StringSet(string key, string data, DateTime utcExpiry)
        {
            RedisKey redisKey = key;
            RedisValue redisValue = data;
            TimeSpan timeSpanForExpiry = utcExpiry - DateTime.UtcNow;
            OperationExecutor(() => RealConnection.StringSet(redisKey, redisValue, timeSpanForExpiry));
        }

        public byte[] Get(string key)
        {
            RedisKey redisKey = key;
            RedisValue redisValue = (RedisValue) OperationExecutor(() => RealConnection.StringGet(redisKey));
            return (byte[]) redisValue;
        }

        public string StringGet(string key)
        {
            RedisKey redisKey = key;
            RedisValue redisValue = (RedisValue)OperationExecutor(() => RealConnection.StringGet(redisKey));
            return (string)redisValue;
        }

        public void Remove(string key)
        {
            RedisKey redisKey = key;
            OperationExecutor(() => RealConnection.KeyDelete(redisKey));
        }

        public void RemoveAll(string keyPattern)
        {
            var redisKeys = _sharedConnection.GetKeys(keyPattern);
            if (redisKeys != null)
                foreach (RedisKey redisKey in redisKeys)
                {
                    OperationExecutor(() => RealConnection.KeyDelete(redisKey));
                }
        }

        public bool KeyExists(string key)
        {
           RedisKey redisKey = key;
           return Convert.ToBoolean(OperationExecutor(() => RealConnection.KeyExists(redisKey)));
        }

        public byte[] GetOutputCacheDataFromResult(object rowDataFromRedis) 
        {
            RedisResult rowDataAsRedisResult = (RedisResult)rowDataFromRedis;
            return (byte[]) rowDataAsRedisResult;
        }
    }
}
