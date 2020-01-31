using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public class DataReconciliationQueuePresenter : Presenter<IDataReconciliationQueueView>
    {
        /// <summary>
        /// Getting Institutions list for dropdown.
        /// </summary>
        public override void OnViewLoaded()
        {
            View.lstTenants = SecurityManager.GetTenantList().OrderBy(x => x.TenantName).ToList();
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Getting data based on institution ids selected from dropdow which is present in list(selectedTenantIDs).
        /// And setting virtaul record count and page size in view.
        /// </summary>
        public void GetQueueData()
        {
            if (!View.selectedTenantIDs.IsNullOrEmpty() && View.selectedTenantIDs.Count > 0)
            {
                String institutionIds = String.Join(",", View.selectedTenantIDs);
                View.lstDataReconciliationQueueContract = ComplianceDataManager.GetQueueData(institutionIds, View.GridCustomPaging);
                if (!View.lstDataReconciliationQueueContract.IsNullOrEmpty() && View.lstDataReconciliationQueueContract.Count > 0)
                {
                    View.VirtualRecordCount = View.GridCustomPaging.VirtualPageCount;
                    View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                }
                else
                {
                    View.VirtualRecordCount = AppConsts.NONE;

                }
            }
            else
            {
                View.lstDataReconciliationQueueContract = new List<DataReconciliationQueueContract>();
            }
        }

        #region UAT-4067
        public void GetSelectedNodeIDBySubscriptionID(String selectedtenantID, String packageSubscriptionID)
        {
            Int32 tenantID = Convert.ToInt32(selectedtenantID);
            Int32 pkgSubscriptionID = Convert.ToInt32(packageSubscriptionID);
            var lstSelectedNodeIDForOrders = ComplianceDataManager.GetSelectedNodeIDBySubscriptionID(tenantID, pkgSubscriptionID);
            View.selectedNodeIDs = lstSelectedNodeIDForOrders.Where(x => !x.IsDeleted).DistinctBy(x => x.Order.SelectedNodeID).Select(x => x.Order.SelectedNodeID ?? 0).ToList();
        }
        public void GetAllowedFileExtensions(String tenantID)
        {
            Int32 tenantId = Convert.ToInt32(tenantID);
            String selectedNodeIDs = String.Join(",", View.selectedNodeIDs);
            var lstAllowedFileExtensions = ComplianceDataManager.GetAllowedFileExtensionsByNodeIDs(tenantId, selectedNodeIDs);
            View.allowedFileExtensions = lstAllowedFileExtensions.Select(x => x.Name).ToList();
        }
        #endregion

    }
}
