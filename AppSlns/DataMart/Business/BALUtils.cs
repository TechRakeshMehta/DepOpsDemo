#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXExceptionMessageFormatter.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined


using System;
using System.Web;
using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofSecurityModel.Interface.Services;

#endregion

#region Application Specific

using DataMart.DAL.Interfaces;
using DataMart.DAL.Repository;
using INTSOF.Contracts;
using INTSOF.Logger;
using INTSOF.ServiceUtil;
using INTSOF.Utils;
#endregion

#endregion

namespace DataMart.Business
{
    /// <summary>
    /// This class handles the operation related to BAL.
    /// </summary>
    internal static class BALUtils
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static ISysXLoggerService _sysXLoggerService;
        private static ILogger _logger = null;
        private static String _classModule;
        //Added AMS
        private static ISysXSessionService _sysxSessionService = null;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Current Class Module
        /// </summary>
        public static String ClassModule
        {
            get
            {
                return _classModule;
            }
            set
            {
                _classModule = value;
            }
        }

        /// <summary>
        /// Get instance of ISysXLoggerService
        /// </summary>
        public static ISysXLoggerService LoggerService
        {
            get
            {
                if (_sysXLoggerService.IsNull())
                {
                    //Returned null instance of ExceptionService if call is made from batch job
                    if (HttpContext.Current.IsNotNull())
                    {
                        //if (HttpContext.Current.ApplicationInstance is IMVCApplication)
                        //{
                        //    _sysXLoggerService = (HttpContext.Current.ApplicationInstance as IMVCApplication).LoggerService;
                        //}
                        //else
                        //{
                        if ((HttpContext.Current.ApplicationInstance as IWebApplication).IsNotNull())
                        {
                            _sysXLoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                        }
                        //}
                    }
                    //Check if ParallelTaskContext is not null and return the instance of ISysXLoggerService
                    else if (ParallelTaskContext.Current.IsNotNull())
                    {
                        _sysXLoggerService = ParallelTaskContext.LoggerService();
                    }
                    else
                    {
                        _sysXLoggerService = null;
                    }
                }
                return _sysXLoggerService;
            }
        }

        #endregion

        #region Private Properties
        private static ISysXExceptionService _exceptionService;
        public static ISysXExceptionService ExceptionService
        {
            get
            {
                if (_exceptionService.IsNull())
                {

                    if (HttpContext.Current.IsNotNull())
                    {
                        if (HttpContext.Current.ApplicationInstance is INTSOF.ServiceModelInterface.IIntsofService)
                        {
                            _exceptionService = (HttpContext.Current.ApplicationInstance as INTSOF.ServiceModelInterface.IIntsofService).ExceptionService;
                        }
                        else
                        {
                            _exceptionService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                        }
                    }
                    //Check if ParallelTaskContext is not null and return the instance of ISysXExceptionService
                    else if (ParallelTaskContext.Current.IsNotNull())
                    {
                        _exceptionService = ParallelTaskContext.ExceptionService();
                    }
                    else
                    {
                        _exceptionService = null;
                    }

                }

                return _exceptionService;
            }
        }
        private static ILogger Logger
        {
            get
            {
                if (_logger.IsNull())
                {
                    //Returned null instance of logger if call is made from batch job
                    if (LoggerService != null)
                        _logger = BALUtils.LoggerService.GetLogger();
                }
                return _logger;
            }
        }

        #endregion

        #endregion

        #region Logging Methods

        /// <summary>
        /// Used to Log the Error Message
        /// </summary>
        /// <param name="infoMessage"></param>
        /// <param name="ex">Exception</param>
        public static void LogError(String infoMessage, Exception ex)
        {
            //Returned nothing if call is made from batch job and loging is done using Nlog
            if (ExceptionService != null)
                ExceptionService.HandleError(infoMessage, ex);
        }

        /// <summary>
        /// Used to Log the Error Message
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <remarks></remarks>
        public static void LogError(Exception ex)
        {
            //Returned nothing if call is made from batch job and loging is done using Nlog
            if (ExceptionService != null)
                ExceptionService.HandleError(ex.Message, ex);
        }
        /// <summary>
        /// Used to Log the debug Message
        /// </summary>
        /// <param name="message"></param>
        public static void LogDebug(string message)
        {
            if (ExceptionService != null)
                ExceptionService.HandleDebug(message);
        }
        #endregion

        #region Data Mart
        public static IDataMartRepository GetDataMartRepoInstance(Int32 tenantID = 1)
        {
            return new DataMartRepository(tenantID);
        }
        #endregion
    }
}
