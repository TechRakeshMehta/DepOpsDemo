using ExternalServices.DataContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace ExternalServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IComplianceRuleService
    {
        [OperationContract]
        DataSet Search(Int32 userID, String searchTypeCode, String searchParameters, List<ListDataContract> idList, Int32 searchScope, Int32 searchInstanceID, Int32 pageIndex, Int32 pageSize, String orderBy, String sortDirection);
        
        [OperationContract]
        DataSet GetSearchResultList(Int32 userID, String searchTypeCode, Int16[] searchScopeIDs);
    }
}
