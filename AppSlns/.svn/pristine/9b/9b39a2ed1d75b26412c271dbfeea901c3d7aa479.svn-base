using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Entity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.HtmlControls;
using CoreWeb.ComplianceOperations.Views;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Collections;
using System.Linq;
using CoreWeb.Shell;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class DataReconciliationQueue : BaseUserControl, IDataReconciliationQueueView
    {
        #region Constants

        /// <summary>
        /// Constant for FilterColumns
        /// </summary>
        public const String Filter_Columns = "FilterColumns";

        /// <summary>
        /// Constant for FilterOperators
        /// </summary>
        public const String Filter_Operators = "FilterOperators";

        /// <summary>
        /// Constant for FilterValues
        /// </summary>
        public const String Filter_Values = "FilterValues";

        /// <summary>
        ///  Constant for FilterTypes
        /// </summary>
        public const String Filter_Types = "FilterTypes";

        #endregion

        #region Private Variables

        private DataReconciliationQueuePresenter _presenter = new DataReconciliationQueuePresenter();
        private List<Int32> _selectedTenantIds = null;
        private CustomPagingArgsContract _gridCustomPaging = null;
        private DataReconciliationQueueContract _dataReconciliationQueueContract = null;
        private String _viewType;
        #endregion

        #region PAGE EVENTS

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                //Page title. 
                base.Title = "Data Reconciliation Queue";
                base.SetPageTitle("Data Reconciliation Queue");
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
                Presenter.OnViewLoaded();
                ddlTenant.DataSource = CurrentViewContext.lstTenants;
                ddlTenant.DataBind();

                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    GetSessionValues();
                }
            }
        }

        #endregion

        #region  GRID EVENTS
        /// <summary>
        /// Need Data Source of grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdReconciliationQueue_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                //SetVerGridFilters();
                SetFilterValues();
                CurrentViewContext.lstDataReconciliationQueueContract = new List<DataReconciliationQueueContract>();
                CurrentViewContext.GridCustomPaging.CurrentPageIndex = CurrentViewContext.CurrentPageIndex;
                CurrentViewContext.GridCustomPaging.PageSize = CurrentViewContext.PageSize;
                CurrentViewContext.GridCustomPaging.FilterColumns = dataReconciliationQueueContract.FilterColumns;
                CurrentViewContext.GridCustomPaging.FilterOperators = dataReconciliationQueueContract.FilterOperators;

                CurrentViewContext.GridCustomPaging.FilterValues = dataReconciliationQueueContract.FilterValues;
                CurrentViewContext.GridCustomPaging.FilterTypes = dataReconciliationQueueContract.FilterTypes;
                CurrentViewContext.GridCustomPaging.DefaultSortExpression = "SubmissionDate";
                Presenter.GetQueueData();

                grdReconciliationQueue.DataSource = CurrentViewContext.lstDataReconciliationQueueContract;

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
        /// filter and view detail functionality is implemented in Item_Command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdReconciliationQueue_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                SetVerGridFilters();
                Pair filter = (Pair)e.CommandArgument;

                Int32 filterIndex = CurrentViewContext.dataReconciliationQueueContract.FilterColumns.IndexOf(filter.Second.ToString());
                if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString())
                {
                    String filteringType = grdReconciliationQueue.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName;
                    String filterValue = grdReconciliationQueue.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;

                    if (filterIndex != -1)
                    {
                        CurrentViewContext.dataReconciliationQueueContract.FilterTypes[filterIndex] = filteringType;
                        CurrentViewContext.dataReconciliationQueueContract.FilterOperators[filterIndex] = filter.First.ToString();
                        CurrentViewContext.dataReconciliationQueueContract.FilterValues[filterIndex] = filterValue;
                    }
                    else
                    {
                        CurrentViewContext.dataReconciliationQueueContract.FilterTypes.Add(filteringType);
                        CurrentViewContext.dataReconciliationQueueContract.FilterColumns.Add(filter.Second.ToString());
                        CurrentViewContext.dataReconciliationQueueContract.FilterOperators.Add(filter.First.ToString());
                        CurrentViewContext.dataReconciliationQueueContract.FilterValues.Add(filterValue);
                    }
                }
                else if (filterIndex != -1)
                {
                    CurrentViewContext.dataReconciliationQueueContract.FilterOperators.RemoveAt(filterIndex);
                    CurrentViewContext.dataReconciliationQueueContract.FilterValues.RemoveAt(filterIndex);
                    CurrentViewContext.dataReconciliationQueueContract.FilterColumns.RemoveAt(filterIndex);
                    CurrentViewContext.dataReconciliationQueueContract.FilterTypes.RemoveAt(filterIndex);
                }

                ViewState[Filter_Columns] = CurrentViewContext.dataReconciliationQueueContract.FilterColumns;
                ViewState[Filter_Operators] = CurrentViewContext.dataReconciliationQueueContract.FilterOperators;
                ViewState[Filter_Values] = CurrentViewContext.dataReconciliationQueueContract.FilterValues;
                ViewState[Filter_Types] = CurrentViewContext.dataReconciliationQueueContract.FilterTypes;
            }
            else if (e.CommandName.Equals("ViewDetail"))
            {
                SetSessionValues();
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                String flatComplianceItemReconciliationDataID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FlatComplianceItemReconciliationDataID"].ToString();

                DataReconciliationQueueContract currentContract = CurrentViewContext.lstDataReconciliationQueueContract
                    .FirstOrDefault(cond => cond.FlatComplianceItemReconciliationDataID == Convert.ToInt32(flatComplianceItemReconciliationDataID));
                String itemDataId = Convert.ToString(currentContract.ApplicantComplianceItemId);
                String packageId = Convert.ToString(currentContract.PackageID);
                String categoryId = Convert.ToString(currentContract.CategoryID);
                String complianceCategoryId = Convert.ToString(currentContract.ApplicantComplianceCategoryId);
                String packageSubscriptionId = Convert.ToString(currentContract.PackageSubscriptionID);
                String applicantId = Convert.ToString(currentContract.ApplicantId);
                String tenantId = Convert.ToString(currentContract.TenantId);
                String institutionIds = String.Join(",", CurrentViewContext.selectedTenantIDs);
                queryString = new Dictionary<String, String>
                        { 
                        { "TenantId",tenantId},
                        { "Child", @"~\ComplianceOperations\UserControl\ReconciliationDetail.ascx"},
                        { "ItemDataId", itemDataId},
                        {"PackageId",packageId},
                        {"CategoryId",categoryId}, 
                        {"SelectedComplianceCategoryId",categoryId},
                        {"SelectedPackageSubscriptionId",packageSubscriptionId},
                        {"ApplicantId",applicantId},
                        {"ComplianceItemReconciliationDataID",flatComplianceItemReconciliationDataID},
                        //{"institutionIds",institutionIds}
                        };
                String url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }
            else if (e.CommandName.Equals("TriPanelNav"))
            {
                //Int32 selectedTenantId = 0;
                //if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                //{
                //    selectedTenantId = SelectedTenantId;
                //}
                //else
                //{
                //    selectedTenantId = TenantId;
                //}

                //if (ddlVerSelectedUser.SelectedValue.IsNullOrEmpty())
                //{
                //    VerSelectedUserId = 0;
                //}
                //else
                //{
                //    VerSelectedUserId = Convert.ToInt32(ddlVerSelectedUser.SelectedValue);
                //}
                SetSessionValues();
                Dictionary<String, String> queryString = new Dictionary<String, String>();

                String flatComplianceItemReconciliationDataID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FlatComplianceItemReconciliationDataID"].ToString();
                DataReconciliationQueueContract currentContract = CurrentViewContext.lstDataReconciliationQueueContract
                  .FirstOrDefault(cond => cond.FlatComplianceItemReconciliationDataID == Convert.ToInt32(flatComplianceItemReconciliationDataID));

                String workQueueType  = WorkQueueType.ReconciliationQueue.ToString();  
                String tenantId = Convert.ToString(currentContract.TenantId);
                String itemDataId = Convert.ToString(currentContract.ApplicantComplianceItemId);
                String packageId = Convert.ToString(currentContract.PackageID);
                String categoryId = Convert.ToString(currentContract.CategoryID);
                String applicantId = Convert.ToString(currentContract.ApplicantId);
                String packageSubscriptionId = Convert.ToString(currentContract.PackageSubscriptionID);

                #region UAT-4067
                Presenter.GetSelectedNodeIDBySubscriptionID(tenantId,  packageSubscriptionId);
                Presenter.GetAllowedFileExtensions(tenantId);
                #endregion

                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TenantId", tenantId },
                                                                    { "Child", ChildControls.VerificationDetailsNew},
                                                                    { "ItemDataId", itemDataId},
                                                                    {"IsException","false"},
                                                                    {"WorkQueueType",workQueueType},
                                                                    {"PackageId",packageId},
                                                                    {"CategoryId",categoryId},
                                                                    {"SelectedPackageSubscriptionId",packageSubscriptionId},
                                                                    {"SelectedComplianceCategoryId",categoryId},
                                                                    //{"IncludeIncompleteItems",chkShowIncompleteItems.Checked.ToString()},
                                                                    //{"ShowOnlyRushOrders",ShowOnlyRushOrders.ToString()},
                                                                    //{"UserGroupId",SelectedUserGroupId.ToString()},
                                                                    {"ApplicantId",applicantId},
                                                                    {"allowedFileExtensions", String.Join(",",CurrentViewContext.allowedFileExtensions)}, //UAT-4067
                                                                 };
                string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }
        }


        /// <summary>
        /// Grid sort expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdReconciliationQueue_SortCommand(object sender, GridSortCommandEventArgs e)
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

        protected void grdReconciliationQueue_Init(object sender, EventArgs e)
        {
            GridFilterMenu menu = grdReconciliationQueue.FilterMenu;

            if (grdReconciliationQueue.clearFilterMethod == null)
                grdReconciliationQueue.clearFilterMethod = new WclGrid.ClearFilters(ClearViewStatesForFilter);

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

        #region Properties

        #region Presenter

        public DataReconciliationQueuePresenter Presenter
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

        List<Tenant> IDataReconciliationQueueView.lstTenants
        {
            get;
            set;
        }


        List<Int32> IDataReconciliationQueueView.selectedTenantIDs
        {
            get
            {
                _selectedTenantIds = new List<int>();
                foreach (RadComboBoxItem item in ddlTenant.Items)
                {
                    if (item.Checked == true)
                        _selectedTenantIds.Add(Convert.ToInt32(item.Value));
                }
                return _selectedTenantIds;
            }
            set
            {
                _selectedTenantIds = value;
                foreach (RadComboBoxItem item in ddlTenant.Items)
                {
                    if (_selectedTenantIds.Contains(Convert.ToInt32(item.Value)))
                        item.Checked = true;
                }
            }
        }

        List<DataReconciliationQueueContract> IDataReconciliationQueueView.lstDataReconciliationQueueContract
        {
            get
            {
                if (!ViewState["lstDataReconciliationQueueContract"].IsNull())
                {
                    return ViewState["lstDataReconciliationQueueContract"] as List<DataReconciliationQueueContract>;
                }

                return new List<DataReconciliationQueueContract>();
            }
            set
            {
                ViewState["lstDataReconciliationQueueContract"] = value;
            }
        }

        IDataReconciliationQueueView CurrentViewContext
        {
            get
            {
                return this;
            }

        }

        #region UAT-4067
        public List<Int32> selectedNodeIDs
        {
            get
            {
                if (!ViewState["selectedNodeIDs"].IsNull())
                {
                    return (ViewState["selectedNodeIDs"]) as List<Int32>;
                }
                return new List<Int32>();
            }
            set { ViewState["selectedNodeIDs"] = value; }
        }

        public List<String> allowedFileExtensions
        {
            get
            {
                if (!ViewState["allowedFileExtensions"].IsNull())
                {
                    return (ViewState["allowedFileExtensions"]) as List<String>;
                }
                return new List<String>();
            }
            set { ViewState["allowedFileExtensions"] = value; }
        }

        #endregion-4067


        #region Custom Paging

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        Int32 IDataReconciliationQueueView.CurrentPageIndex
        {
            get
            {
                return grdReconciliationQueue.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdReconciliationQueue.MasterTableView.CurrentPageIndex > 0)
                {
                    grdReconciliationQueue.MasterTableView.CurrentPageIndex = value - 1;
                }
            }
        }

        ///// <summary>
        ///// PageSize</summary>
        ///// <value>
        ///// Gets the value for PageSize.</value>
        Int32 IDataReconciliationQueueView.PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                // return grdMultiTenantVerificationItemData.PageSize > 100 ? 100 : grdMultiTenantVerificationItemData.PageSize;
                return grdReconciliationQueue.PageSize;
            }
            set
            {
                grdReconciliationQueue.PageSize = value;
            }
        }

        ///// <summary>
        ///// VirtualPageCount</summary>
        ///// <value>
        ///// Sets the value for VirtualPageCount.</value>
        Int32 IDataReconciliationQueueView.VirtualRecordCount
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
                grdReconciliationQueue.VirtualItemCount = value;
                grdReconciliationQueue.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract IDataReconciliationQueueView.GridCustomPaging
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
                CurrentViewContext.VirtualRecordCount = value.VirtualPageCount;
                CurrentViewContext.PageSize = value.PageSize;
                CurrentViewContext.CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        public DataReconciliationQueueContract dataReconciliationQueueContract
        {
            get
            {
                if (_dataReconciliationQueueContract.IsNull())
                {
                    _dataReconciliationQueueContract = new DataReconciliationQueueContract();
                }
                return _dataReconciliationQueueContract;
            }
        }

        #endregion

        #endregion

        #region METHODS

        /// <summary>
        /// clearing filter data from view state.
        /// </summary>
        private void ClearViewStatesForFilter()
        {
            ViewState[Filter_Columns] = null;
            ViewState[Filter_Operators] = null;
            ViewState[Filter_Types] = null;
            ViewState[Filter_Values] = null;
        }

        /// <summary>
        /// setting filter values in contract for filtering.
        /// </summary>
        private void SetVerGridFilters()
        {
            if (!ViewState["SortExpression"].IsNull())
            {
                CurrentViewContext.GridCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                CurrentViewContext.GridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
            }

            CurrentViewContext.dataReconciliationQueueContract.FilterColumns = ViewState[Filter_Columns] == null ? new List<String>() : (List<String>)(ViewState[Filter_Columns]);
            CurrentViewContext.dataReconciliationQueueContract.FilterOperators = ViewState[Filter_Operators] == null ? new List<String>() : (List<String>)(ViewState[Filter_Operators]);
            CurrentViewContext.dataReconciliationQueueContract.FilterValues = ViewState[Filter_Values] == null ? new ArrayList() : (ArrayList)(ViewState[Filter_Values]);
            CurrentViewContext.dataReconciliationQueueContract.FilterTypes = ViewState[Filter_Types] == null ? new List<String>() : (List<String>)(ViewState[Filter_Types]);
        }

        /// <summary>
        /// In this we are settinig values in session when we are redirecting to detail page.
        /// </summary>
        private void SetSessionValues()
        {
            DataReconciliationQueueContract dataReconciliationQueueFilters = new DataReconciliationQueueContract();
            if (CurrentViewContext.selectedTenantIDs.Count > 0)
            {
                dataReconciliationQueueFilters.selectedTenantIds = _selectedTenantIds;
            }

            dataReconciliationQueueFilters.FilterColumns = ViewState[Filter_Columns].IsNullOrEmpty() ? null : (List<String>)ViewState[Filter_Columns];
            dataReconciliationQueueFilters.FilterOperators = ViewState[Filter_Operators].IsNullOrEmpty() ? null : (List<String>)ViewState[Filter_Operators];
            dataReconciliationQueueFilters.FilterValues = ViewState[Filter_Values].IsNullOrEmpty() ? null : (ArrayList)ViewState[Filter_Values];
            dataReconciliationQueueFilters.FilterTypes = ViewState[Filter_Types].IsNullOrEmpty() ? null : (List<String>)ViewState[Filter_Types];



            SysXWebSiteUtils.SessionService.SetCustomData("Data_Reconciliation_Queue", dataReconciliationQueueFilters);
        }

        /// <summary>
        /// In this we are gettinig values from session when we are again redirecting back to queue page.
        /// </summary>
        private void GetSessionValues()
        {
            DataReconciliationQueueContract dataReconciliationQueueFilters = (DataReconciliationQueueContract)SysXWebSiteUtils.SessionService.GetCustomData("Data_Reconciliation_Queue");
            if (dataReconciliationQueueFilters.IsNotNull())
            {
                if (!dataReconciliationQueueFilters.selectedTenantIds.IsNullOrEmpty())
                {
                    foreach (RadComboBoxItem item in ddlTenant.Items)
                    {
                        if (dataReconciliationQueueFilters.selectedTenantIds.Contains(Convert.ToInt32(item.Value)))
                            item.Checked = true;
                    }
                }

                ViewState[Filter_Columns] = CurrentViewContext.dataReconciliationQueueContract.FilterColumns = dataReconciliationQueueFilters.FilterColumns;
                ViewState[Filter_Operators] = CurrentViewContext.dataReconciliationQueueContract.FilterOperators = dataReconciliationQueueFilters.FilterOperators;
                ViewState[Filter_Values] = CurrentViewContext.dataReconciliationQueueContract.FilterValues = dataReconciliationQueueFilters.FilterValues;
                ViewState[Filter_Types] = CurrentViewContext.dataReconciliationQueueContract.FilterTypes = dataReconciliationQueueFilters.FilterTypes;
                SetFilterValues();


                SysXWebSiteUtils.SessionService.SetCustomData("Data_Reconciliation_Queue", null);
            }
        }

        /// <summary>
        /// setting back filter values when redirected back to this page from its detail page.
        /// </summary>
        private void SetFilterValues()
        {
            if (!CurrentViewContext.dataReconciliationQueueContract.FilterColumns.IsNullOrEmpty() && CurrentViewContext.dataReconciliationQueueContract.FilterColumns.Count > 0)
            {
                CurrentViewContext.dataReconciliationQueueContract.FilterColumns.ForEach(x =>
                    grdReconciliationQueue.Columns.FindByUniqueName(x).CurrentFilterValue = CurrentViewContext.dataReconciliationQueueContract.FilterValues[CurrentViewContext.dataReconciliationQueueContract.FilterColumns.IndexOf(x)].ToString()
                    );
            }
        }

        #endregion

        #region  BUTTON

        /// <summary>
        /// This event is used to search the queue data in data reconciliation queue based on the selected tenants
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs</param>
        protected void CmdBarSearch_SaveClick(object sender, EventArgs e)
        {
            grdReconciliationQueue.Rebind();
        }

        /// <summary>
        /// This event is used to reset the screen data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_SubmitClick(object sender, EventArgs e)
        {
            CurrentViewContext.selectedTenantIDs = new List<int>();
            ddlTenant.ClearCheckedItems();
            grdReconciliationQueue.MasterTableView.SortExpressions.Clear();
            grdReconciliationQueue.MasterTableView.FilterExpression = "";
            foreach (GridColumn column in grdReconciliationQueue.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            grdReconciliationQueue.MasterTableView.FilterExpression = string.Empty;
            ClearViewStatesForFilter();
            grdReconciliationQueue.Rebind();
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

        #endregion

    }
}