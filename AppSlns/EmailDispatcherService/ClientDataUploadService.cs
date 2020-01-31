using Business.RepoManagers;
using Entity;
using Entity.SharedDataEntity;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.Services;
using INTSOF.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.RegularExpressions;
namespace EmailDispatcherService
{
    public class ClientDataUploadService
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Boolean _isServiceLoggingEnabled = ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]) : false;

        public static void UploadClientData(Int32? clientDataUpload_Id = null)
        {
            try
            {
                logger.Info("******************* Calling UploadClientData: " + DateTime.Now.ToString() + " *******************");
                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                Int32 backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                Boolean isTestingModeOn = ConfigurationManager.AppSettings["IsTestingModeOn"].IsNotNull() ?
                                                            Convert.ToBoolean(ConfigurationManager.AppSettings["IsTestingModeOn"]) : false;

                Boolean isIgnoreAceMap = ConfigurationManager.AppSettings["IsIgnoreAceMap"].IsNotNull() ?
                                                           Convert.ToBoolean(ConfigurationManager.AppSettings["IsIgnoreAceMap"]) : false;

                Boolean IsAceMappExceptionThrow = ConfigurationManager.AppSettings["IsAceMappExceptionThrow"].IsNotNull() ?
                                                           Convert.ToBoolean(ConfigurationManager.AppSettings["IsAceMappExceptionThrow"]) : false;

                string AceMappErrorCodeAndText = ConfigurationManager.AppSettings["AceMappErrorCodeAndText"].IsNotNull() ?
                                                           Convert.ToString(ConfigurationManager.AppSettings["AceMappErrorCodeAndText"]) : string.Empty;

                Int32 Chunksize = ConfigurationManager.AppSettings["ClientDataUploadService_ChunkSize"].IsNotNull() ?
                                          Convert.ToInt32(ConfigurationManager.AppSettings["ClientDataUploadService_ChunkSize"]) : AppConsts.CHUNK_SIZE_FOR_CLIENT_DATA_UPLOAD;

                bool IsApiCallByPassed = ConfigurationManager.AppSettings["IsApiCallByPassed"].IsNotNull() ?
                                             Convert.ToBoolean(ConfigurationManager.AppSettings["IsApiCallByPassed"]) : false;


                //Get Client Data upload Ids
                List<Int32> lstClientDataUploadIds = ComplianceDataManager.GetTenantForClientDataUpload();

                if (!clientDataUpload_Id.IsNullOrEmpty() && lstClientDataUploadIds.Count > AppConsts.NONE)
                {
                    if (lstClientDataUploadIds.Contains(Convert.ToInt32(clientDataUpload_Id)))
                    {
                        lstClientDataUploadIds = new List<int>();
                        lstClientDataUploadIds.Add(Convert.ToInt32(clientDataUpload_Id));
                    }

                }
                logger.Info("*******************## Client Data upload loop Start ##*******************");
                foreach (var ClientDataUploadId in lstClientDataUploadIds)
                {

                    logger.Info("******************* Started Upload Client Data Service: " + DateTime.Now.ToString() + " *******************");
                    DateTime jobStartTime = DateTime.Now;
                    DateTime jobEndTime;

                    List<ClientDataUploadContract> lstClientDataUpload = ComplianceDataManager.GetTenantNodeMappingData(ClientDataUploadId);
                    ClientDataUploadContract clientServiceSetting = lstClientDataUpload.FirstOrDefault();
                    // String NodeIds = String.Join(",", lstClientDataUpload.Select(sel => sel.HierarchyNodeID).Distinct().ToList());
                    String SPName = clientServiceSetting.StoreProcedureName;
                    Int32 LoopCounter = clientServiceSetting.LoopCounter;
                    Int32 clientDataUploadId = clientServiceSetting.ClientDataUploadID;


                    //Get all Client node Mapping Data based on ClientdatauploadID

                    List<Int32> lstTenantIds = ComplianceDataManager.GetTenantDataBasedClientDataUpload(ClientDataUploadId);

                    logger.Info("*******************## Tenant loop Start ##*******************");
                    foreach (var TenantId in lstTenantIds)
                    {

                        ClientDBConfiguration clientDbConfs = SecurityManager.GetClientDBConfigurationForSelectedTenants(TenantId);
                        if (CheckDB.TestConnString(clientDbConfs.CDB_ConnectionString))
                        {

                            String NodeIds = String.Join(",", lstClientDataUpload.Where(con => con.TenantId == TenantId).Select(sel => sel.HierarchyNodeID).Distinct().ToList());

                            Int32 tenantId = clientDbConfs.CDB_TenantID;
                            DateTime? processStartdate = DateTime.Now;
                            String tpcduIds = String.Empty;
                            try
                            {


                                for (int Counter = 0; Counter < LoopCounter; Counter++)
                                {
                                    logger.Info("*******************## LoopCouter Loop Start Here ##*******************");
                                    logger.Info("*******************" + "##Current Counter : " + Counter + " And Total Loop Counter " + LoopCounter + " ##*******************");
                                    Dictionary<String, String> result = ComplianceDataManager.GetDataToUpload(Chunksize, NodeIds, SPName, tenantId);

                                    logger.Info("*******************" + "##Current ClientDateUploadId : " + ClientDataUploadId + " ##Current Tenant : " + tenantId + " ##Current Counter : " + Counter + " And Total Loop Counter " + LoopCounter + " TPCDU ID : " + result["TPCDUIds"].ToString() + " ##XML Result : " + result["XMLResult"].ToString() + " ##*******************");

                                    String xmlResultToUpload = String.Empty;
                                    if (result.ContainsKey("XMLResult"))
                                    {
                                        xmlResultToUpload = result["XMLResult"];
                                        if (xmlResultToUpload == null || xmlResultToUpload == string.Empty)
                                        {
                                            logger.Info("*******************No Records Found, Loop Break :" + DateTime.Now.ToString() + " *******************");
                                            break;
                                        }

                                    }
                                    if (result.ContainsKey("TPCDUIds"))
                                    {
                                        tpcduIds = result["TPCDUIds"];
                                    }

                                    List<ClientDataUploadConfiguration> lstClientDataUploadConfiguration = ComplianceDataManager.GetClientDataUploadConfiguration(clientDataUploadId,
                                                                                                                                                                   tenantId);

                                    if (!lstClientDataUploadConfiguration.IsNullOrEmpty() && !xmlResultToUpload.IsNullOrEmpty())
                                    {
                                        //Call Adater method to upload Data.
                                        String serviceConfiguration = CreateXml(lstClientDataUploadConfiguration);

                                        logger.Info("*******************START calling Method: UploadClientData :" + DateTime.Now.ToString() + " *******************");
                                        ClientServiceLibrary.ThirdPartyDataUploadResponse response = ClientDataUploadHelper.ClientDataUploadAdapter().UploadClientData(xmlResultToUpload, serviceConfiguration,
                                                                                                          clientServiceSetting.WebServiceURL, clientServiceSetting.ClassFullName,
                                                                                                          clientServiceSetting.AssemblyLocation,
                                                                                                          clientServiceSetting.ClientRequestFormatName,
                                                                                                          clientServiceSetting.AuthenticationRequestURL, isTestingModeOn, clientServiceSetting.Code, isIgnoreAceMap, IsAceMappExceptionThrow, AceMappErrorCodeAndText);
                                        logger.Info("*******************END calling Method: UploadClientData :" + DateTime.Now.ToString() + " *******************");
                                        List<ThirdPartyDataUploadResponseTypeContract> thirdPartyResponseRegex = StoredProcedureManagers.GetThirdPartyDataUploadResponseRegex(clientDataUploadId, tenantId);
                                        HandleClientResponse(thirdPartyResponseRegex, response);

                                        if (!response.IsNullOrEmpty() && response.ThirdPartyBatchResponse.Count > 0)
                                        {
                                            String tpuIdsWithStatusOK = GetCommaSeparatedTpUIds(response, HttpStatusCode.OK, tpcduIds);
                                            String tpuIdsWithStatusError = GetCommaSeparatedTpUIds(response, HttpStatusCode.InternalServerError, tpcduIds);
                                            String ReposnseXML = !String.IsNullOrEmpty(response.ReposnseXML) ? response.ReposnseXML : response.ThirdPartyBatchResponse.FirstOrDefault().Response.Content.ReadAsStringAsync().Result;

                                            if (IsApiCallByPassed)
                                            {
                                                StoredProcedureManagers.UpdateThirdPartyComplianceDataUploadStatus(tpuIdsWithStatusError, ThirdPartyUploadStatus.UPLOAD_COMPLETE.GetStringValue(),
                                                                                                                    backgroundProcessUserId, tenantId);
                                            }
                                            else if (!String.IsNullOrEmpty(tpuIdsWithStatusOK))
                                            {
                                                //Update Status of uploaded data                                         
                                                StoredProcedureManagers.UpdateThirdPartyComplianceDataUploadStatus(tpuIdsWithStatusOK, ThirdPartyUploadStatus.UPLOAD_COMPLETE.GetStringValue(),
                                                                                                                   backgroundProcessUserId, tenantId);
                                            }
                                            else if (!String.IsNullOrEmpty(tpuIdsWithStatusError))
                                            {
                                                ValidateACEMapp(backgroundProcessUserId, tenantId, tpuIdsWithStatusOK, tpuIdsWithStatusError, ReposnseXML, AceMappErrorCodeAndText);
                                            }

                                            //Save Client Data Upload History.
                                            Entity.SharedDataEntity.ClientDataUploadServiceHistory uploadHistory = new Entity.SharedDataEntity.ClientDataUploadServiceHistory();
                                            uploadHistory.CDUSH_Request = xmlResultToUpload;
                                            uploadHistory.CDUSH_Response = ReposnseXML;
                                            uploadHistory.CDUSH_CreatedOn = DateTime.Now;
                                            uploadHistory.CDUSH_CreatedByID = backgroundProcessUserId;
                                            ComplianceDataManager.CreateClientDataUploadServiceHistory(uploadHistory, tenantId);
                                        }

                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                                logger.Error("An Error has occured in UploadClientData method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                if (!String.IsNullOrEmpty(tpcduIds))
                                    StoredProcedureManagers.UpdateThirdPartyComplianceDataUploadStatus(tpcduIds, ThirdPartyUploadStatus.ERROR.GetStringValue(),
                                                                                                        backgroundProcessUserId, tenantId);
                            }
                        }
                    }
                    logger.Info("*******************## End Start Here##*******************");
                    //Update Client Data upload service wait until time.
                    logger.Info("*******************START calling Method: UpdateClientDataUploadService :" + DateTime.Now.ToString() + " *******************");
                    ComplianceDataManager.UpdateClientDataUploadService(ClientDataUploadId, clientServiceSetting.Frequency, DateTime.Now);
                    logger.Info("*******************END calling Method: UpdateClientDataUploadService :" + DateTime.Now.ToString() + " *******************");

                    //Save service logging data to DB
                    if (_isServiceLoggingEnabled)
                    {
                        logger.Info("*******************_isServiceLoggingEnabled Enabled :  *******************");
                        jobEndTime = DateTime.Now;
                        ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                        serviceLoggingContract.ServiceName = ServiceName.EmailDispatcherService.GetStringValue();
                        serviceLoggingContract.JobName = JobName.UploadClientData.GetStringValue();
                        //serviceLoggingContract.TenantID = ClientDataUploadId;///
                        serviceLoggingContract.JobStartTime = jobStartTime;
                        serviceLoggingContract.JobEndTime = jobEndTime;
                        //serviceLoggingContract.Comments = "";
                        serviceLoggingContract.IsDeleted = false;
                        serviceLoggingContract.CreatedBy = backgroundProcessUserId;
                        serviceLoggingContract.CreatedOn = DateTime.Now;
                        SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                    }
                    ServiceContext.ReleaseDBContextItems();
                }
            }
            catch (Exception ex)
            {
                logger.Error("An Error has occured in UploadClientData method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
            }
        }

        private static void ValidateACEMapp(Int32 backgroundProcessUserId, Int32 tenantId, String tpuIdsWithStatusOK, String tpuIdsWithStatusError, String ReposnseXML, string AceMappErrorCodeAndText)
        {
            if (AceMappErrorCodeAndText != string.Empty && ReposnseXML.ToLower().Contains(AceMappErrorCodeAndText.ToLower()))
            {
                logger.Info("*******************## Throw the exception ACEMAPP      " + AceMappErrorCodeAndText + "       ##*******************");

                StoredProcedureManagers.UpdateThirdPartyComplianceDataUploadStatus(tpuIdsWithStatusError, ThirdPartyUploadStatus.UPLOAD_COMPLETE.GetStringValue(),
                                                                              backgroundProcessUserId, tenantId);
                logger.Info("*******************## Update the ACEMAPP data with Okay status : " + tpuIdsWithStatusError + "  ##*******************");
            }
            else
            {
                StoredProcedureManagers.UpdateThirdPartyComplianceDataUploadStatus(tpuIdsWithStatusError, ThirdPartyUploadStatus.ERROR.GetStringValue(), backgroundProcessUserId, tenantId);
            }
        }

        private static String CreateXml(List<ClientDataUploadConfiguration> lstClientDataUploadConfiguration)
        {
            XElement xmlElements = new XElement("Configuration", lstClientDataUploadConfiguration
                                    .Select(i => new XElement(i.CDUC_Key, i.CDUC_Value)));
            return xmlElements.ToString();
        }

        private static void HandleClientResponse(List<ThirdPartyDataUploadResponseTypeContract> thirdPartyDataUploadResponseTypeContract, ClientServiceLibrary.ThirdPartyDataUploadResponse thirdPartyDataUploadResponse)
        {
            if (thirdPartyDataUploadResponseTypeContract.IsNotNull() && thirdPartyDataUploadResponseTypeContract.Count > 0 && thirdPartyDataUploadResponse.IsNotNull() && thirdPartyDataUploadResponse.ThirdPartyBatchResponse.Count > 0)
            {
                foreach (ClientServiceLibrary.ThirdPartyDataUploadBatchResponse item in thirdPartyDataUploadResponse.ThirdPartyBatchResponse.Where(x => x.Response.StatusCode == HttpStatusCode.OK).ToList())
                {
                    foreach (ThirdPartyDataUploadResponseTypeContract contract in thirdPartyDataUploadResponseTypeContract)
                    {
                        Regex regularExpression = new Regex(@contract.Regex, RegexOptions.IgnoreCase);
                        Match match = regularExpression.Match(item.Response.Content.ReadAsStringAsync().Result);

                        if (match.Success)
                        {
                            if (contract.ThirdPartyUploadOutputTypeCode == ThirdPartyUploadOutputType.Error.GetStringValue())
                            {
                                item.Response.StatusCode = HttpStatusCode.InternalServerError;
                                break;
                            }
                            //else if (contract.ThirdPartyUploadOutputTypeCode == ThirdPartyUploadOutputType.Success.GetStringValue())
                            //{
                            //    item.Response.StatusCode = HttpStatusCode.OK;
                            //}
                            //else if (contract.ThirdPartyUploadOutputTypeCode == ThirdPartyUploadOutputType.IgnoreError.GetStringValue())
                            //{
                            //    item.Response.StatusCode = HttpStatusCode.OK;
                            //}
                        }

                    }
                }
            }
        }
        private static String GetCommaSeparatedTpUIds(ClientServiceLibrary.ThirdPartyDataUploadResponse thirdPartyDataUploadResponse, HttpStatusCode statusCode, String tpcduIds)
        {
            String CommaSeparatedtpcduIds = String.Empty;

            if (!String.IsNullOrEmpty(thirdPartyDataUploadResponse.ReposnseXML))
            {
                CommaSeparatedtpcduIds = String.Join(",", thirdPartyDataUploadResponse.ThirdPartyBatchResponse.Where(x => x.Response.StatusCode == statusCode && x.TPDUId != null).Select(x => x.TPDUId).Distinct().ToArray());
            }
            else
            {
                if (thirdPartyDataUploadResponse.ThirdPartyBatchResponse.Any(x => x.Response.StatusCode == statusCode))
                {
                    CommaSeparatedtpcduIds = tpcduIds;
                }
            }
            return CommaSeparatedtpcduIds;
        }
    }
}
