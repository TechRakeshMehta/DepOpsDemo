using Business.RepoManagers;
using Entity;
using Entity.ClientEntity;
using INTSOF.ServiceUtil;
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
    public class RequirementScheduleAction
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;

        public static void ProcessRequirementScheduleActionExecuteCategoryRules(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling ProcessRequirementScheduleActionExecuteCategoryRules: " + DateTime.Now.ToString() + " *******************");

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
                        List<RequirementScheduledAction> actionToBeExecuted = new List<RequirementScheduledAction>();
                        Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_REQUIREMNT_CATEGORY_RULE_EXECUTION;
                        actionToBeExecuted = RequirementRuleManager.GetActiveScheduleActionList(chunkSize, ReqScheduleAction.EXECUTE_CATEGORY_RULES.GetStringValue(), tenantId);
                        while (actionToBeExecuted.Count > AppConsts.NONE)
                        {
                            logger.Trace("******************* Started executeing requirement schedule action of Requirement category for chunk of action items: " + DateTime.Now.ToString() + " *******************");
                            RequirementRuleManager.ExecuteRequirementCategoryScheduleAction(backgroundProcessUserId, AppConsts.CHUNK_SIZE_FOR_CATEGORY_RULE_REOCCUR, tenantId, ReqScheduleAction.EXECUTE_CATEGORY_RULES.GetStringValue());
                            logger.Trace("******************* Ended executeing requirement schedule action of Requirement category for chunk of action items::" + DateTime.Now.ToString() + " *******************");

                            ServiceContext.ReleaseDBContextItems();
                            actionToBeExecuted = RequirementRuleManager.GetActiveScheduleActionList(chunkSize, ReqScheduleAction.EXECUTE_CATEGORY_RULES.GetStringValue(), tenantId);
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.RequirementScheduleActionExecuteCategoryRules.GetStringValue();
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
                logger.Error("An Error has occured in ProcessRequirementScheduleActionExecuteCategoryRules method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        public static void ProcessRequirementScheduleActionExecutePackageRules(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling ProcessRequirementScheduleActionExecutePackageRules: " + DateTime.Now.ToString() + " *******************");

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
                        List<RequirementScheduledAction> actionToBeExecuted = new List<RequirementScheduledAction>();
                        Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_REQUIREMNT_PACKAGE_RULE_EXECUTION;
                        actionToBeExecuted = RequirementRuleManager.GetActiveScheduleActionList(chunkSize, ReqScheduleAction.EXECUTE_PACKAGE_RULES.GetStringValue(), tenantId);
                        while (actionToBeExecuted.Count > AppConsts.NONE)
                        {
                            logger.Trace("******************* Started executeing requirement schedule action of Requirement packages for  chunk of action items: " + DateTime.Now.ToString() + " *******************");
                            RequirementRuleManager.ExecuteRequirementPackageScheduleAction(backgroundProcessUserId, AppConsts.CHUNK_SIZE_FOR_CATEGORY_RULE_REOCCUR, tenantId);
                            logger.Trace("******************* Ended executeing requirement schedule action of Requirement packages for chunk of action items::" + DateTime.Now.ToString() + " *******************");

                            ServiceContext.ReleaseDBContextItems();
                            actionToBeExecuted = RequirementRuleManager.GetActiveScheduleActionList(chunkSize, ReqScheduleAction.EXECUTE_PACKAGE_RULES.GetStringValue(), tenantId);
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.RequirementScheduleActionExecutePackageRules.GetStringValue();
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
                logger.Error("An Error has occured in ProcessRequirementScheduleActionExecutePackageRules method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        public static void ProcessRequirementScheduleActionAfterDataSync(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling ProcessRequirementScheduleActionAfterDataSync: " + DateTime.Now.ToString() + " *******************");

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
                        List<RequirementScheduledAction> actionToBeExecuted = new List<RequirementScheduledAction>();
                        Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_REQUIREMNT_PACKAGE_RULE_EXECUTION;
                        actionToBeExecuted = RequirementRuleManager.GetActiveScheduleActionList(chunkSize, ReqScheduleAction.EXECUTE_RULES_FOR_SYNC.GetStringValue(), tenantId);
                        while (actionToBeExecuted.Count > AppConsts.NONE)
                        {
                            logger.Trace("******************* Started executeing requirement schedule action of data sync for  chunk of action items: " + DateTime.Now.ToString() + " *******************");
                            RequirementRuleManager.ExecuteRequirementCategoryScheduleAction(tenantId, AppConsts.CHUNK_SIZE_FOR_CATEGORY_RULE_REOCCUR, tenantId, ReqScheduleAction.EXECUTE_RULES_FOR_SYNC.GetStringValue());
                            logger.Trace("******************* Ended executeing requirement schedule action of data sync for chunk of action items:" + DateTime.Now.ToString() + " *******************");

                            ServiceContext.ReleaseDBContextItems();
                            actionToBeExecuted = RequirementRuleManager.GetActiveScheduleActionList(chunkSize, ReqScheduleAction.EXECUTE_RULES_FOR_SYNC.GetStringValue(), tenantId);
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.RequirementScheduleActionDataSyncRules.GetStringValue();
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
                logger.Error("An Error has occured in ProcessRequirementScheduleActionAfterDataSync method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        /// <summary>
        /// UAT 3080
        /// </summary>
        /// <param name="tenant_Id"></param>
        public static void ProcessRequirementScheduleCategoryComplianceRules(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling ProcessRequirementScheduleActionExecuteScheduledCategoryrules: " + DateTime.Now.ToString() + " *******************");

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
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Info("*******************  Started executing requirement scheduled Category Compliance rule for tenant id:" + tenantId.ToString() + " *******************");
                            executeLoop = RequirementRuleManager.ExecuteRequirementScheduledCategoryComplianceRules(backgroundProcessUserId, AppConsts.CHUNK_SIZE_FOR_CATEGORY_RULE_REOCCUR, tenantId);
                            logger.Info("*******************  Ended executing requirement scheduled Category Compliance rule for tenant id:" + tenantId.ToString() + " *******************");
                            ServiceContext.ReleaseDBContextItems();
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.RequirementScheduleActionExecuteCategoryRules.GetStringValue();
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
                logger.Error("An Error has occured in ProcessRequirementScheduleActionExecuteScheduledCategoryrules method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
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
