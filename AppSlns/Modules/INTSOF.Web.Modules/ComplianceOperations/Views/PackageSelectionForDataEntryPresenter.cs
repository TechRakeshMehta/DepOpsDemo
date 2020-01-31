using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public class PackageSelectionForDataEntryPresenter : Presenter<IPackageSelectionForDataEntryView>
    {

        #region Methods

        public void GetPackageSubscriptionOfApplicant()
        {
            List<PackageSubscriptionForDataEntry> lstPackageSubscription = new List<PackageSubscriptionForDataEntry>();
            if (View.SelectedTenantID > 0 && View.ApplicantOrganizationUserID > 0)
            {
                lstPackageSubscription = ComplianceDataManager.GetPackageSubscriptionForDataEntry(View.ApplicantOrganizationUserID, View.SelectedTenantID);
            }
            View.lstPackageSubscription = lstPackageSubscription;
        }

        /// <summary>
        /// Upadte the Document status to 'Complete' if the admin chooses different subscription to load,
        /// while working on Details screen.
        /// </summary>
        public void UpdateDocumentStatus()
        {
            ComplianceDataManager.UpdateDoccumentStatusAfterDataEntry(View.DocumentId, View.FDEQId, View.DocumentStatus, View.CurrentUserId, View.SelectedTenantID);
        }

        /// <summary>
        /// Update the Time tracking
        /// </summary>
        public void UpdateTimeTracking()
        {
            ComplianceDataManager.DataEntryTimeTracking(View.DataEntryTimeTracking, View.SelectedTenantID);
        }

        /// <summary>
        /// Get the DocumentStatusID by Code, to be used for Data Entry time tracking.
        /// </summary>
        public void GetDocumentStatusIdByCode()
        {
            View.DocumentStatusId = ComplianceDataManager.GetDocumentStatusIdByCode(View.DocumentStatus);
        }

        #endregion

    }
}
