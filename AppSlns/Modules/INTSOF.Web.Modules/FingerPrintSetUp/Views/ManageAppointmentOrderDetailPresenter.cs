using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class ManageAppointmentOrderDetailPresenter : Presenter<IManageAppointmentOrderDetailView>
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

        public void GetAppointmentOrderDetailData()
        {
            View.AppointmentDetailContract = FingerPrintSetUpManager.GetAppointmentOrderDetailData(View.CurrentLoggedInUserID, View.IsAdminLoggedIn, View.SelectedTenantId.ToString(), View.ApplicantAppointmentID);
            //if (!View.AppointmentDetailContract.IsNullOrEmpty() && View.AppointmentDetailContract.FingerPrintingSite == FingerPrintingSite.OUT_OF_STATE.GetStringValue()
            //    && View.AppointmentDetailContract.AppointmentStatusCode == FingerPrintAppointmentStatus.SUBMITTED_TO_CBI.GetStringValue())
            //{
            //    var updatedata = FingerPrintDataManager.GetAllUpdatedFileResults(View.SelectedTenantId, 1, View.ApplicantAppointmentID);
            //}
        }

        public void GetOrderPaymentDetailList()
        {
            View.OrderPaymentDetailList = ComplianceDataManager.GetAllPaymentDetailsOfOrderByOrderID(View.SelectedTenantId, View.AppointmentDetailContract.OrderId);
        }

        public void GetOrderPaymentContactList()
        {
            View.PaymentDetailContactList = ComplianceDataManager.GetOrderPaymentContactList(View.SelectedTenantId, View.AppointmentDetailContract.OrderId);
        }


        public Boolean ApprovePayment()
        {
            //return FingerPrintDataManager.ApprovePayment(View.SelectedTenantId, View.AppointmentDetailContract.OrderId, View.AppointmentDetailContract.OPD_ID, View.CurrentLoggedInUserID);
            //Changed Calling in UAT-3850
            return FingerPrintDataManager.ApprovePayment(View.SelectedTenantId, View.AppointmentDetailContract.OrderId, View.SelectedOPDID, View.CurrentLoggedInUserID);
        }

        public AddressContract ApplicantAddress(int applicantId)
        {

            var UserData = ComplianceDataManager.GetOrganizationUserDetailByOrganizationUserId(applicantId);
            return StoredProcedureManagers.GetAddressByAddressHandleId(UserData.AddressHandleID.Value, UserData.Organization.TenantID.Value);

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
        public bool SaveOrderPaymentInvoice(Int32 tenantId, Int32 orderID, Int32 currentLoggedInUserId, Boolean modifyShipping)
        {
            return ComplianceDataManager.SaveOrderPaymentInvoice(tenantId, orderID, currentLoggedInUserId, modifyShipping);
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
