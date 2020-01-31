#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  EventLogger.cs
// Purpose:   EventLogger class
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Diagnostics;
using System.Configuration;
#endregion

#region Application Specific

using INTSOF.Logger.consts;


#endregion

#endregion

namespace INTSOF.Logger
{
    /// <summary>
    /// Handles the functionality of event logger.
    /// </summary>
    internal class EventLogger : ILogger
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether this instance is debug enabled.
        /// </summary>
        /// <remarks></remarks>
        public Boolean IsDebugEnabled
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is info enabled.
        /// </summary>
        /// <remarks></remarks>
        public Boolean IsInfoEnabled
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is warn enabled.
        /// </summary>
        /// <remarks></remarks>
        public Boolean IsWarnEnabled
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is fatal enabled.
        /// </summary>
        /// <remarks></remarks>
        public Boolean IsFatalEnabled
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is error enabled.
        /// </summary>
        /// <remarks></remarks>
        public Boolean IsErrorEnabled
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the logger.
        /// </summary>
        /// <remarks></remarks>
        public String LoggerName
        {
            get;
            private set;
        }


        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogger"/> class.
        /// </summary>
        /// <param name="logName">Name of the log.</param>
        /// <param name="logLevel">The log level.</param>
        /// <remarks></remarks>
        public EventLogger(String logName, String logLevel)
        {
            this.Initialize(logName, logLevel);
            CreateEventLogSource();
        }

        #region ILogger Methods

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public void Debug(Object message)
        {
            if (IsDebugEnabled)
            {
                this.WriteEntry(message, EventLogEntryType.Information);
            }
        }

        /// <summary>
        /// Debugs the specified debug message.
        /// </summary>
        /// <param name="debugMesage">The debug message.</param>
        /// <param name="exception">The exception.</param>
        /// <remarks></remarks>
        public void Debug(Object debugMesage, Exception exception)
        {
            this.Debug(debugMesage + SysXLoggerConst.DETAILS + exception.Message + SysXLoggerConst.STACK_TRACE + exception);
        }

        /// <summary>
        /// Information the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public void Info(Object message)
        {
            if (IsInfoEnabled)
            {
                this.WriteEntry(message, EventLogEntryType.Information);
            }
        }

        /// <summary>
        /// Information the specified information.
        /// </summary>
        /// <param name="information">The information.</param>
        /// <param name="exception">The exception.</param>
        /// <remarks></remarks>
        public void Info(Object information, Exception exception)
        {
            this.Info(information + SysXLoggerConst.DETAILS + exception.Message + SysXLoggerConst.STACK_TRACE + exception.StackTrace);
        }

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public void Warn(Object message)
        {
            if (IsWarnEnabled)
            {
                this.WriteEntry(message, EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Warns the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="exception">The exception.</param>
        /// <remarks></remarks>
        public void Warn(Object text, Exception exception)
        {
            this.Warn(text + SysXLoggerConst.DETAILS + exception.Message + SysXLoggerConst.STACK_TRACE + exception.StackTrace);
        }

        /// <summary>
        /// Fatal's the specified fatal message.
        /// </summary>
        /// <param name="fatalMessage">The fatal message.</param>
        /// <remarks></remarks>
        public void Fatal(Object fatalMessage)
        {
            if (IsFatalEnabled)
            {
                this.WriteEntry(fatalMessage, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Fatal's the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <remarks></remarks>
        public void Fatal(Object message, Exception exception)
        {
            this.Fatal(message + SysXLoggerConst.DETAILS + exception.Message + SysXLoggerConst.STACK_TRACE + exception.StackTrace);
        }

        /// <summary>
        /// Errors the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <remarks></remarks>
        public void Error(Object errorMessage)
        {
            if (IsErrorEnabled)
            {
                this.WriteEntry(errorMessage.ToString(), EventLogEntryType.Error);
            }

        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <remarks></remarks>
        public void Error(Object message, Exception exception)
        {
            Error(message + SysXLoggerConst.DETAILS + exception.Message + SysXLoggerConst.STACK_TRACE + exception.StackTrace + SysXLoggerConst.EXCEPTION_DETAILS + exception.ToString());

        }

        #endregion

        #endregion

        #region Private Methods

        private void Initialize(String logName, String logLevel)
        {
            this.LoggerName = logName;

            IsDebugEnabled = IsInfoEnabled = IsWarnEnabled = IsErrorEnabled = IsFatalEnabled = false;
            switch (logLevel)
            {
                case SysXLoggerConst.EVENT_LOG_LEVEL_ALL:
                    IsDebugEnabled = IsInfoEnabled = IsWarnEnabled = IsErrorEnabled = IsFatalEnabled = true;
                    break;
                case SysXLoggerConst.EVENT_LOG_LEVEL_DEBUG:
                    IsDebugEnabled = IsInfoEnabled = IsWarnEnabled = IsErrorEnabled = IsFatalEnabled = true;
                    break;
                case SysXLoggerConst.EVENT_LOG_LEVEL_INFO:
                    IsInfoEnabled = IsWarnEnabled = IsErrorEnabled = IsFatalEnabled = true;
                    break;
                case SysXLoggerConst.EVENT_LOG_LEVEL_WARN:
                    IsWarnEnabled = IsErrorEnabled = IsFatalEnabled = true;
                    break;
                case SysXLoggerConst.EVENT_LOG_LEVEL_ERROR:
                    IsErrorEnabled = IsFatalEnabled = true;
                    break;
                case SysXLoggerConst.EVENT_LOG_LEVEL_FATAL:
                    IsFatalEnabled = true;
                    break;
                case SysXLoggerConst.EVENT_LOG_LEVEL_OFF:
                    break;
                default:
                    IsErrorEnabled = IsFatalEnabled = true;
                    break;
            }




        }

        private void WriteEntry(Object message, EventLogEntryType eventType)
        {
            try
            {
                EventLog.WriteEntry(SysXLoggerConst.EVENT_SOURCE_NAME, message.ToString(), eventType);
            }
            catch (Exception)
            {
                
                //catch the error if event logger can not be used for lack of rights
            }
            
        }

        private void CreateEventLogSource()
        {
            try
            {

                if (!EventLog.SourceExists(SysXLoggerConst.EVENT_SOURCE_NAME))
                {
                    EventLog.CreateEventSource(SysXLoggerConst.EVENT_SOURCE_NAME, SysXLoggerConst.APPLICATION);
                }

            }
            catch (Exception)
            {
                
                //catch the error if event logger can not be used for lack of rights
            }
        }

        #endregion

        #endregion
    }
}