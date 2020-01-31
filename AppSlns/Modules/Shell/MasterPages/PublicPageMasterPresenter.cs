#region Namespaces

#region System Defined
using System;
using System.Linq;
using INTSOF.SharedObjects;

#endregion

#region Application Specific

using Entity;
using Business.RepoManagers;


#endregion

#endregion

namespace CoreWeb.Shell.MasterPages
{
    public class PublicPageMasterPresenter : Presenter<IPublicPageMasterView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IShellController _controller;
        // public PublicPageMasterPresenter([CreateNew] IShellController controller)
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

        public void GetWebsiteFooter()
        {
            if (View.WebSiteId > 0)
            {
                WebSiteWebConfig webSiteWebConfig = WebSiteManager.GetWebSiteWebConfig(View.WebSiteId);
                if (webSiteWebConfig != null)
                {
                    View.WebsitePages = webSiteWebConfig.WebSite.WebSiteWebPages.Where(webPage => !webPage.IsDeleted && webPage.IsActive).OrderBy(linkOrder => linkOrder.LinkOrder).ToList();
                    View.FooterContent = webSiteWebConfig.FooterText;
                }
            }
        }

        public void GetClientLogo()
        {
            if (View.WebSiteId > 0)
                View.ClientLogoUrl = WebSiteManager.GetWebSiteLoginImage(View.WebSiteId);
        }

        public void GetClientRightLogo()
        {
            if (View.WebSiteId > 0)
                View.ClientRightLogoUrl = WebSiteManager.GetWebSiteRightLogoImage(View.WebSiteId);
        }

        public int GetDefaultTenantWebsite()
        {
            return WebSiteManager.GetDefaultTenantWebsite().WebSiteID;
        }

        //UAT-2439
        public string GetTenantName()
        {
            if (View.TenantId > 0)
            {
                return SecurityManager.GetTenant(View.TenantId).TenantName;
            }
            else
            {
                return string.Empty;
            }
        }

        //UAT 3600
        public void IsLocationServiceTenant()
        {
            int TenantId = GetTenantID(View.WebSiteId);
            if (TenantId > 0)
                View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(TenantId);
        }

        public int GetTenantID(int WebSiteId)
        {
            return SecurityManager.GetTenantID(WebSiteId);
        }

        public Entity.WebSite GetWebSite(Int32 tenantId)
        {
            return WebSiteManager.GetWebSiteDetail(tenantId);
        }

        public string GetLocationTenantCompanyName()
        {
             return SecurityManager.GetLocationTenantCompanyName();
        }
    }
}




