#region Header Comment Block
// 
// Copyright LPS, Inc.  2008
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  EventLogger.cs
// Purpose:   
//
// Revisions:
// Author       Date            Comment
// ------       ----------      -------------------------------------------------
// 
#endregion

#region Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LPSDesktop.Logging;
using System.Diagnostics;
using LPSDesktop.Logging.Interfaces;
using System.Configuration;
using LPSFS.Logger.consts;
using System.Configuration;

#endregion

namespace LPSFS.SYSX.WEB.SysXLoggerModel
{
    public class EventLogger : ILogger
    {
        #region Variables
        public Boolean IsDebugEnabled { get; private set; }
        public Boolean IsInfoEnabled { get; private set; }
        public Boolean IsWarnEnabled { get; private set; }
        public Boolean IsFatalEnabled { get; private set; }
        public Boolean IsErrorEnabled { get; private set; }
        public String LoggerName { get; private set; }
        #endregion

        #region Class Construction

        public EventLogger()
        {
            this.Initialize("Empty", LPSFS.Utils.Consts.SysXLoggerConst.DEFAULT_LOG_LEVEL);
        }


        /// <summary>
        /// Event Logger constructor 
        /// </summary>
        /// <param name="logName">Log Name</param>
        public EventLogger(String logName)
        {
            this.Initialize(logName, LPSFS.Utils.Consts.SysXLoggerConst.DEFAULT_LOG_LEVEL);
            this.CreateEventLogSource();
        }

        /// <summary>
        /// Parametrised Event Logger
        /// </summary>
        /// <param name="LogName">Log Name</param>
        /// <param name="logLevel">Log Level</param>
        public EventLogger(String LogName, String logLevel)
        {
            this.Initialize(LogName, logLevel);
            CreateEventLogSource();
        }

        /// <summary>
        /// Initialize the bool variabled 
        /// </summary>
        /// <param name="LogName">Log Name</param>
        /// <param name="LogLevel">Log Level</param>
        private void Initialize(String LogName, String LogLevel)
        {
            this.LoggerName = LogName;

            IsDebugEnabled = IsInfoEnabled = IsWarnEnabled = IsErrorEnabled = IsFatalEnabled = false;
            switch (LogLevel)
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
        #endregion

        #region ILogger Methods

        /// <summary>
        /// Writes the Debug Message in Log.
        /// </summary>
        /// <param name="debugMesage">Debug Mesage</param>
        public void Debug(Object debugMesage)
        {
            if (IsDebugEnabled)
            {
                this.WriteEntry(debugMesage, EventLogEntryType.Information);
            }
        }

        /// <summary>
        /// Writes the Debug Message
        /// </summary>
        /// <param name="debugMesage">Debug Message</param>
        /// <param name="ex"></param>
        public void Debug(Object debugMesage, Exception ex)
        {
            this.Debug(debugMesage + "\r\nDetails: " + ex.Message + "\r\nStackTrace: " + ex);
        }

        /// <summary>
        /// Writes the Information Log
        /// </summary>
        /// <param name="information">Information</param>
        public void Info(Object information)
        {
            if (IsInfoEnabled)
            {
                this.WriteEntry(information, EventLogEntryType.Information);
            }
        }

        /// <summary>
        /// Writing the Information Message
        /// </summary>
        /// <param name="information">Information Message</param>
        /// <param name="exception">Exception</param>
        public void Info(Object information, Exception exception)
        {
            this.Info(information + "\r\nDetails: " + exception.Message + "\r\nStackTrace: " + exception.StackTrace);
        }

        /// <summary>
        /// Writing Worning Text
        /// </summary>
        /// <param name="warning">Worning Message</param>
        public void Warn(Object warning)
        {
            if (IsWarnEnabled)
            {
                this.WriteEntry(warning, EventLogEntryType.Warning);
            }
        }

        /// <summary>
        /// Writing Worning Message
        /// </summary>
        /// <param name="text">Worning Message</param>
        /// <param name="ex">Exception</param>
        public void Warn(Object text, Exception ex)
        {
            this.Warn(text + "\r\nDetails: " + ex.Message + "\r\nStackTrace: " + ex.StackTrace);
        }

        /// <summary>
        /// Writing the Fault Message
        /// </summary>
        /// <param name="fatalMessage">FatalMessage</param>
        public void Fatal(Object fatalMessage)
        {
            if (IsFatalEnabled)
            {
                this.WriteEntry(fatalMessage, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Writes the Fatal Message
        /// </summary>
        /// <param name="fatalMessage">Fatal Message</param>
        /// <param name="ex">Exception</param>
        public void Fatal(Object fatalMessage, Exception ex)
        {
            this.Fatal(fatalMessage + "\r\nDetails: " + ex.Message + "\r\nStackTrace: " + ex.StackTrace);
        }

        /// <summary>
        /// Used for Writing the Exception Message
        /// </summary>
        /// <param name="errorMessage">Exception Message</param>
        public void Error(Object errorMessage)
        {
            if (IsErrorEnabled)
            {
                this.WriteEntry(errorMessage.ToString(), EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Used for showing Error Exception Message
        /// </summary>
        /// <param name="text">Exception Message</param>
        /// <param name="errorMessage">Exception Message</param>
        public void Error(Object text, Exception ex)
        {
            Error(text + "\r\nDetails: " + ex.Message + "\r\nStackTrace: " + ex.StackTrace);
        }
        #endregion

        #region Private Methods

        private void WriteEntry(Object message, EventLogEntryType eventType)
        {
            string isEventLog = ConfigurationManager.AppSettings["IsEventLog"].ToLower();
            if (isEventLog == "true")
            {
                EventLog.WriteEntry(SysXLoggerConst.EVENT_SOURCE_NAME, message.ToString(), eventType);
            }
        }

        private void CreateEventLogSource()
        {
        }
        #endregion
    }
}
