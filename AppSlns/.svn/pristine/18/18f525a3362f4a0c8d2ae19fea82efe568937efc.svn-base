using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Modules.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using CoreWeb.IntsofSecurityModel;
using INTSOF.UI.Contract.PlacementMatching;
using Telerik.Web.UI;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Core;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class ManageRequest : BaseUserControl, IManageRequestView
    {
        #region Variables

        #region Private Variables
        private ManageRequestPresenter _presenter = new ManageRequestPresenter();
        private DateTime _defaultDateTime = Convert.ToDateTime("01/01/1900");
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        public ManageRequestPresenter Presenter
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

        public IManageRequestView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public String UserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.UserId;
            }
        }


        List<TenantDetailContract> IManageRequestView.lstTenants
        {
            get
            {
                if (!ViewState["lstTenants"].IsNullOrEmpty())
                {
                    return (List<TenantDetailContract>)ViewState["lstTenants"];
                }
                return new List<TenantDetailContract>();
            }
            set
            {
                ViewState["lstTenants"] = value;
                //ddlTenantName.DataSource = value;
                //ddlTenantName.DataBind();
            }
        }

        List<WeekDayContract> IManageRequestView.WeekDayList
        {
            set
            {
                ddlDays.DataSource = value;
                ddlDays.DataBind();
            }
        }

        List<String> IManageRequestView.SharedUserTypeCodes
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

        List<RequestDetailContract> IManageRequestView.lstRequests
        {
            get
            {
                if (!ViewState["lstRequests"].IsNullOrEmpty())
                {
                    return (List<RequestDetailContract>)ViewState["lstRequests"];
                }
                return new List<RequestDetailContract>();
            }
            set
            {
                ViewState["lstRequests"] = value;
            }
        }

        RequestDetailContract IManageRequestView.SearchRequestContract
        {
            get
            {
                if (!ViewState["SearchRequestContract"].IsNullOrEmpty())
                {
                    return (RequestDetailContract)ViewState["SearchRequestContract"];
                }
                return new RequestDetailContract();
            }
            set
            {
                ViewState["SearchRequestContract"] = value;
            }
        }

        Boolean IManageRequestView.IsAdvanceSearchPanelDisplay
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

        List<lkpInventoryAvailabilityType> IManageRequestView.lstInstitutionAvailability
        {
            get
            {
                if (!ViewState["lstInstitutionAvailability"].IsNullOrEmpty())
                    return (List<lkpInventoryAvailabilityType>)(ViewState["lstInstitutionAvailability"]);
                return new List<lkpInventoryAvailabilityType>();
            }
            set
            {
                ViewState["lstInstitutionAvailability"] = value;
            }
        }

        List<AgencyLocationDepartmentContract> IManageRequestView.lstLocations
        {
            get
            {
                if (!ViewState["lstLocations"].IsNullOrEmpty())
                    return (List<AgencyLocationDepartmentContract>)(ViewState["lstLocations"]);
                return new List<AgencyLocationDepartmentContract>();
            }
            set
            {
                ViewState["lstLocations"] = value;
            }
        }

        Guid IManageRequestView.UserId
        {
            get
            {
                return base.SysXMembershipUser.UserId;
            }
        }

        Int32 IManageRequestView.AgencyRootNodeID
        {
            get
            {
                if (!ViewState["AgencyRootNodeID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["AgencyRootNodeID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["AgencyRootNodeID"] = value;
            }
        }

        List<DepartmentContract> IManageRequestView.lstDepartment
        {
            get
            {
                if (!ViewState["lstDepartment"].IsNullOrEmpty())
                {
                    return ViewState["lstDepartment"] as List<DepartmentContract>;
                }
                return new List<DepartmentContract>();
            }
            set
            {
                ViewState["lstDepartment"] = value;
            }
        }

        List<StudentTypeContract> IManageRequestView.lstStudentType
        {
            get
            {
                if (!ViewState["lstStudentType"].IsNullOrEmpty())
                {
                    return ViewState["lstStudentType"] as List<StudentTypeContract>;
                }
                return new List<StudentTypeContract>();
            }
            set
            {
                ViewState["lstStudentType"] = value;
            }
        }

        List<SpecialtyContract> IManageRequestView.lstSpecialties
        {
            get
            {
                if (!ViewState["lstSpecialties"].IsNullOrEmpty())
                {
                    return ViewState["lstSpecialties"] as List<SpecialtyContract>;
                }
                return new List<SpecialtyContract>();
            }
            set
            {
                ViewState["lstSpecialties"] = value;
            }
        }


        List<RequestStatusContract> IManageRequestView.lstRequestStatus
        {
            get
            {
                if (!ViewState["lstRequestStatus"].IsNullOrEmpty())
                {
                    return ViewState["lstRequestStatus"] as List<RequestStatusContract>;
                }
                return new List<RequestStatusContract>();
            }
            set
            {
                ViewState["lstRequestStatus"] = value;
            }
        }

        List<CustomAttribteContract> IManageRequestView.lstSharedCustomAttribute
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
                base.Title = "Manage Request";
                base.SetPageTitle("Manage Request");
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
                    BindInstitutionAvailability();
                    BindLocationsAndDepartments();
                    BindStudentType();
                    BindSpecialty();
                    BindRequestStatus();
                }
                SetAdvanceSearchPanel();
                if (!CurrentViewContext.AgencyRootNodeID.IsNullOrEmpty() && CurrentViewContext.AgencyRootNodeID > AppConsts.NONE)
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

        protected void grdRequest_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                BindSearchContract();
                Presenter.GetRequests();
                grdRequest.DataSource = CurrentViewContext.lstRequests;
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

        protected void grdRequest_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Status")
                {
                    //Redirect to request Details Screen.
                    String requestStatusCode = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StatusCode"]);
                    Int32 requestID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RequestID"]);
                    Int32 opportunityID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OpportunityID"]);
                    hdnRequestId.Value = requestID.ToString();
                    hdnPageRequested.Value = RequestDetails.REQUESTDETAILS.GetStringValue();
                    hdnRequestStatusCode.Value = requestStatusCode;
                    hdnOpportunityID.Value = opportunityID.ToString();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RequestDetailsPop();", true);
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

        protected void grdRequest_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    RadButton btnStatus = ((RadButton)e.Item.FindControl("btnStatus"));
                    String requestStatus = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Status"].IsNullOrEmpty() ? String.Empty : (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Status"].ToString();
                    String requestStatusCode = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StatusCode"].IsNullOrEmpty() ? String.Empty : (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StatusCode"].ToString();

                    if (requestStatusCode == RequestStatusCodes.Approved.GetStringValue())
                    {
                        btnStatus.ForeColor = System.Drawing.Color.LightGreen;
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

        #region Button Events

        protected void fsucCmdBarButton_ResetClick(object sender, EventArgs e)
        {
            try
            {
                ResetTenant();
                ResetSearchFilters();
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

        protected void fsucCmdBarButton_SearchClick(object sender, EventArgs e)
        {
            try
            {
                grdRequest.Rebind();
                SetAdvanceSearchPanel();
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
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", ChildControls.SharedUserDashboard}
                                                                 };
                Response.Redirect(String.Format("~/ProfileSharing/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
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

        protected void btnDoPostback_Click(object sender, EventArgs e)
        {
            try
            {
                if (!hdnRequestSavedSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnRequestSavedSuccessfully.Value.ToLower()))
                    {
                        base.ShowSuccessMessage("Request saved successfully.");
                        grdRequest.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                    }
                }
                if (!hdnRequestCancelledSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnRequestCancelledSuccessfully.Value.ToLower()))
                    {
                        base.ShowSuccessMessage("Request cancelled successfully.");
                        grdRequest.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                    }
                }
                if (!hdnRequestRejectedSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnRequestRejectedSuccessfully.Value.ToLower()))
                    {
                        base.ShowSuccessMessage("Request rejected successfully.");
                        grdRequest.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                    }
                }
                if (!hdnRequestArchivedSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnRequestArchivedSuccessfully.Value.ToLower()))
                    {
                        base.ShowSuccessMessage("Request archived successfully.");
                        grdRequest.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                    }
                }
                if (!hdnRequestApprovedSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnRequestApprovedSuccessfully.Value.ToLower()))
                    {
                        base.ShowSuccessMessage("Request approved successfully.");
                        grdRequest.Rebind();
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
                        grdRequest.Rebind();
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

        #endregion

        #region CheckBox Events

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// Method to Reset Grid 
        /// </summary>
        private void ResetGridFilters()
        {
            grdRequest.MasterTableView.SortExpressions.Clear();
            grdRequest.CurrentPageIndex = 0;
            grdRequest.MasterTableView.CurrentPageIndex = 0;
            grdRequest.MasterTableView.IsItemInserted = false;
            grdRequest.MasterTableView.ClearEditItems();
            grdRequest.Rebind();
        }

        /// <summary>
        /// Method to reset search filters
        /// </summary>
        private void ResetSearchFilters()
        {
            ddlLocation.ClearSelection();
            ddlDepartment.ClearSelection();
            dpStartDate.Clear();
            dpEndDate.Clear();
            ddlSpecialty.ClearSelection();
            ddlStudentType.ClearCheckedItems();
            txtMaxNoOfStudent.Text = String.Empty;
            ddlDays.ClearCheckedItems();
            txtShift.Text = String.Empty;
            ddlStatus.ClearSelection();
            caCustomAttributesID.ResetCustomAttributes();
            //ucAgencyHierarchyMultiple.Reset();
            //ucAgencyHierarchyMultiple.CurrentOrgUserId = CurrentViewContext.CurrentLoggedInUserID;
            //ucAgencyHierarchyMultiple.AgencyHierarchyNodeSelection = true;
            //ucAgencyHierarchyMultiple.TenantId = (CurrentViewContext.lstTenants.FirstOrDefault().IsNullOrEmpty()) ? 0 : Convert.ToInt32(CurrentViewContext.lstTenants.FirstOrDefault().TenantID);
        }

        /// <summary>
        /// Method to Reset the tenant(s) checked in tenant dropdown on reset click.
        /// </summary>
        private void ResetTenant()
        {
            //ddlTenantName.SelectedValue = String.Empty;
            //ddlTenantName.Items.ForEach(itm => itm.Checked = false);
            rblInstitutionAvailability.SelectedValue = InstitutionAvailabilityType.AssociatedInstitution.GetStringValue(); // Associated Institutions
        }

        /// <summary>
        /// Method to bind search contract.
        /// </summary>
        private void BindSearchContract()
        {
            CurrentViewContext.SearchRequestContract = new RequestDetailContract();
            //CurrentViewContext.SearchRequestContract.SelectedTenantIds = ddlTenantName.CheckedItems.IsNullOrEmpty() ? String.Empty : String.Join(",", ddlTenantName.CheckedItems.Select(x => x.Value));
            CurrentViewContext.SearchRequestContract.InventoryAvailabilityTypeCode = String.IsNullOrEmpty(rblInstitutionAvailability.SelectedValue) ? InstitutionAvailabilityType.AssociatedInstitution.GetStringValue() : rblInstitutionAvailability.SelectedValue;
            CurrentViewContext.SearchRequestContract.AgencyLocationID = String.IsNullOrEmpty(ddlLocation.SelectedValue) ? AppConsts.NONE : Convert.ToInt32(ddlLocation.SelectedValue);
            CurrentViewContext.SearchRequestContract.Location = ddlLocation.SelectedItem.IsNullOrEmpty() ? String.Empty : (String.IsNullOrEmpty(ddlLocation.SelectedItem.Text) ? String.Empty : ddlLocation.SelectedItem.Text.Trim());
            CurrentViewContext.SearchRequestContract.DepartmentID = String.IsNullOrEmpty(ddlDepartment.SelectedValue) ? AppConsts.NONE : Convert.ToInt32(ddlDepartment.SelectedValue);
            CurrentViewContext.SearchRequestContract.Department = ddlDepartment.SelectedItem.IsNullOrEmpty() ? String.Empty : String.IsNullOrEmpty(ddlDepartment.SelectedItem.Text) ? String.Empty : ddlDepartment.SelectedItem.Text.Trim();
            CurrentViewContext.SearchRequestContract.StartDate = dpStartDate.SelectedDate.IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(dpStartDate.SelectedDate);
            CurrentViewContext.SearchRequestContract.EndDate = dpEndDate.SelectedDate.IsNullOrEmpty() ? (DateTime?)null : Convert.ToDateTime(dpEndDate.SelectedDate);
            CurrentViewContext.SearchRequestContract.SpecialtyID = ddlSpecialty.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ddlSpecialty.SelectedValue);
            CurrentViewContext.SearchRequestContract.Specialty = ddlSpecialty.SelectedItem.IsNullOrEmpty() ? String.Empty : (ddlSpecialty.SelectedItem.Text.IsNullOrEmpty() ? String.Empty : ddlSpecialty.SelectedItem.Text);
            CurrentViewContext.SearchRequestContract.StudentTypeIds = ddlStudentType.CheckedItems.IsNullOrEmpty() ? String.Empty : String.Join(",", ddlStudentType.CheckedItems.Select(x => x.Value));
            CurrentViewContext.SearchRequestContract.StudentTypes = ddlStudentType.CheckedItems.IsNullOrEmpty() ? String.Empty : String.Join(",", ddlStudentType.CheckedItems.Select(x => x.Text));
            CurrentViewContext.SearchRequestContract.Max = String.IsNullOrEmpty(txtMaxNoOfStudent.Text) ? AppConsts.NONE : Convert.ToInt32(txtMaxNoOfStudent.Text.Trim());
            CurrentViewContext.SearchRequestContract.Days = String.Join(",", ddlDays.CheckedItems.Select(x => x.Text));
            CurrentViewContext.SearchRequestContract.DayIds = String.Join(",", ddlDays.CheckedItems.Select(x => x.Value));
            CurrentViewContext.SearchRequestContract.Shift = String.IsNullOrEmpty(txtShift.Text) ? String.Empty : txtShift.Text.Trim();
            CurrentViewContext.SearchRequestContract.StatusID = String.IsNullOrEmpty(ddlStatus.SelectedValue) ? AppConsts.NONE : Convert.ToInt32(ddlStatus.SelectedValue);
            CurrentViewContext.SearchRequestContract.RequestStatus = ddlStatus.SelectedItem.IsNullOrEmpty() ? String.Empty : (String.IsNullOrEmpty(ddlStatus.SelectedItem.Text) ? String.Empty : ddlStatus.SelectedItem.Text.Trim());
            if (!CurrentViewContext.CustomDataXML.IsNullOrEmpty())
            {
                CurrentViewContext.SearchRequestContract.SharedCustomAttributes = CurrentViewContext.CustomDataXML;
            }
        }

        private void SetAdvanceSearchPanel()
        {
            if (CurrentViewContext.IsAdvanceSearchPanelDisplay)
            {
                contentPanel.Attributes.Add("style", "display:block");
                hdrAdvancedSearch.Attributes["class"] = "mhdr";
                sectionPanel.Attributes["class"] = "section";
            }
            else
            {
                contentPanel.Attributes.Add("style", "display:none");
                hdrAdvancedSearch.Attributes.Add("class", String.Join(" ", hdrAdvancedSearch
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

        private void BindInstitutionAvailability()
        {
            Presenter.GetInstitutionAvailability();
            rblInstitutionAvailability.DataSource = CurrentViewContext.lstInstitutionAvailability;
            rblInstitutionAvailability.DataBind();
            rblInstitutionAvailability.SelectedValue = InstitutionAvailabilityType.AssociatedInstitution.GetStringValue();
        }

        private void BindLocationsAndDepartments()
        {
            Presenter.GetAgencyRootNode();
            Presenter.GetLocations();
            ddlLocation.DataSource = CurrentViewContext.lstLocations;
            ddlLocation.DataBind();
            //ddlLocation.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
            Presenter.GetDepartments();
            ddlDepartment.DataSource = CurrentViewContext.lstDepartment;
            ddlDepartment.DataBind();
            //ddlDepartment.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        }

        private void BindStudentType()
        {
            Presenter.GetStudentTypes();
            ddlStudentType.DataSource = CurrentViewContext.lstStudentType;
            ddlStudentType.DataBind();
            //ddlStudentType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        }

        public void BindSpecialty()
        {
            Presenter.GetSpecialty();
            ddlSpecialty.DataSource = CurrentViewContext.lstSpecialties;
            ddlSpecialty.DataBind();
            //ddlSpecialty.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        }

        private void BindRequestStatus()
        {
            Presenter.GetRequestStatuses();
            ddlStatus.DataSource = CurrentViewContext.lstRequestStatus;
            ddlStatus.DataBind();
        }

        private void GetSharedCustomAttributeList()
        {
            Presenter.GetSharedCustomAttributeList();
        }

        #endregion

        #region Public Methods

        #endregion

        #endregion
    }
}