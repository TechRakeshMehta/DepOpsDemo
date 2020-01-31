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


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ApplicantDataAuditHistory : BaseUserControl, IApplicantDataAuditHistoryView
    {

        #region Private
        private ApplicantDataAuditHistoryPresenter _presenter = new ApplicantDataAuditHistoryPresenter();
        private String _viewType;
        private Int32 tenantId;
        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = "Admin Data Audit History";
                base.SetPageTitle("Admin Data Audit History");
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
                    grdApplicantDataAudit.Visible = false;
                    Presenter.OnViewInitialized();
                    divTenant.Visible = true;
                    BindInstitutionList();
                    if (!Presenter.IsDefaultTenant)
                    {
                        BindPackages();
                        Presenter.GetComplianceCategory();
                    }
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

        protected void ddlPackage_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {   //UAT-2069 :- Added checkbox functionality and checked wheather the call is from package dropdown using hiddedfield.
                if (hdnIsPackageSelected.Value == "1")
                {
                    SelectedPackageIds = ddlPackage.CheckedItems.Select(i => Convert.ToInt32(i.Value)).ToList();
                    hdnPreviousSelectedPackageValues.Value = String.Join(",", SelectedPackageIds);
                    Presenter.GetComplianceCategory();
                    SelectedCategoryIds = new List<Int32>();
                    Presenter.GetComplianceItem();
                    SelectedItemID = 0;
                    ddlItem.SelectedIndex = 0;
                    hdnIsPackageSelected.Value = "0";
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

        protected void btnPackageChecked_Click(object sender, EventArgs e)
        {
            try
            {
                //UAT-2069 :- Added checkbox functionality and checked wheather the call is from package dropdown using hiddedfield.
                if (hdnIsPackageSelected.Value == "1")
                {
                    SelectedPackageIds = ddlPackage.CheckedItems.Select(i => Convert.ToInt32(i.Value)).ToList();
                    hdnPreviousSelectedPackageValues.Value = String.Join(",", SelectedPackageIds);
                    Presenter.GetComplianceCategory();
                    SelectedCategoryIds = new List<Int32>();
                    SelectedItemID = 0;
                    ddlItem.SelectedIndex = 0;
                    hdnIsPackageSelected.Value = "0";
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

        protected void ddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (ddlTenantName.SelectedValue.IsNotNull() && Convert.ToInt32(ddlTenantName.SelectedValue) != AppConsts.NONE)
                {
                    SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
                    Presenter.GetAllUserGroups();
                    Presenter.GetCompliancePackage();
                    //UAT-2069 Get all Categories on tenant selection.without selecting package .
                    Presenter.GetComplianceCategory();
                    BindUserGroups();
                    BindPackages();
                    BindRoles();
                    SelectedCategoryIds = new List<Int32>();
                    SelectedPackageIds = new List<Int32>();
                }
                else
                {
                    //BindRoles();
                    ResetGridFilters();
                    ResetSearchControls();
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

        protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {    //UAT-2069 :- Added checkbox functionality and checked wheather the call is from Category dropdown using hiddedfield.
                if (hdnIsCategorySelected.Value == "1")
                {
                    if (ddlCategory.CheckedItems.Count <= 0)
                    {
                        //SelectedCategoryId = 0;
                        ddlItem.Enabled = false;
                    }
                    else
                    {
                        //SelectedCategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                        ddlItem.Enabled = true;
                    }

                    SelectedCategoryIds = ddlCategory.CheckedItems.Select(i => Convert.ToInt32(i.Value)).ToList();
                    Presenter.GetComplianceItem();
                    SelectedItemID = 0;
                    ddlItem.SelectedIndex = 0;
                    hdnIsCategorySelected.Value = "0";
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

        protected void btnCategoryChecked_Click(object sender, EventArgs e)
        {
            try
            {
                //UAT-2069 :- Added checkbox functionality and checked wheather the call is from Category dropdown using hiddedfield.
                if (hdnIsCategorySelected.Value == "1")
                {
                    if (ddlCategory.CheckedItems.Count <= 0)
                    {
                        //SelectedCategoryId = 0;
                        ddlItem.Enabled = false;
                    }
                    else
                    {
                        //SelectedCategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                        ddlItem.Enabled = true;
                    }

                    SelectedCategoryIds = ddlCategory.CheckedItems.Select(i => Convert.ToInt32(i.Value)).ToList();
                    Presenter.GetComplianceItem();
                    SelectedItemID = 0;
                    ddlItem.SelectedIndex = 0;
                    hdnIsCategorySelected.Value = "0";
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
                grdApplicantDataAudit.Visible = true;
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
                Presenter.GetTenants();
                //Method to Reset Search Controls
                ResetSearchControls();
                //Method to reset grid filters 
                if (!Presenter.IsDefaultTenant) // UAT-4371
                    Presenter.IsClientAdminReset = true;
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
                selectedFltrRoleNames = null;
                if (cmbRole.CheckedItems.Count() > 0)
                {
                    selectedFltrRoleIds = cmbRole.CheckedItems.Select(x => Convert.ToInt32(x.Value)).ToList();
                    var fltrRoleNames = cmbRole.CheckedItems.Select(x => x.Text.Replace(" ","")).ToList();
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
                //Merging the rows of grid if the value of the following columns are same:-ApplicantName,PackageName,CategoryName,ItemName and TimeStampValue.
                foreach (GridDataItem dataItem in grdApplicantDataAudit.Items)
                {
                    GridTableView grdTableView = (GridTableView)dataItem.OwnerTableView;
                    for (int rowIndex = grdTableView.Items.Count - 2; rowIndex >= 0; rowIndex--)
                    {
                        GridDataItem row = grdTableView.Items[rowIndex];
                        GridDataItem previousRow = grdTableView.Items[rowIndex + 1];
                        if (row["ApplicantName"].Text == previousRow["ApplicantName"].Text
                            && (row["PackageName"].Text == previousRow["PackageName"].Text)
                            && (row["CategoryName"].Text == previousRow["CategoryName"].Text)
                            && (row["ItemName"].Text == previousRow["ItemName"].Text)
                            && (row["TimeStampValue"].Text == previousRow["TimeStampValue"].Text)
                            && (row["ChangeBy"].Text == previousRow["ChangeBy"].Text)
                            )
                        {
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
                            row["ChangeBy"].RowSpan = previousRow["ChangeBy"].RowSpan < 2 ? 2 : previousRow["ChangeBy"].RowSpan + 1;
                            previousRow["ChangeBy"].Visible = false;
                            previousRow["ChangeBy"].Text = "&nbsp;";
                        }
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
                        row["ChangeBy"].BorderColor = System.Drawing.Color.Black;
                        row["ChangeBy"].BorderStyle = BorderStyle.Solid;
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

        #region Properties

        #region Public
        /// <summary>
        /// Sets or gets the Selected Tenant Id from the select tenant dropdown.
        /// </summary>
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

        /// <summary>
        /// Sets or gets the Tenant Id for the logged-in user.
        /// </summary>
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

        /// <summary>
        /// returns the object of type IApplicantDataAuditHistoryView.
        /// </summary>
        public IApplicantDataAuditHistoryView CurrentViewContext
        {
            get { return this; }
        }

        /// <summary>
        /// returns the Current Logged In User Id.
        /// </summary>
        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        /// <summary>
        /// Gets or sets the list of active tenants.
        /// </summary>
        public List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set First Name
        /// </summary>
        public String FirstName
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
        public String LastName
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
        public DateTime TimeStampFromDate
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
        public DateTime TimeStampToDate
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

        /// <summary>
        /// List of User groups.
        /// </summary>
        public List<UserGroup> lstUserGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Selected User Group id.
        /// </summary>
        public Int32 SelectedUserGroupId
        {
            get
            {

                return Convert.ToInt32(cmbChkUserGroup.SelectedValue);

            }
            set
            {
                if (cmbChkUserGroup.Items.Count > 0)
                {
                    cmbChkUserGroup.SelectedValue = value.ToString();
                }
            }

        }

        /// <summary>
        /// Get or Set Selected Package ids.
        /// </summary>
        public List<Int32> SelectedPackageIds
        {
            get
            {
                return ViewState["SelectedPackageIds"] as List<Int32>;
            }
            set
            {
                ViewState["SelectedPackageIds"] = value;
            }
        }

        /// <summary>
        /// Get or Set Selected Category ids.
        /// </summary>
        public List<Int32> SelectedCategoryIds
        {
            get
            {
                return ViewState["SelectedCategoryIds"] as List<Int32>;

                //if (!ddlCategory.SelectedValue.IsNullOrEmpty())
                //    return Convert.ToInt32(ddlCategory.SelectedValue);
                //else
                //    return 0;
            }
            set
            {
                ViewState["SelectedCategoryIds"] = value;
                //if (ddlCategory.Items.Count > 0)
                //{
                //    ddlCategory.SelectedValue = value.ToString();
                //}
            }
        }

        /// <summary>
        /// Populates the dropdown with the list of Compliance Package.
        /// </summary>
        public List<ComplaincePackageDetails> lstCompliancePackage
        {
            get;
            set;
        }

        /// <summary>
        /// Populates the dropdown with the list of Compliance Category.
        /// </summary>
        public List<ComplianceCategory> lstComplianceCategory
        {
            set
            {
                ddlCategory.DataSource = value;
                ddlCategory.DataBind();
                if (ddlCategory.Items.Count >= 10)
                {
                    ddlCategory.Height = Unit.Pixel(200);
                }
            }
        }

        public List<Entity.ClientEntity.ApplicantDataAuditHistory> ApplicantDataAuditHistoryList
        {
            get;
            set;
        }


        public ApplicantDataAuditHistoryPresenter Presenter
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

        /// <summary>
        /// Get or Set Selected ItemID
        /// </summary>
        public Int32 SelectedItemID
        {
            get
            {
                if (!ddlItem.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(ddlItem.SelectedValue);
                else
                    return 0;
            }
            set
            {
                if (ddlItem.Items.Count > 0)
                {
                    ddlItem.SelectedValue = value.ToString();
                }
            }
        }

        /// <summary>
        /// Populates dropdown with list of Compliance Items
        /// </summary>
        public List<ComplianceItem> lstComplianceItems
        {
            set
            {
                ddlItem.DataSource = value;
                ddlItem.DataBind();
                if (ddlItem.Items.Count >= 10)
                {
                    ddlItem.Height = Unit.Pixel(200);
                }
            }
        }

        #region UAT - 4107
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

        #endregion
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
                if (!CurrentViewContext.lstTenant.IsNullOrEmpty())
                {
                    ddlTenantName.DataSource = CurrentViewContext.lstTenant;
                    ddlTenantName.DataBind();
                    if (ddlTenantName.Items.Count >= 10)
                    {
                        ddlTenantName.Height = Unit.Pixel(200);
                    }
                    Presenter.GetAllUserGroups();
                    BindUserGroups();
                    if (Presenter.IsDefaultTenant)
                    {
                        ddlTenantName.Enabled = true;
                        CurrentViewContext.SelectedTenantId = 0;
                    }
                    else
                    {
                        CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
                        Presenter.GetCompliancePackage(CurrentViewContext.CurrentLoggedInUserId);
                        BindRoles();
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
        /// To bind User group dropdown
        /// </summary>
        private void BindUserGroups()
        {
            cmbChkUserGroup.DataSource = CurrentViewContext.lstUserGroup;
            cmbChkUserGroup.DataBind();
            if (cmbChkUserGroup.Items.Count >= 10)
            {
                cmbChkUserGroup.Height = Unit.Pixel(200);
            }
        }

        /// <summary>
        /// Method to bind the Package dropdown 
        /// </summary>
        private void BindPackages()
        {
            ddlPackage.DataSource = CurrentViewContext.lstCompliancePackage;
            ddlPackage.DataBind();
            if (ddlPackage.Items.Count >= 10)
            {
                ddlPackage.Height = Unit.Pixel(200);
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
            dpTmStampFromDate.SelectedDate = null;
            dpTmStampToDate.SelectedDate = null;
            SelectedUserGroupId = AppConsts.NONE;
            hdnPreviousSelectedPackageValues.Value = "";
            SelectedItemID = AppConsts.NONE;
            selectedFltrRoleIds = new List<int>();
            selectedFltrRoleNames = null;
            if (!SelectedPackageIds.IsNullOrEmpty())
            {
                SelectedPackageIds.Clear();
                ddlPackage.Items.ForEach(item => item.Checked = false);
            }
            if (!SelectedCategoryIds.IsNullOrEmpty())
            {
                SelectedCategoryIds.Clear();
            }
            cmbRole.Items.ForEach(item => item.Checked = false);
            if (Presenter.IsDefaultTenant)
            {
                ddlTenantName.SelectedValue = AppConsts.NONE.ToString();
                Presenter.GetAllUserGroups();
                BindUserGroups();
                Presenter.GetCompliancePackage();
                BindPackages();
                BindRoles();
                //Presenter.GetComplianceCategory();
            }
            Presenter.GetComplianceCategory();
            Presenter.GetComplianceItem();
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

            //List<KeyValuePair<Int32, String>> fltrRoleList = new List<KeyValuePair<int, string>>();
            //fltrRoleList.Add(new KeyValuePair<int, string>(Convert.ToInt32(AdminDataAuditHistoryRole.ADBAdmin), Convert.ToString(AdminDataAuditHistoryRole.ADBAdmin)));
            //fltrRoleList.Add(new KeyValuePair<int, string>(Convert.ToInt32(AdminDataAuditHistoryRole.ClientAdmin), Convert.ToString(AdminDataAuditHistoryRole.ClientAdmin)));
            //fltrRoleList.Add(new KeyValuePair<int, string>(Convert.ToInt32(AdminDataAuditHistoryRole.Student), Convert.ToString(AdminDataAuditHistoryRole.Student)));
            //fltrRoleList.Add(new KeyValuePair<int, string>(Convert.ToInt32(AdminDataAuditHistoryRole.System), Convert.ToString(AdminDataAuditHistoryRole.System)));
            var fltrRoleList = Enum.GetValues(typeof(AdminDataAuditHistoryRole)).Cast<AdminDataAuditHistoryRole>().Select(x => new KeyValuePair<Int32, String>(Convert.ToInt32(x), x.GetStringValue())).ToList();
            cmbRole.DataSource = fltrRoleList;
            cmbRole.DataBind();
        }
        #endregion

        #endregion

        #endregion
    }
}

