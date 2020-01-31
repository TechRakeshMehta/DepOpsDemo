using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using Entity.ClientEntity;
using System.Web;
using INTSOF.Contracts;
using INTSOF.ServiceUtil;
using INTSOF.Utils;

namespace CoreWeb.BkgOperations.Views
{
    public class BkgPackagesOrderedPresenter : Presenter<IBkgPackagesOrderedView>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        /// <summary>
        /// Called when viwe is initialized.
        /// </summary>
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public List<PackageDetailsContract> GetPackageByOrderId()
        {
            return BackgroundProcessOrderManager.GetPackageByOrderId(View.SelectedTenantId, View.OrderID);
        }

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public List<ExternalVendorServiceContract> GetExternalVendorServicesByOrderId()
        {
            return BackgroundProcessOrderManager.GetExternalVendorServicesByOrderId(View.SelectedTenantId, View.OrderID);
        }

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public OrderLineDetailsContract GetBkgOrderLineItemDetails(Int32 PSLI_ID)
        {
            return BackgroundProcessOrderManager.GetBkgOrderLineItemDetails(View.SelectedTenantId, PSLI_ID);
        }

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public List<lkpOrderLineItemResultStatu> GetlkpOrderLineItemResultStatus()
        {
            return BackgroundProcessOrderManager.GetlkpOrderLineItemResultStatus(View.SelectedTenantId);
        }

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public void UpdateRecordToADBCopy(BkgOrderLineItemResultCopy bkgOrderLineItemResultCopy)
        {
            //UAT-1455:UAT-1455:Flag override should re-trigger data sync for the service group
            Boolean isPackageFlaggedOverride = false;
            Boolean flaggedStatusBeforeCopy = false;
            Boolean flaggedStatusAfterCopy = false;
            flaggedStatusBeforeCopy = BackgroundProcessOrderManager.GetBackGroundPackageFlaggedStatus(View.SelectedTenantId, 
                                                                    bkgOrderLineItemResultCopy.OLIR_BkgOrderPackageSvcLineItemID);
            if (BackgroundProcessOrderManager.UpdateRecordToADBCopy(View.SelectedTenantId, bkgOrderLineItemResultCopy, View.CurrentLoggedInUserId))
            {
                //UAT-1455:UAT-1455:Flag override should re-trigger data sync for the service group
                flaggedStatusAfterCopy = BackgroundProcessOrderManager.GetBackGroundPackageFlaggedStatus(View.SelectedTenantId,
                                                                    bkgOrderLineItemResultCopy.OLIR_BkgOrderPackageSvcLineItemID);
                isPackageFlaggedOverride = flaggedStatusBeforeCopy != flaggedStatusAfterCopy ? true : false;
                ReTriggerDataSync(bkgOrderLineItemResultCopy.OLIR_BkgOrderPackageSvcLineItemID, isPackageFlaggedOverride);
            }
        }

        #region UAT-1455:Flag override should re-trigger data sync for the service group

        public void ReTriggerDataSync(Int32 PSLI_ID, Boolean isPackageFlaggedOverride)
        {
            BackgroundProcessOrderManager.RemoveDataSyncHistoryToRetriggerDataSync(View.SelectedTenantId, PSLI_ID, View.CurrentLoggedInUserId,isPackageFlaggedOverride);
            //Method to Copy Background data to compliance package 
            CopyBkgDataToCompliancePackage();
        }

        public void CopyBkgDataToCompliancePackage()
        {
            BkgOrder bkgOrder = BackgroundProcessOrderManager.GetBkgOrderByOrderID(View.SelectedTenantId, View.OrderID);
            if (bkgOrder != null)
            {
                Dictionary<String, Object> dataDict = new Dictionary<String, Object>();
                dataDict.Add("bkgOrderID", bkgOrder.BOR_ID);
                dataDict.Add("tenantId", View.SelectedTenantId);
                dataDict.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);

                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                ParallelTaskContext.PerformParallelTask(CopyData, dataDict, LoggerService, ExceptiomService);
            }
        }

        private void CopyData(Dictionary<String, Object> data)
        {
            Int32 bkgOrderId = Convert.ToInt32(data.GetValue("bkgOrderID"));
            Int32 tenantId = Convert.ToInt32(data.GetValue("tenantId"));
            Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("CurrentLoggedInUserId"));
            ComplianceDataManager.CopyData(-1, tenantId, currentLoggedInUserId, bkgOrderId);
        }
        #endregion
    }
}
