using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
/// <summary>
/// This is a class representation of a mathematical formula. Taken a literal string (or collection of characters) and an environment with
/// variable definitions, it calculates the value of said formula.
/// </summary>
/// 
namespace INTSOF.RuleEngine.Expression
{
    internal class Evaluator
    {

        #region Static Members

        /// <summary>
        /// Static method to check if a token is an operator.
        /// </summary>
        /// <param name="token">The token we want to check.</param>
        /// <returns>True if it is an operator, else false.</returns>
        private static bool isOperator(String token)
        {
            return Operators.GetOperator(token) != null;
        }

        /// <summary>
        /// Static method to check if the type of operation is associative (left or right).
        /// </summary>
        /// <param name="token">The token operator.</param>
        /// <param name="type">The type of association (left or right).</param>
        /// <returns>True if it's associative, else false.</returns>
        private static bool isAssociative(String token, OperatorAssociationType type)
        {
            Operator op = Operators.GetOperator(token);
            if (op == null)
                throw new ArgumentException("Invalid token: " + token);

            if (op.AssociationType == type)
                return true;

            return false;
        }

        /// <summary>
        /// Static method to compare operator precendece.
        /// </summary>
        /// <param name="token1">First operator.</param>
        /// <param name="token2">Second operator.</param>
        /// <returns>The value of precedence between the two operators.</returns>
        private static int comparePrecedence(String token1, String token2)
        {
            Operator op1 = Operators.GetOperator(token1);
            Operator op2 = Operators.GetOperator(token2);
            if (op1 == null || op2 == null)
                throw new ArgumentException("Invalid token: " + token1 + " " + token2);

            return op1.Precedence - op2.Precedence;
        }

        /// <summary>
        /// Static method to transfor a normal formula into an RPN formula.
        /// </summary>
        /// <param name="input">The normal infix formula.</param>
        /// <returns>The RPN formula.</returns>
        public static String InfixToRPN(String input)
        {
            //TBD input cleanup code

            input = input.Trim();

            String[] inputTokens = input.Split(new string[] { INTSOF.RuleEngine.Utility.Constants.TOKEN_DELIMITER }, StringSplitOptions.RemoveEmptyEntries);
            List<string> outList = new List<string>();
            Stack<string> stack = new Stack<string>();

            foreach (string token in inputTokens)
            {
                if (token == " " || token == String.Empty)
                    continue;

                if (isOperator(token))
                {
                    while (stack.Count != 0 && isOperator(stack.Peek()))
                    {
                        if ((isAssociative(token, OperatorAssociationType.Left) && comparePrecedence(token, stack.Peek()) <= 0) ||
                            (isAssociative(token, OperatorAssociationType.Right) && comparePrecedence(token, stack.Peek()) < 0))
                        {
                            outList.Add(stack.Pop());
                            continue;
                        }
                        break;
                    }
                    stack.Push(token);
                }
                else if (token == "(")
                {
                    stack.Push(token);
                }
                else if (token == ")")
                {
                    while (stack.Count != 0 && stack.Peek() != "(")
                        outList.Add(stack.Pop());
                    if (stack.Count == 0)
                        throw new Exception("Missing Parenthesis");
                    stack.Pop();
                }
                else
                {
                    outList.Add(token);
                }
            }

            while (stack.Count != 0)
                outList.Add(stack.Pop());

            if (outList.Find(t => t.Equals("(") || t.Equals(")")) != null)
                throw new Exception("Missing Parenthesis");

            return String.Join(INTSOF.RuleEngine.Utility.Constants.TOKEN_DELIMITER, outList.ToArray());
        }

        /// <summary>
        /// Static method to handle a single operation between two operands.
        /// </summary>
        /// <param name="val1">First operand.</param>
        /// <param name="val2">Second operand.</param>
        /// <param name="OP">Operator.</param>
        /// <returns>The result of the operation.</returns>
        private string DoOperation(string [] vals, string OP)
        {
            Operator op = Operators.GetOperator(OP);
            try
            {
                foreach (string val in vals)
                {
                    if (val.Equals(Utility.Constants.RESULT_ERROR, StringComparison.OrdinalIgnoreCase))
                    {
                        if (op.ResultType == OperatorResultType.Boolean)
                            return Boolean.FalseString;
                        else
                            return Utility.Constants.RESULT_ERROR;
                    }
                }
                return op.Calculate(vals);
            }
            catch (Exception ex)
            {
                if (skipErrorForBool)
                {
                    skippedErrorMessage = skippedErrorMessage + " " + ex.Message;
                    if (op.ResultType == OperatorResultType.Boolean)
                        return Boolean.FalseString;
                    else
                        return Utility.Constants.RESULT_ERROR;
                }
                else
                    throw (ex);
            }
        }




        #endregion

        private bool skipErrorForBool = false;
        public bool SkipErrorForBool
        {
            get { return skipErrorForBool; }
            set { skipErrorForBool = value; }
        }

        private string skippedErrorMessage = string.Empty;
        public string SkippedErrorMessage
        {
            get { return skippedErrorMessage; }
            set { skippedErrorMessage = value; }
        }


        /// <summary>
        /// Environment for all the variables in the formula.
        /// </summary>
        private Dictionary<string, float> environment = new Dictionary<string, float>();

        /// <summary>
        /// Written formula in normal form.
        /// </summary>
        private string formula;

        /// <summary>
        /// Written formula in Reverse Polish Notation form.
        /// </summary>
        private string rpnFormula;

        private bool hasToken;
        /// <summary>
        /// Value of a parameter in the environment.
        /// </summary>
        /// <param name="param">Parameter name.</param>
        /// <returns>Value of the parameter.</returns>
        public float this[string param]
        {
            get
            {
                return environment[param];
            }
            set
            {
                environment[param] = value;
            }
        }

        /// <summary>
        /// Value of the formula.
        /// </summary>
        public string Value
        {
            get
            {
                return Calculate().ToString();
            }
        }



        /// <summary>
        /// Method use to calculate the value of the formula.
        /// </summary>
        /// <returns>The floating point value of the formula.</returns>
        private string Calculate()
        {
            String[] tokens = rpnFormula.Split(new string[] { INTSOF.RuleEngine.Utility.Constants.TOKEN_DELIMITER }, StringSplitOptions.RemoveEmptyEntries);
            Stack<string> values = new Stack<string>();

            foreach (string token in tokens)
            {
                Operator op = Operators.GetOperator(token);
                if (op == null)
                {
                    values.Push(token);
                }
                else
                {
                    if (op.NoOfOperands > 0)
                    {
                        string[] vals = new string[op.NoOfOperands];
                        for (int operandCounter = op.NoOfOperands-1; operandCounter >= 0; operandCounter--)
                        {
                            vals[operandCounter] = values.Pop();
                        }
                        values.Push(DoOperation(vals, token));
                    }
                }
            }

            if (values.Count != 1)
                throw new InvalidOperationException("Cannot calculate expression.");

            return values.Pop();
        }

        public string SimulateCalculation()
        {
            String[] tokens = rpnFormula.Split(new string[] { INTSOF.RuleEngine.Utility.Constants.TOKEN_DELIMITER }, StringSplitOptions.RemoveEmptyEntries);
            Stack<string> values = new Stack<string>();

            foreach (string token in tokens)
            {
                Operator op = Operators.GetOperator(token);
                if (op == null)
                {
                    values.Push(token);
                }
                else
                {
                    if (op.NoOfOperands > 0)
                    {
                        string[] vals = new string[op.NoOfOperands];
                        for (int operandCounter = op.NoOfOperands - 1; operandCounter >= 0; operandCounter--)
                        {
                            vals[operandCounter] = values.Pop();
                        }
                        values.Push("DummyResult");
                    }
                }
            }

            if (values.Count != 1)
                throw new InvalidOperationException("Cannot calculate expression.");

            return values.Pop();
        }


        /// <summary>
        /// String representation of the formula.
        /// </summary>
        public string Formula
        {
            get
            {
                return formula;
            }
            set
            {
                rpnFormula = InfixToRPN(value);
                hasToken = (rpnFormula.Split(new string[] { INTSOF.RuleEngine.Utility.Constants.TOKEN_DELIMITER }, StringSplitOptions.RemoveEmptyEntries).Length > 0);
                formula = value;
            }
        }

        public bool HasToken
        {
            get { return hasToken; }
        }

        /// <summary>
        /// RPN Representation of formula
        /// </summary>
        public string RPNFormula
        {
            get
            {
                return rpnFormula;
            }
        }

        /// <summary>
        /// RPN Representation of formula
        /// </summary>
        public OperatorResultType ResultType
        {
            get
            {
                int lastIndex = rpnFormula.LastIndexOf(INTSOF.RuleEngine.Utility.Constants.TOKEN_DELIMITER);
                int delimiterLength = INTSOF.RuleEngine.Utility.Constants.TOKEN_DELIMITER.Length;
                if (lastIndex == -1)
                    return OperatorResultType.Undefined;
                string lastOperator = rpnFormula.Substring(lastIndex + delimiterLength, rpnFormula.Length - lastIndex - delimiterLength);
                return Operators.GetOperator(lastOperator).ResultType;
            }
        }

        /// <summary>
        /// String representation of the formula.
        /// </summary>
        /// <returns>The string formula.</returns>
        public override string ToString()
        {
            return formula;
        }

        /// <summary>
        /// CTor.
        /// </summary>
        /// <param name="formula">Infix Notation of the formula.</param>
        public Evaluator(string formula)
        {
            this.formula = formula;
            rpnFormula = InfixToRPN(formula);
            hasToken = (rpnFormula.Split(new string[] { INTSOF.RuleEngine.Utility.Constants.TOKEN_DELIMITER }, StringSplitOptions.RemoveEmptyEntries).Length > 0);
        }

        /// <summary>
        /// Adds a parameter (variable) to the environment.
        /// </summary>
        /// <param name="param">Parameter name.</param>
        /// <param name="value">Paramatere value.</param>
        public void AddParameter(string param, float value)
        {
            environment.Add(param, value);
        }

        /// <summary>
        /// Checks if the environment contains the passed parameter.
        /// </summary>
        /// <param name="param">Name of the parameter.</param>
        /// <returns>True if it contains the parameter, else false.</returns>
        public bool ContainsParameter(string param)
        {
            return environment.ContainsKey(param);
        }
    }
}