using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System.Web;
using INTSOF.Contracts;
using INTSOF.ServiceUtil;

namespace CoreWeb.BkgOperations.Views
{
    public class OrderDetailPagePresenter : Presenter<IOrderDetailPageView>
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
            GetGranularPermissionForDOBandSSN();//UAT-806
        }


        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public ApplicantOrderDetail GetApplicantOrderDetail()
        {
            return BackgroundProcessOrderManager.GetApplicantOrderDetail(View.SelectedTenantId, View.OrderID);
        }
        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public Boolean UpdateOrderStatus(Int32 selectedOrderColorStatusId, Int32 selectedOrderStatusTypeId, BkgOrderPackageSvcGroup bkgOrderPackageSvcGroupObj)
        {
            return BackgroundProcessOrderManager.UpdateOrderStatus(View.SelectedTenantId, selectedOrderColorStatusId, View.OrderID, selectedOrderStatusTypeId, View.CurrentLoggedInUserId, View.orderPkgSvcGroupID, bkgOrderPackageSvcGroupObj);
        }

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public List<lkpOrderStatusType> GetOrderRequestType()
        {
            return BackgroundProcessOrderManager.GetOrderRequestType(View.SelectedTenantId);
        }

        public void CopyData(Int32 selectedOrderStatusTypeId)
        {
            lkpOrderStatusType orderStatusType = BackgroundProcessOrderManager.GetOrderRequestType(View.SelectedTenantId).FirstOrDefault(x => x.OrderStatusTypeID == selectedOrderStatusTypeId);
            //Commented below code related: UAT-1160-
            //if (orderStatusType.IsNotNull() && orderStatusType.Code == OrderStatusType.COMPLETED.GetStringValue())
            //{
                BkgOrder bkgOrder = BackgroundProcessOrderManager.GetBkgOrderByOrderID(View.SelectedTenantId, View.OrderID);

                Dictionary<String, Object> dataDict = new Dictionary<String, Object>();
                dataDict.Add("packageSubscriptionID", -1);
                dataDict.Add("tenantId", View.SelectedTenantId);
                dataDict.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
                dataDict.Add("BkgOrderID", bkgOrder.BOR_ID);

                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                ParallelTaskContext.PerformParallelTask(CopyData, dataDict, LoggerService, ExceptiomService);
           // }
        }

        private void CopyData(Dictionary<String, Object> data)
        {
            Int32 packageSubscriptionID = Convert.ToInt32(data.GetValue("packageSubscriptionID"));
            Int32 tenantId = Convert.ToInt32(data.GetValue("tenantId"));
            Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("CurrentLoggedInUserId"));
            Int32 bkgOrderID = Convert.ToInt32(data.GetValue("BkgOrderID"));
            ComplianceDataManager.CopyData(packageSubscriptionID, tenantId, currentLoggedInUserId, bkgOrderID);
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public void GetGranularPermissionForDOBandSSN()
        {
            View.IsDOBDisable = false;
            View.SSNPermissionCode = EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue();
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions))
            {
                if (dicPermissions.ContainsKey(EnumSystemEntity.DOB.GetStringValue()) && dicPermissions[EnumSystemEntity.DOB.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                {
                    View.IsDOBDisable = true;
                }
                if (dicPermissions.ContainsKey(EnumSystemEntity.SSN.GetStringValue()))
                {
                    View.SSNPermissionCode = dicPermissions[EnumSystemEntity.SSN.GetStringValue()];
                }
            }
        }

        /// <summary>
        /// Getting Masked SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetMaskedSSN(String unMaskedSSN)
        {
            return ApplicationDataManager.GetMaskedSSN(unMaskedSSN);
        }

        #endregion

        #region UAT-844
        /// <summary>
        /// Get Order package service group data by OPSG_ID
        /// </summary>
        public void GetOrderPackageServiceGroupData()
        {
            if (View.orderPkgSvcGroupID.IsNotNull())
            {
                BkgOrderPackageSvcGroup bkgOrderPackageSvcGroupObj = BackgroundProcessOrderManager.GetOrderPackageServiceGroupData(View.SelectedTenantId, View.orderPkgSvcGroupID);
                if (bkgOrderPackageSvcGroupObj.IsNotNull())
                {
                    View.bkgOrderPackageSvcGroup = bkgOrderPackageSvcGroupObj;
                }
                else
                {
                    View.bkgOrderPackageSvcGroup = new BkgOrderPackageSvcGroup();
                }
            }
        }

        /// <summary>
        /// Get Service Group Review Status list to bind combo
        /// </summary>
        public void GetServiceGroupReviewStatusList()
        {
            View.lstServiceGroupReviewStatus = BackgroundProcessOrderManager.GetServiceGroupReviewStatusList(View.SelectedTenantId)
                        .Where(x => x.BSGRS_ReviewCode.Equals(BkgSvcGrpReviewStatusType.MANUAL_REVIEW_COMPLETED.GetStringValue())
                        ).ToList();// Removing Auto Review Completed status from the dropdown
        }

        /// <summary>
        /// Get Service Group Reveiw Status Type ID by Code
        /// </summary>
        /// <param name="selectedSvcGroupReviewStatusTypeCode"></param>
        /// <returns></returns>
        public Int32? GetSvcGroupReviewStatusTypeIdByCode(String selectedSvcGroupReviewStatusTypeCode)
        {
            return BackgroundProcessOrderManager.GetSvcGroupReviewStatusTypeIdByCode(View.SelectedTenantId, selectedSvcGroupReviewStatusTypeCode);
        }

        /// <summary>
        /// Get Service Group Status Type ID by Code
        /// </summary>
        /// <param name="selectedSvcGroupStatusTypeCode"></param>
        /// <returns></returns>
        public Int32? GetSvcGroupStatusTypeIdByCode(String selectedSvcGroupStatusTypeCode)
        {
            return BackgroundProcessOrderManager.GetSvcGroupStatusTypeIdByCode(View.SelectedTenantId, selectedSvcGroupStatusTypeCode);
        }
        #endregion


        public Boolean AreServiceGroupsCompleted()
        {
            return BackgroundProcessOrderManager.AreServiceGroupsCompleted(View.SelectedTenantId, View.OrderID);
        }

        #region UAT-2304: Random review of auto completed supplements.

        /// <summary>
        /// Get Supplement Automation Pending Review Status ID
        /// </summary>
        /// <returns></returns>
        public Int32 GetSupplementAutomationPendingReviewStatusID()
        {
            String supplementAutomationPendingReviewCode = SupplementAutomationStatus.PENDING_REVIEW.GetStringValue();
            return LookupManager.GetLookUpData<Entity.ClientEntity.lkpSupplementAutomationStatu>(View.SelectedTenantId).FirstOrDefault(x => x.SAS_Code == supplementAutomationPendingReviewCode && !x.SAS_IsDeleted).SAS_ID;
        }

        /// <summary>
        /// Update Supplement Automation Status
        /// </summary>
        /// <returns></returns>
        public Boolean UpdateSupplementAutomationStatus()
        {
            String supplementAutomationReviewedCode = SupplementAutomationStatus.REVIEWED.GetStringValue();
            Int32 supplementAutomationReviewedStatusID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpSupplementAutomationStatu>(View.SelectedTenantId).FirstOrDefault(x => x.SAS_Code == supplementAutomationReviewedCode && !x.SAS_IsDeleted).SAS_ID;

            return BackgroundProcessOrderManager.UpdateSupplementAutomationStatus(View.SelectedTenantId, View.orderPkgSvcGroupID, supplementAutomationReviewedStatusID, View.CurrentLoggedInUserId);
        }

        /// <summary>
        /// Rollback Supplement Automation
        /// </summary>
        /// <returns></returns>
        public Boolean RollbackSupplementAutomation()
        {
            return BackgroundProcessOrderManager.RollbackSupplementAutomation(View.SelectedTenantId, View.OrderID, View.orderPkgSvcGroupID, View.CurrentLoggedInUserId);
        }

        #endregion
    }
}
