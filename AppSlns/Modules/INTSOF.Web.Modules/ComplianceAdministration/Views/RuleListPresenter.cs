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
    public class RuleListPresenter : Presenter<IRuleListView>
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
            return RuleManager.GetRuleObjectMappingType(View.CurrentViewContext.SelectedTenantId,false);
        }


        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public void SaveRuleMapping(RuleMapping ruleMapping)
        {
            RuleManager.AddRuleMapping(ruleMapping, View.SelectedTenantId);
            View.RuleMappingId = ruleMapping.RLM_ID;
        }

        public void GetRuleTemplates()
        {
            View.CurrentViewContext.lstRuleTemplates = RuleManager.GetRuleTemplates(View.CurrentViewContext.SelectedTenantId,false);
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
            //Commented in optimization//
            //View.CurrentViewContext.lstRuleMapping = RuleManager.GetRuleMappings(View.RuleSetId, View.CurrentViewContext.SelectedTenantId);

            View.lstRuleMapping = RuleManager.GetRuleMappings(View.RuleSetId, View.CurrentViewContext.SelectedTenantId);
        }

        public Boolean DeleteRuleMapping()
        {
            RuleManager.DeleteRuleMapping(View.CurrentViewContext.RuleMappingId, View.CurrentViewContext.CurrentLoggedInUserId, View.CurrentViewContext.SelectedTenantId);
            return true;
        }

        public List<lkpConstantType> getConstantType()
        {
            return RuleManager.getConstantType(View.CurrentViewContext.SelectedTenantId,false).ToList();
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

        public void DeactivatePreviousRulesAndCreateNewRule(String SettingXml)
        {
           View.IsScheduleActionRecordInserted= RuleManager.DeactivatePreviousRulesAndCreateNewRule(SettingXml, View.CurrentLoggedInUserId, View.SelectedTenantId);
        }

        public String CreateXmlForVersionSettings(Int32 packageId, String userGroupIdList)
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

            return root.ToString();
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

        public void GetListOfInstanceWichCanShareRule()
        {
            //as for master database we do not need rule sharing.
            if (View.SelectedTenantId == SecurityManager.DefaultTenantID)
            {
                View.PackageListForSharingRuleInstance = null;
            }
            else
            {
                String HID = String.Empty;
                if (View.ObjectType == "ATR")
                {
                    HID = "1-" + View.PackageId + "|2-" + View.CurrentCategoryID + "|3-" + View.CurrentItemID + "|4-" + View.ObjectId;
                }
                else if(View.ObjectType == "ITM")
                {
                    HID = "1-" + View.PackageId + "|2-" + View.CurrentCategoryID + "|3-" + View.ObjectId;
                }
                else if (View.ObjectType == "CAT")
                {
                    HID = "1-" + View.PackageId + "|2-" + View.ObjectId;
                }
                View.PackageListForSharingRuleInstance = RuleManager.GetListOfInstanceWichCanShareRule(View.RuleSetId, View.SelectedTenantId, HID);
            }
        }

        public void ComplianceRuleSynchronisation(List<Int32> packageList, String settingParameters, Boolean sourceRuleHasSubscription)
        {
            Boolean isScheduleActionRecordInserted;
            RuleManager.ComplianceRuleSynchronisation(packageList, View.RuleMappingId, View.CurrentLoggedInUserId, settingParameters, View.SelectedTenantId, sourceRuleHasSubscription, out isScheduleActionRecordInserted);
            if (isScheduleActionRecordInserted)
                View.IsScheduleActionRecordInserted = isScheduleActionRecordInserted;
        }

        public void InsertSystemServiceTrigger()
        {
            RuleManager.InsertSystemSeriveTrigger(View.CurrentLoggedInUserId, View.SelectedTenantId);
        }
        public void GetRuleSetDataByObjectId()
        {
            View.lstRuleSetAssociationData = RuleManager.GetRuleSetDataByObjectId(View.SelectedTenantId, View.ObjectId, View.ObjectType);
        }
        public Boolean IsRuleAssociationExists(Int32 ruleMappingId)
        {
            if (View.SelectedTenantId == SecurityManager.DefaultTenantID)
            {
                return false;
            }
            else
            {
                //GetRuleSetDataByObjectId(); //Changed
                return RuleManager.IsRuleAssociationExists(View.SelectedTenantId, ruleMappingId);
            }
        }

        /// <summary>
        /// Adds the Rule set to the selected Object after saving the new rule set added(if any).
        /// </summary>
        public void SaveNewRuleSet()
        {
            RuleSet newRuleSet = new RuleSet
            {
                RLS_Name = "Rule Set",
                // RLS_RuleSetType = View.ViewContract.RuleSetTypeID,
                RLS_IsActive = true,
                RLS_Description = "Rule Set",
                RLS_TenantID = View.SelectedTenantId,
                RLS_IsCreatedByAdmin = View.TenantId == SecurityManager.DefaultTenantID ? true : false
            };
            Int32? associationHierarchyId = null;
            associationHierarchyId = getAssociationHierarchy();
            newRuleSet.RLS_AssignmentHierarchyID = associationHierarchyId;
            View.RuleSetId = ComplianceSetupManager.SaveComplianceRuleSetDetail(newRuleSet, View.CurrentLoggedInUserId, View.SelectedTenantId).RLS_ID;
        }

        private Int32? getAssociationHierarchy()
        {
            Int32? associationHierarchyId = null;
            if (View.ObjectType == LCObjectType.ComplianceCategory.GetStringValue())
            {
                associationHierarchyId = ComplianceSetupManager.getAssociationHierarchyIdForObject(View.CurrentLoggedInUserId, View.SelectedTenantId, View.PackageId, View.ObjectId);
            }
            else if (View.ObjectType == LCObjectType.ComplianceItem.GetStringValue())
            {
                associationHierarchyId = ComplianceSetupManager.getAssociationHierarchyIdForObject(View.CurrentLoggedInUserId, View.SelectedTenantId, View.PackageId, View.CurrentCategoryID, View.ObjectId);
            }
            else if (View.ObjectType == LCObjectType.ComplianceATR.GetStringValue())
            {
                associationHierarchyId = ComplianceSetupManager.getAssociationHierarchyIdForObject(View.CurrentLoggedInUserId, View.SelectedTenantId, View.PackageId, View.CurrentCategoryID, View.CurrentItemID, View.ObjectId);
            }
            return associationHierarchyId;
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
        #endregion
    }
}




