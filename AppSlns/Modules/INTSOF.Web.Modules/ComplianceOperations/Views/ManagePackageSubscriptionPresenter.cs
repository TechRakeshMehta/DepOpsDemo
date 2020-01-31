using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManagePackageSubscriptionPresenter : Presenter<IManagePackageSubscriptionView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public ManagePackageSubscriptionPresenter([CreateNew] IComplianceOperationsController controller)
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

        public void GetClientCompliancePackages()
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(View.loggedInUserId);
            View.tenantId = organizationUser.Organization.Tenant.TenantID;
            View.ClientCompliancePackages = ComplianceSetupManager.GetClientCompliancePackageByClient(View.loggedInUserId, View.tenantId);
        }
    }
}




