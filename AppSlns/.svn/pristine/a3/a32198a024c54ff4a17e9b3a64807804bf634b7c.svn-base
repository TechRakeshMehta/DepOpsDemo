#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  LoginCenterPanel.ascx.cs
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
    public partial class LoginCenterPanel : BaseUserControl, ILoginCenterPanelView
    {
        #region Variables

        #region Private Variables

        private LoginCenterPanelPresenter _presenter = new LoginCenterPanelPresenter();

        #endregion

        #region Public Variables

        //public delegate void ManageControlsOnSubmitClick();
        //public event ManageControlsOnSubmitClick eventManageControlsOnSubmitClick;

        public delegate void ExceptionMessage(String exceptionMessage);
        public event ExceptionMessage eventExceptionMessage;

        public delegate void ShowCentralLoginDiv(Boolean isCentralLoginVisible, Boolean hideBothDivs = false);
        public event ShowCentralLoginDiv eventShowCentralLoginDiv;

        public delegate void HandleVerificationMessage(String verificationMessage);
        public event HandleVerificationMessage eventVerificationMessage;

        public delegate void IsAccountInActive(Boolean isAccountInActive);
        public event IsAccountInActive eventIsAccountInActive;

        public delegate void ErrorMessageExtended();
        public event ErrorMessageExtended eventErrorMessageExtended;

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        /// <summary>
        /// Presenter</summary>
        /// <value>
        /// Represents Common Login Presenter.</value>

        public LoginCenterPanelPresenter Presenter
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

        public ILoginCenterPanelView CurrentViewContext
        {
            get
            {
                return this;
            }

        }

        public String UserName
        {
            get
            {
                return txtUserName.Text;
            }
            set
            {
                txtUserName.Text = value;
            }
        }

        public String Password
        {
            get
            {
                return txtPassword.Text;
            }
        }
        public Int32 StoreExternalLoginTenantId
        {
            get
            {
                if (!ViewState["StoreExternalLoginTenantId"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["StoreExternalLoginTenantId"]);
                return 0;
            }
            set
            {
                ViewState["StoreExternalLoginTenantId"] = value;
            }
        }
        public Boolean IsRequestExteranlLogin
        {
            get
            {
                if (!ViewState["IsRequestExteranlLogin"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsRequestExteranlLogin"]);
                return false;
            }
            set
            {
                ViewState["IsRequestExteranlLogin"] = value;
            }
        }

        public IPersistViewState ViewStateProvider
        {
            get
            {
                if (ConfigurationManager.AppSettings[SysXCachingConst.CUSTOMVIEWSTATEPROVIDER].IsNotNull())
                {
                    switch (ConfigurationManager.AppSettings[SysXCachingConst.CUSTOMVIEWSTATEPROVIDER].ToString().ToUpper())
                    {
                        case "REDIS":
                            {
                                return new RedisPersistViewStateProvider();
                            }
                        case "SQL":
                            {
                                return new SysXPersistViewStateProvider();
                            }
                    }
                }
                return new SysXPersistViewStateProvider();
            }
        }

        #region New Region

        public Boolean IsRedirectToMobileSite
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains(INTSOF.Utils.Consts.SysXSecurityConst.REDIRECT_TO_MOBILE_SITE))
                {
                    Boolean IsRedirectToMobileSite = Convert.ToBoolean(!String.IsNullOrEmpty(ConfigurationManager.AppSettings[SysXSecurityConst.REDIRECT_TO_MOBILE_SITE]) ? ConfigurationManager.AppSettings[SysXSecurityConst.REDIRECT_TO_MOBILE_SITE] : "false");
                    if (IsRedirectToMobileSite && ConfigurationManager.AppSettings.AllKeys.Contains(INTSOF.Utils.Consts.SysXSecurityConst.MOBILE_DEVICE_AUTO_REDIRECT_SITES))
                    {
                        String sitenames = ConfigurationManager.AppSettings[SysXSecurityConst.MOBILE_DEVICE_AUTO_REDIRECT_SITES];
                        String absoluteHost = new Uri(HttpContext.Current.Request.Url.AbsoluteUri).GetLeftPart(UriPartial.Authority);
                        String[] Sites = sitenames.Split(',');
                        if (Sites.IsNotNull() && Sites.Length > 0)
                        {
                            if (Sites.Any(x => absoluteHost.Contains(x)))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }


                    }
                    return IsRedirectToMobileSite;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// Gets the current user id.
        /// </summary>
        /// <remarks></remarks>
        public String CurrentSessionId
        {
            get
            {
                return Page.Session.SessionID.IsNullOrEmpty() ? String.Empty : Page.Session.SessionID;
            }
        }

        public String SiteUrl
        {
            get;
            set;
        }

        #region UAT-1110 - Profiel Sharing
        public String SharedUserLoginURL
        {
            get
            {
                return Convert.ToString(ViewState["SharedUserLoginURL"]);
            }
            set
            {
                ViewState["SharedUserLoginURL"] = value;
            }
        }

        #endregion

        public Boolean needToShowEmploymentResultReport
        {
            get
            {
                if (!(ViewState["needToShowEmploymentResultReport"]).IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["needToShowEmploymentResultReport"]);
                return false;
            }
            set
            {
                ViewState["needToShowEmploymentResultReport"] = value;
            }
        }

        /// <summary>
        /// Maintains the condition, whether the Tenant Url eneterd by appluicant exists in the database
        /// </summary>
        private Boolean InstUrlExists
        {
            get;
            set;
        }

        public Boolean CheckWebsiteURL
        {
            get
            {
                if (ConfigurationManager.AppSettings[SysXSecurityConst.CHECK_WEBSITE_URL_LOGIN].IsNull())
                    return true;
                String chkWebsite = ConfigurationManager.AppSettings[SysXSecurityConst.CHECK_WEBSITE_URL_LOGIN];
                return Convert.ToBoolean(chkWebsite);
            }
        }

        #region UAT-1218 CRM Login Operations
        List<OrganizationUserTypeMapping> ILoginCenterPanelView.OrganizationUserTypeMapping
        {
            get
            {
                if (!ViewState["OrganizationUserTypeMapping"].IsNullOrEmpty())
                {
                    return ViewState["OrganizationUserTypeMapping"] as List<OrganizationUserTypeMapping>;
                }
                return new List<OrganizationUserTypeMapping>();
            }
            set
            {
                ViewState["OrganizationUserTypeMapping"] = value;
            }

        }

        Boolean ILoginCenterPanelView.IsSharedUserHasOtherRoles { get; set; }

        #endregion

        /// <summary>
        /// NOTE - WILL BE OVER-RIDEN, WHEN THE APPLICANT IS VALIDATED FOR AUTOLOGIN PROCESS
        /// Property to decide whether the User is from Correct Tenant Url or not. 
        /// Will be set TRUE even if it is Non-Central login type.
        /// </summary>
        public Boolean IsIncorrectLoginUrl
        {
            get;
            set;
        }

        public String ErrorMessage
        {
            set
            {
                eventExceptionMessage(Convert.ToString(value));
            }
        }

        /// <summary>
        /// SelectedBlockID</summary>
        /// <value>
        /// Gets the value for selected block's id.</value>
        public Int32 SelectedBlockId
        {
            get;
            set;
        }

        /// <summary>
        /// SelectedBlockName</summary>
        /// <value>
        /// Gets the value for selected block name.</value>
        public String SelectedBlockName
        {
            get;
            set;
        }
        public String VerificationMessage
        {
            set
            {
                eventVerificationMessage(Convert.ToString(value));
            }
        }


        #region UAT-2792 UCONN SSO Process

        public Boolean IsShibbolethLogin
        {
            get
            {
                if (!ViewState["IsShibbolethLogin"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsShibbolethLogin"]);
                }
                return false;
            }
            set
            {
                ViewState["IsShibbolethLogin"] = value;
            }
        }

        String ILoginCenterPanelView.ShibbolethUniqueIdentifier
        {
            get
            {
                if (!ViewState["ShibbolethUniqueIdentifier"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["ShibbolethUniqueIdentifier"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["ShibbolethUniqueIdentifier"] = value;
            }
        }

        Int32 ILoginCenterPanelView.IntegrationClientId
        {
            get
            {
                return Convert.ToInt32(ViewState["IntegrationClientId"]);
            }
            set
            {
                ViewState["IntegrationClientId"] = value;
            }
        }

        public Boolean IsAutoLoginThroughShibboleth { get; set; }

        Int32 ILoginCenterPanelView.ShibbolethHostID
        {
            get
            {
                return Convert.ToInt32(ViewState["ShibbolethHostID"]);
            }
            set
            {
                ViewState["ShibbolethHostID"] = value;
            }
        }

        String ILoginCenterPanelView.HostName
        {
            get
            {
                return Convert.ToString(ViewState["HostName"]);
            }
            set
            {
                ViewState["HostName"] = value;
            }
        }

        public Boolean IsExistingAccount
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsExistingAccount"]);
            }
            set
            {
                ViewState["IsExistingAccount"] = value;
            }
        }
        Boolean ILoginCenterPanelView.IsShibbolethApplicant
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsShibbolethApplicant"]);
            }
            set
            {
                ViewState["IsShibbolethApplicant"] = value;
            }
        }

        #endregion

        Boolean ILoginCenterPanelView.IsAccountInActive
        {
            set
            {
                eventIsAccountInActive(Convert.ToBoolean(value));
            }
        }

        public String EncPValue
        {
            get
            {
                if (ViewState["EncPValue"] != null)
                    return Convert.ToString(ViewState["EncPValue"]);
                return
                   String.Empty;
            }
            set
            {
                if (ViewState["EncPValue"] == null)
                    ViewState["EncPValue"] = value;
            }
        }

        //UAT-2930
        Boolean ILoginCenterPanelView.IsTwoFactorAuthenticationRequired
        {
            get;
            set;
        }

        /// <summary>
        /// UAT-2494, New Account verification enhancements (additional verification step)
        /// </summary>
        public Boolean ShowAdditionalAccountVerificationPage
        {
            get
            {
                if (!ViewState["ShowAdditionalAccountVerificationPage"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["ShowAdditionalAccountVerificationPage"]);
                }
                return false;
            }
            set
            {
                ViewState["ShowAdditionalAccountVerificationPage"] = value;
            }
        }

        public string InvalidUsernamePswd
        {
            get
            {
                return Resources.Language.INCRTUSERNAMEPSWD;
            }
        }

        public string AccountLockedMessage
        {
            get
            {
                return Resources.Language.ACCOUNTLOCKED;
            }
        }
        
        #endregion

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!HttpContext.Current.Request.QueryString["aeait"].IsNullOrEmpty())
                    {
                        ManageHrAdminPortalUserLogin(HttpContext.Current.Request.QueryString["aeait"].ToString());
                    }
                    else
                    {
                        SysXWebSiteUtils.SessionService.SetCustomData("showMinDetails", false);
                    }
                    GetTenantId();
                }
                txtUserName.Focus();

            }
            catch (Exception ex)
            {
                eventExceptionMessage(ex.Message);
            }
        }

        private void GetTenantId()
        {
            Dictionary<String, String> invitationArgs = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNullOrEmpty())
            {
                invitationArgs.ToDecryptedQueryString(Request.QueryString["args"]);
                if ( (invitationArgs.ContainsKey("IsRequestExternalPage") && !String.IsNullOrEmpty(invitationArgs["IsRequestExternalPage"]) &&  (invitationArgs.ContainsKey("ExternalTenantId") && !String.IsNullOrEmpty(invitationArgs["ExternalTenantId"]))))
                {
                    StoreExternalLoginTenantId = Convert.ToInt32(invitationArgs["ExternalTenantId"]);
                    IsRequestExteranlLogin=Convert.ToBoolean(invitationArgs["IsRequestExternalPage"]);
                }
            }
        }

        #endregion

        #endregion

        #region Button Events

        /// <summary>
        /// Handles the Click event of the btnSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ManageControlsOnSubmitClick();
            }
            catch (Exception ex)
            {
                eventExceptionMessage(ex.Message);
            }

        }

        #endregion

        public void ManageValidation()
        {
            //SetValidations("revUserName", ResourceConst.SECURITY_INVALID_CHARACTER);
            //SetValidations("revPassword", ResourceConst.SECURITY_INVALID_CHARACTER);

            //((RequiredFieldValidator)FindControl("rfvUserName")).ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ENTER_USERNAME);
            //((RequiredFieldValidator)FindControl("rfvPassword")).ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ENTER_PASSWORD);


            SetValidations("revUserName", Resources.Language.INVALIDCHARS);
            SetValidations("revPassword", Resources.Language.INVALIDCHARS);

            ((RequiredFieldValidator)FindControl("rfvUserName")).ErrorMessage = Resources.Language.ENTERUSERNAME;
            ((RequiredFieldValidator)FindControl("rfvPassword")).ErrorMessage = Resources.Language.ENTERPASSWORD;
        }

        /// <summary>
        /// This method sets the validation for all the control.
        /// </summary>
        /// <param name="validatorId"> The id of validation control.</param>
        /// <param name="errorMessage">The error message need to be set.</param>
        private void SetValidations(String validatorId, String errorMessage)
        {
            BaseValidator baseValidator = (BaseValidator)FindControl(validatorId);
            baseValidator.ErrorMessage = SysXUtils.GetMessage(errorMessage);
        }



        public void RedirectToMobileSite()
        {
            if (CurrentViewContext.IsRedirectToMobileSite && !IsPostBack)
            {
                if (((System.Web.Configuration.HttpCapabilitiesBase)(Request.Browser)).IsMobileDevice)
                {
                    //Code implemented for mobile app.
                    Boolean isReturnToDesktopSite = true;
                    if (!Request.QueryString["isReturnToDesktopSite"].IsNullOrEmpty())
                        isReturnToDesktopSite = false;
                    if (isReturnToDesktopSite)
                    {
                        string mobileUrl = new Uri(HttpContext.Current.Request.Url.AbsoluteUri).GetLeftPart(UriPartial.Authority).Replace(HttpContext.Current.Request.Url.Scheme + "://", "");
                        String Query = new Uri(HttpContext.Current.Request.Url.AbsoluteUri).Query;
                        #region UAT-3428
                        String mobileUrlPrefix = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_MOBILE_URL_PREFIX]);
                        #endregion
                        if (Query.IsNullOrEmpty())
                        {
                            Response.Redirect(HttpContext.Current.Request.Url.Scheme + "://" + mobileUrlPrefix + mobileUrl);
                        }
                        else
                        {
                            if (Query.Contains("UsrVerCode"))
                            {
                                Query = "#/login" + Query;
                            }
                            Response.Redirect(HttpContext.Current.Request.Url.Scheme + "://" + mobileUrlPrefix + mobileUrl + "/" + Query);
                        }

                    }
                }
            }
        }

        public void AdminAsStudent()
        {
            //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
            if (!Request.QueryString["DeletePrevUsrState"].IsNullOrEmpty() && Convert.ToBoolean(Request.QueryString["DeletePrevUsrState"]))
            {
                Presenter.DoLogOff(true);
            }
            if (!Request.QueryString["ChangePassword"].IsNull())
            {
                String strClientScript = "<script type=\"text/javascript\"> if (top !== self) { top.location = " + "'" + Request.Url.ToString() + "'" + "; } </" + "script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", strClientScript);
            }
            if (!Request.QueryString["IsRedirectedFromOtherAccountLinking"].IsNull())
            {
                String strClientScript = "<script type=\"text/javascript\"> if (top !== self) { top.location = " + "'" + Request.Url.ToString() + "'" + "; } </" + "script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", strClientScript);
            }
            else
            {
                StringBuilder redirectURL = new StringBuilder(Uri.UriSchemeHttp.ToString() + Uri.SchemeDelimiter.ToString() + Context.Request.Url.Authority + Context.Request.Url.Segments[0].ToString() + Context.Request.Url.Segments[1].ToString());
                String strClientScript = "<script type=\"text/javascript\"> if (top !== self) { top.location = " + "'" + redirectURL + "'" + "; } </" + "script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", strClientScript);
            }
        }

        public void GetSiteURL()
        {
            SiteUrl = Page.Request.ServerVariables.Get("server_name");
            CurrentViewContext.SharedUserLoginURL = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);
        }

        public void ManageUser(SysXMembershipUser user)
        {

            CurrentViewContext.needToShowEmploymentResultReport = false;
            if (!user.IsNull())
            {
                #region UAT-1053
                Boolean showReport = false;
                Boolean permissionViolated = false;
                Dictionary<String, String> args = new Dictionary<String, String>();
                String queryStringForReportViewer = "";
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey("ReportType"))
                    {
                        //If the report sent to the student and currently the same student logged in
                        if (args.ContainsKey("IsReportSentToStudent") && Convert.ToBoolean(args["IsReportSentToStudent"]) == true && args.ContainsKey("OrganizationUserID"))
                        {
                            if (user.IsNotNull() && user.IsApplicant && user.OrganizationUserId == Convert.ToInt32(args["OrganizationUserID"]))
                            {
                                showReport = true;
                            }
                            else
                            {
                                permissionViolated = true;
                            }
                        }

                        if (args.ContainsKey("IsReportSentToStudent")
                               && Convert.ToBoolean(args["IsReportSentToStudent"]) == false
                               && args.ContainsKey("HierarchyNodeID")
                               && Convert.ToInt32(args["HierarchyNodeID"]) == -1)
                        {
                            if (user.IsNotNull() && !user.IsApplicant)
                            {
                                showReport = true;
                            }
                            else
                            {
                                permissionViolated = true;
                            }
                        }

                        //If the report is sent to client admin and he has node permission
                        if (args.ContainsKey("HierarchyNodeID") && Convert.ToInt32(args["HierarchyNodeID"]) > 0)
                        {
                            Boolean result = false;
                            if (user.IsNotNull() && !user.IsApplicant && user.TenantId != AppConsts.SUPER_ADMIN_TENANT_ID && user.TenantTypeCode == TenantType.Institution.GetStringValue())
                            {
                                result = Presenter.HasNodePermission(user.TenantId.Value, user.OrganizationUserId, Convert.ToInt32(args["HierarchyNodeID"]));
                            }
                            if (result)
                            {
                                showReport = true;
                            }
                            else
                            {
                                permissionViolated = true;
                            }
                        }

                        //Report can be seen by super admin in all cases
                        if (user.IsNotNull() && user.TenantId == AppConsts.SUPER_ADMIN_TENANT_ID && !user.IsSharedUser)
                        {
                            showReport = true;
                        }
                        else
                        {
                            permissionViolated = true;
                        }
                        queryStringForReportViewer = args.ToEncryptedQueryString();
                    }
                    else if (args.ContainsKey("DocumentType"))
                    {

                    }
                }
                CurrentViewContext.needToShowEmploymentResultReport = showReport;
                if (showReport)
                {
                    SysXWebSiteUtils.SessionService.SetCustomData("CURRENT_URL", AppConsts.BKG_REPORT_VIEWER + "?args=" + queryStringForReportViewer);
                }
                else if (permissionViolated)
                {
                    SysXWebSiteUtils.SessionService.SetCustomData("CURRENT_URL", AppConsts.BKG_REPORT_VIEWER + "?args=PermissionVoilated");
                }
                #endregion

                HandleAgencyVerification(user.UserName);

                Response.Redirect("Default.aspx", false);
            }

        }

        private void HandleAgencyVerification(String userName)
        {
            Entity.OrganizationUser orgUser = new Entity.OrganizationUser();
            Dictionary<String, String> profileArgs = new Dictionary<String, String>();
            if (SiteUrl.ToLower() == CurrentViewContext.SharedUserLoginURL.ToLower() && !Request.QueryString["args"].IsNull())
            {
                profileArgs.ToDecryptedQueryString(Request.QueryString["args"]);
                if (profileArgs.ContainsKey(AppConsts.QUERY_STRING_AGENCY_USER_ID))
                {
                    orgUser = Presenter.IsSharedUserExists(Guid.Empty, false, Convert.ToInt32(profileArgs[AppConsts.QUERY_STRING_AGENCY_USER_ID]));
                }
                if (profileArgs.ContainsKey(AppConsts.PROFILE_SHARING_URL_TYPE)
                       && profileArgs[AppConsts.PROFILE_SHARING_URL_TYPE].Equals(AppConsts.PROFILE_SHARING_URL_TYPE_AGENCY_VERIFICATION) && !orgUser.IsNullOrEmpty()
                       && profileArgs.ContainsKey(AppConsts.QUERY_STRING_USER_TYPE_CODE)
                    && Convert.ToString(profileArgs[AppConsts.QUERY_STRING_USER_TYPE_CODE]) == OrganizationUserType.AgencyUser.GetStringValue()
                  && orgUser.aspnet_Users.UserName.ToLower().Equals(userName.ToLower()))
                {
                    //Code to update newly mapped agencies to IsVerified=1 (for which verification code has been sent in agruments)
                    Boolean isAgencyVerified = Presenter.UpdateAgencyUserAgenciesVerificationCode(Convert.ToInt32(profileArgs[AppConsts.QUERY_STRING_AGENCY_USER_ID])
                          , Convert.ToString(profileArgs[AppConsts.PROFILE_SHARING_URL_VERIFICATION_TOKEN]), orgUser);
                    if (isAgencyVerified)
                    {
                        Session[AppConsts.SESSION_KEY_ISAGENCY_MAPPED] = "true";
                    }
                }
            }
        }

        public void ManageCentralLoginDiv()
        {
            this.InstUrlExists = Presenter.IsUrlExistForInstitutionType();

            if (this.InstUrlExists || SiteUrl.Contains(AppConsts.LOCAL_HOST) || !IsCentralLogin())
                eventShowCentralLoginDiv(false);
            else
                eventShowCentralLoginDiv(true);

            if (Presenter.IsUrlAdminType())
                eventShowCentralLoginDiv(true, true);

            //UAT-1110 - Profile sharing - hiding create account and shared create account divs if current url is shared login URL
            if (IsSharedUserLogin())
            {
                eventShowCentralLoginDiv(false, true);
                //dvCreateAccount.Visible = false;
                //divCentralCreateAccount.Visible = false;
            }

        }

        /// <summary>
        /// Change the UI settings, as per whether the screen is opened as a central url.
        /// </summary>
        private Boolean IsCentralLogin()
        {
            var _centralLoginUrl = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_CENTRAL_LOGIN_URL]);

            String _centralHost = _centralLoginUrl;
            if (_centralLoginUrl.Contains("http"))
            {
                Uri _url = new Uri(_centralLoginUrl);
                _centralHost = _url.Host;
            }

            if (_centralLoginUrl.IsNullOrEmpty()
                        ||
                 (!_centralLoginUrl.IsNullOrEmpty() && !CurrentViewContext.SiteUrl.ToLower().Trim().Contains(_centralHost.ToLower().Trim()))
               )
                return false;
            else
                return true;
        }

        /// <summary>
        /// Method to Check whether the user tries to login with shared user login URL
        /// </summary>
        /// <returns></returns>
        private Boolean IsSharedUserLogin()
        {
            var _sharedLoginUrl = CurrentViewContext.SharedUserLoginURL.ToLower().Trim();

            String _sharedLoginHost = _sharedLoginUrl;
            if (_sharedLoginUrl.Contains("http"))
            {
                Uri _url = new Uri(_sharedLoginUrl);
                _sharedLoginHost = _url.Host;
            }

            if (_sharedLoginUrl.IsNullOrEmpty()
                        ||
                 (!_sharedLoginUrl.IsNullOrEmpty() && !CurrentViewContext.SiteUrl.ToLower().Trim().Contains(_sharedLoginHost.ToLower().Trim()))
               )
                return false;
            else
                return true;
        }

        public void ManageProfileSharing()
        {
            #region UAT-1110 - Profile Sharing And UAT-1218

            Entity.OrganizationUser orgUser = new Entity.OrganizationUser();
            Dictionary<String, String> profileArgs = new Dictionary<String, String>();
            if (SiteUrl.ToLower() == CurrentViewContext.SharedUserLoginURL.ToLower() && !Request.QueryString["args"].IsNull())
            {
                profileArgs.ToDecryptedQueryString(Request.QueryString["args"]);

                //If Client Contact Token is recieved from QueryString
                if (profileArgs.ContainsKey(AppConsts.QUERY_STRING_CLIENTCONTACT_TOKEN))
                {
                    orgUser = Presenter.IsSharedUserExists(Guid.Parse(profileArgs[AppConsts.QUERY_STRING_CLIENTCONTACT_TOKEN]), false, null);
                }
                //If Invitation Token is recieved from QueryString
                else if (profileArgs.ContainsKey(AppConsts.QUERY_STRING_INVITE_TOKEN))
                {
                    orgUser = Presenter.IsSharedUserExists(Guid.Parse(profileArgs[AppConsts.QUERY_STRING_INVITE_TOKEN]), true, null);
                }
                else if (profileArgs.ContainsKey(AppConsts.QUERY_STRING_AGENCY_USER_ID))
                {
                    orgUser = Presenter.IsSharedUserExists(Guid.Empty, false, Convert.ToInt32(profileArgs[AppConsts.QUERY_STRING_AGENCY_USER_ID]));
                }

                //Add OrguserTypeMapping if current UserTypeCode is not exist
                if (!orgUser.IsNullOrEmpty())
                {
                    if (profileArgs.ContainsKey(AppConsts.PROFILE_SHARING_URL_TYPE)
                        && profileArgs[AppConsts.PROFILE_SHARING_URL_TYPE].Equals(AppConsts.PROFILE_SHARING_URL_TYPE_AGENCY_VERIFICATION))
                    {
                        String userName = orgUser.aspnet_Users.UserName;
                        //WclTextBox txtUserName = ucCentrePanel.FindControl("txtUserName") as WclTextBox;
                        if (!txtUserName.IsNullOrEmpty())
                        {
                            txtUserName.Text = userName;
                            txtUserName.Enabled = false;
                        }
                    }
                    else
                    {
                        Presenter.GetOrganizationUserTypeMapping(orgUser.UserID);
                        List<String> orgUserTypeCode = new List<String>();
                        if (!CurrentViewContext.OrganizationUserTypeMapping.IsNullOrEmpty())
                        {
                            orgUserTypeCode = CurrentViewContext.OrganizationUserTypeMapping.Select(col => col.lkpOrgUserType.OrgUserTypeCode).ToList();
                        }
                        if (!orgUserTypeCode.Contains(Convert.ToString(profileArgs[AppConsts.QUERY_STRING_USER_TYPE_CODE])))
                        {
                            Presenter.AddOrgUserTypeMapping(orgUser.OrganizationUserID, Convert.ToString(profileArgs[AppConsts.QUERY_STRING_USER_TYPE_CODE]));
                        }
                        if (Convert.ToString(profileArgs[AppConsts.QUERY_STRING_USER_TYPE_CODE]) == OrganizationUserType.AgencyUser.GetStringValue())
                        {
                            if (profileArgs.ContainsKey(AppConsts.QUERY_STRING_INVITE_TOKEN))
                            {
                                Presenter.UpdateInviteeOrganizationUserID(orgUser.OrganizationUserID, Guid.Parse(profileArgs[AppConsts.QUERY_STRING_INVITE_TOKEN]), null);
                            }
                            else if (profileArgs.ContainsKey(AppConsts.QUERY_STRING_AGENCY_USER_ID))
                            {
                                Presenter.UpdateInviteeOrganizationUserID(orgUser.OrganizationUserID, Guid.Empty, Convert.ToInt32(profileArgs[AppConsts.QUERY_STRING_AGENCY_USER_ID]));
                            }
                            Presenter.AssignDefaultRolesToAgencyUser(orgUser);
                        }
                    }
                }
                else
                {
                    //redirect to shared user registration page
                    Response.Redirect(AppConsts.SHARED_USER_REGISTRATION + "?args=" + Request.QueryString["args"], true);
                }
            }
            #endregion
        }

        public void ValidateUser()
        {
            ValidateUserAndAutoLogin(CurrentViewContext.needToShowEmploymentResultReport);
            ValidateUserViaEmail();

            //UAT-2494, New Account verification enhancements (additional verification step)
            if (CurrentViewContext.ShowAdditionalAccountVerificationPage)
            {
                hdnAccountVerificationPopup.Value = "SHOWPOPUP";
                hdnVerificationCode.Value = Request.QueryString["UsrVerCode"].ToString();
            }
            else if (!String.IsNullOrEmpty(Request.QueryString["IsUserActivated"]))
            {
                if (Request.QueryString["IsUserActivated"].ToString() == "1")
                {
                    CurrentViewContext.VerificationMessage = ResourceConst.SECURITY_VERIFICATION_SUCCESS_MESSAGE;
                }
            }
            else
            {
                //Else existing login process before UAT-2494
                ValidateEmailAddressViaEmail();
            }

            ManageValidation();
        }

        /// <summary>
        /// To validate user and auto login
        /// </summary>
        private void ValidateUserAndAutoLogin(Boolean needToShowEmploymentResultReport)
        {
            if (Request.QueryString["TokenKey"] != null)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["TokenKey"]))
                {
                    try
                    {
                        Presenter.ValidateUserAndAutoLogin(Request.QueryString["TokenKey"].ToString());
                        SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                        // Set the Value in Session, to manage the display of error message
                        if (!user.IsNull())
                        {
                            user.IncorrectLoginUrlUsed = this.IsIncorrectLoginUrl;
                            //Changes to be done
                            if (user.UserName.Equals(UserName) && user.IsNewPassword)
                            {
                                Response.Redirect("ChangePassword.aspx");
                            }

                            HandleDisclosureFormDisplay(user, needToShowEmploymentResultReport);
                            ManageAlumniRedirection();//UAT-2960
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                }
            }
        }

        /// <summary>
        /// Method to validate user by Email Address
        /// </summary>
        private void ValidateUserViaEmail()
        {
            if (Request.QueryString["UsrVerCode"] != null)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["UsrVerCode"]))
                {
                    try
                    {
                        Presenter.ValidateUserViaEmailAndRedirect(Request.QueryString["UsrVerCode"].ToString());
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                }
            }

        }

        /// <summary>
        /// Method to Validate EmailAddress By email
        /// </summary>
        private void ValidateEmailAddressViaEmail()
        {
            if (Request.QueryString["AuthReqVerCode"] != null)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["AuthReqVerCode"]))
                {
                    try
                    {
                        Presenter.ValidateEmailAddressViaEmail(Request.QueryString["AuthReqVerCode"].ToString());
                    }

                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                }
            }
        }

        /// <summary>
        /// Method to Check whether need to display Employment or User Attestation Disclosure forms.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="numberOfBuisnessChannel"></param>
        private void HandleDisclosureFormDisplay(SysXMembershipUser user, bool needToShowEmploymentResultReport)
        {
            //If User is client admin and he has any background Feature OR User is shared user and he has recieved any bkg Order Invitation
            //then redirect to User Attestation Disclosure Form Page.

            String documentTypeCode;
            Boolean usertype_Isinstructor = false; //UAT-4292
            Int32 TenantId = user.TenantId.HasValue ? user.TenantId.Value : AppConsts.NONE;
            Boolean IsLocation = SecurityManager.IsLocationServiceTenant(TenantId);
            Int32 numberOfBuisnessChannel = this.Presenter.GetLineOfBusinessesByUser(Convert.ToString(user.UserId), TenantId);
            Boolean isAttestationDocumentAlreadySubmitted = Presenter.IsAttestationDocumentAlreadySubmitted(user.OrganizationUserId);
            var _userType = Presenter.GetUserType(user);
            Presenter.GetOrganizationUserTypeMapping(user.UserId);
            List<String> orgUserTypeCode = new List<String>();
            if (!CurrentViewContext.OrganizationUserTypeMapping.IsNullOrEmpty())
            {
                orgUserTypeCode = CurrentViewContext.OrganizationUserTypeMapping.Select(col => col.lkpOrgUserType.OrgUserTypeCode).ToList();
            }

            //UAT-4292
            if (orgUserTypeCode.Count() == AppConsts.ONE && (orgUserTypeCode.Contains("AAAC") || orgUserTypeCode.Contains("AAAD")))
            {
                usertype_Isinstructor = true;
            }
            //Case 1: If User is Client Admin - then cheking User Attestation Disclosure conditions and Redirect to UAD
            if ((_userType == UserType.CLIENTADMIN
                && CurrentViewContext.SiteUrl.ToLower() != CurrentViewContext.SharedUserLoginURL.ToLower()
                && !isAttestationDocumentAlreadySubmitted
                && Presenter.CheckForClientRoleFeatures(user.UserId)) && !IsLocation)
            {
                documentTypeCode = DislkpDocumentType.USER_ATTESTATION_DISCLOSURE_FORM_CLIENT_ADMIN.GetStringValue();
                RedirectToDisclosurePage(documentTypeCode, user.TenantId.Value, user.OrganizationUserId, needToShowEmploymentResultReport);
            }

            //Case 2: If User is Shared User - then cheking User Attestation Disclosure conditions and Redirect to UAD
            else if ((!orgUserTypeCode.IsNullOrEmpty() && !usertype_Isinstructor
                && CurrentViewContext.SiteUrl.ToLower() == CurrentViewContext.SharedUserLoginURL.ToLower()
                && !isAttestationDocumentAlreadySubmitted && System.Web.HttpContext.Current.Session["AgencyViewAdminOrgUsrID"].IsNullOrEmpty()) && !IsLocation) //UAT 1620:Commented Code : [&& Presenter.CheckForBkgInvitation(user.OrganizationUserId)]
            {
                documentTypeCode = DislkpDocumentType.USER_ATTESTATION_DISCLOSURE_FORM_SHARED_USER.GetStringValue();
                RedirectToDisclosurePage(documentTypeCode, user.TenantId.Value, user.OrganizationUserId, needToShowEmploymentResultReport);
            }

            //Case 3: If User is Shared User - then cheking Employment Disclosure conditions and Redirect to ED
            //UAT-1176 To check if user is client admin who has access to any employment node 
            //then redirect him to EmploymentDisclosure Form page.
            else if ((_userType == UserType.CLIENTADMIN
                && needToShowEmploymentResultReport
                && CurrentViewContext.SiteUrl.ToLower() != CurrentViewContext.SharedUserLoginURL.ToLower()
                && Presenter.CheckEmploymentNodePermission(user.TenantId.Value, user.OrganizationUserId)
                && !Presenter.IsEDFormPreviouslyAccepted(user.OrganizationUserId)) && !IsLocation)
            {
                documentTypeCode = DislkpDocumentType.EMPLOYMENT_DISCLOSURE_FORM.GetStringValue();
                RedirectToDisclosurePage(documentTypeCode, user.TenantId.Value, user.OrganizationUserId, needToShowEmploymentResultReport);
            }
            else if (_userType == UserType.CLIENTADMIN && IsLocation)
            {
                Response.Redirect("/Default.aspx", false);
            }
            //Case 4: TO BE CONFIRM
            /// By not adding this as the 'if-else if' condition above,
            /// it will make sure that when the Client admin logins from Correct url with Single tenant account, with multiple 
            /// bussiness channels, he is redirect to the 'Select Business Channel' screen
            else if ((_userType != UserType.APPLICANT
                && CurrentViewContext.SiteUrl.ToLower() != CurrentViewContext.SharedUserLoginURL.ToLower()
                && numberOfBuisnessChannel > 1))
                Response.Redirect(AppConsts.SELECT_BUSINESS_CHANNEL_URL);

        }

        //UAT-1176 and UAT-1178
        /// <summary>
        /// Method to Redirect to User Attestation or Employment Disclosure Page for Client admins and shared users
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="tenantID"></param>
        /// <param name="organizationUserID"></param>
        private void RedirectToDisclosurePage(String documentTypeCode, Int32 tenantID, Int32 organizationUserID, bool needToShowEmploymentResultReport)
        {
            //Check if User Attestation Doc already submitted
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"DocumentTypeCode", documentTypeCode},
                                                                    {"TenantID", Convert.ToString(tenantID)},
                                                                    {"OrganizationUserID", Convert.ToString(organizationUserID)},
                                                                    {"NeedToShowEmploymentResultReport", Convert.ToString(needToShowEmploymentResultReport)}
                                                                 };
            String url = String.Empty;
            if (documentTypeCode == DislkpDocumentType.EMPLOYMENT_DISCLOSURE_FORM.GetStringValue())
            {
                url = String.Format(AppConsts.EMPLOYMENT_DISCLOSURE + "?args={0}", queryString.ToEncryptedQueryString());
            }
            else if ((documentTypeCode == DislkpDocumentType.USER_ATTESTATION_DISCLOSURE_FORM_CLIENT_ADMIN.GetStringValue() ||
                documentTypeCode == DislkpDocumentType.USER_ATTESTATION_DISCLOSURE_FORM_SHARED_USER.GetStringValue()))
            {
                url = String.Format(AppConsts.USER_ATTESTATION_DISCLOSURE_PAGE + "?args={0}", queryString.ToEncryptedQueryString());
            }

            Response.Redirect(url);
        }

        /// <summary>
        /// UAT-2960
        /// </summary>
        private void ManageAlumniRedirection()
        {
            Int32 alumniTenantId = Presenter.GetAlumniTenantId();
            if (alumniTenantId > AppConsts.NONE)
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (!user.IsNullOrEmpty() && user.IsApplicant)
                {
                    Int32 tenantId = user.TenantId.IsNullOrEmpty() ? AppConsts.NONE : user.TenantId.Value;
                    if (alumniTenantId == tenantId)
                    {
                        SysXWebSiteUtils.AllClientSessionService.IsAlumniRedirectionDue = true;
                    }
                    Boolean IsAlumniRedirectionDue = (Boolean)SysXWebSiteUtils.AllClientSessionService.IsAlumniRedirectionDue;

                    if (alumniTenantId > AppConsts.NONE && !IsAlumniRedirectionDue)
                    {
                        if (alumniTenantId != tenantId)
                        {
                            if (Presenter.IsAlumniAcessActivated(user.OrganizationUserId, tenantId))
                            {
                                //Session["RedirectToAlumni"] = Convert.ToString("Yes");
                                SysXWebSiteUtils.AllClientSessionService.IsAlumniRedirectionDue = true;

                                Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
                                ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
                                appInstData.UserID = Convert.ToString(user.UserId);
                                appInstData.TagetInstURL = Presenter.GetInstitutionUrl(alumniTenantId);
                                appInstData.TokenCreatedTime = DateTime.Now;
                                appInstData.TenantID = alumniTenantId;

                                //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
                                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                                {
                                    appInstData.AdminOrgUserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                                }
                                String key = Guid.NewGuid().ToString();

                                Dictionary<String, ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
                                if (applicationData != null)
                                {
                                    applicantData = applicationData;
                                    applicantData.Add(key, appInstData);
                                    Presenter.UpdateWebApplicationData("ApplicantInstData", applicantData);
                                }
                                else
                                {
                                    applicantData.Add(key, appInstData);
                                    Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
                                }
                                SysXWebSiteUtils.SessionService.ClearSession(true);
                                FormsAuthentication.SignOut();
                                SysXAppDBEntities.ClearContext();

                                //Redirect to login page for Auto-Login 
                                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TokenKey", key  }
                                                                 };
                                Response.Redirect(String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}", key));
                                //Response.Redirect(String.Format("http://" + SharedUserLoginURL + "/Login.aspx?TokenKey={0}", key));
                            }
                        }
                    }
                }
            }
        }

        public void ManageQueryString()
        {
            #region UAT-2792 UCONN SSO Process
            Dictionary<String, String> shibbolethArgs = new Dictionary<String, String>();
            if (!Request.QueryString["shibbolethArgs"].IsNull())
            {
                shibbolethArgs.ToDecryptedQueryString(Request.QueryString["shibbolethArgs"]);
                if (shibbolethArgs.ContainsKey("IsShibbolethLogin"))
                {
                    if (!shibbolethArgs["IsShibbolethLogin"].IsNullOrEmpty())
                    {
                        CurrentViewContext.IsShibbolethLogin = Convert.ToBoolean(shibbolethArgs["IsShibbolethLogin"]);
                    }
                    if (shibbolethArgs.ContainsKey("ShibbolethUniqueIdentifier") && !shibbolethArgs["ShibbolethUniqueIdentifier"].IsNullOrEmpty())
                    {
                        CurrentViewContext.ShibbolethUniqueIdentifier = Convert.ToString(shibbolethArgs["ShibbolethUniqueIdentifier"]);
                    }
                    if (shibbolethArgs.ContainsKey("IntegrationClientID") && !shibbolethArgs["IntegrationClientID"].IsNullOrEmpty())
                    {
                        CurrentViewContext.IntegrationClientId = Convert.ToInt32(shibbolethArgs["IntegrationClientID"]);
                    }
                    if (shibbolethArgs.ContainsKey("IsAutoLoginThroughShibboleth") && !shibbolethArgs["IsAutoLoginThroughShibboleth"].IsNullOrEmpty())
                    {
                        CurrentViewContext.IsAutoLoginThroughShibboleth = Convert.ToBoolean(shibbolethArgs["IsAutoLoginThroughShibboleth"]);
                    }
                    if (shibbolethArgs.ContainsKey("UserName") && !shibbolethArgs["UserName"].IsNullOrEmpty())
                    {
                        CurrentViewContext.UserName = Convert.ToString(shibbolethArgs["UserName"]);
                    }
                    if (shibbolethArgs.ContainsKey("TenantID") && !shibbolethArgs["TenantID"].IsNullOrEmpty())
                    {
                        CurrentViewContext.ShibbolethHostID = Convert.ToInt32(shibbolethArgs["TenantID"]);
                    }
                    if (shibbolethArgs.ContainsKey("Host") && !shibbolethArgs["Host"].IsNullOrEmpty())
                    {
                        CurrentViewContext.HostName = Convert.ToString(shibbolethArgs["Host"]);
                    }
                    if (shibbolethArgs.ContainsKey("IsExistingAccount") && !shibbolethArgs["IsExistingAccount"].IsNullOrEmpty())
                    {
                        CurrentViewContext.IsExistingAccount = Convert.ToBoolean(shibbolethArgs["IsExistingAccount"]);
                    }
                    if (shibbolethArgs.ContainsKey("IsShibbolethApplicant") && !shibbolethArgs["IsShibbolethApplicant"].IsNullOrEmpty())
                    {
                        CurrentViewContext.IsShibbolethApplicant = Convert.ToBoolean(shibbolethArgs["IsShibbolethApplicant"]);
                    }
                }
            }
            #endregion
            //UAT 3600
            if (!Session["IsAutoActivateAndLogin"].IsNullOrEmpty() && Session["IsAutoActivateAndLogin"].ToString() == "true")
            {
                Dictionary<String, String> autoLoginArgs = new Dictionary<String, String>();
                if (!Request.QueryString["autoLoginArgs"].IsNull())
                {
                    autoLoginArgs.ToDecryptedQueryString(Request.QueryString["autoLoginArgs"]);
                    if (autoLoginArgs.ContainsKey("UserName") && !autoLoginArgs["UserName"].IsNullOrEmpty())
                    {
                        CurrentViewContext.UserName = Convert.ToString(autoLoginArgs["UserName"]);
                        //if (Presenter.AutoLogInUsingUserName())
                        //{
                        //    SysXMembershipUser shibbonethUser = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                        //    HandleUserLoginType(shibbonethUser);
                        //    HandleDisclosureFormDisplay(shibbonethUser, needToShowEmploymentResultReport);
                        //}
                        AutoLogInUsingUserName();
                    }
                }
            }
        }

        /// <summary>
        /// Method to Handle User logins for different types of logins like applicant, Client Admin, ADB Admins 
        /// Shared User, Instructor, Precptor etc.
        /// </summary>
        /// <param name="user"></param>
        private void HandleUserLoginType(SysXMembershipUser user)
        {
            var _isIncorrectUrl = CheckIncorrectUrl();
            var _userType = Presenter.GetUserType(user);

            List<String> orgUserTypeCode = new List<String>();
            if (!CurrentViewContext.OrganizationUserTypeMapping.IsNullOrEmpty())
            {
                orgUserTypeCode = CurrentViewContext.OrganizationUserTypeMapping.Select(col => col.lkpOrgUserType.OrgUserTypeCode).ToList();
            }

            HandleAgencyVerification(CurrentViewContext.UserName.ToLower());
            if (this.CheckWebsiteURL)
            {
                //Case 1:  If User is ONLY ONE of the Applicant or Client Admin
                if (orgUserTypeCode.Count == AppConsts.NONE
                    && (_userType == UserType.APPLICANT || _userType == UserType.CLIENTADMIN))
                {
                    var _isMultiTenantUser = this.Presenter.IsMultiTenantUser(user.UserId);
                    if (_isMultiTenantUser && this.IsIncorrectLoginUrl)
                    {
                        // This will be added to the 'WebApplicationData' object from the SelectTenant.aspx screen 
                        user.IncorrectLoginUrlUsed = _isIncorrectUrl;
                        Response.Redirect(AppConsts.SELECT_TENANT_URL);
                    }
                    else if (!_isMultiTenantUser && this.IsIncorrectLoginUrl && !SiteUrl.Contains(AppConsts.LOCAL_HOST))
                        ManageIncorrectUrlLogin(user.UserId, Convert.ToInt32(user.TenantId), _isIncorrectUrl, false);
                }

                //Case 2:  If User is ONLY ONE of the Super Admin, ADBAdmin, or Third Party   
                else if (this.IsIncorrectLoginUrl
                    && orgUserTypeCode.Count == AppConsts.NONE
                    && (_userType == UserType.SUPERADMIN || _userType == UserType.THIRDPARTYADMIN))
                {
                    ManageIncorrectUrlLogin(user.UserId, Convert.ToInt32(user.TenantId), _isIncorrectUrl, false);
                }

                //Case 3:  If User is ONLY ONE of the Applicants Shared User, Agency User, Instructor or Preceptor 
                else if (!orgUserTypeCode.IsNullOrEmpty()
                    && orgUserTypeCode.Count > AppConsts.NONE
                    && !CurrentViewContext.IsSharedUserHasOtherRoles)
                {
                    if (this.IsIncorrectLoginUrl && !SiteUrl.Contains(AppConsts.LOCAL_HOST))
                        ManageIncorrectUrlLogin(user.UserId, AppConsts.SHARED_USER_TENANT_ID, _isIncorrectUrl, true);
                }

                //Case 4:  If User is ONE of the Applicants Shared User, Agency User, Instructor or Preceptor + He also have Other Roles
                else if (!orgUserTypeCode.IsNullOrEmpty()
                    && orgUserTypeCode.Count > AppConsts.NONE && CurrentViewContext.IsSharedUserHasOtherRoles)
                {
                    var _isMultiTenantUser = this.Presenter.IsMultiTenantUser(user.UserId);
                    if (CurrentViewContext.SiteUrl.ToLower() != CurrentViewContext.SharedUserLoginURL.ToLower())
                    {
                        if (_isMultiTenantUser && this.IsIncorrectLoginUrl)
                        {
                            // This will be added to the 'WebApplicationData' object from the SelectTenant.aspx screen 
                            user.IncorrectLoginUrlUsed = _isIncorrectUrl;
                            Response.Redirect(AppConsts.SELECT_TENANT_URL);
                        }
                        else if (!_isMultiTenantUser && this.IsIncorrectLoginUrl && !SiteUrl.Contains(AppConsts.LOCAL_HOST))
                            ManageIncorrectUrlLogin(user.UserId, Convert.ToInt32(user.TenantId), _isIncorrectUrl, false);
                    }
                }
            }
        }

        /// <summary>
        /// Returns whether the user is trying to login from incorrect url, 
        /// based on TenantId from DB (by IsIncorrectLoginUrl property) and 
        /// if it is Not Central Login type
        /// </summary>
        /// <returns></returns>
        private Boolean CheckIncorrectUrl()
        {
            var _isIncorrectUrl = false;

            // Check of 'this.IsIncorrectLoginUrl' is required, 
            // otherwise, even if the user is using corect Tenant Url,
            // removing this check will make it an incorrect login url case
            if (this.IsIncorrectLoginUrl && !this.IsCentralLogin() && !SiteUrl.Contains(AppConsts.LOCAL_HOST))// && !this.IsSharedUserLogin())
                _isIncorrectUrl = true;
            return _isIncorrectUrl;
        }

        /// <summary>
        /// Manage the code to Redriect the applicant to appropriate url, if it was single tenant user
        /// and is using incorrect url to login, including the central login screen
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        private void ManageIncorrectUrlLogin(Guid userId, Int32 tenantId, Boolean isIncorrectUrl, Boolean isSharedUser = false)
        {
            if (!isSharedUser)
            {
                var _targetInstURL = Presenter.GetInstitutionUrl(Convert.ToInt32(tenantId));
                String key = Guid.NewGuid().ToString();
                Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
                ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
                appInstData.UserID = Convert.ToString(userId);
                appInstData.TagetInstURL = _targetInstURL;
                appInstData.TokenCreatedTime = DateTime.Now;
                appInstData.TenantID = Convert.ToInt32(tenantId);
                appInstData.IsIncorrectLogin = isIncorrectUrl;

                Dictionary<String, ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
                if (applicationData != null)
                {
                    applicantData = applicationData;
                    applicantData.Add(key, appInstData);
                    Presenter.UpdateWebApplicationData("ApplicantInstData", applicantData);
                }
                else
                {
                    applicantData.Add(key, appInstData);
                    Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
                }

                SysXWebSiteUtils.SessionService.ClearSession(true);
                FormsAuthentication.SignOut();
                SysXAppDBEntities.ClearContext();

                //Redirect to login page for Auto-Login 
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TokenKey", key  }
                                                                 };
                Response.Redirect(String.Format(_targetInstURL + "/Login.aspx?TokenKey={0}", key));
            }
            else if (isSharedUser)// Shared User
            {
                String key = Guid.NewGuid().ToString();
                Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
                ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
                appInstData.UserID = Convert.ToString(userId);
                appInstData.TokenCreatedTime = DateTime.Now;
                appInstData.IsIncorrectLogin = isIncorrectUrl;
                appInstData.IsSharedUser = true;
                appInstData.TenantID = AppConsts.SHARED_USER_TENANT_ID;
                Dictionary<String, ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
                if (applicationData != null)
                {
                    applicantData = applicationData;
                    applicantData.Add(key, appInstData);
                    Presenter.UpdateWebApplicationData("ApplicantInstData", applicantData);
                }
                else
                {
                    applicantData.Add(key, appInstData);
                    Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
                }

                SysXWebSiteUtils.SessionService.ClearSession(true);
                FormsAuthentication.SignOut();
                SysXAppDBEntities.ClearContext();

                //Redirect to login page for Auto-Login 
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TokenKey", key  }
                                                                 };
                Response.Redirect(String.Format("http://" + SharedUserLoginURL + "/Login.aspx?TokenKey={0}", key));
            }
        }

        #region UAT-2792 UCONN SSO Process
        public void AutoLogInUsingUserName()
        {
            if (Presenter.AutoLogInUsingUserName())
            {
                SysXMembershipUser shibbonethUser = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                HandleUserLoginType(shibbonethUser);
                HandleDisclosureFormDisplay(shibbonethUser, needToShowEmploymentResultReport);
            }
        }
        #endregion

        public void AceMappLogin()
        {
            Dictionary<String, String> dicargs = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                dicargs.ToDecryptedQueryString(Request.QueryString["args"]);

                if (dicargs.ContainsKey("IsRequestExternalPage")==true  && dicargs.ContainsKey("IslinkingExternalUser") && Convert.ToBoolean(dicargs["IslinkingExternalUser"]) && dicargs.ContainsKey("ExternalID") && !dicargs["ExternalID"].IsNullOrEmpty() && dicargs.ContainsKey("IntegrationClientId") && Convert.ToInt32(dicargs["IntegrationClientId"]) > 0)
                {
                    SysXMembershipUser organizationUser = System.Web.Security.Membership.GetUser(System.Text.RegularExpressions.Regex.Replace(CurrentViewContext.UserName, @"(?<=^\s*)\s|\s(?=\s*$)", INTSOF.Utils.Consts.SysXSecurityConst.ASCIISPACE)) as SysXMembershipUser;


                    //Changes to be done || CBI

                    if (organizationUser.IsNotNull() && organizationUser.TenantId == Convert.ToInt32( dicargs["ExternalTenantId"]))
                    {
                        Presenter.InsertAceMappLoginIntegrationEntry(organizationUser.OrganizationUserId, dicargs["ExternalID"].ToString(), Convert.ToInt32(dicargs["IntegrationClientId"]));
                    }
                }
            }
        }

        public void HideButtonAlumniTenant()
        {
            if (!String.IsNullOrEmpty(SiteUrl) && !SiteUrl.Contains(AppConsts.LOCAL_HOST) && !IsCentralLogin())
            {
                Int32 alumniTenantId = Presenter.GetAlumniTenantId();
                if (alumniTenantId > AppConsts.NONE)
                {
                    Int32 selectedTenantID = Presenter.GetWebsiteTenantId(SiteUrl);
                    if (selectedTenantID > AppConsts.NONE && selectedTenantID == alumniTenantId)
                    {
                        eventShowCentralLoginDiv(false, true);
                    }
                }
            }
        }

        #region UAT-4151
        public void HandleOtherAccountLinking()
        {
            if (!Request.QueryString["IsRedirectedFromOtherAccountLinking"].IsNull())
            {
                // CurrentViewContext.ErrorMessage = "Account linked successfully. Please login again.";
                ErrorMessage = Resources.Language.ACCTLINKEDSUCMSG;
            }
        }
        #endregion

        //UAT-2894
        public void SetSessionRequirementShares(SysXMembershipUser user)
        {
            if (!user.IsNull())
            {
                SetSessionForFilteredRequirementShares(true);
            }
        }

        /// <summary>
        /// UAT-2894
        /// </summary>
        /// <param name="IsNeedToRedireactToMainDefault"></param>
        private void SetSessionForFilteredRequirementShares(Boolean IsNeedToRedireactToMainDefault)
        {
            if (!Request.QueryString["args"].IsNullOrEmpty())
            {
                Dictionary<String, String> requestArgs = new Dictionary<String, String>();
                requestArgs.ToDecryptedQueryString(Request.QueryString["args"]);
                if (!requestArgs.IsNullOrEmpty() && requestArgs.ContainsKey("IsSearchedShare"))
                {
                    SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_FILTERED_REQUIREMENT_SHARES, requestArgs);
                    if (IsNeedToRedireactToMainDefault)
                        Response.Redirect("~/Main/Default.aspx");
                }
            }
        }

        public void GetErrorMsgInQueryString()
        {
            if (!Request.QueryString[AppConsts.SESSION_EXPIRED].IsNull())
            {
                ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_SESSION_EXPIRED);
            }
            else if (!Request.QueryString["ChangePassword"].IsNull())
            {
                //ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_PASSWORD_CHANGED_SUCCESSFULLY);
                ErrorMessage = Resources.Language.PSWDCHNGSUCCESSMSG;
            }
            else if (!Request.QueryString["ForgotPassword"].IsNull())
            {
                ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_NEWPASSWORD_SENT_BYEMAIL);
            }
            else if (!Request.QueryString["ForgotUserName"].IsNull())
            {
                ErrorMessage = ResourceConst.SECURITY_USERNAME_SENT_BYEMAIL;
            }
            else if (Request.QueryString["logout"] == "module") // Add This Functionality on TFS BUG # 2111
            {
                System.Reflection.PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                isreadonly.SetValue(Request.QueryString, false, null);
                Request.QueryString.Remove("logout");
                SysXWebSiteUtils.SessionService.ClearSession(true);
            }
        }

        //UAT-2930
        public void ManageTwoFactorAuthentication(SysXMembershipUser user)
        {
            if (!user.IsNullOrEmpty() &&
                (SysXWebSiteUtils.SessionService.UserGoogleAuthenticated == GoogleAuthenticationStatus.Authenticated
                || SysXWebSiteUtils.SessionService.UserGoogleAuthenticated == GoogleAuthenticationStatus.NotApplicable))
            {
                //to do login auto login user through username
                FormsAuthentication.RedirectFromLoginPage(user.UserName, false);
                CurrentViewContext.UserName = user.UserName;
                HandlePendingButtonClick();

            }
        }

        //UAT 3600
        public void HandleAutoActivateAndLoginInCBI()
        {
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;

            if (!Session["IsAutoActivateAndLogin"].IsNullOrEmpty() && Session["IsAutoActivateAndLogin"].ToString() == "true")
            {
                FormsAuthentication.RedirectFromLoginPage(user.UserName, false);
                CurrentViewContext.UserName = user.UserName;
                HandlePendingButtonClick();
            }
        }

        /// <summary>
        /// A we have break the submit ckick in 2 peices.
        /// </summary>
        private void HandlePendingButtonClick()
        {
            SetSessionForFilteredRequirementShares(false);

            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            if (!user.IsNull())
            {
                SetSessionForAlumniToken(user);//UAT-2960

                #region UAT-1053 Bkg Report Display
                bool needToShowEmploymentResultReport = HandleBkgReportsDisplay(user);
                #endregion

                #region If Need to redirect to ChangePassword
                //Changes to be done || CBI
                if (user.UserName.Equals(UserName) && user.IsNewPassword)
                {
                    //UAT-1850 :On Applicant PW reset, change the "Old Password" field to read "Temporary Password"
                    Dictionary<String, String> queryString = null;
                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"IsTempPassReset","true"}
                                                                 };
                    Response.Redirect(String.Format("ChangePassword.aspx?args={0}", queryString.ToEncryptedQueryString()));
                }
                #endregion

                #region UAT-4097 : If Password expired then navigated to change password functionality.
                if (!user.IsNullOrEmpty())
                {
                    //if (Presenter.IsUserExixtInLocationTenants(user.UserId))
                    // {
                    Int32 expiryDays = AppConsts.NONE;
                    if (Presenter.IsUserExixtInLocationTenants(user.UserId))
                    {
                        expiryDays = 90;
                    }
                    else
                    {
                        if (ConfigurationManager.AppSettings.AllKeys.Contains("PasswordExpiryDays"))
                            expiryDays = Convert.ToInt32(ConfigurationManager.AppSettings["PasswordExpiryDays"]);
                    }

                    if (expiryDays > AppConsts.NONE)
                    {
                        if (Presenter.IsPasswordNeedToBeChanged(user.UserId, expiryDays))
                        {
                            Response.Redirect("ChangePassword.aspx");
                        }
                    }
                }
                #endregion

                #region UAT-1218 Conditions for different types of logins and managing incorrect loin url
                HandleUserLoginType(user);
                #endregion

                #region UAT-1178 USER ATTESTATION DISCLOSURE AND UAT-1176 EMPLOYMENT NODE DISCLOSURE
                HandleDisclosureFormDisplay(user, needToShowEmploymentResultReport);
                #endregion

                ManageAlumniRedirection();//UAT-2960
            }
        }

        //UAT-2960
        private void SetSessionForAlumniToken(SysXMembershipUser user)
        {
            if (!Request.QueryString["args"].IsNullOrEmpty())
            {
                Dictionary<String, String> requestArgs = new Dictionary<String, String>();
                requestArgs.ToDecryptedQueryString(Request.QueryString["args"]);
                if (!requestArgs.IsNullOrEmpty() && requestArgs.ContainsKey("AlumniToken"))
                {
                    Guid Token = Guid.Parse(requestArgs["AlumniToken"]);
                    if (!Token.IsNullOrEmpty())
                    {
                        Int32 orgUserId = user.OrganizationUserId.IsNullOrEmpty() ? AppConsts.NONE : user.OrganizationUserId;
                        if (Presenter.CheckForAlumnAccessStatus(Token, orgUserId))
                        {
                            if (Session["ClientMachineIP"].IsNullOrEmpty())
                            {
                                Session["ClientMachineIP"] = Request.UserHostAddress;
                            }
                            String MachineIP = Convert.ToString(Session["ClientMachineIP"]);
                            Presenter.CreateAlumniDefaultSubscription(user.OrganizationUserId, user.OrganizationUserId, Convert.ToInt32(user.TenantId), MachineIP);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method to check and display Background report if user redurected from report pdf link from mail.
        /// </summary>
        /// <param name="user"></param>
        private bool HandleBkgReportsDisplay(SysXMembershipUser user)
        {
            Boolean showReport = false;
            Boolean permissionViolated = false;
            Dictionary<String, String> args = new Dictionary<String, String>();
            String queryStringForReportViewer = "";
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (args.ContainsKey("ReportType"))
                {
                    //If the report sent to the student and currently the same student logged in
                    if (args.ContainsKey("IsReportSentToStudent")
                                && Convert.ToBoolean(args["IsReportSentToStudent"]) == true
                                && args.ContainsKey("OrganizationUserID"))
                    {
                        if (user.IsNotNull() && user.IsApplicant && user.OrganizationUserId == Convert.ToInt32(args["OrganizationUserID"]))
                            showReport = true;
                        else
                            permissionViolated = true;
                    }

                    if (args.ContainsKey("IsReportSentToStudent")
                                && Convert.ToBoolean(args["IsReportSentToStudent"]) == false
                                && args.ContainsKey("HierarchyNodeID")
                                && Convert.ToInt32(args["HierarchyNodeID"]) == -1)
                    {
                        if (user.IsNotNull() && !user.IsApplicant && user.TenantId != AppConsts.SUPER_ADMIN_TENANT_ID && user.TenantTypeCode == TenantType.Institution.GetStringValue())
                            showReport = true;
                        else
                            permissionViolated = true;
                    }

                    //If the report is sent to client admin and he has node permission
                    if (args.ContainsKey("HierarchyNodeID") && Convert.ToInt32(args["HierarchyNodeID"]) > 0)
                    {
                        Boolean result = false;
                        if (user.IsNotNull() && !user.IsApplicant && user.TenantId != AppConsts.SUPER_ADMIN_TENANT_ID && user.TenantTypeCode == TenantType.Institution.GetStringValue())
                            result = Presenter.HasNodePermission(user.TenantId.Value, user.OrganizationUserId, Convert.ToInt32(args["HierarchyNodeID"]));
                        if (result)
                            showReport = true;
                        else
                            permissionViolated = true;
                    }

                    //Report can be seen by super admin in all cases
                    //UAT-1735:- Any shared user will not able to see report
                    if (user.IsNotNull() && user.TenantId == AppConsts.SUPER_ADMIN_TENANT_ID && !user.IsSharedUser)
                        showReport = true;
                    else
                        permissionViolated = true;
                    queryStringForReportViewer = args.ToEncryptedQueryString();
                }
            }

            if (showReport)
            {
                SysXWebSiteUtils.SessionService.SetCustomData("CURRENT_URL", AppConsts.BKG_REPORT_VIEWER + "?args=" + queryStringForReportViewer);
            }
            else if (permissionViolated)
            {
                SysXWebSiteUtils.SessionService.SetCustomData("CURRENT_URL", AppConsts.BKG_REPORT_VIEWER + "?args=PermissionVoilated");
            }
            return showReport;
        }

        private void ManageControlsOnSubmitClick()
        {
            CurrentViewContext.VerificationMessage = String.Empty;
            SiteUrl = Page.Request.ServerVariables.Get("server_name");
            CurrentViewContext.IsAccountInActive = false;
            eventErrorMessageExtended();

            Presenter.ValidateUserAndRedirect();

            if (CurrentViewContext.IsTwoFactorAuthenticationRequired
                  && (SysXWebSiteUtils.SessionService.UserGoogleAuthenticated == GoogleAuthenticationStatus.NotAuthenticated_With_GoogleAuthenticator
                  || SysXWebSiteUtils.SessionService.UserGoogleAuthenticated == GoogleAuthenticationStatus.NotAuthenticated_With_TextMessage))
            {
                if (!Request.QueryString["args"].IsNull())
                {
                    Session["PreLoginQueryString"] = Convert.ToString(Request.QueryString["args"]);
                }
                Response.Redirect("Authenticator.aspx", true);
            }

            HandlePendingButtonClick();
        }


        #region Hr Admin Portal
        public void ManageHrAdminPortalUserLogin(String AdminEntryAppInviteToken)
        {
            SysXWebSiteUtils.SessionService.SetCustomData("showMinDetails", true);
            SysXWebSiteUtils.SessionService.SetCustomData("TokenKey", AdminEntryAppInviteToken);
            var userdata = Presenter.ManageHrAdminPortalUserLogin(AdminEntryAppInviteToken);

            //ADB Admin Entry Portal Order Flow: An applicant can place the order through tampered Invitation link as well. (UAT-4584||Bug ID:22807||P:2||S: Major)
            Int32 selectedTenantID = Presenter.GetWebsiteTenantId(SiteUrl);

            //USE BELOW MENTIONED LINE IF URL IS LOCALHOST
            //if (!userdata.IsNullOrEmpty())

            if (!userdata.IsNullOrEmpty() && userdata.TenantId == selectedTenantID)
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.ADMIN_ENTRY_APPLICANT_LANDING_SCREEN},
                                                                    { "OrderId", userdata.OrderId.ToString()},
                                                                    { "TenantId", userdata.TenantId.ToString()}
                                                                 };
                string url = String.Format("~/AdminEntryPortal/Default.aspx?ucid={0}&args={1}", null, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }
            else
            {
                eventExceptionMessage("Invite URL is invalid");
            }
        }

        #endregion

    }
}