#region Header Comment Block

// 
// Copyright 2014 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ExternalVendorOrderManager.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#region Application Specific

using Entity;
using Entity.ClientEntity;
using Entity.ExternalVendorContracts;
using INTSOF.Utils;
using System.Data;
using System.Text;
using INTSOF.UI.Contract.BkgOperations;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using INTSOF.UI.Contract.BkgSetup;
using System.Xml;
using System.Xml.Linq;

#endregion

#endregion

namespace Business.RepoManagers
{
    public class ExternalVendorOrderManager
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static Int32 _bkgOrderServiceUserId;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        static ExternalVendorOrderManager()
        {
            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BKG_ORDER_SERVICE_USER_ID);
            _bkgOrderServiceUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BKG_ORDER_SERVICE_USER_ID_VALUE;
        }

        private static Int32 BkgOrderServiceUserID
        {
            get
            {
                return _bkgOrderServiceUserId;
            }
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get list of external vendor orders
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        public static List<OrderToBeDisptached> GetOrdersToBeDispatchedToVendor(Int32 tenantID, Int32 chunkSize, Int32 maxRetryCount, Int32 retryTimeLag, Boolean isTestMode)
        {
            try
            {
                return BALUtils.GetEVOrderClientRepoInstance(tenantID).GetOrdersToBeDispatchedToVendor(chunkSize, maxRetryCount, retryTimeLag, isTestMode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        /// <summary>
        /// Get External Service Code for BkgService
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        public static Dictionary<Int32, String> GetExternalServiceCodeForBkgService(List<Int32> backgroundServiceIDs)
        {
            try
            {
                return BALUtils.GetEVOrderSecurityRepoInstance().GetExternalServiceCodeForBkgService(backgroundServiceIDs);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        public static Boolean SaveExtSvcInvokeHistory(Int32 tenantID, Int32? bkgOrderID, Int32? bkgOrderPackageSvcLineItemID, Int32? bkgOrderPackageSvcGroupID, String methodName, String parameterData,
                                                      String response, String svcName, DateTime methodStartTime, DateTime methodEndTime, String comment = "")
        {
            try
            {
                ExtSvcInvokeHistory extSvcInvokeHistory = new ExtSvcInvokeHistory();
                extSvcInvokeHistory.ESIH_BkgOrderID = bkgOrderID;
                extSvcInvokeHistory.ESIH_BkgOrderPackageSvcGroupID = bkgOrderPackageSvcGroupID;
                extSvcInvokeHistory.ESIH_BkgOrderPackageSvcLineItemID = bkgOrderPackageSvcLineItemID;
                extSvcInvokeHistory.ESIH_MethodName = methodName;
                extSvcInvokeHistory.ESIH_ParameterData = parameterData;
                extSvcInvokeHistory.ESIH_Response = response;
                extSvcInvokeHistory.ESIH_SvcName = svcName;
                extSvcInvokeHistory.ESIH_CreatedBy = BkgOrderServiceUserID;
                extSvcInvokeHistory.ESIH_CreatedOn = DateTime.Now;
                extSvcInvokeHistory.ESIH_MethodStartTime = methodStartTime;
                extSvcInvokeHistory.ESIH_MethodEndTime = methodEndTime;

                if (!String.IsNullOrEmpty(comment))
                {
                    extSvcInvokeHistory.ESIH_Comments = comment;
                }

                return BALUtils.GetEVOrderClientRepoInstance(tenantID).SaveExtSvcInvokeHistory(extSvcInvokeHistory);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError("ExternalVendors" + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError("ExternalVendors" + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        public static Boolean SaveVendorOrderData(EvCreateOrderContract evOrderContract, Int32 tenantID, Boolean isNotificationRequestOn)
        {
            //Check for any Vendor Error while sending Order
            //No Vendor Error
            List<ExtSvcIntegrationRecord> ExtSvcIntegrationRecordToSave = new List<ExtSvcIntegrationRecord>();
            List<ExtVendorBkgOrderLineItemDetail> extVendorBkgOrderLineItemDetailToSave = new List<ExtVendorBkgOrderLineItemDetail>();
            List<lkpOrderStatusType> orderStatusTypeList = LookupManager.GetLookUpData<lkpOrderStatusType>(tenantID).ToList();

            //BkgOrder
            String previousOrderStatusTypeCode = String.Empty;
            BkgOrder bkgOrder = BALUtils.GetEVOrderClientRepoInstance(tenantID).GetBkgOrder(evOrderContract.BkgOrderID);
            if (bkgOrder.IsNotNull())
            {
                previousOrderStatusTypeCode = bkgOrder.lkpOrderStatusType.IsNotNull() ? bkgOrder.lkpOrderStatusType.Code : String.Empty;
                bkgOrder.BOR_ModifiedByID = BkgOrderServiceUserID;
                bkgOrder.BOR_ModifiedOn = DateTime.Now;
                if (orderStatusTypeList.IsNotNull())
                {
                    //lkpOrderStatusType additionalWorkStatus = orderStatusTypeList
                    //                                        .FirstOrDefault(cond => cond.Code == OrderStatusType.ADDITIONALWORK.GetStringValue());

                    //if (additionalWorkStatus.IsNotNull() && !bkgOrder.BOR_OrderStatusTypeID.Equals(additionalWorkStatus.OrderStatusTypeID))
                    //{
                    if (orderStatusTypeList.Any(cond => cond.Code == OrderStatusType.INPROGRESS.GetStringValue()))
                    {
                        //UAT-1120 Order Review Document Points
                        //Leo: Rajeev, Can we please add a status on the UI for when supplemented work is in progress? We can call this “Additional Work In-Progress.
                        if (previousOrderStatusTypeCode == OrderStatusType.ADDITIONALWORK.GetStringValue()
                            && orderStatusTypeList.Any(cond => cond.Code == OrderStatusType.ADDITIONALWORKINPROGRESS.GetStringValue()))
                        {
                            bkgOrder.BOR_OrderStatusTypeID = orderStatusTypeList.Where(cond => cond.Code == OrderStatusType.ADDITIONALWORKINPROGRESS.GetStringValue())
                                                             .FirstOrDefault().OrderStatusTypeID;
                        }
                        else
                        {
                            bkgOrder.BOR_OrderStatusTypeID = orderStatusTypeList.Where(cond => cond.Code == OrderStatusType.INPROGRESS.GetStringValue())
                                                                                            .FirstOrDefault().OrderStatusTypeID;
                        }

                        if (previousOrderStatusTypeCode != OrderStatusType.INPROGRESS.GetStringValue())
                        {
                            BkgOrderEventHistory bkgOrderEventHistory = null;
                            String orderNewStatusTypeCode = String.Empty;
                            if (previousOrderStatusTypeCode == OrderStatusType.ADDITIONALWORK.GetStringValue())
                            {
                                orderNewStatusTypeCode = OrderStatusType.ADDITIONALWORKINPROGRESS.GetStringValue();
                            }
                            else
                            {
                                orderNewStatusTypeCode = OrderStatusType.INPROGRESS.GetStringValue();
                            }

                            //Add Order Event History when Order Status changes. ADD new entry in Order Event History.                
                            bkgOrderEventHistory = InsertBkgOrderEventHistory(tenantID, orderStatusTypeList, previousOrderStatusTypeCode, orderNewStatusTypeCode);

                            if (bkgOrderEventHistory.IsNotNull())
                            {
                                bkgOrder.BkgOrderEventHistories.Add(bkgOrderEventHistory);
                            }
                        }
                        //}
                    }
                }


                //Get Look Up tables Data
                List<lkpDispatchStatu> bkgOrderDetailDisptachStatus = LookupManager.GetLookUpData<lkpDispatchStatu>(tenantID).ToList();
                List<lkpOrderLineItemStatu> orderLineItemStatuList = LookupManager.GetLookUpData<lkpOrderLineItemStatu>(tenantID).ToList();
                List<lkpSvcLineItemDispatchStatu> lkpSvcLineItemDispatchStatusList = LookupManager.GetLookUpData<lkpSvcLineItemDispatchStatu>(tenantID).ToList();
                List<lkpOrderLineItemResultStatu> lkpOrderLineItemResultStatusList = LookupManager.GetLookUpData<lkpOrderLineItemResultStatu>(tenantID).ToList();

                List<Int32> errorOrderItemIDs = new List<Int32>();
                List<ExtVendorReport> extVendorReports = new List<ExtVendorReport>();
                List<Int32> successfulSvcGroupIds = new List<Int32>();
                evOrderContract.PackageSvcGroups.ForEach(pkgSvcGroup =>
                    {
                        if (!pkgSvcGroup.VendorResponse.IsVendorError)
                        {
                            //ExtVendorReport
                            ExtVendorReport extVendorReport = new ExtVendorReport();
                            extVendorReport.EVR_ReportPath = String.Empty;
                            extVendorReport.EVR_IsDeleted = false;
                            extVendorReport.EVR_CreatedByID = BkgOrderServiceUserID;
                            extVendorReport.EVR_CreatedOn = DateTime.Now;

                            //Insert ExternalVendorBkgOrderDetails
                            ExternalVendorBkgOrderDetail externalVendorBkgOrderDetail = new ExternalVendorBkgOrderDetail();
                            externalVendorBkgOrderDetail.EVOD_AccountNumber = evOrderContract.AccountNumber;
                            externalVendorBkgOrderDetail.EVOD_BkgOrderID = evOrderContract.BkgOrderID;
                            externalVendorBkgOrderDetail.EVOD_CreatedByID = BkgOrderServiceUserID;
                            externalVendorBkgOrderDetail.EVOD_CreatedOn = DateTime.Now;
                            if (bkgOrderDetailDisptachStatus.IsNotNull() && bkgOrderDetailDisptachStatus.Any(cond => cond.DST_Code == BkgOrderDetailDispatchStatus.DISPATCHED.GetStringValue()))
                            {
                                externalVendorBkgOrderDetail.EVOD_DispatchStatusID = bkgOrderDetailDisptachStatus.FirstOrDefault(cond => cond.DST_Code ==
                                                                                 BkgOrderDetailDispatchStatus.DISPATCHED.GetStringValue()).DST_ID;
                            }
                            externalVendorBkgOrderDetail.EVOD_IsDeleted = false;
                            externalVendorBkgOrderDetail.EVOD_VendorID = evOrderContract.VendorID;
                            externalVendorBkgOrderDetail.EVOD_VendorProfileID = pkgSvcGroup.BkgOrderVendorProfileID;
                            externalVendorBkgOrderDetail.EVOD_BkgOrderPackageSvcGroupID = pkgSvcGroup.BkgOrderPkgSvcGroupID;
                            successfulSvcGroupIds.Add(pkgSvcGroup.BkgOrderPkgSvcGroupID);

                            foreach (var item in pkgSvcGroup.OrderItems)
                            {
                                if (!item.VendorResponse.IsVendorError)
                                {
                                    //BkgOrderPackageSvcLineItem
                                    BkgOrderPackageSvcLineItem bkgOrderPackageSvcLineItem = BALUtils.GetEVOrderClientRepoInstance(tenantID).
                                                                                            GetBkgOrderPackageSvcLineItem(item.BkgOrderPackageSvcLineItemID);

                                    if (bkgOrderPackageSvcLineItem.IsNotNull())
                                    {
                                        bkgOrderPackageSvcLineItem.PSLI_ModifiedByID = BkgOrderServiceUserID;
                                        bkgOrderPackageSvcLineItem.PSLI_ModifiedOn = DateTime.Now;

                                        if (!item.VendorResponse.IsSpecialError)
                                        {
                                            //Set PSLI_DispatchedExternalVendor to Dispatched
                                            if (lkpSvcLineItemDispatchStatusList.IsNotNull() && lkpSvcLineItemDispatchStatusList.Any(cond => cond.SLIDS_Code
                                                                                                                                    == SvcLineItemDispatchStatus.DISPATCHED.GetStringValue()))
                                            {
                                                bkgOrderPackageSvcLineItem.PSLI_DispatchedExternalVendor = lkpSvcLineItemDispatchStatusList.FirstOrDefault(cnd => cnd.SLIDS_Code
                                                                                                           == SvcLineItemDispatchStatus.DISPATCHED.GetStringValue()).SLIDS_ID;
                                            }
                                        }
                                        else
                                        {
                                            //Set PSLI_DispatchedExternalVendor to Dispatched Error
                                            if (lkpSvcLineItemDispatchStatusList.IsNotNull() && lkpSvcLineItemDispatchStatusList.Any(cond => cond.SLIDS_Code
                                                                                                                                    == SvcLineItemDispatchStatus.DISPATCHED_ERROR.GetStringValue()))
                                            {
                                                bkgOrderPackageSvcLineItem.PSLI_DispatchedExternalVendor = lkpSvcLineItemDispatchStatusList.FirstOrDefault(cnd => cnd.SLIDS_Code
                                                                                                           == SvcLineItemDispatchStatus.DISPATCHED_ERROR.GetStringValue()).SLIDS_ID;
                                            }
                                        }
                                        //Set PSLI_OrderLineItemStatusID
                                        if (orderLineItemStatuList.IsNotNull() && orderLineItemStatuList.Any(cond => cond.OLIS_Code == OrderStatusType.INPROGRESS.GetStringValue()))
                                        {
                                            bkgOrderPackageSvcLineItem.PSLI_OrderLineItemStatusID = orderLineItemStatuList.Where(cond => cond.OLIS_Code
                                                                                                    == OrderLineItemStatus.NEW.GetStringValue()).FirstOrDefault().OLIS_ID;
                                        }
                                    }

                                    //ExtVendorBkgOrderLineItemDetails
                                    ExtVendorBkgOrderLineItemDetail extVendorBkgOrderLineItemDetail = new ExtVendorBkgOrderLineItemDetail();
                                    extVendorBkgOrderLineItemDetail.OLID_AccountNumber = evOrderContract.AccountNumber;
                                    extVendorBkgOrderLineItemDetail.OLID_BkgOrderPackageSvcLineItemID = item.BkgOrderPackageSvcLineItemID;
                                    extVendorBkgOrderLineItemDetail.OLID_CreatedByID = BkgOrderServiceUserID;
                                    extVendorBkgOrderLineItemDetail.OLID_CreatedOn = DateTime.Now;
                                    extVendorBkgOrderLineItemDetail.OLID_IsDeleted = false;
                                    extVendorBkgOrderLineItemDetail.OLID_VendorLineItemOrderID = item.ExternalVendorOrderID;
                                    extVendorBkgOrderLineItemDetail.OLID_VendorSvcID = item.BackgroundServiceID;

                                    if (lkpOrderLineItemResultStatusList.IsNotNull())
                                    {
                                        //If Order is Transmitted on Vendor then OrderSvcLineItem Status should be In-Progress 
                                        //Else OrderSvcLineItem Status should be Draft
                                        if (pkgSvcGroup.IsTransmitted && lkpOrderLineItemResultStatusList.Any(cond => cond.LIRS_Code
                                                                                                                  == BkgOrderLineItemDetailStatus.INPROGRESS.GetStringValue()))
                                        {
                                            extVendorBkgOrderLineItemDetail.OLID_OrderLineItemResultStatusID = lkpOrderLineItemResultStatusList.FirstOrDefault(cnd => cnd.LIRS_Code
                                                                                                            == BkgOrderLineItemDetailStatus.INPROGRESS.GetStringValue()).LIRS_ID;
                                        }
                                        else if (!pkgSvcGroup.IsTransmitted && lkpOrderLineItemResultStatusList.Any(cond => cond.LIRS_Code
                                                                                                                  == BkgOrderLineItemDetailStatus.DRAFT.GetStringValue()))
                                        {
                                            extVendorBkgOrderLineItemDetail.OLID_OrderLineItemResultStatusID = lkpOrderLineItemResultStatusList.FirstOrDefault(cnd => cnd.LIRS_Code
                                                                                                            == BkgOrderLineItemDetailStatus.DRAFT.GetStringValue()).LIRS_ID;
                                        }
                                    }
                                    externalVendorBkgOrderDetail.ExtVendorBkgOrderLineItemDetails.Add(extVendorBkgOrderLineItemDetail);

                                    if (isNotificationRequestOn)
                                    {
                                        ExtSvcIntegrationRecord extSvcIntegrationRecord = new ExtSvcIntegrationRecord();
                                        extSvcIntegrationRecord.ESIR_BkgOrderID = evOrderContract.BkgOrderID;
                                        extSvcIntegrationRecord.ESIR_IsDeleted = false;
                                        extSvcIntegrationRecord.ESIR_CreatedByID = BkgOrderServiceUserID;
                                        extSvcIntegrationRecord.ESIR_CreatedOn = DateTime.Now;
                                        //extSvcIntegrationRecord.ESIR_ExtSvcIntegrationStatusID = 'Send to Vendor'?                            
                                        extSvcIntegrationRecord.ESIR_TenantID = tenantID;
                                        extSvcIntegrationRecord.ESIR_VendorOrderProfileID = pkgSvcGroup.BkgOrderVendorProfileID;
                                        extSvcIntegrationRecord.ESIR_VendorLineItemOrderID = item.ExternalVendorOrderID;
                                        extSvcIntegrationRecord.ESIR_BkgOrderLineItemID = item.BkgOrderPackageSvcLineItemID;
                                        ExtSvcIntegrationRecordToSave.Add(extSvcIntegrationRecord);
                                    }
                                }
                                else
                                {
                                    errorOrderItemIDs.Add(item.BkgOrderPackageSvcLineItemID);
                                }
                            }

                            extVendorReport.ExternalVendorBkgOrderDetails.Add(externalVendorBkgOrderDetail);
                            extVendorReports.Add(extVendorReport);
                        }
                        else
                        {
                            errorOrderItemIDs.AddRange(pkgSvcGroup.OrderItems.Select(col => col.BkgOrderPackageSvcLineItemID));
                        }
                    });

                //Update Service Group Status
                //if (!previousOrderStatusTypeCode.Equals(OrderStatusType.ADDITIONALWORK.GetStringValue()))
                //{
                //Update order Service group status.

                if (bkgOrder.BkgOrderPackages.IsNotNull() && bkgOrder.BkgOrderPackages.Count > AppConsts.NONE)
                {
                    List<Int32> _lstBOPIDs = bkgOrder.BkgOrderPackages.Select(slct => slct.BOP_ID).ToList();
                    List<BkgOrderPackageSvcGroup> _lstBkgOrderPAckageSvcGroup = BALUtils.GetEVOrderClientRepoInstance(tenantID).GetBkgOrderPkgSvcGroupByOrderID(_lstBOPIDs);

                    List<BkgOrderPackageSvcGroup> successfulSvcGroups = _lstBkgOrderPAckageSvcGroup.Where(cond => successfulSvcGroupIds.Contains(cond.OPSG_ID)).ToList();

                    //Set service group InProgress for Successful Svc Groups
                    UpdateServiceGroupStatus(tenantID, BkgSvcGrpStatusType.IN_PROGRESS.GetStringValue(), bkgOrder, successfulSvcGroups);

                    List<BkgOrderPackageSvcGroup> emptySvcGroups = _lstBkgOrderPAckageSvcGroup.Where(cond => cond.BkgOrderPackageSvcs.All(fx =>
                                                                                                            ((fx.BkgOrderPackageSvcLineItems.IsNull()) ||
                                                                                                            (fx.BkgOrderPackageSvcLineItems.IsNotNull() &&
                                                                                                            fx.BkgOrderPackageSvcLineItems.Count == 0)))).ToList();
                    //Set service group Completed for Empty Svc Groups
                    UpdateServiceGroupStatus(tenantID, BkgSvcGrpStatusType.COMPLETED.GetStringValue(), bkgOrder, emptySvcGroups);

                }
                //}

                //Save Tenant DB Data
                if (BALUtils.GetEVOrderClientRepoInstance(tenantID).SaveVendorOrderData(extVendorReports))
                {
                    if (isNotificationRequestOn)
                    {
                        BALUtils.GetEVOrderSecurityRepoInstance().SaveExtSvcIntegrationRecord(ExtSvcIntegrationRecordToSave);
                    }
                }

                //Update Retry count for error items
                if (errorOrderItemIDs.Count > AppConsts.NONE)
                {
                    BALUtils.GetEVOrderClientRepoInstance(tenantID).UpdateRetryCountForBkgOrderLineItems(errorOrderItemIDs);
                }

            }
            return true;
        }

        /// <summary>
        /// Returns a collection of Test Mode Background order IDs.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static List<Int32> GetTestModeBkgOrders(Int32 tenantID)
        {
            return BALUtils.GetEVOrderClientRepoInstance(tenantID).GetTestModeBkgOrders();
        }

        public static List<usp_GetOrdersToBeUpdatedVendorData_Result> GetOrdersToBeUpdatedVendorData(Int32 tenantID, Int32 chunkSize, Int32 startingOrderID,
                                                                                                     Int32 endingOrderID, Int32 recentDays, Int32 updateOrderRetryTimeLag)
        {
            try
            {
                return BALUtils.GetEVOrderClientRepoInstance(tenantID).GetOrdersToBeUpdatedVendorData(chunkSize, startingOrderID, endingOrderID,
                                                                                                      recentDays, updateOrderRetryTimeLag);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        private static Int32 GetBkgOrderStatusTypeID(String orderStatusTypeCode, Int32 tenantID)
        {
            Int32 orderStatusTypeID = AppConsts.NONE;
            try
            {
                List<lkpOrderStatusType> orderStatusTypeList = LookupManager.GetLookUpData<lkpOrderStatusType>(tenantID).ToList();
                if (orderStatusTypeList.IsNotNull())
                {
                    if (orderStatusTypeList.Any(cond => cond.Code == OrderStatusType.INPROGRESS.GetStringValue()))
                    {
                        orderStatusTypeID = orderStatusTypeList.Where(cond => cond.Code == orderStatusTypeCode).
                                                      FirstOrDefault().OrderStatusTypeID;
                    }
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return orderStatusTypeID;
        }

        //public static Boolean UpdateExternalVendorOrders(EvUpdateOrderContract evUpdateOrderContract,
        //                                                 IEnumerable<usp_GetOrdersToBeUpdatedVendorData_Result> processedOrderItemList, Int32 tenantID)

        private static String ParseHtmlToText(String externalVendorResponseHtmlString)
        {
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(externalVendorResponseHtmlString);

            String htmlNodesInnerText = htmlDoc.DocumentNode.InnerText;
            String parseText = Regex.Replace(htmlNodesInnerText, @"\t|\n|\r", "");
            parseText = parseText.RemoveExtraSpaces();

            return parseText;
        }
        public static Boolean UpdateExternalVendorOrders(EvUpdateOrderContract evUpdateOrderContract, Int32 tenantID)
        {
            try
            {
                //commented to make changes according to UAT-1244
                #region Old Implementation

                //if (!evUpdateOrderContract.VendorResponse.IsVendorError)
                //{
                //    List<lkpOrderStatusType> orderStatusTypeList = LookupManager.GetLookUpData<lkpOrderStatusType>(tenantID).ToList();

                //    List<lkpBkgSvcGrpReviewStatusType> BkgSvcGrpReviewStatusTypeList = LookupManager.GetLookUpData<lkpBkgSvcGrpReviewStatusType>(tenantID).Where(cond => cond.BSGRS_IsDeleted == false).ToList();
                //    List<lkpBkgSvcGrpStatusType> BkgSvcGrpStatusTypeList = LookupManager.GetLookUpData<lkpBkgSvcGrpStatusType>(tenantID).Where(cond => cond.BSGS_IsDeleted == false).ToList();
                //    String bkgSvcGroupCompletedStatusCode = BkgSvcGrpStatusType.COMPLETED.GetStringValue();
                //    String bkgSvcGroupCancelledStatusCode = BkgSvcGrpStatusType.CANCELLED.GetStringValue();

                //    //Update BkgOrder data.

                //    BkgOrder bkgOrder = BALUtils.GetEVOrderClientRepoInstance(tenantID).GetBkgOrder(evUpdateOrderContract.BkgOrderID);
                //    /*Commented below code related to UAt-844
                //    if (bkgOrder.IsNotNull())
                //    {
                //        bkgOrder.BOR_ModifiedByID = BkgOrderServiceUserID;
                //        bkgOrder.BOR_ModifiedOn = DateTime.Now;
                //        bkgOrder.BOR_FlaggedInd = evUpdateOrderContract.BkgOrderFlaggedInd;

                //        if (orderStatusTypeList.IsNotNull())
                //        {
                //            if (orderStatusTypeList.Any(cond => cond.Code == evUpdateOrderContract.OrderNewStatusTypeCode))
                //            {
                //                bkgOrder.BOR_OrderStatusTypeID = orderStatusTypeList.Where(cond => cond.Code == evUpdateOrderContract.OrderNewStatusTypeCode)
                //                                                 .FirstOrDefault().OrderStatusTypeID;
                //            }
                //        }

                //        if (evUpdateOrderContract.OrderNewStatusTypeCode == OrderStatusType.COMPLETED.GetStringValue())
                //        {
                //            bkgOrder.BOR_OrderCompleteDate = DateTime.Now;
                //        }
                //    }

                //    //Add Order Event History when Order Status changes. If order pervious status code and order new status is different then add 
                //    //new entry in Order Event History.
                //    if (evUpdateOrderContract.OrderNewStatusTypeCode != evUpdateOrderContract.OrderPreviousStatusTypeCode
                //        && !evUpdateOrderContract.OrderNewStatusTypeCode.IsNullOrEmpty())
                //    {
                //        BkgOrderEventHistory bkgOrderEventHistory = InsertBkgOrderEventHistory(tenantID, orderStatusTypeList, evUpdateOrderContract.OrderPreviousStatusTypeCode,
                //                                                                               evUpdateOrderContract.OrderNewStatusTypeCode);
                //        if (bkgOrderEventHistory.IsNotNull())
                //        {
                //            bkgOrder.BkgOrderEventHistories.Add(bkgOrderEventHistory);
                //        }
                //    }*/

                //    //This is required in case of Supplement Order. AS for Supplement Orders, Order status remains "Additional work",
                //    //UpdateOrder SP works st Order Profile level, then system should process each order profile at same time.
                //    //TODO:
                //    //if (processedOrderItemList.IsNotNull() && processedOrderItemList.Count() > AppConsts.NONE)
                //    //{
                //    //    processedOrderItemList.Select(col => col.ExternalVendorBkgOrderDetailID).Distinct().ForEach(processedExtVendorBkgOrderDetailID =>
                //    //    {
                //    //        ExternalVendorBkgOrderDetail processedExtVendorBkgOrderDetail = BALUtils.GetEVOrderClientRepoInstance(tenantID).
                //    //                                                                     GetExternalVendorBkgOrderDetail(processedExtVendorBkgOrderDetailID);
                //    //        processedExtVendorBkgOrderDetail.EVOD_LastUpdateRetryDate = DateTime.Now;
                //    //    });
                //    //}

                //    ExternalVendorBkgOrderDetail externalVendorBkgOrderDetail = BALUtils.GetEVOrderClientRepoInstance(tenantID).
                //                                                                GetExternalVendorBkgOrderDetail(evUpdateOrderContract.ExternalVendorBkgOrderDetailID);

                //    if (externalVendorBkgOrderDetail.IsNotNull())
                //    {
                //        externalVendorBkgOrderDetail.EVOD_LastUpdateRetryDate = DateTime.Now;
                //    }

                //    foreach (EvUpdateOrderItemContract item in evUpdateOrderContract.EvUpdateOrderItemContract)
                //    {
                //        ExtVendorBkgOrderLineItemDetail extVendorBkgOrderLineItemDetail = BALUtils.GetEVOrderClientRepoInstance(tenantID).
                //                                                                GetExtVendorBkgOrderLineItemDetail(item.ExtVendorBkgOrderLineItemDetailID);

                //        if (extVendorBkgOrderLineItemDetail.IsNotNull()
                //             && extVendorBkgOrderLineItemDetail.lkpOrderLineItemResultStatu.LIRS_Code != BkgOrderLineItemDetailStatus.COMPLETED.GetStringValue()
                //            )
                //        {
                //            extVendorBkgOrderLineItemDetail.OLID_ResultText = item.ResultText;
                //            extVendorBkgOrderLineItemDetail.OLID_ResultXML = item.ResultXML;
                //            if (item.DateCompleted.IsNotNull())
                //            {
                //                extVendorBkgOrderLineItemDetail.OLID_DateCompleted = item.DateCompleted;
                //            }
                //            if (item.OrderLineItemResultStatusID.IsNotNull() && item.OrderLineItemResultStatusID > AppConsts.NONE)
                //            {
                //                extVendorBkgOrderLineItemDetail.OLID_OrderLineItemResultStatusID = item.OrderLineItemResultStatusID;
                //            }
                //            extVendorBkgOrderLineItemDetail.OLID_FlaggedInd = item.SvcLineItemFlaggedInd;
                //            extVendorBkgOrderLineItemDetail.OLID_ModifiedByID = BkgOrderServiceUserID;
                //            extVendorBkgOrderLineItemDetail.OLID_ModifiedOn = DateTime.Now;
                //        }
                //    }

                //    //Update Background Order Package service group detail.                    
                //    //evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.Where(cnd => cnd.ServiceGroupNewReviewStatusCode != null
                //    //                                                             && cnd.ServiceGroupNewReviewStatusCode != String.Empty)
                //    //                                                            .ForEach(item =>
                //    if (evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode != null && evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode != String.Empty)
                //    {
                //        BkgOrderPackageSvcGroup bkgOrderPackageSvcGroupDetail = BALUtils.GetEVOrderClientRepoInstance(tenantID)
                //                                                                        .GetBkgOrderPkgSvcGroupDetail(evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgOrderPackageServiceGroupID);
                //        if (bkgOrderPackageSvcGroupDetail.IsNotNull())
                //        {
                //            if (evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupStatusCode != BkgSvcGrpStatusType.COMPLETED.GetStringValue())
                //            {
                //                bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpLastUpdatedDate = DateTime.Now;
                //                bkgOrderPackageSvcGroupDetail.OPSG_ModifiedByID = BkgOrderServiceUserID;
                //                bkgOrderPackageSvcGroupDetail.OPSG_ModifiedOn = DateTime.Now;
                //                bkgOrderPackageSvcGroupDetail.OPSG_FlaggedInd = evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupFlaggedInd;

                //                if (BkgSvcGrpReviewStatusTypeList.IsNotNull()
                //                    && BkgSvcGrpReviewStatusTypeList.Any(cond => cond.BSGRS_ReviewCode == evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode))
                //                {
                //                    bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpReviewStatusTypeID = BkgSvcGrpReviewStatusTypeList
                //                                                                                   .Where(cond => cond.BSGRS_ReviewCode == evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode)
                //                                                                                   .FirstOrDefault().BSGRS_ID;
                //                }

                //                if (evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode == BkgSvcGrpReviewStatusType.AUTO_REVIEW_COMPLETED.GetStringValue())
                //                {
                //                    if (BkgSvcGrpStatusTypeList.IsNotNull()
                //                                                       && BkgSvcGrpStatusTypeList.Any(cond => cond.BSGS_StatusCode == bkgSvcGroupCompletedStatusCode))
                //                    {
                //                        bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpStatusTypeID = BkgSvcGrpStatusTypeList
                //                                                                                      .Where(cond => cond.BSGS_StatusCode == bkgSvcGroupCompletedStatusCode)
                //                                                                                      .FirstOrDefault().BSGS_ID;
                //                    }
                //                    bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpCompletionDate = DateTime.Now;
                //                }
                //            }
                //        }
                //        //Insert in 'bkgOrderServiceGroupEventHistory' for each service group detail updation.
                //        if (evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupPreviousReviewStatusCode != evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode)
                //        {
                //            BkgOrderEventHistory bkgOrderServiceGroupEventHistory = InsertBkgOrderServiceGroupEventHistory(tenantID, BkgSvcGrpReviewStatusTypeList,
                //                                                                                                           evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupPreviousReviewStatusCode,
                //                                                                                                           evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode,
                //                                                                                                           evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgSvcGroupName);
                //            if (bkgOrderServiceGroupEventHistory.IsNotNull())
                //            {
                //                bkgOrder.BkgOrderEventHistories.Add(bkgOrderServiceGroupEventHistory);
                //            }
                //        }
                //    }
                //    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
                //    BALUtils.GetEVOrderClientRepoInstance(tenantID).UpdateVendorData();
                //    BALUtils.GetEVOrderClientRepoInstance(tenantID).CompleteOrderServiceForms(evUpdateOrderContract.BkgOrderID);
                //    CopyData(evUpdateOrderContract, tenantID);
                //    return true;
                //}
                //else
                //{
                //    ExternalVendorBkgOrderDetail externalVendorBkgOrderDetail = BALUtils.GetEVOrderClientRepoInstance(tenantID).
                //                                                               GetExternalVendorBkgOrderDetail(evUpdateOrderContract.ExternalVendorBkgOrderDetailID);

                //    if (externalVendorBkgOrderDetail.IsNotNull())
                //    {
                //        externalVendorBkgOrderDetail.EVOD_LastUpdateRetryDate = DateTime.Now;
                //    }
                //    BALUtils.GetEVOrderClientRepoInstance(tenantID).UpdateVendorData();
                //    return true;
                //}

                #endregion

                //New implementation done as per UAT-1244 requirement
                //No change is required if Clear Star profile is not completed
                //Service Group Status will be set to cancelled if profile is cancelled
                #region New Implementation

                if (!evUpdateOrderContract.VendorResponse.IsVendorError)
                {
                    List<lkpOrderStatusType> orderStatusTypeList = LookupManager.GetLookUpData<lkpOrderStatusType>(tenantID).ToList();

                    List<lkpBkgSvcGrpReviewStatusType> BkgSvcGrpReviewStatusTypeList = LookupManager.GetLookUpData<lkpBkgSvcGrpReviewStatusType>(tenantID).Where(cond => cond.BSGRS_IsDeleted == false).ToList();
                    List<lkpBkgSvcGrpStatusType> BkgSvcGrpStatusTypeList = LookupManager.GetLookUpData<lkpBkgSvcGrpStatusType>(tenantID).Where(cond => cond.BSGS_IsDeleted == false).ToList();
                    String bkgSvcGroupCompletedStatusCode = BkgSvcGrpStatusType.COMPLETED.GetStringValue();
                    String bkgSvcGroupCancelledStatusCode = BkgSvcGrpStatusType.CANCELLED.GetStringValue();
                    String bkgSvcGroupInProgressStatusCode = BkgSvcGrpStatusType.IN_PROGRESS.GetStringValue();

                    List<lkpBkgResultResponseFormat> BkgResultResponseFormat = LookupManager.GetLookUpData<lkpBkgResultResponseFormat>().Where(cond => cond.BRRF_IsDeleted == false).ToList();
                    String bkgExtVendorResultResponseClearStarHtmlCode = BkgExternalVendorResultResponseFormat.ClearStarHtml.GetStringValue();
                    Int32? resultResponseFormatTypeId = null;
                    //Update BkgOrder data.
                    List<lkpOrderLineItemResultStatu> lkpOrderLineItemResultStatusList = LookupManager.GetLookUpData<lkpOrderLineItemResultStatu>(tenantID).ToList();

                    BkgOrder bkgOrder = BALUtils.GetEVOrderClientRepoInstance(tenantID).GetBkgOrder(evUpdateOrderContract.BkgOrderID);
                    ExternalVendorBkgOrderDetail externalVendorBkgOrderDetail = BALUtils.GetEVOrderClientRepoInstance(tenantID).
                                                                                GetExternalVendorBkgOrderDetail(evUpdateOrderContract.ExternalVendorBkgOrderDetailID);
                    if (externalVendorBkgOrderDetail.IsNotNull())
                    {
                        externalVendorBkgOrderDetail.EVOD_LastUpdateRetryDate = DateTime.Now;
                    }

                    //if (evUpdateOrderContract.VendorProfileStatus == ClearstarProfileStatus.Completed)       //UAT-4377// Changes required here
                    if (evUpdateOrderContract.VendorProfileStatus == ClearstarProfileStatus.Completed
                        || evUpdateOrderContract.VendorProfileStatus == ClearstarProfileStatus.Draft || evUpdateOrderContract.VendorProfileStatus == ClearstarProfileStatus.InProgress) //Added in UAT-4377
                    {
                        foreach (EvUpdateOrderItemContract item in evUpdateOrderContract.EvUpdateOrderItemContract)
                        {
                            //if ((item.OrderLineItemResultStatusID.IsNotNull() &&
                            //    item.OrderLineItemResultStatusID == lkpOrderLineItemResultStatusList.FirstOrDefault(x => x.LIRS_Code == BkgOrderLineItemDetailStatus.COMPLETED.GetStringValue()).LIRS_ID)
                            //   || evUpdateOrderContract.VendorProfileStatus == ClearstarProfileStatus.Completed) //Added in UAT-4377
                            // {

                            ExtVendorBkgOrderLineItemDetail extVendorBkgOrderLineItemDetail = BALUtils.GetEVOrderClientRepoInstance(tenantID).
                                                                                    GetExtVendorBkgOrderLineItemDetail(item.ExtVendorBkgOrderLineItemDetailID);

                            if (extVendorBkgOrderLineItemDetail.IsNotNull()
                                 && extVendorBkgOrderLineItemDetail.lkpOrderLineItemResultStatu.LIRS_Code != BkgOrderLineItemDetailStatus.COMPLETED.GetStringValue())
                            {
                                //Start code changes for UAT 3331.

                                resultResponseFormatTypeId = BALUtils.GetEVOrderClientRepoInstance(tenantID).GetExternalVendorResultResponseFormatTypeId(extVendorBkgOrderLineItemDetail.OLID_ID);

                                if (resultResponseFormatTypeId.IsNotNull() && BkgResultResponseFormat.Any(x => x.BRRF_ID == resultResponseFormatTypeId && x.BRRF_Code == bkgExtVendorResultResponseClearStarHtmlCode))
                                {
                                    String parseText = ParseHtmlToText(item.ResultText);
                                    extVendorBkgOrderLineItemDetail.OLID_ResultText = parseText;
                                    extVendorBkgOrderLineItemDetail.OLID_OriginalResultText = item.ResultText;
                                }
                                else
                                {
                                    extVendorBkgOrderLineItemDetail.OLID_ResultText = item.ResultText;
                                }
                                // End code changes for UAT 3331.
                                extVendorBkgOrderLineItemDetail.OLID_ResultXML = item.ResultXML;

                                if (item.DateCompleted.IsNotNull())
                                {
                                    extVendorBkgOrderLineItemDetail.OLID_DateCompleted = item.DateCompleted;
                                }
                                if (item.OrderLineItemResultStatusID.IsNotNull() && item.OrderLineItemResultStatusID > AppConsts.NONE)
                                {
                                    // UAT 3742: Pull in updates to archived screening orders if update is sent from ClearStar
                                    if (lkpOrderLineItemResultStatusList.IsNotNull() && lkpOrderLineItemResultStatusList.Count > 0 && lkpOrderLineItemResultStatusList.FirstOrDefault(x => x.LIRS_Code == BkgOrderLineItemDetailStatus.ARCHIVED.GetStringValue()).LIRS_ID == item.OrderLineItemResultStatusID)
                                    {
                                        extVendorBkgOrderLineItemDetail.OLID_OrderLineItemResultStatusID = lkpOrderLineItemResultStatusList.FirstOrDefault(x => x.LIRS_Code == BkgOrderLineItemDetailStatus.COMPLETED.GetStringValue()).LIRS_ID;
                                    }
                                    else
                                    {
                                        extVendorBkgOrderLineItemDetail.OLID_OrderLineItemResultStatusID = item.OrderLineItemResultStatusID;
                                    }

                                    #region Admin Entry Portal

                                     Int32 lineItemStatusID_Completed = lkpOrderLineItemResultStatusList.FirstOrDefault(x => x.LIRS_Code == BkgOrderLineItemDetailStatus.COMPLETED.GetStringValue()).LIRS_ID;

                                    if (extVendorBkgOrderLineItemDetail.IsNotNull()
                                        && extVendorBkgOrderLineItemDetail.OLID_OrderLineItemResultStatusID == lineItemStatusID_Completed
                                        && IsAdminEntryPortalOrder(bkgOrder))
                                    {
                                        Int16 lineItemAdminEntryStatusID_CompleteORCompleteFlagged = AppConsts.NONE;
                                        String lineItemAdminEntryStatusCode_Complete_Flagged = AdminEntryOrderLineItemStatus.COMPLETE_FLAGGED.GetStringValue();
                                        String lineItemAdminEntryStatusCode_Complete= AdminEntryOrderLineItemStatus.COMPLETE.GetStringValue();

                                        List <lkpAdminEntryOrderLineItemStatu> lineItemAdminEntryStatusList = LookupManager.GetLookUpData<lkpAdminEntryOrderLineItemStatu>(tenantID)
                                                                                                                          .Where(cond => cond.AEOLIS_IsDeleted == false).ToList();
                                        if (item.SvcLineItemFlaggedInd)
                                        {
                                            lineItemAdminEntryStatusID_CompleteORCompleteFlagged = lineItemAdminEntryStatusList.FirstOrDefault(cnd => cnd.AEOLIS_Code == lineItemAdminEntryStatusCode_Complete_Flagged).AEOLIS_ID;
                                        }
                                        else
                                        {
                                            lineItemAdminEntryStatusID_CompleteORCompleteFlagged = lineItemAdminEntryStatusList.FirstOrDefault(cnd => cnd.AEOLIS_Code == lineItemAdminEntryStatusCode_Complete).AEOLIS_ID;
                                        }

                                        extVendorBkgOrderLineItemDetail.BkgOrderPackageSvcLineItem.PSLI_AdminEntryLineItemStatusID = lineItemAdminEntryStatusID_CompleteORCompleteFlagged;
                                        extVendorBkgOrderLineItemDetail.BkgOrderPackageSvcLineItem.PSLI_ModifiedByID = BkgOrderServiceUserID;
                                        extVendorBkgOrderLineItemDetail.BkgOrderPackageSvcLineItem.PSLI_ModifiedOn = DateTime.Now;
                                    }

                                    #endregion 
                                }
                                extVendorBkgOrderLineItemDetail.OLID_FlaggedInd = item.SvcLineItemFlaggedInd;
                                extVendorBkgOrderLineItemDetail.OLID_ModifiedByID = BkgOrderServiceUserID;
                                extVendorBkgOrderLineItemDetail.OLID_ModifiedOn = DateTime.Now;
                            }
                            // }
                        }

                        if (evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode != null && evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode != String.Empty)
                        {
                            BkgOrderPackageSvcGroup bkgOrderPackageSvcGroupDetail = BALUtils.GetEVOrderClientRepoInstance(tenantID)
                                                                                            .GetBkgOrderPkgSvcGroupDetail(evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgOrderPackageServiceGroupID);
                            if (bkgOrderPackageSvcGroupDetail.IsNotNull())
                            {
                                if (evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupStatusCode != BkgSvcGrpStatusType.COMPLETED.GetStringValue())
                                {
                                    bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpLastUpdatedDate = DateTime.Now;
                                    bkgOrderPackageSvcGroupDetail.OPSG_ModifiedByID = BkgOrderServiceUserID;
                                    bkgOrderPackageSvcGroupDetail.OPSG_ModifiedOn = DateTime.Now;
                                    bkgOrderPackageSvcGroupDetail.OPSG_FlaggedInd = evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupFlaggedInd;

                                    if (BkgSvcGrpReviewStatusTypeList.IsNotNull()
                                        && BkgSvcGrpReviewStatusTypeList.Any(cond => cond.BSGRS_ReviewCode == evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode))
                                    {
                                        bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpReviewStatusTypeID = BkgSvcGrpReviewStatusTypeList
                                                                                                        .Where(cond => cond.BSGRS_ReviewCode == evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode)
                                                                                                       .FirstOrDefault().BSGRS_ID;
                                    }



                                    if (evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode == BkgSvcGrpReviewStatusType.AUTO_REVIEW_COMPLETED.GetStringValue())
                                    {
                                        if (BkgSvcGrpStatusTypeList.IsNotNull()
                                                                           && BkgSvcGrpStatusTypeList.Any(cond => cond.BSGS_StatusCode == bkgSvcGroupCompletedStatusCode))
                                        {
                                            bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpStatusTypeID = BkgSvcGrpStatusTypeList
                                                                                                          .Where(cond => cond.BSGS_StatusCode == bkgSvcGroupCompletedStatusCode)
                                                                                                          .FirstOrDefault().BSGS_ID;
                                        }
                                        bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpCompletionDate = DateTime.Now;
                                    }

                                }
                            }

                            //Insert in 'bkgOrderServiceGroupEventHistory' for each service group detail updation.
                            if (evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupPreviousReviewStatusCode != evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode)
                            {
                                BkgOrderEventHistory bkgOrderServiceGroupEventHistory = InsertBkgOrderServiceGroupEventHistory(tenantID, BkgSvcGrpReviewStatusTypeList,
                                                                                                                               evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupPreviousReviewStatusCode,
                                                                                                                               evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode,
                                                                                                                               evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgSvcGroupName);
                                if (bkgOrderServiceGroupEventHistory.IsNotNull())
                                {
                                    bkgOrder.BkgOrderEventHistories.Add(bkgOrderServiceGroupEventHistory);
                                }
                            }
                        }
                        BALUtils.GetEVOrderClientRepoInstance(tenantID).UpdateVendorData();
                        BALUtils.GetEVOrderClientRepoInstance(tenantID).CompleteOrderServiceForms(evUpdateOrderContract.BkgOrderID);
                        CopyData(evUpdateOrderContract, tenantID);
                        //UAT-1852 : If a Service Group is complete, but other service groups are not complete within an order, the system should send the applicant an email.
                        //if (!evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.IsNullOrEmpty())
                        //{
                        //    DataTable table = BALUtils.GetEVOrderClientRepoInstance(tenantID).GetserviceGroupDetailsForMail(evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgOrderPackageServiceGroupID, evUpdateOrderContract.BkgOrderID);

                        //   List<EvServiceGroupMailContract> SvcDetails = ConvertDataTableToContrat(table);
                        //    if (SvcDetails.IsNotNull() && SvcDetails.Count > 0 && SvcDetails.Any(any=>!any.IsCompleted))
                        //        SendSeriveGroupIncompleteStatusMail(SvcDetails, tenantID);
                        //}
                    }
                    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    else if (evUpdateOrderContract.VendorProfileStatus == ClearstarProfileStatus.Cancelled)
                    {
                        //BkgOrderPackageSvcGroup bkgOrderPackageSvcGroupDetail = BALUtils.GetEVOrderClientRepoInstance(tenantID)
                        //                                                                    .GetBkgOrderPkgSvcGroupDetail(evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgOrderPackageServiceGroupID);
                        //if (bkgOrderPackageSvcGroupDetail.IsNotNull())
                        //{
                        //    bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpStatusTypeID = BkgSvcGrpStatusTypeList
                        //                                                                                  .Where(cond => cond.BSGS_StatusCode == bkgSvcGroupCancelledStatusCode)
                        //                                                                                  .FirstOrDefault().BSGS_ID;
                        //    bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpLastUpdatedDate = DateTime.Now;
                        //    bkgOrderPackageSvcGroupDetail.OPSG_ModifiedByID = BkgOrderServiceUserID;
                        //    bkgOrderPackageSvcGroupDetail.OPSG_ModifiedOn = DateTime.Now;
                        //}
                        //UAT-2531
                        BALUtils.GetEVOrderClientRepoInstance(tenantID).UpdateVendorData();
                    }
                    //else if (evUpdateOrderContract.VendorProfileStatus == ClearstarProfileStatus.Archived)
                    //{
                    //    UpdateExtVendorOrderAsCompleted(evUpdateOrderContract, tenantID, bkgOrder, BkgSvcGrpReviewStatusTypeList, BkgSvcGrpStatusTypeList);
                    //}
                    else
                    {
                        BALUtils.GetEVOrderClientRepoInstance(tenantID).UpdateVendorData();
                    }
                    //return true;

                    //UAT-2370:Supplement SSN Processing updates
                    if (!evUpdateOrderContract.EvUpdateOrderItemContract.IsNullOrEmpty())
                    {
                        List<Int32> lstIds = evUpdateOrderContract.EvUpdateOrderItemContract.Select(x => x.ExtVendorBkgOrderLineItemDetailID).ToList();
                        String vendorBkgOrderLineItemDetailIDs = String.Join(",", lstIds);
                        Int32 BkgOrderID = evUpdateOrderContract.BkgOrderID;
                        BackgroundProcessOrderManager.SendEmailWhenExceptionInSSNResult(tenantID, BkgOrderServiceUserID, BkgOrderID, vendorBkgOrderLineItemDetailIDs);
                    }
                }
                else
                {
                    ExternalVendorBkgOrderDetail externalVendorBkgOrderDetail = BALUtils.GetEVOrderClientRepoInstance(tenantID).
                                                                               GetExternalVendorBkgOrderDetail(evUpdateOrderContract.ExternalVendorBkgOrderDetailID);

                    if (externalVendorBkgOrderDetail.IsNotNull())
                    {
                        externalVendorBkgOrderDetail.EVOD_LastUpdateRetryDate = DateTime.Now;
                    }
                    BALUtils.GetEVOrderClientRepoInstance(tenantID).UpdateVendorData();
                    //return true;
                }

                //UAT-2303: Supplement automation.
                BkgOrder bkgOrderTemp = BALUtils.GetEVOrderClientRepoInstance(tenantID).GetBkgOrder(evUpdateOrderContract.BkgOrderID);
                if (bkgOrderTemp.IsNotNull() && bkgOrderTemp.lkpOrderStatusType.IsNotNull() && bkgOrderTemp.lkpOrderStatusType.Code != OrderStatusType.COMPLETED.GetStringValue())
                {
                    UpdateSupplementOrderAutomatically(tenantID, bkgOrderTemp.BOR_MasterOrderID, bkgOrderTemp.BOR_ID
                                                       , evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgOrderPackageServiceGroupID); //UAT-2399
                }

                return true;

                #endregion
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }
        private static void UpdateExtVendorOrderAsCompleted(EvUpdateOrderContract evUpdateOrderContract, Int32 tenantID, BkgOrder bkgOrder, List<lkpBkgSvcGrpReviewStatusType> BkgSvcGrpReviewStatusTypeList, List<lkpBkgSvcGrpStatusType> BkgSvcGrpStatusTypeList)
        {
            List<lkpOrderStatusType> orderStatusTypeList = LookupManager.GetLookUpData<lkpOrderStatusType>(tenantID).ToList();

            String bkgSvcGroupCompletedStatusCode = BkgSvcGrpStatusType.COMPLETED.GetStringValue();

            List<lkpBkgResultResponseFormat> BkgResultResponseFormat = LookupManager.GetLookUpData<lkpBkgResultResponseFormat>().Where(cond => cond.BRRF_IsDeleted == false).ToList();
            String bkgExtVendorResultResponseClearStarHtmlCode = BkgExternalVendorResultResponseFormat.ClearStarHtml.GetStringValue();
            Int32? resultResponseFormatTypeId = null;
            //Update BkgOrder data.
            List<lkpOrderLineItemResultStatu> lkpOrderLineItemResultStatusList = LookupManager.GetLookUpData<lkpOrderLineItemResultStatu>(tenantID).ToList();
            foreach (EvUpdateOrderItemContract item in evUpdateOrderContract.EvUpdateOrderItemContract)
            {
                ExtVendorBkgOrderLineItemDetail extVendorBkgOrderLineItemDetail = BALUtils.GetEVOrderClientRepoInstance(tenantID).
                                                                        GetExtVendorBkgOrderLineItemDetail(item.ExtVendorBkgOrderLineItemDetailID);
                if (extVendorBkgOrderLineItemDetail.IsNotNull()
                     && extVendorBkgOrderLineItemDetail.lkpOrderLineItemResultStatu.LIRS_Code != BkgOrderLineItemDetailStatus.COMPLETED.GetStringValue())
                {
                    //Start code changes for UAT 3331.
                    resultResponseFormatTypeId = BALUtils.GetEVOrderClientRepoInstance(tenantID).GetExternalVendorResultResponseFormatTypeId(extVendorBkgOrderLineItemDetail.OLID_ID);

                    if (resultResponseFormatTypeId.IsNotNull() && BkgResultResponseFormat.Any(x => x.BRRF_ID == resultResponseFormatTypeId && x.BRRF_Code == bkgExtVendorResultResponseClearStarHtmlCode))
                    {
                        if (!item.ResultText.IsNullOrEmpty())
                        {
                            String parseText = ParseHtmlToText(item.ResultText);
                            extVendorBkgOrderLineItemDetail.OLID_ResultText = parseText;
                            extVendorBkgOrderLineItemDetail.OLID_OriginalResultText = item.ResultText;
                        }
                    }
                    else
                    {
                        extVendorBkgOrderLineItemDetail.OLID_ResultText = item.ResultText;
                    }
                    // End code changes for UAT 3331.
                    extVendorBkgOrderLineItemDetail.OLID_ResultXML = item.ResultXML;


                    if (item.DateCompleted.IsNotNull())
                    {
                        extVendorBkgOrderLineItemDetail.OLID_DateCompleted = item.DateCompleted;
                    }
                    else
                    {
                        extVendorBkgOrderLineItemDetail.OLID_DateCompleted = DateTime.Now;
                    }
                    if (item.OrderLineItemResultStatusID.IsNotNull() && item.OrderLineItemResultStatusID > AppConsts.NONE)
                    {
                        extVendorBkgOrderLineItemDetail.OLID_OrderLineItemResultStatusID = item.OrderLineItemResultStatusID;
                    }
                    else
                    {
                        extVendorBkgOrderLineItemDetail.OLID_OrderLineItemResultStatusID = lkpOrderLineItemResultStatusList.FirstOrDefault(x => x.LIRS_Code == BkgOrderLineItemDetailStatus.COMPLETED.GetStringValue()).LIRS_ID;
                    }

                    //if (item.OrderLineItemResultStatusID.IsNotNull() && item.OrderLineItemResultStatusID == AppConsts.NONE)
                    //{
                    //    extVendorBkgOrderLineItemDetail.OLID_OrderLineItemResultStatusID = lkpOrderLineItemResultStatusList.FirstOrDefault(x => x.LIRS_Code == BkgOrderLineItemDetailStatus.ARCHIVED.GetStringValue()).LIRS_ID;
                    //}

                    extVendorBkgOrderLineItemDetail.OLID_FlaggedInd = item.SvcLineItemFlaggedInd;
                    extVendorBkgOrderLineItemDetail.OLID_ModifiedByID = BkgOrderServiceUserID;
                    extVendorBkgOrderLineItemDetail.OLID_ModifiedOn = DateTime.Now;
                }
            }

            if (evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode != null && evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode != String.Empty)
            {
                BkgOrderPackageSvcGroup bkgOrderPackageSvcGroupDetail = BALUtils.GetEVOrderClientRepoInstance(tenantID)
                                                                                .GetBkgOrderPkgSvcGroupDetail(evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgOrderPackageServiceGroupID);
                if (bkgOrderPackageSvcGroupDetail.IsNotNull())
                {
                    if (evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupStatusCode != BkgSvcGrpStatusType.COMPLETED.GetStringValue())
                    {
                        bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpLastUpdatedDate = DateTime.Now;
                        bkgOrderPackageSvcGroupDetail.OPSG_ModifiedByID = BkgOrderServiceUserID;
                        bkgOrderPackageSvcGroupDetail.OPSG_ModifiedOn = DateTime.Now;
                        bkgOrderPackageSvcGroupDetail.OPSG_FlaggedInd = evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupFlaggedInd;

                        if (BkgSvcGrpReviewStatusTypeList.IsNotNull()
                            && BkgSvcGrpReviewStatusTypeList.Any(cond => cond.BSGRS_ReviewCode == evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode))
                        {
                            bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpReviewStatusTypeID = BkgSvcGrpReviewStatusTypeList
                                                                                            .Where(cond => cond.BSGRS_ReviewCode == evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode)
                                                                                           .FirstOrDefault().BSGRS_ID;
                        }

                        if (evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode == BkgSvcGrpReviewStatusType.AUTO_REVIEW_COMPLETED.GetStringValue())
                        {
                            if (BkgSvcGrpStatusTypeList.IsNotNull()
                                                               && BkgSvcGrpStatusTypeList.Any(cond => cond.BSGS_StatusCode == bkgSvcGroupCompletedStatusCode))
                            {
                                bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpStatusTypeID = BkgSvcGrpStatusTypeList
                                                                                              .Where(cond => cond.BSGS_StatusCode == bkgSvcGroupCompletedStatusCode)
                                                                                              .FirstOrDefault().BSGS_ID;
                            }
                            bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpCompletionDate = DateTime.Now;
                        }
                    }
                }

                //Insert in 'bkgOrderServiceGroupEventHistory' for each service group detail updation.
                if (evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupPreviousReviewStatusCode != evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode)
                {
                    BkgOrderEventHistory bkgOrderServiceGroupEventHistory = InsertBkgOrderServiceGroupEventHistory(tenantID, BkgSvcGrpReviewStatusTypeList,
                                                                                                                   evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupPreviousReviewStatusCode,
                                                                                                                   evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode,
                                                                                                                   evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgSvcGroupName);
                    if (bkgOrderServiceGroupEventHistory.IsNotNull())
                    {
                        bkgOrder.BkgOrderEventHistories.Add(bkgOrderServiceGroupEventHistory);
                    }
                }
            }
            else
            {
                BkgOrderPackageSvcGroup bkgOrderPackageSvcGroupDetail = BALUtils.GetEVOrderClientRepoInstance(tenantID)
                                                                                .GetBkgOrderPkgSvcGroupDetail(evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgOrderPackageServiceGroupID);
                if (bkgOrderPackageSvcGroupDetail.IsNotNull())
                {
                    if (evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupStatusCode != BkgSvcGrpStatusType.COMPLETED.GetStringValue())
                    {
                        bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpLastUpdatedDate = DateTime.Now;
                        bkgOrderPackageSvcGroupDetail.OPSG_ModifiedByID = BkgOrderServiceUserID;
                        bkgOrderPackageSvcGroupDetail.OPSG_ModifiedOn = DateTime.Now;
                        bkgOrderPackageSvcGroupDetail.OPSG_FlaggedInd = evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupFlaggedInd;

                        if (BkgSvcGrpReviewStatusTypeList.IsNotNull() && !evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode.IsNullOrEmpty()
                            && BkgSvcGrpReviewStatusTypeList.Any(cond => cond.BSGRS_ReviewCode == evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode))
                        {
                            bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpReviewStatusTypeID = BkgSvcGrpReviewStatusTypeList
                                                                                            .Where(cond => cond.BSGRS_ReviewCode == evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode)
                                                                                           .FirstOrDefault().BSGRS_ID;
                        }
                        else
                        {
                            String autoReviewCode = BkgSvcGrpReviewStatusType.AUTO_REVIEW_COMPLETED.GetStringValue();
                            bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpReviewStatusTypeID = BkgSvcGrpReviewStatusTypeList
                                                                                            .Where(cond => cond.BSGRS_ReviewCode == autoReviewCode)
                                                                                           .FirstOrDefault().BSGRS_ID;
                        }

                        if (!evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode.IsNullOrEmpty() && evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode == BkgSvcGrpReviewStatusType.AUTO_REVIEW_COMPLETED.GetStringValue())
                        {
                            if (BkgSvcGrpStatusTypeList.IsNotNull()
                                                               && BkgSvcGrpStatusTypeList.Any(cond => cond.BSGS_StatusCode == bkgSvcGroupCompletedStatusCode))
                            {
                                bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpStatusTypeID = BkgSvcGrpStatusTypeList
                                                                                              .Where(cond => cond.BSGS_StatusCode == bkgSvcGroupCompletedStatusCode)
                                                                                              .FirstOrDefault().BSGS_ID;
                            }
                            bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpCompletionDate = DateTime.Now;
                        }
                        else
                        {
                            if (BkgSvcGrpStatusTypeList.IsNotNull()
                                                                  && BkgSvcGrpStatusTypeList.Any(cond => cond.BSGS_StatusCode == bkgSvcGroupCompletedStatusCode))
                            {
                                bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpStatusTypeID = BkgSvcGrpStatusTypeList
                                                                                              .Where(cond => cond.BSGS_StatusCode == bkgSvcGroupCompletedStatusCode)
                                                                                              .FirstOrDefault().BSGS_ID;
                            }
                            bkgOrderPackageSvcGroupDetail.OPSG_SvcGrpCompletionDate = DateTime.Now;
                        }
                    }
                }
                //Insert in 'bkgOrderServiceGroupEventHistory' for each service group detail updation.
                if (!evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode.IsNullOrEmpty() && evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupPreviousReviewStatusCode != evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode)
                {
                    BkgOrderEventHistory bkgOrderServiceGroupEventHistory = InsertBkgOrderServiceGroupEventHistory(tenantID, BkgSvcGrpReviewStatusTypeList,
                                                                                                                   evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupPreviousReviewStatusCode,
                                                                                                                   evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupNewReviewStatusCode,
                                                                                                                   evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgSvcGroupName);
                    if (bkgOrderServiceGroupEventHistory.IsNotNull())
                    {
                        bkgOrder.BkgOrderEventHistories.Add(bkgOrderServiceGroupEventHistory);
                    }
                }
            }
            BALUtils.GetEVOrderClientRepoInstance(tenantID).UpdateVendorData();
            BALUtils.GetEVOrderClientRepoInstance(tenantID).CompleteOrderServiceForms(evUpdateOrderContract.BkgOrderID);
            CopyData(evUpdateOrderContract, tenantID);
        }
        public static IEnumerable<OrderAttribute> GetAttributesForOrder(Int32 orderID, Int32 tenantID)
        {
            try
            {
                //  Collection of attributes (of Residential History, Personal Alias, Personal Info, Custom Attribute Groups) for order.
                //  OrderAttribute represents a Custom Table of the form
                //  AttributeGroupID | InstanceID  |   AttributeGroupMappingID   |   AttributeValue
                //        1          |     1       |           10                |   Gautam
                //        1          |     2       |           11                |   1
                //        1          |     3       |           12                |   0
                //        2          |     1       |           33                |   CO
                //        2          |     2       |           34                |   Denver

                return BALUtils.GetEVOrderClientRepoInstance(tenantID).GetAttributesForOrder(orderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return null;
        }

        public static String GetCCFDocument(Int32 organizationUserProfileID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetEVOrderClientRepoInstance(tenantID).GetCCFDocument(organizationUserProfileID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return String.Empty;
        }

        /// <summary>
        /// To check if any items are pending of a external vendor order service group
        /// </summary>
        /// <param name="organizationUserProfileID"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static Boolean CheckPendingSvcGroupItems(Int32 tenantID, Int32 externalVendorOrderDetailID)
        {
            try
            {
                return BALUtils.GetEVOrderClientRepoInstance(tenantID).CheckPendingSvcGroupItems(externalVendorOrderDetailID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }

            return false;
        }

        public static void PopulateOrderWithAttributes(EvCreateOrderSvcGroupContract svcGroupContract, Int32 tenantID, List<OrderAttribute> orderAttributes)
        {
            try
            {
                //Collection of all the background service ids corresponding to the Service Group.
                IEnumerable<Int32> backgroundServicesForOrder = svcGroupContract.OrderItems.Select(col => col.BackgroundServiceID);

                //Collection of all the external service ids corresponding to the Service Group.
                IEnumerable<Int32> externalServicesForOrder = svcGroupContract.OrderItems.Select(col => col.ExternalServiceID);

                //Collection of all the bkgOrderPackageSvcLineItem ids corresponding to the Service Group.
                IEnumerable<Int32> bkgOrderPackageSvcLineItemsForOrder = svcGroupContract.OrderItems.Select(col => col.BkgOrderPackageSvcLineItemID);

                //Collection of all BkgOrderLineItemDataMappings (containing InstanceID,BkgSvcAttributeGroupID) for Service Group.
                List<BkgOrderLineItemDataMapping> bkgOrderLineItemDataMappings = BALUtils.GetEVOrderClientRepoInstance(tenantID)
                                                                                                .GetBkgOrderLineItemDataMappings(bkgOrderPackageSvcLineItemsForOrder).ToList();

                //Collection of all vw_BkgExtSvcAttributeMapping (containing combination of External and Background Services with their attribute mapping) for Service Group.

                /*
                 ********IMPORTANT NOTES****************************
                 * Whenever a developer update / modified SECURITY vw_BkgExtSvcAttributeMapping VIEW. Then please make sure while updating the view in Entity
                  you should make UniqueID (column returened from View) TRUE as Entity Key. 
                 * You can do this by Open the edmx file -> Go to Model Browser -> Expand Entity Types-> Locate vw_BkgExtSvcAttributeMapping VIEW
                 * -> Right click on view and click properties-> Set the Entity Key as TRUE.
                 * 
                 * This is mandatory else Vendor Order Service won't work as expected and result in error for those services having composite attributes mapping.
                 * 
                 */

                IEnumerable<vw_BkgExtSvcAttributeMapping> bkgExtSvcAttributeMappingForOrder = BALUtils.GetEVOrderSecurityRepoInstance().GetBkgExtSvcAttributeMappings()
                                                                                        .Where(cond =>
                                                                                        cond.BSAGM_ServiceId.HasValue
                                                                                        ? (backgroundServicesForOrder.
                                                                                        Contains(cond.BSAGM_ServiceId.Value) && externalServicesForOrder.Contains(cond.EBS_ID))
                                                                                        : false);

                //Collection of attributes for all the external services mapped to background service purchased by applicant.
                IEnumerable<ExternalBkgSvcAttribute> bkgExtSvcAttributesForOrder = BALUtils.GetEVOrderSecurityRepoInstance().GetExtSvcAttributes()
                                                                                                                                .Where(cond =>
                                                                                                                                 externalServicesForOrder
                                                                                                                                 .Contains(cond.EBSA_SvcId));

                //For each loop to fill the EvOrderItemAttributeContracts for each BkgOrderPackageSvcLineItem in Service Group.
                svcGroupContract.OrderItems.ForEach(orderItem =>
                    {
                        List<OrderAttribute> filteredOrderAttributes = orderAttributes.Where(c => !c.LineItemID.HasValue 
                                                                                             || (c.LineItemID.HasValue && c.LineItemID.Value == orderItem.BkgOrderPackageSvcLineItemID)).ToList();

                        // Collection of BkgOrderLineItemDataMapping for the BkgOrderPackageSvcLineItem for Non Required Service Items.
                        // All the data mappings for non required service items will have not "NULL" value for "InstanceID" column.
                        IEnumerable<BkgOrderLineItemDataMapping> bkgOLIDMForNonReqItem = bkgOrderLineItemDataMappings.
                                                                                                       Where(cond =>
                                                                                                           cond.OLIDM_BkgOrderPackageSvcLineItemID
                                                                                                           == orderItem.BkgOrderPackageSvcLineItemID && cond.OLIDM_InstanceID.HasValue);


                        // Collection of BkgOrderLineItemDataMapping for the BkgOrderPackageSvcLineItem for Required Service Items.
                        // All the data mappings for required service items will have "NULL" value for "InstanceID" column.
                        IEnumerable<BkgOrderLineItemDataMapping> bkgOLIDMForRequiredItem = bkgOrderLineItemDataMappings.
                                                                                                       Where(cond =>
                                                                                                           cond.OLIDM_BkgOrderPackageSvcLineItemID
                                                                                                           == orderItem.BkgOrderPackageSvcLineItemID && !cond.OLIDM_InstanceID.HasValue);

                        //BkgOrderLineItemDataUsedAttrb table contains values for all the attributes which are required for a "Required Service Item"
                        List<BkgOrderLineItemDataUsedAttrb> bkgOLIDUserAttrForRequiredItem = new List<BkgOrderLineItemDataUsedAttrb>();

                        //Now for each data mapping we need to fill the collection of BkgOrderLineItemDataUsedAttrb so that 
                        //values corresponding to external service attributes can be deducted from them in case of required search  
                        bkgOLIDMForRequiredItem.ForEach(col =>
                        {
                            bkgOLIDUserAttrForRequiredItem.AddRange(col.BkgOrderLineItemDataUsedAttrbs);
                        });

                        //After collecting all the information in parts about the order
                        //The join below gets the collection of attribute group mapping with their value for attribute group and instance for the order item.
                        var nonReqItemInstancesForLine = bkgOLIDMForNonReqItem
                             .Join(filteredOrderAttributes
                             , olidm => new KeyValuePair<Int32?, Int32?>(olidm.OLIDM_InstanceID, olidm.OLIDM_BkgSvcAttributeGroupID)
                             , attrInstance => new KeyValuePair<Int32?, Int32?>(attrInstance.InstanceID, attrInstance.AttributeGroupID)
                             , (olidm, attrInstance) => new
                             {
                                 OrderLineItemID = olidm.OLIDM_BkgOrderPackageSvcLineItemID,
                                 AttributeGroupMappingID = attrInstance.AttributeGroupMappingID,
                                 AttributeGroupID = olidm.OLIDM_BkgSvcAttributeGroupID,
                                 AttributeGroupCode = olidm.BkgSvcAttributeGroup.BSAD_Code.ToString(),
                                 AttributeValue = attrInstance.AttributeValue
                             });

                        var reqItemInstancesForLine = bkgOLIDMForRequiredItem
                           .Join(bkgOLIDUserAttrForRequiredItem
                           , bofri => bofri.OLIDM_ID
                           , boua => boua.OLIDUA_OrderLineItemDataMappingID
                           , (bofri, boua) => new
                           {
                               OrderLineItemID = bofri.OLIDM_BkgOrderPackageSvcLineItemID,
                               AttributeGroupMappingID = boua.OLIDUA_BkgAttributeGroupMappingID,
                               AttributeGroupID = bofri.OLIDM_BkgSvcAttributeGroupID,
                               AttributeGroupCode = bofri.BkgSvcAttributeGroup.BSAD_Code.ToString(),
                               AttributeValue = boua.OLIDUA_AttributeValue
                           });


                        var aliasMappingForItem = nonReqItemInstancesForLine.FirstOrDefault(cond => cond.AttributeGroupCode.ToLower()
                                                                                        .Equals(AppConsts.PERSONAL_ALIAS_ATTRIBUTE_GROUP_CODE.ToLower()));
                        if (aliasMappingForItem.IsNotNull())
                        {
                            orderItem.Alias = aliasMappingForItem.AttributeValue.Trim();
                        }
                        else
                        {
                            var aliasMappingFromReqItem = reqItemInstancesForLine.FirstOrDefault(cond => cond.AttributeGroupCode.ToLower()
                                                                                       .Equals(AppConsts.PERSONAL_ALIAS_ATTRIBUTE_GROUP_CODE.ToLower()));

                            orderItem.Alias = aliasMappingFromReqItem.IsNotNull() ? aliasMappingFromReqItem.AttributeValue.Trim() : String.Empty;
                        }


                        //Collection of all vw_BkgExtSvcAttributeMapping 
                        //(containing combination of External and Background Services with their attribute mapping)
                        //for order item.
                        IEnumerable<vw_BkgExtSvcAttributeMapping> bkgExtSvcAttributeMappingForItem = bkgExtSvcAttributeMappingForOrder
                                                .Where(cond => cond.BSAGM_ServiceId == orderItem.BackgroundServiceID
                                                       && cond.EBS_ID == orderItem.ExternalServiceID);

                        IEnumerable<ExternalBkgSvcAttribute> bkgExtSvcAttributesForItem = bkgExtSvcAttributesForOrder
                                                                                                    .Where(cond => cond.EBSA_SvcId == orderItem.ExternalServiceID);

                        //The join below populates the order item with external attribute fields and their value for an attribute group.
                        orderItem.EvOrderItemAttributeContracts = nonReqItemInstancesForLine.Where(cond => cond.AttributeGroupID.HasValue).Join(
                                                   bkgExtSvcAttributeMappingForItem.Where(cond => cond.EBSA_FieldID.HasValue)
                                                 , liim => liim.AttributeGroupMappingID
                                                 , besam => besam.BSAGM_AttributeGroupMappingID
                                                 , (liim, besam) => new EvOrderItemAttributeContract
                                                 {
                                                     AttributeGroupID = liim.AttributeGroupID.Value,
                                                     FieldID = besam.EBSA_FieldID.Value,
                                                     FieldValue = liim.AttributeValue,
                                                     FieldName = besam.EBSA_Name,
                                                     DefaultValue = besam.EBSA_DefaultValue,
                                                     FieldFormat = besam.FieldFormat.IsNotNull() ? besam.FieldFormat : String.Empty,
                                                     FieldSequence = besam.FieldSequence.HasValue ? besam.FieldSequence.Value : AppConsts.NONE,
                                                     FieldDelimiter = besam.FieldDelimiter.IsNotNull() ? besam.FieldDelimiter : String.Empty,
                                                     FieldDataType = besam.FieldDataType,
                                                     FieldLabel = besam.FieldLabel,
                                                     ExternalBkgSvcAttributeID = besam.ExternalBkgSvcAttributeID,
                                                     ExtSvcAttributeLocationField = besam.ExtSvcAttributeLocationField
                                                 })
                                                 .Union
                                                 (
                                                 reqItemInstancesForLine.Where(cond => cond.AttributeGroupID.HasValue).Join(
                                                   bkgExtSvcAttributeMappingForItem.Where(cond => cond.EBSA_FieldID.HasValue)
                                                 , liim => liim.AttributeGroupMappingID
                                                 , besam => besam.BSAGM_AttributeGroupMappingID
                                                 , (liim, besam) => new EvOrderItemAttributeContract
                                                 {
                                                     AttributeGroupID = liim.AttributeGroupID.Value,
                                                     FieldID = besam.EBSA_FieldID.Value,
                                                     FieldValue = liim.AttributeValue,
                                                     FieldName = besam.EBSA_Name,
                                                     DefaultValue = besam.EBSA_DefaultValue,
                                                     FieldFormat = besam.FieldFormat.IsNotNull() ? besam.FieldFormat : String.Empty,
                                                     FieldSequence = besam.FieldSequence.HasValue ? besam.FieldSequence.Value : AppConsts.NONE,
                                                     FieldDelimiter = besam.FieldDelimiter.IsNotNull() ? besam.FieldDelimiter : String.Empty,
                                                     FieldDataType = besam.FieldDataType,
                                                     FieldLabel = besam.FieldLabel,
                                                     ExternalBkgSvcAttributeID = besam.ExternalBkgSvcAttributeID,
                                                     ExtSvcAttributeLocationField = besam.ExtSvcAttributeLocationField
                                                 })).ToList();


                        List<Int32> contractFields = orderItem.EvOrderItemAttributeContracts.Select(col => col.FieldID).Distinct().ToList();

                        orderItem.EvOrderItemAttributeContracts.AddRange(bkgExtSvcAttributesForItem.Where(cond => cond.EBSA_FieldID.HasValue
                                                   ? !contractFields.Contains(cond.EBSA_FieldID.Value) : false)
                                                   .Select(extAttr => new EvOrderItemAttributeContract
                                                   {
                                                       AttributeGroupID = AppConsts.NONE,
                                                       FieldID = extAttr.EBSA_FieldID.Value,
                                                       FieldLabel = extAttr.EBSA_Label,
                                                       FieldValue = String.Empty,
                                                       FieldName = extAttr.EBSA_Name,
                                                       DefaultValue = extAttr.EBSA_DefaultValue,
                                                       FieldFormat = String.Empty,
                                                       FieldSequence = AppConsts.NONE,
                                                       FieldDelimiter = String.Empty,
                                                       ExternalBkgSvcAttributeID = extAttr.EBSA_ID,
                                                       ExtSvcAttributeLocationField = extAttr.EBSA_LocationField
                                                   }));

                    });
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static Int32 GetBkgOrderStatusTypeIDByCpde(String orderStatusTypeCode, Int32 tenantID)
        {
            Int32 orderStatusTypeID = AppConsts.NONE;
            try
            {
                List<lkpOrderStatusType> orderStatusTypeList = LookupManager.GetLookUpData<lkpOrderStatusType>(tenantID).ToList();
                if (orderStatusTypeList.IsNotNull())
                {
                    orderStatusTypeID = orderStatusTypeList.Where(cond => cond.Code == orderStatusTypeCode).
                                                  FirstOrDefault().OrderStatusTypeID;
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return orderStatusTypeID;
        }

        public static Boolean UpdateExternalVendorOrdersRetryDate(Int32 externalVendorBkgOrderDetailID, Int32 tenantID)
        {
            Boolean response = false;
            try
            {
                ExternalVendorBkgOrderDetail externalVendorBkgOrderDetail = BALUtils.GetEVOrderClientRepoInstance(tenantID).
                                                                              GetExternalVendorBkgOrderDetail(externalVendorBkgOrderDetailID);

                if (externalVendorBkgOrderDetail.IsNotNull())
                {
                    externalVendorBkgOrderDetail.EVOD_LastUpdateRetryDate = DateTime.Now;
                }
                BALUtils.GetEVOrderClientRepoInstance(tenantID).UpdateVendorData();
                response = true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                response = false;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                response = false;
            }
            return response;
        }

        public static void UpdateRetryCountForBkgOrderLineItems(Int32 tenantID, List<Int32> bkgOrderLineItemIDs)
        {
            try
            {
                BALUtils.GetEVOrderClientRepoInstance(tenantID).UpdateRetryCountForBkgOrderLineItems(bkgOrderLineItemIDs);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

            }
        }

        #region Create JIRA Ticket Busines Methods

        public static List<usp_GetOrdersToBeCreateJiraTicket_Result> GetOrdersToBeCreateJiraTicket(Int32 tenantID, Int32 chunkSize, Int32 maxRetryCount,
                                                                                                    Int32 createJiraTicketTimeLag, Int32 retryCountTimeLag)
        {
            try
            {
                return BALUtils.GetEVOrderClientRepoInstance(tenantID).GetOrdersToBeCreateJiraTicket(chunkSize, maxRetryCount, createJiraTicketTimeLag, retryCountTimeLag)
                                                                      .ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        public static List<ExtSvcInvokeHistory> GetExtVendorUploadErrorListFromExtSvcInvokeHistory(Int32 tenantID,
                                                                                                   IEnumerable<usp_GetOrdersToBeCreateJiraTicket_Result> orderToBeCreateJiraTicket)
        {
            try
            {
                Int32 bkgOrderID = orderToBeCreateJiraTicket.Select(col => col.BkgOrderID).FirstOrDefault();
                return BALUtils.GetEVOrderClientRepoInstance(tenantID).GetExtVendorUploadErrorListFromExtSvcInvokeHistory(bkgOrderID, orderToBeCreateJiraTicket).ToList();

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return null;
        }

        public static Boolean CreateJiraTicketDetail(Int32 tenantID, JiraTicketDetail jiraTicketDetail)
        {
            try
            {
                jiraTicketDetail.CreatedByID = BkgOrderServiceUserID;
                jiraTicketDetail.CreatedOn = DateTime.Now;
                return BALUtils.GetEVOrderClientRepoInstance(tenantID).CreateJiraTicketDetail(jiraTicketDetail);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        public static Boolean UpdateBkgOrderIgnoredCreateJiraTicket(Int32 tenantID, Int32 bkgOrderID)
        {
            try
            {
                return BALUtils.GetEVOrderClientRepoInstance(tenantID).UpdateBkgOrderIgnoredCreateJiraTicket(BkgOrderServiceUserID, bkgOrderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            return false;
        }

        #endregion


        #endregion

        #region Private Methods

        private static BkgOrderEventHistory InsertBkgOrderEventHistory(Int32 tenantID, List<lkpOrderStatusType> orderStatusTypeList,
                                                                        String OrderPreviousStatusTypeCode, String OrderNewStatusTypeCode)
        {
            BkgOrderEventHistory bkgOrderEventHistory = null;
            try
            {
                List<lkpEventHistory> eventHistoryList = LookupManager.GetLookUpData<lkpEventHistory>(tenantID).ToList();
                bkgOrderEventHistory = new BkgOrderEventHistory();
                //bkgOrderEventHistory.BOEH_BkgOrderID = evUpdateOrderContract.BkgOrderID;
                if (orderStatusTypeList.IsNotNull())
                {
                    String previousOrderStatusType = String.Empty;
                    String newOrderStatusType = String.Empty;

                    if (orderStatusTypeList.Any(cond => cond.Code == OrderPreviousStatusTypeCode))
                    {
                        previousOrderStatusType = orderStatusTypeList.Where(cond => cond.Code == OrderPreviousStatusTypeCode)
                                                     .FirstOrDefault().StatusType;
                    }

                    if (orderStatusTypeList.Any(cond => cond.Code == OrderNewStatusTypeCode))
                    {
                        newOrderStatusType = orderStatusTypeList.Where(cond => cond.Code == OrderNewStatusTypeCode)
                                                     .FirstOrDefault().StatusType;
                    }

                    bkgOrderEventHistory.BOEH_OrderEventDetail = String.Format("Changed Order from {0} to {1}", previousOrderStatusType, newOrderStatusType);

                    if (eventHistoryList.IsNotNull())
                    {
                        if (eventHistoryList.Any(cond => cond.EH_Code == BkgOrderEvents.ORDER_UPDATED.GetStringValue()))
                        {
                            bkgOrderEventHistory.BOEH_EventHistoryId = eventHistoryList.Where(cond => cond.EH_Code == BkgOrderEvents.ORDER_UPDATED.GetStringValue())
                                                             .FirstOrDefault().EH_ID;
                        }
                    }

                    bkgOrderEventHistory.BOEH_IsDeleted = false;
                    bkgOrderEventHistory.BOEH_CreatedByID = BkgOrderServiceUserID;
                    bkgOrderEventHistory.BOEH_CreatedOn = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bkgOrderEventHistory;
        }

        private static void CopyData(EvUpdateOrderContract evUpdateOrderContract, Int32 tenantID)
        {
            try
            {
                //Commented below code related: UAT-1160-
                //if (evUpdateOrderContract.OrderNewStatusTypeCode == OrderStatusType.COMPLETED.GetStringValue())
                //{
                ComplianceDataManager.CopyData(-1, tenantID, BkgOrderServiceUserID, evUpdateOrderContract.BkgOrderID);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static BkgOrderEventHistory InsertBkgOrderServiceGroupEventHistory(Int32 tenantID, List<lkpBkgSvcGrpReviewStatusType> bkgSvcGrpReviewStatusList,
                                                                      String ServiceGroupPreviousStatusTypeCode, String ServiceGroupNewStatusTypeCode, String svcGroupName)
        {
            BkgOrderEventHistory bkgOrderEventHistory = null;
            try
            {
                if (!String.IsNullOrEmpty(ServiceGroupPreviousStatusTypeCode) && !String.IsNullOrEmpty(ServiceGroupNewStatusTypeCode))
                {
                    List<lkpEventHistory> eventHistoryList = LookupManager.GetLookUpData<lkpEventHistory>(tenantID).ToList();
                    bkgOrderEventHistory = new BkgOrderEventHistory();
                    //bkgOrderEventHistory.BOEH_BkgOrderID = evUpdateOrderContract.BkgOrderID;
                    if (bkgSvcGrpReviewStatusList.IsNotNull())
                    {
                        String previousSvcGroupReviewStatusType = String.Empty;
                        String newSvcGroupReviewStatusType = String.Empty;

                        if (bkgSvcGrpReviewStatusList.Any(cond => cond.BSGRS_ReviewCode == ServiceGroupPreviousStatusTypeCode))
                        {
                            previousSvcGroupReviewStatusType = bkgSvcGrpReviewStatusList.Where(cond => cond.BSGRS_ReviewCode == ServiceGroupPreviousStatusTypeCode)
                                                         .FirstOrDefault().BSGRS_ReviewStatusType;
                        }

                        if (bkgSvcGrpReviewStatusList.Any(cond => cond.BSGRS_ReviewCode == ServiceGroupNewStatusTypeCode))
                        {
                            newSvcGroupReviewStatusType = bkgSvcGrpReviewStatusList.Where(cond => cond.BSGRS_ReviewCode == ServiceGroupNewStatusTypeCode)
                                                         .FirstOrDefault().BSGRS_ReviewStatusType;
                        }

                        bkgOrderEventHistory.BOEH_OrderEventDetail = String.Format("Changed Background Service Group: {0} Review Status from {1} to {2}", svcGroupName, previousSvcGroupReviewStatusType, newSvcGroupReviewStatusType);
                        if (eventHistoryList.IsNotNull())
                        {
                            if (eventHistoryList.Any(cond => cond.EH_Code == BkgOrderEvents.ORDER_UPDATED.GetStringValue()))
                            {
                                bkgOrderEventHistory.BOEH_EventHistoryId = eventHistoryList.Where(cond => cond.EH_Code == BkgOrderEvents.ORDER_UPDATED.GetStringValue())
                                                                 .FirstOrDefault().EH_ID;
                            }
                        }
                        bkgOrderEventHistory.BOEH_IsDeleted = false;
                        bkgOrderEventHistory.BOEH_CreatedByID = BkgOrderServiceUserID;
                        bkgOrderEventHistory.BOEH_CreatedOn = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bkgOrderEventHistory;
        }

        /// <summary>
        /// Update order service group status type
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="newServiceGroupStatusTypeCode">New service group status type code</param>
        /// <param name="bkgOrder"></param>
        public static void UpdateServiceGroupStatus(Int32 tenantId, String newServiceGroupStatusTypeCode, BkgOrder bkgOrder, List<BkgOrderPackageSvcGroup> _lstBkgOrderPAckageSvcGroup)
        {
            List<lkpBkgSvcGrpStatusType> bkgSvcGroupStatusTypeList = LookupManager.GetLookUpData<lkpBkgSvcGrpStatusType>(tenantId).ToList();
            List<lkpBkgSvcGrpReviewStatusType> bkgSvcGroupReviewStatusTypeList = LookupManager.GetLookUpData<lkpBkgSvcGrpReviewStatusType>(tenantId).ToList();
            List<lkpEventHistory> eventHistoryList = LookupManager.GetLookUpData<lkpEventHistory>(tenantId).ToList();
            Int32 newBkgSvcGroupStatusId = AppConsts.NONE;
            String previousBkgSvcGroupStatusName = String.Empty;
            String newBkgSvcGroupStatusName = String.Empty;
            String previousSvcGroupStatusTypeCode = String.Empty;
            String bkgServiceGroupName = String.Empty;

            BkgOrderEventHistory bkgOrderEventHistory = null;

            if (bkgSvcGroupStatusTypeList.Any(x => x.BSGS_StatusCode == newServiceGroupStatusTypeCode && x.BSGS_IsDeleted == false))
            {
                newBkgSvcGroupStatusId = bkgSvcGroupStatusTypeList.FirstOrDefault(x => x.BSGS_StatusCode == newServiceGroupStatusTypeCode && x.BSGS_IsDeleted == false).BSGS_ID;
                newBkgSvcGroupStatusName = bkgSvcGroupStatusTypeList.FirstOrDefault(x => x.BSGS_StatusCode == newServiceGroupStatusTypeCode && x.BSGS_IsDeleted == false).BSGS_StatusType;
            }

            if (_lstBkgOrderPAckageSvcGroup.IsNotNull() && _lstBkgOrderPAckageSvcGroup.Count > AppConsts.NONE)
            {
                _lstBkgOrderPAckageSvcGroup.ForEach(SvcGroup =>
                {
                    if (SvcGroup.lkpBkgSvcGrpStatusType.IsNotNull())
                    {
                        previousBkgSvcGroupStatusName = SvcGroup.lkpBkgSvcGrpStatusType.BSGS_StatusType;
                        previousSvcGroupStatusTypeCode = SvcGroup.lkpBkgSvcGrpStatusType.BSGS_StatusCode;
                    }
                    if (SvcGroup.BkgSvcGroup.IsNotNull())
                    {
                        bkgServiceGroupName = SvcGroup.BkgSvcGroup.BSG_Name;
                    }

                    //If previous status of Service group is not equals to new status.
                    if (previousSvcGroupStatusTypeCode != newServiceGroupStatusTypeCode)
                    {
                        SvcGroup.OPSG_SvcGrpStatusTypeID = newBkgSvcGroupStatusId;
                        if (newServiceGroupStatusTypeCode == BkgSvcGrpStatusType.COMPLETED.GetStringValue())
                        {
                            Int32 bkgSvcGroupReviewStatusID = bkgSvcGroupReviewStatusTypeList.Where(cond =>
                                                                            cond.BSGRS_ReviewCode == BkgSvcGrpReviewStatusType.AUTO_REVIEW_COMPLETED.GetStringValue())
                                                                            .Select(col => col.BSGRS_ID).FirstOrDefault();
                            if (bkgSvcGroupReviewStatusID != AppConsts.NONE)
                            {
                                SvcGroup.OPSG_SvcGrpReviewStatusTypeID = bkgSvcGroupReviewStatusID;
                                SvcGroup.OPSG_SvcGrpCompletionDate = DateTime.Now;
                            }
                        }
                        SvcGroup.OPSG_ModifiedByID = bkgOrder.BOR_ModifiedByID;
                        SvcGroup.OPSG_ModifiedOn = DateTime.Now;

                        //Insert record in 'BkgOrderEventHistory'
                        if (!String.IsNullOrEmpty(previousBkgSvcGroupStatusName) && !String.IsNullOrEmpty(newBkgSvcGroupStatusName))
                        {
                            bkgOrderEventHistory = new BkgOrderEventHistory();
                            bkgOrderEventHistory.BOEH_OrderEventDetail = String.Format("Changed Background Service Group: {0} status from {1} to {2}", bkgServiceGroupName, previousBkgSvcGroupStatusName, newBkgSvcGroupStatusName);

                            if (eventHistoryList.IsNotNull())
                            {
                                if (eventHistoryList.Any(cond => cond.EH_Code == BkgOrderEvents.ORDER_UPDATED.GetStringValue()))
                                {
                                    bkgOrderEventHistory.BOEH_EventHistoryId = eventHistoryList.Where(cond => cond.EH_Code == BkgOrderEvents.ORDER_UPDATED.GetStringValue())
                                                                     .FirstOrDefault().EH_ID;
                                }
                            }

                            bkgOrderEventHistory.BOEH_IsDeleted = false;
                            bkgOrderEventHistory.BOEH_CreatedByID = bkgOrder.BOR_ModifiedByID.Value;
                            bkgOrderEventHistory.BOEH_CreatedOn = DateTime.Now;
                            bkgOrder.BkgOrderEventHistories.Add(bkgOrderEventHistory);
                        }
                    }
                });
            }
        }

        #region UAT-2303: Supplement automation

        static private Boolean _isDateExistInSSNResult = true;
        static private Boolean _isValidNamesForSSN = false;
        static private Boolean _isMilitaryAddressExist = false;
        static private String _noMiddleNameText = WebConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY];

        /// <summary>
        /// Update Supplement Order Automatically
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="masterOrderID"></param>
        /// <param name="bkgOrderID"></param>
        private static void UpdateSupplementOrderAutomatically(Int32 tenantID, Int32 masterOrderID, Int32 bkgOrderID, Int32 orderPkgSvcGroupID)
        {
            List<SupplementAdditionalSearchContract> lstSSNResultForAdditionalSearch = GetOtherServiceResults(tenantID, masterOrderID);

            //UAT-2114: Dont show additional searches if line items will not be created.
            List<SupplementAdditionalSearchContract> lstSSNResultProcessedForAdditionalSearch = GetMatchedAdditionalSearchData(tenantID, masterOrderID, lstSSNResultForAdditionalSearch);

            if (!lstSSNResultForAdditionalSearch.IsNullOrEmpty() && !lstSSNResultProcessedForAdditionalSearch.IsNullOrEmpty())
            {
                if (!(lstSSNResultForAdditionalSearch.Any(an => an.IsExistInLastSevenYear && !an.DisplayRowInGray && !an.IsUsedForSearch)))
                {
                    //Changes related to UAT-2399:If there are no red lines in SSN trace results and additional searches are added by the system
                    //, add the searches automatically without need for review.
                    //Tuple Data return below items:-
                    // 1) Item1 represents that searches created for order or not.
                    // 2) Item2 return the Supplement Order services data(Pricing data).
                    // 3) Item3 return the list of services in order.
                    Tuple<Boolean, List<SupplementOrderServices>, List<Int32>> tupleData = GetFilteredLocationSearches(tenantID, masterOrderID, lstSSNResultProcessedForAdditionalSearch);
                    //if (!GetFilteredLocationSearches(tenantID, masterOrderID, lstSSNResultProcessedForAdditionalSearch))
                    if (!tupleData.Item1)
                    {
                        CheckOrderToUpdate(tenantID, masterOrderID, bkgOrderID);
                    }
                    else
                    {
                        //Changes related to UAT-2399:If there are no red lines in SSN trace results and additional searches are added by the system
                        //, add the searches automatically without need for review.
                        AddAdditionalSearchesForOrderServiceGroup(tenantID, _bkgOrderServiceUserId, masterOrderID, tupleData.Item2, orderPkgSvcGroupID, tupleData.Item3);
                    }
                }
            }

        }

        /// <summary>
        /// Method to check and apply success indicator and update the order status.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="masterOrderID"></param>
        /// <param name="bkgOrderID"></param>
        /// <returns></returns>
        private static Boolean CheckOrderToUpdate(Int32 tenantID, Int32 masterOrderID, Int32 bkgOrderID)
        {
            Dictionary<String, String> resultDic = new Dictionary<String, String>();
            Int32 institutionColorFlagID = AppConsts.NONE;
            Int32 orderStatusTypeID_Completed = AppConsts.NONE;
            String orderStatusTypeCode_Completed = OrderStatusType.COMPLETED.GetStringValue();
            Boolean saveStatus = false;
            Boolean isSuccessIndicatorApplicable = false;
            Boolean isAllExistingSearchesAreClear = false;
            Boolean isOtherServiceGroupsAreCompleted = false;

            resultDic = BackgroundProcessOrderManager.CheckBackgroundOrderForAllSvcGroupsToUpdate(tenantID, masterOrderID);

            if (!resultDic.IsNullOrEmpty() && resultDic.ContainsKey(AppConsts.IS_SUCCESS_INDICATOR_APPLICABLE) && resultDic.ContainsKey(AppConsts.IS_ALL_EXISTING_SEARCHES_ARE_CLEAR))
            {
                isSuccessIndicatorApplicable = Convert.ToBoolean(resultDic[AppConsts.IS_SUCCESS_INDICATOR_APPLICABLE]);
                isAllExistingSearchesAreClear = Convert.ToBoolean(resultDic[AppConsts.IS_ALL_EXISTING_SEARCHES_ARE_CLEAR]);
                institutionColorFlagID = Convert.ToInt32(resultDic[AppConsts.INSTITUTION_COLOR_FLAG_ID]);
                isOtherServiceGroupsAreCompleted = Convert.ToBoolean(resultDic[AppConsts.IS_OTHER_SERVICE_GROUPS_ARE_COMPLETED]);
            }
            orderStatusTypeID_Completed = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderStatusType>(tenantID).FirstOrDefault(x => x.Code == orderStatusTypeCode_Completed).OrderStatusTypeID;
            if (isOtherServiceGroupsAreCompleted)
            {
                if (isSuccessIndicatorApplicable && isAllExistingSearchesAreClear)
                {
                    //Update Supplement Automation status for Bkg order and save Supplement Automation status history
                    List<Entity.ClientEntity.lkpSupplementAutomationStatu> supplementAutomationStatusList = LookupManager.GetLookUpData<Entity.ClientEntity.lkpSupplementAutomationStatu>(tenantID).Where(x => !x.SAS_IsDeleted).ToList();

                    String bkgSvcGrpStatusTypeCodeCompleted = BkgSvcGrpStatusType.COMPLETED.GetStringValue();
                    Int32 bkgSvcGrpStatusTypeCompletedID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpBkgSvcGrpStatusType>(tenantID).FirstOrDefault(x => x.BSGS_StatusCode == bkgSvcGrpStatusTypeCodeCompleted && !x.BSGS_IsDeleted).BSGS_ID;
                    String bkgSvcGrpReviewStatusTypeCodeFirstReview = BkgSvcGrpReviewStatusType.FIRST_REVIEW.GetStringValue();
                    Int32 bkgSvcGrpReviewStatusTypeFirstReviewID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpBkgSvcGrpReviewStatusType>(tenantID).FirstOrDefault(x => x.BSGRS_ReviewCode == bkgSvcGrpReviewStatusTypeCodeFirstReview && !x.BSGRS_IsDeleted).BSGRS_ID;

                    BackgroundProcessOrderManager.SaveSupplementAutomationStatusAndHistory(tenantID, masterOrderID, supplementAutomationStatusList, bkgSvcGrpStatusTypeCompletedID, bkgSvcGrpReviewStatusTypeFirstReviewID, _bkgOrderServiceUserId);

                    //Update order status to complete
                    if (BackgroundProcessOrderManager.UpdateOrderStatus(tenantID, institutionColorFlagID, masterOrderID, orderStatusTypeID_Completed, _bkgOrderServiceUserId, AppConsts.NONE, null))
                    {
                        saveStatus = true;
                        EvUpdateOrderContract evUpdateOrderContract = new EvUpdateOrderContract();
                        evUpdateOrderContract.BkgOrderID = bkgOrderID;
                        CopyData(evUpdateOrderContract, tenantID);
                    }
                }
            }
            return saveStatus;
        }

        /// <summary>
        /// Bind the SSN Trace data
        /// </summary>
        private static List<SupplementAdditionalSearchContract> GetOtherServiceResults(Int32 tenantID, Int32 masterOrderID)
        {
            SourceServiceDetailForSupplement sourceService = BackgroundProcessOrderManager.CheckSourceServicesForSupplement(tenantID, masterOrderID);
            if (sourceService.IfSSNServiceExist)
            {
                String completeSSNResultText = sourceService.SSNServiceResult;
                List<KeyValuePair<string, string>> ssnDS = new List<KeyValuePair<string, string>>();

                if (String.IsNullOrEmpty(completeSSNResultText))
                {
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>("Empty result. Perhaps not sent yet to Clearstar...", Guid.NewGuid().ToString());
                    ssnDS.Add(kvp);
                }
                else
                {
                    string[] splittedCompleteSSNResultText = Regex.Split(completeSSNResultText, @"##NewLine##");
                    foreach (String ssnResultText in splittedCompleteSSNResultText)
                    {
                        ProcessSSNResultText(ssnDS, ssnResultText);
                    }
                    //Method to Get Additional Search data from  SSN Result XML [UAT-2062]
                    return GetAdditionalSearchDataFromSSNResult(ssnDS);

                }
            }
            return null;
        }

        /// <summary>
        /// Process SSN Result text
        /// </summary>
        /// <param name="ssnDS"></param>
        /// <param name="ssnResultText"></param>
        private static void ProcessSSNResultText(List<KeyValuePair<string, string>> ssnDS, String ssnResultText)
        {
            String resultText = ssnResultText;
            int myLastCharPosition = resultText.IndexOf("This product is a locater index");
            if (myLastCharPosition > 0)
            {
                resultText = resultText.Substring(0, myLastCharPosition);
            }
            string[] mySplitSSNResults = Regex.Split(resultText, @"_+");
            foreach (string s in mySplitSSNResults)
            {
                Regex.Replace(s, @"^\s+", "");
                Regex.Replace(s, @"\s+$", "");
                KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(s, Guid.NewGuid().ToString());
                ssnDS.Add(kvp);
            }
        }

        /// <summary>
        /// Get Matched Additional Search Data
        /// </summary>
        private static List<SupplementAdditionalSearchContract> GetMatchedAdditionalSearchData(Int32 tenantID, Int32 masterOrderID, List<SupplementAdditionalSearchContract> lstSSNResultForAdditionalSearch)
        {
            if (!lstSSNResultForAdditionalSearch.IsNullOrEmpty())
            {
                String inputXml = GenerateAdditionalSearchXML(lstSSNResultForAdditionalSearch);
                if (!inputXml.IsNullOrEmpty())
                {
                    return BackgroundProcessOrderManager.GetMatchedAdditionalSearchData(tenantID, inputXml, masterOrderID);
                }
            }
            return null;
        }

        /// <summary>
        /// Method to convert Additional search data into XML
        /// </summary>
        /// <param name="lstSSNResultForAdditionalSearch"></param>
        /// <returns></returns>
        private static String GenerateAdditionalSearchXML(List<SupplementAdditionalSearchContract> lstSSNResultForAdditionalSearch)
        {
            var tempSSNResultList = lstSSNResultForAdditionalSearch.Where(x => x.IsUsedForSearch && x.IsExistInLastSevenYear);
            if (!tempSSNResultList.IsNullOrEmpty())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<SearchDatas>");
                tempSSNResultList.ToList().ForEach(cnd =>
                {
                    sb.Append("<SearchData>");
                    sb.Append("<FirstName>" + cnd.FirstName + "</FirstName>");
                    sb.Append("<LastName>" + cnd.LastName + "</LastName>");
                    sb.Append("<MiddleName>" + cnd.MiddleName + "</MiddleName>");
                    sb.Append("<County>" + cnd.CountyName + "</County>");
                    sb.Append("<StateAbbreviation>" + cnd.StateAbbreviation + "</StateAbbreviation>");
                    sb.Append("<IsUsedForSearch>" + cnd.IsUsedForSearch + "</IsUsedForSearch>");
                    sb.Append("<IsExistInLastSevenYear>" + cnd.IsExistInLastSevenYear + "</IsExistInLastSevenYear>");
                    sb.Append("</SearchData>");
                });
                sb.Append("</SearchDatas>");
                return sb.ToString();
            }
            return null;
        }

        /// <summary>
        /// Method to get Additional Search Data from SSN Result
        /// </summary>
        /// <param name="SSNResults"></param>
        private static List<SupplementAdditionalSearchContract> GetAdditionalSearchDataFromSSNResult(List<KeyValuePair<string, string>> SSNResults)
        {
            List<SupplementAdditionalSearchContract> lstSSNResultForAdditionalSearch = new List<SupplementAdditionalSearchContract>();
            if (!SSNResults.IsNullOrEmpty())
            {
                foreach (var _SSNResult in SSNResults)
                {
                    String firstName = String.Empty;
                    String lastName = String.Empty;
                    String middleName = String.Empty;
                    String county = String.Empty;
                    String toDate = String.Empty;
                    String state = String.Empty;
                    String resultRow = _SSNResult.Key;
                    String guidValue = _SSNResult.Value;
                    String lastSevenYrStringDate = DateTime.Now.AddYears(-7).ToString("MM/yyyy");
                    DateTime lastSevenYrDate = Convert.ToDateTime(lastSevenYrStringDate);
                    DateTime? toDateTime = null;

                    //check for 'Subject' row if exist : (ex.'5 Subjects Found.SSN is valid.Issued in California  (Issued In Year 1989-1990)')
                    if (!resultRow.IsNullOrEmpty() && !resultRow.Contains("Subjects"))
                    {
                        try
                        {
                            String[] splitRsultRowBySSN = Regex.Split(resultRow, @"SSN");
                            if (!splitRsultRowBySSN.IsNullOrEmpty() && splitRsultRowBySSN.Length > AppConsts.ONE && IsResultContainsRequiredParameters(resultRow))
                            {
                                //To Get FirstName And LastName
                                String applicantName = splitRsultRowBySSN[0].Replace("\n", "");
                                String[] splitedNames = applicantName.IsNullOrEmpty() ? null : applicantName.Trim().Split(' ');
                                if (!splitedNames.IsNullOrEmpty() && splitedNames.Length > AppConsts.NONE)
                                {
                                    firstName = splitedNames.First().Trim();
                                    //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                                    middleName = String.Join(" ", splitedNames.Where((cond, index) => index < splitedNames.Length - 1 && index > AppConsts.NONE));
                                    lastName = splitedNames.Length > AppConsts.ONE ? splitedNames.LastOrDefault().Trim() : String.Empty;
                                    middleName = middleName.IsNullOrEmpty() ? _noMiddleNameText : middleName;
                                }
                                //To Get County: Get all text after 5-digit number and upto the text 'county'.
                                var countyMatches = Regex.Matches(splitRsultRowBySSN[1], @"(([^\d{5}]*)(county*\s))", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
                                if (!countyMatches.IsNullOrEmpty() && countyMatches.Count > AppConsts.NONE && !countyMatches[0].IsNullOrEmpty())
                                {
                                    String[] splittedCounty = Regex.Split(Convert.ToString(countyMatches[0]).Trim(), @" ");
                                    county = String.Join(" ", splittedCounty.Where((cond, index) => index < splittedCounty.Length - 1));
                                    county = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(county.Trim().ToLower());
                                }

                                //To Get State: Get all text after ',' and upto the 5-digit number.
                                var stateMatches = Regex.Matches(splitRsultRowBySSN[1], @"([^\,]*)(\s\d{5}\s)", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
                                if (!stateMatches.IsNullOrEmpty() && stateMatches.Count > AppConsts.NONE && !stateMatches[0].IsNullOrEmpty())
                                {
                                    string stateMatch = Convert.ToString(stateMatches[0]);
                                    if (Regex.IsMatch(Convert.ToString(stateMatches[0]), @"DOB", RegexOptions.IgnoreCase)
                                        && Regex.IsMatch(Convert.ToString(stateMatches[0]), @"Address", RegexOptions.IgnoreCase)
                                        && stateMatches.Count > AppConsts.ONE
                                        )
                                    {
                                        stateMatch = Convert.ToString(stateMatches[1]);
                                    }
                                    String[] splittedState = Regex.Split(stateMatch.Trim(), @" ");
                                    splittedState = splittedState.Where(x => x != null && x.Trim() != "").ToArray();
                                    if (splittedState.Length == AppConsts.TWO && Regex.IsMatch(splittedState[1], @"(\d{5})", RegexOptions.IgnoreCase)
                                         && splittedState[0].Length == AppConsts.TWO)
                                    {
                                        state = splittedState[0].Trim();
                                    }
                                }

                                //To Get End Date: Get text after text 'to'
                                var toDatematches = Regex.Matches(splitRsultRowBySSN[1], @"(?<=to\s)(?<toDate>\b\S+\b)", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
                                if (!toDatematches.IsNullOrEmpty() && toDatematches.Count > AppConsts.NONE && !toDatematches[0].IsNullOrEmpty())
                                {
                                    toDate = Convert.ToString(toDatematches[0].Groups["toDate"]);
                                    if (!toDate.IsNullOrEmpty())
                                    {
                                        toDateTime = Convert.ToDateTime(toDate);
                                    }
                                }
                            }
                            //UAT-2149:Scenarios not to show in red text on SSN Trace results
                            _isMilitaryAddressExist = IsMilitaryAddressExist(resultRow);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                        finally
                        {
                            SupplementAdditionalSearchContract additionalSearchData = new SupplementAdditionalSearchContract();
                            additionalSearchData.StateAbbreviation = state;
                            additionalSearchData.CountyName = county;
                            additionalSearchData.FirstName = firstName;
                            additionalSearchData.LastName = lastName;
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            additionalSearchData.MiddleName = middleName;
                            additionalSearchData.UniqueRowId = guidValue;
                            additionalSearchData.IsUsedForSearch = _isMilitaryAddressExist ? false : IsSSNResultValidForAdditionalSearch(firstName, lastName, state, county, toDateTime);
                            additionalSearchData.IsExistInLastSevenYear = true;
                            //UAT-2149:Scenarios not to show in red text on SSN Trace results
                            additionalSearchData.DisplayRowInGray = !_isDateExistInSSNResult ? true : _isMilitaryAddressExist ? true : !_isValidNamesForSSN;

                            //Business logic: end date within the last 7 years
                            if ((!toDateTime.IsNullOrEmpty() && toDateTime < lastSevenYrDate))
                            {
                                additionalSearchData.IsExistInLastSevenYear = false;
                                //UAT-2149:Scenarios not to show in red text on SSN Trace results
                                additionalSearchData.DisplayRowInGray = true;
                            }

                            lstSSNResultForAdditionalSearch.Add(additionalSearchData);
                        }
                    }
                }
                //if (!lstSSNResultForAdditionalSearch.IsNullOrEmpty() && lstSSNResultForAdditionalSearch.Any(an => an.IsExistInLastSevenYear && !an.IsUsedForSearch && !an.DisplayRowInGray))
                //{
                //    UnIdentifiedSSNResultMessage = "System could not parse highlighted SSN trace results based on format specified. Please process these results manually.";
                //}
            }
            return lstSSNResultForAdditionalSearch;
        }

        /// <summary>
        /// Method to identify that the SSN result contains Military Address(text:- APO, AE, DPO, or FPO)
        /// </summary>
        /// <param name="SSNResultRowData">SSN result</param>
        /// <returns>Boolean</returns>
        private static Boolean IsMilitaryAddressExist(String SSNResultRowData)
        {
            String address = string.Empty;
            Boolean isMilitaryAddressExist = false;
            if (!SSNResultRowData.IsNullOrEmpty())
            {
                String[] splitAddress = Regex.Split(SSNResultRowData, @"Address", RegexOptions.IgnoreCase);
                if (!splitAddress.IsNullOrEmpty())
                {
                    address = splitAddress.Where(x => x != null && x != String.Empty).LastOrDefault();
                    address = address.Replace(',', ' ');
                    if (!address.IsNullOrEmpty()
                        && (Regex.IsMatch(address, @"(?<=\s)(APO+\s)", RegexOptions.IgnoreCase)
                            || Regex.IsMatch(address, @"(?<=\s)(AE+\s)", RegexOptions.IgnoreCase)
                            || Regex.IsMatch(address, @"(?<=\s)(DPO+\s)", RegexOptions.IgnoreCase)
                            || Regex.IsMatch(address, @"(?<=\s)(FPO+\s)", RegexOptions.IgnoreCase)
                           )
                       )
                    {
                        isMilitaryAddressExist = true;
                    }
                }
            }
            return isMilitaryAddressExist;
        }

        /// <summary>
        /// Is SSN Result valid for Additional Search
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="state"></param>
        /// <param name="county"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private static Boolean IsSSNResultValidForAdditionalSearch(String firstName, String lastName, String state, String county, DateTime? toDate)
        {
            Boolean isValidForSearch = false;

            if (!firstName.IsNullOrEmpty() && !lastName.IsNullOrEmpty() && !state.IsNullOrEmpty() && !county.IsNullOrEmpty() && !toDate.IsNullOrEmpty())
            {
                //Bussiness logic: there must be at least 3 characters in both the first name and last name in order to add the search [UAT-2149]
                isValidForSearch = IsValidFirstAndLastNameLength(firstName, lastName, AppConsts.THREE);
                _isValidNamesForSSN = isValidForSearch;
            }
            return isValidForSearch;
        }

        /// <summary>
        /// Method to validate the first name and last name according to the following business logics
        /// 1) Bussiness logic: there must be at least 2 characters in both the first name and last name for non-military addresses in order to add the search [UAT-2062]
        /// 2) Bussiness logic: there must be at least 3 characters in both the first name and last name for military addresses in order to add the search [UAT-2149]
        /// </summary>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        /// <param name="charLength">Char length ie. 2 or 3 according to above defined business logics</param>
        /// <returns>Boolean</returns>
        private static Boolean IsValidFirstAndLastNameLength(String firstName, String lastName, Int32 charLength)
        {
            Boolean isLengthValid = false;
            if (firstName.Length > charLength - 1 && lastName.Length > charLength - 1)
            {
                isLengthValid = true;
            }
            return isLengthValid;
        }

        /// <summary>
        /// Method to Check following parameters exist in result or not
        /// Parameters: SSN:,5 digit number,Comma,"to","county"
        /// </summary>
        /// <param name="result"></param>
        /// <returns>Boolean</returns>
        private static Boolean IsResultContainsRequiredParameters(String result)
        {
            Boolean isValid = true;
            if (!Regex.IsMatch(result, @"(SSN:)", RegexOptions.IgnoreCase))
                isValid = false;
            if (!Regex.IsMatch(result, @"(\s\d{5}\s)", RegexOptions.IgnoreCase))
                isValid = false;
            if (!result.Contains(","))
                isValid = false;
            if (!Regex.IsMatch(result, @"(\sto\s)", RegexOptions.IgnoreCase))
            {
                _isDateExistInSSNResult = false;
                isValid = false;
            }
            if (!Regex.IsMatch(result, @"(county)", RegexOptions.IgnoreCase))
                isValid = false;
            return isValid;

        }

        /// <summary>
        /// Get Filtered Location Searches
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="masterOrderID"></param>
        /// <returns></returns>
        public static Tuple<Boolean, List<SupplementOrderServices>, List<Int32>> GetFilteredLocationSearches(Int32 tenantID, Int32 masterOrderID, List<SupplementAdditionalSearchContract> lstSSNResultForAdditionalSearch)
        {
            List<Entity.ClientEntity.BkgAttributeGroupMapping> lstBkgAttributeGroupMapping = BackgroundProcessOrderManager.GetAllBkgAttributeGroupMapping(tenantID);
            Guid aliasNameGuid = new Guid("56258C54-C2BC-4514-94E1-2EF2EFFFDBF5");
            Guid stateGuid = new Guid("CAEAC9FA-FFF4-4F7A-A644-96967F399362");
            Guid countryGuid = new Guid("37B6B708-C691-4568-B604-6F70F24BC839");
            Guid countyGuid = new Guid("C00AEFB5-37DF-44F7-A050-D2C9581909DE");
            Int32 attributeGroupMappingIdForAliasName = lstBkgAttributeGroupMapping.Where(cond => cond.BAGM_Code == aliasNameGuid && !cond.BAGM_IsDeleted).FirstOrDefault().BAGM_ID;
            Int32 attributeGroupMappingIdForState = lstBkgAttributeGroupMapping.Where(cond => cond.BAGM_Code == stateGuid && !cond.BAGM_IsDeleted).FirstOrDefault().BAGM_ID;
            Int32 attributeGroupMappingIdForCounty = lstBkgAttributeGroupMapping.Where(cond => cond.BAGM_Code == countyGuid && !cond.BAGM_IsDeleted).FirstOrDefault().BAGM_ID;
            Int32 attributeGroupMappingIdForCountry = lstBkgAttributeGroupMapping.Where(cond => cond.BAGM_Code == countryGuid && !cond.BAGM_IsDeleted).FirstOrDefault().BAGM_ID;

            bool checkIfLinetItemsExists = false;
            List<SupplementOrderServices> _lstSupplementOrderPricing = new List<SupplementOrderServices>();
            List<SupplementServicesInformation> lstSupplementServiceList = BackgroundProcessOrderManager.GetSupplementServices(masterOrderID, tenantID);
            List<Int32> selectedSupplementServices = lstSupplementServiceList.Select(x => x.PackageServiceId).ToList();
            List<SupplementServiceItemCustomForm> lstSupplementServiceCustomFormList = GetListOfCustomFormsForSelectedServices(tenantID, selectedSupplementServices);
            SupplementOrderCart supplementOrderCart = SetSupplOrderDataInCartToFilterLocationSearch(tenantID, masterOrderID, lstSupplementServiceCustomFormList, selectedSupplementServices, lstSSNResultForAdditionalSearch, attributeGroupMappingIdForAliasName, attributeGroupMappingIdForState, attributeGroupMappingIdForCounty);

            //&& CurrentViewContext.ParentScreenName == AppConsts.CUSTOM_FORM_FOR_SERVICE_ITEM_SUPPLEMENT
            if (supplementOrderCart.IsNotNull())
            {
                XmlDocument _doc = new XmlDocument();
                XmlElement _rootNode = (XmlElement)_doc.AppendChild(_doc.CreateElement("BkgOrderAddition"));

                List<Int32> _lstPackageServiceIds = lstSupplementServiceCustomFormList
                                                .DistinctBy(svc => svc.PackageServiceId)
                                                .Select(svcId => svcId.PackageServiceId).ToList();

                _rootNode.AppendChild(_doc.CreateElement("MasterOrderID")).InnerText = Convert.ToString(masterOrderID);

                XmlNode _packageServiceNode = _rootNode.AppendChild(_doc.CreateElement("PackageService"));
                foreach (var _packageServiceId in _lstPackageServiceIds)
                {
                    _packageServiceNode.AppendChild(_doc.CreateElement("PackageSvcID")).InnerText = Convert.ToString(_packageServiceId);
                }

                List<SupplementServiceItemCustomForm> lstCustomForms = new List<SupplementServiceItemCustomForm>();
                lstCustomForms = lstSupplementServiceCustomFormList.DistinctBy(x => x.CustomFormID).Select(col => col).ToList();
                foreach (SupplementServiceItemCustomForm customForm in lstCustomForms)
                {
                    List<SupplementOrderData> _lstSupplementOrderData = supplementOrderCart.lstSupplementOrderData
                                                                        .Where(sod => sod.BkgServiceId == customForm.ServiceId)
                                                                        .ToList();
                    List<SupplementOrderData> _lstBkgAttributeDataGrps = _lstSupplementOrderData
                                                                            .Where(svc => svc.PackageSvcItemId == customForm.ServiceItemID
                                                                                  && svc.BkgServiceId == customForm.ServiceId)
                                                                            .ToList();
                    foreach (var _attrGrp in _lstBkgAttributeDataGrps)
                    {
                        XmlNode _attDataGrpNode = _rootNode.AppendChild(_doc.CreateElement("BkgSvcAttributeDataGroup"));
                        _attDataGrpNode.AppendChild(_doc.CreateElement("AttributeGroupID")).InnerText = Convert.ToString(_attrGrp.BkgSvcAttributeGroupId);
                        _attDataGrpNode.AppendChild(_doc.CreateElement("InstanceId")).InnerText = Convert.ToString(_attrGrp.InstanceId);

                        if (_attrGrp.FormData.Any(cond => cond.Key == attributeGroupMappingIdForState))//if not personal alias type mapping id then add mapping id for country
                        {
                            XmlNode expChild_Country = _attDataGrpNode.AppendChild(_doc.CreateElement("BkgSvcAttributeData"));
                            expChild_Country.AppendChild(_doc.CreateElement("BkgAttributeGroupMappingID")).InnerText = Convert.ToString(attributeGroupMappingIdForCountry);
                            expChild_Country.AppendChild(_doc.CreateElement("Value")).InnerText = Convert.ToString("UNITED STATES");
                        }
                        //UAT-2100:
                        Boolean isArmedForceStateExist = IsArmedForcesStateExist(_attrGrp, attributeGroupMappingIdForState, attributeGroupMappingIdForCounty);
                        foreach (var _attrData in _attrGrp.FormData)
                        {
                            XmlNode expChild = _attDataGrpNode.AppendChild(_doc.CreateElement("BkgSvcAttributeData"));
                            expChild.AppendChild(_doc.CreateElement("BkgAttributeGroupMappingID")).InnerText = Convert.ToString(_attrData.Key);
                            expChild.AppendChild(_doc.CreateElement("Value")).InnerText = isArmedForceStateExist ? String.Empty : Convert.ToString(_attrData.Value);
                        }
                    }
                }
                if (_lstPackageServiceIds.IsNullOrEmpty())
                {
                    //New Change 21072016
                    if (!supplementOrderCart.LstSupplementServiceId.IsNullOrEmpty())
                    {
                        _lstPackageServiceIds = supplementOrderCart.LstSupplementServiceId;
                    }
                }

                //_lstSupplementOrderPricing = GetSupplementOrderPricingData(tenantID, masterOrderID, BkgOrderServiceUserID, _doc.OuterXml, supplementOrderCart.OrdPkgSvcGroupId, _lstPackageServiceIds);
                _lstSupplementOrderPricing = GetSupplementOrderPricingData(tenantID, masterOrderID, BkgOrderServiceUserID, _doc.OuterXml);

                _lstSupplementOrderPricing.ForEach(oli =>
                {
                    if (!oli.lstOrderLineItems.IsNullOrEmpty())
                    {
                        checkIfLinetItemsExists = true;
                    }
                });
            }

            //return checkIfLinetItemsExists;
            return new Tuple<Boolean, List<SupplementOrderServices>, List<Int32>>(checkIfLinetItemsExists, _lstSupplementOrderPricing, selectedSupplementServices);
        }

        /// <summary>
        /// Save Data in cart
        /// </summary>
        private static SupplementOrderCart SetSupplOrderDataInCartToFilterLocationSearch(Int32 tenantID, Int32 masterOrderID, List<SupplementServiceItemCustomForm> lstSupplementServiceCustomFormList,
                            List<Int32> selectedSupplementServices, List<SupplementAdditionalSearchContract> lstSSNResultForAdditionalSearch, Int32 attributeGroupMappingIdForAliasName, Int32 attributeGroupMappingIdForState, Int32 attributeGroupMappingIdForCounty)
        {
            SupplementOrderCart supplementOrderCartTemp = new SupplementOrderCart();
            List<SupplementOrderData> lstSupplementOrderData = GetSupplementOrderDataToFilterLocationSearch(tenantID, masterOrderID, lstSupplementServiceCustomFormList, lstSSNResultForAdditionalSearch, attributeGroupMappingIdForAliasName, attributeGroupMappingIdForState, attributeGroupMappingIdForCounty);

            if (supplementOrderCartTemp.lstSupplementOrderData.IsNullOrEmpty())
            {
                supplementOrderCartTemp.lstSupplementOrderData = new List<SupplementOrderData>();
            }
            supplementOrderCartTemp.lstSupplementOrderData.AddRange(lstSupplementOrderData);

            //supplementOrderCartTemp.OrdPkgSvcGroupId = this.OrderPkgSvcGroupId;
            //supplementOrderCartTemp.ParentScreen = this.ParentScreen;
            //ucApplicantData.lstSupplementServiceCustomFormList = lstSupplementServiceCustomFormList;
            //ucApplicantData.supplementOrderCartTemp = supplementOrderCartTemp;

            supplementOrderCartTemp.LstSupplementServiceId = selectedSupplementServices;

            return supplementOrderCartTemp;
        }

        /// <summary>
        /// Get Supplement Order data To Filter Search data
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="masterOrderID"></param>
        /// <param name="lstSupplementServiceCustomFormList"></param>
        /// <param name="lstMatchedAdditionalSearchData"></param>
        /// <param name="attributeGroupMappingIdForAliasName"></param>
        /// <param name="attributeGroupMappingIdForState"></param>
        /// <param name="attributeGroupMappingIdForCounty"></param>
        /// <returns></returns>
        private static List<SupplementOrderData> GetSupplementOrderDataToFilterLocationSearch(Int32 tenantID, Int32 masterOrderID, List<SupplementServiceItemCustomForm> lstSupplementServiceCustomFormList,
                                List<SupplementAdditionalSearchContract> lstSSNResultForAdditionalSearch, Int32 attributeGroupMappingIdForAliasName, Int32 attributeGroupMappingIdForState, Int32 attributeGroupMappingIdForCounty)
        {
            List<SupplementOrderData> lstSupplementOrderData = new List<SupplementOrderData>();

            if (!lstSupplementServiceCustomFormList.IsNullOrEmpty() && !lstSSNResultForAdditionalSearch.IsNullOrEmpty())
            {
                List<SupplementAdditionalSearchContract> lstMatchedNameForAdditionalSearch = lstSSNResultForAdditionalSearch.Where(cnd => cnd.IsNameUsedForSearch)
                                                                                       .DistinctBy(dst => new { dst.FirstName, dst.LastName }).ToList();

                List<SupplementAdditionalSearchContract> lstMatchedLocationForAdditionalSearch = lstSSNResultForAdditionalSearch.Where(cnd => cnd.IsLocationUsedForSearch)
                                                                                        .DistinctBy(dst => new { dst.StateAbbreviation, dst.CountyName }).ToList();

                foreach (var serviceItemCustomForm in lstSupplementServiceCustomFormList)
                {
                    var lstCustomFormAttributes = BackgroundProcessOrderManager.GetListOfAttributesForSelectedItem(tenantID, serviceItemCustomForm.CustomFormID, serviceItemCustomForm.ServiceItemID);

                    if (!lstCustomFormAttributes.IsNullOrEmpty() && lstCustomFormAttributes.Count > 0 && lstCustomFormAttributes.Any(x => x.IsDisplay))
                    {
                        serviceItemCustomForm.lstCustomFormAttributes = lstCustomFormAttributes;
                        List<Int32> groupIds = lstCustomFormAttributes.DistinctBy(x => x.AttributeGroupId).OrderBy(x => x.Sequence).Select(x => x.AttributeGroupId).ToList();

                        for (int i = 0; i < groupIds.Count; i++)
                        {
                            //if it is location atrribute group then get supplement order form data to filter Service Item Additional Search
                            if (lstCustomFormAttributes.Any(x => x.IsDisplay && x.AttributeGroupId == groupIds[i]) && lstCustomFormAttributes.Any(x => x.AtrributeGroupMappingId == attributeGroupMappingIdForState))
                            {
                                var locationSearchData = GetLocationSearchData(lstMatchedLocationForAdditionalSearch, attributeGroupMappingIdForState, attributeGroupMappingIdForCounty,
                                        serviceItemCustomForm.ServiceId, serviceItemCustomForm.PackageServiceId, serviceItemCustomForm.CustomFormID, serviceItemCustomForm.ServiceItemID, groupIds[i]);

                                lstSupplementOrderData.AddRange(locationSearchData);
                            }
                            //if it is alias atrribute group then get supplement order form data
                            if (lstCustomFormAttributes.Any(x => x.IsDisplay && x.AttributeGroupId == groupIds[i]) && lstCustomFormAttributes.Any(x => x.AtrributeGroupMappingId == attributeGroupMappingIdForAliasName))
                            {
                                var aliasNameSearchData = GetAliasNameSearchData(lstMatchedNameForAdditionalSearch, attributeGroupMappingIdForAliasName, serviceItemCustomForm.ServiceId,
                                                            serviceItemCustomForm.PackageServiceId, serviceItemCustomForm.CustomFormID, serviceItemCustomForm.ServiceItemID, groupIds[i]);

                                lstSupplementOrderData.AddRange(aliasNameSearchData);
                            }
                        }
                    }
                }
            }
            return lstSupplementOrderData;
        }

        /// <summary>
        /// Get Location Search Data
        /// </summary>
        /// <param name="lstMatchedLocationForAdditionalSearch"></param>
        /// <param name="attributeGroupMappingIdForState"></param>
        /// <param name="attributeGroupMappingIdForCounty"></param>
        /// <param name="serviceID"></param>
        /// <param name="packageServiceId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private static List<SupplementOrderData> GetLocationSearchData(List<SupplementAdditionalSearchContract> lstMatchedLocationForAdditionalSearch, Int32 attributeGroupMappingIdForState,
                                                                       Int32 attributeGroupMappingIdForCounty, Int32 serviceID, Int32 packageServiceId, Int32 customFormId, Int32 serviceItemID, Int32 groupId)
        {
            List<SupplementOrderData> lstResult = new List<SupplementOrderData>();
            //if (lstCustomFormAttributes.Any(x => x.AtrributeGroupMappingId == attributeGroupMappingIdForState))
            //{
            if (lstMatchedLocationForAdditionalSearch.Count() > 0)
            {
                Int32 count = 1;
                foreach (var matchedLocationForAdditionalSearch in lstMatchedLocationForAdditionalSearch)
                {
                    SupplementOrderData supplementOrderData = new SupplementOrderData();
                    supplementOrderData.InstanceId = count;
                    supplementOrderData.BkgServiceId = serviceID;
                    supplementOrderData.PackageServiceId = packageServiceId;
                    supplementOrderData.CustomFormId = customFormId;
                    supplementOrderData.PackageSvcItemId = serviceItemID;
                    supplementOrderData.BkgSvcAttributeGroupId = groupId;
                    supplementOrderData.FormData = new Dictionary<int, string>();
                    SetLocationFormData(matchedLocationForAdditionalSearch, attributeGroupMappingIdForState, attributeGroupMappingIdForCounty, supplementOrderData.FormData);

                    lstResult.Add(supplementOrderData);
                    count++;

                }
            }
            //}
            return lstResult;
        }

        /// <summary>
        /// Set Location Form Data
        /// </summary>
        /// <param name="matchedLocationForAdditionalSearch"></param>
        /// <param name="attributeGroupMappingIdForState"></param>
        /// <param name="attributeGroupMappingIdForCounty"></param>
        /// <param name="formDataDetails"></param>
        private static void SetLocationFormData(SupplementAdditionalSearchContract matchedLocationForAdditionalSearch, Int32 attributeGroupMappingIdForState,
                                    Int32 attributeGroupMappingIdForCounty, Dictionary<Int32, String> formDataDetails)
        {
            if (matchedLocationForAdditionalSearch.IsLocationUsedForSearch)
            {
                if (!matchedLocationForAdditionalSearch.StateName.IsNullOrEmpty())
                    formDataDetails.Add(attributeGroupMappingIdForState, matchedLocationForAdditionalSearch.StateName);
                if (!matchedLocationForAdditionalSearch.CountyName.IsNullOrEmpty())
                    formDataDetails.Add(attributeGroupMappingIdForCounty, matchedLocationForAdditionalSearch.CountyName);
            }
        }

        /// <summary>
        /// Get Alias Name Search Data
        /// </summary>
        /// <param name="lstMatchedNameForAdditionalSearch"></param>
        /// <param name="attributeGroupMappingIdForAliasName"></param>
        /// <param name="serviceID"></param>
        /// <param name="packageServiceId"></param>
        /// <param name="customFormId"></param>
        /// <param name="serviceItemID"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private static List<SupplementOrderData> GetAliasNameSearchData(List<SupplementAdditionalSearchContract> lstMatchedNameForAdditionalSearch, Int32 attributeGroupMappingIdForAliasName,
                                                                       Int32 serviceID, Int32 packageServiceId, Int32 customFormId, Int32 serviceItemID, Int32 groupId)
        {
            List<SupplementOrderData> lstResult = new List<SupplementOrderData>();
            //if (lstCustomFormAttributes.Any(x => x.AtrributeGroupMappingId == attributeGroupMappingIdForState))
            //{
            if (lstMatchedNameForAdditionalSearch.Count() > 0)
            {
                Int32 count = 1;
                foreach (var matchedNameForAdditionalSearch in lstMatchedNameForAdditionalSearch)
                {
                    SupplementOrderData supplementOrderData = new SupplementOrderData();
                    supplementOrderData.InstanceId = count;
                    supplementOrderData.BkgServiceId = serviceID;
                    supplementOrderData.PackageServiceId = packageServiceId;
                    supplementOrderData.CustomFormId = customFormId;
                    supplementOrderData.PackageSvcItemId = serviceItemID;
                    supplementOrderData.BkgSvcAttributeGroupId = groupId;
                    supplementOrderData.FormData = new Dictionary<int, string>();
                    SetAliasNameFormData(matchedNameForAdditionalSearch, attributeGroupMappingIdForAliasName, supplementOrderData.FormData);

                    lstResult.Add(supplementOrderData);
                    count++;

                }
            }
            //}
            return lstResult;
        }

        /// <summary>
        /// Set Alias Name Form Data
        /// </summary>
        /// <param name="matchedNameForAdditionalSearch"></param>
        /// <param name="attributeGroupMappingIdForAliasName"></param>
        /// <param name="formDataDetails"></param>
        private static void SetAliasNameFormData(SupplementAdditionalSearchContract matchedNameForAdditionalSearch, Int32 attributeGroupMappingIdForAliasName, Dictionary<Int32, String> formDataDetails)
        {
            if (matchedNameForAdditionalSearch.IsNameUsedForSearch)
            {
                formDataDetails.Add(attributeGroupMappingIdForAliasName, matchedNameForAdditionalSearch.FirstName + " " + _noMiddleNameText + " " + matchedNameForAdditionalSearch.LastName);
            }
        }

        /// <summary>
        /// Get List of Custom Forms for Selected Services
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="selectedSupplementServices"></param>
        /// <returns></returns>
        private static List<SupplementServiceItemCustomForm> GetListOfCustomFormsForSelectedServices(Int32 tenantID, List<Int32> selectedSupplementServices)
        {
            String supplementalServiceIds = GetCommaSeperatedStringOfSelectedSupplementServices(selectedSupplementServices);

            //UAT-2369 : Color review issue
            if (!supplementalServiceIds.IsNullOrEmpty())
            {
                return BackgroundProcessOrderManager.GetListOfCustomFormsForSelectedServices(tenantID, supplementalServiceIds);
            }
            else
            {
                return new List<SupplementServiceItemCustomForm>();
            }
            //var LstDistinctCustomFormId = View.lstSupplementServiceCustomFormList.DistinctBy(col => col.CustomFormID).Select(x => x.CustomFormID).ToList();
        }

        /// <summary>
        /// Get Comma Seperated String of Selected Supplement Services
        /// </summary>
        /// <param name="selectedSupplementServices"></param>
        /// <returns></returns>
        private static String GetCommaSeperatedStringOfSelectedSupplementServices(List<Int32> selectedSupplementServices)
        {
            String supplementServices = String.Empty;
            if (!selectedSupplementServices.IsNullOrEmpty())
            {
                selectedSupplementServices.ForEach(x => supplementServices += Convert.ToString(x) + ",");
                if (supplementServices.EndsWith(","))
                    supplementServices = supplementServices.Substring(0, supplementServices.Length - 1);
            }
            return supplementServices;
        }

        /// <summary>
        /// Is Armed Forces State Exist
        /// </summary>
        /// <param name="suppOrderData"></param>
        /// <param name="attributeGroupMappingIdForState"></param>
        /// <param name="attributeGroupMappingIdForCounty"></param>
        /// <returns></returns>
        private static Boolean IsArmedForcesStateExist(SupplementOrderData suppOrderData, Int32 attributeGroupMappingIdForState, Int32 attributeGroupMappingIdForCounty)
        {
            if (!suppOrderData.FormData.IsNullOrEmpty() && suppOrderData.FormData.ContainsKey(attributeGroupMappingIdForState) && suppOrderData.FormData.Any(cnd => cnd.Value.ToLower().Contains(("Armed Forces").ToLower())))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get Supplement Order Pricing Data
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="inputXML"></param>
        /// <param name="ordPkgSvcGroupId"></param>
        /// <param name="lstPackageServiceIds"></param>
        /// <returns></returns>
        private static List<SupplementOrderServices> GetSupplementOrderPricingData(Int32 tenantID, Int32 masterOrderID, Int32 currentUserId, String inputXML)
        {
            SupplementOrderContract _supplementOrdContract = new SupplementOrderContract();
            _supplementOrdContract.CreatedById = currentUserId;
            _supplementOrdContract.OrderId = masterOrderID;
            String _pricingXML = StoredProcedureManagers.GetSupplementOrderPricingData(inputXML, tenantID);
            return ParseOutputXML(_pricingXML);
        }

        /// <summary>
        /// Parse the output received from the Supplement order pricing stored procedure
        /// </summary>
        /// <param name="outputXML"></param>
        /// <returns></returns>
        private static List<SupplementOrderServices> ParseOutputXML(String outputXML)
        {
            XDocument _docToParse = XDocument.Parse(outputXML);

            // GET <Service> TAG'S INSIDE <Services> TAG
            var _services = _docToParse.Root.Descendants("Services")
                               .Descendants("Service")
                               .Select(element => element)
                               .ToList();

            List<SupplementOrderServices> _lstData = new List<SupplementOrderServices>();
            foreach (var _service in _services)
            {

                Int32 _serviceId = Convert.ToInt32(_service.Element("ServiceID").Value);
                SupplementOrderServices _supplementService = new SupplementOrderServices();
                //_supplementService.ServiceId = _serviceId;

                #region ADD DATA OF <OrderLineItem> TAG'S INSIDE <OrderLineItems> TAG

                var _orderLineItems = _service.Descendants("OrderLineItems").Descendants("OrderLineItem")
                                         .Select(element => element)
                                         .ToList();

                _supplementService.lstOrderLineItems = new List<SupplementOrderOrderLineItem_PricingData>();

                foreach (var _ordLineItem in _orderLineItems)
                {
                    SupplementOrderOrderLineItem_PricingData _orderLineItem = new SupplementOrderOrderLineItem_PricingData();
                    _orderLineItem.PackageOrderItemPriceId = String.IsNullOrEmpty(_ordLineItem.Element("PackageOrderItemPriceID").Value)
                                                            ? AppConsts.NONE
                                                            : Convert.ToInt32(_ordLineItem.Element("PackageOrderItemPriceID").Value);

                    _orderLineItem.PackageSvcGrpID = String.IsNullOrEmpty(_ordLineItem.Element("PackageSvcGrpID").Value)
                                                           ? AppConsts.NONE
                                                           : Convert.ToInt32(_ordLineItem.Element("PackageSvcGrpID").Value);

                    _orderLineItem.PackageServiceItemId = Convert.ToInt32(_ordLineItem.Element("PackageServiceItemID").Value);
                    _orderLineItem.Price = String.IsNullOrEmpty(_ordLineItem.Element("Price").Value)
                                           ? AppConsts.NONE
                                           : Convert.ToDecimal(_ordLineItem.Element("Price").Value);

                    _orderLineItem.TotalPrice = String.IsNullOrEmpty(_ordLineItem.Element("TotalPrice").Value)
                                                ? AppConsts.NONE
                                                : Convert.ToDecimal(_ordLineItem.Element("TotalPrice").Value);

                    _orderLineItem.PriceDescription = _ordLineItem.Element("PriceDescription").Value;
                    _orderLineItem.LineItemDescription = _ordLineItem.Element("Description").Value;

                    #region ADD DATA OF <Fee> TAG'S INSIDE  <Fees> TAG

                    var _fees = _ordLineItem.Descendants("Fees").Descendants("Fee")
                                                 .Select(element => element)
                                                 .ToList();

                    _orderLineItem.lstFees = new List<SupplementOrderFee_PricingData>();
                    foreach (var _fee in _fees)
                    {
                        _orderLineItem.lstFees.Add(new SupplementOrderFee_PricingData
                        {
                            Amount = String.IsNullOrEmpty(_fee.Element("Amount").Value) ? AppConsts.NONE : Convert.ToDecimal(_fee.Element("Amount").Value),
                            Description = _fee.Element("Description").Value,
                            PackageOrderItemFeeId = String.IsNullOrEmpty(_fee.Element("PackageOrderItemFeeID").Value)
                                                    ? AppConsts.NONE
                                                    : Convert.ToInt32(_fee.Element("PackageOrderItemFeeID").Value)

                        });
                    }

                    #endregion

                    #region ADD DATA OF <BkgSvcAttributeDataGroup> TAG

                    var _bkgAttrDataGrps = _ordLineItem.Descendants("BkgSvcAttributeDataGroup")
                                                                   .Select(element => element)
                                                                   .ToList();

                    _orderLineItem.lstBkgSvcAttributeDataGroup = new List<SupplementOrderBkgSvcAttributeDataGroup_PricingData>();
                    foreach (var _bkgAttrDataGrp in _bkgAttrDataGrps)
                    {
                        String _instanceId = _bkgAttrDataGrp.Element("InstanceID").Value;

                        SupplementOrderBkgSvcAttributeDataGroup_PricingData _bkgSvcAttrDataGrpPricingData = new SupplementOrderBkgSvcAttributeDataGroup_PricingData
                        {
                            AttributeGroupId = Convert.ToInt32(_bkgAttrDataGrp.Element("AttributeGroupID").Value),
                            InstanceId = _instanceId
                        };

                        //if (String.IsNullOrEmpty(_instanceId))
                        var _attributeData = _bkgAttrDataGrp.Descendants("BkgSvcAttributes").Descendants("BkgSvcAttributeData")
                                                      .Select(element => element)
                                                      .ToList();

                        _bkgSvcAttrDataGrpPricingData.lstAttributeData = new List<SupplementOrderAttributeData_PricingData>();
                        foreach (var _attrData in _attributeData)
                        {
                            #region ADD DATA OF BkgSvcAttributeData TAG

                            String _attributeGrpMappingId = _attrData.Element("AttributeGroupMapingID").Value;

                            if (!String.IsNullOrEmpty(_attributeGrpMappingId))
                            {
                                _bkgSvcAttrDataGrpPricingData.lstAttributeData.Add(new SupplementOrderAttributeData_PricingData
                                {
                                    AttributeGroupMappingID = Convert.ToInt32(_attributeGrpMappingId),
                                    AttributeValue = _attrData.Element("Value").Value
                                });
                            }

                            #endregion
                        }

                        _orderLineItem.lstBkgSvcAttributeDataGroup.Add(_bkgSvcAttrDataGrpPricingData);
                    }
                    #endregion

                    _supplementService.lstOrderLineItems.Add(_orderLineItem);
                }
                #endregion

                _lstData.Add(_supplementService);
            }
            return _lstData;
        }


        #region UAT-2399: If there are no red lines in SSN trace results and additional searches are added by the system, add the searches automatically without need for review.
        /// <summary>
        /// Method to add the supplement order automatically for those whose additional searches will be created.
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="currentUserId">currentUserId</param>
        /// <param name="masterOrderId">masterOrderId</param>
        /// <param name="lstSupplementOrderPricing">lstSupplementOrderPricing</param>
        /// <param name="ordPkgSvcGroupId">ordPkgSvcGroupId</param>
        /// <param name="selectedSupplementServices">selectedSupplementServices</param>
        private static void AddAdditionalSearchesForOrderServiceGroup(Int32 tenantId, Int32 currentUserId, Int32 masterOrderId, List<SupplementOrderServices> lstSupplementOrderPricing
                                                                        , Int32 ordPkgSvcGroupId, List<Int32> selectedSupplementServices)
        {
            BkgOrderPackageSvcGroup orderPkgSvcGroupData = BackgroundProcessOrderManager.GetOrderPackageServiceGroupData(tenantId, ordPkgSvcGroupId);

            if (!orderPkgSvcGroupData.IsNullOrEmpty() && !orderPkgSvcGroupData.lkpBkgSvcGrpReviewStatusType.IsNullOrEmpty()
                 && orderPkgSvcGroupData.lkpBkgSvcGrpReviewStatusType.BSGRS_ReviewCode == BkgSvcGrpReviewStatusType.FIRST_REVIEW.GetStringValue())
            {
                Dictionary<Int32, Boolean> randomlySelectedSvcGroup = new Dictionary<Int32, Boolean>();
                String selectedSvcGroupID = Convert.ToString(ordPkgSvcGroupId);
                randomlySelectedSvcGroup = BackgroundProcessOrderManager.CheckSupplementAutoSvcGrpForRandonReview(tenantId, selectedSvcGroupID);

                //List<SupplementServicesInformation> lstSupplementServiceList = BackgroundProcessOrderManager.GetSupplementServices(masterOrderId, tenantId);
                //List<Int32> selectedSupplementServices = lstSupplementServiceList.Select(x => x.PackageServiceId).ToList();

                //SupplementOrderContract _supplementOrdContract = new SupplementOrderContract();
                //_supplementOrdContract.CreatedById = currentUserId;
                //_supplementOrdContract.OrderId = orderId;
                // String _pricingXML = StoredProcedureManagers.GetSupplementOrderPricingData(inputXML, tenantId);

                //List<SupplementOrderServices> _lstOutput = ParseOutputXML(_pricingXML);

                //bool checkIfLinetItemsExists = false;
                //_lstOutput.ForEach(oli =>
                //{
                //    if (!oli.lstOrderLineItems.IsNullOrEmpty())
                //    {
                //        checkIfLinetItemsExists = true;
                //    }
                //});


                //if (checkIfLinetItemsExists) // if any line item is created
                //{
                if (randomlySelectedSvcGroup.Any(x => x.Value))
                {
                    SupplementOrderContract _supplementOrderContract = new SupplementOrderContract
                    {
                        OrderId = masterOrderId,
                        CreatedById = currentUserId,
                        lstSupplementOrderData = lstSupplementOrderPricing,
                        OrderPkgSvcGroupId = ordPkgSvcGroupId
                    };
                    BackgroundProcessOrderManager.GenerateSupplementOrder(_supplementOrderContract, tenantId, selectedSupplementServices);
                }
                //return true;
                //}
                //else
                //{
                //    return false;
                //}
            }
        }
        #endregion
        #endregion

        #endregion

        #endregion

        #region UAT-1852: If a Service Group is complete, but other service groups are not complete within an order, the system should send the applicant an email.
        //private static List<EvServiceGroupMailContract> ConvertDataTableToContrat(DataTable table)
        //{
        //    try
        //    {
        //        IEnumerable<DataRow> rows = table.AsEnumerable();
        //        return rows.Select(x => new EvServiceGroupMailContract
        //        {
        //            OrderPackageSvceGrpID = x["PackageServiceGroupID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["PackageServiceGroupID"]),
        //            ServiceName = x["ServiceGroupName"].GetType().Name == "DBNull" ? String.Empty : x["ServiceGroupName"].ToString(),
        //            ApplicantName = x["ApplicantName"].GetType().Name == "DBNull" ? String.Empty : x["ApplicantName"].ToString(),
        //            PrimaryEmailaddress = x["PrimaryEmailaddress"].GetType().Name == "DBNull" ? String.Empty : x["PrimaryEmailaddress"].ToString(),
        //            OrganizationUserId = x["OrganizationUserId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["OrganizationUserId"]),
        //            IsCompleted = x["IsCompleted"] == DBNull.Value ? false : Convert.ToBoolean(x["IsCompleted"]),
        //            OrderNumber = x["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(x["OrderNumber"])

        //        }).ToList();
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}
        //private static void SendSeriveGroupIncompleteStatusMail(List<EvServiceGroupMailContract> svcGrpDetails, Int32 tenantId)
        //{
        //    String ApplicantName = svcGrpDetails[0].ApplicantName;
        //    String PrimaryEmailaddress = svcGrpDetails[0].PrimaryEmailaddress;
        //    Int32 OrganizationUserId = svcGrpDetails[0].OrganizationUserId;
        //    String OrderNumber = svcGrpDetails[0].OrderNumber;
        //    String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);
        //    String tenantName = SecurityManager.GetTenant(tenantId).TenantName;

        //    CommunicationSubEvents commSubEvent = CommunicationSubEvents.PENDING_SERVICE_GROUP_NOTIFICATION;

        //    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
        //    dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, ApplicantName);
        //    dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
        //    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
        //    dictMailData.Add(EmailFieldConstants.SERVICE_GROUP_NAME, GetMailContentForIncompleteSvcGrpStatus(svcGrpDetails, true));
        //    dictMailData.Add(EmailFieldConstants.Order_Number, OrderNumber);
        //    dictMailData.Add(EmailFieldConstants.PENDING_SERVICE_GROUP_NAME, GetMailContentForIncompleteSvcGrpStatus(svcGrpDetails, false));

        //    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
        //    mockData.UserName = ApplicantName;
        //    mockData.EmailID = PrimaryEmailaddress;
        //    mockData.ReceiverOrganizationUserID = OrganizationUserId;

        //    CommunicationManager.SendMailForIncompleteServiceGroup(commSubEvent, mockData, dictMailData, tenantId);
        //}
        //private static String GetMailContentForIncompleteSvcGrpStatus(List<EvServiceGroupMailContract> svcGrpDetails, Boolean IsCompleted)
        //{
        //    StringBuilder _sbSvcGrpDetails = new StringBuilder();
        //    foreach (var data in svcGrpDetails)
        //    {
        //        if (data.IsCompleted == IsCompleted)
        //        {
        //            _sbSvcGrpDetails.Append(data.ServiceName + ", ");
        //        }
        //    }
        //    return Convert.ToString(_sbSvcGrpDetails).Trim().TrimEnd(',');
        //}
        #endregion

        #region Method to Get Current processed order profile Aliase
        public static List<EvOrderProfileAliases> GetOrderProfileAliases(Int32 tenantID, Int32 orderID, Int32 organizationUSerProfileID, String noMiddleNameText)
        {
            try
            {
                List<EvOrderProfileAliases> lstFinalAliases = new List<EvOrderProfileAliases>();
                var result = BALUtils.GetEVOrderClientRepoInstance(tenantID).GetOrderProfileAliases(orderID, organizationUSerProfileID);

                if (!result.IsNullOrEmpty())
                {
                    result.ForEach(alias =>
                    {
                        String firstname = String.Empty;
                        String lastname = String.Empty;
                        String middleName = String.Empty;
                        firstname = alias.PAP_FirstName.IsNullOrEmpty() ? String.Empty : alias.PAP_FirstName.Trim();
                        lastname = alias.PAP_LastName.IsNullOrEmpty() ? String.Empty : alias.PAP_LastName.Trim();
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        if (!firstname.IsNullOrEmpty() && !lastname.IsNullOrEmpty())
                        {
                            middleName = alias.PAP_MiddleName.IsNullOrEmpty() ? String.Empty : alias.PAP_MiddleName.Trim();
                            middleName = middleName.Replace(noMiddleNameText, "");
                        }
                        EvOrderProfileAliases aliasData = new EvOrderProfileAliases();
                        aliasData.FirstName = firstname;
                        aliasData.LastName = lastname;
                        aliasData.MiddleName = middleName;
                        aliasData.FullName = firstname + " " + middleName + " " + lastname;
                        lstFinalAliases.Add(aliasData);
                    });
                }
                return lstFinalAliases.Where(x => x.FullName.Trim() != String.Empty).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

            }
            return new List<EvOrderProfileAliases>();
        }
        #endregion

        #region UAT-2337
        public static void UpdateEndDateForCurrentEmployer(List<OrderAttribute> orderAttribute, Int32 tenantID)
        {
            List<Int32> lstAttGrpMapIds = orderAttribute.Select(sel => sel.AttributeGroupMappingID).ToList();

            Dictionary<String, List<Int32>> result = BALUtils.GetEVOrderClientRepoInstance(tenantID).UpdateEndDateForCurrentEmployer(lstAttGrpMapIds);

            List<Int32> empEndDate = result["EmpEndDate"];
            List<Int32> isCurrentEmploye = result["IsCurrentEmploye"];

            if (!empEndDate.IsNullOrEmpty() && !isCurrentEmploye.IsNullOrEmpty())
            {
                foreach (var item in orderAttribute.DistinctBy(dis => dis.InstanceID))
                {
                    OrderAttribute AttributeCurrentEmploye = orderAttribute.Where(cond => isCurrentEmploye.Contains(cond.AttributeGroupMappingID)
                                                                                  && cond.InstanceID == item.InstanceID).FirstOrDefault();

                    OrderAttribute AttributeEmpEndDate = orderAttribute.Where(cond => empEndDate.Contains(cond.AttributeGroupMappingID)
                                                                               && cond.InstanceID == item.InstanceID).FirstOrDefault();

                    if (!AttributeCurrentEmploye.IsNullOrEmpty() && !AttributeEmpEndDate.IsNullOrEmpty())
                    {
                        if (!AttributeCurrentEmploye.AttributeValue.IsNullOrEmpty() && Convert.ToBoolean(AttributeCurrentEmploye.AttributeValue))
                        {
                            AttributeEmpEndDate.AttributeValue = AppConsts.EMPLOYE_CURRENT_END_DATE;
                        }
                    }
                }
            }
        }
        #endregion

        #region //UAT-2893:Update country code sent to clearstar for international criminals
        public static List<EvCountryLookup> GetCountriesLookupData()
        {
            try
            {
                List<EvCountryLookup> lstCountries = new List<EvCountryLookup>();
                var result = SecurityManager.GetCountries();

                if (!result.IsNullOrEmpty())
                {
                    result.ForEach(country =>
                    {
                        EvCountryLookup countryData = new EvCountryLookup();
                        countryData.CountryID = country.CountryID;
                        countryData.ClearStarCountryID = country.ClearStarCountryID.HasValue ? country.ClearStarCountryID.Value : AppConsts.NONE;
                        countryData.CompleteName = country.CompleteName;
                        countryData.FullName = country.FullName;
                        countryData.ShortName = country.ShortName;
                        countryData.Alpha2Code = country.Alpha2Code;
                        countryData.ISO3Code = country.ISO3Code;
                        countryData.PrintScanCode = country.PrintScanCode;
                        lstCountries.Add(countryData);
                    });
                }
                return lstCountries.Where(x => x.FullName.Trim() != String.Empty).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

            }
            return new List<EvCountryLookup>();
        }
        #endregion


        public static BkgOrderPackageSvcLineItem GetBkgOrderPackageSvcLineItem(Int32 tenantID, Int32 bkgOrderPackageSvcLineItemID)
        {
            try
            {
                return BALUtils.GetEVOrderClientRepoInstance(tenantID).GetBkgOrderPackageSvcLineItem(bkgOrderPackageSvcLineItemID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

            }
            return new BkgOrderPackageSvcLineItem();
        }


        #region Admin Entry Portal
        private static Boolean IsAdminEntryPortalOrder(BkgOrder bkgOrder)
        {
            Boolean isAdminEntryPortalOrder = false;
            try
            {
                if (!bkgOrder.BkgAdminEntryOrderDetails.IsNullOrEmpty() && bkgOrder.BkgAdminEntryOrderDetails.Any(cnd=>!cnd.BAEOD_IsDeleted))
                {
                    isAdminEntryPortalOrder = true;
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

            }
            return isAdminEntryPortalOrder;
        }
        
        #endregion

    }
}
