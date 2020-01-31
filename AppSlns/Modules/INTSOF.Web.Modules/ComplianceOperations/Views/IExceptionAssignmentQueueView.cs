using System;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IExceptionAssignmentQueueView
    {
        Int32 TenantId { get; set; }
        String TenantTypeCode { get; set; }
    }
}
