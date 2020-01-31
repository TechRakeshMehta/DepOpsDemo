using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System.Configuration;
using Business.RepoManagers;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ItemDataReadOnlyModePresenter : Presenter<IItemDataReadOnlyModeView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public ItemDataReadOnlyModePresenter([CreateNew] IComplianceOperationsController controller)
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

        /// <summary>
        /// UAT 1740: Move 604 notification from the time of login to when an admin attempts for view an employment result report. 
        /// </summary>
        /// <returns></returns>
        public Boolean IsEDFormPreviouslyAccepted()
        {
            Double employmentDisclosureIntervalHours = AppConsts.NONE;
            if (!ConfigurationManager.AppSettings["EmploymentDisclosureIntervalHours"].IsNullOrEmpty())
            {
                employmentDisclosureIntervalHours = Convert.ToDouble(ConfigurationManager.AppSettings["EmploymentDisclosureIntervalHours"]);
            }
            return SecurityManager.IsEDFormPreviouslyAccepted(View.CurrentLoggedInUserId, employmentDisclosureIntervalHours);
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                // return View.Tenant.TenantID.Equals(SecurityManager.DefaultTenantID);
                return View.SelectedTenantId_Global.Equals(SecurityManager.DefaultTenantID);
            }
        }
    }
}




