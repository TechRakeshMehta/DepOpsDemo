using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;
using System.Net.Http;

namespace INTSOF.ServiceUtil
{
    public class DataUploadServiceAdapter : IDataUploadServiceAdapter
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion


        #region Methods

        #region PrivateMethods
        #endregion

        #region PublicMethods

        /// <summary>
        /// Method to Generate authentication Token to upload data.
        /// </summary>
        /// <returns>String</returns>
        String IDataUploadServiceAdapter.GenerateAuthToken(String serviceConfiguration, String className, String assemblyLocation, String contentType, String authRequestUrl)
        {
            object[] requestParameters = new object[3];

            requestParameters[0] = serviceConfiguration;
            requestParameters[1] = authRequestUrl;
            requestParameters[2] = contentType;

            //Load the dll
            Assembly assembly = Assembly.LoadFrom(assemblyLocation);

            //Get Class Type
            Type type = assembly.GetType(className);

            // create instance of class Calculator
            object classInstance = Activator.CreateInstance(type);

            // invoke public instance method: public void GetData()
            Object result = type.InvokeMember("GenerateAuthToken",
                BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                null, classInstance, new object[] { requestParameters });

            return Convert.ToString(result);
        }

        ///// <summary>
        ///// Method to upload client data using token.
        ///// </summary>
        ///// <returns>Response</returns>
        //public HttpResponseMessage UploadDataUsingToken(String dataToUpload,String serviceConfiguration,String serviceRequestUrl,String className,String assemblyLocation,String contentType)
        //{
        //    return new HttpResponseMessage();
        //    //{
        //    //    Content = new StringContent(response, Encoding.UTF8, "application/xml")
        //    //};
        //}

        /// <summary>
        /// Method to upload client data using credentials.
        /// </summary>
        /// <returns></returns>
        ClientServiceLibrary.ThirdPartyDataUploadResponse IDataUploadServiceAdapter.UploadClientData(String dataToUpload, String serviceConfiguration, String serviceRequestUrl, String className,
                                                                       String assemblyLocation, String contentType, String authRequestUrl, Boolean isLocalHost, String code, Boolean isIgnoreAceMap, Boolean IsAceMappExceptionThrow, string AceMappErrorCodeAndText)
        {

            object[] requestParameters = new object[7];

            requestParameters[0] = serviceConfiguration;
            requestParameters[1] = authRequestUrl;
            requestParameters[2] = contentType;
            requestParameters[3] = serviceRequestUrl;
            requestParameters[4] = dataToUpload;
            requestParameters[5] = isLocalHost;
            requestParameters[6] = isIgnoreAceMap;
        

            Object result = new Object();
            ClientServiceLibrary.ThirdPartyDataUploadResponse returnResponse = new ClientServiceLibrary.ThirdPartyDataUploadResponse();
            returnResponse.ThirdPartyBatchResponse = new List<ClientServiceLibrary.ThirdPartyDataUploadBatchResponse>();

            if (!String.IsNullOrEmpty(assemblyLocation))
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

                //Load the dll
                Assembly assembly = Assembly.LoadFrom(assemblyLocation);

                //Get Class Type
                Type type = assembly.GetType(className);

                // create instance of class 
                object classInstance = Activator.CreateInstance(type);

                // invoke public instance method: public void GetData()
                result = type.InvokeMember("UploadData",
                   BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                   null, classInstance, new object[] { requestParameters });
            }
            else
            {
                if (!String.IsNullOrEmpty(code) && code.ToUpper() == "ACEMAPP")
                {
                    ClientServiceLibrary.AceMappAuthentication aceMappAuth = new ClientServiceLibrary.AceMappAuthentication();
                    if (IsAceMappExceptionThrow)
                    {
                        result = aceMappAuth.UploadData(requestParameters, IsAceMappExceptionThrow, AceMappErrorCodeAndText);

                    }
                    else
                    {
                        result = aceMappAuth.UploadData(requestParameters);
                    }

                }
                else if (!String.IsNullOrEmpty(code) && code.ToUpper() == "WCU")
                {
                    ClientServiceLibrary.WestCoastClientFTPIntegration wcUFTPIntegration = new ClientServiceLibrary.WestCoastClientFTPIntegration();
                    result = wcUFTPIntegration.UploadData(requestParameters);
                }
                else if (!String.IsNullOrEmpty(code) && code.ToUpper() == "WCU")
                {
                    ClientServiceLibrary.WestCoastClientFTPIntegration wcUFTPIntegration = new ClientServiceLibrary.WestCoastClientFTPIntegration();
                    result = wcUFTPIntegration.UploadData(requestParameters);
                }
                else if (!String.IsNullOrEmpty(code) && code.ToUpper() == "TYPHON")
                {
                    ClientServiceLibrary.TyphonAuthentication typhonAuthentication = new ClientServiceLibrary.TyphonAuthentication();
                    return TranslateIntegrationResponse(typhonAuthentication.UploadData(requestParameters));
                }
                else
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                    ClientServiceLibrary.AuthenticateWithToken auth = new ClientServiceLibrary.AuthenticateWithToken();
                    result = auth.UploadData(requestParameters);
                }
            }

            returnResponse.ThirdPartyBatchResponse.Add(new ClientServiceLibrary.ThirdPartyDataUploadBatchResponse() {  Response = result as HttpResponseMessage });
            return returnResponse;
        }
        private ClientServiceLibrary.ThirdPartyDataUploadResponse TranslateIntegrationResponse(ClientServiceLibrary.ThirdPartyDataUploadResponse batchResponse)
        {
            List<String> recordsWithInternalServerErrorStatus = new List<String>();

            if (batchResponse != null && batchResponse.ThirdPartyBatchResponse.Count > 0)
            {
                recordsWithInternalServerErrorStatus = batchResponse.ThirdPartyBatchResponse.Where(x => x.Response.StatusCode == HttpStatusCode.InternalServerError).Select(x => x.TPDUId).ToList();

                batchResponse.ThirdPartyBatchResponse.ForEach(x =>
                        {
                            if (recordsWithInternalServerErrorStatus.Contains(x.TPDUId) && x.Response.StatusCode == HttpStatusCode.OK)
                            {
                                x.Response.StatusCode = HttpStatusCode.InternalServerError;
                            }
                        });
            }
            return batchResponse;
        }

        #endregion

        #endregion
    }
}
