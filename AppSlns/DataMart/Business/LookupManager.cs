#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  LookupManager.cs
// Purpose:   To get Lookup Data
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core.Objects.DataClasses;


#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using INTSOF.AppFabricCacheServer;
using System.Data.Entity.Core.EntityClient;
#endregion

#endregion

namespace DataMart.Business.RepoManagers
{
    /// <summary>
    /// LookupManager
    /// </summary>
    public static class LookupManager
    {

        public static List<TEntity> GetSharedDBLookUpData<TEntity>() where TEntity : EntityObject
        {
            return SysXCacheUtils.GetAddCacheLookup<TEntity>(ADBDatabaseDetail.ADBDatabaseType.SHAREDDATA_DB.GetStringValue()).ToList();
        }

        /// <summary>
        /// Get Id of Look Up, in the SharedDatabase
        /// </summary>
        /// <returns>Id </returns>
        public static Int32 GetSharedLookUpIDbyCode<TEntity>(Func<TEntity, bool> predicate) where TEntity : EntityObject
        {
            TEntity Enity = SysXCacheUtils.GetAddCacheLookup<TEntity>(ADBDatabaseDetail.ADBDatabaseType.SHAREDDATA_DB.GetStringValue()).SingleOrDefault<TEntity>(predicate);
            return Convert.ToInt32(Enity.EntityKey.EntityKeyValues.FirstOrDefault().Value);
        }

        /// <summary>
        /// Get Id of Look Up
        /// </summary>
        /// <returns>Id </returns>
        public static Int32 GetLookUpIDbyCode<TEntity>(Func<TEntity, bool> predicate) where TEntity : EntityObject
        {
            TEntity Enity = SysXCacheUtils.GetAddCacheLookup<TEntity>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()).SingleOrDefault<TEntity>(predicate);
            return Convert.ToInt32(Enity.EntityKey.EntityKeyValues.FirstOrDefault().Value);
        }

        /// <summary>
        /// Get code of Look Up
        /// </summary>
        /// <returns>Id </returns>
        public static String GetLookUpCodebyID<TEntity>(Func<TEntity, bool> predicate, Func<TEntity, String> selector) where TEntity : EntityObject
        {
            return SysXCacheUtils.GetAddCacheLookup<TEntity>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()).Where(predicate).Select(selector).FirstOrDefault();
            //return Convert.ToInt32(Enity.EntityKey.EntityKeyValues.FirstOrDefault().Value);
        }

        /// <summary>
        /// Get Connection String
        /// </summary>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        private static string GetConnectionString(Int32? TenantId)
        {
            if (TenantId.IsNotNull())
            {

                EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                string tenantConnectionString = GetLookUpCodebyID<ClientDBConfiguration>(fx => fx.CDB_TenantID == TenantId, fd => fd.CDB_ConnectionString);
                entityBuilder.ProviderConnectionString = tenantConnectionString; 
                entityBuilder.Provider = "System.Data.SqlClient";
                entityBuilder.Metadata = @"res://*/ClientEntity.ADBClientEntity.csdl|res://*/ClientEntity.ADBClientEntity.ssdl|res://*/ClientEntity.ADBClientEntity.msl";
                return entityBuilder.ToString();
            }
            return null;
        }
    }
}
