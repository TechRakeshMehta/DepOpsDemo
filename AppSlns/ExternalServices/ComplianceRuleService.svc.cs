using ExternalServices.DataContracts;
using Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace ExternalServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class ComplianceRuleServiceService : IComplianceRuleService
    {
        public DataSet Search(Int32 userID, String searchTypeCode, String searchParameters, List<ListDataContract> idList, Int32 searchScope, Int32 searchInstanceID, Int32 pageIndex, Int32 pageSize, String orderBy, String sortDirection)
        {
            MasterSearch masterSearch = new MasterSearch();
            ArrayList tenantIdList = idList == null ? null : new ArrayList();

            if (tenantIdList != null)
            {
                foreach (var id in idList)
                {
                    tenantIdList.Add(id.ID);
                }
            }
            return masterSearch.Search(userID, searchTypeCode, searchParameters, tenantIdList, searchScope, searchInstanceID, pageIndex, pageSize, orderBy, sortDirection);
        }

        public DataSet GetSearchResultList(Int32 userID, String searchTypeCode, Int16[] searchScopeIDs)
        {
            MasterSearch masterSearch = new MasterSearch();
            return masterSearch.GetSearchResultList(userID, searchTypeCode, searchScopeIDs);
        }
    }
}
