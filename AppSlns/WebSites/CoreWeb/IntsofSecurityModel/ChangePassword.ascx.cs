#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ChangePassword.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region Application Specific

using INTSOF.Utils;
using CoreWeb.Shell;
using INTSOF.UI.Contract.IntsofSecurityModel;
using System.Threading;
using Resources;
using System.Collections.Generic;


#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to change password section in security module.
    /// </summary>
    public partial class ChangePassword : BaseUserControl, IChangePasswordView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ChangePasswordPresenter _presenter = new ChangePasswordPresenter();
        private ChangePasswordContract _viewContract;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Presenter.
        /// </summary>
        /// <value>
        /// Represents Manage Tenant Presenter.
        /// </value>

        public ChangePasswordPresenter Presenter
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
        /// ErrorMessage.
        /// </summary>
        /// <value>
        /// Gets or sets the value for error message.
        /// </value>
        String IChangePasswordView.ErrorMessage
        {
            get
            {
                return lblMessage.Text;
            }
            set
            {
                lblMessage.Text = value;
            }
        }

        /// <summary>
        /// OldPassword.
        /// </summary>
        /// <value>
        /// Gets or sets the value for Old Password.
        /// </value>
        String IChangePasswordView.OldPassword
        {
            get
            {
                return txtOldPassword.Text.Trim();
            }
            set
            {
                txtOldPassword.Text = value;
            }
        }

        /// <summary>
        /// NewPassword.
        /// </summary>
        /// <value>
        /// Gets or sets the value for New Password.
        /// </value>
        String IChangePasswordView.NewPassword
        {
            get
            {
                return txtNewPassword.Text.Trim();
            }
            set
            {
                txtNewPassword.Text = value;
            }
        }

        /// <summary>
        /// DefaultLineOfBusiness</summary>
        /// <value>
        /// Gets or sets the value for Default Line of Business.</value>
        String IChangePasswordView.DefaultLineOfBusiness
        {
            get;
            set;
        }

        /// <summary>
        /// ConfirmPassword.
        /// </summary>
        /// <value>
        /// Gets or sets the value for Confirm Password.
        /// </value>
        String IChangePasswordView.ConfirmPassword
        {
            get
            {
                return txtCmpPassword.Text.Trim();
            }
            set
            {
                txtCmpPassword.Text = value;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <value>
        /// The view contract.
        /// </value>
        ChangePasswordContract IChangePasswordView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ChangePasswordContract();
                }

                return _viewContract;
            }
        }

        /// <summary>
        /// Property to distinguish, if user is changing password for first time login or from account section
        /// </summary>
        public Boolean IsFirstTimeLogin
        {
            get
            {
                return ViewState["IsFirstTimeLogin"].IsNullOrEmpty() ? false : true;
            }
            set
            {
                ViewState["IsFirstTimeLogin"] = value;
            }
        }

        //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        Int32 IChangePasswordView.OrgUsrID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                else
                    return base.CurrentUserId;
            }
        }

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <value>
        /// The current view context.
        /// </value>
        IChangePasswordView CurrentViewContext
        {
            get
            {
                return this;
            }
        }


        string IChangePasswordView.OLDPASWRDNOTMATCH
        {
            get
            {
                return Resources.Language.OLDPASWRDNOTMATCH;
            }
        }

        string IChangePasswordView.PSWRDCHNGESUCSESFLY
        {
            get
            {
                return Resources.Language.PSWRDCHNGESUCSESFLY;
            }
        }

        private Boolean IsTempPassReset
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsTempPassReset"]);
            }
            set
            {
                ViewState["IsTempPassReset"] = value;
            }
        }
        #endregion

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// This method sets the validation for all the control.
        /// </summary>
        /// <param name="validatorId"> The id of validation control.</param>
        /// <param name="errorMessage">The error message need to be set.</param>
        private void SetValidations(String validatorId, String errorMessage)
        {
            BaseValidator baseValidator = (BaseValidator)FindControl(validatorId);
            //baseValidator.ErrorMessage = SysXUtils.GetMessage(errorMessage);
            //baseValidator.ErrorMessage = SysXMessages.ResourceManager.GetString(errorMessage);
            baseValidator.ErrorMessage = errorMessage;
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// Override this method and set IsPolicyEnable = false to disable policy settings. - TG
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                //base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_CHANGE_PASSWORD);
                base.BreadCrumbTitleKey = "Key_CHGEPASSWRD";
                base.Title = Resources.Language.CHGEPASSWRD;
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                //lblChangedPwdTopMsgPart1.Text = SysXMessages.ResourceManager.GetString(ResourceConst.LBL_CHANGED_PWD_TOP_MSG_PART1);
                lblChangedPwdTopMsgPart1.Text = Resources.Language.PSWDCONDITION;
                base.OnInit(e);
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

        /// <summary>
        /// Pre render event for setting the values before the page gets rendered..
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["ChangePasswordStatus"] = Session["ChangePasswordStatus"];
        }

        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.RegisterControlForPostBack(btnUpdate);
                base.RegisterControlForPostBack(btnCancel);

                if (!this.IsPostBack)
                {
                    //UAT-1850: On Applicant PW reset, change the "Old Password" field to read "Temporary Password"
                    Dictionary<String, String> args = new Dictionary<String, String>();
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    IsTempPassReset = args.ContainsKey("IsTempPassReset");
                    if (IsTempPassReset)
                    {
                        lblOldPassword.Text = "Temporary Password";
                    }

                    SysXMembershipUser user = null;
                    if (!Session["UserVerified"].IsNullOrEmpty() && Session["UserVerified"].ToString() == "InitResetPassword")
                    {
                        ilOldPswd.Visible = false;
                        //ilOldPswd.Attributes.Add("style", "display:none;");
                        Session["UserVerified"] = "PostResetPassword";
                        List<Entity.OrganizationUser> lstOrganizationUser = Business.RepoManagers.SecurityManager.GetOrganizationUsersByEmail(Session["VerifiedMail"].ToString());

                        foreach (var item in lstOrganizationUser)
                        {
                            user = System.Web.Security.Membership.GetUser(item.UserID) as SysXMembershipUser;
                        }
                        txtNewPassword.Focus();
                        tltpCatExplanation.Show();
                    }
                    //UAT-1850: On Applicant PW reset, change the "Old Password" field to read "Temporary Password"
                    else if (IsTempPassReset)
                    {
                        ilOldPswd.Visible = true;
                        SetValidations("rfvOldPassword", ResourceConst.SECURITY_TEMP_PASSWORD);
                        user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                        txtOldPassword.Focus();
                    }
                    else
                    {
                        ilOldPswd.Visible = true;
                        //ilOldPswd.Attributes.Add("style", "display:block;");
                        //SetValidations("rfvOldPassword", ResourceConst.SECURITY_OLD_PASSWORD);
                        SetValidations("rfvOldPassword", Resources.Language.OLDPASWRDREQ);
                        user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                        txtOldPassword.Focus();
                    }
                    //SetValidations("revOldPassword", ResourceConst.SECURITY_INVALID_CHARACTER);
                    //SetValidations("rfvNewPassword", ResourceConst.SECURITY_NEW_PASSWORD);
                    SetValidations("rfvNewPassword", Resources.Language.NEWPASWRDREQ);

                    //UAT-1850: On Applicant PW reset, change the "Old Password" field to read "Temporary Password"
                    if (IsTempPassReset)
                    {
                        SetValidations("cmpvalNewWithOldPwd", ResourceConst.SECURITY_TEMP_PASSWORD_MATCHING);
                    }
                    else
                    {
                        //SetValidations("cmpvalNewWithOldPwd", ResourceConst.SECURITY_PASSWORD_MATCHING);
                        SetValidations("cmpvalNewWithOldPwd", Resources.Language.OLDPASWRDCNTSAMETONEWPASWRD);
                    }
                    //SetValidations("revNewPassword", ResourceConst.SECURITY_PASSWORD_LENGTH);
                    SetValidations("revNewPassword", Resources.Language.PASWRDCONDITIONS);
                    //SetValidations("rfvCmpPassword", ResourceConst.SECURITY_PASSWORD_CONFIRMATION);
                    SetValidations("rfvCmpPassword", Resources.Language.CNFMPASSWORDREQ);
                    //SetValidations("cmpvalCmpPassword", ResourceConst.SECURITY_NEW_CONFIRM_PASSWORD_MATCHING);
                    SetValidations("cmpvalCmpPassword", Resources.Language.NEWPASWRDSHOULDSAME);

                    Session["ChangePasswordStatus"] = Server.UrlEncode(DateTime.Now.ToString());

                    if (user.IsNull())
                    {
                        Response.Redirect("Default.aspx", false);
                    }

                    Presenter.OnViewInitialized();
                    cmbSxBlocks.DataSource = this.Presenter.GetLineOfBusinessesByUser(Convert.ToString(user.UserId));
                    cmbSxBlocks.DataBind();

                    if (cmbSxBlocks.Items.Count.Equals(Convert.ToInt32(AppConsts.ZERO)) && Presenter.CheckIfUserIsApplicantOrSharedUser(user.OrganizationUserId) == true)
                    {
                        cmbSxBlocks.Visible = false;
                        lblSxBlocks.Visible = false;
                    }

                    //UAT-4312
                    Boolean isUserExixtInLocationTenants = false;
                    isUserExixtInLocationTenants = Presenter.IsUserExixtInLocationTenants(user.UserId);
                    if (isUserExixtInLocationTenants){ 
                        specialchar.Visible = false;
                    }
                    else{
                        specialcharcbi.Visible = false;
                    }
                }

                Presenter.OnViewLoaded();
                lblMessage.Text = String.Empty;
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
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

        /// <summary>
        /// Update the Password.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["ChangePasswordStatus"]).Equals(Convert.ToString(ViewState["ChangePasswordStatus"])))
                {
                    SysXMembershipUser user = null;
                    if (ilOldPswd.Visible && Membership.ValidateUser(SysXWebSiteUtils.SessionService.SysXMembershipUser.UserName, txtOldPassword.Text.Trim()))
                    {
                        user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                        //CurrentViewContext.ViewContract.OrganizationUserId = user.OrganizationUserId;
                        CurrentViewContext.ViewContract.Email = user.Email;
                        //UAT-4097
                        Boolean isUserExixtInLocationTenants = false;
                        isUserExixtInLocationTenants = Presenter.IsUserExixtInLocationTenants(user.UserId);

                        if (Presenter.IsPasswordExistsInHistory(user.UserId, isUserExixtInLocationTenants))
                        {
                            //lblMessage.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_MATCH_PREVIOUS_PASSWORDS);
                            if (!isUserExixtInLocationTenants)
                                lblMessage.Text = Resources.Language.PASWRDSHOULDNTSAMETOOLDFIVEPASWRD;
                            else
                                lblMessage.Text = Resources.Language.PASWRDSHOULDNTSAMETOOLDTENPASWRD;
                        }
                        else
                        {
                            if ((txtNewPassword.Text.Trim().Equals(txtCmpPassword.Text.Trim())) && !(txtNewPassword.Text.Trim().Equals(txtOldPassword.Text.Trim())))
                            {
                                CurrentViewContext.ViewContract.OperationStatus = false;
                                CurrentViewContext.DefaultLineOfBusiness = cmbSxBlocks.SelectedValue;
                                Presenter.UpdatePassword();
                                var currentLanguage = LanguageTranslateUtils.GetCurrentLanguageCultureFromSession();
                                SysXWebSiteUtils.SessionService.ClearSession(true);
                                //LanguageTranslateUtils.SetLanguageInSession(t);
                                Response.Redirect(String.Format("{1}?ChangePassword={0}&lang={2}", "successful", FormsAuthentication.LoginUrl, currentLanguage));
                            }
                        }
                    }
                    else if (!Session["UserVerified"].IsNullOrEmpty() && Session["UserVerified"].ToString() == "PostResetPassword")
                    {
                        List<Entity.OrganizationUser> lstOrganizationUser = Business.RepoManagers.SecurityManager.GetOrganizationUsersByEmail(Session["VerifiedMail"].ToString());

                        foreach (var item in lstOrganizationUser)
                        {
                            user = System.Web.Security.Membership.GetUser(item.UserID) as SysXMembershipUser;
                        }
                        //UAT-4097
                        Boolean isUserExixtInLocationTenants = false;
                        isUserExixtInLocationTenants = Presenter.IsUserExixtInLocationTenants(user.UserId);

                        if (Presenter.IsPasswordExistsInHistory(user.UserId, isUserExixtInLocationTenants))
                        {
                            //lblMessage.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_MATCH_PREVIOUS_PASSWORDS);
                            if (!isUserExixtInLocationTenants)
                                lblMessage.Text = Resources.Language.PASWRDSHOULDNTSAMETOOLDFIVEPASWRD;
                            else
                                lblMessage.Text = Resources.Language.PASWRDSHOULDNTSAMETOOLDTENPASWRD;
                        }
                        else
                        {
                            if (txtNewPassword.Text.Trim().Equals(txtCmpPassword.Text.Trim()))
                            {
                                CurrentViewContext.ViewContract.OperationStatus = false;
                                CurrentViewContext.DefaultLineOfBusiness = cmbSxBlocks.SelectedValue;
                                Presenter.UpdatePassword(Session["VerifiedMail"].ToString());
                                var currentLanguage = LanguageTranslateUtils.GetCurrentLanguageCultureFromSession();
                                SysXWebSiteUtils.SessionService.ClearSession(true);
                                Response.Redirect(String.Format("{1}?ChangePassword={0}&lang={2}", "successful", FormsAuthentication.LoginUrl, currentLanguage));
                            }
                        }
                    }
                    else
                    {
                        //UAT-1850: On Applicant PW reset, change the "Old Password" field to read "Temporary Password".
                        if (IsTempPassReset)
                        {
                            //lblMessage.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_PASSWORD_ENTER_CORRECT_RECEIVED_PASSWORD);
                            lblMessage.Text = Resources.Language.INVALIDTEMPPASWRDENT;
                        }
                        else
                        {
                            //lblMessage.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_OLD_PASSWORD_MATCHING);
                            lblMessage.Text = Resources.Language.OLDPASWRDNOTMATCH;
                        }
                    }

                    Session["ChangePasswordStatus"] = Server.UrlEncode(DateTime.Now.ToString());
                }
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
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

        /// <summary>
        /// Cancel the operation.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.IsFirstTimeLogin)
            {
                SysXWebSiteUtils.SessionService.ClearSession(true);
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            //SysXWebSiteUtils.SessionService.ClearSession(true);
            //Response.Redirect(FormsAuthentication.LoginUrl);
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

                if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
                {
                    queryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                }
                //to check if applicant has come from dashboard
                String pageType = String.Empty;
                //Checks if the OrderId is present in Query String.
                if (queryString.ContainsKey("PageType"))
                {
                    pageType = queryString["PageType"];
                }
                if (pageType == AppConsts.DASHBOARD)
                {
                    Response.Redirect(AppConsts.DASHBOARD_URL);
                }
                else
                {
                    queryString = new Dictionary<String, String>{
                                                                    { "Child", AppConsts.EDITPROFILE_PAGE_NAME},
                                                                    {"PageType", "MyProfile"}
                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
            }
            else if (Request.QueryString["ucid"] != null && Request.QueryString["ucid"].ToString() == "Instructor")
            {
                if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
                {
                    queryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                }
                //to check if instructor has come from dashboard
                String pageType = String.Empty;
                if (queryString.ContainsKey("PageType"))
                {
                    pageType = queryString["PageType"];
                }
                if (pageType == "InstructorDashboard")
                {
                    queryString = new Dictionary<String, String>{
                                                                    { "Child", "~/ProfileSharing/InstructorPreceptorDashboard.ascx"},
                                                                    {"MenuID","1"}
                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url);
                }
                else
                {
                    queryString = new Dictionary<String, String>{
                                                                    { "Child", "~/ClinicalRotation/ClientContactProfile.ascx"},
                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url);
                }
            }
            //UAT-4475
            else if (Request.QueryString["ucid"] != null && Request.QueryString["ucid"].ToString() == "AgencyUser")
            {
                queryString = new Dictionary<String, String>{
                                                                    { "Child", AppConsts.AGENCY_USER_PROFILE_URL},
                                                                 };
                string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }
            else
            {
                Response.Redirect("~/Dashboard/Pages/ApplicantDashboardMain.aspx");
            }
        }

        #endregion

        #region Grild Related Events

        #endregion

        #endregion
    }
}