#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageRolePresenter.cs
// Purpose:   
//

#endregion

#region Namespace

#region System Defined

using System;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Business.RepoManagers;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class has the method's implementation which performs all the CRUD(Create/ Read/ Update/ Delete) operation for managing roles with its details.
    /// </summary>
    public class ManageRolePresenter : Presenter<IManageRoleView>
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
        /// This method is invoked by the view every time it loads.
        /// </summary>
        public override void OnViewLoaded()
        {
            if (!View.IsAdmin)
            {
                if (!View.LoginUserProductId.IsNull())
                {
                    View.ViewContract.LoginUserProductName = SecurityManager.GetTenantProduct((Int32)View.LoginUserProductId).Name;
                }
            }

            View.TenantsRole = SecurityManager.GetTenant(View.ViewContract.TenantId);
        }

        /// <summary>
        /// This method will wet the value of property for the list of products getting bind to the product dropdown.
        /// </summary>
        public void BindComboWithProducts(Int32? loggedInUserTenantId, Boolean isSysXAdmin)
        {
            var products = isSysXAdmin ? SecurityManager.GetProducts().ToList() : SecurityManager.GetProductsForTenant(loggedInUserTenantId ?? 0).ToList();
            View.TenantProducts = products;
            if (products.IsNotNull() && !isSysXAdmin)
            {
                View.ViewContract.TenantProductId = products.FirstOrDefault().TenantProductID;
            }
        }

        /// <summary>
        /// Performs a delete operation for role details.
        /// </summary>
        public void DeleteRoleDetail()
        {
            RoleDetail roleDetail = SecurityManager.GetRoleDetailById(View.ViewContract.RoleDetailId);

            if (SecurityManager.IsRoleInUse(View.ViewContract.RoleDetailId))
            {
                throw new SysXException(SysXUtils.GetMessage(ResourceConst.SECURITY_ROLE_REMOVE_MESSAGE) +
                                    SysXUtils.GetMessage(ResourceConst.SPACE) + roleDetail.Name.Split(new char[] { '_' }).FirstOrDefault() +
                                    SysXUtils.GetMessage(ResourceConst.SPACE) +
                                    SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE_DETAILS));
            }
            else
            {
                roleDetail.IsDeleted = true;
                roleDetail.ModifiedByID = View.CurrentUserId;
                roleDetail.ModifiedOn = DateTime.Now;
                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.ROLE) + SysXUtils.GetMessage(ResourceConst.SPACE) + roleDetail.Name.Split(new char[] { '_' }).FirstOrDefault() + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.DELETED_SUCCESSFULLY);
                roleDetail.Name = roleDetail.Name + "_" + Guid.NewGuid();
                SecurityManager.DeleteRoleDetail(roleDetail);
            }
        }

        /// <summary>
        /// Retrieves a list of all roles with it's details.
        /// </summary>
        public void RetrievingRoleDetails()
        {
            if (View.ViewContract.TenantId > AppConsts.NONE)
            {
                var tenantProductId = SecurityManager.GetTenantProductId(View.ViewContract.TenantId) ?? default(Int32);
                View.ViewContract.TenantProductId = tenantProductId;

                View.RoleDetails = SecurityManager.GetRoleDetailsByProductId(tenantProductId).ToList();
            }
            else
            {
                View.RoleDetails = SecurityManager.GetRoleDetail(View.IsAdmin, View.CurrentUserId).ToList();
            }
        }

        /// <summary>
        /// Performs an insert operation for role.
        /// </summary>
        public void RoleDetailSave()
        {

            var tenantId = SecurityManager.GetTenantProduct(View.ViewContract.TenantProductId).TenantID;

            if (SecurityManager.IsRoleExists(View.ViewContract.TenantProductId, View.ViewContract.Name + "_" + tenantId))
            {
                View.ErrorMessage = View.ViewContract.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_ROLE_EXISTS);
            }
            else
            {
                View.ErrorMessage = String.Empty;
                RoleDetail roleDetail = new RoleDetail
                                            {
                                                Name = View.ViewContract.Name,
                                                Description = View.ViewContract.Description,
                                                TenantProduct = SecurityManager.GetTenantProduct(View.ViewContract.TenantProductId),
                                                CreatedByID = View.CurrentUserId,
                                                CreatedOn = DateTime.Now,
                                                IsActive = true,
                                                IsDeleted = false,
                                                IsUserGroupLevel = View.ViewContract.IsUserGroupLevel,
                                                ShowAdminEntryDashboard = View.ViewContract.ShowAdminEntryPortal //Admin Entry Portal
                                            };

                SecurityManager.AddRoleDetail(roleDetail);
                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.ROLE) + SysXUtils.GetMessage(ResourceConst.SPACE) + roleDetail.Name.Split(new char[] { '_' }).FirstOrDefault() + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
            }
        }

        /// <summary>
        ///  Performs an update operation for role details.
        /// </summary>
        public void RoleDetailUpdate()
        {
            RoleDetail roleDetail = SecurityManager.GetRoleDetailById(View.ViewContract.RoleDetailId);
            var tenantId = SecurityManager.GetTenantProduct(View.ViewContract.TenantProductId).TenantID;

            if (!roleDetail.Name.EndsWith("_" + tenantId.ToString()))
            {
                roleDetail.Name = roleDetail.Name + "_" + tenantId;
            }

            if (SecurityManager.IsRoleExists(View.ViewContract.TenantProductId, View.ViewContract.Name + "_" + tenantId, roleDetail.Name))
            {
                View.ErrorMessage = View.ViewContract.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_ROLE_EXISTS);
            }
            else
            {
                View.ErrorMessage = String.Empty;
                roleDetail.Name = View.ViewContract.Name;
                roleDetail.Description = View.ViewContract.Description;
                roleDetail.ModifiedByID = View.CurrentUserId;
                roleDetail.ModifiedOn = DateTime.Now;
                roleDetail.IsActive = true;
                roleDetail.IsDeleted = false;
                roleDetail.IsUserGroupLevel = View.ViewContract.IsUserGroupLevel;
                roleDetail.ShowAdminEntryDashboard = View.ViewContract.ShowAdminEntryPortal; //Ádmin  Entry Portal

                SecurityManager.UpdateRoleDetail(roleDetail);
                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.ROLE) + SysXUtils.GetMessage(ResourceConst.SPACE) + roleDetail.Name.Split(new char[] { '_' }).FirstOrDefault() + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.UPDATED_SUCCESSFULLY);
            }
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}