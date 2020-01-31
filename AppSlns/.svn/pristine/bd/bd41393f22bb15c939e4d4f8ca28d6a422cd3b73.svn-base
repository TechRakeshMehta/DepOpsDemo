using CoreWeb.ClinicalRotation.Views;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.ClinicalRotation
{
    public partial class RequirementDataAuditHistory : BaseUserControl, IRequirementDataAuditHistory
    {
        #region [Private Variables]

        private RequirementDataAuditHistoryPresenter _presenter = new RequirementDataAuditHistoryPresenter();
        private Int32 tenantId;

        #endregion

        #region Properties

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


        public IRequirementDataAuditHistory CurrentViewContext
        {
            get { return this; }
        }


        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }


        public List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }


        public String FirstName
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


        public String LastName
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

        public List<UserGroup> lstUserGroup
        {
            get;
            set;
        }

        public Int32 SelectedUserGroupId
        {
            get
            {
                if (ddlUserGroup.SelectedValue.IsNullOrEmpty())
                {
                    return AppConsts.NONE;
                }
                else
                {
                    return Convert.ToInt32(ddlUserGroup.SelectedValue);
                }
            }
            set
            {
                if (ddlUserGroup.Items.Count > 0)
                {
                    ddlUserGroup.SelectedValue = value.ToString();
                }
            }

        }

        public Int32 SelectedReqPackageTypeID
        {
            get
            {
                if (String.IsNullOrEmpty(cmbPackageType.SelectedValue))
                    return 0;
                return Convert.ToInt32(cmbPackageType.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    cmbPackageType.SelectedValue = Convert.ToString(value);
                }
                else
                {
                    cmbPackageType.SelectedIndex = value;
                }
            }
        }

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


        public List<Int32> SelectedCategoryIds
        {
            get
            {
                return ViewState["SelectedCategoryIds"] as List<Int32>;
            }
            set
            {
                ViewState["SelectedCategoryIds"] = value;
            }
        }


        public List<RequirementPackageContract> lstRequirementPackage
        {
            get;
            set;
        }

        public List<RequirementCategoryContract> lstRequirementCategory
        {
            set
            {
                ddlCategory.DataSource = value;
                ddlCategory.DataBind();
            }
        }

        #region UAT-4019
        public List<RequirementPackageTypeContract> lstRequirementPackageType
        {
            get;
            set;
        }
        #endregion

        public List<ApplicantRequirementDataAuditContract> ApplicantRequirementDataAuditList
        {
            get;
            set;
        }


        public RequirementDataAuditHistoryPresenter Presenter
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

        //UAT-3117
        public String ComplioId
        {
            get
            {
                return txtComplioId.Text.Trim();
            }
            set
            {
                txtComplioId.Text = value;
            }
        }

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

        public List<RequirementItemContract> lstRequirementItems
        {
            set
            {
                value.Insert(0, new RequirementItemContract { RequirementItemID = 0, RequirementItemName = "--Select--" });
                ddlItem.DataSource = value;
                ddlItem.DataBind();
            }
        }

        #region Custom Paging


        public Int32 CurrentPageIndex
        {
            get
            {
                return grdDataAudit.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdDataAudit.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        public Int32 PageSize
        {
            get
            {

                return grdDataAudit.PageSize;
            }
            set
            {
                grdDataAudit.PageSize = value;
            }
        }

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
                grdDataAudit.VirtualItemCount = value;
                grdDataAudit.MasterTableView.VirtualItemCount = value;
            }
        }

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

        #region [Page Events]

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Admin Requirement Data Audit History";
                base.SetPageTitle("Admin Requirement Data Audit History");
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
                    grdDataAudit.Visible = false;
                    Presenter.OnViewInitialized();
                    fsucCmdBarButton.SaveButton.ValidationGroup = "vgAuditHistory";
                    BindInstitutionList();
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

        #region [Combox Events]

        protected void ddlTenantName_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (ddlTenantName.SelectedValue.IsNotNull() && Convert.ToInt32(ddlTenantName.SelectedValue) != AppConsts.NONE)
                {
                    CurrentViewContext.SelectedReqPackageTypeID = 1;
                    ResetSearchFilter();
                    SelectedTenantId = Convert.ToInt32(ddlTenantName.SelectedValue);
                    Presenter.GetAllUserGroups();
                    Presenter.GetRequirementPackageTypes(); //UAT-4019
                    Presenter.GetRequirementPackage();
                    Presenter.GetRequirementCategory();
                    BindUserGroups();
                    BindPackageTypes();
                    if (cmbPackageType.Items.Count > 0)
                        cmbPackageType.SelectedValue = "1"; //UAT-4245
                    BindPackages();
                    SelectedCategoryIds = new List<Int32>();
                    SelectedPackageIds = new List<Int32>();
                    SelectedReqPackageTypeID = AppConsts.NONE;
                    CurrentViewContext.lstRequirementItems = new List<RequirementItemContract>();
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

        #region [Grid Events]

        protected void grdDataAudit_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (CurrentViewContext.GridCustomPaging.SortExpression.IsNullOrEmpty())
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = "TimeStampValue";
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = true;
                }

                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                CurrentViewContext.GridCustomPaging = GridCustomPaging;
                Presenter.GetDataAuditHistory();
                grdDataAudit.DataSource = CurrentViewContext.ApplicantRequirementDataAuditList;
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

        protected void grdDataAudit_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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
                            grdDataAudit.MasterTableView.GetColumn("ChangeTemp").Display = true;
                        }
                        else
                        {
                            grdDataAudit.MasterTableView.GetColumn("ChangeTemp").Display = false;
                        }
                        if (e.CommandName == RadGrid.CancelCommandName)
                        {
                            grdDataAudit.MasterTableView.GetColumn("ChangeTemp").Display = false;
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

        protected void grdDataAudit_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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
                    GridCustomPaging.SortExpression = "TimeStampValue";
                    GridCustomPaging.SortDirectionDescending = true;
                    CurrentViewContext.GridCustomPaging.SortExpression = "TimeStampValue";
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = true;
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

        protected void grdDataAudit_PreRender(object sender, EventArgs e)
        {
            try
            {
                string rowStyle_BottomBorder = "border-bottom: 1px solid black !important;";
                string rowStyle_BottomBorderWithBgColor = "border-bottom: 1px solid black !important; background-color:#A7D468;";
                string rowStyle_WithBgColor = "background-color:#A7D468;";

                //Merging the rows of grid if the value of the following columns are same:-ApplicantName,PackageName,CategoryName,ItemName and TimeStampValue.
                foreach (GridDataItem dataItem in grdDataAudit.Items)
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
                            //UAT- 3117
                            && (row["ComplioId"].Text == previousRow["ComplioId"].Text)
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

                            //UAT- 3117
                            row["ComplioId"].RowSpan = previousRow["ComplioId"].RowSpan < 2 ? 2 : previousRow["ComplioId"].RowSpan + 1;
                            previousRow["ComplioId"].Visible = false;
                            previousRow["ComplioId"].Text = "&nbsp;";
                        }

                        row["ApplicantName"].Attributes.Add("style", rowStyle_BottomBorder);

                        row["PackageName"].Attributes.Add("style", rowStyle_BottomBorder);

                        row["CategoryName"].Attributes.Add("style", rowStyle_BottomBorder);

                        row["ItemName"].Attributes.Add("style", rowStyle_BottomBorder);

                        row["TimeStampValue"].Attributes.Add("style", rowStyle_BottomBorder);

                        row["ChangeBy"].Attributes.Add("style", rowStyle_BottomBorder);

                        //UAT- 3117
                        row["ComplioId"].Attributes.Add("style", rowStyle_BottomBorder);

                        row["ChangeValue"].Attributes.Add("style", rowStyle_BottomBorderWithBgColor);

                        previousRow["ChangeValue"].Attributes.Add("style", rowStyle_BottomBorderWithBgColor);

                        row["ChangeTemp"].Attributes.Add("style", rowStyle_BottomBorderWithBgColor);

                        previousRow["ChangeTemp"].Attributes.Add("style", rowStyle_BottomBorderWithBgColor);
                    }
                }

                if (grdDataAudit.Items.Count == 1)
                {
                    GridDataItem row = grdDataAudit.Items[0];
                    row["ChangeValue"].Attributes.Add("style", rowStyle_WithBgColor);
                }

                grdDataAudit.ClientSettings.EnableRowHoverStyle = false;
                grdDataAudit.ClientSettings.Selecting.AllowRowSelect = false;
                grdDataAudit.ClientSettings.EnableAlternatingItems = false;
                grdDataAudit.GridLines = GridLines.None;
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

        #region [Button Click]

        //Search Click
        protected void fsucCmdBarButton_SaveClick(object sender, EventArgs e)
        {
            try
            {
                grdDataAudit.Visible = true;
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

        //Reset Click
        protected void fsucCmdBarButton_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                Presenter.GetTenants();
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

        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
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

        protected void btnPackageChange_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedPackageIds = ddlPackages.CheckedItems.Select(i => Convert.ToInt32(i.Value)).ToList();
                Presenter.GetRequirementCategory();

                SelectedCategoryIds = new List<Int32>();
                Presenter.GetRequirementItem();

                SelectedItemID = 0;
                ddlItem.SelectedIndex = 0;
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
        #region UAT-4019

        protected void cmbPackageType_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                // SelectedReqPackageTypeID = ddlPackageTypes.CheckedItems.Select(i => Convert.ToInt32(i.Value)).ToList();
                Presenter.GetRequirementPackage();
                BindPackages();
                SelectedPackageIds = new List<Int32>();
                Presenter.GetRequirementCategory();
                SelectedCategoryIds = new List<Int32>();
                ddlCategory.SelectedIndex = 0;

                Presenter.GetRequirementItem();

                SelectedItemID = 0;
                ddlItem.SelectedIndex = 0;
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
        //protected void btnPackageTypeChange_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //       // SelectedReqPackageTypeID = ddlPackageTypes.CheckedItems.Select(i => Convert.ToInt32(i.Value)).ToList();
        //        Presenter.GetRequirementPackage();

        //        SelectedPackageIds = new List<Int32>();
        //        Presenter.GetRequirementCategory();

        //        SelectedCategoryIds = new List<Int32>();
        //        ddlCategory.SelectedIndex = 0;

        //        Presenter.GetRequirementItem();

        //        SelectedItemID = 0;
        //        ddlItem.SelectedIndex = 0;
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //}
        #endregion

        protected void btnCategoryChange_Click(object sender, EventArgs e)
        {
            try
            {
                SelectedCategoryIds = ddlCategory.CheckedItems.Select(i => Convert.ToInt32(i.Value)).ToList();
                Presenter.GetRequirementItem();
                SelectedItemID = 0;
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

        #region [Private Methods]

        private void BindInstitutionList()
        {
            try
            {
                if (!CurrentViewContext.lstTenant.IsNullOrEmpty())
                {
                    ddlTenantName.DataSource = CurrentViewContext.lstTenant;
                    ddlTenantName.DataBind();

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
                        Presenter.GetRequirementPackage();
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

        private void BindUserGroups()
        {
            ddlUserGroup.DataSource = CurrentViewContext.lstUserGroup;
            ddlUserGroup.DataBind();
        }

        private void BindPackages()
        {
            Presenter.GetRequirementPackage(); //UAT-4245
            ddlPackages.DataSource = CurrentViewContext.lstRequirementPackage;
            ddlPackages.DataBind();
        }

        #region UAT-UAT-4019
        private void BindPackageTypes()
        {
            cmbPackageType.DataSource = CurrentViewContext.lstRequirementPackageType;
            cmbPackageType.DataBind();
            //cmbPackageType.Items.Insert(0, new RadComboBoxItem("--SELECT--", "0"));
        }
        #endregion

        private void ResetSearchControls()
        {
            ResetSearchFilter();

            if (Presenter.IsDefaultTenant)
            {
                ddlTenantName.SelectedValue = AppConsts.NONE.ToString();
                Presenter.GetAllUserGroups();
                BindUserGroups();
                Presenter.GetRequirementPackage();
                BindPackages();
            }

            Presenter.GetRequirementCategory();
            Presenter.GetRequirementItem();
        }

        private void ResetSearchFilter()
        {
            txtApplicantFirstName.Text = String.Empty;
            txtApplicantLastName.Text = String.Empty;
            txtComplioId.Text = String.Empty; //UAT-3117
            txtAdminFirstName.Text = String.Empty;
            txtAdminLastName.Text = String.Empty;
            dpTmStampFromDate.SelectedDate = null;
            dpTmStampToDate.SelectedDate = null;
            SelectedUserGroupId = AppConsts.NONE;
            SelectedItemID = AppConsts.NONE;
            cmbPackageType.Items.Clear();
            cmbPackageType.Items.Insert(0, new RadComboBoxItem("--Select--", "0"));
            cmbPackageType.SelectedIndex = 0;

            if (!SelectedPackageIds.IsNullOrEmpty())
                SelectedPackageIds.Clear();
            if (!SelectedCategoryIds.IsNullOrEmpty())
                SelectedCategoryIds.Clear();
        }

        private void ResetGridFilters()
        {
            grdDataAudit.MasterTableView.SortExpressions.Clear();
            grdDataAudit.CurrentPageIndex = 0;
            grdDataAudit.MasterTableView.CurrentPageIndex = 0;
            grdDataAudit.Rebind();
        }

        #endregion


    }
}