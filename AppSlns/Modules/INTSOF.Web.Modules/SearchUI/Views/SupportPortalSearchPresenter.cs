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
    public class SupportPortalSearchPresenter : Presenter<ISupportPortalSearchView>
    {

        public void GetTenantList()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetTenants(SortByName, false, clientCode); ;
        }


        public void GetSupportPortalSearchData()
        {
            View.lstApplicantData = new List<ApplicantData>();
            if (View.SelectedTenantIds.IsNullOrEmpty())
            {
                View.lstApplicantData = new List<ApplicantData>();
                return;
            }

            View.searchContract = new SearchItemDataContract();

            String selectedTenantIds = String.Empty;
            if (!View.SelectedTenantIds.IsNullOrEmpty())
            {
                View.searchContract.SelectedTenants = View.SelectedTenantIds;
                selectedTenantIds = String.Join(",", View.SelectedTenantIds);
            }

            // UAT:-4153:- Add "Account Activated" (can be Yes/No) info in Support Portal and Manage Preceptor results grids.
            View.searchContract.IsAccountActivated = View.IsAccountActivated;
            //UAT-4020

            #region //UAT-4155
            View.searchContract.UserName = View.ApplicantUserName;

            #endregion

            View.searchContract.SelectedUserTypeCode = View.SelectedUserTypeIds;

            View.searchContract.ApplicantFirstName = View.ApplicantFirstName;
            View.searchContract.ApplicantLastName = View.ApplicantLastName;

            View.searchContract.ApplicantSSN = ApplicationDataManager.GetSSNForFilters(View.SSN);//UAT-4355 //View.SSN;
            View.searchContract.DateOfBirth = View.DOB;
            View.searchContract.EmailAddress = View.EmailAddress;



            View.searchContract.GridCustomPagingArguments = View.GridCustomPaging;
            View.lstApplicantData = ComplianceDataManager.GetSupportPortalSearchData(View.searchContract, selectedTenantIds, View.GridCustomPaging, View.CurrentLoggedInUserId);

            //UAT- 4247 BugID: 22009
            if (View.lstApplicantData.IsNotNull() && View.lstApplicantData.Count > 0)
            {
                if (View.lstApplicantData[0].TotalCount > 0)
                {
                    View.VirtualRecordCount = View.lstApplicantData[0].TotalCount;
                }
                View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
            }
            else
            {
                View.VirtualRecordCount = 0;
                View.CurrentPageIndex = 1;
            }
            //UAT- 4247 BugID: 22009

            //if (View.lstApplicantData.IsNullOrEmpty())
            //    View.VirtualRecordCount = AppConsts.NONE;
            //else
            //    View.VirtualRecordCount = View.lstApplicantData.FirstOrDefault().TotalCount;
        }

        /// <summary>
        /// To get User Types: Applicant / Instr-Preceptor -UAT-4020
        /// </summary>
        public void GetUserType()
        {
            if (!View.SelectedTenantIds.IsNullOrEmpty())
                View.dicUserTypes = new Dictionary<String, String>();
            else
            {
                Dictionary<String, String> dicUserTypes = new Dictionary<String, String>();
                dicUserTypes.Add("AAAC", UserTypeSwitchView.Applicant.ToString());
                dicUserTypes.Add("AAAE", "Instructor/Preceptor");
                dicUserTypes.Add("AAAB", "Client Admin");
                View.dicUserTypes = dicUserTypes;
            }
        }

    }
}
