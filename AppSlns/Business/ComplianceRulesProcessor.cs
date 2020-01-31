using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public static class ComplianceRulesProcessor
    {
        public static String ValidateRule(List<RuleElementNode> lstRuleElementNodes)
        {
            try
            {
                StringBuilder processExpression = new StringBuilder();
                int totalVariables = 0;

                //create expression
                foreach (RuleElementNode ruleElementNode in lstRuleElementNodes)
                {
                    if (ruleElementNode.NodeType == RuleElementNodeType.ItemAttribute || ruleElementNode.NodeType == RuleElementNodeType.Constant || ruleElementNode.NodeType == RuleElementNodeType.Variable)
                    {
                        processExpression.Append("Variable" + (totalVariables + 1) + " ");
                        totalVariables++;
                    }
                    if (ruleElementNode.NodeType == RuleElementNodeType.Operator)
                    {
                        processExpression.Append(ruleElementNode.NodeValue);
                    }
                }

                var p = new ExpressionEvaluator.CompiledExpression(processExpression.ToString());

                totalVariables = 0;
                //add all variables value
                foreach (RuleElementNode ruleElementNode in lstRuleElementNodes)
                {
                    if (ruleElementNode.NodeType == RuleElementNodeType.ItemAttribute || ruleElementNode.NodeType == RuleElementNodeType.Constant || ruleElementNode.NodeType == RuleElementNodeType.Variable)
                    {
                        if (ruleElementNode.NodeDataType == RuleElementNodeDataType.Text)
                        {
                            string testString = "teststring";
                            p.RegisterType("Variable" + (totalVariables + 1), testString);
                        }
                        else if (ruleElementNode.NodeDataType == RuleElementNodeDataType.Number || ruleElementNode.NodeDataType == RuleElementNodeDataType.TimespanDays || ruleElementNode.NodeDataType == RuleElementNodeDataType.TimespanMonths || ruleElementNode.NodeDataType == RuleElementNodeDataType.TimespanYears)
                        {
                            double testNumber = 100;
                            p.RegisterType("Variable" + (totalVariables + 1), testNumber);
                        }
                        else if (ruleElementNode.NodeDataType == RuleElementNodeDataType.Date)
                        {
                            CustomDate customDate = new CustomDate();
                            customDate.Date = DateTime.Today;
                            p.RegisterType("Variable" + (totalVariables + 1), customDate);
                        }
                        totalVariables++;
                    }

                }
                p.Parse();
                p.Compile();
                p.Eval();

                return String.Empty;
            }
            catch (Exception ex)
            {
                return Convert.ToString("Exception is : " + ex);
            }
        }

        public static String ValidateRuleSet(List<RuleSetElementNode> lstRuleSetElementNodes)
        {
            try
            {
                StringBuilder processExpression = new StringBuilder();
                int totalVariables = 0;

                //create expression
                foreach (RuleSetElementNode ruleSetElementNode in lstRuleSetElementNodes)
                {
                    if (ruleSetElementNode.NodeType == RuleSetElementNodeType.Item)
                    {
                        processExpression.Append("Variable" + (totalVariables + 1) + " ");
                        totalVariables++;
                    }
                    if (ruleSetElementNode.NodeType == RuleSetElementNodeType.Operator)
                    {
                        String Operator = String.Empty;
                        switch (ruleSetElementNode.NodeValue)
                        {
                            case "OR":
                                Operator = "|| ";
                                break;
                            case "AND":
                                Operator = "&& ";
                                break;
                            case "NOT":
                                Operator = "! ";
                                break;
                            default:
                                Operator = ruleSetElementNode.NodeValue;
                                break;
                        }
                        processExpression.Append(Operator);
                    }
                }

                var p = new ExpressionEvaluator.CompiledExpression(processExpression.ToString());

                totalVariables = 0;
                //add all variables value
                foreach (RuleSetElementNode ruleSetElementNode in lstRuleSetElementNodes)
                {
                    if (ruleSetElementNode.NodeType == RuleSetElementNodeType.Item)
                    {
                        bool testBoolValue = true;
                        p.RegisterType("Variable" + (totalVariables + 1), testBoolValue);
                        totalVariables++;
                    }

                }
                p.Parse();
                p.Compile();
                p.Eval();

                return String.Empty;
            }
            catch (Exception ex)
            {
                return Convert.ToString("Exception is : " + ex);
            }
        }
    }
}
