using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Linq;

namespace CoreWeb.ComplianceOperations.Views
{
    public class AttributeControlPresenter : Presenter<IAttributeControlView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public AttributeControlPresenter([CreateNew] IComplianceOperationsController controller)
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

        // TODO: Handle other view events and set state in the view

        public void GetDocuments()
        {
            List<ApplicantDocument> applicantDocuments = ComplianceDataManager.GetApplicantDocuments(View.CurrentLoggedInUserId, View.TenantId);
            if (applicantDocuments != null)
            {
                View.CurrentViewContext.ApplicantDocuments = ComplianceDataManager.GetApplicantDocumentsExceptEsigned(applicantDocuments, View.TenantId);
            }

        }

        public ApplicantDocument GetApplicantDocumentByApplAttrDataID(Int32 ApplAttributeDataId)
        {
            return ComplianceDataManager.GetApplicantDocumentByApplAttrDataID(View.TenantId, ApplAttributeDataId);

        }

        #region UAT-4067
        //public void GetAllowedFileExtensions(String selectedNodeIds)
        //{
        //    View.lstAllowedExtensions = new List<String>();
        //    var lstAllowedExtns = ComplianceDataManager.GetAllowedFileExtensionsByNodeIDs(View.TenantId, selectedNodeIds);
        //    if (!lstAllowedExtns.IsNullOrEmpty())
        //    {
        //        View.lstAllowedExtensions = lstAllowedExtns.Select(Sel => Sel.Name).ToList();
        //    }
        //}

        #endregion
    }
}




