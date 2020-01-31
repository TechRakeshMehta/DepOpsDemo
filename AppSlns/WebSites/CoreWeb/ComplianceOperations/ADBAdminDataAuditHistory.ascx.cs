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
using INTSOF.UI.Contract.Mobility;
using CoreWeb.IntsofSecurityModel;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.UI.Contract.ComplianceOperation;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ADBAdminDataAuditHistory : BaseUserControl, IADBAdminDataAuditHistoryView
    {
        #region Private Variables
        private ADBAdminDataAuditHistoryPresenter _presenter = new ADBAdminDataAuditHistoryPresenter();
        private String _viewType;
        private Int32 tenantId;
        private List<TenantDetailsContract> _selectedTenants = null;

        #endregion

        #region Properties

        #region Public

        public ADBAdminDataAuditHistoryPresenter Presenter
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
        /// returns the object of type IADBAdminDataAuditHistoryView.
        /// </summary>
        public IADBAdminDataAuditHistoryView CurrentViewContext
        {
            get { return this; }
        }

        /// <summary>
        /// Sets or gets the Tenant Id for the logged-in user.
        /// </summary>
        Int32 IADBAdminDataAuditHistoryView.TenantId
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

        List<TenantDetailsContract> IADBAdminDataAuditHistoryView.lstSelectedTenants
        {
            get
            {
                _selectedTenants = new List<TenantDetailsContract>();
                foreach (var item in cmbTenant.Items.Where(itm => itm.Checked))
                {
                    TenantDetailsContract tenantDetailsContract = new TenantDetailsContract()
                    {
                        TenantID = Convert.ToInt32(item.Value),
                        TenantName = item.Text
                    };
                    _selectedTenants.Add(tenantDetailsContract);
                }
                return _selectedTenants;
            }
        }

        /// <summary>
        /// Gets or sets the list of active tenants.
        /// </summary>
        List<Entity.Tenant> IADBAdminDataAuditHistoryView.lstTenant
        {
            set
            {
                cmbTenant.DataSource = value;
                cmbTenant.DataBind();
                if (cmbTenant.Items.Count >= 10)
                {
                    cmbTenant.Height = Unit.Pixel(200);
                }
            }
        }

        /// <summary>
        /// returns the Current Logged In User Id.
        /// </summary>
        Int32 IADBAdminDataAuditHistoryView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        /// <summary>
        /// Get or Set First Name
        /// </summary>
        String IADBAdminDataAuditHistoryView.FirstName
        {
            get
            {
                return txtFirstName.Text.Trim();
            }
            set
            {
                txtFirstName.Text = value;
            }

        }

        /// <summary>
        /// Get or Set Last Name
        /// </summary>
        String IADBAdminDataAuditHistoryView.LastName
        {
            get
            {
                return txtLastName.Text.Trim();
            }
            set
            {
                txtLastName.Text = value;
            }
        }

        /// <summary>
        /// Get or set the Time stamp from Date.
        /// </summary>
        DateTime IADBAdminDataAuditHistoryView.TimeStampFromDate
        {
            get
            {
                return Convert.ToDateTime(dpTmStampFromDate.SelectedDate);
            }
            set
            {
                dpTmStampFromDate.SelectedDate = value;
            }
        }

        /// <summary>
        /// Get or set the Time stamp to Date.
        /// </summary>
        DateTime IADBAdminDataAuditHistoryView.TimeStampToDate
        {
            get
            {
                return Convert.ToDateTime(dpTmStampToDate.SelectedDate);
            }
            set
            {
                dpTmStampToDate.SelectedDate = value;
            }
        }

        String IADBAdminDataAuditHistoryView.PackageName
        {
            get
            {
                return txtPackageName.Text.Trim();
            }
            set
            {
                txtPackageName.Text = value;
            }
        }

        String IADBAdminDataAuditHistoryView.CategoryName
        {
            get
            {
                return txtCategoryName.Text.Trim();
            }
            set
            {
                txtCategoryName.Text = value;
            }
        }

        String IADBAdminDataAuditHistoryView.ItemName
        {
            get
            {
                return txtItemName.Text.Trim();
            }
            set
            {
                txtItemName.Text = value;
            }
        }

        List<ApplicantDataAuditHistoryContract> IADBAdminDataAuditHistoryView.ApplicantDataAuditHistoryList
        {
            get;
            set;
        }


        #region UAT-950
        /// <summary>
        /// Get or Set Admin First Name
        /// </summary>
        public String AdminFirstName
        {
            get
            {
                return txtAdminFirstName.Text.Trim();
            }
            set
            {
                txtAdminFirstName.Text = value;
            }
        }

        /// <summary>
        /// Get or Set Admin Last Name
        /// </summary>
        public String AdminLastName
        {
            get
            {
                return txtAdminLastName.Text.Trim();
            }
            set
            {
                txtAdminLastName.Text = value;
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
                return grdApplicantDataAudit.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdApplicantDataAudit.MasterTableView.CurrentPageIndex = value - 1;
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
                //return grdApplicantDataAudit.PageSize > 100 ? 100 : grdApplicantDataAudit.PageSize;
                return grdApplicantDataAudit.PageSize;
            }
            set
            {
                grdApplicantDataAudit.PageSize = value;
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
                grdApplicantDataAudit.VirtualItemCount = value;
                grdApplicantDataAudit.MasterTableView.VirtualItemCount = value;
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
        #region  UAT - 4107
        public List<Int32> selectedFltrRoleIds
        {
            get
            {
                if (!ViewState["selectedFltrRoleIds"].IsNullOrEmpty())
                    return ViewState["selectedFltrRoleIds"] as List<Int32>;
                return new List<Int32>();
            }
            set
            {
                ViewState["selectedFltrRoleIds"] = value;
            }
        }
        public String selectedFltrRoleNames
        {
            get
            {
                if (!ViewState["selectedFltrRoleNames"].IsNullOrEmpty())
                    return ViewState["selectedFltrRoleNames"].ToString();
                return null;
            }
            set
            {
                ViewState["selectedFltrRoleNames"] = value;
            }
        }
        #endregion

        #endregion

        #endregion

        #region Private

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = "Multiple Institutions Admin Data Audit History";
                base.SetPageTitle("Multiple Institutions Admin Data Audit History");
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
                    Presenter.OnViewInitialized();
                    BindInstitutionList();
                    BindRoles();
                }
                fsucCmdBar1.SaveButton.ValidationGroup = "vgAuditHistory";
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

        #region DropDown Events

        #endregion

        #region Button Events

        #region Search Buttons

        /// <summary>
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            try
            {
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
        /// To reset controls and filters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            try
            {
                //Presenter.GetTenants();
                //Method to Reset Search Controls
                ResetSearchControls();
                //Method to reset grid filters 
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

        #endregion

        #region Grid Events

        protected void grdApplicantDataAudit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                CurrentViewContext.GridCustomPaging = GridCustomPaging;
                if (cmbRole.CheckedItems.Count() > 0)
                {
                    selectedFltrRoleIds = cmbRole.CheckedItems.Select(x => Convert.ToInt32(x.Value)).ToList();
                    var fltrRoleNames = cmbRole.CheckedItems.Select(x => x.Text.Replace(" ", "")).ToList();
                    selectedFltrRoleNames = fltrRoleNames.Count > 0 ? String.Join(",", fltrRoleNames) : null;
                }
                Presenter.GetApplicantDataAuditHistory();
                grdApplicantDataAudit.DataSource = CurrentViewContext.ApplicantDataAuditHistoryList;
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
        protected void grdApplicantDataAudit_SortCommand(object sender, GridSortCommandEventArgs e)
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

        protected void grdApplicantDataAudit_PreRender(object sender, EventArgs e)
        {
            try
            {
                //Merging the rows of grid if the value of the following columns are same:-TenantName,ApplicantName,PackageName,CategoryName,ItemName and TimeStampValue.
                foreach (GridDataItem dataItem in grdApplicantDataAudit.Items)
                {
                    GridTableView grdTableView = (GridTableView)dataItem.OwnerTableView;
                    for (int rowIndex = grdTableView.Items.Count - 2; rowIndex >= 0; rowIndex--)
                    {
                        GridDataItem row = grdTableView.Items[rowIndex];
                        GridDataItem previousRow = grdTableView.Items[rowIndex + 1];
                        if (row["TenantName"].Text == previousRow["TenantName"].Text
                            && (row["ApplicantName"].Text == previousRow["ApplicantName"].Text)
                            && (row["PackageName"].Text == previousRow["PackageName"].Text)
                            && (row["CategoryName"].Text == previousRow["CategoryName"].Text)
                            && (row["ItemName"].Text == previousRow["ItemName"].Text)
                            && (row["TimeStampValue"].Text == previousRow["TimeStampValue"].Text)
                            && (row["AdminName"].Text == previousRow["AdminName"].Text)
                            )
                        {
                            row["TenantName"].RowSpan = previousRow["TenantName"].RowSpan < 2 ? 2 : previousRow["TenantName"].RowSpan + 1;
                            previousRow["TenantName"].Visible = false;
                            previousRow["TenantName"].Text = "&nbsp;";
                            row["ApplicantName"].RowSpan = previousRow["ApplicantName"].RowSpan < 2 ? 2 : previousRow["ApplicantName"].RowSpan + 1;
                            previousRow["ApplicantName"].Visible = false;
                            previousRow["ApplicantName"].Text = "&nbsp;";
                            row["PackageName"].RowSpan = previousRow["PackageName"].RowSpan < 2 ? 2 : previousRow["PackageName"].RowSpan + 1;
                            previousRow["PackageName"].Visible = false;
                            previousRow["PackageName"].Text = "&nbsp;";
                            row["CategoryName"].RowSpan = previousRow["CategoryName"].RowSpan < 2 ? 2 : previousRow["CategoryName"].RowSpan + 1;
                            previousRow["CategoryName"].Visible = false;
                            previousRow["CategoryName"].Text = "&nbsp;";
                            row["ItemName"].RowSpan = previousRow["ItemName"].RowSpan < 2 ? 2 : previousRow["ItemName"].RowSpan + 1;
                            previousRow["ItemName"].Visible = false;
                            previousRow["ItemName"].Text = "&nbsp;";
                            row["TimeStampValue"].RowSpan = previousRow["TimeStampValue"].RowSpan < 2 ? 2 : previousRow["TimeStampValue"].RowSpan + 1;
                            previousRow["TimeStampValue"].Visible = false;
                            previousRow["TimeStampValue"].Text = "&nbsp;";
                            row["AdminName"].RowSpan = previousRow["AdminName"].RowSpan < 2 ? 2 : previousRow["AdminName"].RowSpan + 1;
                            previousRow["AdminName"].Visible = false;
                            previousRow["AdminName"].Text = "&nbsp;";
                        }
                        row["TenantName"].BorderColor = System.Drawing.Color.Black;
                        row["TenantName"].BorderStyle = BorderStyle.Solid;
                        row["ApplicantName"].BorderColor = System.Drawing.Color.Black;
                        row["ApplicantName"].BorderStyle = BorderStyle.Solid;
                        row["PackageName"].BorderColor = System.Drawing.Color.Black;
                        row["PackageName"].BorderStyle = BorderStyle.Solid;
                        row["CategoryName"].BorderColor = System.Drawing.Color.Black;
                        row["CategoryName"].BorderStyle = BorderStyle.Solid;
                        row["ItemName"].BorderColor = System.Drawing.Color.Black;
                        row["ItemName"].BorderStyle = BorderStyle.Solid;
                        row["TimeStampValue"].BorderColor = System.Drawing.Color.Black;
                        row["TimeStampValue"].BorderStyle = BorderStyle.Solid;
                        row["AdminName"].BorderColor = System.Drawing.Color.Black;
                        row["AdminName"].BorderStyle = BorderStyle.Solid;
                        row["ChangeValue"].BorderColor = System.Drawing.Color.Black;
                        row["ChangeValue"].BorderStyle = BorderStyle.Solid;
                        row["ChangeValue"].BackColor = System.Drawing.Color.FromArgb(167, 212, 104);
                        previousRow["ChangeValue"].BackColor = System.Drawing.Color.FromArgb(167, 212, 104);
                        row["ChangeTemp"].BorderColor = System.Drawing.Color.Black;
                        row["ChangeTemp"].BorderStyle = BorderStyle.Solid;
                        row["ChangeTemp"].BackColor = System.Drawing.Color.FromArgb(167, 212, 104);
                        previousRow["ChangeTemp"].BackColor = System.Drawing.Color.FromArgb(167, 212, 104);
                    }

                }
                grdApplicantDataAudit.ClientSettings.EnableRowHoverStyle = false;
                grdApplicantDataAudit.ClientSettings.Selecting.AllowRowSelect = false;
                grdApplicantDataAudit.ClientSettings.EnableAlternatingItems = false;
                grdApplicantDataAudit.GridLines = GridLines.None;
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

        protected void grdApplicantDataAudit_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.Item is GridCommandItem)
                {
                    if (!(e.CommandName == RadGrid.RebindGridCommandName))
                    {
                        WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                        if (cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel"))
                        {
                            grdApplicantDataAudit.MasterTableView.GetColumn("ChangeTemp").Display = true;
                        }
                        else
                        {
                            grdApplicantDataAudit.MasterTableView.GetColumn("ChangeTemp").Display = false;
                        }
                        if (e.CommandName == RadGrid.CancelCommandName)
                        {
                            grdApplicantDataAudit.MasterTableView.GetColumn("ChangeTemp").Display = false;
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

        #endregion

        #region Methods

        #region Private Methods
        /// <summary>
        /// To bind Institution drop down 
        /// </summary>
        private void BindInstitutionList()
        {
            try
            {
                if (Presenter.IsDefaultTenant)
                {
                    cmbTenant.Enabled = true;
                }
                else
                {
                    cmbTenant.Enabled = false;
                    cmbTenant.SelectedValue = CurrentViewContext.TenantId.ToString();
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
        /// Method to reset search controls.
        /// </summary>
        private void ResetSearchControls()
        {
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            txtAdminFirstName.Text = String.Empty;
            txtAdminLastName.Text = String.Empty;
            txtPackageName.Text = String.Empty;
            txtCategoryName.Text = String.Empty;
            txtItemName.Text = String.Empty;
            dpTmStampFromDate.SelectedDate = null;
            dpTmStampToDate.SelectedDate = null;

            if (Presenter.IsDefaultTenant)
            {
                cmbTenant.SelectedValue = String.Empty;
                cmbTenant.Items.ForEach(itm => itm.Checked = false);
                BindRoles();
            }
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdApplicantDataAudit.MasterTableView.SortExpressions.Clear();
            grdApplicantDataAudit.CurrentPageIndex = 0;
            grdApplicantDataAudit.MasterTableView.CurrentPageIndex = 0;
            grdApplicantDataAudit.Rebind();
        }
        #region UAT- 4107
        private void BindRoles()
        {
            var fltrRoleList = Enum.GetValues(typeof(AdminDataAuditHistoryRole)).Cast<AdminDataAuditHistoryRole>().Select(x => new KeyValuePair<Int32, String>(Convert.ToInt32(x), x.GetStringValue())).ToList();
            cmbRole.DataSource = fltrRoleList;
            cmbRole.DataBind();
        }
        #endregion
        #endregion

        #endregion
    }
}

