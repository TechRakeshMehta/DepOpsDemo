using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public class AdminDocumentAuditHistoryPresenter : Presenter<IAdminDocumentAuditHistoryView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetTenants();
        }

        #region Public Methods

        /// <summary>
        /// To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            List<Entity.Tenant> lstTemp = SecurityManager.GetTenants(SortByName, false, clientCode);
            lstTemp.Insert(0, new Entity.Tenant { TenantID = 0, TenantName = "--Select--" });
            View.lstTenant = lstTemp;
        }

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        /// <summary>
        /// Get the admin Document audit History
        /// </summary>
        public void GetAdminDocumentAuditHistory()
        {

            AdminDataAuditHistory objAdminDataAuditSearch = new AdminDataAuditHistory();
            if (View.SelectedTenantId != AppConsts.NONE && View.SelectedTenantId.IsNotNull())
            { objAdminDataAuditSearch.SelectedTenantID = View.SelectedTenantId; }
            else { objAdminDataAuditSearch.SelectedTenantID = 0; }
            objAdminDataAuditSearch.DocumentName = String.IsNullOrEmpty(View.DocumentName) ? null : View.DocumentName;
            objAdminDataAuditSearch.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
            objAdminDataAuditSearch.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
            objAdminDataAuditSearch.AdminFirstName = String.IsNullOrEmpty(View.AdminFirstName) ? null : View.AdminFirstName;
            objAdminDataAuditSearch.AdminLastName = String.IsNullOrEmpty(View.AdminLastName) ? null : View.AdminLastName;

            objAdminDataAuditSearch.AdminLoggedInUserID = View.CurrentLoggedInUserId;
            

            if (!View.TimeStampFromDate.IsNullOrEmpty() && View.TimeStampFromDate != DateTime.MinValue)
            {
                objAdminDataAuditSearch.FromDate = View.TimeStampFromDate;
            }
            else
            {
                objAdminDataAuditSearch.FromDate = null;
            }
            if (!View.TimeStampToDate.IsNullOrEmpty() && View.TimeStampToDate != DateTime.MinValue)
            {
                objAdminDataAuditSearch.ToDate = View.TimeStampToDate;
            }
            else
            {
                objAdminDataAuditSearch.ToDate = null;
            }


            if (View.ActionTypeID != AppConsts.NONE && View.ActionTypeID.IsNotNull())
            { objAdminDataAuditSearch.ActionTypeID = View.ActionTypeID; }
            else { objAdminDataAuditSearch.ActionTypeID = null; }


            if (View.DiscardReasonId != AppConsts.NONE && View.DiscardReasonId.IsNotNull())
            { objAdminDataAuditSearch.DiscardReasonId = View.DiscardReasonId; }
            else { objAdminDataAuditSearch.DiscardReasonId = null; }

            View.ApplicantDataAuditHistoryList = ComplianceSetupManager.GetAdminDocumentDataAuditHistory(objAdminDataAuditSearch, View.GridCustomPaging);


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

            //try
            //{
            //    if (ClientId != 0 && ClientId.IsNotNull())
            //    {
            //        SearchItemDataContract searchDataContract = new SearchItemDataContract();

            //        //searchDataContract.DisallowApostropheConversion = true;

            //        //searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
            //        //searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;

            //        //#region UAT-950
            //        //searchDataContract.AdminFirstName = String.IsNullOrEmpty(View.AdminFirstName) ? null : View.AdminFirstName;
            //        //searchDataContract.AdminLastName = String.IsNullOrEmpty(View.AdminLastName) ? null : View.AdminLastName;
            //        //#endregion

            //        //if (!IsDefaultTenant && View.CurrentLoggedInUserId != AppConsts.NONE)
            //        //{
            //        //    searchDataContract.LoggedInUserId = View.CurrentLoggedInUserId;
            //        //}
            //        //searchDataContract.LoggedInUserTenantId = View.TenantId;
            //        //if (View.SelectedUserGroupId != AppConsts.NONE && View.SelectedUserGroupId.IsNotNull())
            //        //{
            //        //    searchDataContract.FilterUserGroupID = View.SelectedUserGroupId;
            //        //}
            //        ////UAT-2069 - Passing more than one package or category id to search filter
            //        //if (!View.SelectedCategoryIds.IsNullOrEmpty())
            //        //{
            //        //    searchDataContract.CategoryIDs = String.Join(",", View.SelectedCategoryIds);
            //        //}

            //        //if (!View.SelectedPackageIds.IsNullOrEmpty())
            //        //{
            //        //    searchDataContract.PackageIDs = String.Join(",", View.SelectedPackageIds);
            //        //}
            //        //searchDataContract.ItemID = View.SelectedItemID;
            //        //if (!View.TimeStampFromDate.IsNullOrEmpty() && View.TimeStampFromDate != DateTime.MinValue)
            //        //{
            //        //    searchDataContract.FromDate = View.TimeStampFromDate;
            //        //}
            //        //else
            //        //{
            //        //    searchDataContract.FromDate = null;
            //        //}
            //        //if (!View.TimeStampToDate.IsNullOrEmpty() && View.TimeStampToDate != DateTime.MinValue)
            //        //{
            //        //    searchDataContract.ToDate = View.TimeStampToDate;
            //        //}
            //        //else
            //        //{
            //        //    searchDataContract.ToDate = null;
            //        //}
            
            //    }
            //    else
            //    {
            //        View.ApplicantDataAuditHistoryList = new List<ApplicantDataAuditHistory>();
            //    }
            //}
            //catch (Exception e)
            //{
            //    View.ApplicantDataAuditHistoryList = new List<ApplicantDataAuditHistory>();
            //    throw e;
            //}

        }

        public void GetlkpDataEntryDocumentStatus()
        {
            List<String> requiredCode = new List<String>();
            requiredCode.Add("AAAB");
            requiredCode.Add("AAAE");
            
            View.lkpDataEntryDocumentStatus = ComplianceDataManager.GetlkpDataEntryDocumentStatus(View.TenantId);
            View.lkpDataEntryDocumentStatus = View.lkpDataEntryDocumentStatus.Where(a => requiredCode.Contains(a.LDEDS_Code)).ToList();
        }

        public void GetDocumentDiscardReasonList()
        {
            var tempList = ComplianceDataManager.GetDocumentDiscardReasonList(View.SelectedTenantId);

            //if (tempList != null)
            //{
            //    tempList.Insert(0, new Entity.ClientEntity.lkpDocumentDiscardReason { DDR_Name = "--SELECT--", DDR_ID = 0 });
            //}
            View.LstDocumentDiscradReason = tempList;
        }

        #endregion
    }
}
