using CoreWeb.CommonOperations.Views;
using CoreWeb.Shell;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb
{
    public partial class Authenticator : System.Web.UI.Page
    {
        public GoogleAuthenticatorPresenter GoogleAuthenticatorContext
        {
            get
            {
                return new GoogleAuthenticatorPresenter();
            }
        }

        public Boolean IsLocationServiceTenant
        {
            get
            {
                if (!ViewState["IsLocationServiceTenant"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsLocationServiceTenant"]);
                return false;
            }
            set
            {
                ViewState["IsLocationServiceTenant"] = value;
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ManageLanguageTranslation();
        }

        #region Globalization for Multi-Language

        protected override void InitializeCulture()
        {
            //If is location service tenant and key added in config is true.
            Boolean isLanguageTransaltionEnable = ConfigurationManager.AppSettings["IsLanguageTranslation"].IsNullOrEmpty() ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["IsLanguageTranslation"]);

            var WebsiteUrl = Page.Request.ServerVariables.Get("server_name"); //"CBI.complio.com"; //
            Int32 tenantId = GoogleAuthenticatorContext.GetWebsiteTenantId(WebsiteUrl);
            if (tenantId > AppConsts.NONE)
            {
                IsLocationServiceTenant = GoogleAuthenticatorContext.IsLocationServiceTenant(tenantId);
                if (Session["IsLocationTenant"].IsNullOrEmpty())
                    SysXWebSiteUtils.SessionService.SetCustomData("IsLocationTenant", IsLocationServiceTenant);
            }

            if (isLanguageTransaltionEnable && IsLocationServiceTenant)
            {
                LanguageTranslateUtils.LanguageTranslateInit();
                base.InitializeCulture();
            }
        }

        protected void btnLanguage_Click(object sender, EventArgs e)
        {
            String languageCode = String.Empty;
            languageCode = hdnLanguageCode.Value;
            LanguageTranslateUtils.SetLanguageInSession(languageCode);
            Server.Transfer(Request.Url.PathAndQuery, false);
        }

        private void ManageLanguageTranslation()
        {
            var WebsiteUrl = Page.Request.ServerVariables.Get("server_name"); //"CBI.complio.com"; //
            Int32 tenantId = GoogleAuthenticatorContext.GetWebsiteTenantId(WebsiteUrl);
            if (tenantId > AppConsts.NONE)
            {
                IsLocationServiceTenant = GoogleAuthenticatorContext.IsLocationServiceTenant(tenantId);
                SysXWebSiteUtils.SessionService.SetCustomData("IsLocationTenant", IsLocationServiceTenant);
            }
            if (IsLocationServiceTenant)
            {
                dvLanguage.Style.Add("display", "block");
                btnLanguage.Visible = true;
                hdnLanguageCode.Value = Languages.ENGLISH.GetStringValue();

                string _currentLangSession = Convert.ToString(LanguageTranslateUtils.GetCurrentLanguageCultureFromSession());

                if (_currentLangSession.IsNullOrEmpty() || _currentLangSession.ToString() == LanguageCultures.ENGLISH_CULTURE.GetStringValue())
                {
                    btnLanguage.Text = "Spanish";
                    btnLanguage.ToolTip = "Click for Spanish";
                    hdnLanguageCode.Value = Languages.SPANISH.GetStringValue();
                }
                if (!_currentLangSession.IsNullOrEmpty() && _currentLangSession.ToString() == LanguageCultures.SPANISH_CULTURE.GetStringValue())
                {
                    btnLanguage.Text = "English";
                    btnLanguage.ToolTip = "Click for English";
                    hdnLanguageCode.Value = Languages.ENGLISH.GetStringValue();
                }
            }
            else
            {
                dvLanguage.Style.Add("display", "none");
                btnLanguage.Visible = false;

            }
        }
        #endregion

    }
}