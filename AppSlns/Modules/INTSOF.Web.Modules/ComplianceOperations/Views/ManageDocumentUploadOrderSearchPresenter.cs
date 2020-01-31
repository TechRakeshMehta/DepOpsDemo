using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManageDocumentUploadOrderSearchPresenter : Presenter<IManageDocumentUploadOrderSearchView>
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

        public void GetMatchedApplicantOrderData()
        {
            List<UploadedDocumentApplicantDataContract> tempList = new List<UploadedDocumentApplicantDataContract>();

            if (View.SelectedTenantId > AppConsts.NONE && View.ApplicantXmlData.IsNotNull())
            {
                Int32? CurrentLoggedInUserID = null;
                if (!IsDefaultTenant)
                {
                    CurrentLoggedInUserID = View.CurrentLoggedInUserId;
                }
                View.GridCustomPaging.DefaultSortExpression = QueueConstants.ORDER_QUEUE_DEFAULT_SORTING_FIELDS;
                View.GridCustomPaging.SecondarySortExpression = QueueConstants.ORDER_QUEUE_SECONDARY_SORTING_FIELDS;

                tempList = ComplianceDataManager.GetUploadedDocumentApplicantOrders(View.SelectedTenantId, View.ApplicantXmlData, View.lstSelectedOrderPkgType, CurrentLoggedInUserID, View.GridCustomPaging);
            }
            if (tempList.IsNotNull() && tempList.Count > 0)
            {
                if (tempList[0].TotalCount > 0)
                {
                    View.VirtualRecordCount = tempList[0].TotalCount;
                }
                View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
            }
            else
            {
                View.VirtualRecordCount = 0;
                View.CurrentPageIndex = 1;
            }

            View.MatchedApplicantOrderList = tempList;
        }
        /// <summary>
        /// Gets the Lookup ams.lkpOrderPackageTypes
        /// </summary>
        public void GetOrderPackageTypes()
        {
            View.lstOrderPackageType = LookupManager.GetLookUpData<lkpOrderPackageType>(View.SelectedTenantId);
        }

        /// <summary>
        /// To get un-matched applicants data
        /// </summary>
        public void GetUnMatchedApplicantsData()
        {
            List<ApplicantDetailContract> tempList = new List<ApplicantDetailContract>();
            if (View.SelectedTenantId > AppConsts.NONE && View.ApplicantXmlData.IsNotNull())
            {
                tempList = ComplianceDataManager.GetUnMatchedApplicantDetails(View.SelectedTenantId, View.ApplicantXmlData);
            }
            View.UnMatchedApplicantDetails = tempList;

        }
    }
}
