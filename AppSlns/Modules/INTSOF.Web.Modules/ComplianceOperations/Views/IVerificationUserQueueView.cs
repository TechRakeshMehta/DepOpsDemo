using System;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IVerificationUserQueueView
    {
        Int32 TenantId { get; set; }
        String TenantTypeCode { get; set; }
    }
}
