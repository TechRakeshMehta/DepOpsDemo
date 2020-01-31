#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  MapUserRolePresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using INTSOF.SharedObjects;

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
    /// This class has the method's implementation related to mapping of User's with roles.
    /// </summary>
    public class MapUserRolePresenter : Presenter<IMapUserRoleView>
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
            if (!View.ViewContract.UserId.IsNull())
            {
               // View.SelectedUser = SecurityManager.GetOrganizationUserInfoByUserId(View.ViewContract.UserId).FirstOrDefault();
                View.SelectedUser = SecurityManager.GetOrganizationUser(View.ViewContract.OrganizationUserId);
            }

        }

        /// <summary>
        /// Manages the mapping between User and its roles.
        /// </summary>
        public void MappingUserRoles(Boolean IsNoUserGroup = true, Boolean IsUserOfUserGroup = false)
        {
            if (IsNoUserGroup)
            {
                var userRoleMapping = SecurityManager.GetAspnetUser(Convert.ToString(View.SelectedUser.UserID)).aspnet_Roles.Where(condition => condition.RoleDetail.IsUserGroupLevel == false);

                // Existing Role's Ids.
                List<String> existingRolesId = userRoleMapping.Select(condition => condition.RoleId.ToString()).ToList();

                // Existing Role's Name.
                String[] existingRolesName = userRoleMapping.Select(condition => condition.RoleName).ToArray();
                View.ViewContract.NewMappedRoles = existingRolesId.Except(View.SelectedItems.Where(condition => condition.Value.Value == false).Select(ss => ss.Key.ToString())).ToList();
                View.ViewContract.NewMappedRoles.AddRange(View.SelectedItems.Where(condition => condition.Value.Value).Select(ss => ss.Key.ToString()).ToList());

                List<String> allAssignedRoleName = existingRolesName.Select(ss => ss.Split(new char[] { '_' }).FirstOrDefault()).Except(View.SelectedItems.Where(condition => condition.Value.Value == false).Select(ss => ss.Value.Key)).ToList();
                allAssignedRoleName.AddRange(View.SelectedItems.Where(condition => condition.Value.Value).Select(ss => ss.Value.Key.ToString()).ToList());
                View.AllAssignedRoleName = allAssignedRoleName.ToArray();

                SecurityManager.SaveMappingOfRolesWithSelectedUser(View.SelectedUser, View.ViewContract.NewMappedRoles, View.AllAssignedRoleName, View.ViewContract.DefaultPassword);

                //UAT-3228
                SecurityManager.InsertDefaultColumnConfiguration(View.CurrentUserId, View.SelectedUser, View.ViewContract.NewMappedRoles);
            }
            else
            {
                if (IsUserOfUserGroup)
                {
                    List<String> existingRolesId = SecurityManager.GetAllRoleOfUserGroupUser(View.ViewContract.UserId).Select(condition => condition.RoleId.ToString()).ToList();
                    View.ViewContract.NewMappedRoles = existingRolesId.Except(View.SelectedItems.Where(condition => condition.Value.Value == false).Select(ss => ss.Key.ToString())).ToList();
                    View.ViewContract.NewMappedRoles.AddRange(View.SelectedItems.Where(condtion => condtion.Value.Value == true).Select(ss => ss.Key.ToString()).ToList());
                    SecurityManager.SaveRoleOfUserGroupUser(View.ViewContract.UserGroupId, View.ViewContract.UserId, View.ViewContract.NewMappedRoles);

                }
                else
                {
                    UserGroup usergroup = SecurityManager.GetAllUserGroup(View.IsAdmin, View.CurrentUserId).Where(condtion => condtion.UserGroupID == View.ViewContract.UserGroupId).FirstOrDefault();
                    List<String> userGroupRoleMapping = usergroup.RolesInUserGroups.Select(condtion => condtion.RoleID.ToString()).ToList();
                    var aspnetRoles = usergroup.RolesInUserGroups.Select(condtion => condtion.aspnet_Roles);
                    String[] existringRolesName = aspnetRoles.Select(condtion => condtion.RoleName).ToArray();
                    View.ViewContract.NewMappedRoles = userGroupRoleMapping.Except(View.SelectedItems.Where(condtion => condtion.Value.Value == false).Select(ss => ss.Key.ToString())).ToList();
                    View.ViewContract.NewMappedRoles.AddRange(View.SelectedItems.Where(condition => condition.Value.Value).Select(ss => ss.Key.ToString()).ToList());

                    List<String> allAssignedRoleName = existringRolesName.Select(ss => ss.Split(new char[] { '_' }).FirstOrDefault()).Except(View.SelectedItems.Where(condition => condition.Value.Value == false).Select(ss => ss.Value.Key)).ToList();
                    allAssignedRoleName.AddRange(View.SelectedItems.Where(condition => condition.Value.Value).Select(ss => ss.Value.Key.ToString()).ToList());
                    SecurityManager.UserGroupRoleMapping(View.ViewContract.UserGroupId, View.ViewContract.NewMappedRoles);
                }

            }
            View.RedirectToManageUser();
        }

        /// <summary>
        /// Retrieves a list of all the roles with its details.
        /// </summary>
        public void RetrievingRoles(Boolean IsNoUserGroup = true, Boolean IsUserOfUserGroup = false)
        {
             var currentUserDetails = SecurityManager.GetOrganizationUser(View.CurrentUserId);
            // OrganizationUser selectedUserDetails = SecurityManager.GetOrganizationUserInfoByUserId(View.ViewContract.UserId).FirstOrDefault();
             OrganizationUser selectedUserDetails = SecurityManager.GetOrganizationUser(View.ViewContract.OrganizationUserId);
            if (IsNoUserGroup)
            {
                View.CurrentUserRoles = SecurityManager.GetUserRolesById(View.ViewContract.UserId); //This stores all the roles associated with current user.
                // This stores all the roles associated with the product of given user's id.                
               
                var currentUserOrganizationDetails = SecurityManager.GetOrganization(currentUserDetails.OrganizationID);
                View.AllRoles = SecurityManager.GetRolesByUserId(View.ViewContract.UserId, false, selectedUserDetails.OrganizationID).ToList();
                if (View.IsAdmin) // It handles the case for Super Admin
                {
                    View.AllRolesOfProduct = SecurityManager.GetRolesByUserId(View.ViewContract.UserId, false, selectedUserDetails.OrganizationID).ToList();
                }
                else if (currentUserOrganizationDetails.ParentOrganizationID == null && !View.IsAdmin) // It handles the case for Product Admin
                {
                    //Removed condition && condition.RoleDetail.CreatedByID != SecurityManager.GetOrganizationUser(View.CurrentUserId).CreatedByID
                    //to display all roles related to that client admin
                    View.AllRolesOfProduct = View.AllRoles.Where(condition => !condition.RoleDetail.IsUserGroupLevel).ToList();
                }
                else // It handles the case for Normal User(Department Admin)
                {
                    View.AllRolesOfProduct = View.AllRoles.Where(condition => !condition.RoleDetail.IsUserGroupLevel && condition.RoleDetail.CreatedByID == View.CurrentUserId).ToList();
                }

               

                if (selectedUserDetails.IsNull())
                {
                    return;
                }

                // Returns the CreatedByID of user who has created the department under which current user is.
                if (!selectedUserDetails.IsNull())
                {
                    View.ViewContract.CreatedByUserId = Convert.ToInt32(selectedUserDetails.Organization.CreatedByID);
                }
            }
            else
            {
                if (IsUserOfUserGroup)
                {
                    View.CurrentUserRoles = SecurityManager.GetAllRoleOfUserGroupUser(View.ViewContract.UserId); 
                    // This stores all the roles associated with the product of given user's id.
                    View.AllRoles = SecurityManager.GetRolesByUserId(View.ViewContract.UserId, false, selectedUserDetails.OrganizationID).ToList();
                    View.AllRolesOfProduct = SecurityManager.GetAllRoleOfUserGroup(View.ViewContract.UserGroupId);
                }
                else
                {
                    View.CurrentUserRoles = SecurityManager.GetAllRoleOfUserGroup(View.ViewContract.UserGroupId);
                    // This stores all the roles associated with the product of given user's id.
                    View.AllRoles = SecurityManager.GetRolesByUserId(View.ViewContract.UserId, false, selectedUserDetails.OrganizationID).Where(condition => condition.RoleDetail.IsUserGroupLevel == true).ToList();
                    View.AllRolesOfProduct = SecurityManager.GetRolesByUserId(View.ViewContract.UserGroupId).Where(condition => condition.RoleDetail.IsUserGroupLevel == true).ToList();
                }

            }
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}