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
   public class AceMappAuthentication
    {
        /// <summary>
        /// Upload data to third party service
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
       public HttpResponseMessage UploadData(object[] requestParameters, Boolean IsAceMappExceptionThrow = false,string AceMappErrorCodeAndText="")
        {
            String ServiceConfiguration = requestParameters[0].ToString();
            String AuthRequestUrl = requestParameters[1].ToString();
            String ContentType = requestParameters[2].ToString();
            String UploadRequestUrl = requestParameters[3].ToString();
            String DataToUpload = requestParameters[4].ToString();
            Boolean IsLocalhost = Convert.ToBoolean(requestParameters[5]);
            Boolean isIgnoreAceMap = Convert.ToBoolean(requestParameters[6]);

            if (String.Compare(ContentType, "JSON", true) == 0)
            {
                ContentType = "application/json";
                DataToUpload = XMLToJSONConverter.XMLToJSONConversion(DataToUpload);
            }
            else if (String.Compare(ContentType, "XML", true) == 0)
            {
                ContentType = "application/x-www-form-urlencoded";
            }

            String responseMessage = string.Empty;
            HttpResponseMessage response = new HttpResponseMessage();
            if (isIgnoreAceMap)
            {
                responseMessage = "<Response> <ExternalServiceResponse> Acemap request ignored. </ExternalServiceResponse>  <ResponseMessage> Acemap request ignored. </ResponseMessage> <ResponseCode> 1 </ResponseCode> <Description> Response 1 means data upload successfully and 0 means failed to upload data due to internal server error. </Description> </Response>";
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StringContent(responseMessage, Encoding.UTF8, "application/xml");
                return response;
            }
            try
            {
                Byte[] userDataToPost = Encoding.ASCII.GetBytes(DataToUpload);
                HttpWebRequest updateWebRequest = (HttpWebRequest)WebRequest.Create(UploadRequestUrl);
                updateWebRequest.Method = "POST";
                updateWebRequest.ContentType = ContentType;   
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
                if (IsAceMappExceptionThrow)
                {
                    responseMessage = "<Response><ResponseMessage>" + AceMappErrorCodeAndText + "</ResponseMessage>  <ResponseCode> 0 </ResponseCode> <Description> Response 1 means data upload successfully and 0 means failed to upload data due to internal server error. </Description> </Response>";
                    response.StatusCode = HttpStatusCode.InternalServerError;
                }
                else
                {
                    responseMessage = "<Response><ResponseMessage>" + ex.Message + "</ResponseMessage>  <ResponseCode> 0 </ResponseCode> <Description> Response 1 means data upload successfully and 0 means failed to upload data due to internal server error. </Description> </Response>";
                    response.StatusCode = HttpStatusCode.InternalServerError;
                }
            }

            //Return Response
            response.Content = new StringContent(responseMessage, Encoding.UTF8, "application/xml");
            return response;
        }
    }
}
