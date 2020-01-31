using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManagePersonalDocumentView
    {
        List<ApplicantDocumentDetails> ApplicantUploadedDocuments { get; set; }

        ApplicantDocument ToUpdateUploadedDocument { get; set; }

        Int32 CurrentUserID { get; }

        Int32 TenantID { get; set; }

        Int32 ApplicantUploadedDocumentID { get; set; }

        Int32 FromAdminApplicantID { get; set; }

        String ErrorMessage
        {
            get;
            set;
        }

        Int32 OrgUsrID { get; }
    }
}




