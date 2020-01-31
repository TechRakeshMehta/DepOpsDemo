using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using CuteWebUI;
using System.IO;
using System.Web.Configuration;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Linq;
using Business.RepoManagers;
using INTSOF.UI.Contract.FingerPrintSetup;
using System.Configuration;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class UploadDocuments : BaseUserControl, IUploadDocumentsView
    {

        #region Private Variables
        private UploadDocumentsPresenter _presenter = new UploadDocumentsPresenter();

        Boolean showUploadButton = true;
        String _errorMessage = String.Empty;
        Int32 organizationUserID = 0;
        String savePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
        Int32 tenantID = 0;
        Telerik.Web.UI.AsyncUpload.MultipleFileSelection multipleFileSelection = Telerik.Web.UI.AsyncUpload.MultipleFileSelection.Automatic;
        Int32 maxFileInputsCount = 0;
        #endregion


        #region Properties

        #region Public Properties
        public UploadDocumentsPresenter Presenter
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

        public IUploadDocumentsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Boolean ShowUploadButton
        {
            get { return showUploadButton; }
            set { showUploadButton = value; }
        }

        public String SavePath
        {
            get { return savePath; }
            set { savePath = value; }
        }

        public List<ApplicantDocument> ToSaveApplicantUploadedDocuments
        {
            get;
            set;
        }


        public String ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                base.LogInfo(_errorMessage);
            }
        }

        //Applicantid Get and Set when screen opens from Admin(verification Details And Portfolio Search)
        public Int32 FromAdminApplicantID
        {
            get;
            set;
        }

        //Applicantid Get and Set when screen opens from Admin(verification Details And Portfolio Search)
        public Int32 FromAdminTenantID
        {
            get;
            set;
        }

        // to check from where this screen opens
        public Boolean IsAdminScreen
        {
            get;
            set;
        }
        public Boolean IsLocationDetailScreen { get; set; }
        public Boolean IsItemFormScreen { get; set; }
        public Int32 OrganiztionUserID
        {
            get { return organizationUserID == 0 ? base.CurrentUserId : organizationUserID; }
            set { organizationUserID = value; }

        }
        //UAT-3478
        public Boolean isDropZoneEnabled
        {
            get;
            set;
        }
        public String DropzoneID
        {
            get;
            set;
        }

        public String DropzoneCSS
        {
            get;
            set;
        }


        public Int32 CurrentLoggedUserID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                }
                else
                {
                    return base.CurrentUserId;
                }
            }
        }

        public Int32 TenantID
        {
            get { return tenantID; }
            set { tenantID = value; }
        }

        public Telerik.Web.UI.AsyncUpload.MultipleFileSelection MultipleFileSelection
        {
            get { return multipleFileSelection; }
            set { uploadControl.MultipleFileSelection = value; }
        }

        public Int32 MaxFileInputsCount
        {
            get { return maxFileInputsCount; }
            set
            {
                if (value > 0)
                    uploadControl.MaxFileInputsCount = value;
            }
        }

        public List<lkpItemDocMappingType> lstItemDocMappingType
        {
            get;
            set;
        }
        public List<FingerPrintLocationImagesContract> AddedLocationImagesData { get; set; }
        private Boolean _isCorruptedFileUploaded = false;

        public String FilePath { get; set; }
        public String OriginalFileName { get; set; }
        public Int32 LocationId { get; set; }

        #region UAT-4270
        public FingerPrintOrderContract fingerPrintOrderContract { get; set; }
        public Boolean IsFingerPrintAppointmentDetailScreen { get; set; }
        public Int32 FingerprintApplicantAppointmentId { get; set; }
        public delegate void UploadDelegateResponse(Boolean Response, String message);
        public event UploadDelegateResponse OnCompletedFileUpload;
        public String errorMSg;
        #endregion
        #region ABI Review
        public Boolean IsAbiReviewUpload
        {
            get
            {
                if (!ViewState["IsAbiReviewUpload"].IsNullOrEmpty())
                {
                    return (Boolean)ViewState["IsAbiReviewUpload"];
                }
                return false;
            }
            set
            {
                ViewState["IsAbiReviewUpload"] = value;
            }
        }
        #endregion

        public delegate void UploadDelegate();
        public event UploadDelegate OnCompletedUpload;


        #region UAT-1049:Admin Data Entry
        public Int16 DataEntryDocNewStatusId
        {
            get
            {
                if (ViewState["DataEntryDocNewStatusId"] != null)
                    return Convert.ToInt16(ViewState["DataEntryDocNewStatusId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["DataEntryDocNewStatusId"] = value;
            }
        }

        public Int16 DataEntryDocCompleteStatusId
        {
            get
            {
                if (ViewState["DataEntryDocCompleteStatusId"] != null)
                    return Convert.ToInt16(ViewState["DataEntryDocCompleteStatusId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["DataEntryDocCompleteStatusId"] = value;
            }
        }

        public List<UploadDocumentContract> lstSubcribedItems
        {
            get
            {
                if (ViewState["lstSubcribedItems"] != null)
                {
                    return ViewState["lstSubcribedItems"] as List<UploadDocumentContract>;
                }
                return new List<UploadDocumentContract>();
            }
            set
            {
                ViewState["lstSubcribedItems"] = value;
            }
        }


        public Boolean IsPersonalDocumentScreen
        {
            get
            {
                if (!ViewState["IsPersonalDocumentScreen"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsPersonalDocumentScreen"]);
                }
                return false;
            }
            set
            {
                ViewState["IsPersonalDocumentScreen"] = value;
            }
        }
        //UAT-3593
        public Boolean IsInstructorPreceptorDocumentScreen
        {
            get
            {
                if (!ViewState["IsInstructorPreceptorDocumentScreen"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsInstructorPreceptorDocumentScreen"]);
                }
                return false;
            }
            set
            {
                ViewState["IsInstructorPreceptorDocumentScreen"] = value;
            }
        }

        #endregion

        //UAT-4067
        public String AllowedExtensions
        {
            get;
            set;
        }
        public Boolean IsAllowedFileExtensionEnable
        {
            get;
            set;
        }


        #endregion
        #endregion

        #region Events

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPersonalDocumentScreen || IsLocationDetailScreen || IsItemFormScreen || IsFingerPrintAppointmentDetailScreen)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "HideDropDown();", true);
            }
            //if (!this.IsPostBack)
            //{

            Presenter.OnViewInitialized();
            //}
            Presenter.OnViewLoaded();
            HideButton();

            uploadControl.MaxFileSize = Convert.ToInt32(WebConfigurationManager.AppSettings[AppConsts.MAXIMUM_FILE_SIZE]);
            if (IsAdminScreen)
                hdfOrganizationUserId.Value = FromAdminApplicantID.ToString();

            //UAT-2128 Get ClientSettings. 
            if (IsPersonalDocumentScreen)
            {
                cmbItems.Visible = false;
                hdnDocumentAssociationSettingEnabled.Value = AppConsts.ZERO;
                CurrentViewContext.lstSubcribedItems = new List<UploadDocumentContract>();
            }
            else if (IsLocationDetailScreen)
            {
                uploadControl.AllowedFileExtensions = new String[] { ".jpg", ".jpeg", ".tiff", ".bmp", ".bitmap", ".png", ".JPG", ".PNG", ".BITMAP", ".JPEG", ".TIFF", ".BMP" };
                cmbItems.Visible = false;
                hdnDocumentAssociationSettingEnabled.Value = AppConsts.ZERO;
                CurrentViewContext.lstSubcribedItems = new List<UploadDocumentContract>();
                hdrUploadDocument.Visible = false;
            }
            else if (IsFingerPrintAppointmentDetailScreen)
            {
                String[] extentions = WebConfigurationManager.AppSettings["ManualFingerprintFileExtention"].Split(',').ToArray();
                uploadControl.AllowedFileExtensions = new String[extentions.Length];
                uploadControl.AllowedFileExtensions = extentions;
                cmbItems.Visible = false;
                hdnDocumentAssociationSettingEnabled.Value = AppConsts.ZERO;
                CurrentViewContext.lstSubcribedItems = new List<UploadDocumentContract>();
                hdrUploadDocument.Visible = false;
            }
            else if (IsItemFormScreen)
            {
                cmbItems.Visible = false;
                hdnDocumentAssociationSettingEnabled.Value = AppConsts.ZERO;
                CurrentViewContext.lstSubcribedItems = new List<UploadDocumentContract>();
                hdrUploadDocument.Visible = false;
            }
            else
            {

                List<ClientSetting> lstClientSetting = new List<ClientSetting>();

                //UAT-3593
                if (!IsInstructorPreceptorDocumentScreen)
                {
                    lstClientSetting = Presenter.GetClientSetting();
                }
                else
                {
                    hdfOrganizationUserId.Value = Convert.ToString(CurrentViewContext.CurrentLoggedUserID);
                    //hdfSelectedTenantId.Value = Convert.ToString(TenantID);
                }
                var _setting = lstClientSetting.IsNullOrEmpty() ? null : lstClientSetting.FirstOrDefault(cs => cs.lkpSetting.Code == Setting.APPLICANT_DOCUMENT_ASSOCIATION.GetStringValue());

                if (_setting.IsNullOrEmpty() || _setting.CS_SettingValue == AppConsts.ZERO)
                {
                    cmbItems.Visible = false;
                }
                if (!_setting.IsNullOrEmpty() && !CurrentViewContext.IsAdminScreen)
                {
                    hdnDocumentAssociationSettingEnabled.Value = _setting.CS_SettingValue;
                }
                else
                {
                    hdnDocumentAssociationSettingEnabled.Value = AppConsts.ZERO;
                }

                //UAT-2128 Get Applicant subscribed package categorys and items.
                if (hdnDocumentAssociationSettingEnabled.Value == AppConsts.STR_ONE)
                {
                    Presenter.GetSubscribedPackagesItems();
                    cmbItems.DataSource = CurrentViewContext.lstSubcribedItems;
                    cmbItems.DataBind();
                }
            }
            if (IsItemFormScreen)
            {
                hdrUploadDocument.Visible = false;
                DropzoneCSS = "mod-content issue-drop-zoneItemForm dvApplicantDocumentDropzone drgndrop_border_class";
            }
            else
            {
                DropzoneCSS = "mod-content issue-drop-zone dvApplicantDocumentDropzone drgndrop_border_class";
            }

        }

        protected void Page_Init(object sender, EventArgs e)
        {

        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.DropzoneID.IsNullOrEmpty())
                return;
            this.uploadControl.ToolTip = "";
            if (uploadControl.DropZones.Length > 0)
            {
                String dropzonename = uploadControl.DropZones[0];
                this.DropzoneID = dropzonename.Remove("#") + "0";
            }
            String[] dropzone = new String[] { "#" + this.DropzoneID };
            uploadControl.DropZones = dropzone;
            this.uploadControl.Localization.Select = "Hidden";

            //UAT-4067
            if (IsAllowedFileExtensionEnable)
            {
                String[] allowedExtensions = AllowedExtensions.Split(',');
                uploadControl.AllowedFileExtensions = allowedExtensions;
            }
        }
        #endregion

        #region Button Events
        protected void btnUploadCancel_Click(object sender, EventArgs e)
        {
            uploadControl.UploadedFiles.Clear();
            HideButton();
        }

        protected void btnUploadAll_Click(object sender, EventArgs e)
        {
            if (IsLocationDetailScreen)
            {
                CheckAndSaveProfilePic();
                Presenter.SaveLocationImages(LocationId);
                if (OnCompletedUpload != null)
                    OnCompletedUpload();
            }
            else if (IsFingerPrintAppointmentDetailScreen)
            {
                Boolean reponse = UploadManualFingerPrintFile();
                if (OnCompletedFileUpload != null)
                {
                    errorMSg = reponse ? "File submitted successfully." : errorMSg;
                    OnCompletedFileUpload(reponse, errorMSg);

                }
            }
            else
                UploadAllDocuments();
            HideButton();
        }
        #endregion

        #region ComboBox Events

        /// <summary>
        /// UAT-2128 Add Drop down to  associate documetns with items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbItems_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            if (e.Item.Value.Contains("_-1"))
            {
                e.Item.IsSeparator = true;
                e.Item.CssClass = "Category";
                e.Item.Enabled = false;
            }
        }
        #endregion
        #endregion

        #region Private Methods
        public void UploadAllDocuments()
        {
            try
            {
                String filePath = String.Empty;
                Boolean aWSUseS3 = false;
                //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                Boolean isCorruptedFileUploaded = false;
                StringBuilder corruptedFileMessage = new StringBuilder();
                String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
                StringBuilder docMessage = new StringBuilder();

                if (!WebConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                {
                    aWSUseS3 = Convert.ToBoolean(WebConfigurationManager.AppSettings["AWSUseS3"]);
                }
                if (tempFilePath.IsNullOrEmpty())
                {
                    base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                    return;
                }
                if (!tempFilePath.EndsWith(@"\"))
                {
                    tempFilePath += @"\";
                }
                tempFilePath += "Tenant(" + tenantID.ToString() + @")\";

                if (!Directory.Exists(tempFilePath))
                    Directory.CreateDirectory(tempFilePath);

                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    if (filePath.IsNullOrEmpty())
                    {
                        base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                        return;
                    }
                    if (!filePath.EndsWith("\\"))
                    {
                        filePath += "\\";
                    }

                    filePath += "Tenant(" + tenantID.ToString() + @")\";

                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);
                }

                Presenter.GetlkpItemDocMappingType();
                foreach (UploadedFile item in uploadControl.UploadedFiles)
                {
                    ApplicantDocument applicantDocument = new ApplicantDocument();
                    String fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    //Save file
                    String newTempFilePath = Path.Combine(tempFilePath, fileName);
                    item.SaveAs(newTempFilePath);

                    if (ToSaveApplicantUploadedDocuments == null)
                        ToSaveApplicantUploadedDocuments = new List<ApplicantDocument>();

                    //Code changes for UAT 531- As a student, I should not be able to upload a duplicate document.
                    //Get original file bytes and check if same document is already uploaded
                    byte[] fileBytes = File.ReadAllBytes(newTempFilePath);
                    var documentName = Presenter.IsDocumentAlreadyUploaded(item.FileName, item.ContentLength, fileBytes, IsPersonalDocumentScreen, IsInstructorPreceptorDocumentScreen);

                    if (!documentName.IsNullOrEmpty())
                    {
                        //docMessage.Append("You have already updated " + item.FileName + " document as " + documentName + ". <BR/>");
                        docMessage.Append("You have already updated " + item.FileName + " document as " + documentName + ". \\n");
                        continue;
                    }

                    //Check whether use AWS S3, true if need to use
                    if (aWSUseS3 == false)
                    {
                        //Move file to other location
                        String destFilePath = Path.Combine(filePath, fileName);
                        File.Copy(newTempFilePath, destFilePath);
                        applicantDocument.DocumentPath = destFilePath;

                    }
                    else
                    {
                        if (filePath.IsNullOrEmpty())
                        {
                            base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                            return;
                        }
                        if (!filePath.EndsWith("//"))
                        {
                            filePath += "//";
                        }
                        //AWS code to save document to S3 location
                        AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                        String destFolder = filePath + "Tenant(" + tenantID.ToString() + @")/";
                        String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, fileName, destFolder);
                        //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                        if (returnFilePath.IsNullOrEmpty())
                        {
                            isCorruptedFileUploaded = true;
                            corruptedFileMessage.Append("Your file " + item.FileName + " is not uploaded. \\n");
                            continue;
                        }
                        applicantDocument.DocumentPath = returnFilePath; //Path.Combine(destFolder, fileName);
                    }
                    try
                    {
                        if (!String.IsNullOrEmpty(newTempFilePath))
                            File.Delete(newTempFilePath);
                    }
                    catch (Exception) { }

                    if (CurrentViewContext.IsAdminScreen == false)
                    {
                        //when documents are uploaded from Applicant Data Entry Screen 
                        applicantDocument.OrganizationUserID = OrganiztionUserID;
                    }
                    else
                    {
                        //when documents are uploaded from Admin's Screen 
                        applicantDocument.OrganizationUserID = FromAdminApplicantID;
                    }
                    applicantDocument.FileName = item.FileName;
                    applicantDocument.Size = item.ContentLength;
                    applicantDocument.Description = item.GetFieldValue("TextBox");
                    applicantDocument.CreatedByID = CurrentLoggedUserID;
                    applicantDocument.CreatedOn = DateTime.Now;
                    applicantDocument.IsDeleted = false;

                    //UAT-3593
                    if (IsInstructorPreceptorDocumentScreen)
                        applicantDocument.DataEntryDocumentStatusID = DataEntryDocCompleteStatusId;
                    else
                        applicantDocument.DataEntryDocumentStatusID = IsPersonalDocumentScreen ? DataEntryDocCompleteStatusId : DataEntryDocNewStatusId;//Set Data Entry Document Status of type new [UAT-1049:Admin Data Entry] 


                    if (hdnDocumentAssociationSettingEnabled.Value == "1")
                    {
                        String mappedItemIds = item.GetFieldValue("HiddedFeild");
                        if (!mappedItemIds.IsNullOrEmpty())
                        {
                            string[] mappedIds = mappedItemIds.Split(',');
                            foreach (string id in mappedIds)
                            {
                                String[] catItemId = id.Split('_');
                                if (catItemId[1] != "-1")
                                {
                                    DocItemAssociationForDataEntry newRecord = new DocItemAssociationForDataEntry();
                                    newRecord.DAFD_ComplianceCategoryId = Convert.ToInt32(catItemId[0]);
                                    Int32? itemId = Convert.ToInt32(catItemId[1]) == AppConsts.NONE ? (Int32?)null : Convert.ToInt32(catItemId[1]);

                                    newRecord.DAFD_ComplianceItemId = itemId;
                                    newRecord.DAFD_MappingType = itemId.IsNull() ? CurrentViewContext.lstItemDocMappingType.FirstOrDefault(cond => cond.IDMT_Code == ItemDocMappingType.CATEGORY_EXCEPTION.GetStringValue()).IDMT_ID
                                                                                    : CurrentViewContext.lstItemDocMappingType.FirstOrDefault(cond => cond.IDMT_Code == ItemDocMappingType.ITEM_DATA.GetStringValue()).IDMT_ID;
                                    newRecord.DAFD_IsDeleted = false;
                                    newRecord.DAFD_CreatedOn = DateTime.Now;
                                    newRecord.DAFD_CreatedById = CurrentLoggedUserID;
                                    applicantDocument.DocItemAssociationForDataEntries.Add(newRecord);
                                }
                            }
                        }
                    }
                    ToSaveApplicantUploadedDocuments.Add(applicantDocument);
                }
                if (ToSaveApplicantUploadedDocuments != null && ToSaveApplicantUploadedDocuments.Count > 0)
                {
                    String newFilePath = String.Empty;
                    if (aWSUseS3 == false)
                    {
                        newFilePath = filePath;
                    }
                    else
                    {
                        if (filePath.IsNullOrEmpty())
                        {
                            base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                            return;
                        }
                        if (!filePath.EndsWith("//"))
                        {
                            filePath += "//";
                        }

                        newFilePath = filePath + "Tenant(" + tenantID.ToString() + @")/";
                    }
                    if (Presenter.AddApplicantUploadedDocuments(newFilePath, IsPersonalDocumentScreen))
                    {
                        if (!IsPersonalDocumentScreen)
                        {
                            Presenter.CallParallelTaskPdfConversionMerging();
                        }
                        else
                        {
                            Presenter.CallParallelTaskPdfConversionWithoutMerging();
                        }
                        //if (!String.IsNullOrEmpty(ErrorMessage))
                        //{
                        //    base.LogDebug(ErrorMessage);
                        //}

                        ////UAT-2297: For schools where the document association at time of upload is turned on, the success message for upload complete should be different.
                        //if (hdnDocumentAssociationSettingEnabled.Value == "1")
                        //{
                        //    base.ShowSuccessMessage("<b>Thank you for uploading your documents. Please understand that there are a large number of documents being uploaded at this time so we are prioritizing documents that have not previously been reviewed in another system. If a document has been previously reviewed and accepted in another system (via fax or Castle Branch), your compliance status will not be affected if the document has not yet been reviewed and accepted in Complio. If you have any questions about documents that are pending review or your compliance status, please contact your Clinical Compliance Coordinator.<br/><br/>Thank you for your patience and understanding with the migration efforts.<b/>");
                        //}
                        //else
                        //{
                        if (IsAdminScreen || IsPersonalDocumentScreen || IsInstructorPreceptorDocumentScreen)
                        {
                            base.ShowSuccessMessage("Document Saved Successfully.");
                        }
                        else
                        {
                            //UAT-2431:Customizable document upload confirmation text by client
                            List<ClientSetting> lstClientSetting = Presenter.GetClientSetting();
                            var _setting = lstClientSetting.FirstOrDefault(cs => cs.lkpSetting.Code == Setting.DOCUMENT_UPLOAD_CONFIRMATION_MESSAGE_TEXT.GetStringValue());
                            base.ShowSuccessMessage(_setting.CS_SettingValue);

                            //base.ShowSuccessMessage("<b>Please return to the Data Entry Screen to complete your requirements.<b/>");
                        }
                        //  }
                    }
                    else
                    {
                        base.ShowErrorMessage("Document cannot be Saved.");
                    }
                }
                if (OnCompletedUpload != null)
                    OnCompletedUpload();

                //Code changes for UAT 531- As a student, I should not be able to upload a duplicate document.
                if ((docMessage.Length > 0 && !(docMessage.ToString().IsNullOrEmpty())) || (corruptedFileMessage.Length > 0 && !(corruptedFileMessage.ToString().IsNullOrEmpty())))
                {
                    //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                    if (isCorruptedFileUploaded)
                    {
                        corruptedFileMessage.Append("Please again upload these documents .");
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCallBackMessage('" + corruptedFileMessage.ToString() + "');", true);
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCallBackMessage('" + docMessage.ToString() + "');", true);
                    }
                }
            }
            catch(Exception ex)
            {
                LogError(ex);
            }
        }

        private void HideButton()
        {
            btnUploadCancel.Style.Add("display", "none");
            btnUploadAll.Style.Add("display", "none");
        }
        #endregion

        #region FingerPrint Location
        /// <summary>
        /// Method for uploading and saving of profile picture
        /// </summary>
        private void CheckAndSaveProfilePic()
        {
            try
            {
                if (uploadControl.UploadedFiles.Count > 0)
                {
                    AddedLocationImagesData = new List<FingerPrintLocationImagesContract>();
                    for (int i = 0; i < uploadControl.UploadedFiles.Count; i++)
                    {
                        //Entity.OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(CurrentViewContext.CurrentLoggedInUserId);
                        Boolean aWSUseS3 = false;
                        if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                        {
                            aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                        }
                        //DeleteOriginalFile(organizationUser, aWSUseS3);
                        List<String> extensions = new List<String>();
                        extensions.Add(".jpg");
                        extensions.Add(".jpeg");
                        extensions.Add(".tiff");
                        extensions.Add(".bmp");
                        extensions.Add(".bitmap");
                        extensions.Add(".png");

                        String fileExtension = Path.GetExtension(uploadControl.UploadedFiles[i].FileName);
                        if (extensions.Contains(fileExtension.ToLower()))
                        {
                            OriginalFileName = uploadControl.UploadedFiles[i].FileName;
                            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss");
                            String fileName = OriginalFileName + "_" + date + Path.GetExtension(uploadControl.UploadedFiles[i].FileName);

                            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                            if (tempFilePath.IsNullOrEmpty())
                            {
                                base.LogError("Please provide path for TemporaryFileLocation in config", null);
                                return;
                            }
                            if (!tempFilePath.EndsWith(@"\"))
                            {
                                tempFilePath += @"\";
                            }
                            tempFilePath += "Location(" + LocationId.ToString() + @")\" + @"Pics\";
                            if (!Directory.Exists(tempFilePath))
                                Directory.CreateDirectory(tempFilePath);
                            //String fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadControl.UploadedFiles[0].FileName);
                            tempFilePath = Path.Combine(tempFilePath, fileName);
                            uploadControl.UploadedFiles[i].SaveAs(tempFilePath);
                            FilePath = WebConfigurationManager.AppSettings[AppConsts.LOCATION_TENANT_FILE_LOCATION];
                            //Check whether use AWS S3, true if need to use
                            if (aWSUseS3 == false)
                            {
                                if (FilePath.IsNullOrEmpty())
                                {
                                    base.LogError("Please provide path for " + AppConsts.LOCATION_TENANT_FILE_LOCATION + " in config", null);
                                    return;
                                }
                                if (!FilePath.EndsWith(@"\"))
                                {
                                    FilePath += @"\";
                                }
                                FilePath += "Location(" + LocationId.ToString() + @")\";
                                FilePath = FilePath + @"Pics\";
                                if (!Directory.Exists(FilePath))
                                    Directory.CreateDirectory(FilePath);

                                FilePath = Path.Combine(FilePath, fileName);
                                MoveFile(tempFilePath, FilePath);
                            }
                            else
                            {
                                if (FilePath.IsNullOrEmpty())
                                {
                                    base.LogError("Please provide path for " + AppConsts.LOCATION_TENANT_FILE_LOCATION + " in config", null);
                                    return;
                                }
                                if (!FilePath.EndsWith("//"))
                                {
                                    FilePath += "//";
                                }
                                //AWS code to save document to S3 location
                                AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                                String destFolder = FilePath + "Location(" + LocationId.ToString() + @")/" + @"Pics/";
                                String returnFilePath = objAmazonS3.SaveDocument(tempFilePath, fileName, destFolder);
                                try
                                {
                                    if (!String.IsNullOrEmpty(tempFilePath))
                                        File.Delete(tempFilePath);
                                }
                                catch (Exception) { }
                                //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                                if (returnFilePath.IsNullOrEmpty())
                                {
                                    _isCorruptedFileUploaded = true;
                                }
                                FilePath = returnFilePath; //Path.Combine(destFolder, fileName);
                            }
                            FingerPrintLocationImagesContract ImageData = new FingerPrintLocationImagesContract();
                            ImageData.FPLI_FileName = fileName;
                            ImageData.FPLI_FilePath = FilePath;
                            ImageData.OriginalFileName = OriginalFileName;
                            AddedLocationImagesData.Add(ImageData);
                        }
                    }

                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// Move file to other location
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationFilePath"></param>
        /// <returns></returns>
        private static void MoveFile(String sourceFilePath, String destinationFilePath)
        {
            if (!String.IsNullOrEmpty(sourceFilePath))
            {
                File.Copy(sourceFilePath, destinationFilePath);
            }
            try
            {
                if (!String.IsNullOrEmpty(sourceFilePath))
                    File.Delete(sourceFilePath);
            }
            catch (Exception) { }
        }
        #endregion

        #region UAT - 4270
        private Boolean UploadManualFingerPrintFile()
        {
            Boolean result = false;
            try
            {
                if (uploadControl.UploadedFiles.Count > 0)
                {
                    //fingerPrintOrderContract = new FingerPrintOrderContract();
                    for (int i = 0; i < uploadControl.UploadedFiles.Count; i++)
                    {
                        //Entity.OrganizationUser organizationUser = SecurityManager.GetOrganizationUser(CurrentViewContext.CurrentLoggedInUserId);
                        Boolean aWSUseS3 = false;
                        if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                        {
                            aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                        }
                        //DeleteOriginalFile(organizationUser, aWSUseS3);
                        List<String> extensions = new List<String>();
                        List<String> extnList = WebConfigurationManager.AppSettings["ManualFingerprintFileExtention"].Split(',').ToList();
                        foreach (var ext in extnList)
                        {
                            extensions.Add(ext);
                        }
                        String fileExtension = Path.GetExtension(uploadControl.UploadedFiles[i].FileName);
                        if (extensions.Contains(fileExtension.ToLower()))
                        {
                            OriginalFileName = Path.GetFileNameWithoutExtension(uploadControl.UploadedFiles[i].FileName);

                            String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss");
                            String fileName = OriginalFileName + "_" + date + fileExtension;
                            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                            if (tempFilePath.IsNullOrEmpty())
                            {
                                errorMSg = "Please provide path for TemporaryFileLocation in config";
                                base.LogError("Please provide path for TemporaryFileLocation in config", null);
                                return result;
                            }
                            if (!tempFilePath.EndsWith(@"\"))
                            {
                                tempFilePath += @"\";
                            }
                            tempFilePath += "Tenant(" + TenantID.ToString() + @")\" + @"ManualFile\";
                            if (!Directory.Exists(tempFilePath))
                                Directory.CreateDirectory(tempFilePath);
                            //String fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadControl.UploadedFiles[0].FileName);
                            tempFilePath = Path.Combine(tempFilePath, fileName);
                            uploadControl.UploadedFiles[i].SaveAs(tempFilePath);
                            byte[] fileBytes = File.ReadAllBytes(tempFilePath);
                            FilePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
                            //Check whether use AWS S3, true if need to use
                            if (aWSUseS3 == false)
                            {
                                if (FilePath.IsNullOrEmpty())
                                {
                                    errorMSg = "Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config";
                                    base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config", null);
                                    return result;
                                }
                                if (!FilePath.EndsWith(@"\"))
                                {
                                    FilePath += @"\";
                                }
                                FilePath += "Tenant(" + TenantID.ToString() + @")\";
                                FilePath = FilePath + @"ManualFile\";
                                if (!Directory.Exists(FilePath))
                                    Directory.CreateDirectory(FilePath);

                                FilePath = Path.Combine(FilePath, fileName);
                                MoveFile(tempFilePath, FilePath);
                            }
                            else
                            {
                                if (FilePath.IsNullOrEmpty())
                                {
                                    errorMSg = "Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config";
                                    base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config", null);
                                    return result;
                                }
                                if (!FilePath.EndsWith("//"))
                                {
                                    FilePath += "//";
                                }
                                //AWS code to save document to S3 location
                                AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                                String destFolder = FilePath + "Tenant(" + TenantID.ToString() + @")/" + @"ManualFile/";
                                String returnFilePath = objAmazonS3.SaveDocument(tempFilePath, fileName, destFolder);
                                try
                                {
                                    if (!String.IsNullOrEmpty(tempFilePath))
                                        File.Delete(tempFilePath);
                                }
                                catch (Exception) { }
                                //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                                if (returnFilePath.IsNullOrEmpty())
                                {
                                    _isCorruptedFileUploaded = true;
                                }
                                FilePath = returnFilePath; //Path.Combine(destFolder, fileName);

                                String docTypeCode = DislkpDocumentType.FINGERPRINT_DOCUMENT.GetStringValue();
                                var documentType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(TenantID).FirstOrDefault(x => x.DMT_Code == docTypeCode && !x.DMT_IsDeleted);
                                ApplicantDocument applicantDoc = new ApplicantDocument();
                                applicantDoc.DocumentPath = FilePath;
                                applicantDoc.OrganizationUserID = OrganiztionUserID;
                                applicantDoc.FileName = fileName;
                                applicantDoc.DocumentType = documentType.IsNullOrEmpty() ? AppConsts.NONE : documentType.DMT_ID;
                                applicantDoc.Description = "Fingerprint result manual document for the applicant bkg fingerprint order";
                                applicantDoc.OriginalDocMD5Hash = fileBytes.IsNullOrEmpty() ? null : CommonFileManager.GetMd5Hash(fileBytes);
                                applicantDoc.Size = fileBytes.IsNullOrEmpty() ? AppConsts.NONE : fileBytes.Length;
                                applicantDoc.CreatedByID = CurrentLoggedUserID;
                                applicantDoc.CreatedOn = DateTime.Now;
                                applicantDoc.IsDeleted = false;

                                result = Presenter.SaveManualFingerPrintFile(applicantDoc, FingerprintApplicantAppointmentId, TenantID, IsAbiReviewUpload);
                            }


                        }
                    }

                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion


    }
}

