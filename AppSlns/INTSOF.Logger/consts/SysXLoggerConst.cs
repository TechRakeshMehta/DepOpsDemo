#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXLoggerConst.cs
// Purpose:   SysXLoggerConst Class
//

#endregion

#region Namespace

#region System Defined

using System;


#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Logger.consts
{
    /// <summary>
    /// Sys X Logger Constants
    /// </summary>
    public class SysXLoggerConst
    {
        #region Constants

        /// <summary>
        /// Constant for SYSX_LOGGER_NAME
        /// </summary>
        public const String SYSX_LOGGER_NAME = "SysXLogger";

        /// <summary>
        /// Constant for EVENT_SOURCE_NAME
        /// </summary>
        public const String EVENT_SOURCE_NAME = "SysXEvent";

        /// <summary>
        /// Constant for LOG_LEVEL_CINFIG_KEY_NAME
        /// </summary>
        public const String LOG_LEVEL_CINFIG_KEY_NAME = "LogLevel";

        /// <summary>
        /// Constant for LOG_CLASS_CONFIG_KEY_NAME
        /// </summary>
        public const String LOG_CLASS_CONFIG_KEY_NAME = "LoggerClassName";

        /// <summary>
        /// Constant for DEFAULT_LOG_LEVEL
        /// </summary>
        public const String DEFAULT_LOG_LEVEL = "WARN";

        #region Flat file Logger Constant

        /// <summary>
        /// Constant for LOGGER_INITIALS
        /// </summary>
        public const String LOGGER_INITIALS = "SysXLog.";

        /// <summary>
        /// Constant for LOGGER_DATE_FORMAT
        /// </summary>
        public const String LOGGER_DATE_FORMAT = "dd-MMM-yyyy";

        /// <summary>
        /// Constant for LOGGER_EXTENSION
        /// </summary>
        public const String LOGGER_EXTENSION = ".txt";

        #endregion

        #region Event Logger Constants

        /// <summary>
        /// Constant for EVENT_LOG_LEVEL_ALL
        /// </summary>
        public const String EVENT_LOG_LEVEL_ALL = "ALL";

        /// <summary>
        /// Constant for EVENT_LOG_LEVEL_DEBUG
        /// </summary>
        public const String EVENT_LOG_LEVEL_DEBUG = "DEBUG";

        /// <summary>
        /// Constant for EVENT_LOG_LEVEL_INFO
        /// </summary>
        public const String EVENT_LOG_LEVEL_INFO = "INFO";

        /// <summary>
        /// Constant for EVENT_LOG_LEVEL_WARN
        /// </summary>
        public const String EVENT_LOG_LEVEL_WARN = "WARN";

        /// <summary>
        /// Constant for EVENT_LOG_LEVEL_ERROR
        /// </summary>
        public const String EVENT_LOG_LEVEL_ERROR = "ERROR";

        /// <summary>
        /// Constant for EVENT_LOG_LEVEL_FATAL
        /// </summary>
        public const String EVENT_LOG_LEVEL_FATAL = "FATAL";

        /// <summary>
        /// Constant for EVENT_LOG_LEVEL_OFF
        /// </summary>
        public const String EVENT_LOG_LEVEL_OFF = "OFF";

        /// <summary>
        /// Unable to load logger
        /// </summary>
        public const String UNABLE_TO_LOAD_LOGGER = "Unable to load logger :";

        /// <summary>
        /// Details const  
        /// </summary>
        public const String DETAILS = "\r\nMessage: ";

        /// <summary>
        /// StackTrace  const
        /// </summary>
        public const String STACK_TRACE =  "\r\nStackTrace: ";

        public const String EXCEPTION_DETAILS = "\r\nDetails: ";

        /// <summary>
        /// APPLICATION const
        /// </summary>
        public const String APPLICATION = "Application";

        /// <summary>
        /// Pipe Symbol
        /// </summary>
        public const String PIPE_SYMBOL = " | ";

        /// <summary>
        /// Used at date time
        /// </summary>
        public const String  DATETIME_G = "G";

        /// <summary>
        /// Log Folder Path 
        /// </summary>
        public const String LOG_FOLDER_PATH = "LogFolderPath";

        /// <summary>
        /// Log Online Payment Folder Path 
        /// </summary>
        public const String LOG_ONLINEPAYMENT_FOLDER_PATH = "LogOnlinePaymentFolderPath";

        /// <summary>
        /// Is Log Online Payment
        /// </summary>
        public const String IS_LOG_ONLINEPAYMENT = "IsLogOnlinePayment";

        /// <summary>
        /// Email Settings - From Address
        /// </summary>
        public const String FROM_ADDRESS = "From_Address";

        /// <summary>
        /// Constant for Zip folder
        /// </summary>
        public const String ZIP_FOLDER= "//Zipfiles";

        public const String SESSION_ID = "SessionId: ";
        public const String REQUEST_ID = "RequestId: ";
        public const String REQUESTID = "RequestId";
        #endregion

        #region Logging Keys
        public const String LOGGER_TYPE = "LoggerType";
        public const String FILE_LOGGER = "FileLogger";
        public const String DB_LOGGER = "DBLogger";
        public const String FILE_LOGGER_AND_DB_LOGGER = "FileLoggerAndDBLogger";
        #endregion

        #endregion
    }
}