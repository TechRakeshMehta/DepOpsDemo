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

namespace CoreWeb.RotationPackages.Views
{
    public class RequirementRuleSetUpPresenter : Presenter<IRequirementRuleSetUp>
    { 
        public void GetRuleMappings()
        {
            View.lstRuleMapping = RequirementRuleManager.GetRuleMappings(View.ParentObjectTreeID);
        }

        public bool DeleteRequirementRuleMapping()
        {
            RequirementRuleManager.DeleteRequirementRuleMapping(View.ObjectRuleID, View.CurrentViewContext.CurrentLoggedInUserId);
            return true;
        }
    }
}
