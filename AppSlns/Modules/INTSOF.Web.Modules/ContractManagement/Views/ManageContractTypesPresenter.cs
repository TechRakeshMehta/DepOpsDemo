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

namespace CoreWeb.ContractManagement.Views
{
    public class ManageContractTypesPresenter : Presenter<IManageContractTypesView>
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
            //Checked if logged user is admin or not.
            if (View.TenantId == SecurityManager.DefaultTenantID)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Get Contract Types
        /// </summary>
        public void GetContractTypes()
        {
            View.ListContractTypes = ContractManager.GetContractTypes(View.SelectedTenantID);
        }

        /// <summary>
        /// Save Contract Types
        /// </summary>
        /// <returns></returns>
        public Boolean SaveContractTypes()
        {
            if (IsContractTypeExist(View.ContractTypeName, null))
            {
                ContractType contractType = new ContractType();

                contractType.CT_Name = View.ContractTypeName;
                contractType.CT_Label = View.ContractTypeLabel;
                contractType.CT_Description = View.ContractTypeDescription;
                contractType.CT_IsDeleted = false;
                contractType.CT_CreatedByID = View.CurrentLoggedInUserId;
                contractType.CT_CreatedOn = DateTime.Now;
                GetLastContractTypeCode();
                if (View.LastCode.IsNullOrEmpty())
                    contractType.CT_Code = "AAAA";
                else
                    contractType.CT_Code = GetRunningCode(View.LastCode);
                if (ContractManager.SaveContractTypes(View.SelectedTenantID, contractType))
                {
                    View.SuccessMessage = "Contract Type saved successfully.";
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
                View.ErrorMessage = "Contract Type already exist.";
                return false;
            }
        }

        /// <summary>
        /// Is Contract Type Exist
        /// </summary>
        /// <param name="contractTypeName"></param>
        /// <param name="ContractTypeId"></param>
        /// <returns></returns>
        public Boolean IsContractTypeExist(String contractTypeName, Int32? ContractTypeId = null)
        {
            View.ListContractTypes = ContractManager.GetContractTypes(View.SelectedTenantID);
            if (ContractTypeId != null)
            {
                if (View.ListContractTypes.Any(x => x.CT_Name.ToLower() == contractTypeName.ToLower() && x.CT_ID != ContractTypeId))
                {
                    return false;
                }
                return true;
            }
            else
            {
                if (View.ListContractTypes.Any(x => x.CT_Name.ToLower() == contractTypeName.ToLower() && !x.CT_IsDeleted))
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Update Contract Types
        /// </summary>
        /// <returns></returns>
        public Boolean UpdateContractTypes()
        {
            if (IsContractTypeExist(View.ContractTypeName, View.ContractTypeId))
            {
                ContractType contractType = ContractManager.GetContractTypeById(View.SelectedTenantID, View.ContractTypeId);
                contractType.CT_Name = View.ContractTypeName;
                contractType.CT_Label = View.ContractTypeLabel;
                contractType.CT_Description = View.ContractTypeDescription;
                contractType.CT_IsDeleted = false;
                contractType.CT_ModifiedByID = View.CurrentLoggedInUserId;
                contractType.CT_ModifiedOn = DateTime.Now;
                if (ContractManager.UpdateContractTypes(View.SelectedTenantID))
                {
                    View.SuccessMessage = "Contract Type updated successfully.";
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
                View.ErrorMessage = "Contract Type already exist.";
                return false;
            }
        }

        /// <summary>
        /// Delete Contract Types
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteContractTypes()
        {
            if (!ContractManager.IsContractTypeMapped(View.SelectedTenantID, View.ContractTypeId))
            {
                ContractType contractType = ContractManager.GetContractTypeById(View.SelectedTenantID, View.ContractTypeId);
                contractType.CT_IsDeleted = true;
                contractType.CT_ModifiedByID = View.CurrentLoggedInUserId;
                contractType.CT_ModifiedOn = DateTime.Now;

                if (ContractManager.UpdateContractTypes(View.SelectedTenantID))
                {
                    View.SuccessMessage = "Contract Type deleted successfully.";
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
                ContractType contractType = ContractManager.GetContractTypeById(View.SelectedTenantID, View.ContractTypeId);
                View.ErrorMessage = String.Format("You cannot delete contract {0} as it is in use.",contractType.CT_Name);
                return false;
            }
        }

        /// <summary>
        /// Get last/previous Contract Types Code
        /// </summary>
        public void GetLastContractTypeCode()
        {
            View.LastCode = ContractManager.GetLastContractTypeCode(View.SelectedTenantID);
        }

        /// <summary>
        /// Get running code
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public String GetRunningCode(String inputString)
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
