#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManagePermission.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using INTERSOFT.WEB.UI.WebControls;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to managing permission in security module.
    /// </summary>
    public partial class ManagePermission : BaseUserControl, IManagePermissionView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ManagePermissionPresenter _presenter=new ManagePermissionPresenter();
        private ManagePermissionContract _viewContract;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Presenter.
        /// </summary>
        /// <value>
        /// Represents Manage Tenant Presenter.
        /// </value>
        
        public ManagePermissionPresenter Presenter
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
        String IManagePermissionView.ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// PermissionTypes.
        /// </summary>
        /// <value>
        /// Gets or sets the list of all permission types.
        /// </value>
        List<PermissionType> IManagePermissionView.PermissionTypes
        {
            get;
            set;
        }

        /// <summary>
        /// CurrentUserID.
        /// </summary>
        /// <value>
        /// Gets or sets the value for current user's id.
        /// </value>
        Int32 IManagePermissionView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// Permissions.
        /// </summary>
        /// <value>
        /// Gets or sets the list of all permissions.
        /// </value>
        IQueryable<Permission> IManagePermissionView.Permissions
        {
            set
            {
                grdPermission.DataSource = value;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <value>
        /// The view contract.
        /// </value>
        ManagePermissionContract IManagePermissionView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ManagePermissionContract();
                }

                return _viewContract;
            }
        }

        /// <summary>
        /// SuccessMessage</summary>
        /// <value>
        /// Gets or sets the value for Success message.</value>
        String IManagePermissionView.SuccessMessage
        {
            get;
            set;
        }

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IManagePermissionView CurrentViewContext
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
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MANAGE_PERMISSIONS);
                lblManagePermission.Text = base.Title;
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
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
        /// <param name="e">     An <see cref="T:System.EventArgs"></see> object that contains the event
        ///  data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }

                Presenter.OnViewLoaded();
                base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_PERMISSIONS));
                lblSuccess.Visible = false;
                lblSuccess.Text = String.Empty;
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

        /// <summary>
        /// Retrieves a list of all permissions.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdPermission_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.RetrievingPermissions();
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
        /// Performs an update operation for permission details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdPermission_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ErrorMessage = (e.Item.FindControl("lblErrorMessage") as Label).Text.Trim();
                CurrentViewContext.ViewContract.PermissionId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PermissionId"]);
                CurrentViewContext.ViewContract.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.PermissionTypeId = Convert.ToInt32((e.Item.FindControl("cmbPermType") as WclComboBox).SelectedValue);

                Presenter.PermissionUpdate();

                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                }
                else
                {
                    e.Canceled = false;
                }

                lblSuccess.Visible = true;
                lblSuccess.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
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
        /// Performs a delete operation for permission details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdPermission_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.PermissionId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PermissionId"]);
                Presenter.DeletePermission();
                lblSuccess.Visible = true;
                lblSuccess.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
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
        /// Performs an insert operation for permission details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdPermission_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ErrorMessage = (e.Item.FindControl("lblErrorMessage") as Label).Text.Trim();
                CurrentViewContext.ViewContract.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.PermissionTypeId = Convert.ToInt32((e.Item.FindControl("cmbPermType") as WclComboBox).SelectedValue);

                Presenter.PermissionSave();

                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                }
                else
                {
                    e.Canceled = false;
                }

                grdPermission.MasterTableView.CurrentPageIndex = grdPermission.PageCount;
                lblSuccess.Visible = true;
                lblSuccess.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
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
        /// Event handler. Called by grdPermission for item created events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdPermission_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (GridEditFormItem)e.Item;
                    WclComboBox ddlPermissionType = (WclComboBox)editform.FindControl("cmbPermType");

                    Presenter.RetrievingPermissionTypes();

                    if (!CurrentViewContext.PermissionTypes.IsNull())
                    {
                        PermissionType permissionType = new PermissionType
                        {
                            Name = AppConsts.COMBOBOX_ITEM_SELECT,
                            PermissionTypeID = AppConsts.NONE
                        };

                        CurrentViewContext.PermissionTypes.Insert(AppConsts.NONE, permissionType);
                        ddlPermissionType.DataSource = CurrentViewContext.PermissionTypes;
                        ddlPermissionType.DataBind();

                        if (!(e.Item.DataItem is GridInsertionObject))
                        {
                            var permission = (e.Item).DataItem as Permission;

                            if (!permission.IsNull())
                            {
                                ddlPermissionType.SelectedValue = Convert.ToString(permission.PermissionTypeId);
                            }
                        }
                    }
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

        /// <summary>
        /// To allow edit or insert mode.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdPermission_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    grdPermission.MasterTableView.IsItemInserted = false;
                }
                else
                {
                    grdPermission.MasterTableView.ClearChildEditItems();
                }
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName)
                {
                    foreach (GridFilteringItem filterItem in grdPermission.MasterTableView.GetItems(GridItemType.FilteringItem))
                    {
                        filterItem.Visible = false;
                    }
                    grdPermission.ExportSettings.ExportOnlyData = true;
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

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}