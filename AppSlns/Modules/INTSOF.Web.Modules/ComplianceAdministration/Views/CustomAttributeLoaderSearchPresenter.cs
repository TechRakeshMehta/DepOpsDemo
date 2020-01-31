using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class CustomAttributeLoaderSearchPresenter : Presenter<ICustomAttributeLoaderSearchView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceAdministrationController _controller;
        // public CustomAttributeLoaderSearchPresenter([CreateNew] IComplianceAdministrationController controller)
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

        public void GetCustomAttributes(Int32 mappingRecordId,String useTypeCode, Int32 tenantId)
        {
            View.lstTypeCustomAttributes = ComplianceDataManager.GetCustomAttributesSearch(mappingRecordId, useTypeCode, tenantId);
        }

        /// <summary>
        /// Is Default Tenant.
        /// </summary>
        /// <returns></returns>
        public Boolean IsDefaultTenant()
        {
            Int32 tenantId= SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
            if (SecurityManager.DefaultTenantID == tenantId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}




