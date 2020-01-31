#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  MapRoleFeaturePresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System.Linq;
using System.Collections.Generic;
using INTSOF.SharedObjects;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Business.RepoManagers;
using INTSOF.UI.Contract.SysXSecurityModel;
using System;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class has the method's implementation related to mapping of Roles's with its features.
    /// </summary>
    public class MapRoleFeaturePresenter : Presenter<IMapRoleFeatureView>
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Retrieves a list of all blocks.
        /// </summary>
        public void RetrievingBlocks()
        {
            View.Blocks = SecurityManager.GetUserLineOfBusinessesByRoleId(View.ViewContract.RoleId);
        }

        /// <summary>
        /// Retrieves mapping between role and features.
        /// </summary>
        public void RoleFeatureMapping(List<RoleFeatureActionContract> roleFeatureActions)
        {
            SecurityManager.RoleFeatureMapping(View.ViewContract.RoleId, View.ProductId, View.ViewContract.FeaturePermissions, View.ViewContract.UpdatedSysXBlockIDs, roleFeatureActions);
            //UAT-3228
            //if (!View.ViewContract.FeaturePermissions.IsNullOrEmpty())
            //{
                List<String> lstRoles = new List<string>();
                lstRoles.Add(View.ViewContract.RoleId);
                //foreach (KeyValuePair<Int32, Int32> kvp in View.ViewContract.FeaturePermissions)
                //{
                    SecurityManager.InsertDefaultColumnConfiguration(View.CurrentUserId, null, lstRoles);
                //}
            //}
            View.RedirectToManageRole();
        }

        /// <summary>
        /// Retrieves product features.
        /// </summary>
        public void RetrievingProductFeature()
        {
            View.RoleFeatures = SecurityManager.GetFeatureForRole(View.ViewContract.RoleId, View.ViewContract.BlockId).ToList();
            View.ProductFeatures = SecurityManager.GetTenantProductFeaturesForLineOfBusiness(View.ProductId, View.ViewContract.BlockId).ToList();
        }

        /// <summary>
        /// Retrieves list of all features associated with the block.
        /// </summary>
        public void RetrievingBlockFeatureCount()
        {
            View.ViewContract.FeatureCount = SecurityManager.GetFeatureForRole(View.ViewContract.RoleId, View.ViewContract.BlockId).Count() > AppConsts.NONE ? SecurityManager.GetFeatureForRole(View.ViewContract.RoleId, View.ViewContract.BlockId).Count() : AppConsts.NONE;
        }

        /// <summary>
        /// Retrieves a list of all permissions.
        /// </summary>
        /// <returns></returns>
        public List<Permission> GetPermissionList()
        {
            return SecurityManager.GetPermissions().ToList();
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}