using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class RulesetListPresenter : Presenter<IRulesetListView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceAdministrationController _controller;
        // public RulesetListPresenter([CreateNew] IComplianceAdministrationController controller)
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
        }

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public void GetRuleSetsForObject()
        {
            Int32? associationHierarchyId = null;
            associationHierarchyId = getAssociationHierarchy();
            if(associationHierarchyId!=null)
                View.RuleSetList = ComplianceSetupManager.GetRuleSetForObject(associationHierarchyId.Value, View.ObjectTypeId, View.SelectedTenantId);
        }

        public void GetMasterRuleSetsForObject()
        {
            //View.complianceRuleSets = ComplianceSetupManager.GetMasterRuleSetsForObject(View.ObjectId, View.ObjectTypeId, View.SelectedTenantId);
        }

        public void GetRuleSetTypeList()
        {
            View.RuleSetType = ComplianceSetupManager.GetRuleSetTypeList(View.SelectedTenantId);
        }

        /// <summary>
        /// Adds the Rule set to the selected Object after saving the new rule set added(if any).
        /// </summary>
        public void SaveNewRuleSet()
        {
            RuleSet newRuleSet = new RuleSet
                {
                    RLS_Name = View.ViewContract.Name,
                    // RLS_RuleSetType = View.ViewContract.RuleSetTypeID,
                    RLS_IsActive = View.ViewContract.IsActive,
                    RLS_Description = View.ViewContract.Description,
                    RLS_TenantID = View.SelectedTenantId,
                    RLS_IsCreatedByAdmin = View.TenantId == SecurityManager.DefaultTenantID ? true : false
                };
            Int32? associationHierarchyId = null;
            associationHierarchyId = getAssociationHierarchy();
            newRuleSet.RLS_AssignmentHierarchyID = associationHierarchyId;
            View.ViewContract.RuleSetId = ComplianceSetupManager.SaveComplianceRuleSetDetail(newRuleSet, View.CurrentLoggedInUserId, View.SelectedTenantId).RLS_ID;
        }

        private Int32? getAssociationHierarchy()
        {
            Int32? associationHierarchyId = null;
            if (View.ObjectTypeCode == LCObjectType.ComplianceCategory.GetStringValue())
            {
                associationHierarchyId = ComplianceSetupManager.getAssociationHierarchyIdForObject(View.CurrentLoggedInUserId, View.SelectedTenantId, View.parentPackageId, View.ObjectId);
            }
            else if (View.ObjectTypeCode == LCObjectType.ComplianceItem.GetStringValue())
            {
                associationHierarchyId = ComplianceSetupManager.getAssociationHierarchyIdForObject(View.CurrentLoggedInUserId, View.SelectedTenantId, View.parentPackageId, View.parentCategoryId, View.ObjectId);
            }
            else if (View.ObjectTypeCode == LCObjectType.ComplianceATR.GetStringValue())
            {
                associationHierarchyId = ComplianceSetupManager.getAssociationHierarchyIdForObject(View.CurrentLoggedInUserId, View.SelectedTenantId, View.parentPackageId, View.parentCategoryId, View.parentItemId, View.ObjectId);
            }
            return associationHierarchyId;
        }

        public Boolean DeleteRuleSetObjectMapping()
        {
            IntegrityCheckResponse response = IntegrityManager.IfObjectRuleSetMappingCanBeDeleted(View.ViewContract.RuleSetId, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                RuleSet ruleSet = ComplianceSetupManager.GetRuleSetInfoByID(View.ViewContract.RuleSetId, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, ruleSet.RLS_Name);
                return false;
            }
            else
            {
                ComplianceSetupManager.DeleteRuleSet(View.ViewContract.RuleSetId, View.ObjectId, View.ObjectTypeId, View.CurrentLoggedInUserId, View.SelectedTenantId);
                return true;
            }
        }

        public String getObjectTypeCodeById(Int32 objectTypeId)
        {
            return RuleManager.GetObjectTypeById(objectTypeId, View.SelectedTenantId).OT_Code;
        }
    }
}




