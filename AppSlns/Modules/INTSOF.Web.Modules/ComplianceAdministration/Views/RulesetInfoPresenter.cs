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
    public class RulesetInfoPresenter : Presenter<IRulesetInfoView>
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

        public void GetRuleSetInfoByID()
        {
            View.ComplianceRuleSet = ComplianceSetupManager.GetRuleSetInfoByID(View.CurrentRuleSetId, View.SelectedTenantId);
        }

        public void GetRuleSetTypeList()
        {
            View.RuleSetType = ComplianceSetupManager.GetRuleSetTypeList(View.SelectedTenantId);
        }

        public void UpdateRuleSetDetail()
        {
            RuleSet newRuleSet = new RuleSet
                {
                    RLS_ID= View.ViewContract.RuleSetId,
                    RLS_Name = View.ViewContract.Name,
                   // RLS_RuleSetType = View.ViewContract.RuleSetTypeID,
                    RLS_IsActive = View.ViewContract.IsActive,
                    RLS_Description = View.ViewContract.Description,
                    RLS_TenantID = View.SelectedTenantId
                };
            ComplianceSetupManager.UpdateComplianceRuleSetDetail(newRuleSet, View.CurrentLoggedInUserId, View.SelectedTenantId);
        }
    }
}




