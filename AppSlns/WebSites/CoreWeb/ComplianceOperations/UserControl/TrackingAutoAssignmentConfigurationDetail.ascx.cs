using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class TrackingAutoAssignmentConfigurationDetail : BaseUserControl, ITrackingAutoAssignmentConfigurationDetailView
    {

        #region VARIABLES

        private TrackingAutoAssignmentConfigurationDetailPresenter _presenter = new TrackingAutoAssignmentConfigurationDetailPresenter();
        private String _viewType;

        #endregion

        #region PROPERTIES

        public TrackingAutoAssignmentConfigurationDetailPresenter Presenter
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

        public ITrackingAutoAssignmentConfigurationDetailView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<Entity.OrganizationUser> lstOrganizationUser
        {
            get
            {
                if (!ViewState["lstOrganizationUser"].IsNullOrEmpty())
                {
                    return ViewState["lstOrganizationUser"] as List<Entity.OrganizationUser>;
                }
                return new List<Entity.OrganizationUser>();
            }
            set
            {
                ViewState["lstOrganizationUser"] = value;
            }
        }

        public List<Int32> lstSelectedAdminsIds
        {
            get
            {
                if (!cmbAdmins.CheckedItems.IsNullOrEmpty() && cmbAdmins.CheckedItems.Count > AppConsts.NONE)
                {
                    return cmbAdmins.CheckedItems.Select(sel => int.Parse(sel.Value)).ToList();
                }
                return new List<Int32>();

            }
            set
            {
                foreach (RadComboBoxItem item in cmbAdmins.Items)
                {
                    if (value.Contains(Convert.ToInt32(item.Value)))
                        item.Checked = true;
                }
            }
        }

        Int32 ITrackingAutoAssignmentConfigurationDetailView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        //UAT-3075
        public List<CompliancePriorityObjectContract> lstObjects
        {
            get
            {
                if (!ViewState["lstObjects"].IsNullOrEmpty())
                {
                    return ViewState["lstObjects"] as List<CompliancePriorityObjectContract>;
                }
                return new List<CompliancePriorityObjectContract>();
            }
            set
            {
                ViewState["lstObjects"] = value;
            }
        }

        public List<CompliancePriorityObjectContract> lstOldSelectedObjects
        {
            get
            {
                if (!ViewState["lstOldSelectedObjects"].IsNullOrEmpty())
                {
                    return ViewState["lstOldSelectedObjects"] as List<CompliancePriorityObjectContract>;
                }
                return new List<CompliancePriorityObjectContract>();
            }
            set
            {
                ViewState["lstOldSelectedObjects"] = value;
            }
        }
        #endregion

        #region EVENTS

        #region PAGE EVENTS

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Auto Assignment Configuration";
                base.SetPageTitle("Auto Assignment Configuration");
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
                    BindAdmin();
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region BUTTON EVENTS

        protected void fsucCommandBar_SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (cmbAdmins.CheckedItems.Count > AppConsts.NONE)
                {
                    BindRepeater();
                    pnlAdminConfig.Visible = true;
                    cmbAdmins.Enabled = false;
                    fsucCommandBar.SaveButton.Enabled = false;
                    //UAT-3075
                    BindObjects();
                }
                if (cmbAdmins.CheckedItems.Count == AppConsts.NONE)
                {
                    rfvAdmin.IsValid = false;
                    return;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void fsucCommandBar_CancelClick(object sender, EventArgs e)
        {
            try
            {
                RedirectToTrackingConfigList(false);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void fsucCommandBar_ExtraClick(object sender, EventArgs e)
        {
            try
            {
                ResetControls();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void btnSaveAbdReturnToQueue_Click(object sender, EventArgs e)
        {
            List<TrackingAssignmentConfigurationContract> lstAdminsConfig = new List<TrackingAssignmentConfigurationContract>();
            List<TrackingAssignmentConfigurationContract> lstAllUsersBucketData = Presenter.UserBucketData();
            List<String> lstOfUserWithExistingDateRange = new List<String>();
            //BindObjects(); //UAT-3075
            foreach (RepeaterItem item in rptrAdminConfig.Items)
            {
                HiddenField hdnAdminID = item.FindControl("hdnAdminID") as HiddenField;
                WclNumericTextBox txtAssignmentCount = item.FindControl("txtAssignmentCount") as WclNumericTextBox;
                // Commented FOR UAT-3075 WclDatePicker dpDateFrom = item.FindControl("dpDateFrom") as WclDatePicker;// Commented FOR UAT-3075
                // Commented FOR UAT-3075 WclDatePicker dpDateTo = item.FindControl("dpDateTo") as WclDatePicker;
                WclTextBox txtAdminName = item.FindControl("txtAdminName") as WclTextBox;

                //UAT-3075
                WclNumericTextBox txtDaysFrom = item.FindControl("txtDaysFrom") as WclNumericTextBox;
                WclNumericTextBox txtDaysTo = item.FindControl("txtDaysTo") as WclNumericTextBox;
                Repeater rptrObjectPriority = item.FindControl("rptrObjectPriority") as Repeater;


                List<TrackingAssignmentConfigurationContract> lstUserBucketData = lstAllUsersBucketData.Where(cond => cond.AdminId == Convert.ToInt32(hdnAdminID.Value)).ToList();

                Boolean isConfigWithSameDateRange = false;
                if (!lstUserBucketData.IsNullOrEmpty())
                {
                    /* Commented FOR UAT-3075 isConfigWithSameDateRange = lstUserBucketData.Where(cond => (
                                                                            ((cond.DateFrom <= Convert.ToDateTime(dpDateFrom.SelectedDate)) && ((Convert.ToDateTime(dpDateFrom.SelectedDate) <= cond.DateTo)))
                                                                           || ((cond.DateFrom <= Convert.ToDateTime(dpDateTo.SelectedDate)) && ((Convert.ToDateTime(dpDateTo.SelectedDate) <= cond.DateTo)))
                                                                           || ((Convert.ToDateTime(dpDateFrom.SelectedDate) <= cond.DateFrom) && ((cond.DateTo <= Convert.ToDateTime(dpDateTo.SelectedDate))))
                                                                           )).Any(); 
                     */

                    /*Implementation fro number of days according to UAT-3075*/
                    Int32 greaterNoOfDays;
                    Int32 lowerNoOfDays;
                    if (Convert.ToInt32(txtDaysFrom.Text.Trim()) > Convert.ToInt32(txtDaysTo.Text.Trim()))
                    {
                        greaterNoOfDays = Convert.ToInt32(txtDaysFrom.Text.Trim());
                        lowerNoOfDays = Convert.ToInt32(txtDaysTo.Text.Trim());
                    }
                    else
                    {
                        greaterNoOfDays = Convert.ToInt32(txtDaysTo.Text.Trim());
                        lowerNoOfDays = Convert.ToInt32(txtDaysFrom.Text.Trim());
                    }

                    isConfigWithSameDateRange = lstUserBucketData.Where(cond => (
                                                                                   ((cond.DaysFrom <= lowerNoOfDays && cond.DaysTo >= lowerNoOfDays)
                                                                                     || (cond.DaysTo <= lowerNoOfDays && cond.DaysFrom >= lowerNoOfDays))
                                                                                 ||
                                                                                   ((cond.DaysFrom <= greaterNoOfDays && cond.DaysTo >= greaterNoOfDays)
                                                                                     || (cond.DaysTo <= greaterNoOfDays && cond.DaysFrom >= greaterNoOfDays))
                                                                                 ||
                                                                                   ((cond.DaysFrom >= lowerNoOfDays && cond.DaysTo >= lowerNoOfDays)
                                                                                             && (cond.DaysFrom <= greaterNoOfDays && cond.DaysTo <= greaterNoOfDays))
                                                                                 )).Any();
                }

                if (!isConfigWithSameDateRange)
                {
                    TrackingAssignmentConfigurationContract trackingAssignmentConfig = new TrackingAssignmentConfigurationContract();
                    trackingAssignmentConfig.AdminId = Convert.ToInt32(hdnAdminID.Value);
                    trackingAssignmentConfig.AssignmentCount = txtAssignmentCount.Text.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(txtAssignmentCount.Text);
                    // Commented FOR UAT-3075 trackingAssignmentConfig.DateFrom = Convert.ToDateTime(dpDateFrom.SelectedDate);
                    // Commented FOR UAT-3075trackingAssignmentConfig.DateTo = Convert.ToDateTime(dpDateTo.SelectedDate);

                    //UAT-3075
                    trackingAssignmentConfig.DaysFrom = txtDaysFrom.Text.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(txtDaysFrom.Text.Trim());
                    trackingAssignmentConfig.DaysTo = txtDaysTo.Text.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(txtDaysTo.Text.Trim());
                    trackingAssignmentConfig.lstConfigObjMapping = new List<TrackingConfigObjectMappingContract>();
                    foreach (RepeaterItem objItem in rptrObjectPriority.Items)
                    {
                        HiddenField hdnObjId = objItem.FindControl("hdnObjId") as HiddenField;
                        WclNumericTextBox txtPriority = objItem.FindControl("txtPriority") as WclNumericTextBox;
                        TrackingConfigObjectMappingContract trackingConfigObjMapping = new TrackingConfigObjectMappingContract();
                        trackingConfigObjMapping.TCOM_ComplianceObjectID = Convert.ToInt32(hdnObjId.Value);
                        trackingConfigObjMapping.TCOM_Priority = Convert.ToInt32(txtPriority.Text.Trim());
                        trackingAssignmentConfig.lstConfigObjMapping.Add(trackingConfigObjMapping);
                    }
                    //END UAT-3075
                    lstAdminsConfig.Add(trackingAssignmentConfig);
                }
                else
                {
                    lstOfUserWithExistingDateRange.Add("'" + txtAdminName.Text.Trim() + "'");
                    //base.ShowInfoMessage("Please select different submission date range as submitted date range overlapped with previous added configuration of '" + txtAdminName.Text.Trim() + "'."); 
                    //return;
                }

            }

            if (!lstOfUserWithExistingDateRange.IsNullOrEmpty())
            {
                base.ShowInfoMessage("Please select different days range as submitted no of days overlapped with previous added configuration of " + String.Join(", ", lstOfUserWithExistingDateRange) + ".");
                return;
            }

            if (Presenter.SaveAdminsConfig(lstAdminsConfig))
            {

                RedirectToTrackingConfigList(true);
            }
        }

        protected void lnkGoBack_Click(object sender, EventArgs e)
        {
            RedirectToTrackingConfigList(false);
        }

        #endregion

        #endregion

        #region Methods

        private void BindRepeater()
        {
            if (!cmbAdmins.CheckedItems.IsNullOrEmpty() && cmbAdmins.CheckedItems.Count > AppConsts.NONE)
            {
                rptrAdminConfig.DataSource = CurrentViewContext.lstOrganizationUser.Where(cond => (lstSelectedAdminsIds.Contains(cond.OrganizationUserID))).ToList();
                rptrAdminConfig.DataBind();
            }
        }

        private void BindAdmin()
        {
            Presenter.GetUserList();
            cmbAdmins.DataSource = CurrentViewContext.lstOrganizationUser;
            cmbAdmins.DataBind();
        }

        private void ResetControls()
        {
            BindAdmin();
            pnlAdminConfig.Visible = false;
            cmbAdmins.Enabled = true;
            fsucCommandBar.SaveButton.Enabled = true;
        }

        private void RedirectToTrackingConfigList(Boolean IsSaveSuccessfully)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                             {
                                                                { "Child", ChildControls.ManageTrackingAutoAssignmentConfiguration},
                                                                 {"IsSaveSuccessfully",Convert.ToString(IsSaveSuccessfully)}
                                                                };
            string url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        #endregion

        #region UAT-3075

        protected void btnObjectPriority_Click(object sender, EventArgs e)
        {
            BindObjectPriority();
        }

        private void BindObjects()
        {
            Presenter.GetComplianceObjects();
            foreach (RepeaterItem item in rptrAdminConfig.Items)
            {
                WclComboBox ddlObjects = item.FindControl("ddlObjects") as WclComboBox;
                ddlObjects.DataSource = CurrentViewContext.lstObjects;
                ddlObjects.DataBind();
            }
        }

        protected void BindObjectPriority()
        {
            foreach (RepeaterItem item in rptrAdminConfig.Items)
            {
                WclComboBox ddlObjects = item.FindControl("ddlObjects") as WclComboBox;
                Repeater rptrObjectPriority = item.FindControl("rptrObjectPriority") as Repeater;
                HiddenField hdnOldSelectedObjects = item.FindControl("hdnOldSelectedObjects") as HiddenField;
                List<CompliancePriorityObjectContract> lstObjectsSelected = new List<CompliancePriorityObjectContract>();
                List<Int32> lstSelectedObjects = new List<Int32>();
                if (!ddlObjects.CheckedItems.IsNullOrEmpty() && ddlObjects.CheckedItems.Count > AppConsts.NONE)
                {
                    lstSelectedObjects = ddlObjects.CheckedItems.Select(sel => int.Parse(sel.Value)).ToList();  // Get the list of selected objects for a particular configuration.
                    lstObjectsSelected = CurrentViewContext.lstObjects.Where(cond => (lstSelectedObjects.Contains(cond.CPO_ID))).ToList();
                }
                if (!hdnOldSelectedObjects.Value.Contains(String.Join(",", lstSelectedObjects)))
                {
                    rptrObjectPriority.DataSource = lstObjectsSelected;
                    rptrObjectPriority.DataBind();
                    hdnOldSelectedObjects.Value = String.Join(",", lstSelectedObjects);
                }
                if (ddlObjects.CheckedItems.Count == AppConsts.NONE)
                {
                    rptrObjectPriority.DataSource = lstObjectsSelected;
                    rptrObjectPriority.DataBind();
                    hdnOldSelectedObjects.Value = String.Join(",", lstSelectedObjects);
                }
            }
        }

        #endregion

    }
}