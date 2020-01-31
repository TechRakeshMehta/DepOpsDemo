using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using INTSOF.UI.Contract.FingerPrintSetup;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class ManageOrderFulFillmentQueueDetailPresenter : Presenter<IManageOrderFulFillmentQueueDetailView>
    {
        public void GetTenantIdByOrganizationUserID()
        {
            View.TenantId = SecurityManager.GetTenantIdByOrganizationUserID(View.CurrentLoggedInUserID);
        }



        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }

        public Boolean SaveUpdateApplicantAppointment(AppointmentOrderScheduleContract AppOrdSchdContract, AppointmentSlotContract scheduleAppointmentContract)
        {
            return FingerPrintSetUpManager.SaveUpdateApplicantAppointment(AppOrdSchdContract, scheduleAppointmentContract, View.CurrentLoggedInUserID);
        }

       

        

        public void GetOrderPaymentDetailList()
        {
            View.OrderPaymentDetailList = ComplianceDataManager.GetAllPaymentDetailsOfOrderByOrderID(View.SelectedTenantId, View.AppointmentDetailContract.OrderId);
        }

        public Boolean ApprovePayment()
        {
            //return FingerPrintDataManager.ApprovePayment(View.SelectedTenantId, View.AppointmentDetailContract.OrderId, View.AppointmentDetailContract.OPD_ID, View.CurrentLoggedInUserID);
            //Changed Calling in UAT-3850
            return FingerPrintDataManager.ApprovePayment(View.SelectedTenantId, View.AppointmentDetailContract.OrderId, View.SelectedOPDID, View.CurrentLoggedInUserID);
        }

        public bool GetIfApprovePaymentReqd()
        {
            return FingerPrintDataManager.GetIfApprovePaymentReqd(View.SelectedTenantId, View.AppointmentDetailContract.OrderId, View.CurrentLoggedInUserID);
        }

        public bool GetIsRevertToMoneyOrder(int orderPaymentDetailId)
        {
            return FingerPrintDataManager.GetIsRevertToMoneyOrder(View.SelectedTenantId, View.AppointmentDetailContract.OrderId, View.CurrentLoggedInUserID, orderPaymentDetailId);
        }

        public Boolean IsOrderPaymentStatusPending()
        {            
            return FingerPrintDataManager.IsOrderPaymentStatusPending(View.SelectedTenantId, View.AppointmentDetailContract.OrderId, View.CurrentLoggedInUserID);
        }




        public AddressContract ApplicantAddress(int applicantId)
        {
            var UserData = ComplianceDataManager.GetOrganizationUserDetailByOrganizationUserId(applicantId);
            return StoredProcedureManagers.GetAddressByAddressHandleId(UserData.AddressHandleID.Value, UserData.Organization.TenantID.Value);

        }

        public List<OrderDetailContract> OrderSerivceDetail(int OrderId)
        {
            return FingerPrintDataManager.OrderSerivceDetail( View.SelectedTenantId, OrderId, View.CurrentLoggedInUserID);

        }

        public string GetPackageNameForCompleteOrder(Int32 orderId, string serviceType, bool isIdRequired)
        {
            return FingerPrintDataManager.GetPackageNameForCompleteOrder(View.SelectedTenantId, orderId, serviceType, isIdRequired);
        }
        public string GetShippingLineItemName(string serviceType)
        {
            return FingerPrintDataManager.GetShippingLineItemName(View.SelectedTenantId, serviceType);
        }
        public string GetBkgOrderServiceDetails(int orderID)
        {
            return BackgroundProcessOrderManager.GetBkgOrderServiceDetails(View.SelectedTenantId, orderID);
        }

        public void GetServiceStatues()
        {
            View.ServiceStatusList = FingerPrintDataManager.GetServiceStatues(View.SelectedTenantId);
        }
        public void SaveServiceStatus(int detailExtID, int serviceStatusID)
        {
            FingerPrintDataManager.SaveServiceStatus(View.SelectedTenantId, detailExtID, serviceStatusID, false, View.CurrentLoggedInUserID);
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
                                                                                                    View.CurrentLoggedInUserID, View.SelectedTenantId);
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
                    INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract result = ComplianceDataManager.GetItempaymentDetailsByOrderId(ordPaymentDetail.Order.OrderID, View.SelectedTenantId);
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

        public string GetOrderAppointmentStatus(int OrderID)
        {
            return FingerPrintDataManager.GetOrderAppointmentStatus(View.SelectedTenantId, OrderID, View.CurrentLoggedInUserID);
        }

        public bool GetIsOrderRescheduled(int OrderID)
        {
            return FingerPrintDataManager.GetIsOrderRescheduled(View.SelectedTenantId, OrderID, View.CurrentLoggedInUserID);
        }

        public List<OrderPaymentDetail> GetAllPaymentDetailsOfOrderByOrderID(Int32 orderID)
        {
            return ComplianceDataManager.GetAllPaymentDetailsOfOrderByOrderID(View.SelectedTenantId, orderID);
        }

        public bool CheckShipmntPriorAppointment(int OrderID, int AppointmentDetailExtId)
        {
            return FingerPrintDataManager.CheckShipmntPriorAppointment(View.SelectedTenantId, OrderID, View.CurrentLoggedInUserID, AppointmentDetailExtId);
        }

        public void SendServiceStatusChangeMailMessage(string selectedStatus, string serviceName, Int32 OrderId)
        {
            Entity.OrganizationUser organizationUser = new Entity.OrganizationUser();
            organizationUser = GetUserData();
            DateTime? ShipDate;
            ShipDate = FingerPrintDataManager.GetAdditionalServiceShipmentDate(View.SelectedTenantId, View.AppointmentDetailContract.OrderId, View.CurrentLoggedInUserID, View.AppointmentDetailContract.ApplicantAppointmentDetailID);
            Entity.ClientEntity.Order orderDetails = new Entity.ClientEntity.Order();
            orderDetails = FingerPrintDataManager.GetOrderByOrderId(View.SelectedTenantId, OrderId);
            CommunicationManager.SendServiceStatusChangeMailMessage(selectedStatus, organizationUser, View.SelectedTenantId, orderDetails, serviceName, ShipDate);
        }        

        public void GetAppointmentFulFillmentQueueOrderDetailData()
        {
           View.AppointmentDetailContract =
             FingerPrintDataManager.GetAppointmentFulFillmentQueueOrderDetailData(View.CurrentLoggedInUserID,View.IsAdminLoggedIn, View.SelectedTenantId, View.ApplicantAppointmentID);
        }



        public Boolean CanclePackage(Decimal? RefundAmount = null)
        {
            var orderStatusCode = ApplicantOrderStatus.Cancelled.GetStringValue();
            Boolean isCancelledByApplicant = false;
            return FingerPrintDataManager.CancelBkgOrder(View.SelectedTenantId, View.AppointmentDetailContract.OrderId, orderStatusCode, View.CurrentLoggedInUserID, isCancelledByApplicant, View.ApplicantAppointmentID, RefundAmount, View.AppointmentDetailContract.ApplicantOrgUserId);
        }

        //UAT-3734
        public string GetLocTenMaxAllowedDays()
        {
            return SecurityManager.GetLocTenMaxAllowedDays();
        }
        public List<RefundHistory> GetRefundHistory()
        {
            return ComplianceDataManager.GetRefundHistory(View.AppointmentDetailContract.OrderId, View.SelectedTenantId);
        }


        public void GetOrderPkgPaymentDetail()
        {
            if (View.AppointmentDetailContract.OrderId > AppConsts.NONE)
            {
                View.OrderPkgPaymentDetailList = ComplianceDataManager.GetOrderPkgPaymentDetailsByOrderID(View.SelectedTenantId, View.AppointmentDetailContract.OrderId);
            }
        }
        public void AddRefundHistory(RefundHistory refundHistory)
        {
            ComplianceDataManager.AddRefundHistory(refundHistory, View.SelectedTenantId);
            FingerPrintDataManager.SaveUpdateApointmentRefundAudit(refundHistory, View.SelectedTenantId, View.AppointmentDetailContract.ApplicantOrgUserId, View.CurrentLoggedInUserID);
        }
        /// <summary>
        /// Get CustomerProfileId by UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Entity.AuthNetCustomerProfile GetCustomerProfile(Guid userId)
        {
            return ComplianceDataManager.GetCustomerProfile(userId, View.SelectedTenantId);
        }
        public bool SaveOrderPaymentInvoice(Int32 tenantId, Int32 orderID, Int32 currentLoggedInUserId, Boolean modifyShipping)
        {
            return ComplianceDataManager.SaveOrderPaymentInvoice(tenantId, orderID, currentLoggedInUserId, modifyShipping);
        }

        public String GenerateDescription()
        {
            String _tenantName = ComplianceDataManager.GetTenantList(SecurityManager.DefaultTenantID, true).Where(col => col.TenantID == View.SelectedTenantId).FirstOrDefault().TenantName;
            //02/17/2015 [SG]: UAT-1023 - Complio: Update Credit Card Spreadsheet
            //return ("Complio: " + _tenantName + " (Order #:  " + View.OrderId + ")");
            return _tenantName;
        }

        public Boolean CanRevertToMoneyOrder()
        {
            if (View.IsAdminScreen && View.IsAdminLoggedIn)
            {
                return FingerPrintDataManager.CanRevertToMoneyOrder(View.SelectedTenantId, View.AppointmentDetailContract.OrderId);
            }

            return false;
        }

        public Int32 GetPaymentOptionIdByCode(String paymentOptionCode)
        {
            return ComplianceDataManager.GetPaymentOptionIdByCode(paymentOptionCode, View.SelectedTenantId);
        }

        public String UpdateOrderByID(Order _applicantOrder, string orderStatuscode, int ordPaymentDetailId, int paymentModeId, out int _insertedOPDetailId)
        {
            return ComplianceDataManager.UpdateOrderByID(_applicantOrder, orderStatuscode, ordPaymentDetailId, paymentModeId, out _insertedOPDetailId, View.SelectedTenantId, true);
        }

        public void SendRevertNotification(Dictionary<String, object> dictMailData, Entity.CommunicationMockUpData mockData)
        {
            var CommSubEvnt = CommunicationSubEvents.NOTIFICATION_TO_APPLICANTS_FOR_REFUND_AMOUNT_OF_CC_AND_PAYMENT_TYPE_REVERTED_TO_MONEY_ORDER;
            ////// send mail/message notification
            CommunicationManager.SentMailMessageNotification(CommSubEvnt, mockData, dictMailData, View.AppointmentDetailContract.ApplicantOrgUserId, Convert.ToInt32(View.SelectedTenantId), View.AppointmentDetailContract.HeirarchyNodeId);
        }


        public Boolean UpdateAppointmentHistory(string orderStatuscode, int paymentModeId, decimal refundAmount)
        {

            return FingerPrintDataManager.UpdateAppointmentHistory(View.AppointmentDetailContract.OrderId, orderStatuscode, paymentModeId, View.SelectedTenantId, View.CurrentLoggedInUserID, View.ApplicantAppointmentID, refundAmount);
        }

        public void SavePaymentTypeAuditChange(String PaymentModeCode, Int32 OrderId, Int32 ordPaymentDetailId)
        {
            FingerPrintDataManager.SavePaymentTypeAuditChange(PaymentModeCode, OrderId, View.SelectedTenantId, View.CurrentLoggedInUserID, ordPaymentDetailId);
        }
        public void IsFileSentToCbi()
        {
            if (!View.AppointmentDetailContract.IsNullOrEmpty())
                View.IsFileSentToCBI = FingerPrintDataManager.IsFileSentToCbi(View.SelectedTenantId, View.AppointmentDetailContract.OrderId);
        }
        #region UAT-4088
        public Boolean RejectOutOfStateOrderByCBI()
        {
            return FingerPrintDataManager.RejectOutOfStateOrderByCBI(View.ApplicantAppointmentID, View.CurrentLoggedInUserID, View.SelectedTenantId);
        }
        #endregion
        public Entity.OrganizationUser GetUserData()
        {
            return SecurityManager.GetOrganizationUserDetailByOrganizationUserId(View.AppointmentDetailContract.ApplicantOrgUserId);
        }

        /// <summary>
        /// To Get mailing detail.
        /// 
        /// </summary>
        /// 
        public void GetMailingDetail(int OrderId)
        {
            View.MailingAddressData = FingerPrintDataManager.GetMailingDetail(OrderId, View.SelectedTenantId);
        }

        public ReserveSlotContract ReserveSlot(Int32 slotId)
        {
            return FingerPrintSetUpManager.ReserveSlot(0, slotId, View.CurrentLoggedInUserID);
        }
        #region ABI Review
        public Boolean changeAppointmentStatus(String appointmentStatus)
        {
            if (View.ApplicantAppointmentID > AppConsts.NONE)
            {
                return FingerPrintDataManager.UpdateAppointmentStatus(appointmentStatus, View.ApplicantAppointmentID, View.SelectedTenantId, View.CurrentLoggedInUserID);
            }
            return false;
        }
        #endregion

    }
}
