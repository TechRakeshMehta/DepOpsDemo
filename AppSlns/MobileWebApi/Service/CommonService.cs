using Business.RepoManagers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Security.Claims;
using System.Threading;
using System.Web;

namespace MobileWebApi.Service
{
    public static class CommonService
    {
        public static TenantContract GetTenantDetails()
        {                   
            var requestUrl = HttpContext.Current.Request.Url.Host;
            return GetTenantDetailsByUrl(requestUrl);
        }

        public static TenantContract GetTenantDetailsByUrl(string requestUrl)
        {
            //requestUrl = "m-qainst6.adbdemo.com";
            //requestUrl = "CBI.complio.com";
            String mobileURLPrefix = ConfigurationManager.AppSettings["MobileUrlPrefix"];
            String tenantUrl = requestUrl.Replace(mobileURLPrefix, String.Empty);
            tenantUrl = tenantUrl.Replace("http://", String.Empty);
            tenantUrl = tenantUrl.Replace("https://", String.Empty);
            tenantUrl = tenantUrl.Replace("/MobileWebApi", String.Empty);
            TenantContract tenantContract = new TenantContract();
            var result = WebSiteManager.GetWebSite(tenantUrl);
            if (result != null)
            {
                var tenant = result.TenantWebsiteMappings.FirstOrDefault(twm => twm.TWM_IsDeleted == false).Tenant;
                if (tenant != null)
                {
                    tenantContract.TenantName = tenant.TenantName;
                    tenantContract.TenantUrl = tenantUrl;
                    tenantContract.TenantId = tenant.TenantID;
                }
            }
            if (tenantContract.TenantId <= 0)
            {
                tenantContract.TenantName = "ADB";
                tenantContract.TenantUrl = tenantUrl;
                tenantContract.TenantId = 0;
            }
            return tenantContract;
        }

        #region Mobile app Internationalization

        public static LanguageContract GetLanguageContract(String languageCode = default(String), Guid userId = default(Guid))
        {
            LanguageContract languageContract = new LanguageContract();

            if ((languageCode == null || languageCode == "") && userId != null)
            {
                var currentLang = SecurityManager.GetLanguageCulture(userId);

                languageContract.LanguageID = currentLang.LanguageID;
                languageContract.LanguageName = currentLang.LanguageName;
                languageContract.LanguageCode = currentLang.LanguageCode;
                languageContract.LanguageCulture = currentLang.LanguageCulture;
            }

            if (languageCode != null && languageCode != "")
            {
                var lkpLanguages = LookupManager.GetLookUpData<Entity.lkpLanguage>();
                var currentLang = lkpLanguages.Where(col => col.LAN_Code == languageCode).FirstOrDefault();

                languageContract.LanguageID = currentLang.LAN_ID;
                languageContract.LanguageName = currentLang.LAN_Name;
                languageContract.LanguageCode = currentLang.LAN_Code;
                languageContract.LanguageCulture = currentLang.LAN_Culture;
            }

            return languageContract;
        }

        public static string GetResourceKeyValue(string languageCode,string key)
        {
            var lkpLanguages = LookupManager.GetLookUpData<Entity.lkpLanguage>();
            var currentLang = lkpLanguages.FirstOrDefault(col => col.LAN_Code == languageCode);

            var cultureInfo = new CultureInfo(currentLang.LAN_Culture);
            var langRes = new ResourceManager("Resources.Language", global::System.Reflection.Assembly.Load("App_GlobalResources"));
            var resSet = langRes.GetResourceSet(cultureInfo, true, true);
            if (resSet != null)
            {
                var resSetDic = resSet.Cast<DictionaryEntry>().ToDictionary(r => r.Key.ToString(), r => r.Value.ToString());
                return resSetDic.FirstOrDefault(cond => cond.Key == key).Value;
            }
            return null;
        }

        public static string GetUserAgreementText(string LangCode)
        {
            var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
            var result = "";
            if (claims != null && claims.Any(d => d.Type == "OrganizationUserID") && claims.Any(d => d.Type == "TenantID"))
            {
                var tenantID = Convert.ToInt32(claims.FirstOrDefault(d => d.Type == "TenantID").Value);
                result= ApiSecurityManager.GetAgreementText(tenantID, LangCode);
            }
            return result;
        }

        #endregion
    }
}