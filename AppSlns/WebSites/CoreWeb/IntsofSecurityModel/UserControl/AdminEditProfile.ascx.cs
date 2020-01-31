#region Header Comment Block
// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  AdminEditProfile.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined Namespaces

using System;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Collections;
using System.Text.RegularExpressions;

#endregion

#region User Defined Namespaces

using INTSOF.Utils;
using Telerik.Web.UI;
using CoreWeb.Shell;
using Entity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTERSOFT.WEB.UI.WebControls;
using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    public partial class AdminEditProfile : BaseUserControl, IAdminEditProfileView
    {
        #region Variables

        #region Private Variables

        private AdminEditProfilePresenter _presenter = new AdminEditProfilePresenter();
        private String _reg = @"^[\w\d\s\-\.\@\+]{1,50}$";
        private String _viewType;
        #endregion



        #endregion

        #region Properties

        #region Public Properties


        public AdminEditProfilePresenter Presenter
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

        public IAdminEditProfileView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public Boolean IsAdbAdmin
        {
            get
            {
                if (!base.SysXMembershipUser.IsNullOrEmpty() && base.SysXMembershipUser.OrganizationId == AppConsts.ONE)
                    return true;
                return false;
            }
        }

        public String UserName
        {
            get
            {
                return txtUsername.Text.Trim();
            }
            set
            {
                txtUsername.Text = value;
            }
        }

        public String FirstName
        {
            get
            {
                return txtFirstName.Text.Trim();
            }
            set
            {
                txtFirstName.Text = value;
            }
        }

        public String LastName
        {
            get
            {
                return txtLastName.Text.Trim();
            }
            set
            {
                txtLastName.Text = value;
            }
        }

        public String PrimaryEmail
        {
            get
            {
                return txtPrimaryEmail.Text.Trim();
            }
            set
            {
                txtPrimaryEmail.Text = value;
            }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// OnInit event to set title
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.SetPageTitle("My Profile");
                base.Title = "Profile";
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
        /// Page_Load event loads data on page loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                Presenter.GetOrgUserData();
            }
            Presenter.OnViewLoaded();

            cBarMain.SaveButton.ToolTip = "Click to save any updates made to your profile";
            cBarMain.CancelButton.ToolTip = "Click to cancel. Any updates made to your profile will not be saved";

            //UAT-712
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            lnkChangePassword.Enabled = true;
            queryString = RedirectToChangePassword(queryString);

            //UAT-2930
            ShowhideTwoFactorAuthentication();

            #region UAT-4151
            if (CurrentViewContext.IsAdbAdmin)
            {
                cBarMain.ClearButton.Style.Add("display", "none");
            }
            else
            {
                cBarMain.ClearButton.Style.Add("display", "inline-block");
            }
            #endregion


        }

        /// <summary>
        /// Used for open the change password page.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        private Dictionary<string, string> RedirectToChangePassword(Dictionary<String, String> queryString)
        {
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", @"~/IntsofSecurityModel/ChangePassword.ascx"},
                                                                    //{"PageType","ChangePassword"}
                                                                 };
            _viewType = "ClientAdmin";
            lnkChangePassword.NavigateUrl = String.Format("~/IntsofSecurityModel/default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

            //Adding tooltip for the user profile link only if link is enabled
            if (!string.IsNullOrWhiteSpace(lnkChangePassword.NavigateUrl))
            {
                lnkChangePassword.ToolTip = "Click here to change password";
            }

            //Setting iframe as a target 
            lnkChangePassword.Target = "pageFrame";
            return queryString;
        }

        /// <summary>
        /// On click of Update button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar1_SaveClick(object sender, EventArgs e)
        {
            lblUserNameMessage.Text = String.Empty;

            //Username check
            if (String.IsNullOrEmpty(CurrentViewContext.UserName.Trim()))
            {
                lblMessage.Visible = true;
                base.ShowErrorInfoMessage("Username is required.");
                return;
            }
            if (Regex.IsMatch(CurrentViewContext.UserName, _reg) == false)
            {
                lblMessage.Visible = true;
                base.ShowErrorInfoMessage("Please enter valid character(s) in Username.");
                return;
            }
            if (Presenter.IsExistingUser())
            {
                lblMessage.Visible = true;
                base.ShowErrorInfoMessage("This username is already in use. Try another?"); ;
                return;
            }
            if (Presenter.SaveUserData())
            {
                lblMessage.Visible = true;
                base.ShowSuccessMessage("Data updated successfully.");
            }
            else
            {
                base.ShowInfoMessage("Data is not saved.");
            }
        }

        /// <summary>
        /// On click of cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar1_CancelClick(object sender, EventArgs e)
        {
            try
            {
                RedirectToDashboard();
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

        protected void cBarMain_ClearClick(object sender, EventArgs e)
        {
            try
            {
                _viewType = "ClientAdmin";
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",ChildControls.OtherAccountLinking}
                                                                 };
                string url = String.Format("~/CommonOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
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
        /// To Validate Username
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCheckUsername_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(CurrentViewContext.UserName.Trim()))
            {
                lblUserNameMessage.Text = "Username is required.";
                lblUserNameMessage.ForeColor = System.Drawing.Color.Red;
            }
            else if (Regex.IsMatch(CurrentViewContext.UserName, _reg) == false)
            {
                lblUserNameMessage.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_INVALID_CHARACTER);
                lblUserNameMessage.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                if (Presenter.IsExistingUser())
                {
                    lblUserNameMessage.Text = "This Username is not available. Try another?";
                    lblUserNameMessage.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblUserNameMessage.Text = "This Username is available.";
                    lblUserNameMessage.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

        #endregion

        #region Methods

        #region Private Methods

        private void RedirectToDashboard()
        {
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
        }

        //UAT-2930
        private void ShowhideTwoFactorAuthentication()
        {
            String userId = SysXWebSiteUtils.SessionService.UserId;
            if (!userId.IsNullOrEmpty() && Presenter.ShowhideTwoFactorAuthentication(userId))
            {
                dvTwoFactorAuthentication.Style["display"] = "block";
            }
            else
            {
                dvTwoFactorAuthentication.Style["display"] = "none";
            }
        }

        #endregion



        #endregion
    }
}

