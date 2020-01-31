#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
//using INTSOF.SharedObjects;
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
namespace CoreWeb.ComplianceAdministration.Views
{
    public class RuleTemplateFormPresenter : Presenter<IRuleTemplateFormView>
    {
        public override void OnViewLoaded()
        {
           //todo
        }

        public override void OnViewInitialized()
        {
            View.RuleTypes = RuleManager.GetRuleTypes(View.SelectedTenantId);
            View.RuleResultTypes = RuleManager.GetRuleResultTypes(View.SelectedTenantId);
            View.RuleActionTypes = RuleManager.GetRuleActionTypes(View.SelectedTenantId);
        }

        public void InitializeRuleTemplate()
        {
            if (View.RuleTemplateID == null || View.RuleTemplateID == 0)
            {
                View.CurrentRuleTemplate = new Entity.ComplianceRuleTemplate();
            }
            else
            {
                View.CurrentRuleTemplate = RuleManager.GetRuleTemplate(View.RuleTemplateID, View.SelectedTenantId);
            }
        }

        public bool SaveRuleTemplate()
        {
            if (View.CurrentRuleTemplate != null)
            {
                if (View.CurrentRuleTemplate.RLT_ID.Equals(0))
                    RuleManager.AddRuleTemplate(View.CurrentRuleTemplate, View.SelectedTenantId);
                else
                    RuleManager.UpdateRuleTemplate(View.CurrentRuleTemplate, View.ExpressionIds, View.SelectedTenantId);

                return true;
            }
            return false;
        }

        public RuleProcessingResult ValidateRuleTemplate(Int32 resultType)
        {
            String ResultCode = LookupManager.GetLookUpData<lkpRuleResultType>(View.SelectedTenantId).First(x => x.RSL_ID == resultType).RSL_Code;
            return RuleManager.ValidateRuleTemplate(View.CurrentRuleTemplate.ComplianceRuleExpressionTemplates, ResultCode, View.SelectedTenantId);
        }

        public void CreateElementList(int ExprIndex)
        {

            if (View.IsUIRule)
            {
                //Set UI Rule list
                View.ExpressionOperators = RuleManager.GetExpressionOperators(View.ObjectCount, View.SelectedTenantId);

                for (int index = 0; index < ExprIndex; index++)
                {
                    string uilabel = "E" + (index + 1).ToString();
                    View.ExpressionOperators.Add(new lkpExpressionOperator() { EO_SQL= uilabel, EO_UILabel = uilabel, EO_ID = index });
                }
            }
            else
            {
                //Set Business rule list
                View.ExpressionOperators = RuleManager.GetExpressionOperators(View.ObjectCount, View.SelectedTenantId);
                for (int index = 0; index < ExprIndex; index++)
                {
                    string uilabel = "E" + (index + 1).ToString();
                    View.ExpressionOperators.Add(new lkpExpressionOperator() { EO_SQL = uilabel, EO_UILabel = uilabel, EO_ID = index });
                }
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
    }
}




