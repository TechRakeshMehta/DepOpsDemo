﻿#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  FileLogger.cs
// Purpose:   FileLogger class
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.IO;
using System.Configuration;

#endregion

#region Application Specific

using INTSOF.Logger.consts;
using System.Web;


#endregion

#endregion

namespace INTSOF.Logger
{
    /// <summary>
    /// This class handles the logger Services.
    /// </summary>
    class FileLogger : ILogger, IDisposable
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static Object _lockObject = new Object();
        StreamWriter _streamWriter;

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
        /// Flat File Logger constructor 
        /// </summary>
        /// <param name="logName">Log Name</param>
        /// <param name="logLevel"></param>
        public FileLogger(String logName, String logLevel)
        {
            Initialize(logName, logLevel);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            _streamWriter.Flush();
            _streamWriter.Close();
        }

        /// <summary>
        /// Writing in flat files
        /// </summary>
        /// <param name="logType">value for log type</param>
        /// <param name="message">Message to Write in flat files</param>
        /// <param name="ex">ex</param> 
        public void WriteToFile(String logType, String message, Exception ex)
        {
            try
            {
                var context = HttpContext.Current;
                String sessionId = String.Empty;
                String requestId =String.Empty;
                if (context != null)
                {
                    if (context.Session != null)
                    {
                        sessionId = HttpContext.Current.Session.SessionID;
                    }
                    if (context.Items.Contains(SysXLoggerConst.REQUESTID))
                    {
                        requestId = Convert.ToString(context.Items[SysXLoggerConst.REQUESTID]);
                    }
                }
                lock (_lockObject)
                {
                    if (ex != null)
                    {
                        _streamWriter.WriteLine(logType + SysXLoggerConst.PIPE_SYMBOL + DateTime.Now.ToString(SysXLoggerConst.DATETIME_G) + SysXLoggerConst.PIPE_SYMBOL + SysXLoggerConst.SESSION_ID + sessionId + SysXLoggerConst.PIPE_SYMBOL + SysXLoggerConst.REQUEST_ID + requestId + SysXLoggerConst.PIPE_SYMBOL + message + Environment.NewLine + ex);

                    }
                    else
                    {
                        _streamWriter.WriteLine(logType + SysXLoggerConst.PIPE_SYMBOL + DateTime.Now.ToString(SysXLoggerConst.DATETIME_G) + SysXLoggerConst.PIPE_SYMBOL + SysXLoggerConst.SESSION_ID + sessionId + SysXLoggerConst.PIPE_SYMBOL + SysXLoggerConst.REQUEST_ID + requestId + SysXLoggerConst.PIPE_SYMBOL + message);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #region ILogger Methods

        /// <summary>
        /// Writes the Debug Message in Log.
        /// </summary>
        /// <param name="message">Debug Message</param>
        public void Debug(Object message)
        {
            if (IsDebugEnabled)
            {
                WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_DEBUG, message.ToString(), null);
            }
        }

        /// <summary>
        /// Writes the Debug Message
        /// </summary>
        /// <param name="debugMesage">Debug Message</param>
        /// <param name="exception"></param>
        public void Debug(Object debugMesage, Exception exception)
        {
            WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_DEBUG, debugMesage.ToString(), exception);
        }

        /// <summary>
        /// Writes the Information Log
        /// </summary>
        /// <param name="message">Information</param>
        public void Info(Object message)
        {
            if (IsInfoEnabled)
            {
                WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_INFO, message.ToString(), null);
            }
        }

        /// <summary>
        /// Writing the Information Message
        /// </summary>
        /// <param name="information">Information Message</param>
        /// <param name="exception">Exception</param>
        public void Info(Object information, Exception exception)
        {
            WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_INFO, information.ToString(), exception);
        }

        /// <summary>
        /// Writing Warning Text
        /// </summary>
        /// <param name="message">Warning Message</param>
        public void Warn(Object message)
        {
            if (IsWarnEnabled)
            {
                WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_WARN, message.ToString(), null);
            }
        }

        /// <summary>
        /// Writing Warning Message
        /// </summary>
        /// <param name="text">Warning Message</param>
        /// <param name="exception">Exception</param>
        public void Warn(Object text, Exception exception)
        {
            Warn(text + SysXLoggerConst.DETAILS + exception.Message + SysXLoggerConst.STACK_TRACE + exception.StackTrace);
        }

        /// <summary>
        /// Writing the Fault Message
        /// </summary>
        /// <param name="fatalMessage">FatalMessage</param>
        public void Fatal(Object fatalMessage)
        {
            if (IsFatalEnabled)
            {
                WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_FATAL, fatalMessage.ToString(), null);
            }
        }

        /// <summary>
        /// Writes the Fatal Message
        /// </summary>
        /// <param name="message">Fatal Message</param>
        /// <param name="exception">Exception</param>
        public void Fatal(Object message, Exception exception)
        {
            WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_FATAL, message.ToString(), exception);
        }

        /// <summary>
        /// Used for Writing the Exception Message
        /// </summary>
        /// <param name="errorMessage">Exception Message</param>
        public void Error(Object errorMessage)
        {
            if (IsErrorEnabled)
            {
                WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_ERROR, errorMessage.ToString(), null);
            }

        }

        /// <summary>
        /// Used for showing Error Exception Message
        /// </summary>
        /// <param name="message">Exception Message</param>
        /// <param name="exception"></param>
        public void Error(Object message, Exception exception)
        {
            WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_ERROR, message.ToString(), exception);
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize the bool variable
        /// </summary>
        /// <param name="logName">Log Name</param>
        /// <param name="logLevel">Log Level</param>
        private void Initialize(String logName, String logLevel)
        {
            LoggerName = logName;

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

            //TODO Initialize File.
            String folderPath = ConfigurationManager.AppSettings[SysXLoggerConst.LOG_FOLDER_PATH];

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            String fileFullPath = folderPath + SysXLoggerConst.LOGGER_INITIALS + DateTime.Now.ToString(SysXLoggerConst.LOGGER_DATE_FORMAT) + SysXLoggerConst.LOGGER_EXTENSION;

            _streamWriter = new StreamWriter(fileFullPath, true);
            _streamWriter.AutoFlush = true;

        }

        #endregion

        #endregion
    }
}