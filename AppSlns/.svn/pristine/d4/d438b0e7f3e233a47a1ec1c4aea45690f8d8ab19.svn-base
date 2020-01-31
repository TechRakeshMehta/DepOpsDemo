using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.ClinicalRotation.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ClinicalRotation.UserControl
{
    public partial class ManageSharedUserDocument : BaseUserControl, IManageSharedUserDocumentView
    {

        #region VARIABLES

        #region PRIVATE VARIABLES

        private ManageSharedUserDocumentPresenter _presenter = new ManageSharedUserDocumentPresenter();
        private Int32 _uploadedDocumentFileSize;
        private String _uploadedDocumentFilePath;
        private Int32 tenantId = 0;
        private bool _isCmbInstitutionVisible = false;

        #endregion

        #endregion

        #region [Properties]

        List<SharedSystemDocTypeContract> IManageSharedUserDocumentView.DocumentTypeList
        {
            get
            {
                if (!ViewState["DocumentTypeList"].IsNull())
                {
                    return (List<SharedSystemDocTypeContract>)(ViewState["DocumentTypeList"]);
                }

                return new List<SharedSystemDocTypeContract>();
            }
            set
            {
                ViewState["DocumentTypeList"] = value;
            }
        }

        Int32 IManageSharedUserDocumentView.ClientContactID
        {
            get
            {
                if (!ViewState["ClientContactID"].IsNull())
                {
                    return (Int32)(ViewState["ClientContactID"]);
                }

                return AppConsts.NONE;
            }
            set
            {
                ViewState["ClientContactID"] = value;
            }
        }

        public Int32 SelectedTenantID
        {
            get
            {
                if (!_isCmbInstitutionVisible)
                {
                    if (!ViewState["SelectedTenantID"].IsNull())
                    {
                        return (Convert.ToInt32(ViewState["SelectedTenantID"]));
                    }

                    return 0;
                }
                else
                {
                    if (String.IsNullOrEmpty(cmbTenant.SelectedValue))
                        return 0;
                    return Convert.ToInt32(cmbTenant.SelectedValue);
                }
            }
            set
            {
                if (!_isCmbInstitutionVisible)
                {
                    ViewState["SelectedTenantID"] = value;
                }
                else
                {
                    if (value > 0)
                    {
                        cmbTenant.SelectedValue = Convert.ToString(value);
                    }
                    else
                    {
                        cmbTenant.SelectedIndex = value;
                    }
                }
            }
        }

        public IManageSharedUserDocumentView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public ManageSharedUserDocumentPresenter Presenter
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

        SharedSystemDocumentContract IManageSharedUserDocumentView.UploadedDocument
        {
            get;
            set;
        }

        String IManageSharedUserDocumentView.ClientContactEmailID
        {
            get
            {
                return base.SysXMembershipUser.Email;
            }
            set { }
        }

        List<SharedSystemDocumentContract> IManageSharedUserDocumentView.UploadedDocumentList
        {
            get
            {
                if (!ViewState["UploadedDocumentList"].IsNull())
                {
                    return (List<SharedSystemDocumentContract>)(ViewState["UploadedDocumentList"]);
                }

                return new List<SharedSystemDocumentContract>();
            }
            set
            {
                ViewState["UploadedDocumentList"] = value;
            }
        }

        Boolean IManageSharedUserDocumentView.SuccessMsg
        {
            get;
            set;
        }

        Int32 IManageSharedUserDocumentView.TenantID
        {
            get
            {
                if (tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return tenantId;
            }
            set { tenantId = value; }
        }

        List<TenantDetailContract> IManageSharedUserDocumentView.LstTenant
        {
            get;
            set;
        }

        public string ParentPage { get; set; }

        public Boolean IsFalsePostBack { get; set; }

        public Boolean Visiblity
        {
            get
            {
                if (ViewState["Visiblity"] != null)
                    return (Convert.ToBoolean(ViewState["Visiblity"]));
                return true;
            }
            set
            {
                ViewState["Visiblity"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //change done for Instructor/Preceptor dashboard redesign.
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    _isCmbInstitutionVisible = true;

                    if (ParentPage.IsNotNull() && ParentPage.ToLower() == "editprofile")
                    {
                        _isCmbInstitutionVisible = false;
                    }

                    if (!_isCmbInstitutionVisible)
                        dvInstitution.Visible = _isCmbInstitutionVisible;

                    if (!Page.IsPostBack || IsFalsePostBack)
                    {
                        BindControls();
                        HandleDocumentUploadGridSettings();
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

        protected void cmbDocumentType_DataBound(object sender, EventArgs e)
        {
            WclComboBox cmbDocumentType = sender as WclComboBox;
            cmbDocumentType.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        #region [Grid Events]
        protected void grdUploadDocuments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //change done for Instructor/Preceptor dashboard redesign.
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                    {
                        GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                        //Bind Document Type
                        WclComboBox cmbDocumentType = gridEditableItem.FindControl("cmbDocumentType") as WclComboBox;
                        cmbDocumentType.DataSource = CurrentViewContext.DocumentTypeList;
                        cmbDocumentType.DataBind();
                    }
                    if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                    {
                        LinkButton lnkClientContactDoc = (LinkButton)e.Item.FindControl("lnkClientContactDoc");
                        Int32 doucmentID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DocumentID"]);
                        string url = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?sharedSystemDocumentID={0}", doucmentID);
                        lnkClientContactDoc.OnClientClick = "DownloadForm('" + url + "')";
                        // anchor.HRef = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?documentId={0}&tenantId={1}", applicantDoc.ApplicantDocumentID, CurrentUserTenantId);
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

        protected void grdUploadDocuments_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                //change done for Instructor/Preceptor dashboard redesign.
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    if (e.CommandName.Equals("PerformInsert"))
                    {
                        if (CurrentViewContext.SelectedTenantID > AppConsts.NONE && CurrentViewContext.ClientContactID > AppConsts.NONE)
                        {

                            GridEditFormItem item = e.Item as GridEditFormItem;
                            WclAsyncUpload uploadControl = (WclAsyncUpload)item.FindControl("uploadControl");
                            WclTextBox documentDescription = (WclTextBox)item.FindControl("txtDocDescription");
                            WclComboBox cmbDocumentType = (WclComboBox)item.FindControl("cmbDocumentType");

                            if (uploadControl.UploadedFiles.Count == AppConsts.NONE)
                            {
                                base.ShowInfoMessage(AppConsts.CLIENT_CONTACT_DOC_UPLOADINFOMSG);
                                return;
                            }
                            else
                            {
                                CurrentViewContext.UploadedDocument = new SharedSystemDocumentContract();
                                CurrentViewContext.UploadedDocument.FileName = uploadControl.UploadedFiles[0].FileName;
                                CurrentViewContext.UploadedDocument.Description = documentDescription.Text.Trim();
                                CurrentViewContext.UploadedDocument.DocumentTypeID = Convert.ToInt32(cmbDocumentType.SelectedValue);
                                CurrentViewContext.UploadedDocument.DocumentTypeName = CurrentViewContext.DocumentTypeList.Where(x => x.SharedSystemDocTypeID == CurrentViewContext.UploadedDocument.DocumentTypeID).Select(x => x.Name).FirstOrDefault();

                                //UAT 1426 WB: As an admin or Instructor/preceptor, I should be able to upload multiple licences for an instructor/preceptor
                                //if (IsDocumentIsAllowedToUpload(CurrentViewContext.UploadedDocument))
                                //{
                                //CurrentViewContext.DocumentTypeList.RemoveAll(x => x.SharedSystemDocTypeID == Convert.ToInt32(cmbDocumentType.SelectedValue));

                                //1.Move the uploaded documents to Amazon S3 or FileSystem
                                if (UploadFileToS3OrFileSystem(uploadControl, CurrentViewContext.UploadedDocument.DocumentTypeID))
                                {
                                    CurrentViewContext.UploadedDocument.DocumentPath = _uploadedDocumentFilePath;
                                    CurrentViewContext.UploadedDocument.FileSize = _uploadedDocumentFileSize;
                                }
                                Presenter.SaveUploadedDocument();
                                if (CurrentViewContext.SuccessMsg)
                                    base.ShowSuccessMessage("Document uploaded successfully.");

                                grdUploadDocuments.Rebind();
                                //UAT 1426 WB: As an admin or Instructor/preceptor, I should be able to upload multiple licences for an instructor/preceptor
                                //ShowHideUploadDocumentButton(grdUploadDocuments);
                                //}
                                //else
                                //{
                                //    base.ShowInfoMessage(AppConsts.CLIENT_CONTACT_DOC_ALREADY_UPLOADED);
                                //}

                            }
                        }
                        else
                        {
                            base.ShowInfoMessage("Please select institution.");
                        }
                    }
                    if (e.CommandName.Equals("Delete"))
                    {
                        GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                        Int32 DocumentID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("DocumentID"));
                        Presenter.DeleteUploadedDocument(DocumentID);
                        if (CurrentViewContext.SuccessMsg)
                            base.ShowSuccessMessage("Document deleted successfully.");

                        //UAT 1426 WB: As an admin or Instructor/preceptor, I should be able to upload multiple licences for an instructor/preceptor
                        //SharedSystemDocumentContract currentRecord = CurrentViewContext.UploadedDocumentList.FirstOrDefault(x => x.DocumentID == DocumentID);
                        //SharedSystemDocTypeContract documentToAdd = DocumentTypeListTemp.Where(x => x.SharedSystemDocTypeID == currentRecord.DocumentTypeID).FirstOrDefault();
                        //CurrentViewContext.DocumentTypeList.Add(documentToAdd);

                        grdUploadDocuments.Rebind();
                        //UAT 1426 WB: As an admin or Instructor/preceptor, I should be able to upload multiple licences for an instructor/preceptor
                        //ShowHideUploadDocumentButton(grdUploadDocuments);
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

        protected void grdUploadDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                //change done for Instructor/Preceptor dashboard redesign.
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    Presenter.GetClientContactDocument();
                    grdUploadDocuments.DataSource = CurrentViewContext.UploadedDocumentList;
                    //handle the number of document types available to upload document.
                    //HandleDocumentTypeDropdown();
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

        #region [Private Functions]

        private void BindControls()
        {
            Presenter.GetDocumentType();

            if (_isCmbInstitutionVisible)
            {
                Presenter.GetTenants();
                cmbTenant.DataSource = CurrentViewContext.LstTenant;
                cmbTenant.DataBind();
            }
        }

        /// <summary>
        /// 1. Upload file to Amazon S3 or File System.
        /// 2. Update the UploadedDocumentList with updated path.
        /// </summary>
        /// <returns></returns>
        private Boolean UploadFileToS3OrFileSystem(WclAsyncUpload uploadControl, Int32 documentTypeID)
        {
            #region VARIABLES
            Boolean aWSUseS3 = false;
            StringBuilder corruptedFileMessage = new StringBuilder();
            String filePath = String.Empty;
            String fileSystemFileLocation = String.Empty;
            String awsS3FileLocation = String.Empty;
            #endregion

            #region Temporary Upload Document Settings
            String newTempFilePath = string.Empty;
            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];

            if (tempFilePath.IsNullOrEmpty())
            {
                base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                return false;
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "Tenant(" + CurrentViewContext.TenantID.ToString() + @")\";

            if (!Directory.Exists(tempFilePath))
            {
                Directory.CreateDirectory(tempFilePath);
            }
            #endregion

            #region CHECK WHETHER USE S3 or NOT
            if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
            {
                aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
            }
            #endregion

            filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
            //IF Amazon S3 is false then:
            if (aWSUseS3 == false)
            {
                if (filePath.IsNullOrEmpty())
                {
                    base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                    return false;
                }
                if (!filePath.EndsWith(@"\"))
                {
                    filePath += @"\";
                }
                filePath += "Tenant(" + CurrentViewContext.TenantID.ToString() + @")\";

                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
            }

            try
            {
                foreach (UploadedFile uploadedDocument in uploadControl.UploadedFiles)
                {
                    //String fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadedDocument.FileName);
                    String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                    String fileName = "ClientContact_" + CurrentViewContext.TenantID.ToString() + "_" + documentTypeID.ToString() + "_" + date + Path.GetExtension(uploadedDocument.FileName);

                    //filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];

                    newTempFilePath = Path.Combine(tempFilePath, fileName);
                    uploadedDocument.SaveAs(newTempFilePath);

                    //Get original file bytes and check if same document is already uploaded
                    byte[] fileBytes = File.ReadAllBytes(newTempFilePath);

                    //Check whether use AWS S3, true if need to use
                    if (aWSUseS3 == false)
                    {
                        //Move file to other location
                        fileSystemFileLocation = Path.Combine(filePath, fileName);
                        File.Copy(newTempFilePath, fileSystemFileLocation);
                    }
                    else
                    {
                        //AWS code to save document to S3 location
                        AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                        String destFolder = filePath + "Tenant(" + CurrentViewContext.TenantID.ToString() + ")";
                        awsS3FileLocation = objAmazonS3.SaveDocument(newTempFilePath, fileName, destFolder);
                        if (awsS3FileLocation.IsNullOrEmpty())
                        {
                            corruptedFileMessage.Append("Your file " + fileName + " is not uploaded. \\n");
                        }
                    }

                    //update filesize
                    _uploadedDocumentFileSize = fileBytes.Length;

                    try
                    {
                        if (!String.IsNullOrEmpty(newTempFilePath))
                            File.Delete(newTempFilePath);
                    }
                    catch (Exception) { }


                    //Update the list with update document path based on saving location.
                    if (aWSUseS3 == false)
                        _uploadedDocumentFilePath = fileSystemFileLocation;
                    else
                        _uploadedDocumentFilePath = awsS3FileLocation;
                }
                return true; // Means no execption occured, document should uploaded successfully.
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void HandleDocumentUploadGridSettings()
        {
            if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
            {
                //Get Client Contact ID by Email 
                Presenter.GetClientContactByEmail();
                //Bind Upload Document Grid

                //cmdbar_ClientContactProfile.Visible = true;
                hfTenantId.Value = Convert.ToString(CurrentViewContext.SelectedTenantID);
            }
            else
            {
                //grdUploadDocuments.Visible = false;
                //grdRotationDocuments.Visible = false;
                //cmdbar_ClientContactProfile.Visible = false;
                CurrentViewContext.ClientContactID = AppConsts.NONE; //Reset client contactID
                hfTenantId.Value = String.Empty;
            }
            Presenter.GetClientContactDocument();
            //ShowHideUploadDocumentButton(grdUploadDocuments);
            grdUploadDocuments.DataSource = CurrentViewContext.UploadedDocumentList;
            grdUploadDocuments.DataBind();
        }
        #endregion

        protected void cmbTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            CurrentViewContext.SelectedTenantID = String.IsNullOrEmpty(cmbTenant.SelectedValue) ? 0 : Convert.ToInt32(cmbTenant.SelectedValue);
            HandleDocumentUploadGridSettings();
        }

        protected void cmbTenant_DataBound(object sender, EventArgs e)
        {
            cmbTenant.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        public void ReloadPage()
        {
            cmbTenant.ClearSelection();
            ResetGridFilters();
        }
        private void ResetGridFilters()
        {
            grdUploadDocuments.MasterTableView.SortExpressions.Clear();
            grdUploadDocuments.CurrentPageIndex = 0;
            grdUploadDocuments.MasterTableView.CurrentPageIndex = 0;
            grdUploadDocuments.MasterTableView.IsItemInserted = false;
            grdUploadDocuments.MasterTableView.ClearEditItems();
            grdUploadDocuments.Rebind();
        }


    }
}