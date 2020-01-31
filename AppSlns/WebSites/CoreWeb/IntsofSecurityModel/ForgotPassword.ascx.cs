#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ForgotPassword.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web.Security;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region Application Specific

using INTSOF.Utils;
using INTSOF.UI.Contract.IntsofSecurityModel;
using System.Threading;
using Business.RepoManagers;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to forgot password section of security module.
    /// </summary>
    public partial class ForgotPassword : BaseUserControl, IForgotPasswordView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ForgotPasswordPresenter _presenter = new ForgotPasswordPresenter();
        private ForgotPasswordContract _viewContract;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Presenter.
        /// </summary>
        /// <value>
        /// Represents Manage Tenant Presenter.
        /// </value>

        public ForgotPasswordPresenter Presenter
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
        String IForgotPasswordView.ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// password Reset mode.
        /// </summary>
        /// <value>
        /// Is UseName to be reset.
        /// </value>
        Boolean IForgotPasswordView.IsUserNameReset
        {
            get
            {
                return rdlRecoverOpt.SelectedValue.Equals("Username") ? true : false;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <value>
        /// The view contract.
        /// </value>
        ForgotPasswordContract IForgotPasswordView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ForgotPasswordContract();
                }

                return _viewContract;
            }
        }

        //Properties for globalization for multi-language.
        String IForgotPasswordView.IncorrectVerificationCode
        {
            get
            {
                return Resources.Language.ENTRCRCTVERIFICATIONCODE;
            }

        }

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <value>
        /// The current view context.
        /// </value>
        IForgotPasswordView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #endregion

        public Int32 cbi_TenantId
        {
            get
            {
                if (!ViewState["cbi_TenantId"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["cbi_TenantId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["cbi_TenantId"] = value;
            }

        }

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
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_FORGOT_PASSWORD);
                base.OnInit(e);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Page load event.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.RegisterControlForPostBack(btnSave);
                base.RegisterControlForPostBack(btnCancel);

                if (!this.IsPostBack)
                {


                    //SetValidations("revEmail", ResourceConst.SECURITY_EMAIL_FORMAT);
                    SetValidations("revEmail", Resources.Language.NTCORRECTFORMATEMAIL);
                    //SetValidations("rfvmail", ResourceConst.SECURITY_EMAIL_REQUIRED);
                    SetValidations("rfvmail", Resources.Language.EMAILREQ);
                    //SetValidations("rfvCode", ResourceConst.SECURITY_CODE_REQUIRED);
                    SetValidations("rfvCode", Resources.Language.CODEREQ);
                    //SetValidations("revExpTxtCode", ResourceConst.SECURITY_INVALID_CHARACTER);
                    SetValidations("revExpTxtCode", Resources.Language.INVALIDCHARS);
                    rdlRecoverOpt.SelectedIndex = 1;
                    Presenter.OnViewInitialized();
                    Session["ForgotPasswordStatus"] = Server.UrlEncode(DateTime.Now.ToString());
                }
                txtEmail.Focus();

                Presenter.OnViewLoaded();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Use to set view state.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:System.EventArgs"></see> object that contains the event
        ///  data.</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["ForgotPasswordStatus"] = Session["ForgotPasswordStatus"];
            ViewState["TempForgotPasswordStatus"] = Session["ForgotPasswordStatus"];
        }

        /// <summary>
        /// Generate the validation Code.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:System.EventArgs"></see> object that contains the event
        ///  data.</param>
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                SysXMembershipUser user = Membership.GetUser(txtEmail.Text) as SysXMembershipUser;

                if (!user.IsNull() && user.IsLockedOut)
                {
                    lblMessage.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_ACCONT_ALREADY_LOCKED_CONTACT_TO_ADMIN);
                    return;
                }

                if (Convert.ToString(ViewState["TempForgotPasswordStatus"]).Equals(Convert.ToString(ViewState["ForgotPasswordStatus"])))
                {
                    CurrentViewContext.ViewContract.VerificationCode = radCpatchaVerificationCode.CaptchaImage.Text.Trim();
                    CurrentViewContext.ViewContract.Email = txtEmail.Text.Trim();
                    CurrentViewContext.ViewContract.OperationStatus = false;
                    //Presenter.HandleGenerateCode();
                    Presenter.HandleGenerateCode(Resources.Language.REGISTEREDEMAILREQUIRED);
                    CurrentViewContext.ViewContract.TenantName = AppUtils.GetInstitutionName;
                    if (CurrentViewContext.ViewContract.OperationStatus)
                    {
                        Dictionary<String, Object> contents = new Dictionary<String, Object>
                                                                  {
                                                                      //{"UserName",CurrentViewContext.ViewContract.UserName},
                                                                      //{"Code",CurrentViewContext.ViewContract.VerificationCode},
                                                                      // {"TenantName",CurrentViewContext.ViewContract.TenantName}
                                                                      {EmailFieldConstants.USER_FULL_NAME,CurrentViewContext.ViewContract.UserName},
                                                                      {EmailFieldConstants.VERIFICATION_CODE,CurrentViewContext.ViewContract.VerificationCode},
                                                                      {EmailFieldConstants.INSTITUTE_NAME,CurrentViewContext.ViewContract.TenantName},
                                                                      {EmailFieldConstants.TENANT_ID,CurrentViewContext.cbi_TenantId },
                                                                      {EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID,CurrentViewContext.ViewContract.OrganizationUserId}
                                                                  };
                        Boolean mailStatus;
                        if (CurrentViewContext.IsUserNameReset)
                        {
                            //SysXEmailService.SendMail(contents, SysXUtils.GetMessage(ResourceConst.SECURITY_VERIFICATION_CODE_DETAILS) + CurrentViewContext.ViewContract.UserName, CurrentViewContext.ViewContract.Email, AppConsts.VerficationCodeUserName, AppConsts.Normal);
                            SecurityManager.PrepareAndSendSystemMail(CurrentViewContext.ViewContract.Email, contents, CommunicationSubEvents.NOTIFICATION_VERIFICATION_CODE_USERNAME, cbi_TenantId, false);
                            mailStatus = true;
                        }
                        else
                        {
                            //mailStatus = SysXEmailService.SendMail(contents, SysXUtils.GetMessage(ResourceConst.SECURITY_VERIFICATION_CODE_DETAILS) + CurrentViewContext.ViewContract.UserName
                            //    , CurrentViewContext.ViewContract.Email, AppConsts.VerficationCode, AppConsts.Normal);
                            SecurityManager.PrepareAndSendSystemMail(CurrentViewContext.ViewContract.Email, contents, CommunicationSubEvents.NOTIFICATION_VERIFICATION_CODE_PASSWORD, cbi_TenantId, false);
                            mailStatus = true;
                        }
                        if (mailStatus)
                        {
                            txtEmail.Enabled = false;
                            rdlRecoverOpt.Enabled = false;
                            txtCode.Enabled = true;
                            btnGenerate.Enabled = false;
                            pnlForgotPassword.DefaultButton = "btnSave";
                            valSumForgotPassword.ValidationGroup = "ValgrpForgetpasscode";
                            //lblMessage.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_VERIFICATION_EMAIL);
                            lblMessage.Text = Resources.Language.VERIFICATIONCODEEMAILSNTINFOMSG;
                            btnSave.Enabled = true;
                            txtCode.Focus();
                            Spncode.Visible = true;
                            Session["ForgotPasswordStatus"] = Server.UrlEncode(DateTime.Now.ToString());
                        }
                        else
                        {
                            lblMessage.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_VERIFICATION_EMAIL_FAILURE);
                        }
                    }
                    else
                    {
                        lblMessage.Text = CurrentViewContext.ErrorMessage;
                        txtEmail.Focus();
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Send the Password.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:System.EventArgs"></see> object that contains the event
        ///  data.</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.VerificationCode = txtCode.Text;
                CurrentViewContext.ViewContract.ResetPassword = radCpatchaPassword.CaptchaImage.Text;
                CurrentViewContext.ViewContract.Email = txtEmail.Text;
                CurrentViewContext.ViewContract.OperationStatus = false;                
                CurrentViewContext.ViewContract.TenantName = AppUtils.GetInstitutionName;
                Presenter.HandleSave();

                if (CurrentViewContext.ViewContract.OperationStatus)
                {
                    Boolean mailStatus;
                    if (CurrentViewContext.IsUserNameReset)
                    {
                        Dictionary<String, Object> contents = new Dictionary<String, Object>
                                                              {
                                                                  //{"UserName", CurrentViewContext.ViewContract.UserName},
                                                                  //{"LoginUserName", CurrentViewContext.ViewContract.LoginUserName},
                                                                  //{"TenantName",CurrentViewContext.ViewContract.TenantName}
                                                                  {EmailFieldConstants.USER_FULL_NAME, CurrentViewContext.ViewContract.UserName},
                                                                  {EmailFieldConstants.USER_NAME,CurrentViewContext.ViewContract.LoginUserName},
                                                                  {EmailFieldConstants.INSTITUTE_NAME,CurrentViewContext.ViewContract.TenantName},
                                                                  {EmailFieldConstants.TENANT_ID,CurrentViewContext.cbi_TenantId},
                                                                  {EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID,CurrentViewContext.ViewContract.OrganizationUserId}
                                                              };

                        //c
                        SecurityManager.PrepareAndSendSystemMail(CurrentViewContext.ViewContract.Email, contents, CommunicationSubEvents.NOTIFICATION_FORGET_USERNAME_RESET, cbi_TenantId, false);
                        mailStatus = true;
                        // mailStatus = SysXEmailService.SendMail(contents, ResourceConst.SECURITY_USERNAME_REQUEST_DETAILS, CurrentViewContext.ViewContract.Email, AppConsts.ForgotUserNameReset, AppConsts.Normal);
                    }
                    else
                    {
                        // 28/08/2014 UAT-700 Implementated
                        //Dictionary<String, Object> contents = new Dictionary<String, Object>
                        //                                      {
                        //                                          //{"UserName", CurrentViewContext.ViewContract.UserName},
                        //                                          //{"Password", CurrentViewContext.ViewContract.ResetPassword},
                        //                                          //{"TenantName",CurrentViewContext.ViewContract.TenantName}
                        //                                          {EmailFieldConstants.USER_FULL_NAME,CurrentViewContext.ViewContract.UserName},
                        //                                          {EmailFieldConstants.PASSWORD,CurrentViewContext.ViewContract.ResetPassword},
                        //                                          {EmailFieldConstants.INSTITUTE_NAME,CurrentViewContext.ViewContract.TenantName}
                        //                                      };
                        //SecurityManager.PrepareAndSendSystemMail(CurrentViewContext.ViewContract.Email, contents, CommunicationSubEvents.NOTIFICATION_FORGET_PASSWORD_RECOVER, null);
                        mailStatus = true;
                        Session["UserVerified"] = "InitResetPassword";
                        Session["VerifiedMail"] = txtEmail.Text;
                        //ADB password reset request details
                        //mailStatus = SysXEmailService.SendMail(contents, SysXUtils.GetMessage(ResourceConst.SECURITY_PASSWORD_RESET_REQUEST_DETAILS), CurrentViewContext.ViewContract.Email, AppConsts.ForgotPasswordReset, AppConsts.Normal);
                    }

                    if (mailStatus)
                    {
                        if (CurrentViewContext.IsUserNameReset)
                        {
                            Server.Transfer(String.Format("{1}?ForgotUserName={0}", (new Dictionary<String, String> { { "ForgotUserName", String.Empty } }).ToEncryptedQueryString(), FormsAuthentication.LoginUrl));
                        }
                        else
                        {
                            Response.Redirect("ChangePassword.aspx");
                            //Server.Transfer(String.Format("{1}?ForgotPassword={0}", (new Dictionary<String, String> { { "ForgotPassword", String.Empty } }).ToEncryptedQueryString(), FormsAuthentication.LoginUrl));
                        }
                    }
                    else
                    {
                        lblMessage.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_VERIFICATION_EMAIL_FAILURE);
                    }
                }
                else
                {
                    lblMessage.Text = CurrentViewContext.ErrorMessage;
                    txtCode.Focus();
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
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Cancel the operation.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:System.EventArgs"></see> object that contains the event
        ///  data.</param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(FormsAuthentication.LoginUrl);
        }

        #endregion

        #region Grid Related Events

        #endregion

        #endregion

        #region Method

        #region Public Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// This method sets the validation for all the control.
        /// </summary>
        /// <param name="validatorId">The id of validation control.</param>
        /// <param name="errorMessage">The error message need to be set.</param>
        private void SetValidations(String validatorId, String errorMessage)
        {
            BaseValidator baseValidator = (BaseValidator)FindControl(validatorId);
            //   baseValidator.ErrorMessage = SysXUtils.GetMessage(errorMessage);
            baseValidator.ErrorMessage = errorMessage;
        }

        #endregion

        #endregion
    }
}