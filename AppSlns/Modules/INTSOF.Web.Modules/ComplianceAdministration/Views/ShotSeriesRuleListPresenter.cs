using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Entity.ClientEntity;
using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System.Xml.Linq;
using System.Linq;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class ShotSeriesRuleListPresenter : Presenter<IShotSeriesRuleListView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceAdministrationController _controller;
        // public RuleListPresenter([CreateNew] IComplianceAdministrationController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        //public List<ComplianceCategoryItem> GetComplianceItem(Int32 cagteogryId)
        //{
        //    return ComplianceSetupManager.GetComplianceCategoryItems(cagteogryId, View.SelectedTenantId);
        //}

        public List<RuleSetTree> GetRuleSetTreeData()
        {
            return RuleManager.GetRuleSetTreeData(View.SelectedTenantId);
        }

        public List<lkpObjectType> GetObjectTypeList()
        {
            return RuleManager.GetObjectTypeList(View.SelectedTenantId);
        }

        public lkpObjectType GetObjectTypeByCode(String ot_Code)
        {
            return RuleManager.GetObjectType(ot_Code, View.SelectedTenantId);
        }

        public lkpRuleObjectMappingType GetRuleObjectMappingTypeByCode(String rmt_Code)
        {
            return RuleManager.GetRuleObjectMappingType(rmt_Code, View.SelectedTenantId);
        }

        public List<lkpRuleObjectMappingType> GetRuleObjectMappingType()
        {
            return RuleManager.GetRuleObjectMappingType(View.CurrentViewContext.SelectedTenantId, true);
        }


        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public void SaveRuleMapping(RuleMapping ruleMapping)
        {
            Boolean isCreatedByAdmin = View.TenantId == SecurityManager.DefaultTenantID ? true : false;
            RuleManager.AddShotSeriesRuleMapping(ruleMapping, View.SelectedTenantId, View.SelectedObjectTypeCode, View.SelectedObjectIds, View.CurrentLoggedInUserId, isCreatedByAdmin);
            View.RuleMappingId = ruleMapping.RLM_ID;
        }

        public void GetRuleTemplates()
        {
            View.CurrentViewContext.lstRuleTemplates = RuleManager.GetRuleTemplates(View.CurrentViewContext.SelectedTenantId, true);
        }

        public void GetRuleTemplateDetails()
        {
            View.CurrentViewContext.RuleTemplateDetails = RuleManager.GetRuleTemplateDetails(View.CurrentViewContext.SelectedRuleTemplateId, View.CurrentViewContext.SelectedTenantId);
        }

        public RuleProcessingResult ValidateRule(String ruleTemplateXml, String ruleExpressionXml)
        {

            return RuleManager.ValidateExpression(ruleTemplateXml, ruleExpressionXml, View.SelectedTenantId);
        }

        public List<ComplianceAttribute> getAttributeDetail(List<Int32> objectIds)
        {
            return RuleManager.getAttributeDetail(objectIds, View.SelectedTenantId);
        }

        public String RuleObjectMappingTypeCodebyId(Int32 rmtID)
        {
            return RuleManager.RuleObjectMappingTypeCodebyId(rmtID, View.SelectedTenantId);
        }

        public void GetRuleMappings()
        {
            View.CurrentViewContext.lstRuleMapping = RuleManager.GetShotSeriesRuleMappings(View.SeriesId, View.CurrentViewContext.SelectedTenantId);
        }

        public Boolean DeleteRuleMapping()
        {
            RuleManager.DeleteShotSeriesRuleMapping(View.CurrentViewContext.RuleMappingId, View.CurrentViewContext.CurrentLoggedInUserId, View.CurrentViewContext.SelectedTenantId);
            return true;
        }

        public List<lkpConstantType> getConstantType()
        {
            return RuleManager.getConstantType(View.CurrentViewContext.SelectedTenantId,true).ToList();
        }

        public lkpConstantType getConstantTypeByCode(String constantCode)
        {
            return RuleManager.getConstantTypeByCode(constantCode, View.SelectedTenantId);
        }

        public lkpConstantType getConstantTypeCodeById(Int32? constantTypeId)
        {
            return RuleManager.getConstantTypeById(constantTypeId.Value, View.SelectedTenantId);
        }

        public List<ItemSeriesItem> GetItemSeriesItemList()
        {
            return RuleManager.GetSeriesItemList(View.SeriesId, View.SelectedTenantId);
        }

        public List<ItemSeriesAttribute> GetItemSeriesAttributeBySeriesId()
        {
            return RuleManager.GetItemSeriesAttributeBySeriesId(View.SeriesId, View.SelectedTenantId);
        }

    }
}




