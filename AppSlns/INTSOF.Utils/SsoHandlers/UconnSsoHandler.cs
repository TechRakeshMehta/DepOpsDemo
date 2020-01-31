using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using System.Xml;


namespace INTSOF.Utils.SsoHandlers
{
    public class UconnSsoHandler : SsoHandler
    {
        #region Page Constants
        //private const String SESSION_HOST = "Session_Host";
        private const String NET_ID = "HTTP_UID";
        private const String EDU_PERSON_PRINCIPAL_NAME = "HTTP_EPPN";
        private const String EMAIL = "HTTP_MAIL";
        private const String ROLE = "HTTP_UNSCOPEDAFFILIATION";
        private const String PEOPLE_SOFT_ID = "HTTP_EMPLOYEENUMBER";

        #endregion

        private String host = String.Empty;
        private String netId = String.Empty;
        private String role = String.Empty;
        private String peopleSoftId = String.Empty;
        private String eppn = String.Empty;
        private String email = String.Empty;
        private String emailId = String.Empty;

        public override String ProcessSessiondata(Boolean isSaveHistoryRequired)
        {
            if (HttpContext.Current.Session[AppConsts.SESSION_HOST] != null)
            {
                host = Convert.ToString(HttpContext.Current.Session[AppConsts.SESSION_HOST]);
            }

            if (!HttpContext.Current.Request.ServerVariables[NET_ID].IsNullOrEmpty())
            {
                netId = HttpContext.Current.Request.ServerVariables[NET_ID];
            }
            if (!HttpContext.Current.Request.ServerVariables[EDU_PERSON_PRINCIPAL_NAME].IsNullOrEmpty())
            {
                eppn = HttpContext.Current.Request.ServerVariables[EDU_PERSON_PRINCIPAL_NAME];
            }
            if (!HttpContext.Current.Request.ServerVariables[EMAIL].IsNullOrEmpty())
            {
                email = HttpContext.Current.Request.ServerVariables[EMAIL];
            }
            if (!HttpContext.Current.Request.ServerVariables[ROLE].IsNullOrEmpty())
            {
                role = HttpContext.Current.Request.ServerVariables[ROLE];
            }
            if (!HttpContext.Current.Request.ServerVariables[PEOPLE_SOFT_ID].IsNullOrEmpty())
            {
                peopleSoftId = HttpContext.Current.Request.ServerVariables[PEOPLE_SOFT_ID];
            }
            if (!email.IsNullOrEmpty())
            {
                emailId = email;
            }
            else
            {
                emailId = eppn;
            }

            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("sessionData"));
            el.AppendChild(doc.CreateElement("NetID")).InnerText = netId;
            el.AppendChild(doc.CreateElement("Host")).InnerText = host;
            el.AppendChild(doc.CreateElement("Email")).InnerText = email;
            el.AppendChild(doc.CreateElement("EduPersonPrincipalName")).InnerText = eppn;
            el.AppendChild(doc.CreateElement("Role")).InnerText = role;
            el.AppendChild(doc.CreateElement("PeopleSoftID")).InnerText = peopleSoftId;
            String sessionData = doc.OuterXml.ToString();
            return sessionData;
        }

        public override SsoHandlerContract GenerateWebApplcationData(Int32 integrationClientID)
        {
            SsoHandlerContract ssoHandlerContractObj = null;
            if (!peopleSoftId.IsNullOrEmpty() && !host.IsNullOrEmpty() && !netId.IsNullOrEmpty() && !emailId.IsNullOrEmpty() && !role.IsNullOrEmpty())
            {
                ssoHandlerContractObj = new SsoHandlerContract();
                ssoHandlerContractObj.UniqueID = peopleSoftId;
                ssoHandlerContractObj.Email = emailId;
                ssoHandlerContractObj.Role = role;
                ssoHandlerContractObj.AtributeID = netId;
                ssoHandlerContractObj.Host = host;
                ssoHandlerContractObj.IntegrationClientID = integrationClientID;
                ssoHandlerContractObj.HandlerType = AppConsts.SHIBBOLETH_UCONN;
            } return ssoHandlerContractObj;
        }
    }
}
