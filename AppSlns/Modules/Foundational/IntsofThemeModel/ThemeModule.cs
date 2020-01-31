using Business.RepoManagers;
using Entity;
using INTSOF.Utils;
using ModuleUtility;
using System;
using System.Configuration;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
//using IntsofExceptionModel.Interface;
//using CoreWeb.

namespace CoreWeb.IntsofThemeModel
{
    public class ThemeModule : IHttpModule
    {
        /// <summary>
        /// Registers the PreRequestHandlerExecute handler so that we can 
        /// insert our information into the http request pipeline.
        /// </summary>
        /// <param name="application"></param>
        public void Init(HttpApplication application)
        {
            application.PreRequestHandlerExecute += new EventHandler(application_PreRequestHandlerExecute);
            application.EndRequest += new EventHandler(Application_EndRequest);
        }


        public void Dispose()
        { }

        /// <summary>
        /// This is where the register for the pages PreInit event and enable themes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            if (page != null)
            {
                page.EnableTheming = true;
                page.PreInit += new EventHandler(page_PreInit);
            }
        }

        /// <summary>
        /// This is called for each page request. Here we use the
        /// data in the cache that was loaded at application startup
        /// in the GlobalAsax.cs file. This is where we set the 
        /// master page and theme for the request based on the url
        /// that the request came in on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void page_PreInit(object sender, EventArgs e)
        {
            try
            {
                Page page = (Page)sender;
                String _currentUrl = page.Request.ServerVariables.Get("server_name");
                ModuleUtils.LoggerService.GetLogger().Info("IntsofThemeModel The vaue of _currentUrl: " + _currentUrl);
                Entity.WebSite _currentWebSite = WebSiteManager.GetWebSite(_currentUrl);

                if (_currentWebSite == null)
                {
                    if (_currentUrl.Contains("localhost"))
                    {
                        _currentWebSite = WebSiteManager.GetDefaultTenantWebsite();
                    }
                    else if (!IsCentralLoginType(_currentUrl) && !IsSharedUserLoginType(_currentUrl)) // Skip the check to validate the Website existence of the 'Central Login' screen and UAT-1110 "Shared User Login"
                    {
                        //stop logging  website resolution failed  event, it will be the case in every 10 seconds in Load Balancing environment and log file keeps growing.
                        // ModuleUtils.LoggerService.GetLogger().Error(String.Format("Website resolution failed for URL:{0} in table 'WebSites' in security database.", _currentUrl));
                        //HttpContext.Current.Response.StatusCode = 503;
                        HttpContext.Current.Response.Write(String.Format("Website configuration not found for domain '{0}'. Please contact your Institute's administrator.", _currentUrl));
                        //HttpContext.Current.Response.StatusDescription = "Website is not ready. Please try again later.";
                        HttpContext.Current.Response.End();
                    }
                    //UAT-1110- Profile sharing - Setting Default theme for the shared user.
                    else if (IsSharedUserLoginType(_currentUrl))
                    {
                        ModuleUtils.SessionService.SetCustomData(ResourceConst.CLIENT_WEB_SITE_THEME, "Default");
                        return;
                    }
                }
                else if (!_currentWebSite.IsSiteUp)
                {
                    if (_currentWebSite.SiteDownText.IsNotNull())
                    {
                        HttpContext.Current.Response.Write(_currentWebSite.SiteDownText);
                    }
                    else
                    {
                        HttpContext.Current.Response.Write(String.Format("Complio is currently undergoing routine maintenance. Please contact us at complio@americandatabank.com with any queries or concern.", _currentUrl));
                    }

                    HttpContext.Current.Response.End();
                }

                if (ModuleUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_ID) == null && _currentWebSite.IsNotNull())
                {

                    ModuleUtils.LoggerService.GetLogger().Info("IntsofThemeModel The vaule of _currentWebSite  WebSiteID: " + _currentWebSite.WebSiteID);

                    WebSiteWebConfig webSiteWebConfig = WebSiteManager.GetWebSiteWebConfig(_currentWebSite.WebSiteID);
                    ModuleUtils.LoggerService.GetLogger().Info("IntsofThemeModel The vaue of webSiteWebConfig is null" + webSiteWebConfig.IsNull());
                    ModuleUtils.SessionService.SetCustomData(ResourceConst.CLIENT_WEB_SITE_ID, _currentWebSite.WebSiteID);

                    String theme = (webSiteWebConfig.IsNotNull() && String.IsNullOrEmpty(webSiteWebConfig.DefaultTheme)) ? "Default" : webSiteWebConfig.IsNull() ? "Default" : webSiteWebConfig.DefaultTheme;
                    ModuleUtils.SessionService.SetCustomData(ResourceConst.CLIENT_WEB_SITE_THEME, theme);
                }

                if (ModuleUtils.SessionService.SysXMembershipUser != null)
                {
                    String _userThemePreference = ModuleUtils.SessionService.SysXMembershipUser.Comment;

                    // If user has set the theme or user has updated the theme
                    if (!(String.IsNullOrEmpty(_userThemePreference)))
                    {
                        ModuleUtils.SessionService.SetCustomData(ResourceConst.CLIENT_WEB_SITE_THEME, _userThemePreference);
                    }
                }
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            catch (ThreadAbortException thex)
            {
                //You can ignore this 
            }
            catch (Exception ex)
            {
                ModuleUtils.LoggerService.GetLogger().Error(ex.Message, ex);
            }
        }

        public class MockSessionHttpHandler : IHttpHandler, IRequiresSessionState
        {
            internal readonly IHttpHandler _mainHandler;

            public MockSessionHttpHandler(IHttpHandler mainHandler)
            {
                _mainHandler = mainHandler;
            }

            public void ProcessRequest(HttpContext context)
            {
                throw new InvalidOperationException("MockSessionHttpHandler is not implemented.");
            }

            public bool IsReusable
            {
                get { return false; }
            }
        }

        protected void Application_EndRequest(Object source, EventArgs e)
        {
            //BaseManager.ClearDBContexts();
        }

        /// <summary>
        /// Returns whether the Central Login screen is being opened or not
        /// Will be executed only when the Central login screen is NOT from the Website table.
        /// </summary>
        /// <param name="_currentUrl"></param>
        /// <param name="_centralLoginUrl"></param>
        /// <returns></returns>
        private Boolean IsCentralLoginType(String _currentUrl)
        {
            var _centralLoginUrl = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_CENTRAL_LOGIN_URL]);
            String _centralHost = _centralLoginUrl;
            if (_centralLoginUrl.Contains("http"))
            {
                Uri _url = new Uri(_centralLoginUrl);
                _centralHost = _url.Host;
            }

            return !String.IsNullOrEmpty(_centralLoginUrl) && _currentUrl.ToLower().Trim().Contains(_centralHost.ToLower().Trim());
        }


        /// <summary>
        /// If Shared User Login URL is not present in the WEBSITE table
        /// </summary>
        /// <param name="_currentUrl"></param>
        /// <param name="_centralLoginUrl"></param>
        /// <returns></returns>
        private Boolean IsSharedUserLoginType(String _currentUrl)
        {
            var _sharedUserLoginUrl = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);
            String _sharedUserHost = _sharedUserLoginUrl;
            if (_sharedUserLoginUrl.Contains("http"))
            {
                Uri _url = new Uri(_sharedUserLoginUrl);
                _sharedUserHost = _url.Host;
            }

            return !String.IsNullOrEmpty(_sharedUserLoginUrl) && _currentUrl.ToLower().Trim().Contains(_sharedUserHost.ToLower().Trim());
        }
    }
}
