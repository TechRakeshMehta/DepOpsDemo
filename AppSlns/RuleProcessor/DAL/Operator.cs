using System;
using System.Collections.Generic;
using System.Text;

using INTSOF.RuleEngine.Expression;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace INTSOF.RuleEngine.DAL
{
    internal class DALOperator
    {
        public static List<INTSOF.RuleEngine.Expression.Operator> GetOperators()
        {
            List<INTSOF.RuleEngine.Expression.Operator> operators = new List<INTSOF.RuleEngine.Expression.Operator>();

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM vwOperatorTypes WHERE EOT_Code IN ('AROP','CMOP','LGOP','CDOP','DTFN')";

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string sign, uiLabel, operatorTypeCode;
                        OperatorAssociationType associationType;
                        int precedence, noOfOperands;
                        operatorTypeCode = reader["EOT_Code"].ToString();
                        sign = reader["EO_SQL"].ToString();
                        uiLabel = reader["EO_UILabel"].ToString();
                        associationType = (Convert.ToBoolean(reader["EO_IsLeftAssociationType"]) ? OperatorAssociationType.Left : OperatorAssociationType.Right);
                        precedence = Convert.ToInt32(reader["EO_Precedence"]);
                        noOfOperands = Convert.ToInt32(reader["EO_NoOfOperands"]);
                        operators.Add(Operators.GetOperator(operatorTypeCode, sign, uiLabel, associationType, precedence, noOfOperands));
                    }
                    return operators;
                }
            }

        }
    }
}
