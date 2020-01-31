using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using INTSOF.Complio.API.ComplioAPIDAL;
using INTSOF.Complio.API.Core;

namespace INTSOF.Complio.API.ComplioAPIBusiness
{
    public class ComplioAPIManager
    {
        public static string GetData(int tenantId, string entityTypeCode, string xmlData)
        {
            return ComplioAPIRespository.GetData(tenantId, entityTypeCode, xmlData);
        }

        public static string AddData(int tenantId, string entityTypeCode, string xmlData)
        {
            return ComplioAPIRespository.AddData(tenantId, entityTypeCode, xmlData);
        }

        public static string UpdateData(int tenantId, string entityTypeCode, string xmlData)
        {
            return ComplioAPIRespository.UpdateData(tenantId, entityTypeCode, xmlData);
        }

        public static void GetCredientials(ref string userName, ref string password)
        {
            string headerData = WebOperationContext.Current.IncomingRequest.Headers[ComplioAPIConstants.AuthorizationHeaderName];

            if (!string.IsNullOrEmpty(headerData))
            {
                string _base64Credentials = headerData.Substring(6);
                string _decodedCredientials = Encoding.ASCII.GetString(Convert.FromBase64String(_base64Credentials));
                userName = _decodedCredientials.Split(':')[0];
                password = _decodedCredientials.Substring(userName.Length + 1);
            }
        }

        public static TokenValidateData GetTokenValidateData(int tenantId, string entityTypeCode, string mappingCode)
        {
            return ComplioAPIRespository.GetTokenValidateData(tenantId, entityTypeCode, mappingCode);
        }

        public static Boolean IsClientTokenValidateRequired(int tenantId, string entityTypeCode)
        {
            return ComplioAPIRespository.IsClientTokenValidateRequired(tenantId, entityTypeCode);
        }

        /// <summary>
        /// Method to check whether the sp return json result or not.
        /// </summary>
        /// <param name="entityTypeCode"></param>
        /// <returns></returns>
        public static Boolean IsJsonReturn(string entityTypeCode)
        {
            return ComplioAPIRespository.IsJsonReturn(entityTypeCode);
        }
    }
}