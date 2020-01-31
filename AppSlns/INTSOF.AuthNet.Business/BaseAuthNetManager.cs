using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace INTSOF.AuthNet.Business
{
    public class BaseAuthNetManager
    {
        static string m_ClassModule = string.Empty;

        public static string ClassModule
        {
            get
            {
                return m_ClassModule;
            }
            set
            {
                m_ClassModule = value;
            }
        }

        protected String ApiLogin
        { 
            get {
                //return Convert.ToString(ConfigurationManager.AppSettings["Authorize.Net-ApiLogin"]);
                return Settings.ApiLogin;
            }
        }

        protected String ApiTransKey
        {
            get {
                //return Convert.ToString(ConfigurationManager.AppSettings["Authorize.Net-TranKey"]);
                return Settings.TranKey;
            }
        }

        protected String AuthNetTestURL
        {
            get
            {
                //if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["Authorize.Net-PostUrl-Test"]))
                //    return Convert.ToString(ConfigurationManager.AppSettings["Authorize.Net-PostUrl-Test"]);
                if (!String.IsNullOrEmpty(Settings.PostUrl_Test))
                    return Settings.PostUrl_Test;
                else
                    return "https://test.authorize.net/gateway/transact.dll";
            }
        }

        protected String AuthNetLiveURL
        {
            get
            {
                //if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["Authorize.Net-PostUrl-Secure"]))
                //    return Convert.ToString(ConfigurationManager.AppSettings["Authorize.Net-PostUrl-Secure"]);
                if (!String.IsNullOrEmpty(Settings.PostUrl_Secure))
                    return Settings.PostUrl_Secure;
                else
                    return "https://secure.authorize.net/gateway/transact.dll";
            }
        }

        protected String AuthNetMD5Hash
        {
            get {
                //return Convert.ToString(ConfigurationManager.AppSettings["AuthNet-MD5Hash"]);
                return Settings.MD5Hash;
            }
        }

        protected bool IsAuthNetInTestMode
        {
            get
            {
                //if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["Authorize.Net-IsTestRequest"]))
                //    return Convert.ToBoolean(ConfigurationManager.AppSettings["Authorize.Net-IsTestRequest"]);
                if (!String.IsNullOrEmpty(Settings.IsTestRequest))
                    return Convert.ToBoolean(Settings.IsTestRequest);
                else
                    return true;
            }
        }

        protected bool IsAppInTestMode
        {
            get {
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["TestFlag"]))
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["TestFlag"]);
                else
                    return true;
            }
        }

        protected bool CanWriteToLog
        {
            get
            {
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AUCTIONS_WRITE_TO_LOG"]))
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["AUCTIONS_WRITE_TO_LOG"]);
                else
                    return false;
            }
        }

        /// <summary>
        /// Returns the method name.
        /// </summary>
        /// <returns></returns>
        public static string ShowTrace()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1);
            System.Diagnostics.StackFrame sf = st.GetFrame(0);
            String str = sf.GetMethod().DeclaringType.FullName + "." + sf.GetMethod().Name;
            str = sf.GetMethod().Name;
            return str;
        }
    }
}
