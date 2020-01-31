#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXLoggerFactory.cs
// Purpose:   SysX Logger Factory class
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Reflection;

#endregion

#region Application Specific

using INTSOF.Logger.consts;

#endregion

#endregion

namespace INTSOF.Logger.factory
{
    public class SysXLoggerFactory
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private List<SysXLoggingClient> _clients;
        private static Object _lock = new Object();
        private static SysXLoggerFactory _this;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <remarks></remarks>
        public ICollection<SysXLoggingClient> Clients
        {
            get
            {
                return this._clients;
            }
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
        /// Gets the instance.
        /// </summary>
        /// <returns>SysXLoggerFactory</returns>
        /// <remarks></remarks>
        public static SysXLoggerFactory GetInstance()
        {
            return _this;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <returns>ILogger</returns>
        /// <remarks></remarks>
        public ILogger GetLogger()
        {
            return this.Clients.FirstOrDefault().Logger;
        }

        /// <summary>
        /// Gets the new logger.
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        /// <returns>ILogger</returns>
        /// <remarks></remarks>
        public ILogger GetNewLogger(String loggerName)
        {
            return this.GetNewLogger(loggerName, SysXLoggerConst.DEFAULT_LOG_LEVEL);
        }

        /// <summary>
        /// Gets the new logger.
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        /// <param name="logLevel">The log level.</param>
        /// <returns>ILogger</returns>
        /// <remarks></remarks>
        public ILogger GetNewLogger(String loggerName, String logLevel)
        {
            return GetLoggerObject(loggerName, logLevel);

        }

        /// <summary>
        /// Registers the specified client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <remarks></remarks>
        public void Register(SysXLoggingClient client)
        {
            this._clients.Add(client);
        }

        #endregion

        #region Private Methods

        static SysXLoggerFactory()
        {
            lock (_lock)
            {
                if (_this == null)
                {
                    _this = new SysXLoggerFactory();
                }
            }
        }

        private SysXLoggerFactory()
        {
            String LogLevel = String.Empty;

            LogLevel = ConfigurationManager.AppSettings[SysXLoggerConst.LOG_LEVEL_CINFIG_KEY_NAME] ??
                       SysXLoggerConst.DEFAULT_LOG_LEVEL;

            this._clients = new List<SysXLoggingClient>();
            this.Register(new SysXLoggingClient()
            {
                Logger = GetLoggerObject(SysXLoggerConst.SYSX_LOGGER_NAME, LogLevel)
            });
        }

        /// <summary>
        /// Get The object of logger class which is Defined in side the web.config
        /// </summary>
        /// <returns>ILogger</returns>
        private ILogger GetLoggerObject(String loggerName, String logLevel)
        {
            String loggerClassName = ConfigurationManager.AppSettings[SysXLoggerConst.LOG_CLASS_CONFIG_KEY_NAME];
            ILogger logger = null;

            try
            {
                Type loggerType = Assembly.GetExecutingAssembly().GetType(loggerClassName);
                Object[] args = new Object[2] { loggerName, logLevel };
                logger = (ILogger)Activator.CreateInstance(loggerType, args) ?? GetDefaultLogger(loggerName, logLevel);
            }
            catch (Exception ex)
            {
                logger = GetDefaultLogger(loggerName, logLevel);
                logger.Error( SysXLoggerConst.UNABLE_TO_LOAD_LOGGER + loggerClassName, ex);
            }

            return logger;
        }

        private ILogger GetDefaultLogger(String loggerName, String logLevel)
        {
            return new EventLogger(loggerName, logLevel); //if Some thing goes wrong by default EventLogger will be activated
        }

        #endregion

        #endregion
    }
}