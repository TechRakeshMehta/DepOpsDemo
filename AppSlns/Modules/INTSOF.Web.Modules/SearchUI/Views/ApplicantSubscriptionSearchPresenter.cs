using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.Search.Views
{
    public class ApplicantSubscriptionSearchPresenter : Presenter<IApplicantSubscriptionSearchView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetTenants();
            GetGranularPermissionForDOBandSSN();
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        /// <summary>
        /// To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        /// <summary>
        /// To get Tenant Id
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// To get Client Id
        /// </summary>
        private Int32 ClientId
        {
            get
            {
                if (IsDefaultTenant)
                    return View.SelectedTenantId;
                return View.TenantId;
            }
        }

        public void GetCompliancePackage()
        {
            Int32 clientId = View.TenantId;
            Int32? orgUserId = null;

            //Checks if the logged in user is admin and some client is selected from the dropdown
            if (IsDefaultTenant && View.SelectedTenantId != 0)
            {
                clientId = View.SelectedTenantId;
            }
            if (!IsDefaultTenant)
            {
                orgUserId = View.CurrentLoggedInUserId;
            }
            try
            {
                View.lstCompliancePackage = ComplianceSetupManager.GetPermittedPackagesByUserID(clientId, orgUserId);
            }
            catch (Exception e)
            {
                View.lstCompliancePackage = new List<ComplaincePackageDetails>();
            }
        }

        /// <summary>
        /// To perform search
        /// </summary>
        public void PerformSearch()
        {
            if (ClientId == 0)
            {
                View.ApplicantSearchData = new List<ApplicantSearchDataContract>();
                View.VirtualPageCount = 0;
                View.CurrentPageIndex = 1;
            }
            else
            {
                SearchItemDataContract searchDataContract = new SearchItemDataContract();
                searchDataContract.ClientID = ClientId;
                searchDataContract.ApplicantFirstName = View.ApplicantFirstName;
                searchDataContract.ApplicantLastName = View.ApplicantLastName;
                searchDataContract.DateOfBirth = View.DateOfBirth;
                searchDataContract.LoggedInUserId = View.CurrentLoggedInUserId;

                try
                {
                    View.GridCustomPaging.DefaultSortExpression = QueueConstants.APPLICANT_SEARCH_DEFAULT_SORTING_FIELDS;

                    if (View.TenantId == SecurityManager.DefaultTenantID)
                        searchDataContract.IsRestricted = false;
                    else
                        searchDataContract.IsRestricted = true;

                    View.ApplicantSearchData = ComplianceDataManager.GetApplicantListDataValues(searchDataContract, View.GridCustomPaging);
                    View.VirtualPageCount = View.GridCustomPaging.VirtualPageCount;
                    View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                }
                catch (Exception e)
                {
                    View.ApplicantSearchData = new List<ApplicantSearchDataContract>();
                    throw e;
                }
            }
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public void GetGranularPermissionForDOBandSSN()
        {
            View.IsDOBDisable = false;

            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions))
            {
                if (dicPermissions.ContainsKey(EnumSystemEntity.DOB.GetStringValue()) && dicPermissions[EnumSystemEntity.DOB.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                {
                    View.IsDOBDisable = true;
                }
            }
        }

        #endregion
    }
}




