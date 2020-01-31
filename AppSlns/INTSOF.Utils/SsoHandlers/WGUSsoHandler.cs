using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace INTSOF.Utils.SsoHandlers
{
    public class WGUSsoHandler : SsoHandler
    {
        #region Page Constants
        private const String WGU_UUID = "HTTP_WGUUUID";
        private const String FIRST_NAME = "HTTP_FIRSTNAME";
        private const String LAST_NAME = "HTTP_LASTNAME";
        private const String WGU_BANNER_ID = "HTTP_WGUBANNERID";
        private const String WGU_LEVEL_ONE_ROLE = "HTTP_WGULEVELONEROLE";
        private const String EMAIL = "HTTP_EMAIL";
        private const String USER_NAME = "HTTP_USERNAME";
        #endregion

        private String firstName = String.Empty;
        private String lastName = String.Empty;
        private String wguuuId = String.Empty;
        private String wguBannerId = String.Empty;
        private String wguRole = String.Empty;
        private String email = String.Empty;
        private String userName = String.Empty;
        private String host = String.Empty;


        public override String ProcessSessiondata(Boolean isSaveHistoryRequired)
        {
            if (HttpContext.Current.Session[AppConsts.SESSION_HOST] != null)
            {
                host = Convert.ToString(HttpContext.Current.Session[AppConsts.SESSION_HOST]);
            }
            if (!HttpContext.Current.Request.ServerVariables[FIRST_NAME].IsNullOrEmpty())
            {
                firstName = HttpContext.Current.Request.ServerVariables[FIRST_NAME];
            }
            if (!HttpContext.Current.Request.ServerVariables[LAST_NAME].IsNullOrEmpty())
            {
                lastName = HttpContext.Current.Request.ServerVariables[LAST_NAME];
            }
            if (!HttpContext.Current.Request.ServerVariables[WGU_UUID].IsNullOrEmpty())
            {
                wguuuId = HttpContext.Current.Request.ServerVariables[WGU_UUID];
            }
            if (!HttpContext.Current.Request.ServerVariables[WGU_BANNER_ID].IsNullOrEmpty())
            {
                wguBannerId = HttpContext.Current.Request.ServerVariables[WGU_BANNER_ID];
            }
            if (!HttpContext.Current.Request.ServerVariables[WGU_LEVEL_ONE_ROLE].IsNullOrEmpty())
            {
                wguRole = HttpContext.Current.Request.ServerVariables[WGU_LEVEL_ONE_ROLE];
            }
            if (!HttpContext.Current.Request.ServerVariables[EMAIL].IsNullOrEmpty())
            {
                email = HttpContext.Current.Request.ServerVariables[EMAIL];
            }
            if (!HttpContext.Current.Request.ServerVariables[USER_NAME].IsNullOrEmpty())
            {
                userName = HttpContext.Current.Request.ServerVariables[USER_NAME];
            }
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("sessionData"));
            el.AppendChild(doc.CreateElement("Host")).InnerText = host;
            el.AppendChild(doc.CreateElement("WguUUId")).InnerText = wguuuId;
            el.AppendChild(doc.CreateElement("FirstName")).InnerText = firstName;
            el.AppendChild(doc.CreateElement("LastName")).InnerText = lastName;
            el.AppendChild(doc.CreateElement("Email")).InnerText = email;
            el.AppendChild(doc.CreateElement("WguBannerId")).InnerText = wguBannerId;
            el.AppendChild(doc.CreateElement("Role")).InnerText = wguRole;
            el.AppendChild(doc.CreateElement("UserName")).InnerText = userName;
            String sessionData = doc.OuterXml.ToString();
            return sessionData;
        }

        public override SsoHandlerContract GenerateWebApplcationData(Int32 integrationClientID)
        {
            SsoHandlerContract ssoHandlerContractObj = null;
            if (!wguuuId.IsNullOrEmpty() && !firstName.IsNullOrEmpty() && !lastName.IsNullOrEmpty() && !email.IsNullOrEmpty() && !wguRole.IsNullOrEmpty() && !host.IsNullOrEmpty())
            {
                ssoHandlerContractObj = new SsoHandlerContract();
                ssoHandlerContractObj.UniqueID = wguuuId;
                ssoHandlerContractObj.Email = email;
                ssoHandlerContractObj.Role = wguRole;
                ssoHandlerContractObj.AtributeID = wguBannerId;
                ssoHandlerContractObj.Host = host;
                ssoHandlerContractObj.IntegrationClientID = integrationClientID;
                ssoHandlerContractObj.FirstName = firstName;
                ssoHandlerContractObj.LastName = lastName;
                ssoHandlerContractObj.UserName = userName;
                ssoHandlerContractObj.HandlerType = AppConsts.SHIBBOLETH_WGU;
            }
            return ssoHandlerContractObj;
        }
    }
}
