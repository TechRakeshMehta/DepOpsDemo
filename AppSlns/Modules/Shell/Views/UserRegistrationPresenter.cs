﻿using Business.RepoManagers;
using INTSOF.SharedObjects;

namespace CoreWeb.Shell.Views
{
    public class UserRegistrationPresenter : Presenter<IUserRegistrationView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IShellController _controller;
        // public UserRegistrationPresenter([CreateNew] IShellController controller)
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


        public int GetWebsiteTenantId(string websiteUrl)
        {
            return WebSiteManager.GetWebsiteTenantId(websiteUrl);
        }

        public bool IsLocationServiceTenant(int tenantId)
        {
           return SecurityManager.IsLocationServiceTenant(tenantId);
        }
       
    }
}



