using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class AgencyHierarchySetting : BaseUserControl, IAgencyHierarchySettingView
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

        #endregion


        #region [Page Events]

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    CurrentViewContext.AgencyHierarchyID = NodeId;
                    if (CurrentViewContext.AgencyHierarchyID > AppConsts.NONE)
                    {
                        Presenter.IsAgencyHierachySettingExisted();

                        if (CurrentViewContext.IsRootNode)
                        {
                            rbtnExpirationCriteriaNo.Checked = true;                            
                        }

                        if (CurrentViewContext.IsAgencyHierachySettingExisted)
                        {
                            //Bind Setting
                            BindAgencyHierarchySetting();
                        }
                        else
                        {
                            rbtnCheckParentSettingYes.Checked = true;
                            dvHierarchySetting.Visible = false;
                            rbtnExpirationCriteriaYes.Checked = true;
                        }
                        Presenter.IsAgencyHierachyAutomaticArchiveSettingExist();
                        //UAT_3950
                        if (CurrentViewContext.IsAutoArchivedRotationSettingExisted)
                        {
                            //Bind Setting
                            BindAutoArchivedRotationSetting();
                        }
                        else
                        {
                            rbtnAutoArchivedSettingCheckYes.Checked = true;
                            dvAutomaticallyArchivedSetting.Visible = false;
                            rdbAutoArchivedNo.Checked = true;
                        }

                        //Start UAT-4673
                        Presenter.DoesUpdateReviewStatusSettingExist();

                        if (CurrentViewContext.IsUpdateReviewStatusSettingExisted)
                        {
                            BindUpdateReviewStatusSetting();
                        }
                        else
                        {
                            rbReviewStatusParentSettingYes.Checked = true;
                            dvReviewStatusSetting.Visible = false;
                            rdbUpdateReviewStatusNo.Checked = true;
                        }
                        //End UAT-4673

                        if (CurrentViewContext.IsRootNode)
                        {
                            rbtnCheckParentSettingYes.Enabled = false;
                            rbtnCheckParentSettingNo.Enabled = false;
                            rbtnCheckParentSettingYes.Checked = false;
                            rbtnCheckParentSettingNo.Checked = true;
                            dvHierarchySetting.Visible = true;
                            //rbtnExpirationCriteriaYes.Checked = true; // commeneted UAT:4654  2
                            //rbtnExpirationCriteriaNo.Checked = true;

                            //UAT-3950
                            rbtnAutoArchivedSettingCheckYes.Enabled = false;
                            rbtnAutoArchivedSettingCheckNo.Enabled = false;
                            rbtnAutoArchivedSettingCheckYes.Checked = false;
                            rbtnAutoArchivedSettingCheckNo.Checked = true;
                            if (!rdbAutoArchivedYes.Checked)
                                rdbAutoArchivedNo.Checked = true;
                            dvAutomaticallyArchivedSetting.Visible = true;

                            //Start UAT-4673
                            rbReviewStatusParentSettingYes.Enabled = false;
                            rbReviewStatusParentSettingNo.Enabled = false;
                            rbReviewStatusParentSettingYes.Checked = false;
                            rbReviewStatusParentSettingNo.Checked = true;
                            dvReviewStatusSetting.Visible = true;
                            //End UAT-4673
                        }

                        //UAT-3662
                        BindInstPrecepMandatIndividlShare();
                    }
                    else
                    {
                        dvAgencyHierarchySetting.Visible = false;
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

        protected void fsucCmdBar_SubmitClick(object sender, EventArgs e)
        {
            try
            {


                var agencyHierarchySettingContract = new AgencyHierarchySettingContract();
                agencyHierarchySettingContract.CheckParentSetting = rbtnCheckParentSettingYes.Checked ? true : false;
                agencyHierarchySettingContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyID;
                agencyHierarchySettingContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;
                agencyHierarchySettingContract.IsExpirationCriteria = rbtnExpirationCriteriaYes.Checked;
                agencyHierarchySettingContract.SettingTypeCode = AgencyHierarchySettingType.EXPIRATION_CRITERIA.GetStringValue();
                agencyHierarchySettingContract.IsRotationArchivedAutomatically = rdbAutoArchivedYes.Checked ? true : false;




                if (Presenter.SaveUpdateAgencyHierarchySetting(agencyHierarchySettingContract))

                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Agency Hierarchy Setting(s) Saved Successfully.");
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

        protected void fsucCmdBarArchivedRotation_SubmitClick(object sender, EventArgs e)
        {
            try
            {


                var agencyHierarchySettingContract = new AgencyHierarchySettingContract();
                agencyHierarchySettingContract.CheckParentSetting = rbtnAutoArchivedSettingCheckYes.Checked ? true : false;
                agencyHierarchySettingContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyID;
                agencyHierarchySettingContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;
                agencyHierarchySettingContract.SettingTypeCode = AgencyHierarchySettingType.AUTOMATICALLY_ARCHIVED_ROTATION.GetStringValue();
                agencyHierarchySettingContract.IsRotationArchivedAutomatically = rdbAutoArchivedYes.Checked ? true : false;

                if (Presenter.SaveUpdateAgencyHierarchySetting(agencyHierarchySettingContract))

                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Agency Hierarchy Setting(s) Saved Successfully.");
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

        //Start UAT-4673
        protected void cmdBarReviewStatus_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                var agencyHierarchySettingContract = new AgencyHierarchySettingContract();
                agencyHierarchySettingContract.CheckParentSetting = rbReviewStatusParentSettingYes.Checked ? true : false;
                agencyHierarchySettingContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyID;
                agencyHierarchySettingContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;
                agencyHierarchySettingContract.SettingTypeCode = AgencyHierarchySettingType.UPDATE_REVIEW_STATUS.GetStringValue();
                agencyHierarchySettingContract.SettingValue = rdbUpdateReviewStatusYes.Checked ? AppConsts.STR_ONE : AppConsts.ZERO;
                
                if (Presenter.SaveUpdateAgencyHierarchySetting(agencyHierarchySettingContract))
                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Agency Hierarchy Setting(s) Saved Successfully.");
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
        //End UAT-4673

        #endregion

        #region [Control Events]

        #region [Radio Button Events]

        protected void rbtnCheckParentSettingYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnCheckParentSettingYes.Checked)
            {
                dvDisplayAgencyHierarchySettings.Visible = false;
            }
            else
            {
                dvHierarchySetting.Visible = true;
                dvDisplayAgencyHierarchySettings.Visible = true;
            }
        }

        protected void rbtnCheckParentSettingNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnCheckParentSettingNo.Checked)
            {
                dvHierarchySetting.Visible = true;
                dvDisplayAgencyHierarchySettings.Visible = true;
            }
            else
                dvDisplayAgencyHierarchySettings.Visible = false;
        }


        #region UAT-3950
        protected void rbtnAutoArchivedSettingCheckYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnAutoArchivedSettingCheckYes.Checked)
            {
                dvAutomaticallyArchivedSetting.Visible = false;
            }
            else
            {
                //dvHierarchySetting.Visible = true;
                dvAutomaticallyArchivedSetting.Visible = true;
            }
        }

        protected void rbtnAutoArchivedSettingCheckNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnAutoArchivedSettingCheckNo.Checked)
            {
                //dvHierarchySetting.Visible = true;
                dvAutomaticallyArchivedSetting.Visible = true;
            }
            else
                dvAutomaticallyArchivedSetting.Visible = false;
        }
        #endregion

        //Start UAT-4673
        protected void rbReviewStatusParentSettingYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbReviewStatusParentSettingYes.Checked)
            {
                dvReviewStatusSetting.Visible = false;
            }
            else
            {
                dvReviewStatusSetting.Visible = true;
            }
        }

        protected void rbReviewStatusParentSettingNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbReviewStatusParentSettingNo.Checked)
            {
                dvReviewStatusSetting.Visible = true;
            }
            else
            {
                dvReviewStatusSetting.Visible = false;
            }
        }
        //End UAT-4673

        #endregion

        #endregion


        #region [Methods]

        #region [Private Methods]
        private void ResetControl()
        {
            rbtnExpirationCriteriaNo.Checked = true;
            if (CurrentViewContext.IsRootNode)
                rbtnCheckParentSettingNo.Checked = true;
            //else
            //{
            //    //rbtnCheckParentSettingYes.Checked = true;
            //   // dvDisplayAgencyHierarchySettings.Visible = false;
            //}
        }

        private void BindAgencyHierarchySetting()
        {
            var agencysettings = CurrentViewContext.AgencyHierarchySettingContract;
            if (agencysettings.CheckParentSetting)
            {
                dvHierarchySetting.Visible = false;
                rbtnCheckParentSettingYes.Checked = true;
                if (agencysettings.IsExpirationCriteria)
                {
                    rbtnExpirationCriteriaYes.Checked = true;
                    rbtnExpirationCriteriaNo.Checked = false;
                }
                else
                {
                    rbtnExpirationCriteriaNo.Checked = true;
                    rbtnExpirationCriteriaYes.Checked = false;
                }
            }
            else
            {
                rbtnCheckParentSettingNo.Checked = true;
                if (CurrentViewContext.IsRootNode)
                {
                    rbtnCheckParentSettingNo.Enabled = false;
                    rbtnCheckParentSettingYes.Enabled = false;
                }

                if (agencysettings.IsExpirationCriteria)
                {
                    rbtnExpirationCriteriaYes.Checked = true;
                    rbtnExpirationCriteriaNo.Checked = false;
                }
                else
                {
                    rbtnExpirationCriteriaYes.Checked = false;
                    rbtnExpirationCriteriaNo.Checked = true;
                }
            }
        }

        #region UAT-3950
        private void BindAutoArchivedRotationSetting()
        {
            var agencysettings = CurrentViewContext.AutoArchivedRotationSettingContract;
            if (agencysettings.CheckParentSetting)
            {
                dvAutomaticallyArchivedSetting.Visible = false;
                rbtnAutoArchivedSettingCheckYes.Checked = true;
                //if (agencysettings.IsRotationArchivedAutomatically)
                //{
                rdbAutoArchivedYes.Checked = IsAutoArchivedRotationSettingOn();
                rdbAutoArchivedNo.Checked = !IsAutoArchivedRotationSettingOn();
                //}
                //}
                //else
                //{
                //    rbtnExpirationCriteriaNo.Checked = true;
                //    rbtnExpirationCriteriaYes.Checked = false;
                //}
            }
            else
            {
                rbtnAutoArchivedSettingCheckNo.Checked = true;
                //rdbAutoArchivedYes.Checked = IsAutoArchivedRotationSettingOn();
                //rdbAutoArchivedNo.Checked = !IsAutoArchivedRotationSettingOn();
                if (IsAutoArchivedRotationSettingOn())
                    rdbAutoArchivedYes.Checked = true;
                else
                    rdbAutoArchivedNo.Checked = true;
            }
        }

        //Start UAT-4673
        private void BindUpdateReviewStatusSetting()
        {
            var agencysettings = CurrentViewContext.UpdateReviewStatusSettingContract;
            if (agencysettings.CheckParentSetting)
            {
                dvReviewStatusSetting.Visible = false;
                rbReviewStatusParentSettingYes.Checked = true;
                if (agencysettings.SettingValue == AppConsts.ZERO)
                    rdbUpdateReviewStatusNo.Checked = true;
                else
                    rdbUpdateReviewStatusYes.Checked = true;
            }
            else
            {
                rbReviewStatusParentSettingNo.Checked = true;
                if (agencysettings.SettingValue == AppConsts.ZERO)
                    rdbUpdateReviewStatusNo.Checked = true;
                else
                    rdbUpdateReviewStatusYes.Checked = true;
            }
        }
        //End UAT-4673

        private Boolean IsAutoArchivedRotationSettingOn()
        {
            return CurrentViewContext.AutoArchivedRotationSettingContract.SettingValue == AppConsts.ZERO ? false : true;
        }
        #endregion

        #region UAT-3662

        private void BindInstPrecepMandatIndividlShare()
        {
            Presenter.GetInstPrecMandatIndividlShareSetting();
            AgencyHierarchySettingContract agencysettings = CurrentViewContext.InstPrecepMandateIndividualShareContract;

            if (NodeId == SelectedRootNodeID) // Or == CurrentViewContext.AgencyHierarchyID
            {
                rdbtnDefault.Visible = false;
            }

            if (!agencysettings.IsNullOrEmpty() && !agencysettings.SettingValue.IsNullOrEmpty())
            {
                if (Convert.ToInt32(agencysettings.SettingValue) == AppConsts.NONE)
                {
                    rdbtnDefault.Checked = true; //0
                    rdbtnOptional.Checked = false; //1
                    rdbtnRequired.Checked = false; //2
                }
                else if (Convert.ToInt32(agencysettings.SettingValue) == AppConsts.ONE)
                {
                    rdbtnDefault.Checked = false;
                    rdbtnOptional.Checked = true;
                    rdbtnRequired.Checked = false;
                }
                else
                {
                    rdbtnDefault.Checked = false;
                    rdbtnOptional.Checked = false;
                    rdbtnRequired.Checked = true;
                }
            }
            else
            {
                if (NodeId == SelectedRootNodeID) // Or == CurrentViewContext.AgencyHierarchyID
                {
                    rdbtnDefault.Checked = false; //0
                    rdbtnOptional.Checked = true; //1
                    rdbtnRequired.Checked = false; //2
                    // rdbtnDefault.Visible = false;
                }
                else
                {
                    rdbtnDefault.Checked = true; //0
                    rdbtnOptional.Checked = false; //1
                    rdbtnRequired.Checked = false; //2
                }
            }
        }

        protected void cmdInstPrecpMandatoryIndvShare_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                var agencyHierarchySettingContract = new AgencyHierarchySettingContract();
                Int32 settingValue = rdbtnRequired.Checked ? AppConsts.TWO : (rdbtnOptional.Checked ? AppConsts.ONE : AppConsts.NONE);
                agencyHierarchySettingContract.CheckParentSetting = rdbtnDefault.Checked ? true : false;
                agencyHierarchySettingContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyID;
                agencyHierarchySettingContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;
                agencyHierarchySettingContract.SettingValue = Convert.ToString(settingValue);
                agencyHierarchySettingContract.SettingTypeCode = AgencyHierarchySettingType.INSTPRECEPTOR_MANDATORY_FOR_INDIVIDUAL_SHARE.GetStringValue();

                if (Presenter.SaveUpdateAgencyHierarchySetting(agencyHierarchySettingContract))

                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Agency Hierarchy Setting(s) Saved Successfully.");
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


        #endregion

        #region [Public Methods]

        #endregion

        #endregion
    }
}