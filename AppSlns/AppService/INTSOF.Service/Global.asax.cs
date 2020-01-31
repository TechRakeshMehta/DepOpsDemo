using CoreWeb.IntsofCachingModel.Interface.Services;
using CoreWeb.IntsofCachingModel.Services;
using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.IntsofExceptionModel.Services;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofLoggerModel.Services;
using CoreWeb.Shell;
using INTSOF.Logger.consts;
using INTSOF.ServiceModelInterface;
using INTSOF.ServiceUtil;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace INTSOF.Service
{

    public class Global : System.Web.HttpApplication, IIntsofService
    {
        static ISysXAppFabricCacheService appFabricService;
        static ISysXCachingDependencyService cacheDependencyService;
        static ISysXLoggerService loggerService;
        static long loggedRequestId;
        static ISysXExceptionService exceptionService;

        protected void Application_Start(object sender, EventArgs e)
        {
            //ServiceSingleton.Instance.AppFabricService = new SysXAppFabricCacheService();
            //ServiceSingleton.Instance.CacheDependencyService = new SysXCachingDependencyService();
            //ServiceSingleton.Instance.LoggerService = new SysXLoggerService();
            //ServiceSingleton.Instance.ExceptionService = new SysXExceptionService();
            appFabricService = new SysXAppFabricCacheService();
            loggerService = new SysXLoggerService();
            cacheDependencyService = new SysXCachingDependencyService();
            loggedRequestId = AppConsts.NONE;
            exceptionService = new SysXExceptionService();
        }

        void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            long currentRequestId = loggedRequestId;
            HttpContext.Current.Items.Add(SysXLoggerConst.REQUESTID, currentRequestId);
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
            var logger = SysXWebSiteUtils.LoggerService.GetLogger();

            // Code that runs when an unhandled error occurs
            if (logger.IsNotNull() && logger.IsErrorEnabled)
            {
                // Get the exception object.
                Exception exc = Server.GetLastError();
                if (exc.IsNotNull())
                {
                    logger.Error("Unhandled error: " + exc.Message + Environment.NewLine + exc.StackTrace, exc);
                }
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        ISysXAppFabricCacheService IIntsofService.AppFabricCacheService
        {
            get { return appFabricService; }
            //get
            //{
            //    return ServiceSingleton.Instance.AppFabricService;
            //}
        }

        ISysXCachingDependencyService IIntsofService.CacheDependencyService
        {
            get { return cacheDependencyService; }
            //get
            //{
            //    return ServiceSingleton.Instance.CacheDependencyService;
            //}
        }
        ISysXLoggerService IIntsofService.LoggerService
        {
            get { return loggerService; }
            //get
            //{
            //    return ServiceSingleton.Instance.LoggerService;
            //}
        }
        ISysXExceptionService IIntsofService.ExceptionService
        {
            get { return exceptionService; }
            //get
            //{
            //    return ServiceSingleton.Instance.ExceptionService;
            //}
        }
    }
}