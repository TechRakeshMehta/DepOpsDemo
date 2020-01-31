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
    public class AdminAuditHistoryDiscardedDocumentsPresenter : Presenter<IAdminAuditHistoryDiscardedDocumentsView>
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

        /// <summary>
        /// Get the admin Document audit History
        /// </summary>
        public void GetDiscardedDocumentDataAuditHistory()
        {
            DiscardedDocumentAuditContract objDiscardedDocAuditSearch = new DiscardedDocumentAuditContract();
            if (View.SelectedTenantId != AppConsts.NONE && View.SelectedTenantId.IsNotNull())
            {
                objDiscardedDocAuditSearch.SelectedTenantID = View.SelectedTenantId;
            }
            else
            {
                objDiscardedDocAuditSearch.SelectedTenantID = 0;
            }
            //objDiscardedDocAuditSearch.DocumentName = String.IsNullOrEmpty(View.DocumentName) ? null : View.DocumentName;
            objDiscardedDocAuditSearch.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
            objDiscardedDocAuditSearch.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
            //objDiscardedDocAuditSearch.AdminFirstName = String.IsNullOrEmpty(View.AdminFirstName) ? null : View.AdminFirstName;
            //objDiscardedDocAuditSearch.AdminLastName = String.IsNullOrEmpty(View.AdminLastName) ? null : View.AdminLastName;

            objDiscardedDocAuditSearch.AdminLoggedInUserID = View.CurrentLoggedInUserId;

            if (!View.TimeStampFromDate.IsNullOrEmpty() && View.TimeStampFromDate != DateTime.MinValue)
            {
                objDiscardedDocAuditSearch.FromDate = View.TimeStampFromDate;
            }
            else
            {
                objDiscardedDocAuditSearch.FromDate = null;
            }
            if (!View.TimeStampToDate.IsNullOrEmpty() && View.TimeStampToDate != DateTime.MinValue)
            {
                objDiscardedDocAuditSearch.ToDate = View.TimeStampToDate;
            }
            else
            {
                objDiscardedDocAuditSearch.ToDate = null;
            }
            View.ApplicantDataAuditHistoryList = ComplianceSetupManager.GetDiscardedDocumentDataAuditHistory(objDiscardedDocAuditSearch, View.GridCustomPaging);

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

            View.ApplicantDataAuditHistoryList.ForEach(x =>
            {
                x.DiscardedBy = x.AdminFirstName + " " + x.AdminLastName;
                x.StudentName = x.ApplicantFirstName + " " + x.ApplicantLastName;
            });
        }

        #endregion
    }
}
