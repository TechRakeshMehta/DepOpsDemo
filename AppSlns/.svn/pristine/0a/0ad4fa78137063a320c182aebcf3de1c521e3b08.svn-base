using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.SearchUI.Views;
using CoreWeb.Shell;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.Utils;
using Telerik.Web.UI;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using System.IO;
using CoreWeb.ClinicalRotation.Views;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ClinicalRotationMapping : BaseWebPage, IClinicalRotationMappingView
    {
        #region  Variables
        #region Private Variables
        private ClinicalRotationMappingPresenter _presenter = new ClinicalRotationMappingPresenter();
        private ClinicalRotationDetailContract _viewContract = null;
        private Int32 _tenantId;
        #endregion
        #endregion

        #region Properties

        #region Public Properties

        public ClinicalRotationMappingPresenter Presenter
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

        public IClinicalRotationMappingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String IClinicalRotationMappingView.ErrorMessage
        {
            get;
            set;
        }

        String IClinicalRotationMappingView.SuccessMessage
        {
            get;
            set;
        }

        Int32 IClinicalRotationMappingView.TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
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

        Int32 IClinicalRotationMappingView.SelectedTenantID
        {
            get
            {
                if (ViewState["SelectedTenantID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["SelectedTenantID"]);
                }
                return 0;
            }
            set
            {
                ViewState["SelectedTenantID"] = value;
            }
        }

        Int32 IClinicalRotationMappingView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        ClinicalRotationDetailContract IClinicalRotationMappingView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ClinicalRotationDetailContract();
                }
                return _viewContract;
            }
        }

        List<ClinicalRotationDetailContract> IClinicalRotationMappingView.ClinicalRotationData
        {
            get;
            set;
        }

        List<TenantDetailContract> IClinicalRotationMappingView.lstTenant
        {
            get;
            set;
        }

        List<CustomAttribteContract> IClinicalRotationMappingView.GetCustomAttributeList
        {
            get;
            set;
        }

        List<CustomAttribteContract> IClinicalRotationMappingView.SaveCustomAttributeList
        {
            get;
            set;
        }

        List<Int32> IClinicalRotationMappingView.SelectedClientContacts
        {
            get;
            set;
        }

        List<AgencyDetailContract> IClinicalRotationMappingView.lstAgencyForAddForm
        {
            get;
            set;
        }

        Int32 IClinicalRotationMappingView.SelectedAgencyIDForAddForm
        {
            get;
            set;
        }

        Int32 IClinicalRotationMappingView.SelectedTenantIDForAddForm
        {
            get;
            set;
        }

        List<ClientContactContract> IClinicalRotationMappingView.ClientContactListForAddForm
        {
            get
            {
                if (!ViewState["ClientContactListForAddForm"].IsNull())
                {
                    return ViewState["ClientContactListForAddForm"] as List<ClientContactContract>;
                }

                return new List<ClientContactContract>();
            }
            set
            {
                ViewState["ClientContactListForAddForm"] = value;
            }
        }

        List<WeekDayContract> IClinicalRotationMappingView.WeekDayListForAddForm
        {
            get;
            set;
        }

        String IClinicalRotationMappingView.HierarchyNode
        {
            get;
            set;
        }

        /// <summary>
        /// Granular Permissions of the logged-in user.
        /// </summary>
        Dictionary<String, String> IClinicalRotationMappingView.dicGranularPermissions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        public List<Int32> AssignRotationIds
        {
            get
            {
                if (!ViewState["AssignRotationIds"].IsNull())
                {
                    return ViewState["AssignRotationIds"] as List<Int32>;
                }

                return new List<Int32>();
            }
            set
            {
                ViewState["AssignRotationIds"] = value;
            }
        }

        public List<Int32> ApplicantUserIds
        {
            get
            {
                if (!Session["OrgUsersToList"].IsNullOrEmpty())
                {
                    var orgUsersToList = Session["OrgUsersToList"] as Dictionary<Int32, String>;
                    return orgUsersToList.Keys.ToList();
                }
                return new List<Int32>();
            }
        }

        public String ScreenMode
        {
            get
            {
                if (ViewState["ScreenMode"].IsNotNull())
                {
                    return Convert.ToString(ViewState["ScreenMode"]);
                }
                return null;
            }
            set
            {
                ViewState["ScreenMode"] = value;
            }
        }


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
                return grdRotations.MasterTableView.PageSize;
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

        #endregion

        #endregion

        #region Events

        #region  Page Events

        protected override void OnInit(EventArgs e)
        {
            base.Title = "Rotation Mapping";
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            lblMessage.Visible = false;


            if (!this.IsPostBack)
            {
                //Capture Querystring parameters
                CaptureQuerystringParameters();

                if (Presenter.IsAdminLoggedIn())
                {

                }
            }
            ShowHideControls();

        }

        #endregion

        #region Grid Related Events

        protected void grdRotations_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                GridCustomPaging.DefaultSortExpression = "RotationId";

                CurrentViewContext.GridCustomPaging.FilterColumns = ViewState["FilterColumns"] == null ? new List<String>() : (List<String>)(ViewState["FilterColumns"]);
                CurrentViewContext.GridCustomPaging.FilterOperators = ViewState["FilterOperators"] == null ? new List<String>() : (List<String>)(ViewState["FilterOperators"]);
                CurrentViewContext.GridCustomPaging.FilterValues = ViewState["FilterValues"] == null ? new System.Collections.ArrayList() : (System.Collections.ArrayList)(ViewState["FilterValues"]);
                CurrentViewContext.GridCustomPaging.FilterTypes = ViewState["FilterTypes"] == null ? new List<String>() : (List<String>)ViewState["FilterTypes"];

                Presenter.GetRotationDetail();
                grdRotations.DataSource = CurrentViewContext.ClinicalRotationData;

                if (CurrentViewContext.ScreenMode.ToLower() != "assign")
                    grdRotations.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
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
                CurrentViewContext.GridCustomPaging.FilterColumns = ViewState["FilterColumns"] == null ? new List<String>() : (List<String>)(ViewState["FilterColumns"]);
                CurrentViewContext.GridCustomPaging.FilterOperators = ViewState["FilterOperators"] == null ? new List<String>() : (List<String>)(ViewState["FilterOperators"]);
                CurrentViewContext.GridCustomPaging.FilterValues = ViewState["FilterValues"] == null ? new System.Collections.ArrayList() : (System.Collections.ArrayList)(ViewState["FilterValues"]);
                CurrentViewContext.GridCustomPaging.FilterTypes = ViewState["FilterTypes"] == null ? new List<String>() : (List<String>)ViewState["FilterTypes"];

                if (e.CommandName == RadGrid.FilterCommandName)
                {
                    /*UAT-2904*/
                    foreach (GridColumn item in grdRotations.MasterTableView.Columns)
                    {
                        string filterFunction = item.CurrentFilterFunction.ToString();
                        string filterValue = item.CurrentFilterValue;
                        if (filterValue.IsNullOrEmpty())
                        {
                            item.CurrentFilterFunction = Telerik.Web.UI.GridKnownFunction.NoFilter;
                        }
                    }
                    /*UAT-2904 ENDS here*/
                    Pair filter = (Pair)e.CommandArgument;

                    Int32 filterIndex = CurrentViewContext.GridCustomPaging.FilterColumns.IndexOf(filter.Second.ToString());
                    if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString())
                    {
                        String filteringType = (e.Item as GridFilteringItem)[filter.Second.ToString()].Controls[0].GetType().Name;
                        String filterValue = grdRotations.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;
                        String filterValueType = grdRotations.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName;
                        if (filterIndex != -1)
                        {
                            CurrentViewContext.GridCustomPaging.FilterOperators[filterIndex] = filter.First.ToString();
                            CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = filterValue.ToLower();
                            CurrentViewContext.GridCustomPaging.FilterTypes[filterIndex] = filterValueType;
                            if (filterValueType == "System.Decimal")
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDecimal(filterValue);
                            }
                            else if (filterValueType == "System.Int32")
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToInt32(filterValue);
                            }
                            else if (filterValueType == "System.DateTime")
                            {
                                //  CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = Convert.ToDateTime(filterValue);
                            }
                            else
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues[filterIndex] = filterValue.ToLower();
                            }
                        }
                        else
                        {
                            CurrentViewContext.GridCustomPaging.FilterColumns.Add(filter.Second.ToString());
                            CurrentViewContext.GridCustomPaging.FilterOperators.Add(filter.First.ToString());
                            CurrentViewContext.GridCustomPaging.FilterTypes.Add(filterValueType);
                            if (grdRotations.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Decimal")
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToDecimal(filterValue));
                            }
                            else if (grdRotations.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.Int32")
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues.Add(Convert.ToInt32(filterValue));
                            }
                            else if (grdRotations.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName == "System.DateTime")
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues.Add(filterValue);
                            }
                            else
                            {
                                CurrentViewContext.GridCustomPaging.FilterValues.Add(filterValue.ToLower());
                            }

                        }
                    }
                    else
                    {
                        if (CurrentViewContext.GridCustomPaging.FilterOperators.Count > 0)
                        {
                            CurrentViewContext.GridCustomPaging.FilterOperators.RemoveAt(filterIndex);
                            CurrentViewContext.GridCustomPaging.FilterValues.RemoveAt(filterIndex);
                            CurrentViewContext.GridCustomPaging.FilterColumns.RemoveAt(filterIndex);
                            CurrentViewContext.GridCustomPaging.FilterTypes.RemoveAt(filterIndex);
                        }
                    }

                    ViewState["FilterColumns"] = CurrentViewContext.GridCustomPaging.FilterColumns;
                    ViewState["FilterOperators"] = CurrentViewContext.GridCustomPaging.FilterOperators;
                    ViewState["FilterValues"] = CurrentViewContext.GridCustomPaging.FilterValues;
                    ViewState["FilterTypes"] = CurrentViewContext.GridCustomPaging.FilterTypes;
                }

                if (e.CommandName == "InitInsert")
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenAddEditRotationPopup('');", true);
                }
                //Insert/Update functionality
                else if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    if (e.CommandName == RadGrid.UpdateCommandName)
                    {
                        CurrentViewContext.ViewContract.RotationID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"]);
                    }

                    if (hdnInstNodeIdNew.Value == AppConsts.ZERO || hdnInstNodeIdNew.Value == String.Empty)
                    {
                        e.Canceled = true;
                        (e.Item.FindControl("lblName1") as Label).ShowMessage("Please select Institution Hierarchy.", MessageType.Error);
                        (e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = String.Empty;
                        return;
                    }
                    CurrentViewContext.HierarchyNode = hdnDepartmentProgmapNew.Value;
                    WclComboBox ddlTenant = e.Item.FindControl("ddlTenant") as WclComboBox;
                    WclComboBox ddlAgency = e.Item.FindControl("ddlAgency") as WclComboBox;
                    CurrentViewContext.ViewContract.TenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                    CurrentViewContext.ViewContract.AgencyID = Convert.ToInt32(ddlAgency.SelectedValue);
                    CurrentViewContext.ViewContract.RotationName = (e.Item.FindControl("txtClassification") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtClassification") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.Department = (e.Item.FindControl("txtDepartment") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtDepartment") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.Program = (e.Item.FindControl("txtProgram") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtProgram") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.Course = (e.Item.FindControl("txtCourse") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtCourse") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.UnitFloorLoc = (e.Item.FindControl("txtUnit") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtUnit") as WclTextBox).Text.Trim();
                    //UAT-1467: Addition of "Type/Specialty" field on rotation creation and rotation details
                    CurrentViewContext.ViewContract.TypeSpecialty = (e.Item.FindControl("txtTypeSpecialty") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtTypeSpecialty") as WclTextBox).Text.Trim();

                    //UAT 1414 notification to go out prior to student's start date for clinical rotation.
                    // CurrentViewContext.ViewContract.DaysBefore = Convert.ToInt32((e.Item.FindControl("txtDaysBefore") as WclTextBox).Text);
                    if (!String.IsNullOrEmpty((e.Item.FindControl("txtDaysBefore") as WclNumericTextBox).Text.Trim()))
                    {
                        CurrentViewContext.ViewContract.DaysBefore = Convert.ToInt32((e.Item.FindControl("txtDaysBefore") as WclNumericTextBox).Text.Trim());
                    }
                    else
                    {
                        CurrentViewContext.ViewContract.DaysBefore = null;
                    }
                    // CurrentViewContext.ViewContract.DaysBefore = (e.Item.FindControl("txtDaysBefore") as WclNumericTextBox).Text.IsNullOrEmpty() ? (int?)null : Convert.ToInt32((e.Item.FindControl("txtDaysBefore") as WclNumericTextBox).Text.Trim());
                    CurrentViewContext.ViewContract.Frequency = (e.Item.FindControl("txtFrequency") as WclNumericTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtFrequency") as WclNumericTextBox).Text.Trim();

                    if (!String.IsNullOrEmpty((e.Item.FindControl("txtRecommendedHrs") as WclNumericTextBox).Text.Trim()))
                    {
                        CurrentViewContext.ViewContract.RecommendedHours = float.Parse((e.Item.FindControl("txtRecommendedHrs") as WclNumericTextBox).Text.Trim());
                    }
                    else
                    {
                        CurrentViewContext.ViewContract.RecommendedHours = null;
                    }
                    //UAT-1769 Addition of "# of Students" field on rotation creation and rotation details for all except students
                    if (!String.IsNullOrEmpty((e.Item.FindControl("txtStudents") as WclNumericTextBox).Text.Trim()))
                    {
                        CurrentViewContext.ViewContract.Students = float.Parse((e.Item.FindControl("txtStudents") as WclNumericTextBox).Text.Trim());
                    }
                    else
                    {
                        CurrentViewContext.ViewContract.Students = null;
                    }
                    CurrentViewContext.ViewContract.Shift = (e.Item.FindControl("txtShift") as WclTextBox).Text.Trim();
                    //UAT 1355 Addition of Term field to the Create rotation, rotation details screens
                    CurrentViewContext.ViewContract.Term = (e.Item.FindControl("txtTerm") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.StartDate = (e.Item.FindControl("dpStartDate") as WclDatePicker).SelectedDate;
                    CurrentViewContext.ViewContract.EndDate = (e.Item.FindControl("dpEndDate") as WclDatePicker).SelectedDate;
                    CurrentViewContext.ViewContract.StartTime = (e.Item.FindControl("tpStartTime") as WclTimePicker).SelectedTime;
                    CurrentViewContext.ViewContract.EndTime = (e.Item.FindControl("tpEndTime") as WclTimePicker).SelectedTime;
                    WclComboBox ddlDays = e.Item.FindControl("ddlDays") as WclComboBox;
                    WclComboBox ddlInstructor = e.Item.FindControl("ddlInstructor") as WclComboBox;

                    List<Int32> selectedDays = new List<Int32>();
                    foreach (RadComboBoxItem slctdItem in ddlDays.CheckedItems)
                    {
                        selectedDays.Add(Convert.ToInt32(slctdItem.Value));
                    }

                    List<Int32> selectedContacts = new List<Int32>();
                    foreach (RadComboBoxItem slctdContact in ddlInstructor.CheckedItems)
                    {
                        selectedContacts.Add(Convert.ToInt32(slctdContact.Value));
                    }
                    CurrentViewContext.ViewContract.DaysIdList = String.Join(",", selectedDays.ToArray());
                    CurrentViewContext.SelectedClientContacts = selectedContacts;
                    CurrentViewContext.ViewContract.ContactIdList = String.Join(",", selectedContacts.ToArray());
                    CurrentViewContext.ViewContract.HierarchyNodeIDList = CurrentViewContext.HierarchyNode;
                    WclAsyncUpload fileUpload = (e.Item.FindControl("uploadControl") as WclAsyncUpload);
                    Boolean isFileuploaeded = true;
                    Label lblOldFilePath = (e.Item.FindControl("lblUploadFormPath") as Label);
                    if (!hdnFileRemoved.Value.IsNullOrEmpty() && hdnFileRemoved.Value == "True")
                    {

                        CurrentViewContext.ViewContract.IfSyllabusFileRemoved = true;
                    }
                    if (fileUpload.UploadedFiles.Count > AppConsts.NONE)
                    {
                        String savedFilePath = UploadSyllabusDocuments(fileUpload);
                        if (savedFilePath.IsNullOrEmpty())
                            isFileuploaeded = false;
                        CurrentViewContext.ViewContract.SyllabusFilePath = savedFilePath;
                        CurrentViewContext.ViewContract.SyllabusFileName = fileUpload.UploadedFiles[0].FileName;
                        CurrentViewContext.ViewContract.SyllabusFileSize = fileUpload.UploadedFiles[0].ContentLength;
                    }

                    if (e.Item.FindControl("caCustomAttributes").IsNotNull())
                    {
                        SharedUserCustomAttributeForm caCustomAttributes = e.Item.FindControl("caCustomAttributes") as SharedUserCustomAttributeForm;
                        CurrentViewContext.SaveCustomAttributeList = caCustomAttributes.GetCustomAttributeValues();
                    }
                    else
                        CurrentViewContext.SaveCustomAttributeList = new List<CustomAttribteContract>();
                    if (isFileuploaeded)
                    {
                        if (Presenter.SaveUpdateClinicalRotation())
                        {
                            if (e.CommandName == RadGrid.UpdateCommandName)
                            {
                                base.ShowSuccessMessage("Clinical rotation updated successfully.");
                            }
                            else
                            {
                                base.ShowSuccessMessage("Clinical rotation saved successfully.");
                            }
                        }
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowErrorMessage("some error has occured due to which rotation can not be saved.");
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
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"].ToString();

                    if (CurrentViewContext.ScreenMode.ToLower() == "assign")
                    {
                        ClinicalRotationDetailContract clinicalRotationDetailContract = (INTSOF.ServiceDataContracts.Modules.ClinicalRotation.ClinicalRotationDetailContract)e.Item.DataItem;
                        if (!clinicalRotationDetailContract.IsNullOrEmpty())
                        {
                            if (DateTime.Now.Date >= clinicalRotationDetailContract.StartDate && (clinicalRotationDetailContract.EndDate.HasValue && DateTime.Now.Date <= clinicalRotationDetailContract.EndDate.Value))
                            {
                                CheckBox chkSelectItem = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                                chkSelectItem.Enabled = false;
                            }
                        }
                    }

                    if (Convert.ToInt32(itemDataId) != 0)
                    {
                        List<Int32> selectedItems = CurrentViewContext.AssignRotationIds;
                        if (selectedItems.IsNotNull())
                        {
                            if (selectedItems.Contains(Convert.ToInt32(itemDataId)))
                            {
                                CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                                checkBox.Checked = true;
                            }
                        }
                    }
                    else
                    {
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                        checkBox.Enabled = false;
                    }

                    //UAT 3041
                    if (!Presenter.IsAdminLoggedIn())
                    {
                        Boolean IsEditableByClientAdmin = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsEditableByClientAdmin"]);
                        if (!IsEditableByClientAdmin)
                        {
                            CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                            checkBox.Enabled = false;
                        }
                    }
                }

                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdRotations.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdRotations.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectItem"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem headerItem = grdRotations.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                            CheckBox checkBox = ((CheckBox)headerItem.FindControl("chkSelectAll"));
                            checkBox.Checked = true;
                        }
                    }
                }

                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    hdnDepartmentProgmapNew.Value = String.Empty;
                    hdnInstNodeIdNew.Value = String.Empty;
                    WclComboBox ddlTenant = editform.FindControl("ddlTenant") as WclComboBox;
                    WclComboBox ddlAgency = editform.FindControl("ddlAgency") as WclComboBox;
                    Presenter.GetTenants();
                    ddlTenant.DataSource = CurrentViewContext.lstTenant;
                    ddlTenant.DataBind();
                    ddlTenant.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));

                    if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
                    {
                        ddlTenant.Enabled = false;
                        ddlTenant.SelectedValue = CurrentViewContext.SelectedTenantID.ToString();
                        //todo
                        //CurrentViewContext.lstAgencyForAddForm = CurrentViewContext.lstAgency;
                        //if (CurrentViewContext.SelectedAgencyID > AppConsts.NONE)
                        //{
                        //    CurrentViewContext.SelectedAgencyIDForAddForm = CurrentViewContext.SelectedAgencyID;
                        //}
                    }
                    else
                    {
                        ddlTenant.Enabled = true;
                    }

                    BindAgencyForAddForm(ddlAgency);

                    WclComboBox cmbDays = editform.FindControl("ddlDays") as WclComboBox;
                    BindWeekDaysForAddForm(cmbDays);

                    WclComboBox cmbContacts = editform.FindControl("ddlInstructor") as WclComboBox;
                    BindContactssForAddForm(cmbContacts);

                    WclDatePicker dpStartDate = editform.FindControl("dpStartDate") as WclDatePicker;
                    WclDatePicker dpEndDate = editform.FindControl("dpEndDate") as WclDatePicker;
                    WclTimePicker tpStartTime = editform.FindControl("tpStartTime") as WclTimePicker;
                    WclTimePicker tpEndTime = editform.FindControl("tpEndTime") as WclTimePicker;
                    HtmlGenericControl dvComplioId = editform.FindControl("dvComplioId") as HtmlGenericControl;
                    WclAsyncUpload fileUpload = editform.FindControl("uploadControl") as WclAsyncUpload;
                    Label lblUploadFormName = editform.FindControl("lblUploadFormName") as Label;
                    Label lblUploadFormPath = editform.FindControl("lblUploadFormPath") as Label;
                    LinkButton lnkRemove = editform.FindControl("lnkRemove") as LinkButton;

                    WclNumericTextBox txtDaysBefore = editform.FindControl("txtDaysBefore") as WclNumericTextBox;
                    WclNumericTextBox txtFrequency = editform.FindControl("txtFrequency") as WclNumericTextBox;

                    dvComplioId.Visible = false;
                    ClinicalRotationDetailContract clinicalRotationDetail = e.Item.DataItem as ClinicalRotationDetailContract;

                    //UAT 1414 notification to go out prior to student's start date for clinical rotation
                    txtDaysBefore.Text = "30";
                    txtFrequency.Text = "15";
                    if (e.Item is GridEditFormInsertItem)
                    {
                        if (!Presenter.IsAdminLoggedIn())
                        {
                            Dictionary<Int32, String> dicDefaultNode = Presenter.GetDefaultPermissionForClientAdmin();
                            if (!dicDefaultNode.IsNullOrEmpty())
                            {
                                hdnDepartmentProgmapNew.Value = Convert.ToString(dicDefaultNode.Keys.FirstOrDefault());
                                hdnInstNodeIdNew.Value = Convert.ToString(dicDefaultNode.Keys.FirstOrDefault());
                                (e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = dicDefaultNode.Values.FirstOrDefault();
                            }
                        }
                    }

                    if (clinicalRotationDetail != null)
                    {
                        dvComplioId.Visible = true;
                        ddlAgency.Enabled = false;
                        dpStartDate.SelectedDate = clinicalRotationDetail.StartDate;
                        dpEndDate.SelectedDate = clinicalRotationDetail.EndDate;
                        tpStartTime.SelectedTime = clinicalRotationDetail.StartTime;
                        tpEndTime.SelectedTime = clinicalRotationDetail.EndTime;

                        //UAT 1414 notification to go out prior to student's start date for clinical rotation
                        txtDaysBefore.Text = clinicalRotationDetail.DaysBefore.ToString();
                        txtFrequency.Text = clinicalRotationDetail.Frequency;

                        if (!clinicalRotationDetail.ContactIdList.IsNullOrEmpty())
                        {
                            String[] selectedContactIds = clinicalRotationDetail.ContactIdList.Split(',');
                            foreach (RadComboBoxItem item in cmbContacts.Items)
                            {
                                item.Checked = selectedContactIds.Contains(item.Value);
                            }
                        }
                        if (!clinicalRotationDetail.DaysIdList.IsNullOrEmpty())
                        {
                            String[] selectedDayIds = clinicalRotationDetail.DaysIdList.Split(',');
                            foreach (RadComboBoxItem item in cmbDays.Items)
                            {
                                item.Checked = selectedDayIds.Contains(item.Value);
                            }
                        }
                        if (!clinicalRotationDetail.SyllabusFileName.IsNullOrEmpty())
                        {
                            lblUploadFormName.Text = clinicalRotationDetail.SyllabusFileName;
                            lblUploadFormPath.Text = clinicalRotationDetail.SyllabusFilePath;
                            lblUploadFormName.Visible = true;
                            lnkRemove.Visible = true;
                            fileUpload.Visible = false;
                        }

                        hdnDepartmentProgmapNew.Value = clinicalRotationDetail.HierarchyNodeIDList;
                        hdnInstNodeIdNew.Value = clinicalRotationDetail.HierarchyNodeIDList;
                        (e.Item.FindControl("lblInstitutionHierarchyPB") as Label).Text = clinicalRotationDetail.HierarchyNodes;

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

        protected void grdRotations_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                //if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                //{
                //    GridEditFormItem editform = (e.Item as GridEditFormItem);
                //    WclComboBox ddlTenant = editform.FindControl("ddlTenant") as WclComboBox;
                //    if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
                //    {
                //        CurrentViewContext.SelectedTenantIDForAddForm = CurrentViewContext.SelectedTenantID;
                //    }
                //    if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                //    {
                //        CurrentViewContext.SelectedTenantIDForAddForm = Convert.ToInt32(ddlTenant.SelectedValue);
                //    }
                //    if (CurrentViewContext.SelectedTenantIDForAddForm > AppConsts.NONE)
                //    {
                //        SharedUserCustomAttributeForm caCustomAttributes = (SharedUserCustomAttributeForm)Page.LoadControl("~\\ClinicalRotation\\UserControl\\SharedUserCustomAttributeForm.ascx");
                //        caCustomAttributes.ID = "caCustomAttributes";
                //        Int32? rotationID = null;
                //        if (!(e.Item is GridEditFormInsertItem))
                //        {
                //            rotationID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationID"]);
                //        }
                //        Presenter.GetCustomAttributeList(rotationID);
                //        if (!CurrentViewContext.GetCustomAttributeList.IsNullOrEmpty())
                //        {
                //            GenerateCustomAttributes(caCustomAttributes);
                //            Panel pnlEditForm = editform.FindControl("pnlEditForm") as Panel;
                //            pnlEditForm.Controls.Add(caCustomAttributes);
                //        }

                //        // Get Granular Permissions, only for Client Admin
                //        if (!Presenter.IsAdminLoggedIn())
                //        {
                //            Presenter.GetGranularPermissions();
                //            ApplyGranularPermissions(editform);
                //        }
                //    }
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

        #endregion

        #region Button Events

        /// <summary>
        /// Assign students to rotations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarAssign_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedTenantID == 0)
                {
                    base.ShowInfoMessage("Please select an institute to assign applicant(s) to rotation(s).");
                    return;
                }
                else if (!CurrentViewContext.AssignRotationIds.Any())
                {
                    base.ShowInfoMessage("Please select at least one rotation to assign applicant(s).");
                    return;
                }
                else
                {
                    String message = String.Empty;
                    String messageType = String.Empty;
                    List<ClinicalRotationMembersContract> lstClinicalRotationDetailContract = Presenter.IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(String.Join("," , CurrentViewContext.AssignRotationIds), CurrentViewContext.SelectedTenantID, String.Join(", ", CurrentViewContext.ApplicantUserIds), string.Empty);
                    if (lstClinicalRotationDetailContract.IsNullOrEmpty())
                    {
                        //AddApplicantToRotation(ref message, ref messageType);

                        List<ClinicalRotationDetailContract> lstClinicalRotationApplicants = Presenter.GetApplicantDetailsForSelectedRotations(String.Join(",", CurrentViewContext.AssignRotationIds), CurrentViewContext.SelectedTenantID);

                        HtmlGenericControl divApplLimit = new HtmlGenericControl("div");
                        divApplLimit.Style.Add("float", "left");
                        HtmlGenericControl ulAppLimit = new HtmlGenericControl("ul");
                        int msgsCount = 0;
                        if (lstClinicalRotationApplicants.Any(x => Convert.ToInt32(x.Students) > 0))
                        {
                            lstClinicalRotationApplicants.ForEach(x =>
                            {
                                HtmlGenericControl li = new HtmlGenericControl("li");                                
                                if ((x.ApplicantCount + CurrentViewContext.ApplicantUserIds.Count) > Convert.ToInt32(x.Students) && (x.Students > 0))
                                {
                                    li.InnerText = "You cannot add applicants to Rotation with Complio ID " + x.ComplioID + " more than the specified limit of " + x.Students + ".";
                                    li.Style["list-style"] = "disc";
                                    ulAppLimit.Controls.Add(li);
                                    msgsCount++;
                                }
                            });
                            ulAppLimit.Style["padding-left"] = "30px";
                            divApplLimit.Controls.Add(ulAppLimit);
                            pnlRotationApplicantLimit.Controls.Add(divApplLimit);
                            if (msgsCount > 0)
                            {
                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowRotationApplicantLimitViolationNotification();", true);
                                return;
                            }
                            else 
                            {
                                AddApplicantToRotation(ref message, ref messageType);
                            }
                        }
                        else
                        {
                            AddApplicantToRotation(ref message, ref messageType);
                        }
                    }
                    else
                    {
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Style.Add("float", "left");
                        HtmlGenericControl ul = new HtmlGenericControl("ul");
                        if (lstClinicalRotationDetailContract.Any(x => x.IsApplicant == false))
                        {
                            lstClinicalRotationDetailContract.ForEach(x =>
                            {
                                HtmlGenericControl li = new HtmlGenericControl("li");
                                if (x.IsApplicant == false)
                                {
                                    li.InnerText = "Rotation with Complio ID " + x.ComplioID + " already has " + x.UserName + " as Instructor/Preceptor.";
                                    li.Style["list-style"] = "disc";
                                    ul.Controls.Add(li);
                                }
                                //else
                                //{
                                //    li.InnerText = "Rotation with Complio ID " + x.ComplioID + " already has " + x.UserName + " as an Applicant ";
                                //    li.Style["list-style"] = "disc";
                                //    ul.Controls.Add(li);
                                //}
                            });
                            ul.Style["padding-left"] = "30px";
                            div.Controls.Add(ul);
                            pnlExistingRotationMembers.Controls.Add(div);

                            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowExistingRotationMembers();", true);
                            return;
                        }
                        else 
                        {
                            //AddApplicantToRotation(ref message, ref messageType);

                            List<ClinicalRotationDetailContract> lstClinicalRotationApplicants = Presenter.GetApplicantDetailsForSelectedRotations(String.Join(",", CurrentViewContext.AssignRotationIds), CurrentViewContext.SelectedTenantID);

                            HtmlGenericControl divApplLimit = new HtmlGenericControl("div");
                            divApplLimit.Style.Add("float", "left");
                            HtmlGenericControl ulAppLimit = new HtmlGenericControl("ul");
                            int msgsCount = 0;
                            if (lstClinicalRotationApplicants.Any(x => Convert.ToInt32(x.Students) > 0))
                            {
                                lstClinicalRotationApplicants.ForEach(x =>
                                {
                                    HtmlGenericControl li = new HtmlGenericControl("li");
                                    if ((x.ApplicantCount + CurrentViewContext.ApplicantUserIds.Count) > Convert.ToInt32(x.Students) && (x.Students > 0))
                                    {
                                        li.InnerText = "You cannot add applicants to Rotation with Complio ID " + x.ComplioID + " more than the specified limit of " + x.Students + ".";
                                        li.Style["list-style"] = "disc";
                                        ulAppLimit.Controls.Add(li);
                                        msgsCount++;
                                    }
                                });
                                ulAppLimit.Style["padding-left"] = "30px";
                                divApplLimit.Controls.Add(ulAppLimit);
                                pnlRotationApplicantLimit.Controls.Add(divApplLimit);
                                if (msgsCount > 0)
                                {
                                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowRotationApplicantLimitViolationNotification();", true);
                                    return;
                                }
                                else
                                {
                                    AddApplicantToRotation(ref message, ref messageType);
                                }
                            }
                            else
                            {
                                AddApplicantToRotation(ref message, ref messageType);
                            }
                        }
                    }
                        

                    //if (Presenter.AssignRotations(out message))
                    //    base.ShowSuccessMessage("Rotation(s) assigned successfully.");
                    //else
                    //    base.ShowSuccessMessage("Some error occurred. Please try again.");

                    CurrentViewContext.AssignRotationIds = new List<Int32>();
                    grdRotations.Rebind();
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

        private void AddApplicantToRotation(ref String message, ref String messageType)
        {
            if (Presenter.AssignRotations(out message, out messageType))
            {
                if (String.IsNullOrEmpty(message))
                {
                    base.ShowSuccessMessage("Rotation(s) assigned successfully.");
                }
                else
                {
                    if (messageType == MessageType.SuccessMessage.ToString())
                    {
                        base.ShowSuccessMessage(message);
                    }
                    else if (messageType == MessageType.Information.ToString())
                    {
                        base.ShowInfoMessage(message);
                    }

                }
            }
            else
            {
                if (String.IsNullOrEmpty(message))
                {
                    base.ShowErrorMessage("Some error occurred. Please try again.");
                }
                else
                {
                    if (messageType == MessageType.SuccessMessage.ToString())
                    {
                        base.ShowSuccessMessage(message);
                    }
                    else if (messageType == MessageType.Information.ToString())
                    {
                        base.ShowInfoMessage(message);
                    }
                }
            }
        }

        /// <summary>
        /// Unassign students from rotations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarUnassign_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedTenantID == 0)
                {
                    base.ShowInfoMessage("Please select an institute to unassign applicant(s) from rotation(s).");
                    return;
                }
                else if (!CurrentViewContext.AssignRotationIds.Any())
                {
                    base.ShowInfoMessage("Please select at least one rotation to unassign applicant(s).");
                    return;
                }
                else
                {
                    if (Presenter.UnassignRotations())
                        base.ShowSuccessMessage("Rotation(s) unassigned successfully.");
                    else
                        base.ShowSuccessMessage("Some error occurred. Please try again.");

                    CurrentViewContext.AssignRotationIds = new List<Int32>();
                    grdRotations.Rebind();
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

        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            //Reset session
            //Session[AppConsts.APPLICANT_SEARCH_SESSION_KEY] = null;
            //Dictionary<String, String> queryString = new Dictionary<String, String>();
            //Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
        }

        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            try
            {
                GridEditFormItem editForm = (sender as LinkButton).NamingContainer as GridEditFormItem;
                if (editForm.IsNotNull())
                {
                    WclAsyncUpload fileUpload = editForm.FindControl("uploadControl") as WclAsyncUpload;
                    Label lblUploadFormName = editForm.FindControl("lblUploadFormName") as Label;
                    LinkButton lnkRemove = editForm.FindControl("lnkRemove") as LinkButton;
                    lblUploadFormName.Visible = false;
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

        #region Dropdown and Checkbox events

        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                GridEditFormInsertItem insertItem = (sender as WclComboBox).NamingContainer as GridEditFormInsertItem;
                String selectedValue = (sender as WclComboBox).SelectedValue;

                CurrentViewContext.SelectedTenantIDForAddForm = selectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(selectedValue);
                WclComboBox ddlAgency = insertItem.FindControl("ddlAgency") as WclComboBox;
                BindAgencyForAddForm(ddlAgency);

                WclComboBox cmbDays = insertItem.FindControl("ddlDays") as WclComboBox;
                BindWeekDaysForAddForm(cmbDays);

                WclComboBox cmbContacts = insertItem.FindControl("ddlInstructor") as WclComboBox;
                BindContactssForAddForm(cmbContacts);
                if (CurrentViewContext.SelectedTenantIDForAddForm > AppConsts.NONE)
                {
                    SharedUserCustomAttributeForm caCustomAttributes = (SharedUserCustomAttributeForm)Page.LoadControl("~\\ClinicalRotation\\UserControl\\SharedUserCustomAttributeForm.ascx");
                    caCustomAttributes.ID = "caCustomAttributes";
                    Int32? rotationID = null;
                    Presenter.GetCustomAttributeList(rotationID);
                    if (!CurrentViewContext.GetCustomAttributeList.IsNullOrEmpty())
                    {
                        GenerateCustomAttributes(caCustomAttributes);
                        Panel pnlEditForm = insertItem.FindControl("pnlEditForm") as Panel;
                        pnlEditForm.Controls.Add(caCustomAttributes);
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

        protected void chkSelectItem_CheckedChanged(object sender, EventArgs e)
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
                List<Int32> selectedItems = CurrentViewContext.AssignRotationIds;
                Int32 rotationID = (Int32)dataItem.GetDataKeyValue("RotationID");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectItem")).Checked;

                if (isChecked)
                {
                    if (!selectedItems.Contains(rotationID))
                    {
                        selectedItems.Add(rotationID);
                    }
                }
                else
                {
                    if (selectedItems != null && selectedItems.Contains(rotationID))
                    {
                        selectedItems.Remove(rotationID);
                    }
                }

                CurrentViewContext.AssignRotationIds = selectedItems;
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

        private void GenerateCustomAttributes(SharedUserCustomAttributeForm caCustomAttributes)
        {
            // Generate the control using database, but set the values from the session
            caCustomAttributes.TenantId = CurrentViewContext.SelectedTenantID;
            caCustomAttributes.TypeCode = CustomAttributeUseTypeContext.Hierarchy.GetStringValue();
            caCustomAttributes.DataSourceModeType = DataSourceMode.Ids;
            caCustomAttributes.Title = "Other Details";
            caCustomAttributes.ControlDisplayMode = DisplayMode.Controls;
            caCustomAttributes.CurrentLoggedInUserId = CurrentViewContext.CurrentLoggedInUserId;
            caCustomAttributes.ValidationGroup = "grpFormSubmit";
            caCustomAttributes.IsReadOnly = false;
            caCustomAttributes.lstTypeCustomAttributes = CurrentViewContext.GetCustomAttributeList;
            caCustomAttributes.EnableViewState = false;
        }

        public String UploadSyllabusDocuments(WclAsyncUpload fileUpload)
        {

            String fileName = String.Empty;
            String savedFilePath = String.Empty;

            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];

            if (tempFilePath.IsNullOrEmpty())
            {
                base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                return String.Empty;
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "Tenant(" + CurrentViewContext.SelectedTenantID.ToString() + @")\";

            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);


            var item = fileUpload.UploadedFiles[0];
            fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
            //Save file on temp location
            String newTempFilePath = Path.Combine(tempFilePath, fileName);
            item.SaveAs(newTempFilePath);

            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
            String destFileName = "RotationSyllabus_" + CurrentViewContext.SelectedTenantID.ToString() + "_" + date + Path.GetExtension(newTempFilePath);
            String desFilePath = "Tenant(" + CurrentViewContext.SelectedTenantID.ToString() + @")\" + destFileName;

            savedFilePath = CommonFileManager.SaveDocument(newTempFilePath, desFilePath, FileType.SystemDocumentLocation.GetStringValue());

            return savedFilePath;
        }

        /// <summary>
        /// Method to Bind Agencies
        /// </summary>
        private void BindAgencyForAddForm(RadComboBox ddlAgencyOnAddForm)
        {
            if (CurrentViewContext.lstAgencyForAddForm.IsNullOrEmpty())
            {
                Presenter.GetAllAgencyForAddForm();
            }
            ddlAgencyOnAddForm.DataSource = CurrentViewContext.lstAgencyForAddForm;
            ddlAgencyOnAddForm.DataBind();
            ddlAgencyOnAddForm.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
            if (CurrentViewContext.SelectedAgencyIDForAddForm > AppConsts.NONE)
            {
                ddlAgencyOnAddForm.SelectedValue = CurrentViewContext.SelectedAgencyIDForAddForm.ToString();
            }
        }

        private void BindWeekDaysForAddForm(RadComboBox cmbDays)
        {
            Presenter.GetWeekDaysForAddForm();
            cmbDays.DataSource = CurrentViewContext.WeekDayListForAddForm;
            cmbDays.DataBind();
        }

        private void BindContactssForAddForm(RadComboBox cmbContacts)
        {
            Presenter.GetClientContactsForAddForm();
            cmbContacts.DataSource = CurrentViewContext.ClientContactListForAddForm;
            cmbContacts.DataBind();
        }

        /// <summary>
        /// UAT-1784: Apply the granular permissions for the Nag-Notification settings.
        /// </summary>
        private void ApplyGranularPermissions(GridEditFormItem editform)
        {
            var _managePkgPermissions = CurrentViewContext.dicGranularPermissions.Where(gp => gp.Key == EnumSystemEntity.ASSIGN_ROTATION_PACKAGE.GetStringValue()).FirstOrDefault();

            if (_managePkgPermissions.IsNotNull() && _managePkgPermissions.Key.IsNotNull() && _managePkgPermissions.Value.IsNotNull())
            {
                if (_managePkgPermissions.Value == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue())
                {
                    var _txtDaysBefore = (editform.FindControl("txtDaysBefore") as WclNumericTextBox);
                    var _txtFrequency = (editform.FindControl("txtFrequency") as WclNumericTextBox);

                    if (_txtDaysBefore.IsNotNull() && _txtFrequency.IsNotNull())
                    {
                        _txtDaysBefore.Enabled = false;
                        _txtFrequency.Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the properties from the arguments recieved through querystring.
        /// </summary>
        /// <param name="args"></param>
        private void CaptureQuerystringParameters()
        {
            if (!Request.QueryString["TenantID"].IsNullOrEmpty())
            {
                CurrentViewContext.SelectedTenantID = Convert.ToInt32(Request.QueryString["TenantID"]);
                hdnSelectedTenantId.Value = CurrentViewContext.SelectedTenantID.ToString();
            }
            if (!Request.QueryString["ScreenMode"].IsNullOrEmpty())
            {
                CurrentViewContext.ScreenMode = Request.QueryString["ScreenMode"].ToString();
            }
            //UAT-2052: Multi Agencies C11.
            if (!Request.QueryString["popupHeight"].IsNullOrEmpty())
            {
                int height = 0;
                Int32.TryParse(Request.QueryString["popupHeight"].ToString(), out height);
                if (height > 0)
                {
                    height = height - 220;

                    DivGridRotations.Style.Add("max-height", Convert.ToString(height) + "px");
                }
            }


        }

        private void ShowHideControls()
        {
            if (CurrentViewContext.ScreenMode.ToLower() == "assign")
            {
                fsucCmdBarButton.SubmitButton.Style.Add("display", "none");
                fsucCmdBarButton.SubmitButton.Visible = false;
            }
            else //Unassign
            {
                fsucCmdBarButton.SaveButton.Style.Add("display", "none");
                fsucCmdBarButton.SaveButton.Visible = false;
            }
        }

        #endregion


        protected void btnRelod_Click(object sender, EventArgs e)
        {
            if (hdnNeedToShowRotSaveMsg.Value == "1")
            {
                lblMessage.Visible = true;
                hdnNeedToShowRotSaveMsg.Value = "0";
                lblMessage.ShowMessage("Clinical rotation saved successfully.", MessageType.SuccessMessage);
            }
            else
            {
                lblMessage.Visible = false;
            }

            grdRotations.Rebind();
        }

        protected void grdRotations_Init(object sender, EventArgs e)
        {
            if (grdRotations.clearFilterMethod == null)
            {
                grdRotations.clearFilterMethod = new WclGrid.ClearFilters(ClearViewStatesForFilter);
            }
        }

        private void ClearViewStatesForFilter()
        {
            ViewState["FilterColumns"] = null;
            ViewState["FilterOperators"] = null;
            ViewState["FilterValues"] = null;
            ViewState["FilterTypes"] = null;
        }

    }
}