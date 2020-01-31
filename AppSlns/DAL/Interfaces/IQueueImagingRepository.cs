using System;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IQueueImagingRepository
    {
        List<Int32> GetTenantListDueForImaging();

        void SyncVerificationDataForTenant(Int32 tenantID);
        Boolean UpdateInsertQueueImagingDue(Int32 tenantId);
    }
}
