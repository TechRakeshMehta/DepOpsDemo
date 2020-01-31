#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;
using System.Data.Entity.Core.Objects;

#endregion

#region UserDefined

using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManageUserGroupsPresenter : Presenter<IManageUserGroupsView>
    {

        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            View.ListTenants = ComplianceDataManager.getClientTenant();
        }
        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            //Int32 currentUserID = GetTenantId();
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentUserId).Organization.TenantID.Value;
        }
        /// <summary>
        /// Get All User Group to bind the grid
        /// </summary>
        public void GetAllUserGroup(String selectedHierarchyIds)
        {
            //UAT-2284: User Group permisson/access and availability by node
            Int32? currentUserId = GetTenantId() == SecurityManager.DefaultTenantID ? (Int32?)null : View.CurrentUserId;
            View.ListUserGroup = ComplianceSetupManager.GetAllUserGroupWithPermissionAll(View.SelectedTenantID, currentUserId, selectedHierarchyIds).OrderBy(ex => ex.UG_Name).ToList();
        }

        /// <summary>
        /// Archive User Groups
        ///
        /// </summary>
        public String ArchiveUnArchiveUserGroups(bool isArchive)
        {
           return ComplianceSetupManager.ArchiveUnArchiveUserGroups(View.SelectedTenantID, View.ListSelectedUserGroupIds, isArchive);
        }
        /// <summary>
        /// Save the user Group 
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean SaveUserGroup()
        {
            if (IsUserGroupExist(View.UserGroupName, null))
            {
                UserGroup userGroup = new UserGroup();
                userGroup.UG_Name = View.UserGroupName;
                userGroup.UG_Description = View.UserGroupDescription;
                userGroup.UG_IsDeleted = false;
                userGroup.UG_CreatedByID = View.CurrentUserId;
                userGroup.UG_CreatedOn = DateTime.Now;

                List<Int32> nodesToBeMapped = new List<Int32>();
                if (!View.HierarchyNode.IsNullOrEmpty())
                {
                    nodesToBeMapped = View.HierarchyNode.Split(',').Select(int.Parse).ToList();
                    foreach (Int32 node in nodesToBeMapped)
                    {
                        UserGroupHierarchyMapping newUserGroupHierarchyMapping = new UserGroupHierarchyMapping();
                        newUserGroupHierarchyMapping.UGHM_HierarchyNodeID = node;
                        newUserGroupHierarchyMapping.UGHM_IsDeleted = false;
                        newUserGroupHierarchyMapping.UGHM_CreatedBy = View.CurrentUserId;
                        newUserGroupHierarchyMapping.UGHM_CreatedOn = DateTime.Now;
                        userGroup.UserGroupHierarchyMappings.Add(newUserGroupHierarchyMapping);
                    }
                }
                if (ComplianceSetupManager.SaveUserGroup(View.SelectedTenantID, userGroup))
                {
                    View.SuccessMessage = "User group saved successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occured.Please try again.";
                    return false;
                }
            }
            else
            {
                View.ErrorMessage = "User group already exist.";
                return false;
            }
        }
        /// <summary>
        /// Check the user group existance
        /// </summary>
        /// <param name="userGroupName"></param>
        /// <param name="userGroupId"></param>
        /// <returns>Boolean</returns>
        public Boolean IsUserGroupExist(String userGroupName, Int32? userGroupId = null)
        {
            View.ListUserGroup = ComplianceSetupManager.GetAllUserGroup(View.SelectedTenantID).ToList();
            if (userGroupId != null)
            {
                if (View.ListUserGroup.Any(x => x.UG_Name.ToLower() == userGroupName.ToLower() && x.UG_ID != userGroupId))
                {
                    return false;
                }
                return true;
            }
            else
            {
                if (View.ListUserGroup.Any(x => x.UG_Name.ToLower() == userGroupName.ToLower() && !x.UG_IsDeleted))
                {
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// Update the User Group
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean UpdateUserGroup()
        {
            if (IsUserGroupExist(View.UserGroupName, View.UserGroupId))
            {
                UserGroup userGroup = ComplianceSetupManager.GetUserGroupById(View.SelectedTenantID, View.UserGroupId);
                userGroup.UG_Name = View.UserGroupName;
                userGroup.UG_Description = View.UserGroupDescription;
                userGroup.UG_IsDeleted = false;
                userGroup.UG_ModifiedByID = View.CurrentUserId;
                userGroup.UG_ModifiedOn = DateTime.Now;

                List<UserGroupHierarchyMapping> existingUserHierarchyMappings = userGroup.UserGroupHierarchyMappings.Where(cond => !cond.UGHM_IsDeleted).ToList();
                List<Int32> nodesToBeMapped = new List<Int32>();

                if (!View.HierarchyNode.IsNullOrEmpty())
                    nodesToBeMapped = View.HierarchyNode.Split(',').Select(int.Parse).ToList();

                //Check whether existing node exist in current list if not exist then delete it
                foreach (UserGroupHierarchyMapping existingNode in existingUserHierarchyMappings)
                {
                    if (!(nodesToBeMapped.Contains(existingNode.UGHM_HierarchyNodeID)))
                    {
                        existingNode.UGHM_IsDeleted = true;
                        existingNode.UGHM_ModifiedBy =View.CurrentUserId;
                        existingNode.UGHM_ModifiedOn = DateTime.Now;
                    }
                }

                //Check whether selected node exist in db if not exist then insert it
                foreach (Int32 node in nodesToBeMapped)
                {
                    if (!(existingUserHierarchyMappings.Any(obj => obj.UGHM_HierarchyNodeID == node && obj.UGHM_IsDeleted == false)))
                    {
                        UserGroupHierarchyMapping newUserGroupHierarchyMapping = new UserGroupHierarchyMapping();
                        newUserGroupHierarchyMapping.UGHM_HierarchyNodeID = node;
                        newUserGroupHierarchyMapping.UGHM_IsDeleted = false;
                        newUserGroupHierarchyMapping.UGHM_CreatedBy = View.CurrentUserId;
                        newUserGroupHierarchyMapping.UGHM_CreatedOn = DateTime.Now;
                        userGroup.UserGroupHierarchyMappings.Add(newUserGroupHierarchyMapping);
                    }
                }
                if (ComplianceSetupManager.UpdateUserGroup(View.SelectedTenantID))
                {
                    View.SuccessMessage = "User group updated successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occured.Please try again.";
                    return false;
                }
            }
            else
            {
                View.ErrorMessage = "User group already exist.";
                return false;
            }
        }
        /// <summary>
        /// Delete the user group
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean DeleteUserGroup()
        {
            UserGroup userGroup = ComplianceSetupManager.GetUserGroupById(View.SelectedTenantID, View.UserGroupId);
            userGroup.UG_IsDeleted = true;
            userGroup.UG_ModifiedByID = View.CurrentUserId;
            userGroup.UG_ModifiedOn = DateTime.Now;
            userGroup.ApplicantUserGroupMappings.ForEach(x => x.AUGM_IsDeleted = true);

            if (ComplianceSetupManager.UpdateUserGroup(View.SelectedTenantID))
            {
                View.SuccessMessage = "User group deleted successfully.";
                return true;
            }
            else
            {
                View.ErrorMessage = "Some error occured.Please try again.";
                return false;
            }
        }
    }
}




