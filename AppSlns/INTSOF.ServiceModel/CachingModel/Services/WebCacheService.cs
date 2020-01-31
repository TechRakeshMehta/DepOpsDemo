#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXAppFabricCacheService.cs
// Purpose:   SysXAppFabricCacheService
//

#endregion
#region Namespace
using CoreWeb.IntsofCachingModel.Interface.Services;
using INTSOF.AppFabricCacheServer;
using INTSOF.Logger;
using INTSOF.Logger.factory;
using INTSOF.Utils;
using Microsoft.ApplicationServer.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace CoreWeb.IntsofCachingModel.Services
{
    public class SysXAppFabricCacheService : ISysXAppFabricCacheService
    {
        #region Private Variables

       
        private static DataCacheFactory _factory;
        private static DataCache dataCache;
        private static readonly ReaderWriterLockSlim CacheLockTableLocker = new ReaderWriterLockSlim();
        private List<CacheLockEntry> _cacheLockTable;
        private IList<CacheTimeoutEntity> _cacheTimeoutEntity;
        private static readonly ReaderWriterLockSlim NamedCacheThreadTableLocker = new ReaderWriterLockSlim();
        private Dictionary<int, string> _namedCacheThreadTable;
        private ILogger _logger;
        ICacheManager cacheManager;

        #endregion

        public SysXAppFabricCacheService()
        {
            try
            {
                _logger = SysXLoggerFactory.GetInstance().GetLogger();
                this._cacheLockTable = new List<CacheLockEntry>();
                this._namedCacheThreadTable = new Dictionary<int, string>();
                //Set Cache Timeout for specific Entities
                this._cacheTimeoutEntity = new CacheTimeoutEntity().SetCacheTimeoutEntity();

                if (SysXCacheUtils.CacheType.Equals(SysXCachingConst.CACHETYPEAPPFABRIC, StringComparison.CurrentCultureIgnoreCase))
                {
                    SetCache();
                }
                else if (SysXCacheUtils.CacheType.Equals(SysXCachingConst.CACHETYPESYSTEM, StringComparison.CurrentCultureIgnoreCase))
                {
                    cacheManager = CacheFactory.GetCacheManager();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        #region  Add Methods

        /// <summary>
        /// Adds an object to the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">Serializable object</param>
        /// <returns></returns>
        public bool Add(string key, object value)
        {
            var result = false;
            try
            {
                if (dataCache != null)
                {
                    dataCache.Add(key, value);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// Adds an object to the system default cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"> object</param>
        /// <returns></returns>
        public bool AddSystemCache(string key, object value)
        {
            var result = false;
            try
            {
                //Set Cache Timeout for specific Entities
                this._cacheTimeoutEntity.Where(con => key.ToLower().StartsWith(con.EntityName.ToLower()))
                    .ForEach(x =>
                {   
                    //Creating Absolute Time Expiration
                    Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.AbsoluteTime _AbsoulteTime =
                             new Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.AbsoluteTime(x.EntityTimeout);
                    // Using ICacheItemExpiration To Set multiple Cache Expiration policy
                    cacheManager.Add(key, value, Microsoft.Practices.EnterpriseLibrary.Caching.CacheItemPriority.Normal, null,
                        new ICacheItemExpiration[] { _AbsoulteTime });
                    result = true;
                    return;
                });
                if (result == false)
                {
                    result = true;
                    cacheManager.Add(key, value);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Adds an object to the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">Serializable object</param>
        ///     /// <param name="timeout">Time out value</param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan timeout)
        {
            var result = false;
            try
            {
                if (dataCache != null)
                {
                    dataCache.Add(key, value, timeout);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Adds an object to the cache in specified Region
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public bool Add(string key, object value, string regionName)
        {
            var result = false;
            try
            {
                if (dataCache != null)
                {
                    dataCache.Add(key, value, regionName);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;

        }

        /// <summary>
        /// Adds an object to the cache in region and sets time out
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="regionName"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool Add(string key, object value, string regionName, TimeSpan timeout)
        {
            var result = false;
            try
            {

                if (string.IsNullOrEmpty(regionName))
                    throw new ArgumentNullException("regionName");

                if (dataCache != null)
                {
                    dataCache.Add(key, value, timeout, regionName);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;



        }

        /// <summary>
        /// Adds an object to the cache with cache Tags
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serializable Object</param>
        /// <param name="cacheTags">Cache Tags</param>
        /// <returns></returns>
        public bool Add(string key, object value, IEnumerable<string> cacheTags)
        {
            var result = false;
            try
            {

                if (dataCache != null)
                {
                    var dataCacheTags = cacheTags.Select(cacheTag => new DataCacheTag(cacheTag));

                    if (dataCacheTags.Count() > 0)
                        dataCache.Add(key, value, dataCacheTags);
                    else
                        dataCache.Add(key, value);

                    result = true;
                }


            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Adds an object to the cache with cache Tags in specified Region 
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serializable Object</param>
        /// <param name="cacheTags">Cache Tags</param>
        /// <param name="regionName">Cache Region</param>
        /// <returns></returns>
        public bool Add(string key, object value, IEnumerable<string> cacheTags, string regionName)
        {
            var result = false;
            try
            {
                if (dataCache != null)
                {
                    var dataCacheTags = cacheTags.Select(cacheTag => new DataCacheTag(cacheTag));
                    dataCache.Add(key, value, dataCacheTags, regionName);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 
        ///  Adds an object to the cache with cache Tags and time out settings>
        /// <param name="key">Key</param>
        /// <param name="value">Serializable Object</param>
        /// <param name="cacheTags">Cache Tags</param>
        /// <param name="cacheTags"></param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan timeout, IEnumerable<string> cacheTags)
        {
            var result = false;
            try
            {
                if (dataCache != null)
                {
                    var dataCacheTags = cacheTags.Select(cacheTag => new DataCacheTag(cacheTag));
                    if (dataCacheTags.Count() > 0)
                        dataCache.Add(key, value, timeout, dataCacheTags);
                    else
                        dataCache.Add(key, value, timeout);

                    result = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Adds an object to the cache with cache Tags in specific region with time out settings>
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serializable Object</param>
        /// <param name="cacheTags">Cache Tags</param>
        /// <param name="timeout">Time Out</param>
        /// <param name="regionName">Region</param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan timeout, IEnumerable<string> cacheTags, string regionName)
        {

            if (string.IsNullOrEmpty(regionName))
                throw new ArgumentException("Tags may only be used to retrieve a cached object if that object is stored in a region. Hence,parameter regionName must have a value");
            var result = false;
            try
            {



                if (dataCache != null)
                {
                    var dataCacheTags = cacheTags.Select(cacheTag => new DataCacheTag(cacheTag));

                    dataCache.Add(key, value, timeout, dataCacheTags, regionName);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);

            }
            return result;
        }

        #endregion

        #region Get Methods

        /// <summary>
        /// Get Object from Cache
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <returns>Object</returns>
        public object Get(string key)
        {
            try
            {
                return dataCache.Get(key);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Get Generic Object from Cache
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <returns>Object of type <typeparam name="T"></typeparam></returns>
        public T Get<T>(string key)
        {
            T result = default(T);
            try
            {
                if (dataCache != null)
                {
                    result = (T)dataCache.Get(key);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Get Generic Object from System Cache
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <returns>Object of type <typeparam name="T"></typeparam></returns>

        public T GetSystemCache<T>(string key)
        {
            T result = default(T);
            try
            {
                result = (T)cacheManager.GetData(key);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }
        /// <summary>
        /// Get Dictonary for Cache Objects for Keys Array
        /// </summary>
        /// <param name="keys">Array of Key</param>
        /// <returns>Key/Object Pair</returns>
        public IDictionary<string, object> Get(params string[] keys)
        {
            Dictionary<string, object> result = null;
            try
            {
                if (dataCache != null)
                {
                    result = new Dictionary<string, object>();
                    foreach (var key in keys)
                    {
                        var keyValue = dataCache.Get(key);
                        result.Add(key, keyValue);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }
        /// <summary>
        /// Get object from cache and Lock the object 
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="timeout">time out value</param>
        /// <param name="lockId">lock id</param>
        /// <returns>Object</returns>
        public object GetAndLock(string key, TimeSpan timeout, out Guid lockId)
        {
            object result = null;
            lockId = Guid.Empty;

            try
            {
                if (dataCache != null)
                {
                    DataCacheLockHandle lockHandle;
                    result = dataCache.GetAndLock(key, timeout, out lockHandle);

                    var lockEntry = new CacheLockEntry(lockHandle);
                    this.AddToCacheLockTable(lockEntry);
                    lockId = lockEntry.LockId;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Get object from cache and Lock the object 
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="timeout">time out value</param>
        /// <param name="lockId">lock id</param>
        /// <param name="regionName">regionName</param>
        /// <returns>Object</returns>
        public object GetAndLock(string key, TimeSpan timeout, out Guid lockId, string regionName)
        {
            object result = null;
            lockId = Guid.Empty;
            try
            {
                if (dataCache != null)
                {
                    DataCacheLockHandle lockHandle;
                    result = dataCache.GetAndLock(key, timeout, out lockHandle, regionName);
                    var lockEntry = new CacheLockEntry(lockHandle);

                    this.AddToCacheLockTable(lockEntry);
                    lockId = lockEntry.LockId;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Get object from cache and Lock the object 
        /// </summary>
        /// <param name="key">cache key</param>
        /// <returns>Object</returns>
        public object Get(string key, string regionName)
        {

            object result = null;
            try
            {
                if (dataCache != null)
                {
                    result = dataCache.Get(key, regionName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Get object from cache filtered by cache tags
        /// </summary>
        /// <param name="cacheTags">Cache Tags</param>
        /// <param name="regionName">Region</param>
        /// <returns>Key/Pair for Cache key and Object</returns>
        public IEnumerable<KeyValuePair<string, object>> GetObjectsByAllTags(IEnumerable<string> cacheTags, string regionName)
        {
            IEnumerable<KeyValuePair<string, object>> result = null;
            try
            {
                if (dataCache != null)
                {
                    var dataCacheTags = cacheTags.Select(cacheTag => new DataCacheTag(cacheTag));
                    result = dataCache.GetObjectsByAllTags(dataCacheTags, regionName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Get object from cache with any cache tag
        /// </summary>
        /// <param name="cacheTags">Cache Tags</param>
        /// <param name="regionName">Region</param>
        /// <returns>Key/Pair for Cache key and Object</returns>
        public IEnumerable<KeyValuePair<string, object>> GetObjectsByAnyTag(IEnumerable<string> cacheTags, string regionName)
        {
            IEnumerable<KeyValuePair<string, object>> result = null;
            try
            {
                if (dataCache != null)
                {
                    var dataCacheTags = cacheTags.Select(cacheTag => new DataCacheTag(cacheTag));
                    result = dataCache.GetObjectsByAnyTag(dataCacheTags, regionName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Get object from cache with specified cache tag
        /// </summary>
        /// <param name="cacheTag">Cache Tag</param>
        /// <param name="regionName">Region</param>
        /// <returns>Key/Pair for Cache key and Object</returns>
        public IEnumerable<KeyValuePair<string, object>> GetObjectsByTag(string cacheTag, string regionName)
        {
            IEnumerable<KeyValuePair<string, object>> result = null;
            try
            {
                if (dataCache != null)
                {
                    var dataCacheTag = new DataCacheTag(cacheTag);
                    result = dataCache.GetObjectsByTag(dataCacheTag, regionName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Get object from cache with specified region
        /// </summary>
        /// <param name="regionName">Region</param>
        /// <returns>Key/Pair for Cache key and Object</returns>
        public IEnumerable<KeyValuePair<string, object>> GetObjectsInRegion(string regionName)
        {
            IEnumerable<KeyValuePair<string, object>> result = null;
            try
            {
                if (dataCache != null)
                {
                    result = dataCache.GetObjectsInRegion(regionName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Get Region Name of cached object
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>region name</returns>
        public string GetSystemRegionName(string key)
        {
            string result = null;
            try
            {

                if (dataCache != null)
                {
                    result = dataCache.GetSystemRegionName(key);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Get all Regions name
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetSystemRegions()
        {
            IEnumerable<string> result = null;
            try
            {
                if (dataCache != null)
                {
                    result = dataCache.GetSystemRegions();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }
        /// <summary>
        /// Check Cache Version is same or not
        /// </summary>
        /// <param name="earlierVersion">earlierVersion</param>
        /// <param name="key">Key</param>
        /// <param name="regionName">region</param>
        /// <returns>True/False</returns>
        public bool IsCacheVersionSame(object earlierVersion, string key, string regionName)
        {
            DataCacheItem dataCacheItem = null;

            try
            {
                if (dataCache != null)
                {
                    if (!string.IsNullOrEmpty(regionName))
                    {
                        dataCacheItem = dataCache.GetCacheItem(key, regionName);

                    }
                    else
                        dataCacheItem = dataCache.GetCacheItem(key);
                }

                if (dataCacheItem != null)
                {
                    var currentCacheItemVersion = dataCacheItem.Version;

                    var earlierCacheItemVersion = GetDataCacheItemVersion(earlierVersion);

                    if (currentCacheItemVersion.Equals(earlierCacheItemVersion))
                        return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return false;
        }

        #endregion

        #region Put Methods

        /// <summary>
        /// Put Object in Cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        /// <returns>Item version</returns>
        public DataCacheItemVersion Put(string key, object value)
        {
            DataCacheItemVersion result = null;
            try
            {
                if (dataCache != null)
                {
                    result = dataCache.Put(key, value);
                }
            }

            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }
        /// <summary>
        /// Put Object in Cache with Timeout
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        ///  <param name="timeout">timeout</param>
        /// <returns>Item version</returns>

        public DataCacheItemVersion Put(string key, object value, TimeSpan timeout)
        {
            DataCacheItemVersion result = null;

            try
            {
                if (dataCache != null)
                {
                    result = dataCache.Put(key, value, timeout);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;

        }

        /// <summary>
        /// Put Object in Cache in Region with Timeout
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        ///  <param name="timeout">timeout</param>
        /// <param name="regionName">region</param>
        /// <returns>Item version</returns>
        public DataCacheItemVersion Put(string key, object value, TimeSpan timeout, string regionName)
        {
            DataCacheItemVersion result = null;
            try
            {
                if (dataCache != null)
                {
                    result = dataCache.Put(key, value, timeout, regionName);
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Put Object in Cache along with Tags in Region with Timeout
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        ///  <param name="timeout">timeout</param>
        ///  <param name="cacheTags">cacheTags</param>
        /// <param name="regionName">region</param>
        /// <returns>Item version</returns>
        public DataCacheItemVersion Put(string key, object value, TimeSpan timeout, IEnumerable<string> cacheTags, string regionName)
        {
            DataCacheItemVersion result = null;

            try
            {
                if (dataCache != null)
                {
                    var dataCacheTags = cacheTags.Select(cacheTag => new DataCacheTag(cacheTag));

                    result = dataCache.Put(key, value, timeout, dataCacheTags, regionName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }
        /// <summary>
        /// Put Object in Cache along with Tags with Timeout value
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        ///  <param name="timeout">timeout</param>
        ///  <param name="cacheTags">cacheTags</param>
        /// <returns>Item version</returns>
        public DataCacheItemVersion Put(string key, object value, TimeSpan timeout, IEnumerable<string> cacheTags)
        {
            DataCacheItemVersion result = null;

            try
            {
                if (dataCache != null)
                {
                    var dataCacheTags = cacheTags.Select(cacheTag => new DataCacheTag(cacheTag));

                    result = dataCache.Put(key, value, timeout, dataCacheTags);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Put Object in Cache in Region
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        /// <param name="regionName">region</param>
        /// <returns>Item version</returns>
        public DataCacheItemVersion Put(string key, object value, string regionName)
        {
            DataCacheItemVersion result = null;
            try
            {
                if (dataCache != null)
                {
                    result = dataCache.Put(key, value, regionName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;

        }

        /// <summary>
        /// Put Object in Cache along with Tags with Timeout value in Region
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        ///  <param name="timeout">timeout</param>
        ///  <param name="cacheTags">cacheTags</param>
        ///   <param name="regionName">region</param>
        /// <returns>Item version</returns>
        public DataCacheItemVersion Put(string key, object value, IEnumerable<string> cacheTags, string regionName)
        {
            DataCacheItemVersion result = null;

            try
            {
                if (dataCache != null)
                {
                    var dataCacheTags = cacheTags.Select(cacheTag => new DataCacheTag(cacheTag));

                    result = dataCache.Put(key, value, dataCacheTags, regionName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;

        }
        /// <summary>
        /// Put Object in Cache along with Tags
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        ///  <param name="cacheTags">cacheTags</param>
        ///   <param name="regionName">region</param>
        ///   <returns>Item version</returns>
        public DataCacheItemVersion Put(string key, object value, IEnumerable<string> cacheTags)
        {
            DataCacheItemVersion result = null;

            try
            {
                if (dataCache != null)
                {
                    var dataCacheTags = cacheTags.Select(cacheTag => new DataCacheTag(cacheTag));

                    result = dataCache.Put(key, value, dataCacheTags);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;

        }

        /// <summary>
        /// Put Object in Cache and unlock the cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        ///  <param name="lockId">lockId</param>
        ///   <returns></returns>
        public bool PutAndUnlock(string key, object value, Guid lockId)
        {
            bool result = false;

            CacheLockEntry cacheLockEntry;

            try
            {
                if (dataCache != null)
                {
                    try
                    {
                        CacheLockTableLocker.EnterReadLock();
                        cacheLockEntry = (from lockHandleEntry in this._cacheLockTable
                                          where lockHandleEntry.LockId.Equals(lockId)
                                          select lockHandleEntry).FirstOrDefault();

                    }
                    finally
                    {
                        CacheLockTableLocker.ExitReadLock();
                    }

                    if (cacheLockEntry != null)
                    {
                        dataCache.PutAndUnlock(key, value, cacheLockEntry.LockHandle);

                        try
                        {
                            CacheLockTableLocker.EnterWriteLock();
                            this._cacheLockTable.Remove(cacheLockEntry);
                            result = true;
                        }
                        finally
                        {
                            CacheLockTableLocker.ExitWriteLock();
                        }
                    }
                    else
                    {
                        throw new ArgumentException("lockId");
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// Put Object in Cache with Tags and unlock the cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        ///  <param name="lockId">lockId</param>
        /// <param name="cacheTags">cacheTags</param>
        ///   <returns></returns>
        public bool PutAndUnlock(string key, object value, Guid lockId, IEnumerable<string> cacheTags)
        {
            bool result = false;
            CacheLockEntry cacheLockEntry;
            if (dataCache != null)
            {
                try
                {
                    CacheLockTableLocker.EnterReadLock();
                    cacheLockEntry = (from lockHandleEntry in this._cacheLockTable
                                      where lockHandleEntry.LockId.Equals(lockId)
                                      select lockHandleEntry).FirstOrDefault();

                }
                finally
                {
                    CacheLockTableLocker.ExitReadLock();
                }

                if (cacheLockEntry != null)
                {
                    var dataCacheTags = cacheTags.Select(cacheTag => new DataCacheTag(cacheTag));

                    dataCache.PutAndUnlock(key, value, cacheLockEntry.LockHandle, dataCacheTags);

                    try
                    {
                        CacheLockTableLocker.EnterWriteLock();
                        this._cacheLockTable.Remove(cacheLockEntry);
                        result = true;
                    }
                    finally
                    {
                        CacheLockTableLocker.ExitWriteLock();
                    }
                }
                else
                {
                    throw new ArgumentException("lockId");
                }
            }

            return result;
        }
        /// <summary>
        /// Put Object in Cache with Tags and unlock the cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        ///  <param name="lockId">lockId</param>
        /// <param name="cacheTags">cacheTags</param>
        ///   <param name="regionName">regionName</param>
        ///   <returns></returns>
        public bool PutAndUnlock(string key, object value, Guid lockId, IEnumerable<string> cacheTags, string regionName)
        {
            bool result = false;
            CacheLockEntry cacheLockEntry;
            if (dataCache != null)
            {
                try
                {
                    CacheLockTableLocker.EnterReadLock();
                    cacheLockEntry = (from lockHandleEntry in this._cacheLockTable
                                      where lockHandleEntry.LockId.Equals(lockId)
                                      select lockHandleEntry).FirstOrDefault();

                }
                finally
                {
                    CacheLockTableLocker.ExitReadLock();
                }

                if (cacheLockEntry != null)
                {

                    var dataCacheTags = cacheTags.Select(cacheTag => new DataCacheTag(cacheTag));

                    dataCache.PutAndUnlock(key, value, cacheLockEntry.LockHandle, dataCacheTags, regionName);
                    try
                    {
                        CacheLockTableLocker.EnterWriteLock();
                        this._cacheLockTable.Remove(cacheLockEntry);
                        result = true;
                    }
                    finally
                    {
                        CacheLockTableLocker.ExitWriteLock();
                    }
                }
                else
                {
                    throw new ArgumentException("lockId");
                }
            }

            return result;
        }

        /// <summary>
        /// Put Object in Cache in Region and unlock the cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        ///  <param name="lockId">lockId</param>
        ///   <param name="regionName">regionName</param>
        ///   <returns></returns>

        public bool PutAndUnlock(string key, object value, Guid lockId, string regionName)
        {
            bool result = false;
            CacheLockEntry cacheLockEntry;
            if (dataCache != null)
            {
                try
                {
                    CacheLockTableLocker.EnterReadLock();
                    cacheLockEntry = (from lockHandleEntry in this._cacheLockTable
                                      where lockHandleEntry.LockId.Equals(lockId)
                                      select lockHandleEntry).FirstOrDefault();

                }
                finally
                {
                    CacheLockTableLocker.ExitReadLock();
                }

                dataCache.PutAndUnlock(key, value, cacheLockEntry.LockHandle, regionName);
                try
                {
                    CacheLockTableLocker.EnterWriteLock();
                    this._cacheLockTable.Remove(cacheLockEntry);
                    result = true;
                }
                finally
                {
                    CacheLockTableLocker.ExitWriteLock();
                }
            }

            return result;
        }

        /// <summary>
        /// Put Object in Cache with timeout and unlock the cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        ///  <param name="lockId">lockId</param>
        ///   <param name="timeout">timeout</param>
        ///   <returns></returns>
        public bool PutAndUnlock(string key, object value, Guid lockId, TimeSpan timeout)
        {
            bool result = false;
            CacheLockEntry cacheLockEntry;
            if (dataCache != null)
            {
                try
                {
                    CacheLockTableLocker.EnterReadLock();
                    cacheLockEntry = (from lockHandleEntry in this._cacheLockTable
                                      where lockHandleEntry.LockId.Equals(lockId)
                                      select lockHandleEntry).FirstOrDefault();

                }
                finally
                {
                    CacheLockTableLocker.ExitReadLock();
                }

                dataCache.PutAndUnlock(key, value, cacheLockEntry.LockHandle, timeout);
                try
                {
                    CacheLockTableLocker.EnterWriteLock();
                    this._cacheLockTable.Remove(cacheLockEntry);
                    result = true;
                }
                finally
                {
                    CacheLockTableLocker.ExitWriteLock();
                }
            }

            return result;
        }
        /// <summary>
        /// Put Object in Cache with cacheTags and unlock the cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        ///  <param name="cacheTags">cacheTags</param>
        ///   <param name="timeout">timeout</param>
        ///   <returns></returns>
        public bool PutAndUnlock(string key, object value, Guid lockId, TimeSpan timeout, IEnumerable<string> cacheTags)
        {
            bool result = false;
            CacheLockEntry cacheLockEntry;
            if (dataCache != null)
            {
                try
                {
                    CacheLockTableLocker.EnterReadLock();
                    cacheLockEntry = (from lockHandleEntry in this._cacheLockTable
                                      where lockHandleEntry.LockId.Equals(lockId)
                                      select lockHandleEntry).FirstOrDefault();

                }
                finally
                {
                    CacheLockTableLocker.ExitReadLock();
                }

                var dataCacheTags = cacheTags.Select(cacheTag => new DataCacheTag(cacheTag));

                dataCache.PutAndUnlock(key, value, cacheLockEntry.LockHandle, timeout, dataCacheTags);
                try
                {
                    CacheLockTableLocker.EnterWriteLock();
                    this._cacheLockTable.Remove(cacheLockEntry);
                    result = true;
                }
                finally
                {
                    CacheLockTableLocker.ExitWriteLock();
                }
            }

            return result;
        }


        /// <summary>
        /// Put Object in Cache with cacheTags and unlock the cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        ///  <param name="cacheTags">cacheTags</param>
        ///   <param name="timeout">timeout</param>
        ///   <param name="regionName">regionName</param>
        ///   <returns></returns>
        public bool PutAndUnlock(string key, object value, Guid lockId, TimeSpan timeout, IEnumerable<string> cacheTags, string regionName)
        {
            bool result = false;
            CacheLockEntry cacheLockEntry;
            if (dataCache != null)
            {
                try
                {
                    CacheLockTableLocker.EnterReadLock();
                    cacheLockEntry = (from lockHandleEntry in this._cacheLockTable
                                      where lockHandleEntry.LockId.Equals(lockId)
                                      select lockHandleEntry).FirstOrDefault();

                }
                finally
                {
                    CacheLockTableLocker.ExitReadLock();
                }

                var dataCacheTags = cacheTags.Select(cacheTag => new DataCacheTag(cacheTag));

                dataCache.PutAndUnlock(key, value, cacheLockEntry.LockHandle, timeout, dataCacheTags, regionName);
                try
                {
                    CacheLockTableLocker.EnterWriteLock();
                    this._cacheLockTable.Remove(cacheLockEntry);
                    result = true;
                }
                finally
                {
                    CacheLockTableLocker.ExitWriteLock();
                }
            }
            return result;
        }

        /// <summary>
        /// Put Object in Cache in Region and unlock the cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Serialized Object</param>
        ///  <param name="cacheTags">cacheTags</param>
        ///   <param name="timeout">timeout</param>
        ///   <param name="regionName">regionName</param>
        ///   <returns></returns>
        public bool PutAndUnlock(string key, object value, Guid lockId, TimeSpan timeout, string regionName)
        {
            bool result = false;
            CacheLockEntry cacheLockEntry;
            if (dataCache != null)
            {
                try
                {
                    CacheLockTableLocker.EnterReadLock();
                    cacheLockEntry = (from lockHandleEntry in this._cacheLockTable
                                      where lockHandleEntry.LockId.Equals(lockId)
                                      select lockHandleEntry).FirstOrDefault();

                }
                finally
                {
                    CacheLockTableLocker.ExitReadLock();
                }

                dataCache.PutAndUnlock(key, value, cacheLockEntry.LockHandle, timeout, regionName);


                try
                {
                    CacheLockTableLocker.EnterWriteLock();
                    this._cacheLockTable.Remove(cacheLockEntry);
                    result = true;
                }
                finally
                {
                    CacheLockTableLocker.ExitWriteLock();
                }
            }

            return result;
        }

        ///// <summary>
        ///// Create SyxDependencyInfo Object and call Dependency service to Add SQL Query Noticfication
        ///// </summary>
        ///// <param name="Key">Cache Key</param>
        ///// <param name="ObjectQuery">select query</param>
        ///// <returns>True if Success</returns>
        //public bool ProcessDependency(string  Key, string ObjectQuery)
        //{
        //     SyxDependencyInfo DependencyInfo = new SyxDependencyInfo();
        //     DependencyInfo.CacheKey = Key;
        //     DependencyInfo.SelectQuery = ObjectQuery.Replace("\r\n[Extent1].", "").Replace("\r\n", "").Replace("[Extent1].", "").Replace("AS [Extent1]", "");
        //     SysXCacheUtils.CachingDependencyService.AddSqlDependencies(Key,DependencyInfo);
        //     return true;

        //}

        #endregion

        #region Remove Methods
        /// <summary>
        /// Remove Object from Cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True if Success</returns>
        public bool Remove(string key)
        {
            bool result = false;
            try
            {

                if (dataCache != null)
                {
                    result = dataCache.Remove(key);
                }
            }

            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Remove Object from Sytem Cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True if Success</returns>
        public bool RemoveSystemCache(string key)
        {
            bool result = false;
            try
            {
                cacheManager.Remove(key);
                result = true;
            }

            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }
        /// <summary>
        /// Remove all Objects
        /// </summary>
        /// <returns>True if Success</returns>
        public void RemoveAll()
        {
            try
            {
                Parallel.ForEach(dataCache.GetSystemRegions(), region =>
                {
                    dataCache.ClearRegion(region);
                    var sysRegion = dataCache.GetSystemRegionName(region);
                    dataCache.ClearRegion(sysRegion);
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Remove Object 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lockId"></param>
        /// <returns></returns>
        public bool Remove(string key, Guid lockId)
        {
            bool result = false;
            CacheLockEntry cacheLockEntry;
            if (dataCache != null)
            {
                try
                {
                    CacheLockTableLocker.EnterReadLock();
                    cacheLockEntry = (from lockHandleEntry in this._cacheLockTable
                                      where lockHandleEntry.LockId.Equals(lockId)
                                      select lockHandleEntry).FirstOrDefault();

                }
                finally
                {
                    CacheLockTableLocker.ExitReadLock();
                }

                if (cacheLockEntry != null)
                {
                    dataCache.Remove(key, cacheLockEntry.LockHandle);

                    try
                    {
                        CacheLockTableLocker.EnterWriteLock();
                        this._cacheLockTable.Remove(cacheLockEntry);
                        result = true;
                    }
                    finally
                    {
                        CacheLockTableLocker.ExitWriteLock();
                    }
                }
            }

            return result;
        }
        public bool Remove(string key, Guid lockId, string regionName)
        {
            bool result = false;
            CacheLockEntry cacheLockEntry;
            if (dataCache != null)
            {

                try
                {
                    CacheLockTableLocker.EnterReadLock();
                    cacheLockEntry = (from lockHandleEntry in this._cacheLockTable
                                      where lockHandleEntry.LockId.Equals(lockId)
                                      select lockHandleEntry).FirstOrDefault();

                }
                finally
                {
                    CacheLockTableLocker.ExitReadLock();
                }

                if (cacheLockEntry != null)
                {
                    dataCache.Remove(key, cacheLockEntry.LockHandle, regionName);

                    try
                    {
                        CacheLockTableLocker.EnterWriteLock();
                        this._cacheLockTable.Remove(cacheLockEntry);
                        result = true;
                    }
                    finally
                    {
                        CacheLockTableLocker.ExitWriteLock();
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Remove Object from Cache Region
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="regionName">Region</param>
        /// <returns></returns>
        public bool Remove(string key, string regionName)
        {
            bool result = false;

            try
            {
                if (dataCache != null)
                {

                    result = dataCache.Remove(key, regionName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Remove Object from Cache Region
        /// </summary>
        /// <param name="regionName">Region</param>
        /// <returns></returns>
        public bool RemoveRegion(string regionName)
        {
            bool result = false;
            try
            {
                if (dataCache != null)
                {
                    result = dataCache.RemoveRegion(regionName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }
        #endregion

        #region Misc Methods
        /// <summary>
        /// Create Region on Appfabric Cache
        /// </summary>
        /// <param name="regionName">region name</param>
        /// <returns></returns>
        public bool CreateRegion(string regionName)
        {
            bool result = false;

            try
            {
                if (dataCache != null)
                {
                    result = dataCache.CreateRegion(regionName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// Clear Region on Appfabric Cache
        /// </summary>
        /// <param name="regionName">region name</param>
        /// <returns></returns>
        public bool ClearRegion(string regionName)
        {
            bool result = false;
            try
            {
                if (dataCache != null)
                {
                    dataCache.ClearRegion(regionName);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return result;
        }
        public object this[string key]
        {
            get { return this.Get(key); }
            set
            {

                dataCache.Put(key, value);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Set Cache From Configurations 
        /// </summary>
        private void SetCache()
        {

            if (dataCache == null)
            {
                var configuration = new DataCacheFactoryConfiguration();
                _factory = new DataCacheFactory(configuration);
                dataCache = _factory.GetCache(ConfigurationManager.AppSettings[SysXCachingConst.APPFABRICCACHENAME]);
            }

        }
        /// <summary>
        /// Get CacheItem Version
        /// </summary>
        /// <param name="earlierVersion"></param>
        /// <returns></returns>
        private static DataCacheItemVersion GetDataCacheItemVersion(object earlierVersion)
        {
            if (earlierVersion != null)
            {
                var dataCacheItemVersion = Activator.CreateInstance(
                                                                    typeof(DataCacheItemVersion),
                                                                    BindingFlags.Instance | BindingFlags.NonPublic,
                                                                    null,
                                                                    new[] { earlierVersion },
                                                                    null,
                                                                    null
                                                                    );

                if (dataCacheItemVersion != null)
                {
                    if (dataCacheItemVersion is DataCacheItemVersion)
                        return dataCacheItemVersion as DataCacheItemVersion;

                    Debug.Assert(dataCacheItemVersion as DataCacheItemVersion != null, "Conversion to DataCacheItemVersion failed");
                }
            }

            return null;
        }

        /// <summary>
        /// Add Lock Table Entry
        /// </summary>
        /// <param name="lockEntry"></param>
        private void AddToCacheLockTable(CacheLockEntry lockEntry)
        {
            try
            {
                CacheLockTableLocker.EnterWriteLock();
                this._cacheLockTable.Add(lockEntry);
            }
            finally
            {
                CacheLockTableLocker.ExitWriteLock();
            }
        }
        #endregion
    }

    /// <summary>
    /// Class To handle Cache Lock while doing Get and Put operations
    /// </summary>
    [Serializable]
    internal class CacheLockEntry
    {

        public DataCacheLockHandle LockHandle { get; private set; }
        public Guid LockId { get; private set; }
        public CacheLockEntry(DataCacheLockHandle lockHandle)
        {
            this.LockHandle = lockHandle;
            this.LockId = Guid.NewGuid();
        }
    }

    /// <summary>
    /// Class To handle Cache Timeout for specific Entities or tables
    /// </summary>
    [Serializable]
    internal class CacheTimeoutEntity
    {
        public String EntityName { get; private set; }
        public TimeSpan EntityTimeout { get; private set; }

        public CacheTimeoutEntity()
        {
        }

        public IList<CacheTimeoutEntity> SetCacheTimeoutEntity()
        {
            var lstCacheTimeoutEntity = new List<CacheTimeoutEntity>();
            lstCacheTimeoutEntity.Add(new CacheTimeoutEntity() { EntityName = "Tenants", EntityTimeout = TimeSpan.FromMinutes(5) });
            lstCacheTimeoutEntity.Add(new CacheTimeoutEntity() { EntityName = "ClientDBConfigurations", EntityTimeout = TimeSpan.FromMinutes(5) });
            lstCacheTimeoutEntity.Add(new CacheTimeoutEntity() { EntityName = "SystemEventSettings", EntityTimeout = TimeSpan.FromMinutes(15) });
            lstCacheTimeoutEntity.Add(new CacheTimeoutEntity() { EntityName = "CommunicationTemplates", EntityTimeout = TimeSpan.FromMinutes(15) });
            lstCacheTimeoutEntity.Add(new CacheTimeoutEntity() { EntityName = "CommunicationTemplatePlaceHolders", EntityTimeout = TimeSpan.FromMinutes(15) });
            lstCacheTimeoutEntity.Add(new CacheTimeoutEntity() { EntityName = "CommunicationTemplatePlaceHolderSubEvents", EntityTimeout = TimeSpan.FromMinutes(15) });
            lstCacheTimeoutEntity.Add(new CacheTimeoutEntity() { EntityName = "vw_GetTenants", EntityTimeout = TimeSpan.FromMinutes(1) });
            lstCacheTimeoutEntity.Add(new CacheTimeoutEntity() { EntityName = "Websites", EntityTimeout = TimeSpan.FromMinutes(1) });
            lstCacheTimeoutEntity.Add(new CacheTimeoutEntity() { EntityName = "lkpPaymentOptions", EntityTimeout = TimeSpan.FromMinutes(15) });
            lstCacheTimeoutEntity.Add(new CacheTimeoutEntity() { EntityName = "lkpCommunicationSubEvent", EntityTimeout = TimeSpan.FromMinutes(15) });            
            return lstCacheTimeoutEntity;
        }
    }
}
