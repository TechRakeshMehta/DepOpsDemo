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
    public class PdfDocumentViewerPresenter : Presenter<IPdfDocumentViewerView>
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
            if (View.UnifiedPdfDocument.IsNotNull())
            {
                return DocumentManager.GetBytesFromUnifiedPdf(View.UnifiedPdfDocument.UPD_PdfDocPath);
            }

            if (!View.SelectedTenantId.IsNullOrEmpty() && !View.OrganizationUserId.IsNullOrEmpty() && View.OrganizationUserId > AppConsts.NONE &&
                View.SelectedTenantId > AppConsts.NONE)
            {
                var applicantUnifiedDoc = DocumentManager.GetPdfAsUnifiedDocument(View.SelectedTenantId, View.OrganizationUserId);
                if (applicantUnifiedDoc != null)
                {
                    return DocumentManager.GetBytesFromUnifiedPdf(applicantUnifiedDoc.UPD_PdfDocPath);
                }
            }
            return null;
        }

        public void GetUnifiedDocument()
        {
            if (!View.SelectedTenantId.IsNullOrEmpty() && !View.OrganizationUserId.IsNullOrEmpty() && View.OrganizationUserId > AppConsts.NONE &&
                View.SelectedTenantId > AppConsts.NONE)
            {
                View.UnifiedPdfDocument = DocumentManager.GetPdfAsUnifiedDocument(View.SelectedTenantId, View.OrganizationUserId);
            }
        }

        //public bool UpdateUnifiedDocumentIsFormFlattening(Int32 CurrentLoggedUserId)
        //{
        //    bool IsSuccessFlattening = false;
        //    if (!View.SelectedTenantId.IsNullOrEmpty() &&
        //       CurrentLoggedUserId > AppConsts.NONE &&
        //        View.SelectedTenantId > AppConsts.NONE)
        //    {
        //        IsSuccessFlattening = DocumentManager.UpdateUnifiedDocumentIsFormFlattening(View.SelectedTenantId, CurrentLoggedUserId);
        //    }
        //    return IsSuccessFlattening;
        //}



        ////UAT:4132  
        //public bool UpdateApplicantIsFormFlattening(Int32 CurrentLoggedUserId)
        //{
        //    bool IsSuccessFlattening = false;
        //    if (!View.SelectedTenantId.IsNullOrEmpty() &&
        //     CurrentLoggedUserId > AppConsts.NONE &&
        //        View.ApplicantDocumentId > AppConsts.NONE &&
        //        View.SelectedTenantId > AppConsts.NONE)
        //    {
        //        IsSuccessFlattening = DocumentManager.UpdateApplicantIsFormFlattening(View.SelectedTenantId, CurrentLoggedUserId, View.ApplicantDocumentId);
        //    }
        //    return IsSuccessFlattening;
        //}


        public void GetApplicantDocuments()
        {
            if (!View.OrganizationUserId.IsNullOrEmpty() && View.OrganizationUserId > AppConsts.NONE)
            {
                View.lstApplicantDocument = ComplianceDataManager.GetApplicantDocumentsData(View.OrganizationUserId, View.SelectedTenantId);
            }
        }

        #region UAT-1538
        public void GetApplicantDocument()
        {
            if (!View.SelectedTenantId.IsNullOrEmpty() && View.ApplicantDocumentId > AppConsts.NONE && View.SelectedTenantId > AppConsts.NONE)
            {
                View.ApplicantDocument = ComplianceDataManager.GetApplicantDocument(View.ApplicantDocumentId, View.SelectedTenantId);
            }
        }

        public byte[] GetPDFByteDataForSingleDocument()
        {
            if (View.ApplicantDocument.IsNotNull())
            {
                return DocumentManager.GetBytesFromUnifiedPdf(View.ApplicantDocument.PdfDocPath);
            }
            return null;
        }
        #endregion

        //public byte[] GetPDFByteDataForViewDocSingleDocument()
        //{
        //    return DocumentManager.GetBytesFromUnifiedPdf(View.DocumentPath);
        //}
    }
}

