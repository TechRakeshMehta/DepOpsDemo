#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  DefaultMaster.master.cs
// Purpose:   
//
// Revisions:
//Comment
//-------------------------------------------------
//added OnInit() event to handle the user authentication, based on its information in database.

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web.UI;
using Microsoft.Practices.ObjectBuilder;
using System.Xml.Linq;
using System.Linq;
using System.Web.Security;
using System.Web;
using System.Web.UI.HtmlControls;
#endregion

#region Application Specific

using INTSOF.Utils;
using INTSOF.Utils.Consts;
using Entity;
using CoreWeb.IntsofSecurityModel;
using System.Collections.Generic;
using CoreWeb.CommonControls.Views;
using INTERSOFT.WEB.UI.Config;
using Business.RepoManagers;
using System.Web.UI.WebControls;
using System.Configuration;


#endregion

#endregion

namespace CoreWeb.Shell.MasterPages
{
    /// <summary>
    /// Default master.
    /// </summary>
    public partial class DefaultMaster : BaseMasterPage, IDefaultMasterView
    {
        #region Variables

        private DefaultMasterPresenter _presenter = new DefaultMasterPresenter();
        private aspnet_Membership _aspnetMembership;

        public DefaultMaster()
        {
            ShowQueueHistory = false;
            UseAsPopUpWindow = false;
        }

        #endregion

        #region Properties

        #region public properties

        /// <summary>
        /// Gets or sets the presenter.
        /// </summary>
        /// <value>
        /// The presenter.
        /// </value>

        public DefaultMasterPresenter Presenter
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
        /// <summary>
        /// Gets and sets the aspnet_Membership
        /// </summary>
        public aspnet_Membership aspnet_Membership
        {
            get
            {
                return _aspnetMembership;
            }
            set
            {
                _aspnetMembership = value;
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
                return Page.Session.SessionID.IsNullOrEmpty() ? string.Empty : Page.Session.SessionID;
            }
        }

        public bool ShowQueueHistory
        {
            get;
            set;
        }

        public int EntityId
        {
            get;
            set;
        }

        public Dictionary<Int32, Dictionary<String, String>> ContextInfo
        {
            get;
            set;
        }

        public AdminQueueContext AdminQueueContextId
        {
            get;
            set;
        }

        public Int32 TimeoutMinutes
        {
            get
            {
                return ((Int32)FormsAuthentication.Timeout.TotalSeconds - 600);
            }
        }

        //public Boolean IsApplicant
        //{
        //    get;
        //    set;
        //}

        #region UAT-3077
        public Boolean UseAsPopUpWindow
        {
            get;
            set;
        }
        #endregion


        #endregion

        #region Private Properties

        private Int32 QueueOrder
        {
            get
            {
                if (Application["QueueOrder"].IsNullOrEmpty())
                {
                    return -1;
                }
                else
                {
                    return Convert.ToInt32(Application["QueueOrder"]);
                }
            }
            set
            {
                Application["QueueOrder"] = value;
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

        public String Title
        {
            get;
            set;
        }

        private Boolean showMinDetails
        {
            get
            {
                return Convert.ToBoolean(SysXWebSiteUtils.SessionService.GetCustomData("showMinDetails"));
            }
        }
        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Hides the title bar UI component from the rendered page content.
        /// </summary>
        /// <param name="IncludeCssClass">Includes 'no_error_panel' css class in the form element</param>
        public void HideTitleBars(bool IncludeCssClass = false)
        {
            pnlModBar.Visible = false;
            if (IncludeCssClass)
            {
                string cssClass = string.IsNullOrWhiteSpace(this.Page.Form.Attributes["class"]) ? "no_error_panel" : this.Page.Form.Attributes["class"].Replace("no_error_panel", "").Trim() + " no_error_panel";
                this.Page.Form.Attributes["class"] = cssClass;
            }
        }

        /// <summary>
        /// Sets Module title.
        /// </summary>
        /// <param name="title">.</param>
        public void SetModuleTitle(String title)
        {
            lblModHdr.Text = title;
            // lblModHdrSharedUser.Text = title;
            this.Title = title;
        }

        /// <summary>
        /// Sets Page / Screen Title.
        /// </summary>
        /// <param name="title">.</param>
        public void SetPageTitle(String title)
        {
            lblPageHdr.Text = title;
            lblPageHdrSharedUser.Text = title;
        }

        /// <summary>
        /// Shows the error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <remarks></remarks>
        public void ShowErrorMessage(String errorMessage)
        {
            lblError.Text = SysXExceptionConsts.SYSX_EXCEPTION_GENERIC_ERROR_MESSAGE + errorMessage;
            lblError.CssClass = "error";
            pnlError.Update();
        }

        /// <summary>
        /// Shows the error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <remarks></remarks>
        public void ShowSearchErrorMessage(String errorMessage)
        {
            lblError.Text = errorMessage;
            lblError.CssClass = "error";
            pnlError.Update();
        }

        /// <summary>
        /// Display message and set css class as per the message type
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="messageType"></param>
        public void ShowMessageOnPage(String errorMessage, MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Error:
                    lblError.Text = SysXExceptionConsts.SYSX_EXCEPTION_GENERIC_ERROR_MESSAGE + errorMessage;
                    lblError.CssClass = "error";
                    break;
                case MessageType.Information:
                    lblError.CssClass = "info";
                    break;
                case MessageType.SuccessMessage:
                    lblError.CssClass = "sucs";
                    break;
                default:
                    break;
            }

            pnlError.Update();
        }

        /// <summary>
        /// Shows the info message.
        /// </summary>
        /// <param name="infoMessage">The info message.</param>
        /// <remarks></remarks>
        public void ShowInfoMessage(String infoMessage)
        {
            lblError.Text = infoMessage;
            lblError.CssClass = "info";
            pnlError.Update();
        }

        /// <summary>
        /// Shows the info message.
        /// </summary>
        /// <param name="successMessage">The success message.</param>
        /// <remarks></remarks>
        public void ShowSuccessMessage(String successMessage)
        {
            lblError.Text = successMessage;
            lblError.CssClass = "sucs";
            pnlError.Update();
        }

        /// <summary>
        /// Hides the error message.
        /// </summary>
        /// <remarks></remarks>
        public void HideErrorMessage()
        {
            lblError.Text = String.Empty;
            pnlError.Update();
        }

        /// <summary>
        /// Shows the error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <remarks></remarks>
        public void ShowErrorInfoMessage(String errorMessage)
        {
            lblError.Text = errorMessage;
            lblError.CssClass = "error";
            pnlError.Update();
        }

        public void LoadQueueHistory()
        {
            //if (ShowQueueHistory)
            //{
            //    this.plcDynamic.Controls.Clear();
            //    UserControl ucQueueHistory = (UserControl)LoadControl("~/CommonControls/UserControl/QueueHistory.ascx");
            //    ucQueueHistory.ID = "ucQueueHistory";
            //    plcDynamic.Controls.Add(ucQueueHistory);

            //    IQueueHistoryView ucQHistory = (IQueueHistoryView)ucQueueHistory;
            //    //ucQHistory.EntityId = EntityId;
            //    ucQHistory.ContextInfo = ContextInfo;
            //    ucQHistory.AdminQueueContextId = AdminQueueContextId;
            //    ucQHistory.BindHistory();
            //}
        }

        /// <summary>
        /// Register postback controls inside an UpdatePanel control as triggers. 
        /// Controls that are registered by using this method update a whole page instead of updating only the UpdatePanel control's content
        /// </summary>
        /// <param name="registerControl"></param>
        public void RegisterControlForPostBack(Control registerControl)
        {
            scmPage.RegisterPostBackControl(registerControl);
        }
        /// <summary>
        /// Adds query String parameters to the bread crumb node being created for current page
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddQryParamsToBreadCrumbNode(String key, String value)
        {
            breadcrum.AddQueryString(key, value);
            ucSharedUserBreadcrumb.AddQueryString(key, value);
        }
        #endregion

        #region Private Methods
        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Event handler. Called by Page for load events.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">     The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetNoStore();
                if (SysXWebSiteUtils.SessionService.IsNull() || SysXWebSiteUtils.SessionService.SysXMembershipUser.IsNull() 
                    || !Context.User.Identity.IsAuthenticated || Context.User.Identity.Name.IsNullOrEmpty()
                    )
                    Response.Redirect(FormsAuthentication.LoginUrl);
                // Check the user's authentication.
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;

                if (!Context.User.Identity.IsAuthenticated || Context.User.Identity.Name.IsNullOrEmpty())
                {
                    Presenter.DoLogOff(!Context.User.IsNull(), user.OrganizationUserId, user.UserLoginHistoryID);
                    Response.Redirect(FormsAuthentication.LoginUrl);
                }

                if (!IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    hdntimeout.Value = TimeoutMinutes.ToString();
                }

                Presenter.OnViewLoaded();

                if (!Context.User.Identity.IsAuthenticated || Context.User.Identity.Name.IsNullOrEmpty())
                {
                    Presenter.DoLogOff(!user.IsNull(), user.OrganizationUserId, user.UserLoginHistoryID);
                }


                if (!user.IsNull() && !user.IsSharedUser)
                {
                    //Method to show the get help for applicant. 
                    Boolean IsLocationServicetenant = SecurityManager.IsLocationServiceTenant(user.TenantId.Value); //UAT 3675 CBI
                    if (!IsLocationServicetenant) //UAT 3675 CBI
                    {
                        ShowGetHelp(user.IsApplicant);
                    }

                    Boolean status = Presenter.CheckUserStatus(user.UserId);

                    if (!status)
                    {
                        Presenter.DoLogOff(true, user.OrganizationUserId, user.UserLoginHistoryID);
                        RedirectToLoginPage();
                        return;
                    }
                }

                #region   UAT 1349: Pending Design: Agency User Dashboard
                if (!user.IsNull() && user.IsSharedUser)
                {
                    //Hide the Bread Crumb for Shared Users
                    dvNormalBreadcrumb.Visible = false;
                    dvSharedUserBreadcrumb.Visible = true;

                    //Load CSS for Shared User
                    ltrlBootStrap.Text = "<link rel=\"stylesheet\" href=\"../Resources/Mod/Dashboard/Styles/bootstrap.min.css\" type='text/css'/>";
                    ltrlTitilliumFont.Text = "<link rel=\"stylesheet\" href=\"https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700\" type='text/css' />";
                    ltrlFontAwesome.Text = "<link rel=\"stylesheet\" href=\"../Resources/Mod/Dashboard/Styles/font-awesome.min.css\" type='text/css'/>";
                    ltrlSUDashboard.Text = "<link rel=\"stylesheet\" href=\"../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css\" type='text/css'/>";
                }
                else if (!user.IsNull() && user.IsApplicant)
                {
                    if (showMinDetails)
                    {
                        breadcrum.Visible = false;
                    }
                    else
                    {
                        dvNormalBreadcrumb.Visible = true;
                        dvSharedUserBreadcrumb.Visible = false;
                    }
                }
                else
                {
                    dvNormalBreadcrumb.Visible = false;
                    dvSharedUserBreadcrumb.Visible = true;
                    ltrlSUDashboard.Text = "<link rel=\"stylesheet\" href=\"../Resources/Mod/Dashboard/Styles/Breadcrumb.css\" type='text/css'/>";
                }
                #endregion


                if ((!Context.Session.IsNull() && Session.IsNewSession) || user.IsNull())
                {
                    RedirectToLoginPage();
                    return;
                }
                // End

                // Register script that will clear error message on submit.
                String jsname = "OnSubmitScript";
                Type jstype = this.GetType();

                Page.ClientScript.RegisterOnSubmitStatement(jstype, jsname, "FSObject.$('.msgbox').fadeOut();");
                // Update timer countdown on each postback.
                Page.ClientScript.RegisterOnSubmitStatement(jstype, "countdownTimer", "parent.StartCountDown('" + TimeoutMinutes + "');");
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "panel_update", "<script>Page.updatePanel();</script>");

                Dictionary<String, String> assetLegend = SetValuesFromQueryString();
                if (!assetLegend.IsNull())
                {
                    //AD: Changing code to use latest lib function
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "menu_update", "<script>Page.updateMenu();</script>");
                    //rsrMgrPage.CurrentScriptWriter.ModuleScriptBlock.PageLoadBlock.Statements.Append("$page.app.leftPanel.update()");
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "lpanel_update", "<script>$page.app.leftPanel.update();</script>");

                    // Todo: SysXWebSiteUtils.SessionService.SetCustomData(SysXAssetMasterConst.ASSET_LEGEND, null);
                    SysXWebSiteUtils.SessionService.SetCustomData("key", true);
                }
                else
                {
                    Boolean isloaded = SysXWebSiteUtils.SessionService.GetCustomData("key").IsNull() ? false : Convert.ToBoolean(SysXWebSiteUtils.SessionService.GetCustomData("key"));

                    if (isloaded)
                    {
                        //AD: Changing code to use latest lib function
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "menu_update", "<script>Page.updateMenu();</script>");
                        // rsrMgrPage.CurrentScriptWriter.ModuleScriptBlock.PageLoadBlock.Statements.Append("$page.app.leftPanel.update()");
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "lpanel_update", "<script>$page.app.leftPanel.update();</script>");

                        SysXWebSiteUtils.SessionService.SetCustomData("key", false);
                        if (!Session["LoanId"].IsNullOrEmpty())
                        {
                            Session.Remove("LoanId");
                        }
                    }
                }

                //AD: Added code to control page refresh, writing current URI into the session
                if (!string.IsNullOrWhiteSpace(Page.Request.Url.AbsoluteUri))
                {
                    if (!UseAsPopUpWindow)
                        SysXWebSiteUtils.SessionService.SetCustomData("CURRENT_URL", Page.Request.Url.AbsoluteUri);
                }
            }
            catch (System.Exception ex)
            {
                SysXWebSiteUtils.LoggerService.GetLogger().Error("DefaultMaster.cs, Resource Manager Initialisation", ex);
                SysXWebSiteUtils.ExceptionService.HandleError("Unable to load default master because : ", ex);
            }
        }

        /// <summary>
        /// To Set Values From QueryString.
        /// </summary>
        private Dictionary<String, String> SetValuesFromQueryString()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            Dictionary<String, String> dictionaryAssetLegend = null;

            if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
            {
                queryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);


            }
            else
            {
                // Remove from Session
            }

            return dictionaryAssetLegend;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rsrMgr_Init(object sender, EventArgs e)
        {
            try
            {
                String _userPreferenceTheme = String.Empty;

                _userPreferenceTheme = (String)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_THEME);

                string themeSection = System.Configuration.ConfigurationManager.AppSettings["ThemeSection"];
                string defaultTheme = System.Configuration.ConfigurationManager.AppSettings["DefaultTheme"];

                WclTheme currentTheme = null;
                if (!String.IsNullOrEmpty(_userPreferenceTheme))
                    //WclTheme currentTheme = (System.Configuration.ConfigurationManager.GetSection(themeSection) as WclThemeSection).Themes[_userPreferenceTheme];
                    currentTheme = (System.Configuration.ConfigurationManager.GetSection(themeSection) as WclThemeSection).Themes[_userPreferenceTheme];

                if (currentTheme == null)
                {
                    _userPreferenceTheme = defaultTheme;
                    currentTheme = (System.Configuration.ConfigurationManager.GetSection(themeSection) as WclThemeSection).Themes[_userPreferenceTheme];
                }
                rsrMgrPage.ThemeName = _userPreferenceTheme;
                rsrMgrPage.SkinCollection = !currentTheme.IsNull() ? currentTheme.Skins : null;
            }
            catch (System.Exception ex)
            {
                SysXWebSiteUtils.LoggerService.GetLogger().Error("appMaster.cs, Resource Manager Initialisation", ex);
                SysXWebSiteUtils.ExceptionService.HandleError("Unable to build menus for user : " + SysXWebSiteUtils.SessionService.SysXMembershipUser.UserName, ex);
            }

        }

        /// <summary>
        /// Method to show the get help for Applicant.
        /// </summary>
        /// <param name="organizationUserId">organizationUserId</param>
        private void ShowGetHelp(Boolean isApplicant)
        {
            if (isApplicant)
            {
                lhnScript.Visible = true;
            }
        }

        #endregion
    }
}