using System;
using System.Collections.Generic;
using System.Linq;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{
    public partial class OrderClientStatus : BaseUserControl, IOrderClientStatusView
    {
        #region VARIABLES
        #region PUBLIC VARIABLES

        #endregion

        #region PRIVATE VARIABLES

        private OrderClientStatusPresenter _presenter = new OrderClientStatusPresenter();
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _tenantid;
        private OrderClientStatusContract _viewContract;


        #endregion
        #endregion

        #region PROPERTIES

        #region PUBLIC PROPERTIES

        public IOrderClientStatusView CurrentViewContext
        {
            get { return this; }
        }

        public List<Entity.ClientEntity.BkgOrderClientStatu> OrderClientStatusList
        {
            get;
            set;
        }

        public List<OrderClientStatusContract> OrderClientStatusContractList
        {
            get;
            set;
        }

        public OrderClientStatusContract ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new OrderClientStatusContract();
                }

                return _viewContract;
            }
        }

        public int OrderClientStatusId
        {
            get;
            set;
        }

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

        public Int32 SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedTenantId"]);
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        public List<Entity.ClientEntity.Tenant> TenantsList
        {
            get;
            set;
        }

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

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
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

        public String OrderClientStatusTypeName
        {
            get;
            set;
        }

        public Boolean IsOrderClientStatusSaved
        {
            get;
            set;
        }

        #endregion

        #region PRIVATE PROPERTIES

        #endregion

        #endregion

        #region PRESENTER
        public OrderClientStatusPresenter Presenter
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
        #endregion

        #region EVENTS

        #region PAGE EVENTS
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Order Client Status";
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
                ApplyActionLevelPermission(ActionCollection, "Order Client Status");
                grdOrderClientStatus.Visible = false;
                if (TenantId != DefaultTenantId)
                {
                    SelectedTenantId = TenantId;
                    grdOrderClientStatus.Visible = true;
                }
                BindTenantDropdown();
            }
            base.SetPageTitle("Order Client Status");
        }

        #endregion

        #region DROPDOWN EVENTS
        protected void ddlTenant_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                {
                    grdOrderClientStatus.Visible = true;
                    grdOrderClientStatus.Rebind();
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

        #region GRID EVENTS

        protected void grdOrderClientStatus_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                String errorMessage;
                OrderClientStatusId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BOCS_ID"]);
                errorMessage = Presenter.DeleteOrderClientStatus();
                if (errorMessage.Equals(String.Empty))
                {
                    base.ShowSuccessMessage("Order Client Status deleted successfully.");
                }
                else
                {
                    base.ShowErrorInfoMessage(errorMessage);
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

        protected void grdOrderClientStatus_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                Boolean isInserted = true;
                OrderClientStatusTypeName = (e.Item.FindControl("txtClientStatusName") as WclTextBox).Text.Trim();
                Presenter.SaveOrderClientStatus();
                if (isInserted)
                {
                    base.ShowSuccessMessage("Order Client Status Added successfully.");
                }
                else
                {
                    base.ShowErrorInfoMessage("Order Client Status Added failed.");
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

        protected void grdOrderClientStatus_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                Boolean isUpdated = true;
                OrderClientStatusId = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BOCS_ID"]);
                OrderClientStatusTypeName = (e.Item.FindControl("txtClientStatusName") as WclTextBox).Text.Trim();
                Presenter.UpdateOrderClientStatus();
                if (isUpdated)
                {
                    base.ShowSuccessMessage("Order Client Status Updated successfully.");
                }
                else
                {
                    base.ShowErrorInfoMessage("Order Client Status Updation failed.");
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

        protected void grdOrderClientStatus_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                SelectedTenantId = ddlTenant.SelectedValue != String.Empty ? Convert.ToInt32(ddlTenant.SelectedValue) : TenantId;
                Presenter.FetchOrderClientStatus();
                grdOrderClientStatus.DataSource = OrderClientStatusList;
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

        protected void grdOrderClientStatus_RowDrop(object sender, Telerik.Web.UI.GridDragDropEventArgs e)
        {
            try
            {
                SelectedTenantId = ddlTenant.SelectedValue != String.Empty ? Convert.ToInt32(ddlTenant.SelectedValue) : TenantId;
                if (OrderClientStatusList.IsNull())
                    Presenter.FetchOrderClientStatus();
                if (String.IsNullOrEmpty(e.HtmlElement))
                {

                    if (e.DestDataItem != null && e.DestDataItem.OwnerGridID == grdOrderClientStatus.ClientID)
                    {
                        //reorder items in grid
                        Int32 destAttributeGroupMappingId = Convert.ToInt32(e.DestDataItem.GetDataKeyValue("BOCS_ID"));
                        BkgOrderClientStatu selectedOrderClientStatus = OrderClientStatusList.Where(cond => cond.BOCS_ID == destAttributeGroupMappingId).FirstOrDefault();
                        Int32? destinationIndex = selectedOrderClientStatus.BOCS_DisplayOrder;
                        IList<BkgOrderClientStatu> statusToMove = new List<Entity.ClientEntity.BkgOrderClientStatu>();
                        IList<BkgOrderClientStatu> shiftedStatus = null;

                        foreach (GridDataItem draggedItem in e.DraggedItems)
                        {
                            Int32 draggedOrderStatusId = Convert.ToInt32(draggedItem.GetDataKeyValue("BOCS_ID"));
                            BkgOrderClientStatu tmpOrderClientStatusContractList = OrderClientStatusList.Where(cond => cond.BOCS_ID == draggedOrderStatusId).FirstOrDefault();
                            if (tmpOrderClientStatusContractList != null)
                                statusToMove.Add(tmpOrderClientStatusContractList);
                        }
                        BkgOrderClientStatu lastAttributeToMove = statusToMove.OrderByDescending(i => i.BOCS_DisplayOrder).FirstOrDefault();
                        Int32? sourceIndex = lastAttributeToMove.BOCS_DisplayOrder;
                        if (sourceIndex > destinationIndex)
                        {
                            shiftedStatus = OrderClientStatusList.Where(obj => obj.BOCS_DisplayOrder >= destinationIndex && obj.BOCS_DisplayOrder < sourceIndex).ToList();
                            if (shiftedStatus.IsNotNull())
                                statusToMove.AddRange(shiftedStatus);
                        }
                        else if (sourceIndex < destinationIndex)
                        {
                            shiftedStatus = OrderClientStatusList.Where(obj => obj.BOCS_DisplayOrder <= destinationIndex && obj.BOCS_DisplayOrder > sourceIndex).ToList();
                            if (shiftedStatus.IsNotNull())
                                shiftedStatus.AddRange(statusToMove);
                            statusToMove = shiftedStatus;
                            destinationIndex = sourceIndex;
                        }

                        // Update Sequence
                        Presenter.UpdateClientStatusSequence(statusToMove, destinationIndex);
                        grdOrderClientStatus.Rebind();
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

        #region METHODS

        #region PRIVATE METHODS

        /// <summary>
        /// Method for binding Tenant Dropdown
        /// </summary>
        private void BindTenantDropdown()
        {
            if (IsAdminLoggedIn == true)
            {
                Presenter.GetTenants();
                ddlTenant.DataSource = TenantsList;
                ddlTenant.DataBind();
            }
            else
            {
                pnlTenant.Visible = false;
            }
        }
        #endregion

        #region PUBLIC METHODS

        #endregion

        #endregion

        #region ACTION PERMISSION
        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions();

        }

        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                Entity.ClientEntity.ClsFeatureAction objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Delete";
                objClsFeatureAction.CustomActionLabel = "Delete";
                objClsFeatureAction.ScreenName = "Order Client Status";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Edit";
                objClsFeatureAction.CustomActionLabel = "Edit";
                objClsFeatureAction.ScreenName = "Order Client Status";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Add";
                objClsFeatureAction.CustomActionLabel = "Add";
                objClsFeatureAction.ScreenName = "Order Client Status";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "ReOrder";
                objClsFeatureAction.CustomActionLabel = "ReOrder";
                objClsFeatureAction.ScreenName = "Order Client Status";
                actionCollection.Add(objClsFeatureAction);

                return actionCollection;
            }
        }

        private void ApplyPermisions()
        {
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
                                    grdOrderClientStatus.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdOrderClientStatus.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdOrderClientStatus.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "ReOrder")
                                {
                                    grdOrderClientStatus.ClientSettings.AllowRowsDragDrop = false;
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