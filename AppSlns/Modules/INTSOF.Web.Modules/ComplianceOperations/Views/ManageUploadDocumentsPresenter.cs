using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManageUploadDocumentsPresenter : Presenter<IManageUploadDocumentsView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public ManageUploadDocumentsPresenter([CreateNew] IComplianceOperationsController controller)
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

        public void GetApplicantUploadedDocuments()
        {
            View.ApplicantUploadedDocuments = ComplianceDataManager.GetApplicantDocuments(View.CurrentUserID, View.TenantID);
        }

        public void DeleteApplicantUploadedDocument()
        {
            ApplicantDocument uploadedDocument = ComplianceDataManager.GetApplicantUploadedDocument(View.ApplicantUploadedDocumentID, View.TenantID);
            var documentMapped = uploadedDocument.ApplicantComplianceDocumentMaps.Where(x => !x.IsDeleted);
            if (documentMapped == null || documentMapped.Count() == 0)
            {
                ComplianceDataManager.DeleteApplicantUploadedDocument(View.ApplicantUploadedDocumentID, View.TenantID, View.CurrentUserID);
                // delete succesfully
            }
            else
            { 
                // its mapped with item
            }
        }

        public void UpdateApplicantUploadedDocument()
        {
            ComplianceDataManager.UpdateApplicantUploadedDocument(View.ToUpdateUploadedDocument, View.TenantID);            
        }




        // TODO: Handle other view events and set state in the view
    }
}




