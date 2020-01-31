using System;
using System.Linq;
using INTSOF.Utils;
using System.Collections.Generic;

namespace INTSOF.Utils
{

    #region Messaging

    /// <summary>
    /// Enum for numbers.
    /// </summary>
    public enum DefaultNumbers
    {
        MinusOne = -1,
        None = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Eleven = 11,
        Twelve = 12,
        Thirteen = 13,
        Fourteen = 14,
        Fifteen = 15,
        Sixteen = 16,
        Seventeen = 17,
        Eighteen = 18,
        Nineteen = 19,
        Twenty = 20,
        Fifty = 50,
        HundredThree = 103
    }

    public enum SysXMessageFolder
    {
        Inbox = 1,
        Sent = 6,
        Drafts = 5,
        Junk = 2,
        Deleted = 3,
        Followup = 7
    }

    public enum MessageStatus
    {
        Unread = 1,
        Read = 27,
        Deleted = 3,
        Draft = 4,
        Archive = 5
    }

    public enum MessagingAction
    {
        NewMail = 1,
        Reply = 2,
        ReplyAll = 3,
        Forward = 4,
        Draft = 5,
        Template = 6,
        Detail = 7
    }

    #endregion

    #region Search

    /// <summary>
    /// Represents search type.
    /// </summary>
    public enum SearchViewMode
    {
        /// <summary>
        /// Represents Quick search.
        /// </summary>
        QuickSearch = 1,
        /// <summary>
        /// Represents Advance search.
        /// </summary>
        AdvanceSearch = 2
    }

    /// <summary>
    /// Represents search option type.
    /// </summary>
    public enum QuickSearchOption
    {
        /// <summary>
        /// Represents user search.
        /// </summary>
        User = 1,

        /// <summary>
        /// Represents client search.
        /// </summary>
        Client = 2,

        /// <summary>
        /// Represents supplier search.
        /// </summary>
        Supplier = 3,

        /// <summary>
        /// Represents asset search.
        /// </summary>
        Asset = 4,

        /// <summary>
        /// Represents Investor search.
        /// </summary>
        Investor = 5,

        /// <summary>
        /// Represents Insurer search.
        /// </summary>
        Insurer = 6,

        /// <summary>
        /// Represents Organization search.
        /// </summary>
        Organization = 7,

        /// <summary>
        /// Represents Product search.
        /// </summary>
        Product = 8,

        /// <summary>
        /// Represents Employee search.
        /// </summary>
        Employee = 9,

        /// <summary>
        /// Represents Item search.
        /// </summary>
        Item = 10,

        /// <summary>
        /// Represents Zone search.
        /// </summary>
        Zone = 11,

        /// <summary>
        /// Represents Service Request Search
        /// </summary>
        ServiceRequest = 12,
    }

    #endregion

    #region Global



    public enum TenantType
    {
        [StringValue("TTY000")]
        SELECT = 0,
        [StringValue("TTYCLI")]
        Institution = 1,
        [StringValue("TTYSUP")]
        Supplier = 2,
        [StringValue("TTYCOM")]
        Company = 3,
        [StringValue("TTYEMP")]
        Employee,
        [StringValue("TTYCMR")]
        Compliance_Reviewer
    }

    /// <summary>
    /// Enum to identify the type of User trying to login into application
    /// </summary>
    public enum UserType
    {
        [StringValue("AAAA")]
        SUPERADMIN,
        [StringValue("AAAB")]
        CLIENTADMIN,
        [StringValue("AAAC")]
        APPLICANT,
        [StringValue("AAAD")]
        THIRDPARTYADMIN,
        [StringValue("AAAE")]
        SHAREDUSER
    }

    public enum DatabaseExistsStatus
    {
        EXISTING,
        NO_CONNECTION,
        NO_DB
    }

    public enum TenantTypeEnum
    {
        None = 0,
        Client = 1,
        Supplier = 2,
        Company = 3
    }

    /// <summary>
    /// This enum is for view mode.
    /// </summary>
    /// <remarks></remarks>
    public enum ViewMode
    {
        // ----------------------------------------------
        // Enum								Value			
        // ----------------------------------------------
        Add = 1,
        Edit = 2,
        Search = 3,
        Notes = 4,
        Hoa = 5,
        Approval = 6,
        Queue = 7,
        Menu = 8,
        Pending = 9
    }



    /// <summary>
    /// This enum is for view mode.
    /// </summary>
    /// <remarks></remarks>
    public enum MessageType
    {
        // ----------------------------------------------
        // Enum								Value			
        // ----------------------------------------------
        Error = 1,
        Information = 2,
        SuccessMessage = 3
    }

    public enum TreeListViewTemplateColumnType
    {
        CheckBox = 1,
        RadioButton = 2
    }

    /// <summary>
    /// This enum is for status.
    /// </summary>
    public enum Status
    {
        // ----------------------------------------------
        // Enum								Value			
        // ----------------------------------------------
        Success,							//  0		
        UserError,	    					//  1		
        Other,								//  2		
        SystemException = 101,
        CustomException = 102
    }

    /// <summary>
    /// This enum is for severity.
    /// </summary>
    /// <remarks></remarks>
    public enum Severity
    {
        // ----------------------------------------------
        // Enum								Value			
        // ----------------------------------------------
        High = 1,
        Medium = 2,
        Low = 3,
        Warn = 4
    }



    /// <summary>
    /// Handles SysXHttpCodes.
    /// </summary>
    /// <remarks></remarks>
    public enum SysXHttpCodes
    {
        UserDoesntHaveAccessToTheRequestedResource = 0x193
    }



    public enum ContactType
    {
        [StringValue("NONE")]
        SELECT = 0,

        [StringValue("PPHN")]
        PrimaryPhone = 1,

        [StringValue("SPHN")]
        SecondaryPhone = 2,

        [StringValue("CPHN")]
        CellPhone = 3,

        [StringValue("CFAX")]
        Fax = 4,

        [StringValue("EMAL")]
        PrimaryEmail = 5,

        [StringValue("CVIO")]
        ForCodeViolations = 6,

        [StringValue("DOHA")]
        ForDoorHangers = 7,

        //Newly added as per UAT database on 23/05/2012.

        [StringValue("EMER")]
        EmergencyContact = 8,

        [StringValue("AHRS")]
        AfterHoursContact = 9,

        [StringValue("HRIK")]
        HighRisk = 10,

        [StringValue("BILL")]
        BillingContact = 11,

        //End.

        [StringValue("247C")]
        ContactInfo = 12,

        [StringValue("EXTN")]
        Ext = 13,

        [StringValue("WSIT")]
        Website = 14,

        [StringValue("PHON")]
        Phone = 15,

        [StringValue("MOB")]
        Mobile = 16,

        [StringValue("OFFC")]
        OfficePhone = 17,


        //Added for IRF General Info
        [StringValue("HOME")]
        Home = 39,

        [StringValue("BSNS")]
        Business = 40,

        [StringValue("OTHR")]
        Other = 41
    }

    /// <summary>
    /// Positions of tbe buttons inside command bar
    /// </summary>
    public enum CommandBarButtonsPosition
    {
        Left = 0, Center, Right
    }

    [Flags]
    public enum CommandBarButtons
    {
        None = 0x0,
        Save = 0x1,
        Cancel = 0x2,
        Submit = 0x4,
        Clear = 0x8,
        Extra = 0x10,
        Extended = 0x20
    }




    #endregion

    #region Approval/User Queue

    public enum QueuePriority
    {
        [StringValue("PRT001")]
        URGENT,

        [StringValue("PRT002")]
        VERYHIGH,

        [StringValue("PRT003")]
        HIGH,

        [StringValue("PRT004")]
        MEDIUM,

        [StringValue("PRT005")]
        LOW,

        [StringValue("PRT006")]
        NORMAL
    }

    public enum MyQueueView
    {
        SubContextDetail,
        UserQueueDetail
    }

    public enum PageSource
    {
        None,
        SupplierDashboard,
        ServiceOrderActivity,
        ApprovalQueue,
        SupplierQueue,
        SupplierActivityType,
        InspectionException,
        InspectionPrintResult,
        AssetEventsQueueSummary,
        ServiceRequestQueueSummary,
        ExceptionEsclation,
        ClientDashboard,
        BidsManagement,
        AccountingSignOffQueueSummary,
        SIgnOffAuth,
        PaymentReview,
        TransactionsSettlement,
        ManageSettlement
    }
    public enum AdminQueueContext
    {
        CalendarAdmin = 1,
        ACF = 2,
        Client = 3,
        ZoneMaintenance = 4,
        InvestorInsurer = 5,
        IRF = 6,
        QueueAdmin = 7,
        Others = 8,
        Employee = 9,
        SupplierIncident = 10,
        Accounting = 11,
        ServiceRequest = 12,
        ClientPortal = 13,
        BidEstimate = 14
    }
    public enum QueueStatus
    {
        [StringValue("QST000")]
        None,
        [StringValue("QSTDRF")]
        Draft,
        [StringValue("QSTPND")]
        Pending,
        [StringValue("QSTAPP")]
        Approved,
        [StringValue("QSTREJ")]
        Rejected,
        [StringValue("QSTOHD")]
        OnHold,
        [StringValue("QSTRTA")]
        ReadytoApprove
    }
    public enum AdminQueueSubContext
    {
        CalendarAdmin = 1,
        ACF = 2,
        Client = 3,
        ZoneMaintenance = 4,
        [StringValue("Investor Insurer Profile")]
        InvestorInsurer = 5,
        IRF = 6,
        QueueAdmin = 7,
        [StringValue("Investor Insurer Contact")]
        InvestorInsurerContact = 8,
        Employee = 9,
        Others = 10,
        ClientSOW = 34,
        ClientInspection = 12,
        ClientContacts = 13,
        ClientSubClient = 14,
        ClientLSCI = 15,
        ClientRate = 16,
        ClientMBACode = 17,
        ClientOtherService = 18,
        ClientServiceSchedule = 19,
        SupplierIncident = 20,
        AccountingSigningLimit = 21,
        AccountingTranCodeMaintenance = 22,
        [StringValue("Investor Insurer Guidelines")]
        InvestorInsurerGuideline = 23,
        Bundle = 24,
        ServiceRequest = 25,
        AccountingCheckRequest = 35,
        INLAppointmentEntry = 27,
        INLResultSubmit = 28,
        INLPreReview = 29,
        UnableToLocate = 30,
        ExtensionRequest = 31,
        DetermineReassignment = 32,
        ServiceItem = 33,
        AccountingClientDispute = 37,
        AccountingSupplierDispute = 36,
        ZoneMaintenanceDefineZone = 38,
        ZoneMaintenanceZoneGeography = 39,
        ZoneMaintenanceAssignSuppliers = 40,
        ZoneMaintenanceItemCostAdjustment = 41,
        ZoneMaintenanceZoneGeoViewState = 42,
        EyeballBidEstimate = 43,
        AccountingSigningAuthority = 44,
        ItemizedBid = 45,
        BidGroupEstimation = 46,
        BidEstimation = 47,
        ManualSupplierAssignment = 48,
        ClientREOFlatFee = 49,
        ClientFlatFeeSchedule = 50,
        ClientFrequentInspection = 51,
        ClientServiceBundle = 52,
        ServiceTypeModifierConfiguration = 53,
        ClientServiceInstruction = 54
    }

    public enum AdminQueueStatus
    {
        None = 0,
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Draft = 4
    }
    #endregion

    #region Client



    [Serializable()]
    public enum SysXClientContext
    {
        /// <summary>
        /// Client	
        /// </summary>
        [StringValue("CMSACTX001")]
        Client,
        /// <summary>
        /// Master Service Agreement
        /// </summary>
        [StringValue("CMSACTX002")]
        MasterServiceAgreement,
        /// <summary>
        /// Field Services Service Agreement
        /// </summary>
        [StringValue("CFSACTX003")]
        FieldServicesServiceAgreement,

        /// <summary>
        /// Agreement File	
        /// </summary>
        [StringValue("CMAFCTX004")]
        AgreementFile,

        /// <summary>
        /// Client Statement of Work
        /// </summary>
        [StringValue("CMSWCTX005")]
        ClientSOW,

        /// <summary>
        /// Client Inspection	
        /// </summary>
        [StringValue("CMINCTX006")]
        ClientInspection,

        /// <summary>
        /// Client Service Schedule	
        /// </summary>
        [StringValue("CMSACTX013")]
        ClientServiceSchedule,
        /// <summary>
        /// Client Contacts
        /// </summary>
        [StringValue("CMSACTX007")]
        ClientContacts,
        /// <summary>
        /// Client - Sub Clients
        /// </summary>
        [StringValue("CMSACTX008")]
        ClientSubClients,
        /// <summary>
        /// Client LSCI
        /// </summary>
        [StringValue("CMSACTX009")]
        ClientLSCI,
        /// <summary>
        /// Client Rate
        /// </summary>
        [StringValue("CMSACTX010")]
        ClientRate,
        /// <summary>
        /// Client MBA Code
        /// </summary>
        [StringValue("CMSACTX011")]
        ClientMBACode,

        /// <summary>
        /// Client Other Service
        /// </summary>
        [StringValue("CMSACTX012")]
        ClientOtherService,

        /// <summary>
        /// Client Service Item
        /// </summary>
        [StringValue("CMSACTX014")]
        ClientServiceItem,

        /// <summary>
        /// Client Service Item
        /// </summary>
        [StringValue("CMSACTX015")]
        ClientServiceBundle,

        /// <summary>
        /// Client Frequent Inspection
        /// </summary>
        [StringValue("CMSACTX016")]
        ClientFrequentInspection,

        /// <summary>
        /// Client REO Flat Fee
        /// </summary>
        [StringValue("CMSACTX017")]
        ClientREOFlatFee,

        /// <summary>
        /// Client Flat Fee Schedule
        /// </summary>
        [StringValue("CMSACTX018")]
        ClientFlatFeeSchedule,

        /// <summary>
        /// ServiceType Modifier Configurations
        /// </summary>
        [StringValue("CMSACTX019")]
        ServiceTypeModifierConfiguration,

        /// <summary>
        /// Client Service Instruction
        /// </summary>
        [StringValue("CMSACTX020")]
        ClientServiceInstruction,


        /// <summary>
        /// Client Accounting
        /// </summary>
        [StringValue("CMSACTX021")]
        ClientInvoicePlatform


    }



    #endregion

    #region Message
    public enum lkpMessageFolderContext
    {

        [StringValue("MSGPNF")]
        PERSONALFOLDERS,
        [StringValue("MSGINB")]
        INBOX,
        [StringValue("MSGINM")]
        INTERNALMESSAGES,
        [StringValue("MSGEML")]
        EMAILS,
        [StringValue("MSGSMS")]
        SMS,
        [StringValue("MSGNTI")]
        NOTIFICATIONS,
        [StringValue("MSGOTB")]
        OUTBOX,
        [StringValue("MSGSNT")]
        SENTITEMS,
        [StringValue("MSGDEL")]
        DELETEDITEMS,
        [StringValue("MSGNTE")]
        NOTES,
        [StringValue("MSGSRF")]
        SEARCHFOLDERS,
        [StringValue("MSGLBU")]
        LIBERTYUNIVERSITY,
        [StringValue("MSGAPM")]
        APPLICANTSMESSAGES,
        [StringValue("MSGLBA")]
        LIBERTYADMINS,
        [StringValue("MSGFSM")]
        FSUMAILS,
        [StringValue("MSGDRF")]
        DRAFTS,
        [StringValue("MSGFUP")]
        FOLLOWUP,
        [StringValue("MSGJNK")]
        JUNK
    }

    public enum MessageMode
    {
        [StringValue("D")]
        DRAFTMESSAGE,
        [StringValue("S")]
        SENDMESSAGE,
        [StringValue("T")]
        TEMPLATEMESSAGE
    }

    public enum MessageToolBarAction
    {
        [StringValue("SaveMessage")]
        SAVE,
        [StringValue("Send")]
        SEND,
    }
    #endregion

    #region Communication Types
    public enum lkpCommunicationTypeContext
    {
        [StringValue("CT00")]
        SELECT,
        [StringValue("CT01")]
        MESSAGE,
        [StringValue("CT02")]
        EMAIL,
        [StringValue("CT03")]
        SMS,
        [StringValue("CT04")]
        NOTIFICATION,
        [StringValue("CT05")]
        ALERTS,
        [StringValue("CT06")]
        REMINDERS
    }

    public enum lkpCommunicationEventContext
    {
        [StringValue("EVTODR")]
        ORDER,
        [StringValue("EVTSUB")]
        SUBSCRIPTIONS,
        [StringValue("EVTPFC")]
        PROFILE_CHANGES,
        [StringValue("EVTATS")]
        ACCOUNT_STATUS,
        [StringValue("EVTINM")]
        INTERNAL_MESSAGES,
        [StringValue("EVTSCE")]
        SCHEDULE_EVENTS,
        [StringValue("EVTCMN")]
        COMMON,
        [StringValue("EVTRTN")]
        ROTATION
    }

    //lkpNodeNotificationType
    public enum lkpNodeNotificationTypesContext
    {
        [StringValue("AAAA")]
        DEADLINE,
        [StringValue("AAAB")]
        NAGEMAILS
    }

    #endregion

    #region Sub Events

    public enum CommunicationSubEvents
    {
        [StringValue("NONE")] // Added this to manage the parameter when re-sending the mail through admin
        NONE,
        [StringValue("DEFAULTSVCFORM")]
        DEFAULT_SVC_FORM,
        [StringValue("NTAPPACCRO")]       
        APPLICANT_ACCOUNT_CREATION_OHSU,

        [StringValue("NTIODRCMO")]
        NOTIFICATION_ORDER_CREATION_MONEY_ORDER,
        [StringValue("NTIODRCIN")]
        NOTIFICATION_ORDER_CREATION_INVOICE,
        [StringValue("NTIODRA")]
        NOTIFICATION_ORDER_APPROVAL,
        [StringValue("NTIODRR")]
        NOTIFICATION_ORDER_REJECTION,
        [StringValue("NTIODRD")]
        NOTIFICATION_ORDER_COMPLETION,
        [StringValue("NTICRDT")]
        NOTIFICATION_CREDIT_CARD,
        [StringValue("NTIMNOD")]
        NOTIFICATION_MONEY_ORDER,
        [StringValue("NTIMBSP")]
        NOTIFICATION_MBS_PAYMENT,
        [StringValue("NTISBSN")]
        NOTIFICATION_NEW_SUBSCRIPTIONS,
        [StringValue("NTISBSE")]
        NOTIFICATION_EXPIRED_SUBSCRIPTIONS,
        [StringValue("NTISBSR")]
        NOTIFICATION_RENEWABLE_SUBSCRIPTION,
        [StringValue("NTIPRFC")]
        NOTIFICATION_PROFILE_CHANGE,
        [StringValue("NTIACTS")]
        NOTIFICATION_ACCOUNT_STATUS,
        [StringValue("NTIIMSG")]
        NOTIFICATION_INTERNAL_MESSAGES,
        [StringValue("NTIODRCNC")]
        NOTIFICATION_ORDER_CANCELLATION,
        [StringValue("NTIODRCNCA")]
        NOTIFICATION_ORDER_CANCELLATION_APPROVED,
        [StringValue("NTIODRCNCR")]
        NOTIFICATION_ORDER_CANCELLATION_REJECTED,
        [StringValue("NTICITME")]
        COMPLIANCE_ITEM_EXPIRED,
        [StringValue("NTIRSHOCNF")]
        NOTIFICATION_RUSH_ORDER_CONFIRMATION,
        [StringValue("ALTODRD")]
        ALERT_ORDER_DENIED,
        [StringValue("ALTSBSE")]
        ALERT_SUBSCRIPTION_EXPIRE,
        [StringValue("ALTPRFC")]
        ALERT_PROFILE_CHANGE,
        [StringValue("ALTACTS")]
        ALERT_ACCOUNT_STATUS,
        [StringValue("ALTIMSG")]
        ALERT_INTERNAL_MESSAGES,
        [StringValue("ALTSCHE")]
        ALERT_START_OF_EVENT,
        [StringValue("ALTSCHM")]
        ALERT_EVENT_MODIFICATIONS,
        [StringValue("RMRSBSE")]
        REMINDER_SUBSCRIPTION_EXPIRE,
        [StringValue("RMRIMSG")]
        REMINDER_INTERNAL_MESSAGES,
        [StringValue("RMRSBPND")]
        REMINDER_SUBSCRIPTION_PENDING,
        [StringValue("RMRCIE")]
        COMPLIANCE_ITEM_ABOUT_TO_EXPIRE,
        [StringValue("NTISBSSTSC")]
        NOTIFICATION_PACKAGE_SUBSCRIPTION_COMPLIANT_STATUS_CHANGE,
        [StringValue("NTIFPC")]
        NOTIFICATION_FOR_PROGRAMME_CHANGE,
        [StringValue("NTIFPCABP")]
        NOTIFICATION_FOR_PROGRAMME_CHANGE_AND_BALANCE_PAYMENT,
        [StringValue("NTEXCAPP")]
        NOTIFICATION_FOR_EXCEPTION_APPLIED,
        [StringValue("NTITMREJ")]
        NOTIFICATION_FOR_ITEM_REJECTED,
        [StringValue("NTFSTITMSB")]
        NOTIFICATION_FOR_FIRST_ITEM_SUBMISSION,
        [StringValue("NTRULCHNG")]
        NOTIFICATION_FOR_RULE_CHANGE,
        [StringValue("NTACCRAD")]
        NOTIFICATION_ACCOUNT_CREATION_ADMIN_USERS,
        [StringValue("NTACCRTP")]
        NOTIFICATION_ACCOUNT_CREATION_THIRDPARTY_USERS,
        [StringValue("NTDEADLN")]
        NOTIFICATION_FOR_DEADLINE,
        [StringValue("NTNAGEML")]
        NOTIFICATION_FOR_NAG_EMAIL,
        [StringValue("NTISBNC")]
        NOTIFICATION_PACKAGE_SUBSCRIPTION_NON_COMPLIANT_STATUS_CHANGE,
        [StringValue("NTAPPACCR")]
        NOTIFICATION_ACCOUNT_CREATION_APPLICANT,
        [StringValue("NTPWDRCVR")]
        NOTIFICATION_FORGET_PASSWORD_RECOVER,
        [StringValue("NTPSWRSTAD")]
        NOTIFICATION_PASSWORD_RESET_BY_ADMIN,
        [StringValue("NTFGTUNRST")]
        NOTIFICATION_FORGET_USERNAME_RESET,
        [StringValue("NTAPPINTC")]
        NOTIFICATION_APPLICANT_INSTITUTION_CHANGE,
        [StringValue("ALTEMLADDC")]
        ALERT_APPLICANT_EMAIL_ADDRESS_CHANGE,
        [StringValue("NTEMLADDC")]
        NOTIFICATION_APPLICANT_EMAIL_ADDRESS_CHANGE,
        [StringValue("NTVCEMADDC")]
        NOTIFICATION_VERIFICATION_CODE_APPLICANT_EMAIL_ADDRESS_CHANGE,
        [StringValue("NTVCUSNM")]
        NOTIFICATION_VERIFICATION_CODE_USERNAME,
        [StringValue("NTVCPSWD")]
        NOTIFICATION_VERIFICATION_CODE_PASSWORD,
        [StringValue("NTROLUP")]
        NOTIFICATION_ROLE_UPDATE,
        [StringValue("NTACTWLCM")]
        NOTIFICATION_NEW_ACCOUNT_CREATION,
        [StringValue("NTUPNCSHCC")]
        NOTIFICATION_NON_COMPLIANCE_SCHEDULE_CATEGORY,
        [StringValue("NTCACRSRCH")]
        NOTIFICATION_FOR_CANADA_NATIONAL_CRIMINAL_SEARCH,
        [StringValue("NTCOCABREG")]
        NOTIFICATION_FOR_CO_CHILD_ABUSE_REGISTRY,
        [StringValue("NTDCABRELF")]
        NOTIFICATION_FOR_DELAWARE_CHILD_ABUSE_RELEASE_FORM,
        [StringValue("NTFLAEFBIP")]
        NOTIFICATION_FOR_FL_AHCA_ELECTRONIC_FBI_PROCESSING,
        [StringValue("NTICABRELF")]
        NOTIFICATION_FOR_IOWA_CHILD_ABUSE_RELEASE_FORM,
        [StringValue("NTIDEPADAB")]
        NOTIFICATION_FOR_IOWA_DEP_ADULT_ABUSE,
        [StringValue("NTMOCRGVRF")]
        NOTIFICATION_FOR_MO_CAREGIVER_FORM,
        [StringValue("NTPACABREG")]
        NOTIFICATION_FOR_PA_CHILD_ABUSE_REGISTRY,
        [StringValue("NTSTMOCGBS")]
        NOTIFICATION_FOR_STL_CC_MO_CAREGIVER_BACKGROUND_SCREENING,
        [StringValue("NTSCMOCANR")]
        NOTIFICATION_FOR_STL_CC_MO_REQUEST_FOR_CHILD_ABUSE_NEGLECT_RECORD,
        [StringValue("NTTNFPTINS")]
        NOTIFICATION_FOR_TN_FINGERPRINT_INSTRUCTIONS,
        [StringValue("NTVACHDABU")]
        NOTIFICATION_FOR_VA_CHILD_ABUSE,
        [StringValue("NTCMORDRES")]
        NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS,
        [StringValue("NTCORWPDFA")]
        NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS_WITHOUT_PDF_ATTACHMENT,
        [StringValue("NTOCMOBKGP")]
        NOTIFICATION_FOR_ORDER_CREATION_MONEY_ORDER_AMSPACKAGES,
        [StringValue("NTOCINBKGP")]
        NOTIFICATION_FOR_ORDER_CREATION_INVOICE_AMSPACKAGES,
        [StringValue("NTOCMOBTHP")]
        NOTIFICATION_ORDER_CREATION_MONEY_ORDER_AMS_COMPLIO_PACKAGES,
        [StringValue("NTOCIBOTHP")]
        NOTIFICATION_ORDER_CREATION_INVOICE_AMS_COMPLIO_PACKAGES,
        [StringValue("NTAPPBOTHP")]
        NOTIFICATION_ORDER_APPROVAL_AMS_COMPLIO_PACKAGES,
        [StringValue("NTOAPPBKGP")]
        NOTIFICATION_ORDER_APPROVAL_AMS_PACKAGES,
        [StringValue("NTEDSINS")]
        NOTIFICATION_FOR_ELECTRONIC_DRUG_SCREENING_INSTRUCTIONS,
        [StringValue("NTCCOABKGP")]
        CREDIT_CARD_ORDER_APPROVAL_FOR_AMS_PACKAGES,
        [StringValue("NTCCOABTHP")]
        CREDIT_CARD_ORDER_APPROVAL_FOR_AMS_COMPLIO_PACKAGES,
        [StringValue("NTCCORDACP")]
        CREDIT_CARD_ORDER_APPROVAL_FOR_COMPLIO_PACKAGES,
        [StringValue("NTCCROCNF")]
        CREDIT_CARD_RUSH_ORDER_CONFIRMATION,
        [StringValue("NTCATEXAPP")]
        NOTIFICATION_FOR_CATEGORY_EXCEPTION_APPLIED,
        [StringValue("NTCATREJ")]
        NOTIFICATION_FOR_CATEGORY_REJECTED,
        [StringValue("NTCMPCATEX")]
        NOTIFICATION_FOR_COMPLIANCE_CATEGORY_EXPIRED,
        [StringValue("NTCTCABREG")]
        NOTIFICATION_FOR_CT_CHILD_ABUSE_REGISTRY_CHECK,
        [StringValue("NTCACRSRDS")]
        NOTIFICATION_FOR_CANADA_NATIONAL_CRIMINAL_SEARCH_FORM_DISPATCHED,
        [StringValue("NTDRSCALDS")]
        NOTIFICATION_FOR_DRUG_SCREENING_10_PANELS_ALCOHOL_FORM_DISPATCHED,
        [StringValue("NTDRSCPNDS")]
        NOTIFICATION_FOR_DRUG_SCREENING_10_PANELS_FORM_DISPATCHED,
        [StringValue("NTPLDRSCDS")]
        NOTIFICATION_FOR_PAPERLESS_DRUG_SCREEN_FORM_DISPATCHED,
        [StringValue("NTDRSCSPDS")]
        NOTIFICATION_FOR_DRUG_SCREENING_10_SPLIT_FORM_DISPATCHED,
        [StringValue("NTFBIFRMDS")]
        NOTIFICATION_FOR_FBI_FORM_DISPATCHED,
        [StringValue("NTFBIFPCDS")]
        NOTIFICATION_FOR_FBI_FINGERPRINT_CHECK_FORM_DISPATCHED,
        [StringValue("NTIOCHABDS")]
        NOTIFICATION_FOR_IOWA_CHILD_ABUSE_RELEASE_FORM_DISPATCHED,
        [StringValue("NTPACHABDS")]
        NOTIFICATION_FOR_PA_CHILD_ABUSE_REGISTRY_FORM_DISPATCHED,
        [StringValue("NTSMCHABDS")]
        NOTIFICATION_FOR_STL_CC_MO_REQUEST_FOR_CHILD_ABUSE_NEGLECT_RECORD_FORM_DISPATCHED,
        [StringValue("NTIOADABDS")]
        NOTIFICATION_FOR_IOWA_DEP_ADULT_ABUSE_FORM_DISPATCHED,
        [StringValue("NTDRSCPLDS")]
        NOTIFICATION_FOR_DRUG_SCREENING_10_PAPERLESS_FORM_DISPATCHED,
        [StringValue("NTDRSCPQDS")]
        NOTIFICATION_FOR_DRUG_SCREENING_10_PAPERLESS_Q_FORM_DISPATCHED,
        [StringValue("NTDLWACADS")]
        NOTIFICATION_FOR_DELAWARE_CHILD_ABUSE_RELEASE_FORM_DISPATCHED,
        [StringValue("NTVACAFRDS")]
        NOTIFICATION_FOR_VA_CHILD_ABUSE_FORM_DISPATCHED,
        [StringValue("NTDSPLLDS")]
        NOTIFICATION_FOR_DRUG_SCREENING_10_PAPERLESS_L_FORM_DISPATCHED,
        [StringValue("NTPADPEFDS")]
        NOTIFICATION_FOR_PA_DPW_ELECTRONIC_FINGERPRINT_FORM_DISPATCHED,
        [StringValue("NTTNFNINDS")]
        NOTIFICATION_FOR_TN_FINGERPRINT_INSTRUCTIONS_FORM_DISPATCHED,
        [StringValue("NTDSMDTSDS")]
        NOTIFICATION_FOR_DRUG_SCREENING_10_PANELS_MDA_TS_FORM_DISPATCHED,
        [StringValue("NTFLFBIPDS")]
        NOTIFICATION_FOR_FL_ELECTRONIC_FBI_PROCESSING_FORM_DISPATCHED,
        [StringValue("NTPARLFPDS")]
        NOTIFICATION_FOR_PA_DPW_ROLLED_FINGERPRINT_FORM_DISPATCHED,
        [StringValue("NTDRPLQNDS")]
        NOTIFICATION_FOR_DRUG_5_PL_Q_6405N_FORM_DISPATCHED,
        [StringValue("NTMOCAFRDS")]
        NOTIFICATION_FOR_MO_CAREGIVER_FORM_DISPATCHED,
        [StringValue("NTSCMCBSDS")]
        NOTIFICATION_FOR_STL_CC_MO_CAREGIVER_BACKGROUND_SCREENING_FORM_DISPATCHED,
        [StringValue("NTDSPLQNDS")]
        Notification_For_Drug_Screen_PL_Q_21832N_Form_Dispatched,
        [StringValue("NTCOCARGDS")]
        NOTIFICATION_FOR_CO_CHILD_ABUSE_REGISTRY_FORM_DISPATCHED,
        [StringValue("NTFLEFBIDS")]
        NOTIFICATION_FOR_FL_E_FBI_NONRESIDENT_FORM_DISPATCHED,
        [StringValue("NTCTCARGDS")]
        NOTIFICATION_FOR_CT_CHILD_ABUSE_REGISTRY_CHECK_FORM_DISPATCHED,
        [StringValue("NTFLAHEFDS")]
        NOTIFICATION_FOR_FL_AHCA_ELECTRONIC_FBI_PROCESSING_FORM_DISPATCHED,
        [StringValue("NTFBIFPSF")]
        NOTIFICATION_FOR_FBI_FINGERPRINT_SERVICE_FORM,
        [StringValue("NTMOCANGSF")]
        NOTIFICATION_FOR_MO_CHILD_ABUSE_NEGLECT,
        [StringValue("NTWGFLAHSF")]
        NOTIFICATION_FOR_WGU_FL_AHCA_FORM,
        [StringValue("NTFBIFPDS")]
        NOTIFICATION_FOR_FBI_FINGERPRINT_SERVICE_FORM_DISPATCHED,
        [StringValue("NTMOCANGDS")]
        NOTIFICATION_FOR_MO_CHILD_ABUSE_NEGLECT_FORM_DISPATCHED,
        [StringValue("NTWGFLAHDS")]
        NOTIFICATION_FOR_WGU_FL_AHCA_FORM_DISPATCHED,
        [StringValue("NTFLFBIPSF")]
        NOTIFICATION_FOR_VECHS_FL_ELECTRONIC_FBI_PROCESSING_FORM,
        [StringValue("NTCMSGRES")]
        NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS,
        [StringValue("NTCSGRWPA")]
        NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS_WITHOUT_PDF_ATTACHMENT,
        [StringValue("NTFLORDRES")]
        NOTIFICATION_FOR_FLAGGED_ORDER_RESULTS,
        [StringValue("NTCLRFLORD")]
        NOTIFICATION_FOR_BKG_ORDERS_WITH_COLOR_FLAG_AND_FLAG_STATUS,
        [StringValue("NTDLYCMPSG")]
        NOTIFICATION_FOR_DAILY_COMPLETED_SERVICE_GROUPS,
        [StringValue("NTPAODRCNC")]
        NOTIFICATION_FOR_PARTIAL_ORDER_CANCELLATION,
        [StringValue("NTAUAREXSU")]
        NOTIFICATION_FOR_DAILY_AUTO_ARCHIVED_PACKAGE_SUBSCRIPTIONS,
        [StringValue("NTEMPFLORD")]
        EMPLOYMENT_NOTIFICATION_FOR_FLAG_ORDER,
        [StringValue("NTFUCATCR")]
        NOTIFICATION_UPCOMING_CATEGORIES_COMPLIANCE_REQUIRED,
        [StringValue("NFPSS")]
        NOTIFICATION_FOR_PRINT_SCAN_SERVICE,
        [StringValue("NTRTNSTRT")]
        ROTATION_ABOUT_TO_START,
        [StringValue("NFIOO")]
        NOTIFICATION_FOR_INCOMPLETE_ONLINE_ORDERS,
        [StringValue("NTWKLYEDS")]
        WEEKLY_INCOMPLETE_DRUG_SCREEN_ORDERS_NOTIFICATION_AND_REPORT,
        [StringValue("EXTEMNF")]
        EXTERNAL_EMAIL_NOTIFICATION,
        [StringValue("NTFCOABTEX")]
        CONTRACT_ABOUT_TO_EXPIRE,
        [StringValue("NTFCOEXPIR")]
        CONTRACT_EXPIRED,
        [StringValue("NTAGUACC")]
        AGENCY_USER_ACCOUNT_CREATION,
        //Code commented for UAT-2541
        //UAT-1641
        //[StringValue("NTUAGMP")]
        //USER_AGENCY_MAPPING,
        [StringValue("NTAGSHINNR")]
        REQUIREMENTS_SHARING_INVITATION_NON_ROTATION,
        [StringValue("NTAGSHINWR")]
        REQUIREMENTS_SHARING_INVITATION_ROTATION_SHARING,
        [StringValue("NTADEDREJ")]
        NOTIFICATION_ADMIN_DATA_ENTRY_DOCUMENT_REJECTED,
        [StringValue("NTCFINST")]
        CONFIRMATION_FOR_INVITATION_SENT,
        [StringValue("NTEMPFLCSG")]
        EMPLOYMENT_NOTIFICATION_FOR_FLAGGED_COMPLETED_SERVICE_GROUP,
        [StringValue("NTWKLYEXC")]
        WEEKLY_SUBMITTED_EXCEPTION_REQUEST_NOTIFICATION_AND_REPORT,
        [StringValue("NTITMSUBRW")]
        NOTIFICATION_FOR_ITEM_SUBMITTED_FOR_REVIEW,
        [StringValue("NTRPCFCTNC")]
        NOTIFICATION_FOR_SUBSCRIPTION_CHANGED_FOR_COMPLIANT_TO_NON_COMPLIANT,
        [StringValue("NTFLCMSG")]
        NOTIFICATION_FOR_FLAGGED_COMPLETED_SERVICE_GROUP,
        [StringValue("NTCFINSTFR")]
        CONFIRMATION_FOR_ROTATION_INVITATION_SENT,
        [StringValue("NTRTSHRE")]
        NOTIFY_OF_ROTATION_SCHEDULE_AND_REQUIREMENTS,
        [StringValue("PDSVGRNT")]
        PENDING_SERVICE_GROUP_NOTIFICATION,
        [StringValue("NTFSTGRD")]
        NOTIFICATION_FOR_STUDENT_GRADUATED,
        [StringValue("NTFAPCO")]
        NOTIFICATION_FOR_APPLICANT_COMPLETE_ORDER,
        [StringValue("NTFRACO")]
        NOTIFICATION_FOR_REGISTER_ACCOUNT_COMPLETE_ORDER,
        [StringValue("NTFRMSF")]
        NOTIFICATION_FOR_RECEIVED_MANUAL_SERVICE_FORM,
        [StringValue("RTPKVRNT")]
        ROTATION_PACKAGE_VERSIONING_NOTIFICATION,
        [StringValue("NTFCSYSTEM")]
        SYSTEM_SUBEVENT,
        //UAT-2156: New Notification for students with Comm Copy setting for Form Dispatched (Manual Service Forms) .
        [StringValue("MSFDPNT")]
        Manual_Service_Form_Dispatched_Notification,
        [StringValue("NTADNLDOC")]
        Notification_For_Additional_Documents,
        //UAT-2073
        [StringValue("NTOODRCCC")]
        NOTIFICATION_ORDER_CREATION_CREDIT_CARD,
        //UAT-2370 : Supplement SSN Processing updates
        [StringValue("NTSSNTRQ")]
        NOTIFICATION_FOR_SSN_TRACE_RE_QUEUED,
        //UAT-2538
        [StringValue("NTRIAVRJ")]
        NOTIFICATION_FOR_ROTATION_INVITATION_APPROVAL_REJECTION,
        //UAT-2556:Separate email template for student shares that used agency dropdown from those that do not. 
        [StringValue("NTIPSWE")]
        NOTIFICATION_FOR_INDIVIDUAL_PROFILE_SHARING_WITH_EMAIL,
        [StringValue("NTFRTAP")]
        NOTIFICATION_FOR_REQUIREMENT_APPROVAL,
        //UAT-2628
        [StringValue("NTFADTMEG")]
        NOTIFICATION_FOR_FAILED_APPLICANT_DOCUMENT_MERGING,
        //UAT2753
        [StringValue("NFARCAOP")]
        NOTIFICATION_FOR_ALL_ROTATION_CATEGORIES_APPROVED_OR_PENDING_REVIEW,
        //UAT-2671
        [StringValue("NSFSCTIPTF")]
        NOTIFICATION_FOR_SVC_FORM_STATUS_CHANGED_TO_IN_PROCESS_TO_FBI,
        [StringValue("NFSFSCTRBF")]
        NOTIFICATION_FOR_SVC_FORM_STATUS_CHANGED_TO_REJECTED_BY_FBI,
        [StringValue("NTIAIPKG")]
        NOTIFICATION_FOR_ALL_AUTOMATIC_PACKAGE_INVITATION,
        //UAT-2905
        [StringValue("NFSIIRVQ")]
        NOTIFICATION_FOR_SUBMITTED_ITEM_INTO_REQUIREMENT_VERIFICATION_QUEUE,
        //UAT-2958
        [StringValue("NTAACSSO")]
        NOTIFICATION_ACCOUNT_CREATION_APPLICANT_THROUGH_SSO,
        //UAT-2970
        [StringValue("NTOCDCCO")]
        NOTIFICATION_ORDER_CONFIRMATION_DOCUMENT_FOR_CREDIT_CARD,
        //UAT-2907
        [StringValue("NTCRASGSMS")]
        NOTIFICATION_CLINICAL_ROTATION_ASSIGNED_SMS,
        //UAT-2977
        [StringValue("NTAUSFOC")]
        NOTIFICATION_TO_APPLICANT_UPON_STUDENT_FALL_OUT_OF_COMPLIANCE,
        //UAT-2930
        [StringValue("NTUSRTFA")]
        NOTIFICATION_FOR_TWO_FACTOR_AUTHENTICATION_DISABLED,
        //UAT-3059
        [StringValue("NTAUFUAR")]
        NOTIFICATION_TO_AGENCY_USER_FOR_UPDATED_APPLICANT_REQUIREMENTS,
        //UAT-3068
        [StringValue("NTFL2FASMS")]
        NOTIFICATION_FOR_LOGIN_VIA_2FA_SMS,
        [StringValue("ITMPMTNTF")]
        NOTIFICATION_FOR_ITEM_PAYMENT,
        //UAT-3097
        [StringValue("NRCFCNCSMS")]
        NOTIFICATION_FOR_SUBSCRIPTION_CHANGED_FOR_COMPLIANT_TO_NON_COMPLIANT_SMS,
        //UAT-3112
        [StringValue("NTFBDFAT")]
        NOTIFICATION_FOR_BADGE_FORM_ATTACHMENT,
        //UAT-2960
        [StringValue("NTFALACD")]
        NOTIFICATION_ALUMNI_ACCESS_DUE,
        [StringValue("NTFALPKGA")]
        NOTIFICATION_FOR_ALUMNI_PACKAGE_AVALIABILITY,
        //UAT-3108
        [StringValue("NTFACRFC")]
        NOTIFICATION_FOR_ROTATION_FIELD_CHANGES,
        //UAT-3222
        [StringValue("NTFSDFRT")]
        NOTIFICATION_FOR_STUDENT_DROPPED_FROM_ROTATION,
        [StringValue("NTAGUEMAC")]
        NOTIFICATION_FOR_AGENCY_USER_EMAIL_ADDRESS_CHANGES,
        [StringValue("CEATE")]
        COMPLIANCE_EXCEPTION_ABOUT_TO_EXPIRE,
        //UAT-3485
        [StringValue("NTFRMRIE")]
        REQUIREMENT_ITEM_ABOUT_TO_EXPIRE,
        //UAT-3273
        [StringValue("NTFAAIRBAC")]
        NOTIFICATION_TO_ADMINS_WHEN_ALL_APPLICANTS_IN_A_ROTATION_BECOMES_AGENCY_COMPLIANT,
        //UAT-3137
        [StringValue("NTFFRCGTBR")]
        NOTIFICATION_FOR_ROTATION_CATEGORY_GOING_TO_BE_REQUIRED,
        [StringValue("NTFFOACNG")]
        NOTIFICATION_FOR_FINGERPRINT_ORDER_APPOINTMENT_CHANGES,
        [StringValue("NTFFOAFX")]
        NOTIFICATION_FOR_FINGERPRINT_ORDER_APPOINTMENT_FIXED,
        //UAT-3669
        [StringValue("NIDWNRICFC")]
        IT_NOTIFICATION_EDS_REGISTRATION_ID_BLANK,
        //UAT-3627
        [StringValue("NTAUSFOCTP")]
        NOTIFICATION_TO_ADMIN_UPON_STUDENT_FALL_OUT_OF_COMPLIANCE_TRACKING_PACKAGE,
        //UAT-3675
        [StringValue("NTOCCBKGP")]
        NOTIFICATION_FOR_ORDER_CREATION_CASH_AMSPACKAGES,
        //UAT-3734
        [StringValue("NTMFAPP")]
        NOTIFICATION_FOR_MISSED_FINGERPRINT_APPOINTMENT,
        [StringValue("NTAFTCOFS")]
        NOTIFICATION_TO_APPLICANT_FOR_TEMPORARY_CLOSURE_OF_FINGERPRINT_SITE,
        [StringValue("NTAFPCOFS")]
        NOTIFICATION_TO_APPLICANT_FOR_PERMANENT_CLOSURE_OF_FINGERPRINT_SITE,
        [StringValue("NTAFEI")]
        NOTIFICATION_TO_APPLICANT_FOR_EVENT_INVITATION,
        [StringValue("NTAFRAOCA")]
        NOTIFICATION_TO_APPLICANTS_FOR_REFUND_AMOUNT_OF_CC_AND_PAYMENT_TYPE_REVERTED_TO_MONEY_ORDER,
        [StringValue("NFEAC")]
        NOTIFICATION_FOR_EVENT_APPOINTMENT_CONFIRMATION,
        [StringValue("NTFOSAF")]
        NOTIFICATION_FOR_OUTOFSTATE_APPOINTMENT_FIXED,
        [StringValue("NTAUFESCD")]
        NOTIFICATION_TO_AGENCY_USER_FOR_E_SIGN_COMPLETED_DOCUMENT,
        [StringValue("NFFFR")]
        NOTIFICATION_FOR_FINGERPRINT_FILE_REJECTED,
        [StringValue("NFFFSP")]
        NOTIFICATION_FOR_FINGERPRINT_FILE_SUCESSFULLY_PROCESSED,
        [StringValue("NFCOSFR")]
        NOTIFICATION_FOR_CBI_OUT_OF_STATE_FINGERPRINT_REJECTION,
        [StringValue("NFCFR")]
        NOTIFICATION_FOR_CBI_FILE_RICIEPT,
        [StringValue("NFFFFR")]
        NOTIFICATION_FOR_FINGERPRINT_FILE_FINALLY_REJECTED,
        [StringValue("NTPRFSSFS")]
        NOTIFICATION_FOR_PENDING_RECEIVED_FROM_STUDENT_SERVICE_FORM_STATUS,
        [StringValue("NTIFSCNCTC")]
        NOTIFICATION_TO_INSTRUCTOR_FOR_REQUIREMENT_PACKAGE_SUBSCRIPTION_TO_COMPLIANT,
        //UAT-3957
        [StringValue("NTRITMREJ")]
        NOTIFICATION_FOR_REQUIREMENT_ITEM_REJECTED,
        [StringValue("NFCFFR")]
        NOTIFICATION_FOR_CBI_FINGERPRINT_FILE_REJECTED,
        [StringValue("NFCFFSR")]
        NOTIFICATION_FOR_CBI_FINGERPRINT_FILE_SECOND_REJECTION,
        [StringValue("NFFOFSFR")]
        NOTIFICATION_FOR_FBI_OUT_OF_STATE_FINGERPRINT_REJECTION,
        [StringValue("NFFOFSFSR")]
        NOTIFICATION_FOR_FBI_OUT_OF_STATE_FINGERPRINT_SECOND_REJECTION,
        [StringValue("NFCOSFSR")]
        NOTIFICATION_FOR_CBI_OUT_OF_STATE_FINGERPRINT_SECOND_REJECTION,
        //UAT-3795
        [StringValue("NTWKLYNCA")]
        WEEKLY_NON_COMPLIANT_APPLICANT_REPORT_NOTIFICATION,
        [StringValue("NFFOOSMR")]
        NOTIFICATION_FOR_FINGERPRINT_OUT_OF_STATE_MANUAL_REJECTION,
        [StringValue("NFFASFES")]
        NOTIFICATION_FOR_FINGERPRINT_APPOINTMENT_SET_FOR_ENROLLER_SITE,
        //UAT-4398
        [StringValue("NTAUFRDAMD")]
        NOTIFICATION_TO_AGENCY_USER_FOR_ROTATIONDETAILS_AND_ROTATIONMEMBERSDETAILS,
        [StringValue("NTAFDO")]
        NOTIFICATION_TO_ADMIN_FOR_DRAFT_ORDERS,
        [StringValue("NTAIP")]
        NOTIFICATION_TO_APPLICANT_OF_INVITATION_PENDING,
        [StringValue("NTCAFAA")]
        NOTIFICATION_TO_CLIENT_ADMIN_FOR_AUTO_ARCHIVED,
        [StringValue("NTCAOOC")]
        NOTIFICATION_TO_CLIENT_ADMIN_OF_ORDER_COMPLETED,

        [StringValue("NTCAOBOCWA")]
        NOTIFICATION_TO_CLIENT_ADMIN_OF_BKG_ORDER_COMPLETED_WITH_ATTACHMENT,
        [StringValue("NTCAFCS")]
        NOTIFICATION_TO_CLIENT_ADMIN_FOR_CONFIRM_SUBMIT,
        [StringValue("NTFREDC")]
        NOTIFICATION_FOR_ROTATION_END_DATE_CHANGE, //UAT-4561
        [StringValue("NFSFSCRBFF")]
        NOTIFICATION_FOR_SVC_FORM_STATUS_CHANGED_TO_REJECTED_BY_FBI_FIRST, //UAT-3752
        [StringValue("NFSSRBFSTF")]
        NOTIFICATION_FOR_SVC_FORM_STATUS_CHANGED_TO_REJECTED_BY_FBI_SECOND_THIRD_FOURTH, //UAT3752
        [StringValue("NFSFSCINPA")]
        NOTIFICATION_FOR_SVC_FORM_STATUS_CHANGED_TO_IN_PROCESS_AGENCY_STATUS, //UAT4613
        [StringValue("NTWLYCMPSG")]
        NOTIFICATION_FOR_Weekly_COMPLETED_SERVICE_GROUPS, //UAT 4609
        [StringValue("NTAUUSFOC")] 
        NOTIFICATION_To_Agency_User_Upon_Student_Fall_Out_Of_Compliance, //UAT 4400
        [StringValue("NTAUSFTP")]
        NOTIFICATION_To_Agency_User_Upon_Student_Fall_Out_Of_Compliance_Tracking_Package, //UAT 4400
        [StringValue("NTFAEAI")]
        NOTIFICATION_FOR_ADMIN_ENTRY_APPLICANT_INVITE,
        [StringValue("NTIMSA")]
        NOTIFICATION_FOR_MODIFYING_SHIPPING_ADDRESS,      
        [StringValue("NTFSSRTS")]
        NOTIFICATION_FOR_SERVICE_STATUS_RETURN_TO_SENDER,
        [StringValue("NTFSSR")]
        NOTIFICATION_FOR_SERVICE_STATUS_REJECTED,
        [StringValue("NTFSSS")]
        NOTIFICATION_FOR_SERVICE_STATUS_SHIPPED,
        [StringValue("NFFPETR")]
        NOTIFICATION_FOR_FINGER_PRINTING_EXCEEDED_TAT //UAT4710
    }

    #endregion

    #region Content Type
    public enum ContentType
    {
        [StringValue("CTWEBPAGE")]
        WEBPAGE,
        [StringValue("CTHEADER")]
        HEADER,
        [StringValue("CTFOOTER")]
        FOOTER,
        [StringValue("CTAPPDEH")]
        APPLICANTDATAENTRYHELP,
        [StringValue("CTDISF")]
        DISCLOSUREFORM
    }
    #endregion

    #region Permission
    public enum Permissions
    {
        [StringValue("Full Access")]
        FullAccess,
        [StringValue("Read Only")]
        ReadOnly,
        [StringValue("No Access")]
        NoAccess
    }
    #endregion

    #region Role Type

    public enum RoleTypes
    {
        [StringValue("ADB Admin")]
        AAA,
        [StringValue("Client Admin")]
        AAB,
        [StringValue("Applicant")]
        AAC,
        [StringValue("Other")]
        AAD
    }

    public enum ComplianceScreenType
    {
        [StringValue("Admin")]
        Admin,
        [StringValue("Client")]
        Client
    }

    #endregion

    #region Compliance
    public enum ComplianceAttributeDatatypes
    {
        [StringValue("ADTDAT")]
        Date = 1,
        [StringValue("ADTTEX")]
        Text = 2,
        [StringValue("ADTOPT")]
        Options = 3,
        [StringValue("ADTNUM")]
        Numeric = 4,
        [StringValue("ADTFUP")]
        FileUpload = 5,
        [StringValue("ADTVWD")]
        View_Document = 6,
        [StringValue("ADTSDOC")]
        Screening_Document = 7,
        [StringValue("ADTSIGN")]
        Signature = 8
    }

    public enum DocumentFieldTypes
    {
        [StringValue("AAAA")]
        Signature,
        [StringValue("AAAB")]
        CurrentDate,
        [StringValue("AAAC")]
        FirstName
    }

    public enum ComplianceAttributeType
    {
        [StringValue("CATMANUL")]
        Manual = 1,
        [StringValue("CATCALCU")]
        Calculated = 2
    }

    public enum ComplianceItemDeletionStatus
    {
        ComplianceItemAssociated = 0,
        DeletedSuccessfully = 1,
        DeletionFailed = -1
    }

    public enum ClientCompliancePackageCopyType
    {
        CopyMyPackage = 1,
        CopyMasterPackage = 0
    }

    public enum ComplianceRuleConstantTypes
    {
        [StringValue("Text")]
        Text = 1,
        [StringValue("Number")]
        Number = 2,
        [StringValue("Date")]
        Date = 3,
        [StringValue("Timespan")]
        Timespan = 4
    }

    public enum ComplianceRuleSystemVariables
    {
        [StringValue("Current Date")]
        CurrentDate,
        [StringValue("Empty")]
        Empty
    }

    public enum ComplianceTimespanTypes
    {
        [StringValue("Days")]
        Days,
        [StringValue("Months")]
        Months,
        [StringValue("Years")]
        Years
    }

    public struct RuleSetTreeNodeType
    {
        public const String PackageLabel = "LPAK";
        public const String Package = "PAK";
        public const String CategoryLabel = "LCAT";
        public const String Category = "CAT";
        public const String ItemLabel = "LITM";
        public const String Item = "ITM";
        public const String AttributeLabel = "LATR";
        public const String Attribute = "ATR";
        public const String RuleSetLabel = "LRLS";
        public const String RuleSet = "RLS";
        public const String RuleLabel = "LRL";
        public const String Rule = "RL";

        public const String RuleObject = "RuleObject";
        public const String RuleMapping = "RuleMapping";
        public const String RuleMappingDetail = "RuleMappingDetail";
        public const String RuleTemplateExpression = "RuleTemplateExpression";
        public const String LKPObjectType = "LKPObjectType";
        public const String Department = "DEP";
        public const String Program = "PROG";
        public const String CompliancePackage = "PKG";
        public const String Subscription = "SUBS";
        public const String InstituteHierarchyNode = "IHN";
        public const String Contact = "CNTCT";
        public const String Service = "SRVC";
        public const String ServiceItem = "SITM";
        public const String ServiceFeeItem = "FITM";
        public const String PackageBundle = "PKGBND";

    }

    public struct ComplainceObjectType
    {
        public const String Package = "PKG";
        public const String Category = "CAT";
        public const String Item = "ITM";
        public const String Attribute = "ATR";
    }

    public enum LCObjectType
    {
        [StringValue("PKG")]
        CompliancePackage,
        [StringValue("CAT")]
        ComplianceCategory,
        [StringValue("ITM")]
        ComplianceItem,
        [StringValue("ATR")]
        ComplianceATR,
        [StringValue("RLS")]
        ComplianceRuleStep,
        [StringValue("RL")]
        ComplianceRule,
        [StringValue("EXP")]
        ComplianceExpression,
        [StringValue("PKGCAT")]
        CompliancePackageCategoryLink,
        [StringValue("CATITM")]
        ComplianceCategoryItemLink,
        [StringValue("ITMATR")]
        ComplianceItemAttributeLink,
        [StringValue("RLSRL")]
        RuleStepRuleLink,
        [StringValue("RLEXP")]
        RuleExpressionLink,
        [StringValue("RQCAT")]
        RequirementCategory
    }

    public enum LCContentType
    {
        [StringValue("LCT01")]
        ExplanatoryNotes,
        [StringValue("LCT02")]
        ExceptionDescription

    }

    public enum ComplianceRuleType
    {
        [StringValue("URLS")]
        UIRules,
        [StringValue("BRLS")]
        BuisnessRules
    }

    public enum ReviewerType
    {
        [StringValue("ADMN")]
        Admin,
        [StringValue("CADM")]
        Client_Admin
    }


    public struct LkpEditableBy
    {
        public const String Admin = "EDTADMN";
        public const String InstitutionAdmin = "EDTIADN";
        public const String Applicant = "EDTAPCT";
    }

    public struct LkpReviewerType
    {
        public const String Admin = "ADMN";
        public const String ClientAdmin = "CADM";
    }

    #endregion

    #region Applicant Compliance

    public enum DocumentStatus
    {
        [StringValue("AAAA")]
        MERGING_IN_PROGRESS,
        [StringValue("AAAB")]
        MERGING_FAILED,
        [StringValue("AAAC")]
        MERGING_COMPLETED,
        [StringValue("AAAD")]
        MERGING_COMPLETED_WITH_ERRORS
    }

    public enum ApplicantCategoryComplianceStatus
    {
        [StringValue("APRD")]
        Approved,
        [StringValue("INCM")]
        Incomplete,
        [StringValue("APWE")]
        Approved_With_Exception,
        [StringValue("PNDR")]
        Pending_Review,
    }

    public enum ApplicantCategoryExceptionStatus
    {
        [StringValue("AAAA")]
        EXCEPTION_APPLIED,
        [StringValue("AAAB")]
        EXCEPTION_REJECTED,
        [StringValue("AAAC")]
        EXCEPTION_EXPIRED,
        [StringValue("AAAD")]
        APPROVED_BY_OVERRIDE,
        [StringValue("AAAE")]
        EXCEPTION_APPROVED
    }

    public enum ApplicantItemComplianceStatus
    {
        [StringValue("INCM")]
        Incomplete,
        [StringValue("PNDG")]
        Pending_Review,
        [StringValue("APRD")]
        Approved,
        [StringValue("NAPD")]
        Not_Approved,
        [StringValue("EXPD")]
        Expired,
        [StringValue("AWAP")]
        Awaiting_Approval,
        [StringValue("APWE")]
        Approved_With_Exception,
        [StringValue("APFE")]
        Applied_For_Exception,
        [StringValue("EXAP")]
        Exception_Approved,
        [StringValue("EXRJ")]
        Exception_Rejected,
        [StringValue("PNDC")]
        Pending_Review_For_Client,
        [StringValue("PNDT")]
        Pending_Review_For_Third_Party




    }

    public enum ReconciliationMatchingStatus
    {
        [StringValue("AAAA")]
        Reviewed_Only,
        [StringValue("AAAB")]
        Matched,
        [StringValue("AAAC")]
        Not_Matched,
        [StringValue("AAAD")]
        Reviewed_Not_Submitted,
        [StringValue("AAAE")]
        Reviewe_OverRidden
    }

    public enum ApplicantPackageComplianceStatus
    {
        [StringValue("COMP")]
        Compliant,
        [StringValue("NCMP")]
        Not_Compliant
    }

    public enum defaultRole
    {
        [StringValue("DFRAPCNT")]
        Applicant,
        [StringValue("DFRSU")]
        SharedUser
    }

    public enum ObjectMappingType
    {
        [StringValue("COMPL")]
        Compliance_Value,
        [StringValue("DVAL")]
        Data_Value,
        [StringValue("CONST")]
        Defined_Value,
        [StringValue("SIS")]
        Series_Item_Status,
        [StringValue("SIA")]
        Series_Item_Attribute
    }

    public enum ObjectType
    {
        [StringValue("PKG")]
        Compliance_Package,
        [StringValue("CAT")]
        Compliance_Category,
        [StringValue("ITM")]
        Compliance_Item,
        [StringValue("ATR")]
        Compliance_ATR,
        [StringValue("FITM")]
        First_Series_Item,
        [StringValue("LITM")]
        Last_Series_Item,
        [StringValue("NWITM")]
        New_Series_Item,
        [StringValue("SITM")]
        Series_Item_Rule,
        [StringValue("PITM")]
        Previous_Series_Item,
        [StringValue("NSITM")]
        Next_Series_Item,
        [StringValue("SRS")]
        Series,
        [StringValue("SRITM")]
        Series_Item,
        [StringValue("RQITM")]
        Requirement_Item,
    }

    public enum RuleSetTreeType
    {
        [StringValue("PAK")]
        Packages,
        [StringValue("CAT")]
        Categories,
        [StringValue("ITM")]
        Items,
        [StringValue("ATR")]
        Attributes
    }

    public enum OperandType
    {
        [StringValue("Text")]
        Text,
        [StringValue("Numeric")]
        Numeric,
        [StringValue("Date")]
        Date,
        [StringValue("Boolean")]
        Boolean,
        [StringValue("TDAY")]
        TimeSpanDay,
        [StringValue("TMTH")]
        TimeSpanMnth,
        [StringValue("TYR")]
        TimeSpanYear,
        [StringValue("Country")]
        Country,
        [StringValue("County")]
        County,
        [StringValue("State")]
        State,
        [StringValue("City")]
        City,
        [StringValue("Zip Code")]
        ZipCode,
        [StringValue("Cascading")]
        Cascading,
        //[StringValue("DOB")]
        //DOB,
    }

    public enum UIValidationResultType
    {
        [StringValue("True")]
        True,
        [StringValue("False")]
        False,
        [StringValue("Error")]
        Error
    }

    public enum WorkQueueType
    {
        AssignmentWorkQueue = 1,
        UserWorkQueue,
        DataItemSearch,
        AssigneeDataItemSearch,
        ComplianceSearch,
        ExceptionAssignmentWorkQueue,
        ExceptionUserWorkQueue,
        EsclationAssignmentWorkQueue,
        EsclationUserWorkQueue,
        ComprehensiveSearch,
        ApplicantPortFolioSearch,
        ClientUserSearch,
        SupportPortalDetail,
        SupportPortalSearch,
        ApplicantPortFolioDetail,
        ReconciliationQueue,
        ReconciliationDetail,
        VerificationDetail
    }

    public enum CmpSearchDetailMode
    {
        DataEntry,
        Verification
    }

    public enum DataEntryReadOnlyMode
    {
        [StringValue("MobilitySwitched")]
        MobilitySwitched
    }

    public enum MobilityChangeScreenType
    {
        [StringValue("AutoTransition")]
        NodeTransitionApprovalQueue,
        [StringValue("AdminTransition")]
        AdminChangeProgramQueue
    }

    public enum PackageMappingStatus
    {
        [StringValue("AAAA")]
        Pending_Review,
        [StringValue("AAAB")]
        Reviewed,
        [StringValue("AAAC")]
        Reviewed_Instance,
        [StringValue("MNDF")] // This is not Defined in lookUp table
        MappingNotDefined,
    }

    /// <summary>
    /// Enum to track the Movement of Item from One user type to another
    /// </summary>
    public enum LkpItemMovementStatus
    {
        [StringValue("IVDREV")]
        VIA_VERIFICATION_FROM_INCOMPLETE_REVIEW_REQUIRED,
        [StringValue("IVDRNR")]
        VIA_VERIFICATION_FROM_INCOMPLETE_REVIEW_NOT_REQUIRED,
        [StringValue("PNGADM")]
        VIA_VERIFICATION_FROM_PENDING_REVIEW_FOR_ADMIN,
        [StringValue("PNGCNT")]
        VIA_VERIFICATION_FROM_PENDING_REVIEW_FOR_CLIENT_ADMIN,
        [StringValue("PNGTP")]
        VIA_VERIFICATION_FROM_PENDING_REVIEW_FOR_THIRD_PARTY,
        [StringValue("IVDIDE")]
        VIA_VERIFICATION_FROM_ITEM_DATA_EXPIRED,
        [StringValue("REJDR")]
        VIA_DATA_ENTRY_FROM_REJECTED,
        [StringValue("REJVD")]
        VIA_VERIFICATION_FROM_REJECTED,
        [StringValue("APPR")]
        VIA_VERIFICATION_FROM_APPROVED,
        [StringValue("EXPD")]
        VIA_EXPIRED,
        [StringValue("INCDE")]
        VIA_DATA_ENTRY_FROM_INCOMPLETE,
        [StringValue("APPDE")]
        VIA_DATA_ENTRY_FROM_APPROVED,
        [StringValue("PNDADE")]
        VIA_DATA_ENTRY_FROM_PENDING_REVIEW_FOR_ADMIN,
        [StringValue("PNDCDE")]
        VIA_DATA_ENTRY_FROM_PENDING_REVIEW_FOR_CLIENT_ADMIN,
        [StringValue("PNDTDE")]
        VIA_DATA_ENTRY_FROM_PENDING_REVIEW_FOR_THIRD_PARTY,
        [StringValue("VADE")]
        VIA_ADMIN_DATA_ENTRY,
        [StringValue("INCDNR")]
        VIA_DATA_ENTRY_FROM_INCOMPLETE_REVIEW_NOT_REQUIRED,
        [StringValue("QUZE")]
        VIA_QUIZ_EVALUATION
    }

    public enum ComplianceScreenModes
    {
        DATAENTRY,
        VERIFICATION
    }

    #endregion

    #region Exception Detail
    public enum Gender
    {
        [StringValue("Male")]
        Male = 1,
        [StringValue("Female")]
        Female = 2

    }

    public enum VerificationDataMode
    {
        [StringValue("REO")]
        ReadOnly,
        [StringValue("DEN")]
        DataEntry,
        [StringValue("EXC")]
        Exception
    }
    #endregion

    #region Website Management

    public enum CustomPageLinkPosition
    {
        Header = 1,
        Footer = 2
    }

    #endregion

    public class EnumList
    {
        public static IEnumerable<KeyValuePair<T, string>> Of<T>()
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(p => new KeyValuePair<T, string>(p, p.ToString()))
                .ToList();
        }
    }

    #region Applicant Order

    public enum ApplicantOrderStatus
    {
        [StringValue("OSPPA")]
        Pending_Payment_Approval = 1,
        [StringValue("OSPAD")]
        Paid = 2,
        [StringValue("OSCNL")]
        Cancelled = 3,
        [StringValue("OSCNR")]
        Cancellation_Requested = 4,
        [StringValue("OSONP")]
        Send_For_Online_Payment = 5,
        [StringValue("OSPRJ")]
        Payment_Rejected = 6,
        [StringValue("OSPDU")]
        Payment_Due = 7,
        [StringValue("OSPNC")]
        Online_Payment_Not_Completed = 8,
        [StringValue("OSPSA")]
        Pending_School_Approval = 9,
        [StringValue("OSPMS")]
        Modify_Shipping_Send_For_Online_Payment = 10
    }

    public enum BackgroundOrderStatus
    {
        [StringValue("AAAA")]
        NEW,
        [StringValue("AAAB")]
        IN_PROGRESS,
        [StringValue("AAAC")]
        CANCELLED,
        [StringValue("AAAD")]
        COMPLETED,
        [StringValue("AAAE")]
        PAYMENT_PENDING,
        [StringValue("AAAF")]
        FIRST_REVIEW,
        [StringValue("AAAG")]
        ADDITIONAL_WORK,
        [StringValue("AAAH")]
        SECOND_REVIEW
    }

    public enum PaymentOptions
    {
        [StringValue("PTCC")]
        Credit_Card,
        [StringValue("PTMO")]
        Money_Order,
        [StringValue("PTIN")]
        InvoiceWithApproval,
        [StringValue("PTINA")]
        InvoiceWithOutApproval,
        [StringValue("PTPYP")]
        Paypal,
        [StringValue("PTOS")]
        OfflineSettlement,
        [StringValue("PTCCWAR")]
        Credit_Card_With_Approval_Required
    }

    public enum PDFInclusionOptions
    {
        [StringValue("PDID")]
        Default,
        [StringValue("PDIE")]
        Excluded,
        [StringValue("PDIN")]
        Not_Specified,
    }

    public enum ResultsSentToApplicantOptions
    {
        [StringValue("AAAA")]
        Yes,
        [StringValue("AAAB")]
        No,
        [StringValue("AAAC")]
        Default,
    }

    public enum AdminOrderStatusTypeOptions
    {
        [StringValue("AAAC")]
        ReadyForTransmit,
    }

    public enum ArchiveState
    {
        [StringValue("AA")]
        Active,
        [StringValue("AB")]
        Archived,
        [StringValue("ZZ")]
        All,
        [StringValue("AC")]
        Graduated,
        [StringValue("AD")]
        Archived_and_Graduated,
        [StringValue("AF")]
        Package_Subscription_Cancelled,
        [StringValue("AE")]
        Inactive
    }

    #region UAT-3470
    public enum InvitationArchiveState
    {
        [StringValue("AAAA")]
        Active,
        [StringValue("AAAB")]
        Archived,
        [StringValue("AAAC")]
        All
    }
    #endregion
    public enum SubscriptionState
    {
        [StringValue("AA")]
        Active,
        [StringValue("AB")]
        Archived,
        [StringValue("AC")]
        Graduated,
        [StringValue("AD")]
        Archived_Graduated
    }

    public enum SubscriptionOptions
    {
        [StringValue("9F9C244A-1FF2-4EAC-B77F-30145AE703D2")]
        CustomMonthly = 1
    }

    public enum OrderRequestType
    {
        [StringValue("AAA")]
        NewOrder,
        [StringValue("AAB")]
        RenewalOrder,
        [StringValue("AAC")]
        ChangeSubscription,
        [StringValue("AAD")]
        RushOrder,
        [StringValue("AAE")]
        ChangeSubscriptionByAdmin,
        [StringValue("YYY")]
        CompleteOrderByApplicant,
        [StringValue("AAF")]
        ItemPayment,
        [StringValue("AAG")]
        ModifyShipping
    }

    /// <summary>
    /// Determine from which page user has landed on the PendinOrder.ascx
    /// </summary>
    public enum PendingOrderNavigationFrom
    {
        /// <summary>
        /// Applicant coming from the Dashboard
        /// </summary>
        [StringValue("AAAA")]
        ApplicantDashboard,

        /// <summary>
        /// Applicant coming from the Landing page
        /// </summary>
        [StringValue("AAAB")]
        ApplicantLandingPage,

        /// <summary>
        /// Applicant coming from the List of subscriptions page
        /// </summary>
        [StringValue("AAAC")]
        ApplicantChangeSubscription,
        //UAT-3484 CBI|CABS
        /// <summary>
        /// Applicant is coming from tenant where location service is available and coming from location selection page.
        /// </summary>
        [StringValue("AAAD")]
        FingerPrintDataControl,
        /// <summary>
        /// UAT - 4331
        /// </summary>
        [StringValue("AAAE")]
        ScheduleApplicantAppointment,
        /// <summary>
        /// if Applicant is coming from Archived order screen
        /// </summary>
        [StringValue("AAAF")]
        ArchivedOrderForm


    }

    /// <summary>
    /// lkpPriceModel
    /// </summary>
    public enum PriceModel
    {
        [StringValue("PMPK")]
        Package = 1,
        [StringValue("PMCG")]
        Category,
        [StringValue("PMIM")]
        Item
    }

    public struct OrderStages
    {
        public const Int32 PendingOrder = 10;
        public const Int32 RenewalOrder = 11;
        public const Int32 OrderHistory = 12;
        public const Int32 BalancePayment = 13;
        public const Int32 OrderPaymentDetails = 14;
        public const Int32 ApplicantProfile = 19;
        public const Int32 ApplicantProfileCompleted = 20;

        public const Int32 CustomForms = 21;
        public const Int32 CustomFormsCompleted = 22;

        public const Int32 ScheduleAppointmentCompleted = 24;

        public const Int32 Disclaimer = 30;
        public const Int32 Disclosure = 31;
        public const Int32 RequiredDocumentation = 32;
        public const Int32 RushOrderReview = 40;
        public const Int32 OrderReview = 41;
        public const Int32 OrderPayment = 42;
        public const Int32 OnlinePaymentSubmission = 50;
        public const Int32 PaypalPaymentSubmission = 51;
        public const Int32 CIMAccountSelection = 55;
        public const Int32 OnlineConfirmation = 60;
        public const Int32 RushOrderConfirmation = 61;
    }

    #endregion

    #region Client Settings
    public enum Setting
    {
        [StringValue("PPFQ")]
        Pending_Package_Frequency = 1,
        [StringValue("RMBE")]
        Reminder_Before_Expiry = 2,
        [StringValue("REFQ")]
        Reminder_Expiry_Frequency = 3,
        [StringValue("RMAE")]
        Reminder_After_Expiry = 4,
        [StringValue("EROI")]
        Enable_Rush_Order_For_Invoice = 6,
        [StringValue("CIRMBE")]
        Compliance_Item_Before_Expiry = 7,
        [StringValue("CIREFQ")]
        Compliance_Item_Expiry_Frequency = 8,
        [StringValue("ERO")]
        Enable_Rush_Order = 9,
        [StringValue("RPMET")]
        REVIEW_PACKAGE_MAPPING_EVERY_TRANSITION = 10,
        [StringValue("MILD")]
        MOBILITY_INSTANCE_LEAD_DAYS = 11,
        [StringValue("MTLD")]
        MOBILITY_TRANSITION_LEAD_DAYS = 12,
        [StringValue("AATR")]
        AUTO_APPROVAL_TRANSITION = 13,
        [StringValue("SFNTF")]
        SERVICE_FORM_NOTIFICATION = 14,
        [StringValue("RMSFFRQ")]
        REMINDER_SERVICE_FORM_FREQUENCY_ELECTRONIC = 15,
        [StringValue("DSSN")]
        DISABLE_SSN = 16,
        [StringValue("IACR")]
        IS_AUTO_COMPLETE_REVIEW = 17,
        [StringValue("BOFRL")]
        BACKGROUND_ORDER_FLOW_REQUIRED_LABEL = 18,
        [StringValue("BOFOL")]
        BACKGROUND_ORDER_FLOW_OPTIONAL_LABEL = 19,
        [StringValue("RMSFFRQMN")]
        REMINDER_SERVICE_FORM_FREQUENCY_MANUAL = 20,
        [StringValue("OFIPL")]
        ORDERFLOW_IMMINIZATION_PACKAGE_SECTION_LABEL,
        [StringValue("OFAPL")]
        ORDER_FLOW_ADMINISTRATIVE_PACKAGE_SECTION_LABEL,
        [StringValue("BODNOTE")]
        ENABLE_BACKGROUND_ORDER_NOTES,
        [StringValue("ENDSDUDEQ")]
        ENABLED_NON_DATA_SYNC_DOCUMENT_UPLOAD_TO_DATA_ENTRY_QUEUE,
        [StringValue("CSOCI")]
        APPROVED_TO_PENDING_REVIEW_CATEGORY_HAS_NO_IMPACT_ON_OVERALL_COMPLIANCE,
        [StringValue("SRNA")]
        SUBSCRIPTION_RENEWAL_NEED_APPROVAL,
        [StringValue("ADA")]
        APPLICANT_DOCUMENT_ASSOCIATION,
        [StringValue("ASOSPS")]
        AGENCY_SUGGESTION_ON_STUDENT_PROFILE_SHARE,
        [StringValue("APDT")]
        APPLICANT_PERSONAL_DOCUMENT_TAB,
        //UAT-2251 : Ability to turn on/off the initial student videos individually and to replace the video link by institution
        [StringValue("DIVSSBTC")]
        DISPLAY_INITAIL_VIDEO_SETTING_ON_SUBSCRIBE_TO_COMPLIO,
        [StringValue("DIVSDCUP")]
        DISPLAY_INITAIL_VIDEO_SETTING_ON_DOCUMENT_UPLOAD,
        [StringValue("DIVSDTE")]
        DISPLAY_INITAIL_VIDEO_SETTING_ON_DATA_ENTRY,
        [StringValue("SBTCOVUR")]
        SUBSCRIBE_TO_COMPLIO_OVERRIDE_VIDEO_URL,
        [StringValue("DCUPOVUR")]
        DOCUMENT_UPLOAD_OVERRIDE_VIDEO_URL,
        [StringValue("DTEOVUR")]
        DATA_ENTRY_OVERRIDE_VIDEO_URL,
        //UAT-2413
        [StringValue("ADEERT")]
        APPLICANT_DATA_ENTRY_ENTER_REQUIREMENT_TEXT,
        //UAT-2466
        [StringValue("RSDR")]
        ROTATION_START_DATE_REQUIRED,
        [StringValue("REDR")]
        ROTATION_END_DATE_REQUIRED,
        //UAT2494, New Account verification enhancements (additional verification step)
        [StringValue("AVPM")]
        ACCOUNT_VERIFICATION_PROCESS_MAIN,
        [StringValue("AVPRR")]
        ACCOUNT_VERIFICATION_PROCESS_RESPONSE_REQD,
        [StringValue("AVPDOBPMSN")]
        ACCOUNT_VERIFICATION_PROCESS_DOB_PERMISSION,
        [StringValue("AVPDOBTEXT")]
        ACCOUNT_VERIFICATION_PROCESS_DOB_OVERRIDE_TEXT,
        [StringValue("AVPSSNPMSN")]
        ACCOUNT_VERIFICATION_PROCESS_SSN_PERMISSION,
        [StringValue("AVPSSNTEXT")]
        ACCOUNT_VERIFICATION_PROCESS_SSN_OVERRIDE_TEXT,
        [StringValue("AVPLSSNPSN")]
        ACCOUNT_VERIFICATION_PROCESS_LSSN_PERMISSION,
        [StringValue("AVPLSSNTXT")]
        ACCOUNT_VERIFICATION_PROCESS_LSSN_OVERRIDE_TEXT,
        [StringValue("AVPPCAPMSN")]
        ACCOUNT_VERIFICATION_PROCESS_PROF_CUST_ATTR_PERMISSION,
        [StringValue("AVPPCATEXT")]
        ACCOUNT_VERIFICATION_PROCESS_PROF_CUST_ATTR_OVERRIDE_TEXT,
        //UAT-2569 //
        [StringValue("ACWPN")]
        ACCOUNT_WITHOUT_PURCHASE_NOTIFICATION,
        //UAT-2431
        [StringValue("DUCMTEXT")]
        DOCUMENT_UPLOAD_CONFIRMATION_MESSAGE_TEXT,
        //UAT-2705
        [StringValue("RSSET")]
        REQUIREMENT_SHARES_SETTING,
        [StringValue("DBPCKGN")]
        DISPLAY_BUNDLE_PACKAGE_NOTES_SETTINGS,
        [StringValue("CHRTCLR")]
        PIE_CHART_COLOR,
        [StringValue("AATSM")]
        ALLOW_APPLICANT_TO_SEND_MESSAGE,
        //UAT-2340
        [StringValue("PRSSNP")]
        PASSPORT_REPORT_SSN_PERMISSION,
        [StringValue("PRDOBP")]
        PASSPORT_REPORT_DOB_PERMISSION,
        [StringValue("PRPNP")]
        PASSPORT_REPORT_PHONE_NUMBER_PERMISSION,
        //UAT-2802
        [StringValue("OFMS")]
        ORDER_FLOW_MESSAGE_SETTING,
        //UAT-2930
        [StringValue("TFA")]
        TWO_FACTOR_AUTHENTICATION_SETTING,
        [StringValue("CAUDDEQ")]
        ENABLE_CLIENT_ADMIN_UPLOADED_DOCUMENT_TO_DATA_ENTRY_QUEUE,
        [StringValue("EOCCRM")]
        EXECUTE_COMPLIANCE_RULE_WHEN_OPTIONAL_CATEGORY_COMPLIANCE_RULE_MET,
        [StringValue("ALMEMAIL")]
        ALUMNI_ACCESS_EMAIL,
        [StringValue("ALMPOPUP")]
        ALUMNI_ACCESS_POPUP,
        //UAT-3223
        [StringValue("SAWPNATP")]
        STOP_ACCOUNT_WITHOUT_PURCHASE_NOTIFICATION_AFTER_THRESHOLD_PERIOD,
        [StringValue("TPFSAWPN")]
        THRESHOLD_PERIOD_FOR_STOPPING_ACCOUNT_WITHOUT_PURCHASE_NOTIFICATION,
        //UAT-3240
        [StringValue("DCAIE")]
        DISABLE_CATEGORY_AND_ITEM_EXCEPTIONS,
        //UAT-3485
        [StringValue("RIRMBE")]
        ROTATION_ITEM_BEFORE_EXPIRE,
        //UAT-3137
        [StringValue("RCGTBR")]
        ROTATION_CATEGORY_GOING_TO_BE_REQUIRED,
        [StringValue("AAAL")]
        Auto_Activate_and_Login,
        //UAT-3601
        [StringValue("OFSPL")]
        ORDERFLOW_SCREENING_PACKAGE_SECTION_LABEL,
        [StringValue("OPDH")]
        ORDERREVIEW_PACKAGE_DETAIL_HEADER,
        [StringValue("OCPS")]
        ORDERREVIEW_CHANGE_PACKAGE_SELECTION,
        [StringValue("PPNL")]
        PAYMENTMETHOD_PACKAGE_NAME_LABEL,
        [StringValue("AIPR")]
        APPLICANT_IS_PASSWORD_RETAIN,
        [StringValue("BPTT")]
        BACKGROUND_PACKAGE_TAB_LABEL,
        //UAT-3620
        [StringValue("DSOP")]
        DRUG_SCREENING_ORDER_PROCEED,
        [StringValue("OQDOPA")]
        ORDER_QUEUE_DUPLICATE_ORDER_POPUP_ALERT,
        [StringValue("SASADTI")]
        SHOW_APPLICANT_SSN_AND_DOB_TO_INSTRUCTOR_PRECEPTOR,
        [StringValue("RATOUGIOP")]
        RESTRICT_APPLICANT_TO_ONE_USER_GROUP_IN_ORDER_PROCESS
    }
    #endregion

    #region Verification Queue Possible Status

    public enum VerificationDataActions
    {
        //UAT-1860:Change "Approved" and "Rejected" status text for items to "Meets Requirements" and "Does Not Meet Requirements"
        [StringValue("Meets Requirements")]
        APPROVED,
        //UAT-1860:Change "Approved" and "Rejected" status text for items to "Meets Requirements" and "Does Not Meet Requirements"
        [StringValue("Does Not Meet Requirements")]
        DECLINED,
        [StringValue("Approved")]
        APPROVED_EXCEPTION,
        [StringValue("Rejected")]
        DECLINED_EXCEPTION,
        [StringValue("Send for Client Review")]
        SEND_FOR_CLIENT_REVIEW,
        [StringValue("Further Review (Send To Third party)")]
        FURTHER_REVIEW_THIRD_PARTY,
        [StringValue("Send for Third Party Review")]
        SEND_REVIEW_THIRD_PARTY,
        [StringValue("Pending Review")]
        PENDING_REVIEW,
        [StringValue("Pending Review")]
        PENDING_REVIEW_EXCEPTION,
        [StringValue("Incomplete")]
        INCOMPLETE,
        [StringValue("Expired")]
        EXPIRED,
        //CATEGORY_EXCEPTIONALLY_APPROVED is added as on 'Applicant Data Entry screen' total count for 
        //approved categories is fetched based on review status
        [StringValue("Exceptionally Approved")]
        CATEGORY_EXCEPTIONALLY_APPROVED,
        //UAT-1860:Change "Approved" and "Rejected" status text for items to "Meets Requirements" and "Does Not Meet Requirements"
        //Added a new enum for category override rule so that its text will be "approved" instead of "meets requirements"
        [StringValue("Approved")]
        APPROVED_BY_OVERRIDE
    }

    /// <summary>
    /// Used to set different attributes for different items stauts in Verification Details, to manage the filters
    /// If this is changed, its client side code must be changed 
    /// </summary>
    public struct VerificationDetailsItemDivTypes
    {
        public const String PENDING_REVIEW = "PNGREV"; // All types of Pending Reviews
        public const String ITM_APPROVED_REJECTED = "ITMAPPREJ"; // Include Approved & Rejected Items
        public const String EXC_APPROVED_REJECTED = "EXCAPPREJ"; // Include Exceptionally Approved & Rejected
        public const String INCOMPLETE_ITEM = "INCITM"; // Include Incomplete items
        public const String OTHER_ITEMS = "OTHITMS"; // Include Applied for Exception, Expired Items
    }

    public enum VerificationDetailActionType
    {
        [StringValue("SPN")]
        SubscriptionPreviousNext, // Change in Applicant/Subscription from Left Panel
        [StringValue("CATPNLEFT")]
        CategoryPreviousNextLeft,// Change in category from Left Panel
        [StringValue("CATPNTOP")]
        CategoryPreviousNextTop, // Change in category from top of Middle Panel
        [StringValue("CATPNBTM")]
        CategoryPreviousNexBottom, // Change in category from bottom of Middle Panel
        [StringValue("UPLDOC")]
        UploadNewDocument,
        [StringValue("AUDOC")]
        AssignUnAssignDocument,
        [StringValue("REMDOC")]
        RemoveDocument,
    }

    #endregion

    #region Integrity Checks

    public struct IntegrityCheckResponse
    {
        public CheckStatus CheckStatus;
        public String UIMessage;

    }

    public enum CheckStatus
    {
        True = 1,
        False = 0
    }

    #endregion

    public enum ConstantType
    {
        [StringValue("VDTE")]
        Date,
        [StringValue("VNUM")]
        Numeic,
        [StringValue("VTXT")]
        Text,
        [StringValue("VBL")]
        Bool,
        [StringValue("VEMT")]
        Empty,
        [StringValue("TDAY")]
        Day,
        [StringValue("TMTH")]
        Month,
        [StringValue("TYR")]
        Year,
        [StringValue("DOB")]
        DOB,
        [StringValue("ICS")]
        ItemComplianceStatus,
        [StringValue("ISD")]
        ItemSubmissionDate,
        [StringValue("RSD")]
        RotationStartDate,
        [StringValue("RED")]
        RotationEndDate
    }

    public enum PkgMappingType
    {
        [StringValue("AAAA")]
        From,
        [StringValue("AAAB")]
        To,
    }

    public enum PkgMappingStatus
    {
        [StringValue("AAAA")]
        Pending_Review,
        [StringValue("AAAB")]
        Reviewed,
        [StringValue("AAAC")]
        Reviewed_instance,


    }

    public enum AuthRequestType
    {
        [StringValue("AA")]
        Email_Confirmation,
        [StringValue("AB")]
        Email_Updation_For_SharedUser
    }

    public struct DashBoardUsers
    {
        public const Int32 AdminDashBoard = 0;
        public const Int32 ApplicantDashBoard = 1;
        public const Int32 ClientAdminDashBoard = 2;
        public const Int32 AMSAdminDashBoard = 3;
        public const Int32 AMSApplicantDashBoard = 4;
        public const Int32 AMSClientAdminDashBoard = 5;
    }

    public enum UtilityFeatures
    {
        [StringValue("AAAA")]
        Ctrl_Save,
        [StringValue("AAAB")]
        Dock_UnDock,
        [StringValue("AAAC")]
        Unified_Document,
        [StringValue("AAAD")]
        Single_Document,
        //UAT-2302-As a student, I should see a popup on login if i have logged in using a non-preferred browser.
        [StringValue("AAAE")]
        NonPrefferedBrowser
    }

    #region PreferredBrowser Versions
    //UAT-2302-As a student, I should see a popup on login if i have logged in using a non-preferred browser.
    public enum PreferredBrowserVersions
    {
        Chrome = 44,
        Mozilla = 36,
        InternetExplorer = 9,
        Safari = 8
    }
    #endregion

    #region InstitutionNodeType

    public enum NodeType
    {
        [StringValue("AAAA")]
        Institution = 1,
        [StringValue("AAAB")]
        Department = 2,
        [StringValue("AAAC")]
        Program = 3,
        [StringValue("AAAD")]
        Grade = 4,
    }

    /// <summary>
    /// lkpPermission
    /// </summary>
    public enum LkpPermission
    {
        [StringValue("AAAA")]
        FullAccess = 1,
        [StringValue("AAAB")]
        ReadOnly,
        [StringValue("AAAC")]
        NoAccess,
        [StringValue("AAAD")]
        AdministrativePackagePermission,
        [StringValue("AAAE")]
        ImmunizationPackagePermission,
        [StringValue("AAAF")]
        BothPackagePermission
    }

    #endregion

    #region Custom Attribute

    public enum CustomAttributeDatatype
    {
        [StringValue("CADTDAT")]
        Date,
        [StringValue("CADTTEX")]
        Text,
        [StringValue("CADTBLN")]
        Boolean,
        [StringValue("CADTNUM")]
        Numeric,
        [StringValue("CADTUG")]
        User_Group
    }

    public enum CustomAttributeUseTypeContext
    {
        [StringValue("CAUTHRCHY")]
        Hierarchy,
        [StringValue("CAUCLNRTN")]
        Clinical_Rotation,
        [StringValue("CAUPRFL")]
        Profile
    }

    #region Enums to manage Custom Attribute the Controls

    /// <summary>
    /// Data source of the control
    /// </summary>
    public enum DataSourceMode
    {
        ExternalList,
        Ids
    }

    /// <summary>
    /// Display the labels or actual controls
    /// </summary>
    public enum DisplayMode
    {
        Labels, // Case of Order confirmation
        Controls,
        ReadOnlyLabels // With ronly property of the labels
    }

    #endregion

    #endregion

    #region Search

    public struct SearchScope
    {
        public const Int32 SingleTenantSynch = 1;
        public const Int32 SingleTenantAsynch = 2;
        public const Int32 TenantsListSynch = 3;
        public const Int32 TenantsListAsynch = 4;
        public const Int32 AllTenantsSynch = 5;
        public const Int32 AllTenantsAsynch = 6;
    }

    public struct LkpSearchResultStatus
    {
        public const String Completed = "AAAA";
        public const String InProgress = "AAAB";
        public const String Error = "AAAC";
    }

    public struct ReturnType
    {
        public const String Return = "Return";
        public const String InsertResultTable = "InsertResultTable";
    }

    public struct SearchDataTable
    {
        public const String SearchResult = "SearchResult";
        public const String Error = "Error";
        public const String SearchResultInstance = "SearchResultInstance";
    }

    public enum MasterSearchMode
    {
        Offline = 0,
        Online = 1
    }

    public struct SearchTypeCode
    {
        public const String AppilicantPortfolioSearch = "AAAA";
    }

    #endregion

    #region userGroup
    public enum UserGroupType
    {
        [StringValue("Fixed")]
        Fixed,
        [StringValue("Defined")]
        Defined
    }

    public enum FixedUserGroups
    {
        [StringValue("All")]
        All = 1,
        [StringValue("New")]
        New = 2,
        [StringValue("Existing")]
        Existing = 3
    }
    #endregion

    #region Mobility

    public struct LkpInstChangeRequestStatus
    {
        public const String Pending = "INCRSP";
        public const String Complete = "INCRSC";
        public const String Rejected = "INCRSR";
    }
    public enum ApplicantNodeTransitionApprovalStatus
    {
        [StringValue("AAAA")]
        MovementDue,
        [StringValue("AAAB")]
        Hold,
        [StringValue("AAAC")]
        MovementDone
    }

    public enum MobilityDurationType
    {
        [StringValue("AAA")]
        Years,
        [StringValue("AAB")]
        Months,
        [StringValue("AAC")]
        Weeks,
        [StringValue("AAD")]
        Days
    }

    public struct LkpSubscriptionMobilityStatus
    {
        public const String MobilitySwitched = "AAAA";
        public const String DataMovementDue = "AAAB";
        public const String DataMovementComplete = "AAAC";
        public const String InstituteSwitched = "AAAD";
        public const String DataMovementNotRequired = "AAAE";
    }

    #endregion

    #region Data Entry Help
    public enum RecordType
    {
        [StringValue("AAAA")]
        Package,
        [StringValue("AAAB")]
        Order,
        [StringValue("AAAC")]
        BackgroundProfile,
        [StringValue("AAAD")]
        Compliance_Package,
        [StringValue("AAAE")]
        Background_Package,
        [StringValue("AAAF")]
        Institution_Node,
        [StringValue("AAAG")]
        Background_Service
    }

    public enum WebsiteWebPageType
    {
        [StringValue("AAAA")]
        Other,
        [StringValue("AAAB")]
        DataEntryHelp,
        [StringValue("AAAC")]
        DisclosureForm
    }
    #endregion

    #region System Service

    public enum SystemService
    {
        [StringValue("EMLSRVC")]
        EMAIL_SERVICE,
        [StringValue("SUBEXPSRVC")]
        SUBSCRIPTION_EXPIRY_SERVICE,
        [StringValue("COMEXPSRVC")]
        COMPLIANCE_EXPIRY_SERVICE,
        [StringValue("REOCRUSRVC")]
        REOCCUR_RULES_SERVICE,
        [StringValue("MISRVC")]
        MOBILITY_INSTANCE_SERVICE,
        [StringValue("CPPKGSRVC")]
        COPY_PACKAGE_DATA_SERVICE,
        [StringValue("DASSUMSRVC")]
        DASHBOARD_SUMMARY_SERVICE
    }



    public struct LkpSystemService
    {
        public const String EMAIL_SERVICE = "EMLSRVC";
        public const String SUBSCRIPTION_EXPIRY_SERVICE = "SUBEXPSRVC";
        public const String COMPLIANCE_EXPIRY_SERVICE = "COMEXPSRVC";
        public const String REOCCUR_RULES_SERVICE = "REOCRUSRVC";
        public const String MOBILITY_INSTANCE_SERVICE = "MISRVC";
        public const String COPY_PACKAGE_DATA_SERVICE = "CPPKGSRVC";
        public const String DASHBOARD_SUMMARY_SERVICE = "DASSUMSRVC";
        public const String EXECUTE_MULTI_RULES_SERVICE = "EXEMRUSRVC";
        public const String EXECUTE_REQUIREMENT_RULES_SERVICE = "EXERQRSRVC";
        public const String BACKGROUND_COPY_PACKAGE_DATA = "BKGCPYPKDT"; //UAT-3224 :-System queue to retry the data sync which was not completed

    }

    public enum LKPDocumentStatus
    {
        [StringValue("AAAA")]
        MergingInProgress,
        [StringValue("AAAB")]
        MergingFailed,
        [StringValue("AAAC")]
        MergingCompleted,
        [StringValue("AAAD")]
        MergingCompletedWithErrors
    }

    #endregion

    #region QUEUE FRAMEWORK

    public enum QueueConfirgurationType
    {
        [StringValue("AAAA")]
        BasicAssignment,

        [StringValue("AAAB")]
        SpecializedUserAssignment
    }

    /// <summary>
    /// Enum types for Queue Meta Data
    /// </summary>
    public enum QueueMetaDataType
    {
        [StringValue("AAAA")]
        Verification_Queue_For_Admin,

        [StringValue("AAAB")]
        Verification_Queue_For_ClientAdmin,

        [StringValue("AAAC")]
        Verification_Queue_For_Third_Party,

        [StringValue("AAAD")]
        Exception_Queue_For_Admin,

        [StringValue("AAAE")]
        Exception_Queue_For_ClientAdmin,

        [StringValue("AAAF")]
        Escalated_Verification_Queue_For_Admin,

        [StringValue("AAAG")]
        Escalated_Verification_Queue_For_ClientAdmin,

        [StringValue("AAAH")]
        Escalated_Verification_Queue_For_Third_Party,

        [StringValue("AAAI")]
        Escalated_Exception_Queue_For_Admin,

        [StringValue("AAAJ")]
        Escalated_Exception_Queue_For_ClientAdmin,

        [StringValue("AAAK")]
        Reconciliation_Queue_For_Admin,
    }

    /// <summary>
    /// Enum types for the QueueFieldsMetaData Entity, based on the QF_ValueFieldName column
    /// </summary>
    public enum QueuefieldsMetaDataType
    {
        #region Admin Queues
        [StringValue("AAAB")]
        ComplianceItemId_VerificationQueueAdmin,

        [StringValue("AAAC")]
        CategoryId_VerificationQueueAdmin,

        [StringValue("AAAD")]
        PackageID_VerificationQueueAdmin,

        [StringValue("AABT")]
        ApplicantComplianceItemID_VerificationQueueAdmin,

        [StringValue("AABC")]
        ComplianceItemId_ExceptionQueueAdmin,

        [StringValue("AABD")]
        CategoryId_ExceptionQueueAdmin,

        [StringValue("AABE")]
        PackageID_ExceptionQueueAdmin,

        [StringValue("AACC")]
        ApplicantComplianceItemID_ExceptionQueueAdmin,

        [StringValue("AADI")]
        ComplianceItemId_EsclationQueueAdmin,

        [StringValue("AADJ")]
        CategoryId_EsclationQueueAdmin,

        [StringValue("AADQ")]
        ApplicantComplianceItemID_EsclationQueueAdmin,

        [StringValue("AAFH")]
        ComplianceItemId_ExceptionEsclationQueueAdmin,

        [StringValue("AAFI")]
        CategoryId_ExceptionEsclationQueueAdmin,

        [StringValue("AAFP")]
        ApplicantComplianceItemID_ExceptionEsclationQueueAdmin,
        #endregion

        #region ClientAdmin
        [StringValue("AAAF")]
        VerificationStatus_VerificationQueueAdmin,

        [StringValue("AAAO")]
        VerificationStatus_VerificationQueueClientAdmin,

        [StringValue("AAAL")]
        CategoryId_VerificationQueueClientAdmin,

        [StringValue("AABW")]
        ApplicantComplianceItemID_VerificationQueueClientAdmin,

        [StringValue("AABG")]
        VerificationStatus_ExceptionQueueAdmin,

        [StringValue("AABN")]
        PackageID_ExceptionQueueClientAdmin,

        [StringValue("AABM")]
        CategoryId_ExceptionQueueClientAdmin,

        [StringValue("AABL")]
        ComplianceItemId_ExceptionQueueClientAdmin,

        [StringValue("AAAK")]
        ComplianceItemId_VerificationQueueClientAdmin,

        [StringValue("AAAM")]
        PackageID_VerificationQueueClientAdmin,

        [StringValue("AACF")]
        ApplicantComplianceItemID_ExceptionClientAdmin,

        [StringValue("AADZ")]
        ComplianceItemId_EsclationQueueClientAdmin,

        [StringValue("AAEA")]
        CategoryId_EsclationQueueClientAdmin,

        [StringValue("AAEH")]
        ApplicantComplianceItemID_EsclationQueueClientAdmin,


        [StringValue("AAGG")]
        ApplicantComplianceItemID_ExceptionEsclationClientAdmin,

        [StringValue("AAFZ")]
        CategoryId_ExceptionEsclationQueueClientAdmin,

        [StringValue("AAFY")]
        ComplianceItemId_ExceptionEsclationQueueClientAdmin,
        #endregion

        #region THIRD PARTY QUEUES

        [StringValue("AAAT")]
        ComplianceItemId_VerificationQueueThirdParty,
        [StringValue("AAAV")]
        PackageID_VerificationQueueThirdParty,
        [StringValue("AAAU")]
        CategoryId_VerificationQueueThirdParty,
        [StringValue("AABZ")]
        ApplicantComplianceItemID_VerificationQueueThirdParty,

        [StringValue("AAEQ")]
        ComplianceItemId_EsclationQueueThirdParty,

        [StringValue("AAER")]
        CategoryId_EsclationQueueThirdParty,

        [StringValue("AAEY")]
        ApplicantComplianceItemID_EsclationQueueThirdParty
        #endregion
    }

    public enum lkpQueueActionType
    {
        [StringValue("AAAA")]
        Next_Level_Review_Required,

        [StringValue("AAAB")]
        Proceed_To_Next_Queue,

        [StringValue("AAAC")]
        Escalation_Required,

        /// <summary>
        ///  This is added only to identify items for which NO STATUS was changed, but SAVE was clicked for them.
        ///  THIS SHOULD NEVER BE ADDED IN THE ACTUAL LOOK UP TABLES
        /// </summary>
        [StringValue("NSC")]
        No_Status_Changed,
        [StringValue("RRR")]
        Random_Review_Required,
        [StringValue("SRRR")]
        SendFor_Random_Review_Required,
        [StringValue("RROC")]
        Random_Review_OverRidden_Clntadmn,
    }

    public enum lkpQueueEscalationType
    {
        [StringValue("AAAA")]
        Escalated,

        [StringValue("AAAB")]
        Escalation_Resolved,
    }

    /// <summary>
    /// Constants to represent the Verification screen Meta data fields
    /// </summary>
    public struct QueuefieldsMetaDataTypeConstants
    {
        public const String ApplicantName = "ApplicantName";
        public const String ComplianceItemId = "ComplianceItemId";
        public const String CategoryId = "CategoryId";
        public const String PackageID = "PackageID";
        public const String SubmissionDate = "SubmissionDate";
        public const String Verification_Status = "StatusID";
        public const String System_StatusID = "SystemStatusID";
        public const String System_Status = "SystemStatus";
        public const String Rush_Order_Status = "RushOrderStatusCode";
        public const String Assigned_To_User = "AssignedToUserID";
        public const String ApplicantComplianceItemID = "ApplicantComplianceItemID ";
        public const String HierarchyNodeID = "HierarchyNodeID";
        public const String ApplicantId = "ApplicantId";
        public const String Item_Name = "ItemName";
        public const String Category_Name = "CategoryName";
        public const String Package_Name = "PackageName";
        public const String Verification_Status_Text = "VerificationStatus";
        public const String Rush_Order_Status_Text = "RushOrderStatus";
        public const String Rush_Order_Status_Code = "RushOrderStatusCode";
    }

    #endregion

    public enum DocumentType
    {
        [StringValue("AAAA")]
        DisclaimerDocument,
        [StringValue("AAAB")]
        DisclosureDocument,
        [StringValue("AAAC")]
        EDS_AuthorizationForm,
        [StringValue("AAAD")]
        Disclosure_n_Release,
        [StringValue("AAAE")]
        Reciept_Document,
        [StringValue("AAAF")]
        REQUIREMENT_FIELD_VIEW_DOCUMENT,
        [StringValue("AAAG")]
        REQUIREMENT_FIELD_UPLOAD_DOCUMENT,
        [StringValue("AAAH")]
        ROTATION_SYLLABUS,
        [StringValue("AAAI")]
        CONTRACT_DOCUMENT,
        [StringValue("AAAJ")]
        INSURANCE_CERTIFICATE_DOCUMENT,
        [StringValue("AAAK")]
        COMPLIANCE_VIEW_DOCUMENT,
        [StringValue("AAAL")]
        LETTER_OF_RENEWAL,
        [StringValue("AAAM")]
        AMENDMENT,
        [StringValue("AAAN")]
        OTHER,
        [StringValue("AAAO")]
        ADDITIONAL_DOCUMENTS,
        [StringValue("AAAP")]
        SCREENING_DOCUMENT_ATTRIBUTE_TYPE_DOCUMENT,
        [StringValue("AAAQ")]
        PERSONAL_DOCUMENT,
        [StringValue("AAAR")]
        DOCUMENT_FOR_REQUIREMENT_APPROVAL_NOTIFICATION,
        [StringValue("AAAS")]
        ATTESTATION_FORM,
        [StringValue("AAAT")]
        FINGERPRINT_SCAN_DOCUMENT,
        [StringValue("AAAV")]
        TICKET_ISSUE_DOCUMENT,

    }

    public enum ScheduleActionType
    {
        [StringValue("ECR")]
        EXECUTE_CATEGORY_RULES,
        [StringValue("ECRARE")]
        EXECUTE_CATEGORY_RULES_AFTER_RULE_EDIT,
        [StringValue("ERAOD")]
        EXECUTE_RULES_AFTER_OBJECT_DELETION
    }

    public enum CopyType
    {
        [StringValue("AAAA")]
        CC,
        [StringValue("AAAB")]
        BCC
    }

    public enum FileType
    {
        [StringValue("AAAA")]
        ApplicantFileLocation,
        [StringValue("AAAB")]
        MessageFileLocation,
        [StringValue("AAAC")]
        SystemDocumentLocation
    }

    #region CommunicationSubEventCategoryType

    public enum SubEventCategoryType
    {
        [StringValue("AAAA")]
        ComplioSystem,
        [StringValue("AAAB")]
        ComplioStudent,
        [StringValue("AAAC")]
        ComplioAdmin,
        [StringValue("AAAD")]
        COMMON,
        [StringValue("AAAE")]
        AMS_SYSTEM

    }
    #endregion

    #region  Template Module related Enums

    public enum TemplateNodeLevels
    {
        [StringValue("AAAA")]
        ROOTLEVEL,
        [StringValue("AAAB")]
        CURRENTLEVEL
    }

    #endregion

    public enum OrderPackageTypes
    {
        [StringValue("AAAA")]
        COMPLIANCE_PACKAGE,
        [StringValue("AAAB")]
        BACKGROUND_PACKAGE,
        [StringValue("AAAC")]
        COMPLIANCE_AND_BACKGROUMD_PACKAGE,
        [StringValue("AAAD")]
        COMPLIANCE_RUSHORDER_PACKAGE,
        [StringValue("AAAE")]
        TRACKING_ITEM_PAYMENT,
        [StringValue("AAAF")]
        REQUIREMENT_ITEM_PAYMENT,
        [StringValue("AAAG")]
        ADDITIONAL_PRICE_BACKGROUND_PACKAGE
    }

    /// <summary>
    /// Represents the 'lkpCompliancePackageType' table, both in Tenant & Security.
    /// Represents the Types of Compliance Packages i.e. Immunization or Administrative Packages.
    /// </summary>
    public enum CompliancePackageTypes
    {
        [StringValue("IMNZ")]
        IMMUNIZATION_COMPLIANCE_PACKAGE,
        [StringValue("ADMN")]
        ADMINISTRATIVE_COMPLIANCE_PACKAGE,
    }

    #region Custom Forms

    public enum DisplayColumn
    {
        Two = 2,
        Three = 3
    }

    public enum OccurenceType
    {
        Single = 1,
        Multiple = 2
    }

    #endregion

    #region Bkg Order Status Type - lkpOrderStatusType

    public enum OrderStatusType
    {
        [StringValue("AAAA")]
        NEW,
        [StringValue("AAAB")]
        INPROGRESS,
        [StringValue("AAAC")]
        CANCELLED,
        [StringValue("AAAD")]
        COMPLETED,
        [StringValue("AAAE")]
        PAYMENTPENDING,
        //[StringValue("AAAF")]
        //FIRSTREVIEW,
        [StringValue("AAAG")]
        ADDITIONALWORK,
        //[StringValue("AAAH")]
        //SECONDREVIEW
        [StringValue("AAAI")]
        ADDITIONALWORKINPROGRESS
    }

    /// <summary>
    /// Enum for the Lookup of Background order history i.e. ams.lkpEventHistory
    /// </summary>
    public enum BkgOrderEvents
    {
        [StringValue("AAAA")]
        ORDER_CREATED,
        [StringValue("AAAB")]
        ORDER_APPROVED,
        [StringValue("AAAC")]
        ORDER_REJECTED,
        [StringValue("AAAD")]
        ORDER_UPDATED,
        [StringValue("AAAE")]
        ORDER_ARCHIVED,
        [StringValue("AAAF")]
        ORDER_ACTIVE,
        [StringValue("AAAN")]
        APPLICANT_INVITATION_COMPLETED,
        [StringValue("AAAH")]
        ADMIN_ENTRY_ORDER_INPROGRESS,
        [StringValue("AAAO")]
        ADMIN_ENTRY_ORDER_ON_HOLD
    }

    public enum OrderLineItemStatusType
    {
        [StringValue("AAAA")]
        NEW,
        [StringValue("AAAB")]
        FIRST_REVIEW,
        [StringValue("AAAC")]
        AUTO_COMPLETE
    }

    /// <summary>
    /// Enum for the ams.lkpPackageSupplementalType table
    /// </summary>
    public enum BkgPackageSupplementalType
    {
        [StringValue("AAAA")]
        NONE,
        [StringValue("AAAB")]
        FLAGGED,
        [StringValue("AAAC")]
        ANY
    }

    /// <summary>
    /// Enum for the lkpOrderAdditionType table
    /// </summary>
    public enum OrderAdditionType
    {
        [StringValue("AAAA")]
        SUPPLEMENT_BACKGROUND_ORDER
    }

    #endregion

    #region Bkg Order Line Item Status - lkpOrderLineItemStatus

    public enum OrderLineItemStatus
    {
        [StringValue("AAAA")]
        NEW,
        [StringValue("AAAB")]
        FIRSTREVIEW,
        [StringValue("AAAC")]
        AUTOCOMPLETE
    }


    public enum BkgOrderDetailDispatchStatus
    {
        [StringValue("PNDG")]
        PENDING,
        [StringValue("DSPT")]
        DISPATCHED
    }

    public enum BkgOrderLineItemDetailStatus
    {

        [StringValue("AAAA")]
        DRAFT,
        [StringValue("AAAB")]
        NEW,
        [StringValue("AAAC")]
        INPROGRESS,
        [StringValue("AAAD")]
        COMPLETED,
        [StringValue("AAAE")]
        CANCELLED,
        [StringValue("AAAF")]
        ARCHIVED,
        [StringValue("AAAG")]
        EXPIRED
    }


    #endregion

    #region Background Service Enumeration(BkgAttributeGroupMapping)

    public enum ServiceAttributeGroup
    {
        [StringValue("CC184FC4-5401-445D-90AA-E77167227904")]
        PERSONAL_INFORMATION,
        [StringValue("854D4938-C1FD-4455-8FFB-AD76859CDE64")]
        EMPLOYMENT_HISTORY,
        [StringValue("1F05A41A-0871-4A4E-91AD-E76BCE8220C0")]
        EDUCATION_HISTORY,
        [StringValue("338F1CA2-6B0A-43C1-B900-A8F6B058678F")]
        RESIDENTIAL_HISTORY,
        [StringValue("E77307A6-E982-4C66-8A65-F49D1F2FD415")]
        PROFESSIONAL_LICENSE,
        [StringValue("6B4A93FE-11E6-4589-978D-3BD59DED4D24")]
        PERSONAL_REFERENCE,
        [StringValue("6E24F8BD-671D-4BBF-9984-5F6AE50B7041")]
        ELECTRONIC_DRUGSCREEN,
        [StringValue("21F2B0E5-C33C-4104-B1BA-F87866B3FADC")]
        INTERNATIONAL_EMPLOYMENT_HISTORY,
        [StringValue("EF62841D-E4E9-40AC-A350-54DD74F4E623")]
        EXTENDED_PERSONAL_INFORMATION,
        [StringValue("A26014A2-ED57-47CC-B68F-17C02E376D60")]
        PERSONAL_ALIAS
    }

    public enum PersonalInformationAttGroup
    {
        [StringValue("88A26BC1-FE90-4103-8E83-D0133479DC00")]
        FIRST_NAME,
        [StringValue("0260B1EA-2834-4CBD-B494-15EE84BB7016")]
        MIDDLE_NAME,
        [StringValue("D4798877-38D4-4418-92B8-B9912EAB02E4")]
        LAST_NAME,
        [StringValue("62CAD6E3-AE98-4981-BF50-A25661506EB5")]
        SOCIAL_SECURITY_NUMBER,
        [StringValue("897DFC34-9123-47C6-8B41-5007E8E16AA1")]
        DATE_OF_BIRTH,
        [StringValue("9D76FDE5-0E5B-429B-BE1B-2150E2582FD3")]
        PHONE_NUMBER,
        [StringValue("326F6BCF-54F3-4820-8950-CB78D78F638C")]
        EMAIL,
        [StringValue("F3CAC96F-D28F-4C63-BD33-DC3B4083F217")]
        GENDER,
        [StringValue("E4DBBC03-9A6E-448A-AA5D-A8C9DA6B7B6A")]
        SEND_BACKGROUND_REPORT
        //[StringValue("E1435BEC-C102-4996-8BC7-65EAD2A7A274")]
        //DRIVERS_LICENSE_NUMBER,
        //[StringValue("A1072BF1-7D53-4244-BF98-0B5D0883A9DF")]
        //DRIVERS_LICENSE_STATE
    }

    public enum PersonalAliasAttGroup
    {
        [StringValue("56258C54-C2BC-4514-94E1-2EF2EFFFDBF5")]
        ALIAS_NAME
    }

    public enum ResidentialHistoryAttGroup
    {
        [StringValue("405D0F80-BFDE-4536-9DDC-E669518C152E")]
        ADDRESS1,
        [StringValue("25200D5C-39DC-42C1-A0A3-32E8646C36B1")]
        ADDRESS2,
        [StringValue("2AB9E10B-9615-4B86-AE84-8F61407E0D9A")]
        CITY,
        [StringValue("CAEAC9FA-FFF4-4F7A-A644-96967F399362")]
        STATE,
        [StringValue("C00AEFB5-37DF-44F7-A050-D2C9581909DE")]
        COUNTY,
        [StringValue("FCE8175F-FB17-4D24-B6B9-E32C0AA0CE4B")]
        POSTAL_CODE,
        [StringValue("37B6B708-C691-4568-B604-6F70F24BC839")]
        COUNTRY,
        [StringValue("7892AC30-E3BC-4DD6-A251-B4EE1944D0B3")]
        DATE_START,
        [StringValue("F47B0517-EE57-47A3-AE5C-BC9C42A21AA5")]
        DATE_END,
        [StringValue("72C4A313-5677-4948-9789-350FB0467800")]
        STATE_PROVINCE,
        [StringValue("2080F339-01B8-4C4E-A6D3-04568CE8AFE1")]
        IS_CURRENT_ADDRESS,
        [StringValue("1D7FADE5-DDED-443C-AF6E-1107E2E418DA")]
        IS_PRIMARY_ADDRESS
    }

    public class ClearstarProfileStatus
    {
        public const string Draft = "0";
        public const string InProgress = "1";
        public const string Completed = "2";
        public const string InReview = "3";
        public const string Cancelled = "4";
        public const string Archived = "5";
    }


    #endregion

    #region AMS Service Item fee Type
    public enum ServiceItemFeeType
    {
        [StringValue("AAAA")]
        STATE_COURT_FEE,
        [StringValue("AAAB")]
        ALL_COUNTY_COURT_FEE,
        [StringValue("AAAC")]
        MVR_FEE,
        [StringValue("AAAD")]
        COUNTY_COURT_FEE,
        [StringValue("AAAE")]
        FIXED_FEE,
        /// <summary>
        /// Represents the Global Country fee for International Criminal Search
        /// </summary>
        [StringValue("AAAF")]
        COUNTRY_FEE,
        [StringValue("AAAG")]
        SHIPPING_FEE,
        [StringValue("AAAH")]
        ADDITIONAL_SERVICE_FEE
    }
    #endregion

    #region AMS - BkgSvcAttributeGroups

    /// <summary>
    /// Represents the Attribute Groups in BkgSvcAttributeGroup table.
    /// Any change in their names in the table MUST be updated here.
    /// </summary>
    public enum SvcAttributeGroups
    {
        [StringValue("Personal Information")]
        PERSONAL_INFORMATION,
        [StringValue("Residential History")]
        RESIDENTIAL_HISTORY,
        [StringValue("Personal Alias")]
        PERSONAL_ALIAS,
        [StringValue("MVR")]
        MVR
    }

    #endregion

    public enum SvcAttributeDataType
    {
        [StringValue("AAAA")]
        DATE,
        [StringValue("AAAB")]
        TEXT,
        [StringValue("AAAC")]
        NUMERIC,
        [StringValue("AAAD")]
        OPTION,
        [StringValue("AAAE")]
        COUNTRY,
        [StringValue("AAAF")]
        STATE,
        [StringValue("AAAG")]
        COUNTY,
        [StringValue("AAAH")]
        CITY,
        [StringValue("AAAI")]
        ZIP_CODE,
        [StringValue("AAAJ")]
        STAND_ALONE_COUNTRY,
        [StringValue("AAAK")]
        CASCADING
    }

    public enum BkgObjectType
    {
        [StringValue("SATR")]
        SERVICE_ATTRIBUTE,
        [StringValue("SITM")]
        SERVICE_ITEM,
        [StringValue("SERV")]
        SERVICE,
    }

    public enum BkgRuleObjectMappingType
    {
        [StringValue("SRVC")]
        SERVICE_RESULT,
        [StringValue("DVAL")]
        DATA_VALUE,
        [StringValue("CONST")]
        DEFINED_VALUE
    }

    #region Background Order Notification And Status Change Enums
    public enum BusinessChannelType
    {
        [StringValue("AAAA")]
        COMPLIO,
        [StringValue("AAAB")]
        AMS,
        [StringValue("AAAC")]
        COMMON
    }

    public enum ServiceFormStatus
    {
        [StringValue("AAAA")]
        NEED_TO_SEND,
        [StringValue("AAAB")]
        SENT_TO_STUDENT,
        [StringValue("AAAC")]
        RESENT_TO_STUDENT,
        [StringValue("AAAD")]
        RECEIVED_FROM_STUDENT,
        [StringValue("AAAE")]
        IN_PROCESS_AGENCY,
        [StringValue("AAAF")]
        IN_PROCESS_FBI,
        [StringValue("AAAG")]
        COPY_REQUESTED_FROM_STUDENT,
        [StringValue("AAAH")]
        REJECTED_BY_FBI_FIRST,
        [StringValue("AAAI")]
        REJECTED_BY_FBI_SECOND,
        [StringValue("AAAJ")]
        REJECTED_BY_FBI_THIRD,
        [StringValue("AAAK")]
        REJECTED_BY_FBI_FOURTH,
        [StringValue("AAAL")]
        AUTO_COMPLETED
    }

    /// <summary>
    /// Dispatch type modes of the Service forms
    /// </summary>
    public enum ServiceFormDispatchType
    {
        [StringValue("AAAA")]
        DEFAULT,
        [StringValue("AAAB")]
        AUTOMATIC,
        [StringValue("AAAC")]
        MANUAL
    }

    public enum OrderNotifyStatus
    {
        [StringValue("AAAA")]
        PENDING,
        [StringValue("AAAB")]
        NOTIFIED,
        [StringValue("AAAC")]
        ERROR
    }

    public enum OrderNotificationType
    {
        [StringValue("AAAA")]
        SERVICE_FORM_DOCUMENT,
        [StringValue("AAAB")]
        SERVICE_FORM_NOTIFICATION,
        [StringValue("AAAC")]
        ORDER_PAID_NOTIFICATION,
        [StringValue("AAAD")]
        ORDER_RESULT,
        [StringValue("AAAE")]
        FLAGGED_ORDER_RESULT,
        [StringValue("AAAF")]
        FLAGGED_ORDER_EMPLOYMENT_NOTIFICATION,
        [StringValue("AAAG")]
        ONLINE_FAILED_ORDER_NOTIFICATION,
        [StringValue("AAAH")]
        FLAGGED_COMPLETED_SVC_GRP_EMPLOYMENT_NOTIFICATION,
        [StringValue("AAAI")]
        FLAGGED_COMPLETED_SVC_GRP_NOTIFICATION,
        [StringValue("AAAJ")]
        SEND_ADDITIONALDOCUMENT_NOTIFICATION,
        [StringValue("AAAK")]
        EDS_NOTIFICATION
    }

    public enum DocumentAttachmentType
    {
        [StringValue("AAAA")]
        SYSTEM_DOCUMENT,
        [StringValue("AAAB")]
        APPLICANT_DOCUMENT,
        [StringValue("AAAC")]
        MESSAGE_DOCUMENT,
        [StringValue("AAAD")]
        ORDER_COMPLETION_DOCUMENT,
        [StringValue("AAAE")]
        DAILY_REPORT,
        [StringValue("AAAF")]
        REQUIREMENT_EXPLANATION
    }

    public enum CommunicationEntityType
    {
        [StringValue("AAAA")]
        BACKGROUND_SERVICE_FORM,
        [StringValue("AAAB")]
        ORDER_PACAKGE_TYPE,
        [StringValue("AAAC")]
        CREDIT_CARD_ORDER_PACAKGE_TYPE,
        [StringValue("AAAD")]
        SERVICE_FORM_NOTIFICATION_REMINDER,
        //UAT-3453
        [StringValue("AAAE")]
        SCREENING_RESULT
    }

    #endregion

    #region UAT-3453
    public enum ScreeningResultType
    {
        [StringValue("AAAA")]
        Clear,
        [StringValue("AAAB")]
        Flagged
    }

    #endregion

    #region BkgServiceTypes

    public enum BkgServiceType
    {
        [StringValue("AAAA")]
        SIMPLE,
        [StringValue("AAAB")]
        LOCATION,
        [StringValue("AAAC")]
        EMPLOYMENT,
        [StringValue("AAAD")]
        INTERNATIONAL,
        [StringValue("AAAE")]
        EDUCATION,
        [StringValue("AAAF")]
        PROFESSIONAL,
        [StringValue("AAAG")]
        VALIDATION,
        [StringValue("AAAH")]
        REFERENCE,
        [StringValue("AAAI")]
        NONLOCATION,
        [StringValue("AAAJ")]
        MVR,
        [StringValue("AAAK")]
        FEDERALCRIMINAL,
        [StringValue("AAAL")]
        ELECTRONICFINGERPRINT,
        [StringValue("AAAM")]
        ELECTRONICDRUGSCREEN,
        [StringValue("AAAN")]
        ADBBASE,
        [StringValue("AAAO")]
        OPERATIONSUPPORTAUTOCOMPLETE,
        [StringValue("AAAP")]
        PRINT_SCAN,
        [StringValue("AAAQ")]
        FingerPrint_Card,
        [StringValue("AAAR")]
        Passport_Photo
    }

    public enum CountryISO3Code
    {
        [StringValue("USA")]
        UNITED_STATES,
        [StringValue("GBR")]
        UNITED_KINGDOM,
        [StringValue("CAN")]
        CANADA,
        [StringValue("MEX")]
        MEXICO
    }

    #endregion

    public enum BkgConstantType
    {
        [StringValue("VDTE")]
        DATE,
        [StringValue("VNUM")]
        NUMEIC,
        [StringValue("VTXT")]
        TEXT,
        [StringValue("VBL")]
        BOOL,
        [StringValue("VEMT")]
        EMPTY,
        [StringValue("TDAY")]
        DAY,
        [StringValue("TMTH")]
        MONTH,
        [StringValue("TYR")]
        YEAR,
        [StringValue("LCTY")]
        CITY,
        [StringValue("LCNTY")]
        COUNTY,
        [StringValue("LSTAT")]
        STATE,
        [StringValue("LCNTRY")]
        COUNTRY,
    }

    public enum BkgOrderDetailScreenType
    {
        [StringValue("AdminBkgOrderDetail")]
        AdminBkgOrderDetail,
        [StringValue("ClientAdminBkgOrderDetail")]
        ClientAdminBkgOrderDetail
    }
    public enum BkgOrderDateType
    {
        [StringValue("0")]
        Created,
        [StringValue("1")]
        Paid,
        [StringValue("2")]
        Completed
    }
    #region LKP Svc Line Item Dispatch Status
    public enum SvcLineItemDispatchStatus
    {
        [StringValue("AAAA")]
        NOT_DISPATCHED,
        [StringValue("AAAB")]
        DISPATCHED,
        [StringValue("AAAC")]
        DISPATCHED_ERROR,
        [StringValue("AAAD")]
        DISPTACH_ON_HOLD_WAITING_FOR_EDS_DATA
    }
    #endregion

    #region Disclaimer Document Enums

    public enum DislkpDocumentStatus
    {
        [StringValue("AAAA")]
        NEW,
        [StringValue("AAAB")]
        UPLOADED,
        [StringValue("AAAC")]
        SCANNED,
        [StringValue("AAAD")]
        RECIEVED,
        [StringValue("AAAE")]
        REVIEWED,
        [StringValue("AAAF")]
        STORED,
        [StringValue("AAAG")]
        ARCHIVED
    }

    public enum DislkpDocumentType
    {
        [StringValue("AAAA")]
        SERVICE_FORM,
        [StringValue("AAAB")]
        RETURNED_FORM,
        [StringValue("AAAC")]
        DISCLOSURE_AND_RELEASE,
        [StringValue("AAAD")]
        COMPLETED_ORDER_RESULTS,
        [StringValue("AAAE")]
        COMPLETED_SERVICE_GROUP_REPORT,
        [StringValue("AAAF")]
        COMPLETED_ORDER_ITEM_REPORT,
        [StringValue("AAAG")]
        IN_PROCESS_ORDERS_REPORT,
        [StringValue("AAAH")]
        DISCLOSURE_AND_RELEASE_TEMPLATE,
        [StringValue("AAAI")]
        JOB_SEND_CLIENT_INDIVIDUAL_SERVICE_GROUP_RESULTS,
        [StringValue("AAAJ")]
        JOB_COMPLETED_ORDER_ITEMS_FLAGGED_ONLY,
        [StringValue("AAAK")]
        JOB_SEND_ORDERS_WITH_COLORS,
        [StringValue("AAAL")]
        INVOICE_REPORT,
        [StringValue("AAAM")]
        EDS_DEFAULT_AUTHORIZATION_FORM,
        [StringValue("AAAO")]
        EMPLOYMENT_DISCLOSURE_FORM,
        [StringValue("AAAP")]
        USER_ATTESTATION_DISCLOSURE_FORM_CLIENT_ADMIN,
        [StringValue("AAAQ")]
        USER_ATTESTATION_DISCLOSURE_FORM_SHARED_USER,
        [StringValue("AAAR")]
        ADDITIONAL_DOCUMENTS,
        [StringValue("AAAS")]
        BADGE_FORM,
        [StringValue("AAAT")]
        USER_GUIDE_FOR_AGENCY_USER,
        [StringValue("AAAU")]
        FINGERPRINT_DOCUMENT
    }

    #endregion

    #region Custom Form
    public enum BkgCustomForm
    {
        [StringValue("AAAA")]
        Personal_and_residential_Information

    }

    public enum CustomFormType
    {
        [StringValue("AAAA")]
        Fresh_Order_Form,
        [StringValue("AAAB")]
        Supplement_Order_Form
    }

    /// <summary>
    /// Enum for different modes of the Page load or Post back events, of Custom forms
    /// </summary>
    public enum CustomFormPageMode
    {
        Default = 0,

        /// <summary>
        /// Enum to represent Page Load by Edit event from Order review
        /// </summary>
        ReviewEditClicked = 5,

        /// <summary>
        /// Enum to represent Page Load by Previous button event 
        /// </summary>
        PreviousClicked = 10,

        /// <summary>
        /// Enum to represent Page Load by 'Next' button event 
        /// </summary>
        NextClicked = 15
    }
    #endregion

    public enum AttributeAccessType
    {
        [StringValue("chkPermission1")]
        FullAcess,
        [StringValue("chkPermission3")]
        ReadOnly,
        [StringValue("chkPermission4")]
        NoAccess
    }

    #region D & R
    public enum AttributeMappingFieldName
    {
        [StringValue("dateofbirth")]
        DateofBirth,
        [StringValue("digitallysigned")]
        DigitallySigned,
        [StringValue("datedigitallysigned")]
        DateDigitallySigned,
        [StringValue("datesigned")]
        DateSigned,
        [StringValue("datesatcurrentresidency")]
        DatesAtCurrentResidency
    }
    #endregion

    public enum BkgDataPointType
    {
        [StringValue("AAAA")]
        SERVICE_GROUP_COMPLETED_DATE,
        [StringValue("AAAB")]
        SERVICE_COMPLETED_DATE,
        [StringValue("AAAC")]
        PACKAGE_FLAGGED,
        [StringValue("AAAD")]
        SERVICE_GROUP_FLAGGED,
        [StringValue("AAAE")]
        ORDER_RESULT_DOCUMENT,
        [StringValue("AAAF")]
        SERVICE_GROUP_RESULT_DOCUMENT,
        [StringValue("AAAG")]
        SERVICE_LINE_ITEM_FLAGGED
    }

    //----------------------------------------------------
    public enum BkgSvcGrpReviewStatusType
    {
        [StringValue("AAAA")]
        NEW,
        [StringValue("AAAB")]
        IN_PROGRESS,
        [StringValue("AAAC")]
        FIRST_REVIEW,
        [StringValue("AAAD")]
        FIRST_REVIEW_COMPLETED,
        [StringValue("AAAE")]
        SECOND_REVIEW,
        [StringValue("AAAF")]
        SECOND_REVIEW_COMPLETED,
        [StringValue("AAAG")]
        AUTO_REVIEW_COMPLETED,
        [StringValue("AAAH")]
        MANUAL_REVIEW_COMPLETED,
        [StringValue("AAAI")]
        NO_REVIEW_REQUIRED
    }

    public enum BkgSvcGrpStatusType
    {
        [StringValue("AAAA")]
        NEW,
        [StringValue("AAAB")]
        IN_PROGRESS,
        [StringValue("AAAC")]
        COMPLETED,
        [StringValue("AAAD")]
        CANCELLED
    }


    public enum PackageAvailability
    {
        [StringValue("AAAA")]
        AVAILABLE_FOR_ORDER,
        [StringValue("AAAB")]
        NOT_AVAILABLE_FOR_ORDER
    }

    public enum TaskStatusType
    {
        [StringValue("AAAA")]
        PENDING,
        [StringValue("AAAB")]
        COMPLETED,
        [StringValue("AAAC")]
        ERROR,
        [StringValue("AAAD")]
        ABANDONED
    }

    public enum TaskType
    {
        [StringValue("AAAA")]
        INVOICEORDERBULKAPPROVE,
        [StringValue("AAAB")]
        RENEWEXPIREDINVOICESUBSCRIPTION,
        [StringValue("AAAC")]
        DAILY_BACKGROUND_REPORT,
        [StringValue("AAAD")]
        SALES_FORCE,
        [StringValue("AAAE")]
        AUTO_ARCHIVE_EXPIRED_SUBSCRIPTION,
        [StringValue("AAAF")]
        AUTO_ARCHIVE_EXPIRED_SUBSCRIPTION_NOTIFICATION,
        [StringValue("AAAG")]
        WEEKLY_BACKGROUND_REPORT,
        //UAT-1852 : If a Service Group is complete, but other service groups are not complete within an order, the system should send the applicant an email.
        [StringValue("AAAH")]
        SEND_MAIL_BKG_SVC_GROUPS_COMPLETION,
        //UAT-3795
        [StringValue("AAAI")]
        SEND_MAIL_NON_COMPLIANT_STUDENTS_WEEKLY_REPORT,
        [StringValue("AAAJ")]
        sync_Requirement_Verification_DataTo_FlatTable
    }

    public enum RecurringBackgroundReport
    {
        [StringValue("AAAC")]
        DAILY_BACKGROUND_REPORT,
        [StringValue("AAAG")]
        WEEKLY_BACKGROUND_REPORT
    }

    #region UAT-523 Category Level Exception
    public enum lkpCategoryExceptionStatus
    {
        [StringValue("AAAA")]
        EXCEPTION_APPLIED,
        [StringValue("AAAB")]
        EXCEPTION_REJECTED,
        [StringValue("AAAC")]
        EXCEPTION_EXPIRED,
        [StringValue("AAAE")]
        EXCEPTION_APPROVED,
        [StringValue("AAAD")]
        APPROVED_BY_OVERRIDE,
        [StringValue("AAAF")]
        APPROVED_BY_OVERRIDE_DISABLE,
        [StringValue("XXXX")]
        DEFAULT
    }
    #endregion

    public enum SearchQueueType
    {
        ApplicantPortfolioSearch,
        ApplicantComprehensiveSearch,
        ClientUserSearch
    }

    public enum ServiceItemType
    {
        [StringValue("AAAA")]
        SingleSearch,
        [StringValue("AAAB")]
        PerOccurrenceSearch,
        [StringValue("AAAC")]
        Aliassearch,
        [StringValue("AAAD")]
        Statesearch,
        [StringValue("AAAE")]
        CountySearch
    }

    #region UAT-806 Creation of granular permissions for Client Admin users
    public enum EnumSystemEntity
    {
        [StringValue("AAAA")]
        DOB,
        [StringValue("AAAB")]
        SSN,
        [StringValue("AAAC")]
        BKG_ORDER_COLOR_FLAG,
        [StringValue("AAAD")]
        BKG_ORDER_RESULT_REPORT,
        [StringValue("AAAE")]
        MANAGE_CONTRACT,
        /// <summary>
        /// UAT-1784: Permission code to manage Add/Update Rotation Package.
        /// </summary>
        [StringValue("AAAF")]
        MANAGE_ROTATION_PACKAGE,
        /// <summary>
        /// UAT-1784: Permission code to manage assigning the Rotation package to Rotation
        /// </summary>
        [StringValue("AAAG")]
        ASSIGN_ROTATION_PACKAGE,
        /// <summary>
        /// UAT-2056, Buttons should be permissions based (I didn't specifically call this out in the request and should have). (C14)
        /// </summary>
        [StringValue("AAAH")]
        STUDENT_BUCKET_ASSIGNMENT
            /// <summary>
            /// UAT-3010, Granular Permission to turn off Archive ability. Achive Buttons should be permission based.
            /// </summary>
        ,
        [StringValue("AAAJ")]
        ARCHIVE_ABILITY,
        [StringValue("AAAK")]
        ROTATION_NOTIFICATION
            ,
        [StringValue("AAAL")]
        BACKGROUND_COMPLETION_STATUS,
        [StringValue("AAAM")]
        BACKGROUND_COMPLETION_RESULT

    }

    public enum EnumSystemPermissionCode
    {
        [StringValue("FULL")]
        FULL_PERMISSION,
        [StringValue("NO")]
        NO_ACCESS_PERMISSION,
        [StringValue("READ")]
        MASKED_READ_PERMISSION,
        [StringValue("BORD")]
        Order_Result_Document,
        [StringValue("BORVS")]
        Order_Result_Vendor_Status,
        [StringValue("BSGRD")]
        Service_Group_Result_Document,
        [StringValue("BSGVS")]
        Service_Group_Vendor_Status,
        [StringValue("BSLIVS")]
        Service_Line_Item_Vendor_Status,
        [StringValue("BODSP")]
        Ability_To_See_Order_Details_Screen_On_Off,
        [StringValue("BNOTE")]
        MANAGE_BKG_ORDER_NOTES,
        [StringValue("BNONE")]
        NONE,
        [StringValue("USRPER")]
        ASSIGN_UNASSIGN_USER_GROUP,
        [StringValue("ROTPER")]
        ASSIGN_UNASSIGN_ROTATION,
        [StringValue("ARCPER")]
        ARCHIVE_UNARCHIVE,
        [StringValue("MSGPER")]
        SEND_MESSAGE,
        [StringValue("NONPER")]
        NONPER,
        [StringValue("NFARC")]
        NOTIFICATION_FOR_ALL_ROTATION_CATEGORIES_APPROVED_OR_PENDING_REVIEW,
        [StringValue("NFIRVQ")]
        NOTIFICATION_FOR_SUBMITTED_ITEM_INTO_REQUIREMENT_VERIFICATION_QUEUE
    }
    #endregion

    public enum OrderPaymentType
    {
        [StringValue("Credit Card")]
        CreditCard,
        [StringValue("Money Order")]
        MoneyOrder,
        [StringValue("Invoice With Approval")]
        InvoiceWithApproval,
        [StringValue("PayPal")]
        PayPal,
        [StringValue("Invoice to Institution")]
        InvoicetoInstitution,
        [StringValue("Offline Settlement")]
        OfflineSettlement
    }

    public enum PartialOrderCancellationType
    {
        [StringValue("AAAA")]
        COMPLIANCE_PACKAGE,
        [StringValue("AAAB")]
        BACKGROUND_PACKAGE,
        [StringValue("AAAC")]
        COMPLIANCE_BACKGROUND_PACKAGES
    }

    #region Data Feed API Enums

    public enum DataFeedIntervalMode
    {
        [StringValue("AAAA")]
        CurrentMonth,
        [StringValue("AAAB")]
        NoofDays,
        [StringValue("AAAC")]
        NoOfMonthDays
    }

    public enum DataFeedInvokeResult
    {
        [StringValue("AAAA")]
        Passed,
        [StringValue("AAAB")]
        Failed
    }

    #endregion

    /// <summary>
    /// List of Background services, for which the history is maintained in BackgroundServiceExecutionHistory table
    /// </summary>
    public enum BackgroundServiceType
    {
        [StringValue("DailyColorFlaggedOrderReport")]
        Daily_Color_Flagged_Report,
        [StringValue("AutoArchiveExpiredSubscriptionNotification")]
        DAILY_AUTO_ARCHIVE_EXPIRED_SUBSCRIPTION_NOTIFICATION
    }

    /// <summary>
    /// Represents the enum for the HTTP request types
    /// </summary>
    public enum HttpRequestTypes
    {
        [StringValue("POST")]
        POST
    }

    /// <summary>
    /// Represents the enum for content-type for the HTTP request 
    /// </summary>
    public enum HttpRequestContentTypes
    {
        [StringValue("application/x-www-form-urlencoded")]
        DEFAULT,
        [StringValue("application/json")]
        JSON
    }

    /// <summary>
    /// Enum to represent the uploading of the Compliance Data to Third Party like Sales Force
    /// </summary>
    public enum ThirdPartyUploadStatus
    {
        [StringValue("AAAA")]
        PENDING,
        [StringValue("AAAB")]
        ERROR,
        [StringValue("AAAC")]
        UPLOAD_COMPLETE
    }

    /// <summary>
    /// Enum to represent the AppConfiguration table settings for a Tenant
    /// </summary>
    public enum TenantAppConfigurations
    {
        [StringValue("UploadComplianceData")]
        UPLOAD_COMPLIANCE_DATA_KEY
    }

    public enum DispatchType
    {
        Manual = 0,
        Automatic = 1
    }

    public enum CommunicationTemplatePlaceHoldersProperty
    {
        UserFullName = 0,
        OrderNo = 1,
        OrderDate = 2,
        OrderStatus = 3,
        PackageName = 4,
        InstituteName = 5,
        InstitutionUrl = 6,
        ServiceName = 7,
        ServiceFormName = 8,
        ServiceFormDispatchDate = 9,
        ServiceGroupName = 10
    }
    public enum ServiceFormMappingType
    {
        [StringValue("AAAA")]
        AllInstitute,
        [StringValue("AAAB")]
        SpecificInstitute
    }

    #region lkpComplianceSubscriptionArchiveChangeType [UAT-977:Additional work towards archive ability]
    public enum ComplianceSubscriptionArchiveChangeType
    {
        [StringValue("AAAA")]
        SET_TO_ARCHIVE,
        [StringValue("AAAB")]
        UN_ARCHIVE_REQUESTED,
        [StringValue("AAAC")]
        UN_ARCHIVE_APPROVED,
        [StringValue("AAAD")]
        UN_ARCHIVE_REJECTED,
        [StringValue("AAAE")]
        RE_PURCHASE,
        [StringValue("AAAF")]
        UN_ARCHIVE_BY_ADMIN
    }
    #endregion

    #region Service and Job Enums
    public enum ServiceName
    {
        [StringValue("EmailDispatcherService")]
        EmailDispatcherService,
        [StringValue("BkgOrderService")]
        BkgOrderService,
        [StringValue("AutoArchival")]
        AutoArchival
    }

    public enum JobName
    {
        [StringValue("SystemCommunicationDelivery")]
        SystemCommunicationDelivery,
        [StringValue("SendMailBeforeExpiry")]
        SendMailBeforeExpiry,
        [StringValue("SendMailAfterExpiry")]
        SendMailAfterExpiry,
        [StringValue("SendMailForPendingPackage")]
        SendMailForPendingPackage,
        [StringValue("ProcessItemExpiry")]
        ProcessItemExpiry,
        [StringValue("SendMailForExpiringItems")]
        SendMailForExpiringItems,
        [StringValue("SendMailForExpiredItems")]
        SendMailForExpiredItems,
        [StringValue("ProcessCategoryExpiry")]
        ProcessCategoryExpiry,
        [StringValue("SendMailForDispatchedServiceForm")]
        SendMailForDispatchedServiceForm,
        [StringValue("SendMailForScheduleActionExecuteCategoryrules")]
        SendMailForScheduleActionExecuteCategoryrules,
        [StringValue("SendMailForNonComplianceCategories")]
        SendMailForNonComplianceCategories,
        [StringValue("CreateMobilityInstance")]
        CreateMobilityInstance,
        [StringValue("InsertNodeTransition")]
        InsertNodeTransition,
        [StringValue("AutomaticNodeTransitionMovement")]
        AutomaticNodeTransitionMovement,
        [StringValue("CopyPackgeDataExecuteRule")]
        CopyPackgeDataExecuteRule,
        [StringValue("PopulateAdminWidgetData")]
        PopulateAdminWidgetData,
        [StringValue("SystemServiceTrigger")]
        SystemServiceTrigger,
        [StringValue("SendNagMails")]
        SendNagMails,
        [StringValue("QueueImagingData")]
        QueueImagingData,
        [StringValue("SendMailForDeadline")]
        SendMailForDeadline,
        [StringValue("ApproveInvoiceOrderExecuteRule")]
        ApproveInvoiceOrderExecuteRule,
        [StringValue("ScheduleTaskForAutoRenewExpiredinvoiceSubscription")]
        ScheduleTaskForAutoRenewExpiredinvoiceSubscription,
        [StringValue("AutoArchiveExpiredSubscriptions")]
        AutoArchiveExpiredSubscriptions,
        [StringValue("SendColorFlaggedOrderReports")]
        SendColorFlaggedOrderReports,
        [StringValue("SalesForceScheduleTask")]
        SalesForceScheduleTask,
        [StringValue("SendNotificationForAutoArchivedExpiredSubscriptions")]
        SendNotificationForAutoArchivedExpiredSubscriptions,
        [StringValue("SendExternalVendorOrders")]
        SendExternalVendorOrders,
        [StringValue("UpdateExtVendorOrder")]
        UpdateExtVendorOrder,
        [StringValue("CreateBkgOrderNotification")]
        CreateBkgOrderNotification,
        [StringValue("UpdateBkgOrderColorStatus")]
        UpdateBkgOrderColorStatus,
        [StringValue("CreateBkgOrderResultCompletedNotification")]
        CreateBkgOrderResultCompletedNotification,
        [StringValue("CreateServiceGroupCompletedNotification")]
        CreateServiceGroupCompletedNotification,
        [StringValue("CreateBkgFlaggedOrderResultCompletedNotification")]
        CreateBkgFlaggedOrderResultCompletedNotification,
        [StringValue("CreateJiraTicketForFailedOrders")]
        CreateJiraTicketForFailedOrders,
        [StringValue("SendBkgFlaggedOrderCompletedEmploymentNotification")]
        SendBkgFlaggedOrderCompletedEmploymentNotification,
        [StringValue("ScheduleActionExecuteRulesOnObjectDeletion")]
        ScheduleActionExecuteRulesOnObjectDeletion,
        [StringValue("ProcessCategoryComplianceRqd")]
        ProcessCategoryComplianceRqd,
        [StringValue("NotificationUpcomingNonComplianceRequiredCategoryAction")]
        NotificationUpcomingNonComplianceRequiredCategoryAction,
        [StringValue("ProcessRequirementItemExpiry")]
        ProcessRequirementItemExpiry,
        [StringValue("ProcessRotationToArchive")]
        ProcessRotationToArchive,  //UAT-3139
        [StringValue("RotationAboutToStart")]
        RotationAboutToStart,
        [StringValue("NotificationForIncompleteOnlineOrders")]
        NotificationForIncompleteOnlineOrders,
        [StringValue("CreateEmploymentFlaggedServiceGroupCompletedNotification")]
        CreateEmploymentFlaggedServiceGroupCompletedNotification,
        [StringValue("CreateFlaggedServiceGroupCompletedNotification")]
        CreateFlaggedServiceGroupCompletedNotification,
        [StringValue("ComplianceDocumentCompletion")]
        ComplianceDocumentCompletion,
        [StringValue("ComplianceAuditDataSynchronise")]
        ComplianceAuditDataSynchronise,
        [StringValue("ReconcillationQueueSync")]
        ReconcillationQueueSync,
        //UAt-1852 : If a Service Group is complete, but other service groups are not complete within an order, the system should send the applicant an email.
        [StringValue("ServiceGroupCompletionMailForApplicant")]
        SvcGrpCompletionMailForApplicant,
        [StringValue("CreateBulkOrder")]
        CreateBulkOrder,
        [StringValue("ProcessRotationCategoryComplianceRqd")]
        ProcessRotationCategoryComplianceRqd,
        [StringValue("RequirementScheduleActionExecutePackageRules")]
        RequirementScheduleActionExecutePackageRules,
        [StringValue("RequirementScheduleActionExecuteCategoryRules")]
        RequirementScheduleActionExecuteCategoryRules,
        [StringValue("RequirementScheduleActionDataSyncRules")]
        RequirementScheduleActionDataSyncRules,
        [StringValue("CopyComplianceDataToRequirement")]
        CopyComplianceDataToRequirement,
        [StringValue("CreateRequirementSnapshotOnRotationEnd")]
        CreateRequirementSnapshotOnRotationEnd,
        [StringValue("UploadClientData")]
        UploadClientData,
        //UAT-2370 : Supplement SSN Processing updates
        [StringValue("SendEmailToApplicantWhenExceptionInSSNResult")]
        SendEmailToApplicantWhenExceptionInSSNResult,
        [StringValue("RequirementPackageSyn")]
        RequirementPackageSynching,
        [StringValue("AutoArchival")]
        AutoArchival,
        //UAT-2603
        [StringValue("RotationDataMovement")]
        RotationDataMovement,
        [StringValue("ConvertAndMergeFailedApplicantDocument")]
        ConvertAndMergeFailedApplicantDocument,
        [StringValue("AutomaticPackageInvitation")]
        AutomaticPackageInvitation,
        [StringValue("UpdatedApplicantRequirementsNotification")]//UAT-3059
        UpdatedApplicantRequirementsNotification,
        //UAT-2513
        [StringValue("BatchRotationUploadThroughExcel")]
        BatchRotationUploadThroughExcel,
        [StringValue("CreateBulkOrderForRepeatedSearch")]
        CreateBulkOrderForRepeatedSearch,
        //UAT-3112
        [StringValue("SendBadgeFormNotifications")]
        SendBadgeFormNotifications,
        //UAT-2960
        [StringValue("UpdateApplicantForAlumniAccess")]
        UpdateApplicantForAlumniAccess,
        [StringValue("CopyComplianceToCompliance")]
        CopyComplianceToCompliance,
        [StringValue("BackgroundCopyPackageData")]
        BackgroundCopyPackageData,
        //UAT-3485
        [StringValue("SendMailForRequirementExpiringItems")]
        SendMailForRequirementExpiringItems,
        [StringValue("ComplianceExceptionExpiry")]
        ComplianceExceptionExpiry,
        //UAT-3137
        [StringValue("SendEmailRequirementCategoriesBeforeGoingToBeRequired")]
        SendEmailRequirementCategoriesBeforeGoingToBeRequired,
        [StringValue("BackgroundAppointmentDigestionCall")]
        BackgroundAppointmentDigestionCall,
        [StringValue("UpdateFingerPrintOrder")]
        UpdateFingerPrintOrder,
        //UAT-3669
        [StringValue("SendAlertMailForWebCCFError")]
        SendAlertMailForWebCCFError,
        //UAT-3761
        [StringValue("UpdateAppointmentStatusToMissed")]
        UpdateAppointmentStatusToMissed,
        [StringValue("SendMissedAppointmentMail")]
        SendMissedAppointmentMail,
        //UAT-3734
        [StringValue("SendOffTimeRevokedMail")]
        SendOffTimeRevokedMail,
        [StringValue("CDRExportData")]
        CDRExportData,
        //UAT - 3851
        [StringValue("ChangeStatusForUpdatedCBIResultFileService")]
        ChangeStatusForUpdatedCBIResultFileService,
        //UAT - 3950
        [StringValue("AutomaticallyArchiveRotation")]
        AutomaticallyArchiveRotation,
        //UAT-3820
        [StringValue("SendMailForReceivedFromStudentServiceFormStatus")]
        SendMailForReceivedFromStudentServiceFormStatus,
        //UAT-3795
        [StringValue("SendNonCompliantStudentsWeeklyReport")]
        SendNonCompliantStudentsWeeklyReport,
        //UAT - 4088
        [StringValue("RescheduleManuallyRejectedOutOfStateOrder")]
        RescheduleManuallyRejectedOutOfStateOrder,
        [StringValue("GetDSOrderDataFromClearStar")]
        GetDSOrderDataFromClearStar,
        [StringValue("NotificationForInvitationPendingOrderstoApplicant")]
        NotificationForInvitationPendingOrderstoApplicant,
        [StringValue("NotificationForDraftOrderstoAdmin")]
        NotificationForDraftOrderstoAdmin,
        [StringValue("DeleteDraftOrder")]
        DeleteDraftOrder,
        [StringValue("NotificationtoAdminForBkgSvcGroupCompletedOrder")]
        NotificationtoAdminForBkgSvcGroupCompletedOrder,
        [StringValue("NotificationtoAdminForBkgSvcGroupCompletedOrderWithAttachment")]
        NotificationtoAdminForBkgSvcGroupCompletedOrderWithAttachment,
        [StringValue("NotificationForChangeOrdersStatusCompletedToArchived")]
        NotificationForChangeOrdersStatusCompletedToArchived,
        [StringValue("NotificationForChangeServiceFormStatusToInProcessAgency")]
        NotificationForChangeServiceFormStatusToInProcessAgency,
        [StringValue("ProcessReconciliationQueueProductivityData")]
        ProcessReconciliationQueueProductivityData,
        [StringValue("NotificationForFingerprintingExceededTAT")]
        NotificationForFingerprintingExceededTAT,
        //UAT-4657
        [StringValue("RequirementPackageVersioning")]
        RequirementPackageVersioning,
        [StringValue("RequirementCategoryDisassociation")]
        RequirementCategoryDisassociation

    }
    #endregion

    #region UAT-1049:Admin Data Entry
    public enum DataEntryQueueType
    {
        [StringValue("DEAQ")]
        DATA_ENTRY_ASSIGNMENT_QUEUE,
        [StringValue("DEUWQ")]
        DATA_ENTRY_USER_WORK_QUEUE,

    }

    /// <summary>
    /// Enum for the 'lkpDataEntryDocumentStatus' table
    /// </summary>
    public enum DataEntryDocumentStatus
    {
        [StringValue("AAAA")]
        NEW,
        [StringValue("AAAB")]
        COMPLETE,
        [StringValue("AAAC")]
        NEW_QUEUED,
        [StringValue("AAAD")]
        IN_PROGRESS,
        [StringValue("AAAE")]
        DOCUMENT_REJECTED,
    }
    #endregion

    public enum BackgroundReportType
    {
        [StringValue("OrderCompletion")]
        ORDER_COMPLETION,
        [StringValue("BkgSvcGrpCompletedCode")]
        DAILY_SERVICE_GROUP_COMPLETION_REPORT,
        [StringValue("BkgOrderStatusColorCode")]
        DAILY_FLAGGED_ORDER_COMPLETION_REPORT,
        [StringValue("BkgWeeklySvcGrpCompletedCode")]
        WEEKLY_SERVICE_GROUP_COMPLETION_REPORT
    }

    public enum ReportTypeEnum
    {
        [StringValue("PassportReportDataOnly")]
        PASSPORT_REPORT_DATA_ONLY,
        [StringValue("PassportReportMulti")]
        PASSPORT_REPORT_MULTI,
        [StringValue("Attestation")]
        ATTESTATION_REPORT,
        [StringValue("VerticalAttestation")]
        ATTESTATION_REPORT_VERTICAL,
        [StringValue("ConsolidatedAttestation")]
        ATTESTATION_REPORT_CONSOLIADTED,
        [StringValue("CombinedAttestationReportWithoutSignature")]
        COMBINED_ATTESTATION_REPORT_WITHOUT_SIGNATURE,
        [StringValue("ConsolidatedPassportReport")]
        CONSOLIDATEDPASSPORTREPORT,
        [StringValue("ContractExportContractOnly")]
        CONTRACT_EXPORT_CONTRACT_ONLY,
        [StringValue("ContractExportSiteOnly")]
        CONTRACT_EXPORT_SITE_ONLY,
        [StringValue("ContractExportBoth")]
        CONTRACT_EXPORT_CONTRACT_BOTH,
        [StringValue("PassportReportMultiApprovedItems")]
        PASSPORT_REPORT_MULTI_APPROVED_ITEMS,
    }

    public enum AttestationReportType
    {
        [StringValue("AAAA")]
        HORIZONTAL,
        [StringValue("AAAB")]
        VERTICAL,
        [StringValue("AAAC")]
        CONSOLIDATED,
        [StringValue("AAAD")]
        CONSOLIDATED_WITHOUT_SIGN
    }


    #region UAT-1078
    public enum VideoType
    {
        [StringValue("https://player.vimeo.com/video/106016415")]
        CAHV, //Create Account Help Video
        [StringValue("https://player.vimeo.com/video/119982357")]
        ESHV,  //E-Signature Help Video
        [StringValue("https://player.vimeo.com/video/106016719")]
        COHV, //Create Order Help Video
        [StringValue("https://player.vimeo.com/video/106016720")]
        UDHV, //Upload Document Help Video
        [StringValue("https://player.vimeo.com/video/106017637")]
        DEHV,  //Data Entry Help Video
        [StringValue("https://player.vimeo.com/video/126823530")]
        FEEV,  //Fee Explaination Video
        [StringValue("https://player.vimeo.com/video/128259313")]
        MIHV,  //Manage Invitation Help Video
    }
    #endregion

    #region Profile Sharing

    /// <summary>
    /// Represnts the Invitation source types 
    /// </summary>
    public enum InvitationSourceTypes
    {
        [StringValue("AAAA")]
        ADMIN,
        [StringValue("AAAB")]
        CLIENTADMIN,
        [StringValue("AAAC")]
        APPLICANT
    }


    /// <summary>
    /// Represnts the Expiration types for the Profile sharing
    /// </summary>
    public enum InvitationExpirationTypes
    {
        [StringValue("AAAA")]
        NO_EXPIRATION_CRITERIA,
        [StringValue("AAAB")]
        NUMBER_OF_VIEWS,
        [StringValue("AAAC")]
        SPECIFIC_DATE
    }

    /// <summary>
    /// Represnts the Applicant MetaData that can be shared in Profile Sharing
    /// </summary>
    public enum LkpApplicanteMetaData
    {
        [StringValue("AAAA")]
        NAME_FIELD,
        [StringValue("AAAB")]
        EMAIL_ADDRESS_FIELD,
        [StringValue("AAAC")]
        GENDER_FIELD,
        [StringValue("AAAD")]
        SECONDARY_EMAIL_ADDRESS,
        [StringValue("AAAE")]
        ADDRESS_FIELD,
        [StringValue("AAAF")]
        PHONE_NUMBER_FIELD,

        //These fields are added to enum but not added to lookup table "ApplicantInvitationMetaData"
        //UAT-3006
        [StringValue("BAAA")]
        SCHOOL_CONTACT_NAME,
        [StringValue("BAAB")]
        SCHOOL_CONTACT_EMAIL_ADDRESS,
        [StringValue("BAAC")]
        INSTRUCTOR_PRECEPTOR
    }

    /// <summary>
    /// Represnts the SharedInfo master type i.e. Compliance or Background
    /// </summary>
    public enum SharedInfoMasterType
    {
        [StringValue("IMM")]
        MASTERTYPE_COMPLIANCE,
        [StringValue("BKG")]
        MASTERTYPE_BACKGROUND,
        [StringValue("REQROT")]
        MASTERTYPE_REQUIREMENT_ROTATION
    }

    /// <summary>
    /// Represnts the SharedInfo type  
    /// </summary>
    public enum SharedInfoType
    {
        [StringValue("AAAA")]
        SUMMARY_INFO,
        [StringValue("AAAB")]
        DATA_DOCUMENTS_SUMMARY_INFO,
        [StringValue("AAAC")]
        COMPLIANCE_NONE,
        //UAT 1640
        //[StringValue("AAAD")]
        //COLOR_FLAG,
        [StringValue("AAAE")]
        COMPLETED_RESULT_REPORT,
        [StringValue("AAAF")]
        FLAGGED_ONLY_RESULT_REPORT,
        [StringValue("AAAG")]
        FLAG_STATUS,
        [StringValue("AAAH")]
        REQUIREMENT_ROTATION_ALL,
        [StringValue("AAAI")]
        REQUIREMENT_ROTATION_SUMMARY,
        [StringValue("AAAJ")]
        REQUIREMENT_ROTATION_NONE,

        [StringValue("AAAK")]
        COMPLIANCE_ATTESTATION_ONLY,
        [StringValue("AAAL")]
        BACKGROUND_ATTESTATION_ONLY,
        [StringValue("AAAM")]
        REQUIREMENT_ATTESTATION_ONLY
    }

    /// <summary>
    /// Represnts the Invitation Status types for the Profile sharing
    /// </summary>
    public enum LkpInviationStatusTypes
    {
        [StringValue("AAAA")]
        NEW,
        [StringValue("AAAB")]
        VIEWED_BY_INVITEE,
        [StringValue("AAAC")]
        EXPIRED,
        [StringValue("AAAD")]
        REVOKED,
        [StringValue("AAAE")]
        DATA_CHANGED_INVITATION_REVOKED,
        [StringValue("AAAF")]
        ADMIN_INITLIAZED,
        [StringValue("AAAG")]
        INVITATION_SCHEDULED
    }

    #endregion
    public enum SubscriptionType
    {
        [StringValue("ARSB")]
        ARCHIVED_SUBSCRIPTIONS,
        [StringValue("UARS")]
        UNARCHIVAL_REQUESTED_SUBSCRIPTIONS
    }

    public enum HierarchyPermissionTypes
    {
        [StringValue("AAAA")]
        COMPLIANCE,
        [StringValue("AAAB")]
        BACKGROUND,
        [StringValue("AAAC")]
        COMPLIANCE_AND_BACKGROUND
    }

    public enum AttestationDocumentTypes
    {
        [StringValue("_Full")] //will be considered as TrackingAndScreeningWithFlag
        FULL,
        [StringValue("_TrackingOnly")]
        TRACKING_ONLY,
        [StringValue("_ScreeningOnly")] // will be considered as ScreeningWithFlag
        SCREENING_ONLY,
        [StringValue("_None")]
        NONE,
        [StringValue("_TrackingAndScreeningWithFlag")]
        TRACKING_AND_SCREENING_WITH_FLAG,
        [StringValue("_TrackingAndScreeningWithoutFlag")]
        TRACKING_AND_SCREENING_WITHOUT_FLAG,
        [StringValue("_ScreeningWithoutFlag")]
        SCREENING_WITHOUT_FLAG,


        [StringValue("_ScreeningAndTracking")] // will be considered as Screening + Tracking
        SCREENING_AND_TRACKING,
        [StringValue("_TrackingAndScreeningAndRotationWithoutFlag")]
        TRACKING_SCREENING__ROTATION_WITHOUT_FLAG,
        [StringValue("_RotationOnly")]
        ROTATION_ONLY,
        [StringValue("_RotationAndTracking")] // will be considered as Rotation + Tracking
        ROTATION_AND_TRACKING,
        [StringValue("_ScreeningAndRotationWithFlag")] // will be considered as Rotation + Tracking
        SCREENING_AND_ROTATION_WITH_FLAG,
        [StringValue("_RotationAndScreeningWithoutFlag")] // will be considered as ScreeningWithoutFlag + Rotations
        ROTATION_AND_SCREENING_WITHOUT_FLAG,
        [StringValue("_AttestationForm")]
        ATTESTATION_FORM,
    }

    public enum AttestationDocumentLinkTypes
    {
        [StringValue("Full")]
        FULL,
        [StringValue("Tracking Only")]
        TRACKING_ONLY,
        [StringValue("Screening Only")]
        SCREENING_ONLY,
        [StringValue("None")]
        NONE,
        [StringValue("Tracking & Screening with Flag")]
        TRACKING_AND_SCREENING_WITH_FLAG,
        [StringValue("Tracking & Screening w/o Flag")]
        TRACKING_AND_SCREENING_WITHOUT_FLAG,
        [StringValue("Screening w/o Flag")]
        SCREENING_WITHOUT_FLAG,

        [StringValue("Screening & Tracking")] // will be considered as Screening + Tracking
        SCREENING_AND_TRACKING,
        [StringValue("Tracking & Screening & Rotation w/o Flag")]
        TRACKING_SCREENING__ROTATION_WITHOUT_FLAG,
        [StringValue("Rotation Only")]
        ROTATION_ONLY,
        [StringValue("Rotation & Tracking")] // will be considered as Rotation + Tracking
        ROTATION_AND_TRACKING,
        [StringValue("Screening & Rotation with Flag")] // will be considered as Rotation + Tracking
        SCREENING_AND_ROTATION_WITH_FLAG,
        [StringValue("Rotation & Screening w/o Flag")] // will be considered as ScreeningWithoutFlag + Rotations
        ROTATION_AND_SCREENING_WITHOUT_FLAG
    }

    public enum lkpAgencyUserAttestationPermissionsEnum
    {
        [StringValue("AAAA")]
        Full,
        [StringValue("AAAB")]
        NONE,
        [StringValue("AAAC")]
        TRACKING_ONLY,
        [StringValue("AAAD")]
        SCREENING_ONLY,
        [StringValue("AAAE")]
        SCREENING_WITHOUT_FLAG,
        [StringValue("AAAF")]
        TRACKING_AND_SCREENING_WITHOUT_FLAG,
        [StringValue("AAAG")]
        TRACKING_AND_SCREENING_WITH_FLAG,
        [StringValue("AAAH")]
        SCREENING_AND_TRACKING,
        [StringValue("AAAI")]
        ROTATION_AND_TRACKING,
        [StringValue("AAAJ")]
        ROTATION_ONLY,
        [StringValue("AAAK")]
        SCREENING_AND_ROTATION_WITH_FLAG,
        [StringValue("AAAL")]
        ROTATION_AND_SCREENING_WITHOUT_FLAG,
        [StringValue("AAAM")]
        TRACKING_SCREENING_ROTATION_WITHOUT_FLAG
    }

    #region Rotation Packages

    public enum RequirementFieldDataType
    {
        [StringValue("AAAA")]
        DATE,
        [StringValue("AAAB")]
        UPLOAD_DOCUMENT,
        [StringValue("AAAC")]
        OPTIONS,
        [StringValue("AAAD")]
        VIEW_DOCUMENT,
        [StringValue("AAAE")]
        VIEW_VIDEO,
        [StringValue("AAAF")] //UAT-2701
        TEXT,
        [StringValue("AAAG")] //UAT-2701
        SIGNATURE,
    }

    public enum RequirementPackageType
    {
        [StringValue("AAAA")]
        APPLICANT_ROTATION_PACKAGE,
        [StringValue("AAAB")]
        INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE
    }

    public enum RequirementPackageActiveStatus
    {
        [StringValue("AAAA")]
        ALL,
        [StringValue("AAAB")]
        ACTIVE,
        [StringValue("AAAC")]
        INACTIVE
    }

    public enum DocumentAcroFieldType
    {
        [StringValue("AAAA")]
        FULL_NAME,
        [StringValue("AAAB")]
        CURRENT_DATE,
        [StringValue("AAAC")]
        SIGNATURE,
        [StringValue("AAAD")]
        FIRST_NAME,
        [StringValue("AAAE")]
        MIDDLE_NAME,
        [StringValue("AAAF")]
        LAST_NAME,
        [StringValue("AAAG")]
        FULL_SSN,
        [StringValue("AAAH")]
        DOB,
        [StringValue("AAAI")]
        PHONE_NUMBER,
        [StringValue("AAAJ")]
        EMAIL_ADDRESS,
        [StringValue("AAAK")]
        GENDER,
        [StringValue("AAAL")]
        ALIAS_NAME,
        [StringValue("AAAM")]
        ADDRESS_1,
        [StringValue("AAAN")]
        ADDRESS_2,
        [StringValue("AAAO")]
        CITY,
        [StringValue("AAAP")]
        STATE,
        [StringValue("AAAQ")]
        POSTAL_CODE,
        [StringValue("AAAR")]
        COUNTY,
        [StringValue("AAAS")]
        COUNTRY,
        [StringValue("AAAT")]
        INSTITUTION_NAME,
        [StringValue("AAAU")]
        LAST_FOUR_DIGIT_OF_SSN,
        [StringValue("AAAV")]
        BlankField
    }

    public enum RequirementFixedRuleType
    {
        [StringValue("AAAA")]
        ALL_CATEGORY,
        [StringValue("AAAB")]
        ANY_CATEGORY,
        [StringValue("AAAC")]
        ALL_ITEMS,
        [StringValue("AAAD")]
        ANY_ITEM,
        [StringValue("AAAE")]
        FIXED_EXPIRATION,
        [StringValue("AAAF")]
        ENTERED_DATE_BASED_EXPIRATION,
        [StringValue("AAAG")]
        ENTERED,
        [StringValue("AAAH")]
        ENTERED_AND_APPROVED,
        [StringValue("AAAI")]
        CATEGORY_IF_LOGICAL_EXPRESSION_IS_REQUIRED,
        [StringValue("AAAJ")]
        EXPIRES_CONDITIONALLY,
        [StringValue("AAAK")]
        LESS_THAN,
        [StringValue("AAAL")]
        GREATER_THAN
    }

    #endregion

    /// <summary>
    /// Enum to identify the Source of the ProfileSharing screen.
    /// </summary>
    public enum ProfileSharingScreenSource
    {
        [StringValue("AAAA")]
        ROTATION_DETAILS,

    }
    /// <summary>
    /// Enum to identify the Source of the Rotation Detail screen. UAT-2313
    /// </summary>
    public enum RotationDetailsScreenSource
    {
        [StringValue("AAAA")]
        MANAGE_ROTATION_BY_AGENCY,

        //UAT-4054
        [StringValue("AAAB")]
        INSTRCTR_PRECEPTR_ROTATION_SEARCH_URL,
    }

    /// <summary>
    /// Enum for the 'lkpProfileSharingInvitationGroupType' in Shared Data
    /// </summary>
    public enum ProfileSharingInvitationGroupTypes
    {
        [StringValue("AAAA")]
        PROFILE_SHARING_TYPE,
        [StringValue("AAAB")]
        ROTATION_SHARING_TYPE,
    }

    /// <summary>
    /// Represents the enum for 'lkpSharedSystemDocType' in SharedData
    /// </summary>
    public enum LKPSharedSystemDocumentTypes
    {
        [StringValue("AAAA")]
        CREDNTIAL,
        [StringValue("AAAB")]
        CV_RESUME,
        [StringValue("AAAC")]
        LICENSE,
        [StringValue("AAAD")]
        OTHERS,
        [StringValue("AAAE")]
        ATTESTATION_DOCUMENT,
        [StringValue("AAAF")]
        ATTESTATION_DOCUMENT_VERTICAL,
        [StringValue("AAAG")]
        ATTESTATION_DOCUMENT_CONSOLIDATED,
        [StringValue("AAAH")]
        ATTESTATION_DOCUMENT_CONSOLIDATED_WITHOUGHT_SIGN,
        [StringValue("AAAI")]
        SHARED_USER_INVITATION_DOCUMENT,
        [StringValue("AAAJ")]
        UPLOADED_INVITATION_DOCUMENT
    }

    /// <summary>
    /// Struct to represent the Query string constants used in ProfileSharing.
    /// </summary>
    public struct ProfileSharingQryString
    {
        public const String ApplicantId = "ApplicantId";
        public const String SelectedTenantId = "SelectedTenantId";
        public const String SourceScreen = "SrcScreen";
        public const String ReqPkgSubscriptionId = "RPSId";
        public const String RotationId = "RotationId";
        public const String IPRotationId = "IPRotationId";
        public const String AgencyId = "AgencyId";
        public const String RequirementPackageID = "RequirementPackageID";
        public const String TenantID = "TenantID";
        public const String InvitationID = "InvitationID";
        public const String InvitationGroupID = "InvitationGroupID";
        public const String InvitationGroupTypeCode = "InvitationGroupTypeCode";
        public const String ReqPkgTypeCode = "ReqPkgTypeCode";
        public const String Visibility = "Visibility";
        public const String ControlUseType = "ControlUseType";
        public const String IsOpenInReadOnlyMode = "False";
        public const String RotationInvitationIds = "RotationInvitationIds";
        public const String PageType = "PageType";
        public const String SourceScreenSource = "SrcScreenSource"; //UAT-2313
        public const String RotationAgencyIds = "RotationAgencyIds"; //UAT-2668
        public const String SendNotificationToAdminOnAppReqApproveRejection = "SendNotificationToAdminOnAppReqApproveRejection";
        public const String IsApplicantDropped = "IsApplicantDropped";
        public const String IsInstructorPreceptorPackage = "IsInstructorPreceptorPackage";//UAT-3338
        public const String ClientContactId = "ClientContactId";
        //public const String SharedOrganizationUserId = "SharedOrganizationUserId";//UAT-3338
        public const String IsNeedShowAllRotationInstructors = "IsNeedShowAllRotationInstructors";//UAT-3977
    }



    /// <summary>
    /// lkpClientContactType
    /// </summary>
    public enum ClientContactType
    {
        [StringValue("AAAA")]
        Instructor,
        [StringValue("AAAB")]
        Preceptor
    }

    #region UAT-1218
    ///<summary>
    ///Enum for lkpOrgUserType from Security and lkpSharedUserType in Tenant database
    ///</summary>
    public enum OrganizationUserType
    {
        [StringValue("AAAA")]
        ApplicantsSharedUser,
        [StringValue("AAAB")]
        AgencyUser,
        [StringValue("AAAC")]
        Instructor,
        [StringValue("AAAD")]
        Preceptor
    }

    ///<summary>
    ///[lkpUserTypeSwitchView]
    ///</summary>
    public enum UserTypeSwitchView
    {
        [StringValue("AAAA")]
        ADBAdmin,
        [StringValue("AAAB")]
        ClientAdmin,
        [StringValue("AAAC")]
        Applicant,
        [StringValue("AAAD")]
        SharedUser,
        [StringValue("AAAE")]
        InstructorOrPreceptor,
        [StringValue("AAAF")]
        AgencyUser
    }

    #endregion

    public enum ObjectAttribute
    {
        [StringValue("AAAA")]
        REQUIRED,
        [StringValue("AAAB")]
        SIGNATURE_REQUIRED,
        [StringValue("AAAC")]
        REQUIRED_TO_VIEW,
        [StringValue("AAAD")]
        REQUIRED_TO_OPEN,
        [StringValue("AAAE")]
        BOX_STAYS_OPEN_TIME
    }

    #region UAT-1316
    public enum RequirementCategoryStatus
    {
        [StringValue("AAAA")]
        INCOMPLETE,
        [StringValue("AAAB")]
        PENDING_REVIEW,
        [StringValue("AAAC")]
        APPROVED,
    }

    public enum RequirementItemStatus
    {
        [StringValue("AAAA")]
        INCOMPLETE,
        [StringValue("AAAB")]
        PENDING_REVIEW,
        [StringValue("AAAC")]
        APPROVED,
        [StringValue("AAAD")]
        NOT_APPROVED,
        [StringValue("AAAE")]
        EXPIRED,
        //TODO IN DB 
        [StringValue("AAAF")]
        SUBMITTED,
    }

    #endregion

    public enum RequirementPackageStatus
    {
        [StringValue("AAAA")]
        REQUIREMENT_COMPLIANT,
        [StringValue("AAAB")]
        REQUIREMENT_NOT_COMPLIANT
    }

    public enum RequirementSubscriptionType
    {
        [StringValue("AAAA")]
        ROTATION_SUBSCRIPTION
    }

    #region UAT-1310
    public enum SystemPackageTypes
    {
        [StringValue("AAAA")]
        COMPLIANCE_PKG,
        [StringValue("AAAB")]
        BACKGROUND_PKG,
        [StringValue("AAAC")]
        REQUIREMENT_ROT_PKG
    }
    #endregion

    /// <summary>
    /// Enum for type of Document to be downloaded through DocumentViewer.aspx
    /// </summary>
    public enum DocumentViewerDocType
    {
        /// <summary>
        /// For viewwing PDF format document (both UPLOAD and VIEW type) on Verification Detail
        /// </summary>
        [StringValue("AAAA")]
        ROTATION_DOCUMENT_PDF,
    }

    /// <summary>
    /// Enum for 'lkpAgencySearchStatus' in SharedData
    /// </summary>
    public enum AgencySearchStausTypes
    {
        [StringValue("AAAA")]
        REVIEWED,
        [StringValue("AAAB")]
        NOT_REVIEWED,
        [StringValue("AAAC")]
        AVAILABLE
    }

    /// <summary>
    /// Enum for 'lkpSharedUserReviewStatus' in SharedData
    /// </summary>
    public enum SharedUserReviewStatus
    {
        [StringValue("AAAA")]
        NEW,
        [StringValue("AAAB")]
        REVIEWED
    }

    /// <summary>
    /// Enum for 'lkpProfileSynchSourceType' in SharedData
    /// </summary>
    public enum ProfileSynchSourceType
    {
        [StringValue("AAAA")]
        ADD_TO_CLINICAL_ROTATION,
        [StringValue("AAAB")]
        CLIENTCONTACT_REGISTRATION
    }
    public struct SharedUserRoleDetails
    {
        public const String AgencyUserRole = "9238FAC5-C6FD-4D3D-94BB-CEFD671C5B05";
        public const String RotationPackageRole = "1D34ED3C-073A-4617-9919-D3FF7DC97E9B";
        public const String AgencyUserAndRotationPackageRole = "1854D799-0420-452E-85DB-3DE119070A26";
    }

    public enum RotationStatus
    {
        [StringValue("AAAA")]
        Active,
        [StringValue("AAAB")]
        Completed
    }

    /// <summary>
    /// Enum for 'lkpContractExpirationType' table
    /// </summary>
    public enum ContractExpirationType
    {
        [StringValue("AAAB")]
        BASED_ON_END_DATE,
        [StringValue("AAAC")]
        TERM,
        [StringValue("AAAD")]
        EXPIRATION_DATE,
    }

    public enum ReportType
    {
        [StringValue("RSD02")]
        ROTATION_STUDENT_DETAILS_WITH_USERID,
        [StringValue("ISR01")]
        IMMUNIZATION_SUMMARY_REPORT_PASSPORT,
        [StringValue("CCR01")]
        CATEGORY_COMPLIANCE_REPORT,
        [StringValue("CCR02")]
        PENDING_CATEGORY_COMPLIANCE_REPORT,
        [StringValue("CCR03")]
        SALES_FIGURE_REPORT,
        [StringValue("CCR04")]
        PENDING_REVIEWSUBSCRIPTIONSREPORT,
        [StringValue("CCR05")]
        ORDER_STATUS_REPORT,
        [StringValue("CCR06")]
        SUBSCRIPTION_SETUP_REPORT,
        [StringValue("CCR07")]
        ACCOUNTS_WITHOUT_ORDER_REPORT,
        [StringValue("CDR01")]
        COMPLIANCE_DETAIL,
        [StringValue("CDH01")]
        COMPLIANCE_DETAIL_HORIZONTAL,
        [StringValue("CCRH")]
        CATEGORY_COMPLIANCE_REPORT_HORIZONTAL,
        [StringValue("MSFR")]
        MANUAL_SERVICE_FORM_REPORT,
        [StringValue("PRM01")]
        PASSPORT_REPORT,
        [StringValue("UCER")]
        Upcoming_Expiration_Report,
        [StringValue("DAA01")]
        DA_DOCUMENT_AND_PACKAGE_ASSOCIATION,
        [StringValue("IDR01")]
        INSTITUTION_DETAIL_REPORT,
        [StringValue("AN01")]
        ADMIN_NOTIFICATION,
        [StringValue("BSS01")]
        BACKGROUND_SUBSCRIPTION_SETUP,
        [StringValue("CAL01")]
        CLIENT_ADMIN_LIST,
        [StringValue("BOSC")]
        BKG_ORDER_STATUS_COLOR,
        [StringValue("BOCSG")]
        COMPLETED_SERVICE_GROUP,
        [StringValue("TPRVT")]
        PRODUCTIVITY,
        [StringValue("BSFRM")]
        BKG_SERVICE_FORMS,
        [StringValue("PRDO")]
        PASSPORT_REPORT_DATA_ONLY,
        [StringValue("DULR")]
        DOCUMENTS_UPLOADED,
        [StringValue("CCSR")]
        CLIENT_COMPLIANCE_STATUS,
        [StringValue("NCSL")]
        NON_COMPLIANT_STUDENT_LIST,
        [StringValue("ATR01")]
        ATTESTATION,
        [StringValue("DEP01")]
        DATAENTRY_PRODUCTIVITY,
        [StringValue("IOR01")]
        INVOICE_ORDER,
        [StringValue("UAR01")]
        UPCOMING_ARCHIVAL,
        [StringValue("HRS01")]
        HIERARCHY_SETUP,
        [StringValue("RPS01")]
        REQUIREMENT_PACKAGE_SETUP,
        [StringValue("AMSMBSORD")]
        AMSMBS_Order_Report,
        [StringValue("AURS01")]
        AGENCY_USER_ROTATION_STATUS,
        [StringValue("INCEDSREP")]
        INCOMPLETE_EDS_ORDERS_REPORT,
        [StringValue("INCEDSREUI")]
        INCOMPLETE_EDS_ORDERS_REPORTUI,
        [StringValue("AWP01")]
        ACCOUNT_WITHOUT_PURCHASE_REPORT,
        //UAT-3052
        [StringValue("AGUSERREP")]
        INSTITUTION_COUNT,
        [StringValue("REITDAREP")]
        REQUIREMENT_ITEM_DATA_REPORT,
        [StringValue("RSD01")]
        ROTATION_STUDENT_DETAILS,
        [StringValue("AGADPT")]
        AGENCY_ADMINS_BY_DEPARTMENT,
        [StringValue("IDCR")]
        ITEM_DATA_COUNT_REPORT,
        [StringValue("RSNCS")]
        ROTATION_STUDENTS_OVERALL_NONCOMPLIANCE_STATUS,
        [StringValue("RSCS")]
        ROTATION_STUDENTS_COMPLIANCE_STATUS,
        [StringValue("CDR02")]
        CATEGORY_DATA_REPORT,
        [StringValue("RSDOW")]
        ROTATION_STUDENTS_BY_DAY_OF_THE_WEEK,
        [StringValue("PRMAI01")]
        PASSPORT_APPROVED_ITEMS_REPORT,
        [StringValue("CDRBCI")]
        CATEGORY_DATA_REPORT_WITH_COMPLIO_ID
    }

    public enum ControlType
    {
        [StringValue("AAAA")]
        TEXT,
        [StringValue("AAAB")]
        NUMERIC,
        [StringValue("AAAC")]
        HIERARCHY,
        [StringValue("AAAD")]
        OPTION,
        [StringValue("AAAE")]
        MULTIPLE_OPTIONS,
        [StringValue("AAAF")]
        DATE,
        [StringValue("AAAG")]
        EMPTY,
        [StringValue("AAAH")] //UAT-3052
        RADIO_BUTTON,

    }

    /// <summary>
    /// Enum to identify the Type of use of VerificationDetailsDocumnetControl.ascx
    /// </summary>
    public enum DocumentControlType
    {
        [StringValue("AAAA")]
        SCREENING_DOCUMENT,
    }


    public enum ReportParameters
    {
        [StringValue("TenantID")]
        TENANT_ID,
        [StringValue("UserID")]
        USER_ID,
        [StringValue("Institute")]
        INSTITUTE,
        [StringValue("Hierarchy")]
        HIERARCHY,
        [StringValue("OverallStatus")]
        OVERALL_STATUS,
        [StringValue("Category")]
        CATEGORY,
        [StringValue("CategoryStatus")]
        CATEGORY_STATUS,
        [StringValue("UserGroup")]
        USERGROUP,
        [StringValue("Node")]
        NODE,
        [StringValue("FromDate")]
        FROM_DATE,
        [StringValue("ToDate")]
        TO_DATE,
        [StringValue("Item")]
        ITEM,
        [StringValue("SubscriptionArchiveState")]
        SUBSCRIPTION_ARCHIVE_STATE,
        [StringValue("OrderType")]
        ORDER_TYPE,
        [StringValue("OrderFromDate")]
        ORDER_FROMDATE,
        [StringValue("OrderToDate")]
        ORDER_TODATE,

        #region UAT-3052
        [StringValue("LoggedInUserEmailId")]
        LOGGEDIN_USER_EMAILID,
        [StringValue("IncludeUndefinedDateShares")]
        INCLUDE_UNDEFINED_DATESHARES,
        [StringValue("Agency")]
        AGENCY,
        [StringValue("WeekDays")]  //UAT 3214
        WEEK_DAYS,
        #endregion
        [StringValue("UserType")]  
        USER_TYPE,
        [StringValue("ReviewStatus")] 
        REVIEW_STATUS,

    }

    /// <summary>
    /// Enum for Tenant [lkpSharedUserRotationReviewStatus] Entity
    /// </summary>
    public enum SharedUserRotationReviewStatus
    {
        [StringValue("AAAA")]
        PENDING_REVIEW,
        //UAT-1844:
        [StringValue("AAAB")]
        APPROVED,
        [StringValue("AAAC")]
        NOT_APPROVED,
        //UAT-4460
        [StringValue("AAAD")]
        Dropped,
    }

    public enum SharedUserGridSource
    {
        [StringValue("RotationDetails")]
        ROTATION_DETAILS_TAB,
        [StringValue("RotationWidget")]
        ROTATION_WIDGET_TAB,
        [StringValue("InvitationDetails")]
        INVITATION_DETAILS_WIDGET_TAB,
        [StringValue("ClinicalRotations")]
        CLINICAL_ROTATION,
        [StringValue("RequirementShares")]
        REQUIREMENT_SHARES,
        [StringValue("RotationRequirement")]
        ROTATION_REQUIREMENT,
        //UAT-2469
        [StringValue("StudentRotationSearch")]
        STUDENT_ROTATION_SEARCH,
        [StringValue("AGENCYAPPLICANTSTATUS")]
        AGENCY_APPLICANT_STATUS,
        //UAT-4013
        [StringValue("INSTRCTR_PRECEPTR_ROTATION_SEARCH_URL")]
        INSTRCTR_PRECEPTR_ROTATION_SEARCH_URL
    }

    #region UAT-1578
    public enum NotificationDeliveryType
    {
        [StringValue("AAAA")]
        EMAIL,
        [StringValue("AAAB")]
        SMS,

    }
    public enum SMSSubscriptionStatus
    {
        [StringValue("PendingConfirmation")]
        PENDING_CONFIRMATION,
        [StringValue("Confirmed")]
        CONFIRMED,
        [StringValue("Not Confirmed")]
        NOT_CONFIRMED,
    }
    #endregion


    public struct ShotSeriesTreeNodeType
    {
        public const String Category = "CAT";
        public const String Series = "SER";
        public const String Item = "ITM";
        public const String RuleLabel = "LRULE";
        public const String Rule = "RULE";
    }

    /// <summary>
    /// Enum for 'lkpitemstatuspostdatashuffle' table
    /// </summary>
    public enum ItemStatusPostDataShuffle
    {
        [StringValue("AAAA")]
        RETAIN,
        [StringValue("AAAB")]
        PENDING_REVIEW,
    }

    #region Shot Series UAT-1607
    public struct ShotSeriesHandleCalledFrom
    {
        public const String StudentDataEntry = "SE";
        public const String AdminDataEntry = "AE";
        public const String VerificationDetailScreen = "VD";
    }
    #endregion

    /// <summary>
    /// UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
    /// </summary>
    public enum AgencyUserPermissionType
    {
        [StringValue("AAAA")]
        ATTESTATION_REPORT_TEXT_PERMISSION,
        [StringValue("AAAB")]
        GRANULAR_SSN_PERMISSION,
        [StringValue("AAAC")]
        AGENCY_PORTAL_DETAIL_LINK_PERMISSION,
        //UAT-3316
        [StringValue("AAAD")]
        AGENCY_APPLICANT_STATUS_PERMISSION,
        [StringValue("AAAE")]
        ROTATION_PACKAGE_PERMISSION,
        [StringValue("AAAF")]
        AGENCY_USER_PERMISSION,
        [StringValue("AAAG")]
        ROTATION_PACKAGE_VIEW_PERMISSION,
        [StringValue("AAAH")]
        ALLOW_JOB_POSTING_PERMISSION,
        [StringValue("AAAI")]
        DONOT_SHOW_NON_AGENCY_SHARES_PERMISSION,
        [StringValue("AAAJ")]
        REPORTS_PERMISSION
    }

    /// <summary>
    /// UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
    /// </summary>
    public enum AgencyUserPermissionAccessType
    {
        [StringValue("AAAA")]
        YES,
        [StringValue("AAAB")]
        NO
    }

    /// <summary>
    /// UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
    /// </summary>
    public enum AgencyPermissionType
    {
        [StringValue("AAAA")]
        COMPLIANCE_REQD_ROTATION_PRMSN,
        [StringValue("AAAB")]
        COMPLIANCE_REQD_TRACKING_PRMSN,
        [StringValue("AAAC")]
        ONLY_ROTATION_PACKAGE_SHARE_PRMSN,
        //UAT-2554
        [StringValue("AAAD")]
        PRECEPTOR_REQD_ROTATION_PRMSN,
        //UAT-3977
        [StringValue("AAAE")]
        COMPLIANCE_REQD_INSTRUC_PRECEP_ROTATION_PKG_PRMSN,
        [StringValue("AAAF")]
        SHOW_ROTATION_TYPE_SPECIALTY_OPTIONS
    }

    /// <summary>
    /// UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
    /// </summary>
    public enum AgencyPermissionAccessType
    {
        [StringValue("AAAA")]
        YES,
        [StringValue("AAAB")]
        NO
    }

    public enum LKPDurationOptions
    {
        [StringValue("AAAA")]
        LAST_WEEK,
        [StringValue("AAAB")]
        LAST_MONTH,
        [StringValue("AAAC")]
        LAST_YEAR,
        [StringValue("AAAD")]
        DATE_RANGE,
    }


    public enum SharedUserInvitationReviewStatus
    {
        [StringValue("AAAA")]
        PENDING_REVIEW,
        //UAT-1844:Phase 2 8: Agency User> Rotation tab and Other Tab
        [StringValue("AAAB")]
        APPROVED,
        [StringValue("AAAC")]
        NOT_APPROVED,
        //UAT-4460
        [StringValue("AAAD")]
        Dropped,
    }
    //UAT-1683: Add the Archive button and Manage Un-Archive to the Screening side.
    public enum ArchivePackageType
    {
        [StringValue("AAAA")]
        Tracking,
        [StringValue("AAAB")]
        Screening,
    }

    /// <summary>
    /// Enum for Different Types of NLog Loggers implemented
    /// </summary>
    public enum NLogLoggerTypes
    {
        [StringValue("OrderFlowLogger")]
        ORDER_FLOW_LOGGER
    }

    /// <summary>
    /// Enum for Different Types of NLog Loggers implemented for  PDF Document VIew
    /// </summary>
    public enum NLogLoggerPDFView
    {
        [StringValue("PDFViewer")]
        PDF_Viewer_LOGGER
    }

    public enum StatusMessages
    {
        [StringValue("ErrorMessage")]
        ERROR_MESSAGE,
        [StringValue("InfoMessage")]
        INFO_MESSAGE,
        [StringValue("SuccessMessage")]
        SUCCESS_MESSAGE
    }

    public struct RequirementTreeNodeType
    {
        public const String PACKAGELABEL = "LPKG";
        public const String PACKAGE = "PKG";
        public const String CATEGORYLABEL = "LCAT";
        public const String CATEGORY = "CAT";
        public const String ITEMLABEL = "LITM";
        public const String ITEM = "ITM";
        public const String FIELDLABEL = "LFLD";
        public const String FIELD = "FLD";
        public const String RULELABEL = "LRULE";
        public const String RULE = "RULE";
    }

    public enum ItemSubmissionReconStatus
    {
        [StringValue("AAAA")]
        MULTI_REVIEW_NOT_REQUIRED,
        [StringValue("AAAB")]
        SELECTED_FOR_MULTIREVIEW,
        [StringValue("AAAC")]
        REVIEW_STARTED,
        [StringValue("AAAD")]
        REVIEWS_MATCHED,
        [StringValue("AAAE")]
        NEED_RECONCILIATION,
        [StringValue("AAAF")]
        COMPLETED_RECONCILIATION,
        [StringValue("AAAG")]
        NOT_AssesedMultireview,
        [StringValue("AAAI")]
        REVIEW_OVERRIDDEN_CLNTADMN,
    }

    /// <summary>
    /// Enum for lkpBulkOrderStatus
    /// </summary>
    public enum BulkOrderStatus
    {
        [StringValue("AAAA")]
        OrderAccepted,
        [StringValue("AAAB")]
        OrderPlaced,
        [StringValue("AAAC")]
        OrderFailed
    }

    /// <summary>
    /// UAT-2697
    /// Enum for lkporderdatasource
    /// </summary>
    public enum Orderdatasource
    {
        [StringValue("AAAA")]
        Manual,
        [StringValue("AAAB")]
        Previous
    }

    #region UAT-2034
    public enum RotationAssignmentType
    {
        [StringValue("AA")]
        ASSIGN_PRECEPTOR_PACKAGES,
        [StringValue("AB")]
        ASSIGN_STUDENT_PACKAGES,
        [StringValue("AC")]
        UPLOAD_SYLLABUS,
        [StringValue("AD")]
        ASSIGN_PRECEPTOR,
    }
    #endregion


    public enum ReshduleOrderType
    {
        Invalid,
        Valid,
        ValidWithmailing,
    }

    public enum CabsReshduleStatusType
    {
        InProgress,
        Completed,
        HideReshdule,
        ShowReshdule,
    }



    public enum ItemDocMappingType
    {
        [StringValue("AAAA")]
        ITEM_DATA,
        [StringValue("AAAB")]
        CATEGORY_EXCEPTION
    }

    public enum PaymentApproval
    {
        [StringValue("AAAA")]
        APPROVAL_REQUIRED_BEFORE_PAYMENT,
        [StringValue("AAAB")]
        APPROVAL_NOT_REQUIRED_BEFORE_PAYMENT,
        [StringValue("AAAC")]
        NOT_SPECIFIED
    }

    public enum ReqScheduleAction
    {
        [StringValue("AAAA")]
        EXECUTE_PACKAGE_RULES,
        [StringValue("AAAB")]
        EXECUTE_CATEGORY_RULES,
        [StringValue("AAAC")]
        EXECUTE_RULES_FOR_SYNC,
        [StringValue("AAAD")]
        EXECUTE_SCHEDULED_CATEGORY_RULES
    }

    public enum PkgNotesDisplayPosition
    {
        [StringValue("AAAA")]
        DISPLAY_ABOVE,
        [StringValue("AAAB")]
        DISPLAY_BELOW
    }

    /// <summary>
    /// UAT-2264: Grid Customizatiom
    /// code is saved in screen table w.r.t. its grid name.
    /// </summary>
    public enum Screen
    {
        [StringValue("AAAA")]
        grdRotations,
        //UAT-2360
        [StringValue("AAAB")]
        grdRequirementShares,
        //UAT-2696
        [StringValue("AAAC")]
        grdAdminManageRotations,
        //UAT-2940
        [StringValue("AAAD")]
        grdRequirementSharesStudentDetail,
        [StringValue("AAAE")]
        grdRotationStudentsStudentDetail,
        [StringValue("AAAF")]
        grdClinicalRotationsStudentDetail,
        [StringValue("AAAG")]
        grdRotationRequirementsStudentDetail,
        //UAT-2675
        [StringValue("AAAI")]
        grdManageComplianceSearch,
        //3717
        [StringValue("AAAJ")]
        grdPlacementMatchingMapping,
        [StringValue("AAAK")]
        grdSearchOpportunities,
        //UAT-3952
        [StringValue("AAAL")]
        trlInstituteHierarchy,
        [StringValue("AAAM")]
        trlAgencyHierarchy,
        //UAT-4013
        [StringValue("AAAN")]
        grdRotationStudentSearch,
    }

    //Enum for ams.lkpSupplementAutomationStatus
    public enum SupplementAutomationStatus
    {
        [StringValue("AAAA")]
        PENDING_REVIEW,
        [StringValue("AAAB")]
        REVIEWED
    }

    //UAT-2304:
    public enum SvcGrpReviewType
    {
        [StringValue("AA")]
        ALL,
        [StringValue("AB")]
        AUTOMATIC_REVIEWED
    }

    #region UAT-2305:
    public enum UniversalMappingTypeEnum
    {
        [StringValue("AAAA")]
        COMPLIANCE_TYPE,
        [StringValue("AAAB")]
        REQUIREMENT_TYPE
    }

    public enum UniversalAttributeDataTypeEnum
    {
        [StringValue("AAAA")]
        DATE,
        [StringValue("AAAB")]
        OPTIONS,
        [StringValue("AAAC")]
        UPLOAD_DOCUMENT,
        [StringValue("AAAD")]
        VIEW_DOCUMENT,
        [StringValue("AAAE")]
        TEXT
    }
    #endregion

    #region UAT-2514
    public enum RequirementPackageObjectActionTypeEnum
    {
        [StringValue("AAAA")]
        ADDED,
        [StringValue("AAAB")]
        REMOVED,
        [StringValue("AAAC")]
        REPLACED,
        [StringValue("AAAD")]
        EDITED,
        [StringValue("AAAE")]
        RULECHANGED,
        [StringValue("AAAF")]
        PACKAGE_ARCHIVED,
        [StringValue("AAAG")]
        CATEGORY_REMOVE_FROM_PACKAGE
        ,
        [StringValue("AAAH")]
        PACKAGE_UNARCHIVED,
        [StringValue("AAAI")]
        DisplayOrderChangeCategoryFromPackage

    }

    public enum RequirementPackageObjectTypeEnum
    {
        [StringValue("AAAA")]
        REQUIREMENT_CATEGORY,
        [StringValue("AAAB")]
        REQUIREMENT_ITEM,
        [StringValue("AAAC")]
        REQUIREMENT_FIELD,
        [StringValue("AAAD")]
        REQUIREMENT_RULE,
        [StringValue("AAAE")]
        REQUIREMENT_PACKAGE,
        [StringValue("AAAF")]
        REQUIREMENT_ATTRIBUTE_GROUP,
        [StringValue("AAAG")]
        REQUIREMENT_ITEM_URL,
        [StringValue("AAAH")]
        REQUIREMENT_CATEGORY_DOC_URL,
    }
    #endregion

    #region UAT-2490
    public enum ApplicantComplianceItemDataDeletedfrom
    {
        [StringValue("AAAA")]
        STUDENT_DATA_ENTRY_SCREEN,
        [StringValue("AAAB")]
        THREE_PANEL_SCREEN,
        [StringValue("AAAC")]
        SHOT_SERIES_SCREEN,
    }
    #endregion

    public enum SecurityTokenType
    {
        [StringValue("DFLT")]
        DefaultServerToken,
        [StringValue("LGDT")]
        LoginDataToken,
        [StringValue("DLTK")]
        DocListToken
    }

    #region UAT-2511
    public enum AuditChangeType
    {
        [StringValue("AAAA")]
        INVITATION_STATUS,
        [StringValue("AAAB")]
        REJECTION_NOTES,
        [StringValue("AAAC")]
        INVITATION_NOTES,
        [StringValue("AAAD")]
        ROTATION_STATUS,
        [StringValue("AAAE")]
        ROTATION_REJECTION_NOTES,
        [StringValue("AAAF")]
        REQUEST_FOR_AUDIT_INVITATION
    }

    public enum AuditType
    {
        [StringValue("AAAA")]
        INVITATION,
        [StringValue("AAAB")]
        ROTATION,
        [StringValue("AAAC")]
        INVITATION_DETAIL,
        [StringValue("AAAD")]
        REQUEST_FOR_AUDIT_INVITATION
    }
    #endregion

    #region UAT-2427
    public enum AgencyJobType
    {
        [StringValue("AAAA")]
        Internship,
        [StringValue("AAAB")]
        Employement
    }

    public enum AgencyJobStatus
    {
        [StringValue("AAAA")]
        Draft,
        [StringValue("AAAB")]
        Published,
        [StringValue("AAAC")]
        Archived,
        [StringValue("AAAD")]
        DraftAndArchived
    }
    #endregion

    #region UAT-2625
    public enum DisclosureDocumentAgeGroup
    {
        [StringValue("AAAA")]
        EIGHTEEN_AND_ABOVE,
        [StringValue("AAAB")]
        SEVENTEEN_OR_UNDER,
        [StringValue("AAAC")]
        ALL_AGES,
    }
    #endregion


    #region UAT-2784
    /// <summary>
    /// UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
    /// </summary>
    public enum AgencyHierarchySettingType
    {
        [StringValue("AAAA")]
        EXPIRATION_CRITERIA,
        [StringValue("AAAB")]
        SPECIFIC_ATTESTATION_FORM,
        [StringValue("AAAC")]
        AUTOMATICALLY_ARCHIVED_ROTATION,
        [StringValue("AAAD")]
        INSTPRECEPTOR_MANDATORY_FOR_INDIVIDUAL_SHARE,
        [StringValue("AAAE")]
        IS_NODE_AVAILABLE_FOR_ROTATION, //UAT-4443
        [StringValue("AAAF")]
        UPDATE_REVIEW_STATUS, //UAT-4673
    }
    #endregion

    #region UAT-2842
    public enum AdminOrderStatus
    {
        [StringValue("AAAA")]
        NEW,
        [StringValue("AAAB")]
        DRAFT,
        [StringValue("AAAC")]
        READY_FOR_TRANSMIT,
        [StringValue("AAAD")]
        TRANSMITTED
    }
    #endregion


    #region UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
    public enum AgencyUserNotificationLookup
    {
        [StringValue("AAAA")]
        REQUIREMENTS_SHARING_INVITATION_NON_ROTATION,
        [StringValue("AAAB")]
        REQUIREMENTS_SHARING_INVITATION_ROTATION_SHARING,
        [StringValue("AAAC")]
        NOTIFICATION_FOR_ROTATION_INVITATION_APPROVAL_REJECTION,
        [StringValue("AAAD")]
        NOTIFICATION_FOR_INDIVIDUAL_PROFILE_SHARING_WITH_EMAIL,
        [StringValue("AAAE")]
        NOTIFICATION_FOR_PROFILE_SHARING_WITH_AGENCY_APPROVED,
        [StringValue("AAAF")]
        NOTIFICATION_UPON_STUDENT_FALL_OUT_OF_COMPLIANCE,
        [StringValue("AAAG")]
        NOTIFICATION_FOR_UPDATED_APPLICANT_REQUIREMENTS,
        [StringValue("AAAH")]
        NOTIFICATION_FOR_UPDATED_ROTATION_DETAILS,//UAT-3108
        [StringValue("AAAI")]
        NOTIFICATION_FOR_STUDENT_DROPPED_FROM_ROTATION
        ,
        [StringValue("AAAJ")]
        NOTIFICATION_FOR_IT_SYSTEM_ACCESS_FORM//UAT-3998
        ,
        [StringValue("AAAK")]
        NOTIFICATION_FOR_ROTATION_END_DATE_CHANGE, //UAT-4561
    }

    #endregion

    #region UAT-2930
    public enum GoogleAuthenticationStatus
    {
        [StringValue("NAPP")]
        NotApplicable,
        [StringValue("AUTH")]
        Authenticated,
        [StringValue("NAUTHGA")]
        NotAuthenticated_With_GoogleAuthenticator,
        [StringValue("NAUTHTM")]
        NotAuthenticated_With_TextMessage,
    }
    #endregion

    #region UAT-3068

    public enum AuthenticationMode
    {
        [StringValue("NONE")]
        None,
        [StringValue("AAAA")]
        Google_Authenticator,
        [StringValue("AAAB")]
        Text_Message
    }
    public enum lkpAuthenticationUseTypes
    {
        [StringValue("AAAA")]
        Application_Login
    }

    #endregion

    #region UAT-3083: Item Payment
    public enum ItemDataType
    {
        [StringValue("AAAA")]
        Applicant_Compliance_Item_Data = 1,
        [StringValue("AAAB")]
        Applicant_Requirement_Item_Data = 2
    }
    #endregion

    #region UAT-3112
    public enum BagdeFormFieldType
    {
        [StringValue("AAAA")]
        FullName,
        [StringValue("AAAB")]
        DOB,
        [StringValue("AAAC")]
        PhoneNumber,
        [StringValue("AAAD")]
        FullAddress,
        [StringValue("AAAE")]
        RotationName,
        [StringValue("AAAF")]
        RotationStartDate,
        [StringValue("AAAG")]
        RotationEndDate,
        [StringValue("AAAH")]
        Program,
        [StringValue("AAAI")]
        Location,
        [StringValue("AAAJ")]
        ItemSubmisssionDate,
        [StringValue("AAAK")]
        ItemApprovalDate,
        [StringValue("AAAL")]
        Signature,
        [StringValue("AAAM")]
        Universal
    }
    public enum PackageType
    {
        [StringValue("AAAA")]
        Compliance_Package = 1,
        [StringValue("AAAB")]
        Requirement_Package = 2
    }
    #endregion

    #region UAT-2960

    public enum lkpAlumniStatus
    {
        [StringValue("AAAA")]
        Due,
        [StringValue("AAAB")]
        Dismissed,
        [StringValue("AAAC")]
        Initiated,
        [StringValue("AAAD")]
        Activated
    }

    public enum ComplianceDataMovementStatus
    {
        [StringValue("AAAA")]
        Data_Movement_Due,
        [StringValue("AAAB")]
        Data_Movement_Not_Required,
        [StringValue("AAAC")]
        Data_Movement_Completed,
        [StringValue("AAAD")]
        Data_Movement_Failed
    }

    public enum AlumniSettings
    {
        [StringValue("AAAA")]
        AlumniTenantID,
        [StringValue("AAAB")]
        DefaultAlumniPackageID
    }
    public enum lkpEmailNotificationStatus
    {
        [StringValue("AAAA")]
        Pending,
        [StringValue("AAAB")]
        Sent,
        [StringValue("AAAC")]
        Failed
    }
    #endregion

    #region UAT 3071
    public enum ThirdPartyUploadOutputType
    {
        [StringValue("AAAA")]
        Error = 1,
        [StringValue("AAAB")]
        Success = 2,
        [StringValue("AAAC")]
        IgnoreError = 3
    }
    #endregion
    #region UAT 3268

    public enum ExemptedHierarchyNodeValueType
    {
        [StringValue("AAAA")]
        Yes,
        [StringValue("AAAB")]
        No,
        [StringValue("AAAC")]
        Default,
    }
    #endregion

    #region UAT 3331
    public enum BkgExternalVendorResultResponseFormat
    {
        [StringValue("AAAA")]
        ClearStarHtml
    }
    #endregion

    #region UAT 3313
    public enum PackageBundlePackageType
    {
        [StringValue("COMPKGBND")]
        COMPLIANCEPACKAGEBUNDLE,
        [StringValue("SCRNPKGBND")]
        SCREENINGPACKAGEBUNDLE
    }
    #endregion

    #region Login Page Enum

    public enum LoginPageToOpen
    {
        [StringValue("CommonLogin.ascx")]
        CommonLogin,
        [StringValue("ConfigurableLogin.ascx")]
        ConfigurableLogin,
    }

    #endregion

    #region UAT-3669

    public enum lkpBlockedOrderReason
    {
        [StringValue("AAAA")]
        WebCCF_Didnot_Supplied_Registration_Id
    }

    #endregion

    #region UAT-3734

    public enum FingerPrintAppointmentStatus
    {
        [StringValue("AAAA")]
        ACTIVE,
        [StringValue("AAAB")]
        CANCELLED,
        [StringValue("AAAC")]
        MISSED,
        [StringValue("AAAD")]
        REVOKED,
        [StringValue("AAAE")]
        REVOKED_AND_NOTIFIED,
        [StringValue("AAAF")]
        MISSED_AND_NOTIFIED,
        [StringValue("AAAG")]
        COMPLETED,
        [StringValue("AAAH")]
        SUBMITTED_TO_CBI,
        [StringValue("AAAI")]
        FINGERPRINTS_COMPLETED_SUCCESSFULLY,
        [StringValue("AAAJ")]
        FINGERPRINT_FILE_REJECTED,
        [StringValue("AAAK")]
        FINGERPRINT_FILE_ERROR,
        [StringValue("AAAL")]
        TECHNICAL_REVIEW,
        [StringValue("AAAM")]
        CONTACT_AGENCY_EMPLOYER,
        [StringValue("AAAN")]
        CBI_FINGERPRINT_FILE_REJECTED,
        [StringValue("AAAO")]
        MANUALLY_REJECTED_ORDER,
        [StringValue("AAAP")]
        PENDING_ABI_REVIEW,
        [StringValue("AAAQ")]
        REJECTED_BY_ABI,
        [StringValue("AAAN")]
        REJECTED_BY_CBI,
        [StringValue("AAAJ")]
        REJECTED_BY_FBI


    }
    public enum FingerPrintRevokeType
    {
        [StringValue("AAAA")]
        CLOSURE,
        [StringValue("AAAB")]
        OFF_TIME
    }

    public enum PersonAliasPageType
    {
        [StringValue("AAAA")]
        ITSUserRegistration,
        [StringValue("AAAB")]
        ApplicantProfile,
        [StringValue("AAAC")]
        OrderReview,
        [StringValue("AAAD")]
        EditProfile,
        [StringValue("AAAE")]
        OrderPaymentDetails,
    }

    public enum FingerPrintingSite
    {
        [StringValue("Out Of State")]
        OUT_OF_STATE,
        [StringValue("Onsite")]
        ONSITE
    }
    public enum CABSServiceStatus
    {
        [StringValue("AAAA")]
        PENDING_SHIPMENT,
        [StringValue("AAAB")]
        SHIPPED,
        [StringValue("AAAC")]
        RETURNED_TO_SENDER,
        [StringValue("AAAD")]
        REJECTSERVICE,
        [StringValue("AAAE")]
        NEW

    }

    //Added for checkbox visibility on FingerPrinting Order GridViews
    public enum OrderStatusText
    {
        [StringValue("New")]
        NEW,
        [StringValue("Payment Pending")]
        PAYMENT_PENDING,
    }

    public enum CabsMailingOption
    {
        [StringValue("AAAA")]
        FIRST_CLASS_MAIL,
        [StringValue("AAAB")]
        OVERNIGHT_SHIPPING,
        [StringValue("AAAC")]
        TWO_DAY_SHIPPING
    }


    #endregion

    #region UAT-3969
    public enum ConditionType
    {
        [StringValue("AAAA")]
        LEGAL_NAME_CHANGE
    }
    #endregion

    #region Placement Matching

    public enum InstitutionAvailabilityType
    {
        [StringValue("AAAA")]
        AssociatedInstitution,
        [StringValue("AAAB")]
        AllInstitutions,
    }
    public enum InventoryStatus
    {
        [StringValue("AAAA")]
        Draft,
        [StringValue("AAAB")]
        Published,
        [StringValue("AAAC")]
        Open,
        [StringValue("AAAD")]
        Closed,
        [StringValue("AAAE")]
        Removed
    }

    public enum InventoryRecordType
    {
        [StringValue("AAAA")]
        PrimaryInventory
    }

    public enum RequestDetails
    {
        [StringValue("CREATEDRAFT")]
        CREATEDRAFT,
        [StringValue("REQUESTDETAILS")]
        REQUESTDETAILS
    }

    public enum RequestStatusCodes
    {
        [StringValue("AAAA")]
        Requested,
        [StringValue("AAAB")]
        Modified,
        [StringValue("AAAC")]
        Approved,
        [StringValue("AAAD")]
        Rejected,
        [StringValue("AAAE")]
        Archived,
        [StringValue("AAAF")]
        Cancelled,
        [StringValue("AAAG")]
        Draft,
        [StringValue("AAAH")]
        Conflicts,
    }

    public enum SharedCustomAttributeUseType
    {
        [StringValue("AAAA")]
        ClinicalInventory,
        [StringValue("AAAB")]
        ClinicalInventoryRequest,
        [StringValue("AAAC")]
        ClinicalInventoryAndRequest
    }

    public enum CustomAttributeValueRecordType
    {
        [StringValue("AAAA")]
        ClinicalInventory,
        [StringValue("AAAB")]
        ClinicalInventoryRequest
    }

    #endregion

    #region UAT-3761
    public enum LocationAppointmentAuditChangeType
    {
        [StringValue("AAAA")]
        LOCATION_CHANGE,
        [StringValue("AAAB")]
        SCHEDULE_CHANGE,
        [StringValue("AAAC")]
        STATUS_CHANGE,
        [StringValue("AAAD")]
        REFUND_UPDATE,
        [StringValue("AAAE")]
        MODE_CHANGE,
        [StringValue("AAAF")]
        PAYMENT_TYPE_UPDATE,
        [StringValue("AAAG")]
        ORDER_CREATED,        
        [StringValue("AAAK")]
        ADDITIONAL_SERVICES_STATUS_CHANGE
    }
    #endregion
    #region UAT-3824
    public enum CommunicationLanguages
    {
        [StringValue("AAAA")]
        DEFAULT,
        [StringValue("AAAB")]
        SPANISH
    }
    #endregion

    #region UAT-3805
    public enum lkpUseTypeEnum
    {
        [StringValue("AAAA")]
        ROTATION,
        [StringValue("AAAB")]
        COMPLIANCE,
        [StringValue("AAAC")]
        BOTH
    }
    #endregion

    #region Ticket Center
    public enum TicketIssueType
    {
        [StringValue("AAAA")]
        WORKSTATION_DOWNTIME,
        [StringValue("AAAB")]
        SECURITY_CONCERN,
        [StringValue("AAAC")]
        PRIVACY_CONCERN,
        [StringValue("AAAD")]
        APPLICANT_SUPPORT,
        [StringValue("AAAE")]
        TECHNICAL_SUPPORT
    }
    public enum TicketStatusEnum
    {
        [StringValue("AAAA")]
        NEW,
        [StringValue("AAAB")]
        INVALID,
        [StringValue("AAAC")]
        ON_HOLD,
        [StringValue("AAAD")]
        COMPLETED
    }

    public enum TicketSeverityEnum
    {
        [StringValue("AAAA")]
        CRITICAL,
        [StringValue("AAAB")]
        HIGH,
        [StringValue("AAAC")]
        NORMAL,
        [StringValue("AAAD")]
        LOW
    }

    public enum ScreenName
    {
        [StringValue("ManageTickets.ascx")]
        ManageTickets
    }

    public enum PageAction
    {
        [StringValue("upd")]
        Update,
        [StringValue("add")]
        Add
    }

    public enum ClientEntityType
    {
        [StringValue("TKTISSU")]
        TicketIssue = 1,

    }

    #endregion


    #region Language Globalization Enums
    public enum Languages
    {
        [StringValue("AAAA")]
        ENGLISH,
        [StringValue("AAAB")]
        SPANISH,
    }

    public enum LanguageCultures
    {
        [StringValue("en-US")]
        ENGLISH_CULTURE,
        [StringValue("es-MX")]
        SPANISH_CULTURE,
    }

    public enum LanguageTranslationEntityType
    {
        [StringValue("AAAA")]
        BkgSvcAttribute,
        [StringValue("AAAB")]
        ClientSettings,
        [StringValue("AAAC")]
        CustomFormSectionHeader,
        [StringValue("AAAD")]
        InstitutionNodeType,
        [StringValue("AAAE")]
        CustomFormCustomHTML
    }
    #endregion

    #region UAT-3664
    public enum AgencyUserReports
    {
        [StringValue("AAAA")]
        InstitutionCount,
        [StringValue("AAAB")]
        ItemDataReport,
        [StringValue("AAAC")]
        AttestationDocumentReports,
        [StringValue("AAAD")]
        RotationStudentDetailsReport,
        [StringValue("AAAE")]
        AgencyAdminsByDepartment,
        [StringValue("AAAF")]
        ItemDataCountReport,
        [StringValue("AAAG")]
        CategoryDataReport,
        [StringValue("AAAI")]
        ComplianceRecoveryReport,
        [StringValue("AAAJ")]
        RotationStudentsNonComplianceStatus,
        [StringValue("AAAK")]
        RotationStudentsByDayoftheWeek,
        [StringValue("AAAL")]
        SavedReportSearches,
        [StringValue("AAAM")]
        CategoryDataReportWithComplioId
    }

    #endregion

    #region UAT-3691
    /// <summary>
    /// UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
    /// </summary>
    public enum AgencyHierarchyRootNodeSettingType
    {
        [StringValue("AAAA")]
        OPTIONS_FOR_TYPE_SPECIALTY_ROTATION_FIELD,
        [StringValue("AAAB")]
        OPTIONS_TO_SPECIFY_INSTRUCTOR_AVAILABILITY
    }
    #endregion

    #region UAT - 4107
    public enum AdminDataAuditHistoryRole
    {
        [StringValue("ADB Admin")]
        ADBAdmin = 1,
        [StringValue("Client Admin")]
        ClientAdmin = 2,
        [StringValue("Student")]
        Student = 3,
        [StringValue("System")]
        System = 4
    }
    #endregion

    #region UAT-4162

    public enum DataPullStatusType
    {
        [StringValue("AAAA")]
        DUE_STATUS,
        [StringValue("AAAB")]
        DONE_STATUS
    }

    #endregion
    #region UAT - 4165
    public enum RotationDataEditableBy
    {
        [StringValue("ADB Admin")]
        ADBAdmin = 1,
        [StringValue("Client Admin")]
        ClientAdmin = 2,
        [StringValue("Applicant")]
        Applicant = 3

    }
    public enum RotationItemEditableBySettingType
    {
        [StringValue("Default")]
        Default = 1,
        [StringValue("Custom")]
        Custom = 2
    }
    #endregion

    #region Admin Entry Portal

    public enum FeatureAreaType
    {
        [StringValue("AAAA")]
        COMPLIO,
        [StringValue("AAAB")]
        ADMINENTRYPORTAL
    }

    public enum RedirectTokenType
    {
        [StringValue("AAAA")]
        MVPTOREACT,
        [StringValue("AAAB")]
        REACTTOMVP
    }

    public enum AdminEntryAccountSettingEnum
    {
        [StringValue("AAAA")]
        AddNewOrder_Active,
        [StringValue("AAAB")]
        AddApplicantInvite_Active,
        [StringValue("AAAC")]
        Auto_ArchiveTimeLine,
        [StringValue("AAAD")]
        OnHoldStatus,
        [StringValue("AAAE")]
        ApplicantInvite_SubmitStatus
    }

    public enum ApplicantInviteSubmitStatusType
    {
        [StringValue("AAAA")]
        DRAFT,
        [StringValue("AAAB")]
        TRANSMIT,
    }

    public enum AdminEntryOrderLineItemStatus
    {
        [StringValue("AAAA")]
        INPROGRESS,
        [StringValue("AAAB")]
        DRAFT,
        [StringValue("AAAC")]
        COMPLETE,
        [StringValue("AAAD")]
        COMPLETE_FLAGGED,
    }
    public enum AdminEntryOrderDraftStatus
    {
        [StringValue("AAAA")]
        ADMIN_ENTRY,
        [StringValue("AAAB")]
        INVITATION_PENDING,
        [StringValue("AAAC")]
        INVITATION_SENT,
        [StringValue("AAAD")]
        INVITATION_COMPLETED,
    }

    public enum AdminEntryOrderStatus
    {
        [StringValue("AAAA")]
        INPROGRESS,
        [StringValue("AAAB")]
        DRAFT,
        [StringValue("AAAC")]
        ARCHIVED,
        [StringValue("AAAD")]
        COMPLETE,
    }
    #endregion

    #region UAT - 4371
    public enum StudentClientAdminDataAuditHistoryRole
    {
        [StringValue("Client Admin")]
        ClientAdmin = 1,
        [StringValue("Student")]
        Student = 2
    }
    #endregion

    #region UAT-4775
    public enum lkpContentTypeEnum
    {
        [StringValue("AAAA")]
        Applicant_Invite_Landing_Page
    }

    public enum lkpContentRecordTypeEnum
    {
        [StringValue("AAAA")]
        Institution_Hierarchy
    }
    #endregion

    #region UAT - 3683
    public enum SubscriptionTypeCategorySetting
    {
        [StringValue("CP")]
        COMPLIANCE_PACKAGE,
        [StringValue("RP")]
        ROTATION_PACKAGE
    }
    #endregion


    #region UAT-4657
    public enum lkpRequirementPkgVersioningStatus
    {
        [StringValue("AAAA")]
        DUE,
        [StringValue("AAAB")]
        IN_PROGRESS,
        [StringValue("AAAC")]
        NO_ROTATIONS_FOUND_FOR_THE_PACKAGE_MOVEMENT,
        [StringValue("AAAD")]
        COMPLETED,
        [StringValue("AAAE")]
        PACKAGE_COPY_COMPLETED_IN_TENANT
    }
    #endregion

    #region Accessibility Enums
    public struct AccessibilityScreenType
    {
        public const String APPMASTER = "AAAA";
        public const String DEFAULTMASTER = "AAAB";
        public const String PUBLICPAGEMASTER = "AAAC";
        public const String REGISTRATION = "AAAD";
        public const String LOGIN = "AAAE";
        public const String CHANGEPASSWORD = "AAAF";
        public const String APPOINTMENTRESCHEDULE = "AAAG";
        public const String EVENTAPPOINTMENTRESCHEDULE = "AAAH";
        public const String FORGOTPASSWORD = "AAAI";

    }
    #endregion
}