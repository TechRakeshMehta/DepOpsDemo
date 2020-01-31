using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace INTSOF.AuthNet.Business
{
    public class SoapAPIUtilities
    {
        public static string MERCHANT_NAME
        {
            get
            {
                return Settings.ApiLogin;
            }
        }

        public static String TRANSACTION_KEY
        {
            get
            {
                return Settings.TranKey;
            }
        }

        //public static String TRANSACTION_KEY = Settings.TranKey;

        public static Boolean IsTestRequest = (!String.IsNullOrEmpty(Settings.IsTestRequest)) ? Convert.ToBoolean(Settings.IsTestRequest) : true;
        public static Boolean IsUseAuthorizeDotNetTls_1_2 = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["UseAuthorizeDotNetTls_1_2"]) ? Convert.ToBoolean(ConfigurationManager.AppSettings["UseAuthorizeDotNetTls_1_2"]) : false;

        public static String API_URL
        {
            get
            {
                return IsTestRequest ? Settings.CIMServiceUrl_Test : Settings.CIMServiceUrl_Secure;   // "https://apitest.authorize.net/soap/v1/Service.asmx";
            }
        }

        //private static CustomerProfileWS.MerchantAuthenticationType m_auth = null;
        private static CustomerProfileWS.Service service = null;



        public static String LoginID
        {
            get;
            set;
        }

        public static String TransactionKey
        {
            get;
            set;
        }

        //public static String API_URL
        //{
        //    get;
        //    set;
        //}


        //public static CustomerProfileWS.MerchantAuthenticationType MerchantAuthentication
        //{
        //    get
        //    {
        //        if (m_auth == null)
        //        {
        //            m_auth = new CustomerProfileWS.MerchantAuthenticationType();
        //            m_auth.name = MERCHANT_NAME;
        //            m_auth.transactionKey = Settings.TranKey;
        //        }
        //        if (IsUseAuthorizeDotNetTls_1_2)
        //        {
        //            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Ssl3;
        //        }

        //        return m_auth;
        //    }
        //}

        public static CustomerProfileWS.MerchantAuthenticationType MerchantAuthentication
        {
            get
            {
                AuthNetUtils.OverrideTenantID = 0;
                CustomerProfileWS.MerchantAuthenticationType m_auth = new CustomerProfileWS.MerchantAuthenticationType();
                m_auth.name = MERCHANT_NAME;
                m_auth.transactionKey = TRANSACTION_KEY;

                if (IsUseAuthorizeDotNetTls_1_2)
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Ssl3;
                }

                return m_auth;
            }
        }

        public static CustomerProfileWS.Service Service
        {
            get
            {
                if (service == null)
                {
                    service = new CustomerProfileWS.Service();
                    service.Url = API_URL;
                }
                return service;
            }
        }

        public static CustomerProfileWS.MerchantAuthenticationType MerchantAuthenticationByTenant(Int32 tenantID)
        {
            CustomerProfileWS.MerchantAuthenticationType m_auth = new CustomerProfileWS.MerchantAuthenticationType();
            
            
            AuthNetUtils.OverrideTenantID = tenantID;
            m_auth.name = MERCHANT_NAME;
            m_auth.transactionKey = TRANSACTION_KEY;

            if (IsUseAuthorizeDotNetTls_1_2)
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Ssl3;
            }
            AuthNetUtils.OverrideTenantID = 0;
            return m_auth;
        }

    }
}
