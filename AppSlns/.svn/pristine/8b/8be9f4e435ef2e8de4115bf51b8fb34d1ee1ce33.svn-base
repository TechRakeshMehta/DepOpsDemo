#region Namespaces

#region System Defined
using System;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;
#endregion

#region Application Specific
using INTSOF.Utils;
using Telerik.Web.UI;
using Entity.ClientEntity;
using CoreWeb.Shell;
using System.Xml.Serialization;
using System.IO;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Text;
using System.Threading;
using System.Data;
using CoreWeb.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public partial class ApplicantPortFolioSearch_Copy : BaseUserControl, IApplicantPortFolioSearch_CopyView
    {
        #region Variables

        private ApplicantPortFolioSearch_CopyPresenter _presenter = new ApplicantPortFolioSearch_CopyPresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private CustomPagingArgsContract _gridCustomPaging = null;
        private SearchItemDataContract _gridSearchContract = null;

        #endregion

        #region Properties


        public ApplicantPortFolioSearch_CopyPresenter Presenter
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
                if (ddlTenantName.SelectedValue.IsNullOrEmpty())
                    return AppConsts.DEFAULT_SELECTED_TENANTID;
                return Convert.ToInt32(ddlTenantName.SelectedValue);
            }
            set
            {
                if (value >= AppConsts.NONE)
                {
                    ddlTenantName.SelectedValue = value.ToString();
                }
                else
                {
                    ddlTenantName.SelectedIndex = AppConsts.NONE;
                }
            }
        }

        public Int32? OrganizationUserID
        {
            get
            {
                if (String.IsNullOrEmpty(txtUserID.Text))
                    return AppConsts.NONE;
                return Convert.ToInt32(txtUserID.Text);
            }
            set
            {
                if (value == null)
                    txtUserID.Text = String.Empty;
                else
                    txtUserID.Text = value.ToString();
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (tenantId == 0)
                {
                    //tenantId = Presenter.GetTenantId();
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

        public Int32 FilterUserGroupId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlUserGroup.SelectedValue))
                    return AppConsts.NONE;
                return Convert.ToInt32(ddlUserGroup.SelectedValue);
            }
            set
            {
                if (value > AppConsts.NONE)
                {
                    ddlUserGroup.SelectedValue = value.ToString();
                }
                else
                {
                    ddlUserGroup.SelectedIndex = value;
                }
            }
        }

        public Int32 MatchUserGroupId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlUserGroup.SelectedValue))
                    return AppConsts.NONE;
                return Convert.ToInt32(ddlUserGroup.SelectedValue);
            }
            set
            {
                if (value > AppConsts.NONE)
                {
                    ddlUserGroup.SelectedValue = value.ToString();
                }
                else
                {
                    ddlUserGroup.SelectedIndex = value;
                }
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public IApplicantPortFolioSearch_CopyView CurrentViewContext
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

        public String EmailAddress
        {
            get
            {
                return txtEmail.Text;
            }
            set
            {
                txtEmail.Text = value;
            }
        }

        public String SSN
        {
            get
            {
                return txtSSN.Text;
            }
            set
            {
                txtSSN.Text = value;
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

        public List<ApplicantDataList> ApplicantSearchData
        {
            get;
            set;
        }

        public List<UserGroup> lstUserGroup
        {
            get;
            set;
        }

        public Int32 UserGroupId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlUserGroup.SelectedValue))
                    return AppConsts.NONE;
                return Convert.ToInt32(ddlUserGroup.SelectedValue);
            }
            set
            {
                if (value > AppConsts.NONE)
                {
                    ddlUserGroup.SelectedValue = value.ToString();
                }
                else
                {
                    ddlUserGroup.SelectedIndex = value;
                }
            }
        }

        public Int32 DPM_ID
        {
            get;
            set;
        }

        public String CustomFields
        {
            get;
            set;
        }

        public Int32 GetSearchScopeType
        {
            get
            {
                if (SelectedTenantId == AppConsts.NONE)
                {
                    if (Convert.ToInt32(cmbSearchType.SelectedValue) == AppConsts.NONE)
                    {
                        return SearchScope.AllTenantsAsynch;
                    }
                    else
                    {
                        return SearchScope.AllTenantsSynch;
                    }
                }
                else
                {
                    if (Convert.ToInt32(cmbSearchType.SelectedValue) == AppConsts.NONE)
                    {
                        return SearchScope.SingleTenantAsynch;
                    }
                    else
                    {
                        return SearchScope.SingleTenantSynch;
                    }
                }
            }
        }

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

                grdApplicantSearchData.MasterTableView.CurrentPageIndex = value - 1;

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
                return grdApplicantSearchData.PageSize;
            }
            set
            {
                grdApplicantSearchData.PageSize = value;
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

        /// <summary>
        /// To set shared class object of search contract
        /// </summary>
        public SearchItemDataContract SetSearchItemDataContract
        {
            set
            {
                var serializer = new XmlSerializer(typeof(SearchItemDataContract));
                var sb = new StringBuilder();
                using (TextWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, value);
                }
                //Session for maintaning Grid Filter, Paging and Index
                Session[AppConsts.APPLICANT_SEARCH_GRID_SESSION_KEY] = sb.ToString();
            }
        }

        /// <summary>
        /// To get shared class object of search contract
        /// </summary>
        public SearchItemDataContract GetSearchItemDataContract
        {
            get
            {
                if (_gridSearchContract.IsNull())
                {
                    var serializer = new XmlSerializer(typeof(SearchItemDataContract));
                    TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.APPLICANT_SEARCH_GRID_SESSION_KEY]));
                    _gridSearchContract = (SearchItemDataContract)serializer.Deserialize(reader);
                }
                return _gridSearchContract;
            }
        }

        /// <summary>
        /// To set Applicant Search Data
        /// </summary>
        public DataTable SetApplicantSearchData
        {
            set
            {
                if (value.IsNotNull())
                {
                    grdApplicantSearchData.DataSource = value;
                }
                else
                {
                    grdApplicantSearchData.DataSource = new DataTable();
                }
            }
        }


        /// <summary>
        /// To set or get Search Instance Id
        /// </summary>
        public Int32 SearchInstanceId
        {
            get
            {
                if (!ViewState["SearchInstanceId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SearchInstanceId"]);
                }

                return AppConsts.DEFAULT_SEARCH_INSTANCEID;
            }
            set
            {
                ViewState["SearchInstanceId"] = value;
            }
        }

        /// <summary>
        /// To set or get tab index of master page.
        /// </summary>
        public Int16 MasterPageTabIndex
        {
            get
            {
                if (!ViewState["MasterPageTabIndex"].IsNull())
                {
                    return Convert.ToInt16(ViewState["MasterPageTabIndex"]);
                }

                return AppConsts.NONE;
            }
            set
            {
                ViewState["MasterPageTabIndex"] = value;
            }
        }

        /// <summary>
        /// To set or get error message
        /// </summary>
        public String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// To Show CommandBar
        /// </summary>
        public Boolean IsOfflineMode
        {
            set
            {
                if (value)
                {
                    fsucCmdBarButton.Visible = false;
                    ddlTenantName.Enabled = false;
                    cmbSearchType.Enabled = false;
                    ddlUserGroup.Enabled = false;
                    dpkrDOB.Enabled = false;
                    txtEmail.Enabled = false;
                    txtFirstName.Enabled = false;
                    txtLastName.Enabled = false;
                    txtSSN.Enabled = false;
                    txtUserID.Enabled = false;
                    grdApplicantSearchData.MasterTableView.CommandItemSettings.ShowRefreshButton = false;
                }
            }
        }

        /// <summary>
        /// To populate tenant dropdown
        /// </summary>
        public List<Entity.Tenant> TenantDropdownDataSource
        {
            set
            {
                ddlTenantName.DataSource = value;
                ddlTenantName.DataBind();
            }
        }

        /// <summary>
        /// To populate UserGroup List
        /// </summary>
        public List<UserGroup> UserGroupListDataSource
        {
            set
            {
                ddlUserGroup.DataSource = value;
                ddlUserGroup.DataBind();
            }
        }

        /// <summary>
        /// To populate Search Type dropdown
        /// </summary>
        public Dictionary<Int32, String> SearchTypeDataSource
        {
            set
            {
                if (value.Count > AppConsts.NONE)
                {
                    cmbSearchType.DataTextField = "Value";
                    cmbSearchType.DataValueField = "Key";
                    cmbSearchType.DataSource = value;
                    cmbSearchType.DataBind();
                }

                if (fsucCmdBarButton.Visible == false)
                    cmbSearchType.SelectedValue = 0.ToString();
            }
        }

        /// <summary>
        /// To populate Search Type dropdown
        /// </summary>
        public Boolean RebindDatagrid
        {
            set
            {
                if (value)
                    grdApplicantSearchData.Rebind();
            }
        }

        /// <summary>
        /// To reset page controls
        /// </summary>
        public Boolean ResetPageControlsOffline
        {
            set
            {
                SearchInstanceId = AppConsts.DEFAULT_SEARCH_INSTANCEID;
                BindUserGroups();
                BindSearchModeList();
                ApplicantFirstName = String.Empty;
                ApplicantLastName = String.Empty;
                OrganizationUserID = null;
                EmailAddress = String.Empty;
                SSN = String.Empty;
                DateOfBirth = null;
                DPM_ID = AppConsts.NONE;
                CustomFields = String.Empty;
                MatchUserGroupId = AppConsts.NONE;
                FilterUserGroupId = AppConsts.NONE;
                ResetGridFilters();
            }
        }

        public Boolean SetuserContrOnLoad
        {
            set
            {
                SetUserControlOnLoad();
            }
        }

        //public Int32 CustomAttributeTenantId
        //{
        //    set
        //    {
        //        if (value > 0)
        //            ucCustomAttributeLoaderSearch.TenantId = value;
        //    }
        //}


        /// <summary>
        /// To determine online or offline mode .
        /// </summary>
        public Boolean IsOnlineUserControl
        {
            get
            {
                return fsucCmdBarButton.Visible;
            }
        }

        public SearchItemDataContract GetSessionValues
        {
            set
            {
                if (SearchInstanceId == -1 || SearchInstanceId == 0 || MasterPageTabIndex == 1)
                {
                    SelectedTenantId = value.ClientID;
                }
                else
                {
                    SelectedTenantId = 0;
                }
                ApplicantFirstName = value.ApplicantFirstName;
                ApplicantLastName = value.ApplicantLastName;
                EmailAddress = value.EmailAddress;
                DateOfBirth = value.DateOfBirth;
                SSN = value.ApplicantSSN;
                if (value.OrganizationUserId != null && value.OrganizationUserId > 0)
                    OrganizationUserID = value.OrganizationUserId;
                CustomFields = value.CustomFields;
                DPM_ID = value.DPM_Id ?? 0;
                BindUserGroups();
                FilterUserGroupId = value.FilterUserGroupID ?? 0;
                MatchUserGroupId = value.MatchUserGroupID ?? 0;
                CurrentViewContext.GridCustomPaging = value.GridCustomPagingArguments;
                //Reset session
                Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
                BindSearchModeList();
            }
        }
        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Applicant Portfolio Search Copy";
                base.SetPageTitle("Applicant Portfolio Search Copy");

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
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            //To reset grid filters 
            SearchInstanceId = AppConsts.DEFAULT_SEARCH_INSTANCEID;
            ResetGridFilters();
            if (cmbSearchType.SelectedValue == Convert.ToInt32(MasterSearchMode.Offline).ToString() && SearchInstanceId != AppConsts.DEFAULT_SEARCH_INSTANCEID)
            {
                String message = "Offilne search has been initiated and will be completed in a while. For the results please click on tab 'Offline Search Results'.";
                ShowSuccessMessage(message);
            }
        }

        /// <summary>
        /// To reset controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            ResetPageControls();
        }

        /// <summary>
        /// Redirect to Home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            //Reset session
            Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
        }

        #region Grid Events

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
                CurrentViewContext.GridCustomPaging = GridCustomPaging;
                if (GridCustomPaging.SortExpression.IsNullOrEmpty())
                    GridCustomPaging.SortExpression = null;
                //CurrentViewContext.DPM_ID = ucCustomAttributeLoaderSearch.DPM_ID;
                //CurrentViewContext.CustomFields = ucCustomAttributeLoaderSearch.GetCustomDataXML();
                Presenter.PerformSearch();
                if (!ErrorMessage.IsNullOrEmpty())
                    ShowErrorInfoMessage(ErrorMessage);
                //To set controls values in session
                SetSessionValues();
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
        /// Grid Item DataBound Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                dataItem["SSN"].Text = Presenter.GetFormattedSSN(Convert.ToString(dataItem["SSN"].Text));
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
                #region DetailScreenNavigation

                if (e.CommandName.Equals("ViewDetail"))
                {
                    Int32 selectedTenantId = 0;
                    Int16 masterPageTabIndex = AppConsts.ONLINE_SEARCH_TABINDEX;
                    //SetSessionValues();
                    if (ddlTenantName.SelectedValue.IsNullOrEmpty())
                    {
                        selectedTenantId = TenantId;
                    }
                    if (IsOnlineUserControl == false)
                    {
                        masterPageTabIndex = AppConsts.OFFLINE_SEARCH_TABINDEX;
                    }

                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String organizationUserId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"].ToString();
                    String tenantId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantId"].ToString();
                    Int32 searchInstanceId = SearchInstanceId == 0 ? -1 : SearchInstanceId;
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", tenantId },
                                                                    { "Child", ChildControls.ApplicantPortfolioDetailPage},
                                                                    { "OrganizationUserId", organizationUserId},
                                                                    { "PageType", "ApplicantPortfolioSearch_Copy"},
                                                                    { "SearchInstanceId", searchInstanceId.ToString()},
                                                                    { "MasterPageTabIndex", masterPageTabIndex.ToString()}
                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);

                }
                #endregion

                #region For Sort command

                if (!ViewState["SortExpression"].IsNull())
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
                }

                #endregion
                // Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdApplicantSearchData);
                }
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
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

        #endregion

        /// <summary>
        /// To bind Admin Program Study dropdown when Tenant Name changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindUserGroups();
            //if (ddlTenantName.SelectedIndex <= 0)
            //{
            //    ucCustomAttributeLoaderSearch.Reset();
            //}
            //else
            //{
            //    ucCustomAttributeLoaderSearch.Reset(SelectedTenantId);
            //}
        }

        /// <summary>
        /// Tenant Name DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            ddlTenantName.Items.Insert(0, new DropDownListItem("--Select--"));
            ddlTenantName.Items.Insert(1, new DropDownListItem("All", "0"));
        }

        /// <summary>
        /// Tenant Name DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUserGroup_DataBound(object sender, EventArgs e)
        {
            ddlUserGroup.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To bind controls
        /// </summary>
        private void BindControls()
        {
            if (Presenter.IsDefaultTenant && IsOnlineUserControl)
            {
                ddlTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantId = -1;
            }
            else
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
            }
            BindUserGroups();
            BindSearchModeList();
        }

        /// <summary>
        /// To bind program dropdown
        /// </summary>
        private void BindUserGroups()
        {
            if (SelectedTenantId > 0)
            {
                ddlUserGroup.Enabled = true;
                Presenter.GetAllUserGroups(SelectedTenantId);
            }
            else
            {
                ddlUserGroup.Enabled = false;
                UserGroupListDataSource = new List<UserGroup>();
            }

            if (!IsOnlineUserControl)
                ddlUserGroup.Enabled = false;
        }

        /// <summary>
        /// To set controls values in session
        /// </summary>
        private void SetSessionValues()
        {
            SearchItemDataContract searchDataContract = new SearchItemDataContract();

            searchDataContract.ClientID = SelectedTenantId;
            searchDataContract.ApplicantFirstName = ApplicantFirstName;
            searchDataContract.ApplicantLastName = ApplicantLastName;
            searchDataContract.EmailAddress = EmailAddress;
            searchDataContract.DateOfBirth = DateOfBirth;
            searchDataContract.ApplicantSSN = SSN;
            searchDataContract.OrganizationUserId = OrganizationUserID.Value;
            //searchDataContract.DPM_Id = ucCustomAttributeLoaderSearch.DPM_ID;
            //searchDataContract.CustomFields = ucCustomAttributeLoaderSearch.GetCustomDataXML();
            //searchDataContract.NodeLabel = ucCustomAttributeLoaderSearch.nodeLable;
            searchDataContract.GridCustomPagingArguments = GridCustomPaging;
            if (ddlUserGroup.SelectedIndex > 0)
            {
                searchDataContract.FilterUserGroupID = Convert.ToInt32(ddlUserGroup.SelectedValue);
                searchDataContract.MatchUserGroupID = searchDataContract.FilterUserGroupID;
            }
            var serializer = new XmlSerializer(typeof(SearchItemDataContract));
            var strbuilder = new StringBuilder();

            using (TextWriter writer = new StringWriter(strbuilder))
            {
                serializer.Serialize(writer, searchDataContract);
            }
            //Session for maintaining control values
            Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = strbuilder.ToString();
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdApplicantSearchData.MasterTableView.SortExpressions.Clear();
            grdApplicantSearchData.CurrentPageIndex = 0;
            grdApplicantSearchData.MasterTableView.CurrentPageIndex = 0;
            grdApplicantSearchData.Rebind();
        }

        private void BindSearchModeList()
        {
            Dictionary<Int32, String> searchModeList = new Dictionary<Int32, String>();
            searchModeList[Convert.ToInt32(MasterSearchMode.Online)] = MasterSearchMode.Online.ToString();
            searchModeList[Convert.ToInt32(MasterSearchMode.Offline)] = MasterSearchMode.Offline.ToString();
            SearchTypeDataSource = searchModeList;
        }

        private void ResetPageControls()
        {
            BindControls();
            txtUserID.Text = String.Empty;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            dpkrDOB.SelectedDate = null;
            txtEmail.Text = String.Empty;
            txtSSN.Text = String.Empty;
            //ucCustomAttributeLoaderSearch.NodeId = 0;
            //ucCustomAttributeLoaderSearch.Reset();
            //To reset grid filters 
            ResetGridFilters();
            //Reset session
            Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
        }

        private void SetUserControlOnLoad()
        {
            //Set MinDate and MaxDate for DOB
            dpkrDOB.MinDate = Convert.ToDateTime("01-01-1900");
            dpkrDOB.MaxDate = DateTime.Now;
            Presenter.GetTenants();
            BindControls();
        }

        #endregion

    }
}

