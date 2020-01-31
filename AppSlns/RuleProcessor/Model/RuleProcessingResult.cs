using System;
using System.Collections.Generic;
using System.Text;

using System.Xml.Serialization;

namespace INTSOF.RuleEngine.Model
{
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    internal class RuleProcessingResult
    {
        public int RuleMappingID { get; set; }
        public string RuleName { get; set; }
        public string NextActionType { get; set; }
        public string NextActionBlock { get; set; }
        public int Status { get; set; }
        public string Action { get; set; }
        public string UIExpressionLabel { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorStackTrace { get; set; }
        public Int16 MinRequirementMet { get; set; }
        public string AppliedToIdentifier { get; set; }

        private string result;
        public string Result
        {
            get
            { 
                return result; 
            }
            set 
            { 
                if(value.Equals(Utility.Constants.OPERAND_EMPTY_TEXT_EXPR,StringComparison.OrdinalIgnoreCase))
                    value=string.Empty;
                result = value; 
            }
        }

        public RuleProcessingResult()
        {
            RuleName = string.Empty;
            NextActionType = string.Empty;
            NextActionBlock = string.Empty;
            Action = string.Empty;
            result = string.Empty;
            UIExpressionLabel = string.Empty;
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;
            ErrorStackTrace = string.Empty;
            MinRequirementMet = 0;
            AppliedToIdentifier = string.Empty;
        }

        public string ToXML()
        {
            StringBuilder sb = new StringBuilder("<RuleProcessingResult>");
            sb.Append("<Status>" + Status.ToString() + "</Status>");
            sb.Append("<Action><![CDATA[" + Action + "]]></Action>");
            sb.Append("<Result>" + Result + "</Result>");
            sb.Append("<UIExpressionLabel>" + UIExpressionLabel + "</UIExpressionLabel>");
            sb.Append("<SuccessMessage><![CDATA[" + SuccessMessage + "]]></SuccessMessage>");
            sb.Append("<ErrorMessage><![CDATA[" + ErrorMessage + "]]></ErrorMessage>");
            sb.Append("<ErrorStackTrace><![CDATA[" + ErrorStackTrace + "]]></ErrorStackTrace>");
            sb.Append("</RuleProcessingResult>");
            return sb.ToString();
        }
        public string ToEvaluationXML()
        {
            StringBuilder sb = new StringBuilder("<Result>");
            sb.Append("<RuleMappingID>" + RuleMappingID.ToString() + "</RuleMappingID>");
            sb.Append("<Name><![CDATA[" + RuleName.ToString() + "]]></Name>");
            sb.Append("<Status>" + Status.ToString() + "</Status>");
            sb.Append("<Result>" + Result + "</Result>");
            sb.Append("<MinRequirementMet>" + MinRequirementMet + "</MinRequirementMet>");
            sb.Append("<Expression><![CDATA[" + UIExpressionLabel + "]]></Expression>");
            sb.Append("<SuccessMessage><![CDATA[" + SuccessMessage + "]]></SuccessMessage>");
            sb.Append("<ErrorMessage><![CDATA[" + ErrorMessage + "]]></ErrorMessage>");
            sb.Append("<NextActionType>" + NextActionType + "</NextActionType>");
            sb.Append("<NextActionBlock><![CDATA[" + NextActionBlock + "]]></NextActionBlock>");
            sb.Append("<ErrorStackTrace><![CDATA[" + ErrorStackTrace + "]]></ErrorStackTrace>");
            sb.Append("<AppliedToIdentifier>" + AppliedToIdentifier + "</AppliedToIdentifier>");
            sb.Append("</Result>");
            return sb.ToString();
        }

    }
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    internal class RuleEvaluationResults
    {
        public List<RuleProcessingResult> Results { get; set; }
        public RuleEvaluationResults()
        {
            Results = new List<RuleProcessingResult>();
        }

        public string ToXML()
        {
            StringBuilder sb = new StringBuilder("<Results>");
            foreach (RuleProcessingResult result in Results)
            {
                sb.Append(result.ToEvaluationXML());
            }
            sb.Append("</Results>");
            return sb.ToString();
        }
    }


}
