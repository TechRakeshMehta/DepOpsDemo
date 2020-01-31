using CoreWeb.Shell;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CoreWeb.IntsofSecurityModel;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Core;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class ManageOpportunity : BaseUserControl, IManageOpportunityView
    {
        #region Variables

        #region Private Variables

        private ManageOpportunityPresenter _presenter = new ManageOpportunityPresenter();
        private DateTime _defaultDateTime = Convert.ToDateTime("01/01/1900");

        #endregion

        #region public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region public Properties

        public ManageOpportunityPresenter Presenter
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

        public IManageOpportunityView CurrentViewContext
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

        List<lkpInventoryAvailabilityType> IManageOpportunityView.lstInstitutionAvailability
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

        List<AgencyLocationDepartmentContract> IManageOpportunityView.lstLocations
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

        Guid IManageOpportunityView.UserId
        {
            get
            {
                return base.SysXMembershipUser.UserId;
            }
        }

        Int32 IManageOpportunityView.AgencyRootNodeID
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

        List<DepartmentContract> IManageOpportunityView.lstDepartment
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

        List<StudentTypeContract> IManageOpportunityView.lstStudentType
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

        List<SpecialtyContract> IManageOpportunityView.lstSpecialties
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

        List<PlacementMatchingContract> IManageOpportunityView.lstPlacementMatchingContract
        {
            get;
            set;
        }

        PlacementMatchingContract IManageOpportunityView.SearchContract { get; set; }

        List<TenantDetailContract> IManageOpportunityView.lstTenants
        {
            get
            {
                if (!ViewState["lstTenants"].IsNullOrEmpty())
                    return (List<TenantDetailContract>)ViewState["lstTenants"];
                return new List<TenantDetailContract>();
            }
            set
            {
                ViewState["lstTenants"] = value;
            }
        }

        Boolean IManageOpportunityView.IsAdvanceSearchPanelDisplay
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

        List<WeekDayContract> IManageOpportunityView.WeekDayList
        {
            set
            {
                ddlDays.DataSource = value;
                ddlDays.DataBind();
            }
        }

        List<String> IManageOpportunityView.SharedUserTypeCodes
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

        //public String UserID
        //{
        //    get
        //    {
        //        return SysXWebSiteUtils.SessionService.UserId;
        //    }
        //}

        Boolean IManageOpportunityView.IsSavedSuccessfully
        {
            get;
            set;
        }

        Dictionary<Int32, String> IManageOpportunityView.SelectedOpportunityIDs
        {
            get
            {
                if (ViewState["SelectedOpportunityIDs"] != null)
                {
                    return (Dictionary<Int32, String>)ViewState["SelectedOpportunityIDs"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["SelectedOpportunityIDs"] = value;
            }
        }

        List<CustomAttribteContract> IManageOpportunityView.lstSharedCustomAttribute
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
                base.Title = "Manage Opportunity";
                base.SetPageTitle("Manage Opportunity");
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
                }
                SetAdvanceSearchPanel();
                BindSearchContract();

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



                //ucAgencyHierarchyMultiple.CurrentOrgUserId = CurrentViewContext.CurrentLoggedInUserID;
                //ucAgencyHierarchyMultiple.AgencyHierarchyNodeSelection = true;
                //ucAgencyHierarchyMultiple.TenantId = (CurrentViewContext.lstTenants.FirstOrDefault().IsNullOrEmpty()) ? 0 : Convert.ToInt32(CurrentViewContext.lstTenants.FirstOrDefault().TenantID);
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

        protected void fsucCmdBarButton_SearchClick(object sender, EventArgs e)
        {
            try
            {
                BindSearchContract();
                grdOpportunities.Rebind();
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

        protected void fsucCmdBarButton_ResetClick(object sender, EventArgs e)
        {
            try
            {
                ResetTenant();
                ResetSearchFilters();
                ResetGridFilters();
                BindSearchContract();
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
                if (!hdnIsSavedSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnIsSavedSuccessfully.Value.ToLower()))
                    {
                        base.ShowSuccessMessage("Opportunity saved successfully.");
                        grdOpportunities.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                    }
                }
                if (!hdnIsPublishedSuccessfully.Value.IsNullOrEmpty())
                {
                    if (Convert.ToBoolean(hdnIsPublishedSuccessfully.Value.ToLower()))
                    {
                        base.ShowSuccessMessage("Opportunity published successfully.");
                        grdOpportunities.Rebind();
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

        protected void btnUnArchive_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CurrentViewContext.SelectedOpportunityIDs.IsNullOrEmpty())
                {
                    if (Presenter.UnArchiveOpportunities())//UnArchiveOpportunities())
                    {
                        base.ShowSuccessMessage("Opportunity(s) unarchived successfully.");
                        CurrentViewContext.SelectedOpportunityIDs = new Dictionary<Int32, String>();
                        CloseGridForm();
                    }
                    else
                    {
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                    }
                }
                else
                {
                    base.ShowInfoMessage("Please select opportunity(s).");
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
                if (!CurrentViewContext.SelectedOpportunityIDs.IsNullOrEmpty())
                {
                    if (Presenter.ArchiveOpportunities())//ArchiveOpportunities())
                    {
                        base.ShowSuccessMessage("Opportunity(s) archived successfully.");
                        CurrentViewContext.SelectedOpportunityIDs = new Dictionary<Int32, String>();
                        CloseGridForm();
                    }
                    else
                    {
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                    }
                }
                else
                {
                    base.ShowInfoMessage("Please select opportunity(s).");
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CurrentViewContext.SelectedOpportunityIDs.IsNullOrEmpty())
                {
                    if (CurrentViewContext.SelectedOpportunityIDs.Values.Contains(InventoryStatus.Published.GetStringValue()))
                    {
                        base.ShowInfoMessage("Published opportunity(s) can't be deleted. Please select the appropriate opportunity(s) to delete.");
                    }
                    else
                    {

                        if (Presenter.DeleteOpportunity())//DeleteOpportunity())
                        {
                            base.ShowSuccessMessage("Opportunity(s) deleted successfully.");
                            CurrentViewContext.SelectedOpportunityIDs = new Dictionary<Int32, String>();
                            CloseGridForm();
                        }
                        else
                        {
                            base.ShowInfoMessage("Some error has occurred. Please try again.");
                        }
                    }
                }
                else
                {
                    base.ShowInfoMessage("Please select opportunity(s).");
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

        protected void grdOpportunities_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetOpportunities();
                grdOpportunities.DataSource = CurrentViewContext.lstPlacementMatchingContract;
                if (!CurrentViewContext.lstPlacementMatchingContract.IsNullOrEmpty() && CurrentViewContext.lstPlacementMatchingContract.Count > AppConsts.NONE)
                {
                    cmdArchive.Visible = true;
                    btnDelete.Visible = true;
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

        protected void grdOpportunities_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    hdnIsNewOpportunity.Value = true.ToString();
                    hdnIsExistingOpportunity.Value = false.ToString();
                    hdnOppportunityId.Value = String.Empty;
                    hdnStatusCode.Value = String.Empty;
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "CreateUpdateOpportunityPopUp();", true);
                }

                if (e.CommandName == "Preview")
                {
                    Int32 opportunityID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OpportunityID"]);
                    String statusCode = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StatusCode"]);
                    //Boolean ForAllInstitution = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ForAllInstitution"]);
                    hdnOppportunityId.Value = opportunityID.ToString();
                    hdnIsNewOpportunity.Value = false.ToString();
                    hdnIsExistingOpportunity.Value = true.ToString();
                    hdnStatusCode.Value = statusCode;
                    //hdnForAllInstitution.Value = ForAllInstitution.ToString().ToLower();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "CreateUpdateOpportunityPopUp();", true);
                }
                e.Canceled = true;
                return;
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

        #region Checkbox events

        protected void chkSelectOpportunity_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                if (checkBox.IsNull())
                    return;

                isChecked = checkBox.Checked;

                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                Int32 opportunityID = (Int32)dataItem.GetDataKeyValue("OpportunityID");
                String StatusCode = Convert.ToString(dataItem.GetDataKeyValue("StatusCode"));

                if (CurrentViewContext.SelectedOpportunityIDs.IsNull())
                    CurrentViewContext.SelectedOpportunityIDs = new Dictionary<Int32, String>();

                if (isChecked)
                {
                    if (!CurrentViewContext.SelectedOpportunityIDs.Keys.Contains(opportunityID))
                    {
                        CurrentViewContext.SelectedOpportunityIDs.Add(opportunityID, StatusCode);
                    }
                }
                else
                {
                    if (CurrentViewContext.SelectedOpportunityIDs != null && CurrentViewContext.SelectedOpportunityIDs.Keys.Contains(opportunityID))
                        CurrentViewContext.SelectedOpportunityIDs.Remove(opportunityID);
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

        #region Radio button

        protected void rblInstitutionAvailability_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rblInstitutionAvailability.SelectedValue == InstitutionAvailabilityType.AssociatedInstitution.GetStringValue())
                    Presenter.GetAssociatedTenants();
                if (rblInstitutionAvailability.SelectedValue == InstitutionAvailabilityType.AllInstitutions.GetStringValue())
                    Presenter.GetTenants();
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

        #endregion

        #endregion

        #region Methods

        #region Private Methods

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

        /// <summary>
        /// Method to Reset Grid 
        /// </summary>
        private void ResetGridFilters()
        {
            grdOpportunities.MasterTableView.SortExpressions.Clear();
            grdOpportunities.CurrentPageIndex = 0;
            grdOpportunities.MasterTableView.CurrentPageIndex = 0;
            grdOpportunities.MasterTableView.IsItemInserted = false;
            grdOpportunities.MasterTableView.ClearEditItems();
            grdOpportunities.Rebind();
        }

        /// <summary>
        /// Method to reset search filters
        /// </summary>
        private void ResetSearchFilters()
        {
            ddlLocation.ClearSelection();
            ddlDepartment.ClearSelection();
            ddlSpecialty.ClearSelection();
            txtMaxNoOfStudent.Text = String.Empty;
            txtShift.Text = String.Empty;
            //ddlGroups.ClearSelection();
            dpStartDate.Clear();
            dpEndDate.Clear();
            ddlDays.ClearCheckedItems();
            ddlStudentType.ClearCheckedItems();
            rbArchiveStatus.SelectedValue = "false";
            caCustomAttributesID.ResetCustomAttributes();
            //ucAgencyHierarchyMultiple.Reset();
            //ucAgencyHierarchyMultiple.CurrentOrgUserId = CurrentViewContext.CurrentLoggedInUserID;
            //ucAgencyHierarchyMultiple.AgencyHierarchyNodeSelection = true;
            //ucAgencyHierarchyMultiple.TenantId = (CurrentViewContext.lstTenants.FirstOrDefault().IsNullOrEmpty()) ? 0 : Convert.ToInt32(CurrentViewContext.lstTenants.FirstOrDefault().TenantID);
        }

        private void ResetTenant()
        {
            //ddlTenantName.SelectedValue = String.Empty;
            //ddlTenantName.Items.ForEach(itm => itm.Checked = false);
            rblInstitutionAvailability.SelectedValue = InstitutionAvailabilityType.AssociatedInstitution.GetStringValue(); // Associated Institutions
        }

        private void BindSearchContract()
        {
            CurrentViewContext.SearchContract = new PlacementMatchingContract();

            //CurrentViewContext.SearchContract.SelectedTenantIds = ddlTenantName.CheckedItems.IsNullOrEmpty() ? String.Empty : String.Join(",", ddlTenantName.CheckedItems.Select(x => x.Value));
            //CurrentViewContext.SearchContract.SelectedTenantIds = CurrentViewContext.lstTenants.IsNullOrEmpty() ? String.Empty : String.Join(",", CurrentViewContext.lstTenants.Select(x => x.TenantID));
            CurrentViewContext.SearchContract.InventoryAvailabilityTypeCode = String.IsNullOrEmpty(rblInstitutionAvailability.SelectedValue) ? InstitutionAvailabilityType.AssociatedInstitution.GetStringValue() : rblInstitutionAvailability.SelectedValue;
            CurrentViewContext.SearchContract.AgencyLocationID = String.IsNullOrEmpty(ddlLocation.SelectedValue) ? AppConsts.NONE : Convert.ToInt32(ddlLocation.SelectedValue);
            CurrentViewContext.SearchContract.Location = ddlLocation.SelectedItem.IsNullOrEmpty() ? String.Empty : (String.IsNullOrEmpty(ddlLocation.SelectedItem.Text) ? String.Empty : ddlLocation.SelectedItem.Text.Trim());
            CurrentViewContext.SearchContract.DepartmentID = String.IsNullOrEmpty(ddlDepartment.SelectedValue) ? AppConsts.NONE : Convert.ToInt32(ddlDepartment.SelectedValue);
            CurrentViewContext.SearchContract.Department = ddlDepartment.SelectedItem.IsNullOrEmpty() ? String.Empty : String.IsNullOrEmpty(ddlDepartment.SelectedItem.Text) ? String.Empty : ddlDepartment.SelectedItem.Text.Trim();
            CurrentViewContext.SearchContract.StartDate = dpStartDate.SelectedDate.IsNullOrEmpty() ? (DateTime?)(null) : Convert.ToDateTime(dpStartDate.SelectedDate);
            CurrentViewContext.SearchContract.EndDate = dpEndDate.SelectedDate.IsNullOrEmpty() ? (DateTime?)(null) : Convert.ToDateTime(dpEndDate.SelectedDate);
            CurrentViewContext.SearchContract.SpecialtyID = ddlSpecialty.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ddlSpecialty.SelectedValue);
            CurrentViewContext.SearchContract.Specialty = ddlSpecialty.SelectedItem.IsNullOrEmpty() ? String.Empty : (ddlSpecialty.SelectedItem.Text.IsNullOrEmpty() ? String.Empty : ddlSpecialty.SelectedItem.Text);
            CurrentViewContext.SearchContract.StudentTypeIds = ddlStudentType.CheckedItems.IsNullOrEmpty() ? String.Empty : String.Join(",", ddlStudentType.CheckedItems.Select(x => x.Value));
            CurrentViewContext.SearchContract.StudentTypes = ddlStudentType.CheckedItems.IsNullOrEmpty() ? String.Empty : String.Join(",", ddlStudentType.CheckedItems.Select(x => x.Text));
            CurrentViewContext.SearchContract.Max = String.IsNullOrEmpty(txtMaxNoOfStudent.Text) ? AppConsts.NONE : Convert.ToInt32(txtMaxNoOfStudent.Text.Trim());
            CurrentViewContext.SearchContract.Days = String.Join(",", ddlDays.CheckedItems.Select(x => x.Text));
            CurrentViewContext.SearchContract.DayIds = String.Join(",", ddlDays.CheckedItems.Select(x => x.Value));
            CurrentViewContext.SearchContract.Shift = String.IsNullOrEmpty(txtShift.Text) ? String.Empty : txtShift.Text.Trim();
            CurrentViewContext.SearchContract.IsArchived = String.IsNullOrEmpty(rbArchiveStatus.SelectedValue) ? false : Convert.ToBoolean(rbArchiveStatus.SelectedValue);

            if (!CurrentViewContext.CustomDataXML.IsNullOrEmpty())
            {
                CurrentViewContext.SearchContract.SharedCustomAttributes = CurrentViewContext.CustomDataXML;
            }
            // below code needed to refine.
            //List<Int32> selectedAgencyIds = new List<Int32>();
            //AgencyhierarchyCollection agencyHierarchyCollection = ucAgencyHierarchyMultiple.GetAgencyHierarchyCollection();

            //if (agencyHierarchyCollection.IsNotNull() && agencyHierarchyCollection.agencyhierarchy.IsNotNull())
            //{
            //    selectedAgencyIds.AddRange(agencyHierarchyCollection.agencyhierarchy.Select(d => d.AgencyID).ToList());
            //    if (!selectedAgencyIds.IsNullOrEmpty())
            //        CurrentViewContext.SearchContract.AgencyIdList = String.Join(",", selectedAgencyIds.ToArray());

            //    List<Int32> lstAgencyHierarchyIds = agencyHierarchyCollection.agencyhierarchy.Select(sel => sel.AgencyNodeID).ToList();
            //    if (!lstAgencyHierarchyIds.IsNullOrEmpty())
            //    {
            //        CurrentViewContext.SearchContract.HierarchyNodes = String.Join(",", lstAgencyHierarchyIds);
            //    }
            //}
        }

        private void CloseGridForm()
        {
            grdOpportunities.MasterTableView.IsItemInserted = false;
            grdOpportunities.MasterTableView.ClearEditItems();
            grdOpportunities.MasterTableView.Rebind();
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