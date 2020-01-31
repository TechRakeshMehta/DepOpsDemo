using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Linq;
using System.Xml.Linq;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class ShotSeriesRuleInfoPresenter : Presenter<IShotSeriesRuleInfoView>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public void GetRuleTemplates()
        {
            View.CurrentViewContext.lstRuleTemplates = RuleManager.GetRuleTemplates(View.CurrentViewContext.SelectedTenantId, true);
        }

        public void getRuleInfo()
        {
            View.CurrentViewContext.RuleMapping = RuleManager.GetRuleMapping(View.CurrentViewContext.RuleMappingId, View.CurrentViewContext.SelectedTenantId, true);
        }

        public void GetRuleTemplateDetails()
        {
            View.CurrentViewContext.RuleTemplateDetails = RuleManager.GetRuleTemplateDetails(View.CurrentViewContext.SelectedRuleTemplateId, View.CurrentViewContext.SelectedTenantId);
        }

        public String RuleObjectMappingTypeCodebyId(Int32 rmtID)
        {
            return RuleManager.RuleObjectMappingTypeCodebyId(rmtID, View.SelectedTenantId);
        }

        public List<lkpRuleObjectMappingType> GetRuleObjectMappingType()
        {
            return RuleManager.GetRuleObjectMappingType(View.CurrentViewContext.SelectedTenantId, true);
        }

        //public RuleMappingObjectTree GetRuleMapingObjectTree(Int32 ruleMappingDetailId, RuleSetTreeType ruleSetTreeType)
        //{
        //    RuleSetTree ruleSetTree = RuleManager.GetRuleSetTreeByUICode(ruleSetTreeType.GetStringValue(), View.SelectedTenantId);
        //    return RuleManager.GetRuleMapingObjectTree(ruleMappingDetailId, ruleSetTree.RST_ID, View.SelectedTenantId);
        //}

        public lkpObjectType GetObjectTypeyId(Int32 ot_ID)
        {
            return RuleManager.GetObjectTypeById(ot_ID, View.SelectedTenantId);
        }

        public lkpObjectType GetObjectTypeByCode(String ot_Code)
        {
            return RuleManager.GetObjectType(ot_Code, View.SelectedTenantId);
        }

        public List<lkpObjectType> GetObjectTypeList()
        {
            return RuleManager.GetObjectTypeList(View.SelectedTenantId);
        }

        public List<lkpConstantType> getConstantType()
        {
            return RuleManager.getConstantType(View.CurrentViewContext.SelectedTenantId,true).ToList();
        }

        public lkpRuleObjectMappingType GetRuleObjectMappingTypeByCode(String rmt_Code)
        {
            return RuleManager.GetRuleObjectMappingType(rmt_Code, View.SelectedTenantId);
        }

        public List<RuleSetTree> GetRuleSetTreeData()
        {
            return RuleManager.GetRuleSetTreeData(View.SelectedTenantId);
        }

        public RuleProcessingResult ValidateRule(String ruleTemplateXml, String ruleExpressionXml)
        {
            return RuleManager.ValidateExpression(ruleTemplateXml, ruleExpressionXml, View.SelectedTenantId);
        }

        public List<ComplianceAttribute> getAttributeDetail(List<Int32> objectIds)
        {
            return RuleManager.getAttributeDetail(objectIds, View.SelectedTenantId);
        }

        public Int32? UpdateRuleMapping(RuleMapping ruleMapping)
        {
            ruleMapping.RLM_ModifiedByID = View.CurrentLoggedInUserId;
            ruleMapping.RLM_ModifiedOn = DateTime.Now;
            if (RuleManager.UpdateShotSeriesRuleMapping(ruleMapping, View.SelectedObjectIds, View.SelectedObjectTypeCode, View.CurrentLoggedInUserId, View.SelectedTenantId))
            {
                return ruleMapping.RLM_ID;
            }
            return null;
        }

        public lkpConstantType getConstantTypeByCode(String constantCode)
        {
            return RuleManager.getConstantTypeByCode(constantCode, View.SelectedTenantId);
        }

        public lkpConstantType getConstantTypeCodeById(Int32? constantTypeId)
        {
            return RuleManager.getConstantTypeById(constantTypeId.Value, View.SelectedTenantId);
        }

        public Boolean IsAnySubscriptionExist(Int32 packageId)
        {
            if (View.SelectedTenantId == SecurityManager.DefaultTenantID)
            {
                return false;
            }
            else
            {
                return ComplianceDataManager.IsAnySubscriptionExist(packageId, View.SelectedTenantId);
            }

        }

        public void InsertSystemServiceTrigger()
        {
            RuleManager.InsertSystemSeriveTrigger(View.CurrentLoggedInUserId, View.SelectedTenantId);
        }

        public Boolean IsDefaultTenant()
        {
            return View.SelectedTenantId.Equals(SecurityManager.DefaultTenantID);
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




