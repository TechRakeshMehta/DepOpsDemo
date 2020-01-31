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

namespace CoreWeb.ComplianceAdministration.Views
{
    public class ManageAttributeGroupPresenter : Presenter<IManageAttributeGroupView>
    {

        public override void OnViewLoaded()
        {
            View.ListTenants = ComplianceDataManager.GetMasterAndInstitutionTypeTenants(View.DefaultTenantId);
            //View.TenantId = GetTenantId();
        }

        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
           
        }

       /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
         //   Int32 currentUserID = GetTenantId();
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
        public void GetAllComplianceAttributeGroup()
        {
            View.ListComplianceAttributeGroup = ComplianceSetupManager.GetAllComplianceAttributeGroup(View.SelectedTenantID);
        }
        /// <summary>
        /// Save the Attribute Group 
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean SaveAttributeGroup()
        {
            if (IsAttributeGroupExist(View.Name, null))
            {                
                ComplianceAttributeGroup attributeGroup = new ComplianceAttributeGroup();
                attributeGroup.CAG_Name = View.Name;
                attributeGroup.CAG_Label = View.Label;
                attributeGroup.CAG_IsDeleted = false;
                attributeGroup.CAG_CreatedByID = View.CurrentUserId;
                attributeGroup.CAG_CreatedOn = DateTime.Now;
                attributeGroup.CAG_TenantID = View.SelectedTenantID;
                attributeGroup.CAG_Code = Guid.NewGuid();
                attributeGroup.CAG_IsCreatedByAdmin = View.TenantId == SecurityManager.DefaultTenantID ? true : false;
                if (ComplianceSetupManager.SaveAttributeGroup(View.SelectedTenantID, attributeGroup))
                {
                    View.SuccessMessage = "Attribute group saved successfully.";
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
                View.ErrorMessage = "Attribute group already exist.";
                return false;
            }
        }
        /// <summary>
        /// Check the user Attribute existance
        /// </summary>
        /// <param name="userGroupName"></param>
        /// <param name="userGroupId"></param>
        /// <returns>Boolean</returns>
        public Boolean IsAttributeGroupExist(String userGroupName, Int32? userGroupId = null)
        {
            View.ListComplianceAttributeGroup = ComplianceSetupManager.GetAllComplianceAttributeGroup(View.SelectedTenantID);
            if (userGroupId != null)
            {
                if (View.ListComplianceAttributeGroup.Any(x => x.CAG_Name.ToLower() == userGroupName.ToLower() && x.CAG_ID != userGroupId))
                {
                    return false;
                }
                return true;
            }
            else
            {
                if (View.ListComplianceAttributeGroup.Any(x => x.CAG_Name.ToLower() == userGroupName.ToLower() && !x.CAG_IsDeleted))
                {
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// Update the Attribute Group
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean UpdateAttributeGroup()
        {
            if (IsAttributeGroupExist(View.Name, View.ComplianceAttributeGroupId))
            {
                ComplianceAttributeGroup attributeGroup = ComplianceSetupManager.GetAttributeGroupById(View.SelectedTenantID, View.ComplianceAttributeGroupId);
                attributeGroup.CAG_Name = View.Name;
                attributeGroup.CAG_Label = View.Label;
                attributeGroup.CAG_IsDeleted = false;
                attributeGroup.CAG_ModifiedByID = View.CurrentUserId;
                attributeGroup.CAG_ModifiedOn = DateTime.Now;

                if (ComplianceSetupManager.UpdateAttributeGroup(View.SelectedTenantID))
                {
                    View.SuccessMessage = "Attribute group updated successfully.";
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
                View.ErrorMessage = "Attribute group already exist.";
                return false;
            }
        }
        /// <summary>
        /// Delete the Attribute Group
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean DeleteAttributeGroup()
        {
            if (!ComplianceSetupManager.IsAttributeGroupMapped(View.SelectedTenantID, View.ComplianceAttributeGroupId))
            {
                ComplianceAttributeGroup attributeGroup = ComplianceSetupManager.GetAttributeGroupById(View.SelectedTenantID, View.ComplianceAttributeGroupId);
                attributeGroup.CAG_IsDeleted = true;
                attributeGroup.CAG_ModifiedByID = View.CurrentUserId;
                attributeGroup.CAG_ModifiedOn = DateTime.Now;                
                if (ComplianceSetupManager.UpdateUserGroup(View.SelectedTenantID))
                {
                    View.SuccessMessage = "Attribute group deleted successfully.";
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
                View.ErrorMessage = "This Attribute group is in use.";
                return false;
            }
        }
    }
}




