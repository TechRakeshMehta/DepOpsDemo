using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.IntsofLoggerModel.Interface;
using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace DAL.Repository
{
    public class BackgroundProcessOrderRepository : ClientBaseRepository, IBackgroundProcessOrderRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _ClientDBContext;

        public BackgroundProcessOrderRepository(Int32 tenantId)
            : base(tenantId)
        {
            _ClientDBContext = base.ClientDBContext;
        }

        #region Backround Order Search
        /// <summary>
        /// Get the backround Order details based on the search parameters.
        /// </summary>
        /// <param name="gridCustomPaging">gridCustomPaging</param>
        /// <param name="bkgOrderSearchContract">bkgOrderSearchContract</param>
        /// <returns>List<BackroundOrderSearch></returns>
        public List<BackroundOrderSearch> GetBackroundOrderSearchDetail(CustomPagingArgsContract gridCustomPaging, BkgOrderSearchContract bkgOrderSearchContract)
        {
            string orderBy = QueueConstants.APPLICANT_SEARCH_DEFAULT_SORTING_FIELDS;
            string ordDirection = null;

            orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? null : "desc";

            // List<BackroundOrderSearch> backroundOrderSearchDeatil = _ClientDBContext.BackroundOrderSearchResult(bkgOrderSearchContract.OrderID, bkgOrderSearchContract.OrderFromDate, bkgOrderSearchContract.OrderToDate, bkgOrderSearchContract.PaidFromDate, bkgOrderSearchContract.PaidToDate,
            //  bkgOrderSearchContract.ApplicantFirstName, bkgOrderSearchContract.ApplicantLastName, bkgOrderSearchContract.ApplicantSSN, bkgOrderSearchContract.DateOfBirth, bkgOrderSearchContract.OrderPaymentStatusID, bkgOrderSearchContract.OrderStatusID, bkgOrderSearchContract.ServiceID, bkgOrderSearchContract.InstitutionStatusColorID, orderBy, ordDirection, gridCustomPaging.CurrentPageIndex, gridCustomPaging.PageSize).ToList();
            return null;
        }

        /// <summary>
        /// Check the backround order is dispatched to clearstar or not
        /// </summary>
        /// <param name="bkgOrderId">bkgOrderId</param>
        /// <returns>ExternalVendorBkgOrderDetail</returns>
        public ExternalVendorBkgOrderDetail GetExternalVendorBkgOrderDetail(Int32 bkgOrderId)
        {
            ExternalVendorBkgOrderDetail externalVendorBkgOrderDetail = _ClientDBContext.ExternalVendorBkgOrderDetails.Include("lkpDispatchStatu").Where(obj => obj.EVOD_BkgOrderID == bkgOrderId && obj.EVOD_IsDeleted == false).FirstOrDefault();
            if (externalVendorBkgOrderDetail.IsNotNull())
            {
                return externalVendorBkgOrderDetail;
            }
            return null;
        }

        public List<InstitutionOrderFlag> GetInstitutionStatusColor(Int32 tenantId)
        {
            if (tenantId > AppConsts.NONE)
            {

                List<InstitutionOrderFlag> institutionOrderFlag = _ClientDBContext.InstitutionOrderFlags.Include("lkpOrderFlag").Where(obj => obj.IOF_TenantID == tenantId && obj.IOF_IsDeleted == false).ToList();
                if (institutionOrderFlag.IsNotNull() && institutionOrderFlag.Count > 0)
                {
                    return institutionOrderFlag;
                }
            }
            return null;
        }
        List<Entity.ClientEntity.BkgSvcGroup> IBackgroundProcessOrderRepository.GetBackroundServiceGroup()
        {
            List<BkgSvcGroup> bkgSvcGroup = _ClientDBContext.BkgSvcGroups.Where(obj => !obj.BSG_IsDeleted).ToList();
            if (bkgSvcGroup.IsNotNull() && bkgSvcGroup.Count > 0)
            {
                return bkgSvcGroup;
            }
            return null;
        }
        List<Entity.ClientEntity.BkgOrderClientStatu> IBackgroundProcessOrderRepository.GetBkgOrderClientStatus(Int32 tenantId)
        {
            if (tenantId > AppConsts.NONE)
            {
                List<BkgOrderClientStatu> bkgOrderClientStatu = _ClientDBContext.BkgOrderClientStatus.Where(obj => !obj.BOCS_IsDeleted && obj.BOCS_InstitutionID == tenantId).ToList();
                if (bkgOrderClientStatu.IsNotNull() && bkgOrderClientStatu.Count > 0)
                {
                    return bkgOrderClientStatu.OrderBy(cond => cond.BOCS_DisplayOrder).ToList();
                }
            }
            return null;
        }

        Entity.ClientEntity.InstitutionOrderFlag IBackgroundProcessOrderRepository.GetOrderInstitutionStatusColor(Int32 institutionStatusColorId)
        {
            InstitutionOrderFlag institutionOrderFlag = _ClientDBContext.InstitutionOrderFlags.Include("lkpOrderFlag").Where(obj => obj.IOF_ID == institutionStatusColorId && obj.IOF_IsDeleted == false).FirstOrDefault();
            if (institutionOrderFlag.IsNotNull())
            {
                return institutionOrderFlag;
            }
            return null;
        }
        Int32 IBackgroundProcessOrderRepository.GetClientStatusByOrderId(Int32 orderId)
        {
            var clientStatus = _ClientDBContext.BkgOrders.Where(obj => obj.BOR_MasterOrderID == orderId && obj.BOR_IsDeleted == false).FirstOrDefault().BOR_BkgOrderClientStatus;
            if (clientStatus.IsNotNull())
            {
                return Convert.ToInt32(clientStatus);
            }
            return 0;
        }
        Boolean IBackgroundProcessOrderRepository.UpdateOrderClientStatus(Int32 orderId, Int32 ClientStatusId, Int32 currentLoggedInUserId)
        {
            BkgOrder bkgOrder = _ClientDBContext.BkgOrders.Where(obj => obj.BOR_MasterOrderID == orderId && obj.BOR_IsDeleted == false).FirstOrDefault();
            if (bkgOrder.IsNotNull())
            {
                bkgOrder.BOR_BkgOrderClientStatus = ClientStatusId;
                bkgOrder.BOR_ModifiedOn = DateTime.Now;
                bkgOrder.BOR_ModifiedByID = currentLoggedInUserId;
                if (_ClientDBContext.SaveChanges() > 0)
                    return true;
            }
            return false;
        }
        Boolean IBackgroundProcessOrderRepository.SaveOrderClientStatusHistory(Int32 orderId, String notes, Int32 currentLoggedInUserId)
        {
            Int32 bkgOrderId = _ClientDBContext.BkgOrders.Where(obj => obj.BOR_MasterOrderID == orderId && obj.BOR_IsDeleted == false).FirstOrDefault().BOR_ID;
            if (bkgOrderId.IsNotNull() && bkgOrderId > 0)
            {
                BkgOrderClientStatusHistory bkgOrderClientStatus = new BkgOrderClientStatusHistory();
                bkgOrderClientStatus.OCSH_OrderID = bkgOrderId;
                bkgOrderClientStatus.OCSH_Notes = notes;
                bkgOrderClientStatus.OCSH_IsDeleted = false;
                bkgOrderClientStatus.OCSH_CreatedOn = DateTime.Now;
                bkgOrderClientStatus.OCSH_CreatedByID = currentLoggedInUserId;
                _ClientDBContext.AddToBkgOrderClientStatusHistories(bkgOrderClientStatus);
                if (_ClientDBContext.SaveChanges() > 0)
                    return true;
                return false;
            }
            return false;
        }

        List<Entity.ClientEntity.BkgOrderClientStatusHistory> IBackgroundProcessOrderRepository.GetClientOrderStatusHistory(Int32 orderId)
        {
            Int32 bkgOrderId = _ClientDBContext.BkgOrders.Where(obj => obj.BOR_MasterOrderID == orderId && obj.BOR_IsDeleted == false).FirstOrDefault().BOR_ID;
            if (bkgOrderId.IsNotNull() && bkgOrderId > 0)
            {
                List<BkgOrderClientStatusHistory> bkgOrderClientStatus = _ClientDBContext.BkgOrderClientStatusHistories.Where(obj => obj.OCSH_OrderID == bkgOrderId && obj.OCSH_IsDeleted == false).ToList();
                if (bkgOrderClientStatus.IsNotNull())
                {
                    return bkgOrderClientStatus;
                }
            }
            return null;
        }

        BkgOrder IBackgroundProcessOrderRepository.GetBkgOrderDetail(Int32 masterOrderId)
        {
            BkgOrder bkgOrder = _ClientDBContext.BkgOrders.Where(obj => obj.BOR_MasterOrderID == masterOrderId && obj.BOR_IsDeleted == false).FirstOrDefault();
            if (bkgOrder.IsNotNull())
            {
                return bkgOrder;
            }
            return null;
        }
        Boolean IBackgroundProcessOrderRepository.UpdateBkgOrderArchiveStatus(Int32 masterOrderId, Boolean archiveStatus, String eventDetailNotes, Int32 loggedInUserId)
        {
            BkgOrderEventHistory bkgOrderEventHistory = new BkgOrderEventHistory();
            List<Entity.ClientEntity.lkpEventHistory> lstEvnetHistory = _ClientDBContext.lkpEventHistories.Where(obj => obj.EH_IsDeleted == false).ToList();
            if (lstEvnetHistory.IsNotNull() && lstEvnetHistory.Count > AppConsts.NONE)
            {
                Int32 orderArchiveEnventId = lstEvnetHistory.Where(obj => obj.EH_Code == "AAAE").FirstOrDefault().EH_ID;
                Int32 orderActiveEnventId = lstEvnetHistory.Where(obj => obj.EH_Code == "AAAF").FirstOrDefault().EH_ID;
                BkgOrder bkgOrder = _ClientDBContext.BkgOrders.Where(obj => obj.BOR_MasterOrderID == masterOrderId && obj.BOR_IsDeleted == false).FirstOrDefault();

                if (bkgOrder.IsNotNull())
                {
                    //Update archive status in Bkg order  
                    bkgOrder.BOR_IsArchived = archiveStatus;
                    bkgOrder.BOR_LastArchivedDate = DateTime.Now;

                    //Add new record in bkg event history
                    bkgOrderEventHistory.BOEH_BkgOrderID = bkgOrder.BOR_ID;
                    bkgOrderEventHistory.BOEH_OrderEventDetail = eventDetailNotes;
                    bkgOrderEventHistory.BOEH_EventHistoryId = archiveStatus == true ? orderArchiveEnventId : orderActiveEnventId;
                    bkgOrderEventHistory.BOEH_CreatedOn = DateTime.Now;
                    bkgOrderEventHistory.BOEH_CreatedByID = loggedInUserId;
                    _ClientDBContext.AddToBkgOrderEventHistories(bkgOrderEventHistory);
                }
                if (_ClientDBContext.SaveChanges() > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        #endregion


        #region Order Detail

        /// <summary>
        /// Method is used to get the Menu
        /// </summary>
        /// <param name="orderID">orderID</param>
        /// <returns></returns>
        OrderDetailMain IBackgroundProcessOrderRepository.GetOrderDetailMenuItem(Int32 orderID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("ams.usp_GetCustomFormsByOrderID", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@orderID", orderID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                OrderDetailMain objOrderDetailMain = new OrderDetailMain();


                objOrderDetailMain.OrderDetailMenuItem = ds.Tables[0].AsEnumerable().Select(col =>
                      new OrderDetailMenuItem
                      {
                          MenuID = Convert.ToInt32(col["MenuID"]),
                          MenuName = Convert.ToString(col["MenuName"]),
                          MenuToolTip = Convert.ToString(col["MenuDescription"])
                      }).ToList();


                objOrderDetailMain.OrderDetailHeaderInfo = ds.Tables[1].AsEnumerable().Select(col =>
                     new OrderDetailHeaderInfo
                     {
                         OrderID = Convert.ToInt32(col["OrderID"]),
                         DOB = Convert.ToString(col["DOB"]),
                         ApplicantName = Convert.ToString(col["ApplicantName"]),
                         Gender = Convert.ToString(col["GenderName"]),
                         PhoneNumber = col["PhoneNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["PhoneNumber"]),
                         StatusType = col["StatusType"] == DBNull.Value ? String.Empty : Convert.ToString(col["StatusType"]),
                         OrderDate = Convert.ToDateTime(col["OrderDate"]),
                         InstitutionColorStatus = col["InstitutionColorStatus"] == DBNull.Value ? String.Empty : Convert.ToString(col["InstitutionColorStatus"]),
                         InstitutionColorStatusID = col["InstitutionColorStatusID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["InstitutionColorStatusID"]),
                         PaymentType = Convert.ToString(col["PaymentType"]),
                         TotalPrice = col["TotalPrice"] == DBNull.Value ? AppConsts.NONE : Convert.ToDecimal(col["TotalPrice"]),
                         PaymentStatus = col["PaymentStatus"] == DBNull.Value ? String.Empty : Convert.ToString(col["PaymentStatus"]),
                         OrderNumber = col["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["OrderNumber"])
                     }).FirstOrDefault();


                return objOrderDetailMain;
            }
        }

        /// <summary>
        /// Method is used to get the Applicant Order Detail
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        ApplicantOrderDetail IBackgroundProcessOrderRepository.GetApplicantOrderDetail(Int32 orderID, Int32 tenantID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_BackroundOrderDetail", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", orderID);
                command.Parameters.AddWithValue("@TenantID", tenantID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                ApplicantOrderDetail objApplicantOrderDetail = new ApplicantOrderDetail();


                objApplicantOrderDetail.OrderDetailInfo = ds.Tables[0].AsEnumerable().Select(col =>
                      new OrderDetailInfo
                      {
                          OrderID = Convert.ToInt32(col["OrderID"]),
                          DOB = Convert.ToString(col["DOB"]),
                          ApplicantName = Convert.ToString(col["ApplicantName"]),
                          Gender = Convert.ToString(col["GenderName"]),
                          PhoneNumber = col["PhoneNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["PhoneNumber"]),
                          Address = Convert.ToString(col["CityName"]) + ',' + Convert.ToString(col["StateName"]) + ',' + Convert.ToString(col["ZipCode"]),
                          Email = Convert.ToString(col["PrimaryEmailAddress"]),
                          SSN = Convert.ToString(col["SSN"]),
                          OrderDate = Convert.ToDateTime(col["OrderDate"]),
                          InstitutionColorStatus = Convert.ToString(col["InstitutionColorStatus"]),
                          CompletedDate = col["OrderCompleteDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["OrderCompleteDate"]),
                          //PaymentType = col["PaymentType"] == DBNull.Value ? String.Empty : Convert.ToString(col["PaymentType"]),
                          TotalPrice = col["TotalPrice"] == DBNull.Value ? AppConsts.NONE : Convert.ToDecimal(col["TotalPrice"]),
                          InstitutionHierarchy = col["InstitutionHierarchy"] == DBNull.Value ? String.Empty : Convert.ToString(col["InstitutionHierarchy"]),
                          StatusType = col["StatusType"] == DBNull.Value ? String.Empty : Convert.ToString(col["StatusType"]),
                          //PaymentStatus = col["PaymentStatus"] == DBNull.Value ? String.Empty : Convert.ToString(col["PaymentStatus"]),
                          InstitutionColorStatusID = col["InstitutionColorStatuID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["InstitutionColorStatuID"]),
                          OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["OrganizationUserID"]),
                          IsInternationalPhoneNumber = col["IsInternationalPhoneNumber"] == DBNull.Value ? false : Convert.ToBoolean(col["IsInternationalPhoneNumber"]),
                          IsInternationalSecondaryPhone = col["IsInternationalSecondaryPhone"] == DBNull.Value ? false : Convert.ToBoolean(col["IsInternationalSecondaryPhone"])

                      }).FirstOrDefault();


                if (ds.Tables[1].Rows.Count > AppConsts.NONE)
                {
                    objApplicantOrderDetail.ApplicantAlias = ds.Tables[1].AsEnumerable().Select(col =>
                      new ApplicantAlias
                      {
                          ID = Convert.ToInt32(col["PAP_ID"]),
                          ApplicantAliasName = Convert.ToString(col["ApplicantAliasName"]),

                      }).ToList();
                }

                if (ds.Tables[2].Rows.Count > AppConsts.NONE)
                {
                    objApplicantOrderDetail.ServiceGroup = ds.Tables[2].AsEnumerable().Select(col =>
                      new ServiceGroup
                      {
                          BSG_ID = Convert.ToInt32(col["BSG_ID"]),
                          BSG_Name = Convert.ToString(col["BSG_Name"]),

                      }).ToList();
                }


                if (ds.Tables[3].Rows.Count > AppConsts.NONE)
                {
                    objApplicantOrderDetail.ExtVendorAccount = ds.Tables[3].AsEnumerable().Select(col =>
                      new ExtVendorAccount
                      {
                          EVOD_AccountNumber = Convert.ToString(col["EVOD_AccountNumber"]),

                      }).ToList();
                }

                if (ds.Tables[4].Rows.Count > AppConsts.NONE)
                {
                    objApplicantOrderDetail.OrderFlags = ds.Tables[4].AsEnumerable().Select(col =>
                      new OrderFlags
                      {
                          IOF_ID = Convert.ToInt32(col["IOF_ID"]),
                          //IOF_OrderFlagID = Convert.ToInt32(col["IOF_OrderFlagID"]),
                          OFL_FileName = Convert.ToString(col["OFL_FileName"]),
                          OFL_Tooltip = Convert.ToString(col["OFL_Tooltip"]),
                          OFL_FilePath = Convert.ToString(col["OFL_FilePath"])
                      }).ToList();
                }


                if (ds.Tables[5].Rows.Count > AppConsts.NONE)
                {
                    objApplicantOrderDetail.IsSupplement = Convert.ToBoolean(ds.Tables[5].Rows[0]["IsSupplement"]);
                }


                if (ds.Tables[6].Rows.Count > AppConsts.NONE)
                {
                    objApplicantOrderDetail.CFOD_Value = Convert.ToString(ds.Tables[6].Rows[0]["CFOD_Value"]);
                    objApplicantOrderDetail.BSAD_Name = ds.Tables[6].Rows.Count > AppConsts.ONE ? Convert.ToString(ds.Tables[6].Rows[1]["CFOD_Value"]) : "NA";
                }


                if (ds.Tables[7].Rows.Count > AppConsts.NONE)
                {
                    objApplicantOrderDetail.PaymentTypesAndStatus = ds.Tables[7].AsEnumerable().Select(col =>
                       new PaymentTypesAndStatus
                       {
                           PaymentOption = Convert.ToString(col["PaymentOption"]),
                           PaymentStatus = Convert.ToString(col["PaymentStatus"]),
                           PaymentStatusID = Convert.ToInt32(col["PaymentStatusID"]),
                           PaymentOptionID = Convert.ToInt32(col["PaymentOptionID"]),
                           OrderPaymentDetailID = Convert.ToInt32(col["OrderPaymentDetailID"]),
                           PaymentStatusCode = Convert.ToString(col["PaymentStatusCode"]),

                       }).ToList();
                }


                return objApplicantOrderDetail;
            }


        }

        /// <summary>
        /// To update Order Status
        /// </summary>
        /// <param name="selectedOrderColorStatusId"></param>
        /// <param name="orderID"></param>
        /// <param name="selectedOrderStatusTypeId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        Boolean IBackgroundProcessOrderRepository.UpdateOrderStatus(Int32 selectedOrderColorStatusId, Int32 orderID, Int32 selectedOrderStatusTypeId, Int32 loggedInUserId, Int32 orderPkgSvcGroupID, BkgOrderPackageSvcGroup bkgOrderPackageSvcGroup)
        {
            BkgOrder bkgOrder = _ClientDBContext.BkgOrders.Where(cond => cond.BOR_MasterOrderID == orderID).FirstOrDefault();
            String oldStatus = _ClientDBContext.lkpOrderStatusTypes.Where(cond => cond.OrderStatusTypeID == bkgOrder.BOR_OrderStatusTypeID).Select(col => col.StatusType).FirstOrDefault();
            lkpOrderStatusType lkpOrderStatusType = _ClientDBContext.lkpOrderStatusTypes.Where(cond => cond.OrderStatusTypeID == selectedOrderStatusTypeId).FirstOrDefault();
            String newStatus = lkpOrderStatusType.StatusType;
            String selectedOrderStatusTypeCode = lkpOrderStatusType.Code;
            String completedOrderLineResultStatusCode = BkgOrderLineItemDetailStatus.COMPLETED.GetStringValue();
            Int32 completedOrderLineResultStatusID = _ClientDBContext.lkpOrderLineItemResultStatus.FirstOrDefault(cond => cond.LIRS_Code
                                                                                           == completedOrderLineResultStatusCode).LIRS_ID;

            List<lkpBkgSvcGrpReviewStatusType> lkpBkgSvcGrpReviewStatusTypesObj = _ClientDBContext.lkpBkgSvcGrpReviewStatusTypes.Where(x => !x.BSGRS_IsDeleted).ToList();
            List<lkpBkgSvcGrpStatusType> bkgSvcGrpStatusTypesList = _ClientDBContext.lkpBkgSvcGrpStatusTypes.Where(x => !x.BSGS_IsDeleted).ToList();

            //UAT-1996:Setting to allow Client admins the ability to edit color flags
            String oldColorFlagStatus = String.Empty;
            String oldColorFlagStatusCode = String.Empty;
            String newColorFlagStatus = String.Empty;
            String newColorFlagStatusCode = String.Empty;

            if (bkgOrder.IsNotNull())
            {
                //UAT-2878
                String oldOrderStatus = bkgOrder.lkpOrderStatusType.IsNotNull() ? bkgOrder.lkpOrderStatusType.Code : String.Empty;

                bkgOrder.BOR_OrderStatusTypeID = selectedOrderStatusTypeId;

                //UAT-1996:Setting to allow Client admins the ability to edit color flags
                if (!bkgOrder.BOR_InstitutionStatusColorID.IsNullOrEmpty())
                {
                    oldColorFlagStatus = bkgOrder.InstitutionOrderFlag.lkpOrderFlag.OFL_Name;
                    oldColorFlagStatusCode = bkgOrder.InstitutionOrderFlag.lkpOrderFlag.OFL_Code;
                }

                if (String.Compare(oldOrderStatus, OrderStatusType.COMPLETED.GetStringValue(), true) != AppConsts.NONE
                    && selectedOrderStatusTypeCode == OrderStatusType.COMPLETED.GetStringValue()) //For Complete
                {
                    //CompleteOrderLineItemsWithServiceForms();
                    CompleteOrderLineItems(bkgOrder, completedOrderLineResultStatusCode, completedOrderLineResultStatusID);

                    if (selectedOrderColorStatusId != 0)
                    {
                        bkgOrder.BOR_InstitutionStatusColorID = selectedOrderColorStatusId;
                    }
                    bkgOrder.BOR_OrderCompleteDate = DateTime.Now;

                    String bkgSvcGrpReviewStatusTypeCode = BkgSvcGrpReviewStatusType.MANUAL_REVIEW_COMPLETED.GetStringValue();
                    Int32 bkgSvcGrpReviewStatusTypeID = lkpBkgSvcGrpReviewStatusTypesObj
                                                        .Where(cond => cond.BSGRS_ReviewCode == bkgSvcGrpReviewStatusTypeCode
                                                                       && !cond.BSGRS_IsDeleted).Select(col => col.BSGRS_ID)
                                                        .FirstOrDefault();

                    String bkgSvcGrpStatusTypeCode = BkgSvcGrpStatusType.COMPLETED.GetStringValue();
                    Int32 bkgSvcGrpStatusTypeID = bkgSvcGrpStatusTypesList.Where(cond => cond.BSGS_StatusCode == bkgSvcGrpStatusTypeCode).Select(col => col.BSGS_ID)
                                                                          .FirstOrDefault();

                    IEnumerable<Int32> bop_ids = bkgOrder.BkgOrderPackages.Where(cond => cond.BOP_IsDeleted == false).Select(col => col.BOP_ID);
                    List<BkgOrderPackageSvcGroup> lstBkgOrderPackageSvcGroup = _ClientDBContext.BkgOrderPackageSvcGroups
                                                                               .Where(cond => bop_ids.Contains(cond.OPSG_BkgOrderPackageID)
                                                                                              && cond.OPSG_IsDeleted == false).ToList();

                    lstBkgOrderPackageSvcGroup.ForEach(bkgOrderPkgSvcGrp =>
                    {
                        bkgOrderPkgSvcGrp.OPSG_SvcGrpReviewStatusTypeID = bkgSvcGrpReviewStatusTypeID;
                        bkgOrderPkgSvcGrp.OPSG_SvcGrpStatusTypeID = bkgSvcGrpStatusTypeID;
                        bkgOrderPkgSvcGrp.OPSG_SvcGrpLastUpdatedDate = DateTime.Now;
                        bkgOrderPkgSvcGrp.OPSG_SvcGrpCompletionDate = DateTime.Now;
                        bkgOrderPkgSvcGrp.OPSG_ModifiedOn = DateTime.Now;
                        bkgOrderPkgSvcGrp.OPSG_ModifiedByID = loggedInUserId;
                    });
                }
                else
                {
                    if (selectedOrderColorStatusId != 0)
                    {
                        bkgOrder.BOR_InstitutionStatusColorID = selectedOrderColorStatusId;
                    }
                    if (String.Compare(selectedOrderStatusTypeCode, OrderStatusType.COMPLETED.GetStringValue(), true) != AppConsts.NONE)
                    {
                        bkgOrder.BOR_OrderCompleteDate = (DateTime?)null;
                    }
                }
                bkgOrder.BOR_ModifiedByID = loggedInUserId;
                bkgOrder.BOR_ModifiedOn = DateTime.Now;
                if (!oldStatus.Equals(newStatus))
                {
                    String orderStatus = BkgOrderEvents.ORDER_UPDATED.GetStringValue();
                    BkgOrderEventHistory _bkgOrderEventHistory = new BkgOrderEventHistory
                    {
                        BOEH_BkgOrderID = bkgOrder.BOR_ID,
                        BOEH_OrderEventDetail = "Changed Order status from " + oldStatus + " to " + newStatus + "",
                        BOEH_IsDeleted = false,
                        BOEH_CreatedByID = loggedInUserId,
                        BOEH_CreatedOn = DateTime.Now,
                        BOEH_EventHistoryId = _ClientDBContext.lkpEventHistories.Where(cond => cond.EH_Code == orderStatus).Select(col => col.EH_ID).FirstOrDefault()
                    };
                    _ClientDBContext.BkgOrderEventHistories.AddObject(_bkgOrderEventHistory);
                }

                #region UAT-1996:Setting to allow Client admins the ability to edit color flags
                //Capture Order color flag change history.
                if (selectedOrderColorStatusId != 0)
                {
                    var instOrderFlag = _ClientDBContext.InstitutionOrderFlags.FirstOrDefault(cond => cond.IOF_ID == selectedOrderColorStatusId && cond.IOF_IsDeleted == false);
                    if (!instOrderFlag.IsNullOrEmpty())
                    {
                        newColorFlagStatus = instOrderFlag.lkpOrderFlag.OFL_Name;
                        newColorFlagStatusCode = instOrderFlag.lkpOrderFlag.OFL_Code;
                    }
                    if (!oldColorFlagStatusCode.Equals(newColorFlagStatusCode))
                    {
                        String orderStatus = BkgOrderEvents.ORDER_UPDATED.GetStringValue();
                        BkgOrderEventHistory _bkgOrderEventHistory = new BkgOrderEventHistory
                        {
                            BOEH_BkgOrderID = bkgOrder.BOR_ID,
                            BOEH_OrderEventDetail = !oldColorFlagStatus.IsNullOrEmpty() ? "Changed Color flag from " + oldColorFlagStatus + " to " + newColorFlagStatus + ""
                                                                                       : "Applied Color flag " + newColorFlagStatus + "",
                            BOEH_IsDeleted = false,
                            BOEH_CreatedByID = loggedInUserId,
                            BOEH_CreatedOn = DateTime.Now,
                            BOEH_EventHistoryId = _ClientDBContext.lkpEventHistories.Where(cond => cond.EH_Code == orderStatus).Select(col => col.EH_ID).FirstOrDefault()
                        };
                        _ClientDBContext.BkgOrderEventHistories.AddObject(_bkgOrderEventHistory);
                    }
                }
                #endregion

                #region UAT-844
                BkgOrderPackageSvcGroup bkgOrderPackageSvcGroupObj = _ClientDBContext.BkgOrderPackageSvcGroups.Where(cond => cond.OPSG_ID == orderPkgSvcGroupID).FirstOrDefault();
                String svcGroupOldReviewStatus = "";
                String svcGroupNewReviewStatus = "";
                String svcGroupOldStatus = "";
                String svcGroupNewStatus = "";
                if (bkgOrderPackageSvcGroupObj.IsNotNull())
                {
                    svcGroupOldReviewStatus = lkpBkgSvcGrpReviewStatusTypesObj.Where(x => x.BSGRS_ID == bkgOrderPackageSvcGroupObj.OPSG_SvcGrpReviewStatusTypeID)
                                                                              .Select(x => x.BSGRS_ReviewStatusType).FirstOrDefault().ToString();
                    //svcGroupNewStatus = lkpBkgSvcGrpReviewStatusTypesObj.Where(x => x.BSGRS_ReviewCode == manualReviewCompletedCode).Select(x => x.BSGRS_ReviewStatusType).FirstOrDefault().ToString();

                    svcGroupNewReviewStatus = lkpBkgSvcGrpReviewStatusTypesObj.Where(x => x.BSGRS_ID == bkgOrderPackageSvcGroup.OPSG_SvcGrpReviewStatusTypeID)
                                                                              .Select(x => x.BSGRS_ReviewStatusType).FirstOrDefault().ToString();

                    svcGroupOldStatus = bkgSvcGrpStatusTypesList.Where(x => x.BSGS_ID == bkgOrderPackageSvcGroupObj.OPSG_SvcGrpStatusTypeID)
                                                                .Select(col => col.BSGS_StatusType).FirstOrDefault().ToString();

                    svcGroupNewStatus = bkgSvcGrpStatusTypesList.Where(x => x.BSGS_ID == bkgOrderPackageSvcGroup.OPSG_SvcGrpStatusTypeID)
                                                                .Select(col => col.BSGS_StatusType).FirstOrDefault().ToString();
                }

                if (bkgOrderPackageSvcGroupObj.IsNotNull())
                {
                    bkgOrderPackageSvcGroupObj.OPSG_SvcGrpReviewStatusTypeID = bkgOrderPackageSvcGroup.OPSG_SvcGrpReviewStatusTypeID; //bkgOrderPackageSvcGroup.OPSG_SvcGrpReviewStatusTypeID > 0 ? bkgOrderPackageSvcGroup.OPSG_SvcGrpReviewStatusTypeID : bkgOrderPackageSvcGroupObj.OPSG_SvcGrpReviewStatusTypeID;
                    bkgOrderPackageSvcGroupObj.OPSG_SvcGrpStatusTypeID = bkgOrderPackageSvcGroup.OPSG_SvcGrpStatusTypeID;
                    bkgOrderPackageSvcGroupObj.OPSG_SvcGrpCompletionDate = bkgOrderPackageSvcGroup.OPSG_SvcGrpCompletionDate; //DateTime.Now;
                    bkgOrderPackageSvcGroupObj.OPSG_ModifiedByID = loggedInUserId;
                    bkgOrderPackageSvcGroupObj.OPSG_ModifiedOn = DateTime.Now;
                    bkgOrderPackageSvcGroupObj.OPSG_SvcGrpLastUpdatedDate = DateTime.Now;
                }


                #region Maintaing Order Event History for Service Group Review changed status

                String orderUpdatedstatusCode = BkgOrderEvents.ORDER_UPDATED.GetStringValue();
                Int32 eventHistoryId = _ClientDBContext.lkpEventHistories.Where(cond => cond.EH_Code == orderUpdatedstatusCode).Select(col => col.EH_ID).FirstOrDefault();

                if (svcGroupOldReviewStatus != svcGroupNewReviewStatus)
                {
                    var historyData = String.Format("Changed Background Service Group: {0} review status from {1} to {2}", bkgOrderPackageSvcGroupObj.BkgSvcGroup.BSG_Name
                                                    , svcGroupOldReviewStatus, svcGroupNewReviewStatus);
                    BkgOrderEventHistory _bkgOrderEventHistoryObj = new BkgOrderEventHistory
                    {
                        BOEH_BkgOrderID = bkgOrder.BOR_ID,
                        BOEH_OrderEventDetail = historyData,
                        BOEH_IsDeleted = false,
                        BOEH_CreatedByID = loggedInUserId,
                        BOEH_CreatedOn = DateTime.Now,
                        BOEH_EventHistoryId = eventHistoryId
                    };
                    _ClientDBContext.BkgOrderEventHistories.AddObject(_bkgOrderEventHistoryObj);
                }

                if (svcGroupOldStatus != svcGroupNewStatus)
                {
                    var historyData = String.Format("Changed Background Service Group: {0} status from {1} to {2}", bkgOrderPackageSvcGroupObj.BkgSvcGroup.BSG_Name
                                                    , svcGroupOldStatus, svcGroupNewStatus);
                    BkgOrderEventHistory _bkgOrderEventHistoryObj = new BkgOrderEventHistory
                    {
                        BOEH_BkgOrderID = bkgOrder.BOR_ID,
                        BOEH_OrderEventDetail = historyData,
                        BOEH_IsDeleted = false,
                        BOEH_CreatedByID = loggedInUserId,
                        BOEH_CreatedOn = DateTime.Now,
                        BOEH_EventHistoryId = eventHistoryId
                    };
                    _ClientDBContext.BkgOrderEventHistories.AddObject(_bkgOrderEventHistoryObj);
                }
                #endregion

                #endregion

                _ClientDBContext.SaveChanges();

                if (selectedOrderStatusTypeCode == OrderStatusType.COMPLETED.GetStringValue()) //For Complete
                {
                    _ClientDBContext.usp_CompleteBkgOrderServiceForms(bkgOrder.BOR_ID);
                }

                return true;
            }
            else
                return false;
        }

        private void CompleteOrderLineItems(BkgOrder bkgOrder, String completedOrderLineResultStatusCode, Int32 completedOrderLineResultStatusID)
        {
            List<ExtVendorBkgOrderLineItemDetail> inCompleteBkgOrderLineItemDetails = _ClientDBContext.ExtVendorBkgOrderLineItemDetails
                                                                                                     .Include("ExternalVendorBkgOrderDetail")
                                                                                                     .Where(cond =>
                                                                                                     (cond.ExternalVendorBkgOrderDetail.EVOD_BkgOrderID
                                                                                                     .Equals(bkgOrder.BOR_ID))
                                                                                                     &&
                                                                                                     cond.OLID_OrderLineItemResultStatusID.HasValue
                                                                                                     &&
                                                                                                     (cond.OLID_OrderLineItemResultStatusID.Value != completedOrderLineResultStatusID
                                                                                                     )).ToList();

            inCompleteBkgOrderLineItemDetails.ForEach(lineItemDetail =>
            {
                lineItemDetail.OLID_OrderLineItemResultStatusID = completedOrderLineResultStatusID;
                if (lineItemDetail.OLID_DateCompleted.IsNull())
                {
                    lineItemDetail.OLID_DateCompleted = DateTime.Now;
                }
            });
        }

        /// <summary>
        /// Method to Get Notes By OrderId
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        DataTable IBackgroundProcessOrderRepository.GetNotesByOrderId(Int32 orderID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetOrderNotes", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", orderID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > AppConsts.NONE)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        /// <summary>
        /// Method to add note
        /// </summary>
        /// <param name="orderNote"></param>
        /// <returns></returns>
        Boolean IBackgroundProcessOrderRepository.AddNote(OrderNote orderNote)
        {
            _ClientDBContext.OrderNotes.AddObject(orderNote);
            _ClientDBContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Method is used to Get Package By OrderId
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        List<PackageDetailsContract> IBackgroundProcessOrderRepository.GetPackageByOrderId(Int32 orderID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetPackagesOrderByOrderID", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", orderID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                List<PackageDetailsContract> objPackageDetailsContract = new List<PackageDetailsContract>();
                objPackageDetailsContract = ds.Tables[0].AsEnumerable().Select(col =>
                      new PackageDetailsContract
                      {
                          BPA_Name = col["BPA_Name"] == DBNull.Value ? String.Empty : Convert.ToString(col["BPA_Name"]),
                          BPA_Description = col["BPA_Description"] == DBNull.Value ? String.Empty : Convert.ToString(col["BPA_Description"]),
                          PackagePrice = col["PackagePrice"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["PackagePrice"])
                      }).ToList();

                return objPackageDetailsContract;
            }
        }

        /// <summary>
        /// Get Bkg Order Custom Form Attributes Data
        /// </summary>
        /// <param name="masterOrderId">Master Order ID</param>
        /// <param name="customFormId">Custom Form ID</param>
        /// <returns></returns>
        BkgOrderDetailCustomFormDataContract IBackgroundProcessOrderRepository.GetBkgOrderCustomFormAttributesData(Int32 masterOrderId, Int32 customFormId)
        {
            BkgOrderDetailCustomFormDataContract customFormDataContract = null;
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[ams].[usp_GetOrderDetailControlsAndValues]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@customFormID", customFormId);
                command.Parameters.AddWithValue("@OrderID", masterOrderId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 1)
                {
                    customFormDataContract = new BkgOrderDetailCustomFormDataContract();
                    customFormDataContract.lstCustomFormAttributes = SetAttributesForCustomForm(ds.Tables[0]);
                    customFormDataContract.lstDataForCustomForm = SetDataForTheCustomForm(ds.Tables[1]);
                }
            }
            return customFormDataContract;
        }

        /// <summary>
        /// Get Organisation User Profile By OrderId
        /// </summary>
        /// <param name="masterOrderId"></param>
        /// <returns></returns>
        OrganizationUserProfile IBackgroundProcessOrderRepository.GetOrganisationUserProfileByOrderId(Int32 masterOrderId)
        {
            var order = _ClientDBContext.Orders.FirstOrDefault(cond => cond.OrderID == masterOrderId && cond.IsDeleted == false);
            if (order.IsNotNull() && order.OrganizationUserProfile.IsNotNull())
            {
                return order.OrganizationUserProfile;
            }
            return null;
        }

        /// <summary>
        /// Method is used to get the ServiceLine Price by OrderID
        /// </summary>
        /// <param name="orderID">orderID</param>
        /// <returns>List of BackroundOrderServiceLinePrice</returns>
        OrderServiceLineItemPriceInfo IBackgroundProcessOrderRepository.GetBackroundOrderServiceLinePriceByOrderID(Int32 orderID, List<Int32> Bkg_PkgIDs)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[ams].[usp_BackroundOrderServiceLinePrice]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", orderID);
                if (!Bkg_PkgIDs.IsNullOrEmpty())
                {
                    String Bkg_PkgID = String.Join(",", Bkg_PkgIDs);
                    command.Parameters.AddWithValue("@BkgPkgIDs", Bkg_PkgID);
                }

                //}
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                OrderServiceLineItemPriceInfo objOrderServiceLineItemPriceInfo = new OrderServiceLineItemPriceInfo();


                if (ds.Tables.Count > AppConsts.NONE)
                {
                    if (ds.Tables[0].Rows.Count > AppConsts.NONE)
                    {
                        objOrderServiceLineItemPriceInfo.BkgOrderPkg = ds.Tables[0].AsEnumerable().Select(col =>
                       new BkgOrderPackageInfo
                       {
                           BkgPackagePrice = col["BOP_BasePrice"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(col["BOP_BasePrice"]),
                           BkgPackageName = Convert.ToString(col["PackageName"]),
                           BkgPackageLabel = Convert.ToString(col["PackageLabel"]) //UAT-3481

                       }).ToList();
                    }


                    if (ds.Tables[1].Rows.Count > AppConsts.NONE)
                    {
                        objOrderServiceLineItemPriceInfo.BOSLPrice = ds.Tables[1].AsEnumerable().Select(col =>
                       new BackroundOrderServiceLinePrice
                       {
                           BackgroundServiceID = Convert.ToInt32(col["BSE_ID"]),
                           BackgroundServiceName = Convert.ToString(col["BSE_Name"]),
                           Amount = col["Amount"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(col["Amount"]),
                           AdjAmount = col["AdjAmount"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(col["AdjAmount"]),
                           NetAmount = Convert.ToDecimal(col["NetAmount"]),
                           Description = Convert.ToString(col["PricingInformation"])
                       }).ToList();
                    }

                    if (ds.Tables[2].Rows.Count > AppConsts.NONE)
                    {
                        DataRow dRow = ds.Tables[2].Rows[0];
                        objOrderServiceLineItemPriceInfo.CompliancePackageName = dRow["PackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dRow["PackageName"]);
                        objOrderServiceLineItemPriceInfo.CompliancePkgAmount = dRow["CompliancePkgAmount"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(dRow["CompliancePkgAmount"]);
                        objOrderServiceLineItemPriceInfo.CompliancePkgTotalAmount = dRow["TotalAmount"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(dRow["TotalAmount"]);

                    }



                }

                return objOrderServiceLineItemPriceInfo;


            }
            return null;
        }

        /// <summary>
        /// Method is used to Get External Vendor Services By OrderId
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        List<ExternalVendorServiceContract> IBackgroundProcessOrderRepository.GetExternalVendorServicesByOrderId(Int32 orderID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetExtVendorServiceLinesByOrderID", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", orderID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                List<ExternalVendorServiceContract> externalVendorServiceContract = new List<ExternalVendorServiceContract>();
                externalVendorServiceContract = ds.Tables[0].AsEnumerable().Select(col =>
                      new ExternalVendorServiceContract
                      {
                          BOR_ID = Convert.ToInt32(col["BOR_ID"]),
                          PSLI_ID = Convert.ToInt32(col["PSLI_ID"]),
                          BSE_Name = Convert.ToString(col["BSE_Name"]),
                          VendorStatus = Convert.ToString(col["VendorStatus"]),
                          Flagged = Convert.ToBoolean(col["Flagged"]),
                          FlaggedText = Convert.ToString(col["FlaggedText"]),
                          VendorCode = Convert.ToString(col["VendorCode"]),
                          VendorOrderID = Convert.ToString(col["VendorOrderID"]),
                          EVOD_VendorProfileID = Convert.ToString(col["EVOD_VendorProfileID"])
                      }).ToList();

                return externalVendorServiceContract;
            }
        }

        /// <summary>
        /// Method is used to Get Bkg Order Line Item Details
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        OrderLineDetailsContract IBackgroundProcessOrderRepository.GetBkgOrderLineItemDetails(Int32 PSLI_ID)
        {
            OrderLineDetailsContract oderLineDetailsContract = new OrderLineDetailsContract();
            oderLineDetailsContract.BkgOrderLineItemResultCopy = _ClientDBContext.BkgOrderLineItemResultCopies.Where(cond => cond.OLIR_BkgOrderPackageSvcLineItemID == PSLI_ID).FirstOrDefault();
            oderLineDetailsContract.ExtVendorBkgOrderLineItemDetails = _ClientDBContext.ExtVendorBkgOrderLineItemDetails.Where(cond => cond.OLID_BkgOrderPackageSvcLineItemID == PSLI_ID).FirstOrDefault();
            return oderLineDetailsContract;
        }
        /// <summary>
        /// Method is used to Update Record To ADBCopy
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        Boolean IBackgroundProcessOrderRepository.UpdateRecordToADBCopy(BkgOrderLineItemResultCopy bkgOrderLineItemResultCopy, Int32 currentLoggedInUserId)
        {
            BkgOrderLineItemResultCopy objBkgOrderLineItemResultCopy = _ClientDBContext.BkgOrderLineItemResultCopies.Where(cond => cond.OLIR_BkgOrderPackageSvcLineItemID == bkgOrderLineItemResultCopy.OLIR_BkgOrderPackageSvcLineItemID).FirstOrDefault();
            if (objBkgOrderLineItemResultCopy.IsNull())
            {
                _ClientDBContext.BkgOrderLineItemResultCopies.AddObject(bkgOrderLineItemResultCopy);
            }
            else
            {
                objBkgOrderLineItemResultCopy.OLIR_BkgOrderPackageSvcLineItemID = bkgOrderLineItemResultCopy.OLIR_BkgOrderPackageSvcLineItemID;
                objBkgOrderLineItemResultCopy.OLIR_ResultText = bkgOrderLineItemResultCopy.OLIR_ResultText;
                objBkgOrderLineItemResultCopy.OLIR_FlaggedInd = bkgOrderLineItemResultCopy.OLIR_FlaggedInd;
                objBkgOrderLineItemResultCopy.OLIR_OrderLineItemResultStatusID = bkgOrderLineItemResultCopy.OLIR_OrderLineItemResultStatusID;
                objBkgOrderLineItemResultCopy.OLIR_ModifiedByID = currentLoggedInUserId;
                objBkgOrderLineItemResultCopy.OLIR_ModifiedDate = DateTime.Now;
                if (objBkgOrderLineItemResultCopy.OLIR_DateCompleted.IsNotNull())
                {
                    objBkgOrderLineItemResultCopy.OLIR_DateCompleted = bkgOrderLineItemResultCopy.OLIR_DateCompleted;
                }
            }
            _ClientDBContext.SaveChanges();
            return true;
        }
        #endregion

        #region Order Notification

        /// <summary>
        /// To Get Background Order Notification Data
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <returns>DataTable</returns>
        DataTable IBackgroundProcessOrderRepository.GetBkgOrderNotificationData(Int32 chunkSize, String orderIds)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetBkgOrderNotificationData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                command.Parameters.AddWithValue("@OrderIDs", orderIds);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        /// <summary>
        /// To Get Background Order Notification Data
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <returns>DataTable</returns>
        DataTable IBackgroundProcessOrderRepository.GetBkgOrderResultCompletedNotificationData(Int32 chunkSize)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetBkgOrderCompletedResultData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }


        String IBackgroundProcessOrderRepository.GetBGPkgPDFAttachementStatus(Int32 hierarchyNodeID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetBGPkgPDFAttachementSettting", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DPM_ID", hierarchyNodeID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["DPM_PdfInclusionSetting"].ToString();
                }
                else
                {
                    return PDFInclusionOptions.Default.GetStringValue();
                }
            }
            //var BGPkgPDFAttachementStatus = _ClientDBContext.DeptProgramMappings.Where(cond => cond.DPM_ID == hierarchyNodeID && !cond.DPM_IsDeleted && cond.DPM_BGPDFInclusionID != null).FirstOrDefault();

            //if (BGPkgPDFAttachementStatus == null || BGPkgPDFAttachementStatus.lkpPDFInclusionOption.Code.Equals(PDFInclusionOptions.Not_Specified.GetStringValue()))
            //{
            //    var ParentNodeSetting = _ClientDBContext.DeptProgramMappings.Where(cond => cond.DPM_InstitutionNodeID == 1 && !cond.DPM_IsDeleted && cond.DPM_BGPDFInclusionID != null).FirstOrDefault();
            //    return ParentNodeSetting.lkpPDFInclusionOption.IsDeleted == false ? ParentNodeSetting.lkpPDFInclusionOption.Code : string.Empty;
            //}
            //else
            //    return BGPkgPDFAttachementStatus.lkpPDFInclusionOption.IsDeleted == false ? BGPkgPDFAttachementStatus.lkpPDFInclusionOption.Code : PDFInclusionOptions.Default.GetStringValue();
        }

        /// <summary>
        /// To Get Service Group Notification Data
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <returns>DataTable</returns>
        DataTable IBackgroundProcessOrderRepository.GetSvcGrpResultCompletedNotificationData(Int32 chunkSize)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetBkgServiceGroupCompletedResultData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        DataTable IBackgroundProcessOrderRepository.GetFlaggedSvcGrpResultCompletedNotificationData(Int32 chunkSize)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetFlaggedBkgServiceGroupCompletedResultData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        /// <summary>
        /// Create Background Order Notification
        /// </summary>
        /// <param name="orderNotification"></param>
        /// <returns>OrderNotificationID</returns>
        Int32 IBackgroundProcessOrderRepository.CreateOrderNotification(OrderNotification orderNotification)
        {
            _ClientDBContext.OrderNotifications.AddObject(orderNotification);
            if (_ClientDBContext.SaveChanges() > 0)
                return orderNotification.ONTF_ID;
            else
                return 0;
        }

        /// <summary>
        /// Create Background Order Service Form
        /// </summary>
        /// <param name="bkgOrderServiceForm"></param>
        /// <returns>BkgOrderServiceFormID</returns>
        Int32 IBackgroundProcessOrderRepository.CreateBkgOrderServiceForm(BkgOrderServiceForm bkgOrderServiceForm)
        {
            _ClientDBContext.BkgOrderServiceForms.AddObject(bkgOrderServiceForm);
            if (_ClientDBContext.SaveChanges() > 0)
                return bkgOrderServiceForm.OSF_ID;
            else
                return 0;
        }

        /// <summary>
        /// Update Background Order Notify Status
        /// </summary>
        /// <param name="bkgOrder"></param>
        /// <returns>True/False</returns>
        Boolean IBackgroundProcessOrderRepository.UpdateBkgOrderNotifyStatus(BkgOrder bkgOrd)
        {
            BkgOrder bkgOrder = _ClientDBContext.BkgOrders.Where(cond => cond.BOR_ID == bkgOrd.BOR_ID && cond.BOR_IsDeleted == false).FirstOrDefault();

            bkgOrder.BOR_OrderNotifyStatusID = bkgOrd.BOR_OrderNotifyStatusID;
            bkgOrder.BOR_ModifiedByID = bkgOrd.BOR_ModifiedByID;
            bkgOrder.BOR_ModifiedOn = bkgOrd.BOR_ModifiedOn;

            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        #endregion

        #region Order Details for Client Admin

        List<OrderEventHistoryContract> IBackgroundProcessOrderRepository.GetOrderEventHistory(Int32 orderID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetBkgOrderEventHistory", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@orderId", orderID);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                List<OrderEventHistoryContract> orderEventHistoryContract = new List<OrderEventHistoryContract>();
                orderEventHistoryContract = ds.Tables[0].AsEnumerable().Select(col =>
                      new OrderEventHistoryContract
                      {
                          BOEH_ID = Convert.ToInt32(col["BOEH_ID"]),
                          BOEH_OrderEventDetail = Convert.ToString(col["BOEH_OrderEventDetail"]),
                          BOEH_CreatedOn = col["BOEH_CreatedOn"] == DBNull.Value ? String.Empty : Convert.ToString(col["BOEH_CreatedOn"]),
                          BOEH_FullName = col["FullName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FullName"]),
                      }).ToList();

                return orderEventHistoryContract;
            }

            //List<OrderEventHistoryContract> lstOrderEventHistoryContract = new List<OrderEventHistoryContract>();
            //List<BkgOrderEventHistory> lstBkgOrderEventHistory = _ClientDBContext.BkgOrderEventHistories.Include("BkgOrder").Where(cond => cond.BkgOrder.BOR_MasterOrderID == orderID).OrderBy(col => col.BOEH_CreatedOn).ToList();
            //foreach (var orderEvent in lstBkgOrderEventHistory)
            //{
            //    OrganizationUser orgUser = new OrganizationUser();
            //    orgUser = _ClientDBContext.OrganizationUsers.Where(x => x.OrganizationUserID == orderEvent.BOEH_CreatedByID && !x.IsDeleted).FirstOrDefault();
            //    OrderEventHistoryContract orderEventHistoryContract = new OrderEventHistoryContract();
            //    orderEventHistoryContract.BOEH_CreatedOn = Convert.ToString(orderEvent.BOEH_CreatedOn);
            //    orderEventHistoryContract.BOEH_ID = orderEvent.BOEH_ID;
            //    orderEventHistoryContract.BOEH_OrderEventDetail = orderEvent.BOEH_OrderEventDetail;
            //    if (orgUser.IsNotNull())
            //        orderEventHistoryContract.BOEH_FullName = orgUser.FirstName + " " + orgUser.MiddleName + " " + orgUser.LastName;
            //    lstOrderEventHistoryContract.Add(orderEventHistoryContract);
            //}
            //return lstOrderEventHistoryContract;
        }

        OrderDetailClientAdmin IBackgroundProcessOrderRepository.GetOrderDetailsInfo(Int32 orderID, Int32 tenantID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_BackroundOrderDetailForClientAdmin", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", orderID);
                command.Parameters.AddWithValue("@TenantID", tenantID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                OrderDetailClientAdmin objOrderDetail = new OrderDetailClientAdmin();


                objOrderDetail.OrderDetailInfoClientAdmin = ds.Tables[0].AsEnumerable().Select(col =>
                      new OrderDetailInfoClientAdmin
                      {
                          OrderID = Convert.ToInt32(col["OrderID"]),
                          DOB = Convert.ToString(col["DOB"]),
                          ApplicantName = col["ApplicantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ApplicantName"]),
                          Gender = col["GenderName"] == DBNull.Value ? String.Empty : Convert.ToString(col["GenderName"]),
                          PhoneNumber = col["PhoneNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["PhoneNumber"]),
                          Address1 = col["Address1"] == DBNull.Value ? String.Empty : Convert.ToString(col["Address1"]),
                          Address2 = col["Address2"] == DBNull.Value ? String.Empty : Convert.ToString(col["Address2"]),
                          OrderStatus = col["OrderStatus"] == DBNull.Value ? String.Empty : Convert.ToString(col["OrderStatus"]),
                          Email = col["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["PrimaryEmailAddress"]),
                          SSN = col["SSN"] == DBNull.Value ? String.Empty : Convert.ToString(col["SSN"]),
                          DatePaid = col["DatePaid"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["DatePaid"]),
                          DateCreated = col["DateCreated"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["DateCreated"]),
                          DateCompleted = col["OrderCompleteDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["OrderCompleteDate"]),
                          //PaymentType = col["PaymentType"] == DBNull.Value ? String.Empty : Convert.ToString(col["PaymentType"]),
                          InstitutionHierarchy = col["InstitutionHierarchy"] == DBNull.Value ? String.Empty : Convert.ToString(col["InstitutionHierarchy"]),
                          Category = col["Category"] == DBNull.Value ? String.Empty : Convert.ToString(col["Category"]),
                          City = col["CityName"] == DBNull.Value ? String.Empty : Convert.ToString(col["CityName"]),
                          State = col["StateName"] == DBNull.Value ? String.Empty : Convert.ToString(col["StateName"]),
                          Zip = col["ZipCode"] == DBNull.Value ? String.Empty : Convert.ToString(col["ZipCode"]),
                      }).FirstOrDefault();


                if (ds.Tables[1].Rows.Count > AppConsts.NONE)
                {
                    objOrderDetail.OrderNotes = ds.Tables[1].AsEnumerable().Select(col =>
                      new OrderNotesClientAdmin
                      {
                          ONTS_ID = Convert.ToInt32(col["ONTS_ID"]),
                          ONTS_NoteText = Convert.ToString(col["ONTS_NoteText"]),

                      }).ToList();
                }

                if (ds.Tables[2].Rows.Count > AppConsts.NONE)
                {
                    objOrderDetail.PaymentTypesAndStatus = ds.Tables[2].AsEnumerable().Select(col =>
                       new PaymentTypesAndStatus
                       {
                           PaymentOption = Convert.ToString(col["PaymentOption"]),
                           PaymentStatus = Convert.ToString(col["PaymentStatus"]),
                           PaymentStatusID = Convert.ToInt32(col["PaymentStatusID"]),
                           PaymentOptionID = Convert.ToInt32(col["PaymentOptionID"]),
                           OrderPaymentDetailID = Convert.ToInt32(col["OrderPaymentDetailID"]),
                           PaymentStatusCode = Convert.ToString(col["PaymentStatusCode"]),

                       }).ToList();
                }

                return objOrderDetail;
            }
        }

        List<OrderServiceGroupDetails> IBackgroundProcessOrderRepository.GetServiceGroupDetails(Int32 orderID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_ServiceGroupDetailForClientAdmin", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", orderID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                List<OrderServiceGroupDetails> lstOrderServiceGroupDetail = new List<OrderServiceGroupDetails>();
                lstOrderServiceGroupDetail = ds.Tables[0].AsEnumerable().Select(col =>
                      new OrderServiceGroupDetails
                      {
                          MasterOrderID = Convert.ToInt32(col["MasterOrderID"]),
                          ServiceGroupID = col["ServiceGroupName"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["ServiceGroupID"]),
                          ServiceGroupName = col["ServiceGroupName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ServiceGroupName"]),
                          //ServiceID = col["ServiceID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["ServiceID"]),
                          ServiceName = col["ServiceName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ServiceName"]),
                          VendorStatus = col["VendorStatus"] == DBNull.Value ? "Not Uploaded" : Convert.ToString(col["VendorStatus"]),
                          Label = col["Label"] == DBNull.Value ? String.Empty : Convert.ToString(col["Label"]),
                          CustomValue = col["CustomValue"] == DBNull.Value ? String.Empty : Convert.ToString(col["CustomValue"]),
                          //BkgOrderID = Convert.ToInt32(col["BkgOrderID"]),
                          LineItemID = col["LineItemID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["LineItemID"]),
                          //OrderPackageServiceGroupID = col["OrderPackageServiceGroupID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["OrderPackageServiceGroupID"]),
                          //OrderPackageServiceID = col["OrderPackageServiceID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["OrderPackageServiceID"]),
                          IsOrderFlagged = col["IsOrderFlagged"] == DBNull.Value ? false : Convert.ToBoolean(col["IsOrderFlagged"]),
                          IsCompleted = col["IsCompleted"] == DBNull.Value ? false : Convert.ToBoolean(col["IsCompleted"]),
                          IsFlagged = col["IsFlagged"] == DBNull.Value ? false : Convert.ToBoolean(col["IsFlagged"]),
                          HierarchyNodeID = col["HierarchyNodeID"] == DBNull.Value ? 0 : Convert.ToInt32(col["HierarchyNodeID"]),
                          LineDescription = col["LineDescription"] == DBNull.Value ? String.Empty : Convert.ToString(col["LineDescription"]),
                          ServiceTypeCode = col["ServiceTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(col["ServiceTypeCode"]),
                          PackageServiceGroupID = col["PackageServiceGroupID"] == DBNull.Value ? 0 : Convert.ToInt32(col["PackageServiceGroupID"]),
                          SvcGrpReviewStatusType = col["SvcGrpReviewStatusType"] == DBNull.Value ? String.Empty : Convert.ToString(col["SvcGrpReviewStatusType"]),
                          SvcGrpStatusType = col["SvcGrpStatusType"] == DBNull.Value ? String.Empty : Convert.ToString(col["SvcGrpStatusType"]),
                          IsServiceGroupStatusComplete = col["IsServiceGroupStatusComplete"] == DBNull.Value ? false : Convert.ToBoolean(col["IsServiceGroupStatusComplete"]),
                          SvcIsReportable = col["SvcIsReportable"] == DBNull.Value ? true : Convert.ToBoolean(col["SvcIsReportable"]),
                          IsEmployment = col["IsEmployment"] == DBNull.Value ? true : Convert.ToBoolean(col["IsEmployment"]),
                      }).ToList();

                return lstOrderServiceGroupDetail;
            }

        }

        /// <summary>
        /// Method is used to get the Menu
        /// </summary>
        /// <param name="orderID">orderID</param>
        /// <returns></returns>
        OrderDetailHeaderInfo IBackgroundProcessOrderRepository.GetOrderDetailHeaderInfo(Int32 orderID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("ams.usp_GetOrderDataForHeader", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@orderID", orderID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                OrderDetailHeaderInfo orderDetailHeaderInfo = new OrderDetailHeaderInfo();


                orderDetailHeaderInfo = ds.Tables[0].AsEnumerable().Select(col =>
                     new OrderDetailHeaderInfo
                     {
                         OrderID = Convert.ToInt32(col["OrderID"]),
                         DOB = Convert.ToString(col["DOB"]),
                         ApplicantName = Convert.ToString(col["ApplicantName"]),
                         Gender = Convert.ToString(col["GenderName"]),
                         PhoneNumber = col["PhoneNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["PhoneNumber"]),
                         StatusType = col["StatusType"] == DBNull.Value ? String.Empty : Convert.ToString(col["StatusType"]),
                         OrderDate = Convert.ToDateTime(col["OrderDate"]),
                         InstitutionColorStatus = col["InstitutionColorStatus"] == DBNull.Value ? String.Empty : Convert.ToString(col["InstitutionColorStatus"]),
                         InstitutionColorStatusID = col["InstitutionColorStatusID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["InstitutionColorStatusID"]),
                         PaymentType = Convert.ToString(col["PaymentType"]),
                         TotalPrice = col["TotalPrice"] == DBNull.Value ? AppConsts.NONE : Convert.ToDecimal(col["TotalPrice"]),
                         PaymentStatus = col["PaymentStatus"] == DBNull.Value ? String.Empty : Convert.ToString(col["PaymentStatus"]),
                         OrderNumber = col["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["OrderNumber"]),
                         //UAT-2439,Client Admin screen updates for text to icon
                         InstitutionColorStatusTooltip = col["InstitutionColorStatusTooltip"] == DBNull.Value ? String.Empty : Convert.ToString(col["InstitutionColorStatusTooltip"]),
                     }).FirstOrDefault();


                return orderDetailHeaderInfo;
            }
        }

        #endregion

        #region Custom Forms

        public List<CustomFormDataContract> GetCustomFormsForThePackage(String packageId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[ams].[usp_GetCustomFormsForPackage]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@packageID", packageId);
                //command.Parameters.AddWithValue("@filteringSortingData", verificationGridCustomPaging.XML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return SetCustomData(ds.Tables[0]);
                }
            }
            return new List<CustomFormDataContract>();
        }

        public List<AttributesForCustomFormContract> GetAttributesForTheCustomForm(String packageId, Int32 customFormId, string languageCode)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[ams].[usp_GetCustomFormAttributes]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@packageID", packageId);
                command.Parameters.AddWithValue("@customFormID", customFormId);
                command.Parameters.AddWithValue("@LanguageCode", languageCode);
                //command.Parameters.AddWithValue("@filteringSortingData", verificationGridCustomPaging.XML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return SetAttributesForCustomForm(ds.Tables[0]);
                }
            }
            return new List<AttributesForCustomFormContract>();
        }

        public List<BkgSvcAttributeOption> GetOptionValues(Int32 attributeId)
        {
            return _ClientDBContext.BkgSvcAttributeOptions.Where(x => x.EBSAO_BkgSvcAttributeID == attributeId && !x.EBSAO_IsDeleted && x.EBSAO_IsActive).ToList();
        }

        #region Methods needed for custom form

        private List<CustomFormDataContract> SetCustomData(DataTable table)
        {
            IEnumerable<DataRow> rows = table.AsEnumerable();
            return rows.Select(x => new CustomFormDataContract
            {
                CustomFormName = Convert.ToString(x["CustomFormName"]),
                customFormId = Convert.ToInt32(Convert.ToString(x["CustomFormID"])),
                //Level = Convert.ToInt32(Convert.ToString("Levels")),
            }).ToList();

        }

        public static List<AttributesForCustomFormContract> SetAttributesForCustomForm(DataTable table)
        {

            IEnumerable<DataRow> rows = table.AsEnumerable();
            return rows.Select(x => new AttributesForCustomFormContract
            {
                PackageID = Convert.ToInt32(Convert.ToString(x["PackageID"])),
                AtrributeGroupMappingId = Convert.ToInt32(Convert.ToString(x["AtrributeGroupMappingId"])),
                AttributeGroupId = Convert.ToInt32(Convert.ToString(x["AttributeGroupId"])),
                AttriButeGroupName = Convert.ToString(x["AttriButeGroupName"]),
                AttributeId = Convert.ToInt32(Convert.ToString(x["AttributeId"])),
                AttributeName = Convert.ToString(x["AttributeName"]),
                AttributeType = Convert.ToString(x["AttributeType"]),
                IsDisplay = Convert.ToString(x["IsDisplay"]).IsNullOrEmpty() ? false : Convert.ToBoolean(Convert.ToString(x["IsDisplay"])),
                IsHiddenFromUI = Convert.ToString(x["IsHiddenFromUI"]).IsNullOrEmpty() ? false : Convert.ToBoolean(Convert.ToString(x["IsHiddenFromUI"])),
                IsRequired = Convert.ToString(x["IsRequired"]).IsNullOrEmpty() ? false : Convert.ToBoolean(Convert.ToString(x["IsRequired"])),
                CustomFieldsDisplaySequence = Convert.ToInt32(Convert.ToString(x["CustomFieldsDisplaySequence"])),
                SectionTitle = Convert.ToString(x["SectionTitle"]),
                Sequence = Convert.ToInt32(Convert.ToString(x["Sequence"])),
                Occurence = Convert.ToInt32(Convert.ToString(x["Occurence"])),
                DisplayColumns = Convert.ToInt32(Convert.ToString(x["DisplayColumn"])),
                MinimumOccurence = Convert.ToString(x["minimumoccurence"]).IsNullOrEmpty() ? 1 : Convert.ToInt32(Convert.ToString(x["minimumoccurence"])),
                MaximumOccurence = Convert.ToString(x["MAXOccurence"]).IsNullOrEmpty() ? 10 : Convert.ToInt32(Convert.ToString(x["MAXOccurence"])),
                //SectionTitle = Convert.ToString(x["SectionTitle"]),
                CustomHtml = Convert.ToString(x["CustomHtml"]),
                AttributeTypeCode = Convert.ToString(x["AttributeTypeCode"]),
                AttributeGroupMappingCode = Convert.ToString(x["BAGM_Code"]),
                MinimumValue = Convert.ToString(x["MinimumValue"]),
                MaximumValue = Convert.ToString(x["MaximumValue"]),
                InstructionText = Convert.ToString(x["InstructionText"]),
                IsDecisionField = Convert.ToBoolean(x["IsDecisionField"]),
                AttributeCode = Convert.ToString(x["AttributeCode"]),
                ParentAttributeGroupMappingId = Convert.ToString(x["ParentAttributeGroupMappingId"]).IsNullOrEmpty() ? 0 : Convert.ToInt32(x["ParentAttributeGroupMappingId"]), //UAT 3521
                ValidateExpression = Convert.ToString(x["ValidateExpression"]).IsNullOrEmpty() ? String.Empty : Convert.ToString(x["ValidateExpression"]), //UAT 3521
                ValidationMessage = Convert.ToString(x["ValidationMessage"]).IsNullOrEmpty() ? String.Empty : Convert.ToString(x["ValidationMessage"]), //UAT 3521
                Name = Convert.ToString(x["Name"]),//UAT 3821
                BkgSvcAttributeGroupCode = Convert.ToString(x["BkgSvcAttributeGroupCode"]).IsNull() ? String.Empty : Convert.ToString(x["BkgSvcAttributeGroupCode"]),
                ServiceTypeCode = Convert.ToString(x["ServiceTypeCode"]).IsNull() ? String.Empty : Convert.ToString(x["ServiceTypeCode"]),  // UAT: 4594
                DisplayName = Convert.ToString(x["DisplayName"]).IsNull() ? String.Empty : Convert.ToString(x["DisplayName"])  // UAT: 4730
            }).ToList();
        }

        private List<BkgOrderDetailCustomFormUserData> SetDataForTheCustomForm(DataTable table)
        {
            IEnumerable<DataRow> rows = table.AsEnumerable();
            return rows.Select(x => new BkgOrderDetailCustomFormUserData
            {
                BackgroundOrderID = Convert.ToString(x["BackgroundOrderID"]).IsNull() ? 0 : Convert.ToInt32(Convert.ToString(x["BackgroundOrderID"])),
                CustomFormID = Convert.ToString(x["CustomFormID"]).IsNull() ? 0 : Convert.ToInt32(Convert.ToString(x["CustomFormID"])),
                AttributeGroupMappingID = Convert.ToString(x["AttributeGroupMappingID"]).IsNull() ? 0 : Convert.ToInt32(Convert.ToString(x["AttributeGroupMappingID"])),
                AttributeGroupID = Convert.ToString(x["AttributeGroupID"]).IsNull() ? 0 : Convert.ToInt32(Convert.ToString(x["AttributeGroupID"])),
                InstanceID = Convert.ToString(x["InstanceID"]).IsNull() ? 0 : Convert.ToInt32(Convert.ToString(x["InstanceID"])),
                AttributName = Convert.ToString(x["AttributName"]),
                Value = Convert.ToString(x["Value"])
            }).ToList();

        }

        #endregion

        #endregion

        #region Assign Flag To Completed Orders

        Boolean IBackgroundProcessOrderRepository.AssignFlagToCompletedOrders(Int32 backgroundProcessUserId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("ams.usp_AssignFlagToCompletedOrders", con);
                _command.Parameters.AddWithValue("@BackgroundProcessUserId", backgroundProcessUserId);
                _command.CommandType = CommandType.StoredProcedure;
                con.Open();
                Int32 rowsAffected = _command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return true;
                }
                con.Close();
            }
            return false;
        }
        #endregion

        #region Order Notification History

        List<OrderNotificationDetail> IBackgroundProcessOrderRepository.GetOrderNotificationHistory(Int32 orderId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[ams].[usp_GetOrderNotificationHistories]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", orderId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > AppConsts.NONE && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    return ds.Tables[0].AsEnumerable().Select(col =>
                       new OrderNotificationDetail
                       {
                           NotificationId = Convert.ToInt32(col["NotificationId"]),
                           SystemCommunicationId = col["SystemCommunicationId"] == DBNull.Value ? 0 : Convert.ToInt32(col["SystemCommunicationId"]),
                           ServiceFormId = col["ServiceFormId"] == DBNull.Value ? 0 : Convert.ToInt32(col["ServiceFormId"]),
                           NotificationType = col["NotificationType"] == DBNull.Value ? String.Empty : "Service Form - " + Convert.ToString(col["NotificationType"]),
                           NotificationAuto = !(Convert.ToBoolean(col["IsPostal"])),
                           CreatedDate = Convert.ToDateTime(col["CreatedDate"]),
                           SentBy = Convert.ToString(col["SentBy"]),
                           StatusId = col["StatusId"] == DBNull.Value ? 0 : Convert.ToInt32(col["StatusId"]),
                           SystemDocumentId = col["SystemDocumentID"] == DBNull.Value ? 0 : Convert.ToInt32(col["SystemDocumentID"]),
                           NotificationTypeCode = col["NotificationTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(col["NotificationTypeCode"]),
                           OrderId = col["OrderId"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrderId"]),
                           HierarchyNodeID = col["HierarchyNodeID"] == DBNull.Value ? 0 : Convert.ToInt32(col["HierarchyNodeID"]),
                           BkgPackageSvcGroupID = col["BkgPackageSvcGroupID"] == DBNull.Value ? 0 : Convert.ToInt32(col["BkgPackageSvcGroupID"]),
                           SvcGroupID = col["SvcGroupID"] == DBNull.Value ? 0 : Convert.ToInt32(col["SvcGroupID"]),
                           NotificationDetail = col["NotificationDetail"] == DBNull.Value ? String.Empty : Convert.ToString(col["NotificationDetail"]),
                           OldStatusId = col["OldStatusId"] == DBNull.Value ? 0 : Convert.ToInt32(col["OldStatusId"]),
                           NewStatusId = col["NewStatusId"] == DBNull.Value ? 0 : Convert.ToInt32(col["NewStatusId"]),
                           SvcGrpName = col["SvcGrpName"] == DBNull.Value ? String.Empty : Convert.ToString(col["SvcGrpName"]),
                           OrderNumber = col["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["OrderNumber"]),
                           //UAT-2156: New Notification for students with Comm Copy setting for Form Dispatched (Manual Service Forms).
                           PackageName = col["PackageName"] == DBNull.Value ? String.Empty : Convert.ToString(col["PackageName"]),
                           ServiceName = col["ServiceName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ServiceName"]),

                       }).ToList();
                }
            }
            return null;
        }

        List<LookupContract> IBackgroundProcessOrderRepository.GetHistoryByOrderNotificationId(Int32 orderNotificationId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[ams].[usp_GetHistoryByOrderNotificationId]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderNotificationID", orderNotificationId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > AppConsts.NONE && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    return ds.Tables[0].AsEnumerable().Select(col =>
                       new LookupContract
                       {
                           ID = Convert.ToInt32(col["NotificationId"]),
                           //Name = Convert.ToString(col["CreatedDate"]) + "-" + (col["StatusDesc"] == DBNull.Value ? String.Empty : "Changed from " + Convert.ToString(col["StatusDesc"])) + "-" + Convert.ToString(col["SentBy"]),
                           //ExtendedName = (col["OldStatusDesc"] != DBNull.Value || col["NewStatusDesc"] != DBNull.Value) ? (Convert.ToString(col["CreatedDate"]) + "-" 
                           // + (col["OldStatusDesc"] == DBNull.Value ? String.Empty : "Changed from " + Convert.ToString(col["OldStatusDesc"]))
                           //  + ((col["OldStatusDesc"] != DBNull.Value && col["NewStatusDesc"] != DBNull.Value) ? " To- " : String.Empty)
                           //  + (col["NewStatusDesc"] == DBNull.Value ? String.Empty : Convert.ToString(col["NewStatusDesc"]))) : String.Empty
                           Name = (col["OldStatusDesc"] != DBNull.Value || col["NewStatusDesc"] != DBNull.Value) ? (Convert.ToString(col["CreatedDate"]) + "-"
                             + (col["OldStatusDesc"] == DBNull.Value ? String.Empty : "Changed from " + Convert.ToString(col["OldStatusDesc"]))
                             + ((col["OldStatusDesc"] != DBNull.Value && col["NewStatusDesc"] != DBNull.Value) ? " To- " : String.Empty)
                             + (col["NewStatusDesc"] == DBNull.Value ? String.Empty : Convert.ToString(col["NewStatusDesc"])) + "-" + Convert.ToString(col["SentBy"])) : Convert.ToString(col["CreatedDate"]) + "-" + (col["StatusDesc"] == DBNull.Value ? String.Empty : "Changed from " + Convert.ToString(col["StatusDesc"])) + "-" + Convert.ToString(col["SentBy"])
                       }).ToList();
                }
            }
            return null;
        }

        OrderNotification IBackgroundProcessOrderRepository.GetOrderNotificationById(Int32 orderNotificationId)
        {
            return ClientDBContext.OrderNotifications.Include("BkgOrderServiceForms").Where(cond => cond.ONTF_ID == orderNotificationId).FirstOrDefault();
        }

        Boolean IBackgroundProcessOrderRepository.UpdateBkgOrderServiceFormStatus(OrderNotification newOrderNotification)
        {
            ClientDBContext.OrderNotifications.AddObject(newOrderNotification);
            ClientDBContext.SaveChanges();
            return true;
        }

        List<OrderNotificationDetail> IBackgroundProcessOrderRepository.GetApplicantSpecificOrderNotificationHistory(Int32 orgUserID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[ams].[usp_GetApplicantOrderNotificationHistories]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrgUserID", orgUserID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > AppConsts.NONE && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    return ds.Tables[0].AsEnumerable().Select(col =>
                       new OrderNotificationDetail
                       {
                           NotificationId = Convert.ToInt32(col["NotificationId"]),
                           SystemCommunicationId = col["SystemCommunicationId"] == DBNull.Value ? 0 : Convert.ToInt32(col["SystemCommunicationId"]),
                           NotificationType = col["NotificationType"] == DBNull.Value ? String.Empty : "Service Form - " + Convert.ToString(col["NotificationType"]),
                           CreatedDate = Convert.ToDateTime(col["CreatedDate"]),
                           SentBy = Convert.ToString(col["SentBy"]),
                           SystemDocumentId = col["SystemDocumentID"] == DBNull.Value ? 0 : Convert.ToInt32(col["SystemDocumentID"]),
                           OrderId = col["OrderId"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrderId"]),
                           NotificationDetail = col["NotificationDetail"] == DBNull.Value ? String.Empty : Convert.ToString(col["NotificationDetail"]),
                           OrderNumber = col["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["OrderNumber"]),
                       }).ToList();
                }
            }
            return null;
        }

        #endregion

        #region E Drug Screening Form
        String IBackgroundProcessOrderRepository.GetClearStarServiceIdAndExtVendorId(List<Int32> backgroundPackageServiceId, String serviceTypeCode)
        {
            String result = String.Empty;
            //Get background package service group id(BPSG_ID-BkgPackageSvcGroup)
            List<Int32> bkgPackageServiceGrpIds = _ClientDBContext.BkgPackageSvcGroups.Where(cond => backgroundPackageServiceId.Contains(cond.BPSG_BackgroundPackageID) && cond.BPSG_IsDeleted == false).Select(slct => slct.BPSG_ID).ToList();
            //Get service id of service type E Drug Screening.
            List<BackgroundService> lstBkgService = _ClientDBContext.BkgPackageSvcs.Where(cond => bkgPackageServiceGrpIds.Contains(cond.BPS_BkgPackageSvcGroupID) && cond.BPS_IsDeleted == false).Select(slct => slct.BackgroundService).ToList();
            if ((!lstBkgService.IsNullOrEmpty()) && lstBkgService.Count() > 0)
            {
                BackgroundService backgroundService = lstBkgService.FirstOrDefault(cnd => cnd.lkpBkgSvcType.BST_Code == serviceTypeCode && cnd.BSE_IsDeleted == false);
                if (!backgroundService.IsNullOrEmpty())
                {
                    Entity.BkgSvcExtSvcMapping bkgExtSvcMapping = base.SecurityContext.BkgSvcExtSvcMappings.FirstOrDefault(cond => cond.BSESM_BkgSvcId == backgroundService.BSE_ID && cond.BSESM_IsDeleted == false);
                    if ((!bkgExtSvcMapping.IsNullOrEmpty()) && (!bkgExtSvcMapping.ExternalBkgSvc.IsNullOrEmpty()))
                        result = bkgExtSvcMapping.ExternalBkgSvc.EBS_ExternalCode + "," + Convert.ToString(bkgExtSvcMapping.ExternalBkgSvc.EBS_VendorId);
                }
            }
            return result;
        }
        Int32 IBackgroundProcessOrderRepository.GetDPM_IDForEDSPackage(List<Int32> backgroundPackageServiceId, String serviceTypeCode)
        {
            List<Int32> bkgPackageServiceGrpIds = _ClientDBContext.BkgPackageSvcGroups.Where(cond => backgroundPackageServiceId.Contains(cond.BPSG_BackgroundPackageID) && cond.BPSG_IsDeleted == false).Select(slct => slct.BPSG_ID).ToList();
            //Get service id of service type E Drug Screening.
            BkgPackageSvc bkgPackageService = _ClientDBContext.BkgPackageSvcs.FirstOrDefault(cond => bkgPackageServiceGrpIds.Contains(cond.BPS_BkgPackageSvcGroupID) && cond.BPS_IsDeleted == false && cond.BackgroundService.lkpBkgSvcType.BST_Code == serviceTypeCode && cond.BackgroundService.BSE_IsDeleted == false);
            if (bkgPackageService.IsNotNull())
            {
                BkgPackageHierarchyMapping backgroundPackageHierarchyMapping = _ClientDBContext.BkgPackageHierarchyMappings.FirstOrDefault(cond => cond.BPHM_BackgroundPackageID == bkgPackageService.BkgPackageSvcGroup.BPSG_BackgroundPackageID && cond.BPHM_IsDeleted == false);
                if (backgroundPackageHierarchyMapping.IsNotNull())
                    return backgroundPackageHierarchyMapping.BPHM_InstitutionHierarchyNodeID;
            }
            return AppConsts.NONE;
        }

        String IBackgroundProcessOrderRepository.GetVendorAccountNumber(Int32 extVendorId, Int32 DPM_ID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetVendorAccountNumberForEDS", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ExtVendorId", extVendorId);
                command.Parameters.AddWithValue("@DPM_ID", DPM_ID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return Convert.ToString(ds.Tables[0].Rows[0]["EVA_AccountNumber"]);
                }
            }
            return String.Empty;
        }

        List<BkgAttributeGroupMapping> IBackgroundProcessOrderRepository.GetAttributeListByGroupId(Int32 attributeGroupId)
        {
            return _ClientDBContext.BkgAttributeGroupMappings.Where(cond => cond.BAGM_BkgSvcAttributeGroupId == attributeGroupId && cond.BAGM_IsDeleted == false).ToList();
        }

        Entity.CustomFormAttributeGroup IBackgroundProcessOrderRepository.GetEDrugScreeningHtml(Int32 customFormId, Int32 attributeGroupid)
        {
            return base.SecurityContext.CustomFormAttributeGroups.FirstOrDefault(cond => cond.CFAG_CustomFormId == customFormId && cond.CFAG_BkgSvcAttributeGroupId == attributeGroupid && cond.CFAG_IsDeleted == false);
        }
        Entity.ZipCode IBackgroundProcessOrderRepository.GetZipCodeObjByZipCode(String zipCode)
        {
            Entity.ZipCode zipCodeObj = base.SecurityContext.ZipCodes.FirstOrDefault(cond => cond.ZipCode1 == zipCode);
            if (!zipCodeObj.IsNullOrEmpty())
                return zipCodeObj;
            return new Entity.ZipCode();
        }
        String IBackgroundProcessOrderRepository.GetEDrugAttributeGroupId(String eDrugSvcAttributeGrpCode)
        {
            String result = String.Empty;
            Guid guidCodeEdeugSvcAttGrp = new Guid(eDrugSvcAttributeGrpCode);
            BkgSvcAttributeGroup bkgSvcAttributeGroupObj = _ClientDBContext.BkgSvcAttributeGroups.FirstOrDefault(cond => cond.BSAD_Code == guidCodeEdeugSvcAttGrp && cond.BSAD_IsSystemPreConfigured == true && cond.BSAD_IsDeleted == false);
            if (!bkgSvcAttributeGroupObj.IsNullOrEmpty())
            {
                Int32 eDrugCustomFormId = base.SecurityContext.CustomFormAttributeGroups.FirstOrDefault(cnd => cnd.CFAG_BkgSvcAttributeGroupId == bkgSvcAttributeGroupObj.BSAD_ID && cnd.CFAG_IsDeleted == false).CFAG_CustomFormId.Value;
                result = Convert.ToString(eDrugCustomFormId) + "," + Convert.ToString(bkgSvcAttributeGroupObj.BSAD_ID);
            }
            return result;
        }

        List<BkgOrderPackage> IBackgroundProcessOrderRepository.GetBackgroundPackageIdListByBkgOrderId(Int32 bkgOrderId)
        {
            return _ClientDBContext.BkgOrderPackages.Where(cond => cond.BOP_BkgOrderID == bkgOrderId && cond.BOP_IsDeleted == false).ToList();
        }

        CustomFormDataGroup IBackgroundProcessOrderRepository.GetCustomFormDataGroupForEDSData(Int32 bkgOrderId, String eDrugAttributegroupCode)
        {
            Guid guidCodeEdeugSvcAttGrp = new Guid(eDrugAttributegroupCode);
            return _ClientDBContext.CustomFormDataGroups.FirstOrDefault(cond => cond.CFDG_BkgOrderID == bkgOrderId && cond.BkgSvcAttributeGroup.BSAD_Code == guidCodeEdeugSvcAttGrp && cond.CFDG_IsDeleted == false);
        }

        List<BkgAttributeGroupMapping> IBackgroundProcessOrderRepository.GetListBkgAttributeGroupMappingForEDrug(String eDrugAttributeGroupCode)
        {
            Guid guidCodeEdeugSvcAttGrp = new Guid(eDrugAttributeGroupCode);
            return _ClientDBContext.BkgAttributeGroupMappings.Where(cond => cond.BkgSvcAttributeGroup.BSAD_Code == guidCodeEdeugSvcAttGrp && cond.BAGM_IsDeleted == false).ToList();
        }

        Boolean IBackgroundProcessOrderRepository.SaveCustomFormOrderDataForEDrug(List<CustomFormOrderData> lstCustomFormOrderDataObj)
        {
            foreach (CustomFormOrderData obj in lstCustomFormOrderDataObj)
            {
                _ClientDBContext.CustomFormOrderDatas.AddObject(obj);
            }
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }
        Boolean IBackgroundProcessOrderRepository.UpdateBkgOrderSvcLineItem(Int32 vendorId, Int32 bkgOrderId, String svcLineItemDisStatusCode, Int32 CurrentLoggedInUserId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_UpdateDispatchedExternalVendorStatus", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@VendorId", vendorId);
                command.Parameters.AddWithValue("@BkgOrderId", bkgOrderId);
                command.Parameters.AddWithValue("@Status", svcLineItemDisStatusCode);
                command.Parameters.AddWithValue("@CurrentLoggedInUserId", CurrentLoggedInUserId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        String IBackgroundProcessOrderRepository.GetRegistrationIdAttributeName(Guid registrationIdAttributeCode, Int32 attributeGroupId)
        {
            return _ClientDBContext.BkgAttributeGroupMappings.FirstOrDefault(cond => cond.BAGM_BkgSvcAttributeGroupId == attributeGroupId && cond.BAGM_Code == registrationIdAttributeCode && cond.BAGM_IsDeleted == false).BkgSvcAttribute.BSA_Name;
        }

        BkgOrder IBackgroundProcessOrderRepository.GetBkgOrderByOrderID(Int32 masterOrderId)
        {
            return _ClientDBContext.BkgOrders.FirstOrDefault(cond => cond.BOR_MasterOrderID == masterOrderId && cond.BOR_IsDeleted == false);
        }
        Boolean IBackgroundProcessOrderRepository.UpdateChanges()
        {
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        Boolean IBackgroundProcessOrderRepository.SaveWebCCFPDFDocument(ApplicantDocument webCCFPdfDocument)
        {
            _ClientDBContext.ApplicantDocuments.AddObject(webCCFPdfDocument);
            return true;
        }
        String IBackgroundProcessOrderRepository.GetStateNameByAbbreviation(String stateAbbreviation)
        {
            return base.SecurityContext.States.FirstOrDefault(cnd => cnd.StateAbbreviation == stateAbbreviation && cnd.IsActive == true).StateName;
        }

        /// <summary>
        /// This Parallel Task method is used to call the SaveCCFDataAndCCFPDF for EDS services.
        /// </summary>
        /// <param name="operation">operation</param>
        /// 
        /// <param name="loggerService">LoggerService (HttpContext.Current.ApplicationInstance of ISysXLoggerService )</param>
        /// <param name="exceptionService">ExceptionService (HttpContext.Current.ApplicationInstance of ISysXExceptionService)</param>
        public void RunParallelTaskSaveCCFDataAndPDF(INTSOF.ServiceUtil.ParallelTaskContext.ParallelTask operation, Dictionary<String, Object> dicParam, ISysXLoggerService loggerService, ISysXExceptionService exceptionService)
        {
            ParallelTaskContext.PerformParallelTask(operation, dicParam, loggerService, exceptionService);
        }

        /// <summary>
        /// Gets the BOPId of the EDS Package, from all the BKGPackages
        /// </summary>
        /// <param name="lstBOPIds"></param>
        /// <returns></returns>
        public Int32 GetEDSBkgOrderPkgId(List<Int32> lstBOPIds)
        {
            Int32 edsPkgId = AppConsts.NONE;

            var lstBOP = _ClientDBContext.BkgOrderPackages.Where(bop => lstBOPIds.Contains(bop.BOP_ID) && bop.BOP_IsDeleted == false).ToList();

            var lstBkgPkgIds = new List<Int32>();

            foreach (var bop in lstBOP)
            {
                if (!bop.BkgPackageHierarchyMapping.BPHM_IsDeleted)
                    lstBkgPkgIds.Add(bop.BkgPackageHierarchyMapping.BPHM_BackgroundPackageID);
            }

            var lstBPSG = _ClientDBContext.BkgPackageSvcGroups.Where(bpsg => lstBkgPkgIds.Contains(bpsg.BPSG_BackgroundPackageID) && bpsg.BPSG_IsDeleted == false)
                          .Select(bpsg => bpsg.BPSG_ID).ToList();

            var _lstBPS = _ClientDBContext.BkgPackageSvcs.Where(bps => lstBPSG.Contains(bps.BPS_BkgPackageSvcGroupID) && bps.BPS_IsDeleted == false).ToList();

            var bpSvc = _lstBPS.Where(bps => bps.BPS_IsDeleted == false && bps.BackgroundService.lkpBkgSvcType.BST_Code == BkgServiceType.ELECTRONICDRUGSCREEN.GetStringValue()).FirstOrDefault();

            //var _bpsg = _lstBPS.Where(x => x.BPS_BackgroundServiceID == bps.BPS_ID && x.BPS_IsDeleted == false).FirstOrDefault();
            if (bpSvc.IsNotNull())
            {
                edsPkgId = bpSvc.BkgPackageSvcGroup.BPSG_BackgroundPackageID;
            }

            var _bop = lstBOP.Where(x => x.BkgPackageHierarchyMapping.BPHM_BackgroundPackageID == edsPkgId && x.BOP_IsDeleted == false).FirstOrDefault();
            if (_bop.IsNotNull())
                return _bop.BOP_ID;

            return AppConsts.NONE;
        }

        #endregion

        #region D & R Document Entity Mapping

        List<DRDocsMappingContract> IBackgroundProcessOrderRepository.GetDRDocumentEntityMappingList(DRDocsMappingObjectIds docsEntityMappingFilters)
        {
            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetDRDocumentsEntityMappings", con);
                command.CommandType = CommandType.StoredProcedure;
                if (docsEntityMappingFilters.DocumentId > 0)
                    command.Parameters.AddWithValue("@DocumentID", docsEntityMappingFilters.DocumentId);
                //Added a Minus one check related to UAT-2833
                if (docsEntityMappingFilters.CountryId > 0 || docsEntityMappingFilters.CountryId == AppConsts.MINUS_ONE)
                    command.Parameters.AddWithValue("@CountryID", docsEntityMappingFilters.CountryId);
                if (docsEntityMappingFilters.TenantId > 0)
                    command.Parameters.AddWithValue("@TenantID", docsEntityMappingFilters.TenantId);
                if (docsEntityMappingFilters.StateId > 0)
                    command.Parameters.AddWithValue("@StateID", docsEntityMappingFilters.StateId);
                if (docsEntityMappingFilters.ServiceId > 0)
                    command.Parameters.AddWithValue("@ServiceID", docsEntityMappingFilters.ServiceId);
                if (docsEntityMappingFilters.InstitutionHierarchyId > 0)
                    command.Parameters.AddWithValue("@InstitutionHierarchyID", docsEntityMappingFilters.InstitutionHierarchyId);
                if (docsEntityMappingFilters.RegulatoryEntityTypeId > 0)
                    command.Parameters.AddWithValue("@RegulatoryEntityTypeID", docsEntityMappingFilters.RegulatoryEntityTypeId);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > AppConsts.NONE && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    return ds.Tables[0].AsEnumerable().Select(col =>
                       new DRDocsMappingContract
                       {
                           DisclosureDocumentMappingId = Convert.ToInt32(col["DisclosureDocumentMappingId"]),
                           CountryId = Convert.ToInt32(col["CountryId"]),
                           CountryName = Convert.ToString(col["CountryName"]),
                           StateId = col["StateId"] == DBNull.Value ? 0 : Convert.ToInt32(col["StateId"]),
                           StateName = col["StateName"] == DBNull.Value ? String.Empty : Convert.ToString(col["StateName"]),
                           ServiceId = col["ServiceId"] == DBNull.Value ? 0 : Convert.ToInt32(col["ServiceId"]),
                           ServiceName = col["ServiceName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ServiceName"]),
                           InstitutionHierarchyId = col["InstitutionHierarchyId"] == DBNull.Value ? 0 : Convert.ToInt32(col["InstitutionHierarchyId"]),
                           RegulatoryEntityTypeId = col["RegulatoryEntityTypeId"] == DBNull.Value ? Convert.ToInt16(0) : Convert.ToInt16(col["RegulatoryEntityTypeId"]),
                           RegulatoryEntityType = col["RegulatoryEntityType"] == DBNull.Value ? String.Empty : Convert.ToString(col["RegulatoryEntityType"]),
                           DocumentId = Convert.ToInt32(col["DocumentId"]),
                           DocumentName = Convert.ToString(col["DocumentName"]),
                           TenantId = col["TenantId"] == DBNull.Value ? 0 : Convert.ToInt32(col["TenantId"]),
                           TenantName = col["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["TenantName"]),
                           //UAT-2180: Update Grid on D&A Mapping screen to include hierarchy information.
                           InstitutionHierarchy = col["InstitutionHierarchyLabel"] == DBNull.Value ? String.Empty : Convert.ToString(col["InstitutionHierarchyLabel"]),
                       }).ToList();
                }
            }
            return new List<DRDocsMappingContract>();
        }

        #endregion


        #region MVR Fields in Personal Information Page
        public List<AttributeFieldsOfSelectedPackages> GetMVRAttriGrpID(String packageIds)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_CheckMvrGrpExistinOrderedPkgs", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@packageIds", packageIds);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                //Int32 attributeMVRID = 0;
                //if (ds.Tables[0].Rows.Count > AppConsts.NONE)
                //    attributeMVRID = Convert.ToInt32(ds.Tables[0].Rows[0]["AttributeGrpId"]);
                List<AttributeFieldsOfSelectedPackages> mvrFieldOfSelectedPackages = new List<AttributeFieldsOfSelectedPackages>();

                mvrFieldOfSelectedPackages = ds.Tables[0].AsEnumerable().Select(col =>
                    new AttributeFieldsOfSelectedPackages
                    {
                        AttributeGrpId = Convert.ToInt32(col["AttributeGrpId"]),
                        AttributeGrpMapingID = Convert.ToInt32(col["AttributeGrpMapingID"]),
                        AttributeID = Convert.ToInt32(col["AttributeID"]),
                        BSA_Code = Convert.ToString(col["BSA_Code"])
                    }).ToList();


                return mvrFieldOfSelectedPackages;
            }
        }
        public Int32 GetCustomFormIDBYCode(String customFormCode)
        {
            return base.SecurityContext.CustomForms.Where(x => x.CF_Code == customFormCode && !x.CF_IsDeleted).FirstOrDefault().CF_ID;
        }
        #endregion

        #region Supplement Service implementation

        /// <summary>
        /// Gets the Supplemental Service for a particular OrderPackageServiceGroup
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderPkgSvcGroupId"></param>
        /// <returns></returns>
        //public List<SupplementServicesInformation> GetSupplementServices(Int32 orderId, Int32 orderPkgSvcGroupId)
        public List<SupplementServicesInformation> GetSupplementServices(Int32 orderId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[AMS].[GetSupplementalServicesForTheOrder]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", orderId);
                //command.Parameters.AddWithValue("@OrdPkgSvcGroupId", orderPkgSvcGroupId);
                //command.Parameters.AddWithValue("@filteringSortingData", verificationGridCustomPaging.XML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return SetSupplementServiceData(ds.Tables[0]);
                }
            }
            return new List<SupplementServicesInformation>();
        }

        public List<SupplementServiceItemInformation> GetSupplementServiceItem(Int32 orderId, Int32 serviceId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[AMS].[GetSupplementalServiceItemForTheService]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", orderId);
                command.Parameters.AddWithValue("@ServiceId", serviceId);
                //command.Parameters.AddWithValue("@filteringSortingData", verificationGridCustomPaging.XML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return SetSupplementServiceItemData(ds.Tables[0]);
                }
            }
            return new List<SupplementServiceItemInformation>();
        }

        public List<SupplementServiceItemCustomForm> GetListOfCustomFormsForSelectedItem(String serviceItemId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[AMS].[usp_GetCustomFormsForTheServiceItems]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ServiceItemId", serviceItemId);
                //command.Parameters.AddWithValue("@filteringSortingData", verificationGridCustomPaging.XML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return SetSupplementServiceCustomFormData(ds.Tables[0]);
                }
            }
            return new List<SupplementServiceItemCustomForm>();
        }

        public List<SupplementServiceItemCustomForm> GetListOfCustomFormsForSelectedServices(String packageServiceIds)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[AMS].[usp_GetCustomFormsForSelectedServices]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageServiceIds", packageServiceIds);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return SetSupplementServiceCustomFormData(ds.Tables[0]);
                }
            }
            return new List<SupplementServiceItemCustomForm>();
        }

        public List<AttributesForCustomFormContract> GetListOfAttributesForSelectedItem(Int32 customFormId, Int32 serviceItemId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[AMS].[usp_GetAllTheAttributesForServiceItem]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomFormID", customFormId);
                command.Parameters.AddWithValue("@ServiceItemId", serviceItemId);
                //command.Parameters.AddWithValue("@filteringSortingData", verificationGridCustomPaging.XML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return SetSupplementServiceItemAttributeData(ds.Tables[0]);
                }
            }
            return new List<AttributesForCustomFormContract>();
        }

        public SourceServiceDetailForSupplement CheckSourceServicesForSupplement(Int32 orderId)
        {
            SourceServiceDetailForSupplement sourceServiceDetailForSupplement = new SourceServiceDetailForSupplement();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_CheckSourceServicesForSupplement", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderId", orderId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > AppConsts.NONE)
                {
                    if (ds.Tables[0].Rows.Count > AppConsts.NONE)
                        sourceServiceDetailForSupplement.IfSSNServiceExist = Convert.ToBoolean(ds.Tables[0].Rows[0]["IfSSNServiceExist"]);
                    if (ds.Tables[1].Rows.Count > AppConsts.NONE)
                        sourceServiceDetailForSupplement.IfNationalCriminalServiceExist = Convert.ToBoolean(ds.Tables[1].Rows[0]["IfNationalCriminalServiceExist"]);
                    if (ds.Tables[2].Rows.Count > AppConsts.NONE)
                        sourceServiceDetailForSupplement.SSNServiceResult = Convert.ToString(ds.Tables[2].Rows[0]["SSNServiceResult"]);
                    if (ds.Tables[3].Rows.Count > AppConsts.NONE)
                        sourceServiceDetailForSupplement.NationalCriminalServiceResult = Convert.ToString(ds.Tables[3].Rows[0]["NationalCriminalServiceResult"]);
                }
            }
            return sourceServiceDetailForSupplement;
        }

        #region Methods for the supplement service implementation

        private List<SupplementServicesInformation> SetSupplementServiceData(DataTable table)
        {
            IEnumerable<DataRow> rows = table.AsEnumerable();
            return rows.Select(x => new SupplementServicesInformation
            {
                OrderId = Convert.ToInt32(Convert.ToString(x["OrderId"])),
                ServiceName = Convert.ToString(x["ServiceName"]),
                ServiceId = Convert.ToInt32(Convert.ToString(x["ServiceId"])),
                PackageServiceId = x["PackageServiceId"] == DBNull.Value ? 0 : Convert.ToInt32(Convert.ToString(x["PackageServiceId"])),
            }).ToList();

        }

        private List<SupplementServiceItemInformation> SetSupplementServiceItemData(DataTable table)
        {
            IEnumerable<DataRow> rows = table.AsEnumerable();
            return rows.Select(x => new SupplementServiceItemInformation
            {
                OrderId = Convert.ToInt32(Convert.ToString(x["OrderId"])),
                ServiceItemName = Convert.ToString(x["ServiceItemName"]),
                ServiceId = Convert.ToInt32(Convert.ToString(x["ServiceId"])),
                ServiceItemId = Convert.ToInt32(Convert.ToString(x["ServiceItemId"])),
            }).ToList();

        }

        private List<SupplementServiceItemCustomForm> SetSupplementServiceCustomFormData(DataTable table)
        {
            IEnumerable<DataRow> rows = table.AsEnumerable();
            return rows.Select(x => new SupplementServiceItemCustomForm
            {
                CustomFormID = Convert.ToInt32(Convert.ToString(x["CustomFormID"])),
                ServiceItemName = Convert.ToString(x["ServiceItemName"]),
                CustomFormName = Convert.ToString(x["CustomFormName"]),
                ServiceItemID = Convert.ToInt32(Convert.ToString(x["ServiceItemID"])),
                ServiceId = Convert.ToInt32(Convert.ToString(x["ServiceID"])),
                PackageServiceId = x["PackageServiceId"] == DBNull.Value ? 0 : Convert.ToInt32(Convert.ToString(x["PackageServiceId"])),
            }).ToList();

        }

        private List<AttributesForCustomFormContract> SetSupplementServiceItemAttributeData(DataTable table)
        {
            IEnumerable<DataRow> rows = table.AsEnumerable();
            return rows.Select(x => new AttributesForCustomFormContract
            {
                AtrributeGroupMappingId = Convert.ToInt32(Convert.ToString(x["AtrributeGroupMappingId"])),
                AttributeGroupId = Convert.ToInt32(Convert.ToString(x["AttributeGroupId"])),
                AttriButeGroupName = Convert.ToString(x["AttriButeGroupName"]),
                AttributeId = Convert.ToInt32(Convert.ToString(x["AttributeId"])),
                AttributeName = Convert.ToString(x["AttributeName"]),
                AttributeType = Convert.ToString(x["AttributeType"]),
                IsDisplay = Convert.ToString(x["IsDisplay"]).IsNullOrEmpty() ? false : Convert.ToBoolean(Convert.ToString(x["IsDisplay"])),
                IsRequired = Convert.ToString(x["IsRequired"]).IsNullOrEmpty() ? false : Convert.ToBoolean(Convert.ToString(x["IsRequired"])),
                //CustomFieldsDisplaySequence = Convert.ToInt32(Convert.ToString(x["CustomFieldsDisplaySequence"])),
                SectionTitle = Convert.ToString(x["SectionTitle"]),
                Sequence = Convert.ToInt32(Convert.ToString(x["Sequence"])),
                Occurence = Convert.ToInt32(Convert.ToString(x["Occurence"])),
                DisplayColumns = Convert.ToInt32(Convert.ToString(x["DisplayColumn"])),
                MinimumOccurence = Convert.ToString(x["minimumoccurence"]).IsNullOrEmpty() ? 1 : Convert.ToInt32(Convert.ToString(x["minimumoccurence"])),
                //UAT-1571: WB: increase the count of Alias and Location inputs to 25. I still don’t want this to be open ended.
                //Set Minus one if MaxOccurrence is not set.
                MaximumOccurence = Convert.ToString(x["MAXOccurence"]).IsNullOrEmpty() ? AppConsts.MINUS_ONE : Convert.ToInt32(Convert.ToString(x["MAXOccurence"])),
                //SectionTitle = Convert.ToString(x["SectionTitle"]),
                CustomHtml = Convert.ToString(x["CustomHtml"]),
                AttributeTypeCode = Convert.ToString(x["AttributeTypeCode"]),
                AttributeCode = Convert.ToString(x["AttributeCode"]),
                AttributeGroupMappingCode = Convert.ToString(x["AttributeGroupCode"]),
            }).ToList();

        }

        #endregion

        #endregion

        #region Supplement Order
        public Int32 GetPaymentTypeIdForOrder(Int32 orderId)
        {
            Order originalOrder = _ClientDBContext.Orders.Where(cond => cond.OrderID == orderId).FirstOrDefault();
            return originalOrder.PaymentOptionID.HasValue ? originalOrder.PaymentOptionID.Value : 0;
        }

        ///// <summary>
        ///// Generate Supplment order line items for a Service Group
        ///// </summary>
        ///// <param name="supplementOrderData"></param>
        ///// <param name="paymentTypeIsInvoice"></param>
        //public void GenerateSupplementOrder(SupplementOrderContract supplementOrderData, Boolean paymentTypeIsInvoice, List<Int32> lstPackageServiceIds)
        //{
        //    DateTime _creationDateTime = DateTime.Now;
        //    var _bkgServiceGroupName = String.Empty;
        //    var _previousSvcGrpReviewSts = String.Empty;
        //    String _previousSvcGrpSts = String.Empty;
        //    //code moved to line after foreach loop

        //    //if (supplementOrderData.OrderPkgSvcGroupId > AppConsts.NONE)
        //    //{
        //    //    var _opsg = _ClientDBContext.BkgOrderPackageSvcGroups.Where(opsg => opsg.OPSG_ID ==
        //    //            supplementOrderData.OrderPkgSvcGroupId && !opsg.OPSG_IsDeleted).FirstOrDefault();

        //    //    if (_opsg.IsNotNull())
        //    //    {
        //    //        _previousSvcGrpReviewSts = _opsg.lkpBkgSvcGrpReviewStatusType.BSGRS_ReviewStatusType;
        //    //        _opsg.OPSG_SvcGrpReviewStatusTypeID = supplementOrderData.BkgSvcGrpReviewStatusTypeId;
        //    //        _opsg.OPSG_ModifiedByID = supplementOrderData.CreatedById;
        //    //        _opsg.OPSG_SvcGrpLastUpdatedDate = _opsg.OPSG_ModifiedOn = _creationDateTime;
        //    //        _bkgServiceGroupName = _opsg.BkgSvcGroup.BSG_Name;
        //    //    }
        //    //}

        //    //var _historyData = String.Format("Changed Background Service Group: {0} review status from {1} to {2}", _bkgServiceGroupName, _previousSvcGrpReviewSts, supplementOrderData.BkgSvcGrpReviewStatusType);

        //    TransactionGroup _transactionGrp = new TransactionGroup();
        //    _transactionGrp.TG_OrderID = supplementOrderData.OrderId;
        //    _transactionGrp.TG_TxnDate = _creationDateTime;
        //    _transactionGrp.TG_CreatedByID = supplementOrderData.CreatedById;
        //    _transactionGrp.TG_CreatedOn = _creationDateTime;

        //    OrderAddition _orderAddition = new OrderAddition();
        //    _orderAddition.OA_OrderID = supplementOrderData.OrderId;
        //    _orderAddition.OA_OrderStatusID = supplementOrderData.OrderStatusIdPaid;
        //    _orderAddition.OA_OrderAdditionTypeID = supplementOrderData.OrderAdditionTypeId;
        //    _orderAddition.OA_CreatedByID = supplementOrderData.CreatedById;
        //    _orderAddition.OA_CreatedOn = _creationDateTime;
        //    _orderAddition.OA_IsDeleted = false;

        //    List<vwBkgOrderPkgHierarchyData> _lst = _ClientDBContext.vwBkgOrderPkgHierarchyDatas
        //                                            .Where(bkgOrdData => bkgOrdData.MasterOrderId == supplementOrderData.OrderId)
        //                                            .ToList();

        //    // List of ALL the PackageSvcItemIds, to get the associated Background Service Id
        //    List<Int32> _lstPkgSvcItemIds = new List<Int32>();

        //    //List of all PkgSvcGrp for which line items have been created
        //    List<Int32> lstPkgSvcGrpIdsWithLineItems = new List<Int32>();

        //    foreach (var _supplementService in supplementOrderData.lstSupplementOrderData)
        //    {
        //        foreach (var _lineItem in _supplementService.lstOrderLineItems)
        //        {
        //            _lstPkgSvcItemIds.Add(_lineItem.PackageServiceItemId);
        //            lstPkgSvcGrpIdsWithLineItems.Add(_lineItem.PackageSvcGrpID);
        //        }
        //    }

        //    // UAT - 1120 - Order Review/Color Review/Supplemental Enhancements 
        //    // If any service from a service group is being supplemented, then the status of that service group must also be get changed

        //    List<Int32> _lstSelectedOrderPkgSvcGrpIds = new List<Int32>();
        //    Int32 bkgSvcid;
        //    //foreach (Int32 pkgSvcId in lstPackageServiceIds)
        //    //{
        //    //    bkgSvcid = _ClientDBContext.BkgPackageSvcs.Where(cond => cond.BPS_ID == pkgSvcId && !cond.BPS_IsDeleted).FirstOrDefault().BPS_BackgroundServiceID;
        //    //    bkgOrderPkgSvcGrpId = _lst.Where(vwData => vwData.BackgroundServiceId == bkgSvcid).FirstOrDefault().BkgOrdPkgSvcGrpId;
        //    //    _lstSelectedOrderPkgSvcGrpIds.Add(bkgOrderPkgSvcGrpId);
        //    //}

        //    foreach (Int32 pkgSvcId in lstPackageServiceIds)
        //    {
        //        bkgSvcid = _ClientDBContext.BkgPackageSvcs.Where(cond => cond.BPS_ID == pkgSvcId && !cond.BPS_IsDeleted).FirstOrDefault().BPS_BackgroundServiceID;
        //        if (_lst.Any(vwData => vwData.BackgroundServiceId == bkgSvcid))
        //        {
        //          var  bkgOrderPkgSvcGrp = _lst.Where(vwData => vwData.BackgroundServiceId == bkgSvcid).FirstOrDefault();
        //          _lstSelectedOrderPkgSvcGrpIds.Add(bkgOrderPkgSvcGrp.BkgOrdPkgSvcGrpId);
        //        }
        //    }
        //    List<PackageServiceItem> _lstPackageServiceItem = this.GetBackgroundServiceIds(_lstPkgSvcItemIds);

        //    Decimal _totalPrice = AppConsts.NONE;
        //    foreach (var _supplementService in supplementOrderData.lstSupplementOrderData)
        //    {
        //        //remove those opsg from _lstSelectedOrderPkgSvcGrpIds for which no line item has been created i.e. for which BPSG do not exist in xml
        //        //foreach (var item in BpsgIdsInXML)
        //        //{
        //            //check if OPSG exists 
        //                    //update opsg
        //           //else
        //                    //add opsg in updated form
        //        //}
        //        foreach (var _lineItem in _supplementService.lstOrderLineItems)
        //        {
        //            #region Add BkgOrderPackageSvcLineItem Table Data

        //            Int32 _bkgSvcId = _lstPackageServiceItem.Where(psi => psi.PSI_ID == _lineItem.PackageServiceItemId
        //                                           && !psi.PSI_IsDeleted).FirstOrDefault().BkgPackageSvc.BPS_BackgroundServiceID;

        //            BkgOrderPackageSvcLineItem _bkgOrdPkgSvcLineItem = new BkgOrderPackageSvcLineItem();
        //            _bkgOrdPkgSvcLineItem.PSLI_BkgOrderPackageSvcID = _lst.Where(vwData => vwData.BackgroundServiceId == _bkgSvcId).FirstOrDefault().BkgOrdPkgSvcId;
        //            _bkgOrdPkgSvcLineItem.OrderAddition = _orderAddition;
        //            _bkgOrdPkgSvcLineItem.PSLI_OrderLineItemStatusID = supplementOrderData.OrderLineItemStatusId;
        //            _bkgOrdPkgSvcLineItem.PSLI_ServiceItemID = _lineItem.PackageServiceItemId;
        //            _bkgOrdPkgSvcLineItem.PSLI_IsDeleted = false;
        //            _bkgOrdPkgSvcLineItem.PSLI_CreatedByID = supplementOrderData.CreatedById;
        //            _bkgOrdPkgSvcLineItem.PSLI_CreatedOn = _creationDateTime;
        //            _bkgOrdPkgSvcLineItem.PSLI_DispatchedExternalVendor = supplementOrderData.SvcLineItemDispatchStatusId;
        //            _bkgOrdPkgSvcLineItem.PSLI_NeedsExternalDispatch = true;
        //            _bkgOrdPkgSvcLineItem.PSLI_Description = _lineItem.LineItemDescription;

        //            #endregion

        //            if (!_lineItem.Price.IsNullOrEmpty())
        //            {
        //                #region Add Transaction Table Data if Line Item Price is available
        //                if (!_lineItem.PackageOrderItemPriceId.IsNullOrEmpty() && _lineItem.PackageOrderItemPriceId != AppConsts.NONE)
        //                {
        //                    Transaction _transaction = new Transaction();
        //                    _transaction.TransactionGroup = _transactionGrp;
        //                    _transaction.BkgOrderPackageSvcLineItem = _bkgOrdPkgSvcLineItem;
        //                    _transaction.TD_PackageServiceItemPriceID = _lineItem.PackageOrderItemPriceId;
        //                    if (paymentTypeIsInvoice)
        //                    {
        //                        _transaction.TD_Amount = _lineItem.Price;
        //                        _transaction.TD_Description = _lineItem.PriceDescription;
        //                    }
        //                    else
        //                    {
        //                        _transaction.TD_Amount = 0;
        //                        _transaction.TD_Description = String.Empty;
        //                    }
        //                    _transaction.TD_IsDeleted = false;
        //                    _transaction.TD_CreatedByID = supplementOrderData.CreatedById;
        //                    _transaction.TD_CreatedOn = _creationDateTime;

        //                    _bkgOrdPkgSvcLineItem.Transactions.Add(_transaction);
        //                }
        //                #endregion
        //            }
        //            if (!_lineItem.lstFees.IsNullOrEmpty())
        //            {
        //                foreach (var fee in _lineItem.lstFees)
        //                {
        //                    #region Add Transaction Table Data if Line Item Fees is available
        //                    if (!fee.PackageOrderItemFeeId.IsNullOrEmpty() && fee.PackageOrderItemFeeId != AppConsts.NONE)
        //                    {
        //                        Transaction _transaction = new Transaction();
        //                        _transaction.TransactionGroup = _transactionGrp;
        //                        _transaction.BkgOrderPackageSvcLineItem = _bkgOrdPkgSvcLineItem;
        //                        _transaction.TD_PackageServiceItemFeeID = fee.PackageOrderItemFeeId;
        //                        if (paymentTypeIsInvoice)
        //                        {
        //                            _transaction.TD_Amount = fee.Amount;
        //                            _transaction.TD_Description = fee.Description;
        //                        }
        //                        else
        //                        {
        //                            _transaction.TD_Amount = 0;
        //                            _transaction.TD_Description = String.Empty;
        //                        }
        //                        _transaction.TD_IsDeleted = false;
        //                        _transaction.TD_CreatedByID = supplementOrderData.CreatedById;
        //                        _transaction.TD_CreatedOn = _creationDateTime;

        //                        _bkgOrdPkgSvcLineItem.Transactions.Add(_transaction);
        //                    }
        //                    #endregion
        //                }
        //            }

        //            foreach (var _bkgSvcAttDataGroup in _lineItem.lstBkgSvcAttributeDataGroup)
        //            {
        //                BkgOrderLineItemDataMapping _lineItemDataMapping = new BkgOrderLineItemDataMapping();

        //                _lineItemDataMapping.BkgOrderPackageSvcLineItem = _bkgOrdPkgSvcLineItem;
        //                _lineItemDataMapping.OLIDM_BkgSvcAttributeGroupID = _bkgSvcAttDataGroup.AttributeGroupId;
        //                _lineItemDataMapping.OLIDM_InstanceID = null;
        //                _lineItemDataMapping.OLIDM_CreatedByID = supplementOrderData.CreatedById;
        //                _lineItemDataMapping.OLIDM_CreatedOn = _creationDateTime;
        //                _lineItemDataMapping.OLIDM_IsDeleted = false;
        //                _bkgOrdPkgSvcLineItem.BkgOrderLineItemDataMappings.Add(_lineItemDataMapping);

        //                foreach (var _attrData in _bkgSvcAttDataGroup.lstAttributeData)
        //                {
        //                    BkgOrderLineItemDataUsedAttrb _lineItemDataUsedAttr = new BkgOrderLineItemDataUsedAttrb();
        //                    _lineItemDataUsedAttr.BkgOrderLineItemDataMapping = _lineItemDataMapping;
        //                    _lineItemDataUsedAttr.OLIDUA_BkgAttributeGroupMappingID = _attrData.AttributeGroupMappingID;
        //                    _lineItemDataUsedAttr.OLIDUA_AttributeValue = _attrData.AttributeValue;

        //                    _lineItemDataMapping.BkgOrderLineItemDataUsedAttrbs.Add(_lineItemDataUsedAttr);
        //                }
        //            }
        //            if (paymentTypeIsInvoice)
        //                _totalPrice += _lineItem.TotalPrice;
        //            else
        //                _totalPrice += 0;
        //            _ClientDBContext.BkgOrderPackageSvcLineItems.AddObject(_bkgOrdPkgSvcLineItem);
        //        }

        //        _orderAddition.OA_Amount = _totalPrice;

        //        _ClientDBContext.OrderAdditions.AddObject(_orderAddition);
        //    }

        //    BkgOrder _bkgOrder = _ClientDBContext.BkgOrders.Include("Order").Where(x => x.BOR_MasterOrderID == supplementOrderData.OrderId).FirstOrDefault();

        //    #region Code to update BkgOrderPackageSvcGroup table and maintaining history

        //    List<BkgOrderPackageSvcGroup> lstBkgOrderPackageSvcGroupTobeUpdated = _ClientDBContext.BkgOrderPackageSvcGroups
        //                                                                         .Where(cond => _lstSelectedOrderPkgSvcGrpIds.Contains(cond.OPSG_ID) && !cond.OPSG_IsDeleted).ToList();
        //    String firstReviewCode = BkgSvcGrpReviewStatusType.FIRST_REVIEW.GetStringValue();

        //    foreach (BkgOrderPackageSvcGroup bkgOrderPackageSvcGroup in lstBkgOrderPackageSvcGroupTobeUpdated)
        //    {
        //        if (bkgOrderPackageSvcGroup.lkpBkgSvcGrpReviewStatusType.BSGRS_ReviewCode.Equals(firstReviewCode))
        //        {
        //            _previousSvcGrpReviewSts = bkgOrderPackageSvcGroup.lkpBkgSvcGrpReviewStatusType.BSGRS_ReviewStatusType;
        //            bkgOrderPackageSvcGroup.OPSG_SvcGrpReviewStatusTypeID = supplementOrderData.BkgSvcGrpReviewStatusTypeId;
        //            bkgOrderPackageSvcGroup.OPSG_ModifiedByID = supplementOrderData.CreatedById;
        //            bkgOrderPackageSvcGroup.OPSG_SvcGrpLastUpdatedDate = bkgOrderPackageSvcGroup.OPSG_ModifiedOn = _creationDateTime;
        //            _bkgServiceGroupName = bkgOrderPackageSvcGroup.BkgSvcGroup.BSG_Name;
        //            _previousSvcGrpSts = bkgOrderPackageSvcGroup.lkpBkgSvcGrpStatusType.BSGS_StatusType;
        //            bkgOrderPackageSvcGroup.OPSG_SvcGrpStatusTypeID = supplementOrderData.BkgSvcGrpStatusTypeId;

        //            String _historyData = String.Format("Changed Background Service Group: {0} review status from {1} to {2}", _bkgServiceGroupName, _previousSvcGrpReviewSts, supplementOrderData.BkgSvcGrpReviewStatusType);
        //            BkgOrderEventHistory _orderEventHistory = new BkgOrderEventHistory
        //            {
        //                BOEH_BkgOrderID = _bkgOrder.BOR_ID,
        //                BOEH_OrderEventDetail = _historyData,
        //                BOEH_EventHistoryId = supplementOrderData.BkgOrderEventHistoryId,
        //                BOEH_IsDeleted = false,
        //                BOEH_CreatedByID = supplementOrderData.CreatedById,
        //                BOEH_CreatedOn = _creationDateTime
        //            };
        //            _ClientDBContext.BkgOrderEventHistories.AddObject(_orderEventHistory);

        //            _historyData = String.Format("Changed Background Service Group: {0} status from {1} to {2}", _bkgServiceGroupName
        //                                                , _previousSvcGrpSts, supplementOrderData.BkgSvcGrpStatusType);
        //            _orderEventHistory = new BkgOrderEventHistory
        //            {
        //                BOEH_BkgOrderID = _bkgOrder.BOR_ID,
        //                BOEH_OrderEventDetail = _historyData,
        //                BOEH_EventHistoryId = supplementOrderData.BkgOrderEventHistoryId,
        //                BOEH_IsDeleted = false,
        //                BOEH_CreatedByID = supplementOrderData.CreatedById,
        //                BOEH_CreatedOn = _creationDateTime
        //            };
        //            _ClientDBContext.BkgOrderEventHistories.AddObject(_orderEventHistory);
        //        }

        //    }

        //    #endregion

        //    Int32 previousOrderStatusTypeID = _bkgOrder.BOR_OrderStatusTypeID ?? 0;
        //    Int32 newOrderStatusTypeID = supplementOrderData.BkgOrderStatusTypeId;

        //    if (!_bkgOrder.IsNullOrEmpty())
        //    {
        //        _bkgOrder.BOR_OrderStatusTypeID = supplementOrderData.BkgOrderStatusTypeId;
        //        _bkgOrder.BOR_ModifiedByID = supplementOrderData.CreatedById;
        //        _bkgOrder.BOR_ModifiedOn = _creationDateTime;

        //        if (!_bkgOrder.Order.GrandTotal.IsNullOrEmpty())
        //            _bkgOrder.Order.GrandTotal += _totalPrice;
        //        else
        //            _bkgOrder.Order.GrandTotal = _totalPrice;

        //        _bkgOrder.Order.ModifiedByID = supplementOrderData.CreatedById;
        //        _bkgOrder.Order.ModifiedOn = _creationDateTime;
        //    }

        //    if (previousOrderStatusTypeID != newOrderStatusTypeID)
        //    {
        //        List<lkpOrderStatusType> lstOrderStatusTypes = _ClientDBContext.lkpOrderStatusTypes.ToList();
        //        String previousOrderStatus = lstOrderStatusTypes.Where(x => x.OrderStatusTypeID == previousOrderStatusTypeID).Select(x => x.StatusType).FirstOrDefault();
        //        String newOrderStatus = lstOrderStatusTypes.Where(x => x.OrderStatusTypeID == newOrderStatusTypeID).Select(x => x.StatusType).FirstOrDefault();

        //        String orderUpdatedCode = BkgOrderEvents.ORDER_UPDATED.GetStringValue();
        //        Int32 eventHistoryID = _ClientDBContext.lkpEventHistories.Where(x => x.EH_Code == orderUpdatedCode).Select(x => x.EH_ID).FirstOrDefault();

        //        String orderStatusHistoryData = "Changed Order status from " + previousOrderStatus + " to " + newOrderStatus + "";

        //        BkgOrderEventHistory _orderEventHistoryForOrderStatus = new BkgOrderEventHistory
        //        {
        //            BOEH_BkgOrderID = _bkgOrder.BOR_ID,
        //            BOEH_OrderEventDetail = orderStatusHistoryData,
        //            BOEH_EventHistoryId = eventHistoryID,
        //            BOEH_IsDeleted = false,
        //            BOEH_CreatedByID = supplementOrderData.CreatedById,
        //            BOEH_CreatedOn = _creationDateTime
        //        };
        //        _ClientDBContext.BkgOrderEventHistories.AddObject(_orderEventHistoryForOrderStatus);
        //    }
        //    //code moved after foreach loop ===> moved with code where service group status is being updated

        //    //BkgOrderEventHistory _orderEventHistory = new BkgOrderEventHistory
        //    //{
        //    //    BOEH_BkgOrderID = _bkgOrder.BOR_ID,
        //    //    BOEH_OrderEventDetail = _historyData,
        //    //    BOEH_EventHistoryId = supplementOrderData.BkgOrderEventHistoryId,
        //    //    BOEH_IsDeleted = false,
        //    //    BOEH_CreatedByID = supplementOrderData.CreatedById,
        //    //    BOEH_CreatedOn = _creationDateTime
        //    //};
        //    //_ClientDBContext.BkgOrderEventHistories.AddObject(_orderEventHistory);

        //    _ClientDBContext.SaveChanges();
        //}



        /// <summary>
        /// Generate Supplment order line items for a Service Group
        /// </summary>
        /// <param name="supplementOrderData"></param>
        /// <param name="paymentTypeIsInvoice"></param>
        public void GenerateSupplementOrder(SupplementOrderContract supplementOrderData, Boolean paymentTypeIsInvoice, List<Int32> lstPackageServiceIds)
        {
            DateTime _creationDateTime = DateTime.Now;
            var _bkgServiceGroupName = String.Empty;
            var _previousSvcGrpReviewSts = String.Empty;
            String _previousSvcGrpSts = String.Empty;

            TransactionGroup _transactionGrp = new TransactionGroup();
            _transactionGrp.TG_OrderID = supplementOrderData.OrderId;
            _transactionGrp.TG_TxnDate = _creationDateTime;
            _transactionGrp.TG_CreatedByID = supplementOrderData.CreatedById;
            _transactionGrp.TG_CreatedOn = _creationDateTime;

            OrderAddition _orderAddition = new OrderAddition();
            _orderAddition.OA_OrderID = supplementOrderData.OrderId;
            _orderAddition.OA_OrderStatusID = supplementOrderData.OrderStatusIdPaid;
            _orderAddition.OA_OrderAdditionTypeID = supplementOrderData.OrderAdditionTypeId;
            _orderAddition.OA_CreatedByID = supplementOrderData.CreatedById;
            _orderAddition.OA_CreatedOn = _creationDateTime;
            _orderAddition.OA_IsDeleted = false;

            List<vwBkgOrderPkgHierarchyData> _lst = _ClientDBContext.vwBkgOrderPkgHierarchyDatas
                                                    .Where(bkgOrdData => bkgOrdData.MasterOrderId == supplementOrderData.OrderId)
                                                    .ToList();

            // List of ALL the PackageSvcItemIds, to get the associated Background Service Id
            List<Int32> _lstPkgSvcItemIds = new List<Int32>();


            foreach (var _supplementService in supplementOrderData.lstSupplementOrderData)
            {
                foreach (var _lineItem in _supplementService.lstOrderLineItems)
                {
                    _lstPkgSvcItemIds.Add(_lineItem.PackageServiceItemId);
                }
            }

            //List<Int32> _lstSelectedOrderPkgSvcGrpIds = new List<Int32>();
            //Int32 bkgSvcid;

            //foreach (Int32 pkgSvcId in lstPackageServiceIds)
            //{
            //    bkgSvcid = _ClientDBContext.BkgPackageSvcs.Where(cond => cond.BPS_ID == pkgSvcId && !cond.BPS_IsDeleted).FirstOrDefault().BPS_BackgroundServiceID;
            //    if (_lst.Any(vwData => vwData.BackgroundServiceId == bkgSvcid))
            //    {
            //        var bkgOrderPkgSvcGrp = _lst.Where(vwData => vwData.BackgroundServiceId == bkgSvcid).FirstOrDefault();
            //        if (bkgOrderPkgSvcGrp.BkgOrdPkgSvcGrpId.HasValue)
            //        {
            //            _lstSelectedOrderPkgSvcGrpIds.Add(bkgOrderPkgSvcGrp.BkgOrdPkgSvcGrpId.Value);
            //        }
            //    }
            //}

            List<PackageServiceItem> _lstPackageServiceItem = this.GetBackgroundServiceIds(_lstPkgSvcItemIds);
            List<BkgOrderPackageSvcGroup> lstBkgOrderPackageSvcGroupAdded = new List<BkgOrderPackageSvcGroup>();
            List<Int32> lstBkgOrderPackageSvcGroupIDUpdated = new List<Int32>();
            Decimal _totalPrice = AppConsts.NONE;
            List<BkgOrderPackageSvc> addedBkgOrderPackageSvcList = new List<BkgOrderPackageSvc>();
            foreach (var _supplementService in supplementOrderData.lstSupplementOrderData)
            {
                if (!_supplementService.lstOrderLineItems.IsNullOrEmpty())
                {
                    var pkgSvcGrpID = _supplementService.lstOrderLineItems.FirstOrDefault().PackageSvcGrpID;
                    var pkgSvcGroupList = _lst.Where(cond => cond.BkgPackageSvcGroupID == pkgSvcGrpID).ToList();

                    if (pkgSvcGroupList.IsNotNull())
                    {
                        var pkgSvcGroup = pkgSvcGroupList.FirstOrDefault();

                        if (pkgSvcGroup.BkgOrdPkgSvcGrpId.IsNull() &&
                            (!lstBkgOrderPackageSvcGroupAdded.Any(cond => cond.OPSG_BkgOrderPackageID == pkgSvcGroup.BkgOrdPkgId
                                                                     && cond.OPSG_BkgSvcGroupID == pkgSvcGroup.BkgSvcGroupID))
                            )
                        {
                            #region Add BkgOrderPackageSvcGroup Table Data

                            //Add OPSG in Database
                            BkgOrderPackageSvcGroup _bkgOrderPackageSvcGroup = new BkgOrderPackageSvcGroup();
                            _bkgOrderPackageSvcGroup.OPSG_BkgOrderPackageID = pkgSvcGroup.BkgOrdPkgId;
                            _bkgOrderPackageSvcGroup.OPSG_BkgSvcGroupID = Convert.ToInt32(pkgSvcGroup.BkgSvcGroupID);
                            _bkgOrderPackageSvcGroup.OPSG_IsDeleted = false;
                            _bkgOrderPackageSvcGroup.OPSG_CreatedByID = supplementOrderData.CreatedById;
                            _bkgOrderPackageSvcGroup.OPSG_CreatedOn = _creationDateTime;
                            _bkgOrderPackageSvcGroup.OPSG_SvcGrpReviewStatusTypeID = supplementOrderData.BkgSvcGrpReviewStatusTypeId;
                            _bkgOrderPackageSvcGroup.OPSG_SvcGrpStatusTypeID = supplementOrderData.BkgSvcGrpStatusTypeId;

                            #endregion

                            #region Add BkgOrderPackageSvc Table Data

                            addedBkgOrderPackageSvcList = new List<BkgOrderPackageSvc>();

                            foreach (var item in pkgSvcGroupList)
                            {
                                BkgOrderPackageSvc _bkgOrderPackageSvc = new BkgOrderPackageSvc();
                                _bkgOrderPackageSvc.BkgOrderPackageSvcGroup = _bkgOrderPackageSvcGroup;
                                _bkgOrderPackageSvc.BOPS_BackgroundServiceID = item.BackgroundServiceId;
                                _bkgOrderPackageSvc.BOPS_IsDeleted = false;
                                _bkgOrderPackageSvc.BOPS_CreatedByID = supplementOrderData.CreatedById;
                                _bkgOrderPackageSvc.BOPS_CreatedOn = _creationDateTime;
                                _bkgOrderPackageSvcGroup.BkgOrderPackageSvcs.Add(_bkgOrderPackageSvc);
                                addedBkgOrderPackageSvcList.Add(_bkgOrderPackageSvc);
                            }

                            #endregion
                            lstBkgOrderPackageSvcGroupAdded.Add(_bkgOrderPackageSvcGroup);
                            _ClientDBContext.BkgOrderPackageSvcGroups.AddObject(_bkgOrderPackageSvcGroup);
                        }
                        else if (pkgSvcGroup.BkgOrdPkgSvcGrpId.IsNotNull())
                        {
                            lstBkgOrderPackageSvcGroupIDUpdated.Add(pkgSvcGroup.BkgOrdPkgSvcGrpId.Value);
                        }

                        #region Add BkgOrderPackageSvcLineItem Table Data

                        //var orderLineItemsList = _supplementService.lstOrderLineItems.Where(cond => cond.PackageSvcGrpID == pkgSvcGrpID).ToList();

                        foreach (var _lineItem in _supplementService.lstOrderLineItems)
                        {
                            Int32 _bkgSvcId = _lstPackageServiceItem.Where(psi => psi.PSI_ID == _lineItem.PackageServiceItemId
                                               && !psi.PSI_IsDeleted).FirstOrDefault().BkgPackageSvc.BPS_BackgroundServiceID;

                            BkgOrderPackageSvcLineItem _bkgOrdPkgSvcLineItem = new BkgOrderPackageSvcLineItem();

                            if (!addedBkgOrderPackageSvcList.IsNullOrEmpty())
                            {
                                _bkgOrdPkgSvcLineItem.BkgOrderPackageSvc = addedBkgOrderPackageSvcList.Where(cond => cond.BOPS_BackgroundServiceID == _bkgSvcId)
                                                                                                      .FirstOrDefault();
                            }
                            else
                            {
                                _bkgOrdPkgSvcLineItem.PSLI_BkgOrderPackageSvcID = _lst.Where(vwData => vwData.BackgroundServiceId == _bkgSvcId)
                                                                                      .FirstOrDefault().BkgOrdPkgSvcId.Value;
                            }

                            _bkgOrdPkgSvcLineItem.OrderAddition = _orderAddition;
                            _bkgOrdPkgSvcLineItem.PSLI_OrderLineItemStatusID = supplementOrderData.OrderLineItemStatusId;
                            _bkgOrdPkgSvcLineItem.PSLI_ServiceItemID = _lineItem.PackageServiceItemId;
                            _bkgOrdPkgSvcLineItem.PSLI_IsDeleted = false;
                            _bkgOrdPkgSvcLineItem.PSLI_CreatedByID = supplementOrderData.CreatedById;
                            _bkgOrdPkgSvcLineItem.PSLI_CreatedOn = _creationDateTime;
                            _bkgOrdPkgSvcLineItem.PSLI_DispatchedExternalVendor = supplementOrderData.SvcLineItemDispatchStatusId;
                            _bkgOrdPkgSvcLineItem.PSLI_NeedsExternalDispatch = true;
                            _bkgOrdPkgSvcLineItem.PSLI_Description = _lineItem.LineItemDescription;

                            if (!_lineItem.Price.IsNullOrEmpty())
                            {
                                #region Add Transaction Table Data if Line Item Price is available

                                if (!_lineItem.PackageOrderItemPriceId.IsNullOrEmpty() && _lineItem.PackageOrderItemPriceId != AppConsts.NONE)
                                {
                                    Transaction _transaction = new Transaction();
                                    _transaction.TransactionGroup = _transactionGrp;
                                    _transaction.BkgOrderPackageSvcLineItem = _bkgOrdPkgSvcLineItem;
                                    _transaction.TD_PackageServiceItemPriceID = _lineItem.PackageOrderItemPriceId;
                                    if (paymentTypeIsInvoice)
                                    {
                                        _transaction.TD_Amount = _lineItem.Price;
                                        _transaction.TD_Description = _lineItem.PriceDescription;
                                    }
                                    else
                                    {
                                        _transaction.TD_Amount = 0;
                                        _transaction.TD_Description = String.Empty;
                                    }
                                    _transaction.TD_IsDeleted = false;
                                    _transaction.TD_CreatedByID = supplementOrderData.CreatedById;
                                    _transaction.TD_CreatedOn = _creationDateTime;

                                    _bkgOrdPkgSvcLineItem.Transactions.Add(_transaction);
                                }

                                #endregion
                            }
                            if (!_lineItem.lstFees.IsNullOrEmpty())
                            {
                                foreach (var fee in _lineItem.lstFees)
                                {
                                    #region Add Transaction Table Data if Line Item Fees is available
                                    if (!fee.PackageOrderItemFeeId.IsNullOrEmpty() && fee.PackageOrderItemFeeId != AppConsts.NONE)
                                    {
                                        Transaction _transaction = new Transaction();
                                        _transaction.TransactionGroup = _transactionGrp;
                                        _transaction.BkgOrderPackageSvcLineItem = _bkgOrdPkgSvcLineItem;
                                        _transaction.TD_PackageServiceItemFeeID = fee.PackageOrderItemFeeId;
                                        if (paymentTypeIsInvoice)
                                        {
                                            _transaction.TD_Amount = fee.Amount;
                                            _transaction.TD_Description = fee.Description;
                                        }
                                        else
                                        {
                                            _transaction.TD_Amount = 0;
                                            _transaction.TD_Description = String.Empty;
                                        }
                                        _transaction.TD_IsDeleted = false;
                                        _transaction.TD_CreatedByID = supplementOrderData.CreatedById;
                                        _transaction.TD_CreatedOn = _creationDateTime;

                                        _bkgOrdPkgSvcLineItem.Transactions.Add(_transaction);
                                    }
                                    #endregion
                                }
                            }

                            foreach (var _bkgSvcAttDataGroup in _lineItem.lstBkgSvcAttributeDataGroup)
                            {
                                BkgOrderLineItemDataMapping _lineItemDataMapping = new BkgOrderLineItemDataMapping();

                                _lineItemDataMapping.BkgOrderPackageSvcLineItem = _bkgOrdPkgSvcLineItem;
                                _lineItemDataMapping.OLIDM_BkgSvcAttributeGroupID = _bkgSvcAttDataGroup.AttributeGroupId;
                                _lineItemDataMapping.OLIDM_InstanceID = null;
                                _lineItemDataMapping.OLIDM_CreatedByID = supplementOrderData.CreatedById;
                                _lineItemDataMapping.OLIDM_CreatedOn = _creationDateTime;
                                _lineItemDataMapping.OLIDM_IsDeleted = false;
                                _bkgOrdPkgSvcLineItem.BkgOrderLineItemDataMappings.Add(_lineItemDataMapping);

                                foreach (var _attrData in _bkgSvcAttDataGroup.lstAttributeData)
                                {
                                    BkgOrderLineItemDataUsedAttrb _lineItemDataUsedAttr = new BkgOrderLineItemDataUsedAttrb();
                                    _lineItemDataUsedAttr.BkgOrderLineItemDataMapping = _lineItemDataMapping;
                                    _lineItemDataUsedAttr.OLIDUA_BkgAttributeGroupMappingID = _attrData.AttributeGroupMappingID;
                                    _lineItemDataUsedAttr.OLIDUA_AttributeValue = _attrData.AttributeValue;

                                    _lineItemDataMapping.BkgOrderLineItemDataUsedAttrbs.Add(_lineItemDataUsedAttr);
                                }
                            }

                            if (paymentTypeIsInvoice)
                            {
                                _totalPrice += _lineItem.TotalPrice;
                            }
                            else
                            {
                                _totalPrice += 0;
                            }
                            _ClientDBContext.BkgOrderPackageSvcLineItems.AddObject(_bkgOrdPkgSvcLineItem);
                        }
                        #endregion
                    }
                }
                _orderAddition.OA_Amount = _totalPrice;
                _ClientDBContext.OrderAdditions.AddObject(_orderAddition);
            }

            BkgOrder _bkgOrder = _ClientDBContext.BkgOrders.Include("Order").Where(x => x.BOR_MasterOrderID == supplementOrderData.OrderId).FirstOrDefault();

            #region Code to update BkgOrderPackageSvcGroup table and maintaining history

            List<BkgOrderPackageSvcGroup> lstBkgOrderPackageSvcGroupTobeUpdated = _ClientDBContext.BkgOrderPackageSvcGroups
                                                                                 .Where(cond => !cond.OPSG_IsDeleted
                                                                                                && lstBkgOrderPackageSvcGroupIDUpdated.Contains(cond.OPSG_ID))
                                                                                  .ToList();
            String firstReviewCode = BkgSvcGrpReviewStatusType.FIRST_REVIEW.GetStringValue();

            foreach (BkgOrderPackageSvcGroup bkgOrderPackageSvcGroup in lstBkgOrderPackageSvcGroupTobeUpdated)
            {
                if (bkgOrderPackageSvcGroup.lkpBkgSvcGrpReviewStatusType.BSGRS_ReviewCode.Equals(firstReviewCode))
                {
                    _previousSvcGrpReviewSts = bkgOrderPackageSvcGroup.lkpBkgSvcGrpReviewStatusType.BSGRS_ReviewStatusType;
                    bkgOrderPackageSvcGroup.OPSG_SvcGrpReviewStatusTypeID = supplementOrderData.BkgSvcGrpReviewStatusTypeId;
                    bkgOrderPackageSvcGroup.OPSG_ModifiedByID = supplementOrderData.CreatedById;
                    bkgOrderPackageSvcGroup.OPSG_SvcGrpLastUpdatedDate = bkgOrderPackageSvcGroup.OPSG_ModifiedOn = _creationDateTime;
                    _bkgServiceGroupName = bkgOrderPackageSvcGroup.BkgSvcGroup.BSG_Name;
                    _previousSvcGrpSts = bkgOrderPackageSvcGroup.lkpBkgSvcGrpStatusType.BSGS_StatusType;
                    bkgOrderPackageSvcGroup.OPSG_SvcGrpStatusTypeID = supplementOrderData.BkgSvcGrpStatusTypeId;

                    String _historyData = String.Format("Changed Background Service Group: {0} review status from {1} to {2}", _bkgServiceGroupName
                                                                                                                            , _previousSvcGrpReviewSts
                                                                                                                            , supplementOrderData.BkgSvcGrpReviewStatusType);
                    BkgOrderEventHistory _orderEventHistory = new BkgOrderEventHistory
                    {
                        BOEH_BkgOrderID = _bkgOrder.BOR_ID,
                        BOEH_OrderEventDetail = _historyData,
                        BOEH_EventHistoryId = supplementOrderData.BkgOrderEventHistoryId,
                        BOEH_IsDeleted = false,
                        BOEH_CreatedByID = supplementOrderData.CreatedById,
                        BOEH_CreatedOn = _creationDateTime
                    };
                    _ClientDBContext.BkgOrderEventHistories.AddObject(_orderEventHistory);

                    _historyData = String.Format("Changed Background Service Group: {0} status from {1} to {2}", _bkgServiceGroupName
                                                        , _previousSvcGrpSts, supplementOrderData.BkgSvcGrpStatusType);
                    _orderEventHistory = new BkgOrderEventHistory
                    {
                        BOEH_BkgOrderID = _bkgOrder.BOR_ID,
                        BOEH_OrderEventDetail = _historyData,
                        BOEH_EventHistoryId = supplementOrderData.BkgOrderEventHistoryId,
                        BOEH_IsDeleted = false,
                        BOEH_CreatedByID = supplementOrderData.CreatedById,
                        BOEH_CreatedOn = _creationDateTime
                    };
                    _ClientDBContext.BkgOrderEventHistories.AddObject(_orderEventHistory);
                }

            }

            foreach (BkgOrderPackageSvcGroup bkgOrderPackageSvcGroup in lstBkgOrderPackageSvcGroupAdded)
            {
                _bkgServiceGroupName = bkgOrderPackageSvcGroup.BkgSvcGroup.BSG_Name;
                String _historyData = String.Format("Created Background Service Group: {0} during supplement order with review status {1} and status {2}", _bkgServiceGroupName
                                                         , supplementOrderData.BkgSvcGrpReviewStatusType, supplementOrderData.BkgSvcGrpStatusType);
                BkgOrderEventHistory _orderEventHistory = new BkgOrderEventHistory
                {
                    BOEH_BkgOrderID = _bkgOrder.BOR_ID,
                    BOEH_OrderEventDetail = _historyData,
                    BOEH_EventHistoryId = supplementOrderData.BkgOrderEventHistoryId,
                    BOEH_IsDeleted = false,
                    BOEH_CreatedByID = supplementOrderData.CreatedById,
                    BOEH_CreatedOn = _creationDateTime
                };
                _ClientDBContext.BkgOrderEventHistories.AddObject(_orderEventHistory);
            }

            #endregion

            Int32 previousOrderStatusTypeID = _bkgOrder.BOR_OrderStatusTypeID ?? 0;
            Int32 newOrderStatusTypeID = supplementOrderData.BkgOrderStatusTypeId;

            if (!_bkgOrder.IsNullOrEmpty())
            {
                _bkgOrder.BOR_OrderStatusTypeID = supplementOrderData.BkgOrderStatusTypeId;
                _bkgOrder.BOR_ModifiedByID = supplementOrderData.CreatedById;
                _bkgOrder.BOR_ModifiedOn = _creationDateTime;

                if (!_bkgOrder.Order.GrandTotal.IsNullOrEmpty())
                    _bkgOrder.Order.GrandTotal += _totalPrice;
                else
                    _bkgOrder.Order.GrandTotal = _totalPrice;

                _bkgOrder.Order.ModifiedByID = supplementOrderData.CreatedById;
                _bkgOrder.Order.ModifiedOn = _creationDateTime;
            }

            if (previousOrderStatusTypeID != newOrderStatusTypeID)
            {
                List<lkpOrderStatusType> lstOrderStatusTypes = _ClientDBContext.lkpOrderStatusTypes.ToList();
                String previousOrderStatus = lstOrderStatusTypes.Where(x => x.OrderStatusTypeID == previousOrderStatusTypeID).Select(x => x.StatusType).FirstOrDefault();
                String newOrderStatus = lstOrderStatusTypes.Where(x => x.OrderStatusTypeID == newOrderStatusTypeID).Select(x => x.StatusType).FirstOrDefault();

                String orderUpdatedCode = BkgOrderEvents.ORDER_UPDATED.GetStringValue();
                Int32 eventHistoryID = _ClientDBContext.lkpEventHistories.Where(x => x.EH_Code == orderUpdatedCode).Select(x => x.EH_ID).FirstOrDefault();

                String orderStatusHistoryData = "Changed Order status from " + previousOrderStatus + " to " + newOrderStatus + "";

                BkgOrderEventHistory _orderEventHistoryForOrderStatus = new BkgOrderEventHistory
                {
                    BOEH_BkgOrderID = _bkgOrder.BOR_ID,
                    BOEH_OrderEventDetail = orderStatusHistoryData,
                    BOEH_EventHistoryId = eventHistoryID,
                    BOEH_IsDeleted = false,
                    BOEH_CreatedByID = supplementOrderData.CreatedById,
                    BOEH_CreatedOn = _creationDateTime
                };
                _ClientDBContext.BkgOrderEventHistories.AddObject(_orderEventHistoryForOrderStatus);
            }
            _ClientDBContext.SaveChanges();
        }





        /// <summary>
        /// Gets the list of all the PackageServiceItems & their related Background services, using Navigation property
        /// </summary>
        /// <param name="_lstPkgSvcItemIds"></param>
        /// <returns></returns>
        private List<PackageServiceItem> GetBackgroundServiceIds(List<Int32> _lstPkgSvcItemIds)
        {
            return _ClientDBContext.PackageServiceItems.Include("BkgPackageSvc").Where(x => _lstPkgSvcItemIds.Contains(x.PSI_ID) && !x.PSI_IsDeleted).ToList();
        }

        #endregion

        #region Send Order Complete Mail

        /// <summary>
        /// Get Organisation User By OrderId
        /// </summary>
        /// <param name="masterOrderId"></param>
        /// <returns></returns>
        OrganizationUser IBackgroundProcessOrderRepository.GetOrganisationUserByOrderId(Int32 masterOrderId)
        {
            Order order = _ClientDBContext.Orders.FirstOrDefault(cond => cond.OrderID == masterOrderId && cond.IsDeleted == false);
            if (order.IsNotNull() && order.OrganizationUserProfile.IsNotNull())
            {
                return order.OrganizationUserProfile.OrganizationUser;
            }
            return null;
        }

        Int32 IBackgroundProcessOrderRepository.CheckIfOrderCompleteNotificationExistsByOrderID(Int32 masterOrderId, String notificationType, Int32? packageServiceGroupID)
        {
            OrderNotification ordNotification;
            if (packageServiceGroupID != null)
            {
                ordNotification = _ClientDBContext.OrderNotifications.FirstOrDefault(cond => cond.ONTF_OrderID == masterOrderId && cond.lkpOrderNotificationType.ONT_Code.Equals(notificationType)
                && cond.ONTF_ParentNotificationID == null && cond.ONTF_BkgPackageSvcGroupID == packageServiceGroupID.Value);
            }
            else
            {
                ordNotification = _ClientDBContext.OrderNotifications.FirstOrDefault(cond => cond.ONTF_OrderID == masterOrderId && cond.lkpOrderNotificationType.ONT_Code.Equals(notificationType)
                && cond.ONTF_ParentNotificationID == null);
            }

            if (ordNotification.IsNotNull())
            {
                return ordNotification.ONTF_ID;
            }
            else
            {
                return 0;
            }
            //return _ClientDBContext.OrderNotifications.Any(cond => cond.ONTF_OrderID == masterOrderId && cond.lkpOrderNotificationType.ONT_Code.Equals(notificationType) && cond.ONTF_ParentNotificationID==null);
        }

        #endregion

        #region Disclosure And Release Form

        List<Entity.ClientEntity.ApplicantDocument> IBackgroundProcessOrderRepository.GetDisclosureReleaseDoc(Int32 masterOrderId)
        {
            Order order = _ClientDBContext.Orders.FirstOrDefault(cond => cond.OrderID == masterOrderId && cond.IsDeleted == false);
            Int32 backroundProfileRecordTypeID = _ClientDBContext.lkpRecordTypes.Where(cond => cond.Code.Equals("AAAC") && !cond.IsDeleted).FirstOrDefault().RecordTypeID;

            if (order.IsNotNull() && order.OrganizationUserProfile.IsNotNull())
            {
                Int32 orgUserProfileId = order.OrganizationUserProfile.OrganizationUserProfileID;
                List<Int32> lstApplicantDocumentIds = _ClientDBContext.GenericDocumentMappings.Where(cond => cond.lkpRecordType.RecordTypeID == backroundProfileRecordTypeID && cond.GDM_RecordID == orgUserProfileId && !cond.GDM_IsDeleted).Select(s => s.GDM_ApplicantDocumentID).ToList();

                if (lstApplicantDocumentIds.IsNotNull() && lstApplicantDocumentIds.Count > 0)
                {
                    List<ApplicantDocument> applicantDocument = _ClientDBContext.ApplicantDocuments.Where(cond => lstApplicantDocumentIds.Contains(cond.ApplicantDocumentID) && cond.IsDeleted == false && cond.lkpDocumentType.DMT_Code == "AAAD").ToList();
                    if (applicantDocument.IsNotNull() && applicantDocument.Count > 0)
                    {
                        return applicantDocument;
                    }
                    else
                    {
                        return new List<ApplicantDocument>();
                    }
                }
            }
            return new List<ApplicantDocument>();
        }
        #endregion

        #region Show EDS Document
        Boolean IBackgroundProcessOrderRepository.IsEdsServiceExitForOrder(Int32 orderId, String serviceTypeCode)
        {
            BkgOrder bkgOrder = _ClientDBContext.BkgOrders.FirstOrDefault(cond => cond.BOR_MasterOrderID == orderId && cond.BOR_IsDeleted == false);
            if (bkgOrder.IsNotNull() && bkgOrder.BkgOrderPackages.IsNotNull())
            {
                List<BkgOrderPackage> lstBkgOrderPackage = bkgOrder.BkgOrderPackages.Where(cnd => cnd.BOP_IsDeleted == false).ToList();
                if (lstBkgOrderPackage.IsNotNull() && lstBkgOrderPackage.Count > AppConsts.NONE)
                {
                    List<Int32> lstBkgOrderPackageIds = lstBkgOrderPackage.Select(slct => slct.BOP_ID).ToList();
                    List<BkgOrderPackageSvcGroup> lstBkgOrderPackageSvcGroup = _ClientDBContext.BkgOrderPackageSvcGroups.Where(cond => lstBkgOrderPackageIds.Contains(cond.OPSG_BkgOrderPackageID) && cond.OPSG_IsDeleted == false).ToList();
                    if (lstBkgOrderPackageSvcGroup.IsNotNull() && lstBkgOrderPackageSvcGroup.Count > AppConsts.NONE)
                    {
                        List<Int32> lstBkgOrderPackageSvcGroupIds = lstBkgOrderPackageSvcGroup.Select(slct => slct.OPSG_ID).ToList();
                        List<BkgOrderPackageSvc> lstBkgOrderPackageSvc = _ClientDBContext.BkgOrderPackageSvcs.Where(cnd => lstBkgOrderPackageSvcGroupIds.Contains(cnd.BOPS_BkgOrderPackageSvcGroupID.Value) && cnd.BOPS_IsDeleted == false).ToList();
                        if (lstBkgOrderPackageSvc.IsNotNull() && lstBkgOrderPackageSvc.Count > AppConsts.NONE && lstBkgOrderPackageSvc.Where(cnd => cnd.BackgroundService.IsNotNull()).ToList().Count > AppConsts.NONE)
                        {
                            return lstBkgOrderPackageSvc.Any(an => an.BackgroundService.lkpBkgSvcType.BST_Code == serviceTypeCode && an.BackgroundService.BSE_IsDeleted == false);
                        }
                    }
                }
            }
            return false;
        }
        ApplicantDocument IBackgroundProcessOrderRepository.GetApplicantDocumentForEds(Int32 orderId, String documentTypeCode, String recordTypeCode)
        {
            BkgOrder bkgOrder = _ClientDBContext.BkgOrders.FirstOrDefault(cond => cond.BOR_MasterOrderID == orderId && cond.BOR_IsDeleted == false);
            ApplicantDocument applicantDocument = new ApplicantDocument();
            if (bkgOrder.IsNotNull())
            {
                List<GenericDocumentMapping> documentMappings = _ClientDBContext.GenericDocumentMappings.Where(cond => cond.GDM_RecordID == bkgOrder.BOR_OrganizationUserProfileID && cond.lkpRecordType.Code == recordTypeCode && cond.GDM_IsDeleted == false).ToList();
                if (documentMappings.IsNotNull() && documentMappings.Count > AppConsts.NONE)
                {
                    List<ApplicantDocument> applicantDocumentLst = documentMappings.Select(slct => slct.ApplicantDocument).ToList();
                    if (applicantDocumentLst.IsNotNull() && applicantDocumentLst.Count > AppConsts.NONE)
                    {
                        applicantDocument = applicantDocumentLst.FirstOrDefault(cnd => cnd.lkpDocumentType.DMT_Code == documentTypeCode && cnd.IsDeleted == false);
                    }
                }
            }
            return applicantDocument;
        }
        Int32 IBackgroundProcessOrderRepository.GetSvcAttributeGroupIdByCode(String eDrugSvcAttributeGrpCode)
        {
            Guid guidCodeEdeugSvcAttGrp = new Guid(eDrugSvcAttributeGrpCode);
            BkgSvcAttributeGroup bkgSvcAttributeGroupObj = _ClientDBContext.BkgSvcAttributeGroups.FirstOrDefault(cond => cond.BSAD_Code == guidCodeEdeugSvcAttGrp && cond.BSAD_IsSystemPreConfigured == true && cond.BSAD_IsDeleted == false);
            if (!bkgSvcAttributeGroupObj.IsNullOrEmpty())
                return bkgSvcAttributeGroupObj.BSAD_ID;
            return AppConsts.NONE;
        }
        #endregion

        #region Show Residentail History Check

        /// <summary>
        /// Get the information for Residential History section (Attribute Group).
        /// Residential History section should not come at personal information page if no service in the selected packages required that.
        /// </summary>
        /// <param name="backgorundPackagesID"></param>
        /// <returns>Returns the Boolean Value True: If residentaial History Attribute Group is mapped with Background Package Service Group, Show Residential History Section.
        /// Else return false, Hide Residential History Section.</returns>
        List<PackageGroupContract> IBackgroundProcessOrderRepository.CheckShowResidentialHistory(List<Int32> backgorundPackagesID)
        {
            //Select c.* from ams.BkgPackageSvcGroup a
            //inner join ams.BkgPackageSvc s on s.BPS_BkgPackageSvcGroupID = a.BPSG_ID
            //Inner join ams.BkgSvcAttributeGroupMapping sa on sa.BSAGM_ServiceId= s.BPS_BackgroundServiceID
            //inner join ams.bkgattributegroupmapping b on sa.BSAGM_AttributeGroupMappingID = b.BAGM_ID
            //inner join ams.BkgSvcAttributeGroup c on c.BSAD_ID = b.BAGM_BkgSvcAttributeGroupId
            //where c.BSAD_Code = '338F1CA2-6B0A-43C1-B900-A8F6B058678F'

            //Guid resHistory_ServiceAttributeGroup = new Guid(ServiceAttributeGroup.RESIDENTIAL_HISTORY.GetStringValue());
            //Guid persInformation_SvcAttributeGroup = new Guid(ServiceAttributeGroup.PERSONAL_INFORMATION.GetStringValue());


            //return 
            var tempList = _ClientDBContext.BkgPackageSvcGroups
              .Where(cond => backgorundPackagesID.Contains(cond.BPSG_BackgroundPackageID) && cond.BPSG_IsDeleted == false)
              .Join(_ClientDBContext.BkgPackageSvcs.Where(cond => cond.BPS_IsDeleted == false)
                     , bpsg => bpsg.BPSG_ID
                     , bps => bps.BPS_BkgPackageSvcGroupID
                     , (bpsg, bps) => new { serviceGroupId = bps.BPS_BackgroundServiceID, packageId = bpsg.BPSG_BackgroundPackageID })
              .Join(_ClientDBContext.BkgSvcAttributeGroupMappings.Where(cond => cond.BSAGM_ServiceId.HasValue && cond.BSAGM_AttributeGroupMappingID.HasValue
                                                                        && cond.BSAGM_IsDeleted == false)
                     , bsid => bsid.serviceGroupId
                     , bsagm => bsagm.BSAGM_ServiceId.Value
                     , (bsid, bsagm) => new { groupMappingId = bsagm.BSAGM_AttributeGroupMappingID.Value, packageId = bsid.packageId })
              .Join(_ClientDBContext.BkgAttributeGroupMappings.Include("BkgSvcAttributeGroup").Where(cond => cond.BAGM_IsDeleted == false)
                     , bsagid => bsagid.groupMappingId
                     , bagm => bagm.BAGM_ID
                     , (bsagid, bagm) => new { code = bagm.BkgSvcAttributeGroup.BSAD_Code, packageId = bsagid.packageId }).ToList();
            return tempList.Select(fx => new PackageGroupContract { PackageId = fx.packageId, Code = fx.code }).ToList();

            //.Any(fx => fx.Equals(resHistory_ServiceAttributeGroup));

        }

        List<Int32> IBackgroundProcessOrderRepository.GetBackGroundPackagesForOrderId(Int32 orderId)
        {
            //Select c.* from ams.BkgPackageSvcGroup a
            //inner join ams.BkgPackageSvc s on s.BPS_BkgPackageSvcGroupID = a.BPSG_ID
            //Inner join ams.BkgSvcAttributeGroupMapping sa on sa.BSAGM_ServiceId= s.BPS_BackgroundServiceID
            //inner join ams.bkgattributegroupmapping b on sa.BSAGM_AttributeGroupMappingID = b.BAGM_ID
            //inner join ams.BkgSvcAttributeGroup c on c.BSAD_ID = b.BAGM_BkgSvcAttributeGroupId
            //where c.BSAD_Code = '338F1CA2-6B0A-43C1-B900-A8F6B058678F'

            //Guid resHistory_ServiceAttributeGroup = new Guid(ServiceAttributeGroup.RESIDENTIAL_HISTORY.GetStringValue());

            return _ClientDBContext.BkgOrders
                 .Where(cond => cond.BOR_MasterOrderID == orderId)
                 .Join(_ClientDBContext.BkgOrderPackages
                        , bpsg => bpsg.BOR_ID
                        , bps => bps.BOP_BkgOrderID
                        , (bpsg, bps) => bps.BOP_BkgPackageHierarchyMappingID)
                 .Join(_ClientDBContext.BkgPackageHierarchyMappings
                        , bsid => bsid
                        , bsagm => bsagm.BPHM_ID
                        , (bsid, bsagm) => bsagm.BPHM_BackgroundPackageID)
                 .ToList();
        }

        #endregion

        #region Get Min and Max Ocuurance Residential History

        /// <summary>
        /// Get Max Min Occurances based on the Background Packages ID. Returns a Dictionary containing Maximum of 'Min Occurance' 
        /// and Maximum of 'Max Occurances'.
        /// </summary>
        /// <param name="backgorundPackagesID"></param>
        /// <param name="attributeGrpCode"> Guid code for that Attrobute group</param>
        /// <returns></returns>
        Dictionary<String, Int32?> IBackgroundProcessOrderRepository.GetMinMaxOccurancesForAttributeGroup(List<Int32> backgorundPackagesID, Guid attributeGrpCode)
        {
            //Select psi.* from ams.BkgPackageSvcGroup a
            //Inner join ams.BkgPackageSvc b on a.BPSG_ID = b.BPS_BkgPackageSvcGroupID
            //Inner join ams.PackageServiceItem psi on psi.PSI_PackageServiceID = b.BPS_ID
            //INNER JOIN ams.BkgSvcAttributeGroup BSAG ON psi.PSI_AttributeGroupId = BSAG.BSAD_ID
            //WHERE BSAG.BSAD_Code = '338F1CA2-6B0A-43C1-B900-A8F6B058678F'

            //Guid resHistory_ServiceAttributeGroup = new Guid(ServiceAttributeGroup.RESIDENTIAL_HISTORY.GetStringValue());

            var getMaxMinOccurancesForAttributeGrp = _ClientDBContext.BkgPackageSvcGroups
                 .Where(cond => backgorundPackagesID.Contains(cond.BPSG_BackgroundPackageID) && cond.BPSG_IsDeleted == false)
                 .Join(_ClientDBContext.BkgPackageSvcs
                        , bpsg => bpsg.BPSG_ID
                        , bps => bps.BPS_BkgPackageSvcGroupID
                        , (bpsg, bps) => bps.BPS_ID)
                 .Join(_ClientDBContext.PackageServiceItems.Where(cond => cond.PSI_AttributeGroupId.HasValue && cond.PSI_IsDeleted == false)
                        , bps => bps
                        , psi => psi.PSI_PackageServiceID
                        , (bps, psi) => psi)
                 .Join(_ClientDBContext.BkgSvcAttributeGroups.Where(cond => cond.BSAD_IsDeleted == false)
                        , psi => psi.PSI_AttributeGroupId.Value
                        , bsag => bsag.BSAD_ID
                        , (psi, bsag) => new
                        {
                            AttrCode = bsag.BSAD_Code,
                            MaxOccurrence = psi.PSI_MaxOccurrences,
                            MinOccurrence = psi.PSI_MinOccurrences
                        })
                        .Where(cond => cond.AttrCode.Equals(attributeGrpCode))
                        .Select(col => new
                        {
                            MaxOccurrence = col.MaxOccurrence, //AppConsts.NONE,UAT-605
                            MinOccurrence = col.MinOccurrence.HasValue ? col.MinOccurrence.Value : AppConsts.NONE
                        }).ToList();



            //Dictionary<String, Int32> MinMaxOccuranceDict = new Dictionary<String, Int32>();
            Dictionary<String, Int32?> MinMaxOccuranceDict = new Dictionary<String, Int32?>();//UAT-605 make int32 nullable
            MinMaxOccuranceDict.Add("MaxOccurrence", getMaxMinOccurancesForAttributeGrp.Count() > AppConsts.NONE ?
                getMaxMinOccurancesForAttributeGrp.Max(cond => cond.MaxOccurrence) : null); //AppConsts.NONE);UAT-605
            MinMaxOccuranceDict.Add("MinOccurrence", getMaxMinOccurancesForAttributeGrp.Count() > AppConsts.NONE ?
                getMaxMinOccurancesForAttributeGrp.Max(cond => cond.MinOccurrence) : AppConsts.NONE);
            return MinMaxOccuranceDict;
        }

        #endregion

        #region package level instruction text for Residential History

        Dictionary<Guid, String> IBackgroundProcessOrderRepository.ShowInstructionTextForResiHistory(List<Int32> backgorundPackagesID)
        {
            Guid resHistory_ServiceAttributeGroup = new Guid(ServiceAttributeGroup.RESIDENTIAL_HISTORY.GetStringValue());
            Guid persInformation_ServiceAttributeGroup = new Guid(ServiceAttributeGroup.PERSONAL_INFORMATION.GetStringValue());
            Dictionary<Guid, String> instructionTextDictionary = new Dictionary<Guid, string>();
            //List<String> 
            var lstResHistoryInstruction = _ClientDBContext.BkgPkgAttributeGroupInstructions.Include("BkgSvcAttributeGroups")
                                  .Where(x => backgorundPackagesID.Contains(x.BPAGI_BackgroundPackageID)
                                   && (x.BkgSvcAttributeGroup.BSAD_Code == resHistory_ServiceAttributeGroup || x.BkgSvcAttributeGroup.BSAD_Code == persInformation_ServiceAttributeGroup)
                                   && !x.BPAGI_InstructionText.Equals(String.Empty))
                                   .Select(y => new
                                   {
                                       instructionText = y.BPAGI_InstructionText,
                                       SvcAttributeGroupId = y.BkgSvcAttributeGroup.BSAD_Code
                                   }).ToList();
            if (!lstResHistoryInstruction.IsNullOrEmpty() && lstResHistoryInstruction.Count > 0)
            {
                instructionTextDictionary.Add(resHistory_ServiceAttributeGroup, lstResHistoryInstruction.Where(x => x.SvcAttributeGroupId == resHistory_ServiceAttributeGroup).Count() > AppConsts.NONE ? lstResHistoryInstruction.Where(x => x.SvcAttributeGroupId == resHistory_ServiceAttributeGroup).FirstOrDefault().instructionText : String.Empty);
                instructionTextDictionary.Add(persInformation_ServiceAttributeGroup, lstResHistoryInstruction.Where(x => x.SvcAttributeGroupId == persInformation_ServiceAttributeGroup).Count() > AppConsts.NONE ? lstResHistoryInstruction.Where(x => x.SvcAttributeGroupId == persInformation_ServiceAttributeGroup).FirstOrDefault().instructionText : String.Empty);
            }
            return instructionTextDictionary;
        }

        #endregion

        /// <summary>
        /// Get All Client Admins having node permission
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="hierarchyNodeID"></param>
        /// <returns>DataTable</returns>
        DataTable IBackgroundProcessOrderRepository.GetClientAdminWithNodePermission(Int32 tenantID, Int32 hierarchyNodeID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[dbo].[usp_GetClientAdminWithNodePermission]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantID", tenantID);
                command.Parameters.AddWithValue("@HierarchyNodeID", hierarchyNodeID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > AppConsts.NONE && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        #region UAT-586 WB: AMS: When adding supplemental services to an order, need to be able to see what the applicant has already entered (location, alias, etc.)
        List<AttributesForCustomFormContract> IBackgroundProcessOrderRepository.GetAttributeDataListForPreExistingSupplement(Int32 groupId, Int32 masterOrderId, Int32 serviceItemId, Int32 serviceId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[AMS].[usp_GetSupplementAttributeData]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MasterOrderId", masterOrderId);
                command.Parameters.AddWithValue("@AttributeGroupId", groupId);
                command.Parameters.AddWithValue("@ServiceId", serviceId);
                command.Parameters.AddWithValue("@ServiceItemId", serviceItemId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return SetSupplementPreExistingAttributeData(ds.Tables[0]);
                }
            }
            return new List<AttributesForCustomFormContract>();
        }

        private List<AttributesForCustomFormContract> SetSupplementPreExistingAttributeData(DataTable table)
        {
            IEnumerable<DataRow> rows = table.AsEnumerable();
            return rows.Select(x => new AttributesForCustomFormContract
            {
                AtrributeGroupMappingId = Convert.ToInt32(Convert.ToString(x["AttributeGroupMappingId"])),
                AttributeGroupId = Convert.ToInt32(Convert.ToString(x["AttributeGroupID"])),
                AttributeId = Convert.ToInt32(Convert.ToString(x["AttributeID"])),
                AttributeName = Convert.ToString(x["AttributeName"]),
                AttributeType = Convert.ToString(x["AttributeType"]),
                AttributeTypeCode = Convert.ToString(x["AttributeTypeCode"]),
                AttributeDataValue = Convert.ToString(x["AttributeDataValue"]),
            }).ToList();

        }
        #endregion

        #region UAT-777: As an applicant, I should be able to access the service form PDFs from my complio account

        /// <summary>
        /// To Get Automatic Services Forms for an Order.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>DataTable</returns>
        DataTable IBackgroundProcessOrderRepository.GetAutomaticServiceFormForOrder(Int32 orderId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("ams.usp_GetAutomaticServiceFormForOrder", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", orderId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        #endregion

        #region UAT-807: Addition of a flagged report only notification

        /// <summary>
        /// To Get Background Flagged Order Notification Data
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <returns>DataTable</returns>
        DataTable IBackgroundProcessOrderRepository.GetBkgFlaggedOrderResultCompletedNotificationData(Int32 chunkSize)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetBkgFlaggedOrderCompletedResultData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        #endregion

        #region UAT-844 Bkg Order Review details Screen changes
        public BkgOrderPackageSvcGroup GetOrderPackageServiceGroupData(Int32 orderPkgSvcGroupID)
        {
            return _ClientDBContext.BkgOrderPackageSvcGroups.Where(x => x.OPSG_ID == orderPkgSvcGroupID && !x.OPSG_IsDeleted).FirstOrDefault();
        }
        #endregion


        DataTable IBackgroundProcessOrderRepository.GetBkgOrderReviewQueueData(BkgOrderReviewQueueContract bkgOrderReviewQueueContract, CustomPagingArgsContract gridCustomPaging)
        {

            #region SG REVIEW STATUS TYPE
            String svcGrpReviewStatusTypeIDs = "";
            if (bkgOrderReviewQueueContract.SvcGrpReviewStatusTypeIDs.IsNotNull() && bkgOrderReviewQueueContract.SvcGrpReviewStatusTypeIDs.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var item in bkgOrderReviewQueueContract.SvcGrpReviewStatusTypeIDs)
                {
                    builder.Append("<SvcGrpReviewStatusTypeID>" + item.ToString() + "</SvcGrpReviewStatusTypeID>");
                }
                svcGrpReviewStatusTypeIDs = "<SvcGrpReviewStatusTypeIDList>" + builder + "</SvcGrpReviewStatusTypeIDList>";
            }

            #endregion

            String orderBy = QueueConstants.ORDER_REVIEW_QUEUE_DEFAULT_SORTING_FIELDS;
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? "desc" : "asc" : "desc";

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetBkgOrderReviewQueueData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", bkgOrderReviewQueueContract.OrderNumber);
                command.Parameters.AddWithValue("@ApplicantFirstName", bkgOrderReviewQueueContract.ApplicantFirstName);
                command.Parameters.AddWithValue("@ApplicantLastName", bkgOrderReviewQueueContract.ApplicantLastName);
                command.Parameters.AddWithValue("@OrderFromDate", bkgOrderReviewQueueContract.OrderFromDate);
                command.Parameters.AddWithValue("@OrderToDate", bkgOrderReviewQueueContract.OrderToDate);
                command.Parameters.AddWithValue("@SvcGrpLastUpdatedFromDate", bkgOrderReviewQueueContract.SvcGrpUpdatedFromDate);
                command.Parameters.AddWithValue("@SvcGrpLastUpdatedToDate", bkgOrderReviewQueueContract.SvcGrpUpdatedToDate);
                //command.Parameters.AddWithValue("@TargetHierarchyNodeId", bkgOrderReviewQueueContract.DeptProgramMappingID);
                command.Parameters.AddWithValue("@TargetHierarchyNodeIds", bkgOrderReviewQueueContract.DeptProgramMappingIDs);
                command.Parameters.AddWithValue("@ReviewCriteriaId", bkgOrderReviewQueueContract.SelectedReviewCriteriaId);
                command.Parameters.AddWithValue("@SvcGrpReviewStatusId", svcGrpReviewStatusTypeIDs);//
                command.Parameters.AddWithValue("@SvcGrpStatusId", bkgOrderReviewQueueContract.SvcGrpStatusTypeID);
                command.Parameters.AddWithValue("@CurrentLoggedInUserId", bkgOrderReviewQueueContract.LoggedInUserId);
                command.Parameters.AddWithValue("@ArchivedState", bkgOrderReviewQueueContract.SelectedArchiveStateCode);
                command.Parameters.AddWithValue("@SvcGrpReviewType", bkgOrderReviewQueueContract.SvcGrpReviewType);
                command.Parameters.AddWithValue("@OrderBy", orderBy);
                command.Parameters.AddWithValue("@OrderDirection", ordDirection);
                command.Parameters.AddWithValue("@PageIndex", gridCustomPaging.CurrentPageIndex);
                command.Parameters.AddWithValue("@PageSize", gridCustomPaging.PageSize);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return new DataTable();
            }
        }

        List<BkgReviewCriteria> IBackgroundProcessOrderRepository.GetAllReviewCriterias()
        {
            return _ClientDBContext.BkgReviewCriterias.Where(cond => !cond.BRC_IsDeleted).ToList();
        }

        /// <summary>
        /// Method to check whether all service groups of an Order completed
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="serviceGroupCompletedID"></param>
        /// <returns></returns>
        Boolean IBackgroundProcessOrderRepository.AreServiceGroupsCompleted(Int32 orderID, Int32? serviceGroupCompletedID)
        {
            BkgOrder bkgOrder = _ClientDBContext.BkgOrders.Where(cond => cond.BOR_MasterOrderID == orderID).FirstOrDefault();
            IEnumerable<Int32> bop_ids = bkgOrder.BkgOrderPackages.Where(cond => cond.BOP_IsDeleted == false).Select(col => col.BOP_ID);
            List<BkgOrderPackageSvcGroup> lstBkgOrderPackageSvcGroup = _ClientDBContext.BkgOrderPackageSvcGroups
                                                                       .Where(cond => bop_ids.Contains(cond.OPSG_BkgOrderPackageID)
                                                                                      && cond.OPSG_IsDeleted == false
                                                                                      && (cond.OPSG_SvcGrpStatusTypeID ?? 0) != serviceGroupCompletedID).ToList();

            if (lstBkgOrderPackageSvcGroup.IsNotNull() && lstBkgOrderPackageSvcGroup.Count > 0)
                return false;
            return true;
        }

        List<BkgAttributeGroupMapping> IBackgroundProcessOrderRepository.GetAllBkgAttributeGroupMapping()
        {
            return _ClientDBContext.BkgAttributeGroupMappings.Where(cond => !cond.BAGM_IsDeleted).ToList();
        }

        #region UAT- 1159 WB: Add link to all Electronic service forms where the e-drug link is on the student dashboard.

        /// <summary>
        /// To Get Automatic Services Forms for list of Orders.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>DataTable</returns>
        DataTable IBackgroundProcessOrderRepository.GetAutomaticServiceFormForListOfOrders(string commaDelemittedOrderIDs)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("ams.usp_GetAutomaticServiceFormForListOfOrders", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DelimittedOrderIDs", commaDelemittedOrderIDs);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }
        #endregion

        #region UAT-1177:System Updates for 613[Notification for Employment flag order]

        /// <summary>
        /// To Get Background Flagged Order Completed Employment Notification Data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderID"></param>
        /// <returns>List of BkgOrderNotificationDataContract</returns>
        DataTable IBackgroundProcessOrderRepository.GetBkgFlaggedOrderEmploymentNotificationData(Int32 chunkSize)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetEmpBkgFlaggedOrderCompletedData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }
        #endregion

        List<ServiceLevelDetailsForOrderContract> IBackgroundProcessOrderRepository.GetServiceLevelDetailsForOrder(Int32 orderID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetServiceLevelDetailsForOrder", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", orderID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                List<ServiceLevelDetailsForOrderContract> lstServiceLevelDetailsForOrderContract = new List<ServiceLevelDetailsForOrderContract>();
                lstServiceLevelDetailsForOrderContract = ds.Tables[0].AsEnumerable().Select(col =>
                      new ServiceLevelDetailsForOrderContract
                      {
                          PackageID = Convert.ToInt32(col["PackageID"]),
                          PackageName = Convert.ToString(col["PackageName"]),
                          IsServiceGroupStatusComplete = Convert.ToBoolean(col["IsServiceGroupStatusComplete"]),
                          ServiceForms = col["ServiceForms"] == DBNull.Value ? String.Empty : Convert.ToString(col["ServiceForms"]),
                          ServiceGroupCompletionDate = col["ServiceGroupCompletionDate"] == DBNull.Value ? String.Empty : Convert.ToString(col["ServiceGroupCompletionDate"]),
                          PackageServiceGroupID = Convert.ToInt32(col["PackageServiceGroupID"]),
                          ServiceGroupID = Convert.ToInt32(col["ServiceGroupID"]),
                          ServiceGroupName = Convert.ToString(col["ServiceGroupName"]),
                          ServiceGroupStatus = col["ServiceGroupStatus"] == DBNull.Value ? String.Empty : Convert.ToString(col["ServiceGroupStatus"]),
                          ServiceID = Convert.ToInt32(col["ServiceID"]),
                          ServiceName = Convert.ToString(col["ServiceName"]),
                          ServiceStatus = Convert.ToString(col["ServiceStatus"]),
                          ServiceTypeCode = Convert.ToString(col["ServiceTypeCode"]),
                          IsServiceCompleted = Convert.ToBoolean(col["IsServiceCompleted"]),
                          IsServiceFlagged = Convert.ToBoolean(col["IsServiceFlagged"]),
                      }).ToList();

                return lstServiceLevelDetailsForOrderContract;
            }

        }

        #region UAT-1358: Complio Notification to applicant for PrintScan
        Boolean IBackgroundProcessOrderRepository.IsBkgServiceExistInOrder(Int32 orderPaymentDetailId, String bkgServiceTypeCode)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_IsBkgServiceExistInOrder", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderPaymentDetailID", orderPaymentDetailId);
                command.Parameters.AddWithValue("@BkgServiceTypeCode", bkgServiceTypeCode);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.IsNotNull() && ds.Tables.Count > AppConsts.NONE)
                {
                    return Convert.ToBoolean(ds.Tables[0].Rows[0].ItemArray[0]);
                }
            }
            return false;
        }
        #endregion

        #region  #region UAT-1455:Flag override should re-trigger data sync for the service group
        void IBackgroundProcessOrderRepository.RemoveDataSyncHistoryToRetriggerDataSync(Int32 PSLI_ID, Int32 currentLoggedInUserID, Boolean isPackageFlaggedOverride)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("ams.usp_RemoveDataSyncHistoryToReTriggerDataSync", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderPkgSvcLineItemID", PSLI_ID);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserID);
                command.Parameters.AddWithValue("@IsFlaggedOverride", isPackageFlaggedOverride);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
            }
        }

        Boolean IBackgroundProcessOrderRepository.GetBackGroundPackageFlaggedStatus(Int32 PSLI_ID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("ams.usp_GetBkgOrderPackageFlaggedStatus", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderPkgSvcLineItemID", PSLI_ID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToBoolean(ds.Tables[0].Rows[0]["IsPackageFlagged"]).IsNullOrEmpty() ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["IsPackageFlagged"]);
                }
                return false;
            }
        }
        #endregion

        DataTable IBackgroundProcessOrderRepository.GetFlaggedEmploymentServiceGroupCompletedNotificationData(Int32 chunkSize)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetFlaggedBkgServiceGroupCompletedEmployementPkgResultData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        #region UAT-1648:As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
        /// <summary>
        /// Get BkgOrderPackage List for BOPIDs
        /// </summary>
        /// <param name="BOPIds">BOPIds</param>
        /// <returns>List of BkgOrderPackage</returns>
        public List<BkgOrderPackage> GetBackgroundOrderPackageListById(List<Int32> BOPIds)
        {
            return _ClientDBContext.BkgOrderPackages.Where(cnd => !cnd.BOP_IsDeleted && BOPIds.Contains(cnd.BOP_ID)).ToList();
        }
        /// <summary>
        /// Get Bkg Order selected service details xml
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string GetBkgOrderServiceDetails(Int32 orderId)
        {
            return _ClientDBContext.CABSServiceOrderDetails.Where(cnd => cnd.CSOD_IsActive == true && cnd.CSOD_OrderID == orderId).Select(x => x.CSOD_ServiceDetails).FirstOrDefault();
        }

        /// <summary>
        /// Get Bkg Order Custom Form Attributes Data
        /// </summary>
        /// <param name="masterOrderId">Master Order ID</param>
        /// <param name="customFormId">Custom Form ID</param>
        /// <returns></returns>
        BkgOrderDetailCustomFormDataContract IBackgroundProcessOrderRepository.GetBkgORDCustomFormAttrDataForCompletingOrder(Int32 masterOrderId, String bopIds,
                                                                                                                             Boolean isIncludeMvrData)
        {
            BkgOrderDetailCustomFormDataContract customFormDataContract = null;
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[ams].[usp_GetOrderDetailControlsAndValuesForCompletingOrder]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BOPIds", bopIds);
                command.Parameters.AddWithValue("@OrderID", masterOrderId);
                command.Parameters.AddWithValue("@IncludeMVRData", isIncludeMvrData);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    customFormDataContract = new BkgOrderDetailCustomFormDataContract();
                    //customFormDataContract.lstCustomFormAttributes = SetAttributesForCustomForm(ds.Tables[0]);
                    customFormDataContract.lstDataForCustomForm = SetDataForTheCustomForm(ds.Tables[0]);
                }
            }
            return customFormDataContract;
        }
        #endregion

        #region  #region UAT 1659 Notes section added on the background screening side.

        Boolean IBackgroundProcessOrderRepository.SaveBkgOrderNote(Int32 orderId, String notes, Int32 loggedInUserId)
        {
            BkgOrderQueueNote newNote = new BkgOrderQueueNote()
            {
                BOQN_MasterOrderID = orderId,
                BOQN_Notes = notes,
                BOQN_IsDeleted = false,
                BOQN_CreatedBy = loggedInUserId,
                BOQN_CreatedOn = DateTime.Now
            };
            _ClientDBContext.BkgOrderQueueNotes.AddObject(newNote);
            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        List<BkgOrderQueueNotesContract> IBackgroundProcessOrderRepository.GetBkgOrderNotes(Int32 orderId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.GetBkgOrderQueueNotes", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MasterOrderId", orderId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                List<BkgOrderQueueNotesContract> lstBkgOrderQueueNotesContract = new List<BkgOrderQueueNotesContract>();

                lstBkgOrderQueueNotesContract = ds.Tables[0].AsEnumerable().Select(col =>
                    new BkgOrderQueueNotesContract
                    {
                        Note = Convert.ToString(col["Note"]),
                        UserName = Convert.ToString(col["UserName"]),
                        CreatedByID = Convert.ToInt32(col["CreatedByID"]),
                        CreatedOnDate = Convert.ToDateTime(col["CreatedOnDate"]).ToString()
                    }).ToList();
                return lstBkgOrderQueueNotesContract;
            }
        }

        #endregion

        public List<AttributeFieldsOfSelectedPackages> GetAttributeFieldsOfSelectedPackages(String packageIds)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_AttributeFieldsOfSelectedPackages", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@packageIds", packageIds);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                List<AttributeFieldsOfSelectedPackages> lstAttributeFieldsOfSelectedPackages = new List<AttributeFieldsOfSelectedPackages>();

                lstAttributeFieldsOfSelectedPackages = ds.Tables[0].AsEnumerable().Select(col =>
                    new AttributeFieldsOfSelectedPackages
                    {
                        AttributeGrpId = Convert.ToInt32(col["AttributeGrpId"]),
                        AttributeGrpCode = Convert.ToString(col["AttributeGrpCode"]),
                        AttributeGrpMapingID = Convert.ToInt32(col["AttributeGrpMapingID"]),
                        AttributeID = Convert.ToInt32(col["AttributeID"]),
                        BSA_Code = Convert.ToString(col["BSA_Code"]),
                        IsAttributeRequired = col["IsAttributeRequired"] == DBNull.Value ? false : Convert.ToBoolean(col["IsAttributeRequired"]),
                        IsAttributeDisplay = col["IsAttributeDisplay"] == DBNull.Value ? true : Convert.ToBoolean(col["IsAttributeDisplay"]),
                    }).ToList();


                return lstAttributeFieldsOfSelectedPackages;
            }
        }

        #region UAT-1795 : Add D&A download button on Background Order Queue search.

        List<BkgOrderSearchQueueContract> IBackgroundProcessOrderRepository.GetAllDnADocument(List<Int32> masterOrderId)
        {
            List<Order> orderDeatils = _ClientDBContext.Orders.Where(cond => masterOrderId.Contains(cond.OrderID) && !cond.IsDeleted).ToList();
            Int32 backroundProfileRecordTypeID = _ClientDBContext.lkpRecordTypes.Where(cond => cond.Code.Equals("AAAC") && !cond.IsDeleted).FirstOrDefault().RecordTypeID;
            List<BkgOrderSearchQueueContract> lstApplicantDocument = new List<BkgOrderSearchQueueContract>();
            BkgOrderSearchQueueContract ApplicantDocumentDetail = null;
            foreach (var order in orderDeatils)
            {
                if (order.IsNotNull() && order.OrganizationUserProfile.IsNotNull())
                {
                    Int32 orgUserProfileId = order.OrganizationUserProfile.OrganizationUserProfileID;
                    List<ApplicantDocument> applicantDocument = _ClientDBContext.GenericDocumentMappings.Where(cond => cond.lkpRecordType.RecordTypeID == backroundProfileRecordTypeID && cond.GDM_RecordID == orgUserProfileId && !cond.GDM_IsDeleted
                                                                                      && !cond.ApplicantDocument.IsDeleted && cond.ApplicantDocument.lkpDocumentType != null && cond.ApplicantDocument.lkpDocumentType.DMT_Code == "AAAD").Select(s => s.ApplicantDocument).ToList();
                    if (applicantDocument.IsNotNull() && applicantDocument.Count > 0)
                    {
                        foreach (var documentDetails in applicantDocument)
                        {
                            ApplicantDocumentDetail = new BkgOrderSearchQueueContract();
                            ApplicantDocumentDetail.OrderID = order.OrderID;
                            ApplicantDocumentDetail.OrderNumber = order.OrderNumber;
                            ApplicantDocumentDetail.FileName = documentDetails.FileName;
                            ApplicantDocumentDetail.ApplicantDocumentID = documentDetails.ApplicantDocumentID;
                            ApplicantDocumentDetail.DocumentPath = documentDetails.DocumentPath;
                            lstApplicantDocument.Add(ApplicantDocumentDetail);
                        }
                    }
                }
            }
            if (!lstApplicantDocument.IsNullOrEmpty())
                return lstApplicantDocument;
            return new List<BkgOrderSearchQueueContract>();
        }
        #endregion

        //UAT-1852 : If a Service Group is complete, but other service groups are not complete within an order, the system should send the applicant an email.
        public DataTable SendingMailForBkgSvcGrpCompletion()
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetPendingServiceGroupNotificationData", con);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        #region UAT-1996:Setting to allow Client admins the ability to edit color flags
        /// <summary>
        /// To update Order color flag Status
        /// </summary>
        /// <param name="selectedOrderColorStatusId"></param>
        /// <param name="orderID"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        Boolean IBackgroundProcessOrderRepository.UpdateOrderColorFlag(Int32 selectedOrderColorStatusId, Int32 orderID, Int32 loggedInUserId)
        {
            BkgOrder bkgOrder = _ClientDBContext.BkgOrders.Where(cond => cond.BOR_MasterOrderID == orderID).FirstOrDefault();

            if (bkgOrder.IsNotNull())
            {
                String oldColorFlagStatus = String.Empty;
                String oldColorFlagStatusCode = String.Empty;
                String newColorFlagStatus = String.Empty;
                String newColorFlagStatusCode = String.Empty;
                if (!bkgOrder.BOR_InstitutionStatusColorID.IsNullOrEmpty())
                {
                    oldColorFlagStatus = bkgOrder.InstitutionOrderFlag.lkpOrderFlag.OFL_Name;
                    oldColorFlagStatusCode = bkgOrder.InstitutionOrderFlag.lkpOrderFlag.OFL_Code;
                }
                var instOrderFlag = _ClientDBContext.InstitutionOrderFlags.FirstOrDefault(cond => cond.IOF_ID == selectedOrderColorStatusId && cond.IOF_IsDeleted == false);
                if (!instOrderFlag.IsNullOrEmpty())
                {
                    newColorFlagStatus = instOrderFlag.lkpOrderFlag.OFL_Name;
                    newColorFlagStatusCode = instOrderFlag.lkpOrderFlag.OFL_Code;
                }
                bkgOrder.BOR_InstitutionStatusColorID = selectedOrderColorStatusId;
                bkgOrder.BOR_ModifiedOn = DateTime.Now;
                bkgOrder.BOR_ModifiedByID = loggedInUserId;
                if (!oldColorFlagStatusCode.Equals(newColorFlagStatusCode))
                {
                    String orderStatus = BkgOrderEvents.ORDER_UPDATED.GetStringValue();
                    BkgOrderEventHistory _bkgOrderEventHistory = new BkgOrderEventHistory
                    {
                        BOEH_BkgOrderID = bkgOrder.BOR_ID,
                        BOEH_OrderEventDetail = !oldColorFlagStatus.IsNullOrEmpty() ? "Changed Color flag from " + oldColorFlagStatus + " to " + newColorFlagStatus + ""
                                                                                   : "Applied Color flag " + newColorFlagStatus + "",
                        BOEH_IsDeleted = false,
                        BOEH_CreatedByID = loggedInUserId,
                        BOEH_CreatedOn = DateTime.Now,
                        BOEH_EventHistoryId = _ClientDBContext.lkpEventHistories.Where(cond => cond.EH_Code == orderStatus).Select(col => col.EH_ID).FirstOrDefault()
                    };
                    _ClientDBContext.BkgOrderEventHistories.AddObject(_bkgOrderEventHistory);
                }

                if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
                    return true;
                return false;
            }
            return false;
        }

        #endregion

        List<Entity.ClientEntity.ApplicantDocument> IBackgroundProcessOrderRepository.GetOrderAndBackgroundProfileRelatedDocuments(Int32 masterOrderId)
        {
            Order order = _ClientDBContext.Orders.FirstOrDefault(cond => cond.OrderID == masterOrderId && cond.IsDeleted == false);
            Int32 backroundProfileRecordTypeID = _ClientDBContext.lkpRecordTypes.Where(cond => cond.Code.Equals("AAAC") && !cond.IsDeleted).FirstOrDefault().RecordTypeID;
            Int32 orderRecordTypeID = _ClientDBContext.lkpRecordTypes.Where(cond => cond.Code.Equals("AAAB") && !cond.IsDeleted).FirstOrDefault().RecordTypeID;

            if (order.IsNotNull() && order.OrganizationUserProfile.IsNotNull())
            {
                Int32 orgUserProfileId = order.OrganizationUserProfile.OrganizationUserProfileID;
                List<Int32> lstApplicantDocumentIds = _ClientDBContext.GenericDocumentMappings.Where(cond => (cond.lkpRecordType.RecordTypeID == backroundProfileRecordTypeID && cond.GDM_RecordID == orgUserProfileId && !cond.GDM_IsDeleted)
                                                                         || (cond.lkpRecordType.RecordTypeID == orderRecordTypeID && cond.GDM_RecordID == masterOrderId)
                                                                        ).Select(s => s.GDM_ApplicantDocumentID).ToList();

                if (lstApplicantDocumentIds.IsNotNull() && lstApplicantDocumentIds.Count > 0)
                {
                    List<ApplicantDocument> applicantDocument = _ClientDBContext.ApplicantDocuments.Include("GenericDocumentMappings").Where(cond => lstApplicantDocumentIds.Contains(cond.ApplicantDocumentID) && cond.IsDeleted == false
                                                                    && (cond.lkpDocumentType.DMT_Code == "AAAD" || cond.lkpDocumentType.DMT_Code == "AAAA" || cond.lkpDocumentType.DMT_Code == "AAAB" || cond.lkpDocumentType.DMT_Code == "AAAO")).ToList();

                    if (applicantDocument.IsNotNull() && applicantDocument.Count > 0)
                    {
                        return applicantDocument;
                    }
                    else
                    {
                        return new List<ApplicantDocument>();
                    }
                }
            }
            return new List<ApplicantDocument>();
        }

        Boolean IBackgroundProcessOrderRepository.UpdateOrderIntervalSearchRefOrderId(Int32 newOrderId, Int32 refOrderId, Int32 currentLoggedInUserId)
        {
            Order newOrder = _ClientDBContext.Orders.Where(cond => cond.OrderID == newOrderId && !cond.IsDeleted).FirstOrDefault();

            if (newOrder.IsNotNull())
            {
                newOrder.IntervalSearchRefOrderId = refOrderId;
                newOrder.ModifiedOn = DateTime.Now;
                newOrder.ModifiedByID = currentLoggedInUserId;

                if (_ClientDBContext.SaveChanges() > 0)
                    return true;
            }
            return false;
        }

        Int32 IBackgroundProcessOrderRepository.SaveAutoRecurringOrderHistory(AutoRecurringOrderHistory autoRecurringOrderHistory)
        {
            if (autoRecurringOrderHistory.AROH_Id == 0)
            {
                _ClientDBContext.AutoRecurringOrderHistories.AddObject(autoRecurringOrderHistory);
                _ClientDBContext.SaveChanges();
                return autoRecurringOrderHistory.AROH_Id;
            }
            return 0;
        }

        Boolean IBackgroundProcessOrderRepository.UpdateAutoRecurringOrderHistory(Int32 autoRecurringOrderHistoryId, DateTime? orderCompletionDate, Int32? newOrderId, String notes, Int32 currentUserId)
        {
            AutoRecurringOrderHistory autoRecurringOrderHistory = _ClientDBContext.AutoRecurringOrderHistories.Where(cond => cond.AROH_Id == autoRecurringOrderHistoryId && !cond.AROH_IsDeleted).FirstOrDefault();

            if (!autoRecurringOrderHistory.IsNullOrEmpty())
            {
                autoRecurringOrderHistory.AROH_OrderCompletionDate = orderCompletionDate;
                autoRecurringOrderHistory.AROH_NewOrderId = newOrderId;
                autoRecurringOrderHistory.AROH_Notes = notes;
                autoRecurringOrderHistory.AROH_ModifiedOn = DateTime.Now;
                autoRecurringOrderHistory.AROH_ModifiedBy = currentUserId;
                if (_ClientDBContext.SaveChanges() > 0)
                {
                    return true;
                }
            }

            return false;
        }

        #region UAT-2062:System to determine and add additional searches in supplement (SSN Trace)
        DataTable IBackgroundProcessOrderRepository.GetMatchedAdditionalSearchData(String inputAdditionalSearchXML, Int32 masterOrderId)
        {
            //UAT-2372:Dismiss suffixes on names in supplements
            String IgnoreSuffixesNames = WebConfigurationManager.AppSettings["IgnoreSuffixesNamesInSupplements"];

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetAdditionalSearchDataForSupplement", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InputXML", inputAdditionalSearchXML);
                command.Parameters.AddWithValue("@MasterOrderID", masterOrderId);
                //UAT-2372: Dismiss suffixes on names in supplements
                command.Parameters.AddWithValue("@IgnoreSuffixesNames", IgnoreSuffixesNames);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > AppConsts.NONE)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }
        #endregion

        #region UAT-2117:"Continue" button behavior
        Dictionary<String, String> IBackgroundProcessOrderRepository.CheckBackgroundOrderToUpdate(Int32 tenantId, Int32 masterOrderID, Int32 ordPackageSvcGrpID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            Dictionary<String, String> resultDic = new Dictionary<String, String>();
            resultDic.Add(AppConsts.IS_SUCCESS_INDICATOR_APPLICABLE, "false");
            resultDic.Add(AppConsts.IS_ALL_EXISTING_SEARCHES_ARE_CLEAR, "false");
            resultDic.Add(AppConsts.IS_OTHER_SERVICE_GROUPS_ARE_COMPLETED, "false");
            resultDic.Add(AppConsts.INSTITUTION_COLOR_FLAG_ID, "0");
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_CheckBkgOrderToApplySuccessIndicator", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantId", tenantId);
                command.Parameters.AddWithValue("@MasterOrderID", masterOrderID);
                command.Parameters.AddWithValue("@CurrentServiceGroupID", ordPackageSvcGrpID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > AppConsts.NONE)
                {
                    resultDic[AppConsts.IS_SUCCESS_INDICATOR_APPLICABLE] = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsApplySuccessIndicator"]) ? "true" : "false";
                    resultDic[AppConsts.IS_ALL_EXISTING_SEARCHES_ARE_CLEAR] = Convert.ToBoolean(ds.Tables[1].Rows[0]["IsAllSearchesClear"]) ? "true" : "false";
                    resultDic[AppConsts.INSTITUTION_COLOR_FLAG_ID] = ds.Tables[2].Rows[0]["InstitutionColorStatusID"] == DBNull.Value ? AppConsts.ZERO
                                                                       : Convert.ToString(ds.Tables[2].Rows[0]["InstitutionColorStatusID"]);
                    resultDic[AppConsts.IS_OTHER_SERVICE_GROUPS_ARE_COMPLETED] = Convert.ToBoolean(ds.Tables[3].Rows[0]["IsOtherServiceGroupsCompleted"]) ? "true" : "false";
                }
            }
            return resultDic;
        }
        #endregion

        #region UAT-2304: Random review of auto completed supplements

        /// <summary>
        /// Method to check the background order for all Svc Groups to update(check the success indicator is applicable and all existing searchs of order are clear)
        /// </summary>
        /// <param name="masterOrderID">MasterOrderID</param>
        /// <returns>Truth values for Success indicator and existing searches clear or not and InstitutionColorFlagID</returns>
        Dictionary<String, String> IBackgroundProcessOrderRepository.CheckBackgroundOrderForAllSvcGroupsToUpdate(Int32 masterOrderID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            Dictionary<String, String> resultDic = new Dictionary<String, String>();
            resultDic.Add(AppConsts.IS_SUCCESS_INDICATOR_APPLICABLE, "false");
            resultDic.Add(AppConsts.IS_ALL_EXISTING_SEARCHES_ARE_CLEAR, "false");
            resultDic.Add(AppConsts.IS_OTHER_SERVICE_GROUPS_ARE_COMPLETED, "false");
            resultDic.Add(AppConsts.INSTITUTION_COLOR_FLAG_ID, "0");
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_CheckBkgOrderForAllSvcGroupsToApplySuccessIndicator", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MasterOrderID", masterOrderID);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > AppConsts.NONE)
                {
                    resultDic[AppConsts.IS_SUCCESS_INDICATOR_APPLICABLE] = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsApplySuccessIndicator"]) ? "true" : "false";
                    resultDic[AppConsts.IS_ALL_EXISTING_SEARCHES_ARE_CLEAR] = Convert.ToBoolean(ds.Tables[1].Rows[0]["IsAllSearchesClear"]) ? "true" : "false";
                    resultDic[AppConsts.INSTITUTION_COLOR_FLAG_ID] = ds.Tables[2].Rows[0]["InstitutionColorStatusID"] == DBNull.Value ? AppConsts.ZERO
                                                                       : Convert.ToString(ds.Tables[2].Rows[0]["InstitutionColorStatusID"]);
                    resultDic[AppConsts.IS_OTHER_SERVICE_GROUPS_ARE_COMPLETED] = Convert.ToBoolean(ds.Tables[3].Rows[0]["IsOtherServiceGroupsCompleted"]) ? "true" : "false";
                }
            }
            return resultDic;
        }

        /// <summary>
        /// Save Supplement Automation Status and History
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="supplementAutomationStatusList"></param>
        /// <param name="bkgSvcGrpStatusTypeCompletedID"></param>
        /// <param name="bkgSvcGrpReviewStatusTypeFirstReviewID"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        Boolean IBackgroundProcessOrderRepository.SaveSupplementAutomationStatusAndHistory(Int32 tenantID, Int32 orderID, List<lkpSupplementAutomationStatu> supplementAutomationStatusList, Int32 bkgSvcGrpStatusTypeCompletedID, Int32 bkgSvcGrpReviewStatusTypeFirstReviewID, Int32 loggedInUserId)
        {
            BkgOrder bkgOrder = _ClientDBContext.BkgOrders.Where(cond => cond.BOR_MasterOrderID == orderID).FirstOrDefault();

            if (bkgOrder.IsNotNull())
            {
                BkgOrderAutoCompleteStatu bkgOrderAutoCompleteStatus = _ClientDBContext.BkgOrderAutoCompleteStatus.Where(x => x.BOACS_BkgOrderID == bkgOrder.BOR_ID
                                                                        && x.BOACS_IsActive && !x.BOACS_IsDeleted).FirstOrDefault();
                if (bkgOrderAutoCompleteStatus.IsNotNull())
                {
                    bkgOrderAutoCompleteStatus.BOACS_IsActive = false;
                    bkgOrderAutoCompleteStatus.BOACS_ModifiedBy = loggedInUserId;
                    bkgOrderAutoCompleteStatus.BOACS_ModifiedOn = DateTime.Now;

                    CreatebkgOrderAutoCompleteStatusAndHistoryObjects(tenantID, bkgOrder, supplementAutomationStatusList, bkgSvcGrpStatusTypeCompletedID, bkgSvcGrpReviewStatusTypeFirstReviewID, loggedInUserId);
                }
                else
                {
                    CreatebkgOrderAutoCompleteStatusAndHistoryObjects(tenantID, bkgOrder, supplementAutomationStatusList, bkgSvcGrpStatusTypeCompletedID, bkgSvcGrpReviewStatusTypeFirstReviewID, loggedInUserId);
                }

                if (_ClientDBContext.SaveChanges() > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;

        }

        /// <summary>
        /// Create bkg Order Auto Complete Status And History objects
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="bkgOrder"></param>
        /// <param name="supplementAutomationStatusList"></param>
        /// <param name="bkgSvcGrpStatusTypeCompletedID"></param>
        /// <param name="bkgSvcGrpReviewStatusTypeFirstReviewID"></param>
        /// <param name="loggedInUserId"></param>
        private void CreatebkgOrderAutoCompleteStatusAndHistoryObjects(Int32 tenantID, BkgOrder bkgOrder, List<lkpSupplementAutomationStatu> supplementAutomationStatusList, Int32 bkgSvcGrpStatusTypeCompletedID, Int32 bkgSvcGrpReviewStatusTypeFirstReviewID, Int32 loggedInUserId)
        {
            Int32 oldOrderStatusTypeID = bkgOrder.BOR_OrderStatusTypeID.Value;

            BkgOrderAutoCompleteStatu bkgOrderAutoCompleteStatusObj = new BkgOrderAutoCompleteStatu();
            bkgOrderAutoCompleteStatusObj.BOACS_BkgOrderID = bkgOrder.BOR_ID;
            bkgOrderAutoCompleteStatusObj.BOACS_IsActive = true;
            bkgOrderAutoCompleteStatusObj.BOACS_IsDeleted = false;
            bkgOrderAutoCompleteStatusObj.BOACS_CreatedBy = loggedInUserId;
            bkgOrderAutoCompleteStatusObj.BOACS_CreatedOn = DateTime.Now;

            BkgOrderStatusHistory bkgOrderStatusHistory = new BkgOrderStatusHistory();
            bkgOrderStatusHistory.BOSH_BkgOrderStatusID = oldOrderStatusTypeID;
            bkgOrderStatusHistory.BOSH_IsDeleted = false;
            bkgOrderStatusHistory.BOSH_CreatedBy = loggedInUserId;
            bkgOrderStatusHistory.BOSH_CreatedOn = DateTime.Now;

            bkgOrderAutoCompleteStatusObj.BkgOrderStatusHistories.Add(bkgOrderStatusHistory);

            //Get Bkg Order Package Svc Group List
            IEnumerable<Int32> bop_ids = bkgOrder.BkgOrderPackages.Where(cond => !cond.BOP_IsDeleted).Select(col => col.BOP_ID);
            List<BkgOrderPackageSvcGroup> lstBkgOrderPackageSvcGroup = _ClientDBContext.BkgOrderPackageSvcGroups.Where(con => bop_ids.Contains(con.OPSG_BkgOrderPackageID)
                                                                                      && !con.OPSG_IsDeleted && con.OPSG_SvcGrpReviewStatusTypeID == bkgSvcGrpReviewStatusTypeFirstReviewID
                                                                                      && con.OPSG_SvcGrpStatusTypeID != bkgSvcGrpStatusTypeCompletedID).ToList();

            lstBkgOrderPackageSvcGroup.ForEach(bkgOrderPkgSvcGrp =>
            {
                BkgOrderStatusHistory bkgOrderStatusHistoryObj = new BkgOrderStatusHistory();
                bkgOrderStatusHistoryObj.BOSH_BkgOrderStatusID = oldOrderStatusTypeID;
                bkgOrderStatusHistoryObj.BOSH_BkgOrderPackageSvcGroupID = bkgOrderPkgSvcGrp.OPSG_ID;
                bkgOrderStatusHistoryObj.BOSH_BkgSvcGrpStatusID = bkgOrderPkgSvcGrp.OPSG_SvcGrpStatusTypeID;
                bkgOrderStatusHistoryObj.BOSH_BkgSvcGrpReviewStatusID = bkgOrderPkgSvcGrp.OPSG_SvcGrpReviewStatusTypeID;
                bkgOrderStatusHistoryObj.BOSH_IsDeleted = false;
                bkgOrderStatusHistoryObj.BOSH_CreatedBy = loggedInUserId;
                bkgOrderStatusHistoryObj.BOSH_CreatedOn = DateTime.Now;

                bkgOrderAutoCompleteStatusObj.BkgOrderStatusHistories.Add(bkgOrderStatusHistoryObj);

                //Update Supplement Automation Pending Review Status ID in BkgOrderPackageSvcGroup table
                String supplementAutomationPendingReviewCode = SupplementAutomationStatus.PENDING_REVIEW.GetStringValue();
                String supplementAutomationReviewedCode = SupplementAutomationStatus.REVIEWED.GetStringValue();
                Int32 supplementAutomationPendingReviewID = supplementAutomationStatusList.FirstOrDefault(x => x.SAS_Code == supplementAutomationPendingReviewCode).SAS_ID;
                Int32 supplementAutomationReviewedID = supplementAutomationStatusList.FirstOrDefault(x => x.SAS_Code == supplementAutomationReviewedCode).SAS_ID;

                //Check Supplement Automation Svc Grp for Randon Review
                String bkgOrderPackageSvcGroupIDs = String.Join(",", lstBkgOrderPackageSvcGroup.Select(x => x.OPSG_ID));
                Dictionary<Int32, Boolean> selectedBkgOrderPackageSvcGroupIDs = CheckSupplementAutoSvcGrpForRandonReview(tenantID, bkgOrderPackageSvcGroupIDs);

                selectedBkgOrderPackageSvcGroupIDs.ForEach(x =>
                    {
                        if (x.Key == bkgOrderPkgSvcGrp.OPSG_ID)
                        {
                            if (x.Value == true)
                            {
                                bkgOrderPkgSvcGrp.OPSG_SupplementAutomationStatusID = supplementAutomationPendingReviewID;
                                bkgOrderPkgSvcGrp.OPSG_ModifiedByID = loggedInUserId;
                                bkgOrderPkgSvcGrp.OPSG_ModifiedOn = DateTime.Now;
                            }
                            else
                            {
                                bkgOrderPkgSvcGrp.OPSG_SupplementAutomationStatusID = supplementAutomationReviewedID;
                                bkgOrderPkgSvcGrp.OPSG_ModifiedByID = loggedInUserId;
                                bkgOrderPkgSvcGrp.OPSG_ModifiedOn = DateTime.Now;
                            }
                        }
                    });

            });

            _ClientDBContext.BkgOrderAutoCompleteStatus.AddObject(bkgOrderAutoCompleteStatusObj);
        }

        /// <summary>
        /// Check Supplement Automation Svc Grp for Randon Review
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="bkgOrderPackageSvcGroupIDs"></param>
        /// <returns></returns>
        public Dictionary<Int32, Boolean> CheckSupplementAutoSvcGrpForRandonReview(Int32 tenantID, String bkgOrderPackageSvcGroupIDs)
        {
            List<Int32> lstBkgOrderPackageSvcGroupID = new List<Int32>();
            Dictionary<Int32, Boolean> dicResult = new Dictionary<Int32, Boolean>();


            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_CheckSupplementAutoSvcGrpForRandonReview", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantID", tenantID);
                command.Parameters.AddWithValue("@BkgOrderPackageSvcGroupIds", bkgOrderPackageSvcGroupIDs);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > AppConsts.NONE)
                {
                    if (ds.Tables[0].Rows.Count > AppConsts.NONE)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            dicResult.Add(Convert.ToInt32(row["BkgOrderPackageSvcGroupID"]), Convert.ToBoolean(row["IsSelected"]));
                        }
                    }
                }
                return dicResult;
            }
        }

        /// <summary>
        /// Update Supplement Automation Status of order Pkg Svc Group
        /// </summary>
        /// <param name="orderPkgSvcGroupID"></param>
        /// <param name="supplementAutomationReviewedStatusID"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        Boolean IBackgroundProcessOrderRepository.UpdateSupplementAutomationStatus(Int32 orderPkgSvcGroupID, Int32 supplementAutomationReviewedStatusID, Int32 loggedInUserId)
        {
            BkgOrderPackageSvcGroup bkgOrderPackageSvcGroup = GetOrderPackageServiceGroupData(orderPkgSvcGroupID);

            if (bkgOrderPackageSvcGroup.IsNotNull())
            {
                bkgOrderPackageSvcGroup.OPSG_SupplementAutomationStatusID = supplementAutomationReviewedStatusID;
                bkgOrderPackageSvcGroup.OPSG_ModifiedByID = loggedInUserId;
                bkgOrderPackageSvcGroup.OPSG_ModifiedOn = DateTime.Now;

                if (_ClientDBContext.SaveChanges() > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        /// <summary>
        /// Rollback Supplement Automation
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="orderPkgSvcGroupID"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        Boolean IBackgroundProcessOrderRepository.RollbackSupplementAutomation(Int32 orderID, Int32 orderPkgSvcGroupID, Int32 loggedInUserId)
        {
            BkgOrder bkgOrder = _ClientDBContext.BkgOrders.FirstOrDefault(cond => cond.BOR_MasterOrderID == orderID && cond.BOR_IsDeleted == false);

            if (bkgOrder.IsNotNull())
            {
                BkgOrderAutoCompleteStatu bkgOrderAutoCompleteStatus = _ClientDBContext.BkgOrderAutoCompleteStatus.FirstOrDefault(con => con.BOACS_BkgOrderID == bkgOrder.BOR_ID
                                                                            && con.BOACS_IsActive && !con.BOACS_IsDeleted);

                if (bkgOrderAutoCompleteStatus.IsNotNull())
                {
                    var bkgOrderStatusHistoryList = bkgOrderAutoCompleteStatus.BkgOrderStatusHistories.Where(x => !x.BOSH_IsDeleted).ToList();
                    //x.BOSH_BkgOrderPackageSvcGroupID == orderPkgSvcGroupID

                    if (!bkgOrderStatusHistoryList.IsNullOrEmpty())
                    {
                        var bkgOrderStatusID = bkgOrderStatusHistoryList.FirstOrDefault().BOSH_BkgOrderStatusID;

                        bkgOrder.BOR_OrderStatusTypeID = bkgOrderStatusID;
                        bkgOrder.BOR_OrderCompleteDate = null;
                        bkgOrder.BOR_ModifiedByID = loggedInUserId;
                        bkgOrder.BOR_ModifiedOn = DateTime.Now;

                        var bkgOrderStatusHistory = bkgOrderStatusHistoryList.FirstOrDefault(x => x.BOSH_BkgOrderPackageSvcGroupID == orderPkgSvcGroupID);

                        if (bkgOrderStatusHistory.IsNotNull())
                        {
                            BkgOrderPackageSvcGroup bkgOrderPackageSvcGroup = GetOrderPackageServiceGroupData(orderPkgSvcGroupID);

                            if (bkgOrderPackageSvcGroup.IsNotNull())
                            {
                                bkgOrderPackageSvcGroup.OPSG_SvcGrpStatusTypeID = bkgOrderStatusHistory.BOSH_BkgSvcGrpStatusID;
                                bkgOrderPackageSvcGroup.OPSG_SvcGrpReviewStatusTypeID = bkgOrderStatusHistory.BOSH_BkgSvcGrpReviewStatusID;
                                bkgOrderPackageSvcGroup.OPSG_SvcGrpCompletionDate = null;
                                bkgOrderPackageSvcGroup.OPSG_SupplementAutomationStatusID = null;
                                bkgOrderPackageSvcGroup.OPSG_ModifiedByID = loggedInUserId;
                                bkgOrderPackageSvcGroup.OPSG_ModifiedOn = DateTime.Now;
                            }
                        }

                        if (_ClientDBContext.SaveChanges() > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }

            return false;
        }

        #endregion

        #region UAT-2319

        DataTable IBackgroundProcessOrderRepository.GetPackageDocumentDataPoints(Int32 currentLoggedInUserId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetBackgroundDataPointMappingToSyncData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        /// <summary>
        /// Data copy from Background to compliance package
        /// </summary>
        /// <param name="packageSubscriptionID"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="docXml"></param>
        /// <param name="tenantId"></param>
        /// <param name="BkgComplianceMappingIDs"></param>
        /// <param name="ItemIDs"></param>
        List<Int32> IBackgroundProcessOrderRepository.SyncDataForNewMapping(Int32 packageSubscriptionID, Int32 currentLoggedInUserId, String docXml, Int32 tenantId, String BkgCompliancePackageMappingIDs, String ItemIDs, String MasterOrderIDs)
        {

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            List<Int32> lstAffectedCategoryDataIDs = new List<Int32>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_SyncDataForNewMapping", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageSubscriptionID", packageSubscriptionID);
                command.Parameters.AddWithValue("@SourceXML", docXml);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                command.Parameters.AddWithValue("@TenantID", tenantId);
                command.Parameters.AddWithValue("@BkgCompliancePackageMappingIDs", BkgCompliancePackageMappingIDs);
                command.Parameters.AddWithValue("@ItemIDs", ItemIDs);
                command.Parameters.AddWithValue("@MasterOrderIDs", MasterOrderIDs);
                //con.Open();
                //command.ExecuteNonQuery();
                //command.Dispose();
                //con.Close();
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dRow in ds.Tables[0].Rows)
                    {
                        lstAffectedCategoryDataIDs.Add(Convert.ToInt32(dRow["ApplicantComplianceCategoryID"]));
                    }
                }
            }
            return lstAffectedCategoryDataIDs;
        }
        #endregion
        //UAT-2370 : Supplement SSN Processing updates.
        List<BkgSSNExceptionNotificationDataContract> IBackgroundProcessOrderRepository.SendEmailWhenExceptionInSSNResult(Int32 BkgOrderID, String vendorBkgOrderLineItemDetailIDs)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetDataWhenExceptionInSSNResult", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderId", BkgOrderID);
                command.Parameters.AddWithValue("@ExtVendorBkgOrderLineItemDetailIdList", vendorBkgOrderLineItemDetailIDs);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                List<BkgSSNExceptionNotificationDataContract> lstBkgSSNExceptionNotificationDataContract = new List<BkgSSNExceptionNotificationDataContract>();
                lstBkgSSNExceptionNotificationDataContract = ds.Tables[0].AsEnumerable().Select(col =>
                      new BkgSSNExceptionNotificationDataContract
                      {
                          OrganizationUserID = col["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["OrganizationUserID"]),
                          BkgServiceID = col["BkgServiceID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["BkgServiceID"]),
                          ApplicantName = col["ApplicantName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ApplicantName"]),
                          SSN = col["SSN"] == DBNull.Value ? String.Empty : Convert.ToString(col["SSN"]),
                          ResultText = col["ResultText"] == DBNull.Value ? String.Empty : Convert.ToString(col["ResultText"]),
                          VendorOrderLineItem = col["VendorOrderLineItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["VendorOrderLineItemID"]),
                          BkgOrderID = col["BkgOrderID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["BkgOrderID"]),
                          SelectedNodeID = col["SelectedNodeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["SelectedNodeID"])
                      }).ToList();

                return lstBkgSSNExceptionNotificationDataContract;
            }
        }

        #region UAT-2842-- Admin Create Screening Order

        List<AdminCreateOrderContract> IBackgroundProcessOrderRepository.GetAdminCreateOrderSearchData(AdminOrderSearchContract searchContract, CustomPagingArgsContract gridCustomePaging)
        {
            String orderBy = "OrderID";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(gridCustomePaging.SortExpression) ? orderBy : gridCustomePaging.SortExpression;
            ordDirection = gridCustomePaging.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetAdminCreateOrderSearchData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderNumber", searchContract.OrderNumber.IsNullOrEmpty() ? null : searchContract.OrderNumber);
                command.Parameters.AddWithValue("@FirstName", searchContract.FirstName.IsNullOrEmpty() ? null : searchContract.FirstName);
                command.Parameters.AddWithValue("@LastName", searchContract.LastName.IsNullOrEmpty() ? null : searchContract.LastName);
                command.Parameters.AddWithValue("@SSN", searchContract.SSN.IsNullOrEmpty() ? null : searchContract.SSN);
                command.Parameters.AddWithValue("@DOB", searchContract.DOB.IsNullOrEmpty() ? null : searchContract.DOB);
                command.Parameters.AddWithValue("@ReadyToTransmit", searchContract.ReadyToTransmit.IsNullOrEmpty() ? null : searchContract.ReadyToTransmit);
                command.Parameters.AddWithValue("@OrderHierarchy", searchContract.OrderHierarchy.IsNullOrEmpty() ? null : searchContract.OrderHierarchy);
                command.Parameters.AddWithValue("@OrderBy", orderBy);
                command.Parameters.AddWithValue("@OrderDirection", ordDirection);
                command.Parameters.AddWithValue("@PageIndex", gridCustomePaging.CurrentPageIndex);
                command.Parameters.AddWithValue("@PageSize", gridCustomePaging.PageSize);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                List<AdminCreateOrderContract> lstAdminOrders = new List<AdminCreateOrderContract>();
                lstAdminOrders = ds.Tables[0].AsEnumerable().Select(col =>
                      new AdminCreateOrderContract
                      {
                          OrderID = col["OrderID"] == DBNull.Value ? 0 : Convert.ToInt32(col["OrderID"]),
                          OrderNumber = col["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["OrderNumber"]),
                          FirstName = col["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["FirstName"]),
                          LastName = col["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["LastName"]),
                          SSN = col["SSN"] == DBNull.Value ? String.Empty : Convert.ToString(col["SSN"]),
                          DOB = col["DOB"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["DOB"]),
                          IsReadyToTransmit = col["IsReadyToTransmit"] == DBNull.Value ? String.Empty : Convert.ToString(col["IsReadyToTransmit"]),
                          OrderHierarchy = col["OrderHierarchy"] == DBNull.Value ? String.Empty : Convert.ToString(col["OrderHierarchy"]),
                          EntryDate = col["OrderDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["OrderDate"]),
                          TotalCount = col["TotalCount"] == DBNull.Value ? 0 : Convert.ToInt32(col["TotalCount"])
                      }).ToList();

                return lstAdminOrders;
            }
        }
        #endregion

        #region UAT-2842

        OrganizationUser IBackgroundProcessOrderRepository.GetOrganisationUserByUserID(Guid UserID)
        {
            OrganizationUser orgUserDetails = _ClientDBContext.OrganizationUsers.Where(cond => cond.UserID == UserID && cond.IsDeleted == true).FirstOrDefault();

            if (orgUserDetails.IsNullOrEmpty())
            {
                return new OrganizationUser();
            }
            else
            {
                return orgUserDetails;
            }
        }

        DataTable IBackgroundProcessOrderRepository.GetBkgPackageDetailsForAdminOrder(String DPMIds)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("ams.usp_GetBkgPackageForAdminOrder", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@DPMIds", DPMIds);
                SqlDataAdapter _adp = new SqlDataAdapter();
                _adp.SelectCommand = _command;
                DataSet _ds = new DataSet();
                _adp.Fill(_ds);
                if (_ds.Tables.Count > 0)
                    return _ds.Tables[0];
            }

            return new DataTable();
        }

        List<Int32> IBackgroundProcessOrderRepository.GetBkgSvcGroupDetailsByBkgPkgId(Int32 bkgPackageId)
        {
            return _ClientDBContext.BkgPackageSvcGroups.Where(cond => !cond.BPSG_IsDeleted && cond.BPSG_BackgroundPackageID == bkgPackageId
                                    && cond.BkgSvcGroup != null && !cond.BkgSvcGroup.BSG_IsDeleted).Select(sel => sel.BPSG_BkgSvcGroupID.Value).ToList();
        }

        List<Int32> IBackgroundProcessOrderRepository.GetBackgroundServiceIdsBysvcGrpId(Int32 SvcGrpId, Int32 bkgPackageId)
        {
            return _ClientDBContext.BkgPackageSvcs.Where(cond => !cond.BPS_IsDeleted
                                                         && cond.BkgPackageSvcGroup != null
                                                         && !cond.BkgPackageSvcGroup.BPSG_IsDeleted
                                                  && cond.BackgroundService != null
                                                  && !cond.BackgroundService.BSE_IsDeleted
                                                  && cond.BkgPackageSvcGroup.BPSG_BackgroundPackageID == bkgPackageId
                                                  && cond.BkgPackageSvcGroup.BPSG_BkgSvcGroupID == SvcGrpId)
                                                  .Select(sel => sel.BPS_BackgroundServiceID).ToList();
        }

        Order IBackgroundProcessOrderRepository.SaveAdminOrderDetails(Order orderDetails)
        {
            if (orderDetails.OrderID <= AppConsts.NONE)
            {
                _ClientDBContext.Orders.AddObject(orderDetails);
            }

            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
                return orderDetails;
            return orderDetails;
        }
        DataSet IBackgroundProcessOrderRepository.GetAdminOrderDetailsByOrderId(Int32 OrderId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("ams.usp_GetAdminOrderDetailsByOrderId", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@OrderId", OrderId);
                SqlDataAdapter _adp = new SqlDataAdapter();
                _adp.SelectCommand = _command;
                DataSet _ds = new DataSet();
                _adp.Fill(_ds);
                if (_ds.Tables.Count > 0)
                    return _ds;
            }

            return new DataSet();
        }

        Boolean IBackgroundProcessOrderRepository.IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, Int32 ApplicantOrgUserID, Int32 ApplicantDocTypeId)
        {
            ApplicantDocument ApplicantDocument = _ClientDBContext.ApplicantDocuments.Where(cond => !cond.IsDeleted
                                                                                             && cond.OrganizationUserID == ApplicantOrgUserID
                                                                                             && cond.FileName == documentName
                                                                                             && cond.Size == documentSize
                                                                                             && cond.DocumentType == ApplicantDocTypeId).FirstOrDefault();
            if (ApplicantDocument.IsNullOrEmpty())
                return false;
            return true;
        }

        Boolean IBackgroundProcessOrderRepository.IsDandAAlreadyUploaded(Int32 ApplicantOrgUserID, Int32 ApplicantDocTypeId)
        {
            ApplicantDocument DandADocument = _ClientDBContext.ApplicantDocuments.Where(cond => !cond.IsDeleted
                                                                                        && cond.OrganizationUserID == ApplicantOrgUserID
                                                                                        && cond.DocumentType == ApplicantDocTypeId).FirstOrDefault();
            if (DandADocument.IsNullOrEmpty())
                return false;
            return true;
        }



        Boolean IBackgroundProcessOrderRepository.SaveApplicantDocumentDetails(List<ApplicantDocument> lstApplicantDocument)
        {
            lstApplicantDocument.ForEach(x =>
            {
                _ClientDBContext.ApplicantDocuments.AddObject(x);
            });
            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        List<ApplicantDocumentContract> IBackgroundProcessOrderRepository.GetApplicantDocuments(Int32 organizationUserId)
        {
            List<ApplicantDocumentContract> lstApplicantDocuments = (from ad in _ClientDBContext.ApplicantDocuments
                                                                     join ldt in _ClientDBContext.lkpDocumentTypes on ad.DocumentType equals ldt.DMT_ID
                                                                     where !ad.IsDeleted && !ldt.DMT_IsDeleted && (ad.OrganizationUserID == organizationUserId)
                                                                     select new ApplicantDocumentContract
                                                                     {
                                                                         ApplicantDocumentID = ad.ApplicantDocumentID,
                                                                         DocumentTypeName = ldt.DMT_Name,
                                                                         FileName = ad.FileName,
                                                                     }).ToList();

            return lstApplicantDocuments;
        }

        Boolean IBackgroundProcessOrderRepository.DeleteApplicantDocuments(Int32 organizationUserID, Int32 currentLoggedInUserId)
        {
            List<ApplicantDocument> lstdeletedDocuments = _ClientDBContext.ApplicantDocuments.Where(cond => !cond.IsDeleted && cond.OrganizationUserID == organizationUserID).ToList();
            lstdeletedDocuments.ForEach(x =>
            {
                x.IsDeleted = true;
                x.ModifiedByID = currentLoggedInUserId;
                x.ModifiedOn = DateTime.Now;
            });

            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        Boolean IBackgroundProcessOrderRepository.DeleteAdminOrderDetails(Int32 currentLoggedInUserId, Int32 orderId)
        {
            BkgOrder BkgOrders = _ClientDBContext.BkgOrders.Where(cond => cond.BOR_MasterOrderID == orderId).FirstOrDefault();

            if (!BkgOrders.IsNullOrEmpty())
            {
                BkgOrders.BkgAdminOrderDetails.ForEach(x =>
                 {
                     x.BAOD_IsDeleted = true;
                     x.BAOD_ModifiedBy = currentLoggedInUserId;
                     x.BAOD_ModifiedOn = DateTime.Now;
                 });

                BkgOrders.BkgOrderPackages.ForEach(x =>
                 {
                     x.BOP_IsDeleted = true;
                     x.BOP_ModifiedByID = currentLoggedInUserId;
                     x.BOP_ModifiedOn = DateTime.Now;
                 });
            }

            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        Order IBackgroundProcessOrderRepository.GetAdminOrderDataByOrderId(Int32 OrderId)
        {
            return _ClientDBContext.Orders.Where(cond => cond.IsDeleted && cond.OrderID == OrderId).FirstOrDefault();
        }

        Boolean IBackgroundProcessOrderRepository.SaveCustomFormDetails(List<BackgroundOrderData> lstBkgOrderData, Int32 BkgOrderID, Int32 currentLoggedInUserId)
        {
            List<CustomFormDataGroup> lstCustomFormDataGroup = _ClientDBContext.CustomFormDataGroups.Where(cond => !cond.CFDG_IsDeleted && cond.CFDG_BkgOrderID == BkgOrderID).ToList();

            if (!lstCustomFormDataGroup.IsNullOrEmpty())
            {
                var lstBkgOrderForRemove = lstBkgOrderData.Select(sel => new { sel.CustomFormId, sel.InstanceId, sel.BkgSvcAttributeGroupId }).Distinct().ToList();

                List<CustomFormDataGroup> lstCurrentChangeCustomForm = lstCustomFormDataGroup.Where(cond => lstBkgOrderForRemove.Select(sel => sel.CustomFormId).ToList().Contains(cond.CFDG_CustomFormID.Value)
                                   && lstBkgOrderForRemove.Select(sel => sel.InstanceId).ToList().Contains(cond.CFDG_InstanceID.Value)
                                   && lstBkgOrderForRemove.Select(sel => sel.BkgSvcAttributeGroupId).ToList().Contains(cond.CFDG_BkgSvcAttributeGroupID.Value)
                                    ).ToList();

                List<CustomFormDataGroup> DataToRemove = lstCustomFormDataGroup.Except(lstCurrentChangeCustomForm).ToList();

                DataToRemove.ForEach(x =>
                {
                    x.CFDG_IsDeleted = true;
                    x.CFDG_ModifiedBy = currentLoggedInUserId;
                    x.CFDG_ModifiedOn = DateTime.Now;

                    x.CustomFormOrderDatas.ForEach(y =>
                    {
                        y.CFOD_IsDeleted = true;
                        y.CFOD_ModifiedBy = currentLoggedInUserId;
                        y.CFOD_ModifiedOn = DateTime.Now;

                        y.CustomFormOrderExtraDatas.ForEach(z =>
                        {
                            z.CFOED_IsDeleted = true;
                            z.CFOED_ModifiedBy = currentLoggedInUserId;
                            z.CFOED_ModifiedOn = DateTime.Now;
                        });
                    });
                });
            }

            foreach (var bkgOrderData in lstBkgOrderData)
            {
                CustomFormDataGroup customformData = lstCustomFormDataGroup.Where(cond => cond.CFDG_BkgSvcAttributeGroupID == bkgOrderData.BkgSvcAttributeGroupId && cond.CFDG_CustomFormID == bkgOrderData.CustomFormId && cond.CFDG_InstanceID == bkgOrderData.InstanceId).FirstOrDefault();

                if (customformData.IsNullOrEmpty())
                {
                    CustomFormDataGroup _customFormDataGroup = new CustomFormDataGroup
                    {
                        CFDG_BkgOrderID = BkgOrderID,
                        CFDG_BkgSvcAttributeGroupID = bkgOrderData.BkgSvcAttributeGroupId,
                        CFDG_CustomFormID = bkgOrderData.CustomFormId,
                        CFDG_InstanceID = bkgOrderData.InstanceId,
                        CFDG_IsDeleted = false,
                        CFDG_CreatedBy = currentLoggedInUserId,
                        CFDG_CreatedOn = DateTime.Now
                    };

                    foreach (var customFormData in bkgOrderData.CustomFormData)
                    {
                        CustomFormOrderData _customFormOrderData = new CustomFormOrderData
                        {
                            CustomFormDataGroup = _customFormDataGroup,
                            CFOD_BkgAttributeGroupMappingID = customFormData.Key,
                            CFOD_Value = customFormData.Value,
                            CFOD_IsDeleted = false,
                            CFOD_ModifiedBy = currentLoggedInUserId,
                            CFOD_ModifiedOn = DateTime.Now
                        };

                        //UAT-2447
                        if (!bkgOrderData.CustomFormIntPhoneNumExtraData.IsNullOrEmpty() && bkgOrderData.CustomFormIntPhoneNumExtraData.ContainsKey(customFormData.Key)
                            && Convert.ToBoolean(bkgOrderData.CustomFormIntPhoneNumExtraData[customFormData.Key]))
                        {
                            CustomFormOrderExtraData _customFormOrderExtraData = new CustomFormOrderExtraData();
                            _customFormOrderExtraData.CustomFormOrderData = _customFormOrderData;
                            _customFormOrderExtraData.CFOED_IsInternationalPhone = Convert.ToBoolean(bkgOrderData.CustomFormIntPhoneNumExtraData[customFormData.Key]);
                            _customFormOrderExtraData.CFOED_IsDeleted = false;
                            _customFormOrderExtraData.CFOED_CreatedBy = currentLoggedInUserId;
                            _customFormOrderExtraData.CFOED_CreatedOn = DateTime.Now;
                        }

                    }

                    _ClientDBContext.CustomFormDataGroups.AddObject(_customFormDataGroup);
                }
                else
                {
                    foreach (var customFormData in bkgOrderData.CustomFormData)
                    {
                        CustomFormOrderData customFormOrderData = customformData.CustomFormOrderDatas.Where(cond => cond.CFOD_IsDeleted == false && cond.CFOD_BkgAttributeGroupMappingID == customFormData.Key).FirstOrDefault();
                        if (customFormOrderData.IsNullOrEmpty())
                        {
                            CustomFormOrderData _customFormOrderData = new CustomFormOrderData
                            {
                                CustomFormDataGroup = customformData,
                                CFOD_BkgAttributeGroupMappingID = customFormData.Key,
                                CFOD_Value = customFormData.Value,
                                CFOD_IsDeleted = false,
                                CFOD_ModifiedBy = currentLoggedInUserId,
                                CFOD_ModifiedOn = DateTime.Now
                            };

                            //UAT-2447
                            if (!bkgOrderData.CustomFormIntPhoneNumExtraData.IsNullOrEmpty() && bkgOrderData.CustomFormIntPhoneNumExtraData.ContainsKey(customFormData.Key)
                                && Convert.ToBoolean(bkgOrderData.CustomFormIntPhoneNumExtraData[customFormData.Key]))
                            {
                                CustomFormOrderExtraData _customFormOrderExtraData = new CustomFormOrderExtraData();
                                _customFormOrderExtraData.CustomFormOrderData = _customFormOrderData;
                                _customFormOrderExtraData.CFOED_IsInternationalPhone = Convert.ToBoolean(bkgOrderData.CustomFormIntPhoneNumExtraData[customFormData.Key]);
                                _customFormOrderExtraData.CFOED_IsDeleted = false;
                                _customFormOrderExtraData.CFOED_CreatedBy = currentLoggedInUserId;
                                _customFormOrderExtraData.CFOED_CreatedOn = DateTime.Now;
                            }
                        }
                        else
                        {
                            customFormOrderData.CFOD_BkgAttributeGroupMappingID = customFormData.Key;
                            customFormOrderData.CFOD_Value = customFormData.Value;
                            customFormOrderData.CFOD_ModifiedBy = currentLoggedInUserId;
                            customFormOrderData.CFOD_ModifiedOn = DateTime.Now;

                            //UAT-2447
                            if (!bkgOrderData.CustomFormIntPhoneNumExtraData.IsNullOrEmpty() && bkgOrderData.CustomFormIntPhoneNumExtraData.ContainsKey(customFormData.Key)
                                && Convert.ToBoolean(bkgOrderData.CustomFormIntPhoneNumExtraData[customFormData.Key]))
                            {
                                CustomFormOrderExtraData customFormOrderExtraData = customFormOrderData.CustomFormOrderExtraDatas.FirstOrDefault();
                                customFormOrderExtraData.CFOED_IsInternationalPhone = Convert.ToBoolean(bkgOrderData.CustomFormIntPhoneNumExtraData[customFormData.Key]);
                                customFormOrderExtraData.CFOED_ModifiedBy = currentLoggedInUserId;
                                customFormOrderExtraData.CFOED_ModifiedOn = DateTime.Now;
                            }
                        }
                    }
                }
            }

            //BkgAdminOrderDetail bkgAdminOrderDetails = _ClientDBContext.BkgAdminOrderDetails.Where(cond => cond.BAOD_BkgOrderID == BkgOrderID && !cond.BAOD_IsDeleted).FirstOrDefault();
            //if (!bkgAdminOrderDetails.IsNullOrEmpty())
            //{
            //    String ReadyToTransmitCode = AdminOrderStatus.READY_FOR_TRANSMIT.GetStringValue();
            //    Int32 ReadyToTransmitOrderId = _ClientDBContext.lkpAdminOrderStatus.Where(cond => cond.LAOS_Code == ReadyToTransmitCode && !cond.LAOS_IsDeleted).FirstOrDefault().LAOS_ID;

            //    bkgAdminOrderDetails.BAOD_OrderStatusID = ReadyToTransmitOrderId;
            //    bkgAdminOrderDetails.BAOD_ModifiedBy = currentLoggedInUserId;
            //    bkgAdminOrderDetails.BAOD_ModifiedOn = DateTime.Now;
            //}

            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IBackgroundProcessOrderRepository.DeleteOldOrganizationUserProfileData(OrganizationUser OrgUserDetails)
        {
            OrgUserDetails.OrganizationUserProfiles.ForEach(x =>
            {
                x.IsDeleted = true;
            });

            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        Boolean IBackgroundProcessOrderRepository.DeleteCustomFormData(Int32 BkgOrderID, Int32 currentLoggedInUserId, String packageIDs)
        {
            List<CustomFormDataContract> lstCustomForms = GetCustomFormsForThePackage(packageIDs);
            List<Int32> lstCustomFormIDs = new List<Int32>();

            if (!lstCustomForms.IsNullOrEmpty())
            {
                lstCustomFormIDs = lstCustomForms.Select(slct => slct.customFormId).ToList();
            }


            List<CustomFormDataGroup> lstCustomFormDataGroup = _ClientDBContext.CustomFormDataGroups.Where(cond => !cond.CFDG_IsDeleted && cond.CFDG_BkgOrderID == BkgOrderID
                                                                                                           && !lstCustomFormIDs.Contains(cond.CFDG_CustomFormID.Value)).ToList();

            lstCustomFormDataGroup.ForEach(x =>
            {
                x.CFDG_IsDeleted = true;
                x.CFDG_ModifiedBy = currentLoggedInUserId;
                x.CFDG_ModifiedOn = DateTime.Now;

                x.CustomFormOrderDatas.ForEach(y =>
                {
                    y.CFOD_IsDeleted = true;
                    y.CFOD_ModifiedBy = currentLoggedInUserId;
                    y.CFOD_ModifiedOn = DateTime.Now;

                    y.CustomFormOrderExtraDatas.ForEach(z =>
                    {
                        z.CFOED_IsDeleted = true;
                        z.CFOED_ModifiedBy = currentLoggedInUserId;
                        z.CFOED_ModifiedOn = DateTime.Now;
                    });
                });
            });

            BkgAdminOrderDetail bkgAdminOrderDetails = _ClientDBContext.BkgAdminOrderDetails.Where(cond => cond.BAOD_BkgOrderID == BkgOrderID && !cond.BAOD_IsDeleted).FirstOrDefault();
            if (!bkgAdminOrderDetails.IsNullOrEmpty())
            {
                String DRAFT = AdminOrderStatus.DRAFT.GetStringValue();
                Int32 DraftOrderId = _ClientDBContext.lkpAdminOrderStatus.Where(cond => cond.LAOS_Code == DRAFT && !cond.LAOS_IsDeleted).FirstOrDefault().LAOS_ID;

                bkgAdminOrderDetails.BAOD_OrderStatusID = DraftOrderId;
                bkgAdminOrderDetails.BAOD_ModifiedBy = currentLoggedInUserId;
                bkgAdminOrderDetails.BAOD_ModifiedOn = DateTime.Now;
            }

            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }
        #endregion

        #region UAT-2842:(Transmit Order Functionality)
        Boolean IBackgroundProcessOrderRepository.TransmmitAdminOrders(Int32 tenantId, Int32 currentLoggedInUserId, List<Int32> orderIds)
        {
            String stringOrderIDs = String.Join(",", orderIds);
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("ams.usp_TransmitAdminOrder", con);
                _command.Parameters.AddWithValue("@MasterOrderIDs", stringOrderIDs);
                _command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                _command.Parameters.AddWithValue("@TenantID", tenantId);
                _command.CommandType = CommandType.StoredProcedure;
                con.Open();
                Int32 rowsAffected = _command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return true;
                }
                con.Close();
            }
            return true;
        }


        List<AdminOrderDetailReadyToTransmitContract> IBackgroundProcessOrderRepository.AdminOrderIsReadyToTransmit(Int32 tenantId, Int32 currentLoggedInUserId, String orderIds)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_AdminOrdersReadyToTransmit", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MasterOrderIDs", orderIds);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                command.Parameters.AddWithValue("@TenantID", tenantId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                List<AdminOrderDetailReadyToTransmitContract> lstAdminOrderDetailReadyToTransmitContract = new List<AdminOrderDetailReadyToTransmitContract>();
                lstAdminOrderDetailReadyToTransmitContract = ds.Tables[0].AsEnumerable().Select(col =>
                      new AdminOrderDetailReadyToTransmitContract
                      {
                          MasterOrderID = col["MasterOrderID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["MasterOrderID"]),
                          BkgOrderID = col["BkgOrderID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["BkgOrderID"]),
                          IsProfileExist = col["IsProfileExist"] == DBNull.Value ? false : Convert.ToBoolean(col["IsProfileExist"]),
                          IsPackageSelected = col["IsPackageSelected"] == DBNull.Value ? false : Convert.ToBoolean(col["IsPackageSelected"]),
                          IsOrderReadyToTransmit = col["IsOrderReadyToTransmit"] == DBNull.Value ? false : Convert.ToBoolean(col["IsOrderReadyToTransmit"]),
                          IsALLCustomFormDataExist = col["IsALLCustomFormDataExist"] == DBNull.Value ? false : Convert.ToBoolean(col["IsALLCustomFormDataExist"])

                      }).ToList();

                return lstAdminOrderDetailReadyToTransmitContract;
            }
        }

        Boolean IBackgroundProcessOrderRepository.IsAdminCreatedOrder(Int32 tenantId, Int32 orderId)
        {
            var bkgOrder = _ClientDBContext.BkgOrders.FirstOrDefault(x => x.BOR_MasterOrderID == orderId && !x.BOR_IsDeleted);
            if (!bkgOrder.IsNullOrEmpty())
            {
                return bkgOrder.BOR_IsAdminOrder;
            }
            return false;
        }

        Boolean IBackgroundProcessOrderRepository.SaveUpdateGenericDocumentmapping(Int16 recordTypeId, List<Int32> lstApplicantDocumentIds, Int32 currentLoggedInUserId, Int32 organizationUserProfileId)
        {
            List<GenericDocumentMapping> lstGenDocs = _ClientDBContext.GenericDocumentMappings.Where(cond => lstApplicantDocumentIds.Contains(cond.GDM_ApplicantDocumentID) && !cond.GDM_IsDeleted).ToList();
            List<Int32> lstApplicantDocIdsToUpdate = new List<Int32>();
            if (!lstGenDocs.IsNullOrEmpty())
            {
                lstApplicantDocIdsToUpdate = lstGenDocs.Select(sel => sel.GDM_ApplicantDocumentID).ToList();
            }
            List<Int32> lstApplicantDocumentIdsToAdd = lstApplicantDocumentIds.Except(lstApplicantDocIdsToUpdate).ToList();

            if (!lstApplicantDocumentIdsToAdd.IsNullOrEmpty())
            {
                //To save the Mapping.
                foreach (Int32 applicantDocId in lstApplicantDocumentIdsToAdd)
                {
                    GenericDocumentMapping GenericDoc = new GenericDocumentMapping();
                    GenericDoc.GDM_ApplicantDocumentID = applicantDocId;
                    GenericDoc.GDM_RecordID = organizationUserProfileId;
                    GenericDoc.GDM_RecordTypeID = recordTypeId;
                    GenericDoc.GDM_CreatedBy = currentLoggedInUserId;
                    GenericDoc.GDM_CreatedOn = DateTime.Now;
                    _ClientDBContext.GenericDocumentMappings.AddObject(GenericDoc);
                }
            }

            if (!lstApplicantDocIdsToUpdate.IsNullOrEmpty())
            {
                // to update the existing mapping.
                lstGenDocs.ForEach(x =>
                    {
                        x.GDM_ModifiedBy = currentLoggedInUserId;
                        x.GDM_ModifiedOn = DateTime.Now;
                        x.GDM_RecordID = organizationUserProfileId;
                    });
            }

            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        DataTable IBackgroundProcessOrderRepository.CheckOrderAvailabilityForTrasmit(String OrderIds)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("ams.usp_CheckOrderAvailabilityForTrasmit", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@OrderIds", OrderIds);
                SqlDataAdapter _adp = new SqlDataAdapter();
                _adp.SelectCommand = _command;
                DataSet _ds = new DataSet();
                _adp.Fill(_ds);
                if (_ds.Tables.Count > 0)
                    return _ds.Tables[0];
            }

            return new DataTable();
        }

        #endregion

        //UAT-2154

        BkgOrder IBackgroundProcessOrderRepository.GetBkgOrderByBkgOrderId(Int32 BkgOrderID)
        {
            return _ClientDBContext.BkgOrders.Where(cond => !cond.BOR_IsDeleted && cond.BOR_ID == BkgOrderID).FirstOrDefault();
        }

        #region UAT-2587:- Acknowledgement Popup data for Applicants

        List<BackroundServicesContract> IBackgroundProcessOrderRepository.AcknowledgeMessagePopUpContent(String bkgPackageIds, Int32 selectedNodeId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetServiceNameForAcknowledgementPopup", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BkgPackageIds", bkgPackageIds);
                command.Parameters.AddWithValue("@SelectedNodeId", selectedNodeId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                List<BackroundServicesContract> lstBkgServices = new List<BackroundServicesContract>();
                lstBkgServices = ds.Tables[0].AsEnumerable().Select(col =>
                      new BackroundServicesContract
                      {
                          ServiceID = col["ServiceID"] == DBNull.Value ? 0 : Convert.ToInt32(col["ServiceID"]),
                          ServicreName = col["ServiceName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ServiceName"]),

                      }).ToList();

                return lstBkgServices;
            }
        }
        #endregion

        #region UAT-3268:- Manage Additional Fee for Background package needed to Qualify for rotation.
        List<PkgAdditionalPaymentInfo> IBackgroundProcessOrderRepository.GetAdditionalPriceData(List<Int32> lstBkgHierarchyPkgId)
        {
            return (from bphm in _ClientDBContext.BkgPackageHierarchyMappings
                    join lpo in _ClientDBContext.lkpPaymentOptions on bphm.BPHM_AdditionalPricePaymentOptionID equals lpo.PaymentOptionID
                    join bpa in _ClientDBContext.BackgroundPackages on bphm.BPHM_BackgroundPackageID equals bpa.BPA_ID
                    where (!bphm.BPHM_IsDeleted && !lpo.IsDeleted && lstBkgHierarchyPkgId.Contains(bphm.BPHM_ID) && bpa.BPA_IsReqToQualifyInRotation)
                    select new PkgAdditionalPaymentInfo
                    {
                        BPHM_ID = bphm.BPHM_ID,
                        AdditionalPrice = bphm.BPHM_AdditionalPrice,
                        PackageID = bphm.BPHM_BackgroundPackageID,
                        BasePrice = bphm.BPHM_PackageBasePrice,
                        AdditionalPaymentOptionID = bphm.BPHM_AdditionalPricePaymentOptionID,
                        AdditionalPaymentOption = lpo.Name,
                        PackageName = bpa.BPA_Name,
                        PackageLable = bpa.BPA_Label
                    }).ToList();
        }

        List<OrderPaymentDetail> IBackgroundProcessOrderRepository.GetAdditionalPaymentModes(List<Int32> lstOpdIds)
        {
            var AdditionalPriceBkgPackageTypeCode = OrderPackageTypes.ADDITIONAL_PRICE_BACKGROUND_PACKAGE.GetStringValue();
            Int32 AdditionalPriceBkgPackageTypeId = _ClientDBContext.lkpOrderPackageTypes.Where(cond => !cond.OPT_IsDeleted && cond.OPT_Code == AdditionalPriceBkgPackageTypeCode).Select(Sel => Sel.OPT_ID).FirstOrDefault();
            return (List<OrderPaymentDetail>)(from opd in _ClientDBContext.OrderPaymentDetails
                                              join oppd in _ClientDBContext.OrderPkgPaymentDetails on opd.OPD_ID equals oppd.OPPD_OrderPaymentDetailID
                                              where (!opd.OPD_IsDeleted && !oppd.OPPD_IsDeleted && lstOpdIds.Contains(opd.OPD_ID) && oppd.OPPD_OrderPackageTypeID == AdditionalPriceBkgPackageTypeId)
                                              select opd).ToList();
            //{
            //     opd.OPD_ID
            //    //OPD_OnlinePaymentTransactionID = opd.OPD_OnlinePaymentTransactionID,
            //    //OPD_OrderID = opd.OPD_OrderID,
            //    //OPD_ReferenceNo = opd.OPD_ReferenceNo,
            //    //OPD_PaymentOptionID = opd.OPD_PaymentOptionID,
            //    //OPD_OrderStatusID = opd.OPD_OrderStatusID,
            //    //OPD_Amount = opd.OPD_Amount,
            //    //OPD_RejectionReason = opd.OPD_RejectionReason,
            //    //OPD_ApprovalDate = opd.OPD_ApprovalDate,
            //    //OPD_ApprovedBy = opd.OPD_ApprovedBy,
            //    //OPD_CustomerPaymentProfileID = opd.OPD_CustomerPaymentProfileID
            //}).ToList();
        }
        #endregion

        #region UAT-3453
        Boolean IBackgroundProcessOrderRepository.IsBkgOrderFlagged(Int32 masterOrderId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("[ams].[usp_IsBkgOrderFlagged]", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@MasterOrderID", masterOrderId);
                SqlDataAdapter _adp = new SqlDataAdapter();
                _adp.SelectCommand = _command;
                DataSet _ds = new DataSet();
                _adp.Fill(_ds);
                if (_ds.Tables.Count > AppConsts.NONE && _ds.Tables[0].Rows.Count > AppConsts.NONE && _ds.Tables[0].Rows[0]["IsOrderFlagged"].IsNotNull())
                {
                    return _ds.Tables[0].Rows[0]["IsOrderFlagged"] == DBNull.Value ? false : Convert.ToBoolean(_ds.Tables[0].Rows[0]["IsOrderFlagged"]);
                }
            }

            return false;
        }

        #endregion


        //UAT 3521 
        Dictionary<String, List<String>> IBackgroundProcessOrderRepository.GetDataForCascadingDropDown(String searchId, Int32 atrributeGroupId, Int32 AttributeID)
        {
            Int32 serviceAttributeID = 0;
            Dictionary<String, List<String>> dic = new Dictionary<String, List<String>>();
            List<BkgAttributeGroupMapping> attrGroupMapping = _ClientDBContext.BkgAttributeGroupMappings.Where(cond => !cond.BAGM_IsDeleted && cond.BAGM_BkgSvcAttributeGroupId == atrributeGroupId && cond.BAGM_SourceAttributeID == AttributeID).ToList();

            if (!attrGroupMapping.IsNullOrEmpty())
            {

                foreach (BkgAttributeGroupMapping item in attrGroupMapping.DistinctBy(cond => cond.BAGM_BkgSvcAtributeID))
                {
                    serviceAttributeID = item.BAGM_BkgSvcAtributeID;

                    if (serviceAttributeID > AppConsts.NONE)
                    {
                        List<String> lstValues = _ClientDBContext.CascadingAttributeOptions.Where(cond => cond.CAO_AttributeID == serviceAttributeID && cond.CAO_SourceValue == searchId && !cond.CAO_IsDeleted).Select(sel => sel.CAO_Value).Distinct().ToList();
                        dic.Add(item.BAGM_ID.ToString(), lstValues);
                    }
                }
            }
            return dic;
        }

        //UAT 3521 
        List<String> IBackgroundProcessOrderRepository.GetDataForBindingCascadingDropDown(Int32 attributeGroupID, Int32 attributeId, String searchID)
        {
            if (!searchID.IsNullOrEmpty())
            {
                return _ClientDBContext.CascadingAttributeOptions.Where(cond => cond.CAO_AttributeID == attributeId
                && cond.CAO_Value != null
                && cond.CAO_SourceValue == searchID
                && !cond.CAO_IsDeleted)
                .Select(sel => sel.CAO_Value)
                .Distinct()
                .ToList();
            }
            else
            {
                BkgAttributeGroupMapping attrGroupMapping = _ClientDBContext.BkgAttributeGroupMappings.Where(cond => !cond.BAGM_IsDeleted && cond.BAGM_BkgSvcAttributeGroupId == attributeGroupID && cond.BAGM_SourceAttributeID == null && cond.BAGM_BkgSvcAtributeID == attributeId).FirstOrDefault();
                if (!attrGroupMapping.IsNullOrEmpty())
                {
                    return _ClientDBContext.CascadingAttributeOptions.Where(cond => cond.CAO_AttributeID == attributeId && !cond.CAO_IsDeleted).Select(sel => sel.CAO_Value).Distinct().ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        //UAT 3573 CBI 
        String IBackgroundProcessOrderRepository.ValidatePageData(StringBuilder xmlStringData, Boolean IsCustomFormScreen, string languageCode)
        {
            String ErrorMessage = String.Empty;
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_ValidateCustomAttribute", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InputXml", xmlStringData.ToString());
                command.Parameters.AddWithValue("@IsCustomFormScreen", IsCustomFormScreen.ToString());
                command.Parameters.AddWithValue("@LanguageCode", languageCode.ToString());
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    ErrorMessage = Convert.ToString(ds.Tables[0].Rows[0]["ErrorMessage"]);
                }
                return ErrorMessage;
            }
        }


        List<CustomFormAutoFillDataContract> IBackgroundProcessOrderRepository.GetConditionsforAttributes(StringBuilder xmlStringData, String languageCode)
        {
            List<CustomFormAutoFillDataContract> lstCustomAttribute = new List<CustomFormAutoFillDataContract>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetConditionsforAttributes", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InputXml", xmlStringData.ToString());
                command.Parameters.AddWithValue("@LanguageCode", languageCode);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                lstCustomAttribute = ds.Tables[0].AsEnumerable().Select(col =>
                      new CustomFormAutoFillDataContract
                      {
                          AttributeGroupID = col["AttributeGroupID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["AttributeGroupID"]),
                          AttributeID = col["AttributeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["AttributeID"]),
                          IsAttributeHidden = col["IsAttributeHidden"] == DBNull.Value ? false : Convert.ToBoolean(col["IsAttributeHidden"]),
                          InstanceId = col["InstanceID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["InstanceID"]),
                          IsAttributeGroupHidden = col["IsAttributeGroupHidden"] == DBNull.Value ? false : Convert.ToBoolean(col["IsAttributeGroupHidden"]),
                          HeaderLabel = col["SectionTitle"].ToString(),
                      }).ToList();

                return lstCustomAttribute;
            }
        }

        #region UAT-36669
        DataTable IBackgroundProcessOrderRepository.GetAlertMailDataForWebCCFError(Int32 CurrentLogedInUserId, String BlockedOrderReasonCode)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAlertMailDataForWebCCFError", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CurrentLogedInUserId", CurrentLogedInUserId);
                command.Parameters.AddWithValue("@BlockedOrderReasonCode", BlockedOrderReasonCode);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.IsNotNull() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }

            }
            return new DataTable();

        }
        #endregion

        List<LookupContract> IBackgroundProcessOrderRepository.GetCustomAttributeOptionsData(String attributeName)
        {
            List<LookupContract> lstAttributeOption = new List<LookupContract>();
            BkgSvcAttribute bkgSvcAttributeData = _ClientDBContext.BkgSvcAttributes.Where(cond => cond.BSA_Name.Trim().ToLower() == attributeName.Trim().ToLower() && !cond.BSA_IsDeleted).FirstOrDefault();
            if (!bkgSvcAttributeData.IsNullOrEmpty())
            {
                lstAttributeOption.AddRange(_ClientDBContext.BkgSvcAttributeOptions.Where(cond => cond.EBSAO_BkgSvcAttributeID == bkgSvcAttributeData.BSA_ID && !cond.EBSAO_IsDeleted)
                    .Select(sel => new LookupContract { ID = sel.EBSAO_BkgSvcAttributeID.Value, Name = sel.EBSAO_OptionText }).ToList());
            }
            return lstAttributeOption;
        }

        #region UAT-3820
        public DataTable GetDataForReceivedFromStudentServiceFormStatus(String TenantIDs, Int32 serviceFormStatusLimit)
        {
            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[usp_GetNotificationsForStudentServiceFormStatus]", con);
                command.Parameters.AddWithValue("@TenantIDs", TenantIDs);
                command.Parameters.AddWithValue("@serviceFormStatusLimit", serviceFormStatusLimit);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > AppConsts.NONE)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }
        #endregion

        #region Mobile API
        List<Entity.ClientEntity.BkgPackageSvcGroup> IBackgroundProcessOrderRepository.GetBkgSvcGroupByBkgPkgId(Int32 bkgPackageId)
        {
            return _ClientDBContext.BkgPackageSvcGroups.Where(cond => !cond.BPSG_IsDeleted && cond.BPSG_BackgroundPackageID == bkgPackageId
                                    && cond.BkgSvcGroup != null && !cond.BkgSvcGroup.BSG_IsDeleted).ToList();
        }
        #endregion

        #region UAT-3745

        List<SystemDocBkgSvcMapping> IBackgroundProcessOrderRepository.GetApplicantDocsMappedWithSvc(Int32 orderId)
        {
            List<SystemDocBkgSvcMapping> lstAdditionalDocs = new List<SystemDocBkgSvcMapping>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@OrderID", orderId.ToString()),
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetApplicantDocsMappedWithSvc", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SystemDocBkgSvcMapping systemDocBkgSvcMapping = new SystemDocBkgSvcMapping();
                            systemDocBkgSvcMapping.BkgServiceID = dr["BkgServiceID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BkgServiceID"]);
                            systemDocBkgSvcMapping.BkgServiceName = dr["BkgServiceName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BkgServiceName"]);
                            systemDocBkgSvcMapping.ApplicantDocumentID = dr["ApplicantDocumentID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ApplicantDocumentID"]);
                            systemDocBkgSvcMapping.FileName = dr["FileName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FileName"]);
                            systemDocBkgSvcMapping.Description = dr["Description"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Description"]);

                            lstAdditionalDocs.Add(systemDocBkgSvcMapping);
                        }
                    }
                }
                return lstAdditionalDocs;
            }
        }
        #endregion

        #region UAT-4004

        List<VendorProfileSvcLineItemContract> IBackgroundProcessOrderRepository.GetLineItemsDataforOrderID(Int32 orderId)
        {
            List<VendorProfileSvcLineItemContract> lstVendorProfileSvcLineItemContract = new List<VendorProfileSvcLineItemContract>();

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@OrderID",orderId),
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetLineItemsDataforOrderID", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            VendorProfileSvcLineItemContract vendorProfileSvcLineItem = new VendorProfileSvcLineItemContract();

                            vendorProfileSvcLineItem.OrderID = dr["OrderID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrderID"]);
                            vendorProfileSvcLineItem.OrderNumber = dr["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(dr["OrderNumber"]);
                            vendorProfileSvcLineItem.BkgOrderID = dr["BkgOrderID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BkgOrderID"]);
                            vendorProfileSvcLineItem.BkgOrderPackageID = dr["BkgOrderPackageID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BkgOrderPackageID"]);
                            vendorProfileSvcLineItem.BackgroundPackageID = dr["BackgroundPackageID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BackgroundPackageID"]);
                            vendorProfileSvcLineItem.BackgroundPackageName = dr["BackgroundPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BackgroundPackageName"]);
                            vendorProfileSvcLineItem.ServiceGroupID = dr["ServiceGroupID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ServiceGroupID"]);
                            vendorProfileSvcLineItem.ServiceGroupName = dr["ServiceGroupName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ServiceGroupName"]);
                            vendorProfileSvcLineItem.ServiceID = dr["ServiceID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ServiceID"]);
                            vendorProfileSvcLineItem.ServiceName = dr["ServiceName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ServiceName"]);
                            vendorProfileSvcLineItem.OrderPkgSvcGroupID = dr["OrderPkgSvcGroupID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrderPkgSvcGroupID"]);
                            vendorProfileSvcLineItem.SvcLineItemStatusID = dr["SvcLineItemStatusID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SvcLineItemStatusID"]);
                            vendorProfileSvcLineItem.SvcLineItemStatus = dr["SvcLineItemStatus"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SvcLineItemStatus"]);
                            vendorProfileSvcLineItem.OrderPkgSvcID = dr["OrderPkgSvcID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrderPkgSvcID"]);
                            vendorProfileSvcLineItem.PackageSvcLineItemID = dr["PackageSvcLineItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["PackageSvcLineItemID"]);
                            vendorProfileSvcLineItem.ExtVendorBkgOrderDetailID = dr["ExtVendorBkgOrderDetailID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ExtVendorBkgOrderDetailID"]);
                            vendorProfileSvcLineItem.ExtVendorBkgOrderLineItemDetailID = dr["ExtVendorBkgOrderLineItemDetailID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ExtVendorBkgOrderLineItemDetailID"]);
                            vendorProfileSvcLineItem.ExtVendorID = dr["ExtVendorID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ExtVendorID"]);
                            vendorProfileSvcLineItem.VendorProfileID = dr["VendorProfileID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["VendorProfileID"]);
                            vendorProfileSvcLineItem.VendorLineItemOrderID = dr["VendorLineItemOrderID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["VendorLineItemOrderID"]);

                            lstVendorProfileSvcLineItemContract.Add(vendorProfileSvcLineItem);
                        }
                    }
                }
            }
            return lstVendorProfileSvcLineItemContract;

        }

        Boolean IBackgroundProcessOrderRepository.SaveUpdateSvcLineItemMapping(Int32 currentLoggedInUserId, VendorProfileSvcLineItemContract vendorProfileSvcLineItemData)
        {
            Boolean _result = false;

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@OrderID",vendorProfileSvcLineItemData.OrderID),
                            new SqlParameter("@BackgroundPackageID",vendorProfileSvcLineItemData.BackgroundPackageID),
                            new SqlParameter("@BkgServiceID",vendorProfileSvcLineItemData.ServiceID),
                            new SqlParameter("@BkgServiceGroupID",vendorProfileSvcLineItemData.ServiceGroupID),
                            new SqlParameter("@ExtVendorID",vendorProfileSvcLineItemData.ExtVendorID),
                            new SqlParameter("@LineItemStatusID",vendorProfileSvcLineItemData.SvcLineItemStatusID),
                            new SqlParameter("@VendorProfileID",vendorProfileSvcLineItemData.VendorProfileID),
                            new SqlParameter("@VendorOrderID",vendorProfileSvcLineItemData.VendorLineItemOrderID),
                            new SqlParameter("@IsLineItemUpdate",vendorProfileSvcLineItemData.IsLinkProfile),
                            new SqlParameter("@CurrentLoggedInUserID",currentLoggedInUserId),
                            new SqlParameter("@PkgSvcLineItemID",vendorProfileSvcLineItemData.PackageSvcLineItemID),
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_SaveUpdateSvcLineItemMapping", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            _result = dr["Result"] == DBNull.Value ? false : Convert.ToBoolean(dr["Result"]);
                        }
                    }
                }
            }
            return _result;
        }


        List<VendorProfileSvcLineItemContract> IBackgroundProcessOrderRepository.GetSvcLineItemsCreated(Int32 orderId)
        {
            List<VendorProfileSvcLineItemContract> lstVendorProfileSvcLineItemContract = new List<VendorProfileSvcLineItemContract>();

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@OrderID",orderId),
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetSvcLineItemsMapped", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            VendorProfileSvcLineItemContract vendorProfileSvcLineItem = new VendorProfileSvcLineItemContract();

                            vendorProfileSvcLineItem.OrderID = dr["OrderID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrderID"]);
                            //vendorProfileSvcLineItem.OrderNumber = dr["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(dr["OrderNumber"]);
                            vendorProfileSvcLineItem.BkgOrderID = dr["BkgOrderID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BkgOrderID"]);
                            //vendorProfileSvcLineItem.BkgOrderPackageID = dr["BkgOrderPackageID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BkgOrderPackageID"]);
                            vendorProfileSvcLineItem.BackgroundPackageID = dr["BackgroundPackageID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BackgroundPackageID"]);
                            vendorProfileSvcLineItem.BackgroundPackageName = dr["BackgroundPackage"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BackgroundPackage"]);
                            vendorProfileSvcLineItem.ServiceGroupID = dr["BkgSvcGroupID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BkgSvcGroupID"]);
                            vendorProfileSvcLineItem.ServiceGroupName = dr["BkgSvcGroup"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BkgSvcGroup"]);
                            vendorProfileSvcLineItem.ServiceID = dr["BackgroundServiceID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BackgroundServiceID"]);
                            vendorProfileSvcLineItem.ServiceName = dr["BackgroundService"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BackgroundService"]);
                            vendorProfileSvcLineItem.OrderPkgSvcGroupID = dr["BkgOrderPackageSvcGroupID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BkgOrderPackageSvcGroupID"]);
                            vendorProfileSvcLineItem.SvcLineItemStatusID = dr["SvcLineItemStatusID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SvcLineItemStatusID"]);
                            vendorProfileSvcLineItem.SvcLineItemStatus = dr["SvcLineItemStatus"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SvcLineItemStatus"]);
                            vendorProfileSvcLineItem.OrderPkgSvcID = dr["BkgOrderPackageSvcID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BkgOrderPackageSvcID"]);

                            vendorProfileSvcLineItem.PackageSvcLineItemID = dr["BkgOrderPackageSvcLineItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BkgOrderPackageSvcLineItemID"]);
                            vendorProfileSvcLineItem.ExtVendorBkgOrderDetailID = dr["ExternalVendorBkgOrderDetailsID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ExternalVendorBkgOrderDetailsID"]);
                            vendorProfileSvcLineItem.ExtVendorBkgOrderLineItemDetailID = dr["ExtVendorBkgOrderLineItemDetailsID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ExtVendorBkgOrderLineItemDetailsID"]);
                            //vendorProfileSvcLineItem.ExtVendorID = dr["ExtVendorID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ExtVendorID"]);
                            vendorProfileSvcLineItem.VendorProfileID = dr["VendorProfileID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["VendorProfileID"]);
                            vendorProfileSvcLineItem.VendorLineItemOrderID = dr["VendorLineItemOrderID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["VendorLineItemOrderID"]);

                            lstVendorProfileSvcLineItemContract.Add(vendorProfileSvcLineItem);
                        }
                    }
                }
            }
            return lstVendorProfileSvcLineItemContract;
        }

        #endregion

        #region UAT-4162:- Apply retry logic while pulling DS Registration details from ClearStar

        List<VendorProfileSvcLineItemContract> IBackgroundProcessOrderRepository.GetDSOrderToGetCSData(Int32 chunkSize, Int32 maxRetryCount, Int32 retryTimeLag)
        {
            List<VendorProfileSvcLineItemContract> lstDSOrderToBeProcessed = new List<VendorProfileSvcLineItemContract>();

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ChunkSize",chunkSize),
                             new SqlParameter("@MaxRetryCount",maxRetryCount),
                             new SqlParameter("@RetryTimeLag",retryTimeLag)
                        };

                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetNonDataPulledOrders", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            VendorProfileSvcLineItemContract dSOrderToBeProcessed = new VendorProfileSvcLineItemContract();

                            dSOrderToBeProcessed.OrderID = dr["OrderID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrderID"]);
                            dSOrderToBeProcessed.BkgOrderID = dr["BkgOrderID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BkgOrderID"]);
                            dSOrderToBeProcessed.ExtVendorID = dr["VendorID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["VendorID"]);
                            dSOrderToBeProcessed.BkgHierarchyMappingID = dr["BkgHierarchyMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BkgHierarchyMappingID"]);
                            dSOrderToBeProcessed.OrganizationUserID = dr["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrganizationUserID"]);
                            dSOrderToBeProcessed.OrganizationUserProfileID = dr["OrganizationUserProfileID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrganizationUserProfileID"]);
                            dSOrderToBeProcessed.ApplicantName = dr["ApplicantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ApplicantName"]);
                            dSOrderToBeProcessed.PrimaryEmailAddress = dr["PrimaryEmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PrimaryEmailAddress"]);
                            dSOrderToBeProcessed.SelectedNodeID = dr["SelectedNodeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SelectedNodeID"]);
                            dSOrderToBeProcessed.PackageSvcLineItemID = dr["PackageSvcLineItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["PackageSvcLineItemID"]);

                            lstDSOrderToBeProcessed.Add(dSOrderToBeProcessed);
                        }
                    }
                }
            }
            return lstDSOrderToBeProcessed;
        }

        Boolean IBackgroundProcessOrderRepository.UpdateRetryCountForDsOrders(Dictionary<Int32, Int32> dicbkgSvcLineItems, Int32 loggedInUserId)
        {
            if (!dicbkgSvcLineItems.IsNullOrEmpty())
            {
                DateTime currentTimeStamp = DateTime.Now;
                foreach (var item in dicbkgSvcLineItems)
                {
                    BkgOrderPackageSvcLineItem svcLineItem = _ClientDBContext.BkgOrderPackageSvcLineItems.Where(cond => cond.PSLI_IsDeleted != true && cond.PSLI_ID == item.Key).FirstOrDefault();
                    if (!svcLineItem.IsNullOrEmpty())
                    {
                        svcLineItem.PSLI_DataPulledStatusTypeID = item.Value;
                        svcLineItem.PSLI_DataPullRetryCount = svcLineItem.PSLI_DataPullRetryCount.IsNullOrEmpty() ? AppConsts.ONE : svcLineItem.PSLI_DataPullRetryCount + AppConsts.ONE;
                        svcLineItem.PSLI_DataPullRetryLastDate = currentTimeStamp;
                        svcLineItem.PSLI_ModifiedByID = loggedInUserId;
                        svcLineItem.PSLI_ModifiedOn = currentTimeStamp;
                    }
                }
            }

            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        #endregion

        public bool SaveCustomFormApplicantData(string xmlStringData, int applicantOrganisationId, int currentLoggedInUserId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.InsertCustomFormApplicantData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Data", xmlStringData.ToString());
                command.Parameters.AddWithValue("@CurrentLoggedInUserId", currentLoggedInUserId);
                command.Parameters.AddWithValue("@OrganisationUserID", applicantOrganisationId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
            }
            return true;
        }

        DataTable IBackgroundProcessOrderRepository.GetBkgOrderServiceFormNotificationDataForAdminEntry(Int32 orderId, String serviceIds)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetBkgOrderNotificationData_AEP", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@orderId", orderId);
                command.Parameters.AddWithValue("@serviceIds", serviceIds);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }
        #region UAT-4613
        DataTable IBackgroundProcessOrderRepository.GetDataForInProcessAgencyFromApplicantServiceFormStatus(int serviceFormStatusLimit)
        {
            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[ams].[usp_GetBkgSvcFormStatusInProcAgency]", con);
                command.Parameters.AddWithValue("@serviceFormStatusLimit", serviceFormStatusLimit);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > AppConsts.NONE)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }
        #endregion
        #region UAT-5114
        //   bool CheckIfOrderIsAdminEntryOrder(Int32 bkgOrderId)
        Boolean IBackgroundProcessOrderRepository.CheckIfOrderIsAdminEntryOrder(Int32 bkgOrderId)
        {
           var data = _ClientDBContext.BkgAdminEntryOrderDetails.Where(cond => cond.BAEOD_BkgOrderID == bkgOrderId && !cond.BAEOD_IsDeleted).FirstOrDefault();
            if (!data.IsNullOrEmpty())
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}