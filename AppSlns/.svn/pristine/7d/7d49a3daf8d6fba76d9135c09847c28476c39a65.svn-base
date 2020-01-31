using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class AgencyHierarchyRootNodeSetting : BaseUserControl, IAgencyHierarchySettingView
    {
        #region Handlers

        public delegate bool ShowMessageHandler(object sender, StatusMessages messageType, String message);
        public event ShowMessageHandler eventShowMessage;

        #endregion

        #region [Variables / Properties]

        #region [Private Variables]

        private AgencyHierarchySettingPresenter _presenter = new AgencyHierarchySettingPresenter();

        #endregion

        public IAgencyHierarchySettingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public AgencyHierarchySettingPresenter Presenter
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

        Int32 IAgencyHierarchySettingView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IAgencyHierarchySettingView.AgencyHierarchyID
        {
            get
            {
                return Convert.ToInt32(ViewState["AgencyHierarchyId"]);
            }
            set
            {
                ViewState["AgencyHierarchyId"] = value;
            }
        }

        Boolean IAgencyHierarchySettingView.IsRootNode
        {
            get;
            set;
        }

        public Int32 NodeId { get; set; }

        AgencyHierarchySettingContract IAgencyHierarchySettingView.AgencyHierarchySettingContract
        {
            get;
            set;
        }

        Boolean IAgencyHierarchySettingView.IsAgencyHierachySettingExisted
        {
            get;
            set;
        }

        #region UAT-3950
        AgencyHierarchySettingContract IAgencyHierarchySettingView.AutoArchivedRotationSettingContract
        {
            get;
            set;
        }

        Boolean IAgencyHierarchySettingView.IsAutoArchivedRotationSettingExisted
        {
            get;
            set;
        }

        //Start UAT-4673
        Boolean IAgencyHierarchySettingView.IsUpdateReviewStatusSettingExisted
        {
            get;
            set;
        }

        AgencyHierarchySettingContract IAgencyHierarchySettingView.UpdateReviewStatusSettingContract
        {
            get;
            set;
        }
        //End UAT-4673
        
        #endregion

        #region UAT-3662
        public Int32 SelectedRootNodeID
        {
            get
            {
                if (!ViewState["SelectedRootNodeID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["SelectedRootNodeID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedRootNodeID"] = value;
            }
        }

        AgencyHierarchySettingContract IAgencyHierarchySettingView.InstPrecepMandateIndividualShareContract
        {
            get;
            set;
        }

        #endregion

        //#region UAT-4150
        //List<AgencyHierarchyRootNodeSettingContract> IAgencyHierarchySettingView.lstAgencyHierarchyRootNodeSettingContract
        //{
        //    get
        //    {
        //        if (!ViewState["lstAgencyHierarchyRootNodeSettingContract"].IsNullOrEmpty())
        //            return ViewState["lstAgencyHierarchyRootNodeSettingContract"] as List<AgencyHierarchyRootNodeSettingContract>;
        //        return new List<AgencyHierarchyRootNodeSettingContract>();
        //    }
        //    set
        //    {
        //        ViewState["lstAgencyHierarchyRootNodeSettingContract"] = value;
        //    }
        //}
        //#endregion

        #endregion

        #region [Page Events]

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    if (CurrentViewContext.SelectedRootNodeID > AppConsts.NONE)
                    {

                        if (Presenter.IsAgencyHierachyRootNodeSettingExists())
                        {
                            rdbOptionsTypeSpecialtyYes.Checked = true;
                            rdbOptionsTypeSpecialtydNo.Checked = false;
                            showTypeSpecialityOptionDiv.Visible = true;

                        }
                        else
                        {
                            rdbOptionsTypeSpecialtyYes.Checked = false;
                            showTypeSpecialityOptionDiv.Visible = false;
                            rdbOptionsTypeSpecialtydNo.Checked = true;
                        }
                        //   ManageSettingRadioButtons();
                        if (Presenter.InstAvailabilityHierarchyRootNodeSetting())
                        {
                            rdbOptionsInstAvailabilityYes.Checked = true;
                            rdbOptionsInstAvailabilityNo.Checked = false;
                        }
                        else
                        {
                            rdbOptionsInstAvailabilityYes.Checked = false;
                            rdbOptionsInstAvailabilityNo.Checked = true;
                        }
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        #endregion

        #region [Button Events]

        protected void fsucCmdBarArchivedRotation_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                var agencyHierarchySettingContract = new AgencyHierarchyRootNodeSettingContract();
                agencyHierarchySettingContract.AgencyHierarchyID = CurrentViewContext.SelectedRootNodeID;
                agencyHierarchySettingContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;
                agencyHierarchySettingContract.SettingTypeCode = AgencyHierarchyRootNodeSettingType.OPTIONS_FOR_TYPE_SPECIALTY_ROTATION_FIELD.GetStringValue();
                agencyHierarchySettingContract.SettingValue = rdbOptionsTypeSpecialtyYes.Checked ? AppConsts.STR_ONE : AppConsts.ZERO;

                if (Presenter.SaveAgencyHierarchyRootNodeSetting(agencyHierarchySettingContract))
                {
                    if (rdbOptionsTypeSpecialtyYes.Checked)
                    {
                        showTypeSpecialityOptionDiv.Visible = true;
                        grdRotTypeSpecialtyOptions.Rebind();
                    }
                    else
                    {
                        showTypeSpecialityOptionDiv.Visible = false;
                    }
                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Agency Hierarchy Root Node Setting(s) Saved Successfully.");
                }
                else
                    eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Error while saving setting(s) to selected node.");
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

        /// <summary>
        /// This event is added in UAT-4150. To save the agency root node setting "Rotation must specify Instructor availability"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdInstructorAvailablity_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                AgencyHierarchyRootNodeSettingContract agencyHierarchySettingContract = new AgencyHierarchyRootNodeSettingContract();
                agencyHierarchySettingContract.AgencyHierarchyID = CurrentViewContext.SelectedRootNodeID;
                agencyHierarchySettingContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;
                agencyHierarchySettingContract.SettingTypeCode = AgencyHierarchyRootNodeSettingType.OPTIONS_TO_SPECIFY_INSTRUCTOR_AVAILABILITY.GetStringValue();
                agencyHierarchySettingContract.SettingValue = rdbOptionsInstAvailabilityYes.Checked ? AppConsts.STR_ONE : AppConsts.ZERO;
                if (Presenter.SaveAgencyHierarchyRootNodeSetting(agencyHierarchySettingContract))
                {
                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Agency Hierarchy Root Node Setting(s) Saved Successfully.");
                }
                else
                    eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Error while saving setting(s) to selected node.");
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

        #region [Methods]

        #region [Private Methods]

        //private void ManageSettingRadioButtons()
        //{
        //    Presenter.GetAgencyHierarchyRootNodeSettings();
        //    if (!CurrentViewContext.lstAgencyHierarchyRootNodeSettingContract.IsNullOrEmpty())
        //    {
        //        foreach (var item in CurrentViewContext.lstAgencyHierarchyRootNodeSettingContract)
        //        {
        //            Boolean isTrue = false;
        //            List<RadioButton> lstRdbtnToUnCheck = new List<RadioButton>();
        //            if (!item.SettingValue.IsNullOrEmpty())
        //                isTrue = Convert.ToBoolean(item.SettingValue);
        //            else
        //                isTrue = false;

        //            if (item.SettingTypeCode == AgencyHierarchyRootNodeSettingType.OPTIONS_FOR_TYPE_SPECIALTY_ROTATION_FIELD.GetStringValue())
        //            {
        //                lstRdbtnToUnCheck = new List<RadioButton>();
        //                if (isTrue)
        //                {
        //                    lstRdbtnToUnCheck.Add(rdbOptionsTypeSpecialtydNo);
        //                    CheckRadioButtons(rdbOptionsTypeSpecialtyYes, lstRdbtnToUnCheck);
        //                    showTypeSpecialityOptionDiv.Visible = true;
        //                }
        //                else// Also for default
        //                {
        //                    lstRdbtnToUnCheck.Add(rdbOptionsTypeSpecialtyYes);
        //                    CheckRadioButtons(rdbOptionsTypeSpecialtydNo, lstRdbtnToUnCheck);
        //                    showTypeSpecialityOptionDiv.Visible = false;
        //                }
        //            }

        //            if (item.SettingTypeCode == AgencyHierarchyRootNodeSettingType.OPTIONS_TO_SPECIFY_INSTRUCTOR_AVAILABILITY.GetStringValue())
        //            {
        //                if (isTrue)
        //                {
        //                    lstRdbtnToUnCheck.Add(rdbOptionsInstAvailabilityNo);
        //                    CheckRadioButtons(rdbOptionsInstAvailabilityYes, lstRdbtnToUnCheck);
        //                }
        //                else // Also for default
        //                {
        //                    lstRdbtnToUnCheck.Add(rdbOptionsInstAvailabilityYes);
        //                    CheckRadioButtons(rdbOptionsInstAvailabilityNo, lstRdbtnToUnCheck);
        //                }
        //            }
        //        }
        //    }
        //}

        //private void CheckRadioButtons(RadioButton rdBtnToCheck, List<RadioButton> lstRdBtnToUncheck)
        //{
        //    if (!rdBtnToCheck.IsNullOrEmpty())
        //    {
        //        rdBtnToCheck.Checked = true;
        //    }
        //    if (!lstRdBtnToUncheck.IsNullOrEmpty())
        //    {
        //        foreach (RadioButton rdbtn in lstRdBtnToUncheck)
        //        {
        //            rdbtn.Checked = false;
        //        }
        //    }
        //}

        #endregion

        #region [Public Methods]

        #endregion

        #endregion

        #region [Grid Events]

        protected void grdRotTypeSpecialtyOptions_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    ViewState["SortExpression"] = e.SortExpression;
                    ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    ViewState["SortExpression"] = String.Empty;
                    ViewState["SortDirection"] = false;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdRotTypeSpecialtyOptions_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                List<AgencyHierarchyRootNodeSettingContract> agencyHierarchyRootNodeSettingContract = Presenter.GetAgencyHierarchyRootNodeMapping();
                grdRotTypeSpecialtyOptions.DataSource = agencyHierarchyRootNodeSettingContract;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdRotTypeSpecialtyOptions_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                AgencyHierarchyRootNodeSettingContract agencyHierarchyUserContract = new AgencyHierarchyRootNodeSettingContract();
                agencyHierarchyUserContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;
                agencyHierarchyUserContract.AgencyHierarchyID = CurrentViewContext.SelectedRootNodeID;
                agencyHierarchyUserContract.MappingID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["MappingID"]);
                agencyHierarchyUserContract.IsRecordDeleted = true;
                agencyHierarchyUserContract.SettingTypeCode = AgencyHierarchyRootNodeSettingType.OPTIONS_FOR_TYPE_SPECIALTY_ROTATION_FIELD.GetStringValue();
                if (Presenter.SaveUpdateAgencyHierarchyRootNodeMapping(agencyHierarchyUserContract))
                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Type/Specialty Option Successfully Deleted.");
                else
                    eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Unable to delete type/specialty option.");
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdRotTypeSpecialtyOptions_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    WclTextBox txtOptionValue = e.Item.FindControl("txtOptionValue") as WclTextBox;
                    AgencyHierarchyRootNodeSettingContract agencyHierarchyRootNodeSettingContract = new AgencyHierarchyRootNodeSettingContract();
                    agencyHierarchyRootNodeSettingContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;

                    if (!txtOptionValue.IsNullOrEmpty())
                    {
                        String DisplayMessage = String.Empty;
                        agencyHierarchyRootNodeSettingContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;
                        agencyHierarchyRootNodeSettingContract.AgencyHierarchyID = CurrentViewContext.SelectedRootNodeID;
                        agencyHierarchyRootNodeSettingContract.IsRecordDeleted = false;
                        agencyHierarchyRootNodeSettingContract.SettingTypeCode = AgencyHierarchyRootNodeSettingType.OPTIONS_FOR_TYPE_SPECIALTY_ROTATION_FIELD.GetStringValue();
                        agencyHierarchyRootNodeSettingContract.MappingValue = txtOptionValue.Text;

                        if (e.CommandName == RadGrid.UpdateCommandName)
                        {
                            DisplayMessage = "Type/Specialty Option(s) Successfully Updated.";
                            agencyHierarchyRootNodeSettingContract.MappingID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["MappingID"]);
                        }
                        else
                        {
                            DisplayMessage = "Type/Specialty Option Successfully Added.";
                            agencyHierarchyRootNodeSettingContract.MappingID = null;
                        }
                        if (Presenter.SaveUpdateAgencyHierarchyRootNodeMapping(agencyHierarchyRootNodeSettingContract))
                            eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, DisplayMessage);
                        else
                            eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Error while adding type/specialty option to selected node.");
                    }

                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }
        #endregion
    }
}