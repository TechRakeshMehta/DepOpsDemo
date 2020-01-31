#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using CoreWeb.Shell;
using System.Web.Services;
using System.Linq;
using System.Web.UI.WebControls;

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

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ManageSubscriptionOption : BaseUserControl, IManageSubscriptionOptionView
    {
        #region Variables

        #region Private Variables

        private ManageSubscriptionOptionPresenter _presenter = new ManageSubscriptionOptionPresenter();
        private Int32 _selectedTenantID = 0;
        private Int32 _tenantId;

        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties


        public ManageSubscriptionOptionPresenter Presenter
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

        public List<Tenant> ListTenants
        {
            set;
            get;
        }

        public Int32 SelectedTenantID
        {
            get
            {
                if (_selectedTenantID == 0 && ViewState["ClientTenantID"].IsNotNull())
                {
                    _selectedTenantID = Convert.ToInt32(ViewState["ClientTenantID"]);
                }
                return _selectedTenantID;
            }
            set
            {
                ViewState["ClientTenantID"] = value;
            }
        }

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Int32 CurrentUserTenantId
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

        public List<SubscriptionOption> ListSubscriptionOptions
        {
            get;
            set;
        }

        public Int32 SubscriptionOptionID
        {
            get;
            set;
        }

        /*UAT - 3032*/

        public IManageSubscriptionOptionView CurrentViewContext
        {
            get { return this; }
        }

        Int32 IManageSubscriptionOptionView.PreferredSelectedTenantID
        {
            get
            {
                if (!ViewState["PreferredSelectedTenantID"].IsNull())
                {
                    return (Int32)ViewState["PreferredSelectedTenantID"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["PreferredSelectedTenantID"] = value;
            }
        }
        /* END UAT - 3032*/
        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Subscription Options";
                base.SetPageTitle("Subscription Options");

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
        /// Loads the page ManageAssignmentProperties.aspx.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    if (Presenter.IsAdminLoggedIn())
                    {
                        Presenter.OnViewLoaded();
                        ddlTenant.DataSource = ListTenants;
                        ddlTenant.DataBind();
                        /*UAT-3032*/
                        GetPreferredSelectedTenant();
                        /*END UAT-3032*/
                    }
                }
                if (Presenter.IsAdminLoggedIn())
                {
                    if (ddlTenant.SelectedValue == String.Empty || Convert.ToInt32(ddlTenant.SelectedValue) == AppConsts.NONE)
                    {
                        dvSubscriptionOptions.Visible = false;
                    }
                }
                else
                {
                    pnlTenant.Visible = false;
                    SelectedTenantID = CurrentUserTenantId;
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

        #region Grid Events

        protected void grdSubscriptionOptions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetSubscriptionOptionsList();
                grdSubscriptionOptions.DataSource = ListSubscriptionOptions;
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
        /// Called when data is bound in grid.
        /// </summary>
        /// <param name="sender">Current control i.e. Grid</param>
        /// <param name="e">Contains related data</param>
        protected void grdSubscriptionOptions_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    SubscriptionOptionID = Convert.ToInt32(dataItem.GetDataKeyValue("SubscriptionOptionID"));
                    Boolean isSystem = Convert.ToBoolean(dataItem["IsSystem"].Text);
                    Boolean isSubscriptionOptionUsedByPackage = Presenter.IsSubscriptionOptionUsedByPackage();

                    if (isSystem == true)
                    {
                        ImageButton editColumn = dataItem["EditCommandColumn"].Controls[0] as ImageButton;
                        editColumn.Visible = false;
                        ImageButton deleteColumn = dataItem["DeleteColumn"].Controls[0] as ImageButton;
                        deleteColumn.Visible = false;
                    }
                    else if (isSubscriptionOptionUsedByPackage == true)
                    {
                        ImageButton deleteColumn = dataItem["DeleteColumn"].Controls[0] as ImageButton;
                        deleteColumn.Visible = false;
                        ImageButton editColumn = dataItem["EditCommandColumn"].Controls[0] as ImageButton;
                        editColumn.Visible = false;
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

        protected void grdSubscriptionOptions_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                dvSubscriptionOptions.Visible = true;
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdSubscriptionOptions);
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

        protected void grdSubscriptionOptions_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                SaveSubscriptionOption(e);
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

        protected void grdSubscriptionOptions_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                SaveSubscriptionOption(e);
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

        protected void grdSubscriptionOptions_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                SubscriptionOptionID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("SubscriptionOptionID"));
                Presenter.DeleteSubscriptionOption();
                base.ShowSuccessMessage("Subscription Option deleted successfully.");
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

        #region Button Events



        #endregion

        #region DropDown Events

        /// <summary>
        /// Binds the subscription options as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                {
                    SelectedTenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                    dvSubscriptionOptions.Visible = true;
                    grdSubscriptionOptions.Rebind();
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

        private void SaveSubscriptionOption(GridCommandEventArgs e)
        {
            GridEditableItem gridEditableItem = e.Item as GridEditableItem;
            Int32? subscriptionOptionID = null;
            Boolean yearAndMonthNotExists = true;
            String infoMessage = null;
            String ntxtYear = (e.Item.FindControl("ntxtYear") as WclNumericTextBox).Text;
            String ntxtMonth = (e.Item.FindControl("ntxtMonth") as WclNumericTextBox).Text;

            if (e.Item.ItemIndex > 0)
            {
                subscriptionOptionID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("SubscriptionOptionID"));
            }

            SubscriptionOption newSubscriptionOption = new SubscriptionOption();
            newSubscriptionOption.Label = (e.Item.FindControl("txtLabel") as WclTextBox).Text.Trim();
            newSubscriptionOption.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();

            if (!ntxtYear.IsNullOrEmpty())
            {
                newSubscriptionOption.Year = Convert.ToInt32(ntxtYear);
                yearAndMonthNotExists = false;
            }

            if (!ntxtMonth.IsNullOrEmpty())
            {
                newSubscriptionOption.Month = Convert.ToInt32(ntxtMonth);
                yearAndMonthNotExists = false;
            }

            if (yearAndMonthNotExists == true)
            {
                infoMessage = "Please enter either Year or/and Month.";
            }

            if (!Presenter.IsUniqueSubscriptionOptionLabel(newSubscriptionOption, subscriptionOptionID))
            {
                infoMessage += "<br>" + newSubscriptionOption.Label + " Label already exists.";
            }

            if (infoMessage.IsNotNull())
            {
                base.ShowInfoMessage(infoMessage);
                e.Canceled = true;
                return;
            }
            else if (Presenter.SaveSubscriptionOption(newSubscriptionOption, subscriptionOptionID))
            {
                if (subscriptionOptionID.IsNotNull())
                {
                    base.ShowSuccessMessage("Subscription Option updated successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("Subscription Option saved successfully.");
                }
            }
        }

        #endregion

        #endregion

        #region UAT-3032:- Sticky institution

        private void GetPreferredSelectedTenant()
        {
            if (SelectedTenantID.IsNullOrEmpty() || SelectedTenantID == AppConsts.NONE)
            {
               // Presenter.GetPreferredSelectedTenant();
                CurrentViewContext.PreferredSelectedTenantID = (Session["PreferredSelectedTenant"]).IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(Session["PreferredSelectedTenant"]);
                if (CurrentViewContext.PreferredSelectedTenantID > AppConsts.NONE)
                {
                    ddlTenant.SelectedValue = Convert.ToString(CurrentViewContext.PreferredSelectedTenantID);
                    SelectedTenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                }
            }
        }
        #endregion
    }
}

