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
    public class ManageInstitutionNodeTypePresenter : Presenter<IManageInstitutionNodeTypeView>
    {

        public override void OnViewLoaded()
        {

            View.ListTenants = ComplianceDataManager.getClientTenant();
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
        public void GetInstitutionNodeType()
        {
            View.ListInstitutionNodeType = ComplianceSetupManager.GetAllInstitutionNodeType(View.SelectedTenantID);
        }
        public Boolean SaveInsitutionNodeType()
        {
            if (IsInstitutionNodeTypetExist(View.InstitutionNodeTypeName, null))
            {
                InstitutionNodeType institutionNodeType = new InstitutionNodeType();

                institutionNodeType.INT_Name = View.InstitutionNodeTypeName;
                institutionNodeType.INT_Description = View.InstitutionNodeTypeDescription;
                institutionNodeType.INT_IsDeleted = false;
                institutionNodeType.INT_CreatedByID = View.CurrentUserId;
                institutionNodeType.INT_CreatedOn = DateTime.Now;
                GetLastInstitutionNodeTypeCode();
                institutionNodeType.INT_Code = runningCode(View.LastCode);
                if (ComplianceSetupManager.SaveInstitutionNodeType(View.SelectedTenantID, institutionNodeType))
                {
                    View.SuccessMessage = "Institution node type saved successfully.";
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
                View.ErrorMessage = "Institution node type already exist.";
                return false;
            }
        }

        public Boolean IsInstitutionNodeTypetExist(String institutionNodeName, Int32? institutionNodeTypeId = null)
        {
            View.ListInstitutionNodeType = ComplianceSetupManager.GetAllInstitutionNodeType(View.SelectedTenantID);
            if (institutionNodeTypeId != null)
            {
                if (View.ListInstitutionNodeType.Any(x => x.INT_Name.ToLower() == institutionNodeName.ToLower() && x.INT_ID != institutionNodeTypeId))
                {
                    return false;
                }
                return true;
            }
            else
            {
                if (View.ListInstitutionNodeType.Any(x => x.INT_Name.ToLower() == institutionNodeName.ToLower() && !x.INT_IsDeleted))
                {
                    return false;
                }
                return true;
            }
        }
        public Boolean UpdateInstitutionNodeType()
        {
            if (IsInstitutionNodeTypetExist(View.InstitutionNodeTypeName, View.InstitutionNodeTypeId))
            {
                InstitutionNodeType institutionNodeType = ComplianceSetupManager.GetInstitutionNodeTypeById(View.SelectedTenantID, View.InstitutionNodeTypeId);
                institutionNodeType.INT_Name = View.InstitutionNodeTypeName;
                institutionNodeType.INT_Description = View.InstitutionNodeTypeDescription;
                institutionNodeType.INT_IsDeleted = false;
                institutionNodeType.INT_ModifiedByID = View.CurrentUserId;
                institutionNodeType.INT_ModifiedOn = DateTime.Now;
                if (ComplianceSetupManager.UpdateInstitutionNodeType(View.SelectedTenantID))
                {
                    View.SuccessMessage = "Institution node type updated successfully.";
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
                View.ErrorMessage = "Institution node type already exist.";
                return false;
            }
        }
        public Boolean DeleteInstitutionNodeType()
        {
            if (!ComplianceSetupManager.IsInstitutionNodeTypeMapped(View.SelectedTenantID, View.InstitutionNodeTypeId))
            {
                InstitutionNodeType institutionNodeType = ComplianceSetupManager.GetInstitutionNodeTypeById(View.SelectedTenantID, View.InstitutionNodeTypeId);
                institutionNodeType.INT_IsDeleted = true;
                institutionNodeType.INT_ModifiedByID = View.CurrentUserId;
                institutionNodeType.INT_ModifiedOn = DateTime.Now;

                if (ComplianceSetupManager.UpdateInstitutionNodeType(View.SelectedTenantID))
                {
                    View.SuccessMessage = "Institution node type deleted successfully.";
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
                View.ErrorMessage = "This institution node type is in use.";
                return false;
            }
        }
        public void GetLastInstitutionNodeTypeCode()
        {
            View.LastCode = ComplianceSetupManager.GetLastInstitutionNodeTypeCode(View.SelectedTenantID);
        }
        public String runningCode(String inputString)
        {
            char[] charArray = inputString.ToCharArray();
            Int32 i = charArray.Length - 1;
            while (i != -1)
            {
                Int32 asciiCode = charArray[i];
                if (asciiCode < 90)
                {
                    charArray[i] = (char)(asciiCode + 1);
                    break;
                }
                else
                {
                    charArray[i] = (char)65;
                    i--;
                }
            }
            String outputString = new String(charArray);
            return outputString;
        }

    }
}




