#region References 
using System;
using System.Collections.Generic;

#endregion

namespace Entity
{
    /// <summary>
    /// RuleExpressionElementType
    /// </summary>
    public enum ComplianceRuleExpressionElementType
    {
        Object = 1,
        Operator = 2,
        Expression =3
    }

    /// <summary>
    /// RuleExpressionElement
    /// </summary>
    [Serializable]
    public class ComplianceRuleExpressionElement
    {

        #region Properties 
        /// <summary>
        /// ElementValue
        /// </summary>
        public string ElementValue
        {
            get; set;
        }

        public string ElementOperator
        {
            get;
            set;
        }

        public ComplianceRuleExpressionElementType ExpressionElementType
        {
            get; set;
        }

        #endregion 

    }

    /// <summary>
    /// RuleExpressionTemplate
    /// </summary>
    [Serializable]
    public class ComplianceRuleExpressionTemplate
    {
        #region Private Data Members

        private List<ComplianceRuleExpressionElement> _ruleExpressionElements;

        #endregion

        #region Properties
        public List<ComplianceRuleExpressionElement> RuleExpressionElements
        {
            get { return _ruleExpressionElements; }
            set { _ruleExpressionElements = value; }
        }

        public int EX_ID{get;set;}
        public string EX_Name { get; set; }
        public string EX_Description { get; set; }
        public string EX_Expression{get;set;}
        public int EX_ResultType { get; set; }
        public Boolean EX_IsActive{get;set;}
        public Boolean EX_IsDeleted { get; set; }
        public int EX_CreatedByID{get;set;}
        public DateTime EX_CreatedOn{get;set;}
        public int? EX_ModifiedByID{get;set;}
        public DateTime ? EX_ModifiedOn{get;set;}
        public string EX_Code{get;set;}
        public int ExpressionOrder{get;set;}

        #endregion 

        #region Constructor 

        public ComplianceRuleExpressionTemplate()
        {
            _ruleExpressionElements = new List<ComplianceRuleExpressionElement>();
        }

        #endregion 
    }
    
    /// <summary>
    /// RuleTemplate
    /// </summary>
    [Serializable]
    public class ComplianceRuleTemplate
    {
        #region Private Data Members

        private List<ComplianceRuleExpressionTemplate> _ruleExpressionTemplates;

        #endregion 
        
        #region Constructor

        public ComplianceRuleTemplate()
        {
            _ruleExpressionTemplates = new List<ComplianceRuleExpressionTemplate>();
        }

        #endregion 

        #region Properties 

        /// <summary>
        /// RuleGroupExpression
        /// </summary>
        public string RuleGroupExpression
        {
            get ; set; 
        }

        /// <summary>
        /// RuleExpressionTemplates
        /// </summary>
        public List<ComplianceRuleExpressionTemplate> ComplianceRuleExpressionTemplates
        {
            get { return _ruleExpressionTemplates; }
            set { _ruleExpressionTemplates = value; }
        }

        public Int32 RLT_ID{get;set;}
        public String RLT_Name { get; set; }
        public String RLT_Description { get; set; }
        public String RLT_UserExpression { get; set; }
        public Int32 RLT_ResultType { get; set; }
        public Int32 RLT_ActionType {get;set;}
        public Int32 RLT_Type {get;set;}
        public Boolean RLT_IsActive { get; set; }
        public Boolean RLT_IsDeleted { get; set; }
        public Int32 RLT_CreatedByID { get; set; }
        public DateTime RLT_CreatedOn { get; set; }
        public Int32 ? RLT_ModifiedByID { get; set; }
        public DateTime? RLT_ModifiedOn { get; set; }
        public Guid RLT_Code { get; set; }
        public Int32 RLT_ObjectCount { get; set; }
        public Boolean IsRuleTemplateAssociatedWithRule { get; set; }
        public String RLT_Notes { get; set; }
        public Boolean RLT_IsCopied { get; set; }
        #endregion 

    }

}
