using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;

namespace CoreWeb.Dashboard.Views
{
    public class DashboardLoaderPresenter : Presenter<IDashboardLoaderView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IDashboardController _controller;
        // public DashboardLoaderPresenter([CreateNew] IDashboardController controller)
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


        public Dictionary<string, string> GetDashboardMarkup(Guid userid, int tenantID)
        {

            //Dictionary<string, string> dashboard = SecurityManager.GetDashboardMarkup(userid, tenantID);
            //return dashboard;
            return null;
        }

        public void SaveWidgetStates(Guid userid, Dictionary<string, string> dashboard)
        {
            //SecurityManager.SavePersonalizedPreference(userid, dashboard);

        }
    }
}




