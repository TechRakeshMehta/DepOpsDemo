using Business.ReportExecutionService;
using Entity;
using INTSOF.Utils;
using System;
using System.Configuration;
using System.IO;
using System.Web.Configuration;
using System.Collections.Generic;
using INTSOF.UI.Contract.Report;
using System.Linq;
using System.Data.Entity.Core.Objects.DataClasses;

namespace Business.RepoManagers
{
    public static class ReportManager
    {
        public static Report GetReportByCode(string reportCode)
        {
            return BALUtils.GetReportRepoInstance().GetReportByCode(reportCode);
        }

        /// <summary>
        /// To Get Report Byte Array
        /// </summary>
        /// <param name="reportName"></param>
        /// <param name="parameters"></param>
        /// <returns>Report Byte Array</returns>
        public static byte[] GetReportByteArray(String reportName, ParameterValue[] reportParameters)
        {
            try
            {
                if (reportParameters.IsNotNull())
                {
                    byte[] reportContent = new byte[1];

                    using (ReportExecutionService.ReportExecutionService client = new ReportExecutionService.ReportExecutionService())
                    {
                        //client.Credentials = System.Net.CredentialCache.DefaultCredentials;
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

                        //client.Credentials = System.Net.CredentialCache.DefaultCredentials;
                        client.Credentials = new System.Net.NetworkCredential(userName, password, domain);

                        string reportPath = String.Empty;

                        if (!ConfigurationManager.AppSettings["ReportPath"].IsNullOrEmpty())
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
                {
                    return null;
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static byte[] GetReportByteArrayFormat(String reportName, ParameterValue[] reportParameters, String format = "EXCELOPENXML")
        {
            try
            {
                if (reportParameters.IsNotNull())
                {
                    byte[] reportContent = new byte[1];

                    using (ReportExecutionService.ReportExecutionService client = new ReportExecutionService.ReportExecutionService())
                    {
                        //client.Credentials = System.Net.CredentialCache.DefaultCredentials;
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

                        //client.Credentials = System.Net.CredentialCache.DefaultCredentials;
                        client.Credentials = new System.Net.NetworkCredential(userName, password, domain);

                        string reportPath = String.Empty;

                        if (!ConfigurationManager.AppSettings["ReportPath"].IsNullOrEmpty())
                        {
                            reportPath = "/" + ConfigurationManager.AppSettings["ReportPath"] + "/" + reportName;
                        }

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
                {
                    return null;
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String ConvertByteArrayToReportFile(byte[] reportContent, String fileName)
        {
            try
            {
                if (!reportContent.IsNullOrEmpty())
                {
                    String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];

                    if (tempFilePath.IsNullOrEmpty())
                    {
                        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine +
                            "Please provide path for TemporaryFileLocation in config.", null);
                        return "";
                    }
                    if (!tempFilePath.EndsWith(@"\"))
                    {
                        tempFilePath += @"\";
                    }
                    tempFilePath += @"OrderCompletionResults\";

                    if (!Directory.Exists(tempFilePath))
                        Directory.CreateDirectory(tempFilePath);

                    //Save file at temporary location
                    String newTempFilePath = Path.Combine(tempFilePath, fileName);

                    File.WriteAllBytes(newTempFilePath, reportContent);

                    String destFilePath = @"OrderCompletionResults\" + fileName;

                    String filePath = CommonFileManager.SaveDocument(newTempFilePath, destFilePath, FileType.ApplicantFileLocation.GetStringValue());

                    return filePath;
                }
                else
                {
                    return "";
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static lkpReportParameter GetReportParameterByCode(String parameterCode)
        {
            return LookupManager.GetLookUpData<lkpReportParameter>().Where(cond => cond.RP_Code == parameterCode && !cond.RP_IsDeleted).FirstOrDefault();
        }

        public static Boolean SaveReportFavouriteParameter(FavReportParamContract favReportParamContract)
        {
            Report report = GetReportByCode(favReportParamContract.ReportCode);
            if (!report.IsNullOrEmpty())
            {
                ReportFavouriteParameter reportFavouriteParameter = new ReportFavouriteParameter();
                reportFavouriteParameter.RFP_Name = favReportParamContract.FavParamName;
                reportFavouriteParameter.RFP_Description = favReportParamContract.FavParamDescription;
                reportFavouriteParameter.RFP_ReportID = report.RP_ID;
                reportFavouriteParameter.RFP_CreatedByID = favReportParamContract.CreatedByID;
                reportFavouriteParameter.RFP_CreatedOn = DateTime.Now;
                reportFavouriteParameter.RFP_IsDeleted = false;
                reportFavouriteParameter.RFP_UserID = favReportParamContract.UserID; //UAT-3052

                reportFavouriteParameter.FavParamReportParamMappings = new EntityCollection<FavParamReportParamMapping>();

                foreach (KeyValuePair<String, String> parameter in favReportParamContract.ParameterValues)
                {
                    lkpReportParameter reportParameter = GetReportParameterByCode(parameter.Key);
                    if (!reportParameter.IsNullOrEmpty())
                    {
                        FavParamReportParamMapping favParamReportParamMapping = new FavParamReportParamMapping();
                        favParamReportParamMapping.FPRPM_ParamID = reportParameter.RP_ID;
                        favParamReportParamMapping.FPRPM_Value = parameter.Value;
                        favParamReportParamMapping.FPRPM_IsDeleted = false;
                        favParamReportParamMapping.FPRPM_CreatedByID = favReportParamContract.CreatedByID;
                        favParamReportParamMapping.FPRPM_CreatedOn = DateTime.Now;
                        reportFavouriteParameter.FavParamReportParamMappings.Add(favParamReportParamMapping);
                    }
                }
                return BALUtils.GetReportRepoInstance().SaveReportFavouriteParameter(reportFavouriteParameter);
            }
            return false;
        }

        public static List<ReportFavouriteParameter> GetReportFavouriteParametersByUserID(Int32 currentLoggedInUserID)
        {
            return BALUtils.GetReportRepoInstance().GetReportFavouriteParametersByUserID(currentLoggedInUserID);
        }

        public static ReportFavouriteParameter GetReportFavouriteParameterByID(Int32 selectedFavParamID)
        {
            return BALUtils.GetReportRepoInstance().GetReportFavouriteParameterByID(selectedFavParamID);
        }

        public static Boolean UpdateFavParamReportParamMapping(Dictionary<Int32, String> dicUpdatedParameters, ReportFavouriteParameter favParam)
        {
            return BALUtils.GetReportRepoInstance().UpdateFavParamReportParamMapping(dicUpdatedParameters, favParam);
        }

        public static String SaveUpdatedApplicantRequirements(byte[] reportContent, String fileName)
        {
            try
            {
                if (!reportContent.IsNullOrEmpty())
                {
                    String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];

                    if (tempFilePath.IsNullOrEmpty())
                    {
                        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine +
                            "Please provide path for TemporaryFileLocation in config.", null);
                        return "";
                    }
                    if (!tempFilePath.EndsWith(@"\"))
                    {
                        tempFilePath += @"\";
                    }
                    tempFilePath += @"UpdatedApplicantRequirements\";

                    if (!Directory.Exists(tempFilePath))
                        Directory.CreateDirectory(tempFilePath);

                    //Save file at temporary location
                    String newTempFilePath = Path.Combine(tempFilePath, fileName);

                    File.WriteAllBytes(newTempFilePath, reportContent);

                    String destFilePath = @"UpdatedApplicantRequirements\" + fileName;

                    String filePath = CommonFileManager.SaveDocument(newTempFilePath, destFilePath, FileType.ApplicantFileLocation.GetStringValue());

                    return filePath;
                }
                else
                {
                    return "";
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String SaveSvcFormApplicantAgency(byte[] reportContent, String fileName)
        {
            try
            {
                if (!reportContent.IsNullOrEmpty())
                {
                    String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];

                    if (tempFilePath.IsNullOrEmpty())
                    {
                        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine +
                            "Please provide path for TemporaryFileLocation in config.", null);
                        return "";
                    }
                    if (!tempFilePath.EndsWith(@"\"))
                    {
                        tempFilePath += @"\";
                    }
                    tempFilePath += @"UpdatedApplicantRequirements\";

                    if (!Directory.Exists(tempFilePath))
                        Directory.CreateDirectory(tempFilePath);

                    //Save file at temporary location
                    String newTempFilePath = Path.Combine(tempFilePath, fileName);

                    File.WriteAllBytes(newTempFilePath, reportContent);

                    String destFilePath = @"UpdatedApplicantRequirements\" + fileName;

                    String filePath = CommonFileManager.SaveDocument(newTempFilePath, destFilePath, FileType.ApplicantFileLocation.GetStringValue());

                    return filePath;
                }
                else
                {
                    return "";
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteFavParamReportParamMapping(string RPF_ids, Int32 CurrentUserId)
        {
            return BALUtils.GetReportRepoInstance().DeleteFavParamReportParamMapping(RPF_ids, CurrentUserId);
        }

        public static String SaveDocument(byte[] reportContent, String fileName)
        {
            try
            {
                if (!reportContent.IsNullOrEmpty())
                {
                    String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];

                    if (tempFilePath.IsNullOrEmpty())
                    {
                        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine +
                            "Please provide path for TemporaryFileLocation in config.", null);
                        return "";
                    }
                    if (!tempFilePath.EndsWith(@"\"))
                    {
                        tempFilePath += @"\";
                    }
                    tempFilePath += @"UpdatedApplicantRequirements\";

                    if (!Directory.Exists(tempFilePath))
                        Directory.CreateDirectory(tempFilePath);

                    //Save file at temporary location
                    String newTempFilePath = Path.Combine(tempFilePath, fileName);

                    File.WriteAllBytes(newTempFilePath, reportContent);

                    String destFilePath = @"UpdatedApplicantRequirements\" + fileName;

                    String filePath = CommonFileManager.SaveDocument(newTempFilePath, destFilePath, FileType.ApplicantFileLocation.GetStringValue());

                    return filePath;
                }
                else
                {
                    return "";
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
    }
}