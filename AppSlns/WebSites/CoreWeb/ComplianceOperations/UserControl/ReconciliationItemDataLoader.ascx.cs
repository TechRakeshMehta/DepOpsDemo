using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using Entity.ClientEntity;
using System.Linq;
using CoreWeb.ComplianceOperations.Views;
using INTSOF.Utils;
using System.Web;
using System.Text;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Web.UI;
using System.Configuration;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.Shell.Views;
using System.Xml;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ReconciliationItemDataLoader : BaseUserControl, IReconciliationItemDataLoaderView
    {
        public event DataSaved DataSavedClick;
        public delegate void DataSaved(object sender, EventArgs e);

        public List<ComplianceVerificationControls> ComplianceVerificationControlsList
        {
            get;
            set;
        }

        #region Variables

        #region Private Variables
        private List<UserSpecializationDetails> _lstUserSpecializations;
        private ReconciliationItemDataLoaderPresenter _presenter = new ReconciliationItemDataLoaderPresenter();

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        public String viewType
        {
            get
            {
                if (ViewState["viewTypeLoader"] != null)
                    return (String)ViewState["viewTypeLoader"];
                return null;
            }
            set
            {

                ViewState["viewTypeLoader"] = value;
            }
        }

        public List<ApplicantItemVerificationData> lst
        {
            get;
            set;
        }


        public List<ReconciliationDetailsDataContract> lstReconciliationDetailsData { get; set; }

        public ApplicantItemVerificationData CategoryLevelExcItemVerificationData
        {
            get;
            set;
        }

        public Int32 PackageId_Global
        {
            get
            {
                if (!ViewState["PackageIdLoader"].IsNullOrEmpty())
                    return (Int32)(ViewState["PackageIdLoader"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["PackageIdLoader"].IsNullOrEmpty())
                    ViewState["PackageIdLoader"] = value;
            }
        }

        public Int32 CategoryId_Global
        {
            get
            {
                if (!ViewState["CategoryIdLoader"].IsNullOrEmpty())
                    return (Int32)(ViewState["CategoryIdLoader"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["CategoryIdLoader"].IsNullOrEmpty())
                    ViewState["CategoryIdLoader"] = value;
            }
        }

        public List<ApplicantDocuments> lstApplicantDocument
        {
            get;
            set;
        }


        /// <summary>
        /// set user group id for return back to queue.
        /// </summary>
        public Int32 UserGroupId
        {
            get
            {
                return Convert.ToInt32(ViewState["UserGroupId"] ?? "0");
            }
            set
            {
                ViewState["UserGroupId"] = value;
            }
        }

        public IReconciliationItemDataLoaderView CurrentViewContext
        {
            get { return this; }
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                if (!ViewState["IsDefaultTenant_Loader"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsDefaultTenant_Loader"]);

                return false;
            }
            set
            {
                ViewState["IsDefaultTenant_Loader"] = value;
            }
        }

        public Boolean IsException
        {
            get { return (Boolean)(ViewState["IsException_Loader"]); }
            set { ViewState["IsException_Loader"] = value; }
        }

        public ReconciliationItemDataLoaderPresenter Presenter
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

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        public Int32 CurrentTenantId_Global
        {
            get
            {
                if (!ViewState["CurrentTenantIdLoader"].IsNullOrEmpty())
                    return (Int32)(ViewState["CurrentTenantIdLoader"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["CurrentTenantIdLoader"].IsNullOrEmpty())
                    ViewState["CurrentTenantIdLoader"] = value;
            }
        }

        public Int32 SelectedTenantId_Global
        {
            get
            {
                if (!ViewState["SelectedTenantIdLoader"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedTenantIdLoader"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedTenantIdLoader"].IsNullOrEmpty())
                    ViewState["SelectedTenantIdLoader"] = value;
            }
        }

        public Int32 SelectedCompliancePackageId_Global
        {
            get
            {
                if (!ViewState["SelectedCompliancePackageIdLoader"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedCompliancePackageIdLoader"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedCompliancePackageIdLoader"].IsNullOrEmpty())
                    ViewState["SelectedCompliancePackageIdLoader"] = value;
            }
        }

        public Int32 SelectedComplianceCategoryId_Global
        {
            get
            {
                if (!ViewState["SelectedComplianceCategoryIdLoader"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedComplianceCategoryIdLoader"]);
                else
                    return 0;
            }
            set
            {
                ViewState["SelectedComplianceCategoryIdLoader"] = value;
            }
        }

        public Int32 SelectedApplicantId_Global
        {
            get
            {
                if (!ViewState["SelectedApplicantIdLoader"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedApplicantIdLoader"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedApplicantIdLoader"].IsNullOrEmpty())
                    ViewState["SelectedApplicantIdLoader"] = value;
            }
        }

        public String CurrentLoggedInUserName_Global
        {
            get
            {
                if (!ViewState["CurrentLoggedInUserNameLoader"].IsNullOrEmpty())
                    return (String)(ViewState["CurrentLoggedInUserNameLoader"]);
                else
                    return String.Empty;
            }
            set
            {
                if (ViewState["CurrentLoggedInUserNameLoader"].IsNullOrEmpty())
                    ViewState["CurrentLoggedInUserNameLoader"] = value;
            }
        }

        public Int32 AssignedToVerUser
        {
            get
            {
                if (!ViewState["AssignedToVerUserLoader"].IsNullOrEmpty())
                    return (Int32)(ViewState["AssignedToVerUserLoader"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["AssignedToVerUserLoader"].IsNullOrEmpty())
                    ViewState["AssignedToVerUserLoader"] = value;
            }
        }

        public String StatusMessage { get; set; }

        public Int32 CurrentPackageSubscriptionID_Global
        {
            get { return (Int32)(ViewState["CurrentPackageSubscriptionIDLoader"]); }
            set { ViewState["CurrentPackageSubscriptionIDLoader"] = value; }
        }

        public Int32 NextComplianceCategoryId_Global
        {
            get
            {
                if (!ViewState["NextComplianceCategoryId"].IsNullOrEmpty())
                    return (Int32)(ViewState["NextComplianceCategoryId"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["NextComplianceCategoryId"].IsNullOrEmpty())
                    ViewState["NextComplianceCategoryId"] = value;
            }
        }

        public Int32 PreviousComplianceCategoryId_Global
        {
            get
            {
                if (!ViewState["PreviousComplianceCategoryId"].IsNullOrEmpty())
                    return (Int32)(ViewState["PreviousComplianceCategoryId"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["PreviousComplianceCategoryId"].IsNullOrEmpty())
                    ViewState["PreviousComplianceCategoryId"] = value;
            }
        }

        public Boolean IncludeIncompleteItems_Global
        {
            get { return (Boolean)(ViewState["IncludeIncompleteItemsLoader"]); }
            set { ViewState["IncludeIncompleteItemsLoader"] = value; }
        }

        public Boolean ShowOnlyRushOrders
        {
            get
            {
                if (!ViewState["ShowOnlyRushOrders"].IsNull())
                {
                    return (Boolean)ViewState["ShowOnlyRushOrders"];
                }
                return false;
            }
            set
            {
                ViewState["ShowOnlyRushOrders"] = value;
            }
        }

        public Int32 ItemDataId_Global
        {
            get { return (int)(ViewState["ItemDataIdLoader"]); }
            set { ViewState["ItemDataIdLoader"] = value; }
        }

        /// <summary>
        /// Contains the data of all the Items of a Category of a package
        /// </summary>
        public List<ListItemEditableBies> lstEditableByData
        {
            get;
            set;
        }

        public Boolean IsCategoryLevelException
        {
            get;
            set;
        }

        /// <summary>
        /// Contains Assignment Properties of all Items of a Category of a package
        /// </summary>
        public List<ListItemAssignmentProperties> lstAssignmentProperties
        {
            get;
            set;
        }

        //UAT 537 Verification Details Screen "go to Next Pending for Review Category" should save data and button text change.
        public Boolean IsDataSavedSuccessfully
        {
            get;
            set;
        }

        public Boolean IsUIValidationApplicable
        {
            get
            {
                Boolean _isUIValidation;
                String _uiValidation = Convert.ToString(ConfigurationManager.AppSettings["IsUIValidationApplicable"]);
                if (!String.IsNullOrEmpty(_uiValidation))
                {
                    Boolean.TryParse(_uiValidation, out _isUIValidation);
                    return _isUIValidation;
                }
                return false;
            }
        }

        public CommandBar btnSave { get; set; }

        public String LoggedInUserInitials_Global
        {
            get
            {
                if (!ViewState["LoggedInUserInitials"].IsNullOrEmpty())
                    return (String)(ViewState["LoggedInUserInitials"]);
                else
                    return String.Empty;
            }
            set
            {
                if (ViewState["LoggedInUserInitials"].IsNullOrEmpty())
                    ViewState["LoggedInUserInitials"] = value;
            }
        }
        #region UAT-523 Category exception
        public String ApprovedWithExcepStatusCode
        {
            get
            {
                return ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue();
            }
        }

        public String MinExpirationDate { get { return (DateTime.Now.AddDays(1)).ToShortDateString(); } }
        #endregion


        //UAT-3805
        public Entity.ClientEntity.PackageSubscription PackageSubscriptionBeforeSaving
        {
            get
            {
                if (!ViewState["PackageSubscriptionBeforeSaving"].IsNullOrEmpty())
                    return (Entity.ClientEntity.PackageSubscription)(ViewState["PackageSubscriptionBeforeSaving"]);
                else
                    return null;
            }
            set
            {
                ViewState["PackageSubscriptionBeforeSaving"] = value;
            }
        }

        #region UAT-3951:Addition of option to use preset ADB Admin rejection notes
        public List<Entity.RejectionReason> ListRejectionReasons { get; set; }
        #endregion

        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {            
            Dictionary<String, String> args = new Dictionary<string, string>();
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();

            }
            Presenter.OnViewLoaded();

            LoadControls();
            viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            (btnSave as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click here to save all changes made to the Items in this Category";
        }

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// Loads different user controls, based on the type of Mode & user
        /// </summary>
        private void LoadControls()
        {
            ComplianceVerificationControlsList = new List<ComplianceVerificationControls>();


            if (lst.IsNotNull() && lst.Count > 0)
            {
                CurrentViewContext.PackageSubscriptionBeforeSaving = Presenter.GetPackageSubscriptionByPackageID();
                String _tenantTypeCode = _presenter.GetTenantType();
                this.lstAssignmentProperties = _presenter.GetAssignmentPropertiesByCategoryId();
                this.lstEditableByData = _presenter.GetEditableBiesByCategoryId(); //UAT-3599
                #region UAT-523 Category Level Exception
                //UAT-523:Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
                Guid catExceptionItemCode = new Guid(AppConsts.WHOLE_CATEGORY_GUID);//For Dummy item inserted for category level exception "9122D197-27EE-44AC-8CC4-0A68F3D21F32"
                ApplicantItemVerificationData applicantDataCatException = lst.FirstOrDefault(cond => cond.CatExceptionItemCode == catExceptionItemCode);
                CategoryLevelExcItemVerificationData = applicantDataCatException;
                if (applicantDataCatException.IsNotNull())
                {
                    if (applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPLIED.GetStringValue()
                        || applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_REJECTED.GetStringValue()
                        || applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPROVED.GetStringValue())
                        IsCategoryLevelException = true;
                    else
                        IsCategoryLevelException = false;
                    SetDataForCategoryException(applicantDataCatException);
                    //Remove the category level exception dummy item from ApplicantItemVarificationdata.
                }
                lst = lst.Where(cond => cond.CatExceptionItemCode != catExceptionItemCode).ToList();
                //UAT-845 Creation of admin override function (verification details).
                if (!IsCategoryLevelException)
                {
                    ApplicantItemVerificationData applicantDataCatOverride = lst.FirstOrDefault();
                    CategoryLevelExcItemVerificationData = applicantDataCatOverride;
                }


                #endregion

                List<Int32> lstApplicantItemIds = lst.Select(verData => verData.ComplianceItemId).Distinct().ToList();

                #region GET THE MAPPED DOCUMENTIds' RELATED DATA

                List<ApplicantDocumentMappingData> _lstDocumentMappingList = new List<ApplicantDocumentMappingData>();
                String _xmlApplicantComplianceItemDataIds = String.Empty;
                String _xmlApplicantComplianceAttributeDataIds = String.Empty;
                String _fileUploadCode = ComplianceAttributeDatatypes.FileUpload.GetStringValue();
                String _viewDocCode = ComplianceAttributeDatatypes.View_Document.GetStringValue();
                String _incompleteItemStatus = ApplicantItemComplianceStatus.Incomplete.GetStringValue();

                List<String> _lstExceptionStatusCodes = new List<String>();
                _lstExceptionStatusCodes.Add(ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue());
                _lstExceptionStatusCodes.Add(ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue());
                _lstExceptionStatusCodes.Add(ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue());

                List<String> _lstItemStatusCodes = new List<String>();
                _lstItemStatusCodes.Add(ApplicantItemComplianceStatus.Pending_Review.GetStringValue());
                _lstItemStatusCodes.Add(ApplicantItemComplianceStatus.Approved.GetStringValue());
                _lstItemStatusCodes.Add(ApplicantItemComplianceStatus.Not_Approved.GetStringValue());
                _lstItemStatusCodes.Add(ApplicantItemComplianceStatus.Expired.GetStringValue());
                _lstItemStatusCodes.Add(ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue());
                _lstItemStatusCodes.Add(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue());

                // Case when the Documents have been uploaded, without data entry, is also incomplete
                _lstItemStatusCodes.Add(ApplicantItemComplianceStatus.Incomplete.GetStringValue());

                // Get all Items which are related to Exception Applied, Approved or Rejected
                List<Int32?> _lstApplicantComplianceItemIds = lst.Where(x => x.ApplicantCompItemId != null
                                                                  && _lstExceptionStatusCodes.Contains(x.ItemComplianceStatusCode))
                                                                 .Select(x => x.ApplicantCompItemId).ToList();

                if (_lstApplicantComplianceItemIds.Count() > 0)
                    _xmlApplicantComplianceItemDataIds = GenerateItemIdsXML(_lstApplicantComplianceItemIds);

                // Get all items for which data entry has been done and have attribute of type fileupload
                List<ApplicantItemVerificationData> _lstVerificationData = lst.Where(
                                                                               x => x.ApplicantCompItemId != null
                                                                               && _lstItemStatusCodes.Contains(x.ItemComplianceStatusCode)
                                                                               && (x.AttributeTypeCode == _fileUploadCode || x.AttributeTypeCode == _viewDocCode)).ToList();

                if (_lstVerificationData.Count() > 0)
                    _xmlApplicantComplianceAttributeDataIds = GenerateAttributeIdsXML(_lstVerificationData);

                if (!String.IsNullOrEmpty(_xmlApplicantComplianceItemDataIds) || !String.IsNullOrEmpty(_xmlApplicantComplianceAttributeDataIds))
                {
                    _lstDocumentMappingList = Presenter.GetApplicantDocumentsData(_xmlApplicantComplianceItemDataIds, _xmlApplicantComplianceAttributeDataIds);
                }

                #endregion


                //#region Get Reconciled Doc Related Data

                //List<Int32> lstReconciledDocs = new List<int>();

                //List<String> lstReconciledData = lstReconciliationDetailsData.GroupBy(attr => attr.ReviewerID)
                //                                                                                         .Select(attr => attr.First().ApplicantDocumentIDs)
                //                                                                                         .ToList();
                //foreach (String csvDocIds in lstReconciledData)
                //{
                //    if (!csvDocIds.IsNullOrEmpty())
                //    {
                //        lstReconciledDocs.AddRange(csvDocIds.Split(',').Select(Int32.Parse).ToList());
                //    }
                //}
                //lstReconciledDocs = lstReconciledDocs.Distinct().ToList();
                //#endregion

                pnlItems.Controls.Clear();

                List<Int32> AssignedItemIDs = new List<Int32>();
                for (Int32 i = 0; i < lstApplicantItemIds.Count(); i++)
                {
                    ComplianceVerificationControls complianceVerificationControl = new ComplianceVerificationControls();
                    List<ApplicantItemVerificationData> tempList = lst.Where(data => data.ComplianceItemId == lstApplicantItemIds[i]).ToList();
                    String _currentItemStatus = tempList[0].ItemComplianceStatusCode;
                    Int32? _itemDataId = tempList[0].ApplicantCompItemId;
                    Int32? _reviewerTenantId = tempList[0].ReviewerTenantId;
                    Int32? _assignedToUserId = tempList[0].AssignedToUserId;
                    String _subscriptionMobilityStatusCode = tempList[0].SubscriptionMobilityStatusCode;
                    Int32? _itemMovementTypeId = tempList[0].ItemMovementTypeID;
                    Int32? unifiedDocumentPageId = 0;
                    var UnifiedDocument = CurrentViewContext.lstApplicantDocument.Where(cond => cond.ItemID == _itemDataId
                        //|| lstReconciledDocs.Contains(cond.ApplicantDocumentId)
                        ).ToList();
                    if (UnifiedDocument.IsNotNull() && UnifiedDocument.Count() > 0)
                    {
                        unifiedDocumentPageId = UnifiedDocument.FirstOrDefault().UnifiedDocumentStartPageID;
                    }

                    if (_itemDataId != CurrentViewContext.ItemDataId_Global)
                    {
                        System.Web.UI.Control readOnlyMode = Page.LoadControl("~/ComplianceOperations/UserControl/ReconciliationItemDataReadOnlyMode.ascx");
                        (readOnlyMode as ReconciliationItemDataReadOnlyMode).VerificationData = tempList;
                        (readOnlyMode as ReconciliationItemDataReadOnlyMode).FormMode = _currentItemStatus;
                        (readOnlyMode as ReconciliationItemDataReadOnlyMode).SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
                        (readOnlyMode as ReconciliationItemDataReadOnlyMode).CurrentPackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionID_Global;
                        (readOnlyMode as ReconciliationItemDataReadOnlyMode).lstApplicantDocument = CurrentViewContext.lstApplicantDocument;
                        (readOnlyMode as ReconciliationItemDataReadOnlyMode).UnifiedDocumentStartPageID = unifiedDocumentPageId;
                        (readOnlyMode as ReconciliationItemDataReadOnlyMode).lstApplicantComplianceDocumentMaps = FilterItemDocumentMapping(_lstDocumentMappingList, Convert.ToInt32(_itemDataId));
                        (readOnlyMode as ReconciliationItemDataReadOnlyMode).lstReconciliationDetailsData = lstReconciliationDetailsData;
                        (readOnlyMode as ReconciliationItemDataReadOnlyMode).lstAssignmentProperties = GetItemAssignmentProperties(lstApplicantItemIds[i]);
                        pnlItems.Controls.Add(readOnlyMode);

                        complianceVerificationControl.EditModeCntrlsList = readOnlyMode;
                        complianceVerificationControl.ComplianceItemID = lstApplicantItemIds[i];
                        complianceVerificationControl.VerificationDataMode = VerificationDataMode.ReadOnly;
                        ComplianceVerificationControlsList.Add(complianceVerificationControl);
                    }
                    else
                    {
                        System.Web.UI.Control editMode = Page.LoadControl("~/ComplianceOperations/UserControl/ReconciliationItemDataEditMode.ascx");

                        (editMode as ReconciliationItemDataEditMode).ApplicantItemDataId = Convert.ToInt32(_itemDataId);
                        (editMode as ReconciliationItemDataEditMode).VerificationData = tempList;
                        (editMode as ReconciliationItemDataEditMode).IsItmEditableByApplcnt = (lstEditableByData.Where(cond => cond.ComplianceItemId == lstApplicantItemIds[i]).Any(cond => cond.EditableByCode == "EDTAPCT")) ? true : false; //UAT-3599
                        (editMode as ReconciliationItemDataEditMode).lstReconciliationDetailsData = lstReconciliationDetailsData;
                        (editMode as ReconciliationItemDataEditMode).SelectedComplianceCategoryId_Global = CurrentViewContext.SelectedComplianceCategoryId_Global;
                        (editMode as ReconciliationItemDataEditMode).SelectedCompliancePackageId_Global = CurrentViewContext.SelectedCompliancePackageId_Global;
                        (editMode as ReconciliationItemDataEditMode).SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
                        (editMode as ReconciliationItemDataEditMode).SelectedApplicantId_Global = CurrentViewContext.SelectedApplicantId_Global;
                        (editMode as ReconciliationItemDataEditMode).CurrentLoggedInUserName_Global = CurrentViewContext.CurrentLoggedInUserName_Global;
                        (editMode as ReconciliationItemDataEditMode).CurrentTenantId_Global = CurrentViewContext.CurrentTenantId_Global;
                        (editMode as ReconciliationItemDataEditMode).CurrentPackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionID_Global;
                        (editMode as ReconciliationItemDataEditMode).lstAssignmentProperties = GetItemAssignmentProperties(lstApplicantItemIds[i]);  //--- STEP 2
                        (editMode as ReconciliationItemDataEditMode).lstApplicantDocument = CurrentViewContext.lstApplicantDocument;
                        (editMode as ReconciliationItemDataEditMode).UnifiedDocumentStartPageID = unifiedDocumentPageId;
                        (editMode as ReconciliationItemDataEditMode).lstApplicantComplianceDocumentMaps = FilterItemDocumentMapping(_lstDocumentMappingList, Convert.ToInt32(_itemDataId));
                        (editMode as ReconciliationItemDataEditMode).CurrentTenantTypeCode = _tenantTypeCode;
                        (editMode as ReconciliationItemDataEditMode).LoggedInUserInitials_Global = CurrentViewContext.LoggedInUserInitials_Global;
                        //UAT-3951
                        (editMode as ReconciliationItemDataEditMode).ListRejectionReasons = CurrentViewContext.ListRejectionReasons;
                        pnlItems.Controls.Add(editMode);

                        btnSave.Visible = true;

                        complianceVerificationControl.EditModeCntrlsList = editMode;
                        complianceVerificationControl.ComplianceItemID = lstApplicantItemIds[i];
                        complianceVerificationControl.VerificationDataMode = VerificationDataMode.DataEntry;
                        ComplianceVerificationControlsList.Add(complianceVerificationControl);
                    }

                    System.Web.UI.WebControls.HiddenField hdn = new System.Web.UI.WebControls.HiddenField();
                    hdn.ID = "hdnUrl" + tempList[0].ItemName;
                    hdn.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    hdn.Value = tempList[0].SampleDocFormURL;
                    pnlItems.Controls.Add(hdn);
                }

            }
        }

        /// <summary>
        /// Gets the list of editable by properties, for eacb attribute of an item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        private List<ListItemEditableBies> GetItemEditableByProperties(Int32 itemId)
        {
            return this.lstEditableByData
                       .Where(ap => ap.ComplianceItemId == itemId)
                       .ToList();
        }

        /// <summary>
        /// Gets the Assignment properties of current item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        private List<ListItemAssignmentProperties> GetItemAssignmentProperties(Int32 itemId)
        {
            return this.lstAssignmentProperties
                       .Where(ap => ap.ComplianceItemId == itemId)
                       .ToList();
        }

        /// <summary>
        /// Generate XML related to Exception related Items
        /// </summary>
        /// <param name="lstApplicantComplianceItemIds"></param>
        /// <returns></returns>
        private String GenerateItemIdsXML(List<Int32?> lstApplicantComplianceItemIds)
        {
            lstApplicantComplianceItemIds = lstApplicantComplianceItemIds.Distinct().ToList();
            XmlElement child = null;
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("ApplicantComplianceItemIds");
            XmlText value = null;
            xml.AppendChild(root);

            foreach (var applicantComplianceItemId in lstApplicantComplianceItemIds)
            {
                child = xml.CreateElement("ApplicantComplianceItemId");
                value = xml.CreateTextNode(Convert.ToString(applicantComplianceItemId));
                child.AppendChild(value);
                root.AppendChild(child);
            }
            return xml.InnerXml;
        }

        /// <summary>
        /// Generate XML of Items related to Data Entry
        /// </summary>
        /// <param name="lstVerificationData"></param>
        /// <returns></returns>
        private String GenerateAttributeIdsXML(List<ApplicantItemVerificationData> lstVerificationData)
        {
            XmlElement child = null;
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("ApplicantComplianceAttributeIds");
            XmlText value = null;
            xml.AppendChild(root);

            foreach (var verificationData in lstVerificationData)
            {
                child = xml.CreateElement("ApplicantComplianceAttributeId");
                value = xml.CreateTextNode(Convert.ToString(verificationData.ApplAttributeDataId));
                child.SetAttribute("ApplicantComplianceItemId", Convert.ToString(verificationData.ApplicantCompItemId));
                child.AppendChild(value);
                root.AppendChild(child);
            }
            return xml.InnerXml;
        }

        /// <summary>
        /// Filter the Particular item Mappings, for Data entry case
        /// </summary>
        /// <param name="lstAllDocumentMapping"></param>
        /// <param name="applicantComplianceItemId"></param>
        /// <returns></returns>
        private List<ApplicantComplianceDocumentMap> FilterItemDocumentMapping(List<ApplicantDocumentMappingData> lstAllDocumentMapping, Int32 applicantComplianceItemId)
        {
            List<ApplicantComplianceDocumentMap> lstDocumentMapping = new List<ApplicantComplianceDocumentMap>();
            var currentApplicantComplianceItemMapping = lstAllDocumentMapping.Where(map => map.ApplicantComplianceItemId == applicantComplianceItemId
                                                                             && map.IsExceptionDocument == false).ToList();

            foreach (var mapping in currentApplicantComplianceItemMapping)
            {
                lstDocumentMapping.Add(new ApplicantComplianceDocumentMap
                {
                    ApplicantDocumentID = mapping.ApplicantDocumentId,
                    ApplicantComplianceDocumentMapID = mapping.ApplicantComplianceDocumentMapId,
                    ApplicantComplianceAttributeID = mapping.ApplicantComplianceAttributeId,
                });
            }

            return lstDocumentMapping;
        }

        /// <summary>
        /// Filter the Particular item Mappings, for Exception Case
        /// </summary>
        /// <param name="lstAllDocumentMapping"></param>
        /// <param name="applicantComplianceItemId"></param>
        /// <returns></returns>
        private List<ExceptionDocumentMapping> FilterExceptionDocumentMapping(List<ApplicantDocumentMappingData> lstAllDocumentMapping, Int32 applicantComplianceItemId)
        {
            List<ExceptionDocumentMapping> lstDocumentMapping = new List<ExceptionDocumentMapping>();
            var currentApplicantComplianceItemMapping = lstAllDocumentMapping.Where(map => map.ApplicantComplianceItemId == applicantComplianceItemId
                                                                              && map.IsExceptionDocument == true).ToList();

            foreach (var mapping in currentApplicantComplianceItemMapping)
            {
                lstDocumentMapping.Add(new ExceptionDocumentMapping
                {
                    ApplicantDocumentID = mapping.ApplicantDocumentId,
                    ApplicantComplianceItemID = mapping.ApplicantComplianceItemId,
                    ExceptionDocumentMappingID = mapping.ExceptionDocumentMappingId
                });
            }
            return lstDocumentMapping;
        }

        #endregion

        #region Public Methods

        public void RebindComplianceItemPanelData(List<ApplicantItemVerificationData> applicantItmVerificationData)
        {

            #region UAT-523 Category Level Exception
            //UAT-523:Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
            if (IsCategoryLevelException)
            {
                Guid catExceptionItemCode = new Guid(AppConsts.WHOLE_CATEGORY_GUID);//For Dummy item inserted for category level exception "9122D197-27EE-44AC-8CC4-0A68F3D21F32"
                ApplicantItemVerificationData applicantDataCatException = applicantItmVerificationData.FirstOrDefault(cond => cond.CatExceptionItemCode == catExceptionItemCode);
                CategoryLevelExcItemVerificationData = applicantDataCatException;
                if (applicantDataCatException.IsNotNull())
                {
                    SetDataForCategoryException(applicantDataCatException, true);
                }
                applicantItmVerificationData = applicantItmVerificationData.Where(cond => cond.CatExceptionItemCode != catExceptionItemCode).ToList();
            }
            #endregion

            foreach (var item in ComplianceVerificationControlsList)
            {
                var tmVerificationData = applicantItmVerificationData.Where(x => x.ComplianceItemId == item.ComplianceItemID).ToList();
                if (tmVerificationData.IsNotNull())
                {
                    if (item.VerificationDataMode == VerificationDataMode.ReadOnly)
                    {
                        (item.EditModeCntrlsList as ReconciliationItemDataReadOnlyMode).RebindItemDataReadOnlyModeControlsData(tmVerificationData, true);
                    }
                    else if (item.VerificationDataMode == VerificationDataMode.DataEntry)
                    {
                        (item.EditModeCntrlsList as ReconciliationItemDataEditMode).RebindItemDataEditModeControlsData(tmVerificationData, true);
                    }

                }
            }
            btnSave.SaveButton.Visible = false;
        }

        public Boolean GetNewReadOnlyControlAfterSave(VerificationDataMode oldVerificationDataMode, VerificationDataMode newVerificationDataMode)
        {
            if (oldVerificationDataMode == newVerificationDataMode)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        // public void Save(object sender, EventArgs e)
        public void Save(String catExplanatoryNotes, String categoryMoreInfoURL)
        {
            //UAT-718 created a boolean variable to update the queueImaging Data

            Boolean isDataSave = false;
            //List of next Queue Records
            List<ApplicantComplianceItemData> _lstNextQueueRecord = new List<ApplicantComplianceItemData>();
            List<ApplicantComplianceItemData> _lstReviewCompleted = new List<ApplicantComplianceItemData>();
            List<ApplicantComplianceAttributeData> _completeAttributeData = new List<ApplicantComplianceAttributeData>();
            List<ApplicantComplianceItemData> _completeItemData = new List<ApplicantComplianceItemData>();
            String _approved = ApplicantItemComplianceStatus.Approved.GetStringValue();
            String _rejected = ApplicantItemComplianceStatus.Not_Approved.GetStringValue();
            String _exceptionApproved = ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue();
            //String _exceptionApproved = ApplicantItemComplianceStatus.Exception_Approved.GetStringValue();
            String _exceptionRejected = ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue();
            String _expired = ApplicantItemComplianceStatus.Expired.GetStringValue();
            
            String _pendingReviewForClient = ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue();

            #region UAT-1608
            List<ApplicantComplianceItemData> lstDeletedItemsForSeries = new List<ApplicantComplianceItemData>();
            #endregion

            #region GET DATA OF ITEMS FOR DATA ENTRY

            Int32 _controlCount = pnlItems.Controls.Count;

            for (int i = 0; i < _controlCount; i++)
            {
                if (pnlItems.Controls[i] is ReconciliationItemDataEditMode)
                {
                    // Use only items for which status != Incompelete, in the radiobutton selected
                    ReconciliationItemDataEditMode _editModeControl = pnlItems.Controls[i] as ReconciliationItemDataEditMode;
                    _completeAttributeData.AddRange(_editModeControl.GetApplicantItemData().ToList());

                    if (!_editModeControl.IsIncompleteSelected)
                    {
                        _completeItemData.Add(GetItemData(_editModeControl));
                    }
                }
            }

            #endregion

            Dictionary<Int32, String> _dicValidationMessages = _presenter.ValidateApplicantData(_completeItemData, _completeAttributeData);
            if (!_dicValidationMessages.IsNullOrEmpty() && _dicValidationMessages.Count() > 0)
            {
                for (int i = 0; i < _controlCount; i++)
                {
                    if (pnlItems.Controls[i] is ReconciliationItemDataEditMode)
                    {
                        String _validationMessage = "Item could not be validated";
                        ReconciliationItemDataEditMode _editModeControl = pnlItems.Controls[i] as ReconciliationItemDataEditMode;

                        KeyValuePair<Int32, String> _keyValuePair = _dicValidationMessages.Where(vm => vm.Key == _editModeControl.ComplianceItemId).FirstOrDefault();

                        if (!_keyValuePair.IsNullOrEmpty())
                            _validationMessage = _keyValuePair.Value;

                        _editModeControl.SetValidationMessage(_validationMessage);
                    }
                }
            }
            else
            {
                // This list DOES NOT CONTAINS items which are to be deleted. Items to be deleted are in '_lstItemsToDelete'
                List<ApplicantComplianceItemData> _lstItemData = new List<ApplicantComplianceItemData>();

                // Copy the data collected to another list for further processing
                _lstItemData.AddRange(_completeItemData);

                #region Lists used to manage the items

                // Represents the Items for which no status was changed, and were saved as it is (DOES not include Incomplete items)
                List<ApplicantComplianceItemData> _lstNoStatusChanged = new List<ApplicantComplianceItemData>();

                #endregion

                StringBuilder sbErrors = new StringBuilder();

                //TODO
                // STEP 1 - Call the Get Next Action. Include Items for which ApplicantComplianceItemID == 0 and is Exception Items
                //_presenter.GetNextActionForItems(_lstItemData, this.lstAssignmentProperties, out _lstNextReviewRecord, out _lstNextQueueRecord, out _lstEscalationRecord,
                //                                 out _lstIncompleteSameQueue);

                _lstNoStatusChanged = _lstItemData.Where(itmData => itmData.ApplicantComplianceItemID > 0
                                                                    && itmData.AttemptedStatusCode == itmData.CurrentStatusCode).ToList();
                _lstNextQueueRecord = _lstItemData.Where(itmdata => itmdata.AttemptedStatusCode == _pendingReviewForClient).ToList();

                _lstReviewCompleted = _lstItemData.Where(x => x.AttemptedStatusCode == _approved
                                                              || x.AttemptedStatusCode == _rejected
                                                              || x.AttemptedStatusCode == _exceptionApproved
                                                              || x.AttemptedStatusCode == _exceptionRejected).ToList();

                #region UAT - 1763

                List<Int32> lstItems = _lstItemData.Select(col => col.ComplianceItemID).ToList();

                Dictionary<Int32, List<Int32>> dicAdjustItems = new Dictionary<int, List<int>>();

                dicAdjustItems.Add(SelectedComplianceCategoryId_Global, lstItems);

                #endregion


                #region SAVE NORMAL ITEM DATA

                for (int i = 0; i < _controlCount; i++)
                {
                    if (pnlItems.Controls[i] is ReconciliationItemDataEditMode)
                    {
                        ApplicantComplianceItemData _itemData = new ApplicantComplianceItemData();
                        ReconciliationItemDataEditMode _editModeControl = pnlItems.Controls[i] as ReconciliationItemDataEditMode;

                        Boolean _isIncompleteItem = _editModeControl.ApplicantItemDataId == AppConsts.NONE ? true : false;

                        // Use this for deciding whether the current ITEM(which is already added i.e. not Incomplete),
                        // is to be updated or not (depending on next action)
                        String _recordActionType = String.Empty;

                        _recordActionType = CheckItemType(_lstItemData, _lstNextQueueRecord,
                                                          _lstNoStatusChanged,_lstReviewCompleted, _editModeControl.ComplianceItemId);

                        // Save the Item Data
                        String _exceptionMessage = _editModeControl.Save(out _itemData, _recordActionType);
                        if (_exceptionMessage.IsNullOrEmpty())
                            isDataSave = true;
                        if (!_editModeControl.IsIncompleteSelected)
                        {
                            Int32 _applicantComplianceItemId = _itemData.ApplicantComplianceItemID;
                        }

                        if (!String.IsNullOrEmpty(_exceptionMessage))
                        {
                            sbErrors.Append(_exceptionMessage + ", ");
                        }
                        else
                        {
                            //TODO
                            //String sucessMessage = GetSuccessMessage(_lstNextReviewRecord, _lstEscalationRecord, _lstIncompleteSameQueue, _editModeControl.ComplianceItemId, _editModeControl.NewItemStatusText);
                            //if (!String.IsNullOrEmpty(sucessMessage))
                            //{
                            //    _editModeControl.SetSuccessMessage(sucessMessage);
                            //}
                        }
                    }
                }

                #endregion
               

                // Remove records which do not need further reviews or operations as they are rejetced, approved
                _lstNextQueueRecord.RemoveAll(x => _lstReviewCompleted.Select(y => y.ComplianceItemID)
                                                                       .Contains(x.ComplianceItemID));

                // STEP 3 - Clear the settings for Items to be moved to Next queue or next review
                _presenter.ClearQueueRecords();
                
                //TODO
                // STEP 4 - Call Parallel Task after all Save completed
                // Use the items for which New status is not equal to the 4 status of Approve, Reject etc.
                //List<ApplicantComplianceItemData> _lstTempVerificationCompleted = _lstVerificationCompleted.Where(x => x.AttemptedStatusCode != _approved
                //                                                           && x.AttemptedStatusCode != _rejected
                //                                                           && x.AttemptedStatusCode != _exceptionApproved
                //                                                           && x.AttemptedStatusCode != _exceptionRejected
                //                                                           && x.AttemptedStatusCode != _expired  //Added a status check for expired item(UAT-505) to not include in list of GetNextActionForItems.
                //                                                           ).ToList();

                _presenter.CallHandleAssignmentParallelTask(_lstNextQueueRecord);

                #region UAT-1608:Admin Verification Screen Changes regarding Shot Series.
                _presenter.ShufflingOfSeriesItemsData(lstDeletedItemsForSeries, _completeItemData);
                #endregion

                #region UAT - 1763

                Presenter.EvaluateAdjustItemSeriesRules(CurrentPackageSubscriptionID_Global, dicAdjustItems);

                #endregion

                String _userMessage = Convert.ToString(sbErrors);
                Boolean _IsSavedSuccess = true;
                if (!String.IsNullOrEmpty(_userMessage))
                {
                    _userMessage = "Some of the items could not be saved.";
                    _IsSavedSuccess = false;
                }
                else
                {
                    _userMessage = "Item(s) updated successfully.";
                    _IsSavedSuccess = true;
                }
                //UAT 537 Verification Details Screen "go to Next Pending for Review Category" should save data and button text change.
                IsDataSavedSuccessfully = _IsSavedSuccess;

                //UAT-718 
                if (isDataSave)
                {
                    Presenter.SetQueueImaging();
                }

                //UAT-3805
                if (IsDataSavedSuccessfully)
                {
                    Presenter.SendItemDocNotificationToAgencyUser(new List<Int32>(), CurrentViewContext.SelectedComplianceCategoryId_Global, true);
                }

                var applicantItemVerificationDataList = _presenter.GetApplicantItemVerificationData();

                RebindComplianceItemPanelData(applicantItemVerificationDataList);

                if (applicantItemVerificationDataList.IsNotNull())
                {
                    var catComplianceStatusName = String.Empty;
                    var catComplianceStatus = String.Empty;
                    if (applicantItemVerificationDataList.FirstOrDefault().CatComplianceStatusCode == ApplicantCategoryComplianceStatus.Incomplete.GetStringValue()
                        && !String.IsNullOrEmpty(CategoryLevelExcItemVerificationData.CatExceptionStatusCode)
                        && CategoryLevelExcItemVerificationData.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPLIED.GetStringValue())
                    {
                        catComplianceStatusName = "Pending Review";
                        catComplianceStatus = ApplicantCategoryComplianceStatus.Pending_Review.GetStringValue();
                    }
                    else
                    {
                        catComplianceStatusName = applicantItemVerificationDataList.FirstOrDefault().CatComplianceStatus;
                        catComplianceStatus = applicantItemVerificationDataList.FirstOrDefault().CatComplianceStatusCode;
                    }

                    var imgUrl = GetImageUrl(catComplianceStatus);
                    String packageStatusImgUrl = String.Empty;

                    var packageSubscription = Presenter.GetPackageSubscriptionByPackageID();
                    if (packageSubscription.IsNotNull())
                    {
                        if (packageSubscription.lkpPackageComplianceStatu.Code.ToLower() == ApplicantPackageComplianceStatus.Not_Compliant.GetStringValue().ToLower())
                        {
                            packageStatusImgUrl = "/Resources/Mod/Compliance/icons/no16.png";
                        }
                        else
                        {
                            packageStatusImgUrl = "/Resources/Mod/Compliance/icons/yes16.png";
                        }
                    }

                    var catComplianceCatID = applicantItemVerificationDataList.FirstOrDefault().ComplianceCatId;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), Convert.ToString(Guid.NewGuid()),
                                                        "ChangeApplicantPanelData('" + imgUrl + "','" + catComplianceStatusName + "','" + packageStatusImgUrl
                                                        + "','" + packageSubscription.lkpPackageComplianceStatu.Code.ToLower()
                                                        + "','" + _IsSavedSuccess
                                                        + "');", true);
                }
            }

            //Contract to add mail content for item, Escalated Item and Exception Item rejected.
            List<ExceptionRejectionContract> lstExceptionRejectionContract = new List<ExceptionRejectionContract>();
            Int32 tenantId = CurrentViewContext.SelectedTenantId_Global;

            //Add contract for Item and Exception Item.
            if (_lstReviewCompleted.IsNotNull() && _lstReviewCompleted.Count > 0)
            {
                String instituteUrl = Presenter.GetInstitutionUrl(tenantId);

                foreach (var item in _lstReviewCompleted)
                {
                    if (item.AttemptedStatusCode != item.CurrentStatusCode && ((item.AttemptedStatusCode == ApplicantItemComplianceStatus.Not_Approved.GetStringValue())
                        || (item.AttemptedStatusCode == ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue())))
                    {
                        ExceptionRejectionContract excRejContract = new ExceptionRejectionContract();
                        excRejContract.UserFullName = item.ApplicantName;
                        excRejContract.ApplicationUrl = instituteUrl;
                        excRejContract.ApplicantID = CurrentViewContext.SelectedApplicantId_Global;
                        excRejContract.RejectionReason = String.IsNullOrEmpty(item.VerificationComments) ? "N/A" : item.VerificationComments;
                        excRejContract.HierarchyNodeID = item.HierarchyNodeId;
                        excRejContract.ItemID = item.ComplianceItemID;
                        //UAT-1519: Add the ability to include the Category Explanatory note in the Item rejected email notification.
                        excRejContract.CategoryExplanatoryNotes = catExplanatoryNotes;
                        excRejContract.CategoryMoreInfoURL = categoryMoreInfoURL;

                        lstExceptionRejectionContract.Add(excRejContract);
                    }
                }
            }

            if (lstExceptionRejectionContract.IsNotNull() && lstExceptionRejectionContract.Count > 0)
            {
                String primaryEmail = Presenter.GetApplicantPrimaryEmail(lstExceptionRejectionContract[0].ApplicantID);
                //UAT-2172: Added a new parameter CurrentLoggedInUserId in existing method
                Presenter.CallParallelTaskForMail(lstExceptionRejectionContract, primaryEmail, tenantId, CurrentLoggedInUserId);
            }
        }

        /// <summary>
        ///  Get data related to XML of 'HandleAssignment' SP in 'Queue Framework'
        /// </summary>
        /// <param name="_editModeControl"></param>
        /// <param name="itemToDelete"></param>
        /// <returns></returns>
        private static ApplicantComplianceItemData GetItemData(ReconciliationItemDataEditMode _editModeControl, Boolean itemToDelete = false)
        {
            ApplicantComplianceItemData _itemData = new ApplicantComplianceItemData();

            _itemData.ApplicantComplianceItemID = _editModeControl.ApplicantItemDataId;
            _itemData.StatusID = Convert.ToInt32(_editModeControl.VerificationData[0].ItemComplianceStatusId);
            _itemData.CurrentStatusCode = _editModeControl.CurrentItemStatus;

            if (!itemToDelete)
            {
                _itemData.ApplicantName = _editModeControl.VerificationData[0].ApplicantName;
                _itemData.ComplianceItemID = _editModeControl.ComplianceItemId;
                _itemData.SubmissionDate = _editModeControl.VerificationData[0].SubmissionDate;

                _itemData.SystemStatusText = _editModeControl.VerificationData[0].SystemStatus;
                _itemData.RushOrderStatusCode = _editModeControl.VerificationData[0].RushOrderStatusCode;

                _itemData.HierarchyNodeId = Convert.ToInt32(_editModeControl.VerificationData[0].HierarchyNodeID);
                _itemData.ComplianceItemName = Convert.ToString(_editModeControl.VerificationData[0].ItemName);
                _itemData.ComplianceCategoryName = Convert.ToString(_editModeControl.VerificationData[0].CatName);
                _itemData.CompliancePackageName = _editModeControl.VerificationData[0].PackageName;
                _itemData.VerificationStatusText = _editModeControl.CurrentItemStatusText;
                _itemData.RushOrderStatusText = _editModeControl.VerificationData[0].RushOrderStatus;
                _itemData.AttemptedItemStatusId = _editModeControl.AttemptedItemStatusId;
                _itemData.IsFileUploadApplicable = _editModeControl.IsFileUpoloadApplicable;

                _itemData.AttemptedStatusCode = _editModeControl.AttemptedItemStatus;
                _itemData.CurrentTenantTypeCode = _editModeControl.CurrentTenantTypeCode;
                _itemData.IsExceptionTypeItem = false;
                _itemData.IsEscalatedItem = false;
                if (_itemData.IsFileUploadApplicable)
                    _itemData.FileUploadAttributeId = _editModeControl.FileUpoladAttributeId;
                _itemData.VerificationComments = _editModeControl.AdminComments;
                //_itemData.VerificationComments = _editModeControl.ExceptionComments;
            }

            return _itemData;
        }

        /// <summary>
        /// Check for the Queue Type action applicable on the Item.
        /// If item is Incomplete, moving in the same queue(Next review), then treat it similar to 'Next-Review' required
        /// </summary>
        /// <param name="lstNextReviewRecord"></param>
        /// <param name="lstNextQueueRecord"></param>
        /// <param name="lstEscalationRecord"></param>
        /// <param name="lstIncompleteSameQueue">This is required, as Incomplete items moving in Same queue, are not available in other 3 lists being used </param>
        /// <param name="complianceItemId"></param>
        /// <returns></returns>
        private String CheckItemType(List<ApplicantComplianceItemData> _lstItemData, List<ApplicantComplianceItemData> lstNextQueueRecord,
                                     List<ApplicantComplianceItemData> lstNoStatusChanged, List<ApplicantComplianceItemData> _lstReviewCompleted, Int32 complianceItemId)
        {
            String _actionType = String.Empty;

            if (lstNextQueueRecord.Where(x => x.ComplianceItemID == complianceItemId).Any()
                ||_lstReviewCompleted.Where(x => x.ComplianceItemID == complianceItemId).Any())
                _actionType = lkpQueueActionType.Proceed_To_Next_Queue.GetStringValue();

            else if (lstNoStatusChanged.Where(x => x.ComplianceItemID == complianceItemId).Any())
                _actionType = lkpQueueActionType.No_Status_Changed.GetStringValue();

            return _actionType;
        }

        protected string GetImageUrl(string status)
        {
            string url = "";

            if (status.IsNullOrEmpty() || status == ApplicantCategoryComplianceStatus.Incomplete.GetStringValue())
            {
                url = ResolveUrl("~/Resources/Mod/Compliance/icons/no16.png");
            }
            else if (status == ApplicantCategoryComplianceStatus.Approved.GetStringValue())
            {
                url = ResolveUrl("~/Resources/Mod/Compliance/icons/yes16.png");
            }
            else if (status == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue())
            {
                url = ResolveUrl("~/Resources/Mod/Compliance/icons/yesx16.png");
            }
            else if (status == ApplicantCategoryComplianceStatus.Pending_Review.GetStringValue())
            {
                url = ResolveUrl("~/Resources/Mod/Compliance/icons/attn16.png");
            }
            return url;
        }

        /// <summary>
        /// Get Sucess Message
        /// </summary>
        /// <param name="lstNextReviewRecord">List of NextReviewRecord</param>
        /// <param name="lstEscalationRecord">List of EscalationRecord</param>
        /// <param name="complainceItemID">ComplainceItemID</param>
        /// <param name="status">Item Staus</param>
        /// <returns>Sucess Message</returns>
        private String GetSuccessMessage(List<ApplicantComplianceItemData> lstNextReviewRecord, List<ApplicantComplianceItemData> lstEscalationRecord,
          List<ApplicantComplianceItemData> lstIncompleteSameQueue, Int32 complainceItemID, String status)
        {
            String successMessage = String.Empty;

            if (lstNextReviewRecord.Where(condition => condition.ComplianceItemID == complainceItemID).Any()
                || lstEscalationRecord.Where(condition => condition.ComplianceItemID == complainceItemID).Any()
                || lstIncompleteSameQueue.Where(condition => condition.ComplianceItemID == complainceItemID).Any())
            {
                successMessage = "Your changes were saved with status " + status + ".";
            }
            return successMessage;
        }

        private void UpdateApplicantItemDataId(List<ApplicantComplianceItemData> lstIncompleteSameQueue,
            Int32 complianceItemId, Int32 applicantComplianceItemId)
        {
            ApplicantComplianceItemData _applicantComplianceItemData = lstIncompleteSameQueue.Where(x => x.ComplianceItemID == complianceItemId).FirstOrDefault();
            if (!_applicantComplianceItemData.IsNullOrEmpty())
            {
                _applicantComplianceItemData.ApplicantComplianceItemID = applicantComplianceItemId;
            }
        }

        /// <summary>
        /// Check for exception Expired or not.
        /// </summary>
        /// <param name="itemData"></param>
        /// <returns></returns>
        private Boolean IsExceptionExpired(String currentStatus, Int32? itemMovementTypeId)
        {
            if (currentStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Expired.GetStringValue().ToLower().Trim() && itemMovementTypeId.IsNull())
            {
                return true;
            }
            return false;
        }

        #endregion

        #endregion

        #region UAT-523 Exception At CategoryLevel
        //UAT-523:Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
        /// <summary>
        /// Method to bind the controls data for category exception 
        /// </summary>
        /// <param name="applicantDataCatException">Category exception data object</param>
        private void SetDataForCategoryException(ApplicantItemVerificationData applicantDataCatException, Boolean isRebindDataAfterSave = false)
        {
            String _xmlApplicantComplianceItemDataIds = String.Empty;
            List<ApplicantDocumentMappingData> _lstDocumentMappingList = new List<ApplicantDocumentMappingData>();
            if (!applicantDataCatException.IsNullOrEmpty() && IsCategoryLevelException)
            {
                String _tenantTypeCode = Presenter.GetTenantType();
                pnlCategoryException.Visible = true;
                dpExpiDateReadOnly.SelectedDate = applicantDataCatException.CatExpirationDate;
                hdnExpirationDate.Value = applicantDataCatException.CatExpirationDate.IsNullOrEmpty() ? String.Empty : Convert.ToString(applicantDataCatException.CatExpirationDate);
                //UAT-819: WB: Category Exception enhancements
                litExceptionReason.Text = applicantDataCatException.ExceptionReason;
                HideShowCategoryExceptionControl(true);
                //UAT-959 
                if (applicantDataCatException.CatExceptionStatusCode.IsNotNull() && applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPLIED.GetStringValue()
                    && applicantDataCatException.CatComplianceStatusCode == ApplicantCategoryComplianceStatus.Incomplete.GetStringValue())
                {
                    litCurrentCategoryStatus.Text = "Pending Review";
                }
                else
                {
                    litCurrentCategoryStatus.Text = applicantDataCatException.CatComplianceStatus;
                }
                if ((applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPLIED.GetStringValue() || applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPROVED.GetStringValue()))
                {
                    if (applicantDataCatException.CatComplianceStatusCode == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue() || applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPROVED.GetStringValue())
                    {
                        litCategoryExceptionStatus.Text = "Approved With Exception";
                    }
                    else
                    {
                        litCategoryExceptionStatus.Text = "Applied For Exception";
                    }
                }
                else
                {
                    if (applicantDataCatException.CatComplianceStatusCode == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue() || applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPROVED.GetStringValue())
                    {
                        litCategoryExceptionStatus.Text = "Approved With Exception";
                    }
                    else
                    {
                        litCategoryExceptionStatus.Text = "Applied For Exception";
                    }
                    if (isRebindDataAfterSave)
                        HideShowCategoryExceptionControl(true);
                }
                if ((applicantDataCatException.CatComplianceStatusCode == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue() || applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPROVED.GetStringValue()))
                {
                    divExpiDateReadOnly.Style["Display"] = "block";
                }
                else
                {
                    divExpiDateReadOnly.Style["Display"] = "none";
                }
                List<Int32?> _lstApplicantComplianceItemIds = new List<int?>();
                _lstApplicantComplianceItemIds.Add(applicantDataCatException.ApplicantCompItemId);
                _xmlApplicantComplianceItemDataIds = GenerateItemIdsXML(_lstApplicantComplianceItemIds);
                if (!String.IsNullOrEmpty(_xmlApplicantComplianceItemDataIds))
                {
                    _lstDocumentMappingList = Presenter.GetApplicantDocumentsData(_xmlApplicantComplianceItemDataIds, String.Empty);
                }

                hdfCatRejectionCodeException.Value = ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue();

            }
        }

        /// <summary>
        /// Show Hide UI controls for Category Level Exception
        /// </summary>
        /// <param name="isReadOnly">isReadOnly</param>
        private void HideShowCategoryExceptionControl(Boolean isReadOnly)
        {
            if (isReadOnly)
            {
                divExpiDateReadOnly.Style["Display"] = "block";
            }
            else
            {
                divExpiDateReadOnly.Style["Display"] = "none";
            }
        }
        #endregion

    }

}

