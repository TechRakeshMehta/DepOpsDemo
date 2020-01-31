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
    public partial class AgencyHierarchyRotationFieldOption : BaseUserControl, IAgencyHierarchyRotationFieldOptionView
    {
        #region Handlers

        public delegate bool ShowMessageHandler(object sender, StatusMessages messageType, String message);
        public event ShowMessageHandler eventShowMessage;

        #endregion

        #region [Variables / Properties]

        #region [Private Variables]

        private AgencyHierarchyRotationFieldOptionPresenter _presenter = new AgencyHierarchyRotationFieldOptionPresenter();

        #endregion

        public IAgencyHierarchyRotationFieldOptionView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public AgencyHierarchyRotationFieldOptionPresenter Presenter
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

        Int32 IAgencyHierarchyRotationFieldOptionView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IAgencyHierarchyRotationFieldOptionView.AgencyHierarchyID
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

        AgencyHierarchyRotationFieldOptionContract IAgencyHierarchyRotationFieldOptionView.AgencyHierarchyRotationFieldOptionContract
        {
            get;
            set;
        }

        Boolean IAgencyHierarchyRotationFieldOptionView.IsRotationFieldOptionSettingExisted
        {
            get;
            set;
        }

        Boolean IAgencyHierarchyRotationFieldOptionView.IsRootNode
        {
            get;
            set;
        }
        public Int32 NodeId { get; set; }
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
                        Presenter.IsAgencyHierarchyRotationFieldOptionSettingSaved();
                        if (CurrentViewContext.IsRotationFieldOptionSettingExisted)
                        {
                            //Bind Setting
                            GetRotationFieldOptionPermissions();
                        }
                        else
                        {
                            ResetControl();
                        }
                        if (CurrentViewContext.IsRootNode)
                        {
                            rbtnCheckParentSettingYes.Enabled = false;
                            rbtnCheckParentSettingNo.Enabled = false;
                        }
                    }
                    else
                    {
                        dvRotationFieldOption.Visible = false;
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

        #region [Control Events]

        #region [Button Event]

        //protected void btnUpdate_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        EditMode();
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //}

        //protected void btnAddRotationFieldSetting_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        dvDisplayRotationSettings.Visible = true;
        //        dvAddRotationSettings.Visible = false;
        //        ResetControl();
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //}

        //protected void fsucCmdBar_CancelClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (CurrentViewContext.IsRotationFieldOptionSettingExisted)
        //        {
        //            dvDisplayRotationSettings.Visible = true;
        //            dvAddRotationSettings.Visible = false;
        //            ReadMode();
        //        }
        //        else
        //        {
        //            dvDisplayRotationSettings.Visible = false;
        //            dvAddRotationSettings.Visible = true;
        //        }
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //}

        protected void fsucCmdBar_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                var rotationFieldOptionPermissions = new AgencyHierarchyRotationFieldOptionContract();
                rotationFieldOptionPermissions.AHRFO_CheckParentSetting = rbtnCheckParentSettingYes.Checked ? true : false;
                rotationFieldOptionPermissions.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyID;
                rotationFieldOptionPermissions.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;
                if (!rbtnCheckParentSettingYes.Checked)
                {
                    rotationFieldOptionPermissions.AHRFO_IsCourse_Required = rbtnCourseYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsDaysBefore_Required = rbtnDaysBeforeYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsDeadlineDate_Required = rbtnDeadlineDateYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsDepartment_Required = rbtnDepartmentYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsEndTime_Required = rbtnEndTimeYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsFrequency_Required = rbtnFrequencyYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsIP_Required = rbtnIPYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsNoOfHours_Required = rbtnHourYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsNoOfStudents_Required = rbtnStudentYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsProgram_Required = rbtnProgramYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsRotationName_Required = rbtnRotationNameYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsRotationShift_Required = rbtnShiftYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsRotDays_Required = rbtnDaysYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsStartTime_Required = rbtnStartTimeYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsSyllabusDocument_Required = rbtnSyllabusYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsTerm_Required = rbtnTermYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsTypeSpecialty_Required = rbtnTypeYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsUnitFloorLoc_Required = rbtnUnitYes.Checked;
                    rotationFieldOptionPermissions.AHRFO_IsAdditionalDocuments_Required = rbtnAdditionalDocumentsYes.Checked;
                }


                if (Presenter.SaveUpdateAgencyHierarchyRotationFieldOption(rotationFieldOptionPermissions))
                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Rotation Field Permission(s) Saved Successfully.");
                else
                    eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Error while saving permission(s) to selected node.");

                //dvDisplayRotationSettings.Visible = true;
                //dvAddRotationSettings.Visible = false;
                //ReadMode();

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

        #region [Radio Button]
        protected void rbtnCheckParentSettingYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnCheckParentSettingYes.Checked)
                dvNodeRotationFieldOptionSetting.Visible = false;
            else
                dvNodeRotationFieldOptionSetting.Visible = true;
        }

        protected void rbtnCheckParentSettingNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnCheckParentSettingNo.Checked)
                dvNodeRotationFieldOptionSetting.Visible = true;
            else
                dvNodeRotationFieldOptionSetting.Visible = false;
        }
        #endregion

        #endregion

        #region [Private Methods]
        private void ReadMode()
        {
            rbtnCourseNo.Enabled = false;
            rbtnDaysBeforeNo.Enabled = false;
            rbtnDaysNo.Enabled = false;
            rbtnDeadlineDateNo.Enabled = false;
            rbtnDepartmentNo.Enabled = false;
            rbtnEndTimeNo.Enabled = false;
            rbtnFrequencyNo.Enabled = false;
            rbtnHourNo.Enabled = false;
            rbtnIPNo.Enabled = false;
            rbtnProgramNo.Enabled = false;
            rbtnRotationNameNo.Enabled = false;
            rbtnShiftNo.Enabled = false;
            rbtnStartTimeNo.Enabled = false;
            rbtnStudentNo.Enabled = false;
            rbtnSyllabusNo.Enabled = false;
            rbtnTermNo.Enabled = false;
            rbtnTypeNo.Enabled = false;
            rbtnUnitNo.Enabled = false;

            rbtnCourseYes.Enabled = false;
            rbtnDaysBeforeYes.Enabled = false;
            rbtnDaysYes.Enabled = false;
            rbtnDeadlineDateYes.Enabled = false;
            rbtnDepartmentYes.Enabled = false;
            rbtnEndTimeYes.Enabled = false;
            rbtnFrequencyYes.Enabled = false;
            rbtnHourYes.Enabled = false;
            rbtnIPYes.Enabled = false;
            rbtnProgramYes.Enabled = false;
            rbtnRotationNameYes.Enabled = false;
            rbtnShiftYes.Enabled = false;
            rbtnStartTimeYes.Enabled = false;
            rbtnStudentYes.Enabled = false;
            rbtnSyllabusYes.Enabled = false;
            rbtnAdditionalDocumentsYes.Enabled = false;
            rbtnTermYes.Enabled = false;
            rbtnTypeYes.Enabled = false;
            rbtnUnitYes.Enabled = false;

            fsucCmdBar.Visible = false;
            //btnUpdate.Visible = true;
        }
        private void EditMode()
        {
            rbtnAdditionalDocumentsYes.Enabled = true;
            rbtnCourseNo.Enabled = true;
            rbtnDaysBeforeNo.Enabled = true;
            rbtnDaysNo.Enabled = true;
            rbtnDeadlineDateNo.Enabled = true;
            rbtnDepartmentNo.Enabled = true;
            rbtnEndTimeNo.Enabled = true;
            rbtnFrequencyNo.Enabled = true;
            rbtnHourNo.Enabled = true;
            rbtnIPNo.Enabled = true;
            rbtnProgramNo.Enabled = true;
            rbtnRotationNameNo.Enabled = true;
            rbtnShiftNo.Enabled = true;
            rbtnStartTimeNo.Enabled = true;
            rbtnStudentNo.Enabled = true;
            rbtnSyllabusNo.Enabled = true;
            rbtnTermNo.Enabled = true;
            rbtnTypeNo.Enabled = true;
            rbtnUnitNo.Enabled = true;
            rbtnCourseYes.Enabled = true;
            rbtnDaysBeforeYes.Enabled = true;
            rbtnDaysYes.Enabled = true;
            rbtnDeadlineDateYes.Enabled = true;
            rbtnDepartmentYes.Enabled = true;
            rbtnEndTimeYes.Enabled = true;
            rbtnFrequencyYes.Enabled = true;
            rbtnHourYes.Enabled = true;
            rbtnIPYes.Enabled = true;
            rbtnProgramYes.Enabled = true;
            rbtnRotationNameYes.Enabled = true;
            rbtnShiftYes.Enabled = true;
            rbtnStartTimeYes.Enabled = true;
            rbtnStudentYes.Enabled = true;
            rbtnSyllabusYes.Enabled = true;
            rbtnTermYes.Enabled = true;
            rbtnTypeYes.Enabled = true;
            rbtnUnitYes.Enabled = true;

            //fsucCmdBar.Visible = true;
            //btnUpdate.Visible = false;
        }
        private void ResetControl()
        {
            rbtnCourseYes.Checked = true;
            rbtnDaysBeforeNo.Checked = true;
            rbtnDaysNo.Checked = true;
            rbtnDeadlineDateNo.Checked = true;
            rbtnDepartmentYes.Checked = true;
            rbtnEndTimeNo.Checked = true;
            rbtnFrequencyNo.Checked = true;
            rbtnHourNo.Checked = true;
            rbtnIPNo.Checked = true;
            rbtnProgramYes.Checked = true;
            rbtnRotationNameNo.Checked = true;
            rbtnShiftNo.Checked = true;
            rbtnStartTimeNo.Checked = true;
            rbtnStudentNo.Checked = true;
            rbtnSyllabusNo.Checked = true;
            rbtnAdditionalDocumentsNo.Checked = true;
            rbtnTermNo.Checked = true;
            rbtnTypeNo.Checked = true;
            rbtnUnitNo.Checked = true;
            if (CurrentViewContext.IsRootNode)
                rbtnCheckParentSettingNo.Checked = true;
            else
            {
                rbtnCheckParentSettingYes.Checked = true;
                dvNodeRotationFieldOptionSetting.Visible = false;
            }
            //btnUpdate.Visible = false;
        }

        private void GetRotationFieldOptionPermissions()
        {
            var rotationFieldOptionPermissions = CurrentViewContext.AgencyHierarchyRotationFieldOptionContract;

            if (rotationFieldOptionPermissions.AHRFO_IsCourse_Required.Value)
                rbtnCourseYes.Checked = true;
            else
                rbtnCourseNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsDaysBefore_Required.Value)
                rbtnDaysBeforeYes.Checked = true;
            else
                rbtnDaysBeforeNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsDeadlineDate_Required.Value)
                rbtnDeadlineDateYes.Checked = true;
            else
                rbtnDeadlineDateNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsDepartment_Required.Value)
                rbtnDepartmentYes.Checked = true;
            else
                rbtnDepartmentNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsEndTime_Required.Value)
                rbtnEndTimeYes.Checked = true;
            else
                rbtnEndTimeNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsFrequency_Required.Value)
                rbtnFrequencyYes.Checked = true;
            else
                rbtnFrequencyNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsIP_Required.Value)
                rbtnIPYes.Checked = true;
            else
                rbtnIPNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsNoOfHours_Required.Value)
                rbtnHourYes.Checked = true;
            else
                rbtnHourNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsNoOfStudents_Required.Value)
                rbtnStudentYes.Checked = true;
            else
                rbtnStudentNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsProgram_Required.Value)
                rbtnProgramYes.Checked = true;
            else
                rbtnProgramNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsRotationName_Required.Value)
                rbtnRotationNameYes.Checked = true;
            else
                rbtnRotationNameNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsRotationShift_Required.Value)
                rbtnShiftYes.Checked = true;
            else
                rbtnShiftNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsRotDays_Required.Value)
                rbtnDaysYes.Checked = true;
            else
                rbtnDaysNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsStartTime_Required.Value)
                rbtnStartTimeYes.Checked = true;
            else
                rbtnStartTimeNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsSyllabusDocument_Required.Value)
                rbtnSyllabusYes.Checked = true;
            else
                rbtnSyllabusNo.Checked = true;

            #region 4062
            if (rotationFieldOptionPermissions.AHRFO_IsAdditionalDocuments_Required.Value)
                rbtnAdditionalDocumentsYes.Checked = true;
            else
                rbtnAdditionalDocumentsNo.Checked = true; 
            #endregion

            if (rotationFieldOptionPermissions.AHRFO_IsTerm_Required.Value)
                rbtnTermYes.Checked = true;
            else
                rbtnTermNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsTypeSpecialty_Required.Value)
                rbtnTypeYes.Checked = true;
            else
                rbtnTypeNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_IsUnitFloorLoc_Required.Value)
                rbtnUnitYes.Checked = true;
            else
                rbtnUnitNo.Checked = true;

            if (rotationFieldOptionPermissions.AHRFO_CheckParentSetting)
            {
                rbtnCheckParentSettingYes.Checked = true;
                dvNodeRotationFieldOptionSetting.Visible = false;
            }
            else
                rbtnCheckParentSettingNo.Checked = true;
        }
        #endregion
    }
}