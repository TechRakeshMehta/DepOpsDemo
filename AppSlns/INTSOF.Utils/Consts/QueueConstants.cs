#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  QueueConstants.cs
// Purpose:   
//
// Revisions:
// Comment
// -------------------------------------------------
// Initial.
// Code review findings incorporation.
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
    /// Stores the constant for Queue.
    /// </summary>
    public static class QueueConstants
    {
        #region Constants

        /// <summary>
        /// Constant for SupplierQueue.
        /// </summary>
        public const String SUPPLIERQUEUE = "SupplierQueue";

        /// <summary>
        /// Constant for BidQueue.
        /// </summary>
        public const String BIDQUEUE = "BidsQueue";

        /// <summary>
        /// Constant for MessageQueue.
        /// </summary>
        public const String MESSAGEQUEUE = "MessageQueue";

        /// <summary>
        /// Constant for QueueAdminSubmitChanges
        /// </summary>
        public const String QUEUEADMINSUBMITCHANGES = "QueueAdminSubmitChanges";

        /// <summary>
        /// Constant for QUEUE STATUS
        /// </summary>
        public const String QUEUESTATUS = "Pending";

        /// <summary>
        /// Constant for QUEUE STATUS CONSTANTS 
        /// </summary>
        public const String STRQUEUESTATUS = "QueueStatus";

        /// <summary>
        /// Constant for QUEUE STATUS APPROVED
        /// </summary>
        public const String QUEUESTATUSAPPROVED = "Approved";

        /// <summary>
        /// Constant for QUEUE STATUS Rejected
        /// </summary>
        public const String QUEUESTATUSREJECTED = "Rejected";

        /// <summary>
        /// Queue Admin Submitter
        /// </summary>
        public const String QUEUE_ADMIN_SUBMITTER = "Submitter.";

        /// <summary>
        /// CALENDAR_DATES_APPROVER
        /// </summary>
        public const String QUEUE_ADMIN_APPROVER = "Approver.";

        /// <summary>
        /// QUEUE_ADMIN_SPACE
        /// </summary>
        public const String QUEUE_ADMIN_SPACE = " ";

        /// <summary>
        /// QUEUE_ADMIN_BREAK
        /// </summary>
        public const String QUEUE_ADMIN_LINE_BREAK = "<br />";

        /// <summary>
        /// Constant for QUEUE Context Code
        /// </summary>
        public const String QUEUECONTEXTCODE = "QADCTX0001";

        /// <summary>
        /// Constant for SUPPLIER Incident Context Code
        /// </summary>
        public const String SUPPLIERINCIDENTCONTEXTCODE = "SPICTX0001";

        /// <summary>
        /// Constant for SUPPLIER Incident Context Code
        /// </summary>
        public const String SERVICEREQUESTCONTEXTCODE = "SUPCTX0013";

        /// <summary>
        /// Constant for QueueAdminApproveRejectChanges
        /// </summary>
        public const String QUEUEADMINAPPROVEREJECTCHANGES = "QueueAdminApproveRejectChanges";

        /// <summary>
        /// Constant for Queue Admin Approve Reject Changes page.
        /// </summary>
        public const String QUEUEADMINAPPROVEREJECTCHANGE = "UserControl/ApproveRejectChangeRequests.ascx";

        /// <summary>
        /// Constant for Client Calendar Approve page.
        /// </summary>
        public const String QUEUEADMINAPPROVEQUEUE = "ClientCalendarApprove.ascx";
        /// <summary>
        /// Constant for Queue Admin Approve Reject Changes page.
        /// </summary>
        public const String QUEUEADMINPAGE = "QueueAdmin.ascx";

        /// <summary>
        /// Constant for Queue Admin
        /// </summary>
        public const String QUEUEADMIN = "Queue Admin";

        /// <summary>
        /// Work flow mapper.
        /// </summary>
        public const String WORKFLOWMAPPER = "WorkFlowMapper";

        /// <summary>
        /// Activity Info.
        /// </summary>
        public const String ACTIVITYINFO = "ActivityInfo";

        /// <summary>
        /// Is User Queue.
        /// </summary>
        public const String ISUSERQUEUE = "IsUserQueue";

        /// <summary>
        /// Update Command.
        /// </summary>
        public const String UPDATECOMMAND = "UpdateCommand";

        /// <summary>
        /// Close Edit Form.
        /// </summary>
        public const String CLOSEEDITFORM = "CloseEditForm";

        /// <summary>
        /// Update Activity.
        /// </summary>
        public const String UPDATEACTIVITY = "UpdateActivity";

        /// <summary>
        /// Calendar.
        /// </summary>
        public const String CALENDAR = "Calendar";

        /// <summary>
        /// Business.
        /// </summary>
        public const String BUSINESS = "Business";

        /// <summary>
        /// Url page.
        /// </summary>
        public const String URLPAGENAME = "Default.aspx?{0}={1}";

        /// <summary>
        /// Url page.
        /// </summary>
        public const String QUEUE_URL_PAGE_NAME = "../Queues/Default.aspx?{0}={1}";

        /// <summary>
        /// MYQUEUEURL
        /// </summary>
        public const String MYQUEUEURL = "../Queues/Default.aspx?{0}={1}";

        /// <summary>
        /// Client Id.
        /// </summary>
        public const String CLIENTID = "ClientId";

        /// <summary>
        /// Client Id.
        /// </summary>
        public const String QUEUEID = "QueueId";

        /// <summary>
        /// Context Id.
        /// </summary>
        public const String CONTEXTID = "ContextID";

        /// <summary>
        /// Client Id for Queue Email.
        /// </summary>
        public const String QUEUECLIENTID = "ClientID";

        /// <summary>
        /// Is Business Day.
        /// </summary>
        public const String ISBUSINESSDAY = "IsBusinessDay";

        /// <summary>
        /// Queue Name.
        /// </summary>
        public const String QUEUEADMIN_QUEUENAME = "QueueName";

        /// <summary>
        /// Approval Queue.
        /// </summary>
        public const String QUEUEADMIN_APPROVALQUEUE = "ApprovalQueue";

        /// <summary>
        /// Default Path.
        /// </summary>
        public const String QUEUEADMIN_DEFAULT = "../QueueAdmin/Default.aspx?{0}={1}";

        /// <summary>
        /// Default Path.
        /// </summary>
        public const String SERVICEREQUEST_DEFAULT = "../ServiceRequest/Default.aspx?{0}={1}";

        /// <summary>
        /// Default Path.
        /// </summary>
        public const String CLIENTPORTAL_DEFAULT = "../ClientPortal/Default.aspx?{0}={1}";

        /// <summary>
        /// Client Portal.
        /// </summary>
        public const String CLIENTPORTAL = "ClientPortal";

        /// <summary>
        /// Default Path.
        /// </summary>
        public const String INSURANCELOSS_DEFAULT = "../InsuranceLoss/Default.aspx?{0}={1}";

        /// <summary>
        /// User Queue
        /// </summary>
        public const String QUEUEADMIN_USERQUEUE = "UserQueue";

        /// <summary>
        /// Approve
        /// </summary>
        public const String QUEUEADMIN_APPROVE = "A";

        /// <summary>
        /// Reject
        /// </summary>
        public const String QUEUEADMIN_REJECT = "R";

        /// <summary>
        /// ADB
        /// </summary>
        public const String QUEUEADMIN_ADB = "ADB";

        /// <summary>
        /// Time
        /// </summary>
        public const String QUEUEADMIN_TIME = "Time";

        /// <summary>
        /// Activity
        /// </summary>
        public const String QUEUEADMIN_ACTIVITY = "Activity";

        /// <summary>
        /// Constant for Cancel
        /// </summary>
        public const String QUEUESTATUSCANCEL = "Cancel";

        /// <summary>
        /// Constant for ApproveReject
        /// </summary>
        public const String QUEUEAPPROVEREJECT = "ApproveReject";

        /// <summary>
        /// Constant for QueueAdmin
        /// </summary>
        public const String QUEUE_QUEUEADMIN = "QueueAdmin";

        /// <summary>
        /// Constant for UserQueue.ascx
        /// </summary>
        public const String QUEUE_USERQUEUE_ASCX = "../Queues/Default.aspx?{0}={1}";

        /// <summary>
        /// Constant for True
        /// </summary>
        public const String QUEUE_TRUE = "True";

        /// <summary>
        /// Constant for View Mode.
        /// </summary>
        public const String QUEUE_VIEWMODE = "ViewMode";

        /// <summary>
        /// Constant for Queue
        /// </summary>
        public const String QUEUE = "Queue";

        /// <summary>
        /// Queue Type
        /// </summary>
        public const String QUEUE_TYPE = "QueueType";


        /// <summary>
        /// Queue Type
        /// </summary>
        public const String QUEUE_TYPE_REGION = "Region";


        /// <summary>
        /// Queue Type
        /// </summary>
        public const String REGION_ID = "RegionID";

        /// <summary>
        /// Item Data Queue secondary sorting fields. 
        /// </summary>
        public const String DEFAULT_SORTING_FIELDS = " , ApplicantComplianceCategoryId, ComplianceItemId";

        public const String DEFAULT_SORTING_FIELDS_ASSIGNMENT = "ApplicantComplianceCategoryId, ComplianceItemId";

        public const String DEFAULT_SORTING_FIELDS_COMPLIANCE_SEARCH = "OrderId";
        public const String DEFAULT_SORTING_FIELDS_UPCOMING_EXPIRATION_SEARCH = "StudentID";

        /// <summary>
        /// Order Queue Default sorting fields. 
        /// </summary>
        public const String ORDER_QUEUE_DEFAULT_SORTING_FIELDS = "OrderId";

        /// <summary>
        /// Order Queue secondary sorting fields. 
        /// </summary>
        public const String ORDER_QUEUE_SECONDARY_SORTING_FIELDS = ", OrderId";

        /// <summary>
        /// Applicant Search Queue Default sorting fields. 
        /// </summary>
        public const String APPLICANT_SEARCH_DEFAULT_SORTING_FIELDS = "ApplicantFirstName";

        /// <summary>
        ///  Applicant Search Queue secondary sorting fields. 
        /// </summary>
        public const String APPLICANT_SEARCH_SECONDARY_SORTING_FIELDS = "OrganizationUserId";

        /// <summary>
        /// Communication Summary Default sorting fields. 
        /// </summary>
        public const String COM_SUMMARY_DEFAULT_SORTING_FIELDS = "CommunicationSubEvent";

        /// <summary>
        /// Communication Summary secondary sorting fields. 
        /// </summary>
        public const String COM_SUMMARY_SECONDARY_SORTING_FIELDS = ", CommunicationSubEvent";

        /// <summary>
        /// Applicant Search Queue Default sorting fields. 
        /// </summary>
        public const String USER_SEARCH_DEFAULT_SORTING_FIELDS = "UserFirstName";

        /// <summary>
        /// Order Reveiw Queue Default sorting fields. 
        /// </summary>
        public const String ORDER_REVIEW_QUEUE_DEFAULT_SORTING_FIELDS = "OrderID";

        /// <summary>
        ///  Order Reveiw Queue secondary sorting fields. 
        /// </summary>
        public const String ORDER_REVIEW_QUEUE_SECONDARY_SORTING_FIELDS = ", OrderID";

        #endregion

    }
}
