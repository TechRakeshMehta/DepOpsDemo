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
    public class ManageAttributeDocumentsPresenter : Presenter<IManageAttributeDocumentsView>
    {
        public void GetUploadedComplianceViewDocuments()
        {
            if (View.TenantID > AppConsts.NONE)
            {
                Int32 docTypeId = ComplianceSetupManager.GetDocumentTypeIDByCode(DocumentType.COMPLIANCE_VIEW_DOCUMENT.GetStringValue());

                List<Entity.ClientEntity.ClientSystemDocument> lstClientSystemDocuments = ComplianceSetupManager.GetComplianceViewDocuments(View.TenantID, docTypeId);
                if (!lstClientSystemDocuments.IsNullOrEmpty())
                {
                    lstClientSystemDocuments.ForEach(col => col.CSD_Size = col.CSD_Size.HasValue ? col.CSD_Size / 1024 : AppConsts.NONE);
                }
                View.ComplianceViewDocuments = lstClientSystemDocuments;
            }
            else
            {
                View.ComplianceViewDocuments = new List<Entity.ClientEntity.ClientSystemDocument>();
            }
        }

        public Boolean DeleteUploadedComplianceViewDocument(Int32 currentUserId)
        {
            return ComplianceSetupManager.DeleteComplianceViewDocument(View.TenantID, View.SystemDocumentID, currentUserId);
        }

        public Boolean UpdateUploadedComplianceViewDocument()
        {
            return ComplianceSetupManager.UpdateComplianceViewDocument(View.TenantID, View.ComplianceViewDocumentToUpdate);
        }

        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.ClientTenantID);
        }

        public void GetTenants()
        {
            View.lstTenant = ComplianceDataManager.getClientTenant();
        }

        public Boolean IsDocumentMappedWithAttribute()
        {
            return ComplianceSetupManager.IsDocumentMappedWithAttribute(View.TenantID, View.SystemDocumentID);
        }
    }
}
