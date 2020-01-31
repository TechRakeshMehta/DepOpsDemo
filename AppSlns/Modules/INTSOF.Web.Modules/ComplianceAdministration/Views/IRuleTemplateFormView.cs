using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IRuleTemplateFormView
    {
        /// <summary>
        /// Rule Types</summary>
        /// <value>
        /// Sets the list of all Rule types.</value>
        List<lkpRuleType> RuleTypes
        {
            set;
        }

        /// <summary>
        /// Rule Result Types</summary>
        /// <value>
        /// Sets the list of all Rule Result Types.</value>
        List<lkpRuleResultType> RuleResultTypes
        {
            set;
        }

        /// <summary>
        /// Rule Action Types</summary>
        /// <value>
        /// Sets the list of all Rule Action types.</value>
        List<lkpRuleActionType> RuleActionTypes
        {
            set;
        }

        /// <summary>
        /// Current Rule Template
        /// </summary>
        Entity.ComplianceRuleTemplate CurrentRuleTemplate { get; set; }

        /// <summary>
        /// ID of Current Template
        /// </summary>
        Int32 RuleTemplateID { get; set; }

        List<Entity.ComplianceRuleExpressionTemplate> ComplianceExpressionTemplates { get; }


        Int32 ObjectCount { get; }


        /// <summary>
        /// Rule Expression Operator Types</summary>
        /// <value>
        /// Sets the list of all Rule Action types.</value>
        List<lkpExpressionOperator> ExpressionOperators
        {
            get;
            set;
        }

        List<Int32> ExpressionIds
        {
            get;
            set;
        }

        bool IsUIRule { get; }

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 currentloggedinuserId
        {
            get;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }
    }
}




