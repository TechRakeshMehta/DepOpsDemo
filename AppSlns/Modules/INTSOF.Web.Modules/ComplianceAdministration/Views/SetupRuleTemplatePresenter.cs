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

using Entity;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.Utils.Consts;

#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public class SetupRuleTemplatePresenter : Presenter<ISetupRuleTemplateView>
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

            View.RuleTemplates = RuleManager.GetRuleTemplates(View.SelectedTenantId);
        }

        public Boolean DeleteRuleTemplate(Int32 ruleId)
        {
            IntegrityCheckResponse response = IntegrityManager.IfRuleTemplateCanBeDeleted(ruleId, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                Entity.ClientEntity.RuleTemplate ruleTemplate = RuleManager.GetRuleTemplateDetails(ruleId, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, ruleTemplate.RLT_Name);
                return false;
            }
            else
            {
                RuleManager.DeleteRuleTemplate(ruleId, View.currentloggedinuserId, View.SelectedTenantId);
                return true;
            }
        }
        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.currentloggedinuserId).Organization.TenantID.Value;
        }

        public void GetTenants()
        {
            View.ListTenants = ComplianceDataManager.GetMasterAndInstitutionTypeTenants(View.DefaultTenantId);
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




