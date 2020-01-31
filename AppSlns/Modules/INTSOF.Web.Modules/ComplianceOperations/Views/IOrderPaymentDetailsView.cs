using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Data;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IOrderPaymentDetailsView
    {
        #region Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IOrderPaymentDetailsView CurrentViewContext
        {
            get;
        }

        /// <summary>
        /// Sets or gets the Tenant Id for the logged-in user.
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// returns the Current Logged In User Id.
        /// </summary>
        Int32 CurrentLoggedInUserId
        {
            get;
        }

        /// <summary>
        /// Sets or gets the Selected Tenant Id from the select tenant dropdown.
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        String OrderStatusCode
        {
            get;
            set;
        }

        Int32 PackageId
        {
            get;
            set;
        }

        Int32 OrganizationUserId
        {
            get;
            set;
        }

        //UAT-916
        ///// <summary>
        ///// Sets or gets the order payment details object.
        ///// </summary>
        //OrderPaymentDetail OrderPaymentDetail
        //{
        //    get;
        //    set;
        //}

        String ParentControl
        {
            get;
            set;
        }
        Boolean IsInvoiceOnly
        {
            get;
            set;
        }
        List<lkpPaymentOption> ListPaymentOptions { get; set; }
        Int32 SelectedPaymentOptionID { get; set; }
        Int32 DPM_ID { get; set; }
        List<PersonAliasContract> PersonAliasList
        {
            get;
            set;
        }

        #region Page Controls Properties



        Int32 OrderId
        {
            get;
            set;
        }

        String TextOrderId
        {
            set;
        }

        String OrderNumber
        {
            get;
            set;
        }

        String OrderDate
        {
            set;
        }

        String SubscriptionStartDate
        {
            set;
        }

        String SubscriptionExpirationDate
        {
            set;
        }

        String TotalOrderValue
        {
            get;
            set;
        }

        List<OrderDetailContract> orderDetailContracts
        { get;
          set;
        }

        String DuePayment
        {
            set;
        }

        String RushOrderPrice
        {
            get;
            set;
        }

        String GrandTotalPrice
        {
            get;
            set;
        }

        String OrderStatus
        {
            set;
        }

        String RushOrderStatus
        {
            set;
        }

        String InstituteHierarchy
        {
            set;
        }

        String FirstName
        {
            get;
            set;
        }

        String MiddleName
        {
            set;
        }

        String LastName
        {
            get;
            set;
        }

        //String Alias1
        //{
        //    set;
        //}

        //String Alias2
        //{
        //    set;
        //}

        //String Alias3
        //{
        //    set;
        //}

        String Gender
        {
            set;
        }

        String DateOfBirth
        {
            set;
        }

        String SocialSecurityNumber
        {
            get;
            set;
        }

        String PrimaryEmail
        {
            set;
        }

        String SecondaryEmail
        {
            set;
        }

        String Phone
        {
            set;
        }

        String SecondaryPhone
        {
            set;
        }

        String Address1
        {
            get;
            set;
        }

        String Address2
        {
            get;
            set;
        }

        String City
        {
            get;
            set;
        }

        String State
        {
            get;
            set;
        }

        String Zip
        {
            get;
            set;
        }

        String Package
        {
            set;
        }

        DateTime ExpiryDate
        {
            get;
            set;
        }

        String DurationMonths
        {
            get;
            set;
        }

        Boolean ShowApprovePayment
        {
            get;
            set;
        }

        Boolean ShowApproveCancellation
        {
            get;
            set;
        }

        String PaymentType
        {

            set;
        }

        String ReferenceNumber
        {
            get;
            set;
        }

        String RejectionReason
        {
            get;
            set;
        }

        String RejectionPaymentReason
        {
            get;
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
        }

        /// <summary>
        /// get object of shared class of search contract
        /// </summary>
        OrderApprovalQueueContract GridSearchContract
        {
            get;
        }

        Int32 NextOrderId
        {
            get;
            set;
        }

        Int32 FirstOrderId
        {
            get;
            set;
        }

        String PaymentTypeCode
        {
            get;
            set;
        }

        OrganizationUserProfile OrganizationUserProfile
        {
            get;
            set;
        }

        Int32 DPPSId
        {
            get;
            set;
        }

        List<BackgroundPackagesContract> lstExternalPackages { get; set; }

        DeptProgramPackageSubscription SelectedPackageDetails { get; set; }

        Int32 BkgOrderID
        {
            get;
            set;
        }

        DataTable BkgPackagesList
        {
            get;
            set;
        }

        Int32? OrderPackageType
        {
            get;
            set;
        }

        String OrderPackageTypeCode
        {
            get;
            set;
        }

        Boolean ShowOfflineSettlement
        {
            get;
            set;
        }

        /* UAt-916
         * /// <summary>
         /// Property to store Online TransactionId, used for Refund process
         /// </summary>
         String TransactionId
         {
             get;
             set;
         }

         /// <summary>
         /// Property to store CCNumber, used for Refund process
         /// </summary>
         String CCNumber
         {
             get;
             set;
         }*/

        /// <summary>
        /// Used to get the CustomerProfileId, for Refund process
        /// </summary>
        Guid UserId
        {
            get;
            set;
        }

        /* UAT-916
         * /// <summary>
         /// Property to store InvoiceNumber, used for Refund process 
         /// </summary>
         String InvoiceNumber
         {
             get;
             set;
         }*/
        #endregion

        List<ServiceFormContract> lstServiceForm { get; set; }
        List<Int32> BopIds { get; set; }

        Int32 PaymentMode_InvoiceWithoutApprovalId { get; set; }

        Int32 PaymentMode_InvoicetoInstitutionId { get; set; }

        #region UAT-806 Creation of granular permissions for Client Admin users

        String SSNPermissionCode { get; set; }
        Boolean IsDOBDisable { get; set; }

        String SocialSecurityNumberMaskedOnly
        {
            set;
        }
        #endregion
        #endregion

        #region UAT-796
        Boolean AutomaticRenewalTurnedOff { get; set; }
        Boolean ShowAutoRenewalControl { get; set; }
        Boolean DisableAutoRenewalControl { get; set; }
        #endregion

        /// <summary>
        /// Get and set next page path.
        /// </summary>
        String NextPagePath
        {
            get;
            set;
        }

        Boolean IsCompliancePartialOrderCancelled
        {
            get;
            set;
        }

        String PartialOrderCancellationXML
        {
            get;
            set;
        }

        String PartialOrderCancellationTypeCode
        {
            get;
            set;
        }
        #region UAT-916 : WB: As an application admin, I should be able to define payment options at the package level in addition to the node leve
        List<OrderPkgPaymentDetail> OrderPkgPaymentDetailList
        { get; set; }

        List<OrderPaymentDetail> OrderPaymentDetailList
        {
            get;
            set;
        }

        OrderPaymentDetail CompPkgOrderPaymentDetail
        {
            get;
            set;
        }

        String SetCompliancePkgPaymentType
        {
            set;
        }

        Int32 OrderPaymentDetailID
        {
            get;
            set;
        }

        String OrderPaymentDetailStatusCode
        {
            get;
            set;
        }
        Boolean IsCompliancePackageInclude
        {
            get;
            set;
        }
        //UAT-4537
        DataSet lstPaymentOption
        {
            get;
            set;
        }
        //UAT-4537
        List<String> lstPendingApprovalPackageNames
        {
            get;
            set;
        }
        Decimal OrderPaymentAmount
        {
            get;
            set;
        }
        #endregion

        Boolean ShowApproveRejectButtons { get; set; }

        String PackageHeading { set; }

        #region UAT-1189:If payment method is the same, both tracking and screening are getting cancelled when the applicant attempts to cancel the tracking order.

        /// <summary>
        /// Property return boolean value for cancellation of compliance package due to change subscription
        /// </summary>
        Boolean IsCompliancePackageCancelledByChangeSubs { get; set; }
        #endregion


        //UAT-1558 As a Student, I should be able to mark when I have "Graduated" from a tracking and/or screening package's corresponding program
        String ArchiveStateCode
        {
            get;
            set;
        }
        //UAT-1683 : Implementation of Archive WRT Graduated and Un-Graduated Archive State
        String BkgArchiveStateCode
        {
            get;
            set;
        }

        //UAT:1261
        Int32 OrgUsrID { get; }

        #region UAT-2212: Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality

        String NoMiddleNameText
        {
            get;
        }
        #endregion

        Boolean IsClientAdmin { get; set; }

        #region UAT-2447
        String PhoneUnMasking
        {
            set;
        }

        String SecondaryPhoneUnMasking
        {
            set;
        }
        #endregion

        //UAT-2971

        Int32 ApplicantOrgUserID { get; set; }

        //UAT-3077
        Boolean IsItemPaymentOrder
        {
            get;
            set;
        }

        //UAT-3521 ||CBI || CABS
        AppointmentSlotContract AppointSlotContract
        {
            get;
            set;
        }

        Int32 SelectedSlotID { get; set; }
        Boolean IsBkgOrderWithAppointment { get; set; }
        //Boolean IsOutOfStateAppointment { get; set; }

        Boolean IsLocationServiceTenant { get; set; }
        OnlinePaymentTransaction OnlinePaymentTransaction { get; set; }
        Order OrderDetail { get; set; }
        Decimal OnlinePaymentAmount { get; set; }
        Boolean IsRevokedAppointment { get; set; }
        List<Entity.lkpSuffix> lstSuffixes { get; set; }
        String LanguageCode { get; }
        Boolean IsFileSentToCBI { get; set; }

        #region UAT-3632
        String PaymentItemName
        {
            get;
            set;
        }

        Boolean IsRequirementItemPayment
        {
            get;
            set;
        }

        String ItemPaymentCIDOROrderID
        {
            get;
            set;
        }

        #endregion

        //UAT-3335
        SharedUserDashboardDetailsContract SharedUserDetails { get; set; }

        #region UAT - 3636
        String TrackingPkgCancelledBy { get; set; }
        #endregion

        List<OrderContract> lstOrderQueue
        {
            get;
            set;
        }

        FingerPrintOrderKeyDataContract lstFingerPrintData
        {
            get;
            set;
        }
        PreviousAddressContract MailingAddressData
        {
            get;
            set;
        }

        String CompliancePkgCancelledBy { set; }

        String CompliancePkgCancelledOn { set; }

        //String BkgPkgCancelledBy { set; }

        //String BkgPkgCancelledOn { set; }

    }
}




