using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace INTSOF.Utils.SsoHandlers
{
    public class NscSsoHandler : SsoHandler
    {

        #region Page Constants

        //private const String EDU_PERSON_PRINCIPAL_NAME = "HTTP_EPPN";
        // private const String UID = "HTTP_UID";
        private const String EMAIL = "HTTP_MAIL";
        private const String ROLE = "HTTP_ROLES";
        private const String FIRST_Name = "HTTP_GIVENNAME";
        private const String LAST_NAME = "HTTP_SN";
        private const String USER_NAME = "HTTP_USERNAME";
        private const String DISPLAY_NAME = "HTTP_DISPLAYNAME";
        #endregion

        private String host = String.Empty;
        private String role = String.Empty;
        // private String eppn = String.Empty;
        private String email = String.Empty;
        //private String emailId = String.Empty;
        private String firstName = String.Empty;
        private String lastName = String.Empty;
        private String userName = String.Empty;
        private String displayName = String.Empty;

        public override String ProcessSessiondata(Boolean isSaveHistoryRequired)
        {
            if (HttpContext.Current.Session[AppConsts.SESSION_HOST] != null)
            {
                host = Convert.ToString(HttpContext.Current.Session[AppConsts.SESSION_HOST]);
            }
            if (!HttpContext.Current.Request.ServerVariables[USER_NAME].IsNullOrEmpty())
            {
                userName = HttpContext.Current.Request.ServerVariables[USER_NAME];
            }
            if (!HttpContext.Current.Request.ServerVariables[EMAIL].IsNullOrEmpty())
            {
                email = HttpContext.Current.Request.ServerVariables[EMAIL];
            }
            if (!HttpContext.Current.Request.ServerVariables[ROLE].IsNullOrEmpty())
            {
                role = HttpContext.Current.Request.ServerVariables[ROLE];
            }
            if (!HttpContext.Current.Request.ServerVariables[FIRST_Name].IsNullOrEmpty())
            {
                firstName = HttpContext.Current.Request.ServerVariables[FIRST_Name];
            }
            if (!HttpContext.Current.Request.ServerVariables[LAST_NAME].IsNullOrEmpty())
            {
                lastName = HttpContext.Current.Request.ServerVariables[LAST_NAME];
            }
            if (!HttpContext.Current.Request.ServerVariables[DISPLAY_NAME].IsNullOrEmpty())
            {
                displayName = HttpContext.Current.Request.ServerVariables[DISPLAY_NAME];
            }
            //if (!email.IsNullOrEmpty())
            //{
            //    emailId = email;
            //}
            //else
            //{
            //    emailId = eppn;
            //}

            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("sessionData"));
            el.AppendChild(doc.CreateElement("Host")).InnerText = host;
            el.AppendChild(doc.CreateElement("Email")).InnerText = email;
            el.AppendChild(doc.CreateElement("UID")).InnerText = userName;
            el.AppendChild(doc.CreateElement("UserName")).InnerText = userName;
            el.AppendChild(doc.CreateElement("Role")).InnerText = role;
            el.AppendChild(doc.CreateElement("FirstName")).InnerText = firstName;
            el.AppendChild(doc.CreateElement("LastName")).InnerText = lastName;
            el.AppendChild(doc.CreateElement("DisplayName")).InnerText = displayName;
            String sessionData = doc.OuterXml.ToString();
            return sessionData;
        }

        public override SsoHandlerContract GenerateWebApplcationData(Int32 integrationClientID)
        {
            SsoHandlerContract ssoHandlerContractObj = null;
            if (!userName.IsNullOrEmpty() && !host.IsNullOrEmpty() && !role.IsNullOrEmpty() && !firstName.IsNullOrEmpty() && !lastName.IsNullOrEmpty())
            {
                ssoHandlerContractObj = new SsoHandlerContract();
                ssoHandlerContractObj.UniqueID = userName;
                ssoHandlerContractObj.UserName = userName;
                ssoHandlerContractObj.Email = email;
                ssoHandlerContractObj.Role = role;
                ssoHandlerContractObj.Host = host;
                ssoHandlerContractObj.IntegrationClientID = integrationClientID;
                ssoHandlerContractObj.FirstName = firstName;
                ssoHandlerContractObj.LastName = lastName;
                ssoHandlerContractObj.DisplayName = displayName;
                ssoHandlerContractObj.HandlerType = AppConsts.SHIBBOLETH_NSC;
            }
            return ssoHandlerContractObj;
        }
    }
}
