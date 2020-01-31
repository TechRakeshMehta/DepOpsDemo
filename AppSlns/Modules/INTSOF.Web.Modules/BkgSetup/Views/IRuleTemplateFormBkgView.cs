using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.BkgSetup.Views
{
    public interface IRuleTemplateFormBkgView
    {
        /// <summary>
        /// Rule Result Types</summary>
        /// <value>
        /// Sets the list of all Rule Result Types.</value>
        List<lkpBkgRuleResultType> RuleResultTypes
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
        List<lkpBkgExpressionOperator> ExpressionOperators
        {
            get;
            set;
        }

        List<Int32> ExpressionIds
        {
            get;
            set;
        }

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
