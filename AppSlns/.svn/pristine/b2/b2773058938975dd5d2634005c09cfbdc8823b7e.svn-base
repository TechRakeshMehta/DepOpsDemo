using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.Config;
using System.Web.UI;

namespace CoreWeb
{
    public static class AccessibilityUtils
    {
        public static void SetVisibility(Dictionary<ImageButton, Boolean> dicBtns)
        {
            if (!dicBtns.IsNullOrEmpty())
            {
                foreach (ImageButton item in dicBtns.Keys)
                {
                    if (!item.IsNotNull())
                    {
                        Boolean _isVisible = false;
                        _isVisible = dicBtns.Where(cond => cond.Key == item).FirstOrDefault().Value;
                        item.Visible = _isVisible;
                    }
                }
            }
        }

        public static void SetAccessibilityInSession(Boolean isAccessibilityActive)
        {
            if (!isAccessibilityActive.IsNullOrEmpty())
                SysXWebSiteUtils.SessionService.SetCustomData("IsAccessibilityActive", isAccessibilityActive);
        }

        public static Boolean GetAccessibility()
        {
            if (!SysXWebSiteUtils.SessionService.GetCustomData("IsAccessibilityActive").IsNullOrEmpty())
                return Convert.ToBoolean(SysXWebSiteUtils.SessionService.GetCustomData("IsAccessibilityActive"));
            return false;
        }

        public static LinkedResource GetLinkedResourceFile(String path, ResourceTypes resType)
        {
            LinkedResource lnkRes = new LinkedResource();
            if (!path.IsNullOrEmpty() && !resType.IsNullOrEmpty())
            {
                lnkRes.Path = path;
                lnkRes.ResourceType = resType;
            }
            return lnkRes;
        }

        public static void LoadApplicationCss(String screenType, WclResourceManagerProxy resMngrPrxy = null, WclResourceManager resMngr = null)
        {
            if ((!resMngr.IsNullOrEmpty() || !resMngrPrxy.IsNullOrEmpty()) && !screenType.IsNullOrEmpty())
            {
                ResourceTypes resTypeCSS = ResourceTypes.StyleSheet;
                ResourceTypes resTypeJS = ResourceTypes.JavaScript;

                Boolean IsAccessibilityActive = false;
                if (!SysXWebSiteUtils.SessionService.GetCustomData("IsAccessibilityActive").IsNullOrEmpty())
                    IsAccessibilityActive = Convert.ToBoolean(SysXWebSiteUtils.SessionService.GetCustomData("IsAccessibilityActive"));

                LinkedResource lnkRes = new LinkedResource();

                switch (screenType)
                {
                    case AccessibilityScreenType.APPMASTER:
                        if (!resMngr.IsNullOrEmpty())
                        {
                            String _userPreferenceTheme = String.Empty;
                            String themeSection = String.Empty;
                            String defaultTheme = String.Empty;
                            WclTheme currentTheme = new WclTheme();

                            resMngr.SupportCssPath = "~/Resources/Generic/Client";
                            resMngr.SupportScriptPath = "~/Resources/Generic/Client";
                            resMngr.ThemesFolder = "~/Resources/Themes";

                            _userPreferenceTheme = (String)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_THEME);
                            themeSection = System.Configuration.ConfigurationManager.AppSettings["ThemeSection"];

                            // Step 1 : Add a new accessibility folder in Resources/Themes

                            if (IsAccessibilityActive)
                            {
                                defaultTheme = AppConsts.ACCESSIBILITY_THEME;
                                _userPreferenceTheme = defaultTheme;
                            }

                            else
                            {
                                defaultTheme = System.Configuration.ConfigurationManager.AppSettings["DefaultTheme"];
                            }

                            currentTheme = (System.Configuration.ConfigurationManager.GetSection(themeSection) as WclThemeSection).Themes[_userPreferenceTheme];
                            if (currentTheme == null)
                            {
                                _userPreferenceTheme = defaultTheme;
                                currentTheme = (System.Configuration.ConfigurationManager.GetSection(themeSection) as WclThemeSection).Themes[_userPreferenceTheme];
                            }

                            resMngr.ThemeName = _userPreferenceTheme;
                            resMngr.SkinCollection = !currentTheme.IsNull() ? currentTheme.Skins : null;
                        }
                        break;

                    case AccessibilityScreenType.DEFAULTMASTER:
                        if (!resMngr.IsNullOrEmpty())
                        {
                            String _userPreferenceTheme = String.Empty;
                            String themeSection = String.Empty;
                            String defaultTheme = String.Empty;
                            WclTheme currentTheme = new WclTheme();

                            resMngr.SupportCssPath = "~/Resources/Generic/Client";
                            resMngr.SupportScriptPath = "~/Resources/Generic/Client";
                            resMngr.ThemesFolder = "~/Resources/Themes";

                            _userPreferenceTheme = (String)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_THEME);
                            themeSection = System.Configuration.ConfigurationManager.AppSettings["ThemeSection"];

                            // Step 1 : Add a new accessibility folder in Resources/Themes

                            if (IsAccessibilityActive)
                            {
                                defaultTheme = AppConsts.ACCESSIBILITY_THEME;
                                _userPreferenceTheme = defaultTheme;
                            }

                            else
                            {
                                defaultTheme = System.Configuration.ConfigurationManager.AppSettings["DefaultTheme"];
                            }

                            currentTheme = (System.Configuration.ConfigurationManager.GetSection(themeSection) as WclThemeSection).Themes[_userPreferenceTheme];
                            if (currentTheme == null)
                            {
                                _userPreferenceTheme = defaultTheme;
                                currentTheme = (System.Configuration.ConfigurationManager.GetSection(themeSection) as WclThemeSection).Themes[_userPreferenceTheme];
                            }

                            resMngr.ThemeName = _userPreferenceTheme;
                            resMngr.SkinCollection = !currentTheme.IsNull() ? currentTheme.Skins : null;
                        }
                        break;


                    case AccessibilityScreenType.PUBLICPAGEMASTER:
                        if (!resMngr.IsNullOrEmpty())
                        {
                            if (IsAccessibilityActive)
                            {
                                lnkRes = GetLinkedResourceFile(AppConsts.Core_DEFAULT_ACCESSIBILITY_CSS_PATH, resTypeCSS);
                            }
                            else
                            {
                                lnkRes = GetLinkedResourceFile(AppConsts.Core_DEFAULT_CSS_PATH, resTypeCSS);
                            }
                        }
                        break;

                    case AccessibilityScreenType.LOGIN:
                        if (IsAccessibilityActive)
                        {
                            lnkRes = GetLinkedResourceFile(AppConsts.Login_ACCESSIBILITY_CSS_PATH, resTypeCSS);
                        }
                        else
                        {
                            lnkRes = GetLinkedResourceFile(AppConsts.Login_DEFAULT_CSS_PATH, resTypeCSS);
                        }
                        break;
                }

                //Add Linked Resource in resource Manager proxy and resource Manager.
                if (!lnkRes.IsNullOrEmpty())
                {
                    if (!resMngrPrxy.IsNullOrEmpty())
                        resMngrPrxy.LinkedResources.Add(lnkRes);
                    if (!resMngr.IsNullOrEmpty())
                        resMngr.LinkedResources.Add(lnkRes);
                }

            }
        }

        #region Globalization for Multi-Language

        public static string GetLanguageCulture()
        {
            //string currentCulture = null;
            //Boolean IsAccessibilityActive = false;
            //if (!SysXWebSiteUtils.SessionService.GetCustomData("IsAccessibilityActive").IsNullOrEmpty())
            //{
            //    IsAccessibilityActive = Convert.ToBoolean(SysXWebSiteUtils.SessionService.GetCustomData("IsAccessibilityActive"));
            //}
            ////if (ViewState["LanguageCulture"].ToString().IsNullOrEmpty())

            //if (IsAccessibilityActive)
            //{
            //    return currentCulture = Convert.ToString(LanguageTranslateUtils.GetCurrentLanguageCultureFromSession());
            //}
            //else
            //{
            //    return currentCulture = LanguageCultures.ENGLISH_CULTURE.GetStringValue();
            //}
            return LanguageCultures.ENGLISH_CULTURE.GetStringValue();
        }


        #endregion


    }
}