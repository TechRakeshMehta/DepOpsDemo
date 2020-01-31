using Business.RepoManagers;
using Entity;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.Services;
using INTSOF.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace EmailDispatcherService
{
    public class MobilityInstance
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;

        /// <summary>
        /// Creates Mobility Instance For Institute Hierarchy Mobility
        /// </summary>
        public static void CreateMobilityInstance(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling CreateMobilityInstance: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                // Get All Tenant
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
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Trace("******************* Started placing entry in Mobility Instance for Institute Hierarchy Mobility: " + DateTime.Now.ToString() + " *******************");
                            executeLoop = MobilityManager.CreateMobilityInstance(tenantId, backgroundProcessUserId, AppConsts.CHUNK_SIZE_FOR_CREATE_MOBILITY_INSTANCE);
                            logger.Trace("******************* Ended placing entry in Mobility Instance for Institute Hierarchy Mobility:" + DateTime.Now.ToString() + " *******************");
                            ServiceContext.ReleaseDBContextItems();
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.CreateMobilityInstance.GetStringValue();
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
                logger.Error("An Error has occured in CreateMobilityInstance method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        /// <summary>
        /// Creates Node Transitions for Mobility Instance Prior to expire
        /// </summary>
        public static void InsertNodeTransition(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling InsertNodeTransition: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                // Get All Tenant
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
                        Int32 mobilityTransitionLeadDays = ComplianceDataManager.GetMobilityTansitionLeadDays(tenantId, Setting.MOBILITY_TRANSITION_LEAD_DAYS.GetStringValue());
                        Boolean executeLoop = true;
                        while (executeLoop)
                        {
                            logger.Trace("******************* Started placing entry in Node Transition for Mobility Instance: " + DateTime.Now.ToString() + " *******************");
                            executeLoop = MobilityManager.InsertNodeTranistionQueue(tenantId, backgroundProcessUserId, mobilityTransitionLeadDays); //AppConsts.DAYS_DUE_BEFORE_TRANSITION
                            logger.Trace("******************* Ended placing entry in Node Transition for Mobility Instance:" + DateTime.Now.ToString() + " *******************");
                            ServiceContext.ReleaseDBContextItems();
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.InsertNodeTransition.GetStringValue();
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
                logger.Error("An Error has occured in InsertNodeTransition method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        /// <summary>
        /// Automatic Movement of Node transitions for which auto approval is true
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static void AutomaticNodeTransitionMovement(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling AutomaticNodeTransitionMovement: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                // Get All Tenant
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
                        Boolean isAutoApproval = ComplianceDataManager.GetAutoApprovalTransition(tenantId, Setting.AUTO_APPROVAL_TRANSITION.GetStringValue());
                        if (isAutoApproval)
                        {
                            String tenantName = clntDbConf.Tenant.TenantName;
                            String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                            Boolean executeLoop = true;
                            while (executeLoop)
                            {
                                List<Entity.ClientEntity.ApplicantsNodeTransitions> lstAppNodeTransitions = MobilityManager.GetApplicantsNodeTransitionsDue(tenantId, AppConsts.CHUNK_SIZE_FOR_AUOMATIC_MOVEMENT_NODE_TRANSITIONS);
                                if (lstAppNodeTransitions != null && lstAppNodeTransitions.Count > 0)
                                {
                                    String nodeTransitionXML = AutomaticNodeTransitionXML(lstAppNodeTransitions, tenantId, backgroundProcessUserId);
                                    List<Entity.ClientEntity.AutomaticChangedSubscriptions> lstAutomaticChangedSubscriptions = MobilityManager.AutomaticChangeSubscription(tenantId, nodeTransitionXML);
                                    if (lstAutomaticChangedSubscriptions != null && lstAutomaticChangedSubscriptions.Count > 0)
                                    {
                                        foreach (Entity.ClientEntity.AutomaticChangedSubscriptions changedSubscription in lstAutomaticChangedSubscriptions)
                                        {
                                            Entity.ClientEntity.usp_SubscriptionChange_Result changeSubscription = new Entity.ClientEntity.usp_SubscriptionChange_Result();
                                            changeSubscription.DuePayment = Convert.ToInt32(changedSubscription.DuePayment);
                                            changeSubscription.FirstName = changedSubscription.FirstName;
                                            changeSubscription.LastName = changedSubscription.LastName;
                                            changeSubscription.NewOrderId = 0;
                                            changeSubscription.NewOrderStatusId = 0;
                                            changeSubscription.NewSubscriptionId = 0;
                                            changeSubscription.OranizationUserId = changedSubscription.OrganizationUserId;
                                            changeSubscription.PrimaryEmailAddress = changedSubscription.PrimaryEmailAddress;
                                            changeSubscription.SourceSubscriptionId = 0;

                                            MobilityManager.sendMailForChangeSubscription(tenantId, changeSubscription, applicationUrl, tenantName, changedSubscription.ChangedHierarchyNode, changedSubscription.HierarchyNodeID);
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
                                serviceLoggingContract.JobName = JobName.AutomaticNodeTransitionMovement.GetStringValue();
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
                logger.Error("An Error has occured in AutomaticNodeTransitionMovement method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        static String AutomaticNodeTransitionXML(List<Entity.ClientEntity.ApplicantsNodeTransitions> lstAppNodeTransitions, Int32 tenantId, Int32 backgroundProcessUserId)
        {
            string str = "<root>";
            foreach (Entity.ClientEntity.ApplicantsNodeTransitions obj in lstAppNodeTransitions)
            {
                str += "<ApplicantChangeSubscription>";
                str += "<OrganizationUserID>" + obj.OrganizationUserID.ToString() + "</OrganizationUserID>";
                str += "<SourceNodeID>" + obj.SourceNodeID.ToString() + "</SourceNodeID>";
                str += "<SourcePackageID>" + obj.SourcePackageID.ToString() + "</SourcePackageID>";
                str += "<SourceSubscriptionID>" + obj.SourceSubscriptionID.ToString() + "</SourceSubscriptionID>";
                str += "<TargetNodeID>" + obj.TargetNodeID.ToString() + "</TargetNodeID>";
                str += "<TargetPackageID>" + obj.TargetPackageID.ToString() + "</TargetPackageID>";
                str += "<TargetMobilityInstanceID>" + obj.TargetMobilityInstanceID.ToString() + "</TargetMobilityInstanceID>";
                str += "<TargetDurationDays>" + obj.TargetDurationDays.ToString() + "</TargetDurationDays>";
                str += "<TenantID>" + tenantId.ToString() + "</TenantID>";
                str += "<CurrentLoggedInUserID>" + backgroundProcessUserId.ToString() + "</CurrentLoggedInUserID>";
                str += "</ApplicantChangeSubscription>";
            }
            str += "</root>";

            return str;
        }
    }
}
