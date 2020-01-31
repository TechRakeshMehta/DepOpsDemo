#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXWebSiteUtils.cs
// Purpose:
// 

#endregion

#region Namespaces

#region System Defined

using System.Web;

#endregion

#region Application Specific

using CoreWeb.IntsofCachingModel.Interface;
using CoreWeb.IntsofCachingModel.Interface.Services;
using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofSecurityModel.Interface;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using INTSOF.Contracts;

#endregion

#endregion

namespace CoreWeb.Shell
{
    /// <summary>
    /// This class handles all the operations to be performed on SysXWebSiteUtils.
    /// </summary>
    public static class SysXWebSiteUtils
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static ISysXSessionService _sysXSessionService;
        private static ISysXLoggerService _sysXLoggerService;
        private static ISysXSecurityService _sysXSecurityService;
        private static ISysXExceptionService _sysXExceptionService;
        private static IAllClientSessionService _allClientSessionService;
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Get instance of ISysXSessionService
        /// </summary>
        public static ISysXSessionService SessionService
        {
            get
            {
                 return _sysXSessionService ??
                    (_sysXSessionService = (HttpContext.Current.ApplicationInstance as IWebApplication).SysXSessionService);
               
            }
        }

        /// <summary>
        /// Get instance of ISysXLoggerService
        /// </summary>
        public static ISysXLoggerService LoggerService
        {
            get
            {
                try
                {
                    return _sysXLoggerService ??
                               (_sysXLoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService);
                }
                catch (System.Exception)
                {
                    return _sysXLoggerService = null;
                }                
            }
        }

        /// <summary>
        /// Get instance of ISysXSecurityService
        /// </summary>
        public static ISysXSecurityService SecurityService
        {
            get
            {            
                  return _sysXSecurityService ??
                    (_sysXSecurityService = (HttpContext.Current.ApplicationInstance as IWebApplication).SysXSecurityService);             
            }
        }

        /// <summary>
        /// Gets the exception service.
        /// </summary>
        /// <remarks></remarks>
        public static ISysXExceptionService ExceptionService
        {
            get
            {
                return _sysXExceptionService ??
                    (_sysXExceptionService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService);
                
            }
        }

        /// <summary>
        /// Gets the AllclientSession service.
        /// </summary>
        /// <remarks></remarks>
        public static IAllClientSessionService AllClientSessionService
        {
            get
            {
                return _allClientSessionService ??
                    (_allClientSessionService = (HttpContext.Current.ApplicationInstance as IWebApplication).AllClientSessionService);

            }
        }
        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        static SysXWebSiteUtils()
        {
            //WebClientApplication applicationInstance = (WebClientApplication)HttpContext.Current.ApplicationInstance;
            //RootContainer = applicationInstance.RootContainer;
        }

        #endregion

        #endregion
    }
}