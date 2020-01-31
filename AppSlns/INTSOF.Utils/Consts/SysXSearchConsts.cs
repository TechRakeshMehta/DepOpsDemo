#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXSearchConsts.cs
// Purpose:  Represents SysX search constants.
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
    /// Represents SysX search constants
    /// </summary>
    public static class SysXSearchConsts
    {
        //Validation Constants
        public const String INVALID_SPECIAL_CHARACTERS = "^[^;^#^~^<^>]*$";
       
        //General Constants
        public const String QUICK_SEARCH_OPTION = "QuickSearchOption";
        public const String SELECTED_QUICK_SEARCH_OPTION = "SelectedSearchOption";
        public const String SEARCH_PAGE = "Search/default.aspx?{0}={1}";
        public const String VIEW_MODE = "ViewMode";
        public const String SEARCH_MODE = "SearchMode";
        public const String LOAD_RESULTS = "LoadResults";
        public const String SEARCH_OPTIONS = "SearchOptions";
        public const char SEARCH_VALUE_SEPERATOR = ';';
        public const char QUICK_SEARCH_VALUE_SEPERATOR = ' ';
        public const char SEARCH_FIELD_SEPERATOR = '#';
        public const char SEARCH_MULTIVALUE_SEPERATOR = '~';
        public const String SEARCHTYPE = "//SearchType/";
        public const String HEADERTEXT = "HeaderText";
        public const String DATAFIELD = "DataField";
        public const String UNIQUENAME = "UniqueName";
        public const String SEARCHGRIDSETTINGFILE = "SearchGridSettingFile";
        public const String SOURCE_NAME = "SourceName";
        public const String NON_SEARCHABLE_ITEM = "NonSearchableItem";


        // Quick Search Options
        public const String QUICK_SEARCH_ITEMS = "Fields";
        public const String QUICK_SEARCH_ITEMSVALUE = "FieldValues";

        //Asset Search Options
        public const String ASSET_SEARCH_OPTION_ASSETNUMBER = "AssetNumber";
        public const String ASSET_SEARCH_OPTION_ADDRESS = "Address1";
        public const String ASSET_SEARCH_OPTION_CLIENTLSCI = "ClientLSCI";
        public const String ASSET_SEARCH_OPTION_CLIENTNAME = "ClientName";
        public const String ASSET_SEARCH_OPTION_CLIENTNUMBER = "ClientNumber";
        public const String ASSET_SEARCH_OPTION_LEGACYCLIENTNUMBER = "LegacyClientNumber";
        public const String ASSET_SEARCH_OPTION_ZIP = "Zip";
        public const String ASSET_SEARCH_OPTION_LOANNUMBER = "LoanNumber";
        public const String ASSET_SEARCH_OPTION_LOANTYPENAME = "LoanTypeName";
        public const String ASSET_SEARCH_OPTION_LOANTYPEDESC = "LoanTypeDesc";
        public const String ASSET_SEARCH_OPTION_CITYNAME = "CityName";
        public const String ASSET_SEARCH_OPTION_STATENAME = "StateName";
        public const String ASSET_SEARCH_OPTION_STATEID = "StateID";
        public const String ASSET_SEARCH_OPTION_LOANTYPEID = "LoanTypeID";
        public const String ASSET_SEARCH_OPTION_ASSETDETAILS = "AssetDetails";
        public const String ASSET_SEARCH_OPTION_ASSETDETAILS_HEADER = "Property and Loan Details";
        public const String ASSET_SEARCH_OPTION_ARCHIVEDETAILS = "ArchiveDetails";
        public const String ASSET_SEARCH_OPTION_CRITICALEVENTHISTORY = "CriticalEventHistory";
        public const String ASSET_SEARCH_OPTION_REPORTEMERGENCY = "ReportEmergency";
        public const String ASSET_SEARCH_OPTION_BIDSUMMARY = "BidSummary";

        //Supplier Search Options
        public const String SUPPLIER_SEARCH_OPTION_STATENAME = "StateName";
        public const String SUPPLIER_SEARCH_OPTION_STATEID = "StateID";
        public const String SUPPLIER_SEARCH_OPTION_SUPPLIERRATINGTYPE = "SupplierRatingDesc";
        public const String SUPPLIER_SEARCH_OPTION_SUPPLIERRATINGID = "SupplierRatingID";
        public const String SUPPLIER_SEARCH_OPTION_SUPPLIERSTATUSNAME = "SupplierStatus";
        public const String SUPPLIER_SEARCH_OPTION_SUPPLIERSTATUSID = "SupplierStatusID";
        public const String SUPPLIER_SEARCH_OPTION_SUPPLIERID = "SupplierID";
        public const String SUPPLIER_SEARCH_OPTION_SUPPLIERNAME = "SupplierName";
        public const String SUPPLIER_SEARCH_OPTION_COUNTYNAME = "CountyName";
        public const String SUPPLIER_SEARCH_OPTION_CITYNAME = "CityName";
        public const String SUPPLIER_SEARCH_OPTION_PRIMARY_ZIP = "PrimaryZip";
        public const String SUPPLIER_SEARCH_OPTION_SECONDARY_ZIP = "SecondaryZip";
        public const String SUPPLIER_SEARCH_OPTION_WORKTYPEDESCRIPTION = "ServiceTypeDesc";
        public const String SUPPLIER_SEARCH_OPTION_WORKTYPE = "SupplierWorktype";
        public const String SUPPLIER_SEARCH_OPTION_WORKTYPEID = "SvcTypeID";
        public const String SUPPLIER_SEARCH_OPTION_SUPPLIER_SPECILITYSERVICEDESC = "ServiceTypeDesc";
        public const String SUPPLIER_SEARCH_OPTION_SUPPLIER_SPECILITYSERVICE = "SupplierSpecialityServiceDesc";
        public const String SUPPLIER_SEARCH_OPTION_SUPPLIER_SPECILITYSERVICEID = "SvcTypeID";
        public const String SUPPLIER_SERVICE_REQUEST_ID = "SupplierServiceRequestID";
        public const String SUPPLIER_ACTIVITY_TYPE = "SupplierActivityType";
        public const string SUPPLIER_SEARCH_OPTION_INCIDENT_REPORTED = "IncidentsReported";
        public const string SUPPLIER_SEARCH_OPTION_INCIDENTS_REPORTED = "Report/View Incidents";

        //Client Search Options
        public const String CLIENT_SEARCH_OPTION_STATEID = "StateID";
        public const String CLIENT_SEARCH_OPTION_STATENAME = "StateName";
        public const String CLIENT_SEARCH_OPTION_STATUS = "Status";
        public const String CLIENT_SEARCH_OPTION_CLIENTNAME = "ClientName";
        public const String CLIENT_SEARCH_OPTION_CLIENTNUMBER = "ClientNumber";
        public const String CLIENT_SEARCH_OPTION_ADDRESS = "Address1";
        public const String CLIENT_SEARCH_OPTION_CLIENTLSCI = "ClientLSCI";
        public const String CLIENT_SEARCH_OPTION_CITYNAME = "CityName";
        public const String CLIENT_SEARCH_OPTION_ZIPCODE = "ZipCode";
        public const String CLIENT_SEARCH_OPTION_USERID = "UserID";
        public const String CLIENT_SEARCH_OPTION_LSCINUMBER = "LSCINumber";


        //User Search Options
        public const String USER_SEARCH_OPTION_FIRSTNAME = "FirstName";
        public const String USER_SEARCH_OPTION_LASTNAME = "LastName";
        public const String USER_SEARCH_OPTION_EMAIL = "Email";
        public const String USER_SEARCH_OPTION_MOBILE = "Mobile";
        public const String USER_SEARCH_OPTION_USERNAME = "UserName";
        public const String USER_SEARCH_OPTION_PARENTNAME = "OrganizationName";

        //Investor/Insurer Search Option
        public const String INVESTOR_SEARCH_OPTION_PROFILEID = "ProfileId";
        public const String INVESTOR_SEARCH_OPTION_INVESTORID = "InvestorId";
        public const String INVESTOR_SEARCH_OPTION_NAME = "Name";
        public const String INVESTOR_SEARCH_OPTION_SERVICETYPE = "ServiceType";
        public const String INVESTOR_SEARCH_OPTION_EFFECTIVESTARTDATE = "EffectiveStartDate";
        public const String INVESTOR_SEARCH_OPTION_EFFECTIVEENDDATE = "EffectiveEndDate";
        public const String INVESTOR_SEARCH_OPTION_LOANTYPE = "LoanType";
        public const String INVESTOR_SEARCH_OPTION_CITY = "City";
        public const String INVESTOR_SEARCH_OPTION_STATE = "State";
        public const String INVESTOR_SEARCH_OPTION_ZIP = "Zip";
        public const String INVESTOR_SEARCH_OPTION_CLIENT = "Client";
        public const String INVESTOR_SEARCH_OPTION_STATUS = "Status";
        public const String INVESTOR_SEARCH_OPTION_INVSURER_FLAG = "IsInsurer";


        //Organization Search Options
        public const String ORGANIZATION_SEARCH_ORGANIZATIONNAME = "OrganizationName";
        public const String ORGANIZATION_SEARCH_TENANTNAME = "TenantName";
        public const String ORGANIZATION_SEARCH_STATUS = "Status";
        public const String ORGANIZATION_SEARCH_ADDRESS = "Address";
        public const String ORGANIZATION_SEARCH_STATE = "State";
        public const String ORGANIZATION_SEARCH_CITY = "City";
        public const String ORGANIZATION_SEARCH_ZIP = "Zip";
        public const String ORGANIZATION_SEARCH_CREATEDBY = "CreatedBy";

        //Product Search Options
        public const String PRODUCT_SEARCH_NAME = "ProductName";
        public const String PRODUCT_SEARCH_TENANTNAME = "TenantName";
        public const String PRODUCT_SEARCH_STATUS = "Status";
        public const String PRODUCT_SEARCH_CREATEDBY = "CreatedBy";

        //Item Search Options
        public const String ITEM_SEARCH_NUMBER = "ItemNumber";
        public const String ITEM_SEARCH_NAME = "ItemName";
        public const String ITEM_SEARCH_CATEGORY = "ItemCategoryID";
        public const String ITEM_SEARCH_SUBCATEGORY = "ItemSubCategoryID";
        public const String ITEM_SEARCH_ACTION = "ItemActionID";
        public const String ITEM_SEARCH_STARTDATE = "EffectiveStartDate";
        public const String ITEM_SEARCH_ENDDATE = "EffectiveEndDate";
        public const String ITEM_SEARCH_LINEOFBUSINESS = "LineOfBusiness";
        public const String ITEM_SEARCH_KNOWLEDGEBASELINK = "KnowledgeBaseLink";
        public const String ITEM_SEARCH_KNOWLEDGEBASELINK_HEADER = "Knowledge Base Link";
        public const String ITEM_SEARCH_LOB = "LOB";
       

        //Employee Search Options
        public const String EMPLOYEE_SEARCH_NAME_TYPE = "EmployeeTypeDesc";
        public const String EMPLOYEE_SEARCH_TYPEID = "EmployeeTypeID";
        public const String EMPLOYEE_SEARCH_NUMBER = "EmployeeNumber";
        public const String EMPLOYEE_SEARCH_FIRSTNAME = "FirstName";
        public const String EMPLOYEE_SEARCH_MIDDLENAME = "MiddleInitial";
        public const String EMPLOYEE_SEARCH_LASTNAME = "LastName";
        public const String EMPLOYEE_SEARCH_ID = "EmployeeID";
        public const String EMPLOYEE_SEARCH_ManagerID = "ManagerID";
        public const String EMPLOYEE_SEARCH_MANAGERNAME = "ManagerName";
        public const String EMPLOYEE_SEARCH_HIREDATE = "HireDate";
        public const String EMPLOYEE_SEARCH_ORGANIZATIONNAME = "OrganizationName";
        public const String EMPLOYEE_SEARCH_ORGANIZATIONID = "OrganizationID";
        public const String EMPLOYEE_SEARCH_STATUS = "Status";
        public const String EMPLOYEE_SEARCH = "Employee Search";


        //Zone Search Option
        public const String ZONE_SEARCH_OPTION_ZONENAME = "ZoneName";
        public const String ZONE_SEARCH_OPTION_ZONETYPE = "ZoneTypeID";
        public const String ZONE_SEARCH_OPTION_ZONESTATE = "ZoneState";
        public const String ZONE_SEARCH_OPTION_ZONEZIP = "ZoneZips";
        public const String ZONE_SEARCH_OPTION_SUPPLIERS = "AssignedSuppliers";
        public const String ZONE_SEARCH_OPTION_ZONELOB = "LineOfBusiness";

        // Service Request Search
        public const String SRV_REQ_SEARCH_OPTION_CLIENTNUMBER = "ClientID";
        public const String SRV_REQ_SEARCH_OPTION_LSCINUMBER = "LSCINumber";
        public const String SRV_REQ_SEARCH_OPTION_LOANNUMBER = "LoanNumber";
        public const String SRV_REQ_SEARCH_OPTION_LOBID = "LOBID";
        public const String SRV_REQ_SEARCH_OPTION_SERVICEREQUESTID = "ServiceRequestID";

        //Search Results Constants
        public const String SEARCH_RESULT_ASSETADDRESSID = "AssetAddressID";
        public const String SEARCH_RESULT_ASSETDETAILS = "AssetDetails";
        public const String SEARCH_RESULT_LOANID = "LoanID";
        public const String SEARCH_RESULT_ASSETUNITID = "AssetUnitID";
        public const String SEARCH_RESULT_ASSETVIEWMODE = "AssetViewMode";
        public const String SEARCH_RESULT_PROPERTYDETAILS = "PropertyDetails";
        public const String SEARCH_RESULT_PRIMARYZIP = "PrimaryZip";
        public const String SEARCH_RESULT_SECONDARYZIP = "SecondaryZip";
        public const String SEARCH_RESULT_SUPPLIERWORKTYPE = "SupplierWorktype";
        public const String SEARCH_RESULT_ASSETCITYNAME = "CityName";
        public const String SEARCH_RESULT_ASSETSTATENAME = "StateName";
        public const String SEARCH_RESULT_ITEMID = "ItemID";
        public const String SEARCH_RESULT_CLIENTADDRESSID = "ClientAddressID";
        public const String SEARCH_RESULT_LASTACTIVITYDATE = "LastActivityDate";
        public const String SEARCH_RESULT_SERVICETYPE = "ServiceType";
        public const String SEARCH_RESULT_CLIENT = "Client";
        public const String SEARCH_RESULT_CLIENTID = "ClientID";
        public const String SEARCH_RESULT_ZONEID = "ZoneId";

        // Asset Emergency Search - Under Supplier Portal
        public const String SEARCH_RESULT_ASSETLOANID = "AssetLoanId";
        public const String SEARCH_RESULT_ASSETID = "AssetId";
        public const String SEARCH_RESULT_SERVICEORDERID = "ServiceOrderId";
        
        //Added For Search
        public const String Emails = "emails";
        public const String Alerts = "alerts";
        public const String SMS = "sms";
        public const String Chats = "chats";
        public const String Applicants = "applicants";
        public const String Students = "students";
        public const String Messages = "messages";
        public const String CommunicationTypes = "communicationtypes";
    }
}