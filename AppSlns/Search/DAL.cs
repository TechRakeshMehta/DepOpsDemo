using INTSOF.Utils;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Search
{
    public class DAL
    {
        #region Private Variables

        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public Methods

        public SqlConnection CreateConnection(String connectionString)
        {
            SqlConnection sqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = connectionString;
                sqlConnection.Open();
            }
            catch (Exception ex)
            {
                logger.Error("** Error in CreateConnection: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
            return sqlConnection;
        }

        public DataSet GetDataSet(SqlConnection connection, CommandType commandType, String commandText, DataSet resultDataSet, String resultType, SqlParameter[] sqlParameterCollection = null)
        {
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = commandType;
                    sqlCommand.CommandText = commandText;

                    if (sqlParameterCollection != null)
                    {
                        sqlCommand.Parameters.AddRange(sqlParameterCollection);
                    }
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(resultDataSet, resultType);
                    return resultDataSet;
                }
            }
            catch (Exception ex)
            {
                logger.Error("** Error in GetDataSet: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public Object GetParameterValue(SqlConnection connection, CommandType commandType, String commandText, SqlParameter[] sqlParameter = null)
        {
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = commandType;
                    sqlCommand.CommandText = commandText;

                    if (sqlParameter.IsNotNull())
                    {
                        sqlCommand.Parameters.AddRange(sqlParameter);
                    }
                    return sqlCommand.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                logger.Error("** Error in GetParameterValue: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public DataSet CreateError(DataSet resultDataSet, List<String> errorMessages)
        {
            try
            {
                DataTable error = new DataTable(SearchDataTable.Error);
                error.Columns.Add("Message", typeof(String));

                foreach (var message in errorMessages)
                {
                    DataRow dataRow = error.NewRow();
                    dataRow["Message"] = message;
                    error.Rows.Add(dataRow);
                }
                resultDataSet.Tables.Add(error);
                return resultDataSet;
            }
            catch (Exception ex)
            {
                logger.Error("** Error in CreateError: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public DataSet GetSearchType(SqlConnection searchConnection, String searchTypeCode, DataSet searchDataSet)
        {
            try
            {
                String commandText = "SELECT * FROM lkpSearchType WHERE ST_SearchCode = @ST_SearchCode";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@ST_SearchCode", searchTypeCode)
                        };
                searchDataSet = GetDataSet(searchConnection, CommandType.Text, commandText, searchDataSet, "lkpSearchType", sqlParameterCollection);

                if (searchDataSet.Tables.Count > 0 && searchDataSet.Tables["lkpSearchType"].IsNotNull() && searchDataSet.Tables["lkpSearchType"].Rows.Count > 0)
                {
                    return searchDataSet;
                }
            }
            catch (Exception ex)
            {
                logger.Error("** Error in GetSearchType: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
            return null;
        }

        public String GetTenantConnectionString(String securityConnectionString, Int32 tenantID)
        {
            try
            {
                String commandText = "SELECT CDB_ConnectionString FROM ClientDBConfiguration WHERE CDB_TenantID = @CDB_TenantID;";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@CDB_TenantID", tenantID)
                        };

                using (SqlConnection securityConnection = new SqlConnection())
                {
                    securityConnection.ConnectionString = securityConnectionString;
                    securityConnection.Open();
                    return Convert.ToString(GetParameterValue(securityConnection, CommandType.Text, commandText, sqlParameterCollection));
                }
            }
            catch (Exception ex)
            {
                logger.Error("** Error in GetTenantConnectionString: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public DataSet ExecuteSearchUSP(DataSet resultDataSet, MasterSearchObject masterSearchObject, String tenantConnectionString)
        {
            try
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@OrderBy", masterSearchObject.OrderBy),
                           new SqlParameter("@OrderDirection", masterSearchObject.SortDirection),
                           new SqlParameter("@DataBaseName", masterSearchObject.SearchDatabaseName),
                           new SqlParameter("@ResultTableName", masterSearchObject.MasterTableName),
                           new SqlParameter("@PageIndex", masterSearchObject.PageIndex),
                           new SqlParameter("@PageSize", masterSearchObject.PageSize),
                           new SqlParameter("@ReturnResultType", ReturnType.Return),
                           new SqlParameter("@SearchInstanceId", masterSearchObject.SearchInstanceID),
                           new SqlParameter("@SearchParameter", masterSearchObject.SearchParameters)
                        };
                //Get tenant connection.
                using (SqlConnection tenantConnection = new SqlConnection())
                {
                    tenantConnection.ConnectionString = tenantConnectionString;
                    tenantConnection.Open();
                    return GetDataSet(tenantConnection, CommandType.StoredProcedure, masterSearchObject.SearchUSP, resultDataSet, SearchDataTable.SearchResult, sqlParameterCollection);
                }
            }
            catch (Exception ex)
            {
                logger.Error("** Error in ExecuteSearchUSP: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public String GetStatusCodeForSearchInstanceID(SqlConnection searchConnection, Int32 searchInstanceID)
        {
            try
            {
                String commandText = "SELECT SRS_Code FROM SearchResultInstance SRI INNER JOIN lkpSearchResultStatus LSRS ON SRI.SRI_StatusID = LSRS.SRS_ID WHERE SRI_IsDeleted = 0 AND SRI_ID = @SRI_ID";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@SRI_ID", searchInstanceID)
                        };
                return Convert.ToString(GetParameterValue(searchConnection, CommandType.Text, commandText, sqlParameterCollection));
            }
            catch (Exception ex)
            {
                logger.Error("** Error in GetStatusCodeForSearchInstanceID: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public Boolean CheckInProgressSearchCount(SqlConnection searchConnection, Int32 totalSearchAllowed)
        {
            try
            {
                String commandText = "SELECT COUNT(*) AS Count FROM SearchResultInstance SRI INNER JOIN lkpSearchResultStatus LSRS ON SRI.SRI_StatusID = LSRS.SRS_ID WHERE SRI_IsDeleted = 0 AND LSRS.SRS_Code = @SRS_Code;";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@SRS_Code", LkpSearchResultStatus.InProgress)
                        };
                Object count = GetParameterValue(searchConnection, CommandType.Text, commandText, sqlParameterCollection);

                if (count != null && totalSearchAllowed != 0 && Convert.ToInt32(count) == totalSearchAllowed)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("** Error in CheckInProgressSearchCount: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
            return true;
        }

        public Boolean CheckInProgressSearchForSearhType(SqlConnection searchConnection, Int32 totalSearchPerSearchTypeAllowed, Int32 searchTypeId)
        {
            try
            {
                String commandText = "SELECT COUNT(*) AS Count FROM SearchResultInstance SRI INNER JOIN lkpSearchResultStatus LSRS ON SRI.SRI_StatusID = LSRS.SRS_ID WHERE SRI_IsDeleted = 0 AND LSRS.SRS_Code = @SRS_Code AND SRI_SearchTypeId = @SRI_SearchTypeId;";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@SRS_Code", LkpSearchResultStatus.InProgress),
                           new SqlParameter("@SRI_SearchTypeId", searchTypeId)
                        };
                Object count = GetParameterValue(searchConnection, CommandType.Text, commandText, sqlParameterCollection);

                if (count != null && totalSearchPerSearchTypeAllowed != 0 && Convert.ToInt32(count) == totalSearchPerSearchTypeAllowed)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("** Error in CheckInProgressSearchForSearhType: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
            return true;
        }

        public DataTable GetAllTenantList(String securityConnectionString, DataSet searchDataSet, ArrayList tenantIDList = null)
        {
            try
            {
                String commandText = "SELECT * FROM ClientDBConfiguration CDC INNER JOIN Tenant T ON CDC.CDB_TenantID = T.TenantID INNER JOIN lkpTenantType LTY ON T.TenantTypeID = LTY.TenantTypeID WHERE LTY.TenantTypeCode = '"
                    + TenantType.Institution.GetStringValue() + "'";

                if (tenantIDList != null)
                {
                    commandText = commandText + " AND T.TenantID IN (" + String.Join(",", tenantIDList.ToArray()) + ")";
                }

                using (SqlConnection securityConnection = new SqlConnection())
                {
                    securityConnection.ConnectionString = securityConnectionString;
                    securityConnection.Open();
                    searchDataSet = GetDataSet(securityConnection, CommandType.Text, commandText, searchDataSet, "ClientDBConfiguration");

                    if (searchDataSet.Tables.Count > 0 && searchDataSet.Tables["ClientDBConfiguration"].Rows.Count > 0)
                    {
                        return searchDataSet.Tables["ClientDBConfiguration"];
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("** Error in GetAllTenantList: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
            return null;
        }

        public DataSet GetlkpSearchResultStatus(SqlConnection searchConnection, DataSet searchDataSet)
        {
            try
            {
                String commandText = "SELECT * FROM lkpSearchResultStatus";
                return GetDataSet(searchConnection, CommandType.Text, commandText, searchDataSet, "lkpSearchResultStatus");
            }
            catch (Exception ex)
            {
                logger.Error("** Error in GetlkpSearchResultStatus: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public Int32 InsertSearchResultInstance(SqlConnection searchConnection, DataSet searchDataSet, Int32 inProgressStatusID, MasterSearchObject masterSearchObject)
        {
            try
            {
                //String searchInstanceName = userName + _ + lkpSearchType.SearchName + _ + mmddyyyy_hhmm
                String searchName = Convert.ToString(searchDataSet.Tables["lkpSearchType"].Rows[0]["ST_SearchName"]);
                String searchInstanceName = masterSearchObject.UserName + "_" + searchName + "_" + DateTime.Now.ToString("MMddyyyy_hhmm");

                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@SRI_SearchTypeId", masterSearchObject.SearchTypeID),
                           new SqlParameter("@SRI_SearchInstanceName", searchInstanceName),
                           new SqlParameter("@SRI_StatusID", inProgressStatusID),
                           new SqlParameter("@SRI_LastSortBy", masterSearchObject.OrderBy),
                           new SqlParameter("@SRI_LastSortDirection", masterSearchObject.SortDirection),
                           new SqlParameter("@SRI_UserId", masterSearchObject.UserID),
                           new SqlParameter("@SRI_IsDeleted", false),
                           new SqlParameter("@SRI_CreatedOn", DateTime.Now),
                           new SqlParameter("@SRI_CreatedByID", Consts.SEARCH_PROCESS_ID),
                           new SqlParameter("@SRI_SearchScopeId", masterSearchObject.SearchScopeID),
                           new SqlParameter("@SRI_SearchParams", masterSearchObject.SearchParameters)
                        };
                //SearchResultInstanceId  = Inesrt Into SearchResultInstance(SearchInstance, SearchTypeId, searchInstanceName, Datetime, ‘In Progress’);
                Object searchInstanceID = GetParameterValue(searchConnection, CommandType.StoredProcedure, "usp_InsertSearchResultInstance", sqlParameterCollection);
                return searchInstanceID == null ? 0 : Convert.ToInt32(searchInstanceID);
            }
            catch (Exception ex)
            {
                logger.Error("** Error in InsertSearchResultInstance: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public String GetUserName(String securityConnectionString, Int32 userID)
        {
            try
            {
                String commandText = "SELECT LastName + '_' + FirstName AS UserName FROM OrganizationUser WHERE OrganizationUserID = @OrganizationUserID";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@OrganizationUserID", userID)
                        };

                using (SqlConnection securityConnection = new SqlConnection())
                {
                    securityConnection.ConnectionString = securityConnectionString;
                    securityConnection.Open();
                    return Convert.ToString(GetParameterValue(securityConnection, CommandType.Text, commandText, sqlParameterCollection));
                }
            }
            catch (Exception ex)
            {
                logger.Error("** Error in GetUserName: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public DataSet GetSearchResultInstance(SqlConnection searchConnection, Int32 searchInstanceID, DataSet resultDataSet)
        {
            try
            {
                String commandText = "SELECT * FROM SearchResultInstance WHERE SRI_ID = @SRI_ID;";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@SRI_ID", searchInstanceID)
                        };
                resultDataSet = GetDataSet(searchConnection, CommandType.Text, commandText, resultDataSet, SearchDataTable.SearchResultInstance, sqlParameterCollection);

                if (resultDataSet.Tables.Count > 0 && resultDataSet.Tables[SearchDataTable.SearchResultInstance].IsNotNull() && resultDataSet.Tables[SearchDataTable.SearchResultInstance].Rows.Count > 0)
                {
                    return resultDataSet;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.Error("** Error in GetSearchResultInstance: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public void UpdatelkpSearchResultTables(SqlConnection searchConnection, Int32 searchInstanceID, Int32 searchTypeID, Int32 parallelThreadCount, String searchParameters)
        {
            try
            {
                String commandText = "UPDATE TOP(@ParallelThreadCount) lkpSearchResultTables SET SRT_InUseSearchResultInstanceId = @SRT_InUseSearchResultInstanceId, SRT_SearchParameters = @SRT_SearchParameters WHERE SRT_SearchTypeId = @SRT_SearchTypeId AND SRT_InUseSearchResultInstanceId IS NULL";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@ParallelThreadCount", parallelThreadCount),
                           new SqlParameter("@SRT_InUseSearchResultInstanceId", searchInstanceID),
                           new SqlParameter("@SRT_SearchParameters", searchParameters),
                           new SqlParameter("@SRT_SearchTypeId", searchTypeID),
                        };
                GetParameterValue(searchConnection, CommandType.Text, commandText, sqlParameterCollection);
            }
            catch (Exception ex)
            {
                logger.Error("** Error in UpdatelkpSearchResultTables: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public DataSet GetAvailableSearchResultTables(SqlConnection searchConnection, Int32 searchInstanceID, Int32 searchTypeID, DataSet searchDataSet)
        {
            try
            {
                String commandText = "SELECT * FROM lkpSearchResultTables WHERE SRT_SearchTypeId = @SRT_SearchTypeId AND SRT_InUseSearchResultInstanceId = @SRT_InUseSearchResultInstanceId;";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@SRT_SearchTypeId", searchTypeID),
                           new SqlParameter("@SRT_InUseSearchResultInstanceId", searchInstanceID)
                        };
                searchDataSet = GetDataSet(searchConnection, CommandType.Text, commandText, searchDataSet, "lkpSearchResultTables", sqlParameterCollection);

                if (searchDataSet.Tables.Count > 0 && searchDataSet.Tables["lkpSearchResultTables"].IsNotNull() && searchDataSet.Tables["lkpSearchResultTables"].Rows.Count > 0)
                {
                    return searchDataSet;
                }
            }
            catch (Exception ex)
            {
                logger.Error("** Error in GetAvailableSearchResultTables: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
            return null;
        }

        public DataSet GetMasterResultBySearchInstanceID(SqlConnection searchConnection, MasterSearchObject masterSearchObject, String retType, DataSet resultDataSet)
        {
            try
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@OrderBy", masterSearchObject.OrderBy),
                    new SqlParameter("@OrderDirection", masterSearchObject.SortDirection),
                    new SqlParameter("@PageIndex", masterSearchObject.PageIndex),
                    new SqlParameter("@PageSize", masterSearchObject.PageSize),
                    new SqlParameter("@SearchInstanceId", masterSearchObject.SearchInstanceID),
                    new SqlParameter("@ReturnResultType", retType),
                    new SqlParameter("@MasterTableName", masterSearchObject.MasterTableName),
                    new SqlParameter("@UserId", masterSearchObject.UserID),
                    new SqlParameter("@SearchScopeId", masterSearchObject.SearchScopeID)
                };
                return GetDataSet(searchConnection, CommandType.StoredProcedure, "usp_MultipleTenantSearch", resultDataSet, SearchDataTable.SearchResult, sqlParameterCollection);
            }
            catch (Exception ex)
            {
                logger.Error("** Error in GetMasterResultBySearchInstanceID: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public Boolean CheckIfErrorInAvailableResultTables(SqlConnection searchConnection, Int32 errorStatusID, Int32 searchInstanceID, DataSet resultDataSet)
        {
            try
            {
                String commandText = "SELECT * FROM SearchResultProgress WHERE SRP_StatusID = @SRP_StatusID AND SRP_SearchResultInstanceId = @SRP_SearchResultInstanceId;";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@SRP_StatusID", errorStatusID),
                           new SqlParameter("@SRP_SearchResultInstanceId", searchInstanceID)
                        };
                resultDataSet = GetDataSet(searchConnection, CommandType.Text, commandText, resultDataSet, SearchDataTable.Error, sqlParameterCollection);

                if (resultDataSet.Tables.Count > 0 && resultDataSet.Tables[SearchDataTable.Error].IsNotNull() && resultDataSet.Tables[SearchDataTable.Error].Rows.Count > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Error("** Error in CheckIfErrorInAvailableResultTables: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
            return false;
        }

        public void UpdateSearchResultInstanceStatus(SqlConnection searchConnection, MasterSearchObject masterSearchObject, Int32 statusId, Int32 completedStatusId)
        {
            try
            {
                String commandText = "UPDATE SearchResultInstance SET SRI_StatusID = @SRI_StatusID, SRI_ModifiedByID = @SRI_ModifiedByID, SRI_ModifiedOn = @SRI_ModifiedOn  WHERE SRI_ID = @SRI_ID AND SRI_StatusID <> @CompletedStatusId;";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@SRI_StatusID", statusId),
                           new SqlParameter("@SRI_ModifiedByID", Consts.SEARCH_PROCESS_ID),
                           new SqlParameter("@SRI_ModifiedOn", DateTime.Now),
                           new SqlParameter("@SRI_ID", masterSearchObject.SearchInstanceID),
                           new SqlParameter("@CompletedStatusId", completedStatusId)
                        };
                GetParameterValue(searchConnection, CommandType.Text, commandText, sqlParameterCollection);
            }
            catch (Exception ex)
            {
                logger.Error("** Error in UpdateSearchResultInstanceStatus: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public void UpdateSearchResultInstance(SqlConnection searchConnection, MasterSearchObject masterSearchObject, Int32 completedStatusId, DataSet resultDataSet)
        {
            try
            {
                Int32 recordCount = 0;

                if (resultDataSet.Tables[SearchDataTable.SearchResult] != null && resultDataSet.Tables[SearchDataTable.SearchResult].Rows.Count > 0)
                {
                    Object totalCount = resultDataSet.Tables[SearchDataTable.SearchResult].Rows[0]["TotalCount"];
                    recordCount = totalCount == null ? 0 : Convert.ToInt32(totalCount);
                }
                String commandText = "UPDATE SearchResultInstance SET SRI_StatusID = @SRI_StatusID, SRI_SearchCompleteTime = @SRI_SearchCompleteTime, SRI_TotalRecordCount = @SRI_TotalRecordCount, SRI_ModifiedByID = @SRI_ModifiedByID, SRI_ModifiedOn = @SRI_ModifiedOn WHERE SRI_ID = @SRI_ID;";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@SRI_StatusID", completedStatusId),
                           new SqlParameter("@SRI_SearchCompleteTime", DateTime.Now),
                           new SqlParameter("@SRI_TotalRecordCount", recordCount),
                           new SqlParameter("@SRI_ModifiedByID", Consts.SEARCH_PROCESS_ID),
                           new SqlParameter("@SRI_ModifiedOn", DateTime.Now),
                           new SqlParameter("@SRI_ID", masterSearchObject.SearchInstanceID)
                        };
                GetParameterValue(searchConnection, CommandType.Text, commandText, sqlParameterCollection);
            }
            catch (Exception ex)
            {
                logger.Error("** Error in UpdateSearchResultInstance: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public void NullInUseSearchResultInstanceId(SqlConnection searchConnection, MasterSearchObject masterSearchObject)
        {
            try
            {
                String commandText = "UPDATE lkpSearchResultTables SET SRT_InUseSearchResultInstanceId = NULL, SRT_SearchParameters = NULL WHERE SRT_SearchTypeId = @SRT_SearchTypeId AND SRT_InUseSearchResultInstanceId = @SRT_InUseSearchResultInstanceId;";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@SRT_SearchTypeId", masterSearchObject.SearchTypeID),
                           new SqlParameter("@SRT_InUseSearchResultInstanceId", masterSearchObject.SearchInstanceID)
                        };
                GetParameterValue(searchConnection, CommandType.Text, commandText, sqlParameterCollection);
            }
            catch (Exception ex)
            {
                logger.Error("** Error in NullInUseSearchResultInstanceId: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public void AddEntryInSearchResultProgress(SqlConnection searchConnection, SearchObject searchObj, Int32 inProgressStatusID)
        {
            try
            {
                String commandText = "INSERT INTO SearchResultProgress(SRP_SearchResultInstanceId, SRP_TenantId, SRP_StatusID, SRP_CreatedOn) VALUES (@SRP_SearchResultInstanceId, @SRP_TenantId, @SRP_StatusID, @SRP_CreatedOn);";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@SRP_SearchResultInstanceId", searchObj.SearchResultInstanceId),
                           new SqlParameter("@SRP_TenantId", searchObj.TenantId),
                           new SqlParameter("@SRP_StatusID", inProgressStatusID),
                           new SqlParameter("@SRP_CreatedOn", DateTime.Now)
                        };
                GetParameterValue(searchConnection, CommandType.Text, commandText, sqlParameterCollection);
            }
            catch (Exception ex)
            {
                logger.Error("** Error in AddEntryInSearchResultProgress: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public String GetTenantSearchResult(SearchObject searchObj, String retType)
        {
            try
            {
                using (SqlConnection tenantConnection = new SqlConnection())
                {
                    tenantConnection.ConnectionString = searchObj.ConnectionDetails;
                    tenantConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@OrderBy", searchObj.SortBy),
                           new SqlParameter("@OrderDirection", searchObj.SortDirection),
                           new SqlParameter("@DataBaseName", searchObj.SearchDatabaseName),
                           new SqlParameter("@ResultTableName", searchObj.ResultTableName),
                           new SqlParameter("@PageIndex", searchObj.PageIndex),
                           new SqlParameter("@PageSize", searchObj.PageSize),
                           new SqlParameter("@ReturnResultType", ReturnType.InsertResultTable),
                           new SqlParameter("@SearchInstanceId", searchObj.SearchResultInstanceId),
                           new SqlParameter("@SearchParameter", searchObj.SearchParam)
                        };
                        SqlParameter sqlParameter = new SqlParameter("@TotalCount", SqlDbType.VarChar);
                        sqlParameter.Direction = ParameterDirection.ReturnValue;

                        sqlCommand.Connection = tenantConnection;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = searchObj.SearchUSP;
                        sqlCommand.Parameters.AddRange(sqlParameterCollection);
                        sqlCommand.Parameters.Add(sqlParameter);
                        sqlCommand.ExecuteScalar();
                        return Convert.ToString(sqlCommand.Parameters["@TotalCount"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("** Error in GetTenantSearchResult: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public void UpdateSearchResultProgress(SqlConnection searchConnection, SearchObject searchObj, String recordCount, Int32 completedStatusID)
        {
            try
            {
                String recordCountQuery = String.Empty;

                if (!recordCount.IsNullOrEmpty())
                {
                    recordCountQuery = "SRP_RecordCount = " + recordCount + ",";
                }
                String commandText = "UPDATE SearchResultProgress SET " + recordCountQuery + " SRP_CompletionTime = @SRP_CompletionTime, SRP_StatusID = @SRP_StatusID WHERE SRP_SearchResultInstanceId = @SRP_SearchResultInstanceId AND SRP_TenantId = @SRP_TenantId;";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@SRP_CompletionTime", DateTime.Now),
                           new SqlParameter("@SRP_StatusID", completedStatusID),
                           new SqlParameter("@SRP_SearchResultInstanceId", searchObj.SearchResultInstanceId),
                           new SqlParameter("@SRP_TenantId", searchObj.TenantId)
                        };
                GetParameterValue(searchConnection, CommandType.Text, commandText, sqlParameterCollection);
            }
            catch (Exception ex)
            {
                logger.Error("** Error in UpdateSearchResultProgress: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public void UpdateErrorInResultProgress(SqlConnection searchConnection, SearchObject searchObj, String error, Int32 errorStatusID)
        {
            try
            {
                error = error.Remove("'");
                error = error.Remove("\"");
                String commandText = "UPDATE SearchResultProgress SET SRP_StatusID = @SRP_StatusID, SRP_Error = @SRP_Error WHERE SRP_SearchResultInstanceId = @SRP_SearchResultInstanceId AND SRP_TenantId = @SRP_TenantId;";
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@SRP_StatusID", errorStatusID),
                           new SqlParameter("@SRP_Error", error),
                           new SqlParameter("@SRP_SearchResultInstanceId", searchObj.SearchResultInstanceId),
                           new SqlParameter("@SRP_TenantId", searchObj.TenantId)
                        };
                GetParameterValue(searchConnection, CommandType.Text, commandText, sqlParameterCollection);
            }
            catch (Exception ex)
            {
                logger.Error("** Error in UpdateErrorInResultProgress: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public void DeleteDataFromReplicaTables(SqlConnection searchConnection, MasterSearchObject masterSearchObject)
        {
            try
            {
                foreach (var replicaTable in masterSearchObject.AvailableSearchResultsTables)
                {
                    String commandText = "DELETE FROM " + replicaTable + ";";
                    GetParameterValue(searchConnection, CommandType.Text, commandText);
                }
            }
            catch (Exception ex)
            {
                logger.Error("** Error in DeleteDataFromReplicaTables: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        public DataSet GetSearchResultList(String searchConnectionString, Int32 userID, String searchTypeCode, Int16[] searchScopeIDs)
        {
            try
            {
                DataSet searchDataSet = new DataSet();
                using (SqlConnection searchConnection = new SqlConnection())
                {
                    searchConnection.ConnectionString = searchConnectionString;
                    searchConnection.Open();
                    Int32 searchTypeID = GetSearchTypeID(searchConnection, searchTypeCode);
                    String searchScopes = String.Join(",", searchScopeIDs);
                    String commandText = "SELECT SRI.SRI_ID, SRI.SRI_SearchInstanceName, SRI.SRI_StatusID, LSRS.SRS_Description AS Status, SRI.SRI_TotalRecordCount, SRI.SRI_SearchCompleteTime, SRI.SRI_CreatedOn, SRI.SRI_SearchParams FROM SearchResultInstance SRI "
                        + " INNER JOIN lkpSearchResultStatus LSRS ON SRI.SRI_StatusID = LSRS.SRS_ID"
                        + " WHERE SRI_SearchTypeId = @SRI_SearchTypeId AND SRI_UserId = @SRI_UserId AND SRI_SearchScopeId IN ( " + searchScopes + ") ORDER BY SRI_SearchCompleteTime DESC;";
                    SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@SRI_SearchTypeId", searchTypeID),
                           new SqlParameter("@SRI_UserId", userID)
                        };
                    searchDataSet = GetDataSet(searchConnection, CommandType.Text, commandText, searchDataSet, SearchDataTable.SearchResult, sqlParameterCollection);
                    return searchDataSet;
                }
            }
            catch (Exception ex)
            {
                logger.Error("** Error in DeleteDataFromReplicaTables: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                throw ex;
            }
        }

        private Int32 GetSearchTypeID(SqlConnection searchConnection, String searchTypeCode)
        {
            String commandText = "SELECT * FROM lkpSearchType WHERE ST_SearchCode = @ST_SearchCode";
            SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@ST_SearchCode", searchTypeCode)
                        };
            Object searchTypeID = GetParameterValue(searchConnection, CommandType.Text, commandText, sqlParameterCollection);
            return searchTypeID == null ? 0 : Convert.ToInt32(searchTypeID);
        }

        #endregion
    }
}
