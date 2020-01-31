using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ComplianceOperation;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.BkgOperations;
using System.Data;

namespace DAL.Interfaces
{
    public interface IFingerPrintClientRepository
    {
        Dictionary<Int32, Int32> GetLocationHierarchy(Int32 locationID);
        Boolean IsPrinterAvailableAtOldLoc(Int32 orderId);
        Boolean SaveUpdateApplicantAppointmentHistory(AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserId);
        Address GetMailingAddressDetails(Int32 OrderID, Int32 orgUserID);
        decimal GetOrderPriceTotal(Int32 OrderID, Int32 orgUserID);
        string GetOrderNumber(Int32 OrderID, Int32 orgUserID);
        SelectedMailingData GetSelectedMailingOptionPrice(Int32 OrderID, Int32 orgUserID);
        decimal GetSentForOnlinePaymentAmount(Int32 OrderID);
        PreviousAddressContract GetAddressData(Int32 OrderID, Int32 orgUserID);
        PreviousAddressContract GetShippingAddressData(Int32 OrderID, Int32 orgUserID);
        void UpdateMailingDetailXML(Int32 orderID, Guid mailingAddressHandleId,String mailingoptionID,String mailingOptionPrice);
        Int32 GetOrderHeirarchyNodeId(Int32 OrderID);
        Order GetOrderByOrderId(Int32 OrderID);
        void UpdateApplicantAppointmentExt(Int32 OrderID, PreviousAddressContract mailingAddress, Boolean isLocationServiceTenant, Int32 orgUserID, bool IsPaymentReqInMdfyShpng, Decimal MailingPrice = 0);

        void UpdateApplicantAppointmentDetailExt(Int32 OrderID, PreviousAddressContract mailingAddress, Boolean isLocationServiceTenant, Int32 orgUserID);
        
        List<ServiceStatusContract> GetServiceStatues();        
        void SaveServiceStatus(Int32 detailExtID, Int32 serviceStatusID, bool IsServiceStatusRejected, int CurrentLoggedInUserID);
        string GetOrderAppointmentStatus(Int32 OrderID, Int32 orgUserID);
        bool GetIsOrderRescheduled(Int32 OrderID, Int32 orgUserID);
        bool CheckShipmntPriorAppointment(int OrderID, int CurrentLoggedInUserID, int AppointmentDetailExtId);
        DateTime? GetAdditionalServiceShipmentDate(Int32 OrderID, Int32 CurrentLoggedInUserID, Int32 AppointmentDetailExtId);
        List<String> GetServiceStatus(Int32 OrderID, Int32 orgUserID);
        void SaveTrackingNumber(Int32 detailExtID, string trackingnum);
        Boolean ResetApplicantAppointmenBkgOrderStatus(AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserId);
        List<OrderDetailContract> OrderSerivceDetail(Int32 orderID, Int32 currentLoggedInUserID);   
        Boolean SaveTenantLocationMapping(Int32 selectedLocationID, Int32 tenantId, Int32 selectedDpmId, Int32 currentLoggedInUserId);
        Boolean HideReschedule(AppointmentSlotContract AppointmentSlotContract);
        Boolean HideCancel(Int32 orderID);
        int? GetRevertToMoneyDetails(List<int> PaymentDetails);
        Boolean HideRescheduleForAdmin(int OrderId);

        List<Int32> GetDPMLocationIDs(Int32 tenantId, Int32 selectedDpmId);
        ReserveSlotContract SubmitApplicantAppointment(ReserveSlotContract reserveSlotContract, Int32 currentLoggedInUserId, Boolean isCompleteYourOrderClick);
        Boolean SubmitOrderBillingCodeMapping(Int32 orderID, String billingCode, Int32 currentLoggedInUserID); //UAT 4243
        Boolean DeleteTenantLocationMapping(Int32 tenantId, Int32 selectedDPMId, Int32 selectedLocationId, Int32 currentLoggedInUserId);
        Boolean DeleteTenantNodeLocationMapping(Int32 tenantId, Int32 selectedDPMId, Int32 currentLoggedInUserId);
        //  Boolean SaveRescheduledAppointment(Int32 tenantId, Int32 currentLoggedInUserId, AppointmentSlotContract appointSlotContract);
        Boolean DeleteApplicantAppointment(Int32 tenantId, Int32 orderId, Int32 currentLoggedInUserId, Int32 applicantAppointmentId, Decimal? RefundAmount);
        Boolean IsOrderPaymentStatusPending(Int32 tenantId, Int32 orderId, Int32 currentLoggedInUserId);
        Boolean ApprovePayment(Int32 tenantId, Int32 orderId, Int32 orderPaymentDetailId, Int32 orderStatusPaidId, Int32 currentLoggedInUserId, Int32 newOrderStatusTypeID);
        bool GetIfApprovePaymentReqd(Int32 tenantId, Int32 orderId, Int32 currentLoggedInUserId);
        bool GetIsRevertToMoneyOrder(Int32 tenantId, Int32 orderId, Int32 currentLoggedInUserId, int orderPaymentDetailId);
        AppointmentSlotContract GetBkgOrderWithAppointmentData(Int32 tenantId, Int32 OrderId, Int32 ApplicantOrgUserID);
        List<ValidateRegexDataContract> GetPersonalInformationExpressions(Int32 BkgPkgID, String languageCode);
        Boolean SaveFingerPrintDocumentsStaging(List<String> lstFileName, Int32 currentLoggedInUserId);
        List<String> GetFingerPrintDocStaging();
        List<FingerPrintOrderContract> GetInProgressFingerPrintOrders(Int32 completedOrderStatusTypeId);
        Boolean SaveFingerprintApplicantDocument(List<FingerPrintOrderContract> lstFingerPrintOrderContract, Int32 bkgProcessUserID);
        Boolean DeleteFingerPrintDocStaging(List<String> lstFingerPrintDocStagingToDelete, Int32 bkgProcessUserID);
        Boolean AddDocInApplicantAppointmentDetail(List<FingerPrintOrderContract> lstFingerPrintOrderContract, Int32 bkgProcessUserID);
        Boolean CanRevertToMoneyOrder(Int32 orderId);

        Boolean UpdateAppointmentHistory(Int32 orderid, String orderStatusCode, Int32 paymentModeId, Int32 currentLoggedInUserId, Int32 tenantId, Int32 applicantAppointmentId, decimal refundAmount);
        Boolean SaveUpdateAppointmentAudit(AppointmentOrderScheduleContract AppOrdSchdContract, AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserId, Int32 oldStatusID);
        Boolean SaveUpdateApointmentRefundAudit(RefundHistory refundData, Int32 ApplicantOrgUserId, Int32 CurrentUserId);
        PreviousAddressContract GetMailingDetail(Int32 OrderID);

        Boolean SaveUpdateAppointmentStatusAudit(AppointmentOrderScheduleContract AppointmentData, String StatusCode, Int32 BackgroundProcessId, Int32 OldStatusId);
        string GetServiceDescription();

        Dictionary<String, String> ValidateCBIUniqueID(String CBIUniqueID);

        // UAT-4271
        List<LookupContract> GetCBIUniqueIdByAcctNameOrNumber(String acctNameOrNumber);
        Boolean SavePaymentTypeAuditChange(String PaymentModeCode, Int32 OrderId, Int32 CurrentLoggedInUserId, Int32 oldOrderPaymentDetailId);
        void ImportDataToTable(string dataToImport, CDRFileDetailContract fileDetailContract, int? backgroundProcessUserId);
        Int64 GetLastRecordInsertedId();
        List<LocationContract> GetLocationForRescheduling(Int32 orderid, string lng, string lat);
        List<LocationContract> GetApplicantAvailableLocation(Int32 tenantId, string lng, string lat, string orderRequestType, Int32 orderID = 0);
        List<FileResultStatusUpdateContract> GetAllUpdatedFileResults(Int32 ChunkSize, Int32? ApplicantAppointmentId);
        void UpdateStatusForFileResult(FileResultStatusUpdateContract fileResult, Int32 CurrentLoggedInUserId, Boolean IsSubmittedToCBI, Boolean IsContactAgency);
        Int32 GetOrderIdByOrderNumber(string orderNumber);

        List<FingerPrintRecieptContract> GetUserRicieptFileData(Int32 ChunkSize);
        Boolean UpdateFileRecieptDispatched(Int32 ApplicantAppointmentId, Int32 UserId);

        List<LanguageTranslateContract> GetLanguageTranslationData(CustomPagingArgsContract GridCustomPagingArgs, Int32 organizationUserId, LanguageTranslateContract filterContract);
        bool SaveLanguageTranslateDetails(LanguageTranslateContract languageTranslateContract, Int32 currentLoggedInUserId);

        List<SystemSpecificLanguageText> GetSystemSpecificTranslatedText(int entityTypeID);


        #region CBI Billing Status
        List<CBIBillingStatusContract> GetCBIBillingStatus(CustomPagingArgsContract customPagingArgsContract, CBIBillingStatusContract billingStatusContract);
        bool SaveCBIBillingStatus(CBIBillingStatusContract billingStatusContract, Int32 currentLoggedInUserId);
        #endregion

        bool IsFileSentToCbi(Int32 OrderId);
        CustomFormDataContract GetCustomAttributes(Int32 packageID, String cBIUniqueID, String langCode);
        //UAT-3850
        CBIBillingStatu GetCBIBillingStatusData(String cbiUniqueId, String billingCode);
        OrderBillingCodeMapping GetOrderBillingCode(Int32 orderId);
        String GetUserProfileName(Int32 OrderId);

        #region UAT - 4088
        List<FileResultStatusUpdateContract> GetAllManuallyRejectedOrders(Int32 AppointmentID);
        Boolean RejectOutOfStateOrderByCBI(Int32 ApplicantAppointmentId, Int32 CurrentLoggedInUserId);
        #endregion

        #region UAT - 4270
        Boolean SaveManualFingerPrintFile(ApplicantDocument appDocument, Int32 FingerprintAppointmentId, Int32 CurrentLoggedinUserId, Boolean IsAbiReviewUpload);
        #endregion
        #region ABI Review
        Boolean UpdateAppointmentStatus(String appointmentStatusCode, Int32 fingerPrintApplicantAppointmentID, Int32 CurrentLoggedinUserId);
        #endregion

        ApplicantFingerPrintFileImageContract GetFingerPrintImageData(Int32 ApplicantAppointmentDetailID);

        Boolean SaveFingerprintApplicantImages(List<FingerPrintOrderContract> lstFingerPrintOrderContract, Int32 bkgProcessUserID);
        Boolean ChangeSendToCBIAppointmentStatus(List<AppointmentOrderScheduleContract> appointmentSchedules, Int32 CurrentLoggedinUserId);

        List<LocationServiceAppointmentAuditContract> GetOrderHistoryList(Int32 OrderID, bool IsCABSAppointment);
        DataTable GetBkgPkgPrevOrderDetails(Int32 tenantId, Int32 orgainizatuionUserId);
        List<BackgroundServiceContract> GetOrderBackgroundServices(int OrderId);
        bool AdditionalServicesNotshipped(int OrderId);
        bool AdditionalServicesExist(int OrderID);
        LocationContract GetLocationByOrderId(int OrderId);
        LocationContract GetLocationByLocationid(int LocationId);
        bool RescheduleApplicantAppointmentHistory(AppointmentSlotContract scheduleAppointmentContract, int currentLoggedInUserId);
        bool HideABIReviewForFulfilment(int OrderId);
        string GetPackageNameForCompleteOrder(int orderId, string serviceType, bool isIdRequired);
        string GetShippingLineItemName(string serviceType);
        PreviousAddressContract GetAddressThroughAddressHandleID(string MailingAddressHandleId);
    }
}
