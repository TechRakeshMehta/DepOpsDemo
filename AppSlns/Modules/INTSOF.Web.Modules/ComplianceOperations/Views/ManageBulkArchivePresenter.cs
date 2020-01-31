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

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManageBulkArchivePresenter : Presenter<IManageBulkArchiveView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetTenants();
            GetGranularPermissions();
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
        /// Check Admin is Logged In or not
        /// </summary>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }

        public void GetSubscriptionData()
        {
            List<UploadedDocumentApplicantDataContract> tempSubscriptionList = new List<UploadedDocumentApplicantDataContract>();
            if (View.SelectedTenantId > AppConsts.NONE && View.ApplicantXmlData.IsNotNull())
            {
                Int32? CurrentLoggedInUserID = null;
                if (!IsDefaultTenant)
                {
                    CurrentLoggedInUserID = View.CurrentLoggedInUserId;
                }
                tempSubscriptionList = ComplianceDataManager.GetApplicantSubscriptionDataBulkArchive(View.SelectedTenantId, View.ApplicantXmlData, CurrentLoggedInUserID);
                //tempSubscriptionList = ComplianceDataManager.AssignMatchedApplicantSubscriptionsDataToModel(tempDataSet.Tables[0]);
                //View.UnMatchedApplicantDetails = ComplianceDataManager.GetUnMatchedApplicantData(tempDataSet.Tables[1]);
            }
            View.MatchedApplicantSubscriptionList = tempSubscriptionList;
            //View.UnMatchedApplicantDetails = new List<ApplicantDetailContract>();

        }

        public void GetUnMatchedApplicantsData()
        {
            List<ApplicantDetailContract> tempList = new List<ApplicantDetailContract>();
            if (View.SelectedTenantId > AppConsts.NONE && View.ApplicantXmlData.IsNotNull())
            {

                tempList = ComplianceDataManager.GetUnMatchedApplicantDetails(View.SelectedTenantId, View.ApplicantXmlData);
            }
            View.UnMatchedApplicantDetails = tempList;

        }

        public Boolean ArchiveSelectedSubscriptions()
        {
            String result = ComplianceDataManager.ArchieveSubscriptionsManually(View.SelectedSubscriptionsToArchive, View.SelectedTenantId, View.CurrentLoggedInUserId);
            return result == "true" ? true : false;
        }

        #region UAT-2422
        public void SetQueueImaging()
        {
            Dictionary<String, Object> dataDictionary = new Dictionary<String, Object>();
            dataDictionary.Add("TenantID", View.SelectedTenantId);
            QueueImagingManager.AsignQueueImagingRepoInstance(dataDictionary);
        }
        #endregion

        #region  UAT-3010:-  Granular Permission for Client Admin.

        public void GetGranularPermissions()
        {
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions))
            {
                if (dicPermissions.ContainsKey(EnumSystemEntity.ARCHIVE_ABILITY.GetStringValue()))
                {
                    View.ArchivePermissionCode = dicPermissions[EnumSystemEntity.ARCHIVE_ABILITY.GetStringValue()];
                }

            }
        }
        #endregion

    }
}
