using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.RotationPackages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public class AddNewRequirementRulePresenter : Presenter<IAddNewRequrementRule>
    {

        public override void OnViewInitialized()
        {
            View.RuleTypes = RequirementRuleManager.GetRuleTypes();
            View.RuleActionTypes = RequirementRuleManager.GetRuleActionTypes();
        }

        public void GetRuleTemplates()
        {
            View.CurrentViewContext.lstRuleTemplates = RequirementRuleManager.GetRuleTemplates();
        }

        public void GetRuleTemplateDetails()
        {
            View.CurrentViewContext.RuleTemplateDetails = RequirementRuleManager.GetRuleTemplateDetails(View.SelectedRuleTemplateId);
        }

        public List<lkpRuleObjectMappingType> GetRuleObjectMappingType()
        {
            return RequirementRuleManager.GetRuleObjectMappingType();
        }

        public List<lkpObjectType> GetObjectTypeList()
        {
            return RequirementRuleManager.GetObjectTypeList();
        }

        public RequirementObjectTree GetRequirementObjectTree(Int32 ObjectTreeID, Int32 objectTypeID, String objectTypeCode = "")
        {
            return RequirementRuleManager.GetRequirementObjectTree(ObjectTreeID, objectTypeID, objectTypeCode);
        }

        public List<lkpConstantType> GetConstantType()
        {
            return RequirementRuleManager.GetConstantType().ToList();
        }

        public void SaveRuleMapping(RequirementObjectRule ruleMapping)
        {
            RequirementRuleManager.AddRuleMapping(ruleMapping);
            View.RequirementObjectRuleMappingId = ruleMapping.ROR_ID;
        }

        public void GetRuleInfo()
        {
            View.RequirementObjectRule = RequirementRuleManager.GetRuleMapping(View.ObjectRuleID);
        }

        public void UpdateRuleMapping(RequirementObjectRule requirementObjectRule)
        {
            requirementObjectRule.ROR_ModifiedByID = View.CurrentLoggedInUserId;
            requirementObjectRule.ROR_ModifiedOn = DateTime.Now;
            RequirementRuleManager.UpdateRequirementRuleMapping(requirementObjectRule);
        }

        public List<RequirementExpressionData> GetAttributeDetail(List<Int32> objectIds)
        {
            return RequirementRuleManager.GetAttributeDetail(objectIds);
        }

        public RuleProcessingResult ValidateRule(string ruleTemplateXml, string ruleExpressionXml)
        {
            return RequirementRuleManager.ValidateExpression(ruleTemplateXml, ruleExpressionXml);
        }
    }
}
