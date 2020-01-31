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
using iTextSharp.text.pdf;
using Business.RepoManagers;
using System.Configuration;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class UploadAttributeDocuments : BaseUserControl, IUploadAttributeDocumentsView
    {
        private UploadAttributeDocumentsPresenter _presenter = new UploadAttributeDocumentsPresenter();

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

        public IUploadAttributeDocumentsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 TenantID
        {
            get
            {
                if (!ViewState["TenantID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["TenantID"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["TenantID"] = value;
            }
        }

        Boolean showUploadButton = true;
        public Boolean ShowUploadButton
        {
            get { return showUploadButton; }
            set { showUploadButton = value; }
        }

        String savePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
        public String SavePath
        {
            get { return savePath; }
            set { savePath = value; }
        }

        List<ClientSystemDocument> IUploadAttributeDocumentsView.ToSaveUploadedComplianceViewDocuments
        {
            get;
            set;
        }

        String _errorMessage = String.Empty;
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

        Int32 maxFileInputsCount = 0;
        public Int32 MaxFileInputsCount
        {
            get { return maxFileInputsCount; }
            set
            {
                if (value > 0)
                    uploadControl.MaxFileInputsCount = value;
            }
        }

        public UploadAttributeDocumentsPresenter Presenter
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

        public delegate void UploadDelegate();
        public event UploadDelegate OnCompletedUpload;

        public void UploadAllDocuments()
        {
            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
            //String filePath = String.Empty;
            String fileSystemFileLocation = String.Empty;
            String awsS3FileLocation = String.Empty;
            if (tempFilePath.IsNullOrEmpty())
            {
                base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                return;
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "Tenant(" + CurrentViewContext.TenantID.ToString() + @")\";

            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);

            foreach (UploadedFile item in uploadControl.UploadedFiles)
            {
                Boolean aWSUseS3 = false;
                ClientSystemDocument clientSystemDocument = new ClientSystemDocument();
                String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                String fileName = "AD_" + Convert.ToString(CurrentViewContext.TenantID) + "_" + date + Path.GetExtension(item.FileName);

                //Save file at temporary location
                String newTempFilePath = Path.Combine(tempFilePath, fileName);
                item.SaveAs(newTempFilePath);

                if (CurrentViewContext.ToSaveUploadedComplianceViewDocuments == null)
                    CurrentViewContext.ToSaveUploadedComplianceViewDocuments = new List<ClientSystemDocument>();

                #region CHECK WHETHER USE S3 or NOT
                if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                {
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                }
                #endregion

                String destFilePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
                //IF Amazon S3 is false then:
                if (aWSUseS3 == false)
                {
                    if (destFilePath.IsNullOrEmpty())
                    {
                        base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                    }
                    if (!destFilePath.EndsWith(@"\"))
                    {
                        destFilePath += @"\";
                    }
                    destFilePath += "Tenant(" + CurrentViewContext.TenantID.ToString() + @")\";

                    if (!Directory.Exists(destFilePath))
                        Directory.CreateDirectory(destFilePath);
                }
                //destFilePath = destFilePath + fileName;

                //String filePath = CommonFileManager.SaveDocument(newTempFilePath, destFilePath, FileType.ApplicantFileLocation.GetStringValue());

                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    //Move file to other location
                    fileSystemFileLocation = Path.Combine(destFilePath, fileName);
                    File.Copy(newTempFilePath, fileSystemFileLocation);
                }
                else
                {
                    //AWS code to save document to S3 location
                    AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                    String destFolder = destFilePath + "Tenant(" + CurrentViewContext.TenantID.ToString() + ")";
                    awsS3FileLocation = objAmazonS3.SaveDocument(newTempFilePath, fileName, destFolder);
                    
                }


                //Update the list with update document path based on saving location.
                if (aWSUseS3 == false)
                    clientSystemDocument.CSD_DocumentPath = fileSystemFileLocation;
                else
                    clientSystemDocument.CSD_DocumentPath = awsS3FileLocation;

                clientSystemDocument.CSD_FileName = item.FileName;
                clientSystemDocument.CSD_Size = item.ContentLength;
                clientSystemDocument.CSD_Description = item.GetFieldValue("TextBox");
                clientSystemDocument.CSD_CreatedByID = CurrentUserId;
                clientSystemDocument.CSD_CreatedOn = DateTime.Now;
                clientSystemDocument.CSD_IsDeleted = false;

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
                        clientSystemDocument.DocumentFieldMappings.Add(new DocumentFieldMapping
                            {
                                DFM_FieldName = acroField.Key,
                                DFM_DocumentFieldTypeID = null,
                                DFM_IsDeleted = false,
                                DFM_CreatedBy = CurrentUserId,
                                DFM_CreatedOn = DateTime.Now,
                            });
                    }
                }
                stamper.Close();
                pdfReader.Close();
                fileStream.Close();

                CurrentViewContext.ToSaveUploadedComplianceViewDocuments.Add(clientSystemDocument);

                try
                {
                    if (!String.IsNullOrEmpty(newTempFilePath))
                        File.Delete(newTempFilePath);
                    if (!String.IsNullOrEmpty(fileStreamPath))
                        File.Delete(fileStreamPath);
                }
                catch (Exception) { }
            }
            if (CurrentViewContext.ToSaveUploadedComplianceViewDocuments != null && CurrentViewContext.ToSaveUploadedComplianceViewDocuments.Count > 0)
            {
                if (Presenter.AddApplicantUploadedDocuments())
                {
                    base.ShowSuccessMessage("Attribute Document Saved Sucessfully.");
                }
                else
                {
                    base.ShowErrorMessage("Attribute Document cannot be Saved.");
                }
            }
            if (OnCompletedUpload != null)
                OnCompletedUpload();
        }

        protected void btnUploadCancel_Click(object sender, EventArgs e)
        {
            uploadControl.UploadedFiles.Clear();
            HideButton();
        }

        protected void btnUploadAll_Click(object sender, EventArgs e)
        {
            if (CurrentViewContext.TenantID > AppConsts.NONE)
            {
                UploadAllDocuments();
                HideButton();
            }
            else
            {
                base.ShowInfoMessage("Please select institution.");
            }
        }

        private void HideButton()
        {
            btnUploadCancel.Style.Add("display", "none");
            btnUploadAll.Style.Add("display", "none");
        }

    }
}

