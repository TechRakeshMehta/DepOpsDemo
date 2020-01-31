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
using INTSOF.UI.Contract.Alumni;

namespace EmailDispatcherService
{
    public class AlumniAccess
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




        public static void UpdateApplicantForAlumniAccess()
        {
            try
            {

                logger.Info("******************* Calling UpdateApplicantForAlumniAccess: " + DateTime.Now.ToString() + " *******************");

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                Int32 chunkSize = AppConsts.CHUNK_SIZE_ALUMNI_ACCESS_NOTIFICATION;

                DateTime jobEndTime;

                logger.Info("*******************  Store Procedure Execution Start for Update applicants in OrganizationUserAlumniAccess ****************************");
                //added log for the applicants in OrganizationUserAlumniAccess Table security databse whose all subscriptions are archived/graduated.

                AlumniManager.UpdateApplicantForAlumniAccess(backgroundProcessUserId);

                logger.Info("*******************  Store Procedure Execution End for A Update applicants in OrganizationUserAlumniAccess ****************************");

                DateTime jobStartTime = DateTime.Now;

                logger.Info("*******************  Store Procedure Execution Start for Process Applicant Data For Emial ****************************");

                AlumniManager.ProcessApplicantDataForEmial(chunkSize, backgroundProcessUserId);

                logger.Info("*******************  Store Procedure Execution End for Process Applicant Data For Emial ****************************");

                //Save service logging data to DB
                if (_isServiceLoggingEnabled)
                {
                    jobEndTime = DateTime.Now;
                    ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                    serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                    serviceLoggingContract.JobName = JobName.UpdateApplicantForAlumniAccess.GetStringValue();
                    //serviceLoggingContract.TenantID = tenantId;
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
                logger.Error("An Error has occured in UpdateApplicantForAlumniAccess method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        public static void CopyDataFromComplianceToCompliance()
        {
            try
            {

                logger.Info("******************* Calling CopyDataFromComplianceToCompliance: " + DateTime.Now.ToString() + " *******************");

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


                Int32 chunkSize = AppConsts.CHUNK_SIZE_COPY_COMPLIANCE_TO_COMPLIANCE;

                DateTime jobEndTime;

                logger.Info("*******************  Store Procedure Execution Start for Copy Data From Compliance To Compliance ****************************");
                //added log for the applicants in OrganizationUserAlumniAccess Table security databse whose all subscriptions are archived/graduated.

                List<AlumniPackageSubscription> lstTarSubscriptions = AlumniManager.CopyComplianceToCompliance(backgroundProcessUserId, chunkSize);

                logger.Info("*******************  Store Procedure Execution End for Copy Data From Compliance To Compliance ****************************");

                DateTime jobStartTime = DateTime.Now;

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    Int32 tenantId = clntDbConf.CDB_TenantID;
                    if (!lstTarSubscriptions.IsNullOrEmpty())
                    {
                        List<Int32> targetIdsForDataMoved = lstTarSubscriptions.Where(item => item.TarTenantID == tenantId).Select(sel => sel.TarPackageSubscriptionID).ToList();

                        List<AlumniPackageSubscription> lstTarPkgSubscription = lstTarSubscriptions.Where(item => item.TarTenantID == tenantId).ToList();
                        if (!lstTarPkgSubscription.IsNullOrEmpty())
                            AlumniManager.HandleAssignments(lstTarPkgSubscription, tenantId, backgroundProcessUserId);
                        if (!targetIdsForDataMoved.IsNullOrEmpty())
                            RuleManager.ExecuteBusinessRules(targetIdsForDataMoved, tenantId, backgroundProcessUserId);

                        //Email to users that your data movement is complete.
                        List<AlumniPackageSubscription> lstTarSubscriptionsForEmail = lstTarSubscriptions.Where(item => item.TarTenantID == tenantId).ToList();
                        AlumniManager.SendEmailForDataMovementComplete(lstTarSubscriptionsForEmail);

                    }
                }
                //Save service logging data to DB
                if (_isServiceLoggingEnabled)
                {
                    jobEndTime = DateTime.Now;
                    ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                    serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                    serviceLoggingContract.JobName = JobName.CopyComplianceToCompliance.GetStringValue();
                    //serviceLoggingContract.TenantID = tenantId;
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
                logger.Error("An Error has occured in CopyComplianceToCompliance method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
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
