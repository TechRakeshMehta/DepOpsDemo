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

namespace CoreWeb.BkgSetup.Views
{
    public class RuleListBkgPresenter : Presenter<IRuleListBkgView>
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

        public void GetRuleMappings()
        {
            View.CurrentViewContext.lstRuleMapping = BackgroungRuleManager.GetRuleMappings(View.RuleSetId, View.CurrentViewContext.SelectedTenantId);
        }

        public Boolean DeleteRuleMapping()
        {
            //IntegrityCheckResponse response = IntegrityManager.IfRuleSetRuleMappingCanBeDeleted(View.CurrentViewContext.RuleMappingId, View.SelectedTenantId);
            //if (response.CheckStatus == CheckStatus.True)
            //{
            //    RuleMapping ruleMapping = RuleManager.GetRuleMapping(View.CurrentViewContext.RuleMappingId, View.SelectedTenantId);
            //    View.ErrorMessage = String.Format(response.UIMessage, ruleMapping.RLM_Name);
            //    return false;
            //}
            //else
            //{
               return BackgroungRuleManager.DeleteRuleMapping(View.CurrentViewContext.RuleMappingId, View.CurrentViewContext.CurrentLoggedInUserId, View.CurrentViewContext.SelectedTenantId);
            //}
        }
    }
}




