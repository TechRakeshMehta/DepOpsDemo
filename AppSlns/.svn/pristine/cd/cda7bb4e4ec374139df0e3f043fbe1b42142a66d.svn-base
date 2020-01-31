using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.ServiceUtil;
using INTSOF.Contracts;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Core;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class StudentBucketAssignment : BaseUserControl, IStudentBucketAssignmentView
    {
        #region Variables
        private StudentBucketAssignmentPresenter _presenter = new StudentBucketAssignmentPresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private CustomPagingArgsContract _gridCustomPaging = null;
        private SearchItemDataContract _gridSearchContract = null;
        private String _rotationCustomAttributes = null;
        #endregion

        #region Properties
        public StudentBucketAssignmentPresenter Presenter
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

        public IStudentBucketAssignmentView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IStudentBucketAssignmentView.SelectedTenantId
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
                    hdnSelectedTenantID.Value = CurrentViewContext.SelectedTenantId.ToString();
                }
                else
                {
                    ddlTenantName.SelectedIndex = value;
                }
            }
        }

        Int32? IStudentBucketAssignmentView.OrganizationUserID
        {
            get
            {
                if (String.IsNullOrEmpty(txtUserID.Text))
                    return 0;
                return Convert.ToInt32(txtUserID.Text);
            }
            set
            {
                if (value == null)
                    txtUserID.Text = String.Empty;
                else
                    txtUserID.Text = value.ToString();
            }
        }

        Int32 IStudentBucketAssignmentView.TenantId
        {
            get
            {
                if (tenantId == 0)
                {
                    // tenantId = Presenter.GetTenantId();
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

        Int32 IStudentBucketAssignmentView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        List<Entity.Tenant> IStudentBucketAssignmentView.lstTenant
        {
            get;
            set;
        }

        String IStudentBucketAssignmentView.ApplicantFirstName
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

        String IStudentBucketAssignmentView.ApplicantLastName
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

        String IStudentBucketAssignmentView.EmailAddress
        {
            get
            {
                return txtEmail.Text;
            }
            set
            {
                txtEmail.Text = value;
            }
        }

        String IStudentBucketAssignmentView.SSN
        {

            get;
            set;
        }

        DateTime? IStudentBucketAssignmentView.DateOfBirth
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

        List<StudentBucketAssignmentContract> IStudentBucketAssignmentView.GridSearchData
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        Dictionary<Int32, Boolean> IStudentBucketAssignmentView.AssignOrganizationUserIds
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

        String IStudentBucketAssignmentView.SSNPermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["SSNPermissionCode"]);
            }
            set
            {
                ViewState["SSNPermissionCode"] = value;
            }
        }

        Boolean IStudentBucketAssignmentView.IsDOBDisable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsDOBDisable"]);
            }
            set
            {
                ViewState["IsDOBDisable"] = value;
            }
        }

        String IStudentBucketAssignmentView.DPM_IDs
        {
            get;
            set;
        }

        String IStudentBucketAssignmentView.CustomFields
        {
            get;
            set;
        }

        List<Entity.ClientEntity.UserGroup> IStudentBucketAssignmentView.lstUserGroup
        {
            get;
            set;
        }

        Int32 IStudentBucketAssignmentView.FilterUserGroupId
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

        Int32 IStudentBucketAssignmentView.MatchUserGroupId
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
        Dictionary<Int32, String> IStudentBucketAssignmentView.SelectedOrgUsersToList
        {
            get
            {
                if (!ViewState["SelectedOrgUsersToList"].IsNull())
                {
                    return ViewState["SelectedOrgUsersToList"] as Dictionary<Int32, String>;
                }
                return new Dictionary<Int32, String>();
            }
            set
            {
                ViewState["SelectedOrgUsersToList"] = value;
            }
        }

        /// <summary>
        /// UAT-2056, 
        /// </summary>
        List<String> IStudentBucketAssignmentView.LstStudentBucketAssigmentPermissions
        {
            get
            {
                if (ViewState["LstStudentBucketAssigmentPermissions"].IsNotNull())
                {
                    return (List<String>)(ViewState["LstStudentBucketAssigmentPermissions"]);
                }
                return new List<String>();
            }
            set
            {
                ViewState["LstStudentBucketAssigmentPermissions"] = value;
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
                return grdApplicantSearchData.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdApplicantSearchData.MasterTableView.CurrentPageIndex > 0)
                {
                    grdApplicantSearchData.MasterTableView.CurrentPageIndex = value - 1;
                }
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
                //return grdApplicantSearchData.PageSize > 100 ? 100 : grdApplicantSearchData.PageSize;
                return grdApplicantSearchData.PageSize;
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
                grdApplicantSearchData.VirtualItemCount = value;
                grdApplicantSearchData.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPaging
        {
            get
            {
                if (_gridCustomPaging.IsNull())
                {
                    _gridCustomPaging = new CustomPagingArgsContract();
                }
                return _gridCustomPaging;
            }
        }

        #endregion


        #region [UAT-2054]
        Int32 IStudentBucketAssignmentView.AgencyID
        {
            get
            {
                //if (String.IsNullOrEmpty(ddlAgency.SelectedValue))  [ddl]
                //    return 0;
                //return Convert.ToInt32(ddlAgency.SelectedValue);
                //UAT-2600
                if (ucAgencyHierarchy.AgencyId.IsNullOrEmpty())
                    return 0;
                return ucAgencyHierarchy.AgencyId;
            }
        }


        String IStudentBucketAssignmentView.ComplioID
        {
            get
            {
                if (!txtComplioId.Text.Trim().IsNullOrEmpty())
                    return txtComplioId.Text.Trim();

                return string.Empty;
            }
        }


        String IStudentBucketAssignmentView.RotationName
        {
            get
            {
                if (!txtRotationName.Text.Trim().IsNullOrEmpty())
                    return txtRotationName.Text.Trim();

                return string.Empty;
            }
        }


        String IStudentBucketAssignmentView.Department
        {
            get
            {
                if (!txtDepartment.Text.Trim().IsNullOrEmpty())
                    return txtDepartment.Text.Trim();

                return string.Empty;
            }
        }

        String IStudentBucketAssignmentView.Program
        {
            get
            {
                if (!txtProgram.Text.Trim().IsNullOrEmpty())
                    return txtProgram.Text.Trim();

                return string.Empty;
            }
        }


        String IStudentBucketAssignmentView.Course
        {
            get
            {
                if (!txtCourse.Text.Trim().IsNullOrEmpty())
                    return txtCourse.Text.Trim();

                return string.Empty;
            }
        }

        String IStudentBucketAssignmentView.Term
        {
            get
            {
                if (!txtTerm.Text.Trim().IsNullOrEmpty())
                    return txtTerm.Text.Trim();

                return string.Empty;
            }
        }

        String IStudentBucketAssignmentView.UnitFloorLoc
        {
            get
            {
                if (!txtUnit.Text.Trim().IsNullOrEmpty())
                    return txtUnit.Text.Trim();

                return string.Empty;
            }
        }


        float? IStudentBucketAssignmentView.RecommendedHours
        {
            get
            {
                if (!txtRecommendedHrs.Text.Trim().IsNullOrEmpty())
                    return float.Parse(txtRecommendedHrs.Text.Trim());

                return null;
            }
        }

        float? IStudentBucketAssignmentView.Students
        {
            get
            {
                if (!txtStudents.Text.Trim().IsNullOrEmpty())
                    return float.Parse(txtStudents.Text.Trim());

                return null;
            }
        }

        String IStudentBucketAssignmentView.Shift
        {
            get
            {
                if (!txtShift.Text.Trim().IsNullOrEmpty())
                    return txtShift.Text.Trim();

                return string.Empty;
            }
        }

        DateTime? IStudentBucketAssignmentView.StartDate
        {
            get
            {
                if (!dpStartDate.SelectedDate.IsNullOrEmpty())
                    return dpStartDate.SelectedDate;

                return null;
            }
        }

        DateTime? IStudentBucketAssignmentView.EndDate
        {
            get
            {
                if (!dpEndDate.SelectedDate.IsNullOrEmpty())
                    return dpEndDate.SelectedDate;

                return null;
            }
        }


        TimeSpan? IStudentBucketAssignmentView.StartTime
        {
            get
            {
                if (!tpStartTime.SelectedTime.IsNullOrEmpty())
                    return tpStartTime.SelectedTime;

                return null;
            }
        }

        TimeSpan? IStudentBucketAssignmentView.EndTime
        {
            get
            {
                if (!tpEndTime.SelectedTime.IsNullOrEmpty())
                    return tpEndTime.SelectedTime;

                return null;
            }
        }


        String IStudentBucketAssignmentView.DaysIdList
        {
            get
            {
                return String.Join(",", ddlDays.CheckedItems.Select(x => x.Value));
            }
        }

        String IStudentBucketAssignmentView.ContactIdList
        {
            get
            {
                return String.Join(",", ddlContacts.CheckedItems.Select(x => x.Value));
            }
        }

        String IStudentBucketAssignmentView.TypeSpecialty
        {
            get
            {
                if (!txtTypeSpecialty.Text.Trim().IsNullOrEmpty())
                    return txtTypeSpecialty.Text.Trim();

                return string.Empty;
            }
        }

        String IStudentBucketAssignmentView.RotationCustomAttributesXML
        {
            get
            {
                _rotationCustomAttributes = null;

                if (caRotationCustomAttributesID.IsNotNull())
                    _rotationCustomAttributes = caRotationCustomAttributesID.GetCustomDataXML();

                if (!_rotationCustomAttributes.IsNullOrEmpty())
                    return _rotationCustomAttributes;

                return null;
            }
        }

        List<AgencyDetailContract> IStudentBucketAssignmentView.lstAgency
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

        List<ClientContactContract> IStudentBucketAssignmentView.ClientContactList
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

        List<WeekDayContract> IStudentBucketAssignmentView.WeekDayList
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

        List<CustomAttribteContract> IStudentBucketAssignmentView.RotationCustomAttributeList
        {
            get;
            set;
        }

        #endregion

        #region UAT-3010:- Granular Permission for Client Admin Users to Archive.

        public String ArchivePermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["ArchivePermissionCode"]);
            }
            set
            {
                ViewState["ArchivePermissionCode"] = value;
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
                base.Title = "Student Bulk Assignment";
                base.SetPageTitle("Student Bulk Assignment");
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
                CurrentViewContext.SSN = txtSSN.TextWithPrompt;
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to search applicants per the criteria entered above";
                (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel";
                (fsucCmdBarButton as CoreWeb.Shell.Views.CommandBar).SubmitButton.ToolTip = "Click to remove all values entered in the search criteria above";
                if (!this.IsPostBack)
                {
                    grdApplicantSearchData.Visible = false;
                    Session[AppConsts.STUDENT_BUCKET_ASSIGNEMNT_SESSION_KEY] = null;
                    //Set MinDate and MaxDate for DOB
                    dpkrDOB.MinDate = Convert.ToDateTime("01-01-1900");
                    dpkrDOB.MaxDate = DateTime.Now;

                    Presenter.OnViewInitialized();
                    BindControls();

                    ApplySSNMask();

                    //To check if cancel button is clicked on Edit Profile page
                    //and get session values for controls
                    Dictionary<String, String> args = new Dictionary<String, String>();

                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);
                        if (args.ContainsKey("CancelClicked") && args["CancelClicked"].IsNotNull())
                        {
                            GetSessionValues();
                        }
                    }
                    else
                        ShowHideControls(false);
                    //fsucCmdBarButton.ClearButton.Style.Add("display", "none");
                    fsucCmdBarButton.SaveButton.ValidationGroup = "grpFormSubmit";
                    //fsucCmdBarButton.ClearButton.ValidationGroup = "grpFormSubmit";
                }
                Presenter.OnViewLoaded();
                ucCustomAttributeLoaderSearch.TenantId = CurrentViewContext.SelectedTenantId;
                ucCustomAttributeLoaderSearch.ScreenType = "CommonScreen";
                HideShowControlsForGranularPermission();

                if (CurrentViewContext.SelectedTenantId > AppConsts.NONE)
                {
                    caRotationCustomAttributesID.IsSearchTypeControl = true;
                    caRotationCustomAttributesID.TenantId = CurrentViewContext.SelectedTenantId;
                    caRotationCustomAttributesID.TypeCode = CustomAttributeUseTypeContext.Hierarchy.GetStringValue();
                    caRotationCustomAttributesID.DataSourceModeType = DataSourceMode.Ids;
                    caRotationCustomAttributesID.Title = "Other Details";
                    caRotationCustomAttributesID.ControlDisplayMode = DisplayMode.Controls;
                    caRotationCustomAttributesID.CurrentLoggedInUserId = base.CurrentUserId;
                    caRotationCustomAttributesID.ValidationGroup = "grpFormSubmitSearchType";
                    caRotationCustomAttributesID.IsReadOnly = false;

                    Presenter.GetRotationCustomAttributeList(null);
                    if (!CurrentViewContext.RotationCustomAttributeList.IsNullOrEmpty())
                        caRotationCustomAttributesID.lstTypeCustomAttributes = CurrentViewContext.RotationCustomAttributeList;

                    //if (caRotationCustomAttributesID.IsNotNull() && _searchContract.IsNotNull())
                    //    caRotationCustomAttributesID.previousValues = _searchContract.CustomAttributes;

                }
                ucAgencyHierarchy.TenantId = CurrentViewContext.SelectedTenantId; //UAT-2600
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

        /// <summary>
        /// To set data source to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                    if (ViewState["IsBind"] == null)
                    {
                        CurrentViewContext.GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                        GridCustomPaging.PageSize = PageSize;
                        //UAT-1055: WB: As an admin, I should be able to search multiple nodes at a time.
                        CurrentViewContext.DPM_IDs = ucCustomAttributeLoaderSearch.DPM_ID;
                        CurrentViewContext.CustomFields = ucCustomAttributeLoaderSearch.GetCustomDataXML();
                        Presenter.PerformSearch();
                    }
                    grdApplicantSearchData.DataSource = CurrentViewContext.GridSearchData;
                    //ApplicantDataList applicantDataList = CurrentViewContext.ApplicantSearchData.FirstOrDefault();
                    //if (!applicantDataList.IsNullOrEmpty())
                    //{
                    //    hdnTotalUsersAssigned.Value = Convert.ToString(applicantDataList.TotalUsersAssigned);
                    //}
                    //else
                    //{
                    //    hdnTotalUsersAssigned.Value = "0";
                    //}


                    //To set controls values in session
                    SetSessionValues();
                    DisplayMessageSentStatus();
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
        /// Event handler. Called by grdVerificationItemData for item data bound events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdApplicantSearchData_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                    // and displayed the masked column on Export instead of actual column.
                    dataItem["_SSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["_SSN"].Text));

                    //UAT-806 Creation of granular permissions for Client Admin users
                    if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue())
                    {
                        dataItem["SSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["SSN"].Text));
                    }
                    else
                    {
                        ///Formatting SSN as ###-##-####
                        dataItem["SSN"].Text = Presenter.GetFormattedSSN(Convert.ToString(dataItem["SSN"].Text));
                    }
                    //if (Convert.ToString(dataItem["UserGroups"].Text).Length > 20)
                    //{
                    //    dataItem["UserGroups"].ToolTip = dataItem["UserGroups"].Text;
                    //    dataItem["UserGroups"].Text = (dataItem["UserGroups"].Text).ToString().Substring(0, 20) + "...";
                    //}

                    String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"].ToString();
                    if (Convert.ToInt32(itemDataId) != 0)
                    {
                        Dictionary<Int32, Boolean> selectedItems = CurrentViewContext.AssignOrganizationUserIds;
                        if (selectedItems.IsNotNull())
                        {
                            if (selectedItems.ContainsKey(Convert.ToInt32(itemDataId)))
                            {
                                if (Convert.ToBoolean(selectedItems[Convert.ToInt32(itemDataId)].ToString()))
                                {
                                    CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                                    checkBox.Checked = true;
                                }
                                else
                                {
                                    CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                                    checkBox.Checked = Convert.ToBoolean(selectedItems[Convert.ToInt32(itemDataId)].ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                        checkBox.Enabled = false;
                    }
                }
                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdApplicantSearchData.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdApplicantSearchData.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectItem"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem headerItem = grdApplicantSearchData.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                            CheckBox checkBox = ((CheckBox)headerItem.FindControl("chkSelectAll"));
                            checkBox.Checked = true;
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

        private String GetFormattedSSN(String unformattedSSN)
        {
            try
            {
                return Presenter.GetFormattedSSN(unformattedSSN);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
                return null;
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Grid Item Command event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region For Filter command

                if (!ViewState["SortExpression"].IsNull())
                {
                    CurrentViewContext.GridCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
                }

                #endregion

                // Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdApplicantSearchData);
                }

                #region EXPORT FUNCTIONALITY
                //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                // and displayed the masked column on Export instead of actual column.
                if (e.CommandName.IsNullOrEmpty())
                {
                    WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                    if (cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel"))
                    {
                        grdApplicantSearchData.MasterTableView.GetColumn("_SSN").Display = true;

                    }
                }
                else if (e.CommandName == "Cancel")
                {
                    grdApplicantSearchData.MasterTableView.GetColumn("_SSN").Display = false;
                }
                #endregion

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
        protected void grdApplicantSearchData_SortCommand(object sender, GridSortCommandEventArgs e)
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

        #region Dropdown Events

        /// <summary>
        /// Tenant Name DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            ddlTenantName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        }

        protected void ddlUserGroup_DataBound(object sender, EventArgs e)
        {
            ddlUserGroup.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        }

        protected void ddlTenantName_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ViewState["IsBind"] = null;
            if (ddlTenantName.SelectedIndex <= 0)
            {
                ucCustomAttributeLoaderSearch.Reset();
            }
            else
            {
                ucCustomAttributeLoaderSearch.Reset(CurrentViewContext.SelectedTenantId);

            }
            BindUserGroups();
            //if (ddlUserGroup.SelectedIndex <= 0)
            //{
            //    fsucCmdBarButton.ClearButton.Style.Add("display", "none");
            //}
            hdnSelectedTenantID.Value = ddlTenantName.SelectedValue;


            // BindAgency(); [ddl]
            BindContacts();//
            BindWeekDays();
            // Reset AgencyHierarchySelection UC//
            ucAgencyHierarchy.Reset(); //UAT-2600

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

        #endregion

        #region Checkbox Events
        protected void chkSelectItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                //Boolean isMapped = false;
                if (checkBox.IsNull())
                {
                    return;
                }
                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                Dictionary<Int32, Boolean> selectedItems = CurrentViewContext.AssignOrganizationUserIds;
                Dictionary<Int32, String> SelectedOrgUser = CurrentViewContext.SelectedOrgUsersToList;
                Int32 orgUserID = (Int32)dataItem.GetDataKeyValue("OrganizationUserId");
                String orgUserName = Convert.ToString(dataItem["ApplicantFirstName"].Text) + " " + Convert.ToString(dataItem["ApplicantLastName"].Text);
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectItem")).Checked;
                //if (((Label)(dataItem.FindControl("lblIsUserGroup"))).Text != String.Empty)
                //{
                //    isMapped = Convert.ToBoolean(((Label)(dataItem.FindControl("lblIsUserGroup"))).Text);
                //}

                if (selectedItems.IsNotNull() && selectedItems.ContainsKey(orgUserID)) // && isMapped
                {
                    selectedItems[orgUserID] = isChecked;
                }
                else if (selectedItems.IsNotNull() && selectedItems.ContainsKey(orgUserID) && !isChecked) //&& !isMapped
                {
                    selectedItems.Remove(orgUserID);
                }
                else
                {
                    if (!selectedItems.ContainsKey(orgUserID))
                    {
                        selectedItems.Add(orgUserID, isChecked);
                    }
                }

                if (isChecked)
                {
                    if (!SelectedOrgUser.ContainsKey(orgUserID))
                    {
                        SelectedOrgUser.Add(orgUserID, orgUserName);
                    }
                }
                else
                {
                    if (SelectedOrgUser != null && SelectedOrgUser.ContainsKey(orgUserID))
                    {
                        SelectedOrgUser.Remove(orgUserID);
                    }
                }

                CurrentViewContext.AssignOrganizationUserIds = selectedItems;
                CurrentViewContext.SelectedOrgUsersToList = SelectedOrgUser;
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

        #region Command Bar Events
        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
        {
            //Reset session
            Session[AppConsts.STUDENT_BUCKET_ASSIGNEMNT_SESSION_KEY] = null;
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
        }

        protected void fsucCmdBarButton_ClearClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Perform Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarButton_SaveClick(object sender, EventArgs e)
        {
            if (ViewState["SelectedApplicants"] != null)
            {
                ViewState["SelectedApplicants"] = null;
            }
            ViewState["IsBind"] = null;
            CurrentViewContext.SelectedOrgUsersToList = new Dictionary<Int32, String>();
            grdApplicantSearchData.Visible = true;
            //To reset grid filters 
            ShowHideControls(true);
            ResetGridFilters();

            //if (ddlUserGroup.SelectedIndex > 0 && grdApplicantSearchData.Items.Count > 0)
            //{
            //    fsucCmdBarButton.ClearButton.Style.Clear();
            //}
            //else
            //{
            //    fsucCmdBarButton.ClearButton.Style.Add("display", "none");
            //}
        }

        protected void fsucCmdBarButton_SubmitClick(object sender, EventArgs e)
        {
            CurrentViewContext.VirtualRecordCount = 0;
            ViewState["IsBind"] = null;
            if (ViewState["SelectedApplicants"] != null)
            {
                ViewState["SelectedApplicants"] = null;
            }
            Presenter.GetTenants();
            BindControls();
            txtFirstName.Text = String.Empty;
            txtUserID.Text = String.Empty;
            txtLastName.Text = String.Empty;
            dpkrDOB.SelectedDate = null;
            txtEmail.Text = String.Empty;
            txtSSN.Text = String.Empty;
            CurrentViewContext.SSN = null;
            if (Presenter.IsDefaultTenant)
            {
                ucCustomAttributeLoaderSearch.Reset();
            }
            else
            {
                ucCustomAttributeLoaderSearch.ResetControlData(true);
            }
            //rbtnResults.SelectedValue = "false";
            //dpOrderCreatedFrom.SelectedDate = null;
            //dpOrderCreatedTo.SelectedDate = null;
            //To reset grid filters 
            ResetGridFilters();
            //Reset session
            Session[AppConsts.STUDENT_BUCKET_ASSIGNEMNT_SESSION_KEY] = null;
            //if (ddlUserGroup.SelectedIndex <= 0)
            //{
            //    fsucCmdBarButton.ClearButton.Style.Add("display", "none");
            //}

            if (caRotationCustomAttributesID.IsNotNull())
                caRotationCustomAttributesID.Reset();


            //ddlAgency.SelectedIndex = AppConsts.NONE; [ddl]
            ucAgencyHierarchy.AgencyId = AppConsts.NONE; //UAT-2600
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
            caRotationCustomAttributesID.ResetCustomAttributes();

            //Reset AgencyHierarchySelection UC //
            ucAgencyHierarchy.Reset(); //UAT-2600
        }


        #endregion

        #region Button Events i.e UserGroup, Rotation, Notification Etc
        protected void btnScheduleRotation_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedOrgUsersToList.IsNotNull() && !CurrentViewContext.SelectedOrgUsersToList.Any())
                {
                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select user(s) to send message.", MessageType.Information);
                    // base.ShowErrorInfoMessage("Please select user(s) to send message.");
                }
                else
                {
                    Dictionary<String, Object> data = new Dictionary<String, Object>();
                    data.Add("orgUserIds", String.Join(",", CurrentViewContext.SelectedOrgUsersToList.Keys));
                    data.Add("tenantId", CurrentViewContext.SelectedTenantId);
                    data.Add("CurentLoggedInUserId", CurrentViewContext.CurrentLoggedInUserId);

                    var loggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                    var exceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

                    ParallelTaskContext.PerformParallelTask(Presenter.SendScheduleRotationNotification, data, loggerService, exceptiomService);

                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Message delivered successfully for selected user(s).", MessageType.SuccessMessage);
                    //base.ShowSuccessMessage("Message delivered successfully for selected user(s).");
                    grdApplicantSearchData.Rebind();
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

        protected void btnCustomMessage_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedOrgUsersToList.IsNotNull() && !CurrentViewContext.SelectedOrgUsersToList.Any())
                {
                    //UAT-2052 : C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select user(s) to send message.", MessageType.Information);
                    //base.ShowErrorInfoMessage("Please select user(s) to send message.");
                }
                else
                {
                    if (!Session["OrgUsersToList"].IsNullOrEmpty())
                    {
                        Session.Remove("OrgUsersToList");
                    }
                    Session["OrgUsersToList"] = CurrentViewContext.SelectedOrgUsersToList;
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

        protected void btnScheduleRequirements_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedOrgUsersToList.IsNotNull() && !CurrentViewContext.SelectedOrgUsersToList.Any())
                {
                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select user(s) to send message.", MessageType.Information);
                    // base.ShowErrorInfoMessage("Please select user(s) to send message.");
                }
                else
                {
                    Dictionary<String, Object> data = new Dictionary<String, Object>();
                    data.Add("orgUserIds", String.Join(",", CurrentViewContext.SelectedOrgUsersToList.Keys));
                    data.Add("tenantId", CurrentViewContext.SelectedTenantId);
                    data.Add("CurentLoggedInUserId", CurrentViewContext.CurrentLoggedInUserId);

                    var loggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                    var exceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;

                    ParallelTaskContext.PerformParallelTask(Presenter.SendScheduleRequirementsNotification, data, loggerService, exceptiomService);

                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Message delivered successfully for selected user(s).", MessageType.SuccessMessage);
                    //base.ShowSuccessMessage("Message delivered successfully for selected user(s).");
                    grdApplicantSearchData.Rebind();
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

        protected void btnArchive_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedOrgUsersToList.IsNotNull() && !CurrentViewContext.SelectedOrgUsersToList.Any())
                {
                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select user(s) to archive.", MessageType.Information);
                    // base.ShowErrorInfoMessage("Please select user(s) to archive.");
                }
                else
                {
                    Boolean result = Presenter.ArchieveSubscriptions();
                    if (result)
                    {
                        //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                        base.ShowAlertMessage("Subscriptions for selected applicant archived successfully.", MessageType.SuccessMessage);
                        // base.ShowSuccessMessage("Subscriptions for selected applicant archived successfully.");
                        Presenter.SetQueueImaging(); //UAT-2422-Resync data to flat tables
                        grdApplicantSearchData.Rebind();
                    }
                    else
                    {
                        //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                        base.ShowAlertMessage("Subscriptions are not archived successfully. Please try again.", MessageType.Error);
                        // base.ShowErrorMessage("Subscriptions are not archived successfully. Please try again.");
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

        protected void btnUnArchive_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedOrgUsersToList.IsNotNull() && !CurrentViewContext.SelectedOrgUsersToList.Any())
                {
                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select user(s) to Unarchive.", MessageType.Information);
                    //    base.ShowErrorInfoMessage("Please select user(s) to Unarchive.");
                }
                else
                {
                    Boolean result = Presenter.UnarchiveSubscription();
                    if (result)
                    {
                        //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                        base.ShowAlertMessage("Subscriptions for selected applicant Unarchived successfully.", MessageType.SuccessMessage);
                        //base.ShowSuccessMessage("Subscriptions for selected applicant Unarchived successfully.");
                        Presenter.SetQueueImaging(); //UAT-2422-Resync data to flat tables
                        grdApplicantSearchData.Rebind();
                    }
                    else
                    {
                        //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                        base.ShowAlertMessage("Subscriptions are not Unarchived successfully. Please try again.", MessageType.Error);
                        // base.ShowErrorMessage("Subscriptions are not Unarchived successfully. Please try again.");
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

        protected void btnUserGroupAssign_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedTenantId == AppConsts.NONE)
                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select Institution.", MessageType.Information);
                //base.ShowInfoMessage("Please select Institution.");
                else if (!CurrentViewContext.SelectedOrgUsersToList.Any())
                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select user(s) to assign user group(s).", MessageType.Information);
                //  base.ShowErrorInfoMessage("Please select user(s) to assign user group(s).");
                else
                {
                    if (!Session["OrgUsersToList"].IsNullOrEmpty())
                    {
                        Session.Remove("OrgUsersToList");
                    }
                    Session["OrgUsersToList"] = CurrentViewContext.SelectedOrgUsersToList;
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenUserGroupMappingPopup('assign');", true);
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

        protected void btnUserGroupUnassign_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedTenantId == AppConsts.NONE)
                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select Institution.", MessageType.Information);
                //base.ShowInfoMessage("Please select Institution.");
                else if (!CurrentViewContext.SelectedOrgUsersToList.Any())
                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select user(s) to unassign user group(s).", MessageType.Information);
                // base.ShowErrorInfoMessage("Please select user(s) to unassign user group(s).");
                else
                {
                    if (!Session["OrgUsersToList"].IsNullOrEmpty())
                    {
                        Session.Remove("OrgUsersToList");
                    }
                    Session["OrgUsersToList"] = CurrentViewContext.SelectedOrgUsersToList;
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenUserGroupMappingPopup('unassign');", true);
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

        protected void btnRotationAssign_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedTenantId == AppConsts.NONE)
                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select Institution.", MessageType.Information);
                // base.ShowInfoMessage("Please select Institution.");
                else if (!CurrentViewContext.SelectedOrgUsersToList.Any())
                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select user(s) to assign rotation(s).", MessageType.Information);
                // base.ShowErrorInfoMessage("Please select user(s) to assign rotation(s).");
                else
                {
                    if (!Session["OrgUsersToList"].IsNullOrEmpty())
                    {
                        Session.Remove("OrgUsersToList");
                    }
                    Session["OrgUsersToList"] = CurrentViewContext.SelectedOrgUsersToList;
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenRotationMappingPopup('assign');", true);
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

        protected void btnRotationUnassign_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedTenantId == AppConsts.NONE)
                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select Institution.", MessageType.Information);
                // base.ShowInfoMessage("Please select Institution.");
                else if (!CurrentViewContext.SelectedOrgUsersToList.Any())
                    //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                    base.ShowAlertMessage("Please select user(s) to unassign rotation(s).", MessageType.Information);
                //base.ShowErrorInfoMessage("Please select user(s) to unassign rotation(s).");
                else
                {
                    if (!Session["OrgUsersToList"].IsNullOrEmpty())
                    {
                        Session.Remove("OrgUsersToList");
                    }
                    Session["OrgUsersToList"] = CurrentViewContext.SelectedOrgUsersToList;
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenRotationMappingPopup('unassign');", true);
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

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        #region UAT-3861 Client assignments should be retained on package copy (hierarchy or compliance mapping)
        /// <summary>
        /// Show and Hide Controls 
        /// </summary>
        /// <param name="IsVisible"></param>
        private void ShowHideControls(Boolean IsVisible)
        {
            grdApplicantSearchData.Visible = IsVisible;
            cmd.Visible = IsVisible;
        }

        #endregion

        /// <summary>
        /// To bind program dropdown
        /// </summary>
        private void BindUserGroups()
        {
            Presenter.GetAllUserGroups();
            ddlUserGroup.DataSource = CurrentViewContext.lstUserGroup;
            ddlUserGroup.DataBind();
        }

        /// <summary>
        /// To set controls values in session
        /// </summary>
        private void SetSessionValues()
        {
            SearchItemDataContract searchDataContract = new SearchItemDataContract();

            searchDataContract.ClientID = CurrentViewContext.SelectedTenantId;
            searchDataContract.ApplicantFirstName = CurrentViewContext.ApplicantFirstName;
            searchDataContract.ApplicantLastName = CurrentViewContext.ApplicantLastName;
            searchDataContract.EmailAddress = CurrentViewContext.EmailAddress;
            searchDataContract.DateOfBirth = CurrentViewContext.DateOfBirth;
            searchDataContract.ApplicantSSN = CurrentViewContext.SSN;
            searchDataContract.OrganizationUserId = CurrentViewContext.OrganizationUserID.Value;
            //UAT-1055: WB: As an admin, I should be able to search multiple nodes at a time.
            //searchDataContract.DPM_Id = ucCustomAttributeLoaderSearch.DPM_ID;
            searchDataContract.SelectedDPMIds = ucCustomAttributeLoaderSearch.DPM_ID;
            searchDataContract.CustomFields = ucCustomAttributeLoaderSearch.GetCustomDataXML();
            //UAT-1688:As an admin, I should be able to select All/Active/Archived on the Manage Applicant User Group Mapping screen
            //searchDataContract.LstArchiveState = CurrentViewContext.SelectedArchiveStateCode;
            var serializer = new XmlSerializer(typeof(SearchItemDataContract));
            var strbuilder = new StringBuilder();

            using (TextWriter writer = new StringWriter(strbuilder))
            {
                serializer.Serialize(writer, searchDataContract);
            }
            //Session for maintaining control values
            Session[AppConsts.STUDENT_BUCKET_ASSIGNEMNT_SESSION_KEY] = strbuilder.ToString();
        }

        /// <summary>
        /// To get session values for controls
        /// </summary>
        private void GetSessionValues()
        {
            var serializer = new XmlSerializer(typeof(SearchItemDataContract));
            SearchItemDataContract searchDataContract = new SearchItemDataContract();
            if (Session[AppConsts.STUDENT_BUCKET_ASSIGNEMNT_SESSION_KEY].IsNotNull())
            {
                TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.STUDENT_BUCKET_ASSIGNEMNT_SESSION_KEY]));
                searchDataContract = (SearchItemDataContract)serializer.Deserialize(reader);

                CurrentViewContext.SelectedTenantId = searchDataContract.ClientID;
                CurrentViewContext.ApplicantFirstName = searchDataContract.ApplicantFirstName;
                CurrentViewContext.ApplicantLastName = searchDataContract.ApplicantLastName;
                CurrentViewContext.EmailAddress = searchDataContract.EmailAddress;
                CurrentViewContext.DateOfBirth = searchDataContract.DateOfBirth;
                //SSN = searchDataContract.ApplicantSSN;

                if (CurrentViewContext.SSN.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
                {
                    if (!searchDataContract.ApplicantSSN.IsNullOrEmpty())
                    {
                        CurrentViewContext.SSN = searchDataContract.ApplicantSSN.Substring(searchDataContract.ApplicantSSN.Length - AppConsts.FOUR);
                        txtSSN.Text = CurrentViewContext.SSN;
                    }
                }
                else
                {
                    CurrentViewContext.SSN = searchDataContract.ApplicantSSN;
                    txtSSN.Text = CurrentViewContext.SSN;
                }

                CurrentViewContext.OrganizationUserID = searchDataContract.OrganizationUserId;

                #region UAT-1088
                //OrderCreatedFrom = searchDataContract.OrderCreatedFrom;
                //OrderCreatedTo = searchDataContract.OrderCreatedTo;
                #endregion

                //UAT-1688:As an admin, I should be able to select All/Active/Archived on the Manage Applicant User Group Mapping screen
                //if (searchDataContract.ClientID != 0)
                //{
                //    Presenter.GetArchiveStateList();
                //}
                //if (searchDataContract.LstArchiveState.IsNotNull() && searchDataContract.LstArchiveState.Count > 0)
                //{
                //    rbSubscriptionState.SelectedValue = searchDataContract.LstArchiveState.FirstOrDefault();
                //}
                //else if (searchDataContract.LstArchiveState.IsNotNull() && searchDataContract.LstArchiveState.Count == 0)
                //{
                //    rbSubscriptionState.SelectedValue = ArchiveState.All.GetStringValue();
                //}

                //Reset session
                Session[AppConsts.STUDENT_BUCKET_ASSIGNEMNT_SESSION_KEY] = null;
            }
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdApplicantSearchData.MasterTableView.SortExpressions.Clear();
            grdApplicantSearchData.CurrentPageIndex = 0;
            grdApplicantSearchData.MasterTableView.CurrentPageIndex = 0;
            grdApplicantSearchData.Rebind();
        }

        private void ApplySSNMask()
        {
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue().ToUpper())
            {
                //txtSSN.Mask = AppConsts.SSN_MASK_FORMATE; //@"\#\#\#-\#\#-####"
                txtSSN.Mask = AppConsts.SSN_MASK_FORMAT_ALPHANUMERIC;
            }
        }

        private void BindControls()
        {
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();

            if (Presenter.IsDefaultTenant)
            {
                ddlTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantId = 0;
            }
            else
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
            }
            BindUserGroups();
            //Presenter.GetArchiveStateList();

            //  BindAgency(); [ddl]
            BindContacts();
            BindWeekDays();
        }

        /// <summary>
        /// Hide Show grid and page controls
        /// </summary>
        private void HideShowControlsForGranularPermission()
        {
            if (CurrentViewContext.IsDOBDisable)
            {
                divDOB.Visible = false;
                grdApplicantSearchData.MasterTableView.GetColumn("DateOfBirth").Visible = false;
            }

            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                divSSN.Visible = false;
                grdApplicantSearchData.MasterTableView.GetColumn("SSN").Visible = false;
                //Hide Masked column if user does not have permission to view SSN Column.
                grdApplicantSearchData.MasterTableView.GetColumn("_SSN").Visible = false;
            }

            if (!CurrentViewContext.LstStudentBucketAssigmentPermissions.IsNullOrEmpty())
            {
                Dictionary<String, String> dicControls = new Dictionary<String, String>();
                dicControls.Add("UserGroup", "btnUserGroup");
                dicControls.Add("Rotation", "btnRotation");
                dicControls.Add("Archivemun", "btnArchivemun");
                dicControls.Add("SendMessage", "btnsendmail");

                ShowHideControl(dicControls, false);

                if (CurrentViewContext.LstStudentBucketAssigmentPermissions.Contains(EnumSystemPermissionCode.ASSIGN_UNASSIGN_USER_GROUP.GetStringValue()))
                {
                    ShowHideControl("UserGroup", "btnUserGroup", true);
                }
                if (CurrentViewContext.LstStudentBucketAssigmentPermissions.Contains(EnumSystemPermissionCode.ASSIGN_UNASSIGN_ROTATION.GetStringValue()))
                {
                    ShowHideControl("Rotation", "btnRotation", true);
                }
                if (CurrentViewContext.LstStudentBucketAssigmentPermissions.Contains(EnumSystemPermissionCode.ARCHIVE_UNARCHIVE.GetStringValue()))
                {
                    ShowHideControl("Archivemun", "btnArchivemun", true);
                }
                if (CurrentViewContext.LstStudentBucketAssigmentPermissions.Contains(EnumSystemPermissionCode.SEND_MESSAGE.GetStringValue()))
                {
                    ShowHideControl("SendMessage", "btnsendmail", true);
                    if (CurrentViewContext.LstStudentBucketAssigmentPermissions.Contains(EnumSystemPermissionCode.ASSIGN_UNASSIGN_ROTATION.GetStringValue()))
                    {
                        ShowHideControl("ScheduleRotation", "btnScheduleRotation", true);
                        ShowHideControl("ScheduleRequirements", "btnScheduleRequirements", true);
                    }
                    else
                    {
                        ShowHideControl("ScheduleRotation", "btnScheduleRotation", false);
                        ShowHideControl("ScheduleRequirements", "btnScheduleRequirements", false);
                    }
                }
                if (CurrentViewContext.LstStudentBucketAssigmentPermissions.Contains(EnumSystemPermissionCode.NONPER.GetStringValue()))
                {
                    ShowHideControl(dicControls, false);
                }
            }


            // UAT-3010:-  Granular Permission for Client Admin Users to Archive.
            if (CurrentViewContext.ArchivePermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {

                RadMenuItem menuItem = cmd.FindItemByText("Archivemun");
                WclButton btnArchive;
                foreach (var item in menuItem.Items)
                {
                    RadMenuItem itemdata = (RadMenuItem)item;
                    btnArchive = (WclButton)itemdata.FindControl("btnArchive");
                    if (btnArchive != null)
                    {
                        itemdata.Visible = false;
                        btnArchive.Visible = false;
                        break;
                    }
                }



            }
        }

        #region UAT-2056,Buttons should be permissions based (I didn't specifically call this out in the request and should have). (C14)
        private void ShowHideControl(Dictionary<String, String> dicControls, Boolean isVisible)
        {
            foreach (var item in dicControls)
            {
                RadMenuItem menuItem = cmd.FindItemByText(item.Key);
                RadButton btnMenu = (RadButton)menuItem.FindControl(item.Value);
                btnMenu.Visible = isVisible;
            }
        }

        private void ShowHideControl(String menuText, String controlID, Boolean isVisible)
        {
            RadMenuItem menuItem = cmd.FindItemByText(menuText);
            RadButton btnMenu = (RadButton)menuItem.FindControl(controlID);
            btnMenu.Visible = isVisible;
        }
        #endregion

        private void DisplayMessageSentStatus()
        {
            if (hdMessageSent.Value == "sent")
            {
                //UAT-2052: C10:Success or Failure messages should display as their own popups with an okay button
                base.ShowAlertMessage("Message has been sent successfully.", MessageType.SuccessMessage);
                //  base.ShowSuccessMessage("Message has been sent successfully.");
                hdMessageSent.Value = "new";
            }
        }

        //private void BindAgency()    [ddl]
        //{
        //    Presenter.GetAllAgency();
        //    ddlAgency.DataSource = CurrentViewContext.lstAgency;
        //    ddlAgency.DataBind();
        //}

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

        #endregion

        protected void cmd_ItemDataBound(object sender, RadMenuEventArgs e)
        {
            if (CurrentViewContext.LstStudentBucketAssigmentPermissions.IsNotNull())
            {
                if (CurrentViewContext.LstStudentBucketAssigmentPermissions.Contains(EnumSystemPermissionCode.ASSIGN_UNASSIGN_USER_GROUP.GetStringValue()))
                {
                    RadButton btnUserGroup = (RadButton)e.Item.DataItem;
                    if (btnUserGroup.IsNotNull())
                    {
                        btnUserGroup.Visible = true;
                    }
                }
            }
        }
        #endregion

    }
}