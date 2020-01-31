using ClientServiceLibrary.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ClientServiceLibrary
{
    public class AuthenticateWithCredentials
    {

        /// <summary>
        /// Decode service configuraion detail from XML string format to ServiceConfiguration model
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
            String ContentType = requestParameters[1].ToString();
            String UploadRequestUrl = requestParameters[2].ToString();
            String DataToUpload = requestParameters[3].ToString();
            if (String.Compare(ContentType, "JSON", true) == 0)
            {
                ContentType = "application/json";
                DataToUpload = XMLToJSONConverter.XMLToJSONConversion(DataToUpload);
            }
            else if (String.Compare(ContentType, "XML", true) == 0)
            {
                ContentType = "application/x-www-form-urlencoded";
            }

            // Decode service configuraion detail from XML string format to ServiceConfiguration model
            ServiceConfiguration serviceConfigurationDetails = GetServiceConfiguration(ServiceConfiguration);

            String responseMessage = string.Empty;
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                Byte[] userDataToPost = Encoding.ASCII.GetBytes(DataToUpload);
                HttpWebRequest updateWebRequest = (HttpWebRequest)WebRequest.Create(UploadRequestUrl);
                updateWebRequest.Method = "POST";
                updateWebRequest.ContentType = ContentType;
                String encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serviceConfigurationDetails.Username + ":" + serviceConfigurationDetails.Password));
                updateWebRequest.Headers.Add("Authorization", "Basic " + encoded);

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
    }
}
