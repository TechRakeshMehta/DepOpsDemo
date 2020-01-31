using CoreWeb.IntsofCachingModel.Interface.Services;
using CoreWeb.IntsofCachingModel.Services;
using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.IntsofExceptionModel.Services;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofLoggerModel.Services;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using CoreWeb.IntsofSecurityModel.Services;
using INTSOF.Contracts;
using System;
using System.Web;


namespace WidgetDataWebService
{
    public class Global : HttpApplication, IWebApplication
    {
        #region Variables 
       
        #region Private Variables

        static ISysXSessionService sessionService;
        static ISysXSecurityService securityService;
        static ISysXLoggerService loggerService;
        static ISysXExceptionService exceptionService;
        static ISysXAppFabricCacheService appFabricService;
        static ISysXCachingDependencyService cacheDependencyService;
        static IAllClientSessionService allClientSessionService;
        
        #endregion
         
        #endregion

        #region Events

        protected void Application_Start(object sender, EventArgs e)
        {
            exceptionService = new SysXExceptionService();
            sessionService = new SysXSessionService();
            securityService = new SysXSecurityService();
            loggerService = new SysXLoggerService();
            exceptionService = new SysXExceptionService();
            appFabricService = new SysXAppFabricCacheService();
            cacheDependencyService = new SysXCachingDependencyService();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
        
        #endregion

        #region IWebApplication interface implementation

        ISysXLoggerService IWebApplication.LoggerService
        {
            get { return loggerService; }
        }

        ISysXCachingDependencyService IWebApplication.CacheDependencyService
        {
            get { return cacheDependencyService; }
        }

        ISysXAppFabricCacheService IWebApplication.AppFabricCacheService
        {
            get { return appFabricService; }
        }

        ISysXExceptionService IWebApplication.ExceptionService
        {
            get { return exceptionService; }
        }

        ISysXSessionService IWebApplication.SysXSessionService
        {
            get { return sessionService; }
        }

        ISysXSecurityService IWebApplication.SysXSecurityService
        {
            get { return securityService; }
        }

        IAllClientSessionService IWebApplication.AllClientSessionService
        {
            get { return allClientSessionService; }
        } 

        #endregion
    }
}