using INTSOF.ServiceUtil;
using INTSOF.Utils.Consts;
using NLog;
using System;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Timers;

namespace DataFeedAPIService
{
    public partial class DataFeed : ServiceBase
    {
        #region Variables

        #region Public Variables
        #endregion

        #region Private Variables


        private static Logger nlogger;
        private Timer dataFeedTimer = null;
        private Int32 _dataFeedInterval = 3600000;
        private Int32 _dataFeedFromHour = 0;
        private Int32 _dataFeedFromMinute = 0;
        private Int32 _dataFeedToHour = 24;
        private Int32 _dataFeedToMinute = 0;
        private Object lockObject = new Object();
        private Boolean IsDataFeedServiceInterval = false;
        #endregion
        #endregion

        #region Properties

        #region Public Properties

        public Int32 DataFeedInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DataFeedInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DataFeedInterval"])
                        ? ConfigurationManager.AppSettings["DataFeedInterval"] : _dataFeedInterval.ToString());
                else
                    return _dataFeedInterval;
            }
        }

        public Int32 DataFeedFromHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DataFeedFromHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DataFeedFromHour"])
                                ? ConfigurationManager.AppSettings["DataFeedFromHour"] : _dataFeedFromHour.ToString());
                }
                else
                {
                    return _dataFeedFromHour;
                }
            }
        }

        public Int32 DataFeedFromMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DataFeedFromMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DataFeedFromMinute"])
                        ? ConfigurationManager.AppSettings["DataFeedFromMinute"] : _dataFeedFromMinute.ToString());
                }
                else
                {
                    return _dataFeedFromMinute;
                }
            }
        }

        public Int32 DataFeedToHour
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DataFeedToHour"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DataFeedToHour"])
                        ? ConfigurationManager.AppSettings["DataFeedToHour"] : _dataFeedToHour.ToString());
                }
                else
                {
                    return _dataFeedToHour;
                }
            }
        }

        public Int32 DataFeedToMinute
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("DataFeedToMinute"))
                {
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DataFeedToMinute"])
                        ? ConfigurationManager.AppSettings["DataFeedToMinute"] : _dataFeedToMinute.ToString());
                }
                else
                {
                    return _dataFeedToMinute;
                }
            }
        }

        public TimeSpan DataFeedServiceStartTime
        {
            get
            {
                return new TimeSpan(DataFeedFromHour, DataFeedFromMinute, 0);
            }
        }

        public TimeSpan DataFeedServiceEndTime
        {
            get
            {
                return new TimeSpan(DataFeedToHour, DataFeedToHour, 0);
            }
        }

        public Double NextTimeSpanSeconds
        {
            get
            {
                return 86400000;
            }
        }

        #endregion
        #endregion

        #region Service Events

        public DataFeed()
        {
            nlogger = LogManager.GetLogger("DataFeedServiceLogger");
            TimeSpan CurrentTime = DateTime.Now.TimeOfDay;
            ServiceContext.init();
            InitializeComponent();

            nlogger.Info("Initialize Timer for Data Feed.");
            if (DataFeedInterval != SysXDBConsts.MINUS_ONE)
            {
                nlogger.Info("Initializing Timer... for Data Feed.");
                dataFeedTimer = new Timer();
                //Convert.ToDouble(DataFeedInterval);
                dataFeedTimer.Interval = (firstTimerInterval(DataFeedServiceStartTime, DataFeedServiceEndTime, CurrentTime, 1));
                dataFeedTimer.Elapsed += new ElapsedEventHandler(dataFeedTimer_Elapsed);
                nlogger.Info("Timer.. elapsed for Data Feed.");
            }

        }

        private Boolean isDataFeedServiceExecuting = false;

        void dataFeedTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!IsDataFeedServiceInterval)
            {
                dataFeedTimer.Interval = Convert.ToDouble(DataFeedInterval);
                IsDataFeedServiceInterval = true;
            }

            lock (this.lockObject)
            {
                if (isDataFeedServiceExecuting)
                    return;
                isDataFeedServiceExecuting = true;
            }
            nlogger.Info("Starting Data Feed API process..");

            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            try
            {
                if (currentTime >= DataFeedServiceStartTime && currentTime <= DataFeedServiceEndTime)
                {
                    nlogger.Info("Timer invoked for creating Data Feed.");
                    DataFeedService.StartDataFeedService();
                }
                else
                {
                    nlogger.Info("Timer does not invoked as current time is out of start time and End time.");
                }
            }
            catch (Exception ex)
            {
                isDataFeedServiceExecuting = false;
                nlogger.Error(String.Format("An Error has occured in while Data Feed API Service, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString));
            }
            finally
            {
                ServiceContext.ReleaseContextItems();
                isDataFeedServiceExecuting = false;
            }
        }

        protected override void OnStart(string[] args)
        {
            if (dataFeedTimer != null)
            {
                dataFeedTimer.Enabled = true;
                dataFeedTimer.Start();
                nlogger.Info("Data Feed API Service started.");
            }
        }

        protected override void OnStop()
        {
            if (dataFeedTimer != null)
            {
                dataFeedTimer.Stop();
                nlogger.Info("Data Feed API Service stopped.");
            }
        }

        protected override void OnPause()
        {
            if (dataFeedTimer != null)
            {
                dataFeedTimer.Stop();
                nlogger.Info("Data Feed API Service paused.");
            }
        }

        protected override void OnContinue()
        {
            if (dataFeedTimer != null)
            {
                dataFeedTimer.Start();
                nlogger.Info("Data Feed API Service On-Continue started.");
            }
        }

        protected override void OnShutdown()
        {
            if (dataFeedTimer != null)
            {
                nlogger.Info("Data Feed API Service OnShut Down stopped.");
                dataFeedTimer.Stop();
            }
        }

        #endregion

        #region Methods

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
