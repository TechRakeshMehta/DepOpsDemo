using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL.Interfaces
{
    public interface IRuleRepository
    {
        /// <summary>
        /// GetRuleTemplate
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        RuleTemplate GetRuleTemplate(Int32 ruleId);

        /// <summary>
        /// GetRuleTemplates
        /// </summary>
        /// <returns></returns>
        List<RuleTemplate> GetRuleTemplates(Boolean? IsSeriesDataRqd);

        /// <summary>
        /// UpdateRuleTemplate
        /// </summary>
        /// <param name="rule"></param>
        void UpdateRuleTemplate(Entity.ComplianceRuleTemplate ruleTemplate, List<Int32> expressionIds);

        /// <summary>
        /// AddRuleTemplate
        /// </summary>
        /// <param name="ruleTemplate"></param>
        void AddRuleTemplate(Entity.ComplianceRuleTemplate ruleTemplate);

        /// <summary>
        /// GetExpressionOperators
        /// </summary>
        /// <returns></returns>
        List<lkpExpressionOperator> GetExpressionOperators();

        /// <summary>
        /// Delete Rule Template
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="currentUserId"></param>
        void DeleteRuleTemplate(Int32 ruleId, Int32 currentUserId);

        /// <summary>
        /// ValidateRuleTemplate
        /// </summary>
        /// <param name="ruleTemplateXML"></param>
        /// <returns></returns>
        string ValidateRuleTemplate(string ruleTemplateXML);

        String ValidateExpression(String ruleTemplateXml, String ruleExpression);

        List<ComplianceAttribute> getAttributeDetail(List<Int32> objectIds);
        String RuleObjectMappingTypeCodebyId(Int32 rmtID);
        List<RuleImpactGroupMapping> getPreviousImpactedGroupMappings(Int32 ruleMappingId);
        void updateRuleImpactedGroupMappings(List<RuleImpactGroupMapping> impactedGroupMappings, Int32 ruleMappingId, Int32 loggedInUserId);
        #region Rule Mapping

        Boolean AddRuleMapping(RuleMapping ruleMapping);

        RuleMapping GetRuleMapping(Int32 rlm_Id, Boolean getRuleMappingDetails);

        RuleTemplate GetRuleTemplateDetails(Int32 ruleTemplateId);

        List<RuleSetTree> GetRuleSetTreeData();

        lkpObjectType GetObjectType(String ot_Code);

        List<lkpObjectType> GetObjectTypeList();

        lkpRuleObjectMappingType GetRuleObjectMappingType(String rmt_Code);

        //List<RuleMapping> GetRuleMappings(Int32 ruleSetId);
        List<RuleMappingContract> GetRuleMappings(Int32 ruleSetId);

        void DeleteRuleMapping(Int32 ruleMappingId, Int32 currentUserId);
        Boolean DeleteRuleMappingAndRefireRules(Int32 ruleMappingId, Int32 currentUserId);
        List<lkpRuleObjectMappingType> GetRuleObjectMappingType();

        List<lkpObjectType> GetObjectTypes(String ruleObjectMappingTypeCode);

        RuleMappingObjectTree GetRuleMapingObjectTree(Int32 ruleMappingDetailId, Int32 ruleSetTreeID);

        lkpObjectType GetObjectTypeById(Int32 ot_ID);

        Boolean UpdateRuleMapping(RuleMapping ruleMapping);

        Boolean IsRuleAlreadyInUse(Int32 ruleMappingId);

        IQueryable<lkpConstantType> getConstantType();

        lkpConstantType getConstantTypeByCode(String code);
        lkpConstantType getConstantTypeById(Int32 constantTypeId);
        void deActivatePreviousRules(Int32? prevVesrsionId);
        Boolean DeactivatePreviousRulesAndCreateNewRule(String parameters);

        List<lkpRuleImpactGroup> getImpactedUserGroupType();
        DataTable GetListOfInstanceWichCanShareRule(Int32 ruleSetId, String HID);
        RuleSynchronisationData GetListOfInstanceWichCanShareRuleOnEdit(Int32 ruleSetId, Int32 ruleId);
        Boolean ComplianceRuleSynchronisation(List<Int32> packageList, Int32 sourceRuleId, Int32 currentUserId, String settingParameters, Boolean sourceRuleHasSubscription);
        Boolean ComplianceRuleSynchronisationonRuleEdit(List<Int32> packageList, Int32 sourceRuleId, Int32 currentUserId, String settingParameters, Boolean isVersionUpdate, Int32? sourceRuleVersionId, Boolean? updateAllSelected);
        #endregion

        #region Evaluate Post Submit Rules

        void evaluatePostSubmitRules(Int32 applicantUserId, String ruleObjectXml, Int32 systemUserId);

        #endregion


        #region post submit rule On expiary

        /// <summary>
        /// get a list of applicanr compliance item data which are expiring today or already expired
        /// </summary>
        /// <param name="currentDate">Today Date</param>
        /// <returns></returns>
        List<ExpiredItemDataList> getExpiringItemData(Int32 chunkSize, Int32 userId);

        /// <summary>
        /// update the status of item data to expired.
        /// </summary>
        /// <param name="itemDataId"></param>
        void UpdateItemDataStatusToExpire(Int32 itemDataId);

        /// <summary>
        /// Get Expiring Compliance Items
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="beforeExpiry"></param>
        /// <param name="expiryFrequency"></param>
        /// <param name="entitySetName"></param>
        /// <param name="today"></param>
        /// <returns></returns>
        List<GetExpiredItemDataList> GetExpiringComplianceItems(Int32 tenantId, String subEventCode, Int32 subEventId, String entitySetName, Int32 chunkSize);

        /// <summary>
        /// get a list of applicanr compliance item data whose next action is set to be execute category rules.
        /// </summary>
        /// <param name="currentDate">Today Date</param>
        /// <returns></returns>
        List<GetActionScheduleExecuteCategoryRulesList> getActionActionExecuteCategoryRules(Int32 chunkSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduleActionId"></param>
        /// <param name="backgroundProcessUserId"></param>
        /// <returns></returns>
        Boolean inactiveProcessedScheduleAction(Int32 scheduleActionId, Int32 backgroundProcessUserId);

        /// <summary>
        /// for getting count of total record for which category rules scheduled to be reoccur.
        /// </summary>
        /// <returns></returns>
        Int32 getCountRecordsScheduleExecutecategoryrule();

        List<GetUpcomingNonComplianceCategories> GetUpcomingNonComplianceCategory(Int32 tenantId, Int32 chunkSize);

        #endregion

        /// <summary>
        /// Get a list of applicant compliance category data which are expiring today or already expired
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        DataTable GetExpiringCategoryData(Int32 chunkSize, Int32 userId);

        List<ScheduledAction> GetObjectDeletionActiveScheduleActionList(Int32 chunkSize);

        void ExecuteRulesOnObjectDeletion(Int32 packageSubscriptionId, Int32 removedObjectTypeId, Int32 currentUserId, Int32 removedObjectId);

        #region UAT-1217:Notification to correspond with UAT-1209
        DataTable GetNonComplianceRequiredCategoryActionData(Int32 remBfrExp, Int32 remExpFrq, Int32 chunkSize, Int32 tenantId);
        #endregion

        /// <summary>
        /// get the item list from ItemSeriesItem table on basis on series id
        /// </summary>
        /// <param name="seriesId">selected series id</param>
        /// <returns></returns>
        List<ItemSeriesItem> GetItemSeriesItemsBySeriesId(Int32 seriesId);

        /// <summary>
        /// get the item Attribute from ItemSeriesAttribute table on basis on series id
        /// </summary>
        /// <param name="seriesId">selected series id</param>
        /// <returns></returns>
        List<ItemSeriesAttribute> GetItemSeriesAttributeBySeriesId(Int32 seriesId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <returns></returns>
        List<RuleMapping> GetShotSeriesRuleMappings(Int32 seriesId, Int32 seriesObjectTypeId, Int32 seriesItemObjectTypeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleMapping"></param>
        /// <param name="objectIds"></param>
        /// <param name="objectTypeId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        Boolean UpdateShotSeriesRuleMapping(RuleMapping ruleMapping, List<Int32> objectIds, Int32 objectTypeId, Int32 currentUserId);

        void DeleteShotSeriesRuleMapping(Int32 ruleMappingId, Int32 currentUserId);
        //UAT-2047 copy rule template tenant to tenant
        Boolean CopyRuleTemplate(RuleTemplate ruleTemplate, String templateName, Int32 currentUserId);

        String TestComplianceRule(String ruleInputData);

        Tuple<Dictionary<Boolean, String>, Dictionary<Int32, Int32>> EvaluateDataEntryUIRules(Int32 packageSubscriptionID, String nonSeriesData, String seriesData);

        void EvaluateRequirementPostSubmitRules(String ruleObjectXml, Int32 reqSubscriptionId, Int32 systemUserId);

        Boolean IsTriggerForOtherCategoryNeeded(Int32 categoryID); //UAT-2725
        List<Int32> GetComplianceCategoryIdsByPackageID(Int32 packageId); //UAT-2725

        String CalculateDueDate(String resultXml); //UAT-2740

        List<ComplianceExceptionExpiryData> GetComplianceExceptionAboutToExpire(Int32 tenantId, Int32 subEventId, Int32 chunkSize);
        List<RuleSetData> GetRuleSetDataByObjectId(Int32 ObjectId, Int32 ObjectTypeId);
        Boolean IsRuleAssociationExists(Int32 AffectedRuleId);
        CompliancePackage GetCompliancePackageByPackageId(Int32 packageId);
    }
}
