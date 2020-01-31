
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using CoreWeb.Shell;
using System.Text;
using System.Web.Configuration;
using System.IO;
using Telerik.Web.UI;

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class ManageAttestationFormDocument : BaseUserControl, IAttestationFormDocument
    {
        #region Handlers
        public delegate bool ShowMessageHandler(object sender, StatusMessages messageType, String message);
        public event ShowMessageHandler eventShowMessage;
        #endregion

        #region Private Variable
        private AttestationFormDocumentPresenter _presenter = new AttestationFormDocumentPresenter();

        #endregion

        #region Properties
        public AttestationFormDocumentPresenter Presenter
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

        public IAttestationFormDocument CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IAttestationFormDocument.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        List<RequirementApprovalNotificationDocumentContract> IAttestationFormDocument.ToSaveUploadedDocuments
        {
            get;
            set;
        }

        RequirementApprovalNotificationDocumentContract IAttestationFormDocument.AttestationFormDocument
        {
            get
            {
                if (ViewState["AttestationFormDocument"].IsNullOrEmpty())
                {
                    Presenter.GetAttestationFormDocument();
                }
                return (RequirementApprovalNotificationDocumentContract)ViewState["AttestationFormDocument"];
            }
            set
            {
                ViewState["AttestationFormDocument"] = value;
            }
        }

        AgencyHierarchySettingContract IAttestationFormDocument.AgencyHierarchySettingContract
        {
            get;
            set;
        }

        public Int32 SelectedRootNodeID
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedRootNodeID"]);
            }
            set
            {
                ViewState["SelectedRootNodeID"] = value;
            }
        }

        #endregion


        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                iFrameViewDoc.Src = "";
                if (CurrentViewContext.AgencyHierarchyID != AppConsts.NONE)
                {
                    if (SelectedRootNodeID == CurrentViewContext.AgencyHierarchyID)
                    {
                        rbtnAttestationFormSetting.Items.FindByValue("D").Attributes.Add("hidden", "hidden");
                    }
                    ManageUploadSections();
                    if (!IsPostBack)
                    {
                        Presenter.GetAgencyHirarchySettings();

                        if (SelectedRootNodeID == CurrentViewContext.AgencyHierarchyID)
                        {
                            rbtnAttestationFormSetting.SelectedValue = "N";
                        }

                        if (!CurrentViewContext.AgencyHierarchySettingContract.IsNullOrEmpty() && !CurrentViewContext.AgencyHierarchySettingContract.SettingValue.IsNullOrEmpty())
                        {
                            rbtnAttestationFormSetting.SelectedValue = CurrentViewContext.AgencyHierarchySettingContract.SettingValue;
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

        protected void btnUploadAll_Click(object sender, EventArgs e)
        {
            try
            {
                String SettingValue = rbtnAttestationFormSetting.SelectedValue.ToString();
                var agencyHierarchySettingContract = new AgencyHierarchySettingContract();
                agencyHierarchySettingContract.CheckParentSetting = SettingValue == "D" ? true : false;
                agencyHierarchySettingContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyID;
                agencyHierarchySettingContract.CurrentLoggedInUser = CurrentViewContext.CurrentUserId;
                agencyHierarchySettingContract.SettingTypeCode = AgencyHierarchySettingType.SPECIFIC_ATTESTATION_FORM.GetStringValue();
                agencyHierarchySettingContract.SettingValue = SettingValue;
                CurrentViewContext.AgencyHierarchySettingContract = agencyHierarchySettingContract;

                if (uploadAttestationFormControl.UploadedFiles.Count > AppConsts.NONE)
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

                    tempFilePath += @"AttestationFormDocument\";

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

                        filePath += @"AttestationFormDocument\";

                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);
                    }

                    foreach (UploadedFile item in uploadAttestationFormControl.UploadedFiles)
                    {
                        RequirementApprovalNotificationDocumentContract attestationFormDoc = new RequirementApprovalNotificationDocumentContract();

                        String fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                        //Save file
                        String newTempFilePath = Path.Combine(tempFilePath, fileName);
                        item.SaveAs(newTempFilePath);

                        if (CurrentViewContext.ToSaveUploadedDocuments == null)
                            CurrentViewContext.ToSaveUploadedDocuments = new List<RequirementApprovalNotificationDocumentContract>();

                        //Check whether use AWS S3, true if need to use
                        if (aWSUseS3 == false)
                        {
                            //Move file to other location
                            String destFilePath = Path.Combine(filePath, fileName);
                            File.Copy(newTempFilePath, destFilePath);
                            attestationFormDoc.DocumentPath = destFilePath;
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
                            attestationFormDoc.DocumentPath = returnFilePath; //Path.Combine(destFolder, fileName);
                        }
                        try
                        {
                            if (!String.IsNullOrEmpty(newTempFilePath))
                                File.Delete(newTempFilePath);
                        }
                        catch (Exception) { }


                        attestationFormDoc.FileName = item.FileName;
                        attestationFormDoc.Size = item.ContentLength;
                        attestationFormDoc.Description = Convert.ToString(item.GetFieldValue("TextBox"));
                        attestationFormDoc.CreatedBy = CurrentViewContext.CurrentUserId;
                        attestationFormDoc.DocumentTypeCode = DocumentType.ATTESTATION_FORM.GetStringValue();
                        CurrentViewContext.ToSaveUploadedDocuments.Add(attestationFormDoc);
                    }

                    if (!corruptedFileMessage.ToString().IsNullOrEmpty())
                    {
                        if (isCorruptedFileUploaded)
                        {
                            corruptedFileMessage.Append("Please again upload these documents .");
                            eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, corruptedFileMessage.ToString());
                        }
                    }
                }

                if (Presenter.SaveAttestationFormDocuments())
                {
                    Presenter.GetAttestationFormDocument();
                    ManageUploadSections();
                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Attestation Form Uploaded Successfully.");
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

        protected void lnkViewDocument_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CurrentViewContext.AttestationFormDocument.IsNullOrEmpty()
                     && CurrentViewContext.AttestationFormDocument.AgencyHierarchySystemDocumentID > 0)
                {
                    string docPath = CurrentViewContext.AttestationFormDocument.DocumentPath;
                    string fileName = CurrentViewContext.AttestationFormDocument.FileName;

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
                if (Presenter.DeleteAttestationFormDocuments())
                {
                    String SettingValue = rbtnAttestationFormSetting.SelectedValue.ToString();
                    var agencyHierarchySettingContract = new AgencyHierarchySettingContract();
                    agencyHierarchySettingContract.CheckParentSetting = SettingValue == "D" ? true : false;
                    agencyHierarchySettingContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyID;
                    agencyHierarchySettingContract.CurrentLoggedInUser = CurrentViewContext.CurrentUserId;
                    agencyHierarchySettingContract.SettingTypeCode = AgencyHierarchySettingType.SPECIFIC_ATTESTATION_FORM.GetStringValue();
                    agencyHierarchySettingContract.SettingValue = SettingValue == "Y" ? "N" : SettingValue;
                    CurrentViewContext.AgencyHierarchySettingContract = agencyHierarchySettingContract;
                    Presenter.SaveAttestationFormDocuments();
                    rbtnAttestationFormSetting.SelectedValue = SettingValue == "Y" ? "N" : SettingValue;
                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Document Deleted Successfully.");
                    Presenter.GetAttestationFormDocument();
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


        #region Methods

        private void ManageUploadSections()
        {
            if (!CurrentViewContext.AttestationFormDocument.IsNullOrEmpty() && CurrentViewContext.AttestationFormDocument.AgencyHierarchySystemDocumentID > 0)
            {
                dvAttestationUploadDocuments.Visible = false;
                dvAttestationFormUploadedDocuments.Visible = true;
                lblAttestationDocumentName.Text = CurrentViewContext.AttestationFormDocument.FileName;
                lblAttestationDocumentSize.Text = Convert.ToString(CurrentViewContext.AttestationFormDocument.Size/1024);
            }
            else
            {
                dvAttestationFormUploadedDocuments.Visible = false;
                dvAttestationUploadDocuments.Visible = true;
            }
        }

        #endregion
    }
}