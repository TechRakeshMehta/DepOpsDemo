#region Namespaces

#region System Defined
using INTSOF.SharedObjects;
using System.Collections.Generic;
using System;
#endregion

#region Project Specific
using Entity;
using INTSOF.Utils;
using Business.RepoManagers;
#endregion

#endregion

namespace CoreWeb.WebSite.Views
{
    public class WebSiteSetupPresenter : Presenter<IWebSiteSetup>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads

        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            GetWebsiteData();
        }

        /// <summary>
        /// Get Website data.
        /// </summary>
        public void GetWebsiteData()
        {
            Entity.WebSite website = WebSiteManager.GetWebSiteDetail(View.TenantID);
            View.SiteUrl = website.URL;
            View.WebsiteId = website.WebSiteID;
            View.Notes = website.Notes;
            View.LoginImageUrl = website.LoginPageImageURL;
            View.RegistrationImageUrl = website.TopRightImageUrl;
            foreach (WebSiteWebConfig webSiteWebConfigTemp in website.WebSiteWebConfigs)
            {
                View.SiteTitle = webSiteWebConfigTemp.SiteTitle;
                //View.MasterPage = webSiteWebConfigTemp.DefaultMasterPage;
                View.Theme = webSiteWebConfigTemp.DefaultTheme;
            }
        }

        /// <summary>
        /// Method to get the markup value 
        /// </summary>
        /// <returns>TenantWebsiteMapping</returns>
        public TenantWebsiteMapping GetMarkUpValue()
        {
            return WebSiteManager.GetTenantWebsiteMapping(View.TenantID);
        }

        /// <summary>
        /// Method to save the website Data.
        /// </summary>
        /// <param name="currentUserId">currentUserId</param>
        /// <returns>Boolean</returns>
        public Boolean SaveWebSiteData(Int32 currentUserId)
        {
            Entity.WebSite defaultWebSiteData = WebSiteManager.GetWebSiteDetail(1);
            String defaultLoginPageImageUrl = String.Empty;
            String defaultHeaderHtml = String.Empty;
            String defaultFooterText = String.Empty;
            String defaultMasterPage = String.Empty;
            String defaultTheme = String.Empty;
            if (defaultWebSiteData != null)
            {
                defaultLoginPageImageUrl = defaultWebSiteData.LoginPageImageURL;
                foreach (WebSiteWebConfig webSiteWebConfigDefault in defaultWebSiteData.WebSiteWebConfigs)
                {
                    defaultHeaderHtml = webSiteWebConfigDefault.HeaderHtml;
                    defaultFooterText = webSiteWebConfigDefault.FooterText;
                    defaultMasterPage = webSiteWebConfigDefault.DefaultMasterPage;
                }
                Entity.WebSite tempWebSite = new Entity.WebSite();
                tempWebSite.URL = View.SiteUrl;
                tempWebSite.Notes = View.Notes;
                tempWebSite.IsSiteUp = true;
                if (View.LoginImageUrl == null)
                    View.LoginImageUrl = defaultLoginPageImageUrl;
                    tempWebSite.LoginPageImageURL = View.LoginImageUrl == String.Empty ? defaultLoginPageImageUrl : View.LoginImageUrl;
                tempWebSite.CreatedBy = currentUserId.ToString();
                tempWebSite.CreatedDate = DateTime.Now;
                WebSiteWebConfig webSiteWebConfig = new WebSiteWebConfig();
                webSiteWebConfig.SiteTitle = View.SiteTitle;
                webSiteWebConfig.DefaultMasterPage = defaultMasterPage;// View.MasterPage == String.Empty ? defaultMasterPage : View.MasterPage;
                webSiteWebConfig.HeaderHtml = defaultHeaderHtml;
                webSiteWebConfig.FooterText = defaultFooterText;
                webSiteWebConfig.DefaultTheme = View.Theme == String.Empty ? defaultTheme : View.Theme;
                webSiteWebConfig.CreatedByID = currentUserId;
                webSiteWebConfig.CreatedOn = DateTime.Now;
                tempWebSite.WebSiteWebConfigs.Add(webSiteWebConfig);
                TenantWebsiteMapping tenantwebsiteMap = new TenantWebsiteMapping();
                tenantwebsiteMap.TWM_CreatedOn = DateTime.Now;
                tenantwebsiteMap.TWM_IsCustomSiteMarkup = View.IsSiteMarkup;
                tenantwebsiteMap.TWM_TenantID = View.TenantID;
                Boolean IsSaved = WebSiteManager.SaveWebsiteDetail(tempWebSite, tenantwebsiteMap);
                View.WebsiteId = tempWebSite.WebSiteID;
                return IsSaved;
            }
            return false;
        }

        /// <summary>
        /// Update the WebSite Data
        /// </summary>
        /// <param name="currentUserId">currentUserId</param>
        /// <returns>Boolean</returns>
        public Boolean UpdateWebSiteData(Int32 currentUserId)
        {
            Entity.WebSite defaultWebSiteData = WebSiteManager.GetWebSiteDetail(1);
            String defaultLoginPageImageUrl = String.Empty;
            String defaultHeaderHtml = String.Empty;
            String defaultFooterText = String.Empty;
            String defaultMasterPage = String.Empty;
            String defaultTheme = String.Empty;
            if (defaultWebSiteData != null)
            {
                defaultLoginPageImageUrl = defaultWebSiteData.LoginPageImageURL;
                foreach (WebSiteWebConfig webSiteWebConfigDefault in defaultWebSiteData.WebSiteWebConfigs)
                {
                    defaultHeaderHtml = webSiteWebConfigDefault.HeaderHtml;
                    defaultFooterText = webSiteWebConfigDefault.FooterText;
                    defaultMasterPage = webSiteWebConfigDefault.DefaultMasterPage;
                }
                Entity.WebSite webSiteData = WebSiteManager.GetWebSiteDetail(View.TenantID);
                webSiteData.URL = View.SiteUrl;
                webSiteData.Notes = View.Notes;
                webSiteData.LoginPageImageURL = View.LoginImageUrl == String.Empty ? defaultLoginPageImageUrl : View.LoginImageUrl;
                webSiteData.TopRightImageUrl = View.RegistrationImageUrl;
                webSiteData.UpdatedBy = currentUserId.ToString();
                webSiteData.UpdatedDate = DateTime.Now;
                foreach (WebSiteWebConfig webSiteWebConfigTemp in webSiteData.WebSiteWebConfigs)
                {
                    webSiteWebConfigTemp.SiteTitle = View.SiteTitle;
                    webSiteWebConfigTemp.DefaultMasterPage = defaultMasterPage;// View.MasterPage == String.Empty ? defaultMasterPage : View.MasterPage;
                    webSiteWebConfigTemp.DefaultTheme = View.Theme == String.Empty ? defaultTheme : View.Theme;
                    webSiteWebConfigTemp.ModifiedByID = currentUserId;
                    webSiteWebConfigTemp.ModifiedOn = DateTime.Now;
                    webSiteData.WebSiteWebConfigs.Add(webSiteWebConfigTemp);
                }
                TenantWebsiteMapping tenantwebsiteMap = WebSiteManager.GetTenantWebsiteMapping(View.TenantID);
                tenantwebsiteMap.TWM_IsCustomSiteMarkup = View.IsSiteMarkup;
                tenantwebsiteMap.TWM_ModifiedBy = currentUserId;
                tenantwebsiteMap.TWM_ModifiedOn = DateTime.Now;
                return WebSiteManager.UpdateObject();
            }
            return false;
        }

        /// <summary>
        /// Update Tenant website mapping table.
        /// </summary>
        /// <param name="currentUserId">currentUserId</param>
        /// <returns>Boolean</returns>
        public Boolean UpdateTenantWebSiteMapping(Int32 currentUserId)
        {
            TenantWebsiteMapping tenantwebsiteMap = WebSiteManager.GetTenantWebsiteMapping(View.TenantID);
            if (tenantwebsiteMap.TWM_IsCustomSiteMarkup == false && View.IsSiteMarkup == true)
            {
                tenantwebsiteMap.TWM_IsCustomSiteMarkup = View.IsSiteMarkup;
                tenantwebsiteMap.TWM_ModifiedBy = currentUserId;
                tenantwebsiteMap.TWM_ModifiedOn = DateTime.Now;
                return WebSiteManager.UpdateObject();
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Method to get the Tenant Name 
        /// </summary>
        /// <returns>String</returns>
        public String GetTanantName()
        {
            return SecurityManager.GetTenant(View.TenantID).TenantName;
        }

        /// <summary>
        /// Method to update the login image path in DB.
        /// </summary>
        /// <param name="currentUserId"></param>
        public void UpdateLoginImagePath(Int32 currentUserId)
        {
            Entity.WebSite website = WebSiteManager.GetWebSiteDetail(View.TenantID);
            website.LoginPageImageURL = View.LoginImageUrl;
            website.UpdatedBy = currentUserId.ToString();
            website.UpdatedDate = DateTime.Now;
            WebSiteManager.UpdateObject();
        }
    }
}
