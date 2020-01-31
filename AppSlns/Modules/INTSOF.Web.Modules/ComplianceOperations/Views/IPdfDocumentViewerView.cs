using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IPdfDocumentViewerView
    {
        String UnifiedFilePath
        {
            get;
            set;
        }

        Int32 OrganizationUserId
        {
            get;
            set;
        }
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        List<ApplicantDocuments> lstApplicantDocument
        {
            get;
            set;
        }

        UnifiedPdfDocument UnifiedPdfDocument
        {
            get;
            set;
        }

        Int32? SelectedCatUnifiedStartPageID
        {
            get;
            set;
        }

        Int32 ApplicantDocumentId
        {
            get;
            set;
        }

        ApplicantDocument ApplicantDocument
        {
            get;
            set;
        }

        //String DocumentPath
        //{ get; set; }

    }
}
