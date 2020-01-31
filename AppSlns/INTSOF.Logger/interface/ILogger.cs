#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ILogger.cs
// Purpose:   ILogger interface
//

#endregion

#region Namespaces

#region System Defined

using System;


#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Logger
{
    /// <summary>
    /// This interface has the initialization of variables, methods, properties for logger service.
    /// </summary>
    public interface ILogger
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is debug enabled.
        /// </summary>
        /// <remarks></remarks>
        Boolean IsDebugEnabled
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is error enabled.
        /// </summary>
        /// <remarks></remarks>
        Boolean IsErrorEnabled
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is fatal enabled.
        /// </summary>
        /// <remarks></remarks>
        Boolean IsFatalEnabled
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is info enabled.
        /// </summary>
        /// <remarks></remarks>
        Boolean IsInfoEnabled
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is warn enabled.
        /// </summary>
        /// <remarks></remarks>
        Boolean IsWarnEnabled
        {
            get;
        }

     
        #endregion

        #region Events

        #endregion

        #region Methods

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        void Debug(object message);

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <remarks></remarks>
        void Debug(object message, Exception exception);

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        void Error(object message);

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <remarks></remarks>
        void Error(object message, Exception exception);

        /// <summary>
        /// Fatal's the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        void Fatal(object message);

        /// <summary>
        /// Fatal's the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <remarks></remarks>
        void Fatal(object message, Exception exception);

        /// <summary>
        /// Information of the  the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        void Info(object message);

        /// <summary>
        /// Information of the  the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <remarks></remarks>
        void Info(object message, Exception exception);

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        void Warn(object message);

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <remarks></remarks>
        void Warn(object message, Exception exception);


        #endregion
    }
}