using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils.Consts;

namespace CoreWeb.Mobility.Views
{
    public class AdminChangeSubscriptionPresenter : Presenter<IAdminChangeSubscriptionView>
    {
        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private ISearchController _controller;
        // public ApplicantUserGroupPresenter([CreateNew] ISearchController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetTenants();
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

        /// <summary>
        /// To get Admin Program Study
        /// </summary>
        public void GetAllUserGroups()
        {
            if (ClientId == 0)
                View.lstUserGroup = new List<UserGroup>();
            else
                View.lstUserGroup = ComplianceSetupManager.GetAllUserGroup(ClientId).OrderBy(ex=>ex.UG_Name).ToList();
        }

        /// <summary>
        /// To perform search
        /// </summary>
        public void PerformSearch()
        {
            if (ClientId == 0)
                View.ApplicantSearchData = new List<Entity.ClientEntity.AdminChangeSubscription>();

            else
            {
                SearchItemDataContract searchDataContract = new INTSOF.UI.Contract.ComplianceManagement.SearchItemDataContract();
                searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
                searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
                
                if (View.DPM_ID > SysXDBConsts.NONE)
                {
                    searchDataContract.DPM_Id = View.DPM_ID;
                }
                searchDataContract.CustomFields = String.IsNullOrEmpty(View.CustomFields) ? null : View.CustomFields;
                if (View.MatchUserGroupId > SysXDBConsts.NONE)
                {
                    searchDataContract.MatchUserGroupID = View.MatchUserGroupId;
                    searchDataContract.FilterUserGroupID = View.FilterUserGroupId;
                }

                try
                {
                    View.ApplicantSearchData = MobilityManager.GetAdminChangeSubscriptionList(ClientId, searchDataContract, View.GridCustomPaging);
                    if (View.ApplicantSearchData.IsNotNull() && View.ApplicantSearchData.Count > 0)
                    {
                        if (View.ApplicantSearchData[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.ApplicantSearchData[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualRecordCount = 0;
                        View.CurrentPageIndex = 1;
                    }
                }
                catch (Exception e)
                {
                    View.ApplicantSearchData = null;
                    throw e;
                }
            }
        }
    }
}




