using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// Category Rule Set Elements
    /// </summary>
    [Serializable]
    public class CategoryRuleSetElements
    {
        public List<RuleSetElementNode> RuleSetElementNodes { get; set; }

        public CategoryRuleSetElements()
        {
            RuleSetElementNodes = new List<RuleSetElementNode>();
        }
        public void Clear()
        {
            RuleSetElementNodes.Clear();
        }

        /// <summary>
        /// RuleElementsNodeCount
        /// </summary>
        public int RuleSetElementsNodeCount
        {
            get { return RuleSetElementNodes.Count; }
        }

        public void AddRuleSetElementNode(RuleSetElementNode ruleSetElementNode)
        {
            RuleSetElementNodes.Add(ruleSetElementNode);
        }



        /// <summary>
        /// RemoveLastRuleElementNode 
        /// Call this methid for undo last step
        /// </summary>
        public void RemoveLastRuleSetElementNode()
        {
            if (RuleSetElementNodes.Count > 0)
                RuleSetElementNodes.RemoveAt(RuleSetElementNodes.Count - 1);
        }

        public string GetExpression()
        {
            StringBuilder expression = new StringBuilder();
            foreach (RuleSetElementNode ruleSetElementNode in RuleSetElementNodes)
            {
                expression.Append(ruleSetElementNode.NodeDisplayValue + " ");
            }
            return expression.ToString();
        }
    }

    #region Common.cs

    /// <summary>
    /// RuleSetElementNodeType
    /// </summary>
    [Serializable]
    public enum RuleSetElementNodeType
    {
        Item,
        Operator
    }

    /// <summary>
    /// RuleSetElementNode
    /// </summary>
   [Serializable]
    public class RuleSetElementNode
    {
        public RuleSetElementNodeType NodeType { get; set; }
        public string NodeValue { get; set; }
        public string NodeDisplayValue { get; set; }
    }

    #endregion
}
