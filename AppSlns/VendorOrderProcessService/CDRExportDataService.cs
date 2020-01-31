using System;
using Business.RepoManagers;
using NLog;
using RestSharp;
using System.ServiceProcess;
using System.Net;
using System.Configuration;
using System.IO;
using System.Text;
using INTSOF.Utils;
using ExternalVendors;
using Entity;
using System.Collections.Generic;
using System.Linq;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.Services;
using WinSCP;
using System.Web.Configuration;
using INTSOF.UI.Contract.FingerPrintSetup;

namespace VendorOrderProcessService
{
  public static class CDRExportDataService
    {
        #region Variables

        private static String CDRImportDataServiceLogger;
        private static Boolean _isServiceLoggingEnabled;
        private static string CDRImportUserName;
        private static string CDRImportPassword;
        private static string CDRImportDataUrl;

        #endregion
        static CDRExportDataService()
        {
            CDRImportDataServiceLogger = "CDRImportDataServiceLogger";

            if (ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull())
            {
                _isServiceLoggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]);
            }
            else
            {
                _isServiceLoggingEnabled = false;
            }

            if(ConfigurationManager.AppSettings["CDRImportUserName"].IsNotNull())
            {
                CDRImportUserName = Convert.ToString(ConfigurationManager.AppSettings["CDRImportUserName"]);
            }
            if (ConfigurationManager.AppSettings["CDRImportPassword"].IsNotNull())
            {
                CDRImportPassword = Convert.ToString(ConfigurationManager.AppSettings["CDRImportPassword"]);
            }
            
           if (ConfigurationManager.AppSettings["CDRImportDataUrl"].IsNotNull())
            {
                CDRImportDataUrl = Convert.ToString(ConfigurationManager.AppSettings["CDRImportDataUrl"]);
            }       

        }

        /// <summary>
        /// Gets Call Center data from Mitel and exports to a local table
        /// </summary>
        /// <param name="tenant_Id"></param>
        public static void CDRImportDataToTable()
        {
            try
            {
                // ServiceContext.init();
                ServiceLogger.Info("Calling CDRImportData : " + DateTime.Now.ToString(), CDRImportDataServiceLogger);

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                item =>
                {
                    ClientDBConfiguration config = new ClientDBConfiguration();
                    config.CDB_TenantID = item.CDB_TenantID;
                    config.CDB_ConnectionString = item.CDB_ConnectionString;
                    config.Tenant = new Entity.Tenant();
                    config.Tenant.TenantName = item.Tenant.TenantName; return config;
                }).ToList();

                ServiceLogger.Debug<List<ClientDBConfiguration>>("CDRImportData: List of Client DbConfigurations from database:", clientDbConfs, CDRImportDataServiceLogger);
                ServiceLogger.Info("CDRImportData: Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), CDRImportDataServiceLogger);
                var lcoationtenants = SecurityManager.GetListOfTenantWithLocationService().Select(x => x.TenantID);
                var loctiontenantconfig = clientDbConfs.Where(x => lcoationtenants.Contains(x.CDB_TenantID));
                foreach (ClientDBConfiguration cltDbConf in loctiontenantconfig)
                {
                    ServiceLogger.Info("CDRExportData: Check if location service available Tenant: " + DateTime.Now.ToString(), CDRImportDataServiceLogger);
                    if (!cltDbConf.IsNullOrEmpty() && SecurityManager.IsLocationServiceTenant(cltDbConf.CDB_TenantID))
                    {
                        if (CheckDBConnection.TestConnString(cltDbConf.CDB_ConnectionString, CDRImportDataServiceLogger))
                        {
                            DateTime jobStartTime = DateTime.Now;
                            DateTime jobEndTime;
                            Int32 tenantId = cltDbConf.CDB_TenantID;
                            ServiceLogger.Info("CDRImportData: Started exporting data from api and saving in table at. " + DateTime.Now.ToString(),
                                        CDRImportDataServiceLogger);

                            ImportDataToTable(tenantId, backgroundProcessUserId);

                            ServiceLogger.Info("CDRImportData: End exporting data from api and saving in table at. " + DateTime.Now.ToString(),
                                CDRImportDataServiceLogger);

                            //Save service logging data to DB
                            if (_isServiceLoggingEnabled)
                            {
                                jobEndTime = DateTime.Now;
                                ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                                serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                                serviceLoggingContract.JobName = JobName.CDRExportData.GetStringValue();
                                serviceLoggingContract.TenantID = tenantId;
                                serviceLoggingContract.JobStartTime = jobStartTime;
                                serviceLoggingContract.JobEndTime = jobEndTime;
                                //serviceLoggingContract.Comments = "";
                                serviceLoggingContract.IsDeleted = false;
                                serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                                serviceLoggingContract.CreatedOn = DateTime.Now;
                                SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                            }
                        }
                    }
                    ServiceContext.ReleaseDBContextItems();
                }
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in CDRImportDataToTable method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                    CDRImportDataServiceLogger);
            }
        }

        private static void ImportDataToTable(int? tenantId,int? backgroundProcessUserId)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Int64 lastId = FingerPrintDataManager.GetLastRecordInsertedId();
                TimeZoneInfo timeZoneInfo;
                DateTime startDateTime;
                DateTime endDateTime;
                //Set the time zone information to Eastern Standard Time 
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                //Get date and time in Eastern Standard Time
                startDateTime = TimeZoneInfo.ConvertTime(DateTime.Now.AddDays(-7), timeZoneInfo);
                endDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                string startDate= startDateTime.ToString("yyyy-MM-dd");
                string endDate = endDateTime.ToString("yyyy-MM-dd");
                var client = new RestClient();

                string credentialsForAuth = Base64Encode(CDRImportUserName, CDRImportPassword);
                var url = String.Format(CDRImportDataUrl + "startDate={0}&endDate={1}", startDate, endDate);
                if (lastId > 0)
                {
                   url=  String.Format(url + "&lastId={0}", lastId);
                    client = new RestClient(url);
                }
                else
                {
                    client = new RestClient(url);
                }

                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", "Basic " + credentialsForAuth);
                IRestResponse response = null;
                try
                {
                    response = client.Execute(request);   
                }
                catch (Exception ex)
                {
                    ServiceLogger.Error(String.Format("Exception in getting CDR export from client, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                                           ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                                           CDRImportDataServiceLogger);
                }
                CDRFileDetailContract fileDetailContract = new CDRFileDetailContract();
                if (response.IsNotNull())
                {
                    string ReadCSV = response.Content;
                    if (response.Content.IsNullOrEmpty())
                    {
                        return;
                        
                    }
                    String filePath = String.Empty;
                    Boolean aWSUseS3 = false;
                    StringBuilder corruptedFileMessage = new StringBuilder();
                    String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                    filePath = WebConfigurationManager.AppSettings[AppConsts.CDR_EXPORT_FILE_LOCATION];
                    StringBuilder docMessage = new StringBuilder();
                    byte[] fileBytes = null;
                    String applicantDocPath = String.Empty;
                    String FinalFileName = String.Empty;

                    if (!WebConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                    {
                        aWSUseS3 = Convert.ToBoolean(WebConfigurationManager.AppSettings["AWSUseS3"]);
                    }
                    //Check whether use AWS S3, true if need to use
                    if (aWSUseS3 == false)
                    {
                        if (!filePath.EndsWith("\\"))
                        {
                            filePath += "\\";
                        }

                        filePath += "CDRExportedFiles";

                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);
                    }
                    else
                    {
                        if (!filePath.EndsWith("//"))
                        {
                            filePath += "//";
                        }

                        filePath = filePath + "CDRExportedFiles";
                    }

                    String fileName = "CDRExport" + "_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Minute + DateTime.Now.Millisecond+ ".csv";
                    String newTempFilePath = Path.Combine(tempFilePath, "CDRExportedFiles"); // Created Temporary File Path
                    if (!Directory.Exists(newTempFilePath))
                        Directory.CreateDirectory(newTempFilePath);


                    newTempFilePath = Path.Combine(newTempFilePath, fileName);
                    File.WriteAllText(newTempFilePath, response.Content);
                    fileBytes = File.ReadAllBytes(newTempFilePath);

                    if (!fileBytes.IsNullOrEmpty())
                    {
                        //File.WriteAllBytes(newTempFilePath, sftpFileBytes);

                        if (aWSUseS3 == false)
                        {
                            //Move file to other location
                            String destFilePath = Path.Combine(filePath, FinalFileName);
                            File.Copy(newTempFilePath, destFilePath);
                            applicantDocPath = destFilePath;
                        }

                        else
                        {
                            if (!filePath.EndsWith("//"))
                            {
                                filePath += "//";
                            }
                            //AWS code to save document to S3 location
                            AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                            String destFolder = filePath ;
                            String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, fileName, destFolder);
                            if (returnFilePath.IsNullOrEmpty())
                            {
                                corruptedFileMessage.Append("Your file " + fileName + " is not uploaded. \\n");
                            }
                            applicantDocPath = returnFilePath;
                        }

                        try
                        {
                            if (!String.IsNullOrEmpty(newTempFilePath))
                                File.Delete(newTempFilePath);
                        }
                        catch (Exception) { }
                        
                        fileDetailContract.FileName = fileName;
                        fileDetailContract.FilePath = applicantDocPath;
                        fileDetailContract.FileCreatedDate = DateTime.Now;
                        
                    }
                    FingerPrintDataManager.ImportDataToTable(ReadCSV,fileDetailContract,backgroundProcessUserId);
                }
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in ImportDataToTable method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                        ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                        CDRImportDataServiceLogger);
            }
          
        }
        private static string Base64Encode(string username, string password)
        {
            var credentialsinByte = System.Text.Encoding.UTF8.GetBytes(username+":"+ password);
            return System.Convert.ToBase64String(credentialsinByte);
        }
     
    }
}
