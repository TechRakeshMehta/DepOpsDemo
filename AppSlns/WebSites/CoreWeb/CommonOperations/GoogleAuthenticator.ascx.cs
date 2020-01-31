using CoreWeb.CommonOperations.Views;
using CoreWeb.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;

namespace CoreWeb.CommonOperations.Views
{
    public partial class GoogleAuthenticator : BaseUserControl, IGoogleAuthenticatorView
    {

        #region Private Variables

        private GoogleAuthenticatorPresenter _presenter = new GoogleAuthenticatorPresenter();

        #endregion

        #region Properties
        public GoogleAuthenticatorPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        IGoogleAuthenticatorView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String IGoogleAuthenticatorView.UserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.UserId;
            }
        }

        String IGoogleAuthenticatorView.AuthenticationCode
        {
            get
            {
                if (ViewState["AuthenticationCode"].IsNotNull())
                    return ViewState["AuthenticationCode"].ToString().Trim();
                return String.Empty;
            }
            set
            {
                ViewState["AuthenticationCode"] = value.Trim();
            }
        }

        String IGoogleAuthenticatorView.UserFullName
        {
            get
            {
                return string.Concat(SysXMembershipUser.FirstName, " ", SysXMembershipUser.LastName);
            }
        }

        String IGoogleAuthenticatorView.UserPrimaryEmailAddress
        {
            get
            {
                return SysXMembershipUser.Email;
            }
        }

        Int32 IGoogleAuthenticatorView.OrgUserId
        {
            get
            {
                return SysXMembershipUser.OrganizationUserId;
            }
        }
        Int32 IGoogleAuthenticatorView.TenantId
        {
            get
            {

                return SysXMembershipUser.TenantId.Value;
            }
        }

        #endregion

        #region PAGE EVENTS
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SysXWebSiteUtils.SessionService.UserGoogleAuthenticated == GoogleAuthenticationStatus.NotAuthenticated_With_GoogleAuthenticator)
            {
                if (!Page.IsPostBack)
                    Presenter.GetTwofactorAuthenticationForUserID();
                dvGoogleAuth.Style["display"] = "block";
                dvTextMessageAuth.Style["display"] = "none";
                txtVerificationCode.Focus();
            }
            else if (SysXWebSiteUtils.SessionService.UserGoogleAuthenticated == GoogleAuthenticationStatus.NotAuthenticated_With_TextMessage)
            {
                if (!Page.IsPostBack)
                {
                    String authOTP = Presenter.SendOTP();
                    if (!authOTP.IsNullOrEmpty())
                    {
                        Session["AuthOTP"] = authOTP;
                    }
                }
                dvGoogleAuth.Style["display"] = "none";
                dvTextMessageAuth.Style["display"] = "block";
                txtTextMessage.Focus();
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }

        }

        #endregion

        #region BUTTON EVENTS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            String pin = txtVerificationCode.Text.Trim();
            if (Presenter.ValidateTwoFactorPIN(pin))
            {
                RedirectToLoginAfterVerification();
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Invalid Verification Code. Please try again.";
                return;
            }
        }

        private void RedirectToLoginAfterVerification()
        {
            SysXWebSiteUtils.SessionService.UserGoogleAuthenticated = GoogleAuthenticationStatus.Authenticated;
            if (!Session["PreLoginQueryString"].IsNullOrEmpty())
            {
                String queryString = Convert.ToString(Session["PreLoginQueryString"]);
                Session["PreLoginQueryString"] = null;
                Response.Redirect("~/Login.aspx?args=" + queryString);
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // empty the user session and redirect to login page.
            SysXWebSiteUtils.SessionService.ClearSession(true);
            Session["PreLoginQueryString"] = null;
            Response.Redirect("~/Login.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTextMessageverify_Click(object sender, EventArgs e)
        {
            String pin = txtTextMessage.Text.Trim();
            if (!Session["AuthOTP"].IsNullOrEmpty() && pin.ToLower() == Convert.ToString(Session["AuthOTP"]).ToLower())
            {
                RedirectToLoginAfterVerification();
            }
            else
            {
                lblTextAuthMessage.Visible = true;
                lblConfirmationMessage.Visible = false;
                lblTextAuthMessage.Text = "Invalid Verification Code. Please try again.";
                return;
            }

        }

        protected void btnResendOTP_Click(object sender, EventArgs e)
        {
            //to do messagge
            String authOTP = Presenter.SendOTP();
            if (!authOTP.IsNullOrEmpty())
            {
                Session["AuthOTP"] = authOTP;
                txtTextMessage.Text = String.Empty;
                lblConfirmationMessage.Visible = true;
                lblTextAuthMessage.Visible = false;
                lblConfirmationMessage.Text = "OTP sent successfully.";
                return;
            }
        }
        #endregion
    }
}