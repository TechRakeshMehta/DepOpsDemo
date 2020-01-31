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
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;
using System.Xml;

#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public class ManagePackageTypePresenter : Presenter<IManagePackageTypeView>
    {

        #region Properties

        #region Private Properties



        #endregion

        #region Public Properties



        #endregion

        #endregion

        #region Methods

        #region Private Methods

        #endregion
        #endregion

        #region Public Methods

        /// <summary>
        /// Method called when SetUp page view is loaded.
        /// </summary>
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads            
        }
        #endregion
        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }
        /// <summary>
        /// Method called when View is initialized.
        /// </summary>
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetTenants()
        {
            View.ListTenants = ComplianceDataManager.GetTenantList(View.DefaultTenantId);
        }

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public List<BkgPackageType> GetAllBkgPackageTypes(Int32 bkgpkgTypeId, String bkgPackageColorCode)
        {
            List<BkgPackageType> lstBkgType;
            if (View.SelectedTenantId > AppConsts.NONE)
            {
                // View.lstBkgPackageType
                lstBkgType = BackgroundSetupManager.GetAllBkgPackageTypes(View.SelectedTenantId, View.PackageTypeName, View.PackageTypeCode, bkgpkgTypeId, bkgPackageColorCode).ToList();
            }
            else
            {
                lstBkgType = new List<BkgPackageType>();
            }

            if (lstBkgType.Count == AppConsts.NONE)
            {
                View.VirtualRecordCount = AppConsts.NONE;
            }
            else
            {
                View.VirtualRecordCount = lstBkgType.Count;
            }

            View.lstBkgPackageType = lstBkgType;
            return View.lstBkgPackageType;
        }
        /// <summary>
        /// Check the user Attribute existance
        /// </summary>
        /// <param name="userGroupName"></param>
        /// <param name="userGroupId"></param>
        /// <returns>Boolean</returns>
        public Boolean IsPackageTypeCodeExist(Int32 bkgPackageTypeId)
        {
            GetAllBkgPackageTypes(bkgPackageTypeId, String.Empty);
            if (View.lstBkgPackageType.Count > 0)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Save the Attribute Group 
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean SavePackageType()
        {
            String status = String.Empty;
            Boolean IsPackageCodeExists = BackgroundSetupManager.IsPackageTypeCodeAlreadyExists(View.SelectTenantIdForAddForm, View.PackageTypeCode, AppConsts.NONE);
            Boolean IsPackageNameExists = BackgroundSetupManager.IsPackageTypeNameAlreadyExists(View.SelectTenantIdForAddForm, View.PackageTypeName, AppConsts.NONE);
            if (!IsPackageCodeExists && !IsPackageNameExists)
            {
                BkgPackageTypeContract _packageTypeContract = new BkgPackageTypeContract();
                _packageTypeContract.BkgPackageTypeName = View.PackageTypeName;
                _packageTypeContract.BkgPackageTypeCode = View.PackageTypeCode.Trim();
                _packageTypeContract.BkgPackageTypeColorCode = View.PackageTypeColorCode;
                BackgroundSetupManager.SaveUpdatePackageType(View.SelectTenantIdForAddForm, _packageTypeContract, View.CurrentLoggedInUserId);
                status = AppConsts.BPT_SAVED_SUCCESS_MSG;
            }
            else if (IsPackageCodeExists)
            {
                status = "Package type code already exists.";
            }
            else if (IsPackageNameExists)
            {
                status = "Package type name already exists.";
            }
            if (status == AppConsts.BPT_SAVED_SUCCESS_MSG)
            {
                View.SuccessMessage = status;
            }
            else
            {
                View.ErrorMessage = status == String.Empty ? AppConsts.BPT_SAVED_ERROR_MSG : status; ;
            }
            return true;
        }
        /// <summary>
        /// Delete the Package Type
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean DeletePackageType()
        {

            Boolean IsPackageTypeMapped = false;

            IsPackageTypeMapped = BackgroundSetupManager.IsPackageMapped(View.SelectedTenantId, View.BkgPackageTypeId, View.CurrentLoggedInUserId);

            if (!IsPackageTypeMapped)
            {
                String status = BackgroundSetupManager.DeletePackageType(View.SelectedTenantId, View.BkgPackageTypeId, View.CurrentLoggedInUserId);
                if (status == AppConsts.BPT_DELETED_SUCCESS_MSG)
                {
                    View.SuccessMessage = status;
                }
                else
                {
                    View.ErrorMessage = status;
                }
            }
            else
            {
                View.ErrorMessage = "Package type is in use.";
            }
            return true;
        }
        /// <summary>
        /// Update the Attribute Group
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean UpdatePackageType()
        {
            String status = String.Empty;
            Boolean IsUpdated = false;
            Boolean IsPackageCodeExists = BackgroundSetupManager.IsPackageTypeCodeAlreadyExists(View.SelectTenantIdForAddForm, View.PackageTypeCode, View.BkgPackageTypeId);
            Boolean IsPackageNameExists = BackgroundSetupManager.IsPackageTypeNameAlreadyExists(View.SelectTenantIdForAddForm, View.PackageTypeName, View.BkgPackageTypeId);

            if (!IsPackageCodeExists && !IsPackageNameExists)
            {
                BkgPackageTypeContract _packageTypeContract = new BkgPackageTypeContract();
                _packageTypeContract.BkgPackageTypeId = View.BkgPackageTypeId;
                _packageTypeContract.BkgPackageTypeName = View.PackageTypeName;
                _packageTypeContract.BkgPackageTypeCode = View.PackageTypeCode;
                _packageTypeContract.BkgPackageTypeColorCode = View.PackageTypeColorCode;
                IsUpdated = BackgroundSetupManager.SaveUpdatePackageType(View.SelectTenantIdForAddForm, _packageTypeContract, View.CurrentLoggedInUserId);
                status = AppConsts.BPT_UPDATED_SUCCESS_MSG;                               
            }
            else if (IsPackageCodeExists)
            {
                status = "Package type code already exists.";
            }
            else if (IsPackageNameExists)
            {
                status = "Package type name already exists.";
            }
            if (status == AppConsts.BPT_UPDATED_SUCCESS_MSG)
            {
                View.SuccessMessage = status;               
            }
            else
            {
                View.ErrorMessage = status == String.Empty ? AppConsts.BPT_UPDATED_ERROR_MSG : status;
                return false;
            }
            return true;
        }

        public void GetPackageTypeCode()
        {
            View.LastCode = BackgroundSetupManager.GetPackageTypeCode(View.SelectedTenantId);
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
