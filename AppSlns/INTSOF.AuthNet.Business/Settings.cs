using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity;
using System.Data;
using DAL.Repository;
using Business.RepoManagers;
//using Intersof.YNIH.BAL;
using CoreWeb.IntsofSecurityModel.Services;
using CoreWeb.IntsofSecurityModel;

namespace INTSOF.AuthNet.Business
{
    public static class Settings
    {

        private static readonly List<Entity.PaymentIntegrationSetting> settingList;
        private static readonly List<Entity.PaymentIntegrationSettingClientMapping> paymentIntgnSettngClntMappingList;

        public static List<Entity.PaymentIntegrationSetting> PaymentIntegrationSettings
        {
            get
            {
                Int32 _parentPaymentIntSettingID = 0;
                if (IsTenantIdOverridden())
                {
                    _parentPaymentIntSettingID = paymentIntgnSettngClntMappingList.Where(cond => cond.TenantID == AuthNetUtils.OverrideTenantID).FirstOrDefault().PaymentIntegrationSettingID;
                }

                else if ( AuthNetUtils.LoggedInUserTenantID != null && paymentIntgnSettngClntMappingList.Any(cond => cond.TenantID == AuthNetUtils.LoggedInUserTenantID))
                {
                    _parentPaymentIntSettingID = paymentIntgnSettngClntMappingList.Where(cond => cond.TenantID == AuthNetUtils.LoggedInUserTenantID).FirstOrDefault().PaymentIntegrationSettingID;
                }
                else
                {
                    _parentPaymentIntSettingID = paymentIntgnSettngClntMappingList.Where(cond => cond.TenantID == AuthNetUtils.DefaultTenantID).FirstOrDefault().PaymentIntegrationSettingID;
                }
                return settingList.Where(cond => cond.ParentPaymentIntegrationSettingID == _parentPaymentIntSettingID).ToList();
            }
        }

       /* private static bool IsTenantIDOverriddenInContext()
        {
            if (HttpContext.Current != null)
            {
                var context = HttpContext.Current;
                return context.Items["overrideTenant"] != null && context.Items["overrideTenant"].ToString() == "true";
            }
            else {
                return false;
            }
            
        }*/

        private static bool IsTenantIdOverridden()
        {/*
            if (IsTenantIDOverriddenInContext())
            {*/
                return AuthNetUtils.OverrideTenantID != null && AuthNetUtils.OverrideTenantID != 0 && paymentIntgnSettngClntMappingList.Any(cond => cond.TenantID == AuthNetUtils.OverrideTenantID);
           /* }
            else
            {
                return false;
            }*/
        }

        public static string ApiLogin
        {
            get
            {
                //if (ApiLoginDict.Any(x => x.Key == AuthNetUtils.UserTenantID))
                //{
                //    return ApiLoginDict.Where(x => x.Key == AuthNetUtils.UserTenantID).Select(x => x.Value).FirstOrDefault();
                //}
                //else
                //{
                //    return ApiLoginDict.Where(x => x.Key == AuthNetUtils.DefaultTenantID).Select(x => x.Value).FirstOrDefault();
                //}
                ////return _ApiLogin;

                return PaymentIntegrationSettings.SingleOrDefault(q => q.NameKey == "Authorize.Net-ApiLogin").NameValue;
            }
        }

        public static string TranKey
        {
            get
            {
                //if (TranKeyDict.Any(x => x.Key == AuthNetUtils.LoggedInUserTenantID))
                //{
                //    return TranKeyDict.Where(x => x.Key == AuthNetUtils.LoggedInUserTenantID).Select(x => x.Value).FirstOrDefault();
                //}
                //else
                //{
                //    return TranKeyDict.Where(x => x.Key == AuthNetUtils.DefaultTenantID).Select(x => x.Value).FirstOrDefault();
                //}
                return PaymentIntegrationSettings.SingleOrDefault(q => q.NameKey == "Authorize.Net-TranKey").NameValue;
            }
        }

        public static string IsTestRequest
        {
            get
            {
                return PaymentIntegrationSettings.SingleOrDefault(q => q.NameKey == "Authorize.Net-IsTestRequest").NameValue;
            }
        }

        public static string PostUrl_Test
        {
            get
            {
                //return _PostUrl_Test;
                return PaymentIntegrationSettings.SingleOrDefault(q => q.NameKey == "Authorize.Net-PostUrl-Test").NameValue;
            }
        }

        public static string PostUrl_Secure
        {
            get
            {
                return PaymentIntegrationSettings.SingleOrDefault(q => q.NameKey == "Authorize.Net-PostUrl-Secure").NameValue;
                //return _PostUrl_Secure;
            }
        }

        public static string PostUrl_GOLIVE
        {
            get
            {
                //return _PostUrl_GOLIVE;
                return PaymentIntegrationSettings.SingleOrDefault(q => q.NameKey == "Authorize.Net-PostUrl-Secure USE THIS ON GOLIVE").NameValue;
            }
        }
        public static string MD5Hash
        {
            get
            {
                //return _MD5Hash;
                return PaymentIntegrationSettings.SingleOrDefault(q => q.NameKey == "AuthNet-MD5Hash").NameValue;
            }
        }
        public static string TestEmailAddress
        {
            get
            {
                //return _TestEmailAddress;
                return PaymentIntegrationSettings.SingleOrDefault(q => q.NameKey == "TestEmailAddress").NameValue;
            }
        }
        public static string RelayURL
        {
            get
            {
                //return _RelayURL;
                return PaymentIntegrationSettings.SingleOrDefault(q => q.NameKey == "Authorize.RelayURL").NameValue;
            }
        }
        public static string Heading
        {
            get
            {
                //return _Heading;
                return PaymentIntegrationSettings.SingleOrDefault(q => q.NameKey == "Heading").NameValue;
            }
        }

        public static string CIMServiceUrl_Test
        {
            get
            {
                //return _CIMServiceUrl_Test;
                return PaymentIntegrationSettings.SingleOrDefault(q => q.NameKey == "CIM -ServiceUrl-Test").NameValue;
            }
        }

        public static string CIMServiceUrl_Secure
        {
            get
            {
                //return _CIMServiceUrl_Secure;
                return PaymentIntegrationSettings.SingleOrDefault(q => q.NameKey == "CIM -ServiceUrl-Secure").NameValue;
            }
        }

        //public static Int32 LoggedInTenantID
        //{
        //    get
        //    {
        //        return AuthNetUtils.LoggedInUserTenantID;
        //    }
        //}

        //private static readonly List<Entity.PaymentIntegrationSetting> settingList;

        //private static Dictionary<int, string> ApiLoginDict;
        //private static Dictionary<int, string> TranKeyDict;

        //public static string _IsTestRequest;

        //public static string _PostUrl_Test;

        //public static string _PostUrl_Secure;

        //public static string _PostUrl_GOLIVE;

        //public static string _MD5Hash;

        //public static string _TestEmailAddress;

        //public static string _RelayURL;

        //public static string _Heading;

        //public static string _CIMServiceUrl_Test;

        //public static string _CIMServiceUrl_Secure;

        static Settings()
        {
            settingList = SecurityManager.GetPaymentIntegrationSettingsByName("Authorize");
            paymentIntgnSettngClntMappingList = SecurityManager.GetPaymentIntegrationSettingsClientMappings();

            //ApiLoginDict = settingList.Single(q => q.NameKey == "Authorize.Net-ApiLogin").NameValue;
            //ApiLoginDict = settingList.Where(q => q.NameKey == "Authorize.Net-ApiLogin").ToDictionary(x => x.PaymentIntegrationSettingID, x => x.NameValue);
            //_TranKey = settingList.Single(q => q.NameKey == "Authorize.Net-TranKey").NameValue;
            //TranKeyDict = settingList.Where(q => q.NameKey == "Authorize.Net-TranKey").ToDictionary(x => x.PaymentIntegrationSettingID, x => x.NameValue);
            //IsTestRequest = settingList.Single(q => q.NameKey == "Authorize.Net-IsTestRequest").NameValue;
            //PostUrl_Test = settingList.Single(q => q.NameKey == "Authorize.Net-PostUrl-Test").NameValue;
            //PostUrl_Secure = settingList.Single(q => q.NameKey == "Authorize.Net-PostUrl-Secure").NameValue;
            //PostUrl_GOLIVE = settingList.Single(q => q.NameKey == "Authorize.Net-PostUrl-Secure USE THIS ON GOLIVE").NameValue;
            //MD5Hash = settingList.Single(q => q.NameKey == "AuthNet-MD5Hash").NameValue;
            //TestEmailAddress = settingList.Single(q => q.NameKey == "TestEmailAddress").NameValue;
            //RelayURL = settingList.Single(q => q.NameKey == "Authorize.RelayURL").NameValue;
            //Heading = settingList.Single(q => q.NameKey == "Heading").NameValue;
            //CIMServiceUrl_Test = settingList.Single(q => q.NameKey == "CIM -ServiceUrl-Test").NameValue;
            //CIMServiceUrl_Secure = settingList.Single(q => q.NameKey == "CIM -ServiceUrl-Secure").NameValue;
        }
    }
}
