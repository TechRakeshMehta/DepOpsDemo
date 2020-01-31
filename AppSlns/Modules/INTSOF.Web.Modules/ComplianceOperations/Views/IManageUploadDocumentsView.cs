using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManageUploadDocumentsView
    {
        List<ApplicantDocument> ApplicantUploadedDocuments { get; set; }

        ApplicantDocument ToUpdateUploadedDocument { get; set; }

        Int32 CurrentUserID { get; }

        Int32 TenantID { get; set; }

        Int32 ApplicantUploadedDocumentID { get; set; }
    }
}




