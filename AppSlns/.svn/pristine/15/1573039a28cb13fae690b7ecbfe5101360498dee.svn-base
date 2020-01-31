using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class CreateDraftControl : BaseUserControl, ICreateDraftView
    {
        #region Variables
        private CreateDraftPresenter _presenter = new CreateDraftPresenter();
        private Int32 _tenantId;
        #endregion

        #region Properties

        public CreateDraftPresenter Presenter
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

        public ICreateDraftView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 TenantId
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

        Int32 ICreateDraftView.CurrentAgencyHierarchyID { get; set; }
        Boolean ICreateDraftView.IsSharedUser
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user.IsNotNull() && user.IsSharedUser.IsNotNull() && user.IsSharedUser)
                {
                    return true;
                }
                return false;
            }
        }

        Int32 ICreateDraftView.CurrentLoggedInUser
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Int32 OpportunityId { get; set; }

        public String PageRequested { get; set; }

        public Int32 RequestId { get; set; }

        public String RequestStatusCode { get; set; }

        public Int32 SelectedTenantID
        {
            get
            {
                if (!ViewState["SelectedTenantID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["SelectedTenantID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedTenantID"] = value;
            }
        }

        public PlacementMatchingContract OpportunityDetails
        {
            get
            {
                if (!ViewState["OpportunityDetails"].IsNullOrEmpty())
                    return (PlacementMatchingContract)ViewState["OpportunityDetails"];
                return new PlacementMatchingContract();
            }
            set
            {
                ViewState["OpportunityDetails"] = value;
            }
        }

        public RequestDetailContract RequestDetail
        {
            get
            {
                if (!ViewState["RequestDetail"].IsNullOrEmpty())
                    return (RequestDetailContract)ViewState["RequestDetail"];
                return new RequestDetailContract();
            }
            set
            {
                ViewState["RequestDetail"] = value;
            }
        }

        Dictionary<Int32, String> ICreateDraftView.lstDays
        {
            get
            {
                if (!ViewState["lstDays"].IsNullOrEmpty())
                    return (Dictionary<Int32, String>)ViewState["lstDays"];
                return new Dictionary<Int32, String>();
            }
            set
            {
                ViewState["lstDays"] = value;
            }
        }

        String ICreateDraftView.TenantName
        {
            get
            {
                if (!ViewState["TenantName"].IsNullOrEmpty())
                    return ViewState["TenantName"].ToString();
                return String.Empty;
            }
            set
            {
                ViewState["TenantName"] = value;
            }
        }
        List<CustomAttribteContract> ICreateDraftView.CustomAttributeList
        {
            get
            {
                if (!ViewState["CustomAttributeList"].IsNullOrEmpty())
                    return (List<CustomAttribteContract>)ViewState["CustomAttributeList"];
                return new List<CustomAttribteContract>();
            }
            set
            {
                ViewState["CustomAttributeList"] = value;
            }
        }
        List<CustomAttribteContract> ICreateDraftView.SetCustomAttributeList
        {
            get;
            set;
        }
        public Boolean IsAgencyUserLoggedIn
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                List<String> lstSharedUserTypeCode = user.SharedUserTypesCode;
                if (lstSharedUserTypeCode.Contains(OrganizationUserType.AgencyUser.GetStringValue()) && user.IsSharedUser)
                {
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    BindData();
                    ManageButtonVisibility();
                    Presenter.GetSharedCustomAttributeList();
                }

                if (CurrentViewContext.IsAgencyUserLoggedIn)
                {
                    if (!CurrentViewContext.RequestDetail.IsNullOrEmpty())
                    {
                        hdnTenantId.Value = CurrentViewContext.RequestDetail.InstitutionID.ToString();
                        CurrentViewContext.SelectedTenantID = CurrentViewContext.RequestDetail.InstitutionID;
                    }
                }
                else
                {
                    if (Presenter.IsAdminLoggedIn())
                    {
                        hdnTenantId.Value = CurrentViewContext.SelectedTenantID.ToString();
                    }
                    else
                    {
                        hdnTenantId.Value = CurrentViewContext.TenantId.ToString();
                    }
                }


                if (CurrentViewContext.CustomAttributeList.Any())
                {
                    caCustomAttributesID.IsSearchTypeControl = false;
                    caCustomAttributesID.TenantId = CurrentViewContext.SelectedTenantID;
                    caCustomAttributesID.TypeCode = SharedCustomAttributeUseType.ClinicalInventoryRequest.GetStringValue();
                    caCustomAttributesID.DataSourceModeType = DataSourceMode.Ids;
                    caCustomAttributesID.Title = "Other Details";
                    caCustomAttributesID.ControlDisplayMode = DisplayMode.Controls;
                    caCustomAttributesID.CurrentLoggedInUserId = base.CurrentUserId;
                    caCustomAttributesID.ValidationGroup = "grpFormSubmit";

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

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            try
            {
                String requestStatusCode = String.Empty;
                if (CurrentViewContext.IsAgencyUserLoggedIn)
                    requestStatusCode = RequestStatusCodes.Modified.GetStringValue().ToString();
                else
                    requestStatusCode = RequestStatusCodes.Requested.GetStringValue().ToString();
                CurrentViewContext.SetCustomAttributeList = caCustomAttributesID.GetCustomAttributeValues();
                BindRequest(requestStatusCode);
                if (Presenter.SaveRequest())
                {
                    hdnRequestPublishedSuccessfully.Value = true.ToString();
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
                String requestStatusCode = String.Empty;
                if (CurrentViewContext.IsAgencyUserLoggedIn)
                    requestStatusCode = RequestStatusCodes.Modified.GetStringValue().ToString();
                else
                    requestStatusCode = RequestStatusCodes.Draft.GetStringValue().ToString();
                CurrentViewContext.SetCustomAttributeList = caCustomAttributesID.GetCustomAttributeValues();
                BindRequest(requestStatusCode);
                if (Presenter.SaveRequest())
                {
                    hdnRequestSavedSuccessfully.Value = true.ToString();
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

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.ChangeRequestStatus(CurrentViewContext.RequestDetail.RequestID, RequestStatusCodes.Approved.GetStringValue().ToString());
                hdnRequestApprovedSuccessfully.Value = true.ToString();
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

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                btnApprove.Visible = false;
                btnEdit.Visible = false; // to modify request
                btnCancelRequest.Visible = false;
                btnReject.Visible = false;
                btnArchive.Visible = false;
                //fsucCmdBarButton.Visible = true;
                btnCancel.Visible = true;
                btnPublish.Visible = true;
                btnSave.Visible = true;
                caCustomAttributesID.IsReadOnly = true;
                OpportunityDaySelection();

                RequestDaySelection(CurrentViewContext.RequestDetail.lstDays);
                //Enable Field to edit/update
                txtCourse.Enabled = true;
                txtNotes.Enabled = true;
                dteStartDate.Enabled = true;
                dteEndDate.Enabled = true;
                txtStudents.Enabled = true;
                btnMonday.Enabled = true;
                btnWednesday.Enabled = true;
                btnFriday.Enabled = true;
                //ddlGroups.Enabled = true;
                ddlShift.Enabled = true;
                lblCreateDraft.Text = "Update Request";
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

        protected void btnCancelRequest_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.ChangeRequestStatus(CurrentViewContext.RequestDetail.RequestID, RequestStatusCodes.Cancelled.GetStringValue().ToString());
                hdnRequestCancelledSuccessfully.Value = true.ToString();
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

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.ChangeRequestStatus(CurrentViewContext.RequestDetail.RequestID, RequestStatusCodes.Rejected.GetStringValue().ToString());
                hdnRequestRejectedSuccessfully.Value = true.ToString();
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

        protected void btnArchive_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.ChangeRequestStatus(CurrentViewContext.RequestDetail.RequestID, RequestStatusCodes.Archived.GetStringValue().ToString());
                hdnRequestArchivedSuccessfully.Value = true.ToString();
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

        #region Day Button Clicks

        protected void btnSunday_Click(object sender, EventArgs e)
        {
            if (btnSunday.BackColor == System.Drawing.Color.LightGreen)
            {
                btnSunday.BackColor = System.Drawing.Color.LightBlue;
                CurrentViewContext.lstDays.Add(Convert.ToInt32(DayOfWeek.Saturday) + 1, "Sun");
            }
            else
            {
                btnSunday.BackColor = System.Drawing.Color.LightGreen;
                CurrentViewContext.lstDays.Remove(Convert.ToInt32(DayOfWeek.Saturday) + 1);
            }
        }

        protected void btnMonday_Click(object sender, EventArgs e)
        {
            if (btnMonday.BackColor == System.Drawing.Color.LightGreen)
            {
                btnMonday.BackColor = System.Drawing.Color.LightBlue;
                CurrentViewContext.lstDays.Add(Convert.ToInt32(DayOfWeek.Monday), "Mon");
            }
            else
            {
                btnMonday.BackColor = System.Drawing.Color.LightGreen;
                CurrentViewContext.lstDays.Remove(Convert.ToInt32(DayOfWeek.Monday));
            }
        }

        protected void btnTuesday_Click(object sender, EventArgs e)
        {
            if (btnTuesday.BackColor == System.Drawing.Color.LightGreen)
            {
                btnTuesday.BackColor = System.Drawing.Color.LightBlue;
                CurrentViewContext.lstDays.Add(Convert.ToInt32(DayOfWeek.Tuesday), "Tue");
            }
            else
            {
                btnTuesday.BackColor = System.Drawing.Color.LightGreen;
                CurrentViewContext.lstDays.Remove(Convert.ToInt32(DayOfWeek.Tuesday));
            }
        }

        protected void btnWednesday_Click(object sender, EventArgs e)
        {
            if (btnWednesday.BackColor == System.Drawing.Color.LightGreen)
            {
                btnWednesday.BackColor = System.Drawing.Color.LightBlue;
                CurrentViewContext.lstDays.Add(Convert.ToInt32(DayOfWeek.Wednesday), "Wed");
            }
            else
            {
                btnWednesday.BackColor = System.Drawing.Color.LightGreen;
                CurrentViewContext.lstDays.Remove(Convert.ToInt32(DayOfWeek.Wednesday));
            }
        }

        protected void btnThursday_Click(object sender, EventArgs e)
        {
            if (btnThursday.BackColor == System.Drawing.Color.LightGreen)
            {
                btnThursday.BackColor = System.Drawing.Color.LightBlue;
                CurrentViewContext.lstDays.Add(Convert.ToInt32(DayOfWeek.Thursday), "Thr");
            }
            else
            {
                btnThursday.BackColor = System.Drawing.Color.LightGreen;
                CurrentViewContext.lstDays.Remove(Convert.ToInt32(DayOfWeek.Thursday));
            }
        }

        protected void btnFriday_Click(object sender, EventArgs e)
        {
            if (btnFriday.BackColor == System.Drawing.Color.LightGreen)
            {
                btnFriday.BackColor = System.Drawing.Color.LightBlue;
                CurrentViewContext.lstDays.Add(Convert.ToInt32(DayOfWeek.Friday), "Fri");
            }
            else
            {
                btnFriday.BackColor = System.Drawing.Color.LightGreen;
                CurrentViewContext.lstDays.Remove(Convert.ToInt32(DayOfWeek.Friday));
            }
        }

        protected void btnSaturday_Click(object sender, EventArgs e)
        {
            if (btnSaturday.BackColor == System.Drawing.Color.LightGreen)
            {
                btnSaturday.BackColor = System.Drawing.Color.LightBlue;
                CurrentViewContext.lstDays.Add(Convert.ToInt32(DayOfWeek.Saturday), "Sat");
            }
            else
            {
                btnSaturday.BackColor = System.Drawing.Color.LightGreen;
                CurrentViewContext.lstDays.Remove(Convert.ToInt32(DayOfWeek.Saturday));
            }
        }

        #endregion

        #endregion

        #region Dropdown events

        protected void ddlShift_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ResetButtonColor();
            CurrentViewContext.lstDays = new Dictionary<Int32, String>();
            OpportunityDaySelection();
            if (Convert.ToInt32(ddlShift.SelectedValue) == CurrentViewContext.RequestDetail.ShiftID)
            {
                RequestDaySelection(CurrentViewContext.RequestDetail.lstDays);
            }
        }

        #endregion

        #endregion

        #region Methods

        #region private Methods

        private void BindData()
        {
            CurrentViewContext.OpportunityDetails = new PlacementMatchingContract();
            if (!CurrentViewContext.OpportunityId.IsNullOrEmpty() && CurrentViewContext.OpportunityId > AppConsts.NONE)
            {
                Presenter.GetOpportunityData(CurrentViewContext.OpportunityId);
                if (CurrentViewContext.OpportunityDetails.IsNotNull())
                {
                    hdnOpportunityId.Value = CurrentViewContext.OpportunityId.ToString();
                    lblInstitution.Text = CurrentViewContext.TenantName;
                    lblLocation.Text = CurrentViewContext.OpportunityDetails.Location;
                    lblUnit.Text = CurrentViewContext.OpportunityDetails.Unit;
                    //lblUnit1.Text = CurrentViewContext.OpportunityDetails.Unit;
                    lblDepartment.Text = CurrentViewContext.OpportunityDetails.Department;
                    lblSpeciality.Text = CurrentViewContext.OpportunityDetails.Specialty;
                    lblStudentType.Text = CurrentViewContext.OpportunityDetails.StudentTypes;
                    lblMax.Text = CurrentViewContext.OpportunityDetails.Max.ToString();
                    //lblMax1.Text = lblMax.Text;
                    lblDays.Text = CurrentViewContext.OpportunityDetails.Days;
                    lblShift.Text = CurrentViewContext.OpportunityDetails.Shift;
                    lblFloatArea.Text = CurrentViewContext.OpportunityDetails.FloatArea;
                    lblIsPreceptonship.Text = CurrentViewContext.OpportunityDetails.IsPreceptionShip?"Yes":"No";
                    ddlShift.DataSource = CurrentViewContext.OpportunityDetails.lstShift;
                    dteStartDate.MinDate = CurrentViewContext.OpportunityDetails.StartDate.Value;
                    dteStartDate.MaxDate = CurrentViewContext.OpportunityDetails.EndDate.Value;
                    dteEndDate.MinDate = CurrentViewContext.OpportunityDetails.StartDate.Value;
                    dteEndDate.MaxDate = CurrentViewContext.OpportunityDetails.EndDate.Value;
                    lblFloatArea.Text = CurrentViewContext.OpportunityDetails.ContainsFloatArea ? "Yes" : "No";
                    lblFloatAreaText.Text = CurrentViewContext.OpportunityDetails.ContainsFloatArea ? CurrentViewContext.OpportunityDetails.FloatArea : String.Empty;
                    spnFloatArea.Visible = CurrentViewContext.OpportunityDetails.ContainsFloatArea?true:false;
                    ddlShift.DataBind();
                    lblDates.Text = CurrentViewContext.OpportunityDetails.StartDate.HasValue ? CurrentViewContext.OpportunityDetails.StartDate.Value.ToShortDateString() + " - " + CurrentViewContext.OpportunityDetails.EndDate.Value.ToShortDateString() : string.Empty;
                    CurrentViewContext.CurrentAgencyHierarchyID = CurrentViewContext.OpportunityDetails.AgencyHierarchyID;


                    if (CurrentViewContext.PageRequested.Equals(RequestDetails.REQUESTDETAILS.GetStringValue()))
                    {
                        CurrentViewContext.RequestDetail = new RequestDetailContract();
                        if (!CurrentViewContext.RequestId.IsNullOrEmpty() && CurrentViewContext.RequestId > AppConsts.NONE)
                        {
                            Presenter.GetRequestData(CurrentViewContext.RequestId);
                        }

                        List<PlacementRequestAuditContract> auditLogs = new List<PlacementRequestAuditContract>();
                        if (!CurrentViewContext.RequestDetail.IsNullOrEmpty())
                        {
                            txtCourse.Text = CurrentViewContext.RequestDetail.Course;
                            txtCourse.Enabled = false;
                            txtNotes.Text = CurrentViewContext.RequestDetail.Notes;
                            txtNotes.Enabled = false;
                            dteStartDate.SelectedDate = CurrentViewContext.RequestDetail.StartDate;
                            dteStartDate.Enabled = false;
                            dteEndDate.SelectedDate = CurrentViewContext.RequestDetail.EndDate;
                            dteEndDate.Enabled = false;
                            txtStudents.Text = CurrentViewContext.RequestDetail.NumberOfStudents.ToString();
                            txtStudents.Enabled = false;
                            lblCreateDraft.Text = "Request Details";
                            lblStaus.Visible = true;
                            lblStaus.Text = (CurrentViewContext.IsAgencyUserLoggedIn && CurrentViewContext.RequestDetail.StatusCode == RequestStatusCodes.Requested.GetStringValue()) ? "Pending Review" : (CurrentViewContext.RequestDetail.RequestStatus.ToUpper());
                            ddlShift.DataSource = CurrentViewContext.OpportunityDetails.lstShift;
                            ddlShift.DataBind();
                            ddlShift.SelectedValue = CurrentViewContext.RequestDetail.ShiftID.ToString();
                            ddlShift.Enabled = false;
                            RequestDaySelection(CurrentViewContext.RequestDetail.lstDays);
                            lblShift.Enabled = false;
                            ddlShift.Enabled = false;

                            caCustomAttributesID.IsReadOnly = true;

                            //if is agency user then tenant label.
                            //if (CurrentViewContext.IsAgencyUserLoggedIn) edit case
                            lblInstitution.Text = CurrentViewContext.RequestDetail.InstitutionName;

                            auditLogs = Presenter.GetPlacementRequestAuditLogs(CurrentViewContext.RequestDetail.RequestID);
                        }
                        txtAuditLogs.Text = "";
                        foreach (var audit in auditLogs)
                        {
                            txtAuditLogs.Text = txtAuditLogs.Text + audit.CreatedOn.ToString() + " " + audit.ColumnName + " Old Value:" + audit.OldValue + " :: New Value:" + audit.NewValue + Environment.NewLine;
                        }
                        StatuslblColor();
                    }
                }
            }
        }

        private void StatuslblColor()
        {
            switch (CurrentViewContext.RequestDetail.StatusCode)
            {
                case "AAAA": lblStaus.ForeColor = System.Drawing.Color.FromName("#F9E79F"); break;
                case "AAAB": lblStaus.ForeColor = System.Drawing.Color.FromName("#b1e1f9"); break;
                case "AAAC": lblStaus.ForeColor = System.Drawing.Color.DarkGreen; break;
                case "AAAD": lblStaus.ForeColor = System.Drawing.Color.FromName("#ffc27e"); break;
                case "AAAE": lblStaus.ForeColor = System.Drawing.Color.FromName("#bababa"); break;
                case "AAAF": lblStaus.ForeColor = System.Drawing.Color.FromName("#ebccd1"); break;
                default: lblStaus.ForeColor = System.Drawing.Color.Black; break;

            }

        }

        private void ManageButtonVisibility()
        {
            if (CurrentViewContext.IsSharedUser)
            {
                if (!CurrentViewContext.RequestDetail.IsNullOrEmpty() && CurrentViewContext.RequestDetail.RequestID > AppConsts.NONE)
                {
                    //fsucCmdBarButton.Visible = false;
                    // for buttons Visibity dependent uppon status 
                    if (CurrentViewContext.RequestDetail.StatusCode == RequestStatusCodes.Requested.GetStringValue())//Pending Review
                    {
                        btnApprove.Visible = true;
                        btnEdit.Visible = true; // to modify request
                        //btnCancelRequest.Visible = true;
                        btnReject.Visible = true;
                        btnCancel.Visible = true;
                        btnPublish.Visible = false;
                        btnSave.Visible = false;
                        //fsucCmdBarButton.CancelButton.Visible = true;
                    }

                    if (CurrentViewContext.RequestDetail.StatusCode == RequestStatusCodes.Modified.GetStringValue())//Modified
                    {
                        //fsucCmdBarButton.Visible = true;
                        btnEdit.Visible = true;
                        btnCancel.Visible = true;
                        btnPublish.Visible = false;
                        btnSave.Visible = false;
                    }

                    if (CurrentViewContext.RequestDetail.StatusCode == RequestStatusCodes.Approved.GetStringValue())//Approved
                    {
                        btnArchive.Visible = true;
                        //btnCancelRequest.Visible = true;
                        //fsucCmdBarButton.Visible = true;
                        btnCancel.Visible = true;
                        btnPublish.Visible = false;
                        btnSave.Visible = false;
                        //fsucCmdBarButton.SubmitButton.Visible = false;
                        //fsucCmdBarButton.SaveButton.Visible = false;
                        //fsucCmdBarButton.CancelButton.Visible = true;
                    }

                    if (CurrentViewContext.RequestDetail.StatusCode == RequestStatusCodes.Rejected.GetStringValue())//Rejected
                    {
                        btnArchive.Visible = true;
                        btnCancel.Visible = true;
                        btnPublish.Visible = false;
                        btnSave.Visible = false;
                        //fsucCmdBarButton.CancelButton.Visible = true;
                    }

                    if (CurrentViewContext.RequestDetail.StatusCode == RequestStatusCodes.Archived.GetStringValue() || CurrentViewContext.RequestDetail.StatusCode == RequestStatusCodes.Cancelled.GetStringValue())//Archived or //Cancelled
                    {
                        //No Status change button will appear as the request is already cancelled or archived
                        btnCancel.Visible = true;
                        btnPublish.Visible = false;
                        btnSave.Visible = false;
                        //fsucCmdBarButton.CancelButton.Visible = true; // only can redirect back.
                    }
                }
            }
            else
            {
                // all status buttons visible false && Publish,Save and Cancel button visible =true;
                //fsucCmdBarButton.Visible = true;
                if (!CurrentViewContext.RequestDetail.IsNullOrEmpty() && CurrentViewContext.RequestDetail.RequestID > AppConsts.NONE
                            && !CurrentViewContext.RequestDetail.IsRequestPublished.IsNullOrEmpty() && CurrentViewContext.RequestDetail.IsRequestPublished)
                {
                    if (!CurrentViewContext.RequestDetail.StatusCode.IsNullOrEmpty() & (CurrentViewContext.RequestDetail.StatusCode == RequestStatusCodes.Approved.GetStringValue() || CurrentViewContext.RequestDetail.StatusCode == RequestStatusCodes.Requested.GetStringValue()))
                    {
                        btnCancelRequest.Visible = true;

                    }
                }
                else
                {
                    btnPublish.Visible = true;
                    if (CurrentViewContext.PageRequested.Equals(RequestDetails.REQUESTDETAILS.GetStringValue()))
                        btnEdit.Visible = true;
                    else
                        btnSave.Visible = true;

                }
                btnCancel.Visible = true;
            }
        }

        private void ResetButtonColor()
        {
            btnMonday.BackColor = System.Drawing.Color.LightGray;
            btnTuesday.BackColor = System.Drawing.Color.LightGray;
            btnWednesday.BackColor = System.Drawing.Color.LightGray;
            btnThursday.BackColor = System.Drawing.Color.LightGray;
            btnFriday.BackColor = System.Drawing.Color.LightGray;
            btnSaturday.BackColor = System.Drawing.Color.LightGray;
            btnSunday.BackColor = System.Drawing.Color.LightGray;
        }

        private void OpportunityDaySelection()
        {
            List<int> Days = CurrentViewContext.OpportunityDetails.lstShift.Where(id => id.ClinicalInventoryShiftID == Convert.ToInt32(ddlShift.SelectedValue)).FirstOrDefault().lstDaysId;
            foreach (Int32 dayId in Days)
            {
                switch (dayId)
                {
                    case 1:
                        {
                            btnMonday.Enabled = true;
                            btnMonday.BackColor = System.Drawing.Color.LightGreen;
                            break;
                        }
                    case 2:
                        {
                            btnTuesday.Enabled = true;
                            btnTuesday.BackColor = System.Drawing.Color.LightGreen;
                            break;
                        }
                    case 3:
                        {
                            btnWednesday.Enabled = true;
                            btnWednesday.BackColor = System.Drawing.Color.LightGreen;
                            break;
                        }
                    case 4:
                        {
                            btnThursday.Enabled = true;
                            btnThursday.BackColor = System.Drawing.Color.LightGreen;
                            break;
                        }
                    case 5:
                        {
                            btnFriday.Enabled = true;
                            btnFriday.BackColor = System.Drawing.Color.LightGreen;
                            break;
                        }
                    case 6:
                        {
                            btnSaturday.Enabled = true;
                            btnSaturday.BackColor = System.Drawing.Color.LightGreen;
                            break;
                        }
                    case 7:
                        {
                            btnSunday.Enabled = true;
                            btnSunday.BackColor = System.Drawing.Color.LightGreen;
                            break;
                        }
                }
            }
        }

        private void RequestDaySelection(List<Int32> lstdays)
        {
            CurrentViewContext.lstDays = new Dictionary<int, string>();
            foreach (var a in lstdays)
            {
                switch (a)
                {

                    case 1:
                        {

                            btnMonday.BackColor = System.Drawing.Color.LightBlue;
                            CurrentViewContext.lstDays.Add(Convert.ToInt32(DayOfWeek.Monday), "Mon");
                            break;
                        }
                    case 2:
                        {

                            btnTuesday.BackColor = System.Drawing.Color.LightBlue;
                            CurrentViewContext.lstDays.Add(Convert.ToInt32(DayOfWeek.Tuesday), "Tue");
                            break;
                        }
                    case 3:
                        {

                            btnWednesday.BackColor = System.Drawing.Color.LightBlue;
                            CurrentViewContext.lstDays.Add(Convert.ToInt32(DayOfWeek.Wednesday), "Wed");
                            break;
                        }
                    case 4:
                        {

                            btnThursday.BackColor = System.Drawing.Color.LightBlue;
                            CurrentViewContext.lstDays.Add(Convert.ToInt32(DayOfWeek.Thursday), "Thr");
                            break;
                        }
                    case 5:
                        {

                            btnFriday.BackColor = System.Drawing.Color.LightBlue;
                            CurrentViewContext.lstDays.Add(Convert.ToInt32(DayOfWeek.Friday), "Fri");
                            break;
                        }
                    case 6:
                        {

                            btnSaturday.BackColor = System.Drawing.Color.LightBlue;
                            CurrentViewContext.lstDays.Add(Convert.ToInt32(DayOfWeek.Saturday), "Sat");
                            break;
                        }
                    case 7:
                        {

                            btnSunday.BackColor = System.Drawing.Color.LightBlue;
                            CurrentViewContext.lstDays.Add(Convert.ToInt32(DayOfWeek.Saturday) + 1, "Sun");
                            break;
                        }

                }
            }

        }

        private void BindRequest(String statusCode)
        {

            CurrentViewContext.RequestDetail.Course = txtCourse.Text;
            CurrentViewContext.RequestDetail.Shift = ddlShift.Text;
            CurrentViewContext.RequestDetail.NumberOfStudents = Int32.Parse(txtStudents.Text);
            CurrentViewContext.RequestDetail.Notes = txtNotes.Text;
            CurrentViewContext.RequestDetail.StartDate = dteStartDate.SelectedDate;
            CurrentViewContext.RequestDetail.EndDate = dteEndDate.SelectedDate;
            CurrentViewContext.RequestDetail.SelectedTenantId = Int32.Parse(hdnTenantId.Value);
            CurrentViewContext.RequestDetail.DayIds = String.Join(",", CurrentViewContext.lstDays.Keys);
            CurrentViewContext.RequestDetail.Days = String.Join(", ", CurrentViewContext.lstDays.Values);
            CurrentViewContext.RequestDetail.OpportunityID = Int32.Parse(hdnOpportunityId.Value);
            CurrentViewContext.RequestDetail.StatusCode = statusCode;
            CurrentViewContext.RequestDetail.ShiftID = Convert.ToInt32(ddlShift.SelectedValue);
        }

        #endregion



        #region public Methods

        #endregion

        #endregion
    }
}