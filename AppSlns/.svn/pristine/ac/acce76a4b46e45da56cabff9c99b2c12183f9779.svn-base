using CoreWeb.ApplicantModule.Views;
using INTSOF.UI.Contract.PlacementMatching;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.PlacementMatching.Views
{
    public partial class CalenderViewControl : BaseUserControl, ICalanderView
    {
        private CalanderViewPresenter _presenter = new CalanderViewPresenter();
        public delegate void HandleView(Boolean isGridView);
        public event HandleView eventHandleView;

        public CalanderViewPresenter Presenter
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

        public ICalanderView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 ICalanderView.OpportunityId { get; set; }
        Int32 ICalanderView.RequestId { get; set; }
        public Int32 AgencyHierarchyID { get; set; }
        public String StatusCode { get; set; }
        List<RequestDetailContract> ICalanderView.lstPlacementMaching { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Presenter.GetSearchRequestData(AgencyHierarchyID,StatusCode);
                   

                }

                GetSchedularData();
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

        #region CheckBox Events

        protected void chkAgencyHierachy_CheckedChanged(object sender, EventArgs e)
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

        protected void chkShift_CheckedChanged(object sender, EventArgs e)
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

        protected void chkProgram_CheckedChanged(object sender, EventArgs e)
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

        protected void chkCourse_CheckedChanged(object sender, EventArgs e)
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
        #region Schedular Events
        protected void calenderPlacement_AppointmentClick(object sender, Telerik.Web.UI.SchedulerEventArgs e)
        {
            try
            {
                Int32 requestId = Convert.ToInt32(e.Appointment.ID.ToString());
                BindPlacementData(requestId);
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

        protected void calenderPlacement_AppointmentDataBound(object sender, SchedulerEventArgs e)
        {
            try
            {
                e.Appointment.ToolTip = e.Appointment.ToolTip.Replace("</br>", "\n");
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


        protected void fsucCmdBarButton_SaveClick()
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

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var requestDetails = RequestDetails.REQUESTDETAILS.GetStringValue();
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "SearchRequestPopUp(" + "'" + hdnRequestID.Value + "','" + hdnOpportunityID.Value + "','" + requestDetails + "');", true);
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

        protected void btnGrid_Click(object sender, EventArgs e)
        {
            try
            {
                eventHandleView(true);
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

        private void GetSchedularData()
        {
            Presenter.GetSearchRequestData(AgencyHierarchyID, StatusCode);
            List<Appointment> app = new List<Appointment>();
            foreach (var a in CurrentViewContext.lstPlacementMaching)
            {
                StringBuilder description = new StringBuilder();
                int i = 0;
                description.Append(a.Department + "</br>");
                if (chkShift.Checked)
                {
                    description.Append(a.Shift + "</br>");
                }

                if (chkAgencyHierachy.Checked)
                {
                    description.Append(a.AgencyName + "</br>");
                }
                if (chkCourse.Checked)
                {
                    description.Append(a.Course + "</br>");
                }
                Appointment app1 = new Appointment();
                app1.Description = a.Department;
                app1.ID = a.RequestID;
                app1.RecurrenceState = RecurrenceState.NotRecurring;
                app1.Start = a.StartDate.Value.Date.AddSeconds(i++);
                app1.End = app1.Start.AddHours(23);
                app1.Subject = description.ToString();
                app.Add(app1);

            }
            calenderPlacement.DataSource = app;
            calenderPlacement.DataKeyField = "ID";
            calenderPlacement.DataStartField = "Start";
            calenderPlacement.DataEndField = "End";
            calenderPlacement.DataDescriptionField = "Description";
            calenderPlacement.DataSubjectField = "Subject";
            calenderPlacement.ShowAllDayRow = true;
            calenderPlacement.ShowHoursColumn = false;
            calenderPlacement.ShowFooter = false;
            BindPlacementData(CurrentViewContext.lstPlacementMaching.Select(a => a.RequestID).FirstOrDefault());
        }

        public void BindPlacementData(Int32 requestID)
        {
            RequestDetailContract request = new RequestDetailContract();

            request = CurrentViewContext.lstPlacementMaching.Where(a => a.RequestID == requestID).FirstOrDefault();
            if (request != null)
            {
                btnEdit.Visible = true;
                lblDays.Text = request.Days;
                lblDepartment.Text = request.Department;
                lblInstitution.Text = request.InstitutionName;
                lblLocation.Text = request.Location;
                lblShift.Text = request.Shift;
                lblSpecialty.Text = request.Specialty;
                lblStudentType.Text = request.StudentTypes;
                lblMax.Text = request.Max.ToString();
                lblUnit.Text = request.Unit;
                lblIsPreceptonship.Text = request.IsPreceptonship.ToString();
                lblFloatArea.Text = request.ContainsFloatArea;
                lblInstitution.Text = request.InstitutionName;
                lblDates.Text = request.OpportunityStartDate.Value.ToShortDateString() + " - " + request.OpportunityEndDate.Value.ToShortDateString();
                // lblMax1.Text = lblMax.Text;
                // lblUnit1.Text = lblUnit.Text;
                hdnRequestID.Value = requestID.ToString();
                hdnOpportunityID.Value = request.OpportunityID.ToString();
            }
            else
            {
    
                lblDays.Text = String.Empty;
                lblDepartment.Text = String.Empty;
                lblInstitution.Text = String.Empty;
                lblLocation.Text = String.Empty;
                lblShift.Text = String.Empty;
                lblSpecialty.Text = String.Empty;
                lblStudentType.Text = String.Empty;
                lblMax.Text = String.Empty;
                lblUnit.Text = String.Empty;
                lblIsPreceptonship.Text = String.Empty;
                lblFloatArea.Text = String.Empty;
                lblDates.Text = String.Empty;
                // lblMax1.Text = lblMax.Text;
                // lblUnit1.Text = lblUnit.Text;
                hdnRequestID.Value = String.Empty;
                hdnOpportunityID.Value = String.Empty;
                btnEdit.Visible = false;
            }
        }


    }
}