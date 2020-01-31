using CoreWeb.ComplianceOperations.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceOperations
{
    public partial class AdminDataAuditHistory : BaseUserControl, IAdminDocumentAuditHistoryView
    {
        #region Private
        private AdminDocumentAuditHistoryPresenter _presenter = new AdminDocumentAuditHistoryPresenter();
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
                base.Title = "Admin Document Audit History";
                base.SetPageTitle("Admin Document Audit History");
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
                    BindDocumentStatus();
                    GridCustomPaging.SortDirectionDescending = true;

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



        protected void ddlTenantName_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (ddlTenantName.SelectedValue.IsNotNull() && Convert.ToInt32(ddlTenantName.SelectedValue) != AppConsts.NONE)
                {
                    SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
                    CurrentViewContext.SelectedTenantId = SelectedTenantId;
                    BindDiscardStatus();
                }
                else
                {
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
                if (!String.IsNullOrEmpty(ddlDiscardReason.SelectedValue))
                {
                    CurrentViewContext.DiscardReasonId = Convert.ToInt32(ddlDiscardReason.SelectedValue);
                }
                else
                {
                    CurrentViewContext.DiscardReasonId = null;
                }
                if (!String.IsNullOrEmpty(ddlActionType.SelectedValue))
                {
                    CurrentViewContext.ActionTypeID = Convert.ToInt32(ddlActionType.SelectedValue);
                }
                else { CurrentViewContext.ActionTypeID = null; }

                Presenter.GetAdminDocumentAuditHistory();
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
                        if (row["DocumentName"].Text == previousRow["DocumentName"].Text
                            && (row["ApplicantFirstName"].Text == previousRow["ApplicantFirstName"].Text)
                            && (row["ApplicantLastName"].Text == previousRow["ApplicantLastName"].Text)
                            && (row["School"].Text == previousRow["School"].Text)
                            && (row["AdminFirstName"].Text == previousRow["AdminFirstName"].Text)
                            && (row["CreatedOn"].Text == previousRow["CreatedOn"].Text)
                            && (row["AssignToName"].Text == previousRow["AssignToName"].Text)
                            && (row["AssignByName"].Text == previousRow["AssignByName"].Text)
                            && (row["AssignOn"].Text == previousRow["AssignOn"].Text)
                            )
                        {
                            row["DocumentName"].RowSpan = previousRow["DocumentName"].RowSpan < 2 ? 2 : previousRow["DocumentName"].RowSpan + 1;
                            previousRow["DocumentName"].Visible = false;
                            previousRow["DocumentName"].Text = "&nbsp;";

                            row["ApplicantFirstName"].RowSpan = previousRow["ApplicantFirstName"].RowSpan < 2 ? 2 : previousRow["ApplicantFirstName"].RowSpan + 1;
                            previousRow["ApplicantFirstName"].Visible = false;
                            previousRow["ApplicantFirstName"].Text = "&nbsp;";

                            row["ApplicantLastName"].RowSpan = previousRow["ApplicantLastName"].RowSpan < 2 ? 2 : previousRow["ApplicantLastName"].RowSpan + 1;
                            previousRow["ApplicantLastName"].Visible = false;
                            previousRow["ApplicantLastName"].Text = "&nbsp;";

                            row["School"].RowSpan = previousRow["School"].RowSpan < 2 ? 2 : previousRow["School"].RowSpan + 1;
                            previousRow["School"].Visible = false;
                            previousRow["School"].Text = "&nbsp;";

                            row["AdminFirstName"].RowSpan = previousRow["AdminFirstName"].RowSpan < 2 ? 2 : previousRow["AdminFirstName"].RowSpan + 1;
                            previousRow["AdminFirstName"].Visible = false;
                            previousRow["AdminFirstName"].Text = "&nbsp;";

                            row["CreatedOn"].RowSpan = previousRow["CreatedOn"].RowSpan < 2 ? 2 : previousRow["CreatedOn"].RowSpan + 1;
                            previousRow["CreatedOn"].Visible = false;
                            previousRow["CreatedOn"].Text = "&nbsp;";

                            row["AssignToName"].RowSpan = previousRow["AssignToName"].RowSpan < 2 ? 2 : previousRow["AssignToName"].RowSpan + 1;
                            previousRow["AssignToName"].Visible = false;
                            previousRow["AssignToName"].Text = "&nbsp;";

                            row["AssignByName"].RowSpan = previousRow["AssignByName"].RowSpan < 2 ? 2 : previousRow["AssignByName"].RowSpan + 1;
                            previousRow["AssignByName"].Visible = false;
                            previousRow["AssignByName"].Text = "&nbsp;";

                            row["AssignOn"].RowSpan = previousRow["AssignOn"].RowSpan < 2 ? 2 : previousRow["AssignOn"].RowSpan + 1;
                            previousRow["AssignOn"].Visible = false;
                            previousRow["AssignOn"].Text = "&nbsp;";



                        }

                        row["DocumentName"].BorderColor = System.Drawing.Color.Black;
                        row["DocumentName"].BorderStyle = BorderStyle.Solid;

                        row["ApplicantFirstName"].BorderColor = System.Drawing.Color.Black;
                        row["ApplicantFirstName"].BorderStyle = BorderStyle.Solid;

                        row["ApplicantLastName"].BorderColor = System.Drawing.Color.Black;
                        row["ApplicantLastName"].BorderStyle = BorderStyle.Solid;

                        row["School"].BorderColor = System.Drawing.Color.Black;
                        row["School"].BorderStyle = BorderStyle.Solid;

                        row["AdminFirstName"].BorderColor = System.Drawing.Color.Black;
                        row["AdminFirstName"].BorderStyle = BorderStyle.Solid;

                        //row["ActionType"].BorderColor = System.Drawing.Color.Black;
                        //row["ActionType"].BorderStyle = BorderStyle.Solid;

                        row["CreatedOn"].BorderColor = System.Drawing.Color.Black;
                        row["CreatedOn"].BorderStyle = BorderStyle.Solid;

                        row["AssignToName"].BorderColor = System.Drawing.Color.Black;
                        row["AssignToName"].BorderStyle = BorderStyle.Solid;

                        row["AssignByName"].BorderColor = System.Drawing.Color.Black;
                        row["AssignByName"].BorderStyle = BorderStyle.Solid;

                        row["AssignOn"].BorderColor = System.Drawing.Color.Black;
                        row["AssignOn"].BorderStyle = BorderStyle.Solid;

                        row["Changes"].BorderColor = System.Drawing.Color.Black;
                        row["Changes"].BorderStyle = BorderStyle.Solid;
                        row["Changes"].BackColor = System.Drawing.Color.FromArgb(167, 212, 104);
                        previousRow["Changes"].BackColor = System.Drawing.Color.FromArgb(167, 212, 104);

                        row["ChangeTemp"].BorderColor = System.Drawing.Color.Black;
                        row["ChangeTemp"].BorderStyle = BorderStyle.Solid;
                        row["ChangeTemp"].BackColor = System.Drawing.Color.FromArgb(167, 212, 104);
                        previousRow["ChangeTemp"].BackColor = System.Drawing.Color.FromArgb(167, 212, 104);
                    }

                    if (grdTableView.Items.Count == 1)
                    {
                        GridDataItem row = grdTableView.Items[0];
                        row["DocumentName"].BorderColor = System.Drawing.Color.Black;
                        row["DocumentName"].BorderStyle = BorderStyle.Solid;

                        row["ApplicantFirstName"].BorderColor = System.Drawing.Color.Black;
                        row["ApplicantFirstName"].BorderStyle = BorderStyle.Solid;

                        row["ApplicantLastName"].BorderColor = System.Drawing.Color.Black;
                        row["ApplicantLastName"].BorderStyle = BorderStyle.Solid;

                        row["School"].BorderColor = System.Drawing.Color.Black;
                        row["School"].BorderStyle = BorderStyle.Solid;

                        row["AdminFirstName"].BorderColor = System.Drawing.Color.Black;
                        row["AdminFirstName"].BorderStyle = BorderStyle.Solid;


                        row["CreatedOn"].BorderColor = System.Drawing.Color.Black;
                        row["CreatedOn"].BorderStyle = BorderStyle.Solid;

                        row["AssignToName"].BorderColor = System.Drawing.Color.Black;
                        row["AssignToName"].BorderStyle = BorderStyle.Solid;

                        row["AssignByName"].BorderColor = System.Drawing.Color.Black;
                        row["AssignByName"].BorderStyle = BorderStyle.Solid;

                        row["AssignOn"].BorderColor = System.Drawing.Color.Black;
                        row["AssignOn"].BorderStyle = BorderStyle.Solid;

                        row["Changes"].BorderColor = System.Drawing.Color.Black;
                        row["Changes"].BorderStyle = BorderStyle.Solid;
                        row["Changes"].BackColor = System.Drawing.Color.FromArgb(167, 212, 104);

                        row["ChangeTemp"].BorderColor = System.Drawing.Color.Black;
                        row["ChangeTemp"].BorderStyle = BorderStyle.Solid;
                        row["ChangeTemp"].BackColor = System.Drawing.Color.FromArgb(167, 212, 104);
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
        public IAdminDocumentAuditHistoryView CurrentViewContext
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



        public AdminDocumentAuditHistoryPresenter Presenter
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

                    if (Presenter.IsDefaultTenant)
                    {
                        ddlTenantName.Enabled = true;
                        CurrentViewContext.SelectedTenantId = 0;
                    }
                    else
                    {
                        CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
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

        private void BindDiscardStatus()
        {
            Presenter.GetDocumentDiscardReasonList();
            ddlDiscardReason.DataSource = CurrentViewContext.LstDocumentDiscradReason;
            ddlDiscardReason.DataTextField = "DDR_Name";
            ddlDiscardReason.DataValueField = "DDR_ID";
            ddlDiscardReason.DataBind();
            ddlDiscardReason.Items.Insert(0, new RadComboBoxItem("Select", "0"));
        }

        private void BindDocumentStatus()
        {
            Presenter.GetlkpDataEntryDocumentStatus();
            ddlActionType.DataSource = CurrentViewContext.lkpDataEntryDocumentStatus;
            ddlActionType.DataTextField = "LDEDS_Name";
            ddlActionType.DataValueField = "LDEDS_ID";
            ddlActionType.DataBind();
            ddlActionType.Items.Insert(0, new RadComboBoxItem("Select", "0"));
        }

        /// <summary>
        /// Method to reset search controls.
        /// </summary>
        private void ResetSearchControls()
        {
            txtApplicantFirstName.Text = String.Empty;
            txtApplicantLastName.Text = String.Empty;
            txtAdminFirstName.Text = String.Empty;
            txtAdminLastName.Text = String.Empty;
            dpTmStampFromDate.SelectedDate = null;
            dpTmStampToDate.SelectedDate = null;
            ddlTenantName.SelectedValue = "0";
            ddlActionType.SelectedValue = "0";
            ddlDiscardReason.SelectedValue = "0";
            txtDocumentName.Text = "";
            hdnPreviousSelectedPackageValues.Value = "";

            //SelectedItemID = AppConsts.NONE;
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
        #endregion

        #endregion


        /// <summary>
        /// Get or Set ApplicantFirstName
        /// </summary>
        public String ApplicantFirstName
        {
            get
            {
                return txtApplicantFirstName.Text.Trim();
            }
            set
            {
                txtApplicantFirstName.Text = value;
            }

        }
        /// <summary>
        /// Get or Set Last Name
        /// </summary>
        public String ApplicantLastName
        {
            get
            {
                return txtApplicantLastName.Text.Trim();
            }
            set
            {
                txtApplicantLastName.Text = value;
            }
        }

        public string ActionType
        {
            get
            {
                return ddlActionType.SelectedValue;
            }
            set
            {
                ddlActionType.SelectedValue = value;
            }
        }


        public string DocumentName
        {
            get
            {
                return txtDocumentName.Text.Trim();
            }
            set
            {
                txtDocumentName.Text = value;
            }
        }

        public int? DiscardReasonId
        {
            get;
            //{
            //    return Convert.ToInt32(ddlDiscardReason.SelectedValue);
            //}
            set;
            //{
            //    ddlDiscardReason.SelectedValue = value.ToString();
            //}
        }

        public string DiscardReason
        {
            get;
            set;
        }

        public string DiscardNote
        {
            get;
            set;
        }

        public int CreatedById
        {
            get;
            set;
        }

        public int? ActionTypeID
        {
            get;
            //{
            //    return Convert.ToInt32(ddlActionType.SelectedValue);
            //}
            set;
            //{
            //    ddlActionType.SelectedValue = value.ToString();
            //}
        }
        List<INTSOF.UI.Contract.ComplianceManagement.AdminDataAuditHistory> IAdminDocumentAuditHistoryView.ApplicantDataAuditHistoryList
        {
            get;
            set;
        }
        List<lkpDataEntryDocumentStatu> IAdminDocumentAuditHistoryView.lkpDataEntryDocumentStatus
        {
            get;
            set;
        }

        public List<lkpDocumentDiscardReason> LstDocumentDiscradReason
        {
            get;
            set;
        }

    }
}