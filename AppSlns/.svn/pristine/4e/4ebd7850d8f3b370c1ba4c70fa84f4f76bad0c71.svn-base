using CoreWeb.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;
using CoreWeb.IntsofSecurityModel;
using INTSOF.UI.Contract.PlacementMatching;
using Telerik.Web.UI;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using Entity.SharedDataEntity;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.HtmlControls;
using CoreWeb.ClinicalRotation.Views;
using INTSOF.ServiceDataContracts.Core;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class CreateUpdateOpportunityPopup : BaseWebPage, ICreateUpdateOpportunityPopupView
    {
        #region Variables

        #region Private Variables

        private CreateUpdateOpportunityPopupPresenter _presenter = new CreateUpdateOpportunityPopupPresenter();
        private DateTime _defaultDateTime = Convert.ToDateTime("01/01/1900");
        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region public Properties

        public CreateUpdateOpportunityPopupPresenter Presenter
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

        public ICreateUpdateOpportunityPopupView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String ICreateUpdateOpportunityPopupView.UserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.UserId;
            }
        }

        public Int32 CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 ICreateUpdateOpportunityPopupView.AgencyRootNodeID
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

        List<lkpInventoryAvailabilityType> ICreateUpdateOpportunityPopupView.lstInstitutionAvailability
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

        List<AgencyLocationDepartmentContract> ICreateUpdateOpportunityPopupView.lstLocations
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

        List<DepartmentContract> ICreateUpdateOpportunityPopupView.lstDepartment
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

        List<StudentTypeContract> ICreateUpdateOpportunityPopupView.lstStudentType
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

        List<SpecialtyContract> ICreateUpdateOpportunityPopupView.lstSpecialties
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

        Int32 ICreateUpdateOpportunityPopupView.OpportunityID
        {
            get
            {
                if (!ViewState["OpportunityID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["OpportunityID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["OpportunityID"] = value;
            }
        }

        Boolean ICreateUpdateOpportunityPopupView.IsPreviewClick
        {
            get
            {
                if (!ViewState["IsPreviewClick"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsPreviewClick"]);
                }
                return false;
            }
            set
            {
                ViewState["IsPreviewClick"] = value;
            }
        }

        Boolean ICreateUpdateOpportunityPopupView.IsEditClicked
        {
            get
            {
                if (!ViewState["IsEditClicked"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsEditClicked"]);
                }
                return false;
            }
            set
            {
                ViewState["IsEditClicked"] = value;
            }
        }

        Boolean ICreateUpdateOpportunityPopupView.IsNewOpportunity
        {
            get
            {
                if (!ViewState["IsNewOpportunity"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsNewOpportunity"]);
                }
                return false;
            }
            set
            {
                ViewState["IsNewOpportunity"] = value;
            }
        }

        Boolean ICreateUpdateOpportunityPopupView.IsExistingOpportunity
        {
            get
            {
                if (!ViewState["IsExistingOpportunity"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsExistingOpportunity"]);
                }
                return false;
            }
            set
            {
                ViewState["IsExistingOpportunity"] = value;
            }
        }

        Boolean ICreateUpdateOpportunityPopupView.IsShiftsLoadFirstTime
        {
            get
            {
                if (!ViewState["IsShiftsLoadFirstTime"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsShiftsLoadFirstTime"]);
                }
                return false;
            }
            set
            {
                ViewState["IsShiftsLoadFirstTime"] = value;
            }
        }

        List<WeekDayContract> ICreateUpdateOpportunityPopupView.WeekDayList
        {
            get
            {
                if (!ViewState["WeekDayList"].IsNullOrEmpty())
                {
                    return (List<WeekDayContract>)(ViewState["WeekDayList"]);
                }
                return new List<WeekDayContract>(); ;
            }
            set
            {
                ViewState["WeekDayList"] = value;
            }
        }

        List<String> ICreateUpdateOpportunityPopupView.SharedUserTypeCodes
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

        List<TenantDetailContract> ICreateUpdateOpportunityPopupView.lstTenants
        {
            get
            {
                if (!ViewState["lstTenants"].IsNullOrEmpty())
                {
                    return (List<TenantDetailContract>)(ViewState["lstTenants"]);
                }
                return new List<TenantDetailContract>();
            }
            set
            {
                //ddlTenantName.DataSource = value;
                ViewState["lstTenants"] = value;
                //ddlTenantName.DataBind();
            }
        }

        PlacementMatchingContract ICreateUpdateOpportunityPopupView.PlacementMatchingContract
        {
            get
            {
                if (!ViewState["PlacementMatchingContract"].IsNullOrEmpty())
                {
                    return (PlacementMatchingContract)(ViewState["PlacementMatchingContract"]);
                }
                return new PlacementMatchingContract();
            }
            set
            {
                ViewState["PlacementMatchingContract"] = value;
            }
        }

        Int32 ICreateUpdateOpportunityPopupView.SelectedAgencyLocationID
        {
            get
            {
                if (!ViewState["SelectedAgencyLocationID"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedAgencyLocationID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedAgencyLocationID"] = value;
            }
        }

        Int32 ICreateUpdateOpportunityPopupView.SelectedDepartmentID
        {
            get
            {
                if (!ViewState["SelectedDepartmentID"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedDepartmentID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedDepartmentID"] = value;
            }
        }

        List<ShiftDetails> ICreateUpdateOpportunityPopupView.lstShiftDetails
        {
            get
            {
                if (!ViewState["lstShiftDetails"].IsNullOrEmpty())
                    return (List<ShiftDetails>)(ViewState["lstShiftDetails"]);
                return new List<ShiftDetails>();
            }
            set
            {
                ViewState["lstShiftDetails"] = value;
            }
        }

        ShiftDetails ICreateUpdateOpportunityPopupView.ShiftDetail
        {
            get
            {
                if (!ViewState["ShiftDetail"].IsNullOrEmpty())
                    return (ShiftDetails)(ViewState["ShiftDetail"]);
                return new ShiftDetails();
            }
            set
            {
                ViewState["ShiftDetail"] = value;
            }
        }

        String ICreateUpdateOpportunityPopupView.StatusCode
        {
            get
            {
                if (!ViewState["StatusCode"].IsNullOrEmpty())
                    return ViewState["StatusCode"].ToString();
                return String.Empty;
            }
            set
            {
                ViewState["StatusCode"] = value;
            }
        }

        List<CustomAttribteContract> ICreateUpdateOpportunityPopupView.lstSharedCustomAttribute
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

        List<CustomAttribteContract> ICreateUpdateOpportunityPopupView.SaveCustomAttributeList
        {
            get
            {
                if (!ViewState["SaveCustomAttributeList"].IsNullOrEmpty())
                    return (List<CustomAttribteContract>)ViewState["SaveCustomAttributeList"];
                return new List<CustomAttribteContract>();
            }
            set
            {
                ViewState["SaveCustomAttributeList"] = value;
            }
        }

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    CaptureQueryString();
                    CurrentViewContext.IsShiftsLoadFirstTime = true;
                    BindInstitutionAvailability();
                    BindLocations();
                    BindSpecialty();
                    ManageViews();

                }

                ManageButtonsVisibility();
                SavePublishButtons();
                if (!rbtnContainsFloatArea.SelectedValue.IsNullOrEmpty() && Convert.ToBoolean(rbtnContainsFloatArea.SelectedValue))
                    dvFloatArea.Style.Add("display", "block");
                else
                    dvFloatArea.Style.Add("display", "none");


                GenerateCustomAttributeControls();
                //if (!CurrentViewContext.OpportunityID.IsNullOrEmpty() && CurrentViewContext.OpportunityID > AppConsts.NONE)
                //{
                //    //Edit Case
                //    GenerateCustomAttributeControls(CurrentViewContext.OpportunityID);
                //}
                //else
                //{
                //    //Add Case
                //    GenerateCustomAttributeControls(null);
                //}
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

        #region Grid Events

        protected void grdShiftDetails_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    CurrentViewContext.ShiftDetail = new ShiftDetails();
                    if (e.CommandName == RadGrid.UpdateCommandName)
                    {
                        CurrentViewContext.ShiftDetail.ClinicalInventoryShiftID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ClinicalInventoryShiftID"]);
                        CurrentViewContext.lstShiftDetails.Remove(CurrentViewContext.lstShiftDetails[e.Item.ItemIndex]);
                    }

                    WclTextBox txtShift = e.Item.FindControl("txtShift") as WclTextBox;
                    WclTimePicker tpShiftFrom = e.Item.FindControl("tpShiftFrom") as WclTimePicker;
                    WclTimePicker tpShiftTo = e.Item.FindControl("tpShiftTo") as WclTimePicker;
                    WclComboBox ddlDays = e.Item.FindControl("ddlDays") as WclComboBox;
                    WclNumericTextBox txtNoOfStudents = e.Item.FindControl("txtNoOfStudents") as WclNumericTextBox;

                    CurrentViewContext.ShiftDetail.ClinicalInventoryID = CurrentViewContext.OpportunityID.IsNullOrEmpty() ? AppConsts.NONE : CurrentViewContext.OpportunityID;
                    CurrentViewContext.ShiftDetail.Shift = txtShift.Text.IsNullOrEmpty() ? String.Empty : txtShift.Text.Trim();
                    CurrentViewContext.ShiftDetail.ShiftFrom = tpShiftFrom.SelectedTime.IsNullOrEmpty() ? (TimeSpan?)null : tpShiftFrom.SelectedTime;
                    CurrentViewContext.ShiftDetail.ShiftTo = tpShiftTo.SelectedTime.IsNullOrEmpty() ? (TimeSpan?)null : tpShiftTo.SelectedTime;
                    CurrentViewContext.ShiftDetail.NumberOfStudents = txtNoOfStudents.Text.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(txtNoOfStudents.Text.Trim());
                    CurrentViewContext.ShiftDetail.lstDaysId = new List<Int32>();
                    if (!ddlDays.IsNullOrEmpty())
                    {
                        foreach (RadComboBoxItem day in ddlDays.Items)
                        {
                            if (day.Checked)
                            {
                                Int32 dayId = Convert.ToInt32(day.Value);
                                CurrentViewContext.ShiftDetail.lstDaysId.Add(dayId);
                            }
                        }
                        List<String> lstDays = CurrentViewContext.WeekDayList.Where(cond => CurrentViewContext.ShiftDetail.lstDaysId.Contains(cond.WeekDayID)).Select(sel => sel.Name).ToList();
                        CurrentViewContext.ShiftDetail.Days = String.Join(",", lstDays);
                    }


                    if (Presenter.SaveShiftDetails())
                    {
                        grdShiftDetails.Rebind();
                        e.Canceled = false;
                        base.ShowSuccessMessage("Shift saved successfully.");
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowErrorMessage("Some error occured. Please try again.");
                    }
                }
                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    CurrentViewContext.ShiftDetail = new ShiftDetails();
                    CurrentViewContext.ShiftDetail.ClinicalInventoryShiftID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ClinicalInventoryShiftID"]);

                    if (Presenter.DeleteShiftDetail())
                    {
                        grdShiftDetails.Rebind();

                        base.ShowSuccessMessage("Shift deleted successfully.");
                    }
                    else
                    {
                        base.ShowErrorMessage("Some error occured. Please try again.");
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

        protected void grdShiftDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (CurrentViewContext.IsShiftsLoadFirstTime)
                {
                    Presenter.GetShiftsForOpportunity();
                    CurrentViewContext.IsShiftsLoadFirstTime = false;
                }
                grdShiftDetails.DataSource = CurrentViewContext.lstShiftDetails;
                ManageButtonsVisibility();
                SavePublishButtons();
                // grdShiftDetails.DataBind();
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

        protected void grdShiftDetails_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    WclTextBox txtShift = e.Item.FindControl("txtShift") as WclTextBox;
                    WclTimePicker tpShiftFrom = e.Item.FindControl("tpShiftFrom") as WclTimePicker;
                    WclTimePicker tpShiftTo = e.Item.FindControl("tpShiftTo") as WclTimePicker;
                    WclComboBox ddlDays = e.Item.FindControl("ddlDays") as WclComboBox;
                    WclNumericTextBox txtNoOfStudents = e.Item.FindControl("txtNoOfStudents") as WclNumericTextBox;

                    if (!ddlDays.IsNullOrEmpty())
                    {
                        ddlDays.DataSource = CurrentViewContext.WeekDayList;
                        ddlDays.DataBind();
                    }

                    ShiftDetails shiftDetails = e.Item.DataItem as ShiftDetails;

                    if (!shiftDetails.IsNullOrEmpty())
                    {
                        txtShift.Text = shiftDetails.Shift;
                        txtNoOfStudents.Text = shiftDetails.NumberOfStudents.ToString();
                        tpShiftFrom.SelectedTime = shiftDetails.ShiftFrom;
                        tpShiftTo.SelectedTime = shiftDetails.ShiftTo;
                        foreach (RadComboBoxItem item in ddlDays.Items)
                        {
                            Int32 dayId = Convert.ToInt32(item.Value);
                            if (shiftDetails.lstDaysId.Contains(dayId))
                                item.Checked = true;
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

        #endregion

        #region Button Events

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsPreviewClick = true;
                CurrentViewContext.IsEditClicked = false;
                ManageViews();
                if (!CurrentViewContext.SaveCustomAttributeList.IsNullOrEmpty())
                    CurrentViewContext.lstSharedCustomAttribute = CurrentViewContext.SaveCustomAttributeList;
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

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsEditClicked = true;
                CurrentViewContext.IsPreviewClick = false;
                ManageEditControls();
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //publish opportunity.
                if (CurrentViewContext.IsExistingOpportunity && CurrentViewContext.OpportunityID > AppConsts.NONE)
                {
                    CurrentViewContext.PlacementMatchingContract.OpportunityID = CurrentViewContext.OpportunityID;
                    CurrentViewContext.PlacementMatchingContract.lstShift = CurrentViewContext.lstShiftDetails;
                }
                else
                {
                    BindDataContract();
                }
                if (Presenter.PublishOpportunity())
                {
                    hdnIsPublishedSuccessfully.Value = true.ToString();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "returnToParent();", true);
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                BindDataContract();
                if (Presenter.SaveOpportunity())
                {
                    hdnIsSavedSuccessfully.Value = true.ToString();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "returnToParent();", true);
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "returnToParent();", true);
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

        protected void ddlLocation_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (!ddlLocation.SelectedValue.IsNullOrEmpty())
                    CurrentViewContext.SelectedAgencyLocationID = Convert.ToInt32(ddlLocation.SelectedValue);
                BindDepartments();
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

        protected void ddlDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (!ddlDepartment.SelectedValue.IsNullOrEmpty())
                    CurrentViewContext.SelectedDepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue);
                BindStudentType();
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

        #region radio button event

        protected void rblInstitution_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (rblInstitution.SelectedValue == "AAAA")
                //{
                //    Presenter.GetAssociatedTenants();
                //    CurrentViewContext.ForAllInstitutions = false;
                //}
                //if (rblInstitution.SelectedValue == "AAAB")
                //{
                //    Presenter.GetTenants();
                //    CurrentViewContext.ForAllInstitutions = true;
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

        #endregion

        #region Methods

        #region Private Methods

        private void CaptureQueryString()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();

            if (!Request.QueryString["OpportunityId"].IsNullOrEmpty())
            {
                CurrentViewContext.OpportunityID = Convert.ToInt32(Request.QueryString["OpportunityId"]);
            }
            if (!Request.QueryString["IsExistingOpportunity"].IsNullOrEmpty())
            {
                CurrentViewContext.IsExistingOpportunity = Convert.ToBoolean(Request.QueryString["IsExistingOpportunity"]);
            }
            if (!Request.QueryString["IsNewOpportunity"].IsNullOrEmpty())
            {
                CurrentViewContext.IsNewOpportunity = Convert.ToBoolean(Request.QueryString["IsNewOpportunity"]);
            }
            if (!Request.QueryString["StatusCode"].IsNullOrEmpty())
            {
                CurrentViewContext.StatusCode = Convert.ToString(Request.QueryString["StatusCode"]);
            }
        }

        private void ManageViews()
        {
            if (CurrentViewContext.IsExistingOpportunity || CurrentViewContext.IsPreviewClick)
            {
                hdrPopUp.InnerText = "Opportunity Preview ";
                ManagePreview();
            }
            if (CurrentViewContext.IsNewOpportunity)
            {
                hdrPopUp.InnerText = "Create Opportunity";
            }
        }

        private void ManagePreview()
        {
            //Get Data for the selected 
            if (!CurrentViewContext.OpportunityID.IsNullOrEmpty() && CurrentViewContext.OpportunityID > AppConsts.NONE)
            {
                Presenter.GetOpportunityDetailByID();
                Presenter.GetShiftsForOpportunity();
            }
            else
            {
                BindDataContract();
            }
            BindPreviewControls();
        }

        private void BindDataContract()
        {
            CurrentViewContext.PlacementMatchingContract = new PlacementMatchingContract();
            Int32 total = AppConsts.NONE;

            if (CurrentViewContext.OpportunityID > AppConsts.NONE)
            {
                CurrentViewContext.PlacementMatchingContract.OpportunityID = CurrentViewContext.OpportunityID;
            }

            if (!CurrentViewContext.lstShiftDetails.IsNullOrEmpty() && CurrentViewContext.lstShiftDetails.Count > AppConsts.NONE)
            {
                CurrentViewContext.PlacementMatchingContract.lstShift = CurrentViewContext.lstShiftDetails;
                CurrentViewContext.PlacementMatchingContract.Shift = String.Join(",", CurrentViewContext.lstShiftDetails.Select(sel => sel.Shift).ToList());

                List<String> lstRefinedDays = new List<String>();
                List<Int32> lstRefinedDayIds = new List<Int32>();
                foreach (ShiftDetails shiftDetail in CurrentViewContext.lstShiftDetails)
                {
                    lstRefinedDayIds.AddRange(shiftDetail.lstDaysId);
                    total = total + (shiftDetail.NumberOfStudents * shiftDetail.lstDaysId.Count());
                }
                if (!lstRefinedDayIds.IsNullOrEmpty())
                {
                    lstRefinedDayIds = lstRefinedDayIds.Distinct().ToList();
                    lstRefinedDays = CurrentViewContext.WeekDayList.Where(cond => lstRefinedDayIds.Contains(cond.WeekDayID)).Select(sel => sel.Name).ToList();
                    CurrentViewContext.PlacementMatchingContract.Days = String.Join(",", lstRefinedDays);
                }
            }
            else
            {
                //Show error messsage here;
                lblError.Visible = true;
            }

            CurrentViewContext.PlacementMatchingContract.InventoryAvailabilityTypeCode = String.IsNullOrEmpty(rblInstitutionAvailability.SelectedValue) ? InstitutionAvailabilityType.AssociatedInstitution.GetStringValue() : rblInstitutionAvailability.SelectedValue;
            CurrentViewContext.PlacementMatchingContract.AgencyLocationID = String.IsNullOrEmpty(ddlLocation.SelectedValue) ? AppConsts.NONE : Convert.ToInt32(ddlLocation.SelectedValue);
            CurrentViewContext.PlacementMatchingContract.Location = ddlLocation.SelectedItem.IsNullOrEmpty() ? String.Empty : (String.IsNullOrEmpty(ddlLocation.SelectedItem.Text) ? String.Empty : ddlLocation.SelectedItem.Text.Trim());
            CurrentViewContext.PlacementMatchingContract.DepartmentID = String.IsNullOrEmpty(ddlDepartment.SelectedValue) ? AppConsts.NONE : Convert.ToInt32(ddlDepartment.SelectedValue);
            CurrentViewContext.PlacementMatchingContract.Department = ddlDepartment.SelectedItem.IsNullOrEmpty() ? String.Empty : String.IsNullOrEmpty(ddlDepartment.SelectedItem.Text) ? String.Empty : ddlDepartment.SelectedItem.Text.Trim();
            CurrentViewContext.PlacementMatchingContract.Max = total;
            CurrentViewContext.PlacementMatchingContract.StartDate = dpStartDate.SelectedDate.IsNullOrEmpty() ? Convert.ToDateTime(_defaultDateTime) : Convert.ToDateTime(dpStartDate.SelectedDate);
            CurrentViewContext.PlacementMatchingContract.EndDate = dpEndDate.SelectedDate.IsNullOrEmpty() ? Convert.ToDateTime(_defaultDateTime) : Convert.ToDateTime(dpEndDate.SelectedDate);
            CurrentViewContext.PlacementMatchingContract.IsPreceptionShip = String.IsNullOrEmpty(rbtnIsPreceptionship.SelectedValue) ? false : Convert.ToBoolean(rbtnIsPreceptionship.SelectedValue);
            //CurrentViewContext.PlacementMatchingContract.ContainsFloatArea = chkContainsFloatArea.Checked.IsNullOrEmpty() ? false : chkContainsFloatArea.Checked;
            //CurrentViewContext.PlacementMatchingContract.FloatArea = chkContainsFloatArea.Checked ? (String.IsNullOrEmpty(txtFloatArea.Text) ? String.Empty : txtFloatArea.Text.Trim()) : String.Empty;

            CurrentViewContext.PlacementMatchingContract.ContainsFloatArea = rbtnContainsFloatArea.SelectedValue.IsNullOrEmpty() ? false : Convert.ToBoolean(rbtnContainsFloatArea.SelectedValue);
            CurrentViewContext.PlacementMatchingContract.FloatArea = Convert.ToBoolean(rbtnContainsFloatArea.SelectedValue) ? (String.IsNullOrEmpty(txtFloatArea.Text) ? String.Empty : txtFloatArea.Text.Trim()) : String.Empty;
            CurrentViewContext.PlacementMatchingContract.SpecialtyID = ddlSpecialty.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ddlSpecialty.SelectedValue);
            CurrentViewContext.PlacementMatchingContract.Specialty = ddlSpecialty.SelectedItem.IsNullOrEmpty() ? String.Empty : (ddlSpecialty.SelectedItem.Text.IsNullOrEmpty() ? String.Empty : ddlSpecialty.SelectedItem.Text);
            CurrentViewContext.PlacementMatchingContract.StudentTypeIds = ddlStudentType.CheckedItems.IsNullOrEmpty() ? String.Empty : String.Join(",", ddlStudentType.CheckedItems.Select(x => x.Value));
            CurrentViewContext.PlacementMatchingContract.StudentTypes = ddlStudentType.CheckedItems.IsNullOrEmpty() ? String.Empty : String.Join(",", ddlStudentType.CheckedItems.Select(x => x.Text));
            CurrentViewContext.PlacementMatchingContract.Unit = String.IsNullOrEmpty(txtUnit.Text) ? String.Empty : txtUnit.Text.Trim();

            if (dvCustomAttr.FindControl("caCustomAttributes").IsNotNull())
            {
                SharedUserCustomAttributeForm caCustomAttributes = dvCustomAttr.FindControl("caCustomAttributes") as SharedUserCustomAttributeForm;
                CurrentViewContext.SaveCustomAttributeList = caCustomAttributes.GetCustomAttributeValues();
            }
            else
                CurrentViewContext.SaveCustomAttributeList = new List<CustomAttribteContract>();
        }

        private void BindPreviewControls()
        {
            if (!CurrentViewContext.PlacementMatchingContract.IsNullOrEmpty())
            {
                // hide show buttons.
                btnPreview.Visible = false;
                btnSubmit.Visible = CurrentViewContext.PlacementMatchingContract.IsArchived.IsNullOrEmpty() ? true : !CurrentViewContext.PlacementMatchingContract.IsArchived;
                btnEdit.Visible = CurrentViewContext.PlacementMatchingContract.IsArchived.IsNullOrEmpty() ? true : !CurrentViewContext.PlacementMatchingContract.IsArchived;
                btnSave.Visible = false;
                pnlAddOpportunity.Visible = false;
                pnlPreviewOpportunity.Visible = true;


                lblAvailableFor.Text = CurrentViewContext.PlacementMatchingContract.InventoryAvailabilityTypeCode.IsNullOrEmpty() ? "Associated Institution(s)"
                    : (String.Equals(CurrentViewContext.PlacementMatchingContract.InventoryAvailabilityTypeCode, InstitutionAvailabilityType.AssociatedInstitution.GetStringValue()) ? "Associated Institution(s)" : "All Institutions");
                lblLocation.Text = CurrentViewContext.PlacementMatchingContract.Location;
                lblUnit.Text = CurrentViewContext.PlacementMatchingContract.Unit;
                //lblGroup.Text = CurrentViewContext.PlacementMatchingContract.GroupName;
                lblDepartment.Text = CurrentViewContext.PlacementMatchingContract.Department;
                lblSpecialty.Text = CurrentViewContext.PlacementMatchingContract.Specialty;
                lblStudentsType.Text = CurrentViewContext.PlacementMatchingContract.StudentTypes;
                lblMax.Text = CurrentViewContext.PlacementMatchingContract.Max.ToString();
                lblStartDate.Text = CurrentViewContext.PlacementMatchingContract.StartDate.ToString();
                lblEndDate.Text = CurrentViewContext.PlacementMatchingContract.EndDate.ToString();
                lblDays.Text = CurrentViewContext.PlacementMatchingContract.Days;
                lblShift.Text = CurrentViewContext.PlacementMatchingContract.Shift;
                lblIsPreceptionship.Text = Convert.ToBoolean(CurrentViewContext.PlacementMatchingContract.IsPreceptionShip.ToString().ToLower()) == false ? "No" : "Yes";
                lblContainsFloatArea.Text = Convert.ToBoolean(CurrentViewContext.PlacementMatchingContract.ContainsFloatArea.ToString().ToLower()) == false ? "No" : "Yes";

                if (Convert.ToBoolean(CurrentViewContext.PlacementMatchingContract.ContainsFloatArea))
                {
                    dvlabelFloatArea.Visible = true;
                    lblFloatArea.Text = CurrentViewContext.PlacementMatchingContract.FloatArea.IsNullOrEmpty() ? String.Empty : CurrentViewContext.PlacementMatchingContract.FloatArea;
                }
                else
                {
                    dvlabelFloatArea.Visible = false;
                }

                //lblNotes.Text = CurrentViewContext.PlacementMatchingContract.Notes;
            }
        }

        private void ManageEditControls()
        {
            // hide show buttons.
            btnPreview.Visible = true;
            btnSave.Visible = true;
            btnEdit.Visible = false;
            pnlAddOpportunity.Visible = true;
            pnlPreviewOpportunity.Visible = false;
            //rptrShiftDetails.Visible = true;
            grdShiftDetails.Rebind();

            //Manage Controls pre-filled in edit form.
            if (CurrentViewContext.OpportunityID > AppConsts.NONE)
                Presenter.GetOpportunityDetailByID();
            if (!CurrentViewContext.PlacementMatchingContract.IsNullOrEmpty())
            {
                rblInstitutionAvailability.SelectedValue = String.IsNullOrEmpty(CurrentViewContext.PlacementMatchingContract.InventoryAvailabilityTypeCode) ? InstitutionAvailabilityType.AssociatedInstitution.GetStringValue() : CurrentViewContext.PlacementMatchingContract.InventoryAvailabilityTypeCode;
                //Agency hierarchy 
                BindLocations();
                ddlLocation.SelectedValue = CurrentViewContext.PlacementMatchingContract.AgencyLocationID.ToString();
                CurrentViewContext.SelectedAgencyLocationID = CurrentViewContext.PlacementMatchingContract.AgencyLocationID;

                BindDepartments();
                ddlDepartment.SelectedValue = CurrentViewContext.PlacementMatchingContract.DepartmentID.ToString();
                CurrentViewContext.SelectedDepartmentID = CurrentViewContext.PlacementMatchingContract.DepartmentID;

                BindStudentType();
                if (!CurrentViewContext.PlacementMatchingContract.StudentTypeIds.IsNullOrEmpty())
                {
                    String[] studentTypeIds = CurrentViewContext.PlacementMatchingContract.StudentTypeIds.Split(',');
                    foreach (RadComboBoxItem item in ddlStudentType.Items)
                    {
                        item.Checked = studentTypeIds.Contains(item.Value);
                    }
                }

                BindSpecialty();
                ddlSpecialty.SelectedValue = CurrentViewContext.PlacementMatchingContract.SpecialtyID.ToString();

                dpStartDate.SelectedDate = Convert.ToDateTime(CurrentViewContext.PlacementMatchingContract.StartDate);
                dpEndDate.SelectedDate = Convert.ToDateTime(CurrentViewContext.PlacementMatchingContract.EndDate);

                txtUnit.Text = CurrentViewContext.PlacementMatchingContract.Unit;
                rbtnIsPreceptionship.SelectedValue = CurrentViewContext.PlacementMatchingContract.IsPreceptionShip.ToString().ToLower();
                //chkContainsFloatArea.Checked = CurrentViewContext.PlacementMatchingContract.ContainsFloatArea.IsNullOrEmpty() ? false : Convert.ToBoolean(CurrentViewContext.PlacementMatchingContract.ContainsFloatArea);
                rbtnContainsFloatArea.SelectedValue = CurrentViewContext.PlacementMatchingContract.ContainsFloatArea.IsNullOrEmpty() ? false.ToString().ToLower() : CurrentViewContext.PlacementMatchingContract.ContainsFloatArea.ToString().ToLower();
                if (Convert.ToBoolean(CurrentViewContext.PlacementMatchingContract.ContainsFloatArea))
                {
                    dvFloatArea.Style["display"] = "block";
                    txtFloatArea.Text = CurrentViewContext.PlacementMatchingContract.FloatArea;
                }
                else
                {
                    dvFloatArea.Style["display"] = "none";
                }

                //BELOW Code is needed to refined
                //if (!CurrentViewContext.PlacementMatchingContract.HierarchyNodes.IsNullOrEmpty())
                //{
                //    ucAgencyHierarchyMultiple.SelectedAgecnyIds = CurrentViewContext.PlacementMatchingContract.AgencyIdList;
                //    ucAgencyHierarchyMultiple.SelectedNodeIds = CurrentViewContext.PlacementMatchingContract.HierarchyNodes;
                //    ucAgencyHierarchyMultiple.BindTree();
                //}

                //ddlGroups.SelectedValue = CurrentViewContext.PlacementMatchingContract.GroupCode;
                //txtCourse.Text = CurrentViewContext.PlacementMatchingContract.Course;
                //txtNoOfStudents.Text = CurrentViewContext.PlacementMatchingContract.NoOfStudents.ToString();
                //txtSpeciality.Text = CurrentViewContext.PlacementMatchingContract.Speciality;
                //txtNotes.Text = CurrentViewContext.PlacementMatchingContract.Notes;
            }
        }

        private void BindInstitutionAvailability()
        {
            Presenter.GetInstitutionAvailability();
            rblInstitutionAvailability.DataSource = CurrentViewContext.lstInstitutionAvailability;
            rblInstitutionAvailability.DataBind();

            if (CurrentViewContext.IsNewOpportunity)
                rblInstitutionAvailability.SelectedValue = InstitutionAvailabilityType.AssociatedInstitution.GetStringValue();
            if (CurrentViewContext.IsExistingOpportunity)
                rblInstitutionAvailability.SelectedValue = CurrentViewContext.PlacementMatchingContract.IsNullOrEmpty() ? InstitutionAvailabilityType.AssociatedInstitution.GetStringValue()
                                                    : CurrentViewContext.PlacementMatchingContract.InventoryAvailabilityTypeCode;
        }

        private void BindLocations()
        {
            Presenter.GetAgencyRootNode();
            Presenter.GetLocations();
            ddlLocation.DataSource = CurrentViewContext.lstLocations;
            ddlLocation.DataBind();
        }

        private void BindDepartments()
        {
            Presenter.GetLocationDepartments();
            ddlDepartment.DataSource = CurrentViewContext.lstDepartment;
            ddlDepartment.DataBind();
        }

        private void BindStudentType()
        {
            Presenter.GetAgencyDepartmentStudentTypes();
            ddlStudentType.DataSource = CurrentViewContext.lstStudentType;
            ddlStudentType.DataBind();
        }

        public void BindSpecialty()
        {
            Presenter.GetSpecialty();
            ddlSpecialty.DataSource = CurrentViewContext.lstSpecialties;
            ddlSpecialty.DataBind();
        }

        private void ManageButtonsVisibility()
        {
            if (!String.IsNullOrEmpty(CurrentViewContext.StatusCode) && CurrentViewContext.StatusCode == InventoryStatus.Published.GetStringValue())
            {
                btnEdit.Visible = false;
                btnSave.Visible = false;
                btnSubmit.Visible = false;
                btnPreview.Visible = false;
                btnCancel.Visible = true;
            }
            else
            {
                if (CurrentViewContext.IsNewOpportunity)
                {
                    if (CurrentViewContext.IsPreviewClick)
                    {
                        btnEdit.Visible = true;
                        btnSave.Visible = false;
                        btnSubmit.Visible = true;
                        btnPreview.Visible = false;
                        btnCancel.Visible = true;
                    }
                    if (CurrentViewContext.IsEditClicked)
                    {
                        btnEdit.Visible = false;
                        btnSave.Visible = true;
                        btnSubmit.Visible = true;
                        btnPreview.Visible = true;
                        btnCancel.Visible = true;
                    }
                }
                if (CurrentViewContext.IsExistingOpportunity)
                {
                    if (CurrentViewContext.IsEditClicked)
                    {
                        btnEdit.Visible = false;
                        btnSave.Visible = true;
                        btnSubmit.Visible = true;
                        btnPreview.Visible = true;
                        btnCancel.Visible = true;
                    }
                    else
                    {
                        btnEdit.Visible = true;
                        btnSave.Visible = false;
                        btnSubmit.Visible = true;
                        btnPreview.Visible = false;
                        btnCancel.Visible = true;
                    }
                }
            }
        }

        private void SavePublishButtons()
        {
            if (!CurrentViewContext.lstShiftDetails.IsNullOrEmpty() && CurrentViewContext.lstShiftDetails.Count() > AppConsts.NONE)
            {
                btnSave.Enabled = true;
                btnSubmit.Enabled = true;
                btnPreview.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
                btnSubmit.Enabled = false;
                btnPreview.Enabled = false;
            }
        }

        private void GenerateCustomAttributes(SharedUserCustomAttributeForm caCustomAttributes)
        {
            caCustomAttributes.TenantId = AppConsts.NONE;
            caCustomAttributes.TypeCode = SharedCustomAttributeUseType.ClinicalInventory.GetStringValue();
            caCustomAttributes.DataSourceModeType = DataSourceMode.Ids;
            caCustomAttributes.Title = "Other Details";
            caCustomAttributes.ControlDisplayMode = DisplayMode.Controls;
            caCustomAttributes.CurrentLoggedInUserId = CurrentViewContext.CurrentLoggedInUserID;
            caCustomAttributes.ValidationGroup = "grpFormSubmit";
            caCustomAttributes.IsReadOnly = false;
            caCustomAttributes.lstTypeCustomAttributes = CurrentViewContext.lstSharedCustomAttribute;
            caCustomAttributes.EnableViewState = false;
        }

        private void GenerateCustomAttributeControls()
        {
            Int32? opportunityId = null;

            if (!CurrentViewContext.OpportunityID.IsNullOrEmpty() && CurrentViewContext.OpportunityID > AppConsts.NONE)
                opportunityId = CurrentViewContext.OpportunityID;

            if (CurrentViewContext.lstSharedCustomAttribute.IsNullOrEmpty() || CurrentViewContext.lstSharedCustomAttribute.Count == AppConsts.NONE)
                Presenter.GetSharedCustomAttributeList(opportunityId);
            if (!CurrentViewContext.lstSharedCustomAttribute.IsNullOrEmpty())
            {
                SharedUserCustomAttributeForm caCustomAttributesExisting = dvCustomAttr.FindControl("caCustomAttributes") as SharedUserCustomAttributeForm;
                dvCustomAttr.Controls.Remove(caCustomAttributesExisting); //First remove the control of custome attributes that is already added in pageload event
                SharedUserCustomAttributeForm caCustomAttributes = (SharedUserCustomAttributeForm)Page.LoadControl("~\\ClinicalRotation\\UserControl\\SharedUserCustomAttributeForm.ascx");
                caCustomAttributes.ID = "caCustomAttributes";
                GenerateCustomAttributes(caCustomAttributes);
                dvCustomAttr.Controls.Add(caCustomAttributes);
            }
        }

        #endregion

        #endregion

    }
}