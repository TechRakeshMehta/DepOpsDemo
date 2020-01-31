#region Header Comment BaseUserControl

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ServiceController.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined
using System;
using System.Linq;
using System.ServiceProcess;
using System.Configuration;
#endregion

#region Application Specific

using NLog;
using INTSOF.ServiceUtil;
using INTSOF.Utils.Consts;
using System.Timers;
using ExternalVendors;
using INTSOF.Utils;


#endregion
#endregion

namespace VendorOrderProcessService
{
    public partial class BkgOrderService : ServiceBase
    {
        #region Variables

        #region Public Variables
        #endregion

        #region Private Variables

        private Int32 _recordChunkSize = 20;
        private Int32 _sleepTime = 50000;
        private Timer createOrderTimer = null;
        private Timer updateOrderTimer = null;

        private Boolean isCreateOrderServiceExecuting = false;
        private Object lockObject = new Object();
        private Int32 _createOrderInterval = 10000;
        private Int32 _createOrderFromHour = 0;
        private Int32 _createOrderToHour = 24;
        private Int32 _createOrderToMinute = 0;
        private Int32 _createOrderFromMinute = 0;

        //Update Order Servcie
        private Int32 _updateOrderInterval = 10000;
        private Int32 _updateOrderFromHour = 0;
        private Int32 _updateOrderToHour = 24;
        private Int32 _updateOrderToMinute = 0;
        private Int32 _updateOrderFromMinute = 0;
        private Boolean isUpdateOrderServiceExecuting = false;

        //Background Order Notification and Status Service Variables
        private Int32 _orderNotificationInterval = 10000;
        private Int32 _orderNotificationFromHour = 0;
        private Int32 _orderNotificationToHour = 24;
        private Int32 _orderNotificationToMinute = 0;
        private Int32 _orderNotificationFromMinute = 0;
        private Timer bkgOrderStatusServiceTimer = null;
        private Boolean isBkgOrderStatusServiceExecuting = false;
        private Object lockStatusServiceObject = new Object();

        //Create Jira Ticket Service Variables
        private Int32 _jiraTicketServiceInterval = 10000;
        private Int32 _jiraTicketServiceFromHour = 0;
        private Int32 _jiraTicketServiceToHour = 24;
        private Int32 _jiraTicketServiceToMinute = 0;
        private Int32 _jiraTicketServiceFromMinute = 0;
        private Timer jiraTicketServiceTimer = null;
        private Boolean isJIRATicketServiceExecuting = false;
        private Object lockJiraTicketServiceObject = new Object();

        //Logger Static Variables
        private static String CreateOrderServiceServiceLogger;
        private static String UpdateOrderServiceServiceLogger;
        private static String BkgOrderStatusServiceServiceLogger;
        private static String JiraTicketServiceLogger;
        private static String CreateBulkOrderServiceLogger;
        //UAT-3541 -- CBI || CABS
        private static String FingerPrintOrderServiceServiceLogger;
        //UAT- 3826
        private static String CDRImportDataServiceServiceServiceLogger;

        //CBI Result Files
        private static String CBIResultFilesServiceServiceLogger;

        //Employment Notification for flagged orders.
        private Int32 _employemntNotificationInterval = 10000;
        private Int32 _employemntNotificationFromHour = 0;
        private Int32 _employemntNotificationToHour = 24;
        private Int32 _employemntNotificationToMinute = 0;
        private Int32 _employemntNotificationFromMinute = 0;
        private Timer empNotificationServiceTimer = null;
        private Boolean isEmpNotificationServiceExecuting = false;
        private Object lockEmpNotificationServiceObject = new Object();
        private Boolean IsEmploymentNotificationInterval = false;

        //Create Bulk Order Service Variables
        private Int32 _createBulkOrderServiceInterval = 10000;
        private Int32 _createBulkOrderServiceFromHour = 0;
        private Int32 _createBulkOrderServiceToHour = 24;
        private Int32 _createBulkOrderServiceToMinute = 0;
        private Int32 _createBulkOrderServiceFromMinute = 0;
        private Timer createBulkOrderServiceTimer = null;
        private Boolean isCreateBulkOrderServiceExecuting = false;
        private Object lockCreateBulkOrderServiceObject = new Object();


        //UAT-2697
        //create Repeated bulk order service variables
        private Int32 _createRepeatedBulkOrderServiceInterval = 10000;
        private Int32 _createRepeatedBulkOrderServiceFromHour = 0;
        private Int32 _createRepeatedBulkOrderServiceToHour = 24;
        private Int32 _createRepeatedBulkOrderServiceToMinute = 0;
        private Int32 _createRepeatedBulkOrderServiceFromMinute = 0;
        private Timer createRepeatedBulkOrderServiceTimer = null;
        private Boolean isCreateRepeatedBulkOrderServiceExecuting = false;
        private Object lockCreateRepeatedBulkOrderServiceObject = new Object();

        //UAT-3541 CBI||CABS
        private Timer updateFingerPrintOrderTimer = null;
        private Int32 _updateFingerPrintOrderInterval = 10000;
        private Int32 _updateFingerPrintOrderFromHour = 0;
        private Int32 _updateFingerPrintFromMinute = 0;
        private Int32 _updateFingerPrintOrderToHour = 24;
        private Int32 _updateFingerPrintOrderToMinute = 0;
        private Boolean isUpdateFingerPrintOrderServiceExecuting = false;
        private Object lockFingerPrintOrderServiceObject = new Object();

        //UAT-3851 CBI||CABS
        private Timer ChangeStatusForUpdatedCBIResultFileTimer = null;
        private Int32 _ChangeStatusForUpdatedCBIResultFileInterval = 10000;
        private Int32 _ChangeStatusForUpdatedCBIResultFileFromHour = 0;
        private Int32 _ChangeStatusForUpdatedCBIResultFileFromMinute = 0;
        private Int32 _ChangeStatusForUpdatedCBIResultFileToHour = 24;
        private Int32 _ChangeStatusForUpdatedCBIResultFileToMinute = 0;
        private Boolean isChangeStatusForUpdatedCBIResultFileServiceExecuting = false;
        private Object lockChangeStatusForUpdatedCBIResultFileServiceObject = new Object();
        private static String ChangeStatusForUpdatedCBIResultFileServiceLogger;
        private Int32 _CBIResultFileRecordChunkSize = 50;

        //CBI Result files
        private Timer CBIResultFilesTimer = null;
        private Int32 _CBIResultFilesInterval = 10000;
        private Int32 _CBIResultFilesFromHour = 0;
        private Int32 _CBIResultFilesFromMinute = 0;
        private Int32 _CBIResultFilesToHour = 24;
        private Int32 _CBIResultFilesToMinute = 0;
        private Boolean IsCBIResultFilesServiceExecuting = false;
        private Object lockCBIResultFilesServiceObject = new Object();
        //UAT-3826
        private Timer CDRImportDataTimer = null;
        private Int32 _CDRImportDataInterval = 7200000;
        private Int32 _CDRImportDataFromHour = 0;
        private Int32 _CDRImportDataFromMinute = 0;
        private Int32 _CDRImportDataToHour = 24;
        private Int32 _CDRImportDataToMinute = 0;
        private Boolean isUpdateCDRImportDataServiceExecuting = false;
        private Object lockCDRImportDataServiceObject = new Object();

        #region CBI Fingerprint Reciept

        private Timer CBIFingerprintRecieptTimer = null;
        private Int32 _CBIFingerprintRecieptInterval = 10000;
        private Int32 _CBIFingerprintRecieptFromHour = 0;
        private Int32 _CBIFingerprintRecieptFromMinute = 0;
        private Int32 _CBIFingerprintRecieptToHour = 24;
        private Int32 _CBIFingerprintRecieptToMinute = 0;
        private Boolean isCBIFingerprintRecieptServiceExecuting = false;
        private Object lockCBIFingerprintRecieptServiceObject = new Object();
        private static String CBIFingerprintRecieptServiceLogger;
        private Int32 _CBIFingerprintRecieptRecordChunkSize = 50;

        #endregion


        private Timer getDSOrderDataTimer = null;
        private Int32 _getDSOrderDataInterval = 10000;
        private Int32 _getDSOrderDataFromHour = 0;
        private Int32 _getDSOrderDataToHour = 24;
        private Int32 _getDSOrderDataToMinute = 0;
        private Int32 _getDSOrderDataFromMinute = 0;
        private Boolean isGetDSOrderDataFromClearStarServiceExecuting = false;
        private Object lockGetDSOrderDataFromClearStarServiceObject = new Object();


        //UAT 4710
        private Boolean isNotificationForFingerprintingExceededTATInterval = false;
        private Timer notificationForFingerprintingExceededTATTimer = null;
        private Boolean isNotificationForFingerprintingExceededTATExecuting = false;
        private Object lockbkgNotificationForFingerprintingExceededTATObject = new Object();

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public Int32 RecordChunkSize
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RecordChunkSize"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RecordChunkSize"])
                            ? ConfigurationManager.AppSettings["RecordChunkSize"] : _recordChunkSize.ToString());
                }
                else
                {
                    return _recordChunkSize;
                }
            }
        }

        //UAT-3851
        public Int32 CBIResultFileRecordChunkSize
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CBIResultFileRecordChunkSize"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CBIResultFileRecordChunkSize"])
                            ? ConfigurationManager.AppSettings["CBIResultFileRecordChunkSize"] : _CBIResultFileRecordChunkSize.ToString());
                }
                else
                {
                    return _CBIResultFileRecordChunkSize;
                }
            }
        }

        public Int32 SleepTime
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("SleepTime"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SleepTime"])
                             ? ConfigurationManager.AppSettings["SleepTime"] : _sleepTime.ToString());
                }
                else
                {
                    return _sleepTime;
                }
            }
        }

        public Int32 CreateOrderInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateOrderInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateOrderInterval"])
                        ? ConfigurationManager.AppSettings["CreateOrderInterval"] : _createOrderInterval.ToString());
                else
                    return _createOrderInterval;
            }
        }

        public Int32 CreateOrderFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateOrderFromHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateOrderFromHour"])
                                ? ConfigurationManager.AppSettings["CreateOrderFromHour"] : _createOrderFromHour.ToString());
                }
                else
                {
                    return _createOrderFromHour;
                }
            }
        }

        public Int32 CreateOrderFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateOrderFromMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateOrderFromMinute"])
                        ? ConfigurationManager.AppSettings["CreateOrderFromMinute"] : _createOrderFromMinute.ToString());
                }
                else
                {
                    return _createOrderFromMinute;
                }
            }
        }

        public Int32 CreateOrderToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateOrderToHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateOrderToHour"])
                        ? ConfigurationManager.AppSettings["CreateOrderToHour"] : _createOrderToHour.ToString());
                }
                else
                {
                    return _createOrderToHour;
                }
            }
        }

        public Int32 CreateOrderToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateOrderToMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateOrderToMinute"])
                        ? ConfigurationManager.AppSettings["CreateOrderToMinute"] : _createOrderToMinute.ToString());
                }
                else
                {
                    return _createOrderToMinute;
                }
            }
        }


        public Int32 UpdateOrderInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("UpdateOrderInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdateOrderInterval"])
                        ? ConfigurationManager.AppSettings["UpdateOrderInterval"] : _updateOrderInterval.ToString());
                else
                    return _updateOrderInterval;
            }
        }

        public Int32 UpdateOrderFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("UpdateOrderFromHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdateOrderFromHour"])
                                ? ConfigurationManager.AppSettings["UpdateOrderFromHour"] : _updateOrderFromHour.ToString());
                }
                else
                {
                    return _updateOrderFromHour;
                }
            }
        }

        public Int32 UpdateOrderFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("UpdateOrderFromMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdateOrderFromMinute"])
                        ? ConfigurationManager.AppSettings["UpdateOrderFromMinute"] : _updateOrderFromMinute.ToString());
                }
                else
                {
                    return _updateOrderFromMinute;
                }
            }
        }

        public Int32 UpdateOrderToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("UpdateOrderToHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdateOrderToHour"])
                        ? ConfigurationManager.AppSettings["UpdateOrderToHour"] : _updateOrderToHour.ToString());
                }
                else
                {
                    return _updateOrderToHour;
                }
            }
        }

        public Int32 UpdateOrderToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("UpdateOrderToMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdateOrderToMinute"])
                        ? ConfigurationManager.AppSettings["UpdateOrderToMinute"] : _updateOrderToMinute.ToString());
                }
                else
                {
                    return _updateOrderToMinute;
                }
            }
        }

        public Int32 OrderNotificationInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("OrderNotificationInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["OrderNotificationInterval"])
                        ? ConfigurationManager.AppSettings["OrderNotificationInterval"] : _orderNotificationInterval.ToString());
                else
                    return _orderNotificationInterval;
            }
        }

        public Int32 OrderNotificationFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("OrderNotificationFromHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["OrderNotificationFromHour"])
                                ? ConfigurationManager.AppSettings["OrderNotificationFromHour"] : _orderNotificationFromHour.ToString());
                }
                else
                {
                    return _orderNotificationFromHour;
                }
            }
        }

        public Int32 OrderNotificationFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("OrderNotificationFromMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["OrderNotificationFromMinute"])
                        ? ConfigurationManager.AppSettings["OrderNotificationFromMinute"] : _orderNotificationFromMinute.ToString());
                }
                else
                {
                    return _orderNotificationFromMinute;
                }
            }
        }

        public Int32 OrderNotificationToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("OrderNotificationToHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["OrderNotificationToHour"])
                        ? ConfigurationManager.AppSettings["OrderNotificationToHour"] : _orderNotificationToHour.ToString());
                }
                else
                {
                    return _orderNotificationToHour;
                }
            }
        }

        public Int32 OrderNotificationToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("OrderNotificationToMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["OrderNotificationToMinute"])
                        ? ConfigurationManager.AppSettings["OrderNotificationToMinute"] : _orderNotificationToMinute.ToString());
                }
                else
                {
                    return _orderNotificationToMinute;
                }
            }
        }

        public TimeSpan EmploymentNotificationStartTime
        {
            get
            {
                return new TimeSpan(EmploymentNotificationFromHour, EmploymentNotificationFromMinute, 0);
            }
        }

        public TimeSpan EmploymentNotificationEndTime
        {
            get
            {
                return new TimeSpan(EmploymentNotificationToHour, EmploymentNotificationToMinute, 0);
            }
        }

        public Int32 JiraTicketServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("JiraTicketServiceInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["JiraTicketServiceInterval"])
                        ? ConfigurationManager.AppSettings["JiraTicketServiceInterval"] : _jiraTicketServiceInterval.ToString());
                else
                    return _jiraTicketServiceInterval;
            }
        }

        public Int32 JiraTicketServiceFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("JiraTicketServiceFromHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["JiraTicketServiceFromHour"])
                                ? ConfigurationManager.AppSettings["JiraTicketServiceFromHour"] : _jiraTicketServiceFromHour.ToString());
                }
                else
                {
                    return _jiraTicketServiceFromHour;
                }
            }
        }

        public Int32 JiraTicketServiceFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("JiraTicketServiceFromMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["JiraTicketServiceFromMinute"])
                        ? ConfigurationManager.AppSettings["JiraTicketServiceFromMinute"] : _jiraTicketServiceFromMinute.ToString());
                }
                else
                {
                    return _jiraTicketServiceFromMinute;
                }
            }
        }

        public Int32 JiraTicketServiceToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("JiraTicketServiceToHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["JiraTicketServiceToHour"])
                        ? ConfigurationManager.AppSettings["JiraTicketServiceToHour"] : _jiraTicketServiceToHour.ToString());
                }
                else
                {
                    return _jiraTicketServiceToHour;
                }
            }
        }

        public Int32 JiraTicketServiceToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("JiraTicketServiceToMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["JiraTicketServiceToMinute"])
                        ? ConfigurationManager.AppSettings["JiraTicketServiceToMinute"] : _jiraTicketServiceToMinute.ToString());
                }
                else
                {
                    return _jiraTicketServiceToMinute;
                }
            }
        }

        //Create Bulk Order Service Property
        public Int32 CreateBulkOrderServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateBulkOrderServiceInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateBulkOrderServiceInterval"])
                        ? ConfigurationManager.AppSettings["CreateBulkOrderServiceInterval"] : _createBulkOrderServiceInterval.ToString());
                else
                    return _createBulkOrderServiceInterval;
            }
        }

        public Int32 CreateBulkOrderServiceFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateBulkOrderServiceFromHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateBulkOrderServiceFromHour"])
                                ? ConfigurationManager.AppSettings["CreateBulkOrderServiceFromHour"] : _createBulkOrderServiceFromHour.ToString());
                }
                else
                {
                    return _createBulkOrderServiceFromHour;
                }
            }
        }

        public Int32 CreateBulkOrderServiceFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateBulkOrderServiceFromMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateBulkOrderServiceFromMinute"])
                        ? ConfigurationManager.AppSettings["CreateBulkOrderServiceFromMinute"] : _createBulkOrderServiceFromMinute.ToString());
                }
                else
                {
                    return _createBulkOrderServiceFromMinute;
                }
            }
        }

        public Int32 CreateBulkOrderServiceToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateBulkOrderServiceToHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateBulkOrderServiceToHour"])
                        ? ConfigurationManager.AppSettings["CreateBulkOrderServiceToHour"] : _createBulkOrderServiceToHour.ToString());
                }
                else
                {
                    return _createBulkOrderServiceToHour;
                }
            }
        }

        public Int32 CreateBulkOrderServiceToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateBulkOrderServiceToMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateBulkOrderServiceToMinute"])
                        ? ConfigurationManager.AppSettings["CreateBulkOrderServiceToMinute"] : _createBulkOrderServiceToMinute.ToString());
                }
                else
                {
                    return _createBulkOrderServiceToMinute;
                }
            }
        }

        //Create Repeated Bulk order service property
        public Int32 CreateRepeatedBulkOrderServiceInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateRepeatedBulkOrderServiceInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateRepeatedBulkOrderServiceInterval"])
                        ? ConfigurationManager.AppSettings["CreateRepeatedBulkOrderServiceInterval"] : _createBulkOrderServiceInterval.ToString());
                else
                    return _createRepeatedBulkOrderServiceInterval;
            }
        }

        public Int32 CreateRepeatedBulkOrderServiceFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateRepeatedBulkOrderServiceFromHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateRepeatedBulkOrderServiceFromHour"])
                                ? ConfigurationManager.AppSettings["CreateRepeatedBulkOrderServiceFromHour"] : _createBulkOrderServiceFromHour.ToString());
                }
                else
                {
                    return _createRepeatedBulkOrderServiceFromHour;
                }
            }
        }

        public Int32 CreateRepeatedBulkOrderServiceFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateRepeatedBulkOrderServiceFromMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateRepeatedBulkOrderServiceFromMinute"])
                        ? ConfigurationManager.AppSettings["CreateRepeatedBulkOrderServiceFromMinute"] : _createBulkOrderServiceFromMinute.ToString());
                }
                else
                {
                    return _createRepeatedBulkOrderServiceFromMinute;
                }
            }
        }

        public Int32 CreateRepeatedBulkOrderServiceToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateRepeatedBulkOrderServiceToHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateRepeatedBulkOrderServiceToHour"])
                        ? ConfigurationManager.AppSettings["CreateRepeatedBulkOrderServiceToHour"] : _createBulkOrderServiceToHour.ToString());
                }
                else
                {
                    return _createRepeatedBulkOrderServiceToHour;
                }
            }
        }

        public Int32 CreateRepeatedBulkOrderServiceToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CreateRepeatedBulkOrderServiceToMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CreateRepeatedBulkOrderServiceToMinute"])
                        ? ConfigurationManager.AppSettings["CreateRepeatedBulkOrderServiceToMinute"] : _createBulkOrderServiceToMinute.ToString());
                }
                else
                {
                    return _createRepeatedBulkOrderServiceToMinute;
                }
            }
        }


        public Int32 EmploymentNotificationInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("EmploymentNotificationInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["EmploymentNotificationInterval"])
                        ? ConfigurationManager.AppSettings["EmploymentNotificationInterval"] : _employemntNotificationInterval.ToString());
                else
                    return _employemntNotificationInterval;
            }
        }

        public Int32 EmploymentNotificationFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("EmploymentNotificationFromHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["EmploymentNotificationFromHour"])
                                ? ConfigurationManager.AppSettings["EmploymentNotificationFromHour"] : _employemntNotificationFromHour.ToString());
                }
                else
                {
                    return _employemntNotificationFromHour;
                }
            }
        }

        public Int32 EmploymentNotificationFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("EmploymentNotificationFromMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["EmploymentNotificationFromMinute"])
                        ? ConfigurationManager.AppSettings["EmploymentNotificationFromMinute"] : _employemntNotificationFromMinute.ToString());
                }
                else
                {
                    return _employemntNotificationFromMinute;
                }
            }
        }

        public Int32 EmploymentNotificationToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("EmploymentNotificationToHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["EmploymentNotificationToHour"])
                        ? ConfigurationManager.AppSettings["EmploymentNotificationToHour"] : _employemntNotificationToHour.ToString());
                }
                else
                {
                    return _employemntNotificationToHour;
                }
            }
        }

        public Int32 EmploymentNotificationToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("EmploymentNotificationToMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["EmploymentNotificationToMinute"])
                        ? ConfigurationManager.AppSettings["EmploymentNotificationToMinute"] : _employemntNotificationToMinute.ToString());
                }
                else
                {
                    return _employemntNotificationToMinute;
                }
            }
        }

        public Double NextTimeSpanSeconds
        {
            get
            {
                return 86400000;
            }
        }
        
        //UAT-3541 CBI || CABS
        public Int32 UpdateFingerPrintOrderInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("UpdateFingerPrintOrderInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdateFingerPrintOrderInterval"])
                        ? ConfigurationManager.AppSettings["UpdateFingerPrintOrderInterval"] : _updateFingerPrintOrderInterval.ToString());
                else
                    return _updateFingerPrintOrderInterval;
            }
        }

        public Int32 UpdateFingerPrintOrderFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("UpdateFingerPrintOrderFromHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdateFingerPrintOrderFromHour"])
                                ? ConfigurationManager.AppSettings["UpdateFingerPrintOrderFromHour"] : _updateFingerPrintOrderFromHour.ToString());
                }
                else
                {
                    return _updateFingerPrintOrderFromHour;
                }
            }
        }

        public Int32 UpdateFingerPrintOrderFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("UpdateFingerPrintOrderFromMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdateFingerPrintOrderFromMinute"])
                        ? ConfigurationManager.AppSettings["UpdateFingerPrintOrderFromMinute"] : _updateFingerPrintFromMinute.ToString());
                }
                else
                {
                    return _updateFingerPrintFromMinute;
                }
            }
        }

        public Int32 UpdateFingerPrintOrderToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("UpdateFingerPrintOrderToHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdateFingerPrintOrderToHour"])
                        ? ConfigurationManager.AppSettings["UpdateFingerPrintOrderToHour"] : _updateFingerPrintOrderToHour.ToString());
                }
                else
                {
                    return _updateFingerPrintOrderToHour;
                }
            }
        }

        public Int32 UpdateFingerPrintOrderToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("UpdateFingerPrintOrderToMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UpdateFingerPrintOrderToMinute"])
                        ? ConfigurationManager.AppSettings["UpdateFingerPrintOrderToMinute"] : _updateFingerPrintOrderToMinute.ToString());
                }
                else
                {
                    return _updateFingerPrintOrderToMinute;
                }
            }
        }

        #region UAT-3851

        public Int32 ChangeStatusForUpdatedCBIResultFileInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ChangeStatusForUpdatedCBIResultFileInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ChangeStatusForUpdatedCBIResultFileInterval"])
                        ? ConfigurationManager.AppSettings["ChangeStatusForUpdatedCBIResultFileInterval"] : _ChangeStatusForUpdatedCBIResultFileInterval.ToString());
                else
                    return _ChangeStatusForUpdatedCBIResultFileInterval;
            }
        }

        public Int32 ChangeStatusForUpdatedCBIResultFileFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ChangeStatusForUpdatedCBIResultFileFromHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ChangeStatusForUpdatedCBIResultFileFromHour"])
                                ? ConfigurationManager.AppSettings["ChangeStatusForUpdatedCBIResultFileFromHour"] : _ChangeStatusForUpdatedCBIResultFileFromHour.ToString());
                }
                else
                {
                    return _ChangeStatusForUpdatedCBIResultFileFromHour;
                }
            }
        }

        public Int32 ChangeStatusForUpdatedCBIResultFileFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ChangeStatusForUpdatedCBIResultFileFromMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ChangeStatusForUpdatedCBIResultFileFromMinute"])
                        ? ConfigurationManager.AppSettings["ChangeStatusForUpdatedCBIResultFileFromMinute"] : _ChangeStatusForUpdatedCBIResultFileFromMinute.ToString());
                }
                else
                {
                    return _ChangeStatusForUpdatedCBIResultFileFromMinute;
                }
            }
        }

        public Int32 ChangeStatusForUpdatedCBIResultFileToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ChangeStatusForUpdatedCBIResultFileToHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ChangeStatusForUpdatedCBIResultFileToHour"])
                        ? ConfigurationManager.AppSettings["ChangeStatusForUpdatedCBIResultFileToHour"] : _ChangeStatusForUpdatedCBIResultFileToHour.ToString());
                }
                else
                {
                    return _ChangeStatusForUpdatedCBIResultFileToHour;
                }
            }
        }

        public Int32 ChangeStatusForUpdatedCBIResultFileToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ChangeStatusForUpdatedCBIResultFileToMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ChangeStatusForUpdatedCBIResultFileToMinute"])
                        ? ConfigurationManager.AppSettings["ChangeStatusForUpdatedCBIResultFileToMinute"] : _ChangeStatusForUpdatedCBIResultFileToMinute.ToString());
                }
                else
                {
                    return _ChangeStatusForUpdatedCBIResultFileToMinute;
                }
            }
        }
        #endregion
               
        #region CBI Fingerprint Reciept

        public Int32 CBIFingerprintRecieptInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CBIFingerprintRecieptInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CBIFingerprintRecieptInterval"])
                        ? ConfigurationManager.AppSettings["CBIFingerprintRecieptInterval"] : _CBIFingerprintRecieptInterval.ToString());
                else
                    return _CBIFingerprintRecieptInterval;
            }
        }

        public Int32 CBIFingerprintRecieptFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CBIFingerprintRecieptFromHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CBIFingerprintRecieptFromHour"])
                                ? ConfigurationManager.AppSettings["CBIFingerprintRecieptFromHour"] : _CBIFingerprintRecieptFromHour.ToString());
                }
                else
                {
                    return _CBIFingerprintRecieptFromHour;
                }
            }
        }

        public Int32 CBIFingerprintRecieptFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CBIFingerprintRecieptFromMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CBIFingerprintRecieptFromMinute"])
                        ? ConfigurationManager.AppSettings["CBIFingerprintRecieptFromMinute"] : _CBIFingerprintRecieptFromMinute.ToString());
                }
                else
                {
                    return _CBIFingerprintRecieptFromMinute;
                }
            }
        }

        public Int32 CBIFingerprintRecieptToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CBIFingerprintRecieptToHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CBIFingerprintRecieptToHour"])
                        ? ConfigurationManager.AppSettings["CBIFingerprintRecieptToHour"] : _CBIFingerprintRecieptToHour.ToString());
                }
                else
                {
                    return _CBIFingerprintRecieptToHour;
                }
            }
        }

        public Int32 CBIFingerprintRecieptToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CBIFingerprintRecieptToMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CBIFingerprintRecieptToMinute"])
                        ? ConfigurationManager.AppSettings["CBIFingerprintRecieptToMinute"] : _CBIFingerprintRecieptToMinute.ToString());
                }
                else
                {
                    return _CBIFingerprintRecieptToMinute;
                }
            }
        }

        #endregion
        
        //CBI Result Files
        public Int32 CBIResultFilesInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CBIResultFilesInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CBIResultFilesInterval"])
                        ? ConfigurationManager.AppSettings["CBIResultFilesInterval"] : _CBIResultFilesInterval.ToString());
                else
                    return _CBIResultFilesInterval;
            }
        }

        public Int32 CBIResultFilesFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CBIResultFilesFromHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CBIResultFilesFromHour"])
                                ? ConfigurationManager.AppSettings["CBIResultFilesFromHour"] : _CBIResultFilesFromHour.ToString());
                }
                else
                {
                    return _CBIResultFilesFromHour;
                }
            }
        }

        public Int32 CBIResultFilesFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CBIResultFilesFromMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CBIResultFilesFromMinute"])
                        ? ConfigurationManager.AppSettings["CBIResultFilesFromMinute"] : _CBIResultFilesFromMinute.ToString());
                }
                else
                {
                    return _CBIResultFilesFromMinute;
                }
            }
        }

        public Int32 CBIResultFilesToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CBIResultFilesToHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CBIResultFilesToHour"])
                        ? ConfigurationManager.AppSettings["CBIResultFilesToHour"] : _CBIResultFilesToHour.ToString());
                }
                else
                {
                    return _CBIResultFilesToHour;
                }
            }
        }

        public Int32 CBIResultFilesToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CBIResultFilesToMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CBIResultFilesToMinute"])
                        ? ConfigurationManager.AppSettings["CBIResultFilesToMinute"] : _CBIResultFilesToMinute.ToString());
                }
                else
                {
                    return _CBIResultFilesToMinute;
                }
            }
        }

        //UAT-4162

        public Int32 GetDSOrderDataInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("GetDSOrderDataInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["GetDSOrderDataInterval"])
                        ? ConfigurationManager.AppSettings["GetDSOrderDataInterval"] : _getDSOrderDataInterval.ToString());
                else
                    return _getDSOrderDataInterval;
            }
        }

        public Int32 GetDSOrderDataFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("GetDSOrderDataFromHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["GetDSOrderDataFromHour"])
                                ? ConfigurationManager.AppSettings["GetDSOrderDataFromHour"] : _getDSOrderDataFromHour.ToString());
                }
                else
                {
                    return _getDSOrderDataFromHour;
                }
            }
        }

        public Int32 GetDSOrderDataFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("GetDSOrderDataFromMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["GetDSOrderDataFromMinute"])
                        ? ConfigurationManager.AppSettings["GetDSOrderDataFromMinute"] : _getDSOrderDataFromMinute.ToString());
                }
                else
                {
                    return _getDSOrderDataFromMinute;
                }
            }
        }

        public Int32 GetDSOrderDataToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("GetDSOrderDataToHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["GetDSOrderDataToHour"])
                        ? ConfigurationManager.AppSettings["GetDSOrderDataToHour"] : _getDSOrderDataToHour.ToString());
                }
                else
                {
                    return _getDSOrderDataToHour;
                }
            }
        }

        public Int32 GetDSOrderDataToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("GetDSOrderDataToMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["GetDSOrderDataToMinute"])
                        ? ConfigurationManager.AppSettings["GetDSOrderDataToMinute"] : _getDSOrderDataToMinute.ToString());
                }
                else
                {
                    return _getDSOrderDataToMinute;
                }
            }
        }

        #endregion

        #region UAT - 3826 CDR Import
        public Int32 CDRImportDataInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CDRImportDataInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CDRImportDataInterval"])
                        ? ConfigurationManager.AppSettings["CDRImportDataInterval"] : _CDRImportDataInterval.ToString());
                else
                    return _CDRImportDataInterval;
            }
        }

        public Int32 CDRImportDataFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CDRImportDataFromHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CDRImportDataFromHour"])
                                ? ConfigurationManager.AppSettings["CDRImportDataFromHour"] : _CDRImportDataFromHour.ToString());
                }
                else
                {
                    return _CDRImportDataFromHour;
                }
            }
        }

        public Int32 CDRImportDataFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CDRImportDataFromMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CDRImportDataFromMinute"])
                        ? ConfigurationManager.AppSettings["CDRImportDataFromMinute"] : _CDRImportDataFromMinute.ToString());
                }
                else
                {
                    return _CDRImportDataFromMinute;
                }
            }
        }

        public Int32 CDRImportDataToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CDRImportDataToHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CDRImportDataToHour"])
                        ? ConfigurationManager.AppSettings["CDRImportDataToHour"] : _CDRImportDataToHour.ToString());
                }
                else
                {
                    return _CDRImportDataToHour;
                }
            }
        }

        public Int32 CDRImportDataToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CDRImportDataToMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["CDRImportDataToMinute"])
                        ? ConfigurationManager.AppSettings["CDRImportDataToMinute"] : _CDRImportDataToMinute.ToString());
                }
                else
                {
                    return _CDRImportDataToMinute;
                }
            }
        }
        #endregion

        #region UAT 4710 Fingerprinting Exceeded TAT Report

        

        private Int32 _notificationForFingerprintingExceededTATInterval = AppConsts.TwenteyFourHourMilliSec;
        public Int32 NotificationForFingerprintingExceededTATInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("NotificationForFingerprintingExceededTATInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["NotificationForFingerprintingExceededTATInterval"])
                        ? ConfigurationManager.AppSettings["NotificationForFingerprintingExceededTATInterval"] : _notificationForFingerprintingExceededTATInterval.ToString());
                else
                    return _notificationForFingerprintingExceededTATInterval;
            }
        }

        private Int32 _notificationForFingerprintingExceededTATFromHour = AppConsts.NONE;
        public Int32 NotificationForFingerprintingExceededTATFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("NotificationForFingerprintingExceededTATFromHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["NotificationForFingerprintingExceededTATFromHour"])
                        ? ConfigurationManager.AppSettings["NotificationForFingerprintingExceededTATFromHour"] : _notificationForFingerprintingExceededTATFromHour.ToString());
                else
                    return _notificationForFingerprintingExceededTATFromHour;
            }
        }

        private Int32 _notificationForFingerprintingExceededTATFromMinute = AppConsts.NONE;
        public Int32 NotificationForFingerprintingExceededTATFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("NotificationForFingerprintingExceededTATFromMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["NotificationForFingerprintingExceededTATFromMinute"])
                        ? ConfigurationManager.AppSettings["NotificationForFingerprintingExceededTATFromMinute"] : _notificationForFingerprintingExceededTATFromMinute.ToString());
                else
                    return _notificationForFingerprintingExceededTATFromMinute;
            }
        }

        private Int32 _notificationForFingerprintingExceededTATToHour = AppConsts.TWENTYFOUR;
        public Int32 NotificationForFingerprintingExceededTATToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("NotificationForFingerprintingExceededTATToHour"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["NotificationForFingerprintingExceededTATToHour"])
                        ? ConfigurationManager.AppSettings["NotificationForFingerprintingExceededTATToHour"] : _notificationForFingerprintingExceededTATToHour.ToString());
                else
                    return _notificationForFingerprintingExceededTATToHour;
            }
        }

        private Int32 _notificationForFingerprintingExceededTATToMinute = AppConsts.NONE;
        public Int32 NotificationForFingerprintingExceededTATToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("NotificationForFingerprintingExceededTATToMinute"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["NotificationForFingerprintingExceededTATToMinute"])
                        ? ConfigurationManager.AppSettings["NotificationForFingerprintingExceededTATToMinute"] : _notificationForFingerprintingExceededTATToMinute.ToString());
                else
                    return _notificationForFingerprintingExceededTATToMinute;
            }
        }

        public TimeSpan NotificationForFingerprintingExceededTATStartTime
        {
            get
            {
                return new TimeSpan(NotificationForFingerprintingExceededTATFromHour, NotificationForFingerprintingExceededTATFromMinute, 0);
            }
        }
        public TimeSpan NotificationForFingerprintingExceededTATEndTime
        {
            get
            {
                return new TimeSpan(NotificationForFingerprintingExceededTATToHour, NotificationForFingerprintingExceededTATToMinute, 0);
            }
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Service Events

        public BkgOrderService()
        {
            ServiceContext.init();
            InitializeComponent();
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;
            //Set ServiceLogger Instance variables            
            CreateOrderServiceServiceLogger = "CreateOrderServiceLogger";
            UpdateOrderServiceServiceLogger = "UpdateOrderServiceLogger";
            BkgOrderStatusServiceServiceLogger = "BkgOrderStatusServiceLogger";
            JiraTicketServiceLogger = "JiraTicketServiceLogger";
            FingerPrintOrderServiceServiceLogger = "UpdateFingerPrintOrderLogger";
            CBIResultFilesServiceServiceLogger = "UPdateCBIResultFilesLogger";
            CDRImportDataServiceServiceServiceLogger = "CDRImportDataServiceLogger";
            ChangeStatusForUpdatedCBIResultFileServiceLogger = "ChangeStatusForUpdatedCBIResultFileServiceLogger";
            CBIFingerprintRecieptServiceLogger = "CBIFingerprintRecieptServiceLogger";
            

            ServiceLogger.Info("Initialize Timer for Create Order", CreateOrderServiceServiceLogger);
            ServiceLogger.Info("Initialize Timer for Create Order", UpdateOrderServiceServiceLogger);
            ServiceLogger.Info("Initialize Timer for Create Order", BkgOrderStatusServiceServiceLogger);
            ServiceLogger.Info("Initialize Timer for Create Jira Ticket Service....", JiraTicketServiceLogger);
            ServiceLogger.Info("Initialize Timer for Update FingerPrint Order Service....", FingerPrintOrderServiceServiceLogger);
            ServiceLogger.Info("Initialize Timer for Update CBI Result Files....", CBIResultFilesServiceServiceLogger);
            ServiceLogger.Info("Initialize Timer for CDR Import Data Service....", CDRImportDataServiceServiceServiceLogger);
            ServiceLogger.Info("Initialize Timer for Update status for CBI Result File....", ChangeStatusForUpdatedCBIResultFileServiceLogger);
            ServiceLogger.Info("Initialize Timer for Send Reciept File To Applicant....", CBIFingerprintRecieptServiceLogger);
            

            //ServiceLogger.Info("Initialize Timer for Create Order.");
            if (CreateOrderInterval != SysXDBConsts.MINUS_ONE)
            {
                ServiceLogger.Info("Entered for Initializing Timer for Create Order.", CreateOrderServiceServiceLogger);
                createOrderTimer = new Timer();
                createOrderTimer.Interval = Convert.ToDouble(CreateOrderInterval);
                createOrderTimer.Elapsed += new ElapsedEventHandler(createOrderTimer_Elapsed);
                ServiceLogger.Info("Timer elapsed for Create Order.", CreateOrderServiceServiceLogger);
            }

            if (UpdateOrderInterval != SysXDBConsts.MINUS_ONE)
            {
                ServiceLogger.Info("Entered for Initializing Timer for Update Order.", UpdateOrderServiceServiceLogger);
                updateOrderTimer = new Timer();
                updateOrderTimer.Interval = Convert.ToDouble(UpdateOrderInterval);
                updateOrderTimer.Elapsed += new ElapsedEventHandler(UpdateOrderTimer_Elapsed);
                ServiceLogger.Info("Timer elapsed for Update Order.", UpdateOrderServiceServiceLogger);
            }

            if (OrderNotificationInterval != SysXDBConsts.MINUS_ONE)
            {
                ServiceLogger.Info("Entered for Initializing Timer for Background Order Notification and Status Service.", BkgOrderStatusServiceServiceLogger);
                bkgOrderStatusServiceTimer = new Timer();
                bkgOrderStatusServiceTimer.Interval = Convert.ToDouble(OrderNotificationInterval);
                bkgOrderStatusServiceTimer.Elapsed += new ElapsedEventHandler(bkgOrderStatusServiceTimer_Elapsed);
                ServiceLogger.Info("Timer elapsed for Background Order Notification and Status Service.", BkgOrderStatusServiceServiceLogger);
            }

            if (JiraTicketServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                ServiceLogger.Info("Entered for Initializing Timer for Create Jira Ticket Servcie.", JiraTicketServiceLogger);
                jiraTicketServiceTimer = new Timer();
                jiraTicketServiceTimer.Interval = Convert.ToDouble(JiraTicketServiceInterval);
                jiraTicketServiceTimer.Elapsed += new ElapsedEventHandler(CreateJiraTicketServiceTimer_Elapsed);
                ServiceLogger.Info("Timer elapsed for Create Order.", JiraTicketServiceLogger);
            }

            if (EmploymentNotificationInterval != SysXDBConsts.MINUS_ONE)
            {
                ServiceLogger.Info("Entered for Initializing Timer for Background Order Notification and Status Service for Employment Notification.", BkgOrderStatusServiceServiceLogger);
                empNotificationServiceTimer = new Timer();
                empNotificationServiceTimer.Interval = (firstTimerInterval(EmploymentNotificationStartTime, EmploymentNotificationEndTime, CurrentTime, 1));
                empNotificationServiceTimer.Elapsed += new ElapsedEventHandler(employmentNotificationTimer_Elapsed);
                ServiceLogger.Info("Timer elapsed for Background Order Notification and Status Service for Employment Notification.", BkgOrderStatusServiceServiceLogger);
            }

            if (CreateBulkOrderServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                ServiceLogger.Info("Entered for Initializing Timer for Create Bulk Order Servcie.", BkgOrderStatusServiceServiceLogger);
                createBulkOrderServiceTimer = new Timer();
                createBulkOrderServiceTimer.Interval = Convert.ToDouble(CreateBulkOrderServiceInterval);
                createBulkOrderServiceTimer.Elapsed += new ElapsedEventHandler(CreateBulkOrderServiceTimer_Elapsed);
                ServiceLogger.Info("Timer elapsed for Create Order.", BkgOrderStatusServiceServiceLogger);
            }
            //UAT-2697
            if (CreateRepeatedBulkOrderServiceInterval != SysXDBConsts.MINUS_ONE)
            {
                ServiceLogger.Info("Entered for Initializing Timer for Create Repeated Bulk Order Servcie.", BkgOrderStatusServiceServiceLogger);
                createRepeatedBulkOrderServiceTimer = new Timer();
                createRepeatedBulkOrderServiceTimer.Interval = Convert.ToDouble(CreateRepeatedBulkOrderServiceInterval);
                createRepeatedBulkOrderServiceTimer.Elapsed += new ElapsedEventHandler(CreateRepeatedBulkOrderServiceTimer_Elapsed);
                ServiceLogger.Info("Timer elapsed for Create Order.", BkgOrderStatusServiceServiceLogger);
            }

            //UAT-3541 CBI || CABS
            if (UpdateFingerPrintOrderInterval != SysXDBConsts.MINUS_ONE)
            {
                ServiceLogger.Info("Entered for Initializing Timer for Update Order.", FingerPrintOrderServiceServiceLogger);
                updateFingerPrintOrderTimer = new Timer();
                updateFingerPrintOrderTimer.Interval = Convert.ToDouble(UpdateFingerPrintOrderInterval);
                updateFingerPrintOrderTimer.Elapsed += new ElapsedEventHandler(UpdateFingerPrintOrderServiceTimer_Elapsed);
                ServiceLogger.Info("Timer elapsed for Update FingerPrint Order.", FingerPrintOrderServiceServiceLogger);
            }


            //CBI Result Files
            if (CBIResultFilesInterval != SysXDBConsts.MINUS_ONE)
            {
                ServiceLogger.Info("Entered for Initializing Timer for CBI Result files.", CBIResultFilesServiceServiceLogger);
                CBIResultFilesTimer = new Timer();
                CBIResultFilesTimer.Interval = Convert.ToDouble(CBIResultFilesInterval);
                CBIResultFilesTimer.Elapsed += new ElapsedEventHandler(CBIResultFilesServiceTimer_Elapsed);
                ServiceLogger.Info("Timer elapsed for CBI Result Files.", CBIResultFilesServiceServiceLogger);
            }

            //UAT- 3826

            if (CDRImportDataInterval != SysXDBConsts.MINUS_ONE)
            {
                ServiceLogger.Info("Entered for Initializing Timer for Importing Data.", CDRImportDataServiceServiceServiceLogger);
                CDRImportDataTimer = new Timer();
                CDRImportDataTimer.Interval = Convert.ToDouble(CDRImportDataInterval);
                CDRImportDataTimer.Elapsed += new ElapsedEventHandler(CDRImportDataServiceTimer_Elapsed);
                ServiceLogger.Info("Timer elapsed for Importing data.", CDRImportDataServiceServiceServiceLogger);
            }
            // UAT- 3851
            if (ChangeStatusForUpdatedCBIResultFileInterval != SysXDBConsts.MINUS_ONE)
            {
                ServiceLogger.Info("Entered for Initializing Timer for change status for updated CBI file result.", ChangeStatusForUpdatedCBIResultFileServiceLogger);
                ChangeStatusForUpdatedCBIResultFileTimer = new Timer();
                ChangeStatusForUpdatedCBIResultFileTimer.Interval = Convert.ToDouble(ChangeStatusForUpdatedCBIResultFileInterval);
                ChangeStatusForUpdatedCBIResultFileTimer.Elapsed += new ElapsedEventHandler(ChangeStatusForUpdatedCBIResultFileServiceTimer_Elapsed);
                ServiceLogger.Info("Timer elapsed for Change status for updated Cbi file result.", ChangeStatusForUpdatedCBIResultFileServiceLogger);
            }

            if (CBIFingerprintRecieptInterval != SysXDBConsts.MINUS_ONE)
            {
                ServiceLogger.Info("Entered for Initializing Timer for Update Order.", CBIFingerprintRecieptServiceLogger);
                CBIFingerprintRecieptTimer = new Timer();
                CBIFingerprintRecieptTimer.Interval = Convert.ToDouble(CBIFingerprintRecieptInterval);
                CBIFingerprintRecieptTimer.Elapsed += new ElapsedEventHandler(CBIFingerprintRecieptServiceTimer_Elapsed);
                ServiceLogger.Info("Timer elapsed for Update FingerPrint Order.", CBIFingerprintRecieptServiceLogger);
            }

            //Get Ds Order data from clearstar
            if (GetDSOrderDataInterval != SysXDBConsts.MINUS_ONE)
            {
                ServiceLogger.Info("Entered for Initializing Timer for Get Drug Screening Orders From Clearstar.", UpdateOrderServiceServiceLogger);
                getDSOrderDataTimer = new Timer();
                getDSOrderDataTimer.Interval = Convert.ToDouble(GetDSOrderDataInterval);
                getDSOrderDataTimer.Elapsed += new ElapsedEventHandler(GetDSOrderDataFromClearStarTimer_Elapsed);
                ServiceLogger.Info("Timer elapsed for Get Drug Screening Orders From Clearstar.", UpdateOrderServiceServiceLogger);
            }

            //UAT 4710 Fingerprinting Exceeded TAT Report Notification
            if (NotificationForFingerprintingExceededTATInterval != SysXDBConsts.MINUS_ONE)
            {
                ServiceLogger.Info("Entered for Initializing Timer for Fingerprinting Exceeded TAT Report Notification.", BkgOrderStatusServiceServiceLogger);
                notificationForFingerprintingExceededTATTimer = new Timer();
                notificationForFingerprintingExceededTATTimer.Interval = (firstTimerInterval(NotificationForFingerprintingExceededTATStartTime,NotificationForFingerprintingExceededTATEndTime, CurrentTime, 2) + Convert.ToDouble(300000));
                notificationForFingerprintingExceededTATTimer.Elapsed += new ElapsedEventHandler(NotificationForFingerprintingExceededTAT_Elapsed);
                ServiceLogger.Info("Timer elapsed for Fingerprinting Exceeded TAT Report Notification.", BkgOrderStatusServiceServiceLogger);
            }

        }

        void createOrderTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ServiceLogger.Info("Timer invoked for creating external vendor orders", CreateOrderServiceServiceLogger);
            lock (this.lockObject)
            {
                if (isCreateOrderServiceExecuting)
                    return;
                isCreateOrderServiceExecuting = true;
            }
            ServiceLogger.Info("Entered processing for creating external vendor orders", CreateOrderServiceServiceLogger);

            try
            {
                CreateOrderService.SendExternalVendorOrders();
            }
            catch (Exception ex)
            {
                isCreateOrderServiceExecuting = false;
                ServiceLogger.Error(String.Format("An Error has occured in Create External Vendor Order, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                    CreateOrderServiceServiceLogger);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isCreateOrderServiceExecuting = false;
            }
        }

        void UpdateOrderTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ServiceLogger.Info("Timer invoked for updating external vendor orders.", UpdateOrderServiceServiceLogger);

            lock (this.lockObject)
            {
                if (isUpdateOrderServiceExecuting)
                    return;
                isUpdateOrderServiceExecuting = true;
            }

            ServiceLogger.Info("Entered processing for updating external vendor orders.", UpdateOrderServiceServiceLogger);

            try
            {
                UpdateOrderService.UpdateExtVendorOrder();
            }
            catch (Exception ex)
            {
                isUpdateOrderServiceExecuting = false;
                ServiceLogger.Error(String.Format("An Error has occured in Update External Vendor Order, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                UpdateOrderServiceServiceLogger);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isUpdateOrderServiceExecuting = false;
            }
        }

        void bkgOrderStatusServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ServiceLogger.Info("Timer invoked for Background Order Notification and Status Service.", BkgOrderStatusServiceServiceLogger);

            lock (this.lockStatusServiceObject)
            {
                if (isBkgOrderStatusServiceExecuting)
                    return;
                isBkgOrderStatusServiceExecuting = true;
            }

            ServiceLogger.Info("Entered processing for Background Order Notification and Status Service.", BkgOrderStatusServiceServiceLogger);

            try
            {
                BkgOrderStatusService.CreateBkgOrderNotification();
                BkgOrderStatusService.UpdateBkgOrderColorStatus();
                BkgOrderStatusService.CreateBkgOrderResultCompletedNotification();
                BkgOrderStatusService.CreateServiceGroupCompletedNotification();
                BkgOrderStatusService.CreateBkgFlaggedOrderResultCompletedNotification();
                BkgOrderStatusService.SendFlaggedCompletedServiceGroupNotification();
            }
            catch (Exception ex)
            {
                isBkgOrderStatusServiceExecuting = false;
                ServiceLogger.Error(String.Format("An Error has occured in Background Order Status Service, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                BkgOrderStatusServiceServiceLogger);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isBkgOrderStatusServiceExecuting = false;
            }
        }

        void CreateJiraTicketServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ServiceLogger.Info("Timer invoked for creating Jira Ticket", JiraTicketServiceLogger);
            lock (this.lockJiraTicketServiceObject)
            {
                if (isJIRATicketServiceExecuting)
                    return;
                isJIRATicketServiceExecuting = true;
            }
            ServiceLogger.Info("Entered processing for creating Jira Ticket", JiraTicketServiceLogger);

            try
            {
                CreateJiraTicketService.CreateJiraTicketForFailedOrders();
            }
            catch (Exception ex)
            {
                isJIRATicketServiceExecuting = false;
                ServiceLogger.Error(String.Format("An Error has occured in Create External Vendor Order, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                    CreateOrderServiceServiceLogger);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isJIRATicketServiceExecuting = false;
            }
        }

        void employmentNotificationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ServiceLogger.Info("Timer invoked for Background Order Notification and Status Service for Employment Notification.", BkgOrderStatusServiceServiceLogger);

            if (!IsEmploymentNotificationInterval)
            {
                empNotificationServiceTimer.Interval = Convert.ToDouble(EmploymentNotificationInterval);
                IsEmploymentNotificationInterval = true;
            }

            lock (this.lockEmpNotificationServiceObject)
            {
                if (isEmpNotificationServiceExecuting)
                    return;
                isEmpNotificationServiceExecuting = true;
            }

            ServiceLogger.Info("Entered processing for Background Order Notification and Status Service for Employment Notification.", BkgOrderStatusServiceServiceLogger);
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            try
            {
                if (currentTime >= EmploymentNotificationStartTime && currentTime <= EmploymentNotificationEndTime)
                {
                    //Send employment notification for flagged orders [UAT-1177:System Updates for 613]
                    //UAT-1613 Need to stop service
                    //BkgOrderStatusService.SendBkgFlaggedOrderCompletedEmploymentNotification();

                    BkgOrderStatusService.CreateEmploymentFlaggedServiceGroupCompletedNotification();
                }
            }
            catch (Exception ex)
            {
                isEmpNotificationServiceExecuting = false;
                ServiceLogger.Error(String.Format("An Error has occured in Background Order Status Service for Epmloyment Notification, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                BkgOrderStatusServiceServiceLogger);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isEmpNotificationServiceExecuting = false;
            }
        }

        void CreateBulkOrderServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ServiceLogger.Info("Timer invoked for creating Bulk Order", BkgOrderStatusServiceServiceLogger);
            lock (this.lockCreateBulkOrderServiceObject)
            {
                if (isCreateBulkOrderServiceExecuting)
                    return;
                isCreateBulkOrderServiceExecuting = true;
            }
            ServiceLogger.Info("Entered processing for creating Bulk Order", BkgOrderStatusServiceServiceLogger);

            try
            {
                CreateBulkOrderService.CreateBulkOrder();
            }
            catch (Exception ex)
            {
                isCreateBulkOrderServiceExecuting = false;
                ServiceLogger.Error(String.Format("An Error has occured in Create Bulk Order, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                    BkgOrderStatusServiceServiceLogger);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isCreateBulkOrderServiceExecuting = false;
            }
        }
        //UAT-2697
        void CreateRepeatedBulkOrderServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ServiceLogger.Info("Timer invoked for creating Repeated Bulk Order", BkgOrderStatusServiceServiceLogger);
            lock (this.lockCreateRepeatedBulkOrderServiceObject)
            {
                if (isCreateRepeatedBulkOrderServiceExecuting)
                    return;
                isCreateRepeatedBulkOrderServiceExecuting = true;
            }
            ServiceLogger.Info("Entered processing for creating Repeated Bulk Order", BkgOrderStatusServiceServiceLogger);

            try
            {
                CreateBulkOrderService.CreateBulkOrderForRepeatedSearch();
            }
            catch (Exception ex)
            {
                isCreateRepeatedBulkOrderServiceExecuting = false;
                ServiceLogger.Error(String.Format("An Error has occured in Create Repeated Bulk Order, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                    BkgOrderStatusServiceServiceLogger);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isCreateRepeatedBulkOrderServiceExecuting = false;
            }
        }

        //UAT-3541 -- CBI || CABS
        void UpdateFingerPrintOrderServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ServiceLogger.Info("Timer invoked for updating fingerprint service orders.", FingerPrintOrderServiceServiceLogger);

            lock (this.lockFingerPrintOrderServiceObject)
            {
                if (isUpdateFingerPrintOrderServiceExecuting)
                    return;
                isUpdateFingerPrintOrderServiceExecuting = true;
            }

            ServiceLogger.Info("Entered processing for updating fingerprint service orders.", FingerPrintOrderServiceServiceLogger);

            try
            {
                UpdateFingerPrintOrderService.UpdateFingerPrintOrder();
            }
            catch (Exception ex)
            {
                isUpdateFingerPrintOrderServiceExecuting = false;
                ServiceLogger.Error(String.Format("An Error has occured in Update Fingerprint Service Order, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                FingerPrintOrderServiceServiceLogger);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isUpdateFingerPrintOrderServiceExecuting = false;
            }
        }

        #region UAT-3851 CBI||CABS

        public void ChangeStatusForUpdatedCBIResultFileServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ServiceLogger.Info("Timer invoked for Cbi file result status update.", ChangeStatusForUpdatedCBIResultFileServiceLogger);

            lock (this.lockChangeStatusForUpdatedCBIResultFileServiceObject)
            {
                if (isChangeStatusForUpdatedCBIResultFileServiceExecuting)
                    return;
                isChangeStatusForUpdatedCBIResultFileServiceExecuting = true;
            }

            ServiceLogger.Info("Entered processing for cbi file result status update.", ChangeStatusForUpdatedCBIResultFileServiceLogger);

            try
            {
                UpdateFingerPrintOrderService.ChangeStatusForUpdatedCBIResultFile(CBIResultFileRecordChunkSize);
            }
            catch (Exception ex)
            {
                isChangeStatusForUpdatedCBIResultFileServiceExecuting = false;
                ServiceLogger.Error(String.Format("An Error has occured in updating status of Applicant appointment on Cbi result file basis, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                ChangeStatusForUpdatedCBIResultFileServiceLogger);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isChangeStatusForUpdatedCBIResultFileServiceExecuting = false;
            }
        }
        #endregion

        #region UAT - CBI Fingerprint Reciept


        public void CBIFingerprintRecieptServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ServiceLogger.Info("Timer invoked for CBI File Reciept.", CBIFingerprintRecieptServiceLogger);

            lock (this.lockCBIFingerprintRecieptServiceObject)
            {
                if (isCBIFingerprintRecieptServiceExecuting)
                    return;
                isCBIFingerprintRecieptServiceExecuting = true;
            }

            ServiceLogger.Info("Entered processing for cbi file reciept.", CBIFingerprintRecieptServiceLogger);

            try
            {
                UpdateFingerPrintOrderService.SendCBIFingerprintReciept(CBIResultFileRecordChunkSize);
            }
            catch (Exception ex)
            {
                isCBIFingerprintRecieptServiceExecuting = false;
                ServiceLogger.Error(String.Format("An Error has occured in Send Cbi Fingerprint Reciept, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                CBIFingerprintRecieptServiceLogger);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isCBIFingerprintRecieptServiceExecuting = false;
            }
        }

        #endregion

        //UAT-3826
        void CDRImportDataServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ServiceLogger.Info("Timer invoked for CDR Import Data.", CDRImportDataServiceServiceServiceLogger);


            lock (this.lockCDRImportDataServiceObject)
            {
                if (isUpdateCDRImportDataServiceExecuting)
                    return;
                isUpdateCDRImportDataServiceExecuting = true;
            }

            ServiceLogger.Info("Entered processing for CDR Import Data service.", CDRImportDataServiceServiceServiceLogger);

            try
            {
                CDRExportDataService.CDRImportDataToTable();
            }
            catch (Exception ex)
            {
                isUpdateCDRImportDataServiceExecuting = false;
                ServiceLogger.Error(String.Format("An Error has occured in CDR Import Data Service, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                CDRImportDataServiceServiceServiceLogger);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isUpdateCDRImportDataServiceExecuting = false;
            }
        }
        //CBI Result Files
        void CBIResultFilesServiceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ServiceLogger.Info("Timer invoked for Get CBI Result files.", CBIResultFilesServiceServiceLogger);

            lock (this.lockCBIResultFilesServiceObject)
            {
                if (IsCBIResultFilesServiceExecuting)
                    return;
                IsCBIResultFilesServiceExecuting = true;
            }

            ServiceLogger.Info("Entered processing for CBI Result Files service orders.", CBIResultFilesServiceServiceLogger);

            try
            {
                UpdateFingerPrintOrderService.GetCBIResultFilesAndUpdateTables();

            }
            catch (Exception ex)
            {
                IsCBIResultFilesServiceExecuting = false;
                ServiceLogger.Error(String.Format("An Error has occured in CBI Result Files , the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                CBIResultFilesServiceServiceLogger);
            }

            finally
            {
                ServiceContext.ReleaseContextItems();
                IsCBIResultFilesServiceExecuting = false;
            }
        }

        //UAT-4162 // Retry logic for Drug Screening order to get data from clearstar.
        void GetDSOrderDataFromClearStarTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ServiceLogger.Info("Timer invoked for drug screening orders data from Clearstar.", UpdateOrderServiceServiceLogger);

            lock (this.lockGetDSOrderDataFromClearStarServiceObject)
            {
                if (isGetDSOrderDataFromClearStarServiceExecuting)
                    return;
                isGetDSOrderDataFromClearStarServiceExecuting = true;
            }

            ServiceLogger.Info("Entered processing for drug screening orders data from Clearstar.", UpdateOrderServiceServiceLogger);

            try
            {
                UpdateOrderService.GetDSOrderDataFromClearStar();
            }
            catch (Exception ex)
            {
                isGetDSOrderDataFromClearStarServiceExecuting = false;
                ServiceLogger.Error(String.Format("An Error has occured in get drug screening orders data from Clearstar, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                UpdateOrderServiceServiceLogger);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isGetDSOrderDataFromClearStarServiceExecuting = false;
            }
        }


        //UAT 4710
        #region UAT-4710 Fingerprinting Exceeded TAT Report 

        void NotificationForFingerprintingExceededTAT_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isNotificationForFingerprintingExceededTATInterval)
            {
                notificationForFingerprintingExceededTATTimer.Interval = Convert.ToDouble(NotificationForFingerprintingExceededTATInterval);
                isNotificationForFingerprintingExceededTATInterval = true;
            }

            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;

            ServiceLogger.Info("Timer invoked for SendNotificationForFingerprintingExceededTAT", BkgOrderStatusServiceServiceLogger);
            lock (this.lockbkgNotificationForFingerprintingExceededTATObject)
            {
                if (isNotificationForFingerprintingExceededTATExecuting)
                {
                    ServiceLogger.Info(String.Format("Send mail for Finger Printing Exceeded TAT was already in progress. Thread id {0} will exit now.", System.Threading.Thread.CurrentThread.ManagedThreadId), BkgOrderStatusServiceServiceLogger);
                    return;
                }
                ServiceLogger.Info(String.Format("Thread id {0} will execute SendNotificationForFingerprintingExceededTAT now.", System.Threading.Thread.CurrentThread.ManagedThreadId), BkgOrderStatusServiceServiceLogger);
                isNotificationForFingerprintingExceededTATExecuting = true;

            }
            ServiceLogger.Info(String.Format("Entered processing for SendNotificationForFingerprintingExceededTAT"), BkgOrderStatusServiceServiceLogger);
            try
            {

                if (CurrentTime >= NotificationForFingerprintingExceededTATStartTime && CurrentTime <= NotificationForFingerprintingExceededTATEndTime)
                {
                    ServiceLogger.Info("******************* START Calling Method SendNotificationForFingerprintingExceededTAT: " + DateTime.Now.ToString() + " *******************", BkgOrderStatusServiceServiceLogger);
                    BkgOrderStatusService.SendNotificationForFingerprintingExceededTAT();
                    ServiceLogger.Info("******************* END Calling Method for SendNotificationForFingerprintingExceededTAT : " + DateTime.Now.ToString() + " *******************", BkgOrderStatusServiceServiceLogger);
                }
                else
                {
                    ServiceLogger.Info("******************* Exit Copy SendNotificationForFingerprintingExceededTAT" + DateTime.Now.ToString() + " *******************", BkgOrderStatusServiceServiceLogger);
                }
            }
            catch (Exception ex)
            {
                ServiceLogger.Info(String.Format("Error in Thread id {0} will exit SendNotificationForFingerprintingExceededTAT.", System.Threading.Thread.CurrentThread.ManagedThreadId), BkgOrderStatusServiceServiceLogger);
                isNotificationForFingerprintingExceededTATExecuting = false;
                ServiceLogger.Error(String.Format("An Error has occured in SendNotificationForFingerprintingExceededTAT, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString), BkgOrderStatusServiceServiceLogger);
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                ServiceLogger.Info(String.Format("SendNotificationForFingerprintingExceededTAT complete. Thread id {0} will exit SendNotificationForFingerprintingExceededTAT now.", System.Threading.Thread.CurrentThread.ManagedThreadId), BkgOrderStatusServiceServiceLogger);
                isNotificationForFingerprintingExceededTATExecuting = false;
            }
        }
        #endregion

        protected override void OnStart(string[] args)
        {
            if (createOrderTimer != null)
            {
                createOrderTimer.Enabled = true;
                createOrderTimer.Start();
                ServiceLogger.Info("CreateOrderService start.", CreateOrderServiceServiceLogger);
            }

            if (updateOrderTimer != null)
            {
                updateOrderTimer.Enabled = true;
                updateOrderTimer.Start();
                ServiceLogger.Info("UpdateOrderService start.", UpdateOrderServiceServiceLogger);
            }

            if (bkgOrderStatusServiceTimer != null)
            {
                bkgOrderStatusServiceTimer.Enabled = true;
                bkgOrderStatusServiceTimer.Start();
                ServiceLogger.Info("Order Status Service start.", BkgOrderStatusServiceServiceLogger);
            }

            if (jiraTicketServiceTimer != null)
            {
                jiraTicketServiceTimer.Enabled = true;
                jiraTicketServiceTimer.Start();
                ServiceLogger.Info("Jira Ticket Service Service start.", BkgOrderStatusServiceServiceLogger);
            }

            if (empNotificationServiceTimer != null)
            {
                empNotificationServiceTimer.Enabled = true;
                empNotificationServiceTimer.Start();
                ServiceLogger.Info("Order Status Service start for Employment Notification.", BkgOrderStatusServiceServiceLogger);
            }

            if (createBulkOrderServiceTimer != null)
            {
                createBulkOrderServiceTimer.Enabled = true;
                createBulkOrderServiceTimer.Start();
                ServiceLogger.Info("Create Bulk Order Service start.", BkgOrderStatusServiceServiceLogger);
            }

            if (createRepeatedBulkOrderServiceTimer != null)
            {
                createRepeatedBulkOrderServiceTimer.Enabled = true;
                createRepeatedBulkOrderServiceTimer.Start();
                ServiceLogger.Info("Create Repeated Bulk Order Service start.", BkgOrderStatusServiceServiceLogger);
            }

            //UAT-3541 CBI ||CABS
            if (updateFingerPrintOrderTimer != null)
            {
                updateFingerPrintOrderTimer.Enabled = true;
                updateFingerPrintOrderTimer.Start();
                ServiceLogger.Info("UpdateFingerPrintOrderService start.", FingerPrintOrderServiceServiceLogger);
            }
            //CBI Result Files
            if (CBIResultFilesTimer != null)
            {
                CBIResultFilesTimer.Enabled = true;
                CBIResultFilesTimer.Start();
                ServiceLogger.Info("CBI Result Files start.", CBIResultFilesServiceServiceLogger);
            }
            //UAT-3826 CBI ||CABS
            if (CDRImportDataTimer != null)
            {
                CDRImportDataTimer.Enabled = true;
                CDRImportDataTimer.Start();
                ServiceLogger.Info("CDR Import Data Service start.", CDRImportDataServiceServiceServiceLogger);
            }
            // UAT - 3851
            if (ChangeStatusForUpdatedCBIResultFileTimer != null)
            {
                ChangeStatusForUpdatedCBIResultFileTimer.Enabled = true;
                ChangeStatusForUpdatedCBIResultFileTimer.Start();
                ServiceLogger.Info("ChangeStatusForUpdatedCBIResultFileService start.", ChangeStatusForUpdatedCBIResultFileServiceLogger);
            }
           
            #region CBI Fingerprint Reciept
            if (CBIFingerprintRecieptTimer != null)
            {
                CBIFingerprintRecieptTimer.Enabled = true;
                CBIFingerprintRecieptTimer.Start();
                ServiceLogger.Info("CBIFingerprintRecieptService start.", CBIFingerprintRecieptServiceLogger);
            }
            #endregion

            //UAT-4162:- Timer for DS Order to get data from clearstar started
            if (getDSOrderDataTimer != null)
            {
                getDSOrderDataTimer.Enabled = true;
                getDSOrderDataTimer.Start();
                ServiceLogger.Info("GetDSOrderDataFromClearStarService start.", UpdateOrderServiceServiceLogger);
            }

            //UAT 4710
            if(notificationForFingerprintingExceededTATTimer != null)
            {
                notificationForFingerprintingExceededTATTimer.Enabled = true;
                notificationForFingerprintingExceededTATTimer.Start();
                ServiceLogger.Info("NotificationForFingerprintingExceededTATTimer start", BkgOrderStatusServiceServiceLogger);
            }

        }

        protected override void OnStop()
        {
            if (createOrderTimer != null)
            {
                createOrderTimer.Stop();
                ServiceLogger.Info("CreateOrderService stopped.", CreateOrderServiceServiceLogger);
            }

            if (updateOrderTimer != null)
            {
                updateOrderTimer.Stop();
                ServiceLogger.Info("UpdateOrderService stopped.", UpdateOrderServiceServiceLogger);
            }

            if (bkgOrderStatusServiceTimer != null)
            {
                bkgOrderStatusServiceTimer.Stop();
                ServiceLogger.Info("Order Status Service stopped.", BkgOrderStatusServiceServiceLogger);
            }

            if (jiraTicketServiceTimer != null)
            {
                jiraTicketServiceTimer.Stop();
                ServiceLogger.Info("Jira Ticket Service Service stopped.", BkgOrderStatusServiceServiceLogger);
            }

            if (empNotificationServiceTimer != null)
            {
                empNotificationServiceTimer.Stop();
                ServiceLogger.Info("Order Status Service stopped for Employment Notification.", BkgOrderStatusServiceServiceLogger);
            }

            if (createBulkOrderServiceTimer != null)
            {
                createBulkOrderServiceTimer.Stop();
                ServiceLogger.Info("Create Bulk Order Service stopped.", BkgOrderStatusServiceServiceLogger);
            }

            if (createRepeatedBulkOrderServiceTimer != null)
            {
                createRepeatedBulkOrderServiceTimer.Stop();
                ServiceLogger.Info("Create Repeated Bulk Order Service stopped.", BkgOrderStatusServiceServiceLogger);
            }

            //UAT-3541 CBI ||CABS
            if (updateFingerPrintOrderTimer != null)
            {
                updateFingerPrintOrderTimer.Stop();
                ServiceLogger.Info("UpdateFingerPrintOrderService stopped.", FingerPrintOrderServiceServiceLogger);
            }

            //UAT - 3851
            if (ChangeStatusForUpdatedCBIResultFileTimer != null)
            {
                ChangeStatusForUpdatedCBIResultFileTimer.Stop();
                ServiceLogger.Info("ChangeStatusForUpdatedCBIResultFileTimer stopped.", ChangeStatusForUpdatedCBIResultFileServiceLogger);
            }

            
            #region CBI Fingerprint Reciept
            if (CBIFingerprintRecieptTimer != null)
            {
                CBIFingerprintRecieptTimer.Stop();
                ServiceLogger.Info("CBIFingerprintRecieptTimer stopped.", CBIFingerprintRecieptServiceLogger);
            }

            #endregion

            //CBI Result Files
            if (CBIResultFilesTimer != null)
            {
                CBIResultFilesTimer.Stop();
                ServiceLogger.Info("CBI Result Files Service stopped.", CBIResultFilesServiceServiceLogger);
            }
            //UAT-3826
            if (CDRImportDataTimer != null)
            {
                CDRImportDataTimer.Stop();
                ServiceLogger.Info("CDR Import Data Service stopped.", CDRImportDataServiceServiceServiceLogger);
            }

            //UAT-4162:- Timer for DS Order to get data from clearstar stopped
            if (getDSOrderDataTimer != null)
            {
                getDSOrderDataTimer.Stop();
                ServiceLogger.Info("GetDSOrderDataFromClearStarService stopped.", UpdateOrderServiceServiceLogger);
            }
            //UAT 4710
            if (notificationForFingerprintingExceededTATTimer != null)
            {
                notificationForFingerprintingExceededTATTimer.Stop();
                ServiceLogger.Info("NotificationForFingerprintingExceededTATTimer stopped", BkgOrderStatusServiceServiceLogger);
            }
        }

        protected override void OnPause()
        {
            if (createOrderTimer != null)
            {
                createOrderTimer.Stop();
                ServiceLogger.Info("CreateOrderService on Pause stopped.", CreateOrderServiceServiceLogger);
            }

            if (updateOrderTimer != null)
            {
                updateOrderTimer.Stop();
                ServiceLogger.Info("UpdateOrderService on Pause stopped.", UpdateOrderServiceServiceLogger);
            }

            if (bkgOrderStatusServiceTimer != null)
            {
                bkgOrderStatusServiceTimer.Stop();
                ServiceLogger.Info("Order Status Service on Pause stopped.", BkgOrderStatusServiceServiceLogger);
            }

            if (jiraTicketServiceTimer != null)
            {
                jiraTicketServiceTimer.Stop();
                ServiceLogger.Info("Jira Ticket Service Service Pause stopped.", BkgOrderStatusServiceServiceLogger);
            }

            if (empNotificationServiceTimer != null)
            {
                empNotificationServiceTimer.Stop();
                ServiceLogger.Info("Order Status Service on Pause stopped for Employment Notification.", BkgOrderStatusServiceServiceLogger);
            }

            if (createBulkOrderServiceTimer != null)
            {
                createBulkOrderServiceTimer.Stop();
                ServiceLogger.Info("Create Bulk Order Service Pause stopped.", BkgOrderStatusServiceServiceLogger);
            }

            if (createRepeatedBulkOrderServiceTimer != null)
            {
                createRepeatedBulkOrderServiceTimer.Stop();
                ServiceLogger.Info("Create Repeated Bulk Order Service Pause stopped.", BkgOrderStatusServiceServiceLogger);
            }

            //UAT-3541 CBI ||CABS
            if (updateFingerPrintOrderTimer != null)
            {
                updateFingerPrintOrderTimer.Stop();
                ServiceLogger.Info("UpdateFingerPrintOrderService Pause stopped.", FingerPrintOrderServiceServiceLogger);
            }
            //CBI Result Files
            if (CBIResultFilesTimer != null)
            {
                CBIResultFilesTimer.Stop();
                ServiceLogger.Info("CBI Result Files Service Pause stopped.", CBIResultFilesServiceServiceLogger);
            }
            //UAT-3826 CBI ||CABS
            if (CDRImportDataTimer != null)
            {
                CDRImportDataTimer.Stop();
                ServiceLogger.Info("CDR Import data Service Pause stopped.", CDRImportDataServiceServiceServiceLogger);
            }

            //UAT - 3851
            if (ChangeStatusForUpdatedCBIResultFileTimer != null)
            {
                ChangeStatusForUpdatedCBIResultFileTimer.Stop();
                ServiceLogger.Info("ChangeStatusForUpdatedCBIResultFileService Pause stopped.", ChangeStatusForUpdatedCBIResultFileServiceLogger);
            }
           
            #region CBI Fingerprint Reciept

            if (CBIFingerprintRecieptTimer != null)
            {
                CBIFingerprintRecieptTimer.Stop();
                ServiceLogger.Info("CBIFingerprintRecieptService Pause stopped.", CBIFingerprintRecieptServiceLogger);
            }
            #endregion

            //UAT-4162:- Timer for DS Order to get data from clearstar paused
            if (getDSOrderDataTimer != null)
            {
                getDSOrderDataTimer.Stop();
                ServiceLogger.Info("GetDSOrderDataFromClearStarService Pause stopped.", UpdateOrderServiceServiceLogger);
            }

            //UAT 4710
            if (notificationForFingerprintingExceededTATTimer != null)
            {
                notificationForFingerprintingExceededTATTimer.Stop();
                ServiceLogger.Info("NotificationForFingerprintingExceededTATTimer stopped", BkgOrderStatusServiceServiceLogger);
            }
        }

        protected override void OnContinue()
        {
            if (createOrderTimer != null)
            {
                createOrderTimer.Start();
                ServiceLogger.Info("CreateOrderService on Continue started.", CreateOrderServiceServiceLogger);
            }

            if (updateOrderTimer != null)
            {
                updateOrderTimer.Start();
                ServiceLogger.Info("UpdateOrderService on Continue started.", UpdateOrderServiceServiceLogger);
            }

            if (bkgOrderStatusServiceTimer != null)
            {
                bkgOrderStatusServiceTimer.Start();
                ServiceLogger.Info("Order Status Service on Continue started.", BkgOrderStatusServiceServiceLogger);
            }

            if (jiraTicketServiceTimer != null)
            {
                jiraTicketServiceTimer.Start();
                ServiceLogger.Info("Jira Ticket Service Service Continue started.", BkgOrderStatusServiceServiceLogger);
            }

            if (empNotificationServiceTimer != null)
            {
                empNotificationServiceTimer.Start();
                ServiceLogger.Info("Order Status Service on Continue started for Employment Notification.", BkgOrderStatusServiceServiceLogger);
            }

            if (createBulkOrderServiceTimer != null)
            {
                createBulkOrderServiceTimer.Start();
                ServiceLogger.Info("Create Bulk Order Service Continue started.", BkgOrderStatusServiceServiceLogger);
            }

            if (createRepeatedBulkOrderServiceTimer != null)
            {
                createRepeatedBulkOrderServiceTimer.Start();
                ServiceLogger.Info("Create Repeated Bulk Order Service Continue started.", BkgOrderStatusServiceServiceLogger);
            }

            //UAT-3541 CBI ||CABS
            if (updateFingerPrintOrderTimer != null)
            {
                updateFingerPrintOrderTimer.Start();
                ServiceLogger.Info("UpdateFingerPrintOrderService Continue started.", FingerPrintOrderServiceServiceLogger);
            }

            //CBI Result Files
            if (CBIResultFilesTimer != null)
            {
                CBIResultFilesTimer.Start();
                ServiceLogger.Info("CBI Result Files Service Continue started.", CBIResultFilesServiceServiceLogger);
            }
            //UAT-3826 CBI ||CABS
            if (CDRImportDataTimer != null)
            {
                CDRImportDataTimer.Start();
                ServiceLogger.Info("CDRImportDataService Continue started.", CDRImportDataServiceServiceServiceLogger);
            }

            //UAT - 3851
            if (ChangeStatusForUpdatedCBIResultFileTimer != null)
            {
                ChangeStatusForUpdatedCBIResultFileTimer.Start();
                ServiceLogger.Info("ChangeStatusForUpdatedCBIResultFileService Continue started.", ChangeStatusForUpdatedCBIResultFileServiceLogger);
            }

            #region CBI Fingerprint Reciept

            if (CBIFingerprintRecieptTimer != null)
            {
                CBIFingerprintRecieptTimer.Start();
                ServiceLogger.Info("CBIFingerprintRecieptService Continue started.", CBIFingerprintRecieptServiceLogger);
            }
            #endregion

            //UAT-4162:- Timer for DS Order to get data from clearstar continued
            if (getDSOrderDataTimer != null)
            {
                getDSOrderDataTimer.Start();
                ServiceLogger.Info("GetDSOrderDataFromClearStarService Continue started.", UpdateOrderServiceServiceLogger);
            }

            //UAT 4710
            if (notificationForFingerprintingExceededTATTimer != null)
            {
                notificationForFingerprintingExceededTATTimer.Start();
                ServiceLogger.Info("NotificationForFingerprintingExceededTATTimer started", BkgOrderStatusServiceServiceLogger);
            }
        }

        protected override void OnShutdown()
        {
            if (createOrderTimer != null)
            {
                createOrderTimer.Stop();
                ServiceLogger.Info("CreateOrderService on shut down stopped.", CreateOrderServiceServiceLogger);
            }

            if (updateOrderTimer != null)
            {
                updateOrderTimer.Stop();
                ServiceLogger.Info("UpdateOrderService on shut down stopped.", UpdateOrderServiceServiceLogger);
            }

            if (bkgOrderStatusServiceTimer != null)
            {
                bkgOrderStatusServiceTimer.Stop();
                ServiceLogger.Info("Order Status Service on shut down stopped.", BkgOrderStatusServiceServiceLogger);
            }

            if (jiraTicketServiceTimer != null)
            {
                jiraTicketServiceTimer.Stop();
                ServiceLogger.Info("Jira Ticket Service Service on shut down stopped.", BkgOrderStatusServiceServiceLogger);
            }

            if (empNotificationServiceTimer != null)
            {
                empNotificationServiceTimer.Stop();
                ServiceLogger.Info("Order Status Service on shut down stopped for Employment Notification.", BkgOrderStatusServiceServiceLogger);
            }

            if (createBulkOrderServiceTimer != null)
            {
                createBulkOrderServiceTimer.Stop();
                ServiceLogger.Info("Create Bulk Order Service on shut down stopped.", BkgOrderStatusServiceServiceLogger);
            }

            if (createRepeatedBulkOrderServiceTimer != null)
            {
                createRepeatedBulkOrderServiceTimer.Stop();
                ServiceLogger.Info("Create Repeated Bulk Order Service on shut down stopped.", BkgOrderStatusServiceServiceLogger);
            }

            //UAT-3541 CBI ||CABS
            if (updateFingerPrintOrderTimer != null)
            {
                updateFingerPrintOrderTimer.Stop();
                ServiceLogger.Info("UpdateFingerPrintOrderService on shut down stopped.", FingerPrintOrderServiceServiceLogger);
            }

            //CBI Result Files
            if (CBIResultFilesTimer != null)
            {
                CBIResultFilesTimer.Stop();
                ServiceLogger.Info("CBI Result Files service on shut down stopped.", CBIResultFilesServiceServiceLogger);
            }
            //UAT-3826 CBI ||CABS
            if (CDRImportDataTimer != null)
            {
                CDRImportDataTimer.Stop();
                ServiceLogger.Info("CDRImportDataService Continue started.", CDRImportDataServiceServiceServiceLogger);
            }

            //UAT- 3851
            if (ChangeStatusForUpdatedCBIResultFileTimer != null)
            {
                ChangeStatusForUpdatedCBIResultFileTimer.Stop();
                ServiceLogger.Info("ChangeStatusForUpdatedCBIResultFileService on shut down stopped.", ChangeStatusForUpdatedCBIResultFileServiceLogger);
            }

            #region CBI Fingerprint Reciept

            if (CBIFingerprintRecieptTimer != null)
            {
                CBIFingerprintRecieptTimer.Stop();
                ServiceLogger.Info("CBIFingerprintRecieptService on shut down stopped.", CBIFingerprintRecieptServiceLogger);
            }
            #endregion

            //UAT-4162:- Timer for DS Order to get data from clearstar is shutdowned
            if (getDSOrderDataTimer != null)
            {
                getDSOrderDataTimer.Stop();
                ServiceLogger.Info("GetDSOrderDataFromClearStarService on shut down stopped.", UpdateOrderServiceServiceLogger);
            }

            //UAT 4710
            if (notificationForFingerprintingExceededTATTimer != null)
            {
                notificationForFingerprintingExceededTATTimer.Stop();
                ServiceLogger.Info("NotificationForFingerprintingExceededTATTimer on shut down stopped", BkgOrderStatusServiceServiceLogger);
            }

        }

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        private Double firstTimerInterval(TimeSpan startTime, TimeSpan EndTime, TimeSpan currentTime, Int32 multiplier)
        {
            double firstInterval = 0;
            if (currentTime >= startTime && currentTime <= EndTime)
            {
                firstInterval = 1000 * multiplier;
            }
            else
            {
                firstInterval = (startTime - currentTime).TotalMilliseconds;
                if (firstInterval < 0)
                {
                    firstInterval = NextTimeSpanSeconds + firstInterval;
                }
            }
            return firstInterval;
        }

        #endregion

        #endregion
    }
}
