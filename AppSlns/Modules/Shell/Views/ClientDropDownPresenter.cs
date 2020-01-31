using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;

namespace CoreWeb.Shell.Views
{
    public class ClientDropDownPresenter : Presenter<IClientDropDownView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IShellController _controller;
        // public ClientDropDownPresenter([CreateNew] IShellController controller)
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

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }


        public Boolean IsThirdPartyTenant
        {
            get
            {
                return SecurityManager.IsTenantThirdPartyType(View.TenantId, TenantType.Compliance_Reviewer.GetStringValue());
            }
        }

        public void GetTenants()
        {
            if (IsDefaultTenant)
            {

                View.lstTenant = ComplianceDataManager.getClientTenant();
            }
            else if (IsThirdPartyTenant)
            {
                View.lstTenant = ComplianceDataManager.getParentTenant(View.TenantId);
            }
            else
            {
                List<Tenant> lstTnt = new List<Tenant>();
                Entity.Tenant tnt = SecurityManager.GetTenant(View.TenantId);
                lstTnt.Add(new Tenant { TenantID = tnt.TenantID, TenantName = tnt.TenantName });
                View.lstTenant = lstTnt;
            }

        }
        

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }
    }
}




