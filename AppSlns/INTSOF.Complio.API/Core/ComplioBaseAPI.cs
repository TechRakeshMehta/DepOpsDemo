using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using CoreWeb.IntsofSecurityModel;
using INTSOF.Complio.API.ComplioAPIBusiness;

namespace INTSOF.Complio.API.Core
{
    public class ComplioBaseAPI
    {
        #region [Private Variables]
        private Int32 _currentUserId;
        #endregion

        #region Properties
        //To get current user Id, must call UpdateXmlInput method first.
        public Int32 CurrentUserId
        {
            get
            {
                return _currentUserId;
            }
            set
            {
                _currentUserId = value;
            }
        }
        #endregion

        /// <summary>
        /// Returns whether the current request is Xml or Json type
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        protected Boolean IsXmlRequest(String format)
        {
            return
                 (String.IsNullOrEmpty(format) || (!String.IsNullOrEmpty(format) && format.ToLower().Trim() == "xml"))
                ? true
                : false;
        }

        /// <summary>
        /// Check if InputDAtaparameter is XML or JSON.
        /// If XML, then add the xmldata as root tag else convert JSON to XML
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        protected String TryParseXml(string inputData)
        {
            if (inputData.TrimStart().StartsWith("<"))
            {
                var xmlString = inputData;
                //var xDoc = new XDocument(new XElement(ComplioAPIConstants.XmlDataTag, XElement.Parse(xmlString)));
                var xDoc = new XDocument(XElement.Parse(String.Concat(ComplioAPIConstants.XmlDataOpenTag, xmlString, ComplioAPIConstants.XmlDataCloseTag)));
                return Convert.ToString(xDoc);
            }
            else
            {
                XmlDocument doc = (XmlDocument)Newtonsoft.Json.JsonConvert.DeserializeXmlNode(inputData, ComplioAPIConstants.XmlDataTag);
                return (doc.InnerXml);
            }
        }

        protected string AppendUserId(String inputXML, Int32 studentId)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(inputXML);

            string str = "<OrganizationUserId>" + studentId + "</OrganizationUserId>";
            XmlDocumentFragment xmlDocFrag = xmlDoc.CreateDocumentFragment();
            xmlDocFrag.InnerXml = str;
            XmlElement rootElement = xmlDoc.DocumentElement;
            rootElement.AppendChild(xmlDocFrag);
            return xmlDoc.InnerXml;
        }

        /// <summary>
        /// Set the Response code, based on Exception type
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected ServiceStatus SetExceptionType(Exception ex)
        {
            if (ex.GetType() == typeof(System.Data.SqlClient.SqlException))
            {
                System.Data.SqlClient.SqlException sqlEx = ((System.Data.SqlClient.SqlException)ex);

                //Custom Error number in the valid range of 50000 to 2147483647.
                if (sqlEx.Number >= Convert.ToInt32(ExceptionType.SqlCustomExceptionType) && sqlEx.Number <= 2147483647)
                {
                    return new ServiceStatus
                    {
                        Code = Convert.ToInt32(ExceptionType.SqlCustomExceptionType),
                        Message = ex.Message,
                        Type = ComplioAPIConstants.StatusErrorType
                    };
                }
                else
                {
                    return new ServiceStatus
                    {
                        Code = Convert.ToInt32(ExceptionType.SqlExceptionType),
                        Message = ComplioAPIConstants.FailureStatusMessge,
                        Type = ComplioAPIConstants.StatusErrorType
                    };
                }
            }
            else if (ex.GetType() == typeof(System.TimeoutException))
            {
                return new ServiceStatus
                {
                    Code = Convert.ToInt32(ExceptionType.TimeOutException),
                    Message = ComplioAPIConstants.FailureStatusMessge,
                    Type = ComplioAPIConstants.StatusErrorType
                };
            }
            else if (ex.GetType() == typeof(System.Xml.XmlException))
            {
                return new ServiceStatus
                {
                    Code = Convert.ToInt32(ExceptionType.XmlException),
                    Message = ComplioAPIConstants.FailureStatusMessge,
                    Type = ComplioAPIConstants.StatusErrorType
                };
            }
            else if (ex.GetType() == typeof(System.Web.Services.Protocols.SoapException))
            {
                return new ServiceStatus
                {
                    Code = Convert.ToInt32(ExceptionType.ReportServerSoapException),
                    Message = ComplioAPIConstants.FailureStatusMessge,
                    Type = ComplioAPIConstants.StatusErrorType
                };
            }
            else
            {
                return new ServiceStatus
                {
                    Code = Convert.ToInt32(HttpStatusCode.InternalServerError),
                    Message = ComplioAPIConstants.FailureStatusMessge,
                    Type = ComplioAPIConstants.StatusErrorType
                };
            }
        }

        /// <summary>
        /// Get the Applicant details and Set the OrganizationUserID in the XML
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        protected string UpdateXmlInput(string xmlData)
        {
            string _userName = string.Empty;
            string _password = string.Empty;

            //Getting user credientials from request hedaer
            ComplioAPIManager.GetCredientials(ref _userName, ref  _password);

            SysXMembershipUser user = System.Web.Security.Membership.GetUser(Regex.Replace(_userName, ComplioAPIConstants.CredentialValidationExpression,
                                                                                ComplioAPIConstants.ASCIISPACE)) as SysXMembershipUser;

            xmlData = AppendUserId(xmlData, user.OrganizationUserId);
            _currentUserId = user.OrganizationUserId;
            return xmlData;
        }

        /// <summary>
        /// Set the Invalid Request message after Token Validation
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected ServiceStatus SetValidateTokenStatus()
        {
            return new ServiceStatus
            {
                Code = 0,
                Message = ComplioAPIConstants.InvalidTokenMessge,
                Type = ComplioAPIConstants.StatusErrorType
            };
        }
    }
}