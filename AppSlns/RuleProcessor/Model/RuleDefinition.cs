using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using INTSOF.RuleEngine.Expression;

namespace INTSOF.RuleEngine.Model
{
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    internal class RuleDefinition
    {
        public int RuleMappingID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string ActionType { get; set; }
        public string ActionBlock { get; set; }
        public string RuleType { get; set; }
        public string ResultType { get; set; }
        public List<Expression> Expressions { get; set; }
        public ObjectMapping ObjectMappings { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public bool NeedMinRequirementCheck { get; set; }
        public string AppliedToIdentifier { get; set; }

        private string finalExpression= string.Empty;
        private string uiExpression = string.Empty;

        public string FinalExpression
        {
            get
            {
                if (string.IsNullOrEmpty(finalExpression))
                {
                    Dictionary<string, string> outList = new Dictionary<string, string>();
                    foreach (Expression exp in Expressions)
                    {
                        List<string> outExp = new List<string>();
                        outExp.Add("(");
                        string[] tokens = exp.Definition.Split(new string[] { INTSOF.RuleEngine.Utility.Constants.TOKEN_DELIMITER }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string token in tokens)
                        {
                            if (outList.ContainsKey(token))
                                outExp.Add(outList[token]);
                            else
                            {
                                Expression refExp = Expressions.Find(e => e.Name.Equals(token));
                                if (refExp != null)
                                {
                                    outExp.Add("(");
                                    outExp.Add(refExp.Definition);
                                    outExp.Add(")");
                                }
                                else
                                    outExp.Add(token);
                            }
                        }
                        outExp.Add(")");
                        string expName = exp.Name;
                        if (expName.Equals("(Group)"))
                            expName = "GE";
                        outList.Add(expName, String.Join(INTSOF.RuleEngine.Utility.Constants.TOKEN_DELIMITER, outExp.ToArray()));
                    }
                    finalExpression = outList["GE"];
                }
                return finalExpression;
            }
        }

        public string UIExpression
        {
            get
            {
                List<string> outList = new List<string>();
                string[] tokens = FinalExpression.Split(new string[] { INTSOF.RuleEngine.Utility.Constants.TOKEN_DELIMITER }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string token in tokens)
                {
                    Operator op = Operators.GetOperator(token);
                    if (op != null)
                        outList.Add(op.UILabel);
                    else if (ObjectMappings != null)
                    {
                        string mappedName = ObjectMappings.getMappedName(token);
                        if (!(string.IsNullOrEmpty(mappedName)))
                            outList.Add(mappedName);
                        else
                            outList.Add(token);
                    }
                    else
                        outList.Add(token);
                }
                return String.Join(" ", outList.ToArray());
            }
        }
    }
}
