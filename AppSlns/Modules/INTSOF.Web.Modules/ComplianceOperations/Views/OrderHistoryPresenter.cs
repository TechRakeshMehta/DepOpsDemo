#region Namespaces

#region SystemDefined

using System;
using INTSOF.SharedObjects;
using System.Collections.Generic;
using System.Linq;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using System.Data;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion
namespace CoreWeb.ComplianceOperations.Views
{
    public class OrderHistoryPresenter : Presenter<IOrderHistoryView>
    {
        #region Variables

        #region Private Variables
        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #endregion

        #region Public Properties
        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetOrderDetailList()
        {
            if (SecurityManager.DefaultTenantID != View.CurrentUserTenantId)
            {
                if (View.IsScreeningOnly)
                {
                    View.ListOrderDetail = ComplianceDataManager.GetOrderDetailList(View.CurrentUserTenantId, View.CurrentUserId, new List<String>())
                        .Where(x => !x.OrderPackageTypeCode.Contains("AAAA") && !x.OrderPackageTypeCode.Contains("AAAE") && !x.OrderPackageTypeCode.Contains("AAAF")).ToList();

                    //UAT-1033 Add link to download E Drug authorization form (Electronic Service Form) to screening tab
                    if (View.ListOrderDetail.IsNotNull() && View.ListOrderDetail.Count > AppConsts.NONE)
                    {
                        //Get EDS Status.
                        List<Int32> orderIds = View.ListOrderDetail.Select(x => x.OrderId).Distinct().ToList();
                        String commaDelemittedOrderIDs = String.Join(",", orderIds);
                        View.EDSExistStatus = ComplianceDataManager.GetEDSStatusForOrders(View.CurrentUserTenantId, commaDelemittedOrderIDs);
                        View.LstReciptDocumentStatus = ComplianceDataManager.GetReciptDocumentStatus(View.CurrentUserTenantId, orderIds);

                        //Get Electonic service form for OrderIDs
                        View.lstServiceForm = BackgroundProcessOrderManager.GetAutomaticServiceFormForListOfOrders(View.CurrentUserTenantId, commaDelemittedOrderIDs);
                    }
                }
                else
                {
                    View.ListOrderDetail = ComplianceDataManager.GetOrderDetailList(View.CurrentUserTenantId, View.CurrentUserId, new List<String>()).ToList();
                    if (View.ListOrderDetail.IsNotNull() && View.ListOrderDetail.Count > AppConsts.NONE)
                    {
                        List<Int32> orderIds = View.ListOrderDetail.Select(x => x.OrderId).Distinct().ToList();
                        View.LstReciptDocumentStatus = ComplianceDataManager.GetReciptDocumentStatus(View.CurrentUserTenantId, orderIds);
                    }
                }
            }
        }

        public AppointmentSlotContract GetBkgOrderWithAppointmentData(int OrderId)
        {
            AppointmentSlotContract appointment = new AppointmentSlotContract();
            //AppointSlotContract = new AppointmentSlotContract();
            //  List<Int32> orderIds = View.ListOrderDetail.Select(x => x.OrderId).Distinct().ToList();
            if (View.CurrentUserTenantId > AppConsts.NONE && OrderId > AppConsts.NONE && View.CurrentUserId > AppConsts.NONE)
            {
                return FingerPrintDataManager.GetBkgOrderWithAppointmentData(View.CurrentUserTenantId, OrderId, View.CurrentUserId);
            }
            else
            {
                return appointment;
            }
        }

        public void IsBkgOrderWithAppointment(int OrderId)
        {
            if (View.CurrentUserTenantId > AppConsts.NONE && OrderId > AppConsts.NONE && View.CurrentUserId > AppConsts.NONE)
            {
                View.IsBkgOrderWithAppointment = FingerPrintSetUpManager.IsBkgOrderWithAppointment(View.CurrentUserTenantId, OrderId, View.CurrentUserId);
            }
        }

        public string GetLocTenMaxAllowedDays()
        {
            return SecurityManager.GetLocTenMaxAllowedDays();
        }


        /// <summary>
        /// Returns the list of the Orders, for which Rush Order can be placed
        /// </summary>
        /// <returns></returns>
        public List<Int32> GerPossibleRushOrderIds(List<vwOrderDetail> lstOrderDetails)
        {
            if (!lstOrderDetails.IsNullOrEmpty())
                return ComplianceDataManager.GetPossibleRushOrderIds(lstOrderDetails, View.CurrentUserTenantId);

            return new List<Int32>();
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Send Order Cancel Request.
        /// </summary>
        /// <returns></returns>
        public Boolean SendOrderCancelRequest(Int32 orderID)
        {
            Order order = new Order();
            order.OrderID = orderID;
            //order.ModifiedByID = View.CurrentUserId;
            order.ModifiedByID = View.OrgUsrID;
            order.ModifiedOn = DateTime.Now;
            return ComplianceDataManager.UpdateOrderByOrderID(View.CurrentUserTenantId, order, ApplicantOrderStatus.Cancellation_Requested.GetStringValue());
        }

        /// <summary>
        /// Method to Send Order Approval Mail and Message.
        /// </summary>
        /// <returns></returns>
        public void SendOrderCancellationNotification(Int32 orderID)
        {
            OrderPaymentDetail orderPaymentDetail = ComplianceDataManager.GetOrderDetailById(View.CurrentUserTenantId, orderID);
            if (orderPaymentDetail != null)
            {
                CommunicationManager.SendOrderCancellationMail(orderPaymentDetail, View.CurrentUserTenantId);
                CommunicationManager.SendOrderCancellationMessage(orderPaymentDetail, View.CurrentUserTenantId);
            }
        }

        #region UAT-2021: Auto Approve cancellation requests.

        /// <summary>
        /// Approve cancellations requests
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Boolean ApproveCancellations(Int32 orderID)
        {
            Boolean isCancelledByApplicant = true;
            var orderStatusCode = ApplicantOrderStatus.Cancelled.GetStringValue();
            return ComplianceDataManager.CancelPlacedOrder(View.CurrentUserTenantId, orderID, orderStatusCode, View.OrgUsrID, isCancelledByApplicant, isInstantCancellation: true);
        }

        /// <summary>
        /// Method to Send Order Cancellation Approval Mail and Message.
        /// </summary>
        /// <param name="orderID"></param>
        public void SendOrderCancellationApprovalNotification(Int32 orderID)
        {
            OrderPaymentDetail orderPaymentDetail = ComplianceDataManager.GetOrderDetailById(View.CurrentUserTenantId, orderID);
            if (orderPaymentDetail != null)
            {
                CommunicationManager.SendOrderCancellationApprovedMail(orderPaymentDetail, View.CurrentUserTenantId);
                CommunicationManager.SendOrderCancellationApprovedMessage(orderPaymentDetail, View.CurrentUserTenantId);
            }
        }

        #endregion

        public Boolean IsPackageSubscribedForOrderIds(List<Int32> orderIds)
        {
            return ComplianceDataManager.IsPackageSubscribedForOrderIds(View.CurrentUserTenantId, orderIds);
        }

        /// <summary>
        /// Method to set show rush order property from Client Settings.
        /// </summary>
        public void ShowRushOrder()
        {
            int rushOrderID = ComplianceDataManager.GetSettings(View.CurrentUserTenantId).WhereSelect(cond => cond.Code == Setting.Enable_Rush_Order.GetStringValue(), col => col.SettingID).FirstOrDefault();
            string enableRushOrderValue = ComplianceDataManager.GetClientSetting(View.CurrentUserTenantId).WhereSelect(t => t.CS_SettingID == rushOrderID, col => col.CS_SettingValue).FirstOrDefault();
            View.ShowRushOrder = string.IsNullOrEmpty(enableRushOrderValue) ? false : ((enableRushOrderValue == "0") ? false : true);
        }

        public Boolean IsPackageChangeSubscription(Int32 orderID)
        {
            List<Order> orderList = ComplianceDataManager.GetChangeSubscriptionOrderList(View.CurrentUserTenantId, orderID);
            if (orderList.IsNotNull() && orderList.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public Boolean IsPackageRenewSubscription(Int32 orderID)
        {
            return ComplianceDataManager.GetRenewSubscriptionOrder(View.CurrentUserTenantId, orderID);
            //if (orderList.IsNotNull() && orderList.Count() > 0)
            //{
            //    return true;
            //}
            //return false;
        }

        #region [UAT-977:Additional work towards archive ability]
        public Boolean IsPackageRepurchasedSubscription(Int32 orderID)
        {
            Order repurchasedOrder = null;
            repurchasedOrder = ComplianceDataManager.GetRepuchasedOrderByPreviousOrderID(View.CurrentUserTenantId, orderID, OrderRequestType.NewOrder.GetStringValue());
            if (!repurchasedOrder.IsNullOrEmpty())
                return true;
            return false;
        }
        #endregion

        #region UAT-1683
        /// <summary>
        /// Method return true if already an active request is send for the same order subscription id.UAT-1683
        /// </summary>
        /// <param name="BkgOrderID">BkgOrderID</param>
        /// <returns></returns>
        public Boolean IsActiveUnArchiveRequestForBkgOrderSubscriptionId(Int32 BkgOrderID)
        {
            return ComplianceDataManager.IsActiveUnArchiveRequestForBkgOrderSubscriptionId(View.CurrentUserTenantId, BkgOrderID, ComplianceSubscriptionArchiveChangeType.UN_ARCHIVE_REQUESTED.GetStringValue());
        }
        public Boolean SaveBkgOrderUnArchiveRequest(Int32 BkgOrderId)
        {
            BkgOrder orderDetail = ComplianceDataManager.GetBkgOrderDetailByID(View.CurrentUserTenantId, BkgOrderId);
            Int16 unArchiveChangeTypeId = ComplianceDataManager.GetComplianceSubsArchiveChangeTypeIdByCode(View.CurrentUserTenantId, ComplianceSubscriptionArchiveChangeType.UN_ARCHIVE_REQUESTED.GetStringValue());
            BkgOrderArchiveHistory objectToSaveUnArchiveRequest = new BkgOrderArchiveHistory();
            objectToSaveUnArchiveRequest.BOAH_IsActive = true;
            objectToSaveUnArchiveRequest.BOAH_IsDeleted = false;
            objectToSaveUnArchiveRequest.BOAH_BkgOrderID = orderDetail.BOR_ID;
            objectToSaveUnArchiveRequest.BOAH_SubscriptionChangeDetail = null;
            objectToSaveUnArchiveRequest.BOAH_ChangeTypeID = unArchiveChangeTypeId;
            objectToSaveUnArchiveRequest.BOAH_CreatedBy = View.OrgUsrID;
            objectToSaveUnArchiveRequest.BOAH_CreatedOn = DateTime.Now;
            return ComplianceDataManager.SaveBkgOrderArchiveHistoryData(View.CurrentUserTenantId, objectToSaveUnArchiveRequest, orderDetail.BOR_ID);
        }
        #endregion
        #endregion

        #region Private Methods



        #endregion

        #endregion

        #region UAT-916 :WB: As an application admin, I should be able to define payment options at the package level in addition to the node level
        public Boolean CheckIsInvoiceOnlyOrderPayment(Int32 orderID)
        {
            return ComplianceDataManager.CheckIsInvoiceOnlyOrderPayment(View.CurrentUserTenantId, orderID);
        }

        public List<OrderPaymentDetail> GetAllPaymentDetailsOfOrderByOrderID(Int32 orderID)
        {
            return ComplianceDataManager.GetAllPaymentDetailsOfOrderByOrderID(View.CurrentUserTenantId, orderID);
        }
        #endregion

        /// <summary>
        /// UAT-1033 Add link to download E Drug authorization form (Electronic Service Form) to screening tab
        /// Return the applicant document.
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        /// 
        public ApplicantDocument GetEDSDocument(Int32 OrderId)
        {
            String documentTypeCode = DocumentType.EDS_AuthorizationForm.GetStringValue();
            String recordTypeCode = RecordType.BackgroundProfile.GetStringValue();
            if (OrderId > 0 && View.CurrentUserTenantId > 0)
            {
                return BackgroundProcessOrderManager.GetApplicantDocumentForEds(View.CurrentUserTenantId, OrderId, documentTypeCode, recordTypeCode);
            }
            return null;
        }
        public List<String> GetServiceStatus(Int32 orderID, Int32 orgUserId)
        {
            return FingerPrintDataManager.GetServiceStatus(View.CurrentUserTenantId, orderID, orgUserId);
        }

        #region UAT-1648:As an applicant, I should be able to complete payment for an order that is in "sent for online payment"

        /// <summary>
        /// Get Order  by order id.
        /// </summary>
        /// <param name="orderId">orderId</param>
        /// <returns></returns>
        public Order GetOrderByOrderId(Int32 orderId)
        {
            return ComplianceDataManager.GetOrderById(View.CurrentUserTenantId, orderId);
        }

        /// <summary>
        /// Get order payment detail of order those status are "SentForOnlinePayment"
        /// </summary>
        /// <param name="currentOrder">currentOrder</param>
        /// <returns>List of OrderPaymentDetail</returns>
        public List<OrderPaymentDetail> GetOPDOfSentForOnlinePaymentStatus(Order currentOrder)
        {
            List<OrderPaymentDetail> sentForOnlinePaymentDetail = new List<OrderPaymentDetail>();
            if (!currentOrder.IsNullOrEmpty() && !currentOrder.OrderPaymentDetails.IsNullOrEmpty())
            {
                String sentForOnlinePaymentCode = ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue();
                String ordrPkgTypeComplianceRushOrderCode = OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue();
                var sentForOnlinePaymentDetailTemp = currentOrder.OrderPaymentDetails.Where(cnd => cnd.lkpOrderStatu != null
                                                                                     && cnd.lkpOrderStatu.Code == sentForOnlinePaymentCode && !cnd.OPD_IsDeleted).ToList();

                foreach (var opd in sentForOnlinePaymentDetailTemp)
                {
                    if (!opd.OrderPkgPaymentDetails.Any(OPPD => !OPPD.OPPD_IsDeleted && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeComplianceRushOrderCode))
                    {
                        sentForOnlinePaymentDetail.Add(opd);
                    }
                }
            }

            return sentForOnlinePaymentDetail;
        }

        /// <summary>
        /// Check is Current OPD Contains Compliance Package.
        /// </summary>
        /// <param name="sentForOnlinePaymentDetailList">sentForOnlinePaymentDetailList</param>
        /// <returns>Boolean</returns>
        public Boolean IsCompliancePackageIncluded(List<OrderPaymentDetail> sentForOnlinePaymentDetailList)
        {
            String ordrPkgTypeComplianceCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();

            String compliancePackageTypeCode = PartialOrderCancellationType.COMPLIANCE_PACKAGE.GetStringValue();
            String complianceBkgPackageTypeCode = PartialOrderCancellationType.COMPLIANCE_BACKGROUND_PACKAGES.GetStringValue();

            Boolean isCompliancePackageIncluded = false;
            if (!sentForOnlinePaymentDetailList.IsNullOrEmpty())
            {
                foreach (var opd in sentForOnlinePaymentDetailList)
                {
                    isCompliancePackageIncluded = opd.OrderPkgPaymentDetails.Any(OPPD => OPPD.OPPD_BkgOrderPackageID == null && !OPPD.OPPD_IsDeleted
                                                                      && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeComplianceCode);
                    if (isCompliancePackageIncluded)
                    {
                        //Changes related to Bug ID: 15039
                        if (opd.Order.PartialOrderCancellationTypeID.IsNullOrEmpty())
                        {
                            break;
                        }
                        else if (opd.Order.lkpPartialOrderCancellationType.Code == compliancePackageTypeCode || opd.Order.lkpPartialOrderCancellationType.Code == complianceBkgPackageTypeCode)
                        {
                            isCompliancePackageIncluded = false;
                            break;
                        }
                    }
                }
            }
            return isCompliancePackageIncluded;
        }

        /// <summary>
        /// Check that curent OPD contains BackgroundPackage or not.
        /// </summary>
        /// <param name="sentForOnlinePaymentDetailList">sentForOnlinePaymentDetailList</param>
        /// <returns>Boolean</returns>
        public Boolean IsBackgroundPackageIncluded(List<OrderPaymentDetail> sentForOnlinePaymentDetailList)
        {
            String ordrPkgTypeBkgCode = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();
            Boolean isBackgroundPackageIncluded = false;
            if (!sentForOnlinePaymentDetailList.IsNullOrEmpty())
            {
                foreach (var opd in sentForOnlinePaymentDetailList)
                {
                    isBackgroundPackageIncluded = opd.OrderPkgPaymentDetails.Any(OPPD => OPPD.OPPD_BkgOrderPackageID != null && !OPPD.OPPD_IsDeleted
                                                                          && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeBkgCode);
                    if (isBackgroundPackageIncluded)
                    {
                        break;
                    }
                }
            }
            return isBackgroundPackageIncluded;
        }

        /// <summary>
        /// Get the payment options
        /// </summary>
        /// <param name="dpmId">Will be used, in case, when NO Compliance package was selected for the purchase.</param>
        public Boolean IfInvoiceIsOnlyPaymentOptions(Int32 dpmId)
        {
            List<lkpPaymentOption> paymentOptions = ComplianceDataManager.GetPaymentOptionsByDPMId(View.CurrentUserTenantId, dpmId);
            if (paymentOptions.Count == 1)
            {
                return paymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue() || t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
            }
            else if (paymentOptions.Count == 2)
            {
                return (paymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()) && paymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()));
            }
            return false;
        }

        public bool IsUserGroupCustomAttributeExist(String useTypeCode, Int32 selectedDPMId)
        {
            return ComplianceDataManager.GetCustomAttributesByNodes(useTypeCode, selectedDPMId, View.CurrentUserId, View.CurrentUserTenantId)
                .Any(x => x.CADataTypeCode.ToLower().Trim() == CustomAttributeDatatype.User_Group.GetStringValue().ToLower().Trim());
        }

        public vw_AddressLookUp GetAddressLookupByHandlerId(String addressHandleId)
        {
            Guid addHandleId = new Guid(addressHandleId);
            return ComplianceDataManager.GetAddressLookupByHandlerId(addHandleId, View.CurrentUserTenantId);
        }

        /// <summary>
        /// Get Bkg Order Package Data 
        /// </summary>
        /// <param name="sentForOnlinePaymentDetailList">sentForOnlinePaymentDetailList</param>
        /// <returns></returns>
        public List<BkgOrderPackage> GetBkgOrderPackageDetail(List<OrderPaymentDetail> sentForOnlinePaymentDetailList)
        {
            String ordrPkgTypeBkgCode = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();
            List<Int32> lstBopIDs = new List<Int32>();
            List<BkgOrderPackage> lstBkgOrderPackageList = new List<BkgOrderPackage>();
            foreach (var opd in sentForOnlinePaymentDetailList)
            {
                var bopIds = opd.OrderPkgPaymentDetails.Where(OPPD => OPPD.OPPD_BkgOrderPackageID != null && !OPPD.OPPD_IsDeleted
                                                               && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeBkgCode).Select(slct => slct.OPPD_BkgOrderPackageID.Value)
                                                               .ToList();
                lstBopIDs.AddRange(bopIds);
            }
            View.BopIds = lstBopIDs;
            lstBkgOrderPackageList = BackgroundProcessOrderManager.GetBackgroundOrderPackageListById(View.CurrentUserTenantId, lstBopIDs);
            return lstBkgOrderPackageList;

        }
        /// <summary>
        /// Get Bkg Order Service Details xml
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public string GetBkgOrderServiceDetails(int orderID)
        {
            return BackgroundProcessOrderManager.GetBkgOrderServiceDetails(View.CurrentUserTenantId, orderID);
        }

        /// <summary>
        /// Method to Get Decrypted SSN
        /// </summary>
        /// <param name="encryptedSSN"></param>
        public String GetDecryptedSSN(Int32 orgUserID)
        {
            return ComplianceSetupManager.GetFormattedString(orgUserID, true, View.CurrentUserTenantId);
        }

        /// <summary>
        /// Get Attribute Custom Form Data.
        /// </summary>
        /// <param name="masterOrderId">masterOrderId</param>
        /// <param name="isIncludeMvrData">isIncludeMvrData</param>
        public void GetAttributesCustomFormIdOrderId(Int32 masterOrderId, Boolean isIncludeMvrData)
        {
            String bopIds = String.Join(",", View.BopIds);

            BkgOrderDetailCustomFormDataContract bkgOrderDetailCustomFormDataContract = BackgroundProcessOrderManager.GetBkgORDCustomFormAttrDataForCompletingOrder
                                                                                        (View.CurrentUserTenantId, masterOrderId, bopIds, isIncludeMvrData);
            if (bkgOrderDetailCustomFormDataContract.IsNotNull())
            {
                //if (bkgOrderDetailCustomFormDataContract.lstCustomFormAttributes.IsNotNull())
                //    View.lstCustomFormAttributes = bkgOrderDetailCustomFormDataContract.lstCustomFormAttributes;
                //else
                //    View.lstCustomFormAttributes = new List<AttributesForCustomFormContract>();
                if (bkgOrderDetailCustomFormDataContract.lstDataForCustomForm.IsNotNull())
                    View.lstDataForCustomForm = bkgOrderDetailCustomFormDataContract.lstDataForCustomForm;
                else
                    View.lstDataForCustomForm = new List<BkgOrderDetailCustomFormUserData>();
            }

        }

        /// <summary>
        /// Get MVR Attribute Data 
        /// </summary>
        /// <param name="packageIds">packageIds</param>
        /// <returns></returns>
        public void GetAttributeFieldsOfSelectedPackages(String packageIds)
        {
            List<Entity.ClientEntity.AttributeFieldsOfSelectedPackages> lstAttributeFields = BackgroundProcessOrderManager.GetAttributeFieldsOfSelectedPackages(packageIds, View.CurrentUserTenantId);
            if (!lstAttributeFields.IsNullOrEmpty())
            {
                View.lstAttrMVRGrp = lstAttributeFields.Where(cond => (cond.BSA_Code.ToUpper().Equals("1ADA97AE-9100-4BE6-B829-C914B7FA8750")
                                                                        || cond.BSA_Code.ToUpper().Equals("515BEF57-9072-4D2A-A97A-0C248BB045F9"))
                                                                       && cond.AttributeGrpCode.ToUpper().Equals("CF76960D-2120-46FE-9E03-01C218F8A336")).ToList();
            }
            else
            {
                View.lstAttrMVRGrp = new List<AttributeFieldsOfSelectedPackages>();
            }
        }

        /// <summary>
        /// Method to check is Completing Process required for the order on the basis of "SentForOnlinePayment" status.
        /// </summary>
        /// <param name="orderOPDs">Order Payment Details</param>
        /// <returns>Boolean</returns>
        public Boolean IsOrderAvailableForCompletingProcess(List<OrderPaymentDetail> orderOPDs)
        {
            Boolean isOrderAvailableForCompletingProcess = false;
            Boolean isCompPkgAlreadyPurchased = false;
            if (!orderOPDs.IsNullOrEmpty())
            {
                OrderPaymentDetail ordPaymentDetail = orderOPDs.FirstOrDefault();
                if (!ordPaymentDetail.Order.lkpOrderPackageType.IsNullOrEmpty()
                    && (ordPaymentDetail.Order.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()
                       || ordPaymentDetail.Order.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue()))
                {
                    var compPkgSubscription = ComplianceDataManager.GetPackageSubscriptionByPackageID(ordPaymentDetail.Order.DeptProgramPackage.DPP_CompliancePackageID,
                                                                                                    View.CurrentUserId, View.CurrentUserTenantId);
                    if (!compPkgSubscription.IsNullOrEmpty())
                    {
                        String archiveStatusCode = ArchiveState.Active.GetStringValue();
                        if (compPkgSubscription.lkpArchiveState.AS_Code == archiveStatusCode && compPkgSubscription.ExpiryDate >= DateTime.Now)
                        {
                            isCompPkgAlreadyPurchased = true;
                        }
                    }
                }
                if (!ordPaymentDetail.Order.lkpOrderPackageType.IsNullOrEmpty()
                   && (ordPaymentDetail.Order.lkpOrderPackageType.OPT_Code == OrderPackageTypes.REQUIREMENT_ITEM_PAYMENT.GetStringValue()
                      || ordPaymentDetail.Order.lkpOrderPackageType.OPT_Code == OrderPackageTypes.TRACKING_ITEM_PAYMENT.GetStringValue()))
                {
                    INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract result = ComplianceDataManager.GetItempaymentDetailsByOrderId(ordPaymentDetail.Order.OrderID, View.CurrentUserTenantId);
                    if (result.IsNullOrEmpty() || result.orderID <= AppConsts.NONE)
                    {
                        isCompPkgAlreadyPurchased = true;
                    }
                }
                if (!isCompPkgAlreadyPurchased)
                {
                    String sentForOnlinePaymentCode = ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue();
                    String ordrPkgTypeComplianceRushOrderCode = OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue();
                    var sentForOnlinePaymentDetailTemp = orderOPDs.Where(cnd => cnd.lkpOrderStatu != null
                                                                                         && cnd.lkpOrderStatu.Code == sentForOnlinePaymentCode && !cnd.OPD_IsDeleted).ToList();
                    foreach (var opd in sentForOnlinePaymentDetailTemp)
                    {
                        if (!opd.OrderPkgPaymentDetails.Any(OPPD => !OPPD.OPPD_IsDeleted && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeComplianceRushOrderCode))
                        {
                            isOrderAvailableForCompletingProcess = true;
                            break;
                        }
                    }
                }
            }
            return isOrderAvailableForCompletingProcess;

        }
        #endregion

        public Boolean IsOrderAvailableForCompletingCompleteModifyshipping(List<OrderPaymentDetail> orderOPDs)
        {
            Boolean isOrderAvailableForCompletingModifyshipping = false;
            Boolean isCompPkgAlreadyPurchased = false;
            if (!orderOPDs.IsNullOrEmpty())
            {
                OrderPaymentDetail ordPaymentDetail = orderOPDs.FirstOrDefault();
                if (!ordPaymentDetail.Order.lkpOrderPackageType.IsNullOrEmpty()
                    && (ordPaymentDetail.Order.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()
                       || ordPaymentDetail.Order.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue()))
                {
                    var compPkgSubscription = ComplianceDataManager.GetPackageSubscriptionByPackageID(ordPaymentDetail.Order.DeptProgramPackage.DPP_CompliancePackageID,
                                                                                                    View.CurrentUserId, View.CurrentUserTenantId);
                    if (!compPkgSubscription.IsNullOrEmpty())
                    {
                        String archiveStatusCode = ArchiveState.Active.GetStringValue();
                        if (compPkgSubscription.lkpArchiveState.AS_Code == archiveStatusCode && compPkgSubscription.ExpiryDate >= DateTime.Now)
                        {
                            isCompPkgAlreadyPurchased = true;
                        }
                    }
                }
                if (!ordPaymentDetail.Order.lkpOrderPackageType.IsNullOrEmpty()
                   && (ordPaymentDetail.Order.lkpOrderPackageType.OPT_Code == OrderPackageTypes.REQUIREMENT_ITEM_PAYMENT.GetStringValue()
                      || ordPaymentDetail.Order.lkpOrderPackageType.OPT_Code == OrderPackageTypes.TRACKING_ITEM_PAYMENT.GetStringValue()))
                {
                    INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract result = ComplianceDataManager.GetItempaymentDetailsByOrderId(ordPaymentDetail.Order.OrderID, View.CurrentUserTenantId);
                    if (result.IsNullOrEmpty() || result.orderID <= AppConsts.NONE)
                    {
                        isCompPkgAlreadyPurchased = true;
                    }
                }
                if (!isCompPkgAlreadyPurchased)
                {
                    String modifyshippingsentForOnlinePaymentCode = ApplicantOrderStatus.Modify_Shipping_Send_For_Online_Payment.GetStringValue();
                    String ordrPkgTypeComplianceRushOrderCode = OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue();
                    var modifyshippingsentForOnlinePaymentDetailTemp = orderOPDs.Where(cnd => cnd.lkpOrderStatu != null
                                                                                         && cnd.lkpOrderStatu.Code == modifyshippingsentForOnlinePaymentCode && !cnd.OPD_IsDeleted).ToList();
                    foreach (var opd in modifyshippingsentForOnlinePaymentDetailTemp)
                    {
                        if (!opd.OrderPkgPaymentDetails.Any(OPPD => !OPPD.OPPD_IsDeleted && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeComplianceRushOrderCode))
                        {
                            isOrderAvailableForCompletingModifyshipping = true;
                            break;
                        }
                    }
                }
            }
            return isOrderAvailableForCompletingModifyshipping;

        }

        #region As an applicant, I should be able to modify shipping address for an order 

        /// <summary>
        /// Get order payment detail of order to modify shipping address.
        /// </summary>
        /// <param name="currentOrder">currentOrder</param>
        /// <returns>List of OrderPaymentDetail</returns>
        public List<OrderPaymentDetail> GetOrderPaymentDetails(Order currentOrder)
        {
            List<OrderPaymentDetail> PaymentDetail = new List<OrderPaymentDetail>();
            if (!currentOrder.IsNullOrEmpty() && !currentOrder.OrderPaymentDetails.IsNullOrEmpty())
            {
                String ordrPkgTypeComplianceRushOrderCode = OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue();
                //var sentForOnlinePaymentDetailTemp = currentOrder.OrderPaymentDetails.Where(cnd => cnd.lkpOrderStatu != null
                //                                                                     && cnd.lkpOrderStatu.Code == sentForOnlinePaymentCode && !cnd.OPD_IsDeleted).ToList();

                var PaymentDetailTemp = currentOrder.OrderPaymentDetails.Where(cnd => cnd.lkpOrderStatu != null
                                                                     && !cnd.OPD_IsDeleted).ToList();

                foreach (var opd in PaymentDetailTemp)
                {
                    if (!opd.OrderPkgPaymentDetails.Any(OPPD => !OPPD.OPPD_IsDeleted && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeComplianceRushOrderCode))
                    {
                        PaymentDetail.Add(opd);
                    }
                }
            }

            return PaymentDetail;
        }

        #endregion

        #region UAT-3083
        /// <summary>
        /// Get Subscription ID by Order Id for Item Payment Type
        /// </summary>
        /// <returns></returns>
        public INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract GetItempaymentDetailsByOrderId(Int32 OrderID)
        {
            return ComplianceDataManager.GetItempaymentDetailsByOrderId(OrderID, View.CurrentUserTenantId);
        }
        #endregion

        #region UAT-3521 || CBI || CABS

        public void IsLocationServiceTenant()
        {
            if (View.CurrentUserTenantId > AppConsts.NONE)
                View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(View.CurrentUserTenantId);
        }
        #endregion

        public string GetCABSAppointmentStatus(int OrderID)
        {
            return FingerPrintDataManager.GetOrderAppointmentStatus(View.CurrentUserTenantId, OrderID, View.CurrentUserId);
        }
        public string GetPackageNameForCompleteOrder(Int32 orderId, string serviceType, bool isIdRequired)
        {
            return FingerPrintDataManager.GetPackageNameForCompleteOrder(View.CurrentUserTenantId, orderId, serviceType,  isIdRequired);
        }
        public string GetShippingLineItemName(string serviceType)
        {
            return FingerPrintDataManager.GetShippingLineItemName(View.CurrentUserTenantId, serviceType);
        }
        /// <summary>
        /// Gets List of All Countries 
        /// </summary>
        public string GetCountryByCountryId(int CountryId)
        {
            var countryName = "";
            countryName = SecurityManager.GetCountryByCountryId(CountryId);
            return countryName;
        }


        public PreviousAddressContract GetAddressThroughAddressHandleID(string MailingAddressHandleId)
        {
            return FingerPrintDataManager.GetAddressThroughAddressHandleID(View.CurrentUserTenantId,  MailingAddressHandleId);
        }
        public List<OrderDetailContract> OrderSerivceDetail(Int32 orderId)
        {
            return FingerPrintDataManager.OrderSerivceDetail(View.CurrentUserTenantId, orderId, View.CurrentUserTenantId);
        }
    }
}




