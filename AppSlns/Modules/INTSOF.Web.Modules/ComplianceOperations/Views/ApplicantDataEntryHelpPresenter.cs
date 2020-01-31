using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
namespace CoreWeb.ComplianceOperations.Views
{
    public class ApplicantDataEntryHelpPresenter : Presenter<IApplicantDataEntryHelpView>
    {


        public override void OnViewLoaded()
        {
            LoadContent();
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        private void LoadContent()
        {
            if (View.TenantID.IsNotNull() && View.PackageID.IsNotNull())
            {
                String recordType = RecordType.Package.GetStringValue();
                String websiteWebPageType = WebsiteWebPageType.DataEntryHelp.GetStringValue();
                InstitutionWebPage institutionWebPage = ComplianceSetupManager.GeDateHelpHtmlFromtWebSiteWebPage(View.TenantID, View.PackageID, recordType, websiteWebPageType);
                if (institutionWebPage != null)
                {
                    View.PageHTML = institutionWebPage.HtmlMarkup;
                }
            }
            //WebSite webSite = WebSiteManager.GetWebSiteDetail(View.TenantID);
            //if (webSite != null)
            //{
                //WebSiteWebConfig webSiteConfig = WebSiteManager.GetWebSiteWebConfig(webSite.WebSiteID);
                //if (webSiteConfig != null)
                //    View.PageHTML = webSiteConfig.ApplicantDataEntryHelp;
                
                //WebSiteWebPage webSiteWebPage = WebSiteManager.GeDateHelpHtmlFromtWebSiteWebPage(webSite.WebSiteID, View.PackageID, recordType, websiteWebPageType);
                //if (webSiteWebPage != null)
                //{
                //    View.PageHTML = webSiteWebPage.HtmlMarkup;
                //}

            //}
        }
    }
}




