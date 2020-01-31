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
using System.Data;
using System.Linq;

namespace EmailDispatcherService
{
    public class CategoryComplianceRqd
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;
        public static void ProcessCategoryComplianceRqd(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling ProcessCategoryComplianceRqd: " + DateTime.Now.ToString() + " *******************");

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
                        Int32 loopCount = 1;
                        Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_CATEGORY_COMPLIANCE_REQUIRED;
                        List<Entity.ClientEntity.CompliancePackageCategory> lstCategoriesForAction = ComplianceSetupManager.GetCategoriesRqdForComplianceAction(tenantId);
                        List<Entity.ClientEntity.CompliancePackageCategory> chunkOfCategoriesForAction;
                        chunkOfCategoriesForAction = lstCategoriesForAction.Take(chunkSize).ToList();
                        while (chunkOfCategoriesForAction.Count > AppConsts.NONE)
                        {

                            #region UAT-3805
                            //List<Entity.ClientEntity.CompliancePackageCategory> lstOptionalCategories = chunkOfCategoriesForAction.Where(x => !x.CPC_ComplianceRequired).ToList();
                            List<Entity.ClientEntity.CompliancePackageCategory> lstOptionalCategories = chunkOfCategoriesForAction.DistinctBy(x => x.CPC_ID).ToList();

                            List<Entity.ClientEntity.PackageSubscription> lstPkgSubscriptionBeforeUpdate = new List<Entity.ClientEntity.PackageSubscription>();
                            List<Entity.ClientEntity.PackageSubscription> lstPkgSubscriptionAfterUpdate = new List<Entity.ClientEntity.PackageSubscription>();
                            Dictionary<Int32, String> dicApprovedCatOfSub = new Dictionary<Int32, String>();
                            if (!lstOptionalCategories.IsNullOrEmpty())
                            {
                                lstPkgSubscriptionBeforeUpdate = ProfileSharingManager.GetCompliancePkgSubscriptionData(tenantId, lstOptionalCategories);
                                lstPkgSubscriptionBeforeUpdate.DistinctBy(dst => dst.PackageSubscriptionID).ForEach(pkgSub =>
                                {
                                    List<Int32> lstApprovedCaegoryIDs = new List<Int32>();
                                    if (!pkgSub.ApplicantComplianceCategoryDatas.IsNullOrEmpty())
                                    {
                                        lstApprovedCaegoryIDs = pkgSub.ApplicantComplianceCategoryDatas.Where(cnd => (cnd.lkpCategoryComplianceStatu.Code ==
                                                                                                                  ApplicantCategoryComplianceStatus.Approved.GetStringValue()
                                                                                                                  ||
                                                                                                                  cnd.lkpCategoryComplianceStatu.Code ==
                                                                                                                  ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue()
                                                                                                                   )
                                                                                                                   && !cnd.IsDeleted
                                                                                                                  ).Select(slct => slct.ComplianceCategoryID).ToList();
                                    }

                                    if (!dicApprovedCatOfSub.ContainsKey(pkgSub.PackageSubscriptionID))
                                    {
                                        dicApprovedCatOfSub.Add(pkgSub.PackageSubscriptionID, lstApprovedCaegoryIDs.IsNullOrEmpty() ? String.Empty : String.Join(",", lstApprovedCaegoryIDs));

                                    }

                                });
                            }
                            #endregion

                            logger.Trace("******************* Started processing records for a chunk of action items: " + DateTime.Now.ToString() + " *******************");
                            foreach (var categoryForAction in chunkOfCategoriesForAction)
                            {
                                if (categoryForAction.CPC_ComplianceRequired)
                                {
                                    ComplianceSetupManager.ProcessRequiredCategory(backgroundProcessUserId, tenantId, categoryForAction);
                                }
                                else
                                {
                                    ComplianceSetupManager.ProcessOptionalCategory(backgroundProcessUserId, tenantId, categoryForAction);
                                }
                            }

                            #region UAT-3805
                            if (!lstOptionalCategories.IsNullOrEmpty())
                            {
                                lstPkgSubscriptionAfterUpdate = ProfileSharingManager.GetCompliancePkgSubscriptionData(tenantId, lstOptionalCategories);
                            }

                            lstPkgSubscriptionAfterUpdate.DistinctBy(dst => dst.PackageSubscriptionID).ForEach(pkgSub =>
                            {
                                var currentPkgSubBeforeUpdate = lstPkgSubscriptionBeforeUpdate.FirstOrDefault(x => x.PackageSubscriptionID == pkgSub.PackageSubscriptionID);

                                //List<Int32> lstApprovedCaegoryIDs = new List<Int32>();
                                //if (!currentPkgSubBeforeUpdate.IsNullOrEmpty())
                                //{
                                //    lstApprovedCaegoryIDs = currentPkgSubBeforeUpdate.ApplicantComplianceCategoryDatas.Where(cnd => (cnd.lkpCategoryComplianceStatu.Code ==
                                //                                                                              ApplicantCategoryComplianceStatus.Approved.GetStringValue()
                                //                                                                              ||
                                //                                                                              cnd.lkpCategoryComplianceStatu.Code ==
                                //                                                                              ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue()
                                //                                                                               )
                                //                                                                               && !cnd.IsDeleted
                                //                                                                              ).Select(slct => slct.ComplianceCategoryID).ToList();
                                //}

                                //String approvedCategories = lstApprovedCaegoryIDs.IsNullOrEmpty() ? String.Empty : String.Join(",", lstApprovedCaegoryIDs);
                                String approvedCategories = !dicApprovedCatOfSub.ContainsKey(pkgSub.PackageSubscriptionID) ? String.Empty : dicApprovedCatOfSub.GetValue(pkgSub.PackageSubscriptionID);
                                List<Int32> lstCategoryIds = pkgSub.ApplicantComplianceCategoryDatas.Where(x => !x.IsDeleted).Select(sl => sl.ComplianceCategoryID).ToList();
                                if (!lstCategoryIds.IsNullOrEmpty())
                                {
                                    String categoryIds = String.Join(",", lstCategoryIds);

                                    ProfileSharingManager.SendItemDocNotificationOnCategoryApproval(tenantId, categoryIds, pkgSub.OrganizationUserID.Value, approvedCategories
                                                                                                    , lkpUseTypeEnum.COMPLIANCE.GetStringValue(), pkgSub.PackageSubscriptionID
                                                                                                    , null, backgroundProcessUserId);
                                }

                            });
                            #endregion

                            logger.Trace("******************* Ended processing records for a chunk of action items::" + DateTime.Now.ToString() + " *******************");
                            chunkOfCategoriesForAction = lstCategoriesForAction.Skip(loopCount * chunkSize).Take((loopCount + 1) * chunkSize).ToList();
                            loopCount++;
                            ServiceContext.ReleaseDBContextItems();
                        }
                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.ProcessCategoryComplianceRqd.GetStringValue();
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
                logger.Error("An Error has occured in ProcessCategoryComplianceRqd method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }
        }

        #region UAT-1217:Notification to correspond with UAT-1209
        /// <summary>
        /// Send mail to all applicant for categories that are going to be non compliance from compliance by compliance required category action.
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        public static void NotificationUpcomingNonComplianceRequiredCategoryAction(Int32? tenant_Id = null)
        {
            try
            {
                //ServiceContext.init();
                logger.Info("******************* Calling NotificationNonComplianceRequiredCategoryAction: " + DateTime.Now.ToString() + " *******************");

                Entity.lkpCommunicationSubEvent subEvent = CommunicationManager.GetCommunicationTypeSubEvents(CommunicationSubEvents.NOTIFICATION_UPCOMING_CATEGORIES_COMPLIANCE_REQUIRED.GetStringValue());
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

                Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_MAIL_BEFORE_ITEM_EXPIRY;
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    try
                    {
                        if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;
                            Int32 tenantId = clntDbConf.CDB_TenantID;
                            //String tenantName = clntDbConf.Tenant.TenantName;

                            String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                            String entitySetName = "ApplicantNonComplianceRequiredCategoryAction";

                            Int32 remBfrExp = 30;
                            //ComplianceDataManager.GetSubscriptionNotificationBeforeExpiryDays(tenantId, Setting.Reminder_Before_Expiry.GetStringValue());
                            Int32 remExpFrq = 15;
                            //ComplianceDataManager.GetSubscriptionNotificationFrequencyDays(tenantId, Setting.Reminder_Expiry_Frequency.GetStringValue());

                            Boolean executeLoop = true;
                            while (executeLoop)
                            {
                                logger.Info("******************* START while loop of NotificationNonComplianceRequiredCategoryAction method for tenant id: " + tenantId.ToString() + " *******************");

                                List<Entity.ClientEntity.GetUpcomingNonComplianceCategories> complianceRequiredCategoryList = new List<Entity.ClientEntity.GetUpcomingNonComplianceCategories>();
                                complianceRequiredCategoryList = RuleManager.GetNonComplianceRequiredCategoryActionData(tenantId, remBfrExp, remExpFrq, chunkSize);

                                if (complianceRequiredCategoryList != null && complianceRequiredCategoryList.Count > 0)
                                {
                                    foreach (Entity.ClientEntity.GetUpcomingNonComplianceCategories complianceRequiredCategory in complianceRequiredCategoryList)
                                    {
                                        logger.Trace("******************* Placing entry in Email Queue and Notification delivery for a chunk of non-Compliance required categories:" + DateTime.Now.ToString() + " *******************");
                                        if (complianceRequiredCategory != null)
                                        {
                                            //Create Dictionary
                                            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                            dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, complianceRequiredCategory.UserFullName);
                                            dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, complianceRequiredCategory.PackageName);
                                            dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, applicationUrl);
                                            dictMailData.Add(EmailFieldConstants.CATEGORY_NAME, complianceRequiredCategory.CategoryName);
                                            dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, complianceRequiredCategory.NodeHierarchy);
                                            dictMailData.Add(EmailFieldConstants.DUE_DATE, complianceRequiredCategory.DueDate.ToString("MM-dd-yyyy"));
                                            dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId);//UAT-3079
                                            //UAT-3658
                                            dictMailData.Add(EmailFieldConstants.COMPLIANCE_CATEGORY_ID, complianceRequiredCategory.ComplianceCategoryID);

                                            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                            mockData.UserName = complianceRequiredCategory.UserFullName;
                                            mockData.EmailID = complianceRequiredCategory.PrimaryEmailAddress;
                                            mockData.ReceiverOrganizationUserID = complianceRequiredCategory.ApplicantID;

                                            //Send mail
                                            CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_UPCOMING_CATEGORIES_COMPLIANCE_REQUIRED,
                                                                                              dictMailData, mockData, tenantId, complianceRequiredCategory.HierarchyNodeID);

                                            //Send Message
                                            CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_UPCOMING_CATEGORIES_COMPLIANCE_REQUIRED, dictMailData,
                                                                                    complianceRequiredCategory.ApplicantID, tenantId);

                                            //Save Notification Delivery
                                            Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                                            notificationDelivery.ND_OrganizationUserID = complianceRequiredCategory.ApplicantID;
                                            notificationDelivery.ND_SubEventTypeID = subEventId;
                                            notificationDelivery.ND_EntityId = complianceRequiredCategory.ApplicantComplianceCategoryID;
                                            notificationDelivery.ND_EntityName = entitySetName;
                                            notificationDelivery.ND_IsDeleted = false;
                                            notificationDelivery.ND_CreatedOn = DateTime.Now;
                                            notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                                            ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);
                                        }
                                    }
                                    logger.Trace("******************* Processed a chunk of Non Compliance Required categories: " + DateTime.Now.ToString() + " *******************");
                                    ServiceContext.ReleaseDBContextItems();
                                    executeLoop = true;
                                }
                                else
                                {
                                    executeLoop = false;
                                }
                            }
                            logger.Info("******************* END while loop of NotificationNonComplianceRequiredCategoryAction method for tenant id: " + tenantId.ToString() + " *******************");

                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.NotificationUpcomingNonComplianceRequiredCategoryAction.GetStringValue();
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
                        logger.Error("An Error has occured in NotificationNonComplianceRequiredCategoryAction method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString + " current TenantId is : " + clntDbConf.CDB_TenantID);
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
                logger.Error("An Error has occured in NotificationNonComplianceRequiredCategoryAction method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
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

        public static void ProcessRotationCategoryComplianceRqd(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling ProcessRotationCategoryComplianceRqd: " + DateTime.Now.ToString() + " *******************");

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
                        Int32 loopCount = 1;
                        Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_CATEGORY_COMPLIANCE_REQUIRED;
                        List<Entity.ClientEntity.RequirementPackageCategory> lstCategoriesForAction = RequirementPackageManager.GetRequirementCategoriesRqdForComplianceAction(tenantId);
                        List<Entity.ClientEntity.RequirementPackageCategory> chunkOfCategoriesForAction;
                        chunkOfCategoriesForAction = lstCategoriesForAction.Take(chunkSize).ToList();
                        List<Int32> lstRPSID = new List<int>();
                        List<Int32> lstRPSIdsForAppCatMail = new List<Int32>();
                        while (chunkOfCategoriesForAction.Count > AppConsts.NONE)
                        {
                            #region  UAT-3273- Get status before rule execution
                            List<Int32> lstRequirementPackageIds = chunkOfCategoriesForAction.Select(sel => sel.RPC_RequirementPackageID).Distinct().ToList();
                            var dataBeforeRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageIDs(tenantId, String.Join(",", lstRequirementPackageIds));
                            #endregion

                            #region UAT-3805
                            //List<Entity.ClientEntity.RequirementPackageCategory> lstOptionalCategories = chunkOfCategoriesForAction.Where(x => !x.RPC_ComplianceRequired).ToList();
                            List<Entity.ClientEntity.RequirementPackageCategory> lstOptionalCategories = chunkOfCategoriesForAction.DistinctBy(c => c.RPC_ID).ToList();

                            List<Entity.ClientEntity.RequirementPackageSubscription> lstPkgSubscriptionBeforeUpdate = new List<Entity.ClientEntity.RequirementPackageSubscription>();
                            List<Entity.ClientEntity.RequirementPackageSubscription> lstPkgSubscriptionAfterUpdate = new List<Entity.ClientEntity.RequirementPackageSubscription>();
                            Dictionary<Int32, String> dicApprovedCatOfSub = new Dictionary<Int32, String>();
                            if (!lstOptionalCategories.IsNullOrEmpty())
                            {
                                lstPkgSubscriptionBeforeUpdate = ProfileSharingManager.GetRequirementPkgSubscriptionData(tenantId, lstOptionalCategories);
                                lstPkgSubscriptionBeforeUpdate.DistinctBy(dst => dst.RPS_ID).ForEach(pkgSub =>
                                   {
                                       List<Int32> lstApprovedCaegoryIDs = new List<Int32>();
                                       if (!pkgSub.ApplicantRequirementCategoryDatas.IsNullOrEmpty())
                                       {
                                           lstApprovedCaegoryIDs = pkgSub.ApplicantRequirementCategoryDatas.Where(cnd => cnd.lkpRequirementCategoryStatu.RCS_Code ==
                                                                                                                                      RequirementCategoryStatus.APPROVED.GetStringValue()
                                                                                                                                      && !cnd.ARCD_IsDeleted
                                                                                                                               ).Select(slct => slct.ARCD_RequirementCategoryID).ToList();
                                       }
                                       if (!dicApprovedCatOfSub.ContainsKey(pkgSub.RPS_ID))
                                       {
                                           dicApprovedCatOfSub.Add(pkgSub.RPS_ID, lstApprovedCaegoryIDs.IsNullOrEmpty() ? String.Empty : String.Join(",", lstApprovedCaegoryIDs));
                                       }
                                   });

                            }
                            #endregion

                            logger.Trace("******************* Started processing records for a chunk of ProcessRotationCategoryComplianceRqd: " + DateTime.Now.ToString() + " *******************");
                            foreach (var categoryForAction in chunkOfCategoriesForAction)
                            {
                                var RPSIDs = RequirementPackageManager.ProcessRotcomplianceRqdChange(backgroundProcessUserId, tenantId, categoryForAction);
                                lstRPSIdsForAppCatMail.AddRange(RPSIDs.Where(cond => cond.Value).Select(sel => sel.Key).ToList());
                                lstRPSID.AddRange(RPSIDs.Select(sel => sel.Key).ToList());
                            }

                            #region UAT-3805
                            if (!lstOptionalCategories.IsNullOrEmpty())
                            {
                                lstPkgSubscriptionAfterUpdate = ProfileSharingManager.GetRequirementPkgSubscriptionData(tenantId, lstOptionalCategories);
                            }

                            lstPkgSubscriptionAfterUpdate.DistinctBy(dst => dst.RPS_ID).ForEach(pkgSub =>
                            {
                                var currentPkgSubBeforeUpdate = lstPkgSubscriptionBeforeUpdate.FirstOrDefault(x => x.RPS_ID == pkgSub.RPS_ID);

                                //List<Int32> lstApprovedCaegoryIDs = new List<Int32>();
                                //if (!currentPkgSubBeforeUpdate.IsNullOrEmpty())
                                //{
                                //    lstApprovedCaegoryIDs = currentPkgSubBeforeUpdate.ApplicantRequirementCategoryDatas.Where(cnd => cnd.lkpRequirementCategoryStatu.RCS_Code ==
                                //                                                                                               RequirementCategoryStatus.APPROVED.GetStringValue()
                                //                                                                                               && !cnd.ARCD_IsDeleted
                                //                                                                                        ).Select(slct => slct.ARCD_RequirementCategoryID).ToList();
                                //}

                                //String approvedCategories = lstApprovedCaegoryIDs.IsNullOrEmpty() ? String.Empty : String.Join(",", lstApprovedCaegoryIDs);
                                String approvedCategories = !dicApprovedCatOfSub.ContainsKey(pkgSub.RPS_ID) ? String.Empty : dicApprovedCatOfSub.GetValue(pkgSub.RPS_ID);
                                List<Int32> lstCategoryIds = pkgSub.ApplicantRequirementCategoryDatas.Where(x => !x.ARCD_IsDeleted).Select(sl => sl.ARCD_RequirementCategoryID).ToList();
                                if (!lstCategoryIds.IsNullOrEmpty())
                                {
                                    String categoryIds = String.Join(",", lstCategoryIds);

                                    ProfileSharingManager.SendItemDocNotificationOnCategoryApproval(tenantId, categoryIds, pkgSub.RPS_ApplicantOrgUserID, approvedCategories
                                                                                                    , lkpUseTypeEnum.ROTATION.GetStringValue(), pkgSub.RPS_ID
                                                                                                    , null, backgroundProcessUserId);
                                }

                            });
                            #endregion


                            logger.Trace("******************* Ended processing records for a chunk of ProcessRotationCategoryComplianceRqd:" + DateTime.Now.ToString() + " *******************");
                            chunkOfCategoriesForAction = lstCategoriesForAction.Skip(loopCount * chunkSize).Take((loopCount + 1) * chunkSize).ToList();
                            loopCount++;

                            #region  UAT-3273- Get status after rule execution
                            var dataAfterRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(tenantId, String.Join(",", lstRPSID.Distinct()));
                            ProfileSharingManager.CheckAndSendNotificationForNewlyApprovedRotations(dataBeforeRuleExecution, dataAfterRuleExecution, tenantId);
                            #endregion

                            //UAT-4015
                            if (!lstRPSID.IsNullOrEmpty())
                            {
                                foreach (Int32 rpsID in lstRPSID.Distinct())
                                {
                                    if (!rpsID.IsNullOrEmpty() && rpsID > AppConsts.NONE)
                                    {
                                        ApplicantRequirementManager.SendMailToInstPrecpReqPKgCompliantStatus(tenantId, rpsID, backgroundProcessUserId);
                                    }
                                }
                            }

                            #region UAT-3990

                            String RPIDs = String.Join(",", lstRPSIdsForAppCatMail.Distinct());
                            var packagSubscriptionStatuses = Business.RepoManagers.ApplicantRequirementManager.GetPackageSubscriptionCategoryStatus(tenantId, RPIDs);
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

                            #endregion

                            ServiceContext.ReleaseDBContextItems();
                        }

                        

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.ProcessRotationCategoryComplianceRqd.GetStringValue();
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
                logger.Error("An Error has occured in ProcessRotationCategoryComplianceRqd method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
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
