using CoreWeb.Shell;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using Business.RepoManagers;
using INTSOF.UI.Contract.Globalization;

namespace CoreWeb
{
    public static class LanguageTranslateUtils
    {
        #region Globalization for Multi-Language

        public static void LanguageTranslateInit()
        {
            String languageCulture = GetCurrentLanguageCultureFromSession();
            SetCurrentThreadCulture(languageCulture);
        }
        public static void LanguageTranslateInit(String culture)
        {
            SetCurrentThreadCulture(culture);
        }

        private static void SetCurrentThreadCulture(String languageCulture)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(languageCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCulture);
        }

        /// <summary>
        /// If Current Language Culture is empty in the Session then default English Culture will be returned. 
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentLanguageCultureFromSession()
        {
            LanguageContract _currentCulture = (LanguageContract)(SysXWebSiteUtils.SessionService.GetCustomData("LanguageCulture"));
            if (!_currentCulture.IsNullOrEmpty())
            {
                return _currentCulture.LanguageCulture;
            }
            return LanguageCultures.ENGLISH_CULTURE.GetStringValue();
        }

        public static void SetLanguageInSession(string languageCode)
        {
            var _lkpLanguages = LookupManager.GetLookUpData<Entity.lkpLanguage>();
            var _currentLang = _lkpLanguages.Where(col => col.LAN_Code == languageCode).FirstOrDefault();

            LanguageContract _langContract = ConvertLanguageEntitytoContract(_currentLang);

            if (_langContract.IsNotNull())
            {
                SysXWebSiteUtils.SessionService.SetCustomData("LanguageCulture", _langContract);
            }
        }

        /// <summary>
        /// If Current Language in the Session is Empty then this will return Default 'English' language contract.
        /// </summary>
        /// <returns></returns>
        public static LanguageContract GetCurrentLanguageFromSession()
        {
            LanguageContract _currentLanguage = (LanguageContract)(SysXWebSiteUtils.SessionService.GetCustomData("LanguageCulture"));
            if (!_currentLanguage.IsNullOrEmpty())
            {
                return _currentLanguage;
            }

            var _lkpLanguages = LookupManager.GetLookUpData<Entity.lkpLanguage>();
            string _englishLangCode = Languages.ENGLISH.GetStringValue();
            var _englishLanguage = _lkpLanguages.Where(col => col.LAN_Code == _englishLangCode).FirstOrDefault();

            if (_englishLanguage.IsNotNull())
            {
                return ConvertLanguageEntitytoContract(_englishLanguage);
            }
            return null;
        }

        private static LanguageContract ConvertLanguageEntitytoContract(Entity.lkpLanguage _currentLang)
        {            
            LanguageContract _langContract = new LanguageContract();
            _langContract.LanguageID = _currentLang.LAN_ID;
            _langContract.LanguageName = _currentLang.LAN_Name;
            _langContract.LanguageCode = _currentLang.LAN_Code;
            _langContract.LanguageCulture = _currentLang.LAN_Culture;

            return _langContract;
        }

        #endregion

    }
}