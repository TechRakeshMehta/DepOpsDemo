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
    public partial class ReconciliationDocumentPanel : BaseUserControl, IReconciliationDocumentPanelView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables
        private String _viewType;
        private ReconciliationDocumentPanelPresenter _presenter = new ReconciliationDocumentPanelPresenter();
        #endregion

        #endregion

        #region Presenter object

        public ReconciliationDocumentPanelPresenter Presenter
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

            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        /// <summary>
        /// Document TypeID of Screening document synched by data synching process.
        /// </summary>
        public Int32 ScreeningDocumentTypeId
        {
            get;
            set;
        }

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
            UpdateDocumentListDelegate();
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

            if (args.ContainsKey("TenantId"))
            {
                SelectedTenantId = Convert.ToInt32(args["TenantId"]);
                hdnTenantId.Value = SelectedTenantId.ToString();
            }
            if (args.ContainsKey("ItemDataId"))
            {
                ItemDataId = Convert.ToInt32(args["ItemDataId"]);
            }
            if (args.ContainsKey("ApplicantId"))
            {
                OrganizationUserId = Convert.ToInt32(args["ApplicantId"]);
                hdnApplicantIdDocumentPanel.Value = args["ApplicantId"];
            }
            if (args.ContainsKey("SelectedPackageSubscriptionId"))
                PackageSubscriptionId = Convert.ToInt32(args["SelectedPackageSubscriptionId"]);
            if (args.ContainsKey("SelectedComplianceCategoryId"))
                SelectedComplianceCategoryId_Global = Convert.ToInt32(args["SelectedComplianceCategoryId"]);
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
            }

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
            string urlSingleDoc = String.Format(@"/ComplianceOperations/UnifiedPdfDocViewer.aspx?args={0}", requestSingleDocViewerArgs.ToEncryptedQueryString());
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

    }
}

