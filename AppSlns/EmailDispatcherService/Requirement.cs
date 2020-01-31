using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using INTSOF.Utils;
using Entity;
using Business.RepoManagers;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.Services;
using System.Collections;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.UI.Contract.ClinicalRotation;
using INTSOF.UI.Contract.RotationPackages;
using System.Data;
using System.Xml;
using INTSOF.UI.Contract.Templates;


namespace EmailDispatcherService
{
    public class Requirement
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;

        private static Int32 _chunkSize = ConfigurationManager.AppSettings["RecordChunkSize"].IsNotNull() ?
                                                            Convert.ToInt32(ConfigurationManager.AppSettings["RecordChunkSize"]) : AppConsts.NONE;
        private static Int32 _maxRetryCount = ConfigurationManager.AppSettings["MaxRetryCount"].IsNotNull() ?
                                                                    Convert.ToInt32(ConfigurationManager.AppSettings["MaxRetryCount"]) : AppConsts.NONE;
        private static Int32 _retryTimeLag = ConfigurationManager.AppSettings["RetryTimeLag"].IsNotNull() ?
                                                            Convert.ToInt32(ConfigurationManager.AppSettings["RetryTimeLag"]) : AppConsts.NONE;

        public static void ProcessRequirementItemExpiry(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling ProcessRequirementItemExpiry: " + DateTime.Now.ToString() + " *******************");

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
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Info("******************* START while loop of ProcessRequirementItemExpiry method for tenant id: " + tenantId.ToString() + " *******************");
                            logger.Trace("******************* Started placing email in Queue for a chunk of Expired Items: " + DateTime.Now.ToString() + " *******************");
                            executeLoop = RequirementRuleManager.ProcessRequirementItemExpiry(AppConsts.CHUNK_SIZE_FOR_PROCESS_REQUIREMENT_ITEM_EXPIRY, backgroundProcessUserId, tenantId);
                            logger.Trace("******************* Ended placing email in Queue for a chunk of ExpiredItems:" + DateTime.Now.ToString() + " *******************");

                            ServiceContext.ReleaseDBContextItems();
                        }
                        logger.Info("******************* END while loop of ProcessRequirementItemExpiry method for tenant id: " + tenantId.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.ProcessRequirementItemExpiry.GetStringValue();
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
                logger.Error("An Error has occured in ProcessRequirementItemExpiry method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        #region UAT-3139: Client Admin Auto-archive rotations 1 year following the rotation end date.
        public static void ProcessRotationToArchive(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling ProcessRotationToArchive: " + DateTime.Now.ToString() + " *******************");

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
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Info("******************* START while loop of ProcessRotationToArchive method for tenant id: " + tenantId.ToString() + " *******************");
                            logger.Trace("******************* Started placing email in Queue for a chunk of Rotations to Archive: " + DateTime.Now.ToString() + " *******************");
                            executeLoop = ClinicalRotationManager.ProcessRotationToArchive(AppConsts.CHUNK_SIZE_FOR_PROCESS_ROTATION_TO_ARCHIVE, backgroundProcessUserId, tenantId);
                            logger.Trace("******************* Ended placing email in Queue for a chunk of Rotations to Archive:" + DateTime.Now.ToString() + " *******************");

                            ServiceContext.ReleaseDBContextItems();
                        }
                        logger.Info("******************* END while loop of ProcessRotationToArchive method for tenant id: " + tenantId.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.ProcessRotationToArchive.GetStringValue();
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
                logger.Error("An Error has occured in ProcessRequirementItemExpiry method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
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

        public static void CompleteScheduledInvitations()
        {
            try
            {
                logger.Info("******************* Calling CompleteScheduledInvitations: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                //Getting all Scheduled Invitations according to chunk size
                List<SheduledInvitationContract> lstScheduledInvContract = ProfileSharingManager.GetScheduledInvitations(_chunkSize, _maxRetryCount, _retryTimeLag);

                if (lstScheduledInvContract.IsNullOrEmpty() && lstScheduledInvContract.Count == AppConsts.NONE)
                {
                    logger.Info("Total Invitation records which needs to sent is empty. Returning from CompleteScheduledInvitations method.");
                    return;
                }
                else
                {
                    logger.Info("Total Invitation records found:" + lstScheduledInvContract.Count());
                }

                lstScheduledInvContract.DistinctBy(cond => cond.TenantID).Select(col => col.TenantID).ForEach(tenantID =>
                {
                    logger.Info("Starting sending Invitation for Tenant:" + tenantID);
                    try
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;

                        var lstCurrentTenantInvitations = lstScheduledInvContract.Where(cond => cond.TenantID == tenantID).ToList();
                        var lstCurrentTenantInvGroupIDs = lstCurrentTenantInvitations.Where(cond => cond.TenantID == tenantID)
                                                                                     .Select(col => col.InvitationGroupID).Distinct().ToList();

                        logger.Info("Total Invitation Groups for TenantID: " + tenantID + " is : " + lstCurrentTenantInvGroupIDs.Count());
                        foreach (var groupID in lstCurrentTenantInvGroupIDs)
                        {
                            logger.Debug("Starting sending Invitations for PSIG: " + groupID);
                            try
                            {
                                var lstCurrentGroupInvitations = lstCurrentTenantInvitations.Where(cond => cond.InvitationGroupID == groupID).ToList();
                                var rotationID = lstCurrentGroupInvitations.FirstOrDefault(cond => cond.InvitationGroupID == groupID).RotationID;
                                Int32 agencyID = lstCurrentGroupInvitations.FirstOrDefault(cond => cond.InvitationGroupID == groupID).AgencyID;

                                if (rotationID > 0)
                                {
                                    //UAT-2544:- Approved Rotation Student Sharing Functionality
                                    List<Int32> lstClinicalRotationDroppedMembers = ClinicalRotationManager.GetDroppedRotationMembersByRotationID(tenantID, rotationID)
                                                                                                           .Select(cond => cond.CRM_ApplicantOrgUserID)
                                                                                                           .ToList();

                                    if (!lstClinicalRotationDroppedMembers.IsNullOrEmpty())
                                    {
                                        lstCurrentGroupInvitations.RemoveAll(cond => lstClinicalRotationDroppedMembers.Contains(cond.ApplicantID));
                                    }
                                }

                                var applicantOrgUserIdCSV = GetApplicantOrgUserIDs(lstCurrentGroupInvitations);

                                bool overrideAttestationReportWithSelfDoc = false;
                                string selfUploadedDocPath = "";

                                ProfileSharingManager.SaveScheduledInvitationData(applicantOrgUserIdCSV, rotationID, backgroundProcessUserId, tenantID

                                    , lstCurrentGroupInvitations, groupID, agencyID);

                            }
                            catch (Exception ex)
                            {
                                logger.Error(String.Format("An Error has occured while sending invitations for PSIG: " + groupID +
                                             ", the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}"
                                            , ex.Message, ex.InnerException, ex.StackTrace
                                            + " current context key : " + ServiceContext.currentThreadContextKeyString));

                                ServiceContext.ReleaseDBContextItems();
                            }
                            logger.Debug("End sending Invitations for PSIG: " + groupID);
                        }

                        ServiceContext.ReleaseDBContextItems();

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.CopyPackgeDataExecuteRule.GetStringValue();
                            serviceLoggingContract.TenantID = tenantID;
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
                        logger.Error(String.Format("An Error has occured while sending invitations for TenantID: " + tenantID +
                                            ", the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}"
                                           , ex.Message, ex.InnerException, ex.StackTrace
                                           + " current context key : " + ServiceContext.currentThreadContextKeyString));
                        ServiceContext.ReleaseDBContextItems();
                    }

                    logger.Info("Ending sending Invitation for Tenant:" + tenantID);
                });
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in CompleteScheduledInvitations method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                ServiceContext.ReleaseDBContextItems();
            }
        }

        /// <summary>
        /// Method to get Applicant ID CSV by InvitationList
        /// </summary>
        /// <param name="lstCurrentGroupInvitations"></param>
        /// <returns></returns>
        private static String GetApplicantOrgUserIDs(List<SheduledInvitationContract> lstCurrentGroupInvitations)
        {
            return String.Join(",", lstCurrentGroupInvitations.Select(x => x.ApplicantID).Distinct().ToList());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenant_Id"></param>
        public static void SendRotationAbtToStartNotification(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendRotationAbtToStartNotification: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.ROTATION_ABOUT_TO_START.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                String subEventCode = CommunicationSubEvents.ROTATION_ABOUT_TO_START.GetStringValue();

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
                        List<ClinicalRotationMemberDetail> lstClinicalRotationMemberDetail = new List<ClinicalRotationMemberDetail>();
                        lstClinicalRotationMemberDetail = ClinicalRotationManager.GetRotationMemberDetailForNagMail(subEventId, AppConsts.CHUNK_SIZE_FOR_PROCESS_REQUIREMENT_ITEM_EXPIRY, tenantId);
                        while (lstClinicalRotationMemberDetail.Count > AppConsts.NONE)
                        {
                            logger.Info("******************* START while loop of SendRotationAbtToStartNotification method for tenant id: " + tenantId.ToString() + " *******************");
                            logger.Trace("******************* Started placing email in Queue for a chunk of students: " + DateTime.Now.ToString() + " *******************");

                            logger.Info("******************* START while loop of SendRotationAbtToStartNotification method for tenant id: " + tenantId.ToString() + " *******************");
                            foreach (ClinicalRotationMemberDetail clinicalRotationMemberDetail in lstClinicalRotationMemberDetail)
                            {
                                logger.Trace("******************* Placing entry in Email Queue and Notification delivery for a chunk of rotation members:" + DateTime.Now.ToString() + " *******************");

                                //Create Dictionary
                                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, clinicalRotationMemberDetail.ApplicantName);
                                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId.ToString());
                                dictMailData.Add(EmailFieldConstants.ROTATION_NAME, clinicalRotationMemberDetail.RotationName);
                                dictMailData.Add(EmailFieldConstants.ROTATION_START_DATE, Convert.ToDateTime(clinicalRotationMemberDetail.StartDate).ToString("MM/dd/yyyy"));
                                dictMailData.Add(EmailFieldConstants.ROTATION_DETAILS, ClinicalRotationManager.GenerateRotationDetailsHTML(clinicalRotationMemberDetail));

                                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                mockData.UserName = clinicalRotationMemberDetail.ApplicantName;
                                mockData.EmailID = clinicalRotationMemberDetail.PrimaryEmailaddress;
                                mockData.ReceiverOrganizationUserID = clinicalRotationMemberDetail.OrganizationUserId;

                                //Send mail
                                CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.ROTATION_ABOUT_TO_START, dictMailData, mockData, tenantId, -1, null, null, true, false, null, clinicalRotationMemberDetail.RotationHirarchyIds, clinicalRotationMemberDetail.RotationID);

                                //Send Message
                                CommunicationManager.SaveMessageContent(CommunicationSubEvents.ROTATION_ABOUT_TO_START, dictMailData, clinicalRotationMemberDetail.OrganizationUserId, tenantId);

                                //Save Notification Delivery 
                                Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                notificationDelivery.ND_OrganizationUserID = clinicalRotationMemberDetail.OrganizationUserId;
                                notificationDelivery.ND_SubEventTypeID = subEventId;
                                notificationDelivery.ND_EntityId = clinicalRotationMemberDetail.RotationID;
                                notificationDelivery.ND_EntityName = "RotationNagEmail";
                                notificationDelivery.ND_IsDeleted = false;
                                notificationDelivery.ND_CreatedOn = DateTime.Now;
                                notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                                logger.Trace("******************* Placed entry in Email Queue and Notification delivery for a chunk of rotation members: " + DateTime.Now.ToString() + " *******************");
                            }
                            logger.Trace("******************* Processed a chunk of rotation members: " + DateTime.Now.ToString() + " *******************");
                            ServiceContext.ReleaseDBContextItems();
                            lstClinicalRotationMemberDetail = ClinicalRotationManager.GetRotationMemberDetailForNagMail(subEventId, AppConsts.CHUNK_SIZE_FOR_PROCESS_REQUIREMENT_ITEM_EXPIRY, tenantId);
                        }
                        logger.Info("******************* END while loop of SendRotationAbtToStartNotification method for tenant id: " + tenantId.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.RotationAboutToStart.GetStringValue();
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
                logger.Error("An Error has occured in SendRotationAbtToStartNotification method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        #region Method move to "Clinical Rotation Manager"
        //public static string GenerateRotationDetailsHTML(ClinicalRotationMemberDetail rotationDetailsContract)
        //{
        //    if (rotationDetailsContract.IsNullOrEmpty())
        //    {
        //        return String.Empty;
        //    }
        //    StringBuilder _sbRotationDetails = new StringBuilder();
        //    _sbRotationDetails.Append("<h4><i>Rotation Details:</i></h4>");
        //    _sbRotationDetails.Append("<div style='line-height:21px'>");
        //    _sbRotationDetails.Append("<ul style='list-style-type: disc'>");

        //    if (!rotationDetailsContract.AgencyName.IsNullOrEmpty())
        //    {
        //        _sbRotationDetails.Append("<li><b>" + "Agency Name: </b>" + rotationDetailsContract.AgencyName + "</li>");
        //    }
        //    if (!rotationDetailsContract.ComplioID.IsNullOrEmpty())
        //    {
        //        _sbRotationDetails.Append("<li><b>" + "Complio ID: </b>" + rotationDetailsContract.ComplioID + "</li>");
        //    }
        //    if (!rotationDetailsContract.RotationName.IsNullOrEmpty())
        //    {
        //        _sbRotationDetails.Append("<li><b>" + "Rotation Name: </b>" + rotationDetailsContract.RotationName + "</li>");
        //    }
        //    if (!rotationDetailsContract.Department.IsNullOrEmpty())
        //    {
        //        _sbRotationDetails.Append("<li><b>" + "Department: </b>" + rotationDetailsContract.Department + "</li>");
        //    }
        //    if (!rotationDetailsContract.Program.IsNullOrEmpty())
        //    {
        //        _sbRotationDetails.Append("<li><b>" + "Program: </b>" + rotationDetailsContract.Program + "</li>");
        //    }
        //    if (!rotationDetailsContract.Course.IsNullOrEmpty())
        //    {
        //        _sbRotationDetails.Append("<li><b>" + "Course: </b>" + rotationDetailsContract.Course + "</li>");
        //    }
        //    if (!rotationDetailsContract.Term.IsNullOrEmpty())
        //    {
        //        _sbRotationDetails.Append("<li><b>" + "Term: </b>" + rotationDetailsContract.Term + "</li>");
        //    }
        //    if (!rotationDetailsContract.TypeSpecialty.IsNullOrEmpty())
        //    {
        //        _sbRotationDetails.Append("<li><b>" + "Type/Specialty: </b>" + rotationDetailsContract.TypeSpecialty + "</li>");
        //    }
        //    if (!rotationDetailsContract.UnitFloorLoc.IsNullOrEmpty())
        //    {
        //        _sbRotationDetails.Append("<li><b>" + "Unit/Floor or Location: </b>" + rotationDetailsContract.UnitFloorLoc + "</li>");
        //    }
        //    if (!rotationDetailsContract.RecommendedHours.IsNullOrEmpty())
        //    {
        //        _sbRotationDetails.Append("<li><b>" + "# of Recommended Hours: </b>" + rotationDetailsContract.RecommendedHours + "</li>");
        //    }
        //    if (!rotationDetailsContract.DaysName.IsNullOrEmpty())
        //    {
        //        _sbRotationDetails.Append("<li><b>" + "Days: </b>" + rotationDetailsContract.DaysName + "</li>");
        //    }
        //    if (!rotationDetailsContract.Shift.IsNullOrEmpty())
        //    {
        //        _sbRotationDetails.Append("<li><b>" + "Shift: </b>" + rotationDetailsContract.Shift + "</li>");
        //    }
        //    if (!rotationDetailsContract.Time.IsNullOrEmpty() && rotationDetailsContract.Time != "-")
        //    {
        //        _sbRotationDetails.Append("<li><b>" + "Time: </b>" + rotationDetailsContract.Time + "</li>");
        //    }
        //    if (!rotationDetailsContract.StartDate.IsNullOrEmpty() && !rotationDetailsContract.EndDate.IsNullOrEmpty())
        //    {
        //        _sbRotationDetails.Append("<li><b>" + "Dates: </b>" + Convert.ToDateTime(rotationDetailsContract.StartDate).ToString("MM/dd/yyyy") + " - " + Convert.ToDateTime(rotationDetailsContract.EndDate).ToString("MM/dd/yyyy") + "</li>");
        //    }
        //    _sbRotationDetails.Append("</ul>");
        //    _sbRotationDetails.Append("</div>");
        //    return Convert.ToString(_sbRotationDetails);
        //}
        #endregion

        /// <summary>
        /// UAT-2414, Create a snapshot on Rotation End Date 
        /// </summary>
        public static void CreateRequirementSnapshotOnRotationEnd(Int32? tenant_Id = null)
        {
            try
            {

                logger.Info("******************* Calling CreateRequirementSnapshotOnRotationEnd: " + DateTime.Now.ToString() + " *******************");

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

                Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_CREATE_REQUIREMENT_SNAPSHOT_ON_ROTATION_END;

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    Int32 tenantId = clntDbConf.CDB_TenantID;

                    logger.Info("******************* START while loop of CreateRequirementSnapshotOnRotationEnd method for tenant id: " + tenantId.ToString() + " *******************");

                    try
                    {
                        if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;

                            Boolean executeLoop = true;
                            while (executeLoop)
                            {
                                List<RequirmentPkgSubscriptionDataContract> lstRequirmentPkgSubscriptionDataContract = new List<RequirmentPkgSubscriptionDataContract>();

                                logger.Info("******************* START Getting GetDataForCopyToRequirement*******************");

                                lstRequirmentPkgSubscriptionDataContract = ProfileSharingManager.GetRequirementSubscriptionDataForSnapshot(tenantId, chunkSize);

                                logger.Info("******************* END Getting GetDataForCopyToRequirement*******************");

                                if (!lstRequirmentPkgSubscriptionDataContract.IsNullOrEmpty())
                                {
                                    foreach (RequirmentPkgSubscriptionDataContract reqPkgSubsContract in lstRequirmentPkgSubscriptionDataContract)
                                    {
                                        logger.Info("*******************  Store Procedure Execution Start for CreateRequirementSnapshotOnRotationEnd method for tenant id: " + tenantId.ToString() + " for Requirement Package Subcription ID: " + reqPkgSubsContract.RequirementPackageSubscriptionID + " *******************");

                                        Boolean status = ProfileSharingManager.SaveRequirementSnapshotOnRotationEnd(tenantId, reqPkgSubsContract.RequirementPackageSubscriptionID, backgroundProcessUserId, reqPkgSubsContract.ProfileSharingInvitationIDs);

                                        logger.Info("*******************  Store Procedure Execution End for CreateRequirementSnapshotOnRotationEnd method for tenant id: " + tenantId.ToString() + " for Requirement Package Subcription ID: " + reqPkgSubsContract.RequirementPackageSubscriptionID + " *******************");
                                    }
                                }
                                else
                                {
                                    executeLoop = false;
                                }
                            }

                            logger.Info("******************* END while loop of CreateRequirementSnapshotOnRotationEnd method for tenant id: " + tenantId.ToString() + " *******************");

                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.CreateRequirementSnapshotOnRotationEnd.GetStringValue();
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
                        logger.Error("An Error has occured in CreateRequirementSnapshotOnRotationEnd method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString + " current TenantId is : " + clntDbConf.CDB_TenantID);
                        if (ex.Data.Count > 0)
                        {
                            foreach (DictionaryEntry de in ex.Data)
                            {
                                logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                            }
                        }
                        ServiceContext.ReleaseDBContextItems();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in CreateRequirementSnapshotOnRotationEnd method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        public static void RequirementPkgSync(Int32? tenant_Id = null)
        {
            try
            {
                //UAT-4657
             //   List<Int32> lstTenantIds = SharedRequirementPackageManager.GetPendingTenantsWhichPkgVersioningOrCategoryDisassociationPending();

                logger.Info("******************* Calling RequirementPkgSync: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                Int32 chunkSize = ConfigurationManager.AppSettings["RequirementPkgSync_ChunkSize"].IsNotNull() ?
                                                            Convert.ToInt32(ConfigurationManager.AppSettings["RequirementPkgSync_ChunkSize"])
                                                            : AppConsts.CHUNK_SIZE_FOR_REQUIREMENT_PACKAGE_SYNC;
                //Int32 retryTimeLag = ConfigurationManager.AppSettings["RequirementPkgSync_RetryTimeLag"].IsNotNull() ?
                //                                            Convert.ToInt32(ConfigurationManager.AppSettings["RequirementPkgSync_RetryTimeLag"])
                //                                           : AppConsts.RETRY_TIME_LAG_FOR_REQUIREMENT_PACKAGE_SYNC;
                //UAT-3230
                Int32 retryCount = AppConsts.RETRY_COUNT_FOR_REQUIREMENT_PACKAGE_SYNC;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                    item =>
                    {
                        ClientDBConfiguration config = new ClientDBConfiguration();
                        config.CDB_TenantID = item.CDB_TenantID;
                        config.CDB_ConnectionString = item.CDB_ConnectionString;
                        config.Tenant = new Tenant();
                        config.Tenant.TenantName = item.Tenant.TenantName; return config;
                    }).ToList();

                //UAT-4657
             //   clientDbConfs = clientDbConfs.Where(con => !lstTenantIds.Contains(con.CDB_TenantID)).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }
                String AllSyncReqPkgObjectIds = String.Empty;
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    Int32 tenantId = clntDbConf.CDB_TenantID;

                    logger.Info("******************* START while loop of RequirementPkgSync method for tenant id: " + tenantId.ToString() + " *******************");

                    try
                    {
                        if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;

                            Boolean executeLoop = true;
                            while (executeLoop)
                            {
                                logger.Info("******************* START Getting GetRequirementPackageObjectIds*******************");

                                //UAT-3230
                                String reqPackageObjectIds = SharedRequirementPackageManager.GetPendingRequirementPackageObjectIdsForTenant(tenantId, chunkSize, retryCount);
                                //String reqPackageObjectIds = SharedRequirementPackageManager.GetRequirementPackageObjectIds(tenantId, chunkSize, retryTimeLag);

                                logger.Info("******************* END Getting GetRequirementPackageObjectIds*******************");

                                if (!reqPackageObjectIds.IsNullOrEmpty())
                                {

                                    #region  UAT-3273- Get status before rule execution
                                    logger.Info("*******************  START Before rule execution- Getting Approved Rotaions for GetApprovedSubscrptionByRequirementPackageSubscriptionIDs method for tenant id: " + tenantId.ToString() + " , and reqPackageObjectIds: " + reqPackageObjectIds + " *******************");
                                    var dataBeforeRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(tenantId, reqPackageObjectIds);
                                    logger.Info("*******************  END Before rule execution- Getting Approved Rotaions for GetApprovedSubscrptionByRequirementPackageSubscriptionIDs*******************");
                                    #endregion

                                    #region 4015
                                    List<Int32> lstAlreadyApprovedRPS_IDs = new List<Int32>();

                                    #endregion

                                    #region UAT-3805
                                    //List<Int32> lstRequirementPackageSubscriptionIds = reqPackageObjectIds.Split(',').ConvertIntoIntList();
                                    List<Entity.ClientEntity.RequirementPackageSubscription> lstBeforeRequirementPackageSubscriptions =
                                        ProfileSharingManager.GetReqSubscriptionByObjectIDs(tenantId, reqPackageObjectIds);
                                    //lstAlreadyApprovedRPS_IDs = lstBeforeRequirementPackageSubscriptions.Select(sel => sel.RPS_ID).Distinct().ToList();

                                    //RequirementRuleManager.GetRequirementPackageSubscriptionBySubscriptionIds(tenantId, lstRequirementPackageSubscriptionIds);
                                    Dictionary<Int32, String> dicBeforeRPS = new Dictionary<Int32, String>();
                                    foreach (Entity.ClientEntity.RequirementPackageSubscription item in lstBeforeRequirementPackageSubscriptions)
                                    {
                                        if (!item.IsNullOrEmpty())
                                        {
                                            String alreadyApprovedCategoryIds = String.Empty;
                                            String categoryApproveStatusCode = RequirementCategoryStatus.APPROVED.GetStringValue();
                                            List<Int32> lstApprovedCategories = item.ApplicantRequirementCategoryDatas.
                                                Where(cond => !cond.ARCD_IsDeleted && cond.lkpRequirementCategoryStatu.RCS_Code == categoryApproveStatusCode)
                                                .Select(sel => sel.ARCD_RequirementCategoryID).Distinct().ToList();
                                            if (!lstApprovedCategories.IsNullOrEmpty())
                                            {
                                                alreadyApprovedCategoryIds = String.Join(",", lstApprovedCategories);
                                            }
                                            if (!dicBeforeRPS.ContainsKey(item.RPS_ID))
                                                dicBeforeRPS.Add(item.RPS_ID, alreadyApprovedCategoryIds);

                                            #region 4015
                                            if (String.Compare(item.lkpRequirementPackageStatu.RPS_Code, RequirementPackageStatus.REQUIREMENT_COMPLIANT.GetStringValue(), true) == AppConsts.NONE)
                                            {
                                                lstAlreadyApprovedRPS_IDs.Add(item.RPS_ID);
                                            }
                                            #endregion
                                        }
                                    }


                                    #endregion


                                    logger.Info("*******************  Store Procedure Execution Start for RequirementPkgSync method for tenant id: " + tenantId.ToString() + " , and reqPackageObjectIds: " + reqPackageObjectIds + " *******************");
                                    RequirementPackageManager.RequirementPkgSync(tenantId, backgroundProcessUserId, reqPackageObjectIds);

                                    logger.Info("*******************  Store Procedure Execution Start for GetRequirenmentPackageIdsInScheduleTask method for tenant id: " + tenantId.ToString() + " , and reqPackageObjectIds: " + reqPackageObjectIds + " *******************");
                                    RequirementPackageManager.GetRequirenmentPackageIdsInScheduleTask(tenantId, backgroundProcessUserId);

                                    #region UAT-3230
                                    if (AllSyncReqPkgObjectIds.IsNullOrEmpty())
                                    {
                                        AllSyncReqPkgObjectIds = AllSyncReqPkgObjectIds + reqPackageObjectIds;
                                    }
                                    else
                                    {
                                        AllSyncReqPkgObjectIds = AllSyncReqPkgObjectIds + "," + reqPackageObjectIds;
                                    }
                                    SharedRequirementPackageManager.UpdateSyncRequirementPackageObjectsCount(backgroundProcessUserId, reqPackageObjectIds);
                                    #endregion

                                    logger.Info("*******************  Store Procedure Execution End for RequirementPkgSync method for tenant id: " + tenantId.ToString() + " , and reqPackageObjectIds: " + reqPackageObjectIds + " *******************");

                                    #region UAT-3805

                                    List<Entity.ClientEntity.RequirementPackageSubscription> lstAfterRequirementPackageSubscriptions = ProfileSharingManager.GetReqSubscriptionByObjectIDs(tenantId, reqPackageObjectIds);
                                    //RequirementRuleManager.GetRequirementPackageSubscriptionBySubscriptionIds(tenantId, lstRequirementPackageSubscriptionIds);

                                    #endregion


                                    #region  UAT-3273- Get status after rule execution
                                    logger.Info("*******************  START After rule execution- Getting Approved Rotaions for GetApprovedSubscrptionByRequirementPackageSubscriptionIDs method for tenant id: " + tenantId.ToString() + " , and reqPackageObjectIds: " + reqPackageObjectIds + " *******************");
                                    var dataAfterRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(tenantId, reqPackageObjectIds);
                                    ProfileSharingManager.CheckAndSendNotificationForNewlyApprovedRotations(dataBeforeRuleExecution, dataAfterRuleExecution, tenantId);
                                    logger.Info("*******************  END After rule execution- Getting Approved Rotaions for GetApprovedSubscrptionByRequirementPackageSubscriptionIDs*******************");
                                    #endregion

                                    foreach (Entity.ClientEntity.RequirementPackageSubscription reqPkgSubDetails in lstAfterRequirementPackageSubscriptions)
                                    {
                                        String categoryIds = String.Empty;
                                        if (dicBeforeRPS.ContainsKey(reqPkgSubDetails.RPS_ID))
                                            categoryIds = dicBeforeRPS[reqPkgSubDetails.RPS_ID];

                                        List<Int32> affectedCategoryIds = new List<Int32>();
                                        affectedCategoryIds = reqPkgSubDetails.ApplicantRequirementCategoryDatas.Where(con => !con.ARCD_IsDeleted).Select(sel => sel.ARCD_RequirementCategoryID).ToList();
                                        String approvedCategoryIds = String.Join(",", affectedCategoryIds);
                                        ProfileSharingManager.SendItemDocNotificationOnCategoryApproval(tenantId, approvedCategoryIds, reqPkgSubDetails.RPS_ApplicantOrgUserID
                                                                                                                   , categoryIds, lkpUseTypeEnum.ROTATION.GetStringValue()
                                                                                                                   , null, reqPkgSubDetails.RPS_ID, backgroundProcessUserId);
                                    }

                                    List<Int32> lstRpsIds = new List<Int32>();
                                    if (!lstAfterRequirementPackageSubscriptions.IsNullOrEmpty())
                                    {
                                        lstRpsIds = lstAfterRequirementPackageSubscriptions.Select(sel => sel.RPS_ID).ToList();
                                        if (lstAlreadyApprovedRPS_IDs.Count > AppConsts.NONE)
                                            lstRpsIds.RemoveAll(con => lstAlreadyApprovedRPS_IDs.Any(r => con == r));

                                        if (!lstRpsIds.IsNullOrEmpty())
                                        {
                                            foreach (Int32 rpsId in lstRpsIds.Distinct())
                                            {
                                                if (rpsId > AppConsts.NONE)
                                                {
                                                    ApplicantRequirementManager.SendMailToInstPrecpReqPKgCompliantStatus(tenantId, rpsId, backgroundProcessUserId);
                                                }
                                            }
                                        }
                                    }

                                    //UAT-3990
                                    if (!lstRpsIds.IsNullOrEmpty() && lstRpsIds.Count > AppConsts.NONE)
                                    {
                                        String rpsIds = String.Join(",", lstRpsIds);
                                        var packagSubscriptionStatuses = Business.RepoManagers.ApplicantRequirementManager.GetPackageSubscriptionCategoryStatus(tenantId, rpsIds);

                                        foreach (DataRow item in packagSubscriptionStatuses.Rows)
                                        {
                                            var status = String.Empty;
                                            if (Convert.ToString(item["RequirementCategoryStatusCode"]).Equals(AppConsts.REQUIREMENT_PACKAGE_SUBSCRIPTION_APPROVED_CODE))
                                            {
                                                status = "Approved";
                                            }
                                            else if (Convert.ToString(item["RequirementCategoryStatusCode"]).Equals(AppConsts.REQUIREMENT_PACKAGE_SUBSCRIPTION_PENDING_REVIEW_CODE))
                                            {
                                                status = "Pending Review";
                                            }

                                            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                            dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, Convert.ToString(item["ApplicantName"]));
                                            dictMailData.Add(EmailFieldConstants.ROTATION_NAME, Convert.ToString(item["RotationName"]));
                                            dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, Convert.ToString(item["PackageName"]));
                                            dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId);
                                            dictMailData.Add(EmailFieldConstants.STATUS, status);

                                            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();


                                            //UAT-3160
                                            String rotationHierarchyIDs = String.Empty;
                                            Int32 requirementPackageSubscriptionID = Convert.ToInt32(item["RequirementPackageSubscriptionID"]).IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(item["RequirementPackageSubscriptionID"]);
                                            if (requirementPackageSubscriptionID > AppConsts.NONE)
                                            {
                                                rotationHierarchyIDs = ApplicantRequirementManager.GetRotationHierarchyIdsBasedOnSubscriptionID(tenantId, requirementPackageSubscriptionID);
                                            }
                                            #region UAT-3364 - Granular permission for Rotation Creator
                                            Boolean IsAllowed = ComplianceDataManager.CheckRotationCreatorGranularPermissionsByOrgUserIdForSendNotificationForCategoryApproved(Convert.ToInt32(item["OrganizationUserID"]));
                                            #endregion
                                            if (IsAllowed)
                                            {
                                                mockData.UserName = Convert.ToString(item["UserName"]);
                                                mockData.EmailID = Convert.ToString(item["Email"]);
                                                mockData.ReceiverOrganizationUserID = Convert.ToInt32(item["OrganizationUserID"]);
                                            }
                                            else
                                            {
                                                mockData.UserName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
                                                mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                                                mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                                            }
                                            //Send mail
                                            CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_ALL_ROTATION_CATEGORIES_APPROVED_OR_PENDING_REVIEW, dictMailData, mockData, tenantId, -1, null, null, true, false, null, rotationHierarchyIDs);
                                        }
                                    }
                                }
                                else
                                {
                                    executeLoop = false;
                                }
                            }

                            logger.Info("******************* END while loop of RequirementPkgSync method for tenant id: " + tenantId.ToString() + " *******************");

                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.RequirementPackageSynching.GetStringValue();
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
                        logger.Error("An Error has occured in RequirementPkgSync method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString + " current TenantId is : " + clntDbConf.CDB_TenantID);
                        if (ex.Data.Count > 0)
                        {
                            foreach (DictionaryEntry de in ex.Data)
                            {
                                logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                            }
                        }
                        ServiceContext.ReleaseDBContextItems();
                    }
                }
                #region UAT-3230
                SharedRequirementPackageManager.RemoveSyncRequirementPackageObjects(backgroundProcessUserId, AllSyncReqPkgObjectIds);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in RequirementPkgSync method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }

        }

        //UAT-2533 Archive all the packages whose end date has passed.
        public static void RequirementPkgAutoArchive()
        {
            try
            {
                DateTime jobStartTime = DateTime.Now;
                DateTime jobEndTime;

                logger.Info("******************* Calling Requirement Package Auto Archival: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;


                logger.Info("*******************  Store Procedure Execution Start for Auto Archival of Requirement Packages ****************************");
                SharedRequirementPackageManager.ScheduleAutoArchivalRequirementPackage(backgroundProcessUserId);
                logger.Info("*******************  Store Procedure Execution End for Auto Archival of Requirement Packages ****************************");


                //Save service logging data to DB
                if (_isServiceLoggingEnabled)
                {
                    jobEndTime = DateTime.Now;
                    ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                    serviceLoggingContract.ServiceName = ServiceName.AutoArchival.GetStringValue();
                    serviceLoggingContract.JobName = JobName.AutoArchival.GetStringValue();
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
                logger.Error("An Error has occured in ScheduleAutoArchivalRequirementPackage method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }

        }

        //UAT-2603
        public static void RotationDataMovement(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling Rotation Data Movement: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                Int32 chunkSize = ConfigurationManager.AppSettings["RotationDataMovementChunkSize"].IsNotNull() ?
                                                            Convert.ToInt32(ConfigurationManager.AppSettings["RotationDataMovementChunkSize"])
                                                            : AppConsts.CHUNK_SIZE_FOR_ROTATION_DATA_MOVEMENT;

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
                    Int32 tenantId = clntDbConf.CDB_TenantID;

                    logger.Info("******************* START while loop of Rotation Data Movement method for tenant id: " + tenantId.ToString() + " *******************");

                    try
                    {
                        if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;

                            Boolean executeLoop = true;
                            while (executeLoop)
                            {
                                logger.Info("******************* START Getting RPSIds *******************");

                                String rpsIds = ClinicalRotationManager.GetRPSIdsWithDataMovementDueStatus(tenantId, chunkSize);
                                List<Int32> lstRPSID = new List<int>();


                                logger.Info("******************* END Getting RPSIds  *******************");

                                if (!rpsIds.IsNullOrEmpty())
                                {
                                    var prevData = Business.RepoManagers.ApplicantRequirementManager.GetMailDataForItemSubmitted(String.Join(",", lstRPSID), tenantId); //UAT-2905

                                    //UAT 3805
                                    List<Entity.ClientEntity.RequirementPackageSubscription> lstBeforeRequirementPackageSubscriptions = new List<Entity.ClientEntity.RequirementPackageSubscription>();
                                    if (!rpsIds.IsNullOrEmpty())
                                    {
                                        List<Int32> lstReqPackageSubscriptionIds = rpsIds.Split(',').Select(Int32.Parse).ToList();
                                        lstBeforeRequirementPackageSubscriptions = RequirementRuleManager.GetRequirementPackageSubscriptionBySubscriptionIds(tenantId, lstReqPackageSubscriptionIds);
                                    }
                                    logger.Info("*******************  Store Procedure Execution Start for Rotation Data Movement method for tenant id: " + tenantId.ToString() + " , and RPS Ids: " + rpsIds + " *******************");

                                    List<RequirementRuleData> lstRequirementRuleData = ClinicalRotationManager.PerformRotationDataMovement(tenantId, rpsIds, backgroundProcessUserId);

                                    if (!lstRequirementRuleData.IsNullOrEmpty() && lstRequirementRuleData.Count > 0)
                                    {

                                        lstRPSID = lstRequirementRuleData.Select(cond => cond.Rps_Id).Distinct().ToList();
                                        //UAT 3805
                                        List<Entity.ClientEntity.RequirementPackageSubscription> lstAfterRequirementPackageSubscriptions = new List<Entity.ClientEntity.RequirementPackageSubscription>();
                                        if (lstRequirementRuleData.IsNotNull() && lstRequirementRuleData.Count > 0)
                                        {
                                            lstAfterRequirementPackageSubscriptions = RequirementRuleManager.GetRequirementPackageSubscriptionBySubscriptionIds(tenantId, lstRPSID);
                                        }


                                        var prevpackagSubscriptionStatuses = Business.RepoManagers.ApplicantRequirementManager.GetPackageSubscriptionCategoryStatus(tenantId, String.Join(",", lstRPSID));

                                        // string lstRequirementRows = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(lstRequirementRuleData);
                                        logger.Info("*******************  Rule Execution Start for Rotation Data Movement method for tenant id: " + tenantId.ToString() + " and RPS Is " + rpsIds.ToString() + "*******************");

                                        List<RequirementRuleData> lstRuleData = lstRequirementRuleData
                                                               .DistinctBy(cond => new { cond.Rps_Id, cond.PackageId, cond.ApplicantUserID, cond.CategoryId, cond.ItemId })
                                                               .ToList();

                                        #region  UAT-3273- Get status before rule execution
                                        logger.Info("*******************  START Before rule execution- Getting Approved Rotaions for GetApprovedSubscrptionByRequirementPackageSubscriptionIDs method for tenant id: " + tenantId.ToString() + " , and  String.Join(", ", lstRPSID): " + String.Join(",", lstRPSID) + " *******************");
                                        var dataBeforeRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(tenantId, String.Join(",", lstRPSID));
                                        logger.Info("*******************  END Before rule execution- Getting Approved Rotaions for GetApprovedSubscrptionByRequirementPackageSubscriptionIDs*******************");
                                        #endregion

                                        foreach (RequirementRuleData item in lstRuleData)
                                        {
                                            List<Int32> lstReqFields = lstRequirementRuleData.Where(cond => cond.Rps_Id == item.Rps_Id
                                                                                           && cond.PackageId == item.PackageId
                                                                                           && cond.ApplicantUserID == item.ApplicantUserID
                                                                                           && cond.CategoryId == item.CategoryId
                                                                                           && cond.ItemId == item.ItemId
                                                                                           && cond.FieldId > 0)
                                                                                           .Select(cond => cond.FieldId).ToList();

                                            EvaluateRequirementDynamicBuisnessRules(item.Rps_Id, item.CategoryId, item.ItemId, backgroundProcessUserId, tenantId, lstReqFields);



                                            #region UAT-3805

                                            List<Int32> approvedCategoryIDs = new List<Int32>();
                                            Entity.ClientEntity.RequirementPackageSubscription beforeRequirementPackageSubscription = lstBeforeRequirementPackageSubscriptions.Where(con => con.RPS_ID == item.Rps_Id).FirstOrDefault();
                                            if (!beforeRequirementPackageSubscription.IsNullOrEmpty() && !beforeRequirementPackageSubscription.ApplicantRequirementCategoryDatas.IsNullOrEmpty())
                                            {
                                                foreach (var item1 in beforeRequirementPackageSubscription.ApplicantRequirementCategoryDatas)
                                                {
                                                    if (!item1.ARCD_IsDeleted)
                                                    {
                                                        if (item1.lkpRequirementCategoryStatu.RCS_Code == RequirementCategoryStatus.APPROVED.GetStringValue())
                                                        {
                                                            approvedCategoryIDs.Add(item.CategoryId);
                                                        }
                                                    }
                                                }
                                            }
                                            String approvedCategoryIds = approvedCategoryIDs.IsNullOrEmpty() ? String.Empty : String.Join(",", approvedCategoryIDs);
                                            List<Int32> affectedCategoryIds = new List<Int32>();
                                            Entity.ClientEntity.RequirementPackageSubscription afterRequirementPackageSubscription = lstAfterRequirementPackageSubscriptions.Where(con => con.RPS_ID == item.Rps_Id).FirstOrDefault();
                                            affectedCategoryIds = afterRequirementPackageSubscription.ApplicantRequirementCategoryDatas.Where(con => !con.ARCD_IsDeleted).Select(sel => sel.ARCD_RequirementCategoryID).ToList();
                                            String categoryIds = String.Join(",", affectedCategoryIds);
                                            ProfileSharingManager.SendItemDocNotificationOnCategoryApproval(tenantId, categoryIds, item.ApplicantUserID
                                                                                                                       , approvedCategoryIds, lkpUseTypeEnum.ROTATION.GetStringValue()
                                                                                                                       , null, item.Rps_Id, backgroundProcessUserId);


                                            #endregion


                                        }

                                        #region  UAT-3273- Get status after rule execution
                                        logger.Info("*******************  START After rule execution- Getting Approved Rotaions for GetApprovedSubscrptionByRequirementPackageSubscriptionIDs method for tenant id: " + tenantId.ToString() + " , and lstRPSID: " + String.Join(",", lstRPSID) + " *******************");
                                        var dataAfterRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(tenantId, String.Join(",", lstRPSID));
                                        ProfileSharingManager.CheckAndSendNotificationForNewlyApprovedRotations(dataBeforeRuleExecution, dataAfterRuleExecution, tenantId);
                                        logger.Info("*******************  END After rule execution- Getting Approved Rotaions for GetApprovedSubscrptionByRequirementPackageSubscriptionIDs*******************");
                                        #endregion

                                        //UAT-3112:-
                                        List<Int32> lstARID = lstRuleData.Select(s => s.ApplicantRequirementItemDataID).Distinct().ToList();

                                        if (!lstARID.IsNullOrEmpty())
                                        {
                                            string arid = string.Join(",", lstARID);
                                            ComplianceDataManager.SaveBadgeFormNotificationData(tenantId, null, arid, null, backgroundProcessUserId);
                                        }

                                        //UAT-2975
                                        String packageSubscriptionIDs = String.Join(",", lstRPSID);
                                        RequirementVerificationManager.SyncRequirementVerificationToFlatData(packageSubscriptionIDs, tenantId, backgroundProcessUserId);

                                        logger.Info("*******************  Rule Execution Start for Rotation Data Movement method for tenant id: " + tenantId.ToString() + " and RPS Is " + rpsIds.ToString() + "*******************");

                                        var packagSubscriptionStatuses = Business.RepoManagers.ApplicantRequirementManager.GetPackageSubscriptionCategoryStatus(tenantId, String.Join(",", lstRPSID));
                                        foreach (DataRow item in packagSubscriptionStatuses.Rows)
                                        {
                                            Boolean sendEmail = true;
                                            if (prevpackagSubscriptionStatuses.Rows.Count > 0)
                                            {
                                                var results = (from myRow in prevpackagSubscriptionStatuses.AsEnumerable()
                                                               where myRow.Field<int>("RequirementPackageSubscriptionID") == Convert.ToInt32(item["RequirementPackageSubscriptionID"])
                                                               select myRow).CopyToDataTable();

                                                if (results.Rows.Count > 0)
                                                {
                                                    if (Convert.ToString(results.Rows[0]["RequirementCategoryStatusCode"]).Equals(Convert.ToString(item["RequirementCategoryStatusCode"])))
                                                        sendEmail = false;
                                                }
                                            }
                                            if (sendEmail)
                                            {
                                                var status = String.Empty;
                                                if (Convert.ToString(item["RequirementCategoryStatusCode"]).Equals(AppConsts.REQUIREMENT_PACKAGE_SUBSCRIPTION_APPROVED_CODE))
                                                {
                                                    status = "Approved";
                                                }
                                                else if (Convert.ToString(item["RequirementCategoryStatusCode"]).Equals(AppConsts.REQUIREMENT_PACKAGE_SUBSCRIPTION_PENDING_REVIEW_CODE))
                                                {
                                                    status = "Pending Review";
                                                }

                                                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, Convert.ToString(item["ApplicantName"]));
                                                dictMailData.Add(EmailFieldConstants.ROTATION_NAME, Convert.ToString(item["RotationName"]));
                                                dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, Convert.ToString(item["PackageName"]));
                                                dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId);
                                                dictMailData.Add(EmailFieldConstants.STATUS, status);

                                                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();


                                                //UAT-3160
                                                String rotationHierarchyIDs = String.Empty;
                                                Int32 requirementPackageSubscriptionID = Convert.ToInt32(item["RequirementPackageSubscriptionID"]).IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(item["RequirementPackageSubscriptionID"]);
                                                if (requirementPackageSubscriptionID > AppConsts.NONE)
                                                {
                                                    rotationHierarchyIDs = ApplicantRequirementManager.GetRotationHierarchyIdsBasedOnSubscriptionID(tenantId, requirementPackageSubscriptionID);
                                                }
                                                #region UAT-3364 - Granular permission for Rotation Creator
                                                Boolean IsAllowed = ComplianceDataManager.CheckRotationCreatorGranularPermissionsByOrgUserIdForSendNotificationForCategoryApproved(Convert.ToInt32(item["OrganizationUserID"]));
                                                #endregion
                                                if (IsAllowed)
                                                {
                                                    mockData.UserName = Convert.ToString(item["UserName"]);
                                                    mockData.EmailID = Convert.ToString(item["Email"]);
                                                    mockData.ReceiverOrganizationUserID = Convert.ToInt32(item["OrganizationUserID"]);
                                                }
                                                else
                                                {
                                                    mockData.UserName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
                                                    mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                                                    mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                                                }
                                                //Send mail
                                                CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_ALL_ROTATION_CATEGORIES_APPROVED_OR_PENDING_REVIEW, dictMailData, mockData, tenantId, -1, null, null, true, false, null, rotationHierarchyIDs);
                                            }
                                        }

                                        //UAT-2905
                                        // lstItemIds = new List<Int32>();

                                        List<Int32> lstItemIds = lstRequirementRuleData.Select(sel => sel.ItemId).Distinct().ToList(); //UAT-2905
                                        ComplianceDataManager.SendNotificationToAdminForItemSubmitted(true, tenantId, String.Join(",", lstRPSID), String.Join(",", lstItemIds), prevData);
                                        // ComplianceDataManager.SendNotificationToAdminForItemSubmitted(true, tenantId, String.Join(",", lstRPSID));

                                        //UAT-4015
                                        if (!lstRequirementRuleData.IsNullOrEmpty())
                                        {
                                            List<Int32> lstRpsIds = lstRequirementRuleData.Select(sel => sel.Rps_Id).ToList();

                                            if (!lstRpsIds.IsNullOrEmpty())
                                            {
                                                foreach (Int32 rpsId in lstRpsIds.Distinct())
                                                {
                                                    if (!rpsId.IsNullOrEmpty() && rpsId > AppConsts.NONE)
                                                    {
                                                        ApplicantRequirementManager.SendMailToInstPrecpReqPKgCompliantStatus(tenantId, rpsId, backgroundProcessUserId);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    logger.Info("*******************  Store Procedure Execution End for Rotation Data Movement method for tenant id: " + tenantId.ToString() + " , and RPS Ids: " + rpsIds + " *******************");
                                }
                                else
                                {
                                    executeLoop = false;
                                }
                            }

                            logger.Info("******************* END while loop of Rotation Data Movement method for tenant id: " + tenantId.ToString() + " *******************");

                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.RotationDataMovement.GetStringValue();
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
                        logger.Error("An Error has occured in Rotation Data Movement method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString + " current TenantId is : " + clntDbConf.CDB_TenantID);
                        if (ex.Data.Count > 0)
                        {
                            foreach (DictionaryEntry de in ex.Data)
                            {
                                logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                            }
                        }
                        ServiceContext.ReleaseDBContextItems();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in Rotation Data Movement method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }

        }

        public static void EvaluateRequirementDynamicBuisnessRules(Int32 reqSubsID, Int32 reqCategoryId, Int32 reqItemId, Int32 currentLoggedInUserID, Int32 tenantID, List<Int32> lstReqFields)
        {
            List<RequirementRuleObjectTree> ruleObjectMappingList = new List<RequirementRuleObjectTree>();
            string ruleObjectXml = string.Empty;

            RequirementRuleObjectTree ruleObjectMappingForCategory = new RequirementRuleObjectTree
            {
                RuleObjectTypeCode = ObjectType.Compliance_Category.GetStringValue(),
                RuleObjectId = Convert.ToString(reqCategoryId),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RequirementRuleObjectTree ruleObjectMappingForItem = new RequirementRuleObjectTree
            {
                RuleObjectTypeCode = ObjectType.Compliance_Item.GetStringValue(),
                RuleObjectId = Convert.ToString(reqItemId),
                RuleObjectParentId = Convert.ToString(reqCategoryId)
            };

            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            ruleObjectMappingList.Add(ruleObjectMappingForItem);

            foreach (var item in lstReqFields)
            {
                RequirementRuleObjectTree ruleObjectMappingForAttr = new RequirementRuleObjectTree
                {
                    RuleObjectTypeCode = ObjectType.Compliance_ATR.GetStringValue(),
                    RuleObjectId = Convert.ToString(item),
                    RuleObjectParentId = Convert.ToString(reqItemId)
                };
                ruleObjectMappingList.Add(ruleObjectMappingForAttr);
            }

            List<Entity.ClientEntity.lkpObjectType> lstlkpObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantID).Where(cond => !cond.OT_IsDeleted).ToList();

            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("RuleObjects"));
            foreach (RequirementRuleObjectTree ruleObjectMapping in ruleObjectMappingList)
            {
                var lkpObjectType = lstlkpObjectType.Where(sel => sel.OT_Code == ruleObjectMapping.RuleObjectTypeCode).FirstOrDefault();
                XmlNode exp = el.AppendChild(doc.CreateElement("RuleObject"));
                exp.AppendChild(doc.CreateElement("TypeId")).InnerText = lkpObjectType.IsNotNull() ? lkpObjectType.OT_ID.ToString() : String.Empty;
                exp.AppendChild(doc.CreateElement("Id")).InnerText = ruleObjectMapping.RuleObjectId;
                exp.AppendChild(doc.CreateElement("ParentId")).InnerText = ruleObjectMapping.RuleObjectParentId;
            }

            ruleObjectXml = doc.OuterXml.ToString();

            RuleManager.EvaluateRequirementPostSubmitRules(ruleObjectXml, reqSubsID, currentLoggedInUserID, tenantID);
        }

        //UAT-2513
        public static void BatchRotationUploadThroughExcel(Int32? tenant_Id = null)
        {
            try
            {

                logger.Info("******************* Calling BatchRotationUploadThroughExcel: " + DateTime.Now.ToString() + " *******************");

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

                Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_BATCH_ROTATION_UPLOAD;

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    Int32 tenantId = clntDbConf.CDB_TenantID;

                    logger.Info("******************* START while loop of BatchRotationUploadThroughExcel method for tenant id: " + tenantId.ToString() + " *******************");

                    try
                    {
                        if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;

                            Boolean executeLoop = true;
                            while (executeLoop)
                            {
                                List<Int32> lstBatchRotationUploadIDs = new List<Int32>();

                                //logger.Info("******************* START Getting BatchRotationUploadIDs *******************");

                                lstBatchRotationUploadIDs = ClinicalRotationManager.GetBatchRotationListForTimer(tenantId, chunkSize);

                                if (!lstBatchRotationUploadIDs.IsNullOrEmpty())
                                {
                                    logger.Info("*******************  Store Procedure Execution Start for Creation of Batch Rotation Upload method for tenant id: " + tenantId.ToString() + " for Batch Rotation upload IDs: " + String.Join(",", lstBatchRotationUploadIDs) + " *******************");

                                    Boolean status = ClinicalRotationManager.CreateClinicalRotationFromBatchRotationUploadDetails(lstBatchRotationUploadIDs, tenantId, backgroundProcessUserId);

                                    if (status)
                                    {
                                        logger.Info("*******************  Store Procedure Execution End for Creation of Batch Rotation Upload method for tenant id: " + tenantId.ToString() + " for Batch Rotation upload IDs: " + String.Join(",", lstBatchRotationUploadIDs) + " *******************");
                                    }
                                    else
                                    {
                                        logger.Info("*******************  Unable to process Execution for Creation of Batch Rotation Upload method for tenant id: " + tenantId.ToString() + " for Batch Rotation upload IDs: " + String.Join(",", lstBatchRotationUploadIDs) + " *******************");
                                    }
                                }
                                else
                                {
                                    executeLoop = false;
                                }
                            }

                            logger.Info("******************* END while loop of BatchRotationUploadThroughExcel method for tenant id: " + tenantId.ToString() + " *******************");

                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.BatchRotationUploadThroughExcel.GetStringValue();
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
                        logger.Error("An Error has occured in BatchRotationUploadThroughExcel method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString + " current TenantId is : " + clntDbConf.CDB_TenantID);
                        if (ex.Data.Count > 0)
                        {
                            foreach (DictionaryEntry de in ex.Data)
                            {
                                logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                            }
                        }
                        ServiceContext.ReleaseDBContextItems();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in BatchRotationUploadThroughExcel method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        #region [UAT-3059]
        public static void UpdatedApplicantRequirementsNotification()
        {
            try
            {
                logger.Info("******************* Calling UpdatedApplicantRequirementsNotification: " + DateTime.Now.ToString() + " *******************");

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_TO_AGENCY_USER_FOR_UPDATED_APPLICANT_REQUIREMENTS.GetStringValue());

                Int32 updatedApplicantRequirementsSubEventId = !subEvent.IsNullOrEmpty() ? subEvent.CommunicationSubEventID : 0;

                logger.Info("******************* Start Getting Agency Rotation Mapping Data: " + DateTime.Now.ToString() + " *******************");

                DataSet dsAgencyRotationMapping = ProfileSharingManager.GetAgencyRotationMapping();

                logger.Info("******************* End Getting Agency Rotation Mapping Data: " + DateTime.Now.ToString() + " *******************");

                List<AgencyRotationMapping> lstAgencyRotationMapping = new List<AgencyRotationMapping>();
                List<AgencyUserInfoContract> lstAllAgencyUsers = new List<AgencyUserInfoContract>();


                if (!dsAgencyRotationMapping.IsNullOrEmpty() && dsAgencyRotationMapping.Tables.Count > 0)
                {
                    //Getting Rotation Agency Mapping
                    if (!dsAgencyRotationMapping.Tables[0].IsNullOrEmpty() && dsAgencyRotationMapping.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsAgencyRotationMapping.Tables[0].Rows)
                        {
                            AgencyRotationMapping agencyRotationMapping = new AgencyRotationMapping();
                            agencyRotationMapping.AgencyID = dr["AgencyID"].IsNullOrEmpty() ? 0 : Convert.ToInt32(dr["AgencyID"]);
                            agencyRotationMapping.AgencyName = dr["AgencyName"].IsNullOrEmpty() ? string.Empty : Convert.ToString(dr["AgencyName"]);
                            agencyRotationMapping.TenantID = dr["TenantID"].IsNullOrEmpty() ? 0 : Convert.ToInt32(dr["TenantID"]);
                            agencyRotationMapping.TenantName = dr["TenantName"].IsNullOrEmpty() ? string.Empty : Convert.ToString(dr["TenantName"]);
                            agencyRotationMapping.RotationIds = dr["RotationIds"].IsNullOrEmpty() ? string.Empty : Convert.ToString(dr["RotationIds"]);
                            lstAgencyRotationMapping.Add(agencyRotationMapping);
                        }
                    }

                    //Getting Agency Users
                    if (!dsAgencyRotationMapping.Tables[1].IsNullOrEmpty() && dsAgencyRotationMapping.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsAgencyRotationMapping.Tables[1].Rows)
                        {
                            AgencyUserInfoContract agencyUserInfoContract = new AgencyUserInfoContract();
                            agencyUserInfoContract.AgencyID = dr["AgencyID"].IsNullOrEmpty() ? 0 : Convert.ToInt32(dr["AgencyID"]);
                            agencyUserInfoContract.OrgUserID = dr["OrganizationUserID"].IsNullOrEmpty() ? 0 : Convert.ToInt32(dr["OrganizationUserID"]);
                            agencyUserInfoContract.AgencyUserID = dr["AgencyUserID"].IsNullOrEmpty() ? 0 : Convert.ToInt32(dr["AgencyUserID"]);
                            agencyUserInfoContract.FirstName = dr["FirstName"].IsNullOrEmpty() ? string.Empty : Convert.ToString(dr["FirstName"]);
                            agencyUserInfoContract.LastName = dr["LastName"].IsNullOrEmpty() ? string.Empty : Convert.ToString(dr["LastName"]);
                            agencyUserInfoContract.FullName = dr["AgencyUserName"].IsNullOrEmpty() ? string.Empty : Convert.ToString(dr["AgencyUserName"]);
                            agencyUserInfoContract.PrimaryEmailAddress = dr["PrimaryEmailAddress"].IsNullOrEmpty() ? string.Empty : Convert.ToString(dr["PrimaryEmailAddress"]);
                            lstAllAgencyUsers.Add(agencyUserInfoContract);
                        }
                    }

                }

                List<Int32> lstAgency = lstAgencyRotationMapping.Select(cond => cond.AgencyID).ToList();

                foreach (Int32 agencyID in lstAgency)
                {
                    DataTable dtUpdatedItems = new DataTable();


                    logger.Info("******************* Start Getting Updated Applicant Requirement Items for Agency ID: " + agencyID + " : " + DateTime.Now.ToString() + " *******************");

                    DateTime? lastNotificationSentDate = ProfileSharingManager.GetLastNotificationSentDate(updatedApplicantRequirementsSubEventId, agencyID);
                    DateTime fromDate;
                    DateTime toDate = DateTime.Now;

                    if (lastNotificationSentDate.HasValue)
                        fromDate = lastNotificationSentDate.Value;
                    else
                        fromDate = toDate.AddDays(-1);

                    List<AgencyRotationMapping> lstFilteredMappings = new List<AgencyRotationMapping>();
                    lstFilteredMappings = lstAgencyRotationMapping.Where(cond => cond.AgencyID == agencyID).ToList();

                    //Tenant Wise getting Data
                    foreach (AgencyRotationMapping agencyRotationMapping in lstFilteredMappings)
                    {
                        Int32 tenantID_ = agencyRotationMapping.TenantID;
                        String rotationIds = agencyRotationMapping.RotationIds;

                        logger.Info("******************* Start Getting Updated Applicant Requirement Items for Agency ID: " + agencyID + " and Tenant ID " + tenantID_ + " : " + DateTime.Now.ToString() + " *******************");

                        DataTable dtUpdatedRotationItemsOfTenant = ProfileSharingManager.GetUpdatedRotationItems(tenantID_, agencyID, rotationIds, fromDate, toDate);

                        logger.Info("******************* Start Merging Data - Updated Applicant Requirement Items for Agency ID: " + agencyID + " and Tenant ID " + tenantID_ + " : " + DateTime.Now.ToString() + " *******************");

                        foreach (DataRow row in dtUpdatedRotationItemsOfTenant.Rows)
                        {
                            if (dtUpdatedItems.Rows.Count == 0)
                                dtUpdatedItems = dtUpdatedRotationItemsOfTenant.Clone();

                            dtUpdatedItems.ImportRow(row);
                        }

                        logger.Info("******************* End Merging Data - Updated Applicant Requirement Items for Agency ID: " + agencyID + " and Tenant ID " + tenantID_ + " : " + DateTime.Now.ToString() + " *******************");

                        logger.Info("******************* End Getting Updated Applicant Requirement Items for Agency ID: " + agencyID + " and Tenant ID " + tenantID_ + " : " + DateTime.Now.ToString() + " *******************");
                    }

                    if (!dtUpdatedItems.IsNullOrEmpty()
                        && dtUpdatedItems.Rows.Count > 0)
                    {
                        List<CommunicationTemplateContract> lstAgencyUser = new List<CommunicationTemplateContract>();
                        lstAgencyUser = lstAllAgencyUsers.Where(cond => cond.AgencyID == agencyID)
                                            .Select(s => new CommunicationTemplateContract
                                            {
                                                RecieverEmailID = s.PrimaryEmailAddress,
                                                RecieverName = s.FullName,
                                                CurrentUserId = backgroundProcessUserId,
                                                ReceiverOrganizationUserId = s.OrgUserID,
                                                IsToUser = true
                                            }).ToList();

                        if (!lstAgencyUser.IsNullOrEmpty() && lstAgencyUser.Count > 0)
                        {
                            logger.Info("******************* Start Creating Excel - Updated Applicant Requirement Items for Agency ID: " + agencyID + " : " + DateTime.Now.ToString() + " *******************");

                            String agencyName = lstAgencyRotationMapping.Where(cond => cond.AgencyID == agencyID).First().AgencyName;
                            string fileName = string.Concat("Updated_Student_Requirements_", toDate.ToString("MMddyyHHmmssfff"));

                            DataView dv = dtUpdatedItems.DefaultView;
                            dv.Sort = "ApplicantLastName";
                            dtUpdatedItems = dv.ToTable();

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
                            sbHtmlUpdatedRequirements.Append("<td>Agency Name</td>");
                            sbHtmlUpdatedRequirements.Append("<td>Complio ID</td>");
                            sbHtmlUpdatedRequirements.Append("<td>Student First Name</td>");
                            sbHtmlUpdatedRequirements.Append("<td>Student Middle Name</td>");
                            sbHtmlUpdatedRequirements.Append("<td>Student Last Name</td>");
                            sbHtmlUpdatedRequirements.Append("<td>Student Institution</td>");
                            sbHtmlUpdatedRequirements.Append("<td>Item Name</td>");
                            sbHtmlUpdatedRequirements.Append("</tr>");

                            //Appending Data Rows
                            foreach (DataRow row in dtUpdatedItems.Rows)
                            {
                                sbHtmlUpdatedRequirements.Append("<tr>");
                                sbHtmlUpdatedRequirements.Append("<td>" + agencyName + "</td>");
                                sbHtmlUpdatedRequirements.Append("<td>" + Convert.ToString(row["ComplioID"]) + "</td>");
                                sbHtmlUpdatedRequirements.Append("<td>" + Convert.ToString(row["ApplicantFirstName"]) + "</td>");
                                sbHtmlUpdatedRequirements.Append("<td>" + (String.IsNullOrWhiteSpace(Convert.ToString(row["ApplicantMiddleName"])) ? "&nbsp;" : Convert.ToString(row["ApplicantMiddleName"])) + "</td>");
                                sbHtmlUpdatedRequirements.Append("<td>" + Convert.ToString(row["ApplicantLastName"]) + "</td>");
                                sbHtmlUpdatedRequirements.Append("<td>" + Convert.ToString(row["InstitutionName"]) + "</td>");
                                sbHtmlUpdatedRequirements.Append("<td>" + Convert.ToString(row["ItemNames"]) + "</td>");
                                sbHtmlUpdatedRequirements.Append("</tr>");
                            }

                            //Ending Table
                            sbHtmlUpdatedRequirements.Append("</table>");

                            sbHtmlUpdatedRequirements.Append("</body>");

                            sbHtmlUpdatedRequirements.Append("</html>");

                            //Creating Excel
                            byte[] excel = ExcelReader.GetUpdatedApplicantRequirementsByAgency(dtUpdatedItems, fileName, agencyName);
                            string savedFilePath = ReportManager.SaveUpdatedApplicantRequirements(excel, string.Concat(fileName, ".xls"));

                            SystemCommunicationAttachment sysCommAttachment = new SystemCommunicationAttachment();
                            sysCommAttachment.SCA_OriginalDocumentID = -1;
                            sysCommAttachment.SCA_OriginalDocumentName = string.Concat(fileName, ".xls");
                            sysCommAttachment.SCA_DocumentPath = savedFilePath;
                            sysCommAttachment.SCA_DocumentSize = excel.Length;
                            sysCommAttachment.SCA_DocAttachmentTypeID = GetAttachmentDocumentType(DocumentAttachmentType.DAILY_REPORT.GetStringValue());
                            sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                            sysCommAttachment.SCA_IsDeleted = false;
                            sysCommAttachment.SCA_CreatedBy = backgroundProcessUserId;
                            sysCommAttachment.SCA_CreatedOn = DateTime.Now;


                            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                            dictMailData.Add(EmailFieldConstants.AGENCY_NAME, agencyName);
                            dictMailData.Add(EmailFieldConstants.APPLICANT_UPDATED_REQUIREMENTS, sbHtmlUpdatedRequirements.ToString());

                            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                            //mockData.UserName = Convert.ToString(item["UserName"]);
                            //mockData.EmailID = Convert.ToString(item["Email"]);
                            //mockData.ReceiverOrganizationUserID = Convert.ToInt32(item["OrganizationUserID"]);

                            logger.Info("******************* End Creating Excel - Updated Applicant Requirement Items for Agency ID: " + agencyID + " : " + DateTime.Now.ToString() + " *******************");

                            logger.Info("******************* Start Sending Notification - Updated Applicant Requirement Items for Agency ID: " + agencyID + " : " + DateTime.Now.ToString() + " *******************");

                            //Send mail
                            Int32? systemCommunicationID = CommunicationManager.SendUpdatedApplicantRequirementNotification(CommunicationSubEvents.NOTIFICATION_TO_AGENCY_USER_FOR_UPDATED_APPLICANT_REQUIREMENTS, mockData, dictMailData, lstAgencyUser);

                            if (systemCommunicationID != null)
                            {
                                sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                                CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                            }

                            logger.Info("******************* End Sending Notification - Updated Applicant Requirement Items for Agency ID: " + agencyID + " : " + DateTime.Now.ToString() + " *******************");


                            logger.Info("******************* Start Updating Agency Notification - Updated Applicant Requirement Items for Agency ID: " + agencyID + " : " + DateTime.Now.ToString() + " *******************");

                            ProfileSharingManager.SaveAgencyNotification(updatedApplicantRequirementsSubEventId, "UpdatedApplicantRequirementNotification", agencyID, fromDate, toDate, backgroundProcessUserId);

                            logger.Info("******************* End Updating Agency Notification - Updated Applicant Requirement Items for Agency ID: " + agencyID + " : " + DateTime.Now.ToString() + " *******************");
                        }
                        else
                        {
                            logger.Info("******************* No Agency User Found - Updated Applicant Requirement Items for Agency ID: " + agencyID + " : " + DateTime.Now.ToString() + " *******************");
                        }
                    }
                    else
                    {
                        logger.Info("******************* Updated Applicant Requirement Items Not Found To Send Notification for Agency ID: " + agencyID + " : " + DateTime.Now.ToString() + " *******************");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in UpdatedApplicantRequirementsNotification method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        private static Int16 GetAttachmentDocumentType(String docAttachmentTypeCode)
        {
            List<lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();

            return !docAttachmentType.IsNullOrEmpty()
                    ? Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID)
                    : Convert.ToInt16(AppConsts.NONE);
        }
        #endregion

        //UAT-3112
        public static void SendBadgeFormNotifications(Int32? tenant_Id = null)
        {
            try
            {

                logger.Info("******************* Calling SendBadgeFormNotifications: " + DateTime.Now.ToString() + " *******************");

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

                Int32 chunkSize = AppConsts.CHUNK_SIZE_BADGE_FORM_NOTIFICATION;

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    Int32 tenantId = clntDbConf.CDB_TenantID;

                    logger.Info("******************* START while loop of SendBadgeFormNotifications method for tenant id: " + tenantId.ToString() + " *******************");

                    try
                    {
                        if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;

                            Boolean executeLoop = true;
                            while (executeLoop)
                            {
                                logger.Info("******************* Badge Form Notification Start for for tenant id: " + tenantId.ToString() + "*******************");


                                if (!ComplianceDataManager.SendBadgeFormNotifications(tenantId, chunkSize, backgroundProcessUserId))
                                {
                                    executeLoop = false;
                                }
                            }

                            logger.Info("******************* END while loop of SendBadgeFormNotifications method for tenant id: " + tenantId.ToString() + " *******************");

                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.SendBadgeFormNotifications.GetStringValue();
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
                        logger.Error("An Error has occured in SendBadgeFormNotifications method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString + " current TenantId is : " + clntDbConf.CDB_TenantID);
                        if (ex.Data.Count > 0)
                        {
                            foreach (DictionaryEntry de in ex.Data)
                            {
                                logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                            }
                        }
                        ServiceContext.ReleaseDBContextItems();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in SendBadgeFormNotifications method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        #region [UAT-3485]
        public static void SendMailForRequirementExpiringItems(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailForRequirementExpiringItems: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.REQUIREMENT_ITEM_ABOUT_TO_EXPIRE.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                String subEventCode = CommunicationSubEvents.REQUIREMENT_ITEM_ABOUT_TO_EXPIRE.GetStringValue();

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

                Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_MAIL_BEFORE_REQ_ITEM_EXPIRY;
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;

                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);
                        String entitySetName = "ExpiringApplicantRequirementItemData";
                        List<RequirementItemsAboutToExpireContract> lstRequirementItemsAboutToExpireContract = new List<RequirementItemsAboutToExpireContract>();
                        lstRequirementItemsAboutToExpireContract = ClinicalRotationManager.GetExpiringRequirementItems(tenantId, subEventId, chunkSize);

                        while (lstRequirementItemsAboutToExpireContract != null && lstRequirementItemsAboutToExpireContract.Count > 0)
                        {
                            logger.Info("******************* START while loop of SendMailForRequirementExpiringItems method for tenant id: " + tenantId.ToString() + " *******************");
                            foreach (RequirementItemsAboutToExpireContract expiringItem in lstRequirementItemsAboutToExpireContract)
                            {
                                logger.Trace("******************* Placing entry in Email Queue and Notification delivery for a chunk of Expiring Requirement Items:" + DateTime.Now.ToString() + " *******************");

                                //Create Dictionary
                                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(expiringItem.UserFirstName, " ", expiringItem.UserLastName));
                                dictMailData.Add(EmailFieldConstants.REQUIREMENT_ITEM_EXPIRY_DATE, expiringItem.ItemExpirationDate.HasValue ? expiringItem.ItemExpirationDate.Value.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy"));
                                dictMailData.Add(EmailFieldConstants.REQUIREMENT_ITEM_NAME, expiringItem.RequirementItemName);
                                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId.ToString());
                                dictMailData.Add(EmailFieldConstants.COMPLIO_ID, expiringItem.ComplioID.ToString()); //UAT:4619

                                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                mockData.UserName = string.Concat(expiringItem.UserFirstName, " ", expiringItem.UserLastName);
                                mockData.EmailID = expiringItem.PrimaryEmailaddress;
                                mockData.ReceiverOrganizationUserID = expiringItem.OrgUserId;


                                //Send mail
                                CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.REQUIREMENT_ITEM_ABOUT_TO_EXPIRE, dictMailData, mockData, tenantId, -1, null, null, true, false, null, expiringItem.RotationHierachyIds, 0);

                                //Send Message
                                CommunicationManager.SaveMessageContent(CommunicationSubEvents.REQUIREMENT_ITEM_ABOUT_TO_EXPIRE, dictMailData, expiringItem.OrgUserId, tenantId);

                                //Save Notification Delivery 
                                Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                notificationDelivery.ND_OrganizationUserID = expiringItem.OrgUserId;
                                notificationDelivery.ND_SubEventTypeID = subEventId;
                                notificationDelivery.ND_EntityId = expiringItem.ApplicantRequirementItemID;
                                notificationDelivery.ND_EntityName = entitySetName;
                                notificationDelivery.ND_IsDeleted = false;
                                notificationDelivery.ND_CreatedOn = DateTime.Now;
                                notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                                logger.Trace("******************* Placed entry in Email Queue and Notification delivery for a chunk of Expiring Compliance Items: " + DateTime.Now.ToString() + " *******************");
                            }
                            logger.Trace("******************* Processed a chunk of Expiring Compliance Items: " + DateTime.Now.ToString() + " *******************");
                            //expiringComplianceItems = RuleManager.GetExpiringComplianceItems(tenantId, subEventCode, subEventId, entitySetName, chunkSize);
                            lstRequirementItemsAboutToExpireContract = ClinicalRotationManager.GetExpiringRequirementItems(tenantId, subEventId, chunkSize);
                        }
                        logger.Info("******************* END while loop of SendMailForExpiringItems method for tenant id: " + tenantId.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendMailForRequirementExpiringItems.GetStringValue();
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
                logger.Error("An Error has occured in SendMailForRequirementExpiringItems method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
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

        #region [UAT-3137]
        public static void SendMailRequirementCategoriesBeforeGoingToBeRequired(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailRequirementCategoriesBeforeGoingToBeRequired: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_FOR_ROTATION_CATEGORY_GOING_TO_BE_REQUIRED.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                String subEventCode = CommunicationSubEvents.NOTIFICATION_FOR_ROTATION_CATEGORY_GOING_TO_BE_REQUIRED.GetStringValue();

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
                Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_REQUIRED_ROTATION_CATEGORY_BEFORE_GOING_TO_BE_REQUIRED;
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;

                        String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);
                        String entitySetName = "RequirementCategoriesBeforeGoingToBeRequiredData";
                        List<RequirementCategoriesBeforeGoingToBeRequiredContract> lstRequirementCategoriesBeforeGoingToBeRequiredContract = new List<RequirementCategoriesBeforeGoingToBeRequiredContract>();

                        lstRequirementCategoriesBeforeGoingToBeRequiredContract = ClinicalRotationManager.GetRequirementCategoriesBeforeGoingToBeRequired(tenantId, subEventId, chunkSize);

                        while (lstRequirementCategoriesBeforeGoingToBeRequiredContract != null && lstRequirementCategoriesBeforeGoingToBeRequiredContract.Count > 0)
                        {
                            logger.Info("******************* START while loop of SendEmailRequirementCategoriesBeforeGoingToBeRequired method for tenant id: " + tenantId.ToString() + " *******************");
                            foreach (RequirementCategoriesBeforeGoingToBeRequiredContract expiringCategory in lstRequirementCategoriesBeforeGoingToBeRequiredContract)
                            {
                                logger.Trace("******************* Placing entry in Email Queue and Notification delivery for a chunk of Categories Before Going To Be Required:" + DateTime.Now.ToString() + " *******************");

                                //Create Dictionary
                                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, string.Concat(expiringCategory.UserFirstName, " ", expiringCategory.UserLastName));
                                dictMailData.Add(EmailFieldConstants.CATEGORY_NAME, expiringCategory.RequirementCategoryName);
                                dictMailData.Add(EmailFieldConstants.COMPLIANCE_REQUIRED_DATE, expiringCategory.CategoryRequiredDate.HasValue ? expiringCategory.CategoryRequiredDate.Value.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy"));
                                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                dictMailData.Add(EmailFieldConstants.COMPLIO_ID, expiringCategory.ComplioID);
                                dictMailData.Add(EmailFieldConstants.ROTATION_NAME, expiringCategory.RotationName);

                                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                mockData.UserName = string.Concat(expiringCategory.UserFirstName, " ", expiringCategory.UserLastName);
                                mockData.EmailID = expiringCategory.PrimaryEmailaddress;
                                mockData.ReceiverOrganizationUserID = expiringCategory.OrgUserId;


                                //Send mail
                                CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_ROTATION_CATEGORY_GOING_TO_BE_REQUIRED, dictMailData, mockData, tenantId, -1, null, null, true, false, null, expiringCategory.RotationHierachyIds, 0);

                                //Send Message
                                CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_ROTATION_CATEGORY_GOING_TO_BE_REQUIRED, dictMailData, expiringCategory.OrgUserId, tenantId);

                                //Save Notification Delivery 
                                Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                notificationDelivery.ND_OrganizationUserID = expiringCategory.OrgUserId;
                                notificationDelivery.ND_SubEventTypeID = subEventId;
                                notificationDelivery.ND_EntityId = expiringCategory.ApplicantRequirementCategoryID;
                                notificationDelivery.ND_EntityName = entitySetName;
                                notificationDelivery.ND_IsDeleted = false;
                                notificationDelivery.ND_CreatedOn = DateTime.Now;
                                notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);

                                logger.Trace("******************* Placed entry in Email Queue and Notification delivery for a chunk of Categories Before Going To Be Required: " + DateTime.Now.ToString() + " *******************");
                            }
                            logger.Trace("******************* Processed a chunk of Categories Before Going To Be Required: " + DateTime.Now.ToString() + " *******************");
                            lstRequirementCategoriesBeforeGoingToBeRequiredContract = ClinicalRotationManager.GetRequirementCategoriesBeforeGoingToBeRequired(tenantId, subEventId, chunkSize);

                        }
                        logger.Info("******************* END while loop of SendEmailRequirementCategoriesBeforeGoingToBeRequired method for tenant id: " + tenantId.ToString() + " *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendEmailRequirementCategoriesBeforeGoingToBeRequired.GetStringValue();
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
                logger.Error("An Error has occured in SendEmailRequirementCategoriesBeforeGoingToBeRequired method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
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

        #region UAT 3950 Automatically archive rotations after the end date
        public static void AutomaticallyArchiveRotation(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling AutomaticallyArchiveRotation: " + DateTime.Now.ToString() + " *******************");
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
                String AllSyncReqPkgObjectIds = String.Empty;
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    Int32 tenantId = clntDbConf.CDB_TenantID;

                    logger.Info("******************* START while loop of AutomaticallyArchiveRotation method for tenant id: " + tenantId.ToString() + " *******************");

                    try
                    {
                        if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;

                            Boolean executeLoop = true;
                            while (executeLoop)
                            {
                                logger.Info("******************* START Updating Rotations as Archive*******************");

                                ClinicalRotationManager.AutomaticallyArchiveRotation(tenantId);
                                executeLoop = false;
                                logger.Info("******************* END Updating Rotations as Archive*******************");
                            }

                            logger.Info("******************* END while loop of AutomaticallyArchiveRotation method for tenant id: " + tenantId.ToString() + " *******************");

                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.AutomaticallyArchiveRotation.GetStringValue();
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
                        logger.Error("An Error has occured in AutomaticallyArchiveRotation method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString + " current TenantId is : " + clntDbConf.CDB_TenantID);
                        if (ex.Data.Count > 0)
                        {
                            foreach (DictionaryEntry de in ex.Data)
                            {
                                logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                            }
                        }
                        ServiceContext.ReleaseDBContextItems();
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in AutomaticallyArchiveRotation method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
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

        #region UAT-4657 : Requirement Package Versioning and Category Disassociation 

        public static void RequirementPackageVersioning()
        {
            try
            {
                logger.Info("******************* Calling RequirementPackageVersioning: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                logger.Info("****************** Calling Started for ManageRequirementVersionTenantMapping method:" + DateTime.Now.ToString() + "************");

                ClinicalRotationManager.ManageRequirementVersionTenantMapping(backgroundProcessUserId);

                logger.Info("****************** Calling Ended for ManageRequirementVersionTenantMapping method:" + DateTime.Now.ToString() + "************");


                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                   item =>
                   {
                       ClientDBConfiguration config = new ClientDBConfiguration();
                       config.CDB_TenantID = item.CDB_TenantID;
                       config.CDB_ConnectionString = item.CDB_ConnectionString;
                       config.Tenant = new Tenant();
                       config.Tenant.TenantName = item.Tenant.TenantName; return config;
                   }).ToList();

                List<Entity.SharedDataEntity.RequirementPkgVersionTenantMapping> requirementPkgVersionTenantMapping =
                                                            ClinicalRotationManager.GetRequirementPkgVersionTenantMapping();

                if (!requirementPkgVersionTenantMapping.IsNullOrEmpty() && requirementPkgVersionTenantMapping.Any())
                {
                    requirementPkgVersionTenantMapping.DistinctBy(cond => cond.RPVTM_TenantId).Select(col => col.RPVTM_TenantId).ForEach(tenantID =>
                    {
                        ClientDBConfiguration clntDbConf = null;
                        if (clientDbConfs != null && clientDbConfs.Count > 0 && tenantID > AppConsts.NONE)
                        {
                            clntDbConf = clientDbConfs.Where(x => x.CDB_TenantID == tenantID).FirstOrDefault();
                        }
                        try
                        {
                            if (!clntDbConf.IsNullOrEmpty() && CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                            {
                                DateTime jobStartTime = DateTime.Now;
                                DateTime jobEndTime;

                                //  List<Int32> lstRequirementPkgVersionTenantMappingIds = requirementPkgVersionTenantMapping.Where(con => con.RPVTM_TenantId == tenantID)
                                //                                                                                      .Select(sel => sel.RPVTM_ID).ToList();

                                logger.Info("******************* START RequirementPackageVersioning method for tenant id: " + tenantID.ToString() + " *******************");

                                ClinicalRotationManager.VersioningRequirementPackages(tenantID, backgroundProcessUserId);

                                logger.Info("******************* END RequirementPackageVersioning method for tenant id: " + tenantID.ToString() + " *******************");




                                //Save service logging data to DB
                                if (_isServiceLoggingEnabled)
                                {
                                    jobEndTime = DateTime.Now;
                                    ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                    serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                                    serviceLoggingContract.JobName = JobName.RequirementPackageVersioning.GetStringValue();
                                    serviceLoggingContract.TenantID = tenantID;
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
                            logger.Error("An Error has occured in RequirementPackageVersioning method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString + " current TenantId is : " + clntDbConf.CDB_TenantID);
                            if (ex.Data.Count > 0)
                            {
                                foreach (DictionaryEntry de in ex.Data)
                                {
                                    logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                                }
                            }
                            ServiceContext.ReleaseDBContextItems();
                        }
                    });
                }

                logger.Info("********************* STARTED method to udpate status in Requirement Package  *******************");

                ClinicalRotationManager.UpdateRequirementPkgVersioningStatusInRequirementPackage(backgroundProcessUserId);

                logger.Info("********************* ENDED method to udpate status in Requirement Package *******************");
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in RequirementPackageVersioning method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        public static void ProcessRequirementCategoryDisassociation()
        {

            try
            {
                logger.Info("******************* Calling ProcessRequirementCategoryDisassociation: " + DateTime.Now.ToString() + " *******************");
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

                List<Entity.SharedDataEntity.RequirementCategoryDisassociationTenantMapping> requirementCategoryDisassociationTenantMapping =
                                                         ClinicalRotationManager.GetRequirementCategoryDisassociationTenantMappingForDisassociation();

                if (!requirementCategoryDisassociationTenantMapping.IsNullOrEmpty() && requirementCategoryDisassociationTenantMapping.Any())
                {
                    foreach (Entity.SharedDataEntity.RequirementCategoryDisassociationTenantMapping reqCatDisTenantMapping in requirementCategoryDisassociationTenantMapping.OrderBy(x => x.RCDTM_ID))
                    {
                        ClientDBConfiguration clntDbConf = null;
                        if (clientDbConfs != null && clientDbConfs.Count > 0 && reqCatDisTenantMapping.RCDTM_TenantId > AppConsts.NONE)
                        {
                            clntDbConf = clientDbConfs.Where(x => x.CDB_TenantID == reqCatDisTenantMapping.RCDTM_TenantId).FirstOrDefault();
                        }

                        try
                        {
                            if (!clntDbConf.IsNullOrEmpty() && CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                            {
                                DateTime jobStartTime = DateTime.Now;
                                DateTime jobEndTime;

                                logger.Info("******************* START while loop of Requirement Category Disassociation method for  RequirementCategoryDisassociationTenantMappingID : " + reqCatDisTenantMapping.RCDTM_ID.ToString() + " *******************");

                                ClinicalRotationManager.ProcessRequirementCategoryDisassociation(reqCatDisTenantMapping.RCDTM_TenantId, reqCatDisTenantMapping.RCDTM_ID, backgroundProcessUserId);

                                logger.Info("******************* END while loop of Requirement Category Disassociation method for  RequirementCategoryDisassociationTenantMappingID : " + reqCatDisTenantMapping.RCDTM_ID.ToString() + " *******************");

                                //Save service logging data to DB
                                if (_isServiceLoggingEnabled)
                                {
                                    jobEndTime = DateTime.Now;
                                    ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                    serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                                    serviceLoggingContract.JobName = JobName.RequirementCategoryDisassociation.GetStringValue();
                                    serviceLoggingContract.TenantID = reqCatDisTenantMapping.RCDTM_TenantId;
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
                            logger.Error("An Error has occured in RequirementPackageVersioning method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}"
                                , ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString + " current TenantId is : " + clntDbConf.CDB_TenantID);
                            if (ex.Data.Count > 0)
                            {
                                foreach (DictionaryEntry de in ex.Data)
                                {
                                    logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                                }
                            }
                            ServiceContext.ReleaseDBContextItems();
                        }

                    }
                }

                logger.Info("********************* STARTED method to udpate status in RequirementCategoryDisassociation table  *******************");

                ClinicalRotationManager.UpdateRequirementCategoryDisassociationStatus(backgroundProcessUserId);

                logger.Info("********************* ENDED method to udpate status in RequirementCategoryDisassociation table *******************");
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in ProcessRequirementCategoryDisassociation method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
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
