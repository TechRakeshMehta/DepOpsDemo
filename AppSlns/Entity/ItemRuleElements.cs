using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// ItemRuleElements
    /// </summary>
    [Serializable]
    public class ItemRuleElements
    {
        public List<RuleElementNode> RuleElementNodes { get; set; }

        public ItemRuleElements()
        {
            RuleElementNodes = new List<RuleElementNode>();
        }
        public void Clear()
        {
            RuleElementNodes.Clear();
        }

        /// <summary>
        /// RuleElementsNodeCount
        /// </summary>
        public int RuleElementsNodeCount
        {
            get { return RuleElementNodes.Count; }
        }

        public void AddRuleElementNode(RuleElementNode ruleElementNode)
        {
            RuleElementNodes.Add(ruleElementNode);
        }


        /// <summary>
        /// RemoveLastRuleElementNode 
        /// Call this methid for undo last step
        /// </summary>
        public void RemoveLastRuleElementNode()
        {
            if (RuleElementNodes.Count > 0)
                RuleElementNodes.RemoveAt(RuleElementNodes.Count - 1);
        }

        public string GetExpression()
        {
            StringBuilder expression = new StringBuilder();
            foreach (RuleElementNode ruleElementNode in RuleElementNodes)
            {
                expression.Append(ruleElementNode.NodeDisplayValue + " ");
            }
            return expression.ToString();
        }
    }

    #region CustomDate.cs
    /// <summary>
    /// CustomDate
    /// </summary>
    [Serializable]
    public class CustomDate
    {
        private DateTime dt;
        public DateTime Date
        {
            set { dt = value; }
            get { return dt; }
        }
        public static CustomDate operator +(CustomDate customDate, int numberOfDays)
        {
            customDate.Date = customDate.Date.AddDays(numberOfDays);
            return customDate;
        }
        public static CustomDate operator -(CustomDate customDate, int numberOfDays)
        {
            customDate.Date = customDate.Date.AddDays(-1 * numberOfDays);
            return customDate;
        }
        public static CustomDate operator +(CustomDate customDate, double numberOfDays)
        {
            customDate.Date = customDate.Date.AddDays(numberOfDays);
            return customDate;
        }
        public static CustomDate operator -(CustomDate customDate, double numberOfDays)
        {
            customDate.Date = customDate.Date.AddDays(-1 * numberOfDays);
            return customDate;
        }
        public static int operator -(CustomDate customDate1, CustomDate customDate2)
        {
            TimeSpan ts = customDate1.Date - customDate2.Date;
            return ts.Days;
        }
        public static bool operator <(CustomDate customDate1, CustomDate customDate2)
        {
            return customDate1.Date < customDate2.Date;
        }
        public static bool operator >(CustomDate customDate1, CustomDate customDate2)
        {
            return customDate1.Date < customDate2.Date;
        }
        public static bool operator <=(CustomDate customDate1, CustomDate customDate2)
        {
            return customDate1.Date <= customDate2.Date;
        }
        public static bool operator >=(CustomDate customDate1, CustomDate customDate2)
        {
            return customDate1.Date >= customDate2.Date;
        }
    }
    #endregion

    #region Common.cs

    /// <summary>
    /// RuleElementNodeType
    /// </summary>
    [Serializable]
    public enum RuleElementNodeType
    {
        ItemAttribute,
        Constant,
        Variable,
        Operator
    }

    /// <summary>
    /// RuleElementNodeType
    /// </summary>
    [Serializable]
    public enum RuleElementNodeDataType
    {
        Text,
        Number,
        Date,
        TimespanDays,
        TimespanMonths,
        TimespanYears,        
        NA
    }

    /// <summary>
    /// RuleElementNode
    /// </summary>
    [Serializable]
    public class RuleElementNode
    {
        public RuleElementNodeType NodeType { get; set; }
        public string NodeValue { get; set; }
        public RuleElementNodeDataType NodeDataType { get; set; }
        public string NodeDisplayValue { get; set; }

    }
    #endregion
}
