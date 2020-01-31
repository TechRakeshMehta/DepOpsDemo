using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public class RuleInfoBkgPresenter : Presenter<IRuleInfoBkgView>
    {
        public void GetRuleTemplates()
        {
            View.CurrentViewContext.lstRuleTemplates = BackgroungRuleManager.GetRuleTemplateList(View.CurrentViewContext.SelectedTenantId).OrderBy(col=>col.BRLT_Name).ToList();
        }

        public void GetRuleInfo()
        {
            View.CurrentViewContext.RuleMapping = BackgroungRuleManager.GetRuleMapping(View.CurrentViewContext.RuleMappingId, View.CurrentViewContext.SelectedTenantId, true);
        }

        public void GetRuleType()
        {
            View.CurrentViewContext.RuleTypes = BackgroungRuleManager.GetRuleTypes(View.CurrentViewContext.SelectedTenantId);
        }

        public void GetRuleActionType()
        {
            View.CurrentViewContext.RuleActionTypes = BackgroungRuleManager.GetRuleActionTypes(View.CurrentViewContext.SelectedTenantId);
        }

        public void GetRuleTemplateDetails()
        {
            View.CurrentViewContext.RuleTemplateDetails = BackgroungRuleManager.GetRuleTemplateDetails(View.CurrentViewContext.SelectedRuleTemplateId, View.CurrentViewContext.SelectedTenantId);
        }

        public List<lkpBkgRuleObjectMappingType> GetRuleObjectMappingType()
        {
            return BackgroungRuleManager.getRuleObjectMappingType(View.CurrentViewContext.SelectedTenantId);
        }

        public List<lkpBkgObjectType> GetObjectTypeList()
        {
            return BackgroungRuleManager.getBkgObjectType(View.CurrentViewContext.SelectedTenantId);
        }

        public List<lkpBkgConstantType> GetConstantType()
        {
            return BackgroungRuleManager.getConstantType(View.CurrentViewContext.SelectedTenantId);
        }

        public Boolean SaveRuleMapping(BkgRuleMapping ruleMapping)
        { 
            return BackgroungRuleManager.AddRuleMapping(ruleMapping, View.SelectedTenantId,View.CurrentLoggedInUserId);
        }

        public RuleProcessingResult ValidateRule(String ruleTemplateXml, String ruleExpressionXml)
        {
            return RuleManager.ValidateExpression(ruleTemplateXml, ruleExpressionXml, View.SelectedTenantId);
        }
    }
}
