using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using Entity.ClientEntity;
using INTSOF.UI.Contract.RotationPackages;

namespace DAL.Interfaces
{
    public interface IRequirementRuleRepository
    {
        #region Requirement Package Rules Execution

        /// <summary>
        /// Execute rules after saving Verification detail screen data.
        /// </summary>
        /// <param name="ruleObjectXML"></param>
        /// <param name="reqSubscriptionId"></param>
        /// <param name="systemUserId"></param>
        void ExecuteRequirementObjectBuisnessRules(String ruleObjectXML, Int32 reqSubscriptionId, Int32 systemUserId);

        void EvaluateRequirementPostSubmitRules(String ruleObjectXML, Int32 reqSubscriptionId, Int32 systemUserId);
        #endregion

        List<RequirementExpiryContract> SetRequirementItemExpiry(Int32 chunkSize, Int32 systemUserId);

        List<RequirementScheduledAction> GetActiveScheduleActionList(Int32 chunkSize, String actionType);

        List<RequirementPackageSubscriptionContract> ExecuteRequirementCategoryScheduleAction(Int32 systemUserId, Int32 chunkSize, String ScheduleActionTypeCode);

        List<RequirementPackageSubscriptionContract> ExecuteRequirementPackageScheduleAction(Int32 systemUserId, Int32 chunkSize);
        Boolean CopyRuleTemplate(Entity.SharedDataEntity.RequirementRuleTemplate ruleTemp, String RuleTemplateName, Int32 CurrentLoggedInUserId);
        #region Rule Mapping

        List<RequirementExpressionData> GetRequirementCategoryItems(Int32 categoryObjectTreeID);
        List<RequirementExpressionData> GetRequirementItemAttribute(Int32 itemObjectTreeID);
        List<Entity.SharedDataEntity.RequirementRuleTemplate> GetRuleTemplates();
        Entity.SharedDataEntity.RequirementRuleTemplate GetRuleTemplateDetails(Int32 ruleTemplateId);
        Entity.SharedDataEntity.RequirementObjectTree GetRequirementObjectTree(Int32 ObjectTreeID, Int32 objectTypeID, String objectTypeCode = "");
        Boolean AddRuleMapping(Entity.SharedDataEntity.RequirementObjectRule ruleMapping);
        Boolean UpdateRequirementRuleMapping(Entity.SharedDataEntity.RequirementObjectRule requirementObjectRule);
        List<RequirementExpressionData> GetRequirementCategoryByCategoryID(Int32 categoryId, String ObjectTypeCode);
        List<Entity.SharedDataEntity.RequirementObjectRule> GetRuleMappings(Int32 ParentObjectTreeID);
        Entity.SharedDataEntity.RequirementObjectRule GetRequirementRuleMapping(Int32 ObjectRuleID);
        void DeleteRequirementRuleMapping(Int32 ruleMappingId, Int32 currentUserId);
        List<RequirementExpressionData> GetAttributeDetail(List<Int32> lstFieldObjectTreeIDs);
        String ValidateExpression(String ruleTemplateXml, String ruleExpressionXml);
        List<RequirementExpressionData> GetRequirementSubmissionItemsByCategoryID(Int32 categoryId);
        #endregion  

        #region UAT-2514 Copy Package
        /// <summary>
        /// Copy Rules from Shared to Tenant
        /// </summary>
        /// <param name="oldRequirementCategoryID"></param>
        /// <param name="newRequirementCategoryID"></param>
        /// <param name="CurrentLoggedInOrgUserID"></param>
        void CopyRulesFromSharedToTenant(Int32 oldRequirementCategoryID, Int32 newRequirementCategoryID, Int32 CurrentLoggedInOrgUserID);

        #endregion 

        #region UAT 3080
        List<INTSOF.ServiceDataContracts.Modules.RequirementPackage.RequirementScheduleActionCategoryRulesContract> GetScheduleActionExecuteCategoryRulesList(int chunkSize);

        Boolean inactiveProcessedScheduleAction(Int32 scheduleActionId, Int32 systemUserId);
        #endregion

        #region UAT-4428
        List<RequirementObjectTree> GetReqObjectTreeList(List<Int32> lstObjectIds, Int32 objectTypeId);
        List<lkpRotScheduledActionType> GetRotScheduledActionTypes();

        Boolean SaveScheduledActions(List<RequirementScheduledAction> lstRequirementScheduledActions);
        #endregion
    }
}
