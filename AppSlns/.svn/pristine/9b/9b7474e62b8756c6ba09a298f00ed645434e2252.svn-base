using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreWeb.IntsofCachingModel.Interface.Services;
using CoreWeb.IntsofCachingModel.Services;
using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.IntsofExceptionModel.Services;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofLoggerModel.Services;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using CoreWeb.IntsofSecurityModel.Services;
using INTSOF.Contracts;
using INTSOF.Utils;
using INTSOF.Logger.consts;

public class Global : HttpApplication, IWebApplication
{
    static ISysXSessionService sessionService;
    static ISysXSecurityService securityService;
    static ISysXLoggerService loggerService;
    static ISysXExceptionService exceptionService;
    static ISysXAppFabricCacheService appFabricService;
    static ISysXCachingDependencyService cacheDependencyService;
    static IAllClientSessionService allClientSessionService;
    static long loggedRequestId;


    void Application_Start(object sender, EventArgs e)
    {
        sessionService = new SysXSessionService();
        securityService = new SysXSecurityService();
        loggerService = new SysXLoggerService();
        exceptionService = new SysXExceptionService();
        appFabricService = new SysXAppFabricCacheService();
        cacheDependencyService = new SysXCachingDependencyService();
        allClientSessionService = new AllClientSessionService();
        loggedRequestId = AppConsts.NONE;
    }

    void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        var logger = CoreWeb.Shell.SysXWebSiteUtils.LoggerService.GetLogger();
        System.Threading.Interlocked.Increment(ref loggedRequestId);

        long currentRequestId = loggedRequestId;
        HttpContext.Current.Items.Add(SysXLoggerConst.REQUESTID, currentRequestId);

        if (logger.IsNotNull() && (logger.IsInfoEnabled || logger.IsDebugEnabled))
        {
            string urlParamName = String.Empty;

            if (!Request.QueryString[AppConsts.UCID].IsNullOrEmpty())
                urlParamName = AppConsts.UCID;
            else if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNullOrEmpty())
                urlParamName = AppConsts.QUERYSTRING_ARGUMENT;

            Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>();

            if (!urlParamName.IsNullOrEmpty())
                encryptedQueryString.ToDecryptedQueryString(Request.QueryString[urlParamName]);

            String strRequestURLLog = String.Empty;
            var currentRequestFullUrl = HttpContext.Current.Request.Url;
            string requestUri = String.Empty;
            if (currentRequestFullUrl.Query.IsNullOrEmpty())
            {
                requestUri = currentRequestFullUrl.AbsoluteUri;
            }
            else
            {
                requestUri = currentRequestFullUrl.AbsoluteUri.Substring(0, currentRequestFullUrl.AbsoluteUri.IndexOf("?"));
            }

            if (encryptedQueryString.Count > AppConsts.NONE)
            {
                strRequestURLLog = "ID: {0} URI: {1} Params: {2}";
                String parameters = String.Join("&", encryptedQueryString.Select(col => col.Key + "=" + col.Value).ToArray());
                strRequestURLLog = strRequestURLLog.Format(new object[] { loggedRequestId, requestUri, parameters });
            }
            else
            {
                strRequestURLLog = "ID: {0} FullURL: {1}";
                strRequestURLLog = strRequestURLLog.Format(new object[] { loggedRequestId, currentRequestFullUrl });
            }

            if (!HttpContext.Current.Items.Contains("RequestURLLog"))
                HttpContext.Current.Items.Add("RequestURLLog", strRequestURLLog);

            String requestDetails = ">> Begin Request: " + strRequestURLLog;
            if (requestUri.EndsWith(".aspx"))
                logger.Info(requestDetails);
            else
                logger.Debug(requestDetails);
        }
    }


    void Application_PostRequestHandlerExecute(object sender, EventArgs e)
    {
        var logger = CoreWeb.Shell.SysXWebSiteUtils.LoggerService.GetLogger();
        if (logger.IsNotNull() && (logger.IsInfoEnabled || logger.IsDebugEnabled))
        {
            if (HttpContext.Current.Items["RequestURLLog"].IsNotNull())
            {
                String strRequestURLLog = (String)HttpContext.Current.Items["RequestURLLog"];
                String requestDetails = "<< End Request: " + strRequestURLLog;

                if (HttpContext.Current.Request.Url.AbsoluteUri.EndsWith(".aspx"))
                    logger.Info(requestDetails);
                else
                    logger.Debug(requestDetails);
            }
        }
    }

    void Application_Error(object sender, EventArgs e)
    {
        var logger = CoreWeb.Shell.SysXWebSiteUtils.LoggerService.GetLogger();
        // Code that runs when an unhandled error occurs
        if (logger.IsNotNull() && logger.IsErrorEnabled)
        {
            // Get the exception object.
            Exception exc = Server.GetLastError();
            if (exc.IsNotNull())
            {
                //HTML TAG and XSS error throw.
                if(!Response.IsNullOrEmpty())
                {
                    Response.AddHeader("Content-Type", "text/html");
                }
                if (!exc.Message.IsNullOrEmpty() && exc.HResult == AppConsts.XSS_HTML_INJECTION_ERROR_NUMBER 
                    && exc.Message.ToLower().Contains(AppConsts.HTML_XSS_INJECTION_ERROR_MSG))
                {
                    if (!Response.IsNullOrEmpty())
                    {  
                        Response.Write(exc.HResult.ToString());
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
                logger.Error("Unhandled error: " + exc.Message + Environment.NewLine + exc.StackTrace, exc);
            }
        }
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    #region IWebApplication

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
