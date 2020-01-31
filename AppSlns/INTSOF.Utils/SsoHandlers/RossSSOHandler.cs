using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace INTSOF.Utils.SsoHandlers
{
    public class RossSSOHandler : SsoHandler
    {
        #region Page Constants

        private const String EMAIL = "HTTP_EMAIL";
        private const String FIRST_Name = "HTTP_FIRSTNAME";
        private const String LAST_NAME = "HTTP_LASTNAME";
        private const String USER_NAME = "HTTP_USERNAME";
        private const string BANNER_ID = "HTTP_BANNERID";
        private const String ROLE = "HTTP_ROLES";

        #endregion

        private String host = String.Empty;
        private String role = String.Empty;
        private String userName = String.Empty;
        private String email = String.Empty;
        private String emailId = String.Empty;
        private String firstName = String.Empty;
        private String lastName = String.Empty;
        private String bannerId = String.Empty;

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
            if (!HttpContext.Current.Request.ServerVariables[BANNER_ID].IsNullOrEmpty())
            {
                bannerId = HttpContext.Current.Request.ServerVariables[BANNER_ID];
            }

            if (!HttpContext.Current.Request.ServerVariables[FIRST_Name].IsNullOrEmpty())
            {
                firstName = HttpContext.Current.Request.ServerVariables[FIRST_Name];
            }
            if (!HttpContext.Current.Request.ServerVariables[LAST_NAME].IsNullOrEmpty())
            {
                lastName = HttpContext.Current.Request.ServerVariables[LAST_NAME];
            }

            if (!HttpContext.Current.Request.ServerVariables[ROLE].IsNullOrEmpty())
            {
                role = HttpContext.Current.Request.ServerVariables[ROLE];
            }

            //else
            //{
            //    emailId = eppn;
            //}

            ///TODO 
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("sessionData"));
            el.AppendChild(doc.CreateElement("Host")).InnerText = host;           
            el.AppendChild(doc.CreateElement("username")).InnerText = userName;
            el.AppendChild(doc.CreateElement("firstname")).InnerText = firstName;
            el.AppendChild(doc.CreateElement("lastname")).InnerText = lastName;
            el.AppendChild(doc.CreateElement("email")).InnerText = email;
            el.AppendChild(doc.CreateElement("bannerId")).InnerText = bannerId;
            el.AppendChild(doc.CreateElement("Role")).InnerText = role.IsNullOrEmpty() ? "complioStudents" : role;

            String sessionData = doc.OuterXml.ToString();
            return sessionData;
        }

        public override SsoHandlerContract GenerateWebApplcationData(Int32 integrationClientID)
        {
            SsoHandlerContract ssoHandlerContractObj = null;

            if (!bannerId.IsNullOrEmpty() && !host.IsNullOrEmpty() && !firstName.IsNullOrEmpty() && !lastName.IsNullOrEmpty() && !email.IsNullOrEmpty())
            {
                ssoHandlerContractObj = new SsoHandlerContract();
                ssoHandlerContractObj.UserName = userName;
                ssoHandlerContractObj.FirstName = firstName;
                ssoHandlerContractObj.LastName = lastName;
                ssoHandlerContractObj.Email = email;
                ssoHandlerContractObj.UniqueID = bannerId;             
                ssoHandlerContractObj.Role = role;
                ssoHandlerContractObj.Host = host;
                ssoHandlerContractObj.Role = role;
                ssoHandlerContractObj.IntegrationClientID = integrationClientID;
                ssoHandlerContractObj.HandlerType = AppConsts.SHIBBOLETH_ROSS;
            }
           
            return ssoHandlerContractObj;
        }
    }

}
