using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IManageSharedUserDocumentView
    {
        List<SharedSystemDocTypeContract> DocumentTypeList { get; set; }

        Int32 ClientContactID { get; set; }

        Int32 TenantID { get; set; }

        Int32 SelectedTenantID { get; set; }

        SharedSystemDocumentContract UploadedDocument { get; set; }

        String ClientContactEmailID { get; set; }

        List<SharedSystemDocumentContract> UploadedDocumentList { get; set; }

        Boolean SuccessMsg { get; set; }

        List<TenantDetailContract> LstTenant { get; set; }
    }
}
