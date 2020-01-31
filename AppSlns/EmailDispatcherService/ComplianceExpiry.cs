using Business.RepoManagers;
using Entity;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.Services;
using INTSOF.Utils;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace EmailDispatcherService
{
    public class ComplianceExpiry
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;

        public static void ProcessItemExpiry(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling ProcessItemExpiry: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.COMPLIANCE_ITEM_EXPIRED.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;

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
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Info("******************* START while loop of ProcessItemExpiry method for tenant id: " + tenantId.ToString() + " *******************");
                            logger.Trace("******************* Started placing email in Queue for a chunk of Expired Items: " + DateTime.Now.ToString() + " *******************");
                            executeLoop = RuleManager.ProcessItemExpiry(tenantId, applicationUrl, "ApplicantComplianceItemData", subEventId, tenantName, AppConsts.CHUNK_SIZE_FOR_PROCESS_ITEM_EXPIRY);
                            logger.Trace("******************* Ended placing email in Queue for a chunk of ExpiredItems:" + DateTime.Now.ToString() + " *******************");

                            ServiceContext.ReleaseDBContextItems();
                        }
                        logger.Info("******************* END while loop of ProcessItemExpiry method for tenant id: " + tenantId.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                            Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.ProcessItemExpiry.GetStringValue();
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
                logger.Error("An Error has occured in ProcessItemExpiry method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        public static void SendMailForExpiringItems(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailForExpiringItems: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.COMPLIANCE_ITEM_ABOUT_TO_EXPIRE.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                String subEventCode = CommunicationSubEvents.COMPLIANCE_ITEM_ABOUT_TO_EXPIRE.GetStringValue();

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                var itemRecordObject = CommunicationManager.GetlkpRecordObjectType().Where(cond => cond.OT_Code == LCObjectType.ComplianceItem.GetStringValue()).FirstOrDefault();
                Int32 itemObjectTypeID = 0;

                if (itemRecordObject.IsNotNull())
                {
                    itemObjectTypeID = itemRecordObject.OT_ID;
                }

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

                Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_MAIL_BEFORE_ITEM_EXPIRY;
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;

                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                        String entitySetName = "ExpiringApplicantComplianceItemData";

                        List<Entity.ClientEntity.GetExpiredItemDataList> expiringComplianceItems = new List<Entity.ClientEntity.GetExpiredItemDataList>();
                        expiringComplianceItems = RuleManager.GetExpiringComplianceItems(tenantId, subEventCode, subEventId, entitySetName, chunkSize);

                        while (expiringComplianceItems != null && expiringComplianceItems.Count > 0)
                        {
                            logger.Info("******************* START while loop of SendMailForExpiringItems method for tenant id: " + tenantId.ToString() + " *******************");
                            foreach (Entity.ClientEntity.GetExpiredItemDataList expiringItem in expiringComplianceItems)
                            {
                                //if ((expiringItem.Code != ApplicantCategoryComplianceStatus.Approved.GetStringValue()
                                //      && expiringItem.Code != ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue()) ||
                                //    ((expiringItem.Code == ApplicantCategoryComplianceStatus.Approved.GetStringValue()
                                //    || expiringItem.Code == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue())
                                //    && (expiringItem.ExpirationDate.HasValue && expiringItem.CategoryComplianceExpiryDate.HasValue ?
                                //    expiringItem.ExpirationDate.Value.Date == expiringItem.CategoryComplianceExpiryDate.Value.Date :
                                //    false)
                                //    )
                                //    )
                                //{

                                    logger.Trace("******************* Placing entry in Email Queue and Notification delivery for a chunk of Expiring Compliance Items:" + DateTime.Now.ToString() + " *******************");

                                    //Create Dictionary
                                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(expiringItem.UserFirstName, " ", expiringItem.UserLastName));
                                    dictMailData.Add(EmailFieldConstants.COMPLIANCE_ITEM_EXPIRY_DATE, expiringItem.ExpirationDate.HasValue ? expiringItem.ExpirationDate.Value.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy"));
                                    dictMailData.Add(EmailFieldConstants.COMPLIANCE_ITEM_NAME, expiringItem.ItemName);
                                    dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                    dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, expiringItem.NodeHierarchy);
                                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                    dictMailData.Add(EmailFieldConstants.COMPLIANCE_ITEM_ID, expiringItem.ComplianceItemID.ToString());
                                    dictMailData.Add(EmailFieldConstants.COMPLIANCE_CATEGORY_ID, expiringItem.ComplianceCategoryId.ToString());
                                    dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId.ToString());

                                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                    mockData.UserName = string.Concat(expiringItem.UserFirstName, " ", expiringItem.UserLastName);
                                    mockData.EmailID = expiringItem.PrimaryEmailaddress;
                                    mockData.ReceiverOrganizationUserID = expiringItem.OrgUserId;

                                    //Send mail
                                    //[UAT-1072]
                                    //CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.COMPLIANCE_ITEM_ABOUT_TO_EXPIRE, dictMailData, mockData, tenantId, expiringItem.HierarchyNodeID);
                                    CommunicationManager.SendPackageNotificationMailForCCUserSettings(CommunicationSubEvents.COMPLIANCE_ITEM_ABOUT_TO_EXPIRE, dictMailData, mockData, tenantId, expiringItem.HierarchyNodeID, itemObjectTypeID, expiringItem.ComplianceItemID);

                                    //Send Message
                                    //[UAT-1072]
                                    //CommunicationManager.SaveMessageContent(CommunicationSubEvents.COMPLIANCE_ITEM_ABOUT_TO_EXPIRE, dictMailData, expiringItem.OrgUserId, tenantId);
                                    CommunicationManager.SaveMessageContentForCCUserSettings(CommunicationSubEvents.COMPLIANCE_ITEM_ABOUT_TO_EXPIRE, dictMailData, expiringItem.OrgUserId, tenantId, itemObjectTypeID, expiringItem.ComplianceItemID);

                                    //Save Notification Delivery 
                                    Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                    notificationDelivery.ND_OrganizationUserID = expiringItem.OrgUserId;
                                    notificationDelivery.ND_SubEventTypeID = subEventId;
                                    notificationDelivery.ND_EntityId = expiringItem.ApplicantComplianceItemID;
                                    notificationDelivery.ND_EntityName = entitySetName;
                                    notificationDelivery.ND_IsDeleted = false;
                                    notificationDelivery.ND_CreatedOn = DateTime.Now;
                                    notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                    ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                                    logger.Trace("******************* Placed entry in Email Queue and Notification delivery for a chunk of Expiring Compliance Items: " + DateTime.Now.ToString() + " *******************");
                                //}
                            }
                            logger.Trace("******************* Processed a chunk of Expiring Compliance Items: " + DateTime.Now.ToString() + " *******************");
                            expiringComplianceItems = RuleManager.GetExpiringComplianceItems(tenantId, subEventCode, subEventId, entitySetName, chunkSize);
                        }
                        logger.Info("******************* END while loop of SendMailForExpiringItems method for tenant id: " + tenantId.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendMailForExpiringItems.GetStringValue();
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
                logger.Error("An Error has occured in SendMailForExpiredItems method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        public static void SendMailForExpiredItems(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailForExpiredItems: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.COMPLIANCE_ITEM_EXPIRED.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                String subEventCode = CommunicationSubEvents.COMPLIANCE_ITEM_EXPIRED.GetStringValue();

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

                        String entitySetName = "ExpiredApplicantComplianceItemData";
                        Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_MAIL_AFTER_ITEM_EXPIRY;
                        List<Entity.ClientEntity.GetExpiredItemDataList> expiringComplianceItems = new List<Entity.ClientEntity.GetExpiredItemDataList>();
                        expiringComplianceItems = RuleManager.GetExpiringComplianceItems(tenantId, subEventCode, subEventId, entitySetName, chunkSize);

                        while (expiringComplianceItems != null && expiringComplianceItems.Count > 0)
                        {
                            logger.Info("******************* START while loop of SendMailForExpiredItems method for tenant id: " + tenantId.ToString() + " *******************");
                            foreach (Entity.ClientEntity.GetExpiredItemDataList expiringItem in expiringComplianceItems)
                            {
                                //if ((expiringItem.Code != ApplicantCategoryComplianceStatus.Approved.GetStringValue()
                                //     && expiringItem.Code != ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue()) ||
                                //   ((expiringItem.Code == ApplicantCategoryComplianceStatus.Approved.GetStringValue()
                                //   || expiringItem.Code == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue())
                                //   && (expiringItem.ExpirationDate.HasValue && expiringItem.CategoryComplianceExpiryDate.HasValue ?
                                //   expiringItem.ExpirationDate.Value.Date == expiringItem.CategoryComplianceExpiryDate.Value.Date :
                                //   false)
                                //   )
                                //   )
                                //{

                                    logger.Trace("******************* Placing entry in Email Queue and Notification delivery for a chunk of Expiried Compliance Items:" + DateTime.Now.ToString() + " *******************");

                                    //Create Dictionary
                                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(expiringItem.UserFirstName, " ", expiringItem.UserLastName));
                                    dictMailData.Add(EmailFieldConstants.COMPLIANCE_ITEM_EXPIRY_DATE, expiringItem.ExpirationDate.HasValue ? expiringItem.ExpirationDate.Value.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy"));
                                    dictMailData.Add(EmailFieldConstants.COMPLIANCE_ITEM_NAME, expiringItem.ItemName);
                                    dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                    dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, expiringItem.NodeHierarchy);
                                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                    //dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, ComplianceSetupManager.GetCurrentPackageInfo(expiringItem.CompliancePackageId, tenantId).PackageName);
                                    Entity.ClientEntity.CompliancePackage cmpPackage = ComplianceSetupManager.GetCurrentPackageInfo(expiringItem.CompliancePackageId, tenantId);
                                    if (!cmpPackage.IsNullOrEmpty())
                                    {
                                        dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, cmpPackage.PackageLabel.IsNullOrEmpty() ? cmpPackage.PackageName : cmpPackage.PackageLabel);
                                    }
                                    dictMailData.Add(EmailFieldConstants.COMPLIANCE_ITEM_ID, expiringItem.ComplianceItemID.ToString());
                                    dictMailData.Add(EmailFieldConstants.COMPLIANCE_CATEGORY_ID, expiringItem.ComplianceCategoryId.ToString());
                                    dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId.ToString());

                                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                    mockData.UserName = string.Concat(expiringItem.UserFirstName, " ", expiringItem.UserLastName);
                                    mockData.EmailID = expiringItem.PrimaryEmailaddress;
                                    mockData.ReceiverOrganizationUserID = expiringItem.OrgUserId;

                                    //Send mail
                                    CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.COMPLIANCE_ITEM_EXPIRED, dictMailData, mockData, tenantId, expiringItem.HierarchyNodeID);

                                    //Send Message
                                    CommunicationManager.SaveMessageContent(CommunicationSubEvents.COMPLIANCE_ITEM_EXPIRED, dictMailData, expiringItem.OrgUserId, tenantId);

                                    //Save Notification Delivery 
                                    Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                    notificationDelivery.ND_OrganizationUserID = expiringItem.OrgUserId;
                                    notificationDelivery.ND_SubEventTypeID = subEventId;
                                    notificationDelivery.ND_EntityId = expiringItem.ApplicantComplianceItemID;
                                    notificationDelivery.ND_EntityName = entitySetName;
                                    notificationDelivery.ND_IsDeleted = false;
                                    notificationDelivery.ND_CreatedOn = DateTime.Now;
                                    notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                    ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                                    logger.Trace("******************* Placed entry in Email Queue and Notification delivery for a chunk of Expired Compliance Items: " + DateTime.Now.ToString() + " *******************");
                                //}
                            }
                            logger.Trace("******************* Processed a chunk of Expired Compliance Items: " + DateTime.Now.ToString() + " *******************");
                            expiringComplianceItems = RuleManager.GetExpiringComplianceItems(tenantId, subEventCode, subEventId, entitySetName, chunkSize);
                        }
                        logger.Info("******************* END while loop of SendMailForExpiredItems method for tenant id: " + tenantId.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendMailForExpiredItems.GetStringValue();
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
                logger.Error("An Error has occured in SendMailForExpiredItems method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        public static void ProcessCategoryExpiry(Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                logger.Info("******************* Calling ProcessCategoryExpiry: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_FOR_COMPLIANCE_CATEGORY_EXPIRED.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;

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
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Info("******************* START while loop of ProcessCategoryExpiry method for tenant id: " + tenantId.ToString() + " *******************");
                            logger.Trace("******************* Started placing email in Queue for a chunk of Expired Items: " + DateTime.Now.ToString() + " *******************");
                            executeLoop = RuleManager.ProcessCategoryExpiry(tenantId, applicationUrl, tenantName, AppConsts.CHUNK_SIZE_FOR_PROCESS_ITEM_EXPIRY);
                            logger.Trace("******************* Ended placing email in Queue for a chunk of ExpiredItems:" + DateTime.Now.ToString() + " *******************");

                            ServiceContext.ReleaseDBContextItems();
                        }
                        logger.Info("******************* END while loop of ProcessCategoryExpiry method for tenant id: " + tenantId.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                            Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.ProcessCategoryExpiry.GetStringValue();
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
                logger.Error("An Error has occured in ProcessCategoryExpiry method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        public static void SendMailForDispatchedServiceForm(Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                logger.Info("******************* Calling SendMailForDispatchedServiceFrom: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = new lkpCommunicationSubEvent();
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                String entityTypeCode = CommunicationEntityType.SERVICE_FORM_NOTIFICATION_REMINDER.GetStringValue();

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
                        Boolean isSvcFrmDisNotification = ComplianceDataManager.GetIsSvcFrmDisNotification(tenantId, Setting.SERVICE_FORM_NOTIFICATION.GetStringValue());
                        if (isSvcFrmDisNotification)
                        {
                            logger.Info("******************* START if condition of SendMailForDispatchedServiceForm method for tenant id: " + tenantId.ToString() + " and SvcFrmDisNotification setting" + isSvcFrmDisNotification + " *******************");
                            String tenantName = clntDbConf.Tenant.TenantName;
                            String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);
                            Boolean executeLoop = true;

                            //UAT-2446:Include service form attachment on service form reminder emails
                            List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
                            String docAttachmentTypeCode = DocumentAttachmentType.SYSTEM_DOCUMENT.GetStringValue();
                            Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                                Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);
                            while (executeLoop)
                            {
                                logger.Info("******************* START while loop of SendMailForDispatchedServiceForm method for tenant id: " + tenantId.ToString() + " *******************");
                                List<SvcFrmDisNotificationContract> svcFrmDispatchedNotificationData = ComplianceDataManager.GetServiceFormDispatchedNotificationData(tenantId, AppConsts.CHUNK_SIZE_FOR_SVC_FRM_DISPATCHED_NOTIFICATION).ToList();

                                if (svcFrmDispatchedNotificationData != null && svcFrmDispatchedNotificationData.Count > 0)
                                {
                                    List<Int32> masterOrderIds = svcFrmDispatchedNotificationData.Select(col => col.OrderID).Distinct().ToList();
                                    foreach (Int32 masterOrderID in masterOrderIds)
                                    {
                                        logger.Debug("******************* Placing entry in Email Queue and Notification delivery for Service Form Dispatched Notificatioon/Reminder:" + DateTime.Now.ToString() + " *******************");
                                        List<Int32> serviceFormIds = svcFrmDispatchedNotificationData.Where(cond => cond.OrderID == masterOrderID)
                                            .Select(col => col.ServiceFormID).Distinct().ToList();
                                        foreach (Int32 svcFrmID in serviceFormIds)
                                        {
                                            var svcFrmData = svcFrmDispatchedNotificationData.Where(cond => cond.OrderID == masterOrderID && cond.ServiceFormID == svcFrmID).ToList();
                                            String bkgSvcNames = string.Join(",", svcFrmDispatchedNotificationData.Where(cond => cond.OrderID == masterOrderID && cond.ServiceFormID == svcFrmID)
                                                .Select(col => col.BackgroundServiceName).Distinct());
                                            String packageNames = string.Join(",", svcFrmDispatchedNotificationData.Where(cond => cond.OrderID == masterOrderID && cond.ServiceFormID == svcFrmID)
                                                .Select(col => col.PackageName).Distinct());
                                            String svcGrpNames = string.Join(",", svcFrmDispatchedNotificationData.Where(cond => cond.OrderID == masterOrderID && cond.ServiceFormID == svcFrmID)
                                                .Select(col => col.ServiceGroupName).Distinct());

                                            //Service Form Dispatched Email Issue
                                            String orderNumber = string.Join(",", svcFrmDispatchedNotificationData.Where(cond => cond.OrderID == masterOrderID).Select(col => col.OrderNumber).Distinct());

                                            //Create Dictionary for Messaging Contract
                                            Dictionary<String, object> dicMessageParam = new Dictionary<string, object>();
                                            dicMessageParam.Add("EntityID", svcFrmID);
                                            dicMessageParam.Add("EntityTypeCode", entityTypeCode);

                                            //Create Dictionary
                                            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                            dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(svcFrmData[0].FirstName, " ", svcFrmData[0].LastName));
                                            dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, svcFrmData[0].NodeHierarchy);
                                            dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                            dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, applicationUrl);
                                            dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                            dictMailData.Add(EmailFieldConstants.ORDER_NO, orderNumber);
                                            dictMailData.Add(EmailFieldConstants.ORDER_DATE, svcFrmData[0].OrderDate.ToString("MM/dd/yyyy"));
                                            dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, packageNames);
                                            dictMailData.Add(EmailFieldConstants.SERVICE_NAME, bkgSvcNames);
                                            dictMailData.Add(EmailFieldConstants.SERVICE_FORM_NAME, svcFrmData[0].ServiceFormName);
                                            dictMailData.Add(EmailFieldConstants.SERVICE_FORM_DISPATCH_DATE, svcFrmData[0].DispatchedDate.ToString("MM/dd/yyyy"));
                                            dictMailData.Add(EmailFieldConstants.SERVICE_GROUP_NAME, svcGrpNames);
                                            dictMailData.Add(EmailFieldConstants.ORDER_STATUS, svcFrmData[0].OrderStatus);

                                            CommunicationMockUpData mockData = new CommunicationMockUpData();
                                            mockData.UserName = string.Concat(svcFrmData[0].FirstName, " ", svcFrmData[0].LastName);
                                            mockData.EmailID = svcFrmData[0].PrimaryEmailAddress;
                                            mockData.ReceiverOrganizationUserID = svcFrmData[0].OrganizationUserID;

                                            //Send mail
                                            Int32? systemCommunicationID = null;
                                            systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.DEFAULT_SVC_FORM, dictMailData, mockData, tenantId, svcFrmData[0].HierarchyNodeID, svcFrmID, entityTypeCode);

                                            #region UAT-2446:Include service form attachment on service form reminder emails
                                            //Save Mail Attachment
                                            if (svcFrmData[0].SystemDocumentID.HasValue && svcFrmData[0].SystemDocumentID.Value > AppConsts.NONE)
                                            {
                                                Int32? sysCommAttachmentID = null;
                                                if (systemCommunicationID != null)
                                                {
                                                    SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                                                    sysCommAttachment.SCA_OriginalDocumentID = svcFrmData[0].SystemDocumentID.Value;
                                                    sysCommAttachment.SCA_OriginalDocumentName = svcFrmData[0].DocumentName;
                                                    sysCommAttachment.SCA_DocumentPath = svcFrmData[0].DocumentPath;
                                                    sysCommAttachment.SCA_DocumentSize = svcFrmData[0].DocumentSize.HasValue ? svcFrmData[0].DocumentSize.Value : 0;
                                                    sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                                                    sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                                                    sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                                                    sysCommAttachment.SCA_IsDeleted = false;
                                                    sysCommAttachment.SCA_CreatedBy = backgroundProcessUserId;
                                                    sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                                                    sysCommAttachment.SCA_ModifiedBy = null;
                                                    sysCommAttachment.SCA_ModifiedOn = null;

                                                    sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                                                }

                                                Dictionary<string, string> attachedFiles = new Dictionary<string, string>();
                                                List<ADBMessageDocument> messageDocument = new List<ADBMessageDocument>();

                                                ADBMessageDocument documentData = new ADBMessageDocument();
                                                documentData.DocumentName = svcFrmData[0].DocumentPath;
                                                documentData.OriginalDocumentName = svcFrmData[0].DocumentName;
                                                documentData.DocumentSize = svcFrmData[0].DocumentSize.Value;
                                                documentData.SystemCommunicationAttachmentID = sysCommAttachmentID;
                                                messageDocument.Add(documentData);

                                                attachedFiles = MessageManager.SaveDocumentAndGetDocumentId(messageDocument);
                                                if (!attachedFiles.IsNullOrEmpty())
                                                {
                                                    String documentName = String.Empty;
                                                    attachedFiles.ForEach(a => documentName += a.Key.ToString() + ";");

                                                    dicMessageParam.Add("DocumentName", documentName);
                                                }
                                            }

                                            #endregion

                                            //Send Message
                                            CommunicationManager.SaveMessageContent(CommunicationSubEvents.DEFAULT_SVC_FORM, dictMailData, svcFrmData[0].OrganizationUserID, tenantId, dicMessageParam);
                                        }

                                        var svcFrmNotificationData = svcFrmDispatchedNotificationData.Where(cond => cond.OrderID == masterOrderID)
                                            .Select(col => new { col.OrderServiceFormID, col.OrganizationUserID, col.IsManual }).Distinct().ToList();

                                        foreach (var item in svcFrmNotificationData)
                                        {
                                            Int32 sfID = svcFrmDispatchedNotificationData.Where(x => x.OrderServiceFormID == item.OrderServiceFormID).FirstOrDefault().ServiceFormID;
                                            subEvent = CommunicationManager.GetCommSubEventByEntity(sfID, entityTypeCode, CommunicationSubEvents.DEFAULT_SVC_FORM.GetStringValue());
                                            //subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                                            if (subEvent.IsNotNull())
                                            {
                                                //Save Notification Delivery
                                                Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                                notificationDelivery.ND_OrganizationUserID = item.OrganizationUserID;
                                                notificationDelivery.ND_SubEventTypeID = subEvent.CommunicationSubEventID;
                                                notificationDelivery.ND_EntityId = item.OrderServiceFormID;
                                                //UAT - 1259 : WB: Add a new client setting to separate out the reminder frequency settings for manual and electronic service forms
                                                //notificationDelivery.ND_EntityName = "BkgOrderServiceForm";
                                                if (item.IsManual)
                                                {
                                                    notificationDelivery.ND_EntityName = "BkgOrderServiceFormManual";
                                                }
                                                else
                                                {
                                                    notificationDelivery.ND_EntityName = "BkgOrderServiceFormAutomatic";
                                                }
                                                notificationDelivery.ND_IsDeleted = false;
                                                notificationDelivery.ND_CreatedOn = DateTime.Now;
                                                notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                                ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);
                                            }

                                        }

                                        logger.Debug("******************* Placed entry in Email Queue and Notification delivery for Service Form Dispatched Notificatioon/Reminder: " + DateTime.Now.ToString() + " *******************");
                                    }
                                    //svcFrmDispatchedNotificationData.Select(col => col.OrderID).Distinct().ForEach(masterOrderID =>
                                    //{
                                    //    logger.Debug("******************* Placing entry in Email Queue and Notification delivery for Service Form Dispatched Notificatioon/Reminder:" + DateTime.Now.ToString() + " *******************");

                                    //    svcFrmDispatchedNotificationData.Where(cond => cond.OrderID == masterOrderID)
                                    //        .Select(col => col.ServiceFormID).Distinct().ForEach(svcFrmID =>
                                    //        {
                                    //            var svcFrmData = svcFrmDispatchedNotificationData.Where(cond => cond.OrderID == masterOrderID && cond.ServiceFormID == svcFrmID).ToList();
                                    //            String bkgSvcNames = string.Join(",", svcFrmDispatchedNotificationData.Where(cond => cond.OrderID == masterOrderID && cond.ServiceFormID == svcFrmID)
                                    //                .Select(col => col.BackgroundServiceName).Distinct());
                                    //            String packageNames = string.Join(",", svcFrmDispatchedNotificationData.Where(cond => cond.OrderID == masterOrderID && cond.ServiceFormID == svcFrmID)
                                    //                .Select(col => col.PackageName).Distinct());

                                    //            //Create Dictionary for Messaging Contract
                                    //            Dictionary<String, object> dicMessageParam = new Dictionary<string, object>();
                                    //            dicMessageParam.Add("EntityID", svcFrmID);
                                    //            dicMessageParam.Add("EntityTypeCode", entityTypeCode);

                                    //            //Create Dictionary
                                    //            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                    //            dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(svcFrmData[0].FirstName, " ", svcFrmData[0].LastName));
                                    //            dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, svcFrmData[0].NodeHierarchy);
                                    //            dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                    //            dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                    //            dictMailData.Add(EmailFieldConstants.ORDER_NO,masterOrderID);
                                    //            dictMailData.Add(EmailFieldConstants.ORDER_DATE, svcFrmData[0].OrderDate.ToString("MM/dd/yyyy"));
                                    //            dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, packageNames);
                                    //            dictMailData.Add(EmailFieldConstants.SERVICE_NAME, bkgSvcNames);
                                    //            dictMailData.Add(EmailFieldConstants.SERVICE_FORM_NAME, svcFrmData[0].ServiceFormName);
                                    //            dictMailData.Add(EmailFieldConstants.SERVICE_FORM_DISPATCH_DATE, svcFrmData[0].DispatchedDate.ToString("MM/dd/yyyy"));


                                    //            CommunicationMockUpData mockData = new CommunicationMockUpData();
                                    //            mockData.UserName = string.Concat(svcFrmData[0].FirstName, " ", svcFrmData[0].LastName);
                                    //            mockData.EmailID = svcFrmData[0].PrimaryEmailAddress;
                                    //            mockData.ReceiverOrganizationUserID = svcFrmData[0].OrganizationUserID;

                                    //            //Send mail
                                    //            CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.DEFAULT_SVC_FORM, dictMailData, mockData, tenantId, svcFrmData[0].HierarchyNodeID, svcFrmID, entityTypeCode);

                                    //            //Send Message
                                    //            CommunicationManager.SaveMessageContent(CommunicationSubEvents.DEFAULT_SVC_FORM, dictMailData, svcFrmData[0].OrganizationUserID, tenantId, dicMessageParam);

                                    //        });

                                    //    svcFrmDispatchedNotificationData.Where(cond => cond.OrderID == masterOrderID)
                                    //        .Select(col => new {col.OrderServiceFormID, col.OrganizationUserID}).Distinct().ForEach(condition =>
                                    //        {
                                    //            Int32 sfID = svcFrmDispatchedNotificationData.Where(x => x.OrderServiceFormID == condition.OrderServiceFormID).FirstOrDefault().ServiceFormID;
                                    //            subEvent = CommunicationManager.GetCommSubEventByEntity(sfID, entityTypeCode, CommunicationSubEvents.DEFAULT_SVC_FORM);
                                    //            subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;

                                    //            //Save Notification Delivery
                                    //            Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                    //            notificationDelivery.ND_OrganizationUserID = condition.OrganizationUserID;
                                    //            notificationDelivery.ND_SubEventTypeID = subEventId;
                                    //            notificationDelivery.ND_EntityId = condition.OrderServiceFormID;
                                    //            notificationDelivery.ND_EntityName = "BkgOrderServiceForm";
                                    //            notificationDelivery.ND_IsDeleted = false;
                                    //            notificationDelivery.ND_CreatedOn = DateTime.Now;
                                    //            notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                    //            ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);
                                    //        });

                                    //    logger.Debug("******************* Placed entry in Email Queue and Notification delivery for Service Form Dispatched Notificatioon/Reminder: " + DateTime.Now.ToString() + " *******************");
                                    //});

                                    logger.Trace("Processed chunk for Service Form Dispatched Notificatioon/Reminder. " + DateTime.Now.ToString());

                                    ServiceContext.ReleaseDBContextItems();
                                    executeLoop = true;
                                }
                                else
                                {
                                    executeLoop = false;
                                }
                            }
                            logger.Info("******************* END while loop of SendMailForDispatchedServiceForm method for tenant id: " + tenantId.ToString() + " *******************");

                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.SendMailForDispatchedServiceForm.GetStringValue();
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
                logger.Error("An Error has occured in SendMailForDispatchedServiceFrom method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
            finally
            {
                ServiceContext.ReleaseDBContextItems();
            }
        }

        public static void SendMailForComplianceExceptionExpiry(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailForComplianceExceptionExpiry: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.COMPLIANCE_EXCEPTION_ABOUT_TO_EXPIRE.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                String subEventCode = CommunicationSubEvents.COMPLIANCE_EXCEPTION_ABOUT_TO_EXPIRE.GetStringValue();

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
                        Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_MAIL_COMPLIANCE_EXCEPTION_EXPIRY;
                        List<Entity.ClientEntity.ComplianceExceptionExpiryData> expiringComplianceItems = new List<Entity.ClientEntity.ComplianceExceptionExpiryData>();
                        expiringComplianceItems = RuleManager.GetComplianceExceptionAboutToExpire(tenantId, subEventId, chunkSize);

                        while (expiringComplianceItems != null && expiringComplianceItems.Count > 0)
                        {
                            logger.Info("******************* START while loop of SendMailForComplianceExceptionExpiry method for tenant id: " + tenantId.ToString() + " *******************");
                            foreach (Entity.ClientEntity.ComplianceExceptionExpiryData expiringItem in expiringComplianceItems)
                            {
                                logger.Trace("******************* Placing entry in Email Queue and Notification delivery for a chunk of Compliance Exception Expiry:" + DateTime.Now.ToString() + " *******************");

                                //Create Dictionary
                                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(expiringItem.FirstName, " ", expiringItem.LastName));
                                dictMailData.Add(EmailFieldConstants.EXPIRY_DATE, expiringItem.ExpiryDate.ToString("MM/dd/yyyy"));
                                dictMailData.Add(EmailFieldConstants.ITEM_CATEGORY_NAME, expiringItem.ItemCategoryName);
                                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId.ToString());

                                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                mockData.UserName = String.Concat(expiringItem.FirstName, " ", expiringItem.LastName);
                                mockData.EmailID = expiringItem.PrimaryEmailAddress;
                                mockData.ReceiverOrganizationUserID = expiringItem.OrganizationUserID;

                                //Send mail
                                CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.COMPLIANCE_EXCEPTION_ABOUT_TO_EXPIRE, dictMailData, mockData, tenantId, expiringItem.HierarchyNodeID);

                                //Send Message
                                CommunicationManager.SaveMessageContent(CommunicationSubEvents.COMPLIANCE_EXCEPTION_ABOUT_TO_EXPIRE, dictMailData, expiringItem.OrganizationUserID, tenantId);

                                //Save Notification Delivery 
                                Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                notificationDelivery.ND_OrganizationUserID = expiringItem.OrganizationUserID;
                                notificationDelivery.ND_SubEventTypeID = subEventId;
                                notificationDelivery.ND_EntityId = expiringItem.ApplicantComplianceItemID != AppConsts.NONE ? expiringItem.ApplicantComplianceItemID : expiringItem.ApplicantComplianceCategoryID;
                                notificationDelivery.ND_EntityName = expiringItem.ApplicantComplianceItemID != AppConsts.NONE ? "ComplianceExceptionExpiryForItem" : "ComplianceExceptionExpiryForCategory";
                                notificationDelivery.ND_IsDeleted = false;
                                notificationDelivery.ND_CreatedOn = DateTime.Now;
                                notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                                logger.Trace("******************* Placed entry in Email Queue and Notification delivery for a chunk of Compliance Exception Expiry: " + DateTime.Now.ToString() + " *******************");
                            }
                            logger.Trace("******************* Processed a chunk of Compliance Exception Expiry: " + DateTime.Now.ToString() + " *******************");
                            expiringComplianceItems = RuleManager.GetComplianceExceptionAboutToExpire(tenantId, subEventId, chunkSize);
                        }
                        logger.Info("******************* END while loop of SendMailForComplianceExceptionExpiry method for tenant id: " + tenantId.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.ComplianceExceptionExpiry.GetStringValue();
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
                logger.Error("An Error has occured in SendMailForComplianceExceptionExpiry method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
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
