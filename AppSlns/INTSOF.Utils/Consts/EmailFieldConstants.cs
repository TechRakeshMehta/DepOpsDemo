using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.Utils
{
    public class EmailFieldConstants
    {
        public const String USER_FULL_NAME = "UserFullName";
        public const String ORDER_NO = "OrderNo";
        public const String SUBSCRIPTION_DATE = "SubscriptionDate";
        public const String MESSAGE_FROM_NAME = "MessageFromName";
        public const String ACCOUNT_STATUS = "AccountStatus";
        public const String ORDER_DATE = "OrderDate";
        public const String ORDER_CONTENT = "OrderContent";
        public const String SUBSCRIPTION_NAME = "SubscriptionName";
        public const String SUBSCRIPTION_START_DATE = "SubscriptionStartDate";
        public const String SUBSCRIPTION_END_DATE = "SubscriptionEndDate";
        public const String ORDER_STATUS = "OrderStatus";
        public const String PASSWORD = "Password";
        public const String COMPLIANCE_STATUS = "ComplianceStatus";
        public const String EVENT_START_DATE = "EventStartDate";
        public const String EVENT_END_DATE = "EventEndDate";
        public const String EVENT_NEW_START_DATE = "EventNewStartDate";
        public const String EVENT_NEW_END_DATE = "EventNewEndDate";
        public const String EVENT_NAME = "EventName";
        public const String PROGRAM_NAME = "ProgramName";
        public const String COMPLIANCE_ITEM_NAME = "ComplianceItemName";
        public const String COMPLIANCE_ITEM_EXPIRY_DATE = "ComplianceItemExpiryDate";
        public const String COMPLIANCE_NAME = "ComplianceName";
        public const String COMPLIANCE_EXPIRY_DATE = "ComplianceExpiryDate";
        public const String CREDIT_CARD_DETAILS = "CreditCardDetails";
        public const String MONEY_ORDER_DETAILS = "MoneyOrderDetails";
        public const String MBS_PAYMENT_DETAILS = "MBSPaymentDetails";
        public const String MESSAGE_FROM = "MessageFrom";
        public const String MESSAGE_TO = "MessageTo";
        public const String MESSAGE_CC = "MessageCc";
        public const String DEPARTMENT_NAME = "DepartmentName";
        public const String PACKAGE_NAME = "PackageName";
        public const String PROGRAM_DURATION = "ProgramDuration";
        public const String COST = "Cost";
        public const String PAYMENT_METHOD = "PaymentMethod";
        public const String PAYMENT_RECEIVED_DATE = "PaymentReceivedDate";
        public const String INSTITUTION_URL = "InstitutionUrl";
        public const String SHARED_USER_URL = "SharedUserUrl";
        public const String REFUND_POLICY = "RefundPolicy";
        public const String INSTITUTE_NAME = "InstituteName";
        public const String EXPIRY_DATE = "ExpiryDate";
        public const String DAYS_LEFT_TO_EXPIRE = "DaysLeftToExpire";
        public const String APPLICATION_URL = "ApplicationUrl";
        public const String REJECTION_REASON = "RejectionReason";
        public const String NODE_HIERARCHY = "NodeHierarchy";
        public const String OLD_COMPLIANCE_STATUS = "OldComplianceStatus";
        public const String NEW_COMPLIANCE_STATUS = "NewComplianceStatus";
        public const String COMPLIANCE_ITEM_ID = "ComplianceItemID";
        public const String COMPLIANCE_CATEGORY_ID = "ComplianceCategoryID";
        public const String TENANT_ID = "TenantID";
        public const String CHANGED_HIERARCHY_NODE = "ChangedHierarchyNode";
        public const String AMOUNT_DUE = "Amount";
        public const String CATEGORY_NAME = "CategoryName";
        public const String USER_NAME = "UserName";
        public const String TEMPORARY_PASSWORD = "TemporaryPassword";
        public const String APPLICANT_NAME = "ApplicantName";
        public const String ROLE = "Role";
        public const String SURVEY_MONKEY_LINK = "SurveyMonkeyLink";
        public const String DEADLINE_DATE = "DeadlineDate";
        public const String SES_NODE_NOTIFICATION_MAPPING_ID = "SES_NodeNotificationMappingID";
        public const String NEW_EMAIL_ADDRESS = "NewEmailAddress";
        public const String VERIFICATION_CODE = "VerificationCode";
        public const String ALL_ASSIGNED_ROLE_NAME = "AllAssignedRoleName";
        public const String DUE_DATE = "Duedate";
        public const String BKG_PACKAGE_LIST = "BkgPackageList";
        public const String CC_NUMBER = "CCNumber";
        public const String CC_TYPE = "CCType";
        public const String EDS_HTML_BODY = "EDSHtmlBody";
        public const String COMPLIANCE_CATEGORY_NAME = "ComplianceCategoryName";
        public const String COMPLIANCE_CATEGORY_EXPIRY_DATE = "ComplianceCategoryExpiryDate";
        public const String SERVICE_NAME = "ServiceName";
        public const String SERVICE_FORM_NAME = "ServiceFormName";
        public const String SERVICE_FORM_DISPATCH_DATE = "ServiceFormDispatchDate";
        public const String SERVICE_GROUP_NAME = "ServiceGroupName";
        public const String Order_Number = "OrderNumber";
        public const String PENDING_SERVICE_GROUP_NAME = "PendingServiceGroupName";
        public const String FROM_DATE = "FromDate";
        public const String END_DATE = "EndDate";
        public const String APPLICANT_NAMES_WITH_ORDER_NUMBERS = "ApplicantNamesWithOrderNumbers";
        public const String REPORT_URL = "ReportURL";
        //UAT-1787:Ability to insert checklist link (if configured) for a package in the order confirmation emails
        public const String CHECK_LIST_LINK = "ChecklistLink";

        public const String CURRENT_DATE = "CurrentDate";
        public const String APPLICANT_PROFILE_ADDRESS = "ApplicantProfileAddress";
        public const String CLIENT_ADMIN_CONTACT_NAMES = "ClientAdminContactNames";
        public const String INSTITUTE_ADDRESS = "InstituteAddress";
        public const String INSTITUTE_PHONE_NUMBER = "InstitutePhoneNumber";

        public const String CATEGORY_EXPLANATORY_NOTES = "CategoryExplanatoryNotes";
        public const String CATEGORY_URL_MORE_INFORMATION = "CategoryURLMoreInformation";

        public const String ROTATION_NAME = "RotationName";
        public const String ROTATION_START_DATE = "RotationStartDate";
        public const String ROTATION_END_DATE = "RotationEndDate";
        public const String ROTATION_DETAILS = "RotationDetails";
        public const String CUSTOMATTRIBUTES = "CustomAttributes";


        public const String CONTRACT_NAME = "ContractName";
        public const String CONTRACT_START_DATE = "ContractStartDate";
        public const String CONTRACT_END_DATE = "ContractEndDate";
        public const String CONTRACT_EXPIRY_DATE = "ContractExpiryDate";


        public const String AGENCY_NAME = "AgencyName";
        public const String SHARED_APPLICANT_DATA = "SharedApplicantData";

        /// <summary>
        /// Place holder property name for Document Rejected by the Data Entry admin. 
        /// </summary>
        public const String ADMIN_DATA_ENTRY_REJECTED_DOCUMENT_NAME = "DocumentName";
        public const String INVITATION_GROUP_ID = "InvitationGroupId";
        public const String INVITEE_DETAIL = "InviteeDetail";
        public const String ITEM_NAME = "ItemName";
        public const String ITEM_STATUS = "ItemStatus";
        public const String CONTRACT_OR_SITE_NAME = "ContractOrSiteName";
        public const String SHARED_STUDENT_NAMES = "SharedStudentNames";
        public const String CATEGORY_CPF_LINK = "cpflink";


        public const String RECEIVER_ORGANIZATION_USER_ID = "ReceiverOrganizationUserID";
        //UAT-2172 Item rejection email placeholder to show if adb admin or client admin did the rejection
        public const String ADMINISTRATOR = "Administrator";

        //UAT-2370 : Supplement SSN Processing updates
        public const String SSN = "MaskedSSN";
        public const String VENDOR_ORDER_NUMBER = "ClearStarOrderNumber";
        //UAT-2538
        public const String INVITATION_STATUS = "InvitationStatus";
        public const String STUDENT_LAST = "StudentLastName";
        public const String STUDENT_FIRST = "StudentFirstName";
        public const String AGENCY_USER_NAME = "AgencyUserName";

        //UAT-2556:Separate email template for student shares that used agency dropdown from those that do not. 
        public const String SELECTED_PACKAGE_DATA = "SelectedPackageData";
        public const String CUSTOM_MESSAGE = "CustomMessage";

        //UAT-2628
        public const String DOCUMENT_NAME = "DocumentName";

        //UAT-2753
        public const String STATUS = "Status";

        //UAT-2905
        public const String REQUIREMENT_ITEM_NAME = "RequirementItemName";
        public const String ROTATION_PACKAGE_NAME = "RotationPackageName";

        public const String APPLICANT_UPDATED_REQUIREMENTS = "ApplicantUpdatedRequirements";
        //UAT-3068
        public const String OTP = "Otp";
        //UAT-3108
        public const String MODIFIED_BY_NAME = "ModifiedByName";
        public const String COMPLIO_ID = "ComplioId";
        public const String ROTATION_FIELD_CHANGES = "RotationFieldChanges";
        //UAT-3222
        public const String STUDENT_NAMES = "StudentNames";
        //UAT-3485
        public const String REQUIREMENT_ITEM_EXPIRY_DATE = "RequirementItemExpiryDate";

        public const String ITEM_CATEGORY_NAME = "ItemCategoryName";
        //UAT-3466
        public const String NON_COMPLIANT_CATEGORY = "NonCompliantCategory";

        public const String COMPLIANCE_REQUIRED_DATE = "ComplianceRequiredDate";
        //UAT-3541
        public const String LOCATION_LINK = "LocationLink";
        public const String LOCATION_DESCRIPTION = "LocationDescription";
        //UAT-3669
        public const String MAX_IMPACT_COUNT = "MaxImpactCount";
        public const String Impact_Count = "ImpactCount";
        public const String Institution_With_Max_Impact = "InstitutionWithMaxImpact";

        //UAT-3851
        public const String CBI_SUCCESS_STATUS = "CBISuccessStatus";
        public const String CBI_PCN = "CBIPCN";

        //UAT-3783
        public const String LOCATION_IMAGE_LINK = "LocationImagesLink";
        //UAT-3704
        public const String AGENCY_HIERARCHY_ID = "AgencyHierarchyID";        
        //UAT-4201
        public const String ACTIVATION_LINK = "ActivationLink";

        //UAT-4398       
        public const String ROTATION_MEMBERS_DETAILS = "RotationMembersDetails";

        // Send Notification Draft Order to Admin

        public const String Order_ID = "OrderID";

        //
        public const String SEND_INVITE_CREATED_DATE = "SendInviteCreatedDate";
        public const String ORDER_COMPLETED_DATE = "OrderCompletedDate";
        public const String Applicant_Invite_URL = "ApplicantInviteURL";
        // UAT - 4400
        public const String Email_Address = "EmailAddress";
        public const String School_Admin = "SchoolAdmin";
        public const String School_Admin_Email = "SchoolAdminEmail";
        public const String Tracking_Number = "TrackingNumber";
        public const String Ship_Date = "ShipDate";
        public const String Shipping_Method = "ShippingMethod";
        public const String Shipping_Address = "ShippingAddress";

    }
}
