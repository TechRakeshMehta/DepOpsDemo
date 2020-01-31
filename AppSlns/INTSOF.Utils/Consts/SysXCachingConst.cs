#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXCachingConst.cs
// Purpose:   SysX Caching Const
//

#endregion

#region Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace INTSOF.Utils
{
    /// <summary>
    /// SysXCachingConst
    /// </summary>
    public class SysXCachingConst
    {
        public const String SQL_CACHE_TABLES_CACHETABLE = "sqlCacheTables/cacheTable";
        public const String CACHE_ITEM_PRIORITY_HIGH = "High";
        public const String CACHE_ITEM_PRIORITY_LOW = "Low";
        public const String CACHE_ITEM_PRIORITY_NONE = "None";
        public const String CACHE_PATH = "cachePath";
        public const String CACHE_FILE_EXTENSION = ".dat";
        public const String CONNECTION_STRING_CACHING = "SysXAppDBCaching";
        public const String CACHE_CONFIGURATION = "sysxCacheConfiguration";
        public const String CACHE_TABLE = "Table ";
        public const String CACHE_NOT_CONFIGURED = "is not configured for SQL Cache. Please add this table in Web.Config file for SQL caching.";
        public const String SELECT_STAR_FROM = "SELECT * FROM ";
        public const String UNABLE_BUILD_CACHE_TABLE = "Unable to build cache for table ";
        public const String UNABLE_TO_START_SQL_DEPENDENCY = "Unable to start SQL dependency on connection string named: SysXAppDBCaching, Sql Exception Message:- ";
        public const String UNABLE_STOP_SQL_DEPENDENCY = "Unable to Stop SQL dependency on connection string named: SysXAppDBCaching , Sql Exception Message:- ";
        public const String UNABLE_TO_ENABLE_DATABASE = "Unable to enable database ";
        public const String FOR_SQL_CACHE_NOTIFICATION = " for SQL Cache Notification. , Sql Exception Message:-";
        public const String UNABLE_TO_DISABLE_DATABASE = "Unable to disable database ";
        public const String UNABLE_TO_ENABLE_TABLE = "Unable to enable table ";
        public const String FOR_SQL_CACHE = " for SQL Cache , Sql Exception Message:-";
        public const String UNABLE_TO_DISABLE_TABLE = "Unable to disable table ";
        public const String DETAILED_ERROR_MSG = "Detailed Error : ";
        public const String FAILED_START_CACH_SERVER = "Failed to start SysX Caching Server.";
        public const String CONNECTION_STRING_VIEWSTATESTORE = "ConnectionName";
        public const String SP_STORE_VIEWSTATE = "StoreViewState";
        public const String SP_GET_VIEWSTATE = "GetViewState";
        public const String SP_DELETE_VIEWSTATE = "DeleteViewState";
        public const String SP_PARAMETER_RETURNVALUE = "@ReturnValue";
        public const String SP_PARAMETER_ID = "@Id";
        public const String SP_PARAMETER_VIEWSTATEDATA = "@ViewStateData";
        public const String SP_PARAMETER_OVERWRITEID = "@OverWriteId";
        public const String SP_PARAMETER_ISOVERWRITABLE = "@IsOverWritable";
        public const String NULLID_NOTIFICATION = "The parameters SessionId, Page URL or content cannot be empty or null";
        public const String DELIMETER_SESSIONPAGEURL = "|";
        public const String MASTER = "M";
        public const String MASTERPAGEFILE = @"/AppMaster.master";
        public const String LKPNONSQLDEPENDENT = "lkpTableNonSqlDependent";
        public const String CALLINGPROTOCOL = "CallingProtocol";
        public const String APPFABRICCACHENAME = "CacheName";
        public const String CACHETYPE = "Cachetype";
        public const String CACHETYPEAPPFABRIC = "AppFabric";
        public const String CACHETYPESYSTEM = "System";
        public const String CACHETYPENONE = "None";
        public const String CACHETYPEREDIS = "Redis";
        public const String ISCACHELOGENABLE = "IsCacheLogEnable";
        public const String STARTCACHEDEPENDENCY = "StartCacheDependency";
        public const String WIDGET_WEBSERVICE_URI = "WidgetServiceUri";
        public const String CUSTOMVIEWSTATESECTIONNAME = "MyCustomCacheProvider";
        public const String CUSTOMCACHEPROVIDERSECTIONNAME = "MyCustomCacheProvider";
        public const String CUSTOMVIEWSTATETIMEOUTKEY = "ViewStateTimeOutMinutes";
        public const String CUSTOMVIEWSTATEPROVIDER = "ViewStateProvider";
        public const String CUSTOMREDISCACHEEXPIRYMINUTES = "RedisCacheExpiryMinutes";
    }
}
