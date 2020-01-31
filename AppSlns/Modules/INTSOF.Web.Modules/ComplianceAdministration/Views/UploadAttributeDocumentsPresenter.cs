using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class UploadAttributeDocumentsPresenter : Presenter<IUploadAttributeDocumentsView>
    {
        public Boolean AddApplicantUploadedDocuments()
        {
            Int32 docTypeId = ComplianceSetupManager.GetDocumentTypeIDByCode(DocumentType.COMPLIANCE_VIEW_DOCUMENT.GetStringValue());//DocumentType.COMPLIANCE_VIEW_DOCUMENT.GetStringValue()

            View.ToSaveUploadedComplianceViewDocuments.ForEach(condition =>
            {
                condition.CSD_DocumentTypeID = docTypeId;
            });

            return ComplianceSetupManager.SaveComplianceViewDocument(View.TenantID, View.ToSaveUploadedComplianceViewDocuments); ;
        }
    }
}
