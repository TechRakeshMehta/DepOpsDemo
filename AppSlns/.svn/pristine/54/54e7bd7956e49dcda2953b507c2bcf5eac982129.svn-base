#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXSQLCacheService.cs
// Purpose:   SysX SQL Caching Service Class
//

#endregion

#region Namespace

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Caching;
using System.Data;

#endregion

#region Application Defined
using INTSOF.Logger;
using INTSOF.Utils;
using INTSOF.Logger.factory;
using CoreWeb.IntsofCachingModel.Config;
using CoreWeb.IntsofCachingModel.Interface.Services;
#endregion

#endregion

namespace CoreWeb.IntsofCachingModel.Services
{
    /// <summary>
    /// SQL Cache Service Class
    /// </summary>
    public class SysXSQLCacheService : ISysXSQLCacheService, IDisposable
    {
        #region Private Variables

        private String _connectionString = ConfigurationManager.ConnectionStrings[SysXCachingConst.CONNECTION_STRING_CACHING].ConnectionString;
        private String _dbName;
        private ILogger _logger;
        private static List<String> _sqlCacheTables = new List<String>();

        #endregion

        #region Class Constructor

        /// <summary>
        /// Constructor SysXSQLCacheService
        /// </summary>
        public SysXSQLCacheService()
        {
            try
            {
                _logger = SysXLoggerFactory.GetInstance().GetLogger();
                SqlConnectionStringBuilder conStrBuilder = new SqlConnectionStringBuilder();
                conStrBuilder.ConnectionString = _connectionString;
                _dbName = conStrBuilder.InitialCatalog;

                this.startSQLCache(); //Enable DataBase SQL Cache  In case User has Minimum rights
                
                this.EnableTablesForSQLCache(); // Enable Tables SQL Cache
            }
            catch (Exception ex)
            {
                _logger.Error("Error Message on SysXSQLCacheService() Constructor :" + ex.StackTrace);
            }
        }
                
        /// <summary>
        /// Enable Tables For SQLCache
        /// </summary>
        private void EnableTablesForSQLCache()
        {
            try
            {
                //Enable All the tables for SQL Cache.
                SysXCacheConfiguration cache = (SysXCacheConfiguration)ConfigurationManager.GetSection(SysXCachingConst.CACHE_CONFIGURATION);

                String[] enabledTables = { };

                foreach (String tableName in cache.SqlCacheTables)
                {
                    if (enabledTables.Contains<String>(tableName, StringComparer.CurrentCultureIgnoreCase))
                    {
                        //Table is already enabled for SQL Cache
                        _sqlCacheTables.Add(tableName.ToLower());
                    }
                    else
                    {
                        this.EnableTableForSQLCache(tableName);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(SysXCachingConst.DETAILED_ERROR_MSG + ex.StackTrace);
            }
        }

        #endregion

        #region ISysXSQLCacheService Members

        /// <summary>
        /// Get the Sql Cashed Data  (Check for Null to Datatable Returned)
        /// </summary>
        /// <param name="tableName">TableName</param>
        /// <param name="cache">Cache Object</param>
        /// <returns>DataTable</returns>
        public DataTable GetSqlCashedData(String tableName, Cache cache)
        {
            try
            {
                if (!_sqlCacheTables.Contains(tableName.ToLower()))
                {
                    _logger.Error(SysXCachingConst.CACHE_TABLE + tableName + SysXCachingConst.CACHE_NOT_CONFIGURED);
                    return null;
                }

                DataTable dataTable = new DataTable(tableName);

                // Check for cache if it is then get it from cache
                if (!cache[tableName].IsNull())
                {
                    dataTable = (DataTable)cache[tableName];
                    //_logger.Error("Data Come from --Cach-- TableName  : " + tableName);
                }
                else
                {
                    using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                    {
                        String sqlQuery = SysXCachingConst.SELECT_STAR_FROM + tableName;
                        using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection))
                        {
                            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                            {
                                sqlDataAdapter.Fill(dataTable);
                            }
                        }
                        SqlCacheDependency dependency = new SqlCacheDependency("SysXAppDB", tableName);
                        cache[tableName] = dataTable;

                        Int32 slidingExpiration = Convert.ToInt32(ConfigurationManager.AppSettings["CacheSlidingExpiration"]);

                        cache.Insert(tableName, dataTable, dependency, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(slidingExpiration));
                        //_logger.Error("Data Come from --SQL Server-- TableName: " + tableName);
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                _logger.Error(SysXCachingConst.UNABLE_BUILD_CACHE_TABLE + tableName, ex);
            }
            return null;
        }

        /// <summary>
        /// GetSqlCashedData with TableName and Colums
        /// </summary>
        /// <param name="tableName">tableName</param>
        /// <param name="ColumnName">ColumnName</param>
        /// <param name="cache">cache</param>
        /// <returns></returns>
        public DataTable GetSqlCashedData(String tableName,List<String> ColumnNames, Cache cache)
        {
            try
            {
                if (!_sqlCacheTables.Contains(tableName.ToLower()))
                {
                    _logger.Error(SysXCachingConst.CACHE_TABLE + tableName + SysXCachingConst.CACHE_NOT_CONFIGURED);
                    return null;
                }

                DataTable dataTable = new DataTable(tableName);

                // Check for cache if it is then get it from cache
                if (!cache[tableName].IsNull())
                {
                    dataTable = (DataTable)cache[tableName];
                    //_logger.Error("Data Come from --Cach-- TableName  : " + tableName);
                }
                else
                {
                    using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                    {
                        String Colums = String.Join(",", ColumnNames.ToArray());

                        String sqlQuery = "Select " + Colums + " From " + tableName;

                        using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection))
                        {
                            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                            {
                                sqlDataAdapter.Fill(dataTable);
                            }
                        }
                        //== SysXAppDB is DataBaseEntryName. defined in webconfig in sqlCacheDependency Tag
                        SqlCacheDependency dependency = new SqlCacheDependency("SysXAppDB", tableName);
                        cache[tableName] = dataTable;

                        Int32 slidingExpiration = Convert.ToInt32(ConfigurationManager.AppSettings["CacheSlidingExpiration"]);
                        cache.Insert(tableName, dataTable, dependency, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(slidingExpiration));
                        //_logger.Error("Data Come from --SQL Server-- TableName: " + tableName);
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                _logger.Error(SysXCachingConst.UNABLE_BUILD_CACHE_TABLE + tableName, ex);
            }
            return null;
        }

        #endregion

        /// <summary>
        /// Start SQL Cache
        /// </summary>
        public void startSQLCache()
        {
            try
            {
                SqlDependency.Start(_connectionString);
            }
            catch (Exception ex)
            {
                _logger.Error(SysXCachingConst.UNABLE_TO_START_SQL_DEPENDENCY + ex.StackTrace);
            }
        }

        /// <summary>
        /// Stop SQL Cache
        /// </summary>
        public void stopSQLCache()
        {
            try
            {
                SqlDependency.Stop(_connectionString);
            }
            catch (Exception ex)
            {
                _logger.Error(SysXCachingConst.UNABLE_STOP_SQL_DEPENDENCY + ex.StackTrace);
            }
        }

        /// <summary>
        /// Dispose (stop the SQLCache)
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.stopSQLCache();
            }
            catch(Exception ex)
            {
                _logger.Error(SysXCachingConst.DETAILED_ERROR_MSG + ex.StackTrace);
            }
        }

        #region Private Methods

        /// <summary>
        /// Enable Data Base For SQLCache
        /// </summary>
        private void EnableDatabaseForSQLCache()
        {
            try
            {
                //Enables SqlCacheDependency change notifications on the specified database.            
                SqlCacheDependencyAdmin.EnableNotifications(_connectionString);
            }
            catch (Exception ex)
            {
                _logger.Error(SysXCachingConst.UNABLE_TO_ENABLE_DATABASE + _dbName + SysXCachingConst.FOR_SQL_CACHE_NOTIFICATION + ex.StackTrace);
            }
        }

        /// <summary>
        /// Disable Data Base For SQL Cache
        /// </summary>
        private void DisableDataBaseForSQLCache()
        {
            try
            {
                SqlCacheDependencyAdmin.DisableNotifications(_connectionString);
            }
            catch (Exception ex)
            {
                _logger.Error(SysXCachingConst.UNABLE_TO_DISABLE_DATABASE + _dbName + SysXCachingConst.FOR_SQL_CACHE_NOTIFICATION + ex.StackTrace);
            }
        }

        /// <summary>
        /// Enable Table For SQL Cache
        /// </summary>
        /// <param name="tableName">String tableName</param>
        private void EnableTableForSQLCache(String tableName)
        {
            try
            {
                SqlCacheDependencyAdmin.EnableTableForNotifications(_connectionString, tableName);
                _sqlCacheTables.Add(tableName.ToLower());
            }
            catch (Exception ex)
            {
                _logger.Error(SysXCachingConst.UNABLE_TO_ENABLE_TABLE + tableName + SysXCachingConst.FOR_SQL_CACHE + ex.StackTrace);
            }
        }

        /// <summary>
        /// Disable Table For SQL Cache
        /// </summary>
        /// <param name="tableName">String tableName</param>
        private void DisableTableForSQLCache(String tableName)
        {
            try
            {
                SqlCacheDependencyAdmin.DisableTableForNotifications(_connectionString, tableName);
            }
            catch (Exception ex)
            {
                _logger.Error(SysXCachingConst.UNABLE_TO_DISABLE_TABLE + tableName + SysXCachingConst.FOR_SQL_CACHE + ex.StackTrace);
            }
        }

        #endregion

    }
}