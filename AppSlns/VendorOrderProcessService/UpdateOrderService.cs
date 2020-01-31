#region Header Comment Block

// 
// Copyright 2014 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  UpdateOrderService.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

#endregion

#region Application Specific
using INTSOF.ServiceUtil;
using ExternalVendors;
using INTSOF.Utils;
using Entity.ExternalVendorContracts;
using Entity;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.Services;
#endregion

#endregion

namespace VendorOrderProcessService
{
    public static class UpdateOrderService
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static Int32 _recordChunkSize;
        private static Boolean _updateAllInProcessOrders;
        private static Int32 _updateAllInProcessOrdersOnOrAfter;
        private static Boolean _updateEXInProcessOrders;
        private static Boolean _updateAMSInProcessOrders;
        private static Int32 _startingOrderID;
        private static Int32 _endingOrderID;
        private static Int32 _updateOrderRecentDays;
        private static Int32 _updateOrderRetryTimeLag;
        private static String UpdateOrderServiceLogger;
        private static String _loginID;
        private static String _password;
        private static Int32 _businessOwnerID;
        private static Boolean _isServiceLoggingEnabled;

        #region UAT-4162 :-

        private static Int32 _retryDSOrdersChunkSize;
        private static Int32 _retryDSOrderRetryTimeLag;
        private static Int32 _maxRetryCountForDSOrders;
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

        static UpdateOrderService()
        {
            if (ConfigurationManager.AppSettings["RecordChunkSize"].IsNotNull())
            {
                _recordChunkSize = Convert.ToInt32(ConfigurationManager.AppSettings["RecordChunkSize"]);
            }
            else
            {
                _recordChunkSize = AppConsts.CHUNK_SIZE_FOR_CREATE_ORDER_SERVICE;
            }

            if (ConfigurationManager.AppSettings["UpdateAllInProcessOrders"].IsNotNull())
            {
                _updateAllInProcessOrders = Convert.ToBoolean(ConfigurationManager.AppSettings["UpdateAllInProcessOrders"]);
            }
            else
            {
                _updateAllInProcessOrders = AppConsts.UPDATE_ALL_INPROCESS_ORDERS;
            }

            if (ConfigurationManager.AppSettings["UpdateAllInProcessOrdersOnOrAfter"].IsNotNull())
            {
                _updateAllInProcessOrdersOnOrAfter = Convert.ToInt32(ConfigurationManager.AppSettings["UpdateAllInProcessOrdersOnOrAfter"]);
            }
            else
            {
                _updateAllInProcessOrdersOnOrAfter = AppConsts.UPDATE_ALL_INPROCESS_ORDERS_ON_OR_AFTER;
            }

            if (ConfigurationManager.AppSettings["UpdateEXInProcessOrders"].IsNotNull())
            {
                _updateEXInProcessOrders = Convert.ToBoolean(ConfigurationManager.AppSettings["UpdateEXInProcessOrders"]);
            }
            else
            {
                _updateEXInProcessOrders = AppConsts.UPDATE_EX_INPROCESS_ORDERS;
            }

            if (ConfigurationManager.AppSettings["UpdateAMSInProcessOrders"].IsNotNull())
            {
                _updateAMSInProcessOrders = Convert.ToBoolean(ConfigurationManager.AppSettings["UpdateAMSInProcessOrders"]);
            }
            else
            {
                _updateAMSInProcessOrders = AppConsts.UPDATE_AMS_INPROCESS_ORDERS;
            }

            if (ConfigurationManager.AppSettings["StartingOrderID"].IsNotNull())
            {
                _startingOrderID = Convert.ToInt32(ConfigurationManager.AppSettings["StartingOrderID"]);
            }
            else
            {
                _startingOrderID = AppConsts.UPDATE_ORDER_STARTING_ORDERID;
            }

            if (ConfigurationManager.AppSettings["EndingOrderID"].IsNotNull())
            {
                _endingOrderID = Convert.ToInt32(ConfigurationManager.AppSettings["EndingOrderID"]);
            }
            else
            {
                _endingOrderID = AppConsts.UPDATE_ORDER_ENDING_ORDERID;
            }

            if (ConfigurationManager.AppSettings["UpdateOrderRecentDays"].IsNotNull())
            {
                _updateOrderRecentDays = Convert.ToInt32(ConfigurationManager.AppSettings["UpdateOrderRecentDays"]);
            }
            else
            {
                _updateOrderRecentDays = AppConsts.UPDATE_ORDER_RECENT_DAYS;
            }

            if (ConfigurationManager.AppSettings["UpdateOrderRetryTimeLag"].IsNotNull())
            {
                _updateOrderRetryTimeLag = Convert.ToInt32(ConfigurationManager.AppSettings["UpdateOrderRetryTimeLag"]);
            }
            else
            {
                _updateOrderRetryTimeLag = AppConsts.UPDATE_ORDER_RETRY_TIME_LAG;
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
            if (ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull())
            {
                _isServiceLoggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]);
            }
            else
            {
                _isServiceLoggingEnabled = false;
            }


            #region Keys used in UAT-4162:- Retry logic for DS Order to get data from clearstar.

            if (ConfigurationManager.AppSettings["RetryDSOrdersChunkSize"].IsNotNull())
            {
                _retryDSOrdersChunkSize = Convert.ToInt32(ConfigurationManager.AppSettings["RetryDSOrdersChunkSize"]);
            }
            else
            {
                _retryDSOrdersChunkSize = AppConsts.RETRY_DS_ORDERS_CHUNKSIZE;
            }

            if (ConfigurationManager.AppSettings["RetryDSOrderRetryTimeLag"].IsNotNull())
            {
                _retryDSOrderRetryTimeLag = Convert.ToInt32(ConfigurationManager.AppSettings["RetryDSOrderRetryTimeLag"]);
            }
            else
            {
                _retryDSOrderRetryTimeLag = AppConsts.RETRY_DS_ORDER_RETRY_TIMELAG;
            }

            if (ConfigurationManager.AppSettings["MaxRetryCountForDSOrders"].IsNotNull())
            {
                _maxRetryCountForDSOrders = Convert.ToInt32(ConfigurationManager.AppSettings["MaxRetryCountForDSOrders"]);
            }
            else
            {
                _maxRetryCountForDSOrders = AppConsts.MAX_RETRY_COUNT_FOR_DS_ORDERS;
            }

            #endregion

            UpdateOrderServiceLogger = "UpdateOrderServiceLogger";
        }

        #endregion

        #region Methods

        #region Public Methods

        public static void UpdateExtVendorOrder(Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                ServiceLogger.Info("Calling UpdateExtVendorOrder: " + DateTime.Now.ToString(), UpdateOrderServiceLogger);

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Entity.Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                ServiceLogger.Debug<List<ClientDBConfiguration>>("UpdateExtVendorOrder: List of Client DbConfigurations from database:", clientDbConfs, UpdateOrderServiceLogger);

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    ServiceLogger.Debug<Int32?>("UpdateExtVendorOrder: TenantID for which external vendor orders are to be dispatched:", tenant_Id, UpdateOrderServiceLogger);
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }
                ServiceLogger.Info("UpdateExtVendorOrder: Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), UpdateOrderServiceLogger);
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDBConnection.TestConnString(clntDbConf.CDB_ConnectionString, UpdateOrderServiceLogger))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        if (_updateAllInProcessOrders && DateTime.Now.Hour >= _updateAllInProcessOrdersOnOrAfter)
                        {

                            if (_updateAMSInProcessOrders)
                            {
                                ServiceLogger.Info("UpdateExtVendorOrder: Started Update ALL AMS InProcess orders and getAllInprocessOrders at. " + DateTime.Now.ToString(),
                                    UpdateOrderServiceLogger);
                                UpdateAMSInProcessOrders(tenantId, true);
                                ServiceLogger.Info("UpdateExtVendorOrder: End Update ALL AMS InProcess orders and getAllInprocessOrders at. " + DateTime.Now.ToString(),
                                    UpdateOrderServiceLogger);
                            }

                            if (_updateEXInProcessOrders)
                            {
                                //Do we need this?
                            }
                        }
                        else
                        {
                            if (_updateAMSInProcessOrders)
                            {
                                ServiceLogger.Info("UpdateExtVendorOrder: Started Update ALL AMS InProcess orders and not getAllInprocessOrders at. " + DateTime.Now.ToString()
                                    + " for TenantID: " + tenantId, UpdateOrderServiceLogger);
                                UpdateAMSInProcessOrders(tenantId, false);
                                ServiceLogger.Info("UpdateExtVendorOrder: End Update ALL AMS InProcess orders and not getAllInprocessOrders at. " + DateTime.Now.ToString()
                                    + " for TenantID: " + tenantId, UpdateOrderServiceLogger);
                            }

                            if (_updateEXInProcessOrders)
                            {
                                //Do we need this?
                            }
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BKG_ORDER_SERVICE_USER_ID);
                            Int32 bkgOrderServiceUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BKG_ORDER_SERVICE_USER_ID_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.UpdateExtVendorOrder.GetStringValue();
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
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in UpdateExtVendorOrder method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                    UpdateOrderServiceLogger);
            }
        }

        #region UAT-4162:- Retry login for drug screening services order whose data was not pulled from clearstar when the order status is paid.

        public static void GetDSOrderDataFromClearStar(Int32? tenant_Id = null)
        {
            ServiceLogger.Info("Calling GetDSOrderDataFromClearStar: " + DateTime.Now.ToString(), UpdateOrderServiceLogger);

            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BKG_ORDER_SERVICE_USER_ID);
            Int32 bkgOrderServiceUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BKG_ORDER_SERVICE_USER_ID_VALUE;

            List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                item =>
                {
                    ClientDBConfiguration config = new ClientDBConfiguration();
                    config.CDB_TenantID = item.CDB_TenantID;
                    config.CDB_ConnectionString = item.CDB_ConnectionString;
                    config.Tenant = new Entity.Tenant();
                    config.Tenant.TenantName = item.Tenant.TenantName; return config;
                }).ToList();

            ServiceLogger.Debug<List<ClientDBConfiguration>>("GetDSOrderDataFromClearStar: List of Client DbConfigurations from database:", clientDbConfs, UpdateOrderServiceLogger);

            if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
            {
                ServiceLogger.Debug<Int32?>("GetDSOrderDataFromClearStar: TenantID for which drug screening order are to be processed:", tenant_Id, UpdateOrderServiceLogger);
                clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
            }

            ServiceLogger.Info("GetDSOrderDataFromClearStar: Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), UpdateOrderServiceLogger);

            foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
            {
                if (CheckDBConnection.TestConnString(clntDbConf.CDB_ConnectionString, UpdateOrderServiceLogger))
                {
                    DateTime jobStartTime = DateTime.Now;
                    DateTime jobEndTime;
                    Int32 tenantId = clntDbConf.CDB_TenantID;

                    GetDSOrderToProcess(tenantId, bkgOrderServiceUserId);

                    //Save service logging data to DB
                    if (_isServiceLoggingEnabled)
                    {
                        jobEndTime = DateTime.Now;
                        ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                        serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                        serviceLoggingContract.JobName = JobName.GetDSOrderDataFromClearStar.GetStringValue();
                        serviceLoggingContract.TenantID = tenantId;
                        serviceLoggingContract.JobStartTime = jobStartTime;
                        serviceLoggingContract.JobEndTime = jobEndTime;
                        serviceLoggingContract.IsDeleted = false;
                        serviceLoggingContract.CreatedBy = bkgOrderServiceUserId;
                        serviceLoggingContract.CreatedOn = DateTime.Now;
                        SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                    }
                }
            }
        }

        #endregion


        #endregion

        #region Private Methods

        private static void UpdateAMSInProcessOrders(Int32 tenantId, Boolean getAllInprocessOrders)
        {
            //TODO for StartingOrderID, EndingOrderID, REcentDays 
            Boolean executeLoop = true;
            try
            {
                while (executeLoop)
                {
                    Int32 updateOrderRecentDays = (getAllInprocessOrders) ? AppConsts.NONE : _updateOrderRecentDays;
                    ServiceLogger.Info("UpdateAMSInProcessOrders: Started GetOrdersToBeUpdatedVendorData at " + DateTime.Now.ToString() + " for TenantID:" + tenantId
                        + " ,RecordChunkSize: " + _recordChunkSize
                        + " ,StartingOrderID " + _startingOrderID.ToString() + " ,EndingOrderID: " + _endingOrderID.ToString() + " ,getAllInprocessOrders:"
                        + getAllInprocessOrders.ToString() + ", UpdateOrderRecentDays: " + updateOrderRecentDays.ToString(),
                        UpdateOrderServiceLogger);

                    var lstOrdersToBeUpdatedVendorData = ExternalVendorOrderManager.GetOrdersToBeUpdatedVendorData(tenantId, _recordChunkSize, _startingOrderID,
                                                        _endingOrderID, updateOrderRecentDays, _updateOrderRetryTimeLag).ToList();

                    if (lstOrdersToBeUpdatedVendorData.IsNotNull() && lstOrdersToBeUpdatedVendorData.Count() > AppConsts.NONE)
                    {
                        ServiceLogger.Info("UpdateAMSInProcessOrders: End GetOrdersToBeUpdatedVendorData at " + DateTime.Now.ToString()
                            + " TotalCount:" + lstOrdersToBeUpdatedVendorData.Count() + " for TenantID:" + tenantId, UpdateOrderServiceLogger);
                        ServiceLogger.Info("UpdateAMSInProcessOrders: Started foreach loop for VendorID at" + DateTime.Now.ToString(), UpdateOrderServiceLogger);
                        lstOrdersToBeUpdatedVendorData.Where(x => x.VendorID.HasValue).Select(col => col.VendorID.Value).Distinct().ForEach(vendorID =>
                        {
                            ServiceLogger.Info("UpdateAMSInProcessOrders: Started foreach loop for VendorID:" + vendorID, UpdateOrderServiceLogger);
                            ExternalVendor vendor = LookupManager.GetExternalVendorsForAMS().FirstOrDefault(cond => cond.EVE_ID == vendorID);

                            if (vendor.IsNotNull())
                            {
                                String vendorName = vendor.EVE_Name;
                                ServiceLogger.Debug<String>("UpdateAMSInProcessOrders: VendorName:", vendorName, UpdateOrderServiceLogger);
                                ExternalVendorUnityHelper.SetVendorServiceAdapter(vendorName);
                                ServiceLogger.Info("Started foreach loop on lstOrdersToBeUpdatedVendorData based on BkgOrderId: "
                                                    + DateTime.Now.ToString(), UpdateOrderServiceLogger);

                                lstOrdersToBeUpdatedVendorData.Where(cond => cond.VendorID == vendorID).Select(col => col.ExternalVendorBkgOrderDetailID).Distinct()
                                .ForEach(ExteralVendorBkgOrderDetailID =>
                                {
                                    ServiceLogger.Debug("ExteralVendorBkgOrderDetailID from the loop:" + ExteralVendorBkgOrderDetailID, UpdateOrderServiceLogger);
                                    var ordersToBeUpdatedVendorData = lstOrdersToBeUpdatedVendorData.Where(cond => cond.VendorID == vendorID
                                                                                               && cond.ExternalVendorBkgOrderDetailID == ExteralVendorBkgOrderDetailID);
                                    if (ordersToBeUpdatedVendorData.IsNotNull() && ordersToBeUpdatedVendorData.Count() > AppConsts.NONE)
                                    {
                                        ordersToBeUpdatedVendorData.Select(col => col.BkgOrderPackageServiceGroupID).Distinct()
                                        .ForEach(opsg =>
                                        {
                                            ServiceLogger.Debug("Order Package Svc Grp ID within BkgOrderDetailID :" + opsg, UpdateOrderServiceLogger);
                                            var ordersPkgSvcGrpVendorData = ordersToBeUpdatedVendorData.Where(cond => cond.BkgOrderPackageServiceGroupID == opsg).ToList();
                                            if (ordersPkgSvcGrpVendorData.IsNotNull() && ordersPkgSvcGrpVendorData.Count() > AppConsts.NONE)
                                            {
                                                //var bkgOrderID = ordersToBeUpdatedVendorData.FirstOrDefault().BkgOrderID;
                                                //var processedOrderItemList = lstOrdersToBeUpdatedVendorData.Where(cond => cond.BkgOrderID == bkgOrderID
                                                //&& cond.ExternalVendorBkgOrderDetailID != ExteralVendorBkgOrderDetailID);
                                                ServiceLogger.Debug("Total Line Item for OrderPackageSvcGrpID :" + ordersPkgSvcGrpVendorData.Count(), UpdateOrderServiceLogger);
                                                //UpdateExternalVendorOrders(ordersToBeUpdatedVendorData, processedOrderItemList, tenantId, vendorName);
                                                UpdateExternalVendorOrders(ordersPkgSvcGrpVendorData, tenantId, vendorName);
                                            }
                                        });
                                    }
                                });
                                ServiceLogger.Info("Ended foreach loop on lstOrdersToBeUpdatedVendorData based on BkgOrderId at " + DateTime.Now.ToString(),
                                    UpdateOrderServiceLogger);
                            }
                        });
                        ServiceLogger.Info("Ended foreach loop on lstOrderLineItemResult based on vendor id: " + DateTime.Now.ToString(),
                            UpdateOrderServiceLogger);
                        ServiceContext.ReleaseDBContextItems();
                        executeLoop = true;
                    }
                    else
                    {
                        ServiceLogger.Info("UpdateAMSInProcessOrders: End GetOrdersToBeUpdatedVendorData at " + DateTime.Now.ToString()
                               + " TotalCount: " + AppConsts.NONE, UpdateOrderServiceLogger);
                        ServiceContext.ReleaseDBContextItems();
                        executeLoop = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in UpdateAMSInProcessOrders method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                    UpdateOrderServiceLogger);
                executeLoop = false;
                ServiceContext.ReleaseDBContextItems();
            }
        }

        //private static void UpdateExternalVendorOrders(IEnumerable<usp_GetOrdersToBeUpdatedVendorData_Result> ordersToBeUpdatedVendorData,
        //                                               IEnumerable<usp_GetOrdersToBeUpdatedVendorData_Result> processedOrderItemList, Int32 tenantID, String vendorName)
        private static void UpdateExternalVendorOrders(IEnumerable<usp_GetOrdersToBeUpdatedVendorData_Result> ordersToBeUpdatedVendorData, Int32 tenantID, String vendorName)
        {
            try
            {

                ServiceLogger.Info("Start Update Background Orders with Clearstar Order result: " + DateTime.Now.ToString(), UpdateOrderServiceLogger);
                //Get status id for status code "AAAD" (Completed status code)
                Int32 completedOrderStatusId = ExternalVendorOrderManager.GetBkgOrderStatusTypeIDByCpde(OrderStatusType.COMPLETED.GetStringValue(), tenantID);

                if (ordersToBeUpdatedVendorData.Any(cond => cond.OrderLineItemDateCompleted.IsNull()) || ordersToBeUpdatedVendorData.Any(cond => cond.OrderStatusTypeID != completedOrderStatusId))
                {
                    ServiceLogger.Info("Starting UpdateVendorBkgOrder. SvcLineItem found with Date Completed as Null.", UpdateOrderServiceLogger);

                    //Translate Bkg Order Data to UpdateOrderContract list
                    EvUpdateOrderContract transalatedVendorOrders = TranslateOrdersToBeUpdatedVendorDataFields(ordersToBeUpdatedVendorData);

                    //Call UpdateVendorBkgOrder
                    //EvUpdateOrderContract evUpdateOrderContract = ExternalVendorUnityHelper.VendorServiceAdapter(vendorName)
                    //                                                .UpdateVendorBkgOrder(transalatedVendorOrders, processedOrderItemList, tenantID);
                    EvUpdateOrderContract evUpdateOrderContract = ExternalVendorUnityHelper.VendorServiceAdapter(vendorName)
                                                                    .UpdateVendorBkgOrder(transalatedVendorOrders, tenantID);
                    ServiceLogger.Info("Updating Background Orders with Vendor Order data result to the database.", UpdateOrderServiceLogger);

                    try
                    {
                        //Update Bkg Order data with External Vendor Order Data
                        ServiceLogger.Debug<EvUpdateOrderContract>("EvUpdateOrderContract to be updated to database: ", evUpdateOrderContract, UpdateOrderServiceLogger);
                        //ExternalVendorOrderManager.UpdateExternalVendorOrders(evUpdateOrderContract, processedOrderItemList, tenantID);
                        ExternalVendorOrderManager.UpdateExternalVendorOrders(evUpdateOrderContract, tenantID);
                        ServiceLogger.Info("End Updating Background Orders with Vendor Order data result to the database.", UpdateOrderServiceLogger);
                    }
                    catch (Exception ex)
                    {
                        ServiceLogger.Error(String.Format("An Error has occured in UpdateExternalVendorOrders method while updating results to database"
                                   + "for BkgOrder: " + evUpdateOrderContract.BkgOrderID
                                   + ", the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                   ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                   UpdateOrderServiceLogger);

                        ServiceContext.ReleaseDBContextItems();
                    }

                    ServiceLogger.Info("End UpdateVendorBkgOrder.", UpdateOrderServiceLogger);
                }
                else
                {
                    //Translate Bkg Order Data to UpdateOrderContract list
                    EvUpdateOrderContract transalatedVendorOrders = TranslateOrdersToBeUpdatedVendorDataFields(ordersToBeUpdatedVendorData);
                    if (transalatedVendorOrders.IsNotNull())
                    {
                        ServiceLogger.Info("Start Update Retry Date for orders which has no SvcLineItem with Date Completed as Null", UpdateOrderServiceLogger);
                        try
                        {
                            //Update External vendor retry date for those orders whose 0 svcLineItem found with Date Completed as Null
                            ExternalVendorOrderManager.UpdateExternalVendorOrdersRetryDate(transalatedVendorOrders.ExternalVendorBkgOrderDetailID, tenantID);
                            ServiceLogger.Info("End Update Retry Date for orders which has no SvcLineItem with Date Completed as Null", UpdateOrderServiceLogger);
                        }
                        catch (Exception ex)
                        {
                            ServiceLogger.Error(String.Format("An Error has occured in UpdateExternalVendorOrdersRetryDate method while updating results to database"
                                       + "for BkgOrder: " + transalatedVendorOrders.BkgOrderID
                                       + ", the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                       ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                       UpdateOrderServiceLogger);

                            ServiceContext.ReleaseDBContextItems();
                        }
                    }
                    ServiceLogger.Info("0 SvcLineItem found with Date Completed as Null.", UpdateOrderServiceLogger);
                }
                ServiceLogger.Info("End Update Background Orders with Clearstar Order result: " + DateTime.Now.ToString(), UpdateOrderServiceLogger);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in UpdateExternalVendorOrders method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2} for tenantID {3}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString, tenantID.ToString()),
                                    UpdateOrderServiceLogger);

            }
        }
        /// <summary>
        /// Translate Bkg Order Data to UpdateOrderContract list
        /// </summary>
        /// <param name="lstOrdersToBeUpdatedVendorData"></param>
        /// <returns>EvUpdateOrderContract</returns>
        private static EvUpdateOrderContract TranslateOrdersToBeUpdatedVendorDataFields(IEnumerable<usp_GetOrdersToBeUpdatedVendorData_Result> lstOrdersToBeUpdatedVendorData)
        {
            ServiceLogger.Info("Entering TranslateOrdersToBeUpdatedVendorDataFields: " + DateTime.Now.ToString(), UpdateOrderServiceLogger);
            usp_GetOrdersToBeUpdatedVendorData_Result ordersToBeUpdatedVendorData = lstOrdersToBeUpdatedVendorData.FirstOrDefault();
            EvUpdateOrderContract evUpdateOrderContract = null;
            if (ordersToBeUpdatedVendorData.IsNotNull())
            {
                evUpdateOrderContract = new EvUpdateOrderContract
                {
                    LoginName = _loginID,
                    Password = _password,
                    BusinessOwnerID = _businessOwnerID,
                    AccountNumber = ordersToBeUpdatedVendorData.AccountNumber,
                    BkgOrderID = ordersToBeUpdatedVendorData.BkgOrderID,
                    BkgOrderVendorProfileID = ordersToBeUpdatedVendorData.VendorProfileID,
                    OrderPreviousStatusTypeCode = ordersToBeUpdatedVendorData.CurrentOrderStatusTypeCode,
                    OrderStatusTypeID = ordersToBeUpdatedVendorData.OrderStatusTypeID,
                    ExternalVendorBkgOrderDetailID = ordersToBeUpdatedVendorData.ExternalVendorBkgOrderDetailID,
                    /* Commenetd code set below properties at service group level
                     NeedsFirstReview = ordersToBeUpdatedVendorData.NeedsFirstReview,
                     PackageSupplementalTypeID = ordersToBeUpdatedVendorData.PackageSupplementalTypeID,
                     PackageSupplementalTypeCode = ordersToBeUpdatedVendorData.PackageSupplementalTypeCode,*/
                    IsAutoReviewComplete = ordersToBeUpdatedVendorData.IsAutoReviewComplete,//Set IsAutoReviewComplete value in contract
                    EvUpdateOrderItemContract = lstOrdersToBeUpdatedVendorData.Select(item => new EvUpdateOrderItemContract
                    {
                        BkgOrderPackageSvcLineItemID = item.BkgOrderPackageSvcLineItemID,
                        ExternalVendorOrderID = item.VendorLineItemOrderID,
                        DateCompleted = item.OrderLineItemDateCompleted,
                        ExtVendorBkgOrderLineItemDetailID = item.ExtVendorBkgOrderLineItemDetailID,
                        BkgOrderPackageServiceGroupID = item.BkgOrderPackageServiceGroupID
                    }).ToList(),
                };

                //List<EvUpdateOrderPackageSvcGroup> EvUpdateOrderPackageSvcGroup = new List<Entity.ExternalVendorContracts.EvUpdateOrderPackageSvcGroup>();
                EvUpdateOrderPackageSvcGroup evUpdateOrderPackageSvcGroup = new EvUpdateOrderPackageSvcGroup()
                {
                    BkgSvcGroupID = ordersToBeUpdatedVendorData.BkgSvcGroupID,
                    FirstReviewTrigger = ordersToBeUpdatedVendorData.FirstReviewTrigger,
                    SecondReviewTrigger = ordersToBeUpdatedVendorData.SecondReviewTrigger,
                    BkgSvcGroupName = ordersToBeUpdatedVendorData.BkgSvcGroupName,
                    ServiceGroupFlaggedInd = ordersToBeUpdatedVendorData.ServiceGroupFlaggedInd.IsNotNull() ? ordersToBeUpdatedVendorData.ServiceGroupFlaggedInd.Value : false,
                    ServiceGroupPreviousReviewStatusCode = ordersToBeUpdatedVendorData.ServiceGroupReviewStatusTypeCode,
                    ServiceGroupStatusCode = ordersToBeUpdatedVendorData.ServiceGroupStatusCode,
                    BkgOrderPackageServiceGroupID = ordersToBeUpdatedVendorData.BkgOrderPackageServiceGroupID,
                    NeedsFirstReview = ordersToBeUpdatedVendorData.NeedsFirstReview,
                    PackageSupplementalTypeID = ordersToBeUpdatedVendorData.PackageSupplementalTypeID,
                    PackageSupplementalTypeCode = ordersToBeUpdatedVendorData.PackageSupplementalTypeCode
                };


                evUpdateOrderContract.EvUpdateOrderPackageSvcGroup = evUpdateOrderPackageSvcGroup;

                ServiceLogger.Info("Returning translated fields from TranslateOrdersToBeUpdatedVendorDataFields: " + DateTime.Now.ToString(), UpdateOrderServiceLogger);

            }
            return evUpdateOrderContract;
        }

        /// <summary>
        ///  Method to process drug screening orders to get data from clearstar. 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="getAllInprocessOrders"></param>
        private static void GetDSOrderToProcess(Int32 tenantId, Int32 bkgProcessId)
        {
            //TODO for StartingOrderID, EndingOrderID, REcentDays 
            Boolean executeLoop = true;
            try
            {
                while (executeLoop)
                {

                    ServiceLogger.Info("GetOrdersToBeUpdatedVendorData: Started GetDSOrderToProcess at " + DateTime.Now.ToString() + " for TenantID:" + tenantId
                        + " ,RecordChunkSize: " + _retryDSOrdersChunkSize
                        + " ,MaxRetryCount " + _maxRetryCountForDSOrders.ToString() + " ,RetryTimeLag: " + _retryDSOrderRetryTimeLag.ToString(),
                        UpdateOrderServiceLogger);


                    List<INTSOF.UI.Contract.BkgOperations.VendorProfileSvcLineItemContract> lstDsOrdersToBeProcessed = new List<INTSOF.UI.Contract.BkgOperations.VendorProfileSvcLineItemContract>();
                    lstDsOrdersToBeProcessed = BackgroundProcessOrderManager.GetDSOrderToGetCSData(tenantId, _retryDSOrdersChunkSize, _maxRetryCountForDSOrders, _retryDSOrderRetryTimeLag);

                    ServiceLogger.Info("GetOrdersToBeUpdatedVendorData: End GetDSOrderToProcess at " + DateTime.Now.ToString()
                            + " TotalCount:" + lstDsOrdersToBeProcessed.Count() + " for TenantID:" + tenantId, UpdateOrderServiceLogger);
                    //Call manager method het in a List and use below.

                    if (lstDsOrdersToBeProcessed.IsNotNull() && lstDsOrdersToBeProcessed.Count() > AppConsts.NONE)
                    {
                        Dictionary<Int32, Int32> dicDSOrderToUpdateRetry = new Dictionary<Int32, Int32>();
                        String dataPulledDueStatusCode = DataPullStatusType.DUE_STATUS.GetStringValue();
                        String dataPulledDoneStatusCode = DataPullStatusType.DONE_STATUS.GetStringValue();

                        List<lkpDataPullStatusType> lstdataPulledStatus = BackgroundProcessOrderManager.GetDataPulledStatusType(tenantId);

                        Int32 dataPulledDueStatusID = lstdataPulledStatus.Where(con => !con.DPST_IsDeleted && con.DPST_Code == dataPulledDueStatusCode).FirstOrDefault().DPST_ID;
                        Int32 dataPulledDoneStatusID = lstdataPulledStatus.Where(con => !con.DPST_IsDeleted && con.DPST_Code == dataPulledDoneStatusCode).FirstOrDefault().DPST_ID;

                        ServiceLogger.Info("GetOrdersToBeUpdatedVendorData: Started foreach loop for DS Orders at" + DateTime.Now.ToString(), UpdateOrderServiceLogger);

                        foreach (INTSOF.UI.Contract.BkgOperations.VendorProfileSvcLineItemContract dsOrder in lstDsOrdersToBeProcessed)
                        {
                            ServiceLogger.Info("GetOrdersToBeUpdatedVendorData: Started foreach loop for OrderID:" + dsOrder.OrderID, UpdateOrderServiceLogger);
                            try
                            {
                                List<Int32> lstBPHM_Id = new List<Int32>();
                                lstBPHM_Id.Add(dsOrder.BkgHierarchyMappingID);

                                //Create dictionary for parallel task parameter.
                                Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
                                dicParam.Add("BkgOrderId", dsOrder.BkgOrderID);
                                dicParam.Add("TenantId", tenantId);
                                dicParam.Add("ExtVendorId", dsOrder.ExtVendorID);
                                dicParam.Add("BPHMId_List", lstBPHM_Id);
                                dicParam.Add("RegistrationId", String.Empty);
                                dicParam.Add("CurrentLoggedInUserId", bkgProcessId);
                                dicParam.Add("OrganizationUserId", dsOrder.OrganizationUserID);
                                dicParam.Add("OrganizationUserProfileId", dsOrder.OrganizationUserProfileID);
                                dicParam.Add("ApplicantName", dsOrder.ApplicantName);
                                dicParam.Add("PrimaryEmailAddress", dsOrder.PrimaryEmailAddress);
                                dicParam.Add("HierarchyNodeId", dsOrder.SelectedNodeID);

                                ExternalVendors.ClearStarVendor.ClearStarCCF objClearstarCCf = new ExternalVendors.ClearStarVendor.ClearStarCCF();
                                objClearstarCCf.SaveCCFDataAndPDF(dicParam);
                                dicDSOrderToUpdateRetry.Add(dsOrder.PackageSvcLineItemID, dataPulledDoneStatusID);
                            }
                            catch
                            {
                                //Add For those line items whose status id currently not done. 

                                BkgOrderPackageSvcLineItem bkgOrderPackageSvcLineItem = ExternalVendorOrderManager.GetBkgOrderPackageSvcLineItem(tenantId, dsOrder.PackageSvcLineItemID);

                                if (!bkgOrderPackageSvcLineItem.IsNullOrEmpty() && bkgOrderPackageSvcLineItem.PSLI_DataPulledStatusTypeID == dataPulledDueStatusID)
                                    dicDSOrderToUpdateRetry.Add(dsOrder.PackageSvcLineItemID, dataPulledDueStatusID);
                            }
                            //Bind Dictionary here//
                            //Dictionary<String, Object> dicParam //

                            //Call method here

                            //objClearstarCCf.SaveCCFDataAndPDF()

                            //call method to update retry count and retry date. input -- PSLI -- Output boolean --
                        }
                        ServiceLogger.Info("Ended foreach loop on lstOrderLineItemResult based on vendor id: " + DateTime.Now.ToString(),
                            UpdateOrderServiceLogger);

                        ServiceLogger.Info("Calling:Method UpdateRetryCountForDsOrders to update retry count: " + DateTime.Now.ToString(),
                          UpdateOrderServiceLogger);

                        if (!dicDSOrderToUpdateRetry.IsNullOrEmpty() && dicDSOrderToUpdateRetry.Count() > AppConsts.NONE)
                        {
                            if (UpdateRetryCountForDsOrders(tenantId, dicDSOrderToUpdateRetry, bkgProcessId))
                                ServiceLogger.Info("Retry count for Drug screening orders are updated successfully : " + DateTime.Now.ToString(), UpdateOrderServiceLogger);
                            else
                                ServiceLogger.Info("Retry count for Drug screening orders are not updated successfully : " + DateTime.Now.ToString(), UpdateOrderServiceLogger);
                            ServiceLogger.Info("Ended updating retry count for Drug screeining orders: " + DateTime.Now.ToString(), UpdateOrderServiceLogger);
                        }
                        ServiceContext.ReleaseDBContextItems();
                        executeLoop = true;
                    }
                    else
                    {
                        ServiceLogger.Info("GetOrdersToBeUpdatedVendorData: End GetDSOrderToProcess at " + DateTime.Now.ToString()
                               + " TotalCount: " + AppConsts.NONE, UpdateOrderServiceLogger);
                        ServiceContext.ReleaseDBContextItems();
                        executeLoop = false;
                    }
                }
            }
            //Need to update retry count and retry date stamp if exeception occurs.
            //Create a method to update retry count and retry date. input -- PSLI -- Output boolean --

            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in GetDSOrderToProcess method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                    UpdateOrderServiceLogger);
                executeLoop = false;
                ServiceContext.ReleaseDBContextItems();
            }
        }

        private static Boolean UpdateRetryCountForDsOrders(Int32 tenantId, Dictionary<Int32, Int32> dicbkgSvcLineItems, Int32 bkgProcessId)
        {
            try
            {
                Boolean _result = false;
                ServiceLogger.Info("Started updating retry count for Drug screeining orders: " + DateTime.Now.ToString(), UpdateOrderServiceLogger);
                _result = BackgroundProcessOrderManager.UpdateRetryCountForDsOrders(tenantId, dicbkgSvcLineItems, bkgProcessId);
                return _result;
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in UpdateRetryCountForDsOrders method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                    UpdateOrderServiceLogger);
                return false;
                //ServiceContext.ReleaseDBContextItems();
            }
        }


        #endregion

        #endregion
    }
}
