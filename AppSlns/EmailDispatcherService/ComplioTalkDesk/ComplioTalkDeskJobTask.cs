using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmailDispatcherService.ComplioTalkDesk
{
    public class ComplioTalkDeskJobTask
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static string ComplioTalkDeskApiBaseUrl
        {
            get
            {
                if ((ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskApiBaseUrl"))
                    && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskApiBaseUrl"]))
                    return ConfigurationManager.AppSettings["ComplioTalkDeskApiBaseUrl"];
                else
                    return string.Empty;
            }
        }

        private static string ComplioTalkDeskCreateReportJobApiUrl
        {
            get
            {
                if ((ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskCreateReportJobApiUrl"))
                    && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskCreateReportJobApiUrl"]))
                    return ConfigurationManager.AppSettings["ComplioTalkDeskCreateReportJobApiUrl"];
                else
                    return string.Empty;
            }
        }

        private static string ComplioTalkDeskUpdateReportJobApiUrl
        {
            get
            {
                if ((ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskUpdateReportJobApiUrl"))
                    && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskUpdateReportJobApiUrl"]))
                    return ConfigurationManager.AppSettings["ComplioTalkDeskUpdateReportJobApiUrl"];
                else
                    return string.Empty;
            }
        }
                
        public static void CreateTalkDeskJobs()
        {
            try
            {
                if (String.IsNullOrEmpty(ComplioTalkDeskCreateReportJobApiUrl))
                {
                    throw new Exception("ComplioTalkDeskCreateReportJobApiUrl is empty in app configuration file.");
                }

                // Create an HttpClient instance
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(ComplioTalkDeskApiBaseUrl);
                HttpResponseMessage _apiResponse = new HttpResponseMessage();
                _apiResponse = client.GetAsync(ComplioTalkDeskCreateReportJobApiUrl).Result;                
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in CreateTalkDeskJobs, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", 
                    ex.Message, ex.InnerException, ex.StackTrace);
            }

        }

        public static void UpdateTalkDeskJobs()
        {
            try
            {
                if (String.IsNullOrEmpty(ComplioTalkDeskUpdateReportJobApiUrl))
                {
                    throw new Exception("ComplioTalkDeskUpdateReportJobApiUrl is empty in app configuration file.");
                }
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(ComplioTalkDeskApiBaseUrl);
                HttpResponseMessage _apiResponse = new HttpResponseMessage();
                _apiResponse = client.GetAsync(ComplioTalkDeskUpdateReportJobApiUrl).Result;
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in UpdateTalkDeskJobs, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                    ex.Message, ex.InnerException, ex.StackTrace);
            }
        }
    }
}
