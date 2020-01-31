#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageTenant.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using System.Collections;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.CommonControls.Views;
using Business.RepoManagers;
using INTSOF.Utils.Consts;
using INTERSOFT.WEB.UI.WebControls;
using System.Data.SqlClient;
#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to managing tenants in security module.
    /// </summary>
    /// <remarks></remarks>
    public partial class ManageTenant : BaseUserControl, IManageTenantView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ManageTenantPresenter _presenter = new ManageTenantPresenter();
        private String _viewType;
        private ManageTenantContract _viewContract;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the presenter.
        /// </summary>
        /// <value>The presenter.</value>
        /// <remarks></remarks>

        public ManageTenantPresenter Presenter
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

        // Tenant related properties.

        /// <summary>
        /// Sets the tenants.
        /// </summary>
        /// <value>The tenants.</value>
        /// <remarks></remarks>
        List<Tenant> IManageTenantView.Tenants
        {
            set
            {
                grdTenant.DataSource = value;
            }
        }

        /// <summary>
        /// Tenant Types</summary>
        /// <value>
        /// Sets the list of all tenant types.</value>
        List<lkpTenantType> IManageTenantView.TenantTypes
        {
            get;
            set;
        }

        // Product related properties.

        /// <summary>
        /// Gets or sets the tenant products.
        /// </summary>
        /// <value>The tenant products.</value>
        /// <remarks></remarks>
        List<TenantProduct> IManageTenantView.TenantProducts
        {
            get;
            set;
        }
        // Product related properties.

        /// <summary>
        /// Gets or sets the prefixs
        /// </summary>
        /// <value>The tenant products.</value>
        /// <remarks></remarks>
        List<OrganizationUserNamePrefix> IManageTenantView.OrganizationPrefixes
        {
            get;
            set;
        }
        // Organization related properties.

        /// <summary>
        /// Gets or sets the organizations.
        /// </summary>
        /// <value>The organizations.</value>
        /// <remarks></remarks>
        List<Organization> IManageTenantView.Organizations
        {
            get;
            set;
        }

        // Commonly used properties.

        /// <summary>
        /// Gets or sets the cities.
        /// </summary>
        /// <value>The cities.</value>
        /// <remarks></remarks>
        List<City> IManageTenantView.Cities
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the states.
        /// </summary>
        /// <value>The states.</value>
        /// <remarks></remarks>
        List<State> IManageTenantView.States
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        /// <remarks></remarks>
        String IManageTenantView.ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current user id.
        /// </summary>
        /// <remarks></remarks>
        Int32 IManageTenantView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ManageTenantContract IManageTenantView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ManageTenantContract();
                }

                return _viewContract;
            }
        }

        /// <summary>
        /// SuccessMessage</summary>
        /// <value>
        /// Gets or sets the value for Success message.</value>
        String IManageTenantView.SuccessMessage
        {
            get;
            set;
        }

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IManageTenantView CurrentViewContext
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
        /// Calls WebClientApplication.BuildItemWithCurrentContext after the events are handled
        /// to fire the DI engine.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MANAGE_TENANTS);
                lblManageTenant.Text = base.Title;
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
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }

                Presenter.OnViewLoaded();
                base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_TENANTS));
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
        /// Performs an update operation for tenant with its address and contact details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdTenant_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ErrorMessage = (e.Item.FindControl("lblErrorMessage") as Label).Text;

                //Tenant Information
                LocationInfo location = (e.Item.FindControl("locationTenant") as LocationInfo);

                CurrentViewContext.ViewContract.TenantName = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.TenantDesc = (e.Item.FindControl("txtTenantDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.TenantAddress = (e.Item.FindControl("txtAddress") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.TenantTypeId = Convert.ToInt32((e.Item.FindControl("cmbTenantType") as WclComboBox).SelectedValue);

                if (CurrentViewContext.ViewContract.TenantTypeId == Convert.ToInt32(TenantType.Institution))
                {
                    String txtDBName = (e.Item.FindControl("txtDBName") as WclTextBox).Text.Trim();
                    String txtDBServer = (e.Item.FindControl("txtDBServer") as WclTextBox).Text.Trim();
                    String txtDBUserName = (e.Item.FindControl("txtDBUserName") as WclTextBox).Text.Trim();
                    String txtDBPassword = (e.Item.FindControl("txtDBPassword") as TextBox).Text.Trim();
                    SqlConnectionStringBuilder tenantConnectionString = new SqlConnectionStringBuilder();
                    tenantConnectionString.DataSource = txtDBServer;
                    tenantConnectionString.InitialCatalog = txtDBName;
                    tenantConnectionString.UserID = txtDBUserName;
                    tenantConnectionString.Password = txtDBPassword;
                    tenantConnectionString.PersistSecurityInfo = true;

                    CurrentViewContext.ViewContract.TenantDBName = txtDBName;
                    CurrentViewContext.ViewContract.TenantDBServer = txtDBServer;
                    CurrentViewContext.ViewContract.TenantDBUserName = txtDBUserName;
                    CurrentViewContext.ViewContract.TenantDBPassword = txtDBPassword;
                    CurrentViewContext.ViewContract.TenantConnectinString = tenantConnectionString.ConnectionString;
                }

                if (!location.IsNull())
                {
                    CurrentViewContext.ViewContract.TenantZipId = location.ZipId;
                }

                CurrentViewContext.ViewContract.TenantPhone = (e.Item.FindControl("maskTxtPhone") as WclMaskedTextBox).Text;

                // Product Information
                CurrentViewContext.ViewContract.ProductName = (e.Item.FindControl("txtProductName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.ProductDescription = (e.Item.FindControl("txtProductDescription") as WclTextBox).Text.Trim();

                // Organization Information
                //LocationInfo locationOrg = (e.Item.FindControl("locationOrganization") as LocationInfo);
                CurrentViewContext.ViewContract.OrganizationName = (e.Item.FindControl("txtOrgName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.OrganizationAddress = (e.Item.FindControl("txtOrgAddress") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.OrganizationDescription = (e.Item.FindControl("txtOrgDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.OrganizationPhone = (e.Item.FindControl("txtOrgPhone") as WclMaskedTextBox).Text.Trim();

                //if (!locationOrg.IsNull())
                //{
                //    CurrentViewContext.ViewContract.OrganizationZipId = locationOrg.ZipId;
                //}
                CurrentViewContext.OrganizationPrefixes = (List<OrganizationUserNamePrefix>)ViewState["prefix"];
                Presenter.TenantSave();

                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                    e.Canceled = true;
                }
                else
                {
                    e.Canceled = false;
                    grdTenant.MasterTableView.CurrentPageIndex = grdTenant.PageCount;
                }
                ViewState["prefix"] = null;
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
        /// Retrieves a list of all tenant with its address and contact details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdTenant_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.RetrievingTenants();
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
        /// Performs an update operation for tenant with its address and contact details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdTenant_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ErrorMessage = (e.Item.FindControl("lblErrorMessage") as Label).Text;

                // Tenant Information
                LocationInfo location = (e.Item.FindControl("locationTenant") as LocationInfo);
                CurrentViewContext.ViewContract.TenantId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"]);
                CurrentViewContext.ViewContract.TenantName = (e.Item.FindControl("txtName") as WclTextBox).Text;
                CurrentViewContext.ViewContract.TenantDesc = (e.Item.FindControl("txtTenantDescription") as WclTextBox).Text;
                CurrentViewContext.ViewContract.TenantAddress = (e.Item.FindControl("txtAddress") as WclTextBox).Text;
                CurrentViewContext.ViewContract.TenantTypeId = Convert.ToInt32((e.Item.FindControl("cmbTenantType") as WclComboBox).SelectedValue);

                if (CurrentViewContext.ViewContract.TenantTypeId == Convert.ToInt32(TenantType.Institution))
                {
                    String txtDBName = (e.Item.FindControl("txtDBName") as WclTextBox).Text.Trim();
                    String txtDBServer = (e.Item.FindControl("txtDBServer") as WclTextBox).Text.Trim();
                    String txtDBUserName = (e.Item.FindControl("txtDBUserName") as WclTextBox).Text.Trim();
                    String txtDBPassword = (e.Item.FindControl("txtDBPassword") as TextBox).Text.Trim();
                    SqlConnectionStringBuilder tenantConnectionString = new SqlConnectionStringBuilder();
                    tenantConnectionString.DataSource = txtDBServer;
                    tenantConnectionString.InitialCatalog = txtDBName;
                    tenantConnectionString.UserID = txtDBUserName;
                    tenantConnectionString.Password = txtDBPassword;
                    tenantConnectionString.PersistSecurityInfo = true;

                    CurrentViewContext.ViewContract.TenantDBName = txtDBName;
                    CurrentViewContext.ViewContract.TenantDBServer = txtDBServer;
                    CurrentViewContext.ViewContract.TenantDBUserName = txtDBUserName;
                    CurrentViewContext.ViewContract.TenantDBPassword = txtDBPassword;
                    CurrentViewContext.ViewContract.TenantConnectinString = tenantConnectionString.ConnectionString;
                }

                if (!location.IsNull())
                {
                    CurrentViewContext.ViewContract.TenantZipId = location.ZipId;
                }

                CurrentViewContext.ViewContract.TenantPhone = (e.Item.FindControl("maskTxtPhone") as WclMaskedTextBox).Text;

                // Product Information
                CurrentViewContext.ViewContract.ProductName = (e.Item.FindControl("txtProductName") as WclTextBox).Text;
                CurrentViewContext.ViewContract.ProductDescription = (e.Item.FindControl("txtProductDescription") as WclTextBox).Text;

                // Organization Information
                //LocationInfo locationOrg = (e.Item.FindControl("locationOrganization") as LocationInfo);
                CurrentViewContext.ViewContract.OrganizationName = (e.Item.FindControl("txtOrgName") as WclTextBox).Text;
                CurrentViewContext.ViewContract.OrganizationAddress = (e.Item.FindControl("txtOrgAddress") as WclTextBox).Text;
                CurrentViewContext.ViewContract.OrganizationDescription = (e.Item.FindControl("txtOrgDescription") as WclTextBox).Text;
                CurrentViewContext.ViewContract.OrganizationPhone = (e.Item.FindControl("txtOrgPhone") as WclMaskedTextBox).Text;

                //if (!locationOrg.IsNull())
                //{
                //    CurrentViewContext.ViewContract.OrganizationZipId = locationOrg.ZipId;
                //}

                CurrentViewContext.OrganizationPrefixes = (List<OrganizationUserNamePrefix>)ViewState["prefix"];

                Presenter.TenantUpdate();
                ViewState["prefix"] = null;
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
        /// Performs a delete operation for tenant with its address and contact details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdTenant_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.TenantId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"]);
                Presenter.TenantDelete();
                ViewState["prefix"] = null;
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
        /// Event handler. Called by grdTenant for item command events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdTenant_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    grdTenant.MasterTableView.IsItemInserted = false;
                    ViewState["prefix"] = null;
                }
                else if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    ViewState["prefix"] = null;
                }
                else
                {
                    grdTenant.MasterTableView.ClearChildEditItems();
                }
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                     || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdTenant);

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
        /// Event handler. Called by grdTenant for item created events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdTenant_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (GridEditFormItem)e.Item;
                    WclComboBox ddlTenantType = (WclComboBox)editform.FindControl("cmbTenantType");
                    if (!CurrentViewContext.TenantTypes.IsNull())
                    {
                        ddlTenantType.DataSource = CurrentViewContext.TenantTypes;
                        ddlTenantType.DataBind();

                        if (!(e.Item is GridEditFormInsertItem))
                        {
                            Tenant tenant = (Tenant)e.Item.DataItem;

                            if (!tenant.IsNull())
                            {
                                ddlTenantType.SelectedValue = Convert.ToString(tenant.TenantTypeID);

                                ddlTenantType.SelectedValue = Convert.ToString(tenant.TenantTypeID);
                                HtmlGenericControl divClientConnectionString = (HtmlGenericControl)editform.FindControl("divConnectionString");
                                divClientConnectionString.Attributes.Add("class", "sxro sx3co currentconnDiv");

                                if (tenant.TenantTypeID == Convert.ToInt32(TenantType.Institution))
                                {
                                    var currentTenantDBConfiguration = tenant.ClientDBConfigurations.OrderBy(condition => condition.ClientDBConfigurationID).FirstOrDefault();
                                    divClientConnectionString.Attributes.Add("style", "display:inline");
                                    if (!currentTenantDBConfiguration.IsNull())
                                    {
                                        (e.Item.FindControl("txtDBName") as WclTextBox).Text = currentTenantDBConfiguration.CDB_DBName;
                                        (e.Item.FindControl("txtDBServer") as WclTextBox).Text = currentTenantDBConfiguration.CDB_DBServer;
                                        (e.Item.FindControl("txtDBUserName") as WclTextBox).Text = currentTenantDBConfiguration.CDB_UserName;
                                        //(e.Item.FindControl("txtDBPassword") as WclTextBox).Text = currentTenantDBConfiguration.CDB_Password;
                                        (e.Item.FindControl("txtDBPassword") as TextBox).Attributes["value"] = currentTenantDBConfiguration.CDB_Password;
                                    }
                                }
                                else
                                {
                                    divClientConnectionString.Attributes.Add("style", "display:none");
                                    (e.Item.FindControl("txtDBName") as WclTextBox).Text = AppConsts.ZERO;
                                    (e.Item.FindControl("txtDBServer") as WclTextBox).Text = AppConsts.ZERO;
                                    (e.Item.FindControl("txtDBUserName") as WclTextBox).Text = AppConsts.ZERO;
                                    (e.Item.FindControl("txtDBPassword") as TextBox).Attributes["value"] = AppConsts.ZERO;
                                }

                                if (!tenant.TenantProducts.IsNull() && tenant.TenantProducts.Count > AppConsts.NONE)
                                {
                                    var currentProduct = tenant.TenantProducts.OrderBy(condition => condition.TenantProductID).FirstOrDefault();

                                    if (!currentProduct.IsNull())
                                    {
                                        ((WclTextBox)editform.FindControl("txtProductName")).Text = currentProduct.Name;
                                        ((WclTextBox)editform.FindControl("txtProductDescription")).Text = currentProduct.Description;
                                    }

                                    //// Below condition will disable the Tenant Type dropdown, so that user won't be allowed to change tenant type if it is being created by some other resource.
                                    //if (!tenant.Suppliers.FirstOrDefault().IsNull() || !tenant.Clients.FirstOrDefault().IsNull())
                                    //{
                                    //    ddlTenantType.Enabled = false;
                                    //}
                                    //else
                                    //{
                                    //    ddlTenantType.Enabled = true;
                                    //}
                                }

                                if (!tenant.Organizations.IsNull())
                                {
                                    if (tenant.Organizations.Count() > AppConsts.NONE)
                                    {
                                        var currentOrganization = tenant.Organizations.OrderBy(condition => condition.OrganizationID).FirstOrDefault();

                                        if (!currentOrganization.IsNull())
                                        {
                                            ((WclTextBox)editform.FindControl("txtOrgName")).Text = currentOrganization.OrganizationName;
                                            ((WclTextBox)editform.FindControl("txtOrgAddress")).Text = currentOrganization.AddressHandle.Addresses.FirstOrDefault().Address1;
                                            ((WclTextBox)editform.FindControl("txtOrgDescription")).Text = Convert.ToString(currentOrganization.OrganizationDesc);

                                            List<OrganizationUserNamePrefix> list = new List<OrganizationUserNamePrefix>();
                                            foreach (OrganizationUserNamePrefix prefix in currentOrganization.OrganizationUserNamePrefixes)
                                            {
                                                list.Add(prefix);
                                            }
                                            ViewState["prefix"] = list;
                                            WclGrid grid = editform.FindControl("grdOrganizationUserNamePrefix") as WclGrid;
                                            if (!grid.IsNull())
                                            {
                                                grid.DataSource = list.Where(cond => cond.IsActive == true);
                                                grid.DataBind();
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            HtmlGenericControl divClientConnectionString = (HtmlGenericControl)editform.FindControl("divConnectionString");
                            divClientConnectionString.Attributes.Add("class", "sxro sx3co currentconnDiv");
                            //divClientConnectionString.Attributes.Add("style", "display:none");

                        }
                    }
                }

                if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
                {

                    if (e.Item.OwnerTableView.DataKeyNames.Contains("TenantID") && e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"].Equals(Convert.ToInt32(Presenter.GetSysXConfigValue("DefaultTenantID"))))
                    {
                        (e.Item as GridDataItem)["DeleteColumn"].Controls[AppConsts.NONE].Visible = false;
                        (e.Item as GridDataItem)["DeleteColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                        (e.Item as GridDataItem)["EditCommandColumn"].Controls[AppConsts.NONE].Visible = false;
                        (e.Item as GridDataItem)["EditCommandColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);

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
        /// Event handler. Called by grdTenant for item data bound events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdTenant_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();

                    if (e.Item is GridDataItem)
                    {
                        GridDataItem dataItem = (GridDataItem)e.Item;
                        if (e.Item.OwnerTableView.Columns.FindByUniqueNameSafe("Phone") != null)
                        {
                            dataItem["Phone"].Text = Presenter.GetFormattedPhoneNumber(Convert.ToString(dataItem["Phone"].Text));
                        }
                        
                    }

                    if (e.Item.DataItem is Tenant)
                    {
                        Tenant tenant = (Tenant)e.Item.DataItem;
                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString( tenant.TenantID) },
                                                                    { "Child", ChildControls.SecurityManageFootprint}
                                                                    
                                                                 };

                        HtmlAnchor ancServiceFootprint = (HtmlAnchor)e.Item.FindControl("ancServiceFootprint");

                        ancServiceFootprint.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                        queryString.Clear();
                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString( tenant.TenantID) },
                                                                    { "Child", ChildControls.ClientCompliances}, //CopyComplainceToClient (if want to call USP) 
                                                                    {"TenantName",tenant.TenantName}
                                                                 };

                        HtmlAnchor ancManageCompliance = (HtmlAnchor)e.Item.FindControl("ancProCompliance");
                        if (e.Item.OwnerTableView.DataKeyNames.Contains("TenantID") && !e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"].Equals(Convert.ToInt32(Presenter.GetSysXConfigValue("DefaultTenantID")))
                            && tenant.lkpTenantType.TenantTypeCode.Equals(TenantType.Institution.GetStringValue()))
                        {

                            ancManageCompliance.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                        }
                        else
                        {
                            ancManageCompliance.InnerText = String.Empty;

                        }
                        //if (e.Item.OwnerTableView.DataKeyNames.Contains("TenantID") && e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"].Equals(Convert.ToInt32(Presenter.GetSysXConfigValue("DefaultTenantID"))))
                        //{
                        //    ancManageCompliance.InnerText = String.Empty;
                        //}
                        //else
                        //{
                        //    ancManageCompliance.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                        //}
                        queryString.Clear();
                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString( tenant.TenantID) },
                                                                    { "Child", ChildControls.webSiteSetup},
                                                                 };
                        HtmlAnchor ancWebsiteSetup = (HtmlAnchor)e.Item.FindControl("ancWebsiteSetup");
                        //if (e.Item.OwnerTableView.DataKeyNames.Contains("TenantID") && !e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"].Equals(Convert.ToInt32(Presenter.GetSysXConfigValue("DefaultTenantID")))
                        //    && 
                        if (e.Item.OwnerTableView.DataKeyNames.Contains("TenantID") && e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"].Equals(Convert.ToInt32(Presenter.GetSysXConfigValue("DefaultTenantID"))))
                        {
                            ancWebsiteSetup.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                        }
                        else
                        {
                            if (tenant.lkpTenantType.TenantTypeCode.Equals(TenantType.Institution.GetStringValue()))
                            {

                                ancWebsiteSetup.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                            }
                            else
                            {
                                ancWebsiteSetup.InnerText = String.Empty;

                            }
                        }

                        queryString.Clear();
                        queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString( tenant.TenantID) },
                                                                    { "Child", ChildControls.SecurityManageSubTenant},
                                                                 };

                        HtmlAnchor ancManageSubTenant = (HtmlAnchor)e.Item.FindControl("ancManageSubTenant");
                        if (e.Item.OwnerTableView.DataKeyNames.Contains("TenantID") && !e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"].Equals(Convert.ToInt32(Presenter.GetSysXConfigValue("DefaultTenantID")))
                            && tenant.lkpTenantType.TenantTypeCode.Equals(TenantType.Institution.GetStringValue()))
                        {
                            ancManageSubTenant.InnerText = "Manage Third Party";
                            ancManageSubTenant.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                        }
                        else
                        {
                            ancManageSubTenant.InnerText = String.Empty;

                        }

                    }

                    if (e.Item.DataItem is Organization)
                    {
                        Organization organization = (Organization)e.Item.DataItem;
                        

                        (e.Item as GridDataItem)["OrganizationAddress"].Text = (!organization.AddressHandle.IsNull()) ? organization.AddressHandle.Addresses.FirstOrDefault().Address1 : String.Empty;
                        (e.Item as GridDataItem)["OrganizationCity"].Text = (!organization.AddressHandle.IsNull()) ? organization.AddressHandle.Addresses.FirstOrDefault().ZipCode.City.CityName : String.Empty;
                        (e.Item as GridDataItem)["OrganizationState"].Text = (!organization.AddressHandle.IsNull()) ? organization.AddressHandle.Addresses.FirstOrDefault().ZipCode.City.State.StateName : String.Empty;
                        (e.Item as GridDataItem)["OrganizationZipCode"].Text = (!organization.AddressHandle.IsNull()) ? organization.AddressHandle.Addresses.FirstOrDefault().ZipCode.ZipCode1 : String.Empty;
                        Label lblprefixname = e.Item.FindControl("lblprifixName") as Label;
                        if (!lblprefixname.IsNull())
                        {
                            foreach (OrganizationUserNamePrefix prefix in organization.OrganizationUserNamePrefixes)
                            {
                                if (lblprefixname.Text.Length > AppConsts.NONE)
                                    lblprefixname.Text += ",";
                                lblprefixname.Text += prefix.UserNamePrefix.HtmlEncode();
                            }
                        }
                        if (e.Item.OwnerTableView.Columns.FindByUniqueNameSafe("OrganizationPhone") != null)
                        {
                            String phone  = organization.Contact.GetContactDetailValue(ContactType.PrimaryPhone.GetStringValue());
                            (e.Item as GridDataItem)["OrganizationPhone"].Text = Presenter.GetFormattedPhoneNumber(phone);
                        }


                        HtmlAnchor anchorUser = (HtmlAnchor)e.Item.FindControl("ancManageUser");
                        queryString.Add("TenantId", Convert.ToString(organization.TenantID));
                        queryString.Add("IsLinkOnTenant", "yes");
                        queryString["Child"] = ChildControls.SecurityManageUser;
                        anchorUser.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                        //Manage Programs temporary code for prototypes
                        //Dev team please review and correct
                        //HtmlAnchor anchorPrograms = (HtmlAnchor)e.Item.FindControl("ancManageProgram");
                        //queryString.Clear();
                        //queryString.Add("TenantId", Convert.ToString(organization.TenantID));
                        //queryString.Add("OrganizationId", Convert.ToString(organization.OrganizationID));
                        //queryString.Add("IsLinkOnTenant", "yes");
                        //queryString["Child"] = @"ManagePrograms.ascx";
                        //anchorPrograms.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                    }

                    if (e.Item.DataItem is TenantProduct)
                    {
                        TenantProduct tenantProduct = (TenantProduct)e.Item.DataItem;
                        HtmlAnchor anchorRole = (HtmlAnchor)e.Item.FindControl("ancManageRole");
                        queryString.Add("TenantId", Convert.ToString(tenantProduct.TenantID));
                        queryString.Add("IsLinkOnTenant", "yes");
                        queryString["Child"] = ChildControls.SecurityManageRole;
                        anchorRole.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                        //if (!tenantProduct.TenantID.Equals(Convert.ToInt32(Presenter.GetSysXConfigValue("DefaultTenantID"))))
                        //{
                        HtmlAnchor anchorProFeature = (HtmlAnchor)e.Item.FindControl("ancProFeature");
                        Dictionary<String, String> queryStringManageFeatures = new Dictionary<String, String>
                                                                      {
                                                                          {
                                                                              "TenantProductId",
                                                                              Convert.ToString(
                                                                                  tenantProduct.TenantProductID)
                                                                              },
                                                                          {
                                                                              "Child",
                                                                              ChildControls.SecurityMapProductFeature
                                                                              }
                                                                      };
                        anchorProFeature.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryStringManageFeatures.ToEncryptedQueryString());
                        //}
                        //else
                        //{
                        //    //((GridColumn)e.Item.OwnerTableView.Columns.FindByUniqueName("ManageFeatures")).Visible = false;
                        //}
                    }
                }

                if (e.Item is GridEditFormInsertItem)
                {
                    (e.Item.FindControl("locationTenant") as LocationInfo).ValidationGroup = "grpValdManageTenant";
                    //(e.Item.FindControl("locationOrganization") as LocationInfo).ValidationGroup = "grpValdManageTenant";
                    //(e.Item.FindControl("locationTenant") as LocationInfo).ValidationGroup = "grpValdManageTenant";
                    WclGrid prefixGrid = e.Item.FindControl("grdOrganizationUserNamePrefix") as WclGrid;
                    if (!prefixGrid.IsNull())
                    {
                        List<OrganizationUserNamePrefix> organizationUserNamesprefixes = new List<OrganizationUserNamePrefix>();
                        prefixGrid.DataSource = organizationUserNamesprefixes;
                        prefixGrid.DataBind();
                    }
                }

                if (!(e.Item is GridEditFormInsertItem) && e.Item.IsInEditMode)
                {
                    Tenant tenant = (Tenant)e.Item.DataItem;
                    (e.Item.FindControl("maskTxtPhone") as WclMaskedTextBox).Text = tenant.Contact.GetContactDetailValue(ContactType.PrimaryPhone.GetStringValue());
                    (e.Item.FindControl("txtOrgPhone") as WclMaskedTextBox).Text = IsSysXAdmin ? tenant.Organizations.Where(condition => condition.ParentOrganizationID == null).FirstOrDefault().Contact.GetContactDetailValue(ContactType.PrimaryPhone.GetStringValue()) : tenant.Organizations.FirstOrDefault().Contact.GetContactDetailValue(ContactType.PrimaryPhone.GetStringValue());

                    LocationInfo location = (e.Item.FindControl("locationTenant") as LocationInfo);

                    if (!tenant.IsNull())
                    {
                        CurrentViewContext.ViewContract.ZipCode = tenant.AddressHandle.Addresses.FirstOrDefault().ZipCode.ZipCode1;

                        if (!location.IsNull())
                        {
                            location.ZipId = Convert.ToInt32(tenant.AddressHandle.Addresses.FirstOrDefault().ZipCode.ZipCodeID);
                        }
                    }

                    //LocationInfo locationOrganization = (e.Item.FindControl("locationOrganization") as LocationInfo);

                    if (!tenant.Organizations.IsNull())
                    {
                        //if (!locationOrganization.IsNull())
                        //{
                        //    locationOrganization.ZipId = Convert.ToInt32(tenant.Organizations.FirstOrDefault(condition => condition.ParentOrganizationID == null).AddressHandle.Addresses.FirstOrDefault().ZipCode.ZipCodeID);
                        //}

                        (e.Item.FindControl("maskTxtPhone") as WclMaskedTextBox).Text = tenant.Contact.GetContactDetailValue(ContactType.PrimaryPhone.GetStringValue());
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
        /// Retrieves Tenant details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdTenant_DetailTableBound(object sender, GridDetailTableDataBindEventArgs e)
        {
            try
            {
                GridDataItem gridDataItem = (GridDataItem)e.DetailTableView.ParentItem;
                CurrentViewContext.ViewContract.TenantId = Convert.ToInt32(Convert.ToString(gridDataItem.GetDataKeyValue("TenantID")));

                Presenter.RetrievingTenantProducts();
                grdTenant.MasterTableView.DetailTables[AppConsts.NONE].DataSource = CurrentViewContext.TenantProducts;

                Presenter.RetrievingOrganization();
                grdTenant.MasterTableView.DetailTables[AppConsts.ONE].DataSource = CurrentViewContext.Organizations;
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

        protected void grdOrganizationUserNamePrefix_InsertCommand(object sender, GridCommandEventArgs e)
        {
            List<OrganizationUserNamePrefix> list = null;
            WclGrid prefixGrid = (WclGrid)sender;

            try
            {
                if (!ViewState["prefix"].IsNull())
                {
                    list = (List<OrganizationUserNamePrefix>)ViewState["prefix"];
                }
                else
                {
                    list = new List<OrganizationUserNamePrefix>();
                }

                OrganizationUserNamePrefix organizationUserNameprefix = new OrganizationUserNamePrefix();
                organizationUserNameprefix.UserNamePrefix = ((WclTextBox)e.Item.FindControl("txtPrefixName")).Text;
                organizationUserNameprefix.Description = ((WclTextBox)e.Item.FindControl("txtdescription")).Text;
                organizationUserNameprefix.CreatedByID = CurrentUserId;
                organizationUserNameprefix.CreatedOn = DateTime.Now;
                organizationUserNameprefix.IsActive = true;
                if (!Presenter.IsUserNamePrefixExist(organizationUserNameprefix.UserNamePrefix, AppConsts.NONE))
                {
                    list.Add(organizationUserNameprefix);
                }
                else
                {
                    lblSuccess.ShowMessage("User Name prefix already exists.", MessageType.Information);
                }
                ViewState["prefix"] = list;

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

        protected void grdOrganizationUserNamePrefix_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            WclGrid prefixGrid = (WclGrid)sender;

            try
            {
                List<OrganizationUserNamePrefix> list = null;
                if (!ViewState["prefix"].IsNull())
                {
                    list = (List<OrganizationUserNamePrefix>)ViewState["prefix"];
                }
                else
                {
                    list = new List<OrganizationUserNamePrefix>();
                }
                prefixGrid.DataSource = list.Where(condition => condition.IsActive == true);
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

        protected void grdOrganizationUserNamePrefix_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            List<OrganizationUserNamePrefix> list = null;
            OrganizationUserNamePrefix organizationuserPrefix = null;
            WclGrid prefixGrid = (WclGrid)sender;

            try
            {
                if (!ViewState["prefix"].IsNull())
                {
                    list = (List<OrganizationUserNamePrefix>)ViewState["prefix"];
                }
                else
                {
                    list = new List<OrganizationUserNamePrefix>();
                }
                Int32 prefixid = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserNamePrefixID"]);
                if (prefixid == AppConsts.NONE)
                {
                    organizationuserPrefix = list[e.Item.ItemIndex];
                }
                else
                {
                    organizationuserPrefix = list.Where(cond => cond.OrganizationUserNamePrefixID == prefixid).FirstOrDefault();
                }

                if (!Presenter.IsUserNamePrefixExist(((WclTextBox)e.Item.FindControl("txtPrefixName")).Text, prefixid))
                {
                    organizationuserPrefix.UserNamePrefix = ((WclTextBox)e.Item.FindControl("txtPrefixName")).Text;
                    organizationuserPrefix.Description = ((WclTextBox)e.Item.FindControl("txtdescription")).Text;
                    organizationuserPrefix.ModifiedByID = CurrentUserId;
                    organizationuserPrefix.ModifiedOn = DateTime.Now;
                    ViewState["prefix"] = list;
                }
                else
                {
                    lblSuccess.ShowMessage("User Name prefix already exists.", MessageType.Information);
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
        protected void grdOrganizationUserNamePrefix_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            List<OrganizationUserNamePrefix> list = null;
            WclGrid prefixGrid = (WclGrid)sender;
            try
            {
                if (!ViewState["prefix"].IsNull())
                {
                    list = (List<OrganizationUserNamePrefix>)ViewState["prefix"];
                }
                else
                {
                    list = new List<OrganizationUserNamePrefix>();
                }

                Int32 prefixid = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserNamePrefixID"]);
                OrganizationUserNamePrefix organizationuserPrefix = list.Where(cond => cond.OrganizationUserNamePrefixID == prefixid).FirstOrDefault();
                organizationuserPrefix.IsActive = false;
                organizationuserPrefix.ModifiedByID = CurrentUserId;
                organizationuserPrefix.ModifiedOn = DateTime.Now;
                ViewState["prefix"] = list;
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

        #endregion

    }
}
