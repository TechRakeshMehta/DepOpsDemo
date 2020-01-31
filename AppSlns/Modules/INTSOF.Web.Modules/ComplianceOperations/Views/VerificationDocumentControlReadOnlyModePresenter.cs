using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using System.Linq;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public class VerificationDocumentControlReadOnlyModePresenter : Presenter<IVerificationDocumentControlReadOnlyModeView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public VerificationDocumentControlReadOnlyModePresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }


        public void GetApplicantDocuments()
        {
            //if (View.OrganizationUserData != null)
            //        View.lstApplicantDocument = ComplianceDataManager.GetApplicantDocuments(View.OrganizationUserData.OrganizationUserID, View.SelectedTenantId_Global);
        }


        public List<ApplicantDocuments> GetItemRelatedDocument()
        {
            if ((View.ItemDataId != null) && (View.ItemDataId != 0))
            {
                return View.lstApplicantDocument.Where(x=>x.ItemID==View.ItemDataId && x.PackageId==View.PackageSubscriptionId).ToList();
                //if (View.IsException)
                //{
                //    return ComplianceDataManager.GetItemRelatedDocumentForException(View.ItemDataId, View.SelectedTenantId_Global,View.PackageSubscriptionId);
                //}
                //else
                //{
                //    return ComplianceDataManager.GetItemRelatedDocument(View.ItemDataId, View.SelectedTenantId_Global,View.PackageSubscriptionId);
                //}
            }

            else
            {
                return new List<ApplicantDocuments>();
            }
        }
        // TODO: Handle other view events and set state in the view

        public Int32 GetDocumentStatusID()
        {
            List<lkpDocumentStatu> lkpDocumentStatus = Business.RepoManagers.DocumentManager.GetDocumentStatus(View.SelectedTenantId_Global);
            var mergingCompletedCode = DocumentStatus.MERGING_COMPLETED.GetStringValue();
            //Get the document status id based on Merging Completed status code
            return lkpDocumentStatus.Where(obj => obj.DMS_Code == mergingCompletedCode).FirstOrDefault().DMS_ID;
        }

        /// <summary>
        /// Get the "Screening Document" Attribute DocumentTypeID from lkpDocumentType
        /// </summary>
        public void GetScreeningDocumentTypeId()
        {
            var _screeningDocTypeCode = DocumentType.SCREENING_DOCUMENT_ATTRIBUTE_TYPE_DOCUMENT.GetStringValue();
            var _lstLkpDocumentTypes = LookupManager.GetLookUpData<lkpDocumentType>(View.SelectedTenantId_Global);
            View.ScreeningDocTypeId = _lstLkpDocumentTypes.First(dt => dt.DMT_Code == _screeningDocTypeCode).DMT_ID;
        }

    }
}




