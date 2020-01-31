#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  Login.aspx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using System.Text;
using System.Collections.Generic;

#endregion

#region Application Specific

using INTSOF.Utils;
using CoreWeb.IntsofSecurityModel;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using Entity;
using System.Threading;
using System.Text.RegularExpressions;
using Business.RepoManagers;
using System.Web.Security;
using INTSOF.Utils.Consts;
using System.Linq;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Data.Entity.Core.EntityClient;
using System.Net.Mail;
using System.IO;


#endregion

#endregion

namespace CoreWeb.Shell.Views
{
    /// <summary>
    /// This class handles all the operations to be performed on login page.
    /// </summary>
    public partial class Login : System.Web.UI.Page, ILoginView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables
        private LoginPresenter _presenter = new LoginPresenter();
        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Presenter</summary>
        /// <value>
        /// Represents Manage Tenant Presenter.</value>

        public LoginPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public ILoginView CurrentViewContext
        {
            get
            {
                return this;
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

        ///// <summary>
        ///// ErrorMessage</summary>
        ///// <value>
        ///// Sets the value for Error Message.</value>
        //public String ErrorMessage
        //{
        //    set
        //    {

        //        lblErrorMessage.Text = value;
        //        //UAT-2792
        //        if (!lblErrorMessage.Text.IsNullOrEmpty())
        //        {
        //            dvShibbolethMessage.Style.Add("display", "none");
        //        }
        //    }
        //}

        //Boolean ILoginView.IsAccountInActive
        //{
        //    set
        //    {
        //        btnResendActivationLink.Visible = value;
        //        lblErrorMessageExtended.Visible = value;
        //        if (value)
        //        {
        //            lblErrorMessageExtended.Text = "to resend the account activation email.";
        //        }
        //    }
        //}

        //String ILoginView.EncPValue
        //{
        //    get
        //    {
        //        if (ViewState["EncPValue"] != null)
        //            return Convert.ToString(ViewState["EncPValue"]);
        //        return
        //           String.Empty;
        //    }
        //    set
        //    {
        //        if (ViewState["EncPValue"] == null)
        //            ViewState["EncPValue"] = value;
        //    }
        //}

        //public String VerificationMessage
        //{
        //    set
        //    {
        //        lblVerificationMessage.Text = value;
        //    }
        //}

        ///// <summary>
        ///// UserName</summary>
        ///// <value>
        ///// Gets or sets the value for user name.</value>
        //public String UserName
        //{
        //    get
        //    {
        //        return txtUserName.Text;
        //    }
        //    set
        //    {
        //        txtUserName.Text = value;
        //    }
        //}

        ///// <summary>
        ///// Password</summary>
        ///// <value>
        ///// Gets the value for password.</value>
        //public String Password
        //{
        //    get
        //    {
        //        return txtPassword.Text;
        //    }
        //}

        ///// <summary>
        ///// SelectedBlockID</summary>
        ///// <value>
        ///// Gets the value for selected block's id.</value>
        //public Int32 SelectedBlockId
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// SelectedBlockName</summary>
        ///// <value>
        ///// Gets the value for selected block name.</value>
        //public String SelectedBlockName
        //{
        //    get;
        //    set;
        //}

        //public String LoginPageImageUrl
        //{
        //    get;
        //    set;
        //}

        //public Int32 WebSiteId
        //{
        //    get;
        //    set;
        //}

        //public String SiteUrl
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// List of the Website Pages
        ///// </summary>
        //public List<WebSiteWebPage> lstWebsitePages
        //{
        //    get;
        //    set;
        //}

        //public string FooterHtml
        //{
        //    set { litFooter.Text = String.Empty; }
        //}

        //public Boolean CheckWebsiteURL
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings[SysXSecurityConst.CHECK_WEBSITE_URL_LOGIN].IsNull())
        //            return true;
        //        String chkWebsite = ConfigurationManager.AppSettings[SysXSecurityConst.CHECK_WEBSITE_URL_LOGIN];
        //        return Convert.ToBoolean(chkWebsite);
        //    }
        //}

        //public Boolean RedirectToMobileSite
        //{
        //    get
        //    {
        //        if (ConfigurationManager.AppSettings.AllKeys.Contains(INTSOF.Utils.Consts.SysXSecurityConst.REDIRECT_TO_MOBILE_SITE))
        //            return Convert.ToBoolean(!String.IsNullOrEmpty(ConfigurationManager.AppSettings[SysXSecurityConst.REDIRECT_TO_MOBILE_SITE]) ? ConfigurationManager.AppSettings[SysXSecurityConst.REDIRECT_TO_MOBILE_SITE] : "false");
        //        else
        //            return false;
        //    }
        //}

        ///// <summary>
        ///// property to get the curent year for copyright.
        ///// </summary>
        //public String CopyRightYear
        //{
        //    get { return DateTime.Now.Year.ToString(); }
        //}

        ///// <summary>
        ///// NOTE - WILL BE OVER-RIDEN, WHEN THE APPLICANT IS VALIDATED FOR AUTOLOGIN PROCESS
        ///// Property to decide whether the User is from Correct Tenant Url or not. 
        ///// Will be set TRUE even if it is Non-Central login type.
        ///// </summary>
        //public Boolean IsIncorrectLoginUrl
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Maintains the condition, whether the Tenant Url eneterd by appluicant exists in the database
        ///// </summary>
        //private Boolean InstUrlExists
        //{
        //    get;
        //    set;
        //}

        //#region UAT-1110 - Profiel Sharing
        //public String SharedUserLoginURL
        //{
        //    get
        //    {
        //        return Convert.ToString(ViewState["SharedUserLoginURL"]);
        //    }
        //    set
        //    {
        //        ViewState["SharedUserLoginURL"] = value;
        //    }
        //}
        //#endregion

        //#region UAT-1218 CRM Login Operations
        //List<OrganizationUserTypeMapping> ILoginView.OrganizationUserTypeMapping
        //{
        //    get
        //    {
        //        if (!ViewState["OrganizationUserTypeMapping"].IsNullOrEmpty())
        //        {
        //            return ViewState["OrganizationUserTypeMapping"] as List<OrganizationUserTypeMapping>;
        //        }
        //        return new List<OrganizationUserTypeMapping>();
        //    }
        //    set
        //    {
        //        ViewState["OrganizationUserTypeMapping"] = value;
        //    }

        //}
        //Boolean ILoginView.IsSharedUserHasOtherRoles { get; set; }
        //#endregion

        ///// <summary>
        ///// Gets the current user id.
        ///// </summary>
        ///// <remarks></remarks>
        //public String CurrentSessionId
        //{
        //    get
        //    {
        //        return Page.Session.SessionID.IsNullOrEmpty() ? String.Empty : Page.Session.SessionID;
        //    }
        //}

        ///// <summary>
        ///// UAT-2494, New Account verification enhancements (additional verification step)
        ///// </summary>
        //public Boolean ShowAdditionalAccountVerificationPage
        //{
        //    get
        //    {
        //        if (!ViewState["ShowAdditionalAccountVerificationPage"].IsNullOrEmpty())
        //        {
        //            return Convert.ToBoolean(ViewState["ShowAdditionalAccountVerificationPage"]);
        //        }
        //        return false;
        //    }
        //    set
        //    {
        //        ViewState["ShowAdditionalAccountVerificationPage"] = value;
        //    }
        //}
        //#region UAT-2792 UCONN SSO Process

        //Boolean ILoginView.IsShibbolethLogin
        //{
        //    get
        //    {
        //        if (!ViewState["IsShibbolethLogin"].IsNullOrEmpty())
        //        {
        //            return Convert.ToBoolean(ViewState["IsShibbolethLogin"]);
        //        }
        //        return false;
        //    }
        //    set
        //    {
        //        ViewState["IsShibbolethLogin"] = value;
        //    }
        //}

        //String ILoginView.ShibbolethUniqueIdentifier
        //{
        //    get
        //    {
        //        if (!ViewState["ShibbolethUniqueIdentifier"].IsNullOrEmpty())
        //        {
        //            return Convert.ToString(ViewState["ShibbolethUniqueIdentifier"]);
        //        }
        //        return String.Empty;
        //    }
        //    set
        //    {
        //        ViewState["ShibbolethUniqueIdentifier"] = value;
        //    }
        //}

        //Int32 ILoginView.IntegrationClientId
        //{
        //    get
        //    {
        //        return Convert.ToInt32(ViewState["IntegrationClientId"]);
        //    }
        //    set
        //    {
        //        ViewState["IntegrationClientId"] = value;
        //    }
        //}

        //Boolean ILoginView.IsAutoLoginThroughShibboleth { get; set; }

        //Int32 ILoginView.ShibbolethHostID
        //{
        //    get
        //    {
        //        return Convert.ToInt32(ViewState["ShibbolethHostID"]);
        //    }
        //    set
        //    {
        //        ViewState["ShibbolethHostID"] = value;
        //    }
        //}

        //String ILoginView.HostName
        //{
        //    get
        //    {
        //        return Convert.ToString(ViewState["HostName"]);
        //    }
        //    set
        //    {
        //        ViewState["HostName"] = value;
        //    }
        //}

        //Boolean ILoginView.IsExistingAccount
        //{
        //    get
        //    {
        //        return Convert.ToBoolean(ViewState["IsExistingAccount"]);
        //    }
        //    set
        //    {
        //        ViewState["IsExistingAccount"] = value;
        //    }
        //}
        //Boolean ILoginView.IsShibbolethApplicant
        //{
        //    get
        //    {
        //        return Convert.ToBoolean(ViewState["IsShibbolethApplicant"]);
        //    }
        //    set
        //    {
        //        ViewState["IsShibbolethApplicant"] = value;
        //    }
        //}

        //#endregion

        ////UAT-2930
        //Boolean ILoginView.IsTwoFactorAuthenticationRequired
        //{
        //    get;
        //    set;
        //}
        #endregion

        #region Events

        #region PAGE EVENTS

        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        /// 
        protected void Page_Init(object sender, EventArgs e)
        {
            SetDeviceType();
            ManageLanguageTranslation();
            LoadLoginControl();
            SetUserSessionAndRedirect();
            //VerifyTokenAndRedirect();
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            if (!Request.QueryString["lang"].IsNullOrEmpty())
            {
                //LanguageTranslateUtils.LanguageTranslateInit(Request.QueryString["lang"]);
                Form.Action = Request.Url.ToString().Replace("&lang=" + Request.QueryString["lang"], String.Empty);
            }
        }
        private void LoadLoginControl()
        {
            try
            {
                //use tenantid for visibity of user controls
                Int32 tenantId = AppConsts.NONE;
                String SiteUrl = Page.Request.ServerVariables.Get("server_name");
                if (!String.IsNullOrEmpty(SiteUrl))
                {
                    tenantId = Presenter.GetWebsiteTenantId(SiteUrl);

                    UserControl userControl;
                    String userControlPath = String.Empty;
                    String userControlParentPath = AppConsts.ControlParentPath;

                    if (!tenantId.IsNullOrEmpty() && tenantId > AppConsts.NONE)
                    {
                        Entity.WebSite webSiteDetail = Presenter.GetWebSiteDetail(tenantId);

                        if (webSiteDetail.IsNullOrEmpty() || webSiteDetail.LoginControlName.IsNullOrEmpty())
                        {
                            userControlPath = userControlParentPath + LoginPageToOpen.CommonLogin.GetStringValue();// +".ascx";

                        }
                        else
                        {
                            if (webSiteDetail.LoginControlName.ToLower() == LoginPageToOpen.CommonLogin.GetStringValue().ToLower())
                            {
                                userControlPath = userControlParentPath + LoginPageToOpen.CommonLogin.GetStringValue();//+ ".ascx";

                            }
                            if (webSiteDetail.LoginControlName.ToLower() == LoginPageToOpen.ConfigurableLogin.GetStringValue().ToLower())
                            {
                                userControlPath = userControlParentPath + LoginPageToOpen.ConfigurableLogin.GetStringValue();// +".ascx";
                            }
                        }
                        userControl = LoadControl(userControlPath) as UserControl;
                        phDynamic.Controls.Add(userControl);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        #region Globalization for Multi-Language

        protected override void InitializeCulture()
        {
            //If is location service tenant and key added in config is true.
            Boolean isLanguageTransaltionEnable = ConfigurationManager.AppSettings["IsLanguageTranslation"].IsNullOrEmpty() ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["IsLanguageTranslation"]);

            var WebsiteUrl = Page.Request.ServerVariables.Get("server_name"); // "CBI.complio.com"; // 
            //var WebsiteUrl = "CBI.complio.com";
            Int32 tenantId = Presenter.GetWebsiteTenantId(WebsiteUrl);
            if (tenantId > AppConsts.NONE)
            {
                IsLocationServiceTenant = Presenter.IsLocationServiceTenant(tenantId);
                if (Session["IsLocationTenant"].IsNullOrEmpty())
                    SysXWebSiteUtils.SessionService.SetCustomData("IsLocationTenant", IsLocationServiceTenant);
            }

            if (isLanguageTransaltionEnable && IsLocationServiceTenant)
            {
                if (!Request.QueryString["lang"].IsNullOrEmpty())
                {
                    LanguageTranslateUtils.LanguageTranslateInit(Request.QueryString["lang"]);
                    //Form.Action = Form.Action.Remove("&lang=" + Request.QueryString["lang"]);
                }
                else
                {
                    LanguageTranslateUtils.LanguageTranslateInit();
                }
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
            Int32 tenantId = Presenter.GetWebsiteTenantId(WebsiteUrl);
            if (tenantId > AppConsts.NONE)
            {
                IsLocationServiceTenant = Presenter.IsLocationServiceTenant(tenantId);
                SysXWebSiteUtils.SessionService.SetCustomData("IsLocationTenant", IsLocationServiceTenant);
            }
            if (IsLocationServiceTenant)
            {
                dvLanguage.Style.Add("display", "block");
                btnLanguage.Visible = true;
                hdnLanguageCode.Value = Languages.ENGLISH.GetStringValue();

                string _currentLangSession = String.Empty;
                if (!Request.QueryString["lang"].IsNullOrEmpty())
                {
                    _currentLangSession = Request.QueryString["lang"].ToString();
                }
                else
                {
                    _currentLangSession = Convert.ToString(LanguageTranslateUtils.GetCurrentLanguageCultureFromSession());
                }

                if (_currentLangSession.IsNullOrEmpty() || _currentLangSession.ToString() == LanguageCultures.ENGLISH_CULTURE.GetStringValue())
                {
                    btnLanguage.Text = Resources.Language.SPANISH;
                    btnLanguage.ToolTip = Resources.Language.LANGUAGEBUTTONTOOLTIPSPANISH_P1 + " " + "(" + Resources.Language.LANGUAGEBUTTONTOOLTIPSPANISH_P2 + ")";
                    hdnLanguageCode.Value = Languages.SPANISH.GetStringValue();
                }
                if (!_currentLangSession.IsNullOrEmpty() && _currentLangSession.ToString() == LanguageCultures.SPANISH_CULTURE.GetStringValue())
                {
                    btnLanguage.Text = Resources.Language.ENGLISH;
                    btnLanguage.ToolTip = Resources.Language.LANGUAGEBUTTONTOOLTIPENG_P1 + " " + "(" + Resources.Language.LANGUAGEBUTTONTOOLTIPENG_P2 + ")";
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

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (RedirectToMobileSite && !IsPostBack)
        //        {
        //            if (((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).IsMobileDevice)
        //            {
        //                //Code implemented for mobile app.
        //                Boolean isReturnToDesktopSite = true;
        //                if (!Request.QueryString["isReturnToDesktopSite"].IsNullOrEmpty())
        //                    isReturnToDesktopSite = false;
        //                if (isReturnToDesktopSite)
        //                {
        //                    string mobileUrl = new Uri(HttpContext.Current.Request.Url.AbsoluteUri).GetLeftPart(UriPartial.Authority).Replace(HttpContext.Current.Request.Url.Scheme + "://", "");
        //                    #region UAT-3428
        //                    String mobileUrlPrefix = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_MOBILE_URL_PREFIX]);
        //                    #endregion

        //                    Response.Redirect(HttpContext.Current.Request.Url.Scheme + "://" + mobileUrlPrefix + mobileUrl);
        //                }
        //            }
        //        }
        //        // get members user
        //        SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;

        //        //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        //        if (!Request.QueryString["DeletePrevUsrState"].IsNullOrEmpty() && Convert.ToBoolean(Request.QueryString["DeletePrevUsrState"]))
        //        {
        //            Presenter.DoLogOff(true);
        //        }

        //        if (!Request.QueryString["ChangePassword"].IsNull())
        //        {
        //            String strClientScript = "<script type=\"text/javascript\"> if (top !== self) { top.location = " + "'" + Request.Url.ToString() + "'" + "; } </" + "script>";
        //            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", strClientScript);
        //        }
        //        else
        //        {
        //            StringBuilder redirectURL = new StringBuilder(Uri.UriSchemeHttp.ToString() + Uri.SchemeDelimiter.ToString() + Context.Request.Url.Authority + Context.Request.Url.Segments[0].ToString() + Context.Request.Url.Segments[1].ToString());
        //            String strClientScript = "<script type=\"text/javascript\"> if (top !== self) { top.location = " + "'" + redirectURL + "'" + "; } </" + "script>";
        //            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", strClientScript);
        //        }

        //        Boolean needToShowEmploymentResultReport = false;
        //        SiteUrl = Page.Request.ServerVariables.Get("server_name");
        //        CurrentViewContext.SharedUserLoginURL = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);

        //        if (!user.IsNull())
        //        {
        //            #region UAT-1053
        //            Boolean showReport = false;
        //            Boolean permissionViolated = false;
        //            Dictionary<String, String> args = new Dictionary<String, String>();
        //            String queryStringForReportViewer = "";
        //            if (!Request.QueryString["args"].IsNull())
        //            {
        //                args.ToDecryptedQueryString(Request.QueryString["args"]);
        //                if (args.ContainsKey("ReportType"))
        //                {
        //                    //If the report sent to the student and currently the same student logged in
        //                    if (args.ContainsKey("IsReportSentToStudent") && Convert.ToBoolean(args["IsReportSentToStudent"]) == true && args.ContainsKey("OrganizationUserID"))
        //                    {
        //                        if (user.IsNotNull() && user.IsApplicant && user.OrganizationUserId == Convert.ToInt32(args["OrganizationUserID"]))
        //                        {
        //                            showReport = true;
        //                        }
        //                        else
        //                        {
        //                            permissionViolated = true;
        //                        }
        //                    }

        //                    if (args.ContainsKey("IsReportSentToStudent")
        //                           && Convert.ToBoolean(args["IsReportSentToStudent"]) == false
        //                           && args.ContainsKey("HierarchyNodeID")
        //                           && Convert.ToInt32(args["HierarchyNodeID"]) == -1)
        //                    {
        //                        if (user.IsNotNull() && !user.IsApplicant)
        //                        {
        //                            showReport = true;
        //                        }
        //                        else
        //                        {
        //                            permissionViolated = true;
        //                        }
        //                    }

        //                    //If the report is sent to client admin and he has node permission
        //                    if (args.ContainsKey("HierarchyNodeID") && Convert.ToInt32(args["HierarchyNodeID"]) > 0)
        //                    {
        //                        Boolean result = false;
        //                        if (user.IsNotNull() && !user.IsApplicant && user.TenantId != AppConsts.SUPER_ADMIN_TENANT_ID && user.TenantTypeCode == TenantType.Institution.GetStringValue())
        //                        {
        //                            result = Presenter.HasNodePermission(user.TenantId.Value, user.OrganizationUserId, Convert.ToInt32(args["HierarchyNodeID"]));
        //                        }
        //                        if (result)
        //                        {
        //                            showReport = true;
        //                        }
        //                        else
        //                        {
        //                            permissionViolated = true;
        //                        }
        //                    }

        //                    //Report can be seen by super admin in all cases
        //                    if (user.IsNotNull() && user.TenantId == AppConsts.SUPER_ADMIN_TENANT_ID && !user.IsSharedUser)
        //                    {
        //                        showReport = true;
        //                    }
        //                    else
        //                    {
        //                        permissionViolated = true;
        //                    }
        //                    queryStringForReportViewer = args.ToEncryptedQueryString();
        //                }
        //                else if (args.ContainsKey("DocumentType"))
        //                {

        //                }


        //            }
        //            needToShowEmploymentResultReport = showReport;
        //            if (showReport)
        //            {
        //                SysXWebSiteUtils.SessionService.SetCustomData("CURRENT_URL", AppConsts.BKG_REPORT_VIEWER + "?args=" + queryStringForReportViewer);
        //            }
        //            else if (permissionViolated)
        //            {
        //                SysXWebSiteUtils.SessionService.SetCustomData("CURRENT_URL", AppConsts.BKG_REPORT_VIEWER + "?args=PermissionVoilated");
        //            }
        //            #endregion

        //            HandleAgencyVerification(user.UserName);

        //            Response.Redirect("Default.aspx", false);
        //        }

        //        this.InstUrlExists = Presenter.IsUrlExistForInstitutionType();

        //        if (this.InstUrlExists || SiteUrl.Contains(AppConsts.LOCAL_HOST) || !IsCentralLogin())
        //            ShowCentralLoginDiv(false);
        //        else
        //            ShowCentralLoginDiv(true);

        //        if (Presenter.IsUrlAdminType())
        //            ShowCentralLoginDiv(true, true);

        //        //UAT-1110 - Profile sharing - hiding create account and shared create account divs if current url is shared login URL
        //        if (IsSharedUserLogin())
        //        {
        //            dvCreateAccount.Visible = false;
        //            divCentralCreateAccount.Visible = false;
        //        }

        //        #region UAT-1110 - Profile Sharing And UAT-1218

        //        Entity.OrganizationUser orgUser = new Entity.OrganizationUser();
        //        Dictionary<String, String> profileArgs = new Dictionary<String, String>();
        //        if (SiteUrl.ToLower() == CurrentViewContext.SharedUserLoginURL.ToLower() && !Request.QueryString["args"].IsNull())
        //        {
        //            profileArgs.ToDecryptedQueryString(Request.QueryString["args"]);

        //            //If Client Contact Token is recieved from QueryString
        //            if (profileArgs.ContainsKey(AppConsts.QUERY_STRING_CLIENTCONTACT_TOKEN))
        //            {
        //                orgUser = Presenter.IsSharedUserExists(Guid.Parse(profileArgs[AppConsts.QUERY_STRING_CLIENTCONTACT_TOKEN]), false, null);
        //            }
        //            //If Invitation Token is recieved from QueryString
        //            else if (profileArgs.ContainsKey(AppConsts.QUERY_STRING_INVITE_TOKEN))
        //            {
        //                orgUser = Presenter.IsSharedUserExists(Guid.Parse(profileArgs[AppConsts.QUERY_STRING_INVITE_TOKEN]), true, null);
        //            }
        //            else if (profileArgs.ContainsKey(AppConsts.QUERY_STRING_AGENCY_USER_ID))
        //            {
        //                orgUser = Presenter.IsSharedUserExists(Guid.Empty, false, Convert.ToInt32(profileArgs[AppConsts.QUERY_STRING_AGENCY_USER_ID]));
        //            }

        //            //Add OrguserTypeMapping if current UserTypeCode is not exist
        //            if (!orgUser.IsNullOrEmpty())
        //            {
        //                if (profileArgs.ContainsKey(AppConsts.PROFILE_SHARING_URL_TYPE)
        //                    && profileArgs[AppConsts.PROFILE_SHARING_URL_TYPE].Equals(AppConsts.PROFILE_SHARING_URL_TYPE_AGENCY_VERIFICATION))
        //                {
        //                    String userName = orgUser.aspnet_Users.UserName;
        //                    txtUserName.Text = userName;
        //                    txtUserName.Enabled = false;
        //                }
        //                else
        //                {
        //                    Presenter.GetOrganizationUserTypeMapping(orgUser.UserID);
        //                    List<String> orgUserTypeCode = new List<String>();
        //                    if (!CurrentViewContext.OrganizationUserTypeMapping.IsNullOrEmpty())
        //                    {
        //                        orgUserTypeCode = CurrentViewContext.OrganizationUserTypeMapping.Select(col => col.lkpOrgUserType.OrgUserTypeCode).ToList();
        //                    }
        //                    if (!orgUserTypeCode.Contains(Convert.ToString(profileArgs[AppConsts.QUERY_STRING_USER_TYPE_CODE])))
        //                    {
        //                        Presenter.AddOrgUserTypeMapping(orgUser.OrganizationUserID, Convert.ToString(profileArgs[AppConsts.QUERY_STRING_USER_TYPE_CODE]));
        //                    }
        //                    if (Convert.ToString(profileArgs[AppConsts.QUERY_STRING_USER_TYPE_CODE]) == OrganizationUserType.AgencyUser.GetStringValue())
        //                    {
        //                        if (profileArgs.ContainsKey(AppConsts.QUERY_STRING_INVITE_TOKEN))
        //                        {
        //                            Presenter.UpdateInviteeOrganizationUserID(orgUser.OrganizationUserID, Guid.Parse(profileArgs[AppConsts.QUERY_STRING_INVITE_TOKEN]), null);
        //                        }
        //                        else if (profileArgs.ContainsKey(AppConsts.QUERY_STRING_AGENCY_USER_ID))
        //                        {
        //                            Presenter.UpdateInviteeOrganizationUserID(orgUser.OrganizationUserID, Guid.Empty, Convert.ToInt32(profileArgs[AppConsts.QUERY_STRING_AGENCY_USER_ID]));
        //                        }
        //                        Presenter.AssignDefaultRolesToAgencyUser(orgUser);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                //redirect to shared user registration page
        //                Response.Redirect(AppConsts.SHARED_USER_REGISTRATION + "?args=" + Request.QueryString["args"], true);
        //            }
        //        }
        //        #endregion

        //        if (!IsPostBack)
        //        {
        //            ValidateUserAndAutoLogin(needToShowEmploymentResultReport);
        //            ValidateUserViaEmail();
        //            //ManageAlumniRedirection();

        //            //UAT-2494, New Account verification enhancements (additional verification step)
        //            if (CurrentViewContext.ShowAdditionalAccountVerificationPage)
        //            {
        //                hdnAccountVerificationPopup.Value = "SHOWPOPUP";
        //                hdnVerificationCode.Value = Request.QueryString["UsrVerCode"].ToString();
        //            }
        //            else if (!String.IsNullOrEmpty(Request.QueryString["IsUserActivated"]))
        //            {
        //                if (Request.QueryString["IsUserActivated"].ToString() == "1")
        //                {
        //                    CurrentViewContext.VerificationMessage = ResourceConst.SECURITY_VERIFICATION_SUCCESS_MESSAGE;
        //                }
        //            }
        //            else
        //            {
        //                //Else existing login process before UAT-2494
        //                ValidateEmailAddressViaEmail();
        //            }

        //            SetValidations("revUserName", ResourceConst.SECURITY_INVALID_CHARACTER);
        //            SetValidations("revPassword", ResourceConst.SECURITY_INVALID_CHARACTER);

        //            ((RequiredFieldValidator)FindControl("rfvUserName")).ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ENTER_USERNAME);
        //            ((RequiredFieldValidator)FindControl("rfvPassword")).ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ENTER_PASSWORD);

        //            Presenter.OnViewInitialized();

        //            #region SET LOGIN PAGE IMAGE BASED ON THE CLIENT URL

        //            ManageLoginPageImage(sender);

        //            #endregion

        //            lblInstituteName.Text = AppUtils.GetInstitutionName;

        //            #region UAT-2792 UCONN SSO Process
        //            Dictionary<String, String> shibbolethArgs = new Dictionary<String, String>();
        //            if (!Request.QueryString["shibbolethArgs"].IsNull())
        //            {
        //                shibbolethArgs.ToDecryptedQueryString(Request.QueryString["shibbolethArgs"]);
        //                if (shibbolethArgs.ContainsKey("IsShibbolethLogin"))
        //                {
        //                    if (!shibbolethArgs["IsShibbolethLogin"].IsNullOrEmpty())
        //                    {
        //                        CurrentViewContext.IsShibbolethLogin = Convert.ToBoolean(shibbolethArgs["IsShibbolethLogin"]);
        //                    }
        //                    if (shibbolethArgs.ContainsKey("ShibbolethUniqueIdentifier") && !shibbolethArgs["ShibbolethUniqueIdentifier"].IsNullOrEmpty())
        //                    {
        //                        CurrentViewContext.ShibbolethUniqueIdentifier = Convert.ToString(shibbolethArgs["ShibbolethUniqueIdentifier"]);
        //                    }
        //                    if (shibbolethArgs.ContainsKey("IntegrationClientID") && !shibbolethArgs["IntegrationClientID"].IsNullOrEmpty())
        //                    {
        //                        CurrentViewContext.IntegrationClientId = Convert.ToInt32(shibbolethArgs["IntegrationClientID"]);
        //                    }
        //                    if (shibbolethArgs.ContainsKey("IsAutoLoginThroughShibboleth") && !shibbolethArgs["IsAutoLoginThroughShibboleth"].IsNullOrEmpty())
        //                    {
        //                        CurrentViewContext.IsAutoLoginThroughShibboleth = Convert.ToBoolean(shibbolethArgs["IsAutoLoginThroughShibboleth"]);
        //                    }
        //                    if (shibbolethArgs.ContainsKey("UserName") && !shibbolethArgs["UserName"].IsNullOrEmpty())
        //                    {
        //                        CurrentViewContext.UserName = Convert.ToString(shibbolethArgs["UserName"]);
        //                    }
        //                    if (shibbolethArgs.ContainsKey("TenantID") && !shibbolethArgs["TenantID"].IsNullOrEmpty())
        //                    {
        //                        CurrentViewContext.ShibbolethHostID = Convert.ToInt32(shibbolethArgs["TenantID"]);
        //                    }
        //                    if (shibbolethArgs.ContainsKey("Host") && !shibbolethArgs["Host"].IsNullOrEmpty())
        //                    {
        //                        CurrentViewContext.HostName = Convert.ToString(shibbolethArgs["Host"]);
        //                    }
        //                    if (shibbolethArgs.ContainsKey("IsExistingAccount") && !shibbolethArgs["IsExistingAccount"].IsNullOrEmpty())
        //                    {
        //                        CurrentViewContext.IsExistingAccount = Convert.ToBoolean(shibbolethArgs["IsExistingAccount"]);
        //                    }
        //                    if (shibbolethArgs.ContainsKey("IsShibbolethApplicant") && !shibbolethArgs["IsShibbolethApplicant"].IsNullOrEmpty())
        //                    {
        //                        CurrentViewContext.IsShibbolethApplicant = Convert.ToBoolean(shibbolethArgs["IsShibbolethApplicant"]);
        //                    }
        //                }
        //            }
        //            #endregion

        //            //UAT 3600
        //            if (!Session["IsAutoActivateAndLogin"].IsNullOrEmpty() && Session["IsAutoActivateAndLogin"].ToString() == "true")
        //            {
        //                Dictionary<String, String> autoLoginArgs = new Dictionary<String, String>();
        //                if (!Request.QueryString["autoLoginArgs"].IsNull())
        //                {
        //                    autoLoginArgs.ToDecryptedQueryString(Request.QueryString["autoLoginArgs"]);
        //                    if (autoLoginArgs.ContainsKey("UserName") && !autoLoginArgs["UserName"].IsNullOrEmpty())
        //                    {
        //                        CurrentViewContext.UserName = Convert.ToString(autoLoginArgs["UserName"]);
        //                        if (Presenter.AutoLogInUsingUserName())
        //                        {
        //                            SysXMembershipUser shibbonethUser = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
        //                            HandleUserLoginType(shibbonethUser);
        //                            HandleDisclosureFormDisplay(shibbonethUser, needToShowEmploymentResultReport);
        //                        }
        //                    }
        //                }
        //            }

        //        }

        //        txtUserName.Focus();
        //        Presenter.OnViewLoaded();

        //        if (!Request.QueryString[AppConsts.SESSION_EXPIRED].IsNull())
        //        {
        //            ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_SESSION_EXPIRED);
        //        }
        //        else if (!Request.QueryString["ChangePassword"].IsNull())
        //        {
        //            ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_PASSWORD_CHANGED_SUCCESSFULLY);
        //        }
        //        else if (!Request.QueryString["ForgotPassword"].IsNull())
        //        {
        //            ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_NEWPASSWORD_SENT_BYEMAIL);
        //        }
        //        else if (!Request.QueryString["ForgotUserName"].IsNull())
        //        {
        //            ErrorMessage = ResourceConst.SECURITY_USERNAME_SENT_BYEMAIL;
        //        }
        //        else if (Request.QueryString["logout"] == "module") // Add This Functionality on TFS BUG # 2111
        //        {
        //            System.Reflection.PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        //            isreadonly.SetValue(Request.QueryString, false, null);
        //            Request.QueryString.Remove("logout");
        //            SysXWebSiteUtils.SessionService.ClearSession(true);
        //        }


        //        #region UAT-2792 UCONN SSO Process
        //        if (CurrentViewContext.IsShibbolethLogin && CurrentViewContext.IsExistingAccount)
        //        {
        //            dvShibbolethMessage.Style.Add("display", "block");
        //        }
        //        if (CurrentViewContext.IsShibbolethLogin)
        //        {
        //            dvCreateAccount.Visible = false;
        //        }
        //        if (CurrentViewContext.IsAutoLoginThroughShibboleth)
        //        {
        //            if (Presenter.AutoLogInUsingUserName())
        //            {
        //                SysXMembershipUser shibbonethUser = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
        //                HandleUserLoginType(shibbonethUser);
        //                HandleDisclosureFormDisplay(shibbonethUser, needToShowEmploymentResultReport);
        //            }
        //            CurrentViewContext.IsAutoLoginThroughShibboleth = false;
        //        }

        //        #endregion

        //        //UAT-2894
        //        if (!user.IsNull())
        //        {
        //            SetSessionForFilteredRequirementShares(true);
        //        }

        //        //UAT-2930
        //        if (!user.IsNullOrEmpty() &&
        //            (SysXWebSiteUtils.SessionService.UserGoogleAuthenticated == GoogleAuthenticationStatus.Authenticated
        //            || SysXWebSiteUtils.SessionService.UserGoogleAuthenticated == GoogleAuthenticationStatus.NotApplicable))
        //        {
        //            //to do login auto login user through username
        //            FormsAuthentication.RedirectFromLoginPage(user.UserName, false);
        //            CurrentViewContext.UserName = user.UserName;
        //            HandlePendingButtonClick();

        //        }

        //        //UAT 3600
        //        if (!Session["IsAutoActivateAndLogin"].IsNullOrEmpty() && Session["IsAutoActivateAndLogin"].ToString() == "true")
        //        {
        //            FormsAuthentication.RedirectFromLoginPage(user.UserName, false);
        //            CurrentViewContext.UserName = user.UserName;
        //            HandlePendingButtonClick();
        //        }

        //        #region UAT 3054

        //        Dictionary<String, String> dicargs = new Dictionary<String, String>();
        //        if (!Request.QueryString["args"].IsNull())
        //        {
        //            dicargs.ToDecryptedQueryString(Request.QueryString["args"]);

        //            if (dicargs.ContainsKey("IslinkingExternalUser") && Convert.ToBoolean(dicargs["IslinkingExternalUser"]) && dicargs.ContainsKey("ExternalID") && !dicargs["ExternalID"].IsNullOrEmpty() && dicargs.ContainsKey("IntegrationClientId") && Convert.ToInt32(dicargs["IntegrationClientId"]) > 0)
        //            {
        //                SysXMembershipUser organizationUser = System.Web.Security.Membership.GetUser(System.Text.RegularExpressions.Regex.Replace(CurrentViewContext.UserName, @"(?<=^\s*)\s|\s(?=\s*$)", INTSOF.Utils.Consts.SysXSecurityConst.ASCIISPACE)) as SysXMembershipUser;

        //                if (organizationUser.IsNotNull())
        //                {
        //                    Presenter.InsertAceMappLoginIntegrationEntry(organizationUser.OrganizationUserId, dicargs["ExternalID"].ToString(), Convert.ToInt32(dicargs["IntegrationClientId"]));
        //                }
        //            }
        //        }
        //        #endregion

        //        #region [UAT-3177: Hide Create Account button in case of Alumni Tenant]
        //        if (!String.IsNullOrEmpty(SiteUrl) && !SiteUrl.Contains(AppConsts.LOCAL_HOST) && !IsCentralLogin())
        //        {
        //            Int32 alumniTenantId = Presenter.GetAlumniTenantId();
        //            if (alumniTenantId > AppConsts.NONE)
        //            {
        //                Int32 selectedTenantID = Presenter.GetWebsiteTenantId(SiteUrl);
        //                if (selectedTenantID > AppConsts.NONE && selectedTenantID == alumniTenantId)
        //                {
        //                    ShowCentralLoginDiv(false, true);
        //                }
        //            }
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = ex.Message;
        //    }

        //}
        ///// <summary>
        ///// UAT-2894
        ///// </summary>
        ///// <param name="IsNeedToRedireactToMainDefault"></param>
        //private void SetSessionForFilteredRequirementShares(Boolean IsNeedToRedireactToMainDefault)
        //{
        //    if (!Request.QueryString["args"].IsNullOrEmpty())
        //    {
        //        Dictionary<String, String> requestArgs = new Dictionary<String, String>();
        //        requestArgs.ToDecryptedQueryString(Request.QueryString["args"]);
        //        if (!requestArgs.IsNullOrEmpty() && requestArgs.ContainsKey("IsSearchedShare"))
        //        {
        //            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_FILTERED_REQUIREMENT_SHARES, requestArgs);
        //            if (IsNeedToRedireactToMainDefault)
        //                Response.Redirect("~/Main/Default.aspx");
        //        }
        //    }
        //}
        ////UAT-2960
        //private void SetSessionForAlumniToken(SysXMembershipUser user)
        //{
        //    if (!Request.QueryString["args"].IsNullOrEmpty())
        //    {
        //        Dictionary<String, String> requestArgs = new Dictionary<String, String>();
        //        requestArgs.ToDecryptedQueryString(Request.QueryString["args"]);
        //        if (!requestArgs.IsNullOrEmpty() && requestArgs.ContainsKey("AlumniToken"))
        //        {
        //            Guid Token = Guid.Parse(requestArgs["AlumniToken"]);
        //            if (!Token.IsNullOrEmpty())
        //            {
        //                Int32 orgUserId = user.OrganizationUserId.IsNullOrEmpty() ? AppConsts.NONE : user.OrganizationUserId;
        //                if (Presenter.CheckForAlumnAccessStatus(Token, orgUserId))
        //                {
        //                    if (Session["ClientMachineIP"].IsNullOrEmpty())
        //                    {
        //                        Session["ClientMachineIP"] = Request.UserHostAddress;
        //                    }
        //                    String MachineIP = Convert.ToString(Session["ClientMachineIP"]);
        //                    Presenter.CreateAlumniDefaultSubscription(user.OrganizationUserId, user.OrganizationUserId, Convert.ToInt32(user.TenantId), MachineIP);
        //                }
        //            }
        //        }
        //    }
        //}

        #endregion

        //#region BUTTON EVENTS

        ///// <summary>
        ///// Handles the Click event of the btnSubmit control.
        ///// </summary>
        ///// <param name="sender">The source of the event.</param>
        ///// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        ///// <remarks></remarks>
        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        CurrentViewContext.VerificationMessage = String.Empty;
        //        SiteUrl = Page.Request.ServerVariables.Get("server_name");
        //        CurrentViewContext.IsAccountInActive = false;
        //        lblErrorMessageExtended.Text = String.Empty;
        //        lblErrorMessageExtended.Visible = false;
        //        Presenter.ValidateUserAndRedirect();

        //        //UAT-2930
        //        if (CurrentViewContext.IsTwoFactorAuthenticationRequired
        //          && (SysXWebSiteUtils.SessionService.UserGoogleAuthenticated == GoogleAuthenticationStatus.NotAuthenticated_With_GoogleAuthenticator
        //          || SysXWebSiteUtils.SessionService.UserGoogleAuthenticated == GoogleAuthenticationStatus.NotAuthenticated_With_TextMessage))
        //        {
        //            if (!Request.QueryString["args"].IsNull())
        //            {
        //                Session["PreLoginQueryString"] = Convert.ToString(Request.QueryString["args"]);
        //            }
        //            Response.Redirect("Authenticator.aspx", true);
        //        }

        //        HandlePendingButtonClick();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = ex.Message;
        //    }
        //}

        ///// <summary>
        ///// Resent Activation Button Click Event
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void btnResendActivationLink_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (Presenter.ResendVerificationLink())
        //        {
        //            ErrorMessage = String.Empty;
        //            CurrentViewContext.IsAccountInActive = false;
        //            lblErrorMessageExtended.Text = String.Empty;
        //            CurrentViewContext.VerificationMessage = "An activation link has been sent to your email address.";
        //        }
        //        else
        //        {
        //            ErrorMessage = "Some error occured while sending the activation link.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = ex.Message;
        //    }
        //}

        //#endregion

        #endregion

        //#region Methods

        //#region PUBLIC METHODS
        //#endregion

        //#region PRIVATE METHODS

        ///// <summary>
        ///// A we have break the submit ckick in 2 peices.
        ///// </summary>
        //private void HandlePendingButtonClick()
        //{
        //    SetSessionForFilteredRequirementShares(false);

        //    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
        //    if (!user.IsNull())
        //    {

        //        SetSessionForAlumniToken(user);//UAT-2960

        //        #region UAT-1053 Bkg Report Display
        //        bool needToShowEmploymentResultReport = HandleBkgReportsDisplay(user);
        //        #endregion

        //        #region If Need to redirect to ChangePassword

        //        if (user.UserName.Equals(UserName) && user.IsNewPassword)
        //        {
        //            //UAT-1850 :On Applicant PW reset, change the "Old Password" field to read "Temporary Password"
        //            Dictionary<String, String> queryString = null;
        //            queryString = new Dictionary<String, String>
        //                                                         {
        //                                                            {"IsTempPassReset","true"}
        //                                                         };
        //            Response.Redirect(String.Format("ChangePassword.aspx?args={0}", queryString.ToEncryptedQueryString()));
        //        }
        //        #endregion

        //        #region UAT-1218 Conditions for different types of logins and managing incorrect loin url
        //        HandleUserLoginType(user);
        //        #endregion

        //        #region UAT-1178 USER ATTESTATION DISCLOSURE AND UAT-1176 EMPLOYMENT NODE DISCLOSURE
        //        HandleDisclosureFormDisplay(user, needToShowEmploymentResultReport);
        //        #endregion

        //        ManageAlumniRedirection();//UAT-2960
        //    }
        //}

        ///// <summary>
        ///// Load the image of the login page based on the client site.
        ///// </summary>
        ///// <param name="sender">Current page</param>
        //private void ManageLoginPageImage(object sender)
        //{
        //    String baseImagePath = WebConfigurationManager.AppSettings[AppConsts.CLIENT_WEBSITE_IMAGES];
        //    WebSiteId = Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_ID));
        //    Presenter.GetWebsiteFooter();
        //    if (WebSiteId > 0)
        //        GenerateFooter();
        //    if (String.IsNullOrEmpty(LoginPageImageUrl))
        //    {
        //        Presenter.GetImageUrl();
        //        String imageURL = String.Empty;
        //        if (WebSiteId > 0)
        //            imageURL = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?WebsiteId={0}&DocumentType={1}", WebSiteId, "LoginImage");

        //        //AD:Commenting following line, now image will be shown in the image control
        //        //divLogin.Style.Add("background-image", String.Format("url('{0}')", imageURL));

        //        imgLogo.ImageUrl = imageURL;
        //    }
        //}

        ///// <summary>
        ///// Method to check and display Background report if user redurected from report pdf link from mail.
        ///// </summary>
        ///// <param name="user"></param>
        //private bool HandleBkgReportsDisplay(SysXMembershipUser user)
        //{
        //    Boolean showReport = false;
        //    Boolean permissionViolated = false;
        //    Dictionary<String, String> args = new Dictionary<String, String>();
        //    String queryStringForReportViewer = "";
        //    if (!Request.QueryString["args"].IsNull())
        //    {
        //        args.ToDecryptedQueryString(Request.QueryString["args"]);
        //        if (args.ContainsKey("ReportType"))
        //        {
        //            //If the report sent to the student and currently the same student logged in
        //            if (args.ContainsKey("IsReportSentToStudent")
        //                        && Convert.ToBoolean(args["IsReportSentToStudent"]) == true
        //                        && args.ContainsKey("OrganizationUserID"))
        //            {
        //                if (user.IsNotNull() && user.IsApplicant && user.OrganizationUserId == Convert.ToInt32(args["OrganizationUserID"]))
        //                    showReport = true;
        //                else
        //                    permissionViolated = true;
        //            }

        //            if (args.ContainsKey("IsReportSentToStudent")
        //                        && Convert.ToBoolean(args["IsReportSentToStudent"]) == false
        //                        && args.ContainsKey("HierarchyNodeID")
        //                        && Convert.ToInt32(args["HierarchyNodeID"]) == -1)
        //            {
        //                if (user.IsNotNull() && !user.IsApplicant && user.TenantId != AppConsts.SUPER_ADMIN_TENANT_ID && user.TenantTypeCode == TenantType.Institution.GetStringValue())
        //                    showReport = true;
        //                else
        //                    permissionViolated = true;
        //            }

        //            //If the report is sent to client admin and he has node permission
        //            if (args.ContainsKey("HierarchyNodeID") && Convert.ToInt32(args["HierarchyNodeID"]) > 0)
        //            {
        //                Boolean result = false;
        //                if (user.IsNotNull() && !user.IsApplicant && user.TenantId != AppConsts.SUPER_ADMIN_TENANT_ID && user.TenantTypeCode == TenantType.Institution.GetStringValue())
        //                    result = Presenter.HasNodePermission(user.TenantId.Value, user.OrganizationUserId, Convert.ToInt32(args["HierarchyNodeID"]));
        //                if (result)
        //                    showReport = true;
        //                else
        //                    permissionViolated = true;
        //            }

        //            //Report can be seen by super admin in all cases
        //            //UAT-1735:- Any shared user will not able to see report
        //            if (user.IsNotNull() && user.TenantId == AppConsts.SUPER_ADMIN_TENANT_ID && !user.IsSharedUser)
        //                showReport = true;
        //            else
        //                permissionViolated = true;
        //            queryStringForReportViewer = args.ToEncryptedQueryString();
        //        }
        //    }

        //    if (showReport)
        //    {
        //        SysXWebSiteUtils.SessionService.SetCustomData("CURRENT_URL", AppConsts.BKG_REPORT_VIEWER + "?args=" + queryStringForReportViewer);
        //    }
        //    else if (permissionViolated)
        //    {
        //        SysXWebSiteUtils.SessionService.SetCustomData("CURRENT_URL", AppConsts.BKG_REPORT_VIEWER + "?args=PermissionVoilated");
        //    }
        //    return showReport;
        //}

        ///// <summary>
        ///// Method to Check whether need to display Employment or User Attestation Disclosure forms.
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="numberOfBuisnessChannel"></param>
        //private void HandleDisclosureFormDisplay(SysXMembershipUser user, bool needToShowEmploymentResultReport)
        //{
        //    //If User is client admin and he has any background Feature OR User is shared user and he has recieved any bkg Order Invitation
        //    //then redirect to User Attestation Disclosure Form Page.

        //    String documentTypeCode;
        //    Int32 TenantId = user.TenantId.HasValue ? user.TenantId.Value : AppConsts.NONE;
        //    Int32 numberOfBuisnessChannel = this.Presenter.GetLineOfBusinessesByUser(Convert.ToString(user.UserId), TenantId);
        //    Boolean isAttestationDocumentAlreadySubmitted = Presenter.IsAttestationDocumentAlreadySubmitted(user.OrganizationUserId);
        //    var _userType = Presenter.GetUserType(user);
        //    Presenter.GetOrganizationUserTypeMapping(user.UserId);
        //    List<String> orgUserTypeCode = new List<String>();
        //    if (!CurrentViewContext.OrganizationUserTypeMapping.IsNullOrEmpty())
        //    {
        //        orgUserTypeCode = CurrentViewContext.OrganizationUserTypeMapping.Select(col => col.lkpOrgUserType.OrgUserTypeCode).ToList();
        //    }

        //    //Case 1: If User is Client Admin - then cheking User Attestation Disclosure conditions and Redirect to UAD
        //    if (_userType == UserType.CLIENTADMIN
        //        && CurrentViewContext.SiteUrl.ToLower() != CurrentViewContext.SharedUserLoginURL.ToLower()
        //        && !isAttestationDocumentAlreadySubmitted
        //        && Presenter.CheckForClientRoleFeatures(user.UserId))
        //    {
        //        documentTypeCode = DislkpDocumentType.USER_ATTESTATION_DISCLOSURE_FORM_CLIENT_ADMIN.GetStringValue();
        //        RedirectToDisclosurePage(documentTypeCode, user.TenantId.Value, user.OrganizationUserId, needToShowEmploymentResultReport);
        //    }

        //    //Case 2: If User is Shared User - then cheking User Attestation Disclosure conditions and Redirect to UAD
        //    else if (!orgUserTypeCode.IsNullOrEmpty()
        //        && CurrentViewContext.SiteUrl.ToLower() == CurrentViewContext.SharedUserLoginURL.ToLower()
        //        && !isAttestationDocumentAlreadySubmitted && System.Web.HttpContext.Current.Session["AgencyViewAdminOrgUsrID"].IsNullOrEmpty()) //UAT 1620:Commented Code : [&& Presenter.CheckForBkgInvitation(user.OrganizationUserId)]
        //    {
        //        documentTypeCode = DislkpDocumentType.USER_ATTESTATION_DISCLOSURE_FORM_SHARED_USER.GetStringValue();
        //        RedirectToDisclosurePage(documentTypeCode, user.TenantId.Value, user.OrganizationUserId, needToShowEmploymentResultReport);
        //    }

        //    //Case 3: If User is Shared User - then cheking Employment Disclosure conditions and Redirect to ED
        //    //UAT-1176 To check if user is client admin who has access to any employment node 
        //    //then redirect him to EmploymentDisclosure Form page.
        //    else if (_userType == UserType.CLIENTADMIN
        //        && needToShowEmploymentResultReport
        //        && CurrentViewContext.SiteUrl.ToLower() != CurrentViewContext.SharedUserLoginURL.ToLower()
        //        && Presenter.CheckEmploymentNodePermission(user.TenantId.Value, user.OrganizationUserId)
        //        && !Presenter.IsEDFormPreviouslyAccepted(user.OrganizationUserId))
        //    {
        //        documentTypeCode = DislkpDocumentType.EMPLOYMENT_DISCLOSURE_FORM.GetStringValue();
        //        RedirectToDisclosurePage(documentTypeCode, user.TenantId.Value, user.OrganizationUserId, needToShowEmploymentResultReport);
        //    }

        //    //Case 4: TO BE CONFIRM
        //    /// By not adding this as the 'if-else if' condition above,
        //    /// it will make sure that when the Client admin logins from Correct url with Single tenant account, with multiple 
        //    /// bussiness channels, he is redirect to the 'Select Business Channel' screen
        //    else if (_userType != UserType.APPLICANT
        //        && CurrentViewContext.SiteUrl.ToLower() != CurrentViewContext.SharedUserLoginURL.ToLower()
        //        && numberOfBuisnessChannel > 1)
        //        Response.Redirect(AppConsts.SELECT_BUSINESS_CHANNEL_URL);
        //}

        ///// <summary>
        ///// Method to Handle User logins for different types of logins like applicant, Client Admin, ADB Admins 
        ///// Shared User, Instructor, Precptor etc.
        ///// </summary>
        ///// <param name="user"></param>
        //private void HandleUserLoginType(SysXMembershipUser user)
        //{
        //    var _isIncorrectUrl = CheckIncorrectUrl();
        //    var _userType = Presenter.GetUserType(user);

        //    List<String> orgUserTypeCode = new List<String>();
        //    if (!CurrentViewContext.OrganizationUserTypeMapping.IsNullOrEmpty())
        //    {
        //        orgUserTypeCode = CurrentViewContext.OrganizationUserTypeMapping.Select(col => col.lkpOrgUserType.OrgUserTypeCode).ToList();
        //    }

        //    HandleAgencyVerification(CurrentViewContext.UserName.ToLower());
        //    if (this.CheckWebsiteURL)
        //    {
        //        //Case 1:  If User is ONLY ONE of the Applicant or Client Admin
        //        if (orgUserTypeCode.Count == AppConsts.NONE
        //            && (_userType == UserType.APPLICANT || _userType == UserType.CLIENTADMIN))
        //        {
        //            var _isMultiTenantUser = this.Presenter.IsMultiTenantUser(user.UserId);
        //            if (_isMultiTenantUser && this.IsIncorrectLoginUrl)
        //            {
        //                // This will be added to the 'WebApplicationData' object from the SelectTenant.aspx screen 
        //                user.IncorrectLoginUrlUsed = _isIncorrectUrl;
        //                Response.Redirect(AppConsts.SELECT_TENANT_URL);
        //            }
        //            else if (!_isMultiTenantUser && this.IsIncorrectLoginUrl && !SiteUrl.Contains(AppConsts.LOCAL_HOST))
        //                ManageIncorrectUrlLogin(user.UserId, Convert.ToInt32(user.TenantId), _isIncorrectUrl, false);
        //        }

        //        //Case 2:  If User is ONLY ONE of the Super Admin, ADBAdmin, or Third Party   
        //        else if (this.IsIncorrectLoginUrl
        //            && orgUserTypeCode.Count == AppConsts.NONE
        //            && (_userType == UserType.SUPERADMIN || _userType == UserType.THIRDPARTYADMIN))
        //        {
        //            ManageIncorrectUrlLogin(user.UserId, Convert.ToInt32(user.TenantId), _isIncorrectUrl, false);
        //        }

        //        //Case 3:  If User is ONLY ONE of the Applicants Shared User, Agency User, Instructor or Preceptor 
        //        else if (!orgUserTypeCode.IsNullOrEmpty()
        //            && orgUserTypeCode.Count > AppConsts.NONE
        //            && !CurrentViewContext.IsSharedUserHasOtherRoles)
        //        {
        //            if (this.IsIncorrectLoginUrl && !SiteUrl.Contains(AppConsts.LOCAL_HOST))
        //                ManageIncorrectUrlLogin(user.UserId, AppConsts.SHARED_USER_TENANT_ID, _isIncorrectUrl, true);
        //        }

        //        //Case 4:  If User is ONE of the Applicants Shared User, Agency User, Instructor or Preceptor + He also have Other Roles
        //        else if (!orgUserTypeCode.IsNullOrEmpty()
        //            && orgUserTypeCode.Count > AppConsts.NONE && CurrentViewContext.IsSharedUserHasOtherRoles)
        //        {
        //            var _isMultiTenantUser = this.Presenter.IsMultiTenantUser(user.UserId);
        //            if (CurrentViewContext.SiteUrl.ToLower() != CurrentViewContext.SharedUserLoginURL.ToLower())
        //            {
        //                if (_isMultiTenantUser && this.IsIncorrectLoginUrl)
        //                {
        //                    // This will be added to the 'WebApplicationData' object from the SelectTenant.aspx screen 
        //                    user.IncorrectLoginUrlUsed = _isIncorrectUrl;
        //                    Response.Redirect(AppConsts.SELECT_TENANT_URL);
        //                }
        //                else if (!_isMultiTenantUser && this.IsIncorrectLoginUrl && !SiteUrl.Contains(AppConsts.LOCAL_HOST))
        //                    ManageIncorrectUrlLogin(user.UserId, Convert.ToInt32(user.TenantId), _isIncorrectUrl, false);
        //            }
        //        }
        //    }
        //}

        //private void HandleAgencyVerification(String userName)
        //{
        //    Entity.OrganizationUser orgUser = new Entity.OrganizationUser();
        //    Dictionary<String, String> profileArgs = new Dictionary<String, String>();
        //    if (SiteUrl.ToLower() == CurrentViewContext.SharedUserLoginURL.ToLower() && !Request.QueryString["args"].IsNull())
        //    {
        //        profileArgs.ToDecryptedQueryString(Request.QueryString["args"]);
        //        if (profileArgs.ContainsKey(AppConsts.QUERY_STRING_AGENCY_USER_ID))
        //        {
        //            orgUser = Presenter.IsSharedUserExists(Guid.Empty, false, Convert.ToInt32(profileArgs[AppConsts.QUERY_STRING_AGENCY_USER_ID]));
        //        }
        //        if (profileArgs.ContainsKey(AppConsts.PROFILE_SHARING_URL_TYPE)
        //               && profileArgs[AppConsts.PROFILE_SHARING_URL_TYPE].Equals(AppConsts.PROFILE_SHARING_URL_TYPE_AGENCY_VERIFICATION) && !orgUser.IsNullOrEmpty()
        //               && profileArgs.ContainsKey(AppConsts.QUERY_STRING_USER_TYPE_CODE)
        //            && Convert.ToString(profileArgs[AppConsts.QUERY_STRING_USER_TYPE_CODE]) == OrganizationUserType.AgencyUser.GetStringValue()
        //          && orgUser.aspnet_Users.UserName.ToLower().Equals(userName.ToLower()))
        //        {
        //            //Code to update newly mapped agencies to IsVerified=1 (for which verification code has been sent in agruments)
        //            Boolean isAgencyVerified = Presenter.UpdateAgencyUserAgenciesVerificationCode(Convert.ToInt32(profileArgs[AppConsts.QUERY_STRING_AGENCY_USER_ID])
        //                  , Convert.ToString(profileArgs[AppConsts.PROFILE_SHARING_URL_VERIFICATION_TOKEN]), orgUser);
        //            if (isAgencyVerified)
        //            {
        //                Session[AppConsts.SESSION_KEY_ISAGENCY_MAPPED] = "true";
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Returns whether the user is trying to login from incorrect url, 
        ///// based on TenantId from DB (by IsIncorrectLoginUrl property) and 
        ///// if it is Not Central Login type
        ///// </summary>
        ///// <returns></returns>
        //private Boolean CheckIncorrectUrl()
        //{
        //    var _isIncorrectUrl = false;

        //    // Check of 'this.IsIncorrectLoginUrl' is required, 
        //    // otherwise, even if the user is using corect Tenant Url,
        //    // removing this check will make it an incorrect login url case
        //    if (this.IsIncorrectLoginUrl && !this.IsCentralLogin() && !SiteUrl.Contains(AppConsts.LOCAL_HOST))// && !this.IsSharedUserLogin())
        //        _isIncorrectUrl = true;
        //    return _isIncorrectUrl;
        //}

        ///// <summary>
        ///// This method sets the validation for all the control.
        ///// </summary>
        ///// <param name="validatorId"> The id of validation control.</param>
        ///// <param name="errorMessage">The error message need to be set.</param>
        //private void SetValidations(String validatorId, String errorMessage)
        //{
        //    BaseValidator baseValidator = (BaseValidator)FindControl(validatorId);
        //    baseValidator.ErrorMessage = SysXUtils.GetMessage(errorMessage);
        //}

        ///// <summary>
        ///// Method to validate user by Email Address
        ///// </summary>
        //private void ValidateUserViaEmail()
        //{
        //    if (Request.QueryString["UsrVerCode"] != null)
        //    {
        //        if (!String.IsNullOrEmpty(Request.QueryString["UsrVerCode"]))
        //        {
        //            try
        //            {
        //                Presenter.ValidateUserViaEmailAndRedirect(Request.QueryString["UsrVerCode"].ToString());
        //            }
        //            catch (Exception ex)
        //            {
        //                ErrorMessage = ex.Message;
        //            }
        //        }
        //    }

        //}

        ///// <summary>
        ///// Method to Validate EmailAddress By email
        ///// </summary>
        //private void ValidateEmailAddressViaEmail()
        //{
        //    if (Request.QueryString["AuthReqVerCode"] != null)
        //    {
        //        if (!String.IsNullOrEmpty(Request.QueryString["AuthReqVerCode"]))
        //        {
        //            try
        //            {
        //                Presenter.ValidateEmailAddressViaEmail(Request.QueryString["AuthReqVerCode"].ToString());
        //            }

        //            catch (Exception ex)
        //            {
        //                ErrorMessage = ex.Message;
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// To validate user and auto login
        ///// </summary>
        //private void ValidateUserAndAutoLogin(Boolean needToShowEmploymentResultReport)
        //{
        //    if (Request.QueryString["TokenKey"] != null)
        //    {
        //        if (!String.IsNullOrEmpty(Request.QueryString["TokenKey"]))
        //        {
        //            try
        //            {
        //                Presenter.ValidateUserAndAutoLogin(Request.QueryString["TokenKey"].ToString());
        //                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
        //                // Set the Value in Session, to manage the display of error message
        //                if (!user.IsNull())
        //                {
        //                    user.IncorrectLoginUrlUsed = this.IsIncorrectLoginUrl;
        //                    if (user.UserName.Equals(UserName) && user.IsNewPassword)
        //                    {
        //                        Response.Redirect("ChangePassword.aspx");
        //                    }

        //                    HandleDisclosureFormDisplay(user, needToShowEmploymentResultReport);
        //                    ManageAlumniRedirection();//UAT-2960
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                ErrorMessage = ex.Message;
        //            }
        //        }
        //    }
        //}

        ////UAT-1176 and UAT-1178
        ///// <summary>
        ///// Method to Redirect to User Attestation or Employment Disclosure Page for Client admins and shared users
        ///// </summary>
        ///// <param name="documentType"></param>
        ///// <param name="tenantID"></param>
        ///// <param name="organizationUserID"></param>
        //private void RedirectToDisclosurePage(String documentTypeCode, Int32 tenantID, Int32 organizationUserID, bool needToShowEmploymentResultReport)
        //{
        //    //Check if User Attestation Doc already submitted
        //    Dictionary<String, String> queryString = new Dictionary<String, String>();
        //    queryString = new Dictionary<String, String>
        //                                                         {
        //                                                            {"DocumentTypeCode", documentTypeCode},
        //                                                            {"TenantID", Convert.ToString(tenantID)},
        //                                                            {"OrganizationUserID", Convert.ToString(organizationUserID)},
        //                                                            {"NeedToShowEmploymentResultReport", Convert.ToString(needToShowEmploymentResultReport)}
        //                                                         };
        //    String url = String.Empty;
        //    if (documentTypeCode == DislkpDocumentType.EMPLOYMENT_DISCLOSURE_FORM.GetStringValue())
        //    {
        //        url = String.Format(AppConsts.EMPLOYMENT_DISCLOSURE + "?args={0}", queryString.ToEncryptedQueryString());
        //    }
        //    else if (documentTypeCode == DislkpDocumentType.USER_ATTESTATION_DISCLOSURE_FORM_CLIENT_ADMIN.GetStringValue() ||
        //        documentTypeCode == DislkpDocumentType.USER_ATTESTATION_DISCLOSURE_FORM_SHARED_USER.GetStringValue())
        //    {
        //        url = String.Format(AppConsts.USER_ATTESTATION_DISCLOSURE_PAGE + "?args={0}", queryString.ToEncryptedQueryString());
        //    }
        //    Response.Redirect(url);
        //}

        ///// <summary>
        ///// Method to generate Footer
        ///// </summary>
        //private void GenerateFooter()
        //{
        //    StringBuilder sbFooter = new StringBuilder();
        //    Int32 count = 0;
        //    foreach (var page in lstWebsitePages)
        //    {
        //        if (page.LinkPosition == Convert.ToInt32(CustomPageLinkPosition.Footer))
        //        {
        //            Dictionary<String, String> queryString = new Dictionary<String, String>();
        //            String _viewType = String.Empty;
        //            queryString = new Dictionary<String, String>
        //                                                         { 
        //                                                            //{ "Child", @"UserControl/CustomPageContent.ascx"},
        //                                                            {"PageId",Convert.ToString( page.WebSiteWebPageID)},
        //                                                            {"PageTitle",page.LinkText}

        //                                                         };
        //            //String url = String.Format("Website/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
        //            String url = String.Format("CustomContentPage.aspx?args={0}", queryString.ToEncryptedQueryString());
        //            sbFooter = count == 0 ? sbFooter.Append("&nbsp;&nbsp;") : sbFooter.Append("|&nbsp;&nbsp;");
        //            sbFooter.Append("<a href=" + url + ">" + page.LinkText + "</a>&nbsp;&nbsp;");
        //            count++;
        //        }
        //    }
        //    //

        //    litFooter.Text = litFooter.Text + "&nbsp;&nbsp;" + Convert.ToString(sbFooter);
        //}

        ///// <summary>
        ///// Change the UI settings, as per whether the screen is opened as a central url.
        ///// </summary>
        //private Boolean IsCentralLogin()
        //{
        //    var _centralLoginUrl = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_CENTRAL_LOGIN_URL]);

        //    String _centralHost = _centralLoginUrl;
        //    if (_centralLoginUrl.Contains("http"))
        //    {
        //        Uri _url = new Uri(_centralLoginUrl);
        //        _centralHost = _url.Host;
        //    }

        //    if (_centralLoginUrl.IsNullOrEmpty()
        //                ||
        //         (!_centralLoginUrl.IsNullOrEmpty() && !CurrentViewContext.SiteUrl.ToLower().Trim().Contains(_centralHost.ToLower().Trim()))
        //       )
        //        return false;
        //    else
        //        return true;
        //}

        ///// <summary>
        ///// Method to Check whether the user tries to login with shared user login URL
        ///// </summary>
        ///// <returns></returns>
        //private Boolean IsSharedUserLogin()
        //{
        //    var _sharedLoginUrl = CurrentViewContext.SharedUserLoginURL.ToLower().Trim();

        //    String _sharedLoginHost = _sharedLoginUrl;
        //    if (_sharedLoginUrl.Contains("http"))
        //    {
        //        Uri _url = new Uri(_sharedLoginUrl);
        //        _sharedLoginHost = _url.Host;
        //    }

        //    if (_sharedLoginUrl.IsNullOrEmpty()
        //                ||
        //         (!_sharedLoginUrl.IsNullOrEmpty() && !CurrentViewContext.SiteUrl.ToLower().Trim().Contains(_sharedLoginHost.ToLower().Trim()))
        //       )
        //        return false;
        //    else
        //        return true;
        //}C:\Jagjit\Projects\CBI\Src\AppSlns\WebSites\CoreWeb\Login\CommonLogin.ascx

        ///// <summary>
        ///// Show hide the normal registration div and the Central registartion div, alternately
        ///// </summary>
        ///// <param name="isCentralLoginVisible"></param>
        //private void ShowCentralLoginDiv(Boolean isCentralLoginVisible, Boolean hideBothDivs = false)
        //{
        //    if (hideBothDivs)
        //    {
        //        divCentralCreateAccount.Visible = false;
        //        dvCreateAccount.Visible = false;
        //    }
        //    else
        //    {
        //        divCentralCreateAccount.Visible = isCentralLoginVisible;
        //        dvCreateAccount.Visible = !isCentralLoginVisible;
        //    }
        //}

        ///// <summary>
        ///// Manage the code to Redriect the applicant to appropriate url, if it was single tenant user
        ///// and is using incorrect url to login, including the central login screen
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="tenantId"></param>
        //private void ManageIncorrectUrlLogin(Guid userId, Int32 tenantId, Boolean isIncorrectUrl, Boolean isSharedUser = false)
        //{
        //    if (!isSharedUser)
        //    {
        //        var _targetInstURL = Presenter.GetInstitutionUrl(Convert.ToInt32(tenantId));
        //        String key = Guid.NewGuid().ToString();
        //        Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
        //        ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
        //        appInstData.UserID = Convert.ToString(userId);
        //        appInstData.TagetInstURL = _targetInstURL;
        //        appInstData.TokenCreatedTime = DateTime.Now;
        //        appInstData.TenantID = Convert.ToInt32(tenantId);
        //        appInstData.IsIncorrectLogin = isIncorrectUrl;

        //        Dictionary<String, ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
        //        if (applicationData != null)
        //        {
        //            applicantData = applicationData;
        //            applicantData.Add(key, appInstData);
        //            Presenter.UpdateWebApplicationData("ApplicantInstData", applicantData);
        //        }
        //        else
        //        {
        //            applicantData.Add(key, appInstData);
        //            Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
        //        }

        //        SysXWebSiteUtils.SessionService.ClearSession(true);
        //        FormsAuthentication.SignOut();
        //        SysXAppDBEntities.ClearContext();

        //        //Redirect to login page for Auto-Login 
        //        Dictionary<String, String> queryString = new Dictionary<String, String>
        //                                                         {
        //                                                            { "TokenKey", key  }
        //                                                         };
        //        Response.Redirect(String.Format(_targetInstURL + "/Login.aspx?TokenKey={0}", key));
        //    }
        //    else if (isSharedUser)// Shared User
        //    {
        //        String key = Guid.NewGuid().ToString();
        //        Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
        //        ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
        //        appInstData.UserID = Convert.ToString(userId);
        //        appInstData.TokenCreatedTime = DateTime.Now;
        //        appInstData.IsIncorrectLogin = isIncorrectUrl;
        //        appInstData.IsSharedUser = true;
        //        appInstData.TenantID = AppConsts.SHARED_USER_TENANT_ID;
        //        Dictionary<String, ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
        //        if (applicationData != null)
        //        {
        //            applicantData = applicationData;
        //            applicantData.Add(key, appInstData);
        //            Presenter.UpdateWebApplicationData("ApplicantInstData", applicantData);
        //        }
        //        else
        //        {
        //            applicantData.Add(key, appInstData);
        //            Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
        //        }

        //        SysXWebSiteUtils.SessionService.ClearSession(true);
        //        FormsAuthentication.SignOut();
        //        SysXAppDBEntities.ClearContext();

        //        //Redirect to login page for Auto-Login 
        //        Dictionary<String, String> queryString = new Dictionary<String, String>
        //                                                         {
        //                                                            { "TokenKey", key  }
        //                                                         };
        //        Response.Redirect(String.Format("http://" + SharedUserLoginURL + "/Login.aspx?TokenKey={0}", key));
        //    }
        //}

        ///// <summary>
        ///// UAT-2960
        ///// </summary>
        //private void ManageAlumniRedirection()
        //{
        //    Int32 alumniTenantId = Presenter.GetAlumniTenantId();
        //    if (alumniTenantId > AppConsts.NONE)
        //    {
        //        SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
        //        if (!user.IsNullOrEmpty() && user.IsApplicant)
        //        {
        //            Int32 tenantId = user.TenantId.IsNullOrEmpty() ? AppConsts.NONE : user.TenantId.Value;
        //            if (alumniTenantId == tenantId)
        //            {
        //                SysXWebSiteUtils.AllClientSessionService.IsAlumniRedirectionDue = true;
        //            }
        //            Boolean IsAlumniRedirectionDue = (Boolean)SysXWebSiteUtils.AllClientSessionService.IsAlumniRedirectionDue;

        //            if (alumniTenantId > AppConsts.NONE && !IsAlumniRedirectionDue)
        //            {
        //                if (alumniTenantId != tenantId)
        //                {
        //                    if (Presenter.IsAlumniAcessActivated(user.OrganizationUserId, tenantId))
        //                    {
        //                        //Session["RedirectToAlumni"] = Convert.ToString("Yes");
        //                        SysXWebSiteUtils.AllClientSessionService.IsAlumniRedirectionDue = true;

        //                        Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
        //                        ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
        //                        appInstData.UserID = Convert.ToString(user.UserId);
        //                        appInstData.TagetInstURL = Presenter.GetInstitutionUrl(alumniTenantId);
        //                        appInstData.TokenCreatedTime = DateTime.Now;
        //                        appInstData.TenantID = alumniTenantId;

        //                        //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        //                        if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
        //                        {
        //                            appInstData.AdminOrgUserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
        //                        }
        //                        String key = Guid.NewGuid().ToString();

        //                        Dictionary<String, ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
        //                        if (applicationData != null)
        //                        {
        //                            applicantData = applicationData;
        //                            applicantData.Add(key, appInstData);
        //                            Presenter.UpdateWebApplicationData("ApplicantInstData", applicantData);
        //                        }
        //                        else
        //                        {
        //                            applicantData.Add(key, appInstData);
        //                            Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
        //                        }
        //                        SysXWebSiteUtils.SessionService.ClearSession(true);
        //                        FormsAuthentication.SignOut();
        //                        SysXAppDBEntities.ClearContext();

        //                        //Redirect to login page for Auto-Login 
        //                        Dictionary<String, String> queryString = new Dictionary<String, String>
        //                                                         {
        //                                                            { "TokenKey", key  }
        //                                                         };
        //                        Response.Redirect(String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}", key));
        //                        //Response.Redirect(String.Format("http://" + SharedUserLoginURL + "/Login.aspx?TokenKey={0}", key));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}


        //#endregion

        //#endregion

        private void SetDeviceType()
        {
            Boolean isMobileDevice = ((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).IsMobileDevice;
            SysXWebSiteUtils.SessionService.SetCustomData("IsMobileDevice", isMobileDevice);

        }

        #region Admin Entry Portal

        private void SetUserSessionAndRedirect()
        {
            String targetUrl = String.Empty;
            //This method will sets the sessions for user and redirect.
            //if (!Request.QueryString["args"].IsNullOrEmpty())
            if (!Request.QueryString["RMTokenKey"].IsNullOrEmpty())
            {
                targetUrl = SessionSharingManagement.GetTargetUrl(Request.QueryString);

                #region Admin Entry Portal
                SysXWebSiteUtils.SessionService.SetCustomData("IsReactToMVPRedirection", true);
                #endregion

                if (!String.IsNullOrEmpty(targetUrl))
                {
                    ManageTargetUrlRedirection(targetUrl);
                }
            }
        }

        private void ManageTargetUrlRedirection(String targetUrl)
        {
            //This method will be used to redirect to the target Url.
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            if (!user.IsNullOrEmpty())
            {
                FormsAuthentication.RedirectFromLoginPage(user.UserName, false);
                Response.Redirect(targetUrl);
            }
        }

        #region Applicant invitation Order Process

        private void VerifyTokenAndRedirect()
        {
            Dictionary<String, String> invitationArgs = new Dictionary<String, String>();

            Boolean ApplicantInvitationInProgress = !Session["ApplicantInvitationInProgress"].IsNullOrEmpty() ? Convert.ToBoolean(Session["ApplicantInvitationInProgress"]) : false;

            if (!ApplicantInvitationInProgress)
            {
                if (!Request.QueryString["args"].IsNullOrEmpty())
                {
                    invitationArgs.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (invitationArgs.ContainsKey("InvitationToken") && !String.IsNullOrEmpty(invitationArgs["InvitationToken"]))
                    {
                        String invitationToken = invitationArgs["InvitationToken"];

                        //Check here if token is active or not.
                        if (Presenter.IsApplicantInvitationTokenActive(invitationToken))
                        {
                            Response.Redirect("~/AdminEntryPortal/Default.aspx");
                        }

                    }
                }
            }

            if (ApplicantInvitationInProgress)
            {
 
            }


            //Step 1: Get the token from query string.
            //Step 2: Check if token is active, if true then redirect to applicant invite landing page.
        }

        #endregion

        #endregion
    }
}