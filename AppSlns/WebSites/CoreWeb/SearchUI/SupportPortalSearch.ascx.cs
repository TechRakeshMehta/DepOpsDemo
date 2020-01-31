using CoreWeb.SearchUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.SearchUI;

namespace CoreWeb.SearchUI.Views
{
    public partial class SupportPortalSearch : BaseUserControl, ISupportPortalSearchView
    {

        #region Variables

        #region Private Variables

        private SupportPortalSearchPresenter _presenter = new SupportPortalSearchPresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private List<Int32> _selectedTenantIds = null;
        private List<String> _selectedUserTypeIds = null; //UAT-4020
        private String _selectedUserTypeCodes = null; //UAT-4020
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        public SupportPortalSearchPresenter Presenter
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

        public ISupportPortalSearchView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 ISupportPortalSearchView.TenantId
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

        public String IsAccountActivated
        {
            get
            {
                return rbAccountActivated.SelectedValue.ToString();
            }
            set
            {
                rbAccountActivated.SelectedValue = value;
            }
        }

        List<Entity.Tenant> ISupportPortalSearchView.lstTenant
        {
            get;
            set;

        }

        List<Int32> ISupportPortalSearchView.SelectedTenantIds
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



        String ISupportPortalSearchView.ApplicantUserName
        {
            get
            {
                return txtuserName.Text;
            }
            set
            {
                txtuserName.Text = value;
            }
        }

        String ISupportPortalSearchView.ApplicantFirstName
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

        String ISupportPortalSearchView.ApplicantLastName
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

        DateTime? ISupportPortalSearchView.DOB
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

        String ISupportPortalSearchView.SSN
        {
            get
            {
                //UAT-4355
                return txtSSN.TextWithPrompt;
                //return txtSSN.Text;
            }
            set
            {
                txtSSN.Text = value;
            }
            //get;
            //set;
        }
        String ISupportPortalSearchView.EmailAddress
        {
            get
            {
                return txtEmailAddress.Text;
            }
            set
            {
                txtEmailAddress.Text = value;
            }
            //get;
            //set;
        }
        Int32 ISupportPortalSearchView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        List<ApplicantData> ISupportPortalSearchView.lstApplicantData
        {
            get;
            set;
        }

        SearchItemDataContract ISupportPortalSearchView.searchContract
        {
            get;
            set;
        }

        #region Custom Paging

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdSupportPortal.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdSupportPortal.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>

        public Int32 PageSize
        {
            get
            {
                return grdSupportPortal.PageSize; //grdSupportPortal.MasterTableView.PageSize;  //UAT-4247|| Bug ID: 22009 
            }
            set
            {
                //grdSupportPortal.MasterTableView.PageSize = value;   //UAT-4247|| Bug ID: 22009 
                grdSupportPortal.PageSize = value;
            }
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>

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
                grdSupportPortal.VirtualItemCount = value;
                grdSupportPortal.MasterTableView.VirtualItemCount = value;
            }
        }

        #endregion

        //START UAT-3157
        Int32 ISupportPortalSearchView.PreferredSelectedTenantID
        {
            get
            {
                if (!ViewState["PreferredSelectedTenantID"].IsNull())
                {
                    return (Int32)ViewState["PreferredSelectedTenantID"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["PreferredSelectedTenantID"] = value;
            }
        }
        //END UAT-3157

        //UAT-4020
        //Represents the list of user types: Applicant and Instr/Preceptor
        Dictionary<String, String> ISupportPortalSearchView.dicUserTypes
        {
            get
            {
                if (!ViewState["dicUserTypes"].IsNullOrEmpty())
                    return (Dictionary<String, String>)ViewState["dicUserTypes"];
                return new Dictionary<String, String>();
            }
            set
            {
                ViewState["dicUserTypes"] = value;
            }
        }

        String ISupportPortalSearchView.SelectedUserTypeCode
        {
            get
            {
                _selectedUserTypeIds = new List<String>();
                foreach (RadComboBoxItem item in ddlUserType.Items)
                {
                    if (item.Checked == true)
                        _selectedUserTypeIds.Add(Convert.ToString(item.Value));
                }
                return String.Join(",", new List<String>(_selectedUserTypeIds));
            }
            set
            {
                _selectedUserTypeCodes = value;
                _selectedUserTypeIds = _selectedUserTypeCodes.Split(',').ToList();
                foreach (RadComboBoxItem item in ddlUserType.Items)
                {
                    if (_selectedUserTypeIds.Contains(Convert.ToString(item.Value)))
                        item.Checked = true;
                }
            }

        }
        String ISupportPortalSearchView.SelectedUserTypeIds
        {
            get
            {
                return String.Join(",", ddlUserType.CheckedItems.Select(x => x.Value));
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Support Portal Search";
                base.SetPageTitle("Support Portal Search");
                fsucSupportPortalCmdBar.ExtraButton.CausesValidation = false;

                //if(cmbTenantName.SelectedValue)
                //cmbTenantName.EmptyMessage = "--SELECT--";
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

            if (!this.IsPostBack)
            {
                BindTenant();
                BindUserTypes(); //UAT-4020
                /*Start UAT-3157*/
                GetPreferredSelectedTenant();
                /*End UAT-3157*/
                CaptureQuerystringParameters();
                GetSessionValues();
                Session["isChecked"] = null;
            }
            //if (CurrentViewContext.SelectedTenantIds.IsNullOrEmpty())
            //{
            //    cmbTenantName.EmptyMessage = "--SELECT--";
            //}
        }

        #endregion

        #region Grid Events

        /// <summary>
        /// To set Data Source to the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSupportPortal_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                CurrentViewContext.GridCustomPaging = CurrentViewContext.GridCustomPaging;
                Presenter.GetSupportPortalSearchData();
                grdSupportPortal.DataSource = CurrentViewContext.lstApplicantData;
                ///*START UAT-3157*/
                //GetPreferredSelectedTenant();
                ///*END UAT-3157*/
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
        /// For grid Commands.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSupportPortal_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.InitInsertCommandName || e.CommandName == RadGrid.EditCommandName || e.CommandName == "ViewDetail")
                {
                    if (!Session[AppConsts.SUPPORT_PORTAL_SEARCH].IsNullOrEmpty())
                    {
                        Session[AppConsts.SUPPORT_PORTAL_SEARCH] = null;
                    }
                    SetSessionValues();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();

                    if (e.CommandName == RadGrid.EditCommandName || e.CommandName == "ViewDetail")
                    {
                        //String TenantID = (e.Item as GridDataItem)["TenantID"].Text;
                        String TenantID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"].ToString();
                        String OrganizationUserID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"].ToString();
                        // String UserId = (e.Item as GridDataItem)["UserID"].Text;
                        String UserId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"].ToString();
                        String UserType = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserType"].ToString();


                        if (UserType == "Instructor" || UserType == "Preceptor")
                        {
                            String ClientContactId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ClientContactID"].ToString();
                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", ChildControls.InstructorSupportPortalDetails},
                                                                    { "OrganizationUserId",OrganizationUserID},
                                                                    {"TenantId",TenantID},
                                                                    {"UserId",UserId},
                                                                    {"ClientContactId",ClientContactId},
                                                                    {"PageType", WorkQueueType.SupportPortalDetail.ToString()}
                                                                  };
                        }
                        else
                        {
                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", ChildControls.SupportPortalDetails},
                                                                    { "OrganizationUserId",OrganizationUserID},
                                                                    {"TenantId",TenantID},
                                                                    {"UserId",UserId},
                                                                    {"PageType", WorkQueueType.SupportPortalDetail.ToString()}
                                                                  };
                        }

                    }

                    string url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }

                //// Hide filter when exportig to pdf or word
                //if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                //    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                //{
                //    base.ConfigureExport(grdSupportPortal);
                //}
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


        protected void grdSupportPortal_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
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

        #region Button Events

        /// <summary>
        /// Button click to perform search.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucSupportPortalCmdBar_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                ResetGridFilters();
                grdSupportPortal.Rebind();
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
        /// To Reset Search Filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucSupportPortalCmdBar_ExtraClick(object sender, EventArgs e)
        {
            try
            {
                BindTenant();
                ClearFilters();
                ResetGridFilters();
                BindDefaultUserTypes();// UAT-4247                
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
        /// To Redirect to Home page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucSupportPortalCmdBar_CancelClick(object sender, EventArgs e)
        {
            try
            {
                Session[AppConsts.SUPPORT_PORTAL_SEARCH] = null;
                Dictionary<String, String> queryString = new Dictionary<String, String>();
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

        /// <summary>
        /// Method to bind the tenant Dropdown
        /// </summary>
        private void BindTenant()
        {
            Presenter.GetTenantList();
            cmbTenantName.DataSource = CurrentViewContext.lstTenant;
            cmbTenantName.DataBind();
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdSupportPortal.MasterTableView.FilterExpression = null;
            CurrentViewContext.GridCustomPaging.SortExpression = null;
            grdSupportPortal.MasterTableView.SortExpressions.Clear();
            grdSupportPortal.CurrentPageIndex = 0;
            grdSupportPortal.MasterTableView.CurrentPageIndex = 0;
            grdSupportPortal.Rebind();
        }

        //UAT-4020
        private void BindUserTypes()
        {
            Presenter.GetUserType();
            ddlUserType.DataSource = CurrentViewContext.dicUserTypes;
            ddlUserType.DataBind();
            // UAT - 4247
            SearchItemDataContract searchDataContract = new SearchItemDataContract();
            if (Session[AppConsts.SUPPORT_PORTAL_SEARCH].IsNotNull())
            {
                searchDataContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SUPPORT_PORTAL_SEARCH) as SearchItemDataContract;
                CurrentViewContext.SelectedUserTypeCode = searchDataContract.SelectedUserTypeCode;
                searchDataContract.SelectedUserTypeCode = CurrentViewContext.SelectedUserTypeCode;
            }
            else
            {
                BindDefaultUserTypes(); // UAT - 4247
            }
            // UAT - 4247
        }

        public void BindDefaultUserTypes()   // UAT - 4247
        {
            if (!CurrentViewContext.dicUserTypes.IsNullOrEmpty())
            {
                foreach (RadComboBoxItem item in ddlUserType.Items)
                {
                    if (item.Value == "AAAC" || item.Value == "AAAE")
                        item.Checked = true;
                }
            }
        }
        /// <summary>
        /// Method to clear the search filters.
        /// </summary>

        private void ClearFilters()
        {
            txtuserName.Text = String.Empty;
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            dpkrDOB.SelectedDate = null;
            txtSSN.Text = String.Empty;
            txtEmailAddress.Text = String.Empty;
            /*Start UAT-3157*/
            GetPreferredSelectedTenant();
            /*End UAT-3157*/
            grdSupportPortal.Rebind();
            // CurrentViewContext.SSN = null;
            ddlUserType.ClearCheckedItems();  //UAT-4020
            rbAccountActivated.SelectedValue = "2"; //UAT-4153          
        }

        /// <summary>
        /// Set the filter values in session.
        /// </summary>
        private void SetSessionValues()
        {
            SearchItemDataContract searchDataContract = new SearchItemDataContract();

            searchDataContract.IsBackToSearch = true;

            searchDataContract.ApplicantFirstName = CurrentViewContext.ApplicantFirstName;
            searchDataContract.ApplicantLastName = CurrentViewContext.ApplicantLastName;
            searchDataContract.UserName = CurrentViewContext.ApplicantUserName;

            searchDataContract.DateOfBirth = CurrentViewContext.DOB;
            searchDataContract.ApplicantSSN = CurrentViewContext.SSN;
            searchDataContract.EmailAddress = CurrentViewContext.EmailAddress;
            //searchDataContract.OrganizationUserId = CurrentViewContext.OrganizationUserID.Value;
            searchDataContract.SelectedTenants = CurrentViewContext.SelectedTenantIds;
            searchDataContract.GridCustomPagingArguments = CurrentViewContext.GridCustomPaging;

            //UAT-4020
            //if (!searchDataContract.SelectedUserTypeCode.IsNullOrEmpty())
            //{
            //    String[] userTypeId = searchDataContract.SelectedUserTypeCode.Split(',');
            //    foreach (RadComboBoxItem item in ddlUserType.Items)
            //    {
            //        item.Checked = userTypeId.Contains(item.Value);
            //    }
            //}

            searchDataContract.SelectedUserTypeCode = CurrentViewContext.SelectedUserTypeCode;
            searchDataContract.IsAccountActivated = CurrentViewContext.IsAccountActivated;            

            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SUPPORT_PORTAL_SEARCH, searchDataContract);
        }

        /// <summary>
        /// get the values from session.
        /// </summary>
        private void GetSessionValues()
        {
            SearchItemDataContract searchDataContract = new SearchItemDataContract();
            CustomPagingArgsContract gridCustomPaging = new CustomPagingArgsContract();
            if (Session[AppConsts.SUPPORT_PORTAL_SEARCH].IsNotNull())
            {
                searchDataContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SUPPORT_PORTAL_SEARCH) as SearchItemDataContract;
                CurrentViewContext.searchContract = searchDataContract;
                CurrentViewContext.SelectedTenantIds = searchDataContract.SelectedTenants;
                CurrentViewContext.GridCustomPaging = searchDataContract.GridCustomPagingArguments;
                CurrentViewContext.ApplicantFirstName = searchDataContract.ApplicantFirstName;
                CurrentViewContext.ApplicantLastName = searchDataContract.ApplicantLastName;
                CurrentViewContext.ApplicantUserName = searchDataContract.UserName;
                CurrentViewContext.SSN = searchDataContract.ApplicantSSN;
                CurrentViewContext.EmailAddress = searchDataContract.EmailAddress;
                if (!searchDataContract.DateOfBirth.IsNullOrEmpty())
                    CurrentViewContext.DOB = Convert.ToDateTime(searchDataContract.DateOfBirth);


                //UAT-4020
                //   CurrentViewContext.SelectedUserTypeCode = String.Join(",", ddlUserType.CheckedItems.Select(x => x.Value));
                CurrentViewContext.SelectedUserTypeCode = searchDataContract.SelectedUserTypeCode;
                CurrentViewContext.IsAccountActivated = searchDataContract.IsAccountActivated;
            }
            grdSupportPortal.Rebind();
            Session[AppConsts.SUPPORT_PORTAL_SEARCH] = null;
        }

        private void CaptureQuerystringParameters()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNullOrEmpty())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (!args.ContainsKey("CancelClick") || args.ContainsKey("CancelClick").IsNullOrEmpty())
                {
                    Session[AppConsts.SUPPORT_PORTAL_SEARCH] = null;
                }
            }
            else
            {
                Session[AppConsts.SUPPORT_PORTAL_SEARCH] = null;
            }
        }


        #region UAT-3157:- Sticky institution

        private void GetPreferredSelectedTenant()
        {
            if (CurrentViewContext.SelectedTenantIds.IsNullOrEmpty())
            {
                CurrentViewContext.PreferredSelectedTenantID = (Session["PreferredSelectedTenant"]).IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(Session["PreferredSelectedTenant"]);
                if (CurrentViewContext.PreferredSelectedTenantID > AppConsts.NONE)
                {
                    //cmbTenantName.SelectedValue = Convert.ToString(CurrentViewContext.PreferredSelectedTenantID);
                    foreach (RadComboBoxItem item in cmbTenantName.Items)
                    {
                        if (item.Value == Convert.ToString(CurrentViewContext.PreferredSelectedTenantID))
                            item.Checked = true;

                        if (item.Checked == true)
                            CurrentViewContext.SelectedTenantIds.Add(Convert.ToInt32(item.Value));
                        //if (_selectedTenantIds.Contains(Convert.ToInt32(CurrentViewContext.PreferredSelectedTenantID)))
                        //    item.Checked = true;
                    }
                }
            }
        }
        #endregion


        //UAT-4020
        protected void grdSupportPortal_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    RadButton btnSupportPortalDetail = ((RadButton)e.Item.FindControl("btnSupportPortalDetail"));
                    // UAT:4153 Account Activated" (can be Yes/No)
                    if (dataItem["ApplicantAccountActivated"].Text == "true" || dataItem["ApplicantAccountActivated"].Text == "True" || dataItem["ApplicantAccountActivated"].Text == "TRUE")
                        dataItem["ApplicantAccountActivated"].Text = "Yes";
                    else
                        dataItem["ApplicantAccountActivated"].Text = "No";

                    if (Convert.ToString(dataItem["UserType"].Text) == "Applicant" || Convert.ToString(dataItem["UserType"].Text) == "Instructor" || Convert.ToString(dataItem["UserType"].Text) == "Preceptor")
                    {
                        btnSupportPortalDetail.Visible = true;
                    }
                    else
                    {
                        btnSupportPortalDetail.Visible = false;
                    }
                    if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                    {

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
    }
}