using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using INTSOF.Utils;
using Entity;
using System.Web.Script.Serialization;

namespace ExternalVendors
{
    public static class ServiceLogger
    {
        private static Logger _logger;
        private static Logger _createOrderlogger;
        private static Logger _updateOrderlogger;
        private static Logger _bkgOrderStatusLogger;
        private static Logger _JiraTicketServiceLogger;
        private static Logger _UpdateFingerPrintOrderLogger;
        private static Logger _CBIResultFilesLogger;
        private static Logger _CDRImportDataLogger;
        private static Logger _ChangeStatusForUpdatedCBIResultFileServiceLogger;
        private static Logger _CBIFingerprintRecieptServiceLogger;

        static ServiceLogger()
        {
            _createOrderlogger = LogManager.GetLogger("CreateOrderServiceLogger");
            _updateOrderlogger = LogManager.GetLogger("UpdateOrderServiceLogger");
            _bkgOrderStatusLogger = LogManager.GetLogger("BkgOrderStatusServiceLogger");
            _JiraTicketServiceLogger = LogManager.GetLogger("JiraTicketServiceLogger");
            _UpdateFingerPrintOrderLogger = LogManager.GetLogger("UpdateFingerPrintOrderLogger");
            _CBIResultFilesLogger = LogManager.GetLogger("GetCBIResultFilesLogger");
            _CDRImportDataLogger = LogManager.GetLogger("CDRImportDataServiceLogger");
            _ChangeStatusForUpdatedCBIResultFileServiceLogger = LogManager.GetLogger("ChangeStatusForUpdatedCBIResultFileServiceLogger");
            _CBIFingerprintRecieptServiceLogger = LogManager.GetLogger("CBIFingerprintRecieptServiceLogger");
           
        }

        /// <summary>
        /// Logs the Debug message to the passed loggerInstance.
        /// </summary>
        /// <typeparam name="TArgument"></typeparam>
        /// <param name="message">The message that needs to log.</param>
        /// <param name="argument"></param>
        /// <param name="loggerInstance">There are mutliple Log file for Create Order Service Timer and Update Order Service Timer, pass the loggerInstance 
        /// value into which file logging information needs to write: loggerInstance can be either CreateOrderServiceLogger OR UpdateOrderServiceLogger.</param>
        public static void Debug<TArgument>(String message, TArgument argument, String loggerInstance)
        {
            //_logger = loggerInstance == "CreateOrderServiceLogger" ? _createOrderlogger : _updateOrderlogger;
            GetServiceLoggerInstance(loggerInstance);

            if (_logger.IsDebugEnabled)
            {
                if (argument.IsNotNull())
                {
                    _logger.Debug<String>(message + TrySerialize<TArgument>(argument));
                }
                else
                {
                    _logger.Debug(message);
                }
            }
        }

        /// <summary>
        /// Logs the Debug message to the passed loggerInstance.
        /// </summary>
        /// <param name="message">The message that needs to log.</param>
        /// <param name="loggerInstance">There are mutliple Log file for Create Order Service Timer and Update Order Service Timer, pass the loggerInstance 
        /// value into which file logging information needs to write: loggerInstance can be either CreateOrderServiceLogger OR UpdateOrderServiceLogger.</param>
        public static void Debug(String message, String loggerInstance)
        {
            //_logger = loggerInstance == "CreateOrderServiceLogger" ? _createOrderlogger : _updateOrderlogger;
            GetServiceLoggerInstance(loggerInstance);
            _logger.Debug(message);
        }

        /// <summary>
        /// Logs the Error message to the passed loggerInstance.
        /// </summary>
        /// <param name="message">The message that needs to log.</param>
        /// <param name="loggerInstance">There are mutliple Log file for Create Order Service Timer and Update Order Service Timer, pass the loggerInstance 
        /// value into which file logging information needs to write: loggerInstance can be either CreateOrderServiceLogger OR UpdateOrderServiceLogger.</param>
        public static void Error(string message, String loggerInstance)
        {
            //_logger = loggerInstance == "CreateOrderServiceLogger" ? _createOrderlogger : _updateOrderlogger;
            GetServiceLoggerInstance(loggerInstance);
            _logger.Error(message);
        }

        /// <summary>
        /// Logs the Info message to the passed loggerInstance.
        /// </summary>
        /// <param name="message">The message that needs to log.</param>
        /// <param name="loggerInstance">There are mutliple Log file for Create Order Service Timer and Update Order Service Timer, pass the loggerInstance 
        /// value into which file logging information needs to write: loggerInstance can be either CreateOrderServiceLogger OR UpdateOrderServiceLogger.</param>
        public static void Info(string message, String loggerInstance)
        {
            //_logger = loggerInstance == "CreateOrderServiceLogger" ? _createOrderlogger : _updateOrderlogger;
            GetServiceLoggerInstance(loggerInstance);
            _logger.Info(message);
        }

        private static void GetServiceLoggerInstance(String loggerInstance)
        {
            switch (loggerInstance)
            {
                case "CreateOrderServiceLogger":
                    _logger = _createOrderlogger;
                    break;
                case "UpdateOrderServiceLogger":
                    _logger = _updateOrderlogger;
                    break;
                case "BkgOrderStatusServiceLogger":
                    _logger = _bkgOrderStatusLogger;
                    break;
                case "JiraTicketServiceLogger":
                    _logger = _JiraTicketServiceLogger;
                    break;
                case "UpdateFingerPrintOrderLogger":
                    _logger = _UpdateFingerPrintOrderLogger;
                    break;
                case "GetCBIResultFilesLogger":
                    _logger = _CBIResultFilesLogger;
                    break;
                case "CDRImportDataServiceLogger":
                    _logger = _CDRImportDataLogger;
                    break;
                case "ChangeStatusForUpdatedCBIResultFileServiceLogger":
                    _logger = _ChangeStatusForUpdatedCBIResultFileServiceLogger;
                    break;
                case "CBIFingerprintRecieptServiceLogger":
                    _logger = _CBIFingerprintRecieptServiceLogger;
                    break;

            }

        }

        private static String TrySerialize<T>(T target)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                var serializedResult = serializer.Serialize(target);
                return serializedResult.ToString();
            }
            catch
            {
                return target.GetType().FullName;
            }
        }

    }
}
