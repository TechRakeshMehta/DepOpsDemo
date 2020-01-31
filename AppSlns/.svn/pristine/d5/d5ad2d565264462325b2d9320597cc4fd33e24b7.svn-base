#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXExceptionHander.cs
// Purpose:   SysX Exception Hander
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web;
using System.Collections.Specialized;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

#endregion

#region Application Specific

using CoreWeb.IntsofLoggerModel.Interface;
using INTSOF.Utils;
using INTSOF.Contracts;

#endregion

#endregion

namespace CoreWeb.IntsofExceptionModel
{
    [ConfigurationElementType(typeof(CustomHandlerData))]
    public class SysXExceptionHander : IExceptionHandler
    {
        #region Variables

        #region Public Variables


        #endregion

        #region Private Variables

        ISysXLoggerService _loggerService;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="SysXExceptionHander"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <remarks></remarks>
        public SysXExceptionHander(NameValueCollection config)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public SysXExceptionHander()
        {
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="handlingInstanceId">The handling instance id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            LoggerService.GetLogger().Error(exception.Message, exception);
            return exception;
        }

        /// <summary>
        /// Gets or sets the logger service.
        /// </summary>
        /// <value>The logger service.</value>
        /// <remarks></remarks>
        public ISysXLoggerService LoggerService
        {

            get
            {
                if (_loggerService.IsNull())
                {
                    if (HttpContext.Current.IsNotNull())
                    {

                        if (HttpContext.Current.ApplicationInstance is IWebApplication)
                        {
                            _loggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                        }
                        else
                        {
                            _loggerService = (HttpContext.Current.ApplicationInstance as INTSOF.ServiceModelInterface.IIntsofService).LoggerService;
                        }

                    }
                    else if (INTSOF.ServiceUtil.ParallelTaskContext.Current.IsNotNull())
                    {
                        _loggerService = INTSOF.ServiceUtil.ParallelTaskContext.LoggerService();
                    }
                }

                return _loggerService;
            }
            set
            {
                _loggerService = value;
            }
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}