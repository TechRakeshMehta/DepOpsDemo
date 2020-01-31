using System;
using System.Collections;
using System.Data;

namespace Search
{
    public class MasterSearchObject
    {
        public Int32 UserID
        {
            get;
            set;
        }

        public String UserName
        {
            get;
            set;
        }

        public Int32 SearchScopeID
        {
            get;
            set;
        }

        public Int32 SearchTypeID
        {
            get;
            set;
        }

        public String SearchTypeCode
        {
            get;
            set;
        }

        public String SearchUSP
        {
            get;
            set;
        }

        public String SearchParameters
        {
            get;
            set;
        }

        public DataTable TenantIdList
        {
            get;
            set;
        }

        public Int32 SearchInstanceID
        {
            get;
            set;
        }

        public String SearchInstanceName
        {
            get;
            set;
        }

        public ArrayList AvailableSearchResultsTables
        {
            get;
            set;
        }

        public ArrayList AvailableManualResetEvent
        {
            get;
            set;
        }

        public String SearchDatabaseName
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

        public String OrderBy
        {
            get;
            set;
        }

        public String SortDirection
        {
            get;
            set;
        }

        public String MasterTableName
        {
            get;
            set;
        }
    }
}
