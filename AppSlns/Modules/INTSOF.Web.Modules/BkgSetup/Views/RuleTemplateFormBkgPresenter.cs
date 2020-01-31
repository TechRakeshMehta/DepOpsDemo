#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  RuleTemplateFormBkgPresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#region Application Specific

using Entity.ClientEntity;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using ExternalServiceProxy;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.SharedObjects;

#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public class RuleTemplateFormBkgPresenter : Presenter<IRuleTemplateFormBkgView>
    {
        public override void OnViewLoaded()
        {
            //todo
        }

        public override void OnViewInitialized()
        {
            View.RuleResultTypes = BackgroundSetupManager.GetRuleResultTypes(View.SelectedTenantId);
        }

        public void InitializeRuleTemplate()
        {
            if (View.RuleTemplateID == null || View.RuleTemplateID == 0)
            {
                View.CurrentRuleTemplate = new Entity.ComplianceRuleTemplate();
            }
            else
            {
                View.CurrentRuleTemplate = BackgroundSetupManager.GetRuleTemplate(View.RuleTemplateID, View.SelectedTenantId);
            }
        }

        public bool SaveRuleTemplate()
        {
            if (View.CurrentRuleTemplate != null)
            {
                if (View.CurrentRuleTemplate.RLT_ID.Equals(0))
                    BackgroundSetupManager.AddRuleTemplate(View.CurrentRuleTemplate, View.SelectedTenantId);
                else
                    BackgroundSetupManager.UpdateRuleTemplate(View.CurrentRuleTemplate, View.ExpressionIds, View.SelectedTenantId);

                return true;
            }
            return false;
        }

        public RuleProcessingResult ValidateRuleTemplate(Int32 resultType)
        {
            String ResultCode = LookupManager.GetLookUpData<lkpRuleResultType>(View.SelectedTenantId).First(x => x.RSL_ID == resultType).RSL_Code;
            return BackgroundSetupManager.ValidateRuleTemplate(View.CurrentRuleTemplate.ComplianceRuleExpressionTemplates, ResultCode, View.SelectedTenantId);
        }

        public void CreateElementList(int ExprIndex)
        {
            View.ExpressionOperators = BackgroundSetupManager.GetExpressionOperators(View.ObjectCount, View.SelectedTenantId);
            for (int index = 0; index < ExprIndex; index++)
            {
                string uilabel = "E" + (index + 1).ToString();
                View.ExpressionOperators.Add(new lkpBkgExpressionOperator() { BEO_SQL = uilabel, BEO_UILabel = uilabel, BEO_ID = index });
            }
        }
    }
}
