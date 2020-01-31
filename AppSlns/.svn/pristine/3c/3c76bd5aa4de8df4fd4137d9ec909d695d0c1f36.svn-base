#region NameSpace

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

#region Project Specific
using INTSOF.Utils;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageFeeItem : BaseUserControl, IManageFeeItemView
    {
        #region Variables

        #region Private Variables

        private ManageFeeItemPresenter _presenter = new ManageFeeItemPresenter();
        private Int32 _tenantid;
        private String _viewType;
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _selectedTenantId = AppConsts.NONE;

        #endregion

        #region Public Variables


        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #endregion

        #region Public Properties

        public ManageFeeItemPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
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
        Int32 IManageFeeItemView.TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
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
        List<Tenant> IManageFeeItemView.ListTenants
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 IManageFeeItemView.SelectedTenantId
        {
            get
            {
                if (CurrentViewContext.IsAdminLoggedIn)
                {
                    if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                        return Convert.ToInt32(ddlTenant.SelectedValue);
                    return AppConsts.NONE;
                }
                else
                    return CurrentViewContext.TenantId;
            }
            set
            {
                ddlTenant.SelectedValue = Convert.ToString(value);
            }
        }

        Boolean IManageFeeItemView.IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.Value;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }

        Int32 IManageFeeItemView.currentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        String IManageFeeItemView.ErrorMessage
        {
            get;
            set;
        }

        Int32 IManageFeeItemView.SelectedServiceItemFeeTypeId
        {
            get;
            set;
        }
        String IManageFeeItemView.SuccessMessage
        {
            get;
            set;
        }
        String IManageFeeItemView.InfoMessage
        {
            get;
            set;
        }


        /// <summary>
        /// Gets and Sets list of Package Service Item Fee
        /// </summary>
        List<PackageServiceItemFee> IManageFeeItemView.ListPackageServiceItemFee
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and set List of ServiceItemFeeType
        /// </summary>
        List<lkpServiceItemFeeType> IManageFeeItemView.ListServiceItemFeeType
        {
            set;
            get;
        }

        String IManageFeeItemView.ItemFeeName
        {
            get;
            set;
        }

        String IManageFeeItemView.ItemFeeDescription
        {
            get;
            set;
        }

        String IManageFeeItemView.ItemFeeLabel
        {
            get;
            set;
        }

        Boolean IManageFeeItemView.IsGlobal
        {
            get;
            set;
        }


        #region Current View Context
        private IManageFeeItemView CurrentViewContext
        {
            get { return this; }
        }
        #endregion
        #endregion
        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Manage Global Fee Item";
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
                    ApplyActionLevelPermission(ActionCollection, "Manage Global Fee Item");
                    BindTenant();
                }
                ShowHideGrid();
                base.SetPageTitle("Manage Global Fee Item");
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Grid Events

        protected void grdFeeItems_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetPackageSvcItemFeeList();
                grdFeeItems.DataSource = CurrentViewContext.ListPackageServiceItemFee;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdFeeItems_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox cmbFeeItemType = editform.FindControl("cmbFeeItemType") as WclComboBox;
                    Presenter.GetServiceItemFeeTypeList();
                    cmbFeeItemType.DataSource = CurrentViewContext.ListServiceItemFeeType;
                    cmbFeeItemType.DataBind();
                    if (!(e.Item.DataItem is GridInsertionObject))
                    {
                        PackageServiceItemFee pkgSvcItemFee = e.Item.DataItem as PackageServiceItemFee;

                        if (pkgSvcItemFee != null)
                        {
                            CurrentViewContext.SelectedServiceItemFeeTypeId = pkgSvcItemFee.PSIF_ServiceItemFeeType;
                            if (!CurrentViewContext.SelectedServiceItemFeeTypeId.IsNullOrEmpty())
                            {
                                cmbFeeItemType.SelectedValue = CurrentViewContext.SelectedServiceItemFeeTypeId.ToString();
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
        }

        protected void grdFeeItems_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.SelectedServiceItemFeeTypeId = Convert.ToInt32((e.Item.FindControl("cmbFeeItemType") as WclComboBox).SelectedValue);
                CurrentViewContext.ItemFeeName = (e.Item.FindControl("txtFeeItemName") as WclTextBox).Text.Trim();
                //CurrentViewContext.ItemFeeLabel = (e.Item.FindControl("txtFeeItemLabel") as WclTextBox).Text.Trim();
                CurrentViewContext.ItemFeeDescription = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.IsGlobal = true; //(e.Item.FindControl("chkIsGlobal") as WclButton).Checked;
                Presenter.SavePackageServiceItemFeeRecord();
                if (!CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                {
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                else if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdFeeItems_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 pkgSvcItemFeeId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("PSIF_ID"));
                CurrentViewContext.SelectedServiceItemFeeTypeId = Convert.ToInt32((e.Item.FindControl("cmbFeeItemType") as WclComboBox).SelectedValue);
                CurrentViewContext.ItemFeeName = (e.Item.FindControl("txtFeeItemName") as WclTextBox).Text.Trim();
                //CurrentViewContext.ItemFeeLabel = (e.Item.FindControl("txtFeeItemLabel") as WclTextBox).Text.Trim();
                CurrentViewContext.ItemFeeDescription = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.IsGlobal = true; //(e.Item.FindControl("chkIsGlobal") as WclButton).Checked;
                Presenter.UpdatePackageServiceItemFeeRecord(pkgSvcItemFeeId);
                if (!CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                {
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                else if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdFeeItems_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 pkgSvcItemFeeId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("PSIF_ID"));
                Presenter.DeletePackageServiceItemFeeData(pkgSvcItemFeeId);
                if (!CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                {
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                else if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdFeeItems_ItemCommand(object sender, GridCommandEventArgs e)
        {
            #region DetailScreenNavigation

            if (e.CommandName.Equals("ManageFeeRecord"))
            {
                String feeItemName = String.Empty;
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    feeItemName = Convert.ToString(dataItem["PSIF_Name"].Text);
                }
                //SetSessionValues();
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                String PSIF_ID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PSIF_ID"].ToString();
                String SIFT_Code = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["lkpServiceItemFeeType.SIFT_Code"].ToString();
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    
                                                                    { "Child", ChildControls.ManageFeeRecord},
                                                                    { "PSIF_ID",Convert.ToString(PSIF_ID)},
                                                                    {"SIFT_Code",SIFT_Code},
                                                                    {"FeeItemName",feeItemName},
                                                                 
                                                                 };
                string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);

            }
            #endregion
        }

        #endregion

        #region DropDown Events

        /// <summary>
        /// Binds the attributes as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                {
                    grdFeeItems.Rebind();
                    grdFeeItems.Visible = true;
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

        /// <summary>
        /// Method to bind Tenant drop down if admin loggedIn or hide for client admin.
        /// </summary>
        private void BindTenant()
        {
            if (CurrentViewContext.IsAdminLoggedIn == true)
            {
                Presenter.GetTenants();
                ddlTenant.DataSource = CurrentViewContext.ListTenants;
                ddlTenant.DataBind();
            }
            else
            {
                pnlTenant.Visible = false;
            }
        }

        /// <summary>
        /// Set the selected 
        /// </summary>
        private void ShowHideGrid()
        {
            if (!CurrentViewContext.IsAdminLoggedIn)
            {
                divGrdFeeItem.Visible = false;
                divMainSection.Visible = false;
                base.ShowErrorInfoMessage("You don't have access to this page.Please contact administrator.");
            }
            else
            {
                //if (ddlTenant.SelectedValue.IsNullOrEmpty())
                //{
                //    divGrdFeeItem.Visible = false;
                //}
                //else
                //{
                //    divGrdFeeItem.Visible = true;
                //}
                divGrdFeeItem.Visible = true;
                divMainSection.Visible = true;
            }
        }

        #endregion

        #region Apply Permissions

        /// <summary>
        /// Override base ActionCollection property to set the action collections.
        /// </summary>
        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Add",
                    CustomActionLabel = "Add New",
                    ScreenName = "Manage Global Fee Item"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Edit",
                    CustomActionLabel = "Edit",
                    ScreenName = "Manage Global Fee Item"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Delete",
                    CustomActionLabel = "Delete",
                    ScreenName = "Manage Global Fee Item"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "ManageFeeRecord",
                    CustomActionLabel = "ManageFeeRecord",
                    ScreenName = "Manage Global Fee Item"
                });
                return actionCollection;
            }
        }

        public override List<String> ChildScreenPathCollection
        {
            get
            {
                List<String> childScreenPathCollection = new List<String>();
                childScreenPathCollection.Add(@"~/BkgSetup/UserControl/ManageFeeRecord.ascx");
                return childScreenPathCollection;
            }
        }

        /// <summary>
        /// Override ApplyActionLevelPermission to apply the action permissions.
        /// </summary>
        /// <param name="ctrlCollection"></param>
        /// <param name="screenName"></param>
        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
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
                                grdFeeItems.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                            }
                            else if (x.FeatureAction.CustomActionId == "Edit")
                            {
                                grdFeeItems.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                            }
                            else if (x.FeatureAction.CustomActionId == "Delete")
                            {
                                grdFeeItems.MasterTableView.GetColumn("DeleteColumn").Display = false;
                            }
                            else if (x.FeatureAction.CustomActionId == "ManageFeeRecord")
                            {
                                grdFeeItems.MasterTableView.GetColumn("ManageFeeRecord").Display = false;
                            }
                            break;
                        }
                }

            }
                );
        }

        #endregion
    }
}