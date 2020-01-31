#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageUserGroupPresenter.cs
// Purpose:   To Manage user group
//

#endregion

#region Namespace.

#region System defined namespace..

using System;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region Application Specific User Group.

using Entity;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.Utils.Consts;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    public class ManageUserGroupPresenter : Presenter<IManageUserGroupView>
    {
        #region Methods

        #region Private Methods

        #endregion

        #region Public Method

        /// <summary>
        /// On view load
        /// </summary>
        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {

        }

        /// <summary>
        /// Insert A User Group
        /// </summary>
        public void InsertUserGroup()
        {
            UserGroup userGroup = new UserGroup();
            userGroup.UserGroupName = View.ViewContract.UserGoupName;
            userGroup.UserGroupDesc = View.ViewContract.UserGroupDescription;
            userGroup.TenantID = View.ViewContract.TenantId;
            userGroup.IsActive = true;
            userGroup.CreatedOn = DateTime.Now;
            userGroup.CreatedByID = View.CreatedById;
            SecurityManager.InsertUserGroup(userGroup);
            View.SuccessMessage = SysXUtils.GetMessage(SysXSecurityConst.USERGROUPINSERTED);
        }

        /// <summary>
        /// Return All Users Groups
        /// </summary>
        /// <param name="IsAdmin"></param>
        /// <param name="CreatedById"></param>
        /// <returns></returns>
        public List<UserGroup> GetAllUserGroups(Boolean IsAdmin, Int32 CreatedById)
        {
            return SecurityManager.GetAllUserGroup(IsAdmin, CreatedById).ToList();
        }

        /// <summary>
        /// Update Users Group
        /// </summary>
        public void UpdateUserGroup()
        {
            UserGroup userGroup = GetAllUserGroups(View.ViewContract.IsAdmin,View.ViewContract.CreatedById).Where(condtion => condtion.UserGroupID == View.ViewContract.UserGoupId).FirstOrDefault();
            userGroup.UserGroupName = View.ViewContract.UserGoupName;
            userGroup.UserGroupDesc = View.ViewContract.UserGroupDescription;
            userGroup.TenantID = View.ViewContract.TenantId;
            userGroup.IsActive = true;
            userGroup.ModifiedOn = DateTime.Now;
            userGroup.ModifiedByID = View.CreatedById;
            SecurityManager.UpdateUserGroup(userGroup);
            View.SuccessMessage = SysXUtils.GetMessage(SysXSecurityConst.USERGROUPUPDATED);
        }

        /// <summary>
        /// Delete User Groups
        /// </summary>
        public void DeleteUserGroup()
        {
            UserGroup userGroup = GetAllUserGroups(View.ViewContract.IsAdmin, View.ViewContract.CreatedById).Where(condtion => condtion.UserGroupID == View.ViewContract.UserGoupId).FirstOrDefault();
            userGroup.IsActive = false;
            userGroup.ModifiedOn = DateTime.Now;
            userGroup.ModifiedByID = View.CreatedById;
            SecurityManager.UpdateUserGroup(userGroup);
            View.SuccessMessage = SysXUtils.GetMessage(SysXSecurityConst.USERGROUPDELETED);
        }

        /// <summary>
        /// Get All Users In User Group
        /// </summary>
        /// <returns></returns>
        public List<UsersInUserGroup> GetAllUsersInAgroup()
        {
            return SecurityManager.GetAllUsersInAGroup(View.ViewContract.UserGoupId).ToList();
        }

        /// <summary>
        /// Get all Asp net user
        /// </summary>
        /// <returns></returns>
        public List<OrganizationUser> GetAspnetUsers(Int32 productId)
        {
            return SecurityManager.GetUsersByProductId(productId);
        }

        /// <summary>
        /// Get all Asp net user
        /// </summary>
        /// <returns></returns>
        public List<OrganizationUser> GetAspnetUsers()
        {
            return SecurityManager.GetUsersByProductId(Convert.ToInt32(View.AssignToProductId));
        }

        public Int32? GetTenantProductId()
        {
            return SecurityManager.GetTenantProductId(View.ViewContract.TenantId);
        }

        /// <summary>
        /// Return all active tenants.
        /// </summary>
        /// <returns></returns>
        public List<Tenant> GetTenants()
        {
            Boolean SortByName = true;
            return SecurityManager.GetTenants(SortByName);
        }

        /// <summary>
        /// Add user in User Group
        /// </summary>
        public void AddUserInUserGroup()
        {
            UsersInUserGroup userInUserGroup = new UsersInUserGroup();
            userInUserGroup.UserGroupID = View.ViewContract.UserGoupId;
            userInUserGroup.IsActive = true;
            userInUserGroup.UserID = View.ViewContract.Aspnet_UserId;
            SecurityManager.InsertUserInGroup(userInUserGroup);
            View.SuccessMessage = SysXUtils.GetMessage(SysXSecurityConst.USERADDEDINUSERGROUP);
        }

        /// <summary>
        /// Remove user from User Group
        /// </summary>
        public void RemoveUserInUserGroup()
        {
            UsersInUserGroup userInUserGroup = SecurityManager.GetAllUsersInAGroup(View.ViewContract.UserGoupId).Where(condtion => condtion.UsersInUserGroupID == View.ViewContract.UsersInUserGroupID).FirstOrDefault();
            userInUserGroup.IsActive = false;
            SecurityManager.DeleteUserInGroup(userInUserGroup);
            View.SuccessMessage = SysXUtils.GetMessage(SysXSecurityConst.USERREMOVEDFROMUSERGROUP);
        }

        /// <summary>
        /// Get organization user details.
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        public OrganizationUser GetOrganizationUser(Int32 organizationUserId)
        {
            return SecurityManager.GetOrganizationUser(organizationUserId);
        }

        #endregion

        #endregion
    }
}