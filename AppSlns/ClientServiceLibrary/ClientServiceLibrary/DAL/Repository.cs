using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServiceLibrary.DAL
{
    public class Repository
    {
        /// <summary>
        /// Get Data from the Database
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="entityTypeCode"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static TokenValidateDataContract GetTokenValidateData(int tenantId, string entityTypeCode, string mappingCode)
        {
            String _securityDBConnection = ConfigurationManager.ConnectionStrings["ConnectionName"].ConnectionString;
            TokenValidateDataContract output = new TokenValidateDataContract();

            using (SqlConnection _connection = new SqlConnection(_securityDBConnection))
            {

                if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    _connection.Open();
                }

                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@TenantID", tenantId), 
                           new SqlParameter("@EntityTypeCode", entityTypeCode),
                           new SqlParameter("@MappingCode", mappingCode)  
                        };


                using (SqlCommand sqlCommand = new SqlCommand("api.usp_GetTokenValidateData", _connection))
                {
                    sqlCommand.Parameters.AddRange(sqlParameterCollection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    SqlDataReader dr = sqlCommand.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            output.TokenValidateFormat = dr["TokenValidateFormat"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TokenValidateFormat"]);
                            output.TokenValidateURL = dr["TokenValidateURL"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TokenValidateURL"]);
                            //output.ValidateToken = dr["ValidateToken"] == DBNull.Value ? false : Convert.ToBoolean(dr["ValidateToken"]);
                            // output.ValidateTypeCode = dr["ValidateTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ValidateTypeCode"]);
                        }
                    }
                }

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            return output;
        }
    }
}
