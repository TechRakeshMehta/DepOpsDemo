using CoreWeb.AgencyHierarchy.Views;
using CoreWeb.Shell;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.AgencyHierarchy.UserControls
{
    public partial class ManageRequirementApprovalNotificationDocument : BaseUserControl, IRequirementApprovalNotificationDocument
    {
        #region Handlers

        public delegate bool ShowMessageHandler(object sender, StatusMessages messageType, String message);
        public event ShowMessageHandler eventShowMessage;

        #endregion

        #region [Private Variables]

        private RequirementApprovalNotificationDocumentPresenter _presenter = new RequirementApprovalNotificationDocumentPresenter();

        #endregion

        #region Properties

        public RequirementApprovalNotificationDocumentPresenter Presenter
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

        public Int32 AgencyHierarchyID
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

        Int32 IRequirementApprovalNotificationDocument.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public IRequirementApprovalNotificationDocument CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<RequirementApprovalNotificationDocumentContract> ToSaveUploadedDocuments
        {
            get;
            set;
        }

        RequirementApprovalNotificationDocumentContract IRequirementApprovalNotificationDocument.RequirementApprovalNotificationDocument
        {
            get
            {
                if (ViewState["RequirementApprovalNotificationDocument"].IsNullOrEmpty())
                {
                    Presenter.GetMappedRequirementApprovalNotificationDocument();
                }
                return (RequirementApprovalNotificationDocumentContract)ViewState["RequirementApprovalNotificationDocument"];
            }
            set
            {
                ViewState["RequirementApprovalNotificationDocument"] = value;
            }
        }

        #endregion

        #region [Page Event]

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                iFrameViewDoc.Src = "";
                HideButton();
                //fsucCmdBar.SaveButton.ID = "btnUploadAll";
                //fsucCmdBar.CancelButton.ID = "btnUploadCancel";
                ManageUploadSections();
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

        protected void btnUploadAll_Click(object sender, EventArgs e)
        {
            try
            {
                String filePath = String.Empty;
                Boolean aWSUseS3 = false;
                Boolean isCorruptedFileUploaded = false;

                StringBuilder corruptedFileMessage = new StringBuilder();
                String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];

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

                tempFilePath += @"RequirementApprovalNotificationDocument\";

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

                    filePath += @"RequirementApprovalNotificationDocument\";

                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);
                }

                foreach (UploadedFile item in uploadControl.UploadedFiles)
                {
                    RequirementApprovalNotificationDocumentContract approvalNotificationDoc = new RequirementApprovalNotificationDocumentContract();

                    String fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    //Save file
                    String newTempFilePath = Path.Combine(tempFilePath, fileName);
                    item.SaveAs(newTempFilePath);

                    if (ToSaveUploadedDocuments == null)
                        ToSaveUploadedDocuments = new List<RequirementApprovalNotificationDocumentContract>();

                    //Check whether use AWS S3, true if need to use
                    if (aWSUseS3 == false)
                    {
                        //Move file to other location
                        String destFilePath = Path.Combine(filePath, fileName);
                        File.Copy(newTempFilePath, destFilePath);
                        approvalNotificationDoc.DocumentPath = destFilePath;
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
                        String destFolder = filePath; //+ "Tenant(" + tenantID.ToString() + @")/";
                        String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, fileName, destFolder);

                        if (returnFilePath.IsNullOrEmpty())
                        {
                            isCorruptedFileUploaded = true;
                            corruptedFileMessage.Append("Your file " + item.FileName + " is not uploaded. \\n");
                            continue;
                        }
                        approvalNotificationDoc.DocumentPath = returnFilePath; //Path.Combine(destFolder, fileName);
                    }
                    try
                    {
                        if (!String.IsNullOrEmpty(newTempFilePath))
                            File.Delete(newTempFilePath);
                    }
                    catch (Exception) { }


                    approvalNotificationDoc.FileName = item.FileName;
                    approvalNotificationDoc.Size = item.ContentLength;
                    approvalNotificationDoc.Description = Convert.ToString(item.GetFieldValue("TextBox"));
                    approvalNotificationDoc.CreatedBy = CurrentViewContext.CurrentUserId;
                    approvalNotificationDoc.DocumentTypeCode = DocumentType.DOCUMENT_FOR_REQUIREMENT_APPROVAL_NOTIFICATION.GetStringValue();
                    ToSaveUploadedDocuments.Add(approvalNotificationDoc);
                }

                //Code changes for UAT 531- As a student, I should not be able to upload a duplicate document.
                if (!corruptedFileMessage.ToString().IsNullOrEmpty())
                {
                    //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                    if (isCorruptedFileUploaded)
                    {
                        corruptedFileMessage.Append("Please again upload these documents .");
                        eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, corruptedFileMessage.ToString());
                    }
                }

                if (Presenter.SaveApprovalNotificationDocuments())
                {
                    Presenter.GetMappedRequirementApprovalNotificationDocument();
                    ManageUploadSections();
                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Document Uploaded Successfully.");
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

        protected void btnUploadCancel_Click(object sender, EventArgs e)
        {
            try
            {
                HideButton();
                uploadControl.UploadedFiles.Clear();
                ManageUploadSections();
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

        protected void lnkViewDocument_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CurrentViewContext.RequirementApprovalNotificationDocument.IsNullOrEmpty()
                     && CurrentViewContext.RequirementApprovalNotificationDocument.AgencyHierarchySystemDocumentID > 0)
                {
                    string docPath = CurrentViewContext.RequirementApprovalNotificationDocument.DocumentPath;
                    string fileName = CurrentViewContext.RequirementApprovalNotificationDocument.FileName;

                    if (!string.IsNullOrEmpty(fileName))
                        fileName = fileName.Replace("&", string.Empty);
                    iFrameViewDoc.Src = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?FileName={0}&FilePath={1}&IsFileDownloadFromFilePath=true", fileName, docPath, true);
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

        protected void lnkDeleteDocument_Click(object sender, EventArgs e)
        {
            try
            {
                if (Presenter.DeleteApprovalNotificationDocuments())
                {
                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Document Deleted Successfully.");
                    Presenter.GetMappedRequirementApprovalNotificationDocument();
                    HideButton();
                    ManageUploadSections();
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

        #region [Private Methods]

        private void HideButton()
        {
            btnUploadCancel.Style.Add("display", "none");
            btnUploadAll.Style.Add("display", "none");
        }

        private void ManageUploadSections()
        {
            if (!CurrentViewContext.RequirementApprovalNotificationDocument.IsNullOrEmpty() && CurrentViewContext.RequirementApprovalNotificationDocument.AgencyHierarchySystemDocumentID > 0)
            {
                dvUploadDocument.Visible = false;
                dvUploadedDocuments.Visible = true;

                lblDocumentDesc.Text = CurrentViewContext.RequirementApprovalNotificationDocument.Description.HtmlEncode();
                lblDocumentName.Text = CurrentViewContext.RequirementApprovalNotificationDocument.FileName;
                lblDocumentSize.Text = Convert.ToString(CurrentViewContext.RequirementApprovalNotificationDocument.Size/1024);
            }
            else
            {
                dvUploadedDocuments.Visible = false;
                dvUploadDocument.Visible = true;
            }
        }

        #endregion
    }
}