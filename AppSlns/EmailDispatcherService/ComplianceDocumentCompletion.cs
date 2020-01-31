using Business.RepoManagers;
using Entity;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.Services;
using INTSOF.Utils;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Entity.ClientEntity;
using INTSOF.Utils.CommonPocoClasses;
using INTSOF.UI.Contract.Templates;

namespace EmailDispatcherService
{
    public class ComplianceDocumentCompletion
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;

        public static void MarkApplicantDocumentsComplete(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling MarkApplicantDocumentsComplete: " + DateTime.Now.ToString() + " *******************");

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

                Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_COMPLIANCE_DOCUMENT_COMPLETION;
                Int16 completedDataEntryDocumentStatus = LookupManager.GetLookUpData<Entity.lkpDataEntryDocumentStatu>()
                                                               .Where(cnd => !cnd.LDEDS_IsDeleted && cnd.LDEDS_Code == "AAAB").First().LDEDS_ID;

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;

                        Int16 completedDataEntryDocumentStatus_tanant = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDataEntryDocumentStatu>(tenantId)
                                                               .Where(cnd => !cnd.LDEDS_IsDeleted && cnd.LDEDS_Code == "AAAB").First().LDEDS_ID;

                        //Below declared variables used to get org users, on behalf of expiry or archive start and end date
                        DateTime fromDate;
                        DateTime toDate = DateTime.Now;

                        logger.Info("******************* Getting BackgroundServiceExecutionHistory for tenant id: " + tenantId.ToString() + " *******************");

                        BackgroundServiceExecutionHistory executionHistory = ComplianceDataManager.GetLastSuccessfullExecutionHistory(tenantId, JobName.ComplianceDocumentCompletion.GetStringValue());

                        if (!executionHistory.IsNullOrEmpty() && executionHistory.BSEH_DataCaptureTime.HasValue)
                            fromDate = executionHistory.BSEH_DataCaptureTime.Value;
                        else
                            fromDate = new DateTime(1990, 1, 1);

                        logger.Info("******************* Got BackgroundServiceExecutionHistory for tenant id: " + tenantId.ToString() + " *******************");


                        logger.Info("******************* Saving BackgroundServiceExecutionHistory for tenant id: " + tenantId.ToString() + " *******************");
                        BackgroundServiceExecutionHistory backgroundServiceExecutionHistory = new BackgroundServiceExecutionHistory();
                        backgroundServiceExecutionHistory.BSEH_ServiceName = JobName.ComplianceDocumentCompletion.GetStringValue();
                        backgroundServiceExecutionHistory.BSEH_StartTime = DateTime.Now;
                        backgroundServiceExecutionHistory.BSEH_DataCaptureTime = toDate;
                        ComplianceDataManager.SaveBackgroundServiceExecutionHistory(tenantId, backgroundServiceExecutionHistory);
                        logger.Info("******************* Saved BackgroundServiceExecutionHistory for tenant id: " + tenantId.ToString() + " *******************");


                        logger.Info("******************* Getting Users for tenant id: " + tenantId.ToString() + " *******************");
                        List<Int32> lstOrganisationUsers = new List<Int32>();
                        lstOrganisationUsers = ComplianceDataManager.GetUsersToMarkApplicantDocumentsComplete(tenantId, fromDate, toDate, chunkSize, 0);

                        while (!lstOrganisationUsers.IsNullOrEmpty())
                        {

                            logger.Info("******************* START while loop of MarkApplicantDocumentsComplete method for tenant id: " + tenantId.ToString() + " *******************");


                            //Updating Data Entry Document Status in security Db
                            logger.Info("******************* Updating Data Entry Document Status in security Db for tenant id: " + tenantId.ToString() + " *******************");
                            SecurityManager.UpdateStatusOfAppDocumentForDataEntry(lstOrganisationUsers, backgroundProcessUserId, completedDataEntryDocumentStatus, tenantId);
                            logger.Info("******************* Complete updating Data Entry Document Status in security Db for tenant id: " + tenantId.ToString() + " *******************");


                            //Updating Data Entry Document Status in Tenant Db
                            logger.Info("******************* Updating Data Entry Document Status in Tenant Db for tenant id: " + tenantId.ToString() + " *******************");
                            ComplianceDataManager.UpdateStatusForApplicantDocuments(tenantId, lstOrganisationUsers, backgroundProcessUserId, completedDataEntryDocumentStatus_tanant);
                            logger.Info("******************* Complete updating Data Entry Document Status in Tenant Db for tenant id: " + tenantId.ToString() + " *******************");

                            lstOrganisationUsers = ComplianceDataManager.GetUsersToMarkApplicantDocumentsComplete(tenantId, fromDate, toDate, chunkSize, lstOrganisationUsers.Last());
                        }

                        //Updating backgroundServiceExecutionHistory End time
                        logger.Info("******************* Updating BackgroundServiceExecutionHistory End time in Tenant Db for tenant id: " + tenantId.ToString() + " *******************");
                        backgroundServiceExecutionHistory.BSEH_EndTime = DateTime.Now;
                        ComplianceDataManager.UpdateBackgroundServiceExecutionHistory(tenantId, backgroundServiceExecutionHistory, backgroundServiceExecutionHistory.BSEH_Id);
                        logger.Info("******************* Update successfully BackgroundServiceExecutionHistory End time in Tenant Db for tenant id: " + tenantId.ToString() + " *******************");


                        logger.Info("******************* END while loop of MarkApplicantDocumentsComplete method for tenant id: " + tenantId.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.ComplianceDocumentCompletion.GetStringValue();
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
                logger.Error("An Error has occured in MarkApplicantDocumentsComplete method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        #region UAT-2628:
        public static void ConvertAndMergeFailedApplicantDocument(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling ConvertAndMergeFailedApplicantDocument: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_FOR_FAILED_APPLICANT_DOCUMENT_MERGING
                                                                                                       .GetStringValue());
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

                Int32 documentChunkSize = ConfigurationManager.AppSettings["ChunkSizeForDocumentMergingFailedNotifications"].IsNotNull() ?
                                                               Convert.ToInt32(ConfigurationManager.AppSettings["ChunkSizeForDocumentMergingFailedNotifications"])
                                                              : AppConsts.CHUNK_SIZE_FOR_FAILED_DOCUMENT_MERGING;

                Int32 documentRetryCount = ConfigurationManager.AppSettings["MaxRetryCountDocumentMergingFailedNotifications"].IsNotNull() ?
                                                               Convert.ToInt32(ConfigurationManager.AppSettings["MaxRetryCountDocumentMergingFailedNotifications"])
                                                              : AppConsts.THREE;

                String ADBEmailAddresses = ConfigurationManager.AppSettings["EmailAddressesForDocumentMergingFailedNotifications"].IsNotNull() ?
                                                               Convert.ToString(ConfigurationManager.AppSettings["EmailAddressesForDocumentMergingFailedNotifications"])
                                                              : String.Empty;
                List<String> ADBEmailAddressList = new List<String>();
                if (!ADBEmailAddresses.IsNullOrEmpty())
                {
                    ADBEmailAddressList = ADBEmailAddresses.Split(',').ToList();
                }

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {

                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {

                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;

                        //String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                        DateTime today = DateTime.Now.Date;
                        String entitySetName = "ApplicantDocumentID";
                        String institutionName = clntDbConf.Tenant.TenantName;
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            List<ApplicantDocumentPocoClass> applicantDocumentsToMerge = DocumentManager.GetDocumentsDetailsForMergingAndConversing(tenantId, backgroundProcessUserId
                                                                                                        , documentChunkSize, documentRetryCount, entitySetName, subEventId).ToList();

                            if (applicantDocumentsToMerge != null && applicantDocumentsToMerge.Count > 0)
                            {
                                List<ApplicantDocumentPocoClass> distinctApplicantIds = applicantDocumentsToMerge.DistinctBy(x => x.OrganizationUserId).ToList();
                                foreach (ApplicantDocumentPocoClass applicantData in distinctApplicantIds)
                                {
                                    List<ApplicantDocumentPocoClass> applicantDocument = applicantDocumentsToMerge.Where(x =>
                                                                                                                         x.OrganizationUserId == applicantData.OrganizationUserId)
                                                                                                                         .DistinctBy(dst => dst.ApplicantDocumentID).ToList();

                                    List<ApplicantDocument> lstFailedApplicantDocumentToMerge = DocumentManager.AppendApplicantDocumentAutomatically(applicantData.OrganizationUserId
                                                                                                                , tenantId, backgroundProcessUserId
                                                                                                                , applicantDocument, documentRetryCount);
                                    if (!lstFailedApplicantDocumentToMerge.IsNullOrEmpty())
                                    {
                                        logger.Debug("******************* Placing entry in Email Queue and Notification delivery for failed document merging: " + DateTime.Now.ToString() + " *******************");

                                        String documentNames = String.Join(", ", lstFailedApplicantDocumentToMerge.Select(x => x.FileName));
                                        //Create Dictionary
                                        Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                        dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, string.Concat(applicantData.ApplicantFirstName, " ", applicantData.ApplicantLastName));
                                        //dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                        dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, institutionName);
                                        dictMailData.Add(EmailFieldConstants.DOCUMENT_NAME, documentNames);

                                        CommunicationMockUpData mockData = new CommunicationMockUpData();
                                        List<CommunicationTemplateContract> lstcommunicationTemplateContract = new List<CommunicationTemplateContract>();

                                        ADBEmailAddressList.ForEach(ADBEmail =>
                                        {
                                            //CommunicationMockUpData mockData = new CommunicationMockUpData();
                                            //mockData.UserName = "Admin";
                                            //mockData.EmailID = ADBEmail;
                                            //mockData.ReceiverOrganizationUserID = backgroundProcessUserId;

                                            CommunicationTemplateContract communicationTemplateContract = new CommunicationTemplateContract();
                                            communicationTemplateContract.ReceiverOrganizationUserId = AppConsts.NONE;
                                            communicationTemplateContract.RecieverEmailID = ADBEmail;
                                            communicationTemplateContract.RecieverName = "Admin";
                                            communicationTemplateContract.IsToUser = true;
                                            communicationTemplateContract.CurrentUserId = backgroundProcessUserId;
                                            lstcommunicationTemplateContract.Add(communicationTemplateContract);
                                        });

                                        //Send mail
                                        CommunicationManager.SendMailForMultipleReceivers(CommunicationSubEvents.NOTIFICATION_FOR_FAILED_APPLICANT_DOCUMENT_MERGING, dictMailData, mockData, tenantId, -1, lstcommunicationTemplateContract);

                                        //Send Message
                                        //CommunicationManager.SaveMessageContent(CommunicationSubEvents.REMINDER_SUBSCRIPTION_PENDING, dictMailData, remCont.OrganizationUserID, tenantId);

                                        List<NotificationDelivery> lstNotificationDelivery = new List<NotificationDelivery>();
                                        lstFailedApplicantDocumentToMerge.ForEach(docId =>
                                        {
                                            //Save Notification Delivery
                                            Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                            notificationDelivery.ND_OrganizationUserID = applicantData.OrganizationUserId;
                                            notificationDelivery.ND_SubEventTypeID = subEventId;
                                            notificationDelivery.ND_EntityId = docId.ApplicantDocumentID;
                                            notificationDelivery.ND_EntityName = entitySetName;
                                            notificationDelivery.ND_IsDeleted = false;
                                            notificationDelivery.ND_CreatedOn = DateTime.Now;
                                            notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                            lstNotificationDelivery.Add(notificationDelivery);
                                        });

                                        ComplianceDataManager.AddNotificationDeliveryList(tenantId, lstNotificationDelivery);


                                        logger.Debug("******************* Placed entry in Email Queue and Notification delivery for failed document merging: " + DateTime.Now.ToString()
                                            + " *******************");
                                    }
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
                            serviceLoggingContract.JobName = JobName.ConvertAndMergeFailedApplicantDocument.GetStringValue();
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
                logger.Error("An Error has occured in ConvertAndMergeFailedApplicantDocument method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }
        #endregion
    }
}
