using CoreWeb.ComplianceOperations.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceOperations
{
    public partial class AdminAuditHistoryDiscardedDocuments : BaseUserControl, IAdminAuditHistoryDiscardedDocumentsView
    {

        #region Properties

        #region Private
        private AdminAuditHistoryDiscardedDocumentsPresenter _presenter = new AdminAuditHistoryDiscardedDocumentsPresenter();
        private String _viewType;
        private Int32 tenantId;
        #endregion

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
        public IAdminAuditHistoryDiscardedDocumentsView CurrentViewContext
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

        public AdminAuditHistoryDiscardedDocumentsPresenter Presenter
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

        #region Custom Paging

        /// <summary>
        /// Current Page Index</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdApplicantDataAuditDiscadedDocs.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdApplicantDataAuditDiscadedDocs.MasterTableView.CurrentPageIndex = value - 1;
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
                //return grdApplicantDataAuditDiscadedDocs.PageSize > 100 ? 100 : grdApplicantDataAuditDiscadedDocs.PageSize;
                return grdApplicantDataAuditDiscadedDocs.PageSize;
            }
            set
            {
                grdApplicantDataAuditDiscadedDocs.PageSize = value;
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
                grdApplicantDataAuditDiscadedDocs.VirtualItemCount = value;
                grdApplicantDataAuditDiscadedDocs.MasterTableView.VirtualItemCount = value;
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

        //public string DocumentName
        //{
        //    get
        //    {
        //        return txtDocumentName.Text.Trim();
        //    }
        //    set
        //    {
        //        txtDocumentName.Text = value;
        //    }
        //}

        public string DiscardReason
        {
            get;
            set;
        }

        List<INTSOF.UI.Contract.ComplianceManagement.DiscardedDocumentAuditContract> IAdminAuditHistoryDiscardedDocumentsView.ApplicantDataAuditHistoryList
        {
            get;
            set;
        }

        List<Entity.lkpDataEntryDocumentStatu> IAdminAuditHistoryDiscardedDocumentsView.lkpDataEntryDocumentStatus
        {
            get;
            set;
        }

        //public List<lkpDocumentDiscardReason> LstDocumentDiscradReason
        //{
        //    get;
        //    set;
        //}
        #endregion 

        #endregion

        #region Events

        #region Dropdown Events 
        protected void ddlTenantName_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {

            try
            {
                if (ddlTenantName.SelectedValue.IsNotNull() && Convert.ToInt32(ddlTenantName.SelectedValue) != AppConsts.NONE)
                {
                    SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
                    CurrentViewContext.SelectedTenantId = SelectedTenantId;
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

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = "Discarded Document Audit";
                base.SetPageTitle("Discarded Document Audit");
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
                    grdApplicantDataAuditDiscadedDocs.Visible = false;
                    Presenter.OnViewInitialized();
                    divTenant.Visible = true;
                    BindInstitutionList();                    
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

        #region Grid Events 
        protected void grdApplicantDataAuditDiscadedDocs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                CurrentViewContext.GridCustomPaging = GridCustomPaging;
                //if (!String.IsNullOrEmpty(ddlDiscardReason.SelectedValue))
                //{
                //    CurrentViewContext.DiscardReasonId = Convert.ToInt32(ddlDiscardReason.SelectedValue);
                //}
                //else
                //{
                //    CurrentViewContext.DiscardReasonId = null;
                //}
                //if (!String.IsNullOrEmpty(ddlActionType.SelectedValue))
                //{
                //    CurrentViewContext.ActionTypeID = Convert.ToInt32(ddlActionType.SelectedValue);
                //}
                //else { CurrentViewContext.ActionTypeID = null; }

                Presenter.GetDiscardedDocumentDataAuditHistory();
                var data1 = CurrentViewContext.ApplicantDataAuditHistoryList;
                grdApplicantDataAuditDiscadedDocs.DataSource = CurrentViewContext.ApplicantDataAuditHistoryList;
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

        protected void grdApplicantDataAuditDiscadedDocs_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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

        protected void grdApplicantDataAuditDiscadedDocs_PreRender(object sender, EventArgs e)
        {
            try
            {

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

        protected void grdApplicantDataAuditDiscadedDocs_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {

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

        #region Buttons Clicks       

        /// <summary>
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            try
            {
                grdApplicantDataAuditDiscadedDocs.Visible = true;
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
                        //ddlTenantName.Enabled = true; // temporary added
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

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdApplicantDataAuditDiscadedDocs.MasterTableView.SortExpressions.Clear();
            grdApplicantDataAuditDiscadedDocs.CurrentPageIndex = 0;
            grdApplicantDataAuditDiscadedDocs.MasterTableView.CurrentPageIndex = 0;
            grdApplicantDataAuditDiscadedDocs.Rebind();
        }

        /// <summary>
        /// Method to reset search controls.
        /// </summary>
        private void ResetSearchControls()
        {
            txtApplicantFirstName.Text = String.Empty;
            txtApplicantLastName.Text = String.Empty;
            //txtAdminFirstName.Text = String.Empty;
            //txtAdminLastName.Text = String.Empty;
            dpTmStampFromDate.SelectedDate = null;
            dpTmStampToDate.SelectedDate = null;
            ddlTenantName.SelectedValue = "0";
            //txtDocumentName.Text = "";            
        }
        #endregion

        #endregion
    }
}