using Business.Interfaces;
using Business.RepoManagers;
using Business.ReportExecutionService;
using Entity;
using ExternalVendors.ClearStarVendor;
using INTSOF.AuthNet.Business.CustomerProfileWS;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.ScheduleTask;
using INTSOF.UI.Contract.Services;
using INTSOF.Utils;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace EmailDispatcherService
{
    public class ScheduleAction
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;

        public static void SendMailForScheduleActionExecuteCategoryrules(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailForScheduleActionExecuteCategoryrules: " + DateTime.Now.ToString() + " *******************");


                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                    item =>
                    {
                        ClientDBConfiguration config = new ClientDBConfiguration();
                        config.CDB_TenantID = item.CDB_TenantID;
                        config.CDB_ConnectionString = item.CDB_ConnectionString;
                        config.Tenant = new Tenant();
                        config.Tenant.TenantName = item.Tenant.TenantName; return config;
                    }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Trace("******************* Started placing email in Queue for a chunk of action items: " + DateTime.Now.ToString() + " *******************");
                            executeLoop = RuleManager.ProcessActionExecuteCategoryRules(tenantId, tenantName, AppConsts.CHUNK_SIZE_FOR_CATEGORY_RULE_REOCCUR);
                            logger.Trace("******************* Ended placing email in Queue for a chunk of action items::" + DateTime.Now.ToString() + " *******************");
                            ServiceContext.ReleaseDBContextItems();
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                            Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendMailForScheduleActionExecuteCategoryrules.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in SendMailForScheduleActionExecuteCategoryrules method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Copy Package Data and Executes Business rules
        /// </summary>
        public static void CopyPackgeDataExecuteRule(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling CopyPackgeDataExecuteRule: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                // Get All Tenant
                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Trace("******************* Started Copying Package Data: " + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                            executeLoop = MobilityManager.CopyPackageDataExecuteBusinessRule(tenantId, AppConsts.CHUNK_SIZE_FOR_COPY_PACKAGE_EXECUTE_RULES, backgroundProcessUserId);
                            logger.Trace("******************* Ended Copying Package Data:" + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                            ServiceContext.ReleaseDBContextItems();
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.CopyPackgeDataExecuteRule.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in CopyPackgeDataExecuteRule method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        public static void SendMailForNonComplianceCategories(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailForNonComplianceCategories: " + DateTime.Now.ToString() + " *******************");

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                    item =>
                    {
                        ClientDBConfiguration config = new ClientDBConfiguration();
                        config.CDB_TenantID = item.CDB_TenantID;
                        config.CDB_ConnectionString = item.CDB_ConnectionString;
                        config.Tenant = new Tenant();
                        config.Tenant.TenantName = item.Tenant.TenantName; return config;
                    }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Trace("******************* Started placing email in Queue for a chunk of Non Compliance Categories: " + DateTime.Now.ToString() + " *******************");
                            try
                            {
                                executeLoop = RuleManager.ProcessNonComplianceCategories(tenantId, tenantName, AppConsts.CHUNK_SIZE_FOR_CATEGORY_RULE_REOCCUR);
                            }
                            catch (Exception ex)
                            {
                                logger.Error("An Error has occured in  processing the ProcessNonComplianceCategories method, the details of which are: {0}, Inner Exception: {1}, TenantID: {2}, Stack Trace: {3}", ex.Message, ex.InnerException, tenantId, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                            }

                            logger.Trace("******************* Ended placing email in Queue for a chunk of Non Compliance Categories:" + DateTime.Now.ToString() + " *******************");
                            ServiceContext.ReleaseDBContextItems();
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                            Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendMailForNonComplianceCategories.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in SendMailForNonComplianceItemRules method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        #region Support function for AUTH.Net file
        /// <summary>
        /// Get Authorize.Net User ID
        /// </summary>
        /// <returns></returns>
        private static Int32 GetAuthorizeDotNetUserID()
        {
            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.AUTHORIZE_DOT_NET_USER_ID);
            Int32 authorizeDotNetUserId = AppConsts.AUTHORIZE_DOT_NET_USER_VALUE;

            if (appConfiguration.IsNotNull())
            {
                authorizeDotNetUserId = Convert.ToInt32(appConfiguration.AC_Value);
            }
            return authorizeDotNetUserId;
        }

        /// <summary>
        /// Save transaction details
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <param name="transactionDetails"></param>
        public static void SaveTransactionDetails(String invoiceNumber, NameValueCollection transactionDetails, Int32 TenantId)
        {
            if (transactionDetails.IsNotNull())
            {
                Int32 authorizeDotNetUserId = GetAuthorizeDotNetUserID();
                ComplianceDataManager.UpdateOnlineTransactionResults(invoiceNumber, transactionDetails, TenantId, authorizeDotNetUserId);
            }
        }

        #endregion



        /// <summary>
        /// Approve Invoice Order and Executes Business rules
        /// </summary>
        public static void ApproveInvoiceOrderExecuteRule(Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                logger.Info("******************* Calling ApproveInvoiceOrderExecuteRule: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                // Get All Tenant
                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Trace("******************* Started Approve Invoice Order: " + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");

                            //get data from newly added table based on tenant id and call business method by passing parameter

                            Dictionary<Int32, String> dictUpdatedTaskStatus = new Dictionary<Int32, String>();
                            //List<Entity.ClientEntity.ScheduledTask> lstOrder = ComplianceDataManager.GetScheduledTasksForInvoiceOrder(tenantId, AppConsts.CHUNK_SIZE_FOR_APPROVE_INVOICE_ORDER_APPROVAL);
                            List<ScheduleTaskContract> lstScheduleTasks = ComplianceDataManager.GetScheduleTasksToProcess(tenantId, TaskType.INVOICEORDERBULKAPPROVE.GetStringValue(), AppConsts.CHUNK_SIZE_FOR_APPROVE_INVOICE_ORDER_APPROVAL);

                            executeLoop = lstScheduleTasks.Count > 0 ? true : false;
                            lstScheduleTasks.GroupBy(col => col.TaskGroup);
                            lstScheduleTasks.ForEach(column =>
                            {

                                try
                                {
                                    #region AUTH NEt insert data here
                                    logger.Trace("******************* insert data in Authrize net *******************");
                                    ScheduledTaskContract parameters = GetScheduleTaskXMLParameters(column.TaskXMLParameters.ToString());
                                    parameters.ScheduleTaskId = column.ScheduleTaskID;
                                    parameters.IsApprovalEmailSent = column.IsApprovalEmailSent;
                                    Boolean isOrderApproved = false;
                                    if (parameters.OrderStatusCode != ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue())
                                    {
                                        Boolean IsPaymentSuccessful = ProcessCreditCardPaymentDeduction(tenantId, parameters);
                                        if (IsPaymentSuccessful)
                                        {
                                            isOrderApproved = ComplianceDataManager.ProcessInvoiceOrderApproval(tenantId, parameters, new ClearStarCCF() as IClearStarCCF);
                                        }

                                    }
                                    #endregion

                                    if (!parameters.IsNullOrEmpty() && isOrderApproved)
                                    {
                                        dictUpdatedTaskStatus.Add(column.ScheduleTaskID, TaskStatusType.COMPLETED.GetStringValue());
                                    }
                                    else
                                    {
                                        // code to check abondoned order status,
                                        //if (column.StatusTypeCode.Equals(TaskStatusType.PENDING.GetStringValue()))
                                        //{
                                        //    dictUpdatedTaskStatus.Add(column.ScheduleTaskID, TaskStatusType.ERROR.GetStringValue());
                                        //}
                                        //else if (column.StatusTypeCode.Equals(TaskStatusType.ERROR.GetStringValue()))
                                        //{
                                        //    dictUpdatedTaskStatus.Add(column.ScheduleTaskID, TaskStatusType.ABANDONED.GetStringValue());
                                        //}
                                        dictUpdatedTaskStatus.Add(column.ScheduleTaskID, TaskStatusType.ABANDONED.GetStringValue());
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.Error("An Error has occured in ApproveInvoiceOrderExecuteRule method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                    // code to check abondoned order status,
                                    if (column.StatusTypeCode.Equals(TaskStatusType.PENDING.GetStringValue()))
                                    {
                                        dictUpdatedTaskStatus.Add(column.ScheduleTaskID, TaskStatusType.ERROR.GetStringValue());
                                    }
                                    else if (column.StatusTypeCode.Equals(TaskStatusType.ERROR.GetStringValue()))
                                    {
                                        dictUpdatedTaskStatus.Add(column.ScheduleTaskID, TaskStatusType.ABANDONED.GetStringValue());
                                    }
                                }
                            });

                            if (dictUpdatedTaskStatus.Count > AppConsts.NONE)
                            {
                                ComplianceDataManager.UpdateScheduleTaskStatus(tenantId, dictUpdatedTaskStatus, backgroundProcessUserId);
                            }

                            logger.Trace("******************* Ended Approve Invoice Order:" + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                            ServiceContext.ReleaseDBContextItems();
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.ApproveInvoiceOrderExecuteRule.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in ApproveInvoiceOrderExecuteRule method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        private static Boolean ProcessCreditCardPaymentDeduction(Int32 tenantId, ScheduledTaskContract parameters)
        {
            var OrderPkgPaymentDetail = ComplianceDataManager.GetOrderPkgPaymentDetailsByOrderID(tenantId, parameters.OrderId);

            String creditCardWithApproval = PaymentOptions.Credit_Card.GetStringValue();

            Int32 creditCardWithApprovalId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpPaymentOption>(tenantId).FirstOrDefault(cc => cc.Code == creditCardWithApproval && !cc.IsDeleted).PaymentOptionID;

            int _OrderPaymentDetailId = OrderPkgPaymentDetail.Where(x => x.OrderPaymentDetail.OPD_ID == parameters.OrderPaymentDetailId).Select(x => x.OPPD_OrderPaymentDetailID).FirstOrDefault();

            var OrderPaymentDetail = ComplianceDataManager.GetOrdrPaymentDetailByID(tenantId, _OrderPaymentDetailId);
            if (parameters.OrderPaymentDetailId > 0)
            {
                if (OrderPaymentDetail.OPD_PaymentOptionID == creditCardWithApprovalId)
                {
                    decimal? Amount = OrderPkgPaymentDetail.Select(x => x.OrderPaymentDetail.OPD_Amount).FirstOrDefault();
                    String _invoiceNumber = OrderPaymentDetail.OnlinePaymentTransaction.Invoice_num;
                    System.Guid _UserId = OrderPkgPaymentDetail.FirstOrDefault().OrderPaymentDetail.Order.OrganizationUserProfile.OrganizationUser.UserID;
                    Entity.AuthNetCustomerProfile customerProfile = ComplianceDataManager.GetCustomerProfile(_UserId, tenantId);
                    string Description = ComplianceDataManager.GetTenantList(SecurityManager.DefaultTenantID, true).Where(col => col.TenantID == tenantId).FirstOrDefault().TenantName;
                    int OrganizationUserId = parameters.OrganisationUserId;
                    long _customerPaymentProfileID = OrderPaymentDetail.OPD_CustomerPaymentProfileID.IsNullOrEmpty() ? 0 : Convert.ToInt64(OrderPaymentDetail.OPD_CustomerPaymentProfileID);
                    CreateCustomerProfileTransactionResponseType response = INTSOF.AuthNet.Business.AuthorizeNetCreditCard.CreateCustomerProfileTransaction(Convert.ToInt64(customerProfile.CustomerProfileID), _customerPaymentProfileID,
                                                                                                                           OrganizationUserId, Convert.ToDecimal(OrderPaymentDetail.OPD_Amount),
                                                                                                                           _invoiceNumber, Description);
                    NameValueCollection transactionDetails = INTSOF.AuthNet.Business.AuthorizeNetCreditCard.GetResponseFields(response);

                    if (response.resultCode == MessageTypeEnum.Ok)
                    {
                        SaveTransactionDetails(_invoiceNumber, transactionDetails, tenantId);
                        String responseCode = transactionDetails["x_response_code"];
                        String responseReasonCode = transactionDetails["x_response_reason_code"];
                        String responseReasonText = transactionDetails["x_response_reason_text"].ToLower();
                        //String successResponseText =  Resources.Language.TRNSCTNAPRV;
                        logger.Trace("******************* Successfully data in Authrize net *******************");
                        if (responseCode == "1" && responseReasonCode == "1" && responseReasonText == "this transaction has been approved.")
                            return true;
                        else
                            return false;

                    }
                    else
                    {
                        ComplianceDataManager.UpdateOPDStatus(tenantId, ApplicantOrderStatus.Online_Payment_Not_Completed.GetStringValue(), OrganizationUserId, _OrderPaymentDetailId);
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Schedule Task for calling the SalesForce API
        /// </summary>
        public static void SalesForceScheduleTask()
        {
            var _wgusfTenantId = (ConfigurationManager.AppSettings[AppConsts.WGUSF_UNIVERSITY_TENANTID]);
            Int32 wguTenantID = 0;
            Int32.TryParse(_wgusfTenantId, out wguTenantID);
            if (!(wguTenantID > 0))
                return;

            //var tenantId = Convert.ToInt32(_wgusfTenantId);

            logger.Info("******************* Call to SalesForceScheduleTask Starts at: " + DateTime.Now.ToString() + " *******************");
            Entity.AppConfiguration BkgProcessUserIdConfig = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
            Int32 backgroundProcessUserId = BkgProcessUserIdConfig.IsNotNull() ? Convert.ToInt32(BkgProcessUserIdConfig.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

            // Get All Tenant
            List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
             item =>
             {
                 ClientDBConfiguration config = new ClientDBConfiguration();
                 config.CDB_TenantID = item.CDB_TenantID;
                 config.CDB_ConnectionString = item.CDB_ConnectionString;
                 config.Tenant = new Tenant();
                 config.Tenant.TenantName = item.Tenant.TenantName; return config;
             }).Where(x => x.CDB_TenantID == wguTenantID).ToList();

            if (clientDbConfs.IsNullOrEmpty())
            {
                return;
            }

            foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
            {
                Dictionary<Int32, String> dicUpdatedTaskStatus = new Dictionary<Int32, String>();

                if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                {
                    DateTime jobStartTime = DateTime.Now;
                    DateTime jobEndTime;
                    Int32 _selectedTenantId = clntDbConf.CDB_TenantID;
                    Entity.ClientEntity.AppConfiguration _appConfiguration = ComplianceDataManager.GetAppConfiguration(TenantAppConfigurations.UPLOAD_COMPLIANCE_DATA_KEY.GetStringValue(),
                                                                _selectedTenantId);

                    List<ScheduleTaskContract> lstScheduleTasks = ComplianceDataManager.GetScheduleTasksToProcess(_selectedTenantId, TaskType.SALES_FORCE.GetStringValue(), AppConsts.CHUNK_SIZE_FOR_DAILY_BACKGROUND_REPORT);


                    if (!_appConfiguration.IsNullOrEmpty() && _appConfiguration.AC_Value == AppConsts.STR_ONE && lstScheduleTasks.Count() > AppConsts.NONE)
                    {
                        String requestString = String.Empty;
                        String _tpcduIds = String.Empty;

                        try
                        {
                            Boolean hasAnyData = false;

                            //UAT-1808: Fix Salesforce integration issue
                            String chunkSizeKey = ConfigurationManager.AppSettings[AppConsts.CHUNK_SIZE_APP_KEY];
                            Int32 chunkSize = chunkSizeKey.IsNullOrEmpty() ? AppConsts.CHUNK_SIZE_FOR_SALES_FORCE : Convert.ToInt32(chunkSizeKey);

                            Boolean executeLoop = true;
                            while (executeLoop)
                            {
                                logger.Info("******************* Started SalesForceScheduleTask at:" + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");

                                requestString = StoredProcedureManagers.GetComplianceDataToUpload(_selectedTenantId, chunkSize, out hasAnyData, out _tpcduIds);

                                if (hasAnyData)
                                {
                                    foreach (ScheduleTaskContract ScheduleTask in lstScheduleTasks)
                                    {
                                        #region Variable

                                        String _grantType = String.Empty;
                                        String _clientId = String.Empty;
                                        String _clientSecret = String.Empty;
                                        String _username = String.Empty;
                                        String _password = String.Empty;
                                        String _authRequestUrl = String.Empty;
                                        String _uploadRequestUrl = String.Empty;

                                        #endregion

                                        if (_selectedTenantId == Convert.ToInt32(_wgusfTenantId))
                                        {
                                            logger.Trace("******************* Started SalesForceScheduleTask for Tenant at:" + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");

                                            _grantType = ConfigurationManager.AppSettings[AppConsts.GRANT_TYPE_APP_KEY];
                                            _clientId = ConfigurationManager.AppSettings[AppConsts.CLIENT_ID_APP_KEY];
                                            _clientSecret = ConfigurationManager.AppSettings[AppConsts.CLIENT_SECRET_APP_KEY];
                                            _username = ConfigurationManager.AppSettings[AppConsts.USERNAME_SALESFORCE_APP_KEY];
                                            _password = ConfigurationManager.AppSettings[AppConsts.PASSWORD_TOKEN_APP_KEY];
                                            _authRequestUrl = ConfigurationManager.AppSettings[AppConsts.AUTHENTICATION_REQUEST_URL];
                                            _uploadRequestUrl = ConfigurationManager.AppSettings[AppConsts.UPLOAD_REQUEST_URL];


                                            String _authRequestParameters = "grant_type=" + _grantType
                                                                                   + "&client_id=" + _clientId
                                                                                   + "&client_secret=" + _clientSecret
                                                                                   + "&username=" + _username
                                                                                   + "&password=" + _password;

                                            String access_token = GetAuthToken(_authRequestUrl, _authRequestParameters);
                                            UploadComplianceData(_selectedTenantId, requestString, _uploadRequestUrl, access_token);

                                            StoredProcedureManagers.UpdateThirdPartyComplianceDataUploadStatus(_tpcduIds, ThirdPartyUploadStatus.UPLOAD_COMPLETE.GetStringValue(), backgroundProcessUserId, _selectedTenantId);
                                            if (!dicUpdatedTaskStatus.ContainsKey(ScheduleTask.ScheduleTaskID))
                                            {
                                                dicUpdatedTaskStatus.Add(ScheduleTask.ScheduleTaskID, String.Empty);
                                            }
                                            logger.Trace("******************* Ended SalesForceScheduleTask for Tenant at:" + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                                        }
                                    }
                                    if (_selectedTenantId == Convert.ToInt32(_wgusfTenantId))
                                    {
                                        ComplianceDataManager.UpdateScheduleTaskStatus(_selectedTenantId, dicUpdatedTaskStatus, backgroundProcessUserId);
                                    }
                                }
                                else
                                {
                                    logger.Info("******************* SalesForceScheduleTask - _dataToUpload is empty *******************");
                                    logger.Trace("******************* Ended SalesForceScheduleTask at:" + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                                    executeLoop = false;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            String _errorMessage = String.Format("An Error has occured in SalesForceScheduleTask method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                            StoredProcedureManagers.SaveComplianceUploadServiceHistory(requestString, _errorMessage, _selectedTenantId);
                            //logger.Error("An Error has occured in SalesForceScheduleTask method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);

                            if (!String.IsNullOrEmpty(_tpcduIds))
                                StoredProcedureManagers.UpdateThirdPartyComplianceDataUploadStatus(_tpcduIds, ThirdPartyUploadStatus.ERROR.GetStringValue(), backgroundProcessUserId, _selectedTenantId);

                            logger.Error(_errorMessage);
                        }
                    }
                    //Save service logging data to DB
                    if (_isServiceLoggingEnabled)
                    {
                        jobEndTime = DateTime.Now;
                        ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                        serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                        serviceLoggingContract.JobName = JobName.SalesForceScheduleTask.GetStringValue();
                        serviceLoggingContract.TenantID = _selectedTenantId;
                        serviceLoggingContract.JobStartTime = jobStartTime;
                        serviceLoggingContract.JobEndTime = jobEndTime;
                        //serviceLoggingContract.Comments = "";
                        serviceLoggingContract.IsDeleted = false;
                        serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                        serviceLoggingContract.CreatedOn = DateTime.Now;
                        SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                    }
                    ServiceContext.ReleaseDBContextItems();
                }
            }
            logger.Info("******************* Call to SalesForceScheduleTask Ends at: " + DateTime.Now.ToString() + " *******************");
        }

        /// <summary>
        /// Gets the authentication token from he SalesForce, 
        /// using which the data is to be sent again to SalesForce
        /// </summary>
        /// <param name="_authRequestUrl"></param>
        /// <param name="_authRequestParameters"></param>
        /// <returns></returns>
        private static String GetAuthToken(String _authRequestUrl, String _authRequestParameters)
        {
            Byte[] _authRequestData = UTF8Encoding.UTF8.GetBytes(_authRequestParameters);

            HttpWebRequest authWebRequest = (HttpWebRequest)WebRequest.Create(_authRequestUrl);
            authWebRequest.Method = HttpRequestTypes.POST.GetStringValue();
            authWebRequest.ContentType = HttpRequestContentTypes.DEFAULT.GetStringValue();
            authWebRequest.ContentLength = _authRequestData.Length;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            using (Stream stream = authWebRequest.GetRequestStream())
            {
                stream.Write(_authRequestData, 0, _authRequestData.Length);
                stream.Flush();
            }

            SFAuthenticationResponse authResponse;
            using (WebResponse authWebResponse = authWebRequest.GetResponse())
            {
                DataContractJsonSerializer _serializer = new DataContractJsonSerializer(typeof(SFAuthenticationResponse));
                authResponse = _serializer.ReadObject(authWebResponse.GetResponseStream()) as SFAuthenticationResponse;
            }
            return authResponse.access_token;
        }

        /// <summary>
        /// Uploads the data to the SalesForce and Gets the response back
        /// </summary>
        /// <param name="_selectedTenantId"></param>
        /// <param name="_dataToUpload"></param>
        /// <param name="_uploadRequestUrl"></param>
        /// <param name="access_token"></param>
        private static void UploadComplianceData(Int32 _selectedTenantId, String _dataToUpload, String _uploadRequestUrl, String access_token)
        {
            Byte[] _userDataToPost = Encoding.ASCII.GetBytes(_dataToUpload);

            HttpWebRequest updateWebRequest = (HttpWebRequest)WebRequest.Create(_uploadRequestUrl);
            updateWebRequest.Method = HttpRequestTypes.POST.GetStringValue();
            updateWebRequest.ContentType = HttpRequestContentTypes.JSON.GetStringValue();
            updateWebRequest.Headers.Add("Authorization", "Bearer " + access_token);

            updateWebRequest.ContentLength = _userDataToPost.Length;

            using (Stream stream = updateWebRequest.GetRequestStream())
            {
                stream.Write(_userDataToPost, 0, _userDataToPost.Length);
                stream.Flush();
            }

            SFUpdateResponse updateResponse = new SFUpdateResponse();
            using (WebResponse updateWebResponse = updateWebRequest.GetResponse())
            {
                //changes done regarding UAt-1081
                var _streamReader = new StreamReader(updateWebResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                updateResponse.response = _streamReader.ReadToEnd();
                _streamReader.Close();
                //DataContractJsonSerializer _serializer = new DataContractJsonSerializer(typeof(SFUpdateResponse));
                //updateResponse = _serializer.ReadObject(updateWebResponse.GetResponseStream()) as SFUpdateResponse;
            }

            StoredProcedureManagers.SaveComplianceUploadServiceHistory(_dataToUpload, updateResponse.response, _selectedTenantId);
        }

        /// <summary>
        /// Auto renew Invoice Order Subscription and Executes Business rules
        /// </summary>
        public static void AutoRenewExpiredInvoiceSubscription(List<Entity.ClientEntity.Order> lstOrder, Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                logger.Info("******************* Calling AutoRenewExpiredInvoiceSubscription: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                logger.Trace("******************* Started Auto Renew Invoice Order Subscription: " + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                Dictionary<Int32, String> dictAutoRenewedOrderInvoice = new Dictionary<Int32, String>();
                //get data from newly added table based on tenant id and call business method by passing parameter
                lstOrder.ForEach(order =>
                {
                    var _invoiceNumbers = ComplianceDataManager.AutoSubmitOrder(tenant_Id.Value, order.OrderID);

                    foreach (var invNumber in _invoiceNumbers)
                    {
                        dictAutoRenewedOrderInvoice.Add(order.OrderID, invNumber.Value);
                    }
                });

                logger.Trace("******************* Ended Auto Renew Invoice Order Subscription:" + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                ServiceContext.ReleaseDBContextItems();
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in AutoRenewExpiredInvoiceSubscription method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        public static void ScheduleTaskForAutoRenewExpiredinvoiceSubscription()
        {
            // Get All Tenant
            List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
             item =>
             {
                 ClientDBConfiguration config = new ClientDBConfiguration();
                 config.CDB_TenantID = item.CDB_TenantID;
                 config.CDB_ConnectionString = item.CDB_ConnectionString;
                 config.Tenant = new Tenant();
                 config.Tenant.TenantName = item.Tenant.TenantName; return config;
             }).ToList();

            foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
            {
                if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                {
                    DateTime jobStartTime = DateTime.Now;
                    DateTime jobEndTime;
                    Int32 tenantId = clntDbConf.CDB_TenantID;
                    List<ScheduleTaskContract> lstScheduleTasks = ComplianceDataManager.GetScheduleTasksToProcess(tenantId, TaskType.RENEWEXPIREDINVOICESUBSCRIPTION.GetStringValue(), AppConsts.CHUNK_SIZE_FOR_APPROVE_INVOICE_ORDER_APPROVAL);
                    foreach (ScheduleTaskContract task in lstScheduleTasks)
                    {
                        logger.Trace("******************* Started placing entry for Schedule Task for Auto Renew Expired Invoice Subscription: " + DateTime.Now.ToString() + " *******************");

                        ScheduleAction.AutoRenewExpiredInvoiceSubscription(ComplianceDataManager.GetInvoiceOrdersForAutoRenew(tenantId).ToList(), tenantId);

                        logger.Trace("******************* Ended placing entry for Schedule Task for Auto Renew Expired Invoice Subscription:" + DateTime.Now.ToString() + " *******************");
                    }

                    //Save service logging data to DB
                    if (_isServiceLoggingEnabled)
                    {
                        jobEndTime = DateTime.Now;
                        Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                        Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                        ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                        serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                        serviceLoggingContract.JobName = JobName.ScheduleTaskForAutoRenewExpiredinvoiceSubscription.GetStringValue();
                        serviceLoggingContract.TenantID = tenantId;
                        serviceLoggingContract.JobStartTime = jobStartTime;
                        serviceLoggingContract.JobEndTime = jobEndTime;
                        //serviceLoggingContract.Comments = "";
                        serviceLoggingContract.IsDeleted = false;
                        serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                        serviceLoggingContract.CreatedOn = DateTime.Now;
                        SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                    }
                }
            }
        }

        public static ScheduledTaskContract GetScheduleTaskXMLParameters(String xml)
        {
            ScheduledTaskContract parameters = new ScheduledTaskContract();

            if (!xml.IsNullOrEmpty())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                XmlNodeList xnList = xmlDoc.SelectNodes("/ScheduledTaskContract");
                foreach (XmlNode xn in xnList)
                {
                    parameters.OrderId = xn["OrderId"].IsNullOrEmpty() ? 0 : Convert.ToInt32(xn["OrderId"].InnerText);
                    parameters.PackageId = xn["PackageId"].IsNullOrEmpty() ? 0 : Convert.ToInt32(xn["PackageId"].InnerText);
                    parameters.OrganisationUserId = xn["OrganisationUserId"].IsNullOrEmpty() ? 0 : Convert.ToInt32(xn["OrganisationUserId"].InnerText);
                    parameters.ReferenceNumber = xn["ReferenceNumber"].IsNullOrEmpty() ? String.Empty : Convert.ToString(xn["ReferenceNumber"].InnerText);
                    parameters.ExpiryDate = xn["ExpiryDate"].IsNullOrEmpty() ? DateTime.Now : Convert.ToDateTime(xn["ExpiryDate"].InnerText);
                    parameters.OrderStatusCode = xn["OrderStatusCode"].IsNullOrEmpty() ? String.Empty : Convert.ToString(xn["OrderStatusCode"].InnerText);
                    parameters.ApprovedBy = xn["ApprovedBy"].IsNullOrEmpty() ? 0 : Convert.ToInt32(xn["ApprovedBy"].InnerText);
                    parameters.SubEventCode = xn["SubEventCode"].IsNullOrEmpty() ? String.Empty : Convert.ToString(xn["SubEventCode"].InnerText);
                    parameters.ReportName = xn["ReportName"].IsNullOrEmpty() ? String.Empty : Convert.ToString(xn["ReportName"].InnerText);
                    parameters.FileExtension = xn["FileExtension"].IsNullOrEmpty() ? String.Empty : Convert.ToString(xn["FileExtension"].InnerText.Trim());
                    parameters.CutOffTime = xn["CutOffTime"].IsNullOrEmpty() ? -1 : Convert.ToInt32(xn["CutOffTime"].InnerText);
                    parameters.OrderPaymentDetailId = xn["OrderPaymentDetailId"].IsNullOrEmpty() ? 0 : Convert.ToInt32(xn["OrderPaymentDetailId"].InnerText);
                    parameters.DaysOfWeeks = xn["DaysOfWeeks"].IsNullOrEmpty() ? String.Empty : Convert.ToString(xn["DaysOfWeeks"].InnerText);
                }
            }
            return parameters;
        }

        public static void SendRecurringBackgroundReports(Int32? tenantId = null)
        {
            try
            {
                //ServiceContext.init();
                logger.Info("******************* Calling SendRecurringBackgroundReports: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                // Get All Tenant
                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenantId != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenantId).ToList();
                }

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 currentTenantId = clntDbConf.CDB_TenantID;
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Trace("******************* Started SendRecurringBackgroundReports: " + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");


                            //List<RecurringBackgroundReport> lstRecurringBackgroundReports = Enum.GetValues(typeof(RecurringBackgroundReport)).To<List<RecurringBackgroundReport>>();
                            List<RecurringBackgroundReport> lstRecurringBackgroundReports = Enum.GetValues(typeof(RecurringBackgroundReport))
                                                                                                    .Cast<RecurringBackgroundReport>().ToList();
                            foreach (RecurringBackgroundReport recurringBackgroundReport in lstRecurringBackgroundReports)
                            {
                                Dictionary<Int32, String> dictUpdatedTaskStatus = new Dictionary<Int32, String>();
                                List<ScheduleTaskContract> lstScheduleTasks = ComplianceDataManager.GetScheduleTasksToProcess(currentTenantId
                                                                                , recurringBackgroundReport.GetStringValue(),
                                                                                AppConsts.CHUNK_SIZE_FOR_DAILY_BACKGROUND_REPORT);
                                executeLoop = lstScheduleTasks.Count > 0 ? true : false;
                                foreach (ScheduleTaskContract ScheduleTask in lstScheduleTasks)
                                {
                                    try
                                    {
                                        Boolean isCorrectDayOfWeek = true;
                                        ScheduledTaskContract parameters = GetScheduleTaskXMLParameters(ScheduleTask.TaskXMLParameters.ToString());
                                        if (!parameters.IsNullOrEmpty() && !parameters.DaysOfWeeks.IsNullOrEmpty())
                                        {
                                            List<Int32> daysOfWeek = parameters.DaysOfWeeks.Split(',').Select(col => Convert.ToInt32(col)).ToList();
                                            if (!daysOfWeek.Contains((Int32)DateTime.Now.DayOfWeek))
                                            {
                                                isCorrectDayOfWeek = false;
                                            }
                                        }
                                        dictUpdatedTaskStatus.Add(ScheduleTask.ScheduleTaskID, String.Empty);
                                        if (!parameters.IsNullOrEmpty() && isCorrectDayOfWeek)
                                        {

                                            Dictionary<String, List<BackgroundOrderDailyReport>> _dicPermissionsSubscriptionSettings = StoredProcedureManagers.GetPermissionsSubscriptionSettings(parameters.SubEventCode, parameters.ReportName, currentTenantId);
                                            if (!_dicPermissionsSubscriptionSettings.IsNullOrEmpty())
                                            {
                                                DateTime dataCaptureTime = _dicPermissionsSubscriptionSettings.FirstOrDefault().Value.Select(col => col.CaptureDateTime).FirstOrDefault();
                                                Int32 serviceExecutionHistoryId = _dicPermissionsSubscriptionSettings.FirstOrDefault().Value.Select(col => col.ServiceExecutionHistoryId).FirstOrDefault();
                                                DateTime? startDateToFetchOrder = _dicPermissionsSubscriptionSettings.FirstOrDefault().Value.Select(col => col.FromDate).FirstOrDefault();

                                                //call to generate report and send mail
                                                GenerateReportAndSendMail(currentTenantId, _dicPermissionsSubscriptionSettings,
                                                    backgroundProcessUserId, dataCaptureTime, startDateToFetchOrder, parameters,
                                                    ScheduleTask.TaskTypeCode.Equals(RecurringBackgroundReport.WEEKLY_BACKGROUND_REPORT.GetStringValue()));

                                                //code to update ServiceExecutionHistory table by fetching DataFetchTime from _dicPermissionsSubscriptionSettings
                                                DateTime serviceEndTime = DateTime.Now;
                                                String serviceName = parameters.ReportName;
                                                Entity.ClientEntity.BackgroundServiceExecutionHistory bkgSvcHistory = new Entity.ClientEntity.BackgroundServiceExecutionHistory()
                                                {
                                                    BSEH_EndTime = serviceEndTime,
                                                };
                                                ComplianceDataManager.UpdateBackgroundServiceExecutionHistory(currentTenantId, bkgSvcHistory, serviceExecutionHistoryId);
                                            }

                                        }

                                        //code to update scheduleTask table to update waitUntil field  
                                        ComplianceDataManager.UpdateScheduleTaskStatus(currentTenantId, dictUpdatedTaskStatus, backgroundProcessUserId);
                                    }

                                    catch (Exception ex)
                                    {
                                        logger.Error("An Error has occured in SendRecurringBackgroundReports method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                        ComplianceDataManager.UpdateScheduleTaskStatus(currentTenantId, dictUpdatedTaskStatus, backgroundProcessUserId);
                                    }

                                }
                            }

                            logger.Trace("******************* Ended SendRecurringBackgroundReports:" + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                            ServiceContext.ReleaseDBContextItems();
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendColorFlaggedOrderReports.GetStringValue();
                            serviceLoggingContract.TenantID = currentTenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in SendRecurringBackgroundReports method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        private static void GenerateReportAndSendMail(Int32 tenantId, Dictionary<String, List<BackgroundOrderDailyReport>> nodePermissionSubscriptionData,
                                    Int32 backgroundProcessUserId, DateTime dataCaptureTime, DateTime? startDateToFetchOrder,
                                            ScheduledTaskContract scheduleTasksParameters, Boolean isWeeklyReport)
        {
            String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);
            foreach (var key in nodePermissionSubscriptionData)
            {
                Int32? systemCommunicationID = null;
                Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, "Admin");
                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, String.Concat(applicationUrl));

                CommunicationMockUpData mockData = new CommunicationMockUpData();
                mockData.UserName = "Admin";
                mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                String fromDateParameter;
                if (!startDateToFetchOrder.HasValue)
                {
                    if (isWeeklyReport)//scheduleTasksParameters.SubEventCode == CommunicationSubEvents.WEEKLY_INCOMPLETE_DRUG_SCREEN_ORDERS_NOTIFICATION_AND_REPORT.GetStringValue())
                    {
                        fromDateParameter = DateTime.Now.Date.AddDays(-7).ToShortDateString();
                    }
                    else if (scheduleTasksParameters.CutOffTime >= 0 && DateTime.Now.Hour < scheduleTasksParameters.CutOffTime)
                    {
                        fromDateParameter = DateTime.Now.Date.AddDays(-1).ToShortDateString();
                    }
                    else
                    {
                        fromDateParameter = DateTime.Now.Date.ToShortDateString();
                    }
                }
                else if (scheduleTasksParameters.CutOffTime >= 0 && startDateToFetchOrder.Value.Hour < scheduleTasksParameters.CutOffTime)
                {
                    fromDateParameter = startDateToFetchOrder.Value.Date.AddDays(-1).ToShortDateString();
                }
                else
                {
                    fromDateParameter = startDateToFetchOrder.Value.Date.ToShortDateString();
                }

                String toDateParameter = null;
                if (isWeeklyReport)//scheduleTasksParameters.SubEventCode == CommunicationSubEvents.WEEKLY_INCOMPLETE_DRUG_SCREEN_ORDERS_NOTIFICATION_AND_REPORT.GetStringValue())
                {
                    toDateParameter = DateTime.Now.Date.ToShortDateString();
                }


                dictMailData.Add(EmailFieldConstants.FROM_DATE, fromDateParameter);
                dictMailData.Add(EmailFieldConstants.END_DATE, toDateParameter);

                //Create Attachment
                ParameterValue[] parameters = new ParameterValue[6];
                parameters[0] = new ParameterValue();
                parameters[0].Name = "TenantID";
                parameters[0].Value = "1";

                parameters[1] = new ParameterValue();
                parameters[1].Name = "UserID";
                parameters[1].Value = "1";

                parameters[2] = new ParameterValue();
                parameters[2].Name = "Institute";
                parameters[2].Value = tenantId.ToString();

                parameters[3] = new ParameterValue();
                parameters[3].Name = "Hierarchy";
                parameters[3].Value = key.Key;

                parameters[4] = new ParameterValue();
                parameters[4].Name = "FromDate";
                parameters[4].Value = fromDateParameter;

                parameters[5] = new ParameterValue();
                parameters[5].Name = "ToDate";
                parameters[5].Value = toDateParameter;


                String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                String fileName = scheduleTasksParameters.ReportName + "_" + tenantId.ToString() + "_"
                                        + date + (scheduleTasksParameters.FileExtension.Equals(String.Empty) ? ".pdf" : scheduleTasksParameters.FileExtension);

                byte[] reportContent = ReportManager.GetReportByteArray(scheduleTasksParameters.ReportName, parameters);
                String retFilepath = ReportManager.ConvertByteArrayToReportFile(reportContent, fileName);

                SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                sysCommAttachment.SCA_OriginalDocumentID = -1;
                sysCommAttachment.SCA_OriginalDocumentName = scheduleTasksParameters.ReportName
                                                        + (scheduleTasksParameters.FileExtension.Equals(String.Empty) ? ".pdf" : scheduleTasksParameters.FileExtension);
                sysCommAttachment.SCA_DocumentPath = retFilepath;
                sysCommAttachment.SCA_DocumentSize = reportContent.Length;
                sysCommAttachment.SCA_DocAttachmentTypeID = GetAttachmentDocumentType(DocumentAttachmentType.DAILY_REPORT.GetStringValue());
                sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                sysCommAttachment.SCA_IsDeleted = false;
                sysCommAttachment.SCA_CreatedBy = backgroundProcessUserId;
                sysCommAttachment.SCA_CreatedOn = DateTime.Now;

                #region Code to generate report url place holder value

                Dictionary<String, String> reportUrlParameters = new Dictionary<String, String>
                                                         {
                                                            { "UserID",  AppConsts.ONE.ToString()},
                                                            { "Hierarchy",  key.Key},
                                                             { "ReportType", scheduleTasksParameters.ReportName},
                                                             { "TenantId", AppConsts.ONE.ToString()},
                                                             { "Institute", tenantId.ToString()},
                                                             { "FromDate", fromDateParameter},
                                                             { "ToDate", String.Empty},
                                                             {"HierarchyNodeID",Convert.ToString(AppConsts.MINUS_ONE)},
                                                             {"IsReportSentToStudent",Convert.ToString(false)}
                                                         };
                StringBuilder reportUrl = new StringBuilder();
                reportUrl.Append(applicationUrl.Trim() + "?args=");
                reportUrl.Append(reportUrlParameters.ToEncryptedQueryString());

                #endregion

                dictMailData.Add(EmailFieldConstants.REPORT_URL, reportUrl.ToString());

                systemCommunicationID = CommunicationManager.SendRecurringBackgroundReports(scheduleTasksParameters.SubEventCode.ParseEnumbyCode<CommunicationSubEvents>(),
                                                                                        dictMailData, mockData, tenantId, -1, null, null, true, true, key.Value, false);

                if (systemCommunicationID != null)
                {
                    sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                    CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                }
            }
        }

        /// <summary>
        /// Get the document attachment type Id by code
        /// </summary>
        /// <param name="docAttachmentTypeCode"></param>
        /// <returns></returns>
        private static Int16 GetAttachmentDocumentType(String docAttachmentTypeCode)
        {
            List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();

            return !docAttachmentType.IsNullOrEmpty()
                    ? Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID)
                    : Convert.ToInt16(AppConsts.NONE);
        }

        /// <summary>
        /// Automatically Archive Expired Subscriptions whose Grace Period has also passsed
        /// </summary>
        public static void AutoArchiveExpiredSubscriptions(Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                logger.Info("******************* Calling AutoArchiveExpiredSubscriptions: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                // Get All Tenant
                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {

                            logger.Trace("******************* Started AutoArchiveExpiredSubscriptions: " + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                            Dictionary<Int32, String> dictUpdatedTaskStatus = new Dictionary<Int32, String>();
                            List<ScheduleTaskContract> lstScheduleTasks = ComplianceDataManager.GetScheduleTasksToProcess(tenantId, TaskType.AUTO_ARCHIVE_EXPIRED_SUBSCRIPTION.GetStringValue(), AppConsts.CHUNK_SIZE_FOR_ARCHIVE_EXPIRED_SUBSCRIPTION);
                            executeLoop = lstScheduleTasks.Count > 0 ? true : false;
                            lstScheduleTasks.ForEach(scheduledTask =>
                            {
                                try
                                {
                                    dictUpdatedTaskStatus.Add(scheduledTask.ScheduleTaskID, String.Empty);
                                    logger.Trace("******************* Calling AutomaticallyArchiveExpiredSubscriptions : " + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                                    ComplianceDataManager.AutomaticallyArchiveExpiredSubscriptions(tenantId, backgroundProcessUserId);

                                    Dictionary<String, Object> dataDictionary = new Dictionary<String, Object>();
                                    dataDictionary.Add("TenantID", tenantId);
                                    QueueImagingManager.ParalleAssignQueueImagingRepoInstance(dataDictionary);

                                    logger.Trace("******************* Ended AutomaticallyArchiveExpiredSubscriptions : " + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                                    //code to update scheduleTask table to update waitUntil field  
                                    ComplianceDataManager.UpdateScheduleTaskStatus(tenantId, dictUpdatedTaskStatus, backgroundProcessUserId);
                                }
                                catch (Exception ex)
                                {
                                    logger.Error("An Error has occured in ArchiveExpiredSubscriptions, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                    //code to update scheduleTask table to update waitUntil field  
                                    ComplianceDataManager.UpdateScheduleTaskStatus(tenantId, dictUpdatedTaskStatus, backgroundProcessUserId);
                                }
                            });

                            logger.Trace("******************* Ended AutoArchiveExpiredSubscriptions:" + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                            ServiceContext.ReleaseDBContextItems();
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.AutoArchiveExpiredSubscriptions.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in AutoArchiveExpiredSubscriptions method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        public static void SendNotificationForAutoArchivedExpiredSubscriptions(Int32? tenantId = null)
        {
            try
            {
                //ServiceContext.init();
                logger.Info("******************* Calling SendNotificationForAutoArchivedExpiredSubscriptions: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                // Get All Tenant
                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenantId != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenantId).ToList();
                }

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 currentTenantId = clntDbConf.CDB_TenantID;
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Trace("******************* Started SendNotificationForAutoArchivedExpiredSubscriptions: " + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                            Dictionary<Int32, String> dictUpdatedTaskStatus = new Dictionary<Int32, String>();
                            List<ScheduleTaskContract> lstScheduleTasks = ComplianceDataManager.GetScheduleTasksToProcess(currentTenantId, TaskType.AUTO_ARCHIVE_EXPIRED_SUBSCRIPTION_NOTIFICATION.GetStringValue(), AppConsts.CHUNK_SIZE_FOR_ARCHIVE_EXPIRED_SUBSCRIPTION_NOTIFICATION);
                            executeLoop = lstScheduleTasks.Count > 0 ? true : false;
                            foreach (ScheduleTaskContract ScheduleTask in lstScheduleTasks)
                            {
                                try
                                {
                                    ScheduledTaskContract parameters = GetScheduleTaskXMLParameters(ScheduleTask.TaskXMLParameters.ToString());
                                    if (!parameters.IsNullOrEmpty())
                                    {
                                        dictUpdatedTaskStatus.Add(ScheduleTask.ScheduleTaskID, String.Empty);
                                        Dictionary<String, List<BackgroundOrderDailyReport>> _dicPermissionsSubscriptionSettings = StoredProcedureManagers.GetPermissionsSubscriptionSettings(parameters.SubEventCode, BackgroundServiceType.DAILY_AUTO_ARCHIVE_EXPIRED_SUBSCRIPTION_NOTIFICATION.GetStringValue(), currentTenantId);
                                        if (!_dicPermissionsSubscriptionSettings.IsNullOrEmpty())
                                        {
                                            DateTime dataCaptureTime = _dicPermissionsSubscriptionSettings.FirstOrDefault().Value.Select(col => col.CaptureDateTime).FirstOrDefault();
                                            Int32 serviceExecutionHistoryId = _dicPermissionsSubscriptionSettings.FirstOrDefault().Value.Select(col => col.ServiceExecutionHistoryId).FirstOrDefault();
                                            DateTime? startDateToFetchOrder = _dicPermissionsSubscriptionSettings.FirstOrDefault().Value.Select(col => col.FromDate).FirstOrDefault();

                                            //call to Fetch AutoArchivedOrders And Send Notification
                                            FetchAutoArchivedOrdersAndSendNotification(currentTenantId, _dicPermissionsSubscriptionSettings, backgroundProcessUserId, dataCaptureTime, startDateToFetchOrder, parameters);

                                            //code to update ServiceExecutionHistory table by fetching DataFetchTime from _dicPermissionsSubscriptionSettings
                                            DateTime serviceEndTime = DateTime.Now;
                                            String serviceName = parameters.ReportName;
                                            Entity.ClientEntity.BackgroundServiceExecutionHistory bkgSvcHistory = new Entity.ClientEntity.BackgroundServiceExecutionHistory()
                                            {
                                                BSEH_EndTime = serviceEndTime,
                                            };
                                            ComplianceDataManager.UpdateBackgroundServiceExecutionHistory(currentTenantId, bkgSvcHistory, serviceExecutionHistoryId);
                                        }
                                    }

                                    //code to update scheduleTask table to update waitUntil field  
                                    ComplianceDataManager.UpdateScheduleTaskStatus(currentTenantId, dictUpdatedTaskStatus, backgroundProcessUserId);
                                }

                                catch (Exception ex)
                                {
                                    logger.Error("An Error has occured in SendNotificationForAutoArchivedExpiredSubscriptions method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                    ComplianceDataManager.UpdateScheduleTaskStatus(currentTenantId, dictUpdatedTaskStatus, backgroundProcessUserId);
                                }

                            }
                            logger.Trace("******************* Ended SendNotificationForAutoArchivedExpiredSubscriptions:" + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                            ServiceContext.ReleaseDBContextItems();
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendNotificationForAutoArchivedExpiredSubscriptions.GetStringValue();
                            serviceLoggingContract.TenantID = currentTenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in SendNotificationForAutoArchivedExpiredSubscriptions method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }


        private static void FetchAutoArchivedOrdersAndSendNotification(Int32 tenantId, Dictionary<String, List<BackgroundOrderDailyReport>> nodePermissionSubscriptionData,
                                    Int32 backgroundProcessUserId, DateTime dataCaptureTime, DateTime? startDateToFetchOrder, ScheduledTaskContract scheduleTasksParameters)
        {
            String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);
            foreach (var key in nodePermissionSubscriptionData)
            {
                Int32? systemCommunicationID = null;
                String fromDateParameter;
                if (!startDateToFetchOrder.HasValue)
                {
                    if (scheduleTasksParameters.CutOffTime >= 0 && DateTime.Now.Hour < scheduleTasksParameters.CutOffTime)
                    {
                        fromDateParameter = DateTime.Now.Date.AddDays(-1).ToShortDateString();
                    }
                    else
                    {
                        fromDateParameter = DateTime.Now.Date.ToShortDateString();
                    }
                }
                else if (scheduleTasksParameters.CutOffTime >= 0 && startDateToFetchOrder.Value.Hour < scheduleTasksParameters.CutOffTime)
                {
                    fromDateParameter = startDateToFetchOrder.Value.Date.AddDays(-1).ToShortDateString();
                }
                else
                {
                    fromDateParameter = startDateToFetchOrder.Value.Date.ToShortDateString();
                }
                String applicantNamesWithOrderNumbers = ComplianceDataManager.GetApplicantAndTheirOrdersFromHierarchyIds(Convert.ToString(key.Key), fromDateParameter, null, tenantId);
                Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, "Admin");
                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, String.Concat(applicationUrl));
                dictMailData.Add(EmailFieldConstants.FROM_DATE, !startDateToFetchOrder.HasValue ? "-" : Convert.ToString(startDateToFetchOrder.Value.Date.ToShortDateString()));
                dictMailData.Add(EmailFieldConstants.END_DATE, Convert.ToString(dataCaptureTime.Date.ToShortDateString()));
                dictMailData.Add(EmailFieldConstants.APPLICANT_NAMES_WITH_ORDER_NUMBERS, applicantNamesWithOrderNumbers);

                CommunicationMockUpData mockData = new CommunicationMockUpData();
                mockData.UserName = "Admin";
                mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                mockData.ReceiverOrganizationUserID = backgroundProcessUserId;

                //send mail
                systemCommunicationID = CommunicationManager.SendRecurringBackgroundReports(scheduleTasksParameters.SubEventCode.ParseEnumbyCode<CommunicationSubEvents>(),
                dictMailData, mockData, tenantId, -1, null, null, true, true, key.Value, false);

                if (systemCommunicationID.IsNotNull() && systemCommunicationID > 0)
                {
                    //send message
                    CommunicationManager.SaveMessageContent(scheduleTasksParameters.SubEventCode.ParseEnumbyCode<CommunicationSubEvents>(), dictMailData, backgroundProcessUserId, tenantId);
                }
            }
        }

        public static void ScheduleActionExecuteRulesOnObjectDeletion(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling ScheduleActionExecuteRulesOnObjectDeletion: " + DateTime.Now.ToString() + " *******************");


                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                    item =>
                    {
                        ClientDBConfiguration config = new ClientDBConfiguration();
                        config.CDB_TenantID = item.CDB_TenantID;
                        config.CDB_ConnectionString = item.CDB_ConnectionString;
                        config.Tenant = new Tenant();
                        config.Tenant.TenantName = item.Tenant.TenantName; return config;
                    }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Trace("******************* Started executeing schedule action of chunk of action items: " + DateTime.Now.ToString() + " *******************");
                            executeLoop = RuleManager.ProcessActionExecuteRulesOnObjectDeletion(tenantId, tenantName, AppConsts.CHUNK_SIZE_FOR_CATEGORY_RULE_REOCCUR);
                            logger.Trace("******************* Ended executeing schedule action of chunk of action items::" + DateTime.Now.ToString() + " *******************");
                            ServiceContext.ReleaseDBContextItems();
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                            Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.ScheduleActionExecuteRulesOnObjectDeletion.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in SendMailForScheduleActionExecuteCategoryrules method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        //UAT-1852 : If a Service Group is complete, but other service groups are not complete within an order, the system should send the applicant an email.
        public static void SendMailForBkgSvcGrpCompletion(Int32? tenant_Id = null)
        {

            try
            {
                //ServiceContext.init();
                logger.Info("******************* Calling Service Group Completion Mail For Applicant: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                // Get All Tenant
                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    try
                    {
                        if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;
                            Int32 tenantId = clntDbConf.CDB_TenantID;
                            Boolean executeLoop = true;
                            while (executeLoop)
                            {

                                logger.Trace("******************* Started Service Group Completion Mail For Applicant: " + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                                Dictionary<Int32, String> dictUpdatedTaskStatus = new Dictionary<Int32, String>();
                                List<ScheduleTaskContract> lstScheduleTasks = ComplianceDataManager.GetScheduleTasksToProcess(tenantId, TaskType.SEND_MAIL_BKG_SVC_GROUPS_COMPLETION.GetStringValue(), AppConsts.CHUNK_SIZE_FOR_BKG_SVC_GRPS_COMPLETION);
                                executeLoop = lstScheduleTasks.Count > 0 ? true : false;
                                lstScheduleTasks.ForEach(scheduledTask =>
                                {
                                    try
                                    {
                                        dictUpdatedTaskStatus.Add(scheduledTask.ScheduleTaskID, String.Empty);
                                        logger.Trace("******************* Calling Service Group Completion Mail For Applicant : " + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                                        BackgroundProcessOrderManager.SendingMailForBkgSvcGrpCompletion(tenantId, backgroundProcessUserId);
                                        logger.Trace("******************* Ended Service Group Completion Mail For Applicant : " + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                                        //code to update scheduleTask table to update waitUntil field  
                                        ComplianceDataManager.UpdateScheduleTaskStatus(tenantId, dictUpdatedTaskStatus, backgroundProcessUserId);
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.Error("An Error has occured in SendMailForBkgSvcGrpCompletion method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                        //code to update scheduleTask table to update waitUntil field  
                                        ComplianceDataManager.UpdateScheduleTaskStatus(tenantId, dictUpdatedTaskStatus, backgroundProcessUserId);
                                    }
                                });

                                logger.Trace("******************* Ended Service Group Completion Mail For Applicant:" + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                                ServiceContext.ReleaseDBContextItems();
                            }
                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.SvcGrpCompletionMailForApplicant.GetStringValue();
                                serviceLoggingContract.TenantID = tenantId;
                                serviceLoggingContract.JobStartTime = jobStartTime;
                                serviceLoggingContract.JobEndTime = jobEndTime;
                                serviceLoggingContract.IsDeleted = false;
                                serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                                serviceLoggingContract.CreatedOn = DateTime.Now;
                                SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("An Error has occured in SendMailForBkgSvcGrpCompletions method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString + " TenantId : " + clntDbConf.CDB_TenantID);
                        ServiceContext.ReleaseDBContextItems();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in SendMailForBkgSvcGrpCompletions method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }



        public static void ProcessRequirementScheduleActionExecuteCategoryRules(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling ProcessRequirementScheduleActionExecuteCategoryRules: " + DateTime.Now.ToString() + " *******************");


                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                    item =>
                    {
                        ClientDBConfiguration config = new ClientDBConfiguration();
                        config.CDB_TenantID = item.CDB_TenantID;
                        config.CDB_ConnectionString = item.CDB_ConnectionString;
                        config.Tenant = new Tenant();
                        config.Tenant.TenantName = item.Tenant.TenantName; return config;
                    }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Trace("******************* Started executeing requirement schedule action of chunk of action items: " + DateTime.Now.ToString() + " *******************");
                            executeLoop = RuleManager.ProcessActionExecuteRulesOnObjectDeletion(tenantId, tenantName, AppConsts.CHUNK_SIZE_FOR_CATEGORY_RULE_REOCCUR);
                            logger.Trace("******************* Ended executeing requirement schedule action of chunk of action items::" + DateTime.Now.ToString() + " *******************");
                            ServiceContext.ReleaseDBContextItems();
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                            Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.ScheduleActionExecuteRulesOnObjectDeletion.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in ProcessRequirementScheduleActionExecuteCategoryRules method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        public static void BackgroundCopyPackgeData(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling BackgroundCopyPackgeData: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                // Get All Tenant
                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Trace("******************* Started Background Copying Package Data: " + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                            executeLoop = ComplianceDataManager.CopyPackageData(tenantId, AppConsts.CHUNK_SIZE_FOR_BKG_COPY_PACKAGE_DATA, backgroundProcessUserId);
                            logger.Trace("******************* Ended Background Copying Package Data:" + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                            ServiceContext.ReleaseDBContextItems();
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.BackgroundCopyPackageData.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in BackgroundCopyPackgeData method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        public static void BackgroundAppointmentScheduleDigestion()
        {
            try
            {
                DateTime StartTime = DateTime.Now;
                Int32 chunkSize = 10;
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ScheduleDigestionChunkSize"))
                    chunkSize = Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ScheduleDigestionChunkSize"]) ? ConfigurationManager.AppSettings["ScheduleDigestionChunkSize"] : chunkSize.ToString());


                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                List<Int32> ScheduleMasterIds = FingerPrintSetUpManager.GetLocationMasterSchedules();

                for (int i = 0; i < ScheduleMasterIds.Count; i += chunkSize)
                {
                    string SmIDs = string.Join(",", ScheduleMasterIds.Skip(i).Take(chunkSize).Select(n => n.ToString()).ToArray());
                    FingerPrintSetUpManager.CallBackgroungDigestionProcedure(backgroundProcessUserId, SmIDs);
                }
                if (_isServiceLoggingEnabled)
                {
                    DateTime jobStartTime = StartTime;
                    DateTime jobEndTime;
                    jobEndTime = DateTime.Now;
                    ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                    serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                    serviceLoggingContract.JobName = JobName.BackgroundAppointmentDigestionCall.GetStringValue();

                    serviceLoggingContract.JobStartTime = jobStartTime;
                    serviceLoggingContract.JobEndTime = jobEndTime;
                    //serviceLoggingContract.Comments = "";
                    serviceLoggingContract.IsDeleted = false;
                    serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                    serviceLoggingContract.CreatedOn = DateTime.Now;
                    SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in CallBackgroungDigestionProcedure method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }


        #region UAT-3795
        public static void SendNonCompliantStudentsReportWeekly(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendNonCompliantStudentsReportWeekly: " + DateTime.Now.ToString() + " *******************");

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                    item =>
                    {
                        ClientDBConfiguration config = new ClientDBConfiguration();
                        config.CDB_TenantID = item.CDB_TenantID;
                        config.CDB_ConnectionString = item.CDB_ConnectionString;
                        config.Tenant = new Tenant();
                        config.Tenant.TenantName = item.Tenant.TenantName; return config;
                    }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Trace("******************* Started Non Compliant Students Weekly Report: " + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                            Dictionary<Int32, String> dictUpdatedTaskStatus = new Dictionary<Int32, String>();
                            List<ScheduleTaskContract> lstScheduleTasks = ComplianceDataManager.GetScheduleTasksToProcess(tenantId, TaskType.SEND_MAIL_NON_COMPLIANT_STUDENTS_WEEKLY_REPORT.GetStringValue(), AppConsts.NONE);
                            executeLoop = lstScheduleTasks.Count > 0 ? true : false;
                            lstScheduleTasks.ForEach(scheduledTask =>
                              {
                                  try
                                  {
                                      dictUpdatedTaskStatus.Add(scheduledTask.ScheduleTaskID, String.Empty);
                                      logger.Trace("******************* Calling Non Compliant Students Weekly Report : " + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");

                                      String subEventCode = CommunicationSubEvents.WEEKLY_NON_COMPLIANT_APPLICANT_REPORT_NOTIFICATION.GetStringValue();

                                      logger.Trace("******************* Started getting cc users with their hierarchy id(s) which are added to CC copy settings to subevent : " + DateTime.Now.ToString() + " *******************");
                                      List<INTSOF.UI.Contract.ComplianceOperation.WeeklyNonCompliantReportDataContract> lstWeeklyNonCompliantReportDataContract
                                                                                                                          = ComplianceDataManager.GetCCUsersForWeeklyNonComplaintReport(subEventCode, tenantId);
                                      if (!lstWeeklyNonCompliantReportDataContract.IsNullOrEmpty() && lstWeeklyNonCompliantReportDataContract.Count > AppConsts.NONE)
                                      {
                                          List<INTSOF.UI.Contract.ComplianceOperation.WeeklyNonCompliantReportDataContract> lstDistincedByHierarchyIds
                                              = lstWeeklyNonCompliantReportDataContract.Where(x => !x.HeirarchyNodeIds.IsNullOrEmpty() || x.OrganizationID == AppConsts.ONE).DistinctBy(con => con.HeirarchyNodeIds).ToList();

                                          foreach (var item in lstDistincedByHierarchyIds)
                                          {

                                              String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                                              String fileName = "Weekly_Non_Compliant_Students" + "_" + tenantId.ToString() + "_" + date + ".xls";

                                              String hierarchyId = item.HeirarchyNodeIds.IsNullOrEmpty() ? String.Empty : item.HeirarchyNodeIds;
                                              Int32 organizationID = item.OrganizationID;

                                              // fetch report data
                                              DataTable dtNonCompliantReportData = ComplianceDataManager.GetWeeklyNonCompliantReportData(tenantId, hierarchyId, organizationID);

                                              DataView dv = dtNonCompliantReportData.DefaultView;
                                              dtNonCompliantReportData = dv.ToTable();

                                              #region Saving table and coverting it in excel for appending in email as attachment.

                                              //Creating Excel
                                              byte[] reportContent = ExcelReader.ConvertingDataInExcelForNonCompliantStudentsReport(dtNonCompliantReportData, fileName);
                                              if (!reportContent.IsNullOrEmpty())
                                              {
                                                  String retFilepath = ReportManager.ConvertByteArrayToReportFile(reportContent, fileName);
                                                  if (!retFilepath.IsNullOrEmpty())
                                                  {
                                                      List<INTSOF.UI.Contract.ComplianceOperation.WeeklyNonCompliantReportDataContract> lstHavingSameHierarchyIds =
                                                          lstWeeklyNonCompliantReportDataContract.Where(con => con.HeirarchyNodeIds == hierarchyId).ToList();

                                                      //Send Mail
                                                      CommunicationManager.SendWeeklyNonCompliantReport(backgroundProcessUserId, tenantName, fileName, retFilepath, reportContent, lstHavingSameHierarchyIds);
                                                  }
                                              }
                                              #endregion
                                          }
                                      }
                                      logger.Trace("******************* Ended Non Compliant Students Weekly Report : " + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString() + " *******************");
                                      //code to update scheduleTask table to update waitUntil field  
                                      ComplianceDataManager.UpdateScheduleTaskStatus(tenantId, dictUpdatedTaskStatus, backgroundProcessUserId);
                                  }
                                  catch (Exception ex)
                                  {
                                      logger.Error("An Error has occured in SendNonCompliantStudentsReportWeekly method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                      //code to update scheduleTask table to update waitUntil field  
                                      ComplianceDataManager.UpdateScheduleTaskStatus(tenantId, dictUpdatedTaskStatus, backgroundProcessUserId);
                                  }
                              });
                            ServiceContext.ReleaseDBContextItems();
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendNonCompliantStudentsWeeklyReport.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in SendNonCompliantStudentsReportWeekly method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }
        #endregion


    }
}
