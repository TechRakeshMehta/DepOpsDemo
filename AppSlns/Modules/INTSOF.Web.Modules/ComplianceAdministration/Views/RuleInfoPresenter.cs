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
    public class RuleInfoPresenter : Presenter<IRuleInfoView>
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
            View.CurrentViewContext.lstRuleTemplates = RuleManager.GetRuleTemplates(View.CurrentViewContext.SelectedTenantId);
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
            return RuleManager.GetRuleObjectMappingType(View.CurrentViewContext.SelectedTenantId,false);
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
            return RuleManager.getConstantType(View.CurrentViewContext.SelectedTenantId,false).ToList();
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
            if (RuleManager.UpdateRuleMapping(ruleMapping, View.SelectedTenantId))
            {
                return ruleMapping.RLM_ID;
            }
            return null;
        }

        public Boolean IsEditRuleAllowed()
        {
            if (View.SelectedTenantId == SecurityManager.DefaultTenantID)
            {
                return false;
            }
            else
            {
                return RuleManager.IsRuleAlreadyInUse(View.RuleMappingId, View.SelectedTenantId);
            }

        }

        public lkpConstantType getConstantTypeByCode(String constantCode)
        {
            return RuleManager.getConstantTypeByCode(constantCode, View.SelectedTenantId);
        }

        public lkpConstantType getConstantTypeCodeById(Int32? constantTypeId)
        {
            return RuleManager.getConstantTypeById(constantTypeId.Value, View.SelectedTenantId);
        }

        public List<UserGroup> getAllUsergroups()
        {
            return ComplianceSetupManager.GetAllUserGroup(View.SelectedTenantId).OrderBy(ex => ex.UG_Name).ToList();
        }

        public Boolean SaveRuleMapping(RuleMapping ruleMapping)
        {
            View.PreviousRuleMappingId = View.RuleMappingId;
            View.RuleMappingId = 0;
            ruleMapping.RLM_ID = View.RuleMappingId;
            if (View.FirstVersionRuleId == null || View.FirstVersionRuleId == AppConsts.NONE)
            {
                View.FirstVersionRuleId = View.PreviousRuleMappingId;
            }
            ruleMapping.RLM_FirstVersionID = View.FirstVersionRuleId;
            ruleMapping.RLM_CreatedByID = View.CurrentLoggedInUserId;
            ruleMapping.RLM_CreatedOn = DateTime.Now;
            ruleMapping.RLM_Code = Guid.NewGuid();
            if (RuleManager.AddRuleMapping(ruleMapping, View.SelectedTenantId))
            {
                View.CurrentViewContext.RuleMappingId = ruleMapping.RLM_ID;
                return true;
            }
            return false;
        }

        public void CreateXmlForVersionSettings(Int32 packageId, String userGroupIdList)
        {
            XElement root = new XElement("root");
            XElement row = null;
            row = new XElement("PackageId");
            row.Value = packageId.ToString();
            root.Add(row);
            if (View.FirstVersionRuleId != null)
            {
                row = new XElement("FirstVersionRuleId");
                row.Value = View.FirstVersionRuleId.ToString();
                root.Add(row);
            }

            row = new XElement("CurrentVersionRuleId");
            row.Value = View.RuleMappingId.ToString();
            root.Add(row);

            row = new XElement("CurrentUserId");
            row.Value = View.CurrentLoggedInUserId.ToString();
            root.Add(row);

            row = new XElement("IsVersionUpdate");
            row.Value = View.IsVersionUpdate.ToString();
            root.Add(row);

            row = new XElement("IsNewSelected");
            row.Value = View.IsNewSelected.ToString();
            root.Add(row);

            row = new XElement("IsExistingSelected");
            row.Value = View.IsExistingSelected.ToString();
            root.Add(row);

            row = new XElement("IsAllSelected");
            row.Value = View.IsAllSelected.ToString();
            root.Add(row);
            row = new XElement("UserGroupIdList");
            row.Value = userGroupIdList;
            root.Add(row);

            View.SettingXml = root.ToString();
        }

        public void DeactivatePreviousRulesAndCreateNewRule(String settingXml)
        {
            View.IsScheduleActionRecordInserted = RuleManager.DeactivatePreviousRulesAndCreateNewRule(settingXml, View.CurrentLoggedInUserId, View.SelectedTenantId);
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

        public List<lkpRuleImpactGroup> getImpactedUserGroupType()
        {
            return RuleManager.getImpactedUserGroupType(View.SelectedTenantId);
        }

        public List<RuleImpactGroupMapping> getPreviousImpactedGroupMappings()
        {
            return RuleManager.getPreviousImpactedGroupMappings(View.RuleMappingId, View.SelectedTenantId);
        }
        public void updateRuleImpactedGroupMappings(List<RuleImpactGroupMapping> impactedGroupMappings)
        {
            RuleManager.updateRuleImpactedGroupMappings(impactedGroupMappings, View.RuleMappingId, View.CurrentLoggedInUserId, View.SelectedTenantId);
        }

        public void GetListOfInstanceWichCanShareRuleOnEdit()
        {
            //as for master database we do not need rule sharing.
            if (View.SelectedTenantId == SecurityManager.DefaultTenantID)
            {
                View.RuleSynchronisationData.IfRuleIsAlreadyShared = false;
            }
            else
                View.RuleSynchronisationData = RuleManager.GetListOfInstanceWichCanShareRuleOnEdit(View.RuleSetId, View.RuleMappingId, View.SelectedTenantId);
        }

        public void ComplianceRuleSynchronisationonRuleEdit(List<Int32> packageList)
        {
            Boolean isScheduleActionRecordInserted;
            if (View.IsVersionUpdate)
                View.ErrMsg= RuleManager.ComplianceRuleSynchronisationonRuleEdit(View.SelectedTenantId, packageList, View.PreviousRuleMappingId, View.CurrentLoggedInUserId, View.SettingXml, View.IsVersionUpdate, View.IfUpdateAllIsSelected, View.RuleMappingId, out isScheduleActionRecordInserted);
            else
                View.ErrMsg = RuleManager.ComplianceRuleSynchronisationonRuleEdit(View.SelectedTenantId, packageList, View.RuleMappingId, View.CurrentLoggedInUserId, View.SettingXml, View.IsVersionUpdate, View.IfUpdateAllIsSelected, null, out isScheduleActionRecordInserted);
            if (isScheduleActionRecordInserted)
                View.IsScheduleActionRecordInserted = isScheduleActionRecordInserted;
        }

        public void InsertSystemServiceTrigger()
        {
            RuleManager.InsertSystemSeriveTrigger(View.CurrentLoggedInUserId, View.SelectedTenantId);
        }

        public Boolean IsDefaultTenant()
        {
            return View.SelectedTenantId.Equals(SecurityManager.DefaultTenantID);
        }

        #region UAT-5198
        /// <summary>
        /// Check whether rule object categories are available in selected packages.
        /// </summary>
        public Dictionary<String, String> IsCategoriesAvailableinSelectedPackages(List<Int32> lstPacakgeIds, List<Int32> lstCategoryIds, List<Tuple<Int32, Int32, Int32>> tuples)
        {
            return ComplianceSetupManager.IsCategoriesAvailableinSelectedPackages(lstPacakgeIds, lstCategoryIds, tuples, View.SelectedTenantId);
        }

        public AssignmentHierarchy GetAssignmentHierarchyByRuleSetId(int ruleSetId)
        {
            return ComplianceSetupManager.GetAssignmentHierarchyByRuleSetId(ruleSetId, View.SelectedTenantId);
        }

        public List<CompliancePackage> GetListOfInstanceWhichCanShareRule(int RuleSetId, string HID)
        {
            return RuleManager.GetListOfInstanceWichCanShareRule(RuleSetId, View.SelectedTenantId, HID);
        }

        #endregion
    }
}




