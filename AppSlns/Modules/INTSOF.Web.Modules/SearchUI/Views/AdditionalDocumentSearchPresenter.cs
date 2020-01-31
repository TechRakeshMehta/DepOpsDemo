using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.SearchUI.Views
{
    public class AdditionalDocumentSearchPresenter : Presenter<IAdditionalDocumentSearchView>
    {
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
                return SecurityManager.DefaultTenantID == View.TenantID;
            }
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
                return View.TenantID;
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

        public void PerformSearch()
        {
            if (ClientId == 0)
            {
                View.AdditionalDocumentList = new List<ComplianceDocumentSearchContract>();
            }
            else
            {
                SearchItemDataContract searchDataContract = new SearchItemDataContract();
                searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
                searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
                searchDataContract.DocumentName = String.IsNullOrEmpty(View.DocumentName) ? null : View.DocumentName.ToFormatApostrophe();
                if (View.TenantID != SecurityManager.DefaultTenantID)
                {
                    searchDataContract.LoggedInUserId = View.CurrentLoggedInUserId;
                }

                try
                {
                    View.AdditionalDocumentList = ComplianceDataManager.GetAdditionalDocumentSearch(ClientId, searchDataContract, View.GridCustomPaging);
                    if (View.AdditionalDocumentList.IsNotNull() && View.AdditionalDocumentList.Count > 0)
                    {
                        if (View.AdditionalDocumentList[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.AdditionalDocumentList[0].TotalCount;
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
                    View.AdditionalDocumentList = new List<ComplianceDocumentSearchContract>();
                    throw e;
                }
            }
        }
    }
}
