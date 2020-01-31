using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IManageInstructorPreceptorDocumentsView
    {
        Int32 SelectedTenantID { get; set; }
        List<TenantDetailContract> LstTenant { get; set; }
        String ClientContactEmailID { get; }
        List<ApplicantDocumentDetails> ApplicantUploadedDocuments { get; set; }
        Boolean IsRequirementFieldUploadDocument { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        Int32 ApplicantUploadedDocumentID { get; set; }
        ApplicantDocument ToUpdateUploadedDocument { get; set; }
        List<Int32> DocumentIdsToPrint { get; set; }
    }
}
