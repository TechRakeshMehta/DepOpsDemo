#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SecurityRepository.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Collections.Generic;

#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using INTSOF.AppFabricCacheServer;
using System.Data;
//using Microsoft.Data.Extensions;
using DataMart.DAL.Interfaces;

#endregion

#endregion

namespace DataMart.DAL.Repository
{
    /// <summary>
    /// This is the class for security module. 
    /// All functionality related to security module like : Role(Edit/Add/Delete), User(Edit/Add/Delete) will be written in this class only.
    /// </summary>
    public class SecurityRepository : BaseRepository, ISecurityRepository
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private SysXAppDBEntities _dbNavigation;
        private List<Int32> featureIdsToBeDeleted = new List<Int32>();

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public SecurityRepository()
        {
            _dbNavigation = base.Context;
        }

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the current repository context.
        /// </summary>
        /// <remarks></remarks>
        ISecurityRepository CurrentRepositoryContext
        {
            get
            {
                return this;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Retrieves a Connection String of Tenant
        /// </summary>
        /// <param name="tenantId">The tenantId.</param>

        /// <returns>
        /// </returns>
        String ISecurityRepository.GetClientConnectionString(Int32 tenantId)
        {
            //ClientDBConfiguration tenantDBCon = _dbNavigation.ClientDBConfigurations.Where(x => x.CDB_TenantID == tenantId).FirstOrDefault();
            //return tenantDBCon.IsNull() ? string.Empty : tenantDBCon.CDB_ConnectionString;
            string tenantConnectionString = SysXCacheUtils.GetAddCacheLookup<ClientDBConfiguration>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()).Where(fx => fx.CDB_TenantID == tenantId).Select(fd => fd.CDB_ConnectionString).FirstOrDefault();
            return tenantConnectionString;
        }

        #endregion

        #endregion
    }
}