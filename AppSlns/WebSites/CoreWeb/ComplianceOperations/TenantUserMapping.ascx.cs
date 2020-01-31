using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using Entity;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class TenantUserMapping : BaseUserControl, ITenantUserMappingView
    {
        #region Variables
        private TenantUserMappingPresenter _presenter = new TenantUserMappingPresenter();
        #endregion

        #region Properties

        public TenantUserMappingPresenter Presenter
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

        public ITenantUserMappingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<Tenant> lstTenants
        {
            get
            {
                if (!ViewState["lstTenants"].IsNullOrEmpty())
                {
                    return ViewState["lstTenants"] as List<Tenant>;
                }
                return new List<Tenant>();
            }
            set
            {
                ViewState["lstTenants"] = value;
            }
        }

        public Int32 selectedTenantID
        {
            get;
            set;
        }

        public List<Entity.OrganizationUser> lstOrganizationUser
        {
            get;
            set;
        }
        public List<TenantUserMappingContract> lstTenantUserMappings
        {
            get
            {
                if (!ViewState["lstTenantUserMappings"].IsNullOrEmpty())
                {
                    return ViewState["lstTenantUserMappings"] as List<TenantUserMappingContract>;
                }
                return new List<TenantUserMappingContract>();
            }
            set
            {
                ViewState["lstTenantUserMappings"] = value;
            }
        }

        Int32 ITenantUserMappingView.CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }


        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Manage Tenant User Mapping";
                base.SetPageTitle("Manage Tenant User Mapping");
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    BindTenant();
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #region Grid Events

        protected void grdTenantUserMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetTenantUserMappings();
                grdTenantUserMapping.DataSource = CurrentViewContext.lstTenantUserMappings;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdTenantUserMapping_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.PerformInsertCommandName)
                {
                    WclComboBox ddlTenant = e.Item.FindControl("ddlTenant") as WclComboBox;
                    WclComboBox ddlUser = e.Item.FindControl("ddlUser") as WclComboBox;

                    List<TenantUserMappingContract> lstTenantUserMappings = new List<TenantUserMappingContract>();
                    foreach (var item in ddlUser.CheckedItems)
                    {
                        TenantUserMappingContract tenantUserMapping = new TenantUserMappingContract();
                        tenantUserMapping.TenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                        tenantUserMapping.OrganizationUserID = Convert.ToInt32(item.Value);

                        lstTenantUserMappings.Add(tenantUserMapping);
                    }
                    if (!lstTenantUserMappings.IsNullOrEmpty())
                    {
                        if (Presenter.SaveTenantUserMapping(lstTenantUserMappings))
                        {
                            e.Canceled = false;
                            base.ShowSuccessMessage("The tenant user mappings are saved successfully.");
                        }
                        else
                        {
                            e.Canceled = true;
                            base.ShowErrorMessage("Some error has occurred. Please try again.");
                        }
                    }

                }
                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    Int32 tenantUserMappingId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TUM_ID"]);
                    if (!tenantUserMappingId.IsNullOrEmpty() && tenantUserMappingId > AppConsts.NONE)
                    {
                        if (Presenter.DeleteTenantUserMapping(tenantUserMappingId))
                        {
                            base.ShowSuccessMessage("Tenant user mapping is deleted successfully.");
                        }
                        else
                        {
                            e.Canceled = true;
                            base.ShowErrorMessage("Some error has occurred. Please try again.");
                        }
                    }
                }

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdTenantUserMapping_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox ddlTenant = e.Item.FindControl("ddlTenant") as WclComboBox;
                    ddlTenant.DataSource = CurrentViewContext.lstTenants;
                    ddlTenant.DataBind();
                    ddlTenant.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #region DropDown Events

        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                WclComboBox ddlTenant = sender as WclComboBox;
                if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                {
                    CurrentViewContext.selectedTenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                    WclComboBox ddlUser = ddlTenant.NamingContainer.FindControl("ddlUser") as WclComboBox;
                    BindUsers();
                    ddlUser.DataSource = CurrentViewContext.lstOrganizationUser;
                    ddlUser.DataBind();
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion
        #endregion

        #region Methods

        private void BindTenant()
        {
            Presenter.GetTenants();
        }

        private void BindUsers()
        {
            Presenter.GetUserList();
        }

        #endregion





    }
}