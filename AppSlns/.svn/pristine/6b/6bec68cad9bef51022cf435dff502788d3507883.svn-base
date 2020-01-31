using System;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using System.Collections.Generic;
using Telerik.Web.UI;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.IO;
using System.Threading;
using System.Web.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Linq;
using System.Web.UI;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManageUploadDocument : BaseUserControl, IManageUploadDocumentView
    {
        #region Variables

        #region Private Variables
        private String _viewType;
        private ManageUploadDocumentPresenter _presenter = new ManageUploadDocumentPresenter();
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        public ManageUploadDocumentPresenter Presenter
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

        public IManageUploadDocumentView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public String AllowedFileExtension
        {
            get;
            set;
        }

        public List<ApplicantDocument> ToSaveApplicantUploadedDocuments
        {
            get;
            set;
        }
        //Applicantid Set when screen opens from Admin(verification Details And Portfolio Search)
        public Int32 FromAdminApplicantID
        {
            get;
            set;
        }
        //Applicantid Set when screen opens from Admin(verification Details And Portfolio Search)
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
        public String ErrorMessage
        {
            get;
            set;
        }
        public Int32 PackageSubscriptionId
        {
            get { return (Int32)(ViewState["PackageSubscriptionId"]); }
            set { ViewState["PackageSubscriptionId"] = value; }
        }

        public String WorkQueue
        {
            get
            {
                if (ViewState["WorkQueueType"].IsNotNull())
                    return Convert.ToString(ViewState["WorkQueueType"]);
                return String.Empty;
            }
            set
            {
                ViewState["WorkQueueType"] = value;
            }
        }

        public Int32 CurrentUserID
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        public List<ApplicantDocumentDetails> ApplicantUploadedDocuments
        {
            get;
            set;
        }

        public Int32 TenantID
        {
            get;
            set;
        }

        public Int32 ApplicantUploadedDocumentID
        {
            get;
            set;
        }
        public String PageType
        { get; set; }


        public String IsUploadOrViewDocument
        {
            get
            {
                if (ViewState["IsUploadOrViewDocument"].IsNotNull())
                    return Convert.ToString(ViewState["IsUploadOrViewDocument"]);
                return String.Empty;
            }
            set
            {
                ViewState["IsUploadOrViewDocument"] = value;
            }
        }

        public String SelectedComplianceCategoryId_Global
        {
            get
            {
                if (ViewState["SelectedComplianceCategoryId_Global"].IsNotNull())
                    return Convert.ToString(ViewState["SelectedComplianceCategoryId_Global"]);
                return String.Empty;
            }
            set
            {
                ViewState["SelectedComplianceCategoryId_Global"] = value;
            }
        }

        ApplicantDocument toUpdateUploadedDocument = new ApplicantDocument();
        public ApplicantDocument ToUpdateUploadedDocument
        {
            get { return toUpdateUploadedDocument; }
            set { toUpdateUploadedDocument = value; }
        }

        //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
        public Boolean IsFullPermissionForVerification
        {
            get
            {
                return (Boolean)(ViewState["IsFullPermissionForVerification"]);
            }
            set
            {
                ViewState["IsFullPermissionForVerification"] = value;
            }
        }

        /// <summary>
        /// set SelectedPackageId id for return back to queue.
        /// </summary>
        public String SelectedPackageId
        {
            get
            {
                if (ViewState["SelectedPackageId"].IsNotNull())
                    return Convert.ToString(ViewState["SelectedPackageId"]);
                return String.Empty;
            }
            set
            {
                ViewState["SelectedPackageId"] = value;
            }
        }


        /// <summary>
        /// set SelectedItemDataId for return back to queue.
        /// </summary>
        public String SelectedItemDataId
        {
            get
            {
                if (ViewState["SelectedItemDataId"].IsNotNull())
                    return Convert.ToString(ViewState["SelectedItemDataId"]);
                return String.Empty;
            }
            set
            {
                ViewState["SelectedItemDataId"] = value;
            }
        }

        /// <summary>
        /// set ComplianceItemReconciliationDataID for return back to queue.
        /// </summary>
        public String SelectedComplianceItemReconciliationDataID
        {
            get
            {
                if (ViewState["SelectedComplianceItemReconciliationDataID"].IsNotNull())
                    return Convert.ToString(ViewState["SelectedComplianceItemReconciliationDataID"]);
                return String.Empty;
            }
            set
            {
                ViewState["SelectedComplianceItemReconciliationDataID"] = value;
            }
        }


        #region UAT-977: Additional work towards archive ability
        Boolean IManageUploadDocumentView.IsActiveSubscription
        {
            get
            {
                return (Boolean)(ViewState["IsActiveSubscription"]);
            }
            set
            {
                ViewState["IsActiveSubscription"] = value;
            }
        }
        #endregion

        #region UAT-1544: WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.
        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        public List<Int32> DocumentIdsToPrint
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
        #endregion

        public Boolean IsException
        {
            get { return Convert.ToBoolean(ViewState["IsException"]); }
            set { ViewState["IsException"] = value; }
        }

        //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        Int32 IManageUploadDocumentView.OrgUsrID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                }
                else
                {
                    return CurrentUserID;
                }
            }
        }

        //UAT-2296 : Disable already selected item in dropdown for mapping .
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

        //UAT-2296 :Data Entry screen should allow data entry for all items previously selected and the newly selected items. 
        public List<lkpItemDocMappingType> lstItemDocMappingType
        {
            get;
            set;
        }
        //UAT-2296 :Data Entry screen should allow data entry for all items previously selected and the newly selected items. 
        public List<lkpDataEntryDocumentStatu> lkpDataEntryDocumentStatus
        {
            get;
            set;
        }

        public Boolean IsRequirementFieldUploadDocument
        {
            get;
            set;
        }

        //UAT-4067
        public Boolean IsAllowedFileExtensionEnable
        {
            get;
            set;
        }

        //UAT-4687
        Boolean IManageUploadDocumentView.IsApplicantClinicalRotationMember
        {
            get;
            set;
        }


        #endregion

        #region Events

        #region Page Events
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// Calls WebClientApplication.BuildItemWithCurrentContext after the events are handled
        /// to fire the DI engine.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID] == null ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                grdMapping.MasterTableView.PagerStyle.PagerTextFormat = "{4} {5} " + Resources.Language.ITEMS + " " + Resources.Language.IN + " {1} " + Resources.Language.PAGE;
                grdMapping.MasterTableView.PagerStyle.PageSizeLabelText = Resources.Language.PAGESIZE;
                grdMapping.MasterTableView.PagerStyle.NextPagesToolTip = Resources.Language.NXTPAGE;
                grdMapping.MasterTableView.PagerStyle.NextPageToolTip = Resources.Language.NXTPAGE;
                grdMapping.MasterTableView.PagerStyle.PrevPagesToolTip = Resources.Language.PREVPAGE;
                grdMapping.MasterTableView.PagerStyle.PrevPageToolTip = Resources.Language.PREVPAGE;
                grdMapping.MasterTableView.PagerStyle.FirstPageToolTip = Resources.Language.FIRSTPAGE;
                grdMapping.MasterTableView.PagerStyle.LastPageToolTip = Resources.Language.LSTPAGE;
                grdMapping.MasterTableView.PagerStyle.PageSizeLabelText = Resources.Language.PAGESIZE;
                if (this.Visible == false) // this would cover additional cases when the control itself is hidden.
                {
                    ucUploadDocuments.isDropZoneEnabled = false;
                }
                //base.Title = "Manage Documents";
                base.Title = Resources.Language.MANGEDOCS;
                base.BreadCrumbTitleKey = "Key_MANGEDOCS";
                //lblManageTenant.Text = base.Title;
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

        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();

            }

            ifrUnifiedDocument.Src = "";
            Presenter.OnViewLoaded();

            Dictionary<String, String> args = new Dictionary<String, String>();
            if (Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT] != null)
            {
                args.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                if (args.ContainsKey("PageType") && args["PageType"].IsNotNull() && ((args["PageType"] == "VerificationDetail" || args["PageType"] == "PortfolioSearch"
                                   || args["PageType"] == "ApplicantView")))
                {
                    PageType = args["PageType"];
                    if (args.ContainsKey("IsException"))
                    {
                        IsException = Convert.ToBoolean(args["IsException"]);
                    }
                    lnkGoBack.Visible = true;
                    if (PageType == "VerificationDetail")
                    {

                        lnkGoBack.Text = "Back to Verification Details";
                        CurrentViewContext.WorkQueue = Convert.ToString(args["WorkQueueType"]);
                        CurrentViewContext.SelectedComplianceCategoryId_Global = Convert.ToString(args["SelectedComplianceCategoryId_Global"]);
                        CurrentViewContext.SelectedPackageId = Convert.ToString(args["PackageId"]);
                        CurrentViewContext.SelectedItemDataId = Convert.ToString(args["ItemDataId"]);
                        CurrentViewContext.SelectedComplianceItemReconciliationDataID = Convert.ToString(args["ComplianceItemReconciliationDataID"]);
                        //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
                        IsFullPermissionForVerification = Convert.ToBoolean(args["IsFullPermissionForVerification"]);
                    }
                    else if (PageType == "ApplicantView")
                    {
                        lnkGoBack.Text = "Back to Applicant's View";
                        CurrentViewContext.WorkQueue = Convert.ToString(args["WorkQueueType"]);
                        //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
                        IsFullPermissionForVerification = Convert.ToBoolean(args["IsFullPermissionForVerification"]);
                    }
                    else
                    {
                        lnkGoBack.Text = "Back to Search";
                        CurrentViewContext.WorkQueue = String.Empty;
                    }
                    CurrentViewContext.FromAdminApplicantID = args.ContainsKey("OrganizationUserId") ? Convert.ToInt32(args["OrganizationUserId"]) : AppConsts.NONE;
                    CurrentViewContext.FromAdminTenantID = args.ContainsKey("TenantID") ? Convert.ToInt32(args["TenantID"]) : AppConsts.NONE;
                    CurrentViewContext.IsAdminScreen = true;
                    ucUploadDocuments.TenantID = CurrentViewContext.FromAdminTenantID;
                    ucUploadDocuments.FromAdminApplicantID = CurrentViewContext.FromAdminApplicantID;
                    ucUploadDocuments.IsAdminScreen = CurrentViewContext.IsAdminScreen;
                    CurrentViewContext.PackageSubscriptionId = args.ContainsKey("PackageSubscriptionId") ? Convert.ToInt32(args["PackageSubscriptionId"]) : AppConsts.NONE;

                }

                else
                {
                    CurrentViewContext.TenantID = args.ContainsKey("TenantId") ? Convert.ToInt32(args["TenantId"]) : AppConsts.NONE;
                    CurrentViewContext.FromAdminApplicantID = CurrentUserID;
                    CurrentViewContext.FromAdminTenantID = ucUploadDocuments.TenantID = CurrentViewContext.TenantID;
                    CurrentViewContext.IsAdminScreen = false;
                    ucUploadDocuments.IsAdminScreen = CurrentViewContext.IsAdminScreen;
                }

                if (args.ContainsKey("isUploadOrViewDocument") && args["isUploadOrViewDocument"].IsNotNull())
                {

                    IsUploadOrViewDocument = args["isUploadOrViewDocument"];

                    //UAT-2729 : Add link on manage documents screen to return applicant to their compliance tracking. 
                    if (args.ContainsKey("isBackToDashBoardLink") && Convert.ToBoolean(args["isBackToDashBoardLink"]))
                    {
                        dvBackToCompTracking.Style["display"] = "block";
                    }
                }

                //UAT-4067
                if (args.ContainsKey("allowedFileExtensions"))
                {
                    String allowedFileExtensions = args["allowedFileExtensions"];
                    ucUploadDocuments.AllowedExtensions = allowedFileExtensions;
                    CurrentViewContext.AllowedFileExtension = allowedFileExtensions;
                    if (args.ContainsKey("showSeparateTabForApplicantPersonalDocs"))
                    {
                        Boolean isSeparateTabs = Convert.ToBoolean(args["showSeparateTabForApplicantPersonalDocs"]);
                        if (!isSeparateTabs)
                            IsAllowedFileExtensionEnable = !isSeparateTabs;
                    }
                    ucUploadDocuments.IsAllowedFileExtensionEnable = IsAllowedFileExtensionEnable;
                }
            }
            //base.SetPageTitle("Manage Documents");
            base.SetPageTitle(Resources.Language.MANGEDOCS);

            // CurrentViewContext.TenantID = args.ContainsKey("TenantId") ? Convert.ToInt32(args["TenantId"]) : AppConsts.NONE;
            ucUploadDocuments.OnCompletedUpload -= new UploadDocuments.UploadDelegate(ucUploadDocuments_OnCompletedUpload);
            ucUploadDocuments.OnCompletedUpload += new UploadDocuments.UploadDelegate(ucUploadDocuments_OnCompletedUpload);
            // ucUploadDocuments.TenantID = CurrentViewContext.TenantID;
            if (IsUploadOrViewDocument.Equals("View"))
            {
                //lnkBacKToComplianceTracking.Text = "Back to Dashboard";//UAT-2729
                lnkBacKToComplianceTracking.Text = Resources.Language.BKTODASHBRD;//UAT-2729
                divUploadDoc.Visible = false;
                divUnifiedDoc.Visible = false;
                grdMapping.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                grdMapping.MasterTableView.GetColumn("EditCommandColumn").Visible = false;
                grdMapping.MasterTableView.GetColumn("ItemName").Visible = false;
            }
            //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
            HideShowGridControlBasisOnVerPerms();
            //UAT-509 WB: Ability to limit admin access to read only from portfolio search screen.
            HideShowGridControlBasisOnProfilePerms();
            #region UAT-977: Additional work towards archive ability
            if (!this.IsPostBack)
            {
                Presenter.GetSubscriptionArchiveState();                
            }
            if(!CurrentViewContext.IsAdminScreen)
            Presenter.GetApplicantClinicalRotationMember(); // UAT-4687
            HideControlsBasisOnArchiveState();
            #endregion

            #region UAT-2296
            if (!CurrentViewContext.IsAdminScreen)
            {
                List<ClientSetting> lstClientSetting = Presenter.GetClientSetting();
                var _setting = lstClientSetting.WhereSelect(cs => cs.lkpSetting.Code == Setting.APPLICANT_DOCUMENT_ASSOCIATION.GetStringValue()).FirstOrNew();

                if (!_setting.IsNullOrEmpty() && !CurrentViewContext.IsAdminScreen)
                {
                    hdnDocumentAssociationSettingEnabled.Value = _setting.CS_SettingValue;
                }
                else
                {
                    hdnDocumentAssociationSettingEnabled.Value = AppConsts.ZERO;
                }
            }
            #endregion
        }

        #endregion

        #region Grid Related Events
        void ucUploadDocuments_OnCompletedUpload()
        {
            grdMapping.Rebind();
        }

        /// <summary>
        /// Retrieves a list of all uploaded documents.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void grdMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Presenter.GetApplicantUploadedDocuments();
            grdMapping.DataSource = ApplicantUploadedDocuments;
            //WclGrid grid = sender as WclGrid;
            //grid.PagerStyle.PagerTextFormat = "{4} {5}" + Resources.Language.ITEMS + Resources.Language.IN + "{1}" + Resources.Language.PAGE + "dasds";
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
            ApplicantUploadedDocumentID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ApplicantDocumentID"));
            string documentTypeCode = Convert.ToString(gridEditableItem.GetDataKeyValue("DocumentTypeCode"));
            CurrentViewContext.IsRequirementFieldUploadDocument = documentTypeCode.Equals(DocumentType.REQUIREMENT_FIELD_UPLOAD_DOCUMENT.GetStringValue()) ? true : false;

            if (Presenter.DeleteApplicantUploadedDocument())
            {
                //Presenter.CallParallelTaskPdfConversionMerging();
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
            ApplicantUploadedDocumentID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ApplicantDocumentID"));
            String FileType = gridEditableItem.GetDataKeyValue("FileType").ToString().Replace(" File","").RemoveWhitespace();
            if (CurrentViewContext.AllowedFileExtension.Contains(FileType))
            {
                //UAT-2296 Map docuemt with seleted items only if DocumentAssociationSetting is turned in for the tenant
                if (!CurrentViewContext.IsAdminScreen)
                {
                    Presenter.GetlkpDataEntryDocumentStatus();
                    if (hdnDocumentAssociationSettingEnabled.Value == "1")
                    {
                        Presenter.GetlkpItemDocMappingType();
                        WclComboBox ddlcmbItems = e.Item.FindControl("cmbItems") as WclComboBox;
                        foreach (var item in ddlcmbItems.CheckedItems)
                        {
                            if (item.Enabled != false)
                            {
                                String[] catItemId = item.Value.Split('_');
                                if (catItemId[1] != "-1")
                                {
                                    DocItemAssociationForDataEntry newRecord = new DocItemAssociationForDataEntry();
                                    newRecord.DAFD_ApplicantDocumentId = ApplicantUploadedDocumentID;
                                    newRecord.DAFD_ComplianceCategoryId = Convert.ToInt32(catItemId[0]);
                                    Int32? itemId = Convert.ToInt32(catItemId[1]) == AppConsts.NONE ? (Int32?)null : Convert.ToInt32(catItemId[1]);
                                    newRecord.DAFD_ComplianceItemId = itemId;
                                    newRecord.DAFD_MappingType = itemId.IsNull() ? CurrentViewContext.lstItemDocMappingType.FirstOrDefault(cond => cond.IDMT_Code == ItemDocMappingType.CATEGORY_EXCEPTION.GetStringValue()).IDMT_ID
                                                                                    : CurrentViewContext.lstItemDocMappingType.FirstOrDefault(cond => cond.IDMT_Code == ItemDocMappingType.ITEM_DATA.GetStringValue()).IDMT_ID;
                                    newRecord.DAFD_IsDeleted = false;
                                    newRecord.DAFD_CreatedOn = DateTime.Now;
                                    newRecord.DAFD_CreatedById = CurrentViewContext.OrgUsrID;
                                    ToUpdateUploadedDocument.DocItemAssociationForDataEntries.Add(newRecord);
                                }
                            }
                        }
                    }
                }

                String description = (e.Item.FindControl("txtDescription") as WclTextBox).Text;

                ToUpdateUploadedDocument.ApplicantDocumentID = ApplicantUploadedDocumentID;
                ToUpdateUploadedDocument.Description = description;
                ToUpdateUploadedDocument.ModifiedByID = CurrentViewContext.OrgUsrID;
                ToUpdateUploadedDocument.ModifiedOn = DateTime.Now;
                //UAT-2296 change the status of DataEntryDocument after updation.
                if (!CurrentViewContext.IsAdminScreen)
                {
                    ToUpdateUploadedDocument.DataEntryDocumentStatusID = CurrentViewContext.lkpDataEntryDocumentStatus.FirstOrDefault(con => con.LDEDS_Code == DataEntryDocumentStatus.IN_PROGRESS.GetStringValue()).LDEDS_ID;
                }
                if (Presenter.UpdateApplicantUploadedDocument())
                {
                    base.ShowSuccessMessage("Document updated successfully.");
                }
                else
                {
                    base.ShowErrorMessage("Document cannot be updated.");
                }
            }
            else
            {
                base.ShowInfoMessage("Unsupported file type.");
            }

        }

        protected void grdMapping_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var dataItem = (GridDataItem)e.Item;
                var Sizevalue = DataBinder.Eval(dataItem.DataItem, "Size").IsNullOrEmpty() ? 0 : DataBinder.Eval(dataItem.DataItem, "Size");

                //double Sizevalue = DataBinder.Eval(dataItem.DataItem, "Size") as double? ?? 0;
                dataItem["Size"].Text = string.Format((new System.Globalization.CultureInfo(LanguageCultures.ENGLISH_CULTURE.GetStringValue())).NumberFormat, "{0:F}", Sizevalue);
            }

            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                ApplicantDocumentDetails docs = e.Item.DataItem as ApplicantDocumentDetails;
                if (!docs.DocumentPath.IsNullOrEmpty())
                {
                    String fileName = docs.DocumentPath.Substring(docs.DocumentPath.LastIndexOf("/") >= 0 ? docs.DocumentPath.LastIndexOf("/") : 0).Remove("/");
                    // String NavigateUrl = "~/Messaging/Pages/AttachmentDownload.aspx?" + AppConsts.FILE_NAME_QUERY_STRING + "=" + fileName + "&" + AppConsts.ORIGINAL_FILE_NAME_QUERY_STRING + "=" + docs.FileName;
                    HtmlAnchor anchor = (HtmlAnchor)e.Item.FindControl("ancManageDocument");
                    if (CurrentViewContext.IsAdminScreen == false)
                    {
                        anchor.HRef = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?documentId={0}&tenantId={1}&IsPDFFileDownload={2}", docs.ApplicantDocumentID, TenantID, true);
                    }
                    else
                    {
                        anchor.HRef = String.Format("/ComplianceOperations/UserControl/DoccumentDownload.aspx?documentId={0}&tenantId={1}&IsPDFFileDownload={2}", docs.ApplicantDocumentID, FromAdminTenantID, true);
                    }
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
                        if (!documentTypeCode.Equals(DocumentType.REQUIREMENT_FIELD_UPLOAD_DOCUMENT.GetStringValue()) && !documentTypeCode.Equals(DocumentType.REQUIREMENT_FIELD_VIEW_DOCUMENT.GetStringValue())) //UAT-4900
                        {
                            (e.Item as GridEditableItem)["DeleteColumn"].Controls[AppConsts.NONE].Visible = false;
                            (e.Item as GridEditableItem)["DeleteColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                        }
                        (e.Item as GridEditableItem)["EditCommandColumn"].Controls[AppConsts.NONE].Visible = false;
                        (e.Item as GridEditableItem)["EditCommandColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
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
                if (!CurrentViewContext.IsAdminScreen)
                {
                    if (hdnDocumentAssociationSettingEnabled.Value == Convert.ToString(1))
                    {
                        ApplicantDocumentDetails docs = e.Item.DataItem as ApplicantDocumentDetails;
                        hdnAlreadySelectedAppDocItemAssociationID.Value = docs.ApplicantDocItemAssociationID;
                        List<String> lstSelectedIds = docs.ApplicantDocItemAssociationID.Trim().Split(',').ToList();
                        List<String> lstExceptionIds = lstSelectedIds.Where(con => con.Contains("_0")).Select(con => con.Split('_')[0]).ToList();

                        Presenter.GetSubscribedPackagesItems();
                        combo.DataSource = CurrentViewContext.lstSubcribedItems;
                        combo.DataBind();
                        combo.Visible = true;

                        foreach (RadComboBoxItem item in combo.Items)
                        {
                            if (lstSelectedIds.Contains(item.Value))
                            {
                                item.Checked = true;
                                item.Enabled = false;
                            }
                            if (lstExceptionIds.Count > 0)
                            {
                                if (lstExceptionIds.Contains(item.Value.Split('_')[0]))
                                {
                                    item.Enabled = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        combo.Visible = false;
                    }
                }
                else
                {
                    combo.Visible = false;
                }
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

        #region Button event
        protected void lnkGoBack_click(object sender, EventArgs e)
        {
            if (CurrentViewContext.IsAdminScreen == true && PageType == "VerificationDetail")
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", ChildControls.VerificationDetailsNew },
                                                                    {"PageType", "VerificationDetail" },
                                                                    { "TenantId", Convert.ToString( FromAdminTenantID) },
                                                                    {"PackageSubscriptionId",PackageSubscriptionId.ToString()},
                                                                    {"WorkQueueType", WorkQueue},
                                                                    {"ApplicantId",Convert.ToString(FromAdminApplicantID)},
																	{"CategoryId", SelectedComplianceCategoryId_Global.ToString()},
																	{"SelectedComplianceCategoryId", SelectedComplianceCategoryId_Global},
																	{ "IsException", IsException.ToString()},
																	{"PackageId",this.SelectedPackageId.ToString()},
																	{ "ItemDataId", Convert.ToString(this.SelectedItemDataId)},
																	{"ComplianceItemReconciliationDataID",Convert.ToString(this.SelectedComplianceItemReconciliationDataID)},
																 };
                string url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);

            }
            else if (CurrentViewContext.IsAdminScreen == true && PageType == "ApplicantView")
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    {"TenantId", FromAdminTenantID.ToString() },
                                                                    {"Child", ChildControls.SubscriptionDetail},
                                                                    {"WorkQueueType",CurrentViewContext.WorkQueue.ToString()},
                                                                    {"PackageSubscriptionId",Convert.ToString(CurrentViewContext.PackageSubscriptionId)} ,
                                                                    {"ApplicantId",Convert.ToString(FromAdminApplicantID)},
                                                                    {"IsFullPermissionForVerification",IsFullPermissionForVerification.ToString()}
                                                                 };
                string url = String.Format(@"/Dashboard/Pages/ApplicantDashboardMain.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }
            else
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", ChildControls.ApplicantPortFolioSearchPage },
                                                                    {"PageType", "PortfolioSearch" }

                                                                 
                                                                 };
                string url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);


            }

        }

        #region UAT-1544: WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.
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
                    //base.ShowInfoMessage("Please select document(s) to print.");
                    base.ShowInfoMessage(Resources.Language.UPLOADFILESLCTDOCTOPRNT);
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
        #endregion

        #region ComboBox Events

        /// <summary>
        /// UAT-2296
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

        #region CheckBox Events
        #region UAT-1544:WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.
        /// <summary>
        /// Handel selected document 
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
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
        #endregion
        #endregion
        #endregion

        #region Methods

        /// <summary>
        /// Method to hide show grid edit and update links and upload document links on the basis of verification permission.
        /// </summary>
        private void HideShowGridControlBasisOnVerPerms()
        {
            if (PageType == "PortfolioSearch")
            {
                String permissionCode = Presenter.GetUserNodePermission();
                if (permissionCode == LkpPermission.ReadOnly.GetStringValue())
                {
                    grdMapping.MasterTableView.Columns.FindByUniqueName("DeleteColumn").Visible = false;
                    grdMapping.MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
                    divUploadDoc.Style["display"] = "none";
                }
            }

            if (PageType == "ApplicantView" && !this.IsFullPermissionForVerification)
            {
                grdMapping.MasterTableView.Columns.FindByUniqueName("DeleteColumn").Visible = false;
                grdMapping.MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
                divUploadDoc.Style["display"] = "none";
            }
        }

        #region UAT-977: Additional work towards archive ability
        /// <summary>
        /// Method to hide grid edit and update links and upload document links on the basis of Archive State.
        /// </summary>
        private void HideControlsBasisOnArchiveState()
        {
            if (!CurrentViewContext.IsAdminScreen && !CurrentViewContext.IsActiveSubscription && !CurrentViewContext.IsApplicantClinicalRotationMember)
            {
                grdMapping.MasterTableView.Columns.FindByUniqueName("DeleteColumn").Visible = false;
                grdMapping.MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
                divUploadDoc.Style["display"] = "none";
            }
        }
        #endregion


        /// <summary>
        /// Method to hide show grid edit and update links and upload document links on the basis of verification permission.
        /// </summary>
        private void HideShowGridControlBasisOnProfilePerms()
        {
            if (PageType == "VerificationDetail" && !this.IsFullPermissionForVerification)
            {
                grdMapping.MasterTableView.Columns.FindByUniqueName("DeleteColumn").Visible = false;
                grdMapping.MasterTableView.Columns.FindByUniqueName("EditCommandColumn").Visible = false;
                divUploadDoc.Style["display"] = "none";
            }
        }

        #region UAT-1544: WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.
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
                    string url = String.Format(@"/ComplianceOperations/Pages/FormViewer.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    hdnPrintDocumentURL.Value = url;
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenDocumentInPopup", "openApplicantDocumentToPrint();", true);
                }
            }
            else
            {
                //base.ShowInfoMessage("Please select document(s) to print.");
                base.ShowInfoMessage(Resources.Language.UPLOADFILESLCTDOCTOPRNT);
            }
        }
        #endregion

        #endregion

        protected void lnkUnifiedDocument_Click(object sender, EventArgs e)
        {
            var unifiedDocument = Presenter.GetPdfAsUnifiedDocument();
            if (!unifiedDocument.IsNullOrEmpty())
            {
                if (unifiedDocument.UPD_DocumentStatusID.IsNotNull() && unifiedDocument.lkpDocumentStatu.DMS_Code.Equals(LKPDocumentStatus.MergingInProgress.GetStringValue()))
                {
                    base.ShowInfoMessage("Unified Document is not available yet. Please try after some time.");
                }
                else if (unifiedDocument.UPD_DocumentStatusID.IsNotNull() && (unifiedDocument.lkpDocumentStatu.DMS_Code.Equals(LKPDocumentStatus.MergingCompleted.GetStringValue()) ||
                    unifiedDocument.lkpDocumentStatu.DMS_Code.Equals(LKPDocumentStatus.MergingCompletedWithErrors.GetStringValue())))
                {
                    ifrUnifiedDocument.Src = "~/ComplianceOperations/UserControl/DocumentViewer.aspx?tenantId=" + FromAdminTenantID + "&UserId=" + FromAdminApplicantID + "&UnifiedDocument=true";

                }
                else
                {
                    base.ShowInfoMessage("Document not found. Please contact system administrator or try again later.");
                }

            }
            else
            {
                base.ShowInfoMessage("Document not found. Please contact system administrator or try again later.");
            }
            grdMapping.Rebind();
        }

        //Fixed issue on tab change Edit option get visible for all rows in grid.
        public void ReloadGrid()
        {
            grdMapping.Rebind();
        }
        //UAT-2729 : Add link on manage documents screen to return applicant to their compliance tracking. 
        protected void lnkBacKToComplianceTracking_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(AppConsts.APPLICANT_MAIN_PAGE_NAME);
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
    }
}

