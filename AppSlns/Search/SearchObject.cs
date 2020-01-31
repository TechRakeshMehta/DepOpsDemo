using System;
using System.Collections;
using System.Data.SqlClient;
using System.Threading;

namespace Search
{
    public class SearchObject
    {
        public Int32 TenantId
        {
            get;
            set;
        }

        public Semaphore Semaphore
        {
            get;
            set;
        }

        public String SearchParam
        {
            get;
            set;
        }

        public String SearchUSP
        {
            get;
            set;
        }

        public String ConnectionDetails
        {
            get;
            set;
        }

        public Int32 SearchResultInstanceId
        {
            get;
            set;
        }

        public Object TableListLock
        {
            get;
            set;
        }

        public String ResultTableName
        {
            get;
            set;
        }

        public ArrayList AvailableSearchResultsTables
        {
            get;
            set;
        }

        public ArrayList InUseSearchResultsTables
        {
            get;
            set;
        }

        public ArrayList AvailableManualResetEvent
        {
            get;
            set;
        }

        public ArrayList InUseManualResetEvent
        {
            get;
            set;
        }

        public ManualResetEvent FinishManualResetEvent
        {
            get;
            set;
        }

        public Int32 PageIndex
        {
            get;
            set;
        }

        public Int32 PageSize
        {
            get;
            set;
        }

        public String SortBy
        {
            get;
            set;
        }

        public String SortDirection
        {
            get;
            set;
        }

        public String SearchDatabaseName
        {
            get;
            set;
        }

        public SqlConnection SearchConnection
        {
            get;
            set;
        }
    }
}
