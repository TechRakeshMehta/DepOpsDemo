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

namespace EmailDispatcherService
{
    public class ComplianceAuditDataSynchronise
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;
        public static void SyncApplicantDataAuditData(Int32? tenant_Id = null)
        {
            //[SS]:Added for logging purpose in exception.
            Int32 currentTenantId = 0;
            try
            {
                //ServiceContext.init();
                logger.Info("******************* Calling SyncApplicantDataAuditData: " + DateTime.Now.ToString() + " *******************");

                Int32 chunkSize = ConfigurationManager.AppSettings["SyncApplicantDataAuditRecordChunkSize"].IsNotNull() ?
                                                            Convert.ToInt32(ConfigurationManager.AppSettings["SyncApplicantDataAuditRecordChunkSize"]) : 10000;

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

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = currentTenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;
                        Int32? lastSyncedRecordID = null;
                        //UAT-4161
                        Boolean executeLoop = true;
                        Int32 lastRecordToBeSync = AppConsts.NONE;

                        logger.Info("*******************START Getting GetLastSynchedAuditDataRecord for tenant id: " + tenantId.ToString() + " *******************");

                        lastRecordToBeSync = ComplianceDataManager.GetLastRecordToBeSyncAuditData(tenantId);
                        
                        logger.Info("*******************START While Loop for tenant id: " + tenantId.ToString() + " *******************");
                        while (executeLoop)
                        {
                            var lastsyncronisedRecord = ComplianceDataManager.GetLastSynchedAuditDataRecord(tenantId);
                            if (!lastsyncronisedRecord.IsNullOrEmpty())
                            {
                                lastSyncedRecordID = lastsyncronisedRecord.ADASH_LastRecordID;
                            }

                            //UAT-4161
                            List<ApplicantDataAudit> lstDataAuditRecordToBeSync = ComplianceDataManager.GetApplicantDataAuditRecords(tenantId,chunkSize
                                                                                              , lastSyncedRecordID.IsNullOrEmpty() ? AppConsts.NONE : lastSyncedRecordID.Value);

                            if (lstDataAuditRecordToBeSync.IsNullOrEmpty()
                                || !lstDataAuditRecordToBeSync.Any(an => an.ADA_ApplicantDataAuditID < lastRecordToBeSync)
                                || (lstDataAuditRecordToBeSync.Any(an => an.ADA_ApplicantDataAuditID < lastRecordToBeSync)
                                    && lstDataAuditRecordToBeSync.Any(an => an.ADA_ApplicantDataAuditID > lastRecordToBeSync)
                                    )
                                )
                            {
                                executeLoop = false;
                            }

                            logger.Info("*******************END Getting GetLastSynchedAuditDataRecord for tenant id: " + tenantId.ToString() + " *******************");

                            logger.Info("*******************START Calling SyncApplicantAuditDataForTenant for tenant id: " + tenantId.ToString() + " *******************");

                            try
                            {
                                ComplianceDataManager.SyncApplicantAuditDataForTenant(tenantId, lastSyncedRecordID, backgroundProcessUserId, chunkSize);
                            }
                            catch (Exception ex)
                            {
                                executeLoop = false;
                                logger.Error("An Error has occured in SyncApplicantDataAuditData method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}, SyncApplicantDataAuditData TenantID: {3}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString, Convert.ToString(tenantId));
                            }
                         
                            logger.Info("*******************END Calling SyncApplicantAuditDataForTenant for tenant id: " + tenantId.ToString() + " *******************");
                        }
                        logger.Info("*******************END While Loop for tenant id: " + tenantId.ToString() + " *******************");
                        //Release DB Context after each Tenant data Synchronised.
                        ServiceContext.ReleaseDBContextItems();

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.ComplianceAuditDataSynchronise.GetStringValue();
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
                logger.Error("An Error has occured in SyncApplicantDataAuditData method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}, SyncApplicantDataAuditData TenantID: {3}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString, Convert.ToString(currentTenantId));
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }


        public static void SyncReconcillationQueueData(Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                logger.Info("******************* Calling SyncReconcillationQueueData: " + DateTime.Now.ToString() + " *******************");

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

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        String tenantName = clntDbConf.Tenant.TenantName;

                        logger.Info("*******************START Getting SyncReconcillationQueue for tenant id: " + tenantId.ToString() + " *******************");

                        ComplianceDataManager.SyncReconcillationQueueData(tenantId, backgroundProcessUserId);

                        logger.Info("*******************END Getting SyncReconcillationQueue for tenant id: " + tenantId.ToString() + " *******************");

                        //Release DB Context after each Tenant data Synchronised.
                        ServiceContext.ReleaseDBContextItems();

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.ReconcillationQueueSync.GetStringValue();
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
                logger.Error("An Error has occured in ReconcillationQueueSync method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
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
