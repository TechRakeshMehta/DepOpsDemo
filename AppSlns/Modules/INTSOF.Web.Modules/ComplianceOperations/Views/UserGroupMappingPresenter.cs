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
    public class UserGroupMappingPresenter : Presenter<IUserGroupMappingView>
    {

        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {

        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
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
        /// Get All User Group to bind the grid
        /// </summary>
        public void GetAllUserGroup()
        {
            //UAT-2284: User Group permisson/access and availability by node
            Int32? currentUserId = IsAdminLoggedIn() ? (Int32?)null : View.CurrentLoggedInUserId;
            if (View.ScreenMode.ToLower() == "assign")
                View.ListUserGroup = ComplianceSetupManager.GetAllUserGroupWithPermission(View.SelectedTenantID, currentUserId).OrderBy(ex => ex.UG_Name).ToList();
            else
                View.ListUserGroup = ComplianceDataManager.GetUserGroupsByOrgUserIDs(View.SelectedTenantID, View.ApplicantUserIds);
        }

        /// <summary>
        /// Save the user Group 
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean SaveUserGroup()
        {
            if (IsUserGroupExist(View.UserGroupName))
            {
                UserGroup userGroup = new UserGroup();
                userGroup.UG_Name = View.UserGroupName;
                userGroup.UG_Description = View.UserGroupDescription;
                userGroup.UG_IsDeleted = false;
                userGroup.UG_CreatedByID = View.CurrentLoggedInUserId;
                userGroup.UG_CreatedOn = DateTime.Now;
                //UAT-3381
                List<Int32> nodesToBeMapped = new List<Int32>();
                if (!View.HierarchyNode.IsNullOrEmpty())
                {
                    nodesToBeMapped = View.HierarchyNode.Split(',').Select(int.Parse).ToList();
                    foreach (Int32 node in nodesToBeMapped)
                    {
                        UserGroupHierarchyMapping newUserGroupHierarchyMapping = new UserGroupHierarchyMapping();
                        newUserGroupHierarchyMapping.UGHM_HierarchyNodeID = node;
                        newUserGroupHierarchyMapping.UGHM_IsDeleted = false;
                        newUserGroupHierarchyMapping.UGHM_CreatedBy = View.CurrentLoggedInUserId;
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
                    View.ErrorMessage = "Some error occured. Please try again.";
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
            var lstUserGroup = ComplianceSetupManager.GetAllUserGroup(View.SelectedTenantID);
            if (userGroupId != null)
            {
                if (lstUserGroup.Any(x => x.UG_Name.ToLower() == userGroupName.ToLower() && x.UG_ID != userGroupId))
                {
                    return false;
                }
                return true;
            }
            else
            {
                if (lstUserGroup.Any(x => x.UG_Name.ToLower() == userGroupName.ToLower() && !x.UG_IsDeleted))
                {
                    return false;
                }
                return true;
            }
        }

        public Boolean AssignUserGroups()
        {
            return ComplianceDataManager.AssignUserGroupsToUsers(View.SelectedTenantID, View.AssignUserGroupIds, View.ApplicantUserIds, View.CurrentLoggedInUserId);
        }

        public Boolean UnassignUserGroups()
        {
            return ComplianceDataManager.UnassignUserGroups(View.SelectedTenantID, View.AssignUserGroupIds, View.ApplicantUserIds, View.CurrentLoggedInUserId);
        }
    }
}




