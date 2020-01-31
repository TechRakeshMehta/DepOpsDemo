using System;
using System.Collections.Generic;
using Entity;
using Entity.ExternalVendorContracts;
using ExternalVendors.Interface;
using ExternalVendors.Utility;
using INTSOF.ServiceUtil;
using INTSOF.Utils;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Net;
using Business.RepoManagers;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;


namespace ExternalVendors.PrintScanVendor
{
    public class PrintScanAdapter : IVendorServiceAdapter
    {

        #region Private Variables
        private static String CreateOrderServiceLogger;
        private static String UpdateOrderServiceLogger;
        private string PrintScanServiceURL;
        private string PrintScanAuthenticateURL;
        private string PrintScanUserName;
        private string PrintScanPassword;
        private string PrintScanHostURL;
        #endregion

        #region Properties

        public List<State> States
        {
            get;
            set;
        }

        #endregion

        #region Public Methods

        public PrintScanAdapter()
        {
            CreateOrderServiceLogger = "CreateOrderServiceLogger";
            UpdateOrderServiceLogger = "UpdateOrderServiceLogger";
            PrintScanServiceURL = ConfigurationManager.AppSettings["PrintScanServiceURL"] ?? string.Empty;
            PrintScanAuthenticateURL = ConfigurationManager.AppSettings["PrintScanAuthenticateURL"] ?? string.Empty;
            PrintScanUserName = ConfigurationManager.AppSettings["PrintScanUserName"] ?? string.Empty;
            PrintScanPassword = ConfigurationManager.AppSettings["PrintScanPassword"] ?? string.Empty;
            PrintScanHostURL = ConfigurationManager.AppSettings["PrintScanHostURL"] ?? string.Empty;
        }

        EvCreateOrderContract IVendorServiceAdapter.DispatchOrderItemsToVendor(EvCreateOrderContract evOrderContract, Int32 tenantID, Boolean isTestModeON)
        {
            try
            {
                DateTime methodStartTime = DateTime.Now;
                ServiceLogger.Info("Started process of disptaching order items to Print Scan vendor.", CreateOrderServiceLogger);

                PrintScan printScan = new PrintScan(evOrderContract.VendorID);
                printScan.States = States;

                //Authenticate
                PrintScanAuthenticationResponseContract psAuthenticationResponseContract = new PrintScanAuthenticationResponseContract();
                DateTime receivedTokenTime = new DateTime();

                evOrderContract.PackageSvcGroups.ForEach(packageSvcGroup =>
                {
                    try
                    {
                        //Authenticate
                        if (psAuthenticationResponseContract.access_token.IsNullOrEmpty()
                            || (!string.IsNullOrEmpty(psAuthenticationResponseContract.access_token) && DateTime.Now.Ticks > receivedTokenTime.AddSeconds(psAuthenticationResponseContract.expires_in - AppConsts.TEN).Ticks)
                            )
                        {
                            ServiceLogger.Info("Authenticating to print scan service.", CreateOrderServiceLogger);
                            using (WebClient client = new WebClient())
                            {
                                client.Headers[HttpRequestHeader.Accept] = "application/x-www-form-urlencoded";
                                client.Headers[HttpRequestHeader.Host] = PrintScanHostURL;
                                string requestBodyForAuthentication = string.Format("grant_type=password&username={0}&password={1}", PrintScanUserName, PrintScanPassword);
                                byte[] responseForAuthentication = client.UploadData(PrintScanAuthenticateURL, Encoding.UTF8.GetBytes(requestBodyForAuthentication));
                                receivedTokenTime = DateTime.Now;
                                string responseJson = Encoding.UTF8.GetString(responseForAuthentication);
                                psAuthenticationResponseContract = printScan.CreateContractFromResponse<PrintScanAuthenticationResponseContract>(responseJson);
                            }
                            ServiceLogger.Info("Authentication complete to print scan service.", CreateOrderServiceLogger);
                        }

                        List<ExternalVendors.ClearStarVendor.ClearStar.ParamObject> lstParameters = new List<ExternalVendors.ClearStarVendor.ClearStar.ParamObject>();
                        PrintScanResponseContract printScanResponseContract = new PrintScanResponseContract();
                        StringWriter parameterData = new StringWriter();

                        foreach (var lineItem in packageSvcGroup.OrderItems)
                        {
                            try
                            {
                                using (WebClient client = new WebClient())
                                {
                                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                                    client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                                    client.Headers[HttpRequestHeader.Authorization] = string.Format("{0} {1}", psAuthenticationResponseContract.token_type, psAuthenticationResponseContract.access_token);
                                    string requestBody = printScan.GetJsonForExtendedInformation(lineItem.EvOrderItemAttributeContracts, evOrderContract, ref lstParameters);

                                    //Serialize parameter Data
                                    using (parameterData = new StringWriter())
                                    {
                                        XmlSerializer createParameterSerializer = new XmlSerializer(typeof(List<ExternalVendors.ClearStarVendor.ClearStar.ParamObject>));
                                        createParameterSerializer.Serialize(parameterData, lstParameters);
                                    }

                                    byte[] response = client.UploadData(PrintScanServiceURL, Encoding.UTF8.GetBytes(requestBody));
                                    string responseJson = Encoding.UTF8.GetString(response);
                                    printScanResponseContract = printScan.CreateContractFromResponse<PrintScanResponseContract>(responseJson);

                                    DateTime methodEndTime = DateTime.Now;

                                    StringWriter responseData = new StringWriter();
                                    using (responseData = new StringWriter())
                                    {
                                        XmlSerializer createParameterSerializer = new XmlSerializer(typeof(PrintScanResponseContract));
                                        createParameterSerializer.Serialize(responseData, printScanResponseContract);
                                    }

                                    ExternalVendorOrderManager.SaveExtSvcInvokeHistory(tenantID, evOrderContract.BkgOrderID, lineItem.BkgOrderPackageSvcLineItemID
                                                                                       , packageSvcGroup.BkgOrderPkgSvcGroupID, "DispatchOrderItemsToVendor--PrintScan"
                                                                                       , parameterData.ToString(), responseData.ToString(), string.Empty
                                                                                       , methodStartTime, methodEndTime, string.Empty);

                                    lineItem.ExternalVendorOrderID = Convert.ToString(printScanResponseContract.applicantId);
                                    lineItem.VendorResponse = new VendorResponse();
                                    lineItem.VendorResponse.IsVendorError = false;
                                    lineItem.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                                 "Order for Print Scan Vendor Dispatch successfully. "
                                                                 + evOrderContract.BkgOrderID
                                                                 , String.Empty);
                                }
                            }
                            catch (WebException webEx)
                            {
                                if (webEx.Response.IsNotNull())
                                {
                                    using (var errorResponse = (HttpWebResponse)webEx.Response)
                                    {
                                        if (errorResponse.ContentType.Contains("application/json"))
                                        {
                                            using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                                            {
                                                string error = reader.ReadToEnd();

                                                byte[] bytes = Encoding.ASCII.GetBytes(error);
                                                XmlDocument doc = new XmlDocument();
                                                using (var stream = new MemoryStream(bytes))
                                                {
                                                    var quotas = new XmlDictionaryReaderQuotas();
                                                    var jsonReader = JsonReaderWriterFactory.CreateJsonReader(stream, quotas);

                                                    doc.Load(jsonReader);
                                                }

                                                //packageSvcGroup.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,doc.InnerXml, "System Exception");
                                                //packageSvcGroup.VendorResponse.IsVendorError = true;
                                                //ServiceLogger.Error(doc.InnerXml.ToString(), CreateOrderServiceLogger);
                                                ExternalVendorOrderManager.SaveExtSvcInvokeHistory(tenantID, evOrderContract.BkgOrderID
                                                                                                   , lineItem.BkgOrderPackageSvcLineItemID, packageSvcGroup.BkgOrderPkgSvcGroupID
                                                                                                   , "DispatchOrderItemsToVendor--PrintScan", parameterData.ToString()
                                                                                                   , doc.InnerXml, string.Empty, methodStartTime, DateTime.Now, string.Empty);

                                                ServiceLogger.Error(doc.InnerXml.ToString(), CreateOrderServiceLogger);
                                                lineItem.VendorResponse = new VendorResponse();
                                                lineItem.VendorResponse.IsVendorError = true;
                                                lineItem.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty, doc.InnerXml, "WebException");

                                            }
                                        }
                                        else if (errorResponse.ContentType.Contains("text/html"))
                                        {
                                            ServiceLogger.Error(webEx.Message, CreateOrderServiceLogger);
                                            ServiceLogger.Error("Web exception may be due to problem in authorization token, while requesting to print scan service for line items", CreateOrderServiceLogger);
                                            lineItem.VendorResponse = new VendorResponse();
                                            lineItem.VendorResponse.IsVendorError = true;
                                            lineItem.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty, webEx.Message, "WebException");

                                        }

                                    }
                                }

                                //ExternalVendorOrderManager.SaveExtSvcInvokeHistory(tenantID, evOrderContract.BkgOrderID
                                //                                                             , lineItem.BkgOrderPackageSvcLineItemID, packageSvcGroup.BkgOrderPkgSvcGroupID
                                //                                                             , "DispatchOrderItemsToVendor--PrintScan", parameterData.ToString()
                                //                                                             , string.Concat(webEx.Message, webEx.InnerException, webEx.StackTrace), string.Empty, methodStartTime, DateTime.Now, string.Empty);

                                ServiceLogger.Error(string.Concat(webEx.Message, webEx.InnerException, webEx.StackTrace), CreateOrderServiceLogger);
                                if (lineItem.VendorResponse.IsNull())
                                {
                                    lineItem.VendorResponse = new VendorResponse();
                                    lineItem.VendorResponse.IsVendorError = true;
                                    lineItem.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty, string.Concat(webEx.Message, webEx.InnerException, webEx.StackTrace), "WebException");
                                }

                                throw webEx;
                            }
                            catch (Exception ex)
                            {
                                string errorMsg = String.Format("An Error has occured in DispatchOrderItemsToVendor method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                                   ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                                ServiceLogger.Error(errorMsg, CreateOrderServiceLogger);
                                lineItem.VendorResponse = new VendorResponse();
                                lineItem.VendorResponse.IsVendorError = true;
                                lineItem.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty, errorMsg.ToString(), "Exception");
                                throw ex;
                            }
                        }

                        packageSvcGroup.BkgOrderVendorProfileID = Convert.ToString(printScanResponseContract.applicantId);
                        packageSvcGroup.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                                 "Order for Print Scan Vendor Dispatch successfully. "
                                                                 + evOrderContract.BkgOrderID
                                                                 , String.Empty);

                        packageSvcGroup.VendorResponse.IsVendorError = false;
                    }
                    catch (WebException webEx)
                    {
                        if (webEx.Response.IsNotNull())
                        {
                            using (var errorResponse = (HttpWebResponse)webEx.Response)
                            {
                                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                                {
                                    string error = reader.ReadToEnd();
                                    ServiceLogger.Error(error, CreateOrderServiceLogger);
                                }
                            }
                        }
                        packageSvcGroup.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty, webEx.Message, "System Exception");
                        packageSvcGroup.VendorResponse.IsVendorError = true;
                        ServiceLogger.Error(webEx.Message, CreateOrderServiceLogger);
                        throw webEx;
                    }
                    catch (Exception ex)
                    {
                        packageSvcGroup.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                                                       ex.Message, "System Exception");
                        packageSvcGroup.VendorResponse.IsVendorError = true;
                        string errorMsg = String.Format("An Error has occured in DispatchOrderItemsToVendor method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                           ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                        ServiceLogger.Error(errorMsg, CreateOrderServiceLogger);
                        throw ex;
                    }

                });
                ServiceLogger.Info("Ended process of disptaching order items to ADB vendor.", CreateOrderServiceLogger);
            }

            catch (Exception ex)
            {
                string errorMsg = String.Format("An Error has occured in DispatchOrderItemsToVendor method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                   ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString);
                ServiceLogger.Error(errorMsg, CreateOrderServiceLogger);
            }
            return evOrderContract;
        }

        EvUpdateOrderContract IVendorServiceAdapter.UpdateVendorBkgOrder(EvUpdateOrderContract evUpdateOrderContract, int tenantID)
        {
            evUpdateOrderContract.VendorResponse = new VendorResponse();
            evUpdateOrderContract.VendorResponse.IsVendorError = false;

            evUpdateOrderContract.VendorProfileStatus = ClearstarProfileStatus.Completed;

            EvUpdateOrderPackageSvcGroup serviceGroupToUpdate = evUpdateOrderContract.EvUpdateOrderPackageSvcGroup;

            evUpdateOrderContract.EvUpdateOrderItemContract.ForEach(lineItem =>
            {
                lineItem.ResultText = String.Empty;
                lineItem.ResultXML = String.Empty;
                lineItem.SvcLineItemFlaggedInd = false;
                lineItem.OrderLineItemResultStatusID = 4;
                lineItem.DateCompleted = DateTime.Now;
            });

            #region  Commented Below code UAT-1357
            //UAT-1244            
            //var isAllSvcGroupLineItemsCompleted = CommonHelper.IsAllSvcGroupLineItemsCompleted(evUpdateOrderContract
            //                                                                                   , evUpdateOrderContract.ExternalVendorBkgOrderDetailID, tenantID);
            //if (isAllSvcGroupLineItemsCompleted)
            //{
            //    serviceGroupToUpdate.ServiceGroupNewReviewStatusCode = BkgSvcGrpReviewStatusType.AUTO_REVIEW_COMPLETED.GetStringValue();
            //}

            #endregion

            CommonHelper.UpdateServiceGroupStatus(false, evUpdateOrderContract, tenantID, UpdateOrderServiceLogger);

            return evUpdateOrderContract;
        }

        void IVendorServiceAdapter.RevertExternalVendorChanges(String loginName, String password, Int32 boid, String customerID, String vendorProfileNumber,
           Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcGroupID)
        {
            //throw new NotImplementedException();
        }

        #endregion

    }

}
