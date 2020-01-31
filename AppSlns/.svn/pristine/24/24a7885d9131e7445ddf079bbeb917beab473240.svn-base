using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Telerik.Web.UI;
using System.Linq;
using CoreWeb.IntsofSecurityModel;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;

namespace CoreWeb.SearchUI.Views
{
    public partial class ApplicantComprehensiveSearch : BaseUserControl, IApplicantComprehensiveSearchView
    {
        #region Variables

        private ApplicantComprehensiveSearchPresenter _presenter = new ApplicantComprehensiveSearchPresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private CustomPagingArgsContract _gridCustomPaging = null;
        private SearchItemDataContract _gridSearchContract = null;
        private List<Int32> _selectedTenantIds = null;

        #endregion

        #region Properties

        public ApplicantComprehensiveSearchPresenter Presenter
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

        public IApplicantComprehensiveSearchView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public int TenantId
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

        public string ApplicantFirstName
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

        public string ApplicantLastName
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

        public int CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public int? OrganizationUserID
        {
            get
            {
                if (String.IsNullOrEmpty(txtUserID.Text))
                    return 0;
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

        public string SSN
        {
            //get
            //{
            //    return txtSSN.Text;
            //}
            //set
            //{
            //    txtSSN.Text = value;
            //}
            get;
            set;
        }

        public List<Entity.Tenant> lstTenant
        {
            get;
            set;

        }

        public List<ApplicantDataList> ApplicantSearchData
        {
            get;
            set;
        }

        public int CurrentPageIndex
        {
            get
            {
                return grdApplicantComprehensiveSearchData.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {

                grdApplicantComprehensiveSearchData.MasterTableView.CurrentPageIndex = value - 1;

            }
        }

        public int PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                //return grdApplicantComprehensiveSearchData.PageSize > 100 ? 100 : grdApplicantComprehensiveSearchData.PageSize;
                return grdApplicantComprehensiveSearchData.PageSize;
            }
            set
            {
                grdApplicantComprehensiveSearchData.PageSize = value;
            }
        }

        public int VirtualRecordCount
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
                grdApplicantComprehensiveSearchData.VirtualItemCount = value;
                grdApplicantComprehensiveSearchData.MasterTableView.VirtualItemCount = value;
            }
        }

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

        public List<int> ListSubscriptionIds
        {
            get;
            set;
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public String SSNPermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["SSNPermissionCode"]);
            }
            set
            {
                ViewState["SSNPermissionCode"] = value;
            }
        }
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
        #endregion

        #region Events

        #region PageEvents
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Applicant Comprehensive Search";
                base.SetPageTitle("Applicant Comprehensive Search");
                CmdBarSearch.SubmitButton.CausesValidation = false;
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
                CurrentViewContext.SSN = txtSSN.TextWithPrompt;
                if (!this.IsPostBack)
                {
                    BindTenant();
                    Presenter.GetGranularPermissionForDOBandSSN();
                    ApplySSNMask();

                    Dictionary<String, String> args = new Dictionary<String, String>();

                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        if (args.ContainsKey("CancelClicked") && args["CancelClicked"].IsNotNull()
                            || args.ContainsKey("PageType") && args["PageType"].IsNotNull() && (args["PageType"] == WorkQueueType.ComprehensiveSearch.ToString()))
                        {
                            GetSessionValues();
                        }
                        else
                            Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
                    }
                    else
                    {
                        grdApplicantComprehensiveSearchData.Visible = false;
                        Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
                    }
                }
                HideShowControlsForGranularPermission();
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
        /// To set data source to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                    grdApplicantComprehensiveSearchData.Visible = true;
                    GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                    GridCustomPaging.PageSize = PageSize;
                    CurrentViewContext.GridCustomPaging = GridCustomPaging;
                    Boolean allTenantSelected = cmbTenantName.CheckedItems.Count == cmbTenantName.Items.Count;
                    Presenter.PerformSearch(allTenantSelected);
                    grdApplicantComprehensiveSearchData.DataSource = CurrentViewContext.ApplicantSearchData;
                    ////To set controls values in session
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

                if (e.CommandName.Equals("PortFolioDetail"))
                {
                    Int32 hdnTenantID = Convert.ToInt32((e.Item as GridDataItem)["TenantID"].Text);
                    SetSessionValues();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String organizationUserId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"].ToString();
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "TenantId", Convert.ToString(hdnTenantID) },
                                                                    { "Child", ChildControls.ApplicantPortfolioDetailPage},
                                                                    { "OrganizationUserId", organizationUserId},
                                                                    {"PageType", WorkQueueType.ComprehensiveSearch.ToString()}
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
                    base.ConfigureExport(grdApplicantComprehensiveSearchData);
                }

                #region EXPORT FUNCTIONALITY
                //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                // and displayed the masked column on Export instead of actual column.
                if (e.CommandName.IsNullOrEmpty())
                {
                    WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                    if (cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel"))
                    {
                        grdApplicantComprehensiveSearchData.MasterTableView.GetColumn("_SSN").Display = true;
                    }
                }
                else if (e.CommandName == "Cancel")
                {
                    grdApplicantComprehensiveSearchData.MasterTableView.GetColumn("_SSN").Display = false;
                }
                #endregion

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
        /// <summary>
        /// Grid Item Bound Expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                    // and displayed the masked column on Export instead of actual column.
                    dataItem["_SSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["_SSN"].Text));

                    //UAT-806 Creation of granular permissions for Client Admin users
                    if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue())
                    {
                        dataItem["SSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["SSN"].Text));
                    }
                    else
                    {
                        ///Formatting SSN
                        dataItem["SSN"].Text = Presenter.GetFormattedSSN(Convert.ToString(dataItem["SSN"].Text));
                    }
                    RadButton ViewDetail = dataItem["PortFolioDetail"].Controls[1] as RadButton;
                    ViewDetail.ToolTip = "Click to view the applicant's profile, subscription, and order history details";
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
        /// <summary>
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_SearchClick(object sender, EventArgs e)
        {
            try
            {
                //To reset grid filters 
                grdApplicantComprehensiveSearchData.Visible = true;
                ResetGridFilters();

                if (grdApplicantComprehensiveSearchData.Items.Count > 0)
                {
                    CmdBarSearch.ClearButton.Style.Clear();
                    CmdBarSearch.ExtraButton.Style.Clear();
                }
                else
                {
                    CmdBarSearch.ClearButton.Style.Add("display", "none");
                    CmdBarSearch.ExtraButton.Style.Add("display", "none");
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
        /// To reset controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_ResetClick(object sender, EventArgs e)
        {
            BindTenant();
            txtUserID.Text = String.Empty;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            dpkrDOB.SelectedDate = null;
            txtEmail.Text = String.Empty;
            txtSSN.Text = String.Empty;
            SSN = null;
            //To reset grid filters 
            ResetGridFilters();
            //Reset session
            Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
        }

        /// <summary>
        /// Redirect to Home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_CancelClick(object sender, EventArgs e)
        {
            //Reset session
            Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
        }


        /// <summary>
        /// To set controls values in session
        /// </summary>
        private void SetSessionValues()
        {
            SearchItemDataContract searchDataContract = new SearchItemDataContract();

            searchDataContract.IsBackToSearch = true;

            searchDataContract.ApplicantFirstName = ApplicantFirstName;
            searchDataContract.ApplicantLastName = ApplicantLastName;
            searchDataContract.EmailAddress = EmailAddress;
            searchDataContract.DateOfBirth = DateOfBirth;
            searchDataContract.ApplicantSSN = SSN;
            searchDataContract.OrganizationUserId = OrganizationUserID.Value;
            searchDataContract.SelectedTenants = SelectedTenantIds;
            searchDataContract.GridCustomPagingArguments = GridCustomPaging;

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
        /// To get session values for controls
        /// </summary>
        private void GetSessionValues()
        {
            var serializer = new XmlSerializer(typeof(SearchItemDataContract));
            SearchItemDataContract searchDataContract = new SearchItemDataContract();
            if (Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY].IsNotNull())
            {
                TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY]));
                searchDataContract = (SearchItemDataContract)serializer.Deserialize(reader);

                //SelectedTenantId = searchDataContract.ClientID;
                ApplicantFirstName = searchDataContract.ApplicantFirstName;
                ApplicantLastName = searchDataContract.ApplicantLastName;
                EmailAddress = searchDataContract.EmailAddress;
                DateOfBirth = searchDataContract.DateOfBirth;
                //SSN = searchDataContract.ApplicantSSN;

                if (CurrentViewContext.SSN.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
                {
                    if (!searchDataContract.ApplicantSSN.IsNullOrEmpty())
                    {
                        CurrentViewContext.SSN = searchDataContract.ApplicantSSN.Substring(searchDataContract.ApplicantSSN.Length - AppConsts.FOUR);
                        txtSSN.Text = CurrentViewContext.SSN;
                    }
                }
                else
                {
                    CurrentViewContext.SSN = searchDataContract.ApplicantSSN;
                    txtSSN.Text = CurrentViewContext.SSN;
                }

                if (searchDataContract.OrganizationUserId != null && searchDataContract.OrganizationUserId > 0)
                    OrganizationUserID = searchDataContract.OrganizationUserId;
                SelectedTenantIds = searchDataContract.SelectedTenants;
                CurrentViewContext.GridCustomPaging = searchDataContract.GridCustomPagingArguments;

                //Reset session
                Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
            }
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdApplicantComprehensiveSearchData.MasterTableView.SortExpressions.Clear();
            grdApplicantComprehensiveSearchData.CurrentPageIndex = 0;
            grdApplicantComprehensiveSearchData.MasterTableView.CurrentPageIndex = 0;
            grdApplicantComprehensiveSearchData.Rebind();
        }
        private void BindTenant()
        {
            Presenter.GetTenantList();
            cmbTenantName.DataSource = lstTenant;
            cmbTenantName.DataBind();
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
                grdApplicantComprehensiveSearchData.MasterTableView.GetColumn("DateOfBirth").Visible = false;
            }
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                divSSN.Visible = false;
                grdApplicantComprehensiveSearchData.MasterTableView.GetColumn("SSN").Visible = false;
                //Hide Masked column if user does not have permission to view SSN Column.
                grdApplicantComprehensiveSearchData.MasterTableView.GetColumn("_SSN").Visible = false;
            }
            //else if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
            //{
            //    txtSSN.Mask = AppConsts.SSN_MASK_FORMATE; //@"\#\#\#-\#\#-####";
            //}
        }

        private void ApplySSNMask()
        {
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
            {
                //txtSSN.Mask = AppConsts.SSN_MASK_FORMATE; //@"\#\#\#-\#\#-####"
                txtSSN.Mask = AppConsts.SSN_MASK_FORMAT_ALPHANUMERIC;
            }
        }

        #endregion
    }
}