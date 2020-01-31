#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ISysXCachingDependenctService.cs
// Purpose:   Interface  Caching Service
//

#endregion

#region Namespace

#region System Defined

using CoreWeb.IntsofCachingModel.Services;
using System;

#endregion

#region Application Defined
#endregion

#endregion

namespace CoreWeb.IntsofCachingModel.Interface.Services
{
    public interface ISysXCachingDependencyService
    {
        /// <summar
        /// Sets up monitoring for SqlDependency Management.
        /// </summary>
        /// <param name="dbConnectionString">SQL Server DB connection String for which to monitor dependencies</param>
        /// <returns>Returns status</returns>
        Boolean StartDependency(string dbConnectionString);

        /// <summary>
        /// Sets up monitoring for SqlDependency Changes.
        /// </summary>
        /// <param name="dbConnectionString">SQL Server DB connection String for which to stop monitoring dependencies</param>
        /// <returns>Returns Status</returns>
        Boolean StopDependency(string dbConnectionString);

        /// <summary>
        /// Method to Add Sql Dependency 
        /// </summary>
        /// <param name="Key">Cache Key</param>
        /// <param name="dependencyInfo">Dependency Info Class Object</param>
        void AddSqlDependencies(string Key, SyxDependencyInfo dependencyInfo);

        /// <summary>
        /// To check if Cache Dependency is enabled
        /// </summary>
        Boolean IsCacheDependencyEnabled
        {
            get;
        }
    }
}