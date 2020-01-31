#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// SysXACFConstants.cs
// Purpose:   This is used for Different Constant used in Acf pages.
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
    /// Client Consts
    /// </summary>
    public class SysXClientConsts
    {
        /// <summary>
        /// Create Client
        /// </summary>
        public const String CREATE_CLIENT = "Create Client";

        /// <summary>
        /// Master Service Aggrment File
        /// </summary>
        public const String MASTER_SERVICE_AGGRMENT_FILE = "MasterServiceAggrmentFile";

        /// <summary>
        /// Field Services Service Agreement File
        /// </summary>
        public const String FIELD_SERVICES_SERVICE_AGREEMENT_FILE = "FieldServicesServiceAgreementFile";

        /// <summary>
        /// Agreement file name
        /// </summary>
        public const String AGREEMENT_FILE = "AgreementFile";

        /// <summary>
        /// forward slash
        /// </summary>
        public const String FORWARD_SLASH = "/";

        /// <summary>
        /// Zero 
        /// </summary>
        public const Int32 Zero = 0;

        /// <summary>
        /// Client has been Saved successfully.
        /// </summary>
        public const String CLIENT_SAVED = "Client has been Saved successfully.";

        /// <summary>
        /// Client has been Updated successfully.
        /// </summary>
        public const String CLIENT_UPDATED = "Client has been Saved successfully.";

        /// <summary>
        /// Client has been Updated successfully.
        /// </summary>
        public const String CLIENT_ON_BOARDING_WIZARD_ASCX = "ClientOnBoardingWizard.ascx";

        /// <summary>
        /// Asset Contributor
        /// </summary>
        public const String ASSET_CONTRIBUTOR = "Asset Contributor";

        /// <summary>
        /// Client Already Exist with this ClientNumber
        /// </summary>
        public const String CLIENT_ALREADY_EXIST_CLIENTNUMBER = "Client already exists.";

        /// <summary>
        /// Edit Client
        /// </summary>
        public const String EDIT_CLIENT = "Edit Client";

        /// <summary>
        /// Client Select        
        /// </summary>
        public const String CLIENT_SELECT = "- Select -";

        /// <summary>
        /// Two Under Score
        /// </summary>
        public const String CLIENT_UNDERSCORE = "__";

        /// <summary>
        /// Statement Of work.
        /// </summary>
        public const String STATEMENT_OF_WORK = "Statement Of Work";

        /// <summary>
        /// Used for Client Service Schedule
        /// </summary>
        public const String CLIENT_SERVICE_SCHEDULE = "Customized Service Schedules";

        /// <summary>
        /// Create Client
        /// </summary>
        public const String INSPECTION_SERVICE = "Inspection Services";

        /// <summary>
        /// Used Key for Season (Primary)
        /// </summary>
        public const String SERVICE_SCHEDULE_SEASON_KEY_PRIMARY = "P";

        /// <summary>
        /// Used Value for Season Primary
        /// </summary>
        public const String SERVICE_SCHEDULE_SEASON_VALUE_PRIMARY = "Primary";

        /// <summary>
        /// Secondary
        /// </summary>
        public const String SERVICE_SCHEDULE_SEASON_KEY_SECONDARY = "S";

        /// <summary>
        /// Secondary
        /// </summary>
        public const String SERVICE_SCHEDULE_SEASON_VALUE_SECONDARY = "Secondary";

        /// <summary>
        /// Clients_Rollback_fails
        /// </summary>
        public const String CLIENTS_ROLLBACK_FAILS = "Client's Rollback fails.";

        /// <summary>
        /// Client asbc
        /// </summary>
        public const String CLIENT_ASBC = "asbc";

        /// <summary>
        /// ClientServiceScheduleLoanTypes
        /// </summary>
        public const String CLIENT_CLIENTSERVICESCHEDULELOANTYPES = "ClientServiceScheduleLoanTypes";

        /// <summary>
        /// lkpServiceType
        /// </summary>
        public const String CLIENT_LKPSERVICETYPE = "lkpServiceType";

        /// <summary>
        /// lkpRegion
        /// </summary>
        public const String CLIENT_LKPREGION = "lkpRegion";

        /// <summary>
        /// ClientServiceScheduleLoanTypes.lkpLoanType
        /// </summary>
        public const String CLIENT_CLIENTSERVICESCHEDULELOANTYPES_LKPLOANTYPE = "ClientServiceScheduleLoanTypes.lkpLoanType";

        /// <summary>
        /// lkpItemAction
        /// </summary>
        public const String CLIENT_LKPITEMACTION = "lkpItemAction";

        /// <summary>
        /// visibility
        /// </summary>
        public const String CLIENT_VISIBILITY = "visibility";

        /// <summary>
        /// hidden
        /// </summary>
        public const String CLIENT_HIDDEN = "hidden";

        /// <summary>
        /// Parent
        /// </summary>
        public const String PARENT = "Parent";

        /// <summary>
        /// Child
        /// </summary>
        public const String CHILD = "Child";

        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String SUB_CLIENT_TITLE = "Sub Client";

        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_RATEMAINTENANCE_SAVE = "Rate Maintenance saved successfully.";

        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_RATEMAINTENANCE_UPDATE = "Rate Maintenance updated successfully.";

        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_LSCI_SAVE = "Loan Servicing Client ID List (LSCI) saved successfully.";

        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_LSCI_UPDATE = "Loan Servicing Client ID List (LSCI) updated successfully.";

        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_SERVICESCHEDULE_SAVE = "Customized Service Schedules  saved successfully.";

        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_SERVICESCHEDULE_UPDATE = "Customized Service Schedules updated successfully.";

        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_SERVICES_SAVE = "Inspection Service Request Type saved successfully.";


        /// <summary>
        /// Frequent Inspection Schedule
        /// </summary>
        public const String CLIENT_FREQUENT_INSPECTION_SCHEDULE_SAVE = "Frequent Inspection Schedule saved successfully.";


        /// <summary>
        /// Frequent Inspection Schedule
        /// </summary>
        public const String CLIENT_FREQUENT_INSPECTION_SCHEDULE_UPDATE = "Frequent Inspection Schedule updated successfully.";


        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_SERVICES_UPDATE = "Inspection Service Request Type updated successfully.";



        /// <summary>
        /// Unique error message MBA Code
        /// </summary>
        public const String CLIENT_SERVICES_UNIQUEMESSAGE = "Combination of Service Request Type/MBA Code and Department already exists.";


        /// <summary>
        /// Unique error message Other Service 
        /// </summary>
        public const String CLIENT_OTHER_SERVICES_UNIQUEMESSAGE = "Service Request Type already exists.";


        /// <summary>
        /// Unique Service Item 
        /// </summary>
        public const String CLIENT_SERVICES_ITEM_UNIQUEMESSAGE = "Service Item already exists.";

        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_OTHER_SERVICES_SAVE = "Other Service Request saved successfully.";

        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_OTHER_SERVICES_UPDATE = "Other Service Request updated successfully.";


        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_SUBCLIENT_SAVE = "Sub Client saved successfully.";

        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_SUBCLIENT_DELETE = "Sub Client deleted successfully.";


        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_SUBCLIENT_UPDATE = "Sub Client updated successfully.";


        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_CONTACT_SAVE = "Client Contacts saved successfully.";

        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_CONTACT_UPDATE = "Client Contacts updated successfully.";


        /// <summary>
        /// Sub client Title
        /// </summary>
        public const String CLIENT_CONTACT_DELETE = "Client Contacts deleted successfully.";

        /// <summary>
        /// Service Item
        /// </summary>
        public const String CLIENT_MANAGE_SERVICE_ITEM_SAVE = "Service Item saved successfully.";


        /// <summary>
        /// Service Item
        /// </summary>
        public const String CLIENT_MANAGE_SERVICE_ITEM_UPDATE = "Service Item updated successfully.";


        /// <summary>
        /// Service Item
        /// </summary>
        public const String CLIENT_MANAGE_SERVICE_ITEM_DELETED = "Service Item deleted successfully.";


        /// <summary>
        /// Service Item
        /// </summary>
        public const String CLIENT_MANAGE_SERVICE_ITEM_DELETED_MESSAGE = "Delete can't perform as Service item is associated with Service Type";



        /// <summary>
        /// 
        /// </summary>
        public const String CLIENT_DASHBOARD_SERVICEREQ_RUSH = "Rush";

        /// <summary>
        /// New
        /// </summary>
        public const String CLIENT_DASHBOARD_SERVICEREQ_NEW = "New";

        /// <summary>
        /// Default Value
        /// </summary>
        public const String CLIENT_DASHBOARD_ITEM_DEFAULT = "Default Value";

        /// <summary>
        /// ClientServiceRequestType
        /// </summary>
        public const String CLIENT_DASHBOARD_CLIENT_SERVICE_REQUESTTYPE = "ClientServiceRequestType";

        /// <summary>
        /// ClientDashBoardGrid
        /// </summary>
        public const String CLIENT_DASHBOARD_CLIENT_DASHBOARDGRID_REQUEST = "ClientDashBoardRequest";
        /// <summary>
        /// UserControlPath
        /// </summary>
        public const String CLIENT_PROFILE_USERCONTROLPATH = "UserControlPath";

        /// <summary>
        /// grpCreateClient
        /// </summary>
        public const String CLIENT_GRPCREATECLIENT = "grpCreateClient";

        /// <summary>
        /// lkpServiceRequestType
        /// </summary>
        public const String LKP_SERVICEREQUESTTYPE = "lkpServiceRequestType";

        /// <summary>
        /// Duplicate Record Exist.
        /// </summary>
        public const String DUPLICATE_RECORD_EXIST = "Duplicate Record Exist for selected Service type.";

        /// <summary>
        /// Invalid schedule date range.
        /// </summary>
        public const String INVALID_DATE_RANGE = "Schedule dates should be between Effective Start Date and Effective End Date.";

        public const String ZeroDecimal = "0.00";


        /// <summary>
        /// User Access time expire
        /// </summary>
        public const String USER_ACCESS_TIME_EXPIRE = "UserAccessTimeExpire";

        /// <summary>
        /// User Access  default time expire
        /// </summary>
        public const String USER_ACCESS_DEFAULT_TIME_EXPIRE = "15";

        /// <summary>
        /// Unique error message Other Service 
        /// </summary>
        public const String CLIENT_LSCI_UNIQUEMESSAGE = "Service Item already exists.";

        /// <summary> 
        /// Key for RadGrid DeleteColumn 
        /// </summary>
        public const String GRID_DELETE_COLUMN = "DeleteColumn";

        /// <summary> 
        /// Key for RadGrid EditCommandColumn 
        /// </summary>
        public const String GRID_EDIT_COLUMN = "EditCommandColumn";

        #region

        /// <summary>
        /// Flat Fee Schedule  saved successfully.
        /// </summary>
        public const String CLIENT_FLATFEESCHEDULE_SAVE = "REO Flat Fee Schedule  saved successfully.";

        /// <summary>
        /// Flat Fee Schedule updated successfully.
        /// </summary>
        public const String CLIENT_FLATFEESCHEDULE_UPDATE = "REO Flat Fee Schedule updated successfully.";

        /// <summary>
        /// Flat Fee Schedule deleted successfully.
        /// </summary>
        public const String CLIENT_FLATFEESCHEDULE_DELETE = "REO Flat Fee Schedule deleted successfully.";


        /// <summary>
        /// REO Flat Fee Schedule deleted successfully.
        /// </summary>
        public const String CLIENT_REO_FLATFEERATE_DELETE = "REO Flat Fee Rate deleted successfully.";

        /// <summary>
        /// REO Flat Fee Rates saved successfully.
        /// </summary>
        public const String CLIENT_REOFLATFEERATES_SAVE = "REO Flat Fee Rates saved successfully.";

        /// <summary>
        /// REO Flat Fee Rates updated successfully.
        /// </summary>
        public const String CLIENT_REOFLATFEERATES_UPDATE = "REO Flat Fee Rates updated successfully.";

        /// <summary>
        /// lkpFlatFeeType
        /// </summary>
        public const String CLIENT_LKPFLATFEETYPE = "lkpFlatFeeType";

        /// <summary>
        /// lkpServiceItemType
        /// </summary>
        public const String CLIENT_LKPSERVICEITEMTYPE = "lkpServiceItemType";


        /// <summary>
        /// lkpFlatFeeType
        /// </summary>
        public const String CLIENT_LKPINOVICEDEPARTMENT = "lkpInvoiceDepartment";

       
        #endregion
    }
}