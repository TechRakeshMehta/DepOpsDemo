using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Business.RepoManagers;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.SharedObjects;

namespace CoreWeb.ComplianceOperations.Views
{
    public class AdminDataEntryDocViewerPresenter : Presenter<IAdminDataEntryDocViewer>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            //  View.ApplicantCategoryData = ApplicantManager.GetApplicantComplianceData(View.PackageSubscriptionId, View.ClientComplianceItemsContract.ClientComplianceCategoryId, View.TenantId);
        }

        public byte[] GetPDFByteData()
        {
            if (View.ApplicantDocument.IsNotNull())
            {
                return DocumentManager.GetBytesFromUnifiedPdf(View.ApplicantDocument.PdfDocPath);
            }            
            return null;
        }

        public void GetApplicantDocument()
        {
            if (!View.TenantId.IsNullOrEmpty() && !View.ApplicantDocumentId.IsNullOrEmpty() && View.ApplicantDocumentId > AppConsts.NONE &&
                View.TenantId > AppConsts.NONE)
            {
                View.ApplicantDocument = ComplianceDataManager.GetApplicantDocument(View.ApplicantDocumentId, View.TenantId);
            }
        }

        //public void GetApplicantDocuments()
        //{
        //    if (!View.OrganizationUserId.IsNullOrEmpty() && View.OrganizationUserId > AppConsts.NONE)
        //    {
        //        View.lstApplicantDocument = ComplianceDataManager.GetApplicantDocumentsData(View.OrganizationUserId, View.SelectedTenantId);
        //    }
        //}
    }
}

