#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXEntityConstants.cs
// Purpose:  
//

#endregion

#region Namespaces

#region Application Specific

using System;

#endregion

#endregion

namespace DAL
{
    /// <summary>
    /// Class for System X entity constants.
    /// </summary>
    public static class SysXEntityConstants
    {
        #region Constants

        /// <summary>
        /// Constant for ServiceOrders.ServiceRequest.AssetLoan.Asset.ValidatedAddress.ZipCode
        /// </summary>        
        public static String TABLE_ASSETLOAN_DOT_ASSET_DOT_VALIDATEDADDRESS_DOT_ZIPCODE = "AssetLoan.Asset.ValidatedAddress.ZipCode";

        /// <summary>
        /// Constant for ServiceOrders.ServiceRequest.AssetLoan.Asset.ValidatedAddress.ZipCode
        /// </summary>        
        public static String TABLE_SERVICESORDERS_DOT_SERVICEREQUEST_DOT_ASSETLOAN_DOT_ASSET_DOT_VALIDATEDADDRESS_DOT_ZIPCODE = "ServiceOrders.ServiceRequest.AssetLoan.Asset.ValidatedAddress.ZipCode";

        /// <summary>
        /// Constant for ServiceOrders.ServiceRequest.AssetLoan.Asset.ValidatedAddress.ZipCode.City
        /// </summary>
        public static String TABLE_ASSETLOAN_DOT_ASSET_DOT_VALIDATEDADDRESS_DOT_ZIPCODE_DOT_CITY = "AssetLoan.Asset.ValidatedAddress.ZipCode.City";

        /// <summary>
        /// Constant for ServiceOrders.ServiceRequest.AssetLoan.Asset.ValidatedAddress.ZipCode.City
        /// </summary>
        public static String TABLE_SERVICESORDERS_DOT_SERVICEREQUEST_DOT_ASSETLOAN_DOT_ASSET_DOT_VALIDATEDADDRESS_DOT_ZIPCODE_DOT_CITY = "ServiceOrders.ServiceRequest.AssetLoan.Asset.ValidatedAddress.ZipCode.City";

        /// <summary>
        /// Constant for   Address.ZipCode.State
        /// </summary>
        public static String TABLE_ASSETLOAN_DOT_ASSET_DOT_VALIDATEDADDRESS_DOT_ZIPCODE_DOT_CITY_DOT_STATE = "AssetLoan.Asset.ValidatedAddress.ZipCode.City.State";

        /// <summary>
        /// Constant for   Address.ZipCode.State
        /// </summary>
        public static String TABLE_SERVICESORDERS_DOT_SERVICEREQUEST_DOT_ASSETLOAN_DOT_ASSET_DOT_VALIDATEDADDRESS_DOT_ZIPCODE_DOT_CITY_DOT_STATE = "ServiceOrders.ServiceRequest.AssetLoan.Asset.ValidatedAddress.ZipCode.City.State";

        /// <summary>
        /// Constant for  ServiceOrders.ServiceRequest.AssetLoan.Asset.ValidatedAddress
        /// </summary>
        public static String TABLE_ASSETLOAN_DOT_ASSET_DOT_VALIDATEDADDRESS = "AssetLoan.Asset.ValidatedAddress";

        /// <summary>
        /// Constant for  ServiceOrders.ServiceRequest.AssetLoan.Asset.ValidatedAddress
        /// </summary>
        public static String TABLE_SERVICESORDERS_DOT_SERVICEREQUEST_DOT_ASSETLOAN_DOT_ASSET_DOT_VALIDATEDADDRESS = "ServiceOrders.ServiceRequest.AssetLoan.Asset.ValidatedAddress";

        /// <summary>
        /// Constant for lkpAssetAttributeTypes.
        /// </summary>
        public static String TABLE_LKP_ASSET_ATTRIBUTE_TYPE = "lkpGuidelineAttributeType";

        /// <summary>
        /// Constant for lkpContexts.
        /// </summary>
        public static String TABLE_LKP_CONTEXTS = "lkpContext";

        /// <summary>
        /// Constant for AssetAddresses.
        /// </summary>
        public static String TABLE_ASSET_ADDRESSES = "AssetAddresses";

        /// <summary>
        /// Constant for  AssetAddresses.Address.
        /// </summary>
        public static String TABLE_ASSET_ADDRESSES_DOT_ADDRESS = "AssetAddresses.Address";

        /// <summary>
        /// Constant for  AssetAddresses.Address.GeoCoding.
        /// </summary>
        public static String TABLE_ASSET_ADDRESSES_DOT_ADDRESS_DOT_GEO_CODING = "AssetAddresses.Address.GeoCoding";

        /// <summary>
        /// Constant for  AssetAddresses.Address.
        /// </summary>
        public static String TABLE_ASSET_ADDRESSES_DOT_ADDRESS_DOT_ZIPCODE = "AssetAddresses.Address.ZipCode";

        /// <summary>
        /// Constant for  AssetAddresses.Address.
        /// </summary>
        public static String TABLE_ASSET_ADDRESSES_DOT_ADDRESS_DOT_ZIPCODE_DOT_CITY = "AssetAddresses.Address.ZipCode.City";

        /// <summary>
        /// Constant for  AssetAddresses.Address.ZipCode.State.
        /// </summary>
        public static String TABLE_ASSET_ADDRESSES_DOT_ADDRESS_DOT_ZIPCODE_DOT_SATE = "AssetAddresses.Address.ZipCode.State";

        /// <summary>
        /// Constant for   Address.ZipCode
        /// </summary>
        public static String TABLE_ADDRESS_DOT_ZIPCODE = "ValidatedAddress.ZipCode";

        /// <summary>
        /// Constant for   Address.ZipCode.City
        /// </summary>
        public static String TABLE_ADDRESS_DOT_ZIPCODE_DOT_CITY = "ValidatedAddress.ZipCode.City";

        /// <summary>
        /// Constant for   Address.ZipCode.State
        /// </summary>
        public static String TABLE_ADDRESS_DOT_ZIPCODE_DOT_CITY_STATE = "ValidatedAddress.ZipCode.City.State";

        /// <summary>
        /// Constant for ZipCode.
        /// </summary>
        public static String TABLE_ZIPCODE = "ZipCode";

        /// <summary>
        /// Constant for AssetType.
        /// </summary>
        public static String TABLE_ASSET_TYPE = "lkpAssetType";

        /// <summary>
        /// Constant for AssetUnitNumbers.
        /// </summary>
        public static String TABLE_ASSET_UNIT_NUMBERS = "AssetUnitNumbers";

        /// <summary>
        /// Constant for Loans.Contact.
        /// </summary>
        public static String TABLE_LOANS_DOT_CONTACT = "Loans.Contact";

        /// <summary>
        /// Constant for Loans.LoanStatu
        /// </summary>
        public static String TABLE_LOANS_DOT_LOAN_STATU = "Loans.LoanStatu";

        /// <summary>
        /// Constant for Loans.
        /// </summary>
        public static String TABLE_LOANS = "Loans";

        /// <summary>
        /// Constant for Loans.Liens.
        /// </summary>
        public static String TABLE_LOANS_DOT_LIENS = "Loans.Liens";

        /// <summary>
        /// Constant for Loans.LoanDetails.
        /// </summary>
        public static String TABLE_LOANS_DOT_LOAN_DETAILS = "Loans.LoanDetails";

        /// <summary>
        /// Constant for Loans.LoanType.
        /// </summary>
        public static String TABLE_LOANS_DOT_LOAN_TYPE = "Loans.LoanType";

        /// <summary>
        /// Constant for  Loans.Client
        /// </summary>
        public static String TABLE_LOANS_DOT_CLIENT = "Loans.Client";

        /// <summary>
        /// Constant for  AssetAddresses.Address
        /// </summary>
        public static String TABLE_ASSETADDRESSES_DOT_ADDRESS = "AssetAddresses.Address";

        /// <summary>
        /// Constant for  Address
        /// </summary>
        public static String TABLE_ADDRESS = "ValidatedAddress";

        /// <summary>
        /// Constant for   AssetEvents.HOA
        /// </summary>
        public static String TABLE_ASSET_EVENTS_DOT_HOA = " AssetEvents.HOA";

        /// <summary>
        /// Constant for Contact2.
        /// </summary>
        public static String TABLE_CONTACT2 = "Contact2";

        /// <summary>
        /// Constant for Contact1.
        /// </summary>
        public static String TABLE_CONTACT1 = "Contact1";

        /// <summary>
        /// Constant for Contact.
        /// </summary>
        public static String TABLE_CONTACT = "Contact";

        /// <summary>
        /// Constant for LoanStatu.
        /// </summary>
        public static String TABLE_LOAN_STATU = "LoanStatu";

        /// <summary>
        /// Constant for Liens.
        /// </summary>
        public static String TABLE_LIENS = "Liens";

        /// <summary>
        /// Constant for LoanDetails.
        /// </summary>
        public static String TABLE_LOAN_DETAILS = "LoanDetails";

        /// <summary>
        /// Constant for LoanType.
        /// </summary>
        public static String TABLE_LOAN_TYPE = "LoanType";

        /// <summary>
        /// Constant for ContributorDetail.
        /// </summary>
        public static String TABLE_CONTRIBUTOR_DETAIL = "ContributorDetail";

        /// <summary>
        /// Constant for ContributorContacts.
        /// </summary>
        public static String TABLE_CONTRIBUTOR_CONTACTS = "ContributorContacts";

        /// <summary>
        /// Constant for Contributor1.
        /// </summary>
        public static String TABLE_CONTRIBUTOR1 = "Contributor1";

        /// <summary>
        /// Constant for Client.
        /// </summary>
        public static String TABLE_CLIENT = "Client";

        /// <summary>
        /// lkpRegion
        /// </summary>
        public static String TABLE_LKPREGION = "lkpRegion";

        /// <summary>
        /// 
        /// </summary>
        public static String TABLE_LKPSERVICEITEM_CLIENT_ITEM_DOT_LKPITEM_LKPITEMACTION = "lkpServiceItem.ClientItem.lkpItem.lkpItemAction";

        /// <summary>
        /// 
        /// </summary>
        public static String TABLE_LKPSERVICEITEM_DOT_LKPITEM_DOT_LKPITEMACTION = "lkpServiceItem.lkpItem.lkpItemAction";

        /// <summary>
        /// lkpcontact type
        /// </summary>
        public static String TABLE_CLIENT_CONTACT_DOT_LKP_CONTACT_TYPE = "lkpContactType";

        /// <summary>
        /// lkpOccupancyStatusClientRate
        /// </summary>
        public static String TABLE_LKPOCCUPANCY_STATUS_CLIENT_RATE = "lkpOccupancyStatusClientRate";

        /// <summary>
        /// lkpFrequentInspectionFrequencyType
        /// </summary>
        public static String TABLE_LKPFREQUENT_INSPECTION_FREQUENCY_TYPE = "lkpFrequentInspectionFrequencyType";

        /// <summary>
        /// lkpServicingSystem
        /// </summary>
        public static String TABLE_LKPSERVICINGSYSTEM = "lkpServicingSystem";

        /// <summary>
        /// Constant for AssetEvents.
        /// </summary>
        public static String TABLE_ASSET_EVENTS = "AssetEvents";

        /// <summary>
        /// Constant for HOA.
        /// </summary>
        public static String TABLE_HOA = "HOA";

        /// <summary>
        /// Constant for HOA.ValidatedAddress
        /// </summary>
        public static String TABLE_HOA_ADDRESSHANDLE_ADDRESS_DOT_ZIPCODE_DOT_CITY_DOT_STATE = "HOA.AddressHandle.Addresses.ZipCode.City.State";

        /// <summary>
        /// Constant for AssetEventContacts.NonContributor
        /// </summary>
        public static String TABLE_ASSET_EVENT_CONTACTS_DOT_NON_CONTRIBUTOR = "AssetEventContacts.NonContributor";

        /// <summary>
        /// Constant for "AssetEventContacts"
        /// </summary>
        public static String TABLE_ASSET_EVENT_CONTACTS = "AssetEventContacts";

        /// <summary>
        /// Constant for "AssetEventContacts.lkpAssetContactType"
        /// </summary>
        public static String TABLE_ASSET_EVENT_CONTACTS_DOT_LKPASSETCONTACTTYPE = "AssetEventContacts.lkpAssetContactType";

        /// <summary>
        /// Constant for AssetEventContacts.NonContributor.Contact
        /// </summary>
        public static String TABLE_ASSET_EVENT_CONTACTS_DOT_NON_CONTRIBUTOR_DOT_CONTACT = "AssetEventContacts.NonContributor.Contact";

        /// <summary>
        /// Constant for AssetEventContacts.NonContributor.Contact.ContactDetails
        /// </summary>
        public static String TABLE_ASSET_EVENT_CONTACTS_DOT_NON_CONTRIBUTOR_DOT_CONTACT_DOT_CONTACT_DETAILS = "AssetEventContacts.NonContributor.Contact.ContactDetails";

        /// <summary>
        /// Constant for AssetEventContacts.NonContributor.AddressHandle.Addresses.ZipCode.City.State
        /// </summary>
        public static String TABLE_ASSET_EVENT_CONTACTS_DOT_NON_CONTRIBUTOR_ADDRESSHANDLE_ADDRESS_DOT_ZIPCODE_DOT_CITY_DOT_STATE = "AssetEventContacts.NonContributor.AddressHandle.Addresses.ZipCode.City.State";

        /// <summary>
        /// Constant for "AssetEventStatus.lkpAssetEventStatusType"
        /// </summary>
        public static String TABLE_ASSET_EVENT_STATUS_DOT_LKPASSETEVENTSTATUSTYPE = "AssetEventStatus.lkpAssetEventStatusType";

        /// <summary>
        /// Constant for "AssetEventPayments.lkpFeeOccurence"
        /// </summary>
        public static String TABLE_ASSETEVENTPAYMENT_LKPFEEOCCURENCE = "AssetEventPayments.lkpFeeOccurrence";

        /// <summary>
        /// Constant for City.State
        /// </summary>
        public static String TABLE_CITY_STATE = "City.State";

        /// <summary>
        /// Constant for City.State.Countr
        /// </summary>
        public static String TABLE_CITY_STATE_COUNTRY = "City.State.Country";

        /// <summary>
        /// Constant for Documents.
        /// </summary>
        public static String TABLE_DOCUMENTS = "Documents";

        /// <summary>
        /// Constant for City.
        /// </summary>
        public static String TABLE_CITY = "City";

        /// <summary>
        /// Constant for City.
        /// </summary>
        public static String TABLE_COUNTY = "County";

        public static String TABLE_COUNTIES = "Counties";

        /// <summary>
        /// Constant for State.
        /// </summary>
        public static String TABLE_STATE = "State";

        /// <summary>
        /// Constant for AssetNotes.
        /// </summary>
        public static String TABLE_ASSET_NOTES = "AssetNotes";

        /// <summary>
        /// Constant for AssetNotes.Note.
        /// </summary>
        public static String TABLE_ASSET_NOTES_DOT_NOTE = "AssetNotes.Note";

        /// <summary>
        /// Constant for Note.
        /// </summary>
        public static String TABLE_NOTE = "Note";

        /// <summary>
        /// Constant for AssetContacts.
        /// </summary>
        public static String TABLE_ASSET_CONTACTS = "AssetContacts";

        /// <summary>
        /// Constant for AssetContacts.NonContributor
        /// </summary>
        public static String TABLE_ASSET_CONTACTS_DOT_NONCONTRIBUTOR = "AssetContacts.NonContributor";

        /// <summary>
        /// Constant for AssetContacts.NonContributor.Contact
        /// </summary>
        public static String TABLE_ASSET_CONTACTS_DOT_NONCONTRIBUTOR_DOT_CONTACT = "AssetContacts.NonContributor.Contact";

        /// <summary>
        /// Constant for AssetContacts.AssetContactType.
        /// </summary>
        public static String TABLE_ASSET_CONTACTS_DOT_ASSET_CONTACT_TYPE = "AssetContacts.lkpAssetContactType";

        /// <summary>
        /// Constant for AssetContacts.Contact.
        /// </summary>
        public static String TABLE_ASSET_CONTACTS_DOT_CONTACT = "AssetContacts.Contact";

        /// <summary>
        /// Constant for AssetContacts.Contact.ContactDetails.
        /// </summary>
        public static String TABLE_ASSETCONTACTS_DOT_CONTACT_DOT_CONTACT_DETAILS = "AssetContacts.Contact.ContactDetails";

        /// <summary>
        /// Constant for AssetContacts.TimeZoneInfo.
        /// </summary>
        public static String TABLE_ASSET_CONTACTS_DOT_TIMEZONEINFO = "AssetContacts.TimeZoneInfo";

        /// <summary>
        /// Constant for AssetContactType.
        /// </summary>
        public static String TABLE_ASSETCONTACT_TYPE = "lkpAssetContactType";

        /// <summary>
        /// Constant for ContactDetails.
        /// </summary>
        public static String TABLE_CONTACT_DETAILS = "ContactDetails";

        /// <summary>
        /// Constant for TimeZoneInfo.
        /// </summary>
        public static String TABLE_TIME_ZONE_INFO = "TimeZoneInfo";

        /// <summary>
        /// Constant for Asset.
        /// </summary>
        public static String TABLE_ASSET = "Asset";

        /// <summary>
        /// Constant for Bankruptcies.
        /// </summary>
        public static String TABLE_BANK_RUPTCIES = "Bankruptcies";

        /// <summary>
        /// Constant for LockBoxType.
        /// </summary>
        public static String TABLE_LOCK_BOX_TYPE = "LockBoxType";

        /// <summary>
        /// Constant for REODetails.
        /// </summary>
        public static String TABLE_REO_DETAILS = "AssetREODetails";

        /// <summary>
        /// Constant for REODetails.
        /// </summary>
        public static String TABLE_ASSETPROPERTY_DETAILS = "AssetPropertyDetails";

        /// <summary>
        /// Constant for REODetails.
        /// </summary>
        public static String TABLE_ASSETMOBILE_DETAILS = "AssetMobileDetails";

        /// <summary>
        /// Constant for RecurringDetails.
        /// </summary>
        public static String TABLE_RECURRING_DETAILS = "AssetRecurringServices";

        /// <summary>
        /// Constant for AssetDwellings.
        /// </summary>
        public static String TABLE_ASSET_DWELLINGS = "AssetDwellings";

        /// <summary>
        /// Constant for AssetDwellings.DwellingDetails.
        /// </summary>
        public static String TABLE_ASSET_DWELLINGS_DOT_DWELLING_DETAILS = "AssetDwellings.DwellingDetails";

        /// <summary>
        /// Constant for DwellingDetails.
        /// </summary>
        public static String TABLE_DWELLING_DETAILS = "DwellingDetails";

        /// <summary>
        /// Constant for "AssetDwellings.AssetUnitTypes".
        /// </summary>
        public static String TABLE_ASSET_DWELLINGS_DOT_ASSET_UNIT_TYPES = "AssetDwellings.AssetUnitTypes";

        /// <summary>
        /// Constant for AssetDwellings.AssetUnitTypes.AssetConditions
        /// </summary>
        public static String TABLE_ASSET_DWELLINGS_DOT_ASSET_UNIT_TYPES_DOT_ASSET_CONDITIONS = "AssetDwellings.AssetUnitTypes.AssetConditions";

        /// <summary>
        /// Constant for AssetUnitTypes.
        /// </summary>
        public static String TABLE_ASSET_UNIT_TYPES = "AssetUnitTypes";

        /// <summary>
        /// Constant for AssetConditions.
        /// </summary>
        public static String TABLE_ASSET_CONDITIONS = "AssetConditions";

        /// <summary>
        /// Constant for GeoCoding.
        /// </summary>
        public static String TABLE_GEO_CODING = "GeoCoding";

        /// <summary>
        /// Constant for lkpNotesType.
        /// </summary>
        public static String TABLE_LKP_NOTES_TYPE = "lkpNoteType_OBSOLETE";

        /// <summary>
        /// Constant for OrganizationUsers.
        /// </summary>
        public static String TABLE_ORGANIZATION_USER = "OrganizationUser";

        /// <summary>
        /// Constant for OrganizationUsers.
        /// </summary>
        public static String TABLE_LKPCONTEXT = "lkpContext";

        /// <summary>
        /// Constant for Contact.ContactDetails.
        /// </summary>
        public static String TABLE_CONTACT_DOT_CONTACT_DETAILS = "Contact.ContactDetails";

        /// <summary>
        /// Constant for Contact.ContactDetails.
        /// </summary>
        public static String TABLE_ADDRESSHANDLE = "AddressHandle";

        /// <summary>
        /// Constant for Contact.ContactDetails.
        /// </summary>
        public static String TABLE_ADDRESSHANDLE_ADDRESS = "AddressHandle.Addresses";

        /// <summary>
        /// Constant for Contact.ContactDetails.
        /// </summary>
        public static String TABLE_ADDRESSHANDLE_ADDRESS_ZIPCODE = "AddressHandle.Addresses.ZipCode";

        /// <summary>
        /// Constant for Contact.ContactDetails.
        /// </summary>
        public static String TABLE_ADDRESSHANDLE_ADDRESS_ZIPCODE_CITY = "AddressHandle.Addresses.ZipCode.City";

        /// <summary>
        /// 
        /// </summary>
        public static String TABLE_ADDRESSHANDLE_ADDRESS_ZIPCODE_CITY_STATE = "AddressHandle.Addresses.ZipCode.City.State";

        /// <summary>
        /// Constant for lkpReason.
        /// </summary>
        public static String TABLE_LKP_REASON = "lkpReason";

        /// <summary>
        /// Constant for Contact1.ContactDetails.
        /// </summary>
        public static String TABLE_CONTACT1_DOT_CONTACT_DETAILS = "Contact1.ContactDetails";

        /// <summary>
        /// Constant for Contact2.ContactDetails.
        /// </summary>
        public static String TABLE_CONTACT2_DOT_CONTACT_DETAILS = "Contact2.ContactDetails";

        /// <summary>
        /// Constant for ProviderCompanyUtilityTypes
        /// </summary>
        public static String TABLE_PROVIDER_COMPANY_UTILITY_TYPES = "ProviderCompanyUtilityTypes";

        /// <summary>
        /// Constant for UtilityTypeProviders
        /// </summary>
        public static String TABLE_UTILITY_TYPE_PROVIDERS = "UtilityTypeProviders";

        /// <summary>
        /// Constant for ProviderCompanyUtilityTypes.UtilityType
        /// </summary>
        public static String TABLE_PROVIDER_COMPANY_UTILITY_TYPES_DOT_UTILITY_TYPE = "ProviderCompanyUtilityTypes.UtilityType";

        /// <summary>
        /// Constant for lkpTimeZone
        /// </summary>
        public static String TABLE_LKP_TIME_ZONE = "lkpTimeZone";

        /// <summary>
        /// Constant for lkpEmployeeType
        /// </summary>
        public static String TABLE_LKP_EMPLOYEE_TYPE = "lkpEmployeeType";

        /// <summary>
        /// Constant for AssetEventStatus
        /// </summary>
        public static String TABLE_ASSET_EVENT_STATUS = "AssetEventStatus";

        /// <summary>
        /// Constant for AssetEventSteps
        /// </summary>
        public static String TABLE_ASSETEVENTSTEPS_LKPASSETEVENTSTEPTYPE = "AssetEventSteps.lkpAssetEventStepType";

        /// <summary>
        /// Constant for "InvestorInsurerLOBs"
        /// </summary>
        public static String TABLE_INVESTOR_INSURER_LOBS = "InvestorInsurerLOBs";

        /// <summary>
        /// Constant for "ValidatedAddress"
        /// </summary>
        public static String TABLE_VALIDATED_ADDRESS = "ValidatedAddress";

        /// <summary>
        /// Constant for "ValidatedAddress.ZipCode"
        /// </summary>
        public static String TABLE_VALIDATED_ADDRESS_DOT_ZIPCODE = "ValidatedAddress.ZipCode";

        /// <summary>
        /// Constant for ValidatedAddress.ZipCode.City
        /// </summary>
        public static String TABLE_VALIDATED_ADDRESS_DOT_ZIPCODE_DOT_CITY = "ValidatedAddress.ZipCode.City";

        /// <summary>
        /// Constant for ValidatedAddress.ZipCode.City.State
        /// </summary>
        public static String TABLE_VALIDATED_ADDRESS_DOT_ZIPCODE_DOT_STATE = "ValidatedAddress.ZipCode.City.State";

        /// <summary>
        /// Constant for Asset.AssetContact
        /// </summary>
        public static String TABLE_ASSET_DOT_ASSETCONTACT = "AssetContacts";

        /// <summary>
        /// Constant for AssetLoan.AssetStatus
        /// </summary>
        public static String TABLE_ASSETLOAN_DOT_ASSETSTATUS = "AssetLoans.AssetStatus";

        /// <summary>
        /// Constant for InvestorInsurerClients
        /// </summary>
        public static String TABLE_INVESTOR_INSURER_CLIENTS = "InvestorInsurerClients";

        /// <summary>
        /// InvestorInsurerClient
        /// </summary>
        public static String TABLE_INVESTOR_INSURER_CLIENT = "InvestorInsurerClient";

        /// <summary>
        /// InvestorInsurerClient.InvestorInsurer
        /// </summary>
        public static String TABLE_INVESTOR_INSURER_CLIENT_DOT_INVESTORINSURER = "InvestorInsurerClient.InvestorInsurer";

        /// <summary>
        /// Constant for InvestorInsurerClients.Client
        /// </summary>
        public static String TABLE_INVESTOR_INSURER_CLIENTS_DOT_CLIENT = "InvestorInsurerClients.Client";

        /// <summary>
        /// Const for  InvestorInsurerContextStatus
        /// </summary>
        public static String TABLE_INVESTOR_INSURER_CONTEXT_STATUS = "InvestorInsurerContextStatus";

        /// <summary>
        /// Constant for InvestorInsurerClient.Client
        /// </summary>
        public static String TABLE_INVESTOR_INSURER_CLIENT_DOT_CLIENT = "InvestorInsurerClient.Client";

        /// <summary>
        /// Constant for lkpDispositionReason
        /// </summary>
        public static String TABLE_LKP_DISPOSITION_REASON = "lkpDispositionReason";

        /// <summary>
        /// Constant for SysXWorkflow
        /// </summary>
        public static String TABLE_SYSX_WORKFLOW = "SysXWorkflow";

        /// <summary>
        /// Constant for lkpInvestorInsurerStatu
        /// </summary>
        public static String TABLE_LKP_INVESTOR_INSURER_STATU = "lkpInvestorInsurerStatu";

        /// <summary>
        /// Constant for InvestorInsurerStatu
        /// </summary>
        public static String TABLE_INVESTOR_INSURER_STATU = "InvestorInsurerStatu";

        /// <summary>
        /// Constant for InvestorInsurer
        /// </summary>
        public static String TABLE_INVESTOR_INSURER = "InvestorInsurer";

        /// <summary>
        /// Constant for NonContributor
        /// </summary>
        public static String TABLE_NON_CONTRIBUTOR = "NonContributor";

        /// <summary>
        /// Constant for NonContributor.ValidatedAddress
        /// </summary>
        public static String TABLE_NON_CONTRIBUTOR_DOT_VALIDATED_ADDRESS = "NonContributor.AddressHandle.Addresses";

        /// <summary>
        /// Constant for NonContributor.ValidatedAddress.ZipCode
        /// </summary>
        public static String TABLE_NON_CONTRIBUTOR_DOT_VALIDATED_ADDRESS_DOT_ZIPCODE = "NonContributor.AddressHandle.Addresses.ZipCode";

        /// <summary>
        /// Constant for NonContributor.ValidatedAddress.ZipCode.State
        /// </summary>
        public static String TABLE_NON_CONTRIBUTOR_DOT_VALIDATED_ADDRESS_DOT_ZIPCODE_DOT_STATE = "NonContributor.AddressHandle.Addresses.ZipCode.City.State";

        /// <summary>
        /// Constant for NonContributor.ValidatedAddress.ZipCode.City
        /// </summary>
        public static String TABLE_NON_CONTRIBUTOR_DOT_VALIDATED_ADDRESS_DOT_ZIPCODE_DOT_CITY = "NonContributor.AddressHandle.Addresses.ZipCode.City";

        /// <summary>
        /// Constant for NonContributor.Contact
        /// </summary>
        public static String TABLE_NON_CONTRIBUTOR_DOT_CONTACT = "NonContributor.Contact";

        /// <summary>
        /// Constant for NonContributor.Contact.ContactDetails
        /// </summary>
        public static String TABLE_NON_CONTRIBUTOR_DOT_CONTACT_DOT_CONTACT_DETAILS = "NonContributor.Contact.ContactDetails";

        /// <summary>
        /// Constant for InvestorInsurerContact
        /// </summary>
        public static String TABLE_INVESTOR_INSURER_CONTACT = "InvestorInsurerContact";

        /// <summary>
        /// Constant for MessageDetail
        /// </summary>
        public static String TABLE_MESSAGE_DETAIL = "MessageDetail";

        /// <summary>
        /// Constant for MessageFolder
        /// </summary>
        public static String TABLE_MESSAGE_FOLDER = "MessageFolder";

        /// <summary>
        /// Constant for MessageDetail.aspnet_Users
        /// </summary>
        public static String TABLE_MESSAGE_DETAIL_DOT_ASPNET_USERS = "MessageDetail.aspnet_Users";

        /// <summary>
        /// Constant for MessageDetail.aspnet_Users.aspnet_Membership
        /// </summary>
        public static String TABLE_MESSAGE_DETAIL_DOT_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP = "MessageDetail.aspnet_Users.aspnet_Membership";

        /// <summary>
        /// Constant for aspnet_Users.aspnet_Membership
        /// </summary>
        public static String TABLE_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP = "aspnet_Users.aspnet_Membership";

        /// <summary>
        /// Constant for aspnet_Membership
        /// </summary>
        public static String TABLE_ASPNET_MEMBERSHIP = "aspnet_Membership";

        /// <summary>
        /// Constant for "aspnet_Users"
        /// </summary>
        public static String TABLE_ASPNET_USERS = "aspnet_Users";

        /// <summary>
        /// Constant for "aspnet_Users"
        /// </summary>
        public static String TABLE_MESSAGE_TO_LISTS = "MessageToLists";

        /// <summary>
        /// Constant for MessageToLists.MessageFolder
        /// </summary>
        public static String TABLE_MESSAGE_TO_LISTS_DOT_MESSAGE_FOLDER = "MessageToLists.MessageFolder";

        /// <summary>
        /// Constant for MessageToLists.aspnet_Users.aspnet_Membership
        /// </summary>
        public static String TABLE_MESSAGE_TO_LISTS_DOT_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP = "MessageToLists.aspnet_Users.aspnet_Membership";

        /// <summary>
        /// Constant for Message
        /// </summary>
        public static String TABLE_MESSAGE = "Message";

        /// <summary>
        /// Constant for MessageUser
        /// </summary>
        public static String TABLE_MESSAGE_USER = "MessageUser";

        /// <summary>
        /// Constant for MessageDetails
        /// </summary>
        public static String TABLE_MESSAGE_DETAILS = "MessageDetails";

        /// <summary>
        /// Constant for MessageStatu
        /// </summary>
        public static String TABLE_MESSAGE_STATU = "MessageStatu";

        /// <summary>
        /// Constant for MessageState
        /// </summary>
        public static String TABLE_MESSAGE_STATE = "MessageState";

        /// <summary>
        /// Constant for MessageGroups
        /// </summary>
        public static String TABLE_MESSAGE_GROUPS = "MessageGroups";

        /// <summary>
        /// Constant for MessageGroupUserDetail
        /// </summary>
        public static String TABLE_MESSAGE_GROUP_USER_DETAIL = "MessageGroupUserDetail";

        /// <summary>
        /// Constant for RoleDetail
        /// </summary>
        public static String TABLE_ROLE_DETAIL = "RoleDetail";

        /// <summary>
        /// Constant for LkpEmpoyeeTitle
        /// </summary>
        public static String TABLE_LKP_EMPLOYEE_TITLE = "lkpEmployeeTitle";
     
        /// <summary>
        /// Constant for ProductFeature
        /// </summary>
        public static String TABLE_PRODUCT_FEATURE = "ProductFeature";

        /// <summary>
        /// Constant for TenantProduct.Tenant
        /// </summary>
        public static String TABLE_TENANT_PRODUCT_DOT_TENANT = "TenantProduct.Tenant";

        /// <summary>
        /// Constant for Organization
        /// </summary>
        public static String TABLE_ORGANIZATION = "Organization";

        /// <summary>
        /// Constant for PolicyRegisterUserControl2
        /// </summary>
        public static String TABLE_POLICY_REGISTER_USER_CONTROL2 = "PolicyRegisterUserControl2";

        /// <summary>
        /// Constant for Organization Tenant
        /// </summary>
        public static String TABLE_ORGANIZATION_TENANT = "Organization.Tenant";

        /// <summary>
        /// Constant for Organization with Tenant and lkpTenenatType
        /// </summary>
        public static String TABLE_ORGANIZATION_TENANT_LKPTENANTTYPE = "Organization.Tenant.lkpTenantType";

        /// <summary>
        /// Constant for Organization with Tenant and lkpTenenatType
        /// </summary>
        public static String TABLE_TENANT_LKPTENANTTYPE = "Tenant.lkpTenantType";

        /// <summary>
        /// Constant for PolicySetUserControls
        /// </summary>
        public static String TABLE_POLICY_SET_USER_CONTROLS = "PolicySetUserControls";


        /// <summary>
        /// Constant for PolicySet
        /// </summary>
        public static String TABLE_POLICY_SET = "PolicySet";

        /// <summary>
        /// Constant for Policies
        /// </summary>
        public static String TABLE_POLICIES = "Policies";

        /// <summary>
        /// Constant for Policies.PolicyProperties
        /// </summary>
        public static String TABLE_POLICIES_DOT_POLICY_PROPERTIES = "Policies.PolicyProperties";

        /// <summary>
        /// Constant for PolicyRegisterUserControl
        /// </summary>
        public static String TABLE_POLICY_REGISTER_USER_CONTROL = "PolicyRegisterUserControl";

        /// <summary>
        /// Constant for SysXBlocksFeature
        /// </summary>
        public static String TABLE_SYSX_BLOCKS_FEATURE = "SysXBlocksFeature";

        /// <summary>
        /// Constant for SysXBlocksFeature.SysXBlock
        /// </summary>
        public static String TABLE_SYSX_BLOCKS_FEATURE_DOT_SYSXBLOCK = "SysXBlocksFeature.lkpSysXBlock";

        /// <summary>
        /// Constant for SysXBlocksFeature.ProductFeature
        /// </summary>
        public static String TABLE_SYSX_BLOCKS_FEATURE_DOT_PRODUCT_FEATURE = "SysXBlocksFeature.ProductFeature";

        /// <summary>
        /// Constant for PermissionType
        /// </summary>
        public static String TABLE_PERMISSION_TYPE = "PermissionType";

        /// <summary>
        /// Constant for Permission
        /// </summary>
        public static String TABLE_PERMISSION = "Permission";

        /// <summary>
        /// Constant for ProductFeature2
        /// </summary>
        public static String TABLE_PRODUCT_FEATURE2 = "ProductFeature2";

        /// <summary>
        /// Constant for FeaturePermissions
        /// </summary>
        public static String TABLE_FEATUREPERMISSIONS = "FeaturePermissions";

        /// <summary>
        /// Constant for "FeaturePermissions.Permission"
        /// </summary>
        public static String TABLE_FEATUREPERMISSIONS_DOT_PERMISSION = "FeaturePermissions.Permission";

        /// <summary>
        /// Constant for TenantProducts
        /// </summary>
        public static String TABLE_TENANT_PRODUCTS = "TenantProducts";

        /// <summary>
        /// Constant for Organizations
        /// </summary>
        public static String TABLE_ORGANIZATIONS = "Organizations";

        /// <summary>
        /// Constant for AddressHandle.Addresses.ZipCode.City.State
        /// </summary>
        public static String TABLE_ADDRESSHANDLE_ADDRESSES_ZIPCODE_CITY_STATE = "AddressHandle.Addresses.ZipCode.City.State";

        /// <summary>
        /// Constant for Organizations.AddressHandle.Addresses.ZipCode.City.State
        /// </summary>
        public static String TABLE_ORGANIZATIONS_ADDRESSHANDLE_ADDRESSES_ZIPCODE_CITY_STATE = "Organizations.AddressHandle.Addresses.ZipCode.City.State";

        /// <summary>
        /// Constant for TenantProducts.RoleDetails
        /// </summary>
        public static String TABLE_TENANT_PRODUCTS_DOT_ROLE_DETAILS = "TenantProducts.RoleDetails";

        /// <summary>
        /// Constant for RoleDetails.OrganizationUser
        /// </summary>
        public static String TABLE_TENANT_ROLEDETAILS_DOT_ORGANIZATIONUSER = "RoleDetails.OrganizationUser";

        /// <summary>
        /// Constant for SysXBlocksFeature.ProductFeature.ProductFeature2
        /// </summary>
        public static String TABLE_SYSXBLOCKS_FEATURE_DOT_PRODUCT_FEATURE_DOT_PRODUCT_FEATURE2 = "SysXBlocksFeature.ProductFeature.ProductFeature2";

        /// <summary>
        /// Constant for RoleDetails
        /// </summary>
        public static String TABLE_ROLE_DETAILS = "RoleDetails";

        /// <summary>
        /// Constant for RoleDetails
        /// </summary>
        public static String TABLE_TENANT_PRODUCT_FEATURES = "TenantProductFeatures";

        /// <summary>
        /// Constant for Tenant
        /// </summary>
        public static String TABLE_TENANT = "Tenant";

        /// <summary>
        /// Constant for OrganizationUsers
        /// </summary>
        public static String TABLE_ORGANIZATION_USERS = "OrganizationUsers";

        /// <summary>
        /// Constant for OrganizationUsers.aspnet_Users
        /// </summary>
        public static String TABLE_ORGANIZATION_USERS_DOT_ASPNET_USERS = "OrganizationUsers.aspnet_Users";

        /// <summary>
        /// Constant for OrganizationUsers.aspnet_Users
        /// </summary>
        public static String TABLE_ORGANIZATION_USER_DOT_ASPNET_USER = "OrganizationUser.aspnet_User";

        /// <summary>
        /// Constant for SysXBlocksFeatures
        /// </summary>
        public static String TABLE_SYSX_BLOCKS_FEATURES = "SysXBlocksFeatures";

        /// <summary>
        /// Constant for TenantProduct
        /// </summary>
        public static String TABLE_TENANT_PRODUCT = "TenantProduct";

        /// <summary>
        /// Constant for aspnet_Roles
        /// </summary>
        public static String TABLE_ASPNET_ROLES = "aspnet_Roles";

        /// <summary>
        /// Constant for aspnet_Roles.RoleDetail
        /// </summary>
        public static String TABLE_ASPNET_ROLES_ROLEDETAIL = "aspnet_Roles.RoleDetail";

        /// <summary>
        /// Constant for SysXBlock
        /// </summary>
        public static String TABLE_SYSX_BLOCK = "lkpSysXBlock";

        /// <summary>
        /// Constant for ProductFeature.ProductFeature2
        /// </summary>
        public static String TABLE_PRODUCT_FEATURE_DOT_PRODUCT_FEATURE2 = "ProductFeature.ProductFeature2";

        /// <summary>
        /// Constant for PolicyControlPropertyTypes
        /// </summary>
        public static String TABLE_POLICY_CONTROL_PROPERTY_TYPES = "PolicyControlPropertyTypes";

        /// <summary>
        /// Constant for PolicyControlPropertyTypes.PolicyPropertyType
        /// </summary>
        public static String TABLE_POLICY_CONTROL_PROPERTY_TYPES_DOT_POLICY_PROPERTY_TYPE = "PolicyControlPropertyTypes.PolicyPropertyType";

        /// <summary>
        /// Constant for lkpSuppWorkType
        /// </summary>
        public static String TABLE_LKP_SERVICE_TYPE = "lkpServiceType";

        /// <summary>
        /// Constant for Supplier Dashboard LKPSERVICEREQUESTTYPE
        /// </summary>
        public static String TABLE_LKP_SERVICE_DOT_SERVICE_STATUS_TYPE = "ServiceOrders.lkpServiceStatusType";

        /// <summary>
        /// Constant for Supplier Dashboard LKPSERVICEREQUESTTYPE
        /// </summary>
        public static String TABLE_LKP_SERVICE_REQUEST_TYPE = "lkpServiceRequestType";

        /// <summary>
        /// Constant for Supplier Dashboard SERVICEREQUEST_SYSXBLOCK
        /// </summary>
        public static String TABLE_SERVICEREQUEST_SYSXBLOCK = "ServiceRequest.lkpSysXBlock";

        /// <summary>
        /// Constant for Supplier Dashboard Service order results
        /// </summary>
        public static String TABLE_SERVICEORDER_SERVICEORDERRESULTS = "ServiceOrders.ServiceOrderResults";

        /// <summary>
        /// Constant for ServiceOrderResult.ServiceOrder
        /// </summary>
        public static String TABLE_SERVICEORDERRESULT_SERVICEORDER = "ServiceOrderResult.ServiceOrder";

        /// <summary>
        /// Constant for ServiceOrderResult.ServiceOrder.Supplier.SupplierRating
        /// </summary>
        public static String TABLE_SERVICEORDERRESULT_SERVICEORDER_SUPPLIER_LKPSUPPLIERRATING = "ServiceOrderResult.ServiceOrder.Supplier.SupplierRating";

        /// <summary>
        /// Constant for ServiceOrderResults.ServiceItemResults.lkpServiceItem
        /// </summary>
        public static String TABLE_SERVICEORDERRESULT_SERVICEITEMRESULT_LKPSERVICEITEM = "ServiceOrderResults.ServiceItemResults.lkpServiceItem";


        /// <summary>
        /// Constant for  ServiceOrderResults.Asset
        /// </summary>
        public static String TABLE_SERVICEORDERRESULTS_ASSET = "ServiceOrderResults.Asset";

        /// <summary>
        /// Constant for  ServiceOrderResults.Asset.ValidatedAddress
        /// </summary>
        public static String TABLE_SERVICEORDERRESULTS_ASSET_VALIDATEADDRESS = "ServiceOrderResults.Asset.ValidatedAddress";

        /// <summary>
        /// Constant for  ServiceOrderResult.ServiceOrder.ServiceRequest
        /// </summary>
        public static String TABLE_SERVICEORDERRESULT_SERVICEORDER_SERVICEREQUEST = " ServiceOrderResult.ServiceOrder.ServiceRequest";

        /// <summary>
        /// Constant for Supplier Dashboard Service order results
        /// </summary>
        public static String TABLE_SERVICEORDERRESULTS = "ServiceOrderResults";

        /// <summary>
        /// Constant for ServiceOrderResult Table
        /// </summary>
        public static String TABLE_SERVICEORDERRESULT = "ServiceOrderResult";

        /// <summary>
        /// Constant for Supplier Dashboard Service order
        /// </summary>
        public static String TABLE_SERVICEORDER = "ServiceOrder";

        /// <summary>
        /// ServiceRequest.lkpServiceRequestType
        /// </summary>
        public static String TABLE_LKP_SERVICE_REQUEST_REQUEST_TYPE = "ServiceRequest.lkpServiceRequestType";

        /// <summary>
        /// ServiceOrderActivities
        /// </summary>
        public static String TABLE_LKP_SERVICE_REQUEST_SERVICEORDERACTIVITIES = "ServiceOrderActivities";

        /// <summary>
        /// ServiceOrder.ServiceRequest.AssetLoan
        /// </summary>
        public static String TABLE_SERVICEORDER_SERVICEREQUEST_ASSETLOAN = "ServiceOrder.ServiceRequest.AssetLoan";


        /// <summary>
        /// ServiceOrderActivities.lkpActivityType
        /// </summary>
        public static String TABLE_LKP_SERVICE_REQUEST_SERVICEORDERACTIVITIES_DOT_LKPACTIVITYTYPE = "ServiceOrderActivities.lkpActivityType";

        /// <summary>
        /// ServiceOrderActivities.ServiceOrderActivityResults
        /// </summary>
        public static String TABLE_LKP_SERVICE_REQUEST_SERVICEORDERACTIVITIES_DOT_SERVICEORDERACTIVITYRESULTS = "ServiceOrderActivities.ServiceOrderActivityResults";

        /// <summary>
        /// Constant for lkpInsuranceType
        /// </summary>
        public static String TABLE_LKP_SUPP_INSURANCE_TYPE = "lkpInsuranceType";

        /// <summary>
        /// Constant for Document
        /// </summary>
        public static String TABLE_DOCUMENT = "Document";

        /// <summary>
        /// Constant for SupplierRating
        /// </summary>
        public static String TABLE_LKP_SUPPLIER_RATING = "SupplierRating";

        /// <summary>
        /// Constant for lkpSupplierStatu
        /// </summary>
        public static String TABLE_LKP_SUPPLIER_STATU = "lkpSupplierStatu";

        /// <summary>
        /// Constant for SupplierExclusions
        /// </summary>
        public static String TABLE_SUPPLIER_EXCLUSIONS = "SupplierExclusions";

        /// <summary>
        /// Constant for SupplierInsurances
        /// </summary>
        public static String TABLE_SUPPLIER_INSURANCES = "SupplierInsurances";

        /// <summary>
        /// Constant for SupplierLicenses
        /// </summary>
        public static String TABLE_SUPPLIER_LICENSES = "SupplierLicenses";

        /// <summary>
        /// Constant for SupplierRelations
        /// </summary>
        public static String TABLE_SUPPLIER_RELATIONS = "SupplierRelations";

        /// <summary>
        /// Constant for SupplierServiceAreas
        /// </summary>
        public static String TABLE_SUPPLIER_SERVICE_AREAS = "SupplierServiceAreas";

        /// <summary>
        /// Constant for SupplierSpecialityServices
        /// </summary>
        public static String TABLE_SUPPLIER_SPECIALITY_SERVICES = "SupplierSpecialityServices";

        /// <summary>
        /// Constant for SupplierWorkTypes
        /// </summary>
        public static String TABLE_SUPPLIER_WORK_TYPES = "SupplierWorkTypes";

        /// <summary>
        /// Constant for SupplierRatings
        /// </summary>
        public static String TABLE_SUPPLIER_RATINGS = "SupplierRatings";

        /// <summary>
        /// Constant for SupplierRelations1
        /// </summary>
        public static String TABLE_SUPPLIER_RELATIONS1 = "SupplierRelations1";

        /// <summary>
        /// Constant for SupplierActionPlans
        /// </summary>
        public static String TABLE_SUPPLIER_ACTION_PLANS = "SupplierActionPlans";

        /// <summary>
        /// Constant for ItemCategoryActions
        /// </summary>
        public static String TABLE_ITEM_CATEGORY_ACTIONS = "ItemCategoryActions";

        /// <summary>
        /// Constant for TABLE_SYSXBLOCKS.
        /// </summary>
        public static String TABLE_SYSXBLOCKS = "lkpSysXBlocks";

        /// <summary>
        /// Constant for Employee2 (Employee Master managername)
        /// </summary>
        public static String TABLE_EMPLOYEE = "Employee2";

        /// <summary>
        /// Constant for OrganizationUser.aspnet_Users
        /// </summary>
        public static String TABLE_ORGANIZATIONUSER_DOT_ASPNET_USERS = "OrganizationUser.aspnet_Users";

        /// <summary>
        /// Constant for OrganizationUser.aspnet_Users.aspnet_Membership
        /// </summary>
        public static String TABLE_ORGANIZATIONUSER_DOT_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP = "OrganizationUser.aspnet_Users.aspnet_Membership";

        /// <summary>
        /// Constant for OrganizationUser.Organization
        /// </summary>
        public static String TABLE_ORGANIZATIONUSER_DOT_ORGANIZATION = "OrganizationUser.Organization";

        /// <summary>
        /// Constant for Organizations Contact
        /// </summary>
        public static String ORGANIZATIONS_DOT_CONTACT = "Organizations.Contact";

        /// <summary>
        /// Constant for Organizations Contact ContactDetails
        /// </summary>
        public static String ORGANIZATIONS_DOT_CONTACT_DOT_CONTACTDETAILS = "Organizations.Contact.ContactDetails";

        /// <summary>
        /// Constant for Organizations ValidatedAddress
        /// </summary>
        public static String ORGANIZATIONS_DOT_VALIDATEDADDRESS = "Organizations.AddressHandle.Addresses.ZipCode.City.State";

        /// <summary>
        /// Constant for Organizations ValidatedAddress ZipCode
        /// </summary>
        public static String ORGANIZATIONS_DOT_VALIDATEDADDRESS_DOT_ZIPCODE = "Organizations.ValidatedAddress.ZipCode";

        /// <summary>
        /// Constant for lkpTenantType
        /// </summary>
        public static String LKPTENANTTYPE = "lkpTenantType";

        /// <summary>
        /// Constant for Organizations ValidatedAddress ZipCode City
        /// </summary>
        public static String ORGANIZATIONS_DOT_VALIDATEDADDRESS_DOT_ZIPCODE_DOT_CITY = "Organizations.ValidatedAddress.ZipCode.City";

        /// <summary>
        /// Constant for Organizations ValidatedAddress ZipCode City State
        /// </summary>
        public static String ORGANIZATIONS_DOT_VALIDATEDADDRESS_DOT_ZIPCODE_DOT_CITY_DOT_STATE = "Organizations.ValidatedAddress.ZipCode.City.State";

        /// <summary>
        /// Constant for Contact ContactDetails
        /// </summary>
        public static String CONTACT_DOT_CONTACTDETAILS = "Contact.ContactDetails";

        /// <summary>
        /// Constant for Role Permission Product Features
        /// </summary>
        public static String ROLE_PERMISSION_PRODUCT_FEATURES = "RolePermissionProductFeatures";

        /// <summary>
        /// Constant for LoanContacts.
        /// </summary>
        public static String TABLE_LOAN_CONTACTS = "LoanContacts";

        /// <summary>
        /// Constant for LoanContacts.
        /// </summary>
        public static String TABLE_LOAN_CONTACTS_DOT_NONCONTRIBUTOR = "LoanContacts.NonContributor";

        /// <summary>
        /// Constant for LoanContacts.
        /// </summary>
        public static String TABLE_LOAN_CONTACTS_DOT_NONCONTRIBUTOR_DOT_CONTACT = "LoanContacts.NonContributor.Contact";

        /// <summary>
        /// Constant for LoanContacts.Contact.
        /// </summary>
        public static String TABLE_LOAN_CONTACTS_DOT_CONTACT = "LoanContacts.Contact";

        /// <summary>
        /// Constant for LoanStatus.
        /// </summary>
        public static String TABLE_LOAN_STATUS = "LoanStatus";

        /// <summary>
        /// Constant for lkpLoanType.
        /// </summary>
        public static String TABLE_LKP_LOAN_TYPE = "lkpLoanType";

        /// <summary>
        /// Constant for lkpClientDefaultStatusType.
        /// </summary>
        public static String TABLE_LKP_LKPCLIENTDEFAULTSTATUSTYPE = "lkpClientDefaultStatusType";

        /// <summary>
        /// Constant for Client.IntakeRequests.
        /// </summary>
        public static String TABLE_CLIENT_DOT_INTAKE_REQUEST = "Client.IntakeRequests";

        /// <summary>
        /// Constant for ValidatedAddress.LegalAddress
        /// </summary>
        public static String TABLE_VALIDATED_ADDRESS_DOT_LEGAL_ADDRESS = "ValidatedAddress.LegalAddress";

        /// <summary>
        /// Constant for LegalAddress
        /// </summary>
        public static String TABLE_LEGAL_ADDRESS = "LegalAddress";

        /// <summary>
        /// Constant for ValidatedAddress.LegalDescription
        /// </summary>
        public static String TABLE_VALIDATED_ADDRESS_DOT_LEGAL_DESCRIPTION = "ValidatedAddress.LegalDescription";

        /// <summary>
        /// Constant for ValidatedAddress.AddressHistories
        /// </summary>
        public static String TABLE_VALIDATED_ADDRESS_DOT_ADDRESS_HISTORIES = "ValidatedAddress.AddressHistories";

        /// <summary>
        /// Constant for AssetLoans.Loan.Client.IntakeRequests
        /// </summary>
        public static String TABLE_ASSET_LOANS_DOT_LOAN_DOT_CLIENT_INTAKEREQUESTS = "AssetLoans.Loan.Client.IntakeRequests";

        /// <summary>
        /// Constant for LegalDescription
        /// </summary>
        public static String TABLE_LEGAL_DESCRIPTION = "LegalDescription";

        /// <summary>
        /// Constant for ValidatedAddress.GeoCoding
        /// </summary>
        public static String TABLE_VALIDATED_ADDRESS_DOT_GEOCODING = "ValidatedAddress.GeoCoding";

        /// <summary>
        /// Constant for AssetUnits
        /// </summary>
        public static String TABLE_ASSET_UNITS = "AssetUnits";

        /// <summary>
        /// Constant for AssetLoans
        /// </summary>
        public static String TABLE_ASSET_LOANS = "AssetLoans";

        /// <summary>
        /// Constant for AssetLoans.Loan
        /// </summary>
        public static String TABLE_ASSET_LOANS_DOT_LOAN = "AssetLoans.Loan";

        /// <summary>
        /// Constant for AssetLoans.Loan
        /// </summary>
        public static String TABLE_ASSET_LOANS_DOT_LOAN_INVESTORINSURER = "AssetLoans.Loan.InvestorInsurer";

        /// <summary>
        /// Constant for AssetLoans.Loan.Client
        /// </summary>
        public static String TABLE_ASSET_LOANS_DOT_LOAN_DOT_CLIENT = "AssetLoans.Loan.Client";

        /// <summary>
        /// Constant for AssetLoans.Loan.ClientLSCI
        /// </summary>
        public static String TABLE_ASSET_LOANS_DOT_LOAN_DOT_CLIENTLSCI = "AssetLoans.Loan.ClientLSCI";

        /// <summary>
        /// Constant for ClientLSCI
        /// </summary>
        public static String TABLE_CLIENTLSCI = "ClientLSCI";

        /// <summary>
        /// Constant for Loan.Client
        /// </summary>
        public static String TABLE_LOAN_DOT_CLIENT = "Loan.Client";

        /// <summary>
        /// Constant for AssetLoans.Loan.lkpLoanType
        /// </summary>
        public static String TABLE_ASSET_LOANS_DOT_LOAN_DOT_LKP_LOAN_TYPE = "AssetLoans.Loan.lkpLoanType";

        /// <summary>
        /// Constant for AssetLoans.Loan.LoanStatus
        /// </summary>
        public static String TABLE_ASSET_LOANS_DOT_LOAN_LOAN_STATUS = "AssetLoans.Loan.LoanStatus";

        /// <summary>
        /// Constant for AssetLoans.Loan.Liens
        /// </summary>
        public static String TABLE_ASSET_LOANS_DOT_LOAN_DOT_LIENS = "AssetLoans.Loan.Liens";

        /// <summary>
        /// Constant for Asset.AssetLoans.Loan.Liens
        /// </summary>
        public static String TABLE_ASSET_ASSETLOANS_DOT_LOAN_DOT_LIENS = "Asset.AssetLoans.Loan.Liens";

        /// <summary>
        /// Constant for Asset.AssetLoans.Loan.Client
        /// </summary>
        public static String TABLE_ASSET_ASSETLOANS_DOT_LOAN_DOT_CLIENT = "Asset.AssetLoans.Loan.Client";

        /// <summary>
        /// Constant for AssetLoans.Loan.LoanContacts
        /// </summary>
        public static String TABLE_ASSET_LOANS_DOT_LOAN_DOT_LOAN_CONTACTS = "AssetLoans.Loan.LoanContacts";

        /// <summary>
        /// Constant for AssetLoans.Loan.LoanContacts.NonContributor.Contact
        /// </summary>
        public static String TABLE_ASSET_LOANS_DOT_LOAN_DOT_LOAN_CONTACTS_DOT_NONCONTRIBUTOR_DOT_CONTACT = "AssetLoans.Loan.LoanContacts.NonContributor.Contact";

        /// <summary>
        /// Constant for AssetLoans.Loan.LoanContacts.NonContributor
        /// </summary>
        public static String TABLE_ASSET_LOANS_DOT_LOAN_DOT_LOAN_CONTACTS_DOT_NONCONTRIBUTOR = "AssetLoans.Loan.LoanContacts.NonContributor";

        /// <summary>
        /// Constant for Service.lkpServiceType
        /// </summary>
        public static String TABLE_SERVICE_DOT_LKP_SERVICE_TYPE = "ServiceOrder.lkpServiceType";

        /// <summary>
        /// Constant for ServiceOrder.lkpServiceType.serviceElement
        /// </summary>
        public static String TABLE_SERVICE_DOT_LKP_SERVICE_TYPE_DOT_SVC_ELEMENT = "ServiceOrders.lkpServiceType.ServiceElements";

        /// <summary>
        /// Constant for ServiceOrder.lkpServiceType.serviceElement.lkpServiceItem
        /// </summary>
        public static String TABLE_SERVICE_DOT_LKP_SERVICE_TYPE_DOT_SVC_ELEMENT_DOT_LKPSERVICEITEM = "ServiceOrders.lkpServiceType.ServiceElements.lkpServiceItem";

        /// <summary>
        /// Constant for ServiceOrder.lkpServiceType.serviceElement.lkpServiceItem.lkpItem
        /// </summary>
        public static String TABLE_SERVICE_DOT_LKP_SERVICE_TYPE_DOT_SVC_ELEMENT_DOT_LKPSERVICEITEM_DOT_LKPITEM = "ServiceOrders.lkpServiceType.ServiceElements.lkpServiceItem.lkpItem";

        /// <summary>
        /// Constant for ServiceOrder.lkpServiceType.serviceElement.lkpServiceItem.lkpItemCategory
        /// </summary>
        public static String TABLE_SVC_DOT_LKPSVCTYPE_DOT_SVCELEMENT_DOT_LKPSERVICEITEM_DOT_LKPITEMCATEGORY = "ServiceOrders.lkpServiceType.ServiceElements.lkpServiceItem.lkpItemCategory";

        /// <summary>
        /// Constant for ServiceOrder.lkpServiceType.serviceElement.lkpServiceItem.lkpItemCategory.lkpItemSubCategory.
        /// </summary>
        public static String TABLE_SVC_DOT_LKPSVCTYPE_DOT_SVCELEMENT_DOT_LKPSVCEITEM_DOT_LKPITEMCATEGORY_LKPITEMSUBCATEGORY = "ServiceOrders.lkpServiceType.ServiceElements.lkpServiceItem.lkpItemCategory.lkpItemSubCategories";

        /// <summary>
        /// Constant for ServiceItem
        /// </summary>
        public static String TABLE_SERVICE_ITEM = "lkpServiceItem";

        /// <summary>
        /// Constant for lkpItemAction
        /// </summary>
        public static String TABLE_LKP_ITEM_ACTION = "lkpItemAction";

        /// <summary>
        /// Constant for ServiceRequest
        /// </summary>
        public static String TABLE_SERVICE_REQUEST = "ServiceRequest";

        /// <summary>
        /// Constant for "AssetDamageBidGroups.AssetDamage.ACFAssetDamages"
        /// </summary>
        public static String TABLE_ASSET_DAMAGE_BIDGROUP_ASSETDAMAGE_ACFASSETDAMAGES = "AssetDamageBidGroups.AssetDamage.ACFAssetDamages";


        /// <summary>
        /// Constant for AssetLoan
        /// </summary>
        public static String TABLE_ASSET_LOAN = "AssetLoan";

        /// <summary>
        /// Constant for AssetLoan
        /// </summary>
        public static String TABLE_ASSET_LOAN_DOT_LOAN = "AssetLoan.Loan";

        /// <summary>
        /// Constant for AssetLoan.Asset
        /// </summary>
        public static String TABLE_ASSET_LOAN_DOT_ASSET = "AssetLoan.Asset";

        /// <summary>
        /// Constant for AssetLoan.Asset.ValidatedAddress
        /// </summary>
        public static String TABLE_ASSET_LOAN_DOT_ASSET_DOT_VALIDATEDADDRESS = "AssetLoan.Asset.ValidatedAddress";

        /// <summary>
        /// Constant for Asset.ValidatedAddres
        /// </summary>
        public static String TABLE_ASSET_DOT_VALIDATEDADDRESS = "Asset.ValidatedAddress";

        /// <summary>
        /// Constant for AssetLoan.Asset.AssetUnits
        /// </summary>
        public static String TABLE_ASSET_LOAN_DOT_ASSET_ASSETUNITS = "AssetLoan.Asset.AssetUnits";

        /// <summary>
        /// Constant for ACF.ServiceOrders.ServiceRequest.AssetLoan.Loan.Client
        /// </summary>
        public static String TABLE_ACF_DOT_SERVICES_DOT_SERVICEREQUEST_DOT_ASSETLOAN_DOT_LOAN_CLIENT = "ACF.ServiceOrders.ServiceRequest.AssetLoan.Loan.Client";

        /// <summary>
        /// Constant for lkpDamageProcessStatu
        /// </summary>
        public static String TABLE_LKPDAMAGE_PROCESS_STATUS = "lkpDamageProcessStatu";

        /// <summary>
        /// Constant for ACF.ServiceOrders.Supplier
        /// </summary>
        public static String TABLE_ACF_DOT_SERVICES_DOT_SUPPLIER = "ACF.ServiceOrders.Supplier";

        /// <summary>
        /// Constant for Supplier
        /// </summary>
        public static String TABLE_SUPPLIER = "Supplier";

        /// <summary>
        /// Constant for Supplier.lkpSupplierStatu
        /// </summary>
        public static String TABLE_SUPPLIER_DOT_LKPSUPPLIERSTATU = "Supplier.lkpSupplierStatu";

        /// <summary>
        /// Constant for Supplier.SupplierRating
        /// </summary>
        public static String TABLE_SUPPLIER_DOT_SUPPLIERRATING = "Supplier.SupplierRating";

        /// <summary>
        /// Constant for Supplier.SupplierServiceAreas
        /// </summary>
        public static String TABLE_SUPPLIER_DOT_SUPPLIERSERVICEAREAS = "Supplier.SupplierServiceAreas";

        /// <summary>
        /// Constant for SupplierServiceAreas
        /// </summary>
        public static String TABLE_SUPPLIERSERVICEAREAS = "SupplierServiceAreas";

        /// <summary>
        /// Constant for BidGroups.BidItems
        /// </summary>
        public static String TABLE_BIDGROUPS_DOT_BIDITEMS = "BidGroups.BidItems";

        /// <summary>
        /// Constant for lkpDamageLocation
        /// </summary>
        public static String TABLE_LKPDAMAGE_LOCATION = "lkpDamageLocation";

        /// <summary>
        /// Constant for ACF
        /// </summary>
        public static String TABLE_ACF = "ACF";

        /// <summary>
        /// Constant for AssetUnit
        /// </summary>
        public static String TABLE_ASSET_UNIT = "AssetUnit";

        /// <summary>
        /// Constant for BidGroups
        /// </summary>
        public static String TABLE_BID_GROUPS = "BidGroup";

        /// <summary>
        /// Constant of join of BidGroup and lkpBidStatusType
        /// </summary>
        public static String TABLE_BIDGROUP_DOT_LKPBIDSTATUSTYPE = "BidGroup.lkpBidStatusType";


        /// <summary>
        /// Constant of lkpBidStatusType
        /// </summary>
        public static String TABLE_LKPBIDSTATUSTYPE = "lkpBidStatusType";

        /// <summary>
        /// Constant for Estimates
        /// </summary>
        public static String TABLE_ESTIMATES = "Estimates";

        /// <summary>
        /// Constant for lkpDamageType
        /// </summary>
        public static String TABLE_LKP_DAMAGE = "lkpDamageType";

        /// <summary>
        /// Constant for IRF
        /// </summary>
        public static String TABLE_IRF = "IRF";

        /// <summary>
        /// Constant for lkpDamageStatusType
        /// </summary>
        public static String TABLE_LKP_DAMAGE_STATUS_TYPE = "lkpDamageStatusType";

        /// <summary>
        /// Constant for lkpDamageStatusType
        /// </summary>
        public static String TABLE_ASSET_DAMAGE_DOT_LKP_DAMAGE_STATUS_TYPE = "AssetDamage.lkpDamageStatusType";
        /// <summary>
        /// Constant for ACF.ServiceOrderResults
        /// </summary>
        public static String TABLE_ACF_DOT_SERVICE_ORDER_RESULTS = "ACF.ServiceOrderResults";

        /// <summary>
        /// Constant for lkpPropertyEventType
        /// </summary>
        public static String TABLE_LKP_PROPERTY_EVENT_TYPE = "lkpPropertyEventType";

        /// <summary>
        /// Constant for CodeViolations
        /// </summary>
        public static String TABLE_CODE_VIOLATIONS = "CodeViolations";

        /// <summary>
        /// Constant for AssetDamage
        /// </summary>
        public static String TABLE_ASSET_DAMAGE = "AssetDamage";


        /// <summary>
        /// Constant for ACFAssetDamage
        /// </summary>
        public static String TABLE_ACF_ASSET_DAMAGE = "ACFAssetDamage";

        /// <summary>
        /// Constant for lkpDamageLocation
        /// </summary>
        public static String TABLE_LKP_DAMAGE_LOCATION = "lkpDamageLocation";

        /// <summary>
        /// Constant for CodeViolations.lkpCodeViolationFrequencyPeriod
        /// </summary>
        public static String TABLE_CODE_VIOLATIONS_DOT_LKP_CODE_VIOLATION_FREQUENCY_PERIOD = "CodeViolations.lkpCodeViolationFrequencyPeriod";

        /// <summary>
        /// Constant for CodeViolations.lkpCodeViolationType
        /// </summary>
        public static String TABLE_CODE_VIOLATIONS_DOT_LKP_CODE_VIOLATION_TYPE = "CodeViolations.lkpCodeViolationType";

        /// <summary>
        /// constant for ServiceRequest.AssetLoan.Asset.ValidatedAddress.ZipCode.City.State
        /// </summary>
        public static String TABLE_SERVICEREQUEST_ASSETLOAN_ASSET_VALIDATEDADDRESS_ZIPCODE_CITY_STATE = "AssetLoan.Asset.ValidatedAddress.ZipCode.City.State";

        /// <summary>
        /// constant for Asset.ValidatedAddress.ZipCode.City.State
        /// </summary>
        public static String TABLE_SERVICEREQUEST_ASSET_VALIDATEDADDRESS_ZIPCODE_CITY_STATE = "Asset.ValidatedAddress.ZipCode.City.State";

        /// <summary>
        /// constant for ServiceRequest.AssetLoan.Asset.ValidatedAddress
        /// </summary>
        public static String TABLE_SERVICEREQUEST_ASSETLOAN_ASSET_VALIDATEDADDRESS = "AssetLoan.Asset.ValidatedAddress";

        /// <summary>
        /// constant for ServiceRequest.Zone
        /// </summary>
        public static String TABLE_SERVICEREQUEST_ZONE = "Zone";

        /// <summary>
        /// constant for ServiceRequest.Zone.ZoneGeographies
        /// </summary>
        public static String TABLE_SERVICEREQUEST_ZONE_DOT_ZONEGEOGRAPHIES = "Zone.ZoneGeographies";

        /// <summary>
        /// constant for Supplier DashBoard TABLE_ASSETLOAN_ASSET_LEGALADDRESS
        /// </summary>
        //public static String TABLE_SERVICEREQUEST_ASSETLOAN_ASSET_LEGALADDRESS = "AssetLoan.Asset.LegalAddress"; //Temp commented by vipin
        public static String TABLE_SERVICEREQUEST_ASSETLOAN_ASSET_LEGALADDRESS = "AssetLoan.Asset";

        /// <summary>
        /// Constant for IRFStatu
        /// </summary>
        public static String TABLE_IRF_STATUS = "IRFStatu";

        /// <summary>
        /// Constant for ValidatedAddress.ZipCode.County
        /// </summary>
        public static String TABLE_ADDRESS_DOT_ZIPCODE_DOT_COUNTY = "ValidatedAddress.ZipCode.County";

        /// <summary>
        /// lkpServiceRequestType.lkpServiceBundle.Client.ClientAddresses.ValidatedAddress.ZipCode.City.State
        /// </summary>
        public static String TABLE_LKPSERVICEREQUESTTYPE_LKPSERVICEBUNDLE_CLIENT_CLIENTADDRESS_VALIDATEDADDRESS_ZIPCODE_CITY_STATE = "AssetLoan.Loan.Client.AddressHandle.Addresses.ZipCode.City";

        //lkpServiceRequestType.lkpServiceBundle.ClientServiceBundles.Client.ClientAddresses.ValidatedAddress.ZipCode.City.State
        /// <summary>
        /// Constant for HelpMain
        /// </summary>
        public static String TABLE_HELP_MAIN = "HelpMain";

        /// <summary>
        /// Constant for HelpContent
        /// </summary>
        public static String TABLE_HELP_CONTENT = "HelpContents";

        /// <summary>
        /// Constant for lkpUnitOfMeasure
        /// </summary>
        public static String TABLE_LKPUNITOFMEASURE = "lkpUnitOfMeasure";

        /// <summary>
        /// Constant for ZoneCosts
        /// </summary>
        public static String TABLE_ZONECOSTS = "ZoneCosts";

        /// <summary>
        /// Constant for ZoneCosts.lkpCostVarianceType
        /// </summary>
        public static String TABLE_ZONECOSTS_DOT_LKPCOSTVARIANCETYPE = "ZoneCosts.lkpCostVarianceType";

        /// <summary>
        /// Constant for lkpZoneType
        /// </summary>
        public static String TABLE_LKPZONETYPE = "lkpZoneType";

        /// <summary>
        /// Constant for County.State
        /// </summary>
        public static String TABLE_COUNTY_DOT_STATE = "County.State";

        /// <summary>
        /// lkpServiceBundle.BundleElements.lkpServiceType
        /// </summary>
        public static String TABLE_LKPSERVICEBUNDLE_BUNDLEELEMENT_LKPSERVICETYPE = "lkpServiceBundle.BundleElements.lkpServiceType";

        /// <summary>
        /// Constant for ZipCode.County
        /// </summary>
        public static String TABLE_ZIPCODE_DOT_COUNTY = "ZipCode.County";

        /// <summary>
        /// constant for ZipCode.City
        /// </summary>
        public static String TABLE_ZIPCODE_DOT_CITY = "ZipCode.City";

        /// <summary>
        /// constant for ZipCode.City.State
        /// </summary>
        public static String TABLE_ZIPCODE_DOT_CITY_DOT_STATE = "ZipCode.City.State";

        /// <summary>
        /// ClientAddresses.ValidatedAddress.ZipCode.City.State
        /// </summary>
        public static String TABLE_CLIENTADDRESSES_DOT_VALIDATEDADDRESS_DOT_ZIPCODE_DOT_CITY_DOT_STATE = "Addresses.ZipCode.City.State";

        /// <summary>
        /// Constant for Asset.LegalAddress.
        /// </summary>
        public static String TABLE_ASSET_DOT_LEGALADDRESS = "Asset.LegalAddress";
        
        /// <summary>
        /// Constant for ClientTransaction.Client.
        /// </summary>
        public static String TABLE_CLIENTTRANSACTION_DOT_CLIENT = "ClientTransaction.Client";

        /// <summary>
        /// ClientServiceAgreements
        /// </summary>
        public static String TABLE_CLIENTSERVICEAGREEMENTS = "ClientServiceAgreements";

        /// <summary>
        /// ClientContacts
        /// </summary>
        public static String TABLE_CLIENTCONTACTS = "ClientContacts";

        /// <summary>
        /// Tenant.TenantProducts
        /// </summary>
        public static String TABLE_TENANT_DOT_TENANTPRODUCTS = "Tenant.TenantProducts";

        /// <summary>
        /// Client1.ClientAddresses.ValidatedAddress.ZipCode.City.State
        /// </summary>
        public static String TABLE_CLIENT1_DOT_CLIENTADDRESSES_DOT_VALIDATEDADDRESS_DOT_ZIPCODE_DOT_CITY_DOT_STATE = "Client1.AddressHandle.Addresses.ZipCode.City.State";

        /// <summary>
        /// lkpClientStatus
        /// </summary>
        public static String TABLE_LKPCLIENTSTATUS = "lkpClientStatu";

        /// <summary>
        /// ClientLSCI
        /// </summary>
        public static String TABLE_CLIENTLSCIS = "ClientLSCIs";

        /// <summary>
        /// String Constant for Liens.LienContacts.
        /// </summary>
        public static String TABLE_LIENS_DOT_LIENCONTACTS = "Liens.LienContacts";

        /// <summary>
        /// String Constant for Liens.LienContacts.Contact.
        /// </summary>
        public static String TABLE_LIENS_DOTLIENCONTACTS_DOT_CONTACT = "Liens.LienContacts.Contact";

        /// <summary>
        /// String Constant for Liens.LienContacts.NonContributor.AddressHandle.
        /// </summary>
        public static String TABLE_LIENS_DOT_LIENCONTACTS_DOT_NONCONTRIBUTOR_DOT_ADDRESSHANDLE = "Liens.LienContacts.NonContributor.AddressHandle";

        /// <summary>
        /// String Constant for Liens.LienContacts.NonContributor.AddressHandle.Addresses.
        /// </summary>
        public static String TABLE_LIENS_DOT_LIENCONTACTS_DOT_NONCONTRIBUTOR_DOT_ADDRESSHANDLE_DOT_ADDRESSES = "Liens.LienContacts.NonContributor.AddressHandle.Addresses";

        /// <summary>
        /// String Constant for Liens.LienContacts.NonContributor.AddressHandle.Addresses.ZipCode.
        /// </summary>
        public static String TABLE_LIENS_DOT_LIENCONTACTS_DOT_NONCONTRIBUTOR_DOT_ADDRESSHANDLE_DOT_ADDRESSES_DOT_ZIPCODE = "Liens.LienContacts.NonContributor.AddressHandle.Addresses.ZipCode";

        /// <summary>
        /// String Constant for Liens.LienContacts.NonContributor.AddressHandle.Addresses.ZipCode.City.
        /// </summary>
        public static String TABLE_LIENS_DOT_LIENCONTACTS_DOT_NONCONTRIBUTOR_DOT_ADDRESSHANDLE_DOT_ADDRESSES_DOT_ZIPCODE_DOT_CITY = "Liens.LienContacts.NonContributor.AddressHandle.Addresses.ZipCode.City";

        /// <summary>
        /// String Constant for Liens.LienContacts.NonContributor.AddressHandle.Addresses.ZipCode.City.State.
        /// </summary>
        public static String TABLE_LIENS_DOT_LIENCONTACTS_DOT_NONCONTRIBUTOR_DOT_ADDRESSHANDLE_DOT_ADDRESSES_DOT_ZIPCODE_DOT_STATE = "Liens.LienContacts.NonContributor.AddressHandle.Addresses.ZipCode.City.State";

        /// <summary>
        /// String Constant for ServiceAddress.
        /// </summary>
        public static String TABLE_SERVICEADDRESS = "ServiceAddress";

        /// <summary>
        /// String Constant for AssetUnitUtilities.lkpFuelSource.
        /// </summary>
        public static String TABLE_ASSETUNITUTILITIES_DOT_LKPFUELSOURCE = "AssetUnitUtilities.lkpFuelSource";

        /// <summary>
        /// String Constant for AssetUnitUtilities.lkpWaterSource.
        /// </summary>
        public static String TABLE_ASSETUNITUTILITIES_DOT_LKPWATERSOURCE = "AssetUnitUtilities.lkpWaterSource";

        /// <summary>
        /// String Constant for AssetUnits.lkpLockBoxLocation.
        /// </summary>
        public static String TABLE_ASSETUNITS_DOT_LKPLOCKBOXLOCATION = "AssetUnits.lkpLockBoxLocation";

        /// <summary>
        /// String Constant for AssetDamages.lkpDamageLocation.
        /// </summary>
        public static String TABLE_ASSETDAMAGES_DOT_LKPDAMAGELOCATION = "AssetDamages.lkpDamageLocation";

        /// <summary>
        /// String Constant for lkpDwellingQualityBuild.
        /// </summary>
        public static String TABLE_LKPDWELLINGQUALITYBUILD = "lkpDwellingQualityBuild";

        #region Service Master

        /// <summary>
        /// Constant for lkp Client Invoice Code
        /// </summary>
        public static String TABLE_LKP_CLIENT_INVOICE_CODE = "lkpClientInvoiceCode";

        /// <summary>
        /// Constant for lkp Service Item
        /// </summary>
        public static String TABLE_LKP_SERVICE_ITEM = "lkpServiceItem";

        /// <summary>
        /// Constant for lkpServiceItem.lkpItemAction
        /// </summary>
        public static String TABLE_LKP_SERVICE_ITEM_DOT_LKP_ITEM_ACTION = "lkpServiceItem.lkpItemAction";

        /// <summary>
        /// Constant for lkp Service Item.lkp Item 
        /// </summary>
        public static String TABLE_LKP_SERVICE_ITEM_DOT_LKP_ITEM = "lkpServiceItem.lkpItem";

        /// <summary>
        /// Constant for lkp lkpServiceItem.lkpItem.ZoneCosts
        /// </summary>
        public static String TABLE_LKPSERVICEITEM_LKPITEM_ZONECOSTS = "lkpServiceItem.lkpItem.ZoneCosts";

        /// <summary>
        /// constant for TABLE_CLIENT_ITEM_CLIENT
        /// </summary>
        public static String TABLE_CLIENT_ITEM_CLIENT = "ClientItem.Client";

        /// <summary>
        ///  Constant for lkp Service Item.lkp Item Category
        /// </summary>
        public static String TABLE_LKP_SERVICE_ITEM_DOT_LKP_ITEM_CATEGORY = "lkpServiceItem.lkpItemCategory";

        /// <summary>
        /// Constant for lkp Service Item.lkp Unit Of Measure
        /// </summary>
        public static String TABLE_LKP_SERVICE_ITEM_DOT_LKP_UNIT_OF_MEASURE = "lkpServiceItem.lkpUnitOfMeasure";

        /// <summary>
        ///  Constant for lkp Service Bundle
        /// </summary>
        public static String TABLE_LKP_SERVICE_BUNDLE = "lkpServiceBundle";

        /// <summary>
        /// Constant for lkp Service Bundle.Client Service Bundles
        /// </summary>
        public static String TABLE_LKP_SERVICE_BUNDLE_DOT_CLIENT_SERVICE_BUNDLES = "lkpServiceBundle.ClientServiceBundles";

        /// <summary>
        /// Constant for lkp Unit Of Measure
        /// </summary>
        public static String TABLE_LKP_UNIT_OF_MEASURE = "lkpUnitOfMeasure";

        /// <summary>
        /// Constant for lkp Item
        /// </summary>
        public static String TABLE_LKP_ITEM = "lkpItem";

        /// <summary>
        /// Constant for lkp Item
        /// </summary>
        public static String TABLE_LKP_EYEBALL_ESTIMATION_TYPE = "lkpEyeBallEstimationStatusType";

        /// <summary>
        /// Constant for lkp Item Category
        /// </summary>
        public static String TABLE_LKP_ITEM_CATEGORY = "lkpItemCategory";

        /// <summary>
        /// Constant for lkp Item Category.lkp Item Category1
        /// </summary>
        public static String TABLE_LKP_ITEM_CATEGORY_DOT_LKP_ITEM_CATEGORY1 = "lkpItemCategory.lkpItemCategory1";

        /// <summary>
        /// Constant for lkp Item Category.lkp Item Category2
        /// </summary>
        public static String TABLE_LKP_ITEM_CATEGORY_DOT_LKP_ITEM_CATEGORY2 = "lkpItemCategory.lkpItemCategory2";

        /// <summary>
        /// Constant for lkp Specialty Identifier
        /// </summary>
        public static String TABLE_LKP_SPECIALTY_IDENTIFIER = "lkpSpecialtyIdentifier";

        /// <summary>
        /// Constant for Service Elements
        /// </summary>
        public static String TABLE_SERVICE_ELEMENTS = "ServiceElements";

        /// <summary>
        /// Constant for Service Item Attributes
        /// </summary>
        public static String TABLE_SERVICE_ITEM_ATTRIBUTES = "ServiceItemAttributes";

        /// <summary>
        /// Constant for Service Item Object Attributes
        /// </summary>
        public static String TABLE_SERVICE_ITEM_OBJECT_ATTRIBUTES = "ServiceItemObjectAttributes";

        /// <summary>
        /// Constant for Service Item Results
        /// </summary>
        public static String TABLE_SERVICE_ITEM_RESULTS = "ServiceItemResults";

        /// <summary>
        /// Constant for Bid Items
        /// </summary>
        public static String TABLE_BID_ITEMS = "BidItems";

        /// <summary>
        /// Constant for BidItems.lkpServiceItem
        /// </summary>
        public static String TABLE_BID_ITEMS_DOT_LKPSERVICEITEM = "BidItems.lkpServiceItem";
        /// <summary>
        ///  Constant for lkp Service Item . lkp Unit Of Measure
        /// </summary>
        public static String TABLE_SERVICE_ITEM_DOT_LKP_UNIT_OF_MEASURE = "ServiceItem.lkpUnitOfMeasure";

        /// <summary>
        /// Constant for lkp Service elements . lkp service type
        /// </summary>
        public static String TABLE_SERVICE_ELEMENTS_DOT_LKP_SERVICE_TYPE = "ServiceElements.lkpServiceType";

        /// <summary>
        /// Constant for lkp Service Item Object
        /// </summary>
        public static String TABLE_LKP_SERVICE_ITEM_OBJECT = "lkpServiceItemObject";

        /// <summary>
        /// TABLE_LKPSERVICEREQUESTTYPE
        /// </summary>
        public static String TABLE_LKPSERVICEREQUESTTYPE = "lkpServiceRequestType";

        /// <summary>
        //TABLE_LKPSERVICEREQUESTTYPE_LKPSERVICEBUNDLE_BUNDLEELEMENTS_LKPSERVICETYPE
        /// </summary>
        public static String TABLE_LKPSERVICEREQUESTTYPE_LKPSERVICEBUNDLE_BUNDLEELEMENTS_LKPSERVICETYPE = "lkpServiceRequestType.lkpServiceBundle.BundleElements.lkpServiceType";

        /// <summary>
        /// Constant for AssetLoans.Loan.FhaLoanDetails
        /// </summary>
        public static String TABLE_ASSET_LOANS_DOT_LOAN_DOT_FHA_LOAN_DETAILS = "AssetLoans.Loan.FHALoanDetails.lkpConveyanceStatu";

        /// <summary>
        /// Constant for AssetLoans.Loan.LoanStatus.lkpClientLoanStatusType
        /// </summary>
        public static String TABLE_ASSET_LOANS_DOT_LOAN_DOT_LOANSTATUS_DOT_LKPCLIENTLOANSTATUSTYPE = "AssetLoans.Loan.LoanStatus.lkpClientLoanStatusType";

        /// <summary>
        /// Constant for ClientItemCost
        /// </summary>

        public static String TABLE_CLIENTITEMCOST = "ClientItemCosts";


        public static String TABLE_LKPSERVICEBUNDLE_BUNDLELEMENTS = "lkpServiceBundle.BundleElements";



        #endregion

        #region Supplier

        /// <summary>
        /// Tenant
        /// </summary>
        public static String TABLE_TANENT = "Tenant";

        /// <summary>
        /// Tenant Organizations
        /// </summary>
        public static String TABLE_TANENT_ORGANIZATIONS = "Tenant.Organizations";

        /// <summary>
        /// Tenant Organizations Contact
        /// </summary>
        public static String TABLE_TANENT_ORGANIZATIONS_CONTACT = "Tenant.Organizations.Contact";

        /// <summary>
        /// Services
        /// </summary>
        public static String TABLE_SERVICEORDERS = "ServiceOrders";

        /// <summary>
        /// ServiceOrders.lkpServiceType
        /// </summary>
        public static String TABLE_SERVICES_DOT_LKPSERVICETYPE = "ServiceOrders.lkpServiceType";

        /// <summary>
        /// ServiceOrders.lkpServiceType
        /// </summary>
        public static String TABLE_SERVICE_REQUEST_DOT_LKPSERVICEREQUESTTYPE = "ServiceRequests.lkpServiceRequestType";

        /// <summary>
        /// ServiceOrders.lkpServiceType
        /// </summary>
        public static String TABLE_SERVICESORDERS_DOT_SERVICE_REQUEST_DOT_LKPSERVICEREQUESTTYPE = "ServiceOrders.ServiceRequest.lkpServiceRequestType";

        /// <summary>
        /// ServiceOrders.ServiceRequest
        /// </summary>
        public static String TABLE_SERVICESORDERS_DOT_SERVICEREQUEST = "ServiceOrders.ServiceRequest";

        /// <summary>
        ///ServiceRequests
        /// </summary>
        public static String TABLE_SERVICEREQUESTS = "ServiceRequests";

        /// <summary>
        /// AssetLoan
        /// </summary>
        public static String TABLE_ASSETLOAN = "AssetLoan";

        /// <summary>
        /// AssetLoan.LegalAddress
        /// </summary>
        public static String TABLE_ASSETLOAN_LEGALADDRESS = "AssetLoan.LegalAddress";

        /// <summary>
        /// lkpIncidentOutcome
        /// </summary>
        public static String TABLE_LKPINCIDENTOUTCOME = "lkpIncidentOutcome";

        /// <summary>
        /// lkpIncidentStatu
        /// </summary>
        public static String TABLE_LKPINCIDENTSTATUS = "lkpIncidentStatu";

        /// <summary>
        /// lkpIncidentType
        /// </summary>
        public static String TABLE_LKPINCIDENTTYPE = "lkpIncidentType";

        /// <summary>
        /// lkpSeverity
        /// </summary>
        public static String TABLE_LKPSEVERITY = "lkpSeverity";

        /// <summary>
        /// ServiceOrders.ServiceRequest.AssetLoan
        /// </summary>
        public static String TABLE_SERVICESORDERS_DOT_SERVICEREQUEST_DOT_ASSETLOAN = "ServiceOrders.ServiceRequest.AssetLoan";

        /// <summary>
        /// SupplierIncidentTickets.ServiceOrderIncidentTickets
        /// </summary>
        public static String TABLE_SUPPLIERINCIDENTTICKET_DOT_SERVICESORDERINCIDENTTICKET = "SupplierIncidentTickets.ServiceOrderIncidentTickets";

        /// <summary>
        /// SupplierIncidentTickets
        /// </summary>
        public static String TABLE_SUPPLIERINCIDENTTICKETS = "SupplierIncidentTickets";

        /// <summary>
        /// SupplierIncidentTickets
        /// </summary>
        public static String TABLE_SERVICESORDERINCIDENTTICKET = "ServiceOrderIncidentTickets";

        /// <summary>
        /// SupplierIncidentTickets
        /// </summary>
        public static String TABLE_SERVICESORDERINCIDENTTICKET_DOT_SERVICEORDER = "ServiceOrderIncidentTickets.ServiceOrder";

        /// <summary>
        /// ServiceOrders.ServiceRequest.AssetLoan.Loan
        /// </summary>
        public static String TABLE_ASSETLOAN_DOT_LOAN = "AssetLoan.Loan";

        /// <summary>
        /// ServiceOrders.ServiceRequest.AssetLoan.Loan
        /// </summary>
        public static String TABLE_SERVICESORDERS_DOT_SERVICEREQUEST_DOT_ASSETLOAN_DOT_LOAN = "ServiceOrders.ServiceRequest.AssetLoan.Loan";

        /// <summary>
        /// ServiceOrders.Supplier
        /// </summary>
        public static String TABLE_SERVICES_DOT_SUPPLIER = "ServiceOrders.Supplier";

        /// <summary>
        /// ServiceOrders.Supplier.ValidatedAddress
        /// </summary>
        public static String TABLE_SERVICES_DOT_SUPPLIER_DOT_VALIDATEDADDRESS = "ServiceOrders.Supplier.AddressHandle.Addresses.ZipCode.CITY.STATE";

        /// <summary>
        /// ServiceOrders.Supplier.ValidatedAddress.ZipCode
        /// </summary>
        public static String TABLE_SERVICES_DOT_SUPPLIER_DOT_VALIDATEDADDRESS_DOT_ZIPCODE = "ServiceOrders.Supplier.ValidatedAddress.ZipCode";

        /// <summary>
        /// ServiceOrders.Supplier.ValidatedAddress.ZipCode.City
        /// </summary>
        public static String TABLE_SERVICES_DOT_SUPPLIER_DOT_VALIDATEDADDRESS_DOT_ZIPCODE_DOT_CITY = "ServiceOrders.Supplier.ValidatedAddress.ZipCode.City";

        /// <summary>
        /// ServiceOrders.Supplier.ValidatedAddress.ZipCode.City.State
        /// </summary>
        public static String TABLE_SERVICES_DOT_SUPPLIER_DOT_VALIDATEDADDRESS_DOT_ZIPCODE_DOT_CITY_DOT_STATE = "ServiceOrders.Supplier.ValidatedAddress.ZipCode.City.State";

        /// <summary>
        /// Loan
        /// </summary>
        public static String TABLE_LOAN = "Loan";

        /// <summary>
        /// lkpServiceType.ServiceElements.lkpServiceItem
        /// </summary>
        public static String TABLE_LKPSERVICETYPE_DOT_SERVICEELEMENTS_DOT_LKPSERVICEITEM = "lkpServiceType.ServiceElements.lkpServiceItem";

        /// <summary>
        /// lkpServiceType.ServiceElements.lkpServiceItem.ClientItem
        /// </summary>
        public static String TABLE_LKPSVCTYPE_DOT_SVCELEMENTS_DOT_LKPSVCITEM_CLIENTITEM = "lkpServiceType.ServiceElements.lkpServiceItem.ClientItem";

        /// <summary>
        /// lkpServiceType.ServiceElements
        /// </summary>
        public static String TABLE_LKPSERVICETYPE_DOT_SERVICEELEMENTS = "lkpServiceType.ServiceElements";
        
        /// <summary>
        /// tblWorkflowAssemblies
        /// </summary>
        public static String TABLE_TBLWORKFLOWASSEMBLIES = "tblWorkflowAssemblies";

        /// <summary>
        /// ClientItems
        /// </summary>
        public static String TABLE_CLIENTITEMS = "ClientItems";

        /// <summary>
        /// lkpServiceItems
        /// </summary>
        public static String TABLE_LKPSERVICEITEMS = "lkpServiceItems";

        /// <summary>
        /// SysXBlock
        /// </summary>
        public static String TABLE_SYSXBLOCK = "lkpSysXBlock";


        /// <summary>
        /// ServiceOrders.ServiceRequest.ServiceRequestActivities.ServiceRequestActivityAttributes
        /// </summary>
        public static String TABLE_SERVICES_SERVICEREQUEST_SERVICEREQUESTACTIVITIES_SERVICEREQUESTACTIVITYATTRIBUTES = "ServiceOrders.ServiceRequest.ServiceRequestActivities.ServiceRequestActivityAttributes";

        #endregion

        #region Service Request

        /// <summary>
        /// Tenants
        /// </summary>
        public static String TABLE_SERVICE_REQUEST_TENANTS = "Tenants";

        /// <summary>
        /// Tenants.Organizations
        /// </summary>
        public static String TABLE_SERVICE_REQUEST_TENANTS_DOT_ORGANIZATIONS = "Tenants.Organizations";

        /// <summary>
        /// Tenants.Organizations.OrganizationUsers
        /// </summary>
        public static String TABLE_SERVICE_REQUEST_TENANTS_DOT_ORGANIZATIONS_DOT_ORGANIZATIONUSERS = "Tenants.Organizations.OrganizationUsers";

        /// <summary>
        /// Tenants.Organizations.OrganizationUsers.aspnet_Users
        /// </summary>
        public static String TABLE_SERVICE_REQUEST_TENANTS_DOT_ORGANIZATIONS_DOT_ORGANIZATIONUSERS_DOT_ASPNET_USERS = "Tenants.Organizations.OrganizationUsers.aspnet_Users";

        /// <summary>
        /// SupplierRating
        /// </summary>
        public static String TABLE_SERVICE_REQUEST_LKPSUPPLIERRATING = "SupplierRating";

        /// <summary>
        /// lkpSupplierStatu
        /// </summary>
        public static String TABLE_SERVICE_REQUEST_LKPSUPPLIERSTATU = "lkpSupplierStatu";

        /// <summary>
        /// lkpContext
        /// </summary>
        public static String TABLE_SERVICE_REQUEST_LKPCONTEXT = "lkpContext";

        /// <summary>
        /// Constant for UserGroupMessageTypes.
        /// </summary>
        public static String TABLE_USERGROUPMESSAGETYPES = "UserGroupMessageTypes";

        /// <summary>
        /// lkpServiceType
        /// </summary>
        public static String TABLE_SERVICE_REQUEST_LKPSERVICETYPE = "lkpServiceType";

        /// <summary>
        /// ServiceOrders
        /// </summary>
        public static String TABLE_SERVICE_REQUEST_SERVICEORDERS = "ServiceOrders";

        /// <summary>
        /// Constant for Supplier Dash board ServiceOrders.ACF
        /// </summary>
        public static String TABLE_SERVICES_ACF = "ServiceOrders.ServiceOrderResults.ACF";

        /// <summary>
        /// lkpServiceRequestType
        /// </summary>
        public static String TABLE_SERVICES_LKPSERVICEREQUESTTYPE = "lkpServiceRequestType";

        /// <summary>
        /// AssetLoans.Asset
        /// </summary>
        public static String TABLE_ASSETLOANS_ASSET = "AssetLoans.Asset";

        /// <summary>
        /// AssetLoans.Asset
        /// </summary>
        public static String TABLE_ASSETLOANS = "AssetLoans";

        /// <summary>
        /// Constant for Supplier Dashboard - IRF
        /// </summary>
        public static String TABLE_IRF_DOT_IRFSTATU_DOT_IRFSTATUSTYPE = "ServiceOrders.ServiceOrderResults.IRF";

        /// <summary>
        /// ServiceRequestActivities
        /// </summary>
        public static String TABLE_SERVICEREQUESTACTIVITIES = "ServiceRequestActivities";

        /// <summary>
        /// ServiceRequestActivities.ServiceRequestActivityAttributes
        /// </summary>
        public static String TABLE_SERVICEREQUESTACTIVITIES_SERVICEREQUESTACTIVITYATTRIBUTES = "ServiceRequestActivities.ServiceRequestActivityAttributes";

        /// <summary>
        /// ServiceRequest.AssetLoan
        /// </summary>
        public static String TABLE_SERVICEREQUEST_ASSETLOAN = "ServiceRequest.AssetLoan";

        /// <summary>
        /// ServiceRequest.AssetLoan.Loan
        /// </summary>
        public static String TABLE_SERVICEREQUEST_ASSETLOAN_LOAN = "ServiceRequest.AssetLoan.Loan";

        /// <summary>
        /// ServiceRequest.AssetLoan.Loan.Client
        /// </summary>
        public static String TABLE_SERVICEREQUEST_ASSETLOAN_LOAN_CLIENT = "ServiceRequest.AssetLoan.Loan.Client";

        /// <summary>
        /// ServiceRequest.AssetLoan.Loan
        /// </summary>
        public static String TABLE_ASSETLOAN_LOAN = "AssetLoan.Loan";

        /// <summary>
        /// ServiceActivities
        /// </summary>
        public static String TABLE_SERVICEACTIVITIES = "ServiceActivities";

        /// <summary>
        /// ServiceRequest.AssetLoan.Asset
        /// </summary>
        public static String TABLE_SERVICEREQUEST_ASSETLOAN_ASSET = "ServiceRequest.AssetLoan.Asset";

        /// <summary>
        /// ServiceActivities.ServiceActivityReasons
        /// </summary>
        public static String TABLE_SERVICEACTIVITIES_SERVICEACTIVITYREASONS = "ServiceActivities.ServiceActivityReasons";

        /// <summary>
        /// ServiceActivities.ServiceActivityReasons.ServiceActivityReasonAttributes
        /// </summary>
        public static String TABLE_SERVICEACTIVITIES_SERVICEACTIVITYREASONS_SERVICEACTIVITYREASONATTRIBUTES = "ServiceActivities.ServiceActivityReasons.ServiceActivityReasonAttributes";

        /// <summary>
        /// AssetLoan.Loan.lkpLoanType
        /// </summary>
        public static String TABLE_ASSETLOAN_LOAN_LKPLOANTYPE = "AssetLoan.Loan.lkpLoanType";

        /// <summary>
        /// Loan.lkpLoanType
        /// </summary>
        public static String TABLE_LOAN_LKPLOANTYPE = "Loan.lkpLoanType";

        /// <summary>
        /// lkpActivityType
        /// </summary>
        public static String TABLE_LKPACTIVITYTYPE = "lkpActivityType";
        
        /// <summary>
        /// ServiceOrders.ServiceOrderActivities
        /// </summary>
        public static String TABLE_SERVICEORDERS_SERVICEORDERACTIVITIES = "ServiceOrders.ServiceOrderActivities";

        /// <summary>
        /// ServiceOrders.ServiceOrderActivities.lkpActivityType
        /// </summary>
        public static String TABLE_SERVICEORDERS_SERVICEORDERACTIVITIES_LKPACTIVITYTYPE = "ServiceOrders.ServiceOrderActivities.lkpActivityType";

        /// <summary>
        /// ServiceOrders.ServiceOrderActivities.ServiceOrderActivityResults
        /// </summary>
        public static String TABLE_SERVICEORDERS_SERVICEORDERACTIVITIES_SERVICEORDERACTIVITYRESULTS = "ServiceOrders.ServiceOrderActivities.ServiceOrderActivityResults";

        #endregion

        #region Insurance Loss

        /// <summary>
        /// AssetLoan.Asset.ValidatedAddress.ZipCode.City.State
        /// </summary>
        public static String TABLE_ASSETLOAN_ASSET_VALIDATEDADDRESS_ZIPCODE_CITY_STATE = "AssetLoan.Asset.ValidatedAddress.ZipCode.City.State";

        /// <summary>
        /// AssetLoan.Loan.Client
        /// </summary>
        public static String TABLE_ASSETLOAN_LOAN_CLIENT = "AssetLoan.Loan.Client";

        /// <summary>
        /// AssetLoan.Loan.ClientLSCI
        /// </summary>
        public static String TABLE_ASSETLOAN_LOAN_CLIENTLSCI = "AssetLoan.Loan.ClientLSCI";

        /// <summary>
        /// AssetLoan.Loan.Client.Tenant.Organizations.OrganizationUsers
        /// </summary>
        public static String TABLE_ASSETLOAN_LOAN_CLIENT_TENANT_ORGANIZATIONS_ORGANIZATIONUSERS = "AssetLoan.Loan.Client.Tenant.Organizations.OrganizationUsers";

        /// <summary>
        /// AssetLoan.Loan.LoanContacts.NonContributor.Contact
        /// </summary>
        public static String TABLE_ASSETLOANS_LOAN_LOANCONTACTS_NONCONTRIBUTOR_CONTACT = "AssetLoan.Loan.LoanContacts.NonContributor.Contact";

        /// <summary>
        /// AssetLoan.Loan.LoanContacts.NonContributor.lkpNonContributorType
        /// </summary>
        public static String TABLE_ASSETLOANS_LOAN_LOANCONTACTS_NONCONTRIBUTOR_LKPNONCONTRIBUTORTYPE = "AssetLoan.Loan.LoanContacts.NonContributor.lkpNonContributorType";

        /// <summary>
        /// lkpServiceStatusType
        /// </summary>
        public static String TABLE_LKPSERVICESTATUSTYPE = "lkpServiceStatusType";

        /// <summary>
        /// ServiceRequestDetails
        /// </summary>
        public static String TABLE_SERVICEREQUESTDETAILS = "ServiceRequestDetails";

        /// <summary>
        /// ServiceOrders.Supplier
        /// </summary>
        public static String TABLE_SERVICEORDERS_SUPPLIER = "ServiceOrders.Supplier";

        /// <summary>
        /// ServiceOrder.ServiceRequest
        /// </summary>
        public static String TABLE_SERVICEORDER_SERVICEREQUEST = "ServiceOrder.ServiceRequest";

        /// <summary>
        /// ServiceOrder.lkpServiceStatusType
        /// </summary>
        public static String TABLE_SERVICEORDER_LKPSERVICESTATUSTYPE = "ServiceOrder.lkpServiceStatusType";

        /// <summary>
        /// InsuranceLossResultItems.lkpInsuranceLossWorkItem
        /// </summary>
        public static String TABLE_INSURANCELOSSRESULTITEMS_LKPINSURANCELOSSWORKITEM = "InsuranceLossResultItems.lkpInsuranceLossWorkItem";

        /// <summary>
        /// ServiceOrders.lkpServiceStatusType
        /// </summary>
        public static String TABLE_SERVICEORDERS_LKPSERVICESTATUSTYPE = "ServiceOrders.lkpServiceStatusType";

        /// <summary>
        /// ServiceRequest.ServiceOrders
        /// </summary>
        public static String TABLE_SERVICEREQUEST_SERVICEORDERS = "ServiceRequest.ServiceOrders";

        /// <summary>
        /// IRF.IRFStatu.IRFStatusType
        /// </summary>
        public static String TABLE_IRF_IRFSTATU_IRFSTATUSTYPE = "IRF.IRFStatu.IRFStatusType";

        /// <summary>
        /// Contact.NonContributors.ClientContacts
        /// </summary>
        public static String TABLE_CONTACT_NONCONTRIBUTORS_CLIENTCONTACTS = "Contact.NonContributors.ClientContacts";

        /// <summary>
        /// Contact.Tenants.SupplierContacts
        /// </summary>
        public static String TABLE_CONTACT_SUPPLIERCONTACTS = "Contact.SupplierContacts";

        /// <summary>
        /// AssetLoan.Loan.Client.Tenant.Organizations
        /// </summary>
        public static String TABLE_ASSETLOAN_LOAN_CLIENT_TENANT_ORGANIZATION = "AssetLoan.Loan.Client.Tenant.Organizations";

        /// <summary>
        /// SupplierWorkTypes.lkpServiceType.BundleElements.lkpServiceBundle.lkpServiceRequestTypes
        /// </summary>
        public static String TABLE_SUPPLIERWORKTYPES_LKPSERVICETYPE_BUNDLEELEMENTS_LKPSERVICEBUNDLE_LKPSERVICEREQUESTTYPES = "SupplierWorkTypes.lkpServiceType.BundleElements.lkpServiceBundle.lkpServiceRequestTypes";

        #endregion

        #region Accounting

        /// <summary>
        /// Constant for RoleRank.
        /// </summary>
        public static String TABLE_ROLERANK = "RoleRank";

        /// <summary>
        /// Constant for RoleDetail.
        /// </summary>
        public static String TABLE_ROLEDETAIL = "RoleDetail";

        /// <summary>
        /// Constant for RoleDetail.aspnet_Roles.
        /// </summary>
        public static String TABLE_ROLEDETAIL_DOT_ASPNET_ROLES = "RoleDetail.aspnet_Roles";

        /// <summary>
        /// Constant for RoleRank.aspnet_Roles.
        /// </summary>
        public static String TABLE_ROLERANK_DOT_ASPNET_ROLES = "RoleRank.aspnet_Roles";

        /// <summary>
        /// lkpAdjustmentReason table
        /// </summary>
        public static String TABLE_LKPADJUSTMENTREASON = "lkpAdjustmentReason";

        /// <summary>
        /// Constant for AssetLoans.Asset.ValidatedAddress
        /// </summary>
        public static String TABLE_ASSETLOANS_ASSET_VALIDATEDADDRESS = "AssetLoans.Asset.ValidatedAddress";

        /// <summary>
        /// Constant for AssetLoans.Asset.ValidatedAddress.ZipCode.City.State
        /// </summary>
        public static String TABLE_ASSETLOANS_ASSET_VALIDATEDADDRESS_ZIPCODE_CITY_STATE = "AssetLoans.Asset.ValidatedAddress.ZipCode.City.State";

        /// <summary>
        /// Constant for AssetLoans.Asset.ValidatedAddress.ZipCode.City
        /// </summary>
        public static String TABLE_ASSETLOANS_ASSET_VALIDATEDADDRESS_ZIPCODE_CITY = "AssetLoans.Asset.ValidatedAddress.ZipCode.City";

        /// <summary>
        /// Constant for AssetLoans.Asset.ValidatedAddress.ZipCode
        /// </summary>
        public static String TABLE_ASSETLOANS_ASSET_VALIDATEDADDRESS_ZIPCODE = "AssetLoans.Asset.ValidatedAddress.ZipCode";

        /// <summary>
        /// Constant for lkpServiceType.ServiceElements.lkpServiceItem.lkpItem
        /// </summary>
        public static String TABLE_LKPSERVICETYPE_SERVICEELEMENTS_LKPSERVICEITEM_LKPITEM = "lkpServiceType.ServiceElements.lkpServiceItem.lkpItem";

        /// <summary>
        /// Constant for RoleDetail.aspnet_Roles.aspnet_Users.OrganizationUsers
        /// </summary>
        public static String TABLE_ROLEDETAIL_ASPNET_ROLES_ASPNET_USERS_ORGANIZATIONUSERS = "RoleDetail.aspnet_Roles.aspnet_Users.OrganizationUsers";

        /// <summary>
        /// Constant for ServiceOrderResults.ServiceItemResults
        /// </summary>
        public static String TABLE_ServiceOrderResults_ServiceItemResults = "ServiceOrderResults.ServiceItemResults";

        /// <summary>
        /// Constant for ServiceOrderResults.IRF.IRFStatu
        /// </summary>
        public static String TABLE_SERVICEORDERRESULTS_IRF = "ServiceOrderResults.IRF";

        /// <summary>
        /// Constant for AssetSaleViolations.AssetSaleViolationPostedPresents
        /// </summary>
        public static String TABLE_ASSETSALEVIOLATIONS_ASSETSALEVIOLATIONPOSTEDPRESENTS = "AssetSaleViolations.AssetSaleViolationPostedPresents";

        /// <summary>
        /// Constant for AssetTransactions.AssetDamageLocations
        /// </summary>
        public static String TABLE_ASSETTRANCTIONS_ASSETDAMAGELOCATIONS = "AssetTransactions.AssetDamageLocations";

        /// <summary>
        /// Constant for AssetUnits.AssetUnitUtilities1
        /// </summary>
        public static String TABLE_ASSETUNITS_ASSETUNITUTILITIES1 = "AssetUnits.AssetUnitUtilities";

        /// <summary>
        /// Constant for AssetUnits.AssetUnitUtilities.AssetUnitUtilityDetails
        /// </summary>
        public static String TABLE_ASSETUNITS_ASSETUNITUTILITIES_ASSETUNITUTILITYDETAILS = "AssetUnits.AssetUnitUtilities.AssetUnitUtilityDetails";

        /// <summary>
        /// Constant for AssetOccupancies.AssetOccupancyVisualEvidences
        /// </summary>
        public static String TABLE_ASSETOCCUPANCIES_ASSETOCCUPANCYVISUALEVIDENCES = "AssetOccupancies.AssetOccupancyVisualEvidences";

        /// <summary>
        /// Constant for AssetAdditionalStructures
        /// </summary>
        public static String TABLE_ASSETADDITIONALSTRUCTURES = "AssetAdditionalStructures";

        /// <summary>
        /// Constant for AssetFEMADetails
        /// </summary>
        public static String TABLE_ASSETFEMADETAILS = "AssetFEMADetails";

        /// <summary>
        /// Constant for AssetUnits.AssetUnitAppliances
        /// </summary>
        public static String TABLE_ASSETUNITS_ASSETUNITAPPLIANCES = "AssetUnits.AssetUnitAppliances";

        /// <summary>
        /// Constant for AssetContacts.NonContributor.Contact.ContactDetails
        /// </summary>
        public static String TABLE_ASSETCONTACTS_NONCONTRIBUTOR_CONTACT_CONTACTDETAILS = "AssetContacts.NonContributor.Contact.ContactDetails";

        /// <summary>
        /// Constant for AssetPropertyDetails.Contact.ContactDetails
        /// </summary>
        public static String TABLE_ASSETPROPERTYDETAILS_CONTACT_CONTACTDETAILS = "AssetPropertyDetails.Contact.ContactDetails";

        /// <summary>
        /// Constant for AssetLoan.Asset.AssetOccupancies
        /// </summary>
        public static String TABLE_ASSETLOAN_ASSET_ASSETOCCUPANCIES = "AssetLoan.Asset.AssetOccupancies";

        /// <summary>
        /// Constant for AssetLoan.Asset.ValidatedAddress.GeoCoding
        /// </summary>
        public static String TABLE_ASSETLOAN_ASSET_VALIDATEDADDRESS_GEOCODING = "AssetLoan.Asset.ValidatedAddress.GeoCoding";

        /// <summary>
        /// Constant for lkpCodeViolationType
        /// </summary>
        public static String TABLE_LKPCODEVIOLATIONTYPE = "lkpCodeViolationType";

        /// <summary>
        /// Constant for AssetSaleViolation
        /// </summary>
        public static String TABLE_ASSETSALEVIOLATION = "AssetSaleViolation";

        #endregion

        #region Messaging

        /// <summary>
        /// Messaging constants.
        /// </summary>
        /// 

        public static String TABLE_ADBMESSAGING_ADBMESSAGETOUSERGROUPS = "ADBMessage.ADBMessageToUserGroups";
        public static String TABLE_CLIENTMESSAGING_CLIENTMESSAGETOUSERGROUPS = "ApplicantMessage.ApplicantMessageToUserGroups";

        public static String TABLE_MESSAGING_ASPNET_USERS_DOT_ASPNET_MEMBERSHIP = "aspnet_Users.aspnet_Membership";
        public static String TABLE_MESSAGING_ADBMESSAGE = "ADBMessage";
        public static String TABLE_MESSAGING_SUPPLIERMESSAGE = "SupplierMessage";
        public static String TABLE_MESSAGING_CLIENTMESSAGE = "ApplicantMessage";

        public static String TABLE_MESSAGING_ADBMESSAGETOLISTS = "ADBMessageToLists";
        public static String TABLE_MESSAGING_ADBMESSAGETOUSERGROUPS = "ADBMessageToUserGroups";



        
        public static String TABLE_MESSAGING_SUPPLIERMESSAGETOLISTS = "SupplierMessageToLists";
        public static String TABLE_MESSAGING_CLIENTMESSAGETOLISTS = "ClientMessageToLists";

        
        public static String TABLE_MESSAGING_SUPPLIERMESSAGE2 = "SupplierMessage2";
        public static String TABLE_MESSAGING_CLIENTMESSAGE2 = "ClientMessage2";
        public static String TABLE_MESSAGING_ASPNET_USERS_DOT_ASPNET_ROLES_DOT_ROLEDETAIL_DOT_TENANTPRODUCT_DOT_TENANT = "aspnet_Users.aspnet_Roles.RoleDetail.TenantProduct.Tenant";

        #endregion

        #region Investor Insurer Guideline

        public static String TABLE_GUIDELINE = "guideline";
        public static String TABLE_GUIDELINE_DOT_LKPITEM = "guideline.lkpItem";
        public static String TABLE_GUIDELINE_DOT_LKPREGION = "guideline.lkpRegion";
        public static String TABLE_GUIDELINE_DOT_CLIENT = "guideline.Client";
        public static String TABLE_GUIDELINE_ATTRIBUTES = "GuidelineAttributes";
        public static String TABLE_LKP_ITEM_DOT_LKP_UNIT_OF_MEASURE = "lkpItem.lkpUnitOfMeasure";

        #endregion

        #region Asset Utility

        /// <summary>
        /// Asset Utility Gas Active
        /// </summary>
        public static String ASSET_UTILITY_GAS_ACTIVE = "PLDATTRITA55687";

        /// <summary>
        /// Asset Utility Electric Active
        /// </summary>
        public static String ASSET_UTILITY_ELECTRIC_ACTIVE = "PLDATTRITA55680";

        /// <summary>
        /// Asset Utility Propane Fuel Oil Active
        /// </summary>
        public static String ASSET_UTILITY_PROPANE_FUEL_OIL_ACTIVE = "PLDATTRITA55692";

        /// <summary>
        /// Asset Utility Gas Is the Heat Being Maintained
        /// </summary>
        public static String ASSET_UTILITY_IS_THE_HEAT_BEING_MAINTAINED = "PLDATTRITA55681";

        /// <summary>
        /// Asset Utility table lkpAssetAttributeType
        /// </summary>
        public static String ASSET_UTILITY_LKPASSETATTRIBUTETYPE = "lkpAssetAttributeType";

        /// <summary>
        /// Asset Utility table UtilityTypeProviders
        /// </summary>
        public static String ASSET_UTILITY_UTILITYTYPEPROVIDERS = "UtilityTypeProviders";

        /// <summary>
        /// Asset Utility table UtilityTypeProviders.UtilityProvider
        /// </summary>
        public static String ASSET_UTILITY_UTILITYTYPEPROVIDERS_UTILITYPROVIDER = "UtilityTypeProviders.UtilityProvider";

        /// <summary>
        /// Asset Utility table UtilityTypeProviders.UtilityProvider.ProviderCompanies
        /// </summary>
        public static String ASSET_UTILITY_UTILITYTYPEPROVIDERS_DOT_UTILITYPROVIDER_DOT_PROVIDERCOMPANIES = "UtilityTypeProviders.UtilityProvider.ProviderCompanies";

        /// <summary>
        /// Asset Utility table lkpUtilityStatu
        /// </summary>
        public static String ASSET_UTILITY_LKPUTILITYSTATUS = "lkpUtilityStatu";

        /// <summary>
        /// Asset Utility table lkpContext
        /// </summary>
        public static String ASSET_UTILITY_LKPCONTEXT = "lkpContext";

        /// <summary>
        /// Asset Utility table UtilityType
        /// </summary>
        public static String ASSET_UTILITY_UTILITYTYPE = "UtilityType";

        /// <summary>
        /// Asset Utility table lkpUtilityStatu
        /// </summary>
        public static String ASSET_UTILITY_LKPUTILITYSTATU = "lkpUtilityStatu";

        /// <summary>
        /// Asset Utility table ProviderCompany
        /// </summary>
        public static String ASSET_UTILITY_PROVIDERCOMPANY = "ProviderCompany";

        /// <summary>
        /// Asset Utility table ProviderCompany.ValidatedAddress
        /// </summary>
        public static String ASSET_UTILITY_PROVIDERCOMPANY_DOT_VALIDATEDADDRESS = "ProviderCompany.ValidatedAddress";

        /// <summary>
        /// Asset Utility table lkpPaymentStatu
        /// </summary>
        public static String ASSET_UTILITY_LKPPAYMENTSTATU = "lkpPaymentStatu";

        /// <summary>
        /// Asset Utility table lkpPaymentType
        /// </summary>
        public static String ASSET_UTILITY_LKPPAYMENTTYPE = "lkpPaymentType";

        /// <summary>
        /// Asset Utility table AssetUnitUtility
        /// </summary>
        public static String ASSET_UTILITY_ASSETUNITUTILITY = "AssetUnitUtility";

        /// <summary>
        /// Asset Utility table AssetUnitUtility.UtilityType
        /// </summary>
        public static String ASSET_UTILITY_ASSETUNITUTILITY_DOT_UTILITYTYPE = "AssetUnitUtility.UtilityType";

        /// <summary>
        /// Asset Utility table AssetUnitUtility.lkpUtilityStatu
        /// </summary>
        public static String ASSET_UTILITY_ASSETUNITUTILITY_DOT_LKPUTILITYSTATU = "AssetUnitUtility.lkpUtilityStatu";

        /// <summary>
        /// Asset Utility table AssetUnitUtility.ProviderCompany
        /// </summary>
        public static String ASSET_UTILITY_ASSETUNITUTILITY_DOT_PROVIDERCOMPANY = "AssetUnitUtility.ProviderCompany";

        /// <summary>
        /// Asset Utility table AssetUnitUtility.ProviderCompany.ValidatedAddress
        /// </summary>
        public static String ASSET_UTILITY_ASSETUNITUTILITY_DOT_PROVIDERCOMPANY_DOT_VALIDATEDADDRESS = "AssetUnitUtility.ProviderCompany.ValidatedAddress";

        /// <summary>
        /// ProviderCompanies
        /// </summary>
        public static String ASSET_UTILITY_PROVIDERCOMPANIES = "ProviderCompanies";

        /// <summary>
        /// UtilityProvider
        /// </summary>
        public static String ASSET_UTILITY_UTILITYPROVIDER = "UtilityProvider";

        /// <summary>
        /// ProviderCompanyUtilityTypes
        /// </summary>
        public static String ASSET_UTILITY_PROVIDERCOMPANYUTILITYTYPES = "ProviderCompanyUtilityTypes";

        /// <summary>
        /// ProviderCompany.ValidatedAddress.ZipCode
        /// </summary>
        public static String ASSET_UTILITY_PROVIDERCOMPANY_VALIDATEDADDRESS_ZIPCODE = "ProviderCompany.ValidatedAddress.ZipCode";

        /// <summary>
        /// ProviderCompany.ValidatedAddress.ZipCode.City
        /// </summary>
        public static String ASSET_UTILITY_PROVIDERCOMPANY_VALIDATEDADDRESS_ZIPCODE_CITY = "ProviderCompany.ValidatedAddress.ZipCode.City";

        /// <summary>
        /// ProviderCompany.ValidatedAddress.ZipCode.City.State
        /// </summary>
        public static String ASSET_UTILITY_PROVIDERCOMPANY_VALIDATEDADDRESS_ZIPCODE_CITY_STATE = "ProviderCompany.ValidatedAddress.ZipCode.City.State";

        /// <summary>
        /// Suppliers
        /// </summary>
        public static String TABLE_SUPPLIERS = "Suppliers";

        /// <summary>
        /// Clients
        /// </summary>
        public static String TABLE_CLIENTS = "Clients";

        /// <summary>
        ///  ProviderCompany.UtilityProvider
        /// </summary>
        public static String TABLE_PROVIDERCOMPANY_DOT_UTILITYPROVIDER = "ProviderCompany.UtilityProvider";

        /// <summary>
        ///  Tenants
        /// </summary>
        public static String TABLE_TENANTS = "Tenants";

        /// <summary>
        ///  Zone
        /// </summary>
        public static String TABLE_ZONE = "Zone";

        /// <summary>
        ///  RateSchedule
        /// </summary>
        public static String TABLE_RATESCHEDULE = "RateSchedule";

        /// <summary>
        ///  AssetUnitUtilityPayments
        /// </summary>
        public static String TABLE_ASSETUNITUTILITYPAYMENTS = "AssetUnitUtilityPayments";

        /// <summary>
        ///  AssetUnitUtilities
        /// </summary>
        public static String TABLE_ASSETUNITUTILITIES = "AssetUnitUtilities";

        /// <summary>
        ///  AssetUnitUtilityPayments.lkpPaymentStatu
        /// </summary>
        public static String TABLE_ASSETUNITUTILITYPAYMENTS_LKPPAYMENTSTATU = "AssetUnitUtilityPayments.lkpPaymentStatu";

        /// <summary>
        ///  ProviderCompany.AddressHandle.Addresses.ZipCode.City.State
        /// </summary>
        public static String TABLE_PROVIDERCOMPANY_ADDRESSHANDLE_ADDRESSES_ZIPCODE_CITY_STATE = "ProviderCompany.AddressHandle.Addresses.ZipCode.City.State";

        /// <summary>
        ///  AssetUnitUtility.ProviderCompany.AddressHandle.Addresses.ZipCode.City.State
        /// </summary>
        public static String TABLE_ASSETUNITUTILITY_PROVIDERCOMPANY_ADDRESSHANDLE_ADDRESSES_ZIPCODE_CITY_STATE = "AssetUnitUtility.ProviderCompany.AddressHandle.Addresses.ZipCode.City.State";

        #region Rate Schedule

        /// <summary>
        ///  lkpItem
        /// </summary>
        public static String RATE_SCHEDULE_LKPITEM = "lkpItem";

        /// <summary>
        ///  lkpLegacyLOB
        /// </summary>
        public static String LEGACY_SUPPLIER_LKPLEGACYLOB = "lkpLegacyLOB";

        /// <summary>
        ///  lkpLegacyLOB
        /// </summary>
        public static String LEGACY_SUPPLIER_LKPLEGACYSYSTEM = "lkpLegacySystem";

        /// <summary>
        ///  lkpItem.lkpUnitOfMeasure
        /// </summary>
        public static String RATE_SCHEDULE_LKPITEM_DOT_LKPUNITOFMEASURE = "lkpItem.lkpUnitOfMeasure";

        /// <summary>
        ///  RateSchedule
        /// </summary>
        public static String RATE_SCHEDULE_RATESCHEDULE = "RateSchedule";

        /// <summary>
        ///  lkpItemCategory
        /// </summary>
        public static String RATE_SCHEDULE_LKPITEMCATEGORY = "lkpItemCategory";

        /// <summary>
        ///  lkpItemCategory.lkpItemCategory1
        /// </summary>
        public static String RATE_SCHEDULE_LKPITEMCATEGORY_DOT_LKPITEMCATEGORY1 = "lkpItemCategory.lkpItemCategory1";

        /// <summary>
        ///  lkpItemCategory.lkpItems
        /// </summary>
        public static String RATE_SCHEDULE_LKPITEMCATEGORY_DOT_LKPITEMS = "lkpItemCategory.lkpItems";

        /// <summary>
        ///  lkpUnitOfMeasure
        /// </summary>
        public static String RATE_SCHEDULE_LKPUNITOFMEASURE = "lkpUnitOfMeasure";

        /// <summary>
        ///  InspectionBlockID
        /// </summary>
        public static String RATE_SCHEDULE_INSPECTIONBLOCKID = "InspectionBlockID";

        /// <summary>
        ///  Anonymous, User
        /// </summary>
        public static String RATE_SCHEDULE_ANONYMOUS_USER = "Anonymous, User";

        #endregion

        #endregion

        #region PropertyUnitUtilityDetails

        /// <summary>
        /// Constant for AssetUnitUtilities.
        /// </summary>
        public static String TABLE_ASSET_UNIT_UTILITIES = "AssetUnitUtilities";

        /// <summary>
        /// Constant for AssetUnitUtilities.ProviderCompany
        /// </summary>
        public static String TABLE_ASSET_UNIT_UTILITIES_DOT_PROVIDER_COMPANY = "AssetUnitUtilities.ProviderCompany";

        /// <summary>
        /// Constant for AssetUnitUtilities.ProviderCompany.UtilityProvider
        /// </summary>
        public static String TABLE_ASSET_UNIT_UTILITIES_DOT_PROVIDER_COMPANY_DOT_UTILITY_PROVIDER = "AssetUnitUtilities.ProviderCompany.UtilityProvider";

        /// <summary>
        /// Constant for AssetUnitUtilities.UtilityType
        /// </summary>
        public static String TABLE_ASSET_UNIT_UTILITIES_DOT_UTILITY_TYPE = "AssetUnitUtilities.UtilityType";

        /// <summary>
        /// Constant for AssetUnitUtilities.AssetUnitUtilityDetails.
        /// </summary>
        public static String TABLE_ASSET_UNIT_UTILITIES_DOT_ASSET_UNIT_UTILITY_DETAILS = "AssetUnitUtilities.AssetUnitUtilityDetails";

        #endregion

        /// <summary>
        /// Constant for lkpAppliance.
        /// </summary>
        public static String TABLE_LKP_APPLIANCE = "lkpAppliance";

        /// <summary>
        /// Constant for lkpDwellingAmenity.
        /// </summary>
        public static String TABLE_LKP_DWELLING_AMENITY = "lkpDwellingAmenity";

        /// <summary>
        /// Constant for lkpAssetCondition.
        /// </summary>
        public static String TABLE_LKP_ASSET_CONDITION = "lkpAssetCondition";

        /// <summary>
        /// Constant for lkpAdditionalStructureType.
        /// </summary>
        public static String TABLE_LKP_ADDITIONAL_STRUCTURE_TYPE = "lkpAdditionalStructureType";

        /// <summary>
        /// Constant for AssetLoans.Loan.Client.ClientLSCIs.
        /// </summary>
        public static String TABLE_ASSET_LOANS_DOT_LOAN_DOT_CLIENT_DOT_CLIENT_LSCIS = "AssetLoans.Loan.Client.ClientLSCIs";

        /// <summary>
        /// Constant for Asset.AssetAdditionalStructures.
        /// </summary>
        public static String ASSET_DOT_ASSETADDITIONALSTRUCTURES = "Asset.AssetAdditionalStructures";

        /// <summary>
        /// Constant for AssetInspectionDamages.
        /// </summary>
        public static String TABLE_ASSET_INSPECTION_DAMAGES = "AssetInspectionDamages";

        /// <summary>
        /// Constant for AssetInspectionDamages.AssetInspectionDamageEvidences.
        /// </summary>
        public static String TABLE_ASSET_INSPECTION_DAMAGES_DOT_ASSET_INSPECTION_DAMAGE_EVIDENCES = "AssetInspectionDamages.AssetInspectionDamageEvidences";

        /// <summary>
        /// Constant for ProviderCompanyUtilityTypes.ProviderCompany.
        /// </summary>
        public static String TABLE_PROVIDERCOMPANYUTILITYTYPES_PROVIDERCOMPANY = "ProviderCompanyUtilityTypes.ProviderCompany";

        /// <summary>
        /// Constant for AssetAdditionalStructures.lkpAdditionalStructureType.
        /// </summary>
        public static String TABLE_ASSETADDITIONALSTRUCTURES_DOT_LKPADDITIONALSTRUCTURETYPE = "AssetAdditionalStructures.lkpAdditionalStructureType";

        /// <summary>
        /// Constant for AssetAdditionalStructures.lkpAssetCondition.
        /// </summary>
        public static String TABLE_ASSETADDITIONALSTRUCTURES_DOT_LKPASSETCONDITION = "AssetAdditionalStructures.lkpAssetCondition";

        /// <summary>
        /// lkpRegion
        /// </summary>
        public static String TABLE_ClientFrequentInspectionScheduleLoanType = "ClientFrequentInspectionScheduleLoanTypes";

        /// <summary>
        /// Constant for AssetLoans.Loan.lkpClientDefaultStatusType
        /// </summary>
        public static String TABLE_ASSET_LOANS_DOT_LOAN_DOT_LKP_CLIENT_DEFAULT_STATUS_TYPE = "AssetLoans.Loan.lkpClientDefaultStatusType";

        /// <summary>
        /// Constant for AssetLoans.Loan.Liens.lkpLienPosition
        /// </summary>
        public static String TABLE_ASSETLOANS_DOT_LOAN_DOT_LIENS_DOT_LKPLIENPOSITION = "AssetLoans.Loan.Liens.lkpLienPosition";

        /// <summary>
        /// Constant for Loan.Liens
        /// </summary>
        public static String TABLE_LOAN_DOT_LIENS = "Loan.Liens";

        /// <summary>
        /// NonContributor.Contact.ContactDetails.lkpContactType
        /// </summary>
        public static String TABLE_NONCONTRIBUTOR_CONTACT_CONTACTDETAILS_LKPCONTACTTYPE = "NonContributor.Contact.ContactDetails.lkpContactType";

        #endregion

        #region Bids EyeballEstimation
        /// <summary>
        ///  lkpDamageType
        /// </summary>
        public static String TABLE_LKP_DAMAGETYPE = "lkpDamageType";

        /// <summary>
        ///  lkpDamageLocation
        /// </summary>
        public static String TABLE_LKP_DAMAGELOCATION = "lkpDamageLocation";

        #endregion

        #region Bids Management

        /// <summary>
        ///  AssetDamage
        /// </summary>
        public static String TABLE_ASSETDAMAGE = "AssetDamage";

        /// <summary>
        ///  AssetDamageBidGroup
        /// </summary>
        public static String TABLE_ASSETDAMAGEBIDGROUP = "AssetDamageBidGroup";

        /// <summary>
        ///  AssetDamageBidGroup
        /// </summary>
        public static String TABLE_BIDGROUP = "AssetDamageBidGroup";

        /// <summary>
        ///  AssetDamage.AssetDamageBidGroups.BidGroup   
        /// </summary>
        public static String TABLE_ASSETDAMAGE_DOT_ASSETDAMAGEBIDGROUP_DOT_BIDGROUP = "AssetDamage.AssetDamageBidGroups.BidGroup";

        /// <summary>
        ///  AssetDamage.AssetDamageBidGroups.BidGroup.Bids   
        /// </summary>
        public static String TABLE_ASSETDAMAGE_DOT_ASSETDAMAGEBIDGROUP_DOT_BIDGROUP_DOT_BIDS = "AssetDamage.AssetDamageBidGroups.BidGroup.Bids";

        /// <summary>
        /// AssetDamage.EyeBallEstimations   
        /// </summary>
        public static String TABLE_ASSETDAMAGE_DOT_EYEBALLESTIMATIONS = "AssetDamage.EyeBallEstimations";

        /// <summary>
        ///  AssetDamage.AssetDamageBidGroups.BidGroup   
        /// </summary>
        public static String TABLE_ASSETDAMAGEBIDGROUPS = "AssetDamageBidGroups";

        /// <summary>
        ///  AssetDamage.ACFAssetDamages
        /// </summary>
        public static String TABLE_ASSETDAMAGE_DOT_ACFASSETDAMAGES = "AssetDamage.ACFAssetDamages";


        /// <summary>
        ///  ACFAssetDamages.AssetDamage
        /// </summary>
        public static String TABLE_ACFASSETDAMAGES_DOT_ASSETDAMAGE = "ACFAssetDamages.AssetDamage";

        /// <summary>
        ///  Bid
        /// </summary>
        public static String TABLE_BID = "Bid";

        /// <summary>
        ///  Bids
        /// </summary>
        public static String TABLE_BIDS = "Bids";

        /// <summary>
        ///  Bids.BidItems
        /// </summary>
        public static String TABLE_BIDS_DOT_BIDITEMS = "Bids.BidItems";

        /// <summary>
        /// BidGroup.Bids
        /// </summary>
        public static string TABLE_BID_GROUP_DOT_BID = "BidGroup.Bids";

        #endregion

        /// <summary>
        /// lkpInvoiceDepartment
        /// </summary>
        public static String TABLE_CLIENT_LKPINVOICEDEPARTMENT = "lkpInvoiceDepartment";

        /// <summary>
        /// lkpInvoicePlatform
        /// </summary>
        public static String TABLE_CLIENT_LKPINVOICEPLATFORM = "lkpInvoicePlatform";

        /// <summary>
        /// lkpInvoiceType
        /// </summary>
        public static String TABLE_CLIENT_LKPINVOICETYPE = "lkpInvoiceType";

        /// <summary>
        /// lkpInvoiceType
        /// </summary>
        public static String TABLE_CLIENT_LKPINVOICEVERSION = "lkpInvoiceVersion";

        /// <summary>
        /// Constant for AssetLoans.Loan.Bankruptcies
        /// </summary>
        public static String TABLE_ASSETLOANS_LOAN_BANKRUPTCIES = "AssetLoans.Loan.Bankruptcies";

        /// <summary>
        /// Constant for AssetLoans.Loan.FHALoanDetails
        /// </summary>
        public static String TABLE_ASSETLOANS_LOAN_FHALOANDETAILS = "AssetLoans.Loan.FHALoanDetails";

        #region Asset Emergency

        /// <summary>
        /// Constant for lkpAreaOfDamage
        /// </summary>
        public static String TABLE_LKPAREAOFDAMAGE = "lkpAreaOfDamage";

        /// <summary>
        /// Constant for lkpAssetEmergencyPriority
        /// </summary>
        public static String TABLE_LKPASSETEMERGENCYPRIORITY = "lkpAssetEmergencyPriority";

        /// <summary>
        /// Constant for lkpAssetEmergencyStatu
        /// </summary>
        public static String TABLE_LKPASSETEMERGENCYSTATU = "lkpAssetEmergencyStatu";

        /// <summary>
        /// Constant for lkpEmergencyImpact
        /// </summary>
        public static String TABLE_LKPEMERGENCYIMPACT = "lkpEmergencyImpact";

        /// <summary>
        /// Constant for lkpEmergencyReportedBy
        /// </summary>
        public static String TABLE_LKPEMERGENCYREPORTEDBY = "lkpEmergencyReportedBy";

        /// <summary>
        /// Constant for lkpEmergencyUrgency
        /// </summary>
        public static String TABLE_LKPEMERGENCYURGENCY = "lkpEmergencyUrgency";

        /// <summary>
        /// Constant for lkpStatu
        /// </summary>
        public static String TABLE_LKPSTATU = "lkpStatu";

        /// <summary>
        /// Constant for AssetEmergencyBidGroups
        /// </summary>
        public static String TABLE_ASSET_EMERGENCY_BID_GROUP = "AssetEmergencyBidGroups";

        #endregion

        /// <summary>
        /// SupplierLegacySuppliers
        /// </summary>
        public static String TABLE_SUPPLIER_LEGACY_SUPPLIERS = "SupplierLegacySuppliers";

        /// <summary>
        /// AssetEmergencyBidGroups.BidGroup
        /// </summary>
        public static String TABLE_ASSETEMEGENCYBIDGROUP_DOT_BIDGROUP = "AssetEmergencyBidGroups.BidGroup.Bids";

        #region MESSAGE RULES

        /// <summary>
        /// MessageRuleLocations.MessageRuleUserLocations for Message Rules
        /// </summary>
        public static String TABLE_MESSAGERULELOCATIONS_DOT_MESSAGERULEUSERLOCATIONS = "MessageRuleLocations.MessageRuleUserLocations";

        /// <summary>
        /// MessageRuleLocations.MessageRuleUserLocations for Message Rules
        /// </summary>
        public static String TABLE_PROGRAM = "Program";

        #endregion

        public static String TABLE_LKP_COMMUNICATION_EVENT = "lkpCommunicationEvent";

        public static String TABLE_LKP_COMMUNICATION_TYPE = "lkpCommunicationType";

        public static String TABLE_COMMUNICATION_TEMPLATE_PLACEHOLDER = "CommunicationTemplatePlaceHolder";

        public static String TABLE_SYSTEM_COMMUNICATION = "SystemCommunication";

        /// <summary>
        /// Constant for ResidentialHistories.
        /// </summary>
        public static String TABLE_RESIDENTIALHISTORIES = "ResidentialHistories";

        /// <summary>
        /// Constant for Address.
        /// </summary>
        public static String TABLE_RH_ADDRESS = "Address";

        /// <summary>
        /// Constant for Address.ZipCode.
        /// </summary>
        public static String TABLE_RH_ADDRESS_ZIPCODE = "Address.ZipCode";

        /// <summary>
        /// Constant for Address.ZipCode.City.
        /// </summary>
        public static String TABLE_RH_ADDRESS_ZIPCODE_CITY = "Address.ZipCode.City";

        /// <summary>
        /// Constant for Address.ZipCode.County.State.
        /// </summary>
        public static String TABLE_RH_ADDRESS_ZIPCODE_COUNTY_STATE = "Address.ZipCode.County.State";

        #region MyRegion

        public static String APPLICANT_ATTRIBUTEDATA_DOCUMENTMAP = "ApplicantComplianceAttributeDatas.ApplicantComplianceDocumentMaps";
        public static String APPLICANT_COMPLIANCE_ITEMDATA = "ApplicantComplianceItemData";

        #endregion
    }
}
