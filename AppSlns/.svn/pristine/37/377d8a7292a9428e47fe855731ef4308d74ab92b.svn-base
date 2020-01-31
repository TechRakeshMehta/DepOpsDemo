using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System.Collections;
using INTERSOFT.WEB.UI.WebControls;

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class ManageAgencyHierarchy : BaseUserControl, IManageAgencyHierarchyView
    {

        #region Private Variables
        private ManageAgencyHierarchyPresenter _presenter = new ManageAgencyHierarchyPresenter();
        private String _viewType;
        private Int32 _tenantId = 0;
        #endregion

        #region Properties
        #region Public Properties

        public ManageAgencyHierarchyPresenter Presenter
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

        public IManageAgencyHierarchyView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #endregion

        #region Private Properties

        Int32 IManageAgencyHierarchyView.TenantId
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

        Boolean IManageAgencyHierarchyView.IsAdminLoggedIn
        {
            get;
            set;
        }

        Int32 IManageAgencyHierarchyView.CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        List<AgencyHierarchyDataContract> IManageAgencyHierarchyView.lstAgencyHierarchy { get; set; }


        Int32 IManageAgencyHierarchyView.AgencyHierarchyId
        {
            get;
            set;
        }
        #endregion

        #region Custom paging parameters

        Int32 IManageAgencyHierarchyView.CurrentPageIndex
        {
            get
            {
                return grdAgencyHierarchy.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (value > 0)
                {
                    grdAgencyHierarchy.MasterTableView.CurrentPageIndex = value - 1;
                }
            }
        }

        Int32 IManageAgencyHierarchyView.PageSize
        {
            get
            {
                return grdAgencyHierarchy.PageSize;
            }
            set
            {
                grdAgencyHierarchy.PageSize = value;
            }
        }

        Int32 IManageAgencyHierarchyView.VirtualRecordCount
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
                grdAgencyHierarchy.VirtualItemCount = value;
                grdAgencyHierarchy.MasterTableView.VirtualItemCount = value;
            }
        }

        CustomPagingArgsContract IManageAgencyHierarchyView.GridCustomPaging
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
        #endregion

        #endregion

        #region Page Events

        /// <summary>
        /// set the page title on bread crumb
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            base.Title = "Manage Agency Hierarchy";
            base.SetPageTitle("Manage Agency Hierarchy");
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    if (Request.QueryString["args"].IsNullOrEmpty())
                    {
                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_AGENCY_HIERARCHY_GRID, null);
                    }
                    GetSessionValues();
                    //RedirectOnHierarchyTree();

                }
                Presenter.OnViewLoaded();
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

        protected void grdAgencyHierarchy_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdAgencyHierarchy.FilterMenu;

            if (grdAgencyHierarchy.clearFilterMethod == null)
                grdAgencyHierarchy.clearFilterMethod = new WclGrid.ClearFilters(ClearViewStatesForFilter);

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

        protected void grdAgencyHierarchy_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                CurrentViewContext.GridCustomPaging.CurrentPageIndex = CurrentViewContext.CurrentPageIndex;
                CurrentViewContext.GridCustomPaging.PageSize = CurrentViewContext.PageSize;
                CurrentViewContext.GridCustomPaging = CurrentViewContext.GridCustomPaging;
                Presenter.GetAgencyHierarchyList();
                grdAgencyHierarchy.DataSource = CurrentViewContext.lstAgencyHierarchy;
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

        protected void grdAgencyHierarchy_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "InitInsert")
                {
                    RedirectOnHierarchyTree(AppConsts.NONE);
                }
                else if (e.CommandName == "Edit")
                {
                    Int32 agencyHierarchyID = Convert.ToInt32((e.Item as GridDataItem).GetDataKeyValue("AgencyHierarchyID"));
                    RedirectOnHierarchyTree(agencyHierarchyID);
                }

                SetGridFilters();

                if (e.CommandName == RadGrid.FilterCommandName)
                {
                    Pair filter = (Pair)e.CommandArgument;

                    Int32 filterIndex = CurrentViewContext.GridCustomPaging.FilterColumns.IndexOf(filter.Second.ToString());
                    if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString())
                    {
                        String filteringType = grdAgencyHierarchy.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName;
                        String filterValue = grdAgencyHierarchy.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;

                        if (filterIndex != -1)
                        {
                            CurrentViewContext.GridCustomPaging.FilterTypes[filterIndex] = filteringType;
                            CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = filter.First.ToString();
                            CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = filterValue;
                        }
                        else
                        {
                            CurrentViewContext.GridCustomPaging.FilterTypes.Add(filteringType);
                            CurrentViewContext.GridCustomPaging.FilterColumns.Add(filter.Second.ToString());
                            CurrentViewContext.GridCustomPaging.FilterOperators.Add(filter.First.ToString());
                            CurrentViewContext.GridCustomPaging.FilterValues.Add(filterValue);
                        }
                    }
                    else if (filterIndex != -1)
                    {
                        CurrentViewContext.GridCustomPaging.FilterOperators.RemoveAt(filterIndex);
                        CurrentViewContext.GridCustomPaging.FilterValues.RemoveAt(filterIndex);
                        CurrentViewContext.GridCustomPaging.FilterColumns.RemoveAt(filterIndex);
                        CurrentViewContext.GridCustomPaging.FilterTypes.RemoveAt(filterIndex);
                    }

                    ViewState["FilterColumns"] = CurrentViewContext.GridCustomPaging.FilterColumns;
                    ViewState["FilterOperators"] = CurrentViewContext.GridCustomPaging.FilterOperators;
                    ViewState["FilterValues"] = CurrentViewContext.GridCustomPaging.FilterValues;
                    ViewState["FilterTypes"] = CurrentViewContext.GridCustomPaging.FilterTypes;
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
        ///  Grid SortCommand event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdAgencyHierarchy_SortCommand(object sender, GridSortCommandEventArgs e)
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

        protected void grdAgencyHierarchy_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.AgencyHierarchyId = Convert.ToInt32((e.Item as GridDataItem).GetDataKeyValue("AgencyHierarchyID"));


                if (Presenter.DeleteAgencyHierarchy())
                {
                    base.ShowSuccessMessage("Agency Hierarchy deleted successfully.");
                    grdAgencyHierarchy.Rebind();
                }
                else
                {
                    base.ShowInfoMessage("Some error has occurred.Please try again.");
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
        /// <summary>
        /// Method to Redirect on agency hierarchy tree screen
        /// </summary>
        /// <param name="agencyHierarchyID">agencyHierarchyID</param>
        private void RedirectOnHierarchyTree(Int32 agencyHierarchyID)
        {

            Dictionary<String, String> queryString = new Dictionary<String, String>();
            SetSessionValues();
            queryString = new Dictionary<String, String>
                                                         { 
                                                            {"Child", @"~/AgencyHierarchy/UserControls/AgencyHierarchy.ascx"},
                                                            {"AgencyHierarchyID",Convert.ToString(agencyHierarchyID)},
                                                         };
            string url = String.Format("~/AgencyHierarchy/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

            Response.Redirect(url, true);
        }

        private void SetGridFilters()
        {
            CurrentViewContext.GridCustomPaging.FilterColumns = ViewState["FilterColumns"] == null ? new List<String>() : (List<String>)(ViewState["FilterColumns"]);
            CurrentViewContext.GridCustomPaging.FilterOperators = ViewState["FilterOperators"] == null ? new List<String>() : (List<String>)(ViewState["FilterOperators"]);
            CurrentViewContext.GridCustomPaging.FilterValues = ViewState["FilterValues"] == null ? new ArrayList() : (ArrayList)(ViewState["FilterValues"]);
            CurrentViewContext.GridCustomPaging.FilterTypes = ViewState["FilterTypes"] == null ? new List<String>() : (List<String>)(ViewState["FilterTypes"]);
        }

        public void ClearViewStatesForFilter()
        {
            ViewState["FilterColumns"] = null;
            ViewState["FilterOperators"] = null;
            ViewState["FilterValues"] = null;
            ViewState["FilterTypes"] = null;
            CurrentViewContext.GridCustomPaging.FilterColumns = null;
            CurrentViewContext.GridCustomPaging.FilterOperators = null;
            CurrentViewContext.GridCustomPaging.FilterValues = null;
            CurrentViewContext.GridCustomPaging.FilterTypes = null;
        }

        private void SetSessionValues()
        {
            CustomPagingArgsContract customPagingFilter = CurrentViewContext.GridCustomPaging;

            customPagingFilter.FilterColumns = ViewState["FilterColumns"].IsNullOrEmpty() ? null : (List<String>)ViewState["FilterColumns"];
            customPagingFilter.FilterOperators = ViewState["FilterOperators"].IsNullOrEmpty() ? null : (List<String>)ViewState["FilterOperators"];
            customPagingFilter.FilterValues = ViewState["FilterValues"].IsNullOrEmpty() ? null : (ArrayList)ViewState["FilterValues"];
            customPagingFilter.FilterTypes = ViewState["FilterTypes"].IsNullOrEmpty() ? null : (List<String>)ViewState["FilterTypes"];

            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_AGENCY_HIERARCHY_GRID, customPagingFilter);
        }

        private void GetSessionValues()
        {
            CustomPagingArgsContract customPagingFilter = (CustomPagingArgsContract)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_KEY_AGENCY_HIERARCHY_GRID);
            if (customPagingFilter.IsNotNull())
            {
                ViewState["FilterColumns"] = CurrentViewContext.GridCustomPaging.FilterColumns = customPagingFilter.FilterColumns;
                ViewState["FilterOperators"] = CurrentViewContext.GridCustomPaging.FilterOperators = customPagingFilter.FilterOperators;
                ViewState["FilterValues"] = CurrentViewContext.GridCustomPaging.FilterValues = customPagingFilter.FilterValues;
                ViewState["FilterTypes"] = CurrentViewContext.GridCustomPaging.FilterTypes = customPagingFilter.FilterTypes;
                CurrentViewContext.GridCustomPaging = customPagingFilter;
                SetFilterValues();

                SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_AGENCY_HIERARCHY_GRID, null);
            }
        }

        public void SetFilterValues()
        {
            if (!CurrentViewContext.GridCustomPaging.FilterColumns.IsNullOrEmpty() && CurrentViewContext.GridCustomPaging.FilterColumns.Count > 0)
            {
                CurrentViewContext.GridCustomPaging.FilterColumns.ForEach(x =>
                    grdAgencyHierarchy.Columns.FindByUniqueName(x).CurrentFilterValue = CurrentViewContext.GridCustomPaging.FilterValues[CurrentViewContext.GridCustomPaging.FilterColumns.IndexOf(x)].ToString()
                    );
            }
        }

        #endregion
    }
}