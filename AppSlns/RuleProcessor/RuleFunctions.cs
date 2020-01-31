using System;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Xml;

using Microsoft.SqlServer.Server;

using INTSOF.RuleEngine.Model;
using INTSOF.RuleEngine.DAL;
using INTSOF.RuleEngine.Expression;
using INTSOF.RuleEngine.Utility;

public partial class RuleFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, SystemDataAccess = SystemDataAccessKind.Read)]
    public static SqlString udf_CLR_TestChanges(string expressionXML)
    {
        RuleProcessingResult result = new RuleProcessingResult();
        result.Action = "ValidateTemplate";
        try
        {

            string s = expressionXML.Replace(" ", "$$$");
            INTSOF.RuleEngine.Expression.Evaluator mf = new INTSOF.RuleEngine.Expression.Evaluator(s);
            mf.SkipErrorForBool = true;
            string value = mf.Value;

            return value;

        }
        catch
        {
            return "ERROR";
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction(DataAccess=DataAccessKind.Read,SystemDataAccess=SystemDataAccessKind.Read)]
    public static SqlString udf_CLR_ValidateTemplate(string expressionXML)
    {
        RuleProcessingResult result = new RuleProcessingResult();
        result.Action = "ValidateTemplate";
        try
        {
            RuleDefinition rule = GetRuleDataObject(expressionXML);// Helper.FromXml<INTSOF.RuleEngine.Model.RuleData>(expressionXML);
            result.UIExpressionLabel = rule.UIExpression;

            Evaluator evaluator = new Evaluator(rule.FinalExpression);
            evaluator.SimulateCalculation();
            if (Helper.ToDescriptionString(evaluator.ResultType) == rule.ResultType || evaluator.ResultType == OperatorResultType.Undefined)
            {
                result.Status = 0;
                result.Result = "True";
            }
            else
            {
                result.Status = 1;
                result.Result = "False";
                result.ErrorMessage = "Result of expression is not matching with Result type of Rule";
            }
        }
        catch (Exception ex)
        {
            result.Status = 1;
            result.Result = "ERROR";
            result.ErrorMessage = ex.Message;
            result.ErrorStackTrace = ex.StackTrace;
        }
        return result.ToXML();// Helper.ToXml<RuleProcessingResult>(result);
    }

    [Microsoft.SqlServer.Server.SqlFunction(DataAccess=DataAccessKind.Read,SystemDataAccess=SystemDataAccessKind.Read)]
    public static string udf_CLR_ValidateExpression(string expressionXML, string keyValueXML)
    {
        RuleProcessingResult result = new RuleProcessingResult();
        result.Action = "ValidateExpression";
        try
        {
            RuleDefinition rule =  GetRuleDataObject(expressionXML);// Helper.FromXml<INTSOF.RuleEngine.Model.RuleData>(expressionXML);
            rule.ObjectMappings = GetObjectMappingObject(keyValueXML);//  Helper.FromXml<ObjectMapping>(keyValueXML);
            result.UIExpressionLabel = rule.UIExpression;
            string expression = GenerateExpressionWithValues(rule.FinalExpression, rule.ObjectMappings, true);
            Evaluator evaluator = new Evaluator(expression);
            string value = evaluator.Value;
            OperandType valueType = Operand.GetOperandTypeByValue(value);
            if (Helper.ToDescriptionString(evaluator.ResultType) == rule.ResultType ||
                (evaluator.ResultType == OperatorResultType.Undefined &&
                    ((rule.ResultType.Equals("BOOL", StringComparison.OrdinalIgnoreCase) && valueType == OperandType.Boolean) ||
                        ((!rule.ResultType.Equals("BOOL", StringComparison.OrdinalIgnoreCase)) && valueType != OperandType.Boolean))
                )
            )
            {
                result.Status = 0;
                result.Result = "True";
            }
            else
            {
                result.Status = 1;
                result.Result = "False";
                result.ErrorMessage = "Result of expression is not matching with Result type of Rule";
            }
        }
        catch (Exception ex)
        {
            result.Status = 1;
            result.Result = "ERROR";
            result.ErrorMessage = ex.Message;
            result.ErrorStackTrace = ex.StackTrace;
        }
        return result.ToXML();// Helper.ToXml<RuleProcessingResult>(result);
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void usp_CLR_EvaluatePostRules(int userID, int rootObjectID, int ruleObjectTypeID, int ruleObjectID, out string resultsXML)
    {
        RuleEvaluationResults results = new RuleEvaluationResults();
        List<RuleDefinition> rules = DALObjectRule.GetRules();
        foreach (RuleDefinition rule in rules)
        {
            for (int i = 0; i < rule.ObjectMappings.Mappings.Count; i++)
            {
                string happyValue;
                string value = DALRuleDefinition.GetObjectMapping(userID, rootObjectID, rule.ObjectMappings.Mappings[i].ID, out happyValue);
                if(string.IsNullOrEmpty(value))
                {
                    value = Constants.OPERAND_EMPTY_TEXT_EXPR;
                }
                rule.ObjectMappings.Mappings[i].Value = value;
                rule.ObjectMappings.Mappings[i].HappyValue = happyValue;
            }
            RuleProcessingResult result;
            result = EvaluateRule(rule, rule.ObjectMappings, rule.NeedMinRequirementCheck);
            results.Results.Add(result);
        }
        resultsXML=results.ToXML();
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void usp_CLR_EvaluatePostRulesAsOn(int userID, int rootObjectID, int ruleObjectTypeID, int ruleObjectID, DateTime asOnDate, out string resultsXML)
    {
        RuleEvaluationResults results = new RuleEvaluationResults();
        List<RuleDefinition> rules = DALObjectRule.GetRules();
        foreach (RuleDefinition rule in rules)
        {
            for (int i = 0; i < rule.ObjectMappings.Mappings.Count; i++)
            {
                string happyValue;
                string value = DALRuleDefinition.GetObjectMappingAsOn(userID, rootObjectID, rule.ObjectMappings.Mappings[i].ID, asOnDate, out happyValue);
                if (string.IsNullOrEmpty(value))
                {
                    value = Constants.OPERAND_EMPTY_TEXT_EXPR;
                }
                rule.ObjectMappings.Mappings[i].Value = value;
                rule.ObjectMappings.Mappings[i].HappyValue = happyValue;
            }
            RuleProcessingResult result;
            result = EvaluateRule(rule, rule.ObjectMappings, rule.NeedMinRequirementCheck);
            results.Results.Add(result);
        }
        resultsXML = results.ToXML();
    }

    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, SystemDataAccess = SystemDataAccessKind.Read)]
    public static string udf_CLR_EvaluatePreRules(string rulesXML)
    {
        return udf_CLR_EvaluatePreRules_Schema(rulesXML,"dbo");
    }
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, SystemDataAccess = SystemDataAccessKind.Read)]
    public static string udf_CLR_EvaluatePreRules_Schema(string rulesXML, string schema)
    {
        List<RuleDefinition> rules = GetRulesObject(rulesXML, schema);
        RuleEvaluationResults results = new RuleEvaluationResults();

        foreach (RuleDefinition rule in rules)
        {
            foreach (Mapping om in rule.ObjectMappings.Mappings)
            {
                if (string.IsNullOrEmpty(om.Value))
                    om.Value = Constants.OPERAND_EMPTY_TEXT_EXPR; ;
            }
            RuleProcessingResult result = EvaluateRule(rule, rule.ObjectMappings, false);
            result.RuleName = rule.Name;
            result.NextActionType = rule.ActionType;
            result.NextActionBlock = rule.ActionBlock;
            result.RuleMappingID = rule.RuleMappingID;
            result.AppliedToIdentifier = rule.AppliedToIdentifier;
            results.Results.Add(result);
        }
        return results.ToXML();
    }

    private static RuleProcessingResult EvaluateRule(RuleDefinition rule, ObjectMapping mappings, bool calcMinRequirementMet)
    {
        RuleProcessingResult result = new RuleProcessingResult();
        result.Action = "ValidateExpression";
        try
        {
            bool isBoolResultType = rule.ResultType.Equals("BOOL", StringComparison.OrdinalIgnoreCase);
            
            result.RuleMappingID = rule.RuleMappingID;
            result.RuleName = rule.Name;
            result.SuccessMessage = rule.SuccessMessage;
            result.ErrorMessage = rule.ErrorMessage;
            string expression = GenerateExpressionWithValues(rule.FinalExpression, mappings, false);
            result.UIExpressionLabel = expression.Replace(Constants.TOKEN_DELIMITER," ");
            Evaluator evaluator = new Evaluator(expression);
            if (evaluator.HasToken)
            {
                if (isBoolResultType)
                    evaluator.SkipErrorForBool = true;
                string value = evaluator.Value;
                if (evaluator.ResultType == OperatorResultType.Undefined || Helper.ToDescriptionString(evaluator.ResultType) == rule.ResultType)
                {
                    result.Status = 0;
                    result.Result = value;
                    if (calcMinRequirementMet)
                    {
                        if (value.Equals("False", StringComparison.OrdinalIgnoreCase))
                        {
                            if (mappings.SetHappyValue())
                            {
                                string happyExpression = GenerateExpressionWithValues(rule.FinalExpression, mappings, false);
                                Evaluator happyEvaluator = new Evaluator(happyExpression);
                                happyEvaluator.SkipErrorForBool = true;
                                if (happyEvaluator.Value.Equals(Boolean.TrueString, StringComparison.OrdinalIgnoreCase))
                                    result.MinRequirementMet = 1;
                            }
                        }
                        else
                            result.MinRequirementMet = 1;
                    }
                }
                else
                {
                    result.Status = 1;
                    result.Result = "False";
                    result.ErrorMessage = rule.ErrorMessage;
                }
                result.ErrorStackTrace = evaluator.SkippedErrorMessage;
            }
            else
            {
                result.Status = 0;
                if (isBoolResultType)
                    result.Result = Boolean.FalseString;
                else
                    result.Result = "";
            }
        }
        catch (Exception ex)
        {
            result.Status = 1;
            result.Result = "ERROR";
            result.ErrorMessage =rule.ErrorMessage;
            result.ErrorStackTrace = ex.Message+"-"+ex.StackTrace;
        }
        return result;
    }
    private static string GenerateExpressionWithValues(string expression, ObjectMapping mappings, bool isDummy)
    {
        List<string> outList = new List<string>();
        expression = CleanExpressionString(expression);
        string[] tokens = expression.Split(new string[] { INTSOF.RuleEngine.Utility.Constants.TOKEN_DELIMITER }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string token in tokens)
        {
            if (!(token.Equals("(") || token.Equals(")")))
            {
                Operator op = Operators.GetOperator(token);
                if (op == null)
                {
                    Mapping mapping = mappings.Mappings.Find(m => m.Key.Equals(token));
                    if (mapping == null)
                        throw new Exception("Mapping not provided for token: " + token);
                    string value = string.Empty;
                    if (isDummy)
                    {
                        if (mapping.MappedName.Equals(Constants.OPERAND_EMPTY_TEXT, StringComparison.InvariantCultureIgnoreCase))
                            value = Constants.OPERAND_EMPTY_TEXT_EXPR;
                        else
                            value = Operand.GetDummyValue(Operand.GetOperandTypeByTypeString(mapping.DataType));
                    }
                    else
                        value = mapping.Value;
                    outList.Add(value);
                }
                else
                    outList.Add(token);
            }
            else
                outList.Add(token);
        }
        return String.Join(INTSOF.RuleEngine.Utility.Constants.TOKEN_DELIMITER, outList.ToArray());
    }

    private static string CleanExpressionString(string expression)
    {
        while (expression.IndexOf("  ") >= 0)
            expression = expression.Replace("  ", " ");

        return expression;
    }

    private static RuleDefinition GetRuleDataObject(string xml)
    {
        XmlDocument xdoc = new XmlDocument();
        xdoc.LoadXml(xml);
        RuleDefinition rule = new RuleDefinition();
        rule.ResultType = xdoc.FirstChild.ChildNodes[0].InnerText;
        rule.Expressions = new List<Expression>();
        foreach (XmlNode expression in xdoc.FirstChild.ChildNodes[1].ChildNodes)
        {
            Expression exp = new Expression();
            foreach (XmlNode child in expression.ChildNodes)
            {
                switch (child.Name)
                {
                    case "Name":
                        exp.Name = child.InnerText;
                        break;
                    case "Definition":
                        exp.Definition = child.InnerText;
                        break;
                }
            }
            rule.Expressions.Add(exp);
        }

        return rule;

    }

    private static ObjectMapping GetObjectMappingObject(string xml)
    {
        XmlDocument xdoc = new XmlDocument();
        xdoc.LoadXml(xml);

        ObjectMapping mappings = new ObjectMapping();
        mappings.Mappings = new List<Mapping>();
        foreach (XmlNode mapping in xdoc.FirstChild.ChildNodes[0].ChildNodes)
        {
            Mapping map = new Mapping();
            foreach (XmlNode child in mapping.ChildNodes)
            {
                switch (child.Name)
                {
                    case "Key":
                        map.Key = child.InnerText;
                        break;
                    case "DataType":
                        map.DataType = child.InnerText;
                        break;
                    case "MappedName":
                        map.MappedName = child.InnerText;
                        break;
                    case "Value":
                        map.Value = child.InnerText;
                        break;
                }
            }
            mappings.Mappings.Add(map);
        }

        return mappings;

    }

    private static List<RuleDefinition> GetRulesObject(string xml, string schema)
    {
        List<RuleDefinition> rules = new List<RuleDefinition>();
        XmlDocument xdoc = new XmlDocument();
        xdoc.LoadXml(xml);
        foreach (XmlNode ruleNode in xdoc.ChildNodes[0].ChildNodes)
        {
            int ruleMappingID = 0;
            RuleDefinition rule = null;
            foreach (XmlNode child in ruleNode.ChildNodes)
            {
                switch (child.Name)
                {
                    case "Id":
                        ruleMappingID = Convert.ToInt32(child.InnerText);
                        rule = DALRuleDefinition.Get(ruleMappingID, schema);
                        rule.RuleMappingID = ruleMappingID;
                        break;
                    case "ObjectMapping":
                        if (rule != null)
                            rule.ObjectMappings = GetObjectMappingObject(child.OuterXml);
                        break;
                    case "AppliedToIdentifier":
                        rule.AppliedToIdentifier = child.InnerText;
                        break;
                }
            }
            rules.Add(rule);
        }
        return rules;
    }

}
