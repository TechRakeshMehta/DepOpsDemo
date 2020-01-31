using CoreWeb.ClinicalRotation.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class AddEditRotation : BaseUserControl, IAddEditRotationView
    {

        #region [Private Variables]

        private AddEditRotationPresenter _presenter = new AddEditRotationPresenter();
        private Int32 _tenantId;
        private ClinicalRotationDetailContract _viewContract = null;
        private Boolean IsFieldsUpdated = false;
        private Boolean IsAgencyChanged = false;

        #endregion

        #region [Properties]

        /// <summary>
        /// Get The Multiple Additional Document values
        /// </summary>
        List<MultipleAdditionalDocumentsContract> IAddEditRotationView.MultipleAdditionalDocumentsContract
        {
            get
            {
                if (ViewState["MultipleAdditionalDocumentsContract"].IsNotNull())
                    return ViewState["MultipleAdditionalDocumentsContract"] as List<MultipleAdditionalDocumentsContract>;
                return new List<MultipleAdditionalDocumentsContract>();
            }
            set
            {
                ViewState["MultipleAdditionalDocumentsContract"] = value;
            }
        }
        public AddEditRotationPresenter Presenter
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

        public IAddEditRotationView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String IAddEditRotationView.ErrorMessage
        {
            get;
            set;
        }

        Int32 IAddEditRotationView.TenantId
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

        String IAddEditRotationView.SuccessMessage
        {
            get;
            set;
        }

        List<ClientContactContract> IAddEditRotationView.ClientContactList
        {
            get
            {
                if (!ViewState["ClientContactList"].IsNull())
                {
                    return ViewState["ClientContactList"] as List<ClientContactContract>;
                }

                return new List<ClientContactContract>();
            }
            set
            {
                ViewState["ClientContactList"] = value;
            }
        }



        List<AgencyDetailContract> IAddEditRotationView.lstAgency
        {
            get;
            set;
        }

        Dictionary<String, String> IAddEditRotationView.dicGranularPermissions
        {
            get;
            set;
        }

        Int32 IAddEditRotationView.SelectedAgencyID
        {
            get;
            set;
        }

        List<TenantDetailContract> IAddEditRotationView.lstTenant
        {
            get;
            set;
        }

        List<CustomAttribteContract> IAddEditRotationView.GetCustomAttributeList
        {
            get;
            set;
        }

        Int32 IAddEditRotationView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public string hierarchyID
        {
            get;

            set;

        }

        public bool IsCloneRotationCheckbox
        {
            get;
            set;

        }

        public Int32 CloneRotationId
        {
            get
            {
                if (ViewState["CloneRotationId"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["CloneRotationId"]);
                }
                return 0;
            }
            set
            {
                ViewState["CloneRotationId"] = value;
            }
        }

        public AgencyHierarchyRotationFieldOptionContract agencyHierarchyRotationFieldOptionContract
        {
            get
            {
                if (!ViewState["AgencyHierarchyRotationFieldOptionContract"].IsNull())
                {
                    return ViewState["AgencyHierarchyRotationFieldOptionContract"] as AgencyHierarchyRotationFieldOptionContract;
                }

                return new AgencyHierarchyRotationFieldOptionContract();
            }
            set
            {
                ViewState["AgencyHierarchyRotationFieldOptionContract"] = value;
            }


        }

        Int32 IAddEditRotationView.SelectedTenantID
        {
            get
            {
                if (ViewState["SelectedTenantID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["SelectedTenantID"]);
                }
                return 0;
            }
            set
            {
                ViewState["SelectedTenantID"] = value;
            }
        }

        List<WeekDayContract> IAddEditRotationView.WeekDayList
        {
            get;
            set;
        }

        String IAddEditRotationView.HierarchyNode
        {
            get;
            set;
        }

        ClinicalRotationDetailContract IAddEditRotationView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ClinicalRotationDetailContract();
                }
                return _viewContract;
            }
        }

        List<Int32> IAddEditRotationView.SelectedClientContacts
        {
            get;
            set;
        }

        List<CustomAttribteContract> IAddEditRotationView.SaveCustomAttributeList
        {
            get;
            set;
        }

        #region UAT-2424
        public List<ClinicalRotationDetailContract> lstClinicalRotation
        {
            get
            {
                if (!ViewState["lstClinicalRotation"].IsNull())
                {
                    return ViewState["lstClinicalRotation"] as List<ClinicalRotationDetailContract>;
                }

                return new List<ClinicalRotationDetailContract>();
            }
            set
            {
                ViewState["lstClinicalRotation"] = value;
            }
        }

        Int32 IAddEditRotationView.SelectedRotationIDForCloning
        {
            get
            {
                if (!ViewState["SelectedRotationIDForCloning"].IsNull())
                {
                    return (Int32)ViewState["SelectedRotationIDForCloning"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedRotationIDForCloning"] = value;
            }
        }

        public ClinicalRotationDetailContract CloneContract
        {
            get
            {
                if (!ViewState["CloneContract"].IsNull())
                {
                    return ViewState["CloneContract"] as ClinicalRotationDetailContract;
                }

                return new ClinicalRotationDetailContract();
            }
            set
            {
                ViewState["CloneContract"] = value;
            }
        }
        #endregion

        Int32 IAddEditRotationView.SelectedRotationID
        {
            get
            {
                if (ViewState["SelectedRotationID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["SelectedRotationID"]);
                }
                return 0;
            }
            set
            {
                ViewState["SelectedRotationID"] = value;
            }
        }

        ClinicalRotationDetailContract IAddEditRotationView.clinicalRotationDetailContract
        {
            get
            {
                if (!ViewState["ClinicalRotationDetailContract"].IsNull())
                {
                    return ViewState["ClinicalRotationDetailContract"] as ClinicalRotationDetailContract;
                }

                return new ClinicalRotationDetailContract();
            }
            set
            {
                ViewState["ClinicalRotationDetailContract"] = value;
            }
        }

        List<RotationFieldUpdatedByAgencyContract> IAddEditRotationView.lstRotationFieldUpdaeByAgency
        {
            get
            {
                if (!ViewState["lstRotationFieldUpdaeByAgency"].IsNull())
                {
                    return ViewState["lstRotationFieldUpdaeByAgency"] as List<RotationFieldUpdatedByAgencyContract>;
                }

                return new List<RotationFieldUpdatedByAgencyContract>();
            }
            set
            {
                ViewState["lstRotationFieldUpdaeByAgency"] = value;
            }
        }


        String IAddEditRotationView.CommandName
        {
            get
            {
                if (!ViewState["CommandName"].IsNull())
                {
                    return ViewState["CommandName"] as String;
                }

                return String.Empty;
            }
            set
            {
                ViewState["CommandName"] = value;
            }
        }

        #region UAT-3121
        Boolean IAddEditRotationView.IsApplicantPkgNotAssignedThroughCloning
        {
            get
            {
                if (!ViewState["IsApplicantPkgNotAssignedThroughCloning"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsApplicantPkgNotAssignedThroughCloning"]);
                }
                return false;
            }
            set
            {
                ViewState["IsApplicantPkgNotAssignedThroughCloning"] = value;
            }
        }
        Boolean IAddEditRotationView.IsInstructorPkgNotAssignedThroughCloning
        {
            get
            {
                if (!ViewState["IsInstructorPkgNotAssignedThroughCloning"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsInstructorPkgNotAssignedThroughCloning"]);
                }
                return false;
            }
            set
            {
                ViewState["IsInstructorPkgNotAssignedThroughCloning"] = value;
            }
        }
        #endregion

        List<AgencyHierarchyRootNodeSettingContract> IAddEditRotationView.TypeSpecialtyList
        {
            get;
            set;
        }

        Boolean IAddEditRotationView.IsInstAvailabilityDefined
        {
            get
            {
                if (!ViewState["IsInstAvailabilityDefined"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsInstAvailabilityDefined"]);
                return false;
            }
            set
            {
                ViewState["IsInstAvailabilityDefined"] = value;
                hdnIsInstAvailabilityDefined.Value = value.ToString();
            }
        }

        List<RotationMemberDetailContract> IAddEditRotationView.RotationMemberDetailList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the value for Rotation Start Date.
        /// </summary>
        DateTime? IAddEditRotationView.RotationStartDate
        {
            get
            {
                if (!ViewState["RotationStartDate"].IsNull())
                {
                    return Convert.ToDateTime(ViewState["RotationStartDate"]);
                }
                return null;
            }
            set
            {
                ViewState["RotationStartDate"] = value;
            }
        }

        /// <summary>
        /// Gets or Sets the value for Is Rotation Start.
        /// </summary>
        Boolean IAddEditRotationView.IsRotationStart
        {
            get
            {
                if (!ViewState["IsRotationStart"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsRotationStart"]);
                }
                return false;
            }
            set
            {
                ViewState["IsRotationStart"] = value;
            }
        }

        #endregion

        #region [Page Events]

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Request.QueryString["TenantID"].IsNullOrEmpty())
                {
                    CurrentViewContext.SelectedTenantID = Convert.ToInt32(Request.QueryString["TenantID"]);
                }
                if (!Request.QueryString["RotationID"].IsNullOrEmpty())
                {
                    CurrentViewContext.SelectedRotationID = Convert.ToInt32(Request.QueryString["RotationID"]);
                }
                //lblMsg.Visible = false;
                if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
                {

                    if (!Page.IsPostBack)
                    {
                        BindControls();
                        SetInstPreceptorValidation(); //UAT-2554                        
                        ((HiddenField)ucAgencyHierarchyMultiple.FindControl("hdnIsRequestFromAddRotationScrn")).Value = "Yes"; // UAT-4443
                    }

                    GenerateCustomAttributes();

                }
                if (Page.IsPostBack)
                {
                    BindAdditionalDocuments(true);
                }
                //ucAgencyHierarchy.TenantId = CurrentViewContext.SelectedTenantID; // UAT-2600
                //ucAgencyHierarchy.IsInstitutionHierarchyRequired = true;
                ucAgencyHierarchyMultiple.TenantId = CurrentViewContext.SelectedTenantID;
                ucAgencyHierarchyMultiple.IsInstitutionHierarchyRequired = true;
                ucAgencyHierarchyMultiple.AgencyHierarchyNodeSelection = true;
                ucAgencyHierarchyMultiple.IsAgencyNodeCheckable = false;
                if (!String.IsNullOrEmpty(hdnInstNodeLabel.Value))
                    lblInstitutionHierarchyPB.Text = hdnInstNodeLabel.Value;
                //UAT 3041
                if (!Presenter.IsAdminLoggedIn())
                {
                    chkEditPermission.Items.Remove(chkEditPermission.Items.FindByValue("CA"));
                }

                if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
                {
                    //UAT-4150
                    Presenter.InstAvailabilityHierarchyRootNodeSetting(ucAgencyHierarchyMultiple.SelectedRootNodeId);
                    ManageInstAvailabilitySetting(false);
                    //end
                    if (CurrentViewContext.SelectedRotationID > AppConsts.NONE)
                    {
                        if (!Page.IsPostBack)
                        {
                            LoadControlData();
                            //UAT-4441
                            if (!CurrentViewContext.clinicalRotationDetailContract.IsNullOrEmpty()
                                && CurrentViewContext.clinicalRotationDetailContract.EndDate < DateTime.Now && !Presenter.IsAdminLoggedIn())
                            {
                                HideControls();
                            }
                        }

                        GenerateCustomAttributeByrRotationID();
                    }
                    else if (CurrentViewContext.SelectedRotationID == AppConsts.MINUS_ONE)
                    {
                        divClearFix.Attributes["Class"] = "";
                        divAgencyHierarchy.Attributes["Class"] = "";
                        divAgencyHierarchy.Attributes.Add("Class", "form-group col-md-3 col-sm-3");
                        lnkInstitutionHierarchyPB.Attributes["Class"] = "";
                        BindTypeSpecialtyOptions(cmbTypeSpecialty);
                    }
                    else
                    { BindTypeSpecialtyOptions(cmbTypeSpecialty); }

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

        private void GenerateCustomAttributes()
        {
            Int32? rotationID = null;
            if (CurrentViewContext.SelectedRotationIDForCloning > AppConsts.NONE)
            {
                rotationID = CurrentViewContext.SelectedRotationIDForCloning;
            }
            Presenter.GetCustomAttributeList(rotationID);
            if (!CurrentViewContext.GetCustomAttributeList.IsNullOrEmpty())
            {
                SharedUserCustomAttributeForm caCustomAttributesExisting = pnlEditForm.FindControl("caCustomAttributes") as SharedUserCustomAttributeForm;
                pnlEditForm.Controls.Remove(caCustomAttributesExisting); //First remove the control of custome attributes that is already added in pageload event
                SharedUserCustomAttributeForm caCustomAttributes = (SharedUserCustomAttributeForm)Page.LoadControl("~\\ClinicalRotation\\UserControl\\SharedUserCustomAttributeForm.ascx");
                caCustomAttributes.ID = "caCustomAttributes";
                GenerateCustomAttributes(caCustomAttributes);
                pnlEditForm.Controls.Add(caCustomAttributes);//Add control of custome attributes again
            }
        }
        private void GenerateCustomAttributeByrRotationID()
        {
            Presenter.GetCustomAttributeList(CurrentViewContext.SelectedRotationID);
            if (!CurrentViewContext.GetCustomAttributeList.IsNullOrEmpty())
            {
                SharedUserCustomAttributeForm caCustomAttributesExisting = pnlEditForm.FindControl("caCustomAttributes") as SharedUserCustomAttributeForm;
                pnlEditForm.Controls.Remove(caCustomAttributesExisting); //First remove the control of custome attributes that is already added in pageload event
                SharedUserCustomAttributeForm caCustomAttributes = (SharedUserCustomAttributeForm)Page.LoadControl("~\\ClinicalRotation\\UserControl\\SharedUserCustomAttributeForm.ascx");
                caCustomAttributes.ID = "caCustomAttributes";
                GenerateCustomAttributes(caCustomAttributes);
                pnlEditForm.Controls.Add(caCustomAttributes);
            }
        }
        #region UAT-2554
        private void SetInstPreceptorValidation()
        {
            //if (!ddlAgency.IsNullOrEmpty() && !ddlAgency.SelectedValue.IsNullOrEmpty())
            //UAT-2600
            //if (!ucAgencyHierarchyMultiple.IsNullOrEmpty() && ucAgencyHierarchy.AgencyId > AppConsts.NONE)
            //{
            // CurrentViewContext.SelectedAgencyID = Convert.ToInt32(ddlAgency.SelectedValue);  [ddl]  
            //CurrentViewContext.SelectedAgencyID = ucAgencyHierarchy.AgencyId;
            //if (Presenter.IsPreceptorRequiredForAgency())
            //{
            //    cstValidator.Enabled = true;
            //    spnInsPre.Attributes["class"] = "reqd";
            //}
            //else
            //{
            //    cstValidator.Enabled = false;
            //    spnInsPre.Attributes["class"] = "controlHidden reqd";
            //}
            //}
        }
        #endregion

        #endregion

        #region [Private Methods]

        private void BindControls()
        {
            // Get Granular Permissions, only for Client Admin
            if (!Presenter.IsAdminLoggedIn())
            {
                Presenter.GetGranularPermissions();
                ApplyGranularPermissions();
            }

            Presenter.GetTenants();
            ddlTenant.DataSource = CurrentViewContext.lstTenant;
            ddlTenant.DataBind();
            ddlTenant.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));

            if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
            {
                ddlTenant.Enabled = false;
                ddlTenant.SelectedValue = CurrentViewContext.SelectedTenantID.ToString();

                //ToDo
                //CurrentViewContext.lstAgencyForAddForm = CurrentViewContext.lstAgency;
                //if (CurrentViewContext.SelectedAgencyID > AppConsts.NONE)
                //{
                //    CurrentViewContext.SelectedAgencyIDForAddForm = CurrentViewContext.SelectedAgencyID;
                //}
            }
            else
            {
                ddlTenant.Enabled = true;
            }

            // BindAgency(ddlAgency); [ddl]
            BindWeekDays(ddlDays);
            BindContacts(ddlInstructor);
            BindRotationForAddForm(ddlRotation);
            dvComplioId.Visible = false;
            txtDaysBefore.Text = "30";
            txtFrequency.Text = "15";

            if (!Presenter.IsAdminLoggedIn())
            {
                Dictionary<Int32, String> dicDefaultNode = Presenter.GetDefaultPermissionForClientAdmin();
                if (!dicDefaultNode.IsNullOrEmpty())
                {
                    hdnDepartmentProgmapNew.Value = Convert.ToString(dicDefaultNode.Keys.FirstOrDefault());
                    hdnInstNodeIdNew.Value = Convert.ToString(dicDefaultNode.Keys.FirstOrDefault());
                    hdnInstNodeLabel.Value = dicDefaultNode.Values.FirstOrDefault();
                    lblInstitutionHierarchyPB.Text = dicDefaultNode.Values.FirstOrDefault();
                    ucAgencyHierarchyMultiple.SelectedInstitutionNodeIds = Convert.ToString(dicDefaultNode.Keys.FirstOrDefault());
                }
            }
            // BindTypeSpecialtyOptions(cmbTypeSpecialty);
        }

        private void LoadControlData()
        {
            if (CurrentViewContext.SelectedRotationID > AppConsts.NONE)
            {
                Presenter.GetClinicalRotationById();

                if (CurrentViewContext.clinicalRotationDetailContract != null)
                {
                    dvComplioId.Visible = true;
                    dvCloneRotation.Visible = false;
                    lblRotation.Text = "Update Rotation";
                    CurrentViewContext.CommandName = "UpdateCommandName";
                    divClearFix.Attributes["Class"] = "";
                    divAgencyHierarchy.Attributes["Class"] = "";
                    divAgencyHierarchy.Attributes.Add("Class", "form-group col-md-3 col-sm-3");

                    //ddlAgency.Enabled = false; [ddl]
                    //ddlAgency.SelectedValue = clinicalRotationDetail.AgencyID.ToString();
                    dpStartDate.SelectedDate = CurrentViewContext.clinicalRotationDetailContract.StartDate;
                    dpEndDate.SelectedDate = CurrentViewContext.clinicalRotationDetailContract.EndDate;
                    tpStartTime.SelectedTime = CurrentViewContext.clinicalRotationDetailContract.StartTime;
                    tpEndTime.SelectedTime = CurrentViewContext.clinicalRotationDetailContract.EndTime;
                    //UAT-2514
                    // hdnCurrentRotStartDate.Value = dpStartDate.SelectedDate.Value.ToString();
                    txtComplioId.Text = CurrentViewContext.clinicalRotationDetailContract.ComplioID;
                    txtRotationName.Text = CurrentViewContext.clinicalRotationDetailContract.RotationName;
                    txtTypeSpecialty.Text = CurrentViewContext.clinicalRotationDetailContract.TypeSpecialty;
                    txtDepartment.Text = CurrentViewContext.clinicalRotationDetailContract.Department;
                    txtProgram.Text = CurrentViewContext.clinicalRotationDetailContract.Program;
                    txtCourse.Text = CurrentViewContext.clinicalRotationDetailContract.Course;
                    txtTerm.Text = CurrentViewContext.clinicalRotationDetailContract.Term;
                    txtUnit.Text = CurrentViewContext.clinicalRotationDetailContract.UnitFloorLoc;
                    txtStudents.Text = CurrentViewContext.clinicalRotationDetailContract.Students.ToString();
                    txtRecommendedHrs.Text = CurrentViewContext.clinicalRotationDetailContract.RecommendedHours.ToString();
                    txtShift.Text = CurrentViewContext.clinicalRotationDetailContract.Shift;



                    //string agencyIds = Convert.ToString((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyIDs"]);
                    //rotAgencyID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyID"]);
                    //agencyHierarchyID = Convert.ToString((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyHierarchyID"]);
                    //rootNodeID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RootNodeID"]);



                    ucAgencyHierarchyMultiple.SelectedNodeIds = CurrentViewContext.clinicalRotationDetailContract.AgencyHierarchyIDs;
                    ucAgencyHierarchyMultiple.SelectedAgecnyIds = CurrentViewContext.clinicalRotationDetailContract.AgencyIDs;



                    CurrentViewContext.hierarchyID = ucAgencyHierarchyMultiple.SelectedAgecnyIds;
                    Presenter.GetValidationRotation();

                    if (CurrentViewContext.agencyHierarchyRotationFieldOptionContract != null && !CurrentViewContext.hierarchyID.IsNullOrEmpty())
                    {


                        hdnValidateFileUploadControl.Value = CurrentViewContext.agencyHierarchyRotationFieldOptionContract.AHRFO_IsSyllabusDocument_Required.ToString().ToLower();

                        if (CurrentViewContext.agencyHierarchyRotationFieldOptionContract.AHRFO_IsSyllabusDocument_Required.ToString().ToLower() == "true")
                        {
                            spnSyllabus.Attributes.Add("Class", "reqd");
                        }
                        if (CurrentViewContext.agencyHierarchyRotationFieldOptionContract.AHRFO_IsAdditionalDocuments_Required.ToString().ToLower() == "true")
                        {
                            spnAdditional.Attributes.Add("Class", "reqd");
                        }

                        hdnValidateAdditionalControl.Value = CurrentViewContext.agencyHierarchyRotationFieldOptionContract.AHRFO_IsAdditionalDocuments_Required.ToString().ToLower();

                    }

                    Session["NodeSelected"] = CurrentViewContext.clinicalRotationDetailContract.AgencyHierarchyIDs;
                    Session["AgencySelected"] = CurrentViewContext.clinicalRotationDetailContract.AgencyIDs;
                    ucAgencyHierarchyMultiple.SelectedRootNodeId = CurrentViewContext.clinicalRotationDetailContract.RootNodeID;

                    // ucAgencyHierarchyAddEditRotation.IsReadOnlyMode = true;
                    ucAgencyHierarchyMultiple.IsInDisableMode = true;

                    lnkInstitutionHierarchyPB.Attributes["Class"] = "disabled";
                    if (CurrentViewContext.clinicalRotationDetailContract.AgencyID > AppConsts.NONE)
                    {
                        ucAgencyHierarchyMultiple.SelectedAgecnyIds = CurrentViewContext.clinicalRotationDetailContract.AgencyIDs.ToString();
                        Session["AgencySelected"] = CurrentViewContext.clinicalRotationDetailContract.AgencyIDs;
                    }

                    //UAT-2289
                    dpDeadlineDate.SelectedDate = CurrentViewContext.clinicalRotationDetailContract.DeadlineDate;
                    //UAT-2905
                    chkAllowNotification.Checked = CurrentViewContext.clinicalRotationDetailContract.IsAllowNotification;

                    //UAT 3041 
                    foreach (ListItem item in chkEditPermission.Items)
                    {
                        if (item.Value.ToString() == "CA")
                        {
                            item.Selected = CurrentViewContext.clinicalRotationDetailContract.IsEditableByClientAdmin;
                        }

                        if (item.Value.ToString() == "AGU")
                        {
                            item.Selected = CurrentViewContext.clinicalRotationDetailContract.IsEditableByAgencyUser;
                        }
                    }

                    //UAT 1414 notification to go out prior to student's start date for clinical rotation
                    txtDaysBefore.Text = CurrentViewContext.clinicalRotationDetailContract.DaysBefore.ToString();
                    txtFrequency.Text = CurrentViewContext.clinicalRotationDetailContract.Frequency;

                    if (!CurrentViewContext.clinicalRotationDetailContract.ContactIdList.IsNullOrEmpty())
                    {
                        String[] selectedContactIds = CurrentViewContext.clinicalRotationDetailContract.ContactIdList.Split(',');
                        foreach (RadComboBoxItem item in ddlInstructor.Items)
                        {
                            item.Checked = selectedContactIds.Contains(item.Value);
                        }
                    }
                    if (!CurrentViewContext.clinicalRotationDetailContract.DaysIdList.IsNullOrEmpty())
                    {
                        String[] selectedDayIds = CurrentViewContext.clinicalRotationDetailContract.DaysIdList.Split(',');
                        foreach (RadComboBoxItem item in ddlDays.Items)
                        {
                            item.Checked = selectedDayIds.Contains(item.Value);
                        }
                    }
                    if (!CurrentViewContext.clinicalRotationDetailContract.SyllabusFileName.IsNullOrEmpty())
                    {
                        lblUploadFormName.Text = CurrentViewContext.clinicalRotationDetailContract.SyllabusFileName;
                        lblUploadFormPath.Text = CurrentViewContext.clinicalRotationDetailContract.SyllabusFilePath;
                        lblUploadFormName.Visible = true;
                        lnkRemove.Visible = true;
                        //fileUpload.Visible = false;
                    }

                    hdnDepartmentProgmapNew.Value = CurrentViewContext.clinicalRotationDetailContract.HierarchyNodeIDList;
                    hdnInstNodeIdNew.Value = CurrentViewContext.clinicalRotationDetailContract.HierarchyNodeIDList;
                    lblInstitutionHierarchyPB.Text = CurrentViewContext.clinicalRotationDetailContract.HierarchyNodes;

                    if (!hdnInstNodeIdNew.Value.IsNullOrEmpty())
                    {
                        ucAgencyHierarchyMultiple.SelectedInstitutionNodeIds = Convert.ToString(hdnInstNodeIdNew.Value);
                    }
                    #region UAT-2666
                    Int32 currentClinicalRotationId = CurrentViewContext.clinicalRotationDetailContract.RotationID;
                    RotationFieldUpdatedByAgencyContract RotationFieldUpdaeByAgency = CurrentViewContext.lstRotationFieldUpdaeByAgency.Where(cmd => cmd.ClinicalRotationID == currentClinicalRotationId).FirstOrDefault();

                    //HtmlGenericControl dvRotationName = (editform.FindControl("dvRotationName") as HtmlGenericControl);
                    //HtmlGenericControl dvType = (editform.FindControl("dvType") as HtmlGenericControl);
                    //HtmlGenericControl dvDepartment = (editform.FindControl("dvDepartment") as HtmlGenericControl);
                    //HtmlGenericControl dvProgram = (editform.FindControl("dvProgram") as HtmlGenericControl);
                    //HtmlGenericControl dvCourse = (editform.FindControl("dvCourse") as HtmlGenericControl);
                    //HtmlGenericControl dvTerm = (editform.FindControl("dvTerm") as HtmlGenericControl);
                    //HtmlGenericControl dvUnitLoc = (editform.FindControl("dvUnitLoc") as HtmlGenericControl);
                    //HtmlGenericControl dvStudent = (editform.FindControl("dvStudent") as HtmlGenericControl);
                    //HtmlGenericControl dvHour = (editform.FindControl("dvHour") as HtmlGenericControl);
                    //HtmlGenericControl dvShift = (editform.FindControl("dvShift") as HtmlGenericControl);

                    if (!RotationFieldUpdaeByAgency.IsNullOrEmpty())
                    {
                        if (RotationFieldUpdaeByAgency.IsCourseUpdated)
                        {
                            dvCourse.Attributes.Add("class", "highlight");
                        }
                        else
                        {
                            dvCourse.Attributes.Add("class", "");
                        }

                        if (RotationFieldUpdaeByAgency.IsDepartmentUpdated)
                        {
                            dvDepartment.Attributes.Add("class", "highlight");
                        }
                        else
                        {
                            dvDepartment.Attributes.Add("class", "");
                        }

                        if (RotationFieldUpdaeByAgency.IsNoOfHoursUpdated)
                        {
                            dvHour.Attributes.Add("class", "highlight");
                        }
                        else
                        {
                            dvHour.Attributes.Add("class", "");
                        }

                        if (RotationFieldUpdaeByAgency.IsNoOfStudentsUpdated)
                        {
                            dvStudent.Attributes.Add("class", "highlight");
                        }
                        else
                        {
                            dvStudent.Attributes.Add("class", "");
                        }

                        if (RotationFieldUpdaeByAgency.IsProgramUpdated)
                        {
                            dvProgram.Attributes.Add("class", "highlight");
                        }
                        else
                        {
                            dvProgram.Attributes.Add("class", "");
                        }

                        if (RotationFieldUpdaeByAgency.IsRotationNameUpadted)
                        {
                            dvRotationName.Attributes.Add("class", "highlight");
                        }
                        else
                        {
                            dvRotationName.Attributes.Add("class", "");
                        }

                        if (RotationFieldUpdaeByAgency.IsRotationShiftUpdated)
                        {
                            dvShift.Attributes.Add("class", "highlight");
                        }
                        else
                        {
                            dvShift.Attributes.Add("class", "");
                        }

                        if (RotationFieldUpdaeByAgency.IsTermUpdated)
                        {
                            dvTerm.Attributes.Add("class", "highlight");
                        }
                        else
                        {
                            dvTerm.Attributes.Add("class", "");
                        }

                        if (RotationFieldUpdaeByAgency.IsTypeSpecialtyUpdated)
                        {
                            dvType.Attributes.Add("class", "highlight");
                        }
                        else
                        {
                            dvType.Attributes.Add("class", "");
                        }

                        if (RotationFieldUpdaeByAgency.IsUnitFloorLocUpdated)
                        {
                            dvUnitLoc.Attributes.Add("class", "highlight");
                        }
                        else
                        {
                            dvUnitLoc.Attributes.Add("class", "");
                        }
                    }
                    #endregion

                    #region UAT: 4062
                    BindAdditionalDocuments();
                    #endregion
                    #region UAT: 3961
                    hdnSelectRootNodeID.Value = String.Empty;
                    //UAT 3961

                    if (String.IsNullOrEmpty(txtTypeSpecialty.Text))
                    {
                        BindTypeSpecialtyOptions(cmbTypeSpecialty);
                    }
                    else
                    {
                        cmbTypeSpecialty.Visible = false;
                        txtTypeSpecialty.Visible = true;


                        rfvTypeSpecialtyAddEdit.ControlToValidate = "txtTypeSpecialty";
                        rfvTypeSpecialtyAddEdit.InitialValue = "";
                        // rfvCombTypeSpecialtyAddEdit.Visible = false;


                    }

                    if (CurrentViewContext.clinicalRotationDetailContract.AgnecyHierarchyRootNodeSettingMappingID > AppConsts.NONE)
                    {
                        foreach (RadComboBoxItem item in cmbTypeSpecialty.Items)
                        {
                            if (item.Value == Convert.ToString(CurrentViewContext.clinicalRotationDetailContract.AgnecyHierarchyRootNodeSettingMappingID))
                            {
                                item.Selected = true;
                            }
                        }
                    }
                    else
                    {
                        cmbTypeSpecialty.ClearSelection();
                    }
                    #endregion

                    #region UAT-4150
                    Presenter.InstAvailabilityHierarchyRootNodeSetting(ucAgencyHierarchyMultiple.SelectedRootNodeId);
                    ManageInstAvailabilitySetting(true, CurrentViewContext.clinicalRotationDetailContract.IsSchoolSendingInstructor);
                    #endregion
                    ////UAT-3221
                    //if (!(e.Item is GridEditFormInsertItem))
                    //{
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "markAgencyHierarchyLinkDisabled();", true);
                    //}
                }

            }


        }
        #region UAT-4062
        private void BindAdditionalDocuments(bool isPostBackCall = false)
        {
            if (isPostBackCall == false && hdnTempAdditionalDocumentIds.Value.IsNullOrEmpty())
            {
                BindAdditionalDoc(isPostBackCall);
            }
            else
                if (!hdnTempAdditionalDocumentIds.Value.IsNullOrEmpty())
            {
                BindAdditionalDoc(isPostBackCall);
            }
        }
        private void BindAdditionalDoc(bool isPostBackCall)
        {
            Panel PnlAdditionalDocument = pnlEditForm.FindControl("PnlAdditionalDocument") as Panel;
            PnlAdditionalDocument.Controls.Clear();
            if (CurrentViewContext.MultipleAdditionalDocumentsContract == null || CurrentViewContext.MultipleAdditionalDocumentsContract.Count() == AppConsts.NONE)
            {
                Presenter.GetAdditionalDocumnet();
                CurrentViewContext.MultipleAdditionalDocumentsContract = CurrentViewContext.ViewContract.listOfMultipleDocument;
            }
            if (PnlAdditionalDocument != null && CurrentViewContext.MultipleAdditionalDocumentsContract != null &&
                CurrentViewContext.MultipleAdditionalDocumentsContract.Count > 0)
            {
                int i = 0;
                hdnTempAdditionalDocumentIds.Value = string.Empty;
                foreach (var item in CurrentViewContext.MultipleAdditionalDocumentsContract)
                {
                    Label LblB = new Label();
                    LblB.Text = item.AdditionalDocumentFileName;
                    LblB.ID = "lbl_" + i;
                    LblB.CssClass = "lblClass_";
                    LblB.ClientIDMode = ClientIDMode.Static;
                    LinkButton linkB = new LinkButton();
                    linkB.ID = "LinkButton_" + i;
                    linkB.Text = " Remove " + "<br />";
                    linkB.Attributes.Add("href", "#");
                    linkB.OnClientClick = "RemoveRecords('" + item.AdditionalDocumentID + "',this,'" + "lbl_" + i + "')";
                    linkB.ToolTip = "Click this button to remove document";
                    PnlAdditionalDocument.Controls.Add(LblB);
                    PnlAdditionalDocument.Controls.Add(linkB);
                    
                        hdnTempAdditionalDocumentIds.Value += item.AdditionalDocumentID + ",";
                    
                    if (!CurrentViewContext.clinicalRotationDetailContract.IsNullOrEmpty()
                       && CurrentViewContext.clinicalRotationDetailContract.EndDate < DateTime.Now && !Presenter.IsAdminLoggedIn())
                    {
                        linkB.Enabled = false;
                        linkB.OnClientClick = null;
                        linkB.Attributes.Remove("href");
                    }
                    i++;
                }

            }
        }
        #endregion
        private void BindWeekDays(RadComboBox cmbDays)
        {
            Presenter.GetWeekDays();
            cmbDays.DataSource = CurrentViewContext.WeekDayList;
            cmbDays.DataBind();
        }
        private void BindContacts(RadComboBox cmbContacts)
        {
            Presenter.GetClientContacts();
            cmbContacts.DataSource = CurrentViewContext.ClientContactList;
            cmbContacts.DataBind();
        }
        private void BindTypeSpecialtyOptions(RadComboBox cmbTypeSpecialty)
        {
            if (ucAgencyHierarchyMultiple.IsNotNull() && ucAgencyHierarchyMultiple.SelectedRootNodeId > AppConsts.NONE)
            {
                Boolean IsAgencyHierarchyRootNodeSettingExist = Presenter.IsAgencyHierarchyRootNodeSettingExist(ucAgencyHierarchyMultiple.SelectedRootNodeId);

                if (IsAgencyHierarchyRootNodeSettingExist)
                {
                    if (String.IsNullOrEmpty(hdnSelectRootNodeID.Value) || hdnSelectRootNodeID.Value != ucAgencyHierarchyMultiple.SelectedRootNodeId.ToString())
                    {
                        cmbTypeSpecialty.Visible = true;
                        txtTypeSpecialty.Visible = false;
                        //rfvTypeSpecialtyAddEdit.Visible = false;
                        //rfvCombTypeSpecialtyAddEdit.Visible = true;
                        rfvTypeSpecialtyAddEdit.ControlToValidate = "cmbTypeSpecialty";

                        rfvTypeSpecialtyAddEdit.InitialValue = "--SELECT--";
                        Presenter.GetTypeSpecialtyOptions(ucAgencyHierarchyMultiple.SelectedRootNodeId);
                        cmbTypeSpecialty.DataSource = CurrentViewContext.TypeSpecialtyList;
                        cmbTypeSpecialty.DataBind();
                        cmbTypeSpecialty.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
                        hdnSelectRootNodeID.Value = ucAgencyHierarchyMultiple.SelectedRootNodeId.ToString();
                    }
                }
                else
                {
                    cmbTypeSpecialty.Visible = false;
                    txtTypeSpecialty.Visible = true;
                    //rfvTypeSpecialtyAddEdit.Visible = true;
                    //rfvCombTypeSpecialtyAddEdit.Visible = false;
                    rfvTypeSpecialtyAddEdit.InitialValue = "";
                    rfvTypeSpecialtyAddEdit.ControlToValidate = "txtTypeSpecialty";
                    hdnSelectRootNodeID.Value = String.Empty;
                }
            }
            else
            {
                cmbTypeSpecialty.Visible = false;
                txtTypeSpecialty.Visible = true;
                cmbTypeSpecialty.Visible = false;
                txtTypeSpecialty.Visible = true;
                // rfvTypeSpecialtyAddEdit.Visible = true;
                rfvTypeSpecialtyAddEdit.ControlToValidate = "txtTypeSpecialty";
                rfvTypeSpecialtyAddEdit.InitialValue = "";
            }
        }
        //private void BindAgency(RadComboBox ddlAgencyOnAddForm)   [ddl]
        //{
        //    if (CurrentViewContext.lstAgency.IsNullOrEmpty())
        //    {
        //        Presenter.GetAllAgency();
        //    }

        //    ddlAgencyOnAddForm.DataSource = CurrentViewContext.lstAgency;
        //    ddlAgencyOnAddForm.DataBind();
        //    ddlAgencyOnAddForm.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        //    if (CurrentViewContext.SelectedAgencyID > AppConsts.NONE)
        //    {
        //        ddlAgencyOnAddForm.SelectedValue = CurrentViewContext.SelectedAgencyID.ToString();
        //    }
        //}

        private void GenerateCustomAttributes(SharedUserCustomAttributeForm caCustomAttributes)
        {
            // Generate the control using database, but set the values from the session
            caCustomAttributes.TenantId = CurrentViewContext.SelectedTenantID;
            caCustomAttributes.TypeCode = CustomAttributeUseTypeContext.Hierarchy.GetStringValue();
            caCustomAttributes.DataSourceModeType = DataSourceMode.Ids;
            caCustomAttributes.Title = "Other Details";
            caCustomAttributes.ControlDisplayMode = DisplayMode.Controls;
            caCustomAttributes.CurrentLoggedInUserId = CurrentViewContext.CurrentLoggedInUserId;
            caCustomAttributes.ValidationGroup = "grpFormSubmit";
            caCustomAttributes.IsReadOnly = false;
            caCustomAttributes.lstTypeCustomAttributes = CurrentViewContext.GetCustomAttributeList;
            caCustomAttributes.EnableViewState = false;
            //UAT-4441
            if (!CurrentViewContext.clinicalRotationDetailContract.IsNullOrEmpty()
                      && CurrentViewContext.clinicalRotationDetailContract.EndDate < DateTime.Now && !Presenter.IsAdminLoggedIn())
            {
                caCustomAttributes.IsReadOnly = true;
                fsucCmdBarRotation.SubmitButton.Enabled = false;
            }
        }

        private void ApplyGranularPermissions()
        {
            var _managePkgPermissions = CurrentViewContext.dicGranularPermissions.Where(gp => gp.Key == EnumSystemEntity.ASSIGN_ROTATION_PACKAGE.GetStringValue()).FirstOrDefault();

            if (_managePkgPermissions.IsNotNull() && _managePkgPermissions.Key.IsNotNull() && _managePkgPermissions.Value.IsNotNull())
            {
                if (_managePkgPermissions.Value == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue())
                {
                    if (txtDaysBefore.IsNotNull() && txtFrequency.IsNotNull())
                    {
                        txtDaysBefore.Enabled = false;
                        txtFrequency.Enabled = false;
                    }
                }
            }
        }

        private Dictionary<string, string> UploadSyllabusDocuments(WclAsyncUpload fileUpload, string FileNameSyllOrAdditional)
        {

            String fileName = String.Empty;
            Dictionary<string, string> TempD = new Dictionary<string, string>();
            string savedFilePath = string.Empty;

            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];

            if (tempFilePath.IsNullOrEmpty())
            {
                base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                return TempD;
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "Tenant(" + CurrentViewContext.SelectedTenantID.ToString() + @")\";

            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);

            foreach (UploadedFile item in fileUpload.UploadedFiles)
            {
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                //Save file on temp location
                String newTempFilePath = Path.Combine(tempFilePath, fileName);
                item.SaveAs(newTempFilePath);
                String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                String destFileName = FileNameSyllOrAdditional + CurrentViewContext.SelectedTenantID.ToString() + "_" + date + Path.GetExtension(newTempFilePath);
                String desFilePath = "Tenant(" + CurrentViewContext.SelectedTenantID.ToString() + @")\" + destFileName;
                savedFilePath = CommonFileManager.SaveDocument(newTempFilePath, desFilePath, FileType.SystemDocumentLocation.GetStringValue());
                TempD.Add(item.FileName, savedFilePath);
                savedFilePath = string.Empty;
            }
            return TempD;
        }

        private void ResetControls()
        {
            CurrentViewContext.SelectedAgencyID = AppConsts.NONE;
            txtComplioId.Text = String.Empty;
            txtRotationName.Text = String.Empty;
            txtDepartment.Text = String.Empty;
            txtProgram.Text = String.Empty;
            txtCourse.Text = String.Empty;
            txtUnit.Text = String.Empty;
            txtRecommendedHrs.Text = String.Empty;
            txtStudents.Text = String.Empty;
            txtShift.Text = String.Empty;
            dpStartDate.Clear();
            dpEndDate.Clear();
            tpStartTime.Clear();
            tpEndTime.Clear();
            ddlDays.ClearCheckedItems();
            ddlInstructor.ClearCheckedItems();
            txtTerm.Text = String.Empty;
            txtTypeSpecialty.Text = String.Empty;
            hdnInstNodeIdNew.Value = String.Empty;
            cmbTypeSpecialty.ClearSelection();
            hdnSelectRootNodeID.Value = String.Empty;
            //if (this.FindControl("caCustomAttributes").IsNotNull())
            //{
            //    SharedUserCustomAttributeForm caCustomAttributes = this.FindControl("caCustomAttributes") as SharedUserCustomAttributeForm;
            //    //    // CurrentViewContext.SaveCustomAttributeList = caCustomAttributes.GetCustomAttributeValues();
            //    //caCustomAttributes.ResetCustomAttributes();

            //    //    //Presenter.GetCustomAttributeList(null);

            //    //    //if (!CurrentViewContext.GetCustomAttributeList.IsNullOrEmpty())
            //    //    //{
            //    //    //    GenerateCustomAttributes(caCustomAttributes);
            //    //    //    pnlEditForm.Controls.Add(caCustomAttributes);
            //    //    //}
            //}

            lblInstitutionHierarchyPB.Text = string.Empty;
            //ddlAgency.ClearSelection(); [ddl]
            hdnFileRemoved.Value = string.Empty;
            hdnDepartmentProgmapNew.Value = string.Empty;
            // Reset AgencyHierarchySelection UC//
            ucAgencyHierarchyMultiple.Reset(); //UAT-2600

        }

        //UAT-2424
        private void BindRotationForAddForm(RadComboBox ddlRotationOnAddForm)
        {
            Presenter.GetClinicalRotationsForAddForm();
            ddlRotationOnAddForm.DataSource = CurrentViewContext.lstClinicalRotation;
            ddlRotationOnAddForm.DataBind();
            ddlRotationOnAddForm.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        }

        private void ManageInstAvailabilitySetting(Boolean isEditMode, Boolean isSchoolSendingInst = false)
        {
            if (!CurrentViewContext.IsInstAvailabilityDefined.IsNullOrEmpty())
            {
                if (isEditMode)
                {
                    if (CurrentViewContext.IsInstAvailabilityDefined)
                    {
                        dvInstAvailability.Style.Add("display", "block");
                        if (isSchoolSendingInst)//(CurrentViewContext.clinicalRotationDetailContract.IsSchoolSendingInstructor)
                        {
                            rdbInstAvailabileYes.Checked = true;
                            rdbInstAvailabileNo.Checked = false;
                        }
                        else
                        {
                            rdbInstAvailabileYes.Checked = false;
                            rdbInstAvailabileNo.Checked = true;
                        }
                    }
                    else
                    {
                        dvInstAvailability.Style.Add("display", "none");
                        rdbInstAvailabileYes.Checked = false;
                        rdbInstAvailabileNo.Checked = true;
                    }
                    //HandleInstPrecpReqd();
                }
                else
                {
                    if (CurrentViewContext.IsInstAvailabilityDefined)
                    {
                        dvInstAvailability.Style.Add("display", "block");
                    }
                    else
                    {
                        dvInstAvailability.Style.Add("display", "none");
                        rdbInstAvailabileYes.Checked = false;
                        rdbInstAvailabileNo.Checked = true;
                    }
                    //HandleInstPrecpReqd();
                }
                HandleInstPrecpReqd();
            }
        }

        private void HandleInstPrecpReqd()
        {
            if (!CurrentViewContext.IsInstAvailabilityDefined || (rdbInstAvailabileYes.Checked && !rdbInstAvailabileNo.Checked))//CurrentViewContext.clinicalRotationDetailContract.IsSchoolSendingInstructor)
            {
                if (!CurrentViewContext.IsInstAvailabilityDefined)
                {
                    spnInsPre.Attributes.Add("class", "reqd controlHidden");
                    rfvInsPre.Enabled = false;
                }
                else
                {
                    spnInsPre.Attributes.Add("class", "reqd");
                    rfvInsPre.Enabled = true;
                }
                ddlInstructor.Enabled = true; //UAT:4437
                //ddlInstructor.ClearCheckedItems();
            }
            else
            {
                spnInsPre.Attributes.Add("class", "reqd controlHidden");
                rfvInsPre.Enabled = false;
                if (CurrentViewContext.SelectedRotationID < AppConsts.NONE)
                {
                    ddlInstructor.ClearCheckedItems();
                }
                    //Start UAT:4437
                    //ddlInstructor.ClearCheckedItems();
                    ddlInstructor.Enabled = false;
                    //End UAT:4437
                    //if(rdbInstAvailabileNo.Checked)
                    //    ddlInstructor.ClearCheckedItems();
                //}

            }
        }

        private void HideControls()
        {
            txtRotationName.ReadOnly = true;
            cmbTypeSpecialty.Enabled = false;
            txtDepartment.ReadOnly = true;
            txtProgram.ReadOnly = true;
            txtCourse.ReadOnly = true;
            txtTerm.ReadOnly = true;
            txtUnit.ReadOnly = true;
            txtStudents.ReadOnly = true;
            txtRecommendedHrs.ReadOnly = true;
            ddlDays.Enabled = false;
            txtShift.ReadOnly = true;
            ddlInstructor.Enabled = false;
            tpStartTime.Enabled = false;
            tpEndTime.Enabled = false;
            dpStartDate.Enabled = false;
            dpEndDate.Enabled = false;
            uploadControl.Enabled = false;
            dpDeadlineDate.Enabled = false;
            chkEditPermission.Enabled = false;
            UpLoadAdditionalDocuments.Enabled = false;
            txtDaysBefore.ReadOnly = true;
            txtFrequency.ReadOnly = true;
            //hdnValidateFileUploadControl.Value = "false";
            //hdnValidateAdditionalControl.Value = "false";
            fsucCmdBarRotation.SubmitButton.Enabled = false;
            lnkRemove.Enabled = false;
            rdbInstAvailabileYes.Enabled = false;
            rdbInstAvailabileNo.Enabled = false;
        }
        #endregion

        #region [Control Events]

        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                GridEditFormInsertItem insertItem = (sender as WclComboBox).NamingContainer as GridEditFormInsertItem;
                String selectedValue = (sender as WclComboBox).SelectedValue;
                CurrentViewContext.SelectedTenantID = selectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(selectedValue);
                // WclComboBox ddlAgency = insertItem.FindControl("ddlAgency") as WclComboBox;  [ddl]
                // BindAgency(ddlAgency); [ddl]
                WclComboBox cmbDays = insertItem.FindControl("ddlDays") as WclComboBox;
                BindWeekDays(cmbDays);
                WclComboBox cmbContacts = insertItem.FindControl("ddlInstructor") as WclComboBox;
                BindContacts(cmbContacts);
                if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
                {
                    SharedUserCustomAttributeForm caCustomAttributes = (SharedUserCustomAttributeForm)Page.LoadControl("~\\ClinicalRotation\\UserControl\\SharedUserCustomAttributeForm.ascx");
                    caCustomAttributes.ID = "caCustomAttributes";
                    Int32? rotationID = null;
                    Presenter.GetCustomAttributeList(rotationID);
                    if (!CurrentViewContext.GetCustomAttributeList.IsNullOrEmpty())
                    {
                        GenerateCustomAttributes(caCustomAttributes);
                        Panel pnlEditForm = insertItem.FindControl("pnlEditForm") as Panel;
                        pnlEditForm.Controls.Add(caCustomAttributes);
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

        protected void fsucCmdBarRotation_SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (!chkCopyStudentusingClone.Checked)
                {
                    CloneRotationId = AppConsts.NONE;
                    CurrentViewContext.ViewContract.IsCloneRotationStudentCheck = false;
                }
                else
                {
                    CurrentViewContext.ViewContract.CloneRotationId = CloneRotationId;
                    CurrentViewContext.ViewContract.IsCloneRotationStudentCheck = true;
                }

                AgencyhierarchyCollection ucAgencyHierarchyAddEditRotationCollection = ucAgencyHierarchyMultiple.GetAgencyHierarchyCollection();
                String selectedAgencyHierarchyIDs = String.Join(",",ucAgencyHierarchyAddEditRotationCollection.agencyhierarchy.Select(sel => sel.AgencyNodeID.ToString()));
                if (!Presenter.IsAdminLoggedIn() && !CurrentViewContext.CloneContract.IsNullOrEmpty() && CurrentViewContext.CloneContract.RotationID > AppConsts.NONE && !Presenter.IsSelectedAgenycHierarchyAvailable(selectedAgencyHierarchyIDs))
                {
                    lblMsg.Visible = true;
                    lblMsg.ShowMessage("Rotation cannot be added because selected agency hierarchy is not available for creating rotation.", MessageType.Error);
                    ddlTenant.Focus();
                    return;
                }

                if (!hdnchkRotationFieldOptionValidator.Value.IsNullOrEmpty() && hdnchkRotationFieldOptionValidator.Value != AppConsts.TRUE)
                {
                    if (!Page.IsValid)
                        return;
                }
                if (!hdnInstNodeLabel.Value.IsNullOrEmpty())
                    lblInstitutionHierarchyPB.Text = hdnInstNodeLabel.Value.HtmlEncode();

                if (hdnInstNodeIdNew.Value == AppConsts.ZERO || hdnInstNodeIdNew.Value == String.Empty)
                {
                    lblMsg.Visible = true;
                    lblMsg.ShowMessage("Please select Institution Hierarchy.", MessageType.Error);
                    lblInstitutionHierarchyPB.Text = String.Empty;
                    return;
                }
                List<Int32> AgencyIds = new List<int>();

                if (ucAgencyHierarchyMultiple.IsNullOrEmpty())
                {
                    lblMsg.Visible = true;
                    lblMsg.ShowMessage("Please select Agency Hierarchy.", MessageType.Error);
                    return;
                }
                else
                {
                    
                    if (ucAgencyHierarchyAddEditRotationCollection.IsNotNull() && ucAgencyHierarchyAddEditRotationCollection.agencyhierarchy.IsNotNull())
                    {
                        AgencyIds.AddRange(ucAgencyHierarchyAddEditRotationCollection.agencyhierarchy.Select(d => d.AgencyID).Distinct().ToList());
                    }
                }


                if (CurrentViewContext.CommandName.ToLower() == "updatecommandname")
                {
                    if (CurrentViewContext.SelectedRotationID > AppConsts.NONE)
                    {
                        CurrentViewContext.ViewContract.RotationID = CurrentViewContext.SelectedRotationID;
                    }

                    #region UAT-2514
                    Dictionary<Boolean, DateTime> result = Presenter.IsRotationEndDateRangeNeedToManage();

                    if (result.FirstOrDefault().Key
                        && !CurrentViewContext.ViewContract.StartDate.IsNullOrEmpty()
                        && dpStartDate.SelectedDate != Convert.ToDateTime(CurrentViewContext.ViewContract.StartDate)
                        && dpStartDate.SelectedDate.Value < result.FirstOrDefault().Value)
                    {
                        lblMsg.Visible = true;
                        lblMsg.ShowMessage("Rotation Start Date can not be less than assigned package effective start date " + result.FirstOrDefault().Value.ToString("MM/dd/yyyy"), MessageType.Information);
                        return;
                    }

                    #endregion

                    if (!CurrentViewContext.clinicalRotationDetailContract.IsNullOrEmpty() && !CurrentViewContext.clinicalRotationDetailContract.StartDate.IsNullOrEmpty())
                    {
                        CurrentViewContext.RotationStartDate = CurrentViewContext.clinicalRotationDetailContract.StartDate;
                        if (CurrentViewContext.RotationStartDate.Value <= DateTime.Now)
                        {
                            CurrentViewContext.IsRotationStart = true;
                        }
                    }

                    Presenter.GetClinicalRotationMembers();
                    List<RotationMemberDetailContract> lstRotationMembers = CurrentViewContext.RotationMemberDetailList;
                    Int32 cntExistingRotationApplicants = lstRotationMembers.Where(x => x.IsInstructor == false).Count();
                    if (!txtStudents.Text.IsNullOrEmpty() && Convert.ToInt16(txtStudents.Text) < cntExistingRotationApplicants)
                    {
                        lblMsg.Visible = true;
                        lblMsg.ShowMessage("You cannot enter ' # of Students' lesser than the number of students already entered in the rotation.", MessageType.Error);
                        lblInstitutionHierarchyPB.Text = String.Empty;
                        return;
                    }
                }


                //CurrentViewContext.SelectedAgencyID = ucAgencyHierarchy.AgencyId;
                //if (Presenter.IsPreceptorRequiredForAgency())
                //{
                //    if (ddlInstructor.CheckedItems.Count == AppConsts.NONE)
                //    {
                //        lblMsg.Visible = true;
                //        lblMsg.ShowMessage("Instructor/Preceptor is required.", MessageType.Error);
                //        return;
                //    }
                //}

                CurrentViewContext.HierarchyNode = hdnDepartmentProgmapNew.Value;
                CurrentViewContext.ViewContract.TenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                //CurrentViewContext.ViewContract.AgencyID = Convert.ToInt32(ddlAgency.SelectedValue); [ddl]
                CurrentViewContext.ViewContract.AgencyIdList = String.Join(",", AgencyIds.ToArray()); //UAT-2600
                CurrentViewContext.ViewContract.RotationName = txtRotationName.Text.Trim().IsNullOrEmpty() ? String.Empty : txtRotationName.Text.Trim();
                CurrentViewContext.ViewContract.Department = txtDepartment.Text.Trim().IsNullOrEmpty() ? String.Empty : txtDepartment.Text.Trim();
                CurrentViewContext.ViewContract.Program = txtProgram.Text.Trim().IsNullOrEmpty() ? String.Empty : txtProgram.Text.Trim();
                CurrentViewContext.ViewContract.Course = txtCourse.Text.Trim().IsNullOrEmpty() ? String.Empty : txtCourse.Text.Trim();
                CurrentViewContext.ViewContract.UnitFloorLoc = txtUnit.Text.Trim().IsNullOrEmpty() ? String.Empty : txtUnit.Text.Trim();
                //UAT-1467: Addition of "Type/Specialty" field on rotation creation and rotation details
                CurrentViewContext.ViewContract.TypeSpecialty = txtTypeSpecialty.Text.Trim().IsNullOrEmpty() ? String.Empty : txtTypeSpecialty.Text.Trim();

                //UAT 1414 notification to go out prior to student's start date for clinical rotation.
                // CurrentViewContext.ViewContract.DaysBefore = Convert.ToInt32((e.Item.FindControl("txtDaysBefore") as WclTextBox).Text);
                if (!String.IsNullOrEmpty(txtDaysBefore.Text.Trim()))
                {
                    CurrentViewContext.ViewContract.DaysBefore = Convert.ToInt32(txtDaysBefore.Text.Trim());
                }
                else
                {
                    CurrentViewContext.ViewContract.DaysBefore = null;
                }
                // CurrentViewContext.ViewContract.DaysBefore = (e.Item.FindControl("txtDaysBefore") as WclNumericTextBox).Text.IsNullOrEmpty() ? (int?)null : Convert.ToInt32((e.Item.FindControl("txtDaysBefore") as WclNumericTextBox).Text.Trim());
                CurrentViewContext.ViewContract.Frequency = txtFrequency.Text.Trim().IsNullOrEmpty() ? String.Empty : txtFrequency.Text.Trim();

                if (!String.IsNullOrEmpty(txtRecommendedHrs.Text.Trim()))
                {
                    CurrentViewContext.ViewContract.RecommendedHours = float.Parse(txtRecommendedHrs.Text.Trim());
                }
                else
                {
                    CurrentViewContext.ViewContract.RecommendedHours = null;
                }
                //UAT-1769 Addition of "# of Students" field on rotation creation and rotation details for all except students
                if (!String.IsNullOrEmpty(txtStudents.Text.Trim()))
                {
                    CurrentViewContext.ViewContract.Students = Int32.Parse(txtStudents.Text.Trim());
                }
                else
                {
                    CurrentViewContext.ViewContract.Students = null;
                }
                CurrentViewContext.ViewContract.Shift = txtShift.Text.Trim();
                //UAT 1355 Addition of Term field to the Create rotation, rotation details screens
                CurrentViewContext.ViewContract.Term = txtTerm.Text.Trim();
                CurrentViewContext.ViewContract.StartDate = dpStartDate.SelectedDate;
                CurrentViewContext.ViewContract.EndDate = dpEndDate.SelectedDate;
                CurrentViewContext.ViewContract.StartTime = tpStartTime.SelectedTime;
                CurrentViewContext.ViewContract.EndTime = tpEndTime.SelectedTime;
                //UAT-2289
                CurrentViewContext.ViewContract.DeadlineDate = dpDeadlineDate.SelectedDate;

                List<Int32> selectedDays = new List<Int32>();
                foreach (RadComboBoxItem slctdItem in ddlDays.CheckedItems)
                {
                    selectedDays.Add(Convert.ToInt32(slctdItem.Value));
                }

                List<Int32> selectedContacts = new List<Int32>();
                foreach (RadComboBoxItem slctdContact in ddlInstructor.CheckedItems)
                {
                    selectedContacts.Add(Convert.ToInt32(slctdContact.Value));
                }

                CurrentViewContext.ViewContract.DaysIdList = String.Join(",", selectedDays.ToArray());
                CurrentViewContext.SelectedClientContacts = selectedContacts;
                CurrentViewContext.ViewContract.ContactIdList = String.Join(",", selectedContacts.ToArray());
                CurrentViewContext.ViewContract.HierarchyNodeIDList = CurrentViewContext.HierarchyNode;
                CurrentViewContext.ViewContract.IsAllowNotification = chkAllowNotification.Checked;   //UAT-2905

                Boolean isFileuploaeded = true;
                if (!hdnFileRemoved.Value.IsNullOrEmpty() && hdnFileRemoved.Value == "True")
                {
                    CurrentViewContext.ViewContract.IfSyllabusFileRemoved = true;
                }



                #region 4062
                CurrentViewContext.ViewContract.ClinicalRotationDocumentUpdatedIds = hdnTempAdditionalDocumentIds.Value;
                if (hdnValidateAdditionalControl.Value == "true" || hdnValidateFileUploadControl.Value == "true")
                {
                    bool IsValidationSuccess = true;

                    if (uploadControl.UploadedFiles.Count < 1 && lblUploadFormName.Text.IsNullOrWhiteSpace() && hdnValidateFileUploadControl.Value == "true")
                    {
                        lblSyllabusDocumentError.Style["display"] = "block";
                        String agencyHierarchyIDs = String.Empty;
                        if (ucAgencyHierarchyMultiple.IsNullOrEmpty())
                        {
                            AgencyhierarchyCollection agencyHierarchyCollection = ucAgencyHierarchyMultiple.GetAgencyHierarchyCollection();
                            agencyHierarchyIDs = String.Join(",", agencyHierarchyCollection.agencyhierarchy.Select(x => x.AgencyNodeID).ToList());
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "GetRotationRequiredFieldOptions('" + Convert.ToInt32(ddlTenant.SelectedValue) + "','" + agencyHierarchyIDs + "');", true);
                        IsValidationSuccess = false;
                    }

                    if (hdnValidateAdditionalControl.Value == "true" && UpLoadAdditionalDocuments.UploadedFiles.Count < 1 && CurrentViewContext.ViewContract.ClinicalRotationDocumentUpdatedIds.IsNullOrEmpty())
                    {
                        lblAdditionalDocumentsRequired.Style["display"] = "block";
                        String agencyHierarchyIDs = String.Empty;
                        if (ucAgencyHierarchyMultiple.IsNullOrEmpty())
                        {
                            AgencyhierarchyCollection agencyHierarchyCollection = ucAgencyHierarchyMultiple.GetAgencyHierarchyCollection();
                            agencyHierarchyIDs = String.Join(",", agencyHierarchyCollection.agencyhierarchy.Select(x => x.AgencyNodeID).ToList());
                        }
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "GetRotationRequiredFieldOptions('" + Convert.ToInt32(ddlTenant.SelectedValue) + "','" + agencyHierarchyIDs + "');", true);
                        IsValidationSuccess = false;
                    }
                    if (!IsValidationSuccess)
                    {
                        return;
                    }
                }
                if (UpLoadAdditionalDocuments.UploadedFiles.Count > AppConsts.NONE)
                {

                    string RotationAdditionalDocument = "RotationAdditionalDocument_";
                    Dictionary<string, string> GetRecordsOfFiles = UploadSyllabusDocuments(UpLoadAdditionalDocuments, RotationAdditionalDocument);
                    int i = 0;
                    CurrentViewContext.ViewContract.listOfMultipleDocument = new List<MultipleAdditionalDocumentsContract>();

                    foreach (var item in GetRecordsOfFiles)
                    {
                        CurrentViewContext.ViewContract.listOfMultipleDocument.Add(new MultipleAdditionalDocumentsContract
                        {
                            AdditionalDocumentFilePath = item.Value,
                            AdditionalDocumentFileName = item.Key,
                            AdditionalDocumentFileSize = UpLoadAdditionalDocuments.UploadedFiles[i].ContentLength

                        });
                        i++;
                    }

                }

                #endregion
                if (uploadControl.UploadedFiles.Count > AppConsts.NONE)
                {
                    string NameOfSyll = "RotationSyllabus_";
                    String savedFilePath = UploadSyllabusDocuments(uploadControl, NameOfSyll).Select(x => x.Value).FirstOrDefault();
                    if (savedFilePath.IsNullOrEmpty())
                        isFileuploaeded = false;
                    CurrentViewContext.ViewContract.SyllabusFilePath = savedFilePath;
                    CurrentViewContext.ViewContract.SyllabusFileName = uploadControl.UploadedFiles[0].FileName;
                    CurrentViewContext.ViewContract.SyllabusFileSize = uploadControl.UploadedFiles[0].ContentLength;

                }



                if (this.FindControl("caCustomAttributes").IsNotNull())
                {
                    SharedUserCustomAttributeForm caCustomAttributes = this.FindControl("caCustomAttributes") as SharedUserCustomAttributeForm;
                    CurrentViewContext.SaveCustomAttributeList = caCustomAttributes.GetCustomAttributeValues();
                }
                else
                    CurrentViewContext.SaveCustomAttributeList = new List<CustomAttribteContract>();

                if (isFileuploaeded)
                {
                    //UAT-2424 Check if it is normal rotation save click or cloning a rotation.
                    if (CurrentViewContext.SelectedRotationIDForCloning>AppConsts.NONE && !ddlRotation.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(ddlRotation.SelectedValue) != AppConsts.NONE)
                    {
                        if (uploadControl.UploadedFiles.Count == AppConsts.NONE && !lblUploadFormName.Text.IsNullOrEmpty()) //In Cloning mode fileupload control count is zero and file doesn't get updated or uploaded.File name and path is saved in label.
                        {
                            CurrentViewContext.ViewContract.SyllabusFilePath = lblUploadFormPath.Text;
                            CurrentViewContext.ViewContract.SyllabusFileName = lblUploadFormName.Text;
                        }
                        
                        //Is any Field of rotation is updated or not ,if not then raise a info msg.
                      
                        IsRotationFieldsUpdated(CloneContract, CurrentViewContext.ViewContract);

                        if (IsFieldsUpdated)
                        {
                            CurrentViewContext.ViewContract.IsCloningRotation = true;
                            CurrentViewContext.ViewContract.RequirementPackageID = Presenter.GetApplicantPackage();
                            CurrentViewContext.ViewContract.InstructorPreceptorPkgID = Presenter.GetInstructorPackage();
                            CurrentViewContext.ViewContract.IsAgencyUpdated = IsAgencyChanged;
                        }
                        else
                        {
                            lblMsg.Visible = true;
                            lblMsg.ShowMessage("Please update at least one field to complete rotation clone.", MessageType.Error);
                            String agencyHierarchyIDs = String.Empty;
                            if (ucAgencyHierarchyMultiple.IsNullOrEmpty())
                            {
                                AgencyhierarchyCollection agencyHierarchyCollection = ucAgencyHierarchyMultiple.GetAgencyHierarchyCollection();
                                agencyHierarchyIDs = String.Join(",", agencyHierarchyCollection.agencyhierarchy.Select(x => x.AgencyNodeID).ToList());
                            }
                            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "GetRotationRequiredFieldOptions('" + Convert.ToInt32(ddlTenant.SelectedValue) + "','" + agencyHierarchyIDs + "');", true);
                            return;
                        }
                    }

                    //UAT 3041
                    foreach (ListItem item in chkEditPermission.Items)
                    {
                        if (item.Value.ToString() == "CA")
                        {
                            CurrentViewContext.ViewContract.IsEditableByClientAdmin = item.Selected;
                        }
                        else if (item.Value.ToString() == "AGU")
                        {
                            CurrentViewContext.ViewContract.IsEditableByAgencyUser = item.Selected;
                        }
                    }
                    if (!Presenter.IsAdminLoggedIn())
                    {
                        CurrentViewContext.ViewContract.IsEditableByClientAdmin = true;

                    }
                    //UAT 3961
                    if (Presenter.IsAgencyHierarchyRootNodeSettingExist(ucAgencyHierarchyMultiple.SelectedRootNodeId))
                    {
                        if (!String.IsNullOrEmpty(cmbTypeSpecialty.SelectedValue))
                            CurrentViewContext.ViewContract.AgnecyHierarchyRootNodeSettingMappingID = Convert.ToInt32(cmbTypeSpecialty.SelectedValue);
                        else
                            CurrentViewContext.ViewContract.AgnecyHierarchyRootNodeSettingMappingID = null;
                    }
                    //UAT-4150
                    Presenter.InstAvailabilityHierarchyRootNodeSetting(ucAgencyHierarchyMultiple.SelectedRootNodeId);
                    if (CurrentViewContext.IsInstAvailabilityDefined)
                    {
                        CurrentViewContext.ViewContract.IsSchoolSendingInstructor = rdbInstAvailabileYes.Checked ? true : false;
                    }
                    //End

                    if (CurrentViewContext.SelectedRotationID > AppConsts.NONE || CurrentViewContext.SelectedRotationID == AppConsts.MINUS_ONE)
                    {
                        string rotationID = Convert.ToString(CurrentViewContext.ViewContract.RotationID);
                        string selectedOrgUserIDs = string.Empty;
                        string selectedClientContactIDs = CurrentViewContext.ViewContract.ContactIdList;
                        List<ClinicalRotationMembersContract> lstClinicalRotationDetailContract = Presenter.IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(rotationID, CurrentViewContext.ViewContract.TenantID, selectedOrgUserIDs, selectedClientContactIDs);
                        if (lstClinicalRotationDetailContract.IsNullOrEmpty())
                        {
                            if (Presenter.SaveUpdateClinicalRotationData())
                            {
                                hdnIsEditableByClientAdmin.Value = CurrentViewContext.ViewContract.IsEditableByClientAdmin.ToString();
                                hdnIsEditableByAgencyUser.Value = CurrentViewContext.ViewContract.IsEditableByAgencyUser.ToString();
                                hdnTenantId.Value = CurrentViewContext.SelectedTenantID.ToString();
                                if (!CurrentViewContext.ViewContract.AgencyIdList.IsNullOrEmpty())
                                    hdnAgencyId.Value = Convert.ToString(CurrentViewContext.ViewContract.AgencyIdList.Split(',').FirstOrDefault());

                                hdnClinicalRotationId.Value = CurrentViewContext.ViewContract.RotationID.ToString();
                                hdnIsApplicantPkgNotAssignedThroughCloning.Value = CurrentViewContext.IsApplicantPkgNotAssignedThroughCloning.ToString();
                                hdnIsInstructorPkgNotAssignedThroughCloning.Value = CurrentViewContext.IsInstructorPkgNotAssignedThroughCloning.ToString();

                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "CloseAddEditPopup();", true);
                            }
                        }
                        else // UAT-4147
                        {
                            HtmlGenericControl div = new HtmlGenericControl("div");
                            div.Style.Add("float", "left");
                            HtmlGenericControl ul = new HtmlGenericControl("ul");
                            if (lstClinicalRotationDetailContract.Any(x => x.IsApplicant == true))
                            {
                                lstClinicalRotationDetailContract.ForEach(x =>
                                {
                                    HtmlGenericControl li = new HtmlGenericControl("li");
                                    if (x.IsApplicant)
                                    {
                                        li.InnerText = "Rotation with Complio ID " + x.ComplioID + " already has " + x.UserName + " as Applicant.";
                                        li.Style["list-style"] = "disc";
                                        ul.Controls.Add(li);
                                    }
                                });
                                ul.Style["padding-left"] = "30px";
                                div.Controls.Add(ul);
                                pnlExistingRotationMembers.Controls.Add(div);

                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowExistingRotationMembers();", true);
                                return;
                            }
                            else
                            {
                                if (Presenter.SaveUpdateClinicalRotationData())
                                {
                                    hdnIsEditableByClientAdmin.Value = CurrentViewContext.ViewContract.IsEditableByClientAdmin.ToString();
                                    hdnIsEditableByAgencyUser.Value = CurrentViewContext.ViewContract.IsEditableByAgencyUser.ToString();
                                    hdnTenantId.Value = CurrentViewContext.SelectedTenantID.ToString();
                                    if (!CurrentViewContext.ViewContract.AgencyIdList.IsNullOrEmpty())
                                        hdnAgencyId.Value = Convert.ToString(CurrentViewContext.ViewContract.AgencyIdList.Split(',').FirstOrDefault());

                                    hdnClinicalRotationId.Value = CurrentViewContext.ViewContract.RotationID.ToString();
                                    hdnIsApplicantPkgNotAssignedThroughCloning.Value = CurrentViewContext.IsApplicantPkgNotAssignedThroughCloning.ToString();
                                    hdnIsInstructorPkgNotAssignedThroughCloning.Value = CurrentViewContext.IsInstructorPkgNotAssignedThroughCloning.ToString();

                                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "CloseAddEditPopup();", true);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Presenter.SaveUpdateClinicalRotation())
                        {
                            //else
                            //{
                            ResetControls();
                            lblMsg.Visible = true;
                            lblMsg.ShowMessage("Clinical rotation saved successfully.", MessageType.SuccessMessage);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "refreshParentGrid();", true);
                            // }
                        }
                    }
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.ShowMessage("some error has occured due to which rotation can not be saved.", MessageType.Error);
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

        #region UAT-2424

        protected void btnClone_Click(object sender, EventArgs e)
        {
            CurrentViewContext.SelectedRotationIDForCloning = Convert.ToInt32(ddlRotation.SelectedValue);
            //UAT:4395
            CloneRotationId = Convert.ToInt32(ddlRotation.SelectedValue);
            CurrentViewContext.TenantId = Convert.ToInt32(ddlTenant.SelectedValue);

            CloneContract = Presenter.GetClinicalRotationDetailsByID(); //Get details of Rotation that is to be cloned

            //Fill controls with Rotation Details
            if (CloneContract != null)
            {
                txtRotationName.Text = CloneContract.RotationName;
                txtComplioId.Text = CloneContract.ComplioID;
                txtCourse.Text = CloneContract.Course;
                txtDepartment.Text = CloneContract.Department;
                txtFrequency.Text = CloneContract.Frequency;
                txtProgram.Text = CloneContract.Program;
                txtRecommendedHrs.Text = CloneContract.RecommendedHours.ToString();
                txtShift.Text = CloneContract.Shift;
                txtTypeSpecialty.Text = CloneContract.TypeSpecialty;
                txtUnit.Text = CloneContract.UnitFloorLoc;
                txtTerm.Text = CloneContract.Term;
                txtStudents.Text = CloneContract.Students.ToString();
                dvComplioId.Visible = true;
                //ddlAgency.SelectedValue = CloneContract.AgencyID.ToString(); [ddl]
                //ucAgencyHierarchy.AgencyId = CloneContract.AgencyID; //UAT-2600
                //ucAgencyHierarchy.NodeId = CloneContract.AgencyHierarchyID; 
                //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "GetRotationRequiredFieldOptions('" + Convert.ToInt32(ddlTenant.SelectedValue) + "','" + CloneContract.AgencyHierarchyIDs + "');", true);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "GetRotationRequiredFieldOptions('" + Convert.ToInt32(ddlTenant.SelectedValue) + "','" + CloneContract.AgencyIDs + "');", true);
                //ucAgencyHierarchy.SelectedRootNodeId = CloneContract.RootNodeID;
                //ucAgencyHierarchy.Label = string.Empty;
                //ucAgencyHierarchy.AgencyName = string.Empty;

                String formattedAgencyIds = String.Empty;
                List<Int32> agencyIds = new List<Int32>(); //UAT-3241
                if (!CloneContract.AgencyIDs.IsNullOrEmpty())
                {
                    agencyIds = CloneContract.AgencyIDs.Split(',').Select(t => Int32.Parse(t) * -1).ToList();
                    formattedAgencyIds = String.Join(",", agencyIds);
                }

                ucAgencyHierarchyMultiple.SelectedAgecnyIds = formattedAgencyIds;
                ucAgencyHierarchyMultiple.SelectedNodeIds = CloneContract.AgencyHierarchyIDs;
                ucAgencyHierarchyMultiple.SelectedRootNodeId = CloneContract.RootNodeID;
                Session["NodeSelected"] = CloneContract.AgencyHierarchyIDs;
                Session["AgencySelected"] = formattedAgencyIds;
                //UAT 3961
                hdnSelectRootNodeID.Value = String.Empty;
                BindTypeSpecialtyOptions(cmbTypeSpecialty);
                if (CloneContract.AgnecyHierarchyRootNodeSettingMappingID > AppConsts.NONE)
                {
                    foreach (RadComboBoxItem item in cmbTypeSpecialty.Items)
                    {
                        if (item.Value == Convert.ToString(CloneContract.AgnecyHierarchyRootNodeSettingMappingID))
                        {
                            item.Selected = true;
                        }
                    }
                }
                else
                {
                    cmbTypeSpecialty.ClearSelection();
                }

                //UAT-4150
                Presenter.InstAvailabilityHierarchyRootNodeSetting(ucAgencyHierarchyMultiple.SelectedRootNodeId);
                ManageInstAvailabilitySetting(true, CloneContract.IsSchoolSendingInstructor);
                //End

                //UAT-3241
                List<String> selectedAgencyNames = Presenter.GetAgencyNamesByIds(CloneContract.AgencyIDs.Split(',').ConvertIntoIntList());
                String agencyNames = string.Join(", ", selectedAgencyNames);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "GetRotationRequiredFieldOptions('" + Convert.ToInt32(ddlTenant.SelectedValue) + "','" + CloneContract.AgencyIDs + "','" + agencyNames + "');", true);

                if (!CloneContract.HierarchyNodeIDList.IsNullOrEmpty())
                    ucAgencyHierarchyMultiple.SelectedInstitutionNodeIds = Convert.ToString(CloneContract.HierarchyNodeIDList);

                dpStartDate.SelectedDate = CloneContract.StartDate;
                dpEndDate.SelectedDate = CloneContract.EndDate;
                tpStartTime.SelectedTime = CloneContract.StartTime;
                tpEndTime.SelectedTime = CloneContract.EndTime;
                dpDeadlineDate.SelectedDate = CloneContract.DeadlineDate;
                txtDaysBefore.Text = CloneContract.DaysBefore.ToString();
                txtFrequency.Text = CloneContract.Frequency;
                //UAT-2905
                chkAllowNotification.Checked = CloneContract.IsAllowNotification;
                if (!CloneContract.ContactIdList.IsNullOrEmpty())
                {
                    String[] selectedContactIds = CloneContract.ContactIdList.Split(',');
                    foreach (RadComboBoxItem item in ddlInstructor.Items)
                    {
                        item.Checked = selectedContactIds.Contains(item.Value);
                    }
                }
                else
                {
                    ddlInstructor.ClearCheckedItems();
                }
                if (!CloneContract.DaysIdList.IsNullOrEmpty())
                {
                    String[] selectedDayIds = CloneContract.DaysIdList.Split(',');
                    foreach (RadComboBoxItem item in ddlDays.Items)
                    {
                        item.Checked = selectedDayIds.Contains(item.Value);
                    }
                }
                else
                {
                    ddlDays.ClearCheckedItems();
                }

                #region UAT: 4062
                if (CloneContract.listOfMultipleDocument != null && CloneContract.listOfMultipleDocument.Count() > AppConsts.NONE)
                {
                    CurrentViewContext.MultipleAdditionalDocumentsContract = CloneContract.listOfMultipleDocument;
                    BindAdditionalDocuments();
                }
                #endregion

                if (!CloneContract.SyllabusFileName.IsNullOrEmpty())
                {
                    lblUploadFormName.Text = CloneContract.SyllabusFileName;
                    lblUploadFormPath.Text = CloneContract.SyllabusFilePath;
                    lblUploadFormName.Visible = true;
                    lnkRemove.Visible = true;
                    uploadControl.Visible = false;
                }
                else
                {
                    lblUploadFormName.Text = "";
                    lblUploadFormPath.Text = "";
                    uploadControl.Visible = true;
                    lblUploadFormName.Visible = false;
                    lnkRemove.Visible = false;

                }
                hdnDepartmentProgmapNew.Value = CloneContract.HierarchyNodeIDList;
                hdnInstNodeIdNew.Value = CloneContract.HierarchyNodeIDList;
                lblInstitutionHierarchyPB.Text = CloneContract.HierarchyNodes;

                Presenter.GetCustomAttributeList(CurrentViewContext.SelectedRotationIDForCloning);
                if (!CurrentViewContext.GetCustomAttributeList.IsNullOrEmpty())
                {
                    SharedUserCustomAttributeForm caCustomAttributesExisting = pnlEditForm.FindControl("caCustomAttributes") as SharedUserCustomAttributeForm;
                    pnlEditForm.Controls.Remove(caCustomAttributesExisting); //First remove the control of custome attributes that is already added in pageload event
                    SharedUserCustomAttributeForm caCustomAttributes = (SharedUserCustomAttributeForm)Page.LoadControl("~\\ClinicalRotation\\UserControl\\SharedUserCustomAttributeForm.ascx");
                    caCustomAttributes.ID = "caCustomAttributes";
                    GenerateCustomAttributes(caCustomAttributes);
                    pnlEditForm.Controls.Add(caCustomAttributes); //Add custom attribute control again with the values associated with rotation id (that is to be cloned)

                   
                }
                SetInstPreceptorValidation(); //UAT-2554

                foreach (ListItem item in chkEditPermission.Items)
                {
                    if (item.Value.ToString() == "CA")
                    {
                        item.Selected = CloneContract.IsEditableByClientAdmin;
                    }
                    else if (item.Value.ToString() == "AGU")
                    {
                        item.Selected = CloneContract.IsEditableByAgencyUser;
                    }
                }
            }
        }

        void IsRotationFieldsUpdated(ClinicalRotationDetailContract OldRotation, ClinicalRotationDetailContract NewRotation)
        {
            if (CurrentViewContext.SelectedRotationID > AppConsts.NONE || CurrentViewContext.SelectedRotationID == AppConsts.MINUS_ONE || !NewRotation.AgencyIdList.IsNullOrEmpty())
            {
                List<Int32> oldAgencyIds = new List<int>();
                List<Int32> newAgencyIds = NewRotation.AgencyIdList.Split(',').Select(Int32.Parse).ToList();
                if (OldRotation.AgencyIDs != null)
                {
                     oldAgencyIds = OldRotation.AgencyIDs.Split(',').Select(Int32.Parse).ToList();
                }
                if (newAgencyIds.Except(oldAgencyIds).Count() > 0
                    || oldAgencyIds.Except(newAgencyIds).Count() > 0)
                {
                    IsAgencyChanged = true;
                    IsFieldsUpdated = true;
                }
            }
            else
            {
                if (NewRotation.AgencyID != OldRotation.AgencyID)
                {
                    IsAgencyChanged = true;
                    IsFieldsUpdated = true;
                }
            }
            if (NewRotation.HierarchyNodeIDList != OldRotation.HierarchyNodeIDList)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.RotationName.Trim() != OldRotation.RotationName.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.TypeSpecialty.Trim() != OldRotation.TypeSpecialty.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.Department.Trim() != OldRotation.Department.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.Program.Trim() != OldRotation.Program.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.Course.Trim() != OldRotation.Course.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.Term.Trim() != OldRotation.Term.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.UnitFloorLoc.Trim() != OldRotation.UnitFloorLoc.Trim())
            {
                IsFieldsUpdated = true;
            }

            if (NewRotation.Students != OldRotation.Students)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.RecommendedHours != OldRotation.RecommendedHours)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.DaysIdList.Trim() != OldRotation.DaysIdList.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.Shift.Trim() != OldRotation.Shift.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.ContactIdList.Trim() != OldRotation.ContactIdList.Trim())
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.StartTime != OldRotation.StartTime)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.EndTime != OldRotation.EndTime)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.StartDate != OldRotation.StartDate)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.EndDate != OldRotation.EndDate)
            {
                IsFieldsUpdated = true;
            }

            NewRotation.SyllabusFileName = NewRotation.SyllabusFileName.IsNullOrEmpty() ? String.Empty : NewRotation.SyllabusFileName;
            NewRotation.SyllabusFilePath = NewRotation.SyllabusFilePath.IsNullOrEmpty() ? String.Empty : NewRotation.SyllabusFilePath;
            if ((NewRotation.SyllabusFileName != OldRotation.SyllabusFileName) || (NewRotation.SyllabusFilePath != OldRotation.SyllabusFilePath))
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.DeadlineDate != OldRotation.DeadlineDate)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.DaysBefore != OldRotation.DaysBefore)
            {
                IsFieldsUpdated = true;
            }
            if (NewRotation.Frequency != OldRotation.Frequency)
            {
                IsFieldsUpdated = true;
            }
            //UAT-2905
            if (NewRotation.IsAllowNotification != OldRotation.IsAllowNotification)
            {
                IsFieldsUpdated = true;
            }
        }


        #endregion

        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            try
            {
                lblUploadFormName.Visible = false;
                lblUploadFormName.Text = String.Empty;
                uploadControl.Visible = true;
                lnkRemove.Visible = false;
                hdnFileRemoved.Value = true.ToString();
                String agencyHierarchyIDs = String.Empty;
                if (ucAgencyHierarchyMultiple.IsNullOrEmpty())
                {
                    AgencyhierarchyCollection agencyHierarchyCollection = ucAgencyHierarchyMultiple.GetAgencyHierarchyCollection();
                    agencyHierarchyIDs = String.Join(",", agencyHierarchyCollection.agencyhierarchy.Select(x => x.AgencyNodeID).ToList());
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "GetRotationRequiredFieldOptions('" + Convert.ToInt32(ddlTenant.SelectedValue) + "','" + agencyHierarchyIDs + "');", true);
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

        #region UAT-2554

        protected void ddlAgency_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            String selectedValue = (sender as WclComboBox).SelectedValue;
            CurrentViewContext.SelectedAgencyID = Convert.ToInt32(selectedValue);
            //if (Presenter.IsPreceptorRequiredForAgency())
            //{
            //    cstValidator.Enabled = true;
            //    spnInsPre.Visible = true;
            //}
            //else
            //{
            //    cstValidator.Enabled = false;
            //    spnInsPre.Visible = false;
            //}
        }

        #endregion

        #region UAT-4150
        protected void rdbInstAvailabile_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                HandleInstPrecpReqd();
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


       
    }
}