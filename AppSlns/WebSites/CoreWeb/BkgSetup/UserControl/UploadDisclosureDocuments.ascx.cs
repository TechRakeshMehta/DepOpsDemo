using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using CuteWebUI;
using System.IO;
using System.Web.Configuration;
using INTSOF.Utils;
using Entity;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;
using iTextSharp.text.pdf;
using Business.RepoManagers;

namespace CoreWeb.BkgSetup.Views
{
    public partial class UploadDisclosureDocuments : BaseUserControl, IUploadDisclosureDocumentsView
    {
        #region Variables

        #region Private Variables
        Boolean showUploadButton = true;
        String _errorMessage = String.Empty;
        String savePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
        Int32 maxFileInputsCount = 0;
        #endregion

        #endregion

        #region Properties

        #region Private Properties
        private UploadDisclosureDocumentsPresenter _presenter = new UploadDisclosureDocumentsPresenter();
        #endregion

        #region Public Properties
        public IUploadDisclosureDocumentsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public UploadDisclosureDocumentsPresenter Presenter
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

        List<SystemDocument> IUploadDisclosureDocumentsView.ToSaveUploadedDisclosureDocuments
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

        Telerik.Web.UI.AsyncUpload.MultipleFileSelection multipleFileSelection = Telerik.Web.UI.AsyncUpload.MultipleFileSelection.Automatic;
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

        public String DocumentType { get; set; }

        public Boolean? IsOperational { get; set; }

        public Boolean? SendToStudent { get; set; }

        #region UAT-2625: Add ability to choose 18 and above, 17 or under, and all ages as options on D&A association
        public Int32? SelectedAgeGroup { get; set; }
        #endregion

        //UAT-3745
        public Int32? SelectedExtBkgSvcID { get; set; }

        public delegate void UploadDelegate();
        public event UploadDelegate OnCompletedUpload;
        #endregion
        #endregion

        #region Events

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
            HideButton();
            uploadControl.MaxFileSize = Convert.ToInt32(WebConfigurationManager.AppSettings[AppConsts.MAXIMUM_FILE_SIZE]);
        }

        protected void Page_Init(object sender, EventArgs e)
        {

        }
        #endregion

        protected void btnUploadCancel_Click(object sender, EventArgs e)
        {
            uploadControl.UploadedFiles.Clear();
            HideButton();
        }

        protected void btnUploadAll_Click(object sender, EventArgs e)
        {
            if (!DocumentType.IsNullOrEmpty())
            {
                //UAT-2625
                if ((DocumentType != DislkpDocumentType.DISCLOSURE_AND_RELEASE_TEMPLATE.GetStringValue())
                    || (DocumentType == DislkpDocumentType.DISCLOSURE_AND_RELEASE_TEMPLATE.GetStringValue() && SelectedAgeGroup > AppConsts.NONE))
                {
                    UploadAllDocuments();
                    HideButton();
                }
                else
                {
                    base.ShowInfoMessage("Please select Age Group.");
                }
            }
            else
            {
                base.ShowInfoMessage("Please select document type.");
            }

        }

        #endregion

        #region Methods
        #region Private Methods
        private void HideButton()
        {
            btnUploadCancel.Style.Add("display", "none");
            btnUploadAll.Style.Add("display", "none");
        }
        #endregion

        #region Public Methods
        public void UploadAllDocuments()
        {
            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];

            if (tempFilePath.IsNullOrEmpty())
            {
                base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                return;
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "SystemDocuments\\";

            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);

            foreach (UploadedFile item in uploadControl.UploadedFiles)
            {
                SystemDocument systemDocument = new SystemDocument();
                String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                String fileName = "DR_" + SecurityManager.DefaultTenantID.ToString() + "_" + date + Path.GetExtension(item.FileName);

                //Save file at temporary location
                String newTempFilePath = Path.Combine(tempFilePath, fileName);
                item.SaveAs(newTempFilePath);

                if (CurrentViewContext.ToSaveUploadedDisclosureDocuments == null)
                    CurrentViewContext.ToSaveUploadedDisclosureDocuments = new List<SystemDocument>();


                String destFilePath = @"SystemDocuments\" + fileName;

                String filePath = CommonFileManager.SaveDocument(newTempFilePath, destFilePath, FileType.SystemDocumentLocation.GetStringValue());

                systemDocument.DocumentPath = filePath;
                systemDocument.FileName = item.FileName;
                systemDocument.Size = item.ContentLength;
                systemDocument.Description = item.GetFieldValue("TextBox");
                systemDocument.CreatedBy = CurrentUserId;
                systemDocument.CreatedOn = DateTime.Now;
                systemDocument.IsDeleted = false;
                //UAT 1560: WB: We should be able to add documents that need to be signed to the order process
                systemDocument.SD_DocType_ID = Presenter.GetDocumentTypeIDByCode(DocumentType);
                systemDocument.IsOperational = IsOperational;
                systemDocument.SendToStudent = SendToStudent;

                //UAT-2625:Add ability to choose 18 and above, 17 or under, and all ages as options on D&A association
                if (SelectedAgeGroup > AppConsts.NONE)
                {
                    systemDocument.DisclosureDocumentAgeGroupID = SelectedAgeGroup;
                }
                else
                {
                    systemDocument.DisclosureDocumentAgeGroupID = null;
                }

                //Save DocumentFields
                PdfReader pdfReader = new PdfReader(newTempFilePath);
                String fileStreamPath = Path.Combine(tempFilePath, "Temp_" + fileName);
                FileStream fileStream = new FileStream(fileStreamPath, FileMode.Create);
                PdfStamper stamper = new PdfStamper(pdfReader, fileStream);
                AcroFields acroFields = stamper.AcroFields;
                if (acroFields.IsNotNull())
                {
                    foreach (var acroField in acroFields.Fields)
                    {
                        systemDocument.SysDocumentFieldMappings.Add(new SysDocumentFieldMapping
                        {
                            SDFM_FieldName = acroField.Key,
                            SDFM_AttributeGroupMappingID = null,
                            SDFM_IsDeleted = false,
                            SDFM_CreatedBy = CurrentUserId,
                            SDFM_CreatedOn = DateTime.Now,
                        });
                    }
                }
                stamper.Close();
                pdfReader.Close();
                fileStream.Close();

                //UAT-3745
                if (String.Compare(DocumentType.ToLower(), DislkpDocumentType.ADDITIONAL_DOCUMENTS.GetStringValue().ToLower()) == 0
                    && !SelectedExtBkgSvcID.IsNullOrEmpty() && SelectedExtBkgSvcID > AppConsts.NONE)
                {
                    ExtBkgSvcDocumentMapping extBkgSvcDocumentMapping = new ExtBkgSvcDocumentMapping();
                    extBkgSvcDocumentMapping.EBSDM_DocumentTypeID = Convert.ToInt32(systemDocument.SD_DocType_ID);
                    extBkgSvcDocumentMapping.EBSDM_ExtBkgSvcID = Convert.ToInt32(SelectedExtBkgSvcID);
                    extBkgSvcDocumentMapping.EBSDM_CreatedBy = CurrentUserId;
                    extBkgSvcDocumentMapping.EBSDM_CreatedOn = DateTime.Now;

                    systemDocument.ExtBkgSvcDocumentMappings.Add(extBkgSvcDocumentMapping);
                }
                //End

                CurrentViewContext.ToSaveUploadedDisclosureDocuments.Add(systemDocument);

                try
                {
                    if (!String.IsNullOrEmpty(newTempFilePath))
                        File.Delete(newTempFilePath);
                    if (!String.IsNullOrEmpty(fileStreamPath))
                        File.Delete(fileStreamPath);
                }
                catch (Exception) { }
            }
            if (CurrentViewContext.ToSaveUploadedDisclosureDocuments != null && CurrentViewContext.ToSaveUploadedDisclosureDocuments.Count > 0)
            {
                if (Presenter.AddApplicantUploadedDocuments())
                {
                    base.ShowSuccessMessage("D&A Document Saved Sucessfully.");
                }
                else
                {
                    base.ShowErrorMessage("D&A Document cannot be Saved.");
                }
            }
            if (OnCompletedUpload != null)
                OnCompletedUpload();
        }
        #endregion
        #endregion

    }
}

