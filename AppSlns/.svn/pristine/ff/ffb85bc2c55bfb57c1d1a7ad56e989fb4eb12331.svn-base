#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageBlock.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to managing blocks in security module.
    /// </summary>
    public partial class ManageLineOfBusiness : BaseUserControl, IManageBlockView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ManageBlockPresenter _presenter = new ManageBlockPresenter();
        private String _viewType;
        private ManageBlockContract _viewContract;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Presenter.
        /// </summary>
        /// <value>
        /// Represents Manage Tenant Presenter.
        /// </value>

        public ManageBlockPresenter Presenter
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
        String IManageBlockView.ErrorMessage
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
        Int32 IManageBlockView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// Blocks</summary>
        /// <value>
        /// Gets or sets all blocks.</value>
        IEnumerable<lkpSysXBlock> IManageBlockView.Blocks
        {
            set
            {
                grdBlock.DataSource = value;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <value>
        /// The view contract.
        /// </value>
        ManageBlockContract IManageBlockView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ManageBlockContract();
                }

                return _viewContract;
            }
        }

        /// <summary>
        /// SuccessMessage</summary>
        /// <value>
        /// Gets or sets the value for Success message.</value>
        String IManageBlockView.SuccessMessage
        {
            get;
            set;
        }

        #region Private Properties

        IManageBlockView CurrentViewContext
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
        /// Override this method and set IsPolicyEnable = false to disable policy settings. - TG.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MANAGE_LINE_OF_BUSINESSES);
                lblManageLineOfBusiness.Text = base.Title;
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
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
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }

                Presenter.OnViewLoaded();
                base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_LINE_OF_BUSINESS));
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
        /// Retrieves a list of all Line Of Businesses.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdLineOfBusinesses_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.RetrievingLineOfBusinesses();
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
        /// Performs an update operation for Line Of Business.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdgrdLineOfBusinesses_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ErrorMessage = (e.Item.FindControl("lblErrorMessage") as Label).Text.Trim();
                CurrentViewContext.ViewContract.SysXBlockId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SysXBlockId"]);
                CurrentViewContext.ViewContract.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.Code = (e.Item.FindControl("txtCode") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.BusinessChannelTypeID = Convert.ToInt16((e.Item.FindControl("cmbBusinessChannelType") as WclComboBox).SelectedValue);

                Presenter.UpdateLineOfBusinesses();

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
        /// Performs a delete operation for Line Of Business.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdLineOfBusinesses_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.SysXBlockId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SysXBlockId"]);
                Presenter.DeleteLineOfBusinesses();
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
        /// Performs an insert operation for Line Of Business.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdLineOfBusinesses_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ErrorMessage = (e.Item.FindControl("lblErrorMessage") as Label).Text.Trim();
                CurrentViewContext.ViewContract.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.Code = (e.Item.FindControl("txtCode") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.BusinessChannelTypeID = Convert.ToInt16((e.Item.FindControl("cmbBusinessChannelType") as WclComboBox).SelectedValue);

                Presenter.AddLineOfBusinesses();

                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                }
                else
                {
                    e.Canceled = false;
                }

                grdBlock.MasterTableView.CurrentPageIndex = grdBlock.PageCount;
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
        /// Event handler. Called by grdLineOfBusinesses for item data bound events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdLineOfBusinesses_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    var block = (lkpSysXBlock)e.Item.DataItem;
                    var anchor = (HtmlAnchor)e.Item.FindControl("ancFeature");
                    var queryString = new Dictionary<String, String>
                                          {
                                              {"SysXBlockID",Convert.ToString(block.SysXBlockId)},
                                              {"Child", ChildControls.SecurityMapBlockFeature},
                                              {"BusinessChannelTypeID",block.BusinessChannelTypeID.ToString()}
                                          };
                    anchor.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                    if (block.SysXBlockId.Equals(AppConsts.ONE))
                    {
                        (e.Item as GridEditableItem)["DeleteColumn"].Controls[0].Visible = false;
                        (e.Item as GridEditableItem)["DeleteColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                        (e.Item as GridEditableItem)["EditCommandColumn"].Controls[0].Visible = false;
                        (e.Item as GridEditableItem)["EditCommandColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                        (e.Item as GridEditableItem)["ManageFeatures"].Controls[0].Visible = false;
                        (e.Item as GridEditableItem)["ManageFeatures"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
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
        /// Event handler. Called by grdgrdLineOfBusinesses for item command events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdgrdLineOfBusinesses_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    grdBlock.MasterTableView.IsItemInserted = false;
                }
                else
                {
                    grdBlock.MasterTableView.ClearChildEditItems();
                }
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdBlock);

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

        protected void cvDuplicateCode_ServerValidate(object sender, ServerValidateEventArgs e)
        {

            GridEditFormItem editItem = ((CustomValidator)sender).Parent.NamingContainer as GridEditFormItem;
            if (editItem.DataSetIndex > -1)
            {
                e.IsValid = !Presenter.IsLOBCodeExist(e.Value, ((String)editItem.GetDataKeyValue("Code")));
            }
            else
            {
                e.IsValid = !Presenter.IsLOBCodeExist(e.Value, String.Empty);
            }
        }


        #endregion

        protected void grdLineOfBusinesses_ItemCreated(object sender, GridItemEventArgs e)
        {
            if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
            {
                WclComboBox cmbBusinessChannelType = e.Item.FindControl("cmbBusinessChannelType") as WclComboBox;
                cmbBusinessChannelType.DataSource = Presenter.GetBusinessChannelTypes();
                cmbBusinessChannelType.DataBind();

                if (!(e.Item is GridEditFormInsertItem))
                {
                    lkpSysXBlock sysXBlock = e.Item.DataItem as lkpSysXBlock;
                    if (sysXBlock.IsNotNull())
                    {
                        cmbBusinessChannelType.SelectedValue = sysXBlock.BusinessChannelTypeID.ToString();
                    }
                }
            }
        }

        #endregion

    }
}