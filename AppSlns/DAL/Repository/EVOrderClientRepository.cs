#region Header Comment Block

// 
// Copyright 2014 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  EVOrderClientRepository.cs
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
using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;


#endregion

#endregion

namespace DAL.Repository
{
    public class EVOrderClientRepository : ClientBaseRepository, IEVOrderClientRepository
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ADB_LibertyUniversity_ReviewEntities _dbContext;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public EVOrderClientRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets list of OrderLineItems from database which need to be dispatched to vendor.
        /// </summary>
        /// <returns>Collection of OrderLineItems</returns>
        List<OrderToBeDisptached> IEVOrderClientRepository.GetOrdersToBeDispatchedToVendor(Int32 chunkSize, Int32 maxRetryCount, Int32 retryTimeLag,Boolean isTestMode)
        {
            return _dbContext.usp_GetOrdersToBeDispatchedToVendor(chunkSize, maxRetryCount, retryTimeLag,isTestMode).ToList();
        }

        Boolean IEVOrderClientRepository.SaveExtSvcInvokeHistory(ExtSvcInvokeHistory extSvcInvokeHistory)
        {
            _dbContext.ExtSvcInvokeHistories.AddObject(extSvcInvokeHistory);
            _dbContext.SaveChanges();
            return true;
        }

        BkgOrder IEVOrderClientRepository.GetBkgOrder(Int32 bkgOrderID)
        {
            return _dbContext.BkgOrders.Where(cond => cond.BOR_ID == bkgOrderID && cond.BOR_IsDeleted == false).FirstOrDefault();
        }

        BkgOrderPackageSvcLineItem IEVOrderClientRepository.GetBkgOrderPackageSvcLineItem(Int32 bkgOrderPackageSvcLineItemID)
        {
            return _dbContext.BkgOrderPackageSvcLineItems.Where(cond => cond.PSLI_ID == bkgOrderPackageSvcLineItemID && cond.PSLI_IsDeleted == false).FirstOrDefault();
        }

        Boolean IEVOrderClientRepository.SaveVendorOrderData(List<ExtVendorReport> extVendorReports)
        {
            extVendorReports.ForEach(report => _dbContext.ExtVendorReports.AddObject(report));
            _dbContext.SaveChanges();
            return true;
        }

        Boolean IEVOrderClientRepository.UpdateVendorData()
        {
            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Returns a collection of Test Mode Background order IDs.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        List<Int32> IEVOrderClientRepository.GetTestModeBkgOrders()
        {
            return _dbContext.TestModeVendorOrders.Where
                (cond => cond.TMVO_OrderID.HasValue && cond.TMVO_IsDispatched.HasValue ? !cond.TMVO_IsDispatched.Value : false)
                .Select(col => col.TMVO_OrderID.Value).ToList();
        }

        void IEVOrderClientRepository.UpdateRetryCountForBkgOrderLineItems(List<Int32> bkgOrderLineItemIDs)
        {
            IQueryable<BkgOrderPackageSvcLineItem> bkgOrderLineItems = _dbContext.BkgOrderPackageSvcLineItems.Where(cond => bkgOrderLineItemIDs.Contains(cond.PSLI_ID));
            bkgOrderLineItems.ForEach(lineItem =>
            {
                lineItem.PSLI_LastDispatchRetryDate = DateTime.Now;
                lineItem.PSLI_DispatchRetryCount = lineItem.PSLI_DispatchRetryCount.IsNull() ? AppConsts.ONE : lineItem.PSLI_DispatchRetryCount + AppConsts.ONE;
            });
            _dbContext.SaveChanges();
        }

        List<usp_GetOrdersToBeUpdatedVendorData_Result> IEVOrderClientRepository.GetOrdersToBeUpdatedVendorData(Int32 chunkSize, Int32 startingOrderID,
                                                                                                                Int32 endingOrderID, Int32 recentDays, Int32 updateOrderRetryTimeLag)
        {
            return _dbContext.GetOrdersToBeUpdatedVendorData(chunkSize, startingOrderID, endingOrderID, recentDays, updateOrderRetryTimeLag).ToList();
        }

        ExtVendorBkgOrderLineItemDetail IEVOrderClientRepository.GetExtVendorBkgOrderLineItemDetail(Int32 extVendorBkgOrderLineItemDetailID)
        {
            return _dbContext.ExtVendorBkgOrderLineItemDetails.Where(cond => cond.OLID_ID == extVendorBkgOrderLineItemDetailID && cond.OLID_IsDeleted == false).FirstOrDefault();
        }

        ExternalVendorBkgOrderDetail IEVOrderClientRepository.GetExternalVendorBkgOrderDetail(Int32 externalVendorBkgOrderDetailID)
        {
            return _dbContext.ExternalVendorBkgOrderDetails.Where(cond => cond.EVOD_ID == externalVendorBkgOrderDetailID && cond.EVOD_IsDeleted == false).FirstOrDefault();
        }

        IEnumerable<OrderAttribute> IEVOrderClientRepository.GetAttributesForOrder(Int32 orderID)
        {
            return _dbContext.usp_GetAttributesForOrder(orderID).AsEnumerable();
        }

        IEnumerable<BkgOrderLineItemDataMapping> IEVOrderClientRepository.GetBkgOrderLineItemDataMappings(IEnumerable<Int32> bkgOrderLineItemIDs)
        {
            return _dbContext.BkgOrderLineItemDataMappings.Include("BkgSvcAttributeGroup").Include("BkgOrderLineItemDataUsedAttrbs").Where(cond =>
                cond.OLIDM_BkgOrderPackageSvcLineItemID.HasValue
                 ? bkgOrderLineItemIDs.Contains(cond.OLIDM_BkgOrderPackageSvcLineItemID.Value)
                : false
                );
        }

        String IEVOrderClientRepository.GetCCFDocument(Int32 organizationUserProfileID)
        {
            String recordTypeCode = RecordType.BackgroundProfile.GetStringValue();
            String docTypeCode = DocumentType.EDS_AuthorizationForm.GetStringValue();
            List<GenericDocumentMapping> docMappings = _dbContext.GenericDocumentMappings.Include("ApplicantDocument.lkpDocumentType").Where(cond => cond.GDM_RecordID == organizationUserProfileID
                                                        && cond.lkpRecordType.Code == recordTypeCode).ToList();


            if (docMappings.IsNotNull() && docMappings.Count > AppConsts.NONE)
            {
                ApplicantDocument document = docMappings.Where(cond => cond.ApplicantDocument.IsNotNull()
                                                                            && cond.ApplicantDocument.lkpDocumentType.DMT_Code == docTypeCode)
                                                                            .Select(col => col.ApplicantDocument).FirstOrDefault();
                if (document.IsNotNull())
                {
                    return document.DocumentPath;
                }

            }

            return String.Empty;
        }

        void IEVOrderClientRepository.CompleteOrderServiceForms(Int32 bkgOrderID)
        {
            _dbContext.usp_CompleteBkgOrderServiceForms(bkgOrderID);
        }

        BkgOrderPackageSvcGroup IEVOrderClientRepository.GetBkgOrderPkgSvcGroupDetail(Int32 bkgOrderPkgSvcGroupDetailID)
        {
            return _dbContext.BkgOrderPackageSvcGroups.Where(cond => cond.OPSG_ID == bkgOrderPkgSvcGroupDetailID && cond.OPSG_IsDeleted == false).FirstOrDefault();
        }

        List<BkgOrderPackageSvcGroup> IEVOrderClientRepository.GetBkgOrderPkgSvcGroupByOrderID(List<Int32> bopIds)
        {
            return _dbContext.BkgOrderPackageSvcGroups.Where(cnd => bopIds.Contains(cnd.OPSG_BkgOrderPackageID) && cnd.OPSG_IsDeleted == false).ToList();
        }

        /// <summary>
        /// To check if any items are pending of a external vendor order service group
        /// </summary>
        /// <param name="organizationUserProfileID"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        Boolean IEVOrderClientRepository.CheckPendingSvcGroupItems(Int32 externalVendorOrderDetailID)
        {
            var result = _dbContext.usp_CheckPendingSvcGroupItems(externalVendorOrderDetailID).FirstOrDefault();
            if (result.HasValue)
            {
                return result.Value;
            }
            return false;
        }

        #region Create JIRA ticket

        IEnumerable<usp_GetOrdersToBeCreateJiraTicket_Result> IEVOrderClientRepository.GetOrdersToBeCreateJiraTicket(Int32 chunkSize, Int32 maxRetryCount,
                                                                                                                    Int32 createJiraTicketTimeLag, Int32 retryCountTimeLag)
        {

            return _dbContext.usp_GetOrdersToBeCreateJiraTicket(chunkSize, maxRetryCount, createJiraTicketTimeLag, retryCountTimeLag);

        }

        List<ExtSvcInvokeHistory> IEVOrderClientRepository.GetExtVendorUploadErrorListFromExtSvcInvokeHistory(Int32 bkgOrderID,
                                                                                        IEnumerable<usp_GetOrdersToBeCreateJiraTicket_Result> orderToBeCreateJiraTicket)
        {

            List<ExtSvcInvokeHistory> extVendorUploadErrorList = new List<ExtSvcInvokeHistory>();
            List<Int32> orderLineItemIds = orderToBeCreateJiraTicket.Select(col => col.OrderLineItemID).ToList();

            ExtSvcInvokeHistory createProfileInvokeHistory = _dbContext.ExtSvcInvokeHistories.Where(cond => cond.ESIH_BkgOrderID == bkgOrderID)
                                                        .OrderByDescending(cond => cond.ESIH_CreatedOn).FirstOrDefault();
            if (createProfileInvokeHistory.IsNotNull())
            {
                extVendorUploadErrorList.Add(createProfileInvokeHistory);
            }

            //IEnumerable<ExtSvcInvokeHistory> orderLineItemsData = _dbContext.ExtSvcInvokeHistories
            //                                                     .Where(cond => cond.ESIH_BkgOrderPackageSvcLineItemID.HasValue ?
            //                                                             (orderLineItemIds.Contains(cond.ESIH_BkgOrderPackageSvcLineItemID.Value)) : false
            //                                                             && cond.ESIH_MethodName == "AddOrderToProfile")
            //                                                     .OrderByDescending(cond=>cond.ESIH_CreatedOn)
            //                                                     .TakeWhile(x => orderLineItemIds.Contains(x.ESIH_BkgOrderPackageSvcLineItemID.Value))                                                                 
            //                                                     .AsEnumerable();

            List<ExtSvcInvokeHistory> orderLineItemsData = _dbContext.ExtSvcInvokeHistories
                                                                 .GroupBy(x => x.ESIH_BkgOrderPackageSvcLineItemID)
                                                                 .Select(grp => new
                                                                 {
                                                                     ESIH_BkgOrderPackageSvcLineItemID = grp.Key,
                                                                     ESIH_CreatedOn = grp.Max(x => x.ESIH_CreatedOn)
                                                                 })
                                                                 .Join(_dbContext.ExtSvcInvokeHistories,
                                                                 u => new { u.ESIH_CreatedOn, u.ESIH_BkgOrderPackageSvcLineItemID },
                                                                 uir => new { uir.ESIH_CreatedOn, uir.ESIH_BkgOrderPackageSvcLineItemID },
                                                                 (u, uir) => new { u, uir })
                                                                 .Where(cond => cond.uir.ESIH_BkgOrderPackageSvcLineItemID.HasValue ?
                                                                        (orderLineItemIds.Contains(cond.uir.ESIH_BkgOrderPackageSvcLineItemID.Value)) : false
                                                                        && cond.uir.ESIH_MethodName == "AddOrderToProfile")
                                                                 .Select(x => x.uir)
                                                                 .ToList();

            extVendorUploadErrorList.AddRange(orderLineItemsData);
            return extVendorUploadErrorList;
        }

        Boolean IEVOrderClientRepository.CreateJiraTicketDetail(JiraTicketDetail jiraTicketDetail)
        {
            JiraTicketDetail jtd = _dbContext.JiraTicketDetails.Where(cond => cond.OrderID == jiraTicketDetail.OrderID && cond.IsDeleted == false).FirstOrDefault();
            if (jtd.IsNotNull())
            {
                jtd.IsDeleted = false;
                jtd.ModifiedByID = jiraTicketDetail.CreatedByID;
                jtd.ModifiedOn = DateTime.Now;
            }

            _dbContext.AddToJiraTicketDetails(jiraTicketDetail);
            _dbContext.SaveChanges();
            return true;
        }

        Boolean IEVOrderClientRepository.UpdateBkgOrderIgnoredCreateJiraTicket(Int32 currentUserID, Int32 bkgOrderId)
        {
            BkgOrder bkgOrder = _dbContext.BkgOrders.Where(cond => cond.BOR_ID == bkgOrderId && cond.BOR_IsDeleted == false).FirstOrDefault();
            if (bkgOrder.IsNotNull())
            {
                bkgOrder.BOR_IsIgnoredCreateTicket = true;
                bkgOrder.BOR_ModifiedByID = currentUserID;
                bkgOrder.BOR_ModifiedOn = DateTime.Now;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        #endregion

        #endregion

        #region UAT-1852: If a Service Group is complete, but other service groups are not complete within an order, the system should send the applicant an email.
        //DataTable IEVOrderClientRepository.GetserviceGroupDetailsForMail(Int32 orderPackageSvcGrpId, Int32 bkgOrderId)
        //{
        //    EntityConnection connection = _dbContext.Connection as EntityConnection;
        //    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
        //    {

        //        SqlCommand command = new SqlCommand("ams.usp_SeriveGroupDetailsForMail", con);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@orderPackageSvcGrpId", orderPackageSvcGrpId);
        //        command.Parameters.AddWithValue("@BkgOrderId", bkgOrderId);

        //        SqlDataAdapter adp = new SqlDataAdapter();
        //        adp.SelectCommand = command;
        //        DataSet ds = new DataSet();
        //        adp.Fill(ds);
        //        if (ds.Tables.Count > 0)
        //        {
        //            return ds.Tables[0];
        //        }
        //    }
        //    return new DataTable();
        //}
        #endregion

        #region Method to Get Current processed order profile Aliase
        List<PersonAliasProfile> IEVOrderClientRepository.GetOrderProfileAliases(Int32 orderID, Int32 organizationUSerProfileID)
        {
            return _dbContext.PersonAliasProfiles.Where(x => x.PAP_OrganizationUserProfileID == organizationUSerProfileID && !x.PAP_IsDeleted).ToList();
        }
        #endregion

        #region UAT-2337
        Dictionary<String, List<Int32>> IEVOrderClientRepository.UpdateEndDateForCurrentEmployer(List<Int32> lstAttGrpMapIds)
        {
            Dictionary<String, List<Int32>> result = new Dictionary<String, List<Int32>>();
            var data = _dbContext.BkgAttributeGroupMappings
                                                        .Where(cond => lstAttGrpMapIds.Contains(cond.BAGM_ID) && !cond.BAGM_IsDeleted && !cond.BkgSvcAttribute.BSA_IsDeleted
                                                            && (cond.BkgSvcAttribute.BSA_Code == new Guid("7A0F4CC0-5416-48D1-AC9F-62A98F0D8606") || cond.BkgSvcAttribute.BSA_Code == new Guid("41BFF8A4-EC01-42C9-B6A6-778BAC34D488"))).ToList();

            List<Int32> EmpEndDateContract = new List<Int32>();
            List<Int32> IsCurrentEmployeContract = new List<Int32>();

            foreach (var item in data)
            {
                if (item.BkgSvcAttribute.BSA_Code == new Guid("7A0F4CC0-5416-48D1-AC9F-62A98F0D8606"))
                {
                    EmpEndDateContract.Add(item.BAGM_ID);
                }
                if (item.BkgSvcAttribute.BSA_Code == new Guid("41BFF8A4-EC01-42C9-B6A6-778BAC34D488"))
                {
                    IsCurrentEmployeContract.Add(item.BAGM_ID);
                }
            }
            result.Add("EmpEndDate", EmpEndDateContract);
            result.Add("IsCurrentEmploye", IsCurrentEmployeContract);
            return result;
        }
        #endregion

        #region UAT 3331
        Int32? IEVOrderClientRepository.GetExternalVendorResultResponseFormatTypeId(Int32 externalVendorOrderLineItemId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            Int32? ResultResponseFormatTypeId = null;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("ams.GetExternalVendorResultResponseFormat", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EVBOLID_ID", externalVendorOrderLineItemId);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ResultResponseFormatTypeId = ds.Tables[0].Rows[0]["ResultResponseFormatTypeId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(ds.Tables[0].Rows[0]["ResultResponseFormatTypeId"]);
                    }
                }
            }

            return ResultResponseFormatTypeId;
        }

        #endregion
    }
}
