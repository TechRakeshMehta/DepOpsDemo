#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManagePermissionType.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to managing permission types in security module.
    /// </summary>
    /// <remarks></remarks>
    public partial class ManagePermissionType : BaseUserControl, IManagePermissionTypeView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ManagePermissionTypePresenter _presenter=new ManagePermissionTypePresenter();

        private ManagePermissionTypeContract _viewContract;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Presenter.
        /// </summary>
        /// <value>
        /// Gets or sets the values for Manage Tenant Presenter.
        /// </value>
        
        public ManagePermissionTypePresenter Presenter
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
        /// PermissionTypes.
        /// </summary>
        /// <value>
        /// Gets or sets the list of all permission types.
        /// </value>
        IQueryable<PermissionType> IManagePermissionTypeView.PermissionTypes
        {
            set
            {
                grdPermissionType.DataSource = value;
            }
        }

        /// <summary>
        /// CurrentUserID.
        /// </summary>
        /// <value>
        /// Gets the value for current user's id.
        /// </value>
        Int32 IManagePermissionTypeView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <value>
        /// The view contract.
        /// </value>
        ManagePermissionTypeContract IManagePermissionTypeView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ManagePermissionTypeContract();
                }

                return _viewContract;
            }
        }

        /// <summary>
        /// SuccessMessage</summary>
        /// <value>
        /// Gets or sets the value for Success message.</value>
        String IManagePermissionTypeView.SuccessMessage
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
        IManagePermissionTypeView CurrentViewContext
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
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event. Calls
        /// WebClientApplication.BuildItemWithCurrentContext after the events are handled to fire the DI
        /// engine.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MANAGE_PERMISSION_TYPES);
                lblManagePermissionType.Text = base.Title;
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
                base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_PERMISSION_TYPES));
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
        /// <param name="source">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdPermissionType_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            try
            {
                var grid = ((RadGrid)source);
                var eventArgs = new RetrievingEntitiesEventArgs
                {
                    MaxRows = grid.PageSize,
                    StartIndex = grid.CurrentPageIndex * grid.PageSize
                };

                OnRetrievingPermissionTypes(eventArgs);
                grid.VirtualItemCount = eventArgs.TotalRowCount;
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
        /// Performs a delete operation for permission type.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdPermissionType_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.PermissionTypeId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PermissionTypeId"]);
                Presenter.DeletePermissionType();
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
        /// Performs an update operation for permission type.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdPermissionType_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                Int32 permissionTypeId = Convert.ToInt32(editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["PermissionTypeId"]);

                IPermissionTypeEditView ucPermissionTypeEdit = (IPermissionTypeEditView)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);
                ucPermissionTypeEdit.PermissionTypeId = permissionTypeId;
                ucPermissionTypeEdit.UpdatePermissionType();

                if (!String.IsNullOrEmpty(ucPermissionTypeEdit.ErrorMessage))
                {
                    e.Canceled = true;
                    throw new System.Exception(ucPermissionTypeEdit.ErrorMessage);
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
        /// Performs an insert operation for permission type.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdPermissionType_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                IPermissionTypeEditView ucPermissionTypeEdit = (IPermissionTypeEditView)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);
                ucPermissionTypeEdit.SavePermissionType();

                if (!String.IsNullOrEmpty(ucPermissionTypeEdit.ErrorMessage))
                {
                    e.Canceled = true;
                    throw new System.Exception(ucPermissionTypeEdit.ErrorMessage);
                }

                e.Canceled = false;

                grdPermissionType.MasterTableView.CurrentPageIndex = grdPermissionType.PageCount;
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
        /// To allow edit or insert mode.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdPermissionType_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    grdPermissionType.MasterTableView.IsItemInserted = false;
                }
                else
                {
                    grdPermissionType.MasterTableView.ClearChildEditItems();
                }
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName)
                {
                    foreach (GridFilteringItem filterItem in grdPermissionType.MasterTableView.GetItems(GridItemType.FilteringItem))
                    {
                        filterItem.Visible = false;
                    }
                    grdPermissionType.ExportSettings.ExportOnlyData = true;
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

        private void OnRetrievingPermissionTypes(RetrievingEntitiesEventArgs eventArgs)
        {
            try
            {
                Presenter.RetrievingPermissionTypes();
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
    }
}