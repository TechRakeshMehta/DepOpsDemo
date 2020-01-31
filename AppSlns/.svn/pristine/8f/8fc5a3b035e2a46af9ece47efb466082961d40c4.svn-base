using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.BkgSetup.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageOrderColorStatus : BaseUserControl, IManageOrderColorStatusView
    {
        #region Private Variables

        private ManageOrderColorStatusPresenter _presenter = new ManageOrderColorStatusPresenter();
        private ManageOrderColorStatusContract _viewContract;

        private Int32 _tenantid;
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _selectedTenantId = AppConsts.NONE;
        #endregion

        #region Protected Variables

        protected String StatusColorImages = @"~/images/status/";

        #endregion

        #region Public Properties

        public IManageOrderColorStatusView CurrentViewContext
        {
            get { return this; }
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


        public ManageOrderColorStatusPresenter Presenter
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

        public Int32 SelectedInstitutionOrderFlagId
        {
            get;
            set;
        }

        #endregion

        #region Private Properties


        ManageOrderColorStatusContract IManageOrderColorStatusView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ManageOrderColorStatusContract();
                }

                return _viewContract;
            }
        }


        List<lkpOrderFlag> IManageOrderColorStatusView.lstOrderFlags
        {
            get;
            set;
        }

        List<InstitutionOrderFlag> IManageOrderColorStatusView.lstInstitutionOrderFlags
        {
            get;
            set;
        }

        List<Tenant> IManageOrderColorStatusView.lstTenants
        {
            get;
            set;
        }

        Int32 IManageOrderColorStatusView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        String IManageOrderColorStatusView.ErrorMessage
        {
            get;
            set;
        }

        #endregion

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {

                base.Title = "Manage Order Status Color";
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
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                BindTenant();

                ApplyActionLevelPermission(ActionCollection, "Manage Order Status Color");
            }
            SetDefaultSelectedTenantId();
            base.SetPageTitle("Manage Order Status Color");
        }

        #endregion

        #region DropDown Events

        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (ddlTenant.SelectedValue != AppConsts.ONE.ToString())
                { 
                grdOrderColorStatus.CurrentPageIndex = 0;
                grdOrderColorStatus.MasterTableView.SortExpressions.Clear();
                grdOrderColorStatus.MasterTableView.FilterExpression = null;

                foreach (GridColumn column in grdOrderColorStatus.MasterTableView.OwnerGrid.Columns)
                {
                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = String.Empty;
                }
                //SelectedTenantId = Convert.ToInt32(ddlTenant.SelectedValue);
                dvOrderColorStatus.Visible = true; 
                }
                grdOrderColorStatus.Rebind();
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

        protected void grdOrderColorStatus_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (ddlTenant.SelectedValue != AppConsts.ONE.ToString())
                {
                    Presenter.GetInstituteOrderFlags();
                    grdOrderColorStatus.DataSource = CurrentViewContext.lstInstitutionOrderFlags;
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

        protected void grdOrderColorStatus_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                dvOrderColorStatus.Visible = true;
                if (e.CommandName == "PerformInsert")
                {
                    RadComboBox rcbFlag = (e.Item.FindControl("rcbInstitutionStatusColorIcons") as RadComboBox);
                    CurrentViewContext.ViewContract.SelectedOrderFlagId = Convert.ToInt32(rcbFlag.SelectedValue);
                    CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.IsSuccessIndicator = Convert.ToBoolean((e.Item.FindControl("chkIsSuccessIndicator") as WclButton).Checked);
                    Presenter.SaveInstitutionOrderFlagDetail();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist.", rcbFlag.SelectedItem.Text), MessageType.Error);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Order Status Color added successfully.");
                    }

                }
                if (e.CommandName == "Update")
                {
                    CurrentViewContext.SelectedInstitutionOrderFlagId = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IOF_ID"]);
                    RadComboBox rcbFlag = (e.Item.FindControl("rcbInstitutionStatusColorIcons") as RadComboBox);
                    CurrentViewContext.ViewContract.SelectedOrderFlagId = Convert.ToInt32(rcbFlag.SelectedValue);
                    CurrentViewContext.ViewContract.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.IsSuccessIndicator = Convert.ToBoolean((e.Item.FindControl("chkIsSuccessIndicator") as WclButton).Checked);
                    Presenter.UpdateInstitutionOrderFlagDetail();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist.", rcbFlag.SelectedItem.Text), MessageType.Error);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Order Status Color updated successfully.");
                    }
                }
                if (e.CommandName == "Delete")
                {
                    Boolean isDeleted = true;
                    CurrentViewContext.SelectedInstitutionOrderFlagId = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IOF_ID"]);
                    isDeleted = Presenter.DeleteInstitutionOrderFlag();
                    if (isDeleted)
                    {
                        base.ShowSuccessMessage("Order Status Color deleted successfully.");
                    }
                    else
                    {
                        base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
                    }
                }
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                  || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdOrderColorStatus);
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
        protected void grdOrderColorStatus_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    InstitutionOrderFlag currentItem = e.Item.DataItem as InstitutionOrderFlag;
                    RadComboBox rcbInstitutionStatusColorIcons = e.Item.FindControl("rcbInstitutionStatusColorIcons") as RadComboBox;
                    if (currentItem.IsNotNull())
                        Presenter.GetAllOrderFlags(currentItem.IOF_OrderFlagID);
                    else
                        Presenter.GetAllOrderFlags();
                    rcbInstitutionStatusColorIcons.DataSource = CurrentViewContext.lstOrderFlags;
                    rcbInstitutionStatusColorIcons.DataTextField = "OFL_Tooltip";
                    rcbInstitutionStatusColorIcons.DataValueField = "OFL_ID";
                    rcbInstitutionStatusColorIcons.DataBind();
                    //rcbInstitutionStatusColorIcons.Items.Insert(0, new RadComboBoxItem("--select--", "0"));

                    //RadComboBoxItem item = rcbInstitutionStatusColorIcons.Items.Where(x => x.Text == "--select--").FirstOrDefault();
                    //if (item != null)
                    //{
                    //    Image imbIcon = item.FindControl("imbIcon") as Image;
                    //    Label lbl = item.FindControl("lblTooltip") as Label;
                    //    imbIcon.Visible = false;
                    //    lbl.Text = item.Text = "--select--";
                    //    item.Value = "0";
                    //}
                    
                    if (currentItem.IsNotNull())
                    {
                        rcbInstitutionStatusColorIcons.SelectedValue = currentItem.lkpOrderFlag.OFL_ID.ToString();
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

        #region Methods

        private void BindTenant()
        {
            if (IsAdminLoggedIn == true)
            {
                dvOrderColorStatus.Visible = false;
                Presenter.GetTenants();
                ddlTenant.DataSource = CurrentViewContext.lstTenants;
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
                    ScreenName = "Manage Order Status Color"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Edit",
                    CustomActionLabel = "Edit",
                    ScreenName = "Manage Order Status Color"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Delete",
                    CustomActionLabel = "Delete",
                    ScreenName = "Manage Order Status Color"
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
                                    grdOrderColorStatus.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdOrderColorStatus.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdOrderColorStatus.MasterTableView.GetColumn("DeleteColumn").Display = false;
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