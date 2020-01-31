using INTSOF.Utils;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

namespace Search
{
    public class MasterSearch
    {
        #region Private Variables

        private String _searchDatabaseConnectionString = null;
        private String _securityDatabaseConnectionString = null;
        private Int32 _totalThreadPerSearchTypeAllowed = 0;
        private Int32 _completedStatusID = 0;
        private Int32 _errorStatusID = 0;
        private Int32 _inProgressStatusID = 0;
        private SqlConnection _searchConnection = null;
        private DataSet _resultDataSet = new DataSet();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public Methods

        /// <summary>
        /// Search for single or multiple tenants in synchronous or asynchronous mode.
        /// </summary>
        /// <param name="userID">ID of user who is performing search.</param>
        /// <param name="searchTypeCode">Type of search code. For example: Portfolio search or Compliance record search etc.</param>
        /// <param name="searchParameters">Parameters for which search is to be performed.</param>
        /// <param name="tenantIdList">Tenants ID list (define tenants for which search is to be done)</param>
        /// <param name="searchScopeID">SearchScope can be SingleTenantSynch, SingleTenantAsynch, AllTenantsSynch, AllTenantsAsynch, TenantsListSynch, TenantsListAsynch.</param>
        /// <param name="searchInstanceID">SearchInstanceID</param>
        /// <param name="pageIndex">Page Index for appling paging.</param>
        /// <param name="pageSize">Page Size for appling paging.</param>
        /// <param name="orderBy">Order By to sort search result.</param>
        /// <param name="sortDirection">Sort Direction to sort search result in ascending or desending order.</param>
        /// <returns>Data Set including Data Table of SearchResult, SearchResultInstance or Error.</returns>
        public DataSet Search(Int32 userID, String searchTypeCode, String searchParameters, ArrayList tenantIdList, Int32 searchScopeID, Int32 searchInstanceID, Int32 pageIndex, Int32 pageSize, String orderBy, String sortDirection)
        {
            logger.Info("******************* Calling Search: " + DateTime.Now.ToString() + " *******************");
            logger.Info("** Search Parameters: UserID = " + userID + ", SearchTypeCode = " + searchTypeCode
                + ", TenantIdList = " + (tenantIdList == null ? null : String.Join(",", tenantIdList.ToArray())) +
                ", SearchScope = " + searchScopeID + ", PageIndex = " + pageIndex + ", PageSize = " + pageSize + ", SearchInstanceID = " + searchInstanceID +
                ", OrderBy = " + orderBy + " and SortDirection = " + sortDirection + " **");
            logger.Info("** Search XML Parameters: " + searchParameters + " **");
            DataSet searchDataSet = new DataSet();
            MasterSearchObject masterSearchObject = null;

            try
            {
                //Get DB Connection String of Search and security Databases.
                _searchDatabaseConnectionString = ConfigurationManager.ConnectionStrings[Consts.SEARCH_CONNECTION_STRING_KEY].ConnectionString;
                _securityDatabaseConnectionString = ConfigurationManager.ConnectionStrings[Consts.SECURITY_CONNECTION_STRING_KEY].ConnectionString;

                //Open DB Connection for Search Database.
                _searchConnection = GetDALObject().CreateConnection(_searchDatabaseConnectionString);
                logger.Info("** Search Connection opened for Search having UserID = " + userID + ", SearchTypeCode = " + searchTypeCode + " and SearchScopeID = " + searchScopeID + " . **");

                //SearchDataSet = Get SearchUSP for SearchTypeCode from lkpSearchType                
                searchDataSet = GetDALObject().GetSearchType(_searchConnection, searchTypeCode, searchDataSet);

                //Work if lkpSearchType (containing Search Type Code, Search USP and Master Result Table as columns) is found.
                if (searchDataSet.IsNotNull())
                {
                    String searchUSP = Convert.ToString(searchDataSet.Tables["lkpSearchType"].Rows[0]["ST_SearchUSP"]);
                    String masterResultTable = Convert.ToString(searchDataSet.Tables["lkpSearchType"].Rows[0]["ST_MasterResultTable"]);
                    Int32 searchTypeID = Convert.ToInt32(searchDataSet.Tables["lkpSearchType"].Rows[0]["ST_ID"]);

                    //Creates a MasterSearchObject to include search related data like SearchTypeCode, SearchTypeDetails, SearchParameters /* XML */, TenantIdList.
                    masterSearchObject = GetMasterSearchObject(searchParameters, searchDataSet, searchInstanceID, userID, pageIndex, pageSize, orderBy, sortDirection, searchScopeID);

                    //Directly connect to tenant database for single tenant and synchronized search.
                    if (tenantIdList.IsNotNull() && tenantIdList.Count == 1 && searchScopeID == SearchScope.SingleTenantSynch)
                    {
                        Int32 tenantID = Convert.ToInt32(tenantIdList[0]);
                        //Get tenant db connection detail.
                        String tenantConnectionString = GetDALObject().GetTenantConnectionString(_securityDatabaseConnectionString, tenantID);
                        //Search in single database.
                        return GetDALObject().ExecuteSearchUSP(_resultDataSet, masterSearchObject, tenantConnectionString);
                    }

                    //Return search result as per SearchInstanceID.
                    if (searchInstanceID > 0)
                    {
                        //Get status code of Search Result Instance.
                        String searchInstanceStatusCode = GetDALObject().GetStatusCodeForSearchInstanceID(_searchConnection, searchInstanceID);

                        //Check if Search is complete for SearchInstance else return error.
                        if (searchInstanceStatusCode == null || searchInstanceStatusCode != LkpSearchResultStatus.Completed)
                        {
                            logger.Info("** Search is not completed for SearchInstanceID = " + searchInstanceID + " **");
                            List<String> errorMessages = new List<String>() { Consts.SEARCH_NOT_COMPLETED };
                            return GetDALObject().CreateError(_resultDataSet, errorMessages);
                        }
                        else
                        {
                            logger.Info("** Search is completed for SearchInstanceID = " + searchInstanceID + " **");
                            return GetDALObject().GetMasterResultBySearchInstanceID(_searchConnection, masterSearchObject, ReturnType.Return, _resultDataSet);
                        }
                    }
                    Int32 totalSearchAllowed = Convert.ToInt32(ConfigurationManager.AppSettings[Consts.TOTAL_SEARCH_ALLOWED]);
                    _totalThreadPerSearchTypeAllowed = Convert.ToInt32(ConfigurationManager.AppSettings[Consts.TOTAL_THREAD_PER_SEARCH_ALLOWED]);

                    //Check how many “Searches” are in progress from SearchResultInstance. If count is > totalSearchAllowed then return.
                    //Check how many “Searches for the given SearchType” are in progress from SearchResultInstance. If count is > _totalSearchPerSearchTypeAllowed then return.
                    if ((GetDALObject().CheckInProgressSearchCount(_searchConnection, totalSearchAllowed) == false) || (GetDALObject().CheckInProgressSearchForSearhType(_searchConnection, _totalThreadPerSearchTypeAllowed, searchTypeID) == false))
                    {
                        logger.Info("** Threads are busy in performing other searches. **");
                        List<String> errorMessages = new List<String>() { Consts.SERVER_BUSY };
                        return GetDALObject().CreateError(_resultDataSet, errorMessages);
                    }
                    DataTable tenantDataTable = new DataTable();

                    //Get tenants information as per Search Scope.
                    if (searchScopeID == SearchScope.AllTenantsSynch || searchScopeID == SearchScope.AllTenantsAsynch)
                    {
                        if (tenantIdList.IsNotNull())
                        {
                            tenantIdList.Clear();
                        }
                        //Get all tenants information from Security database.
                        tenantDataTable = GetDALObject().GetAllTenantList(_securityDatabaseConnectionString, searchDataSet);
                    }
                    else
                    {
                        //Get multiple tenants information from Security database.
                        tenantDataTable = GetDALObject().GetAllTenantList(_securityDatabaseConnectionString, searchDataSet, tenantIdList);
                    }
                    //Get search result status ids.
                    GetSearchResultSatusIDs(searchDataSet);
                    //Inserts a Instance for the search.
                    masterSearchObject.UserName = GetDALObject().GetUserName(_securityDatabaseConnectionString, userID);

                    searchInstanceID = GetDALObject().InsertSearchResultInstance(_searchConnection, searchDataSet, _inProgressStatusID, masterSearchObject);
                    masterSearchObject.SearchInstanceID = searchInstanceID;
                    
                    if (searchInstanceID != 0)
                    {
                        GetDALObject().GetSearchResultInstance(_searchConnection, searchInstanceID, _resultDataSet);
                        //Gets number of parallel thread to run.
                        Int32 parallelThreadCount = tenantDataTable.Rows.Count > _totalThreadPerSearchTypeAllowed ? _totalThreadPerSearchTypeAllowed : tenantDataTable.Rows.Count;

                        //Update lkpSearchResultTables.InUseSearchResultInstanceId to SearchResultInstanceId and search parameters
                        //where InUseSearchResultInstanceId Is Null and searchTypeId = given searchTypeId
                        GetDALObject().UpdatelkpSearchResultTables(_searchConnection, searchInstanceID, searchTypeID, parallelThreadCount, searchParameters);

                        //AvailableSearchResultsTables  = Get search table list from lkpSearchResultTables 
                        //where InUseSearchResultInstanceId  = SearchResultInstanceId   and searchTypeId = given searchTypeId
                        searchDataSet = GetDALObject().GetAvailableSearchResultTables(_searchConnection, searchInstanceID, searchTypeID, searchDataSet);

                        //At this point the below logic can be put into a background thread. Also, in this case user has to be informed about the searchInstanceId.
                        //UI can show the searchInstanceId and its progress. 
                        //Search UI will need to be changed to give option to user to get the result from previous search results. 
                        //He can then, get results based on status of searchInstanceId later on. In that case we can add the row from SearchResultInstance and return 
                        if (searchDataSet.IsNotNull())
                        {
                            //Update TenantIdList, SearchResultInstanceID, AvailableSearchResultsTables etc in master search object.
                            UpdateMasterSearchObject(masterSearchObject, searchInstanceID, tenantDataTable, searchDataSet);

                            //Delete records from the all AvailableSearchResultsTables in search database. 
                            GetDALObject().DeleteDataFromReplicaTables(_searchConnection, masterSearchObject);

                            //Run threads for multiple or all tenants.
                            if (searchScopeID == SearchScope.AllTenantsSynch || searchScopeID == SearchScope.TenantsListSynch)
                            {
                                MasterSearchProc(masterSearchObject);
                                //Return Dataset from SearchResultInstance;
                                //return _resultDataSet;
                            }
                            else if (searchScopeID == SearchScope.AllTenantsAsynch || searchScopeID == SearchScope.TenantsListAsynch || searchScopeID == SearchScope.SingleTenantAsynch)
                            {
                                Thread masterSearchThread = new Thread(MasterSearchProc);
                                masterSearchThread.Name = "Master Search Thread - " + masterSearchObject.SearchInstanceName;
                                masterSearchThread.Start(masterSearchObject);
                            }
                        }
                    }
                }
                else
                {
                    logger.Info("** lkpSearchType table has no entry for SearchTypeCode = " + searchTypeCode + " **");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in Search: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);

                if (_errorStatusID == 0)
                {
                    //Get search result status ids.
                    GetSearchResultSatusIDs(searchDataSet);
                }
                //Update Error status in Search tables.
                AddErrorInSearch(searchInstanceID, masterSearchObject, ex.Message);
            }
            finally
            {
                //Close DB Connection on Search Database.
                if (_searchConnection.IsNotNull() && _searchConnection.State == System.Data.ConnectionState.Open)
                {
                    _searchConnection.Close();
                    logger.Info("** Search Connection closed for Search. **" + Environment.NewLine + Environment.NewLine);
                }
            }
            return _resultDataSet;
        }

        /// <summary>
        /// Get the Search result list for a particular user, search type and scope id
        /// </summary>
        /// <param name="userID">ID of user for which search result is to be fetched.</param>
        /// <param name="searchTypeID">Search Type ID (ex: Portfolio or Compliance search etc) for which search result is to be fetched.</param>
        /// <param name="searchScopeID">Search scope ID (ex: AllTeannatAsynchronous or MultiTenantAsynchronous etc) for which search result is to be fetched.</param>
        /// <returns>Data Set containing Search Result List</returns>
        public DataSet GetSearchResultList(Int32 userID, String searchTypeCode, Int16[] searchScopeIDs)
        {
            try
            {
                _searchDatabaseConnectionString = ConfigurationManager.ConnectionStrings[Consts.SEARCH_CONNECTION_STRING_KEY].ConnectionString;
                return GetDALObject().GetSearchResultList(_searchDatabaseConnectionString, userID, searchTypeCode, searchScopeIDs);
            }
            catch (Exception ex)
            {
                //log error
                logger.Error("** Error :{0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
            }
            return null;
        }

        #endregion

        #region Private Methods

        private DAL GetDALObject()
        {
            return new DAL();
        }

        private void GetSearchResultSatusIDs(DataSet searchDataSet)
        {
            searchDataSet = GetDALObject().GetlkpSearchResultStatus(_searchConnection, searchDataSet);

            if (searchDataSet.Tables.Count > 0 && searchDataSet.Tables["lkpSearchResultStatus"].IsNotNull() && searchDataSet.Tables["lkpSearchResultStatus"].Rows.Count > 0)
            {
                _completedStatusID = Convert.ToInt32(searchDataSet.Tables["lkpSearchResultStatus"].AsEnumerable().FirstOrDefault(x => x["SRS_Code"].ToString() == LkpSearchResultStatus.Completed)["SRS_ID"]);
                _inProgressStatusID = Convert.ToInt32(searchDataSet.Tables["lkpSearchResultStatus"].AsEnumerable().FirstOrDefault(x => x["SRS_Code"].ToString() == LkpSearchResultStatus.InProgress)["SRS_ID"]);
                _errorStatusID = Convert.ToInt32(searchDataSet.Tables["lkpSearchResultStatus"].AsEnumerable().FirstOrDefault(x => x["SRS_Code"].ToString() == LkpSearchResultStatus.Error)["SRS_ID"]);
            }
        }

        private MasterSearchObject GetMasterSearchObject(String searchParameters, DataSet searchDataSet, Int32 searchInstanceID, Int32 userID, Int32 pageIndex, Int32 pageSize, String orderBy, String sortDirection, Int32 searchScopeID)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_searchDatabaseConnectionString);
            String searchDatabaseName = builder.InitialCatalog;

            return new MasterSearchObject()
            {
                UserID = userID,
                SearchScopeID = searchScopeID,
                SearchTypeID = Convert.ToInt32(searchDataSet.Tables["lkpSearchType"].Rows[0]["ST_ID"]),
                SearchTypeCode = Convert.ToString(searchDataSet.Tables["lkpSearchType"].Rows[0]["ST_SearchCode"]),
                SearchUSP = Convert.ToString(searchDataSet.Tables["lkpSearchType"].Rows[0]["ST_SearchUSP"]),
                MasterTableName = Convert.ToString(searchDataSet.Tables["lkpSearchType"].Rows[0]["ST_MasterResultTable"]),
                SearchParameters = searchParameters,
                SearchInstanceID = searchInstanceID,
                SearchDatabaseName = searchDatabaseName,
                PageIndex = pageIndex,
                PageSize = pageSize,
                OrderBy = orderBy,
                SortDirection = sortDirection
            };
        }

        private MasterSearchObject UpdateMasterSearchObject(MasterSearchObject masterSearchObject, Int32 searchInstanceID, DataTable tenantDataTable, DataSet searchDataSet)
        {
            //masterSearchObject.UserName = _userName,
            masterSearchObject.SearchInstanceID = searchInstanceID;
            masterSearchObject.TenantIdList = tenantDataTable;
            masterSearchObject.SearchInstanceName = Convert.ToString(_resultDataSet.Tables[SearchDataTable.SearchResultInstance].Rows[0]["SRI_SearchInstanceName"]);
            masterSearchObject.AvailableSearchResultsTables = new ArrayList(searchDataSet.Tables["lkpSearchResultTables"].AsEnumerable().Select(a => a["SRT_ResultTable"].ToString()).ToArray());
            return masterSearchObject;
        }

        private void MasterSearchProc(Object threadObj)
        {
            MasterSearchObject masterSearchObject = null;
            SqlConnection searchConnection = new SqlConnection();
            Int32 currentThreadID = Thread.CurrentThread.ManagedThreadId;
            try
            {
                masterSearchObject = (MasterSearchObject)threadObj;
                logger.Info("** Master thread #" + currentThreadID + " :Started for SearchInstanceID = " + masterSearchObject.SearchInstanceID +" and SearchScopeID = " + masterSearchObject.SearchScopeID + ". **");

                //Open DB Connection on Search Database
                searchConnection.ConnectionString = _searchDatabaseConnectionString;
                searchConnection.Open();
                logger.Info("** Master thread #" + currentThreadID + " :Search Connection opened for SearchInstanceID = " + masterSearchObject.SearchInstanceID + " and SearchScopeID = " + masterSearchObject.SearchScopeID + ". **");

                Int32 tenantCount = masterSearchObject.TenantIdList.Rows.Count;
                Int32 parallelThreadCount = tenantCount > _totalThreadPerSearchTypeAllowed ? _totalThreadPerSearchTypeAllowed : tenantCount;

                Semaphore searchSemaphore = new Semaphore(parallelThreadCount, parallelThreadCount);

                ManualResetEvent[] manualEvents = new ManualResetEvent[parallelThreadCount];

                for (Int32 index = 0; index < parallelThreadCount; index++)
                {
                    manualEvents[index] = new ManualResetEvent(true);
                }

                Object tableListLock = new Object();
                ArrayList inUseSearchResultsTables = new ArrayList();
                Int32 manualResetEventCounter = 0;
                Int32 inUseManualResetEvent = 0;

                for (Int32 i = 0; i < tenantCount; i++)
                {
                    Int32 tenantID = Convert.ToInt32(masterSearchObject.TenantIdList.Rows[i]["CDB_TenantID"]);

                    searchSemaphore.WaitOne();

                    SearchObject searchObj = new SearchObject();
                    searchObj.TenantId = tenantID;
                    searchObj.Semaphore = searchSemaphore;
                    searchObj.SearchParam = masterSearchObject.SearchParameters;
                    searchObj.SearchUSP = masterSearchObject.SearchUSP;
                    searchObj.PageIndex = masterSearchObject.PageIndex;
                    searchObj.PageSize = masterSearchObject.PageSize;
                    searchObj.SortBy = masterSearchObject.OrderBy;
                    searchObj.SortDirection = masterSearchObject.SortDirection;
                    searchObj.ConnectionDetails = Convert.ToString(masterSearchObject.TenantIdList.AsEnumerable().
                        FirstOrDefault(x => x["CDB_TenantID"].ToString() == tenantID.ToString())["CDB_ConnectionString"]);
                    searchObj.SearchResultInstanceId = masterSearchObject.SearchInstanceID;
                    searchObj.SearchDatabaseName = masterSearchObject.SearchDatabaseName;
                    searchObj.TableListLock = tableListLock;
                    searchObj.SearchConnection = searchConnection;

                    lock (searchObj.TableListLock)
                    {
                        Int32 tableIndex = masterSearchObject.AvailableSearchResultsTables.Count - 1;
                        String resultTableName = Convert.ToString(masterSearchObject.AvailableSearchResultsTables[tableIndex]);
                        masterSearchObject.AvailableSearchResultsTables.RemoveAt(tableIndex);
                        inUseSearchResultsTables.Add(resultTableName);

                        searchObj.ResultTableName = resultTableName;
                        searchObj.AvailableSearchResultsTables = masterSearchObject.AvailableSearchResultsTables;
                        searchObj.InUseSearchResultsTables = inUseSearchResultsTables;
                    }
                    searchObj.FinishManualResetEvent = manualEvents[manualResetEventCounter];
                    searchObj.FinishManualResetEvent.Reset();
                    manualEvents[manualResetEventCounter] = searchObj.FinishManualResetEvent;
                    inUseManualResetEvent++;

                    if (i < parallelThreadCount)
                    {
                        manualResetEventCounter++;
                    }
                    ThreadPool.QueueUserWorkItem(SearchThreadProc, searchObj);

                    if (inUseManualResetEvent == parallelThreadCount)
                    {
                        manualResetEventCounter = WaitHandle.WaitAny(manualEvents);
                        inUseManualResetEvent = inUseManualResetEvent - 1;
                    }
                }
                /* ManualResetEvent: http://msdn.microsoft.com/en-us/library/z6w25xa6.aspx */

                // Since ThreadPool threads are background threads, wait for the work items to signal before exiting.
                WaitHandle.WaitAll(manualEvents);

                //Check and add (if any) AvailableSearchResultsTables Error 
                if (GetDALObject().CheckIfErrorInAvailableResultTables(searchConnection, _errorStatusID, masterSearchObject.SearchInstanceID, _resultDataSet))
                {
                    //Update SearchResultInstance Set Status = “Error” Where SearchResultInstanceId
                    GetDALObject().UpdateSearchResultInstanceStatus(searchConnection, masterSearchObject, _errorStatusID, _completedStatusID);
                }
                else
                {
                    //Shift data from AvailableSearchResultsTables into lkpSearchType.MasterResultTable
                    GetDALObject().GetMasterResultBySearchInstanceID(searchConnection, masterSearchObject, ReturnType.InsertResultTable, _resultDataSet);

                    //Update SearchResultInstance Set Status = “Complete” Where SearchResultInstanceId
                    GetDALObject().UpdateSearchResultInstance(searchConnection, masterSearchObject, _completedStatusID, _resultDataSet);
                }
                logger.Info("** Master thread #" + currentThreadID + " :Finished for SearchInstanceID = " + masterSearchObject.SearchInstanceID + " and SearchScopeID = " + masterSearchObject.SearchScopeID + ". **");
            }
            catch (Exception ex)
            {
                logger.Error("** Master thread #" + currentThreadID + " :Error :{0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);

                if (searchConnection.IsNotNull() && searchConnection.State == System.Data.ConnectionState.Open && masterSearchObject.IsNotNull() && masterSearchObject.SearchInstanceID.IsNotNull())
                {
                    //Update the SearchResultInstance  Staus = Error if error comes.
                    GetDALObject().UpdateSearchResultInstanceStatus(searchConnection, masterSearchObject, _errorStatusID, _completedStatusID);
                }
            }
            finally
            {
                if (masterSearchObject.IsNotNull())
                {
                    //Update lkpSearchResultTables.InUseSearchResultInstanceId = null
                    //where InUseSearchResultInstanceId  = SearchResultInstanceId   and searchTypeId = given searchTypeId
                    GetDALObject().NullInUseSearchResultInstanceId(searchConnection, masterSearchObject);
                }

                //Close DB Connection on Search Database
                searchConnection.Close();
                logger.Info("** Master thread #" + currentThreadID + " :Search Connection closed. **" + Environment.NewLine);
            }
        }

        private void SearchThreadProc(Object searchInfo)
        {
            SearchObject searchObj = null;
            Int32 currentThreadID = Thread.CurrentThread.ManagedThreadId;
            try
            { 
                searchObj = (SearchObject)searchInfo;
                logger.Info("** Search thread #" + currentThreadID + " :Started for SearchInstanceID = " + searchObj.SearchResultInstanceId + " and TenantID = " + searchObj.TenantId + ". **");

                String recordCount;
                String retType = ReturnType.InsertResultTable;

                //Insert Into SearchResultProgress(searchObj.SearchResultInstanceId, TenantId, CreatedDate)
                GetDALObject().AddEntryInSearchResultProgress(searchObj.SearchConnection, searchObj, _inProgressStatusID);

                //Exec searchObj.SearchUSP searchObj.SearchParam, RetType, searchObj.ResTableName, searchObj.SearchResultInstanceId, SearchDBName, PageIndex,  PageSize
                //Get tenant connection.
                recordCount = GetDALObject().GetTenantSearchResult(searchObj, retType);

                //Insert Into SearchResultProgress(searchObj.SearchResultInstanceId, TenantId, “Completed”, CreatedDate)
                GetDALObject().UpdateSearchResultProgress(searchObj.SearchConnection, searchObj, recordCount, _completedStatusID);
                logger.Info("** Search thread #" + currentThreadID + " :Finished for SearchInstanceID = " + searchObj.SearchResultInstanceId + " and TenantID = " + searchObj.TenantId + ". **");
            }
            catch (Exception ex)
            {
                //log error
                logger.Error("** Search thread #" + currentThreadID + " :Error :{0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);

                //Insert Into SearchResultProgress(searchObj.SearchResultInstanceId, TenantId, “Error”, CreatedDate, Error.Message)
                if (searchObj.IsNotNull() && searchObj.SearchConnection.IsNotNull() && searchObj.SearchConnection.State == System.Data.ConnectionState.Open)
                {
                    GetDALObject().UpdateErrorInResultProgress(searchObj.SearchConnection, searchObj, ex.Message, _errorStatusID);
                }
            }
            finally
            {
                if (searchObj.IsNotNull())
                {
                    lock (searchObj.TableListLock)
                    {
                        searchObj.InUseSearchResultsTables.Remove(searchObj.ResultTableName);
                        searchObj.AvailableSearchResultsTables.Add(searchObj.ResultTableName);
                    }
                    searchObj.Semaphore.Release();
                    searchObj.FinishManualResetEvent.Set();
                    logger.Info("** Search thread #" + currentThreadID + " :Released Semaphore and set ManualResetEvent for SearchInstanceID = " + searchObj.SearchResultInstanceId + " and TenantID = " + searchObj.TenantId + ". **");
                }
            }
        }

        private void AddErrorInSearch(Int32 searchInstanceID, MasterSearchObject masterSearchObject, String errorMessage)
        {
            if (_searchConnection.IsNotNull() && searchInstanceID.IsNotNull())
            {
                //Update the SearchResultInstance  Staus = Error if error comes.
                GetDALObject().UpdateSearchResultInstanceStatus(_searchConnection, masterSearchObject, _errorStatusID, _completedStatusID);

                if (masterSearchObject.IsNotNull())
                {
                    //Update lkpSearchResultTables.InUseSearchResultInstanceId = null
                    //where InUseSearchResultInstanceId  = SearchResultInstanceId   and searchTypeId = given searchTypeId
                    GetDALObject().NullInUseSearchResultInstanceId(_searchConnection, masterSearchObject);
                }

                List<String> errorMessages = new List<String> { errorMessage };
                GetDALObject().CreateError(_resultDataSet, errorMessages);
            }
        }

        #endregion
    }
}

