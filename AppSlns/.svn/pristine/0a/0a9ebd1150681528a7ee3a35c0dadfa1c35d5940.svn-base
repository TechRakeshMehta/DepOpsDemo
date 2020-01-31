#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:   ToolBar.ascx.cs
// Purpose:   
//
// Revisions:
// Author       Date            Comment
// ------       ----------      -------------------------------------------------
// Ashish Daniel    12-Dec-2012 0930   - File updated / cleaned up
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.IO;
using Microsoft.Practices.ObjectBuilder;
using System.Linq;
using System.Collections.Generic;
using Telerik.Web.UI;
using System.Data;
using System.Web;

#endregion

#region Application Specific

using INTSOF.Utils;
using CoreWeb.Shell;
using Business.RepoManagers;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using Entity;
using INTERSOFT.WEB.UI.WebControls;


#endregion

#endregion

namespace CoreWeb.Shell.Views
{
    public partial class ToolBar : BaseUserControl, IToolBarView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ToolBarPresenter _presenter = new ToolBarPresenter();
        private ISysXSessionService _sessionService = SysXWebSiteUtils.SessionService;
        private List<String> _themesName = new List<String>();
        private String _helpFilePath = String.Empty;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public aspnet_Membership aspnet_Membership
        {
            get;
            set;
        }

        /// <summary>
        /// Represents Presenter
        /// </summary>
        
        public ToolBarPresenter Presenter
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

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #region Events Declaration

        #endregion

        #region Page Events

        /// <summary>
        /// Override this method and set the Title
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.RemoveUnusedSession = false;
                base.OnInit(e);
                if (!this.IsPostBack)
                {
                    foreach (RadToolBarItem searchBar in tlbMain.Items)
                    {
                        if (searchBar is RadToolBarSplitButton)
                        {
                            var button = searchBar as RadToolBarSplitButton;

                        }
                        else if (searchBar is RadToolBarButton)
                        {
                            var button = searchBar as RadToolBarButton;
                            if (button.CommandName.Equals("SearchCriteria", StringComparison.OrdinalIgnoreCase))
                            {
                                // Set tooltip for first time visible
                                ViewState.Remove(SysXSearchConsts.SELECTED_QUICK_SEARCH_OPTION);
                            }
                            else if (button.CommandName.Equals("LineOfBusiness", StringComparison.OrdinalIgnoreCase))
                            {
                                // Bind Line of business of current user
                                var comboBox = button.FindControl("cmbLineOfBusiness") as WclComboBox;
                                if (!_sessionService.UserId.IsNullOrEmpty())
                                {
                                    comboBox.DataSource = this.Presenter.GetLineOfBusinessesByUser(_sessionService.UserId);
                                    comboBox.DataBind();
                                    if (!SysXWebSiteUtils.SessionService.SysXBlockId.IsNullOrEmpty())
                                    {
                                        comboBox.SelectedValue = SysXWebSiteUtils.SessionService.SysXBlockId.ToString();
                                    }
                                    else
                                    {
                                        comboBox.SelectedValue = this.Presenter.GetDefaultLineOfBusinessOfLoggedInUser(_sessionService.OrganizationUserId);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(base.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(base.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Handles Page load event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.Presenter.OnViewInitialized();
                }
                this.Presenter.OnViewLoaded();
                _helpFilePath = Presenter.GetSysXConfigValue("HelpFileGetPath");
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", _helpFilePath));
            }
            catch (SysXException ex)
            {
                base.LogError(base.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(base.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Control Related Events

        protected void tblMain_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            // TODO: Comment and optimization.
            RadToolBarButton btnClicked = e.Item as RadToolBarButton;
        }


        /// <summary>
        /// Handles the Change event of the cmbLineOfBusiness control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">     The <see cref="RadComboBoxSelectedIndexChangedEventArgs"/> instance
        ///  containing the event data.</param>
        protected void cmbLineOfBusiness_Change(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            // Refresh the main menu of MasterPage
            Response.Redirect(AppConsts.SYSX_DASHBOARD, false);
            SysXWebSiteUtils.SessionService.SetSysXBlockId(Convert.ToInt32(e.Value));
            SysXWebSiteUtils.SessionService.SetSysXBlockName(e.Text);
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods


        #endregion

        #endregion
    }
}