using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
    public partial class BkgComplPkgDataMapping : BaseUserControl, IBkgComplPkgDataMappingView
    {
        #region VARIABLES
        Boolean _flag = false;
        private BkgComplPkgDataMappingPresenter _presenter = new BkgComplPkgDataMappingPresenter();
        private Int32 tenantId = 0;
        private Int32 _selectedTenantId = AppConsts.NONE;
        private Boolean? _isAdminLoggedIn = null;
        #endregion

        #region PRESENTER

        public BkgComplPkgDataMappingPresenter Presenter
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

        #region PROPERTIES

        public IBkgComplPkgDataMappingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Boolean IBkgComplPkgDataMappingView.IsReset
        {
            get;
            set;
        }

        List<BackgroundPackage> IBkgComplPkgDataMappingView.lstBackgroundPackage
        {
            //UAT-1451
            //set
            //{
            //    cmbBkgPackages.DataSource = value;
            //    cmbBkgPackages.DataBind();
            //    if (cmbBkgPackages.Items.Count >= 10)
            //    {
            //        cmbBkgPackages.Height = Unit.Pixel(200);
            //    }
            //}

            get;
            set;
        }


        List<Entity.Tenant> IBkgComplPkgDataMappingView.ListTenants
        {
            set;
            get;
        }

        Int32 IBkgComplPkgDataMappingView.TenantId
        {
            get
            {
                if (tenantId == 0)
                {
                    //tenantId = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user != null)
                    {
                        tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return tenantId;

            }
            set { tenantId = value; }
        }

        Int32 IBkgComplPkgDataMappingView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Boolean IBkgComplPkgDataMappingView.IsAdminLoggedIn
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

        Int32 IBkgComplPkgDataMappingView.DefaultTenantId
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

        Int32 IBkgComplPkgDataMappingView.SelectedTenantId
        {
            get
            {
                if (_selectedTenantId == AppConsts.NONE)
                {
                    Int32.TryParse(ddlTenant.SelectedValue, out _selectedTenantId);

                    if (_selectedTenantId == AppConsts.NONE)
                        _selectedTenantId = CurrentViewContext.TenantId;
                }
                return _selectedTenantId;
            }
            set
            {
                _selectedTenantId = value;
            }
        }

        List<ComplaincePackageDetails> IBkgComplPkgDataMappingView.lstCompliancePackage
        {
            //set
            //{
            //    cmbComplPackages.DataSource = value;
            //    cmbComplPackages.DataBind();
            //    if (cmbComplPackages.Items.Count >= 10)
            //    {
            //        cmbComplPackages.Height = Unit.Pixel(200);
            //    }
            //}
            set;
            get;
        }

        //UAT-1451
        Int32 IBkgComplPkgDataMappingView.BackgroundPackageId
        {
            get
            {
                if (!cmbBkgPackagesSearch.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(cmbBkgPackagesSearch.SelectedValue);
                }
                return AppConsts.NONE;
            }
        }
        Int32 IBkgComplPkgDataMappingView.CompliancePackageId
        {
            get
            {
                if (!cmbComplPackagesSearch.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(cmbComplPackagesSearch.SelectedValue);
                }
                return AppConsts.NONE;
            }
        }
        String IBkgComplPkgDataMappingView.InfoMessage { get; set; }
        #region Custom Paging


        /// <summary>
        /// Current Page Index</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdBkgComplPkgDataMap.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdBkgComplPkgDataMap.MasterTableView.CurrentPageIndex > 0)
                {
                    grdBkgComplPkgDataMap.MasterTableView.CurrentPageIndex = value - 1;
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
                //return grdApplicantSearchData.PageSize > 100 ? 100 : grdApplicantSearchData.PageSize;
                return grdBkgComplPkgDataMap.PageSize;
            }
            set
            {
                grdBkgComplPkgDataMap.PageSize = value;
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
                grdBkgComplPkgDataMap.VirtualItemCount = value;
                grdBkgComplPkgDataMap.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// To get object of shared class of custom paging
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
                VirtualRecordCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        #endregion

        #endregion

        #region PAGE EVENTS
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Background Compliance Package Data Mapping";
                base.SetPageTitle("Background Compliance Package Data Mapping");
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
                    grdBkgComplPkgDataMap.Visible = false;
                    divDataSyn.Visible = false;
                    BindTenant();
                    Presenter.OnViewInitialized();
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
        protected void grdBkgComplPkgDataMap_Init(object sender, System.EventArgs e)
        {
            try
            {
                GridFilterMenu menu = grdBkgComplPkgDataMap.FilterMenu;

                if (grdBkgComplPkgDataMap.clearFilterMethod == null)
                    grdBkgComplPkgDataMap.clearFilterMethod = new WclGrid.ClearFilters(ClearViewStatesForFilter);
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

        protected void grdBkgComplPkgDataMap_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {

            try
            {
                //UAT-1451
                //if (!ddlTenant.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(ddlTenant.SelectedValue) > AppConsts.NONE && ViewState["ReBindGrid"].IsNullOrEmpty())
                //{
                    GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                    GridCustomPaging.PageSize = PageSize;
                    CurrentViewContext.GridCustomPaging = GridCustomPaging;
                //START UAT-4214
                    if (CurrentViewContext.IsReset && !CurrentViewContext.IsAdminLoggedIn)
                    {
                        grdBkgComplPkgDataMap.DataSource = new List<BkgCompliancePackageMappingSearchData>();
                        grdBkgComplPkgDataMap.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                        CurrentViewContext.VirtualRecordCount = AppConsts.NONE;
                    }
                    else if (!ddlTenant.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(ddlTenant.SelectedValue) > AppConsts.NONE && ViewState["ReBindGrid"].IsNullOrEmpty())
                    {
                        grdBkgComplPkgDataMap.DataSource = Presenter.FetchBkgCompliancePackageMapping().ToList();
                        grdBkgComplPkgDataMap.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = true;
                    }
                    else
                    {
                        grdBkgComplPkgDataMap.DataSource = new List<BkgCompliancePackageMappingSearchData>();
                        grdBkgComplPkgDataMap.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                        CurrentViewContext.VirtualRecordCount = AppConsts.NONE;
                    }
                //END UAT
                
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

        protected void grdBkgComplPkgDataMap_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                BkgComplPkgDataMappingContract bkgComplPkgDataMappingContract = GetDataForMapping(e);

                Boolean IsInserted = Presenter.SaveBkgComplPkgDataMapping(bkgComplPkgDataMappingContract);
                if (IsInserted)
                {
                    ShowSuccessMessage("Package mapped successfully.");
                }
                else
                {
                    if (CurrentViewContext.InfoMessage.IsNullOrEmpty())
                    {
                        ShowErrorMessage("Package mapping failed.");
                    }
                    else
                    {
                        ShowInfoMessage(CurrentViewContext.InfoMessage);
                        e.Canceled = true;
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

        protected void grdBkgComplPkgDataMap_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                String errorMessage;
                BkgComplPkgDataMappingContract bkgComplPkgDataMappingContract = GetDataForMapping(e);
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 BCPM_ID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BCPM_ID"));
                errorMessage = Presenter.UpdateBkgComplPkgDataMapping(BCPM_ID, bkgComplPkgDataMappingContract);
                if (errorMessage == "Mapping Updated Successfully.")
                {
                    base.ShowSuccessMessage("Mapping Updated Successfully.");
                }
                else
                {
                    if (CurrentViewContext.InfoMessage.IsNullOrEmpty())
                    {
                        base.ShowErrorInfoMessage(errorMessage);
                    }
                    else
                    {
                        ShowInfoMessage(CurrentViewContext.InfoMessage);
                        e.Canceled = true;
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

        protected void grdBkgComplPkgDataMap_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                String errorMessage;
                Int32 BCPM_ID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BCPM_ID"]);
                errorMessage = Presenter.DeleteBkgComplPkgDataMapping(BCPM_ID);
                if (errorMessage == "Mapping deleted successfully.")
                {
                    base.ShowSuccessMessage("Mapping deleted successfully.");
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

        protected void grdBkgComplPkgDataMap_ItemDataBound(object sender, GridItemEventArgs e)
        {
            //if (ddlTenant.SelectedIndex > AppConsts.NONE && cmbBkgPackages.SelectedIndex > AppConsts.NONE && cmbComplPackages.SelectedIndex > AppConsts.NONE)
            //{

            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                BkgCompliancePackageMappingSearchData bkgComplianceMapping = new BkgCompliancePackageMappingSearchData();

                WclComboBox cmbDataPoints = gridEditableItem.FindControl("cmbDataPoints") as WclComboBox;
                WclComboBox cmbBkgPkg = gridEditableItem.FindControl("cmbBkgPkg") as WclComboBox;
                WclComboBox cmbComplPkg = gridEditableItem.FindControl("cmbComplPkg") as WclComboBox;
                cmbDataPoints.Items.Insert(0, new RadComboBoxItem("--Select--"));
                cmbDataPoints.DataSource = Presenter.GetDataPoints();
                cmbDataPoints.DataBind();

                //UAT-1451
                Presenter.GetCompliancePackage();
                Presenter.GetBackgroundPackages();
                cmbBkgPkg.Items.Insert(0, new RadComboBoxItem("--Select--"));
                cmbBkgPkg.DataSource = CurrentViewContext.lstBackgroundPackage;
                cmbBkgPkg.DataBind();

                cmbComplPkg.Items.Insert(0, new RadComboBoxItem("--Select--"));
                cmbComplPkg.DataSource = CurrentViewContext.lstCompliancePackage;
                cmbComplPkg.DataBind();

                cmbComplPkg.SelectedValue = cmbComplPackagesSearch.SelectedValue;
                cmbBkgPkg.SelectedValue = cmbBkgPackagesSearch.SelectedValue;


                if (_flag)
                {
                    Int32 BCPM_ID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BCPM_ID"));
                    //UAT-1451
                    // List<BkgCompliancePackageMappingSearchData> bkgCompliancePackageMappingAllrecords = Presenter.FetchBkgCompliancePackageMapping().ToList();
                    //bkgComplianceMapping = bkgCompliancePackageMappingAllrecords.Where(x => x.BCPM_ID == BCPM_ID).FirstOrDefault();
                    bkgComplianceMapping = (BkgCompliancePackageMappingSearchData)e.Item.DataItem;
                    String code = bkgComplianceMapping.BDPT_Code;
                    cmbDataPoints.SelectedValue = code;
                    BindDropDownDependentOnDataPoint(cmbDataPoints, code, bkgComplianceMapping);
                    //cmbCatagories.SelectedValue = Convert.ToString(bkgComplianceMapping.BCPM_ComplianceCategoryID);

                    //UAT-1451
                    if (bkgComplianceMapping.IsNotNull())
                    {
                        cmbComplPkg.SelectedValue = bkgComplianceMapping.BCPM_CompliancePkgID.ToString();
                        cmbBkgPkg.SelectedValue = bkgComplianceMapping.BCPM_BkgPackageID.ToString();
                    }

                }

                Int32 selectedComplPkgID = 0;
                WclComboBox cmbCatagories = gridEditableItem.FindControl("cmbCatagory") as WclComboBox;
                cmbCatagories.Items.Insert(0, new RadComboBoxItem("--Select--"));
                if (!_flag)
                {
                    selectedComplPkgID = cmbComplPkg.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbComplPkg.SelectedValue);
                    ShowHideDataPointDiv(cmbComplPkg);
                }
                else
                {
                    selectedComplPkgID = bkgComplianceMapping.BCPM_CompliancePkgID;
                    //UAT-1451
                    //cmbComplPackages.SelectedValue = Convert.ToString(selectedComplPkgID);

                }
                cmbCatagories.DataSource = Presenter.GetComplianceCatagories(selectedComplPkgID);
                cmbCatagories.DataBind();
                if (_flag)
                {
                    cmbCatagories.SelectedValue = Convert.ToString(bkgComplianceMapping.BCPM_ComplianceCategoryID);
                    BindDropdownRelatedToCatagory(cmbCatagories, bkgComplianceMapping);
                    _flag = false;
                }
            }

            //}
        }

        protected void grdBkgComplPkgDataMap_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region For Filter command

                CurrentViewContext.GridCustomPaging.FilterColumns = ViewState["FilterColumns"] == null ? new List<String>() : (List<String>)(ViewState["FilterColumns"]);
                CurrentViewContext.GridCustomPaging.FilterOperators = ViewState["FilterOperators"] == null ? new List<String>() : (List<String>)(ViewState["FilterOperators"]);
                CurrentViewContext.GridCustomPaging.FilterValues = ViewState["FilterValues"] == null ? new ArrayList() : (ArrayList)(ViewState["FilterValues"]);
                CurrentViewContext.GridCustomPaging.FilterTypes = ViewState["FilterTypes"] == null ? new List<String>() : (List<String>)(ViewState["FilterTypes"]);

                if (e.CommandName == RadGrid.FilterCommandName)
                {
                    /*UAT-2904*/
                    foreach (GridColumn item in grdBkgComplPkgDataMap.MasterTableView.Columns)
                    {
                        string filterFunction = item.CurrentFilterFunction.ToString();
                        string filterValue = item.CurrentFilterValue;
                        if (filterValue.IsNullOrEmpty())
                        {
                            item.CurrentFilterFunction = Telerik.Web.UI.GridKnownFunction.NoFilter;
                        }
                    }
                    /*UAT-2904 ends here*/

                    Pair filter = (Pair)e.CommandArgument;

                    Int32 filterIndex = CurrentViewContext.GridCustomPaging.FilterColumns.IndexOf(filter.Second.ToString());
                    if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString())
                    {
                        String filteringType = grdBkgComplPkgDataMap.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName;
                        String filterValue = grdBkgComplPkgDataMap.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;

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

                #endregion
                if (e.CommandName == "Edit")
                {
                    _flag = true;
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
        protected void grdBkgComplPkgDataMap_SortCommand(object sender, GridSortCommandEventArgs e)
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

        #region Button Events
        /// <summary>
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsReset = false;
                grdBkgComplPkgDataMap.Visible = true;
                divDataSyn.Visible = true;
                ViewState["ReBindGrid"] = null;
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

        /// <summary>
        /// To reset controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsReset = true;
                CurrentViewContext.VirtualRecordCount = 0;
                BindTenant();
                ResetControls();
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

        /// <summary>
        /// Redirect to Home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
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

        #region DROPDOWN EVENTS

        #region DROPDOWN DATABOUND EVENTS
        protected void cmbBkgPackages_DataBound(object sender, EventArgs e)
        {
            cmbBkgPackagesSearch.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbComplPackages_DataBound(object sender, EventArgs e)
        {
            cmbComplPackagesSearch.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbDataPoints_DataBound(object sender, EventArgs e)
        {
            WclComboBox cmbDataPoints = sender as WclComboBox;
            cmbDataPoints.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbServiceGroup_DataBound(object sender, EventArgs e)
        {
            WclComboBox cmbServiceGroup = sender as WclComboBox;
            cmbServiceGroup.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbServices_DataBound(object sender, EventArgs e)
        {
            WclComboBox cmbServices = sender as WclComboBox;
            cmbServices.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbServiceItems_DataBound(object sender, EventArgs e)
        {
            WclComboBox cmbServiceItems = sender as WclComboBox;
            cmbServiceItems.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbCatagory_DataBound(object sender, EventArgs e)
        {
            WclComboBox cmbCatagory = sender as WclComboBox;
            cmbCatagory.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbItems_DataBound(object sender, EventArgs e)
        {
            WclComboBox cmbItems = sender as WclComboBox;
            cmbItems.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbAttributes_DataBound(object sender, EventArgs e)
        {
            WclComboBox cmbAttribute = sender as WclComboBox;
            cmbAttribute.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbFlagged_DataBound(object sender, EventArgs e)
        {
            WclComboBox cmbFlagged = sender as WclComboBox;
            cmbFlagged.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }
        protected void cmbNonFlagged_DataBound(object sender, EventArgs e)
        {
            WclComboBox cmbNonFlagged = sender as WclComboBox;
            cmbNonFlagged.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        //UAT-1451
        protected void cmbBkgPkg_DataBound(object sender, EventArgs e)
        {
            WclComboBox cmbBkgPkg = sender as WclComboBox;
            cmbBkgPkg.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbComplPkg_DataBound(object sender, EventArgs e)
        {
            WclComboBox cmbComplPkg = sender as WclComboBox;
            cmbComplPkg.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }
        #endregion

        #region DROPDOWN SELECTEDINDEX CHANGED EVENTS

        protected void ddlTenant_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindDropDownControl();
            grdBkgComplPkgDataMap.EditIndexes.Clear();
            ViewState["ReBindGrid"] = "False";
            //ResetGridFilters(); //UAT-3874
            ClearViewStatesForFilter();

        }

        //UAT-1451
        protected void cmbCompPkg_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            WclComboBox cmbCompPkg = sender as WclComboBox;
            BindComplianceCategory(cmbCompPkg);
            ShowHideDataPointDiv(cmbCompPkg);
        }

        protected void cmbBkgPkg_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            WclComboBox cmbBkgPkg = sender as WclComboBox;
            ShowHideDataPointDiv(cmbBkgPkg);
        }

        //protected void cmbBkgPackages_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    if (cmbComplPackages.SelectedIndex > 0)
        //    {
        //        grdBkgComplPkgDataMap.EditIndexes.Clear();
        //        grdBkgComplPkgDataMap.Rebind();
        //    }
        //}

        protected void cmbDataPoints_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            WclComboBox cmbDataPoints = sender as WclComboBox;
            String dataPoint = cmbDataPoints.SelectedValue;
            //Reset All the Dropdowns on change in DataPoint
            BindDropDownDependentOnDataPoint(cmbDataPoints, dataPoint);
        }

        protected void cmbServiceGroup_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            WclComboBox cmbServiceGroup = sender as WclComboBox;
            BindServicesFromServiceGroup(cmbServiceGroup);
        }

        protected void cmbServices_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }

        protected void cmbCatagory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

            WclComboBox cmbCatagory = sender as WclComboBox;

            BindDropdownRelatedToCatagory(cmbCatagory);
        }

        protected void cmbItems_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            WclComboBox cmbItems = sender as WclComboBox;
            BindDropdownRelatedToItems(cmbItems);
        }

        protected void cmbAttributes_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            WclComboBox cmbAttribute = sender as WclComboBox;
            BindDropdownsRelatedToAttributes(cmbAttribute);
        }

        protected void cmbFlagged_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }

        protected void cmbNonFlagged_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }
        #endregion

        #endregion

        #region PRIVATE METHODS

        private void BindTenant()
        {
            Presenter.GetTenants();
            ddlTenant.DataSource = CurrentViewContext.ListTenants;
            ddlTenant.DataBind();
            if (Presenter.IsDefaultTenant)
            {
                ddlTenant.Enabled = true;
                ddlTenant.SelectedValue = AppConsts.ZERO;
                CurrentViewContext.SelectedTenantId = 0;
            }
            else
            {
                ddlTenant.Enabled = false;
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
                ddlTenant.SelectedValue = CurrentViewContext.SelectedTenantId.ToString();
                BindDropDownControl();
            }
        }

        private BkgComplPkgDataMappingContract GetDataForMapping(GridCommandEventArgs e)
        {
            BkgComplPkgDataMappingContract bkgComplPkgDataMappingContract = new BkgComplPkgDataMappingContract();
            WclComboBox cmbDataPoints = (e.Item.FindControl("cmbDataPoints") as WclComboBox);
            WclComboBox cmbBkgPkg = (e.Item.FindControl("cmbBkgPkg") as WclComboBox);
            WclComboBox cmbComplPkg = (e.Item.FindControl("cmbComplPkg") as WclComboBox);

            bkgComplPkgDataMappingContract.DataPointCode = cmbDataPoints.SelectedValue;
            bkgComplPkgDataMappingContract.BkgPackageID = Convert.ToInt32(cmbBkgPkg.SelectedValue);
            bkgComplPkgDataMappingContract.ComplPackageID = Convert.ToInt32(cmbComplPkg.SelectedValue);

            WclComboBox cmbServiceGroup = (e.Item.FindControl("cmbServiceGroup") as WclComboBox);
            bkgComplPkgDataMappingContract.ServiceGroupID = string.IsNullOrEmpty(cmbServiceGroup.SelectedValue) ? null : cmbServiceGroup.SelectedValue.To<Int32?>();

            WclComboBox cmbServices = (e.Item.FindControl("cmbServices") as WclComboBox);
            bkgComplPkgDataMappingContract.ServiceID = string.IsNullOrEmpty(cmbServices.SelectedValue) ? null : cmbServices.SelectedValue.To<Int32?>();

            WclComboBox cmbCatagory = (e.Item.FindControl("cmbCatagory") as WclComboBox);
            bkgComplPkgDataMappingContract.CatagoryID = string.IsNullOrEmpty(cmbCatagory.SelectedValue) ? null : cmbCatagory.SelectedValue.To<Int32?>();

            WclComboBox cmbItems = (e.Item.FindControl("cmbItems") as WclComboBox);
            bkgComplPkgDataMappingContract.ItemID = string.IsNullOrEmpty(cmbItems.SelectedValue) ? null : cmbItems.SelectedValue.To<Int32?>();

            WclComboBox cmbAttributes = (e.Item.FindControl("cmbAttributes") as WclComboBox);
            bkgComplPkgDataMappingContract.AttributeID = string.IsNullOrEmpty(cmbAttributes.SelectedValue) ? null : cmbAttributes.SelectedValue.To<Int32?>();

            WclComboBox cmbFlagged = (e.Item.FindControl("cmbFlagged") as WclComboBox);
            bkgComplPkgDataMappingContract.FlaggedValue = cmbFlagged.SelectedValue;

            WclComboBox cmbNonFlagged = (e.Item.FindControl("cmbNonFlagged") as WclComboBox);
            bkgComplPkgDataMappingContract.NonFlaggedValue = cmbNonFlagged.SelectedValue;
            return bkgComplPkgDataMappingContract;
        }

        private void BindDropDownDependentOnDataPoint(WclComboBox cmbDataPoints, String dataPoint, BkgCompliancePackageMappingSearchData bkgCompliancePackageMapping = null)
        {
            ResetDropdowns(cmbDataPoints);
            if (cmbDataPoints.SelectedIndex == AppConsts.NONE)
            {
                ShowSvcGrpPanel(cmbDataPoints, false);
            }
            else
            {
                switch (dataPoint)
                {
                    case "AAAA":
                        //ServiceGroup Only

                        ShowSvcGrpPanel(cmbDataPoints, true);
                        ShowHideSvcPnl(cmbDataPoints, true);
                        ShowServiceGroup(cmbDataPoints, bkgCompliancePackageMapping);
                        ShowHideServiceDiv(cmbDataPoints, false);
                        HideOptionDiv(cmbDataPoints);
                        break;

                    case "AAAB":
                        //ServiceGroup and Service
                        ShowSvcGrpPanel(cmbDataPoints, true);
                        ShowHideSvcPnl(cmbDataPoints, true);
                        ShowServiceGroup(cmbDataPoints, bkgCompliancePackageMapping);
                        ShowHideServiceDiv(cmbDataPoints, true);
                        HideOptionDiv(cmbDataPoints);
                        break;

                    case "AAAC":
                        ShowSvcGrpPanel(cmbDataPoints, true);
                        ShowHideSvcPnl(cmbDataPoints, false);
                        HideOptionDiv(cmbDataPoints);
                        break;

                    case "AAAD":
                        //ServiceGroup Only
                        ShowSvcGrpPanel(cmbDataPoints, true);
                        ShowHideSvcPnl(cmbDataPoints, true);
                        ShowServiceGroup(cmbDataPoints, bkgCompliancePackageMapping);
                        ShowHideServiceDiv(cmbDataPoints, false);
                        HideOptionDiv(cmbDataPoints);
                        break;

                    case "AAAE":
                        //Hide All Divs
                        ShowSvcGrpPanel(cmbDataPoints, true);
                        ShowHideSvcPnl(cmbDataPoints, false);
                        HideOptionDiv(cmbDataPoints);
                        break;

                    case "AAAF":
                        //ServiceGroup Only
                        ShowSvcGrpPanel(cmbDataPoints, true);
                        ShowHideSvcPnl(cmbDataPoints, true);
                        ShowServiceGroup(cmbDataPoints, bkgCompliancePackageMapping);
                        ShowHideServiceDiv(cmbDataPoints, false);
                        HideOptionDiv(cmbDataPoints);
                        break;

                    case "AAAG":
                        //ServiceGroup and Service
                        ShowSvcGrpPanel(cmbDataPoints, true);
                        ShowHideSvcPnl(cmbDataPoints, true);
                        ShowServiceGroup(cmbDataPoints, bkgCompliancePackageMapping);
                        ShowHideServiceDiv(cmbDataPoints, true);
                        HideOptionDiv(cmbDataPoints);
                        break;
                }
            }
        }

        private void ShowServiceGroup(WclComboBox cmbDataPoints, BkgCompliancePackageMappingSearchData bkgComplianceMapping)
        {
            HtmlGenericControl dvServiceGroup = cmbDataPoints.Parent.NamingContainer.FindControl("dvServiceGrp") as HtmlGenericControl;
            dvServiceGroup.Visible = true;
            WclComboBox cmbServiceGroup = cmbDataPoints.Parent.NamingContainer.FindControl("cmbServiceGroup") as WclComboBox;
            WclComboBox cmbBkgPkg = cmbDataPoints.Parent.NamingContainer.FindControl("cmbBkgPkg") as WclComboBox;
            Int32 selectedPackageId = 0;
            if (!_flag)
            {
                //selectedPackageId = cmbBkgPackages.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbBkgPackages.SelectedValue);
                selectedPackageId = cmbBkgPkg.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbBkgPkg.SelectedValue);
            }
            else
            {
                selectedPackageId = bkgComplianceMapping.BCPM_BkgPackageID;
                cmbBkgPkg.SelectedValue = Convert.ToString(selectedPackageId);
            }

            cmbServiceGroup.DataSource = Presenter.GetServiceGroups(selectedPackageId);
            cmbServiceGroup.DataBind();
            if (bkgComplianceMapping != null)
            {
                cmbServiceGroup.SelectedValue = bkgComplianceMapping.BCPM_BkgSvcGroupID > AppConsts.NONE ? Convert.ToString(bkgComplianceMapping.BCPM_BkgSvcGroupID) : null;
                BindServicesFromServiceGroup(cmbServiceGroup, bkgComplianceMapping);
            }
        }

        private void BindServicesFromServiceGroup(WclComboBox cmbServiceGroup, BkgCompliancePackageMappingSearchData bkgComplianceMapping = null)
        {
            WclComboBox cmbService = cmbServiceGroup.Parent.NamingContainer.FindControl("cmbServices") as WclComboBox;
            WclComboBox cmbBkgPkg = cmbServiceGroup.Parent.NamingContainer.FindControl("cmbBkgPkg") as WclComboBox;
            Int32 selectedServiceGrp = cmbServiceGroup.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbServiceGroup.SelectedValue);
            //Int32 selectedBkgPkgID = cmbBkgPackages.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbBkgPackages.SelectedValue);
            Int32 selectedBkgPkgID = cmbBkgPkg.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbBkgPkg.SelectedValue);
            if (bkgComplianceMapping.IsNotNull())
            {
                selectedServiceGrp = bkgComplianceMapping.BCPM_BkgSvcGroupID;
                selectedBkgPkgID = bkgComplianceMapping.BCPM_BkgPackageID;
            }

            cmbService.DataSource = Presenter.GetServices(selectedServiceGrp, selectedBkgPkgID);
            cmbService.DataBind();
            if (bkgComplianceMapping.IsNotNull())
            {
                cmbService.SelectedValue = bkgComplianceMapping.BCPM_BkgSvcID > AppConsts.NONE ? Convert.ToString(bkgComplianceMapping.BCPM_BkgSvcID) : null;
            }
        }

        private void BindDropdownRelatedToCatagory(WclComboBox cmbCatagory, BkgCompliancePackageMappingSearchData bkgComplianceMapping = null)
        {
            WclComboBox cmbAttributes = cmbCatagory.Parent.NamingContainer.FindControl("cmbAttributes") as WclComboBox;
            cmbAttributes.Items.Clear();
            HtmlGenericControl dvOption = cmbCatagory.Parent.NamingContainer.FindControl("dvOption") as HtmlGenericControl;
            dvOption.Visible = false;
            WclComboBox cmbItem = cmbCatagory.Parent.NamingContainer.FindControl("cmbItems") as WclComboBox;
            Int32 selectedCatagoryID = cmbCatagory.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbCatagory.SelectedValue);
            if (bkgComplianceMapping.IsNotNull())
            {
                selectedCatagoryID = bkgComplianceMapping.BCPM_ComplianceCategoryID;
            }
            cmbItem.DataSource = Presenter.GetCatagoryItems(selectedCatagoryID);
            cmbItem.DataBind();
            if (bkgComplianceMapping.IsNotNull())
            {
                cmbItem.SelectedValue = bkgComplianceMapping.BCPM_ComplianceItemID > AppConsts.NONE ? Convert.ToString(bkgComplianceMapping.BCPM_ComplianceItemID) : null;
                BindDropdownRelatedToItems(cmbItem, bkgComplianceMapping);
            }
        }

        private void BindDropdownRelatedToItems(WclComboBox cmbItems, BkgCompliancePackageMappingSearchData bkgComplianceMapping = null)
        {
            WclComboBox cmbAttributes = cmbItems.Parent.NamingContainer.FindControl("cmbAttributes") as WclComboBox;
            Int32 selectedItemID = cmbItems.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbItems.SelectedValue);
            WclComboBox cmbDataPoint = cmbItems.Parent.NamingContainer.FindControl("cmbDataPoints") as WclComboBox;
            String dataPointCode = cmbDataPoint.SelectedValue;
            if (bkgComplianceMapping.IsNotNull())
            {
                selectedItemID = bkgComplianceMapping.BCPM_ComplianceItemID;
            }
            cmbAttributes.DataSource = Presenter.GetComplianceItemAttributes(selectedItemID, dataPointCode);
            cmbAttributes.DataBind();
            if (bkgComplianceMapping.IsNotNull())
            {
                cmbAttributes.SelectedValue = bkgComplianceMapping.BCPM_ComplianceAttributeID > AppConsts.NONE ? Convert.ToString(bkgComplianceMapping.BCPM_ComplianceAttributeID) : null;
                BindDropdownsRelatedToAttributes(cmbAttributes, bkgComplianceMapping);
            }
        }

        private void BindDropdownsRelatedToAttributes(WclComboBox cmbAttribute, BkgCompliancePackageMappingSearchData bkgComplianceMapping = null)
        {
            WclComboBox cmbFlagged = cmbAttribute.Parent.NamingContainer.FindControl("cmbFlagged") as WclComboBox;
            WclComboBox cmbNonFlagged = cmbAttribute.Parent.NamingContainer.FindControl("cmbNonFlagged") as WclComboBox;
            Int32 cmbAttributeSelectedValue = cmbAttribute.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbAttribute.SelectedValue);
            List<Entity.ClientEntity.ComplianceAttributeOption> complianceAttributeList = Presenter.GetComplianceAttributeOption(cmbAttributeSelectedValue);
            var complianceAttributeListFiltered = complianceAttributeList.Where(cond => cond.ComplianceItemAttributeID == cmbAttributeSelectedValue);

            cmbFlagged.DataSource = complianceAttributeListFiltered; //complianceAttributeList;
            cmbFlagged.DataBind();

            cmbNonFlagged.DataSource = complianceAttributeListFiltered; // complianceAttributeList;
            cmbNonFlagged.DataBind();


            WclComboBox cmbDataPoint = cmbAttribute.Parent.NamingContainer.FindControl("cmbDataPoints") as WclComboBox;
            String dataPointCode = cmbDataPoint.SelectedValue;
            if (dataPointCode == "AAAC" || dataPointCode == "AAAD" || dataPointCode == "AAAG")
            {
                HtmlGenericControl dvOption = cmbAttribute.Parent.NamingContainer.FindControl("dvOption") as HtmlGenericControl;
                dvOption.Visible = true;
                if (bkgComplianceMapping.IsNotNull())
                {
                    List<BkgCompliancePkgMappingAttrOption> saveOptions = Presenter.FetchBkgCompliancePkgMappingAttrOptions(bkgComplianceMapping.BCPM_ID).ToList();
                    if (saveOptions.IsNotNull())
                    {
                        foreach (var item in saveOptions)
                        {
                            if (item.BCPAO_BKgValue == "1")
                            {
                                cmbFlagged.SelectedValue = Convert.ToString(complianceAttributeListFiltered.FirstOrDefault(x => x.OptionValue == item.BCPAO_ComplianceAttrOptionValue && item.BCPAO_BKgValue == Convert.ToString(AppConsts.ONE)).OptionValue);
                            }
                            if (item.BCPAO_BKgValue == "0")
                            {
                                cmbNonFlagged.SelectedValue = Convert.ToString(complianceAttributeListFiltered.FirstOrDefault(x => x.OptionValue == item.BCPAO_ComplianceAttrOptionValue && item.BCPAO_BKgValue == Convert.ToString(AppConsts.NONE)).OptionValue);
                            }
                        }
                    }

                }
            }
        }

        private static void ShowHideServiceDiv(WclComboBox cmbDataPoints, Boolean isVisiable)
        {
            HtmlGenericControl dvService = cmbDataPoints.Parent.NamingContainer.FindControl("dvService") as HtmlGenericControl;
            dvService.Visible = isVisiable;
            if (!isVisiable)
            {
                WclComboBox cmbService = cmbDataPoints.Parent.NamingContainer.FindControl("cmbServices") as WclComboBox;
                cmbService.Items.Clear();
            }
        }

        private static void ShowSvcGrpPanel(WclComboBox cmbDataPoints, Boolean IsVisible)
        {
            HtmlGenericControl dvSvcGrpPnl = cmbDataPoints.Parent.NamingContainer.FindControl("dvSvcGrpPnl") as HtmlGenericControl;
            dvSvcGrpPnl.Visible = IsVisible;
        }

        private static void ResetDropdowns(WclComboBox cmbDataPoints)
        {
            WclComboBox cmbServiceGroup = cmbDataPoints.Parent.NamingContainer.FindControl("cmbServiceGroup") as WclComboBox;
            cmbServiceGroup.SelectedIndex = 0;
            WclComboBox cmbCatagory = cmbDataPoints.Parent.NamingContainer.FindControl("cmbCatagory") as WclComboBox;
            cmbCatagory.SelectedIndex = 0;
            WclComboBox cmbService = cmbDataPoints.Parent.NamingContainer.FindControl("cmbServices") as WclComboBox;
            cmbService.Items.Clear();
            WclComboBox cmbItems = cmbDataPoints.Parent.NamingContainer.FindControl("cmbItems") as WclComboBox;
            cmbItems.Items.Clear();
            WclComboBox cmbAttributes = cmbDataPoints.Parent.NamingContainer.FindControl("cmbAttributes") as WclComboBox;
            cmbAttributes.Items.Clear();
        }

        private static void HideOptionDiv(WclComboBox cmbDataPoints)
        {
            HtmlGenericControl dvOption = cmbDataPoints.Parent.NamingContainer.FindControl("dvOption") as HtmlGenericControl;
            dvOption.Visible = false;
        }

        private static void ShowHideSvcPnl(WclComboBox cmbDataPoints, Boolean isVisible)
        {
            HtmlGenericControl dvSvcPnel = cmbDataPoints.Parent.NamingContainer.FindControl("dvSvcPnl") as HtmlGenericControl;
            if (dvSvcPnel.IsNotNull())
                dvSvcPnel.Visible = isVisible;
        }

        /// <summary>
        /// Method to bind background and compliance packages drop down
        /// </summary>
        private void BindDropDownControl()
        {
            Presenter.GetBackgroundPackages();
            Presenter.GetCompliancePackage();
            cmbBkgPackagesSearch.DataSource = CurrentViewContext.lstBackgroundPackage;
            cmbBkgPackagesSearch.DataBind();
            if (cmbBkgPackagesSearch.Items.Count >= 10)
            {
                cmbBkgPackagesSearch.Height = Unit.Pixel(200);
            }

            cmbComplPackagesSearch.DataSource = CurrentViewContext.lstCompliancePackage;
            cmbComplPackagesSearch.DataBind();
            if (cmbComplPackagesSearch.Items.Count >= 10)
            {
                cmbComplPackagesSearch.Height = Unit.Pixel(200);
            }
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdBkgComplPkgDataMap.MasterTableView.SortExpressions.Clear();
            grdBkgComplPkgDataMap.CurrentPageIndex = 0;
            grdBkgComplPkgDataMap.MasterTableView.CurrentPageIndex = 0;
            grdBkgComplPkgDataMap.MasterTableView.IsItemInserted = false;
            grdBkgComplPkgDataMap.Rebind();
        }

        private void ResetControls()
        {
            if (Presenter.IsDefaultTenant)
            {
                cmbBkgPackagesSearch.DataSource = new List<BackgroundPackage>();
                cmbBkgPackagesSearch.DataBind();
                cmbComplPackagesSearch.DataSource = new List<ComplaincePackageDetails>();
                cmbComplPackagesSearch.DataBind();
            }
            ClearViewStatesForFilter();
            foreach (GridColumn column in grdBkgComplPkgDataMap.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            grdBkgComplPkgDataMap.MasterTableView.FilterExpression = string.Empty;

        }

        private void BindComplianceCategory(WclComboBox cmbcompPkg)
        {
            WclComboBox cmbCatagories = cmbcompPkg.Parent.NamingContainer.FindControl("cmbCatagory") as WclComboBox;
            cmbCatagories.Items.Insert(0, new RadComboBoxItem("--Select--"));
            Int32 selectedCompPkgId = cmbcompPkg.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbcompPkg.SelectedValue);
            cmbCatagories.DataSource = Presenter.GetComplianceCatagories(selectedCompPkgId);
            cmbCatagories.DataBind();
        }

        private void ShowHideDataPointDiv(WclComboBox comboBox)
        {
            HtmlGenericControl divDataPoints = comboBox.Parent.NamingContainer.FindControl("divDataPoints") as HtmlGenericControl;
            WclComboBox cmbComplPkg = comboBox.Parent.NamingContainer.FindControl("cmbComplPkg") as WclComboBox;
            WclComboBox cmbBkgPkg = comboBox.Parent.NamingContainer.FindControl("cmbBkgPkg") as WclComboBox;
            WclComboBox cmbDataPoints = comboBox.Parent.NamingContainer.FindControl("cmbDataPoints") as WclComboBox;
            if (!cmbComplPkg.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(cmbComplPkg.SelectedValue) > AppConsts.NONE
                && !cmbBkgPkg.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(cmbBkgPkg.SelectedValue) > AppConsts.NONE
                )
            {
                divDataPoints.Visible = true;
                cmbDataPoints.SelectedValue = AppConsts.ZERO;
            }
            else
            {
                divDataPoints.Visible = false;
            }
            ShowSvcGrpPanel(comboBox, false);
            ShowHideSvcPnl(comboBox, false);
            HideOptionDiv(comboBox);
        }

        public void ClearViewStatesForFilter()
        {
            ViewState["FilterColumns"] = CurrentViewContext.GridCustomPaging.FilterColumns = null;
            ViewState["FilterOperators"] = CurrentViewContext.GridCustomPaging.FilterOperators = null;
            ViewState["FilterValues"] = CurrentViewContext.GridCustomPaging.FilterValues = null;
            ViewState["FilterTypes"] = CurrentViewContext.GridCustomPaging.FilterTypes = null;
        }
        #endregion

        //UAT-2319
        protected void btnDataSync_Click(object sender, EventArgs e)
        {
            //check the value of key in AppConfiguration table if it is ) i.e. false than only bkg proces starts otherwise it is considered that already process is in progress.
            if (Presenter.GetAppConfiguration(AppConsts.Background_Data_Sync_In_Progress))
            {
                //when data sync is started we change the value of key to 1 in AppConfiguration table.
                Presenter.UpdateAppConfiguration(AppConsts.Background_Data_Sync_In_Progress, Convert.ToString(AppConsts.ONE));
                base.ShowSuccessMessage("Data synchronization has been initiated and it will take few minutes to copy the data from background to compliance");

                //calls parallel  task for data sync.
                Presenter.SyncDataForNewMapping();

                //after sucessfully data sync the value of key is again set to zero in AppConfiguration table in security db.
                // Presenter.UpdateAppConfiguration(AppConsts.Background_Data_Sync_In_Progress, AppConsts.ZERO);
            }
            else
            {
                base.ShowErrorInfoMessage("Data synchronization is already in progress,so please try again after some time.");
            }
        }

    }
}