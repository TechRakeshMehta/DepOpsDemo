#region NameSpace
using CoreWeb.ComplianceOperations.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using Telerik.Web.UI;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using WebSiteUtils.SharedObjects;
using System.Collections;
using INTERSOFT.WEB.UI.WebControls;
using Business.RepoManagers;
#endregion
namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManageMultiTenantAssignementData : BaseUserControl, IManageMultiTenantAssignementDataView
    {
        #region Private Variables

        private ManageMultiTenantAssignementDataPresenter _presenter = new ManageMultiTenantAssignementDataPresenter();
        private CustomPagingArgsContract _gridCustomPaging = null;
        private MultiInstitutionAssignmentDataContract _verificationviewContract = null;
        private List<Int32> _selectedTenantIds = null;
        private Int32 _tenantid;
        private List<Int32> _fvdIds = null;
        private List<AssignQueueRecords> _lstAssignQueueRecords = null;

        #endregion

        #region Properties

        public ManageMultiTenantAssignementDataPresenter Presenter
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
        /// returns the object of type IItemsDataVerificationQueueView.
        /// </summary>
        public IManageMultiTenantAssignementDataView CurrentViewContext
        {
            get { return this; }
        }

        /// <summary>
        /// Return the list of tenants 
        /// </summary>
        public List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        public List<AssignQueueRecords> SelectedVerificationItemsNew
        {
            get
            {
                if (!ViewState["SelectedVerificationItemsNew"].IsNull())
                {
                    return ViewState["SelectedVerificationItemsNew"] as List<AssignQueueRecords>;
                }

                return new List<AssignQueueRecords>();
            }
            set
            {
                ViewState["SelectedVerificationItemsNew"] = value;
            }
        }

        /// <summary>
        /// Sets or gets the Tenant Id for the logged-in user.
        /// </summary>
        public Int32 TenantId
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
        /// Populates the dropdown with the list of active users in the organisation.
        /// </summary>
        public List<Entity.OrganizationUser> lstOrganizationUser
        {
            set
            {
                cmbVerSelectedUser.DataSource = value;
                cmbVerSelectedUser.DataBind();
                cmbVerSelectedUser.Items.Insert(0, new RadComboBoxItem("--Select--"));
            }
        }

        /// <summary>
        /// returns the Current Logged In User Id.
        /// </summary>
        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        /// <summary>
        /// Return the checked tenant from the combo box
        /// </summary>
        public List<Int32> SelectedTenantIds
        {
            get
            {
                _selectedTenantIds = new List<int>();
                foreach (RadComboBoxItem item in cmbTenantName.Items)
                {
                    if (item.Checked == true)
                        _selectedTenantIds.Add(Convert.ToInt32(item.Value));
                }
                return _selectedTenantIds;
            }
            set
            {
                _selectedTenantIds = value;
                foreach (RadComboBoxItem item in cmbTenantName.Items)
                {
                    if (_selectedTenantIds.Contains(Convert.ToInt32(item.Value)))
                        item.Checked = true;
                }
            }
        }

        public List<Int32> FVdIdList
        {
            get
            {
                if (ViewState["FvdIdList"].IsNotNull())
                {
                    return ViewState["FvdIdList"] as List<Int32>;
                }
                return new List<Int32>();
            }
            set
            {
                ViewState["FvdIdList"] = value;
            }
        }

        Boolean IManageMultiTenantAssignementDataView.IsMutipleTimesAssignmentAllowed
        {
            get
            {
                if (!ViewState["IsMutipleTimesAssignmentAllowed"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsMutipleTimesAssignmentAllowed"]);
                }
                return false;
            }
            set
            {
                ViewState["IsMutipleTimesAssignmentAllowed"] = value;
            }
        }  //UAT 2809

        String IManageMultiTenantAssignementDataView.ErrorMessage { get; set; }  //UAT 2809

        Boolean IManageMultiTenantAssignementDataView.IsUserAlreadyAssigned { get; set; } //UAT 2809

        #region Custom Paging

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdMultiTenantVerificationItemData.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdMultiTenantVerificationItemData.MasterTableView.CurrentPageIndex > 0)
                {
                    grdMultiTenantVerificationItemData.MasterTableView.CurrentPageIndex = value - 1;
                }
            }
        }

        ///// <summary>
        ///// PageSize</summary>
        ///// <value>
        ///// Gets the value for PageSize.</value>
        public Int32 PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                // return grdMultiTenantVerificationItemData.PageSize > 100 ? 100 : grdMultiTenantVerificationItemData.PageSize;
                return grdMultiTenantVerificationItemData.PageSize;
            }
            set
            {
                grdMultiTenantVerificationItemData.PageSize = value;
            }
        }

        ///// <summary>
        ///// VirtualPageCount</summary>
        ///// <value>
        ///// Sets the value for VirtualPageCount.</value>
        public Int32 VirtualPageCount
        {
            set
            {
                grdMultiTenantVerificationItemData.VirtualItemCount = value;
                grdMultiTenantVerificationItemData.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPaging
        {
            get
            {
                if (ViewState["_gridCustomPaging"] == null)
                {
                    ViewState["_gridCustomPaging"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["_gridCustomPaging"];
            }
            set
            {
                ViewState["_gridCustomPaging"] = value;
                VirtualPageCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        /// <summary>
        /// View Contract
        /// </summary>
        public MultiInstitutionAssignmentDataContract VerificationViewContract
        {
            get
            {
                if (_verificationviewContract.IsNull())
                {
                    _verificationviewContract = new MultiInstitutionAssignmentDataContract();
                }
                return _verificationviewContract;
            }
        }

        public List<MultiInstitutionAssignmentDataContract> lstMultiInstitutionAssignmentData
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Events

        #region Page Load Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                grdMultiTenantVerificationItemData.Visible = false;
                lblPageHdr.Visible = false;
                pnlVerification.Visible = false;
                BindTenant();
                Presenter.OnViewInitialized();
                hdnEditFVDId.Value = null;
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ResetSelectedItems();", true);
            }
            Presenter.OnViewLoaded();
        }

        #endregion

        #region Grid Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                String title = "Multiple Institutions Assignment Queue";
                base.Title = title;
                base.SetPageTitle(title);
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
        /// Retrieves a list of Multi Institution Assignment Data.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdMultiTenantVerificationItemData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                SetVerGridFilters();
                lstMultiInstitutionAssignmentData = new List<MultiInstitutionAssignmentDataContract>();
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                GridCustomPaging.FilterColumns = VerificationViewContract.FilterColumns;
                GridCustomPaging.FilterOperators = VerificationViewContract.FilterOperators;
                GridCustomPaging.FilterValues = VerificationViewContract.FilterValues;
                GridCustomPaging.FilterTypes = VerificationViewContract.FilterTypes;
                GridCustomPaging.DefaultSortExpression = "SubmissionDate";
                Presenter.GetMultiInstitutionAssignmentData();
                grdMultiTenantVerificationItemData.DataSource = lstMultiInstitutionAssignmentData;

                if (lstMultiInstitutionAssignmentData.IsNotNull() && lstMultiInstitutionAssignmentData.Count > 0)
                {
                    pnlVerShowUsers.Visible = true;
                    Presenter.GetUserListForSelectedTenant();
                }
                else
                {
                    pnlVerShowUsers.Visible = false;
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
        /// Grid sort expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdMultiTenantVerificationItemData_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    GridCustomPaging.SortExpression = e.SortExpression;
                    GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    GridCustomPaging.SortExpression = String.Empty;
                    GridCustomPaging.SortDirectionDescending = false;
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

        protected void grdMultiTenantVerificationItemData_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    Boolean IsDirty = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsDirty"]);

                    if (IsDirty.IsNotNull())
                    {
                        if (IsDirty.Equals(true))
                        {
                            CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                            checkBox.Enabled = false;
                        }
                    }

                    String[] checkedFVDIDs = null;
                    if (hdnEditFVDId.Value != null && !hdnEditFVDId.Value.ToString().IsNullOrEmpty())
                    {
                        checkedFVDIDs = hdnEditFVDId.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        if (checkedFVDIDs.IsNotNull())
                        {
                            String orderID = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FVDId"]);
                            if (!String.IsNullOrEmpty(orderID))
                            {
                                if (checkedFVDIDs.Any(cond => cond == orderID))
                                {
                                    CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                                    checkBox.Checked = true;
                                }
                            }
                        }
                    }
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    //UAT 2528
                    Boolean IsAdminLoggedIn = (SecurityManager.DefaultTenantID == TenantId);
                    if (IsAdminLoggedIn)
                    {
                        Boolean IsUiRulesViolate = Convert.ToBoolean(dataItem["IsUiRulesViolate"].Text);
                        if (IsUiRulesViolate)
                        {
                            dataItem.Attributes.Add("Style", "background-color:#ff6666 !important");
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

        protected void grdMultiTenantVerificationItemData_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                SetVerGridFilters();
                Pair filter = (Pair)e.CommandArgument;

                Int32 filterIndex = CurrentViewContext.VerificationViewContract.FilterColumns.IndexOf(filter.Second.ToString());
                if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString())
                {
                    String filteringType = grdMultiTenantVerificationItemData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName;
                    String filterValue = grdMultiTenantVerificationItemData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;

                    if (filterIndex != -1)
                    {
                        CurrentViewContext.VerificationViewContract.FilterTypes[filterIndex] = filteringType;
                        CurrentViewContext.VerificationViewContract.FilterOperators[filterIndex] = filter.First.ToString();
                        CurrentViewContext.VerificationViewContract.FilterValues[filterIndex] = filterValue;
                    }
                    else
                    {
                        CurrentViewContext.VerificationViewContract.FilterTypes.Add(filteringType);
                        CurrentViewContext.VerificationViewContract.FilterColumns.Add(filter.Second.ToString());
                        CurrentViewContext.VerificationViewContract.FilterOperators.Add(filter.First.ToString());
                        CurrentViewContext.VerificationViewContract.FilterValues.Add(filterValue);
                    }
                }
                else if (filterIndex != -1)
                {
                    CurrentViewContext.VerificationViewContract.FilterOperators.RemoveAt(filterIndex);
                    CurrentViewContext.VerificationViewContract.FilterValues.RemoveAt(filterIndex);
                    CurrentViewContext.VerificationViewContract.FilterColumns.RemoveAt(filterIndex);
                    CurrentViewContext.VerificationViewContract.FilterTypes.RemoveAt(filterIndex);
                }

                ViewState["FilterColumns"] = CurrentViewContext.VerificationViewContract.FilterColumns;
                ViewState["FilterOperators"] = CurrentViewContext.VerificationViewContract.FilterOperators;
                ViewState["FilterValues"] = CurrentViewContext.VerificationViewContract.FilterValues;
                ViewState["FilterTypes"] = CurrentViewContext.VerificationViewContract.FilterTypes;
            }
            else if (!(e.CommandName == RadGrid.RebindGridCommandName))
            {
                if (e.Item != null && e.Item.FindControl("cmbExportFormat") != null)
                {
                    WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                    if (cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel"))
                    {
                        grdMultiTenantVerificationItemData.MasterTableView.GetColumn("AdminNoteForExport").Display = true;
                        grdMultiTenantVerificationItemData.MasterTableView.GetColumn("AdminNote").Display = false;
                        grdMultiTenantVerificationItemData.MasterTableView.GetColumn("IsUiRulesViolate").Visible = true;
                    }
                    else
                    {
                        grdMultiTenantVerificationItemData.MasterTableView.GetColumn("AdminNoteForExport").Display = false;
                        grdMultiTenantVerificationItemData.MasterTableView.GetColumn("AdminNote").Display = true;
                        grdMultiTenantVerificationItemData.MasterTableView.GetColumn("AdminNote").Visible = true;
                    }
                }
                if (e.CommandName == RadGrid.CancelCommandName)
                {
                    grdMultiTenantVerificationItemData.MasterTableView.GetColumn("AdminNoteForExport").Display = false;
                    grdMultiTenantVerificationItemData.MasterTableView.GetColumn("AdminNote").Display = true;
                    grdMultiTenantVerificationItemData.MasterTableView.GetColumn("AdminNote").Visible = true;
                    grdMultiTenantVerificationItemData.MasterTableView.GetColumn("IsUiRulesViolate").Visible = true;
                }
            }
        }

        protected void grdMultiTenantVerificationItemData_Init(object sender, EventArgs e)
        {
            GridFilterMenu menu = grdMultiTenantVerificationItemData.FilterMenu;

            if (grdMultiTenantVerificationItemData.clearFilterMethod == null)
                grdMultiTenantVerificationItemData.clearFilterMethod = new WclGrid.ClearFilters(ClearViewStatesForFilter);

            int i = 0;
            while (i < menu.Items.Count)
            {
                if (menu.Items[i].Text == GridKnownFunction.Between.ToString() || menu.Items[i].Text == GridKnownFunction.NotBetween.ToString() ||
                    menu.Items[i].Text == GridKnownFunction.NotIsEmpty.ToString() || menu.Items[i].Text == GridKnownFunction.NotIsNull.ToString())
                {
                    menu.Items.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        #endregion

        #region Button Events

        /// <summary>
        /// This event is used to search the assignment data in multi institutions based on the selected tenants
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        protected void CmdBarSearch_SaveClick(object sender, EventArgs e)
        {
            grdMultiTenantVerificationItemData.Visible = true;
            lblPageHdr.Visible = true;
            pnlVerification.Visible = true;
            hdnEditFVDId.Value = null;
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ResetSelectedItems();", true);

            if (!Presenter.IsDefaultTenant)
            {
                btnAutomaticItemAssigning.Visible = false;
            }
            grdMultiTenantVerificationItemData.Rebind();
        }

        /// <summary>
        /// This event is used to reset the screen data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_SubmitClick(object sender, EventArgs e)
        {
            SelectedTenantIds = new List<int>();
            hdnEditFVDId.Value = null;
            cmbTenantName.ClearCheckedItems();
            grdMultiTenantVerificationItemData.MasterTableView.SortExpressions.Clear();
            grdMultiTenantVerificationItemData.MasterTableView.FilterExpression = "";
            foreach (GridColumn column in grdMultiTenantVerificationItemData.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            grdMultiTenantVerificationItemData.MasterTableView.FilterExpression = string.Empty;
            ClearViewStatesForFilter();
            grdMultiTenantVerificationItemData.Rebind();
        }

        /// <summary>
        /// This event is used to redirect the user on dashboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_CancelClick(object sender, EventArgs e)
        {
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
        }

        /// <summary>
        /// This event is used to assignment of verification record to the specific user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVerAssignUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbVerSelectedUser.SelectedValue != "")
                {
                    Int32 VerSelectedUserId = Convert.ToInt32(cmbVerSelectedUser.SelectedValue);
                    CurrentViewContext.IsMutipleTimesAssignmentAllowed = Convert.ToBoolean(hdnIsMutipleTimesAssignmentAllowed.Value); //UAT 2809
                    if (Presenter.AssignItemsToUser(VerSelectedUserId))
                    {
                        SelectedVerificationItemsNew = new List<AssignQueueRecords>();
                        FVdIdList = new List<Int32>();
                        grdMultiTenantVerificationItemData.Rebind();
                        cmbVerSelectedUser.SelectedIndex = 0;
                        ShowStatusMessage("sucs", AppConsts.MSG_ITEM_ASSIGNED_SUCCESS, true);
                        hdnEditFVDId.Value = null;
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ResetSelectedItems();", true);
                        grdMultiTenantVerificationItemData.Rebind();
                        hdnIsMutipleTimesAssignmentAllowed.Value = "false";  //UAT 2809
                    }
                    else
                    {
                        //UAT 2809
                        if (CurrentViewContext.IsUserAlreadyAssigned && !String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                        {
                            lblConfirmMessage.Text = CurrentViewContext.ErrorMessage;
                            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowConfirmationPopUp();", true);
                            return;
                        }
                        else
                        {
                            ShowStatusMessage("error", AppConsts.MSG_SELECT_ITEM, true);
                        }
                    }
                }
                else
                {
                    ShowStatusMessage("error", AppConsts.MSG_SELECT_USER, true);
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

        #region CheckBox Events

        protected void chkSelectItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                String QueueCode = "QCODE";
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                if (checkBox.IsNull())
                {
                    return;
                }
                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                AssignQueueRecords assignQueueRecords = new AssignQueueRecords();
                List<AssignQueueRecords> items = SelectedVerificationItemsNew;
                Int32 applicantComplianceItemID = (Int32)dataItem.GetDataKeyValue("ApplicantComplianceItemID");
                Int32 complianceItemId = (Int32)dataItem.GetDataKeyValue("ComplianceItemId");
                Int32 categoryId = (Int32)dataItem.GetDataKeyValue("CategoryId");
                String status = (String)dataItem.GetDataKeyValue("VerificationStatusCode");
                String itemName = (String)dataItem.GetDataKeyValue("ItemName");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectItem")).Checked;
                List<Int32> fvdIdList = FVdIdList;
                fvdIdList.Add((Int32)dataItem.GetDataKeyValue("FVDId"));
                FVdIdList = fvdIdList;

                assignQueueRecords.ApplicantComplienceItemId = applicantComplianceItemID;
                assignQueueRecords.ComplianceItemId = complianceItemId;
                assignQueueRecords.CategoryId = categoryId;
                assignQueueRecords.verificationStatusCode = status;
                assignQueueRecords.ItemName = itemName;
                assignQueueRecords.IsChecked = isChecked;
                assignQueueRecords.tenantID = (Int32)dataItem.GetDataKeyValue("TenantID");
                assignQueueRecords.IsDefaultThirdPartyTenant = false;
                assignQueueRecords.IsEsclationRecord = false;
                assignQueueRecords.QueueCode = QueueCode;

                if (items.Any(x => x.ApplicantComplienceItemId == applicantComplianceItemID))
                {
                    items.FirstOrDefault(x => x.ApplicantComplienceItemId == applicantComplianceItemID).IsChecked = isChecked;
                }
                else
                {
                    items.Add(assignQueueRecords);
                }

                SelectedVerificationItemsNew = items;
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

        /// <summary>
        /// Bind tenant combo box
        /// </summary>
        private void BindTenant()
        {
            Presenter.GetTenantList();
            cmbTenantName.DataSource = lstTenant;
            cmbTenantName.DataBind();
        }

        /// <summary>
        ///Show the message on the screen. 
        /// </summary>
        /// <param name="cssClass">cssClass</param>
        /// <param name="message">message</param>
        /// <param name="isVerificationMsg">isVerificationMsg</param>
        private void ShowStatusMessage(String cssClass, String message, Boolean isVerificationMsg)
        {
            lblVerError.Text = message;
            lblVerError.CssClass = cssClass;
            pnlVerError.Update();
        }

        private void SetVerGridFilters()
        {
            if (!ViewState["SortExpression"].IsNull())
            {
                CurrentViewContext.GridCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                CurrentViewContext.GridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
            }

            CurrentViewContext.VerificationViewContract.FilterColumns = ViewState["FilterColumns"] == null ? new List<String>() : (List<String>)(ViewState["FilterColumns"]);
            CurrentViewContext.VerificationViewContract.FilterOperators = ViewState["FilterOperators"] == null ? new List<String>() : (List<String>)(ViewState["FilterOperators"]);
            CurrentViewContext.VerificationViewContract.FilterValues = ViewState["FilterValues"] == null ? new ArrayList() : (ArrayList)(ViewState["FilterValues"]);
            CurrentViewContext.VerificationViewContract.FilterTypes = ViewState["FilterTypes"] == null ? new List<String>() : (List<String>)(ViewState["FilterTypes"]);
        }

        #endregion

        #region Public Methods

        public void ClearViewStatesForFilter()
        {
            ViewState["FilterColumns"] = null;
            ViewState["FilterOperators"] = null;
            ViewState["FilterValues"] = null;
            ViewState["FilterTypes"] = null;
        }
        #endregion

        protected void btnAutomaticItemAssigning_Click(object sender, EventArgs e)
        {
            try
            {
                var result = Presenter.GetAppConfiguration(AppConsts.Automatic_Items_Assign_To_Admin_In_Progress);
                if (result.Item1)
                {
                    Presenter.ActivateAutomaticAssignItemToUserProcessForAllTenants();
                    base.ShowSuccessMessage(AppConsts.Automatic_Items_Assign_To_Admin_Initiation_Process_Message);
                }
                else
                {
                    if (String.Equals(result.Item2, AppConsts.Automatic_Items_Assign_To_Admin_Error_Message))
                    {
                        base.ShowErrorMessage(AppConsts.Automatic_Items_Assign_To_Admin_Error_Message);
                    }
                    else
                    {
                        base.ShowInfoMessage(AppConsts.Automatic_Items_Assign_To_Admin_In_Progress_Message);
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


    }
}