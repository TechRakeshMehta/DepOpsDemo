#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  MapProductFeaturePresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Collections.Generic;
using INTSOF.SharedObjects;

#endregion

#region Application Specific

using Business.RepoManagers;
using Entity;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class has the method's implementation related to mapping of product with it's features.
    /// </summary>
    public class MapProductFeaturePresenter : Presenter<IMapProductFeatureView>
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
        /// Retrieves a list of all permissions.
        /// </summary>
        /// <returns></returns>
        public List<Permission> GetPermissionList()
        {
            View.ProductFeatures = SecurityManager.GetFeaturesForProduct(View.ViewContract.ProductId).ToList();
            return SecurityManager.GetPermissions().ToList();
        }

        /// <summary>
        /// Retrieves a list of all Line of businesses.
        /// </summary>
        public void RetrievingBlocks()
        {
            View.Blocks = SecurityManager.GetLineOfBusinesses();
        }

        /// <summary>
        /// Handles the mapping for features.
        /// </summary>
        public void MappingFeature()
        {
            SecurityManager.ProductFeatureMapping(SecurityManager.GetTenantProduct(View.ViewContract.ProductId), View.FeatureMappings.ToList(), View.ViewContract.UpdatedSysXBlockIDs, View.OrganizationUserId);
            View.RedirectToManageTenant();
        }

        public Boolean IsFeaturePermissionUsedByRole()
        {
            return SecurityManager.GetFeatureWithPermissionUsedByRole(View.FeatureMappings.ToList()).Any();
        }

        /// <summary>
        /// Handles ViewInit events.
        /// </summary>
        public void ViewInit()
        {
            View.Permissions = SecurityManager.GetPermissions().ToList();
        }

        /// <summary>
        /// Retrieves product features.
        /// </summary>
        public void RetrievingProductFeature()
        {
            View.Features = SecurityManager.GetFeaturesForLineOfBusinessAndAllowDelegation(View.ViewContract.BlockId);
        }

        /// <summary>
        /// This method checks is the feature is associated with role?
        /// </summary>
        public void CheckFeatureInRole()
        {
            List<Int32> featureIdLists = new List<Int32>();
            IEnumerable<TenantProductFeature> tenantProductFeatureList = SecurityManager.GetFeaturesForProduct(View.ViewContract.ProductId);

            foreach (var item in View.ViewContract.UpdatedSysXBlockIDs.ToList())
            {
                featureIdLists.AddRange(
                    tenantProductFeatureList.Where(
                        eachFeature => eachFeature.SysXBlocksFeature.lkpSysXBlock.SysXBlockId == item).Select(
                            eachFeature => eachFeature.SysXBlockFeatureID));
            }

            foreach (var tenantProductFeature in View.FeatureMappings.ToList())
            {
                var tenantProductFeatures = tenantProductFeatureList.Where(featureDetails => featureDetails.SysXBlockFeatureID == tenantProductFeature.SysXBlockBlockId).FirstOrDefault();

                if (tenantProductFeatures.IsNull())
                {
                    featureIdLists.Remove(tenantProductFeature.SysXBlockBlockId);
                }
                else
                {
                    featureIdLists.Remove(tenantProductFeature.SysXBlockBlockId);
                }
            }

            if (!featureIdLists.IsNull())
            {
                foreach (var flag in featureIdLists.Select(tenantProductFeature => SecurityManager.CheckFeatureAssignToRole(tenantProductFeature, View.ViewContract.ProductId)).Where(flag => flag))
                {
                    View.ViewContract.CheckInRole = true;
                    break;
                }
            }

            if (View.ViewContract.CheckInRole)
            {
                throw new SysXException(SysXUtils.GetMessage(ResourceConst.SECURITY_ROLE_ASIGN));
            }
        }

        /// <summary>
        /// This method counts the number of feature in a block.
        /// </summary>
        public void RetrievingBlockFeatureCount()
        {
            View.ViewContract.FeatureCount = SecurityManager.GetFeaturesForLineOfBusinessAndAllowDelegation(View.ViewContract.BlockId).Count() > AppConsts.NONE ? SecurityManager.GetFeaturesForLineOfBusinessAndAllowDelegation(View.ViewContract.BlockId).Count() : AppConsts.NONE;
        }

        /// <summary>
        /// Retrieves the role's details based on product Id.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <param name="currentProdcutId">.</param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get role details by product
        /// identifier in this collection.
        /// </returns>
        public IQueryable<RoleDetail> GetRoleDetailsByProductId(Int32 currentProdcutId)
        {
            return SecurityManager.GetRoleDetailsByProductId(currentProdcutId);
        }

        /// <summary>
        /// Retrieves all the relations from RolePermissionProductFeature table.
        /// </summary>
        /// <exception cref="SysXException">Thrown when a system x coordinate error condition occurs.</exception>
        /// <exception cref="Exception">    Thrown when an exception error condition occurs.</exception>
        public Boolean IsPermissionUsedByRole(Guid roleDetailId, Int32 currentFeatureId, Int32 currentProdcutId)
        {
            return SecurityManager.GetRolePermissionProductFeatures().Where(
                                   condition =>
                                   condition.RoleId == roleDetailId &&
                                   condition.SysXBlockFeatureId == currentFeatureId &&
                                   condition.PermissionId == currentProdcutId).Count() > AppConsts.NONE;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}