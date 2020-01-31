#region NameSpaces

#region System Defined
using INTSOF.SharedObjects;
using System.Linq;
using System;
#endregion

#region Project Specific
using Entity;
using Business.RepoManagers;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.WebSite.Views
{
    public class WebsitePageListPresenter : Presenter<IWebsitePageListView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IWebSiteController _controller;
        // public WebsitePageListPresenter([CreateNew] IWebSiteController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            GetWebSiteWebPageList();
            GetTanantName();
            GetWebsiteUrl();
        }

        public override void OnViewInitialized()
        {
           
        }

        /// <summary>
        /// Method to set the view property with WebsiteWebPage List.
        /// </summary>
        public void GetWebSiteWebPageList()
        {
            View.GetWebsSiteWebPageList = WebSiteManager.GetWebSiteWebPages(View.WebSiteId).Where(cond=>cond.lkpWebsiteWebPageType.Code==WebsiteWebPageType.Other.GetStringValue()).ToList();
        }

        public void GetTanantName()
        {
            View.TenantName=SecurityManager.GetTenant(View.TenantID).TenantName;
        }

        public void GetWebsiteUrl()
        {
            View.SiteUrl = WebSiteManager.GetWebSiteDetail(View.TenantID).URL;
        }
    }
}




