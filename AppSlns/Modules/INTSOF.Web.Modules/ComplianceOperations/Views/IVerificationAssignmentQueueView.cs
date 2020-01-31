using System;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IVerificationAssignmentQueueView
    {
        Int32 TenantId { get; set; }
        String TenantTypeCode { get; set; }
    }
}
