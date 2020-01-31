#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageConfiguration.ascx..cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;

#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using INTERSOFT.WEB.UI.WebControls;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to managing configurations in security module.
    /// </summary>
    public partial class ManageConfiguration : BaseUserControl, IManageConfigurationView
    {
        #region Variables

        #region Private Variables

        private ManageConfigurationPresenter _presenter=new ManageConfigurationPresenter();
        private ManageConfigurationContract _viewContract;

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value for Presenter.
        /// </summary>
        /// <value>
        /// The presenter.
        /// </value>
        
        public ManageConfigurationPresenter Presenter
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
        /// Sets the value for database configurations.
        /// </summary>
        /// <value>
        /// The database configurations.
        /// </value>
        IQueryable<SysXConfig> IManageConfigurationView.DbConfigurations
        {
            set
            {
                grdDBConfigDetail.DataSource = value;
            }
        }

        /// <summary>
        /// Sets the value for application configurations.
        /// </summary>
        /// <value>
        /// The application configurations.
        /// </value>
        Dictionary<String, String> IManageConfigurationView.AppConfigurations
        {
            set
            {
                grdAppConfigDetail.DataSource = value;
            }
        }

        /// <summary>
        /// Gets the value for full name of web configuration.
        /// </summary>
        /// <value>
        /// The name of the web configuration full.
        /// </value>
        String IManageConfigurationView.WebConfigurationFullName
        {
            get
            {
                return Server.MapPath("~") + "\\Web.Config";
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <value>
        /// The view contract.
        /// </value>
        ManageConfigurationContract IManageConfigurationView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ManageConfigurationContract();
                }

                return _viewContract;
            }
        }

        /// <summary>
        /// ErrorMessage.
        /// </summary>
        /// <value>
        /// Gets or sets the value for error message.
        /// </value>
        String IManageConfigurationView.ErrorMessage
        {
            get;
            set;
        }

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <value>
        /// The current view context.
        /// </value>
        IManageConfigurationView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        ///  Calls WebClientApplication.BuildItemWithCurrentContext after the events are handled to fire
        ///  the DI engine.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MANAGE_WEB_CONFIGURATION);
                lblManageWebConfiguration.Text = base.Title;
                lblManageDBConfiguration.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_MANAGE_DATABASE_CONFIGURATION);
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
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }

                Presenter.OnViewLoaded();
                base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_APP_SLASH_DB_CONFIGURATIONS));
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

        #endregion

        #region Grid Related Events

        #region Application Configuration Detail Events

        /// <summary>
        /// Retrieves a list of all Applications Configuration Details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdAppConfigDetail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.RetrievingAppConfigurationSettings();
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
        /// Performs an update operation for Applications Configuration Details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdAppConfigDetail_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.Value = (e.Item.FindControl("txtAppValue") as WclTextBox).Text;
                CurrentViewContext.ViewContract.Key = (e.Item.FindControl("txtAppKeyName") as WclTextBox).Text;
                Presenter.UpdateAppConfiguration();
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

        protected void grdAppConfigDetail_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName)
                {
                    foreach (GridFilteringItem filterItem in grdAppConfigDetail.MasterTableView.GetItems(GridItemType.FilteringItem))
                    {
                        filterItem.Visible = false;
                    }
                    grdAppConfigDetail.ExportSettings.ExportOnlyData = true;
                }
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

        #endregion

        #region DataBase Configuration Detail Events

        /// <summary>
        /// Performs an insert operation for department details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdDBConfigDetail_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.Value = (e.Item.FindControl("txtDBValue") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.Key = (e.Item.FindControl("txtDBKeyName") as WclTextBox).Text.Trim();
                Presenter.SaveDBConfiguration();

                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                }
                else
                {
                    e.Canceled = false;
                }

                grdDBConfigDetail.MasterTableView.CurrentPageIndex = grdDBConfigDetail.PageCount;
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
        /// Retrieves a list of all Database Configuration Details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdDBConfigDetail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.RetrievingDbConfigurationSettings();
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
        /// Performs an update operation for Database Configuration Details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdDBConfigDetail_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.Value = (e.Item.FindControl("txtDBValue") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.Key = (e.Item.FindControl("txtDBKeyName") as WclTextBox).Text.Trim();
                Presenter.UpdateDbConfiguration();

                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                }
                else
                {
                    e.Canceled = false;
                }
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

        protected void grdDBConfigDetail_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName)
                {
                    foreach (GridFilteringItem filterItem in grdDBConfigDetail.MasterTableView.GetItems(GridItemType.FilteringItem))
                    {
                        filterItem.Visible = false;
                    }
                    grdDBConfigDetail.ExportSettings.ExportOnlyData = true;
                }
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

        #endregion

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