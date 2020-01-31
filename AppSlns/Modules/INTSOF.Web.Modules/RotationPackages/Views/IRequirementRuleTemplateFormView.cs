using System;
using System.Collections.Generic;
using System.Text;
using Entity.SharedDataEntity;

namespace CoreWeb.RotationPackages.Views
{
    public interface IRequirementRuleTemplateFormView
    {

        /// <summary>
        /// Rule Result Types</summary>
        /// <value>
        /// Sets the list of all Rule Result Types.</value>
        List<lkpRuleResultType> RuleResultTypes
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
       

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 CurrentloggedinUserId
        {
            get;
        }
    }
}




