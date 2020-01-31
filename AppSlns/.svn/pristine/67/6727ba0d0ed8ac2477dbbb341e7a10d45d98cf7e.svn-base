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
#endregion

#region Application Specific

using INTSOF.Utils;
using INTSOF.Utils.Consts;
using Entity;
using CoreWeb.IntsofSecurityModel;
using System.Collections.Generic;
using CoreWeb.CommonControls.Views;
using INTERSOFT.WEB.UI.Config;

#endregion

#endregion

namespace CoreWeb.Shell.MasterPages
{
    public partial class ChildPage : System.Web.UI.MasterPage, IChildPageView
    {
        #region Variables

        private ChildPagePresenter _presenter=new ChildPagePresenter();

        #endregion

        #region Properites

        
        public ChildPagePresenter Presenter
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



        #endregion

        #region Public Methods

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
        /// Shows the success message.
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

        #endregion

        #region Private Methods


        #endregion

        #region Events

        /// <summary>
        /// Page_Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
        }

        /// <summary>
        /// To set theme
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

                WclTheme currentTheme = (System.Configuration.ConfigurationManager.GetSection(themeSection) as WclThemeSection).Themes[_userPreferenceTheme];
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

        #endregion
    }
}
