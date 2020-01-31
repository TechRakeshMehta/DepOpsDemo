using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Interfaces
{
    public interface IEVOrderClientRepository
    {
        List<OrderToBeDisptached> GetOrdersToBeDispatchedToVendor(Int32 chunkSize, Int32 maxRetryCount, Int32 retryTimeLag, Boolean isTestMode);

        Boolean SaveExtSvcInvokeHistory(ExtSvcInvokeHistory extSvcInvokeHistory);

        BkgOrder GetBkgOrder(Int32 bkgOrderID);

        BkgOrderPackageSvcLineItem GetBkgOrderPackageSvcLineItem(Int32 bkgOrderPackageSvcLineItemID);

        Boolean SaveVendorOrderData(List<ExtVendorReport> extVendorReports);

        /// <summary>
        /// Returns a collection of Test Mode Background order IDs.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        List<Int32> GetTestModeBkgOrders();

        void UpdateRetryCountForBkgOrderLineItems(List<Int32> bkgOrderLineItemIDs);

        List<usp_GetOrdersToBeUpdatedVendorData_Result> GetOrdersToBeUpdatedVendorData(Int32 chunkSize, Int32 startingOrderID, Int32 endingOrderID, Int32 recentDays, Int32 updateOrderRetryTimeLag);

        ExtVendorBkgOrderLineItemDetail GetExtVendorBkgOrderLineItemDetail(Int32 extVendorBkgOrderLineItemDetailID);

        Boolean UpdateVendorData();

        ExternalVendorBkgOrderDetail GetExternalVendorBkgOrderDetail(Int32 externalVendorBkgOrderDetailID);

        IEnumerable<OrderAttribute> GetAttributesForOrder(Int32 orderID);

        IEnumerable<BkgOrderLineItemDataMapping> GetBkgOrderLineItemDataMappings(IEnumerable<Int32> bkgOrderLineItemIDs);

        String GetCCFDocument(Int32 organizationUserProfileID);

        void CompleteOrderServiceForms(Int32 bkgOrderID);

        IEnumerable<usp_GetOrdersToBeCreateJiraTicket_Result> GetOrdersToBeCreateJiraTicket(Int32 chunkSize, Int32 maxRetryCount, Int32 createJiraTicketTimeLag
                                                                                            , Int32 retryCountTimeLag);

        List<ExtSvcInvokeHistory> GetExtVendorUploadErrorListFromExtSvcInvokeHistory(Int32 bkgOrderID,
                                                                                      IEnumerable<usp_GetOrdersToBeCreateJiraTicket_Result> orderToBeCreateJiraTicket);

        BkgOrderPackageSvcGroup GetBkgOrderPkgSvcGroupDetail(Int32 bkgOrderPkgSvcGroupDetailID);
        List<BkgOrderPackageSvcGroup> GetBkgOrderPkgSvcGroupByOrderID(List<Int32> bopIds);

        Boolean CreateJiraTicketDetail(JiraTicketDetail jiraTicketDetail);

        Boolean UpdateBkgOrderIgnoredCreateJiraTicket(Int32 currentUserID, Int32 bkgOrderID);

        /// <summary>
        /// To check if any items are pending of a external vendor order service group
        /// </summary>
        /// <param name="organizationUserProfileID"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        Boolean CheckPendingSvcGroupItems(Int32 externalVendorOrderDetailID);

        #region UAT-1852: If a Service Group is complete, but other service groups are not complete within an order, the system should send the applicant an email.
        //DataTable GetserviceGroupDetailsForMail(Int32 orderPackageSvcGrpId, Int32 bkgOrderId);
        #endregion

        #region Method to Get Current processed order profile Aliase
        List<PersonAliasProfile> GetOrderProfileAliases(Int32 orderID, Int32 organizationUSerProfileID);
        #endregion

        Dictionary<String, List<Int32>> UpdateEndDateForCurrentEmployer(List<Int32> lstAttGrpMapIds);

        #region UAT 3331
        Int32? GetExternalVendorResultResponseFormatTypeId(Int32 externalVendorOrderLineItemId); 
        #endregion
    }
}
