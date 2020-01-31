using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public class RulesetListBkgPresenter : Presenter<IRulesetListBkgView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetRuleSetsForObject()
        {
            if (View.ObjectId != null)
                View.RuleSetList = BackgroungRuleManager.GetRuleSetForObject(View.ObjectId, View.ObjectTypeId, View.SelectedTenantId);
            else
                View.RuleSetList = new List<BkgRuleSet>();
        }

        /// <summary>
        /// Adds the Rule set to the selected Object after saving the new rule set added(if any).
        /// </summary>
        public void SaveNewRuleSet()
        {
            BkgRuleSet newRuleSet = new BkgRuleSet
            {
                BRLS_Name = View.ViewContract.Name,
                BRLS_Description = View.ViewContract.Description,
                //todo
                BRLS_ObjectTypeID = View.ObjectTypeId,
                BRLS_ObjectID = View.ObjectId,
                BRLS_IsActive = View.ViewContract.IsActive,
                BRLS_TenantID = View.SelectedTenantId,
                BRLS_IsCreatedByAdmin = View.TenantId == SecurityManager.DefaultTenantID ? true : false
            };
            View.ViewContract.RuleSetId = BackgroungRuleManager.SaveComplianceRuleSetDetail(newRuleSet, View.CurrentLoggedInUserId, View.SelectedTenantId).BRLS_ID;
        }


        public Boolean DeleteRuleSetObjectMapping()
        {
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IfObjectRuleSetMappingCanBeDeleted(View.ViewContract.RuleSetId, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                BkgRuleSet ruleSet = BackgroungRuleManager.GetRuleSetInfoByID(View.ViewContract.RuleSetId, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, ruleSet.BRLS_Name);
                return false;
            }
            else
            {
                BackgroungRuleManager.DeleteRuleSet(View.ViewContract.RuleSetId, View.CurrentLoggedInUserId, View.SelectedTenantId);
                return true;
            }
        }
    }
}
