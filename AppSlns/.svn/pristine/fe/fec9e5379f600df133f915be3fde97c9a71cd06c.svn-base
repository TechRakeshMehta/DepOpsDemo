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
    public partial class ManageTrackingAutoAssignmentConfiguration : BaseUserControl, IManageTrackingAutoAssignmentConfigurationView
    {

        #region Variables

        private ManageTrackingAutoAssignmentConfigurationPresenter _presenter = new ManageTrackingAutoAssignmentConfigurationPresenter();
        private String _viewType;

        #endregion

        #region Properties

        public ManageTrackingAutoAssignmentConfigurationPresenter Presenter
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

        public IManageTrackingAutoAssignmentConfigurationView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<TrackingAssignmentConfigurationContract> lstAdminsConfiguration
        {
            get
            {
                if (!ViewState["lstAdminsConfiguration"].IsNullOrEmpty())
                {
                    return ViewState["lstAdminsConfiguration"] as List<TrackingAssignmentConfigurationContract>;
                }
                return new List<TrackingAssignmentConfigurationContract>();
            }
            set
            {
                ViewState["lstAdminsConfiguration"] = value;
            }
        }

        Int32 IManageTrackingAutoAssignmentConfigurationView.CurrentLoggedInUserId
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

        public List<TrackingConfigObjectMappingContract> lstTrackConfigObjectsMapped
        {
            get
            {
                if (!ViewState["lstTrackConfigObjectsMapped"].IsNullOrEmpty())
                {
                    return ViewState["lstTrackConfigObjectsMapped"] as List<TrackingConfigObjectMappingContract>;
                }
                return new List<TrackingConfigObjectMappingContract>();
            }
            set
            {
                ViewState["lstTrackConfigObjectsMapped"] = value;
            }
        }

        public Int32 selectedTrackConfigID
        {
            get;
            set;
        }
        #endregion

        #region Events

        #region  Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Manage Auto Assignment";
                base.SetPageTitle("Manage Auto Assignment");
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
                    CaptureQueryString();
                    BindObject();
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

        #region Grid Events

        protected void grdAdmins_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetAdminTrackingAssignmentConfiguration();
                grdAdmins.DataSource = CurrentViewContext.lstAdminsConfiguration;
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

        protected void grdAdmins_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", ChildControls.TrackingAutoAssignmentConfigurationDetail},
                                                                    };
                    string url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }

                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    Int32 TAC_ID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TAC_ID"]);
                    if (Presenter.DeleteConfiguration(TAC_ID))
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Auto assignment configuration deleted successfully.");
                        grdAdmins.Rebind();
                    }
                    else
                    {
                        e.Canceled = true;
                        base.ShowInfoMessage("Some error has occurred. Please try again.");
                    }
                }

                if (e.CommandName == "Update")
                {
                    WclTextBox txtAdminName = e.Item.FindControl("txtAdminName") as WclTextBox;
                    WclNumericTextBox txtAssignmentCount = e.Item.FindControl("txtAssignmentCount") as WclNumericTextBox;
                    // Commented FOR UAT-3075 WclDatePicker dpStartDate = e.Item.FindControl("dpStartDate") as WclDatePicker;
                    // Commented FOR UAT-3075 WclDatePicker dpEndDate = e.Item.FindControl("dpEndDate") as WclDatePicker;
                    //UAT-3075
                    WclNumericTextBox txtDaysFrom = e.Item.FindControl("txtDaysFrom") as WclNumericTextBox;
                    WclNumericTextBox txtDaysTo = e.Item.FindControl("txtDaysTo") as WclNumericTextBox;
                    Repeater rptrObjectPriority = e.Item.FindControl("rptrObjectPriority") as Repeater;
                    WclComboBox ddlObjects = e.Item.FindControl("ddlObjects") as WclComboBox;
                    //END UAT-3075
                    Int32 adminId = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AdminId"]);
                    Int32 tac_id = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TAC_ID"]);

                    /* Commented FOR UAT-3075 Boolean noChangeInData = CurrentViewContext.lstAdminsConfiguration.Where(cond => ((cond.DateFrom == Convert.ToDateTime(dpStartDate.SelectedDate))
                                                                                   && (cond.DateTo == Convert.ToDateTime(dpEndDate.SelectedDate))
                                                                                   && (cond.AssignmentCount == Convert.ToInt32(txtAssignmentCount.Text.Trim()))
                                                                                   && cond.TAC_ID == tac_id)).Any(); 
                     */

                    //UAT-3075
                    Boolean noChangeInData = CurrentViewContext.lstAdminsConfiguration.Where(cond => (cond.DaysFrom == Convert.ToInt32(txtDaysFrom.Text.Trim()))
                                                                                        && (cond.DaysTo == Convert.ToInt32(txtDaysTo.Text.Trim()))
                                                                                        && (cond.AssignmentCount == Convert.ToInt32(txtAssignmentCount.Text.Trim()))
                                                                                        && cond.TAC_ID == tac_id).Any();


                    List<Int32> lstAllSelectedObjIds = ddlObjects.CheckedItems.Select(sel => int.Parse(sel.Value)).ToList();
                    Boolean noChangeInobjMapping = false;
                    if (!lstAllSelectedObjIds.IsNullOrEmpty())
                    {
                        foreach (Int32 objId in lstAllSelectedObjIds)
                        {
                            noChangeInobjMapping = true;

                            if (!(CurrentViewContext.lstTrackConfigObjectsMapped.Where(cond => cond.TCOM_ComplianceObjectID == objId).Any()))
                            {
                                noChangeInobjMapping = false;
                                break;
                            }

                        }
                        foreach (RepeaterItem objItem in rptrObjectPriority.Items)
                        {
                            HiddenField hdnObjId = objItem.FindControl("hdnObjId") as HiddenField;
                            WclNumericTextBox txtPriority = objItem.FindControl("txtPriority") as WclNumericTextBox;
                            if (CurrentViewContext.lstTrackConfigObjectsMapped.Where(cond => cond.TCOM_ComplianceObjectID == Convert.ToInt32(hdnObjId.Value)).Any())
                            {
                                TrackingConfigObjectMappingContract trackConfigObjectMappingData = CurrentViewContext.lstTrackConfigObjectsMapped.Where(cond => cond.TCOM_ComplianceObjectID == Convert.ToInt32(hdnObjId.Value)).FirstOrDefault();
                                if (trackConfigObjectMappingData.TCOM_Priority != Convert.ToInt32(txtPriority.Text.Trim()))
                                {
                                    noChangeInobjMapping = false;
                                    break;
                                }
                            }
                        }
                    }


                    if (noChangeInData && noChangeInobjMapping)
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Auto assignment configuration updated successfully.");
                        return;
                    }

                    else
                    {
                        Boolean isConfigWithSameDateRange = false;
                        List<TrackingAssignmentConfigurationContract> lstUserConfigData = CurrentViewContext.lstAdminsConfiguration.Where(cond => cond.AdminId == adminId).ToList();
                        if (!lstUserConfigData.IsNullOrEmpty())
                        {
                            /* Commented FOR UAT-3075 isConfigWithSameDateRange = lstUserConfigData.Where(cond => (
                                                                                         (((cond.DateFrom <= Convert.ToDateTime(dpStartDate.SelectedDate)) && ((Convert.ToDateTime(dpStartDate.SelectedDate) <= cond.DateTo)))
                                                                                         || ((cond.DateFrom <= Convert.ToDateTime(dpEndDate.SelectedDate)) && ((Convert.ToDateTime(dpEndDate.SelectedDate) <= cond.DateTo)))
                                                                                         || ((Convert.ToDateTime(dpStartDate.SelectedDate) <= cond.DateFrom) && ((cond.DateTo <= Convert.ToDateTime(dpEndDate.SelectedDate))))))
                                                                                         && (cond.TAC_ID != tac_id)
                                                                                         ).Any();
                             */
                            /*Implementation for number of days according to UAT-3075*/
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

                            isConfigWithSameDateRange = lstUserConfigData.Where(cond => (((cond.DaysFrom <= lowerNoOfDays && cond.DaysTo >= lowerNoOfDays)
                                                                                           || (cond.DaysTo <= lowerNoOfDays && cond.DaysFrom >= lowerNoOfDays))
                                                                                       ||
                                                                                       ((cond.DaysFrom <= greaterNoOfDays && cond.DaysTo >= greaterNoOfDays)
                                                                                           || (cond.DaysTo <= greaterNoOfDays && cond.DaysFrom >= greaterNoOfDays))
                                                                                       || ((cond.DaysFrom>=lowerNoOfDays && cond.DaysTo>=lowerNoOfDays) 
                                                                                             && (cond.DaysFrom<=greaterNoOfDays && cond.DaysTo<=greaterNoOfDays))
                                                                                         )
                                                                                    && cond.TAC_ID != tac_id).Any();
                        }

                        TrackingAssignmentConfigurationContract trackingConfigurationContract = new TrackingAssignmentConfigurationContract();
                        List<TrackingConfigObjectMappingContract> lstTrackObjMappingToDelete = new List<TrackingConfigObjectMappingContract>(); //UAT-3075
                        if (!isConfigWithSameDateRange)
                        {
                            trackingConfigurationContract.TAC_ID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TAC_ID"]);
                            trackingConfigurationContract.AssignmentCount = Convert.ToInt32(txtAssignmentCount.Text.Trim());
                            // Commented FOR UAT-3075  trackingConfigurationContract.DateFrom = Convert.ToDateTime(dpStartDate.SelectedDate);
                            // Commented FOR UAT-3075  trackingConfigurationContract.DateTo = Convert.ToDateTime(dpEndDate.SelectedDate);

                            //UAT-3075
                            trackingConfigurationContract.DaysFrom = txtDaysFrom.Text.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(txtDaysFrom.Text.Trim());
                            trackingConfigurationContract.DaysTo = txtDaysTo.Text.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(txtDaysTo.Text.Trim());

                            trackingConfigurationContract.lstConfigObjMapping = new List<TrackingConfigObjectMappingContract>();
                            if (!noChangeInobjMapping)
                            {
                                foreach (RepeaterItem objItem in rptrObjectPriority.Items)
                                {
                                    HiddenField hdnObjId = objItem.FindControl("hdnObjId") as HiddenField;
                                    HiddenField hdnObjMappingId = objItem.FindControl("hdnObjMappingId") as HiddenField;
                                    WclNumericTextBox txtPriority = objItem.FindControl("txtPriority") as WclNumericTextBox;
                                    TrackingConfigObjectMappingContract trackingConfigObjMapping = new TrackingConfigObjectMappingContract();

                                    trackingConfigObjMapping.TCOM_ComplianceObjectID = Convert.ToInt32(hdnObjId.Value);
                                    trackingConfigObjMapping.TCOM_Priority = Convert.ToInt32(txtPriority.Text.Trim());
                                    if (!hdnObjMappingId.Value.IsNullOrEmpty() && Convert.ToInt32(hdnObjMappingId.Value) > AppConsts.NONE)
                                        trackingConfigObjMapping.TCOM_ID = Convert.ToInt32(hdnObjMappingId.Value);
                                    trackingConfigurationContract.lstConfigObjMapping.Add(trackingConfigObjMapping);
                                }

                                //Get the objects unchecked from dropdown to delete there mappings.

                                foreach (TrackingConfigObjectMappingContract trackObjMappingToDelete in CurrentViewContext.lstTrackConfigObjectsMapped)
                                {
                                    if (!lstAllSelectedObjIds.Contains(trackObjMappingToDelete.TCOM_ComplianceObjectID))
                                    {
                                        lstTrackObjMappingToDelete.Add(trackObjMappingToDelete);
                                    }
                                }
                            }
                            //END UAT-3075
                        }
                        else
                        {
                            base.ShowInfoMessage("Please select different days range as submitted no of days overlapped with previous added configuration of " + txtAdminName.Text.Trim() + "'.");
                            return;
                        }

                        if (Presenter.UpdateConfiguration(trackingConfigurationContract, lstTrackObjMappingToDelete))
                        {
                            e.Canceled = false;
                            base.ShowSuccessMessage("Auto assignment configuration updated successfully.");
                            grdAdmins.Rebind();
                        }
                        else
                        {
                            e.Canceled = true;
                            base.ShowInfoMessage("Some error has occurred. Please try again.");
                        }
                    }
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

        protected void grdAdmins_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);

                    WclTextBox txtAdminName = editform.FindControl("txtAdminName") as WclTextBox;
                    WclNumericTextBox txtAssignmentCount = editform.FindControl("txtAssignmentCount") as WclNumericTextBox;
                    /* Commented FOR UAT-3075   WclDatePicker dpStartDate = editform.FindControl("dpStartDate") as WclDatePicker;
                      WclDatePicker dpEndDate = editform.FindControl("dpEndDate") as WclDatePicker;
                      */

                    //UAT-3075

                    WclNumericTextBox txtDaysFrom = editform.FindControl("txtDaysFrom") as WclNumericTextBox;
                    WclNumericTextBox txtDaysTo = editform.FindControl("txtDaysTo") as WclNumericTextBox;
                    WclComboBox ddlObjects = editform.FindControl("ddlObjects") as WclComboBox;
                    Repeater rptrObjectPriority = editform.FindControl("rptrObjectPriority") as Repeater;

                    TrackingAssignmentConfigurationContract trackingConfig = e.Item.DataItem as TrackingAssignmentConfigurationContract;

                    if (!trackingConfig.IsNullOrEmpty())
                    {
                        txtAdminName.Text = trackingConfig.AdminFirstName + " " + trackingConfig.AdminLastName;
                        txtAssignmentCount.Text = Convert.ToString(trackingConfig.AssignmentCount);
                        /* Commented FOR UAT-3075 dpStartDate.SelectedDate = trackingConfig.DateFrom;
                         dpEndDate.SelectedDate = trackingConfig.DateTo;
                         */
                        //UAT-3075
                        txtDaysFrom.Text = Convert.ToString(trackingConfig.DaysFrom);
                        txtDaysTo.Text = Convert.ToString(trackingConfig.DaysTo);

                        ddlObjects.DataSource = CurrentViewContext.lstObjects;
                        ddlObjects.DataBind();
                        var lst = trackingConfig.lstConfigObjMapping; //Test
                        CurrentViewContext.selectedTrackConfigID = Convert.ToInt32(trackingConfig.TAC_ID);
                        List<Int32> lstMappedObjectIds = new List<Int32>();
                        if (CurrentViewContext.selectedTrackConfigID > AppConsts.NONE)
                        {
                            BindObjectRepeater();
                            lstMappedObjectIds = CurrentViewContext.lstTrackConfigObjectsMapped.Select(sel => sel.TCOM_ComplianceObjectID).ToList();
                            hdnPreviousObjectValues.Value = String.Join(",", lstMappedObjectIds);
                            rptrObjectPriority.DataSource = CurrentViewContext.lstTrackConfigObjectsMapped;
                            rptrObjectPriority.DataBind();
                        }
                        foreach (RadComboBoxItem item in ddlObjects.Items)
                        {
                            if (!lstMappedObjectIds.IsNullOrEmpty() && lstMappedObjectIds.Contains(Convert.ToInt32(item.Value)))
                            {
                                item.Checked = true;
                            }
                        }
                    }
                }


                //foreach (var item in CurrentViewContext.lstAdminsConfiguration)
                //{
                //    if (!item.lstConfigObjMapping.IsNullOrEmpty())
                //    {
                //        //New parameter append in comma seperated
                //        item.allObjectsName = String.Join(",", item.lstConfigObjMapping.Select(sel => sel.ObjectName));
                //    }
                //}
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

        #endregion

        #region Methods

        private void CaptureQueryString()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNullOrEmpty())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (args.ContainsKey("IsSaveSuccessfully") && Convert.ToBoolean(args["IsSaveSuccessfully"]) == true)
                {
                    base.ShowSuccessMessage("Auto assignment configuration saved successfully.");
                }
            }
        }

        #endregion

        #region UAT-3075

        private void BindObject()
        {
            Presenter.GetComplianceObjects();
        }

        private void BindObjectRepeater()
        {
            Presenter.GetTrackConfigObjectMapped();
        }
        #endregion


        protected void ddlObjects_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (Convert.ToBoolean(hdnIsPostback.Value))
            {
                foreach (GridDataItem item in grdAdmins.EditItems)
                {
                    WclComboBox ddlObjects = item.EditFormItem.FindControl("ddlObjects") as WclComboBox;
                    Repeater rptrObjectPriority = item.EditFormItem.FindControl("rptrObjectPriority") as Repeater;

                    // List<Int32> lstobjectIdsAlreadyMapped = CurrentViewContext.lstTrackConfigObjectsMapped.Select(sel => sel.TCOM_ComplianceObjectID).ToList();// Already mapped ids of object
                    List<TrackingConfigObjectMappingContract> lstTrackingConfigObject = new List<TrackingConfigObjectMappingContract>();
                    if (!ddlObjects.CheckedItems.IsNullOrEmpty() && ddlObjects.CheckedItems.Count > AppConsts.NONE)
                    {
                        List<Int32> lstSelectedObjects = ddlObjects.CheckedItems.Select(sel => int.Parse(sel.Value)).ToList();//Total selected items

                        //New list of objects selected from dropdown

                        lstTrackingConfigObject = CurrentViewContext.lstTrackConfigObjectsMapped.Where(cond => lstSelectedObjects.Contains(cond.TCOM_ComplianceObjectID)).ToList();
                        List<Int32> lstTrackingConfigObjIdsAlreadySelected = lstTrackingConfigObject.Select(sel => sel.TCOM_ComplianceObjectID).ToList();

                        //  List<Int32> lstNewSelectedObjectIds = 
                        foreach (Int32 objId in lstSelectedObjects)
                        {

                            TrackingConfigObjectMappingContract trackConfObj = new TrackingConfigObjectMappingContract();
                            if (!lstTrackingConfigObjIdsAlreadySelected.Contains(objId))
                            {
                                trackConfObj.TCOM_ComplianceObjectID = objId;
                                trackConfObj.ObjectName = CurrentViewContext.lstObjects.Where(cond => cond.CPO_ID == objId).Select(sel => sel.CPO_Name).FirstOrDefault();
                                lstTrackingConfigObject.Add(trackConfObj);
                            }
                        }

                    }
                    rptrObjectPriority.DataSource = lstTrackingConfigObject;
                    rptrObjectPriority.DataBind();
                }
            }
        }
    }
}