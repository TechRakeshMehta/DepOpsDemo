using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.AgencyJobBoard.Views
{
    public partial class ViewAgencyJobPosting : BaseUserControl, IViewAgencyJobPostingView
    {
        #region Variables

        #region Private variables
        private ViewAgencyJobPostingPresenter _presenter = new ViewAgencyJobPostingPresenter();
        private Int32 tenantId = AppConsts.NONE;
        private String _viewType;

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        Int32 IViewAgencyJobPostingView.CurrentLoggedInUserId
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                }
                else
                {
                    return SysXWebSiteUtils.SessionService.OrganizationUserId;
                }
            }
        }

        Int32 IViewAgencyJobPostingView.OrganisationUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }



        public List<DefinedRequirementContract> LstJobFieldType
        {
            get
            {
                if (!ViewState["LstJobFieldType"].IsNull())
                {
                    return (ViewState["LstJobFieldType"]) as List<DefinedRequirementContract>;
                }
                return new List<DefinedRequirementContract>();
            }
            set
            {
                ViewState["LstJobFieldType"] = value;
            }
        }

        String IViewAgencyJobPostingView.JobTitle
        {
            get
            {
                return txtJobTitle.Text.Trim();
            }
            set
            {
                txtJobTitle.Text = value;
            }
        }

        String IViewAgencyJobPostingView.Company
        {
            get
            {
                return txtCompany.Text.Trim();
            }
            set
            {
                txtCompany.Text = value;
            }
        }

        String IViewAgencyJobPostingView.Location
        {
            get
            {
                return txtLocation.Text.Trim();
            }
            set
            {
                txtLocation.Text = value;
            }
        }

        String IViewAgencyJobPostingView.JobTypeCode
        {
            get
            {
                return rbljobType.SelectedValue;
            }
            set
            {
                rbljobType.SelectedValue = value;
            }
        }
        //Chandan Hasija
        Int32 IViewAgencyJobPostingView.SelectedJobFieldTypeID
        {
            get
            {
                return cmbJobFieldType.SelectedIndex;
            }
            set
            {
                cmbJobFieldType.SelectedIndex = value;
            }
        }

        List<AgencyJobContract> IViewAgencyJobPostingView.LstAgencyJobPosting
        {
            get
            {
                if (!ViewState["LstAgencyJobPosting"].IsNull())
                {
                    return (ViewState["LstAgencyJobPosting"]) as List<AgencyJobContract>;
                }
                return new List<AgencyJobContract>();
            }
            set
            {
                ViewState["LstAgencyJobPosting"] = value;
            }
        }

        Boolean IViewAgencyJobPostingView.IsAppliacnt
        {
            get
            {
                if (!ViewState["IsAppliacnt"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsAppliacnt"]);
                }
                return false;
            }
            set
            {
                ViewState["IsAppliacnt"] = value;
            }
        }

        Int32 IViewAgencyJobPostingView.TenantId
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

        List<TenantDetailContract> IViewAgencyJobPostingView.LstTenant
        {
            set
            {
                cmbInstitution.DataSource = value;
                cmbInstitution.DataBind();
            }
        }

        List<Int32> IViewAgencyJobPostingView.SelectedTenantIds
        {
            get
            {
                List<Int32> _selectedTenantIds = new List<int>();
                foreach (RadComboBoxItem item in cmbInstitution.Items)
                {
                    if (item.Checked == true)
                        _selectedTenantIds.Add(Convert.ToInt32(item.Value));
                }
                return _selectedTenantIds;
            }
            set
            {
                foreach (Int32 item in value)
                {
                    cmbInstitution.Items.FindItemByValue(item.ToString()).Checked = true;
                }
            }
        }

        Boolean IViewAgencyJobPostingView.IsAdminLoggedIn
        {
            get;
            set;
        }

        #endregion

        #region Public Properties

        public ViewAgencyJobPostingPresenter Presenter
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

        public IViewAgencyJobPostingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Boolean Visiblity
        {
            get
            {
                if (ViewState["Visiblity"] != null)
                    return (Convert.ToBoolean(ViewState["Visiblity"]));
                return true;
            }
            set
            {
                ViewState["Visiblity"] = value;
            }
        }

        public String ControlUseType
        {
            get
            {
                if (ViewState["ControlUseType"] != null)
                    return (Convert.ToString(ViewState["ControlUseType"]));
                return String.Empty;
            }
            set
            {
                ViewState["ControlUseType"] = value;
            }
        }

        #region Custom paging parameters

        public Int32 CurrentPageIndex
        {
            get
            {
                return grdAgencyJobs.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdAgencyJobs.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        public int PageSize
        {
            get
            {
                return grdAgencyJobs.MasterTableView.PageSize;
            }
            set
            {
                grdAgencyJobs.MasterTableView.PageSize = value;
                grdAgencyJobs.PageSize = value;
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
                grdAgencyJobs.VirtualItemCount = value;
                grdAgencyJobs.MasterTableView.VirtualItemCount = value;
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


        #endregion
        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                Presenter.CheckIfUserIsApplicant();
                if (!CurrentViewContext.IsAppliacnt)
                {
                    base.Title = "Job Board";
                    base.SetPageTitle("Job Board");
                    base.OnInit(e);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    //base.Title = "Job Board";
                    ////base.SetPageTitle("Job Board");
                    //base.OnInit(e);

                    if (!this.IsPostBack || this.ControlUseType == AppConsts.DASHBOARD)
                    {
                        Presenter.OnViewInitialized();
                        var args = new Dictionary<String, String>();
                        if (CurrentViewContext.IsAppliacnt)
                        {
                            if (!Request.QueryString["argss"].IsNullOrEmpty())
                            {
                                args.ToDecryptedQueryString(Request.QueryString["argss"]);
                                if (!args.ContainsKey("CancelClick") || args.ContainsKey("CancelClick").IsNullOrEmpty())
                                {
                                    Session["JobPostingData"] = null;
                                    Session["GridCustomPaging"] = null;
                                }
                            }
                        }
                        else if (!CurrentViewContext.IsAppliacnt)
                        {
                            if (!Request.QueryString["args"].IsNullOrEmpty())
                            {
                                args.ToDecryptedQueryString(Request.QueryString["args"]);
                                if (!args.ContainsKey("CancelClick") || args.ContainsKey("CancelClick").IsNullOrEmpty())
                                {
                                    Session["JobPostingData"] = null;
                                    Session["GridCustomPaging"] = null;
                                }
                            }
                            else
                            {
                                grdAgencyJobs.Visible = false;
                                Session["JobPostingData"] = null;
                                Session["GridCustomPaging"] = null;
                            }
                        }
                        Presenter.IsAdminLoggedIn();
                        if (CurrentViewContext.IsAdminLoggedIn)
                        {
                            rfvInstitution.Enabled = true;
                            dvInstitution.Visible = true;
                            Presenter.GetTenants();
                        }
                        else
                        {
                            rfvInstitution.Enabled = false;
                            dvInstitution.Visible = false;
                        }

                        if (cmbJobFieldType.Items.Count() == AppConsts.NONE)
                        {
                            Presenter.GetViewAgencyJobFieldType();
                            cmbJobFieldType.DataSource = CurrentViewContext.LstJobFieldType;
                            cmbJobFieldType.DataBind();
                            cmbJobFieldType.Items.Insert(0, new RadComboBoxItem { Text = "--SELECT--", Value = "0" });
                        }

                        if (!this.IsPostBack)
                        {
                            GetSeesionData();

                        }
                    }
                    Presenter.OnViewLoaded();
                }
                if (!Visiblity && !ControlUseType.IsNullOrEmpty() && ControlUseType == AppConsts.DASHBOARD)
                {
                    ResetControls();
                    ResetGridFilters();
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
        /// Reset Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_SubmitClick(object sender, EventArgs e)
        {
            try
            {
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
        /// Search Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_SaveClick(object sender, EventArgs e)
        {
            try
            {
                grdAgencyJobs.Visible = true;
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
        /// Cancel Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.IsAppliacnt)
                {
                    Response.Redirect(AppConsts.DASHBOARD_URL);
                }
                else
                {
                    Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), false);
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

        #region Grid Events
        protected void grdAgencyJobs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                Presenter.GetAgencyJobPosting();
                grdAgencyJobs.DataSource = CurrentViewContext.LstAgencyJobPosting;
                SetSessionData();
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

        protected void grdAgencyJobs_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String agencyJobID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyJobID"].ToString();


                    if (!CurrentViewContext.IsAppliacnt)
                    {
                        queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.VIEW_AGENCY_JOB_POST_DETAIL},
                                                                    {"IsApplicant",CurrentViewContext.IsAppliacnt.ToString()},
                                                                    {"AgencyJobID",agencyJobID}
                                                                 };
                        string url = String.Format("~/AgencyJobBoard/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                        Response.Redirect(url, true);
                    }
                    else
                    {
                        queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"IsApplicant",CurrentViewContext.IsAppliacnt.ToString()},
                                                                    {"AgencyJobID",agencyJobID},
                                                                    {"CancelClick","true"},
                                                                    {"ApplicantDashboard","true"}
                                                                 };

                        Response.Redirect(AppConsts.DASHBOARD_URL + "?MenuId=12&argss=" + queryString.ToEncryptedQueryString(), true);
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

        protected void grdAgencyJobs_SortCommand(object sender, GridSortCommandEventArgs e)
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

        protected void grdAgencyJobs_PreRender(object sender, EventArgs e)
        {
            try
            {
                foreach (GridDataItem item in grdAgencyJobs.MasterTableView.Items)
                {
                    string descriptionContent = item["JobDescription"].Text;
                    WclEditor editor = new WclEditor();
                    editor.Content = descriptionContent;

                    if (!string.IsNullOrEmpty(editor.Text) && editor.Text.Length > 200)
                    {
                        item["JobDescription"].Text = string.Concat(editor.Text.Substring(0, 200), "...");
                    }
                    else
                    {
                        item["JobDescription"].Text = editor.Text;
                    }

                    item["JobDescription"].ToolTip = editor.Text;
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

        #region [Private Methods]

        private void ResetGridFilters()
        {
            CurrentViewContext.GridCustomPaging.SortExpression = String.Empty;
            CurrentViewContext.GridCustomPaging.DefaultSortExpression = String.Empty;
            CurrentViewContext.VirtualRecordCount = AppConsts.NONE;
            grdAgencyJobs.MasterTableView.SortExpressions.Clear();
            grdAgencyJobs.CurrentPageIndex = 0;
            grdAgencyJobs.MasterTableView.CurrentPageIndex = 0;
            grdAgencyJobs.MasterTableView.IsItemInserted = false;
            grdAgencyJobs.MasterTableView.ClearEditItems();
            grdAgencyJobs.Rebind();
        }

        private void ResetControls()
        {
            txtJobTitle.Text = string.Empty;
            txtLocation.Text = string.Empty;
            txtCompany.Text = string.Empty;
            rbljobType.SelectedValue = "AAAA";
            cmbInstitution.ClearCheckedItems();
            cmbJobFieldType.ClearSelection();
        }

        private void GetSeesionData()
        {
            if (!Session["JobPostingData"].IsNullOrEmpty())
            {
                JobSearchContract seacrhContract = Session["JobPostingData"] as JobSearchContract;
                CurrentViewContext.JobTitle = seacrhContract.JobTitle;
                CurrentViewContext.Location = seacrhContract.Location;
                CurrentViewContext.Company = seacrhContract.Company;
                CurrentViewContext.JobTypeCode = seacrhContract.JobTypeCode;
                CurrentViewContext.SelectedTenantIds = seacrhContract.lstSelectedTenantIds;

                if (!Session["GridCustomPaging"].IsNullOrEmpty())
                {
                    CustomPagingArgsContract GridPagingContract = Session["GridCustomPaging"] as CustomPagingArgsContract;
                    CurrentViewContext.GridCustomPaging = GridPagingContract;
                    CurrentViewContext.PageSize = GridPagingContract.PageSize;
                    CurrentViewContext.CurrentPageIndex = GridPagingContract.CurrentPageIndex;
                }
                var args = new Dictionary<String, String>();
                if (!Request.QueryString["argss"].IsNullOrEmpty())
                {
                    args.ToDecryptedQueryString(Request.QueryString["argss"]);
                    if (!args.ContainsKey("ApplicantDashboard") || args.ContainsKey("ApplicantDashboard").IsNullOrEmpty())
                    {
                        Session["GridCustomPaging"] = null;
                        Session["JobPostingData"] = null;
                    }
                }
            }
        }

        private void SetSessionData()
        {
            JobSearchContract seacrhContract = new JobSearchContract();
            seacrhContract.JobTitle = CurrentViewContext.JobTitle;
            seacrhContract.Location = CurrentViewContext.Location;
            seacrhContract.Company = CurrentViewContext.Company;
            seacrhContract.JobTypeCode = CurrentViewContext.JobTypeCode;
            seacrhContract.lstSelectedTenantIds = CurrentViewContext.SelectedTenantIds;

            Session["JobPostingData"] = seacrhContract;

            CustomPagingArgsContract GridPagingContract = new CustomPagingArgsContract();
            GridPagingContract = CurrentViewContext.GridCustomPaging;
            GridPagingContract.PageSize = CurrentViewContext.PageSize;
            GridPagingContract.CurrentPageIndex = CurrentViewContext.CurrentPageIndex;
            Session["GridCustomPaging"] = GridPagingContract;
        }

        #endregion
    }
}