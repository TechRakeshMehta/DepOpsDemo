#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXCachingDependencyService.cs
// Purpose:   SysXCachingDependencyService
//

#endregion

#region Namespace

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Threading;
using System.Data;
using System.Web.Caching;
using System.Configuration;

#endregion

#region Application Defined

using INTSOF.Logger;
using INTSOF.Logger.factory;
using CoreWeb.IntsofCachingModel.Interface.Services;
using INTSOF.AppFabricCacheServer;
using INTSOF.Utils;
using Entity;
using Business.RepoManagers;
#endregion

#endregion

namespace CoreWeb.IntsofCachingModel.Services
{
    public class SysXCachingDependencyService : ISysXCachingDependencyService, IDisposable
    {
        #region Private Variables
        private static readonly List<SqlCacheDependencyObjects> lstSqlCacheDependencyObjects = new List<SqlCacheDependencyObjects>();
        private static readonly ReaderWriterLockSlim ReadWriteLock = new ReaderWriterLockSlim();
        private ILogger _logger;
        public static Boolean startCacheDependency = ConfigurationManager.AppSettings[SysXCachingConst.STARTCACHEDEPENDENCY] == null ? false : Convert.ToBoolean(ConfigurationManager.AppSettings[SysXCachingConst.STARTCACHEDEPENDENCY].ToLower());

        #endregion

        #region Public Variables

        #endregion

        #region Properties

        /// <summary>
        /// To check if Cache Dependency is started or enabled
        /// </summary>
        public Boolean IsCacheDependencyStarted
        {
            get;
            set;
        }

        /// <summary>
        /// To check if Cache Dependency is enabled
        /// </summary>
        public Boolean IsCacheDependencyEnabled
        {
            get
            {
                return startCacheDependency;
            }
        }

        #endregion

        public SysXCachingDependencyService()
        {
            _logger = SysXLoggerFactory.GetInstance().GetLogger();
            //StartDependency(ConfigurationManager.ConnectionStrings["SysXAppDBCaching"].ConnectionString);
            //SysXCachingDependencyServiceStart(true);
            SysXCachingDependencyServiceStart(startCacheDependency);
        }

        /// <summary>
        /// Start Caching Dependency Service for Security/Master database and multiple Tenants.
        /// </summary>
        /// <param name="isCallStartDependency"></param>
        public void SysXCachingDependencyServiceStart(Boolean isCallStartDependency)
        {
            if (!startCacheDependency)
                return;
            if (isCallStartDependency)
            {
                StartDependency(ConfigurationManager.ConnectionStrings["SysXAppDBCaching"].ConnectionString);
                StartDependency(ConfigurationManager.ConnectionStrings["MessagingDBCaching"].ConnectionString);
                List<ClientDBConfiguration> AllClientTypeTenants = SecurityManager.GetClientDBConfiguration().ToList();
                foreach (var tenant in AllClientTypeTenants)
                {
                    SqlConnectionStringBuilder startUser = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["SysXAppDBCaching"].ConnectionString);
                    SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder(tenant.CDB_ConnectionString);
                    connection.UserID = startUser.UserID;
                    connection.Password = startUser.Password;
                    connection.MaxPoolSize = 5;
                    connection.Pooling = true;
                    connection.PersistSecurityInfo = true;

                    String startUserConnString = connection.ConnectionString;
                    StartDependency(startUserConnString);
                }
            }
        }

        /// <summary>
        /// Start SQL Dependency
        /// </summary>
        /// <param name="dbConnectionString">Connection String With User having Start Permission</param>
        /// <returns>true if success</returns>
        public bool StartDependency(string dbConnectionString)
        {
            try
            {
                if (!startCacheDependency)
                    return false;
                SqlDependency.Start(dbConnectionString);
                IsCacheDependencyStarted = true;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Stop SQL Dependency
        /// </summary>
        /// <param name="dbConnectionString">Connection String With User having Start Permission</param>
        /// <returns>true if success</returns>
        public bool StopDependency(string dbConnectionString)
        {
            try
            {
                if (!startCacheDependency)
                    return false;
                if (IsCacheDependencyStarted)
                {
                    SqlDependency.Stop(dbConnectionString);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Add to SQL Dependency List Object and Create Notification with SyxDependencyInfo values
        /// </summary>
        /// <param name="Key">Cache Key</param>
        /// <param name="dependencyInfo">Class SyxDependencyInfo Object containing Dependency Information </param>
        public void AddSqlDependencies(string Key,
                                     SyxDependencyInfo dependencyInfo)
        {
            if (!startCacheDependency)
                return;
            if (IsCacheDependencyStarted)
            {
                SqlCacheDependencyObjects SqlCacheDependencyObject = new SqlCacheDependencyObjects();

                SqlCacheDependencyObjects SqlCacheDependencyObjectExists = (from dependency in lstSqlCacheDependencyObjects
                                                                            let InstanceKey = dependency.Key
                                                                            where InstanceKey.Equals(Key)
                                                                            select dependency).FirstOrDefault();
                try
                {
                    if (!string.IsNullOrEmpty(dependencyInfo.SelectQuery))
                    {
                        var sqlCommand = new SqlCommand(dependencyInfo.SelectQuery);
                        var sqlDependency = new SqlDependency(sqlCommand);
                        sqlDependency.OnChange += SqlDependency_OnChange;
                        using (var connection = new SqlConnection(dependencyInfo.DBSubscriberConnectionString))
                        {
                            if (connection.State == ConnectionState.Closed)
                                connection.Open();
                            sqlCommand.Connection = connection;
                            sqlCommand.ExecuteNonQuery();

                        }
                        SqlCacheDependencyObject = new SqlCacheDependencyObjects()
                        {
                            Key = Key,
                            DependencyData = dependencyInfo,
                            SqlDependencyInstance = sqlDependency
                        };

                        //Remove if already Exist  to refresh dependency Id in case of Cache Expiration
                        if (SqlCacheDependencyObjectExists != null)
                        {
                            lstSqlCacheDependencyObjects.Remove(SqlCacheDependencyObjectExists);
                        }

                        lstSqlCacheDependencyObjects.Add(SqlCacheDependencyObject);
                    }
                }
                catch (Exception ex)
                {

                    _logger.Error(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Clear Cache in case of SQL Update on DB
        /// </summary>
        /// <param name="Key">Cache Key</param>
        private void ClearCache(string Key)
        {
            if (SysXCacheUtils.CacheType.Equals(SysXCachingConst.CACHETYPEAPPFABRIC, StringComparison.CurrentCultureIgnoreCase))
            {
                SysXCacheUtils.AppFabricCacheService.Remove(Key);
            }
            else
                if (SysXCacheUtils.CacheType.Equals(SysXCachingConst.CACHETYPESYSTEM, StringComparison.CurrentCultureIgnoreCase))
                {
                    SysXCacheUtils.AppFabricCacheService.RemoveSystemCache(Key);
                }
        }

        /// <summary>
        /// Callback for SqlDependency Change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SqlDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {

            switch (e.Info)
            {
                case SqlNotificationInfo.Insert:
                case SqlNotificationInfo.Update:
                case SqlNotificationInfo.Delete:
                    try
                    {
                        SqlCacheDependencyObjects SqlCacheDependencyObject = new SqlCacheDependencyObjects();
                        ReadWriteLock.EnterReadLock();

                        SqlCacheDependencyObject = (from dependencyInfo in lstSqlCacheDependencyObjects
                                                    let sqlInstanceIds = dependencyInfo.SqlDependencyInstance.Id
                                                    where sqlInstanceIds.Equals((sender as SqlDependency).Id)
                                                    select dependencyInfo).FirstOrDefault();

                        ClearCache(SqlCacheDependencyObject.Key);
                        SqlCacheDependencyObject.SqlDependencyInstance.OnChange -= SqlDependency_OnChange;
                        lstSqlCacheDependencyObjects.Remove(SqlCacheDependencyObject);
                    }
                    finally
                    {
                        ReadWriteLock.ExitReadLock();
                    }

                    break;
                case SqlNotificationInfo.Query:
                case SqlNotificationInfo.Invalid:

                    SqlCacheDependencyObjects SqlCacheDependency = new SqlCacheDependencyObjects();

                    SqlCacheDependency = (from dependencyInfo in lstSqlCacheDependencyObjects
                                          let sqlInstanceIds = dependencyInfo.SqlDependencyInstance.Id
                                          where sqlInstanceIds.Equals((sender as SqlDependency).Id)
                                          select dependencyInfo).FirstOrDefault();
                    SqlCacheDependency.SqlDependencyInstance.OnChange -= SqlDependency_OnChange;
                    lstSqlCacheDependencyObjects.Remove(SqlCacheDependency);

                    _logger.Error("Query Notification not supported for " + SqlCacheDependency.Key);

                    break;
                default:

                    break;
            }
        }

        /// <summary>
        ///   //Stop Dependency
        /// </summary>
        public void Dispose()
        {
            if (!startCacheDependency)
                return;
            if (IsCacheDependencyStarted)
            {
                StopDependency(ConfigurationManager.ConnectionStrings["SysXAppDBCaching"].ConnectionString);
                StopDependency(ConfigurationManager.ConnectionStrings["MessagingDBCaching"].ConnectionString);
                List<ClientDBConfiguration> AllClientTypeTenants = SecurityManager.GetClientDBConfiguration().ToList();
                foreach (var tenant in AllClientTypeTenants)
                {
                    SqlConnectionStringBuilder startUser = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["SysXAppDBCaching"].ConnectionString);
                    SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder(tenant.CDB_ConnectionString);
                    connection.UserID = startUser.UserID;
                    connection.Password = startUser.Password;
                    connection.MaxPoolSize = 5;
                    connection.Pooling = true;
                    connection.PersistSecurityInfo = true;

                    String startUserConnString = connection.ConnectionString;
                    StopDependency(startUserConnString);
                }
            }
        }
    }

    /// <summary>
    /// List of SqlDependency with SyxDependencyInfo
    /// </summary>
    [Serializable]
    public class SqlCacheDependencyObjects
    {
        public string Key { get; set; }
        public SyxDependencyInfo DependencyData { get; set; }
        public SqlDependency SqlDependencyInstance { get; set; }

    }
}

