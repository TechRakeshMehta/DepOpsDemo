#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXExceptionMessageFormatter.cs
// Purpose:   Exception Message Formatter class
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web;
using System.Text;
using System.Diagnostics;
using System.Threading;

#endregion

#region Application Specific

using INTSOF.Utils;
using INTSOF.Utils.Consts;

#endregion

#endregion

namespace CoreWeb.IntsofExceptionModel
{
    /// <summary>
    /// ExceptionMessageFormatter
    /// </summary>
    /// <remarks></remarks>
    public sealed class SysXExceptionMessageFormatter : ISysXExceptionMessageFormatter
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private StringBuilder _strBuilder;

        private static readonly SysXExceptionMessageFormatter _sysXExceptionMessageFormatter = new SysXExceptionMessageFormatter();

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the exception message formatter.
        /// </summary>
        /// <remarks></remarks>
        public static SysXExceptionMessageFormatter ExceptionMessageFormatter
        {
            get
            {
                return _sysXExceptionMessageFormatter;
            }
        }

        #region Properties Used for Exception Message formating

        /// <summary>
        /// Gets or sets the application domain.
        /// </summary>
        /// <value>The application domain.</value>
        /// <remarks></remarks>
        public String ApplicationDomain { get; set; }

        /// <summary>
        /// Gets or sets the trust level.
        /// </summary>
        /// <value>The trust level.</value>
        /// <remarks></remarks>
        public String TrustLevel { get; set; }

        /// <summary>
        /// Gets or sets the application virtual path.
        /// </summary>
        /// <value>The application virtual path.</value>
        /// <remarks></remarks>
        public String ApplicationVirtualPath { get; set; }

        /// <summary>
        /// Gets or sets the appliaction path.
        /// </summary>
        /// <value>The appliaction path.</value>
        /// <remarks></remarks>
        public String AppliactionPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the machine.
        /// </summary>
        /// <value>The name of the machine.</value>
        /// <remarks></remarks>
        public String MachineName { get; set; }

        /// <summary>
        /// Gets or sets the event code.
        /// </summary>
        /// <value>The event code.</value>
        /// <remarks></remarks>
        public String EventCode { get; set; }

        /// <summary>
        /// Gets or sets the event message.
        /// </summary>
        /// <value>The event message.</value>
        /// <remarks></remarks>
        public String EventMessage { get; set; }

        /// <summary>
        /// Gets or sets the event time.
        /// </summary>
        /// <value>The event time.</value>
        /// <remarks></remarks>
        public String EventTime { get; set; }

        /// <summary>
        /// Gets or sets the event sequence.
        /// </summary>
        /// <value>The event sequence.</value>
        /// <remarks></remarks>
        public String EventSequence { get; set; }

        /// <summary>
        /// Gets or sets the event occurrence.
        /// </summary>
        /// <value>The event occurrence.</value>
        /// <remarks></remarks>
        public String EventOccurrence { get; set; }

        /// <summary>
        /// Gets or sets the event detail code.
        /// </summary>
        /// <value>The event detail code.</value>
        /// <remarks></remarks>
        public String EventDetailCode { get; set; }

        /// <summary>
        /// Gets or sets the process id.
        /// </summary>
        /// <value>The process id.</value>
        /// <remarks></remarks>
        public String ProcessId { get; set; }

        /// <summary>
        /// Gets or sets the name of the process.
        /// </summary>
        /// <value>The name of the process.</value>
        /// <remarks></remarks>
        public String ProcessName { get; set; }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        /// <value>The name of the account.</value>
        /// <remarks></remarks>
        public String AccountName { get; set; }

        /// <summary>
        /// Gets or sets the type of the exception.
        /// </summary>
        /// <value>The type of the exception.</value>
        /// <remarks></remarks>
        public String ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets the exception message.
        /// </summary>
        /// <value>The exception message.</value>
        /// <remarks></remarks>
        public String ExceptionMessage { get; set; }

        /// <summary>
        /// Gets or sets the severity.
        /// </summary>
        /// <value>The severity.</value>
        /// <remarks></remarks>
        public String Severity { get; set; }

        /// <summary>
        /// Gets or sets the request URL.
        /// </summary>
        /// <value>The request URL.</value>
        /// <remarks></remarks>
        public String RequestUrl { get; set; }

        /// <summary>
        /// Gets or sets the request path.
        /// </summary>
        /// <value>The request path.</value>
        /// <remarks></remarks>
        public String RequestPath { get; set; }

        /// <summary>
        /// Gets or sets the user host address.
        /// </summary>
        /// <value>The user host address.</value>
        /// <remarks></remarks>
        public String UserHostAddress { get; set; }

        /// <summary>
        /// Gets or sets the is authenticated.
        /// </summary>
        /// <value>The is authenticated.</value>
        /// <remarks></remarks>
        public String IsAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets the type of the authentication.
        /// </summary>
        /// <value>The type of the authentication.</value>
        /// <remarks></remarks>
        public String AuthenticationType { get; set; }

        /// <summary>
        /// Gets or sets the name of the thread account.
        /// </summary>
        /// <value>The name of the thread account.</value>
        /// <remarks></remarks>
        public String ThreadAccountName { get; set; }

        /// <summary>
        /// Gets or sets the thread id.
        /// </summary>
        /// <value>The thread id.</value>
        /// <remarks></remarks>
        public String ThreadId { get; set; }

        /// <summary>
        /// Gets or sets the is impersonating.
        /// </summary>
        /// <value>The is impersonating.</value>
        /// <remarks></remarks>
        public String IsImpersonating { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        /// <value>The stack trace.</value>
        /// <remarks></remarks>
        public String StackTrace { get; set; }

        #endregion

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Format Exception
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>StringBuilder</returns>
        public StringBuilder Format(Exception ex)
        {
            try
            {
                return FormatMessage(String.Empty, ex);
            }
            catch (Exception)
            {
                //throw;
            }
            return _strBuilder;
        }


        /// <summary>
        /// Formats the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public StringBuilder Format(String message, Exception ex)
        {
            return FormatMessage(message, ex);
        }

        #endregion

        #region Private Methods

        private StringBuilder FormatMessage(String message, Exception ex)
        {
            var context = HttpContext.Current;

            _strBuilder = new StringBuilder();

            #region  Application Information
            try
            {
                _strBuilder.Append(Environment.NewLine + SysXExceptionConsts.APPLICATION_INFORMATION + Environment.NewLine + SysXExceptionConsts.ERROR_FORMATTER + Environment.NewLine);
                if (!context.IsNull())
                {
                    _strBuilder.Append(SysXExceptionConsts.APPLICATION_DOMAIN + SysXExceptionConsts.HTTP_INITIAL + context.Request.ServerVariables[SysXExceptionConsts.HTTP_HOST] + Environment.NewLine);
                }
                _strBuilder.Append(SysXExceptionConsts.TRUST_LEVEL + SysXExceptionConsts.Exception_NA + Environment.NewLine);
                if (!context.IsNull())
                {
                    _strBuilder.Append(SysXExceptionConsts.APPLICATION_VIRTUALPATH + context.Request.ApplicationPath + Environment.NewLine);
                    _strBuilder.Append(SysXExceptionConsts.APPLIACTION_PATH + context.Request.PhysicalApplicationPath + Environment.NewLine);
                    _strBuilder.Append(SysXExceptionConsts.MACHINE_NAME + context.Request.UserHostName + Environment.NewLine + Environment.NewLine);
                }
            }
            catch
            {
                //do nothing
            }
            #endregion

            #region Event Information
            try
            {
                _strBuilder.Append(SysXExceptionConsts.EVENT_INFORMATION + Environment.NewLine + SysXExceptionConsts.ERROR_FORMATTER + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.EVENT_CODE + SysXExceptionConsts.Exception_NA + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.EVENT_MESSAGE + ex.Message + Environment.NewLine);
                if (!context.IsNull())
                {
                    _strBuilder.Append(SysXExceptionConsts.EVENT_TIME + context.Timestamp.ToString() + Environment.NewLine);
                }
                else
                {
                    _strBuilder.Append(SysXExceptionConsts.EVENT_TIME + DateTime.Now.ToString() + Environment.NewLine);
                }
                _strBuilder.Append(SysXExceptionConsts.EVENT_SEQUENCE + SysXExceptionConsts.Exception_NA + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.EVENT_OCCURRENCE + SysXExceptionConsts.Exception_NA + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.EVENT_DETAIL_CODE + SysXExceptionConsts.Exception_NA + Environment.NewLine + Environment.NewLine);
            }
            catch
            {
                //do nothing
            }
            #endregion

            #region Exception Information
            try
            {
                _strBuilder.Append(SysXExceptionConsts.EXCEPTION_INFORMATION + Environment.NewLine + SysXExceptionConsts.ERROR_FORMATTER + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.EXCEPTION_TYPE + ex.GetType().Name + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.EXCEPTION_MESSAGE + ex.Message + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.INNEREXCEPTION_MESSAGE + (ex.InnerException.IsNotNull() ? (ex.InnerException).Message : string.Empty) + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.SEVERITY + SysXExceptionSeverity.GetSeverity(ex.GetType()) + Environment.NewLine);
                StackTrace stackTrace = new StackTrace(ex);
                _strBuilder.Append(SysXExceptionConsts.MODULE_PROJECT + stackTrace.GetFrame(0).GetMethod().Module.ScopeName + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.CLASS_FILE_NAME + stackTrace.GetFrame(0).GetMethod().DeclaringType.FullName + SysXExceptionConsts.DOT_CS + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.CLASS_NAME + stackTrace.GetFrame(0).GetMethod().DeclaringType.Name + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.METHOD_NAME + stackTrace.GetFrame(0).GetMethod().Name + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.LINE_NO + ex.StackTrace.ToLower().Remove(0, ex.StackTrace.IndexOf(SysXExceptionConsts.LINE) + 5) + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.STACK_TRACE + ex.StackTrace + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.SOURCE + ex.Source + Environment.NewLine + Environment.NewLine);
            }
            catch
            {
                //do nothing
            }
            #endregion

            #region  Request Information
            try
            {
                _strBuilder.Append(SysXExceptionConsts.REQUEST_INFORMATION + Environment.NewLine + SysXExceptionConsts.ERROR_FORMATTER + Environment.NewLine);
                if (!context.IsNull())
                {
                    _strBuilder.Append(SysXExceptionConsts.REQUEST_URL + context.Request.Url + Environment.NewLine);
                    _strBuilder.Append(SysXExceptionConsts.REQUEST_PATH + context.Request.FilePath + Environment.NewLine);
                    _strBuilder.Append(SysXExceptionConsts.USERHOST_ADDRESS + context.Request.UserHostName + Environment.NewLine);
                    _strBuilder.Append(SysXExceptionConsts.IS_AUTHENTICATED + context.Request.IsAuthenticated + Environment.NewLine);
                }
                _strBuilder.Append(SysXExceptionConsts.AUTHENTICATION_TYPE + SysXExceptionConsts.Exception_NA + Environment.NewLine + Environment.NewLine);
            }
            catch
            {
                //do nothing
            }
            #endregion

            #region Thread Information
            try
            {
                _strBuilder.Append(SysXExceptionConsts.THREAD_INFORMATION + Environment.NewLine + SysXExceptionConsts.ERROR_FORMATTER + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.THREAD_ID + Thread.CurrentThread.ManagedThreadId + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.THREAD_ACCOUNT_NAME + Thread.CurrentPrincipal.Identity + Environment.NewLine);
                _strBuilder.Append(SysXExceptionConsts.IS_IMPERSONATING + SysXExceptionConsts.Exception_NA + Environment.NewLine);
            }
            catch
            {
                //do nothing
            }
            #endregion

            try
            {
                if (!message.IsNullOrEmpty())
                {
                    _strBuilder.Append(Environment.NewLine + SysXExceptionConsts.CUSTOM_MESSAGE + message);
                }
            }
            catch
            {
                //do nothing
            }
            return _strBuilder;
        }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="SysXExceptionMessageFormatter"/> class from being created.
        /// </summary>
        /// <remarks></remarks>
        private SysXExceptionMessageFormatter()
        {
            // TODO: Used in future
        }

        #endregion
    }
}