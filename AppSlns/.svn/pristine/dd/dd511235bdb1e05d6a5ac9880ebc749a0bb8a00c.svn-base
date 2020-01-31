#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;


#endregion

#region Application Specific

using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Web;
using System.Linq;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class VerificationDocumentPanel : BaseUserControl, IVerificationDocumentPanelView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables
        private String _viewType;
        private VerificationDocumentPanelPresenter _presenter = new VerificationDocumentPanelPresenter();
        #endregion

        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {

            _viewType = Request.QueryString[AppConsts.UCID] == null ? String.Empty : Request.QueryString[AppConsts.UCID];
            if (!this.IsPostBack)
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    CaptureQuerystringParameters(args);
                }
                Presenter.OnViewInitialized();
                #region UAT-1538
                BindDocViewTypeRadioListData();
                #endregion
            }
            hdnTenantId.Value = Convert.ToString(this.SelectedTenantId);
            //set document Status ID as Merging Completed
            hdnMergingCompletedDocStatusID.Value = Convert.ToString(Presenter.GetDocumentStatusID());
            lblShowingDocumentText.Visible = false;
            UpdateDocumentListDelegate();
            //UAT-1538
            //ancUpload.Visible = false;
            BindData();
            Presenter.OnViewLoaded();
            if (Request.Form.Count > 0)
            {
                var requestParFloatingMode = Request.Form.GetValues("hdnFloatingMode");
                String floatingMode = requestParFloatingMode.IsNotNull() ? requestParFloatingMode[0] : "";
                var requestParDockLeft = Request.Form.GetValues("hdnDockLeft");
                String dockLeft = requestParDockLeft.IsNotNull() ? requestParDockLeft[0] : "";
                var requestParDockTop = Request.Form.GetValues("hdnDockTop");
                String docktop = requestParDockTop.IsNotNull() ? requestParDockTop[0] : "";
                if (hdnIsFloatingMode.Value.IsNullOrEmpty() && hdnDockLeft.Value.IsNullOrEmpty() && hdnDockTop.Value.IsNullOrEmpty())
                {
                    hdnIsFloatingMode.Value = floatingMode;
                    hdnDockLeft.Value = dockLeft;
                    hdnDockTop.Value = docktop;
                }
            }

        }

        #endregion

        #region Presenter object

        public VerificationDocumentPanelPresenter Presenter
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
        #endregion

        #region Properties

        public Int32 PackageSubscriptionId
        {
            get { return (Int32)(ViewState["PackageSubscriptionId"]); }
            set { ViewState["PackageSubscriptionId"] = value; }
        }

        public Int32 OrganizationUserId
        {
            get { return (Int32)(ViewState["OrganizationUserId_User"]); }
            set { ViewState["OrganizationUserId_User"] = value; }
        }

        public List<ApplicantDocuments> lstApplicantDocument
        {
            get;
            set;
        }

        public List<ApplicantDocuments> oldApplicantDocument
        {
            get;
            set;
        }

        public Int32 SelectedTenantId
        {
            get { return (Int32)(ViewState["SelectedTenantId"]); }
            set { ViewState["SelectedTenantId"] = value; }
        }

        public Int32 ItemDataId
        {
            get
            {
                return (Int32)(ViewState["ItemDataId"]);
            }
            set
            {
                ViewState["ItemDataId"] = value;
            }
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

        /// <summary>
        /// set Category id for return back to queue.
        /// </summary>
        public Int32 SelectedComplianceCategoryId_Global
        {
            get
            {
                if (ViewState["SelectedComplianceCategoryId"] != null)
                    return (Int32)(ViewState["SelectedComplianceCategoryId"]);
                return 0;
            }
            set
            {
                ViewState["SelectedComplianceCategoryId"] = value;
                // documentSection.Visible = IsVisibleDocumentSection;

            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
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

        public Boolean IsException
        {
            get { return Convert.ToBoolean(ViewState["IsException"]); }
            set { ViewState["IsException"] = value; }
        }

        /// <summary>
        /// Document TypeID of Screening document synched by data synching process.
        /// </summary>
        public Int32 ScreeningDocumentTypeId
        {
            get;
            set;
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


		#endregion

		#region Methods

		/// <summary>
		/// aproach is to use multicast delegate to 
		/// update the document list from the middle panel control
		/// </summary>
		private void UpdateDocumentListDelegate()
        {
            //Delegate use to bind data on change of any data in other control.
            if (HttpContext.Current.Items["UpdateDocumentList"] == null)
            {
                del = new UpdateDocumentList(SetUpdateDocumentList_Document);
                HttpContext.Current.Items["UpdateDocumentList"] = del;
            }
            else
            {
                del = (UpdateDocumentList)HttpContext.Current.Items["UpdateDocumentList"];
                del += new UpdateDocumentList(SetUpdateDocumentList_Document);
                HttpContext.Current.Items["UpdateDocumentList"] = del;
            }
        }

        /// <summary>
        /// This Methods set the different property on the page extracting from the query string.
        /// </summary>
        /// <param name="args">Query string parameter thsat contains the value.</param>
        private void CaptureQuerystringParameters(Dictionary<String, String> args)
        {
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey("SelectedTenantId"))
            {
                SelectedTenantId = Convert.ToInt32(args["SelectedTenantId"]);
                hdnTenantId.Value = SelectedTenantId.ToString();
            }
            if (args.ContainsKey("ItemDataId"))
            {
                ItemDataId = Convert.ToInt32(args["ItemDataId"]);
            }
            if (args.ContainsKey("ApplicantId"))
                hdnApplicantIdDocumentPanel.Value = args["ApplicantId"];

            if (args.ContainsKey("ActionType") && args["ActionType"] == VerificationDetailActionType.SubscriptionPreviousNext.GetStringValue())
                hdnIsApplicantChanged.Value = "true";
			if(args.ContainsKey("PackageId"))
				SelectedPackageId = args["PackageId"];
			if (args.ContainsKey("ItemDataId"))
				SelectedItemDataId = args["ItemDataId"];
			if (args.ContainsKey("SelectedComplianceItemReconciliationDataID"))
				SelectedComplianceItemReconciliationDataID = args["SelectedComplianceItemReconciliationDataID"];


		}



		/// <summary>
		/// Binds the document to the rotator specific to the category on page load
		/// and all documents related to the client.
		/// </summary>
		/// <param name="IsCategorySpecificData">Tells whether to get category specific data or all the data.</param>
		public void BindData()
        {
            //ArrayList values = new ArrayList();
            List<ApplicantDocuments> values = new List<ApplicantDocuments>();
            if (hdnViewAll.Value.IsNullOrEmpty())
            {
                //Category Specific documents
                oldApplicantDocument = lstApplicantDocument;
                Presenter.BindDocumentsForCaegory();
            }
            else
            {
                if (!oldApplicantDocument.IsNullOrEmpty())
                {
                    lstApplicantDocument = oldApplicantDocument;
                }
                //UAT-1538---------------
                //lblShowingDocumentText.Visible = true;
                //-----------------------
                //Manage document will visible after clicking View all Link. 
                ancUpload.Visible = true;
                //lnkbtnShowAllDocuments.Visible = true;
            }
            if ((lstApplicantDocument != null) && lstApplicantDocument.Count > 0)
            {
                foreach (var eachDocument in lstApplicantDocument.DistinctBy(x => x.ApplicantDocumentId))
                {
                    String fileExtension = GetFileExtension(eachDocument.DocumentName);
                    //Image icon to be displayed
                    if (!(fileExtension.Equals(String.Empty)))
                    {
                        values.Add(new ApplicantDocuments(eachDocument.DocumentName, "~/images/adb/" + fileExtension + ".png", eachDocument.ApplicantDocumentId,
                            eachDocument.DocumentSize, eachDocument.ItemID, eachDocument.PackageId, eachDocument.CategoryID, eachDocument.ApplicantDocumentMergingID
                            , eachDocument.UnifiedDocumentStartPageID, eachDocument.UnifiedDocumentEndPageID, eachDocument.UnifiedPdfDocumentID
                            , eachDocument.UnifiedPdfFileName, eachDocument.UnifiedPdfDocPath, eachDocument.UnifiedDocumentStatusID, eachDocument.DocumentType
                            , eachDocument.ApplicantDocumentMergingStatusID, eachDocument.ComplianceItemID,eachDocument.DocumentDescription));
                    }
                }
                //Commented below code for UAT-1538:Unified Document/ single document option and updates to document exports
                //if (!(hdnRoratorWidth.Value == String.Empty))
                //{
                //    RadRotator1.Width = Convert.ToInt32(hdnRoratorWidth.Value);
                //}
            }
            //Commented below code for UAT-1538::Unified Document/ single document option and updates to document exports
            //RadRotator1.DataSource = values;
            //RadRotator1.DataBind();

            //ucPdfDocumentViewer.OrganizationUserId = this.OrganizationUserId;
            //ucPdfDocumentViewer.SelectedTenantId = this.SelectedTenantId;

            var selectedCatUnifiedDocList = lstApplicantDocument.Where(cond => cond.CategoryID == this.SelectedComplianceCategoryId_Global).ToList();
            Int32? selectedCatUnifiedStartPageID = 0;
            if (selectedCatUnifiedDocList.IsNotNull() && selectedCatUnifiedDocList.Count() > 0)
            {
                var selectedCatUnifiedDoc = selectedCatUnifiedDocList.Where(cond => cond.UnifiedPdfDocPath.IsNotNull()
                                                && !cond.UnifiedDocumentStartPageID.IsNullOrEmpty()).FirstOrDefault();
                if (selectedCatUnifiedDoc != null)
                {
                    //ucPdfDocumentViewer.SelectedCatUnifiedStartPageID = selectedCatUnifiedDoc.UnifiedDocumentStartPageID;
                    selectedCatUnifiedStartPageID = selectedCatUnifiedDoc.UnifiedDocumentStartPageID;
                }
            }

            Dictionary<String, String> requestDocViewerArgs = new Dictionary<String, String>();
            requestDocViewerArgs = new Dictionary<String, String>
                                                                 { 
                                                                    {"OrganizationUserId", Convert.ToString(this.OrganizationUserId) },
                                                                    {"SelectedTenantId", Convert.ToString(this.SelectedTenantId)},
                                                                    {"SelectedCatUnifiedStartPageID",Convert.ToString(selectedCatUnifiedStartPageID)},
                                                                    {"IsRequestAuth",Convert.ToString(AppConsts.TRUE)},
                                                                    {"DocumentViewType",UtilityFeatures.Unified_Document.GetStringValue()}
                                                                 };
            hdnSelectedCatUnifiedStartPageID.Value = Convert.ToString(selectedCatUnifiedStartPageID);
            string url = String.Format(@"/ComplianceOperations/UnifiedPdfDocViewer.aspx?args={0}", requestDocViewerArgs.ToEncryptedQueryString());
            //Commented below code UAT-1538::Unified Document/ single document option and updates to document exports
            //hdnDocVwr.Value = Convert.ToString(url);
            hdnUnifiedDocVwr.Value = Convert.ToString(url);

            #region UAT-1538::Unified Document/ single document option and updates to document exports

            Presenter.GetScreeningDocumentTypeId();

            var selectedApplicantDocList = lstApplicantDocument.Where(cond => cond.CategoryID == this.SelectedComplianceCategoryId_Global).ToList();
            Int32? documentId = 0;
            if (selectedApplicantDocList.IsNotNull() && selectedApplicantDocList.Count() > 0)
            {
                var selectedApplicantDoc = selectedApplicantDocList.FirstOrDefault(ad => ad.DocumentType == null
                                                                               || (ad.DocumentType != null && ad.DocumentType != ScreeningDocumentTypeId));
                if (selectedApplicantDoc != null)
                {
                    //ucPdfDocumentViewer.SelectedCatUnifiedStartPageID = selectedCatUnifiedDoc.UnifiedDocumentStartPageID;
                    documentId = selectedApplicantDoc.ApplicantDocumentId;
                }
            }
            Dictionary<String, String> requestSingleDocViewerArgs = new Dictionary<String, String>();
            requestSingleDocViewerArgs = new Dictionary<String, String>
                                                                 { 
                                                                    {"OrganizationUserId", Convert.ToString(this.OrganizationUserId) },
                                                                    {"SelectedTenantId", Convert.ToString(this.SelectedTenantId)},
                                                                    {"SelectedCatUnifiedStartPageID",AppConsts.ZERO},
                                                                    {"DocumentId",Convert.ToString(documentId)},
                                                                    {"IsRequestAuth",Convert.ToString(AppConsts.TRUE)},
                                                                    {"DocumentViewType",UtilityFeatures.Single_Document.GetStringValue()}
                                                                 };
            //hdnSelectedCatUnifiedStartPageID.Value = Convert.ToString(selectedCatUnifiedStartPageID);
            string urlSingleDoc = String.Format(@"/ComplianceOperations/UnifiedPdfDocViewer.aspx?args={0}", requestSingleDocViewerArgs.ToEncryptedQueryString());
            //Commented below code UAT-1538::Unified Document/ single document option and updates to document exports
            //hdnDocVwr.Value = Convert.ToString(url);
            hdnCurrentDocID.Value = Convert.ToString(documentId);
            hdnSingleDocVwr.Value = Convert.ToString(urlSingleDoc);
            if (rdbLstViewType.SelectedValue == UtilityFeatures.Unified_Document.GetStringValue())
            {
                hdnDocVwr.Value = hdnUnifiedDocVwr.Value;
            }
            else
            {
                hdnDocVwr.Value = hdnSingleDocVwr.Value;
            }

            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", ChildControls.ManageUploadDocuments},
                                                                    {"TenantID",hdnTenantId.Value},
                                                                    {"PageType","VerificationDetail"},
                                                                    {"OrganizationUserId",OrganizationUserId.ToString()},
                                                                    {"PackageSubscriptionId",PackageSubscriptionId.ToString()},
                                                                    {"WorkQueueType", WorkQueue},
                                                                    {"IsFullPermissionForVerification", IsFullPermissionForVerification.ToString()},
                                                                    {"IsException", IsException.ToString()},
																	{"SelectedComplianceCategoryId_Global", SelectedComplianceCategoryId_Global.ToString()},
																	{ "ItemDataId", Convert.ToString(this.ItemDataId)},
																	{"PackageId",this.SelectedPackageId},
																	{"ComplianceItemReconciliationDataID",this.SelectedComplianceItemReconciliationDataID},
																 };
            //URL to go Manage uploadDocument on Clicking Manage Document Link.  
            // UAT 1767
            ancUpload.HRef = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            //lnkbtnShowAllDocuments.Attributes.Add("url", String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString()));
            #endregion

        }

        public void SetUpdateDocumentList_Document(List<ApplicantDocuments> updatedList)
        {
            lstApplicantDocument = updatedList;
            BindData();
        }

        /// <summary>
        /// Get the extension of the file needed for the icon selection.
        /// </summary>
        /// <param name="fileName">File name as String</param>
        /// <returns>String(Extension Name)</returns>
        private String GetFileExtension(String fileName)
        {
            if (fileName.IsNotNull())
            {
                String fileExtension = fileName.ToLower().Substring((fileName.LastIndexOf('.') + 1), (fileName.Length - (fileName.LastIndexOf('.') + 1)));
                if (fileExtension.Equals("png") || fileExtension.Equals("jpg") || fileExtension.Equals("gif"))
                {
                    return "jpg";
                }
                else if (fileExtension.Equals("pdf") || fileExtension.Equals("docx") || fileExtension.Equals("xlsx") || fileExtension.Equals("xsl"))
                {
                    return fileExtension;
                }
                else
                {
                    return "doc";
                }
            }
            return String.Empty;
        }

        #region UAT-1538::Unified Document/ single document option and updates to document exports
        private void BindDocViewTypeRadioListData()
        {
            rdbLstViewType.Items.Add(new System.Web.UI.WebControls.ListItem { Text = "Unified Document &nbsp;", Value = UtilityFeatures.Unified_Document.GetStringValue() });
            rdbLstViewType.Items.Add(new System.Web.UI.WebControls.ListItem { Text = "Single Document &nbsp;", Value = UtilityFeatures.Single_Document.GetStringValue() });

            Entity.UtilityFeatureUsage docViewTypeUtilitySetting = Presenter.GetDocViewTypeSettings();
            if (docViewTypeUtilitySetting.IsNullOrEmpty())
            {
                Presenter.SaveUpdateDocumentViewSetting();
                rdbLstViewType.SelectedValue = UtilityFeatures.Unified_Document.GetStringValue();
            }
            else
            {
                rdbLstViewType.SelectedValue = docViewTypeUtilitySetting.lkpUtilityFeature.UF_Code;
            }
        }
        #endregion
        #endregion

        #region Events
        /// <summary>
        /// Gets all the document related to the item user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnShowAllDocuments_Click(object sender, EventArgs e)
        {
            //String url = lnkbtnShowAllDocuments.Attributes["url"];
            //if (!url.IsNullOrEmpty())
            //{
            //    Response.Redirect(url);
            //}
            hdnViewAll.Value = "true";
            BindData();
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", ChildControls.ManageUploadDocuments},
                                                                    {"TenantID",hdnTenantId.Value},
                                                                    {"PageType","VerificationDetail"},
                                                                    {"OrganizationUserId",OrganizationUserId.ToString()},
                                                                    {"PackageSubscriptionId",PackageSubscriptionId.ToString()},
                                                                    {"WorkQueueType", WorkQueue},
                                                                    {"IsFullPermissionForVerification", IsFullPermissionForVerification.ToString()},
                                                                 };
            //URL to go Manage uploadDocument on Clicking Manage Document Link.  
            ancUpload.HRef = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
        }
        #endregion
    }
}

