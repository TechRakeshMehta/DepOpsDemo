#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
//using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region Application Specific

using Entity.SharedDataEntity;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using ExternalServiceProxy;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.SharedObjects;

#endregion

#endregion
namespace CoreWeb.RotationPackages.Views
{
    public class RequirementRuleTemplateFormPresenter : Presenter<IRequirementRuleTemplateFormView>
    {
        public override void OnViewLoaded()
        {
            //todo
        }

        public override void OnViewInitialized()
        {
            View.RuleResultTypes = SharedRequirementRuleManager.GetRuleResultTypes();
        }

        public void InitializeRuleTemplate()
        {
            if (View.RuleTemplateID == null || View.RuleTemplateID == 0)
            {
                View.CurrentRuleTemplate = new Entity.ComplianceRuleTemplate();
            }
            else
            {
                View.CurrentRuleTemplate = SharedRequirementRuleManager.GetRuleTemplate(View.RuleTemplateID);
            }
        }

        public bool SaveRuleTemplate()
        {
            if (View.CurrentRuleTemplate != null)
            {
                if (View.CurrentRuleTemplate.RLT_ID.Equals(0))
                    SharedRequirementRuleManager.AddRuleTemplate(View.CurrentRuleTemplate);
                else
                    SharedRequirementRuleManager.UpdateRuleTemplate(View.CurrentRuleTemplate, View.ExpressionIds);

                return true;
            }
            return false;
        }

        public RuleProcessingResult ValidateRuleTemplate(Int32 resultType)
        {
            String ResultCode = LookupManager.GetSharedDBLookUpData<lkpRuleResultType>().First(x => x.RSL_ID == resultType).RSL_Code;
            return SharedRequirementRuleManager.ValidateRuleTemplate(View.CurrentRuleTemplate.ComplianceRuleExpressionTemplates, ResultCode);
        }

        public void CreateElementList(int ExprIndex)
        {
            //Set  rule list
            View.ExpressionOperators = SharedRequirementRuleManager.GetExpressionOperators(View.ObjectCount);
            for (int index = 0; index < ExprIndex; index++)
            {
                string uilabel = "E" + (index + 1).ToString();
                View.ExpressionOperators.Add(new lkpExpressionOperator() { EO_SQL = uilabel, EO_UILabel = uilabel, EO_ID = index });
            }
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentloggedinUserId).Organization.TenantID.Value;
        }
    }
}




