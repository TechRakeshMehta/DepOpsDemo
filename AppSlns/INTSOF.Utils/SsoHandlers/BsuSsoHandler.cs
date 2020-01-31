using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace INTSOF.Utils.SsoHandlers
{
    public class BsuSsoHandler : SsoHandler
    {

        #region Page Constants

        //private const String EDU_PERSON_PRINCIPAL_NAME = "HTTP_EPPN";
        // private const String UID = "HTTP_UID";
        private const String EMAIL = "HTTP_EMAILADDRESS";
        private const String FIRST_Name = "HTTP_GIVENNAME";
        private const String LAST_NAME = "HTTP_SN";
        private const String UDC_ID = "HTTP_NAMEID";
        private const String ROLE = "HTTP_ROLES";
        #endregion

        private String firstName = String.Empty;
        private String lastName = String.Empty;
        private String email = String.Empty;
        private String udcId = String.Empty;
        private String role = String.Empty;
        private String host = String.Empty;
        public override String ProcessSessiondata(Boolean isSaveHistoryRequired)
        {
            if (!HttpContext.Current.Session[AppConsts.SESSION_HOST].IsNullOrEmpty())
            {
                host = Convert.ToString(HttpContext.Current.Session[AppConsts.SESSION_HOST]);
            }
            if (!HttpContext.Current.Request.ServerVariables[EMAIL].IsNullOrEmpty())
            {
                email = HttpContext.Current.Request.ServerVariables[EMAIL];
            }
            if (!HttpContext.Current.Request.ServerVariables[FIRST_Name].IsNullOrEmpty())
            {
                firstName = HttpContext.Current.Request.ServerVariables[FIRST_Name];
            }
            if (!HttpContext.Current.Request.ServerVariables[LAST_NAME].IsNullOrEmpty())
            {
                lastName = HttpContext.Current.Request.ServerVariables[LAST_NAME];
            }
            if (!HttpContext.Current.Request.ServerVariables[UDC_ID].IsNullOrEmpty())
            {
                udcId = HttpContext.Current.Request.ServerVariables[UDC_ID];
            }
            if (!HttpContext.Current.Request.ServerVariables[ROLE].IsNullOrEmpty())
            {
                role = HttpContext.Current.Request.ServerVariables[ROLE];
            }
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("sessionData"));
            el.AppendChild(doc.CreateElement("Email")).InnerText = email;
            el.AppendChild(doc.CreateElement("UDCID")).InnerText = udcId.IsNullOrEmpty() ? email : udcId;
            el.AppendChild(doc.CreateElement("FirstName")).InnerText = firstName;
            el.AppendChild(doc.CreateElement("LastName")).InnerText = lastName;
            el.AppendChild(doc.CreateElement("Role")).InnerText = role.IsNullOrEmpty() ? "complioStudents" : role;//!firstName.IsNullOrEmpty() && !lastName.IsNullOrEmpty() && !email.IsNullOrEmpty() ? "complioAdmin" : "complioStudents";
            String sessionData = doc.OuterXml.ToString();
            return sessionData;
        }

        public override SsoHandlerContract GenerateWebApplcationData(Int32 integrationClientID)
        {
            SsoHandlerContract ssoHandlerContractObj = null;
            if (!email.IsNullOrEmpty() && !host.IsNullOrEmpty() && !firstName.IsNullOrEmpty() && !lastName.IsNullOrEmpty())
            {
                ssoHandlerContractObj = new SsoHandlerContract();
                ssoHandlerContractObj.UniqueID = udcId.IsNullOrEmpty() ? email : udcId;
                ssoHandlerContractObj.Email = email;
                ssoHandlerContractObj.FirstName = firstName;
                ssoHandlerContractObj.LastName = lastName;
                ssoHandlerContractObj.UdcID = udcId.IsNullOrEmpty() ? email : udcId;
                ssoHandlerContractObj.IntegrationClientID = integrationClientID;
                ssoHandlerContractObj.Host = host;
                ssoHandlerContractObj.Role = role;//role.IsNullOrEmpty() ? "complioStudents" : role; //!firstName.IsNullOrEmpty() && !lastName.IsNullOrEmpty() && !email.IsNullOrEmpty() ? "complioAdmin" : "complioStudents";
                ssoHandlerContractObj.HandlerType = AppConsts.SHIBBOLETH_BALL_STATE;
            }
            return ssoHandlerContractObj;
        }
    }
}
