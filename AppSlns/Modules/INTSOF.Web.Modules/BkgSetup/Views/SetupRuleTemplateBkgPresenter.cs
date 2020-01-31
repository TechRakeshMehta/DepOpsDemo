#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SetupRuleTemplateBkgPresenter.cs
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

namespace CoreWeb.BkgSetup.Views
{
    public class SetupRuleTemplateBkgPresenter : Presenter<ISetupRuleTemplateBkgView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
            // TODO: Implement code that will be executed the first time the view loads
        }

        public override void OnViewLoaded()
        {

        }

        public void GetRuleTemplates()
        {
            View.RuleTemplates = BackgroundSetupManager.GetRuleTemplates(View.SelectedTenantId);
        }

        public Boolean DeleteRuleTemplate(Int32 ruleId)
        {
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IfRuleTemplateCanBeDeleted(ruleId, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                Entity.ClientEntity.BkgRuleTemplate ruleTemplate = BackgroundSetupManager.GetRuleTemplateByID(ruleId, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, ruleTemplate.BRLT_Name);
                return false;
            }
            else
            {
                BackgroundSetupManager.DeleteRuleTemplate(ruleId, View.currentloggedinuserId, View.SelectedTenantId);
                return true;
            }
        }
    
        public void GetTenants()
        {
            var tenants = ComplianceDataManager.GetMasterAndInstitutionTypeTenants(View.DefaultTenantId);
            if(tenants.IsNotNull())
            {
                View.ListTenants = tenants.Where(x => x.TenantID != View.DefaultTenantId).ToList();
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
