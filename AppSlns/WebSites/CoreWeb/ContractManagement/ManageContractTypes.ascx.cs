#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using CoreWeb.Shell;
using System.Web.Services;
using System.Linq;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using CoreWeb.IntsofSecurityModel;


#endregion

#endregion
namespace CoreWeb.ContractManagement.Views
{
    public partial class ManageContractTypes : BaseUserControl, IManageContractTypesView
    {
        #region Variables

        #region Private
        private ManageContractTypesPresenter _presenter = new ManageContractTypesPresenter();
        private Int32 _tenantId;
        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        public ManageContractTypesPresenter Presenter
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

        public IManageContractTypesView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public IQueryable<ContractType> ListContractTypes
        {
            get;
            set;
        }

        public List<Tenant> ListTenants
        {
            set;
            get;
        }

        public String ContractTypeName
        {
            get;
            set;
        }

        public String ContractTypeLabel
        {
            get;
            set;
        }

        public String ContractTypeDescription
        {
            get;
            set;
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        public String SuccessMessage
        {
            get;
            set;
        }

        public Int32 ContractTypeId
        {
            get;
            set;
        }

        public String LastCode
        {
            get;
            set;
        }

        public Int32 TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        public Int32 SelectedTenantID
        {
            get
            {
                if (ViewState["ClientTenantID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["ClientTenantID"]);
                }
                return 0;
            }
            set
            {
                ViewState["ClientTenantID"] = value;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        #endregion

        #endregion

        #region Page Events

        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                if (Presenter.IsAdminLoggedIn())
                {
                    Presenter.OnViewLoaded();
                    ddlTenant.DataSource = ListTenants;
                    ddlTenant.DataBind();
                }
            }
            if (Presenter.IsAdminLoggedIn())
            {
                if (ddlTenant.SelectedValue == String.Empty || Convert.ToInt32(ddlTenant.SelectedValue) == AppConsts.NONE)
                {
                    dvContractTypes.Visible = false;
                }
            }
            else
            {
                SelectedTenantID = TenantId;
                pnlTenant.Visible = false;
            }
        }

        /// <summary>
        /// set the page title on bread crumb
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.Title = "Manage Contract Types";
            base.SetPageTitle("Manage Contract Types");
            base.OnInit(e);
        }

        #endregion

        #region Events

        #region Drop Down Selection

        protected void ddlTenant_SelectedIndexChange(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                {
                    SelectedTenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                    dvContractTypes.Visible = true;
                    grdContractTypes.Rebind();
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

        #region Grid Related Events

        protected void grdContractTypes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetContractTypes();
                grdContractTypes.DataSource = CurrentViewContext.ListContractTypes;
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

        protected void grdContractTypes_ItemCommand(object sender, GridCommandEventArgs e)
        {
            dvContractTypes.Visible = true;
            // Hide filter when exportig to pdf or word
            if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            {
                base.ConfigureExport(grdContractTypes);
            }
        }

        protected void grdContractTypes_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ContractTypeName = (e.Item.FindControl("txtContractTypeName") as WclTextBox).Text.Trim();
                CurrentViewContext.ContractTypeLabel = (e.Item.FindControl("txtContractTypeLabel") as WclTextBox).Text.Trim();
                CurrentViewContext.ContractTypeDescription = (e.Item.FindControl("txtContractTypeDescription") as WclTextBox).Text.Trim();
                Presenter.SaveContractTypes();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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

        protected void grdContractTypes_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ContractTypeName = (e.Item.FindControl("txtContractTypeName") as WclTextBox).Text.Trim();
                CurrentViewContext.ContractTypeLabel = (e.Item.FindControl("txtContractTypeLabel") as WclTextBox).Text.Trim();
                CurrentViewContext.ContractTypeDescription = (e.Item.FindControl("txtContractTypeDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.ContractTypeId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CT_ID"]);
                Presenter.UpdateContractTypes();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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

        protected void grdContractTypes_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.ContractTypeId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("CT_ID"));
                Presenter.DeleteContractTypes();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                    grdContractTypes.Rebind();
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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
    }
}

