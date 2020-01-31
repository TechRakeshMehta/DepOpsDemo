#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXExceptionMessageFormatter.cs
// Purpose:   ExceptionService class
//

#endregion

#region Namespace
using CoreWeb.IntsofExceptionModel.Interface;
using INTSOF.Utils;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Threading;
#endregion

namespace CoreWeb.IntsofExceptionModel.Services
{
    /// <summary>
    /// SysX ExceptionService class
    /// </summary>
    public class SysXExceptionService : ISysXExceptionService
    {
        private SysXExceptionHander _exHandlers;
        private ISysXExceptionMessageFormatter _sysxExceptionMessageFormatter = SysXExceptionMessageFormatter.ExceptionMessageFormatter;

        /// <summary>
        /// Constructor
        /// </summary>
        public SysXExceptionService()
        {
            _exHandlers = new SysXExceptionHander(new NameValueCollection());
        }

        public IExceptionHandler GetExceptionHandler()
        {
            return _exHandlers;
        }

        #region ISysXExceptionService Members

        /// <summary>
        /// Writes Error Message
        /// </summary>
        /// <param name="errorMessage">Error Message</param>
        public void HandleError(String errorMessage)
        {
            _exHandlers.LoggerService.GetLogger().Error(errorMessage);
        }

        /// <summary>
        /// Writes the Exception Message 
        /// </summary>
        /// <param name="errorMessage">Error Message</param>
        /// <param name="ex">Exception</param>
        public void HandleError(String errorMessage, Exception ex)
        {
            if (ex.GetType() != typeof(ThreadAbortException))
            {
                _exHandlers.LoggerService.GetLogger().Error(FormatErrorMessage(errorMessage, ex));
                INTSOF.Utils.SysXEmailService.SendExceptionMails(ex);
            }
        }
        /// <summary>
        /// Write  the Debug message
        /// </summary>
        /// <param name="message">Debug message </param>
        public void HandleDebug(string message)
        {
            _exHandlers.LoggerService.GetLogger().Debug(message);
        }
        /// <summary>
        /// Format the Exception
        /// </summary>
        /// <param name="errorMessage">error message</param>
        /// <param name="ex">Exception</param>
        /// <returns>StringBuilder</returns>
        public StringBuilder FormatErrorMessage(String errorMessage, Exception ex)
        {
            if (!errorMessage.IsNullOrEmpty())
            {
                return _sysxExceptionMessageFormatter.Format(errorMessage, ex);
            }
            else
            {
                return _sysxExceptionMessageFormatter.Format(ex);
            }
        }

        #endregion


       
    }
}
