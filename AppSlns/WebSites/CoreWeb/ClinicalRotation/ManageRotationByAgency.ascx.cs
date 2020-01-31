using System;
using System.Linq;
using System.Collections.Generic;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.Configuration;
using System.IO;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.Common;
using System.Web.UI;
using Entity.SharedDataEntity;



namespace CoreWeb.ClinicalRotation.Views
{
    public partial class ManageRotationByAgency : BaseUserControl, IManageRotationByAgencyView
    {
        #region Private Variables
        private ManageRotationByAgencyPresenter _presenter = new ManageRotationByAgencyPresenter();
        private String _viewType;
        private ClinicalRotationDetailContract _searchContract = null;
        private Int32 tenantId = 0;
        #endregion

        #region Properties
        #region Public Properties
        public ManageRotationByAgencyPresenter Presenter
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

        public IManageRotationByAgencyView CurrentViewContext
        {
            get
            {
                return this;
            }
        }


        List<ClinicalRotationDetailContract> IManageRotationByAgencyView.ClinicalRotationData
        {
            get;
            set;
        }

        Int32 IManageRotationByAgencyView.TenantID
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

        //Represnts Selected Institution ID
        Int32 IManageRotationByAgencyView.SelectedTenantID
        {
            get;
            set;
        }

        //private List<String> _selectedTenantIds = null;
        //public List<String> SelectedTenantIds
        //{
        //    get
        //    {
        //        _selectedTenantIds = new List<String>();
        //        foreach (RadComboBoxItem item in ddlTenantName.Items)
        //        {
        //            if (item.Checked == true)
        //                _selectedTenantIds.Add((item.Value));
        //        }
        //        return _selectedTenantIds;
        //    }
        //    set
        //    {
        //        _selectedTenantIds = value;
        //        foreach (RadComboBoxItem item in ddlTenantName.Items)
        //        {
        //            if (_selectedTenantIds.Contains((item.Value)))
        //                item.Checked = true;
        //        }
        //    }
        //}
        //private List<String> _selectedAgencyIds = null;
        //public List<String> SelectedAgencyIds
        //{
        //    get
        //    {
        //        _selectedAgencyIds = new List<String>();
        //        foreach (RadComboBoxItem item in ddlAgency.Items)
        //        {
        //            if (item.Checked == true)
        //                _selectedTenantIds.Add((item.Value));
        //        }
        //        return _selectedTenantIds;
        //    }
        //    set
        //    {
        //        _selectedAgencyIds = value;
        //        foreach (RadComboBoxItem item in ddlAgency.Items)
        //        {
        //            if (_selectedTenantIds.Contains((item.Value)))
        //                item.Checked = true;
        //        }
        //    }
        //}

        /// <summary>
        /// Returns the current logged-in user ID.
        /// </summary>
        Int32 IManageRotationByAgencyView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        List<TenantDetailContract> IManageRotationByAgencyView.lstTenant
        {
            get
            {
                if (!ViewState["lstTenant"].IsNull())
                {
                    return ViewState["lstTenant"] as List<TenantDetailContract>;
                }

                return new List<TenantDetailContract>();
            }
            set
            {
                ViewState["lstTenant"] = value;
            }
        }
        Int32 IManageRotationByAgencyView.SelectedRotationID
        {
            get;
            set;
        }

        bool IManageRotationByAgencyView.IsAdminLoggedIn
        {
            get;
            set;
        }
        Boolean IManageRotationByAgencyView.RebindGrid
        {
            get;
            set;
        }

        //Represents the list of all Agencies
        List<AgencyDetailContract> IManageRotationByAgencyView.lstAgency
        {
            get
            {
                if (!ViewState["lstAgency"].IsNull())
                {
                    return ViewState["lstAgency"] as List<AgencyDetailContract>;
                }

                return new List<AgencyDetailContract>();
            }
            set
            {
                ViewState["lstAgency"] = value;
            }
        }

        ////Represents Selected AgencyID
        //Int32 IManageRotationByAgencyView.SelectedAgencyID
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(ddlAgency.SelectedValue))
        //            return 0;
        //        return Convert.ToInt32(ddlAgency.SelectedValue);
        //    }
        //    set
        //    {
        //        if (value > 0)
        //        {
        //            ddlAgency.SelectedValue = value.ToString();
        //        }
        //        else
        //        {
        //            ddlAgency.SelectedIndex = value;
        //        }
        //    }
        //}

        //Represents ErrorMessage
        String IManageRotationByAgencyView.ErrorMessage
        {

            get;
            set;
        }

        //Represents SuccessMessage
        String IManageRotationByAgencyView.SuccessMessage
        {
            get;
            set;
        }

        List<ClientContactContract> IManageRotationByAgencyView.ClientContactList
        {
            get
            {
                if (!ViewState["ClientContactList"].IsNull())
                {
                    return ViewState["ClientContactList"] as List<ClientContactContract>;
                }

                return new List<ClientContactContract>();
            }
            set
            {
                ViewState["ClientContactList"] = value;
            }
        }

        List<WeekDayContract> IManageRotationByAgencyView.WeekDayList
        {
            get
            {
                if (!ViewState["WeekDayList"].IsNull())
                {
                    return ViewState["WeekDayList"] as List<WeekDayContract>;
                }

                return new List<WeekDayContract>();
            }
            set
            {
                ViewState["WeekDayList"] = value;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ClinicalRotationDetailContract IManageRotationByAgencyView.SearchContract
        {
            get
            {
                if (_searchContract.IsNull())
                {
                    GetSearchParameters();
                }
                return _searchContract;
            }
            set
            {
                _searchContract = value;
                SetSearchParameters();
            }
        }

        /// <summary>
        /// Get and set custom attribute list of type hierarchy.
        /// </summary>


        //Int32? IManageRotationByAgencyView.RotationID
        //{
        //    get
        //    {
        //        if (!ViewState["RotationID"].IsNull())
        //        {
        //            return (Int32?)ViewState["RotationID"];
        //        }
        //        return null;
        //    }
        //    set
        //    {
        //        ViewState["WeekDayList"] = value;
        //    }
        //}

        //List<int> IManageRotationByAgencyView.SelectedClientContacts
        //{
        //    get;
        //    set;
        //}


        /// <summary>
        /// Granular Permissions of the logged-in user.
        /// </summary>
        //Dictionary<String, String> IManageRotationByAgencyView.dicGranularPermissions
        //{
        //    get;
        //    set;
        //}

        #region UAT-2424
        //public List<ClinicalRotationDetailContract> lstClinicalRotation
        //{
        //    get
        //    {
        //        if (!ViewState["lstClinicalRotation"].IsNull())
        //        {
        //            return ViewState["lstClinicalRotation"] as List<ClinicalRotationDetailContract>;
        //        }

        //        return new List<ClinicalRotationDetailContract>();
        //    }
        //    set
        //    {
        //        ViewState["lstClinicalRotation"] = value;
        //    }
        //}




        #endregion

        #region UAT-2034:Phase 4 (16): Manage Rotation screen updates
        //Dictionary<Int32, Int32> IManageRotationByAgencyView.DicOfSelectedRotation
        //{
        //    get
        //    {
        //        if (!ViewState["DicOfSelectedRotation"].IsNull())
        //        {
        //            return (Dictionary<Int32, Int32>)ViewState["DicOfSelectedRotation"];
        //        }
        //        return new Dictionary<Int32, Int32>();
        //    }
        //    set
        //    {
        //        ViewState["DicOfSelectedRotation"] = value;
        //    }
        //}
        #endregion

        #region Custom paging parameters
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdRotations.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdRotations.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        public int PageSize
        {
            get
            {
                return grdRotations.PageSize;
            }
            set
            {
                grdRotations.MasterTableView.PageSize = value;
                grdRotations.PageSize = value;
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
                grdRotations.VirtualItemCount = value;
                grdRotations.MasterTableView.VirtualItemCount = value;
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


        #region UAT-2545

        public List<lkpArchiveState> lstArchiveState
        {
            set
            {
                rbArchiveStatus.DataSource = value.OrderBy(x => x.AS_Code);
                rbArchiveStatus.DataBind();
                rbArchiveStatus.SelectedValue = ArchiveState.Active.GetStringValue();//value.Where(x => x.AS_Code == lkpArchivalState.NonArchived.GetStringValue()).Select(x=>x.AS_Code).ToString();

            }
        }
        public List<String> SelectedArchiveStatusCode
        {
            get
            {
                if (!rbArchiveStatus.SelectedValue.IsNullOrEmpty())
                {
                    List<String> selectedCodes = new List<String>();
                    if (rbArchiveStatus.SelectedValue.Equals(ArchiveState.All.GetStringValue()))
                    {
                        return null;
                    }
                    else
                    {
                        selectedCodes.Add(rbArchiveStatus.SelectedValue);
                    }
                    return selectedCodes;
                }
                else
                    return null;
            }
            set
            {
                rbArchiveStatus.SelectedValue = value.FirstOrDefault();
            }
        }
        #endregion

        //Represents Selected AgencyID
        String IManageRotationByAgencyView.SelectedAgencyIDs
        {
            get
            {
                AgencyhierarchyCollection agencyhierarchyCollection = ucAgencyHierarchyMultipleToSearchRotation.GetAgencyHierarchyCollection();
                string agencyIDs = string.Empty;

                if (!agencyhierarchyCollection.IsNullOrEmpty() && !agencyhierarchyCollection.agencyhierarchy.IsNullOrEmpty())
                    agencyIDs = string.Join(",", agencyhierarchyCollection.agencyhierarchy.Select(d => d.AgencyID).Distinct().ToList());

                return agencyIDs;
            }
            set
            {
                ucAgencyHierarchyMultipleToSearchRotation.SelectedAgecnyIds = value;
            }
        }

        #endregion
        #endregion

        #region Page Events

        /// <summary>
        /// Page OnInit Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Manage Rotations By Agency";
                base.SetPageTitle("Manage Rotations By Agency");
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
                (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search Rotations as per the criteria entered above";
                (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
                (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";

                if (!this.IsPostBack)
                {
                    if (Request.QueryString["args"].IsNull())
                    {
                        grdRotations.Visible = false;
                    }
                    Presenter.OnViewInitialized();
                    BindTenant();
                    CaptureQuerystringParameters();
                    BindControls();
                    GetSessionValues();
                    fsucCmdBarButton.SaveButton.ValidationGroup = "grpFormSearch";
                    ((HiddenField)ucAgencyHierarchyMultipleToSearchRotation.FindControl("hdnIsRequestFromManageRotationByAgencyScrn")).Value = "True"; // UAT-4443
                    
                }
                Presenter.OnViewLoaded();

                ucAgencyHierarchyMultipleToSearchRotation.TenantId = CurrentViewContext.SelectedTenantID; //== AppConsts.NONE ? AppConsts.MINUS_ONE : CurrentViewContext.SelectedTenantID;
                ucAgencyHierarchyMultipleToSearchRotation.AgencyHierarchyNodeSelection = true;
                ucAgencyHierarchyMultipleToSearchRotation.IsInstitutionHierarchyRequired = false;                
                ucAgencyHierarchyMultipleToSearchRotation.IsAgencyNodeCheckable = true;
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
        protected void grdRotations_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (this.IsPostBack || CurrentViewContext.RebindGrid)
                {
                    GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                    GridCustomPaging.PageSize = PageSize;
                    Presenter.GetRotationDetail();
                    grdRotations.DataSource = CurrentViewContext.ClinicalRotationData;
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

        protected void grdRotations_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    //Reset rotation detail session
                    if (!Session[AppConsts.ROTATION_DETAIL_SESSION_KEY].IsNullOrEmpty())
                    {
                        Session[AppConsts.ROTATION_DETAIL_SESSION_KEY] = null;
                    }
                    SetSessionValues();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String ID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"].ToString();
                    var _agencyId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyID"].ToString();
                    var _tenantID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"].ToString();                  
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { ProfileSharingQryString.SelectedTenantId, _tenantID },
                                                                    { "Child",  AppConsts.ROTATION_DETAIL_CONTROL},
                                                                    { "ID", ID},
                                                                    {ProfileSharingQryString.AgencyId, _agencyId},
                                                                    {ProfileSharingQryString.SourceScreen,RotationDetailsScreenSource.MANAGE_ROTATION_BY_AGENCY.GetStringValue()}
                                                                 };
                    string url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }

               // Export functionality
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdRotations);

                }

                //Delete functionality
                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    CurrentViewContext.SelectedRotationID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"]);
                    CurrentViewContext.SelectedTenantID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"]);
                    //if clinical rotation members exist for rotation then user not able to deleted the rotation.
                    if (Presenter.IsClinicalRotationMembersExistForRotation())
                    {
                        base.ShowInfoMessage("You cannot delete this rotation as it is associated with other objects.");
                    }
                    else
                    {
                        Presenter.DeleteClinicalRotation();
                        base.ShowSuccessMessage("Clinical rotation deleted successfully.");
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

        protected void grdRotations_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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

        protected void grdRotations_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
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

        protected void grdRotations_ItemCreated(object sender, GridItemEventArgs e)
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

        #region Button Events
        protected void fsucCmdBarButton_SearchClick(object sender, EventArgs e)
        {
            try
            {
                grdRotations.Visible = true;
               // CurrentViewContext.DicOfSelectedRotation = new Dictionary<Int32, Int32>();
                ResetGridFilters();
                grdRotations.Rebind();
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

        protected void fsucCmdBarButton_ResetClick(object sender, EventArgs e)
        {
            try
            {
                ResetTenant();
                ResetControls();
                ResetGridFilters();
                CurrentViewContext.ClinicalRotationData = new List<ClinicalRotationDetailContract>();
                CurrentViewContext.VirtualRecordCount = 0;
                //CurrentViewContext.CurrentPageIndex = 1;
                grdRotations.DataSource = CurrentViewContext.ClinicalRotationData;
                grdRotations.DataBind();

                ucAgencyHierarchyMultipleToSearchRotation.Reset();
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

        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // sender.na
                GridEditFormItem editForm = (sender as LinkButton).NamingContainer as GridEditFormItem;
                if (editForm.IsNotNull())
                {
                    WclAsyncUpload fileUpload = editForm.FindControl("uploadControl") as WclAsyncUpload;
                    Label lblUploadFormName = editForm.FindControl("lblUploadFormName") as Label;
                    LinkButton lnkRemove = editForm.FindControl("lnkRemove") as LinkButton;
                    lblUploadFormName.Visible = false;
                    lblUploadFormName.Text = String.Empty;
                    fileUpload.Visible = true;
                    lnkRemove.Visible = false;
                }
                hdnFileRemoved.Value = true.ToString();
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


        #region Private Methods
        private void BindArchiveFilter()
        {
            Presenter.GetArchiveStateList();
        }

        /// <summary>
        /// Method to Bind Tenant Dropdown and call to Bind UserGroup and Agency Dropdown
        /// </summary>
        private void BindControls()
        {
            //BindAgency();
            BindContacts();
            BindWeekDays();
            BindArchiveFilter();
        }

        private void BindTenant()
        {
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();
            Presenter.IsAdminLoggedIn();
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                ddlTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantID = 0;
            }
            else
            {
                //CurrentViewContext.SelectedTenantID = CurrentViewContext.TenantID;
            }
        }

        /// <summary>
        /// Method to Bind Agencies
        /// </summary>
        private void BindAgency()
        {
            Presenter.GetAllAgencies();
            //ddlAgency.DataSource = CurrentViewContext.lstAgency;
            //ddlAgency.DataBind();
        }

        /// <summary>
        /// Method to Bind Agencies
        /// </summary>
        private void BindContacts()
        {
            Presenter.GetClientContacts();
            ddlContacts.DataSource = CurrentViewContext.ClientContactList;
            ddlContacts.DataBind();
        }

        private void BindWeekDays()
        {
            Presenter.GetWeekDays();
            ddlDays.DataSource = CurrentViewContext.WeekDayList;
            ddlDays.DataBind();
        }

        /// <summary>
        /// Method to Reset Grid 
        /// </summary>
        private void ResetGridFilters()
        {
            grdRotations.MasterTableView.SortExpressions.Clear();
            grdRotations.CurrentPageIndex = 0;
            grdRotations.MasterTableView.CurrentPageIndex = 0;
            grdRotations.MasterTableView.IsItemInserted = false;
            grdRotations.MasterTableView.ClearEditItems();
            // grdRotations.Rebind();
        }

        private void ResetTenant()
        {
            Presenter.IsAdminLoggedIn();
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                CurrentViewContext.SelectedTenantID = AppConsts.NONE;
            }
            else
            {
                // CurrentViewContext.SelectedTenantID = CurrentViewContext.TenantID;
            }
        }
        private void ResetControls()
        {
            BindControls();
            CurrentViewContext.SelectedAgencyIDs = string.Empty;
            txtComplioId.Text = String.Empty;
            txtRotationName.Text = String.Empty;
            txtDepartment.Text = String.Empty;
            txtProgram.Text = String.Empty;
            txtCourse.Text = String.Empty;
            txtUnit.Text = String.Empty;
            txtRecommendedHrs.Text = String.Empty;
            txtStudents.Text = String.Empty;
            txtShift.Text = String.Empty;
            dpStartDate.Clear();
            dpEndDate.Clear();
            tpStartTime.Clear();
            tpEndTime.Clear();
            ddlDays.ClearCheckedItems();
            ddlContacts.ClearCheckedItems();
            txtTerm.Text = String.Empty;
            txtTypeSpecialty.Text = String.Empty;
            hdnInstNodeIdNew.Value = String.Empty;
            //ddlAgency.ClearCheckedItems();
            ddlTenantName.ClearCheckedItems();
            rbArchiveStatus.SelectedIndex = 0;
            if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
            {
                rbArchiveStatus.Visible = true;
                //Presenter.GetArchiveStateList();
                //btnArchive.Enabled = true;
            }
            else
            {
                //rbArchiveStatus.Visible = false;
                //btnArchive.Enabled = true;
            }
            //UAT-2034:
          //  CurrentViewContext.DicOfSelectedRotation = new Dictionary<Int32, Int32>();

        }


        /// <summary>
        /// Get search parameters
        /// </summary>
        private void GetSearchParameters()
        {
            _searchContract = new ClinicalRotationDetailContract();            
            _searchContract.AgencyIDs = CurrentViewContext.SelectedAgencyIDs;
            _searchContract.TenantID = CurrentViewContext.SelectedTenantID;
            if (!txtComplioId.Text.Trim().IsNullOrEmpty())
                _searchContract.ComplioID = txtComplioId.Text.Trim();
            if (!txtRotationName.Text.Trim().IsNullOrEmpty())
                _searchContract.RotationName = txtRotationName.Text.Trim();
            if (!txtDepartment.Text.Trim().IsNullOrEmpty())
                _searchContract.Department = txtDepartment.Text.Trim();
            if (!txtProgram.Text.Trim().Trim().IsNullOrEmpty())
                _searchContract.Program = txtProgram.Text.Trim();
            if (!txtCourse.Text.Trim().IsNullOrEmpty())
                _searchContract.Course = txtCourse.Text.Trim();
            if (!txtUnit.Text.Trim().IsNullOrEmpty())
                _searchContract.UnitFloorLoc = txtUnit.Text.Trim();
            if (!txtRecommendedHrs.Text.Trim().IsNullOrEmpty())
                _searchContract.RecommendedHours = float.Parse(txtRecommendedHrs.Text.Trim());
            if (!txtStudents.Text.Trim().IsNullOrEmpty())
                _searchContract.Students = float.Parse(txtStudents.Text.Trim());
            if (!txtShift.Text.Trim().IsNullOrEmpty())
                _searchContract.Shift = txtShift.Text.Trim();
            if (!txtTerm.Text.Trim().IsNullOrEmpty())
                _searchContract.Term = txtTerm.Text.Trim();
            if (!dpStartDate.SelectedDate.IsNullOrEmpty())
                _searchContract.StartDate = dpStartDate.SelectedDate;
            if (!dpEndDate.SelectedDate.IsNullOrEmpty())
                _searchContract.EndDate = dpEndDate.SelectedDate;
            if (!tpStartTime.SelectedTime.IsNullOrEmpty())
                _searchContract.StartTime = tpStartTime.SelectedTime;
            if (!tpEndTime.SelectedTime.IsNullOrEmpty())
                _searchContract.EndTime = tpEndTime.SelectedTime;
            if (!txtTypeSpecialty.Text.Trim().IsNullOrEmpty())
                _searchContract.TypeSpecialty = txtTypeSpecialty.Text.Trim();
            _searchContract.DaysIdList = String.Join(",", ddlDays.CheckedItems.Select(x => x.Value));
            _searchContract.ContactIdList = String.Join(",", ddlContacts.CheckedItems.Select(x => x.Value));

            _searchContract.TenantIdList = String.Join(",", ddlTenantName.CheckedItems.Select(x => x.Value));
            _searchContract.AgencyIdList = CurrentViewContext.SelectedAgencyIDs; // String.Join(",", ddlAgency.CheckedItems.Select(x => x.Value));

        }

        /// <summary>
        /// Set search parameters
        /// </summary>
        private void SetSearchParameters()
        {
            CurrentViewContext.SelectedAgencyIDs = _searchContract.AgencyIDs;
            CurrentViewContext.SelectedTenantID = _searchContract.TenantID;
            txtComplioId.Text = _searchContract.ComplioID.IsNullOrEmpty() ? String.Empty : _searchContract.ComplioID;
            txtRotationName.Text = _searchContract.RotationName.IsNullOrEmpty() ? String.Empty : _searchContract.RotationName;
            txtDepartment.Text = _searchContract.Department.IsNullOrEmpty() ? String.Empty : _searchContract.Department;
            txtProgram.Text = _searchContract.Program.IsNullOrEmpty() ? String.Empty : _searchContract.Program;
            txtCourse.Text = _searchContract.Course.IsNullOrEmpty() ? String.Empty : _searchContract.Course;
            txtUnit.Text = _searchContract.UnitFloorLoc.IsNullOrEmpty() ? String.Empty : _searchContract.UnitFloorLoc;
            txtRecommendedHrs.Text = _searchContract.RecommendedHours.IsNullOrEmpty() ? String.Empty : _searchContract.RecommendedHours.ToString();
            txtStudents.Text = _searchContract.Students.IsNullOrEmpty() ? String.Empty : _searchContract.Students.ToString();
            txtShift.Text = _searchContract.Shift.IsNullOrEmpty() ? String.Empty : _searchContract.Shift;
            txtTerm.Text = _searchContract.Term.IsNullOrEmpty() ? String.Empty : _searchContract.Term;
            dpStartDate.SelectedDate = _searchContract.StartDate;
            dpEndDate.SelectedDate = _searchContract.EndDate;
            tpStartTime.SelectedTime = _searchContract.StartTime;
            tpEndTime.SelectedTime = _searchContract.EndTime;
            txtTypeSpecialty.Text = _searchContract.TypeSpecialty.IsNullOrEmpty() ? String.Empty : _searchContract.TypeSpecialty;

            String[] daysId = _searchContract.DaysIdList.Split(',');
            foreach (RadComboBoxItem item in ddlDays.Items)
            {
                item.Checked = daysId.Contains(item.Value);
            }

            String[] contactIds = _searchContract.ContactIdList.Split(',');
            foreach (RadComboBoxItem item in ddlContacts.Items)
            {
                item.Checked = contactIds.Contains(item.Value);
            }
            String[] AgencyIds = _searchContract.AgencyIdList == null? new string[0]: _searchContract.AgencyIdList.Split(',');
            //foreach (RadComboBoxItem item in ddlAgency.Items)
            //{
            //    item.Checked = AgencyIds.Contains(item.Value);
            //}
            String[] TenantIds = _searchContract.TenantIdList == null? new string[0] : _searchContract.TenantIdList.Split(',');
            foreach (RadComboBoxItem item in ddlTenantName.Items)
            {
                item.Checked = TenantIds.Contains(item.Value);
            }
        }

        /// <summary>
        /// To set controls values in session
        /// </summary>
        private void SetSessionValues()
        {
            ManageRotationSearchContract searchDataContract = new ManageRotationSearchContract();
            CurrentViewContext.SearchContract.ArchieveStatusId = rbArchiveStatus.SelectedValue; //UAT-2545
            searchDataContract.SearchParameters = CurrentViewContext.SearchContract;

            searchDataContract.GridCustomPagingArguments = CurrentViewContext.GridCustomPaging;
            //UAT-2034:
            //searchDataContract.DicOfSelectedRotation = CurrentViewContext.DicOfSelectedRotation;

            AgencyhierarchyCollection agencyhierarchyCollection = ucAgencyHierarchyMultipleToSearchRotation.GetAgencyHierarchyCollection();
            string agencyIDs = string.Empty;
            string nodeIds = string.Empty;

            if (!agencyhierarchyCollection.IsNullOrEmpty() && !agencyhierarchyCollection.agencyhierarchy.IsNullOrEmpty())
                agencyIDs = string.Join(",", agencyhierarchyCollection.agencyhierarchy.Select(d => d.AgencyID).Distinct().ToList());

            if (!agencyhierarchyCollection.IsNullOrEmpty() && !agencyhierarchyCollection.agencyhierarchy.IsNullOrEmpty())
                nodeIds = string.Join(",", agencyhierarchyCollection.agencyhierarchy.Select(d => d.AgencyNodeID).Distinct().ToList());

            searchDataContract.SearchParameters.AgencyIDs = agencyIDs;
            searchDataContract.SearchParameters.RootNodeID = ucAgencyHierarchyMultipleToSearchRotation.SelectedRootNodeId;
            searchDataContract.SearchParameters.HierarchyNodeIDList = nodeIds;

            //Session for maintaining control values
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ROTATION_SEARCH_SESSION_KEY, searchDataContract);
        }

        /// <summary>
        /// To get session values for controls
        /// </summary>
        private void GetSessionValues()
        {
            ManageRotationSearchContract searchDataContract = new ManageRotationSearchContract();
            if (Session[AppConsts.ROTATION_SEARCH_SESSION_KEY].IsNotNull())
            {
                searchDataContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.ROTATION_SEARCH_SESSION_KEY) as ManageRotationSearchContract;
                CurrentViewContext.SearchContract = searchDataContract.SearchParameters;
                CurrentViewContext.GridCustomPaging = searchDataContract.GridCustomPagingArguments;
                //UAT-2034:
                //CurrentViewContext.DicOfSelectedRotation = searchDataContract.DicOfSelectedRotation;
                rbArchiveStatus.SelectedValue = searchDataContract.SearchParameters.ArchieveStatusId;

                if (!searchDataContract.SearchParameters.AgencyIDs.IsNullOrEmpty())
                {
                    ucAgencyHierarchyMultipleToSearchRotation.SelectedAgecnyIds = searchDataContract.SearchParameters.AgencyIDs;
                    ucAgencyHierarchyMultipleToSearchRotation.SelectedRootNodeId = searchDataContract.SearchParameters.RootNodeID;
                    ucAgencyHierarchyMultipleToSearchRotation.SelectedNodeIds = searchDataContract.SearchParameters.HierarchyNodeIDList;
                }

                grdRotations.Rebind();
                //Reset session
                Session[AppConsts.ROTATION_SEARCH_SESSION_KEY] = null;
            }
        }

        private void CaptureQuerystringParameters()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey(ProfileSharingQryString.SelectedTenantId))
                {
                    CurrentViewContext.SelectedTenantID = Convert.ToInt32(args[ProfileSharingQryString.SelectedTenantId]);
                    if (!ddlTenantName.SelectedValue.IsNullOrEmpty())
                    {
                        Presenter.GetArchiveStateList();
                    }
                }
                if (args.ContainsKey(ProfileSharingQryString.AgencyId))
                {
                    CurrentViewContext.SelectedAgencyIDs = Convert.ToString(args[ProfileSharingQryString.AgencyId]);
                }
                if (args.ContainsKey("RebindGrid"))
                {
                    if (args["RebindGrid"] == AppConsts.YES)
                        CurrentViewContext.RebindGrid = true;
                    else
                        CurrentViewContext.RebindGrid = false;
                }
            }
            //if user navigate to other feature from detail screen and return to manage rotation again.
            else
                Session[AppConsts.ROTATION_SEARCH_SESSION_KEY] = null;
        }

        #endregion

    }
}