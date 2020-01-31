//UAT-259
#region NAMESPACES

#region SYSTEM_DEFINED
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Microsoft.Practices.ObjectBuilder;
#endregion

#region APPLICATION_SPECIFIC
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Telerik.Web.UI;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public partial class ClientUserSearch : BaseUserControl, IClientUserSearchView
    {
        #region VARIABLES

        #region PUBLIC VARIABLES
        #endregion

        #region PRIVATE VARIABLES
        private ClientUserSearchPresenter _presenter = new ClientUserSearchPresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private CustomPagingArgsContract _gridCustomPaging = null;
        private List<Int32> _selectedTenantIds;
        private List<int> _selectedAgencyIds;

        #endregion

        #endregion

        #region PROPERTIES

        #region PRIVATE PROPERTIES
        #endregion

        #region PUBLIC PROPERTIES

        public ClientUserSearchPresenter Presenter
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

        public Int32 TenantID
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

        public List<int> SelectedTenantIDs
        {
            get
            {
                _selectedTenantIds = new List<int>();
                foreach (RadComboBoxItem item in ddlTenantName.Items)
                {
                    if (item.Checked == true)
                        _selectedTenantIds.Add(Convert.ToInt32(item.Value));
                }
                return _selectedTenantIds;
            }
            set
            {
                _selectedTenantIds = value;
                foreach (RadComboBoxItem item in ddlTenantName.Items)
                {
                    if (_selectedTenantIds.Contains(Convert.ToInt32(item.Value)))
                        item.Checked = true;
                }
            }
        }

        public List<int> SelectedAgencyIDs
        {
            get
            {
                _selectedAgencyIds = new List<int>();
                //foreach (RadComboBoxItem item in cmbAgency.Items)
                //{
                //    if (item.Checked == true)
                //        _selectedAgencyIds.Add(Convert.ToInt32(item.Value));
                //}
                return _selectedAgencyIds;
            }
            set
            {
                _selectedAgencyIds = value;
                //foreach (RadComboBoxItem item in cmbAgency.Items)
                //{
                //    if (_selectedAgencyIds.Contains(Convert.ToInt32(item.Value)))
                //        item.Checked = true;
                //}
            }
        }

        //UAT-4257
        List<AgencyHierarchyContract> IClientUserSearchView.lstAgencyHierarchyRootNodes
        {
            get
            {
                if (!ViewState["lstAgencyHierarchyRootNodes"].IsNullOrEmpty())
                    return (ViewState["lstAgencyHierarchyRootNodes"]) as List<AgencyHierarchyContract>;
                return new List<AgencyHierarchyContract>();
            }
            set
            {
                ViewState["lstAgencyHierarchyRootNodes"] = value;
            }
        }

        List<Int32> IClientUserSearchView.lstSelectedAgencyHierarchyIDs
        {
            get
            {
                var _lstSelectedAgencyHierarchyIDs = cmbAgencyHierarchy.CheckedItems.Select(col => Convert.ToInt32(col.Value)).ToList();
                ViewState["lstSelectedAgencyHierarchyIDs"] = _lstSelectedAgencyHierarchyIDs;

                if (!ViewState["lstSelectedAgencyHierarchyIDs"].IsNullOrEmpty())
                    return (ViewState["lstSelectedAgencyHierarchyIDs"]) as List<Int32>;
                return new List<Int32>();
            }
            set
            {
                ViewState["lstSelectedAgencyHierarchyIDs"] = value;
                if (!value.IsNullOrEmpty())
                {
                    foreach (var item in value)
                    {
                        cmbAgencyHierarchy.FindItemByValue(item.ToString()).Checked = true;
                    }
                }
            }
        }

        public String SelectedAgecnyHierarchyIds
        {
            get
            {
                return ucAgencyHierarchyMultipleToSearchRotation.SelectedNodeIds;
            }
        }

        public String HierarchyNode
        {
            get
            {
                if (!String.IsNullOrEmpty(hdnDepartmntPrgrmMppng.Value))
                {
                    return hdnDepartmntPrgrmMppng.Value;
                }
                return String.Empty;
            }
        }

        public List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        public IClientUserSearchView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public int CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public int OrganizationUserID
        {
            get;
            set;
        }

        //UAT-4257
        public Boolean IsClientUserSearchScreen
        {
            get;
            set;
        }

        public string ClientFirstName
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

        public string ClientLastName
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

        public string ClientUserName
        {
            get
            {
                return txtUserName.Text;
            }
            set
            {
                txtUserName.Text = value;
            }
        }

        public string EmailAddress
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

        public List<ClientUserSearchContract> ClientSearchData { get; set; }

        public int CurrentPageIndex
        {
            get
            {
                return grdClientSearchData.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdClientSearchData.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        public int PageSize
        {
            get
            {
                return grdClientSearchData.PageSize;
            }
            set
            {
                grdClientSearchData.PageSize = value;
            }
        }

        public int VirtualPageCount
        {
            get
            {
                return grdClientSearchData.VirtualItemCount;
            }
            set
            {
                grdClientSearchData.VirtualItemCount = value;
                grdClientSearchData.MasterTableView.VirtualItemCount = value;
            }

        }

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
            set
            {
                _gridCustomPaging = value;
                VirtualPageCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        public List<Agency> lstAgencies
        {
            get;
            set;
        }

        public String SearchType
        {
            get
            {
                return rblUserType.SelectedValue;
            }
        }

        #endregion

        #endregion

        #region EVENTS

        #region PAGE EVENTS

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Client Search";
                base.SetPageTitle("Client Search");
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

        protected void Page_Load(object sender, EventArgs e)
        {
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search User(s) per the criteria entered above";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
            (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";

            //UAT-4257
            ucAgencyHierarchyMultipleToSearchRotation.IsChildBackButtonDisabled = true;
            if (!IsPostBack)
            {
                Presenter.OnViewInitialized();
                BindControls();

                //Getting Data from Session if Back to Search button pressed from ClientProfilePage
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey("CancelClicked") && args["CancelClicked"].IsNotNull()
                        || args.ContainsKey("PageType") && args["PageType"].IsNotNull() && (args["PageType"] == SearchQueueType.ClientUserSearch.GetStringValue()))
                    {
                        GetSessionData();
                    }
                    else
                        Session[AppConsts.CLIENT_SEARCH_SESSION_KEY] = null;
                }
                else
                {
                    grdClientSearchData.Visible = false;
                    Session[AppConsts.CLIENT_SEARCH_SESSION_KEY] = null;
                }

                //UAT-4257// by default it should be in disable mode.
                //ucAgencyHierarchyMultipleToSearchRotation.IsInDisableMode = true;
                //ucAgencyHierarchyMultipleToSearchRotation.AddDisableStyle = true;
                // EnableDisableAgencyHierarchy();
            }

            #region UAT-4257
            if (!CurrentViewContext.SelectedTenantIDs.IsNullOrEmpty() && CurrentViewContext.SelectedTenantIDs.Count == AppConsts.ONE)
            {
                hdnSelectedTenantID.Value = CurrentViewContext.SelectedTenantIDs.FirstOrDefault().ToString();
            }
            else
            {
                hdnSelectedTenantID.Value = String.Empty;
            }
            lblinstituteHierarchy.Text = hdnInstHierarchyLabel.Value.HtmlEncode();

            ManageInstHierarchyLink(); //UAT-4257

            EnableDisableAgencyHierarchy();
            if (CurrentViewContext.lstSelectedAgencyHierarchyIDs.Any() && CurrentViewContext.lstSelectedAgencyHierarchyIDs.Count == AppConsts.ONE)
            {
                //ucAgencyHierarchyMultipleToSearchRotation.IsInDisableMode = false;
                //ucAgencyHierarchyMultipleToSearchRotation.AddDisableStyle = false;
                String agencyRootNodeId = cmbAgencyHierarchy.CheckedItems.Select(Sel => Sel.Value).FirstOrDefault();
                ucAgencyHierarchyMultipleToSearchRotation.SelectedRootNodeId = Convert.ToInt32(agencyRootNodeId);
                ucAgencyHierarchyMultipleToSearchRotation.NodeHierarchySelection = true;
            }


            #endregion


            Presenter.OnViewLoaded();
        }

        #endregion

        #region GRID EVENTS

        protected void grdClientSearchData_NeedDataSource(object sender, System.EventArgs e)
        {
            try
            {
                grdClientSearchData.Visible = true;
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                Presenter.PerformSearch();
                grdClientSearchData.DataSource = CurrentViewContext.ClientSearchData;
                GridCustomPaging.VirtualPageCount = VirtualPageCount;
                //To-do 
                //SetSessionValues();
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

        protected void grdClientSearchData_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    RadButton ViewDetail = dataItem["ViewDetail"].Controls[1] as RadButton;
                    RadButton AgencyView = dataItem["AgencyView"].Controls[1] as RadButton;
                    if (String.IsNullOrEmpty(dataItem.GetDataKeyValue("UserID").ToString()))
                        AgencyView.Visible = false;
                    else if (!dataItem["UserType"].Text.Equals("Agency User", StringComparison.Ordinal))
                        AgencyView.Visible = false;
                    else
                    {
                        if (!Presenter.IsAdminLoggedIn())
                            AgencyView.Visible = false;
                    }
                    ViewDetail.ToolTip = "Click to view the client user's details and explore more options";
                    //UAT-2447
                    //dataItem["Phone"].Text = Presenter.GetFormattedPhoneNumber(Convert.ToString(dataItem["Phone"].Text));
                    if (dataItem["OrganizationUserId"].Text == AppConsts.ZERO)
                    {
                        dataItem["OrganizationUserId"].Text = String.Empty;
                    }
                    if (!dataItem["AssignedRoles"].Text.IsNullOrEmpty() && dataItem["AssignedRoles"].Text != AppConsts.NON_BREAKING_SPACE)
                    {
                        if (Convert.ToString(dataItem["AssignedRoles"].Text).Length > 30)
                        {
                            dataItem["AssignedRoles"].ToolTip = dataItem["AssignedRoles"].Text;
                            dataItem["AssignedRoles"].Text = (dataItem["AssignedRoles"].Text).ToString().Substring(0, 30) + "...";
                        }
                    }
                    if (!string.IsNullOrEmpty(dataItem["LastLoginDateTime"].Text) && !string.IsNullOrWhiteSpace(dataItem["LastLoginDateTime"].Text))
                    {
                        if (!string.IsNullOrEmpty(dataItem["LastLoginDateTime"].Text.Replace("&nbsp;", string.Empty)))
                        {
                            DateTime lastLoginDateTime = DateTime.Parse(dataItem["LastLoginDateTime"].Text);
                            var lastLoginDateTimeUTC = DateTime.SpecifyKind(lastLoginDateTime, DateTimeKind.Utc);
                            TimeZoneInfo mstZone = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
                            DateTime mstDateTime = TimeZoneInfo.ConvertTimeFromUtc(lastLoginDateTimeUTC, mstZone);
                            dataItem["LastLoginDateTime"].Text = Convert.ToString(mstDateTime.ToString("g"));
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

        protected void grdClientSearchData_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region DETAILS SCREEN NAVIGATION
                if (e.CommandName == "ViewDetail")
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    String childControl = Convert.ToString(dataItem["UserType"].Text) == "Client Admin" ? ChildControls.ClientProfilePage : ChildControls.AgencyUserDetail;

                    Int32 tenantID = (e.Item.FindControl("hdnTenantID") as HiddenField).Value.IsNullOrEmpty() ? 0 : Convert.ToInt32((e.Item.FindControl("hdnTenantID") as HiddenField).Value);
                    Dictionary<String, String> queryString = new Dictionary<String, String>();

                    String organizationUserId = dataItem.GetDataKeyValue("OrganizationUserId").ToString();
                    String agencyUserId = Convert.ToString(dataItem["AgencyUserId_tmp"].Text) == AppConsts.NON_BREAKING_SPACE ? AppConsts.ZERO : Convert.ToString(dataItem["AgencyUserId_tmp"].Text);

                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"TenantID", Convert.ToString(tenantID) },
                                                                    {"AgencyUserID", agencyUserId },
                                                                    {"Child", childControl},
                                                                    {"OrganizationUserId", organizationUserId},
                                                                    {"PageType", SearchQueueType.ClientUserSearch.ToString()}
                                                                 };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    SetSessionData();
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

                if (e.CommandName.Equals("AgencyView"))
                {
                    //String organizationUserID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"].ToString();

                    String UserID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"].ToString();


                    #region Switch to Applicant View
                    SwitchToAgency(UserID);
                    #endregion

                }
                if (e.CommandName.IsNullOrEmpty())
                {
                    if (e.Item is GridCommandItem)
                    {
                        WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                        if (IsExportCommand(cmbExportFormat))
                        {
                            grdClientSearchData.MasterTableView.GetColumn("_AssignedRoles").Display = true;
                            grdClientSearchData.MasterTableView.GetColumn("AssignedRoles").Display = false;
                        }
                        else
                        {
                            grdClientSearchData.MasterTableView.GetColumn("AssignedRoles").Display = true;
                            grdClientSearchData.MasterTableView.GetColumn("_AssignedRoles").Display = false;
                        }
                    }
                }
                if (e.CommandName == "Cancel")
                {
                    grdClientSearchData.MasterTableView.GetColumn("_AssignedRoles").Display = false;
                    grdClientSearchData.MasterTableView.GetColumn("AssignedRoles").Display = true;
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

        protected void grdClientSearchData_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    GridCustomPaging.SortExpression = e.SortExpression;
                    ViewState["SortExpression"] = e.SortExpression;
                    GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    GridCustomPaging.SortExpression = String.Empty;
                    ViewState["SortExpression"] = String.Empty;
                    GridCustomPaging.SortDirectionDescending = false;
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

        #region Radio Button Events

        protected void rblUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SetAgencyComboboxVisibility();
                EnableDisableInstHierarchy();
                EnableDisableAgencyHierarchy();
                //cmbAgency.ClearCheckedItems();
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

        #region COMMANDBAR EVENTS
        protected void fsucCmdBarButton_ResetClick(object sender, EventArgs e)
        {
            txtEmail.Text = String.Empty;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            txtUserName.Text = String.Empty;
            Session[AppConsts.CLIENT_SEARCH_SESSION_KEY] = null;
            if (Presenter.IsDefaultTenant)
            {
                ddlTenantName.ClearCheckedItems();
                ddlTenantName.EmptyMessage = "--Select--";
                cmbAgencyHierarchy.EmptyMessage = "--Select--"; //UAT-4257
                ViewState["PreviousSelectedTenants"] = null;
            }
            #region UAT-4257
            hdnSelectedTenantID.Value = string.Empty;
            hdnDepartmntPrgrmMppng.Value = String.Empty;
            ucAgencyHierarchyMultipleToSearchRotation.Reset();
            ucAgencyHierarchyMultipleToSearchRotation.SelectedNodeIds = String.Empty;
            //ucAgencyHierarchyMultipleToSearchRotation.IsInDisableMode = true;
            //ucAgencyHierarchyMultipleToSearchRotation.AddDisableStyle = true;
            lblinstituteHierarchy.Text = "";
            hdnInstHierarchyLabel.Value = String.Empty;
            cmbAgencyHierarchy.ClearCheckedItems();
            hdnInstitutionNodeID.Value = String.Empty;
            EnableDisableInstHierarchy();
            EnableDisableAgencyHierarchy();
            #endregion

            rblUserType.SelectedValue = "ALL";
            hdnPreviousAgencyValues.Value = string.Empty;
            SetAgencyComboboxVisibility();
            grdClientSearchData.MasterTableView.SortExpressions.Clear();
            SetResetGrid();
            BindAgencyRootNodes();
        }

        protected void fsucCmdBarButton_SearchClick(object sender, EventArgs e)
        {
            grdClientSearchData.Visible = true;
            SetResetGrid();
            ManageInstHierarchyLink(); //UAT-4257
        }

        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
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
        #endregion

        #region Button Events

        protected void btnCheckAgencyRootNode_Click(object sender, EventArgs e)
        {
            try
            {
                EnableDisableAgencyHierarchy();
                if (!cmbAgencyHierarchy.CheckedItems.IsNullOrEmpty() && cmbAgencyHierarchy.CheckedItems.Count > AppConsts.NONE)
                {
                    //if (cmbAgencyHierarchy.CheckedItems.Count > AppConsts.ONE)
                    //{
                    //    //ucAgencyHierarchyMultipleToSearchRotation.IsInDisableMode = true;
                    //    // ucAgencyHierarchyMultipleToSearchRotation.AddDisableStyle = true;
                    //}
                    if (cmbAgencyHierarchy.CheckedItems.Count == AppConsts.ONE)
                    {
                        // ucAgencyHierarchyMultipleToSearchRotation.IsInDisableMode = false;
                        // ucAgencyHierarchyMultipleToSearchRotation.AddDisableStyle = false;
                        String agencyRootNodeId = cmbAgencyHierarchy.CheckedItems.Select(Sel => Sel.Value).FirstOrDefault();
                        ucAgencyHierarchyMultipleToSearchRotation.SelectedRootNodeId = Convert.ToInt32(agencyRootNodeId);
                        ucAgencyHierarchyMultipleToSearchRotation.NodeHierarchySelection = true;
                    }
                }
                else
                {
                    ucAgencyHierarchyMultipleToSearchRotation.Reset();
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

        protected void btnCheckInstitutionChanged_Click(object sender, EventArgs e)
        {
            try
            {
                lblinstituteHierarchy.Text = "";
                hdnInstHierarchyLabel.Value = String.Empty;
                hdnDepartmntPrgrmMppng.Value = String.Empty;
                hdnInstitutionNodeID.Value = String.Empty;

                List<Int32> lstCheckedIDs = ddlTenantName.CheckedItems.Select(x => Convert.ToInt32(x.Value)).ToList();
                List<Int32> prevCheckedIds = ViewState["PreviousSelectedTenants"].IsNull() ? new List<Int32>() : ViewState["PreviousSelectedTenants"] as List<Int32>;
                if (!prevCheckedIds.Except(lstCheckedIDs).IsNullOrEmpty() || !lstCheckedIDs.Except(prevCheckedIds).IsNullOrEmpty())
                {
                    if (rblUserType.SelectedValue != "TNTUT")
                    {
                        BindAgencyRootNodes();
                    }
                    ViewState["PreviousSelectedTenants"] = lstCheckedIDs;
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

        private void SetResetGrid()
        {
            grdClientSearchData.MasterTableView.FilterExpression = null;
            grdClientSearchData.MasterTableView.SortExpressions.Clear();
            grdClientSearchData.CurrentPageIndex = 0;
            grdClientSearchData.MasterTableView.CurrentPageIndex = 0;
            grdClientSearchData.Rebind();
        }

        /// <summary>
        /// Method used to check command format of ExportFormat Dropdown.
        /// </summary>
        /// <param name="cmbExportFormat"></param>
        /// <returns>true if command selected is matched</returns>
        private Boolean IsExportCommand(WclComboBox cmbExportFormat)
        {
            return cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel");
        }

        /// <summary>
        /// Method to switch to Agency View
        /// </summary>
        private void SwitchToAgency(String organizationUserID)
        {
            String switchingTargetURL = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL].IsNullOrEmpty()
                                                                            ? String.Empty
                                                                            : Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);
            if (!(switchingTargetURL.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || switchingTargetURL.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            {
                if (HttpContext.Current != null)
                {
                    switchingTargetURL = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", switchingTargetURL.Trim());
                }
                else
                {
                    switchingTargetURL = string.Concat("http://", switchingTargetURL.Trim());
                }
            }
            RedirectToTargetSwitchingView(organizationUserID, switchingTargetURL);
        }

        /// <summary>
        /// Method To create/update WebApplicationData, Redirect to Target applicant View.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="switchingTargetURL"></param>
        private void RedirectToTargetSwitchingView(String agencyUserID, String switchingTargetURL)
        {
            Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
            ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
            appInstData.UserID = agencyUserID;
            appInstData.TagetInstURL = switchingTargetURL;
            appInstData.TokenCreatedTime = DateTime.Now;
            appInstData.UserTypeSwitchViewCode = UserTypeSwitchView.AgencyUser.GetStringValue();
            appInstData.AdminOrgUserID = CurrentLoggedInUserID;
            String key = Guid.NewGuid().ToString();

            Dictionary<String, ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
            if (applicationData != null)
            {
                applicantData = applicationData;
                applicantData.Add(key, appInstData);
                Presenter.UpdateWebAgencyUserData("ApplicantInstData", applicantData);
            }
            else
            {
                applicantData.Add(key, appInstData);
                Presenter.AddWebAgencyUserData("ApplicantInstData", applicantData);
            }

            //Log out from application then redirect to selected tenant url, append key in querystring.
            // On login page get data from Application Variable.
            //Presenter.DoLogOff(true);

            //Redirect to login page
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TokenKey", key  }
                                                                 };

            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenAgencyUserView('" + String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}&DeletePrevUsrState=true", key) + "');", true);
            //Response.Redirect(String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}", key));
        }

        //UAT-4257
        private void ManageInstHierarchyLink()
        {
            // instituteHierarchy.Disabled = true;
            //hdnIsInstHierDisabled.Value = true.ToString();
            //if (rblUserType.SelectedValue != "AUUT")
            //{
            //if (CurrentViewContext.SelectedTenantIDs.Count > AppConsts.ONE)
            //{
            //    // instituteHierarchy.Disabled = true;
            //    hdnIsInstHierDisabled.Value = true.ToString();
            //}
            //else if (CurrentViewContext.SelectedTenantIDs.Count == AppConsts.ONE)
            //{
            //    //instituteHierarchy.Disabled = false;
            //    hdnIsInstHierDisabled.Value = false.ToString();
            //}
            EnableDisableInstHierarchy();
            //}
        }

        private void BindAgencyRootNodes()
        {
            Presenter.GetAgencyRootNodes();
            cmbAgencyHierarchy.DataSource = CurrentViewContext.lstAgencyHierarchyRootNodes;
            cmbAgencyHierarchy.DataBind();
            ucAgencyHierarchyMultipleToSearchRotation.Reset();
            EnableDisableAgencyHierarchy();
            //   ucAgencyHierarchyMultipleToSearchRotation.IsInDisableMode = true;
            // ucAgencyHierarchyMultipleToSearchRotation.AddDisableStyle = true;
        }

        private void GetSessionData()
        {
            var serializer = new XmlSerializer(typeof(ClientUserSearchContract));
            ClientUserSearchContract clientUserSearchContract = new ClientUserSearchContract();
            if (Session[AppConsts.CLIENT_SEARCH_SESSION_KEY].IsNotNull())
            {
                TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.CLIENT_SEARCH_SESSION_KEY]));
                clientUserSearchContract = (ClientUserSearchContract)serializer.Deserialize(reader);
                rblUserType.SelectedValue = clientUserSearchContract.SearchType;
                SetAgencyComboboxVisibility();
                CurrentViewContext.SelectedTenantIDs = clientUserSearchContract.SelectedTenantIDs;
                ViewState["PreviousSelectedTenants"] = clientUserSearchContract.SelectedTenantIDs;
                hdnPreviousAgencyValues.Value = String.Join(",", clientUserSearchContract.SelectedTenantIDs);
                BindAgencyRootNodes();
                CurrentViewContext.lstSelectedAgencyHierarchyIDs = clientUserSearchContract.lstSelectedAgencyHierarchyIDs;
                EnableDisableAgencyHierarchy();
                if (CurrentViewContext.lstSelectedAgencyHierarchyIDs.Any() && CurrentViewContext.lstSelectedAgencyHierarchyIDs.Count == AppConsts.ONE)
                {
                    //  ucAgencyHierarchyMultipleToSearchRotation.IsInDisableMode = false;
                    // ucAgencyHierarchyMultipleToSearchRotation.AddDisableStyle = false;
                    String agencyRootNodeId = cmbAgencyHierarchy.CheckedItems.Select(Sel => Sel.Value).FirstOrDefault();
                    ucAgencyHierarchyMultipleToSearchRotation.SelectedRootNodeId = Convert.ToInt32(agencyRootNodeId);
                    ucAgencyHierarchyMultipleToSearchRotation.SelectedNodeIds = clientUserSearchContract.SelectedAgecnyHierarchyIds;
                    ucAgencyHierarchyMultipleToSearchRotation.Rebind();
                    ucAgencyHierarchyMultipleToSearchRotation.NodeHierarchySelection = true;
                }
                lblinstituteHierarchy.Text = hdnInstHierarchyLabel.Value.Trim().HtmlEncode();
                hdnDepartmntPrgrmMppng.Value = clientUserSearchContract.HierarchyNode;
                hdnInstHierarchyLabel.Value = clientUserSearchContract.HierarchyNodeLabel;

                CurrentViewContext.ClientFirstName = clientUserSearchContract.ClientFirstName;
                CurrentViewContext.ClientLastName = clientUserSearchContract.ClientLastName;
                CurrentViewContext.ClientUserName = clientUserSearchContract.ClientUserName;
                CurrentViewContext.EmailAddress = clientUserSearchContract.EmailAddress;
                //Reset session
                Session[AppConsts.CLIENT_SEARCH_SESSION_KEY] = null;
            }
        }

        private void SetSessionData()
        {
            ClientUserSearchContract clientUserSearchContract = new ClientUserSearchContract();
            clientUserSearchContract.SearchType = CurrentViewContext.SearchType;
            clientUserSearchContract.SelectedTenantIDs = CurrentViewContext.SelectedTenantIDs;
            //clientUserSearchContract.SelectedAgencyIDs = CurrentViewContext.SelectedAgencyIDs; //commented for UAT-4257
            clientUserSearchContract.lstSelectedAgencyHierarchyIDs = CurrentViewContext.lstSelectedAgencyHierarchyIDs;
            clientUserSearchContract.SelectedAgecnyHierarchyIds = CurrentViewContext.SelectedAgecnyHierarchyIds;
            clientUserSearchContract.HierarchyNode = hdnDepartmntPrgrmMppng.Value.Trim();
            clientUserSearchContract.HierarchyNodeLabel = hdnInstHierarchyLabel.Value.Trim();

            clientUserSearchContract.ClientFirstName = CurrentViewContext.ClientFirstName;
            clientUserSearchContract.ClientLastName = CurrentViewContext.ClientLastName;
            clientUserSearchContract.ClientUserName = CurrentViewContext.ClientUserName;
            clientUserSearchContract.EmailAddress = CurrentViewContext.EmailAddress;

            var serializer = new XmlSerializer(typeof(ClientUserSearchContract));
            var strbuilder = new StringBuilder();

            using (TextWriter writer = new StringWriter(strbuilder))
            {
                serializer.Serialize(writer, clientUserSearchContract);
            }
            //Session for maintaining control values
            Session[AppConsts.CLIENT_SEARCH_SESSION_KEY] = strbuilder.ToString();
        }

        private void SetAgencyComboboxVisibility()
        {
            cmbAgencyHierarchy.ClearCheckedItems(); 
            if (rblUserType.SelectedValue == "ALL")
            {
                //divAgency.Visible = true;
                rfvAgency.Enabled = true;
                divTenant.Visible = true;
                //UAT-4257
                cmbAgencyHierarchy.Enabled = true;
                //instituteHierarchy.Disabled = true;
                //  hdnIsInstHierDisabled.Value = true.ToString();
                ucAgencyHierarchyMultipleToSearchRotation.Reset();
                spnAgencyHierarchyNodes.Visible = true;
            }
            else if (rblUserType.SelectedValue == "TNTUT")
            {
                //divAgency.Visible = false;
                rfvAgency.Enabled = false;
                divTenant.Visible = true;
                //UAT-4257
                cmbAgencyHierarchy.Enabled = false;
                hdnIsInstHierDisabled.Value = true.ToString();
                ucAgencyHierarchyMultipleToSearchRotation.Reset();
                //ucAgencyHierarchyMultipleToSearchRotation.IsInDisableMode = true;
                //ucAgencyHierarchyMultipleToSearchRotation.AddDisableStyle = true;
                EnableDisableAgencyHierarchy();
                spnAgencyHierarchyNodes.Visible = false;
            }
            else if (rblUserType.SelectedValue == "AUUT")
            {
                //divAgency.Visible = true;
                rfvAgency.Enabled = true;
                divTenant.Visible = true;
                //UAT-4257
                cmbAgencyHierarchy.Enabled = true;
                hdnIsInstHierDisabled.Value = true.ToString();
                hdnDepartmntPrgrmMppng.Value = String.Empty;
                hdnInstHierarchyLabel.Value = String.Empty;
                lblinstituteHierarchy.Text = "";
                BindAgencyRootNodes();
                spnAgencyHierarchyNodes.Visible = true;
            }
        }

        private void EnableDisableInstHierarchy()
        {
            hdnIsInstHierDisabled.Value = true.ToString();
            if ((rblUserType.SelectedValue != "AUUT"))
            {
                if (CurrentViewContext.SelectedTenantIDs.Count > AppConsts.ONE)
                {
                    hdnIsInstHierDisabled.Value = true.ToString();
                }
                else if (CurrentViewContext.SelectedTenantIDs.Count == AppConsts.ONE)
                {
                    hdnIsInstHierDisabled.Value = false.ToString();
                }
            }
            else
            {
                hdnIsInstHierDisabled.Value = true.ToString();
            }
        }

        private void EnableDisableAgencyHierarchy()
        {
            if (CurrentViewContext.lstSelectedAgencyHierarchyIDs.Any() && CurrentViewContext.lstSelectedAgencyHierarchyIDs.Count == AppConsts.ONE)
            {
                ucAgencyHierarchyMultipleToSearchRotation.IsInDisableMode = false;
                ucAgencyHierarchyMultipleToSearchRotation.AddDisableStyle = false;
            }
            else
            {
                ucAgencyHierarchyMultipleToSearchRotation.IsInDisableMode = true;
                ucAgencyHierarchyMultipleToSearchRotation.AddDisableStyle = true;
            }
        }

        #endregion

        #region PUBLIC METHODS

        public void BindControls()
        {
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();

            //For admins
            if (Presenter.IsDefaultTenant)
            {
                ddlTenantName.Enabled = true;
                ddlTenantName.EmptyMessage = "--Select--";
                cmbAgencyHierarchy.EmptyMessage = "--Select--"; //UAT-4257
                CurrentViewContext.SelectedTenantIDs.Clear();
            }
            //for client admins
            else
            {
                ddlTenantName.CheckBoxes = false;
                List<Int32> selectedTenantIDForClient = new List<Int32>();
                selectedTenantIDForClient.Add(CurrentViewContext.TenantID);
                CurrentViewContext.SelectedTenantIDs = selectedTenantIDForClient;
                Presenter.GetAgenciesByInstitionIDs();
                //cmbAgency.DataSource = CurrentViewContext.lstAgencies;
                //cmbAgency.DataBind();
            }
        }

        #endregion

        #endregion
    }
}