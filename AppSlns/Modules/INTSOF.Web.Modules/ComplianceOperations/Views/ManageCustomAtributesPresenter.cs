using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System.Linq;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManageCustomAttributesPresenter : Presenter<IManageCustomAttributesView>
    {

        public override void OnViewLoaded()
        {
            View.lstTenant = ComplianceDataManager.getClientTenant();
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            Int32 currentUserTenantId = GetTenantId();
            //Checked if logged user is admin or not.
            if (currentUserTenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                View.SelectedTenantID = currentUserTenantId;
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

        public void GetCustomAttDataTypelist()
        {
            View.lstCustomAttDataType = ComplianceDataManager.GetCustomAttrDataType(View.SelectedTenantID);
        }

        public void GetCustomAttrUseTypeList()
        {
            View.lstCustomAttUseType = ComplianceDataManager.GetCustomAttrUseType(View.SelectedTenantID);
        }

        public void GetCustomAttributesList()
        {
            View.lstCustomAttributes = ComplianceDataManager.GetCustomAttributes(View.SelectedTenantID);
        }

        public Boolean AddCustomAttribute(CustomAttributeContract customAttributeContract)
        {
            if (IsAttributeExist(customAttributeContract.CA_AttributeName, customAttributeContract.CA_CustomAttributeDataTypeID, customAttributeContract.CA_CustomAttributeUseTypeID, null))
            {
                View.InfoMessage = "Custom Attribute already exist.";
                return false;                
            }
            else
            {
                CustomAttribute customAttribute = customAttributeContract.TranslateToEntity();
                if (ComplianceDataManager.AddCustomAttribute(View.SelectedTenantID, customAttribute))
                {
                    View.SuccessMessage = "Custom attribute saved successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occurred.Please try again.";
                    return false;
                }
            }
        }

        public Boolean UpdateCustomAttribute(CustomAttributeContract customAttributeContract)
        {
            if (IsAttributeExist(customAttributeContract.CA_AttributeName, customAttributeContract.CA_CustomAttributeDataTypeID, customAttributeContract.CA_CustomAttributeUseTypeID, customAttributeContract.CA_CustomAttributeID))
            {
                View.InfoMessage = "Custom Attribute already exist.";
                return false;                
            }
            else
            {
                CustomAttribute customAttribute = customAttributeContract.TranslateToEntity();
                if (ComplianceDataManager.UpdateCustomAttribute(View.SelectedTenantID, customAttribute))
                {
                    View.SuccessMessage = "Custom attribute updated successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occurred.Please try again.";
                    return false;
                }
            }
        }

        public Boolean DeleteCustomAttribute(Int32 customAttributeId)
        {
            if (IsAttributeMapped(customAttributeId))
            {
                View.InfoMessage = "Custom Attribute cannot be deleted as it is associated with other objects.";
                return false;
            }
            else
            {
                if (ComplianceDataManager.DeleteCustomAttribute(View.SelectedTenantID, customAttributeId, View.CurrentUserId))
                {
                    View.SuccessMessage = "Custom attribute deleted successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occurred.Please try again.";
                    return false;
                }
            }
        }

        private Boolean IsAttributeExist(String attributeName, Int32 dataTypeId, Int32 useTypeId, Int32? id = null)
        {
            IQueryable<CustomAttribute> listCustomAttribute = ComplianceDataManager.GetCustomAttributes(View.SelectedTenantID);
            if (id != null)
            {
                //if (listCustomAttribute.Any(x => x.CA_CustomAttributeID != id && x.CA_AttributeName.ToLower() == attributeName.ToLower() && x.CA_CustomAttributeDataTypeID == dataTypeId && x.CA_CustomAttributeUseTypeID == useTypeId))
                if (listCustomAttribute.Any(x => x.CA_CustomAttributeID != id && x.CA_AttributeName.ToLower() == attributeName.ToLower() && x.CA_CustomAttributeUseTypeID == useTypeId))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (listCustomAttribute.Any(x => x.CA_AttributeName.ToLower() == attributeName.ToLower() && x.CA_CustomAttributeUseTypeID == useTypeId))
                {
                    return true;
                }
                return false;
            }
        }

        private Boolean IsAttributeMapped(Int32 customAttributeId)
        {
            String UseTypeCode = CustomAttributeUseTypeContext.Hierarchy.GetStringValue();
            return ComplianceDataManager.IsAttributeMapped(View.SelectedTenantID, customAttributeId, UseTypeCode);
        }

        public void GetProfileCustomAttributesList(Int32 selectedDataTypeId)
        {
            View.lstProfileCustomAttributes = ComplianceDataManager.GetProfileCustomAttributesByTenantID(View.SelectedTenantID, selectedDataTypeId);
        }
    }
}




