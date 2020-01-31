using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using INTSOF.RuleEngine.Model;

namespace INTSOF.RuleEngine.DAL
{
    internal class DALObjectRule
    {
        public static List<RuleDefinition> GetRules()
        {
            List<RuleDefinition> rules = new List<RuleDefinition>();
            Dictionary<int, bool> ruleIds = new Dictionary<int, bool>();
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT DISTINCT RuleMappingID, NeedMinRequirementCheck FROM #Rules";

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ruleId = Convert.ToInt32(reader["RuleMappingID"]);
                        ruleIds.Add(ruleId, Convert.ToBoolean(reader["NeedMinRequirementCheck"]));
                    }
                }
            }
            foreach (int ruleId in ruleIds.Keys)
            {
                RuleDefinition rule = DALRuleDefinition.Get(ruleId);
                rule.RuleMappingID = ruleId;
                rule.NeedMinRequirementCheck = ruleIds[ruleId];
                rules.Add(rule);
            }
            foreach (RuleDefinition rule in rules)
            {
                rule.ObjectMappings = GetObjectMapping(rule.RuleMappingID);
            }
            return rules;
        }

        private static ObjectMapping GetObjectMapping(int ruleID)
        {
            ObjectMapping objectMapping = new ObjectMapping();
            objectMapping.Mappings = new List<Mapping>();

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT RuleMappingDetailID,PlaceHolder FROM #Rules WHERE RuleMappingID=" + ruleID.ToString();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Mapping mapping = new Mapping();
                        mapping.ID = Convert.ToInt32(reader["RuleMappingDetailID"]);
                        mapping.Key = reader["PlaceHolder"].ToString();
                        objectMapping.Mappings.Add(mapping);
                    }
                }
            }
            return objectMapping;
        }
        public static void CreateHistory(int userID, int rootObjectID, int ruleObjectTypeID, int ruleObjectID, int ruleId, RuleProcessingResult result)
        {
            ObjectMapping objectMapping = new ObjectMapping();
            objectMapping.Mappings = new List<Mapping>();

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO RuleExecutionHistory"
                    + "(REH_RootObjectID,REH_RuleObjectTypeID,REH_RuleObjectID,REH_RuleID,REH_RuleName,REH_Result,REH_ResultXML,REH_CreatedByID)"
                    + " VALUES (" + rootObjectID.ToString() + "," + ruleObjectTypeID.ToString() + "," + ruleObjectID.ToString()
                    + "," + ruleId.ToString() + ",'" + result.RuleName + "','" + result.Result + "','" + result.ToXML() + "'," + userID.ToString() + ");";
                command.ExecuteNonQuery();
            }
        }
    }
}
