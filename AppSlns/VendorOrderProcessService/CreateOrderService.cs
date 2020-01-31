#region Header Comment Block

// 
// Copyright 2014 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  CreateOrderService.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

#endregion

#region Application Specific
using Business.RepoManagers;
using Entity;
using ExternalVendors;
using INTSOF.ServiceUtil;
using INTSOF.Utils;
using Entity.ClientEntity;
using Entity.ExternalVendorContracts;
using System.Text.RegularExpressions;
using INTSOF.UI.Contract.Services;

#endregion

#endregion

namespace VendorOrderProcessService
{
    public static class CreateOrderService
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static Int32 _recordChunkSize;
        private static Int32 _maxRetryCount;
        private static Int32 _retryTimeLag;
        private static Boolean _testModeOn;
        private static Boolean _IsNotificationRequestOn;
        private static String CreateOrderServiceLogger;

        private static String _loginID;
        private static String _password;
        private static Int32 _businessOwnerID;
        private static String _adbTestAccountForAMS;
        private static Boolean _useADBTestAccountForAMS;
        private static Boolean _isServiceLoggingEnabled;

        private static String _clearStarCCFUserName;
        private static String _clearStarCCFPassword;

        private static Guid LCSAttCode = new Guid("1ADA97AE-9100-4BE6-B829-C914B7FA8750");//Driver's License State
        private static Guid LCNAttCode = new Guid("515BEF57-9072-4D2A-A97A-0C248BB045F9");////License Number

        #region UAT-2169:Send Middle Name and Email address to clearstar in Complio
        private static String _noMiddleNameText;
        #endregion
        #region UAT-2254:Complio: Use CreateProfileForCountry API to create profile instead of CreateProfile which is being used in all three system to create profile.
        private static Int32 _USACountryID = 233;
        #endregion
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Constructor

        static CreateOrderService()
        {
            CreateOrderServiceLogger = "CreateOrderServiceLogger";

            if (ConfigurationManager.AppSettings["RecordChunkSize"].IsNotNull())
            {
                _recordChunkSize = Convert.ToInt32(ConfigurationManager.AppSettings["RecordChunkSize"]);
            }
            else
            {
                _recordChunkSize = AppConsts.CHUNK_SIZE_FOR_CREATE_ORDER_SERVICE;
            }
            if (ConfigurationManager.AppSettings["MaxRetryCount"].IsNotNull())
            {
                _maxRetryCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaxRetryCount"]);
            }
            else
            {
                _maxRetryCount = AppConsts.MAX_RETRY_COUNT_FOR_CREATE_ORDER_SERVICE;
            }
            if (ConfigurationManager.AppSettings["RetryTimeLag"].IsNotNull())
            {
                _retryTimeLag = Convert.ToInt32(ConfigurationManager.AppSettings["RetryTimeLag"]);
            }
            else
            {
                _retryTimeLag = AppConsts.RETRY_TIME_LAG_FOR_CREATE_ORDER_SERVICE;
            }

            if (ConfigurationManager.AppSettings["TestModeOn"].IsNotNull())
            {
                _testModeOn = Convert.ToBoolean(ConfigurationManager.AppSettings["TestModeOn"]);
            }
            else
            {
                _testModeOn = false;
            }

            if (ConfigurationManager.AppSettings["IsNotificationRequestOn"].IsNotNull())
            {
                _IsNotificationRequestOn = Convert.ToBoolean(ConfigurationManager.AppSettings["IsNotificationRequestOn"]);
            }
            else
            {
                _IsNotificationRequestOn = false;
            }

            if (ConfigurationManager.AppSettings["ClearStarLoginID"].IsNotNull())
            {
                _loginID = Convert.ToString(ConfigurationManager.AppSettings["ClearStarLoginID"]);
            }
            else
            {
                _loginID = AppConsts.VENDOR_LOGIN_ID;
            }
            if (ConfigurationManager.AppSettings["ClearStarPassword"].IsNotNull())
            {
                _password = Convert.ToString(ConfigurationManager.AppSettings["ClearStarPassword"]);
            }
            else
            {
                _password = AppConsts.VENDOR_PASSWORD;
            }
            if (ConfigurationManager.AppSettings["BusinessOwnerID"].IsNotNull())
            {
                _businessOwnerID = Convert.ToInt32(ConfigurationManager.AppSettings["BusinessOwnerID"]);
            }
            else
            {
                _businessOwnerID = AppConsts.VENDOR_BUSINESS_OWNER_ID;
            }

            if (ConfigurationManager.AppSettings["UseADBTestAccountForAMS"].IsNotNull())
            {
                _useADBTestAccountForAMS = Convert.ToBoolean(ConfigurationManager.AppSettings["UseADBTestAccountForAMS"]);
            }
            else
            {
                _useADBTestAccountForAMS = AppConsts.USE_ADB_TEST_ACCOUNT_FOR_AMS;
            }
            if (ConfigurationManager.AppSettings["ADBTestAccountForAMS"].IsNotNull())
            {
                _adbTestAccountForAMS = Convert.ToString(ConfigurationManager.AppSettings["ADBTestAccountForAMS"]);
            }
            else
            {
                _adbTestAccountForAMS = AppConsts.ADB_TEST_ACCOUNT_FOR_AMS;
            }
            if (ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull())
            {
                _isServiceLoggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]);
            }
            else
            {
                _isServiceLoggingEnabled = false;
            }
            //UAT-2169:Send Middle Name and Email address to clearstar in Complio
            if (ConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY].IsNotNull())
            {
                _noMiddleNameText = ConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY];
            }
            else
            {
                _noMiddleNameText = String.Empty;
            }
            if (ConfigurationManager.AppSettings["ClearStarCCFUserName"].IsNotNull())
            {
                _clearStarCCFUserName = Convert.ToString(ConfigurationManager.AppSettings["ClearStarCCFUserName"]);
            }
            if (ConfigurationManager.AppSettings["ClearStarCCFPassword"].IsNotNull())
            {
                _clearStarCCFPassword = Convert.ToString(ConfigurationManager.AppSettings["ClearStarCCFPassword"]);
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// This method takes the chunk of orders from the database and sends them 
        /// to corresponding external vendor for further processing.
        /// </summary>
        /// <param name="tenant_Id">If not null orders for specific tenant will be processed
        /// else orders for all tenants will be processed.</param>
        public static void SendExternalVendorOrders(Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                ServiceLogger.Info("Calling GetExternalVendorOrders: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
                List<State> states = LookupManager.GetAllStates();
                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Entity.Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    ServiceLogger.Debug<Int32?>("TenantID for which external vendor orders are to be dispatched:", tenant_Id, CreateOrderServiceLogger);
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                ServiceLogger.Info("Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDBConnection.TestConnString(clntDbConf.CDB_ConnectionString, CreateOrderServiceLogger))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            ServiceLogger.Info("Processing the chunk of Order Line Item records: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
                            List<OrderToBeDisptached> lstOrderLineItemResult = ExternalVendorOrderManager.GetOrdersToBeDispatchedToVendor(tenantId, _recordChunkSize,
                                                                               _maxRetryCount, _retryTimeLag, _testModeOn).ToList();
                            if (_testModeOn)
                            {
                                ServiceLogger.Info("Fetching test mode orders from db: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
                                List<Int32> testModeOrderIDs = ExternalVendorOrderManager.GetTestModeBkgOrders(tenantId);
                                ServiceLogger.Info("Total Orders from Test Mode: " + testModeOrderIDs.Count(), CreateOrderServiceLogger);
                                lstOrderLineItemResult = lstOrderLineItemResult.IsNotNull()
                                    ? lstOrderLineItemResult.Where(cond => testModeOrderIDs.Contains(cond.BkgOrderID)).ToList() : null;
                                ServiceLogger.Info("Fetched test mode orders from db: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
                            }

                            if (lstOrderLineItemResult.IsNotNull() && lstOrderLineItemResult.Count() > AppConsts.NONE)
                            {
                                ServiceLogger.Info("Total OrderToBeDisptached Records found: " + lstOrderLineItemResult.Count(), CreateOrderServiceLogger);
                                ServiceLogger.Info("Started foreach loop on OrderToBeDisptached based on vendor id: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
                                lstOrderLineItemResult.Where(x => x.VendorID.HasValue).Select(col => col.VendorID.Value).Distinct().ForEach(vendorID =>
                                {
                                    ServiceLogger.Debug<Int32>("Next Vendor ID from the loop:", vendorID, CreateOrderServiceLogger);
                                    ExternalVendor vendor = LookupManager.GetExternalVendorsForAMS().FirstOrDefault(cond => cond.EVE_ID == vendorID);
                                    ServiceLogger.Debug<ExternalVendor>("Vendor corresponding to vendor ID:", vendor, CreateOrderServiceLogger);
                                    if (vendor.IsNotNull())
                                    {
                                        String vendorName = vendor.EVE_Name;
                                        ServiceLogger.Debug<String>("Name of the vendor:", vendorName, CreateOrderServiceLogger);

                                        if (ExternalVendorUnityHelper.VendorServiceAdapter(vendorName).States.IsNull())
                                        {
                                            ExternalVendorUnityHelper.VendorServiceAdapter(vendorName).States = states;
                                        }

                                        ServiceLogger.Info("Started foreach loop on OrderToBeDisptached based on Order ID: "
                                                            + DateTime.Now.ToString(), CreateOrderServiceLogger);

                                        lstOrderLineItemResult.Where(cond => cond.VendorID == vendorID).Select(col => col.BkgOrderID).Distinct().ForEach(orderID =>
                                        {
                                            ServiceLogger.Debug<Int32>("Next Order ID from the loop:", orderID, CreateOrderServiceLogger);
                                            CreateExternalVendorOrders(lstOrderLineItemResult.Where(cond => cond.VendorID == vendorID && cond.BkgOrderID == orderID),
                                                                            tenantId, vendorName, _testModeOn);
                                        });

                                        ServiceLogger.Info("Ended foreach loop on OrderToBeDisptached based on order id: " + DateTime.Now.ToString(),
                                        CreateOrderServiceLogger);
                                    }
                                });
                                ServiceLogger.Info("Ended foreach loop on OrderToBeDisptached based on Vendor ID: " + AppConsts.NONE
                                    + " at:" + DateTime.Now.ToString(), CreateOrderServiceLogger);
                                ServiceContext.ReleaseDBContextItems();
                                executeLoop = true;
                            }
                            else
                            {
                                executeLoop = false;
                                ServiceContext.ReleaseDBContextItems();
                            }
                            ServiceLogger.Info("Processed the chunk of Order Line Item records: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BKG_ORDER_SERVICE_USER_ID);
                            Int32 bkgOrderServiceUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BKG_ORDER_SERVICE_USER_ID_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendExternalVendorOrders.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = bkgOrderServiceUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }

                    ServiceContext.ReleaseDBContextItems();
                }

                ServiceLogger.Info("Ended foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in GetExternalVendorOrders method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString), CreateOrderServiceLogger);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Fetches the order line items for database and dispatchs them to External Vendor for further processing.
        /// </summary>
        private static void CreateExternalVendorOrders(IEnumerable<OrderToBeDisptached> orderItemResult, Int32 tenantID, String vendorName, Boolean isTestModeON)
        {
            if (orderItemResult.IsNotNull())
            {
                EvCreateOrderContract eVOrderContract = new EvCreateOrderContract();
                try
                {
                    EvCreateOrderContract translatedContract = TranslateOrderFields(orderItemResult, tenantID, isTestModeON);
                    if (translatedContract.IsNotNull())
                    {
                        ServiceLogger.Info("Dispatching Service Line Items to External Vendor.", CreateOrderServiceLogger);
                        eVOrderContract = ExternalVendorUnityHelper.VendorServiceAdapter(vendorName).DispatchOrderItemsToVendor(translatedContract, tenantID, isTestModeON);
                        ServiceLogger.Debug<EvCreateOrderContract>("Order Contract after being dispatched to external vendor:", eVOrderContract, CreateOrderServiceLogger);
                    }
                    else
                    {
                        Int32 bkgOrderPackageSvcGroupID = orderItemResult.FirstOrNew().BkgOrderPackageSvcGroupID;
                        ServiceLogger.Info("Translated Contract is null for BkgOrderPackageSvcGroupID: " + bkgOrderPackageSvcGroupID + ". Return to next Service Group.", CreateOrderServiceLogger);
                        ServiceLogger.Debug<List<OrderToBeDisptached>>("Order Contract after being dispatched to external vendor:", orderItemResult.ToList(), CreateOrderServiceLogger);
                        return;
                    }
                }
                //No network connection
                catch (System.Net.WebException webExc)
                {
                    ServiceLogger.Info("WebException Occurred while Dispatching Service Line Items to External Vendor.", CreateOrderServiceLogger);
                    eVOrderContract.PackageSvcGroups.ForEach(packageSvcGroup =>
                    {
                        if (!String.IsNullOrWhiteSpace(packageSvcGroup.BkgOrderVendorProfileID))
                        {
                            ExternalVendorUnityHelper.VendorServiceAdapter(vendorName).RevertExternalVendorChanges(eVOrderContract.LoginName, eVOrderContract.Password,
                            eVOrderContract.BusinessOwnerID, eVOrderContract.AccountNumber, packageSvcGroup.BkgOrderVendorProfileID, tenantID,
                            eVOrderContract.BkgOrderID, packageSvcGroup.BkgOrderPkgSvcGroupID);
                        }
                    });
                    throw webExc;
                }

                try
                {
                    ServiceLogger.Info("Saving Vendor Order data to the database.", CreateOrderServiceLogger);
                    ExternalVendorOrderManager.SaveVendorOrderData(eVOrderContract, tenantID, _IsNotificationRequestOn);
                    ServiceLogger.Info("Saved Vendor Order data to the database.", CreateOrderServiceLogger);
                }
                catch (Exception ex)
                {
                    ServiceLogger.Info("ERROR in Saving Vendor Order Data to the database: Updating retry count of service line items to the database.", CreateOrderServiceLogger);

                    List<Int32> cancelledLinetItems = new List<Int32>();

                    ServiceContext.ReleaseDBContextItems();

                    eVOrderContract.PackageSvcGroups.ForEach(packageSvcGroup =>
                        {
                            if (!String.IsNullOrWhiteSpace(packageSvcGroup.BkgOrderVendorProfileID))
                            {
                                ExternalVendorUnityHelper.VendorServiceAdapter(vendorName).RevertExternalVendorChanges(eVOrderContract.LoginName, eVOrderContract.Password,
                                eVOrderContract.BusinessOwnerID, eVOrderContract.AccountNumber, packageSvcGroup.BkgOrderVendorProfileID, tenantID,
                                eVOrderContract.BkgOrderID, packageSvcGroup.BkgOrderPkgSvcGroupID);
                            }
                            cancelledLinetItems.AddRange(packageSvcGroup.OrderItems.Select(col => col.BkgOrderPackageSvcLineItemID));
                        });

                    ServiceLogger.Error(String.Format("An Error has occured in CreateExternalVendorOrders method while saving the results, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                            ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString), CreateOrderServiceLogger);
                    ExternalVendorOrderManager.UpdateRetryCountForBkgOrderLineItems(tenantID, cancelledLinetItems);
                    ServiceLogger.Info("ERROR in Saving Vendor Order Data to the database: Updated retry count of service line items to the database.", CreateOrderServiceLogger);
                }
                ServiceLogger.Info("Dispatched Order Items to External Vendor: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
            }
            else
            {
                ServiceLogger.Info("OrderItemResult list is empty. Return to next Order.", CreateOrderServiceLogger);
            }
        }

        /// <summary>
        /// Translate the order line item data to external vendor contract form.
        /// </summary>
        /// <param name="orderLineItems">collection of orderLineItems</param>
        /// <param name="tenantID"></param>
        /// <param name="isTestModeON"></param>
        /// <param name="orderAttributes"></param>
        /// <returns></returns>
        private static EvCreateOrderContract TranslateOrderFields(IEnumerable<OrderToBeDisptached> orderLineItems, Int32 tenantID, Boolean isTestModeON)
        {
            try
            {
                ServiceLogger.Info("Entering TranslateOrderFields: " + DateTime.Now.ToString(), CreateOrderServiceLogger);

                OrderToBeDisptached orderToBeDisptached = orderLineItems.FirstOrDefault();

                EvCreateOrderContract orderContract = new EvCreateOrderContract
                {
                    LoginName = _loginID,
                    Password = _password,
                    BusinessOwnerID = _businessOwnerID,
                    UseADBTestAccountForAMS = _useADBTestAccountForAMS,
                    ADBTestAccountForAMS = _adbTestAccountForAMS,
                    BkgOrderID = orderToBeDisptached.BkgOrderID,
                    //BkgOrderPkgSvcGroupID = orderToBeDisptached.BkgOrderPackageSvcGroupID,
                    AliasFirstName = orderToBeDisptached.AliasFirstName,
                    AliasLastName = orderToBeDisptached.AliasLastName,
                    AliasMiddleName = orderToBeDisptached.AliasMiddleName,
                    Address1 = orderToBeDisptached.Address1,
                    Address2 = orderToBeDisptached.Address2,
                    DateOfBirth = orderToBeDisptached.DateOfBirth,
                    FirstName = isTestModeON ? String.Concat("Test_", orderToBeDisptached.FirstName) : orderToBeDisptached.FirstName,
                    LastName = orderToBeDisptached.LastName,
                    MiddleName = orderToBeDisptached.MiddleName,
                    PhoneNumber = GetFormattedPhoneNumber(orderToBeDisptached.PhoneNumber), //Getting formatted PhoneNumber
                    SSN = GetFormattedSSN(orderToBeDisptached.SSN), //Getting formatted SSN
                    //TransmitInd = orderLineItems.Where(cond => cond.TransmitInd.HasValue).All(col => col.TransmitInd.Value),
                    VendorID = orderToBeDisptached.VendorID.Value,
                    City = orderToBeDisptached.City,
                    State = orderToBeDisptached.State,
                    ZipCode = orderToBeDisptached.ZipCode,
                    Country = orderToBeDisptached.Country,
                    County = orderToBeDisptached.County,
                    InstitutionName = orderToBeDisptached.InstitutionName,
                    AccountNumber = orderToBeDisptached.AccountNumber,
                    AccountName = orderToBeDisptached.AccountName,
                    Gender = orderToBeDisptached.Gender,
                    OrganizationUserProfileID = orderToBeDisptached.OrganizationUserProfileID,
                    OrderID = orderToBeDisptached.OrderID,
                    EmailAddress = orderToBeDisptached.EmailAddress,
                    PackageSvcGroups = new List<EvCreateOrderSvcGroupContract>(),
                    //implemented changes related to production issue for aliases at the time of supplementOrder[19/07/2016]
                    OrderProfileAliases = new List<EvOrderProfileAliases>(),
                    //UAT-2169:Send Middle Name and Email address to clearstar in Complio
                    NoMiddleNameDefaultText = _noMiddleNameText,
                    // UAT-2254:Complio: Use CreateProfileForCountry API to create profile instead of CreateProfile which is being used in all three system to create profile.
                    ClearStarCountryID = orderToBeDisptached.ClearStarCountryID > AppConsts.NONE ? orderToBeDisptached.ClearStarCountryID : _USACountryID,
                    ClearStarCCFUserName = _clearStarCCFUserName.IsNotNull() ? _clearStarCCFUserName : String.Empty,
                    ClearStarCCFPassword = _clearStarCCFPassword.IsNotNull() ? _clearStarCCFPassword : String.Empty,
                    //Admin Entry Portal Changes for profile suffix
                    ProfileSuffix = orderToBeDisptached.ProfileSuffix
                };

                //implemented changes related to production issue for aliases at the time of supplementOrder[19/07/2016]
                orderContract.OrderProfileAliases = ExternalVendorOrderManager.GetOrderProfileAliases(tenantID, orderToBeDisptached.OrderID
                                                                                                    , orderToBeDisptached.OrganizationUserProfileID, _noMiddleNameText);
                List<OrderAttribute> orderAttributes = ExternalVendorOrderManager.GetAttributesForOrder(orderToBeDisptached.BkgOrderID, tenantID).ToList();
                //UAT-2337
                ExternalVendorOrderManager.UpdateEndDateForCurrentEmployer(orderAttributes, tenantID);

                //UAT-2893:Update country code sent to clearstar for international criminals
                orderContract.lstCountries = ExternalVendorOrderManager.GetCountriesLookupData();
                
                orderLineItems.Select(col => col.BkgOrderPackageSvcGroupID).Distinct().ForEach(svcGroupId =>
                {
                    IEnumerable<OrderToBeDisptached> svcGroupLines = orderLineItems.Where(cond => cond.BkgOrderPackageSvcGroupID.Equals(svcGroupId));

                    EvCreateOrderSvcGroupContract packageSvcGroup = new EvCreateOrderSvcGroupContract
                    {
                        BkgOrderPkgSvcGroupID = svcGroupId,
                        TransmitInd = svcGroupLines.Where(cond => cond.TransmitInd.HasValue).All(col => col.TransmitInd.Value),
                        OrderItems = svcGroupLines.Select(item => new EvOrderItemContract
                                                   {
                                                       ExternalBackgroundServiceCode = item.ExternalServiceCode,
                                                       BkgOrderPackageSvcLineItemID = item.OrderLineItemID,
                                                       BackgroundServiceID = item.BackgroundServiceID,
                                                       BkgSvcTypeID = item.ServiceTypeID,
                                                       BkgSvcTypeCode = item.BkgSvcTypeCode,
                                                       ExternalServiceID = item.ExternalServiceID
                                                   }).ToList(),
                        //UAT-1707:Supplement process, don't create new profile, only add orders to existing profile
                        //To Identify that order is supplement order or normal order
                        IsSupplement = svcGroupLines.FirstOrDefault().IsSupplementOrder
                    };

                    ExternalVendorOrderManager.PopulateOrderWithAttributes(packageSvcGroup, tenantID, orderAttributes);
                    orderContract.PackageSvcGroups.Add(packageSvcGroup);
                });

                ServiceLogger.Info("Returning translated fields from TranslateOrderFields: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
                return orderContract;
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in TranslateOrderFields method while translating the results to vendor contract,"
                                                  + " the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message
                                                  , ex.InnerException, ex.StackTrace + " current context key : "
                                                  + ServiceContext.currentThreadContextKeyString), CreateOrderServiceLogger);
                return null;
            }
        }

        private static String GetFormattedSSN(String unformattedSSN)
        {
            try
            {
                if (unformattedSSN == null)
                {
                    ServiceLogger.Info("SSN Found to be null: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
                    return String.Empty;
                }
                string value = unformattedSSN;
                Regex re = new Regex(@"(\d\d\d)(\d\d)(\d\d\d\d)");
                if (re.IsMatch(unformattedSSN))
                {
                    Match match = re.Match(unformattedSSN);
                    value = match.Groups[1] + "-" + match.Groups[2] + "-" + match.Groups[3];
                }
                return value;
            }
            catch (Exception)
            {
                ServiceLogger.Info("Returning Formatted SSN as ###-##-####: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
                return null;
            }
        }

        public static string GetFormattedPhoneNumber(string unformattedPhoneNumber)
        {
            try
            {
                if (unformattedPhoneNumber == null)
                {
                    return String.Empty;
                }
                string value = Regex.Replace(unformattedPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3");
                return value;
            }
            catch (SysXException ex)
            {
                ServiceLogger.Info("Returning Formatted Phone Number as (###)-##-####: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
                return null;
            }
        }
        #endregion

        #endregion
    }
}