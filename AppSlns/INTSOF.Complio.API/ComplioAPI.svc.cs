using System;
using System.Net;
using INTSOF.Complio.API.Core;
using INTSOF.Complio.API.ComplioAPIBusiness;
using System.Xml;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;
using INTSOF.Complio.API.ReportExecution;
using INTSOF.Utils;
using System.Runtime.Serialization.Json;
using System.Configuration;
using System.Text;
using ClientServiceLibrary;

namespace INTSOF.Complio.API
{
    public class ComplioAPI : ComplioBaseAPI, IComplioAPI
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public String GetData(RequestData requestData)
        {
            ServiceResponse _response = new ServiceResponse();

            try
            {
                //Validate token for client request.
                if (ValidateRequestToken(requestData))
                {
                    Boolean IsDataJsonForm = false;
                    if (!requestData.IsNullOrEmpty())
                        IsDataJsonForm = ComplioAPIManager.IsJsonReturn(requestData.EntityTypeCode);

                    #region Add OrganizationUserId to the InputXml

                    String xmlData = String.Empty;

                    if (!String.IsNullOrEmpty(requestData.InputData))
                    {
                        xmlData = base.TryParseXml(requestData.InputData);
                    }
                    else
                    {
                        xmlData = ComplioAPIConstants.XmlDataOpenTag + ComplioAPIConstants.XmlDataCloseTag;
                    }

                    xmlData = base.UpdateXmlInput(xmlData);

                    #endregion

                    String _data = ComplioAPIManager.GetData(Convert.ToInt32(requestData.SchoolId), requestData.EntityTypeCode, xmlData);

                    if (!String.IsNullOrEmpty(_data) && string.Compare(requestData.EntityTypeCode, EntityType.GetScreeningPackagesByOrderIds.GetStringValue()) == 0)
                    {
                        _data = UpdateResultsByAddingPdfBytes(requestData.SchoolId, _data);
                    }

                    /*
                     _response.Status = new ServiceStatus
                     {
                         Code = Convert.ToInt32(HttpStatusCode.OK),
                         Message = ComplioAPIConstants.SuccessStatusMessge,
                         Type = ComplioAPIConstants.StatusSuccessType
                     };

                     #region Return Result as Xml or Json

                     XmlDocument _xmlDoc = new XmlDocument();
                     _xmlDoc.LoadXml(_data);
                     _response.Result = _xmlDoc;
                     */

                    ServiceStatus serviceStatus = new ServiceStatus();
                    serviceStatus.Code = Convert.ToInt32(HttpStatusCode.OK);
                    serviceStatus.Message = ComplioAPIConstants.SuccessStatusMessge;
                    serviceStatus.Type = ComplioAPIConstants.StatusSuccessType;

                    return APIOutputFormatter.GetFormattedResult(base.IsXmlRequest(requestData.Format), _data, serviceStatus, IsDataJsonForm);

                    /*
                    // Return Xml, if Requset Format is not provided.
                    if (base.IsXmlRequest(requestData.Format))
                    {
                        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                        namespaces.Add(string.Empty, string.Empty);

                        StringWriter _stringwriter = new StringWriter();
                        XmlSerializer _serializer = new XmlSerializer(typeof(ServiceResponse));
                        _serializer.Serialize(_stringwriter, _response, namespaces);
                        return WebUtility.HtmlDecode(Convert.ToString(_stringwriter));
                    }
                    else
                    {
                        ServiceResponseWrapper _wrapper = new ServiceResponseWrapper();
                        _wrapper.ServiceResponse = _response;

                        return JsonConvert.SerializeObject(_wrapper);
                    }
                    */

                    //  #endregion
                }
                else
                {
                    _response.Result = null;
                    _response.Status = base.SetValidateTokenStatus();

                    if (base.IsXmlRequest(requestData.Format))
                    {
                        StringWriter _stringwriter = new StringWriter();
                        XmlSerializer _serializer = new XmlSerializer(typeof(ServiceResponse));
                        _serializer.Serialize(_stringwriter, _response);
                        return WebUtility.HtmlDecode(Convert.ToString(_stringwriter));
                    }
                    else
                    {
                        ServiceResponseWrapper _wrapper = new ServiceResponseWrapper();
                        _wrapper.ServiceResponse = _response;
                        return JsonConvert.SerializeObject(_wrapper);
                    }
                }
            }
            catch (Exception ex)
            {
                _response.Result = null;
                _response.Status = base.SetExceptionType(ex);

                if (base.IsXmlRequest(requestData.Format))
                {
                    StringWriter _stringwriter = new StringWriter();
                    XmlSerializer _serializer = new XmlSerializer(typeof(ServiceResponse));
                    _serializer.Serialize(_stringwriter, _response);
                    return WebUtility.HtmlDecode(Convert.ToString(_stringwriter));
                }
                else
                {
                    ServiceResponseWrapper _wrapper = new ServiceResponseWrapper();
                    _wrapper.ServiceResponse = _response;
                    return JsonConvert.SerializeObject(_wrapper);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public String AddData(RequestData requestData)
        {
            ServiceResponse _response = new ServiceResponse();

            try
            {
                //Validate token for client request.
                if (ValidateRequestToken(requestData))
                {
                    #region Add OrganizationUserId to the InputXml

                    String xmlData = String.Empty;

                    if (!string.IsNullOrEmpty(requestData.InputData))
                    {
                        xmlData = base.TryParseXml(requestData.InputData);
                    }
                    else
                    {
                        xmlData = ComplioAPIConstants.XmlDataOpenTag + ComplioAPIConstants.XmlDataCloseTag;
                    }

                    xmlData = base.UpdateXmlInput(xmlData);

                    #endregion

                    String _data = ComplioAPIManager.AddData(Convert.ToInt32(requestData.SchoolId), requestData.EntityTypeCode, xmlData);

                    _response.Status = new ServiceStatus
                    {
                        Code = Convert.ToInt32(HttpStatusCode.OK),
                        Message = ComplioAPIConstants.SuccessStatusMessge,
                        Type = ComplioAPIConstants.StatusSuccessType
                    };

                    #region Return Result as Xml or Json

                    XmlDocument _xmlDoc = new XmlDocument();
                    _xmlDoc.LoadXml(_data);
                    _response.Result = _xmlDoc;

                    // Return Xml, if Requset Format is not provided.
                    if (base.IsXmlRequest(requestData.Format))
                    {
                        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                        namespaces.Add(string.Empty, string.Empty);

                        StringWriter _stringwriter = new StringWriter();
                        XmlSerializer _serializer = new XmlSerializer(typeof(ServiceResponse));
                        _serializer.Serialize(_stringwriter, _response, namespaces);
                        return WebUtility.HtmlDecode(Convert.ToString(_stringwriter));
                    }
                    else
                    {
                        ServiceResponseWrapper _wrapper = new ServiceResponseWrapper();
                        _wrapper.ServiceResponse = _response;

                        return JsonConvert.SerializeObject(_wrapper);
                    }

                    #endregion

                }
                else
                {
                    _response.Result = null;
                    _response.Status = base.SetValidateTokenStatus();

                    if (base.IsXmlRequest(requestData.Format))
                    {
                        StringWriter _stringwriter = new StringWriter();
                        XmlSerializer _serializer = new XmlSerializer(typeof(ServiceResponse));
                        _serializer.Serialize(_stringwriter, _response);
                        return WebUtility.HtmlDecode(Convert.ToString(_stringwriter));
                    }
                    else
                    {
                        ServiceResponseWrapper _wrapper = new ServiceResponseWrapper();
                        _wrapper.ServiceResponse = _response;
                        return JsonConvert.SerializeObject(_wrapper);
                    }
                }
            }
            catch (Exception ex)
            {
                _response.Result = null;
                _response.Status = base.SetExceptionType(ex);

                if (base.IsXmlRequest(requestData.Format))
                {
                    StringWriter _stringwriter = new StringWriter();
                    XmlSerializer _serializer = new XmlSerializer(typeof(ServiceResponse));
                    _serializer.Serialize(_stringwriter, _response);
                    return WebUtility.HtmlDecode(Convert.ToString(_stringwriter));
                }
                else
                {
                    ServiceResponseWrapper _wrapper = new ServiceResponseWrapper();
                    _wrapper.ServiceResponse = _response;
                    return JsonConvert.SerializeObject(_wrapper);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public String UpdateData(RequestData requestData)
        {
            ServiceResponse _response = new ServiceResponse();

            try
            {
                //Validate token for client request.
                if (ValidateRequestToken(requestData))
                {
                    #region Add OrganizationUserId to the InputXml

                    String xmlData = String.Empty;

                    if (!string.IsNullOrEmpty(requestData.InputData))
                    {
                        xmlData = base.TryParseXml(requestData.InputData);
                    }
                    else
                    {
                        xmlData = ComplioAPIConstants.XmlDataOpenTag + ComplioAPIConstants.XmlDataCloseTag;
                    }

                    xmlData = base.UpdateXmlInput(xmlData);

                    #endregion

                    String _data = ComplioAPIManager.UpdateData(Convert.ToInt32(requestData.SchoolId), requestData.EntityTypeCode, xmlData);

                    #region Return Result as Xml or Json

                    XmlDocument _xmlDoc = new XmlDocument();
                    _xmlDoc.LoadXml(_data);
                    _response.Result = _xmlDoc;

                    // Return Xml, if Requset Format is not provided.
                    if (base.IsXmlRequest(requestData.Format))
                    {
                        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                        namespaces.Add(string.Empty, string.Empty);

                        StringWriter _stringwriter = new StringWriter();
                        XmlSerializer _serializer = new XmlSerializer(typeof(ServiceResponse));
                        _serializer.Serialize(_stringwriter, _response, namespaces);
                        return WebUtility.HtmlDecode(Convert.ToString(_stringwriter));
                    }
                    else
                    {
                        ServiceResponseWrapper _wrapper = new ServiceResponseWrapper();
                        _wrapper.ServiceResponse = _response;

                        return JsonConvert.SerializeObject(_wrapper);
                    }

                    #endregion

                }
                else
                {
                    _response.Result = null;
                    _response.Status = base.SetValidateTokenStatus();

                    if (base.IsXmlRequest(requestData.Format))
                    {
                        StringWriter _stringwriter = new StringWriter();
                        XmlSerializer _serializer = new XmlSerializer(typeof(ServiceResponse));
                        _serializer.Serialize(_stringwriter, _response);
                        return WebUtility.HtmlDecode(Convert.ToString(_stringwriter));
                    }
                    else
                    {
                        ServiceResponseWrapper _wrapper = new ServiceResponseWrapper();
                        _wrapper.ServiceResponse = _response;
                        return JsonConvert.SerializeObject(_wrapper);
                    }
                }
            }
            catch (Exception ex)
            {
                _response.Result = null;
                _response.Status = base.SetExceptionType(ex);

                if (base.IsXmlRequest(requestData.Format))
                {
                    StringWriter _stringwriter = new StringWriter();
                    XmlSerializer _serializer = new XmlSerializer(typeof(ServiceResponse));
                    _serializer.Serialize(_stringwriter, _response);
                    return WebUtility.HtmlDecode(Convert.ToString(_stringwriter));
                }
                else
                {
                    ServiceResponseWrapper _wrapper = new ServiceResponseWrapper();
                    _wrapper.ServiceResponse = _response;
                    return JsonConvert.SerializeObject(_wrapper);
                }
            }
        }

        #region [Private Methods]

        private String UpdateResultsByAddingPdfBytes(int tenantId, String _data)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_data);

            XmlNode ordersListNode = doc.SelectSingleNode("/Orders");

            if (ordersListNode.IsNotNull())
            {
                XmlNodeList orderList = ordersListNode.SelectNodes("Order");
                if (orderList.IsNotNull())
                {
                    foreach (XmlNode node in orderList)
                    {
                        bool isResultPdfRequired = false;
                        Int32 orderId = 0;

                        if (node.ChildNodes.IsNotNull())
                        {
                            if (node.ChildNodes[0].IsNotNull() && !string.IsNullOrEmpty(node.ChildNodes[0].InnerText))
                                Int32.TryParse(node.ChildNodes[0].InnerText, out orderId);

                            if (node.ChildNodes[2].IsNotNull() && !string.IsNullOrEmpty(node.ChildNodes[2].InnerText))
                                Boolean.TryParse(node.ChildNodes[2].InnerText, out isResultPdfRequired);
                        }

                        if (isResultPdfRequired && orderId > 0 && node.ChildNodes[3].IsNotNull())
                        {
                            node.ChildNodes[3].InnerText = GetReportBase64String(tenantId.ToString(), orderId.ToString());
                        }
                    }
                }
            }
            return doc.OuterXml;
        }

        private String GetReportBase64String(string tenantId, string orderId)
        {
            ParameterValue[] parameters = new ParameterValue[3];

            parameters[0] = new ParameterValue();
            parameters[0].Name = "OrderID";
            parameters[0].Value = orderId;
            parameters[1] = new ParameterValue();
            parameters[1].Name = "TenantID";
            parameters[1].Value = tenantId;
            parameters[2] = new ParameterValue();
            parameters[2].Name = "UserID";
            parameters[2].Value = base.CurrentUserId.ToString();

            return Convert.ToBase64String(ComplioApiReportManager.GetReportByteArray(ComplioAPIConstants.ReportName, parameters));
        }

        #endregion

        /// <summary>
        /// Method to reverify the request 
        /// </summary>
        /// <param name="requestData">Request Data</param>
        /// <returns></returns>
        private Boolean ValidateRequestToken(RequestData requestData)
        {
            String xmlData = String.Empty;
            String mappingCode = String.Empty;
            String clientToken = String.Empty;
            Boolean isVerifiedRequest = false;
            Boolean isClientTokenValidateRequired = false;
            isClientTokenValidateRequired = ComplioAPIManager.IsClientTokenValidateRequired(Convert.ToInt32(requestData.SchoolId), requestData.EntityTypeCode);

            if (isClientTokenValidateRequired)
            {

                if (!String.IsNullOrEmpty(requestData.InputData))
                {
                    xmlData = base.TryParseXml(requestData.InputData);
                }

                //Read input XML data for mapping code and client token.
                ReadXMLData(xmlData, out mappingCode, out clientToken);

                if (!mappingCode.IsNullOrEmpty() && !clientToken.IsNullOrEmpty())
                {
                    ValidateToken valTokenClassObj = ValidateTokenFactory.GetInstance().GetValidationTokenClass(mappingCode);

                    if (!valTokenClassObj.IsNullOrEmpty())
                    {
                       isVerifiedRequest = valTokenClassObj.ValidateClientToken(Convert.ToInt32(requestData.SchoolId), requestData.EntityTypeCode, mappingCode, clientToken);
                    }
                }
            }
            else
            {
                isVerifiedRequest = true;

            }
            return isVerifiedRequest;
        }

        /// <summary>
        /// Method to get input XML DATA  
        /// </summary>
        /// <param name="xmlData">Input XML</param>
        /// <param name="mappingCode">output parameter mappingCode</param>
        /// <param name="clientToken">output parameter clientToken</param>
        private void ReadXMLData(String xmlData, out String mappingCode, out String clientToken)
        {
            mappingCode = String.Empty;
            clientToken = String.Empty;
            if (!xmlData.IsNullOrEmpty())
            {
                XmlDocument _xmlDoc = new XmlDocument();
                _xmlDoc.LoadXml(xmlData);

                XmlNode node = null;
                node = _xmlDoc.SelectSingleNode("xmldata");
                if (!node.IsNullOrEmpty())
                {
                    mappingCode = node["MappingCode"].IsNullOrEmpty() ? String.Empty : Convert.ToString(node["MappingCode"].InnerText);
                    clientToken = node["TxnToken"].IsNullOrEmpty() ? String.Empty : Convert.ToString(node["TxnToken"].InnerText);
                }
            }
        }

    }
}
