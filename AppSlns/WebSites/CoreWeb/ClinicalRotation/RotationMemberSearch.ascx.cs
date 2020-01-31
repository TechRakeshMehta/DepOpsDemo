#region NameSpace

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion
#region Project Specific
using CoreWeb.IntsofSecurityModel;
using INTSOF.Utils;
using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Modules.Common;
using Telerik.Web.UI;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using System.Web.Configuration;
using System.IO;
using Entity.ClientEntity;
#endregion
#endregion

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RotationMemberSearch : BaseUserControl, IRotationMemberSearchView
    {
        #region Private Variables
        private RotationMemberSearchPresenter _presenter = new RotationMemberSearchPresenter();
        private Int32 tenantId = 0;
        private RotationMemberSearchDetailContract _searchContract = null;
        private String _viewType;
        #endregion

        #region Properties

        #region Public Properties.
        public RotationMemberSearchPresenter Presenter
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

        public IRotationMemberSearchView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Boolean IRotationMemberSearchView.IsReset
        {
            get;
            set;
        }

        Int32 IRotationMemberSearchView.TenantID
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

        Int32 IRotationMemberSearchView.SelectedTenantID
        {
            get
            {
                if (String.IsNullOrEmpty(ddlTenant.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlTenant.SelectedValue);
            }
            set
            {
                if (ddlTenant.Items.Count > 0)
                {
                    ddlTenant.SelectedValue = Convert.ToString(value);
                }
            }
        }

        List<TenantDetailContract> IRotationMemberSearchView.lstTenant
        {
            get;
            set;
        }

        List<AgencyDetailContract> IRotationMemberSearchView.lstAgency
        {
            get;
            set;
        }

        Int32 IRotationMemberSearchView.SelectedAgencyID
        {
            get
            {
                //if (String.IsNullOrEmpty(ddlAgency.SelectedValue))     [ddl]
                //    return 0;
                //return Convert.ToInt32(ddlAgency.SelectedValue);
                //UAT-2600
                if (ucAgencyHierarchy.AgencyId.IsNullOrEmpty())
                    return 0;
                return ucAgencyHierarchy.AgencyId;
            }
            set
            {
                //if (ddlAgency.Items.Count > 0)  [ddl]
                //{
                //    ddlAgency.SelectedValue = value.ToString();
                //}
                //UAT-2600
                if (value > 0)
                {
                    ucAgencyHierarchy.AgencyId = value;
                }
                else
                {
                    ucAgencyHierarchy.AgencyId = value;
                }

            }
        }

        List<ClientContactContract> IRotationMemberSearchView.ClientContactList
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

        List<WeekDayContract> IRotationMemberSearchView.WeekDayList
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
        Boolean IRotationMemberSearchView.IsAdminLoggedIn
        {
            get;
            set;
        }

        //Represents ErrorMessage
        String IRotationMemberSearchView.ErrorMessage
        {

            get;
            set;
        }

        //Represents SuccessMessage
        String IRotationMemberSearchView.SuccessMessage
        {
            get;
            set;
        }

        Int32 IRotationMemberSearchView.CurrentUserID
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        RotationMemberSearchDetailContract IRotationMemberSearchView.SearchContract
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

        List<ApplicantDocumentContract> IRotationMemberSearchView.LstApplicantDocumentToExport { get; set; }
        List<RotationMemberSearchDetailContract> IRotationMemberSearchView.LstApplicantRotationToExport
        {
            get
            {
                if (!ViewState["SelectedAppRotMembers"].IsNull())
                {
                    return ViewState["SelectedAppRotMembers"] as List<RotationMemberSearchDetailContract>;
                }

                return new List<RotationMemberSearchDetailContract>();
            }
            set
            {
                ViewState["SelectedAppRotMembers"] = value;
            }
        }

        List<RotationMemberSearchDetailContract> IRotationMemberSearchView.LstRotationMemberSearchData
        {
            get;
            set;
        }

        //Represnts Selected UserGroupID
        Int32 IRotationMemberSearchView.SelectedUserGroupID
        {
            get
            {
                if (String.IsNullOrEmpty(ddlUserGroup.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlUserGroup.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlUserGroup.SelectedValue = value.ToString();
                }
                else
                {
                    ddlUserGroup.SelectedIndex = value;
                }
            }
        }

        //Represents the list of all user groups
        List<Entity.ClientEntity.UserGroup> IRotationMemberSearchView.lstUserGroup
        {
            get;
            set;
        }

        //UAT-3749
        //Represents the list of user types: Applicant and Instr/Preceptor
        Dictionary<String, String> IRotationMemberSearchView.dicUserTypes
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

        Int32 IRotationMemberSearchView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        #endregion

        #region Custom paging parameters
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdRotationMembers.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdRotationMembers.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        public int PageSize
        {
            get
            {
                return grdRotationMembers.PageSize;
            }
            set
            {
                grdRotationMembers.PageSize = value;
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
                grdRotationMembers.VirtualItemCount = value;
                grdRotationMembers.MasterTableView.VirtualItemCount = value;
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
        #endregion

        #region Drop Down Events
        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

            try
            {
                CurrentViewContext.LstApplicantRotationToExport = new List<RotationMemberSearchDetailContract>();
                CurrentViewContext.LstRotationMemberSearchData = new List<RotationMemberSearchDetailContract>();
                ViewState["ReBindGrid"] = "False";

                ResetControls();
                BindArchiveFilter();
                //ResetGridFilters(); //UAT-4214
                ucAgencyHierarchy.Reset(); //UAT-2600
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

        protected void ddlTenant_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlTenant.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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

        //protected void ddlAgency_DataBound(object sender, EventArgs e) [ddl]
        //{
        //    try
        //    {
        //        ddlAgency.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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

        /// <summary>
        /// UserGroup Dropdown DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUserGroup_DataBound(object sender, EventArgs e)
        {
            ddlUserGroup.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        }

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
                base.Title = "Manage Rotation Member Search";
                base.SetPageTitle("Manage Rotation Member Search");
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
                    if (Request.QueryString["args"].IsNull())
                    {
                        grdRotationMembers.Visible = false;
                        fsucCmdExport.Visible = false;
                    }
                    Presenter.OnViewInitialized();
                    BindTenant();
                    CaptureQuerystringParameters();
                    BindControls();
                    BindArchiveFilter(); //UAT-3435
                    GetSessionValues();
                    fsucCmdBarButton.SaveButton.ValidationGroup = "grpFormSubmit";
                    //BindArchiveFilter();
                }
                ifrExportDocument.Src = String.Empty;
                Presenter.IsAdminLoggedIn();
                Presenter.OnViewLoaded();
                ucAgencyHierarchy.TenantId = CurrentViewContext.SelectedTenantID; //UAT-2600

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
        protected void fsucCmdBarButton_ResetClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.VirtualRecordCount = 0; //UAT-4214
                CurrentViewContext.IsReset = true;
                ResetTenant();
                ResetControls();
                ResetGridFilters();
                ucAgencyHierarchy.Reset(); //UAT-2600
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
        protected void fsucCmdBarButton_SearchClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsReset = false;
                grdRotationMembers.Visible = true;
                fsucCmdExport.Visible = true;
                CurrentViewContext.LstApplicantRotationToExport = new List<RotationMemberSearchDetailContract>();
                ViewState["ReBindGrid"] = null;
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.LstApplicantRotationToExport != null && CurrentViewContext.LstApplicantRotationToExport.Count > 0)
                {
                    ExportDocuments();
                }
                else
                {
                    base.ShowInfoMessage("Please select applicant(s) to Export document.");
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
        protected void grdRotationMembers_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (ViewState["ReBindGrid"].IsNullOrEmpty())
                {
                    GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                    GridCustomPaging.PageSize = PageSize;
                    if (CurrentViewContext.IsReset && !CurrentViewContext.IsAdminLoggedIn)
                    {
                        grdRotationMembers.CurrentPageIndex = 0;
                        grdRotationMembers.MasterTableView.CurrentPageIndex = 0;
                        grdRotationMembers.VirtualItemCount = 0;
                        CurrentViewContext.LstRotationMemberSearchData = new List<RotationMemberSearchDetailContract>();
                    }
                    else
                        Presenter.GetRotationMemberSearchData();
                }
                if (CurrentViewContext.LstRotationMemberSearchData.IsNull())
                {
                    CurrentViewContext.LstRotationMemberSearchData = new List<RotationMemberSearchDetailContract>();
                }
                grdRotationMembers.DataSource = CurrentViewContext.LstRotationMemberSearchData;
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

        protected void grdRotationMembers_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.RebindGridCommandName)
                {
                    CurrentViewContext.LstApplicantRotationToExport = new List<RotationMemberSearchDetailContract>();
                }
                else if (e.CommandName == "ViewDetail")
                {
                    SetSessionValues();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String rotationID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"].ToString();
                    String organizationUserID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"].ToString();
                    String isApplicant = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsApplicant"].ToString();
                    String ReqPkgSubscriptionId = Presenter.GetRequirementSubscriptionIdByClinicalRotID(rotationID, organizationUserID, isApplicant).ToString();

                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.SelectedTenantID) },
                                                                    //{ "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL_CONTROL},
                                                                    { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                                                                    { ProfileSharingQryString.ReqPkgSubscriptionId, ReqPkgSubscriptionId },
                                                                    { ProfileSharingQryString.RotationId, rotationID }, 
                                                                    { ProfileSharingQryString.ApplicantId , organizationUserID },
                                                                    {ProfileSharingQryString.ControlUseType,AppConsts.ROTATION_MEMBER_SEARCH_USE_TYPE_CODE}
                                                                 };
                    string url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
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

        protected void grdRotationMembers_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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

        protected void grdRotationMembers_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    //RotationMemberSearchDetailContract compDoc = e.Item.DataItem as RotationMemberSearchDetailContract;

                    Int32 rotationID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"]);
                    Int32 orgUserId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"]);
                    List<RotationMemberSearchDetailContract> selectedRotationMemeber = CurrentViewContext.LstApplicantRotationToExport;

                    if (selectedRotationMemeber.IsNotNull() && rotationID != AppConsts.NONE && orgUserId != AppConsts.NONE)
                    {
                        if (selectedRotationMemeber.Any(cnd => cnd.OrganizationUserID == orgUserId && cnd.RotationID == rotationID))
                        {
                            CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectDocument"));
                            checkBox.Checked = true;
                        }
                    }
                }
                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdRotationMembers.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdRotationMembers.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectDocument"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem item = grdRotationMembers.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectAll"));
                            checkBox.Checked = true;
                        }
                    }
                }

                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    RadButton btnViewDetail = ((RadButton)e.Item.FindControl("btnViewDetail"));
                    Boolean IsPackageExistsInRotation = Convert.ToBoolean(dataItem.GetDataKeyValue("IsPackageExistsInRotation"));
                    if (!IsPackageExistsInRotation)
                    {
                        btnViewDetail.Visible = false;
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
        /// Handel selected checkboxes 
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void chkSelectDocument_CheckedChanged(object sender, EventArgs e)
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
                if (CurrentViewContext.LstApplicantRotationToExport.IsNull())
                {
                    CurrentViewContext.LstApplicantRotationToExport = new List<RotationMemberSearchDetailContract>();
                }

                List<RotationMemberSearchDetailContract> appRotationListToExport = CurrentViewContext.LstApplicantRotationToExport;
                Int32 rotationID = (Int32)dataItem.GetDataKeyValue("RotationID");
                Int32 orgUserId = (Int32)dataItem.GetDataKeyValue("OrganizationUserId");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectDocument")).Checked;

                RotationMemberSearchDetailContract appRotationData = appRotationListToExport.FirstOrDefault(cnd => cnd.RotationID == rotationID
                                                                                                                  && cnd.OrganizationUserID == orgUserId);
                if (appRotationListToExport.IsNotNull() && appRotationData.IsNullOrEmpty() && isChecked)
                {

                    RotationMemberSearchDetailContract rotationDataToExport = new RotationMemberSearchDetailContract();
                    rotationDataToExport.OrganizationUserID = orgUserId;
                    rotationDataToExport.RotationID = rotationID;
                    appRotationListToExport.Add(rotationDataToExport);
                }
                else if (appRotationListToExport.IsNotNull() && !appRotationData.IsNullOrEmpty() && !isChecked)
                {
                    appRotationListToExport.Remove(appRotationData);
                }

                CurrentViewContext.LstApplicantRotationToExport = appRotationListToExport;
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

        #region Radio Button
        protected void rbArchiveStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Methods
        #region private methods
        private void BindArchiveFilter()
        {
            if (!ddlTenant.SelectedValue.IsNullOrEmpty())
            {
                rbArchiveStatus.Visible = true;
                Presenter.GetArchiveStateList();
            }
            else
            {
                CurrentViewContext.lstArchiveState = new List<lkpArchiveState>();
                rbArchiveStatus.Visible = false;
            }
        }

        private void ResetControls()
        {
            BindControls();
            CurrentViewContext.SelectedAgencyID = AppConsts.NONE;
            txtComplioId.Text = String.Empty;
            txtRotationName.Text = String.Empty;
            txtAppLastName.Text = String.Empty;
            txtAppFirstName.Text = String.Empty;
            txtDepartment.Text = String.Empty;
            txtProgram.Text = String.Empty;
            txtCourse.Text = String.Empty;
            txtUnit.Text = String.Empty;
            txtRecommendedHrs.Text = String.Empty;
            //UAT-1769
            txtStudents.Text = String.Empty;
            txtShift.Text = String.Empty;
            dpStartDate.Clear();
            dpEndDate.Clear();
            tpStartTime.Clear();
            tpEndTime.Clear();
            ddlDays.ClearCheckedItems();
            ddlContacts.ClearCheckedItems();
            ddlUserGroup.SelectedIndex = AppConsts.NONE;
            //UAT-3749
            ddlUserType.ClearCheckedItems();
            //UAT 1355 Addition of Term field to the Create rotation, rotation details screens
            txtTerm.Text = String.Empty;
            //UAT-1467: Addition of "Type/Specialty" field on rotation creation and rotation details
            txtTypeSpecialty.Text = String.Empty;
            if (!ddlTenant.SelectedValue.IsNullOrEmpty())
            {
                rbArchiveStatus.Visible = true;
                Presenter.GetArchiveStateList();
            }
            else
                rbArchiveStatus.Visible = false;
            CurrentViewContext.LstApplicantRotationToExport = new List<RotationMemberSearchDetailContract>();
        }

        /// <summary>
        /// Method to Bind Tenant Dropdown and call to Bind UserGroup and Agency Dropdown
        /// </summary>
        private void BindControls()
        {
            //BindAgency();  [ddl]
            BindContacts();
            BindWeekDays();
            BindUserGroups();
            //UAT-3749
            BindUserTypes();
        }

        /// <summary>
        /// Method to Bind Agencies
        /// </summary>
        //private void BindAgency()      [ddl]
        //{
        //    Presenter.GetAllAgency();
        //    ddlAgency.DataSource = CurrentViewContext.lstAgency;
        //    ddlAgency.DataBind();
        //}

        /// <summary>
        /// Method to Bind User Groups 
        /// </summary>
        private void BindUserGroups()
        {
            Presenter.GetAllUserGroups();
            ddlUserGroup.DataSource = CurrentViewContext.lstUserGroup;
            ddlUserGroup.DataBind();
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

        //UAT-3749
        private void BindUserTypes()
        {
            Presenter.GetUserTypefrRMS();
            ddlUserType.DataSource = CurrentViewContext.dicUserTypes;
            ddlUserType.DataBind();
        }

        /// <summary>
        /// Method to Reset Grid 
        /// </summary>
        private void ResetGridFilters()
        {
            grdRotationMembers.MasterTableView.SortExpressions.Clear();
            grdRotationMembers.CurrentPageIndex = 0;
            grdRotationMembers.MasterTableView.IsItemInserted = false;
            grdRotationMembers.MasterTableView.CurrentPageIndex = 0;
            grdRotationMembers.Rebind();
        }

        private void ResetTenant()
        {
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                CurrentViewContext.SelectedTenantID = AppConsts.NONE;
            }
            else
            {
                CurrentViewContext.SelectedTenantID = CurrentViewContext.TenantID;
            }
        }

        private void BindTenant()
        {
            ddlTenant.DataSource = CurrentViewContext.lstTenant;
            ddlTenant.DataBind();
            Presenter.IsAdminLoggedIn();
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                ddlTenant.Enabled = true;
                CurrentViewContext.SelectedTenantID = 0;
            }
            else
            {
                CurrentViewContext.SelectedTenantID = CurrentViewContext.TenantID;
            }
        }

        /// <summary>
        /// Get search parameters
        /// </summary>
        private void GetSearchParameters()
        {
            _searchContract = new RotationMemberSearchDetailContract();
            _searchContract.AgencyID = CurrentViewContext.SelectedAgencyID;

            //UAT-2646
            if (!_searchContract.AgencyID.IsNullOrEmpty())
            {
                _searchContract.NodeId = ucAgencyHierarchy.NodeId;
                _searchContract.SelectedRootNodeId = ucAgencyHierarchy.SelectedRootNodeId;
            }

            _searchContract.SelectedUserGroupID = CurrentViewContext.SelectedUserGroupID;
            //UAT-3749
            _searchContract.SelectedUserTypeCode = String.Join(",", ddlUserType.CheckedItems.Select(x => x.Value));

            _searchContract.TenantID = CurrentViewContext.SelectedTenantID;
            if (!txtComplioId.Text.Trim().IsNullOrEmpty())
                _searchContract.ComplioID = txtComplioId.Text.Trim();
            if (!txtRotationName.Text.Trim().IsNullOrEmpty())
                _searchContract.RotationName = txtRotationName.Text.Trim();
            if (!txtAppFirstName.Text.Trim().IsNullOrEmpty())
                _searchContract.FirstName = txtAppFirstName.Text.Trim();
            if (!txtAppLastName.Text.Trim().IsNullOrEmpty())
                _searchContract.LastName = txtAppLastName.Text.Trim();
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
            //UAT-1769
            if (!txtStudents.Text.Trim().IsNullOrEmpty())
                _searchContract.Students = float.Parse(txtStudents.Text.Trim());
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
                _searchContract.StartTime = tpStartTime.SelectedTime;
            if (!tpEndTime.SelectedTime.IsNullOrEmpty())
                _searchContract.EndTime = tpEndTime.SelectedTime;
            //UAT-1467: Addition of "Type/Specialty" field on rotation creation and rotation details
            if (!txtTypeSpecialty.Text.Trim().IsNullOrEmpty())
                _searchContract.TypeSpecialty = txtTypeSpecialty.Text.Trim();

            _searchContract.ContactIdList = String.Join(",", ddlContacts.CheckedItems.Select(x => x.Value));
        }

        /// <summary>
        /// Set search parameters
        /// </summary>
        private void SetSearchParameters()
        {
            CurrentViewContext.SelectedAgencyID = _searchContract.AgencyID.HasValue ? _searchContract.AgencyID.Value : AppConsts.NONE;
            CurrentViewContext.SelectedTenantID = _searchContract.TenantID;
            CurrentViewContext.SelectedUserGroupID = _searchContract.SelectedUserGroupID;
            //UAT-3749
            String[] userTypeId = _searchContract.SelectedUserTypeCode.Split(',');
            foreach (RadComboBoxItem item in ddlUserType.Items)
            {
                item.Checked = userTypeId.Contains(item.Value);
            }

            txtComplioId.Text = _searchContract.ComplioID.IsNullOrEmpty() ? String.Empty : _searchContract.ComplioID;
            txtRotationName.Text = _searchContract.RotationName.IsNullOrEmpty() ? String.Empty : _searchContract.RotationName;
            txtAppFirstName.Text = _searchContract.FirstName.IsNullOrEmpty() ? String.Empty : _searchContract.FirstName;
            txtAppLastName.Text = _searchContract.LastName.IsNullOrEmpty() ? String.Empty : _searchContract.LastName;
            txtDepartment.Text = _searchContract.Department.IsNullOrEmpty() ? String.Empty : _searchContract.Department;
            txtProgram.Text = _searchContract.Program.IsNullOrEmpty() ? String.Empty : _searchContract.Program;
            txtCourse.Text = _searchContract.Course.IsNullOrEmpty() ? String.Empty : _searchContract.Course;
            txtUnit.Text = _searchContract.UnitFloorLoc.IsNullOrEmpty() ? String.Empty : _searchContract.UnitFloorLoc;
            txtRecommendedHrs.Text = _searchContract.RecommendedHours.IsNullOrEmpty() ? String.Empty : _searchContract.RecommendedHours.ToString();
            //UAT-1769
            txtStudents.Text = _searchContract.Students.IsNullOrEmpty() ? String.Empty : _searchContract.RecommendedHours.ToString();
            txtShift.Text = _searchContract.Shift.IsNullOrEmpty() ? String.Empty : _searchContract.Shift;
            txtTerm.Text = _searchContract.Term.IsNullOrEmpty() ? String.Empty : _searchContract.Term;
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

            String[] contactIds = _searchContract.ContactIdList.Split(',');
            foreach (RadComboBoxItem item in ddlContacts.Items)
            {
                item.Checked = contactIds.Contains(item.Value);
            }

            //UAT-2646
            if (!CurrentViewContext.SelectedAgencyID.IsNullOrEmpty())
            {
                ucAgencyHierarchy.NodeId = _searchContract.NodeId;
                ucAgencyHierarchy.SelectedRootNodeId = _searchContract.SelectedRootNodeId;
                ucAgencyHierarchy.TenantId = CurrentViewContext.SelectedTenantID;
            }
            //UAT - 3749 : Addition of Instructors to Rotation Member Search with user type filter and column
            if (!_searchContract.SelectedUserTypeCode.IsNullOrEmpty())
            {
                String[] userTypeCode = _searchContract.SelectedUserTypeCode.Split(',');
                foreach (RadComboBoxItem item in ddlUserType.Items)
                {
                    item.Checked = userTypeCode.Contains(item.Value);
                }
            }
        }


        /// <summary>
        /// Method to export the document(s) as zip.
        /// </summary>
        private void ExportDocuments()
        {
            //List<ApplicantDocumentContract> appDocList = new List<ApplicantDocumentContract>();
            String ConsolidatedWithoutSign = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_CONSOLIDATED_WITHOUGHT_SIGN.GetStringValue();
            Presenter.GetApplicantDocumentToExport();
            Int32 fileCount = AppConsts.NONE;
            //appDocList = CurrentViewContext.DocumentListToExport.Select(cond => cond.Value).DistinctBy(x => x.ApplicantDocumentID).ToList();
            //if (appDocList.IsNotNull() && appDocList.Count > 0)
            if (CurrentViewContext.LstApplicantDocumentToExport.IsNotNull() && CurrentViewContext.LstApplicantDocumentToExport.Count > 0)
            {
                String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                String folderName = String.Empty;
                if (tempFilePath.IsNullOrEmpty())
                {
                    base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                    return;
                }
                if (!tempFilePath.EndsWith(@"\"))
                {
                    tempFilePath += @"\";
                }
                folderName = "Tenant_" + CurrentViewContext.SelectedTenantID.ToString() + "_Applicant_Requirement_Doc_Zip_" + (DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss")) + @"\";
                tempFilePath += folderName;
                if (!Directory.Exists(tempFilePath))
                    Directory.CreateDirectory(tempFilePath);
                DirectoryInfo dirInfo = new DirectoryInfo(tempFilePath);
                try
                {
                    foreach (ApplicantDocumentContract applicantDocumentToExport in CurrentViewContext.LstApplicantDocumentToExport.Where(con => con.DataEntryDocumentStatusCode != ConsolidatedWithoutSign).ToList())
                    {
                        String fileExtension = Path.GetExtension(applicantDocumentToExport.DocumentPath);
                        //String fileName = Guid.NewGuid().ToString() + "_" + applicantDocumentToExport.FileName;
                        String fileName = GetFileName(applicantDocumentToExport.FileName);
                        String applicantName = String.Empty;
                        applicantName = applicantDocumentToExport.ApplicantName.Trim().Replace("  ", "_");
                        String applicantID = Convert.ToString(applicantDocumentToExport.ApplicantId);
                        String finalFileName = fileName + (applicantName.IsNullOrEmpty() ? "_" : "_" + applicantName + "_") + applicantID + fileExtension;

                        String newTempFilePath = Path.Combine(tempFilePath, finalFileName);
                        byte[] fileBytes = null;
                        fileBytes = CommonFileManager.RetrieveDocument(applicantDocumentToExport.DocumentPath, FileType.ApplicantFileLocation.GetStringValue());

                        if (fileBytes.IsNotNull())
                        {
                            try
                            {
                                File.WriteAllBytes(newTempFilePath, fileBytes);
                            }
                            catch (Exception ex)
                            {
                                base.LogError("Error found in bytes write for DocumentID: " + applicantDocumentToExport.ApplicantDocumentId.ToString(), ex);
                            }
                        }
                        //tempbytes = fileBytes;
                    }
                    fileCount = Directory.GetFiles(tempFilePath).Count();
                    if (fileCount > AppConsts.NONE)
                    {
                        ifrExportDocument.Src = "~/ComplianceOperations/UserControl/DoccumentDownload.aspx?zipfolderName=" + folderName + "&IsRotationAppZipDoc=" + "True";
                    }
                    else
                    {
                        base.ShowInfoMessage("No document(s) found to export.");
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
            else
            {
                base.ShowInfoMessage("No document(s) found to export.");
            }
        }

        private String GetFileName(String fileNameWithExt)
        {
            String[] splitFileName = fileNameWithExt.Split('.');
            String tempFileName = String.Join(".", splitFileName.Take(splitFileName.Length - 1));
            tempFileName = tempFileName.Replace(@"\", @"-");
            return tempFileName;
        }

        /// <summary>
        /// To set controls values in session
        /// </summary>
        private void SetSessionValues()
        {
            RotationMemberSearchDetailContract searchDataContract = new RotationMemberSearchDetailContract();
            CurrentViewContext.SearchContract.ArchieveStatusId = rbArchiveStatus.SelectedValue; //UAT-2545
            searchDataContract = CurrentViewContext.SearchContract;
            searchDataContract.GridCustomPagingArguments = CurrentViewContext.GridCustomPaging;
            //Session for maintaining control values
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ROTATION_MEMBER_SEARCH_SESSION_KEY, searchDataContract);
        }

        /// <summary>
        /// To get session values for controls
        /// </summary>
        private void GetSessionValues()
        {
            RotationMemberSearchDetailContract searchDataContract = new RotationMemberSearchDetailContract();
            if (Session[AppConsts.ROTATION_MEMBER_SEARCH_SESSION_KEY].IsNotNull())
            {
                searchDataContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.ROTATION_MEMBER_SEARCH_SESSION_KEY) as RotationMemberSearchDetailContract;
                CurrentViewContext.SearchContract = searchDataContract;
                CurrentViewContext.GridCustomPaging = searchDataContract.GridCustomPagingArguments;
                ViewState["ReBindGrid"] = null;
                rbArchiveStatus.SelectedValue = searchDataContract.ArchieveStatusId;
                grdRotationMembers.Rebind();
                //Reset session
                Session[AppConsts.ROTATION_MEMBER_SEARCH_SESSION_KEY] = null;
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
                    if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                    {
                        Presenter.GetArchiveStateList();
                    }
                }
            }
            //if user navigate to other feature from detail screen and return to manage rotation again.
            else
                Session[AppConsts.ROTATION_MEMBER_SEARCH_SESSION_KEY] = null;
        }
        #endregion

        #endregion
    }
}