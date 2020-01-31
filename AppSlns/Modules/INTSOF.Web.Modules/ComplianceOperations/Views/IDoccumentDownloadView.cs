using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IDoccumentDownloadView
    {
        Int32 TenantId { get; set; }
        Int32 ApplicantDocumentId { get; set; }

        Int32 SystemDocumentID
        {
            get;
            set;
        }

        String SystemDocumentType
        {
            get;
            set;
        }

        Int32 ClientSystemDocumentID
        {
            get;
            set;
        }

        Int32 SharedSystemDocumentID
        {
            get;
            set;
        }
    }
}




