using CoreWeb.ClinicalRotation.Views;
using CoreWeb.ComplianceOperations.Views;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.ClinicalRotation.UserControl
{
    public partial class ManageInstructorPreceptorDocuments : BaseUserControl, IManageInstructorPreceptorDocumentsView
    {
        #region VARIABLES

        #region PRIVATE VARIABLES
        private String _viewType;
        private ManageInstructorPreceptorDocumentsPresenter _presenter = new ManageInstructorPreceptorDocumentsPresenter();

        #endregion

        #endregion

        #region [Properties]

        public ManageInstructorPreceptorDocumentsPresenter Presenter
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

        public IManageInstructorPreceptorDocumentsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        List<TenantDetailContract> IManageInstructorPreceptorDocumentsView.LstTenant
        {
            get;
            set;
        }

        String IManageInstructorPreceptorDocumentsView.ClientContactEmailID
        {
            get
            {
                return base.SysXMembershipUser.Email;
            }
        }

        Int32 IManageInstructorPreceptorDocumentsView.SelectedTenantID
        {
            get
            {
                if (String.IsNullOrEmpty(cmbTenant.SelectedValue))
                    return 0;
                return Convert.ToInt32(cmbTenant.SelectedValue);
            }
            set
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

        List<ApplicantDocumentDetails> IManageInstructorPreceptorDocumentsView.ApplicantUploadedDocuments
        {
            get;
            set;
        }

        Boolean IManageInstructorPreceptorDocumentsView.IsRequirementFieldUploadDocument
        {
            get;
            set;
        }

        Int32 IManageInstructorPreceptorDocumentsView.CurrentLoggedInUserID
        {
            get
            {
                return base.SysXMembershipUser.OrganizationUserId;
            }
        }

        Int32 IManageInstructorPreceptorDocumentsView.ApplicantUploadedDocumentID
        {
            get;
            set;
        }

        List<Int32> IManageInstructorPreceptorDocumentsView.DocumentIdsToPrint
        {
            get
            {
                if (!ViewState["SelectedDocuments"].IsNull())
                {
                    return ViewState["SelectedDocuments"] as List<Int32>;
                }

                return new List<Int32>();
            }
            set
            {
                ViewState["SelectedDocuments"] = value;
            }
        }

        ApplicantDocument toUpdateUploadedDocument = new ApplicantDocument();
        public ApplicantDocument ToUpdateUploadedDocument
        {
            get { return toUpdateUploadedDocument; }
            set { toUpdateUploadedDocument = value; }
        }

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

        public String ErrorMessage
        {
            get;
            set;
        }

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID] == null ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
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

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                HandleTabs(false);
                Presenter.GetTenants();
                cmbTenant.DataSource = CurrentViewContext.LstTenant;
                cmbTenant.DataBind();
            }
            ucUploadDocuments.TenantID = CurrentViewContext.SelectedTenantID;
            ucUploadDocuments.OnCompletedUpload -= new UploadDocuments.UploadDelegate(ucUploadDocuments_OnCompletedUpload);
            ucUploadDocuments.OnCompletedUpload += new UploadDocuments.UploadDelegate(ucUploadDocuments_OnCompletedUpload);
        }

        #endregion

        #region Grid Related Events

        void ucUploadDocuments_OnCompletedUpload()
        {
            grdMapping.Rebind();
        }

        protected void grdMapping_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {

            Presenter.GetApplicantUploadedDocuments();
            grdMapping.DataSource = CurrentViewContext.ApplicantUploadedDocuments;

        }

        /// <summary>
        /// Performs an delete operation for uploaded documents.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdMapping_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem gridEditableItem = e.Item as GridEditableItem;
            CurrentViewContext.ApplicantUploadedDocumentID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ApplicantDocumentID"));
            string documentTypeCode = Convert.ToString(gridEditableItem.GetDataKeyValue("DocumentTypeCode"));
            CurrentViewContext.IsRequirementFieldUploadDocument = documentTypeCode.Equals(DocumentType.REQUIREMENT_FIELD_UPLOAD_DOCUMENT.GetStringValue()) ? true : false;

            if (Presenter.DeleteApplicantUploadedDocument())
            {
                if (!String.IsNullOrEmpty(ErrorMessage))
                {
                    base.LogDebug(ErrorMessage);
                }
                base.ShowSuccessMessage("Document deleted successfully.");
            }
            else
            {
                base.ShowInfoMessage("Document cannot be deleted as it is already mapped.");
            }
        }

        /// <summary>
        /// Performs an delete operation for uploaded documents.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdMapping_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem gridEditableItem = e.Item as GridEditableItem;
            CurrentViewContext.ApplicantUploadedDocumentID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ApplicantDocumentID"));

            String description = (e.Item.FindControl("txtDescription") as WclTextBox).Text;

            ToUpdateUploadedDocument.ApplicantDocumentID = CurrentViewContext.ApplicantUploadedDocumentID;
            ToUpdateUploadedDocument.Description = description;
            ToUpdateUploadedDocument.ModifiedByID = CurrentViewContext.CurrentLoggedInUserID;
            ToUpdateUploadedDocument.ModifiedOn = DateTime.Now;

            if (Presenter.UpdateApplicantUploadedDocument())
            {
                base.ShowSuccessMessage("Document updated successfully.");
            }
            else
            {
                base.ShowErrorMessage("Document cannot be updated.");
            }

        }

        protected void grdMapping_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                ApplicantDocumentDetails docs = e.Item.DataItem as ApplicantDocumentDetails;
                if (!docs.DocumentPath.IsNullOrEmpty())
                {
                    String fileName = docs.DocumentPath.Substring(docs.DocumentPath.LastIndexOf("/") >= 0 ? docs.DocumentPath.LastIndexOf("/") : 0).Remove("/");
                    HtmlAnchor anchor = (HtmlAnchor)e.Item.FindControl("ancManageDocument");

                    anchor.HRef = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?documentId={0}&tenantId={1}&IsPDFFileDownload={2}", docs.ApplicantDocumentID, CurrentViewContext.SelectedTenantID, true);
                }
                //hide the delete and edit button for the E signed documents 
                if (!docs.DocumentTypeCode.IsNullOrEmpty())
                {
                    String documentTypeCode = docs.DocumentTypeCode;
                    if (documentTypeCode.Equals(DocumentType.DisclaimerDocument.GetStringValue()) || documentTypeCode.Equals(DocumentType.DisclosureDocument.GetStringValue())
                        || documentTypeCode.Equals(DocumentType.EDS_AuthorizationForm.GetStringValue())
                        || documentTypeCode.Equals(DocumentType.Disclosure_n_Release.GetStringValue())
                        || documentTypeCode.Equals(DocumentType.Reciept_Document.GetStringValue())
                        || documentTypeCode.Equals(DocumentType.REQUIREMENT_FIELD_UPLOAD_DOCUMENT.GetStringValue())
                        || documentTypeCode.Equals(DocumentType.REQUIREMENT_FIELD_VIEW_DOCUMENT.GetStringValue())
                        || documentTypeCode.Equals(DocumentType.ROTATION_SYLLABUS.GetStringValue())//UAT 1035 && UAT 1316
                        || documentTypeCode.Equals(DocumentType.ADDITIONAL_DOCUMENTS.GetStringValue())//UAT-1560:WB: We should be able to add documents that need to be signed to the order process
                        )
                    {
                        if (!documentTypeCode.Equals(DocumentType.REQUIREMENT_FIELD_UPLOAD_DOCUMENT.GetStringValue()) && !documentTypeCode.Equals(DocumentType.REQUIREMENT_FIELD_VIEW_DOCUMENT.GetStringValue())) //UAT-5062
                        {
                            (e.Item as GridEditableItem)["DeleteColumn"].Controls[AppConsts.NONE].Visible = false;
                            (e.Item as GridEditableItem)["DeleteColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);

                            (e.Item as GridEditableItem)["EditCommandColumn"].Controls[AppConsts.NONE].Visible = false;
                            (e.Item as GridEditableItem)["EditCommandColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                        }
                    }
                }

                #region UAT-1544: WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.
                List<Int32> selectedDocumentIDList = CurrentViewContext.DocumentIdsToPrint;

                if (selectedDocumentIDList.IsNotNull() && docs.ApplicantDocumentID != AppConsts.NONE)
                {
                    if (selectedDocumentIDList.Contains(docs.ApplicantDocumentID))
                    {
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectDocument"));
                        checkBox.Checked = true;
                    }
                }
                #endregion
            }
            #region UAT-1544: WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.
            if (e.Item.ItemType.Equals(GridItemType.Footer))
            {
                Int32 rowCount = grdMapping.Items.Count;
                if (rowCount > 0)
                {
                    Int32 checkCount = 0;
                    foreach (GridDataItem item in grdMapping.Items)
                    {
                        CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectDocument"));
                        if (checkBox.Checked)
                        {
                            checkCount++;
                        }
                    }
                    if (rowCount == checkCount)
                    {
                        GridHeaderItem item = grdMapping.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                        CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectAll"));
                        checkBox.Checked = true;
                    }
                }
            }
            #endregion

            #region UAT-2296

            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                GridEditableItem editItem = e.Item as GridEditableItem;
                RadComboBox combo = (RadComboBox)editItem["ManageDocument"].FindControl("cmbItems");
                combo.Visible = false;

            }

            #endregion
        }

        protected void grdMapping_ItemCommand(object sender, GridCommandEventArgs e)
        {
            //Hide filter when exportig to pdf or word
            if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            {
                base.ConfigureExport(grdMapping);

            }
        }


        #endregion
        #endregion

        protected void tabDocuments_Click(object sender, EventArgs e)
        {
            HandleTabs(false);
        }

        protected void tabIPDocuments_ServerClick(object sender, EventArgs e)
        {
            HandleTabs(true);
            ucSharedUserDocuments.Visiblity = Visiblity;
            ucSharedUserDocuments.IsFalsePostBack = IsFalsePostBack;
        }

        private void HandleTabs(Boolean IsPersonalDocument)
        {
            dvUploadSharedUserPersonalDocuments.Visible = false;
            dvManageUploadDocument.Visible = false;
            liDocuments.Attributes.Remove("class");
            liIPDocuments.Attributes.Remove("class");
            if (IsPersonalDocument)
            {
                dvUploadSharedUserPersonalDocuments.Visible = true;
                liIPDocuments.Attributes.Add("class", "active");
                ReloadUploadDocumnts();
               
            }
            else
            {
                dvManageUploadDocument.Visible = true;
                ucUploadDocuments.IsInstructorPreceptorDocumentScreen = true;
                liDocuments.Attributes.Add("class", "active");
                if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
                {
                    dvDocumentsGrid.Style.Add("display", "block");
                }
                else
                {
                    dvDocumentsGrid.Style.Add("display", "none");
                }
                ucSharedUserDocuments.ReloadPage();
            }
        }

        protected void chkSelectDocument_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                if (checkBox.IsNull())
                {
                    return;
                }
                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                List<Int32> documentIdList = CurrentViewContext.DocumentIdsToPrint;
                Int32 applicantDocumentID = (Int32)dataItem.GetDataKeyValue("ApplicantDocumentID");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectDocument")).Checked;

                if (documentIdList.IsNotNull() && !documentIdList.Contains(applicantDocumentID) && isChecked)
                {
                    documentIdList.Add(applicantDocumentID);
                }
                else if (documentIdList.IsNotNull() && documentIdList.Contains(applicantDocumentID) && !isChecked)
                {
                    documentIdList.Remove(applicantDocumentID);
                }

                CurrentViewContext.DocumentIdsToPrint = documentIdList;
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

        protected void cmbItems_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            if (e.Item.Value.Contains("_-1"))
            {
                e.Item.IsSeparator = true;
                e.Item.CssClass = "Category";
                e.Item.Enabled = false;
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.DocumentIdsToPrint != null && CurrentViewContext.DocumentIdsToPrint.Count > 0)
                {
                    PrintDocument();
                }
                else
                {
                    base.ShowInfoMessage("Please select document(s) to print.");
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

        protected void cmbTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            CurrentViewContext.SelectedTenantID = String.IsNullOrEmpty(cmbTenant.SelectedValue) ? 0 : Convert.ToInt32(cmbTenant.SelectedValue);
            if (CurrentViewContext.SelectedTenantID > AppConsts.NONE)
            {
                hfTenantId.Value = Convert.ToString(CurrentViewContext.SelectedTenantID);
                dvDocumentsGrid.Style.Add("display", "block");
                grdMapping.Rebind();
            }
            else
            {
                hfTenantId.Value = String.Empty;
                dvDocumentsGrid.Style.Add("display", "none");
            }
        }

        protected void cmbTenant_DataBound(object sender, EventArgs e)
        {
            cmbTenant.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        private void PrintDocument()
        {
            if (CurrentViewContext.DocumentIdsToPrint.IsNotNull() && CurrentViewContext.DocumentIdsToPrint.Count > 0)
            {
                String printDocumentPath = String.Empty;
                printDocumentPath = Presenter.ConvertDocumentToPdfForPrint();
                if (printDocumentPath.IsNullOrEmpty())
                {
                    base.ShowInfoMessage("No document(s) found to print.");
                }
                else
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    {"PrintDocumentPath", printDocumentPath},
                                                                    {"DocumentType",AppConsts.APPLICANT_PRINT_DOCUMENT_TYPE}
                                                                 };
                    string url = String.Format(@"~/ComplianceOperations/Pages/FormViewer.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    hdnPrintDocumentURL.Value = url;
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenDocumentInPopup", "openApplicantDocumentToPrint();", true);
                }
            }
            else
            {
                base.ShowInfoMessage("Please select document(s) to print.");
            }
        }

        private void ReloadUploadDocumnts()
        {
            cmbTenant.ClearSelection();
            //ucUploadDocuments.Visible
            grdMapping.Rebind();
        }
    }
}