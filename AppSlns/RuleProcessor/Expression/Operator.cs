using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using INTSOF.RuleEngine.Utility;
using INTSOF.RuleEngine.DAL;

namespace INTSOF.RuleEngine.Expression
{
    public enum OperatorType
    {

        [Description("AROP")]
        Arithmetic,
        [Description("CMOP")]
        Comparison,
        [Description("LGOP")]
        Logical,
        [Description("CDOP")]
        Conditional,
        [Description("DTFN")]
        DateFunction

    }
    public enum OperatorAssociationType
    {
        Left,
        Right
    }

    public enum OperatorResultType
    {
        [Description("Undefined")]
        Undefined,
        [Description("BOOL")]
        Boolean = 1,
        [Description("DATA")]
        Value
    }

    internal static class Operators
    {
        static readonly List<Operator> operators;
        static readonly Dictionary<string, OperatorType> operatorTypeCodes;
        static Operators()
        {
            operatorTypeCodes = new Dictionary<string, OperatorType>();
            operatorTypeCodes.Add(Helper.ToDescriptionString(OperatorType.Arithmetic), OperatorType.Arithmetic);
            operatorTypeCodes.Add(Helper.ToDescriptionString(OperatorType.Logical), OperatorType.Logical);
            operatorTypeCodes.Add(Helper.ToDescriptionString(OperatorType.Comparison), OperatorType.Comparison);
            operatorTypeCodes.Add(Helper.ToDescriptionString(OperatorType.Conditional), OperatorType.Conditional);
            operatorTypeCodes.Add(Helper.ToDescriptionString(OperatorType.DateFunction), OperatorType.DateFunction);

            operators = DALOperator.GetOperators();
        }


        public static Operator GetOperator(string operatorTypeCode, string sign, string uiLabel, OperatorAssociationType associationType, int precedence, int noOfOperands)
        {
            OperatorType type = operatorTypeCodes[operatorTypeCode];
            switch (type)
            {
                case OperatorType.Arithmetic:
                    return (new ArithmeticOperator(sign, uiLabel, associationType, precedence, noOfOperands));
                case OperatorType.Logical:
                    return (new LogicalOperator(sign, uiLabel, associationType, precedence, noOfOperands));
                case OperatorType.Comparison:
                    return (new ComparisonOperator(sign, uiLabel, associationType, precedence, noOfOperands));
                case OperatorType.Conditional:
                    return (new ConditionalOperator(sign, uiLabel, associationType, precedence, noOfOperands));
                case OperatorType.DateFunction:
                    return (new DateFunction(sign, uiLabel, associationType, precedence, noOfOperands));

            }
            return null;
        }
        public static Operator GetOperator(string sign)
        {
            return operators.Find(o => o.Sign == sign);
        }

        public static string GetUILabel(string expression)
        {
            List<string> outList = new List<string>();
            string[] tokens = expression.Split(new string[] { INTSOF.RuleEngine.Utility.Constants.TOKEN_DELIMITER }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string token in tokens)
            {
                Operator op = GetOperator(token);
                if (op != null)
                    outList.Add(op.UILabel);
                else
                    outList.Add(token);
            }
            return String.Join(" ", outList.ToArray());
        }
    }

    internal abstract class Operator
    {
        public OperatorType Type { get; set; }
        public string Sign { get; set; }
        public string UILabel { get; set; }
        public OperatorAssociationType AssociationType { get; set; }
        public int Precedence { get; set; }
        public OperatorResultType ResultType { get; set; }
        public int NoOfOperands { get; set; }

        protected string[] Operands;
        protected OperandType[] OperandTypes;
        protected bool[] IsOperandTimespan;

        public Operator(OperatorType type, OperatorResultType resultType)
        {
            Type = type;
            ResultType = resultType;
        }

        protected abstract bool ValidateOperand();

        protected abstract string Execute();

        protected string OperandsToString()
        {
            string retVal = string.Empty;
            if (Operands != null)
            {
                for (int operandCounter = 0; operandCounter < Operands.Length; operandCounter++)
                {
                    retVal = retVal + ", " + Operands[operandCounter];
                }
                if(retVal.Length>2)
                    retVal.Substring(2);
            }
            return retVal;
        }

        public string Calculate(params string[] operands)
        {
            Operands=operands;
            OperandTypes = new OperandType[operands.Length];
            IsOperandTimespan = new bool[operands.Length];
 
            for(int operandCounter=0; operandCounter<operands.Length;operandCounter++)
            {
                OperandTypes[operandCounter] = Operand.GetOperandTypeByValue(operands[operandCounter]);
                IsOperandTimespan[operandCounter] = false;
            }

            bool emptyConstantUse = false;
            for (int operandCounter = 0; operandCounter < operands.Length; operandCounter++)
            {
                if (operands[operandCounter].Equals(Constants.OPERAND_EMPTY_TEXT_EXPR, StringComparison.OrdinalIgnoreCase))
                {
                    operands[operandCounter] = string.Empty;
                    emptyConstantUse = true;
                }
            }

            if (emptyConstantUse && Type == OperatorType.Comparison && (Sign.Equals("=", StringComparison.OrdinalIgnoreCase) || Sign.Equals("!=", StringComparison.OrdinalIgnoreCase)))
            {
                OperandTypes[0] = OperandType.Text;
                OperandTypes[1] = OperandType.Text;
            }
            else if (emptyConstantUse && Type == OperatorType.Logical)
            {
                if (string.IsNullOrEmpty(operands[0]))
                {
                    operands[0] = Boolean.FalseString;
                    OperandTypes[0] = OperandType.Boolean;
                }
                if (string.IsNullOrEmpty(operands[1]))
                {
                    operands[1] = Boolean.FalseString;
                    OperandTypes[1] = OperandType.Boolean;
                }
            }

            for (int operandCounter = 0; operandCounter < operands.Length; operandCounter++)
            {
                string typeSuffix = Helper.ToDescriptionString(OperandTypes[operandCounter]);
                if ((!string.IsNullOrEmpty(typeSuffix)) && typeSuffix.Contains("##") && operands[operandCounter].Contains(typeSuffix))
                {
                    if (emptyConstantUse)
                        throw new Exception("Invalid Operand (" + OperandsToString() + ")");

                    operands[operandCounter] = operands[operandCounter].Replace(typeSuffix, "");
                    if (OperandTypes[operandCounter] == OperandType.TDay)
                        OperandTypes[operandCounter] = OperandType.Numeric;
                    IsOperandTimespan[operandCounter] = true;
                }
            }

            if (ValidateOperand())
                return Execute();
            throw new Exception("Invalid Operand (" + OperandsToString() + ")");
        }
    }
    internal class ArithmeticOperator : Operator
    {
        public ArithmeticOperator(string sign, string uiLabel, OperatorAssociationType associationType, int precedence, int noOfOperands)
            : base(OperatorType.Arithmetic, OperatorResultType.Value)
        {
            Sign = sign;
            UILabel = uiLabel;
            AssociationType = associationType;
            Precedence = precedence;
            NoOfOperands = noOfOperands;
        }
        protected override bool ValidateOperand()
        {
            if ((Sign != "+" && Sign != "-") && (OperandTypes[0] != OperandType.Numeric && OperandTypes[1] != OperandType.Numeric))
                throw new Exception("Invalid Operand For Arithmatic Operation (" + Operands[0] + ", " + Operands[1] + ", " + Sign + ")");
            else
            {
                if (Sign == "+" && (OperandTypes[0] == OperandType.Text || OperandTypes[1] == OperandType.Text))
                    return true;
                if (Sign == "-" && OperandTypes[0] == OperandTypes[1] && OperandTypes[0] == OperandType.Date)
                    return true;
                if ((Sign == "+" || Sign == "-") && ((OperandTypes[0] == OperandType.Date && IsOperandTimespan[1]) || (OperandTypes[1] == OperandType.Date && IsOperandTimespan[0])))
                    return true;
                if ((OperandTypes[0] != OperandType.Numeric && OperandTypes[0] != OperandType.Date) || (OperandTypes[1] != OperandType.Numeric))
                    throw new Exception("Invalid Operand For Arithmatic Operation (" + OperandsToString() + ", " + Sign + ")");
            }
            return true;
        }
        protected override string Execute()
        {
            switch (Sign)
            {
                case "*":
                    return (Convert.ToDecimal(Operands[0]) * Convert.ToDecimal(Operands[1])).ToString();
                case "/":
                    return (Convert.ToDecimal(Operands[0]) / Convert.ToDecimal(Operands[1])).ToString();
                case "+":
                    if (OperandTypes[0] == OperandType.Text || OperandTypes[1] == OperandType.Text)
                        return Operands[0] + Operands[1];
                    //Start Date Operation 
                    if (OperandTypes[0] == OperandType.Date && (OperandTypes[1] == OperandType.Numeric || OperandTypes[1] == OperandType.TDay))
                        return (DateTime.Parse(Operands[0]).AddDays(Convert.ToDouble(Operands[1]))).ToShortDateString();
                    if (OperandTypes[0] == OperandType.Date && OperandTypes[1] == OperandType.TMth)
                        return (DateTime.Parse(Operands[0]).AddMonths(Convert.ToInt32(Operands[1]))).ToShortDateString();
                    if (OperandTypes[0] == OperandType.Date && OperandTypes[1] == OperandType.TYr)
                        return (DateTime.Parse(Operands[0]).AddYears(Convert.ToInt32(Operands[1]))).ToShortDateString();
                    if (OperandTypes[1] == OperandType.Date && (OperandTypes[0] == OperandType.Numeric || OperandTypes[0] == OperandType.TDay))
                        return (DateTime.Parse(Operands[1]).AddDays(Convert.ToDouble(Operands[0]))).ToShortDateString();
                    if (OperandTypes[1] == OperandType.Date && OperandTypes[0] == OperandType.TMth)
                        return (DateTime.Parse(Operands[1]).AddMonths(Convert.ToInt32(Operands[0]))).ToShortDateString();
                    if (OperandTypes[1] == OperandType.Date && OperandTypes[0] == OperandType.TYr)
                        return (DateTime.Parse(Operands[1]).AddYears(Convert.ToInt32(Operands[0]))).ToShortDateString();
                    //End Date Operation
                    if (OperandTypes[0] == OperandType.Numeric)
                        return (Convert.ToDecimal(Operands[0]) + Convert.ToDecimal(Operands[1])).ToString();
                    break;
                case "-":
                    if (OperandTypes[0] == OperandType.Date && OperandTypes[1] == OperandType.Date)
                        return (DateTime.Parse(Operands[0]) - DateTime.Parse(Operands[1])).Days.ToString();
                    //Start Date Operation 
                    if (OperandTypes[0] == OperandType.Date && (OperandTypes[1] == OperandType.Numeric || OperandTypes[1] == OperandType.TDay))
                        return (DateTime.Parse(Operands[0]).AddDays(-Convert.ToDouble(Operands[1]))).ToShortDateString();
                    if (OperandTypes[0] == OperandType.Date && OperandTypes[1] == OperandType.TMth)
                        return (DateTime.Parse(Operands[0]).AddMonths(-Convert.ToInt32(Operands[1]))).ToShortDateString();
                    if (OperandTypes[0] == OperandType.Date && OperandTypes[1] == OperandType.TYr)
                        return (DateTime.Parse(Operands[0]).AddYears(-Convert.ToInt32(Operands[1]))).ToShortDateString();
                    if (OperandTypes[1] == OperandType.Date && (OperandTypes[0] == OperandType.Numeric || OperandTypes[0] == OperandType.TDay))
                        return (DateTime.Parse(Operands[1]).AddDays(-Convert.ToDouble(Operands[0]))).ToShortDateString();
                    if (OperandTypes[1] == OperandType.Date && OperandTypes[0] == OperandType.TMth)
                        return (DateTime.Parse(Operands[1]).AddMonths(-Convert.ToInt32(Operands[0]))).ToShortDateString();
                    if (OperandTypes[1] == OperandType.Date && OperandTypes[0] == OperandType.TYr)
                        return (DateTime.Parse(Operands[1]).AddYears(-Convert.ToInt32(Operands[0]))).ToShortDateString();
                    //End Date Operation
                    if (OperandTypes[0] == OperandType.Numeric)
                        return (Convert.ToDecimal(Operands[0]) - Convert.ToDecimal(Operands[1])).ToString();
                    break;
                default:
                    throw new Exception("Invalid Arithmetic Operator (" + Sign + ")"); ;
            }

            return string.Empty;
        }

    }
    internal class ComparisonOperator : Operator
    {
        public ComparisonOperator(string sign, string uiLabel, OperatorAssociationType associationType, int precedence, int noOfOperands)
            : base(OperatorType.Comparison, OperatorResultType.Boolean)
        {
            Sign = sign;
            UILabel = uiLabel;
            AssociationType = associationType;
            Precedence = precedence;
            NoOfOperands = noOfOperands;
        }
        protected override bool ValidateOperand()
        {
            if (OperandTypes[0] != OperandTypes[1] && Sign != "=" && Sign != "!=" && (OperandTypes[0]!=OperandType.Text || OperandTypes[1]!=OperandType.Text))
                throw new Exception("Invalid Operand For Comparison Operation");
            else if (OperandTypes[0] == OperandType.Boolean && Sign != "=" && Sign != "!=")
                throw new Exception("Invalid Operand For Comparison Operation (" + OperandsToString() + ", " + Sign + ")");
            return true;
        }
        protected override string Execute()
        {
            switch (Sign)
            {
                case ">":
                    if (OperandTypes[0] == OperandType.Date && OperandTypes[1] == OperandType.Date)
                        return (DateTime.Parse(Operands[0]) > DateTime.Parse(Operands[1])).ToString();
                    else if (OperandTypes[0] == OperandType.Numeric && OperandTypes[1] == OperandType.Numeric)
                        return (Convert.ToDecimal(Operands[0]) > Convert.ToDecimal(Operands[1])).ToString();
                    else
                        return (string.Compare(Operands[0], Operands[1], true) > 0).ToString();
                case ">=":
                    if (OperandTypes[0] == OperandType.Date && OperandTypes[1] == OperandType.Date)
                        return (DateTime.Parse(Operands[0]) >= DateTime.Parse(Operands[1])).ToString();
                    else if (OperandTypes[0] == OperandType.Numeric && OperandTypes[1] == OperandType.Numeric)
                        return (Convert.ToDecimal(Operands[0]) >= Convert.ToDecimal(Operands[1])).ToString();
                    else
                        return (string.Compare(Operands[0], Operands[1], true) >= 0).ToString();
                case "<":
                    if (OperandTypes[0] == OperandType.Date && OperandTypes[1] == OperandType.Date)
                        return (DateTime.Parse(Operands[0]) < DateTime.Parse(Operands[1])).ToString();
                    else if (OperandTypes[0] == OperandType.Numeric && OperandTypes[1] == OperandType.Numeric)
                        return (Convert.ToDecimal(Operands[0]) < Convert.ToDecimal(Operands[1])).ToString();
                    else
                        return (string.Compare(Operands[0], Operands[1], true) < 0).ToString();
                case "<=":
                    if (OperandTypes[0] == OperandType.Date && OperandTypes[1] == OperandType.Date)
                        return (DateTime.Parse(Operands[0]) <= DateTime.Parse(Operands[1])).ToString();
                    else if (OperandTypes[0] == OperandType.Numeric && OperandTypes[1] == OperandType.Numeric)
                        return (Convert.ToDecimal(Operands[0]) <= Convert.ToDecimal(Operands[1])).ToString();
                    else
                        return (string.Compare(Operands[0], Operands[1], true) <= 0).ToString();
                case "=":
                    if (OperandTypes[0] == OperandType.Date && OperandTypes[1] == OperandType.Date)
                        return (DateTime.Parse(Operands[0]) == DateTime.Parse(Operands[1])).ToString();
                    else if (OperandTypes[0] == OperandType.Numeric && OperandTypes[1] == OperandType.Numeric)
                        return (Convert.ToDecimal(Operands[0]) == Convert.ToDecimal(Operands[1])).ToString();
                    else if (OperandTypes[0] == OperandType.Boolean && OperandTypes[1] == OperandType.Boolean)
                        return (Convert.ToBoolean(Operands[0]) == Convert.ToBoolean(Operands[1])).ToString();
                    else
                        return (string.Compare(Operands[0], Operands[1], true) == 0).ToString();
                case "!=":
                    if (OperandTypes[0] == OperandType.Date && OperandTypes[1] == OperandType.Date)
                        return (DateTime.Parse(Operands[0]) != DateTime.Parse(Operands[1])).ToString();
                    else if (OperandTypes[0] == OperandType.Numeric && OperandTypes[1] == OperandType.Numeric)
                        return (Convert.ToDecimal(Operands[0]) != Convert.ToDecimal(Operands[1])).ToString();
                    else if (OperandTypes[0] == OperandType.Boolean && OperandTypes[1] == OperandType.Boolean)
                        return (Convert.ToBoolean(Operands[0]) != Convert.ToBoolean(Operands[1])).ToString();
                    else
                        return (string.Compare(Operands[0], Operands[1], true) != 0).ToString();
                default:
                    throw new Exception("Invalid Comparison Operator (" + Sign + ")");
            }
        }

    }
    internal class LogicalOperator : Operator
    {
        public LogicalOperator(string sign, string uiLabel, OperatorAssociationType associationType, int precedence, int noOfOperands)
            : base(OperatorType.Logical, OperatorResultType.Boolean)
        {
            Sign = sign;
            UILabel = uiLabel;
            AssociationType = associationType;
            Precedence = precedence;
            NoOfOperands = noOfOperands;
        }
        protected override bool ValidateOperand()
        {
            if (OperandTypes[0] != OperandType.Boolean || ((!Sign.Equals("!",StringComparison.InvariantCulture)) && OperandTypes[1] != OperandType.Boolean))
                throw new Exception("Invalid Operand For Logical Operation (" + OperandsToString() + ", " + Sign + ")");
            return true;
        }
        protected override string Execute()
        {
            switch (Sign)
            {
                case "!":
                    return (!Boolean.Parse(Operands[0])).ToString();
                case "&&":
                    return (Boolean.Parse(Operands[0]) && Boolean.Parse(Operands[1])).ToString();
                case "||":
                    return (Boolean.Parse(Operands[0]) || Boolean.Parse(Operands[1])).ToString();
                default:
                    throw new Exception("Invalid Logical Operator (" + Sign + ")"); ;
            }
        }

    }
    internal class ConditionalOperator : Operator
    {
        public ConditionalOperator(string sign, string uiLabel, OperatorAssociationType associationType, int precedence, int noOfOperands)
            : base(OperatorType.Conditional, OperatorResultType.Value)
        {
            Sign = sign;
            UILabel = uiLabel;
            AssociationType = associationType;
            Precedence = precedence;
            NoOfOperands = noOfOperands;
        }
        protected override bool ValidateOperand()
        {
            if(OperandTypes[0]!=OperandType.Boolean)
                throw new Exception("First operand must be of type Boolean");

            if (OperandTypes[1] != OperandTypes[2] && (Operands[1] != string.Empty && Operands[2] != string.Empty))
                throw new Exception("Second and Third operand must be of same type for Conditional Operator");

            return true;
        }
        protected override string Execute()
        {
            switch (Sign)
            {
                case "?:":
                    if (Convert.ToBoolean(Operands[0]))
                        return Operands[1];
                    else
                        return Operands[2];

                default:
                    throw new Exception("Invalid Logical Operator (" + Sign + ")"); 
            }
        }

    }
    internal class DateFunction : Operator
    {
        public DateFunction(string sign, string uiLabel, OperatorAssociationType associationType, int precedence, int noOfOperands)
            : base(OperatorType.Conditional, OperatorResultType.Value)
        {
            Sign = sign;
            UILabel = uiLabel;
            AssociationType = associationType;
            Precedence = precedence;
            NoOfOperands = noOfOperands;
        }
        protected override bool ValidateOperand()
        {
            if (Sign.Equals("##DFCD##", StringComparison.InvariantCultureIgnoreCase) && (OperandTypes[0] != OperandType.Numeric || OperandTypes[1] != OperandType.Numeric || OperandTypes[2] != OperandType.Numeric))
                throw new Exception("All Operands must be of Integer type for Create Date function");

            if ((!Sign.Equals("##DFCD##", StringComparison.InvariantCultureIgnoreCase)) && OperandTypes[0] != OperandType.Date)
                throw new Exception("Operand must be of type Date for Date Functions");

            return true;
        }
        protected override string Execute()
        {
            switch (Sign)
            {
                case "##DFMONTH##":
                    return Convert.ToDateTime(Operands[0]).Month.ToString();
                case "##DFYEAR##":
                    return Convert.ToDateTime(Operands[0]).Year.ToString();
                case "##DFDAY##":
                    return Convert.ToDateTime(Operands[0]).Day.ToString();
                case "##DFCD##":
                    return new DateTime(Convert.ToInt32(Operands[0]), Convert.ToInt32(Operands[1]), Convert.ToInt32(Operands[2])).ToShortDateString();
                default:
                    throw new Exception("Invalid Logical Operator (" + Sign + ")");
            }
        }

    }
}
