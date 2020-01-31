#region Namespaces
#region System Defined
using System.Collections;
using System.Collections.Generic;
using System;
using System.Web.UI;
#endregion

#region Application Specific
using INTSOF.Utils;
using INTSOF.UI.Contract.Templates;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
#endregion

#endregion

namespace CoreWeb.Messaging.Views
{
    public partial class CommunicationArchiveSummary : BaseUserControl, ICommunicationArchiveSummaryView
    {
        #region Variables

        #region Private Variables

        private CommunicationArchiveSummaryPresenter _presenter = new CommunicationArchiveSummaryPresenter();
        private CustomPagingArgsContract _gridCustomPaging = null;
        private SearchCommunicationTemplateContract _searchContract = null;
        private String _viewType;
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        public List<CommunicationTemplateContract> CommunicationSummaryList
        {
            get;
            set;
        }

        public ICommunicationArchiveSummaryView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<Int32> SystemCommunicationDeliveryIds
        {
            get;
            set;
        }

        public Int32 CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public Int32 SubEventId
        {
            get;
            set;
        }

        public SearchCommunicationTemplateContract SearchContract
        {
            get
            {
                if (_searchContract.IsNullOrEmpty())
                {
                    GetSearchValues();
                }
                return _searchContract;
            }
            set
            {
                if (!_searchContract.IsNullOrEmpty())
                {
                    _searchContract = value;
                    SetSearchValues();
                }

            }
        }

        #region Presenter


        public CommunicationArchiveSummaryPresenter Presenter
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

        #endregion

        #region Custom Paging


        /// <summary>
        /// Current Page Index</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {                
                return grdCommunicationSummary.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdCommunicationSummary.MasterTableView.CurrentPageIndex > 0)
                {
                    grdCommunicationSummary.MasterTableView.CurrentPageIndex = value - 1;
                }
            }
        }

        /// <summary>
        /// Page Size</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        public Int32 PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                //return grdCommunicationSummary.PageSize > 100 ? 100 : grdCommunicationSummary.PageSize;
                return grdCommunicationSummary.PageSize;
            }
            set
            {
                grdCommunicationSummary.PageSize = value;
            }
        }

        /// <summary>
        /// Virtual Page Count</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        public Int32 VirtualPageCount
        {
            set
            {
                grdCommunicationSummary.VirtualItemCount = value;
                grdCommunicationSummary.MasterTableView.VirtualItemCount = value;
            }
        }

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
                grdCommunicationSummary.VirtualItemCount = value;
                grdCommunicationSummary.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// To get object of shared class of custom paging
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

        #endregion

        #endregion

        #endregion

        #region Page Load Event

        protected override void OnInit(EventArgs e)
        {
            try
            {
                // _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = "Communication Archive";
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
            }
            Presenter.OnViewLoaded();
            base.SetPageTitle("Communication Archive");
        }

        #endregion

        #region Events

        #region ButtonClick Events

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(hdfSelectedIds.Value))
                {
                    String[] _selectedSystemCommunicationDeliveryIds = hdfSelectedIds.Value.Split(',');
                    CurrentViewContext.SystemCommunicationDeliveryIds = _selectedSystemCommunicationDeliveryIds.ConvertIntoIntList();
                    Presenter.QueueReSendingEmails();
                    base.ShowSuccessMessage("Selected email(s) have been added to queue for sending.");
                    hdfSelectedIds.Value = String.Empty;
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

        /// <summary>
        /// Search Button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarButton_SaveClick(object sender, EventArgs e)
        {
            try
            {
                GetSearchValues();
                //It checks whether parameters are provided or not
                // if no paramenter is selected then searchcontract is null
                if (CurrentViewContext.SearchContract == null)
                {
                    base.ShowInfoMessage("Please provide atleast one or more search parameters.");
                    grdCommunicationSummary.Rebind();
                    return;
                }
                grdCommunicationSummary.Rebind();
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

        protected void CmdBarButton_CancelClick(object sender, EventArgs e)
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
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Reset button .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarButton_ClearClick(object sender, EventArgs e)
        {
            try
            {
                hdfSelectedIds.Value = null;
                CurrentViewContext.GridCustomPaging.SortExpression = null;
                CurrentViewContext.GridCustomPaging.SortDirectionDescending = false;
                CurrentViewContext.CurrentPageIndex = 1;
                VirtualRecordCount = 0;
                ViewState["SortExpression"] = null;
                ViewState["SortDirection"] = null;
                ClearViewStatesForFilter();
                ResetControls();
                _searchContract = null;
                ResetGridFilters();
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

        /// <summary>
        /// Sets the list of filters to be displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCommunicationSummary_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdCommunicationSummary.FilterMenu;
            if (grdCommunicationSummary.clearFilterMethod == null)
                grdCommunicationSummary.clearFilterMethod = new WclGrid.ClearFilters(ClearViewStatesForFilter);
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

        /// <summary>
        /// To set data source to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCommunicationSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                if (SearchContract != null)
                {
                    GridCustomPaging.FilterColumns = SearchContract.FilterColumns;
                    GridCustomPaging.FilterOperators = SearchContract.FilterOperators;
                    GridCustomPaging.FilterValues = SearchContract.FilterValues;
                    GridCustomPaging.FilterTypes = SearchContract.FilterTypes;
                }
                else
                {
                    GridCustomPaging.FilterColumns = null;
                    GridCustomPaging.FilterOperators = null;
                    GridCustomPaging.FilterValues = null;
                    GridCustomPaging.FilterTypes = null;
                }
                Presenter.GetCommunicationSummaryArchive();
                grdCommunicationSummary.DataSource = CurrentViewContext.CommunicationSummaryList;
                // SetFilterValues();
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
        /// Grid Item Command event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCommunicationSummary_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region For Filter command

                if (!ViewState["SortExpression"].IsNull())
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
                }

                CurrentViewContext.GridCustomPaging.FilterColumns = ViewState["FilterColumns"] == null ? new List<String>() : (List<String>)(ViewState["FilterColumns"]);
                CurrentViewContext.GridCustomPaging.FilterOperators = ViewState["FilterOperators"] == null ? new List<String>() : (List<String>)(ViewState["FilterOperators"]);
                CurrentViewContext.GridCustomPaging.FilterValues = ViewState["FilterValues"] == null ? new ArrayList() : (ArrayList)(ViewState["FilterValues"]);
                CurrentViewContext.GridCustomPaging.FilterTypes = ViewState["FilterTypes"] == null ? new List<String>() : (List<String>)ViewState["FilterTypes"];

                if (e.CommandName == RadGrid.FilterCommandName)
                {
                    Pair filter = (Pair)e.CommandArgument;

                    Int32 filterIndex = CurrentViewContext.GridCustomPaging.FilterColumns.IndexOf(filter.Second.ToString());
                    if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString())
                    {
                        String filteringType = (e.Item as GridFilteringItem)[filter.Second.ToString()].Controls[0].GetType().Name;
                        String filterValue = grdCommunicationSummary.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;
                        String filterValueType = grdCommunicationSummary.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName;
                        if (filterIndex != -1)
                        {
                            CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = filter.First.ToString();
                            CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = filterValue.ToLower();
                            CurrentViewContext.GridCustomPaging.FilterTypes[filterIndex] = filterValueType;
                            if (filterValueType == "System.Decimal")
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDecimal(filterValue);
                            }
                            else if (filterValueType == "System.Int32")
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToInt32(filterValue);
                            }
                            else if (filterValueType == "System.DateTime")
                            {
                                //  CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDateTime(filterValue);
                            }
                            else
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = filterValue.ToLower();
                            }
                        }
                        else
                        {
                            CurrentViewContext.GridCustomPaging.FilterColumns.Add(filter.Second.ToString());
                            CurrentViewContext.GridCustomPaging.FilterOperators.Add(filter.First.ToString());
                            CurrentViewContext.GridCustomPaging.FilterTypes.Add(filterValueType);
                            if (grdCommunicationSummary.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Decimal")
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToDecimal(filterValue));
                            }
                            else if (grdCommunicationSummary.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Int32")
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToInt32(filterValue));
                            }
                            else if (grdCommunicationSummary.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.DateTime")
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues.Add(filterValue);
                            }
                            else
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues.Add(filterValue.ToLower());
                            }

                        }
                    }
                    else
                    {
                        if (CurrentViewContext.GridCustomPaging.FilterOperators.Count > 0)
                        {
                            CurrentViewContext.GridCustomPaging.FilterOperators.RemoveAt(filterIndex);
                            CurrentViewContext.GridCustomPaging.FilterValues.RemoveAt(filterIndex);
                            CurrentViewContext.GridCustomPaging.FilterColumns.RemoveAt(filterIndex);
                            CurrentViewContext.GridCustomPaging.FilterTypes.RemoveAt(filterIndex);
                        }
                    }

                    ViewState["FilterColumns"] = CurrentViewContext.GridCustomPaging.FilterColumns;
                    ViewState["FilterOperators"] = CurrentViewContext.GridCustomPaging.FilterOperators;
                    ViewState["FilterValues"] = CurrentViewContext.GridCustomPaging.FilterValues;
                    ViewState["FilterTypes"] = CurrentViewContext.GridCustomPaging.FilterTypes;
                }

                // Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdCommunicationSummary);

                }
                #endregion
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
        /// bound the data items in grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCommunicationSummary_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    if (Convert.ToString(dataItem["Subject"].Text).Length > 75)
                    {
                        dataItem["Subject"].ToolTip = dataItem["Subject"].Text;
                        dataItem["Subject"].Text = (dataItem["Subject"].Text).ToString().Substring(0, 70) + "...";
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

        /// <summary>
        /// Grid sort expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCommunicationSummary_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    ViewState["SortExpression"] = e.SortExpression;
                    ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);

                }
                else
                {
                    ViewState["SortExpression"] = String.Empty;
                    ViewState["SortDirection"] = false;
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

        #endregion

        #endregion

        #region Private functions
        /// <summary>
        /// To clear the viewstate value of filters
        /// </summary>
        private void ClearViewStatesForFilter()
        {
            ViewState["FilterColumns"] = null;
            ViewState["FilterOperators"] = null;
            ViewState["FilterValues"] = null;
            ViewState["FilterTypes"] = null;
        }

        /// <summary>
        /// To set controls values in contract for search
        /// </summary>
        private void SetSearchValues()
        {            
            txtEmailType.Text = _searchContract.EmailType.IsNullOrEmpty() ? String.Empty : _searchContract.EmailType;
            txtReceiver.Text = _searchContract.Receiver.IsNullOrEmpty() ? String.Empty : _searchContract.Receiver;
            txtReceiverEmailId.Text = _searchContract.ReceiverEmailId.IsNullOrEmpty() ? String.Empty : _searchContract.ReceiverEmailId;
            txtSubject.Text = _searchContract.Subject.IsNullOrEmpty() ? String.Empty : _searchContract.Subject;
            dpkrDispatchDate.SelectedDate = _searchContract.DispatchDate.IsNullOrEmpty() ? (DateTime?)null : _searchContract.DispatchDate;

            ViewState["FilterColumns"] = CurrentViewContext.GridCustomPaging.FilterColumns = _searchContract.FilterColumns;
            ViewState["FilterOperators"] = CurrentViewContext.GridCustomPaging.FilterOperators = _searchContract.FilterOperators;
            ViewState["FilterValues"] = CurrentViewContext.GridCustomPaging.FilterValues = _searchContract.FilterValues;
            ViewState["FilterTypes"] = CurrentViewContext.GridCustomPaging.FilterTypes = _searchContract.FilterTypes;
        }

        /// <summary>
        /// To get the values in the parameters and save them in contract
        /// </summary>
        private void GetSearchValues()
        {
            if (!txtEmailType.Text.Trim().IsNullOrEmpty())
            {
                if (_searchContract.IsNull())
                    _searchContract = new SearchCommunicationTemplateContract();
                _searchContract.EmailType = txtEmailType.Text.Trim();
            }

            if (!txtReceiver.Text.Trim().IsNullOrEmpty())
            {
                if (_searchContract.IsNull())
                    _searchContract = new SearchCommunicationTemplateContract();
                _searchContract.Receiver = txtReceiver.Text.Trim();
            }

            if (!txtReceiverEmailId.Text.Trim().IsNullOrEmpty())
            {
                if (_searchContract.IsNull())
                    _searchContract = new SearchCommunicationTemplateContract();
                _searchContract.ReceiverEmailId = txtReceiverEmailId.Text.Trim();
            }

            if (!txtSubject.Text.Trim().IsNullOrEmpty())
            {
                if (_searchContract.IsNull())
                    _searchContract = new SearchCommunicationTemplateContract();
                _searchContract.Subject = txtSubject.Text.Trim();
            }

            if (!dpkrDispatchDate.SelectedDate.IsNullOrEmpty())
            {
                if (_searchContract.IsNull())
                    _searchContract = new SearchCommunicationTemplateContract();
                _searchContract.DispatchDate = dpkrDispatchDate.SelectedDate;
            }
            if (rblRecipientType.SelectedItem != null)
            {
                if (_searchContract.IsNull())
                    _searchContract = new SearchCommunicationTemplateContract();

                if (rblRecipientType.SelectedItem.Text.ToString() == "To")
                {
                    _searchContract.IsTo = true;
                }
                else if (rblRecipientType.SelectedItem.Text == "Bcc")
                {
                    _searchContract.IsBcc = true;
                }
                else if (rblRecipientType.SelectedItem.Text == "Cc")
                {
                    _searchContract.IsCc = true;
                }
            }
            if (_searchContract.IsNotNull())
            {
                _searchContract.DisallowApostropheConversion = false;
                _searchContract.FilterColumns = ViewState["FilterColumns"].IsNullOrEmpty() ? null : (List<String>)ViewState["FilterColumns"];
                _searchContract.FilterOperators = ViewState["FilterOperators"].IsNullOrEmpty() ? null : (List<String>)ViewState["FilterOperators"];
                _searchContract.FilterValues = ViewState["FilterValues"].IsNullOrEmpty() ? null : (ArrayList)ViewState["FilterValues"];
                _searchContract.FilterTypes = ViewState["FilterTypes"].IsNullOrEmpty() ? null : (List<String>)ViewState["FilterTypes"];
                SetFilterValues();
            }
        }

        private void SetFilterValues()
        {
            if (!CurrentViewContext.SearchContract.FilterColumns.IsNullOrEmpty() && CurrentViewContext.SearchContract.FilterColumns.Count > 0)
            {
                CurrentViewContext.SearchContract.FilterColumns.ForEach(x =>
                grdCommunicationSummary.Columns.FindByUniqueName(x).CurrentFilterValue = CurrentViewContext.SearchContract.FilterValues[CurrentViewContext.SearchContract.FilterColumns.IndexOf(x)].ToString()
                      );
            }
        }

        private void ResetControls()
        {
            txtEmailType.Text = String.Empty;
            txtReceiver.Text = string.Empty;
            txtReceiverEmailId.Text = string.Empty;
            txtSubject.Text = string.Empty;
            rblRecipientType.ClearSelection();
            dpkrDispatchDate.Clear();
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            foreach (GridColumn column in grdCommunicationSummary.Columns)
            {
                column.CurrentFilterValue = string.Empty;
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                grdCommunicationSummary.MasterTableView.FilterExpression = String.Empty;
                column.AutoPostBackOnFilter = true;
            }

            grdCommunicationSummary.MasterTableView.SortExpressions.Clear();
            grdCommunicationSummary.CurrentPageIndex = 0;
            grdCommunicationSummary.MasterTableView.CurrentPageIndex = 0;
            grdCommunicationSummary.Rebind();
        }
        #endregion



    }
}

