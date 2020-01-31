using ClientServiceLibrary.BAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientServiceLibrary
{
    public class AceMapValidation : ValidateToken
    {
        public override Boolean ValidateClientToken(Int32 schoolId, String entityTypeCode, String mappingCode,String txnToken)
        {
            Boolean isVerifiedRequest = false;

            //Get token validate data for requested mapping code.
            TokenValidateDataContract tokenValidateData = Manager.GetTokenValidateData(schoolId, entityTypeCode, mappingCode);

            //Check is token validation required for this client.
            if (tokenValidateData != null
                && tokenValidateData.TokenValidateURL != null && tokenValidateData.TokenValidateURL.Trim()!= String.Empty
                && tokenValidateData.TokenValidateFormat!=null)
            {
                String validateRequestParameter = String.Empty;
                String txnTokenFromated = "\"" + txnToken + "\"";
                validateRequestParameter = String.Format(tokenValidateData.TokenValidateFormat, txnTokenFromated);
                validateRequestParameter = "{" + validateRequestParameter + "}";
                Byte[] validateRequestData = UTF8Encoding.UTF8.GetBytes(validateRequestParameter);

                try
                {
                    HttpWebRequest validateTokenWebRequest = (HttpWebRequest)WebRequest.Create(tokenValidateData.TokenValidateURL);
                    validateTokenWebRequest.Method = "POST";
                    validateTokenWebRequest.ContentType = "application/json";
                    validateTokenWebRequest.ContentLength = validateRequestData.Length;

                    using (Stream stream = validateTokenWebRequest.GetRequestStream())
                    {
                        stream.Write(validateRequestData, 0, validateRequestData.Length);
                        stream.Flush();
                    }

                    ValidateTokenResponse validateResponse;
                    using (WebResponse authWebResponse = validateTokenWebRequest.GetResponse())
                    {
                        DataContractJsonSerializer _serializer = new DataContractJsonSerializer(typeof(ValidateTokenResponse));
                        validateResponse = _serializer.ReadObject(authWebResponse.GetResponseStream()) as ValidateTokenResponse;
                    }

                    //Verify is valid token request.
                    if (String.Compare(validateResponse.success, "true", true) == 0)
                    {
                        isVerifiedRequest = true;
                    }
                }
                catch (Exception ex)
                {
                    isVerifiedRequest = false;
                }
            }

            return isVerifiedRequest;

        }
    }
}
