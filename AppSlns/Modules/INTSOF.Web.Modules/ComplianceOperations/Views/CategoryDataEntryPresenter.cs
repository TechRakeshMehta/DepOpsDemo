using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using System.Linq;

namespace CoreWeb.ComplianceOperations.Views
{
    public class CategoryDataEntryPresenter : Presenter<ICategoryDataEntryView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public CategoryDataEntryPresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
            GetComplianceItems();
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            //  View.ApplicantCategoryData = ApplicantManager.GetApplicantComplianceData(View.PackageSubscriptionId, View.ClientComplianceItemsContract.ClientComplianceCategoryId, View.TenantId);
        }

        // TODO: Handle other view events and set state in the view

        public void GetComplianceItems()
        {
           // View.ClientComplianceItems = ClientComplianceManagementManager.GetClientComplianceItemsListing(View.ClientComplianceItemsContract.ClientComplianceCategoryId, View.TenantId);
        }
    }
}




