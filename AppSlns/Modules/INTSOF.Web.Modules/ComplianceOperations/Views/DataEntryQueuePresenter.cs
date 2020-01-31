using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public class DataEntryQueuePresenter : Presenter<IDataEntryQueueView>
    {
        #region Properties

        /// <summary>
        /// Check if logged in user is default tenant/ADB admin
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return View.TenantId == SecurityManager.DefaultTenantID;
            }
        }
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get the ADB user list 
        /// </summary>
        public void GetUserListForSelectedTenant()
        {

            if (View.TenantId > AppConsts.NONE && View.IsAdminAssignmentQueue)
            {
                View.lstOrganizationUser = SecurityManager.GetOganisationUsersByTanentId(View.TenantId,IsDefaultTenant,false,false,true,false).Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName + " " + x.LastName,
                    OrganizationUserID = x.OrganizationUserID
                }).ToList();
            }
        }

        /// <summary>
        ///Get Tenanat List 
        /// </summary>
        public void GetTenantList()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetTenants(SortByName, false, clientCode); ;
        }

        public void GetDataEntryAssignmentData()
        {
            String inputXml = null;

            if (View.SelectedTenantIds.IsNotNull() && View.SelectedTenantIds.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Int32 item in View.SelectedTenantIds)
                {
                    sb.Append("<TenantId>" + item.ToString() + "</TenantId>");
                }

                inputXml = "<TenantIdList>" + sb + "</TenantIdList>";
            }

            if (View.IsAdminAssignmentQueue)
            {
                if (View.SelectedTenantIds.Count == AppConsts.ONE)
                    View.lstDataEntryQueueDetail = SecurityManager.GetDataEntryQueueData(inputXml, View.GridCustomPaging, null, View.NodeIds, true, View.SelectedTenantIds.FirstOrDefault());
                else
                    View.lstDataEntryQueueDetail = SecurityManager.GetDataEntryQueueData(inputXml, View.GridCustomPaging, null, String.Empty, false, AppConsts.NONE);
            }
            else
            {
                if (View.SelectedTenantIds.Count == AppConsts.ONE)
                    View.lstDataEntryQueueDetail = SecurityManager.GetDataEntryQueueData(inputXml, View.GridCustomPaging, View.CurrentLoggedInUserID, View.NodeIds, true, View.SelectedTenantIds.FirstOrDefault());
                else
                    View.lstDataEntryQueueDetail = SecurityManager.GetDataEntryQueueData(inputXml, View.GridCustomPaging, View.CurrentLoggedInUserID, String.Empty, false, AppConsts.NONE);
            }



            if (View.lstDataEntryQueueDetail != null && View.lstDataEntryQueueDetail.Count > 0)
            {
                View.VirtualPageCount = View.GridCustomPaging.VirtualPageCount;
                View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
            }
            else
            {
                View.VirtualPageCount = 0;
                View.CurrentPageIndex = 1;
            }
        }

        public Boolean AssignDocumentsToUser(Int32 selectedAssignUserId, String selectedAssigneeUserName)
        {
            if (View.DocumentIdListToAssign.IsNotNull() && View.DocumentIdListToAssign.Count > AppConsts.NONE && selectedAssignUserId > AppConsts.NONE)
                return SecurityManager.AssignDocumentToUserForDataEntry(selectedAssignUserId, selectedAssigneeUserName, View.DocumentIdListToAssign, View.CurrentLoggedInUserID);
            return false;
        }
        #endregion

        #region Private Methods
        #endregion

        /// <summary>
        /// Method to get package subscription of applicant 
        /// </summary>
        /// <param name="selectedTenantID"></param>
        /// <param name="applicantID"></param>
        /// <returns></returns>
        public List<PackageSubscriptionForDataEntry> GetPackageSubscriptionOfApplicant(Int32 selectedTenantID, Int32 applicantID)
        {
            return ComplianceDataManager.GetPackageSubscriptionForDataEntry(applicantID, selectedTenantID);
        }

        #endregion
    }
}
