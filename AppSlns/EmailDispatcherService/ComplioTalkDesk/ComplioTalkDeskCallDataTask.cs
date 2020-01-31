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
    public class ComplioTalkDeskCallDataTask
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

        private static string ComplioTalkDeskPullCallDataJobApiUrl
        {
            get
            {
                if ((ConfigurationManager.AppSettings.AllKeys.Contains("ComplioTalkDeskPullCallDataJobApiUrl"))
                    && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplioTalkDeskPullCallDataJobApiUrl"]))
                    return ConfigurationManager.AppSettings["ComplioTalkDeskPullCallDataJobApiUrl"];
                else
                    return string.Empty;
            }
        }

        public static void GetJobCallData()
        {
            try
            {
                if (String.IsNullOrEmpty(ComplioTalkDeskPullCallDataJobApiUrl))
                {
                    throw new Exception("ComplioTalkDeskPullCallDataJobApiUrl is empty in app configuration file.");
                }
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(ComplioTalkDeskApiBaseUrl);
                HttpResponseMessage _apiResponse = new HttpResponseMessage();
                _apiResponse = client.GetAsync(ComplioTalkDeskPullCallDataJobApiUrl).Result;
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in GetJobCallData, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                    ex.Message, ex.InnerException, ex.StackTrace);
            }
        }
    }
}
