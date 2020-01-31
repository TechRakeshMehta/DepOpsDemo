using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace INTSOF.Complio.API.ComplioAPIDAL
{
    public class ComplioAPIRespository
    {
        /// <summary>
        /// Get Data from the Database
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="entityTypeCode"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static String GetData(int tenantId, string entityTypeCode, string xmlData)
        {
            String _securityDBConnection = ConfigurationManager.ConnectionStrings["securityDBConnection"].ConnectionString;
            String _output = String.Empty;

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
                           new SqlParameter("@XmlData", xmlData)  
                        };


                using (SqlCommand sqlCommand = new SqlCommand("usp_GetData", _connection))
                {
                    sqlCommand.Parameters.AddRange(sqlParameterCollection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    SqlDataReader dr = sqlCommand.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            _output = Convert.ToString(dr[0]);
                        }
                    }
                }

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            return _output;
        }

        /// <summary>
        /// Add the data, based on stored procedure name passed
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="entityTypeCode"></param>
        /// <param name="xmlData"></param>
        /// <param name="spName"></param>
        /// <returns></returns>
        public static String AddData(int tenantId, string entityTypeCode, string xmlData)
        {
            String _output = String.Empty;
            String _securityDBConnection = ConfigurationManager.ConnectionStrings["securityDBConnection"].ConnectionString;

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
                           new SqlParameter("@XmlData", xmlData)  
                        };

                using (SqlCommand sqlCommand = new SqlCommand("usp_AddData", _connection))
                {
                    sqlCommand.Parameters.AddRange(sqlParameterCollection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    SqlDataReader dr = sqlCommand.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            _output = Convert.ToString(dr[0]);
                        }
                    }
                }

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            return _output;
        }

        /// <summary>
        /// Update the data, based on stored procedure name passed
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="entityTypeCode"></param>
        /// <param name="xmlData"></param>
        /// <param name="spName"></param>
        /// <returns></returns>
        public static String UpdateData(int tenantId, string entityTypeCode, string xmlData)
        {
            String _output = String.Empty;
            String _securityDBConnection = ConfigurationManager.ConnectionStrings["securityDBConnection"].ConnectionString;

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
                           new SqlParameter("@XmlData", xmlData)  
                        };

                using (SqlCommand sqlCommand = new SqlCommand("usp_UpdateData", _connection))
                {
                    sqlCommand.Parameters.AddRange(sqlParameterCollection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = sqlCommand.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            _output = Convert.ToString(dr[0]);
                        }
                    }
                }

                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            return _output;
        }


        /// <summary>
        /// Get Data from the Database
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="entityTypeCode"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static TokenValidateData GetTokenValidateData(int tenantId, string entityTypeCode, string mappingCode)
        {
            String _securityDBConnection = ConfigurationManager.ConnectionStrings["securityDBConnection"].ConnectionString;
            TokenValidateData output = new TokenValidateData();

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

        /// <summary>
        /// Get Data from the Database
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="entityTypeCode"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static Boolean IsClientTokenValidateRequired(int tenantId, string entityTypeCode)
        {
            String _securityDBConnection = ConfigurationManager.ConnectionStrings["securityDBConnection"].ConnectionString;
            Boolean output = false; ;

            using (SqlConnection _connection = new SqlConnection(_securityDBConnection))
            {

                if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    _connection.Open();
                }

                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@TenantID", tenantId), 
                           new SqlParameter("@EntityTypeCode", entityTypeCode)
                        };


                using (SqlCommand sqlCommand = new SqlCommand("api.usp_IsClientTokenValidateRequired", _connection))
                {
                    sqlCommand.Parameters.AddRange(sqlParameterCollection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    SqlDataReader dr = sqlCommand.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            output = Convert.ToBoolean(dr[0]);
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

        /// <summary>
        /// Check from database
        /// </summary>
        /// <param name="entityTypeCode"></param>
        /// <returns></returns>
        public static Boolean IsJsonReturn(string entityTypeCode)
        {
            String _securityDBConnection = ConfigurationManager.ConnectionStrings["securityDBConnection"].ConnectionString;
            Boolean result = false;

            using (SqlConnection _connection = new SqlConnection(_securityDBConnection))
            {

                if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    _connection.Open();
                }
                using (SqlCommand sqlCommand = new SqlCommand("SELECT API_IsJSON FROM apiMetadata WHERE API_EntityCode ='" + entityTypeCode + "' AND API_IsDeleted = 0 ", _connection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    SqlDataReader dr = sqlCommand.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            result = dr["API_IsJSON"] == DBNull.Value ? false : Convert.ToBoolean(dr["API_IsJSON"]);
                        }
                    }
                }
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            return result;
        }
    }
}