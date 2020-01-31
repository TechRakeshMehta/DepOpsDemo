using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public class RequiredDocumentsPresenter : Presenter<IRequiredDocumentsView>
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


        public void GetRequirementDocumentsDetails()
        {
            //UAT-2306
            if (!View.IsAdminView)
            {
                View.lstApplicantRequiredDocumentsContract = ComplianceDataManager.GetRequirementDocumentsDetails(View.TenantId, View.CurrentLoggedInUserId);
            }
            else
            {
                View.lstApplicantRequiredDocumentsContract = ComplianceDataManager.GetRequirementDocumentsDetails(View.FromAdminTenantID, View.FromAdminApplicantID);
            }
        }

        //UAT-3161
        public void GetRotReqDocumentsDetails()
        {
            if (!View.IsAdminView)
            {
                View.lstApplicantRotReqdDocumentsContract = ComplianceDataManager.GetRotReqDocumentsDetails(View.TenantId, View.CurrentLoggedInUserId);
            }
            else
            {
                View.lstApplicantRotReqdDocumentsContract = ComplianceDataManager.GetRotReqDocumentsDetails(View.FromAdminTenantID, View.FromAdminApplicantID);
            }
        }
    }
}




