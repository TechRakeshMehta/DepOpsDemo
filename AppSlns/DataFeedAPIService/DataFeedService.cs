#region Header Comment Block

// 
// Copyright 2014 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  DataFeedService.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;


#endregion

#region Application Specific

using INTSOF.Utils;
using Business.RepoManagers;
using INTSOF.UI.Contract.DataFeed_Framework;
using Entity;
using System.IO;
using WinSCP;
using Business.DataFeed_Framework.DataFeed_Formatter;
using INTSOF.ServiceUtil;
using DataFeedAPIService.DataFeedUtils;
using System.Net.Mail;
using System.Text;
using System.Net.Configuration;

#endregion

#endregion

namespace DataFeedAPIService
{
    public static class DataFeedService
    {
        #region Variables

        #region Public Variables
        public static List<DataFeedInvokeHistoryDetail> _lstInvokeHistorydetail = null;
        #endregion

        #region Private Variables

        private static Logger logger;
        private static Int32 dataFeedSvcUserID;

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        private static Int32 _dataFeedInterval = 3600000;

        #endregion

        #region Public Properties

        public static Int32 DataFeedInterval
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

        #endregion

        #endregion

        #region Constructor

        static DataFeedService()
        {
            _lstInvokeHistorydetail = new List<DataFeedInvokeHistoryDetail>();
            logger = LogManager.GetLogger("DataFeedServiceLogger");
            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.DATA_FEED_SERVICE_USER_ID);
            dataFeedSvcUserID = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.DATA_FEED_SERVICE_USER_ID_VALUE;
        }

        #endregion

        #region Methods

        #region Public Methods

        public static void StartDataFeedService(Int32? tenant_Id = null)
        {
            logger.Debug("Data Feed Service Started.");
            List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
            item =>
            {
                ClientDBConfiguration config = new ClientDBConfiguration();
                config.CDB_TenantID = item.CDB_TenantID;
                config.CDB_ConnectionString = item.CDB_ConnectionString;
                config.CDB_DBName = item.CDB_DBName;
                config.Tenant = new Entity.Tenant();
                config.Tenant.TenantName = item.Tenant.TenantName;
                return config;
            }).ToList();

            if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
            {
                logger.Debug("TenantID for which external vendor orders are to be dispatched: ", tenant_Id);
                clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
            }

            foreach (ClientDBConfiguration tnt in clientDbConfs)
            {
                try
                {
                    if (SecurityManager.IsTenantActive(Convert.ToInt32(tnt.CDB_TenantID)))
                    {
                        logger.Debug("****** Invoking Data Feed for: " + tnt.CDB_DBName + "*****");
                        InvokeDataFeed(tnt);
                        logger.Debug("Data Feed for: " + tnt.CDB_DBName + " completed.");
                    }
                    else
                    {
                        logger.Debug("Tenant: " + tnt.CDB_DBName + " is not currently active.");
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(String.Format("An Error has occured in while Start Data Feed Service the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                          ex.Message, ex.InnerException, ex.StackTrace));
                }

                //Release DB Context after each Tenant Data Feed.
                ServiceContext.ReleaseDBContextItems();
            }
            logger.Debug("Data Feed Service Ended.");
        }


        #endregion

        #region Private Methods

        private static void InvokeDataFeed(ClientDBConfiguration tnt)
        {
            try
            {
                List<DataFeedSettingContract> dataFeedSettingContractList = SecurityManager.GetDataFeedSetting(tnt.CDB_TenantID);

                if (dataFeedSettingContractList.IsNotNull() && dataFeedSettingContractList.Count() > 0)
                {
                    dataFeedSettingContractList.ForEach(dataFeedSettings =>
                    {
                        try
                        {
                            if (dataFeedSettings.IsNotNull())
                            {
                                dataFeedSettings.TenantDBName = tnt.CDB_DBName;
                                dataFeedSettings.TenantName = tnt.Tenant.TenantName;
                                logger.Debug("Invoking Date Feed Service for Tenant " + tnt.CDB_DBName + " and DataFeedSettingID " + dataFeedSettings.DataFeedSettingId);
                                logger.Debug("Tenant " + tnt.CDB_DBName + ". TenantID: " + tnt.CDB_TenantID);
                                logger.Debug("Current Date Time " + DateTime.Now.ToString());
                                logger.Debug("Last Data Feed Invoked Date Time " + Convert.ToString(dataFeedSettings.DataFeedInvokeDateTime));
                                logger.Debug("Service Interval Month " + dataFeedSettings.ServiceIntervalMonth);
                                logger.Debug("Service Interval Days " + dataFeedSettings.ServiceIntervalDays);

                                /*.AddMilliseconds(DataFeedInterval))
                                 * we have added DataFeedInterval milliseconds in invoke data time to minimize the gap between 
                                */
                                if ((dataFeedSettings.DataFeedInvokeDateTime.IsNull()) || (dataFeedSettings.DataFeedInvokeDateTime.IsNotNull()
                                     && (dataFeedSettings.DataFeedInvokeDateTime.Value.AddMonths(dataFeedSettings.ServiceIntervalMonth)
                                                                                      .AddDays(dataFeedSettings.ServiceIntervalDays))
                                       <= DateTime.Now.AddMilliseconds(DataFeedInterval)))
                                {
                                    logger.Debug("Data Feed for" + tnt.CDB_DBName + " invoked as per Data Feed Settings.");
                                    //Run Service for Tenant
                                    if (dataFeedSettings.DataFeedIntervalModeCode == DataFeedIntervalMode.NoofDays.GetStringValue())
                                    {
                                        logger.Debug("Data Feed Interval Mode: " + Convert.ToString(DataFeedIntervalMode.NoofDays));
                                        dataFeedSettings.RecordOriginEnddate = DateTime.Now;
                                        DateTime _recordOriginStartDate = DateTime.Now.AddDays(-Convert.ToInt32(dataFeedSettings.NoOfDays));
                                        dataFeedSettings.RecordOriginStartDate = Convert.ToDateTime(_recordOriginStartDate);
                                    }
                                    else if (dataFeedSettings.DataFeedIntervalModeCode == DataFeedIntervalMode.NoOfMonthDays.GetStringValue())
                                    {
                                        logger.Debug("Data Feed Interval Mode: " + Convert.ToString(DataFeedIntervalMode.NoOfMonthDays));
                                        dataFeedSettings.RecordOriginEnddate = DateTime.Now;
                                        DateTime _recordOriginStartDate = DateTime.Now.AddMonths(-Convert.ToInt32(dataFeedSettings.NoOfMonths))
                                                                                      .AddDays(-Convert.ToInt32(dataFeedSettings.NoOfDays));
                                        dataFeedSettings.RecordOriginStartDate = Convert.ToDateTime(_recordOriginStartDate);
                                    }
                                    else if (dataFeedSettings.DataFeedIntervalModeCode == DataFeedIntervalMode.CurrentMonth.GetStringValue())
                                    {
                                        logger.Debug("Data Feed Interval Mode: " + Convert.ToString(DataFeedIntervalMode.CurrentMonth));
                                        dataFeedSettings.RecordOriginStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                                        dataFeedSettings.RecordOriginEnddate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                                    }
                                    else
                                    {
                                        logger.Debug("Data Feed Interval Mode is empty or not supported. Please specify supported Interval Mode. " +
                                                     " Please refer DataFeedAPI document for supported mode.");
                                        return;
                                    }
                                }
                                else
                                {
                                    logger.Debug("Data Feed for" + tnt.CDB_DBName + " does not invoked as per Data Feed Settings."
                                                + " Please verify Data Feed Service Interval Days and Last Invoked Date time.");
                                    return;
                                }

                                //SetDataFeedInvokeHistoryData(dataFeedSettings);

                                ////Get settings and access key for the tenant.
                                //DataFeedSettingContract dataFeedSettingForFileUpload = GetDataFeedSetting(Convert.ToInt32(tnt.CDB_TenantID), recordOriginStartDate, recordOriginEnddate);

                                //Get the formatted data feed xml. 
                                String dataFeedCSVFormattedContent = GetDataFeedCSVFormattedContent(dataFeedSettings);

                                String remotePath = string.Empty;
                                var dataFeedInvokeResult = LookupManager.GetLookUpData<lkpDataFeedInvokeResult>();

                                DataFeedFTPDetail ftpDetail = SecurityManager.GetDataFeedFTPDetail(dataFeedSettings.DataFeedFTPDetailID);

                                //Call upload method to upload the data feed file on SFTP.
                                if (DeliverDataFeedDocumentToClient(dataFeedSettings, dataFeedCSVFormattedContent, ftpDetail))
                                {
                                    if (!String.IsNullOrEmpty(dataFeedCSVFormattedContent))
                                    {
                                        SendDataFeedReportingEmails(dataFeedSettings, ftpDetail.DocumentFolderPath, true);
                                    }
                                    if (dataFeedInvokeResult.Any(cond => cond.Code == DataFeedInvokeResult.Passed.GetStringValue()))
                                    {
                                        logger.Debug("Delivery of Data Feed document passed. Saving data into Invoke History and Invoke History Details.");
                                        Int32 dataFeedInvokeResultPassedID = dataFeedInvokeResult.Where(cond => cond.Code == DataFeedInvokeResult.Passed.GetStringValue())
                                                                             .FirstOrDefault().DataFeedInvokeResultID;
                                        SecurityManager.SaveLogInInvokeHistory(dataFeedSettings, _lstInvokeHistorydetail, dataFeedSvcUserID, dataFeedInvokeResultPassedID);
                                        logger.Debug("Deliver Data Feed Document Success.");
                                    }
                                }
                                else
                                {
                                    SendDataFeedReportingEmails(dataFeedSettings, ftpDetail.DocumentFolderPath, false);
                                    logger.Debug("Delivery of Data Feed document Failed. Saving data into Invoke History and Invoke History Details.");
                                    Int32 dataFeedInvokeResultFailedID = dataFeedInvokeResult.Where(cond => cond.Code == DataFeedInvokeResult.Failed.GetStringValue())
                                                                             .FirstOrDefault().DataFeedInvokeResultID;
                                    SecurityManager.SaveLogInInvokeHistory(dataFeedSettings, _lstInvokeHistorydetail, dataFeedSvcUserID, dataFeedInvokeResultFailedID);
                                    logger.Debug("Deliver Data Feed Document Failed.");
                                }
                                logger.Debug("Date Feed Service for Tenant " + tnt.CDB_DBName + " and DataFeedSettingID " + dataFeedSettings.DataFeedSettingId
                                             + " completed.");
                            }
                            else
                            {
                                ServiceContext.ReleaseDBContextItems();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            ServiceContext.ReleaseDBContextItems();
                            logger.Error(String.Format("An Error has occured in Date Feed Service for Tenant "
                                                       + tnt.CDB_DBName + " and DataFeedSettingID " + dataFeedSettings.DataFeedSettingId
                                                       + "the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                                        ex.Message, ex.InnerException, ex.StackTrace));
                        }
                    });
                }
                else
                {
                    logger.Debug("Data Feed Setting is configured for Tenant: " + tnt.CDB_DBName);
                }
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("An Error has occured while Invoke Data Feed the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                       ex.Message, ex.InnerException, ex.StackTrace));
                throw ex;
            }
        }

        private static void SendDataFeedReportingEmails(DataFeedSettingContract dataFeedSettings, String remotePath, Boolean isDeliverySuccess)
        {
            //Send Emails to user listed in Reporting email list.
            if (!dataFeedSettings.ReportingEmails.IsNullOrEmpty())
            {
                List<String> reportingEmailIDs = dataFeedSettings.ReportingEmails.Split(',').ToList();
                StringBuilder sbEmailBody = new StringBuilder();
                sbEmailBody.Append("Hello," + "<br/><br/>");
                if (isDeliverySuccess)
                {
                    sbEmailBody.Append("ADB shipment of data is complete and documents are uploaded successfully at below mentioned SFTP location:" + "<br/>");
                    sbEmailBody.Append(remotePath + "<br/><br/>");
                    sbEmailBody.Append("Date/Time: " + DateTime.Now + "<br/>");
                    sbEmailBody.Append("Status: " + "Success" + "<br/><br/>");
                }
                else
                {
                    sbEmailBody.Append("ADB shipment of data failed." + "<br/><br/>");
                    sbEmailBody.Append("Date/Time: " + DateTime.Now + "<br/>");
                    sbEmailBody.Append("Status: " + "Failed" + "<br/><br/>");
                }

                sbEmailBody.Append("Thank you," + "<br/>");
                sbEmailBody.Append("American DataBank" + "<br/>");

                String emailContent = Convert.ToString(sbEmailBody);
                String emailSubject = dataFeedSettings.ReportingEmailSubject;

                reportingEmailIDs.ForEach(repEmail =>
                {
                    List<MailAddress> reportingUserEmailAddress = new List<MailAddress>();
                    reportingUserEmailAddress.Add(new MailAddress(repEmail));
                    SendMail(reportingUserEmailAddress, emailSubject, emailContent);

                    logger.Debug(String.Format("Email send successfully to reporting user: {0}", repEmail));
                });
            }
        }

        private static Boolean DeliverDataFeedDocumentToClient(DataFeedSettingContract dataFeedSettings, String fileContent, DataFeedFTPDetail ftpDetail)
        {
            String tenantName = dataFeedSettings.TenantDBName;
            String remotePath = String.Empty;

            try
            {
                logger.Debug("START DeliverDataFeedDocumentToClient for tenantId: " + tenantName);

                if (!fileContent.IsNullOrEmpty())
                {
                    //String fileName = "DataFeedFile";
                    //fileName = fileName + "_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("hhmmss") + DateTime.Now.Millisecond.ToString();

                    //System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9_.]+", "",
                    //                                                                                System.Text.RegularExpressions.RegexOptions.Compiled);

                    String _tenaatName = System.Text.RegularExpressions.Regex.Replace(dataFeedSettings.TenantName, "[^a-zA-Z0-9_.]+", ""
                                                                                                                 , System.Text.RegularExpressions.RegexOptions.Compiled);

                    String fileName = DateTime.Now.ToString("yyyy-MM-dd-hh-MM-ss") + "-" + (String.IsNullOrEmpty(dataFeedSettings.FileNameSuffix) ? _tenaatName
                                                                                    : dataFeedSettings.FileNameSuffix);
                    //String dataBaseNamePrefix = ConfigurationManager.AppSettings["DataBaseNamePrefix"];
                    //Int32 dataBaseNamePrefixCount = 0;

                    //if (!dataBaseNamePrefix.IsNullOrEmpty())
                    //{
                    //    dataBaseNamePrefixCount = dataBaseNamePrefix.Count();
                    //}
                    //if (!dataBaseNamePrefix.IsNullOrEmpty() && !dataBaseNamePrefix.EndsWith("_"))
                    //{
                    //    dataBaseNamePrefixCount += 1;
                    //}
                    //String NewTenantName = tenantName.Remove(0, dataBaseNamePrefixCount);
                    String fileExtension = DataFeedFileUtils.GetFileExtension(dataFeedSettings.OutputCode);
                    String fullDataFeedFileName = fileName + fileExtension;

                    if (dataFeedSettings.OutputCode == DataFeedUtilityConstants.LKP_OUTPUT_TEXT)
                    {
                        fileExtension = DataFeedFileUtils.GetFileExtension(TextFileEnum.TEXT_DATA.ToString());
                        fullDataFeedFileName = fileName + fileExtension;
                    }

                    //Save file at temporary location from bytes
                    String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
                    if (!tempFilePath.EndsWith(@"\"))
                    {
                        tempFilePath += @"\";
                    }
                    tempFilePath += "TempFiles\\" + tenantName + "\\" + fileName;
                    if (!Directory.Exists(tempFilePath))
                    {
                        Directory.CreateDirectory(tempFilePath);
                    }
                    //Save file at temporary location
                    String localPath = Path.Combine(tempFilePath, fullDataFeedFileName);
                    File.WriteAllText(localPath, fileContent);

                    //FileNamePath zipFileInfo = new FileNamePath();
                    if (dataFeedSettings.OutputCode == DataFeedUtilityConstants.LKP_OUTPUT_TEXT)
                    {
                        FileNamePath zipFileInfo = DataFeedFileUtils.CreateZippedTextDataFile(fileName, tempFilePath, localPath);
                        fullDataFeedFileName = zipFileInfo.FullFileName;
                        localPath = zipFileInfo.FullFilePath;
                    }

                    //Upload Document to SFTP location
                    if (dataFeedSettings.DeliveryTypeCode.ToLower() == DataFeedUtilityConstants.LKP_DELIVERY_TYPE_SFTP.ToLower())
                    {
                        logger.Debug("Start Delivery of Data Feed Document to SFTP location based on DataFeedFTPDetail.");

                        if (ftpDetail.IsNotNull() && !String.IsNullOrEmpty(fullDataFeedFileName))
                        {
                            remotePath = ftpDetail.DocumentFolderPath;
                            if (!remotePath.IsNullOrEmpty())
                            {
                                //Upload Data Feed File to Remote Path
                                SessionOptions sessionOptions = DataFeedFTPUtils.SetFTPSessionOption(ftpDetail);
                                if (DataFeedFTPUtils.FTPFileUpload(remotePath, fullDataFeedFileName, localPath, sessionOptions))
                                {
                                    if (dataFeedSettings.OutputCode == DataFeedUtilityConstants.LKP_OUTPUT_TEXT)
                                    {
                                        logger.Debug("Creating Manifest File for Data Feed Document.");
                                        //Upload Manifest File to Remote Path
                                        FileNamePath manifestFileInfo = DataFeedFileUtils.CreateManifestFile(fullDataFeedFileName, fileName, remotePath, tempFilePath);

                                        logger.Debug("Manifest File created succesfully for Data Feed Document.");
                                        return DataFeedFTPUtils.FTPFileUpload(remotePath, manifestFileInfo.FullFileName, manifestFileInfo.FullFilePath, sessionOptions);
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                logger.Debug("Delivery of Data Feed Document to SFTP location failed. Remote Path is empty in DataFeedFTPDetail. "
                                            + ftpDetail.HostName + " " + ftpDetail.PortNo + " " + " " + remotePath + " ");
                                return false;
                            }
                        }
                        else
                        {
                            logger.Debug("Delivery of Data Feed Document to SFTP location failed. Either DataFeedFTPDetail is empty OR Data Feed File Path is empty."
                                        + " Please verify Data Feed FTP Detail for OutputSetting.");
                            return false;
                        }
                    }
                    else
                    {
                        logger.Debug("Delivery of Data Feed Document failed. DataFeedSetting 'DeliveryTypeCode' is either empty or not supported.");
                        return false;
                    }
                }
                else
                {
                    logger.Debug("Delivery of Data Feed Document failed. Data Feed 'fileContent' is either empty or not supported.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Delivery of Data Feed Document failed for " + tenantName);
                logger.Error("Exception details: {0}, Inner Exception: {1}, Stack Trace: {2}"
                                    , ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                return false;
            }
            return true;
        }

        private static DataFeedSettingContract SetDataFeedInvokeHistoryData(DataFeedSettingContract dataObject)
        {
            logger.Debug("START  calling SetDataFeedInvokeHistoryData for tenantId: " + dataObject.TenantID + " and settingId:" + dataObject.DataFeedSettingId);
            DataFeedSettingContract result = SecurityManager.SetDataFeedInvokeHistoryData(dataObject);
            logger.Debug("END calling SetDataFeedInvokeHistoryData for tenantId: " + dataObject.TenantID + "and settingId:" + dataObject.DataFeedSettingId);
            return result;
        }

        /// <summary>
        /// Method that get the datafeed xml from data feed web service and apply filter to remove the already captured orders.
        /// </summary>
        /// <param name="settingObject">object of setting</param>
        /// <returns></returns>
        private static String GetDataFeedCSVFormattedContent(DataFeedSettingContract dataFeedSettingContract)
        {
            logger.Debug("START calling GetDataFeedCSVFormattedContent for TenantId: " + dataFeedSettingContract.TenantID + " and DataFeedSettingId: "
                         + dataFeedSettingContract.DataFeedSettingId);

            DataFeedClient.DataFeedClient dataFeedClient = new DataFeedClient.DataFeedClient();

            Dictionary<String, String> dataFeedXmldic = new Dictionary<String, String>();
            String dataFeedXml = String.Empty;
            String dataFeedCSVFileContent = String.Empty;
            Dictionary<String, Dictionary<String, String>> dicOfDataFeedXml = new Dictionary<String, Dictionary<String, String>>();
            String startDate = String.Empty;
            String endDate = String.Empty;
            DateTime newStartDate = DateTime.MinValue;
            DateTime newEndDate = DateTime.MinValue;
            Boolean isFullBatchProcessed = false;

            if (dataFeedSettingContract.IsNotNull())
            {
                startDate = Convert.ToString(dataFeedSettingContract.RecordOriginStartDate);
                endDate = Convert.ToString(dataFeedSettingContract.RecordOriginEnddate.Value);

                //Get data feed xml from data feed web service on the basis of settings.
                logger.Debug("Fetching XML data using DataFeedWebService for " + dataFeedSettingContract.TenantDBName + ". TenantID: " + dataFeedSettingContract.TenantID
                            + ". DataFeedSettingID: " + dataFeedSettingContract.DataFeedSettingId
                            + ". RecordStartDate: " + startDate + ". RecordEnddate: " + endDate);
                dataFeedXmldic = dataFeedClient.GetXmlData(dataFeedSettingContract.TenantID, dataFeedSettingContract.DataFeedSettingId, dataFeedSettingContract.AccessKey,
                                                            startDate, endDate);
                logger.Debug("XML data using DataFeedWebService for " + dataFeedSettingContract.TenantDBName + " fetched successfully.");
                if (dataFeedXmldic.ContainsKey("Result"))
                {
                    DataFeedFormatterFactory obj = new DataFeedFormatterFactory();

                    if (dataFeedXmldic["Result"] == "SUCCCESS")
                    {

                        dataFeedXml = dataFeedXmldic["Text"].ToString();

                        logger.Debug("START calling DataFeedFormatterFactory 'FormatDataFeed' for " + dataFeedSettingContract.TenantDBName
                                    + " and DataFeedSettingId: " + dataFeedSettingContract.DataFeedSettingId);

                        dicOfDataFeedXml = obj.GetDataFeedFormatter(dataFeedSettingContract.DataFeedSettingId).FormatDataFeed(FileFormat.CSV, dataFeedXml,
                                                                    dataFeedSettingContract.TenantID, dataFeedSettingContract.FormatID, dataFeedSettingContract);
                        logger.Debug("END calling DataFeedFormatterFactory for : " + dataFeedSettingContract.TenantDBName + " and DataFeedSettingId:"
                                    + dataFeedSettingContract.DataFeedSettingId + " Total Records after Format: " + dicOfDataFeedXml.Count());

                        //apply filter on dictionary
                        logger.Debug("START calling ApplyFilterForDataFeedXml for: " + dataFeedSettingContract.TenantDBName + " and DataFeedSettingId: "
                                    + dataFeedSettingContract.DataFeedSettingId);
                        Dictionary<String, Dictionary<String, String>> filteredDataFeedXml = ApplyFilterForDataFeedXml(dicOfDataFeedXml, dataFeedSettingContract);

                        logger.Debug("END calling ApplyFilterForDataFeedXml for: " + dataFeedSettingContract.TenantDBName + " and DataFeedSettingId: "
                                    + dataFeedSettingContract.DataFeedSettingId);

                        if (!filteredDataFeedXml.IsNullOrEmpty())
                        {
                            logger.Debug("START calling DataFeedFormatterFactory 'ConvertDictionarytoCSV' for: " + dataFeedSettingContract.TenantDBName
                                        + " and DataFeedSettingId: " + dataFeedSettingContract.DataFeedSettingId);

                            //Get CSV formated data feed file.
                            dataFeedCSVFileContent = obj.GetDataFeedFormatter(dataFeedSettingContract.DataFeedSettingId).ConvertDictionarytoCSV(filteredDataFeedXml,
                                                                                dataFeedSettingContract);

                            logger.Debug("END calling DataFeedFormatterFactory 'ConvertDictionarytoCSV' for: " + dataFeedSettingContract.TenantDBName
                                         + " and DataFeedSettingId:" + dataFeedSettingContract.DataFeedSettingId);
                        }
                        else
                        {
                            logger.Debug("ApplyFilterForDataFeedXml 'filteredDataFeedXml' for: " + dataFeedSettingContract.TenantDBName
                                + " and DataFeedSettingId: " + dataFeedSettingContract.DataFeedSettingId + " is Empty.");
                        }
                    }
                    else if (dataFeedXmldic["Result"] == "TOOLARGE")
                    {
                        String loopStartDate = startDate;
                        String loopEndDate = endDate;
                        String dataFeedXmlLoop = String.Empty;
                        //DataFeedFormatterFactory obj = new DataFeedFormatterFactory();
                        Dictionary<String, Dictionary<String, String>> dicOfDataFeedXmlLoop = new Dictionary<String, Dictionary<String, String>>();
                        while (!isFullBatchProcessed)
                        {
                            logger.Debug("START calling TooLarge Xml loop  for: " + dataFeedSettingContract.TenantDBName + " and DataFeedSettingId:"
                                        + dataFeedSettingContract.DataFeedSettingId);
                            Dictionary<String, String> dataFeedXmldicLoop = new Dictionary<String, String>();
                            logger.Debug("StartDate:" + loopStartDate + " EndDate: " + loopEndDate);
                            GetStartDateEndDate(Convert.ToDateTime(loopStartDate), Convert.ToDateTime(loopEndDate), out newStartDate, out newEndDate);
                            logger.Debug("NewStartDate:" + Convert.ToString(newStartDate) + " NewEndDate: " + Convert.ToString(newEndDate));
                            dataFeedXmldicLoop = dataFeedClient.GetXmlData(dataFeedSettingContract.TenantID, dataFeedSettingContract.DataFeedSettingId,
                                                                            dataFeedSettingContract.AccessKey, newStartDate.ToString(), newEndDate.ToString());
                            switch (dataFeedXmldicLoop["Result"])
                            {
                                case "SUCCCESS":
                                    {
                                        dataFeedXmlLoop = dataFeedXmldicLoop["Text"].ToString();
                                        Dictionary<String, Dictionary<String, String>> tempDicDataFeedXML = new Dictionary<String, Dictionary<String, String>>();

                                        tempDicDataFeedXML = obj.GetDataFeedFormatter(dataFeedSettingContract.DataFeedSettingId)
                                                                .FormatDataFeed(FileFormat.CSV, dataFeedXmlLoop.ToString(), dataFeedSettingContract.TenantID,
                                                                                dataFeedSettingContract.FormatID, dataFeedSettingContract);
                                        foreach (String key in tempDicDataFeedXML.Keys)
                                        {
                                            if (!dicOfDataFeedXmlLoop.ContainsKey(key))
                                                dicOfDataFeedXmlLoop.Add(key, tempDicDataFeedXML[key]);
                                        }

                                        if (Convert.ToDateTime(newEndDate) >= Convert.ToDateTime(endDate))
                                        {
                                            isFullBatchProcessed = true;
                                            logger.Debug("Full Batch Processed successfully.");
                                        }
                                        else
                                        {
                                            loopStartDate = newEndDate.ToString();
                                            loopEndDate = endDate;
                                        }
                                        break;
                                    }
                                case "TOOLARGE":
                                    loopStartDate = newStartDate.ToString();
                                    loopEndDate = newEndDate.ToString();
                                    break;
                            }
                        }
                        logger.Debug("END calling Too Large Xml loop for: " + dataFeedSettingContract.TenantDBName + " and DataFeedSettingId:"
                                    + dataFeedSettingContract.DataFeedSettingId);
                        //apply filter on dictionary
                        Dictionary<String, Dictionary<String, String>> filteredDataFeedXml = ApplyFilterForDataFeedXml(dicOfDataFeedXmlLoop, dataFeedSettingContract);
                        if (!filteredDataFeedXml.IsNullOrEmpty())
                        {
                            //Get CSV formated data feed file.
                            dataFeedCSVFileContent = obj.GetDataFeedFormatter(dataFeedSettingContract.DataFeedSettingId)
                                                        .ConvertDictionarytoCSV(filteredDataFeedXml, dataFeedSettingContract);

                            logger.Debug("END calling DataFeedFormatterFactory for: " + dataFeedSettingContract.TenantDBName + " and DataFeedSettingId:"
                                        + dataFeedSettingContract.DataFeedSettingId);
                            logger.Debug("END calling GetDataFeedCSVFormattedContent for: " + dataFeedSettingContract.TenantDBName
                                         + " and DataFeedSettingId:" + dataFeedSettingContract.DataFeedSettingId);
                        }
                    }
                    else
                    {
                        logger.Debug(dataFeedXmldic["Text"].ToString());
                    }
                }
                else
                {
                    logger.Debug("DataFeedXmldic does not contains Result for: " + dataFeedSettingContract.TenantDBName + " and DataFeedSettingId: "
                                + dataFeedSettingContract.DataFeedSettingId);
                }
                logger.Debug("END calling GetDataFeedCSVFormattedContent for: " + dataFeedSettingContract.TenantDBName
                            + " and SettingId: " + dataFeedSettingContract.DataFeedSettingId);
                return dataFeedCSVFileContent;
            }
            return dataFeedCSVFileContent;
        }

        private static Dictionary<String, Dictionary<String, String>> ApplyFilterForDataFeedXml(Dictionary<String, Dictionary<String, String>> dataFeedXML,
                                                                                                DataFeedSettingContract dataFeedSettingContract)
        {
            _lstInvokeHistorydetail = new List<DataFeedInvokeHistoryDetail>();
            List<DataFeedInvokeHistoryDetail> dataFeedInvokeDetailList = new List<DataFeedInvokeHistoryDetail>();
            dataFeedInvokeDetailList = dataFeedSettingContract.DataFeedInvokeDetailList;

            Dictionary<String, Dictionary<String, String>> dataFeedXmlDoc = new Dictionary<String, Dictionary<String, String>>();
            logger.Debug("START calling ApplyFilterForDataFeedXml for tenantId: " + dataFeedSettingContract.TenantID + " and DataFeedSettingId:"
                        + dataFeedSettingContract.DataFeedSettingId);
            List<DataFeedInvokeHistoryDetail> ordersTobeRemove = null;
            var keys = dataFeedXML.Select(d => d.Key).ToList();//select distinct keys from main dictionary
            foreach (var key in keys)
            {
                Dictionary<String, String> innerdict = dataFeedXML[key]; //select inner dictionary from main dictionary based on key
                if (innerdict.ContainsKey("OrderID") || innerdict.ContainsKey("##OrderID"))
                {
                    DateTime? lastUpdatedDate = null;
                    Int32 orderID = Convert.ToInt32(innerdict.Where(e => e.Key == "##OrderID" || e.Key == "OrderID").Select(e => e.Value).FirstOrDefault());
                    DateTime ModifiedDate = Convert.ToDateTime(innerdict.Where(e => e.Key == "##LastUpdatedOn").Select(e => e.Value).FirstOrDefault());
                    if (!dataFeedInvokeDetailList.IsNullOrEmpty() && dataFeedInvokeDetailList.Count > 0)
                    {
                        ordersTobeRemove = dataFeedInvokeDetailList.Where(cnd => cnd.DFIHD_EntityKey.Value == orderID).ToList();
                    }
                    if (ordersTobeRemove.IsNotNull() && ordersTobeRemove.Count > 0)
                    {
                        lastUpdatedDate = ordersTobeRemove.Max(x => x.DFIHD_LastUpdatedDate);
                    }

                    //As per discussion, IF Include Only New is True then remove Order which are not updated since last Update.
                    if (dataFeedSettingContract.IncludeOnlyNew && !lastUpdatedDate.IsNullOrEmpty() && (ModifiedDate <= lastUpdatedDate.Value))
                    {
                        dataFeedXML.Remove(key);
                    }
                    else
                    {
                        DataFeedInvokeHistoryDetail objdataFeedInvokeHistorydetail = new DataFeedInvokeHistoryDetail();
                        objdataFeedInvokeHistorydetail.DFIHD_EntityKey = orderID;
                        objdataFeedInvokeHistorydetail.DFIHD_LastUpdatedDate = ModifiedDate;
                        objdataFeedInvokeHistorydetail.DFIHD_IsDeleted = false;
                        objdataFeedInvokeHistorydetail.DFIHD_CreatedOn = DateTime.Now;
                        objdataFeedInvokeHistorydetail.DFIHD_CreatedBy = dataFeedSvcUserID;
                        _lstInvokeHistorydetail.Add(objdataFeedInvokeHistorydetail);
                    }
                    /* Code Commented as per discussion, IF Include Only New is True then remove Order which are not updated since last Update.
                     * IF INCLUDE ONLY NEW IS FALSE, 
                    if (IncludeOnlyNew && !lastUpdatedDate.IsNullOrEmpty())
                    {
                        dataFeedXML.Remove(key);
                    }
                    else if (!IncludeOnlyNew && !lastUpdatedDate.IsNullOrEmpty() && ModifiedDate < lastUpdatedDate.Value)                    
                    {
                        dataFeedXML.Remove(key);
                    }
                    else
                    {
                    
                    DataFeedInvokeHistoryDetail objdataFeedInvokeHistorydetail = new DataFeedInvokeHistoryDetail();
                    objdataFeedInvokeHistorydetail.DFIHD_EntityKey = orderID;
                    objdataFeedInvokeHistorydetail.DFIHD_LastUpdatedDate = ModifiedDate;
                    objdataFeedInvokeHistorydetail.DFIHD_IsDeleted = false;
                    objdataFeedInvokeHistorydetail.DFIHD_CreatedOn = DateTime.Now;
                    objdataFeedInvokeHistorydetail.DFIHD_CreatedBy = dataFeedSvcUserID;
                    _lstInvokeHistorydetail.Add(objdataFeedInvokeHistorydetail);
                    }
                    */
                }
            }

            logger.Debug("END calling ApplyFilterForDataFeedXml for tenantId: " + dataFeedSettingContract.TenantID + " and DataFeedSettingId:"
                        + dataFeedSettingContract.DataFeedSettingId);
            return dataFeedXML;
        }

        private static void GetStartDateEndDate(DateTime startDate, DateTime endDate, out DateTime newStartDate, out DateTime newEndDate)
        {
            Double daysDifference = 0;
            Int32 splitedDays = 0;
            TimeSpan timeSpan = endDate - startDate;
            daysDifference = timeSpan.TotalDays;
            splitedDays = Convert.ToInt32(daysDifference / 2);
            if (daysDifference <= AppConsts.ONE)
            {
                splitedDays = AppConsts.ONE;
            }
            newStartDate = startDate;
            newEndDate = startDate.AddDays(splitedDays);
        }

        private static Boolean SendMail(List<MailAddress> emailAddress, String Subject, String bodyText)
        {
            //SmtpClient smtpClient = new SmtpClient();
            Guid temp = Guid.NewGuid();
            Configuration mConfigurationFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //MailSettingsSectionGroup mailSettings = mConfigurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

            try
            {
                using (MailMessage message = new MailMessage())
                {
                    //MailAddress fromAddress = new MailAddress(Convert.ToString(mailSettings.Smtp.From));
                    //smtpClient.Host = Convert.ToString(mailSettings.Smtp.Network.Host);
                    //smtpClient.Port = Convert.ToInt32(mailSettings.Smtp.Network.Port);
                    //if (mailSettings.Smtp.Network.UserName != null && mailSettings.Smtp.Network.Password != null)
                    //    smtpClient.Credentials = new System.Net.NetworkCredential(mailSettings.Smtp.Network.UserName, mailSettings.Smtp.Network.Password);

                    //message.From = fromAddress;

                    foreach (MailAddress toAddress in emailAddress)
                    {
                        message.To.Add(toAddress);
                    }

                    message.Subject = Subject;
                    message.IsBodyHtml = true;
                    message.Body = bodyText;
                    message.Priority = MailPriority.Normal;
                    logger.Info(DateTime.Now.ToString() + ": " + " : sending email " + temp);
                    if ((emailAddress != null && emailAddress.Count > 0))
                    {
                        Intsof.SMTPService.SMTPService smtpService = new Intsof.SMTPService.SMTPService();
                        smtpService.SendMail(message);
                        //smtpClient.Send(message);
                    }
                    logger.Info(DateTime.Now.ToString() + " : sent email " + temp);
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured sending the mail, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                return false;
            }
            return true;
        }

        #endregion

        #endregion
    }
}
