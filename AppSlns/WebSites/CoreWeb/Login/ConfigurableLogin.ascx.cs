#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ConfigurableLogin.ascx.cs
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
using INTERSOFT.WEB.UI.WebControls;
using System.Globalization;
using INTSOF.UI.Contract.Globalization;
using System.Resources;


#endregion

#endregion
namespace CoreWeb.Shell.Views
{
    public partial class ConfigurableLogin : BaseUserControl, IConfigurableLoginView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables
        private ConfigurableLoginPresenter _presenter = new ConfigurableLoginPresenter();
        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Presenter</summary>
        /// <value>
        /// Represents Common Login Presenter.</value>

        public ConfigurableLoginPresenter Presenter
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

        public IConfigurableLoginView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// property to get the curent year for copyright.
        /// </summary>
        public String CopyRightYear
        {
            get { return DateTime.Now.Year.ToString(); }
        }

        public String FooterGroupName
        {
            get
            {
                return Presenter.GetLocationTenantCompanyName();
            }
        }

        /// <summary>
        /// ErrorMessage</summary>
        /// <value>
        /// Sets the value for Error Message.</value>
        public String ErrorMessage
        {
            set
            {
                lblErrorMessage.Text = value;
                //UAT-2792
                if (!lblErrorMessage.Text.IsNullOrEmpty())
                {
                    dvShibbolethMessage.Style.Add("display", "none");
                }
            }
        }

        Boolean IConfigurableLoginView.IsAccountInActive
        {
            set
            {
                btnResendActivationLink.Visible = value;
                lblErrorMessageExtended.Visible = value;
                if (value)
                {
                    lblErrorMessageExtended.Text = "to resend the account activation email.";
                }
            }
        }

        Boolean IConfigurableLoginView.IsShibbolethLogin
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

        Boolean IConfigurableLoginView.IsExistingAccount
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

        Boolean IConfigurableLoginView.IsAutoLoginThroughShibboleth { get; set; }

        public String VerificationMessage
        {

            set
            {
                lblVerificationMessage.Text = value.HtmlEncode();
            }
        }

        public Int32 WebSiteId
        {
            get;
            set;
        }

        /// <summary>
        /// List of the Website Pages
        /// </summary>
        public List<WebSiteWebPage> lstWebsitePages
        {
            get;
            set;
        }

        public string FooterHtml
        {
            set { litFooter.Text = String.Empty; }
        }

        public String LoginPageImageUrl
        {
            get;
            set;
        }

        public String UserName
        {
            get
            {
                WclTextBox txtUserName = ucCentrePanel.FindControl("txtUserName") as WclTextBox;
                if (!txtUserName.IsNullOrEmpty())
                {
                    return txtUserName.Text;
                }
                return String.Empty;
            }
            set
            {
                WclTextBox txtUserName = ucCentrePanel.FindControl("txtUserName") as WclTextBox;
                if (!txtUserName.IsNullOrEmpty())
                {
                    txtUserName.Text = value;
                }
            }
        }

        public String EncPValue
        {
            get
            {
                if (!ucCentrePanel.EncPValue.IsNullOrEmpty())
                {
                    return ucCentrePanel.EncPValue.ToString();
                }
                return String.Empty;
            }
        }

        #endregion

        #region Events

        #region PAGE EVENTS

        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //WclTextBox txtUserName = ucCentrePanel.FindControl("txtUserName") as WclTextBox;
                #region Delegate Events

                //ucCentrePanel.eventManageControlsOnSubmitClick += new LoginCenterPanel.ManageControlsOnSubmitClick(ManageControlsOnSubmitClick);
                ucCentrePanel.eventExceptionMessage += new LoginCenterPanel.ExceptionMessage(ExceptionMessage);
                ucCentrePanel.eventShowCentralLoginDiv += new LoginCenterPanel.ShowCentralLoginDiv(ShowCentralLoginDiv);
                ucCentrePanel.eventVerificationMessage += new LoginCenterPanel.HandleVerificationMessage(HandleVerificationMessage);
                ucCentrePanel.eventIsAccountInActive += new LoginCenterPanel.IsAccountInActive(SetIsAccountInActive);
                ucCentrePanel.eventErrorMessageExtended += new LoginCenterPanel.ErrorMessageExtended(ErrorMessageExtended);

                #endregion

                ucCentrePanel.RedirectToMobileSite();
                // get members user
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;

                //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
                ucCentrePanel.AdminAsStudent();
                ucCentrePanel.GetSiteURL();
                ucCentrePanel.ManageUser(user);
                ucCentrePanel.ManageCentralLoginDiv();

                #region UAT-1110 - Profile Sharing And UAT-1218
                ucCentrePanel.ManageProfileSharing();
                #endregion

                if (!IsPostBack)
                {
                    ucCentrePanel.ValidateUser();

                    #region SET LOGIN PAGE IMAGE BASED ON THE CLIENT URL
                    ManageLoginPageImage(sender);
                    #endregion

                    lblInstituteName.Text = AppUtils.GetInstitutionName.HtmlEncode();
                    ucCentrePanel.ManageQueryString();
                }
                //txtUserName.Focus();

                ucCentrePanel.GetErrorMsgInQueryString();

                #region UAT-2792 UCONN SSO Process
                HandleShibbolethProcess();
                #endregion

                //UAT-2894
                ucCentrePanel.SetSessionRequirementShares(user);

                //UAT-2930
                ucCentrePanel.ManageTwoFactorAuthentication(user);

                //UAT 3600
                ucCentrePanel.HandleAutoActivateAndLoginInCBI();

                #region UAT 3054
                ucCentrePanel.AceMappLogin();
                #endregion

                #region [UAT-3177: Hide Create Account button in case of Alumni Tenant]
                ucCentrePanel.HideButtonAlumniTenant();
                #endregion

               
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        #endregion

        #region BUTTON EVENTS

        /// <summary>
        /// Resent Activation Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResendActivationLink_Click(object sender, EventArgs e)
        {
            try
            {
                if (Presenter.ResendVerificationLink())
                {
                    ErrorMessage = String.Empty;
                    CurrentViewContext.IsAccountInActive = false;
                    lblErrorMessageExtended.Text = String.Empty;
                    CurrentViewContext.VerificationMessage = "An activation link has been sent to your email address.";
                }
                else
                {
                    ErrorMessage = "Some error occured while sending the activation link.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// Load the image of the login page based on the client site.
        /// </summary>
        /// <param name="sender">Current page</param>
        private void ManageLoginPageImage(object sender)
        {
            String baseImagePath = WebConfigurationManager.AppSettings[AppConsts.CLIENT_WEBSITE_IMAGES];
            WebSiteId = Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_ID));
            Presenter.GetWebsiteFooter();
            if (WebSiteId > 0)
                GenerateFooter();
            if (String.IsNullOrEmpty(LoginPageImageUrl))
            {
                Presenter.GetImageUrl();
                String imageURL = String.Empty;
                if (WebSiteId > 0)
                    imageURL = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?WebsiteId={0}&DocumentType={1}", WebSiteId, "LoginImage");

                //AD:Commenting following line, now image will be shown in the image control
                //divLogin.Style.Add("background-image", String.Format("url('{0}')", imageURL));

                imgLogo.ImageUrl = imageURL;
            }
        }

        /// <summary>
        /// Method to generate Footer
        /// </summary>
        private void GenerateFooter()
        {
            StringBuilder sbFooter = new StringBuilder();
            Int32 count = 0;
            foreach (var page in lstWebsitePages)
            {
                if (page.LinkPosition == Convert.ToInt32(CustomPageLinkPosition.Footer))
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String _viewType = String.Empty;
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    //{ "Child", @"UserControl/CustomPageContent.ascx"},
                                                                    {"PageId",Convert.ToString( page.WebSiteWebPageID)},
                                                                    {"PageTitle",page.LinkText}

                                                                 };
                    //String url = String.Format("Website/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    String url = String.Format("CustomContentPage.aspx?args={0}", queryString.ToEncryptedQueryString());
                    sbFooter = count == 0 ? sbFooter.Append("&nbsp;&nbsp;") : sbFooter.Append("|&nbsp;&nbsp;");
                    sbFooter.Append("<a href=" + url + ">" + page.LinkText + "</a>&nbsp;&nbsp;");
                    count++;
                }
            }
            //

            litFooter.Text = litFooter.Text + "&nbsp;&nbsp;" + Convert.ToString(sbFooter);
        }

        private void HandleShibbolethProcess()
        {
            if (!ucCentrePanel.IsShibbolethLogin.IsNullOrEmpty())
                CurrentViewContext.IsShibbolethLogin = ucCentrePanel.IsShibbolethLogin;
            if (!ucCentrePanel.IsExistingAccount.IsNullOrEmpty())
                CurrentViewContext.IsExistingAccount = ucCentrePanel.IsExistingAccount;
            if (!ucCentrePanel.IsAutoLoginThroughShibboleth.IsNullOrEmpty())
                CurrentViewContext.IsAutoLoginThroughShibboleth = ucCentrePanel.IsAutoLoginThroughShibboleth;

            if (CurrentViewContext.IsShibbolethLogin && CurrentViewContext.IsExistingAccount)
            {
                dvShibbolethMessage.Style.Add("display", "block");
            }
            if (CurrentViewContext.IsShibbolethLogin)
            {
                dvCreateAccount.Visible = false;
            }
            if (CurrentViewContext.IsAutoLoginThroughShibboleth)
            {
                ucCentrePanel.AutoLogInUsingUserName();
                CurrentViewContext.IsAutoLoginThroughShibboleth = false;
            }
        }

        #region Methods Used in Delegate

        /// <summary>
        /// Show hide the normal registration div and the Central registartion div, alternately
        /// </summary>
        /// <param name="isCentralLoginVisible"></param>
        private void ShowCentralLoginDiv(Boolean isCentralLoginVisible, Boolean hideBothDivs = false)
        {
            if (hideBothDivs)
            {
                divCentralCreateAccount.Visible = false;
                dvCreateAccount.Visible = false;
            }
            else
            {
                divCentralCreateAccount.Visible = isCentralLoginVisible;
                dvCreateAccount.Visible = !isCentralLoginVisible;
            }
        }

        private void ExceptionMessage(String exMessage)
        {
            if (!String.IsNullOrEmpty(exMessage))
            {
                if (exMessage == SysXUtils.GetMessage(ResourceConst.SECURITY_ACCOUNT_LOCKED_CONTACT_ADMINISTRATOR))
                {
                    LanguageContract currentLanguage = (LanguageContract)(SysXWebSiteUtils.SessionService.GetCustomData("LanguageCulture"));
                    if (!currentLanguage.IsNullOrEmpty() && currentLanguage.LanguageCode == Languages.SPANISH.GetStringValue())
                    {
                        // exMessage = Resources.Language.ACCOUNTLOCKED;
                        CultureInfo cultureInfo = new CultureInfo(currentLanguage.LanguageCulture);
                        ResourceManager rm = new ResourceManager("Resources.Language", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                        exMessage = rm.GetString("YOURACCTLOCKEDMSG", cultureInfo);
                    }
                }
                ErrorMessage = exMessage;
            }
        }

        private void HandleVerificationMessage(String verificationMessage)
        {
            if (!String.IsNullOrEmpty(verificationMessage))
                CurrentViewContext.VerificationMessage = verificationMessage;
        }

        private void SetIsAccountInActive(Boolean isAccountInActive)
        {
            if (!isAccountInActive.IsNullOrEmpty())
                CurrentViewContext.IsAccountInActive = isAccountInActive;
        }

        public void ErrorMessageExtended()
        {
            lblErrorMessageExtended.Text = String.Empty;
            lblErrorMessageExtended.Visible = false;
        }

        #endregion

        #endregion

        #endregion
    }
}