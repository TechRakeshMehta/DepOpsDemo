using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoreWeb.RotationPackages.Views
{
    public class RequirementRuleTemplateCopyPresenter : Presenter<IRequirementRuleTemplateCopy>
    {
        public bool CopyRuleTemplate()
        {
            return RequirementRuleManager.CopyRuleTemplate(View.RuleTemplateName, View.CurrentLoggedInUserId, View.SelectedRuleTemplateID);
        }
    }
}
