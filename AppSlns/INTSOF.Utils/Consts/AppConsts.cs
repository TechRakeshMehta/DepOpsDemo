#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  AppConsts.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils
{
    /// <summary>
    /// Represents SystemX constants
    /// </summary>
    public static class AppConsts
    {
        #region AppConsts Constants

        /// <summary>
        /// Constant for organization user id.
        /// </summary>
        public const String SESSION_ORG_USERID_KEY = "orgUserId";

        /// <summary>
        /// Constant for Session Institution Data.
        /// </summary>
        public const String SESSION_KEY_EMAIL_VERIFICATION = "EmailVerificationCode";

        /// <summary>
        /// Constant for Session Institution Data.
        /// </summary>
        public const String SESSION_KEY_INSTITUTIONDATA = "CurrentInstitutionData";

        /// <summary>
        /// Constant for Session Institution Data.
        /// </summary>
        public const String SESSION_KEY_VERIFICATIONQUEUE = "Session_Key_VerificationQueue";

        /// <summary>
        /// Constant for Session Search Data.
        /// </summary>
        public const String SESSION_KEY_SEARCHQUEUE = "Session_Key_SearchQueue";

        /// <summary>
        /// Constant for Session.
        /// </summary>
        public const String SESSION_QUEUE_TYPE_KEY = "QueueType";

        /// <summary>
        /// Constant for Session Data Entry Queue data
        /// </summary>
        public const String SESSION_KEY_DATA_ENTRY_QUEUE = "Session_Key_Data_Entry_Queue";

        /// <summary>
        /// Constant for INVALID_DOCUMENT_SIZE.
        /// </summary>
        public const String INVALID_DOCUMENT_SIZE = "InvalidSize";

        /// <summary>
        /// Constant for INVALID_FILE_NAME_LENGTH.
        /// </summary>
        public const String INVALID_FILE_NAME_LENGTH = "InvalidFileNameLength";


        /// <summary>
        /// Constant for SESSION_SYSX_USER_KEY.
        /// </summary>
        public const String SESSION_SYSX_USER_KEY = "orgUserId";

        /// <summary>
        /// Constant for SESSION_SYSX_BLOCKID_KEY.
        /// </summary>
        public const String SESSION_SYSX_BLOCKID_KEY = "SysXBlockId";

        /// <summary>
        /// Constant for SESSION_SYSX_BLOCK_NAME_KEY.
        /// </summary>
        public const String SESSION_SYSX_BLOCK_NAME_KEY = "SysXBlockName";

        /// <summary>
        /// Constant for COMBOBOX_ITEM_SELECT.
        /// </summary>
        public const String COMBOBOX_ITEM_SELECT = "--SELECT--";

        /// <summary>
        /// constant for Shared User Grid Source -- User clicked on detail link from which grid
        /// </summary>
        public const string SESSION_SHARED_USER_GRID_SOURCE = "SharedUserGridSource";

        /// <summary>
        /// constant for Shared User Grid Source -- User clicked on detail link from which grid
        /// </summary>
        public const string SESSION_SHARED_USER_GRID_SOURCE_GridPaging = "SharedUserGridPaging";

        /// <summary>
        /// constant for ViewDetail on InvitationDetails
        /// </summary>
        public const string SESSION_SHARED_USER_VIEWDETAIL = "SharedUserViewDetail";

        /// <summary>
        /// Constant for Session Redirect To Requirement Share.
        /// </summary>
        public const string SESSION_TENANTID_REQUIREMENT_SHARE = "TenantIdRequirementShare";


        /// <summary>
        /// UAT-2998 Constant for Session Redirect To Student Rotation Share.
        /// </summary>
        public const string SESSION_TENANTID_ROTATION_SHARE = "TenantIdStudentRotationShare";

        /// <summary>
        /// UAT-2998 Constant for Session Redirect To View Agency Applicant Status.
        /// </summary>
        public const string SESSION_IS_RETURN_FROM_AGENCY_APPLICANT = "IsReturnAgencyApplicantShare";
        /// <summary>
        /// Constant for Session Redirect To Requirement Share.
        /// </summary>
        public const string SESSION_IS_REQUIREMENT_SHARE = "IsRequirementShareRedirect";
        /// <summary>
        /// Constant for Session Institution Data.
        /// </summary>
        public const String SESSION_KEY_AGENCY_HIERARCHY_GRID = "Session_Key_AgencyHierarchyGrid";

        /// <summary>
        /// Constant for ANY.
        /// </summary>
        public const String ANY = "ANY";

        /// <summary>
        /// Constant for COMBOBOX_ITEM_ANY.
        /// </summary>
        public const String COMBOBOX_ITEM_ANY = "--ANY--";

        /// <summary>
        /// Constant for dashboard path.
        /// </summary>
        public const String SYSX_DASHBOARD = "~/Main/Default.aspx";

        /// <summary>
        /// Constant for true.
        /// </summary>
        public const String TRUE = "true";

        /// <summary>
        /// Constant for false.
        /// </summary>
        public const String FALSE = "false";

        #region Numbers

        /// <summary>
        /// Constant for one.
        /// </summary>
        public const Int32 MINUS_ONE = -1;

        public const Int32 MINUS_TWO = -2;

        public const Int32 MINUS_THREE = -3;
        public const Int32 MINUS_FOUR = -4;
        public const Int32 MINUS_FIVE = -5;
        public const Int32 MINUS_SIX = -6;
        public const Int32 MINUS_SEVEN = -7;
        public const Int32 MINUS_EIGHT = -8;
        public const Int32 MINUS_NINE = -9;
        /// <summary>
        /// Constant for numeric 0 (zero).
        /// </summary>
        public const Int32 NONE = 0;
        /// <summary>
        /// Constant for Zero.
        /// </summary>
        public const String ZERO = "0";

        /// <summary>
        /// Constant for one.
        /// </summary>
        public const Int32 ONE = 1;

        ///// <summary>
        ///// Constant for two.
        ///// </summary>
        public const Int32 TWO = 2;

        /// <summary>
        /// Constant for THREE
        /// </summary>
        public const Int32 THREE = 3;

        /// <summary>
        /// Constant for four.
        /// </summary>
        public const Int32 FOUR = 4;

        /// <summary>
        /// Constant for five.
        /// </summary>
        public const Int32 FIVE = 5;

        ///// Constant for six.
        ///// </summary>
        public const Int32 SIX = 6;

        ///// <summary>
        ///// Constant for seven.
        ///// </summary>
        public const Int32 SEVEN = 7;

        ///// <summary>
        ///// Constant for eight.
        ///// </summary>
        public const Int32 EIGHT = 8;
        /// <summary>
        /// Constant for numeric ten.
        /// </summary>
        public const Int32 NINE = 9;

        /// <summary>
        /// Constant for numeric ten.
        /// </summary>
        public const Int32 TEN = 10;

        ///// <summary>
        ///// ELEVEN
        ///// </summary>
        public const Int32 ELEVEN = 11;

        /// <summary>
        /// Constant for numeric twelve.
        /// </summary>
        public const Int32 TWELVE = 12;

        /// <summary>
        /// Constant for numeric thirteen.
        /// </summary>
        public const Int32 THIRTEEN = 13;

        ///// Constant for fourteen.
        ///// </summary>
        public const Int32 FOURTEEN = 14;

        /// <summary>
        /// Constant for numeric value fifteen.
        /// </summary>
        public const Int32 FIFTEEN = 15;

        ///// <summary>
        ///// Constant for numeric value sixteen.
        ///// </summary>
        public const Int32 SIXTEEN = 16;

        ///// <summary>
        ///// Constant for numeric Seventeen.
        ///// </summary>
        public const Int32 SEVENTEEN = 17;

        /// <summary>
        /// Constant for numeric Eighteen
        /// </summary>
        public const Int32 EIGHTEEN = 18;

        /// <summary>
        /// Constant for numeric Nineteen
        /// </summary>
        public const Int32 NINETEEN = 19;

        /// <summary>
        /// Constant for numeric Twenty
        /// </summary>
        public const Int32 TWENTY = 20;

        /// <summary>
        /// Constant for numeric Twentyone
        /// </summary>
        public const Int32 TWENTYONE = 21;

        // <summary>
        //  Constant for numeric Twentytwo
        // </summary>
        public const Int32 TWENTYTWO = 22;

        /// <summary>
        /// Constant for numeric Twentytwo
        /// </summary>
        public const Int32 TWENTYTHREE = 23;

        ///// <summary>
        ///// Constant for numeric value Twenty four.
        ///// </summary>
        public const Int32 TWENTYFOUR = 24;
        /// <summary>
        /// Constant for numeric Twentytwo
        /// </summary>
        public const Int32 TWENTYFIVE = 25;

        /// <summary>
        /// Constant for numeric Twentytwo
        /// </summary>
        public const Int32 TWENTYSIX = 26;

        /// <summary>
        /// Constant for numeric Twentytwo
        /// </summary>
        public const Int32 TWENTYSEVEN = 27;

        /// <summary>
        /// Constant for numeric Twentytwo
        /// </summary>
        public const Int32 TWENTYEIGHT = 28;

        /// <summary>
        /// Constant for numeric Twentytwo
        /// </summary>
        public const Int32 TWENTYNINE = 29;

        ///// <summary>
        ///// Constant for thirty.
        ///// </summary>
        public const Int32 THIRTY = 30;

        ///// <summary>
        ///// Constant for thirty one.
        ///// </summary>
        public const Int32 THIRTY_ONE = 31;

        ///// <summary>
        ///// Constant for thirty two.
        ///// </summary>
        public const Int32 THIRTY_TWO = 32;

        /// <summary>
        /// Constant for thirty three.
        ///// </summary>
        public const Int32 THIRTY_THREE = 33;

        ///// <summary>
        ///// Constant for thirty four.
        ///// </summary>
        public const Int32 THIRTY_FOUR = 34;

        ///// <summary>
        ///// Constant for thirty five.
        ///// </summary>
        public const Int32 THIRTY_FIVE = 35;

        ///// <summary>
        ///// Constant for thirty six.
        ///// </summary>
        public const Int32 THIRTY_SIX = 36;

        ///// <summary>
        ///// Constant for thirty seven.
        ///// </summary>
        public const Int32 THIRTY_SEVEN = 37;

        ///// <summary>
        ///// Constant for thirty eight.
        ///// </summary>
        public const Int32 THIRTY_EIGHT = 38;

        ///// <summary>
        ///// Constant for thirty nine.
        ///// </summary>
        public const Int32 THIRTY_NINE = 39;
        ///// <summary>
        ///// Constant for numeric forty.
        ///// </summary>
        public const Int32 FORTY = 40;

        ///// <summary>
        ///// Constant for forty one.
        ///// </summary>
        public const Int32 FORTY_ONE = 41;

        ///// <summary>
        ///// Constant for forty two.
        ///// </summary>
        public const Int32 FORTY_TWO = 42;

        ///// <summary>
        ///// Constant for forty three.
        ///// </summary>
        public const Int32 FORTY_THREE = 43;

        ///// <summary>
        ///// Constant for forty four.
        ///// </summary>
        public const Int32 FORTY_FOUR = 44;

        ///// <summary>
        ///// Constant for forty five.
        ///// </summary>
        public const Int32 FORTY_FIVE = 45;

        ///// <summary>
        ///// Constant for forty six.
        ///// </summary>
        public const Int32 FORTY_SIX = 46;

        ///// <summary>
        ///// Constant for forty seven.
        ///// </summary>
        public const Int32 FORTY_SEVEN = 47;

        ///// <summary>
        ///// Constant for forty eight.
        ///// </summary>
        public const Int32 FORTY_EIGHT = 48;

        ///// <summary>
        ///// Constant for forty nine.
        ///// </summary>
        public const Int32 FORTY_NINE = 49;

        ///// <summary>
        ///// Constant for numeric fifty.
        ///// </summary>
        public const Int32 FIFTY = 50;

        ///// <summary>
        ///// Constant for fifty one.
        ///// </summary>
        public const Int32 FIFTY_ONE = 51;

        ///// <summary>
        ///// Constant for fifty two.
        ///// </summary>
        public const Int32 FIFTY_TWO = 52;

        ///// <summary>
        ///// Constant for fifty three.
        ///// </summary>
        public const Int32 FIFTY_THREE = 53;

        ///// <summary>
        ///// Constant for fifty four.
        ///// </summary>
        public const Int32 FIFTY_FOUR = 54;

        ///// <summary>
        ///// Constant for fifty five.
        ///// </summary>
        public const Int32 FIFTY_FIVE = 55;

        ///// <summary>
        ///// Constant for fifty six.
        ///// </summary>
        public const Int32 FIFTY_SIX = 56;

        ///// <summary>
        ///// Constant for fifty seven.
        ///// </summary>
        public const Int32 FIFTY_SEVEN = 57;

        ///// <summary>
        ///// Constant for fifty eight.
        ///// </summary>
        public const Int32 FIFTY_EIGHT = 58;

        ///// <summary>
        ///// Constant for fifty nine.
        ///// </summary>
        public const Int32 FIFTY_NINE = 59;

        ///// <summary>
        ///// Constant for numeric sixty.
        ///// </summary>
        public const Int32 SIXTY = 60;

        ///// <summary>
        ///// Constant for sixty one.
        ///// </summary>
        public const Int32 SIXTY_ONE = 61;

        ///// <summary>
        ///// Constant for sixty two.
        ///// </summary>
        public const Int32 SIXTY_TWO = 62;

        ///// <summary>
        ///// Constant for sixty three.
        ///// </summary>
        public const Int32 SIXTY_THREE = 63;

        ///// <summary>
        ///// Constant for sixty four.
        ///// </summary>
        public const Int32 SIXTY_FOUR = 64;

        ///// <summary>
        ///// Constant for sixty five.
        ///// </summary>
        public const Int32 SIXTY_FIVE = 65;

        /// <summary>
        /// Constant for numeric valueSIXTYSIX
        /// </summary>
        public const Int32 SIXTYSIX = 66;

        /// <summary>
        /// Constant for numeric value SIXTYSEVEN
        /// </summary>
        public const Int32 SIXTYSEVEN = 67;
        /// <summary>
        /// Constant for numeric value NINETYNINE
        /// </summary>
        public const Int32 NINETYNINE = 99;

        /// <summary>
        /// Constant for numeric value HUNDRED
        /// </summary>
        public const Int32 HUNDRED = 100;

        /// <summary>
        /// Constant for numeric value TWOHUNDREDTWENTYTWO
        /// </summary>
        public const Int32 TWOHUNDREDTWENTYTWO = 220;

        ///// <summary>
        ///// Constant for Signing Limit 500
        ///// </summary>
        public const Decimal FIVE_HUNDRED = 500;

        ///// <summary>
        ///// Constant for Signing Limit 1000
        ///// </summary>
        public const Decimal THOUSAND = 1000;

        ///// <summary>
        ///// Constant for Signing Limit 2000
        ///// </summary>
        public const Decimal TWO_THOUSAND = 2000;

        ///// <summary>
        ///// Constant for Signing Limit 5000
        ///// </summary>
        public const Decimal FIVE_THOUSAND = 5000;

        ///// <summary>
        ///// Constant for Signing Limit 12500
        ///// </summary>
        public const Decimal TWELVE_THOUSAND_FIVE_HUNDRED = 12500;

        /// <summary>
        /// Constant for Signing Limit 15000
        /// </summary>
        public const Decimal FIFTEEN_THOUSAND = 15000;

        ///// <summary>
        ///// Constant for Signing Limit 17500
        ///// </summary>
        public const Decimal SEVENTEEN_THOUSAND = 17000;

        ///// <summary>
        ///// Constant for Signing Limit 25000
        ///// </summary>
        public const Decimal TWENTYFIVE_THOUSAND = 25000;


        /// <summary>
        /// Constant for string one.
        /// </summary>
        public const String STR_ONE = "1";

        ///// <summary>
        ///// Constant for string two.
        ///// </summary>
        public const String STR_TWO = "2";

        ///// <summary>
        ///// Constant for string three.
        ///// </summary>
        public const String STR_THREE = "3";

        ///// <summary>
        ///// Constant for string four.
        ///// </summary>
        public const String STR_FOUR = "4";

        ///// <summary>
        ///// Constant for string Five.
        ///// </summary>
        public const String STR_FIVE = "5";

        ///// <summary>
        ///// Constant for string Six.
        ///// </summary>
        public const String STR_SIX = "6";

        ///// <summary>
        ///// Constant for string Seven.
        ///// </summary>
        public const String STR_SEVEN = "7";

        ///// <summary>
        ///// Constant for string Eight.
        ///// </summary>
        public const String STR_EIGHT = "8";

        ///// <summary>
        ///// Constant for string Eight.
        ///// </summary>
        public const String STR_NINE = "9";

        ///// <summary>
        ///// Constant for string ten.
        ///// </summary>
        public const String STR_TEN = "10";

        ///// <summary>
        ///// Constant for string Elleven.
        ///// </summary>
        public const String STR_ELLEVEN = "11";

        ///// <summary>
        ///// Constant for string Twelve.
        ///// </summary>
        public const String STR_TWELVE = "12";

        ///// <summary>
        ///// Constant for string Thirteen.
        ///// </summary>
        public const String STR_THIRTEEN = "13";

        ///// <summary>
        ///// Constant for string Fourteen.
        ///// </summary>
        public const String STR_FOURTEEN = "14";

        ///// <summary>
        ///// Constant for string Fifteen.
        ///// </summary>
        public const String STR_FIFTEEN = "15";

        #endregion

        public const String SESSION_BUSINESS_CHANNEL_TYPE = "BusinessChannelType";

        public const Int16 COMPLIO_BUSINESS_CHANNEL_TYPE = 1;
        public const Int16 AMS_BUSINESS_CHANNEL_TYPE = 2;

        /// <summary>
        /// Constant for COMBOBOX_ITEM_NONE
        /// </summary>
        public const String COMBOBOX_ITEM_NONE = "NONE";

        /// <summary>
        /// Constant for QUERYSTRING_ARGUMENT
        /// </summary>
        public const String QUERYSTRING_ARGUMENT = "args";

        /// <summary>
        /// Constant for IS_POLICY_ENABLE
        /// </summary>
        public const String IS_POLICY_ENABLE = "IsPolicyEnable";

        /// <summary>
        /// Constant for UCID
        /// </summary>
        public const String UCID = "ucid";

        /// <summary>
        /// Constant for UC_DYNAMIC_CONTROL
        /// </summary>
        public const String UC_DYNAMIC_CONTROL = "ucDynamicControl";

        /// <summary>
        /// Constant for SESSION_EXPIRED
        /// </summary>
        public const String SESSION_EXPIRED = "SessionExp";

        /// <summary>
        /// Constant for CHILD
        /// </summary>
        public const String CHILD = "Child";

        /// <summary>
        /// Home Page
        /// </summary>
        public static String HomePage = "/Main/Default.aspx";

        /// <summary>
        /// Home page link
        /// </summary>
        public static String Home = "Home";

        public static String EMPLOYE_CURRENT_END_DATE = "Current";

        ///// <summary>
        ///// Constant for NewAccount.
        ///// </summary>
        public const String NewAccount = "NewAccount";
        public const String NewAccountForAdmin = "NewAccountForAdmins";
        public const String NewApplicantAccount = "NewApplicantAccount";

        public const String ApplicantEmailAddressChange = "ApplicantEmailAddressChange";

        public const String ApplicantAlertEmailAddressChange = "ApplicantAlertEmailAddressChange";

        public const String VerificationCodeForEmailChange = "VerificationCodeForEmailChange";

        public const String ApplicantInstitutionChange = "ApplicantInstitutionChange";
        public const String UserName = "UserName";

        ///// <summary>
        ///// Constant for PasswordReset.
        ///// </summary>
        public const String PasswordReset = "PasswordReset";
        ///// <summary>
        ///// Constant for ForgotPasswordReset.
        ///// </summary>
        public const String ForgotPasswordReset = "ForgotPasswordReset";

        ///// <summary>
        ///// Constant for ForgotPasswordReset.
        ///// </summary>
        public const String ForgotUserNameReset = "ForgotUserNameReset";

        ///// <summary>
        ///// Constant for VerficationCode.
        ///// </summary>
        public const String VerficationCode = "VerficationCode";

        ///// <summary>
        ///// Constant for VerficationCode.
        ///// </summary>
        public const String VerficationCodeUserName = "VerficationCodeUserName";
        ///// <summary>
        ///// Constant for RoleUpdate.
        ///// </summary>
        public const String RoleUpdate = "RoleUpdate";

        /// <summary>
        /// Email Priority Constants
        /// </summary>       
        public const String Normal = "Normal";
        public const String High = "High";
        public const String Low = "Low";

        ///// <summary>
        ///// Constant for Address.
        ///// </summary>
        public const String ADDRESS = "Address";

        ///// <summary>
        ///// Constant for CITY .
        ///// </summary>
        public const String CITY = "City";

        ///// <summary>
        ///// Constant for States .
        ///// </summary>
        public const String STATE = "State";

        ///// <summary>
        ///// Constant for ZipCode .
        ///// </summary>
        public const String ZIPCODE = "ZipCode";
        ///// <summary>
        ///// Constant for TransactionId .
        ///// </summary>
        public const String TransactionID = "TransactionId";

        ///// <summary>
        ///// Constant for alias .
        ///// </summary>
        public const String ALIAS = "Alias";

        /// <summary>
        /// THEMESETTING_CONTROL_NAME
        /// </summary>
        public const String THEMESETTING = "ThemeSetteng";

        /// <summary>
        /// THEMESETTING_CONTROL_NAME
        /// </summary>
        public const String THEMESETTING_CONTROL_NAME = "UserControl\\ThemeSettings.ascx";

        /// <summary>
        /// DASHBOARD_CONTROL_NAME
        /// </summary>
        // public const String DASHBOARD_CONTROL_NAME = "~/dashboard/internaldashboard.ascx";


        /// <summary>
        /// DASHBOARD_CONTROL_NAME
        /// </summary>
        public const String DASHBOARD_PAGE_NAME = "~/Dashboard/Default.aspx";

        /// <summary>
        /// DASHBOARD_CONTROL_NAME
        /// </summary>
        //  public const String APPLICANT_DASHBOARD_CONTROL_NAME = "~/dashboard/externaldashboard.ascx";

        /// <summary>
        /// LANDING_PAGE_CONTROL_NAME
        /// </summary>
        public const String APPLICANT_LANDING_PAGE_CONTROL_NAME = "~/Main/ApplicantLandingPage.ascx";

        /// <summary>
        /// LANDING_PAGE_CONTROL_NAME
        /// </summary>
        public const String AMS_APPLICANT_LANDING_PAGE_CONTROL_NAME = "~/Main/AMSApplicantLandingPage.ascx";

        /// <summary>
        /// DASHBOARD_CONTROL_NAME
        /// </summary>
        public const String APPLICANT_MAIN_PAGE_NAME = "~/Main/Default.aspx";

        /// <summary>
        /// DASHBOARD_CONTROL_NAME
        /// </summary>
        public const String ADMIN_EDITPROFILE_PAGE_NAME = @"~\IntsofSecurityModel\UserControl\AdminEditProfile.ascx";

        /// <summary>
        /// DASHBOARD_CONTROL_NAME
        /// </summary>
        public const String EDITPROFILE_PAGE_NAME = @"~\ApplicantModule\UserControl\EditProfile.ascx";

        public const String FINGER_PRINTDATA_CONTROL = @"~/FingerPrintSetUp/UserControl/FingerPrintDataControl.ascx";


        /// <summary>
        /// NEW_REGISTERED_USER
        /// </summary>
        public const String NEW_REGISTERED_USER = "newUser";

        public const Int32 SUPER_ADMIN_TENANT_ID = 1;

        public const Int32 DEFAULT_PAZE_SIZE = 50;
        /// <summary>
        /// DELIMITER
        /// </summary>
        public const String DELIMITER = "Delimiter";

        /// <summary>
        /// 
        /// </summary>
        public const String CLIENT_WEBSITE_IMAGES = "websiteImages";

        public const String DISCLAIMER_ACCEPTED = "DisclaimerAccepted";
        public const String DISCLOSURE_ACCEPTED = "DisclosureAccepted";
        #region UAT-1560
        public const String REQUIRED_DOCUMENTATION_ACCEPTED = "RequiredDocumentationAccepted";
        #endregion

        public const String ONLINE_PAYMENT_SUBMITTED = "OnlinePaymentSubmitted";

        public const String ORDER_CONFIRMATION = "OrderConfirmation";

        public const String SYSTEM_COMMUNICATION_USER_ID = "SytemCommunicationUserId";

        public const Int32 SYSTEM_COMMUNICATION_USER_VALUE = 2147483100;

        public const String BACKGROUND_PROCESS_USER_ID = "BackgroundProcessUserId";

        public const String BKG_ORDER_SERVICE_USER_ID = "BkgOrderServiceUserId";

        public const Int32 BACKGROUND_PROCESS_USER_VALUE = 2147483200;

        public const Int32 BKG_ORDER_SERVICE_USER_ID_VALUE = 2147483600;

        public const String BACKGROUND_PROCESS_USER_NAME = "Test User";

        public const String BACKGROUND_PROCESS_USER_EMAIL = "testuser@test.com";

        public const String AUTHORIZE_DOT_NET_USER_ID = "AuthorizeDotNetUserId";

        public const Int32 AUTHORIZE_DOT_NET_USER_VALUE = 2147483300;

        public const String PAYPAL_PDT_USER_ID = "PaypalPDTUserId";

        public const Int32 PAYPAL_PDT_USER_VALUE = 2147483400;

        public const String PAYPAL_IPN_USER_ID = "PaypalIPNUserId";

        public const Int32 PAYPAL_IPN_USER_VALUE = 2147483500;

        public const String APPLICATION_URL = "ApplicationUrl";

        public const String DASHBOARD_MARKUP_KEY = "markup";

        public const String DASHBOARD_WIDGETSTATE_KEY = "widgetstate";

        public const String DASHBOARD_LAYOUTPATH_ID = "pathid";

        public const String DATA_FEED_SERVICE_USER_ID = "DataFeedServiceUserId";

        public const Int32 DATA_FEED_SERVICE_USER_ID_VALUE = 2147480000;

        /// <summary>
        /// UAT-2370 : Supplement SSN Processing updates
        /// </summary>
        public const String EMAILID_FOR_SSN_EXCEPTION = "OPSAdminEmailID";

        /// <summary>
        /// UAT-3669
        /// </summary>
        public const String IT_Helpdesk_EmailId = "IT Helpdesk EmailId";

        /// <summary>
        /// Constant for sql data provider
        /// </summary>
        public const String SQL_DATA_PROVIDER = "System.Data.SqlClient";

        /// <summary>
        /// Constant for tenant entity metadata 
        /// </summary>
        public const String TENANT_ENTITY_METADATA = @"res://*/ClientEntity.ADBClientEntity.csdl|res://*/ClientEntity.ADBClientEntity.ssdl|res://*/ClientEntity.ADBClientEntity.msl";

        /// <summary>
        /// Constant for Messaging entity metadata 
        /// </summary>
        public const String MESSAGING_ENTITY_METADATA = @"res://*/ADBMessageQueue.csdl|res://*/ADBMessageQueue.ssdl|res://*/ADBMessageQueue.msl";


        public const String BUSINESS_CHANNEL_EVENT_COMPLIO = "Complio";
        public const String BUSINESS_CHANNEL_EVENT_AMS = "AMS";
        public const String BUSINESS_CHANNEL_EVENT_COMMON = "AMSComplio";

        public const Int32 COUNTRY_USA_ID = 233;

        public const String BIRTH_COUNTRY_USA = "UNITED STATES of AMERICA - STATE"; //UAT_3821
        //UAT-2833
        public const String ALL_COUNTRIES_TEXT = "All Countries";
        #endregion

        #region Supplement Order Flow

        public const String QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID = "OrderPackageSvcGrpID";
        public const String QUERYSTRING_PARENT_SCREEN_NAME = "ParentScreenName";
        public const String QUERYSTRING_SUPPLEMENT_AUTOMATION_STATUS_ID = "SupplementAutomationStatusID";

        #endregion

        #region Alert

        public const String BOOLEAN = "Boolean";
        public const String INT = "Int32";
        public const String DATE_TIME = "DateTime";
        public const String EQUALS = "=";
        public const String NOT_EQUALS = "!=";
        public const String STRING = "String";
        public const String DECIMAL = "Decimal";
        /// <summary>
        /// Constant for HYPHEN.
        /// </summary>
        public const Char HYPHEN = '-';

        /// <summary>
        /// Constant for HYPHEN.
        /// </summary>
        public const String BACKSLASH = @"\";

        /// <summary>
        /// Constant for COLON.
        /// </summary>
        public const Char COLON = ':';

        public const String YES = "Yes";

        public const String NO = "No";

        #endregion

        #region Asset Legend

        /// <summary>
        /// String Constant for "AssetLegend".
        /// </summary>
        public const String ASSET_LEGEND = "AssetLegend";

        /// <summary>
        /// String Constant for "AssetLegendContext".
        /// </summary>
        public const String ASSET_LEGEND_CONTEXT = "AssetLegendContext";

        /// <summary>
        /// String Constant for "AssetID".
        /// </summary>
        public const String ASSET_LEGEND_ASSETID = "AssetID";

        /// <summary>
        /// String Constant for "LoanID".
        /// </summary>
        public const String ASSET_LEGEND_LOANID = "LoanID";

        /// <summary>
        /// String Constant for "AssetUnitID".
        /// </summary>
        public const String ASSET_LEGEND_ASSETUNITID = "AssetUnitID";

        #endregion

        #region Communication Center Query String & ViewState Constants

        public const String COMMUNICATION_TYPE_QUERY_STRING = "cType";

        public const String APPLICANT_TYPE_QUERY_STRING = "applicantId";

        public const String APPLICANT_DESCRIPTON_TYPE_QUERY_STRING = "applicantName";

        public const String ACTION_TYPE_QUERY_STRING = "action";

        public const String MESSAGE_ID_QUERY_STRING = "messageID";

        public const String QUEUE_TYPE_QUERY_STRING = "queueType";

        public const String CURRENT_USER_ID_QUERY_STRING = "currentUserId";

        public const String IS_HIGH_IMPORTANCE_QUERY_STRING = "isImportant";

        public const String FILE_NAME_QUERY_STRING = "fileName";

        public const String ORIGINAL_FILE_NAME_QUERY_STRING = "originalFileName";

        public const String RULE_ID__VIEW_STATE = "MessageRuleID";

        public const String TEMPLATE_ID__VIEW_STATE = "CommunicationTemplateId";

        public const String FROM_ID_QUERY_STRING = "From";

        public const String DATE_FORMAt_QUERY_STRING = "Date";

        public const String SYSTEM_COMMUNICATION_ID_QUERY_STRING = "sysCommId";

        public const String IS_DASHBOARD_MESSAGE_QUERY_STRING = "isDashboardMessage";

        public const String SCREEN_NAME_QUERY_STRING = "SName";

        public const String PORTFOLIO_SEARCH = "PortfolioSearch";

        public const String COMPLIANCE_SEARCH = "ComplianceSearch";

        public const String BACKGROUND_ORDER_SEARCH = "BkgOrderSearchQueue";

        public const String ADMIN_DATA_ITEM_SEARCH = "AdminDataItemSearch";

        public const String STUDENT_BUCKET_ASSIGNEMNT = "StudentBucketAssignment";

        public const String ROTATION_DETAIL_FORM = "RotationDetailForm";

        //UAT-4179
        public const String COMMUNICATION_CENTER = "CommunicationCenter";

        public const String ROTATION_STUDENT_DETAIL = "RotationStudentDetail"; //UAT-3098
        public const String ROTATION_STUDENT_SEARCH = "RotationStudentSearch";
        public const String SCHOOL_REPRESENTATIVE_DETAILS = "SchoolRepresentativeDetails";//UAT-3319
        public const String REQUIREMENT_NONCOMPLIANT_SEARCH = "RequirementNonCompliantSearch";//UAT-3319

        /// <summary>
        /// Constant for ParentQueueType ViewState
        /// </summary>
        public const String PARENT_QUEUE_TYPE_VIEWSTATE = "ParentQueueType";

        /// <summary>
        /// Constant for ParentQueue Query string used
        /// </summary>
        public const String PARENT_QUEUE_QUERYSTRING = "ParentQueue";

        #endregion

        #region COMMAND NAMES

        public const String GRID_COPY_PACKAGE_COMMAND = "CopyPackage";

        #endregion

        #region Communication Center Page Titles Constants

        public const String MESSAGE_VIEWER_PAGE_TITLE = "Message Description";


        #endregion

        public const String APPLICATION_CONNECTION_STRING = "SysXAppDBEntities";

        public const String ORDER_NUMBER = "OrderNumber";

        public const String PENDING_ITEM_NAMES = "PendingItemNames"; 
        #region AppSettings

        public const String APP_SETTING_SENDER_NAME = "SenderName";

        public const String APP_SETTING_SENDER_EMAIL_ID = "SenderEmailId";

        public const String APP_SETTING_STORE_BROWSER_AGENTS = "StoreBrowserAgent";

        /// <summary>
        /// Constant to define the name of the key 'CentralLoginUrl', used to differentiate the Tenant Login screen or Central login screen - UAT 446
        /// </summary>
        public const String APP_SETTING_CENTRAL_LOGIN_URL = "CentralLoginUrl";

        /// <summary>
        /// App Setting to configure whether to store Xml for the Items affected in Admin data entry or not.
        /// </summary>
        public const String APP_SETTING_LOG_DATA_ENTRY_XML = "LogDataEntryXml";

        /// <summary>
        /// App Setting to configure whether to use updated Paypal TLS 1.2 or not.
        /// </summary>
        public const String APP_SETTING_USE_PAYPAL_TLS_1_2 = "UsePaypalTls_1_2";

        #endregion

        #region Compliance Management

        /// <summary>
        /// Used in Compliance Maintenance Form to store the Id of the Edited Compliance item
        /// </summary>
        public const String COMPLIANCE_ITEM_ID_VIEW_STATE = "ComplianceItemID";

        /// <summary>
        /// Used in Compliance category management screen to manage the compliance items related to a Category
        /// </summary>
        public const String COMPLIANCE_PACKAGE_ID__VIEW_STATE = "PackageID";

        public const String COMPLIANCECATEGORY_COMPLIANCEITEM_QUERY_STRING = "cccId";

        public const String COMPLIANCE_CATEGORY_ID_QUERY_STRING = "cId";

        public const String COMPLIANCE_ITEM_ID_QUERY_STRING = "itemId";

        public const String CLIENT_COMPLIANCE_CATEGORY_ID_QUERY_STRING = "ccCatId";

        public const String CLIENT_COMPLIANCE_ITEM_ID = "ccId";

        public const String CLIENT_TENANT_ID = "tenantId";

        public const String COMPLIANCE_SCREEN_TYPE = "type";

        public const String PACKAGE_COMPLIANCE_IMAGE_URL = "~/Resources/Mod/Compliance/icons/yes16.png";
        public const String PACKAGE_NON_COMPLIANCE_IMAGE_URL = "~/Resources/Mod/Compliance/icons/no16.png";

        public const String COMPLIANCE_PKG_RENEWAL_ORDER_PLACED_TEXT = "Renewal Order Placed";
        #endregion

        #region Applicant
        public const String APPLICANT_FILE_LOCATION = "ApplicantFileLocation";
        public const String CDR_EXPORT_FILE_LOCATION = "CDRExportFileLocation";

        public const String SYSTEM_DOCUMENT_LOCATION = "SystemDocumentLocation";

        public const String MAXIMUM_FILE_SIZE = "MaxFileSizeAllowed";

        /// <summary>
        /// Package subscription id for the package selected by applicant for the data entry.
        /// </summary>
        public const String APPLICANT_PACKAGE_SUBSCRIPTION_ID_VIEW_STATE = "PackageSubscriptionId";

        /// <summary>
        /// Package subscription id for the package selected by applicant for the data entry.
        /// </summary>
        public const String APPLICANT_COMPLIANCE_STATUS_ID_VIEW_STATE = "ComplianceStatusID";

        /// <summary>
        /// Package subscription id for the package selected by applicant for the data entry.
        /// </summary>
        public const String APPLICANT_COMPLIANCE_STATUS_VIEW_STATE = "ComplianceStatus";

        /// <summary>
        /// Applicant Category Data Id(PK) for which applicant is updating the data in date entry.
        /// </summary>
        public const String APPLICANT_COMPLIANCE_CATEGORY_DATA_ID_VIEW_STATE = "ApplicantComplianceCategoryDataId";

        #endregion

        #region Queue Session Keys

        public const String VERIFICATION_QUEUE_SESSION_KEY = "VerificationGridCustomPaging";

        public const String EXCEPTION_QUEUE_SESSION_KEY = "ExceptionGridCustomPaging";

        //Session for maintaining control values
        public const String APPLICANT_SEARCH_SESSION_KEY = "ApplicantSearchDataContract";

        public const String CLIENT_SEARCH_SESSION_KEY = "ClientSearchDataContract";

        public const String CLIENT_LOGIN_SESSION_KEY = "ClientLoginSearchDataContract";

        //Session for maintaning Grid Filter, Paging and Index
        public const String APPLICANT_SEARCH_GRID_SESSION_KEY = "ApplicantSearchGridContract";

        public const String ORDER_QUEUE_SESSION_KEY = "OrderGridCustomPaging";

        public const String SEARCH_OBJECT_SESSION_KEY = "OrderGridSearchObject";

        /// <summary>
        /// String to differentiate the Queue is opened in Compliance or Background type.
        /// </summary>
        public const String QUEUE_TYPE_BKGORDER_QUEUE = "BkgOrderApprovalQueue";

        /// <summary>
        /// String to differentiate the Queue is opened in Compliance or Background type.
        /// </summary>
        public const String QUEUE_TYPE_CMPORDER_QUEUE = "ComplianceOrderApprovalQueue";


        public const String SUBSCRIPTION_SETTINGS_SESSION_KEY = "SubscriptionSettingsGridCustomPaging";

        public const String MOBILITY_SEARCH_SESSION_KEY = "MobilitytSearchDataContract";

        public const String BKG_ORDER_REVIEW_QUEUE_CONTRACT = "BkgOrderReviewQueueContract";

        public const String INVITATION_SEARCH_CONTRACT = "InvitationSearchContract";

        public const String SHARED_USER_SEARCH_CONTRACT = "SharedUserSearchContract";

        public const String ROTATION_SEARCH_SESSION_KEY = "ManageRotationSearchContract";

        public const String ROTATION_CATEGORY_SEARCH_SESSION_KEY = "ManageRotationCategorySearchContract";

        public const String ROTATION_DETAIL_SESSION_KEY = "RotationDetailSession";

        public const String REQ_VERIFICATION_QUEUE_SESSION_KEY = "ReqVerificationQueueSession";

        public const String STUDENT_ROTATION_SEARCH_SESSION_KEY = "StudentRotationSearchContract";

        public const String REQUIREMENT_PACKAGE_SEARCH_SESSION_KEY = "RequirementPackageDetailsContract";

        public const String ROTATION_MEMBER_SEARCH_SESSION_KEY = "RotationMemeberSearchContract";

        public const String SHARED_USER_ROTATION_SESSION_KEY = "SharedUserRotationSession";

        public const String SHARED_USER_ROTATION_PACKAGE_SESSION_KEY = "SharedUserRotationPackageSession";

        public const String SHARED_REQUIREMENT_SHARE_SESSION_KEY = "SharedRequirementShareSession";

        public const String MANAGE_INV_EXPIRATION_SESSION_KEY = "InvitationExpirationSession";

        public const String APPROVE_PENDING_REVIEW_ITEMS_SUCCESSMSG = "ApprovePendingReviewItemsSuccessMsg";

        public const String ROTATION_PACKAGE_SEARCH_SESSION_KEY = "ManageRequirmentPackageSearchContract";
        public const String AGENCY_APPLICANT_STATUS_SESSION_KEY = "AgencyApplicantSatusSeession";

        public const String ASSIGN_ROTATON_VERIFICATION_QUEUE_SESSION_KEY = "AssignRotverifcationQueueSession";
        public const String REQ_VERIFICATION_USER_WORK_QUEUE_SESSION_KEY = "ReqVerificationUserWorkQueueSesison";

        public const String MANAGE_REQ_TRI_PANEL_NAVIGATION_SESSION_KEY = "ManageTriPanelNavigation";
        public const String AGENCYUSER_PERMISSION_TEMPLATE_SEARCH_SESSION_KEY = "AgencyUserPermissionTemplateContract";
        public const String MANAGE_APPLICANT_APPOINTMENT_ORDER_SESSION_KEY = "ManageApplicantAppointmentOrderSession";
        public const String ADD_PRINT_SCAN_LOCATION_SESSION_KEY = "AddPrintScanLocationSession";
        public const String MANAGE_ONSITE_EVENTS_SESSION_KEY = "ManageOnsiteEventsSession";
        public const String MANAGE_ONSITE_EVENTS_FILTER_SESSION_KEY = "ManageOnsiteEventsFilterSession";
        public const String ROTATION_STUDENT_SEARCH_SESSION_KEY = "RotationStudentSearchSession";
        public const String ORDER_QUEUE_SEARCH_SESSION_KEY = "OrderQueueSearchSession";
        public const string MANAGE_HR_ADMIN_PERMISSIONS_SESSION_KEY = "ManageHrAdminPermissionsSearchSession";
        public const string MANAGE_ORDER_FULFILLMENT_QUEUE_SESSION_KEY = "ManageFulFillmentQueueSession";

        #endregion

        #region WebSiteSetup
        public const String WEBSITE_IMAGES = "websiteImages";
        public const String WEBSITE_SETUP_TITLE = "Website Setup";
        public const String WEBSITE_CUSTOMIZATION_TITLE = "Website Customization";
        public const String WEBSITE_CONTENT_MANAGEMENT_TITLE = "Content Management";
        public const String DEFAULT_LOGIN_IMAGE_PATH = @"~/InstitutionImages/SuperAdministrator/ADB_logo.jpg";
        #endregion

        #region rule constant
        public const String CURRENT_DAY = "$$TDAY$$";
        public const String CURRENT_MONTH = "$$TMTH$$";
        public const String CURRENT_YEAR = "$$TYR$$";
        public const String EMPTY = "$$NULL$$";
        public const String DOLLAR = "$$";
        //UAT-2234 :Added new Constant Type Submission Date
        public const String SUBMISSION_DATE = "$$SUBDAT$$";

        //UAT-2508 : Added new Constant Type Rotation StartDate
        public const String ROTATION_START_DATE = "$$ROTSDAT$$";
        //UAT-2508 : Added new Constant Type Rotation EndDate
        public const String ROTATION_END_DATE = "$$ROTEDAT$$";
        #endregion

        #region Manage Institution Node
        public const String TITLE_MANAGE_INSTITUTION_NODE = "Manage Institution Node";
        #endregion

        #region UAT:2666
        public const String HIGHLIGHT_ROTATION_FIELD_UPDATED_BY_AGENCIES = "HighlightRotationFieldUpdatedByAgencies";
        public const String CHILD_HIGHLIGHT_ROTATION_FIELD_UPDATED_BY_AGENCIES = "ChildHighlightRotationFieldUpdatedByAgencies";
        #endregion

        #region Validation Messages

        public const String MSG_SELECT_ITEM = "Please select atleast one item to be assigned.";
        public const String MSG_SELECT_USER = "Please select a user to assign items.";
        /// <summary>
        /// Item Assigned Success message
        /// </summary>
        public const String MSG_ITEM_ASSIGNED_SUCCESS = "Item(s) assigned to the selected user successfully.";
        #endregion

        #region service constant

        public const Int32 CHUNK_SIZE_FOR_CATEGORY_RULE_REOCCUR = 50;
        public const Int32 CHUNK_SIZE_FOR_ITEM_EXPIRY = 50;
        public const Int32 CHUNK_SIZE_FOR_MAIL_BEFORE_SUBSCRIPTION_EXPIRY = 50;
        public const Int32 CHUNK_SIZE_FOR_MAIL_AFTER_SUBSCRIPTION_EXPIRY = 50;
        public const Int32 CHUNK_SIZE_FOR_MAIL_PENDING_PACKAGE = 50;
        public const Int32 CHUNK_SIZE_FOR_MAIL_BEFORE_ITEM_EXPIRY = 50;
        public const Int32 CHUNK_SIZE_FOR_MAIL_AFTER_ITEM_EXPIRY = 50;
        public const Int32 CHUNK_SIZE_FOR_PROCESS_ITEM_EXPIRY = 50;
        public const Int32 CHUNK_SIZE_FOR_CREATE_MOBILITY_INSTANCE = 50;
        public const Int32 DAYS_DUE_BEFORE_TRANSITION = 15;
        public const Int32 CHUNK_SIZE_FOR_COPY_PACKAGE_EXECUTE_RULES = 50;
        public const Int32 CHUNK_SIZE_FOR_AUOMATIC_MOVEMENT_NODE_TRANSITIONS = 50;
        public const Int32 CHUNK_SIZE_FOR_DEADLINE_MAIL = 50;
        public const Int32 CHUNK_SIZE_FOR_APPROVE_INVOICE_ORDER_APPROVAL = 50;
        public const Int32 CHUNK_SIZE_FOR_SVC_FRM_DISPATCHED_NOTIFICATION = 50;
        public const Int32 CHUNK_SIZE_FOR_DAILY_BACKGROUND_REPORT = 50;
        public const Int32 CHUNK_SIZE_FOR_ARCHIVE_EXPIRED_SUBSCRIPTION = 50;
        public const Int32 CHUNK_SIZE_FOR_ARCHIVE_EXPIRED_SUBSCRIPTION_NOTIFICATION = 50;
        public const Int32 CHUNK_SIZE_FOR_CATEGORY_COMPLIANCE_REQUIRED = 50;
        public const Int32 CHUNK_SIZE_FOR_PROCESS_REQUIREMENT_ITEM_EXPIRY = 50;
        public const Int32 CHUNK_SIZE_FOR_PROCESS_ROTATION_TO_ARCHIVE = 50; //UAT-3139
        public const Int32 CHUNK_SIZE_FOR_SCHEDULED_INVITATIONS = 10;
        public const Int32 CHUNK_SIZE_FOR_CONTRACTS = 50;
        public const Int32 CHUNK_SIZE_FOR_COMPLIANCE_DOCUMENT_COMPLETION = 50;
        public const Int32 CHUNK_SIZE_FOR_SALES_FORCE = 50;
        //UAT_1852 : If a Service Group is complete, but other service groups are not complete within an order, the system should send the applicant an email.
        public const Int32 CHUNK_SIZE_FOR_BKG_SVC_GRPS_COMPLETION = 50;
        public const Int32 CHUNK_SIZE_FOR_REQUIREMNT_CATEGORY_RULE_EXECUTION = 50;
        public const Int32 CHUNK_SIZE_FOR_REQUIREMNT_PACKAGE_RULE_EXECUTION = 50;
        public const Int32 CHUNK_SIZE_FOR_COPY_COMPLIANCE_DATA_TO_REQUIREMENT = 50;
        public const Int32 CHUNK_SIZE_FOR_REQUIREMENT_PACKAGE_SYNC = 50;
        public const Int32 RETRY_TIME_LAG_FOR_REQUIREMENT_PACKAGE_SYNC = 60;
        public const Int32 RETRY_COUNT_FOR_REQUIREMENT_PACKAGE_SYNC = 3;
        public const Int32 CHUNK_SIZE_FOR_CREATE_REQUIREMENT_SNAPSHOT_ON_ROTATION_END = 50;
        //UAT-2495
        public const Int32 CHUNK_SIZE_FOR_CLIENT_DATA_UPLOAD = 50;
        //UAT-2603
        public const Int32 CHUNK_SIZE_FOR_ROTATION_DATA_MOVEMENT = 50;
        //UAT-2628
        public const Int32 CHUNK_SIZE_FOR_FAILED_DOCUMENT_MERGING = 50;
        public const Int32 CHUNK_SIZE_FOR_AUTOMATIC_PACKAGE_INVITATION = 50;//UAT-2388

        public const Int32 CHUNK_SIZE_FOR_UPDATED_APPLICANT_REQUIREMENTS_NOTIFICATION = 50;//UAT-3059
        //UAT-2513
        public const Int32 CHUNK_SIZE_FOR_BATCH_ROTATION_UPLOAD = 50;
        //UAT-3112
        public const Int32 CHUNK_SIZE_BADGE_FORM_NOTIFICATION = 50;
        //UAT-2960
        public const Int32 CHUNK_SIZE_ALUMNI_ACCESS_NOTIFICATION = 50;
        //UAT-2960
        public const Int32 CHUNK_SIZE_COPY_COMPLIANCE_TO_COMPLIANCE = 50;
        //UAT-3485
        public const Int32 CHUNK_SIZE_FOR_MAIL_BEFORE_REQ_ITEM_EXPIRY = 50;

        public const Int32 CHUNK_SIZE_FOR_MAIL_COMPLIANCE_EXCEPTION_EXPIRY = 50;
        //UAT-3137
        public const Int32 CHUNK_SIZE_FOR_REQUIRED_ROTATION_CATEGORY_BEFORE_GOING_TO_BE_REQUIRED = 50;

        //UAT-3734
        public const Int32 CHUNK_SIZE_FOR_MISSED_FINGERPRINT_APPOINTMENT = 50;

        //Send Mail DarftOrder Notification to Admin
        public const Int32 CHUNK_SIZE_FOR_SEND_DRAFT_ORDER_NOTIFICATION_TO_ADMIN = 50;

        //Send Mail Invitation Pending Order Notification to Applicant
        public const Int32 CHUNK_SIZE_FOR_SEND_INVITATION_PENDING_ORDER_NOTIFICATION_TO_APPLICANT= 50;
        
        public const Int32 CHUNK_SIZE_FOR_SEND_CHANGE_ORDER_STATUS_COMPLETED_TO_ARCHIVED_NOTIFICATION = 50;

        public const Int32 CHUNK_SIZE_FOR_SEND_SRV_GROUP_COMPLETED_ORDER_NOTIFICATION_TO_ADMIN = 50;

        public const Int32 CHUNK_SIZE_FOR_SEND_COMPLETED_ORDER_WITH_ATTACHMENT_NOTIFICATION_TO_ADMIN = 50;
        #endregion

        #region BkgOrderService constant

        public const Int32 CHUNK_SIZE_FOR_CREATE_ORDER_SERVICE = 20;
        public const Int32 MAX_RETRY_COUNT_FOR_CREATE_ORDER_SERVICE = 3;
        public const Int32 RETRY_TIME_LAG_FOR_CREATE_ORDER_SERVICE = 60;
        public const Boolean UPDATE_ALL_INPROCESS_ORDERS = true;
        public const Int32 UPDATE_ALL_INPROCESS_ORDERS_ON_OR_AFTER = 4;
        public const Boolean UPDATE_EX_INPROCESS_ORDERS = false;
        public const Boolean UPDATE_AMS_INPROCESS_ORDERS = true;
        public const Int32 UPDATE_ORDER_STARTING_ORDERID = 0;
        public const Int32 UPDATE_ORDER_ENDING_ORDERID = 0;
        public const Int32 UPDATE_ORDER_RECENT_DAYS = 10;
        public const Int32 UPDATE_ORDER_RETRY_TIME_LAG = 60;
        public const Int32 CHUNK_SIZE_FOR_BKG_ORDER_NOTIFICATION_SERVICE = 50;
        public const String VENDOR_LOGIN_ID = "adb_test";
        public const String VENDOR_PASSWORD = "#StarGate09";
        public const Int32 VENDOR_BUSINESS_OWNER_ID = 59;
        public const Boolean USE_ADB_TEST_ACCOUNT_FOR_AMS = true;
        public const String ADB_TEST_ACCOUNT_FOR_AMS = "AMER_02772";
        public const Int32 CHUNK_SIZE_FOR_CREATE_BULK_ORDER_SERVICE = 10;
        public const Int32 CHUNK_SIZE_FOR_BKG_COPY_PACKAGE_DATA = 50;

        #region UAT-4162:- Retry
        public const Int32 RETRY_DS_ORDERS_CHUNKSIZE = 20;
        public const Int32 RETRY_DS_ORDER_RETRY_TIMELAG = 60;
        public const Int32 MAX_RETRY_COUNT_FOR_DS_ORDERS = 3;
        #endregion

        #endregion

        #region Multiple Tenant search

        public const Int32 DEFAULT_SELECTED_TENANTID = -1;
        public const Int32 DEFAULT_SEARCH_INSTANCEID = 0;
        public const Int32 ONLINE_SEARCH_TABINDEX = 0;
        public const Int32 OFFLINE_SEARCH_TABINDEX = 1;

        #endregion

        #region Mobility Constant

        public const String INSTITUTION_CHANGE_QUEUE_TITLE = "Institution Change Queue";
        public const String INSTITUTION_CHANGE_REQUEST_DETAIL = "InstitutionChangeRequestDetail";

        #endregion

        #region PDF Conversion service
        public const String PDF_CONVERSION_SERVICE_CONTRACT_NAME = "IPDFConversion";
        public const String PDF_CONVERSION_SERVICE_OPERATION_CONTRACT = "GeneratePDF";
        #endregion

        #region AssignmentConfigurationQueue
        public const String SELECTED_TENANTID = "SelectedTenantID";

        public const String CONFIG_QUEUE_TYPE = "QueueType";

        public const String QUEUE_ASSIGNMENT_CONFIG_ID = "QueueAssignmentConfID";

        public const String ASSIGN_CONFIG_SEARCH_SESSION_KEY = "AssignmentConfigQueuetSearchDataContract";
        #endregion

        #region E signed Documents
        public const String DISCLOSURE_DOCUMENT_CHECK_REQUIRED = "DisclosureDocumentCheckRequired";
        #endregion

        public const string SESSIONMENU = "Core.Menu";

        public const String ACTION_PERMISSION = "ActionPermission";

        #region UAT-2529
        public const String ADMIN_AGENCY_PROFILE_SHARING_RESTRICTION_MESSAGE = "has elected to receive student information via rotation share only. Please navigate to the \"Manage Rotation\" Module and process your share from there.";
        public const String STUDENT_INDIVIDUAL_PROFILE_SHARING_RESTRICTION_MESSAGE = "has elected to receive student information via rotation share only. Please select the agency you would like to share with from the Agency Dropdown and enter the details of your rotation.";
        #endregion

        public const String SESSION_MENU_ID = "MenuID";

        public const String BKG_ORDER_QUEUE_SESSION_KEY = "BkgOrderGridCustomPaging";

        public const String BKG_SEARCH_OBJECT_SESSION_KEY = "BkgOrderGridSearchObject";

        public const String BKG_REVIEW_QUEUE_OBJECT_SESSION_KEY = "BkgOrderReveiwGridSearchObject";

        public const String MANUAL_SERVICE_FORMS_OBJECT_SESSION_KEY = "ManualServiceFormsGridSearchObject";

        #region Background Order Event Type Details - Simply the text for any background order event

        public const String Bkg_Order_Approved = "Background Order Approved";
        public const String Bkg_Order_Rejected = "Background Order Rejected";
        public const String Bkg_Order_Created = "New Background Order Created";
        public const String Bkg_Order_Status_Additional_Work = "Changed Order status from First Review to Additional Work";
        
        #region Admin Entry Events
        public const String Bkg_Order_Draft_To_Inprogess = "Changed Order Status from Draft to In Progress.";
        public const String Applicant_Invitation_Completed = "Changed Order Draft Status from Invitation Sent to Invitation Completed.";
        public const String Admin_Entry_Order_On_Hold = "On Hold applied to Background Order.";
        #endregion

        #endregion

        public const String SESSION_ORDER_ID = "OrderID";
        public const String SESSION_SELECTED_TENANT_ID = "SelectedTenantID";
        public const String SESSION_ORDER_NUMBER = "OrderNumber";
        //public const String SESSION_ORDER_PACKAGE_SERVICEGROUP_ID = "OrderPackageSvcGrpID";
        //public const String SESSION_PARENT_SCREEN_NAME = "ParentScreenName";

        #region ExternalVendor

        public const String PERSONAL_ALIAS_ATTRIBUTE_GROUP_CODE = "A26014A2-ED57-47CC-B68F-17C02E376D60";
        public const String COUNTRY = "Country";
        public const String COUNTY = "County";
        public const String RegistrationID = "RegistrationID";

        #endregion

        #region E Drug Screening

        #region E-DRUG Screening Attributes Constant
        public const String IS_US_CITIZEN = "9CDE055A-918F-4F08-8057-FB52B1BB0A33";
        public const String DRUG_SCREEN_REGISTRATION_ID = "597D7961-F7FB-4BB7-AD55-6611BC374540";
        public const String LAB_NAME = "50261392-2015-49B2-9E15-9E28827779D3";
        public const String REGISTRATION_EXPIRATION_DATE = "5F7235A3-43C4-4432-B688-B6214133BD3B";
        public const String SITE_ADDRESS = "F5B5DFDE-4251-401C-A0BB-9019018E0E34";
        public const String SITE_NAME = "0D0D2B44-E5E8-4E56-B93A-8532098A41EE";
        public const String SITE_PHONE_NUMBER = "C85F3AAE-0FC1-4268-91A8-F2B861D954D7";
        public const String POSTAL_CODE = "6B0DC2E4-C63E-45EF-94B7-AB5D4FC98EED";
        public const String DISTANCE_FROM_LAB = "FC457985-9B13-4A4A-9A1B-5865ABB6CEC5";
        public const String SITE_ID = "3AA78183-A871-4B12-BA01-A0EE69660288";
        public const String LC_PANEL_ID = "F5E5EAD9-8D53-4723-8EC2-EBA057B22FBD";
        public const String DONOR_HOME_PHONE_NUMBER = "879A0A77-70D9-4746-B6F0-44C2AD1443F6";
        public const String DONOR_WORK_PHONE_NUMBER = "DE5E4832-0899-4182-B1B3-85E3961F04A8";
        public const String DONOR_GENDER = "6EAB6EA4-7822-40A7-B26F-6087080024B4";
        public const String DONOR_EMAIL = "F1239D67-4E92-4FBA-9F43-181C6CA82AE9";
        public const String REASON_CODE = "E6E16330-F0C5-450C-A58E-EF216B98D5DB";
        public const String REASON_DESCRIPTION = "E03D6131-E602-48BD-974D-03B8E5DB1C9D";
        public const String EXPIRATION_DATE = "4155AF3F-F94A-41E2-BC3B-F46833122B9C";
        public const String COUNTRY_EDRUG = "8011971B-6CAA-476C-AA93-11B2F8E121C0";
        public const String STATE_EDRUG = "BCF7038B-C709-4EA2-B7F8-BB687DD02C8F";
        public const String CITY_EDRUG = "699B9DEC-0677-4021-995A-2E558AD305FD";
        public const String COUNTY_EDRUG = "FF52BCBE-F19A-4B50-BD1A-66720BC1398E";
        #endregion

        #region E-EDRUG Screening Attribute group code constant
        public const String ELECTRONIC_DRUG_SCREEN_ATT_GROUP_CODE = "6E24F8BD-671D-4BBF-9984-5F6AE50B7041";
        #endregion
        #endregion
        public const String CANCELED_ORDER_STATUS_TYPE = "AAAC";

        #region Dissociation Work

        public const String DISSOCIATION_BUTTON_VISIBLE = "Visible";
        public const String DISSOCIATION_BUTTON_HIDDEN = "Hidden";
        public const String DISSOCIATION_BUTTON_ALL = "All";

        #endregion

        //UAT-613 Explanatory State Constants for Verification detail
        public const String APPLICANT_EXPLANATORY_STATE = "Applicant State";
        public const String ADMIN_EXPLANATORY_STATE = "Admin State";
        public const String CLOSED_EXPLANATORY_STATE = "Closed State";

        //UAT-523 WB: Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in. 
        public const String WHOLE_CATEGORY_GUID = "9122D197-27EE-44AC-8CC4-0A68F3D21F32";

        #region Attribute Group Code For Background Packages
        public const String PERSONAL_INFORMATION_ATTRIBUTE_GROUP_CODE = "CC184FC4-5401-445D-90AA-E77167227904";
        public const String RESIDENTIAL_HISTORY_ATTRIBUTE_GROUP_CODE = "338F1CA2-6B0A-43C1-B900-A8F6B058678F";
        public const String MVR_ATTRIBUTE_GROUP_CODE = "CF76960D-2120-46FE-9E03-01C218F8A336";
        #endregion

        #region Order Flow Constants

        public const String PENDING_ORDER_NAVIGATION_FROM = "OrderNavigationFrom";

        /// <summary>
        /// Text for Previous button in order flow
        /// </summary>
        public const String PREVIOUS_BUTTON_TEXT = "Previous";

        /// <summary>
        /// Text for Next button in order flow
        /// </summary>
        public const String NEXT_BUTTON_TEXT = "Next";

        /// <summary>
        /// Text for Restart button in order flow
        /// </summary>
        public const String RESTART_ORDER_BUTTON_TEXT = "Restart Order";

        /// <summary>
        /// Text for Start button in order flow
        /// </summary>
        public const String START_BUTTON_TEXT = "Start Order";

        /// <summary>
        /// Text for Continue button in order flow
        /// </summary>
        public const String CONTINUE_BUTTON_TEXT = "Continue";

        /// <summary>
        /// Text for Cancel button in order flow
        /// </summary>
        public const String CANCEL_BUTTON_TEXT = "Cancel Order";

        /// <summary>
        /// Text for Submit Order button in order flow
        /// </summary>
        public const String SUBMIT_ORDER_BUTTON_TEXT = "Submit Order";

        /// <summary>
        /// Text for Proceed button in Disclosure & Disclaimer in order flow
        /// </summary>
        public const String PROCEED_BUTTON_TEXT = "Proceed";

        /// <summary>
        /// Text for Back to Order Review button in order flow, on OrderPayment screen
        /// </summary>
        public const String BACK_TO_ORDER_REVIEW_BUTTON_TEXT = "Back To Order Review";

        /// <summary>
        /// Text for Accept button in order flow
        /// </summary>
        public const String ACCEPT_BUTTON_TEXT = "Accept";

        /// <summary>
        /// Query string constant for the navigation to previous screen using the 'Previous' button
        /// </summary>
        public const String ORDER_REQUEST_TYPE_VIEWSTATE = "OrderRequestType";

        /// <summary>
        /// UAT -4331
        /// </summary>
        public const String SCHEDULE_APPLICANT_APPOINTMENT = "OrderNavigationFrom";
        #endregion

        #region UAT-806 : Granular Permission
        public const String SSN_MASK_FORMATE = @"\#\#\#-\#\#-####";
        public const String SSN_MASK_FORMAT_ALPHANUMERIC = @"\#\#\#-\#\#-aaaa";
        public const String EMPTY_SSN = "_________";
        public const String EMPTY_SSN_MASKED = "____";
        #endregion

        public const String DASHBOARD = "Dashboard";
        public const String DASHBOARD_URL = "~/Dashboard/Pages/ApplicantDashboardMain.aspx";

        public const String CATEGORY_EXP_NOTES_LINK_TEXT = "More information";

        #region Wait hours Constant to send the order notification mail to newly created applicant
        public const String SEND_NOTIFICATION_WAIT_HOURS_KEY = "SendNotificationWaitHours";
        #endregion

        #region Sales Force

        public const String GRANT_TYPE_APP_KEY = "WGUSF_GrantType";
        public const String CLIENT_ID_APP_KEY = "WGUSF_ClientID";
        public const String CLIENT_SECRET_APP_KEY = "WGUSF_ClientSecret";
        public const String USERNAME_SALESFORCE_APP_KEY = "WGUSF_UserName";
        public const String PASSWORD_TOKEN_APP_KEY = "WGUSF_PasswordToken";
        public const String AUTHENTICATION_REQUEST_URL = "WGUSGF_AuthenticationRequestUrl";
        public const String UPLOAD_REQUEST_URL = "WGUSF_UploadRequestUrl";
        public const String WGUSF_UNIVERSITY_TENANTID = "WGUSF_TenantID";
        public const String CHUNK_SIZE_APP_KEY = "WGUSF_ChunkSize";

        #endregion

        public const String LOCAL_HOST = "localhost";
        public const String SELECT_TENANT_URL = "~/SelectTenant.aspx";
        public const String SELECT_BUSINESS_CHANNEL_URL = "~/SelectBuisnessChannel.aspx";

        public const String TECHNICAL_ERROR_MSG_DISCLOSURE_AUTHORIZATION = "Due to some technical reasons this order can not be completed. Please order again.";

        //UAT 983
        public const String LOCKED = "User Locked";
        public const String UNLOCKED = "User Unlocked";
        public const String CMD_LOCK = "lock";
        public const String CMD_UNLOCK = "unlock";
        public const String LOCK_FAILED_MSG = "User locking failed";
        public const String LOCK_TOOLTIP = "Click here to lock user";
        public const String UNLOCK_TOOLTIP = "Click here to unlock user";


        //Master Review Criteria
        public const String MRC_SAVED_SUCCESS_MSG = "Review criteria has been successfully saved.";
        public const String MRC_UPDATE_SUCCESS_MSG = "Review criteria has been successfully updated.";
        public const String MRC_DELETE_SUCCESS_MSG = "Review criteria has been successfully deleted.";
        public const String MRC_SAVED_ERROR_MSG = "Review criteria insertion failed.";
        public const String MRC_UPDATE_ERROR_MSG = "Review criteria updation failed.";
        public const String MRC_DELETE_ERROR_MSG = "Review criteria deletion failed.";
        public const String INSTITUTION_REQUIRED_INFO = "Please select a institution.";

        //Order Review Queue screen Name
        public const String BKG_ORDER_REVIEW_QUEUE = "BkgOrderReviewQueue";

        //Bkg Report Viewer
        public const String BKG_REPORT_VIEWER = "~/BkgOperations/Pages/BkgReportViewer.aspx";

        //UAT-3525
        public const String BPT_SAVED_SUCCESS_MSG = "Package Type saved successfully.";
        public const String BPT_SAVED_ERROR_MSG = "Some error occurred.Please try again.";
        public const String BPT_UPDATED_SUCCESS_MSG = "Package Type updated successfully.";
        public const String BPT_UPDATED_ERROR_MSG = "Some error occurred.Please try again.";
        public const String BPT_DELETED_SUCCESS_MSG = "Package Type deleted successfully.";
        public const String BPT_DELETED_ERROR_MSG = "Some error occurred.Please try again.";


        //UAT-1039
        public const String SHOW_UPLOAD_DOC_VID = "ShowUploadDocument";
        public const String SHOW_DATA_ENTRY_VID = "ShowDataEntry";
        public const String COMPLIANCE_PKG_NOT_EXIST = "CompliancePkgNotExist";
        public const String ALREADY_UPLOADED_DOC = "AlreadyUploadedDocument";
        public const String ALREADY_DATA_ENTERED = "AlreadyDataEntered";

        public const String HTML_XSS_INJECTION_ERROR_MSG = "a potentially dangerous request.form value";
        public const Int32 XSS_HTML_INJECTION_ERROR_NUMBER = -2147467259;

        #region  Profile Sharing constants

        /// <summary>
        /// Constant for Session to store invitation data in preview.
        /// </summary>
        public const String SESSION_KEY_INVITATIONDATA = "InvitationData";

        public const String APP_SETTING_SHARED_USER_LOGIN_URL = "SharedUserLoginURL";
        public const String APPLICANT_INVITATION_DETAILS = "~\\ProfileSharing\\UserControl\\ApplicantInvitationDetails.ascx";

        public const String YOUTUBE_URL_REPLACEMENT_KEY = "YouTubeUrlReplacementKey";
        public const String VIMEO_URL_REPLACEMENT_KEY = "VimeoUrlReplacementKey";

        /// <summary>
        /// Constant for the 'ManageAgencySharing.ascx' user control
        /// </summary>
        public const String MANAGE_AGENCY_SHARING_CONTROL = @"~\ProfileSharing\ManageAgencySharing.ascx";

        public const String QUERY_STRING_INVITE_TOKEN = "InviteToken";
        public const String SHARED_USER_DASHBOARD = "~/ProfileSharing/ManageInvitationsSharedUser.ascx";
        public const String SHARED_USER_REGISTRATION = "~/SharedUserRegistration.aspx";
        public const Int32 SHARED_USER_TENANT_ID = 1;
        public const String PSIEMAILSUBJECT_APPCONFIGURATIONKEY_APPLICANT = "PSIEmailSubjectApplicant";
        public const String PSIEMAILCONTENT_APPCONFIGURATIONKEY_APPLICANT = "PSIEmailContentApplicant";
        public const String INSTRUCTOR_PRESEPTOR_DASHBOARD = "~/ProfileSharing/InstructorPreceptorDashboard.ascx";

        public const String INSTRUCTOR_PRESEPTOR_ROTATION_PACKAGE_SEARCH = "~/ProfileSharing/UserControl/InstructorPreceptorRotationPackageSearch.ascx";


        public const String PSIEMAILSUBJECT_APPCONFIGURATIONKEY_ADMIN = "PSIEmailSubjectAdmin";
        public const String PSIEMAILCONTENT_APPCONFIGURATIONKEY_ADMIN = "PSIEmailContentAdmin";

        public const String PSIEMAILSUBJECT_APPCONFIGURATIONKEY_ADMIN_ROT_SHARING = "PSIEmailSubjectAdminRotationSharing";
        public const String PSIEMAILCONTENT_APPCONFIGURATIONKEY_ADMIN_ROT_SHARING = "PSIEmailContentAdminRotationSharing";

        public const String PSIEMAIL_RECIPIENTNAME = "##recipientname##";
        public const String PSIEMAIL_RECIPIENTID = "##recipientid##";
        public const String PSIEMAIL_STUDENTNAME = "##studentname##";
        public const String PSIEMAIL_SCHOOLNAME = "##schoolname##";
        public const String PSIEMAIL_INSTITUTION = "##institutionname##";
        public const String PSIEMAIL_PROFILEURL = "##profileurl##";
        public const String PSIEMAIL_CENTRALLOGINURL = "##centralloginurl##";
        public const String PSIEMAIL_CUSTOMMESSAGE = "##custommessage##";
        public const String PSIEMAIL_SELECTEDPACKAGEDATA = "##selectedpackagedata##";
        public const String PSIEMAIL_SHAREDAPPLICANTDATA = "##sharedapplicantdata##";
        public const String PSIEMAIL_RotationDetails = "##rotationdetails##";
        public const String PSIEMAIL_CustomAttributes = "##customattributes##";

        //UAT-2556:Separate email template for student shares that used agency dropdown from those that do not. 
        public const String PSIEMAIL_APPLICANT_NAME = "##applicantname##";
        public const String PSIEMAIL_USER_FULL_NAME = "##userfullname##";
        public const String PSIEMAIL_APPLICATION_URL = "##applicationurl##";

        public const String PSIEMAIL_CUSTOMMESSAGE_TEXT = "Thank you for this opportunity. Please accept this email as an invitation to review my compliance requirements.";

        //UAT-2181:Enhance adding tenants to agencies with check boxes on the Manage Agencies screen to select which agencies you would like to add the selected tenant to
        public const string AG_SAVED_TENANT_SUCCESS_MSG = "Institution assigned to Agency(s) successfully.";
        public const string AG_NOT_SAVED_SUCCESS_MSG = "This institution is already mapped with selected Agency(s).";

        public const String AG_SAVED_SUCCESS_MSG = "Agency Saved successfully.";
        public const String AG_SAVED_ERROR_MSG = "Some error occurred.Please try again.";
        public const String AG_SAVED_HIERARCHY_ERROR_MSG = "Some error occurred while mapping Hierarchy to Agency in one or more Institution.Please try again.";
        public const String AG_UPDATED_SUCCESS_MSG = "Updated successfully.";
        public const String AG_UPDATED_ERROR_MSG = "Some error occurred.Please try again.";
        public const String AG_UPDATED_HIERARCHY_ERROR_MSG = "Some error occurred while updaing Hierarchy mapped with selected Agency.Please try again.";
        public const String AG_DELETED_SUCCESS_MSG = "Agency Deleted successfully.";
        public const String AG_DELETED_HIERARCHY_ERROR_MSG = "Some error occurred while deleting hierarchy of selected agency. Please try again.";
        public const String AG_DELETED_ERROR_MSG = "Some error occurred.Please try again.";
        public const String AG_DELETED_INFO_MSG = "Agency User is exist. First delete Agency Users before deleting Agency";
        public const String AG_NPINUMBER_EXIST_MSG = "This NPI number already exist for an Agency.";

        public const String AG_DELETION_CR_ASSOCIATED_MSG = "This Agency cannot be deleted as it is in use.";

        public const String AGU_SAVED_SUCCESS_MSG = "Agency User saved successfully.";
        public const String AGU_SAVED_ERROR_MSG = "Some error occurred.Please try again.";
        public const String AGU_UPDATED_SUCCESS_MSG = "Agency User updated successfully.";
        public const String AGU_UPDATED_ERROR_MSG = "Some error occurred.Please try again.";
        public const String AGU_DELETED_SUCCESS_MSG = "Agency User deleted successfully.";
        public const String AGU_DELETED_ERROR_MSG = "Some error occurred.Please try again.";
        public const String MIN_ATTESTATION_SIGN_LENGTH_KEY = "MinAttestationSignLength";
        public const String MIN_PROFILE_SHARING_SIGN_LENGTH_KEY = "MinProfileSharingSignLength";
        public const String AGU_SEND_EMAIl_UPDATE_VERIFICATION_LINK_MSG = "An email has been sent with a verification link to agency user on {0}. System will update email address once agency user clicks on verification link.";


        public const String AGUPT_SAVED_SUCCESS_MSG = "Agency User Permisison Template saved successfully.";
        public const String AGUPT_SAVED_ERROR_MSG = "Some error occurred.Please try again.";
        public const String AGUPT_UPDATED_SUCCESS_MSG = "Agency User Permisison Template updated successfully.";
        public const String AGUPT_UPDATED_ERROR_MSG = "Some error occurred.Please try again.";
        public const String AGUPT_DELETED_SUCCESS_MSG = "Agency User Permisison Template deleted successfully.";
        public const String AGUPT_DELETED_ERROR_MSG = "Some error occurred.Please try again.";

        public const String VIEW_ATTESTATION = "viewattestation";

        //UAT-1176 Employment Node Disclosure
        public const String EMPLOYMENT_DISCLOSURE = "~/EmploymentDisclosure.aspx";
        public const String EMPLOYMENT_DISCLOSURE_DOCUMENT = "EmploymentDisclosureDocument";

        //UAT-1178 User Attestation Disclosure  Form
        public const String USER_ATTESTATION_DISCLOSURE_PAGE = "~/UserAttestationDisclosure.aspx";
        public const String USER_ATTESTATION_DISCLOSURE_FOR_CLIENT_ADMIN = "UserAttestationDisclosureClient";
        public const String USER_ATTESTATION_DISCLOSURE_FOR_SHARED_USER = "UserAttestationDisclosureSharedUser";

        public const String SHARED_USER_STUDENT_ROTATION_SEARCH = "~/ProfileSharing/UserControl/StudentRotationSearch.ascx";
        public const String STUDENT_ROTATION_SEARCH = "StudentRotationSearch";
        #endregion

        //UAT-1187
        public const String NO_DISCLOSURE_MAPPED_INFO_MSG = "You cannot place this Order as there is no Disclosure Form associated with it. Please contact system Administrator.";

        #region UAT-1178
        public static string UAF_PARTIALLY_FILLED_MODE = "UAF_PARTIALLY_FILLED_MODE";
        public static string UAF_PREVIEW_MODE = "UAF_PREVIEW_MODE";
        public static string UAF_FULLY_FILLED_MODE = "UAF_FULLY_FILLED_MODE";
        #endregion

        //UAT 1043 WB: Admin Comm Copy Settings should have super admin behavior for all ADB admins. 
        public static string USERTYPE_SUPERADMIN = "USERTYPE_SUPERADMIN";
        public static string USERTYPE_ADBUSER = "USERTYPE_ADBUSER";
        public static string USERTYPE_CLIENTADMIN = "USERTYPE_CLIENTADMIN";

        #region UAT-1201
        public const String VIEW_ATTESTATION_ADMIN = "ViewAttestationForAdmin";
        #endregion

        #region UAT-1214:Changes to "Required" and "Optional" labels in order flow
        public static string BACKGROUND_ORDER_FLOW_REQUIRED_DEFAULT_LABEL = "Required Package(s)";
        public static string BACKGROUND_ORDER_FLOW_OPTIONAL_DEFAULT_LABEL = "Optional Package(s)";
        #endregion

        public static string ORDER_FLOW_IMMUNIZATIONPKG_DEFAULT_LABEL = "Immunization Package(s)";
        public static string ORDER_FLOW_ADMINISTRATIVEPKG_DEFAULT_LABEL = "Administrative Package(s)";

        //UAT-2413
        public static string ENTER_REQUIREMENT_TEXT = "Enter Requirements";

        //UAT-1219
        public static string ATTESTATION_REPORT_TEXT = @"I attest by signing these checklists that all information is maintained in the above-named file, and will be provided by the school to the agency upon request.";

        //        public static string ATTESTATION_REPORT_TEXT = @"As a designated representative of #####, I attest that the above information is present in the student's file and that the above named students have been determined to be competent for the field of study and assigned area.
        //I further attest that the above information is present in the faculty member's file. I further attest that the background investigation report does not include any information about prior or pending investigations, reviews, sanctions or peer review proceedings; or limitations of any licensure, certification, or registration.
        //This attestation is provided in lieu of providing a copy of the background investigation report for each student and faculty/staff member, if applicable.";


        #region UAT-1189:WB: Bulk Archive capability
        public const String SESSION_APPLICANT_DETAIL_XML_KEY = "SessionApplicantDetailXMLKey";
        #endregion

        //UAT-1230
        public static string CREATE_COMPL_ACC_SUBEVNT_CODE = "NTCRACINVT";

        //UAT-1302 As an admin (client or ADB), I should be able to create preceptors/instructors
        public static string CLIENT_CONTACT_INVITATION_SUBEVNT_CODE = "NTCLCONTIV";
        public static string CLIENT_CONTACT_SYSTEM_NAME = "ADB Client Contact Creation System";
        public static string CLIENT_CONTACT_DOC_UPLOADINFOMSG = "Please select a document to upload.";
        public static string CLIENT_CONTACT_DOC_ALREADY_UPLOADED = "This type of document is already uploaded.";

        public static string CLINICAL_ROTATION_ASSIGNED_SUBEVNT_CODE = "NTCLROTASG";

        public const String QUERY_STRING_CLIENTCONTACT_INVITE_TOKEN = "ClientContactToken";
        public const String QUERY_STRING_CLIENTCONTACT_USERTYPE = "UserType";

        public const String QUERY_STRING_AGENCY_USER_ID = "AgencyUserID";
        public const String QUERY_STRING_AGENCY_EMAIL_ID = "AgencyEmailID";
        #region Clinical Rotation

        /// <summary>
        /// Constant for the 'ManageAgencySharing.ascx' user control
        /// </summary>
        public const String ROTATION_DETAIL_CONTROL = @"~\ClinicalRotation\UserControl\RotationDetailForm.ascx";
        public const String MANAGE_ROTATION_CONTROL = @"~\ClinicalRotation\ManageRotation.ascx";
        public const String MANAGE_ROTATION_BY_AGENCY_CONTROL = @"~\ClinicalRotation\ManageRotationByAgency.ascx";
        public const String ADD_REQUIREMENT_PACKAGE_CONTROL = @"~\RotationPackages\UserControl/AddNewRequirementPackage.ascx";
        public const String MANAGE_REQUIREMENT_PACKAGE_CONTROL = @"~\RotationPackages\UserControl/ManageRequirementPackage.ascx";

        /// <summary>
        /// Requirement Verification Details screen constant.
        /// </summary>
        public const String REQUIREMENT_VERIFICATION_DETAIL_CONTROL = @"~\ClinicalRotation\UserControl/RequirementVerificationDetail.ascx";

        /// <summary>
        /// UAT - 1626 - New Requirement Verification Details screen constant.
        /// </summary>
        public const String REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL = @"~\ClinicalRotation\UserControl/RequirementDetails.ascx";

        public static String CLIENT_CONTACT_PROFILE_URL = "~/ClinicalRotation/ClientContactProfile.ascx";
        public static String INSTRCTR_PRECEPTR_ROTATION_SEARCH_URL = "~/ClinicalRotation/RotationStudentSearch.ascx";
        public static String AGENCY_USER_PROFILE_URL = "~/ClinicalRotation/AgencyUserProfile.ascx";

        public const String APPLICANT_REQUIREMENT_DATA_ENTRY = @"~\\ApplicantRotationRequirement\\UserControl\\RotationRequirementDataEntry.ascx";
        public const String APPLICANT_REQUIREMENT_ROTATIONS = "~\\ApplicantRotationRequirement\\UserControl\\RequirementRotations.ascx";
        public static String ROTATION_STUDENT_DETAIL_CONTROL = @"~\ProfileSharing\UserControl\RotationStudentDetail.ascx";
        public static String ROTATION_DETAILS_CONTROL = @"~\CommonControls\UserControl\RotationDetails.ascx";
        public static String View_AGENCY_JOB_POST_FOR_APPLICANT_CONTROL = @"~\AgencyJobBoard\\UserControls\\ViewAgencyJobPostForApplicant.ascx";
        public static String SHARED_USER_ROTATION_DETAILS_CONTROL = @"~\CommonControls\UserControl\SharedUserRotationDetails.ascx";
        public const String SHARED_USER_REQUIREMENT_DATA_ENTRY = @"~\\ApplicantRotationRequirement\\UserControl\\SharedUserRotationRequirementDataEntry.ascx";

        public const String APPLICANT_REQUIRED_DOCUMENTS = @"~\\ComplianceOperations\\UserControl\\RequiredDocuments.ascx";

        public const String ROTATION_DOCUMENTS_CONTROL = @"~\ClinicalRotation\UserControl\RotationDocuments.ascx";
        #endregion

        #region UAT-1218
        public const Int32 SHARED_USER_ORGANIZATION_ID = 1;
        public const String QUERY_STRING_CLIENTCONTACT_TOKEN = "ClientContactToken";
        public const String QUERY_STRING_USER_TYPE_CODE = "UserTypeCode";
        #endregion

        #region Clinical Rotation
        public const String SESSION_KEY_CLINICAL_ROTATION_SEARCH_DATA = "ClinicalRotationSearchData";

        public const String ROTATION_PACKAGE_WIZARD_BREADCRUMB_PREFIX = "Rotation Package > ";
        public const String SHARED_ROTATION_CONTROL_USE_TYPE_CODE = "SRCUTC";
        public const String ROTATION_MEMBER_SEARCH_USE_TYPE_CODE = "RMSUTC";
        public const String ROTATION_PORTFOLIO_SEARCH_USE_TYPE_CODE = "RPSUTC";

        public const String COMPNEHENSIVE_SEARCH_SEARCH_USE_TYPE_CODE = "CSSSUTC";
        public const String SUPPORT_PORTAL_DETAIL_USE_TYPE_CODE = "SPDUTC";
        #endregion

        public const String SHARED_USER_SYSX_BLOCK_CODE = "BCSUT";
        public const String NON_BREAKING_SPACE = "&nbsp;";
        public const String ASSIGN_ROTATION_VERIFICATION_QUEUE_TYPE_CODE = "ARVQ";
        public const String ROTATION_VERIFICATION_USER_WORK__QUEUE_TYPE_CODE = "RVUWQ";

        //UAt-3528
        public const String ROTATION_VERIFICATION_QUEUE = "ROVEQ";

        //UAT-4248
        public const String SUPPORT_PORTAL_DETAIL_INSTRUCTOR_USE_TYPE_CODE = "SPDIUTC";

        #region UAT:-3049

        public const String ROTATION_DETAIL_USE_TYPE_CODE = "RDUTC";
        #endregion
        #region UAT 1409
        public const String SURR_ROTATION_REQ_MSG = "Please select a rotation.";
        public const String SURR_REVIEW_STATUS_REQ_MSG = "Please select a review status.";
        public const String SURR_SUCCESS_MSG = "Review Status of selected Rotation(s) saved successfully.";
        #endregion

        public const String CREDIT_CARD_AGREEMENT_STATEMENT_APPCONFIGKEY = "CreditCardAgreementStatement";

        #region UAT 1349 Pending changes for Agency User Dashboard
        public static String SHARED_USER_NAVIGATION_CONTROL = @"~\CommonControls\UserControl\SharedUserBreadCrumb.ascx";
        public static String MANAGE_AGENCY_USERS = @"~\ProfileSharing\ManageAgencyUsers.ascx";
        public static String MANAGE_ROTATION_PACKAGE = @"~\RotationPackages\ManageRotationPackage.ascx";
        public static String AGENCY_APPLICANT_STATUS = @"~\ProfileSharing\ViewAgencyApplicantStatus.ascx";

        public const String AGENCY_APPLICANT_STATUS_KEY = "AGENCYAPPLICANTSTATUS";
        //UAT-2706
        public static String REQUIREMENT_PACKAGE_VIEW = @"~\RotationPackages\RequirementPackages.ascx";
        //UAT-2427
        public static String JOB_BOARD = @"~\AgencyJobBoard\ManageAgencyJobs.ascx";
        #endregion

        #region UAT-2367
        public const String AGENCY_USER_MAPPING_MAIL_SUCCESS_MESSAGE = "Agency activation email has been sent successfully.";
        public const String AGENCY_USER_MAPPING_MAIL_FAIL_MESSAGE = "Some error occured while re-sending activation link mail. Please try again.";
        #endregion

        #region UAT-1544: WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.
        public const String APPLICANT_PRINT_DOCUMENT_TYPE = "ApplicantPrintDocument";
        public static string NO_AGENCYUSER_MESSAGE = "Profile cannot be shared until a user is created for '{0}' agency. Please complete the form below to send a request to American DataBank for the user you would like added to this agency.";
        #endregion

        #region UAT-2774
        public const String SHARED_USER_INVITATION_DOCUMENT = "SharedUserInvitationDocument";
        #endregion

        #region Constants for Applicant Data Entry Tree View Nodes

        /// <summary>
        /// Constant for Optional Category Section header in Applicant Data Entry - UAT 1265
        /// </summary>
        public const String DATA_ENTRY_OPTIONAL_CATEGORY_NODE = "OPTCAT";

        /// <summary>
        /// Constant for Required Category Section header in Applicant Data Entry - UAT 1265
        /// </summary>
        public const String DATA_ENTRY_REQUIRED_CATEGORY_NODE = "REQCAT";

        #endregion

        #region Shot Series Session Keys

        /// <summary>
        /// Constant for Session used to store the List of Items to be used for Mapping table generation 
        /// </summary>
        public const String SHOTSERIES_SESSION_LISTITEMCONTRACT = "lstSeriesItemContractData";

        /// <summary>
        /// Constant for Session used to store the List of Attributes of all the items in Mapping table 
        /// </summary>
        public const String SHOTSERIES_SESSION_LISTATTRIBUTESCONTRACT = "lstAttributes";

        #endregion

        //UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
        public static String MANAGE_ATTESTATION_STATEMENT = @"~\ProfileSharing\UserControl\ManageAttestationStatement.ascx";

        //UAT-3319: Ability for Agency Users to pull School Client Admin contact information
        public static String SCHOOL_REPRESENTATIVE_DETAIL = @"~\ProfileSharing\UserControl\SchoolRepresentativeDetails.ascx";

        #region UAT-1571: WB: increase the count of Alias and Location inputs to 25. I still don’t want this to be open ended.
        public const String DEFAULT_MAX_OCCURRENCE_KEY = "DefaultMaxOccurrence";
        #endregion

        #region UAT 1656
        public const String SURR_INVITATION_REQ_MSG = "Please select an invitation.";
        public const String SURR_INVITATION_REVIEW_SUCCESS_MSG = "Review Status of selected Invitation(s) saved successfully.";
        #endregion
        #region UAT-2943
        public const String SURR_DETAIL_INVITATION_REVIEW_SUCCESS_MSG = "Review Status saved successfully.";
        #endregion
        #region Changes related to short series 'usp_Rule_EvaluateAdjustItemSeriesRules' SP.
        public static String EVALUATE_ADJUST_ITEM_SERIES_RULE_SP_NAME = "usp_Rule_EvaluateAdjustItemSeriesRules";
        #endregion

        //UAT-1641
        public const String PROFILE_SHARING_URL_TYPE = "Type";
        public const String PROFILE_SHARING_URL_VERIFICATION_TOKEN = "Token";
        public const String PROFILE_SHARING_URL_TYPE_AGENCY_VERIFICATION = "ALV";
        public const String SESSION_KEY_ISAGENCY_MAPPED = "IsAgencyMapped";

        public const String STUDENT_BUCKET_ASSIGNEMNT_SESSION_KEY = "StudentBucketAssignmentContract";

        #region UAT-1812:Creation of an Approval/rejection summary for applicant logins.
        public const String SESSION_KEY_ISSINCE_YOUR_LAST_LOGIN_POPUP_VIEWED = "IsSinceYourLastLoginPopupViewed";
        #endregion
        #region UAT-2302-As a student, I should see a popup on login if i have logged in using a non-preferred browser.
        public const String PREFERRED_BROWSER_REGEX_FILEPATH = "~/Resources/Mod/CommonOperations/BrowserDetails/regexes.yaml";
        public const String SESSION_KEY_ISPREFERED_BROWSER_POPUP_VIEWED = "IsBrowserPreferPopupViewed";
        public const String SESSION_KEY_ALUMNI_POPUP_VIEWED = "AlumniPopupViewed";
        public const String PREFERRED_BROWSER_NAME_CHROME = "chrome";
        public const String PREFERRED_BROWSER_NAME_IE = "ie";
        public const String PREFERRED_BROWSER_NAME_INTERNET_EXPLORER = "internetexplorer";
        public const String PREFERRED_BROWSER_NAME_FIREFOX = "firefox";
        public const String PREFERRED_BROWSER_NAME_MOZILLA = "mozilla";
        public const String PREFERRED_BROWSER_NAME_SAFARI = "safari";
        #endregion

        #region UAT-1844:Phase 2 8: Agency User> Rotation tab and Other Tab
        public const String SCREEN_TYPE_ROTATION = "Rotation";
        public const String SCREEN_TYPE_INVITATION = "Invitation";
        public const String DIC_KEY_REVIEW_STATUS = "ReviewStatus";
        public const String DIC_KEY_NOTES = "Notes";
        public const String DIC_KEY_SCREEN_TYPE = "ScreenType";

        #endregion

        /// <summary>
        /// UAT-1834, NYU Migration 2 of 3: Applicant Complete Order Process.
        /// </summary>
        public const String IS_BULK_ORDER_DISSMISSED = "IsBulkOrderDissmissed";
        public const String BULK_ORDER_UPLOAD_DATA = "BulkOrderUploadData";

        #region CommonTypeIDs

        public const String ROTATION_ID = "RotationID";
        public const String PROFILE_SHARING_INVITATION_GROUP_ID = "ProfileSharingInviationGroupID";
        public const String AGENCY_USER_ID = "AgenycUserID";
        public const String PROFILE_SHARING_INVITATION_ID = "InvitationID";
        public const String ONLY_UNSIGNED_EXCEL = "OnlyUnSignedExcel";
        public const String IGNORE_AGENCY_USER_CHECK = "IgnoreAgencyUserCheck";
        public const String IS_ADMIN = "IsAdmin";
        public const String AGENCY_ID = "AgencyID";
        public static int BULK_ORDER_DATA;
        #endregion

        #region UAT-2117:"Continue" button behavior
        public const String IS_SUCCESS_INDICATOR_APPLICABLE = "IsSuccessIndicatorApplicable";
        public const String IS_ALL_EXISTING_SEARCHES_ARE_CLEAR = "IsAllExistingSearchesAreClear";
        public const String INSTITUTION_COLOR_FLAG_ID = "InstitutionColorStatusID";
        public const String IS_OTHER_SERVICE_GROUPS_ARE_COMPLETED = "IsOtherServiceGroupsAreCompleted";
        public const String SOURCE_SCREEN_NAME = "SourceScreenName";
        public const String MENU_ID = "MenuID";
        public const String ORDER_DETAIL_FOR_SERVICE_ITEM_SUPPLEMENT = "OrderDetailForServiceItemSupplement";
        #endregion

        public const String CUSTOM_FORM_FOR_SERVICE_ITEM_SUPPLEMENT = "CustomFormForServiceItemSupplement";

        #region UAT-2169:Send Middle Name and Email address to clearstar in Complio

        public const String NO_MIDDLE_NAME_TEXT_KEY = "NoMiddleNameText";
        #endregion

        #region Constants for Requirement Categories Tree View Nodes

        /// <summary>
        /// Constant for Optional Category Section header  - UAT 2165
        /// </summary>
        public const String DATA_ENTRY_OPTIONAL_REQUIREMENT_CATEGORY_NODE = "OPTRCAT";

        /// <summary>
        /// Constant for Required Category Section header- UAT 2165
        /// </summary>
        public const String DATA_ENTRY_REQUIRED_REQUIREMENT_CATEGORY_NODE = "REQRCAT";

        #endregion

        //UAT-2245
        public const String WITHOUT_TRACKING_PACKAGE_SHARING_NOT_COMOPLETED = "Please select Tracking Package to complete this sharing.";

        //UAT-2215
        public const String WITHOUT_TRACKING_PACKAGE_SHARING_NOT_COMOPLETED_DISABLED_BUTTON_TOOLTIP = "You must be compliant in order to share this package for the selected Agency.";

        //UAT-2319
        public const String Background_Data_Sync_In_Progress = "BackgroundDataSyncInProgress";

        //UAT-2310
        public const String Automatic_Items_Assign_To_Admin_In_Progress = "AutomaticItemsAssignToAdminInProgress";
        public const String Automatic_Items_Assign_To_Admin_Error_Message = "Some problem has occured please contact administrator.";
        public const String Automatic_Items_Assign_To_Admin_In_Progress_Message = "Item(s) Automatic assigning is already in progress,so please try again after some time.";
        public const String Automatic_Items_Assign_To_Admin_Initiation_Process_Message = "Item(s) have been sent for Auto Assignment. This may take a few minutes to complete.";

        //UAT-2411
        public const String INSTITUTION_CONFIGURATION_BUNDLE_PACKAGE_TYPE = "Bundle";

        //UAT-2444
        public const String CONFIRMATION_TEXT_FOR_COMPLIANCE_PACKAGE = "Are you sure you want to cancel this order? Cancellations cannot be undone.";
        public const String CONFIRMATION_TEXT_FOR_COMPLIANCE_AND_BACKGROUND_PACKAGE = "Are you sure you want to cancel this order? Cancellations cannot be undone. Please note, this order includes a tracking package and a background check. The background check is unable to be cancelled and will be conducted in its entirety.";

        //UAT-2494, New Account verification enhancements (additional verification step)
        public const String ADDITIONAL_ACCOUNT_VERIFICATION_PAGE = "~/AdditionalAccountVerification.aspx";
        //UAT-2463
        public const String REQUIREMENT_SHARES_SCREEN_NAME = "RequirementShares";

        //UAT-2544
        public const String APPLICANT_DROPPED_STATUS = "Dropped";

        //UAT-2431
        public const String APPLICANT_DOC_UPLOAD_CONFIRMATION_DEFAULT_MSG = "Please return to the Data Entry Screen to complete your requirements.";

        //UAT-3367
        public const String VIEW_AGENCY_APPLICANT_SHARE_HISTORY = "ViewAgencyApplicantShareHistory";
        //UAT-4006
        public const String REQUIREMENT_NON_COMPLIANT_SEARCH = @"~\ProfileSharing\UserControl\RequirementNonCompliantSearch.ascx";


        #region Manage Agency Node
        //UAT-2629
        public const String TITLE_MANAGE_AGENCY_NODE = "Manage Agency Node";
        #endregion

        public const String CHANGE_TYPE_PACKAGE = "Package";
        public const String CHANGE_TYPE_ALL = "All";
        public const String CHANGE_TYPE_AGENCYUSER = "AgencyUser";
        public const String CHANGE_TYPE_SCHOOL_HIERARCHY = "SchoolHierarchy";
        public const String CHANGE_TYPE_DOCUMENT = "Document";
        public const String ATTESTATION_FORM = "AttestationForm";
        public const String CHANGE_TYPE_AGENCY_HIERARCHY_SETTING = "Setting";

        //UAT-2519
        public const String SESSION_KEY_FILTERED_REQUIREMENT_SHARES = "FilteredRequirementShares";
        public const String IS_PROFILE_SHARE_SEARCH = "IsProfileShareSearch";
        public const String IS_ROTATION_SHARE_SEARCH = "IsRotationShareSearch";

        public const String VIEW_AGENCY_JOB_POST_DETAIL = @"~\AgencyJobBoard\UserControls\ViewAgencyJobPost.ascx";
        public const String VIEW_AGENCY_JOB_POST = @"~\AgencyJobBoard\ViewAgencyJobPosting.ascx";
        public const String VIEW_AGENCY_JOB_POST_FOR_APPLICANT = @"AgencyJobBoard\ViewAgencyJobPostingForApplicant.ascx";

        public const String PEOPLE_SOFT_ID = "PeopleSoftID";
        public const String NET_ID = "NetID";
        public const String STUDENT_ID = "StudentId";//UAT-2883
        public const String MAPPING_GROUP_CODE_UCONN = "CEECAFE2-2479-4DE8-A883-25F0A216814F";
        public const String MAPPING_GROUP_CODE_WGU = "1CC2F4DF-37A7-48F6-B183-331F57BABF01";//UAT-2883
        public const String SHIBBOLETH_UCONN = "shibboleth uconn";//UAT-2883
        public const String SHIBBOLETH_WGU = "shibboleth wgu";//UAT-2883
        public const String SESSION_HOST = "Session_Host";//UAT-2883
        public const String IS_SHIBBOLETH_HISTORY_TO_BE_LOGGED = "IsShibbolethHistoryToBeLogged";

        //UAT-3272
        public const String MAPPING_GROUP_CODE_UPENN = "06D805CF-422A-41C7-AB83-3569FD245051";
        public const String SHIBBOLETH_UPENN = "shibboleth upenn";

        //UAT-3540
        public const String MAPPING_GROUP_CODE_NYU = "8BCD837B-4359-457B-8C5E-A0789932B28E";
        public const String SHIBBOLETH_NYU = "shibboleth nyu";

        //Release 175
        public const String MAPPING_GROUP_CODE_NSC = "20E24285-434A-4F4E-BB16-A446A470A8FF";
        public const String SHIBBOLETH_NSC = "shibboleth nsc";

        //Release 181: Ross
        public const String MAPPING_GROUP_CODE_ROSS = "A8E87DCE-423F-4B8D-80CA-A7098BCE65C6";
        public const String SHIBBOLETH_ROSS = "shibboleth ross";


        //UPENN DENTAL SSO)
        public const String MAPPING_GROUP_CODE_UPENN_DENTAL = "21bea1df-a191-4510-9a79-4cf2fd9d8535";
        public const String SHIBBOLETH_UPENN_DENTAL = "shibboleth upenn dental";

        //Ball State SSO
        public const String MAPPING_GROUP_CODE_BALL_STATE = "5D0B4952-C485-4C76-B837-59FABADABABF";
        public const String SHIBBOLETH_BALL_STATE = "shibboleth bsu";

        public const String PROFILE_SHARING_TRACKING_PERMISSION_CODE = "AAAK";
        public const String PROFILE_SHARING_BKG_PERMISSION_CODE = "AAAL";
        public const String PROFILE_SHARING_ROT_PERMISSION_CODE = "AAAM";

        public static String APPLICANT_PARKING_PAYMENT_CONTROL = @"~\ComplianceOperations\UserControl\ItemPayment.ascx";

        #region 2753 Package Subscription Status
        public const String REQUIREMENT_PACKAGE_SUBSCRIPTION_APPROVED_CODE = "AAAC";
        public const String REQUIREMENT_PACKAGE_SUBSCRIPTION_PENDING_REVIEW_CODE = "AAAB";
        #endregion
        //AAAL
        //UAT-2696
        public const String UNIQUE_NAME_CUSTOM_ATTRIBUTE_GRD = "CustomAttributesGrd";

        #region 2788
        public const String REQUIREMENT_FIELD_ATTRIBUTE_TYPE_MANUAL_CODE = "CATMANUL";
        public const String REQUIREMENT_FIELD_ATTRIBUTE_TYPE_CALCULATED_CODE = "CATCALCU";
        #endregion

        #region UAT-2842-- Admin Create Screening Order
        public const String ADMIN_CREATE_ORDER_SEARCH = "AdminCreateOrderSearch";
        public const String ADMIN_CREATE_ORDER = @"~/BkgOperations/AdminCreateOrderSearch.ascx";
        #endregion

        public const String MANAGE_APPLICANT_USER_GROUP_MAPPING_SEARCH_FILTER_ERROR = " Please select appropriate User Group from \"Assign To User Group\"  drop down list."; //UAT-2535

        #region UAT-2971
        public const String SUPPORT_PORTAL_SEARCH = "SupportPortalSearch";
        public const String SUPPORT_PORTAL = @"/SearchUI/SupportPortalSearch.ascx";
        public const String SUPPORT_PORTAL_DETAIL = @"~/SearchUI/UserControl/SupportPortalDetails.ascx";
        public const String APPLICANT_ORGANIZATION_USER_ID = "ApplicantOrgUserID";
        public const String APPLICANT_GUID = "ApplicantUserID";
        #endregion

        #region UAT-2942
        public const String APPLICANT_INVITATION_SOURCE_TYPE_CODE = "AAAC";
        #endregion

        #region UAT:3041 New flag/setting for if rotation details should be editable by client admins/agency users or not

        public const String IS_EDITABLE_BY_CLIENT_ADMIN = "IsEditableByClientAdmin";

        public const String IS_EDITABLE_BY_AGENCY_USER = "IsEditableByAgencyUser";

        #endregion

        //UAT-2960
        public const String QUERY_STRING_ALUMNI_TOKEN = "AlumniToken";
        public const String SESSION_KEY_FILTERED_ALUMNI_TOKEN = "AlumniTokenSession";

        //UAT-3321
        public const String USER_GUIDE_FOR_AGENCY_USER = "UserGuideForAgencyUser";

        #region UAT-3428
        public const String APP_SETTING_MOBILE_URL_PREFIX = "MobileUrlPrefix";
        #endregion

        public const String ACCOUNT_LINKING_SESSION_KEY = "AccountLinkingProfileContract"; //UAT-3360

        #region UAT-3675

        public const String LOCATION_IMAGE_FILE = "LocationImageFile";
        #endregion

        public const String ApplicantFingerPrintFile_IMAGE = "FingerPrintFileImage";


        #region UAT-3484: - finger print location and appointment setup

        public const String ADD_PRINT_SCAN_LOCATION = @"~\FingerPrintSetUp\AddPrintScanLocation.ascx";
        public const String FINGERPRINT_LOCATION_ENROLLER = @"~\FingerPrintSetUp\FingerPrintLocationEnroller.ascx";
        public const String FINGERPRINT_LOCATION_GENERAL = @"~\FingerPrintSetUp\FingerPrintLocationGeneral.ascx";
        public const String FINGERPRINT_LOCATION_DETAIL = @"~\FingerPrintSetUp\UserControl\FingerprintLocationDetails.ascx ";
        public const String LOCATION_SETUP_SESSION_KEY = "FingerPrintLocationSetupData";
        public const String FINGERPRINT_APPOINMENT_SLOT_CONFIGURATION = @"~\FingerPrintSetUp\AppointmentSlotConfiguration.ascx";
        public const String FINGERPRINT_APPOINMENT_SLOTS_DETAIL = @"~\FingerPrintSetUp\UserControl\AppointmentSlotsDetail.ascx";
        public const String APPOINTMENT_RESCHEDULER = @"~\FingerPrintSetUp\UserControl\AppointmentRescheduler";
        public const String FINGERPRINT_MANAGE_ORDER_FULFILLMENT_QUEUE_DETAIL = @"~\FingerPrintSetUp\UserControl\ManageOrderFulFillmentQueueDetail.ascx";
        public const String FINGERPRINT_MANAGE_ORDER_FULFILLMENT_QUEUE_ENROLLER = @"~\FingerPrintSetUp\ManageEnrollerOrderFulFillmentQueue.ascx";
        public const String FINGERPRINT_MANAGE_ORDER_FULFILLMENT_QUEUE_GENERAL = @"~\FingerPrintSetUp\ManageGeneralOrderFulFillmentQueue.ascx";
        public const String CASCADING_ATTRIBUTE_DETAILS = @"~\BkgSetup\UserControl\ManageCascadingAttributeDetails.ascx";
        public const String BACKGROUND_SERVICE_ATTRIBUTES = @"~\BkgSetup\ManageServiceAttribute.ascx";
        public const String FINGERPRINT_MANAGE_APPOINTMENT_ORDER_ENROLLER = @"~\FingerPrintSetUp\ManageEnrollerAppointmentOrder.ascx";
        public const String FINGERPRINT_MANAGE_APPOINTMENT_ORDER_GENERAL = @"~\FingerPrintSetUp\ManageGeneralAppointmentOrder.ascx";
        public const String FINGERPRINT_MANAGE_APPOINTMENT_ORDER_DETAIL = @"~\FingerPrintSetUp\UserControl\ManageAppointmentOrderDetail.ascx";
        public const String FINGERPRINT_MANAGE_HR_ADMIN_PERMISSION_DETAIL = @"~\FingerPrintSetUp\UserControl\ManageHrAdminPermissionDetails.ascx";
        public const String DEFAULT_ZIPCODE_LOCATION = "80014";
        public const String LOCATION_TENANT_FILE_LOCATION = "LocationTenantFileLocation";
        public const String FINGERPRINT_MANAGE_APPOINTMENT_ORDER_HR_Admin = @"~\FingerPrintSetUp\ManageHrAdminAppointmentOrder.ascx";
        public const String FINGERPRINT_MANAGE_ORDER_FULFILLMENT_QUEUE_HR_Admin = @"~\FingerPrintSetUp\ManageHrOrderFulFillmentQueue.ascx";
        public const String FINGERPRINT_MANAGE_HRADMIN_PERMISSION = @"~\FingerPrintSetUp\ManageHrAdminPermission.ascx";
        public const String FINGERPRINT_MANAGE_HRADMIN_PERMISSION_DETAIL = @"~\FingerPrintSetUp\UserControl\ManageHrAdminPermissionDetails.ascx";
       
        #endregion

        public const String DefaultSSN = "111111111";
        public const string LOCATION_ID_FOR_OUT_OF_STATE_APPOINTMENT = "LocationIdForOutOfStateAppointment";
        public const string LOCATION_ID_FOR_DOWNTOWN = "LocationIdForDowntown";

        #region UAT-3601
        public const String SCREENING_PACKAGE_HEADER_DEFAULT_LABEL = "Screening";
        //public const String SCREENING_PACKAGE_HEADER_LOCATION_SERVICE_DEFAULT_LABEL = "Order Selection";
        public const String ORDER_REVIEW_PACKAGE_DETAIL_HEADER_DEFAULT_LABEL = "Package Details";
        //public const String ORDER_REVIEW_PACKAGE_DETAIL_HEADER_LOCATION_SERVICE_DEFAULT_LABEL = "Order Selection Detail";
        public const String ORDER_REVIEW_CHANGE_PACKAGE_SELECTION_DEFAULT_LABEL = "Change Package Selection";
        //public const String ORDER_REVIEW_CHANGE_PACKAGE_SELECTION_LOCATION_SERVICE_DEFAULT_LABEL = "Change Order Selection";
        public const String PAYMENT_METHOD_PACKAGE_NAME_DEFAULT_LABEL = "Package Name";
        //public const String PAYMENT_METHOD_PACKAGE_NAME_LOCATION_SERVICE_DEFAULT_LABEL = "Order Selection";
        #endregion
        public const String DefaultBkgPackageTabText = "Background Screening";
        #region Login Page Configurable Consts
        public const string ControlParentPath = "~/Login/";
        #endregion
        #region Onsite Events
        public const String FINGERPRINT_ONSITE_EVENT_ADD_EDIT = @"~\FingerPrintSetUp\UserControl\AddEditOnsiteEvents.ascx ";
        public const String FINGERPRINT_ONSITE_EVENT_LIST = @"~\FingerPrintSetUp\UserControl\ManageOnsiteEvents.ascx ";
        #endregion

        public const String ORDER_DETAILS_JSON_RESULT = "##JsonResult##";

        #region TicketCenter
        //Ticket Centre Implementation
        public const String MANAGE_TICKETS = "ManageTickets";
        public const String DOCUMENT_TYPE_TICKET_CENTER = "TicketCenterDocument";
        #endregion

        public const String CREDIT_CARD_AGREEMENT_STATEMENT_IN_SPANISH_APPCONFIGKEY = "CreditCardAgreementStatementSpanish";
        public const string CBIBillingCode = "CBIBillingCode";
        public const string CBI_SERVICE_NAME = "CBI Print Scan Service";
        public const string FP_Card_SERVICE_NAME = "Fingerprint Card";
        public const string PP_Photo_SERVICE_NAME = "Passport Photo";
        public const String I_AM_IN_SPANISH = "Yo soy";
        public const String FROM_IN_SPANISH = "De";

        #region UAT-3820

        public const String RECEIVED_FROM_STUDENT_SERVICE_FORM_STATUS = "ReceivedfromStudentServiceFormStatus";
        public const String COUNTTO_SET_RECEIVED_FROM_STUDENT_SERVICE_FORM_STATUS = "CountToSetReceivedfromStudentServiceFormStatus";
        #endregion

        //UAT-3335
        public const String NOT_AVAILABLE = "Not Available";

        public const String CORE_ACCOUNT_LINKING_MAPPING_GROUP_CODE = "E8942083-67A7-416F-82F6-1088A751AD98";

        public const String SUFFIX_TYPE = "SuffixType";

        //UAT -4360
        public const String ReasonFingerprinted = "Reason Fingerprinted";
        public const String CBIUniqueID = "CBIUniqueID";
        public const String AcctName = "AcctNam (Literal)";
        public const String BillingORI = "BillingORI";
        //UAT-4454
        public const String MAPPING_GROUP_CODE_ACEMAPP = "9DF399E0-6D0C-403B-A9CE-6D4CE5FE030C";
        public const String MAPPING_GROUP_CODE_MCE = "DDDA8833-7DA1-43AF-B169-6C87CD769530";
        //UAT-4492
        public const String MAPPING_GROUP_CODE_INPLACE = "13E3EE46-4231-41FB-9936-C9EB8EF6625A";
        public const String MAPPING_GROUP_CODE_UPENN_DENTAL_UPPERCASE = "21BEA1DF-A191-4510-9A79-4CF2FD9D8535";

        #region UAT-4592
        public const String DISCLAIMER_DOCUMENT_OVERRIDE = "DisclaimerDocumentOverride";
        #endregion

        #region Admin Entry Portal

        public const String ADMIN_ENTRY_APPLICANT_LANDING_SCREEN = @"~\AdminEntryPortal\UserControl\AdminEntryApplicantLandingScreen.ascx";
        public const String ADMIN_ENTRY_APPLICANT_INFORMATION = @"~\AdminEntryPortal\UserControl\AdminEntryApplicantInformation.ascx";
        public const String ADMIN_ENTRY_REACT_DASHBOARD = "/Home";
        public const String NO_MIDDLE_NAME_TEXT_AEP = "-----";
        #endregion

        //UAT-4613
        public const String IN_PROCESS_AGENCY_FROM_STUDENT_SERVICE_FORM_STATUS = "RecieverEmailsIPAfromStudentServiceFormStatus";
        public const String BKG_SVC_FORM_STATUS_IN_PROCESS_AGENCY_NOTIFICATION_DAYS = "BkgSvcFormStatusInProcAgencyNotifDays";

        //UAT-4658
        public const String PERMISSION_TEMPLATE_VALIDATION_MESSAGE = "Permission Template cannot be deleted as it is mapped with agency user(s).";

        //UAT4710
        public const String Receiver_Email_Fingerprinting_ExceededTAT = "ReceiverEmailFingerprintingExceededTAT";
        public const String TENANT_ID = "TenantID";

        public const Int32 TwenteyFourHourMilliSec = 86400000;

        public const String Admin = "Admin";

        public const String SYSTEM_PROCESS_USERID = "SystemProcessUserId";


        #region Accessibility

        public const String ACCESSIBILITY_THEME = "Accessibility";

        public const String Login_DEFAULT_CSS_PATH = "~/Resources/Mod/shared/login.css";
        public const String Login_ACCESSIBILITY_CSS_PATH = "~/Resources/Mod/Accessibility/login.css";

        public const String Color_DEFAULT_THEMES_CSS_PATH = "~/Resources/Themes/Default/colors.css";
        public const String Color_DEFAULT_ACCESSIBILITY_THEMES_CSS_PATH = "~/Resources/Mod/Accessibility/Themes/Default/colors.css";

        public const String Core_DEFAULT_CSS_PATH = "~/Resources/Mod/Shared/public_pages/core.css";
        public const String Core_DEFAULT_ACCESSIBILITY_CSS_PATH = "~/Resources/Mod/Accessibility/public_pages/core.css";

        #endregion
    }

    /// <summary>
    /// Represents MessagingFolder
    /// </summary>
    /// <remarks></remarks>
    public sealed class MessagingFolder
    {
        #region Public

        /// <summary>
        /// Inbox
        /// </summary>
        public static String Inbox = "Inbox";

        /// <summary>
        /// Drafts
        /// </summary>
        public static String Drafts = "Drafts";


        /// <summary>
        /// SentEmail
        /// </summary>
        public static String SentEmail = "Sent Items";

        /// <summary>
        /// Trash
        /// </summary>
        public static String Trash = "Junk E-mail";

        /// <summary>
        /// FollowUp
        /// </summary>
        public static String FollowUp = "Follow Up";

        /// <summary>
        /// InboxID
        /// </summary>
        public static Int32 InboxId = 1;

        /// <summary>
        /// DraftsID
        /// </summary>
        public static Int32 DraftsId = 2;

        /// <summary>
        /// SentItemsID
        /// </summary>
        public static Int32 SentItemsId = 3;

        /// <summary>
        /// FollowUpID
        /// </summary>
        public static Int32 FollowUpId = 6;

        public const String MESSAGE_FILE_EXTENSION = "MessageFileExtension";

        /// <summary>
        /// BatchFileLocation
        /// </summary>
        public const String MESSAGE_FILE_LOCATION = "MessageFileLocation";

        public const String MESSAGE_FILE_NAME_SEPARATION_CHAR = "_";





        #endregion

        #region Private

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        private MessagingFolder()
        {
            // TODO: Methods implementation.
        }

        #endregion
    }


    #region MyRegion
    public sealed class Queue
    {
        /// <summary>
        /// Constant for QUERYSTRING_ARGUMENT
        /// </summary>
        public const String QUERYSTRING_ARGUMENT = "args";

        /// <summary>
        /// Constant for PageSource
        /// </summary>
        public const String PAGE_SOURCE = "PageSource";
    }
    #endregion

    #region ReceiptPlaceHolders
    public class ReceiptTemplatePlaceHolder
    {

        public const String InstitutionHierarchy = "##InstitutionHierarchy##";
        public const String OrderNumber = "##OrderNumber##";
        public const String TotalPrice = "##TotalPrice##";
        public const String PackageName = "##PackageName##";
        public const String PaymentType = "##PaymentType##";
        public const String Amount = "##Amount##";
        public const String FirstName = "##FirstName##";
        public const String MiddleName = "##MiddleName##";
        public const String LastName = "##LastName##";
        public const String Gender = "##Gender##";
        public const String DateOfBirth = "##DateOfBirth##";
        public const String EmailAddress = "##EmailAddress##";
        public const String PhoneNumber = "##PhoneNumber##";
        public const String Address = "##Address##";
        public const String City = "##City##";
        public const String Country = "##Country##";
        public const String Zip = "##Zip##";
        public const String PlaceOfBirth_Country = "##PlaceOfBirth(Country)##";
        public const String PlaceOfBirth_State = "##PlaceOfBirth(State)##";
        public const String CascadingStateCode = "##CascadingStateCode##";
        public const String Citizenship = "##Citizenship##";
        public const String Race = "##Race##";
        public const String EyeColor = "##EyeColor##";
        public const String HairColor = "##HairColor##";
        public const String HeightFeet = "##HeightFeet##";
        public const String HeightInches = "##HeightInches##";
        public const String Weight = "##Weight##";
        public const String ReasonFingerprinted = "##ReasonFingerprinted##";
        public const String CBIUniqueID = "##CBIUniqueID##";
        public const String DaycareLicense = "##DaycareLicense##";
        public const String AcctName = "##AcctNam##";
        public const String AcctAdress = "##AcctAdr##";
        public const String AcctCty = "##AcctCty##";
        public const String AcctState = "##AcctState##";
        public const String AcctZip = "##AcctZip##";
        public const String ReasonFingerprintedColoradoRevisedStatute = "##ReasonFingerprintedColoradoRevisedStatute##";
        public const String TotalFee = "##TotalFee##";
        public const String MailingAddressName = "##MailingAddressName##";
        public const String MailingAddress = "##MailingAddress##";
        public const String MailingAddressCity = "##MailingAddressCity##";
        public const String MailingAddressState = "##MailingAddressState##";
        public const String MailingAddressZipCode = "##MailingAddressZipCode##";
        public const String PaymentInstructionName = "##PaymentInstructionName##";
        public const String PaymentInstructionContent = "##PaymentInstructionContent##";
        public const String STATE = "##STATE##";
        public const String SSN = "##SSN##";
        public const String ShowSSN = "##ShowSSN##";
        public const String ZipLabel = "##ZipLabel##";
        public const String ShowMailingAddress = "##ShowMailingAddress##";
        public const String ShowState = "##SHOWSTATE##";
        public const String EmployeeInfoSectionTitle = "##EmployeeInfoSectionTitle##";
        public const String ShowUserAgreement = "##ShowUserAgreement##";
        public const String UserAgreementLabel = "##UserAgreementLabel##";
        public const String UserAgreement = "##UserAgreement##";
        public const String PaymentTypes = "##PaymentTypes##";
        public const String ShowPaymentInstruction = "##ShowPaymentInstruction##";
        public const String PaymentInstructionLabel = "##PaymentInstructionLabel##";
    }
    #endregion

    #region Admin Entry Portal

    public class RedirectionSessionData
    {

        public const String ORGANIZATION_USER_ID = "OrganizationUserID";
        public const String USER_ID = "UserID";
        public const String SYSX_BLOCK_ID = "SysXBlockID";
        public const String SYSX_BLOCK_NAME = "SysXBlockName";
        public const String USER_GOOGLE_AUTHENTICATED = "UserGoogleAuthenticated";
        public const String SYSX_MEMBERSHIP_USER = "SysXMembershipUser";
        public const String IS_SYSX_ADMIN = "IsSysXAdmin";
        public const String BUSINESS_CHANNEL_TYPE = "BusinessChannelType";
        public const String TENANT_ID = "TenantID";
    }

    #endregion

}
