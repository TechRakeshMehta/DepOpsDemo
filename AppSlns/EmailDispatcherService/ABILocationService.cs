using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using Entity;
using Business.RepoManagers;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.UI.Contract.Services;
using System.Collections;
using INTSOF.ServiceUtil;

namespace EmailDispatcherService
{
    public class ABILocationService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                    Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;

        #region UAT-3734

        public static void SendMailForOffTimeRevokedAppointmentEmail(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailForOffTimeRevokedAppointmentEmail: " + DateTime.Now.ToString() + " *******************");

                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_TO_APPLICANT_FOR_TEMPORARY_CLOSURE_OF_FINGERPRINT_SITE.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                String subEventCode = CommunicationSubEvents.NOTIFICATION_TO_APPLICANT_FOR_TEMPORARY_CLOSURE_OF_FINGERPRINT_SITE.GetStringValue();

                Entity.AppConfiguration appConfigurations = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfigurations.IsNotNull() ? Convert.ToInt32(appConfigurations.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(ConfigurationManager.AppSettings["LocationServiceTenantIds"]);
                String LocationTenantIds = appConfiguration.IsNotNull() ? appConfiguration.AC_Value : String.Empty;
                List<Int32> LocationTenantIdList = new List<Int32>();
                LocationTenantIdList = LocationTenantIds.IsNullOrEmpty() ? new List<Int32>() : LocationTenantIds.Split(',').Select(Int32.Parse).ToList();
                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Where(x => LocationTenantIdList.Contains(x.CDB_TenantID)).Select(
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

                Int32 chunkSize = Convert.ToInt32(ConfigurationManager.AppSettings["ChunkSizeForOffTimeRevokedAppointmentEmail"]);

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        //Int32 tenantId = clntDbConf.CDB_TenantID;
                        //String tenantName = clntDbConf.Tenant.TenantName;
                        List<AppointmentOrderScheduleContract> OffTimeRevokedAppointmentList = new List<AppointmentOrderScheduleContract>();
                        OffTimeRevokedAppointmentList = FingerPrintSetUpManager.GetApplicantsOffTimeRevokedAppointments(LocationTenantIds, chunkSize);
                        while (OffTimeRevokedAppointmentList != null && OffTimeRevokedAppointmentList.Count > AppConsts.NONE)
                        {
                            logger.Info("******************* START while loop of SendMailForOffTimeRevokedAppointments method *******************");
                            foreach (var OffTimeRevokedAppointment in OffTimeRevokedAppointmentList)
                            {

                                //---------------Add Entry to mail and notification delivery table
                                logger.Trace("******************* Placing entry in Email Queue and Notification delivery for a chunk of Off Time Revoked Appointment for Tenant ID:" + OffTimeRevokedAppointment.TenantID.ToString() + " : " + DateTime.Now.ToString() + " *******************");
                                String applicationUrl = WebSiteManager.GetInstitutionUrl(Convert.ToInt32(OffTimeRevokedAppointment.TenantID));
                                var tenant = SecurityManager.GetTenant(Convert.ToInt32(OffTimeRevokedAppointment.TenantID));
                                String institutionUrl = applicationUrl.IsNullOrEmpty() ? String.Empty : applicationUrl;
                                String institutionName = tenant.IsNull() || tenant.TenantName.IsNullOrEmpty() ? String.Empty : tenant.TenantName;
                                if (!(institutionUrl.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || institutionUrl.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
                                {
                                    institutionUrl = string.Concat("http://", institutionUrl.Trim());
                                }

                                var FormattedAddress = OffTimeRevokedAppointment.LocationAddress.Replace(" ", "+");
                                var LocationLink = ConfigurationManager.AppSettings["DrivingDirectionUrl"];
                                ////Create Dictionary for Mail And Message Data
                                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, string.Concat(OffTimeRevokedAppointment.FirstName, " ", OffTimeRevokedAppointment.LastName));
                                dictMailData.Add(EmailFieldConstants.Order_Number, OffTimeRevokedAppointment.OrderNumber);
                                dictMailData.Add(EmailFieldConstants.AGENCY_NAME, institutionName);
                                dictMailData.Add(EmailFieldConstants.FROM_DATE, OffTimeRevokedAppointment.StartDateTime.HasValue ? OffTimeRevokedAppointment.StartDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null);
                                dictMailData.Add(EmailFieldConstants.END_DATE, OffTimeRevokedAppointment.EndDateTime.HasValue ? OffTimeRevokedAppointment.EndDateTime.Value.ToString("MM/dd/yyyy hh:mm tt"): null);
                                dictMailData.Add(EmailFieldConstants.ITEM_NAME, OffTimeRevokedAppointment.LocationName);
                                dictMailData.Add(EmailFieldConstants.INSTITUTE_ADDRESS, OffTimeRevokedAppointment.LocationAddress);
                                dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, OffTimeRevokedAppointment.PackageName);
                                dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, institutionUrl);
                                dictMailData.Add(EmailFieldConstants.LOCATION_DESCRIPTION, OffTimeRevokedAppointment.LocationDescription);
                                dictMailData.Add(EmailFieldConstants.LOCATION_LINK, LocationLink);
                                dictMailData.Add(EmailFieldConstants.TENANT_ID, OffTimeRevokedAppointment.TenantID);
                                dictMailData.Add(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID, Convert.ToString(OffTimeRevokedAppointment.ApplicantOrgUserId));
                                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                mockData.UserName = string.Concat(OffTimeRevokedAppointment.FirstName, " ", OffTimeRevokedAppointment.LastName);
                                mockData.EmailID = OffTimeRevokedAppointment.ApplicantEmail;
                                mockData.ReceiverOrganizationUserID = OffTimeRevokedAppointment.ApplicantOrgUserId > AppConsts.NONE ? OffTimeRevokedAppointment.ApplicantOrgUserId : OffTimeRevokedAppointment.ApplicantOrgUserId;
                                var CommSubEvnt = CommunicationSubEvents.NOTIFICATION_TO_APPLICANT_FOR_TEMPORARY_CLOSURE_OF_FINGERPRINT_SITE;

                                //// send mail/message notification
                                CommunicationManager.SentMailMessageNotification(CommSubEvnt, mockData, dictMailData, OffTimeRevokedAppointment.ApplicantOrgUserId, Convert.ToInt32(OffTimeRevokedAppointment.TenantID), OffTimeRevokedAppointment.HeirarchyNodeId);
                                logger.Trace("******************* Placed entry in Email Queue and Notification delivery for a chunk of Off Time Revoked Appointment for Tenant ID:" + OffTimeRevokedAppointment.TenantID.ToString() + " : " + DateTime.Now.ToString() + " *******************");

                                //// Update appointment Status to Off Time Revoked and Notified
                                if (FingerPrintSetUpManager.UpdateAppointmentStatus(OffTimeRevokedAppointment, FingerPrintAppointmentStatus.REVOKED_AND_NOTIFIED.GetStringValue(), backgroundProcessUserId,Convert.ToInt32(OffTimeRevokedAppointment.TenantID)))
                                {
                                    logger.Trace("******************* Successfully updated status in FingerPrint Applicant Appointment for Notification of Off Time Revoked Appointment for Tenant ID:" + OffTimeRevokedAppointment.TenantID.ToString() + " : " + DateTime.Now.ToString() + " *******************");
                                }
                                else
                                    logger.Trace("******************* Failed to update status in FingerPrint Applicant Appointment for Notification of OFF Time Revoked Appointment for Tenant ID:" + OffTimeRevokedAppointment.TenantID.ToString() + " : " + DateTime.Now.ToString() + " *******************");


                            }
                            logger.Trace("******************* Processed a chunk of Off Time Revoked Appointments: " + DateTime.Now.ToString() + " *******************");
                            OffTimeRevokedAppointmentList = FingerPrintSetUpManager.GetApplicantsOffTimeRevokedAppointments(LocationTenantIds, chunkSize);
                        }
                        logger.Info("******************* END while loop of SendMailForOffTimeRevokedAppointmentEmail method *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendOffTimeRevokedMail.GetStringValue();
                            serviceLoggingContract.TenantID = clntDbConf.CDB_TenantID;
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
                logger.Error("An Error has occured in SendMailForOffTimeRevokedAppointmentEmail method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
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




        #region Missed Appointment

        public static void SendMailForMissedAppointmentEmail(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendMailForMissedAppointmentEmail: " + DateTime.Now.ToString() + " *******************");
                lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_FOR_MISSED_FINGERPRINT_APPOINTMENT.GetStringValue());
                Int32 subEventId = subEvent.IsNotNull() ? subEvent.CommunicationSubEventID : 0;
                String subEventCode = CommunicationSubEvents.NOTIFICATION_FOR_MISSED_FINGERPRINT_APPOINTMENT.GetStringValue();
                Entity.AppConfiguration appConfigurations = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfigurations.IsNotNull() ? Convert.ToInt32(appConfigurations.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(ConfigurationManager.AppSettings["LocationServiceTenantIds"]);
                String LocationTenantIds = appConfiguration.IsNotNull() ? appConfiguration.AC_Value : String.Empty;
                List<Int32> LocationTenantIdList = new List<Int32>();
                LocationTenantIdList = LocationTenantIds.IsNullOrEmpty() ? new List<Int32>() : LocationTenantIds.Split(',').Select(Int32.Parse).ToList();
                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Where(x => LocationTenantIdList.Contains(x.CDB_TenantID)).Select(
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
                Int32 chunkSize = Convert.ToInt32(ConfigurationManager.AppSettings["ChunkSizeForMissedAppointmentEmail"]);

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        //Int32 tenantId = clntDbConf.CDB_TenantID;
                        //String tenantName = clntDbConf.Tenant.TenantName;
                        List<AppointmentOrderScheduleContract> MissedAppointmentList = new List<AppointmentOrderScheduleContract>();
                        MissedAppointmentList = FingerPrintSetUpManager.GetApplicantsMissedAppointments(LocationTenantIds, chunkSize, false);
                        while (MissedAppointmentList != null && MissedAppointmentList.Count > AppConsts.NONE)
                        {
                            logger.Info("******************* START while loop of SendMailForMissedAppointments method *******************");
                            foreach (var MissedAppointment in MissedAppointmentList)
                            {
                                //---------------Add Entry to mail and notification delivery table
                                logger.Trace("******************* Placing entry in Email Queue and Notification delivery for a chunk of Missed Appointment for Tenant ID:" + MissedAppointment.TenantID.ToString() + " : " + DateTime.Now.ToString() + " *******************");
                                String applicationUrl = WebSiteManager.GetInstitutionUrl(Convert.ToInt32(MissedAppointment.TenantID));
                                var tenant = SecurityManager.GetTenant(Convert.ToInt32(MissedAppointment.TenantID));
                                String institutionUrl = applicationUrl.IsNullOrEmpty() ? String.Empty : applicationUrl;
                                String institutionName = tenant.IsNull() || tenant.TenantName.IsNullOrEmpty() ? String.Empty : tenant.TenantName;
                                if (!(institutionUrl.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || institutionUrl.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
                                {
                                    institutionUrl = string.Concat("http://", institutionUrl.Trim());
                                }

                                var FormattedAddress = MissedAppointment.LocationAddress.Replace(" ", "+");

                                ////Create Dictionary for Mail And Message Data
                                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, string.Concat(MissedAppointment.FirstName, " ", MissedAppointment.LastName));
                                dictMailData.Add(EmailFieldConstants.Order_Number, MissedAppointment.OrderNumber);
                                dictMailData.Add(EmailFieldConstants.AGENCY_NAME, institutionName);
                                dictMailData.Add(EmailFieldConstants.FROM_DATE, MissedAppointment.StartDateTime.HasValue ? MissedAppointment.StartDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null);
                                dictMailData.Add(EmailFieldConstants.END_DATE, MissedAppointment.EndDateTime.HasValue ? MissedAppointment.EndDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") :null);
                                dictMailData.Add(EmailFieldConstants.ITEM_NAME, MissedAppointment.LocationName);
                                dictMailData.Add(EmailFieldConstants.INSTITUTE_ADDRESS, MissedAppointment.LocationAddress);
                                dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, MissedAppointment.PackageName);
                                dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, institutionUrl);
                                dictMailData.Add(EmailFieldConstants.TENANT_ID, MissedAppointment.TenantID);
                                dictMailData.Add(EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID, Convert.ToString(MissedAppointment.ApplicantOrgUserId));
                                dictMailData.Add(EmailFieldConstants.LOCATION_DESCRIPTION, MissedAppointment.LocationDescription);
                                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                mockData.UserName = string.Concat(MissedAppointment.FirstName, " ", MissedAppointment.LastName);
                                mockData.EmailID = MissedAppointment.ApplicantEmail;
                                mockData.ReceiverOrganizationUserID = MissedAppointment.ApplicantOrgUserId > AppConsts.NONE ? MissedAppointment.ApplicantOrgUserId : MissedAppointment.ApplicantOrgUserId;
                                var CommSubEvnt = CommunicationSubEvents.NOTIFICATION_FOR_MISSED_FINGERPRINT_APPOINTMENT;

                                //// send mail/message notification
                                CommunicationManager.SentMailMessageNotification(CommSubEvnt, mockData, dictMailData, MissedAppointment.ApplicantOrgUserId, Convert.ToInt32(MissedAppointment.TenantID), MissedAppointment.HeirarchyNodeId);
                                logger.Trace("******************* Placed entry in Email Queue and Notification delivery for a chunk of Missed Appointment for Tenant ID:" + MissedAppointment.TenantID.ToString() + " : " + DateTime.Now.ToString() + " *******************");

                                //// Update appointment Status to missed and Notified
                                if (FingerPrintSetUpManager.UpdateAppointmentStatus(MissedAppointment, FingerPrintAppointmentStatus.MISSED_AND_NOTIFIED.GetStringValue(), backgroundProcessUserId,Convert.ToInt32(MissedAppointment.TenantID)))
                                {
                                    logger.Trace("******************* Successfully updated status in FingerPrint Applicant Appointment for Notification of Missed Appointment for Tenant ID:" + MissedAppointment.TenantID.ToString() + " : " + DateTime.Now.ToString() + " *******************");
                                }
                                else
                                    logger.Trace("******************* Failed to update status in FingerPrint Applicant Appointment for Notification of Missed Appointment for Tenant ID:" + MissedAppointment.TenantID.ToString() + " : " + DateTime.Now.ToString() + " *******************");

                            }
                            logger.Trace("******************* Processed a chunk of Missed Appointments: " + DateTime.Now.ToString() + " *******************");
                            MissedAppointmentList = FingerPrintSetUpManager.GetApplicantsMissedAppointments(LocationTenantIds, chunkSize, false);
                        }
                        logger.Info("******************* END while loop of SendMailForMissedAppointments method *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.SendMissedAppointmentMail.GetStringValue();
                            serviceLoggingContract.TenantID = clntDbConf.CDB_TenantID;
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
                logger.Error("An Error has occured in SendMailForMissedAppointmentEmail method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        public static void UpdateStatusForMissedAppointments(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling UpdateStatusForMissedAppointments: " + DateTime.Now.ToString() + " *******************");

                Entity.AppConfiguration appConfigurations = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfigurations.IsNotNull() ? Convert.ToInt32(appConfigurations.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(ConfigurationManager.AppSettings["LocationServiceTenantIds"]);
                String LocationTenantIds = appConfiguration.IsNotNull() ? appConfiguration.AC_Value : String.Empty;
                List<Int32> LocationTenantIdList = new List<Int32>();
                LocationTenantIdList = LocationTenantIds.IsNullOrEmpty() ? new List<Int32>() : LocationTenantIds.Split(',').Select(Int32.Parse).ToList();
                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Where(x => LocationTenantIdList.Contains(x.CDB_TenantID)).Select(
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
                Int32 chunkSize = Convert.ToInt32(ConfigurationManager.AppSettings["ChunkSizeForMissedAppointments"]);

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        //Int32 tenantId = clntDbConf.CDB_TenantID;
                        //String tenantName = clntDbConf.Tenant.TenantName;
                        List<AppointmentOrderScheduleContract> MissedAppointmentList = new List<AppointmentOrderScheduleContract>();
                        MissedAppointmentList = FingerPrintSetUpManager.GetApplicantsMissedAppointments(LocationTenantIds, chunkSize, true);
                        while (MissedAppointmentList != null && MissedAppointmentList.Count > AppConsts.NONE)
                        {
                            logger.Info("******************* START while loop of UpdateStatusForMissedAppointments method *******************");
                            foreach (var MissedAppointment in MissedAppointmentList)
                            {

                                //// upadate appointment Status to Missed
                                if (FingerPrintSetUpManager.UpdateAppointmentStatus(MissedAppointment, FingerPrintAppointmentStatus.MISSED.GetStringValue(), backgroundProcessUserId,Convert.ToInt32(MissedAppointment.TenantID)))
                                {
                                    logger.Trace("******************* Successfully updated status in FingerPrint Applicant Appointment for Missed Appointment for Tenant ID:" + MissedAppointment.TenantID.ToString() + " : " + DateTime.Now.ToString() + " *******************");
                                }
                                else
                                    logger.Trace("******************* Failed to update status in FingerPrint Applicant Appointment for Missed Appointment for Tenant ID:" + MissedAppointment.TenantID.ToString() + " : " + DateTime.Now.ToString() + " *******************");
                            }
                            logger.Trace("******************* Processed a chunk of Missed Appointments: " + DateTime.Now.ToString() + " *******************");
                            MissedAppointmentList = FingerPrintSetUpManager.GetApplicantsMissedAppointments(LocationTenantIds, chunkSize, true);
                        }
                        logger.Info("******************* END while loop of UpdateStatusForMissedAppointments method *******************");

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.UpdateAppointmentStatusToMissed.GetStringValue();
                            serviceLoggingContract.TenantID = clntDbConf.CDB_TenantID;
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
                logger.Error("An Error has occured in UpdateStatusForMissedAppointments method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
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
