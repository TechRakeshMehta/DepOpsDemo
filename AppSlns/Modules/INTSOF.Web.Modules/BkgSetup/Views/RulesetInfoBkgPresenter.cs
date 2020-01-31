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
    public class RulesetInfoBkgPresenter : Presenter<IRulesetInfoBkgView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetRuleSetInfoByID()
        {
            View.BkgRuleSet = BackgroungRuleManager.GetRuleSetInfoByID(View.CurrentRuleSetId, View.SelectedTenantId);
        }

        public void UpdateRuleSetDetail()
        {
            BkgRuleSet newRuleSet = new BkgRuleSet
                {
                    BRLS_ID = View.ViewContract.RuleSetId,
                    BRLS_Name = View.ViewContract.Name,
                    BRLS_IsActive = View.ViewContract.IsActive,
                    BRLS_Description = View.ViewContract.Description,
                    BRLS_TenantID = View.SelectedTenantId
                };
            BackgroungRuleManager.UpdateComplianceRuleSetDetail(newRuleSet, View.CurrentLoggedInUserId, View.SelectedTenantId);
        }
    }
}




