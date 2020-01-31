using System;
using System.Collections.Generic;
using System.Linq;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using System.Web.Configuration;
using Telerik.Web.UI;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.UI.Contract.CommonControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using INTSOF.ServiceDataContracts.Core;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using Entity.ClientEntity;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RotationStudentSearch : BaseUserControl, IRotationStudentSearchView
    {
        #region Variables

        private RotationStudentSearchPresenter _presenter = new RotationStudentSearchPresenter();
        private CustomPagingArgsContract _gridCustomPaging;
        private RotationMemberSearchDetailContract _searchContract = null;

        //UAT-4013
        private List<String> _lstCodeForColumnConfig = new List<String>();
        private List<TenantDetailContract> _defaultTenants = null;
        private List<TenantDetailContract> _selectedTenants = null;
        private String _viewType;

        #endregion

        #region Properties

        public RotationStudentSearchPresenter Presenter
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

        public IRotationStudentSearchView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        RotationMemberSearchDetailContract IRotationStudentSearchView.SearchParameterContract
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

        List<RotationMemberSearchDetailContract> IRotationStudentSearchView.lstRotationMemberSearchData
        {
            get;
            set;
        }

        List<TenantDetailContract> IRotationStudentSearchView.lstTenant
        {
            get
            {
                _defaultTenants = new List<TenantDetailContract>();
                foreach (RadComboBoxItem item in cmbTenant.Items)
                {
                    TenantDetailContract abc = new TenantDetailContract()
                    {
                        TenantID = Convert.ToInt32(item.Value),
                        TenantName = item.Text
                    };
                    _defaultTenants.Add(abc);
                }
                return _defaultTenants;
            }
            set
            {
                cmbTenant.DataSource = value;
                cmbTenant.DataBind();
                hdnDefaultTenantIDs.Value = String.Join(",", value.Select(con => con.TenantID).ToList());
            }

            //set
            //{
            //    cmbTenant.DataSource = value;
            //    cmbTenant.DataBind();
            //}
        }

        Int32 IRotationStudentSearchView.SelectedTenantID
        {
            get
            {
                if (String.IsNullOrEmpty(cmbTenant.SelectedValue))
                    return 0;
                return Convert.ToInt32(cmbTenant.SelectedValue);

            }
            set
            {
                if (cmbTenant.Items.Count > 0)
                {
                    cmbTenant.SelectedValue = Convert.ToString(value);
                }
            }
        }

        //UAT-4013
        List<TenantDetailContract> IRotationStudentSearchView.SelectedTenantIDs
        {
            get
            {
                _selectedTenants = new List<TenantDetailContract>();
                foreach (var item in cmbTenant.Items.Where(itm => itm.Checked))
                {
                    TenantDetailContract tenantDetailContract = new TenantDetailContract()
                    {
                        TenantID = Convert.ToInt32(item.Value),
                        TenantName = item.Text
                    };
                    _selectedTenants.Add(tenantDetailContract);
                }
                hdnSelectedTenantIds.Value = String.Join(",", _selectedTenants.Select(n => n.TenantID).ToList()); //UAT-3165  

                return _selectedTenants;
            }
            //set
            //{
            //    if (cmbTenant.Items.Count > 0)
            //    {
            //        cmbTenant.SelectedValue = hdnSelectedTenantIds.Value;
            //    }
            //}
        }

        Boolean IRotationStudentSearchView.IsReturntoRotationStudentSearch
        {
            get;
            set;
        }

        Boolean IRotationStudentSearchView.IsResetClicked
        {
            get;
            set;
        }


        Int32 IRotationStudentSearchView.OrganizationUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }
        List<String> IRotationStudentSearchView.SharedUserTypeCodes
        {
            get
            {
                List<String> lstSharedUserTypeCode = new List<String>();
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user.IsNotNull())
                {
                    lstSharedUserTypeCode = user.SharedUserTypesCode;
                }
                return lstSharedUserTypeCode;
            }
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        public String SuccessMessage
        {
            get;
            set;
        }
        List<AgencyDetailContract> IRotationStudentSearchView.lstAgency
        {
            set
            {
                //ddlAgency.DataSource = value;
                //ddlAgency.DataBind();
            }
        }

        List<Int32> IRotationStudentSearchView.lstSelectedTenantIDs 
        {
            get
            {
                return cmbTenant.CheckedItems.Select(col => Convert.ToInt32(col.Value)).ToList();
            }
            set
            {
                foreach (var val in value)
                {
                    cmbTenant.FindItemByValue(val.ToString()).Checked = true;
                }
            }
        }

        String IRotationStudentSearchView.lstSelectedAgencyIds
        {
            get
            {

                //String selectedAgencyIds = String.Empty;
                //foreach (var item in ddlAgency.Items.Where(itm => itm.Checked))  [ddlm]
                //{
                //    selectedAgencyIds += item.Value + ",";
                //}
                //if (!String.IsNullOrEmpty(selectedAgencyIds))
                //{
                //    selectedAgencyIds = selectedAgencyIds.Substring(0, selectedAgencyIds.LastIndexOf(','));
                //}
                //return selectedAgencyIds;

                List<Int32> selectedAgencyIds = new List<Int32>();
                AgencyhierarchyCollection agencyHierarchyCollection = ucAgencyHierarchyMultiple.GetAgencyHierarchyCollection();
                if (agencyHierarchyCollection.IsNotNull() && agencyHierarchyCollection.agencyhierarchy.IsNotNull())
                {
                    selectedAgencyIds.AddRange(agencyHierarchyCollection.agencyhierarchy.Select(d => d.AgencyID).ToList());
                }
                return String.Join(",", selectedAgencyIds.ToArray());
            }
        }
        Boolean IRotationStudentSearchView.IsAdvanceSearchPanelDisplay
        {
            get
            {
                if (!hdnAdvancesearch.Value.IsNullOrEmpty())
                {
                    return Convert.ToBoolean(hdnAdvancesearch.Value);
                }
                return false;
            }
            set
            {
                hdnAdvancesearch.Value = Convert.ToString(value);
                SetAdvanceSearchPanel();
            }
        }
        List<WeekDayContract> IRotationStudentSearchView.WeekDayList
        {
            set
            {
                ddlDays.DataSource = value;
                ddlDays.DataBind();
            }
        }

        //UAT-4013
        Dictionary<Int32, Boolean> IRotationStudentSearchView.CustomMessageOrgUserIds
        {
            get
            {
                if (!ViewState["CustomMessageOrgUserIds"].IsNull())
                {
                    return ViewState["CustomMessageOrgUserIds"] as Dictionary<Int32, Boolean>;
                }
                return new Dictionary<Int32, Boolean>();
            }
            set
            {
                ViewState["CustomMessageOrgUserIds"] = value;
            }
        }

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        Dictionary<Int32, Boolean> IRotationStudentSearchView.AssignOrganizationUserIds
        {
            get
            {
                if (!ViewState["SelectedApplicants"].IsNull())
                {
                    return ViewState["SelectedApplicants"] as Dictionary<Int32, Boolean>;
                }

                return new Dictionary<Int32, Boolean>();
            }
            set
            {
                ViewState["SelectedApplicants"] = value;
            }
        }

        /// Gets or Sets the value for selected Items.
        /// </summary>
        Dictionary<Int32, Tuple<Boolean, Int32>> IRotationStudentSearchView.RemovedClinicalRotationMemberIds
        {
            get
            {
                if (!ViewState["RemovedApplicants"].IsNull())
                {
                    return ViewState["RemovedApplicants"] as Dictionary<Int32, Tuple<Boolean, Int32>>;
                }
                return new Dictionary<Int32, Tuple<Boolean, Int32>>();
            }
            set
            {
                ViewState["RemovedApplicants"] = value;
            }
        }

        /// <summary>
        /// Gets or Sets the value for approved rotation memebers.
        /// </summary>
        Dictionary<Int32, Boolean> IRotationStudentSearchView.ApprovedClinicalRotationMemberIdsToRemove
        {
            get
            {
                if (!ViewState["ApprovedClinicalRotationMemberIdsToRemove"].IsNull())
                {
                    return ViewState["ApprovedClinicalRotationMemberIdsToRemove"] as Dictionary<Int32, Boolean>;
                }
                return new Dictionary<Int32, Boolean>();
            }
            set
            {
                ViewState["ApprovedClinicalRotationMemberIdsToRemove"] = value;
            }
        }

        #region GridCustom Properties

        public CustomPagingArgsContract StudentGridCustomPaging
        {
            get
            {
                if (_gridCustomPaging.IsNull())
                {
                    _gridCustomPaging = new CustomPagingArgsContract();
                }
                return _gridCustomPaging;
            }
            set
            {
                _gridCustomPaging = value;
                VirtualRecordCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }

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

        public Int32 PageSize
        {
            get
            {
                return grdRotations.PageSize;
            }
            set
            {
                grdRotations.PageSize = value;
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
                grdRotations.VirtualItemCount = value;
                grdRotations.MasterTableView.VirtualItemCount = value;
            }

        }

        //UAT-4013
        Int32 IRotationStudentSearchView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        Dictionary<Int32, String> IRotationStudentSearchView.lstSelectedOrgUserIDs
        {
            get;
            set;
        }

        Boolean IRotationStudentSearchView.RebindGrid
        {
            get;
            set;
        }

        public String lstTenantIds
        {
            get;
            set;
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
                base.Title = "Rotation Student Search";
                base.SetPageTitle("Rotation Student Search");
                //UAT-4013
                _lstCodeForColumnConfig.Add(Screen.grdRotationStudentSearch.GetStringValue());
                ColumnsConfiguration.CurrentViewContext.CurrentLoggedInUserID = CurrentViewContext.CurrentLoggedInUserId;
                ColumnsConfiguration.CurrentViewContext.OrganisationUserID = CurrentViewContext.OrganizationUserID;
                ColumnsConfiguration.CurrentViewContext.lstGridCode = _lstCodeForColumnConfig;
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
                    CaptureQuerystringParameters();
                    SetTenants();
                    GetSessionValues();
                }

                ucAgencyHierarchyMultiple.TenantId = CurrentViewContext.SelectedTenantID;
                ucAgencyHierarchyMultiple.CurrentOrgUserId = CurrentViewContext.OrganizationUserID;
                ucAgencyHierarchyMultiple.AgencyHierarchyNodeSelection = true;
                SetAdvanceSearchPanel(); //UAT-3211
                
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

        #region DropDownEvent
        /// <summary>
        /// Inserting --select-- by default on databinding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbTenant_DataBound(object sender, EventArgs e)
        {
            //cmbTenant.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        #endregion

        #region Grid Events

        protected void grdRotations_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (this.IsPostBack || CurrentViewContext.RebindGrid)
                {
                    StudentGridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                    StudentGridCustomPaging.PageSize = PageSize;
                    Presenter.GetRotationStudentDetails();
                    grdRotations.DataSource = CurrentViewContext.lstRotationMemberSearchData;
                    btnsendmail.Visible = true;
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

        protected void grdRotations_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    CurrentViewContext.StudentGridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.StudentGridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    CurrentViewContext.StudentGridCustomPaging.SortExpression = String.Empty;
                    CurrentViewContext.StudentGridCustomPaging.SortDirectionDescending = false;
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

        protected void grdRotations_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    if (!dataItem["ApplicantSSN"].Text.IsNullOrEmpty() && dataItem["ApplicantSSN"].Text != AppConsts.NON_BREAKING_SPACE)
                        dataItem["ApplicantSSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["ApplicantSSN"].Text));
                    if (!dataItem["DateOfBirth"].Text.IsNullOrEmpty() && dataItem["DateOfBirth"].Text != AppConsts.NON_BREAKING_SPACE)
                        dataItem["DateOfBirth"].Text = Presenter.GetMaskDOB(Convert.ToString(dataItem["DateOfBirth"].Text));
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


        //UAT-4013
        protected void grdRotations_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    SetSessionValues();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String rotationID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"].ToString();
                    String tenantID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"].ToString();
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { ProfileSharingQryString.SelectedTenantId, tenantID },
                                                                    { "Child",  AppConsts.ROTATION_STUDENT_DETAIL_CONTROL}, //ROTATION_DETAIL_CONTROL
                                                                    { ProfileSharingQryString.RotationId, rotationID},
                                                                    {ProfileSharingQryString.SourceScreen,AppConsts.INSTRCTR_PRECEPTR_ROTATION_SEARCH_URL},                      
                                                                    {ProfileSharingQryString.InvitationGroupID, String.Empty}
                                                                 };
                    string url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_SHARED_USER_GRID_SOURCE, SharedUserGridSource.INSTRCTR_PRECEPTR_ROTATION_SEARCH_URL.GetStringValue());
                    Response.Redirect(url, true);
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
        /// Event for Reset Controls 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar_ResetClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsResetClicked = true;
                ResetControls();
                ColumnsConfiguration.BindPageControls();
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
        /// Search button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar_SearchClick(object sender, EventArgs e)
        {
            try
            {
                ResetGridFilters();
                //UAT-4013
                ColumnsConfiguration.BindPageControls();
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
        /// Event to Redirect to shared user dashboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar_CancelClick(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", AppConsts.INSTRUCTOR_PRESEPTOR_DASHBOARD}
                                                                 };
                Response.Redirect(String.Format("~/ClinicalRotation/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
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
        /// Method to empty all the search controls 
        /// </summary>
        private void ResetControls()
        {
            cmbTenant.ClearSelection();
            // CurrentViewContext.SelectedTenantID = AppConsts.NONE;
            //CurrentViewContext.lstTenant = new List<TenantDetailContract>();
            CurrentViewContext.lstTenantIds = string.Empty;
            cmbTenant.ClearCheckedItems();
            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            txtComplioId.Text = String.Empty;
            txtRotationName.Text = String.Empty;
            txtTypeSpecialty.Text = String.Empty;
            txtDepartment.Text = String.Empty;
            txtProgram.Text = String.Empty;
            txtCourse.Text = String.Empty;
            txtTerm.Text = String.Empty;
            txtUnit.Text = String.Empty;
            txtRecommendedHrs.Text = String.Empty;
            txtShift.Text = String.Empty;
            tpStartTime.Clear();
            tpEndTime.Clear();
            dpStartDate.Clear();
            dpEndDate.Clear();
            hdnSelectedTenantIds.Value = string.Empty;
            ucAgencyHierarchyMultiple.Reset();
            ucAgencyHierarchyMultiple.CurrentOrgUserId = CurrentViewContext.OrganizationUserID;
            ucAgencyHierarchyMultiple.AgencyHierarchyNodeSelection = true;
            ucAgencyHierarchyMultiple.TenantId = AppConsts.NONE;
            ucAgencyHierarchyMultiple.Rebind();
            CurrentViewContext.IsAdvanceSearchPanelDisplay = false; //UAT-3211 Advanced Search updates
            ddlDays.ClearCheckedItems();
            ResetGridFilters();
        }

        /// <summary>
        /// Method to get the value from search filters
        /// </summary>
        private void GetSearchParameters()
        {
            _searchContract = new RotationMemberSearchDetailContract();
            //_searchContract.TenantID = CurrentViewContext.SelectedTenantID;

              if (!CurrentViewContext.SelectedTenantIDs.IsNullOrEmpty())
                _searchContract.lstTenantIDs = String.Join(",", CurrentViewContext.SelectedTenantIDs.Select(n => n.TenantID).ToList());
            //UAT-4013

            _searchContract.TenantIDs = CurrentViewContext.SelectedTenantIDs;
            _searchContract.ApplicantFirstName = txtFirstName.Text.IsNullOrEmpty() ? String.Empty : txtFirstName.Text.Trim();
            _searchContract.ApplicantLastName = txtLastName.Text.IsNullOrEmpty() ? String.Empty : txtLastName.Text.Trim();


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
            if (!txtTypeSpecialty.Text.Trim().IsNullOrEmpty())
                _searchContract.TypeSpecialty = txtTypeSpecialty.Text.Trim();
            //UAT-1769Addition of "# of Students" field on rotation creation and rotation details for all except students
            //if (!txtStudents.Text.Trim().IsNullOrEmpty())
            //    _searchContract.Students = float.Parse(txtStudents.Text.Trim());
            _searchContract.DaysIdList = String.Join(",", ddlDays.CheckedItems.Select(x => x.Value));
            if (!txtShift.Text.Trim().IsNullOrEmpty())
                _searchContract.Shift = txtShift.Text.Trim();
            if (!txtTerm.Text.Trim().IsNullOrEmpty())
                _searchContract.Term = txtTerm.Text.Trim();
            if (!dpStartDate.SelectedDate.IsNullOrEmpty())
                _searchContract.StartDate = dpStartDate.SelectedDate;
            if (!dpEndDate.SelectedDate.IsNullOrEmpty())
                _searchContract.EndDate = dpEndDate.SelectedDate;
            if (!tpStartTime.SelectedTime.IsNullOrEmpty())
            {
                _searchContract.StartTime = tpStartTime.SelectedTime;
                // _searchContract.RotationStartTime = tpStartTime.SelectedTime.ToString();
            }
            if (!tpEndTime.SelectedTime.IsNullOrEmpty())
            {
                _searchContract.EndTime = tpEndTime.SelectedTime;
                // _searchContract.RotationEndTime = tpEndTime.SelectedTime.ToString();
            }

            List<Int32> lstAgencyHierarchyIds = ucAgencyHierarchyMultiple.GetAgencyHierarchyCollection().agencyhierarchy.Select(sel => sel.AgencyNodeID).ToList();
            ucAgencyHierarchyMultiple.SelectedAgecnyIds = CurrentViewContext.lstSelectedAgencyIds.IsNullOrEmpty() ? hdnSelectedAgencyIDs.Value : CurrentViewContext.lstSelectedAgencyIds;
            ucAgencyHierarchyMultiple.SelectedNodeIds = lstAgencyHierarchyIds.IsNullOrEmpty() ? hdnSelectedNodeIDs.Value.HtmlEncode() : String.Join(",", lstAgencyHierarchyIds);
            //ucAgencyHierarchyMultiple.TenantId = CurrentViewContext.SearchContract.TenantDetailList.IsNullOrEmpty() ? -1 : AppConsts.NONE;
            ucAgencyHierarchyMultiple.TenantId = CurrentViewContext.SelectedTenantID.IsNullOrEmpty() ? 0 : CurrentViewContext.SelectedTenantID; //[ddlm]
            ucAgencyHierarchyMultiple.BindTree();
            _searchContract.AgencyIdList = CurrentViewContext.lstSelectedAgencyIds.IsNullOrEmpty() ? hdnSelectedAgencyIDs.Value.HtmlEncode() : CurrentViewContext.lstSelectedAgencyIds;

            if (!lstAgencyHierarchyIds.IsNullOrEmpty())
            {
                _searchContract.HierarchyNodes = String.Join(",", lstAgencyHierarchyIds);
            }
            else
            {
                _searchContract.HierarchyNodes = hdnSelectedNodeIDs.Value;
            }
            _searchContract.IsAdvanceSearchPanelDisplay = CurrentViewContext.IsAdvanceSearchPanelDisplay; //UAT-3211
        }

        /// <summary>
        /// Method to set the value in the search filters
        /// </summary>
        private void SetSearchParameters()
        {
            //CurrentViewContext.SelectedTenantID = _searchContract.TenantID;
            //CurrentViewContext.SelectedTenantIDs = _searchContract.TenantIDs;
            String tenantIds = String.Empty;
            if (!_searchContract.lstTenantIDs.IsNullOrEmpty()) //UAT-3596
                CurrentViewContext.lstSelectedTenantIDs = _searchContract.lstTenantIDs.Split(',').Select(col => Convert.ToInt32(col)).ToList();
            txtFirstName.Text = _searchContract.ApplicantFirstName.IsNullOrEmpty() ? String.Empty : _searchContract.ApplicantFirstName;
            txtLastName.Text = _searchContract.ApplicantLastName.IsNullOrEmpty() ? String.Empty : _searchContract.ApplicantLastName;

            txtComplioId.Text = _searchContract.ComplioID.IsNullOrEmpty() ? String.Empty : _searchContract.ComplioID;
            txtRotationName.Text = _searchContract.RotationName.IsNullOrEmpty() ? String.Empty : _searchContract.RotationName;
            txtDepartment.Text = _searchContract.Department.IsNullOrEmpty() ? String.Empty : _searchContract.Department;
            txtProgram.Text = _searchContract.Program.IsNullOrEmpty() ? String.Empty : _searchContract.Program;
            txtCourse.Text = _searchContract.Course.IsNullOrEmpty() ? String.Empty : _searchContract.Course;
            txtUnit.Text = _searchContract.UnitFloorLoc.IsNullOrEmpty() ? String.Empty : _searchContract.UnitFloorLoc;
            txtRecommendedHrs.Text = _searchContract.RecommendedHours.IsNullOrEmpty() ? String.Empty : _searchContract.RecommendedHours.ToString();
            //UAT-1769Addition of "# of Students" field on rotation creation and rotation details for all except students
            //  txtStudents.Text = _searchContract.Students.IsNullOrEmpty() ? String.Empty : _searchContract.Students.ToString();
            txtShift.Text = _searchContract.Shift.IsNullOrEmpty() ? String.Empty : _searchContract.Shift;
            txtTerm.Text = _searchContract.Term.IsNullOrEmpty() ? String.Empty : _searchContract.Term;
            //UAT-1641:As an Agency User, I should be able to be linked to multiple agencies
            //String[] agencyId = _searchContract.AgencyIdList.Split(',');


            if (!_searchContract.HierarchyNodes.IsNullOrEmpty())
            {
                hdnSelectedNodeIDs.Value = _searchContract.HierarchyNodes;
                hdnSelectedAgencyIDs.Value = _searchContract.AgencyIdList;
                ucAgencyHierarchyMultiple.SelectedAgecnyIds = _searchContract.AgencyIdList;
                ucAgencyHierarchyMultiple.SelectedNodeIds = _searchContract.HierarchyNodes;
                ucAgencyHierarchyMultiple.TenantId = AppConsts.NONE;
                ucAgencyHierarchyMultiple.BindTree();
            }

            dpStartDate.SelectedDate = _searchContract.StartDate;
            dpEndDate.SelectedDate = _searchContract.EndDate;
            tpStartTime.SelectedTime = _searchContract.StartTime;
            tpEndTime.SelectedTime = _searchContract.EndTime;
            //UAT-1467: Addition of "Type/Specialty" field on rotation creation and rotation details
            txtTypeSpecialty.Text = _searchContract.TypeSpecialty.IsNullOrEmpty() ? String.Empty : _searchContract.TypeSpecialty;

            String[] daysId = _searchContract.DaysIdList.Split(',');
            foreach (RadComboBoxItem item in ddlDays.Items)
            {
                item.Checked = daysId.Contains(item.Value);
            }
            //  cmbRotationStatus.SelectedValue = _searchContract.StatusTypeCode;
            //String[] reviewStatusId = _searchContract.ReviewStatusIDs.Split(',');
            //foreach (RadComboBoxItem item in cmbReviewStatus.Items)
            //{
            //    item.Checked = reviewStatusId.Contains(item.Value);
            //}

            // txtApplicantName.Text = _searchContract.ApplicantName.IsNullOrEmpty() ? string.Empty : _searchContract.ApplicantName;
            //txtCustomFieldsFilter.Text = _searchContract.IsCustomFilterApplied == true ? "Filter applied" : "No filter applied"; //UAT-3165
            CurrentViewContext.IsAdvanceSearchPanelDisplay = _searchContract.IsAdvanceSearchPanelDisplay; //UAT-3211

            //  rbInvitationArchiveStatus.SelectedValue = _searchContract.SelectedRotationInvitationArchiveStateCode;
        }

        /// <summary>
        /// Method to Reset Grid 
        /// </summary>
        private void ResetGridFilters()
        {
            grdRotations.MasterTableView.SortExpressions.Clear();
            grdRotations.CurrentPageIndex = 0;
            grdRotations.MasterTableView.IsItemInserted = false;
            grdRotations.MasterTableView.CurrentPageIndex = 0;
            grdRotations.Rebind();
            btnsendmail.Visible = true;
        }
        #region UAT-3211 Rotation tab updates(Advanced Search)
        private void SetAdvanceSearchPanel()
        {
            if (CurrentViewContext.IsAdvanceSearchPanelDisplay)
            {
                contentPanel.Attributes.Add("style", "display:block");
                mhdrPanel.Attributes["class"] = "mhdr";
                sectionPanel.Attributes["class"] = "section";
            }
            else
            {
                contentPanel.Attributes.Add("style", "display:none");
                mhdrPanel.Attributes.Add("class", String.Join(" ", mhdrPanel
                          .Attributes["class"]
                         .Split(' ')
                         .Except(new string[] { "", "colps" })
                         .Concat(new string[] { "colps" })
                         .ToArray()
                ));
                sectionPanel.Attributes.Add("class", String.Join(" ", sectionPanel
                         .Attributes["class"]
                         .Split(' ')
                         .Except(new string[] { "", "collapsed" })
                         .Concat(new string[] { "collapsed" })
                         .ToArray()
                ));
            }
        }


        #endregion

        #region 4013
        protected void chkRemoveItem_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Boolean isChecked = false;
            if (checkBox.IsNull())
            {
                return;
            }
            Dictionary<Int32, Boolean> customMessageUserList = CurrentViewContext.CustomMessageOrgUserIds;
            GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
            Dictionary<Int32, Tuple<Boolean, Int32>> selectedItems = CurrentViewContext.RemovedClinicalRotationMemberIds;
            Dictionary<Int32, Boolean> approvedStudentList = CurrentViewContext.ApprovedClinicalRotationMemberIdsToRemove;
            Int32 clinicalRotationMemberId = (Int32)dataItem.GetDataKeyValue("RotationID");

            Int32 orgUserId = (Int32)dataItem.GetDataKeyValue("OrganizationUserId");

            isChecked = ((CheckBox)dataItem.FindControl("chkRemoveItem")).Checked;
            if (isChecked)
            {
                if (!customMessageUserList.ContainsKey(orgUserId))
                    customMessageUserList.Add(orgUserId, true);
            }
            else
            {
                if (customMessageUserList != null && customMessageUserList.ContainsKey(orgUserId))
                    customMessageUserList.Remove(orgUserId);
            }
            CurrentViewContext.RemovedClinicalRotationMemberIds = selectedItems;
            CurrentViewContext.ApprovedClinicalRotationMemberIdsToRemove = approvedStudentList;
            CurrentViewContext.CustomMessageOrgUserIds = customMessageUserList;
        }

        protected void btnsendmail_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.CustomMessageOrgUserIds.IsNotNull() && !CurrentViewContext.CustomMessageOrgUserIds.Any())
                {
                    base.ShowAlertMessage("Please select user(s) to send message.", MessageType.Information);
                }
                else
                {
                    Presenter.GetSelectedOrganizatioUserIDs();
                    if (!Session["OrgUsersToList"].IsNullOrEmpty())
                    {
                        Session.Remove("OrgUsersToList");
                    }
                    Session["OrgUsersToList"] = CurrentViewContext.lstSelectedOrgUserIDs;

                    List<Int32> lstOrgUserIds = CurrentViewContext.CustomMessageOrgUserIds.Keys.Select(x => Convert.ToInt32(x)).ToList();
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

        private void CaptureQuerystringParameters()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey("SelectedTenantID"))
                {
                    CurrentViewContext.SelectedTenantID = Convert.ToInt32(args["SelectedTenantID"]);
                }

                if (args.ContainsKey("IsReturntoRotationStudentSearch"))
                {
                    CurrentViewContext.IsReturntoRotationStudentSearch = Convert.ToBoolean(args.GetValue("IsReturntoRotationStudentSearch"));
                }

                if (CurrentViewContext.IsReturntoRotationStudentSearch)
                    CurrentViewContext.RebindGrid = true;
                //if (this.IsPostBack)
                //    CurrentViewContext.RebindGrid = true;
                //else
                //    CurrentViewContext.RebindGrid = false;
            }
        }

        private void SetSessionValues()
        {
            ManageRotationMemberSearchContract searchDataContract = new ManageRotationMemberSearchContract();
            searchDataContract.SearchParameters = CurrentViewContext.SearchParameterContract;
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ROTATION_STUDENT_SEARCH_SESSION_KEY, searchDataContract);
        }
        /// <summary>
        /// To get session values for controls
        /// </summary>
        private void GetSessionValues()
        {
            ManageRotationMemberSearchContract searchDataContract = new ManageRotationMemberSearchContract();
            if (Session[AppConsts.ROTATION_STUDENT_SEARCH_SESSION_KEY].IsNotNull())
            {
                searchDataContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.ROTATION_STUDENT_SEARCH_SESSION_KEY) as ManageRotationMemberSearchContract;
                CurrentViewContext.SearchParameterContract = searchDataContract.SearchParameters;
                grdRotations.Rebind();
                Session[AppConsts.ROTATION_STUDENT_SEARCH_SESSION_KEY] = null;
            }
        }

        private void SetTenants()
        {
            ManageRotationMemberSearchContract searchDataContract = new ManageRotationMemberSearchContract();
            if (Session[AppConsts.ROTATION_STUDENT_SEARCH_SESSION_KEY].IsNotNull())
            {
                searchDataContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.ROTATION_STUDENT_SEARCH_SESSION_KEY) as ManageRotationMemberSearchContract;
              //  CurrentViewContext.lstSelectedAgencyHierarchyIDs = searchDataContract.SearchParameters.AgencyHierarchyIDs.Split(',').Select(col => Convert.ToInt32(col)).ToList();
                if (!searchDataContract.SearchParameters.lstTenantIDs.IsNullOrEmpty()) //UAT-3596
                    CurrentViewContext.lstSelectedTenantIDs = searchDataContract.SearchParameters.lstTenantIDs.Split(',').Select(col => Convert.ToInt32(col)).ToList(); //UAT-3596
            }
        }
       
        #endregion

        #endregion

    }
}