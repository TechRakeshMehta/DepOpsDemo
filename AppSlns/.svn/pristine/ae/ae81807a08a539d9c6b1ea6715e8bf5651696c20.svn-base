#region Header Comment Master

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  AppMaster.master.cs
// Purpose:   
//
// Revisions:
// Comment
// -------------------------------------------------
// Updated header comment block, and reviewed the code as per review list provided.
// Added Asset Legend

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;
using System.Text;
using System.Linq;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Telerik.Web.UI;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.IntsofSecurityModel.Providers;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using INTERSOFT.WEB.UI.WebControls;
using INTERSOFT.WEB.UI.Config;
using System.Threading;
using INTSOF.UI.Contract.SysXSecurityModel;
using System.Web.Configuration;
using INTSOF.Contracts;
using Entity.Navigation;
using System.Web.Services;
using System.Configuration;
using Entity.SharedDataEntity;
using System.Globalization;
using INTSOF.UI.Contract.ProfileSharing;
#endregion

#endregion

namespace CoreWeb.Shell.MasterPages
{
    /// <summary>
    ///  This class handles the operations related to SysXDefaultMaster.
    /// </summary>
    public partial class AppMaster : BaseMasterPage, IAppMasterView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ISysXSessionService _sessionService = SysXWebSiteUtils.SessionService;

        private AppMasterPresenter _presenter = new AppMasterPresenter();

        private aspnet_Membership _aspnetMembership;

        private string _defaultLineOfBusiness = string.Empty;

        private string _siteTitle = string.Empty;
        private String _viewType;
        private Int32 _tenantId = 0;
        private Int32 _blockId = 0;

        private int _profileSharingInvitationConfirmationTimerInterval = 60000;
        private String _adminPortalAppUrlPrefix = ""; //Admin Entry Portal
        private String _componentRoutePath = ""; //Admin Entry Portal
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the presenter.
        /// </summary>
        /// <value>The presenter.</value>
        /// <remarks></remarks>

        public AppMasterPresenter Presenter
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
        /// Sets the assigned blocks.
        /// </summary>
        /// <value>The assigned blocks.</value>
        /// <remarks></remarks>
        public List<vw_UserAssignedBlocks> AssignedBlocks
        {
            set
            {
                //cmbLineOfBusiness.DataSource = value; cmbLineOfBusiness.DataBind();
            }
        }

        /// <summary>
        /// Gets the current user id.
        /// </summary>
        /// <remarks></remarks>
        public String CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// Gets or Sets the Default Line of Business.
        /// </summary>
        /// <remarks></remarks>
        public String DefaultLineOfBusiness
        {
            get
            {
                return _defaultLineOfBusiness;
            }
            set
            {
                _defaultLineOfBusiness = value;
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
                return Page.Session.SessionID.IsNullOrEmpty() ? String.Empty : Page.Session.SessionID;
            }
        }

        public string SiteTitle
        {
            set { _siteTitle = value; }
        }

        public string HeaderHtml
        {
            set { btnHeader.Text = value; }
        }

        public string FooterHtml
        {
            set { litFooter.Text = value; }
        }

        public bool IsSharedUser
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsSharedUser"]);
            }
            set { ViewState["IsSharedUser"] = value; }
        }

        public Int32 loggedInUserId
        {
            get
            {
                return Convert.ToInt32(base.CurrentUserId);
            }

        }

        public Int32 CurrentOrgUserId
        {
            get
            {
                return base.CurrentOrgUserId;
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
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

        public String SelectedTenantId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlTenantName.SelectedValue))
                    return AppConsts.ZERO;
                return ddlTenantName.SelectedValue;
            }
            set
            {
                ddlTenantName.SelectedValue = value;
            }
        }

        public String SelectedTenantIdForClientAdmin
        {
            get
            {
                if (String.IsNullOrEmpty(clientAdminddlTenantName.SelectedValue))
                    return AppConsts.ZERO;
                return clientAdminddlTenantName.SelectedValue;
            }
            set
            {
                clientAdminddlTenantName.SelectedValue = value;
            }
        }

        public List<Tenant> lstTenant
        {
            get;
            set;
        }

        /*UAT:-3032*/
        public List<Tenant> lstTenants
        {
            get;
            set;
        }
        /*END UAT:-3032 */
        #region UAT-1218
        public Boolean IsSharedUserLoginURL
        {
            get;
            set;
        }
        public List<OrganizationUser> LstOrganizationUser
        {
            get
            {
                return ViewState["LstOrganizationUser"] as List<OrganizationUser>;
            }
            set
            {
                ViewState["LstOrganizationUser"] = value;
            }
        }
        public List<lkpUserTypeSwitchView> lstUserTypeSwitchView
        {
            get;
            set;
        }
        public String UserTypeSwitchViewCode
        {
            get;
            set;
        }
        #endregion


        public int ProfileSharingInvitationConfirmationTimerInterval
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains("ProfileSharingInvitationConfirmationTimerInterval"))
                    return Convert.ToInt32(!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ProfileSharingInvitationConfirmationTimerInterval"]) ? ConfigurationManager.AppSettings["ProfileSharingInvitationConfirmationTimerInterval"] : _profileSharingInvitationConfirmationTimerInterval.ToString());
                else
                    return _profileSharingInvitationConfirmationTimerInterval;
            }
        }

        public bool IsCurrentUserIsAdminOrClientAdmin
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user.IsNotNull())
                {
                    if (user.IsApplicant.IsNotNull() && user.IsApplicant)
                        return false;
                    else if (!user.IsApplicant && user.TenantTypeCode == TenantType.Institution.GetStringValue())
                        return true;
                    else if (!user.IsApplicant && (user.TenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue()))
                        return false;
                    else if (user.IsSharedUser.IsNotNull() && user.IsSharedUser)
                        return false;
                    else
                        return true;
                }
                return false;
            }
        }
        public Boolean AgencyApplicantStatus { get; set; }

        public Boolean IsUserAllowedPreferredTenant
        {
            get
            {
                if (ViewState["IsUserAllowedPreferredTenant"].IsNullOrEmpty())
                {
                    Presenter.IsUserAllowedPreferredTenant();
                }
                return Convert.ToBoolean(ViewState["IsUserAllowedPreferredTenant"]);
            }
            set
            {
                ViewState["IsUserAllowedPreferredTenant"] = value;
            }
        }

        public Boolean IsLocationServiceTenant
        {
            get
            {
                if (ViewState["IsLocationServiceTenant"] == null)
                {
                    ViewState["IsLocationServiceTenant"] = Presenter.IsLocationServiceTenant();
                }

                return Boolean.Parse(ViewState["IsLocationServiceTenant"].ToString());
            }
        }

        public List<AgencyUserReportPermissionContract> lstAgencyUserReportPermissions
        {
            get
            {
                if (!ViewState["lstAgencyUserReportPermissions"].IsNullOrEmpty())
                    return (List<AgencyUserReportPermissionContract>)ViewState["lstAgencyUserReportPermissions"];
                return new List<AgencyUserReportPermissionContract>();
            }
            set
            {
                ViewState["lstAgencyUserReportPermissions"] = value;
            }
        }

        public List<Entity.SharedDataEntity.lkpAgencyUserReport> lstAgencyUserReports
        {
            get
            {
                if (!ViewState["lstAgencyUserReports"].IsNullOrEmpty())
                    return (List<Entity.SharedDataEntity.lkpAgencyUserReport>)ViewState["lstAgencyUserReports"];
                return new List<Entity.SharedDataEntity.lkpAgencyUserReport>();
            }
            set
            {
                ViewState["lstAgencyUserReports"] = value;
            }
        }

        public Boolean IsAllReportsNotVisible
        {
            get
            {
                if (!ViewState["IsAllReportsNotVisible"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsAllReportsNotVisible"]);
                return false;
            }
            set
            {
                ViewState["IsAllReportsNotVisible"] = value;
            }
        }
        #endregion

        #region Private Properties
        private Boolean IsSessionExpired
        {
            get
            {
                return (String.Compare(txtIsSessionExpired.Value, AppConsts.TRUE, true) == 0);
            }
        }

        /// <summary>
        /// List of the Website Pages
        /// </summary>
        public List<WebSiteWebPage> lstWebsitePages
        {
            get;
            set;
        }

        public Int32 AgencyUserPermissionAccessTypeId { get; set; }

        public Int32 AgencyUserPermissionTypeId { get; set; }

        #region Changes- Admin Entry Portal
        private Boolean showMinDetails
        {
            get
            {
                return Convert.ToBoolean(SysXWebSiteUtils.SessionService.GetCustomData("showMinDetails"));
            }
        }
        private Boolean IsReactToMVPRedirection
        {
            get
            {
                return Convert.ToBoolean(SysXWebSiteUtils.SessionService.GetCustomData("IsReactToMVPRedirection"));
            }
        }
        #endregion

        #endregion

        #endregion

        #region Events



        /// <summary>
        /// Raises the <see cref="E:Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                divInstitute.InnerText = "| " + Resources.Language.INSTITUTE + " ";
                base.OnInit(e);
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];


                if (!Context.User.Identity.IsAuthenticated || Context.User.Identity.Name.IsNullOrEmpty()
                    || SysXWebSiteUtils.SessionService == null || SysXWebSiteUtils.SessionService.SysXMembershipUser == null
                    )
                {
                    SessionExpRedirectToLoginPage("success");
                    return;
                }

                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                IsSharedUser = user.IsSharedUser;
                //UAT-900 : New client admin is able to auto-login into the application when URL of the application is set to default on Change Password screen 
                if ((user.IsNotNull() && user.IsNewPassword == true))
                {
                    Response.Redirect("ChangePassword.aspx");
                }

                if (!user.IsNull() && !user.IsSharedUser)
                {
                    //Boolean status = Presenter.CheckUserStatus(user.OrganizationUserId);
                    Boolean status = Presenter.CheckUserStatus(user.UserId, user.UserName, user.TenantId);

                    if (!status)
                    {
                        SessionExpRedirectToLoginPage("success");
                    }
                }

                if ((!Context.Session.IsNull() && Session.IsNewSession) || user.IsNull())
                {
                    SessionExpRedirectToLoginPage("success");
                }

                GetLanguageCulture(user.UserId);
                ManageLanguageTranslation();
                //For Mobile Device Changes.
                SetContainerHeightForMobileDevice();
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
            catch (System.Exception ex)
            {
                Response.Write(ex.Message);
            }
        }



        private void GenerateFooter()
        {
            StringBuilder sbFooter = new StringBuilder();
            if (lstWebsitePages.IsNull()) return;
            foreach (var page in lstWebsitePages)
            {
                if (page.LinkPosition == Convert.ToInt32(CustomPageLinkPosition.Footer))
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", @"UserControl/CustomPageContent.ascx"},
                                                                    {"PageId",Convert.ToString( page.WebSiteWebPageID)}
                                                                 };
                    String url = String.Format("Website/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    sbFooter.Append("<a href=" + url + ">" + page.LinkText + "</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                }
            }
            litFooter.Text = litFooter.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + Convert.ToString(sbFooter);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            String dt = DateTime.Today.ToString("MM/dd/yyyy");
            IsSharedUserLogin(Page.Request.ServerVariables.Get("server_name"));

            #region Culture Specific

            btnLogoff.Text = Resources.Language.LOGOUT;
            btnLogoff.ToolTip = Resources.Language.CLICKLOGOUT;
            lnkhome_link.ToolTip = Resources.Language.CLKTOGOHOME;
            linkHeader.ToolTip = Resources.Language.RETURNTODSHBRD;
            hdnCloseSplashScreen.Value = Resources.Language.CLOSESPLSHSCRN;
            hdnClickingHere.Value = Resources.Language.CLICKINGHERE;
            hdnWhilePageLoad.Value = Resources.Language.WHILEPAGELOAD;
            #endregion
            //lblDT.Text = dt;
            try
            {
                rdmnMainMenu.DataSourceID = null;
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetNoStore();

                var _url = (sender as MasterPage).Request.Url.Authority;

                if (!SysXWebSiteUtils.SessionService.IsNull() && !SysXWebSiteUtils.SessionService.SysXMembershipUser.IsNull())
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;

                    Dictionary<String, String> queryString = new Dictionary<String, String>();

                    //if (obj.IsNotNull())
                    if (user.IsNotNull())
                    {
                        #region Set UserName
                        lnkUserName.Text = ((String.IsNullOrEmpty(user.LastName)) ? "" : user.LastName + ", ") + user.FirstName;
                        #endregion

                        if (user.IsApplicant && !IsSharedUserLoginURL)
                        {

                            #region APPLICANT
                            home_Link.Attributes.CssStyle.Add("display", "block");
                            lnkhome_link.Visible = true;
                            lnkhome_link.NavigateUrl = AppConsts.APPLICANT_MAIN_PAGE_NAME;

                            lnkUserName.Enabled = true;
                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", @"~/ApplicantModule/UserControl/EditProfile.ascx"},
                                                                    {"PageType","MyProfile"}
                                                                 };
                            lnkUserName.NavigateUrl = String.Format("~/ApplicantModule/default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                            //Adding tooltip for the user profile link only if link is enabled
                            if (!string.IsNullOrWhiteSpace(lnkUserName.NavigateUrl))
                            {
                                //lnkUserName.ToolTip = "Click to view your profile";
                                lnkUserName.ToolTip = Resources.Language.CLCKVIEWPRFILETOOLTIP;
                            }

                            //Setting iframe as a target 
                            lnkUserName.Target = "pageFrame";

                            divClientAdminTenant.Visible = false;

                            if (user.IncorrectLoginUrlUsed)
                                RegisterIncorrectUrlMsgScript(_url, user);
                            #endregion

                            #region Changes - Admin Portal
                            if (showMinDetails)
                            {
                                lnkhome_link.NavigateUrl = String.Empty;
                                lnkhome_link.Visible = false;
                                //dvAutoLogOut.Style["display"] = "None";
                                btnLogoff.Style["display"] = "None";
                                dvShowMinDetails.Visible = true;
                                linkHeader.NavigateUrl = "";
                                lnkUserName.Enabled = false;
                                linkHeader.ToolTip = String.Empty;
                                divTenantName.Visible = false;
                                divTenantNameForClientAdmin.Visible = false;
                                divInstitute.Visible = false;
                                divClientAdminTenant.Visible = false;
                                dvLanguage.Visible = false;
                                btnLanguage.Visible = false;
                            }
                            #endregion
                        }
                        //else if ((obj.IsSystem || user.IsSysXAdmin)) //Presenter.IsDefaultTenant() //ADB Admin or Default Tenant
                        else if ((user.IsSystem || user.IsSysXAdmin) && !IsSharedUserLoginURL)
                        {
                            #region SUPER ADMIN
                            //Disable user name link, no need to open edit profile page for ADB Admin
                            lnkUserName.Enabled = false;

                            //Hide Institute if user is not applicant
                            divClientAdminTenant.Visible = false;
                            user.IncorrectLoginUrlUsed = false;
                            if (!IsPostBack)
                                BindPreferredTenant(); //UAT:-3172
                            #endregion
                        }
                        #region COMMENTED CODE FOR CLIENT ADMIN
                        //else if (!user.IsApplicant && user.TenantTypeCode == TenantType.Institution.GetStringValue()
                        //    &&  !IsSharedUserLoginURL) //else Client Admin Or Third party/Compliance Reviewer
                        //{
                        //    #region CLINT ADMIN OR THIRD PARTY 
                        //    lnkUserName.Enabled = true;
                        //    queryString = new Dictionary<String, String>
                        //                                         { 
                        //                                            { "Child", @"~/IntsofSecurityModel/UserControl/AdminEditProfile.ascx"},
                        //                                         };
                        //    lnkUserName.NavigateUrl = String.Format("~/IntsofSecurityModel/default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                        //    //Setting iframe as a target 
                        //    lnkUserName.Target = "pageFrame";

                        //    //Adding tooltip for the user profile link only if link is enabled
                        //    if (!string.IsNullOrWhiteSpace(lnkUserName.NavigateUrl))
                        //    {
                        //        lnkUserName.ToolTip = "Click to view your profile";
                        //    }

                        //    //UAT 712
                        //    //lnkChangePassword.Enabled = true;
                        //    //queryString = RedirectToChangePassword(queryString);
                        //    if (user.TenantId != Presenter.GetDefaultTenantID() && user.TenantTypeCode.Equals(TenantType.Institution.GetStringValue()))
                        //    {
                        //        divClientAdminTenant.Visible = true;
                        //    }
                        //    else
                        //    {
                        //        divClientAdminTenant.Visible = false;
                        //    }
                        //    if (user.IncorrectLoginUrlUsed && user.TenantTypeCode == TenantType.Institution.GetStringValue())
                        //        RegisterIncorrectUrlMsgScript(_url, user);
                        //    #endregion
                        //}
                        #endregion
                        else if (user.IsSharedUser && IsSharedUserLoginURL)
                        {
                            #region SHARED USER
                            lnkUserName.Enabled = true;

                            //If is logged in user is INSTRUCTOR/PERCEPTOR or Shared User(Agency User or Applicant shared user)
                            //If user is not switched then first prefer Agency User, then Instructor/Preceptor, then Applicant Shared User

                            List<OrganizationUserTypeMapping> listOrganizationUserTypeMapping = Presenter.GetOrganizationUserTypeMapping(user.UserId);
                            if (!listOrganizationUserTypeMapping.IsNullOrEmpty())
                            {
                                Boolean checkIfInstructorPreceptor = listOrganizationUserTypeMapping.Any(x => (x.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Instructor.GetStringValue())
                                                                    || (x.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Preceptor.GetStringValue()));
                                Boolean checkIfAgencyUser = listOrganizationUserTypeMapping.Any(x => (x.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.AgencyUser.GetStringValue()));
                                Boolean checkIfApplicantsSharedUser = listOrganizationUserTypeMapping.Any(x => (x.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.ApplicantsSharedUser.GetStringValue()));

                                if (checkIfAgencyUser && user.SharedUserTypesCode.Contains(OrganizationUserType.AgencyUser.GetStringValue()))
                                {
                                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.AGENCY_USER_PROFILE_URL}
                                                                 };

                                    String url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                    lnkUserName.NavigateUrl = url;
                                    lnkEditProfile.HRef = url;
                                    liEditProfileForInstrPrec.Visible = false; //UAT-3208

                                    //Set the navigation links for Shared User Menu
                                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.SHARED_USER_DASHBOARD}
                                                                 };


                                    if (!Session[AppConsts.SESSION_KEY_ISAGENCY_MAPPED].IsNullOrEmpty() && Session[AppConsts.SESSION_KEY_ISAGENCY_MAPPED].ToString().ToLower() == "true")
                                    {
                                        lblVerificationMessage.Text = "Your link with respective Agency has been verified.";
                                        Session.Remove(AppConsts.SESSION_KEY_ISAGENCY_MAPPED);
                                    }
                                    else
                                    {
                                        lblVerificationMessage.Text = "";
                                    }

                                }
                                else if (checkIfInstructorPreceptor && (user.SharedUserTypesCode.Contains(OrganizationUserType.Instructor.GetStringValue())
                                    || user.SharedUserTypesCode.Contains(OrganizationUserType.Preceptor.GetStringValue())))
                                {
                                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.CLIENT_CONTACT_PROFILE_URL},
                                                                    { "PageLoadType",  "Normal"}
                                                                    //,{"PageType","MyProfile"}
                                                                 };

                                    String url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                    lnkUserName.NavigateUrl = url;
                                    //lnkEditProfile.HRef = url; //UAT-3208
                                    lnkEditProfileForInstrPrec.HRef = url;
                                    liTools.Visible = false;

                                    //UAT-1533: WB: Instructor/Preceptor portal rework and search addition
                                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.INSTRCTR_PRECEPTR_ROTATION_SEARCH_URL}
                                                                 };

                                    //String studentSearchUrl = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                    String studentSearchUrl = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                    lnkRotationStudentSearch.HRef = studentSearchUrl;
                                    liRotationStudentSearch.Visible = true;
                                    liSideVideoTutorialNavigation.Visible = false; //UAT- 2951
                                    liHelp.Visible = false; //UAT-3208
                                    liSearch.Visible = false; //UAT-3208
                                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.INSTRUCTOR_PRESEPTOR_DASHBOARD}
                                                                 };
                                }
                                else if (checkIfApplicantsSharedUser && user.SharedUserTypesCode.Contains(OrganizationUserType.ApplicantsSharedUser.GetStringValue()))
                                {
                                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.AGENCY_USER_PROFILE_URL},
                                                                    { "IsApplicantsSharedUser", checkIfApplicantsSharedUser.ToString()}
                                                                 };

                                    String url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                    lnkUserName.NavigateUrl = url;
                                    lnkEditProfile.HRef = url;
                                    liEditProfileForInstrPrec.Visible = false; //UAT-3406
                                    liSearch.Visible = false;  //UAT-3406
                                    //Set the navigation links for Shared User Menu
                                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.SHARED_USER_DASHBOARD}
                                                                 };
                                }
                                else
                                {
                                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.SHARED_USER_DASHBOARD}
                                                                    //,{"PageType","MyProfile"}
                                                                 };

                                    String url = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                    lnkUserName.NavigateUrl = url;
                                    lnkEditProfile.HRef = url;
                                }
                                ////Check to load instructor preceptor Dashboard
                                //if (checkIfInstructorPreceptor)
                                //{
                                //    queryString = new Dictionary<String, String>
                                //                             { 
                                //                                { "Child",  AppConsts.INSTRUCTOR_PRESEPTOR_DASHBOARD}
                                //                             };

                                //    String url = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                //    lnkUserName.NavigateUrl = url;
                                //    lnkEditProfile.HRef = url;
                                //}
                                //else
                                //{
                                //    //queryString = new Dictionary<String, String>
                                //    //                         { 
                                //    //                            { "Child",  AppConsts.SHARED_USER_DASHBOARD}
                                //    //                            //,{"PageType","MyProfile"}
                                //    //                         };

                                //    //String url = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                //    //lnkUserName.NavigateUrl = url;
                                //    //lnkEditProfile.HRef = url;

                                //Check to load instructor preceptor Dashboard
                                //if (checkIfInstructorPreceptor)
                                //{
                                //    queryString = new Dictionary<String, String>
                                //                         { 
                                //                            { "Child",  AppConsts.INSTRUCTOR_PRESEPTOR_DASHBOARD}
                                //                         };

                                //    String url = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                //    lnkUserName.NavigateUrl = url;
                                //    lnkEditProfile.HRef = url;
                                //}
                                //else
                                //{
                                //    queryString = new Dictionary<String, String>
                                //                         { 
                                //                            { "Child",  AppConsts.SHARED_USER_DASHBOARD}
                                //                            //,{"PageType","MyProfile"}
                                //                         };

                                //    String url = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                //    lnkUserName.NavigateUrl = url;
                                //    lnkEditProfile.HRef = url;
                                //}

                                //If user is shared user Modify the Masterpage Layout.
                                #region UAT 1349 Pending Design: Agency User Dashboard

                                apphdrwr.Visible = false;
                                rdpnTopRow.Height = 57;
                                rdpnTopRow.MinHeight = 0;

                                dvSharedUserMenu.Visible = true;
                                ddlUserTypeSwitchingView.Skin = "Silk";
                                ddlUserTypeSwitchingView.AutoSkinMode = false;


                                //Load CSS for Shared User

                                ltrlBootStrap.Text = "<link rel=\"stylesheet\" href=\"../Resources/Mod/Dashboard/Styles/bootstrap.min.css\" type='text/css'/>";
                                ltrlTitilliumFont.Text = "<link rel=\"stylesheet\" href=\"https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700\" type='text/css' />";
                                ltrlSUDashboard.Text = "<link rel=\"stylesheet\" href=\"../Resources/Mod/Dashboard/Styles/SharedUserHeader.css\" type='text/css'/>";
                                ltrlFontAwesome.Text = "<link rel=\"stylesheet\" href=\"../Resources/Mod/Dashboard/Styles/font-awesome.min.css\" type='text/css'/>";


                                //Set the navigation links for Shared User Menu
                                //queryString = new Dictionary<String, String>
                                //                                 { 
                                //                                    { "Child",  AppConsts.SHARED_USER_DASHBOARD}
                                // 

                                lnkHomeMenu.HRef = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                //UAT 1594 Agency User menu Options update: Add dropdown to Home for Rotation and Other.
                                HandleLinkseBySharedUser(user, queryString);


                                if (checkIfAgencyUser && user.SharedUserTypesCode.Contains(OrganizationUserType.AgencyUser.GetStringValue()))  //UAT-3364 -- report "liReports" need to visible =false, if all report permissions are = 2 (NO) (Need to create a method that return a bit that "IsAllReportsNotVisible").  
                                {
                                    AgencyUser agencyUser = Presenter.GetAgencyUserByUserID();
                                    Int32 templateId = Convert.ToInt32(agencyUser.AGU_TemplateId);
                                    AgencyUsrTempPermisisonsContract agencyUsrPermissions = new AgencyUsrTempPermisisonsContract();

                                    if (!agencyUser.AGU_TemplateId.IsNullOrEmpty() && agencyUser.AGU_TemplateId != 0)
                                    {
                                        agencyUsrPermissions = Presenter.GetAgencyUserPermisisonTemplateMappings(templateId);
                                    }
                                    else
                                    {
                                        agencyUsrPermissions = Presenter.GetAgencyUsrPermisisonMappings(agencyUser.AGU_ID);
                                    }

                                    if (!agencyUser.IsNullOrEmpty())
                                    {
                                        if (agencyUsrPermissions.AGU_RotationPackagePermission)
                                        {
                                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.MANAGE_ROTATION_PACKAGE}
                                                                 };

                                            lnkManagePackage.HRef = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                        }
                                        else
                                        {
                                            liManagePackage.Visible = false;
                                        }
                                        //UAT-2706
                                        if (agencyUsrPermissions.AGU_RotationPackageViewPermission)
                                        {
                                            liRequirementPackageView.Visible = true;
                                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.REQUIREMENT_PACKAGE_VIEW}
                                                                 };

                                            lnkRequirementPackageView.HRef = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                        }
                                        else
                                        {
                                            liRequirementPackageView.Visible = false;
                                        }

                                        //UAT-2427
                                        if (agencyUsrPermissions.AGU_AllowJobPosting)
                                        {
                                            liJobBoard.Visible = true;
                                            queryString = new Dictionary<String, String>
                                                               {
                                                                   {"Child", AppConsts.JOB_BOARD}
                                                               };
                                            lnkJobBoard.HRef = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                        }
                                        else
                                        {
                                            liJobBoard.Visible = false;
                                        }


                                        if (agencyUsrPermissions.AGU_AgencyUserPermission)
                                        {
                                            //get data for agency user
                                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.MANAGE_AGENCY_USERS}
                                                                 };

                                            lnkManageUser.HRef = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                        }
                                        else
                                        {
                                            liManageUser.Visible = false;
                                        }
                                        //UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
                                        if (agencyUsrPermissions.AGU_AttestationReport)
                                        {
                                            //Presenter.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.ATTESTATION_REPORT_TEXT_PERMISSION.GetStringValue());
                                            //Presenter.GetAgencyUserPermissionAccessTypeID(AgencyUserPermissionAccessType.YES.GetStringValue());

                                            //if (agencyUser.AgencyUserPermissions.Any(x => x.AUP_PermissionTypeID == AgencyUserPermissionTypeId && x.AUP_PermissionAccessTypeID == AgencyUserPermissionAccessTypeId && !x.AUP_IsDeleted))
                                            //{
                                            //get data for agency user
                                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.MANAGE_ATTESTATION_STATEMENT}
                                                                 };

                                            lnkRptAttTxtPrmsn.HRef = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                            liRptAttTxtPrmsn.Visible = true;
                                            //}
                                            //else
                                            //{
                                            //    liRptAttTxtPrmsn.Visible = false;
                                            //}
                                        }
                                        else
                                        {
                                            liRptAttTxtPrmsn.Visible = false;
                                        }

                                        //UAT-2548
                                        if (!agencyUsrPermissions.AGU_AgencyApplicantStatus)
                                        {
                                            lnkAgencyApplicantStatus.Visible = false;
                                        }

                                        #region UAT-3664:- Reports Visibility on the basis of permission

                                        if (!agencyUser.IsNullOrEmpty())
                                        {
                                            Presenter.IsAllReportsNotVisible(agencyUser.AGU_ID);
                                            //Presenter.GetAgencyUserReportPermissions(agencyUser.AGU_ID);
                                        }

                                        if (IsAllReportsNotVisible)
                                        {
                                            liReports.Visible = false;
                                        }
                                        else
                                        {
                                            List<AgencyUserReportPermissionContract> lstVisibleAgencyUserReports = new List<AgencyUserReportPermissionContract>();
                                            if (!lstAgencyUserReportPermissions.IsNullOrEmpty())
                                            {
                                                String noPermissionAccessType = AgencyUserPermissionAccessType.NO.GetStringValue();
                                                String yesPermissionAccessType = AgencyUserPermissionAccessType.YES.GetStringValue();
                                                List<Int32> lstNotVisibleReportsIds = new List<Int32>();
                                                //List<AgencyUserReportPermissionContract> lstNotVisibleReports = new List<AgencyUserReportPermissionContract>();
                                                lstNotVisibleReportsIds = lstAgencyUserReportPermissions.Where(con => con.PermissionAccessTypeCode == noPermissionAccessType).Select(sel => sel.AgencyUserReportID).ToList();
                                                //lstNotVisibleReports = lstAgencyUserReportPermissions.Where(con => con.PermissionAccessTypeCode == noPermissionAccessType).ToList();
                                                lstVisibleAgencyUserReports = lstAgencyUserReports.Where(c => !lstNotVisibleReportsIds.Contains(c.AUR_ID)).Select(x => new AgencyUserReportPermissionContract
                                                {
                                                    AgencyUserReportID = x.AUR_ID,
                                                    AgencyUserReportCode = x.AUR_Code,
                                                    AgencyUserReportFolderPath = x.AUR_ReportFolderPath,
                                                    AgencyUserReportModule = x.AUR_ReportModule,
                                                    ReportName = x.AUR_Name
                                                }).ToList();
                                            }

                                            else //if agency user does not have any report permission access.
                                            {
                                                lstVisibleAgencyUserReports = lstAgencyUserReports.Select(x =>
                                                     new AgencyUserReportPermissionContract
                                                     {
                                                         AgencyUserReportID = x.AUR_ID,
                                                         AgencyUserReportCode = x.AUR_Code,
                                                         AgencyUserReportFolderPath = x.AUR_ReportFolderPath,
                                                         AgencyUserReportModule = x.AUR_ReportModule,
                                                         ReportName = x.AUR_Name,
                                                     }).ToList();
                                            }
                                            ManageAgencyUserReports(lstVisibleAgencyUserReports);
                                        }

                                        #endregion

                                        #region Below code is commented in UAT-3664

                                        //queryString = new Dictionary<String, String>
                                        //                         {
                                        //                            { "Child",  @"~\Reports\InstitutionCount.ascx"}
                                        //                         };

                                        //lnkAgencyUserReport.HRef = String.Format("~/Reports/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                                        //queryString = new Dictionary<String, String>
                                        //                         {
                                        //                            { "Child",  @"~\Reports\RequirementItemDateReport.ascx"}
                                        //                         };

                                        //lnkItemDateReport.HRef = String.Format("~/Reports/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());


                                        //queryString = new Dictionary<String, String>
                                        //                         {
                                        //                            { "Child",  @"~\ProfileSharing\UserControl\AttestationReportsWithoutSignature.ascx"}
                                        //                         };

                                        //lnkAttestationDocumentReport.HRef = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                        //lnkAttestationDocumentReport.Visible = true;

                                        //queryString = new Dictionary<String, String>
                                        //                         {
                                        //                            { "Child",  @"~\Reports\RotationStudentDetails.ascx"}
                                        //                         };
                                        //lnkRotationStudentDetails.HRef = String.Format("~/Reports/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                                        //Already commented
                                        //queryString = new Dictionary<String, String>
                                        //                         { 
                                        //                            { "Child",  @"~\Reports\StudentCountReport.ascx"}
                                        //                         };
                                        //lnkStudentCountReport.HRef = String.Format("~/Reports/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                                        //queryString = new Dictionary<String, String>
                                        //                         {
                                        //                            { "Child",  @"~\Reports\AgencyAdminsByDepartment.ascx"}
                                        //                         };

                                        //lnkAgencyAdminsByDepartment.HRef = String.Format("~/Reports/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                                        //queryString = new Dictionary<String, String>
                                        //                         {
                                        //                            { "Child",  @"~\Reports\ItemDataCountReport.ascx"}
                                        //                         };

                                        //lnkItemDataCountReport.HRef = String.Format("~/Reports/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                                        //UAT-3052
                                        //queryString = new Dictionary<String, String>
                                        //                         {
                                        //                            { "Child",  @"~\CommonOperations\SavedReports.ascx"}
                                        //                         };

                                        //lnkSaveReports.HRef = String.Format("~/CommonOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                                        //UAT-3146
                                        //queryString = new Dictionary<String, String>
                                        //                         {
                                        //                            { "Child",  @"~\Reports\RotationStudentsComplianceStatus.ascx"}
                                        //                         };

                                        //lnkRotationStudentsComplianceStatus.HRef = String.Format("~/Reports/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                                        //UAT-3146
                                        //queryString = new Dictionary<String, String>
                                        //                         {
                                        //                            { "Child",  @"~\Reports\RotationStudentsOverallNonComplianceStatus.ascx"}
                                        //                         };

                                        //lnkRotationStudentsOverallNonComplianceStatus.HRef = String.Format("~/Reports/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                                        //UAT-3143
                                        //queryString = new Dictionary<String, String>
                                        //                         {
                                        //                            { "Child",  @"~\Reports\CategoryDataReport.ascx"}
                                        //                         };

                                        //lnkCategoryDataReport.HRef = String.Format("~/Reports/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                                        //UAT-3214
                                        //queryString = new Dictionary<String, String>
                                        //                         {
                                        //                            { "Child",  @"~\Reports\RotationStudentsByDayoftheWeek.ascx"}
                                        //                         };

                                        //lnkRotationStudentByDayoftheWeek.HRef = String.Format("~/Reports/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                                        //UAT-3580 : Tri City Medical Center Custom Report
                                        //queryString = new Dictionary<String, String>
                                        //                         {
                                        //                            { "Child",  @"~\Reports\CategoryDataReportByComplioId.ascx"}
                                        //                         };

                                        //lnkCategoryDataReportByComplioId.HRef = String.Format("~/Reports/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                                        #endregion

                                        //UAT-3319
                                        queryString = new Dictionary<String, String>
                                                                 {
                                                                      { "Child",  AppConsts.SCHOOL_REPRESENTATIVE_DETAIL}
                                                                 };

                                        lnkSchoolRepresentativeDetails.HRef = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                        lnkSchoolRepresentativeDetails.Visible = true;

                                        //UAT-3615 : Placement Matching
                                        #region PlacementMatching
                                        Boolean isPlacementTestingModeOn = false;
                                        if (!ConfigurationManager.AppSettings["PlacementMatchingTestingModeON"].IsNullOrEmpty())
                                        {
                                            isPlacementTestingModeOn = Convert.ToBoolean(ConfigurationManager.AppSettings["PlacementMatchingTestingModeON"]);
                                        }

                                        if (isPlacementTestingModeOn)  // need to change on the basis on permission// agencyUsrPermissions.AGU_AllowJobPosting
                                        {
                                            liPlacementMatching.Visible = true;
                                            liAgencyLocationSetup.Visible = true;
                                            liManageOpportunities.Visible = true;
                                            liPlacementDashboard.Visible = true;
                                            liManageCustomAttributes.Visible = true;

                                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  @"~\PlacementMatching\AgencyLocationDepartment.ascx"}
                                                                 };
                                            lnkAgencyLocationSetup.HRef = String.Format("~/PlacementMatching/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  @"~\PlacementMatching\ManageOpportunity.ascx"}
                                                                 };
                                            lnkManageOppotunities.HRef = String.Format("~/PlacementMatching/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  @"~\PlacementMatching\ManageRequest.ascx"}
                                                                 };
                                            lnkManageRequest.HRef = String.Format("~/PlacementMatching/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  @"~\PlacementMatching\AgencyUserPlacementDashboard.ascx"}
                                                                 };
                                            lnkPlacementDashboard.HRef = String.Format("~/PlacementMatching/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  @"~\PlacementMatching\SharedCustomAttributes.ascx"}
                                                                 };
                                            lnkManageCustomAttributes.HRef = String.Format("~/PlacementMatching/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                                        }
                                        else
                                        {
                                            liPlacementMatching.Visible = false;
                                            liAgencyLocationSetup.Visible = false;
                                            liManageOpportunities.Visible = false;
                                            liManageRequest.Visible = false;
                                            liPlacementDashboard.Visible = false;
                                            liManageCustomAttributes.Visible = false;
                                        }

                                        #endregion

                                        //UAT-4006 : Acgency current non-compliant search
                                        queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.REQUIREMENT_NON_COMPLIANT_SEARCH}
                                                                 };

                                        lnkAgencyNonComplaintSearch.HRef = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());


                                    }
                                }
                                else
                                {
                                    liManagePackage.Visible = false;
                                    liManageUser.Visible = false;//UAT-3208
                                    liReports.Visible = false;   //UAT-3664 -- Need to be false 
                                    liRequirementPackageView.Visible = false;
                                    liRptAttTxtPrmsn.Visible = false;
                                    liJobBoard.Visible = false;
                                    liAgencyLocationSetup.Visible = false;
                                    liPlacementMatching.Visible = false;
                                    liManageOpportunities.Visible = false;
                                    liManageRequest.Visible = false;
                                    liPlacementDashboard.Visible = false;
                                    liManageCustomAttributes.Visible = false;
                                    ////UAT-2548
                                    //liAgencyApplicantStatus.Visible = false;
                                }
                                #endregion
                            }

                            //Setting iframe as a target 
                            lnkUserName.Target = "pageFrame";

                            //dvAutoLogOut.Visible = false;
                            divInstitute.Visible = false;
                            divTenantName.Visible = false;
                            divTenantNameForClientAdmin.Visible = false;
                            divClientAdminTenant.Visible = false;

                            if (user.IncorrectLoginUrlUsed)
                                RegisterIncorrectUrlMsgScript(_url, user);
                            #endregion
                        }

                        else //else Client Admin Or Third party/Compliance Reviewer Or ADB Admins
                        {
                            #region ADB ADMINS AND CLIENT ADMINS
                            lnkUserName.Enabled = true;
                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", @"~/IntsofSecurityModel/UserControl/AdminEditProfile.ascx"},
                                                                 };
                            lnkUserName.NavigateUrl = String.Format("~/IntsofSecurityModel/default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                            //Setting iframe as a target 
                            lnkUserName.Target = "pageFrame";

                            //Adding tooltip for the user profile link only if link is enabled
                            if (!string.IsNullOrWhiteSpace(lnkUserName.NavigateUrl))
                            {
                                lnkUserName.ToolTip = "Click to view your profile";
                            }

                            //UAT 712
                            //lnkChangePassword.Enabled = true;
                            //queryString = RedirectToChangePassword(queryString);
                            if (user.TenantId != Presenter.GetDefaultTenantID() && user.TenantTypeCode.Equals(TenantType.Institution.GetStringValue()))
                            {
                                divClientAdminTenant.Visible = true;
                                // divTenantName.Visible = false;
                                //dvPreferredTenant.Visible = false; //UAT:-3032
                            }
                            else
                            {
                                divClientAdminTenant.Visible = false;
                                //dvPreferredTenant.Visible = true; //UAT:-3032
                                if (!IsPostBack)
                                    BindPreferredTenant(); //UAT:-3032
                            }
                            if (user.IncorrectLoginUrlUsed && user.TenantTypeCode == TenantType.Institution.GetStringValue())
                                RegisterIncorrectUrlMsgScript(_url, user);
                            #endregion
                        }

                        #region User Type Switch View
                        if (!IsPostBack)
                        {
                            BindUserTypeSwitchView(user);
                        }
                        #endregion
                    }
                    //APPLICATION lOGO Navigate Url
                    if (!showMinDetails)
                        linkHeader.NavigateUrl = AppConsts.APPLICANT_MAIN_PAGE_NAME;
                }

                Response.Buffer = true;
                Response.Cache.SetNoStore();

                // added config setting to ensure whether to show build number label or not
                Boolean showBuildNumber = Convert.ToBoolean(WebConfigurationManager.AppSettings["ShowBuildNumber"]);
                if (showBuildNumber.IsNullOrEmpty() || !showBuildNumber)
                {
                    lblVersionNumber.Visible = false;
                    lblDT.Visible = false;
                    lblPipe.Visible = false;
                }
                else
                {
                    lblVersionNumber.Visible = true;
                    lblDT.Visible = true;
                    lblPipe.Visible = true;
                    lblVersionNumber.Text = Resources.GlobalResource.SysXWebAppVersion;
                    lblDT.Text = dt;
                }

                if (!_sessionService.IsNull() && !_sessionService.SysXMembershipUser.IsNull())
                {
                    BuildMenus(true);
                }

                if (!this.IsPostBack)
                {
                    if (!showMinDetails)
                    {
                        BindInstitutes();
                    }
                    // BindPreferredTenant(); //UAT:-3032
                    Presenter.OnViewInitialized();
                    GenerateFooter();

                    //Code to generate link for live chat
                    var chatUrl = "";
                    chatUrl = System.Configuration.ConfigurationManager.AppSettings["LiveChatUrl"];

                    liveChatScript.Text = "<script>";
                    liveChatScript.Text += "var __chatbuttonID = '" + lnkLiveChat.ClientID + "';";
                    liveChatScript.Text += "var __chatURL = '" + chatUrl + "';";
                    liveChatScript.Text += "</script>";

                    RedirectToReactApp();
                }

                Presenter.OnViewLoaded();
                //UAT 1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                {
                    //Hide the user type switch dropdown.
                    dvSwitchViewDropdown.Visible = false;
                }

                //Admin Entry Portal
                //RedirectToReactApp();
                //end

            }
            catch (System.Exception ex)
            {
                SysXWebSiteUtils.LoggerService.GetLogger().Error("appMaster.cs, PageLoad", ex);
            }
        }

        /// <summary>
        /// Event handler. Called by rdpnbLeftPanel for item created events.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">     The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void rdpnbLeftPanel_ItemCreated(object sender, RadPanelBarEventArgs e)
        {
            e.Item.Target = "pageFrame";
            e.Item.Expanded = true;
        }

        /// <summary>
        /// Handles the Click event of the btnLogoff control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void btnLogoff_Click(object sender, EventArgs e)
        {
            try
            {
                SysXMembershipUser currentLoggedInUser = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                Int32 userLoginHistoryID = 0;

                if (!currentLoggedInUser.IsNullOrEmpty() && currentLoggedInUser.OrganizationUserId > 0)
                    userLoginHistoryID = currentLoggedInUser.UserLoginHistoryID;

                Int32 currentLogedInUserId = CurrentOrgUserId;
                String logInfo = String.Empty;
                logInfo += logInfo + "**Entered Into LogOff click.";
                if (!CurrentSessionId.IsNullOrEmpty())
                {
                    ViewStateProvider.Delete(CurrentSessionId);
                }
                SysXWebSiteUtils.SessionService.ClearSession(true);

                SysXWebSiteUtils.AllClientSessionService.IsAlumniRedirectionDue = false;
                SysXWebSiteUtils.AllClientSessionService.ClearSession(true);

                //Session.Abandon();
                // Remove the forms-authentication ticket from the browser
                logInfo += logInfo + "**Entered Into FormsAuthentication.SignOut.**";
                FormsAuthentication.SignOut();
                SysXAppDBEntities.ClearContext();
                Presenter.UpdateUserLoginActivity(IsSessionExpired, currentLogedInUserId, userLoginHistoryID);
                if (IsSessionExpired)
                {
                    logInfo += logInfo + "**Entered Into IsSessionExpired.**";
                    Dictionary<String, String> encryptedQueryStrings = new Dictionary<String, String> { { AppConsts.SESSION_EXPIRED, "success" } };
                    String queryString = String.Format(AppConsts.SESSION_EXPIRED + "={0}", encryptedQueryStrings.ToEncryptedQueryString());
                    // Redirects the browser to the login URL
                    FormsAuthentication.RedirectToLoginPage(queryString);
                    logInfo += logInfo + "**RedirectToLoginPage.**";
                }
                else
                {
                    FormsAuthentication.RedirectToLoginPage();
                    logInfo += logInfo + "**RedirectToLoginPage.**";
                }
                CoreWeb.Shell.SysXWebSiteUtils.LoggerService.GetLogger().Info(logInfo);
                //Response.Redirect(FormsAuthentication.LoginUrl);

            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
            catch (System.Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        /// <summary>
        /// Handles the Init event of the rsrMgr control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void rsrMgr_Init(object sender, EventArgs e)
        {
            try
            {
                String _userPreferenceTheme = String.Empty;

                _userPreferenceTheme = (String)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_THEME);
                //_userPreferenceTheme = "Default";

                string themeSection = System.Configuration.ConfigurationManager.AppSettings["ThemeSection"];
                string defaultTheme = System.Configuration.ConfigurationManager.AppSettings["DefaultTheme"];

                WclTheme currentTheme = (System.Configuration.ConfigurationManager.GetSection(themeSection) as WclThemeSection).Themes[_userPreferenceTheme];
                if (currentTheme == null)
                {
                    _userPreferenceTheme = defaultTheme;
                    currentTheme = (System.Configuration.ConfigurationManager.GetSection(themeSection) as WclThemeSection).Themes[_userPreferenceTheme];
                }
                rsrMgr.ThemeName = _userPreferenceTheme;
                rsrMgr.SkinCollection = !currentTheme.IsNull() ? currentTheme.Skins : null;
            }
            catch (System.Exception ex)
            {
                SysXWebSiteUtils.LoggerService.GetLogger().Error("appMaster.cs, Resource Manager Initialisation", ex);
                SysXWebSiteUtils.ExceptionService.HandleError("Unable to build menus for user : " + SysXWebSiteUtils.SessionService.SysXMembershipUser.UserName, ex);
            }
        }

        /// <summary>
        /// Event handler. Called by rdmnMainMenu for item created events.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">     The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void rdmnMainMenu_ItemCreated(object sender, RadMenuEventArgs e)
        {
            e.Item.Target = "pageFrame";
        }

        /// <summary>
        /// Calls when Tenant Name Or Institute changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;

                if (user.IsApplicant && Convert.ToInt32(SelectedTenantId) > 0 && TenantId > 0 && Convert.ToInt32(SelectedTenantId) != TenantId)
                {

                    Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
                    ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
                    appInstData.UserID = CurrentUserId;
                    appInstData.TagetInstURL = Presenter.GetInstitutionUrl();
                    appInstData.TokenCreatedTime = DateTime.Now;
                    appInstData.TenantID = Convert.ToInt32(SelectedTenantId);

                    //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
                    if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                    {
                        appInstData.AdminOrgUserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                    }

                    String key = Guid.NewGuid().ToString();
                    Dictionary<String, ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
                    //if (Application["ApplicantInstData"] != null)
                    if (applicationData != null)
                    {
                        //applicantData = (Dictionary<String, ApplicantInsituteDataContract>)applicationData;
                        applicantData = applicationData;
                        applicantData.Add(key, appInstData);
                        Presenter.UpdateWebApplicationData("ApplicantInstData", applicantData);
                    }
                    else
                    {
                        applicantData.Add(key, appInstData);
                        //Application["ApplicantInstData"] = applicantData;
                        Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
                    }

                    //Log out from application then redirect to selected tenant url, append key in querystring.
                    // On login page get data from Application Variable.

                    // Check the user's authentication.
                    Presenter.DoLogOff(!user.IsNull(), user.UserLoginHistoryID);
                    //Redirect to login page
                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TokenKey", key  }
                                                                 };
                    Response.Redirect(String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}", key));
                    //Response.Redirect(String.Format("~/Login.aspx?TokenKey={0}", key));
                }
                else if (SysXWebSiteUtils.SessionService.IsSysXAdmin)
                {

                    SysXWebSiteUtils.SessionService.BusinessChannelType =
                        new BusinessChannelTypeMappingData
                        {
                            BusinessChannelTypeID = Convert.ToInt16(ddlTenantName.SelectedValue),
                            BusinessChannelTypeName = ddlTenantName.SelectedItem.Text// .SelectedText
                        };

                    this.MenuFeatures = null;
                    BuildMenus(true);

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "RedirectToDashboard", "<script>document.getElementById('linkHeader').click();</script>");
                    //bool throwex = true;
                    //if (throwex)
                    //{
                    //    throw new Exception("pks test exception");
                    //}
                }
                else
                {
                    String[] selectedChannel = ddlTenantName.SelectedValue.Split('#');

                    SysXWebSiteUtils.SessionService.BusinessChannelType =
                        new BusinessChannelTypeMappingData
                        {
                            BusinessChannelTypeID = Convert.ToInt16(selectedChannel[AppConsts.ONE]),
                            BusinessChannelTypeName = Presenter.GetBusinessChannelType()
                            .FirstOrDefault(cond => cond.BusinessChannelTypeID == Convert.ToInt16(selectedChannel[AppConsts.ONE])).Name
                        };

                    _blockId = Convert.ToInt32(selectedChannel[0]);
                    SysXWebSiteUtils.SessionService.SysXBlockId = _blockId;
                    this.MenuFeatures = null;

                    BuildMenus(true);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "RedirectToDashboard", "<script>document.getElementById('linkHeader').click();</script>");
                }
            }
            catch (System.Exception ex)
            {
                Response.Write(ex.Message);
                SysXWebSiteUtils.LoggerService.GetLogger().Error("appMaster.cs, ddlTenantName_SelectedIndexChanged, Unable to find the cached item", ex);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "keyLogout", "<script>document.getElementById('btnLogoff').click();</script>");
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Refreshes the menu items.
        /// </summary>
        /// <remarks></remarks>
        public void RefreshMenuItems(String selectedUser)
        {
            try
            {
                ((SysXSiteMapProvider)SiteMap.Provider).RefreshSiteMap();
                //in order to rebuild the security menus we need the below code for refreshing
                //also need to work on just refreshing the part that is required (either the top menus or the left menus) 
                //as we will not have a situation to refresh both menu's simultaniously once the initial load is complete            
                foreach (SiteMapProvider newprovider in SiteMap.Providers)
                {
                    if (newprovider.Name == ApplicationConstant.SecuritySiteMapProvider)
                    {
                        ((SysXSecuritySiteMapProvider)newprovider).RefreshSiteMap();
                        break;
                    }
                }
                BuildMenus();
                updMenu.Update();
                //pnllBlockText.Update();
            }
            catch (System.Exception ex)
            {
                SysXWebSiteUtils.LoggerService.GetLogger().Error("appMaster.cs, RefreshMenuItem", ex);
            }
        }

        //protected override void OnPreRender(EventArgs e)
        //{
        //    if (this.IsPostBack)
        //    {
        //        Dictionary<String, String> assetLegendOptions = _sessionService.GetCustomData(SysXAssetMasterConst.ASSET_LEGEND) as Dictionary<String, String>;
        //        if (!SysXSiteMapProvider.SecurityService.IsNull())
        //        {
        //            if (SysXSiteMapProvider.SecurityService.IsMenuRefreshRequired && assetLegendOptions.IsNull())//Mode:End
        //            {
        //                RefreshMenuItems(null);
        //                SysXSiteMapProvider.SecurityService.IsMenuRefreshRequired = false;
        //            }
        //            else
        //            {
        //                LoadAssetLegend(false);
        //            }
        //        }
        //        else
        //        {
        //            RefreshMenuItems(null);
        //        }
        //    }

        //}       

        /// <summary>
        /// Register postback controls inside an UpdatePanel control as triggers. 
        /// Controls that are registered by using this method update a whole page instead of updating only the UpdatePanel control's content
        /// </summary>
        /// <param name="registerControl"></param>
        public void RegisterControlForPostBack(Control registerControl)
        {
            scmApp.RegisterPostBackControl(registerControl);
        }

        /// <summary>
        /// Displays a side panels.
        /// </summary>
        /// <param name="left"> (optional) the left.</param>
        /// <param name="right">(optional) the right.</param>
        public void DisplaySidePanels(Boolean left = true, Boolean right = false)
        {
            //rdpnLeftCol.Collapsed = !left;
            //rdpnRightCol.Collapsed = !right;
        }

        #endregion

        #region Private Methods

        private void BuildMenus()
        {
            try
            {
                //we need to bind the DataSourceID of the menu controls to the appropriate siteMapProviders
                if (!SysXSiteMapProvider.SecurityService.IsNull())
                {
                    if (rdmnMainMenu.Items.Count <= 0 || SysXSiteMapProvider.SecurityService.IsMenuRefreshRequired == true)
                    {
                        rdmnMainMenu.DataSourceID = null;
                        rdmnMainMenu.DataSource = this.ApplicationSiteMap;
                        rdmnMainMenu.DataBind();

                        //
                        // Following code not required in the new layout
                        // -->

                        //if (rdmnMainMenu.Items.Count <= 0)
                        //{
                        //    rdpnTopRow.Height = System.Web.UI.WebControls.Unit.Pixel(80);
                        //    rdpnTopRow.MinHeight = 80;
                        //    rdmnMainMenu.Visible = false;
                        //}
                    }
                }
                else
                {
                    rdmnMainMenu.DataSourceID = null;
                    rdmnMainMenu.DataSource = this.ApplicationSiteMap;
                    rdmnMainMenu.DataBind();
                }

                //if (rdpnbLeftPanel.Items.Count <= 0)
                {
                    //rdpnbLeftPanel.DataSource = SecuritySiteMap;
                    //rdpnbLeftPanel.DataBind();
                }

                //AD: Loading setup menu
                if (SecuritySiteMap != null)
                {

                    if (SecuritySiteMap.Provider.RootNode.ChildNodes.Count > 0)
                    {
                        foreach (SiteMapNode node in SecuritySiteMap.Provider.RootNode.ChildNodes[0].ChildNodes)
                        {
                            mnSystemSetup.Items[0].Items.Add(new RadMenuItem() { NavigateUrl = node.Url, Text = node.Title, Target = "pageFrame" });
                        }

                        //pnLeft.Collapsed = false;
                    }
                    else
                    {
                        mnSystemSetup.Items[0].Visible = false;
                        //rdpnbLeftPanel.Items[0].Visible = false;
                        //pnLeft.Collapsed = true;
                    }
                }


                //rdpnbLeftPanel.Items.Clear();

                //SiteMapProvider sitemapProvider = SecuritySiteMap.Provider;
                //String sysFunKey = SysXWebSiteUtils.SecurityService.GetSysXSystemFunctionKey();
                //Dictionary<String, String> assetLegendOptions = _sessionService.GetCustomData(SysXAssetMasterConst.ASSET_LEGEND) as Dictionary<String, String>;                

                //for (int nodeCount = sitemapProvider.RootNode.ChildNodes.Count - 1; nodeCount >= 0; nodeCount--)
                //{
                //    if (sitemapProvider.RootNode.ChildNodes[nodeCount].Key == sysFunKey)
                //    {
                //        RadPanelItem menuItem = new RadPanelItem(sitemapProvider.RootNode.ChildNodes[nodeCount].Title, sitemapProvider.RootNode.ChildNodes[nodeCount].Url);
                //        menuItem.ToolTip = sitemapProvider.RootNode.ChildNodes[nodeCount].Description;

                //        if (sitemapProvider.RootNode.ChildNodes[nodeCount].ChildNodes.Count > SysXConsts.NONE)
                //        {
                //            BuildChildPanelMenus(sitemapProvider.RootNode.ChildNodes[nodeCount], menuItem);
                //        }

                //        menuItem.Expanded = true;
                //        rdpnbLeftPanel.Items.Add(menuItem);
                //    }
                //}

                DisplaySidePanels();
            }
            catch (System.Exception ex)
            {
                SysXWebSiteUtils.LoggerService.GetLogger().Error("appMaster.cs, BuildMenus", ex);
                SysXWebSiteUtils.ExceptionService.HandleError("Unable to build menus for user : " + SysXWebSiteUtils.SessionService.SysXMembershipUser.UserName, ex);
            }
        }

        private void SessionExpRedirectToLoginPage(String sessionExp)
        {

            ISysXSessionService sessionService = (HttpContext.Current.ApplicationInstance as IWebApplication).SysXSessionService;

            sessionService.ClearSession(true);
            FormsAuthentication.SignOut();
            SysXAppDBEntities.ClearContext();
            Response.Redirect(FormsAuthentication.LoginUrl, false);
        }

        private void BuildChildPanelMenus(SiteMapNode siteMapNode, RadPanelItem parentMenuItem)
        {
            //foreach (SiteMapNode node in siteMapNode.ChildNodes)
            //{
            //    RadPanelItem menuItem = new RadPanelItem(node.Title, node.Url);
            //    menuItem.ToolTip = node.Description;
            //    parentMenuItem.Items.Add(menuItem);

            //    if (node.ChildNodes.Count > SysXConsts.NONE)
            //    {
            //        BuildChildPanelMenus(node, menuItem);
            //    }
            //}

            for (int nodeCount = siteMapNode.ChildNodes.Count - 1; nodeCount >= 0; nodeCount--)
            {
                RadPanelItem menuItem = new RadPanelItem(siteMapNode.ChildNodes[nodeCount].Title, siteMapNode.ChildNodes[nodeCount].Url);
                menuItem.ToolTip = siteMapNode.ChildNodes[nodeCount].Description;
                parentMenuItem.Items.Add(menuItem);

                if (siteMapNode.ChildNodes[nodeCount].ChildNodes.Count > AppConsts.NONE)
                {
                    BuildChildPanelMenus(siteMapNode.ChildNodes[nodeCount], menuItem);
                }
            }
        }

        /// <summary>
        /// To bind institutes or tenants
        /// </summary>
        private void BindInstitutes()
        {
            ddlTenantName.DataSource = Presenter.GetTenantData();
            ddlTenantName.DataBind();
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            if (user.IsNotNull()
                && !user.IsApplicant
                && !user.IsSystem
                && !user.IsSysXAdmin
                && user.TenantId != Presenter.GetDefaultTenantID()
                && (!user.IsSharedUser || !IsSharedUserLoginURL))//UAT-1110-SharedUser-check for shared user
            {
                clientAdminddlTenantName.DataSource = Presenter.GetTenantDataForClientAdmin();
                clientAdminddlTenantName.DataBind();
            }

            if (SysXWebSiteUtils.SessionService.IsSysXAdmin && SysXWebSiteUtils.SessionService.BusinessChannelType.IsNullOrEmpty())
            {
                SysXWebSiteUtils.SessionService.BusinessChannelType =
                    new BusinessChannelTypeMappingData
                    {
                        BusinessChannelTypeID = Convert.ToInt16(ddlTenantName.SelectedValue),
                        BusinessChannelTypeName = ddlTenantName.SelectedItem.Text// .SelectedText
                    };
            }

            //Bug Fix//Admin Entry Portal
            if (!SysXWebSiteUtils.SessionService.BusinessChannelType.IsNullOrEmpty())
            {
                Int32 businessChannelType = SysXWebSiteUtils.SessionService.BusinessChannelType.BusinessChannelTypeID;
                ddlTenantName.SelectedValue = businessChannelType.ToString();
            }
        }

        /// <summary>
        /// Register script for Incorrect Url Message
        /// </summary>
        /// <param name="url"></param>
        /// <param name="user"></param>
        private void RegisterIncorrectUrlMsgScript(String url, SysXMembershipUser user)
        {
            String _script = "function onHidden(){ alert('You tried to login into Complio using another institution’s site."
                        + " Please bookmark and use " + url + " for your next visit to Complio');}";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "IncorrectUrlMsg", _script, true);
            user.IncorrectLoginUrlUsed = false;
        }


        #region Private Methods

        /// <summary>
        /// Loads the asset legend.
        /// </summary>
        private void LoadAssetLegend(Boolean isClear)
        {
            //try
            //{
            //    rdpnbLeftPanel.DataSource = SecuritySiteMap;
            //    rdpnbLeftPanel.DataBind();

            //    Dictionary<String, String> assetLegendOptions = _sessionService.GetCustomData(SysXAssetMasterConst.ASSET_LEGEND) as Dictionary<String, String>;

            //    if (!assetLegendOptions.IsNull())
            //    {
            //        // Build Panel Bars
            //        SiteMapProvider sitemapProvider = SecuritySiteMap.Provider;
            //        String sysFunKey = SysXWebSiteUtils.SecurityService.GetSysXSystemFunctionKey();
            //        ////rdpnbLeftPanel.DataSourceID = null;
            //        //rdpnbLeftPanel.Items.Clear();

            //        //foreach (SiteMapNode node in sitemapProvider.RootNode.ChildNodes)
            //        //{
            //        //    if (node.Key.Equals(sysFunKey))
            //        //    {
            //        //        RadPanelItem menuItem = new RadPanelItem(node.Title, node.Url);
            //        //        menuItem.ToolTip = node.Description;

            //        //        if (node.ChildNodes.Count > SysXConsts.NONE)
            //        //        {
            //        //            BuildChildPanelMenus(node, menuItem);
            //        //        }

            //        //        menuItem.Expanded = true;
            //        //        rdpnbLeftPanel.Items.Add(menuItem);
            //        //    }
            //        //}

            //        //for (int nodeCount = sitemapProvider.RootNode.ChildNodes.Count - 1; nodeCount >= 0; nodeCount--)
            //        //{
            //        //    if (sitemapProvider.RootNode.ChildNodes[nodeCount].Key == sysFunKey)
            //        //    {
            //        //        RadPanelItem menuItem = new RadPanelItem(sitemapProvider.RootNode.ChildNodes[nodeCount].Title, sitemapProvider.RootNode.ChildNodes[nodeCount].Url);
            //        //        menuItem.ToolTip = sitemapProvider.RootNode.ChildNodes[nodeCount].Description;

            //        //        if (sitemapProvider.RootNode.ChildNodes[nodeCount].ChildNodes.Count > SysXConsts.NONE)
            //        //        {
            //        //            BuildChildPanelMenus(sitemapProvider.RootNode.ChildNodes[nodeCount], menuItem);
            //        //        }

            //        //        menuItem.Expanded = true;
            //        //        rdpnbLeftPanel.Items.Add(menuItem);
            //        //    }
            //        //}

            //        DisplaySidePanels();
            //        if (isClear == true)
            //        {
            //            SysXWebSiteUtils.SessionService.SetCustomData(SysXAssetMasterConst.ASSET_LEGEND, null);
            //        }

            //        Int32 assetID = Convert.ToInt32(assetLegendOptions[SysXAssetMasterConst.ASSET_LEGEND_ASSETID]);
            //        Int32 loanID = SysXConsts.NONE;
            //        Int32 assetUnitID = SysXConsts.NONE;
            //        if (assetLegendOptions.ContainsKey(SysXAssetMasterConst.ASSET_LEGEND_LOANID))
            //        {
            //            loanID = Convert.ToInt32(assetLegendOptions[SysXAssetMasterConst.ASSET_LEGEND_LOANID]);
            //        }

            //        if (!Session["LoanId"].IsNullOrEmpty())
            //        {
            //            loanID = Convert.ToInt32(Session["LoanId"]);

            //        }
            //        if (assetLegendOptions.ContainsKey(SysXAssetMasterConst.ASSET_LEGEND_ASSETUNITID))
            //        {
            //            assetUnitID = Convert.ToInt32(assetLegendOptions[SysXAssetMasterConst.ASSET_LEGEND_ASSETUNITID]);
            //        }

            //        AssetLegendContract assetLegendContract = this.Presenter.GetAssetLegendInformation(assetID, assetUnitID, loanID);

            //        if (!assetLegendContract.IsNull())
            //        {
            //            // Create Asset Legend based on Latest Information
            //            AddCriticalDates(assetLegendContract); // To Add Critical Dates
            //            AddReoInfo(assetLegendContract);     // To Add Reo Information
            //            AddPropertyInfo(assetLegendContract); // To Add Property Details
            //            AddLoanInfo(assetLegendContract); // To Loan Details
            //            AddPropertyAddress(assetLegendContract); // To Add Property Address
            //        }

            //        SysXSiteMapProvider.SecurityService.IsMenuRefreshRequired = true;
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    SysXWebSiteUtils.LoggerService.GetLogger().Error("appMaster.cs, LoadAssetLegend", ex);
            //}
        }

        /// <summary>
        /// Add Property Address to Asset Legend
        /// </summary>
        /// <param name="assetLegendContract"></param>
        //private void AddPropertyAddress(AssetLegendContract assetLegendContract)
        //{
        //    RadPanelItem radPanelItemPropInfo = new RadPanelItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_PROPERTY_INFORMATION));

        //    radPanelItemPropInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_ADDRESS1), assetLegendContract.PropertyStreetAddress1));
        //    radPanelItemPropInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_CITY), assetLegendContract.PropertyCity));
        //    radPanelItemPropInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_STATE), assetLegendContract.PropertyState));
        //    radPanelItemPropInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_ZIP), assetLegendContract.PropertyZip));
        //    radPanelItemPropInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_PARCEL_ID), assetLegendContract.ParcelId));

        //    // TODO: Required Asset Type Enumerator to show and hide it
        //    radPanelItemPropInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_MOBILE_VIN), assetLegendContract.MobileHomeVin));
        //    radPanelItemPropInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_PROPERTY_TYPE), assetLegendContract.PropertyType));
        //    radPanelItemPropInfo.Expanded = true;

        //    //var oldItem=rdpnbLeftPanel.FindItemByText(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_PROPERTY_INFORMATION));
        //    //if (!oldItem.IsNull())
        //    //{
        //    //    int index = rdpnbLeftPanel.Items.IndexOf(oldItem);
        //    //    rdpnbLeftPanel.Items.Insert(index, radPanelItemPropInfo);
        //    //}
        //    //else
        //    //{
        //    //    rdpnbLeftPanel.Items.Insert(SysXConsts.NONE, radPanelItemPropInfo);
        //    //}            
        //    rdpnbLeftPanel.Items.Insert(SysXConsts.NONE, radPanelItemPropInfo);

        //}

        /// <summary>
        /// Add Loan Details to Asset Legend
        /// </summary>
        /// <param name="assetLegendContract"></param>
        //private void AddLoanInfo(AssetLegendContract assetLegendContract)
        //{
        //    RadPanelItem radPanelItemLoanInfo = new RadPanelItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_LOAN_DETAILS));

        //    // Add Blank Panel Item
        //    RadPanelItem Item = new RadPanelItem();
        //    SysXTreeView tree = new SysXTreeView();
        //    Item.Controls.Add(tree);
        //    radPanelItemLoanInfo.Items.Add(Item);

        //    // Build Tree Items
        //    foreach (var loanItem in assetLegendContract.LoanDetails)
        //    {
        //        RadTreeNode radPanelItemLoanInfoItem = new RadTreeNode(loanItem.ToString());
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_CLIENT_NAME), loanItem.ClientName));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_CLIENT_ID), loanItem.ClientId));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_CLIENT_LSCI), loanItem.ClientLsci));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_FHA_CASE_NO), loanItem.FhaCaseNumber));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_LOAN_NUMBER), loanItem.LoanNumber));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_LOAN_STATUS), loanItem.LoanStatus));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_ASSET_RISK_RATING), loanItem.AssetRiskRating));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_LOAN_TYPE), loanItem.LoanType));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_INVESTOR_LOAN_NO), loanItem.InvestorLoanNumber));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_SPENT_TO_DATE), loanItem.SpendToDate));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_CONVEYANCE_STATUS), loanItem.ConveyanceStatus));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_ASSET_STATUS), loanItem.AssetStatus));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_FORECL_SALE_HELD_DATE), loanItem.ForeclosureSaleHeldDate));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_FORECL_SALE_SCD_DATE), loanItem.ForeclosureSaleScheduleDate));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_FTV_DATE), loanItem.FirstTimeVacantDate));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_PROJ_CONVEY_DATE), loanItem.ProjectedConveyanceDate));
        //        radPanelItemLoanInfoItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_FT_FINAL_DATE), loanItem.FirstTimeFinalDate));

        //        tree.Nodes.Add(radPanelItemLoanInfoItem);
        //    }
        //    rdpnbLeftPanel.Items.Insert(SysXConsts.NONE, radPanelItemLoanInfo);
        //}

        /// <summary>
        /// Add Property Details to Asset Legend
        /// </summary>
        /// <param name="assetLegendContract"></param>
        //private void AddPropertyInfo(AssetLegendContract assetLegendContract)
        //{
        //    RadPanelItem radPanelItemPropertyInfo = new RadPanelItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_PROPERTY_DETAILS));
        //    radPanelItemPropertyInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_LAST_OCC_STATUS), assetLegendContract.LastOccupancyStatus));
        //    radPanelItemPropertyInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_NO_OF_UNITS), assetLegendContract.NumberOfUnit));
        //    radPanelItemPropertyInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_NO_OF_VACANT_UNITS), assetLegendContract.NumberofVacantUnit));
        //    radPanelItemPropertyInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_SECURING_KEY_CODE), assetLegendContract.SecuringKeyCodes));
        //    radPanelItemPropertyInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_LOCK_BOX_CODE), assetLegendContract.LockBoxCode));
        //    radPanelItemPropertyInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_LOT_SQ_FOOTAGE), assetLegendContract.LotSquareFootage));
        //    rdpnbLeftPanel.Items.Insert(SysXConsts.NONE, radPanelItemPropertyInfo);
        //}

        /// <summary>
        /// Add Life Cycle Information to Asset Legend
        /// </summary>
        /// <param name="assetLegendContract"></param>
        //private void AddCriticalDates(AssetLegendContract assetLegendContract)
        //{
        //    RadPanelItem radPanelItemCriticalDates = new RadPanelItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_CRITICAL_DATES));
        //    radPanelItemCriticalDates.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_INIT_GRASS_CUT_DATE), assetLegendContract.InitialGrassCutDate));
        //    radPanelItemCriticalDates.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_ASSET_CREATED_DATE), assetLegendContract.AssetCreatedDate));
        //    radPanelItemCriticalDates.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_REO_VACANCY_DATE), assetLegendContract.ReoVacancyDate));
        //    radPanelItemCriticalDates.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_REO_CLOSING_DATE), assetLegendContract.ReoCloseDate));
        //    radPanelItemCriticalDates.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_FLAT_FEE_START_DATE), assetLegendContract.FlatFeeStartDate));
        //    radPanelItemCriticalDates.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_FLAT_FEE_END_DATE), assetLegendContract.FlatFeeEndDate));
        //    rdpnbLeftPanel.Items.Insert(SysXConsts.NONE, radPanelItemCriticalDates);

        //    // Add Blank Panel Item
        //    RadPanelItem Item = new RadPanelItem();
        //    SysXTreeView tree = new SysXTreeView();
        //    Item.Controls.Add(tree);
        //    radPanelItemCriticalDates.Items.Add(Item);

        //    if (!assetLegendContract.UnitDetails.IsNull() && assetLegendContract.UnitDetails.Count > 1)
        //    {
        //        // Build Tree Items
        //        foreach (var unitItem in assetLegendContract.UnitDetails)
        //        {
        //            RadTreeNode radPanelItem = new RadTreeNode(unitItem.ToString());
        //            radPanelItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_INIT_SECU_DATE), unitItem.InitialSecureDate));
        //            radPanelItem.Nodes.Add(LegendTreeItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_WIN_PERF_DATE), unitItem.WinterizationPerformedDate));
        //            tree.Nodes.Add(radPanelItem);
        //        }
        //    }
        //    else
        //    {
        //        radPanelItemCriticalDates.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_INIT_SECU_DATE), assetLegendContract.InitialSecureDate));
        //        radPanelItemCriticalDates.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_WIN_PERF_DATE), assetLegendContract.WinterizationPerformedDate));
        //    }

        //}

        /// <summary>
        /// Add REO Information to Asset Legend
        /// </summary>
        /// <param name="assetLegendContract"></param>
        //private void AddReoInfo(AssetLegendContract assetLegendContract)
        //{
        //    RadPanelItem radPanelItemREOInfo = new RadPanelItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_REO_INFORMATION));

        //    radPanelItemREOInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_BROKER_CONTACT_TYPE), assetLegendContract.BrokerContactType));
        //    radPanelItemREOInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_BROKER_NAME), assetLegendContract.BrokerName));
        //    radPanelItemREOInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_BROKER_PHONE_NUMBER), assetLegendContract.BrokerContactNumber));
        //    radPanelItemREOInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_ASSET_MAN_CONT_TYPE), assetLegendContract.AssetManagerContactType));
        //    radPanelItemREOInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_ASSET_MAN_NAME), assetLegendContract.AssetManagerName));
        //    radPanelItemREOInfo.Items.Add(LegendItem(SysXUtils.GetMessage(ResourceConst.ASSET_LEGEND_ASSET_MAN_CONT_NO), assetLegendContract.AssetManagerContactNumber));
        //    rdpnbLeftPanel.Items.Insert(SysXConsts.NONE, radPanelItemREOInfo);

        //}

        /// <summary>
        /// Add Rad Panel Item
        /// </summary>
        /// <param name="title"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private RadPanelItem LegendItem(String title, String value)
        {
            // TODO : Discuss with Ashish Daniel
            return new RadPanelItem(String.Format("<span class='pstonehdr' style='font-weight:bold;'>{0}</span>&nbsp;<span class='pstonedata'>{1}</span>", title, value));
        }

        /// <summary>
        /// Add RadTreeNode
        /// </summary>
        /// <param name="title"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private RadTreeNode LegendTreeItem(String title, String value)
        {
            return new RadTreeNode(String.Format("<span class='pstonehdr' style='font-weight:bold;'>{0}</span>&nbsp;<span class='pstonedata'>{1}</span>", title, value));
        }


        #endregion

        #endregion

        #endregion

        #region NewImplementation for Menu

        /// <summary>
        /// To build menu.
        /// </summary>
        private void BuildMenus(Boolean isAllMenu)
        {
            try
            {
                if (isAllMenu || (_blockId == 0) || (_blockId != _sessionService.SysXBlockId))  //if initial load or line of business change event has occurred
                {
                    InitializeRadMenu();

                    rdmnMainMenu.DataBind();
                    var parentItemCount = rdmnMainMenu.Items.Count;
                    if (parentItemCount == 1)
                    {
                        rdmnMainMenu.DefaultGroupSettings.OffsetX = 110;
                    }
                    _blockId = _sessionService.SysXBlockId;
                }
            }
            catch (System.Exception ex)
            {
                SysXWebSiteUtils.LoggerService.GetLogger().Error("appMaster.cs, BuildMenus", ex);
                SysXWebSiteUtils.ExceptionService.HandleError("Unable to build menus for user : " + SysXWebSiteUtils.SessionService.SysXMembershipUser.UserName, ex);
            }
        }

        private void InitializeRadMenu()
        {
            rdmnMainMenu.DataFieldID = "ID";
            rdmnMainMenu.DataFieldParentID = "ParentID";
            rdmnMainMenu.DataTextField = "Text";
            rdmnMainMenu.DataValueField = "ID";
            rdmnMainMenu.DataNavigateUrlField = "URL";

            //UAT-1885 : Complio Menu Re-nesting.
            Int32 SecurityMenuID = this.MenuFeatures.Where(cond => cond.Text == "Security").Select(sel => sel.ID).FirstOrDefault();

            List<MenuViewItem> mainMenuFeatures = this.MenuFeatures.Where(cond => cond.ID != SecurityMenuID && (cond.ParentID.HasValue ? cond.ParentID.Value != SecurityMenuID : true)).ToList();
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            if (user.IsNotNull() && user.TenantId != Presenter.GetDefaultTenantID() && user.TenantTypeCode.Equals(TenantType.Institution.GetStringValue()) && !user.IsSysXAdmin && !user.IsSystem)
            {
                mainMenuFeatures = mainMenuFeatures.Where(cond => cond.TenantID == user.TenantId).ToList();
            }
            rdmnMainMenu.DataSource = mainMenuFeatures;
            rdmnMainMenu.ItemDataBound += new RadMenuEventHandler(rdmnMainMenu_ItemDataBound);
            rdmnMainMenu.ItemClick += new RadMenuEventHandler(rdmnMainMenu_Click);
            mnSystemSetup.Items[0].Items.Clear();
            if (this.MenuFeatures.Any(cond => cond.ID == SecurityMenuID))
            {
                List<MenuViewItem> securityFeatures = this.MenuFeatures.Where(cond => (cond.ParentID.HasValue ? cond.ParentID.Value == SecurityMenuID : false)).ToList();
                if (user.IsNotNull() && user.TenantId != Presenter.GetDefaultTenantID() && user.TenantTypeCode.Equals(TenantType.Institution.GetStringValue()) && !user.IsSysXAdmin && !user.IsSystem)
                {
                    securityFeatures = securityFeatures.Where(cond => cond.TenantID == user.TenantId).ToList();
                }
                if (securityFeatures.Count > AppConsts.NONE)
                {
                    mnSystemSetup.Items[0].Visible = true;
                    foreach (MenuViewItem item in securityFeatures)
                    {
                        mnSystemSetup.Items[0].Items.Add(new RadMenuItem() { NavigateUrl = item.URL, Text = item.Text, Target = "pageFrame" });
                    }
                }
            }
            else
            {
                mnSystemSetup.Items[0].Visible = false;
            }
        }

        protected void rdmnMainMenu_ItemDataBound(object sender, RadMenuEventArgs e)
        {
            MenuViewItem row = (MenuViewItem)e.Item.DataItem;

            if (!string.IsNullOrEmpty(row.Tooltip))
                e.Item.ToolTip = row.Tooltip;

            //Admin Entry Portal
            if (row.IsReactAppUrl)
                e.Item.NavigateUrl = "";

            if (e.Item.Level > 0)
            {
                e.Item.GroupSettings.Flow = ItemFlow.Vertical;
                e.Item.GroupSettings.Width = 200;
            }
        }

        #endregion

        public Int32 BlockID
        {
            get
            {
                return _blockId;
            }

        }

        public String InstituteLabelText
        {
            set { divInstitute.InnerHtml = value; }

        }
        public Boolean IsEnroller
        {
            get
            {
                if ((SysXWebSiteUtils.SessionService.SysXMembershipUser as SysXMembershipUser).IsEnroller == null)
                {
                    (SysXWebSiteUtils.SessionService.SysXMembershipUser as SysXMembershipUser).IsEnroller = Presenter.CheckIfUserIsEnroller(Convert.ToString(((SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser).UserId));
                    return (SysXWebSiteUtils.SessionService.SysXMembershipUser as SysXMembershipUser).IsEnroller.Value;
                }
                else
                    return (SysXWebSiteUtils.SessionService.SysXMembershipUser as SysXMembershipUser).IsEnroller.Value;
            }
        }

        protected void clientAdminddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            if (Convert.ToInt32(SelectedTenantIdForClientAdmin) > 0 && TenantId > 0 && Convert.ToInt32(SelectedTenantIdForClientAdmin) != TenantId)
            {
                Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
                ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
                appInstData.UserID = CurrentUserId;
                appInstData.TagetInstURL = Presenter.GetInstitutionUrl(true);
                appInstData.TokenCreatedTime = DateTime.Now;
                appInstData.TenantID = Convert.ToInt32(SelectedTenantIdForClientAdmin);
                String key = Guid.NewGuid().ToString();

                Dictionary<String, ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
                //if (Application["ApplicantInstData"] != null)
                if (applicationData != null)
                {
                    //applicantData = (Dictionary<String, ApplicantInsituteDataContract>)applicationData;
                    applicantData = applicationData;
                    applicantData.Add(key, appInstData);
                    Presenter.UpdateWebApplicationData("ApplicantInstData", applicantData);
                }
                else
                {
                    applicantData.Add(key, appInstData);
                    //Application["ApplicantInstData"] = applicantData;
                    Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
                }

                //Log out from application then redirect to selected tenant url, append key in querystring.
                // On login page get data from Application Variable.

                // Check the user's authentication.
                Presenter.DoLogOff(!user.IsNull(), user.UserLoginHistoryID);
                //Redirect to login page
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TokenKey", key  }
                                                                 };
                Response.Redirect(String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}", key));
                //Response.Redirect(String.Format("~/Login.aspx?TokenKey={0}", key));
            }
        }

        #region UAT-1218

        /// <summary>
        /// UserType Switching View dropdown selected index changed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUserTypeSwitchingView_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            if (ddlUserTypeSwitchingView.SelectedValue == UserTypeSwitchView.ADBAdmin.GetStringValue())
            {
                #region Switch to ADB Admin View
                SwitchToADBAdmin();
                #endregion
            }
            else if (ddlUserTypeSwitchingView.SelectedValue == UserTypeSwitchView.ClientAdmin.GetStringValue())
            {
                #region Switch to Client Admin View
                SwitchToClientAdmin();
                #endregion
            }
            else if (ddlUserTypeSwitchingView.SelectedValue == UserTypeSwitchView.Applicant.GetStringValue())
            {
                #region Switch to Applicant View
                SwitchToApplicant();
                #endregion
            }
            //UAT-1561: Instructor/Preceptor and Shared User (student share + agency user) should be different system roles.
            else if (ddlUserTypeSwitchingView.SelectedValue == UserTypeSwitchView.InstructorOrPreceptor.GetStringValue())
            {
                #region Switch to Shared User View
                SwitchToSharedUser(UserTypeSwitchView.InstructorOrPreceptor.GetStringValue());
                #endregion
            }
            else if (ddlUserTypeSwitchingView.SelectedValue == UserTypeSwitchView.AgencyUser.GetStringValue())
            {
                #region Switch to Shared User View
                SwitchToSharedUser(UserTypeSwitchView.AgencyUser.GetStringValue());
                #endregion
            }
            else if (ddlUserTypeSwitchingView.SelectedValue == UserTypeSwitchView.SharedUser.GetStringValue())
            {
                #region Switch to Shared User View
                SwitchToSharedUser(UserTypeSwitchView.SharedUser.GetStringValue());
                #endregion
            }
        }

        /// <summary>
        /// Get the type of User trying to login
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserType GetUserType(SysXMembershipUser user)
        {
            if (user.IsApplicant.IsNotNull() && user.IsApplicant)
                return UserType.APPLICANT;
            else if (!user.IsApplicant && user.TenantTypeCode == TenantType.Institution.GetStringValue())
                return UserType.CLIENTADMIN;
            else if (!user.IsApplicant && (user.TenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue()))
                return UserType.THIRDPARTYADMIN;
            else if (user.IsSharedUser.IsNotNull() && user.IsSharedUser)
                return UserType.SHAREDUSER;
            else
                return UserType.SUPERADMIN;
        }

        /// <summary>
        /// Method to check whether user logged-in with Shared User Login URL 
        /// </summary>
        /// <param name="currentUrl"></param>
        private void IsSharedUserLogin(String currentUrl)
        {
            var _sharedUserUrl = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);

            String _sharedUserHost = _sharedUserUrl;
            if (_sharedUserUrl.Contains("http"))
            {
                Uri _url = new Uri(_sharedUserUrl);
                _sharedUserHost = _url.Host;
            }
            if (!_sharedUserUrl.IsNullOrEmpty() && currentUrl.ToLower().Trim().Contains(_sharedUserHost.ToLower().Trim()))
            {
                IsSharedUserLoginURL = true;
                return;
            }
            IsSharedUserLoginURL = false;
            return;
        }

        /// <summary>
        /// Method to Bind User Type Swich View
        /// </summary>
        private void BindUserTypeSwitchView(SysXMembershipUser user)
        {
            Presenter.GetOrganizationUserInfoByUserId(user.UserId);

            if (LstOrganizationUser.Count > AppConsts.ONE)
            {
                //If all organizationusers are not Applicants OR all organizationusers are not Client Admins
                if (!(LstOrganizationUser.All(cond => cond.IsApplicant == true))
                    && !(LstOrganizationUser.All(cond => cond.IsApplicant == false && cond.Organization.TenantID != AppConsts.SUPER_ADMIN_TENANT_ID)))
                {
                    dvSwitchView.Visible = true;
                    dvSwitchViewDropdown.Visible = true;

                    List<String> lstUserTypeSwitchCode = new List<String>();

                    //ADB Admin View
                    if (LstOrganizationUser.Any(cond => (cond.IsApplicant ?? false) == false && (cond.IsSharedUser ?? false) == false && cond.Organization.TenantID == AppConsts.SUPER_ADMIN_TENANT_ID))
                        lstUserTypeSwitchCode.Add(UserTypeSwitchView.ADBAdmin.GetStringValue());

                    //Client Admin View
                    if (LstOrganizationUser.Any(cond => (cond.IsApplicant ?? false) == false && (cond.IsSharedUser ?? false) == false && cond.Organization.TenantID != AppConsts.SUPER_ADMIN_TENANT_ID))
                        lstUserTypeSwitchCode.Add(UserTypeSwitchView.ClientAdmin.GetStringValue());

                    //Applicant View
                    if (LstOrganizationUser.Any(cond => (cond.IsApplicant ?? false) == true))
                        lstUserTypeSwitchCode.Add(UserTypeSwitchView.Applicant.GetStringValue());

                    //Shared User View
                    if (LstOrganizationUser.Any(cond => (cond.IsSharedUser ?? false) == true))
                    {
                        //UAT-1561: Instructor/Preceptor and Shared User (student share + agency user) should be different system roles.
                        var organizationUser = LstOrganizationUser.FirstOrDefault(x => x.IsSharedUser ?? false == true);
                        if (organizationUser.IsNotNull())
                        {
                            //var organizationUserTypeMapping = Business.RepoManagers.SecurityManager.GetOrganizationUserTypeMapping(organizationUser.UserID);
                            List<String> orgUserTypeCode = new List<String>();

                            if (!organizationUser.OrganizationUserTypeMappings.IsNullOrEmpty())
                            {
                                //UAT-4160
                                //orgUserTypeCode = organizationUser.OrganizationUserTypeMappings.Select(col => col.lkpOrgUserType.OrgUserTypeCode).ToList();
                                orgUserTypeCode = organizationUser.OrganizationUserTypeMappings.Where(con => con.OTM_IsDeleted != true).Select(col => col.lkpOrgUserType.OrgUserTypeCode).ToList();
                            }

                            if (!orgUserTypeCode.IsNullOrEmpty())
                            {
                                if (orgUserTypeCode.Contains(OrganizationUserType.Instructor.GetStringValue())
                                    || orgUserTypeCode.Contains(OrganizationUserType.Preceptor.GetStringValue()))
                                {
                                    lstUserTypeSwitchCode.Add(UserTypeSwitchView.InstructorOrPreceptor.GetStringValue());
                                }
                                if (orgUserTypeCode.Contains(OrganizationUserType.AgencyUser.GetStringValue()))
                                {
                                    lstUserTypeSwitchCode.Add(UserTypeSwitchView.AgencyUser.GetStringValue());
                                }
                                if (orgUserTypeCode.Contains(OrganizationUserType.ApplicantsSharedUser.GetStringValue()))
                                {
                                    lstUserTypeSwitchCode.Add(UserTypeSwitchView.SharedUser.GetStringValue());
                                }
                            }
                        }
                    }

                    //Getting All User Type Switch Views
                    Presenter.GetUserTypeSwitchView();

                    //Filtering Required User Type Switch Views
                    List<lkpUserTypeSwitchView> lstRequiredSwitchView = lstUserTypeSwitchView.Where(cond => lstUserTypeSwitchCode.Contains(cond.UTSV_Code)).ToList();

                    //Binding UserTypeSwicthView dropdown
                    ddlUserTypeSwitchingView.DataSource = lstRequiredSwitchView;
                    ddlUserTypeSwitchingView.DataBind();
                    SetCurrentUserTypeView(user);
                }
            }
            //UAT-1561: Instructor/Preceptor and Shared User (student share + agency user) should be different system roles.
            else if (LstOrganizationUser.Count == AppConsts.ONE && LstOrganizationUser.Any(cond => (cond.IsSharedUser ?? false) == true))
            {
                List<String> lstUserTypeSwitchCode = new List<String>();

                var organizationUser = LstOrganizationUser.FirstOrDefault(x => x.IsSharedUser ?? false == true);
                if (organizationUser.IsNotNull())
                {
                    List<String> orgUserTypeCode = new List<String>();

                    if (!organizationUser.OrganizationUserTypeMappings.IsNullOrEmpty())
                    {
                        orgUserTypeCode = organizationUser.OrganizationUserTypeMappings.Select(col => col.lkpOrgUserType.OrgUserTypeCode).ToList();
                    }

                    if (!orgUserTypeCode.IsNullOrEmpty())
                    {
                        if (orgUserTypeCode.Contains(OrganizationUserType.Instructor.GetStringValue())
                            || orgUserTypeCode.Contains(OrganizationUserType.Preceptor.GetStringValue()))
                        {
                            lstUserTypeSwitchCode.Add(UserTypeSwitchView.InstructorOrPreceptor.GetStringValue());
                        }
                        if (orgUserTypeCode.Contains(OrganizationUserType.AgencyUser.GetStringValue()))
                        {
                            lstUserTypeSwitchCode.Add(UserTypeSwitchView.AgencyUser.GetStringValue());
                        }
                        if (orgUserTypeCode.Contains(OrganizationUserType.ApplicantsSharedUser.GetStringValue()))
                        {
                            lstUserTypeSwitchCode.Add(UserTypeSwitchView.SharedUser.GetStringValue());
                        }
                    }
                }

                if (lstUserTypeSwitchCode.Count > AppConsts.ONE)
                {
                    //Getting All User Type Switch Views
                    Presenter.GetUserTypeSwitchView();

                    //Filtering Required User Type Switch Views
                    List<lkpUserTypeSwitchView> lstRequiredSwitchView = lstUserTypeSwitchView.Where(cond => lstUserTypeSwitchCode.Contains(cond.UTSV_Code)).ToList();

                    //Binding UserTypeSwicthView dropdown
                    dvSwitchView.Visible = true;
                    dvSwitchViewDropdown.Visible = true;
                    ddlUserTypeSwitchingView.DataSource = lstRequiredSwitchView;
                    ddlUserTypeSwitchingView.DataBind();
                    SetCurrentUserTypeView(user);
                }
            }
            //UAT-2551
            if (!System.Web.HttpContext.Current.Session["AgencyViewAdminOrgUsrID"].IsNullOrEmpty())
            {
                ddlUserTypeSwitchingView.Enabled = false;
            }
        }

        /// <summary>
        /// Set Current User Type View Or User Type Switching dropdown selected value
        /// </summary>
        /// <param name="user"></param>
        private void SetCurrentUserTypeView(SysXMembershipUser user)
        {
            var _currentUsertype = GetUserType(user);

            if (_currentUsertype == UserType.SUPERADMIN)
                ddlUserTypeSwitchingView.SelectedValue = UserTypeSwitchView.ADBAdmin.GetStringValue();
            else if (_currentUsertype == UserType.CLIENTADMIN)
            {
                ddlUserTypeSwitchingView.SelectedValue = UserTypeSwitchView.ClientAdmin.GetStringValue();

                List<Entity.OrganizationUser> lstAllApplicants = LstOrganizationUser.Where(cond => (cond.IsApplicant ?? false) == true).ToList();
                if (lstAllApplicants.All(cond => cond.Organization.TenantID != TenantId))
                {
                    DropDownListItem item = ddlUserTypeSwitchingView.FindItemByValue(UserTypeSwitchView.Applicant.GetStringValue());
                    ddlUserTypeSwitchingView.Items.Remove(item);
                }
            }
            else if (_currentUsertype == UserType.APPLICANT)
            {
                ddlUserTypeSwitchingView.SelectedValue = UserTypeSwitchView.Applicant.GetStringValue();

                List<Entity.OrganizationUser> lstAllClientAdmins = LstOrganizationUser.Where(cond => (cond.IsApplicant ?? false) == false && (cond.IsSharedUser ?? false) == false && cond.Organization.TenantID != AppConsts.SUPER_ADMIN_TENANT_ID).ToList();
                if (lstAllClientAdmins.All(cond => cond.Organization.TenantID != TenantId))
                {
                    DropDownListItem item = ddlUserTypeSwitchingView.FindItemByValue(UserTypeSwitchView.ClientAdmin.GetStringValue());
                    ddlUserTypeSwitchingView.Items.Remove(item);
                }
            }
            else if (_currentUsertype == UserType.SHAREDUSER)
            {
                //UAT-1561: Instructor/Preceptor and Shared User (student share + agency user) should be different system roles.
                //ddlUserTypeSwitchingView.SelectedValue = UserTypeSwitchView.SharedUser.GetStringValue();

                var organizationUser = LstOrganizationUser.FirstOrDefault(x => x.IsSharedUser ?? false == true);
                if (organizationUser.IsNotNull())
                {
                    List<String> orgUserTypeCode = new List<String>();

                    if (!organizationUser.OrganizationUserTypeMappings.IsNullOrEmpty())
                    {
                        orgUserTypeCode = organizationUser.OrganizationUserTypeMappings.Select(col => col.lkpOrgUserType.OrgUserTypeCode).ToList();
                    }
                    if (!orgUserTypeCode.IsNullOrEmpty())
                    {
                        //If user is not switched then first prefer Agency User, then Instructor/Preceptor, then Applicant Shared User
                        if (Session["SwitchUserType"].IsNullOrEmpty())
                        {
                            if (orgUserTypeCode.Contains(OrganizationUserType.ApplicantsSharedUser.GetStringValue()))
                            {
                                ddlUserTypeSwitchingView.SelectedValue = UserTypeSwitchView.SharedUser.GetStringValue();
                            }
                            if (orgUserTypeCode.Contains(OrganizationUserType.Instructor.GetStringValue())
                                || orgUserTypeCode.Contains(OrganizationUserType.Preceptor.GetStringValue()))
                            {
                                ddlUserTypeSwitchingView.SelectedValue = UserTypeSwitchView.InstructorOrPreceptor.GetStringValue();
                            }
                            if (orgUserTypeCode.Contains(OrganizationUserType.AgencyUser.GetStringValue()))
                            {
                                ddlUserTypeSwitchingView.SelectedValue = UserTypeSwitchView.AgencyUser.GetStringValue();
                            }
                        }
                        else
                        {
                            ddlUserTypeSwitchingView.SelectedValue = Session["SwitchUserType"].ToString();
                        }
                    }
                }
                //End
            }
        }

        /// <summary>
        /// Method to Switch to ADB Admin View
        /// </summary>
        private void SwitchToADBAdmin()
        {
            UserTypeSwitchViewCode = UserTypeSwitchView.ADBAdmin.GetStringValue();
            var tenantID = AppConsts.SUPER_ADMIN_TENANT_ID;
            String switchingTargetURL = Presenter.GetSwitchingTargetUrl(tenantID);
            RedirectToTargetSwitchingView(tenantID, switchingTargetURL);
        }

        /// <summary>
        /// Method to switch to CLient Admin View
        /// </summary>
        private void SwitchToClientAdmin()
        {
            UserTypeSwitchViewCode = UserTypeSwitchView.ClientAdmin.GetStringValue();
            Int32 tenantID = AppConsts.NONE;

            tenantID = LstOrganizationUser.Where(cond => (cond.IsApplicant ?? false) == false
            && (cond.IsSharedUser ?? false) == false
            && cond.Organization.TenantID == TenantId)
            .Select(col => col.Organization.TenantID).FirstOrDefault() ?? AppConsts.NONE;

            if (tenantID == AppConsts.NONE)
            {
                tenantID = LstOrganizationUser.Where(cond => (cond.IsApplicant ?? false) == false
                    && (cond.IsSharedUser ?? false) == false
                    && cond.Organization.TenantID != AppConsts.SUPER_ADMIN_TENANT_ID)
                    .Select(col => col.Organization.TenantID).FirstOrDefault() ?? AppConsts.NONE;
            }
            String switchingTargetURL = Presenter.GetSwitchingTargetUrl(tenantID);
            RedirectToTargetSwitchingView(tenantID, switchingTargetURL);
        }

        /// <summary>
        /// Method to switch to Applicant View
        /// </summary>
        private void SwitchToApplicant()
        {
            UserTypeSwitchViewCode = UserTypeSwitchView.Applicant.GetStringValue();
            Int32 tenantID = AppConsts.NONE;
            tenantID = LstOrganizationUser.Where(cond => (cond.IsApplicant ?? false) == true && cond.Organization.TenantID == TenantId).Select(x => x.Organization.TenantID).FirstOrDefault() ?? AppConsts.NONE;
            if (tenantID == AppConsts.NONE)
            {
                tenantID = LstOrganizationUser.Where(cond => (cond.IsApplicant ?? false) == true).Select(col => col.Organization.TenantID).FirstOrDefault() ?? AppConsts.NONE;
            }
            String switchingTargetURL = Presenter.GetSwitchingTargetUrl(tenantID);
            RedirectToTargetSwitchingView(tenantID, switchingTargetURL);
        }

        /// <summary>
        /// Method to switch to Shared User View.
        /// </summary>
        private void SwitchToSharedUser(String userTypeSwitchView)
        {
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            UserTypeSwitchViewCode = userTypeSwitchView;
            var tenantID = AppConsts.SHARED_USER_TENANT_ID;

            String sharedUserLoginURL = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);
            if (!(sharedUserLoginURL.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || sharedUserLoginURL.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            {
                if (HttpContext.Current != null)
                {
                    sharedUserLoginURL = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", sharedUserLoginURL.Trim());
                }
                else
                {
                    sharedUserLoginURL = string.Concat("http://", sharedUserLoginURL.Trim());
                }
            }

            RedirectToTargetSwitchingView(tenantID, sharedUserLoginURL);
        }

        /// <summary>
        /// Method To create/update WebApplicationData, Do logout current user and then Redirect to Target Switch View.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="switchingTargetURL"></param>
        private void RedirectToTargetSwitchingView(int tenantID, String switchingTargetURL)
        {
            Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
            ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
            appInstData.UserID = CurrentUserId;
            appInstData.TagetInstURL = switchingTargetURL;
            appInstData.TokenCreatedTime = DateTime.Now;
            appInstData.TenantID = Convert.ToInt32(tenantID);
            appInstData.UserTypeSwitchViewCode = UserTypeSwitchViewCode;
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

            //Log out from application then redirect to selected tenant url, append key in querystring.
            // On login page get data from Application Variable.
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            Presenter.DoLogOff(true, user.UserLoginHistoryID);

            //Redirect to login page
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TokenKey", key  }
                                                                 };
            Response.Redirect(String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}", key));
        }

        #endregion

        protected void btnDummyPostBack_Click(object sender, EventArgs e)
        {
            Session["BreadCrumb"] = null;
        }

        // <summary>
        /// Override the Tabe settings/focus, based on the SharedUserTypes
        /// </summary>
        private void HandleLinkseBySharedUser(SysXMembershipUser user, Dictionary<String, String> queryString)
        {
            var _agencyUserCode = OrganizationUserType.AgencyUser.GetStringValue();
            // Agency User
            if (user.SharedUserTypesCode.Contains(_agencyUserCode))
            {
                queryString.Add("TAB", "ROTATION");
                lnkRotation.HRef = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                queryString.Remove("TAB");
                queryString.Add("TAB", AppConsts.REQUIREMENT_SHARES_SCREEN_NAME);
                lnkRequirement.HRef = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                queryString.Remove("TAB");
                queryString.Add("TAB", "OTHER");
                lnkOther.HRef = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                //queryString.Remove("Child");
                queryString.Remove("TAB");
                //UAT-2469
                //queryString.Add("Child", AppConsts.SHARED_USER_STUDENT_ROTATION_SEARCH);
                queryString.Add("TAB", AppConsts.STUDENT_ROTATION_SEARCH);
                lnkRotationStudent.HRef = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                queryString.Remove("TAB");
                //UAT-2548
                queryString.Add("TAB", AppConsts.AGENCY_APPLICANT_STATUS_KEY);
                lnkAgencyApplicantStatus.HRef = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                queryString.Remove("TAB");

                //UAT-3406
                queryString.Add("TAB", AppConsts.SCHOOL_REPRESENTATIVE_DETAILS);
                lnkSchoolRepresentativeDetails.HRef = String.Format("~/ProfileSharing/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                queryString.Remove("TAB");

            }
            else
            {
                liOther.Visible = false;
                liRotation.Visible = false;
                liRotationStudent.Visible = false;
                liRequirementShares.Visible = false;
                liAgencyApplicantStatus.Visible = false; //UAT-2548
                liSchoolRepresentativeDetails.Visible = false; //UAT-3406

            }
        }

        #region UAT:-3032 Sticky "Institution Selection" for ADB admins.

        //Method to bind tenant dropdown.
        private void BindPreferredTenant()
        {
            if (IsUserAllowedPreferredTenant)
            {
                dvPreferredTenant.Visible = true;
                Presenter.GetTenantsForPreferredSelection();
                cmbPreferredTenant.DataSource = lstTenants;
                cmbPreferredTenant.DataBind();
                if (!Session["PreferredSelectedTenant"].IsNullOrEmpty())
                {
                    cmbPreferredTenant.SelectedValue = Convert.ToString(Session["PreferredSelectedTenant"]);
                }
            }
            else
            {
                dvPreferredTenant.Visible = false;
                Session["PreferredSelectedTenant"] = null;
            }
        }

        protected void cmbPreferredTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!cmbPreferredTenant.SelectedValue.IsNullOrEmpty())
            {
                Session["PreferredSelectedTenant"] = cmbPreferredTenant.SelectedValue;
            }
        }

        #endregion

        #region Globalization

        protected void btnLanguage_Click(object sender, EventArgs e)
        {
            String languageCode = String.Empty;
            languageCode = hdnLanguageTransCode.Value;
            LanguageTranslateUtils.SetLanguageInSession(languageCode);
            Server.Transfer(Request.Url.PathAndQuery, false);
        }

        private void ManageLanguageTranslation()
        {
            //HttpContext.Current.Session["BreadCrumb"] = null;
            if (Session["IsLocationTenant"].IsNullOrEmpty())
            {
                SysXWebSiteUtils.SessionService.SetCustomData("IsLocationTenant", IsLocationServiceTenant);
            }
            hdnGenericExceptionMsg.Value = Resources.Language.LOGINEXCEPTION;

            hdnGenericExceptionMsgXSSANDHTML.Value = Resources.Language.LOGINEXCEPTIONCSSANDHTML;

            if (!Session["IsLocationTenant"].IsNullOrEmpty() && Convert.ToBoolean(Session["IsLocationTenant"]))
            {
                hdnGenericExceptionMsg.Value = Resources.Language.LOCATIONTENANTGENERICEXCEPTIONMSG;
                dvLanguage.Style.Add("display", "inline-block");
                if (IsCurrentUserIsAdminOrClientAdmin)
                {
                    dvLanguage.Visible = false;
                    btnLanguage.Visible = false;
                }
                else
                {
                    dvLanguage.Visible = true;
                    btnLanguage.Visible = true;
                }
                hdnLanguageTransCode.Value = Languages.ENGLISH.GetStringValue();
                string _currentLangSession = Convert.ToString(LanguageTranslateUtils.GetCurrentLanguageCultureFromSession());

                if (_currentLangSession.IsNullOrEmpty() || _currentLangSession.ToString() == LanguageCultures.ENGLISH_CULTURE.GetStringValue())
                {
                    //btnLanguage.Text = "Spanish";
                    //btnLanguage.ToolTip = "Click for Spanish";

                    btnLanguage.Text = Resources.Language.SPANISH;
                    btnLanguage.ToolTip = Resources.Language.LANGUAGEBUTTONTOOLTIPSPANISH_P1 + " " + "(" + Resources.Language.LANGUAGEBUTTONTOOLTIPSPANISH_P2 + ")";
                    hdnLanguageTransCode.Value = Languages.SPANISH.GetStringValue();
                }
                if (!_currentLangSession.IsNullOrEmpty() && _currentLangSession.ToString() == LanguageCultures.SPANISH_CULTURE.GetStringValue())
                {
                    //btnLanguage.Text = "English";
                    //btnLanguage.ToolTip = "Click for English";
                    btnLanguage.Text = Resources.Language.ENGLISH;
                    btnLanguage.ToolTip = Resources.Language.LANGUAGEBUTTONTOOLTIPENG_P1 + " " + "(" + Resources.Language.LANGUAGEBUTTONTOOLTIPENG_P2 + ")";
                    hdnLanguageTransCode.Value = Languages.ENGLISH.GetStringValue();
                }
            }
            else
            {
                dvLanguage.Style.Add("display", "none");
                dvLanguage.Visible = false;
                btnLanguage.Visible = false;

            }
        }

        protected void GetLanguageCulture(Guid userId)
        {
            if (Session["LanguageCulture"].IsNullOrEmpty())
            {
                String _languageCulture = LanguageCultures.ENGLISH_CULTURE.GetStringValue();
                _languageCulture = Presenter.GetLanguageCode(userId);
                LanguageTranslateUtils.SetLanguageInSession(_languageCulture);
                //SysXWebSiteUtils.SessionService.SetCustomData("LanguageCulture", _languageCulture);
            }

            String currentLanguageCulture = Convert.ToString(LanguageTranslateUtils.GetCurrentLanguageCultureFromSession());

            CultureInfo cultureInfo = new CultureInfo(currentLanguageCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        private void SetContainerHeightForMobileDevice()
        {
            Boolean isMobileDevice = Convert.ToBoolean(SysXWebSiteUtils.SessionService.GetCustomData("IsMobileDevice"));
            if (isMobileDevice && !Session["IsLocationTenant"].IsNullOrEmpty() && Convert.ToBoolean(Session["IsLocationTenant"]))
            {
                rdpnButtomRow.MinHeight = 2500;
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "EnableMobileBrowsing();", true);
            }
        }

        #endregion

        private void ManageAgencyUserReports(List<AgencyUserReportPermissionContract> lstVisibleAgencyUserReports)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();

            if (!lstVisibleAgencyUserReports.IsNullOrEmpty())
            {
                System.Web.UI.HtmlControls.HtmlGenericControl ul = new System.Web.UI.HtmlControls.HtmlGenericControl();
                //ul.Style.Add("min-width", "180px");
                //ul.Style.Add("width", "18%");

                StringBuilder sb = new StringBuilder();

                sb.Append("<a target='pageFrame' href='#'><i class='fa fa-user iconColor'></i>Reports</a>");
                sb.Append("<ul style='min-width='180px' width='18%''>");


                foreach (var agencyUserReportPermissionContract in lstVisibleAgencyUserReports)
                {
                    System.Web.UI.HtmlControls.HtmlGenericControl li = new System.Web.UI.HtmlControls.HtmlGenericControl("li");

                    System.Web.UI.HtmlControls.HtmlGenericControl anch = new System.Web.UI.HtmlControls.HtmlGenericControl();

                    String path = @"~\" + agencyUserReportPermissionContract.AgencyUserReportFolderPath;
                    String module = agencyUserReportPermissionContract.AgencyUserReportModule;

                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", path}
                                                                 };

                    String href = String.Format(module + "/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                    var anc = "<a runat=server id= 'lnk_" + agencyUserReportPermissionContract.AgencyUserReportCode + "' target = 'pageFrame' href= '" + href + "' onclick='CallDummyPostBack(this)'> " + agencyUserReportPermissionContract.ReportName + "</a>";
                    sb.Append("<li>" + anc + "</li>");
                }

                sb.Append("</ul>");
                liReports.InnerHtml = sb.ToString();
            }
        }

        #region Admin Entry Portal
        protected void rdmnMainMenu_Click(object sender, RadMenuEventArgs e)
        {
            try
            {
                Telerik.Web.UI.RadMenuItem ItemClicked = e.Item;
                MenuViewItem menuItem = new MenuViewItem();
                if (this.MenuFeatures != null && this.MenuFeatures.Count > AppConsts.NONE)
                {
                    Int32 _menuId = Convert.ToInt32(e.Item.Value);
                    menuItem = this.MenuFeatures.Where(con => con.ID == _menuId).FirstOrDefault();
                }
                if (!menuItem.IsNullOrEmpty() && menuItem.IsReactAppUrl)
                {
                    if (!menuItem.ParentID.IsNullOrEmpty())
                    {
                        String redirectUrl = String.Empty;
                        String componentRoutePath = String.Empty;
                        String adminEntryPortalUrlPrefix = String.Empty;
                        String WebsiteUrl = String.Empty;

                        //Get token for the saved session data in DB.
                        Guid? token = SessionSharingManagement.SetSessionForSharing(menuItem.URL);
                        //WebsiteUrl = Page.Request.ServerVariables.Get("server_name"); // "http://localhost:3000";/
                        //String scheme = Page.Request.Url.Scheme;
                        //WebsiteUrl.Replace(scheme + "://", "");

                        if (HttpContext.Current.Request.Url.AbsoluteUri.Contains("localhost"))
                        {
                            WebsiteUrl = "localhost:3000";
                        }
                        else
                        {
                            WebsiteUrl = new Uri(HttpContext.Current.Request.Url.AbsoluteUri).GetLeftPart(UriPartial.Authority).Replace(HttpContext.Current.Request.Url.Scheme + "://", "");
                        }

                        //Get Configuration values from Web.config
                        adminEntryPortalUrlPrefix = _adminPortalAppUrlPrefix;
                        if (ConfigurationManager.AppSettings.AllKeys.Contains("AdminPortalAppUrlPrefix"))
                            adminEntryPortalUrlPrefix = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["AdminPortalAppUrlPrefix"]) ? ConfigurationManager.AppSettings["AdminPortalAppUrlPrefix"] : _adminPortalAppUrlPrefix.ToString();

                        componentRoutePath = _componentRoutePath;
                        if (ConfigurationManager.AppSettings.AllKeys.Contains("ComponentRoutePath"))
                            componentRoutePath = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComponentRoutePath"]) ? ConfigurationManager.AppSettings["ComponentRoutePath"] : _componentRoutePath.ToString();

                        //Redirection to react application//
                        if (token != null && !String.IsNullOrEmpty(componentRoutePath))
                        {
                            Dictionary<String, String> queryString = new Dictionary<String, String>();
                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"MRTokenKey",Convert.ToString(token)}
                                                                 };

                            redirectUrl = String.Format(HttpContext.Current.Request.Url.Scheme + "://" + adminEntryPortalUrlPrefix + WebsiteUrl + componentRoutePath + "? MRTokenKey={0}", Convert.ToString(token));
                            //redirectUrl = String.Format( WebsiteUrl + componentRoutePath + "? MRTokenKey={0}", Convert.ToString(token));

                            SysXWebSiteUtils.SessionService.ClearSession(true);
                            Response.Redirect(redirectUrl, false);

                            //Response.Redirect("http://" + "localhost:3000" + menuItem.URL, false);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void RedirectToReactApp()
        {
            if (!IsReactToMVPRedirection)
            {
                if (!SysXWebSiteUtils.SessionService.IsNull() && !SysXWebSiteUtils.SessionService.SysXMembershipUser.IsNull())
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;

                    if (user != null)
                    {
                        //List<vw_UserAssignedBlocks> userLOBList = new List<vw_UserAssignedBlocks>();
                        List<aspnet_Roles> userRoles = new List<aspnet_Roles>();
                        Dictionary<String, String> dcLob = new Dictionary<string, string>();
                        //Dictionary<String, String> dcLobFiltered = new Dictionary<string, string>();
                        List<String> lstBlockIDs = new List<String>();



                        if (!user.UserId.IsNullOrEmpty())
                        {
                            userRoles = Presenter.GetUserRolesById(user.UserId.ToString());
                            //userLOBList = Presenter.GetLineOfBusinessesByUser(user.UserId.ToString());
                            dcLob = Presenter.GetTenantData();
                        }

                        if (!dcLob.IsNullOrEmpty() && dcLob.Count > AppConsts.NONE
                            && !userRoles.IsNullOrEmpty() && userRoles.Any(x => x.RoleDetail.ShowAdminEntryDashboard)
                            && (!SysXWebSiteUtils.SessionService.IsSysXAdmin && !IsEnroller && !user.IsApplicant && !user.IsSharedUser))
                        {
                            foreach (string key in dcLob.Keys)
                            {
                                string[] valIDs = key.Split('#');
                                string valueBlockID = valIDs[0];
                                string keyNew = valIDs[1];
                                if (String.Compare(keyNew, AppConsts.AMS_BUSINESS_CHANNEL_TYPE.ToString(), true) == AppConsts.NONE)
                                {
                                    lstBlockIDs.Add(valueBlockID);
                                }
                                //dcLobFiltered.Add(keyNew, valueBlockID);
                            }


                            //String blockID = "";
                            String businessChannelTypeID = AppConsts.AMS_BUSINESS_CHANNEL_TYPE.ToString();
                            Boolean IsReactAppUrl = false;

                            //if (!dcLobFiltered.IsNullOrEmpty())
                            //{
                            //    blockID = dcLobFiltered.Where(x => x.Key == AppConsts.AMS_BUSINESS_CHANNEL_TYPE.ToString()).Select(x => x.Value).FirstOrDefault().ToString();
                            //    businessChannelTypeID = dcLobFiltered.Where(x => x.Key == AppConsts.AMS_BUSINESS_CHANNEL_TYPE.ToString()).Select(x => x.Key).FirstOrDefault().ToString();
                            //}


                            //     if (!lstBlockIDs.IsNullOrEmpty())
                            //{
                            //    blockID = dcLobFiltered.Where(x => x.Key == AppConsts.AMS_BUSINESS_CHANNEL_TYPE.ToString()).Select(x => x.Value).FirstOrDefault().ToString();
                            //    //businessChannelTypeID = dcLobFiltered.Where(x => x.Key == AppConsts.AMS_BUSINESS_CHANNEL_TYPE.ToString()).Select(x => x.Key).FirstOrDefault().ToString();
                            //}

                            //if (!blockID.IsNullOrEmpty() && !businessChannelTypeID.IsNullOrEmpty())
                            //    IsReactAppUrl = Presenter.GetMenuItems(Convert.ToString(user.UserId), Convert.ToInt32(blockID), Convert.ToInt32(businessChannelTypeID));
                            Dictionary<Boolean, Int32> dcBlock = new Dictionary<Boolean, Int32>();
                            if (!lstBlockIDs.IsNullOrEmpty())
                                dcBlock = Presenter.GetMenuItems(Convert.ToString(user.UserId), lstBlockIDs, Convert.ToInt32(businessChannelTypeID));


                            if (!dcBlock.IsNullOrEmpty() && dcBlock.Count > AppConsts.NONE)
                                IsReactAppUrl = dcBlock.FirstOrDefault().Key;

                            //if (IsReactAppUrl && (!dcLob.IsNullOrEmpty() && !dcLobFiltered.IsNullOrEmpty() && dcLobFiltered.Any(x => x.Key == AppConsts.AMS_BUSINESS_CHANNEL_TYPE.ToString())))
                            if (IsReactAppUrl && !lstBlockIDs.IsNullOrEmpty())
                            {
                                //SysXWebSiteUtils.SessionService.SetCustomData("IsReactToMVPRedirection", true);
                                String redirectUrl = String.Empty;
                                String componentRoutePath = String.Empty;
                                String adminEntryPortalUrlPrefix = String.Empty;
                                String WebsiteUrl = String.Empty;
                                if (SysXWebSiteUtils.SessionService.BusinessChannelType.IsNullOrEmpty())
                                    SysXWebSiteUtils.SessionService.BusinessChannelType = new BusinessChannelTypeMappingData();

                                //var amsBusinessChannel = dcLobFiltered.Where(x => x.Key == AppConsts.AMS_BUSINESS_CHANNEL_TYPE.ToString()).FirstOrDefault();
                                //if (!amsBusinessChannel.IsNullOrEmpty())
                                //    SysXWebSiteUtils.SessionService.BusinessChannelType =
                                //        new BusinessChannelTypeMappingData
                                //        {
                                //            BusinessChannelTypeID = Convert.ToInt16(amsBusinessChannel.Key),
                                //            BusinessChannelTypeName = Presenter.GetBusinessChannelType()
                                //            .FirstOrDefault(cond => cond.BusinessChannelTypeID == Convert.ToInt16(amsBusinessChannel.Key)).Name
                                //        };

                                //var amsBusinessChannel = dcLobFiltered.Where(x => x.Key == AppConsts.AMS_BUSINESS_CHANNEL_TYPE.ToString()).FirstOrDefault();
                                //if (!amsBusinessChannel.IsNullOrEmpty())
                                SysXWebSiteUtils.SessionService.BusinessChannelType =
                                    new BusinessChannelTypeMappingData
                                    {
                                        BusinessChannelTypeID = Convert.ToInt16(businessChannelTypeID),
                                        BusinessChannelTypeName = Presenter.GetBusinessChannelType()
                                        .FirstOrDefault(cond => cond.BusinessChannelTypeID == Convert.ToInt16(businessChannelTypeID)).Name
                                    };


                                if (lstBlockIDs.Count() > AppConsts.NONE && !lstBlockIDs.Contains(Convert.ToString(SysXWebSiteUtils.SessionService.SysXBlockId)))
                                    SysXWebSiteUtils.SessionService.SysXBlockId = dcBlock.FirstOrDefault().Value;

                                Guid? token = SessionSharingManagement.SetSessionForSharing(AppConsts.ADMIN_ENTRY_REACT_DASHBOARD);

                                if (HttpContext.Current.Request.Url.AbsoluteUri.Contains("localhost"))
                                {
                                    WebsiteUrl = "localhost:3000";
                                }
                                else
                                {
                                    WebsiteUrl = new Uri(HttpContext.Current.Request.Url.AbsoluteUri).GetLeftPart(UriPartial.Authority).Replace(HttpContext.Current.Request.Url.Scheme + "://", "");
                                }
                                //Get Configuration values from Web.config
                                adminEntryPortalUrlPrefix = String.Empty;
                                if (ConfigurationManager.AppSettings.AllKeys.Contains("AdminPortalAppUrlPrefix"))
                                    adminEntryPortalUrlPrefix = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["AdminPortalAppUrlPrefix"]) ? ConfigurationManager.AppSettings["AdminPortalAppUrlPrefix"] : String.Empty;

                                componentRoutePath = String.Empty;
                                if (ConfigurationManager.AppSettings.AllKeys.Contains("ComponentRoutePath"))
                                    componentRoutePath = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["ComponentRoutePath"]) ? ConfigurationManager.AppSettings["ComponentRoutePath"] : String.Empty;

                                if (token != null && !String.IsNullOrEmpty(componentRoutePath))
                                {
                                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"MRTokenKey",Convert.ToString(token)}
                                                                 };

                                    redirectUrl = String.Format(HttpContext.Current.Request.Url.Scheme + "://" + adminEntryPortalUrlPrefix + WebsiteUrl + componentRoutePath + "? MRTokenKey={0}", Convert.ToString(token));

                                    SysXWebSiteUtils.SessionService.ClearSession(true);
                                    Response.Redirect(redirectUrl, true);
                                }
                            }
                        }
                    }
                }
            }

        }

        #endregion
    }
}
