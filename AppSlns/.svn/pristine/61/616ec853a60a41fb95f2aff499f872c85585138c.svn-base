using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;

#region User Defined Namespaces

using INTSOF.Utils;
using Telerik.Web.UI;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Xml;
using CoreWeb.IntsofSecurityModel;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class UpcomingExpirationsSearchControl : BaseUserControl, IUpcomingExpirationsSearchView
    {

        #region Properties

        #region Private Properties

        private UpcomingExpirationsSearchPresenter _presenter = new UpcomingExpirationsSearchPresenter();
        private String _viewType;
        Int32 tenantId = 0;

        #endregion

        #region Public Properties

        String IUpcomingExpirationsSearchView.Categories
        {
            get
            {
                return String.Join(",", ddlCategories.CheckedItems.Select(i => Convert.ToInt32(i.Value)).ToList());
            }
            set
            {
                ddlCategories.SelectedValue = value;
            }
        }

        DateTime? IUpcomingExpirationsSearchView.DateFrom
        {
            get
            {
                return dpkrDateFrom.SelectedDate;
            }
            set
            {
                dpkrDateFrom.SelectedDate = value;
            }
        }

        DateTime? IUpcomingExpirationsSearchView.DateTo
        {
            get
            {
                return dpkrDateTo.SelectedDate;
            }
            set
            {
                dpkrDateTo.SelectedDate = value;
            }
        }

        String IUpcomingExpirationsSearchView.Items
        {
            get
            {
                return String.Join(",", ddlItems.CheckedItems.Select(i => Convert.ToInt32(i.Value)).ToList());
            }
            set
            {
                ddlItems.SelectedValue = value;
            }
        }

        Int32 IUpcomingExpirationsSearchView.TenantID
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
            set
            {
                tenantId = value;
            }
        }

        List<Tenant> IUpcomingExpirationsSearchView.lstTenant
        {
            get;
            set;
        }

        String IUpcomingExpirationsSearchView.UserGroups
        {
            get
            {
                return String.Join(",", ddlUserGroups.CheckedItems.Select(i => Convert.ToInt32(i.Value)).ToList());
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        Int32 IUpcomingExpirationsSearchView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IUpcomingExpirationsSearchView.SelectedTenantId
        {
            get
            {
                if (!ViewState["SelectedTenantId"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["SelectedTenantId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }

        }

        List<UserGroup> IUpcomingExpirationsSearchView.lstUserGroup
        {
            get;
            set;
        }

        //Revert back changes UAt-2834
        //List<ComplianceCategoryDetails> IUpcomingExpirationsSearchView.lstComplianceCategory
        //{
        //    get; set;
        //}

        List<ComplianceCategory> IUpcomingExpirationsSearchView.lstComplianceCategory
        {
            get;
            set;
        }

        List<ComplianceItem> IUpcomingExpirationsSearchView.lstComplianceItem
        {
            get;
            set;
        }

        List<Int32> IUpcomingExpirationsSearchView.SelectedCategoryIds
        {
            get;
            set;
        }

        Dictionary<Int32, String> IUpcomingExpirationsSearchView.AssignOrganizationUserIds
        {
            get
            {
                if (!ViewState["SelectedApplicants"].IsNull())
                {
                    return ViewState["SelectedApplicants"] as Dictionary<Int32, String>;
                }

                return new Dictionary<Int32, String>();
            }
            set
            {
                ViewState["SelectedApplicants"] = value;
            }
        }

        String IUpcomingExpirationsSearchView.HierarchyIds
        {
            get
            {
                return hdnInstNodeIdNew.Value;
            }
            set
            {
                hdnInstNodeIdNew.Value = value;
            }
        }

        CustomPagingArgsContract IUpcomingExpirationsSearchView.customPagingArgsContract
        {
            get
            {
                if (ViewState["customPagingArgsContract"] == null)
                {
                    ViewState["customPagingArgsContract"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["customPagingArgsContract"];
            }
            set
            {
                ViewState["customPagingArgsContract"] = value;
                VirtualPageCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        public Int32 CurrentPageIndex
        {
            get
            {
                return grdUpcomingExpiration.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdUpcomingExpiration.MasterTableView.CurrentPageIndex = value == 0 ? 0 : value - 1;
            }
        }

        /// <summary>
        /// PageSize
        /// </summary>
        /// <value> Gets the value for PageSize.</value>
        public Int32 PageSize
        {
            get
            {
                return grdUpcomingExpiration.PageSize;
            }
            set
            {
                grdUpcomingExpiration.PageSize = value;
            }

        }

        /// <summary>
        /// VirtualPageCount
        /// </summary>
        /// <value> Sets the value for VirtualPageCount.</value>
        public Int32 VirtualPageCount
        {
            set
            {
                grdUpcomingExpiration.VirtualItemCount = value;
                grdUpcomingExpiration.MasterTableView.VirtualItemCount = value;
            }
            get
            {
                return grdUpcomingExpiration.MasterTableView.VirtualItemCount;
            }
        }

        List<UpcomingExpirationContract> IUpcomingExpirationsSearchView.lstUpcomingExpirations { get; set; }

        public IUpcomingExpirationsSearchView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public UpcomingExpirationsSearchPresenter Presenter
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

        #endregion

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                grdUpcomingExpiration.Visible = false;
                Presenter.OnViewLoaded();
                Init();
                // ucAgencyHierarchy.IsInstitutionHierarchyRequired = true;
            }
            fsucCmdBar1.SubmitButton.CausesValidation = false;
            fsucCmdBar1.ClearButton.CausesValidation = false;
            fsucCmdBar1.CancelButton.CausesValidation = false;
            fsucCmdBar1.SaveButton.CausesValidation = true;
            (fsucCmdBar1 as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search as per the criteria entered above";
            (fsucCmdBar1 as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";
            (fsucCmdBar1 as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
            if (!hdnInstNodeLabel.Value.IsNullOrEmpty())
                lblInstitutionHierarchyPB.Text = hdnInstNodeLabel.Value;
        }

        private void Init()
        {
            lblUpcomingExpirationsTitle.Text = "Manage Upcoming Expirations Search";
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();
            if (Presenter.IsDefaultTenant)
            {
                fsucCmdBar1.ClearButton.Style.Add("display", "none");
                fsucCmdBar1.ExtraButton.Style.Add("display", "none");
            }

            else
            {
                fsucCmdBar1.ClearButton.Style.Clear();
                fsucCmdBar1.ExtraButton.Style.Clear();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Upcoming Expirations Search";
                base.SetPageTitle("Upcoming Expirations Search");
                base.OnInit(e);
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

        #region Dropdown Evants

        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlTenantName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--"));
                if (!Presenter.IsDefaultTenant)
                {
                    ddlTenantName.SelectedValue = Convert.ToString(tenantId);
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
                    ddlTenantName.Enabled = false;
                    hdnDepartmentProgmapNew.Value = String.Empty;
                    hdnInstNodeLabel.Value = String.Empty;
                    hdnInstNodeIdNew.Value = String.Empty;
                    lnkInstitutionHierarchyPB.Attributes["Class"] = string.Empty;
                    lblInstitutionHierarchyPB.Text = String.Empty;
                    BindUserGroup();
                    BindComplianceCategory();
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
            if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                CurrentViewContext.SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
            }
            hdnDepartmentProgmapNew.Value = String.Empty;
            hdnInstNodeLabel.Value = String.Empty;
            hdnInstNodeIdNew.Value = String.Empty;
            lnkInstitutionHierarchyPB.Attributes["Class"] = string.Empty;
            lblInstitutionHierarchyPB.Text = String.Empty;
            BindUserGroup();
            BindComplianceCategory();
        }

        /// <summary>
        /// Handles the selection of users to send message.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void chkSelectUser_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                if (checkBox.IsNull())
                {
                    return;
                }
                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                Dictionary<Int32, String> selectedItems = CurrentViewContext.AssignOrganizationUserIds;
                Int32 orgUserID = (Int32)dataItem.GetDataKeyValue("StudentID");
                String orgUserName = Convert.ToString(dataItem["FirstName"].Text) + " " + Convert.ToString(dataItem["LastName"].Text);
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectUser")).Checked;

                if (isChecked)
                {
                    if (!selectedItems.ContainsKey(orgUserID))
                    {
                        selectedItems.Add(orgUserID, orgUserName);
                    }
                }
                else
                {
                    if (selectedItems != null && selectedItems.ContainsKey(orgUserID))
                    {
                        selectedItems.Remove(orgUserID);
                    }

                }
                CurrentViewContext.AssignOrganizationUserIds = selectedItems;
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

        #region Method

        private void BindUserGroup()
        {
            Presenter.GetAllUserGroups();
            ddlUserGroups.DataSource = CurrentViewContext.lstUserGroup;
            ddlUserGroups.DataBind();
        }

        private void BindComplianceCategory()
        {
            Presenter.GetComplianceCategory();
            ddlCategories.DataSource = CurrentViewContext.lstComplianceCategory;
            ddlCategories.DataBind();
        }

        private void BindComplianceCategoryIte()
        {
            Presenter.GetComplianceItem();
            ddlItems.DataSource = CurrentViewContext.lstComplianceItem;
            ddlItems.DataBind();
            ddlItems.SelectedIndex = 0;
        }

        private void DisplayMessageSentStatus()
        {
            if (hdMessageSent.Value == "sent")
            {
                base.ShowSuccessMessage("Message has been sent successfully.");
                hdMessageSent.Value = "new";
                CurrentViewContext.AssignOrganizationUserIds = new Dictionary<int, string>();
            }
        }


        #endregion

        #region Grid Events

        protected void ddlCategories_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            CurrentViewContext.SelectedCategoryIds = ddlCategories.CheckedItems.Select(i => Convert.ToInt32(i.Value)).ToList();
            BindComplianceCategoryIte();
        }

        protected void grdUpcomingExpiration_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            CurrentViewContext.customPagingArgsContract.DefaultSortExpression = QueueConstants.DEFAULT_SORTING_FIELDS_UPCOMING_EXPIRATION_SEARCH;
            CurrentViewContext.customPagingArgsContract.PageSize = PageSize;
            CurrentViewContext.customPagingArgsContract.CurrentPageIndex = CurrentPageIndex;
            Presenter.GetUpcomingExpirations();
            grdUpcomingExpiration.DataSource = CurrentViewContext.lstUpcomingExpirations.IsNullOrEmpty() ? new List<UpcomingExpirationContract>() : CurrentViewContext.lstUpcomingExpirations;
            DisplayMessageSentStatus();
        }

        protected void grdUpcomingExpiration_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            {
                base.ConfigureExport(grdUpcomingExpiration);
            }
        }

        protected void grdUpcomingExpiration_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    CurrentViewContext.customPagingArgsContract.SortExpression = e.SortExpression;
                    CurrentViewContext.customPagingArgsContract.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    CurrentViewContext.customPagingArgsContract.SortExpression = String.Empty;
                    CurrentViewContext.customPagingArgsContract.SortDirectionDescending = false;
                }
                //  ActionType = ViewMode.Queue.ToString();
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

        protected void grdUpcomingExpiration_Init(object sender, EventArgs e)
        {

        }

        protected void grdUpcomingExpiration_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }

        #endregion

        #region Commandbar Events

        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            Presenter.GetTenants();
            ddlTenantName.Items.Clear();
            Init();
            //ucAgencyHierarchy.Reset();
            //ucAgencyHierarchy.IsInstitutionHierarchyRequired = true;
            //ucAgencyHierarchy.TenantId = -1;
            CurrentViewContext.AssignOrganizationUserIds = new Dictionary<Int32, String>();
            ddlCategories.Items.Clear();
            ddlItems.Items.Clear();
            lblInstitutionHierarchyPB.Text = String.Empty;
            ddlUserGroups.Items.Clear();
            VirtualPageCount = 0;
            grdUpcomingExpiration.MasterTableView.SortExpressions.Clear();
            grdUpcomingExpiration.Rebind();
            CurrentViewContext.AssignOrganizationUserIds = new Dictionary<Int32, String>();
            dpkrDateFrom.Clear();
            dpkrDateTo.Clear();
        }

        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME));
        }

        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            grdUpcomingExpiration.Visible = true;
            //ItemDataGridCustomPaging.CurrentPageIndex = CurrentPageIndex;
            //CurrentViewContext.customPagingArgsContract.PageSize = PageSize;
            //CurrentViewContext.customPagingArgsContract.FilterColumns = SearchItemDataContract.FilterColumns;
            //CurrentViewContext.customPagingArgsContract.FilterOperators = SearchItemDataContract.FilterOperators;
            //CurrentViewContext.customPagingArgsContract.FilterValues = SearchItemDataContract.FilterValues;
            //Presenter.PerformSearch();
            CurrentViewContext.AssignOrganizationUserIds = new Dictionary<Int32, String>();
            grdUpcomingExpiration.Rebind();

            if (grdUpcomingExpiration.Items.Count > 0)
            {
                fsucCmdBar1.ClearButton.Style.Clear();
                fsucCmdBar1.ExtraButton.Style.Clear();
            }
            else
            {
                fsucCmdBar1.ClearButton.Style.Add("display", "none");
                fsucCmdBar1.ExtraButton.Style.Add("display", "none");
            }
        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.AssignOrganizationUserIds.IsNotNull() && !CurrentViewContext.AssignOrganizationUserIds.Any())
                {
                    base.ShowErrorInfoMessage("Please select user(s) to send message.");
                }
                else
                {
                    //Get and set OrgUsersToList in session
                    if (!Session["OrgUsersToList"].IsNullOrEmpty())
                    {
                        Session.Remove("OrgUsersToList");
                    }
                    Session["OrgUsersToList"] = CurrentViewContext.AssignOrganizationUserIds;
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenPopup();", true);
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
    }
}