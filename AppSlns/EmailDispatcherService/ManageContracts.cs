using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using System.Collections;
using System.Configuration;
using Entity;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.UI.Contract.Services;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.ContractManagement;
using INTSOF.UI.Contract.BkgOperations;

namespace EmailDispatcherService
{
    public class ManageContracts
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;
        private static string dateFormat = "MM/dd/yyyy";

        public static void SendMailForExpiringOrExpiredItems(CommunicationSubEvents communicationSubEvent, bool isContractNotification, Int32? tenant_Id = null)
        {
            String subEventCode = communicationSubEvent.GetStringValue();
            String entitySetName = string.Empty;
            string notificationForItems = string.Compare(subEventCode, CommunicationSubEvents.CONTRACT_ABOUT_TO_EXPIRE.GetStringValue()) == 0 ? "Expiring" : "Expired";
            string jobName = string.Compare(subEventCode, CommunicationSubEvents.CONTRACT_ABOUT_TO_EXPIRE.GetStringValue()) == 0 ? JobName.SendMailForExpiringItems.GetStringValue() : JobName.SendMailForExpiredItems.GetStringValue();

            if (isContractNotification)
            {
                notificationForItems = string.Concat(notificationForItems, " Contract");
                entitySetName = string.Compare(subEventCode, CommunicationSubEvents.CONTRACT_ABOUT_TO_EXPIRE.GetStringValue()) == 0 ? "ExpiringContractID" : "ExpiredContractID";
            }
            else
            {
                notificationForItems = string.Concat(notificationForItems, " Site");
                entitySetName = string.Compare(subEventCode, CommunicationSubEvents.CONTRACT_ABOUT_TO_EXPIRE.GetStringValue()) == 0 ? "ExpiringSiteID" : "ExpiredSiteID";
            }

            try
            {
                logger.Info("******************* Calling Send Mail For " + notificationForItems + " Items: " + DateTime.Now.ToString() + " *******************");

                Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_CONTRACTS;

                //Getting Sub event
                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(communicationSubEvent.GetStringValue());
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
                        logger.Info("Starting sending e-mail for " + notificationForItems + " items for TenantID: " + clntDbConf.CDB_TenantID);

                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        string applicationURL = WebSiteManager.GetInstitutionUrl(clntDbConf.CDB_TenantID);

                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Info("Started placing entry in Send Mail For " + notificationForItems + " Items.");

                            List<ContractManagementContract> lstContract = new List<ContractManagementContract>();

                            if (isContractNotification)
                                lstContract = ContractManager.GetContractNotificationDetails(subEventCode, clntDbConf.CDB_TenantID, chunkSize);
                            else
                                lstContract = ContractManager.GetSiteNotificationDetails(subEventCode, clntDbConf.CDB_TenantID, chunkSize);

                            if (lstContract.IsNotNull() && lstContract.Count > AppConsts.NONE)
                            {
                                logger.Info("Total " + notificationForItems + " Items for tenant (TenantID: " + clntDbConf.CDB_TenantID + ") found :" + lstContract.Count());

                                List<Int32> lstDistinctEntityIds = new List<int>();

                                if (isContractNotification)
                                    lstDistinctEntityIds = lstContract.Select(cond => cond.ContractId).Distinct().ToList();
                                else
                                    lstDistinctEntityIds = lstContract.Select(cond => cond.SiteID).Distinct().ToList();

                                foreach (Int32 entityId in lstDistinctEntityIds)
                                {
                                    try
                                    {
                                        logger.Info("Starting sending " + notificationForItems + " item notifications for contract # :" + entityId + ", tenant:" + clntDbConf.CDB_TenantID);

                                        ContractManagementContract entityContract = new ContractManagementContract();

                                        if (isContractNotification)
                                            entityContract = lstContract.Where(cond => cond.ContractId == entityId).FirstOrDefault();
                                        else
                                            entityContract = lstContract.Where(cond => cond.SiteID == entityId).FirstOrDefault();

                                        Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                                        dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, "Admin");
                                        dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationURL);
                                        dictMailData.Add(EmailFieldConstants.CONTRACT_OR_SITE_NAME, isContractNotification ? entityContract.AffiliationName : entityContract.SiteName);
                                        dictMailData.Add(EmailFieldConstants.CONTRACT_START_DATE, entityContract.StartDate.HasValue ? entityContract.StartDate.Value.ToString(dateFormat) : string.Empty);
                                        dictMailData.Add(EmailFieldConstants.CONTRACT_END_DATE, entityContract.EndDate.HasValue ? entityContract.EndDate.Value.ToString(dateFormat) : string.Empty);
                                        dictMailData.Add(EmailFieldConstants.CONTRACT_EXPIRY_DATE, entityContract.ExpirationDate.HasValue ? entityContract.ExpirationDate.Value.ToString(dateFormat) : string.Empty);

                                        CommunicationMockUpData mockData = new CommunicationMockUpData();
                                        mockData.UserName = "Admin";
                                        mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                                        mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                                        List<BackgroundOrderDailyReport> lstUsers = new List<BackgroundOrderDailyReport>();

                                        if (isContractNotification)
                                        {
                                            lstUsers = lstContract.Where(cond => cond.ContractId == entityId)
                                                                          .Select(cond => new BackgroundOrderDailyReport
                                                                            {
                                                                                UserName = cond.UserName,
                                                                                EmailAddress = cond.UserEmailAddress,
                                                                                OrganizationUserId = cond.OrganisationUserID
                                                                            }).ToList();
                                        }
                                        else
                                        {
                                            lstUsers = lstContract.Where(cond => cond.SiteID == entityId)
                                                                          .Select(cond => new BackgroundOrderDailyReport
                                                                          {
                                                                              UserName = cond.UserName,
                                                                              EmailAddress = cond.UserEmailAddress,
                                                                              OrganizationUserId = cond.OrganisationUserID
                                                                          }).ToList();
                                        }

                                        List<Entity.ClientEntity.NotificationDelivery> lstNotificationDelivery = new List<Entity.ClientEntity.NotificationDelivery>();
                                        foreach (BackgroundOrderDailyReport user in lstUsers)
                                        {
                                            lstNotificationDelivery.Add(new Entity.ClientEntity.NotificationDelivery()
                                            {
                                                ND_MasterOrgUserID = user.OrganizationUserId,
                                                ND_SubEventTypeID = subEventId,
                                                ND_EntityId = isContractNotification ? entityContract.ContractId : entityContract.SiteID,
                                                ND_EntityName = entitySetName,
                                                ND_IsDeleted = false,
                                                ND_CreatedOn = DateTime.Now,
                                                ND_CreatedBy = backgroundProcessUserId,
                                            });
                                        }

                                        int? systemCommunicationID = CommunicationManager.SendRecurringBackgroundReports(communicationSubEvent, dictMailData, mockData, clntDbConf.CDB_TenantID, -1, null, null, true, true, lstUsers, false);
                                        if (systemCommunicationID.HasValue && systemCommunicationID > AppConsts.NONE)
                                        {
                                            ComplianceDataManager.AddNotificationDeliveryList(clntDbConf.CDB_TenantID, lstNotificationDelivery);
                                        }
                                        logger.Info("End sending " + notificationForItems + " item notifications for contract # :" + entityId + ", tenant:" + clntDbConf.CDB_TenantID);

                                    }
                                    catch (Exception ex)
                                    {
                                        //Log error
                                        logger.Error("An error occured while sending Notification for " + notificationForItems + " Items for contractId #: {0}." + ", Tenant: {4}" +
                                        "The details of which are: {1}, Inner Exception: {2}, Stack Trace: {3}"
                                        , entityId, ex.Message, ex.InnerException, clntDbConf.CDB_TenantID
                                        , ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                    }
                                }
                            }
                            else
                            {
                                executeLoop = false;
                            }
                            logger.Trace("Ended placing entry in Send Mail For " + notificationForItems + " Items:");
                            ServiceContext.ReleaseDBContextItems();
                        }


                        logger.Info("******************* END while loop of Send Mail For " + notificationForItems + " Items method for tenant id: " + clntDbConf.CDB_TenantID.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = jobName;
                            serviceLoggingContract.TenantID = clntDbConf.CDB_TenantID;
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
                logger.Error("An Error has occured in Send Mail For " + notificationForItems + " Items method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
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
