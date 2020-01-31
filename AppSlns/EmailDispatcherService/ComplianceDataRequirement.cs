using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using Business.RepoManagers;
using Entity;
using System.Collections;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.Services;
using System.Data;
using INTSOF.UI.Contract.RotationPackages;

namespace EmailDispatcherService
{
    public static class ComplianceDataRequirement
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;
        public static void CopyComplianceDataToRequirement(Int32? tenant_Id = null)
        {
            try
            {
                Int32 trackingItmDataObjTypeID = 0;
                Int32 trackingSubsDataObjTypeID = 0;
                Int32 rotSubsObjTypeID = 0;

                logger.Info("******************* Calling CopyComplianceDataToRequirement: " + DateTime.Now.ToString() + " *******************");
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

                Int32 chunkSize = ConfigurationManager.AppSettings["ComplianceDataToRequirement_ChunkSize"].IsNotNull() ?
                                                           Convert.ToInt32(ConfigurationManager.AppSettings["ComplianceDataToRequirement_ChunkSize"])
                                                           : AppConsts.CHUNK_SIZE_FOR_COPY_COMPLIANCE_DATA_TO_REQUIREMENT;

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    Int32 tenantId = clntDbConf.CDB_TenantID;

                    logger.Info("******************* START while loop of CopyComplianceDataToRequirement method for tenant id: " + tenantId.ToString() + " *******************");


                    logger.Info("******************* START Getting lkpObjects*******************");

                    List<Entity.ClientEntity.lkpObjectType> lstObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(tenantId)
                                                               .Where(cnd => !cnd.OT_IsDeleted).ToList();

                    trackingItmDataObjTypeID = lstObjectType.Where(cond => cond.OT_Code == "ITMDAT").SingleOrDefault().OT_ID;
                    trackingSubsDataObjTypeID = lstObjectType.Where(cond => cond.OT_Code == "SUBS").SingleOrDefault().OT_ID;
                    rotSubsObjTypeID = lstObjectType.Where(cond => cond.OT_Code == "RQSUBS").SingleOrDefault().OT_ID;

                    logger.Info("******************* END Getting lkpObjects*******************");


                    try
                    {
                        if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;

                            Boolean executeLoop = true;
                            while (executeLoop)
                            {
                                List<Entity.ClientEntity.CopyDataQueue> lstCopyDataQueue = new List<Entity.ClientEntity.CopyDataQueue>();

                                logger.Info("******************* START Getting GetDataForCopyToRequirement*******************");

                                lstCopyDataQueue = ComplianceSetupManager.GetDataForCopyToRequirement(tenantId, trackingItmDataObjTypeID, trackingSubsDataObjTypeID, rotSubsObjTypeID, chunkSize);

                                logger.Info("******************* END Getting GetDataForCopyToRequirement*******************");

                                if (!lstCopyDataQueue.IsNullOrEmpty())
                                {
                                    string itemDataIds = string.Empty;
                                    string RPSIds = string.Empty;

                                    var lstAppliacntComplianceItemIds = lstCopyDataQueue.Where(cond => cond.CDQ_SourceObjectTypeID == trackingItmDataObjTypeID && cond.CDQ_TargetObjectTypeID == rotSubsObjTypeID && !cond.CDQ_TargetObjectID.HasValue && cond.CDQ_SourceObjectID.HasValue).Select(s => s.CDQ_SourceObjectID);
                                    var lstRPSIds = lstCopyDataQueue.Where(cond => cond.CDQ_SourceObjectTypeID == trackingSubsDataObjTypeID && cond.CDQ_TargetObjectTypeID == rotSubsObjTypeID && cond.CDQ_TargetObjectID.HasValue && !cond.CDQ_SourceObjectID.HasValue).Select(s => s.CDQ_TargetObjectID);

                                    if (!lstAppliacntComplianceItemIds.IsNullOrEmpty())
                                        itemDataIds = string.Join(",", lstAppliacntComplianceItemIds);

                                    if (!lstRPSIds.IsNullOrEmpty())
                                        RPSIds = string.Join(",", lstRPSIds);

                                    //var prevData = Business.RepoManagers.ApplicantRequirementManager.GetMailDataForItemSubmitted(RPSIds, tenantId); //UAT-2905

                                    logger.Info("*******************  Store Procedure Execution Start for CopyComplianceDataToRequirement method for tenant id: " + tenantId.ToString() + " , and ItemDataids: " + itemDataIds + " *******************");
                                    //UAT 3805
                                    List<Entity.ClientEntity.RequirementPackageSubscription> lstBeforeRequirementPackageSubscriptions = new List<Entity.ClientEntity.RequirementPackageSubscription>();
                                    Dictionary<Int32, String> dicBeforeRPS = new Dictionary<Int32, String>();

                                    if (!lstRPSIds.IsNullOrEmpty())
                                    {
                                        List<Int32> lstReqPackageSubscriptionIds = RPSIds.Split(',').Select(Int32.Parse).ToList();
                                        lstBeforeRequirementPackageSubscriptions = RequirementRuleManager.GetRequirementPackageSubscriptionBySubscriptionIds(tenantId, lstReqPackageSubscriptionIds);
                                        foreach (Entity.ClientEntity.RequirementPackageSubscription item in lstBeforeRequirementPackageSubscriptions)
                                        {
                                            String alreadyApprovedCategoryIds = String.Empty;
                                            String categoryApproveStatusCode = RequirementCategoryStatus.APPROVED.GetStringValue();
                                            List<Int32> lstApprovedCategories = item.ApplicantRequirementCategoryDatas.Where(cond => !cond.ARCD_IsDeleted && cond.lkpRequirementCategoryStatu.RCS_Code == categoryApproveStatusCode).Select(sel => sel.ARCD_RequirementCategoryID).Distinct().ToList();
                                            if (!lstApprovedCategories.IsNullOrEmpty())
                                            {
                                                alreadyApprovedCategoryIds = String.Join(",", lstApprovedCategories);
                                            }
                                            dicBeforeRPS.Add(item.RPS_ID, alreadyApprovedCategoryIds);
                                        }

                                    }
                                    if (!lstAppliacntComplianceItemIds.IsNullOrEmpty())
                                    {
                                        List<Int32> lstApplicantItemDataIdd = itemDataIds.Split(',').Select(Int32.Parse).ToList();

                                        List<Entity.ClientEntity.RequirementPackageSubscription> lstRPSForAppItemDatIds = RequirementRuleManager.GetRequirementPackageSubscriptionByApplicantComplianceItemIds(tenantId, lstApplicantItemDataIdd);

                                        lstBeforeRequirementPackageSubscriptions.AddRange(lstRPSForAppItemDatIds);
                                    }

                                    var lstRequirementRuleData = ComplianceSetupManager.CopyComplianceDataToRequirement(tenantId, backgroundProcessUserId, itemDataIds, RPSIds);

                                    string rpsIds_RuleData = string.Join(",", lstRequirementRuleData.Select(cond => cond.Rps_Id).ToList());

                                    var prevpackagSubscriptionStatuses = Business.RepoManagers.ApplicantRequirementManager.GetPackageSubscriptionCategoryStatus(tenantId, rpsIds_RuleData);

                                    logger.Info("*******************  Store Procedure Execution End for CopyComplianceDataToRequirement method for tenant id: " + tenantId.ToString() + " , and ItemDataids: " + itemDataIds + " *******************");

                                    if (!lstRequirementRuleData.IsNullOrEmpty())
                                    {

                                        #region  UAT-3273- Get status before rule execution
                                        logger.Info("*******************  START Before rule execution- Getting Approved Rotaions for GetApprovedSubscrptionByRequirementPackageSubscriptionIDs method for tenant id: " + tenantId.ToString() + " , and rpsIds_RuleData: " + rpsIds_RuleData + " *******************");
                                        var dataBeforeRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(tenantId, rpsIds_RuleData);
                                        logger.Info("*******************  END Before rule execution- Getting Approved Rotaions for GetApprovedSubscrptionByRequirementPackageSubscriptionIDs*******************");
                                        #endregion

                                        string lstRequirementRows = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(lstRequirementRuleData);
                                        logger.Info("*******************  Rule Execution Start for CopyComplianceDataToRequirement method for tenant id: " + tenantId.ToString() + " , and Data For Rule Execution is: " + lstRequirementRows + " *******************");

                                        var lstOldPackageData = lstRequirementRuleData.Where(cond => !cond.IsNewPackage).ToList();
                                        var lstNewPackageData = lstRequirementRuleData.Where(cond => cond.IsNewPackage).ToList();

                                        if (!lstOldPackageData.IsNullOrEmpty() && lstOldPackageData.Count > 0)
                                        {
                                            ComplianceSetupManager.ExecuteRequirementRules(tenantId, backgroundProcessUserId, lstOldPackageData);
                                        }

                                        if (!lstNewPackageData.IsNullOrEmpty() && lstNewPackageData.Count > 0)
                                        {

                                            var lstRuleData = lstNewPackageData
                                                                .DistinctBy(cond => new { cond.Rps_Id, cond.PackageId, cond.IsNewPackage, cond.ApplicantUserID, cond.CategoryId, cond.ItemId })
                                                                .ToList();

                                            foreach (var item in lstRuleData)
                                            {
                                                List<Int32> lstReqFields = lstNewPackageData.Where(cond => cond.Rps_Id == item.Rps_Id
                                                                                            && cond.PackageId == item.PackageId
                                                                                            && cond.IsNewPackage == item.IsNewPackage
                                                                                            && cond.ApplicantUserID == item.ApplicantUserID
                                                                                            && cond.CategoryId == item.CategoryId
                                                                                            && cond.ItemId == item.ItemId).Select(cond => cond.FieldId).ToList();

                                                Requirement.EvaluateRequirementDynamicBuisnessRules(item.Rps_Id, item.CategoryId, item.ItemId, backgroundProcessUserId, tenantId, lstReqFields);
                                            }


                                            //UAT-3112:-
                                            List<Int32> lstARID = lstNewPackageData.Select(s => s.ApplicantRequirementItemDataID).Distinct().ToList();

                                            if (!lstARID.IsNullOrEmpty())
                                            {
                                                string arid = string.Join(",", lstARID);
                                                ComplianceDataManager.SaveBadgeFormNotificationData(tenantId, null, arid, null, backgroundProcessUserId);
                                            }

                                        }

                                        var packagSubscriptionStatuses = Business.RepoManagers.ApplicantRequirementManager.GetPackageSubscriptionCategoryStatus(tenantId, rpsIds_RuleData);
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

                                                //UAT-4015
                                                ApplicantRequirementManager.SendMailToInstPrecpReqPKgCompliantStatus(tenantId, requirementPackageSubscriptionID, backgroundProcessUserId);
                                            }
                                        }

                                        //UAT-2905
                                        //List<Int32> lstItemId = lstRequirementRuleData.Select(Sel => Sel.ItemId).Distinct().ToList();//UAT-2905
                                        //ComplianceDataManager.SendNotificationToAdminForItemSubmitted(true, tenantId, rpsIds_RuleData, String.Join(",",lstItemId),prevData);

                                        //UAT-2975
                                        RequirementVerificationManager.SyncRequirementVerificationToFlatData(rpsIds_RuleData, tenantId, backgroundProcessUserId);
                                        logger.Info("*******************  Rule Execution Start for CopyComplianceDataToRequirement method for tenant id: " + tenantId.ToString() + " , and Data For Rule Execution is: " + lstRequirementRows + " *******************");

                                        #region  UAT-3273- Get status after rule execution
                                        logger.Info("*******************  START After rule execution- Getting Approved Rotaions for GetApprovedSubscrptionByRequirementPackageSubscriptionIDs method for tenant id: " + tenantId.ToString() + " , and rpsIds_RuleData: " + rpsIds_RuleData + " *******************");
                                        var dataAfterRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(tenantId, rpsIds_RuleData);
                                        ProfileSharingManager.CheckAndSendNotificationForNewlyApprovedRotations(dataBeforeRuleExecution, dataAfterRuleExecution, tenantId);
                                        logger.Info("*******************  END After rule execution- Getting Approved Rotaions for GetApprovedSubscrptionByRequirementPackageSubscriptionIDs*******************");
                                        #endregion

                                        #region UAT-3805
                                        //UAT 3805
                                        List<Entity.ClientEntity.RequirementPackageSubscription> lstAfterRequirementPackageSubscriptions = new List<Entity.ClientEntity.RequirementPackageSubscription>();
                                        if (lstRequirementRuleData.IsNotNull() && lstRequirementRuleData.Count > 0)
                                        {
                                            List<Int32> lstAfterReqPackageSubscriptions = lstRequirementRuleData.Select(cond => cond.Rps_Id).ToList();
                                            lstAfterRequirementPackageSubscriptions = RequirementRuleManager.GetRequirementPackageSubscriptionBySubscriptionIds(tenantId, lstAfterReqPackageSubscriptions);
                                        }
                                        if (lstRequirementRuleData.IsNotNull() && lstRequirementRuleData.Count > 0)
                                        {

                                            foreach (var requirementData in lstRequirementRuleData.Select(sel => new RequirementRuleData { Rps_Id = sel.Rps_Id, ApplicantUserID = sel.ApplicantUserID }).Distinct())
                                            {
                                                String categoryIds = String.Empty;
                                                if (dicBeforeRPS.ContainsKey(requirementData.Rps_Id))
                                                    categoryIds = dicBeforeRPS[requirementData.Rps_Id];

                                                List<Int32> affectedCategoryIds = new List<Int32>();
                                                Entity.ClientEntity.RequirementPackageSubscription afterRequirementPackageSubscription = lstAfterRequirementPackageSubscriptions.Where(con => con.RPS_ID == requirementData.Rps_Id).FirstOrDefault();
                                                affectedCategoryIds = afterRequirementPackageSubscription.ApplicantRequirementCategoryDatas.Where(con => !con.ARCD_IsDeleted).Select(sel => sel.ARCD_RequirementCategoryID).ToList();
                                                String approvedCategoryIds = String.Join(",", affectedCategoryIds);
                                                ProfileSharingManager.SendItemDocNotificationOnCategoryApproval(tenantId, approvedCategoryIds, requirementData.ApplicantUserID
                                                                                                                           , categoryIds, lkpUseTypeEnum.ROTATION.GetStringValue()
                                                                                                                           , null, requirementData.Rps_Id, backgroundProcessUserId);

                                            }
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    executeLoop = false;
                                }
                            }

                            logger.Info("******************* END while loop of CopyComplianceDataToRequirement method for tenant id: " + tenantId.ToString() + " *******************");

                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.CopyComplianceDataToRequirement.GetStringValue();
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
                        logger.Error("An Error has occured in CopyComplianceDataToRequirement method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString + " current TenantId is : " + clntDbConf.CDB_TenantID);
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
                logger.Error("An Error has occured in CopyComplianceDataToRequirement method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                if (ex.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in ex.Data)
                    {
                        logger.Error("{0}:, {1}", de.Key.ToString(), de.Value);
                    }
                }
            }

        }

        #region UAT-2388
        public static void SendAutomaticPackageInvitationEmail(Int32? tenant_Id = null)
        {
            try
            {
                logger.Info("******************* Calling SendAutomaticPackageInvitationEmail: " + DateTime.Now.ToString() + " *******************");
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

                Int32 chunkSize = AppConsts.CHUNK_SIZE_FOR_AUTOMATIC_PACKAGE_INVITATION;

                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    Int32 tenantId = clntDbConf.CDB_TenantID;

                    logger.Info("******************* START while loop of SendAutomaticPackageInvitationEmail method for tenant id: " + tenantId.ToString() + " *******************");

                    try
                    {
                        if (CheckDB.TestConnString(clntDbConf.CDB_ConnectionString))
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;
                            String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);
                            Boolean executeLoop = true;
                            while (executeLoop)
                            {
                                DataTable lstAutomaticPackageInvitation = new DataTable();
                                logger.Info("******************* START Getting AutomaticPackageInvitation Objects*******************");
                                lstAutomaticPackageInvitation = ComplianceSetupManager.GetAutomaticPackageInvitations(tenantId, chunkSize);

                                logger.Info("******************* END Getting AutomaticPackageInvitation Objects*******************");

                                if (!lstAutomaticPackageInvitation.IsNullOrEmpty() && lstAutomaticPackageInvitation.Rows.Count > AppConsts.NONE)
                                {
                                    logger.Info("******************* START Insert AutomaticPackageInvitation Email in system communication******************");
                                    List<Int32> successful_AIPML_IDs = new List<Int32>();
                                    foreach (DataRow item in lstAutomaticPackageInvitation.Rows)
                                    {
                                        Int32 HierarchyNodeID = Convert.ToInt32(item["SelectedNodeID"]);
                                        Int32 automaticPackageInvitationID = Convert.ToInt32(item["AIP_ID"]);
                                        Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                        dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, Convert.ToString(item["ApplicantName"]));
                                        dictMailData.Add(EmailFieldConstants.Order_Number, Convert.ToString(item["OrderNumber"]));
                                        dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, Convert.ToString(item["InitialPackageHierarchy"]));
                                        dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, Convert.ToString(item["PackageNames"]));
                                        dictMailData.Add(EmailFieldConstants.TENANT_ID, tenantId);
                                        dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
                                        Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                        mockData.UserName = Convert.ToString(item["UserName"]);
                                        mockData.EmailID = Convert.ToString(item["ApplicantEmail"]);
                                        mockData.ReceiverOrganizationUserID = Convert.ToInt32(item["OrganizationUserID"]);

                                        //Send mail
                                        CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_ALL_AUTOMATIC_PACKAGE_INVITATION, dictMailData, mockData, tenantId, HierarchyNodeID, null, null, true);
                                        successful_AIPML_IDs.Add(automaticPackageInvitationID);
                                    }

                                    logger.Info("******************* END Insert AutomaticPackageInvitation Email in system communication******************");

                                    logger.Info("******************* START Update AutomaticPackageInvitation Email Status*******************");
                                    ComplianceSetupManager.UpdateAutomaticPackageInvitationsEmailStatus(tenantId, successful_AIPML_IDs, backgroundProcessUserId);
                                    logger.Info("******************* END Update AutomaticPackageInvitation Email Status*******************");
                                }
                                else
                                {
                                    executeLoop = false;
                                }
                            }

                            logger.Info("******************* END while loop of CopyComplianceDataToRequirement method for tenant id: " + tenantId.ToString() + " *******************");

                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.AutomaticPackageInvitation.GetStringValue();
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
                        logger.Error("An Error has occured in AutomaticPackageInvitation method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString + " current TenantId is : " + clntDbConf.CDB_TenantID);
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
                logger.Error("An Error has occured in AutomaticPackageInvitation method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
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
