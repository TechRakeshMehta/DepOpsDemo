using Business.RepoManagers;
using Entity;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.Services;
using INTSOF.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using System.Data;
using INTSOF.UI.Contract.Templates;
using Business.ReportExecutionService;
using System.IO;

namespace EmailDispatcherService
{
    public class OrderNotification
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;


        private static Int32 _chunkSize = ConfigurationManager.AppSettings["ChunkSizeForIncompletedOnlineOrderNotifications"].IsNotNull() ?
                                                            Convert.ToInt32(ConfigurationManager.AppSettings["ChunkSizeForIncompletedOnlineOrderNotifications"]) : AppConsts.NONE;

        private static Int32 _maxRetryCount = ConfigurationManager.AppSettings["MaxRetryCountForIncompletedOnlineOrderNotifications"].IsNotNull() ?
                                                                    Convert.ToInt32(ConfigurationManager.AppSettings["MaxRetryCountForIncompletedOnlineOrderNotifications"]) : AppConsts.NONE;

        private static Int32 _retryTimeLag = ConfigurationManager.AppSettings["RetryTimeLagForIncompletedOnlineOrderNotifications"].IsNotNull() ?
                                                            Convert.ToInt32(ConfigurationManager.AppSettings["RetryTimeLagForIncompletedOnlineOrderNotifications"]) : AppConsts.NONE;

        private static string _notificationDetail = "Online Failed Order Notification";

        public static void SendOrderNotificationsForIncompleteOnlineOrders()
        {
            try
            {
                logger.Info("Calling NotificationForIncompleteOnlineOrders.");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                // Get All Tenant
                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Entity.Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        logger.Info("Starting sending incomplete online order notification emails for TenantID: " + clntDbConf.CDB_TenantID);
                        try
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;
                            Int32 tenantId = clntDbConf.CDB_TenantID;
                            Boolean IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(tenantId);
                            List<Entity.ClientEntity.lkpOrderNotificationType> orderNotificationType = BackgroundProcessOrderManager.GetOrderNotificationType(tenantId);
                            int onlineFailedOrderNotificationTypeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > 0 ?
                                                        Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == OrderNotificationType.ONLINE_FAILED_ORDER_NOTIFICATION.GetStringValue()).ONT_ID) : Convert.ToInt32(0);

                            Boolean executeLoop = true;
                            if (!IsLocationServiceTenant)
                            {
                                while (executeLoop)
                                {
                                    logger.Info("Started placing entry in NotificationForIncompleteOnlineOrders.");
                                    List<usp_GetIncompleteOnlineOrders_Result> lstOrders = ComplianceDataManager.GetIncompleteOnlineOrders(tenantId, _chunkSize, _maxRetryCount, _retryTimeLag);
                                    if (lstOrders.IsNotNull() && lstOrders.Count > AppConsts.NONE)
                                    {
                                        logger.Info("Total incomplete online orders for tenant (TenantID: " + clntDbConf.CDB_TenantID + ") found :" + lstOrders.Count());
                                        foreach (var order in lstOrders)
                                        {
                                            try
                                            {
                                                logger.Info("Starting sending incomplete online order Notifications for Order # :" + order.OrderID + ", tenant:" + clntDbConf.CDB_TenantID);

                                                CommunicationMockUpData mockData = new CommunicationMockUpData();
                                                Dictionary<String, object> dictMailData = new Dictionary<String, object>();

                                                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, string.Concat(order.FirstName, " ", order.LastName));
                                                dictMailData.Add(EmailFieldConstants.ORDER_NO, order.OrderNumber);
                                                dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, order.DPM_Label);
                                                dictMailData.Add(EmailFieldConstants.PAYMENT_METHOD, order.PaymentOptionName);
                                                //dictMailData.Add(EmailFieldConstants.APPLICATION_URL, WebSiteManager.GetInstitutionUrl(tenantId));

                                                mockData.UserName = string.Concat(order.FirstName, " ", order.LastName);
                                                mockData.EmailID = order.PrimaryEmailAddress;
                                                mockData.ReceiverOrganizationUserID = order.OrganizationUserID;

                                                int? systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_INCOMPLETE_ONLINE_ORDERS
                                                                                                                              , dictMailData, mockData, tenantId
                                                                                                                              , order.SelectedNodeID.HasValue ? order.SelectedNodeID.Value : 0
                                                                                                                              , null);

                                                logger.Info("End sending incomplete online order Notifications for Order # :" + order.OrderID + ", tenant:" + clntDbConf.CDB_TenantID);
                                                if (systemCommunicationID.HasValue)
                                                {
                                                    logger.Info("Starting saving order notification for Order#: " + order.OrderID + ", tenant:" + clntDbConf.CDB_TenantID);

                                                    //Update "IsNotificationSentForIncompleteOnlineOrders" in OPD
                                                    ComplianceDataManager.SaveUpdateOrderNotifications(tenantId, order.ONTF_ID, backgroundProcessUserId, order.OrderID, systemCommunicationID, (Int16)AppConsts.COMPLIO_BUSINESS_CHANNEL_TYPE, onlineFailedOrderNotificationTypeID, _notificationDetail);

                                                    //(tenantId, order.OPD_ID, backgroundProcessUserId);
                                                    logger.Info("End saving order notification for Order#: " + order.OrderID + ", tenant:" + clntDbConf.CDB_TenantID);

                                                    //Sending Message
                                                    logger.Info("Sending message for Order#: " + order.OrderID + ", tenant:" + clntDbConf.CDB_TenantID);
                                                    CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_INCOMPLETE_ONLINE_ORDERS, dictMailData, order.OrganizationUserID, tenantId);
                                                    logger.Info("Message sent for Order#: " + order.OrderID + ", tenant:" + clntDbConf.CDB_TenantID);
                                                }
                                                else
                                                {
                                                    logger.Info("An error occurred while getting system CommunicationID, for Order#: " + order.OrderID + ", tenant:" + clntDbConf.CDB_TenantID + " .System Communication ID null.");
                                                    logger.Info("Starting updating retry count for Order#: " + order.OrderID + ", tenant:" + clntDbConf.CDB_TenantID);
                                                    ComplianceDataManager.SaveUpdateOrderNotifications(tenantId, order.ONTF_ID, backgroundProcessUserId, order.OrderID, systemCommunicationID, (Int16)AppConsts.COMPLIO_BUSINESS_CHANNEL_TYPE, onlineFailedOrderNotificationTypeID, _notificationDetail);
                                                    logger.Info("End updating retry count for Order#: " + order.OrderID + ", tenant:" + clntDbConf.CDB_TenantID);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                //Log error
                                                logger.Error("An error occured while sending Notification for Incomplete Online Orders for Order#: {0}." + ", Tenant: {4}" +
                                                "The details of which are: {1}, Inner Exception: {2}, Stack Trace: {3}"
                                                , order.OrderID, ex.Message, ex.InnerException, clntDbConf.CDB_TenantID
                                                , ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);

                                                logger.Info("Inside Exception: Starting updating retry count for Order#: " + order.OrderID + ", tenant:" + clntDbConf.CDB_TenantID);
                                                ComplianceDataManager.SaveUpdateOrderNotifications(tenantId, order.ONTF_ID, backgroundProcessUserId, order.OrderID, null, (Int16)AppConsts.COMPLIO_BUSINESS_CHANNEL_TYPE, onlineFailedOrderNotificationTypeID, _notificationDetail);
                                                logger.Info("Inside Exception: End updating retry count for Order#: " + order.OrderID + ", tenant:" + clntDbConf.CDB_TenantID);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        executeLoop = false;
                                    }
                                    logger.Trace("Ended placing entry in NotificationForIncompleteOnlineOrders:");
                                    ServiceContext.ReleaseDBContextItems();
                                }
                            }
                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.NotificationForIncompleteOnlineOrders.GetStringValue();
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
                        catch (Exception ex)
                        {
                            //Log error
                            logger.Error("An error occured while sending Notification for Incomplete Online Orders for TenantID: {0}." +
                            "The details of which are: {1}, Inner Exception: {2}, Stack Trace: {3}"
                            , clntDbConf.CDB_TenantID, ex.Message, ex.InnerException
                            , ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in NotificationForIncompleteOnlineOrders method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}"
                              , ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        #region UAT-3669
        public static void SendAlertMailForWebCCFError()
        {
            try
            {
                DateTime jobStartTime = DateTime.Now;
                DateTime jobEndTime;

                logger.Info("******************* Calling Send Alert Mail For WebCCF Error: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;


                logger.Info("*******************  Store Procedure Execution Start for Alert Mail For WebCCF Error ****************************");

                BackgroundProcessOrderManager.SendAlertMailForWebCCFError(backgroundProcessUserId);

                logger.Info("*******************  Store Procedure Execution End for Alert Mail For WebCCF Error ****************************");


                //Save service logging data to DB
                if (_isServiceLoggingEnabled)
                {
                    jobEndTime = DateTime.Now;
                    ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                    serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                    serviceLoggingContract.JobName = JobName.SendAlertMailForWebCCFError.GetStringValue();
                    serviceLoggingContract.TenantID = AppConsts.NONE;
                    serviceLoggingContract.JobStartTime = jobStartTime;
                    serviceLoggingContract.JobEndTime = jobEndTime;
                    serviceLoggingContract.IsDeleted = false;
                    serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                    serviceLoggingContract.CreatedOn = DateTime.Now;
                    SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in SendAlertMailForWebCCFError method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}"
                              , ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }

        }

        #endregion

        #region UAT-4613
        public static void SendNotificationForOrderServiceStatusInProcessAgency(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("************************** Calling SendNotificationForOrderServiceStatusInProcessAgency:" + DateTime.Now.ToString() + " * ******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                Entity.AppConfiguration appConfigForStudentServFormInProcAgencyStatusDaysLimit = SecurityManager.GetAppConfiguration(AppConsts.BKG_SVC_FORM_STATUS_IN_PROCESS_AGENCY_NOTIFICATION_DAYS);
                Int32 serviceFormStatusLimit = appConfigForStudentServFormInProcAgencyStatusDaysLimit.IsNotNull() ? Convert.ToInt32(appConfigForStudentServFormInProcAgencyStatusDaysLimit.AC_Value) : 0;
                DateTime jobStartTime = DateTime.Now;
                DateTime jobEndTime;

                string fileName = string.Concat("Student_Service_Form_InProcessAgency_Status_", DateTime.Now.ToString("MMddyyyy"));

                logger.Info("******************* Start Calling Stored Procedure for Report Data - In Process Agency from Student service form status: " + DateTime.Now.ToString() + " *******************");
                //DataTable dt = BackgroundProcessOrderManager.GetDataForInProcessAgencyFromApplicantServiceFormStatus(serviceFormStatusLimit);
                logger.Info("******************* End Calling Stored Procedure for Report Data - In Process Agency from Student service form status: " + DateTime.Now.ToString() + " *******************");


                logger.Info("******************* Start Creating Excel - In Process Agency from Student service form status: " + DateTime.Now.ToString() + " *******************");
                string reportName= "BkgServiceFormInProcessAgencyStatus";
                byte[] binaryFile = BkgSvcFormInProcessAgencyStatusReport(serviceFormStatusLimit, reportName);
                //byte[] binaryFile = ExcelReader.GetInProcessAgencyfromApplicantServiceFormStatus(dt, fileName);
                logger.Info("******************* End Creating Excel - In Process Agency from Student service form status: " + DateTime.Now.ToString() + " *******************");

                logger.Info("******************* Start Saving Excel Report - In Process Agency from Student service form status: " + DateTime.Now.ToString() + " *******************");

                string savedFilePath = ReportManager.SaveSvcFormApplicantAgency(binaryFile, string.Concat(fileName, ".xlsx"));

                logger.Info("******************* End Saving Excel Report - In Process Agency from Student service form status: " + DateTime.Now.ToString() + " *******************");


                String attachedFiles = String.Empty;

                Int32? sysCommAttachmentID = null;


                Entity.AppConfiguration appConfigurationForStudentServiceForm = SecurityManager.GetAppConfiguration(AppConsts.IN_PROCESS_AGENCY_FROM_STUDENT_SERVICE_FORM_STATUS);
                String emailIds = String.Empty;
                List<String> emailAddressesToSendNotification = new List<String>();
                if (appConfigurationForStudentServiceForm.IsNotNull())
                {
                    emailIds = appConfigurationForStudentServiceForm.AC_Value;
                    emailAddressesToSendNotification = emailIds.Split(',').ToList<String>();
                }
                List<CommunicationTemplateContract> lstUser = new List<CommunicationTemplateContract>();
                foreach (var email in emailAddressesToSendNotification)
                {
                    lstUser.Add(new CommunicationTemplateContract() { RecieverEmailID = email, RecieverName = "Admin", CurrentUserId = backgroundProcessUserId, ReceiverOrganizationUserId = 1, IsToUser = true });
                }

                CommunicationMockUpData mockData = new CommunicationMockUpData();
                mockData.UserName = "Admin";
                mockData.EmailID = emailIds;
                mockData.ReceiverOrganizationUserID = AppConsts.NONE;

                //Send mail
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                Int32? systemCommunicationId = CommunicationManager.SendMailForInProcessAgencyFromStudentServiceFormStatus(CommunicationSubEvents.NOTIFICATION_FOR_SVC_FORM_STATUS_CHANGED_TO_IN_PROCESS_AGENCY_STATUS, dictMailData, mockData, lstUser);

                SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                sysCommAttachment.SCA_OriginalDocumentID = -1;
                sysCommAttachment.SCA_OriginalDocumentName = string.Concat(fileName, ".xlsx");

                sysCommAttachment.SCA_DocumentPath = savedFilePath;
                sysCommAttachment.SCA_DocumentSize = binaryFile.Length;
                sysCommAttachment.SCA_DocAttachmentTypeID = GetAttachmentDocumentType(DocumentAttachmentType.DAILY_REPORT.GetStringValue());
                sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                sysCommAttachment.SCA_IsDeleted = false;
                sysCommAttachment.SCA_CreatedBy = backgroundProcessUserId;
                sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                sysCommAttachment.SCA_SystemCommunicationID = Convert.ToInt32(systemCommunicationId);

                sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                logger.Debug("******************* Placed entry in email queue  for for In Process Agency from student service form status: " + DateTime.Now.ToString() + " Notification delivery id: *******************");
                ServiceContext.ReleaseDBContextItems();

                //Save service logging data to DB
                if (_isServiceLoggingEnabled)
                {
                    jobEndTime = DateTime.Now;
                    ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                    serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                    serviceLoggingContract.JobName = JobName.NotificationForChangeServiceFormStatusToInProcessAgency.GetStringValue();
                    serviceLoggingContract.TenantID = SecurityManager.DefaultTenantID;
                    serviceLoggingContract.JobStartTime = jobStartTime;
                    serviceLoggingContract.JobEndTime = jobEndTime;
                    serviceLoggingContract.IsDeleted = false;
                    serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                    serviceLoggingContract.CreatedOn = DateTime.Now;
                    SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                }
            }
            catch (Exception ex)
            {

                logger.Error("An Error has occured in SendNotificationForOrderServiceStatusInProcessAgency method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }
        #endregion
        private static Int16 GetAttachmentDocumentType(String docAttachmentTypeCode)
        {
            List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();

            return !docAttachmentType.IsNullOrEmpty()
                    ? Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID)
                    : Convert.ToInt16(AppConsts.NONE);
        }

        private static byte[] BkgSvcFormInProcessAgencyStatusReport(Int32 serviceFormStatusLimit, String ReportName)
        {
            try
            {
                ParameterValue[] parameter;
               
                parameter = new ParameterValue[1];

                parameter[0] = new ParameterValue();
                parameter[0].Name = "serviceFormStatusLimit";
                parameter[0].Value = serviceFormStatusLimit.ToString();
                byte[] reportContent = ReportManager.GetReportByteArrayFormat(ReportName, parameter);
                return reportContent;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
