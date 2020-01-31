#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageDepartment.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Telerik.Web.UI;
using CoreWeb.Shell;
using INTSOF.UI.Contract.IntsofSecurityModel;
using INTERSOFT.WEB.UI.WebControls;



#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to managing departments in security module.
    /// </summary>
    public partial class ManageDepartment : BaseUserControl, IManageDepartmentView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ManageDepartmentPresenter _presenter=new ManageDepartmentPresenter();
        private ManageDepartmentContract _viewContract;
        private String _viewType;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Presenter.
        /// </summary>
        /// <value>
        /// Represents Manage Tenant Presenter.
        /// </value>
        
        public ManageDepartmentPresenter Presenter
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
        /// Organization Name.
        /// </summary>
        /// <value>
        /// Gets or sets the value for parent organization's id.
        /// </value>
        Int32 IManageDepartmentView.ParentOrganizationId
        {
            get
            {
                return Convert.ToInt32(ViewState["OrganizationID"]);
            }
            set
            {
                ViewState["OrganizationID"] = value;
            }
        }

        /// <summary>
        /// TenantID.
        /// </summary>
        /// <value>
        /// Gets or sets the value for tenant's id.
        /// </value>
        Int32 IManageDepartmentView.TenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["TenantID"]);
            }
            set
            {
                ViewState["TenantID"] = value;
            }
        }

        /// <summary>
        /// ProductID.
        /// </summary>
        /// <value>
        /// Gets  the value for product's id.
        /// </value>
        Int32 IManageDepartmentView.ProductId
        {
            get
            {
                return Convert.ToInt32(base.SysXMembershipUser.ProductId);
            }
        }

        /// <summary>
        /// Current User Id.
        /// </summary>
        /// <value>
        /// The identifier of the current user.
        /// </value>
        Int32 IManageDepartmentView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// OrganizationDepartments.
        /// </summary>
        /// <value>
        /// Gets or sets the list of all departments.
        /// </value>
        IQueryable<Organization> IManageDepartmentView.OrganizationDepartments
        {
            set
            {
                grdDepartment.DataSource = value;
            }
        }

        /// <summary>
        /// ErrorMessage.
        /// </summary>
        /// <value>
        /// Gets or sets the value for error message.
        /// </value>
        String IManageDepartmentView.ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// IsAdmin.
        /// </summary>
        /// <value>
        /// Gets the value for IsAdmin.
        /// </value>
        Boolean IManageDepartmentView.IsAdmin
        {
            get
            {
                return base.IsSysXAdmin;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <value>
        /// The view contract.
        /// </value>
        ManageDepartmentContract IManageDepartmentView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ManageDepartmentContract();
                }

                return _viewContract;
            }
        }

        /// <summary>
        /// SuccessMessage</summary>
        /// <value>
        /// Gets or sets the value for Success message.</value>
        String IManageDepartmentView.SuccessMessage
        {
            get;
            set;
        }

        /// <summary>
        /// List of Tenant
        /// </summary>
        public List<Entity.ClientEntity.Tenant> TenantList
        {
            get;
            set;
        }

        /// <summary>
        /// Selected ClientId
        /// </summary>
        public Int32 SelectedClientId
        {
            get;
            set;
        }

        /// <summary>
        /// SelectedTenantId
        /// </summary>
        public Int32 SelectedTenantId
        {
            get { return Convert.ToInt32(base.SysXMembershipUser.TenantId); }

        }
        #region Private Properties

        /// <summary>
        /// Gets a context for the current view.
        /// </summary>
        /// <value>
        /// The current view context.
        /// </value>
        IManageDepartmentView CurrentViewContext
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

        ///<summary>
        ///Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// Calls WebClientApplication.BuildItemWithCurrentContext after the events are handled
        /// to fire the DI engine.
        ///</summary>
        ///<param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MANAGE_DEPARTMENTS);
                lblManageDepartment.Text = base.Title;
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
                base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_DEPARTMENTS));
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

        #region Grid Events

        /// <summary>
        /// Performs an insert operation for department details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdDepartment_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {

                CurrentViewContext.ViewContract.OrganizationName = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.OrganizationDesc = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                if (CurrentViewContext.TenantId.Equals(Convert.ToInt32(Presenter.GetSysXConfigValue("DefaultTenantID"))))
                {
                    CurrentViewContext.TenantId = Convert.ToInt32((e.Item.FindControl("cmbTenant") as WclComboBox).SelectedValue);
                }
                CurrentViewContext.ViewContract.CreatedOn = DateTime.Now;
                CurrentViewContext.ViewContract.Active = true;
                CurrentViewContext.ViewContract.Deleted = false;
                Presenter.DepartmentSave();

                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                }
                else
                {
                    e.Canceled = false;
                }

                grdDepartment.MasterTableView.CurrentPageIndex = grdDepartment.PageCount;
                base.RefreshMenu();

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
        /// Retrieves a list of all department details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdDepartment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                CurrentViewContext.ViewContract.OrganizationId = user.OrganizationId;
                Presenter.RetrievingDepartment();
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
        /// Performs an update operation for  department details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdDepartment_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.OrganizationId = Convert.ToInt32((e.Item as GridEditableItem).GetDataKeyValue("OrganizationID"));
                CurrentViewContext.ViewContract.OrganizationName = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.OrganizationDesc = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                if (CurrentViewContext.TenantId.Equals(Convert.ToInt32(Presenter.GetSysXConfigValue("DefaultTenantID"))))
                {
                    CurrentViewContext.TenantId = Convert.ToInt32((e.Item.FindControl("cmbTenant") as WclComboBox).SelectedValue);
                }
                Presenter.UpdateDepartment();

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
        /// Performs a delete operation for department details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdDepartment_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.ViewContract.OrganizationId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("OrganizationID"));
                CurrentViewContext.ViewContract.Deleted = false;
                Presenter.DeleteDepartment();
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
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdDepartment_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    grdDepartment.MasterTableView.IsItemInserted = false;
                }
                else
                {
                    grdDepartment.MasterTableView.ClearChildEditItems();
                }
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdDepartment);

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
        /// No Metadata Documentation available.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdDepartment_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    Organization organization = (Organization)e.Item.DataItem;
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    if (!base.SysXMembershipUser.IsNull())
                    {
                        if (base.SysXMembershipUser.TenantId.Equals(Convert.ToInt32(Presenter.GetSysXConfigValue("DefaultTenantID"))))
                        {
                            (e.Item.OwnerTableView.Columns.FindByUniqueName("ManageUsersOnMgDept")).Visible = false;
                        }
                        else
                        {
                            HtmlAnchor ancManageUsers = (HtmlAnchor)e.Item.FindControl("ancManageUsers");
                            queryString = new Dictionary<String, String>
                                                                {
                                                                    {"AssignToDepartmentId", Convert.ToString(organization.OrganizationID)},
                                                                    {"Child", ChildControls.SecurityManageUser}
                                                                };

                            ancManageUsers.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                        }
                    }


                    if (!organization.IsNull())
                    {
                        CurrentViewContext.ViewContract.CreatedById = Convert.ToInt32(organization.CreatedByID);
                        queryString.Clear();
                        //HtmlAnchor ancManagePrograms = (HtmlAnchor)e.Item.FindControl("ancManagePrograms");
                        //queryString = new Dictionary<String, String>
                        //                                        {
                        //                                            {"OrganizationId", Convert.ToString(organization.OrganizationID)},
                        //                                            {"Child", ChildControls.SecurityManageProgram}
                        //                                        };

                        //ancManagePrograms.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                        //queryString.Clear();
                        HtmlAnchor ancManageGrades = (HtmlAnchor)e.Item.FindControl("ancManageGrades");
                        queryString = new Dictionary<String, String>
                                                                {
                                                                    {"OrganizationId", Convert.ToString(organization.OrganizationID)},
                                                                    {"TenantId",organization.TenantID.ToString()},
                                                                    {"Child", ChildControls.SecurityManageGrade}
                                                                };

                        ancManageGrades.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                        queryString.Clear();
                        HtmlAnchor ancSetupProgramSubscription = (HtmlAnchor)e.Item.FindControl("ancSetupProgramSubscription");
                        queryString = new Dictionary<String, String>
                                                                {
                                                                    {"DepartmentId", Convert.ToString(organization.OrganizationID)},
                                                                    {"TenantId",organization.TenantID.ToString()}
                                                                };

                        ancSetupProgramSubscription.HRef = String.Format(ChildControls.SetupDepartmentProgram + "?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    }
                    if (e.Item is GridDataItem)
                    {
                        GridDataItem dataItem = e.Item as GridDataItem;
                        //if (Presenter.IsDepartmentMapped(organization.OrganizationID))
                        //{
                        //    ImageButton deleteColumn = dataItem["DeleteColumn"].Controls[0] as ImageButton;
                        //    deleteColumn.Visible = false;
                        //}

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
        /// Event handler. Called by grdDepartment for item created events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdDepartment_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (GridEditFormItem)e.Item;
                    WclComboBox ddlTenant = (WclComboBox)editform.FindControl("cmbTenant");
                    HtmlControl divClient = (HtmlControl)editform.FindControl("divClient");
                    RequiredFieldValidator rfvTenant = (RequiredFieldValidator)editform.FindControl("rfvTenant");
                    if (!base.SysXMembershipUser.IsNull())
                    {
                        if (base.SysXMembershipUser.TenantId.Equals(Convert.ToInt32(Presenter.GetSysXConfigValue("DefaultTenantID"))))
                        {
                            Presenter.GetTenantList();
                            if (!CurrentViewContext.TenantList.IsNull())
                            {
                                ddlTenant.DataSource = CurrentViewContext.TenantList;
                                ddlTenant.DataBind();

                                if (!(e.Item is GridEditFormInsertItem))
                                {
                                    Organization organization = (Organization)e.Item.DataItem;

                                    if (!organization.IsNull())
                                    {
                                        ddlTenant.SelectedValue = Convert.ToString(organization.TenantID);
                                        ddlTenant.Enabled = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            divClient.Style["display"] = "none";
                            ddlTenant.Visible = false;
                            rfvTenant.ValidationGroup = String.Empty;
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