#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ISysXSQLCacheService.cs
// Purpose:   Interface for SLQ Cache
//

#endregion

#region Namespace

#region System Defined
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Caching;
#endregion

#region Application Defined
#endregion

#endregion

namespace CoreWeb.IntsofCachingModel.Interface.Services
{
    /// <summary>
    /// For SQL Cache
    /// </summary>
    public interface ISysXSQLCacheService
    {
        /// <summary>
        /// Get the Cached data (Do Not Use)
        /// </summary>
        /// <param name="TableName">TableName</param>
        /// <param name="cache">Cache</param>
        /// <returns>DataTable</returns>
        [Obsolete("TODO: Do Not Use remove in future")]
        DataTable GetSqlCashedData(String TableName, Cache cache);

        /// <summary>
        /// Get the Cached data.
        /// </summary>
        /// <param name="tableName">tableName</param>
        /// <param name="ColumnNames">ColumnNames</param>
        /// <param name="cache">cache</param>
        /// <returns>DataTable</returns>
        DataTable GetSqlCashedData(String tableName, List<String> ColumnNames, Cache cache);
    }
}
