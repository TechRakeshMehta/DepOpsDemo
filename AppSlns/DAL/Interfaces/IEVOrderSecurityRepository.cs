using Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Interfaces
{
    public interface IEVOrderSecurityRepository
    {
        /// <summary>
        /// Get External Service Code for BkgService
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        Dictionary<Int32, String> GetExternalServiceCodeForBkgService(List<Int32> backgroundServiceIDs);

        Boolean SaveExtSvcIntegrationRecord(List<ExtSvcIntegrationRecord> ExtSvcIntegrationRecordToSave);
     
        IQueryable<vw_BkgExtSvcAttributeMapping> GetBkgExtSvcAttributeMappings();
        
        IQueryable<ExternalBkgSvcAttribute> GetExtSvcAttributes();
    }
}
