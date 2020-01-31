using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManageUnArchivalRequestsPresenter : Presenter<IManageUnArchivalRequestsView>
    {
        public override void OnViewInitialized()
        {
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
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

        public void GetTenants()
        {
            View.lstTenants = ComplianceDataManager.getClientTenant();
        }

        public void GetUnArchivalRequestData()
        {
            if ((View.SelectedTenantId > AppConsts.ONE && View.IsAdminLoggedIn) || View.SelectedTenantId > 1)
            {
                View.lstUnArchivalRequestDetails = ComplianceDataManager.GetUnArchivalRequestData(View.SelectedTenantId, View.SelectedSubscriptionType, View.SelectedPackageType);
            }
            else
            {
                View.lstUnArchivalRequestDetails = new List<UnArchivalRequestDetails>();
            }
        }

        public Boolean RejectUnArchivalRequests()
        {
            return ComplianceDataManager.RejectUnArchivalRequests(View.SelectedTenantId, View.SelectedUnArchivalRequestIDList, View.CurrentLoggedInUserId, View.SelectedPackageType);
        }

        public Boolean ApproveUnArchivalRequests()
        {
            return ComplianceDataManager.ApproveUnArchivalRequests(View.SelectedTenantId, View.SelectedUnArchivalRequestIDList, View.CurrentLoggedInUserId, View.SelectedSubscriptionType, View.SelectedPackageType);
        }

        #region UAT-2422
        public void SetQueueImaging()
        {
            Dictionary<String, Object> dataDictionary = new Dictionary<String, Object>();
            dataDictionary.Add("TenantID", View.SelectedTenantId);
            QueueImagingManager.AsignQueueImagingRepoInstance(dataDictionary);
        }
        #endregion
    }
}

