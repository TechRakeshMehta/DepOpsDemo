#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXCachingService.cs
// Purpose:   SysX Caching Service
//

#endregion

#region Namespace

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreWeb.IntsofCachingModel.Interface.Services;
using SPI.Caching;
using CoreWeb.IntsofCachingModel.Wrapper;
using System.Web.Caching;
using System.Web;
using System.Configuration;
using System.IO;
#endregion

#region Application Defined
using INTSOF.Utils;
using INTSOF.Logger;
using INTSOF.Logger.factory;
#endregion

#endregion

namespace CoreWeb.IntsofCachingModel.Services
{
    /// <summary>
    /// Used for Caching Service
    /// </summary>
    public class SysXCachingService : ISysXCachingService
    {
        #region Private Variables
        private ILogger _logger;
        #endregion

        #region Class Construction

        /// <summary>
        /// Default constructor
        /// </summary>
        public SysXCachingService()
        {
            try
            {
                _logger = SysXLoggerFactory.GetInstance().GetLogger();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        #endregion

        #region ISysXCachingService Members

        #region Public Methods

        /// <summary>
        /// Adds an object to MyCache Service
        /// </summary>
        /// <param name="Key">Key</param>
        /// <param name="Value">Value</param>
        public void Add(String Key, Object Value)
        {
            try
            {
                GetSysXCacheManager().Channel.Add(Key, Value);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds an object to MyCache Service along with a FileDependency
        /// </summary>
        /// <param name="Key">Key</param>
        /// <param name="Value">Value</param>        
        /// <param name="absoluteExpiration">Absolute Expiration</param>
        /// <param name="slidingExpiration">Sliding Expiration</param>
        /// <param name="priority">priority</param>
        public void Add(String Key, Object Value, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority)
        {
            try
            {
                String strPriority = priority.ToString();

                //Set appripriate values for priority for MyCache Service
                if (priority == CacheItemPriority.AboveNormal)
                {
                    strPriority = SysXCachingConst.CACHE_ITEM_PRIORITY_HIGH;
                }
                if (priority == CacheItemPriority.BelowNormal)
                {
                    strPriority = SysXCachingConst.CACHE_ITEM_PRIORITY_LOW;
                }
                if (priority == CacheItemPriority.Default)
                {
                    strPriority = SysXCachingConst.CACHE_ITEM_PRIORITY_NONE;
                }

                String dependencyFilePath = GetDependencyFilePath(Key);

                if (!File.Exists(dependencyFilePath))
                {
                    File.Create(dependencyFilePath);
                }

                dependencyFilePath = dependencyFilePath == null ? String.Empty : dependencyFilePath;

                CacheDependency dependency = new CacheDependency(dependencyFilePath);

                if (absoluteExpiration == System.Web.Caching.Cache.NoAbsoluteExpiration)
                {
                    absoluteExpiration = DateTime.MaxValue;
                }

                if (slidingExpiration == Cache.NoSlidingExpiration)
                {
                    slidingExpiration = new TimeSpan(0, 0, 0);
                }

                //Set the Key (Both as a Key and a Value) in Asp.net Cache with specifying FileDependency and CallBack method to get event notification when the underlying
                //file content is modified
                HttpContext.Current.Cache.Add(Key, Key, dependency, absoluteExpiration, slidingExpiration, priority, new CacheItemRemovedCallback(onRemoveCallback));

                GetSysXCacheManager().Channel.Insert(Key, Value, dependencyFilePath, strPriority, absoluteExpiration, slidingExpiration);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds an object to MyCache Service along with a FileDependency
        /// </summary>
        /// <param name="Key">Key</param>
        /// <param name="Value">Value</param>        
        /// <param name="absoluteExpiration">Absolute Expiration</param>
        /// <param name="slidingExpiration">Sliding Expiration</param>
        /// <param name="priority">Priority</param>
        /// <param name="onRemoveCallback">On Remove Call back</param>
        public void Add(String Key, Object Value, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            try
            {
                String strPriority = priority.ToString();

                //Set appripriate values for priority for MyCache Service
                if (priority == CacheItemPriority.AboveNormal)
                {
                    strPriority = SysXCachingConst.CACHE_ITEM_PRIORITY_HIGH;
                }
                if (priority == CacheItemPriority.BelowNormal)
                {
                    strPriority = SysXCachingConst.CACHE_ITEM_PRIORITY_LOW;
                }
                if (priority == CacheItemPriority.Default)
                {
                    strPriority = SysXCachingConst.CACHE_ITEM_PRIORITY_NONE;
                }

                String dependencyFilePath = GetDependencyFilePath(Key);
                if (!File.Exists(dependencyFilePath))
                {
                    using (File.Create(dependencyFilePath)) { }
                }

                dependencyFilePath = dependencyFilePath == null ? String.Empty : dependencyFilePath;

                CacheDependency dependency = new CacheDependency(dependencyFilePath);

                if (absoluteExpiration == System.Web.Caching.Cache.NoAbsoluteExpiration)
                {
                    absoluteExpiration = DateTime.MaxValue;
                }

                if (slidingExpiration == Cache.NoSlidingExpiration)
                {
                    slidingExpiration = new TimeSpan(0, 0, 0);
                }

                //Set the Key (Both as a Key and a Value) in Asp.net Cache with specifying FileDependency and CallBack method to get event notification when the underlying
                //file content is modified
                HttpContext.Current.Cache.Add(Key, Key, dependency, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);

                GetSysXCacheManager().Channel.Insert(Key, Value, dependencyFilePath, strPriority, absoluteExpiration, slidingExpiration);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Tries to obtain a lock for the specified Key on MyCache service
        /// within the CacheItemRemovedCallBack method
        /// </summary>
        /// <param name="Key">Key as String</param>
        /// <returns>Boolean</returns>
        public Boolean SetLock(String Key)
        {
            try
            {
                return GetSysXCacheManager().Channel.SetLock(Key);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return false;
        }

        /// <summary>
        /// Releases the current lock for the specified Key on MyCache service. Only to be used
        /// within the CacheItemRemovedCallBack method
        /// </summary>
        /// <param name="Key">Key as String</param>
        public void ReleaseLock(String Key)
        {
            try
            {
                GetSysXCacheManager().Channel.ReleaseLock(Key);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Removes the corresponding object from MyCache service for the specified Key
        /// </summary>
        /// <param name="Key">Key</param>
        public void Remove(String Key)
        {
            try
            {
                //Remove from MyCache Service
                GetSysXCacheManager().Channel.Remove(Key);
                //Remove from Asp.net Cache for the Key if there is any entry
                HttpContext.Current.Cache.Remove(Key);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get objet from MyCache service for the specified Key
        /// </summary>
        /// <param name="Key">Key</param>
        /// <returns>Object</returns>
        public Object Get(String Key)
        {
            try
            {
                return GetSysXCacheManager().Channel.Get(Key);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return null;
        }

        /// <summary>
        /// Checks whether the corresponding object exists on MyCache service for the Key
        /// </summary>
        /// <param name="Key">Key</param>
        /// <returns>Boolean</returns>
        public Boolean Exists(String Key)
        {
            try
            {
                return GetSysXCacheManager().Channel.Exists(Key);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return false;
        }

        #endregion
        #endregion

        #region Protected Methods

        /// <summary>
        /// Callback function to re-insert the item in cache if cache dependency changed
        /// </summary>
        /// <param name="Key">Key</param>
        /// <param name="Value">Value</param>
        /// <param name="reason">CacheItemRemovedReason</param>
        protected void onRemoveCallback(String Key, Object Value, CacheItemRemovedReason reason)
        {
            try
            {
                if (reason.Equals(CacheItemRemovedReason.DependencyChanged))
                {
                    ISysXCacheManager cacheManager = GetSysXCacheManager().Channel;
                    if (SetLock(Key))
                    {
                        String dependencyFilePath = GetDependencyFilePath(Key);
                        Object modifiedValue = GetObjectFromFile(dependencyFilePath);
                        this.Add(Key, modifiedValue, Cache.NoAbsoluteExpiration, new TimeSpan(0, 5, 60), CacheItemPriority.Normal);
                        //Release lock when done
                        ReleaseLock(Key);
                    }
                }
                if (reason.Equals(CacheItemRemovedReason.Expired) || reason.Equals(CacheItemRemovedReason.Removed))
                {
                    if (Exists(Key))
                    {
                        Remove(Key);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Service Client Wrapper
        /// </summary>
        /// <returns>ServiceClientWrapper(ISysXCacheManager)</returns>
        private ServiceClientWrapper<ISysXCacheManager> GetSysXCacheManager()
        {
            try
            {
                return new ServiceClientWrapper<ISysXCacheManager>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return null;
        }

        /// <summary>
        /// Read data from File
        /// </summary>
        /// <param name="dependencyFilePath">dependencyFilePath</param>
        /// <returns>Object</returns>
        private Object GetObjectFromFile(String dependencyFilePath)
        {
            object updatedValue = null;

            try
            {
                updatedValue = File.ReadAllText(dependencyFilePath);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

            return updatedValue;
        }

        /// <summary>
        /// Get dependency file location
        /// </summary>
        /// <returns>Key</returns>
        private String GetDependencyFilePath(String Key)
        {
            return @ConfigurationManager.AppSettings[SysXCachingConst.CACHE_PATH] + Key + SysXCachingConst.CACHE_FILE_EXTENSION;
        }

        #endregion
    }
}
