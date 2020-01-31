#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageRole.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using System.Web.UI.WebControls;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;


#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to managing roles in security module.
    /// </summary>
    public partial class ManageRole : BaseUserControl, IManageRoleView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ManageRolePresenter _presenter = new ManageRolePresenter();

        private String _viewType;

        private ManageRoleContract _viewContract;

        #endregion

        #endregion

        #region Properties

        #region ClientOnBoardingWizard

        /// <summary>
        /// Gets or sets a value indicating whether this instance is data load.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is data load; otherwise, <c>false</c>.
        /// </value>
        Boolean IManageRoleView.IsDataLoad
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is client on boarding wizard.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is client on boarding wizard; otherwise, <c>false</c>.
        /// </value>
        Boolean IManageRoleView.IsClientOnBoardingWizard
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is page load by client on boarding wizard.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is page load by client on boarding wizard; otherwise, <c>false</c>.
        /// </value>
        Boolean IsPageLoadByClientOnBoardingWizard
        {
            get
            {
                return Convert.ToBoolean(!SysXWebSiteUtils.SessionService.GetCustomData(SysXUtils.GetMessage(ResourceConst.IS_PAGE_MANAGE_ROLE)).IsNull() ? SysXWebSiteUtils.SessionService.GetCustomData(SysXUtils.GetMessage(ResourceConst.IS_PAGE_MANAGE_ROLE)) : false);
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData(SysXUtils.GetMessage(ResourceConst.IS_PAGE_MANAGE_ROLE), value);
            }
        }

        /// <summary>
        /// Gets the value of Validation Group.
        /// </summary>
        String IManageRoleView.ValidationGroup
        {
            get
            {
                return grdRoleDetail.ValidationSettings.ValidationGroup;
            }
        }

        #endregion

        #region Client Profile

        Boolean IManageRoleView.IsClientProfile
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Presenter.
        /// </summary>
        /// <value>
        /// Represents Manage Tenant Presenter.
        /// </value>

        public ManageRolePresenter Presenter
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
        String IManageRoleView.ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// IsAdmin.
        /// </summary>
        /// <value>
        /// Gets or sets the value for user is admin or not?
        /// </value>
        Boolean IManageRoleView.IsAdmin
        {
            get
            {
                return base.IsSysXAdmin;
            }
        }

        /// <summary>
        /// CurrentUserID.
        /// </summary>
        /// <value>
        /// Gets or sets the value for current user's id.
        /// </value>
        Int32 IManageRoleView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// TenantProducts.
        /// </summary>
        /// <value>
        /// Gets or sets the list of all products.
        /// </value>
        List<TenantProduct> IManageRoleView.TenantProducts
        {
            set;
            get;
        }

        /// <summary>
        /// TenantsRole.
        /// </summary>
        /// <value>
        /// Gets or sets the value for roles under the tenant.
        /// </value>
        Tenant IManageRoleView.TenantsRole
        {
            get;
            set;
        }

        /// <summary>
        /// LoginUserProductID.
        /// </summary>
        /// <value>
        /// Gets or sets the value for product id of logged in user.
        /// </value>
        Int32? IManageRoleView.LoginUserProductId
        {
            get
            {
                if (!CurrentViewContext.IsClientOnBoardingWizard && !CurrentViewContext.IsClientProfile)
                {
                    return base.SysXMembershipUser.ProductId;
                }
                else
                {
                    return CurrentViewContext.ViewContract.TenantProductId;
                }
            }
        }

        /// <summary>
        /// RoleDetails
        /// </summary>
        /// <value>Gets or sets the list of all role details.</value>
        /// <remarks></remarks>
        List<RoleDetail> IManageRoleView.RoleDetails
        {
            set
            {
                grdRoleDetail.DataSource = value;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <value>
        /// The view contract.
        /// </value>
        ManageRoleContract IManageRoleView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ManageRoleContract();
                }

                return _viewContract;
            }
        }

        /// <summary>
        /// SuccessMessage</summary>
        /// <value>
        /// Gets or sets the value for Success message.</value>
        String IManageRoleView.SuccessMessage
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
        IManageRoleView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #endregion

        #endregion

        #region Events

        public event EventHandler<EventArgs> MangeFeatureClick;

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
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MANAGE_ROLES);
                lblManageRole.Text = base.Title;
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
        /// <param name="e">     An <see cref="T:System.EventArgs"></see> object that contains the event
        ///  data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                args.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);

                //ClientOnBoardingWiz: Implementation 14/01/2012
                // CurrentViewContext.ViewContract.TenantId = args.ContainsKey("TenantId") ? Int32.Parse(args["TenantId"]) : AppConsts.NONE;
                if (!CurrentViewContext.IsClientOnBoardingWizard && !CurrentViewContext.IsClientProfile)
                {
                    CurrentViewContext.ViewContract.TenantId = args.ContainsKey("TenantId") ? Int32.Parse(args["TenantId"]) : AppConsts.NONE;
                    CurrentViewContext.ViewContract.IsLinkOnTenant = args.ContainsKey("IsLinkOnTenant") && args["IsLinkOnTenant"].Equals("yes", StringComparison.InvariantCultureIgnoreCase);
                }

                //ClientOnBoardingWiz: Implementation 14/01/2012
                if (CurrentViewContext.IsDataLoad && (!IsPageLoadByClientOnBoardingWizard))
                {
                    grdRoleDetail.Rebind();
                    IsPageLoadByClientOnBoardingWizard = true;
                }

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }

                Presenter.OnViewLoaded();

                if (!CurrentViewContext.TenantsRole.IsNull())
                {
                    lblSuffix.Text = SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_FOR) + CurrentViewContext.TenantsRole.TenantName;
                }

                base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_ROLES));
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
        /// Performs an insert operation for role.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdRoleDetail_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ErrorMessage = (e.Item.FindControl("lblErrorMessage") as Label).Text;
                CurrentViewContext.ViewContract.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.CreatedOn = DateTime.Now;
                CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.IsUserGroupLevel = (e.Item.FindControl("chkIsUserGroupLevel") as CheckBox).Checked;
                //Admin Entry Portal
                CurrentViewContext.ViewContract.ShowAdminEntryPortal = (e.Item.FindControl("chkShowAdminEntryPortal") as CheckBox).Checked;

                if (CurrentViewContext.IsAdmin || CurrentViewContext.ViewContract.IsLinkOnTenant)
                {
                    CurrentViewContext.ViewContract.TenantProductId = Convert.ToInt32((e.Item.FindControl("cmbProduct") as WclComboBox).SelectedValue);
                }
                else
                {
                    CurrentViewContext.ViewContract.TenantProductId = (Int32)CurrentViewContext.LoginUserProductId;
                }

                Presenter.RoleDetailSave();

                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                }
                else
                {
                    e.Canceled = false;
                }

                grdRoleDetail.MasterTableView.CurrentPageIndex = grdRoleDetail.PageCount;
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
        /// Retrieves a list of all roles.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdRoleDetail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.RetrievingRoleDetails();
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
        /// Performs an update operation for role's detail.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdRoleDetail_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ErrorMessage = (e.Item.FindControl("lblErrorMessage") as Label).Text;
                CurrentViewContext.ViewContract.RoleDetailId = Convert.ToString((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RoleDetailId"]);
                CurrentViewContext.ViewContract.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.IsUserGroupLevel = (e.Item.FindControl("chkIsUserGroupLevel") as CheckBox).Checked;
                CurrentViewContext.ViewContract.ShowAdminEntryPortal = (e.Item.FindControl("chkShowAdminEntryPortal") as CheckBox).Checked; //Admin Entry Portal

                if (CurrentViewContext.IsAdmin || CurrentViewContext.ViewContract.IsLinkOnTenant)
                {
                    CurrentViewContext.ViewContract.TenantProductId = Convert.ToInt32((e.Item.FindControl("cmbProduct") as WclComboBox).SelectedValue);
                }
                else
                {
                    CurrentViewContext.ViewContract.TenantProductId = (Int32)CurrentViewContext.LoginUserProductId;
                }

                Presenter.RoleDetailUpdate();

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
        /// Performs a delete operation for role.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdRoleDetail_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                var gridDataItem = e.Item as GridDataItem;
                if (!gridDataItem.IsNull())
                {
                    CurrentViewContext.ViewContract.RoleDetailId = Convert.ToString(gridDataItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RoleDetailId"]);
                }

                Presenter.DeleteRoleDetail();
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
        /// Event handler. Called by grdRoleDetail for item created events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdRoleDetail_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (GridEditFormItem)e.Item;
                    WclComboBox ddlProduct = (WclComboBox)editform.FindControl("cmbProduct");

                    if (CurrentViewContext.IsClientOnBoardingWizard || CurrentViewContext.IsClientProfile || CurrentViewContext.ViewContract.IsLinkOnTenant)
                    {
                        Presenter.BindComboWithProducts(CurrentViewContext.ViewContract.TenantId, IsSysXAdmin);
                    }
                    else
                    {
                        Presenter.BindComboWithProducts(base.SysXMembershipUser.TenantId, IsSysXAdmin);
                    }

                    if (!CurrentViewContext.TenantProducts.IsNull())
                    {
                        if (CurrentViewContext.TenantProducts.Count > SysXClientConsts.Zero)
                        {
                            TenantProduct tenantProduct = new TenantProduct
                            {
                                Name = AppConsts.COMBOBOX_ITEM_SELECT,
                                TenantProductID = AppConsts.NONE
                            };

                            CurrentViewContext.TenantProducts.Insert(AppConsts.NONE, tenantProduct);
                        }

                        ddlProduct.DataSource = CurrentViewContext.TenantProducts;
                        ddlProduct.DataBind();

                        if (CurrentViewContext.ViewContract.TenantId > AppConsts.NONE)
                        {
                            ddlProduct.SelectedValue = Convert.ToString(CurrentViewContext.ViewContract.TenantProductId);
                            ddlProduct.Enabled = false;
                        }
                        else if ((e.Item is GridEditFormInsertItem))
                        {
                            if (CurrentViewContext.TenantProducts.Count.Equals(AppConsts.ONE) && !IsSysXAdmin)
                            {
                                ddlProduct.Enabled = false;
                            }
                            else if (!IsSysXAdmin) //If not super admin means if client admin
                            {
                                ddlProduct.SelectedValue = Convert.ToString(CurrentViewContext.ViewContract.TenantProductId);
                                ddlProduct.Enabled = false;
                            }
                            else
                            {
                                ddlProduct.Enabled = true;
                            }
                        }
                        else
                        {
                            var roleDetail = (e.Item).DataItem as RoleDetail;

                            if (!roleDetail.IsNull())
                            {
                                ddlProduct.SelectedValue = Convert.ToString(roleDetail.ProductID);
                            }

                            ddlProduct.Enabled = false;
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
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdRoleDetail_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    grdRoleDetail.MasterTableView.IsItemInserted = false;
                }
                else
                {
                    grdRoleDetail.MasterTableView.ClearChildEditItems();
                }

                //ClientOnBoardingWiz: Implementation 14/01/2012
                if (e.CommandName.Equals("ManageFeatures"))
                {
                    MangeFeatureClick.Invoke(e.CommandArgument, e);
                }
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdRoleDetail);

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
        /// Event handler. Called by grdRoleDetail for item data bound events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdRoleDetail_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    RoleDetail roleDetail = (RoleDetail)e.Item.DataItem;
                    HtmlAnchor anchor = (HtmlAnchor)e.Item.FindControl("ancFeture");
                    HtmlAnchor anchorusers = (HtmlAnchor)e.Item.FindControl("ancUsers");
                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                {
                                                                    {"RoleDetailId", Convert.ToString(roleDetail.RoleDetailID)},
                                                                    {"RoleDetailName", Convert.ToString(roleDetail.Name)},
                                                                    {"RoleDetailDescription", Convert.ToString(roleDetail.RoleName)},
                                                                    {"ProductID", Convert.ToString(roleDetail.TenantProduct.TenantProductID)},
                                                                    {"Child", ChildControls.SecurityMapRoleFeature},
                                                                };

                    anchor.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    queryString.Remove("Child");
                    queryString.Add("Child", ChildControls.SecurityMapRolePolicy);
                    //HtmlAnchor anchorPolicy = (HtmlAnchor)e.Item.FindControl("ancPolicy");
                    //anchorPolicy.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                    queryString.Remove("Child");
                    queryString.Add("IsComingThroughTenant", CurrentViewContext.ViewContract.IsLinkOnTenant ? "yes" : "no");
                    queryString.Add("Child", ChildControls.SecurityManageUser);

                    anchorusers.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                    if (roleDetail.IsSystem)
                    {
                        (e.Item as GridEditableItem)["DeleteColumn"].Controls[0].Visible = false;
                        (e.Item as GridEditableItem)["DeleteColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);

                        (e.Item as GridEditableItem)["EditCommandColumn"].Controls[0].Visible = false;
                        (e.Item as GridEditableItem)["EditCommandColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);


                        //(e.Item as GridEditableItem)["SetPolicy"].Controls[0].Visible = false;
                        //(e.Item as GridEditableItem)["SetPolicy"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);

                        (e.Item as GridEditableItem)["ManageFeature"].Controls[0].Visible = false;
                        (e.Item as GridEditableItem)["ManageFeature"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                    }
                }

                #region ClientOnBoardingWizard

                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    if (CurrentViewContext.IsClientOnBoardingWizard || CurrentViewContext.IsClientProfile)
                    {
                        ((GridColumn)e.Item.OwnerTableView.Columns.FindByUniqueName("ManageFeature")).Visible = false;
                        //((GridColumn)e.Item.OwnerTableView.Columns.FindByUniqueName("SetPolicy")).Visible = false;
                        ((GridColumn)e.Item.OwnerTableView.Columns.FindByUniqueName("ManageUsers")).Visible = false;
                        ((GridColumn)e.Item.OwnerTableView.Columns.FindByUniqueName("ManageFeaturesWiz")).Visible = true;
                    }
                }

                #endregion

                if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
                {
                    RoleDetail roleDetail = (e.Item.DataItem as RoleDetail);

                    if (!roleDetail.IsNull())
                    {
                        (((GridEditFormItem)e.Item).FindControl("txtName") as WclTextBox).Text = roleDetail.Name.Split(new char[] { '_' }).FirstOrDefault();
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