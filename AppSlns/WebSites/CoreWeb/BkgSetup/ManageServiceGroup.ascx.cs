using System;
using Entity.ClientEntity;
using System.Collections.Generic;
using Telerik.Web.UI;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgSetup;
using CoreWeb.Shell.Views;


namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageServiceGroup : BaseUserControl, IManageServiceGroupView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ManageServiceGroupPresenter _presenter = new ManageServiceGroupPresenter();
        private String _viewType;
        private ServiceGroupContract _viewContract;
        private Int32 _tenantid;
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _selectedTenantId = AppConsts.NONE;

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        private ManageServiceGroupPresenter Presenter
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
        /// Gets and sets Logged In User TenantId
        /// </summary>
        public Int32 TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    _tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
            set { _tenantid = value; }
        }

        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        public List<Tenant> ListTenants
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                if (_selectedTenantId == AppConsts.NONE)
                {
                    Int32.TryParse(ddlTenant.SelectedValue, out _selectedTenantId);

                    if (_selectedTenantId == AppConsts.NONE)
                        _selectedTenantId = TenantId;
                }
                return _selectedTenantId;
            }
            set
            {
                _selectedTenantId = value;
            }
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        public Int32 DefaultTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["DefaultTenantId"]);
            }
            set
            {
                ViewState["DefaultTenantId"] = value;
            }
        }

        public Boolean IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.HasValue ? _isAdminLoggedIn.Value : true;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }

        /// <summary>
        /// Returns the current logged-in user ID.
        /// </summary>
        Int32 IManageServiceGroupView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        //Int32 IManageServiceGroupView.TenantId
        //{
        //    get;
        //    set;

        //}

        public IManageServiceGroupView CurrentViewContext
        {
            get { return this; }
        }

        ServiceGroupContract IManageServiceGroupView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ServiceGroupContract();
                }

                return _viewContract;
            }

        }

        List<BkgSvcGroup> IManageServiceGroupView.ServiceGroups
        {
            get;
            set;

        }

        public String ErrorMessage
        {
            get;
            set;
        }
        #endregion

        #region Public Properties


        

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Manage Service Groups";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    ApplyActionLevelPermission(ActionCollection, "Manage Service Group");
                    BindTenant();
                }
                SetDefaultSelectedTenantId();
                base.SetPageTitle("Manage Service Groups");
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

        #region Grid Related Events

        protected void grdServiceGroup_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedTenantId != DefaultTenantId)
                {
                    Presenter.GetServiceGroups();
                    grdServiceGroup.DataSource = CurrentViewContext.ServiceGroups;
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

        protected void grdServiceGroup_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                dvServiceGroup.Visible = true;
                if (e.CommandName == "PerformInsert")
                {
                    CurrentViewContext.ViewContract.ServiceGroupName = (e.Item.FindControl("txtServiceGroupName") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.ServiceGroupDesc = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.Active = Convert.ToBoolean((e.Item.FindControl("chkActive") as IsActiveToggle).Checked);
                    CurrentViewContext.ViewContract.TenantID = TenantId;
                    Presenter.SaveServiceGroup();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        // base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.ServiceGroupName));
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist.", CurrentViewContext.ViewContract.ServiceGroupName), MessageType.Error);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Service Group added successfully.");
                    }

                }
                if (e.CommandName == "Update")
                {
                    CurrentViewContext.ViewContract.ServiceGroupID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BSG_ID"]);
                    CurrentViewContext.ViewContract.ServiceGroupName = (e.Item.FindControl("txtServiceGroupName") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.ServiceGroupDesc = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.Active = Convert.ToBoolean((e.Item.FindControl("chkActive") as IsActiveToggle).Checked);
                    CurrentViewContext.ViewContract.TenantID = TenantId;
                    Presenter.UpdateServiceGroup();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        // base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.ServiceGroupName));
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist.", CurrentViewContext.ViewContract.ServiceGroupName), MessageType.Error);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Service Group updated successfully.");
                    }
                }
                if (e.CommandName == "Delete")
                {
                    Boolean isDeleted = true;
                    CurrentViewContext.ViewContract.ServiceGroupID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BSG_ID"]);
                    isDeleted = Presenter.DeleteServiceGroup();
                    if (isDeleted)
                    {
                        base.ShowSuccessMessage("Service Group deleted successfully.");
                    }
                    else
                    {
                        base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
                    }
                }
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                  || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdServiceGroup);
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
        /// Called when data is bound in grid.
        /// </summary>
        /// <param name="sender">Current control i.e. Grid</param>
        /// <param name="e">Contains related data</param>
        protected void grdServiceGroup_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    dataItem["BSG_Active"].Text = Convert.ToBoolean(dataItem["BSG_Active"].Text) == true ? Convert.ToString("Yes") : Convert.ToString("No");

                    HiddenField hdnfIsCreatedByAdmin = e.Item.FindControl("hdnfIsEditable") as HiddenField;
                    if (hdnfIsCreatedByAdmin.Value == "False")
                    {
                        dataItem["DeleteColumn"].Controls[0].Visible = false;
                        dataItem["EditCommandColumn"].Controls[0].Visible = false;
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

        #region DropDown Events

        /// <summary>
        /// Binds the categories as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                grdServiceGroup.CurrentPageIndex = 0;
                grdServiceGroup.MasterTableView.SortExpressions.Clear();
                grdServiceGroup.MasterTableView.FilterExpression = null;

                foreach (GridColumn column in grdServiceGroup.MasterTableView.OwnerGrid.Columns)
                {
                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;
                }
                SelectedTenantId = Convert.ToInt32(ddlTenant.SelectedValue);
                dvServiceGroup.Visible = true;
                grdServiceGroup.Rebind();
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

        private void BindTenant()
        {
            if (IsAdminLoggedIn == true)
            {
                dvServiceGroup.Visible = false;
                Presenter.GetTenants();
                ddlTenant.DataSource = ListTenants;
                ddlTenant.DataBind();
            }
            else
            {
                pnlTenant.Visible = false;
            }
        }

        private void SetDefaultSelectedTenantId()
        {
            if (ddlTenant.SelectedValue.IsNullOrEmpty())
            {
                SelectedTenantId = TenantId;
                ddlTenant.SelectedValue = Convert.ToString(TenantId);
            }
        }

        #endregion

        #region Apply Permissions

        public override List<ClsFeatureAction> ActionCollection
        {
            get
            {
                List<ClsFeatureAction> actionCollection = new List<ClsFeatureAction>();
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Add",
                    CustomActionLabel = "Add New",
                    ScreenName = "Manage Service Group"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Edit",
                    CustomActionLabel = "Edit",
                    ScreenName = "Manage Service Group"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Delete",
                    CustomActionLabel = "Delete",
                    ScreenName = "Manage Service Group"
                });
                return actionCollection;
            }
        }

        protected override void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
            if (permission.IsNotNull())
            {
                permission.ForEach(x =>
                {
                    switch (x.PermissionID)
                    {
                        case AppConsts.ONE:
                            {
                                break;
                            }
                        case AppConsts.THREE:
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "Add")
                                {
                                    grdServiceGroup.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdServiceGroup.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdServiceGroup.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }

        #endregion
    }
}