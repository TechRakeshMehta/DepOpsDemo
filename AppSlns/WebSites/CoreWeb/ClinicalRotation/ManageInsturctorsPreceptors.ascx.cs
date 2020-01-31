using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using System.Web.UI.HtmlControls;
using INTSOF.UI.Contract.SysXSecurityModel;
using System.Web;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class ManageInsturctorsPreceptors : BaseUserControl, IManageInsturctorsPreceptorsView
    {
        #region VARIABLES

        #region PRIVATE VARIABLES

        private ManageInsturctorsPreceptorsPresenter _presenter = new ManageInsturctorsPreceptorsPresenter();
        private Int32 tenantId = 0;
        private Boolean _editFlag = false;
        #endregion

        #region PUBLIC VARIABLES

        #endregion

        #endregion

        #region PROPERTIES

        public ManageInsturctorsPreceptorsPresenter Presenter
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

        public IManageInsturctorsPreceptorsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        List<TenantDetailContract> IManageInsturctorsPreceptorsView.LstTenants
        {
            get;
            set;
        }

        Boolean IManageInsturctorsPreceptorsView.IsReset
        {
            get;
            set;
        }

        Int32 IManageInsturctorsPreceptorsView.TenantID
        {
            get
            {
                if (tenantId == 0)
                {
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

        public Int32 IsAccountActivated
        {
            get
            {
                return Convert.ToInt32(rbAccountActivated.SelectedValue);
            }
        }

        Int32 IManageInsturctorsPreceptorsView.CurrentLoggedInUserID
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Boolean IManageInsturctorsPreceptorsView.IsAdminLoggedIn
        {
            get;
            set;
        }

        Int32 IManageInsturctorsPreceptorsView.SelectedTenantID
        {
            get
            {
                if (String.IsNullOrEmpty(cmbTenant.SelectedValue))
                    return 0;
                return Convert.ToInt32(cmbTenant.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    cmbTenant.SelectedValue = Convert.ToString(value);
                }
                else
                {
                    cmbTenant.SelectedIndex = value;
                }
            }
        }

        List<SharedSystemDocTypeContract> IManageInsturctorsPreceptorsView.DocumentTypeList
        {
            get
            {
                if (!ViewState["DocumentTypeList"].IsNull())
                {
                    return (List<SharedSystemDocTypeContract>)(ViewState["DocumentTypeList"]);
                }

                return new List<SharedSystemDocTypeContract>();
            }
            set
            {
                ViewState["DocumentTypeList"] = value;
            }
        }

        //UAT 1426: WB: As an admin or Instructor/preceptor, I should be able to upload multiple licences for an instructor/preceptor
        //List<SharedSystemDocTypeContract> IManageInsturctorsPreceptorsView.DocumentTypeListTemp
        //{
        //    get
        //    {
        //        if (!ViewState["DocumentTypeListTemp"].IsNull())
        //        {
        //            return (List<SharedSystemDocTypeContract>)(ViewState["DocumentTypeListTemp"]);
        //        }

        //        return new List<SharedSystemDocTypeContract>();
        //    }
        //    set
        //    {
        //        ViewState["DocumentTypeListTemp"] = value;
        //    }
        //}

        List<SharedSystemDocumentContract> IManageInsturctorsPreceptorsView.UploadedDocumentList
        {
            get
            {
                if (!ViewState["UploadedDocumentList"].IsNull())
                {
                    return (List<SharedSystemDocumentContract>)(ViewState["UploadedDocumentList"]);
                }

                return new List<SharedSystemDocumentContract>();
            }
            set
            {
                ViewState["UploadedDocumentList"] = value;
            }
        }

        List<WeekDayContract> IManageInsturctorsPreceptorsView.WeekDayList
        {
            get;
            set;
        }

        List<ClientContactTypeContract> IManageInsturctorsPreceptorsView.ClientContactTypeList
        {
            get
            {
                if (!ViewState["ClientContactTypeList"].IsNull())
                {
                    return (List<ClientContactTypeContract>)(ViewState["ClientContactTypeList"]);
                }

                return new List<ClientContactTypeContract>();
            }
            set
            {
                ViewState["ClientContactTypeList"] = value;
            }
        }

        ClientContactContract IManageInsturctorsPreceptorsView.ClientContact
        {
            get;
            set;
        }

        List<ClientContactContract> IManageInsturctorsPreceptorsView.ClientContactList
        {
            get
            {
                if (!ViewState["ClientContactList"].IsNull())
                {
                    return (List<ClientContactContract>)(ViewState["ClientContactList"]);
                }

                return new List<ClientContactContract>();
            }
            set
            {
                ViewState["ClientContactList"] = value;
            }
        }

        ClientContactAvailibiltyContract IManageInsturctorsPreceptorsView.ClientAvailibiltyContact
        {
            get;
            set;
        }

        Int32 IManageInsturctorsPreceptorsView.ClientContactID
        {
            get;
            set;
        }

        Boolean IManageInsturctorsPreceptorsView.SuccussMessage
        {
            get;
            set;
        }

        List<ClientContactAvailibiltyContract> IManageInsturctorsPreceptorsView.ClientAvailibiltyContactList
        {
            get;
            set;
        }

        List<Int32> DocumentUploadedHistory
        {
            get
            {
                if (!ViewState["DocumentUploadedHistory"].IsNull())
                {
                    return (List<Int32>)(ViewState["DocumentUploadedHistory"]);
                }
                return new List<Int32>();
            }
            set
            {
                ViewState["DocumentUploadedHistory"] = value;
            }
        }

        AppSettingContract IManageInsturctorsPreceptorsView.AppSettingContract
        {
            get;
            set;
        }

        String IManageInsturctorsPreceptorsView.ClientContactEmailID { get; set; }

        Guid? IManageInsturctorsPreceptorsView.AspNetUserID { get; set; }

        Boolean IManageInsturctorsPreceptorsView.IsClientContactAllowedToDelete { get; set; }

        #region UAT-4239
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        /// <remarks></remarks>
        String IManageInsturctorsPreceptorsView.Password
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region EVENTS

        #region PAGE EVENTS

        /// <summary>
        /// Page OnInit Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Manage Instructor/Preceptors";
                base.SetPageTitle("Manage Instructor/Preceptors");
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
                    BindControls();
                    HandleClientContactGridVisibility();
                }
                Presenter.OnViewLoaded();

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

        #region DROPDOWN EVENTS

        protected void cmbTenant_DataBound(object sender, EventArgs e)
        {
            cmbTenant.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        //protected void cmbAvailableDays_DataBound(object sender, EventArgs e)
        //{
        //    //WclComboBox cmbAvailableDays = sender as WclComboBox;
        //    //cmbAvailableDays.Items.Insert(0, new RadComboBoxItem("--Select--"));
        //}

        protected void cmbUserType_DataBound(object sender, EventArgs e)
        {
            WclComboBox cmbUserType = sender as WclComboBox;
            cmbUserType.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void cmbDocumentType_DataBound(object sender, EventArgs e)
        {
            WclComboBox cmbDocumentType = sender as WclComboBox;
            cmbDocumentType.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        #endregion

        #region GRID EVENTS

        #region CLIENT CONTACT GRID (INSTRUCTOR/PERCEPTORS)

        protected void grdInstrctrPreceptr_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.IsAdminLoggedIn();
                if (!CurrentViewContext.IsAdminLoggedIn && CurrentViewContext.IsReset)
                {
                    CurrentViewContext.ClientContactList = new List<ClientContactContract>();
                }
                else
                    Presenter.GetClientContacts();
                grdInstrctrPreceptr.DataSource = CurrentViewContext.ClientContactList;
                grdInstrctrPreceptr.Columns.FindByUniqueName("LoginAsInstructor").Visible = CurrentViewContext.IsAdminLoggedIn;
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

        protected void grdInstrctrPreceptr_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {

                //Reset
                DocumentUploadedHistory = new List<Int32>();
                CurrentViewContext.UploadedDocumentList = new List<SharedSystemDocumentContract>();

                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                //Bind WeekDays
                WclComboBox cmbAvailableDays = gridEditableItem.FindControl("cmbAvailableDays") as WclComboBox;
                Presenter.GetWeekDays();
                cmbAvailableDays.DataSource = CurrentViewContext.WeekDayList;
                cmbAvailableDays.DataBind();

                //Bind Document Type
                WclComboBox cmbUserType = gridEditableItem.FindControl("cmbUserType") as WclComboBox;
                Presenter.GetClientContactTypeList();
                cmbUserType.DataSource = CurrentViewContext.ClientContactTypeList;
                cmbUserType.DataBind();

                //Bind Uploaded Document Grid
                WclGrid grdUploadDocuments = gridEditableItem.FindControl("grdUploadDocuments") as WclGrid;
                grdUploadDocuments.DataSource = new List<SharedSystemDocumentContract>();
                grdUploadDocuments.DataBind();



                //Edit Mode
                if (_editFlag)
                {
                    WclTextBox txtEmail = (WclTextBox)gridEditableItem.FindControl("txtEmail");
                    txtEmail.Enabled = false;
                    CurrentViewContext.ClientContactID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ClientContactID"));
                    cmbUserType.SelectedValue = Convert.ToString(CurrentViewContext.ClientContactList.Where(x => x.ClientContactID == CurrentViewContext.ClientContactID).Select(x => x.ClientContactTypeID).FirstOrDefault());
                    Presenter.GetClientContactAvailibilty();
                    if (!CurrentViewContext.ClientAvailibiltyContactList.IsNullOrEmpty())
                    {
                        HtmlGenericControl dvAvailabilityTime = cmbAvailableDays.Parent.NamingContainer.FindControl("dvAvailabilityTime") as HtmlGenericControl;
                        dvAvailabilityTime.Visible = true;
                        #region UAT-1874
                        WclTimePicker tpStartTimeMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeMonday") as WclTimePicker;
                        WclTimePicker tpEndTimeMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeMonday") as WclTimePicker;
                        Label lblMondayTime_CCA_ID = cmbAvailableDays.Parent.NamingContainer.FindControl("lblMondayTime_CCA_ID") as Label;
                        WclTimePicker tpStartTimeTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeTuesday") as WclTimePicker;
                        WclTimePicker tpEndTimeTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeTuesday") as WclTimePicker;
                        Label lblTuesdayTime_CCA_ID = cmbAvailableDays.Parent.NamingContainer.FindControl("lblTuesdayTime_CCA_ID") as Label;
                        WclTimePicker tpStartTimeWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeWednesday") as WclTimePicker;
                        WclTimePicker tpEndTimeWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeWednesday") as WclTimePicker;
                        Label lblWednesdayTime_CCA_ID = cmbAvailableDays.Parent.NamingContainer.FindControl("lblWednesdayTime_CCA_ID") as Label;
                        WclTimePicker tpStartTimeThrusday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeThrusday") as WclTimePicker;
                        WclTimePicker tpEndTimeThrusday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeThrusday") as WclTimePicker;
                        Label lblThursdayTime_CCA_ID = cmbAvailableDays.Parent.NamingContainer.FindControl("lblThursdayTime_CCA_ID") as Label;
                        WclTimePicker tpStartTimeFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeFriday") as WclTimePicker;
                        WclTimePicker tpEndTimeFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeFriday") as WclTimePicker;
                        Label lblFridayTime_CCA_ID = cmbAvailableDays.Parent.NamingContainer.FindControl("lblFridayTime_CCA_ID") as Label;
                        WclTimePicker tpStartTimeSat = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeSat") as WclTimePicker;
                        WclTimePicker tpEndTimeSat = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeSat") as WclTimePicker;
                        Label lblSaturdayTime_CCA_ID = cmbAvailableDays.Parent.NamingContainer.FindControl("lblSaturdayTime_CCA_ID") as Label;
                        WclTimePicker tpStartTimeSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeSunday") as WclTimePicker;
                        WclTimePicker tpEndTimeSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeSunday") as WclTimePicker;
                        Label lblSundayTime_CCA_ID = cmbAvailableDays.Parent.NamingContainer.FindControl("lblSundayTime_CCA_ID") as Label;

                        RequiredFieldValidator rfvtpStartTimeMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeMonday") as RequiredFieldValidator;
                        RequiredFieldValidator rfvtpEndTimeMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeMonday") as RequiredFieldValidator;
                        HtmlGenericControl dvMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("dvMonday") as HtmlGenericControl;

                        RequiredFieldValidator rfvtpStartTimeTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeTuesday") as RequiredFieldValidator;
                        RequiredFieldValidator rfvtpEndTimeTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeTuesday") as RequiredFieldValidator;
                        HtmlGenericControl dvTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("dvTuesday") as HtmlGenericControl;

                        RequiredFieldValidator rfvtpStartTimeWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeWednesday") as RequiredFieldValidator;
                        RequiredFieldValidator rfvtpEndTimeWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeWednesday") as RequiredFieldValidator;
                        HtmlGenericControl dvWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("dvWednesday") as HtmlGenericControl;

                        RequiredFieldValidator rfvtpStartTimeThrusday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeThrusday") as RequiredFieldValidator;
                        RequiredFieldValidator rfvtpEndTimeThrusday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeThrusday") as RequiredFieldValidator;
                        HtmlGenericControl dvThr = cmbAvailableDays.Parent.NamingContainer.FindControl("dvThr") as HtmlGenericControl;

                        RequiredFieldValidator rfvtpStartTimeFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeFriday") as RequiredFieldValidator;
                        RequiredFieldValidator rfvtpEndTimeFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeFriday") as RequiredFieldValidator;
                        HtmlGenericControl dvFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("dvFriday") as HtmlGenericControl;

                        RequiredFieldValidator rfvtpStartTimeSat = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeSat") as RequiredFieldValidator;
                        RequiredFieldValidator rfvtpEndTimeSat = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeSat") as RequiredFieldValidator;
                        HtmlGenericControl dvSat = cmbAvailableDays.Parent.NamingContainer.FindControl("dvSat") as HtmlGenericControl;

                        RequiredFieldValidator rfvtpStartTimeSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeSunday") as RequiredFieldValidator;
                        RequiredFieldValidator rfvtpEndTimeSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeSunday") as RequiredFieldValidator;
                        HtmlGenericControl dvSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("dvSunday") as HtmlGenericControl;

                        var mondayDetails = CurrentViewContext.ClientAvailibiltyContactList.Where(s => s.WeekDayID == AppConsts.ONE).FirstOrDefault();
                        var tuesdayDetails = CurrentViewContext.ClientAvailibiltyContactList.Where(s => s.WeekDayID == AppConsts.TWO).FirstOrDefault();
                        var wednesdayDetails = CurrentViewContext.ClientAvailibiltyContactList.Where(s => s.WeekDayID == AppConsts.THREE).FirstOrDefault();
                        var thrusdayDetails = CurrentViewContext.ClientAvailibiltyContactList.Where(s => s.WeekDayID == AppConsts.FOUR).FirstOrDefault();
                        var fridayDetails = CurrentViewContext.ClientAvailibiltyContactList.Where(s => s.WeekDayID == AppConsts.FIVE).FirstOrDefault();
                        var sataurdayDetails = CurrentViewContext.ClientAvailibiltyContactList.Where(s => s.WeekDayID == AppConsts.SIX).FirstOrDefault();
                        var sundayDetails = CurrentViewContext.ClientAvailibiltyContactList.Where(s => s.WeekDayID == AppConsts.SEVEN).FirstOrDefault();

                        //Monday
                        if (!mondayDetails.IsNullOrEmpty())
                        {
                            ManageControlVisibility(tpStartTimeMonday, tpEndTimeMonday, rfvtpStartTimeMonday, rfvtpEndTimeMonday, dvMonday, true);
                            tpStartTimeMonday.Visible = true;
                            tpStartTimeMonday.SelectedTime = new TimeSpan(mondayDetails.StartTime.Value.Hour, mondayDetails.StartTime.Value.Minute, mondayDetails.StartTime.Value.Second);
                            tpEndTimeMonday.SelectedTime = new TimeSpan(mondayDetails.EndTime.Value.Hour, mondayDetails.EndTime.Value.Minute, mondayDetails.EndTime.Value.Second);
                            lblMondayTime_CCA_ID.Text = Convert.ToString(mondayDetails.ClientContactAvailibiltyID);
                        }
                        else
                        {
                            tpStartTimeMonday.SelectedTime = null;
                            tpEndTimeMonday.SelectedTime = null;
                            ManageControlVisibility(tpStartTimeMonday, tpEndTimeMonday, rfvtpStartTimeMonday, rfvtpEndTimeMonday, dvMonday, false);
                        }

                        if (!tuesdayDetails.IsNullOrEmpty())
                        {
                            //Tuesday
                            ManageControlVisibility(tpStartTimeTuesday, tpEndTimeTuesday, rfvtpStartTimeTuesday, rfvtpEndTimeTuesday, dvTuesday, true);
                            tpStartTimeTuesday.SelectedTime = new TimeSpan(tuesdayDetails.StartTime.Value.Hour, tuesdayDetails.StartTime.Value.Minute, tuesdayDetails.StartTime.Value.Second);
                            tpEndTimeTuesday.SelectedTime = new TimeSpan(tuesdayDetails.EndTime.Value.Hour, tuesdayDetails.EndTime.Value.Minute, tuesdayDetails.EndTime.Value.Second);
                            lblTuesdayTime_CCA_ID.Text = Convert.ToString(tuesdayDetails.ClientContactAvailibiltyID);

                        }
                        else
                        {
                            tpStartTimeTuesday.SelectedTime = null;
                            tpEndTimeTuesday.SelectedTime = null;
                            ManageControlVisibility(tpStartTimeTuesday, tpEndTimeTuesday, rfvtpStartTimeTuesday, rfvtpEndTimeTuesday, dvTuesday, false);
                        }

                        //Wednesday
                        if (!wednesdayDetails.IsNullOrEmpty())
                        {
                            ManageControlVisibility(tpStartTimeWednesday, tpEndTimeWednesday, rfvtpStartTimeWednesday, rfvtpEndTimeWednesday, dvWednesday, true);
                            tpStartTimeWednesday.SelectedTime = new TimeSpan(wednesdayDetails.StartTime.Value.Hour, wednesdayDetails.StartTime.Value.Minute, wednesdayDetails.StartTime.Value.Second);
                            tpEndTimeWednesday.SelectedTime = new TimeSpan(wednesdayDetails.EndTime.Value.Hour, wednesdayDetails.EndTime.Value.Minute, wednesdayDetails.EndTime.Value.Second);
                            lblWednesdayTime_CCA_ID.Text = Convert.ToString(wednesdayDetails.ClientContactAvailibiltyID);
                        }
                        else
                        {
                            tpStartTimeWednesday.SelectedTime = null;
                            tpEndTimeWednesday.SelectedTime = null;
                            ManageControlVisibility(tpStartTimeWednesday, tpEndTimeWednesday, rfvtpStartTimeWednesday, rfvtpEndTimeWednesday, dvWednesday, false);
                        }
                        //Thrusday
                        if (!thrusdayDetails.IsNullOrEmpty())
                        {
                            ManageControlVisibility(tpStartTimeThrusday, tpEndTimeThrusday, rfvtpStartTimeThrusday, rfvtpEndTimeThrusday, dvThr, true);
                            tpStartTimeThrusday.SelectedTime = new TimeSpan(thrusdayDetails.StartTime.Value.Hour, thrusdayDetails.StartTime.Value.Minute, thrusdayDetails.StartTime.Value.Second);
                            tpEndTimeThrusday.SelectedTime = new TimeSpan(thrusdayDetails.EndTime.Value.Hour, thrusdayDetails.EndTime.Value.Minute, thrusdayDetails.EndTime.Value.Second);
                            lblThursdayTime_CCA_ID.Text = Convert.ToString(thrusdayDetails.ClientContactAvailibiltyID);
                        }
                        else
                        {
                            tpStartTimeThrusday.SelectedTime = null;
                            tpEndTimeThrusday.SelectedTime = null;
                            ManageControlVisibility(tpStartTimeThrusday, tpEndTimeThrusday, rfvtpStartTimeThrusday, rfvtpEndTimeThrusday, dvThr, false);
                        }
                        //Friday
                        if (!fridayDetails.IsNullOrEmpty())
                        {
                            ManageControlVisibility(tpStartTimeFriday, tpEndTimeFriday, rfvtpStartTimeFriday, rfvtpEndTimeFriday, dvFriday, true);
                            tpStartTimeFriday.SelectedTime = new TimeSpan(fridayDetails.StartTime.Value.Hour, fridayDetails.StartTime.Value.Minute, fridayDetails.StartTime.Value.Second);
                            tpEndTimeFriday.SelectedTime = new TimeSpan(fridayDetails.EndTime.Value.Hour, fridayDetails.EndTime.Value.Minute, fridayDetails.EndTime.Value.Second);
                            lblFridayTime_CCA_ID.Text = Convert.ToString(fridayDetails.ClientContactAvailibiltyID);
                        }
                        else
                        {
                            tpStartTimeFriday.SelectedTime = null;
                            tpEndTimeFriday.SelectedTime = null;
                            ManageControlVisibility(tpStartTimeFriday, tpEndTimeFriday, rfvtpStartTimeFriday, rfvtpEndTimeFriday, dvFriday, false);
                        }
                        //sataurday
                        if (!sataurdayDetails.IsNullOrEmpty())
                        {
                            ManageControlVisibility(tpStartTimeSat, tpEndTimeSat, rfvtpStartTimeSat, rfvtpEndTimeSat, dvSat, true);
                            tpStartTimeSat.SelectedTime = new TimeSpan(sataurdayDetails.StartTime.Value.Hour, sataurdayDetails.StartTime.Value.Minute, sataurdayDetails.StartTime.Value.Second);
                            tpEndTimeSat.SelectedTime = new TimeSpan(sataurdayDetails.EndTime.Value.Hour, sataurdayDetails.EndTime.Value.Minute, sataurdayDetails.EndTime.Value.Second);
                            lblSaturdayTime_CCA_ID.Text = Convert.ToString(sataurdayDetails.ClientContactAvailibiltyID);
                        }
                        else
                        {
                            tpStartTimeSat.SelectedTime = null;
                            tpEndTimeSat.SelectedTime = null;
                            ManageControlVisibility(tpStartTimeSat, tpEndTimeSat, rfvtpStartTimeSat, rfvtpEndTimeSat, dvSat, false);
                        }
                        //sundayDetails
                        if (!sundayDetails.IsNullOrEmpty())
                        {
                            ManageControlVisibility(tpStartTimeSunday, tpEndTimeSunday, rfvtpStartTimeSunday, rfvtpEndTimeSunday, dvSunday, true);
                            tpStartTimeSunday.SelectedTime = new TimeSpan(sundayDetails.StartTime.Value.Hour, sundayDetails.StartTime.Value.Minute, sundayDetails.StartTime.Value.Second);
                            tpEndTimeSunday.SelectedTime = new TimeSpan(sundayDetails.EndTime.Value.Hour, sundayDetails.EndTime.Value.Minute, sundayDetails.EndTime.Value.Second);
                            lblSundayTime_CCA_ID.Text = Convert.ToString(sundayDetails.ClientContactAvailibiltyID);
                        }
                        else
                        {
                            tpStartTimeSunday.SelectedTime = null;
                            tpEndTimeSunday.SelectedTime = null;
                            ManageControlVisibility(tpStartTimeSunday, tpEndTimeSunday, rfvtpStartTimeSunday, rfvtpEndTimeSunday, dvSunday, false);
                        }
                        #endregion
                        //WclTimePicker tpStartTime = (WclTimePicker)gridEditableItem.FindControl("tpStartTime");
                        //WclTimePicker tpEndTime = (WclTimePicker)gridEditableItem.FindControl("tpEndTime");
                        //if (CurrentViewContext.ClientAvailibiltyContactList[AppConsts.NONE].StartTime.Value.Hour != 0 || CurrentViewContext.ClientAvailibiltyContactList[AppConsts.NONE].StartTime.Value.Minute != 0 || CurrentViewContext.ClientAvailibiltyContactList[AppConsts.NONE].StartTime.Value.Second != 0)
                        //{
                        //    tpStartTime.SelectedTime = new TimeSpan(CurrentViewContext.ClientAvailibiltyContactList[AppConsts.NONE].StartTime.Value.Hour, CurrentViewContext.ClientAvailibiltyContactList[AppConsts.NONE].StartTime.Value.Minute, CurrentViewContext.ClientAvailibiltyContactList[AppConsts.NONE].StartTime.Value.Second);
                        //    tpEndTime.SelectedTime = new TimeSpan(CurrentViewContext.ClientAvailibiltyContactList[AppConsts.NONE].EndTime.Value.Hour, CurrentViewContext.ClientAvailibiltyContactList[AppConsts.NONE].EndTime.Value.Minute, CurrentViewContext.ClientAvailibiltyContactList[AppConsts.NONE].EndTime.Value.Second);
                        //}
                        //else
                        //{
                        //    tpStartTime.SelectedTime = null;
                        //    tpEndTime.SelectedTime = null;
                        //}
                        List<Int32> selectedWeekDayIds = CurrentViewContext.ClientAvailibiltyContactList.Select(x => x.WeekDayID).ToList();
                        foreach (RadComboBoxItem item in cmbAvailableDays.Items)
                        {
                            if (selectedWeekDayIds.Contains(Convert.ToInt32(item.Value)))
                                item.Checked = true;
                        }
                        //UAT-2447
                        if (!e.Item.DataItem.IsNullOrEmpty())
                        {
                            ClientContactContract data = e.Item.DataItem as ClientContactContract;
                            ShowHidePhoneControls(data.IsInternationalPhone, e.Item as GridEditFormItem);
                        }
                    }
                    Presenter.GetClientContactDocument();

                    //DocumentUploadedHistory = CurrentViewContext.UploadedDocumentList.Select(x => x.DocumentTypeID).ToList();
                    ShowHideUploadDocumentButton(grdUploadDocuments);
                    grdUploadDocuments.DataSource = CurrentViewContext.UploadedDocumentList;
                    grdUploadDocuments.DataBind();

                    //Fill the ClientOntact
                }
                else //Add New
                {
                    //UAT-2447
                    ShowHidePhoneControls(false, e.Item as GridEditFormItem);
                }
            }
            //UAT-4043
            if (e.Item is GridDataItem)
            {
                RadButton btnReSendActivationInvitationLink = ((RadButton)e.Item.FindControl("btnReSendActivationInvitationLink"));
                RadButton btnLoginAsInstructor = ((RadButton)e.Item.FindControl("btnLoginAsInstructor"));
                RadButton btnResetInstructorPassword = ((RadButton)e.Item.FindControl("btnResetInstructorPassword")); //UAT-4239

                //UAT-4160
                GridDataItem dataItem = e.Item as GridDataItem;
                Boolean IsRegistered = false;

                IsRegistered = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsRegistered"]);
                if (IsRegistered)
                {
                    btnLoginAsInstructor.Visible = true;
                    btnReSendActivationInvitationLink.Visible = false;
                    btnResetInstructorPassword.Visible = true;//UAT-4239

                    // dataItem["DeleteColumn"].Controls[0].Visible = false;                    
                }
                else
                {
                    btnLoginAsInstructor.Visible = false;
                    btnReSendActivationInvitationLink.Visible = true;
                    btnResetInstructorPassword.Visible = false;//UAT-4239

                    //grdInstrctrPreceptr.Columns.FindByUniqueName("DeleteColumn").Display = true;
                    // dataItem["DeleteColumn"].Controls[0].Visible = true;
                }
                //Below code is commented in UAT-4160
                //GridDataItem dataItem = e.Item as GridDataItem;
                //if (Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"]).IsNullOrEmpty())
                //{
                //    btnReSendActivationInvitationLink.Visible = true;
                //    btnLoginAsInstructor.Visible = false;
                //}
                //else
                //{
                //    btnReSendActivationInvitationLink.Visible = false;
                //    btnLoginAsInstructor.Visible = true;
                //}
            }
        }

        protected void grdInstrctrPreceptr_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                //Delete all temporary uploaded documents if user cancelled the operation.
                if (e.CommandName.Equals("Cancel"))
                {
                    DocumentUploadedHistory = new List<Int32>(); //Need this list only on Edit and Add mode else Reset it.
                    if (!CurrentViewContext.UploadedDocumentList.IsNullOrEmpty())
                    {
                        foreach (SharedSystemDocumentContract document in CurrentViewContext.UploadedDocumentList)
                        {
                            //Delete only if present in temp location (DocumentID should be 0 as it is not saved on DB yet)
                            if (document.DocumentID == AppConsts.NONE)
                            {
                                try
                                {
                                    if (!String.IsNullOrEmpty(document.DocumentPath))
                                        File.Delete(document.DocumentPath);
                                }
                                catch (Exception) { }
                            }
                        }
                    }
                }
                if (e.CommandName.Equals("Edit"))
                {
                    _editFlag = true;
                }

                #region UAT-4043

                if (e.CommandName.Equals("ResendActivationLink"))
                {
                     Int32 clientContactID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ClientContactID"]);
                    if (!CurrentViewContext.ClientContactList.IsNullOrEmpty() && !clientContactID.IsNullOrEmpty() && clientContactID > AppConsts.NONE)
                    {

                        ClientContactContract clientContactToResendActivationMail = CurrentViewContext.ClientContactList.Where(con => con.ClientContactID == clientContactID).FirstOrDefault();
                        if (!clientContactToResendActivationMail.IsNullOrEmpty())
                        {
                            if (Presenter.ResendInstructorActivationMail(clientContactToResendActivationMail))
                            {
                                ShowSuccessMessage("Invitation mail has been sent to client contact successfully.");
                            }
                            else
                            {
                                ShowErrorMessage("Invitation mail has not been sent to client contact.");
                            }
                        }
                    }
                }

                #endregion

                #region UAT-4120

                if (e.CommandName.Equals("LoginAsInstructor"))
                {
                    String applicantUserID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"].ToString();
                    SwitchToInstuctorOrPreceptor(applicantUserID);
                }

                #endregion

                #region UAT-4239 : Ability for ADB to reset instructor password
                if (e.CommandName.Equals("ResetInstructorPassword"))
                {
                    String clientContactUserId = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"]);
                    if (!clientContactUserId.IsNullOrEmpty())
                    {
                        CurrentViewContext.Password = radCpatchaPassword.CaptchaImage.Text;
                        Entity.OrganizationUser orgUser = Presenter.GetOrganizationUser(clientContactUserId);
                        if (!orgUser.IsNullOrEmpty())
                        {
                            if (Presenter.ResetPassword(orgUser))
                            {
                                base.ShowSuccessMessage(SysXUtils.GetMessage(ResourceConst.SECURITY_PASSWORD_SEND_SUCEESFULLY));
                            }
                        }
                        else
                        {
                            base.ShowErrorMessage("Password did not reset succesfully.");
                        }
                    }
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

        private void SwitchToInstuctorOrPreceptor(String organizationUserID)
        {
            String switchingTargetURL = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL].IsNullOrEmpty()
                                                                            ? String.Empty
                                                                            : Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);
            if (!(switchingTargetURL.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || switchingTargetURL.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            {
                if (HttpContext.Current != null)
                {
                    switchingTargetURL = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", switchingTargetURL.Trim());
                }
                else
                {
                    switchingTargetURL = string.Concat("http://", switchingTargetURL.Trim());
                }
            }
            RedirectToTargetSwitchingView(organizationUserID, switchingTargetURL);
        }


        /// <summary>
        /// Method To create/update WebApplicationData, Redirect to Target applicant View.
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="switchingTargetURL"></param>
        private void RedirectToTargetSwitchingView(String UserID, String switchingTargetURL)
        {
            Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
            ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
            appInstData.UserID = UserID;
            appInstData.TagetInstURL = switchingTargetURL;
            appInstData.TokenCreatedTime = DateTime.Now;
            appInstData.UserTypeSwitchViewCode = UserTypeSwitchView.InstructorOrPreceptor.GetStringValue();
            appInstData.AdminOrgUserID = CurrentViewContext.CurrentLoggedInUserID;
            String key = Guid.NewGuid().ToString();

            Dictionary<String, ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
            if (applicationData != null)
            {
                applicantData = applicationData;
                applicantData.Add(key, appInstData);
                Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
            }
            else
            {
                applicantData.Add(key, appInstData);
                Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
            }

            //Log out from application then redirect to selected tenant url, append key in querystring.
            // On login page get data from Application Variable.
            //Presenter.DoLogOff(true);
            Presenter.AddImpersonationHistory(UserID, CurrentViewContext.CurrentLoggedInUserID);
            //Redirect to login page
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TokenKey", key  }
                                                                 };

            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenInstructorView('" + String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}&DeletePrevUsrState=true", key) + "');", true);
            //Response.Redirect(String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}", key));
        }

        protected void grdInstrctrPreceptr_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
                {
                    GridEditFormItem item = e.Item as GridEditFormItem;
                    WclTextBox txtEmail = (WclTextBox)item.FindControl("txtEmail");
                    if (Presenter.IsEmailAlreadyExistForTenant(txtEmail.Text))
                    {
                        //ShowInfoMessage("Instructor/Preceptor with same email is already exist.");
                        ShowInfoMessage("Instructor/Preceptor with same email already exists."); //UAT-4139
                        e.Canceled = true;
                        return;
                    }
                    if (!CurrentViewContext.UploadedDocumentList.IsNullOrEmpty())
                    {
                        //1.Move the temporary uploaded documents to Amazon S3 or FileSystem
                        UploadFileToS3OrFileSystem();
                    }
                    FillClientContactContract(e);
                    Presenter.SaveClientContact();
                    if (CurrentViewContext.SuccussMessage)
                    {
                        ShowSuccessMessage("Client Contact created successfully.");
                    }
                }
                else
                {
                    ShowInfoMessage("Please select institution.");
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

        protected void grdInstrctrPreceptr_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                //TO DO
                CurrentViewContext.ClientContactID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ClientContactID"]);
                Presenter.IsClientContactAllowedToDelete();

                if (CurrentViewContext.IsClientContactAllowedToDelete)
                {
                    Presenter.DeleteClientContact();
                }
                else
                {
                    //ShowInfoMessage("You cannot delete this Instructor/Preceptor, this is already in use.");
                    ShowInfoMessage("You cannot delete an Instructor/Preceptor who is assigned to a rotation."); // UAT-4139
                }

                if (CurrentViewContext.SuccussMessage)
                    ShowSuccessMessage("Client Contact deleted successfully.");
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

        protected void grdInstrctrPreceptr_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (!CurrentViewContext.UploadedDocumentList.IsNullOrEmpty())
                {
                    //1.Move the temporary uploaded documents to Amazon S3 or FileSystem
                    UploadFileToS3OrFileSystem();
                }
                FillClientContactContract(e, true);
                Presenter.UpdateClientContact();
                if (CurrentViewContext.SuccussMessage)
                {
                    ShowSuccessMessage("Client Contact updated successfully.");
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

        #region DOCUMENT UPLOAD GRID
        protected void grdUploadDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                WclGrid grdUploadDocuments = (WclGrid)sender;
                Presenter.GetClientContactDocument();
                grdUploadDocuments.DataSource = CurrentViewContext.UploadedDocumentList;
                //UAT 1426: WB: As an admin or Instructor/preceptor, I should be able to upload multiple licences for an instructor/preceptor
                //HandleDocumentTypeDropdown();
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

        protected void grdUploadDocuments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                //Bind Document Type
                WclComboBox cmbDocumentType = gridEditableItem.FindControl("cmbDocumentType") as WclComboBox;
                cmbDocumentType.DataSource = CurrentViewContext.DocumentTypeList;
                cmbDocumentType.DataBind();
            }
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                SharedSystemDocumentContract currentDoc = e.Item.DataItem as SharedSystemDocumentContract;
                HtmlAnchor lnkDoc = (HtmlAnchor)e.Item.FindControl("lnkDoc");
                if (currentDoc.DocumentPath.IsNullOrEmpty())
                {
                    lnkDoc.Visible = false;
                }
                else
                {
                    lnkDoc.HRef = String.Format("../ComplianceOperations/UserControl/DoccumentDownload.aspx?IsFileDownloadFromFilePath=true&FilePath={0}&FileName={1}", currentDoc.DocumentPath, currentDoc.FileName);
                }
            }
        }

        protected void grdUploadDocuments_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("PerformInsert"))
                {
                    GridEditFormItem item = e.Item as GridEditFormItem;
                    WclAsyncUpload uploadControl = (WclAsyncUpload)item.FindControl("uploadControl");
                    WclTextBox documentDescription = (WclTextBox)item.FindControl("txtDocDescription");
                    WclComboBox cmbDocumentType = (WclComboBox)item.FindControl("cmbDocumentType");

                    if (uploadControl.UploadedFiles.Count == AppConsts.NONE)
                    {
                        base.ShowInfoMessage(AppConsts.CLIENT_CONTACT_DOC_UPLOADINFOMSG);
                        return;
                    }

                    SharedSystemDocumentContract uploadedDocument = new SharedSystemDocumentContract();

                    if (CurrentViewContext.UploadedDocumentList.IsNullOrEmpty())
                        uploadedDocument.TempDocumentID = AppConsts.ONE; //List is empty, means 1st record.
                    else
                        uploadedDocument.TempDocumentID = CurrentViewContext.UploadedDocumentList.Count() + AppConsts.ONE;

                    uploadedDocument.FileName = uploadControl.UploadedFiles[0].FileName;
                    uploadedDocument.DocumentPath = UploadDocumentsTempLocation(uploadControl);
                    uploadedDocument.Description = documentDescription.Text.Trim();
                    uploadedDocument.DocumentTypeID = Convert.ToInt32(cmbDocumentType.SelectedValue);
                    uploadedDocument.DocumentTypeName = CurrentViewContext.DocumentTypeList.Where(x => x.SharedSystemDocTypeID == uploadedDocument.DocumentTypeID).Select(x => x.Name).FirstOrDefault();

                    //UAT 1426: WB: As an admin or Instructor/preceptor, I should be able to upload multiple licences for an instructor/preceptor
                    //if (HandleDocumentUploadCondition(uploadedDocument))
                    //{
                    //Add to existing documents
                    List<SharedSystemDocumentContract> tempList = CurrentViewContext.UploadedDocumentList;
                    tempList.Add(uploadedDocument);
                    CurrentViewContext.UploadedDocumentList = tempList;
                    //Rebind grid
                    WclGrid grdUploadDocuments = (WclGrid)sender;
                    //ShowHideUploadDocumentButton(grdUploadDocuments);

                    grdUploadDocuments.Rebind();
                    base.ShowSuccessMessage("Document added successfully.");
                    //}
                    //else
                    //{
                    //    base.ShowInfoMessage(AppConsts.CLIENT_CONTACT_DOC_ALREADY_UPLOADED);
                    //}

                }
                if (e.CommandName.Equals("Delete"))
                {
                    GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                    Int32 tempDocumentID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("TempDocumentID"));

                    SharedSystemDocumentContract recordToRemove = CurrentViewContext.UploadedDocumentList.FirstOrDefault(x => x.TempDocumentID == tempDocumentID);
                    CurrentViewContext.UploadedDocumentList.Remove(recordToRemove);

                    //UAT 1426: WB: As an admin or Instructor/preceptor, I should be able to upload multiple licences for an instructor/preceptor
                    //DocumentUploadedHistory.Remove(recordToRemove.DocumentTypeID);
                    //SharedSystemDocTypeContract documentToAdd = CurrentViewContext.DocumentTypeListTemp.Where(x => x.SharedSystemDocTypeID == recordToRemove.DocumentTypeID).FirstOrDefault();
                    //CurrentViewContext.DocumentTypeList.Add(documentToAdd);

                    WclGrid grdUploadDocuments = (WclGrid)sender;
                    ShowHideUploadDocumentButton(grdUploadDocuments);
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

        #endregion

        #region METHODS

        #region PRIVATE METHODS

        /// <summary>
        /// Method to Bind Tenant Dropdown and Other controls (If added)
        /// </summary>
        private void BindControls()
        {
            Presenter.GetTenants();
            cmbTenant.DataSource = CurrentViewContext.LstTenants;
            cmbTenant.DataBind();
            Presenter.IsAdminLoggedIn();
            Presenter.GetDocumentType();
            //UAT 1426: WB: As an admin or Instructor/preceptor, I should be able to upload multiple licences for an instructor/preceptor
            // CurrentViewContext.DocumentTypeList = new List<SharedSystemDocTypeContract>(CurrentViewContext.DocumentTypeListTemp);

            //Enable or disable tenant dropdown for admin or client admin 
            if (CurrentViewContext.IsAdminLoggedIn)
            {
                cmbTenant.Enabled = true;
                CurrentViewContext.SelectedTenantID = 0;
            }
            else
            {
                cmbTenant.Enabled = false;
                CurrentViewContext.SelectedTenantID = CurrentViewContext.TenantID;
            }
        }

        /// <summary>
        /// 1. Upload the selected file in temporary loaction.
        /// 2. Returns the path of temporary stored document.
        /// </summary>
        /// <param name="uploadControl"></param>
        private String UploadDocumentsTempLocation(WclAsyncUpload uploadControl)
        {
            String newTempFilePath = string.Empty;
            String filePath = String.Empty;
            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];

            if (tempFilePath.IsNullOrEmpty())
            {
                base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                return null;
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "Tenant(" + CurrentViewContext.TenantID.ToString() + @")\";

            if (!Directory.Exists(tempFilePath))
            {
                Directory.CreateDirectory(tempFilePath);
            }
            foreach (UploadedFile item in uploadControl.UploadedFiles)
            {
                String fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                //Save file
                newTempFilePath = Path.Combine(tempFilePath, fileName);
                item.SaveAs(newTempFilePath);
            }
            return newTempFilePath;
        }

        /// <summary>
        /// 1. Upload file to Amazon S3 or File System.
        /// 2. Update the UploadedDocumentList with updated path.
        /// </summary>
        /// <returns></returns>
        private Boolean UploadFileToS3OrFileSystem()
        {
            #region VARIABLES
            Boolean aWSUseS3 = false;
            StringBuilder corruptedFileMessage = new StringBuilder();
            String filePath = String.Empty;
            String fileSystemFileLocation = String.Empty;
            String awsS3FileLocation = String.Empty;
            #endregion

            #region CHECK WHETHER USE S3 or NOT
            if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
            {
                aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
            }
            #endregion
            filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
            //IF Amazon S3 is false then:
            if (aWSUseS3 == false)
            {
                if (filePath.IsNullOrEmpty())
                {
                    base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                    return false;
                }
                if (!filePath.EndsWith(@"\"))
                {
                    filePath += @"\";
                }
                filePath += "Tenant(" + CurrentViewContext.TenantID.ToString() + @")\";

                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
            }

            try
            {
                //Remove the Already UploadedDocuments (S3 or FileSystem) in case of Update.
                List<SharedSystemDocumentContract> newlyUploadedDocumentList = CurrentViewContext.UploadedDocumentList.Where(x => x.DocumentID == AppConsts.NONE).ToList();

                //Move each uploaded file.
                foreach (SharedSystemDocumentContract uploadedDocument in newlyUploadedDocumentList)
                {
                    //filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
                    String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                    String fileName = "ClientContact_" + CurrentViewContext.TenantID.ToString() + "_" + uploadedDocument.DocumentTypeID + "_" + date + Path.GetExtension(uploadedDocument.FileName);

                    //Get original file bytes and check if same document is already uploaded
                    byte[] fileBytes = File.ReadAllBytes(uploadedDocument.DocumentPath);

                    //update filesize
                    uploadedDocument.FileSize = fileBytes.Length;

                    //Check whether use AWS S3, true if need to use
                    if (aWSUseS3 == false)
                    {
                        //Move file to other location
                        fileSystemFileLocation = Path.Combine(filePath, fileName);
                        File.Copy(uploadedDocument.DocumentPath, fileSystemFileLocation);
                    }
                    else
                    {
                        //AWS code to save document to S3 location
                        AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                        String destFolder = filePath + "Tenant(" + CurrentViewContext.TenantID.ToString() + ")";
                        awsS3FileLocation = objAmazonS3.SaveDocument(uploadedDocument.DocumentPath, fileName, destFolder);
                        if (awsS3FileLocation.IsNullOrEmpty())
                        {
                            corruptedFileMessage.Append("Your file " + fileName + " is not uploaded. \\n");
                        }
                    }
                    try
                    {
                        if (!String.IsNullOrEmpty(uploadedDocument.DocumentPath))
                            File.Delete(uploadedDocument.DocumentPath);
                    }
                    catch (Exception) { }

                    //Update the list with update document path based on saving location.
                    if (aWSUseS3 == false)
                        uploadedDocument.DocumentPath = fileSystemFileLocation;
                    else
                        uploadedDocument.DocumentPath = awsS3FileLocation;
                }
                return true; // Means no execption occured, document should uploaded successfully.
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ShowHideUploadDocumentButton(WclGrid grdUploadDocuments)
        {
            //hide add button if document count is 4(means all type of document is uploaded.)
            if (DocumentUploadedHistory.Count == AppConsts.FOUR)
            {
                grdUploadDocuments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
            }
            else
            {
                grdUploadDocuments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = true;
            }
        }


        //UAT 1426: WB: As an admin or Instructor/preceptor, I should be able to upload multiple licences for an instructor/preceptor

        //private Boolean HandleDocumentUploadCondition(SharedSystemDocumentContract uploadedDocument)
        //{
        //    //if (DocumentUploadedHistory.Contains(uploadedDocument.DocumentTypeID))
        //    //{
        //    //   //=return false;
        //    //}
        //    //else
        //    //{
        //    //List<Int32> temp = DocumentUploadedHistory;
        //    //temp.Add(uploadedDocument.DocumentTypeID);
        //    //DocumentUploadedHistory = temp;

        //    //remove the uploaded doc type.
        //    //SharedSystemDocTypeContract sharedSystemSocType = CurrentViewContext.DocumentTypeList.Where(x => x.SharedSystemDocTypeID == uploadedDocument.DocumentTypeID).FirstOrDefault();
        //    //CurrentViewContext.DocumentTypeList.Remove(sharedSystemSocType);
        //    return true;
        //    // }
        //}

        private void HandleClientContactGridVisibility()
        {
            if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
            {
                grdInstrctrPreceptr.Visible = true;
                grdInstrctrPreceptr.Rebind();
            }
            else
            {
                grdInstrctrPreceptr.Visible = false;
            }
        }

        private void FillClientContactContract(GridCommandEventArgs e, Boolean IsUpdateMode = false)
        {
            GridEditFormItem item = e.Item as GridEditFormItem;

            #region Get Controls
            WclTextBox txtName = (WclTextBox)item.FindControl("txtName");
            WclTextBox txtEmail = (WclTextBox)item.FindControl("txtEmail");
            WclMaskedTextBox txtPhone = (WclMaskedTextBox)item.FindControl("txtPhone");
            WclComboBox cmbUserType = (WclComboBox)item.FindControl("cmbUserType");

            WclComboBox cmbAvailableDays = (WclComboBox)item.FindControl("cmbAvailableDays");
            //WclTimePicker tpStartTime = (WclTimePicker)item.FindControl("tpStartTime");
            //WclTimePicker tpEndTime = (WclTimePicker)item.FindControl("tpEndTime");
            //UAT-2447
            WclTextBox txtInternationalPhone = (WclTextBox)item.FindControl("txtInternationalPhone");
            WclCheckBox chkInternationalPhone = (WclCheckBox)item.FindControl("chkInternationalPhone");

            #endregion

            #region UAT-1874:  Availability Time Control

            WclTimePicker tpStartTimeMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeMonday") as WclTimePicker;
            WclTimePicker tpEndTimeMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeMonday") as WclTimePicker;
            Label lblMondayTime_CCA_ID = cmbAvailableDays.Parent.NamingContainer.FindControl("lblMondayTime_CCA_ID") as Label;
            WclTimePicker tpStartTimeTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeTuesday") as WclTimePicker;
            WclTimePicker tpEndTimeTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeTuesday") as WclTimePicker;
            Label lblTuesdayTime_CCA_ID = cmbAvailableDays.Parent.NamingContainer.FindControl("lblTuesdayTime_CCA_ID") as Label;
            WclTimePicker tpStartTimeWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeWednesday") as WclTimePicker;
            WclTimePicker tpEndTimeWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeWednesday") as WclTimePicker;
            Label lblWednesdayTime_CCA_ID = cmbAvailableDays.Parent.NamingContainer.FindControl("lblWednesdayTime_CCA_ID") as Label;
            WclTimePicker tpStartTimeThrusday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeThrusday") as WclTimePicker;
            WclTimePicker tpEndTimeThrusday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeThrusday") as WclTimePicker;
            Label lblThursdayTime_CCA_ID = cmbAvailableDays.Parent.NamingContainer.FindControl("lblThursdayTime_CCA_ID") as Label;
            WclTimePicker tpStartTimeFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeFriday") as WclTimePicker;
            WclTimePicker tpEndTimeFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeFriday") as WclTimePicker;
            Label lblFridayTime_CCA_ID = cmbAvailableDays.Parent.NamingContainer.FindControl("lblFridayTime_CCA_ID") as Label;
            WclTimePicker tpStartTimeSat = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeSat") as WclTimePicker;
            WclTimePicker tpEndTimeSat = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeSat") as WclTimePicker;
            Label lblSaturdayTime_CCA_ID = cmbAvailableDays.Parent.NamingContainer.FindControl("lblSaturdayTime_CCA_ID") as Label;
            WclTimePicker tpStartTimeSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeSunday") as WclTimePicker;
            WclTimePicker tpEndTimeSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeSunday") as WclTimePicker;
            Label lblSundayTime_CCA_ID = cmbAvailableDays.Parent.NamingContainer.FindControl("lblSundayTime_CCA_ID") as Label;

            #endregion

            #region Set App Setting Contract
            CurrentViewContext.AppSettingContract = new AppSettingContract();
            CurrentViewContext.AppSettingContract.ClientContactInvitationURL = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL].IsNullOrEmpty()
                                                                            ? String.Empty
                                                                            : Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);

            CurrentViewContext.AppSettingContract.SenderEmailID = System.Configuration.ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID];

            String selectedUserTypeCode = CurrentViewContext.ClientContactTypeList.Where(x => x.ClientContactTypeID == Convert.ToInt32(cmbUserType.SelectedValue)).Select(sel => sel.Code).FirstOrDefault();
            if (selectedUserTypeCode == ClientContactType.Instructor.GetStringValue())
            {
                CurrentViewContext.AppSettingContract.OrganizationUserType = OrganizationUserType.Instructor.GetStringValue();
            }
            else if (selectedUserTypeCode == ClientContactType.Preceptor.GetStringValue())
            {
                CurrentViewContext.AppSettingContract.OrganizationUserType = OrganizationUserType.Preceptor.GetStringValue();
            }

            #endregion

            #region Set Client Contact
            CurrentViewContext.ClientContact = new ClientContactContract();
            if (IsUpdateMode)
            {
                CurrentViewContext.ClientContact.ClientContactID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ClientContactID"]);
                CurrentViewContext.ClientContact.TokenID = (Guid)(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TokenID"]);
                //Handle UserID later
                if (e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"].IsNotNull())
                {
                    CurrentViewContext.ClientContact.UserID = (Guid)(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"]);
                }
                else
                {
                    CurrentViewContext.ClientContact.UserID = null;
                }

            }
            else
            {
                CurrentViewContext.ClientContact.TokenID = Guid.NewGuid();
                CurrentViewContext.ClientContactEmailID = txtEmail.Text;
                Presenter.GetExistingUserID();
                CurrentViewContext.ClientContact.UserID = CurrentViewContext.AspNetUserID.IsNotNull() ? CurrentViewContext.AspNetUserID : null;
            }
            CurrentViewContext.ClientContact.Name = txtName.Text.Trim();
            //UAt-2447
            CurrentViewContext.ClientContact.IsInternationalPhone = chkInternationalPhone.Checked;
            if (chkInternationalPhone.Checked)
            {
                CurrentViewContext.ClientContact.Phone = txtInternationalPhone.Text.Trim();
            }
            else
            {
                CurrentViewContext.ClientContact.Phone = txtPhone.Text.Trim();
            }
            CurrentViewContext.ClientContact.Email = txtEmail.Text.Trim();
            CurrentViewContext.ClientContact.TenantID = Convert.ToInt32(cmbTenant.SelectedValue);
            CurrentViewContext.ClientContact.ClientContactTypeID = Convert.ToInt32(cmbUserType.SelectedValue);
            #endregion

            #region Set Client Contact Availibilty
            List<ClientContactAvailibiltyContract> tempAvailibiltyList = new List<ClientContactAvailibiltyContract>();
            foreach (RadComboBoxItem day in cmbAvailableDays.CheckedItems)
            {
                ClientContactAvailibiltyContract clientAvailibiltyContact = new ClientContactAvailibiltyContract();
                clientAvailibiltyContact.WeekDayID = Convert.ToInt32(day.Value);
                #region UAT-1874
                switch (clientAvailibiltyContact.WeekDayID)
                {
                    case AppConsts.ONE://Monday
                        clientAvailibiltyContact.StartTime = Convert.ToDateTime(tpStartTimeMonday.SelectedDate);
                        clientAvailibiltyContact.EndTime = Convert.ToDateTime(tpEndTimeMonday.SelectedDate);
                        if (!lblMondayTime_CCA_ID.Text.IsNullOrEmpty())
                            clientAvailibiltyContact.ClientContactAvailibiltyID = Convert.ToInt32(lblMondayTime_CCA_ID.Text);
                        break;
                    case AppConsts.TWO://TUESDAy
                        clientAvailibiltyContact.StartTime = Convert.ToDateTime(tpStartTimeTuesday.SelectedDate);
                        clientAvailibiltyContact.EndTime = Convert.ToDateTime(tpEndTimeTuesday.SelectedDate);
                        if (!lblTuesdayTime_CCA_ID.Text.IsNullOrEmpty())
                            clientAvailibiltyContact.ClientContactAvailibiltyID = Convert.ToInt32(lblTuesdayTime_CCA_ID.Text);
                        break;
                    case AppConsts.THREE://WED
                        clientAvailibiltyContact.StartTime = Convert.ToDateTime(tpStartTimeWednesday.SelectedDate);
                        clientAvailibiltyContact.EndTime = Convert.ToDateTime(tpEndTimeWednesday.SelectedDate);
                        if (!lblWednesdayTime_CCA_ID.Text.IsNullOrEmpty())
                            clientAvailibiltyContact.ClientContactAvailibiltyID = Convert.ToInt32(lblWednesdayTime_CCA_ID.Text);
                        break;
                    case AppConsts.FOUR://THR
                        clientAvailibiltyContact.StartTime = Convert.ToDateTime(tpStartTimeThrusday.SelectedDate);
                        clientAvailibiltyContact.EndTime = Convert.ToDateTime(tpEndTimeThrusday.SelectedDate);
                        if (!lblThursdayTime_CCA_ID.Text.IsNullOrEmpty())
                            clientAvailibiltyContact.ClientContactAvailibiltyID = Convert.ToInt32(lblThursdayTime_CCA_ID.Text);
                        break;
                    case AppConsts.FIVE://FRI
                        clientAvailibiltyContact.StartTime = Convert.ToDateTime(tpStartTimeFriday.SelectedDate);
                        clientAvailibiltyContact.EndTime = Convert.ToDateTime(tpEndTimeFriday.SelectedDate);
                        if (!lblFridayTime_CCA_ID.Text.IsNullOrEmpty())
                            clientAvailibiltyContact.ClientContactAvailibiltyID = Convert.ToInt32(lblFridayTime_CCA_ID.Text);
                        break;
                    case AppConsts.SIX://SAT
                        clientAvailibiltyContact.StartTime = Convert.ToDateTime(tpStartTimeSat.SelectedDate);
                        clientAvailibiltyContact.EndTime = Convert.ToDateTime(tpEndTimeSat.SelectedDate);
                        if (!lblSaturdayTime_CCA_ID.Text.IsNullOrEmpty())
                            clientAvailibiltyContact.ClientContactAvailibiltyID = Convert.ToInt32(lblSaturdayTime_CCA_ID.Text);
                        break;
                    case AppConsts.SEVEN://SUNDAY
                        clientAvailibiltyContact.StartTime = Convert.ToDateTime(tpStartTimeSunday.SelectedDate);
                        clientAvailibiltyContact.EndTime = Convert.ToDateTime(tpEndTimeSunday.SelectedDate);
                        if (!lblSundayTime_CCA_ID.Text.IsNullOrEmpty())
                            clientAvailibiltyContact.ClientContactAvailibiltyID = Convert.ToInt32(lblSundayTime_CCA_ID.Text);
                        break;
                }
                #endregion
                //clientAvailibiltyContact.StartTime = Convert.ToDateTime(tpStartTime.SelectedDate);
                //clientAvailibiltyContact.EndTime = Convert.ToDateTime(tpEndTime.SelectedDate);
                tempAvailibiltyList.Add(clientAvailibiltyContact);
            }

            CurrentViewContext.ClientContact.ListClientContactAvailibiltyContract = tempAvailibiltyList;
            #endregion
        }

        //UAT 1426: WB: As an admin or Instructor/preceptor, I should be able to upload multiple licences for an instructor/preceptor
        //private void HandleDocumentTypeDropdown()
        //{
        //    CurrentViewContext.DocumentTypeList = new List<SharedSystemDocTypeContract>(CurrentViewContext.DocumentTypeListTemp);
        //    List<Int32> alreadyUploadDocTypes = CurrentViewContext.UploadedDocumentList.Select(x => x.DocumentTypeID).ToList();
        //    if (!alreadyUploadDocTypes.IsNullOrEmpty())
        //    {
        //        CurrentViewContext.DocumentTypeList.RemoveAll(x => alreadyUploadDocTypes.Contains(x.SharedSystemDocTypeID));
        //    }
        //}
        #endregion

        #region PUBLIC METHODS

        #endregion

        #endregion

        #region UAT-2447
        protected void chkInternationalPhone_CheckedChanged(object sender, EventArgs e)
        {
            var chkIsMaskingRequired = sender as WclCheckBox;
            ShowHidePhoneControls(chkIsMaskingRequired.Checked, chkIsMaskingRequired.Parent.NamingContainer as GridEditFormItem);
        }
        private void ShowHidePhoneControls(Boolean IsInternationalNumber, GridEditFormItem pnlControl)
        {
            RequiredFieldValidator rfvPhone = (RequiredFieldValidator)pnlControl.FindControl("rfvPhone");
            RequiredFieldValidator rfvTxtInternationalPhn = (RequiredFieldValidator)pnlControl.FindControl("rfvTxtInternationalPhn");
            RegularExpressionValidator revTxtMobilePrmyNonMasking = (RegularExpressionValidator)pnlControl.FindControl("revTxtMobilePrmyNonMasking");

            if (IsInternationalNumber)
            {
                (pnlControl.FindControl("txtInternationalPhone") as WclTextBox).Visible = true;
                (pnlControl.FindControl("txtPhone") as WclMaskedTextBox).Visible = false;
                rfvPhone.Enabled = false;
                rfvTxtInternationalPhn.Enabled = true;
                revTxtMobilePrmyNonMasking.Enabled = true;
            }
            else
            {
                (pnlControl.FindControl("txtInternationalPhone") as WclTextBox).Visible = false;
                (pnlControl.FindControl("txtPhone") as WclMaskedTextBox).Visible = true;
                rfvPhone.Enabled = true;
                rfvTxtInternationalPhn.Enabled = false;
                revTxtMobilePrmyNonMasking.Enabled = false;
            }
        }
        #endregion

        protected void cmbAvailableDays_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                ClearAvailabilityDays(sender);
                if (!String.IsNullOrEmpty(hdnPreviousAvailabilityDaysValues.Value))
                    ManageAvailabilityDaysVisibility(sender);
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

        private void ManageAvailabilityDaysVisibility(object sender)
        {
            var selectedDays = hdnPreviousAvailabilityDaysValues.Value.Split(',').ToList();
            WclComboBox cmbAvailableDays = sender as WclComboBox;
            HtmlGenericControl dvAvailabilityTime = cmbAvailableDays.Parent.NamingContainer.FindControl("dvAvailabilityTime") as HtmlGenericControl;
            foreach (var item in selectedDays)
            {
                dvAvailabilityTime.Visible = true;
                switch (Convert.ToInt32(item))
                {
                    case 1: //Monday
                        WclTimePicker tpStartTimeMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeMonday") as WclTimePicker;
                        RequiredFieldValidator rfvtpStartTimeMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeMonday") as RequiredFieldValidator;
                        WclTimePicker tpEndTimeMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeMonday") as WclTimePicker;
                        RequiredFieldValidator rfvtpEndTimeMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeMonday") as RequiredFieldValidator;
                        HtmlGenericControl dvMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("dvMonday") as HtmlGenericControl;
                        dvMonday.Visible = true;
                        tpStartTimeMonday.Visible = true;
                        rfvtpStartTimeMonday.Enabled = true;

                        tpEndTimeMonday.Visible = true;
                        rfvtpEndTimeMonday.Enabled = true;
                        break;
                    case 2: //Tuesday
                        WclTimePicker tpStartTimeTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeTuesday") as WclTimePicker;
                        RequiredFieldValidator rfvtpStartTimeTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeTuesday") as RequiredFieldValidator;
                        WclTimePicker tpEndTimeTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeTuesday") as WclTimePicker;
                        RequiredFieldValidator rfvtpEndTimeTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeTuesday") as RequiredFieldValidator;
                        HtmlGenericControl dvTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("dvTuesday") as HtmlGenericControl;
                        dvTuesday.Visible = true;
                        tpStartTimeTuesday.Visible = true;
                        rfvtpStartTimeTuesday.Enabled = true;

                        tpEndTimeTuesday.Visible = true;
                        rfvtpEndTimeTuesday.Enabled = true;
                        break;
                    case 3: //Wed
                        WclTimePicker tpStartTimeWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeWednesday") as WclTimePicker;
                        RequiredFieldValidator rfvtpStartTimeWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeWednesday") as RequiredFieldValidator;
                        WclTimePicker tpEndTimeWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeWednesday") as WclTimePicker;
                        RequiredFieldValidator rfvtpEndTimeWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeWednesday") as RequiredFieldValidator;
                        HtmlGenericControl dvWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("dvWednesday") as HtmlGenericControl;
                        dvWednesday.Visible = true;
                        tpStartTimeWednesday.Visible = true;
                        rfvtpStartTimeWednesday.Enabled = true;

                        tpEndTimeWednesday.Visible = true;
                        rfvtpEndTimeWednesday.Enabled = true;
                        break;
                    case 4: //Thr
                        WclTimePicker tpStartTimeThrusday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeThrusday") as WclTimePicker;
                        RequiredFieldValidator rfvtpStartTimeThrusday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeThrusday") as RequiredFieldValidator;
                        WclTimePicker tpEndTimeThrusday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeThrusday") as WclTimePicker;
                        RequiredFieldValidator rfvtpEndTimeThrusday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeThrusday") as RequiredFieldValidator;
                        HtmlGenericControl dvThr = cmbAvailableDays.Parent.NamingContainer.FindControl("dvThr") as HtmlGenericControl;
                        dvThr.Visible = true;
                        tpStartTimeThrusday.Visible = true;
                        rfvtpStartTimeThrusday.Enabled = true;

                        tpEndTimeThrusday.Visible = true;
                        rfvtpEndTimeThrusday.Enabled = true;
                        break;
                    case 5: //Fri
                        WclTimePicker tpStartTimeFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeFriday") as WclTimePicker;
                        RequiredFieldValidator rfvtpStartTimeFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeFriday") as RequiredFieldValidator;
                        WclTimePicker tpEndTimeFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeFriday") as WclTimePicker;
                        RequiredFieldValidator rfvtpEndTimeFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeFriday") as RequiredFieldValidator;
                        HtmlGenericControl dvFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("dvFriday") as HtmlGenericControl;
                        dvFriday.Visible = true;
                        tpStartTimeFriday.Visible = true;
                        rfvtpStartTimeFriday.Enabled = true;

                        tpEndTimeFriday.Visible = true;
                        rfvtpEndTimeFriday.Enabled = true;
                        break;
                    case 6: //Sat
                        WclTimePicker tpStartTimeSat = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeSat") as WclTimePicker;
                        RequiredFieldValidator rfvtpStartTimeSat = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeSat") as RequiredFieldValidator;
                        WclTimePicker tpEndTimeSat = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeSat") as WclTimePicker;
                        RequiredFieldValidator rfvtpEndTimeSat = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeSat") as RequiredFieldValidator;
                        HtmlGenericControl dvSat = cmbAvailableDays.Parent.NamingContainer.FindControl("dvSat") as HtmlGenericControl;
                        dvSat.Visible = true;
                        tpStartTimeSat.Visible = true;
                        rfvtpStartTimeSat.Enabled = true;

                        tpEndTimeSat.Visible = true;
                        rfvtpEndTimeSat.Enabled = true;
                        break;
                    case 7: //Sun
                        WclTimePicker tpStartTimeSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeSunday") as WclTimePicker;
                        RequiredFieldValidator rfvtpStartTimeSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeSunday") as RequiredFieldValidator;
                        WclTimePicker tpEndTimeSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeSunday") as WclTimePicker;
                        RequiredFieldValidator rfvtpEndTimeSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeSunday") as RequiredFieldValidator;
                        HtmlGenericControl dvSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("dvSunday") as HtmlGenericControl;
                        dvSunday.Visible = true;
                        tpStartTimeSunday.Visible = true;
                        rfvtpStartTimeSunday.Enabled = true;

                        tpEndTimeSunday.Visible = true;
                        rfvtpEndTimeSunday.Enabled = true;
                        break;
                }
            }
        }

        private void ClearAvailabilityDays(object sender)
        {
            WclComboBox cmbAvailableDays = sender as WclComboBox;
            HtmlGenericControl dvAvailabilityTime = cmbAvailableDays.Parent.NamingContainer.FindControl("dvAvailabilityTime") as HtmlGenericControl;
            dvAvailabilityTime.Visible = false;
            HtmlGenericControl dvMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("dvMonday") as HtmlGenericControl;
            HtmlGenericControl dvTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("dvTuesday") as HtmlGenericControl;
            HtmlGenericControl dvWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("dvWednesday") as HtmlGenericControl;
            HtmlGenericControl dvThr = cmbAvailableDays.Parent.NamingContainer.FindControl("dvThr") as HtmlGenericControl;
            HtmlGenericControl dvFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("dvFriday") as HtmlGenericControl;
            HtmlGenericControl dvSat = cmbAvailableDays.Parent.NamingContainer.FindControl("dvSat") as HtmlGenericControl;
            HtmlGenericControl dvSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("dvSunday") as HtmlGenericControl;

            dvMonday.Visible = false;
            dvTuesday.Visible = false;
            dvWednesday.Visible = false;
            dvThr.Visible = false;
            dvFriday.Visible = false;
            dvSat.Visible = false;
            dvSunday.Visible = false;
            WclTimePicker tpStartTimeMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeMonday") as WclTimePicker;
            RequiredFieldValidator rfvtpStartTimeMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeMonday") as RequiredFieldValidator;
            WclTimePicker tpEndTimeMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeMonday") as WclTimePicker;
            RequiredFieldValidator rfvtpEndTimeMonday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeMonday") as RequiredFieldValidator;

            tpStartTimeMonday.Visible = false;
            rfvtpStartTimeMonday.Enabled = false;

            tpEndTimeMonday.Visible = false;
            rfvtpEndTimeMonday.Enabled = false;

            WclTimePicker tpStartTimeTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeTuesday") as WclTimePicker;
            RequiredFieldValidator rfvtpStartTimeTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeTuesday") as RequiredFieldValidator;
            WclTimePicker tpEndTimeTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeTuesday") as WclTimePicker;
            RequiredFieldValidator rfvtpEndTimeTuesday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeTuesday") as RequiredFieldValidator;

            tpStartTimeTuesday.Visible = false;
            rfvtpStartTimeTuesday.Enabled = false;

            tpEndTimeTuesday.Visible = false;
            rfvtpEndTimeTuesday.Enabled = false;

            WclTimePicker tpStartTimeWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeWednesday") as WclTimePicker;
            RequiredFieldValidator rfvtpStartTimeWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeWednesday") as RequiredFieldValidator;
            WclTimePicker tpEndTimeWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeWednesday") as WclTimePicker;
            RequiredFieldValidator rfvtpEndTimeWednesday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeWednesday") as RequiredFieldValidator;

            tpStartTimeWednesday.Visible = false;
            rfvtpStartTimeWednesday.Enabled = false;

            tpEndTimeWednesday.Visible = false;
            rfvtpEndTimeWednesday.Enabled = false;

            WclTimePicker tpStartTimeThrusday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeThrusday") as WclTimePicker;
            RequiredFieldValidator rfvtpStartTimeThrusday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeThrusday") as RequiredFieldValidator;
            WclTimePicker tpEndTimeThrusday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeThrusday") as WclTimePicker;
            RequiredFieldValidator rfvtpEndTimeThrusday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeThrusday") as RequiredFieldValidator;

            tpStartTimeThrusday.Visible = false;
            rfvtpStartTimeThrusday.Enabled = false;

            tpEndTimeThrusday.Visible = false;
            rfvtpEndTimeThrusday.Enabled = false;

            WclTimePicker tpStartTimeFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeFriday") as WclTimePicker;
            RequiredFieldValidator rfvtpStartTimeFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeFriday") as RequiredFieldValidator;
            WclTimePicker tpEndTimeFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeFriday") as WclTimePicker;
            RequiredFieldValidator rfvtpEndTimeFriday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeFriday") as RequiredFieldValidator;

            tpStartTimeFriday.Visible = false;
            rfvtpStartTimeFriday.Enabled = false;

            tpEndTimeFriday.Visible = false;
            rfvtpEndTimeFriday.Enabled = false;

            WclTimePicker tpStartTimeSat = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeSat") as WclTimePicker;
            RequiredFieldValidator rfvtpStartTimeSat = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeSat") as RequiredFieldValidator;
            WclTimePicker tpEndTimeSat = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeSat") as WclTimePicker;
            RequiredFieldValidator rfvtpEndTimeSat = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeSat") as RequiredFieldValidator;

            tpStartTimeSat.Visible = false;
            rfvtpStartTimeSat.Enabled = false;

            tpEndTimeSat.Visible = false;
            rfvtpEndTimeSat.Enabled = false;

            WclTimePicker tpStartTimeSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpStartTimeSunday") as WclTimePicker;
            RequiredFieldValidator rfvtpStartTimeSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpStartTimeSunday") as RequiredFieldValidator;
            WclTimePicker tpEndTimeSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("tpEndTimeSunday") as WclTimePicker;
            RequiredFieldValidator rfvtpEndTimeSunday = cmbAvailableDays.Parent.NamingContainer.FindControl("rfvtpEndTimeSunday") as RequiredFieldValidator;

            tpStartTimeSunday.Visible = false;
            rfvtpStartTimeSunday.Enabled = false;

            tpEndTimeSunday.Visible = false;
            rfvtpEndTimeSunday.Enabled = false;
        }
        private void ManageControlVisibility(WclTimePicker wclStartTimePicker, WclTimePicker wclEndTimePicker, RequiredFieldValidator startTimeRequiredFieldValidator, RequiredFieldValidator endTimeRequiredFieldValidator, HtmlGenericControl htmlGenericControl, Boolean isVisible)
        {
            htmlGenericControl.Visible = isVisible;
            wclStartTimePicker.Visible = isVisible;
            wclEndTimePicker.Visible = isVisible;
            startTimeRequiredFieldValidator.Enabled = isVisible;
            endTimeRequiredFieldValidator.Enabled = isVisible;
        }
        protected void cmbAvailableDays_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {

        }

        private void ResetGridFilters()
        {
            grdInstrctrPreceptr.MasterTableView.FilterExpression = null;
            //CurrentViewContext.GridCustomPaging.SortExpression = null;
            grdInstrctrPreceptr.MasterTableView.SortExpressions.Clear();
            grdInstrctrPreceptr.CurrentPageIndex = 0;
            grdInstrctrPreceptr.MasterTableView.CurrentPageIndex = 0;
            grdInstrctrPreceptr.MasterTableView.IsItemInserted = false;
            grdInstrctrPreceptr.MasterTableView.ClearEditItems();
            foreach (GridColumn column in grdInstrctrPreceptr.MasterTableView.RenderColumns)
            {
                if (column.ColumnType == "GridBoundColumn")
                {
                    GridBoundColumn boundColumn = (GridBoundColumn)column;
                    String columnName = boundColumn.UniqueName.ToString();
                    grdInstrctrPreceptr.MasterTableView.GetColumnSafe(columnName).CurrentFilterFunction = GridKnownFunction.NoFilter;
                    grdInstrctrPreceptr.MasterTableView.GetColumnSafe(columnName).CurrentFilterValue = String.Empty;
                }
            }
            grdInstrctrPreceptr.Rebind();
        }

        protected void fsucManageInstructorCmdBar_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsReset = false;
                ResetGridFilters();
                HandleClientContactGridVisibility();
                grdInstrctrPreceptr.Rebind();
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

        protected void fsucManageInstructorCmdBar_ExtraClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.IsReset = true;
                BindControls();
                rbAccountActivated.SelectedValue = "2"; //UAT-4153
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

        protected void fsucManageInstructorCmdBar_CancelClick(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
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

        //UAT-UAT-4420
        protected void cmbTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {                
                if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
                {
                    grdInstrctrPreceptr.DataSource = new List<ClientContactContract>();
                    grdInstrctrPreceptr.Visible = true;
                    grdInstrctrPreceptr.Rebind();
                }
                else
                {
                    grdInstrctrPreceptr.Visible = false;
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
        //UAT-UAT-4420
    }
}