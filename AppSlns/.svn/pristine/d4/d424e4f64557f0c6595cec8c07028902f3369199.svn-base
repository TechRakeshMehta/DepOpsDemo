#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using System.Collections;

#endregion

#region Application Specific

using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.CommonControls.Views;
using INTSOF.Utils.Consts;
using INTERSOFT.WEB.UI.WebControls;
//using Microsoft.Practices.CompositeWeb.Web.UI;
using Entity.ClientEntity;
using CoreWeb.Shell;
using System.Xml.Serialization;
using System.IO;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Text;
using System.Web.UI;
using System.Threading;
using CoreWeb.IntsofSecurityModel;
using INTSOF.UI.Contract.ComplianceOperation;


#endregion

#endregion


namespace CoreWeb.Search.Views
{
    public partial class ApplicantSubscriptionSearch : BaseUserControl, IApplicantSubscriptionSearchView
    {
        #region Variables

        private ApplicantSubscriptionSearchPresenter _presenter = new ApplicantSubscriptionSearchPresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private CustomPagingArgsContract _gridCustomPaging = null;
        private SearchItemDataContract _gridSearchContract = null;

        #endregion

        #region Properties


        public ApplicantSubscriptionSearchPresenter Presenter
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
                if (String.IsNullOrEmpty(ddlTenantName.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlTenantName.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlTenantName.SelectedValue = value.ToString();
                }
                else
                {
                    ddlTenantName.SelectedIndex = value;
                }
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return tenantId;
            }
            set { tenantId = value; }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public IApplicantSubscriptionSearchView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        public String ApplicantFirstName
        {
            get
            {
                return txtFirstName.Text;
            }
            set
            {
                txtFirstName.Text = value;
            }
        }

        public String ApplicantLastName
        {
            get
            {
                return txtLastName.Text;
            }
            set
            {
                txtLastName.Text = value;
            }
        }

        public DateTime? DateOfBirth
        {
            get
            {
                return dpkrDOB.SelectedDate;
            }
            set
            {
                dpkrDOB.SelectedDate = value;
            }
        }

        public List<ApplicantSearchDataContract> ApplicantSearchData
        {
            get;
            set;
        }

        /// <summary>
        /// Populates the dropdown with the list of Compliance Package.
        /// </summary>
        public List<ComplaincePackageDetails> lstCompliancePackage
        {
            set
            {
                ddlPackage.DataSource = value;
                ddlPackage.DataBind();
            }
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public Boolean IsDOBDisable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsDOBDisabled"] ?? false);
            }
            set
            {
                ViewState["IsDOBDisabled"] = value;
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
                return grdApplicantSearchData.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdApplicantSearchData.MasterTableView.CurrentPageIndex > 0)
                {
                    grdApplicantSearchData.MasterTableView.CurrentPageIndex = value - 1;
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
                return grdApplicantSearchData.PageSize;
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
                grdApplicantSearchData.VirtualItemCount = value;
                grdApplicantSearchData.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPaging
        {
            get
            {
                if (_gridCustomPaging.IsNull())
                {
                    _gridCustomPaging = new CustomPagingArgsContract();
                }
                return _gridCustomPaging;
            }
        }

        #endregion

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Applicant Subscription Search";
                base.SetPageTitle("Applicant Subscription Search");
                fsucCmdBarButton.SubmitButton.CausesValidation = false;

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
        /// Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search applicants per the criteria entered above";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";
            if (!this.IsPostBack)
            {
                //Set MinDate and MaxDate for DOB
                dpkrDOB.MinDate = Convert.ToDateTime("01-01-1900");
                dpkrDOB.MaxDate = DateTime.Now;

                Presenter.OnViewInitialized();
                BindControls();
            }
            Presenter.OnViewLoaded();
            HideShowControlsForGranularPermission();
        }

        /// <summary>
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            ResetGridFilters();
        }

        /// <summary>
        /// To reset controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            dpkrDOB.SelectedDate = null;

            if (Presenter.IsDefaultTenant)
            {
                SelectedTenantId = AppConsts.NONE;
                CurrentViewContext.lstCompliancePackage = new List<ComplaincePackageDetails>();
            }
            else
            {
                ddlPackage.SelectedIndex = AppConsts.NONE;
            }


            //To reset grid filters 
            ResetGridFilters();
        }

        /// <summary>
        /// Redirect to Home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
        }

        #region Grid Events

        /// <summary>
        /// Sets the list of filters to be displayed in Queue. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdApplicantSearchData.FilterMenu;
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
        protected void grdApplicantSearchData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;

                Presenter.PerformSearch();
                grdApplicantSearchData.DataSource = CurrentViewContext.ApplicantSearchData;
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
        protected void grdApplicantSearchData_ItemCommand(object sender, GridCommandEventArgs e)
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

                if (e.CommandName == RadGrid.FilterCommandName)
                {
                    Pair filter = (Pair)e.CommandArgument;

                    Int32 filterIndex = CurrentViewContext.GridCustomPaging.FilterColumns.IndexOf(filter.Second.ToString());
                    if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString())
                    {
                        String filteringType = (e.Item as GridFilteringItem)[filter.Second.ToString()].Controls[0].GetType().Name;
                        String filterValue = grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;

                        if (filterIndex != -1)
                        {
                            CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = filter.First.ToString();
                            if (grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Decimal")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDecimal(filterValue);
                                }

                                //If filter Value Is Null Or Empty then set filter value to default value and set IsNull filter to other filter
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = AppConsts.NONE;
                                }
                            }
                            else if (grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Int32")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToInt32(filterValue);
                                }

                                //If filter Value Is Null Or Empty then set filter value to default value and set IsNull filter to other filter
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = AppConsts.NONE;
                                }
                            }
                            else if (grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.DateTime")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    try
                                    {
                                        //try to convert any value to date
                                        CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDateTime(filterValue);
                                    }
                                    catch
                                    {
                                        //date filter value could not be converted, set filter value to any default date
                                        CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDateTime("01/01/1901");
                                        //return;
                                    }
                                }

                                //To set IsNull filter to other Date format filter and set filter value to any default date in case of Null date
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDateTime("01/01/1901");
                                }
                            }
                            else
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = filterValue;
                            }
                        }
                        else
                        {
                            CurrentViewContext.GridCustomPaging.FilterColumns.Add(filter.Second.ToString());
                            CurrentViewContext.GridCustomPaging.FilterOperators.Add(filter.First.ToString());
                            if (grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Decimal")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToDecimal(filterValue));
                                }

                                //If filter Value Is Null Or Empty then set filter value to default value and set IsNull filter to other filter
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    Int32 index = CurrentViewContext.GridCustomPaging.FilterOperators.IndexOf("IsNull");
                                    CurrentViewContext.GridCustomPaging.FilterOperators[index] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues.Add(AppConsts.NONE);
                                }
                            }
                            else if (grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Int32")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToInt32(filterValue));
                                }

                                //If filter Value Is Null Or Empty then set filter value to default value and set IsNull filter to other filter
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    Int32 index = CurrentViewContext.GridCustomPaging.FilterOperators.IndexOf("IsNull");
                                    CurrentViewContext.GridCustomPaging.FilterOperators[index] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues.Add(AppConsts.NONE);
                                }
                            }
                            else if (grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.DateTime")
                            {
                                if (!filterValue.IsNullOrEmpty())
                                {
                                    try
                                    {
                                        //try to convert any value to date
                                        CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToDateTime(filterValue));
                                    }
                                    catch
                                    {
                                        //date filter value could not be converted, set filter value to any default date
                                        CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToDateTime("01/01/1901"));
                                        //return;
                                    }
                                }

                                //To set IsNull filter to other Date format filter and set filter value to any default date in case of Null date
                                if (CurrentViewContext.GridCustomPaging.FilterOperators.Contains("IsNull"))
                                {
                                    Int32 index = CurrentViewContext.GridCustomPaging.FilterOperators.IndexOf("IsNull");
                                    CurrentViewContext.GridCustomPaging.FilterOperators[index] = "NullOtherThanString";
                                    CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToDateTime("01/01/1901"));
                                }
                            }
                            else
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues.Add(filterValue);
                            }
                        }
                    }
                    else
                    {
                        CurrentViewContext.GridCustomPaging.FilterOperators.RemoveAt(filterIndex);
                        CurrentViewContext.GridCustomPaging.FilterValues.RemoveAt(filterIndex);
                        CurrentViewContext.GridCustomPaging.FilterColumns.RemoveAt(filterIndex);
                    }

                    ViewState["FilterColumns"] = CurrentViewContext.GridCustomPaging.FilterColumns;
                    ViewState["FilterOperators"] = CurrentViewContext.GridCustomPaging.FilterOperators;
                    ViewState["FilterValues"] = CurrentViewContext.GridCustomPaging.FilterValues;
                }

                #endregion

                // Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdApplicantSearchData);

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
        protected void grdApplicantSearchData_SortCommand(object sender, GridSortCommandEventArgs e)
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

        /// <summary>
        /// To bind Packages dropdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (ddlTenantName.SelectedIndex > 0)
            {
                Presenter.GetCompliancePackage();
            }
            else
            {
                CurrentViewContext.lstCompliancePackage = new List<ComplaincePackageDetails>();
                ResetGridFilters();
            }
        }

        /// <summary>
        /// Tenant Name DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            ddlTenantName.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        /// <summary>
        /// Package DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPackage_DataBound(object sender, EventArgs e)
        {
            ddlPackage.Items.Insert(0, new DropDownListItem("--Select--"));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To bind controls
        /// </summary>
        private void BindControls()
        {
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();

            if (Presenter.IsDefaultTenant)
            {
                ddlTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantId = 0;
            }
            else
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
                Presenter.GetCompliancePackage();
            }
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdApplicantSearchData.MasterTableView.FilterExpression = null;
            grdApplicantSearchData.MasterTableView.SortExpressions.Clear();
            grdApplicantSearchData.CurrentPageIndex = 0;
            grdApplicantSearchData.MasterTableView.CurrentPageIndex = 0;
            foreach (GridColumn column in grdApplicantSearchData.MasterTableView.RenderColumns)
            {
                if (column.ColumnType == "GridBoundColumn")
                {
                    GridBoundColumn boundColumn = (GridBoundColumn)column;
                    String columnName = boundColumn.UniqueName.ToString();
                    grdApplicantSearchData.MasterTableView.GetColumnSafe(columnName).CurrentFilterFunction = GridKnownFunction.NoFilter;
                    grdApplicantSearchData.MasterTableView.GetColumnSafe(columnName).CurrentFilterValue = String.Empty;
                }
            }
            CurrentViewContext.GridCustomPaging.FilterColumns = new List<String>();
            CurrentViewContext.GridCustomPaging.FilterOperators = new List<String>();
            CurrentViewContext.GridCustomPaging.FilterValues = new ArrayList();

            ViewState["FilterColumns"] = CurrentViewContext.GridCustomPaging.FilterColumns;
            ViewState["FilterOperators"] = CurrentViewContext.GridCustomPaging.FilterOperators;
            ViewState["FilterValues"] = CurrentViewContext.GridCustomPaging.FilterValues;
            
            grdApplicantSearchData.Rebind();
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        /// <summary>
        /// Hide Show grid and page controls
        /// </summary>
        private void HideShowControlsForGranularPermission()
        {
            if (CurrentViewContext.IsDOBDisable)
            {
                divDOB.Visible = false;
                grdApplicantSearchData.MasterTableView.GetColumn("DateOfBirth").Visible = false;
            }
        }
        #endregion

        #endregion
    }
}

