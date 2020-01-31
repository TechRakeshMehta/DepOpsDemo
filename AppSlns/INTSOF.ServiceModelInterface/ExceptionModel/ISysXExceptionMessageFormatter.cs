#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ISysXExceptionMessageFormatter.cs
// Purpose:   ISysXExceptionMessageFormatter Interface
//

#endregion

#region Namespace

#region System Defined

using System;
using System.Text;

#endregion

#region Application Specific

#endregion

#endregion

namespace CoreWeb.IntsofExceptionModel
{
    /// <summary>
    /// ISysXExceptionMessageFormatter Interface
    /// </summary>
    /// <remarks></remarks>
    public interface ISysXExceptionMessageFormatter
    {
        #region Variables

        #endregion

        #region Properties

        #region Properties Used for Exception Message formating

        #region ApplicationInformation

        /// <summary>
        /// Gets or sets the application domain.
        /// </summary>
        /// <value>The application domain.</value>
        /// <remarks></remarks>
        String ApplicationDomain { get; set; }

        /// <summary>
        /// Gets or sets the trust level.
        /// </summary>
        /// <value>The trust level.</value>
        /// <remarks></remarks>
        String TrustLevel { get; set; }

        /// <summary>
        /// Gets or sets the application virtual path.
        /// </summary>
        /// <value>The application virtual path.</value>
        /// <remarks></remarks>
        String ApplicationVirtualPath { get; set; }

        /// <summary>
        /// Gets or sets the appliaction path.
        /// </summary>
        /// <value>The appliaction path.</value>
        /// <remarks></remarks>
        String AppliactionPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the machine.
        /// </summary>
        /// <value>The name of the machine.</value>
        /// <remarks></remarks>
        String MachineName { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Gets or sets the event code.
        /// </summary>
        /// <value>The event code.</value>
        /// <remarks></remarks>
        String EventCode { get; set; }

        /// <summary>
        /// Gets or sets the event message.
        /// </summary>
        /// <value>The event message.</value>
        /// <remarks></remarks>
        String EventMessage { get; set; }

        /// <summary>
        /// Gets or sets the event time.
        /// </summary>
        /// <value>The event time.</value>
        /// <remarks></remarks>
        String EventTime { get; set; }

        /// <summary>
        /// Gets or sets the event sequence.
        /// </summary>
        /// <value>The event sequence.</value>
        /// <remarks></remarks>
        String EventSequence { get; set; }

        /// <summary>
        /// Gets or sets the event occurrence.
        /// </summary>
        /// <value>The event occurrence.</value>
        /// <remarks></remarks>
        String EventOccurrence { get; set; }

        /// <summary>
        /// Gets or sets the event detail code.
        /// </summary>
        /// <value>The event detail code.</value>
        /// <remarks></remarks>
        String EventDetailCode { get; set; }

        #endregion

        #region ProcessInformation


        /// <summary>
        /// Gets or sets the process id.
        /// </summary>
        /// <value>The process id.</value>
        /// <remarks></remarks>
        String ProcessId { get; set; }

        /// <summary>
        /// Gets or sets the name of the process.
        /// </summary>
        /// <value>The name of the process.</value>
        /// <remarks></remarks>
        String ProcessName { get; set; }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        /// <value>The name of the account.</value>
        /// <remarks></remarks>
        String AccountName { get; set; }

        #endregion

        #region ExceptionInformation


        /// <summary>
        /// Gets or sets the type of the exception.
        /// </summary>
        /// <value>The type of the exception.</value>
        /// <remarks></remarks>
        String ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets the exception message.
        /// </summary>
        /// <value>The exception message.</value>
        /// <remarks></remarks>
        String ExceptionMessage { get; set; }

        /// <summary>
        /// Gets or sets the severity.
        /// </summary>
        /// <value>The severity.</value>
        /// <remarks></remarks>
        String Severity { get; set; }

        #endregion

        #region RequestInformation


        /// <summary>
        /// Gets or sets the request URL.
        /// </summary>
        /// <value>The request URL.</value>
        /// <remarks></remarks>
        String RequestUrl { get; set; }

        /// <summary>
        /// Gets or sets the request path.
        /// </summary>
        /// <value>The request path.</value>
        /// <remarks></remarks>
        String RequestPath { get; set; }

        /// <summary>
        /// Gets or sets the user host address.
        /// </summary>
        /// <value>The user host address.</value>
        /// <remarks></remarks>
        String UserHostAddress { get; set; }

        /// <summary>
        /// Gets or sets the is authenticated.
        /// </summary>
        /// <value>The is authenticated.</value>
        /// <remarks></remarks>
        String IsAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets the type of the authentication.
        /// </summary>
        /// <value>The type of the authentication.</value>
        /// <remarks></remarks>
        String AuthenticationType { get; set; }

        /// <summary>
        /// Gets or sets the name of the thread account.
        /// </summary>
        /// <value>The name of the thread account.</value>
        /// <remarks></remarks>
        String ThreadAccountName { get; set; }

        #endregion

        #region ThreadInformation

        /// <summary>
        /// Gets or sets the thread id.
        /// </summary>
        /// <value>The thread id.</value>
        /// <remarks></remarks>
        String ThreadId { get; set; }

        /// <summary>
        /// Gets or sets the is impersonating.
        /// </summary>
        /// <value>The is impersonating.</value>
        /// <remarks></remarks>
        String IsImpersonating { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        /// <value>The stack trace.</value>
        /// <remarks></remarks>
        String StackTrace { get; set; }

        #endregion

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Methods to format Exception

        /// <summary>
        /// Formats the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        StringBuilder Format(Exception ex);

        /// <summary>
        /// Formats the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        StringBuilder Format(String errorMessage, Exception ex);

        #endregion

        #endregion
    }
}