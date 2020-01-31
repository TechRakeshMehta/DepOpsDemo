using INTSOF.Logger.consts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NLog;

namespace INTSOF.Logger
{
    class LogManager : ILogger
    {
        #region Variables

        #region Public variable

        #endregion

        #region Private variable

        private static NLog.Logger DBlogger;
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

        private Boolean IsFileLogOn { get; set; }

        private Boolean IsDBLogOn { get; set; }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Initialize the logger
        /// </summary>
        /// <param name="logName"></param>
        /// <param name="logLevel"></param>
        public LogManager(String logName, String logLevel)
        {
            InitializeLogData(logName, logLevel);
            if (IsFileLogOn)
            {
                InitializeFileLogger();
            }
            if (IsDBLogOn)
            {
                InitializeDBLogger();
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        #region ILogger Methods

        /// <summary>
        /// Writes the Debug Message in Log.
        /// </summary>
        /// <param name="message">Debug Message</param>
        public void Debug(Object message)
        {
            if (IsDebugEnabled && IsFileLogOn)
            {
                WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_DEBUG, message.ToString(), null);
            }
            if (IsDBLogOn)
            {
                LogInDatabase(SysXLoggerConst.EVENT_LOG_LEVEL_DEBUG, message.ToString(), null);
            }
        }

        /// <summary>
        /// Writes the Debug Message
        /// </summary>
        /// <param name="debugMesage">Debug Message</param>
        /// <param name="exception"></param>
        public void Debug(Object debugMesage, Exception exception)
        {
            if (IsFileLogOn)
            {
                WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_DEBUG, debugMesage.ToString(), exception);
            }
            if (IsDBLogOn)
            {
                LogInDatabase(SysXLoggerConst.EVENT_LOG_LEVEL_DEBUG, debugMesage.ToString(), exception);
            }
        }

        /// <summary>
        /// Writes the Information Log
        /// </summary>
        /// <param name="message">Information</param>
        public void Info(Object message)
        {
            if (IsInfoEnabled && IsFileLogOn)
            {
                WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_INFO, message.ToString(), null);
            }
            if (IsDBLogOn)
            {
                LogInDatabase(SysXLoggerConst.EVENT_LOG_LEVEL_INFO, message.ToString(), null);
            }
        }

        /// <summary>
        /// Writing the Information Message
        /// </summary>
        /// <param name="information">Information Message</param>
        /// <param name="exception">Exception</param>
        public void Info(Object information, Exception exception)
        {
            if (IsFileLogOn)
            {
                WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_INFO, information.ToString(), exception);
            }
            if (IsDBLogOn)
            {
                LogInDatabase(SysXLoggerConst.EVENT_LOG_LEVEL_INFO, information.ToString(), exception);
            }
        }

        /// <summary>
        /// Writing Warning Text
        /// </summary>
        /// <param name="message">Warning Message</param>
        public void Warn(Object message)
        {
            if (IsWarnEnabled && IsFileLogOn)
            {
                WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_WARN, message.ToString(), null);
            }
            if (IsDBLogOn)
                LogInDatabase(SysXLoggerConst.EVENT_LOG_LEVEL_WARN, message.ToString(), null);
        }

        /// <summary>
        /// Writing Warning Message
        /// </summary>
        /// <param name="text">Warning Message</param>
        /// <param name="exception">Exception</param>
        public void Warn(Object text, Exception exception)
        {
            if (IsFileLogOn)
            {
                Warn(text + SysXLoggerConst.DETAILS + exception.Message + SysXLoggerConst.STACK_TRACE + exception.StackTrace);
            }
            if (IsDBLogOn)
            {
                LogInDatabase(SysXLoggerConst.EVENT_LOG_LEVEL_WARN, text.ToString(), exception);
            }
        }

        /// <summary>
        /// Writing the Fault Message
        /// </summary>
        /// <param name="fatalMessage">FatalMessage</param>
        public void Fatal(Object fatalMessage)
        {
            if (IsFatalEnabled && IsFileLogOn)
            {
                WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_FATAL, fatalMessage.ToString(), null);
            }
            if (IsDBLogOn)
            {
                LogInDatabase(SysXLoggerConst.EVENT_LOG_LEVEL_FATAL, fatalMessage.ToString(), null);
            }
        }

        /// <summary>
        /// Writes the Fatal Message
        /// </summary>
        /// <param name="message">Fatal Message</param>
        /// <param name="exception">Exception</param>
        public void Fatal(Object message, Exception exception)
        {
            if (IsFileLogOn)
            {
                WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_FATAL, message.ToString(), exception);
            }
            if (IsDBLogOn)
            {
                LogInDatabase(SysXLoggerConst.EVENT_LOG_LEVEL_FATAL, message.ToString(), exception);
            }
        }

        /// <summary>
        /// Used for Writing the Exception Message
        /// </summary>
        /// <param name="errorMessage">Exception Message</param>
        public void Error(Object errorMessage)
        {
            if (IsErrorEnabled && IsFileLogOn)
            {
                WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_ERROR, errorMessage.ToString(), null);
            }
            if (IsDBLogOn)
            {
                LogInDatabase(SysXLoggerConst.EVENT_LOG_LEVEL_ERROR, errorMessage.ToString(), null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="ex"></param>
        public void ErrorLogInDB(Object errorMessage, Exception ex)
        {
            if (IsDBLogOn)
            {
                LogInDatabase(SysXLoggerConst.EVENT_LOG_LEVEL_ERROR, errorMessage.ToString(), ex);
            }
        }

        /// <summary>
        /// Used for showing Error Exception Message
        /// </summary>
        /// <param name="message">Exception Message</param>
        /// <param name="exception"></param>
        public void Error(Object message, Exception exception)
        {
            if (IsFileLogOn)
            {
                WriteToFile(SysXLoggerConst.EVENT_LOG_LEVEL_ERROR, message.ToString(), exception);
            }
            if (IsDBLogOn)
            {
                LogInDatabase(SysXLoggerConst.EVENT_LOG_LEVEL_ERROR, message.ToString(), exception);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize the bool variable
        /// </summary>
        /// <param name="logName">Log Name</param>
        /// <param name="logLevel">Log Level</param>
        private void InitializeLogData(String logName, String logLevel)
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

            String LoggingType = ConfigurationManager.AppSettings[SysXLoggerConst.LOGGER_TYPE];

            IsFileLogOn = false;
            IsDBLogOn = false;

            switch (LoggingType)
            {
                case SysXLoggerConst.FILE_LOGGER:
                    IsFileLogOn = true;
                    break;
                case SysXLoggerConst.DB_LOGGER:
                    IsDBLogOn = true;
                    break;
                case SysXLoggerConst.FILE_LOGGER_AND_DB_LOGGER:
                    IsFileLogOn = true;
                    IsDBLogOn = true;
                    break;
                default:
                    IsFileLogOn = false;
                    IsDBLogOn = false;
                    break;
            }
        }

        /// <summary>
        /// Initialize File Logger
        /// </summary>
        private void InitializeFileLogger()
        {
            String folderPath = ConfigurationManager.AppSettings[SysXLoggerConst.LOG_FOLDER_PATH];

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            String fileFullPath = folderPath + SysXLoggerConst.LOGGER_INITIALS + DateTime.Now.ToString(SysXLoggerConst.LOGGER_DATE_FORMAT) + SysXLoggerConst.LOGGER_EXTENSION;

            _streamWriter = new StreamWriter(fileFullPath, true);
            _streamWriter.AutoFlush = true;
        }

        /// <summary>
        /// Initialize Nlog
        /// </summary>
        private void InitializeDBLogger()
        {
            DBlogger = NLog.LogManager.GetLogger("DBLogger");
        }

        /// <summary>
        /// Writing in flat files
        /// </summary>
        /// <param name="logType">value for log type</param>
        /// <param name="message">Message to Write in flat files</param>
        /// <param name="ex">ex</param> 
        private void WriteToFile(String logType, String message, Exception ex)
        {
            try
            {
                var context = HttpContext.Current;
                String sessionId = String.Empty;
                String requestId = String.Empty;
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

        /// <summary>
        /// Save the log in log database.
        /// </summary>
        /// <param name="logType">value for log type</param>
        /// <param name="message">Message to Write in flat files</param>
        /// <param name="ex">ex</param> 
        private void LogInDatabase(String logType, String message, Exception ex)
        {
            var context = HttpContext.Current;
            String sessionId = String.Empty;
            String requestId = String.Empty;
            String clientIP = String.Empty;
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
                if (context.Request != null)
                {
                    clientIP = context.Request.UserHostAddress;
                }
            }

            //Setting Custom Attribute to save content in database
            GlobalDiagnosticsContext.Set("requestid", requestId);
            GlobalDiagnosticsContext.Set("clientip", clientIP);
            GlobalDiagnosticsContext.Set("sessionid", sessionId);

            switch (logType)
            {
                case SysXLoggerConst.EVENT_LOG_LEVEL_ALL:
                    DBlogger.DebugException(message, ex);
                    DBlogger.Info(message, ex);
                    DBlogger.Warn(message, ex);
                    DBlogger.ErrorException(message, ex);
                    DBlogger.Fatal(message, ex);
                    break;
                case SysXLoggerConst.EVENT_LOG_LEVEL_DEBUG:
                    DBlogger.DebugException(message, ex);
                    break;
                case SysXLoggerConst.EVENT_LOG_LEVEL_INFO:
                    DBlogger.Info(message, ex);
                    break;
                case SysXLoggerConst.EVENT_LOG_LEVEL_WARN:
                    DBlogger.Warn(message, ex);
                    break;
                case SysXLoggerConst.EVENT_LOG_LEVEL_ERROR:
                    DBlogger.ErrorException(message, ex);
                    break;
                case SysXLoggerConst.EVENT_LOG_LEVEL_FATAL:
                    DBlogger.Fatal(message, ex);
                    break;
                case SysXLoggerConst.EVENT_LOG_LEVEL_OFF:
                    break;
                default:
                    DBlogger.ErrorException(message, ex);
                    DBlogger.Fatal(message, ex);
                    break;
            }
        }

        #endregion

        #endregion
    }
}
