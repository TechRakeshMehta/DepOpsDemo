using System;
using System.Configuration;
using INTSOF.Complio.API.ReportExecution;

namespace INTSOF.Complio.API.ComplioAPIBusiness
{
    public class ComplioApiReportManager
    {
        public static byte[] GetReportByteArray(String reportName, ParameterValue[] reportParameters)
        {
            byte[] reportContent = new byte[1];
            try
            {
                if (reportParameters != null && reportParameters.Length > 0)
                {
                    using (ReportExecutionService client = new ReportExecutionService())
                    {
                        //username
                        string userName = ConfigurationManager.AppSettings["ReportViewerUser"];

                        if (string.IsNullOrEmpty(userName))
                            throw new Exception("Missing user name from web.config file");

                        // Password
                        string password = ConfigurationManager.AppSettings["ReportViewerPassword"];

                        if (string.IsNullOrEmpty(password))
                            throw new Exception("Missing password from web.config file");

                        // Domain
                        string domain = ConfigurationManager.AppSettings["ReportViewerDomain"];

                        if (string.IsNullOrEmpty(domain))
                            throw new Exception("Missing domain from web.config file");

                        client.Credentials = new System.Net.NetworkCredential(userName, password, domain);

                        string reportPath = String.Empty;

                        if (ConfigurationManager.AppSettings["ReportPath"] != null && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["ReportPath"].ToString()))
                        {
                            reportPath = "/" + ConfigurationManager.AppSettings["ReportPath"] + "/" + reportName;
                        }

                        string format = "PDF";
                        string historyID = null;
                        string devInfo = @"<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";
                        string extension = null;
                        string mimeType = null;
                        string encoding = null;
                        Warning[] warnings = null;
                        string[] streamIDs = null;

                        client.ExecutionHeaderValue = new ExecutionHeader();


                        ExecutionInfo execInfo = client.LoadReport(reportPath, historyID);
                        client.SetExecutionParameters(reportParameters, "en-us");

                        reportContent = client.Render(format, devInfo, out extension, out encoding, out mimeType, out warnings, out streamIDs);
                    }
                    return reportContent;
                }
                else
                    return null;
            }
            catch
            {
                throw;
            }
        }
    }
}