using System;
using Microsoft.Practices.ObjectBuilder;
using Telerik.Web.UI;
using INTSOF.Utils;
using CoreWeb.Shell;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Entity;
using System.Linq;
using INTSOF.Utils.Consts;
using System.Configuration;


namespace CoreWeb.IntsofSecurityModel.Views
{
    public partial class ManageSubTenant : BaseUserControl, IManageSubTenantView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ManageSubTenantPresenter _presenter=new ManageSubTenantPresenter();
        private Dictionary<String, String> _childTenant = new Dictionary<String, String>();
        private List<Tenant> _tenants;
        private List<Tenant> _childTenants;
        private String _viewType;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the presenter.
        /// </summary>
        /// <value>The presenter.</value>
        /// <remarks></remarks>
        
        public ManageSubTenantPresenter Presenter
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
        /// Get/Set values of available child suppliers.
        /// </summary>
        List<Tenant> IManageSubTenantView.ChildTenants
        {
            get
            {
                return _childTenants;
            }
            set
            {
                _childTenants = value;
                grdManageSubTenant.DataSource = value;
            }
        }

        /// <summary>
        /// Get/Set supplierId.
        /// </summary>
        Int32 IManageSubTenantView.TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Get/Set childSupplierId.
        /// </summary>
        Int32 IManageSubTenantView.ChildTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Get/Set collection of suppliers.
        /// </summary>
        List<Tenant> IManageSubTenantView.Tenants
        {
            get
            {
                return _tenants;
            }
            set
            {
                _tenants = value;
                grdAddSubTenant.DataSource = value;
            }
        }

        /// <summary>
        /// Get/Set ChildSupplierNumbers.
        /// </summary>
        List<Int32> IManageSubTenantView.ChildTenantNumbers
        {
            get;
            set;
        }

        /// <summary>
        /// Get/Set ChildSupplierRelation.
        /// </summary>
        Dictionary<String, String> IManageSubTenantView.ChildTenantRelation
        {
            get;
            set;
        }

        /// <summary>
        /// Get/Set CurrentUserId.
        /// </summary>
        Int32 IManageSubTenantView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// Get/Set Tenant Name.
        /// </summary>
        String IManageSubTenantView.TenantName
        {
            get;
            set;
        }
        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IManageSubTenantView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #endregion

        #endregion

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
                base.Title = "Manage Third Party";
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
                if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
                {
                    Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>();
                    encryptedQueryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                    if (encryptedQueryString.ContainsKey(SysXSecurityConst.TENANTID))
                    {
                        CurrentViewContext.TenantId = Convert.ToInt32(encryptedQueryString[SysXSecurityConst.TENANTID]);
                    }
                }


                //base.SetPageTitle("Sub Tenants");

                base.SetPageTitle("Third Party");

                //base.SetPageTitle("Third Party");

                Presenter.getTenantName();

                lblManageSubTenant.Text = (String.Format("{0} > Manage Third Party", CurrentViewContext.TenantName.HtmlEncode()));

                //Presenter.getTenantName();
                //lblManageSubTenant.Text = (String.Format("{0} >> Manage Sub Tenant", CurrentViewContext.TenantName));

                //lblManageSubTenant.Text = (String.Format("{0} >> Manage Third Party", CurrentViewContext.TenantName));

                Presenter.OnViewLoaded();
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



        #region Grid Related Events

        /// <summary>
        /// Retrieves a list of all child suppliers.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdManageSubTenant_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.BindChildTenants();
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
        /// Event handler. Called by grdManageSubTenant for item command events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdManageSubTenant_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals(SysXSecurityConst.INITINSERT))
                {
                    dvAddSubTenant.Visible = true;
                    Presenter.BindAllTenant();
                    SysXWebSiteUtils.SessionService.SetCustomData(SysXSecurityConst.RETURNEDCHILDTENANTS, String.Empty);
                    grdAddSubTenant.DataBind();
                    e.Canceled = true;
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
        /// Performs a delete operation.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdManageSubTenant_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ChildTenantId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"]);
                Presenter.DeleteSubTenant();
                base.ShowSuccessMessage("Third Party Deleted Sucessfully");
                dvAddSubTenant.Visible = false;
                SysXWebSiteUtils.SessionService.SetCustomData(SysXSecurityConst.RETURNEDCHILDTENANTS, String.Empty);

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
        /// Retrieves a list of all suppliers.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdAddSubTenant_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.BindAllTenant();
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
        /// Event handler. Called by grdAddSubTenant for item data bound events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdAddSubTenant_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    if (e.Item.DataItem is Supplier)
                    {
                        // Manage checkbox status by checking session vale.
                        if (!SysXWebSiteUtils.SessionService.GetCustomData(SysXSecurityConst.RETURNEDCHILDTENANTS).IsNullOrEmpty())
                        {
                            Supplier supplier = (Supplier)e.Item.DataItem;
                            CheckBox chkSupplier = (CheckBox)e.Item.FindControl("chkStatus");
                            Dictionary<String, String> parentSuppliers = (Dictionary<String, String>)SysXWebSiteUtils.SessionService.GetCustomData(SysXSecurityConst.RETURNEDCHILDTENANTS);
                            if (parentSuppliers.ContainsKey(Convert.ToString(supplier.SupplierID)))
                            {
                                chkSupplier.Checked = true;
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

        #endregion

        #region Control Related Events

        /// <summary>
        /// Button cancel event.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                dvAddSubTenant.Visible = false;
                SysXWebSiteUtils.SessionService.SetCustomData(SysXSecurityConst.RETURNEDCHILDTENANTS, String.Empty);
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
        /// Button save event.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!SysXWebSiteUtils.SessionService.GetCustomData(SysXSecurityConst.RETURNEDCHILDTENANTS).IsNullOrEmpty())
                {
                    List<Int32> childTenantNumbers = new List<Int32>();
                    childTenantNumbers.Add(AppConsts.MINUS_ONE);
                    CurrentViewContext.ChildTenantNumbers = childTenantNumbers;

                    // assign session value to class variable.
                    if (!SysXWebSiteUtils.SessionService.GetCustomData(SysXSecurityConst.RETURNEDCHILDTENANTS).GetType().Name.Equals(AppConsts.STRING) && !((Dictionary<String, String>)SysXWebSiteUtils.SessionService.GetCustomData(SysXSecurityConst.RETURNEDCHILDTENANTS)).IsNullOrEmpty())
                    {
                        CurrentViewContext.ChildTenantRelation = (Dictionary<String, String>)SysXWebSiteUtils.SessionService.GetCustomData(SysXSecurityConst.RETURNEDCHILDTENANTS);
                    }

                    // assign session value to class variable.
                    if (!CurrentViewContext.ChildTenantRelation.IsNullOrEmpty())
                    {
                        CurrentViewContext.ChildTenantNumbers = CurrentViewContext.ChildTenantRelation.Select(relation => Convert.ToInt32(relation.Key)).ToList();
                    }

                    Presenter.AddSubTenant();
                    SysXWebSiteUtils.SessionService.SetCustomData(SysXSecurityConst.RETURNEDCHILDTENANTS, String.Empty);
                    dvAddSubTenant.Visible = false;

                    Presenter.BindChildTenants();
                    base.ShowSuccessMessage("Third Party Added Sucessfully");
                    grdManageSubTenant.MasterTableView.CurrentPageIndex = grdManageSubTenant.PageCount;
                    grdManageSubTenant.DataBind();
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
        /// Checkbox changed event.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void chkStatus_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // assign session value to private variable.
                if (!SysXWebSiteUtils.SessionService.GetCustomData(SysXSecurityConst.RETURNEDCHILDTENANTS).IsNullOrEmpty())
                {
                    _childTenant = (Dictionary<String, String>)SysXWebSiteUtils.SessionService.GetCustomData(SysXSecurityConst.RETURNEDCHILDTENANTS);
                }

                GridDataItem dataItem = (GridDataItem)(sender as CheckBox).NamingContainer;
                if (!_childTenant.ContainsKey(dataItem["TenantID"].Text))
                {
                    _childTenant.Add(dataItem["TenantID"].Text, dataItem["TenantName"].Text);
                }
                else
                {
                    _childTenant.Remove(dataItem["TenantID"].Text);
                }

                // assign private variable's vale to session variable.
                SysXWebSiteUtils.SessionService.SetCustomData(SysXSecurityConst.RETURNEDCHILDTENANTS, _childTenant);
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

    }
}

