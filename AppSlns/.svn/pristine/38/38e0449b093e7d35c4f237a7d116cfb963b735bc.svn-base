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
    public partial class TwoFactorAuthenticationSettings : BaseUserControl, ITwoFactorAuthenticationSettingsView
    {
        #region Properties

        #region Private Properties

        private TwoFactorAuthenticationSettingsPresenter _presenter = new TwoFactorAuthenticationSettingsPresenter();

        #endregion

        #region Public Properties

        public TwoFactorAuthenticationSettingsPresenter Presenter
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

        ITwoFactorAuthenticationSettingsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String ITwoFactorAuthenticationSettingsView.UserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.UserId;
            }
        }

        Int32 ITwoFactorAuthenticationSettingsView.CurrentLoggedInUserID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                else
                    return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Boolean ITwoFactorAuthenticationSettingsView.IsVerified { get; set; }

        Boolean ITwoFactorAuthenticationSettingsView.Is2FAEnabled { get; set; }

        Int32 ITwoFactorAuthenticationSettingsView.UserOrgID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Entity.OrganisationUserTextMessageSetting ITwoFactorAuthenticationSettingsView.OrganisationUserTextMessageSettingData { get; set; }

        public String SelectedAuthenticationType
        {
            get
            {
                return rdbSpecifyAuthentication.SelectedValue;
            }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    rdbSpecifyAuthentication.SelectedValue = AuthenticationMode.None.GetStringValue();
                }
                else
                {
                    rdbSpecifyAuthentication.SelectedValue = value;
                }
            }

        }

        Boolean ITwoFactorAuthenticationSettingsView.IsApplicant
        {
            get
            {
                if (ViewState["IsApplicant"].IsNullOrEmpty())
                    ViewState["IsApplicant"] = SysXMembershipUser.IsApplicant;

                return Convert.ToBoolean(ViewState["IsApplicant"]);
            }
        }

        #endregion

        #endregion

        #region PAGE EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            CheckTwofactorAuthentication();
            Presenter.CheckAuthenticationType();
            //IsSMSSubscriptionConfirmed();
            //}

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (CurrentViewContext.IsApplicant)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "AddGoogleAuthenticatorLinkInRadioButton();", true);
                dvSpecifyAuthentication.Style["display"] = "block";
                lblAuthLabel.Style["display"] = "block";
                dvGoogleAuthentication.Style["display"] = "none";
            }
            else
            {
                dvSpecifyAuthentication.Style["display"] = "none";
                lblAuthLabel.Style["display"] = "none";
                dvGoogleAuthentication.Style["display"] = "block";
            }
        }

        #endregion

        #region PRIVATE METHODS

        private void CheckTwofactorAuthentication()
        {
            if (!CurrentViewContext.UserId.IsNullOrEmpty())
            {
                Presenter.CheckForTwoFactorAuthentication();
                if (!CurrentViewContext.Is2FAEnabled)
                {
                    //not enabled till now
                    lblAuthentication.Text = "[Not Enabled]" + "&nbsp;";
                    lnkAuthentication.Style["display"] = "block";
                    lnkDisableAuthentication.Style["display"] = "none";

                    //lnkAuthentication.InnerText = "Click here to enable";
                    lnkAuthentication.InnerText = Resources.Language.CLCKTOENBLE;
                    

                    rdbSpecifyAuthentication.Items[2].Enabled = false;
                    lblAuthLabel.Text = " - To use this setting please make sure to enable Google Authenticator." + "&nbsp;";
                    if (rdbSpecifyAuthentication.SelectedValue == AuthenticationMode.Google_Authenticator.GetStringValue())
                    {
                        //rdbSpecifyAuthentication.Items[0].Selected = true;
                        CurrentViewContext.SelectedAuthenticationType = AuthenticationMode.None.GetStringValue();
                    }
                }
                else if (!CurrentViewContext.IsVerified)
                {
                    //enabled but not verified
                    lblAuthentication.Text = "[Enabled - Not Verified]" + "&nbsp;";
                    lnkAuthentication.Style["display"] = "block";
                    lnkDisableAuthentication.Style["display"] = "none";
                    lnkAuthentication.InnerText = "(Click here to verify)";
                    rdbSpecifyAuthentication.Items[2].Enabled = false;
                }
                else
                {
                    // enabled and verified.
                    lblAuthentication.Text = "[Enabled]" + "&nbsp;";
                    lnkAuthentication.Style["display"] = "none";
                    lnkDisableAuthentication.Style["display"] = "block";
                    rdbSpecifyAuthentication.Items[2].Enabled = true;
                    // rdbSpecifyAuthentication.Items[1].Attributes["title"] = "";
                    lblAuthLabel.Text = "- Please " + "&nbsp;";

                }
            }
        }
        #endregion

        //#region PUBLIC METHODS

        //public void IsSMSSubscriptionConfirmed()
        //{
        //    //Presenter.UpdateSubscriptionStatusFromAmazon();
        //    Presenter.GetUserSMSNotificationData();

        //    if (!CurrentViewContext.OrganisationUserTextMessageSettingData.IsNullOrEmpty()
        //               && CurrentViewContext.OrganisationUserTextMessageSettingData.OUTMS_ID > AppConsts.NONE
        //               && !CurrentViewContext.OrganisationUserTextMessageSettingData.OUTMS_IsDeleted
        //               && CurrentViewContext.OrganisationUserTextMessageSettingData.OUTMS_ReceiveTextNotification)
        //    {
        //        rdbSpecifyAuthentication.Items[1].Enabled = true;
        //        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "SetOnChangeEventOnRadioButtonList();", true);
        //    }
        //    else
        //    {
        //        rdbSpecifyAuthentication.Items[1].Enabled = false;
        //        if (rdbSpecifyAuthentication.SelectedValue == AuthenticationMode.Text_Message.GetStringValue())
        //            rdbSpecifyAuthentication.SelectedValue = AuthenticationMode.None.GetStringValue();
        //    }
        //}

        //#endregion

        #region BUTTON CLICK
        protected void btnHiddenDisable_Click(object sender, EventArgs e)
        {
            //disable 2FA
            Boolean status = Presenter.DeleteTwofactorAuthenticationForUserID();
            if (status)
                CheckTwofactorAuthentication();

        }
        #endregion
    }
}