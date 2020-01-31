using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using Entity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ADBAdminDataAuditHistoryPresenter : Presenter<IADBAdminDataAuditHistoryView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetTenants();
        }

        #region Properties

        #endregion

        #region Methods

        #region Private Methods
        #endregion

        #region Public Methods

        /// <summary>
        /// To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            List<Entity.Tenant> lstTemp = SecurityManager.GetTenants(SortByName, false, clientCode);
            //lstTemp.Insert(0, new Entity.Tenant { TenantID = 0, TenantName = "--Select--" });
            View.lstTenant = lstTemp;
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        /// <summary>
        /// Get the ADB Admin Data audit History
        /// </summary>
        public void GetApplicantDataAuditHistory()
        {
            try
            {
                if (View.lstSelectedTenants.IsNullOrEmpty())
                {
                    View.ApplicantDataAuditHistoryList = new List<ApplicantDataAuditHistoryContract>();
                }
                else
                {
                    SearchItemDataContract searchDataContract = new SearchItemDataContract();

                    searchDataContract.DisallowApostropheConversion = true;

                    searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.FirstName) ? null : View.FirstName;
                    searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.LastName) ? null : View.LastName;
                    #region UAT- 4107
                    searchDataContract.RoleNames = String.IsNullOrEmpty(View.selectedFltrRoleNames) ? null : View.selectedFltrRoleNames;
                    #endregion
                    #region UAT-950
                    searchDataContract.AdminFirstName = String.IsNullOrEmpty(View.AdminFirstName) ? null : View.AdminFirstName;
                    searchDataContract.AdminLastName = String.IsNullOrEmpty(View.AdminLastName) ? null : View.AdminLastName;
                    #endregion

                    if (!IsDefaultTenant && View.CurrentLoggedInUserId != AppConsts.NONE)
                    {
                        searchDataContract.LoggedInUserId = View.CurrentLoggedInUserId;
                    }
                    searchDataContract.LoggedInUserTenantId = View.TenantId;
                    searchDataContract.PackageName = View.PackageName;
                    searchDataContract.CategoryName = View.CategoryName;
                    searchDataContract.ItemName = View.ItemName;
                    if (!View.TimeStampFromDate.IsNullOrEmpty() && View.TimeStampFromDate != DateTime.MinValue)
                    {
                        searchDataContract.FromDate = View.TimeStampFromDate;
                    }
                    else
                    {
                        searchDataContract.FromDate = null;
                    }
                    if (!View.TimeStampToDate.IsNullOrEmpty() && View.TimeStampToDate != DateTime.MinValue)
                    {
                        searchDataContract.ToDate = View.TimeStampToDate;
                    }
                    else
                    {
                        searchDataContract.ToDate = null;
                    }
                    var lstSelectedTenants = View.lstSelectedTenants.Select(x => x.TenantID).ToList();
                    searchDataContract.SelectedTenantIDs = String.Join(",", lstSelectedTenants);

                    View.ApplicantDataAuditHistoryList = SharedUserClinicalRotationManager.GetApplicantDataAuditHistory(searchDataContract, View.GridCustomPaging);

                    if (View.ApplicantDataAuditHistoryList.IsNotNull() && View.ApplicantDataAuditHistoryList.Count > 0)
                    {
                        if (View.ApplicantDataAuditHistoryList[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.ApplicantDataAuditHistoryList[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualRecordCount = 0;
                        View.CurrentPageIndex = 1;
                    }
                }
            }
            catch (Exception e)
            {
                View.ApplicantDataAuditHistoryList = new List<ApplicantDataAuditHistoryContract>();
                throw e;
            }
        }

        #endregion

        #endregion
    }
}




