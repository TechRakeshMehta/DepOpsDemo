using ClientServiceLibrary.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ClientServiceLibrary
{
    public class AuthenticateWithToken
    {
        /// <summary>
        ///  Decode service configuraion detail from XML string format to ServiceConfiguration model
        /// </summary>
        /// <param name="serviceConfiguration">configuraion detail in XML string format</param>
        /// <returns> ServiceConfiguration object</returns>
        ServiceConfiguration GetServiceConfiguration(String serviceConfiguration)
        {
            ServiceConfiguration serviceConfigurationDetails = new ServiceConfiguration();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(serviceConfiguration);

            XmlNodeList parentNode = xmlDoc.GetElementsByTagName("Configuration");
            foreach (XmlNode node in parentNode)
            {
                serviceConfigurationDetails.Username = node.SelectSingleNode("Username").InnerText;
                serviceConfigurationDetails.Password = node.SelectSingleNode("Password").InnerText;
                serviceConfigurationDetails.Grant_Type = node.SelectSingleNode("Grant_Type").InnerText;
                serviceConfigurationDetails.Client_Id = node.SelectSingleNode("Client_Id").InnerText;
                serviceConfigurationDetails.Client_Secret = node.SelectSingleNode("Client_Secret").InnerText;
            }

            return serviceConfigurationDetails;
        }

        /// <summary>
        /// Upload data to third party service
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public HttpResponseMessage UploadData(object[] requestParameters)
        {
            String ServiceConfiguration = requestParameters[0].ToString();
            String AuthRequestUrl = requestParameters[1].ToString();
            String ContentType = requestParameters[2].ToString();
            String UploadRequestUrl = requestParameters[3].ToString();
            String DataToUpload = requestParameters[4].ToString();
            Boolean IsLocalhost = Convert.ToBoolean(requestParameters[5]);
            String AccessToken = string.Empty;

            if (String.Compare(ContentType, "JSON", true) == 0)
            {
                ContentType = "application/json";
                DataToUpload = XMLToJSONConverter.XMLToJSONConversion(DataToUpload);

                if (!String.IsNullOrEmpty(DataToUpload) && !DataToUpload.StartsWith("{\"batchRequests\":["))
                {
                    DataToUpload = DataToUpload.Replace("{\"batchRequests\":", "{\"batchRequests\":[");
                    Int32 lastIndex = DataToUpload.LastIndexOf("}");
                    DataToUpload = DataToUpload.Insert(lastIndex, "]");
                }
            }
            else if (String.Compare(ContentType, "XML", true) == 0)
            {
                ContentType = "application/x-www-form-urlencoded";
            }
            if (IsLocalhost)
                AccessToken = "14c34c47-3893-4a11-8420-6500bec02811";
            else
                AccessToken = GenerateAuthToken(requestParameters);

            String responseMessage = string.Empty;
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                Byte[] userDataToPost = Encoding.ASCII.GetBytes(DataToUpload);
                HttpWebRequest updateWebRequest = (HttpWebRequest)WebRequest.Create(UploadRequestUrl);
                updateWebRequest.Method = "POST";
                updateWebRequest.ContentType = ContentType;
                updateWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
                updateWebRequest.Headers.Add("Timestamp", DateTime.Now.ToString());
                updateWebRequest.ContentLength = userDataToPost.Length;

                using (Stream stream = updateWebRequest.GetRequestStream())
                {
                    stream.Write(userDataToPost, 0, userDataToPost.Length);
                    stream.Flush();
                }

                //Getting API response
                String updateResponse = String.Empty;
                using (WebResponse updateWebResponse = updateWebRequest.GetResponse())
                {
                    var _streamReader = new StreamReader(updateWebResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                    updateResponse = _streamReader.ReadToEnd();
                    _streamReader.Close();
                    responseMessage = "<Response> <ExternalServiceResponse> " + updateResponse + " </ExternalServiceResponse>  <ResponseMessage> Requested Service Successfully Processed. </ResponseMessage> <ResponseCode> 1 </ResponseCode> <Description> Response 1 means data upload successfully and 0 means failed to upload data due to internal server error. </Description> </Response>";
                    response.StatusCode = HttpStatusCode.OK;
                }

            }
            catch (Exception ex)
            {
                responseMessage = "<Response><ResponseMessage>" + ex.Message + "</ResponseMessage>  <ResponseCode> 0 </ResponseCode> <Description> Response 1 means data upload successfully and 0 means failed to upload data due to internal server error. </Description> </Response>";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            //Return Response
            response.Content = new StringContent(responseMessage, Encoding.UTF8, "application/xml");
            return response;

        }

        /// <summary>
        /// Public generate auth token method for future use
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public String GenerateAuthToken(object[] requestParameters)
        {
            String ServiceConfiguration = requestParameters[0].ToString();
            String AuthRequestUrl = requestParameters[1].ToString();
            String ContentType = requestParameters[2].ToString();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            // Decode service configuraion detail from XML string format to ServiceConfiguration model
            ServiceConfiguration serviceConfigurationDetails = GetServiceConfiguration(ServiceConfiguration);

            String _authRequestParameters = "grant_type=" + serviceConfigurationDetails.Grant_Type
                                                          + "&client_id=" + serviceConfigurationDetails.Client_Id
                                                          + "&client_secret=" + serviceConfigurationDetails.Client_Secret
                                                          + "&username=" + serviceConfigurationDetails.Username
                                                          + "&password=" + serviceConfigurationDetails.Password;

            Byte[] _authRequestData = UTF8Encoding.UTF8.GetBytes(_authRequestParameters);

            HttpWebRequest authWebRequest = (HttpWebRequest)WebRequest.Create(AuthRequestUrl);
            authWebRequest.Method = "POST";
            authWebRequest.ContentType = "application/x-www-form-urlencoded";
            authWebRequest.ContentLength = _authRequestData.Length;

            using (Stream stream = authWebRequest.GetRequestStream())
            {
                stream.Write(_authRequestData, 0, _authRequestData.Length);
                stream.Flush();
            }

            SFAuthenticationResponse authResponse;
            using (WebResponse authWebResponse = authWebRequest.GetResponse())
            {
                DataContractJsonSerializer _serializer = new DataContractJsonSerializer(typeof(SFAuthenticationResponse));
                authResponse = _serializer.ReadObject(authWebResponse.GetResponseStream()) as SFAuthenticationResponse;
            }
            return authResponse.access_token;
        }

    }
}
