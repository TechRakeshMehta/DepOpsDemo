#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  DALUtils.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

#region System Defined

using System.Web;


#endregion
using System.Linq;
#endregion

#region Application Specific

using System.Collections.Generic;
using Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Text;
using System.Data.Entity.Core.Objects;
using System.Data.Common;
using System.Data.Entity.Core.Objects.DataClasses;
using System;
using System.Configuration;
using INTSOF.Logger;
using System.Threading.Tasks;
using INTSOF.Utils;
using CoreWeb.IntsofCachingModel.Services;
using System.Diagnostics;
using INTSOF.Contracts;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using CoreWeb.IntsofCachingModel.Interface.Services;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using Entity.SharedDataEntity;
using INTSOF.ServiceModelInterface;

#endregion

#endregion

namespace INTSOF.AppFabricCacheServer
{
    /// <summary>
    /// This class handles the operations for App Fabric Cache
    /// </summary>
    /// <remarks></remarks>
    public static class SysXCacheUtils
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables
        private static ISysXLoggerService _sysXLoggerService;
        private static ISysXAppFabricCacheService _sysXAppFabricCacheService;
        private static ISysXCachingDependencyService _sysXCachingDependencyService;
        private static List<String> lstLkpNotDependent = ConfigurationManager.AppSettings[SysXCachingConst.LKPNONSQLDEPENDENT] == null ? new List<String>() : ConfigurationManager.AppSettings[SysXCachingConst.LKPNONSQLDEPENDENT].Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
        public static String CacheType = ConfigurationManager.AppSettings[SysXCachingConst.CACHETYPE] == null ? SysXCachingConst.CACHETYPENONE : ConfigurationManager.AppSettings[SysXCachingConst.CACHETYPE];
        private static ILogger _logger = null;
        public static Boolean logEnable = ConfigurationManager.AppSettings[SysXCachingConst.ISCACHELOGENABLE] == null ? false : Convert.ToBoolean(ConfigurationManager.AppSettings[SysXCachingConst.ISCACHELOGENABLE]);
        private static ISysXSessionService _sysxSessionService;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Get instance of ISysXLoggerService
        /// </summary>
        public static ISysXLoggerService LoggerService
        {
            get
            {
                if (_sysXLoggerService == null && HttpContext.Current.IsNotNull())
                {
                    if (HttpContext.Current.ApplicationInstance is IWebApplication)
                    {
                        _sysXLoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                    }
                }
                else
                {
                    _sysXLoggerService = null;
                }

                return _sysXLoggerService;
            }
        }

        /// <summary>
        /// Get instance of ISysXAppFabricCacheService
        /// </summary>
        public static ISysXAppFabricCacheService AppFabricCacheService
        {
            get
            {
                if (_sysXAppFabricCacheService == null && HttpContext.Current.IsNotNull())
                {
                    if (HttpContext.Current.ApplicationInstance is IWebApplication)
                    {
                        _sysXAppFabricCacheService = (HttpContext.Current.ApplicationInstance as IWebApplication).AppFabricCacheService;
                    }
                    else if (HttpContext.Current.ApplicationInstance is IIntsofService)
                    {
                            _sysXAppFabricCacheService = (HttpContext.Current.ApplicationInstance as IIntsofService).AppFabricCacheService;
                    }
                }
                //else if (_sysXAppFabricCacheService == null  && OperationContext.Current.IsNotNull())
                //{
                //    _sysXAppFabricCacheService = ServiceSingleton.Instance.AppFabricService;
                //    //ObjectCache cache = MemoryCache.Default;
                //}

                return _sysXAppFabricCacheService;
            }
        }

        public static ISysXSessionService SysXSessionService
        {
            get
            {

                if (_sysxSessionService == null && HttpContext.Current.IsNotNull())
                {
                    if (HttpContext.Current.ApplicationInstance is IWebApplication)
                    {
                        _sysxSessionService = (HttpContext.Current.ApplicationInstance as IWebApplication).SysXSessionService;
                    }
                }
                return _sysxSessionService;
            }
        }

        /// <summary>
        /// Get instance of ISysXCachingDependencyService
        /// </summary>
        public static ISysXCachingDependencyService CachingDependencyService
        {
            get
            {
                if (_sysXCachingDependencyService == null && HttpContext.Current.IsNotNull())
                {
                    if (HttpContext.Current.ApplicationInstance is IWebApplication)
                    {
                        _sysXCachingDependencyService = (HttpContext.Current.ApplicationInstance as IWebApplication).CacheDependencyService;
                    }
                    else if (HttpContext.Current.ApplicationInstance is IIntsofService)
                    {
                        _sysXCachingDependencyService = (HttpContext.Current.ApplicationInstance as IIntsofService).CacheDependencyService;
                    }
                }
                //else if (_sysXCachingDependencyService == null && OperationContext.Current.IsNotNull())
                //{
                //    _sysXCachingDependencyService = ServiceSingleton.Instance.CacheDependencyService;
                //    //ObjectCache cache = MemoryCache.Default;
                //}

             
                return _sysXCachingDependencyService;
            }
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// Get Instance of Logger
        /// </summary>
        private static ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = LoggerService.GetLogger();
                }
                return _logger;
            }
        }

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods


        #region Public Methods


        public static Int32 GetLookUpIDbyCode<TEntity>(Func<TEntity, bool> predicate) where TEntity : EntityObject
        {
            TEntity Enity = SysXCacheUtils.GetAddCacheLookup<TEntity>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()).SingleOrDefault<TEntity>(predicate);
            return Convert.ToInt32(Enity.EntityKey.EntityKeyValues.FirstOrDefault().Value);
        }

        public static String GetLookUpCodebyID<TEntity>(Func<TEntity, bool> predicate, Func<TEntity, String> selector) where TEntity : EntityObject
        {
            return SysXCacheUtils.GetAddCacheLookup<TEntity>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()).Where(predicate).Select(selector).FirstOrDefault();
            //return Convert.ToInt32(Enity.EntityKey.EntityKeyValues.FirstOrDefault().Value);
        }
        /// <summary>
        /// Get Look Up Data from Cache if not found Get from Database and add to Cache
        /// </summary>
        /// <typeparam name="TEntity">EntityObject</typeparam>
        /// <returns>list of EntityObject </returns>
        public static List<TEntity> GetAddCacheLookup<TEntity>(String adbDatabaseType, String connectionString = null, Int32? tenantId = null) where TEntity : EntityObject
        {
            try
            {
                Stopwatch cstopWatch = new Stopwatch();
                Stopwatch stopWatch = new Stopwatch();
                List<TEntity> lstLookUpData = new List<TEntity>();
                String keyValue = Convert.ToString(tenantId);
                ObjectContext context = null;

                if (adbDatabaseType == ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue() && connectionString.IsNull() && tenantId.IsNull())
                {
                    context = new SysXAppDBEntities();
                    context.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                }
                else if (adbDatabaseType == ADBDatabaseDetail.ADBDatabaseType.MESSAGING_DB.GetStringValue())
                {
                    context = new ADBMessageDB_DevEntities();
                    context.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                }
                else if (adbDatabaseType == ADBDatabaseDetail.ADBDatabaseType.SHAREDDATA_DB.GetStringValue())
                {
                    context = new ADB_SharedDataEntities();
                    context.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                }
                else if (adbDatabaseType == ADBDatabaseDetail.ADBDatabaseType.TENANT_DB.GetStringValue())
                {
                    context = new Entity.ClientEntity.ADB_LibertyUniversity_ReviewEntities(connectionString);
                    context.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
                }

                String entitySetName = context.MetadataWorkspace.GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace).BaseEntitySets
                                                                  .Where(bes => bes.ElementType.Name == typeof(TEntity).Name).FirstOrDefault().Name;

                if (lstLkpNotDependent.Contains(typeof(TEntity).Name))
                {
                    lstLookUpData = new List<TEntity>();
                    ObjectQuery<DbDataRecord> _DbDataRecord = GetEnitityData(context, entitySetName);
                    foreach (var rec in _DbDataRecord)
                    {
                        lstLookUpData.Add((TEntity)rec.GetValue(0));
                    }
                    if (logEnable)
                    {
                        if (Logger != null)
                        {
                            Logger.Info("AppFabricCacheService : Retreived Non SQL Dependent look Up " + entitySetName + " from DB.");
                        }
                    }
                }
                else
                {
                    switch (CacheType)
                    {
                        #region Appfabric Cache handling
                        case SysXCachingConst.CACHETYPEAPPFABRIC:

                            #region Fetch from Cache

                            lstLookUpData = AppFabricCacheService.Get<List<TEntity>>(entitySetName);

                            if (lstLookUpData != null)
                            {
                                if (logEnable)
                                {
                                    if (Logger != null)
                                    {
                                        Logger.Info("AppFabricCacheService : Retreived from AppFabric Server  :" + entitySetName + " from Cache. ");
                                    }
                                }
                            }

                            #endregion
                            else
                            {
                                #region Getting from DB and adding to Cache
                                lstLookUpData = new List<TEntity>();
                                ObjectQuery<DbDataRecord> _DbDataRecords = GetEnitityData(context, entitySetName);
                                foreach (var rec in _DbDataRecords)
                                {
                                    lstLookUpData.Add((TEntity)rec.GetValue(0));
                                }

                                #region Adding to Sql CacheDependency

                                if (connectionString.IsNotNull() && tenantId.IsNull())
                                {
                                    if (ProcessDependency(entitySetName, _DbDataRecords.ToTraceString()))
                                    {
                                        if (logEnable)
                                        {
                                            if (Logger != null)
                                            {
                                                Logger.Info("CachingDependencyService : Added SQL Dependency for look up table :-" + entitySetName);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (ProcessDependency(entitySetName, _DbDataRecords.ToTraceString(), tenantId))
                                    {
                                        if (logEnable)
                                        {
                                            if (Logger != null)
                                            {
                                                Logger.Info("CachingDependencyService : Added SQL Dependency for look up table :-" + entitySetName);
                                            }
                                        }
                                    }
                                }

                                #endregion
                                if (AppFabricCacheService.Add(entitySetName, lstLookUpData))
                                {
                                    if (logEnable)
                                    {
                                        if (Logger != null)
                                        {
                                            Logger.Info("AppFabricCacheService : Added to AppFabric Cache Server :" + entitySetName + " in Cache.");
                                        }
                                    }
                                }
                                #endregion
                            }

                            break;
                        #endregion
                        #region System Cache handling
                        case SysXCachingConst.CACHETYPESYSTEM:
                            #region Appfabric:
                            #region Fetch from Cache
                            cstopWatch.Start();
                            lstLookUpData = AppFabricCacheService.GetSystemCache<List<TEntity>>(entitySetName + keyValue);
                            cstopWatch.Stop();
                            if (lstLookUpData != null)
                            {
                                if (logEnable)
                                {
                                    if (Logger != null)
                                    {
                                        Logger.Info("AppFabricCacheService : Retreived from System Cache  :" + entitySetName + " from Cache. Time Taken " + cstopWatch.Elapsed.TotalMilliseconds);
                                    }
                                }
                            }

                            #endregion
                            else
                            {

                                #region Getting from DB and adding to Cache
                                lstLookUpData = new List<TEntity>();
                                ObjectQuery<DbDataRecord> _DbDataRecords = GetEnitityData(context, entitySetName);
                                foreach (var rec in _DbDataRecords)
                                {
                                    lstLookUpData.Add((TEntity)rec.GetValue(0));
                                }

                                #region Adding to Sql CacheDependency
                                if (CachingDependencyService.IsCacheDependencyEnabled && !lstLkpNotDependent.Contains(typeof(TEntity).Name))
                                {
                                    if (connectionString.IsNotNull() && tenantId.IsNull())
                                    {
                                        if (ProcessDependency(entitySetName, _DbDataRecords.ToTraceString()))
                                        {
                                            if (logEnable)
                                            {
                                                if (Logger != null)
                                                {
                                                    Logger.Info("CachingDependencyService : Added SQL Dependency for look up table :-" + entitySetName);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ProcessDependency(entitySetName + keyValue, _DbDataRecords.ToTraceString(), tenantId))
                                        {
                                            if (logEnable)
                                            {
                                                if (Logger != null)
                                                {
                                                    Logger.Info("CachingDependencyService : Added SQL Dependency for look up table :-" + entitySetName);
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                if (AppFabricCacheService.AddSystemCache(entitySetName + keyValue, lstLookUpData))
                                {
                                    if (logEnable)
                                    {
                                        if (Logger != null)
                                        {
                                            Logger.Info("AppFabricCacheService : Added to System Cache Server:" + entitySetName + " in Cache. Time Take " + stopWatch.ElapsedMilliseconds);
                                        }
                                    }
                                }

                                #endregion
                            }

                            break;
                            #endregion
                        #endregion
                        #region No Cache
                        case SysXCachingConst.CACHETYPENONE:

                            lstLookUpData = new List<TEntity>();
                            ObjectQuery<DbDataRecord> _DbDataRecord = GetEnitityData(context, entitySetName);
                            foreach (var rec in _DbDataRecord)
                            {
                                lstLookUpData.Add((TEntity)rec.GetValue(0));
                            }

                            break;
                        default:
                            break;
                        #endregion
                    }
                }
                return lstLookUpData;
            }
            catch (SysXException ex)
            {
                if (Logger != null)
                {
                    Logger.Error("Error in AppFabricCacheService:", ex);
                }
                return null;
                throw ex;
            }
        }

        /// <summary>
        /// Create SyxDependencyInfo Object and call Dependency service to Add SQL Query Noticfication
        /// </summary>
        /// <param name="Key">Cache Key</param>
        /// <param name="ObjectQuery">select query</param>
        /// <returns>True if Success</returns>
        public static bool ProcessDependency(string Key, string ObjectQuery, Int32? tenantId)
        {
            if (!CachingDependencyService.IsCacheDependencyEnabled)
            {
                return false;
            }
            String connectionString = null;
            if (tenantId.IsNotNull())
            {
                connectionString = GetLookUpCodebyID<ClientDBConfiguration>(fx => fx.CDB_TenantID == tenantId, fd => fd.CDB_ConnectionString);
            }
            SyxDependencyInfo DependencyInfo = new SyxDependencyInfo(connectionString);
            DependencyInfo.CacheKey = Key;
            DependencyInfo.SelectQuery = ObjectQuery.Replace("\r\n[Extent1].", "").Replace("\r\n", "").Replace("[Extent1].", "").Replace("AS [Extent1]", "");
            CachingDependencyService.AddSqlDependencies(Key, DependencyInfo);
            return true;
        }

        /// <summary>
        /// Create SyxDependencyInfo Object and call Dependency service to Add SQL Query Noticfication
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="ObjectQuery"></param>
        /// <returns></returns>
        public static bool ProcessDependency(string Key, string ObjectQuery)
        {
            if (!CachingDependencyService.IsCacheDependencyEnabled)
            {
                return false;
            }
            String connectionString = GetADOConnectionString();
            SyxDependencyInfo DependencyInfo = new SyxDependencyInfo(connectionString);
            DependencyInfo.CacheKey = Key;
            DependencyInfo.SelectQuery = ObjectQuery.Replace("\r\n[Extent1].", "").Replace("\r\n", "").Replace("[Extent1].", "").Replace("AS [Extent1]", "");
            CachingDependencyService.AddSqlDependencies(Key, DependencyInfo);
            return true;
        }

        /// <summary>
        /// Get Messaging Connection String
        /// </summary>
        /// <returns></returns>
        private static string GetADOConnectionString()
        {
            ADBMessageDB_DevEntities ctx = new ADBMessageDB_DevEntities(); //create your entity object here
            EntityConnection ec = (EntityConnection)ctx.Connection;
            SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
            string adoConnStr = sc.ConnectionString;
            return adoConnStr;
        }

        /// <summary>
        /// Method to Clear AppFabric Cache
        /// </summary>
        public static void Clear()
        {
            try
            {
                Parallel.ForEach(AppFabricCacheService.GetSystemRegions(), region =>
                {
                    AppFabricCacheService.ClearRegion(region);
                    var sysRegion = AppFabricCacheService.GetSystemRegionName(region);
                    AppFabricCacheService.ClearRegion(sysRegion);
                });
            }
            catch (System.Exception ex)
            {
                Logger.Error("Error in AppFabricCacheService:", ex);
                throw ex;
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Get all Data from Database for Entity
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="entitySetName">entitySetName</param>
        /// <returns>ObjectQuery<DbDataRecord></returns>
        private static ObjectQuery<DbDataRecord> GetEnitityData(ObjectContext context, string entitySetName)
        {

            EntityContainer container = context.MetadataWorkspace.GetItems<EntityContainer>
                                                                (DataSpace.CSpace).First();

            EntitySetBase entitySetBase = container.BaseEntitySets
                .FirstOrDefault(set => set.Name == entitySetName);

            StringBuilder stringBuilder = new StringBuilder().Append("SELECT entity ");
            stringBuilder.Append(" FROM " + container.Name.Trim() + "." + entitySetBase.Name + " AS entity");
            ObjectQuery<DbDataRecord> query = new ObjectQuery<DbDataRecord>(stringBuilder.ToString(), context);

            return query;
        }
        #endregion

        #endregion
    }
}