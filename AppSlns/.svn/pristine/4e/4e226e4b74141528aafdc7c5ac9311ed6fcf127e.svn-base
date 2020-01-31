using CoreWeb.CommonOperations.Views;
using CoreWeb.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;

namespace CoreWeb.CommonOperations.Pages
{
    public partial class AuthenticationPopup : BaseWebPage, IAuthenticationPopupView
    {

        #region Properties

        #region Private Properties

        private AuthenticationPopupPresenter _presenter = new AuthenticationPopupPresenter();

        #endregion

        #region Public Properties

        public AuthenticationPopupPresenter Presenter
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

        IAuthenticationPopupView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String IAuthenticationPopupView.UserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.UserId;
            }
        }

        String IAuthenticationPopupView.AuthenticationCode
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

        String IAuthenticationPopupView.AuthenticationTitle
        {
            get
            {
                return SysXWebSiteUtils.SessionService.SysXMembershipUser.UserName;
            }
        }

        Int32 IAuthenticationPopupView.CurrentLoggedInUserID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                else
                    return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        String IAuthenticationPopupView.AuthenticationBarCodeImage
        {
            get;
            set;
        }

        String IAuthenticationPopupView.AuthenticationManualCode
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region PAGE METHODS

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                CheckTwofactorAuthentication();
        }

        #endregion

        #region PRIVATE METHODS
        private void CheckTwofactorAuthentication()
        {
            if (!CurrentViewContext.UserId.IsNullOrEmpty() && !CurrentViewContext.AuthenticationTitle.IsNullOrEmpty())
            {
                //user wants to verify its 2FA
                Presenter.GenerateTwoFactorAuthentication();
                imgQrCode.ImageUrl = CurrentViewContext.AuthenticationBarCodeImage;
                lblManualSetupCode.Text = CurrentViewContext.AuthenticationManualCode;
                lblAccountName.Text = CurrentViewContext.AuthenticationTitle;
            }
        }

        #endregion

        #region BUTTON EVENTS

        protected void cmdVerification_SaveClick(object sender, EventArgs e)
        {

            //verify the authentication.
            String pin = txtSecurityCode.Text.Trim();
            Boolean status = Presenter.ValidateTwoFactorPIN(pin);
            if (status)
            {
                Presenter.SaveTwoFactorAuthenticationData();
                lblErrorMessage.Visible = false;
                lblSuccessMessage.Visible = true;
                //lblSuccessMessage.Text = "Two factor authentication has been configured successfully.";
                lblSuccessMessage.Text = Resources.Language.TWOFACTORSUCCESSMSG;
                return;
            }
            else
            {
                //show error message
                lblSuccessMessage.Visible = false;
                lblErrorMessage.Visible = true;
                //lblErrorMessage.Text = "Please enter valid verification code.";
                lblErrorMessage.Text = Resources.Language.PLEASEENTERVERICODE;
                return;
            }
        }

        #endregion

    }
}