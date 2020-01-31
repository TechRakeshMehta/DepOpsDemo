using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace INTSOF.Utils.SsoHandlers
{
    public class NyuSsoHandler : SsoHandler
    {

        #region Page Constants

        //private const String EDU_PERSON_PRINCIPAL_NAME = "HTTP_EPPN";
        private const String UID = "HTTP_UID";
        private const String EMAIL = "HTTP_MAIL";
        //private const String ROLE = "HTTP_ENTITLEMENT";
        private const String FIRST_Name = "HTTP_GIVENNAME";
        private const String LAST_NAME = "HTTP_SN";
        private const String MEMBER = "HTTP_MEMBER";

        #endregion

        private String host = String.Empty;
        private String role = String.Empty;
        // private String eppn = String.Empty;
        private String email = String.Empty;
        //private String emailId = String.Empty;
        private String firstName = String.Empty;
        private String lastName = String.Empty;
        private String uid = String.Empty;


        public override String ProcessSessiondata(Boolean isSaveHistoryRequired)
        {
            if (HttpContext.Current.Session[AppConsts.SESSION_HOST] != null)
            {
                host = Convert.ToString(HttpContext.Current.Session[AppConsts.SESSION_HOST]);
            }
            if (!HttpContext.Current.Request.ServerVariables[UID].IsNullOrEmpty())
            {
                uid = HttpContext.Current.Request.ServerVariables[UID];
            }
            if (!HttpContext.Current.Request.ServerVariables[EMAIL].IsNullOrEmpty())
            {
                email = HttpContext.Current.Request.ServerVariables[EMAIL];
            }
            if (!HttpContext.Current.Request.ServerVariables[MEMBER].IsNullOrEmpty())
            {
                role = HttpContext.Current.Request.ServerVariables[MEMBER];
            }
            if (!HttpContext.Current.Request.ServerVariables[FIRST_Name].IsNullOrEmpty())
            {
                firstName = HttpContext.Current.Request.ServerVariables[FIRST_Name];
            }
            if (!HttpContext.Current.Request.ServerVariables[LAST_NAME].IsNullOrEmpty())
            {
                lastName = HttpContext.Current.Request.ServerVariables[LAST_NAME];
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
            el.AppendChild(doc.CreateElement("Uid")).InnerText = uid;
            el.AppendChild(doc.CreateElement("Role")).InnerText = role;
            el.AppendChild(doc.CreateElement("FirstName")).InnerText = firstName;
            el.AppendChild(doc.CreateElement("LastName")).InnerText = lastName;
            String sessionData = doc.OuterXml.ToString();
            return sessionData;
        }

        public override SsoHandlerContract GenerateWebApplcationData(Int32 integrationClientID)
        {
            SsoHandlerContract ssoHandlerContractObj = null;
            if (!uid.IsNullOrEmpty() && !host.IsNullOrEmpty() && !role.IsNullOrEmpty() && !firstName.IsNullOrEmpty() && !lastName.IsNullOrEmpty())
            {
                ssoHandlerContractObj = new SsoHandlerContract();
                ssoHandlerContractObj.UniqueID = uid;
                ssoHandlerContractObj.Email = email;
                ssoHandlerContractObj.Role = role;
                ssoHandlerContractObj.Host = host;
                ssoHandlerContractObj.IntegrationClientID = integrationClientID;
                ssoHandlerContractObj.FirstName = firstName;
                ssoHandlerContractObj.LastName = lastName;
                ssoHandlerContractObj.HandlerType = AppConsts.SHIBBOLETH_NYU;
            } return ssoHandlerContractObj;
        }
    }
}
