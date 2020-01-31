#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SetupRuleTemplatePresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region Application Specific

using Entity.SharedDataEntity;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.Utils.Consts;

#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class MasterRequirementRuleTemplatePresenter : Presenter<IMasterRequirementRuleTemplateView>
    {
        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetRuleTemplates()
        {
            View.RuleTemplates = SharedRequirementRuleManager.GetlstRuleTemplate();
        }

        public Boolean DeleteRuleTemplate(Int32 ruleTemplateId)
        {
            IntegrityCheckResponse response = IntegrityManager.IfRuleTemplateCanBeDeleted(ruleTemplateId, 1);
            if (response.CheckStatus == CheckStatus.True)
            {
                RequirementRuleTemplate ruleTemplate = SharedRequirementRuleManager.GetRuleTemplateDetails(ruleTemplateId);
                View.ErrorMessage = String.Format(response.UIMessage, ruleTemplate.RLT_Name);
                return false;
            }
            else
            {
                SharedRequirementRuleManager.DeleteRuleTemplate(ruleTemplateId, View.CurrentloggedinUserId);
                return true;
            }
        }
       

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }
    }
}




