using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.Utils.Enums;


namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManageUnArchivalRequests : BaseUserControl, IManageUnArchivalRequestsView
    {
        #region Private Variables

        private ManageUnArchivalRequestsPresenter _presenter = new ManageUnArchivalRequestsPresenter();
        private ManageOrderColorStatusContract _viewContract;

        private Int32 _tenantid;
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _selectedTenantId = AppConsts.NONE;
        #endregion

        #region Public Properties

        public IManageUnArchivalRequestsView CurrentViewContext
        {
            get { return this; }
        }

        public Boolean IsReset
        {
            get;
            set;
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


        public ManageUnArchivalRequestsPresenter Presenter
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
                if (Convert.ToInt32(ViewState["SelectedTenantId"]) == AppConsts.NONE)
                {
                    ViewState["SelectedTenantId"] = _tenantid;
                }
                return Convert.ToInt32(ViewState["SelectedTenantId"]);
            }
            set
            {
                ddlTenant.SelectedValue = Convert.ToString(value);
                ViewState["SelectedTenantId"] = value;
            }
        }

        //public Int32 SelectedTenantId
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(ddlTenant.SelectedValue))
        //            return 0;
        //        return Convert.ToInt32(ddlTenant.SelectedValue);
        //    }
        //    set
        //    {
        //        if (value > 0)
        //        {
        //            ddlTenant.SelectedValue = value.ToString();
        //        }
        //        else
        //        {
        //            ddlTenant.SelectedIndex = value;
        //        }
        //    }
        //}

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

        #region UAT-1062
        public String SelectedSubscriptionType
        {
            get
            {
                return Convert.ToString(ViewState["SelectedSubscriptionType"]);
            }
            set
            {
                ViewState["SelectedSubscriptionType"] = value;
            }
        }
        #endregion

        #region UAT-1683
        public String SelectedPackageType
        {
            get
            {
                return Convert.ToString(ViewState["SelectedPackageType"]);
            }
            set
            {
                ViewState["SelectedPackageType"] = value;
                if (value == ArchivePackageType.Screening.GetStringValue())
                {
                    grdUnArchivalRequests.MasterTableView.Columns.FindByUniqueName("ExpiryDate").Visible = false;
                }
                else
                {
                    grdUnArchivalRequests.MasterTableView.Columns.FindByUniqueName("ExpiryDate").Visible = true;
                }
            }
        }
        #endregion

        #endregion

        #region Private Properties


        ManageOrderColorStatusContract IManageUnArchivalRequestsView.ViewContract
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


        List<UnArchivalRequestDetails> IManageUnArchivalRequestsView.lstUnArchivalRequestDetails
        {
            get;
            set;
        }

        List<Tenant> IManageUnArchivalRequestsView.lstTenants
        {
            get;
            set;
        }

        Int32 IManageUnArchivalRequestsView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        String IManageUnArchivalRequestsView.ErrorMessage
        {
            get;
            set;
        }

        List<Int32> IManageUnArchivalRequestsView.SelectedUnArchivalRequestIDList
        {
            get
            {
                if (ViewState["SelectedUnArchivalRequestIDList"] != null)
                {
                    return (List<Int32>)ViewState["SelectedUnArchivalRequestIDList"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["SelectedUnArchivalRequestIDList"] = value;
            }
        }

        #endregion

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {

                base.Title = "Manage Archived Order(s)";
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
                grdUnArchivalRequests.Visible = false;
                btnUnarchive.Visible = false;
                Presenter.OnViewInitialized();
                BindTenant();
            }
            SetDefaultSelectedTenantId();
            base.SetPageTitle("Manage Archived Order(s)");
        }

        #endregion

        #region DropDown Events

        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!ddlTenant.SelectedValue.IsNullOrEmpty())
            {
                CurrentViewContext.SelectedTenantId = Convert.ToInt32(ddlTenant.SelectedValue);
            }
            if (!ddlTenant.SelectedValue.IsNullOrEmpty() && ddlTenant.SelectedValue != Convert.ToString(TenantId))
            {
                dvSubscriptionState.Visible = true;
                rbSubscriptionState.SelectedValue = SubscriptionType.ARCHIVED_SUBSCRIPTIONS.GetStringValue();
                //grdUnArchivalRequests.Rebind(); //UAT-4214
            }
            else
            {
                dvSubscriptionState.Visible = false;
                //grdUnArchivalRequests.Rebind(); //UAT-4214
            }
            //BindGrid();
            //rfvTenantName.Validate();
        }

        #endregion

        #region Select CheckBox Events

        protected void chkSelectItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                if (checkBox.IsNull())
                {
                    return;
                }
                if (CurrentViewContext.SelectedUnArchivalRequestIDList.IsNull())
                {
                    CurrentViewContext.SelectedUnArchivalRequestIDList = new List<Int32>();
                }

                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                Int32 unArchivalRequestId = (Int32)dataItem.GetDataKeyValue("UnArchiveRequestId");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectItem")).Checked;
                if (isChecked)
                {
                    if (!CurrentViewContext.SelectedUnArchivalRequestIDList.Exists(t => t == unArchivalRequestId))
                    {
                        CurrentViewContext.SelectedUnArchivalRequestIDList.Add(unArchivalRequestId);
                    }

                }
                else
                {
                    if (CurrentViewContext.SelectedUnArchivalRequestIDList != null && CurrentViewContext.SelectedUnArchivalRequestIDList.Exists(t => t == unArchivalRequestId))
                        CurrentViewContext.SelectedUnArchivalRequestIDList.Remove(unArchivalRequestId);
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

        protected void grdUnArchivalRequests_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                //START UAT-4214
                if (CurrentViewContext.IsReset && !CurrentViewContext.IsAdminLoggedIn)
                {
                    grdUnArchivalRequests.CurrentPageIndex = 0;
                    grdUnArchivalRequests.MasterTableView.CurrentPageIndex = 0;
                    //grdUnArchivalRequests.VirtualItemCount = 0;
                    CurrentViewContext.lstUnArchivalRequestDetails = new List<UnArchivalRequestDetails>();
                }
                else
                    Presenter.GetUnArchivalRequestData();
                //END UAT
                grdUnArchivalRequests.DataSource = CurrentViewContext.lstUnArchivalRequestDetails;
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
        protected void grdUnArchivalRequests_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    List<Int32> selectedItems = CurrentViewContext.SelectedUnArchivalRequestIDList;
                    if (selectedItems.IsNotNull())
                    {
                        String unArchiveRequestId = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UnArchiveRequestId"]);
                        if (!String.IsNullOrEmpty(unArchiveRequestId))
                        {
                            Int32 requestId = Convert.ToInt32(unArchiveRequestId);
                            if (selectedItems.Exists(cond => cond == requestId))
                            {
                                CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                                checkBox.Checked = true;
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

        #region Methods

        private void BindTenant()
        {
            Presenter.GetTenants();
            ddlTenant.DataSource = CurrentViewContext.lstTenants;
            ddlTenant.DataBind();
            divApproveRejectButtons.Visible = false;
            divUnarchiveButton.Visible = false;
            ddlTenant.SelectedValue = Convert.ToString(TenantId);

            if (IsAdminLoggedIn == true)
            {
                //divUnArchivalRequests.Visible = false;
                //Presenter.GetTenants();
                //ddlTenant.DataSource = CurrentViewContext.lstTenants;
                //ddlTenant.DataBind();
                //divApproveRejectButtons.Visible = false;
                //divUnarchiveButton.Visible = false;
                //divUnArchivalRequests.Visible = false;
                ddlTenant.Enabled = true;
            }
            else
            {
                //pnlTenant.Visible = false; ddlTenant.DataSource = CurrentViewContext.lstTenants;
                //Presenter.GetTenants();
                //ddlTenant.DataSource = CurrentViewContext.lstTenants;
                //ddlTenant.DataBind();
                //ddlTenant.SelectedValue = Convert.ToString(TenantId);
                ddlTenant.Enabled = false;
                dvSubscriptionState.Visible = true;
                SelectedSubscriptionType = SubscriptionType.ARCHIVED_SUBSCRIPTIONS.GetStringValue();
                BindGrid();
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

        #region Button Events

        /// <summary>
        /// Event fire on Hold button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRejection_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(ddlTenant.SelectedValue) != AppConsts.NONE)
                {
                    //AddToList();
                    if (CurrentViewContext.SelectedUnArchivalRequestIDList.IsNotNull() && CurrentViewContext.SelectedUnArchivalRequestIDList.Count > 0)
                    {
                        Boolean isSuccess = Presenter.RejectUnArchivalRequests();
                        if (isSuccess)
                        {
                            base.ShowSuccessMessage("Selected Un-Archival requests rejected successfully");
                            if (ViewState["SelectedUnArchivalRequestIDList"] != null)
                            {
                                ViewState["SelectedUnArchivalRequestIDList"] = null;
                            }
                            grdUnArchivalRequests.Rebind();
                        }
                        else
                        {
                            base.ShowErrorMessage("Some error occured.Please try again.");
                        }
                    }
                    else
                    {
                        base.ShowInfoMessage("Please select Un-Archival request(s) to reject.");
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

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(ddlTenant.SelectedValue) != AppConsts.NONE)
                {
                    //AddToList();
                    if (CurrentViewContext.SelectedUnArchivalRequestIDList.IsNotNull() && CurrentViewContext.SelectedUnArchivalRequestIDList.Count > 0)
                    {
                        Boolean isSuccess = Presenter.ApproveUnArchivalRequests();
                        if (isSuccess)
                        {
                            base.ShowSuccessMessage("Selected Un-Archival requests approved successfully");
                            if (ViewState["SelectedUnArchivalRequestIDList"] != null)
                            {
                                ViewState["SelectedUnArchivalRequestIDList"] = null;
                            }
                            grdUnArchivalRequests.Rebind();
                        }
                        else
                        {
                            base.ShowErrorMessage("Some error occured.Please try again.");
                        }
                    }
                    else
                    {
                        base.ShowInfoMessage("Please select Un-Archival request(s) to approve.");
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

        protected void rbSubscriptionState_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (!ddlTenant.SelectedValue.IsNullOrEmpty() && ddlTenant.SelectedValue != Convert.ToString(TenantId))
            //{

            //} 
            //BindGrid();
            //rfvTenantName.Validate();
            CurrentViewContext.SelectedSubscriptionType = rbSubscriptionState.SelectedValue;
        }
        //UAT-1683
        protected void rdbPackageSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentViewContext.SelectedPackageType = rblPackageSelection.SelectedValue;
            grdUnArchivalRequests.DataSource = new List<UnArchivalRequestDetails>();
            grdUnArchivalRequests.Rebind();
        }

        /// <summary>
        /// Event to unarchive selected archived subscriptions
        /// </summary>
        protected void btnUnarchive_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(ddlTenant.SelectedValue) != AppConsts.NONE)
                {
                    //AddToList();
                    if (CurrentViewContext.SelectedUnArchivalRequestIDList.IsNotNull() && CurrentViewContext.SelectedUnArchivalRequestIDList.Count > 0)
                    {
                        Boolean isSuccess = Presenter.ApproveUnArchivalRequests();
                        if (isSuccess)
                        {
                            if (SelectedPackageType == ArchivePackageType.Tracking.GetStringValue())
                            {
                                base.ShowSuccessMessage("Selected Subscription(s) un-Archived successfully.");
                            }
                            else if (SelectedPackageType == ArchivePackageType.Screening.GetStringValue())
                            {
                                base.ShowSuccessMessage(" Selected Background Order(s) un-Archived successfully.");
                            }
                            if (ViewState["SelectedUnArchivalRequestIDList"] != null)
                            {
                                ViewState["SelectedUnArchivalRequestIDList"] = null;
                            }
                            Presenter.SetQueueImaging(); //UAT-2422-Resync data to flat tables
                            grdUnArchivalRequests.Rebind();
                        }
                        else
                        {
                            base.ShowErrorMessage("Some error occured.Please try again.");
                        }
                    }
                    else
                    {
                        if (SelectedPackageType == ArchivePackageType.Tracking.GetStringValue())
                        {
                            base.ShowInfoMessage("Please select Subscription(s) to Un-Archive.");
                        }
                        else if (SelectedPackageType == ArchivePackageType.Screening.GetStringValue())
                        {
                            base.ShowInfoMessage("Please select Background Order(s) to Un-Archive.");
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

        public void BindGrid()
        {
            try
            {
                //if (!CurrentViewContext.SelectedSubscriptionType.IsNullOrEmpty())
                //{
                //    CurrentViewContext.SelectedSubscriptionType = rbSubscriptionState.SelectedValue;
                //}
                grdUnArchivalRequests.CurrentPageIndex = 0;
                grdUnArchivalRequests.MasterTableView.SortExpressions.Clear();
                grdUnArchivalRequests.MasterTableView.FilterExpression = null;

                foreach (GridColumn column in grdUnArchivalRequests.MasterTableView.OwnerGrid.Columns)
                {
                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = String.Empty;
                }
                if (!CurrentViewContext.SelectedSubscriptionType.IsNullOrEmpty() && CurrentViewContext.SelectedSubscriptionType == SubscriptionType.ARCHIVED_SUBSCRIPTIONS.GetStringValue())
                {
                    grdUnArchivalRequests.MasterTableView.Columns.FindByUniqueName("UnarchiveRequestDate").Display = false;
                    divUnarchiveButton.Visible = true;
                    divApproveRejectButtons.Visible = false;
                }
                else if (!CurrentViewContext.SelectedSubscriptionType.IsNullOrEmpty() && CurrentViewContext.SelectedSubscriptionType == SubscriptionType.UNARCHIVAL_REQUESTED_SUBSCRIPTIONS.GetStringValue())
                {
                    grdUnArchivalRequests.MasterTableView.Columns.FindByUniqueName("UnarchiveRequestDate").Display = true;
                    divApproveRejectButtons.Visible = true;
                    divUnarchiveButton.Visible = false;
                }
                //divUnArchivalRequests.Visible = true;
                grdUnArchivalRequests.Rebind();

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

        protected void ddlTenant_DataBound(object sender, EventArgs e)
        {
            ddlTenant.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        /// <summary>
        /// Reset Button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdBar_SubmitClick(object sender, EventArgs e)
        {
            if (IsAdminLoggedIn)
            {
                Presenter.GetTenants();
                CurrentViewContext.SelectedTenantId = 0;
                CurrentViewContext.SelectedSubscriptionType = null;
                dvSubscriptionState.Visible = false;
            }
            else
            {
                dvSubscriptionState.Visible = true;
                rbSubscriptionState.SelectedValue = SubscriptionType.ARCHIVED_SUBSCRIPTIONS.GetStringValue();
            }
            CurrentViewContext.IsReset = true;
            grdUnArchivalRequests.MasterTableView.Columns.FindByUniqueName("ExpiryDate").Visible = true;
            rblPackageSelection.SelectedValue = ArchivePackageType.Tracking.GetStringValue();
            //grdUnArchivalRequests.CurrentPageIndex = 0;
            //grdUnArchivalRequests.MasterTableView.SortExpressions.Clear();
            //grdUnArchivalRequests.MasterTableView.FilterExpression = null;
            //grdUnArchivalRequests.Rebind();
            BindGrid();
        }

        /// <summary>
        /// Search button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdBar_SaveClick(object sender, EventArgs e)
        {
            grdUnArchivalRequests.Visible = true;
            btnUnarchive.Visible = true;
            CurrentViewContext.IsReset = false;
            if (CurrentViewContext.SelectedSubscriptionType.IsNullOrEmpty())
            {
                CurrentViewContext.SelectedSubscriptionType = SubscriptionType.ARCHIVED_SUBSCRIPTIONS.GetStringValue();
            }
            else
            {
                CurrentViewContext.SelectedSubscriptionType = rbSubscriptionState.SelectedValue;

            }
            //UAT-1683: Add the Archive button and Manage Un-Archive to the Screening side.
            if (CurrentViewContext.SelectedPackageType.IsNullOrEmpty())
            {
                CurrentViewContext.SelectedPackageType = ArchivePackageType.Tracking.GetStringValue();
            }
            else
            {
                CurrentViewContext.SelectedPackageType = rblPackageSelection.SelectedValue;
            }
            BindGrid();
            //rfvTenantName.Validate();
        }

        /// <summary>
        /// Cancel Button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdBar_CancelClick(object sender, EventArgs e)
        {
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME));
        }
    }
}