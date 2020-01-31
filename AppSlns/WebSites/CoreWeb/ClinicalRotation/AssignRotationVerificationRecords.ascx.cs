using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using System.Web.UI;
using System.Collections;
using INTERSOFT.WEB.UI.WebControls; //UAT-3245

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class AssignRotationVerificationRecords : BaseUserControl, IAssignRotationVerificationRecords
    {
        #region Variables

        private AssignRotationVerificationRecordsPresenter _presenter = new AssignRotationVerificationRecordsPresenter();
        private String _viewType;
        private Int32 _verSelectedUserId;
        private String _verSelectedUserName;
        private Int32 _tenantId = 0;
        private RequirementVerificationFilterDataContract _verificationviewContract = null;
        #endregion;

        #region Properties

        public AssignRotationVerificationRecordsPresenter Presenter
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

        public IAssignRotationVerificationRecords CurrentViewContext
        {
            get
            {
                return this;
            }
        }
       
        List<Int32> IAssignRotationVerificationRecords.SelectedTenantIDs
        {
            get
            {
                if (ddlTenantName.CheckedItems.Count == AppConsts.NONE)
                    return new List<Int32>();
                return ddlTenantName.CheckedItems.Select(sel => Convert.ToInt32(sel.Value)).ToList();

            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    foreach (Int32 item in value)
                    {
                        ddlTenantName.FindItemByValue(item.ToString()).Checked = true;
                    }
                }
            }
        }

        // UAT-3245
        String IAssignRotationVerificationRecords.DeptProgramMappingID
        {
            get
            {
                if (!hdnDepartmntPrgrmMppng.Value.IsNullOrEmpty())
                {
                    return hdnDepartmntPrgrmMppng.Value;
                }
                return String.Empty;
            }
        }

        Int32 IAssignRotationVerificationRecords.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        String IAssignRotationVerificationRecords.ApplicantFirstName
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

        String IAssignRotationVerificationRecords.ApplicantLastName
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

        //UAT-3245
        Int32 IAssignRotationVerificationRecords.SelectedAgencyID
        {
            get
            {
                //if (String.IsNullOrEmpty(cmbAgency.SelectedValue))
                //    return 0;
                //return Convert.ToInt32(cmbAgency.SelectedValue);
                if (ucAgencyHierarchy.AgencyId.IsNullOrEmpty())
                    return 0;
                return ucAgencyHierarchy.AgencyId;
            }
            set
            {
                //if (value > 0)
                //{
                //    cmbAgency.SelectedValue = value.ToString();
                //}
                //else
                //{
                //    cmbAgency.SelectedIndex = value;
                //}
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

        DateTime? IAssignRotationVerificationRecords.RotationStartDate
        {
            get
            {
                return dpRotationStartDate.SelectedDate;
            }
            set
            {
                dpRotationStartDate.SelectedDate = value;
            }
        }

        DateTime? IAssignRotationVerificationRecords.RotationEndDate
        {
            get
            {
                return dpRotationEndDate.SelectedDate;
            }
            set
            {
                dpRotationEndDate.SelectedDate = value;
            }
        }

        DateTime? IAssignRotationVerificationRecords.SubmissionDate
        {
            get
            {
                return dpSubmissionDate.SelectedDate;
            }
            set
            {
                dpSubmissionDate.SelectedDate = value;
            }
        }

        String IAssignRotationVerificationRecords.PackageName
        {
            get
            {
                return txtPackageName.Text;
            }
            set
            {
                txtPackageName.Text = value;
            }
        }

        String IAssignRotationVerificationRecords.AssignedUserName
        {
            get
            {
                return txtAssignedUserName.Text;
            }
            set
            {
                txtAssignedUserName.Text = value;
            }
        }

        Boolean IAssignRotationVerificationRecords.IsCurrent
        {
            get
            {
                return chkIsCurrent.Checked;
            }
            set
            {
                chkIsCurrent.Checked = value;
            }
        }

        String IAssignRotationVerificationRecords.ComplioID
        {
            get
            {
                return txtComplioID.Text;
            }
            set
            {
                txtComplioID.Text = value;
            }
        }

        List<TenantDetailContract> IAssignRotationVerificationRecords.lstTenant
        {
            get;
            set;
        }

        List<Agency> IAssignRotationVerificationRecords.lstAgency
        {
            get;
            set;
        }

        String IAssignRotationVerificationRecords.ErrorMessage
        {
            get;
            set;
        }

        String IAssignRotationVerificationRecords.SuccessMessage
        {
            get;
            set;
        }

        List<RequirementVerificationQueueContract> IAssignRotationVerificationRecords.ApplicantSearchData
        {
            get;
            set;
        }

        Boolean IAssignRotationVerificationRecords.IsAdminLoggedIn
        {
            get
            {
                if (ViewState["IsAdminLoggedIn"] != null)
                    return Convert.ToBoolean(ViewState["IsAdminLoggedIn"]);
                else
                    return false;
            }
            set
            {
                ViewState["IsAdminLoggedIn"] = value;
            }
        }

        /// <summary>
        /// Sets or gets the Tenant Id for the logged-in user.
        /// </summary>
        Int32 IAssignRotationVerificationRecords.TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        String IAssignRotationVerificationRecords.RequirementPackageTypes
        {
            get
            {
                String selectedPackageType = String.Empty;
                List<Int32> lstSelectedRequirementPackageTypeID = new List<Int32>();
                foreach (RadComboBoxItem item in cmbRequirementPackageType.CheckedItems)
                {
                    lstSelectedRequirementPackageTypeID.Add(Convert.ToInt32(item.Value));
                }
                selectedPackageType = String.Join(",", lstSelectedRequirementPackageTypeID);

                return selectedPackageType;

            }
            set
            {
                List<String> lstSelectedRequirementPackageType = new List<String>();
                if (!value.IsNullOrEmpty())
                {
                    lstSelectedRequirementPackageType = value.Split(',').ToList();
                }

                cmbRequirementPackageType.Items.ForEach(itm =>
                {
                    if (lstSelectedRequirementPackageType.Contains(itm.Value))
                        itm.Checked = true;
                    else
                        itm.Checked = false;
                });

            }
        }

        List<RequirementPackageTypeContract> IAssignRotationVerificationRecords.lstRequirementPackageType
        {
            get;
            set;
        }

        #region Assignment Properties

        /// <summary>
        /// Sets or gets the Selected User Id.
        /// </summary>
        Int32 IAssignRotationVerificationRecords.VerSelectedUserId
        {
            get
            {
                return _verSelectedUserId;
            }
            set
            {
                _verSelectedUserId = value;
            }
        }

        String IAssignRotationVerificationRecords.VerSelectedUserName
        {
            get
            {
                return _verSelectedUserName;
            }
            set
            {
                _verSelectedUserName = value;
            }
        }

        public List<Int32> SelectedVerificationItems
        {
            get
            {
                if (!ViewState["SelectedVerificationItems"].IsNull())
                {
                    return ViewState["SelectedVerificationItems"] as List<Int32>;
                }

                return new List<Int32>();
            }
            set
            {
                ViewState["SelectedVerificationItems"] = value;
            }
        }


        /// <summary>
        /// Populates the dropdown with the list of active users in the organisation.
        /// </summary>
        public List<Entity.OrganizationUser> lstOrganizationUser
        {
            set
            {
                ddlVerSelectedUser.DataSource = value;
                ddlVerSelectedUser.DataBind();
            }
        }

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
                return grdAssignRotationVerificationQueue.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                //if (grdAssignRotationVerificationQueue.MasterTableView.CurrentPageIndex > 0)
                //{
                grdAssignRotationVerificationQueue.MasterTableView.CurrentPageIndex = value - 1;
                //}
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
                return grdAssignRotationVerificationQueue.PageSize;
            }
            set
            {
                grdAssignRotationVerificationQueue.PageSize = value;
                grdAssignRotationVerificationQueue.MasterTableView.PageSize = value;
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
                grdAssignRotationVerificationQueue.VirtualItemCount = value;
                grdAssignRotationVerificationQueue.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// To get object of shared class of custom paging
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
       

        #endregion
        /// <summary>
        /// View Contract  UAT-3388
        /// </summary>
        public RequirementVerificationFilterDataContract VerificationViewContract
        {
            get
            {
                if (_verificationviewContract.IsNull())
                {
                    _verificationviewContract = new RequirementVerificationFilterDataContract();
                }
                return _verificationviewContract;
            }
        }
        #endregion

        #region Page Events

        /// <summary>
        /// OnInit event to set page titles
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.SetPageTitle("Requirement Verification Assignment Queue");
                base.Title = "Requirement Verification Assignment Queue";
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
                if (!IsPostBack)
                {
                    BindTenant();
                    Presenter.IsAdminLoggedIn();
                    BindRequirementPackageTypes();
                    CaptureQuerystringParameters();
                    BindAgency();
                    GetSessionValues();
                    fsucCmdBarButton.SaveButton.ValidationGroup = "grpFormSubmit";


                    if (CurrentViewContext.IsAdminLoggedIn)
                        grdAssignRotationVerificationQueue.Columns.FindByUniqueName("ReqReviewByDesc").Visible = true;
                    else
                        grdAssignRotationVerificationQueue.Columns.FindByUniqueName("ReqReviewByDesc").Visible = false;
                }
                //UAT-3245
                if (!CurrentViewContext.SelectedTenantIDs.IsNullOrEmpty())
                {
                    ucAgencyHierarchy.lstTenantId = CurrentViewContext.SelectedTenantIDs.IsNullOrEmpty() ? new List<Int32>() : CurrentViewContext.SelectedTenantIDs;
                    if (CurrentViewContext.SelectedTenantIDs.Count == AppConsts.ONE)
                    {
                        hdnTenantID.Value = CurrentViewContext.SelectedTenantIDs.FirstOrDefault().ToString();
                    }
                }
                lblinstituteHierarchy.Text = hdnInstHierarchyLabel.Value.HtmlEncode();
                ManageHierarchyLinks();
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
        protected void grdAssignRotationVerificationQueue_Init(object sender, EventArgs e)
        {
            GridFilterMenu menu = grdAssignRotationVerificationQueue.FilterMenu;

            if (grdAssignRotationVerificationQueue.clearFilterMethod == null)
                grdAssignRotationVerificationQueue.clearFilterMethod = new WclGrid.ClearFilters(ClearViewStatesForFilter);

            int i = 0;
            while (i < menu.Items.Count)
            {
                if (menu.Items[i].Text == GridKnownFunction.Between.ToString() || menu.Items[i].Text == GridKnownFunction.NotBetween.ToString() ||
                    menu.Items[i].Text == GridKnownFunction.NotIsEmpty.ToString() || menu.Items[i].Text == GridKnownFunction.NotIsNull.ToString())
                {
                    menu.Items.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
        protected void grdAssignRotationVerificationQueue_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                GridCustomPaging.DefaultSortExpression = "ApplicantFirstName";
                GridCustomPaging.FilterColumns = VerificationViewContract.FilterColumns;
                GridCustomPaging.FilterOperators = VerificationViewContract.FilterOperators;
                GridCustomPaging.FilterValues = VerificationViewContract.FilterValues;
                GridCustomPaging.FilterTypes = VerificationViewContract.FilterTypes;
                Session[AppConsts.VERIFICATION_QUEUE_SESSION_KEY] = GridCustomPaging;
                Presenter.PerformSearch();
                if (CurrentViewContext.ApplicantSearchData.IsNullOrEmpty())
                {
                    CurrentViewContext.ApplicantSearchData = new List<RequirementVerificationQueueContract>();
                }
                grdAssignRotationVerificationQueue.DataSource = CurrentViewContext.ApplicantSearchData;

                if (CurrentViewContext.ApplicantSearchData.IsNotNull() && CurrentViewContext.ApplicantSearchData.Count > 0)
                {
                    pnlVerShowUsers.Visible = true;
                    Presenter.GetUserListForSelectedTenant();
                }
                else
                {
                    pnlVerShowUsers.Visible = false;
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

        protected void grdAssignRotationVerificationQueue_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                #region For Filter command
                SetVerGridFilters();
                if (e.CommandName == RadGrid.FilterCommandName)
                {
                    /*UAT-2904(set FilterFunction value of column filters to NoFilter,If no action is performed on them)*/
                    foreach (GridColumn item in grdAssignRotationVerificationQueue.MasterTableView.Columns)
                    {
                        string filterFunction = item.CurrentFilterFunction.ToString();
                        string filterValue = item.CurrentFilterValue;
                        if (filterValue.IsNullOrEmpty())
                        {
                            item.CurrentFilterFunction = Telerik.Web.UI.GridKnownFunction.NoFilter;
                        }
                    }
                    /*UAT-2904 ends here*/
                    Pair filter = (Pair)e.CommandArgument;
                    Int32 filterIndex = CurrentViewContext.VerificationViewContract.FilterColumns.IndexOf(filter.Second.ToString());
                    if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString())
                    {
                        String filteringType = grdAssignRotationVerificationQueue.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName;
                        String filterValue = grdAssignRotationVerificationQueue.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;

                        if (filterIndex != -1)
                        {
                            CurrentViewContext.VerificationViewContract.FilterTypes[filterIndex] = filteringType;
                            CurrentViewContext.VerificationViewContract.FilterOperators[filterIndex] = filter.First.ToString();
                            CurrentViewContext.VerificationViewContract.FilterValues[filterIndex] = filterValue;
                        }
                        else
                        {
                            CurrentViewContext.VerificationViewContract.FilterTypes.Add(filteringType);
                            CurrentViewContext.VerificationViewContract.FilterColumns.Add(filter.Second.ToString());
                            CurrentViewContext.VerificationViewContract.FilterOperators.Add(filter.First.ToString());
                            CurrentViewContext.VerificationViewContract.FilterValues.Add(filterValue);
                        }
                    }
                    else if (filterIndex != -1)
                    {
                        CurrentViewContext.VerificationViewContract.FilterOperators.RemoveAt(filterIndex);
                        CurrentViewContext.VerificationViewContract.FilterValues.RemoveAt(filterIndex);
                        CurrentViewContext.VerificationViewContract.FilterColumns.RemoveAt(filterIndex);
                        CurrentViewContext.VerificationViewContract.FilterTypes.RemoveAt(filterIndex);
                    }

                    ViewState["FilterColumns"] = CurrentViewContext.VerificationViewContract.FilterColumns;
                    ViewState["FilterOperators"] = CurrentViewContext.VerificationViewContract.FilterOperators;
                    ViewState["FilterValues"] = CurrentViewContext.VerificationViewContract.FilterValues;
                    ViewState["FilterTypes"] = CurrentViewContext.VerificationViewContract.FilterTypes;
                }

                #endregion
                if (e.CommandName == "ViewDetail")
                {
                    SetSessionValues();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String rotationID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ClinicalRotationID"].ToString();
                    String ReqPkgSubscriptionId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequirementPackageSubscriptionID"].ToString();
                    String RequirementItemId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequirementItemId"].ToString();
                    String ApplicantRequirementItemId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantRequirementItemId"].ToString();
                    String organizationUserID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserID"].ToString();
                    String TenantID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"].ToString();
                    String RequirementPackageTypeId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequirementPackageTypeID"].ToString();

                    Int32 _selectedReqCategoryId = Convert.ToInt32((e.Item.FindControl("hdfReqCatId") as HiddenField).Value);
                    Int32 _selectedReqPackageSubscriptionId = Convert.ToInt32((e.Item.FindControl("hdfReqPackSubscriptionId") as HiddenField).Value);

                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { ProfileSharingQryString.SelectedTenantId, TenantID },
                                                                    { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                                                                    { ProfileSharingQryString.ReqPkgSubscriptionId, ReqPkgSubscriptionId },
                                                                    { ProfileSharingQryString.RotationId, rotationID }, 
                                                                    { ProfileSharingQryString.ApplicantId , organizationUserID },
                                                                    { ProfileSharingQryString.ControlUseType,AppConsts.ASSIGN_ROTATION_VERIFICATION_QUEUE_TYPE_CODE},
                                                                    { "PackageTypeId" , RequirementPackageTypeId},
                                                                    {"RequirementItemId", RequirementItemId },
                                                                    {"ApplicantRequirementItemId", ApplicantRequirementItemId },
                                                                    {"SelectedReqComplianceCategoryId",Convert.ToString( _selectedReqCategoryId)},
                                                                    {"SelectedReqPackageSubscriptionId",Convert.ToString( _selectedReqPackageSubscriptionId)}
                        //{ "searchDataContract",}
                                                                    
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

        protected void grdAssignRotationVerificationQueue_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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

        #endregion

        #region Control Events

        protected void fsucCmdBarButton_SaveClick(object sender, EventArgs e)
        {
            try
            {
                //To reset grid filters 
                ResetAssignRotationVerificationQueueGridFilters();
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

        protected void fsucCmdBarButton_SubmitClick(object sender, EventArgs e)
        {
            ResetControls();
            //ucAgencyHierarchyMultipleToSearchRotation.Reset(); 
            //UAT-3245
            ucAgencyHierarchy.Reset();
            ManageHierarchyLinks();
        }

        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
        {
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME));
        }

        /// <summary>
        /// Agency dropdown DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void cmbAgency_DataBound(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        cmbAgency.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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
        //} UAT-3245

        protected void ddlVerSelectedUser_DataBound(object sender, EventArgs e)
        {
            ddlVerSelectedUser.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void btnVerAssignUser_Click(object sender, EventArgs e)
        {
            if (ddlVerSelectedUser.SelectedValue != "")
            {
                CurrentViewContext.VerSelectedUserId = Convert.ToInt32(ddlVerSelectedUser.SelectedValue);
                CurrentViewContext.VerSelectedUserName = ddlVerSelectedUser.SelectedItem.Text;
                #region UAT-3388
                GridCustomPaging.FilterColumns = VerificationViewContract.FilterColumns;
                GridCustomPaging.FilterOperators = VerificationViewContract.FilterOperators;
                GridCustomPaging.FilterValues = VerificationViewContract.FilterValues;
                GridCustomPaging.FilterTypes = VerificationViewContract.FilterTypes;
                #endregion

                if (Presenter.AssignItemsToUser())
                {
                    SelectedVerificationItems = new List<Int32>();
                    grdAssignRotationVerificationQueue.Rebind();
                    ddlVerSelectedUser.DataBind();
                    ddlVerSelectedUser.SelectedIndex = 0;
                    ShowStatusMessage("sucs", AppConsts.MSG_ITEM_ASSIGNED_SUCCESS, true);
                }
                else
                {
                    if (String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        ShowStatusMessage("error", AppConsts.MSG_SELECT_ITEM, true);
                    }
                    else
                    {
                        CurrentViewContext.ErrorMessage = String.Format(CurrentViewContext.ErrorMessage, ddlVerSelectedUser.SelectedItem.Text);
                        ShowStatusMessage("error", CurrentViewContext.ErrorMessage, true);
                    }
                }
            }
            else
            {
                ShowStatusMessage("error", AppConsts.MSG_SELECT_USER, true);
            }
        }

        /// <summary>
        /// Handles the assignment of items to the users.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void chkSelectVerItem_CheckedChanged(object sender, EventArgs e)
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
                List<Int32> items = SelectedVerificationItems;
                Int32 FlatRequirementVerificationID = (Int32)dataItem.GetDataKeyValue("FlatVerificationDataID");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectItem")).Checked;


                if (items.Any(x => x == FlatRequirementVerificationID) && !isChecked)
                {
                    items.Remove(FlatRequirementVerificationID);
                }
                else
                {
                    items.Add(FlatRequirementVerificationID);
                }

                SelectedVerificationItems = items;
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

        protected void btnDummy_Click(object sender, EventArgs e)
        {
            try
            {
                BindAgency();
                //UAT-3245
                //ManageHierarchyLinks();
                ucAgencyHierarchy.Reset();
                ResetInstitutionHierarchy();
                if (CurrentViewContext.SelectedTenantIDs.Count() > AppConsts.ONE)
                {
                    hdnDepartmntPrgrmMppng.Value = String.Empty;
                    hdnInstHierarchyLabel.Value = String.Empty;
                }
                if (CurrentViewContext.SelectedTenantIDs.IsNullOrEmpty())
                {
                    ucAgencyHierarchy.lstTenantId = new List<Int32>();
                    hdnTenantID.Value = String.Empty;
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

        #region Methods
        private void SetVerGridFilters()
        {
            if (!ViewState["SortExpression"].IsNull())
            {
                CurrentViewContext.GridCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                CurrentViewContext.GridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
            }

            CurrentViewContext.VerificationViewContract.FilterColumns = ViewState["FilterColumns"] == null ? new List<String>() : (List<String>)(ViewState["FilterColumns"]);
            CurrentViewContext.VerificationViewContract.FilterOperators = ViewState["FilterOperators"] == null ? new List<String>() : (List<String>)(ViewState["FilterOperators"]);
            CurrentViewContext.VerificationViewContract.FilterValues = ViewState["FilterValues"] == null ? new ArrayList() : (ArrayList)(ViewState["FilterValues"]);
            CurrentViewContext.VerificationViewContract.FilterTypes = ViewState["FilterTypes"] == null ? new List<String>() : (List<String>)(ViewState["FilterTypes"]);
        }
        private void BindTenant()
        {
            Presenter.GetTenants();
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();
        }
        public void ClearViewStatesForFilter()
        {
            ViewState["FilterColumns"] = null;
            ViewState["FilterOperators"] = null;
            ViewState["FilterValues"] = null;
            ViewState["FilterTypes"] = null;

            CurrentViewContext.GridCustomPaging.FilterColumns = null;
            CurrentViewContext.GridCustomPaging.FilterOperators = null;
            CurrentViewContext.GridCustomPaging.FilterValues = null;
            CurrentViewContext.GridCustomPaging.FilterTypes = null;
        }
        /// <summary>
        /// Method to bind Agencies
        /// </summary>
        private void BindAgency()
        {
            Presenter.GetAllAgency();
            //cmbAgency.DataSource = CurrentViewContext.lstAgency;
            //cmbAgency.DataBind(); UAT-3245
        }


        /// <summary>
        /// To reset search controls
        /// </summary>
        private void ResetControls()
        {
            CurrentViewContext.VirtualRecordCount = 0;
            CurrentViewContext.ApplicantFirstName = String.Empty;
            CurrentViewContext.ApplicantLastName = String.Empty;
            CurrentViewContext.RotationStartDate = null;
            CurrentViewContext.RotationEndDate = null;
            CurrentViewContext.SubmissionDate = null;
            CurrentViewContext.SelectedAgencyID = -1; //UAT-3245
            ddlTenantName.ClearCheckedItems();
            CurrentViewContext.SelectedTenantIDs = new List<Int32>();
            CurrentViewContext.PackageName = String.Empty;
            CurrentViewContext.AssignedUserName = String.Empty;
            CurrentViewContext.IsCurrent = false;
            CurrentViewContext.ComplioID = String.Empty;
            hdnPreviousTenantIds.Value = String.Empty;
            ResetAssignRotationVerificationQueueGridFilters();
            BindRequirementPackageTypes();
            //UAT-3245
            //lblinstituteHierarchy.Text = string.Empty;
            //hdnDepartmntPrgrmMppng.Value = string.Empty;
            //hdnInstHierarchyLabel.Value = string.Empty;
            //hdnInstitutionNodeID.Value = string.Empty;
            ResetInstitutionHierarchy();
            ucAgencyHierarchy.AgencyId = AppConsts.NONE;
            hdnTenantID.Value = string.Empty;
            ucAgencyHierarchy.lstTenantId = new List<Int32>();
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetAssignRotationVerificationQueueGridFilters()
        {
            grdAssignRotationVerificationQueue.MasterTableView.SortExpressions.Clear();
            grdAssignRotationVerificationQueue.CurrentPageIndex = 0;
            grdAssignRotationVerificationQueue.MasterTableView.CurrentPageIndex = 0;
            grdAssignRotationVerificationQueue.Rebind();
        }

        /// <summary>
        /// To set controls values in session
        /// </summary>
        private void SetSessionValues()
        {
            Session[AppConsts.ASSIGN_ROTATON_VERIFICATION_QUEUE_SESSION_KEY] = null;
            Session[AppConsts.REQ_VERIFICATION_USER_WORK_QUEUE_SESSION_KEY] = null;
            RequirementVerificationQueueContract searchDataContract = new RequirementVerificationQueueContract();
            searchDataContract.ApplicantFirstName = CurrentViewContext.ApplicantFirstName;
            searchDataContract.ApplicantLastName = CurrentViewContext.ApplicantLastName;
            searchDataContract.RotationStartDate = CurrentViewContext.RotationStartDate;
            searchDataContract.RotationEndDate = CurrentViewContext.RotationEndDate;
            searchDataContract.SubmissionDate = CurrentViewContext.SubmissionDate;
            searchDataContract.AgencyID = CurrentViewContext.SelectedAgencyID;  //UAT-3245
            searchDataContract.RequirementPackageName = CurrentViewContext.PackageName;
            searchDataContract.AssignedUserName = CurrentViewContext.AssignedUserName;
            searchDataContract.IsCurrentRotation = CurrentViewContext.IsCurrent;
            searchDataContract.RequirementItemVerificationCode = RequirementItemStatus.PENDING_REVIEW.GetStringValue();
            searchDataContract.SelectedTenantIDs = String.Join(",", CurrentViewContext.SelectedTenantIDs);
            searchDataContract.SelectedRequirementPackageTypes = CurrentViewContext.RequirementPackageTypes;
            searchDataContract.ComplioID = CurrentViewContext.ComplioID;
            searchDataContract.GridCustomPagingArguments = CurrentViewContext.GridCustomPaging;
            //UAT-3245
            if (CurrentViewContext.SelectedTenantIDs.Count() == AppConsts.ONE)
            {
                searchDataContract.DPMIds = hdnDepartmntPrgrmMppng.Value;
                searchDataContract.InstituteHierarchySelectedNode = hdnInstHierarchyLabel.Value;
            }
            searchDataContract.SelectedTenantIDs = String.Join(",", CurrentViewContext.SelectedTenantIDs);
            if (!CurrentViewContext.SelectedAgencyID.IsNullOrEmpty() && CurrentViewContext.SelectedAgencyID > AppConsts.NONE)
            {
                searchDataContract.NodeId = ucAgencyHierarchy.NodeId;
                searchDataContract.SelectedRootNodeId = ucAgencyHierarchy.SelectedRootNodeId;
            }

            searchDataContract.FilterColumns = ViewState["FilterColumns"].IsNullOrEmpty() ? null : (List<String>)ViewState["FilterColumns"];
            searchDataContract.FilterOperators = ViewState["FilterOperators"].IsNullOrEmpty() ? null : (List<String>)ViewState["FilterOperators"];
            searchDataContract.FilterValues = ViewState["FilterValues"].IsNullOrEmpty() ? null : (ArrayList)ViewState["FilterValues"];
            searchDataContract.FilterTypes = ViewState["FilterTypes"].IsNullOrEmpty() ? null : (List<String>)ViewState["FilterTypes"];
            //Session for maintaining control values
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ASSIGN_ROTATON_VERIFICATION_QUEUE_SESSION_KEY, searchDataContract);
        }

        /// <summary>
        /// To get session values for controls
        /// </summary>
        private void GetSessionValues()
        {
            RequirementVerificationQueueContract searchDataContract = new RequirementVerificationQueueContract();
            if (Session[AppConsts.ASSIGN_ROTATON_VERIFICATION_QUEUE_SESSION_KEY].IsNotNull())
            {
                searchDataContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.ASSIGN_ROTATON_VERIFICATION_QUEUE_SESSION_KEY) as RequirementVerificationQueueContract;
                CurrentViewContext.ApplicantFirstName = searchDataContract.ApplicantFirstName;
                CurrentViewContext.ApplicantLastName = searchDataContract.ApplicantLastName;
                CurrentViewContext.RotationStartDate = searchDataContract.RotationStartDate;
                CurrentViewContext.RotationEndDate = searchDataContract.RotationEndDate;
                CurrentViewContext.SubmissionDate = searchDataContract.SubmissionDate;
                CurrentViewContext.PackageName = searchDataContract.RequirementPackageName;
                CurrentViewContext.IsCurrent = searchDataContract.IsCurrentRotation;
                CurrentViewContext.AssignedUserName = searchDataContract.AssignedUserName;
                CurrentViewContext.RequirementPackageTypes = searchDataContract.SelectedRequirementPackageTypes;
                CurrentViewContext.ComplioID = searchDataContract.ComplioID;
                CurrentViewContext.SelectedTenantIDs = searchDataContract.SelectedTenantIDs.IsNullOrEmpty() ? null : searchDataContract.SelectedTenantIDs.Split(',').ConvertIntoIntList();
                hdnPreviousTenantIds.Value = searchDataContract.SelectedTenantIDs.IsNullOrEmpty() ? String.Empty : searchDataContract.SelectedTenantIDs;
                CurrentViewContext.GridCustomPaging = searchDataContract.GridCustomPagingArguments;
                BindAgency();
                CurrentViewContext.SelectedAgencyID = searchDataContract.AgencyID ?? 0; // UAT-3245
                //UAT-3245
                if (!searchDataContract.DPMIds.IsNullOrEmpty() && CurrentViewContext.SelectedTenantIDs.Count() == AppConsts.ONE)
                {
                    hdnDepartmntPrgrmMppng.Value = searchDataContract.DPMIds;
                    hdnInstHierarchyLabel.Value = searchDataContract.InstituteHierarchySelectedNode;
                }
                if (!searchDataContract.AgencyID.IsNullOrEmpty() && searchDataContract.AgencyID > AppConsts.NONE)
                {
                    ucAgencyHierarchy.NodeId = searchDataContract.NodeId;
                    ucAgencyHierarchy.SelectedRootNodeId = searchDataContract.SelectedRootNodeId;
                    ucAgencyHierarchy.lstTenantId = CurrentViewContext.SelectedTenantIDs;
                }
                ViewState["FilterColumns"] = CurrentViewContext.VerificationViewContract.FilterColumns = searchDataContract.FilterColumns;
                ViewState["FilterOperators"] = CurrentViewContext.VerificationViewContract.FilterOperators = searchDataContract.FilterOperators;
                ViewState["FilterValues"] = CurrentViewContext.VerificationViewContract.FilterValues = searchDataContract.FilterValues;
                ViewState["FilterTypes"] = CurrentViewContext.VerificationViewContract.FilterTypes = searchDataContract.FilterTypes;
                //Rebind grids
                grdAssignRotationVerificationQueue.Rebind();
                //Reset session
                Session[AppConsts.ASSIGN_ROTATON_VERIFICATION_QUEUE_SESSION_KEY] = null;
            }
        }
        public void SetFilterValues()
        {
            if (!CurrentViewContext.VerificationViewContract.FilterColumns.IsNullOrEmpty() && CurrentViewContext.VerificationViewContract.FilterColumns.Count > 0)
            {
                CurrentViewContext.VerificationViewContract.FilterColumns.ForEach(x =>
                    grdAssignRotationVerificationQueue.Columns.FindByUniqueName(x).CurrentFilterValue = CurrentViewContext.VerificationViewContract.FilterValues[CurrentViewContext.VerificationViewContract.FilterColumns.IndexOf(x)].ToString()
                    );
            }
        }
        private void ShowStatusMessage(String cssClass, String message, Boolean isVerificationMsg)
        {

            lblVerError.Text = message;
            lblVerError.CssClass = cssClass;
            pnlVerError.Update();

        }

        private void CaptureQuerystringParameters()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (Request.QueryString["args"].IsNull())
            {
                Session[AppConsts.ASSIGN_ROTATON_VERIFICATION_QUEUE_SESSION_KEY] = null;
            }
        }

        /// <summary>
        /// Method to bind Requirement Package Types
        /// </summary>
        private void BindRequirementPackageTypes()
        {
            Presenter.GetSharedRequirementPackageTypes();
            cmbRequirementPackageType.DataSource = CurrentViewContext.lstRequirementPackageType;
            cmbRequirementPackageType.DataBind();
            cmbRequirementPackageType.Items.ForEach(itm =>
            {
                itm.Checked = true;
            });
        }

        #region UAT-3245
        private void ManageHierarchyLinks()
        {
            if (CurrentViewContext.SelectedTenantIDs.Count > AppConsts.ONE)
            {
                dvInstHierarchy.Visible = false;
            }
            else if (CurrentViewContext.SelectedTenantIDs.Count == AppConsts.ONE || CurrentViewContext.SelectedTenantIDs.Count == AppConsts.NONE)
            {
                dvInstHierarchy.Visible = true;
            }
        }

        private void ResetInstitutionHierarchy()
        {
            lblinstituteHierarchy.Text = String.Empty;
            hdnDepartmntPrgrmMppng.Value = String.Empty;
            hdnInstHierarchyLabel.Value = String.Empty;
            hdnInstitutionNodeID.Value = String.Empty;
        }

        #endregion

        #endregion
    }
}