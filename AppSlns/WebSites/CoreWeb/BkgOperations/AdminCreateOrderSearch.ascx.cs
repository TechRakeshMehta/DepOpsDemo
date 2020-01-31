using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.BkgOperations.Views
{
    public partial class AdminCreateOrderSearch : BaseUserControl, IAdminCreateOrderSearchView
    {
        #region Variables

        #region Private Variables

        private AdminCreateOrderSearchPresenter _presenter = new AdminCreateOrderSearchPresenter();
        private String _viewType;
        private Int32 _tenantid;
        private DateTime _minCalenderDate = Convert.ToDateTime("01/01/1900");
        private CustomPagingArgsContract _gridCustomPaging = null;
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        private AdminCreateOrderSearchPresenter Presenter
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

        private IAdminCreateOrderSearchView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Boolean IAdminCreateOrderSearchView.IsReset
        {
            get;
            set;
        }

        Int32 IAdminCreateOrderSearchView.TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
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

        Int32 IAdminCreateOrderSearchView.SelectedTenantId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlTenantName.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlTenantName.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlTenantName.SelectedValue = Convert.ToString(value);
                }
                else
                {
                    ddlTenantName.SelectedIndex = value;
                }
            }
        }

        List<Tenant> IAdminCreateOrderSearchView.lstTenant
        {
            get
            {
                if (!ViewState["lstTenant"].IsNull())
                {
                    return (ViewState["lstTenant"]) as List<Tenant>;
                }
                return new List<Tenant>();
            }
            set
            {
                ViewState["lstTenant"] = value;
            }
        }

        String IAdminCreateOrderSearchView.TargetHierarchyNodeIds
        {
            get
            {
                if (!String.IsNullOrEmpty(hdnDepartmntPrgrmMppng.Value))
                {
                    return Convert.ToString(hdnDepartmntPrgrmMppng.Value);
                }
                return String.Empty;
            }
            set
            {
                hdnDepartmntPrgrmMppng.Value = value;
            }
        }

        List<AdminCreateOrderContract> IAdminCreateOrderSearchView.lstAdminOrders
        {
            get;
            set;
        }

        AdminOrderSearchContract IAdminCreateOrderSearchView.searchContract
        {
            get;
            set;
        }

        String IAdminCreateOrderSearchView.FirstName
        {
            get
            {
                return txtFirstName.Text.Trim();
            }
            set
            {
                txtFirstName.Text = value;
            }
        }

        String IAdminCreateOrderSearchView.LastName
        {
            get
            {
                return txtLastName.Text.Trim();
            }
            set
            {
                txtLastName.Text = value;
            }
        }

        String IAdminCreateOrderSearchView.SSN
        {
            get
            {
                return txtSSN.Text.Trim();
            }
            set
            {
                txtSSN.Text = value;
            }
        }

        String IAdminCreateOrderSearchView.OrderNumber
        {
            get
            {
                return txtOrderNumber.Text.Trim();
            }
            set
            {
                txtOrderNumber.Text = value;
            }
        }

        DateTime IAdminCreateOrderSearchView.DOB
        {
            get
            {
                return (dpDob.SelectedDate.IsNotNull() ? Convert.ToDateTime(dpDob.SelectedDate) : _minCalenderDate);
            }
            set
            {
                dpDob.SelectedDate = value;
            }
        }

        String IAdminCreateOrderSearchView.ReadyToTransmit
        {
            get
            {
                return (rdbtnTransmitType.SelectedValue);
            }
            set
            {
                rdbtnTransmitType.SelectedValue = value;
            }
        }

        Boolean IAdminCreateOrderSearchView.IsAdminLoggedIn
        {
            get;
            set;
        }

        Int32 IAdminCreateOrderSearchView.OrderID
        {
            get
            {
                if (!ViewState["OrderID"].IsNull())
                {
                    return (Int32)ViewState["OrderID"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["OrderID"] = value;
            }
        }

        List<Int32> IAdminCreateOrderSearchView.SelectedOrderIds
        {
            get
            {
                if (!ViewState["SelectedOrders"].IsNull())
                {
                    return ViewState["SelectedOrders"] as List<Int32>;
                }

                return new List<Int32>();
            }
            set
            {
                ViewState["SelectedOrders"] = value;
            }
        }

        Int32 IAdminCreateOrderSearchView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Dictionary<Int32, Boolean> IAdminCreateOrderSearchView.DicOfSelectedOrders
        {
            get
            {
                if (!ViewState["DicOfSelectedOrder"].IsNull())
                {
                    return (Dictionary<Int32, Boolean>)ViewState["DicOfSelectedOrder"];
                }
                return new Dictionary<Int32, Boolean>();
            }
            set
            {
                ViewState["DicOfSelectedOrder"] = value;
            }
        }

        #region Custom Paging

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdAdminOrderDetails.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdAdminOrderDetails.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>

        public Int32 PageSize
        {
            get
            {
                return grdAdminOrderDetails.MasterTableView.PageSize;
            }
            set
            {
                grdAdminOrderDetails.MasterTableView.PageSize = value;
                grdAdminOrderDetails.PageSize = value;
            }
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>

        public CustomPagingArgsContract GridCustomPaging
        {
            get
            {
                if (ViewState["GridCustomPaging"] == null)
                {
                    ViewState["GridCustomPaging"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["GridCustomPaging"];
            }
            set
            {
                ViewState["GridCustomPaging"] = value;
                VirtualRecordCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        /// <summary>
        /// Virtual Page Count</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        public Int32 VirtualRecordCount
        {
            get
            {
                if (ViewState["ItemCount"] != null)
                    return Convert.ToInt32(ViewState["ItemCount"]);
                else
                    return 0;
            }
            set
            {
                ViewState["ItemCount"] = value;
                grdAdminOrderDetails.VirtualItemCount = value;
                grdAdminOrderDetails.MasterTableView.VirtualItemCount = value;
            }
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
                base.OnInit(e);
                base.Title = "Admin Order Queue";
                base.SetPageTitle("Admin Order Queue");
                lblOrderSearch.Text = base.Title;
                Presenter.IsAdminLoggedIn();
                grdAdminOrderDetails.MasterTableView.GetColumn("ViewDetail").Display = false;
                if (CurrentViewContext.IsAdminLoggedIn)
                    grdAdminOrderDetails.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                else
                    grdAdminOrderDetails.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = true;
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
                    grdAdminOrderDetails.Visible = false;
                    CmdBarTransmit.Visible = false;
                    Presenter.OnViewInitialized();
                    BindTenant();
                    CaptureQuerystringParameters();
                    GetSessionValues();
                }
                hdnTenantId.Value = CurrentViewContext.SelectedTenantId.ToString();
                lblinstituteHierarchy.Text = hdnHierarchyLabel.Value.HtmlEncode();
                if (CurrentViewContext.SelectedTenantId > AppConsts.NONE)
                {
                    grdAdminOrderDetails.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = true;
                }
                else
                {
                    grdAdminOrderDetails.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
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

        protected void grdAdminOrderDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                CurrentViewContext.GridCustomPaging = CurrentViewContext.GridCustomPaging;
                //START UAT-4214
                if (CurrentViewContext.IsReset && !CurrentViewContext.IsAdminLoggedIn)
                {
                    grdAdminOrderDetails.CurrentPageIndex = 0;
                    grdAdminOrderDetails.MasterTableView.CurrentPageIndex = 0;
                    CurrentViewContext.VirtualRecordCount = 0;
                    CurrentViewContext.lstAdminOrders = new List<AdminCreateOrderContract>();
                }
                else
                Presenter.GetAdminCreateOrderSearchData();
                //END UAT
                grdAdminOrderDetails.DataSource = CurrentViewContext.lstAdminOrders;
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

        protected void grdAdminOrderDetails_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.InitInsertCommandName || e.CommandName == RadGrid.EditCommandName || e.CommandName == "ViewDetail")
                {
                    if (!Session[AppConsts.ADMIN_CREATE_ORDER_SEARCH].IsNullOrEmpty())
                    {
                        Session[AppConsts.ADMIN_CREATE_ORDER_SEARCH] = null;
                    }
                    SetSessionValues();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();

                    if (e.CommandName == RadGrid.EditCommandName || e.CommandName == "ViewDetail")
                    {
                        String SelectedTenantId = Convert.ToString(CurrentViewContext.SelectedTenantId);
                        String OrderID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderID"].ToString();
                        queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", ChildControls.AdminCreateOrderDetails},
                                                                    { "OrderID",OrderID},
                                                                    {"SelectedTenantId",SelectedTenantId}
                                                                  };
                    }

                    if (e.CommandName == RadGrid.InitInsertCommandName)
                    {
                        String SelectedTenantId = Convert.ToString(CurrentViewContext.SelectedTenantId);

                        queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", ChildControls.AdminCreateOrderDetails},
                                                                    {"SelectedTenantId",SelectedTenantId}
                                                                  };
                    }

                    string url = String.Format("~/BkgOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
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

        protected void grdAdminOrderDetails_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = String.Empty;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = false;
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

        protected void grdAdminOrderDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                Int32 orderId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderID"]);
                Dictionary<Int32, Boolean> selectedOrdersDic = CurrentViewContext.DicOfSelectedOrders;

                if (selectedOrdersDic.IsNotNull() && orderId != AppConsts.NONE && selectedOrdersDic.ContainsKey(orderId))
                {
                    CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectOrders"));
                    checkBox.Checked = true;
                }
            }
            if (e.Item.ItemType.Equals(GridItemType.Footer))
            {
                Int32 rowCount = grdAdminOrderDetails.Items.Count;
                if (rowCount > 0)
                {
                    Int32 checkCount = 0;
                    foreach (GridDataItem item in grdAdminOrderDetails.Items)
                    {
                        CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectOrders"));
                        if (checkBox.Checked)
                        {
                            checkCount++;
                        }
                    }
                    if (rowCount == checkCount)
                    {
                        GridHeaderItem item = grdAdminOrderDetails.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                        CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectAll"));
                        checkBox.Checked = true;
                    }
                }
            }
            if (!CurrentViewContext.lstAdminOrders.IsNullOrEmpty())
            {
                List<Int32> orderIds = CurrentViewContext.lstAdminOrders.Select(x => x.OrderID).ToList();
                List<Int32> orderIdsNotReadyToTransmit = CurrentViewContext.lstAdminOrders.Where(cond => cond.IsReadyToTransmit == "No").Select(x => x.OrderID).ToList();
                foreach (GridDataItem item in grdAdminOrderDetails.Items)
                {
                    if (orderIdsNotReadyToTransmit.Contains(Convert.ToInt32(item.GetDataKeyValue("OrderID"))))
                    {
                        CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectOrders"));
                        checkBox.Enabled = false;
                    }

                    if (orderIdsNotReadyToTransmit.Count == grdAdminOrderDetails.Items.Count)
                    {
                        GridHeaderItem Item = grdAdminOrderDetails.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                        CheckBox checkBox1 = ((CheckBox)Item.FindControl("chkSelectAll"));
                        checkBox1.Enabled = false;
                    }
                    else
                    {
                        GridHeaderItem Item = grdAdminOrderDetails.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                        CheckBox checkBox1 = ((CheckBox)Item.FindControl("chkSelectAll"));
                        checkBox1.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAdminOrderDetails_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                Int32 orderID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderID"]);
                if (Presenter.DeleteAdminOrderDetails(orderID))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Order deleted successfully.");
                    grdAdminOrderDetails.Rebind();
                }
                else
                {
                    e.Canceled = true;
                    base.ShowInfoMessage("Some error has occurred. Please try again.");
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

        #region Button Events

        protected void fsucAdminOrderCmdBar_SearchClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsReset = false;
                grdAdminOrderDetails.Visible = true;
                CmdBarTransmit.Visible = true;
                ResetGridFilters();
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

        protected void fsucAdminOrderCmdBar_ResetClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsReset = true;
                ResetTenant();
                ResetControls();
                ResetGridFilters();
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

        protected void fsucAdminOrderCmdBar_CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), false);
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

        protected void CmdBarTransmit_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                if (!CurrentViewContext.SelectedOrderIds.IsNullOrEmpty())
                {
                    String infoMessage = Presenter.CheckOrderAvailabilityForTrasmit();
                    if (!infoMessage.IsNullOrEmpty())
                    {
                        base.ShowInfoMessage(infoMessage);
                        return;
                    }
                    if (Presenter.AdminOrderIsReadyToTransmit())
                    {
                        if (Presenter.TransmmitAdminOrders())
                        {
                            grdAdminOrderDetails.Rebind();
                            base.ShowSuccessMessage("Selected Order(s) are transmitted successfully.");
                        }
                        else
                        {
                            base.ShowErrorInfoMessage("Some error occurred.Please try again.");
                        }
                    }

                    else
                    {
                        base.ShowInfoMessage("Please select the order(s) those status are ready to transmit.");
                    }
                }
                else
                {
                    base.ShowInfoMessage("Please select order(s) to transmit.");
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

        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlTenantName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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

        protected void ddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                ResetControls();
                //ResetGridFilters(); //UAT-3874
                BindControlsForSelectedTenant();
            }
            else
            {
                ResetTenant();
                ResetControls();
                //grdAdminOrderDetails.Rebind(); //UAT-3874
            }
        }
        #endregion

        #region CheckBox Events

        protected void chkSelectOrders_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                if (checkBox.IsNull())
                {
                    return;
                }
                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                if (CurrentViewContext.DicOfSelectedOrders.IsNull())
                {
                    CurrentViewContext.DicOfSelectedOrders = new Dictionary<Int32, Boolean>();
                }

                //List<Int32> selectedOrders = CurrentViewContext.SelectedOrderIds;
                Dictionary<Int32, Boolean> selectedOrdersDic = CurrentViewContext.DicOfSelectedOrders;
                Int32 orderID = (Int32)dataItem.GetDataKeyValue("OrderID");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectOrders")).Checked;

                if (!selectedOrdersDic.ContainsKey(orderID) && isChecked)
                {
                    selectedOrdersDic.Add(orderID, isChecked);
                }
                else if (selectedOrdersDic.ContainsKey(orderID) && !isChecked)
                {
                    selectedOrdersDic.Remove(orderID);
                }

                CurrentViewContext.DicOfSelectedOrders = selectedOrdersDic;
                CurrentViewContext.SelectedOrderIds = CurrentViewContext.DicOfSelectedOrders.Select(sel => sel.Key).ToList();
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

        #region Private Methods

        private void BindTenant()
        {
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();
            Presenter.IsAdminLoggedIn();
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                ddlTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantId = 0;
            }
            else
            {
                ddlTenantName.Enabled = false;
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
            }
        }
        private void BindControlsForSelectedTenant()
        {
            hdnDepartmntPrgrmMppng.Value = String.Empty;
            hdnHierarchyLabel.Value = String.Empty;
            lblinstituteHierarchy.Text = String.Empty;
        }

        private void ResetGridFilters()
        {
            grdAdminOrderDetails.MasterTableView.FilterExpression = null;
            CurrentViewContext.GridCustomPaging.SortExpression = null;
            grdAdminOrderDetails.MasterTableView.SortExpressions.Clear();
            grdAdminOrderDetails.CurrentPageIndex = 0;
            grdAdminOrderDetails.MasterTableView.CurrentPageIndex = 0;
            grdAdminOrderDetails.Rebind();
        }

        private void ResetTenant()
        {
            Presenter.IsAdminLoggedIn();
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                CurrentViewContext.SelectedTenantId = AppConsts.NONE;
                grdAdminOrderDetails.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
            }
            else
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
            }
        }

        private void ResetControls()
        {
            //ResetTenant();
            hdnDepartmntPrgrmMppng.Value = String.Empty;
            hdnHierarchyLabel.Value = String.Empty;
            lblinstituteHierarchy.Text = String.Empty;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            txtOrderNumber.Text = String.Empty;
            txtSSN.Text = String.Empty;
            rdbtnTransmitType.SelectedValue = "All";
            dpDob.SelectedDate = null;
        }

        private void SetSessionValues()
        {
            AdminOrderSearchContract searchDataContract = new AdminOrderSearchContract();
            searchDataContract.ClientId = CurrentViewContext.SelectedTenantId;
            searchDataContract.OrderNumber = CurrentViewContext.OrderNumber.IsNullOrEmpty() ? null : CurrentViewContext.OrderNumber;
            searchDataContract.OrderHierarchy = CurrentViewContext.TargetHierarchyNodeIds;
            searchDataContract.FirstName = CurrentViewContext.FirstName;
            searchDataContract.LastName = CurrentViewContext.LastName;
            searchDataContract.SSN = CurrentViewContext.SSN;
            searchDataContract.ReadyToTransmit = CurrentViewContext.ReadyToTransmit;
            searchDataContract.DOB = dpDob.SelectedDate;
            searchDataContract.gridCustomPaging = CurrentViewContext.GridCustomPaging;
            searchDataContract.HierarchyLable = lblinstituteHierarchy.Text.HtmlDecode();
            searchDataContract.DicSelectedOrders = CurrentViewContext.DicOfSelectedOrders;

            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ADMIN_CREATE_ORDER_SEARCH, searchDataContract);
        }

        private void GetSessionValues()
        {
            AdminOrderSearchContract searchDataContract = new AdminOrderSearchContract();
            CustomPagingArgsContract gridCustomPaging = new CustomPagingArgsContract();
            if (Session[AppConsts.ADMIN_CREATE_ORDER_SEARCH].IsNotNull())
            {
                searchDataContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.ADMIN_CREATE_ORDER_SEARCH) as AdminOrderSearchContract;
                CurrentViewContext.searchContract = searchDataContract;
                CurrentViewContext.SelectedTenantId = searchDataContract.ClientId;
                CurrentViewContext.GridCustomPaging = searchDataContract.gridCustomPaging;
                CurrentViewContext.FirstName = searchDataContract.FirstName;
                CurrentViewContext.LastName = searchDataContract.LastName;
                CurrentViewContext.OrderNumber = searchDataContract.OrderNumber;
                CurrentViewContext.SSN = searchDataContract.SSN;
                CurrentViewContext.ReadyToTransmit = searchDataContract.ReadyToTransmit;
                CurrentViewContext.TargetHierarchyNodeIds = searchDataContract.OrderHierarchy;
                hdnHierarchyLabel.Value = searchDataContract.HierarchyLable;
                if (!searchDataContract.DOB.IsNullOrEmpty())
                    CurrentViewContext.DOB = Convert.ToDateTime(searchDataContract.DOB);
                if (CurrentViewContext.SelectedTenantId > AppConsts.NONE)
                    grdAdminOrderDetails.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = true;

                CurrentViewContext.DicOfSelectedOrders = searchDataContract.DicSelectedOrders;
                CurrentViewContext.SelectedOrderIds = searchDataContract.DicSelectedOrders.Keys.ToList();
                CurrentViewContext.DicOfSelectedOrders = new Dictionary<Int32, Boolean>();

            }
            //grdAdminOrderDetails.Rebind(); //UAT-3874
            Session[AppConsts.ADMIN_CREATE_ORDER_SEARCH] = null;
        }

        private void CaptureQuerystringParameters()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNullOrEmpty())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (!args.ContainsKey("CancelClick") || args.ContainsKey("CancelClick").IsNullOrEmpty())
                {
                    Session[AppConsts.ADMIN_CREATE_ORDER_SEARCH] = null;
                }
                if (args.ContainsKey("ButtonClick") && Convert.ToString(args["ButtonClick"]) == "delete")
                {
                    base.ShowSuccessMessage("Order deleted successfully.");
                }
                if (args.ContainsKey("ButtonClick") && Convert.ToString(args["ButtonClick"]) == "transmit")
                {
                    base.ShowSuccessMessage("Order transmitted successfully.");
                }
            }
            else
            {
                Session[AppConsts.ADMIN_CREATE_ORDER_SEARCH] = null;
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #endregion

    }
}