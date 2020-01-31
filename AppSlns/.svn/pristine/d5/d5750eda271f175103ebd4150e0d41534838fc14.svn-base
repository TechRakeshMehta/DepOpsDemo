using CoreWeb.ComplianceOperations.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.IntsofSecurityModel;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI.WebControls;


namespace CoreWeb.IntsofSecurityModel.Views
{
    public partial class OtherAccountLinking : BaseUserControl, IOtherAccountLinking
    {
        private OtherAccountLinkingPresenter _presenter = new OtherAccountLinkingPresenter();
        private Int32 _tenantId;
        private String _viewType;

        #region Property

        public OtherAccountLinkingPresenter Presenter
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

        public IOtherAccountLinking CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IOtherAccountLinking.CurrentLoggedinOrgUserID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                else
                    return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IOtherAccountLinking.SourceOrgUserId
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                return user.OrganizationUserId;
            }
        }

        String IOtherAccountLinking.SourceUserId
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                return user.UserId.ToString();
            }
        }

        String IOtherAccountLinking.EmailID
        {
            get
            {
                if (!ViewState["EmailID"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["EmailID"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["EmailID"] = value;
            }
        }

        Tuple<LookupContract, Boolean> IOtherAccountLinking.ExistingUser
        {
            get
            {
                if (!ViewState["lstExistingUsers"].IsNullOrEmpty())
                {
                    return (ViewState["lstExistingUsers"]) as Tuple<LookupContract, Boolean>;
                }
                return null;
            }
            set
            {
                ViewState["lstExistingUsers"] = value;

                if (!value.IsNullOrEmpty() && value.Item1.UserID > AppConsts.NONE)
                {
                    //lblMatchedUserName.Text = value.Item1.Name;
                    lblMatchedUserName.Text = Resources.Language.PLEASEENTERPASSWORD + " '" + value.Item1.Code.HtmlEncode() + "'";
                }
            }
        }

        String IOtherAccountLinking.UserName
        {
            get
            {
                if (!ViewState["UserName"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["UserName"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["UserName"] = value;
            }
        }

        String IOtherAccountLinking.Password
        {
            get
            {
                if (!ViewState["Password"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["Password"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["Password"] = value;
            }
        }

        String IOtherAccountLinking.TargetUserID
        {
            get
            {
                if (!ViewState["TargetUserID"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["TargetUserID"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["TargetUserID"] = value;
            }
        }

        List<ManageUsersContract> IOtherAccountLinking.lstUserContract
        {
            get
            {
                if (!ViewState["lstUserContract"].IsNullOrEmpty())
                {
                    return (ViewState["lstUserContract"]) as List<ManageUsersContract>;
                }
                return new List<ManageUsersContract>();
            }
            set
            {
                ViewState["lstUserContract"] = value;
            }
        }

        Boolean IOtherAccountLinking.IsApplicantsSharedUser
        {
            get
            {
                if (!ViewState["IsApplicantsSharedUser"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsApplicantsSharedUser"]);
                }
                return false;
            }
            set
            {
                ViewState["IsApplicantsSharedUser"] = value;
            }
        }

        String IOtherAccountLinking.InstructorPageType
        {
            get
            {
                if (!ViewState["InstructorPageType"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["InstructorPageType"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["InstructorPageType"] = value;
            }
        }


        #endregion

        #region Events

        /// <summary>
        /// Page OnInit Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = Resources.Language.LINKACCOUNT; ;//"Link Account";
                base.SetPageTitle(Resources.Language.LINKACCOUNT);//("Link Account");
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BasePage basePage = base.Page as BasePage;
            if (basePage != null)
            {
                basePage.SetModuleTitle(Resources.Language.LINKACCOUNT);//("Link Account");
            }
            if (!IsPostBack)
            {
                var args = new Dictionary<String, String>();
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey("IsApplicantsSharedUser"))
                {
                    CurrentViewContext.IsApplicantsSharedUser = Convert.ToBoolean(args["IsApplicantsSharedUser"]);
                }
                if (args.ContainsKey("PageType"))
                {
                    CurrentViewContext.InstructorPageType = Convert.ToString(args["PageType"]);
                }
            }

        }

        #endregion

        #region ButtonClicks


        /// <summary>
        /// Validate the email address and get user matched with the email.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar_SubmitClick(object sender, EventArgs e)
        {
            lblMessage.Text = String.Empty;
            CurrentViewContext.EmailID = txtEmailAddress.Text;
            if (!Presenter.ValidateEmail())
            {
                //base.ShowInfoMessage("No accounts with email address " + CurrentViewContext.EmailID + " found");
                base.ShowInfoMessage(Resources.Language.NOACCTWITHEMLADD + " " + CurrentViewContext.EmailID + " " + Resources.Language.FOUND);
                return;
            }
            else
            {
                Presenter.ValidateEmailandGetUsers();
                if (!CurrentViewContext.ExistingUser.IsNullOrEmpty() && CurrentViewContext.ExistingUser.Item1.UserID > AppConsts.NONE)
                {
                    if (CurrentViewContext.ExistingUser.Item2)
                    {
                        base.ShowInfoMessage(Resources.Language.ACCTCNTNINACTIVEPROFILES);
                        return;
                    }
                    else
                    {
                        dvlstExistingUsers.Visible = true;
                        txtEmailAddress.Enabled = false;
                        fsucCmdBar.Visible = false;
                    }
                }
                else
                {
                    fsucCmdBar.Visible = true;
                    dvlstExistingUsers.Visible = false;
                    txtEmailAddress.Enabled = true;
                    //base.ShowInfoMessage("Personal details do not match: Name must match in each account being linked.");
                    base.ShowInfoMessage(Resources.Language.PERSONALDETAILNOTMATCHED);
                    return;
                }
            }
        }


        protected void fsucCmdBar_CancelClick(object sender, EventArgs e)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            String _viewType = null;
            if (Request.QueryString["ucid"] != null && Request.QueryString["ucid"].ToString() == "ClientAdmin")
            {
                queryString = new Dictionary<String, String>{
                                                                    { "Child", AppConsts.ADMIN_EDITPROFILE_PAGE_NAME},
                                                                 };
                string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }
            else if (Request.QueryString["ucid"] != null && Request.QueryString["ucid"].ToString() == "Applicant")
            {
                queryString = new Dictionary<String, String>{
                                                                    { "Child", AppConsts.EDITPROFILE_PAGE_NAME},
                                                                    {"PageType", "MyProfile"}
                                                                 };
                string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);

            }
            else if (Request.QueryString["ucid"] != null && Request.QueryString["ucid"].ToString() == "Instructor")
            {
                queryString = new Dictionary<String, String>{
                                                                    { "Child", "~/ClinicalRotation/ClientContactProfile.ascx"},
                                                                    {"ucid",CurrentViewContext.InstructorPageType}
                                                                 };
                string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url);
            }
            else if (Request.QueryString["ucid"] != null && Request.QueryString["ucid"].ToString() == "AgencyUser")
            {
                queryString = new Dictionary<String, String>{
                                                                    { "Child", "~/ClinicalRotation/AgencyUserProfile.ascx"},
                                                                    {"IsApplicantsSharedUser",CurrentViewContext.IsApplicantsSharedUser.ToString()}
                                                                 };
                string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Link Account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbLinkAccount_SubmitClick(object sender, EventArgs e)
        {
            String _selectedUserId = String.Empty;
            String _otherUserAccount = String.Empty;

            if (rbSelectedUser1.Checked)
            {
                _selectedUserId = CurrentViewContext.lstUserContract[0].UserId;
                _otherUserAccount = CurrentViewContext.lstUserContract[1].UserId;
            }
            else if (rbSelectedUser2.Checked)
            {
                _otherUserAccount = CurrentViewContext.lstUserContract[0].UserId;
                _selectedUserId = CurrentViewContext.lstUserContract[1].UserId;
            }
            else
            {
                //base.ShowInfoMessage("Please select an user.");
                base.ShowInfoMessage(Resources.Language.PLSSELECTUSER);
                return;
            }

            if (!_selectedUserId.IsNullOrEmpty())
            {
                if (Presenter.LinkAccount(_selectedUserId, _otherUserAccount))
                {
                    //Redirect to Login page with a message.

                    var currentLanguage = LanguageTranslateUtils.GetCurrentLanguageCultureFromSession();
                    SysXWebSiteUtils.SessionService.ClearSession(true);
                    Response.Redirect(String.Format("{1}?IsRedirectedFromOtherAccountLinking={0}&lang={2}", "successful", FormsAuthentication.LoginUrl, currentLanguage));
                }
                else
                {
                    //base.ShowErrorInfoMessage("Account Linking Failed");
                    base.ShowErrorInfoMessage(Resources.Language.ACCTLINKINGFAILED);
                    return;
                }
            }


        }

        /// <summary>
        /// Validate the entered username and Password of the selected user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbUserCredentials_SubmitClick(object sender, EventArgs e)
        {
            if (!CurrentViewContext.ExistingUser.IsNullOrEmpty())
            {
                CurrentViewContext.UserName = CurrentViewContext.ExistingUser.Item1.Code;
                CurrentViewContext.Password = txtPassword.Text;
                CurrentViewContext.TargetUserID = CurrentViewContext.ExistingUser.Item1.OrganizationUserId.HasValue ? CurrentViewContext.ExistingUser.Item1.OrganizationUserId.Value.ToString() : String.Empty;
                if (Presenter.ValidateUserNameAndPassword())
                {
                    Presenter.GetSourceTargetDetails();
                    if (!CurrentViewContext.lstUserContract.IsNullOrEmpty())
                    {
                        rbSelectedUser1.Checked = false;
                        rbSelectedUser2.Checked = false;

                        dvUserDataSelection.Visible = true;
                        cmbUserCredentials.Visible = false;

                        lblUserName1.Text = CurrentViewContext.lstUserContract[0].UserName.HtmlEncode();
                        lblEmailAddress1.Text = CurrentViewContext.lstUserContract[0].EmailAddress.HtmlEncode();

                        lblUserName2.Text = CurrentViewContext.lstUserContract[1].UserName.HtmlEncode();
                        lblEmailAddress2.Text = CurrentViewContext.lstUserContract[1].EmailAddress.HtmlEncode();
                    }
                }
                else
                {
                    cmbUserCredentials.Visible = true;
                    dvUserDataSelection.Visible = false;
                    //base.ShowErrorInfoMessage("Invalid Password.");
                    base.ShowErrorInfoMessage(Resources.Language.INVALIDPASSWORD);
                    return;
                }
            }

        }
        #endregion
    }
}