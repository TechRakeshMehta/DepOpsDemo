#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename: SyxDependencyInfo
// Purpose:  Object to store Dependency Information
//

#endregion
#region Namespace

#region System Defined
using System;
using System.Configuration;
using System.Data.SqlClient;
#endregion
#region Application Defined
#endregion
#endregion
namespace CoreWeb.IntsofCachingModel.Services
{

    [Serializable]

    public class SyxDependencyInfo
    {
        #region Private Variables

        private static string _DBSubscriberConnectionString = ConfigurationManager.ConnectionStrings["SysXAppDBSubscriberCaching"].ConnectionString;


        #endregion

        #region Public Properties
        /// <summary>
        /// Cache key
        /// </summary>
        public string CacheKey { get; set; }
        /// <summary>
        /// Select Statement Query  for SQL Dependency 
        /// </summary>
        public string SelectQuery { get; set; }

        /// <summary>
        /// Connection String
        /// </summary>
        public string DBSubscriberConnectionString { get; set; }

        #endregion
        public SyxDependencyInfo(String dbConnectionString = null)
        {
            SqlConnectionStringBuilder _connectionString = new SqlConnectionStringBuilder(_DBSubscriberConnectionString);
            if (dbConnectionString != null)
            {
                SqlConnectionStringBuilder _dbConnectionString = new SqlConnectionStringBuilder(dbConnectionString);
                _connectionString.InitialCatalog = _dbConnectionString.InitialCatalog;
                _connectionString.Pooling = true;
                _connectionString.MaxPoolSize = 5;
                _connectionString.PersistSecurityInfo = true;
            }
            DBSubscriberConnectionString = _connectionString.ConnectionString;

        }

    }
}
