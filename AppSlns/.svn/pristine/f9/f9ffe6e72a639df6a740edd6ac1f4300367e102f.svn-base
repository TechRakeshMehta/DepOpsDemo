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
    public partial class SearchOpportunities : BaseUserControl, IOpportunitiesView
    {
        #region Variables

        #region Private Variables

        private OpportunitiesPresenter _presenter = new OpportunitiesPresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private Boolean? _isAdminLoggedIn = null;
        private List<String> _lstCodeForColumnConfig = new List<String>();
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        public IOpportunitiesView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public OpportunitiesPresenter Presenter
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

        List<TenantDetailContract> IOpportunitiesView.lstTenants
        {
            get;
            set;

        }

        List<SpecialtyContract> IOpportunitiesView.lstSpecialties
        {

            set
            {
                ddlSpecialty.DataSource = value;
                ddlSpecialty.DataBind();
            }
        }

        List<StudentTypeContract> IOpportunitiesView.lstStudentTypes
        {

            set
            {
                ddlStudentType.DataSource = value;
                ddlStudentType.DataBind();
            }
        }

        List<DepartmentContract> IOpportunitiesView.lstDepartments
        {

            set
            {
                ddlDepartment.DataSource = value;
                ddlDepartment.DataBind();
            }
        }

        List<AgencyLocationDepartmentContract> IOpportunitiesView.lstLocations
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

        List<PlacementMatchingContract> IOpportunitiesView.lstPlacementMatching
        {
            get;
            set;
        }
     
        List<ShiftDetails> IOpportunitiesView.lstShifts
        {
            set
            {
                ddlShift.DataSource = value;
                ddlShift.DataBind();
            }
        }

        List<WeekDayContract> IOpportunitiesView.WeekDayList
        {
            set
            {
                ddlDays.DataSource = value;
                ddlDays.DataBind();
            }
        }

        Boolean IOpportunitiesView.IsAdminLoggedIn
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

        Int32 IOpportunitiesView.TenantID
        {
            get
            {
                if (tenantId == 0)
                {
                    //tenantId = Presenter.GetTenantId();
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

        Int32 IOpportunitiesView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        Int32 IOpportunitiesView.OrganizationUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        //Represnts Selected Institution ID
        Int32 IOpportunitiesView.SelectedTenantID
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

        Int32? IOpportunitiesView.selectedLocationId
        {
            get
            {
                return ddlLocation.SelectedValue.IsNullOrEmpty() ? (Int32?)null : Int32.Parse(ddlLocation.SelectedValue);
            }
        }

        String IOpportunitiesView.selectedStudentTypeIds
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

        Int32? IOpportunitiesView.selectedSpecialtyId
        {
            get
            {

                return ddlSpecialty.SelectedValue.IsNullOrEmpty() ? (Int32?)null : Int32.Parse(ddlSpecialty.SelectedValue);
            }
        }

        Int32? IOpportunitiesView.selectedDepartmentId
        {
            get
            {

                return ddlDepartment.SelectedValue.IsNullOrEmpty() ? (Int32?)null : Int32.Parse(ddlDepartment.SelectedValue);
            }
        }

        DateTime? IOpportunitiesView.StartDate
        {
            get
            {
                return dtStartDate.SelectedDate;
            }
        }

        DateTime? IOpportunitiesView.EndDate
        {

            get
            {
                return dtEndDate.SelectedDate;
            }
        }

        String IOpportunitiesView.Max
        {
            get
            {
                return txtMax.Text.Trim();
            }
        }

        String IOpportunitiesView.selectedShift
        {
            get
            {
                return ddlShift.SelectedValue.IsNullOrEmpty() ? String.Empty : ddlShift.SelectedItem.Text;
            }
        }

        String IOpportunitiesView.Days
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

        Int32 IOpportunitiesView.SelectedAgencyRootNodeID
        {
            get
            {
                if (!ViewState["SelectedAgencyRootNodeID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["SelectedAgencyRootNodeID"]);
                return AppConsts.NONE;
            }

            set
            {
                ViewState["SelectedAgencyRootNodeID"] = value;
            }
        }

        List<CustomAttribteContract> IOpportunitiesView.lstSharedCustomAttribute
        {
            get
            {
                if (!ViewState["lstSharedCustomAttribute"].IsNullOrEmpty())
                    return (List<CustomAttribteContract>)ViewState["lstSharedCustomAttribute"];
                return new List<CustomAttribteContract>();
            }
            set
            {
                ViewState["lstSharedCustomAttribute"] = value;
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

        #endregion

        #endregion

        #region Events
        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Search Opportunities";
                base.SetPageTitle("Search Opportunities");
                _lstCodeForColumnConfig.Add(Screen.grdSearchOpportunities.GetStringValue());
                ColumnsConfiguration.CurrentViewContext.CurrentLoggedInUserID = CurrentViewContext.CurrentLoggedInUserId;
                ColumnsConfiguration.CurrentViewContext.OrganisationUserID = CurrentViewContext.OrganizationUserId;
                ColumnsConfiguration.CurrentViewContext.lstGridCode = _lstCodeForColumnConfig;
                List<PreHiddenColumnsContract> lstpreHiddenColumnsContract = new List<PreHiddenColumnsContract>();
                //lstpreHiddenColumnsContract.Add(new PreHiddenColumnsContract { GridCode = Screen.grdAdminManageRotations.GetStringValue(), PredefinedHiddenColumn = "CustomAttributesTemp" });
                //grdRotations.Attributes["PreHiddenColumns"] = "CustomAttributesTemp";
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

                if (!ddlLocation.SelectedValue.IsNullOrEmpty())
                {
                    Int32 selectedLocationID = Convert.ToInt32(ddlLocation.SelectedValue);

                    if (!CurrentViewContext.lstLocations.IsNullOrEmpty())
                        CurrentViewContext.SelectedAgencyRootNodeID = CurrentViewContext.lstLocations.Where(cond => cond.AgencyLocationID == selectedLocationID).FirstOrDefault().AgencyHierarchyID;
                }
                else
                {
                    CurrentViewContext.SelectedAgencyRootNodeID = AppConsts.NONE;
                }

                if (!CurrentViewContext.SelectedAgencyRootNodeID.IsNullOrEmpty() && CurrentViewContext.SelectedAgencyRootNodeID > AppConsts.NONE)
                {
                    caCustomAttributesID.IsSearchTypeControl = true;
                    caCustomAttributesID.TenantId = AppConsts.NONE;
                    caCustomAttributesID.TypeCode = SharedCustomAttributeUseType.ClinicalInventory.GetStringValue();
                    caCustomAttributesID.DataSourceModeType = DataSourceMode.Ids;
                    caCustomAttributesID.Title = "Other Details";
                    caCustomAttributesID.ControlDisplayMode = DisplayMode.Controls;
                    caCustomAttributesID.CurrentLoggedInUserId = base.CurrentUserId;
                    caCustomAttributesID.ValidationGroup = "grpFormSubmitSearchType";
                    caCustomAttributesID.IsReadOnly = false;

                    GetSharedCustomAttributeList();
                    if (!CurrentViewContext.lstSharedCustomAttribute.IsNullOrEmpty())
                        caCustomAttributesID.lstTypeCustomAttributes = CurrentViewContext.lstSharedCustomAttribute;
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

        protected void grdSearchOpportunities_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetOpportunitySearch();
                grdSearchOpportunities.DataSource = ddlTenantName.SelectedIndex > AppConsts.NONE ? CurrentViewContext.lstPlacementMatching : new List<PlacementMatchingContract>();
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

        protected void grdSearchOpportunities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                //   grdSearchOpportunities.DataSource = CurrentViewContext.lstPlacementMaching;
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

        protected void grdSearchOpportunities_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "CreateDraft")
                {
                    Int32 opportunityID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OpportunityID"]);

                    hdnOpportunityId.Value = opportunityID.ToString();
                    hdnPageRequested.Value = RequestDetails.CREATEDRAFT.GetStringValue();
                    hdnSelectedTenantID.Value = ddlTenantName.SelectedValue.ToString();

                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "SearchOpportunityPopUp();", true);
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

        protected void grdSearchOpportunities_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            try
            {
                grdSearchOpportunities.DataSource = CurrentViewContext.lstPlacementMatching;
                ddlTenantName.Enabled = false;
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
                if (ddlTenantName.SelectedValue.IsNullOrEmpty())
                {
                    ResetTenant();
                    //  ucAgencyHierarchyMultiple.Reset();
                    ResetSearchControls();
                    ResetGridFilters();
                }
                grdSearchOpportunities.Rebind();
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
                        grdSearchOpportunities.Rebind();
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
                        grdSearchOpportunities.Rebind();
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
                grdSearchOpportunities.Rebind();
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
              //  ucAgencyHierarchyMultiple.Reset();
                ResetSearchControls();
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

        #region Methods

        #region Private Methods

        private void BindTenant()
        {
            ddlTenantName.DataSource = CurrentViewContext.lstTenants;
            ddlTenantName.DataBind();
            Presenter.IsAdminLoggedIn();
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                ddlTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantID = 0;
            }
            else
            {
                CurrentViewContext.SelectedTenantID = CurrentViewContext.SelectedTenantID;
            }
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

        /// <summary>
        /// Method to reset Search controls
        /// </summary>
        private void ResetSearchControls()
        {
            ddlLocation.ClearSelection();
            ddlDepartment.ClearSelection();
            ddlSpecialty.ClearSelection();
            txtMax.Text = String.Empty;
            ddlShift.ClearSelection();
            ddlStudentType.ClearCheckedItems();
            ddlDays.ClearCheckedItems();
            dtStartDate.Clear();
            dtEndDate.Clear();
            caCustomAttributesID.ResetCustomAttributes();
            caCustomAttributesID.Visible = false;

            hdnOpportunityId.Value = string.Empty;
            hdnPageRequested.Value = string.Empty;
        }

        /// <summary>
        /// Method to Reset Grid 
        /// </summary>
        private void ResetGridFilters()
        {
            grdSearchOpportunities.MasterTableView.SortExpressions.Clear();
            grdSearchOpportunities.CurrentPageIndex = 0;
            grdSearchOpportunities.MasterTableView.CurrentPageIndex = 0;
            grdSearchOpportunities.MasterTableView.IsItemInserted = false;
            grdSearchOpportunities.MasterTableView.ClearEditItems();
            grdSearchOpportunities.Rebind();
        }

        private void BindLocations()
        {
            Presenter.GetAllLocations();
            ddlLocation.DataSource = CurrentViewContext.lstLocations;
            ddlLocation.DataBind();
        }

        private void GetSharedCustomAttributeList()
        {
            Presenter.GetSharedCustomAttributeList();
        }

        #endregion

        #region Public Methods

        #endregion

        #endregion

        #endregion

    }

}