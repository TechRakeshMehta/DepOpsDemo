using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.RotationPackages;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Xml.Linq;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace Business.RepoManagers
{
    public class RequirementRuleManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static RequirementRuleManager()
        {
            BALUtils.ClassModule = "Requirement Rule Manager";
        }

        #endregion

        #region Requirement Package Rules Execution

        /// <summary>
        /// Execute rules after saving Verification detail screen data.
        /// </summary>
        /// <param name="ruleObjectMapping"></param>
        /// <param name="reqSubscriptionId"></param>
        /// <param name="systemUserId"></param>
        /// <param name="tenantId"></param>
        public static void ExecuteRequirementObjectBuisnessRules(List<RequirementRuleObject> ruleObjectMapping, Int32 reqSubscriptionId, Int32 systemUserId, Int32 tenantId)
        {
            try
            {
                String ruleObjectXml = GenarateRuleObjectXml(ruleObjectMapping, tenantId);
                BALUtils.GetRequirementRuleRepoInstance(tenantId).ExecuteRequirementObjectBuisnessRules(ruleObjectXml, reqSubscriptionId, systemUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
            }
        }

        /// <summary>
        /// Execute rules after saving Verification detail screen data.
        /// </summary>
        /// <param name="ruleObjectMapping"></param>
        /// <param name="reqSubscriptionId"></param>
        /// <param name="systemUserId"></param>
        /// <param name="tenantId"></param>
        public static void EvaluateRequirementPostSubmitRules(List<RequirementRuleObject> ruleObjectMapping, Int32 reqSubscriptionId, Int32 systemUserId, Int32 tenantId)
        {
            try
            {
                String ruleObjectXml = GenarateRuleObjectXml(ruleObjectMapping, tenantId);
                BALUtils.GetRequirementRuleRepoInstance(tenantId).EvaluateRequirementPostSubmitRules(ruleObjectXml, reqSubscriptionId, systemUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
            }
        }

        #endregion

        public static Boolean ProcessRequirementItemExpiry(Int32 chunkSize, Int32 systemUserId, Int32 tenantID)
        {
            try
            {
                List<RequirementExpiryContract> lstRequirementExpiryContract = BALUtils.GetRequirementRuleRepoInstance(tenantID).SetRequirementItemExpiry(chunkSize, systemUserId);
                if (lstRequirementExpiryContract.Count > AppConsts.NONE)
                {
                    foreach (RequirementExpiryContract requirementExpiryContract in lstRequirementExpiryContract)
                    {
                        if (requirementExpiryContract.IsNewPackage)
                        {
                            List<RequirementRuleObject> mappingXml = GenerateNewRuleObjectMappingXml(requirementExpiryContract.RequirementCatId, requirementExpiryContract.RequirementItemId);
                            EvaluateRequirementPostSubmitRules(mappingXml, requirementExpiryContract.RequirementSubId, systemUserId, tenantID);
                        }
                        else
                        {
                            List<RequirementRuleObject> mappingXml = GenerateRuleObjectMappingXml(requirementExpiryContract.RequirementPkgId, requirementExpiryContract.RequirementCatId, requirementExpiryContract.RequirementItemId);
                            ExecuteRequirementObjectBuisnessRules(mappingXml, requirementExpiryContract.RequirementSubId, systemUserId, tenantID);
                        }
                    }

                    //UAT- 2975
                    String rps_IDS = String.Join(",", lstRequirementExpiryContract.Select(slct => slct.RequirementSubId).ToList());
                    RequirementVerificationManager.SyncRequirementVerificationToFlatData(rps_IDS, tenantID, systemUserId);

                    //Send Notications [UAT-1597]
                    lstRequirementExpiryContract.DistinctBy(pkg => pkg.RequirementPkgId).ForEach(pkgId =>
                    {
                        //lstRequirementExpiryContract.Where(cond => cond.RequirementPkgId == pkgId.RequirementPkgId).ForEach(itm =>
                        // {
                        //     ComplianceDataManager.SendNotificationOnItemStatusChangedToReviewStatus(true, tenantID, itm.RequirementSubId, AppConsts.NONE, itm.RequirementCatId, itm.RequirementItemId, systemUserId, AppConsts.NONE, RequirementItemStatus.EXPIRED.GetStringValue());
                        // });
                        ComplianceDataManager.SendNotificationOnRotationPkgStatusChangedFromCompToNC(tenantID, pkgId.RequirementSubId, systemUserId, pkgId.RequirementPkgSubStatusCode);
                    });

                    return true;
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementScheduledAction> GetActiveScheduleActionList(Int32 chunkSize, String actionType, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRequirementRuleRepoInstance(tenantId).GetActiveScheduleActionList(chunkSize, actionType);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //UAT-3805
        public static List<RequirementPackageSubscription> GetRequirementPackageSubscriptionBySubscriptionIds(Int32 tenantId, List<Int32> lstPackageSubscriptionIds)
        {
            try
            {
                return BALUtils.GetComplianceDataRepoInstance(tenantId).GetRequirementPackageSubscriptionBySubscriptionIds(lstPackageSubscriptionIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        //UAT-3805
        public static List<RequirementPackageSubscription> GetRequirementPackageSubscriptionByApplicantComplianceItemIds(Int32 tenantId, List<Int32> lstApplicantComplianceItemIds)
        {
            try
            {
                return BALUtils.GetComplianceDataRepoInstance(tenantId).GetRequirementPackageSubscriptionByApplicantComplianceItemIds(lstApplicantComplianceItemIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void ExecuteRequirementCategoryScheduleAction(Int32 systemUserId, Int32 chunkSize, Int32 tenantId, String ScheduleActionTypeCode)
        {
            try
            {
                List<RequirementPackageSubscriptionContract> lstRequirementPackageSubscription = BALUtils.GetRequirementRuleRepoInstance(tenantId)
                                                                                    .ExecuteRequirementCategoryScheduleAction(systemUserId, chunkSize, ScheduleActionTypeCode);

                //UAT-3805
                List<Int32> lstPackageSubscriptionIds = lstRequirementPackageSubscription.Select(slct => slct.RequirementPackageSubscriptionID).ToList();
                List<RequirementPackageSubscription> lstBeforeRequirementPackageSubscriptions =
                    BALUtils.GetComplianceDataRepoInstance(tenantId).GetRequirementPackageSubscriptionBySubscriptionIds(lstPackageSubscriptionIds);




                //UAT- 2975
                String rps_IDS = String.Join(",", lstRequirementPackageSubscription.Select(slct => slct.RequirementPackageSubscriptionID).ToList());

                #region  UAT-3273- Get status before rule execution
                List<Int32> lstRPS = lstRequirementPackageSubscription.Where(cond => cond.RequirementPackageSubscriptionStatusCode == RequirementPackageStatus.REQUIREMENT_NOT_COMPLIANT.GetStringValue()).Select(slct => slct.RequirementPackageSubscriptionID).ToList();
                var dataBeforeRuleExecution = new System.Data.DataTable();
                #endregion

                RequirementVerificationManager.SyncRequirementVerificationToFlatData(rps_IDS, tenantId, systemUserId);

                //UAT-3805
                List<RequirementPackageSubscription> lstAfterRequirementPackageSubscriptions =
                    BALUtils.GetComplianceDataRepoInstance(tenantId).GetRequirementPackageSubscriptionBySubscriptionIds(lstPackageSubscriptionIds);


                //UAT-2533
                foreach (var reqPkgSubDetails in lstRequirementPackageSubscription)
                {
                    ComplianceDataManager.SendNotificationOnRotationPkgStatusChangedFromCompToNC(tenantId, reqPkgSubDetails.RequirementPackageSubscriptionID, systemUserId, reqPkgSubDetails.RequirementPackageSubscriptionStatusCode);

                    #region UAT-3805
                    List<Int32> approvedCategoryIDs = new List<Int32>();
                    RequirementPackageSubscription beforeRequirementPackageSubscription = lstBeforeRequirementPackageSubscriptions.Where(con => con.RPS_ID == reqPkgSubDetails.RequirementPackageSubscriptionID).FirstOrDefault();
                    if (!beforeRequirementPackageSubscription.IsNullOrEmpty() && !beforeRequirementPackageSubscription.ApplicantRequirementCategoryDatas.IsNullOrEmpty())
                    {
                        foreach (var item in beforeRequirementPackageSubscription.ApplicantRequirementCategoryDatas)
                        {
                            if (!item.ARCD_IsDeleted)
                            {
                                if (item.lkpRequirementCategoryStatu.RCS_Code == RequirementCategoryStatus.APPROVED.GetStringValue())
                                {
                                    approvedCategoryIDs.Add(item.ARCD_RequirementCategoryID);
                                }
                            }
                        }
                    }
                    String approvedCategoryIds = approvedCategoryIDs.IsNullOrEmpty() ? String.Empty : String.Join(",", approvedCategoryIDs);
                    List<Int32> affectedCategoryIds = new List<Int32>();
                    RequirementPackageSubscription afterRequirementPackageSubscription = lstAfterRequirementPackageSubscriptions.Where(con => con.RPS_ID == reqPkgSubDetails.RequirementPackageSubscriptionID).FirstOrDefault();
                    affectedCategoryIds = afterRequirementPackageSubscription.ApplicantRequirementCategoryDatas.Where(con => !con.ARCD_IsDeleted).Select(sel => sel.ARCD_RequirementCategoryID).ToList();
                    String categoryIds = String.Join(",", affectedCategoryIds);
                    ProfileSharingManager.SendItemDocNotificationOnCategoryApproval(tenantId, categoryIds, reqPkgSubDetails.ApplicantOrgUserID
                                                                                               , approvedCategoryIds, lkpUseTypeEnum.ROTATION.GetStringValue()
                                                                                            , null, reqPkgSubDetails.RequirementPackageSubscriptionID, systemUserId);

                    #endregion

                }

                #region  UAT-3273- Get status after rule execution
                var dataAfterRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(tenantId, String.Join(",", lstRPS));
                ProfileSharingManager.CheckAndSendNotificationForNewlyApprovedRotations(dataBeforeRuleExecution, dataAfterRuleExecution, tenantId);
                #endregion

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void ExecuteRequirementPackageScheduleAction(Int32 systemUserId, Int32 chunkSize, Int32 tenantId)
        {
            try
            {
                List<RequirementPackageSubscriptionContract> lstRequirementPackageSubscription = BALUtils.GetRequirementRuleRepoInstance(tenantId)
                                                                                            .ExecuteRequirementPackageScheduleAction(systemUserId, chunkSize);

                //UAT- 2975
                String rps_IDS = String.Join(",", lstRequirementPackageSubscription.Select(slct => slct.RequirementPackageSubscriptionID).ToList());

                #region  UAT-3273- Get status before rule execution
                var dataBeforeRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(tenantId, rps_IDS);
                #endregion

                RequirementVerificationManager.SyncRequirementVerificationToFlatData(rps_IDS, tenantId, systemUserId);

                //UAT-2533
                foreach (var reqPkgSubDetails in lstRequirementPackageSubscription)
                {
                    ComplianceDataManager.SendNotificationOnRotationPkgStatusChangedFromCompToNC(tenantId, reqPkgSubDetails.RequirementPackageSubscriptionID, systemUserId, reqPkgSubDetails.RequirementPackageSubscriptionStatusCode);
                }

                #region  UAT-3273- Get status after rule execution
                var dataAfterRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(tenantId, String.Join(",", rps_IDS));
                ProfileSharingManager.CheckAndSendNotificationForNewlyApprovedRotations(dataBeforeRuleExecution, dataAfterRuleExecution, tenantId);
                #endregion

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region Private Methods

        /// <summary>
        /// Method for generating object mapping Xml
        /// </summary>
        /// <param name="ruleObjectMappingList">object list of RuleObjectMapping </param>
        /// <returns></returns>
        private static String GenarateRuleObjectXml(List<RequirementRuleObject> ruleObjectMappingList, Int32 tenantId)
        {
            List<lkpObjectType> lstlkpObjectType = LookupManager.GetLookUpData<lkpObjectType>(tenantId).Where(cond => !cond.OT_IsDeleted).ToList();
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("RuleObjects"));
            foreach (RequirementRuleObject ruleObjectMapping in ruleObjectMappingList)
            {
                var lkpObjectType = lstlkpObjectType.Where(sel => sel.OT_Code == ruleObjectMapping.RuleObjectTypeCode).FirstOrDefault();
                XmlNode exp = el.AppendChild(doc.CreateElement("RuleObject"));
                exp.AppendChild(doc.CreateElement("TypeId")).InnerText = lkpObjectType.IsNotNull() ? lkpObjectType.OT_ID.ToString() : String.Empty;
                exp.AppendChild(doc.CreateElement("Id")).InnerText = ruleObjectMapping.RuleObjectId;
                exp.AppendChild(doc.CreateElement("ParentId")).InnerText = ruleObjectMapping.RuleObjectParentId;
            }
            return doc.OuterXml.ToString();
        }


        private static List<RequirementRuleObject> GenerateRuleObjectMappingXml(Int32 reqPkgId, Int32 reqCategoryId, Int32 reqItemId)
        {
            List<RequirementRuleObject> ruleObjectMappingList = new List<RequirementRuleObject>();
            RequirementRuleObject ruleObjectMappingForPackage = new RequirementRuleObject
            {
                RuleObjectTypeCode = ObjectType.Compliance_Package.GetStringValue(),
                RuleObjectId = Convert.ToString(reqPkgId),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RequirementRuleObject ruleObjectMappingForCategory = new RequirementRuleObject
            {
                RuleObjectTypeCode = ObjectType.Compliance_Category.GetStringValue(),
                RuleObjectId = Convert.ToString(reqCategoryId),
                RuleObjectParentId = Convert.ToString(reqPkgId)
            };

            RequirementRuleObject ruleObjectMappingForItem = new RequirementRuleObject
            {
                RuleObjectTypeCode = ObjectType.Compliance_Item.GetStringValue(),
                RuleObjectId = Convert.ToString(reqItemId),
                RuleObjectParentId = Convert.ToString(reqCategoryId)
            };

            ruleObjectMappingList.Add(ruleObjectMappingForPackage);
            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            ruleObjectMappingList.Add(ruleObjectMappingForItem);
            return ruleObjectMappingList;
        }


        private static List<RequirementRuleObject> GenerateNewRuleObjectMappingXml(Int32 reqCategoryId, Int32 reqItemId)
        {
            List<RequirementRuleObject> ruleObjectMappingList = new List<RequirementRuleObject>();

            RequirementRuleObject ruleObjectMappingForCategory = new RequirementRuleObject
            {
                RuleObjectTypeCode = ObjectType.Compliance_Category.GetStringValue(),
                RuleObjectId = Convert.ToString(reqCategoryId),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RequirementRuleObject ruleObjectMappingForItem = new RequirementRuleObject
            {
                RuleObjectTypeCode = ObjectType.Compliance_Item.GetStringValue(),
                RuleObjectId = Convert.ToString(reqItemId),
                RuleObjectParentId = Convert.ToString(reqCategoryId)
            };
            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            ruleObjectMappingList.Add(ruleObjectMappingForItem);
            return ruleObjectMappingList;
        }

        #endregion

        #region Rule template

        public static bool CopyRuleTemplate(String RuleTemplateName, Int32 CurrentLoggedInUserId, Int32 SelectedRuleTemplateID)
        {
            try
            {
                //RuleTemplate rule = BALUtils.GetRuleRepoInstance(fromTenantId).GetRuleTemplate(ruleId);
                Int32 tenantID = AppConsts.ONE;
                Entity.SharedDataEntity.RequirementRuleTemplate rule = BALUtils.GetRequirementRuleRepoInstance(tenantID).GetRuleTemplateDetails(SelectedRuleTemplateID);
                return BALUtils.GetRequirementRuleRepoInstance(tenantID).CopyRuleTemplate(rule, RuleTemplateName, CurrentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Rule Mapping

        public static List<Entity.SharedDataEntity.RequirementRuleTemplate> GetRuleTemplates()
        {
            try
            {
                Int32 tenantID = AppConsts.ONE;
                return BALUtils.GetRequirementRuleRepoInstance(tenantID).GetRuleTemplates();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Entity.SharedDataEntity.RequirementRuleTemplate GetRuleTemplateDetails(Int32 ruleTemplateId)
        {
            try
            {
                Int32 tenantID = AppConsts.ONE;
                return BALUtils.GetRequirementRuleRepoInstance(tenantID).GetRuleTemplateDetails(ruleTemplateId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Entity.SharedDataEntity.RequirementObjectRule> GetRuleMappings(Int32 ParentObjectTreeID)
        {
            try
            {
                Int32 tenantID = AppConsts.ONE;
                return BALUtils.GetRequirementRuleRepoInstance(tenantID).GetRuleMappings(ParentObjectTreeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Entity.SharedDataEntity.RequirementObjectRule GetRuleMapping(Int32 ObjectRuleID)
        {
            try
            {
                Int32 tenantID = AppConsts.ONE;
                return BALUtils.GetRequirementRuleRepoInstance(tenantID).GetRequirementRuleMapping(ObjectRuleID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Entity.SharedDataEntity.RequirementObjectTree GetRequirementObjectTree(Int32 ObjectTreeID, Int32 objectTypeID, String objectTypeCode = "")
        {
            Int32 tenantID = AppConsts.ONE;
            return BALUtils.GetRequirementRuleRepoInstance(tenantID).GetRequirementObjectTree(ObjectTreeID, objectTypeID, objectTypeCode);
        }

        public static List<RequirementExpressionData> GetAttributeDetail(List<Int32> objectIds)
        {
            try
            {
                Int32 tenantID = AppConsts.ONE;
                return BALUtils.GetRequirementRuleRepoInstance(tenantID).GetAttributeDetail(objectIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static RuleProcessingResult ValidateExpression(String ruleTemplateXml, String ruleExpressionXml)
        {
            try
            {
                Int32 tenantID = AppConsts.ONE;
                String outPutResult = BALUtils.GetRequirementRuleRepoInstance(tenantID).ValidateExpression(ruleTemplateXml, ruleExpressionXml);
                RuleProcessingResult processingResult = ParseValidationXml(outPutResult);
                return processingResult;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static RuleProcessingResult ParseValidationXml(String outPutResult)
        {
            try
            {
                var doc = XDocument.Parse(outPutResult);
                RuleProcessingResult rProcessdResult = new RuleProcessingResult();
                rProcessdResult.Status = Convert.ToInt32(doc.Descendants().FirstOrDefault(x => x.Name == "Status").Value);
                rProcessdResult.Action = Convert.ToString(doc.Descendants().FirstOrDefault(x => x.Name == "Action").Value);
                rProcessdResult.Result = Convert.ToString(doc.Descendants().FirstOrDefault(x => x.Name == "Result").Value);
                rProcessdResult.UIExpressionLabel = Convert.ToString(doc.Descendants().FirstOrDefault(x => x.Name == "UIExpressionLabel").Value);
                rProcessdResult.SuccessMessage = Convert.ToString(doc.Descendants().FirstOrDefault(x => x.Name == "SuccessMessage").Value);
                rProcessdResult.ErrorMessage = Convert.ToString(doc.Descendants().FirstOrDefault(x => x.Name == "ErrorMessage").Value);
                return rProcessdResult;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                RuleProcessingResult rProcessdResult = new RuleProcessingResult();
                rProcessdResult.Status = AppConsts.NONE;
                rProcessdResult.ErrorMessage = "Some Internal error has occured while parsing the Xml";
                return rProcessdResult;
            }

        }

        public static Boolean AddRuleMapping(Entity.SharedDataEntity.RequirementObjectRule ruleMapping)
        {
            try
            {
                Int32 tenantID = AppConsts.ONE;
                return BALUtils.GetRequirementRuleRepoInstance(tenantID).AddRuleMapping(ruleMapping);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void UpdateRequirementRuleMapping(Entity.SharedDataEntity.RequirementObjectRule requirementObjectRule)
        {
            try
            {
                Int32 tenantID = AppConsts.ONE;
                BALUtils.GetRequirementRuleRepoInstance(tenantID).UpdateRequirementRuleMapping(requirementObjectRule);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void DeleteRequirementRuleMapping(Int32 ruleMappingId, Int32 currentUserId)
        {
            try
            {
                Int32 tenantID = AppConsts.ONE;
                BALUtils.GetRequirementRuleRepoInstance(tenantID).DeleteRequirementRuleMapping(ruleMappingId, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Gets the list of all Rule Types.
        /// </summary>
        /// <returns>
        /// IQueryable List of lkpRuleType Objects.
        /// </returns>
        public static List<Entity.SharedDataEntity.lkpRuleType> GetRuleTypes()
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpRuleType>().AsQueryable().ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Gets the list of all Rule Action Types.
        /// </summary>
        /// <returns>
        /// IQueryable List of lkpRuleActionType Objects.
        /// </returns>
        public static List<Entity.SharedDataEntity.lkpRuleActionType> GetRuleActionTypes()
        {
            try
            {
                String BothUseTypeCode = "AAAC";
                Int32 BothUseTypeID = LookupManager.GetSharedLookUpIDbyCode<Entity.SharedDataEntity.lkpUseType>(con => con.UT_Code.Trim().Contains(BothUseTypeCode));
                return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpRuleActionType>().Where(con => con.ACT_UseType == BothUseTypeID).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region Category-Item Mapping


        public static List<RequirementExpressionData> GetRequirementCategoryByCategoryID(Int32 CategoryID, String ObjectTypeCode)
        {
            try
            {
                Int32 tenantID = AppConsts.ONE;
                return BALUtils.GetRequirementRuleRepoInstance(tenantID).GetRequirementCategoryByCategoryID(CategoryID, ObjectTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        /// <summary>
        /// Gets the list of Items related to a category
        /// </summary>
        /// <param name="categoryId">Id of the selected category</param>
        /// <returns>List of the items of that category</returns>
        public static List<RequirementExpressionData> GetRequirementCategoryItems(Int32 categoryId)
        {
            try
            {
                Int32 tenantID = AppConsts.ONE;
                return BALUtils.GetRequirementRuleRepoInstance(tenantID).GetRequirementCategoryItems(categoryId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the list of Items related to a category
        /// </summary>
        /// <param name="categoryId">Id of the selected category</param>
        /// <returns>List of the items of that category</returns>
        public static List<RequirementExpressionData> GetRequirementSubmissionItemsByCategoryID(Int32 categoryId)
        {
            try
            {
                Int32 tenantID = AppConsts.ONE;
                return BALUtils.GetRequirementRuleRepoInstance(tenantID).GetRequirementSubmissionItemsByCategoryID(categoryId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #endregion

        #region Item-Attributes Mapping

        public static List<RequirementExpressionData> GetComplianceItemAttribute(Int32 itemID)
        {
            try
            {
                Int32 tenantID = AppConsts.ONE;
                return BALUtils.GetRequirementRuleRepoInstance(tenantID).GetRequirementItemAttribute(itemID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
        }

        #endregion

        public static List<Entity.SharedDataEntity.lkpRuleObjectMappingType> GetRuleObjectMappingType()
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpRuleObjectMappingType>().Where(Cond => !Cond.RMT_IsDeleted).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static IQueryable<Entity.SharedDataEntity.lkpConstantType> GetConstantType()
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpConstantType>().Where(obj => obj.IsDeleted == false).AsQueryable();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Entity.SharedDataEntity.lkpObjectType> GetObjectTypes(String ruleObjectMappingTypeCode)
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpRuleObjectMappingTypeObjectType>().Where(rmtot => rmtot.lkpRuleObjectMappingType.RMT_Code == ruleObjectMappingTypeCode && !rmtot.RMTO_IsDeleted).
                Select(rmtot => rmtot.lkpObjectType).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Entity.SharedDataEntity.lkpObjectType> GetObjectTypeList()
        {
            return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpObjectType>().ToList();
        }

        public static List<Entity.SharedDataEntity.lkpRequirementItemStatus> GetItemComplianceStatus()
        {
            try
            {
                return LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpRequirementItemStatus>().Where(x => x.RIS_IsDeleted == false).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion


        #region UAT-2514 Copy Package
        /// <summary>
        /// Copy Rules From Shared To Tenant
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="oldRequirementCategoryID"></param>
        /// <param name="newRequirementCategoryID"></param>
        /// <param name="currentLoggedInUserID"></param>
        public static void CopyRulesFromSharedToTenant(Int32 tenantID, int oldRequirementCategoryID, int newRequirementCategoryID, int currentLoggedInUserID)
        {
            try
            {
                BALUtils.GetRequirementRuleRepoInstance(tenantID).CopyRulesFromSharedToTenant(oldRequirementCategoryID, newRequirementCategoryID, currentLoggedInUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Requirement Rules Service Execution

        /// <summary>
        /// UAT -3080
        /// </summary>
        /// <param name="systemUserId"></param>
        /// <param name="chunkSize"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean ExecuteRequirementScheduledCategoryComplianceRules(Int32 systemUserId, Int32 chunkSize, Int32 tenantId)
        {
            List<RequirementScheduleActionCategoryRulesContract> scheduleExecuteCategoryRulesList = new List<RequirementScheduleActionCategoryRulesContract>();
            scheduleExecuteCategoryRulesList = BALUtils.GetRequirementRuleRepoInstance(tenantId).GetScheduleActionExecuteCategoryRulesList(chunkSize);
            if (scheduleExecuteCategoryRulesList.Count > AppConsts.NONE)
            {
                #region  UAT-3273- Get status before rule execution
                List<int> reqPackageObjectIds = scheduleExecuteCategoryRulesList.Select(x => x.RequirementPackageSubscriptionID).ToList();
                var dataBeforeRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(tenantId, String.Join(",", reqPackageObjectIds));
                #endregion

                foreach (RequirementScheduleActionCategoryRulesContract scheduledcategory in scheduleExecuteCategoryRulesList)
                {
                    if (scheduledcategory != null)
                    {
                        try
                        {
                            Int32 itemId = scheduledcategory.RequirementItemID;
                            Int32 categoryId = scheduledcategory.RequirementCategoryID;
                            Int32 applicantId = scheduledcategory.ApplicantOrgUserID;
                            Int32 packageSubscriptionId = scheduledcategory.RequirementPackageSubscriptionID;
                            List<RequirementRuleObject> ruleObjectMappingList = GenerateNewRuleObjectMappingXml(categoryId, itemId);
                            RequirementRuleManager.EvaluateRequirementPostSubmitRules(ruleObjectMappingList, packageSubscriptionId, applicantId, tenantId);
                            BALUtils.GetRequirementRuleRepoInstance(tenantId).inactiveProcessedScheduleAction(scheduledcategory.ScheduleActionId, systemUserId);
                            String newSubscriptionStatusCode = BALUtils.GetRequirementPackageRepoInstance(tenantId).GetReqPackageSubsStatusBySubscriptionID(packageSubscriptionId);

                            if (newSubscriptionStatusCode != scheduledcategory.RequirementPackageSubscriptionStatusCode)
                            {
                                ComplianceDataManager.SendNotificationOnRotationPkgStatusChangedFromCompToNC(tenantId, scheduledcategory.RequirementPackageSubscriptionID, systemUserId, scheduledcategory.RequirementPackageSubscriptionStatusCode);
                            }
                        }
                        catch (SysXException ex)
                        {
                            BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace()
                                + Environment.NewLine + "Tenant Id:" + tenantId
                                + Environment.NewLine + "Package Subscription Id:" + scheduledcategory.RequirementPackageSubscriptionID
                                + Environment.NewLine + ex.Message
                                + Environment.NewLine + ex.StackTrace, ex);
                            ex.Data["TenantId"] = tenantId;
                            ex.Data["PackageSubscriptionId"] = scheduledcategory.RequirementPackageSubscriptionID;
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace()
                                + Environment.NewLine + "Tenant Id:" + tenantId
                                + Environment.NewLine + "Package Subscription Id:" + scheduledcategory.RequirementPackageSubscriptionID
                                + Environment.NewLine + ex.Message
                                + Environment.NewLine + ex.StackTrace, ex);
                            ex.Data["TenantId"] = tenantId;
                            ex.Data["PackageSubscriptionId"] = scheduledcategory.RequirementPackageSubscriptionID;
                            throw (new SysXException(ex.Message, ex));
                        }
                    }
                }

                #region  UAT-3273- Get status after rule execution
                var dataAfterRuleExecution = Business.RepoManagers.ApplicantRequirementManager.GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(tenantId, String.Join(",", reqPackageObjectIds));
                ProfileSharingManager.CheckAndSendNotificationForNewlyApprovedRotations(dataBeforeRuleExecution, dataAfterRuleExecution, tenantId);
                #endregion

                return true;//Returns true if some records were found for procesing.
            }
            return false;//Returns false if no record was processed.
        }

        #endregion

        public static List<lkpRotScheduledActionType> GetRotScheduledActionTypes(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRequirementRuleRepoInstance(tenantId).GetRotScheduledActionTypes();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
    }
}
