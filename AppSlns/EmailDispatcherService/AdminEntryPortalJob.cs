using Business.RepoManagers;
using Business.ReportExecutionService;
using Entity;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.AdminEntryPortal;
using INTSOF.UI.Contract.Services;
using INTSOF.Utils;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailDispatcherService
{
    public class AdminEntryPortalJob
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;

        public static void SendMailDarftOrderNotificationtoAdmin(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailDarftOrderNotificationtoAdmin: " + DateTime.Now.ToString() + " *******************");
                Int32 days_Old = 0;
                Int32 chunkSize = 0;

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_TO_ADMIN_FOR_DRAFT_ORDERS.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                String subEventCode = CommunicationSubEvents.NOTIFICATION_TO_ADMIN_FOR_DRAFT_ORDERS.GetStringValue();

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;


                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendDarftOrderNotificationtoAdminDaysOlder") && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminDaysOlder"]))
                    Int32.TryParse(ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminDaysOlder"], out days_Old);


                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendDarftOrderNotificationtoAdminChunkSize")
                  && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminChunkSize"]))
                {
                    chunkSize = ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminChunkSize"].IsNotNull() ?
                                                       Convert.ToInt32(ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminChunkSize"])
                                                       : AppConsts.CHUNK_SIZE_FOR_SEND_DRAFT_ORDER_NOTIFICATION_TO_ADMIN;
                }
                else
                    chunkSize = AppConsts.CHUNK_SIZE_FOR_SEND_DRAFT_ORDER_NOTIFICATION_TO_ADMIN;

                ////if (C


                //if (ConfigurationManager.AppSettings.AllKeys.Contains("SendDarftOrderNotificationtoAdminChunkSize") && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminChunkSize"]))
                //    if (!Int32.TryParse(ConfigurationManager.AppSettings["SendDarftOrderNotificationtoAdminChunkSize"], out chunkSize))
                //        chunkSize = AppConsts.CHUNK_SIZE_FOR_SEND_DRAFT_ORDER_NOTIFICATION_TO_ADMIN;




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

                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);
                        // String entitySetName = "RequirementCategoriesBeforeGoingToBeRequiredData";
                        List<SendDraftOrderNotificationtoAdminContract> lstSendDraftOrderNotificationtoAdminContract = new List<SendDraftOrderNotificationtoAdminContract>();

                        lstSendDraftOrderNotificationtoAdminContract = AdminEntryPortalManager.GetDraftOrderForNotification(tenantId, subEventId, chunkSize, days_Old);

                        while (lstSendDraftOrderNotificationtoAdminContract != null && lstSendDraftOrderNotificationtoAdminContract.Count > 0)
                        {
                            logger.Info("******************* START while loop of SendMailDarftOrderNotificationtoAdmin method for tenant id: " + tenantId.ToString() + " *******************");
                            foreach (SendDraftOrderNotificationtoAdminContract draftOrder in lstSendDraftOrderNotificationtoAdminContract)
                            {
                                //logger.Trace("******************* Placing entry in Email Queue and Notification delivery for a chunk of Categories Before Going To Be Required:" + DateTime.Now.ToString() + " *******************");

                                //Create Dictionary
                                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                dictMailData.Add(EmailFieldConstants.STUDENT_FIRST, draftOrder.FirstName);
                                dictMailData.Add(EmailFieldConstants.STUDENT_LAST, draftOrder.LastName);
                                dictMailData.Add(EmailFieldConstants.Order_ID, draftOrder.OrderNumber);
                                dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, draftOrder.InstituteName);
                                dictMailData.Add(EmailFieldConstants.ORDER_DATE, draftOrder.CreatedOn.ToString("MM/dd/yyyy"));

                                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                mockData.UserName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
                                mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                                mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;


                                int? systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_TO_ADMIN_FOR_DRAFT_ORDERS
                                                                                                              , dictMailData, mockData, tenantId
                                                                                                              , draftOrder.SelectedNodeID.HasValue ? draftOrder.SelectedNodeID.Value : 0
                                                                                                              , null);




                                //Save Notification Delivery 
                                Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                notificationDelivery.ND_OrganizationUserID = draftOrder.OrgUserId;
                                notificationDelivery.ND_SubEventTypeID = subEventId;
                                notificationDelivery.ND_EntityId = draftOrder.MasterOrderID;
                                notificationDelivery.ND_EntityName = "Draft Order";
                                notificationDelivery.ND_IsDeleted = false;
                                notificationDelivery.ND_CreatedOn = DateTime.Now;
                                notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                                logger.Trace("******************* Placed entry in Email Queue and Notification delivery for a chunk of Darft Orders Notification: " + DateTime.Now.ToString() + " *******************");
                            }
                            logger.Trace("******************* Processed a chunk of Darft Orders Notification: " + DateTime.Now.ToString() + " *******************");
                            lstSendDraftOrderNotificationtoAdminContract = AdminEntryPortalManager.GetDraftOrderForNotification(tenantId, subEventId, chunkSize, days_Old);

                        }
                        logger.Info("******************* END while loop of SendMailDarftOrderNotificationtoAdmin method for tenant id: " + tenantId.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.NotificationForDraftOrderstoAdmin.GetStringValue();
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
                logger.Error("An Error has occured in SendMailDarftOrderNotificationtoAdmin method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        public static void SendMailPendingInvitaiontoApplicant(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailPendingInvitaiontoApplicant: " + DateTime.Now.ToString() + " *******************");
                Int32 chunkSize = 0;
                Int32 days_Old = 0;
                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_TO_APPLICANT_OF_INVITATION_PENDING.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                String subEventCode = CommunicationSubEvents.NOTIFICATION_TO_APPLICANT_OF_INVITATION_PENDING.GetStringValue();

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

                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendInvitationPendingOrderNotificationtoApplicantDaysOlder") && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantDaysOlder"]))
                    Int32.TryParse(ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantDaysOlder"], out days_Old);


                if (ConfigurationManager.AppSettings.AllKeys.Contains("SendInvitationPendingOrderNotificationtoApplicantChunkSize")
                   && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantChunkSize"]))
                {
                    chunkSize = ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantChunkSize"].IsNotNull() ?
                                                       Convert.ToInt32(ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantChunkSize"])
                                                       : AppConsts.CHUNK_SIZE_FOR_SEND_INVITATION_PENDING_ORDER_NOTIFICATION_TO_APPLICANT;
                }
                else
                    chunkSize = AppConsts.CHUNK_SIZE_FOR_SEND_INVITATION_PENDING_ORDER_NOTIFICATION_TO_APPLICANT;

                //if (ConfigurationManager.AppSettings.AllKeys.Contains("SendInvitationPendingOrderNotificationtoApplicantChunkSize") && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantChunkSize"]))
                //    if (!Int32.TryParse(ConfigurationManager.AppSettings["SendInvitationPendingOrderNotificationtoApplicantChunkSize"], out chunkSize))
                //        chunkSize = AppConsts.CHUNK_SIZE_FOR_SEND_INVITATION_PENDING_ORDER_NOTIFICATION_TO_APPLICANT;


                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;

                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);
                        // String entitySetName = "RequirementCategoriesBeforeGoingToBeRequiredData";
                        List<SendInvitationPendingOrderNotificationtoApplicantContract> lstSendInvitationPendingOrderNotifitoAppContract = new List<SendInvitationPendingOrderNotificationtoApplicantContract>();

                        lstSendInvitationPendingOrderNotifitoAppContract = AdminEntryPortalManager.GetInvitationPendingStatusOrderForApplicant(tenantId, subEventId, chunkSize, days_Old);

                        while (lstSendInvitationPendingOrderNotifitoAppContract != null && lstSendInvitationPendingOrderNotifitoAppContract.Count > 0)
                        {
                            logger.Info("******************* START while loop of SendMailPendingInvitaiontoApplicant method for tenant id: " + tenantId.ToString() + " *******************");
                            foreach (SendInvitationPendingOrderNotificationtoApplicantContract InvitationPendingOrder in lstSendInvitationPendingOrderNotifitoAppContract)
                            {
                                String formattedURL = String.Format(applicationUrl + "?aeait=" + InvitationPendingOrder.AdminEntryInvitationToken);

                                //logger.Trace("******************* Placing entry in Email Queue and Notification delivery for a chunk of Categories Before Going To Be Required:" + DateTime.Now.ToString() + " *******************");

                                //Create Dictionary
                                CommunicationMockUpData mockData = new CommunicationMockUpData();

                                Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                                dictMailData.Add(EmailFieldConstants.STUDENT_FIRST, InvitationPendingOrder.FirstName);
                                dictMailData.Add(EmailFieldConstants.STUDENT_LAST, InvitationPendingOrder.LastName);
                                dictMailData.Add(EmailFieldConstants.SEND_INVITE_CREATED_DATE, InvitationPendingOrder.CreatedDate.ToString("dd/MM/yyyy"));
                                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, InvitationPendingOrder.hierarchyNodeName);
                                //dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, formattedURL);
                                dictMailData.Add(EmailFieldConstants.Applicant_Invite_URL, formattedURL);


                                mockData.UserName = string.Concat(InvitationPendingOrder.FirstName, " ", InvitationPendingOrder.LastName);
                                mockData.EmailID = InvitationPendingOrder.Email;
                                mockData.ReceiverOrganizationUserID = InvitationPendingOrder.OrgUserId;


                                int? systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_TO_APPLICANT_OF_INVITATION_PENDING
                                                                                                              , dictMailData, mockData, tenantId
                                                                                                              , InvitationPendingOrder.SelectedNodeID.HasValue ? InvitationPendingOrder.SelectedNodeID.Value : 0
                                                                                                              , null);


                                //Save Notification Delivery 
                                Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                notificationDelivery.ND_OrganizationUserID = InvitationPendingOrder.OrgUserId;
                                notificationDelivery.ND_SubEventTypeID = subEventId;
                                notificationDelivery.ND_EntityId = InvitationPendingOrder.BkgOrderID;
                                notificationDelivery.ND_EntityName = "Invitation Sent Order";
                                notificationDelivery.ND_IsDeleted = false;
                                notificationDelivery.ND_CreatedOn = DateTime.Now;
                                notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                                logger.Trace("******************* Placed entry in Email Queue and Notification delivery for a chunk of Pending Invitaion to Applicant Notification: " + DateTime.Now.ToString() + " *******************");
                            }
                            logger.Trace("******************* Processed a chunk of Pending Invitaion to Applicant Notification: " + DateTime.Now.ToString() + " *******************");
                            lstSendInvitationPendingOrderNotifitoAppContract = AdminEntryPortalManager.GetInvitationPendingStatusOrderForApplicant(tenantId, subEventId, chunkSize, days_Old);

                        }
                        logger.Info("******************* END while loop of SendMailPendingInvitaiontoApplicant method for tenant id: " + tenantId.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.NotificationForInvitationPendingOrderstoApplicant.GetStringValue();
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
                logger.Error("An Error has occured in SendMailInvitaionPendingtoApplicent method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        public static void DeleteDraftOrder(Int32? TenantId = null)
        {
            try
            {
                logger.Info("******************* Calling DeleteDraftOrder: " + DateTime.Now.ToString() + " *******************");
                Int32 days_Old = 0;

                if (ConfigurationManager.AppSettings.AllKeys.Contains("DeleteDraftOrderStatusDaysOlder") && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["DeleteDraftOrderStatusDaysOlder"]))
                    Int32.TryParse(ConfigurationManager.AppSettings["DeleteDraftOrderStatusDaysOlder"], out days_Old);

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

                if (clientDbConfs != null && clientDbConfs.Count > 0 && TenantId != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == TenantId).ToList();
                }
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    DateTime jobStartTime = DateTime.Now;
                    DateTime jobEndTime;
                    Int32 tenantId = AppConsts.NONE;

                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        //DateTime jobStartTime = DateTime.Now;
                        //DateTime jobEndTime;
                        tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;
                        AdminEntryPortalManager.DeleteDraftOrder(days_Old, tenantId, backgroundProcessUserId);
                    }

                    logger.Info("******************* END while loop of DeleteDraftOrder method for tenant id: " + tenantId.ToString() + " *******************");

                    //Save service logging data to DB
                    if (_isServiceLoggingEnabled)
                    {
                        jobEndTime = DateTime.Now;
                        ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                        serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                        serviceLoggingContract.JobName = JobName.DeleteDraftOrder.GetStringValue();
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
                logger.Error("An Error has occured in SendMailAutoArchivedOrderNotificationtoAdmin method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }


        }

        public static void ChangeOrdersStatusCompletedToArchived(Int32? TenantId = null)
        {
            try
            {

                logger.Info("******************* Started ChangeOrdersStatusCompletedToArchived method *******************");

                Int32 defaultAutoArchivedDays = 0;
                Int32 chunkSize = 0;

                if (ConfigurationManager.AppSettings.AllKeys.Contains("ChangeOrderStatusCompleteToArchivedDefaultDaysOlder") && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedDefaultDaysOlder"]))
                    Int32.TryParse(ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedDefaultDaysOlder"], out defaultAutoArchivedDays);


                if (ConfigurationManager.AppSettings.AllKeys.Contains("ChangeOrderStatusCompleteToArchivedChunkSize")
                    && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedChunkSize"]))
                {
                    chunkSize = ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedChunkSize"].IsNotNull() ?
                                                       Convert.ToInt32(ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedChunkSize"])
                                                       : AppConsts.CHUNK_SIZE_FOR_SEND_CHANGE_ORDER_STATUS_COMPLETED_TO_ARCHIVED_NOTIFICATION;
                }
                else
                    chunkSize = AppConsts.CHUNK_SIZE_FOR_SEND_CHANGE_ORDER_STATUS_COMPLETED_TO_ARCHIVED_NOTIFICATION;


                //if (!Int32.TryParse(ConfigurationManager.AppSettings["ChangeOrderStatusCompleteToArchivedChunkSize"], out chunkSize))
                //    chunkSize = AppConsts.CHUNK_SIZE_FOR_SEND_CHANGE_ORDER_STATUS_COMPLETED_TO_ARCHIVED_NOTIFICATION;

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                    item =>
                    {
                        ClientDBConfiguration config = new ClientDBConfiguration();
                        config.CDB_TenantID = item.CDB_TenantID;
                        config.CDB_ConnectionString = item.CDB_ConnectionString;
                        config.Tenant = new Tenant();
                        config.Tenant.TenantName = item.Tenant.TenantName;
                        return config;
                    }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && TenantId != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == TenantId).ToList();
                }
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    DateTime jobStartTime = DateTime.Now;
                    DateTime jobEndTime;
                    Int32 tenantId = AppConsts.NONE;

                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        tenantId = clntDbConf.CDB_TenantID;

                        logger.Info("******************* Started while loop of ChangeOrdersStatusCompletedToArchived method for tenant id: " + tenantId.ToString() + " *******************");

                        List<AutoArchivedTimeLineDays> lstAutoArchivedTimeLineDays = AdminEntryPortalManager.GetAutoArchiveTimeLineDays(chunkSize, tenantId);
                        foreach (var autoArchivedTimeLineDays in lstAutoArchivedTimeLineDays)
                        {
                            Int32 autoArchiveDays = !autoArchivedTimeLineDays.Days.IsNullOrEmpty() && autoArchivedTimeLineDays.Days > AppConsts.NONE ? autoArchivedTimeLineDays.Days : defaultAutoArchivedDays;

                            bool res = AdminEntryPortalManager.ChangeBkgOrdersStatusCompletedToArchived(autoArchivedTimeLineDays.BkgAdminEntryOrderId, autoArchiveDays, tenantId, backgroundProcessUserId);
                            if (res == true)
                            {
                                SendMailAutoArchivedOrderNotificationtoAdmin(autoArchivedTimeLineDays, clntDbConf, autoArchiveDays);
                            }
                        }

                    }
                    logger.Info("******************* END while loop of ChangeOrdersStatusCompletedToArchived method for tenant id: " + tenantId.ToString() + " *******************");
                    //Save service logging data to DB
                    if (_isServiceLoggingEnabled)
                    {
                        jobEndTime = DateTime.Now;
                        ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                        serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                        serviceLoggingContract.JobName = JobName.NotificationForChangeOrdersStatusCompletedToArchived.GetStringValue();
                        serviceLoggingContract.TenantID = tenantId;
                        serviceLoggingContract.JobStartTime = jobStartTime;
                        serviceLoggingContract.JobEndTime = jobEndTime;
                        serviceLoggingContract.IsDeleted = false;
                        serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                        serviceLoggingContract.CreatedOn = DateTime.Now;
                        SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                    }
                    logger.Info("******************* Ended ChangeOrdersStatusCompletedToArchived method *******************");
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in ChangeOrdersStatusCompletedToArchived method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }

        }

        public static void SendMailAutoArchivedOrderNotificationtoAdmin(AutoArchivedTimeLineDays autoArchivedTimeLineDays, ClientDBConfiguration clntDbConf, Int32 days_Old = 0, Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Started Calling SendMailAutoArchivedOrderNotificationtoAdmin: " + DateTime.Now.ToString() + " *******************");

                if (!autoArchivedTimeLineDays.IsNullOrEmpty())
                {
                    logger.Info("******************* Starting processing SendMailAutoArchivedOrderNotificationtoAdmin: " + DateTime.Now.ToString() + "for BkgOrderId" + autoArchivedTimeLineDays.BkgOrderId.ToString() + " *******************");

                    lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_TO_CLIENT_ADMIN_FOR_AUTO_ARCHIVED.GetStringValue());
                    Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                    String subEventCode = CommunicationSubEvents.NOTIFICATION_TO_CLIENT_ADMIN_FOR_AUTO_ARCHIVED.GetStringValue();

                    Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                    Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                    Int32 tenantId = clntDbConf.CDB_TenantID;
                    String tenantName = clntDbConf.Tenant.TenantName;

                    String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);


                    logger.Info("******************* START while loop of SendMailDarftOrderNotificationtoAdmin method for tenant id: " + tenantId.ToString() + " *******************");

                    //Create Dictionary
                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                    dictMailData.Add(EmailFieldConstants.STUDENT_FIRST, autoArchivedTimeLineDays.FirstName);
                    dictMailData.Add(EmailFieldConstants.STUDENT_LAST, autoArchivedTimeLineDays.LastName);
                    dictMailData.Add(EmailFieldConstants.Order_ID, autoArchivedTimeLineDays.OrderNumber);
                    dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, autoArchivedTimeLineDays.InstitutionHierarchyName);
                    dictMailData.Add(EmailFieldConstants.ORDER_COMPLETED_DATE, autoArchivedTimeLineDays.OrderCompletedDate);

                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                    mockData.UserName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
                    mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                    mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;


                    int? systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_TO_CLIENT_ADMIN_FOR_AUTO_ARCHIVED
                                                                                                  , dictMailData, mockData, tenantId
                                                                                                  , autoArchivedTimeLineDays.SelectedNodeID.HasValue ? autoArchivedTimeLineDays.SelectedNodeID.Value : 0
                                                                                                  , null);

                    //Save Notification Delivery 
                    Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                    notificationDelivery.ND_OrganizationUserID = autoArchivedTimeLineDays.OrgUserId;
                    notificationDelivery.ND_SubEventTypeID = subEventId;
                    notificationDelivery.ND_EntityId = autoArchivedTimeLineDays.BkgOrderId;
                    notificationDelivery.ND_EntityName = "Order Status Auto Archived";
                    notificationDelivery.ND_IsDeleted = false;
                    notificationDelivery.ND_CreatedOn = DateTime.Now;
                    notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                    ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                    logger.Trace("******************* Placed entry in Email Queue and Notification delivery for a chunk of AutoArchived Order Notification to Admin: " + DateTime.Now.ToString() + " *******************");


                    logger.Info("******************* Ended processing SendMailAutoArchivedOrderNotificationtoAdmin: " + DateTime.Now.ToString() + "for BkgOrderId" + autoArchivedTimeLineDays.BkgOrderId.ToString() + " *******************");

                }
                logger.Info("******************* Ended Calling SendMailAutoArchivedOrderNotificationtoAdmin: " + DateTime.Now.ToString() + " *******************");

            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in SendMailAutoArchivedOrderNotificationtoAdmin method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }


        public static void SendMailBkgSvcGroupCompletedOrderNotificationtoAdmin(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailBkgSvcGroupCompletedOrderNotificationtoAdmin: " + DateTime.Now.ToString() + " *******************");

                Int32 chunkSize = 0;

                if (ConfigurationManager.AppSettings.AllKeys.Contains("BKgCompletedOrderToClientAdminChunkSize")
                  && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["BKgCompletedOrderToClientAdminChunkSize"]))
                {
                    chunkSize = ConfigurationManager.AppSettings["BKgCompletedOrderToClientAdminChunkSize"].IsNotNull() ?
                                                       Convert.ToInt32(ConfigurationManager.AppSettings["BKgCompletedOrderToClientAdminChunkSize"])
                                                       : AppConsts.CHUNK_SIZE_FOR_SEND_SRV_GROUP_COMPLETED_ORDER_NOTIFICATION_TO_ADMIN;
                }
                else
                    chunkSize = AppConsts.CHUNK_SIZE_FOR_SEND_SRV_GROUP_COMPLETED_ORDER_NOTIFICATION_TO_ADMIN;



                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_TO_CLIENT_ADMIN_OF_ORDER_COMPLETED.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;

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

                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                        List<CompleteStatusOrders> lstSendSrvGroupCompletedOrderNotificationtoAdminContract = new List<CompleteStatusOrders>();
                        lstSendSrvGroupCompletedOrderNotificationtoAdminContract = AdminEntryPortalManager.GetRecentCompletedOrders(chunkSize, "Order Status Completed", tenantId, subEventId);

                        if (lstSendSrvGroupCompletedOrderNotificationtoAdminContract != null && lstSendSrvGroupCompletedOrderNotificationtoAdminContract.Count > 0)
                        {
                            logger.Info("******************* START while loop of SendMailSvcGroupCompletedOrderNotificationtoAdmin method for tenant id: " + tenantId.ToString() + " *******************");
                            foreach (CompleteStatusOrders completeStatusOrders in lstSendSrvGroupCompletedOrderNotificationtoAdminContract)
                            {
                                logger.Trace("******************* Placing entry in Email Queue and Notification delivery for a chunk of Categories Before Going To Be Required:" + DateTime.Now.ToString() + " *******************");

                                //Create Dictionary
                                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                dictMailData.Add(EmailFieldConstants.STUDENT_FIRST, completeStatusOrders.FirstName);
                                dictMailData.Add(EmailFieldConstants.STUDENT_LAST, completeStatusOrders.LastName);
                                dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, completeStatusOrders.InstitutionHierarchyName);

                                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                mockData.UserName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
                                mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                                mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;


                                int? systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_TO_CLIENT_ADMIN_OF_ORDER_COMPLETED
                                                                                                              , dictMailData, mockData, tenantId
                                                                                                              , completeStatusOrders.SelectedNodeId.HasValue ? completeStatusOrders.SelectedNodeId.Value : 0
                                                                                                              , null);

                                //Save Notification Delivery 
                                Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                notificationDelivery.ND_OrganizationUserID = completeStatusOrders.OrganizationUserID;
                                notificationDelivery.ND_SubEventTypeID = subEventId;
                                notificationDelivery.ND_EntityId = completeStatusOrders.OrderID;
                                notificationDelivery.ND_EntityName = "Order Status Completed";
                                notificationDelivery.ND_IsDeleted = false;
                                notificationDelivery.ND_CreatedOn = DateTime.Now;
                                notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                                logger.Trace("******************* Placed entry in Email Queue and Notification delivery for a chunk of Bkg Svc Group Completed Order Notification: " + DateTime.Now.ToString() + " *******************");
                            }
                            logger.Trace("******************* Processed a chunk of Completed Order Notification: " + DateTime.Now.ToString() + " *******************");


                        }
                        logger.Info("******************* END while loop of SendMailBkgSvcGroupCompletedOrderNotificationtoAdmin method for tenant id: " + tenantId.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.NotificationtoAdminForBkgSvcGroupCompletedOrder.GetStringValue();
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
                logger.Error("An Error has occured in SendMailBkgSvcGroupCompletedOrderNotificationtoAdmin method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        public static void SendMailCompletedOrderWithAttachmentNotificationtoAdmin(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailCompletedOrderWithAttachmentNotificationtoAdmin: " + DateTime.Now.ToString() + " *******************");
                Int32 chunkSize = 0;

                if (ConfigurationManager.AppSettings.AllKeys.Contains("BKgCompletedOrderWithAttachedPDFToClientAdminChunkSize")
                  && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["BKgCompletedOrderWithAttachedPDFToClientAdminChunkSize"]))
                {
                    chunkSize = ConfigurationManager.AppSettings["BKgCompletedOrderWithAttachedPDFToClientAdminChunkSize"].IsNotNull() ?
                                                       Convert.ToInt32(ConfigurationManager.AppSettings["BKgCompletedOrderWithAttachedPDFToClientAdminChunkSize"])
                                                       : AppConsts.CHUNK_SIZE_FOR_SEND_COMPLETED_ORDER_WITH_ATTACHMENT_NOTIFICATION_TO_ADMIN;
                }
                else
                    chunkSize = AppConsts.CHUNK_SIZE_FOR_SEND_COMPLETED_ORDER_WITH_ATTACHMENT_NOTIFICATION_TO_ADMIN;


                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_TO_CLIENT_ADMIN_OF_BKG_ORDER_COMPLETED_WITH_ATTACHMENT.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                String subEventCode = CommunicationSubEvents.NOTIFICATION_TO_CLIENT_ADMIN_OF_BKG_ORDER_COMPLETED_WITH_ATTACHMENT.GetStringValue();

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

                        List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
                        List<Entity.ClientEntity.lkpOrderNotificationType> orderNotificationType = BackgroundProcessOrderManager.GetOrderNotificationType(tenantId);

                        String docAttachmentTypeCode = DocumentAttachmentType.ORDER_COMPLETION_DOCUMENT.GetStringValue();
                        Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                            Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);


                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                        List<CompleteStatusOrders> lstOrder = new List<CompleteStatusOrders>();
                        lstOrder = AdminEntryPortalManager.GetRecentCompletedOrders(chunkSize, "Order Completed With Attachment", tenantId, subEventId);

                        if (lstOrder != null && lstOrder.Count > 0)
                        {
                            logger.Info("******************* START while loop of SendMailCompletedOrderWithAttachmentNotificationtoAdmin method for tenant id: " + tenantId.ToString() + " *******************");
                            foreach (CompleteStatusOrders order in lstOrder)
                            {
                                logger.Trace("******************* Placing entry in Email Queue and Notification delivery for a chunk of Completed Order With Attachment Notification:" + DateTime.Now.ToString() + " *******************");

                                //Create Dictionary
                                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                dictMailData.Add(EmailFieldConstants.STUDENT_FIRST, order.FirstName);
                                dictMailData.Add(EmailFieldConstants.STUDENT_LAST, order.LastName);
                                dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, order.InstitutionHierarchyName);

                                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                mockData.UserName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
                                mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                                mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                                var PkgPDFAttachementStatus = BackgroundProcessOrderManager.GetBGPkgPDFAttachementStatus(tenantId, order.SelectedNodeId.HasValue ? order.SelectedNodeId.Value : 0);

                                //UAT-2855
                                if (order.IsAdminOrder)
                                {
                                    order.IsEmployment = false;
                                    PkgPDFAttachementStatus = PDFInclusionOptions.Not_Specified.GetStringValue();
                                }

                                SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                                if (!order.IsEmployment && !PkgPDFAttachementStatus.Equals(PDFInclusionOptions.Excluded.GetStringValue()))
                                {
                                    //Create Attachment
                                    ParameterValue[] parameters = new ParameterValue[3];
                                    parameters[0] = new ParameterValue();
                                    parameters[0].Name = "OrderID";
                                    parameters[0].Value = order.OrderID.ToString(); //condition.BkgOrderID.ToString();
                                    parameters[1] = new ParameterValue();
                                    parameters[1].Name = "TenantID";
                                    parameters[1].Value = tenantId.ToString();
                                    parameters[2] = new ParameterValue();
                                    parameters[2].Name = "UserID";
                                    parameters[2].Value = AppConsts.STR_ONE;

                                    String reportName = "OrderCompletion";// +order.OrderNumber;
                                    String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                                    String fileName = "OCR_" + tenantId.ToString() + "_" + order.OrderNumber.ToString() + "_" + date + ".pdf";

                                    byte[] reportContent = ReportManager.GetReportByteArray(reportName, parameters);

                                    String retFilepath = ReportManager.ConvertByteArrayToReportFile(reportContent, fileName);


                                    sysCommAttachment.SCA_OriginalDocumentID = -1;
                                    sysCommAttachment.SCA_OriginalDocumentName = "OrderCompletedReport_" + order.OrderID.ToString() + ".pdf";
                                    sysCommAttachment.SCA_DocumentPath = retFilepath;
                                    sysCommAttachment.SCA_DocumentSize = reportContent.Length;
                                    sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                                    sysCommAttachment.SCA_TenantID = tenantId;// SecurityManager.DefaultTenantID;
                                    sysCommAttachment.SCA_IsDeleted = false;
                                    sysCommAttachment.SCA_CreatedBy = backgroundProcessUserId;
                                    sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                                    sysCommAttachment.SCA_ModifiedBy = null;
                                    sysCommAttachment.SCA_ModifiedOn = null;
                                }

                                #region Code to generate report url place holder value

                                Dictionary<String, String> reportUrlParameters = new Dictionary<String, String>
                                                         {
                                                            { "OrderID",  order.OrderID.ToString()},
                                                             { "ReportType",  BackgroundReportType.ORDER_COMPLETION.GetStringValue()},
                                                             { "TenantId",  tenantId.ToString()},
                                                             {"HierarchyNodeID",Convert.ToString(order.SelectedNodeId)},
                                                             {"OrganizationUserID",Convert.ToString(order.OrganizationUserID)},
                                                             {"IsReportSentToStudent",Convert.ToString(true)}
                                                         };
                                StringBuilder reportUrl = new StringBuilder();
                                reportUrl.Append(applicationUrl.Trim() + "?args=");
                                reportUrl.Append(reportUrlParameters.ToEncryptedQueryString());

                                #endregion

                                dictMailData.Add(EmailFieldConstants.REPORT_URL, reportUrl.ToString());

                                int? systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_TO_CLIENT_ADMIN_OF_BKG_ORDER_COMPLETED_WITH_ATTACHMENT
                                                                                                              , dictMailData, mockData, tenantId
                                                                                                              , order.SelectedNodeId.HasValue ? order.SelectedNodeId.Value : 0
                                                                                                              , null);


                                if (!sysCommAttachment.IsNullOrEmpty() && !systemCommunicationID.IsNullOrEmpty() && systemCommunicationID.Value > AppConsts.NONE)
                                {
                                    sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                                    CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                                }

                                //Save Notification Delivery 
                                Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                notificationDelivery.ND_OrganizationUserID = order.OrganizationUserID;
                                notificationDelivery.ND_SubEventTypeID = subEventId;
                                notificationDelivery.ND_EntityId = order.OrderID;
                                notificationDelivery.ND_EntityName = "Order Completed With Attachment";
                                notificationDelivery.ND_IsDeleted = false;
                                notificationDelivery.ND_CreatedOn = DateTime.Now;
                                notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                                logger.Trace("******************* Placed entry in Email Queue and Notification delivery for a chunk of Completed Order With Attachment Notification: " + DateTime.Now.ToString() + " *******************");
                            }
                            logger.Trace("******************* Processed a chunk of Completed Order With Attachment Notification: " + DateTime.Now.ToString() + " *******************");


                        }
                        logger.Info("******************* END while loop of SendMailCompletedOrderWithAttachmentNotificationtoAdmin method for tenant id: " + tenantId.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.NotificationtoAdminForBkgSvcGroupCompletedOrderWithAttachment.GetStringValue();
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
                logger.Error("An Error has occured in SendMailCompletedOrderWithAttachmentNotificationtoAdmin method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }
    }
}
