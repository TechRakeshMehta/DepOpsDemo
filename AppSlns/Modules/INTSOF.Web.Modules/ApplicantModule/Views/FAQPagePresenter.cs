using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Entity;
using Business.RepoManagers;
using System.Web;
using ModuleUtility;
using INTSOF.Utils;

namespace CoreWeb.ApplicantModule.Views
{
    public class FAQPagePresenter : Presenter<IFAQPageView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IApplicantModuleController _controller;
        // public FAQPagePresenter([CreateNew] IApplicantModuleController controller)
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

            object data = ModuleUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_ID);
            if (data != null)
            {
                int webSiteID = (Int32)(data);
                WebSiteWebPage webSiteWebPage = WebSiteManager.GetWebSiteWebPage(webSiteID, View.PageName);
                View.HtmlMarkup = webSiteWebPage.HtmlMarkup;
            }            
        }

        // TODO: Handle other view events and set state in the view
    }
}




