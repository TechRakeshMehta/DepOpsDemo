using CoreWeb.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;
using System.Web.Configuration;
using System.IO;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using System.Web.UI.HtmlControls;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class ManageRotationAssignmentPopup : BaseWebPage, IManageRotationAssignmentPopupView
    {

        #region Private Variables

        private ManageRotationAssignmentPopupPresenter _presenter = new ManageRotationAssignmentPopupPresenter();

        #endregion

        #region Properties
        public ManageRotationAssignmentPopupPresenter Presenter
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

        public IManageRotationAssignmentPopupView CurrentViewContext
        {
            get
            {
                return this;
            }
        }


        Int32 IManageRotationAssignmentPopupView.CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        List<InstructorAvailabilityContract> IManageRotationAssignmentPopupView.lstInstructorAvailabilityContracts
        {
            get
            {
                if (!ViewState["lstInstructorAvailabilityContracts"].IsNull())
                {
                    return (List<InstructorAvailabilityContract>)ViewState["lstInstructorAvailabilityContracts"];
                }
                return new List<InstructorAvailabilityContract>();
            }
            set
            {
                ViewState["lstInstructorAvailabilityContracts"] = value;
            }
        }

        String IManageRotationAssignmentPopupView.ComplioIDs {
            get
            {
                if (!ViewState["ComplioIDs"].IsNull())
                {
                    return (String)ViewState["ComplioIDs"];
                }
                return String.Empty;
            }
            set
            {
                ViewState["ComplioIDs"] = value;
            }

        }
        Int32 IManageRotationAssignmentPopupView.TenantId
        {
            get
            {
                if (!ViewState["TenantId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["TenantId"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        Int32 IManageRotationAssignmentPopupView.AgencyID
        {
            get
            {
                if (!ViewState["AgencyID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["AgencyID"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["AgencyID"] = value;
            }
        }

        String IManageRotationAssignmentPopupView.RotationIDs
        {
            get
            {
                if (!ViewState["RotationIDs"].IsNull())
                {
                    return Convert.ToString(ViewState["RotationIDs"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["RotationIDs"] = value;
            }
        }
        String IManageRotationAssignmentPopupView.RotationAssignmentTypeCode
        {
            get
            {
                if (!ViewState["RotationAssignmentTypeCode"].IsNull())
                {
                    return Convert.ToString(ViewState["RotationAssignmentTypeCode"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["RotationAssignmentTypeCode"] = value;
            }
        }        
        List<ClientContactContract> IManageRotationAssignmentPopupView.ClientContactList
        {
            set
            {
                ddlInstructor.DataSource = value;
                ddlInstructor.DataBind();
            }
        }

        List<RequirementPackageContract> IManageRotationAssignmentPopupView.lstTenantRequirementPackage
        {
            get;
            set;
        }

        List<RequirementPackageContract> IManageRotationAssignmentPopupView.lstSharedRequirementPackage
        {
            get;
            set;
        }

        List<RequirementPackageContract> IManageRotationAssignmentPopupView.lstCombinedRequirementPackage
        {
            set
            {
                cmbPackage.DataSource = value;
                cmbPackage.DataBind();
            }
        }

        List<RequirementPackageContract> IManageRotationAssignmentPopupView.lstSharedInstructorRequirementPackages
        {
            get;
            set;
        }


        List<RequirementPackageContract> IManageRotationAssignmentPopupView.lstCombinedInstructorRequirementPackages
        {
            set
            {
                cmbInstPackage.DataSource = value;
                cmbInstPackage.DataBind();
            }
        }

        List<RequirementPackageContract> IManageRotationAssignmentPopupView.lstInstructorRequirementPackage
        {
            get;
            set;
        }

        Int32 IManageRotationAssignmentPopupView.SelectedInstructionPackageID
        {
            get
            {
                if (!cmbInstPackage.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(cmbInstPackage.SelectedValue);
                }
                return AppConsts.NONE;
            }

        }
        Int32 IManageRotationAssignmentPopupView.SelectedRequirementPackageID
        {
            get
            {
                if (!cmbPackage.SelectedValue.IsNullOrEmpty())
                {
                    return Convert.ToInt32(cmbPackage.SelectedValue);
                }
                return AppConsts.NONE;
            }
        }

        //// <summary>
        ///// Gets the view contract.
        ///// </summary>
        ///// <remarks></remarks>
        ClinicalRotationDetailContract IManageRotationAssignmentPopupView.RotationDataContarct
        {
            get;
            set;
        }

        List<Int32> IManageRotationAssignmentPopupView.UnMappedRotationIDList
        {
            get;
            set;
        }
        List<String> IManageRotationAssignmentPopupView.UnMappedRotationNameList
        {
            get;
            set;
        }
        #endregion

        String IManageRotationAssignmentPopupView.RotationAgencyIDs
        {
            get
            {
                if (!ViewState["RotationAgencyIDs"].IsNull())
                {
                    return Convert.ToString(ViewState["RotationAgencyIDs"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["RotationAgencyIDs"] = value;
            }
        }

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    CaptureQuerystringParameters();
                    BindDropDownControls();
                    Presenter.CheckInstAvailabilityByRotationIds(CurrentViewContext.RotationIDs);
                    if (!CurrentViewContext.ComplioIDs.IsNullOrEmpty())
                    {
                        ShowInfoMessage("Preceptor cannot be assigned to rotation(s) with following Complio ID(s) " + CurrentViewContext.ComplioIDs + " as agency setting 'Will the school be sending an instructor for this rotation ? ' is set to No.") ;
                    }
                }
                SetControlsVisibility();
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
        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // sender.na
                GridEditFormItem editForm = (sender as LinkButton).NamingContainer as GridEditFormItem;
                if (editForm.IsNotNull())
                {
                    WclAsyncUpload fileUpload = editForm.FindControl("uploadControl") as WclAsyncUpload;
                    Label lblUploadFormName = editForm.FindControl("lblUploadFormName") as Label;
                    LinkButton lnkRemove = editForm.FindControl("lnkRemove") as LinkButton;
                    lblUploadFormName.Visible = false;
                    fileUpload.Visible = true;
                    lnkRemove.Visible = false;
                }
                //hdnFileRemoved.Value = true.ToString();
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

        protected void cmdSaveAssignments_SaveClick(object sender, EventArgs e)
        {
            try
            {
                string rotationID = Convert.ToString(CurrentViewContext.RotationIDs);
                List<Int32> selectedContacts = new List<Int32>();
                foreach (RadComboBoxItem slctdContact in ddlInstructor.CheckedItems)
                {
                    selectedContacts.Add(Convert.ToInt32(slctdContact.Value));
                }

                string selectedOrgUserIDs = string.Empty;
                string selectedClientContactIDs = String.Join(",", selectedContacts.ToArray());
                List<ClinicalRotationMembersContract> lstClinicalRotationDetailContract = Presenter.IsOrgUserAlreadyExistsAsInstructorOrApplicantInClinicalRotation(rotationID, CurrentViewContext.TenantId, selectedOrgUserIDs, selectedClientContactIDs);
                if (lstClinicalRotationDetailContract.IsNullOrEmpty())
                {
                    SaveRotationData();
                }
                else // UAT-4147
                {
                    if (lstClinicalRotationDetailContract.Any(x => x.IsApplicant == true))
                    {
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Style.Add("float", "left");
                        HtmlGenericControl ul = new HtmlGenericControl("ul");

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
                    else {
                        SaveRotationData();
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

        #region Methods

        public String UploadSyllabusDocuments(WclAsyncUpload fileUpload)
        {

            String fileName = String.Empty;
            String savedFilePath = String.Empty;

            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];

            if (tempFilePath.IsNullOrEmpty())
            {
                base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                return String.Empty;
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "Tenant(" + CurrentViewContext.TenantId.ToString() + @")\";

            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);


            var item = fileUpload.UploadedFiles[0];
            fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
            //Save file on temp location
            String newTempFilePath = Path.Combine(tempFilePath, fileName);
            item.SaveAs(newTempFilePath);

            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
            String destFileName = "RotationSyllabus_" + CurrentViewContext.TenantId.ToString() + "_" + date + Path.GetExtension(newTempFilePath);
            String desFilePath = "Tenant(" + CurrentViewContext.TenantId.ToString() + @")\" + destFileName;

            savedFilePath = CommonFileManager.SaveDocument(newTempFilePath, desFilePath, FileType.SystemDocumentLocation.GetStringValue());
            return savedFilePath;
        }

        private void BindDropDownControls()
        {
            if (String.Compare(CurrentViewContext.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_PRECEPTOR.GetStringValue(), true) == 0)
            {
                Presenter.GetClientContacts();
            }
            else if (String.Compare(CurrentViewContext.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_PRECEPTOR_PACKAGES.GetStringValue(), true) == 0)
            {
                Presenter.GetInstructorRequirementPackages();
            }
            else if (String.Compare(CurrentViewContext.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_STUDENT_PACKAGES.GetStringValue(), true) == 0)
            {
                Presenter.GetRequirementPackages();
            }
        }

        private void SetControlsVisibility()
        {
            if (String.Compare(CurrentViewContext.RotationAssignmentTypeCode, RotationAssignmentType.UPLOAD_SYLLABUS.GetStringValue(), true) == 0)
            {
                divUploadSyllabus.Visible = true;
                cmdSaveAssignments.SaveButton.ValidationGroup = "grpSyllabusDocument";
                Page.Title = "Upload Syllabus Document";
            }
            else if (String.Compare(CurrentViewContext.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_PRECEPTOR.GetStringValue(), true) == 0)
            {
                divAssignPreceptor.Visible = true;
                cmdSaveAssignments.SaveButton.ValidationGroup = "grpAssignInstructor";
                Page.Title = "Assign Instructor/Preceptor";
            }
            else if (String.Compare(CurrentViewContext.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_PRECEPTOR_PACKAGES.GetStringValue(), true) == 0)
            {
                dvInstrüctorPkg.Visible = true;
                cmdSaveAssignments.SaveButton.ValidationGroup = "grpAssignInstPackage";
                Page.Title = "Assign Instructor/Preceptor Package";
            }
            else if (String.Compare(CurrentViewContext.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_STUDENT_PACKAGES.GetStringValue(), true) == 0)
            {
                divStudentPackages.Visible = true;
                cmdSaveAssignments.SaveButton.ValidationGroup = "grpAssignStudentPackage";
                Page.Title = "Assign Student Package";
            }
        }

        /// <summary>
        /// Sets the properties from the arguments recieved through querystring.
        /// </summary>
        /// <param name="args"></param>
        private void CaptureQuerystringParameters()
        {
            if (Request.QueryString["TenantID"] != null)
            {
                CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["TenantID"]);
            }
            if (Request.QueryString["AgencyId"] != null)
            {
                CurrentViewContext.RotationAgencyIDs = Convert.ToString(Request.QueryString["AgencyId"]);
            }
            if (Request.QueryString["SelectedRotationIds"] != null)
            {
                CurrentViewContext.RotationIDs = Request.QueryString["SelectedRotationIds"];
            }
            if (Request.QueryString["RotationAssignmentTypeCode"] != null)
            {
                CurrentViewContext.RotationAssignmentTypeCode = Request.QueryString["RotationAssignmentTypeCode"];
            }
        }

        private void SaveRotationData()
        {
            if (CurrentViewContext.RotationDataContarct.IsNullOrEmpty())
            {
                CurrentViewContext.RotationDataContarct = new ClinicalRotationDetailContract();
            }
            CurrentViewContext.RotationDataContarct.AgencyID = CurrentViewContext.AgencyID;
            CurrentViewContext.RotationDataContarct.TenantID = CurrentViewContext.TenantId;
            Boolean isSharedPackageSelected = false;
            Boolean isDataEnteredForAnyRotation = false;
            Boolean IsNewPackage = false;
            if (String.Compare(CurrentViewContext.RotationAssignmentTypeCode, RotationAssignmentType.UPLOAD_SYLLABUS.GetStringValue(), true) == 0)
            {
                if (uploadControl.UploadedFiles.Count > AppConsts.NONE)
                {
                    lblUploadFormMsg.Visible = false;
                    String savedFilePath = UploadSyllabusDocuments(uploadControl);
                    //if (savedFilePath.IsNullOrEmpty())
                    //isFileuploaeded = false;
                    CurrentViewContext.RotationDataContarct.SyllabusFilePath = savedFilePath;
                    CurrentViewContext.RotationDataContarct.SyllabusFileName = uploadControl.UploadedFiles[0].FileName;
                    CurrentViewContext.RotationDataContarct.SyllabusFileSize = uploadControl.UploadedFiles[0].ContentLength;
                }
                else
                {
                    lblUploadFormMsg.Visible = true;
                    //ShowInfoMessage("Please select Syllabus document to upload.");
                    return;
                }
            }
            else if (String.Compare(CurrentViewContext.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_PRECEPTOR.GetStringValue(), true) == 0)
            {
                List<Int32> selectedContacts = new List<Int32>();
                foreach (RadComboBoxItem slctdContact in ddlInstructor.CheckedItems)
                {
                    selectedContacts.Add(Convert.ToInt32(slctdContact.Value));
                }
                CurrentViewContext.RotationDataContarct.ContactIdList = String.Join(",", selectedContacts.ToArray());
            }
            else if (String.Compare(CurrentViewContext.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_PRECEPTOR_PACKAGES.GetStringValue(), true) == 0)
            {
                if (!Presenter.IsDataEnteredForAnyRotation(RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue()))
                {
                    isSharedPackageSelected = Convert.ToBoolean(cmbInstPackage.SelectedItem.Attributes["IsShared"]);
                    IsNewPackage = Convert.ToBoolean(cmbInstPackage.SelectedItem.Attributes["IsNewPackage"]);
                }
                else
                {
                    isDataEnteredForAnyRotation = true;
                    ShowInfoMessage("Data entry of rotation package has been performed by some of the user. Therefore, you cannot change the rotation package.");
                }
            }
            else if (String.Compare(CurrentViewContext.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_STUDENT_PACKAGES.GetStringValue(), true) == 0)
            {
                if (!Presenter.IsDataEnteredForAnyRotation(RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue()))
                {
                    isSharedPackageSelected = Convert.ToBoolean(cmbPackage.SelectedItem.Attributes["IsShared"]);
                    IsNewPackage = Convert.ToBoolean(cmbPackage.SelectedItem.Attributes["IsNewPackage"]);
                }
                else
                {
                    isDataEnteredForAnyRotation = true;
                    ShowInfoMessage("Data entry of rotation package has been performed by some of the user. Therefore, you cannot change the rotation package.");
                }
            }
            if (!isDataEnteredForAnyRotation && Presenter.SaveUpdateClinicalRotationAssignments(isSharedPackageSelected, IsNewPackage))
            {
                hdnSavedStatus.Value = "True";
                ShowMessages();
            }
            #region UAT-4657
            else if (CurrentViewContext.UnMappedRotationNameList.Any())
            {
                String RotationNames = String.Join(",", CurrentViewContext.UnMappedRotationNameList);
                ShowInfoMessage("This package is not assigned to following rotation(s) due to package effective date: " + RotationNames);
            }
            #endregion
            else
            {
                if (!CurrentViewContext.ComplioIDs.IsNullOrEmpty())
                {
                    ShowInfoMessage("Preceptor cannot be assigned to rotation(s) with following Complio ID(s) " + CurrentViewContext.ComplioIDs + " as agency setting 'Will the school be sending an instructor for this rotation ? ' is set to No.");
                }
            }

        }

        private void ShowMessages()
        {
            String RotationNames = String.Empty;
            String PopUpMessage = String.Empty;
            if (!CurrentViewContext.UnMappedRotationIDList.IsNullOrEmpty())
            {
                RotationNames = String.Join(",", CurrentViewContext.UnMappedRotationNameList);
            }
            if (String.Compare(CurrentViewContext.RotationAssignmentTypeCode, RotationAssignmentType.UPLOAD_SYLLABUS.GetStringValue(), true) == 0)
            {
                ShowSuccessMessage("Syllabus has been uploaded successfully.");
            }
            else if (String.Compare(CurrentViewContext.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_PRECEPTOR.GetStringValue(), true) == 0)
            {
                    ShowSuccessMessage("Instructor/Preceptor has been assigned successfully.");
            }
            else if (String.Compare(CurrentViewContext.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_PRECEPTOR_PACKAGES.GetStringValue(), true) == 0)
            {
                if (!CurrentViewContext.UnMappedRotationIDList.IsNullOrEmpty())
                {
                    PopUpMessage = "Instructor package has been assigned successfully, but this package is not assigned to following rotation(s) due to package effective date: " + RotationNames;
                }
                else
                {
                    PopUpMessage = "Instructor package has been assigned successfully.";
                }
                ShowSuccessMessage(PopUpMessage);
            }
            else if (String.Compare(CurrentViewContext.RotationAssignmentTypeCode, RotationAssignmentType.ASSIGN_STUDENT_PACKAGES.GetStringValue(), true) == 0)
            {

                if (!CurrentViewContext.UnMappedRotationIDList.IsNullOrEmpty())
                {
                    PopUpMessage = "Student package has been assigned successfully, but this package is not assigned to following rotation(s) due to package effective date: " + RotationNames;
                }
                else
                {
                    PopUpMessage = "Student package has been assigned successfully.";
                }
                ShowSuccessMessage(PopUpMessage);
            }
        }
        #endregion

        #region DropDown Events
        /// <summary>
        /// Dropdown DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbPackage_DataBound(object sender, EventArgs e)
        {
            try
            {
                cmbPackage.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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

        protected void cmbPackage_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            try
            {
                RequirementPackageContract dataItem = (RequirementPackageContract)e.Item.DataItem;
                e.Item.Attributes.Add("IsShared", dataItem.IsSharedUserPackage.ToString());
                e.Item.Attributes.Add("IsCopied", dataItem.IsCopied.ToString());
                if (dataItem.IsSharedUserPackage || dataItem.IsCopied)
                {
                    e.Item.Style["color"] = "#006E00";
                    e.Item.Style["font-weight"] = "Bold";
                }
                //UAT-2213
                e.Item.Attributes.Add("IsNewPackage", dataItem.IsNewPackage.ToString());
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

        protected void cmbInstPackage_DataBound(object sender, EventArgs e)
        {
            try
            {
                cmbInstPackage.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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

        protected void cmbInstPackage_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            try
            {
                RequirementPackageContract dataItem = (RequirementPackageContract)e.Item.DataItem;
                e.Item.Attributes.Add("IsShared", dataItem.IsSharedUserPackage.ToString());
                e.Item.Attributes.Add("IsCopied", dataItem.IsCopied.ToString());
                if (dataItem.IsSharedUserPackage || dataItem.IsCopied)
                {
                    e.Item.Style["color"] = "#006E00";
                    e.Item.Style["font-weight"] = "Bold";
                }
                //UAT-2213
                e.Item.Attributes.Add("IsNewPackage", dataItem.IsNewPackage.ToString());
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


    }
}