using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using System.Text;
using System.Threading.Tasks;
using CoreWeb.Shell;
using INTSOF.Utils;
using CoreWeb.IntsofSecurityModel;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using CoreWeb.AgencyHierarchy.Pages;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.IO;

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class AgencyNodeMapping : BaseUserControl, IAgencyNodeMappingView
    {
        #region Handlers
        public delegate bool ShowMessageHandler(object sender, StatusMessages messageType, String message);
        public event ShowMessageHandler eventShowMessage;
        public delegate bool ShowHideNodeButtonHandler(object sender, Boolean ShowHideButton);
        public event ShowHideNodeButtonHandler eventShowHideNodeButton;
        #endregion

        #region Private Variables
        private AgencyNodeMappingPresenter _presenter = new AgencyNodeMappingPresenter();
        private Int32 _tenantId = 0;
        #endregion

        #region Properties

        #region Public Properties

        public AgencyNodeMappingPresenter Presenter
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

        public IAgencyNodeMappingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 selectedAgencyID
        {
            get;
            set;
        }

        #endregion

        #region Private Properties
        List<AgencyNodeMappingContract> IAgencyNodeMappingView.lstAgencies
        {
            get;
            set;
        }

        List<AgencyNodeMappingContract> IAgencyNodeMappingView.lstAgencyHierarchyAgencies
        {
            get;
            set;
        }

        Boolean IAgencyNodeMappingView.IsAgencyHierarchyLeafNode
        {
            get;
            set;
        }

        Int32 IAgencyNodeMappingView.AgencyHierarchyID
        {
            get
            {
                return Convert.ToInt32(ViewState["AgencyHierarchyID"]);
            }
            set
            {
                ViewState["AgencyHierarchyID"] = value;
            }
        }

        Int32 IAgencyNodeMappingView.CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        AgencyNodeMappingContract IAgencyNodeMappingView.AgencyNodeMappingContract
        {
            get;
            set;
        }

        List<RequirementApprovalNotificationDocumentContract> IAgencyNodeMappingView.ToSaveUploadedDocuments
        {
            get;
            set;
        }
        public Int32 NodeId { get; set; }

        AgencyHierarchySettingContract IAgencyNodeMappingView.AgencyHierarchySettingContract { get; set; }
        #endregion
        #endregion

        #region Private Methods
        /// <summary>
        /// Method to Bind Agencies
        /// </summary>
        private void BindAgencyDropDown()
        {
            //Presenter.GetAllAgency();
            //cmbAgency.DataSource = CurrentViewContext.LstAgency;
            //cmbAgency.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--", "0"));
            //cmbAgency.DataBind();
        }
        /// <summary>
        /// Method to get current agency mapping
        /// </summary>

        private void BindCurrentAgencyMapping()
        {
            //Presenter.GetAgencyHierarchyAgencyMapping();
            //if (CurrentViewContext.AgencyNodeMappingContract.AgencyID > AppConsts.NONE)
            //{
            //    cmbAgency.SelectedValue = Convert.ToString(CurrentViewContext.AgencyNodeMappingContract.AgencyID);
            //    btnDeleteMapping.Visible = true;
            //}
        }
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!Page.IsPostBack)
                {
                    CurrentViewContext.AgencyHierarchyID = NodeId;
                    Presenter.CheckForLeafNode();
                    if (CurrentViewContext.IsAgencyHierarchyLeafNode)
                    {
                        dvAgencyHierarchyAgency.Style.Add("display", "block");
                    }
                    else
                        dvAgencyHierarchyAgency.Style.Add("display", "none");
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

        #region Old Code
        //protected void btnMapAgency_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        cmbAgency.Enabled = true;
        //        btnSave.Visible = true;
        //        btnMapAgency.Visible = false;
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        eventShowMessage(sender,StatusMessages.ERROR_MESSAGE, ex.Message);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        base.LogError(ex);
        //        eventShowMessage(sender,StatusMessages.ERROR_MESSAGE, ex.Message);
        //    }
        //}

        //protected void btnDeleteMapping_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbAgency.SelectedValue != AppConsts.NONE.ToString())
        //        {
        //            CurrentViewContext.AgencyHierarchyId = CurrentViewContext.AgencyHierarchyId;
        //            CurrentViewContext.AgencyID = Convert.ToInt32(cmbAgency.SelectedValue);
        //            Presenter.DeleteAgencyNodeMapping();
        //            BindCurrentAgencyMapping();
        //            eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Agency mapping removed successfully.");
        //            //cmbAgency.Enabled = true;
        //            //cmbAgency.SelectedValue = AppConsts.ZERO;
        //            cmbAgency.ClearSelection();
        //            btnDeleteMapping.Visible = false;
        //            btnMapAgency.Visible = true;
        //        }
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        eventShowMessage(sender,StatusMessages.ERROR_MESSAGE, ex.Message);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        base.LogError(ex);
        //        eventShowMessage(sender,StatusMessages.ERROR_MESSAGE, ex.Message);
        //    }
        //}

        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        CurrentViewContext.AgencyHierarchyId = CurrentViewContext.AgencyHierarchyId;
        //        CurrentViewContext.SelectedAgencyIDs = cmbAgency.CheckedItems.Select(col => Int32.Parse(col.Value)).ToList();
        //        //CurrentViewContext.AgencyID = Convert.ToInt32(cmbAgency.SelectedValue);
        //        Presenter.MapAgencyOnNode();
        //        btnMapAgency.Visible = true;

        //        if (CurrentViewContext.ReturnValue == AppConsts.ONE)
        //        {
        //            BindCurrentAgencyMapping();
        //            eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Agency mapped successfully.");
        //            cmbAgency.Enabled = false;
        //            btnSave.Visible = false;
        //        }
        //        else if (CurrentViewContext.ReturnValue == AppConsts.TWO)
        //        {
        //            eventShowMessage(sender, StatusMessages.INFO_MESSAGE, "Selected agency already mapped.");
        //            cmbAgency.Enabled = true;
        //            btnSave.Visible = true;
        //            btnMapAgency.Visible = false;
        //        }
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        eventShowMessage(sender,StatusMessages.ERROR_MESSAGE, ex.Message);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        base.LogError(ex);
        //        eventShowMessage(sender,StatusMessages.ERROR_MESSAGE, ex.Message);
        //    }
        //}
        #endregion

        #endregion

        #region Control Events

        #region Grid Events
        protected void grdAgencyHirarchyAgency_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetAgencyHirarchyAgencies();
                grdAgencyHirarchyAgency.DataSource = CurrentViewContext.lstAgencyHierarchyAgencies;

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

        protected void grdAgencyHirarchyAgency_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox cmbAgency = editform.FindControl("cmbAgency") as WclComboBox;

                    AgencyNodeMappingContract agencyNodeMappingContract = e.Item.DataItem as AgencyNodeMappingContract;

                    if (!agencyNodeMappingContract.IsNullOrEmpty())
                    {
                        //cmbAgency.SelectedValue = agencyNodeMappingContract.AgencyID.ToString();
                    }
                    BindAgenciesForAddEditForm(cmbAgency, agencyNodeMappingContract);
                }

                if (e.Item.IsInEditMode)
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    AgencyNodeMappingContract agencyNodeMappingContract = e.Item.DataItem as AgencyNodeMappingContract;
                    if (!agencyNodeMappingContract.IsNullOrEmpty())
                    {
                        RadioButtonList rbl = editform.FindControl("rbtnAttestationFormSettings") as RadioButtonList;
                        rbl.SelectedValue = agencyNodeMappingContract.AttestationformSettingValue.IsNullOrEmpty() ? "D" : agencyNodeMappingContract.AttestationformSettingValue.ToString();
                        Boolean IsDocumentAvailable = agencyNodeMappingContract.AttestationDocumentID > AppConsts.NONE ? true : false;
                        if (IsDocumentAvailable)
                        {
                            Label lblDocumentName = editform.FindControl("lblDocumentName") as Label;
                            lblDocumentName.Text = agencyNodeMappingContract.AttestationFileName;
                            selectedAgencyID = agencyNodeMappingContract.AgencyID;
                        }
                        ManageUploadSections(IsDocumentAvailable, editform);
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

        protected void grdAgencyHirarchyAgency_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {

        }

        protected void grdAgencyHirarchyAgency_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                AgencyNodeMappingContract agencyHierarchyAgencyContract = new AgencyNodeMappingContract();
                agencyHierarchyAgencyContract.CurrentLoggedInUserID = CurrentViewContext.CurrentLoggedInUserID;
                agencyHierarchyAgencyContract.AgencyHierarchyAgencyID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyHierarchyAgencyID"]);
                agencyHierarchyAgencyContract.AgencyHierarchyId = CurrentViewContext.AgencyHierarchyID;
                CurrentViewContext.AgencyNodeMappingContract = agencyHierarchyAgencyContract;

                if (Presenter.DeleteAgencyHierarchyAgencyMapping())
                {
                    //UAT 2821 
                    Int32 agencyID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyID"]);
                    Presenter.DeleteAgencyHierarchySetting(agencyID);
                    Presenter.DeleteAttestationFormDocuments(agencyID);

                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Agency Deleted Successfully.");
                    eventShowHideNodeButton(sender, true);
                }
                else
                    eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Error while agency un-mapping with selected node.");
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

        protected void grdAgencyHirarchyAgency_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "PerformInsert")
                {
                    WclComboBox cmbAgency = e.Item.FindControl("cmbAgency") as WclComboBox;
                    AgencyNodeMappingContract agencyNodeMappingContract = new AgencyNodeMappingContract();
                    agencyNodeMappingContract.CurrentLoggedInUserID = CurrentViewContext.CurrentLoggedInUserID;
                    agencyNodeMappingContract.AgencyHierarchyId = CurrentViewContext.AgencyHierarchyID;
                    if (cmbAgency.IsNotNull())
                    {
                        List<Int32> selectedAgenciesIds = new List<Int32>();
                        if (cmbAgency.CheckedItems.Count > 0)
                        {
                            foreach (var agencyId in cmbAgency.CheckedItems)
                            {
                                selectedAgenciesIds.Add(Convert.ToInt32(agencyId.Value));
                            }
                        }
                        if (!selectedAgenciesIds.IsNullOrEmpty())
                        {
                            agencyNodeMappingContract.SelectedAgencyIDs = selectedAgenciesIds;
                            CurrentViewContext.AgencyNodeMappingContract = agencyNodeMappingContract;
                            if (Presenter.SaveAgencyHierarchyAgencyMapping())
                            {
                                eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Agency Successfully Added.");
                                eventShowHideNodeButton(sender, true);
                            }
                            else
                                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Error while adding agency to selected node.");
                        }
                        else
                        {
                            Label lblError = e.Item.FindControl("lblError") as Label;
                            lblError.Style["display"] = "block";
                            e.Canceled = true;
                        }
                    }
                }

                if (e.CommandName == RadGrid.UpdateCommandName)
                {
                    RadioButtonList rbtnAttestationFormSetting = e.Item.FindControl("rbtnAttestationFormSettings") as RadioButtonList;
                    String SettingValue = rbtnAttestationFormSetting.SelectedValue.ToString();

                    var agencyHierarchySettingContract = new AgencyHierarchySettingContract();
                    agencyHierarchySettingContract.CheckParentSetting = SettingValue == "D" ? true : false;
                    agencyHierarchySettingContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyID;
                    agencyHierarchySettingContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserID;
                    agencyHierarchySettingContract.AgencyID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyID"]);
                    agencyHierarchySettingContract.SettingTypeCode = AgencyHierarchySettingType.SPECIFIC_ATTESTATION_FORM.GetStringValue();
                    agencyHierarchySettingContract.SettingValue = SettingValue;
                    CurrentViewContext.AgencyHierarchySettingContract = agencyHierarchySettingContract;

                    #region UploadDocument

                    WclAsyncUpload uploadControl = e.Item.FindControl("uploadAttestationControl") as WclAsyncUpload;

                    if (uploadControl.UploadedFiles.Count > AppConsts.NONE)
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

                        tempFilePath += @"AttestationForm\";

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

                            filePath += @"AttestationForm\";

                            if (!Directory.Exists(filePath))
                                Directory.CreateDirectory(filePath);
                        }

                        foreach (UploadedFile item in uploadControl.UploadedFiles)
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
                            attestationFormDoc.CreatedBy = CurrentViewContext.CurrentLoggedInUserID;
                            attestationFormDoc.DocumentTypeCode = DocumentType.ATTESTATION_FORM.GetStringValue();
                            attestationFormDoc.AgencyID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyID"]);
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

                    #endregion

                    if (Presenter.SaveAttestationForm())
                    {
                        if (CurrentViewContext.ToSaveUploadedDocuments.IsNullOrEmpty() || CurrentViewContext.ToSaveUploadedDocuments.Count == AppConsts.NONE)
                        {
                            eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Attestation Setting Saved Successfully.");
                        }
                        else
                        {
                            eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Attestation Form Uploaded Successfully.");
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

        #region Radio Button event

        //protected void rbtnAttestationFormSetting_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 SelectedValue = Convert.ToInt32((sender as RadioButtonList).SelectedValue);
        //    GridEditFormItem insertItem = (sender as RadioButtonList).NamingContainer as GridEditFormItem;
        //    HtmlGenericControl dvAttestationFormUpload = insertItem.FindControl("dvAttestationFormUpload") as HtmlGenericControl;
        //    if (SelectedValue == AppConsts.ONE)
        //    {
        //        dvAttestationFormUpload.Visible = true;
        //    }
        //    else
        //    {
        //        dvAttestationFormUpload.Visible = false;
        //    }
        //}

        #endregion

        #endregion

        #region Methods
        #region [Private Methods]
        private void BindAgenciesForAddEditForm(RadComboBox cmbAgencyOnAddEditForm, AgencyNodeMappingContract agencyNodeMappingContract)
        {
            Presenter.GetAllAgency(agencyNodeMappingContract);
            cmbAgencyOnAddEditForm.DataSource = CurrentViewContext.lstAgencies;
            cmbAgencyOnAddEditForm.DataBind();
            // cmbAgencyOnAddEditForm.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
            cmbAgencyOnAddEditForm.Focus();
        }

        private void ManageUploadSections(Boolean IsDocumentAlreadyUploaded, GridEditFormItem editItems)
        {

            HtmlGenericControl dvAttestationFormUpload = editItems.FindControl("dvAttestationFormUpload") as HtmlGenericControl;
            HtmlGenericControl dvUploadedDocuments = editItems.FindControl("dvUploadedDocuments") as HtmlGenericControl;

            if (IsDocumentAlreadyUploaded)
            {
                dvAttestationFormUpload.Visible = false;
                dvUploadedDocuments.Visible = true;
            }
            else
            {
                dvAttestationFormUpload.Visible = true;
                dvUploadedDocuments.Visible = false;
            }
        }

        #endregion

        public void ResetControl(Int32 seletedNodeID)
        {
            CurrentViewContext.AgencyHierarchyID = seletedNodeID;
            Presenter.CheckForLeafNode();
            if (CurrentViewContext.IsAgencyHierarchyLeafNode)
            {
                dvAgencyHierarchyAgency.Style.Add("display", "block");
            }
            else
                dvAgencyHierarchyAgency.Style.Add("display", "none");
        }
        #endregion

        protected void lnkViewDocument_Click(object sender, EventArgs e)
        {
            try
            {
                GridEditFormItem insertItem = (sender as LinkButton).NamingContainer as GridEditFormItem;
                HiddenField Docpath = insertItem.FindControl("hdnDocPath") as HiddenField;
                Label lblfileName = insertItem.FindControl("lblDocumentName") as Label;
                if (!Docpath.Value.IsNullOrEmpty()
                    && !lblfileName.Text.IsNullOrEmpty())
                {
                    String docPath = Docpath.Value;
                    String fileName = lblfileName.Text;

                    if (!string.IsNullOrEmpty(fileName))
                        fileName = fileName.Replace("&", string.Empty);
                    HtmlIframe iFrameViewDoc = insertItem.FindControl("iFrameViewDocs") as HtmlIframe;
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
                GridEditFormItem insertItem = (sender as LinkButton).NamingContainer as GridEditFormItem;
                Int32 agencyID = Convert.ToInt32(insertItem.OwnerTableView.DataKeyValues[insertItem.ItemIndex]["AgencyID"]);
                if (Presenter.DeleteAttestationFormDocuments(agencyID))
                {
                    RadioButtonList rbtnAttestationFormSetting = insertItem.FindControl("rbtnAttestationFormSettings") as RadioButtonList;
                    String SettingValue = rbtnAttestationFormSetting.SelectedValue.ToString();
                    var agencyHierarchySettingContract = new AgencyHierarchySettingContract();
                    agencyHierarchySettingContract.CheckParentSetting = SettingValue == "D" ? true : false;
                    agencyHierarchySettingContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyID;
                    agencyHierarchySettingContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserID;
                    agencyHierarchySettingContract.AgencyID = agencyID;
                    agencyHierarchySettingContract.SettingTypeCode = AgencyHierarchySettingType.SPECIFIC_ATTESTATION_FORM.GetStringValue();
                    agencyHierarchySettingContract.SettingValue = SettingValue == "Y" ? "N" : SettingValue;
                    CurrentViewContext.AgencyHierarchySettingContract = agencyHierarchySettingContract;
                    Presenter.SaveAttestationForm();
                    rbtnAttestationFormSetting.SelectedValue = SettingValue == "Y" ? "N" : SettingValue;
                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "Document Deleted Successfully.");
                    //Presenter.GetMappedRequirementApprovalNotificationDocument();
                    ManageUploadSections(false, insertItem);
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
    }
}