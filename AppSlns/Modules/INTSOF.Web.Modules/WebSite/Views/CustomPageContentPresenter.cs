using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Entity;
using Business.RepoManagers;
using INTSOF.Utils;


namespace CoreWeb.WebSite.Views
{
    public class CustomPageContentPresenter : Presenter<ICustomPageContentView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IWebSiteController _controller;
        // public CustomPageContentPresenter([CreateNew] IWebSiteController controller)
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
            WebSiteWebPage webSiteWebPage = WebSiteManager.GetWebSiteWebPage(View.CurrentViewContext.WebPageId);

            if (webSiteWebPage.IsNotNull())
                View.CurrentViewContext.PageHTML = webSiteWebPage.HtmlMarkup;
            else
                View.CurrentViewContext.PageHTML = String.Empty;
        }

        // TODO: Handle other view events and set state in the view
    }
}




