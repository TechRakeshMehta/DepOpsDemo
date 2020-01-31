using System;
using System.Linq;
using System.Collections.Generic;
using INTSOF.Utils;
//using INTSOF.SharedObjects;

using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils.CommonPocoClasses;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManagePersonalDocumentPresenter : Presenter<IManagePersonalDocumentView>
    {


        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetApplicantPersonalDocumentDetails()
        {
            View.ApplicantUploadedDocuments = ComplianceDataManager.GetApplicantPersonalDocumentDetails(View.FromAdminApplicantID, View.TenantID);
        }

        public Boolean DeleteApplicantUploadedDocument()
        {
            ComplianceDataManager.DeleteApplicantUploadedDocument(View.ApplicantUploadedDocumentID, View.TenantID, View.OrgUsrID, View.FromAdminApplicantID);
            return true;
        }

        public Boolean UpdateApplicantUploadedDocument()
        {
            //Updation when operation performed from Admin Side Screen.
            ComplianceDataManager.UpdateApplicantUploadedDocument(View.ToUpdateUploadedDocument, View.TenantID);
            return true;
        }
    }
}




