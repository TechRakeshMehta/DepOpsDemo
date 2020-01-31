#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ApplicationConstants.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils
{
    public class ApplicationConstant
    {
        public const String INLHEADER = "INLHeader";

        public static String SecuritySiteMapProvider = "SecuritySiteMapProvider";
    }

    //public class SysXSupplierPortalConsts
    //{

    //    #region SupplierDashboard

    //    /// <summary>
    //    /// Constant for SupplierId
    //    /// </summary>
    //    public const String SUPPLIER_SEARCH_OPTION_SUPPLIERID = "SupplierID";

    //    /// <summary>
    //    /// Constant for Service Request Id
    //    /// </summary>
    //    public const String SUPPLIER_SERVICE_REQUEST_ID = "SupplierServiceRequestID";

    //    /// <summary>
    //    /// Constant for Service Order Id
    //    /// </summary>
    //    public const String SUPPLIER_SERVICE_ORDER_ID = "SupplierServiceOrderIds";

    //    /// <summary>
    //    /// Constant for Viewmode in page
    //    /// </summary>
    //    public const String VIEW_MODE = "ViewMode";

    //    /// <summary>
    //    /// Constant for Child in page
    //    /// </summary>
    //    public const String CHILD = "Child";

    //    /// <summary>
    //    /// Constant for Supplier Activity
    //    /// </summary>
    //    public const String SUPPLIER_ACTIVITY_TYPE = "SupplierActivityType";

    //    public const String PAGE_SOURCE = "PageSource";

    //    /// <summary>
    //    /// Constant for page Source name
    //    /// </summary>
    //    public const String SOURCE_NAME = "SourceName";

    //    #endregion

    //    #region Supplier Dashboard Action dropdown values

    //    /// <summary>
    //    /// Constant for "INL Appointment" values in dropdown
    //    /// </summary>       
    //    public const String INL_APPOINTMENT = "INL Appointment";

    //    /// <summary>
    //    /// Constant for "Submit INL Results" values in dropdown
    //    /// </summary>
    //    public const String SUBMIT_INL_RESULTS = "Submit INL Results";

    //    /// <summary>
    //    /// Constant for "Submit IRF" values in dropdown
    //    /// </summary>        
    //    public const String SUBMIT_IRF = "Submit IRF";

    //    /// <summary>
    //    /// Constant for "Submit ACF" values in dropdown
    //    /// </summary>  
    //    public const String SUBMIT_ACF = "Submit ACF";

    //    /// <summary>
    //    /// Constant for "Determine Reassignment" values in dropdown
    //    /// </summary>  
    //    public const String DETERMINE_REASSIGNMENT = "Determine Reassignment";

    //    /// <summary>
    //    /// Constant for "Extension Request" values in dropdown
    //    /// </summary>  
    //    public const String EXTENSION_REQUEST = "Extension Request";

    //    /// <summary>
    //    /// Constant for "Unable to Locate Property" values in dropdown
    //    /// </summary> 
    //    public const String UNABLE_TO_LOCATE_PROPERTY = "Unable to Locate Property";

    //    /// <summary>
    //    /// Constant for "Accept" button
    //    /// </summary>
    //    public const String ACCEPT = "Accept";

    //    /// <summary>
    //    /// Constant for "Reject" button
    //    /// </summary>
    //    public const String REJECT = "Reject";

    //    /// <summary>
    //    /// Constant for Status Assigned
    //    /// </summary>
    //    public const String ASSIGNED = "Assigned";

    //    #endregion

    //    #region SupplierDashboard Grid Name

    //    /// <summary>
    //    /// Constant for Supplier dashboard grid name
    //    /// </summary>
    //    public const String SUPPDASHBOARDREQUEST = "SuppDashBoardRequest";

    //    /// <summary>
    //    /// Constant for Supplier dashboard service request grid name
    //    /// </summary>
    //    public const String SUPPSERVICEREQUESTTYPE = "SuppServiceRequestType";

    //    /// <summary>
    //    /// Constant for Supplier dashboard service order grid name
    //    /// </summary>
    //    public const String SUPPSERVICEORDER = "SuppServiceOrder";

    //    #endregion

    //}
    /// <summary>
    /// Handles the child controls.
    /// </summary>
    /// <remarks></remarks>
    public static class ChildControls
    {
        /// <summary>
        /// To store value for SecurityMapBlockFeature.
        /// </summary>
        public static String SecurityMapBlockFeature = @"UserControl\MapBlockFeature.ascx";

        /// <summary>
        /// To store value for SecurityMapProductFeature.
        /// </summary>
        public static String SecurityMapProductFeature = @"UserControl\MapProductFeature.ascx";

        /// <summary>
        /// To store value for SecurityMapRoleFeature.
        /// </summary>
        public static String SecurityMapRoleFeature = @"UserControl\MapRoleFeature.ascx";

        /// <summary>
        /// To store value for SecurityMapUserRole.
        /// </summary>
        public static String SecurityMapUserRole = @"UserControl\MapUserRole.ascx";

        /// <summary>
        /// To store value for SecurityMapUserInstitution.
        /// </summary>
        public static String SecurityMapUserInstitution = @"UserControl\MapUserInstitution.ascx";

        /// <summary>
        /// To store value for SecurityMapRolePolicy.
        /// </summary>
        public static String ManageProviderCompany = @"UserControl\ProviderCompany.ascx";

        /// <summary>
        /// 
        /// </summary>
        public static String SupplierPortal = @"UserControl\SerReqDetails.ascx";

        /// <summary>
        /// Constant for Supplier portal user control ServiceOrderActivity.ascx
        /// </summary>
        public static String SupplierPortalServiceOrderActivity = @"UserControl\ServiceOrderActivity.ascx";

        /// <summary>
        /// 
        /// </summary>
        public static String CalendarSetup = @"CalendarAdmin\CalendarSetup.ascx";

        /// <summary>
        /// To store value for SecurityMapRolePolicy.
        /// </summary>
        public static String SecurityMapRolePolicy = @"UserControl\MapRolePolicy.ascx";

        /// <summary>
        /// To store value for SecurityManageRole.
        /// </summary>
        public static String SecurityManageRole = @"ManageRole.ascx";

        /// <summary>
        /// To store value for SecurityManageUser.
        /// </summary>
        public static String SecurityManageUser = @"ManageUsers.ascx";

        public static String SecurityManageUserGroup = @"ManageUserGroup.ascx";

        public static String SecurityManageFootprint = @"ManageFootprint.ascx";
        public static String SecurityManageProgram = @"ManagePrograms.ascx";
        public static String SecurityManageGrade = @"ManageGrade.ascx";


        public static String SecurityManageCountyRules = @"ManageCountyRules.ascx";

        public static String SecurityManageCategory = @"ManageCategory.ascx";

        public static string ServiceRequestInsView = @"UserControl\CSRInsView.ascx";

        public static string SearchServiceRequestResult = @"UserControl\SearchSRResult.ascx";

        public static string ServiceRequestSummary = @"UserControl\ServiceRequestSummary.ascx";

        public static string ServiceRequestPreservationView = @"UserControl\CSRPreservationView.ascx";

        public static String ManageFilesAddNewFile = @"UserControl\AddBatch.ascx";

        public static String ManageFilesSaveMapFile = @"UserControl\SaveMapFile.ascx";

        public static String ManageFilesProcessFile = @"UserControl\ProcessDuplicates.ascx";

        /// <summary>
        /// To store value for SecurityManageUser.
        /// </summary>
        public static String SecurityManageSubTenant = @"ManageSubTenant.ascx";

        public static String SecurityManageClientSubTenant = @"ManageSubClientTenant.ascx";

        public static string PrintAcfWithData = @"UserControl\Print\PrintACFWithData.ascx";

        public static string PrintAcfTemplate = @"UserControl\Print\PrintTemplate.ascx";

        public static String EditProfilePage = @"~\ApplicantModule\UserControl\EditProfile.ascx";
        public static String ApplicantPortfolioDetailPage = @"~\SearchUI\UserControl\ApplicantPortfolioDetails.ascx";
        public static String ClientProfilePage = @"~\SearchUI\UserControl\ClientProfile.ascx";

        public static String ApplicantMessageGrid = @"~\SearchUI\UserControl\ApplicantMessageGrid.ascx";

        public static String ApplicantPortFolioSearchPage = @"~\SearchUI\ApplicantPortFolioSearch.ascx";

        public static String ApplicantPortFolioSearchCopyPage = @"~\SearchUI\ApplicantPortfolioSearchMaster.ascx";

        public static String ApplicantComprehensiveSearchPage = @"~\SearchUI\ApplicantComprehensiveSearch.ascx";
        //public static String ApplicantDashboard = @"~\ApplicantModule\ApplicantDashboard.ascx";
        //public static String InternalDashboard = @"~\DashBoard\InternalDashboard.ascx";
        //public static String ExternalDashboard = @"~\DashBoard\ExternalDashboard.ascx";
        //public static String ApplicantDashboardFullPath = @"~\DashBoard\InternalDashboard.ascx";
        //public static String UserDashBoard = @"DashBoard.ascx";
        public static String ApplicantSearch = @"ApplicantSearch.ascx";
        public static String ClientUserSearchPage = @"~\SearchUI\ClientUserSearch.ascx";
        public static String SetupDepartmentProgram = @"~\ComplianceAdministration\Pages\SetupDepartmentProgram.aspx";

        public static String ViewPackageDetail = @"~\ComplianceOperations\UserControl\ViewPackageDetail.ascx";

        public static String RenewalOrder = @"~\ComplianceOperations\UserControl\RenewalOrder.ascx";

        public static String SupplementService = @"~\BkgOperations\UserControl\BkgServiceItemCustomForm.ascx";

        public static String InstituteChangeRequestDetail = @"~\Mobility\UserControl\InstituteChangeRequestDetail.ascx";

        public static String InstituteChangeRequestQueue = @"~\Mobility\InstitutionChangeRequestQueue.ascx";

        public static String ViewBackroundPackageDetail = @"~\BkgOperations\UserControl\ViewBkgPackageDetail.ascx";
        public static String SupportPortalDetail = @"~\SearchUI\UserControl\SupportPortalDetails.ascx";

        #region Supplier Portal


        //  public static String SupplierPortalServiceOrderActivity = @"UserControl\ServiceOrderActivity.ascx";


        public static String UploadPhotoAndDocs = @"UserControl\UploadPhotoNDoc.ascx";


        #endregion

        #region BidManagement

        /// <summary>
        /// To store value for BidDetails.
        /// </summary>
        public static String BidDetails = @"UserControl/BidDetails.ascx";

        /// <summary>
        /// To store value for CreateBid.
        /// </summary>
        public static String CreateBid = @"UserControl/CreateBid.ascx";

        /// <summary>
        /// To store value for BidSummary.
        /// </summary>
        public static String BidSummary = @"UserControl/BidSummary.ascx";

        /// <summary>
        /// To store value for SupplierDetail.
        /// </summary>
        public static String SupplierDetail = @"UserControl/ViewSupplierDetail.ascx";

        /// <summary>
        /// To store value for ServiceOrderDetail.
        /// </summary>
        public static String ServiceOrderDetail = @"UserControl/ViewServiceOrderDetail.ascx";

        /// <summary>
        /// To store value for CreateEyeballEstimate.
        /// </summary>
        public static String CreateEyeballEstimate = @"UserControl/CreateEyeballEstimate.ascx";

        /// <summary>
        /// To store value for AssetEmergencyBidDetail.
        /// </summary>
        public static String AssetEmergencyBidDetail = @"UserControl/AssetEmergencyBidDetail.ascx";

        #endregion

        #region Client Compliance Management
        public static String ClientCompliances = @"~\ComplianceAdministration\UserControl\CopyTree.ascx";
        //For Copy compliance to Client Usercontrol 
        public static String CopyComplainceToClient = @"~\ComplianceAdministration\UserControl\CopyComplainceToClient.ascx";
        public static String ClientCompliancesCategories = @"~\ComplianceAdministration\UserControl\ManageClientCategories.ascx";
        public static String SubscriptionDetail = @"UserControl\CompliancePackageDetails.ascx";
        public static String ManagePackageSubscription = @"~\ComplianceOperations\ManagePackageSubscription.ascx";
        public static String PackageSubscription = @"~\ComplianceOperations\PackageSubscription.ascx";

        public static String ManageUploadDocuments = @"UserControl\ManageUploadDocument.ascx";

        public static String ComplianceSearchControl = @"~\ComplianceOperations\AdminComplianceSearch.ascx";
        public static String VerificationDetailsNew = @"~\ComplianceOperations\UserControl\VerificationDetails.ascx";
        public static String ReconciliationQueue = @"~\ComplianceOperations\DataReconciliationQueue.ascx";
        public static String ReconciliationDetail = @"~\ComplianceOperations\UserControl\ReconciliationDetail.ascx";
        public static String VerificationDetails = @"~\ComplianceOperations\UserControl\VerificationDetail.ascx";
        public static String VerificationApplicantPanel = @"~\ComplianceOperations\UserControl\VerificationApplicantPanel.ascx";
        public static String ExceptionDetails = @"~\ComplianceOperations\UserControl\ExceptionDetail.ascx";
        public static String VerificationQueue = @"~\ComplianceOperations\ItemsDataVerificationQueue.ascx";
        public static String ExceptionVerificationQueue = @"~\ComplianceOperations\ExceptionDataVerificationQueue.ascx";
        public static String ExceptionUserWorkQueue = @"~\ComplianceOperations\UserExceptionDataQueue.ascx";
        public static String UserWorkQueue = @"~\ComplianceOperations\UserItemsDataQueue.ascx";
        public static String ExceptionQueue = @"~\ComplianceOperations\ItemDataExceptionQueue.ascx";
        public static String DatatItemSearch = @"~\ComplianceOperations\AdminDataItemSearch.ascx";
        public static String AssigneeDatatItemSearch = @"~\ComplianceOperations\AssigneeDataItemSearch.ascx";
        public static String OrderPaymentDetails = @"~\ComplianceOperations\UserControl\OrderPaymentDetails.ascx";
        public static String OrderQueue = @"~\ComplianceOperations\OrderQueue.ascx";
        public static String BkgOrderQueue = @"~\BkgOperations\OrderApprovalQueue.ascx";
        public static String ApplicantDataEntryHelp = @"UserControl\ApplicantDataEntryHelp.ascx";
        public static String EsclationAssignmentQueue = @"~\ComplianceOperations\VerificationAssignmentQueue.ascx";
        public static String EsclationUserQueue = @"~\ComplianceOperations\VerificationUserQueue.ascx";
        public static String EsclationExceptionAssignmentQueue = @"~\ComplianceOperations\ExceptionAssignmentQueue.ascx";
        public static String EsclationExceptionUserQueue = @"~\ComplianceOperations\ExceptionUserQueue.ascx";
        public static String VideTutorialDashBoard = @"~\Dashboard\VideoTutorialLoader.ascx";
        public static String FeatureBookmark = @"~\PersonalSettings\ManageFeatureBookmarks.ascx";
        #endregion

        #region WebSiteSetup
        public static string webSiteSetup = @"~\WebSite\WebSiteSetUp.ascx";
        public static string webSiteMarkUp = @"~\WebSite\Markup.ascx";
        public static string webSitePageList = @"~\WebSite\UserControl\WebsitePageList.ascx";
        #endregion

        #region APPLICANT ORDER PROCESS

        public static String ApplicantProfile = @"~\ComplianceOperations\UserControl\ApplicantProfile.ascx";
        public static String CustomFormLoad = @"~\BkgOperations\UserControl\CustomFormLoad.ascx";
        public static String ApplicantPendingOrder = @"~/ComplianceOperations/UserControl/PendingOrder.ascx";
        public static String ServiceItemCustomForm = @"~/BkgOperations/UserControl/CustomFormLoadForServiceItem.ascx";
        public static String ApplicantOrderReview = @"~/ComplianceOperations/UserControl/OrderReview.ascx";
        public static String ApplicantDisclaimerPage = @"~\ComplianceOperations\UserControl\ApplicantDisclaimer.ascx";
        public static String ApplicantOrderConfirmation = @"~/ComplianceOperations/UserControl/OrderConfirmation.ascx";
        public static String ItemPaymentConfirmation = @"~/ComplianceOperations/UserControl/ItemPaymentConfirmation.ascx";
        public static String OrderHistory = @"~/ComplianceOperations/OrderHistory.ascx";
        public static String RushOrderReview = @"~\ComplianceOperations\UserControl\RushOrderReview.ascx";
        public static String ApplicantRushOrderConfirmPage = @"~/ComplianceOperations/UserControl/RushOrderConfirmation.ascx";
        public static String OnlinePaymentSubmission = "Pages/OnlinePaymentSubmission.aspx";
        public static String PaypalPaymentSubmission = "Pages/PaypalPaymentSubmission.aspx";
        public static String ApplicantDisclosurePage = @"~\ComplianceOperations\UserControl\ApplicantDisclosure.ascx";
        public static String ModifyShipping = @"~\ComplianceOperations/UserControl/ModifyShippingInfo.ascx"; 
        public static String OrderPayment = @"~\ComplianceOperations\UserControl\OrderPayment.ascx";
        public static String CIMAccountSelectionPage = "Pages/CIMAccountSelection.aspx";
        public static String ApplicantRequiredDocumentationPage = @"~\ComplianceOperations\UserControl\ApplicantRequiredDocumentation.ascx";
        public const String FINGER_PRINTDATA_CONTROL = @"~/FingerPrintSetUp/UserControl/FingerPrintDataControl.ascx"; //UAT 3521
        public const String APPLICANT_APPOINTMENT_SCHEDULE = @"~/FingerPrintSetUp/UserControl/ScheduleApplicantAppointment.ascx";  //UAT 3521
        public static String AdminEntryCustomFormLoad = @"~\AdminEntryPortal\UserControl\AdminEntryCustomFormLoad.ascx";
        public static String AdminEntryApplicantDisclosurePage = @"~\AdminEntryPortal\UserControl\AdminEntryApplicantDisclosure.ascx";
        public static String AdminEntryApplicantDisclaimerPage = @"~\AdminEntryPortal\UserControl\AdminEntryApplicantDisclaimer.ascx";
        public static String AdminEntryApplicantRequiredDocumentationPage = @"~\AdminEntryPortal\UserControl\AdminEntryApplicantRequiredDocumentation.ascx";
        public static String AdminEntryApplicantOrderReview = @"~\AdminEntryPortal\UserControl\AdminEntryOrderReview.ascx";
        public static String AdminEntryOrderConfirmation = @"~\AdminEntryPortal\UserControl\AdminEntryOrderConfirmation.ascx";
        public const String ArchivedOrder = @"~/FingerPrintSetUp/UserControl/ArchivedOrder.ascx"; //UAT 3521
        #endregion

        #region MOBILITY PROCESS

        public static String InstituteChangeRequestScreen = @"~\Mobility\UserControl\InstituteChangeRequestScreen.ascx";

        public static String ComplianceItemMappingDetail = @"~\Mobility\UserControl\ComplianceItemMappingDetail.ascx";

        public static String ProgramChangeApproveModify = @"~\Mobility\UserControl\ProgramChangeApproveModify.ascx";

        public static String ApplicantBalancePayment = @"~\Mobility\UserControl\ApplicantBalancePayment.ascx";
        public static String NodeTransitionApprovalQueue = @"~\Mobility\MobilityApprovalQueue.ascx";
        public static String CompliancePkgMappingDependencies = @"~\Mobility\UserControl\CompliancePakageMappingDependencies.ascx";
        public static String ComplianceItemMappingQueue = @"~\Mobility\ComplianceItemMappingQueue.ascx";
        public static String AdminChangeSubscription = @"~\Mobility\AdminChangeSubscription.ascx";

        #endregion

        #region QueueMamagement
        public static String QueueAssignmentControl = @"~\QueueManagement\UserControl\QueueAssignmentControl.ascx";
        public static String BasicAssignmentConfigurationQueueControl = @"~\QueueManagement\QueueAssignmentParameter.ascx";
        public static String SpecializedAssignmentConfigurationQueueControl = @"~\QueueManagement\QueueSpecializedAssignmentParameter.ascx";
        public static String ReconcilliationAssignmentConfigurationQueueControl = @"~\QueueManagement\ReconciliationQueueAssignmentParameter.ascx";
        public static String ReconcilliationQueueAssignmentControl = @"~\QueueManagement\UserControl\ReconcilliationQueueAssignmentControl.ascx";
        #endregion

        public static String NodeTemplates = @"~\Templates\UserControl\DeadlineTemplatesGrid.ascx";
        public static String NodeNotificationTemplates = @"~\Templates\UserControl\NodeNotifications.ascx";

        #region Custom Forms
        public static String ConfigureCustomForm = @"~\BkgSetup\UserControl\ConfigureCustomForm.ascx";

        #endregion

        #region Background SetUp
        public static String ManageServiceAttributeGroup = @"~\BkgSetup\UserControl\ManageServiceAttributeGroup.ascx";
        public static String ManageCustomForm = @"~\BkgSetup\UserControl\ManageBkgServiceCustomForm.ascx";
        public static String ManageMasterService = @"~\BkgSetup\ManageService.ascx";
        public static String VendorAccountSetting = @"~\BkgSetup\UserControl\VendorAccountSettings.ascx";
        public static String VendorAccount = @"~\BkgSetup\ServiceVendors.ascx";
        public static String VendorServices = @"~\BkgSetup\UserControl\VendorServices.ascx";
        public static String VendorServiceMapping = @"~\BkgSetup\VendorServiceMapping.ascx";
        public static String ManageMasterCustomForm = @"~\BkgSetup\ManageCustomForm.ascx";

        #endregion

        #region Manage Service Atrribute Mapping

        public static String SetupMasterServiceAttributeGroup = @"~\BkgSetup\SetupServiceAttributeGroup.ascx";
        public static String MapServiceAttributeToAttributrGroup = @"~\BkgSetup\UserControl\MapServiceAttributeToGroup.ascx";

        #endregion

        #region Vendor Service Mapping

        public static String VendorServiceAttributeMappingControl = @"~\BkgSetup\UserControl\VendorServiceAttributeMapping.ascx";

        #endregion

        #region Background Order Detail

        public static String BackgroundOrderSearchQueue = @"~\BkgOperations\BkgOrderSearchQueue.ascx";

        public static String BackgroundOrderReviewQueue = @"~\BkgOperations\BkgOrderReviewQueue.ascx";

        #endregion

        #region Manage Global Fee Item and Fee Record
        public static String ManageFeeRecord = @"~\BkgSetup\UserControl\ManageFeeRecord.ascx";
        public static String ManageFeeItem = @"~\BkgSetup\ManageFeeItem.ascx";
        #endregion

        #region Manage D & R Document

        public static String DisclosureDocumentFieldMapping = @"~\BkgSetup\UserControl\DAndRAttributeGroupMapping.ascx";
        public static String DisclosureDocument = @"~\BkgSetup\DisclosureDocuments.ascx";
        #endregion

        #region Manage Attribute Documents
        public static String AttributeDocumentFieldMapping = @"~\ComplianceAdministration\UserControl\AttributeDocumentFieldMapping.ascx";
        public static String ManageAttributeDocument = @"~\ComplianceAdministration\ManageAttributeDocuments.ascx";
        #endregion

        #region UAT-1049:Admin Data Entry
        public static String DataEntryViewDetail = @"~\ComplianceOperations\UserControl\DataEntry.ascx";
        public static String DataEntryAssignmentQueue = @"~\ComplianceOperations\DataEntryAssignmentQueue.ascx";
        public static String DataEntryUserWorkQueue = @"~\ComplianceOperations\DataEntryUserWorkQueue.ascx";
        public static String DataEntryQueue = @"~\ComplianceOperations\UserControl\DataEntryQueue.ascx";
        #endregion

        #region PROFILE SHARING
        public static String SharedUserDashboard = @"~\ProfileSharing\ManageInvitationsSharedUser.ascx";
        public static String SharedUserInvitationDetail = @"~\ProfileSharing\UserControl\SharedUserInvitationDetail.ascx";
        public static String AgencyUserDetail = @"~\ProfileSharing\UserControl\AgencyUserDetail.ascx";
        public static String ViewAgencyApplicantShareHistory = @"~\ProfileSharing\ViewAgencyApplicantShareHistory.ascx";
        #endregion

        #region UAT-1188
        public static String RenewalOrderOptions = @"~/ComplianceOperations/UserControl/RenewalOrderOptions.ascx";
        #endregion

        #region UAT-1237 - SharedUserSearchDetails
        public static String SharedUserSearch = @"~/SearchUI/SharedUserSearch.ascx";
        public static String SharedUserSearchDetails = @"~/SearchUI/UserControl/SharedUserSearchDetails.ascx";
        #endregion

        #region UAT-2842-Admin Create Screening order (HR)
        public static String AdminCreateOrderDetails = @"~/BkgOperations/UserControl/AdminCreateOrderDetails.ascx";
        #endregion


        #region UAT-2971:Support Portal
        public static String SupportPortalDetails = @"~/SearchUI/UserControl/SupportPortalDetails.ascx";
        #endregion

        #region UAT-2310:-Tracking Assignment Efficiencies
        public static String TrackingAutoAssignmentConfigurationDetail = @"~/ComplianceOperations/UserControl/TrackingAutoAssignmentConfigurationDetail.ascx";
        public static String ManageTrackingAutoAssignmentConfiguration = @"~/ComplianceOperations/ManageTrackingAutoAssignmentConfiguration.ascx";
        #endregion


        #region UAT-3316
        public static String AgencyUserPermissionTemplateSearch = @"~/ProfileSharing/ManageAgencyUserPermissionTemplate.ascx";
        #endregion

        #region Ticket Centre
        public static String AddTicket = @"~\FingerPrintSetUp\UserControl\AddTicket.ascx";
        public static String TicketingCentre = @"~\FingerPrintSetUp\ManageTickets.ascx";
        #endregion

        #region UAT-4151
        public static String OtherAccountLinking = @"~/IntsofSecurityModel/OtherAccountLinking.ascx";
        public static String OtherAccountLinkingNew = @"~/IntsofSecurityModel/OtherAccountLinkingNew.ascx";
        #endregion

        #region UAT-4248: Instructor Support Portal
        public static String InstructorSupportPortalDetails = @"~/SearchUI/UserControl/InstructorSupportPortalDetail.ascx";
        #endregion
    }


    /// <summary>
    /// Handles search fields.
    /// </summary>
    /// <remarks></remarks>
    public static class EqualSearchFields
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        static EqualSearchFields()
        {
            EqualSearchCollection = new List<String> { "ClientID", "PackageID", "CategoryID", "ApplicantFirstName", "ApplicantLastName", "ProgramID", "DateOfBirth", "ItemLabel",
                "ItemName", "ItemScreenLabel", "AssignedToUserID"};
            //,"PaymentTypeCode" }; Commented for UAT - 916
        }

        /// <summary>
        /// Gets or sets the equal search collection.
        /// </summary>
        /// <value>The equal search collection.</value>
        /// <remarks></remarks>
        public static List<String> EqualSearchCollection
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Handles properties for policy.
    /// </summary>
    /// <remarks></remarks>
    public static class PolicyProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        static PolicyProperties()
        {
            PolicyPropertyCollection = new Dictionary<String, String>
                              {
                                  {"Disabled", "Enabled"},
                                  {"ReadOnly", "ReadOnly"},
                                  {"Deny Delete", "AllowAutomaticDeletes"},
                                  {"Deny Insert", "AllowAutomaticInserts"},
                                  {"Deny Update", "AllowAutomaticUpdates"}
                              };
        }

        /// <summary>
        /// Gets or sets the policy property collection.
        /// </summary>
        /// <value>The policy property collection.</value>
        /// <remarks></remarks>
        public static Dictionary<String, String> PolicyPropertyCollection
        {
            get;
            set;
        }
    }


    /// <summary>
    /// Handles Related Proerties
    /// </summary>
    public static class RelatedProperties
    {
        static RelatedProperties()
        {

            RelatedPropertyValueListForItems = new List<String>()
            {
                "OriginalName",
                "ItemScreenLabel"
            };

            RelatedPropertyCollection = new Dictionary<String, List<String>>
            {
                {"ItemLabel",RelatedPropertyValueListForItems},
            };
        }

        /// <summary>
        /// gets or sets the RelatedProperty collection
        /// </summary>
        public static Dictionary<String, List<String>> RelatedPropertyCollection
        {
            get;
            set;
        }

        /// <summary>
        /// sets the Related Proprties values
        /// </summary>
        private static List<String> RelatedPropertyValueListForItems
        {
            get;
            set;
        }
    }
}
