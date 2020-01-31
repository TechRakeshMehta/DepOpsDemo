using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Linq;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class RuleTemplateCopyPresenter : Presenter<IRuleTemplateCopyView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
            // TODO: Implement code that will be executed the first time the view loads
        }

        public bool CopyRuleTemplate()
        {
            return RuleManager.CopyRuleTemplate(View.FromTenantID, View.ToTenantID, View.RuleTemplateName, View.CurrentLoggedInUserId, View.SelectedRuleTemplateID);
        }

        public void GetTenants()
        {
            Boolean SortByName = true;
            String tenantTypeCodeForClient = TenantType.Institution.GetStringValue();
            Int32 defaultTenantId = SecurityManager.DefaultTenantID;
            View.ListTenants = SecurityManager.GetTenants(SortByName, false, tenantTypeCodeForClient).Where(obj => obj.TenantID != View.DefaultTenantId).ToList();
        }
    }
}
