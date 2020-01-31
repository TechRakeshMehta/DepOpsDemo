using Business.RepoManagers;
using System;
using System.Collections.Generic;
using System.ServiceModel.Activation;

namespace WidgetDataWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DataFeed" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DataFeed.svc or DataFeed.svc.cs at the Solution Explorer and start debugging 
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DataFeed : IDataFeed
    {

        
        /// <summary>
        /// return Xml data 
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        /// <param name="settingID">settingID</param>
        /// <param name="formatID">formatID</param>
        /// <param name="accessKey">accessKey</param>
        /// <param name="recordOriginStartDate">recordOriginStartDate</param>
        /// <param name="recordOriginEndDate">recordOriginEndDate</param>
        /// <returns></returns>
        public Dictionary<String,String> GetXmlData(Int32 tenantID, Int32 settingID, Guid accessKey, String fromDate, String toDate)
        {
            Boolean isAuthenticated = false;
            DateTime startDate;
            DateTime endDate;
            var res = new Dictionary<String, String>();
            try
            {
                startDate = Convert.ToDateTime(fromDate);
                endDate = Convert.ToDateTime(toDate);
                isAuthenticated = DataFeedManager.IsDataFeedAccessSettingAuthenticated(tenantID, settingID, accessKey);
            }
            catch (FormatException ex)
            {
                res.Add("Result", "INVALID");
                res.Add("Text", ex.Message);
                return res;
            }
            catch (Exception ex)
            {                
                res.Add("Result", "INVALID");
                res.Add("Text", "An  error occured while authenticating your request. Please cross check supplied parameters.");
                return res;
            }

            try
            {
                if (isAuthenticated)
                {
                    return DataFeedManager.GetDataFeedXml(tenantID, settingID, startDate, endDate);
                }
                else
                {
                    res.Add("Result", "INVALID");
                    res.Add("Text", "Invalid Parameters");
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Add("Result", "INVALID");
                res.Add("Text", "An error occured while processing your request. Please check your supplied setting and date range.");
                return res;
            }
        }

    }

}
