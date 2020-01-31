using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using INTSOF.RuleEngine.Model;

namespace INTSOF.RuleEngine.DAL
{
    internal class DALRuleDefinition
    {
        public static RuleDefinition Get(int id)
        {
            return Get(id, "dbo");
        }
        public static RuleDefinition Get(int id, string schema)
        {
            RuleDefinition rule = new RuleDefinition();
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = schema+".usp_Rule_GetDefinition";

                command.Parameters.Add(new SqlParameter("@RuleID", id));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        rule.Name = reader["Name"].ToString();
                        rule.Version = reader["Version"].ToString();
                        rule.ActionType = reader["ActionType"].ToString();
                        rule.ActionBlock = reader["ActionBlock"].ToString();
                        rule.ResultType = reader["ResultType"].ToString();
                        rule.RuleType = reader["RuleType"].ToString();
                        rule.SuccessMessage = reader["SuccessMessage"].ToString();
                        rule.ErrorMessage = reader["ErrorMessage"].ToString();

                        if (reader.NextResult())
                        {
                            rule.Expressions = new List<Model.Expression>();
                            while (reader.Read())
                            {
                                Model.Expression e = new Model.Expression();
                                e.Name = reader["EX_Name"].ToString();
                                e.Definition = reader["EX_Expression"].ToString();
                                rule.Expressions.Add(e);
                            }
                        }
                    }
                }

            }
            return rule;
        }

        public static string GetObjectMapping(int userID, int rootObjectID, int mappingDetailID, out string happyValue)
        {
            return GetObjectMappingAsOn(userID, rootObjectID, mappingDetailID, DateTime.Now, out happyValue);
        }
        public static string GetObjectMappingAsOn(int userID, int rootObjectID, int mappingDetailID, DateTime asOnDate, out string happyValue)
        {
            string retVal = string.Empty;
            happyValue = string.Empty;
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_Rule_GetObjectMappedValue_CLR";

                command.Parameters.Add(new SqlParameter("@UserID", userID));
                command.Parameters.Add(new SqlParameter("@RootObjectKeyValue", rootObjectID));
                command.Parameters.Add(new SqlParameter("@MappingDetailID", mappingDetailID));
                command.Parameters.Add(new SqlParameter("@AsOnDate", asOnDate));
                
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        try
                        {
                            happyValue = reader["HappyValue"].ToString();
                        }
                        catch { }

                        retVal = reader["Value"].ToString();
                    }
                }
                return retVal;
            }
        }
    }
}
