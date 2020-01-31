using Business.RepoManagers;
using Entity;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.Services;
using INTSOF.UI.Contract.Templates;
using INTSOF.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

namespace EmailDispatcherService
{
    public class NotificationService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Int32 _totalThreadPerSearchTypeAllowed = 0;
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;

        public static void SendMailBeforeExpiry(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailBeforeExpiry: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.REMINDER_SUBSCRIPTION_EXPIRE.GetStringValue());
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

                Int32 emailChunkSize = AppConsts.CHUNK_SIZE_FOR_MAIL_BEFORE_SUBSCRIPTION_EXPIRY;
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;
                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                        //Int32 remBfrExp = ComplianceDataManager.GetSubscriptionNotificationBeforeExpiryDays(tenantId, Setting.Reminder_Before_Expiry.GetStringValue());
                       
                        //Int32 remExpFrq = ComplianceDataManager.GetSubscriptionNotificationFrequencyDays(tenantId, Setting.Reminder_Expiry_Frequency.GetStringValue());
                        

                        //DateTime dtBeforeExp = DateTime.Now.Date.AddDays(remBfrExp).Date;
                        DateTime today = DateTime.Now.Date;
                        String entitySetName = "PackageSubscription";
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            List<ReminderContract> expiryPackageSubscriptions = ComplianceDataManager.GetExpiryPackageSubscriptions(tenantId, entitySetName, emailChunkSize).ToList();

                            if (expiryPackageSubscriptions != null && expiryPackageSubscriptions.Count > 0)
                            {
                                foreach (ReminderContract remCont in expiryPackageSubscriptions)
                                {
                                    logger.Debug("******************* Placing entry in Email Queue and Notification delivery an Expiring package:" + DateTime.Now.ToString() + " *******************");

                                    //Create Dictionary
                                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(remCont.FirstName, " ", remCont.LastName));
                                    dictMailData.Add(EmailFieldConstants.EXPIRY_DATE, remCont.ExpiryDate.ToString("MM/dd/yyyy"));
                                    dictMailData.Add(EmailFieldConstants.DAYS_LEFT_TO_EXPIRE, (remCont.ExpiryDate - DateTime.Now.Date).Days);
                                    dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                    dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, remCont.NodeHierarchy);
                                    dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, remCont.PackageName);
                                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);

                                    CommunicationMockUpData mockData = new CommunicationMockUpData();
                                    mockData.UserName = string.Concat(remCont.FirstName, " ", remCont.LastName);
                                    mockData.EmailID = remCont.PrimaryEmailAddress;
                                    mockData.ReceiverOrganizationUserID = remCont.OrganizationUserID;

                                    //Send mail
                                    CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.REMINDER_SUBSCRIPTION_EXPIRE, dictMailData, mockData, tenantId, remCont.HierarchyNodeID);

                                    //Send Message
                                    CommunicationManager.SaveMessageContent(CommunicationSubEvents.REMINDER_SUBSCRIPTION_EXPIRE, dictMailData, remCont.OrganizationUserID, tenantId);

                                    //Save Notification Delivery
                                    Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                    notificationDelivery.ND_OrganizationUserID = remCont.OrganizationUserID;
                                    notificationDelivery.ND_SubEventTypeID = subEventId;
                                    notificationDelivery.ND_EntityId = remCont.PackageSubscriptionID;
                                    notificationDelivery.ND_EntityName = entitySetName;
                                    notificationDelivery.ND_IsDeleted = false;
                                    notificationDelivery.ND_CreatedOn = DateTime.Now;
                                    notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                    ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                                    logger.Debug("******************* Placed entry in Email Queue and Notification delivery for an expiring package: " + DateTime.Now.ToString() + " Notification delivery id: " + notificationDelivery.ND_ID
                                        + " *******************");
                                }
                                logger.Trace("Processed chunk for expiring package. " + DateTime.Now.ToString());

                                ServiceContext.ReleaseDBContextItems();
                                executeLoop = true;
                            }
                            else
                            {
                                executeLoop = false;
                            }
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendMailBeforeExpiry.GetStringValue();
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
                logger.Error("An Error has occured in SendMailBeforeExpiry method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        public static void SendMailAfterExpiry(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailAfterExpiry: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_EXPIRED_SUBSCRIPTIONS.GetStringValue());
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

                Int32 emailChunkSize = AppConsts.CHUNK_SIZE_FOR_MAIL_BEFORE_SUBSCRIPTION_EXPIRY;
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;
                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                        Int32 remAftExp = 30;
                        //ComplianceDataManager.GetSubscriptionNotificationAfterExpiryDays(tenantId, Setting.Reminder_After_Expiry.GetStringValue());
                        Int32 remExpFrq = 15;
                        //ComplianceDataManager.GetSubscriptionNotificationFrequencyDays(tenantId, Setting.Reminder_Expiry_Frequency.GetStringValue());

                        //DateTime today = DateTime.Now.Date;
                        String entitySetName = "PendingPackageSubscription";
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            List<ReminderContract> expiredPackageSubscriptions = ComplianceDataManager.GetExpiredPackageSubscriptions(tenantId, remAftExp, remExpFrq, entitySetName,  emailChunkSize).ToList();
                            if (expiredPackageSubscriptions != null && expiredPackageSubscriptions.Count > 0)
                            {
                                foreach (ReminderContract remCont in expiredPackageSubscriptions)
                                {
                                    logger.Debug("******************* Placing entry in Email Queue and Notification delivery for an Expired package: " + DateTime.Now.ToString() + " *******************");

                                    //Create Dictionary
                                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(remCont.FirstName, " ", remCont.LastName));
                                    dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, remCont.PackageName);
                                    dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, remCont.NodeHierarchy);
                                    dictMailData.Add(EmailFieldConstants.EXPIRY_DATE, remCont.ExpiryDate.ToString("MM/dd/yyyy"));
                                    dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                    dictMailData.Add(EmailFieldConstants.ORDER_NO, remCont.OrderNumber.ToString());

                                    CommunicationMockUpData mockData = new CommunicationMockUpData();
                                    mockData.UserName = string.Concat(remCont.FirstName, " ", remCont.LastName);
                                    mockData.EmailID = remCont.PrimaryEmailAddress;
                                    mockData.ReceiverOrganizationUserID = remCont.OrganizationUserID;

                                    //Send mail
                                    CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_EXPIRED_SUBSCRIPTIONS, dictMailData, mockData, tenantId, remCont.HierarchyNodeID);

                                    //Send Message
                                    CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_EXPIRED_SUBSCRIPTIONS, dictMailData, remCont.OrganizationUserID, tenantId);

                                    //Save Notification Delivery
                                    Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                    notificationDelivery.ND_OrganizationUserID = remCont.OrganizationUserID;
                                    notificationDelivery.ND_SubEventTypeID = subEventId;
                                    notificationDelivery.ND_EntityId = remCont.PackageSubscriptionID;
                                    notificationDelivery.ND_EntityName = entitySetName;
                                    notificationDelivery.ND_IsDeleted = false;
                                    notificationDelivery.ND_CreatedOn = DateTime.Now;
                                    notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                    ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                                    logger.Debug("******************* Placed entry in Email Queue and Notification delivery for for an expired package: " + DateTime.Now.ToString() + " Notification delivery id: " + notificationDelivery.ND_ID
                                        + " *******************");
                                }
                                logger.Trace("Processed chunk for expired package. " + DateTime.Now.ToString());

                                ServiceContext.ReleaseDBContextItems();
                                executeLoop = true;
                            }
                            else
                            {
                                executeLoop = false;
                            }
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendMailAfterExpiry.GetStringValue();
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
                logger.Error("An Error has occured in SendMailAfterExpiry method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        public static void SendMailForPendingPackage(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailForPendingPackage: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.REMINDER_SUBSCRIPTION_PENDING.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                Entity.AppConfiguration appConfig = SecurityManager.GetAppConfiguration(ConfigurationManager.AppSettings["LocationServiceTenantIds"]);
                List<Int32> locationServiceTenantIds = appConfig.IsNotNull() ? appConfig.AC_Value.Split(',').Select(Int32.Parse).ToList() : new List<int>();

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Where(x => locationServiceTenantIds.Count == AppConsts.NONE || !locationServiceTenantIds.Contains(x.CDB_TenantID)).Select(
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

                Int32 emailChunkSize = AppConsts.CHUNK_SIZE_FOR_MAIL_PENDING_PACKAGE;

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {

                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        //UAT-2569
                        // Modification check for the account without purchase notification.
                        Boolean isPurchaseNotificationEnabled = true;
                        var clientSetting = ComplianceDataManager.GetClientSetting(clntDbConf.CDB_TenantID, Setting.ACCOUNT_WITHOUT_PURCHASE_NOTIFICATION.GetStringValue());
                        if (!clientSetting.IsNullOrEmpty())
                        {
                            isPurchaseNotificationEnabled = clientSetting.CS_SettingValue == "1" ? true : false;
                        }

                        if (isPurchaseNotificationEnabled)
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;
                            Int32 tenantId = clntDbConf.CDB_TenantID;

                            String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                            Int32 pndgFrq = ComplianceDataManager.GetPendingPackageFrequencyDays(tenantId, Setting.Pending_Package_Frequency.GetStringValue()); ;

                            DateTime today = DateTime.Now.Date;
                            String entitySetName = "OrganizationUser";
                            Boolean executeLoop = true;
                            while (executeLoop)
                            {
                                List<ReminderContract> pendingPackageSubscriptions = ComplianceDataManager.GetPendingPackageSubscriptions(tenantId, pndgFrq, entitySetName, today, emailChunkSize).ToList();

                                if (pendingPackageSubscriptions != null && pendingPackageSubscriptions.Count > 0)
                                {
                                    foreach (ReminderContract remCont in pendingPackageSubscriptions)
                                    {
                                        logger.Debug("******************* Placing entry in Email Queue and Notification delivery for a pending package: " + DateTime.Now.ToString() + " *******************");

                                        //Create Dictionary
                                        Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                        dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(remCont.FirstName, " ", remCont.LastName));
                                        dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);

                                        CommunicationMockUpData mockData = new CommunicationMockUpData();
                                        mockData.UserName = string.Concat(remCont.FirstName, " ", remCont.LastName);
                                        mockData.EmailID = remCont.PrimaryEmailAddress;
                                        mockData.ReceiverOrganizationUserID = remCont.OrganizationUserID;

                                        //Send mail
                                        CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.REMINDER_SUBSCRIPTION_PENDING, dictMailData, mockData, tenantId, remCont.HierarchyNodeID);

                                        //Send Message
                                        CommunicationManager.SaveMessageContent(CommunicationSubEvents.REMINDER_SUBSCRIPTION_PENDING, dictMailData, remCont.OrganizationUserID, tenantId);

                                        //Save Notification Delivery
                                        Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                        notificationDelivery.ND_OrganizationUserID = remCont.OrganizationUserID;
                                        notificationDelivery.ND_SubEventTypeID = subEventId;
                                        notificationDelivery.ND_EntityId = remCont.OrganizationUserID;
                                        notificationDelivery.ND_EntityName = entitySetName;
                                        notificationDelivery.ND_IsDeleted = false;
                                        notificationDelivery.ND_CreatedOn = DateTime.Now;
                                        notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                        ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                                        logger.Debug("******************* Placed entry in Email Queue and Notification delivery for a  pending package: " + DateTime.Now.ToString() + " Notification delivery id: " + notificationDelivery.ND_ID
                                            + " *******************");
                                    }

                                    ServiceContext.ReleaseDBContextItems();
                                    executeLoop = true;
                                }
                                else
                                {
                                    executeLoop = false;
                                }
                            }
                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.SendMailForPendingPackage.GetStringValue();
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
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in SendMailForPendingPackage method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        public static void SendNagMails(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendNagMails: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_FOR_NAG_EMAIL.GetStringValue());
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

                Int32 emailChunkSize = AppConsts.CHUNK_SIZE_FOR_MAIL_PENDING_PACKAGE;
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;
                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                        DateTime today = DateTime.Now.Date;
                        String entitySetName = "NagEmail";
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            List<NagEmailData> nagEmailData = ComplianceDataManager.GetNagMailData(tenantId, subEventId, emailChunkSize);

                            if (nagEmailData != null && nagEmailData.Count > 0)
                            {
                                foreach (NagEmailData remCont in nagEmailData)
                                {
                                    logger.Debug("******************* Placing entry in Email Queue and Notification delivery for a Nag Emails: " + DateTime.Now.ToString() + " *******************");

                                    //Create Dictionary
                                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, remCont.Userfullname);
                                    dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                    dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, remCont.Packagename);
                                    dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, remCont.NodeHierarchy);
                                    dictMailData.Add(EmailFieldConstants.CATEGORY_NAME, remCont.CategoryList);
                                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                    dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId);
                                    //UAT 1465 WB: Add same from UAT-1434 to the Nag Email Template
                                    String complianceCatName = ComplianceDataManager.GetCatagoryDetailForNagEmail(tenantId, remCont.HierarchyNodeID, remCont.PackageSubscriptionId);
                                    dictMailData.Add(EmailFieldConstants.COMPLIANCE_CATEGORY_NAME, complianceCatName);
                                    if (!remCont.MainNodeId.IsNullOrEmpty())
                                        dictMailData.Add(EmailFieldConstants.SES_NODE_NOTIFICATION_MAPPING_ID, remCont.MainNodeId);

                                    CommunicationMockUpData mockData = new CommunicationMockUpData();
                                    mockData.UserName = remCont.Userfullname;
                                    mockData.EmailID = remCont.Email;
                                    mockData.ReceiverOrganizationUserID = remCont.OrganizationUserID;

                                    //Send mail
                                    CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_NAG_EMAIL, dictMailData, mockData, tenantId, remCont.HierarchyNodeID);

                                    //Send Message
                                    CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_NAG_EMAIL, dictMailData, remCont.OrganizationUserID, tenantId);

                                    //Save Notification Delivery
                                    Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                    notificationDelivery.ND_OrganizationUserID = remCont.OrganizationUserID;
                                    notificationDelivery.ND_SubEventTypeID = subEventId;
                                    notificationDelivery.ND_EntityId = remCont.PackageSubscriptionId;
                                    notificationDelivery.ND_EntityName = entitySetName;
                                    notificationDelivery.ND_IsDeleted = false;
                                    notificationDelivery.ND_CreatedOn = DateTime.Now;
                                    notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                    ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                                    logger.Debug("******************* Placed entry in Email Queue and Notification delivery for a  pending package: " + DateTime.Now.ToString() + " Notification delivery id: " + notificationDelivery.ND_ID
                                        + " *******************");
                                }

                                ServiceContext.ReleaseDBContextItems();
                                executeLoop = true;
                            }
                            else
                            {
                                executeLoop = false;
                            }
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendNagMails.GetStringValue();
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
                logger.Error("An Error has occured in SendNagEmails method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        public static void SendMailForDeadline(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailForDeadline: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_FOR_DEADLINE.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

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
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                Int32 emailChunkSize = AppConsts.CHUNK_SIZE_FOR_DEADLINE_MAIL;
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;
                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                        // Int32 pndgFrq = ComplianceDataManager.GetPendingPackageFrequencyDays(tenantId, Setting.Pending_Package_Frequency.GetStringValue()); ;

                        DateTime today = DateTime.Now.Date;
                        String entitySetName = "Deadline";
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            List<Entity.ClientEntity.GetUserBeforeExpiringDeadline> applicatlistBeforExpiringDeadline = ComplianceDataManager.GetAllUserBeforeExpiringDeadline(tenantId, emailChunkSize);

                            if (applicatlistBeforExpiringDeadline != null && applicatlistBeforExpiringDeadline.Count > 0)
                            {
                                foreach (Entity.ClientEntity.GetUserBeforeExpiringDeadline remCont in applicatlistBeforExpiringDeadline)
                                {
                                    logger.Debug("******************* Placing entry in email queue and notification delivery for a deadline: " + DateTime.Now.ToString() + " *******************");

                                    String compCategoryList = String.Empty;
                                    logger.Debug("******************* Get category list for tenantId: " + tenant_Id + ", Selected Node Id:  " + remCont.HierarchyNodeId +
                                                 " and Organization User Id : " + remCont.OrganizationUserID + " *******************");
                                    compCategoryList = ComplianceDataManager.GetCategoryDetailForDeadLineNotification(tenantId, remCont.HierarchyNodeId,
                                                                                                                      remCont.OrganizationUserID);

                                    //Create Dictionary
                                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(remCont.UserFirstName, " ", remCont.UserLastName));
                                    dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                    dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, remCont.HierarchyLabel);
                                    dictMailData.Add(EmailFieldConstants.DEADLINE_DATE, remCont.DeadlineDate.ToString("MM-dd-yyyy"));
                                    dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId);
                                    dictMailData.Add(EmailFieldConstants.SES_NODE_NOTIFICATION_MAPPING_ID, remCont.NodeNotificationMappingId);
                                    dictMailData.Add(EmailFieldConstants.COMPLIANCE_CATEGORY_NAME, compCategoryList);

                                    CommunicationMockUpData mockData = new CommunicationMockUpData();
                                    mockData.UserName = string.Concat(remCont.UserFirstName, " ", remCont.UserLastName);
                                    mockData.EmailID = remCont.PrimaryEmailaddress;
                                    mockData.ReceiverOrganizationUserID = remCont.OrganizationUserID;

                                    //Send mail
                                    CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_DEADLINE, dictMailData, mockData, tenantId, remCont.HierarchyNodeId);

                                    //Send Message
                                    CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_DEADLINE, dictMailData, remCont.OrganizationUserID, tenantId);

                                    //Save Notification Delivery
                                    Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                    notificationDelivery.ND_OrganizationUserID = remCont.OrganizationUserID;
                                    notificationDelivery.ND_SubEventTypeID = subEventId;
                                    notificationDelivery.ND_EntityId = remCont.NodeNotificationMappingId;
                                    notificationDelivery.ND_EntityName = entitySetName;
                                    notificationDelivery.ND_IsDeleted = false;
                                    notificationDelivery.ND_CreatedOn = DateTime.Now;
                                    notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                    ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                                    logger.Debug("******************* Placed entry in email queue and notification delivery for a deadline: " + DateTime.Now.ToString() + " Notification delivery id: " + notificationDelivery.ND_ID
                                        + " *******************");
                                }

                                ServiceContext.ReleaseDBContextItems();
                                executeLoop = true;
                            }
                            else
                            {
                                executeLoop = false;
                            }
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendMailForDeadline.GetStringValue();
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
                logger.Error("An Error has occured in SendMailForDeadline method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        public static void QueueImagingData()
        {
            Int32 currentThreadID = Thread.CurrentThread.ManagedThreadId;
            try
            {
                List<Int32> lstTenant = QueueImagingManager.GetTenantListDueForImaging();
                Int32 tenantCount = lstTenant.Count;
                _totalThreadPerSearchTypeAllowed = Convert.ToInt32(ConfigurationManager.AppSettings["TotalThreadPerQueueImaging"]);
                Int32 parallelThreadCount = tenantCount > _totalThreadPerSearchTypeAllowed ? _totalThreadPerSearchTypeAllowed : tenantCount;

                if (parallelThreadCount > 0)
                {
                    Semaphore searchSemaphore = new Semaphore(parallelThreadCount, parallelThreadCount);

                    ManualResetEvent[] manualEvents = new ManualResetEvent[parallelThreadCount];

                    for (Int32 index = 0; index < parallelThreadCount; index++)
                    {
                        manualEvents[index] = new ManualResetEvent(true);
                    }

                    Int32 manualResetEventCounter = 0;
                    Int32 inUseManualResetEvent = 0;
                    Int32 i = 0;
                    foreach (Int32 tenantID in lstTenant)
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        GetverificationDataSynced queueImagingData = new GetverificationDataSynced();

                        logger.Info("** Master thread #" + currentThreadID + " :Started for tenant = " + tenantID + ". **");
                        searchSemaphore.WaitOne();
                        queueImagingData.TenantId = tenantID;
                        queueImagingData.Semaphore = searchSemaphore;
                        queueImagingData.FinishManualResetEvent = manualEvents[manualResetEventCounter];
                        queueImagingData.FinishManualResetEvent.Reset();
                        manualEvents[manualResetEventCounter] = queueImagingData.FinishManualResetEvent;
                        inUseManualResetEvent++;

                        if (i < parallelThreadCount)
                        {
                            manualResetEventCounter++;
                        }
                        ThreadPool.QueueUserWorkItem(ExecuteSyncingStoreProcedure, queueImagingData);

                        if (inUseManualResetEvent == parallelThreadCount)
                        {
                            manualResetEventCounter = WaitHandle.WaitAny(manualEvents);
                            inUseManualResetEvent = inUseManualResetEvent - 1;
                        }
                        i++;

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                            Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.QueueImagingData.GetStringValue();
                            serviceLoggingContract.TenantID = tenantID;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }
                    /* ManualResetEvent: http://msdn.microsoft.com/en-us/library/z6w25xa6.aspx */

                    // Since ThreadPool threads are background threads, wait for the work items to signal before exiting.
                    WaitHandle.WaitAll(manualEvents);
                }
            }
            catch (Exception ex)
            {
                logger.Error("** Master thread #" + currentThreadID + " :Error :{0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
            }
        }

        private static void ExecuteSyncingStoreProcedure(Object getverificationDataSynced)
        {
            GetverificationDataSynced verificationData = null;
            try
            {
                verificationData = (GetverificationDataSynced)getverificationDataSynced;
                QueueImagingManager.SyncVerificationDataForTenant(verificationData.TenantId);
            }
            catch (Exception ex)
            {
                //[SS]:- Added tenant id in exception log.
                String tenantId = verificationData.IsNullOrEmpty() ? AppConsts.ZERO : Convert.ToString(verificationData.TenantId);
                logger.Error("** :Error :{0}, Inner Exception: {1}, Stack Trace: {2},Verification Data Sync TenantID: {3}", ex.Message, ex.InnerException, ex.StackTrace, tenantId);

            }
            finally
            {
                verificationData.Semaphore.Release();
                verificationData.FinishManualResetEvent.Set();

            }

        }


        public static void SendMailForReceivedFromStudentServiceFormStatus(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendNotificationForReceivedFromStudentServiceFormStatus: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_FOR_PENDING_RECEIVED_FROM_STUDENT_SERVICE_FORM_STATUS.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                Entity.AppConfiguration appConfigStudentServiceFormLimit = SecurityManager.GetAppConfiguration(AppConsts.COUNTTO_SET_RECEIVED_FROM_STUDENT_SERVICE_FORM_STATUS);
                String serviceFormStatusLimit = String.Empty;

                if (appConfigStudentServiceFormLimit.IsNotNull())
                {
                    serviceFormStatusLimit = appConfigStudentServiceFormLimit.AC_Value;
                }

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
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                String tenantIds = string.Join<string>(",", clientDbConfs.Select(x => x.CDB_TenantID.ToString()).ToList());

                DateTime jobStartTime = DateTime.Now;
                DateTime jobEndTime;
                String applicationUrl = WebSiteManager.GetInstitutionUrl(1);

                DateTime today = DateTime.Now.Date;
                DataTable dtServiceFormStatus = BackgroundProcessOrderManager.GetDataForReceivedFromStudentServiceFormStatus(tenantIds, Convert.ToInt32(serviceFormStatusLimit));

                logger.Info("******************* Start Creating Excel - Received from Student service form status: " + DateTime.Now.ToString() + " *******************");
                string fileName = string.Concat("Student_Service_Form_Status_", DateTime.Now.ToString("MMddyyyy"));

                DataView dv = dtServiceFormStatus.DefaultView;
                dtServiceFormStatus = dv.ToTable();

                StringBuilder sbHtmlUpdatedRequirements = new StringBuilder(string.Empty);

                sbHtmlUpdatedRequirements.Append("<html>");
                sbHtmlUpdatedRequirements.Append("<head>");
                sbHtmlUpdatedRequirements.Append("<style>");
                sbHtmlUpdatedRequirements.Append("#tbItems {border-spacing: 0;border-collapse: collapse;background-color: transparent;width: 100%;max-width: 100%;margin-bottom: 20px;border: 1px solid #black;}");
                sbHtmlUpdatedRequirements.Append("#tbItems td {border: 1px solid black;}");

                sbHtmlUpdatedRequirements.Append("</style>");
                sbHtmlUpdatedRequirements.Append("</head>");

                sbHtmlUpdatedRequirements.Append("<body>");

                //Initating Table
                sbHtmlUpdatedRequirements.Append("<table id='tbItems'>");

                //Appending head row
                sbHtmlUpdatedRequirements.Append("<tr>");
                sbHtmlUpdatedRequirements.Append("<td>Applicant FirstName</td>");
                sbHtmlUpdatedRequirements.Append("<td>Applicant LastName</td>");
                sbHtmlUpdatedRequirements.Append("<td>Institution</td>");
                sbHtmlUpdatedRequirements.Append("<td>Order Number</td>");
                sbHtmlUpdatedRequirements.Append("<td>Recieved Date</td>");
                sbHtmlUpdatedRequirements.Append("</tr>");

                //Appending Data Rows
                foreach (DataRow row in dtServiceFormStatus.Rows)
                {
                    sbHtmlUpdatedRequirements.Append("<tr>");
                    sbHtmlUpdatedRequirements.Append("<td>" + Convert.ToString(row["ApplicantFirstName"]) + "</td>");
                    sbHtmlUpdatedRequirements.Append("<td>" + Convert.ToString(row["ApplicantLastName"]) + "</td>");
                    sbHtmlUpdatedRequirements.Append("<td>" + Convert.ToString(row["Institution"]) + "</td>");
                    sbHtmlUpdatedRequirements.Append("<td>" + Convert.ToString(row["OrderNumber"]) + "</td>");
                    sbHtmlUpdatedRequirements.Append("<td>" + Convert.ToString(row["RecievedDate"]) + "</td>");
                    sbHtmlUpdatedRequirements.Append("</tr>");
                }

                //Ending Table
                sbHtmlUpdatedRequirements.Append("</table>");

                sbHtmlUpdatedRequirements.Append("</body>");

                sbHtmlUpdatedRequirements.Append("</html>");

                //Creating Excel
                byte[] excel = ExcelReader.GetPendingReceivedfromStudentServiceFormStatus(dtServiceFormStatus, fileName);
                string savedFilePath = ReportManager.SaveUpdatedApplicantRequirements(excel, string.Concat(fileName, ".xls"));
                String attachedFiles = String.Empty;

                Int32? sysCommAttachmentID = null;

                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.APPLICANT_UPDATED_REQUIREMENTS, sbHtmlUpdatedRequirements.ToString());

                Dictionary<String, object> dicMessageParam = new Dictionary<string, object>();
                dicMessageParam.Add("DocumentName", attachedFiles);
                dicMessageParam.Add("IgnoreSpecificTemplate", true);

                Entity.AppConfiguration appConfigurationForStudentServiceForm = SecurityManager.GetAppConfiguration(AppConsts.RECEIVED_FROM_STUDENT_SERVICE_FORM_STATUS);
                String emailIds = String.Empty;
                List<String> emailAddressesToSendNotification = new List<String>();
                if (appConfigurationForStudentServiceForm.IsNotNull())
                {
                    emailIds = appConfigurationForStudentServiceForm.AC_Value;
                    emailAddressesToSendNotification = emailIds.Split(',').ToList<String>();
                    CommunicationTemplateContract obj = new CommunicationTemplateContract();
                }

                List<CommunicationTemplateContract> lstUser = new List<CommunicationTemplateContract>();
                foreach (var email in emailAddressesToSendNotification)
                {
                    lstUser.Add(new CommunicationTemplateContract() { RecieverEmailID = email, RecieverName = "Admin", CurrentUserId = backgroundProcessUserId, ReceiverOrganizationUserId = 1, IsToUser = true });
                }

                CommunicationMockUpData mockData = new CommunicationMockUpData();
                mockData.UserName = "Admin"; //string.Concat(remCont.UserFirstName, " ", remCont.UserLastName);
                mockData.EmailID = emailIds; //remCont.PrimaryEmailaddress;
                mockData.ReceiverOrganizationUserID = AppConsts.NONE;

                logger.Info("******************* End Creating Excel - Updated Applicant Requirement Items for Agency ID: " + DateTime.Now.ToString() + " *******************");

                //Send mail
                Int32? systemCommunicationId = CommunicationManager.SendMailForPendingReceivedFromStudentServiceFormStatus(CommunicationSubEvents.NOTIFICATION_FOR_PENDING_RECEIVED_FROM_STUDENT_SERVICE_FORM_STATUS, dictMailData, lstUser);

                SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                sysCommAttachment.SCA_OriginalDocumentID = -1;
                sysCommAttachment.SCA_OriginalDocumentName = string.Concat(fileName, ".xls");

                sysCommAttachment.SCA_DocumentPath = savedFilePath;
                sysCommAttachment.SCA_DocumentSize = excel.Length;
                sysCommAttachment.SCA_DocAttachmentTypeID = GetAttachmentDocumentType(DocumentAttachmentType.MESSAGE_DOCUMENT.GetStringValue());
                sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                sysCommAttachment.SCA_IsDeleted = false;
                sysCommAttachment.SCA_CreatedBy = backgroundProcessUserId;
                sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                sysCommAttachment.SCA_SystemCommunicationID = Convert.ToInt32(systemCommunicationId);

                sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);

                //Send Message
                // CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_PENDING_RECEIVED_FROM_STUDENT_SERVICE_FORM_STATUS, dictMailData, 1, AppConsts.NONE, dicMessageParam);

                logger.Debug("******************* Placed entry in email queue  for for pendings received from student service form status: " + DateTime.Now.ToString() + " Notification delivery id: *******************");
                ServiceContext.ReleaseDBContextItems();

                //Save service logging data to DB
                if (_isServiceLoggingEnabled)
                {
                    jobEndTime = DateTime.Now;
                    ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                    serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                    serviceLoggingContract.JobName = JobName.SendMailForReceivedFromStudentServiceFormStatus.GetStringValue();
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
                logger.Error("An Error has occured in SendMailForReceivedFromStudentServiceFormStatus method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        private static Int16 GetAttachmentDocumentType(String docAttachmentTypeCode)
        {
            List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();

            return !docAttachmentType.IsNullOrEmpty()
                    ? Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID)
                    : Convert.ToInt16(AppConsts.NONE);
        }
    }
}

