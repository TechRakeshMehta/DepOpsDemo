using System;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IExceptionUserQueueView
    {
        Int32 TenantId { get; set; }
        String TenantTypeCode { get; set; }
    }
}
