using INTSOF.Complio.API.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using INTSOF.Utils;

namespace INTSOF.Complio.API
{
    public static class APIOutputFormatter
    {
        public static String GetFormattedResult(Boolean isXML, String data, ServiceStatus serviceStatus, Boolean isDataJsonForm)
        {
            ServiceResponse resp = null;
            ServiceResponseJson jresp = null;

            if (!isDataJsonForm)
            {
                resp = new ServiceResponse();
                resp.Status = serviceStatus;
                XmlDocument _xmlDoc = new XmlDocument();
                _xmlDoc.LoadXml(data);
                resp.Result = _xmlDoc;
            }
            else
            {
                jresp = new ServiceResponseJson();
                jresp.Status = serviceStatus;
                if (data != null)
                    jresp.Result = data;// .Replace("\\\"", "\"");
            }

            if (resp != null)
            {
                if (isXML)
                {
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add(string.Empty, string.Empty);

                    StringWriter _stringwriter = new StringWriter();
                    XmlSerializer _serializer = new XmlSerializer(typeof(ServiceResponse));
                    _serializer.Serialize(_stringwriter, resp, namespaces);
                    return WebUtility.HtmlDecode(Convert.ToString(_stringwriter));
                }
                else
                {
                    ServiceResponseWrapper _wrapper = new ServiceResponseWrapper();
                    _wrapper.ServiceResponse = resp;

                    return JsonConvert.SerializeObject(_wrapper);
                }
            }
            else
            {
                ServiceResponseJsonWrapper _wrapper = new ServiceResponseJsonWrapper();
                _wrapper.ServiceResponseJson = new ServiceResponseJson();
                String jsonResult = AppConsts.ORDER_DETAILS_JSON_RESULT ;

                _wrapper.ServiceResponseJson.Status = jresp.Status;
                _wrapper.ServiceResponseJson.Result = jsonResult;
                //ServiceResponseJson serviceResponse = new ServiceResponseJson();
                //serviceResponse.Status = jresp.Status;
                var serResp = JsonConvert.SerializeObject(_wrapper);
                if (serResp != null)
                    serResp = serResp.Replace('"' + jsonResult + '"', jresp.Result);
                return serResp;
                //return JsonConvert.SerializeObject(new
                //{
                //    serviceResponse = jresp
                //});
            }
        }
    }
}