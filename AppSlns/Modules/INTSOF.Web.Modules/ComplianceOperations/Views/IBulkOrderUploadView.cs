using System;
using System.Collections.Generic;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IBulkOrderUploadView
    {
        IBulkOrderUploadView CurrentViewContext { get; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 TenantID { get; }
        String ErrorMessage { get; set; }
        String SuccessMessage { get; set; }
        List<Entity.Tenant> LstTenant { get; set; }
        List<BulkOrderUploadContract> ApplicantDataList { get; set; }
        String ApplicantXmlData { get; set; }
    }
}
