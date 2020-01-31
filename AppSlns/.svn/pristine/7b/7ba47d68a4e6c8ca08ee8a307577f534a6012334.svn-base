
using CoreWeb.ClinicalRotation.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.CommonControls;
using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class SearchRequest : BaseUserControl, ISearchRequestView
    {

        #region Variables

        #region Private Variables

        private SearchRequestPresenter _presenter = new SearchRequestPresenter();
        private Int32 tenantId;
        private Boolean? _isAdminLoggedIn = null;
        private List<String> _lstCodeForColumnConfig = new List<String>();
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties
        List<CustomAttribteContract> ISearchRequestView.CustomAttributeList
        {
            get;
            set;
        }
        List<SpecialtyContract> ISearchRequestView.lstSpecialties
        {

            set
            {
                ddlSpecialty.DataSource = value;
                ddlSpecialty.DataBind();
            }
        }
        List<StudentTypeContract> ISearchRequestView.lstStudentTypes
        {

            set
            {
                ddlStudentType.DataSource = value;
                ddlStudentType.DataBind();
            }
        }
        List<DepartmentContract> ISearchRequestView.lstDepartments
        {

            set
            {
                ddlDepartment.DataSource = value;
                ddlDepartment.DataBind();
            }
        }
        List<AgencyLocationDepartmentContract> ISearchRequestView.lstLocations
        {
            get
            {
                if (!ViewState["lstLocations"].IsNullOrEmpty())
                    return (List<AgencyLocationDepartmentContract>)ViewState["lstLocations"];
                return new List<AgencyLocationDepartmentContract>();
            }

            set
            {
                ViewState["lstLocations"] = value;
            }
        }
        List<ShiftDetails> ISearchRequestView.lstShifts
        {
            set
            {
                ddlShift.DataSource = value;
                ddlShift.DataBind();
            }
        }

        List<RequestStatusContract> ISearchRequestView.lstRequestStatuses
        {
            set
            {
                ddlRequestStatus.DataSource = value;
                ddlRequestStatus.DataBind();
            }
        }
        List<RequestDetailContract> ISearchRequestView.lstPlacementMaching
        {
            get;
            set;

        }

        List<TenantDetailContract> ISearchRequestView.lstTenants
        {
            get;
            set;

        }

        List<WeekDayContract> ISearchRequestView.WeekDayList
        {
            set
            {
                ddlDays.DataSource = value;
                ddlDays.DataBind();
            }
        }

        Boolean ISearchRequestView.IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.HasValue ? _isAdminLoggedIn.Value : true;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }

        Int32 ISearchRequestView.TenantID
        {
            get
            {
                if (tenantId == 0)
                {
                    //tenantId = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        tenantId = user.TenantId.HasValue ? user.TenantId.Value : AppConsts.NONE;
                    }
                }
                return tenantId;

            }
            set { tenantId = value; }
        }

        //Represnts Selected Institution ID

        Int32 ISearchRequestView.SelectedTenantID
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
                    ddlTenantName.SelectedValue = Convert.ToString(value);
                }
                else
                {
                    ddlTenantName.SelectedIndex = value;
                }
            }
        }

        Int32? ISearchRequestView.selectedLocationId
        {
            get
            {
                return ddlLocation.SelectedValue.IsNullOrEmpty() ? (Int32?)null : Int32.Parse(ddlLocation.SelectedValue);
            }
        }
        Int32 ISearchRequestView.CurrentAgencyRootID
        {
            get
            {
                if (!ViewState["CurrentAgencyRootID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["CurrentAgencyRootID"]);
                }

                return AppConsts.NONE;
            }
            set
            {
                ViewState["CurrentAgencyRootID"] = value;
            }
        }
        String ISearchRequestView.selectedStudentTypeIds
        {
            get
            {
                List<Int32> selectedStudentTypes = new List<Int32>();
                foreach (RadComboBoxItem slctdItem in ddlStudentType.CheckedItems)
                {
                    selectedStudentTypes.Add(Convert.ToInt32(slctdItem.Value));
                }
                return String.Join(",", selectedStudentTypes);
            }
        }
        Int32? ISearchRequestView.selectedSpecialtyId
        {
            get
            {

                return ddlSpecialty.SelectedValue.IsNullOrEmpty() ? (Int32?)null : Int32.Parse(ddlSpecialty.SelectedValue);
            }
        }
        String ISearchRequestView.selectedStatusIds
        {
            get
            {

                return ddlRequestStatus.SelectedValue.IsNullOrEmpty() ? String.Empty : ddlRequestStatus.SelectedValue;
            }
        }

        Int32? ISearchRequestView.selectedDepartmentId
        {
            get
            {

                return ddlDepartment.SelectedValue.IsNullOrEmpty() ? (Int32?)null : Int32.Parse(ddlDepartment.SelectedValue);
            }
        }

        DateTime? ISearchRequestView.StartDate
        {
            get
            {
                return dtStartDate.SelectedDate;
            }
        }
        DateTime? ISearchRequestView.EndDate
        {

            get
            {
                return dtEndDate.SelectedDate;
            }
        }
        String ISearchRequestView.Max
        {
            get
            {
                return txtMax.Text.Trim();
            }
        }

        String ISearchRequestView.selectedShift
        {
            get
            {
                return ddlShift.SelectedValue.IsNullOrEmpty() ? String.Empty : ddlShift.SelectedItem.Text;
            }
        }
        String ISearchRequestView.Days
        {
            get
            {
                List<Int32> selectedDays = new List<Int32>();
                foreach (RadComboBoxItem slctdItem in ddlDays.CheckedItems)
                {
                    selectedDays.Add(Convert.ToInt32(slctdItem.Value));
                }
                return String.Join(",", selectedDays);
            }
        }
        #endregion

        #region Public Properties

        public SearchRequestPresenter Presenter
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

        public ISearchRequestView CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        public String CustomDataXML
        {
            get
            {
                if (caCustomAttributesID.IsNotNull())
                    return caCustomAttributesID.GetCustomDataXML();
                else
                    return null;
            }
        }
        public Int32 CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public String departmentIds
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
                base.Title = "Search Requests";
                base.SetPageTitle("Search Requests");
                _lstCodeForColumnConfig.Add(Screen.grdPlacementMatchingMapping.GetStringValue());
                ColumnsConfiguration.CurrentViewContext.CurrentLoggedInUserID = CurrentViewContext.CurrentLoggedInUserID;
                ColumnsConfiguration.CurrentViewContext.OrganisationUserID = SysXWebSiteUtils.SessionService.OrganizationUserId; ;
                ColumnsConfiguration.CurrentViewContext.lstGridCode = _lstCodeForColumnConfig;
                List<PreHiddenColumnsContract> lstpreHiddenColumnsContract = new List<PreHiddenColumnsContract>();
                //lstpreHiddenColumnsContract.Add(new PreHiddenColumnsContract { GridCode = Screen.grdPlacementMatchingMapping.GetStringValue(), PredefinedHiddenColumn = "" });
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
                    Presenter.OnViewInitialized();
                    BindTenant();
                    BindLocations();

                }
                if (!ddlLocation.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(ddlLocation.SelectedValue) > AppConsts.NONE)
                {
                    CurrentViewContext.CurrentAgencyRootID = CurrentViewContext.lstLocations.Where(cond => cond.AgencyLocationID == Convert.ToInt32(ddlLocation.SelectedValue)).FirstOrDefault().AgencyHierarchyID;
                    caCustomAttributesID.IsSearchTypeControl = true;
                    caCustomAttributesID.TenantId = CurrentViewContext.SelectedTenantID;
                    caCustomAttributesID.TypeCode = SharedCustomAttributeUseType.ClinicalInventoryRequest.GetStringValue();
                    caCustomAttributesID.DataSourceModeType = DataSourceMode.Ids;
                    caCustomAttributesID.Title = "Other Details";
                    caCustomAttributesID.ControlDisplayMode = DisplayMode.Controls;
                    caCustomAttributesID.CurrentLoggedInUserId = base.CurrentUserId;
                    caCustomAttributesID.ValidationGroup = "grpFormSubmit";
                    caCustomAttributesID.IsReadOnly = false;
                    Presenter.GetSharedCustomAttributeList();
                    caCustomAttributesID.lstTypeCustomAttributes = CurrentViewContext.CustomAttributeList;
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

        protected void btnDoPostback_Click(object sender, EventArgs e)
        {
            try
            {
                if (!hdnRequestSavedSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnRequestSavedSuccessfully.Value.ToLower()))
                    {
                        base.ShowSuccessMessage("Request saved successfully.");
                        grdPlacementMatchingMapping.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                    }
                }
                if (!hdnRequestPublishedSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnRequestPublishedSuccessfully.Value.ToLower()))
                    {
                        base.ShowSuccessMessage("Request published successfully.");
                        grdPlacementMatchingMapping.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
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

        protected void fsucCmdBarButton_SearchClick(object sender, EventArgs e)
        {
            try
            {
                grdPlacementMatchingMapping.Rebind();
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
                ResetSearchFilter();
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

        #endregion

        #region Grid Events

        protected void grdPlacementMatchingMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetSearchRequestData();
                grdPlacementMatchingMapping.DataSource = ddlTenantName.SelectedIndex > AppConsts.NONE ? CurrentViewContext.lstPlacementMaching : new List<RequestDetailContract>();
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

        protected void grdPlacementMatchingMapping_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Details")
                {
                    Int32 requestID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequestId"]);
                    Int32 opportunityId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OpportunityID"]);
                    hdnRequestId.Value = requestID.ToString();
                    hdnOpportuityID.Value = opportunityId.ToString();
                    hdnPageRequested.Value = RequestDetails.REQUESTDETAILS.GetStringValue();
                    hdnSelectedTenantID.Value = ddlTenantName.SelectedValue.ToString();

                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "SearchRequestPopUp();", true);
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

        protected void grdPlacementMatchingMapping_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
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

        #region Dropdown Events

        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlTenantName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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
            try
            {
                grdPlacementMatchingMapping.Rebind();

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

        #region Method

        #region Private Method

        private void BindTenant()
        {
            ddlTenantName.DataSource = CurrentViewContext.lstTenants;
            ddlTenantName.DataBind();
            Presenter.IsAdminLoggedIn();
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                ddlTenantName.Enabled = true;
                //rfvTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantID = 0;
            }
            else
            {
                CurrentViewContext.SelectedTenantID = CurrentViewContext.SelectedTenantID;
            }
        }
        public void BindLocations()
        {
            Presenter.GetAllLocations();
            ddlLocation.DataSource = CurrentViewContext.lstLocations;
            ddlLocation.DataBind();
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
                CurrentViewContext.SelectedTenantID = CurrentViewContext.SelectedTenantID;
            }
        }

        private void ResetSearchFilter()
        {
            //ddlInstitution.ClearSelection();
            ddlDepartment.ClearSelection();
            ddlLocation.ClearSelection();
            dtStartDate.Clear();
            ddlSpecialty.ClearSelection();
            ddlStudentType.ClearCheckedItems();
            txtMax.Text = String.Empty;
            dtEndDate.Clear();
            ddlDays.ClearCheckedItems();
            ddlShift.ClearSelection();
            ddlRequestStatus.ClearSelection();
            //ddlGroups.ClearSelection();

            //ucAgencyHierarchyMultiple.Reset();
            //ucAgencyHierarchyMultiple.CurrentOrgUserId = CurrentViewContext.CurrentLoggedInUserID;
            //ucAgencyHierarchyMultiple.AgencyHierarchyNodeSelection = true;
            //ucAgencyHierarchyMultiple.TenantId = (CurrentViewContext.lstTenants.FirstOrDefault().IsNullOrEmpty()) ? 0 : Convert.ToInt32(CurrentViewContext.lstTenants.FirstOrDefault().TenantID);
        }

        /// <summary>
        /// Method to Reset Grid 
        /// </summary>
        private void ResetGridFilters()
        {
            grdPlacementMatchingMapping.MasterTableView.SortExpressions.Clear();
            grdPlacementMatchingMapping.CurrentPageIndex = 0;
            grdPlacementMatchingMapping.MasterTableView.CurrentPageIndex = 0;
            grdPlacementMatchingMapping.MasterTableView.IsItemInserted = false;
            grdPlacementMatchingMapping.MasterTableView.ClearEditItems();
            grdPlacementMatchingMapping.Rebind();
        }

        #endregion

        protected void fsucColConfigCmdBarButton_CancelClick(object sender, EventArgs e)
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

        protected void fsucColConfigCmdBarButton_SaveClick(object sender, EventArgs e)
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

        private void ManageColumnConfiguration()
        {
            String grdCode = String.Empty;

            _lstCodeForColumnConfig.Add(Screen.grdPlacementMatchingMapping.GetStringValue());
            grdCode = INTSOF.Utils.Screen.grdPlacementMatchingMapping.GetStringValue();

            if (!grdCode.IsNullOrEmpty())
            {
                grdPlacementMatchingMapping.Attributes["GridCode"] = grdCode;
                ColumnsConfiguration.CurrentViewContext.CurrentLoggedInUserID = CurrentViewContext.CurrentLoggedInUserID;
                ColumnsConfiguration.CurrentViewContext.OrganisationUserID = CurrentViewContext.CurrentLoggedInUserID;
                ColumnsConfiguration.CurrentViewContext.lstGridCode = _lstCodeForColumnConfig;

                List<PreHiddenColumnsContract> lstpreHiddenColumnsContract = new List<PreHiddenColumnsContract>();
                lstpreHiddenColumnsContract.Add(new PreHiddenColumnsContract { GridCode = "AAAJ", PredefinedHiddenColumn = "CustomAttributesTemp" });
                lstpreHiddenColumnsContract.Add(new PreHiddenColumnsContract { GridCode = "AAAJ", PredefinedHiddenColumn = "NotesTemp" });
                ColumnsConfiguration.CurrentViewContext.lstPredefinedHIddenColumns = lstpreHiddenColumnsContract;
                grdPlacementMatchingMapping.Attributes["PreHiddenColumns"] = "CustomAttributesTemp,NotesTemp";

                List<String> lstColumnnsToHide = Presenter.GetScreenColumnsToHide(grdCode, CurrentViewContext.CurrentLoggedInUserID);
                if (!lstColumnnsToHide.IsNullOrEmpty())
                {
                    if (lstColumnnsToHide.Contains("CustomAttributes") && lstColumnnsToHide.Contains("Notes"))
                    {
                        grdPlacementMatchingMapping.Attributes["ExportingColumnsNotInGrid"] = String.Empty;
                    }
                    else if (lstColumnnsToHide.Contains("Notes") && !lstColumnnsToHide.Contains("CustomAttributes"))
                    {
                        grdPlacementMatchingMapping.Attributes["ExportingColumnsNotInGrid"] = "CustomAttributesTemp";
                    }
                    else if (lstColumnnsToHide.Contains("CustomAttributes") && !lstColumnnsToHide.Contains("Notes"))
                    {
                        grdPlacementMatchingMapping.Attributes["ExportingColumnsNotInGrid"] = "NotesTemp";
                    }
                    else
                    {
                        grdPlacementMatchingMapping.Attributes["ExportingColumnsNotInGrid"] = "CustomAttributesTemp,NotesTemp";
                    }
                }
                else
                {
                    grdPlacementMatchingMapping.Attributes["ExportingColumnsNotInGrid"] = "CustomAttributesTemp,NotesTemp";
                }
            }
        }
        #region Public Method

        #endregion

        #endregion

        protected void ddlLocation_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            
        }
    }
}