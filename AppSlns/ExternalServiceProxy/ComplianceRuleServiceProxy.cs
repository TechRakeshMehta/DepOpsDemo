using ExternalServiceProxy.ComplianceRuleServiceReference;
using System;
using System.Collections.Generic;
using System.Data;

namespace ExternalServiceProxy
{
    public static class ComplianceRuleServiceProxy
    {
        static ComplianceRuleServiceClient _clientComplianceRuleServiceProxy;

        public static DataSet Search(Int32 userID, String searchTypeCode, String searchParameters, List<Int32> tenantIdList, Int32 searchScope, Int32 searchInstanceID, Int32 pageIndex, Int32 pageSize, String orderBy, String sortDirection)
        {
            List<ListDataContract> listDataContract = new List<ListDataContract>();
            ListDataContract[] tenantIDs = null;

            if (tenantIdList != null)
            {
                foreach (var id in tenantIdList)
                {
                    ListDataContract dataContract = new ListDataContract();
                    dataContract.ID = id;
                    listDataContract.Add(dataContract);
                }
                tenantIDs = listDataContract.ToArray();
            }
            return clientComplianceRuleServiceProxy.Search(userID, searchTypeCode, searchParameters, tenantIDs, searchScope, searchInstanceID, pageIndex, pageSize, orderBy, sortDirection);
        }

        public static ComplianceRuleServiceClient clientComplianceRuleServiceProxy
        {
            get
            {
                if (_clientComplianceRuleServiceProxy == null)
                    _clientComplianceRuleServiceProxy = new ComplianceRuleServiceClient();

                return _clientComplianceRuleServiceProxy;
            }
        }

        public static DataSet GetSearchResultList(Int32 userID, String searchTypeCode, Int16[] searchScopeIDs)
        {
            return clientComplianceRuleServiceProxy.GetSearchResultList(userID, searchTypeCode, searchScopeIDs);
        }
    }
}
