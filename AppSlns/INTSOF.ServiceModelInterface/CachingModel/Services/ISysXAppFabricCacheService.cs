#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ISysXAppFabricCacheService.cs
// Purpose:   Interface for SysXAppFabricCacheService
//

#endregion
#region Namespace
#region System Defined
using Microsoft.ApplicationServer.Caching;
using System;
using System.Collections.Generic;

#endregion
#region Application Defined
#endregion
#endregion
namespace CoreWeb.IntsofCachingModel.Interface.Services
{
    public interface ISysXAppFabricCacheService
    {
      
        #region Add Methods

        bool Add(string key, object value);
        bool AddSystemCache(string key, object value);
        bool Add(string key, object value, TimeSpan timeout);
        bool Add(string key, object value, string regionName);
        bool Add(string key, object value, string regionName, TimeSpan timeout);
        bool Add(string key, object value, IEnumerable<string> cacheTags);
        bool Add(string key, object value, IEnumerable<string> cacheTags, string regionName);
        bool Add(string key, object value, TimeSpan timeout, IEnumerable<string> cacheTags);
        bool Add(string key, object value, TimeSpan timeout, IEnumerable<string> cacheTags, string regionName);

        #endregion
        #region Get Method

        object Get(string key);
        T Get<T>(string key);
        T GetSystemCache<T>(string key);
        IDictionary<string, object> Get(params string[] keys);
        object GetAndLock(string key, TimeSpan timeout, out Guid lockId);
         object GetAndLock(string key, TimeSpan timeout, out Guid lockId, string regionName);
        object Get(string key, string regionName);
        IEnumerable<KeyValuePair<string, object>> GetObjectsByAllTags(IEnumerable<string> cacheTags, string regionName);
        IEnumerable<KeyValuePair<string, object>> GetObjectsByAnyTag(IEnumerable<string> cacheTags, string regionName);
        IEnumerable<KeyValuePair<string, object>> GetObjectsByTag(string cacheTag, string regionName);
        IEnumerable<KeyValuePair<string, object>> GetObjectsInRegion(string regionName);
        string GetSystemRegionName(string key);
        IEnumerable<string> GetSystemRegions();
        #endregion

        bool IsCacheVersionSame(object earlierVersion, string key, string regionName);
        #region Put Methods
        DataCacheItemVersion Put(string key, object value);
        DataCacheItemVersion Put(string key, object value, TimeSpan timeout);
        DataCacheItemVersion Put(string key, object value, TimeSpan timeout, string regionName);
        DataCacheItemVersion Put(string key, object value, TimeSpan timeout, IEnumerable<string> cacheTags, string regionName);
        DataCacheItemVersion Put(string key, object value, TimeSpan timeout, IEnumerable<string> cacheTags);
        DataCacheItemVersion Put(string key, object value, string regionName);
        DataCacheItemVersion Put(string key, object value, IEnumerable<string> cacheTags, string regionName);
        DataCacheItemVersion Put(string key, object value, IEnumerable<string> cacheTags);
        bool PutAndUnlock(string key, object value, Guid lockId);
        bool PutAndUnlock(string key, object value, Guid lockId, IEnumerable<string> cacheTags);
        bool PutAndUnlock(string key, object value, Guid lockId, IEnumerable<string> cacheTags, string regionName);
        bool PutAndUnlock(string key, object value, Guid lockId, string regionName);
        bool PutAndUnlock(string key, object value, Guid lockId, TimeSpan timeout);
        bool PutAndUnlock(string key, object value, Guid lockId, TimeSpan timeout, IEnumerable<string> cacheTags);
        bool PutAndUnlock(string key, object value, Guid lockId, TimeSpan timeout, IEnumerable<string> cacheTags,
                                                 string regionName);
        bool PutAndUnlock(string key, object value, Guid lockId, TimeSpan timeout, string regionName);
      
        #endregion

        #region Remove 
        bool Remove(string key);
        bool RemoveSystemCache(string key);
        void RemoveAll();
        bool Remove(string key, Guid lockId);
        bool Remove(string key, Guid lockId, string regionName);
        bool Remove(string key, string regionName);
        bool RemoveRegion(string regionName);
        #endregion
        #region Misc
        bool CreateRegion(string regionName);
        bool ClearRegion(string regionName);
      
        #endregion
        object this[string key] { get; set; }

      // Boolean ProcessDependency(string Key, string ObjectQuery);
    }
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
}
