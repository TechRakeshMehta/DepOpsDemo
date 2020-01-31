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
using INTSOF.UI.Contract.QueueManagement;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ItemDataLoader : BaseUserControl, IItemDataLoaderView
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
        //private List<UserSpecializationDetails> _lstUserSpecializations;
        private ItemDataLoaderPresenter _presenter = new ItemDataLoaderPresenter();

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #region UAT-1812:Creation of an Approval/rejection summary for applicant logins
        List<ApplicantComplianceItemData> IItemDataLoaderView.lstNotApprovedItems { get; set; }
        #endregion
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

        public WorkQueueType WorkQueue
        {
            get
            {
                if (ViewState["WorkQueue"] != null)
                    return (WorkQueueType)ViewState["WorkQueue"];
                return WorkQueueType.AssignmentWorkQueue;
            }
            set
            {
                ViewState["WorkQueue"] = value;
            }
        }

        public List<ApplicantItemVerificationData> lst
        {
            get;
            set;
        }

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

        public IItemDataLoaderView CurrentViewContext
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

        public ItemDataLoaderPresenter Presenter
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

        /// <summary>
        /// List of current Items assigned to the user
        /// </summary>
        public List<UserCurrentAssignments> lstCurrentAssignments
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
        #region UAT-523 Category exception
        public String ApprovedWithExcepStatusCode
        {
            get
            {
                return ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue();
            }
        }

        public VerificationDataMode CategoryExceptionControlMode
        {
            get;
            set;
        }
        public String MinExpirationDate { get { return (DateTime.Now.AddDays(1)).ToShortDateString(); } }

        #region UAT-3951:Addition of option to use preset ADB Admin rejection notes
        public String NotApprovedStatusCode
        {
            get
            {
                return ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue();
            }
        }
        public List<Entity.RejectionReason> ListRejectionReasons { get; set; }

        List<Int32> IItemDataLoaderView.SelectedRejectionReasonIds
        {
            get
            {
                if (!hdnSelectedRejectionReasonIDsCatExcep.Value.IsNullOrEmpty())
                {
                    return hdnSelectedRejectionReasonIDsCatExcep.Value.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                }
                return null;
            }
        }
        #endregion
        #endregion

        #region UAT-850: WB: As an admin, I should be able to Delete a category exception/exception request
        public Boolean IsDeleteCategoryExceptionChecked
        {
            get { return chkDeleteCatException.Checked; }
        }

        public Boolean IsDeleteOverideStatusChecked
        {
            get { return chkDeleteOverideStatus.Checked; }
        }
        #endregion

        #region UAT-845 Creation of admin override function (verification details).
        public String ApprovedCatOverrideStatusCode
        {
            get
            {
                return lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue();
            }
        }
        #endregion

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
        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<String, String> args = new Dictionary<string, string>();
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey("WorkQueueType"))
            {
                CurrentViewContext.WorkQueue = (WorkQueueType)Enum.Parse(typeof(WorkQueueType), args["WorkQueueType"].ToString(), true);
            }

            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();

            }
            Presenter.OnViewLoaded();

            LoadControls();
            viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            (btnSave as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click here to save all changes made to the Items in this Category";

            //UAT-3951:
            BindRejectionReasons();
            ShowHideRejectionReasonControl();
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
                // this.lstEditableByData = Presenter.GetCategoryLevelEditableBies(); //--- STEP 1
                this.lstEditableByData = _presenter.GetEditableBiesByCategoryId();
                this.lstAssignmentProperties = _presenter.GetAssignmentPropertiesByCategoryId();
                this.lstCurrentAssignments = _presenter.GetCurrentAssignments();

                List<ApplicantItemVerificationData> _temp = lst.GroupBy(d => d.ComplianceItemId).Select(g => g.First()).ToList();
                //this._lstUserSpecializations = _presenter.GetUserSpecializations(_temp, _tenantTypeCode);

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
                else
                {
                    ucVerificationDetailsDocumentConrol.PackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionID_Global;
                    ucVerificationDetailsDocumentConrol.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
                    ucVerificationDetailsDocumentConrol.ApplicantId = CurrentViewContext.SelectedApplicantId_Global;
                    ucVerificationDetailsDocumentConrol.ItemDataId = 0;
                    ucVerificationDetailsDocumentConrol.ComplianceItemId = 0;
                    ucVerificationDetailsDocumentConrol.IsReadOnly = false;
                    hdfCatRejectionCodeException.Value = String.Empty;


                    //------------------------------------------Un- Assigned Documents-----------------------------
                    ucVerificationDetailsUnassignedDocumentConrol.PackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionID_Global;
                    ucVerificationDetailsUnassignedDocumentConrol.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
                    ucVerificationDetailsUnassignedDocumentConrol.ApplicantId = CurrentViewContext.SelectedApplicantId_Global;
                    ucVerificationDetailsUnassignedDocumentConrol.ItemDataId = 0;
                    ucVerificationDetailsUnassignedDocumentConrol.ComplianceItemId = 0;
                    ucVerificationDetailsUnassignedDocumentConrol.IsReadOnly = false;
                    hdfCatRejectionCodeException.Value = String.Empty;
                    ucVerificationDetailsUnassignedDocumentConrol.ComplianceCategoryId = CurrentViewContext.SelectedComplianceCategoryId_Global;
                    ucVerificationDetailsUnassignedDocumentConrol.lstApplicantDocument = CurrentViewContext.lstApplicantDocument;
                    //--------------------------------------------------------------------------------
                }
                lst = lst.Where(cond => cond.CatExceptionItemCode != catExceptionItemCode).ToList();
                //UAT-845 Creation of admin override function (verification details).
                if (!IsCategoryLevelException)
                {
                    ApplicantItemVerificationData applicantDataCatOverride = lst.FirstOrDefault();
                    CategoryLevelExcItemVerificationData = applicantDataCatOverride;
                    CategoryOverride(applicantDataCatOverride);
                }


                #endregion

                List<Int32> lstApplicantItemIds = lst.Select(verData => verData.ComplianceItemId).Distinct().ToList(); //Sumit:  Done Order By in SP .OrderBy(x => x.ComplianceItemId)

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

                    #region Check if Item is Escalated Item or not

                    Boolean _isEscalatedItem = false;

                    // If item is not Incomplete
                    //if (_itemDataId.IsNotNull() && _itemDataId > AppConsts.NONE)
                    //{
                    //    String _escalationCode = tempList[0].EscalationCode;
                    //    if (!String.IsNullOrEmpty(_escalationCode) && _escalationCode == lkpQueueEscalationType.Escalated.GetStringValue())
                    //    {
                    //        _isEscalatedItem = true;
                    //    }
                    //}

                    #endregion

                    VerificationDataMode controlMode = ControlModeType(_itemDataId, _currentItemStatus, _assignedToUserId,
                        _reviewerTenantId, _subscriptionMobilityStatusCode, _tenantTypeCode, Convert.ToInt32(lstApplicantItemIds[i]), _isEscalatedItem, _itemMovementTypeId);

                    Int32? unifiedDocumentPageId = 0;
                    var UnifiedDocument = CurrentViewContext.lstApplicantDocument.Where(cond => cond.ItemID == _itemDataId).ToList();
                    if (UnifiedDocument.IsNotNull() && UnifiedDocument.Count() > 0)
                    {
                        unifiedDocumentPageId = UnifiedDocument.FirstOrDefault().UnifiedDocumentStartPageID;
                    }

                    if (controlMode == VerificationDataMode.ReadOnly)
                    {
                        System.Web.UI.Control readOnlyMode = Page.LoadControl("~/ComplianceOperations/UserControl/ItemDataReadOnlyMode.ascx");
                        (readOnlyMode as ItemDataReadOnlyMode).VerificationData = tempList;
                        (readOnlyMode as ItemDataReadOnlyMode).IsItmEditableByApplcnt = (lstEditableByData.Where(cond => cond.ComplianceItemId == lstApplicantItemIds[i]).Any(cond => cond.EditableByCode == "EDTAPCT")) ? true : false; //UAT-3599
                        (readOnlyMode as ItemDataReadOnlyMode).FormMode = _currentItemStatus;
                        (readOnlyMode as ItemDataReadOnlyMode).SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
                        (readOnlyMode as ItemDataReadOnlyMode).CurrentPackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionID_Global;
                        (readOnlyMode as ItemDataReadOnlyMode).lstApplicantDocument = CurrentViewContext.lstApplicantDocument;
                        (readOnlyMode as ItemDataReadOnlyMode).UnifiedDocumentStartPageID = unifiedDocumentPageId;
                        (readOnlyMode as ItemDataReadOnlyMode).lstApplicantComplianceDocumentMaps = FilterItemDocumentMapping(_lstDocumentMappingList, Convert.ToInt32(_itemDataId));

                        pnlItems.Controls.Add(readOnlyMode);

                        complianceVerificationControl.EditModeCntrlsList = readOnlyMode;
                        complianceVerificationControl.ComplianceItemID = lstApplicantItemIds[i];
                        complianceVerificationControl.VerificationDataMode = controlMode;
                        ComplianceVerificationControlsList.Add(complianceVerificationControl);
                    }
                    else if (controlMode == VerificationDataMode.Exception)
                    {
                        System.Web.UI.Control exceptionMode = Page.LoadControl("~/ComplianceOperations/UserControl/ItemDataExceptionMode.ascx");

                        if (lst.Where(x => x.ComplianceItemId == lstApplicantItemIds[i]).FirstOrDefault().ApplicantCompItemId.IsNotNull())
                        {
                            Int32 currentApplicantComplianceItemID = lst.Where(x => x.ComplianceItemId == lstApplicantItemIds[i]).FirstOrDefault().ApplicantCompItemId.Value;
                            if (IsCurrentItemAssigned(currentApplicantComplianceItemID))
                            {
                                (exceptionMode as ItemDataExceptionMode).IsItemAsisgnedToCurrentUser = true;
                            }
                            else
                            {
                                (exceptionMode as ItemDataExceptionMode).IsItemAsisgnedToCurrentUser = false;
                            }
                        }

                        (exceptionMode as ItemDataExceptionMode).VerificationData = tempList;
                        (exceptionMode as ItemDataExceptionMode).IsItmEditableByApplcnt = (lstEditableByData.Where(cond => cond.ComplianceItemId == lstApplicantItemIds[i]).Any(cond => cond.EditableByCode == "EDTAPCT")) ? true : false; //UAT-3599
                        (exceptionMode as ItemDataExceptionMode).CurrentTenantId_Global = CurrentViewContext.CurrentTenantId_Global;
                        (exceptionMode as ItemDataExceptionMode).SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
                        (exceptionMode as ItemDataExceptionMode).SelectedCompliancePackageId_Global = CurrentViewContext.SelectedCompliancePackageId_Global;
                        (exceptionMode as ItemDataExceptionMode).SelectedComplianceCategoryId_Global = CurrentViewContext.SelectedComplianceCategoryId_Global;
                        (exceptionMode as ItemDataExceptionMode).SelectedApplicantId_Global = CurrentViewContext.SelectedApplicantId_Global;
                        (exceptionMode as ItemDataExceptionMode).SelectedCompliancePackageId_Global = CurrentViewContext.SelectedCompliancePackageId_Global;
                        (exceptionMode as ItemDataExceptionMode).CurrentPackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionID_Global;
                        (exceptionMode as ItemDataExceptionMode).lstApplicantDocument = CurrentViewContext.lstApplicantDocument;
                        (exceptionMode as ItemDataExceptionMode).lstAssignmentProperties = this.lstAssignmentProperties;
                        (exceptionMode as ItemDataExceptionMode).UnifiedDocumentStartPageID = unifiedDocumentPageId;
                        (exceptionMode as ItemDataExceptionMode).CurrentTenantTypeCode = _tenantTypeCode;

                        (exceptionMode as ItemDataExceptionMode).lstExceptionDocumentDocumentMaps = FilterExceptionDocumentMapping(_lstDocumentMappingList, Convert.ToInt32(_itemDataId));

                        //UAT-3951
                        (exceptionMode as ItemDataExceptionMode).ListRejectionReasons = CurrentViewContext.ListRejectionReasons;
                        pnlItems.Controls.Add(exceptionMode);
                        btnSave.Visible = true;

                        complianceVerificationControl.EditModeCntrlsList = exceptionMode;
                        complianceVerificationControl.ComplianceItemID = lstApplicantItemIds[i];
                        complianceVerificationControl.VerificationDataMode = controlMode;
                        ComplianceVerificationControlsList.Add(complianceVerificationControl);
                    }
                    else
                    {
                        System.Web.UI.Control editMode = Page.LoadControl("~/ComplianceOperations/UserControl/ItemDataEditMode.ascx");

                        if (lst.Where(x => x.ComplianceItemId == lstApplicantItemIds[i]).FirstOrDefault().ApplicantCompItemId.IsNotNull())
                        {
                            Int32 currentApplicantComplianceItemID = lst.Where(x => x.ComplianceItemId == lstApplicantItemIds[i]).FirstOrDefault().ApplicantCompItemId.Value;
                            if (IsCurrentItemAssigned(currentApplicantComplianceItemID))
                            {
                                (editMode as ItemDataEditMode).IsItemAsisgnedToCurrentUser = true;
                            }
                            else
                            {
                                (editMode as ItemDataEditMode).IsItemAsisgnedToCurrentUser = false;
                            }
                        }

                        (editMode as ItemDataEditMode).ApplicantItemDataId = Convert.ToInt32(_itemDataId);
                        (editMode as ItemDataEditMode).VerificationData = tempList;
                        (editMode as ItemDataEditMode).IsItmEditableByApplcnt = (lstEditableByData.Where(cond => cond.ComplianceItemId == lstApplicantItemIds[i]).Any(cond => cond.EditableByCode == "EDTAPCT")) ? true : false; //UAT-3599
                        (editMode as ItemDataEditMode).SelectedComplianceCategoryId_Global = CurrentViewContext.SelectedComplianceCategoryId_Global;
                        (editMode as ItemDataEditMode).SelectedCompliancePackageId_Global = CurrentViewContext.SelectedCompliancePackageId_Global;
                        //(editMode as ItemDataEditMode).PackageId = CurrentViewContext.PackageId_Global;
                        //(editMode as ItemDataEditMode).ComplianceCategoryId = CurrentViewContext.CategoryId_Global;                        

                        (editMode as ItemDataEditMode).SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
                        (editMode as ItemDataEditMode).SelectedApplicantId_Global = CurrentViewContext.SelectedApplicantId_Global;
                        (editMode as ItemDataEditMode).CurrentLoggedInUserName_Global = CurrentViewContext.CurrentLoggedInUserName_Global;
                        (editMode as ItemDataEditMode).CurrentTenantId_Global = CurrentViewContext.CurrentTenantId_Global;
                        (editMode as ItemDataEditMode).CurrentPackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionID_Global;
                        (editMode as ItemDataEditMode).lstEditableByData = GetItemEditableByProperties(lstApplicantItemIds[i]);  //--- STEP 2
                        (editMode as ItemDataEditMode).lstAssignmentProperties = GetItemAssignmentProperties(lstApplicantItemIds[i]);  //--- STEP 2
                        (editMode as ItemDataEditMode).lstApplicantDocument = CurrentViewContext.lstApplicantDocument;
                        (editMode as ItemDataEditMode).UnifiedDocumentStartPageID = unifiedDocumentPageId;

                        (editMode as ItemDataEditMode).lstApplicantComplianceDocumentMaps = FilterItemDocumentMapping(_lstDocumentMappingList, Convert.ToInt32(_itemDataId));
                        (editMode as ItemDataEditMode).CurrentTenantTypeCode = _tenantTypeCode;
                        (editMode as ItemDataEditMode).LoggedInUserInitials_Global = CurrentViewContext.LoggedInUserInitials_Global;

                        //UAT-3951
                        (editMode as ItemDataEditMode).ListRejectionReasons = CurrentViewContext.ListRejectionReasons;
                        pnlItems.Controls.Add(editMode);

                        btnSave.Visible = true;

                        complianceVerificationControl.EditModeCntrlsList = editMode;
                        complianceVerificationControl.ComplianceItemID = lstApplicantItemIds[i];
                        complianceVerificationControl.VerificationDataMode = controlMode;
                        ComplianceVerificationControlsList.Add(complianceVerificationControl);
                    }

                    System.Web.UI.WebControls.HiddenField hdn = new System.Web.UI.WebControls.HiddenField();
                    hdn.ID = "hdnUrl" + tempList[0].ItemName;
                    hdn.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    hdn.Value = tempList[0].SampleDocFormURL;
                    pnlItems.Controls.Add(hdn);

                    //#region UAT-1056
                    //if(IsCurrentItemAssigned(Convert.ToInt32(_lstVerificationData.Where(x=>x.ComplianceItemId == lstApplicantItemIds[i]).FirstOrDefault().ApplicantCompItemId)))
                    //{
                    //    AssignedItemIDs.Add(lstApplicantItemIds[i]);    
                    //}
                    //#endregion
                }

                #region UAT-523 Category Exception
                //UAT-523:Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
                if (IsCategoryLevelException && CategoryExceptionControlMode != VerificationDataMode.ReadOnly)
                {
                    btnSave.Visible = true;
                }
                #endregion
            }
        }


        public void LoadVerificationDetailsUnassignedDocumentConrol()
        {
            //------------------------------------------Un- Assigned Documents-----------------------------
            ucVerificationDetailsUnassignedDocumentConrol.PackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionID_Global;
            ucVerificationDetailsUnassignedDocumentConrol.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
            ucVerificationDetailsUnassignedDocumentConrol.ApplicantId = CurrentViewContext.SelectedApplicantId_Global;
            ucVerificationDetailsUnassignedDocumentConrol.ItemDataId = 0;
            ucVerificationDetailsUnassignedDocumentConrol.ComplianceItemId = 0;
            ucVerificationDetailsUnassignedDocumentConrol.IsReadOnly = false;
            hdfCatRejectionCodeException.Value = String.Empty;
            ucVerificationDetailsUnassignedDocumentConrol.ComplianceCategoryId = CurrentViewContext.SelectedComplianceCategoryId_Global;
            ucVerificationDetailsUnassignedDocumentConrol.lstApplicantDocument = CurrentViewContext.lstApplicantDocument;
            ucVerificationDetailsUnassignedDocumentConrol.BindDocumentList(true);
            //--------------------------------------------------------------------------------
        }
        /// <summary>
        /// Check whether the current Item control should be ReadOnly, Data-Entry or Exception type
        /// </summary>
        /// <param name="applicantItemDataId"></param>
        /// <param name="currentItemStatus"></param>
        /// <param name="assignedToUserId"></param>
        /// <param name="reviewerTenantId"></param>
        /// <param name="subscriptionMobilityStatusCode"></param>
        /// <param name="_tenantTypeCode"></param>
        /// <param name="complianceItemId"></param>
        /// <returns></returns>
        private VerificationDataMode ControlModeType(Int32? applicantItemDataId, String currentItemStatus, Int32? assignedToUserId,
            Int32? reviewerTenantId, String subscriptionMobilityStatusCode, String _tenantTypeCode, Int32 complianceItemId, Boolean isEscalatedItem, Int32? itemMovementTypeId)
        {
            //Below code is commented for ticket UAT -1056 - Ability to enter data for all categories from user work queue.
            //if (CurrentViewContext.WorkQueue == WorkQueueType.UserWorkQueue && assignedToUserId.IsNotNull() && assignedToUserId != CurrentLoggedInUserId)
            //    return VerificationDataMode.ReadOnly; 

            // Case of Admin
            if (CurrentViewContext.IsDefaultTenant)
            {
                if (IsReadOnlyForAdmin(applicantItemDataId, currentItemStatus, subscriptionMobilityStatusCode, complianceItemId, isEscalatedItem))
                    return VerificationDataMode.ReadOnly;
                else if (!String.IsNullOrEmpty(currentItemStatus)
                    && (currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue().ToLower().Trim()
                    || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue().ToLower().Trim()
                    || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue().ToLower().Trim()
                    || IsExceptionExpired(currentItemStatus, itemMovementTypeId))) //UAT-505
                    return VerificationDataMode.Exception;
                else if (String.IsNullOrEmpty(currentItemStatus)
                    || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Pending_Review.GetStringValue().ToLower().Trim()
                    || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Incomplete.GetStringValue().ToLower().Trim()
                    || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue().ToLower().Trim()
                    || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue().ToLower().Trim()
                    || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Approved.GetStringValue().ToLower().Trim()
                    || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Not_Approved.GetStringValue().ToLower().Trim() //UAT 373
                    || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Expired.GetStringValue().ToLower().Trim())//UAT-505

                    return VerificationDataMode.DataEntry;

            }
            else if (_tenantTypeCode.ToLower() == TenantType.Institution.GetStringValue().ToLower()) // Case of Client Admin
            {
                if (IsReadOnlyForClientAdmin(applicantItemDataId, currentItemStatus, subscriptionMobilityStatusCode, complianceItemId, isEscalatedItem))
                    return VerificationDataMode.ReadOnly;
                else if (String.IsNullOrEmpty(currentItemStatus)
                    || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Incomplete.GetStringValue().ToLower().Trim()
                    || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue().ToLower().Trim()
                    || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue().ToLower().Trim()
                    || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Approved.GetStringValue().ToLower().Trim() //UAT 373
                    || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Not_Approved.GetStringValue().ToLower().Trim() //UAT 373
                    || (currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Expired.GetStringValue().ToLower().Trim() && !IsExceptionExpired(currentItemStatus, itemMovementTypeId))//UAT-505
                    || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Pending_Review.GetStringValue().ToLower().Trim()//UAT-505
                    )
                    return VerificationDataMode.DataEntry;
                else if (currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue().ToLower().Trim()
                         || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue().ToLower().Trim() //UAT-505
                         || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue().ToLower().Trim() //UAT-824 :As a client admin, I should be able to update the item status and data when the status is Approved with Exception
                         || IsExceptionExpired(currentItemStatus, itemMovementTypeId)
                         )
                    return VerificationDataMode.Exception;
            }
            else if (_tenantTypeCode.ToLower() == TenantType.Compliance_Reviewer.GetStringValue().ToLower()) // Case of Third Party
            {
                if (IsReadOnlyForThirdParty(applicantItemDataId, currentItemStatus, subscriptionMobilityStatusCode, complianceItemId, isEscalatedItem))
                    return VerificationDataMode.ReadOnly;
                else if (applicantItemDataId.IsNotNull() && ((!String.IsNullOrEmpty(currentItemStatus)))
                    && currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue().ToLower().Trim()
                    && reviewerTenantId == CurrentViewContext.CurrentTenantId_Global)
                    return VerificationDataMode.DataEntry;
            }

            return VerificationDataMode.ReadOnly;
        }

        private bool IsReadOnlyForAdmin(Int32? applicantItemDataId, String currentItemStatus, String subscriptionMobilityStatusCode, Int32 complianceItemId,
            Boolean isEscalatedItem)
        {
            Boolean _isReadOnly = (!String.IsNullOrEmpty(subscriptionMobilityStatusCode) && subscriptionMobilityStatusCode == LkpSubscriptionMobilityStatus.MobilitySwitched);
            //Commented code for UAT-505 to give the permission for edit.
            //  ((!String.IsNullOrEmpty(currentItemStatus))
            //&& (
            // currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Expired.GetStringValue().ToLower().Trim()
            // || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue().ToLower().Trim()
            // || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue().ToLower().Trim()
            // || (!String.IsNullOrEmpty(subscriptionMobilityStatusCode) && subscriptionMobilityStatusCode == LkpSubscriptionMobilityStatus.MobilitySwitched)));


            // If Item is Not read only, then, check if it is specialized user item
            if (!_isReadOnly)
            {
                //Check user specialization, only if it is not an Escalated Item
                //if (!isEscalatedItem)
                //    _isReadOnly = !IsSpecialzedUser(complianceItemId);

                //Below code is commented for ticket UAT -1056 - Ability to enter data for all categories from user work queue.
                // If Item is Not read only, then, check if it is Assigned to user, in case of User Work Queue
                //if (!_isReadOnly && this.WorkQueue == WorkQueueType.UserWorkQueue)
                //{
                //    _isReadOnly = !IsCurrentItemAssigned(Convert.ToInt32(applicantItemDataId));
                //}


                //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
                if (!this.IsFullPermissionForVerification)
                {
                    _isReadOnly = true;
                }
            }

            return _isReadOnly;
        }

        private bool IsReadOnlyForClientAdmin(Int32? applicantItemDataId, String currentItemStatus, String subscriptionMobilityStatusCode, Int32 complianceItemId, Boolean isEscalatedItem)
        {
            Boolean _isReadOnly = (!String.IsNullOrEmpty(subscriptionMobilityStatusCode) && subscriptionMobilityStatusCode == LkpSubscriptionMobilityStatus.MobilitySwitched);
            //Commented code for UAT-505 to give the permission for edit.
            //(!String.IsNullOrEmpty(currentItemStatus) &&
            //              (
            //              currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Expired.GetStringValue().ToLower().Trim()
            //              || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue().ToLower().Trim()
            //              || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue().ToLower().Trim()
            //              || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue().ToLower().Trim()
            //              || (!String.IsNullOrEmpty(subscriptionMobilityStatusCode) && subscriptionMobilityStatusCode == LkpSubscriptionMobilityStatus.MobilitySwitched)));

            // If Item is Not read only, then, check if it is specialized user item
            if (!_isReadOnly)
            {
                //Check user specialization, only if it is not an Escalated Item
                //if (!isEscalatedItem)
                //    _isReadOnly = !IsSpecialzedUser(complianceItemId);

                //Below code is commented for ticket UAT -1056 - Ability to enter data for all categories from user work queue.
                // If Item is Not read only, then, check if it is Assigned to user, in case of User Work Queue
                //if (!_isReadOnly && this.WorkQueue == WorkQueueType.UserWorkQueue)
                //{
                //    _isReadOnly = !IsCurrentItemAssigned(Convert.ToInt32(applicantItemDataId));
                //}

                //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
                if (!this.IsFullPermissionForVerification)
                {
                    _isReadOnly = true;
                }
            }

            return _isReadOnly;
        }

        private bool IsReadOnlyForThirdParty(Int32? applicantItemDataId, String currentItemStatus, String subscriptionMobilityStatusCode, Int32 complianceItemId, Boolean isEscalatedItem)
        {
            Boolean _isReadOnly = applicantItemDataId.IsNull() || ((!String.IsNullOrEmpty(currentItemStatus)) &&
                               (currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Approved.GetStringValue().ToLower().Trim()
                               || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Incomplete.GetStringValue().ToLower().Trim()
                               || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Not_Approved.GetStringValue().ToLower().Trim()
                               || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Expired.GetStringValue().ToLower().Trim()
                               || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue().ToLower().Trim()
                               || currentItemStatus.ToLower().Trim() == ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue().ToLower().Trim()
                               || (!String.IsNullOrEmpty(subscriptionMobilityStatusCode) && subscriptionMobilityStatusCode == LkpSubscriptionMobilityStatus.MobilitySwitched)));

            // If Item is Not read only, then, check if it is specialized user item
            if (!_isReadOnly)
            {
                //Check user specialization, only if it is not an Escalated Item
                //if (!isEscalatedItem)
                //    _isReadOnly = !IsSpecialzedUser(complianceItemId);

                //Below code is commented for ticket UAT -1056 - Ability to enter data for all categories from user work queue.
                // If Item is Not read only, then, check if it is Assigned to user, in case of User Work Queue
                //if (!_isReadOnly && this.WorkQueue == WorkQueueType.UserWorkQueue)
                //{
                //    _isReadOnly = !IsCurrentItemAssigned(Convert.ToInt32(applicantItemDataId));
                //}

                //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
                if (!this.IsFullPermissionForVerification)
                {
                    _isReadOnly = true;
                }
            }
            return _isReadOnly;
        }

        /// <summary>
        /// Checks if the particular Item is Specialized item for the current logged in user. 
        /// </summary>
        /// <param name="complianceItemId"></param>
        /// <returns></returns>
        //private Boolean IsSpecialzedUser(Int32 complianceItemId)
        //{
        //    UserSpecializationDetails _specializationDetails = _lstUserSpecializations.Where(itm => itm.ReferenceId == complianceItemId).FirstOrDefault();

        //    if ((_specializationDetails.IsSpecializedUser) ||
        //        (!_specializationDetails.IsSpecializedUser && _specializationDetails.SpecializedUserCount == AppConsts.NONE))
        //        return true;
        //    else
        //        return false;
        //}

        /// <summary>
        /// Check if an Item is assigned to the particular user, who has opened the User queue.
        /// </summary>
        /// <param name="applicantComplianceItemId"></param>
        /// <returns></returns>
        private Boolean IsCurrentItemAssigned(Int32 applicantComplianceItemId)
        {
            if (!lstCurrentAssignments.IsNullOrEmpty() && applicantComplianceItemId > AppConsts.NONE)
                return lstCurrentAssignments.Where(uca => uca.QRA_RecordID == applicantComplianceItemId).Any();
            else
                return false;
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
            Boolean isSaveBtnReadOnly = true;
            //LoadControls();
            if (!ComplianceVerificationControlsList.IsNullOrEmpty())
                this.lstCurrentAssignments = _presenter.GetCurrentAssignments();

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
                    //Remove the category level exception dummy item from ApplicantItemVarificationdata.
                }
                applicantItmVerificationData = applicantItmVerificationData.Where(cond => cond.CatExceptionItemCode != catExceptionItemCode).ToList();
            }
            else //UAT-845 Creation of admin override function (verification details).
            {
                ApplicantItemVerificationData applicantDataCatOverride = applicantItmVerificationData.FirstOrDefault();
                CategoryLevelExcItemVerificationData = applicantDataCatOverride;
                if (applicantDataCatOverride.IsNotNull())
                {
                    CategoryOverride(applicantDataCatOverride, true);
                }

            }
            #endregion

            foreach (var item in ComplianceVerificationControlsList)
            {
                var tmVerificationData = applicantItmVerificationData.Where(x => x.ComplianceItemId == item.ComplianceItemID).ToList();
                if (tmVerificationData.IsNotNull())
                {
                    var _itemDataId = tmVerificationData.FirstOrDefault().ApplicantCompItemId;
                    var _currentItemStatus = tmVerificationData.FirstOrDefault().ItemComplianceStatusCode;
                    var _assignedToUserId = tmVerificationData.FirstOrDefault().AssignedToUserId;
                    var _reviewerTenantId = tmVerificationData.FirstOrDefault().ReviewerTenantId;
                    var _subscriptionMobilityStatusCode = tmVerificationData.FirstOrDefault().SubscriptionMobilityStatusCode;
                    var _itemMovementTypeId = tmVerificationData.FirstOrDefault().ItemMovementTypeID;
                    String _tenantTypeCode = Presenter.GetTenantType();

                    #region Check if Item is Escalated Item or not

                    Boolean _isEscalatedItem = false;

                    // If item is not Incomplete
                    if (_itemDataId.IsNotNull() && _itemDataId > AppConsts.NONE)
                    {
                        String _escalationCode = tmVerificationData[0].EscalationCode;
                        if (!String.IsNullOrEmpty(_escalationCode) && _escalationCode == lkpQueueEscalationType.Escalated.GetStringValue())
                            _isEscalatedItem = true;
                    }

                    #endregion

                    VerificationDataMode controlMode = ControlModeType(_itemDataId, _currentItemStatus, _assignedToUserId, _reviewerTenantId, _subscriptionMobilityStatusCode, _tenantTypeCode, item.ComplianceItemID, _isEscalatedItem, _itemMovementTypeId);
                    Boolean isReadOnlyControlsAfterSave = (controlMode == VerificationDataMode.ReadOnly ? true : false);
                    //UAT-2610
                    Boolean isExceptionItemRemoves = (controlMode == VerificationDataMode.DataEntry && item.VerificationDataMode == VerificationDataMode.Exception) ? true : false;
                    if (isExceptionItemRemoves)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "setTimeout(ReloadPage(),3000)", true);
                    }

                    if (item.VerificationDataMode == VerificationDataMode.ReadOnly)
                    {
                        (item.EditModeCntrlsList as ItemDataReadOnlyMode).RebindItemDataReadOnlyModeControlsData(tmVerificationData, isReadOnlyControlsAfterSave);
                    }
                    else if (item.VerificationDataMode == VerificationDataMode.Exception)
                    {
                        (item.EditModeCntrlsList as ItemDataExceptionMode).RebindItemDataExceptionModeControlsData(tmVerificationData, isReadOnlyControlsAfterSave);
                    }
                    else
                    {
                        (item.EditModeCntrlsList as ItemDataEditMode).ApplicantItemDataId = Convert.ToInt32(_itemDataId);
                        (item.EditModeCntrlsList as ItemDataEditMode).RebindItemDataEditModeControlsData(tmVerificationData, isReadOnlyControlsAfterSave);
                    }
                    if ((isSaveBtnReadOnly && controlMode == VerificationDataMode.DataEntry) || (!isReadOnlyControlsAfterSave))
                    {
                        isSaveBtnReadOnly = false;
                    }

                }
            }
            #region UAT-523 Category Exception
            //UAT-523:Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
            if (IsCategoryLevelException && (isSaveBtnReadOnly && CategoryExceptionControlMode != VerificationDataMode.ReadOnly))
            {
                isSaveBtnReadOnly = false;
            }
            #endregion
            if (isSaveBtnReadOnly)
            {
                btnSave.SaveButton.Visible = false;
            }

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
        //this method is obsolete post implementation of UAT-1886.
        public void Save_Backup(String catExplanatoryNotes, String catMoreInfoURL)
        {
            //UAT-718 created a boolean variable to update the queueImaging Data

            Boolean isDataSave = false;
            //List of next Queue Records
            List<ApplicantComplianceItemData> _lstNextQueueRecord = new List<ApplicantComplianceItemData>();
            List<ApplicantComplianceItemData> _lstReviewCompleted = new List<ApplicantComplianceItemData>();
            List<ApplicantComplianceItemData> _lstFinalEscalationResolved = new List<ApplicantComplianceItemData>();

            // Records already escalated and are to be Saved. 
            List<ApplicantComplianceItemData> _lstEscalatedRecords = new List<ApplicantComplianceItemData>();

            List<ApplicantComplianceAttributeData> _completeAttributeData = new List<ApplicantComplianceAttributeData>();
            List<ApplicantComplianceItemData> _completeItemData = new List<ApplicantComplianceItemData>();

            // Include items that are to be deleted but not with the status of type  Rejected, Approved, Exception Approved/Rejected
            List<ApplicantComplianceItemData> _lstItemsToDelete = new List<ApplicantComplianceItemData>();

            String _approved = ApplicantItemComplianceStatus.Approved.GetStringValue();
            String _rejected = ApplicantItemComplianceStatus.Not_Approved.GetStringValue();
            String _exceptionApproved = ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue();
            //String _exceptionApproved = ApplicantItemComplianceStatus.Exception_Approved.GetStringValue();
            String _exceptionRejected = ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue();
            String _expired = ApplicantItemComplianceStatus.Expired.GetStringValue();

            #region UAT-1608
            List<ApplicantComplianceItemData> lstDeletedItemsForSeries = new List<ApplicantComplianceItemData>();
            #endregion

            #region GET DATA OF ITEMS FOR DATA ENTRY

            Int32 _controlCount = pnlItems.Controls.Count;

            for (int i = 0; i < _controlCount; i++)
            {
                if (pnlItems.Controls[i] is ItemDataEditMode)
                {
                    // Use only items for which status != Incompelete, in the radiobutton selected
                    ItemDataEditMode _editModeControl = pnlItems.Controls[i] as ItemDataEditMode;

                    if (!_editModeControl.IsDeleteChecked)
                    {

                        _completeAttributeData.AddRange(_editModeControl.GetApplicantItemData().ToList());

                        if (!_editModeControl.IsIncompleteSelected)
                        {
                            String _escalationCode = _editModeControl.EscalatedCode;

                            if (!String.IsNullOrEmpty(_escalationCode) && _escalationCode == lkpQueueEscalationType.Escalated.GetStringValue())
                            {
                                ApplicantComplianceItemData _itemData = GetItemData(_editModeControl);
                                _itemData.IsEscalatedItem = true;
                                _lstEscalatedRecords.Add(_itemData);
                            }
                            else
                                _completeItemData.Add(GetItemData(_editModeControl));
                        }
                    }
                    else // Items to be deleted, needs to be used for the 'ClearQueueRecords'
                    {
                        if (_editModeControl.CurrentItemStatus != _approved && _editModeControl.CurrentItemStatus != _rejected
                           && _editModeControl.CurrentItemStatus != _exceptionApproved && _editModeControl.CurrentItemStatus != _exceptionRejected && _editModeControl.CurrentItemStatus != _expired)
                        {
                            _lstItemsToDelete.Add(GetItemData(_editModeControl, true));
                            #region UAT-1608
                            lstDeletedItemsForSeries.Add(GetItemData(_editModeControl));
                            #endregion
                        }
                        else
                        {
                            #region UAT-1608
                            lstDeletedItemsForSeries.Add(GetItemData(_editModeControl));
                            #endregion
                        }
                    }
                }
            }

            #endregion





            Dictionary<Int32, String> _dicValidationMessages = _presenter.ValidateApplicantData(_completeItemData, _completeAttributeData);
            if (!_dicValidationMessages.IsNullOrEmpty() && _dicValidationMessages.Count() > 0)
            {
                for (int i = 0; i < _controlCount; i++)
                {
                    if (pnlItems.Controls[i] is ItemDataEditMode)
                    {
                        String _validationMessage = "Item could not be validated";
                        ItemDataEditMode _editModeControl = pnlItems.Controls[i] as ItemDataEditMode;

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

                List<ApplicantComplianceItemData> _lstNextReviewRecord = new List<ApplicantComplianceItemData>();
                //List<ApplicantComplianceItemData> _lstNextQueueRecord = new List<ApplicantComplianceItemData>();

                // Records to be Escalated
                List<ApplicantComplianceItemData> _lstEscalationRecord = new List<ApplicantComplianceItemData>();

                // Records already Rejected, Approved, Exception Approved, Exception Rejected
                List<ApplicantComplianceItemData> _lstVerificationCompleted = new List<ApplicantComplianceItemData>();

                // Represents the Items for which no status was changed, and were saved as it is (DOES not include Incomplete items)
                List<ApplicantComplianceItemData> _lstNoStatusChanged = new List<ApplicantComplianceItemData>();

                // This list contains 2 types of items, one which were attempted to be in 
                // same queue and another which were attempted to be in different queue, but need another review
                List<ApplicantComplianceItemData> _lstIncompleteSameQueue = new List<ApplicantComplianceItemData>();

                #endregion

                StringBuilder sbErrors = new StringBuilder();

                #region GET DATA OF EXCEPTION RELATED ITEMS, FOR GETTING NEXT ACTION

                for (int i = 0; i < _controlCount; i++)
                {
                    if (pnlItems.Controls[i] is ItemDataExceptionMode)
                    {
                        var _exceptionModeControl = (pnlItems.Controls[i] as ItemDataExceptionMode);

                        ApplicantComplianceItemData _itemData = new ApplicantComplianceItemData
                        {
                            ApplicantName = _exceptionModeControl.VerificationData[0].ApplicantName,
                            ComplianceItemID = _exceptionModeControl.CurrentItemId,
                            SubmissionDate = _exceptionModeControl.VerificationData[0].SubmissionDate,
                            StatusID = Convert.ToInt32(_exceptionModeControl.VerificationData[0].ItemComplianceStatusId),
                            SystemStatusText = _exceptionModeControl.VerificationData[0].SystemStatus,
                            RushOrderStatusCode = _exceptionModeControl.RushOrderStatusCode,
                            ApplicantComplianceItemID = _exceptionModeControl.ApplicantItemDataId,
                            HierarchyNodeId = _exceptionModeControl.HierarchyNodeId,
                            ComplianceItemName = _exceptionModeControl.VerificationData[0].ItemName,
                            ComplianceCategoryName = _exceptionModeControl.VerificationData[0].CatName,
                            CompliancePackageName = _exceptionModeControl.VerificationData[0].PackageName,
                            RushOrderStatusText = _exceptionModeControl.RushOrderStatusText,
                            CurrentStatusCode = _exceptionModeControl.StatusCode,
                            VerificationStatusText = _exceptionModeControl.CurrentItemStatusText,
                            IsExceptionTypeItem = true,
                            AttemptedStatusCode = _exceptionModeControl.AttemptedStatusCode,
                            AttemptedItemStatusId = _exceptionModeControl.AttemptedItemStatusId,
                            VerificationComments = _exceptionModeControl.AdminComments
                            //VerificationComments = _exceptionModeControl.ExceptionComments
                        };

                        if (!_exceptionModeControl.IsDeleteChecked)
                        {
                            //Added to get data related to XML of 'HandleAssignment' SP in 'Queue Framework'
                            // Add to Escalated records list if attempted status !=  currrent status
                            String _escalationCode = _exceptionModeControl.EscalatedCode;
                            if (!String.IsNullOrEmpty(_escalationCode) && _escalationCode == lkpQueueEscalationType.Escalated.GetStringValue())
                            {
                                _itemData.IsEscalatedItem = true;
                                _lstEscalatedRecords.Add(_itemData);
                            }
                            else
                                _lstItemData.Add(_itemData);
                        }
                        else // Items to be deleted, needs to be used for the 'ClearQueueRecords'
                        {
                            if (_exceptionModeControl.StatusCode != _approved && _exceptionModeControl.StatusCode != _rejected
                           && _exceptionModeControl.StatusCode != _exceptionApproved && _exceptionModeControl.StatusCode != _exceptionRejected && _exceptionModeControl.StatusCode != _expired)
                                _lstItemsToDelete.Add(_itemData);
                        }
                    }
                }

                #region UAT-523 Category Level Exception
                //UAT-523:Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
                ApplicantComplianceItemData _applicantCatExcepData = new ApplicantComplianceItemData();
                if (IsCategoryLevelException)
                {
                    //Guid catExceptionItemCode = new Guid(AppConsts.WHOLE_CATEGORY_GUID);//For Dummy item inserted for category level exception "9122D197-27EE-44AC-8CC4-0A68F3D21F32"

                    //ApplicantItemVerificationData applicantDataCatException = lstIncludeCategoryLevelExcItem.FirstOrDefault(cond => cond.CatExceptionItemCode == catExceptionItemCode);
                    if (CategoryLevelExcItemVerificationData.IsNotNull() && !rbtnActions.SelectedValue.IsNullOrEmpty())
                    {

                        _applicantCatExcepData.ApplicantName = CategoryLevelExcItemVerificationData.ApplicantName;
                        _applicantCatExcepData.ComplianceItemID = CategoryLevelExcItemVerificationData.ComplianceItemId;
                        _applicantCatExcepData.SubmissionDate = CategoryLevelExcItemVerificationData.SubmissionDate;
                        _applicantCatExcepData.StatusID = Convert.ToInt32(CategoryLevelExcItemVerificationData.ItemComplianceStatusId);
                        _applicantCatExcepData.SystemStatusText = CategoryLevelExcItemVerificationData.SystemStatus;
                        _applicantCatExcepData.RushOrderStatusCode = CategoryLevelExcItemVerificationData.RushOrderStatusCode;
                        _applicantCatExcepData.ApplicantComplianceItemID = Convert.ToInt32(CategoryLevelExcItemVerificationData.ApplicantCompItemId);
                        _applicantCatExcepData.ApplicantComplianceCategoryID = Convert.ToInt32(CategoryLevelExcItemVerificationData.ApplicantCompCatId);
                        _applicantCatExcepData.HierarchyNodeId = Convert.ToInt32(CategoryLevelExcItemVerificationData.HierarchyNodeID);
                        _applicantCatExcepData.ComplianceItemName = CategoryLevelExcItemVerificationData.ItemName;
                        _applicantCatExcepData.ComplianceCategoryName = CategoryLevelExcItemVerificationData.CatName;
                        _applicantCatExcepData.CompliancePackageName = CategoryLevelExcItemVerificationData.PackageName;
                        _applicantCatExcepData.RushOrderStatusText = CategoryLevelExcItemVerificationData.RushOrderStatus;
                        _applicantCatExcepData.CurrentStatusCode = CategoryLevelExcItemVerificationData.ItemComplianceStatusCode;
                        _applicantCatExcepData.VerificationStatusText = CategoryLevelExcItemVerificationData.ItemComplianceStatus;
                        _applicantCatExcepData.IsExceptionTypeItem = true;
                        _applicantCatExcepData.AttemptedStatusCode = rbtnActions.SelectedValue;
                        _applicantCatExcepData.AttemptedItemStatusId = Presenter.GetNewStatusId(rbtnActions.SelectedValue);
                        //UAT:819: WB: Category Exception enhancements
                        _applicantCatExcepData.VerificationComments = txtAdminNotes.Text.Trim();
                        //VerificationComments = _exceptionModeControl.ExceptionComments

                        //UAT-850:WB: As an admin, I should be able to Delete a category exception/exception request
                        if (!IsDeleteCategoryExceptionChecked)
                        {
                            //Added to get data related to XML of 'HandleAssignment' SP in 'Queue Framework'
                            // Add to Escalated records list if attempted status !=  currrent status
                            String _escalationCode = CategoryLevelExcItemVerificationData.EscalationCode;
                            if (!String.IsNullOrEmpty(_escalationCode) && _escalationCode == lkpQueueEscalationType.Escalated.GetStringValue())
                            {
                                _applicantCatExcepData.IsEscalatedItem = true;
                                _lstEscalatedRecords.Add(_applicantCatExcepData);
                            }
                            else
                                _lstItemData.Add(_applicantCatExcepData);
                        }
                    }
                }
                #endregion

                #endregion

                // Will also contain the records for which we change the status from Approved/Reject etc. TO 
                // any othe status like Pending Review for Admin, Pending review for Client, Or reject, approve etc.
                // Records which are already Approved, Rejected etc, will be removed in further steps to avoid calling the 'CallHandleAssignments'
                _lstVerificationCompleted = _lstItemData.Where(itm => (itm.CurrentStatusCode == _approved
                                                              || itm.CurrentStatusCode == _rejected
                                                              || itm.CurrentStatusCode == _exceptionApproved
                                                              || itm.CurrentStatusCode == _exceptionRejected
                                                              || itm.CurrentStatusCode == _expired  //Added a status check for expired item(UAT-505) to not include in list of GetNextActionForItems.          
                                                               )).ToList();

                // Remove the Items with Above status so that they are not included in the 'GetNextACtions' method
                // Will also remove the items for which we are changing the status from  above 4 status to Pending review for admin or Client etc.
                _lstItemData.RemoveAll(x => _lstVerificationCompleted.Select(y => y.ComplianceItemID).Contains(x.ComplianceItemID));


                List<ApplicantComplianceItemData> lstItemUnderRandomReview = new List<ApplicantComplianceItemData>();

                List<ApplicantComplianceItemData> lstItemNeedToSendForRandomReview = new List<ApplicantComplianceItemData>();

                List<ApplicantComplianceItemData> lstItemsWithZeroReconciliationReviewCount = new List<ApplicantComplianceItemData>();

                List<ApplicantComplianceItemData> lstItemUnderRandomReviewOverridenByClntAdmn = new List<ApplicantComplianceItemData>();

                if (IsDefaultTenant)
                {
                    lstItemUnderRandomReview = _lstItemData.Where(x => x.ReconciliationReviewCount.IsNotNull() && x.ReconciliationReviewCount > AppConsts.NONE).ToList();

                    _lstItemData.RemoveAll(x => lstItemUnderRandomReview.Select(y => y.ComplianceItemID).Contains(x.ComplianceItemID));


                    lstItemsWithZeroReconciliationReviewCount = _lstItemData.Where(cond => cond.ApplicantComplianceItemID > AppConsts.NONE
                                                                                                             && cond.ReconciliationReviewCount.IsNotNull()
                                                                                                             && cond.ReconciliationReviewCount == AppConsts.NONE).ToList();

                    List<Int32> lstTemDataIds = lstItemsWithZeroReconciliationReviewCount.Select(sel => sel.ApplicantComplianceItemID).ToList();
                    if (!lstTemDataIds.IsNullOrEmpty())
                    {
                        Dictionary<Int32, Boolean> itemList = Presenter.CheckIfItemsAreInReconciliationProcess(lstTemDataIds);

                        List<ApplicantComplianceItemData> lstItemsUnderReconciliation = new List<ApplicantComplianceItemData>();
                        foreach (var item in itemList)
                        {
                            if (item.Value)
                            {
                                lstItemsUnderReconciliation.Add(lstItemsWithZeroReconciliationReviewCount.
                                                                FirstOrDefault(cond => cond.ApplicantComplianceItemID == item.Key));
                            }
                        }
                        if (!lstItemsUnderReconciliation.IsNullOrEmpty())
                        {
                            string itemIds = String.Join(",", lstItemsUnderReconciliation.Select(cond => cond.ComplianceItemID));

                            List<ItemReconciliationAvailiblityContract> lstItemReconciliationAvailiblityContract = Presenter.GetItemReconciliationAvailiblityStatus(itemIds);

                            foreach (var reconciliation in lstItemReconciliationAvailiblityContract)
                            {
                                var itemData = lstItemsUnderReconciliation.FirstOrDefault(cond => cond.ComplianceItemID == reconciliation.ItemID);
                                itemData.ReconciliationReviewCount = reconciliation.ReviewerCount;
                                if (reconciliation.IsSelected)
                                {
                                    lstItemNeedToSendForRandomReview.Add(itemData);
                                }
                            }

                            _lstItemData.RemoveAll(x => lstItemNeedToSendForRandomReview.Select(y => y.ComplianceItemID).Contains(x.ComplianceItemID));
                        }
                    }
                }

                else
                {
                    lstItemUnderRandomReviewOverridenByClntAdmn = _lstItemData.Where(cond => cond.ApplicantComplianceItemID > AppConsts.NONE
                                                                                             && cond.ReconciliationReviewCount.IsNotNull())
                                                                                             .ToList();
                }

                // STEP 1 - Call the Get Next Action. Include Items for which ApplicantComplianceItemID == 0 and is Exception Items
                _presenter.GetNextActionForItems_Backup(_lstItemData, this.lstAssignmentProperties, out _lstNextReviewRecord, out _lstNextQueueRecord, out _lstEscalationRecord,
                                                 out _lstIncompleteSameQueue);

                _lstNoStatusChanged = _lstItemData.Where(itmData => itmData.ApplicantComplianceItemID > 0
                                                                    && itmData.AttemptedStatusCode == itmData.CurrentStatusCode).ToList();


                #region UAT - 1763

                List<Int32> lstItems = _lstItemData.Select(col => col.ComplianceItemID).ToList();

                Dictionary<Int32, List<Int32>> dicAdjustItems = new Dictionary<int, List<int>>();

                dicAdjustItems.Add(SelectedComplianceCategoryId_Global, lstItems);

                #endregion


                #region Save Category Override Data
                //UAT-845 Creation of admin override function (verification details)
                if (!IsCategoryLevelException && !rbtnActionsCatOverride.SelectedValue.IsNullOrEmpty())
                {
                    DateTime? expirationDateCatOverride = null;
                    String categoryOverrideMessage = String.Empty;
                    String categorySucessMessage = String.Empty;
                    if (!dpExpteCatOverrideEditMode.SelectedDate.IsNullOrEmpty())
                    {
                        expirationDateCatOverride = Convert.ToDateTime(dpExpteCatOverrideEditMode.SelectedDate);
                    }

                    if (rbtnActionsCatOverride.SelectedValue == lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue() && expirationDateCatOverride.IsNotNull() && !hdnExpirationDate.Value.IsNullOrEmpty()
                         && Convert.ToDateTime(expirationDateCatOverride).ToShortDateString() != Convert.ToDateTime(hdnExpirationDate.Value).ToShortDateString()
                         && Convert.ToDateTime(Convert.ToDateTime(expirationDateCatOverride).ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString())
                        )
                    {
                        categoryOverrideMessage = "Expiration Date should be a future date.";
                        lblMessageCatException.Text = categoryOverrideMessage;
                        lblMessageCatException.CssClass = "error";
                    }
                    else if (rbtnActionsCatOverride.SelectedValue == lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue()
                        || (rbtnActionsCatOverride.SelectedValue == lkpCategoryExceptionStatus.DEFAULT.GetStringValue() && CategoryLevelExcItemVerificationData.CatExceptionStatusCode == lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue()))
                    {
                        if (!Presenter.UpdateCategoryOverrideData(expirationDateCatOverride, rbtnActionsCatOverride.SelectedValue, CategoryLevelExcItemVerificationData, txtOverrideNotes.Text.Trim()))
                        {
                            categoryOverrideMessage = CategoryLevelExcItemVerificationData.CatName + "could not be saved.";
                        }

                    }
                    if (!String.IsNullOrEmpty(categoryOverrideMessage))
                        sbErrors.Append(categoryOverrideMessage + ", ");
                    else
                    {
                        if (!String.IsNullOrEmpty(categorySucessMessage))
                        {
                            lblMessageCatException.Text = categorySucessMessage;
                            lblMessageCatException.CssClass = "sucs";
                        }
                    }

                }
                #endregion

                #region SAVE NORMAL ITEM DATA

                for (int i = 0; i < _controlCount; i++)
                {
                    if (pnlItems.Controls[i] is ItemDataEditMode)
                    {
                        ApplicantComplianceItemData _itemData = new ApplicantComplianceItemData();
                        ItemDataEditMode _editModeControl = pnlItems.Controls[i] as ItemDataEditMode;

                        Boolean _isIncompleteItem = _editModeControl.ApplicantItemDataId == AppConsts.NONE ? true : false;

                        // Use this for deciding whether the current ITEM(which is already added i.e. not Incomplete),
                        // is to be updated or not (depending on next action)
                        String _recordActionType = String.Empty;

                        _recordActionType = CheckItemType(_lstNextReviewRecord, _lstNextQueueRecord, _lstEscalationRecord,
                                                          _lstIncompleteSameQueue, _lstNoStatusChanged, _lstEscalatedRecords,
                                                          _lstVerificationCompleted, _editModeControl.ComplianceItemId
                                                          , lstItemUnderRandomReview, lstItemNeedToSendForRandomReview, lstItemUnderRandomReviewOverridenByClntAdmn);

                        if (!lstItemNeedToSendForRandomReview.IsNullOrEmpty())
                        {
                            var itemData = lstItemNeedToSendForRandomReview.FirstOrDefault(cond => cond.ComplianceItemID == _editModeControl.ComplianceItemId);
                            if (itemData.IsNotNull())
                                _editModeControl.ReconciliationReviewCount = itemData.ReconciliationReviewCount;
                        }


                        if (_isIncompleteItem && _recordActionType.Equals(lkpQueueActionType.Next_Level_Review_Required.GetStringValue()))
                        {
                            ApplicantComplianceItemData _applicantComplianceItemData = _lstIncompleteSameQueue.Where(x => x.ComplianceItemID == _editModeControl.ComplianceItemId).FirstOrDefault();
                            if (!_applicantComplianceItemData.IsNullOrEmpty() && _applicantComplianceItemData.StatusID != 0)
                            {
                                _editModeControl.IncompleteItemNewStatusId = _applicantComplianceItemData.StatusID;
                                _editModeControl.IncompleteItemNewStatusCode = _applicantComplianceItemData.CurrentStatusCode;
                            }
                        }

                        ApplicantComplianceItemData _incItemDataNextQueue = _lstIncompleteSameQueue.Where(x => x.ComplianceItemID == _editModeControl.ComplianceItemId).FirstOrDefault();
                        if (!_incItemDataNextQueue.IsNullOrEmpty() && _incItemDataNextQueue.IsIncompleteWithDocuments)
                        {
                            _recordActionType = lkpQueueActionType.Proceed_To_Next_Queue.GetStringValue();
                            _editModeControl.IncompleteItemNewStatusId = _incItemDataNextQueue.StatusID;
                            _editModeControl.IncompleteItemNewStatusCode = _incItemDataNextQueue.CurrentStatusCode;
                        }
                        // Save the Item Data
                        String _exceptionMessage = _editModeControl.Save(out _itemData, _recordActionType);
                        if (_exceptionMessage.IsNullOrEmpty())
                            isDataSave = true;
                        if (!_editModeControl.IsDeleteChecked && !_editModeControl.IsIncompleteSelected)
                        {
                            UpdateReconciliationStatus(lstItemUnderRandomReview, _itemData, lstItemsWithZeroReconciliationReviewCount, lstItemNeedToSendForRandomReview, lstItemUnderRandomReviewOverridenByClntAdmn);
                            Int32 _applicantComplianceItemId = _itemData.ApplicantComplianceItemID;

                            if (_isIncompleteItem)
                            {
                                UpdateApplicantItemDataId(_lstIncompleteSameQueue, _editModeControl.ComplianceItemId, _applicantComplianceItemId);
                                if (_lstNextQueueRecord.Any(x => x.ApplicantComplianceItemID == AppConsts.NONE))
                                {
                                    UpdateApplicantItemDataId(_lstNextQueueRecord, _editModeControl.ComplianceItemId, _applicantComplianceItemId);
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(_exceptionMessage))
                        {
                            sbErrors.Append(_exceptionMessage + ", ");
                        }
                        else
                        {
                            String sucessMessage = GetSuccessMessage(_lstNextReviewRecord, _lstEscalationRecord, _lstIncompleteSameQueue, _editModeControl.ComplianceItemId, _editModeControl.NewItemStatusText, lstItemUnderRandomReview);
                            if (!String.IsNullOrEmpty(sucessMessage))
                            {
                                _editModeControl.SetSuccessMessage(sucessMessage);
                            }
                        }
                    }
                }

                #endregion

                #region Save Exception Item Data

                for (int i = 0; i < _controlCount; i++)
                {
                    if (pnlItems.Controls[i] is ItemDataExceptionMode)
                    {
                        ItemDataExceptionMode _exceptionModeControl = pnlItems.Controls[i] as ItemDataExceptionMode;

                        String _recordActionType = String.Empty;
                        Int32 _applicantComplianceItemId = _exceptionModeControl.ApplicantItemDataId;

                        _recordActionType = CheckItemType(_lstNextReviewRecord, _lstNextQueueRecord, _lstEscalationRecord,
                                                          _lstIncompleteSameQueue, _lstNoStatusChanged, _lstEscalatedRecords,
                                                          _lstVerificationCompleted, _exceptionModeControl.CurrentItemId,
                                                          lstItemUnderRandomReview, lstItemNeedToSendForRandomReview, lstItemUnderRandomReviewOverridenByClntAdmn);

                        String _exceptionMessage = _exceptionModeControl.Save(_recordActionType);
                        if (_exceptionMessage.IsNullOrEmpty())
                            isDataSave = true;
                        if (!String.IsNullOrEmpty(_exceptionMessage))
                            sbErrors.Append(_exceptionMessage + ", ");
                        else
                        {
                            String sucessMessage = GetSuccessMessage(_lstNextReviewRecord, _lstEscalationRecord, _lstIncompleteSameQueue, _exceptionModeControl.CurrentItemId, _exceptionModeControl.NewItemStatusText, lstItemUnderRandomReview);
                            if (!String.IsNullOrEmpty(sucessMessage))
                            {
                                _exceptionModeControl.SetSuccessMessage(sucessMessage);
                            }
                        }
                    }
                }

                //UAT-4342 Delete Overide Status 
                if (IsDeleteOverideStatusChecked)
                {
                    Presenter.DeleteOverideComplianceStatus();
                    rbtnActionsCatOverride.SelectedValue = lkpCategoryExceptionStatus.DEFAULT.GetStringValue();
                    chkDeleteOverideStatus.Visible = false;
                }

                #region UAT-523 Save Category Level Exception Data
                //UAT-523:Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
                if (IsCategoryLevelException)
                {
                    String categoryExceptionMessage = String.Empty;
                    Boolean isCategoryExceptionDeleted = false;


                    //UAT-850:WB: As an admin, I should be able to Delete a category exception/exception request
                    if (IsDeleteCategoryExceptionChecked)
                    {
                        ApplicantComplianceItemData appCmpItmData = Presenter.DeleteCategoryException();
                        // Items to be deleted, needs to be used for the 'ClearQueueRecords'
                        if (appCmpItmData.IsNotNull())
                        {
                            if (_applicantCatExcepData.CurrentStatusCode != _approved && _applicantCatExcepData.CurrentStatusCode != _rejected
                               && _applicantCatExcepData.CurrentStatusCode != _exceptionApproved && _applicantCatExcepData.CurrentStatusCode != _exceptionRejected && _applicantCatExcepData.CurrentStatusCode != _expired)
                                _lstItemsToDelete.Add(_applicantCatExcepData);
                            IsCategoryLevelException = false;
                            isCategoryExceptionDeleted = true;
                            pnlCategoryException.Visible = false;
                        }
                        else
                        {
                            categoryExceptionMessage = _applicantCatExcepData.ComplianceCategoryName + "could not be saved.";
                        }
                    }
                    else
                    {
                        if (!rbtnActions.SelectedValue.IsNullOrEmpty())
                        {
                            String _recordActionTypeCatLevelExc = String.Empty;
                            DateTime? expirationDate = null;
                            if (!dpExpiDateEditMode.SelectedDate.IsNullOrEmpty())
                            {
                                expirationDate = Convert.ToDateTime(dpExpiDateEditMode.SelectedDate);
                            }
                            _recordActionTypeCatLevelExc = CheckItemType(_lstNextReviewRecord, _lstNextQueueRecord, _lstEscalationRecord,
                                                                      _lstIncompleteSameQueue, _lstNoStatusChanged, _lstEscalatedRecords,
                                                                      _lstVerificationCompleted, _applicantCatExcepData.ComplianceItemID
                                                                      , lstItemUnderRandomReview, lstItemNeedToSendForRandomReview, lstItemUnderRandomReviewOverridenByClntAdmn);
                            if (rbtnActions.SelectedValue == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue() && expirationDate.IsNotNull() && !hdnExpirationDate.Value.IsNullOrEmpty()
                                 && Convert.ToDateTime(expirationDate).ToShortDateString() != Convert.ToDateTime(hdnExpirationDate.Value).ToShortDateString()
                                 && Convert.ToDateTime(Convert.ToDateTime(expirationDate).ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString())
                                )
                            {
                                categoryExceptionMessage = "Expiration Date should be a future date.";
                                lblMessageCatException.Text = categoryExceptionMessage;
                                lblMessageCatException.CssClass = "error";
                            }
                            else
                            {
                                if (Presenter.UpdateCategoryLevelExceptionData(_applicantCatExcepData.ApplicantComplianceCategoryID, expirationDate, rbtnActions.SelectedValue, _recordActionTypeCatLevelExc, _applicantCatExcepData, this.lstAssignmentProperties))
                                {
                                    if (rbtnActions.SelectedValue == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue() && _recordActionTypeCatLevelExc == lkpQueueActionType.Proceed_To_Next_Queue.GetStringValue())
                                    {
                                        //litCurrentCategoryStatus.Text = "Approved With Exception";
                                        //UAT-959
                                        litCategoryExceptionStatus.Text = "Approved With Exception";
                                    }
                                }
                                else
                                {
                                    categoryExceptionMessage = _applicantCatExcepData.ComplianceCategoryName + "could not be saved.";
                                }
                            }
                        }
                    }
                    if (!String.IsNullOrEmpty(categoryExceptionMessage))
                        sbErrors.Append(categoryExceptionMessage + ", ");
                    else
                    {
                        String categorySucessMessage = String.Empty;
                        if (!isCategoryExceptionDeleted)
                        {
                            if (!rbtnActions.SelectedValue.IsNullOrEmpty())
                                categorySucessMessage = GetSuccessMessage(_lstNextReviewRecord, _lstEscalationRecord, _lstIncompleteSameQueue, _applicantCatExcepData.ComplianceItemID, rbtnActions.SelectedItem.Text, lstItemUnderRandomReview);
                        }
                        else
                        {
                            categorySucessMessage = "Category exception deleted successfully.";
                        }
                        if (!String.IsNullOrEmpty(categorySucessMessage))
                        {
                            lblMessageCatException.Text = categorySucessMessage;
                            lblMessageCatException.CssClass = "sucs";
                        }
                    }

                }
                #endregion

                #endregion

                // Use the Escalated records further, Only those, for which Status was changed.
                _lstEscalatedRecords = _lstEscalatedRecords.Where(escRecords => escRecords.CurrentStatusCode != escRecords.AttemptedStatusCode).ToList();

                // Use this list to identify the items for which reviews are completed and further need no processing on SAVE
                //List<ApplicantComplianceItemData> _lstReviewCompleted = _lstNextQueueRecord.Where(x => x.AttemptedStatusCode == _approved
                //                                                || x.AttemptedStatusCode == _rejected
                //                                                || x.AttemptedStatusCode == _exceptionApproved
                //                                                || x.AttemptedStatusCode == _exceptionRejected).ToList();
                _lstReviewCompleted = _lstNextQueueRecord.Where(x => x.AttemptedStatusCode == _approved
                                                               || x.AttemptedStatusCode == _rejected
                                                               || x.AttemptedStatusCode == _exceptionApproved
                                                               || x.AttemptedStatusCode == _exceptionRejected).ToList();


                //Remove all those records which are not updated during save opration.
                string reviewNotSubmittedStatus = ReconciliationMatchingStatus.Reviewed_Not_Submitted.GetStringValue();
                lstItemUnderRandomReview.RemoveAll(cond => cond.ReconciliationMatchingStatus == reviewNotSubmittedStatus);
                lstItemsWithZeroReconciliationReviewCount.RemoveAll(cond => cond.ReconciliationMatchingStatus == reviewNotSubmittedStatus);
                lstItemNeedToSendForRandomReview.RemoveAll(cond => cond.ReconciliationMatchingStatus == reviewNotSubmittedStatus);

                lstItemUnderRandomReviewOverridenByClntAdmn.RemoveAll(cond => cond.ReconciliationMatchingStatus == reviewNotSubmittedStatus);

                //Get all the records for which review is matched (these records will move to their respective normal queues.)
                List<ApplicantComplianceItemData> lstItemsForReviewsMatched = lstItemUnderRandomReview.Where(cond => cond.ReconciliationMatchingStatus == ReconciliationMatchingStatus.Matched.GetStringValue()).ToList();

                lstItemUnderRandomReview.RemoveAll(x => lstItemsForReviewsMatched.Select(y => y.ComplianceItemID)
                                                                       .Contains(x.ComplianceItemID));
                _lstReviewCompleted.AddRange(lstItemsForReviewsMatched.Where(cond => cond.AttemptedStatusCode == _approved
                                                                                                         || cond.AttemptedStatusCode == _rejected));

                _lstNextQueueRecord.AddRange(lstItemsForReviewsMatched.Where(cond => cond.AttemptedStatusCode != cond.CurrentStatusCode && cond.AttemptedStatusCode != _rejected
                                                                                                            && cond.AttemptedStatusCode != _approved));

                //_lstReviewCompleted.AddRange()

                // Remove records which do not need further reviews or operations as they are rejetced, approved
                _lstNextQueueRecord.RemoveAll(x => _lstReviewCompleted.Select(y => y.ComplianceItemID)
                                                                       .Contains(x.ComplianceItemID));


                // Items for whic the attempted status is either Rejected, Approved, Exception Approved/Rejected are 
                // handled separately for generating the clear queue XML. So create them separately
                //List<ApplicantComplianceItemData> _lstFinalEscalationResolved = _lstEscalatedRecords.Where(x => x.AttemptedStatusCode == _approved
                //                                                                                    || x.AttemptedStatusCode == _rejected
                //                                                                                    || x.AttemptedStatusCode == _exceptionApproved
                //                                                                                    || x.AttemptedStatusCode == _exceptionRejected).ToList();
                _lstFinalEscalationResolved = _lstEscalatedRecords.Where(x => x.AttemptedStatusCode == _approved
                                                                                                    || x.AttemptedStatusCode == _rejected
                                                                                                    || x.AttemptedStatusCode == _exceptionApproved
                                                                                                    || x.AttemptedStatusCode == _exceptionRejected).ToList();


                // '_lstEscalatedRecords' Also contains ths items in '_lstFinalEscalationResolved'
                // So Use a temporary list to create the XML for rest of the items.
                List<ApplicantComplianceItemData> _lstTemp = _lstEscalatedRecords.Where(x => !_lstFinalEscalationResolved.Select(y => y.ComplianceItemID).Contains(x.ComplianceItemID)).ToList();


                //// STEP 2 - Escalate the Escalation records AND Apply the changes for already escalated records
                //_presenter.ApplyEscalationChanges(_lstEscalationRecord, _lstEscalatedRecords);



                List<ApplicantComplianceItemData> _lstItemsToDeleteUnderReconcillation = _lstItemsToDelete.Where(cond => cond.ReconciliationReviewCount.IsNotNull()).ToList();
                _lstItemsToDelete.RemoveAll(x => _lstItemsToDeleteUnderReconcillation.Select(y => y.ComplianceItemID).Contains(x.ComplianceItemID));

                // STEP 3 - Clear the settings for Items to be moved to Next queue or next review
                _presenter.ClearQueueRecords(_lstNextQueueRecord, _lstNextReviewRecord, _lstItemsToDelete, _lstReviewCompleted,
                                            _lstEscalationRecord, _lstTemp, _lstFinalEscalationResolved);

                _presenter.ClearReconciliationQueueRecords(lstItemsWithZeroReconciliationReviewCount, lstItemsForReviewsMatched, _lstItemsToDeleteUnderReconcillation);

                _presenter.ReconcillationOverRideByClntAdmin(lstItemUnderRandomReviewOverridenByClntAdmn);

                // Remove the items which are either Rejected, Approved, Exception Approved/Rejected as they are not needed for CallHandleAssignments.
                _lstEscalatedRecords.RemoveAll(x => _lstFinalEscalationResolved.Select(y => y.ComplianceItemID).Contains(x.ComplianceItemID));


                // STEP 4 - Call Parallel Task after all Save completed
                // Use the items for which New status is not equal to the 4 status of Approve, Reject etc.
                List<ApplicantComplianceItemData> _lstTempVerificationCompleted = _lstVerificationCompleted.Where(x => x.AttemptedStatusCode != _approved
                                                                           && x.AttemptedStatusCode != _rejected
                                                                           && x.AttemptedStatusCode != _exceptionApproved
                                                                           && x.AttemptedStatusCode != _exceptionRejected
                                                                           && x.AttemptedStatusCode != _expired  //Added a status check for expired item(UAT-505) to not include in list of GetNextActionForItems.
                                                                           ).ToList();

                _presenter.CallHandleAssignmentParallelTask(_lstNextQueueRecord, _lstNextReviewRecord, _lstIncompleteSameQueue,
                    _lstEscalationRecord, _lstEscalatedRecords, _lstTempVerificationCompleted);

                _presenter.CallHandleAssignmentReconciliationParallelTask(lstItemNeedToSendForRandomReview, lstItemUnderRandomReview);

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
                    //Session["IsDataSavedSuccessfully"] = true;
                }
                //UAT 537 Verification Details Screen "go to Next Pending for Review Category" should save data and button text change.
                IsDataSavedSuccessfully = _IsSavedSuccess;

                //UAT-718 
                if (isDataSave)
                {
                    Presenter.SetQueueImaging();
                }

                var applicantItemVerificationDataList = _presenter.GetApplicantItemVerificationData();

                RebindComplianceItemPanelData(applicantItemVerificationDataList);

                if (applicantItemVerificationDataList.IsNotNull())
                {
                    var catComplianceStatusName = String.Empty;
                    var catComplianceStatus = String.Empty;
                    var CategoryExceptionStatusCode = CategoryLevelExcItemVerificationData.CatExceptionStatusCode == null ? String.Empty : CategoryLevelExcItemVerificationData.CatExceptionStatusCode;
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
                    var imgUrl = GetImageUrl(catComplianceStatus, CategoryExceptionStatusCode);
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
                    String tooltipComplianceStatus = String.Empty;
                    if (catComplianceStatusName == "Approved" && CategoryExceptionStatusCode == "AAAD")
                    {
                        tooltipComplianceStatus = "Approved Override";
                    }
                    else
                    {
                        tooltipComplianceStatus = catComplianceStatusName;
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), Convert.ToString(Guid.NewGuid()),
                                                        "ChangeApplicantPanelData('" + imgUrl + "','" + tooltipComplianceStatus + "','" + packageStatusImgUrl
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
                        excRejContract.CategoryMoreInfoURL = catMoreInfoURL;

                        lstExceptionRejectionContract.Add(excRejContract);
                    }
                }
            }

            //Add contract for Escalated Item.
            if (_lstFinalEscalationResolved.IsNotNull() && _lstFinalEscalationResolved.Count > 0)
            {
                String instituteUrl = Presenter.GetInstitutionUrl(tenantId);
                foreach (var item in _lstFinalEscalationResolved)
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
                        excRejContract.CategoryMoreInfoURL = catMoreInfoURL;

                        lstExceptionRejectionContract.Add(excRejContract);
                    }
                }
            }

            if (lstExceptionRejectionContract.IsNotNull() && lstExceptionRejectionContract.Count > 0)
            {
                String primaryEmail = Presenter.GetApplicantPrimaryEmail(lstExceptionRejectionContract[0].ApplicantID);
                Presenter.CallParallelTaskForMail(lstExceptionRejectionContract, primaryEmail, tenantId, CurrentLoggedInUserId);

                //UAT-1812:
                Presenter.SaveSeriesRejectedItemStatusHistory(lstExceptionRejectionContract);
            }
        }


        public void Save(String catExplanatoryNotes, String catMoreInfoURL)
        {

            #region UAT 4067 Ability (by Node) to restrict upload of certain file types.
            Boolean IsRestrictedTypeDocExist = false;

            var tupleList = new List<Tuple<int, string, Boolean>>();
            tupleList = CheckRestrictedFileTypeExist(IsRestrictedTypeDocExist);

            HiddenField hdnIsRestrictedFileTypeChecked = ucVerificationDetailsDocumentConrol.FindControl("hdnIsRestrictedFileTypeChecked") as HiddenField;
            Boolean isRestricted = false;
            if (!hdnIsRestrictedFileTypeChecked.Value.IsNullOrEmpty())
            {
                isRestricted = Convert.ToBoolean(hdnIsRestrictedFileTypeChecked.Value);
            }

            if ((tupleList.Count > AppConsts.NONE && tupleList.All(x => x.Item3 == false && !x.Item2.IsNullOrEmpty()))
                || (IsCategoryLevelException && isRestricted && !IsDeleteCategoryExceptionChecked))
            {
                lblMessageCatException.Text = "Please remove the non-supported document(s).";
                lblMessageCatException.CssClass = "error";
                return;
            }
            #endregion
            //UAT-718 created a boolean variable to update the queueImaging Data
            //Session["IsDataSavedSuccessfully"] = false;
            Boolean isDataSave = false;
            //List of next Queue Records
            List<ApplicantComplianceItemData> _lstNextQueueRecord = new List<ApplicantComplianceItemData>();
            List<ApplicantComplianceItemData> _lstReviewCompleted = new List<ApplicantComplianceItemData>();
            List<ApplicantComplianceItemData> _lstFinalEscalationResolved = new List<ApplicantComplianceItemData>();
            List<ApplicantComplianceItemData> _lstExpiredToNotApproved = new List<ApplicantComplianceItemData>();
            List<Entity.ClientEntity.ApplicantItemVerificationData> listVerificationData = new List<ApplicantItemVerificationData>();
            // Records already escalated and are to be Saved. 
            List<ApplicantComplianceItemData> _lstEscalatedRecords = new List<ApplicantComplianceItemData>();

            List<ApplicantComplianceAttributeData> _completeAttributeData = new List<ApplicantComplianceAttributeData>();
            List<ApplicantComplianceItemData> _completeItemData = new List<ApplicantComplianceItemData>();

            // Include items that are to be deleted but not with the status of type  Rejected, Approved, Exception Approved/Rejected
            List<ApplicantComplianceItemData> _lstItemsToDelete = new List<ApplicantComplianceItemData>();

            String _approved = ApplicantItemComplianceStatus.Approved.GetStringValue();
            String _rejected = ApplicantItemComplianceStatus.Not_Approved.GetStringValue();
            String _exceptionApproved = ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue();
            String _exceptionRejected = ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue();
            String _expired = ApplicantItemComplianceStatus.Expired.GetStringValue();
            //UAT-2079:Do not trigger UI rules on verification details screen when only deleting an item's data
            Boolean isAnyItemDataChanged = false;
            //UAT-2525
            //Boolean isDataNeedToSaveForIncompleteItm = true;
            //Dictionary<Int32, List<ApplicantComplianceAttributeData>> _incompleteAttributeData = new Dictionary<int, List<ApplicantComplianceAttributeData>>();

            #region UAT-1608
            List<ApplicantComplianceItemData> lstDeletedItemsForSeries = new List<ApplicantComplianceItemData>();
            #endregion

            #region GET DATA OF ITEMS FOR DATA ENTRY

            Int32 _controlCount = pnlItems.Controls.Count;

            //UAT-2768
            Boolean isValidDocumentAssigned = true;
            Int32 totalCountOfItemEditForm = 0;
            for (int i = 0; i < _controlCount; i++)
            {
                if (pnlItems.Controls[i] is ItemDataEditMode)
                {
                    totalCountOfItemEditForm = totalCountOfItemEditForm + 1;
                }
                else if (pnlItems.Controls[i] is ItemDataExceptionMode)
                {
                    totalCountOfItemEditForm = totalCountOfItemEditForm + 1;
                }

            }

            //UAT-2768
            //Save Category Level Exception Documents
            if (IsCategoryLevelException)
            {
                Boolean needToFireUIRules = false;
                Boolean isUpdateSessionList = totalCountOfItemEditForm > AppConsts.NONE ? false : true;
                ucVerificationDetailsDocumentConrol.AssignDocument(isUpdateSessionList, needToFireUIRules, false);
            }

            Int32 currentItemDataCount = 0;

            for (int i = 0; i < _controlCount; i++)
            {
                if (pnlItems.Controls[i] is ItemDataEditMode)
                {
                    currentItemDataCount = currentItemDataCount + 1;
                    // Use only items for which status != Incompelete, in the radiobutton selected
                    ItemDataEditMode _editModeControl = pnlItems.Controls[i] as ItemDataEditMode;

                    if (!_editModeControl.IsDeleteChecked)
                    {
                        //UAT-2768
                        Boolean isUpdateSessionList = false;
                        Boolean isUpdateOnlySessionList = false;
                        Boolean needToFireUIRules = false;
                        Boolean isNotApprovedStatus = false;
                        if ((_editModeControl.CurrentItemStatus.IsNullOrEmpty() && String.Compare(_editModeControl.AttemptedItemStatus, ApplicantItemComplianceStatus.Incomplete.GetStringValue(), true) != AppConsts.NONE)
                              || (!_editModeControl.CurrentItemStatus.IsNullOrEmpty() && String.Compare(_editModeControl.CurrentItemStatus, _editModeControl.AttemptedItemStatus, true) != AppConsts.NONE))
                        {
                            needToFireUIRules = true;
                        }
                        if (needToFireUIRules && String.Compare(_editModeControl.AttemptedItemStatus, _rejected, true) == AppConsts.NONE)
                        {
                            needToFireUIRules = false;
                            isNotApprovedStatus = true;
                        }
                        if (currentItemDataCount == totalCountOfItemEditForm)
                        {
                            isUpdateSessionList = true;
                        }
                        Dictionary<String, String> dictAssignedDocResult = _editModeControl.AssignDocument(isUpdateSessionList, needToFireUIRules, isUpdateOnlySessionList
                                                                                                           , isNotApprovedStatus);
                        Boolean isValid = Convert.ToBoolean(dictAssignedDocResult["ValidAssignUnAssignDocument"]);
                        Int32 ItemDataID = Convert.ToInt32(dictAssignedDocResult["ItemDataID"]);
                        if ((ItemDataID > AppConsts.NONE || ItemDataID == -1) && !isNotApprovedStatus)
                        {
                            isAnyItemDataChanged = true;
                        }
                        _editModeControl.ApplicantItemDataId = _editModeControl.ApplicantItemDataId > AppConsts.NONE ? _editModeControl.ApplicantItemDataId : ItemDataID;
                        isValidDocumentAssigned = isValidDocumentAssigned ? isValid : isValidDocumentAssigned;

                        List<ApplicantComplianceAttributeData> completeAttributeData = _editModeControl.GetApplicantItemData().ToList();
                        if (!_editModeControl.IsIncompleteSelected)
                        {
                            _completeAttributeData.AddRange(completeAttributeData);
                            String _escalationCode = _editModeControl.EscalatedCode;

                            if (!String.IsNullOrEmpty(_escalationCode) && _escalationCode == lkpQueueEscalationType.Escalated.GetStringValue())
                            {
                                ApplicantComplianceItemData _itemData = GetItemData(_editModeControl);
                                _itemData.IsEscalatedItem = true;
                                _lstEscalatedRecords.Add(_itemData);
                            }
                            else
                                _completeItemData.Add(GetItemData(_editModeControl));
                        }
                        else //UAT-2525
                        {
                            if (!completeAttributeData.IsNullOrEmpty())
                            {
                                foreach (var attrDetails in completeAttributeData)
                                {
                                    if (!attrDetails.AttributeValue.IsNullOrEmpty() && attrDetails.AttributeValue != "0")
                                    {
                                        //isDataNeedToSaveForIncompleteItm = false;
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), Convert.ToString(Guid.NewGuid()), "IncompleteItemHasDataInfoMessage()", true);
                                        return;
                                    }
                                }
                                //if (!isDataNeedToSaveForIncompleteItm)
                                //{
                                //    _incompleteAttributeData.Add(_editModeControl.ComplianceItemId,completeAttributeData);
                                //}
                            }
                        }
                        //UAT-2079:Do not trigger UI rules on verification details screen when only deleting an item's data
                        if ((!isAnyItemDataChanged && _editModeControl.IsDataChanged) ||
                            (_editModeControl.AttemptedItemStatus != _rejected && //UAT-2523
                            _editModeControl.AttemptedItemStatus != ApplicantItemComplianceStatus.Incomplete.GetStringValue()
                            && String.Compare(_editModeControl.CurrentItemStatus, _editModeControl.AttemptedItemStatus, true) != AppConsts.NONE))
                        {
                            isAnyItemDataChanged = true;
                        }
                    }
                    else // Items to be deleted, needs to be used for the 'ClearQueueRecords'
                    {
                        if (_editModeControl.CurrentItemStatus != _approved && _editModeControl.CurrentItemStatus != _rejected
                           && _editModeControl.CurrentItemStatus != _exceptionApproved && _editModeControl.CurrentItemStatus != _exceptionRejected && _editModeControl.CurrentItemStatus != _expired)
                        {
                            _lstItemsToDelete.Add(GetItemData(_editModeControl, true));
                        }
                        #region UAT-1608
                        lstDeletedItemsForSeries.Add(GetItemData(_editModeControl));
                        #endregion
                    }
                }
                //UAT-2768
                else if (pnlItems.Controls[i] is ItemDataExceptionMode)
                {
                    var _exceptionModeControl = (pnlItems.Controls[i] as ItemDataExceptionMode);

                    //UAT-2768
                    currentItemDataCount = currentItemDataCount + 1;
                    Boolean isUpdateSessionList = false;
                    Boolean needToFireUIRules = false;
                    if (currentItemDataCount == totalCountOfItemEditForm)
                    {
                        isUpdateSessionList = true;
                    }
                    Dictionary<String, String> dictAssignedDocResult = _exceptionModeControl.AssignDocument(isUpdateSessionList, needToFireUIRules);
                    Boolean isValid = Convert.ToBoolean(dictAssignedDocResult["ValidAssignUnAssignDocument"]);
                    Int32 ItemDataID = Convert.ToInt32(dictAssignedDocResult["ItemDataID"]);
                    _exceptionModeControl.ApplicantItemDataId = _exceptionModeControl.ApplicantItemDataId > AppConsts.NONE ? _exceptionModeControl.ApplicantItemDataId : ItemDataID;
                    isValidDocumentAssigned = isValidDocumentAssigned ? isValid : isValidDocumentAssigned;
                }
            }

            #endregion
            //UAT-2079:Do not trigger UI rules on verification details screen when only deleting an item's data
            Dictionary<Int32, String> _dicValidationMessages = isAnyItemDataChanged ? _presenter.ValidateApplicantData(_completeItemData, _completeAttributeData) : null;
            if (!_dicValidationMessages.IsNullOrEmpty() && _dicValidationMessages.Count() > 0)
            {
                SetUIValidationMessage(_controlCount, _dicValidationMessages);
            }
            else if (isValidDocumentAssigned)//UAT-2768
            {
                // This list DOES NOT CONTAINS items which are to be deleted. Items to be deleted are in '_lstItemsToDelete'
                List<ApplicantComplianceItemData> _lstItemData = new List<ApplicantComplianceItemData>();

                // Copy the data collected to another list for further processing
                _lstItemData.AddRange(_completeItemData);

                //List for Expired to Rejected Item.
                if (_lstItemData.Count > 0 && _lstItemData.Any(x => x.CurrentStatusCode == _expired && x.AttemptedStatusCode == _rejected))
                {
                    _lstExpiredToNotApproved.AddRange(_lstItemData.Where(x => x.CurrentStatusCode == _expired && x.AttemptedStatusCode == _rejected));
                }
                #region Lists used to manage the items

                List<ApplicantComplianceItemData> _lstNextReviewRecord = new List<ApplicantComplianceItemData>();
                //List<ApplicantComplianceItemData> _lstNextQueueRecord = new List<ApplicantComplianceItemData>();

                // Records to be Escalated
                List<ApplicantComplianceItemData> _lstEscalationRecord = new List<ApplicantComplianceItemData>();

                // Records already Rejected, Approved, Exception Approved, Exception Rejected
                List<ApplicantComplianceItemData> _lstVerificationCompleted = new List<ApplicantComplianceItemData>();

                // Represents the Items for which no status was changed, and were saved as it is (DOES not include Incomplete items)
                List<ApplicantComplianceItemData> _lstNoStatusChanged = new List<ApplicantComplianceItemData>();

                // This list contains 2 types of items, one which were attempted to be in 
                // same queue and another which were attempted to be in different queue, but need another review
                List<ApplicantComplianceItemData> _lstIncompleteSameQueue = new List<ApplicantComplianceItemData>();

                #endregion

                StringBuilder sbErrors = new StringBuilder();

                #region GET DATA OF EXCEPTION RELATED ITEMS, FOR GETTING NEXT ACTION

                for (int i = 0; i < _controlCount; i++)
                {
                    if (pnlItems.Controls[i] is ItemDataExceptionMode)
                    {
                        var _exceptionModeControl = (pnlItems.Controls[i] as ItemDataExceptionMode);

                        ApplicantComplianceItemData _itemData = new ApplicantComplianceItemData
                        {
                            ApplicantName = _exceptionModeControl.VerificationData[0].ApplicantName,
                            ComplianceItemID = _exceptionModeControl.CurrentItemId,
                            SubmissionDate = _exceptionModeControl.VerificationData[0].SubmissionDate,
                            StatusID = Convert.ToInt32(_exceptionModeControl.VerificationData[0].ItemComplianceStatusId),
                            SystemStatusText = _exceptionModeControl.VerificationData[0].SystemStatus,
                            RushOrderStatusCode = _exceptionModeControl.RushOrderStatusCode,
                            ApplicantComplianceItemID = _exceptionModeControl.ApplicantItemDataId,
                            HierarchyNodeId = _exceptionModeControl.HierarchyNodeId,
                            ComplianceItemName = _exceptionModeControl.VerificationData[0].ItemName,
                            ComplianceCategoryName = _exceptionModeControl.VerificationData[0].CatName,
                            CompliancePackageName = _exceptionModeControl.VerificationData[0].PackageName,
                            RushOrderStatusText = _exceptionModeControl.RushOrderStatusText,
                            CurrentStatusCode = _exceptionModeControl.StatusCode,
                            VerificationStatusText = _exceptionModeControl.CurrentItemStatusText,
                            IsExceptionTypeItem = true,
                            AttemptedStatusCode = _exceptionModeControl.AttemptedStatusCode,
                            AttemptedItemStatusId = _exceptionModeControl.AttemptedItemStatusId,
                            VerificationComments = _exceptionModeControl.AdminComments,
                            VerificationCommentsWithInitials = _exceptionModeControl.AdminComments //UAT 2807
                            //VerificationComments = _exceptionModeControl.ExceptionComments
                        };

                        if (!_exceptionModeControl.IsDeleteChecked)
                        {
                            //Added to get data related to XML of 'HandleAssignment' SP in 'Queue Framework'
                            _lstItemData.Add(_itemData);
                        }
                        else // Items to be deleted, needs to be used for the 'ClearQueueRecords'
                        {
                            if (_exceptionModeControl.StatusCode != _approved && _exceptionModeControl.StatusCode != _rejected
                           && _exceptionModeControl.StatusCode != _exceptionApproved && _exceptionModeControl.StatusCode != _exceptionRejected && _exceptionModeControl.StatusCode != _expired)
                                _lstItemsToDelete.Add(_itemData);
                        }
                    }
                }

                #region UAT-523 Category Level Exception
                //UAT-523:Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
                ApplicantComplianceItemData _applicantCatExcepData = new ApplicantComplianceItemData();
                if (IsCategoryLevelException)
                {
                    //Guid catExceptionItemCode = new Guid(AppConsts.WHOLE_CATEGORY_GUID);//For Dummy item inserted for category level exception "9122D197-27EE-44AC-8CC4-0A68F3D21F32"

                    //ApplicantItemVerificationData applicantDataCatException = lstIncludeCategoryLevelExcItem.FirstOrDefault(cond => cond.CatExceptionItemCode == catExceptionItemCode);
                    if (CategoryLevelExcItemVerificationData.IsNotNull() && !rbtnActions.SelectedValue.IsNullOrEmpty())
                    {

                        _applicantCatExcepData.ApplicantName = CategoryLevelExcItemVerificationData.ApplicantName;
                        _applicantCatExcepData.ComplianceItemID = CategoryLevelExcItemVerificationData.ComplianceItemId;
                        _applicantCatExcepData.SubmissionDate = CategoryLevelExcItemVerificationData.SubmissionDate;
                        _applicantCatExcepData.StatusID = Convert.ToInt32(CategoryLevelExcItemVerificationData.ItemComplianceStatusId);
                        _applicantCatExcepData.SystemStatusText = CategoryLevelExcItemVerificationData.SystemStatus;
                        _applicantCatExcepData.RushOrderStatusCode = CategoryLevelExcItemVerificationData.RushOrderStatusCode;
                        _applicantCatExcepData.ApplicantComplianceItemID = Convert.ToInt32(CategoryLevelExcItemVerificationData.ApplicantCompItemId);
                        _applicantCatExcepData.ApplicantComplianceCategoryID = Convert.ToInt32(CategoryLevelExcItemVerificationData.ApplicantCompCatId);
                        _applicantCatExcepData.HierarchyNodeId = Convert.ToInt32(CategoryLevelExcItemVerificationData.HierarchyNodeID);
                        _applicantCatExcepData.ComplianceItemName = CategoryLevelExcItemVerificationData.ItemName;
                        _applicantCatExcepData.ComplianceCategoryName = CategoryLevelExcItemVerificationData.CatName;
                        _applicantCatExcepData.CompliancePackageName = CategoryLevelExcItemVerificationData.PackageName;
                        _applicantCatExcepData.RushOrderStatusText = CategoryLevelExcItemVerificationData.RushOrderStatus;
                        _applicantCatExcepData.CurrentStatusCode = CategoryLevelExcItemVerificationData.ItemComplianceStatusCode;
                        _applicantCatExcepData.VerificationStatusText = CategoryLevelExcItemVerificationData.ItemComplianceStatus;
                        _applicantCatExcepData.IsExceptionTypeItem = true;
                        _applicantCatExcepData.AttemptedStatusCode = rbtnActions.SelectedValue;
                        _applicantCatExcepData.AttemptedItemStatusId = Presenter.GetNewStatusId(rbtnActions.SelectedValue);
                        //UAT:819: WB: Category Exception enhancements
                        _applicantCatExcepData.VerificationComments = txtAdminNotes.Text.Trim();
                        _applicantCatExcepData.VerificationCommentsWithInitials = txtAdminNotes.Text.Trim(); //UAT 2807
                        //VerificationComments = _exceptionModeControl.ExceptionComments

                        //UAT-850:WB: As an admin, I should be able to Delete a category exception/exception request
                        if (!IsDeleteCategoryExceptionChecked)
                        {
                            //Added to get data related to XML of 'HandleAssignment' SP in 'Queue Framework'
                            // Add to Escalated records list if attempted status !=  currrent status
                            String _escalationCode = CategoryLevelExcItemVerificationData.EscalationCode;
                            if (!String.IsNullOrEmpty(_escalationCode) && _escalationCode == lkpQueueEscalationType.Escalated.GetStringValue())
                            {
                                _applicantCatExcepData.IsEscalatedItem = true;
                                _lstEscalatedRecords.Add(_applicantCatExcepData);
                            }
                            else
                                _lstItemData.Add(_applicantCatExcepData);
                        }
                    }
                }
                #endregion

                #endregion

                // Will also contain the records for which we change the status from Approved/Reject etc. TO 
                // any othe status like Pending Review for Admin, Pending review for Client, Or reject, approve etc.
                // Records which are already Approved, Rejected etc, will be removed in further steps to avoid calling the 'CallHandleAssignments'
                _lstVerificationCompleted = _lstItemData.Where(itm => (itm.CurrentStatusCode == _approved
                                                              || itm.CurrentStatusCode == _rejected
                                                              || itm.CurrentStatusCode == _exceptionApproved
                                                              || itm.CurrentStatusCode == _exceptionRejected
                                                              || itm.CurrentStatusCode == _expired  //Added a status check for expired item(UAT-505) to not include in list of GetNextActionForItems.          
                                                               )).ToList();

                // Remove the Items with Above status so that they are not included in the 'GetNextACtions' method
                // Will also remove the items for which we are changing the status from  above 4 status to Pending review for admin or Client etc.
                _lstItemData.RemoveAll(x => _lstVerificationCompleted.Select(y => y.ComplianceItemID).Contains(x.ComplianceItemID));


                List<ApplicantComplianceItemData> lstItemUnderRandomReview = new List<ApplicantComplianceItemData>();

                List<ApplicantComplianceItemData> lstItemNeedToSendForRandomReview = new List<ApplicantComplianceItemData>();

                List<ApplicantComplianceItemData> lstItemsWithZeroReconciliationReviewCount = new List<ApplicantComplianceItemData>();

                List<ApplicantComplianceItemData> lstItemUnderRandomReviewOverridenByClntAdmn = new List<ApplicantComplianceItemData>();
                if (IsDefaultTenant)
                {
                    lstItemUnderRandomReview = _lstItemData.Where(x => x.ReconciliationReviewCount.IsNotNull() && x.ReconciliationReviewCount > AppConsts.NONE).ToList();

                    _lstItemData.RemoveAll(x => lstItemUnderRandomReview.Select(y => y.ComplianceItemID).Contains(x.ComplianceItemID));


                    lstItemsWithZeroReconciliationReviewCount = _lstItemData.Where(cond => cond.ApplicantComplianceItemID > AppConsts.NONE
                                                                                                             && cond.ReconciliationReviewCount.IsNotNull()
                                                                                                             && cond.ReconciliationReviewCount == AppConsts.NONE).ToList();

                    List<Int32> lstTemDataIds = lstItemsWithZeroReconciliationReviewCount.Select(sel => sel.ApplicantComplianceItemID).ToList();
                    if (!lstTemDataIds.IsNullOrEmpty())
                    {
                        Dictionary<Int32, Boolean> itemList = Presenter.CheckIfItemsAreInReconciliationProcess(lstTemDataIds);

                        List<ApplicantComplianceItemData> lstItemsUnderReconciliation = new List<ApplicantComplianceItemData>();
                        foreach (var item in itemList)
                        {
                            if (item.Value)
                            {
                                lstItemsUnderReconciliation.Add(lstItemsWithZeroReconciliationReviewCount.
                                                                FirstOrDefault(cond => cond.ApplicantComplianceItemID == item.Key));
                            }
                        }
                        if (!lstItemsUnderReconciliation.IsNullOrEmpty())
                        {
                            string itemIds = String.Join(",", lstItemsUnderReconciliation.Select(cond => cond.ComplianceItemID));

                            List<ItemReconciliationAvailiblityContract> lstItemReconciliationAvailiblityContract = Presenter.GetItemReconciliationAvailiblityStatus(itemIds);

                            foreach (var reconciliation in lstItemReconciliationAvailiblityContract)
                            {
                                var itemData = lstItemsUnderReconciliation.FirstOrDefault(cond => cond.ComplianceItemID == reconciliation.ItemID);
                                itemData.ReconciliationReviewCount = reconciliation.ReviewerCount;
                                if (reconciliation.IsSelected)
                                {
                                    lstItemNeedToSendForRandomReview.Add(itemData);
                                }
                            }

                            _lstItemData.RemoveAll(x => lstItemNeedToSendForRandomReview.Select(y => y.ComplianceItemID).Contains(x.ComplianceItemID));
                        }
                    }
                }

                else
                {
                    lstItemUnderRandomReviewOverridenByClntAdmn = _lstItemData.Where(cond => cond.ApplicantComplianceItemID > AppConsts.NONE
                                                                                             && cond.ReconciliationReviewCount.IsNotNull())
                                                                                             .ToList();
                }

                // STEP 1 - Call the Get Next Action. Include Items for which ApplicantComplianceItemID == 0 and is Exception Items
                _presenter.GetNextActionForItems(_lstItemData, this.lstAssignmentProperties, out _lstNextQueueRecord,
                                                 out _lstIncompleteSameQueue);

                _lstNoStatusChanged = _lstItemData.Where(itmData => itmData.ApplicantComplianceItemID > 0
                                                                    && itmData.AttemptedStatusCode == itmData.CurrentStatusCode).ToList();


                #region UAT - 1763

                List<Int32> lstItems = _lstItemData.Select(col => col.ComplianceItemID).ToList();

                Dictionary<Int32, List<Int32>> dicAdjustItems = new Dictionary<int, List<int>>();

                dicAdjustItems.Add(SelectedComplianceCategoryId_Global, lstItems);

                #endregion


                #region Save Category Override Data
                //UAT-845 Creation of admin override function (verification details)
                if (!IsCategoryLevelException && !rbtnActionsCatOverride.SelectedValue.IsNullOrEmpty())
                {
                    DateTime? expirationDateCatOverride = null;
                    String categoryOverrideMessage = String.Empty;
                    String categorySucessMessage = String.Empty;
                    if (!dpExpteCatOverrideEditMode.SelectedDate.IsNullOrEmpty())
                    {
                        expirationDateCatOverride = Convert.ToDateTime(dpExpteCatOverrideEditMode.SelectedDate);
                    }

                    if (rbtnActionsCatOverride.SelectedValue == lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue() && expirationDateCatOverride.IsNotNull() && !hdnExpirationDate.Value.IsNullOrEmpty()
                         && Convert.ToDateTime(expirationDateCatOverride).ToShortDateString() != Convert.ToDateTime(hdnExpirationDate.Value).ToShortDateString()
                         && Convert.ToDateTime(Convert.ToDateTime(expirationDateCatOverride).ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString())
                        )
                    {
                        categoryOverrideMessage = "Expiration Date should be a future date.";
                        lblMessageCatException.Text = categoryOverrideMessage;
                        lblMessageCatException.CssClass = "error";
                    }
                    else if (rbtnActionsCatOverride.SelectedValue == lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue()
                        || (rbtnActionsCatOverride.SelectedValue == lkpCategoryExceptionStatus.DEFAULT.GetStringValue() && CategoryLevelExcItemVerificationData.CatExceptionStatusCode == lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue()))
                    {
                        //UAT-2547
                        String OverrideNotes = String.Empty;
                        if (rbtnActionsCatOverride.SelectedValue == lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue())
                        {
                            OverrideNotes = txtOverrideNotes.Text.Trim();
                        }
                        if (!Presenter.UpdateCategoryOverrideData(expirationDateCatOverride, rbtnActionsCatOverride.SelectedValue, CategoryLevelExcItemVerificationData, OverrideNotes))
                        {
                            categoryOverrideMessage = CategoryLevelExcItemVerificationData.CatName + "could not be saved.";
                        }

                    }
                    if (!String.IsNullOrEmpty(categoryOverrideMessage))
                        sbErrors.Append(categoryOverrideMessage + ", ");
                    else
                    {
                        if (!String.IsNullOrEmpty(categorySucessMessage))
                        {
                            lblMessageCatException.Text = categorySucessMessage;
                            lblMessageCatException.CssClass = "sucs";
                        }
                    }

                }
                #endregion

                #region SAVE NORMAL ITEM DATA

                for (int i = 0; i < _controlCount; i++)
                {
                    if (pnlItems.Controls[i] is ItemDataEditMode)
                    {
                        ApplicantComplianceItemData _itemData = new ApplicantComplianceItemData();
                        ItemDataEditMode _editModeControl = pnlItems.Controls[i] as ItemDataEditMode;

                        Boolean _isIncompleteItem = _editModeControl.ApplicantItemDataId == AppConsts.NONE ? true : false;

                        // Use this for deciding whether the current ITEM(which is already added i.e. not Incomplete),
                        // is to be updated or not (depending on next action)
                        String _recordActionType = String.Empty;

                        _recordActionType = CheckItemType(_lstNextReviewRecord, _lstNextQueueRecord, _lstEscalationRecord,
                                                          _lstIncompleteSameQueue, _lstNoStatusChanged, _lstEscalatedRecords,
                                                          _lstVerificationCompleted, _editModeControl.ComplianceItemId
                                                          , lstItemUnderRandomReview, lstItemNeedToSendForRandomReview, lstItemUnderRandomReviewOverridenByClntAdmn);

                        if (!lstItemNeedToSendForRandomReview.IsNullOrEmpty())
                        {
                            var itemData = lstItemNeedToSendForRandomReview.FirstOrDefault(cond => cond.ComplianceItemID == _editModeControl.ComplianceItemId);
                            if (itemData.IsNotNull())
                                _editModeControl.ReconciliationReviewCount = itemData.ReconciliationReviewCount;
                        }


                        if (_isIncompleteItem && _recordActionType.Equals(lkpQueueActionType.Next_Level_Review_Required.GetStringValue()))
                        {
                            ApplicantComplianceItemData _applicantComplianceItemData = _lstIncompleteSameQueue.Where(x => x.ComplianceItemID == _editModeControl.ComplianceItemId).FirstOrDefault();
                            if (!_applicantComplianceItemData.IsNullOrEmpty() && _applicantComplianceItemData.StatusID != 0)
                            {
                                _editModeControl.IncompleteItemNewStatusId = _applicantComplianceItemData.StatusID;
                                _editModeControl.IncompleteItemNewStatusCode = _applicantComplianceItemData.CurrentStatusCode;
                            }
                        }

                        ApplicantComplianceItemData _incItemDataNextQueue = _lstIncompleteSameQueue.Where(x => x.ComplianceItemID == _editModeControl.ComplianceItemId).FirstOrDefault();
                        if (!_incItemDataNextQueue.IsNullOrEmpty() && _incItemDataNextQueue.IsIncompleteWithDocuments)
                        {
                            _recordActionType = lkpQueueActionType.Proceed_To_Next_Queue.GetStringValue();
                            _editModeControl.IncompleteItemNewStatusId = _incItemDataNextQueue.StatusID;
                            _editModeControl.IncompleteItemNewStatusCode = _incItemDataNextQueue.CurrentStatusCode;
                        }
                        // Save the Item Data
                        String _exceptionMessage = _editModeControl.Save(out _itemData, _recordActionType);
                        if (_exceptionMessage.IsNullOrEmpty())
                            isDataSave = true;
                        if (!_editModeControl.IsDeleteChecked && !_editModeControl.IsIncompleteSelected)
                        {
                            UpdateReconciliationStatus(lstItemUnderRandomReview, _itemData, lstItemsWithZeroReconciliationReviewCount, lstItemNeedToSendForRandomReview, lstItemUnderRandomReviewOverridenByClntAdmn);
                            Int32 _applicantComplianceItemId = _itemData.ApplicantComplianceItemID;

                            if (_isIncompleteItem)
                            {
                                UpdateApplicantItemDataId(_lstIncompleteSameQueue, _editModeControl.ComplianceItemId, _applicantComplianceItemId);
                                if (_lstNextQueueRecord.Any(x => x.ApplicantComplianceItemID == AppConsts.NONE))
                                {
                                    UpdateApplicantItemDataId(_lstNextQueueRecord, _editModeControl.ComplianceItemId, _applicantComplianceItemId);
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(_exceptionMessage))
                        {
                            sbErrors.Append(_exceptionMessage + ", ");
                        }
                        else
                        {
                            String sucessMessage = GetSuccessMessage(_lstNextReviewRecord, _lstEscalationRecord, _lstIncompleteSameQueue, _editModeControl.ComplianceItemId, _editModeControl.NewItemStatusText, lstItemUnderRandomReview);
                            if (!String.IsNullOrEmpty(sucessMessage))
                            {
                                _editModeControl.SetSuccessMessage(sucessMessage);
                            }
                        }
                    }
                }

                #endregion

                #region Save Exception Item Data

                for (int i = 0; i < _controlCount; i++)
                {
                    if (pnlItems.Controls[i] is ItemDataExceptionMode)
                    {
                        ItemDataExceptionMode _exceptionModeControl = pnlItems.Controls[i] as ItemDataExceptionMode;

                        String _recordActionType = String.Empty;
                        Int32 _applicantComplianceItemId = _exceptionModeControl.ApplicantItemDataId;

                        _recordActionType = CheckItemType(_lstNextReviewRecord, _lstNextQueueRecord, _lstEscalationRecord,
                                                          _lstIncompleteSameQueue, _lstNoStatusChanged, _lstEscalatedRecords,
                                                          _lstVerificationCompleted, _exceptionModeControl.CurrentItemId,
                                                          lstItemUnderRandomReview, lstItemNeedToSendForRandomReview, lstItemUnderRandomReviewOverridenByClntAdmn);

                        String _exceptionMessage = _exceptionModeControl.Save(_recordActionType);
                        if (_exceptionMessage.IsNullOrEmpty())
                            isDataSave = true;
                        if (!String.IsNullOrEmpty(_exceptionMessage))
                            sbErrors.Append(_exceptionMessage + ", ");
                        else
                        {
                            String sucessMessage = GetSuccessMessage(_lstNextReviewRecord, _lstEscalationRecord, _lstIncompleteSameQueue, _exceptionModeControl.CurrentItemId, _exceptionModeControl.NewItemStatusText, lstItemUnderRandomReview);
                            if (!String.IsNullOrEmpty(sucessMessage))
                            {
                                _exceptionModeControl.SetSuccessMessage(sucessMessage);
                            }
                        }
                    }
                }

                //UAT-4342 Delete Overide Status 
                if (IsDeleteOverideStatusChecked)
                {
                    Presenter.DeleteOverideComplianceStatus();
                    rbtnActionsCatOverride.SelectedValue = lkpCategoryExceptionStatus.DEFAULT.GetStringValue();
                    chkDeleteOverideStatus.Visible = false;
                }

                #region UAT-523 Save Category Level Exception Data
                //UAT-523:Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
                if (IsCategoryLevelException)
                {
                    String categoryExceptionMessage = String.Empty;
                    Boolean isCategoryExceptionDeleted = false;



                    //UAT-850:WB: As an admin, I should be able to Delete a category exception/exception request
                    if (IsDeleteCategoryExceptionChecked)
                    {
                        ApplicantComplianceItemData appCmpItmData = Presenter.DeleteCategoryException();
                        // Items to be deleted, needs to be used for the 'ClearQueueRecords'
                        if (appCmpItmData.IsNotNull())
                        {
                            if (_applicantCatExcepData.CurrentStatusCode != _approved && _applicantCatExcepData.CurrentStatusCode != _rejected
                               && _applicantCatExcepData.CurrentStatusCode != _exceptionApproved && _applicantCatExcepData.CurrentStatusCode != _exceptionRejected && _applicantCatExcepData.CurrentStatusCode != _expired)
                                _lstItemsToDelete.Add(_applicantCatExcepData);
                            IsCategoryLevelException = false;
                            isCategoryExceptionDeleted = true;
                            pnlCategoryException.Visible = false;
                        }
                        else
                        {
                            categoryExceptionMessage = _applicantCatExcepData.ComplianceCategoryName + "could not be saved.";
                        }
                    }
                    else
                    {
                        if (!rbtnActions.SelectedValue.IsNullOrEmpty())
                        {
                            String _recordActionTypeCatLevelExc = String.Empty;
                            DateTime? expirationDate = null;
                            if (!dpExpiDateEditMode.SelectedDate.IsNullOrEmpty())
                            {
                                expirationDate = Convert.ToDateTime(dpExpiDateEditMode.SelectedDate);
                            }
                            _recordActionTypeCatLevelExc = CheckItemType(_lstNextReviewRecord, _lstNextQueueRecord, _lstEscalationRecord,
                                                                      _lstIncompleteSameQueue, _lstNoStatusChanged, _lstEscalatedRecords,
                                                                      _lstVerificationCompleted, _applicantCatExcepData.ComplianceItemID
                                                                      , lstItemUnderRandomReview, lstItemNeedToSendForRandomReview, lstItemUnderRandomReviewOverridenByClntAdmn);
                            if (rbtnActions.SelectedValue == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue() && expirationDate.IsNotNull() && !hdnExpirationDate.Value.IsNullOrEmpty()
                                 && Convert.ToDateTime(expirationDate).ToShortDateString() != Convert.ToDateTime(hdnExpirationDate.Value).ToShortDateString()
                                 && Convert.ToDateTime(Convert.ToDateTime(expirationDate).ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString())
                                )
                            {
                                categoryExceptionMessage = "Expiration Date should be a future date.";
                                lblMessageCatException.Text = categoryExceptionMessage;
                                lblMessageCatException.CssClass = "error";
                            }
                            else
                            {
                                if (Presenter.UpdateCategoryLevelExceptionData(_applicantCatExcepData.ApplicantComplianceCategoryID, expirationDate, rbtnActions.SelectedValue, _recordActionTypeCatLevelExc, _applicantCatExcepData, this.lstAssignmentProperties))
                                {
                                    if (rbtnActions.SelectedValue == ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue() && _recordActionTypeCatLevelExc == lkpQueueActionType.Proceed_To_Next_Queue.GetStringValue())
                                    {
                                        //litCurrentCategoryStatus.Text = "Approved With Exception";
                                        //UAT-959
                                        litCategoryExceptionStatus.Text = "Approved With Exception";
                                    }
                                }
                                else
                                {
                                    categoryExceptionMessage = _applicantCatExcepData.ComplianceCategoryName + "could not be saved.";
                                }
                            }
                        }
                    }
                    if (!String.IsNullOrEmpty(categoryExceptionMessage))
                        sbErrors.Append(categoryExceptionMessage + ", ");
                    else
                    {
                        String categorySucessMessage = String.Empty;
                        if (!isCategoryExceptionDeleted)
                        {
                            if (!rbtnActions.SelectedValue.IsNullOrEmpty())
                                categorySucessMessage = GetSuccessMessage(_lstNextReviewRecord, _lstEscalationRecord, _lstIncompleteSameQueue, _applicantCatExcepData.ComplianceItemID, rbtnActions.SelectedItem.Text, lstItemUnderRandomReview);
                        }
                        else
                        {
                            categorySucessMessage = "Category exception deleted successfully.";
                        }
                        if (!String.IsNullOrEmpty(categorySucessMessage))
                        {
                            lblMessageCatException.Text = categorySucessMessage;
                            lblMessageCatException.CssClass = "sucs";
                        }
                    }

                }
                #endregion

                #endregion

                // Use the Escalated records further, Only those, for which Status was changed.
                _lstEscalatedRecords = _lstEscalatedRecords.Where(escRecords => escRecords.CurrentStatusCode != escRecords.AttemptedStatusCode).ToList();

                // Use this list to identify the items for which reviews are completed and further need no processing on SAVE
                //List<ApplicantComplianceItemData> _lstReviewCompleted = _lstNextQueueRecord.Where(x => x.AttemptedStatusCode == _approved
                //                                                || x.AttemptedStatusCode == _rejected
                //                                                || x.AttemptedStatusCode == _exceptionApproved
                //                                                || x.AttemptedStatusCode == _exceptionRejected).ToList();
                _lstReviewCompleted = _lstNextQueueRecord.Where(x => x.AttemptedStatusCode == _approved
                                                               || x.AttemptedStatusCode == _rejected
                                                               || x.AttemptedStatusCode == _exceptionApproved
                                                               || x.AttemptedStatusCode == _exceptionRejected).ToList();


                //Remove all those records which are not updated during save opration.
                string reviewNotSubmittedStatus = ReconciliationMatchingStatus.Reviewed_Not_Submitted.GetStringValue();
                lstItemUnderRandomReview.RemoveAll(cond => cond.ReconciliationMatchingStatus == reviewNotSubmittedStatus);
                lstItemsWithZeroReconciliationReviewCount.RemoveAll(cond => cond.ReconciliationMatchingStatus == reviewNotSubmittedStatus);
                lstItemNeedToSendForRandomReview.RemoveAll(cond => cond.ReconciliationMatchingStatus == reviewNotSubmittedStatus);
                lstItemUnderRandomReviewOverridenByClntAdmn.RemoveAll(cond => cond.ReconciliationMatchingStatus == reviewNotSubmittedStatus);

                //Get all the records for which review is matched (these records will move to their respective normal queues.)
                List<ApplicantComplianceItemData> lstItemsForReviewsMatched = lstItemUnderRandomReview.Where(cond => cond.ReconciliationMatchingStatus == ReconciliationMatchingStatus.Matched.GetStringValue()).ToList();

                lstItemUnderRandomReview.RemoveAll(x => lstItemsForReviewsMatched.Select(y => y.ComplianceItemID)
                                                                       .Contains(x.ComplianceItemID));
                _lstReviewCompleted.AddRange(lstItemsForReviewsMatched.Where(cond => cond.AttemptedStatusCode == _approved
                                                                                                         || cond.AttemptedStatusCode == _rejected));

                _lstNextQueueRecord.AddRange(lstItemsForReviewsMatched.Where(cond => cond.AttemptedStatusCode != cond.CurrentStatusCode && cond.AttemptedStatusCode != _rejected
                                                                                                            && cond.AttemptedStatusCode != _approved));

                //_lstReviewCompleted.AddRange()

                // Remove records which do not need further reviews or operations as they are rejetced, approved
                _lstNextQueueRecord.RemoveAll(x => _lstReviewCompleted.Select(y => y.ComplianceItemID)
                                                                       .Contains(x.ComplianceItemID));


                // Items for whic the attempted status is either Rejected, Approved, Exception Approved/Rejected are 
                // handled separately for generating the clear queue XML. So create them separately
                //List<ApplicantComplianceItemData> _lstFinalEscalationResolved = _lstEscalatedRecords.Where(x => x.AttemptedStatusCode == _approved
                //                                                                                    || x.AttemptedStatusCode == _rejected
                //                                                                                    || x.AttemptedStatusCode == _exceptionApproved
                //                                                                                    || x.AttemptedStatusCode == _exceptionRejected).ToList();
                _lstFinalEscalationResolved = _lstEscalatedRecords.Where(x => x.AttemptedStatusCode == _approved
                                                                                                    || x.AttemptedStatusCode == _rejected
                                                                                                    || x.AttemptedStatusCode == _exceptionApproved
                                                                                                    || x.AttemptedStatusCode == _exceptionRejected).ToList();


                // '_lstEscalatedRecords' Also contains ths items in '_lstFinalEscalationResolved'
                // So Use a temporary list to create the XML for rest of the items.
                List<ApplicantComplianceItemData> _lstTemp = _lstEscalatedRecords.Where(x => !_lstFinalEscalationResolved.Select(y => y.ComplianceItemID).Contains(x.ComplianceItemID)).ToList();


                //// STEP 2 - Escalate the Escalation records AND Apply the changes for already escalated records
                //_presenter.ApplyEscalationChanges(_lstEscalationRecord, _lstEscalatedRecords);


                //Session["IsItemDeletedFromReconciliationQueue"] = false;
                List<ApplicantComplianceItemData> _lstItemsToDeleteUnderReconcillation = _lstItemsToDelete.Where(cond => cond.ReconciliationReviewCount.IsNotNull()).ToList();
                _lstItemsToDelete.RemoveAll(x => _lstItemsToDeleteUnderReconcillation.Select(y => y.ComplianceItemID).Contains(x.ComplianceItemID));

                // STEP 3 - Clear the settings for Items to be moved to Next queue or next review
                _presenter.ClearQueueRecords(_lstNextQueueRecord, _lstNextReviewRecord, _lstItemsToDelete, _lstReviewCompleted,
                                            _lstEscalationRecord, _lstTemp, _lstFinalEscalationResolved);
                //if (_lstItemsToDelete.Count > AppConsts.NONE)
                //{
                //    Session["IsItemDeletedFromReconciliationQueue"] = true;
                //}

                _presenter.ClearReconciliationQueueRecords(lstItemsWithZeroReconciliationReviewCount, lstItemsForReviewsMatched, _lstItemsToDeleteUnderReconcillation);

                _presenter.ReconcillationOverRideByClntAdmin(lstItemUnderRandomReviewOverridenByClntAdmn);


                // Remove the items which are either Rejected, Approved, Exception Approved/Rejected as they are not needed for CallHandleAssignments.
                _lstEscalatedRecords.RemoveAll(x => _lstFinalEscalationResolved.Select(y => y.ComplianceItemID).Contains(x.ComplianceItemID));


                // STEP 4 - Call Parallel Task after all Save completed
                // Use the items for which New status is not equal to the 4 status of Approve, Reject etc.
                List<ApplicantComplianceItemData> _lstTempVerificationCompleted = _lstVerificationCompleted.Where(x => x.AttemptedStatusCode != _approved
                                                                           && x.AttemptedStatusCode != _rejected
                                                                           && x.AttemptedStatusCode != _exceptionApproved
                                                                           && x.AttemptedStatusCode != _exceptionRejected
                                                                           && x.AttemptedStatusCode != _expired  //Added a status check for expired item(UAT-505) to not include in list of GetNextActionForItems.
                                                                           ).ToList();

                _presenter.CallHandleAssignmentParallelTask(_lstNextQueueRecord, _lstNextReviewRecord, _lstIncompleteSameQueue,
                    _lstEscalationRecord, _lstEscalatedRecords, _lstTempVerificationCompleted);

                _presenter.CallHandleAssignmentReconciliationParallelTask(lstItemNeedToSendForRandomReview, lstItemUnderRandomReview);

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
                //else if (!isDataNeedToSaveForIncompleteItm) //UAT-2525
                //{
                //    _userMessage = "An item with data entered can not be incomplete.";
                //    _IsSavedSuccess = false;
                //}
                else
                {
                    _userMessage = "Item(s) updated successfully.";
                    _IsSavedSuccess = true;
                    //Session["IsDataSavedSuccessfully"] = true;
                }
                //UAT 537 Verification Details Screen "go to Next Pending for Review Category" should save data and button text change.
                IsDataSavedSuccessfully = _IsSavedSuccess;

                //UAT-718 
                if (isDataSave)
                {
                    Presenter.SetQueueImaging();
                }
                //UAT-2618
                Presenter.UpdateIsDocumentAssociated();

                var applicantItemVerificationDataList = _presenter.GetApplicantItemVerificationData();
                listVerificationData.AddRange(applicantItemVerificationDataList);
                //UAT-2610
                if (applicantItemVerificationDataList.IsNotNull())
                {
                    var applicantItemData = applicantItemVerificationDataList.FirstOrDefault();
                    if (IsCategoryLevelException
                        && !applicantItemData.IsNullOrEmpty()
                        && (applicantItemData.CatComplianceStatusCode == ApplicantCategoryComplianceStatus.Approved.GetStringValue()
                            || applicantItemData.CatComplianceStatusCode == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue())
                        && (applicantItemData.CatExceptionStatusCode.IsNull()
                            || applicantItemData.CatExceptionStatusCode == ApplicantCategoryExceptionStatus.EXCEPTION_APPLIED.GetStringValue()))
                    {
                        if (_applicantCatExcepData.CurrentStatusCode != _approved && _applicantCatExcepData.CurrentStatusCode != _rejected
                           && _applicantCatExcepData.CurrentStatusCode != _exceptionApproved && _applicantCatExcepData.CurrentStatusCode != _exceptionRejected && _applicantCatExcepData.CurrentStatusCode != _expired)
                            _lstItemsToDelete.Add(_applicantCatExcepData);
                        IsCategoryLevelException = false;
                        pnlCategoryException.Visible = false;
                    }
                }

                RebindComplianceItemPanelData(applicantItemVerificationDataList);

                if (applicantItemVerificationDataList.IsNotNull())
                {
                    var catComplianceStatusName = String.Empty;
                    var catComplianceStatus = String.Empty;
                    var CategoryExceptionStatusCode = CategoryLevelExcItemVerificationData.CatExceptionStatusCode == null ? String.Empty : CategoryLevelExcItemVerificationData.CatExceptionStatusCode;
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
                    //UAT-2836
                    //var imgUrl = GetImageUrl(catComplianceStatus, CategoryExceptionStatusCode);
                    var imgUrl = String.Empty;
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
                    String tooltipComplianceStatus = String.Empty;
                    //if (catComplianceStatusName == "Approved" && CategoryExceptionStatusCode == "AAAD")
                    //{
                    //    tooltipComplianceStatus = "Approved Override";
                    //}
                    //else
                    //{
                    //    tooltipComplianceStatus = catComplianceStatusName;
                    //}
                    hdnPackageSubscriptionId.Value = Convert.ToString(CurrentPackageSubscriptionID_Global);
                    hdnTenantId.Value = Convert.ToString(SelectedTenantId_Global);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Convert.ToString(Guid.NewGuid()),
                                                        "ChangeApplicantPanelData('" + imgUrl + "','" + tooltipComplianceStatus + "','" + packageStatusImgUrl
                                                        + "','" + packageSubscription.lkpPackageComplianceStatu.Code.ToLower()
                                                        + "','" + _IsSavedSuccess + "');", true);
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
                        excRejContract.CategoryMoreInfoURL = catMoreInfoURL;
                        //UAT-4345 - Start
                        excRejContract.CategoryName = item.ComplianceCategoryName;
                        excRejContract.ComplianceItemName = item.ComplianceItemName;
                        //UAT-4345 - End

                        lstExceptionRejectionContract.Add(excRejContract);
                    }
                }
            }

            //Add contract for Escalated Item.
            if (_lstFinalEscalationResolved.IsNotNull() && _lstFinalEscalationResolved.Count > 0)
            {
                String instituteUrl = Presenter.GetInstitutionUrl(tenantId);
                foreach (var item in _lstFinalEscalationResolved)
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
                        excRejContract.CategoryMoreInfoURL = catMoreInfoURL;
                        //UAT-4345 - Start
                        excRejContract.CategoryName = item.ComplianceCategoryName;
                        excRejContract.ComplianceItemName = item.ComplianceItemName;
                        //UAT-4345 - End

                        lstExceptionRejectionContract.Add(excRejContract);
                    }
                }
            }
            //Add contract for Expired to Rejected Item.
            if (_lstExpiredToNotApproved.IsNotNull() && _lstExpiredToNotApproved.Count > 0)
            {

                String instituteUrl = Presenter.GetInstitutionUrl(tenantId);
                foreach (var item in _lstExpiredToNotApproved)
                {
                    if (item.AttemptedStatusCode != item.CurrentStatusCode && ((item.AttemptedStatusCode == ApplicantItemComplianceStatus.Not_Approved.GetStringValue())
                        && (item.CurrentStatusCode == ApplicantItemComplianceStatus.Expired.GetStringValue())) && listVerificationData.IsNotNull() && listVerificationData.Count > 0 && listVerificationData.Any(x => x.ApplicantCompItemId == item.ApplicantComplianceItemID && x.ItemComplianceStatusCode == _rejected))
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
                        excRejContract.CategoryMoreInfoURL = catMoreInfoURL;
                        //UAT-4345 - Start
                        excRejContract.CategoryName = item.ComplianceCategoryName;
                        excRejContract.ComplianceItemName = item.ComplianceItemName;
                        //UAT-4345 - End

                        lstExceptionRejectionContract.Add(excRejContract);
                    }
                }
            }

            if (lstExceptionRejectionContract.IsNotNull() && lstExceptionRejectionContract.Count > 0)
            {
                String primaryEmail = Presenter.GetApplicantPrimaryEmail(lstExceptionRejectionContract[0].ApplicantID);

                //UAT-2172: addded CurrentLoggedInUserId in exixting method.
                Presenter.CallParallelTaskForMail(lstExceptionRejectionContract, primaryEmail, tenantId, CurrentLoggedInUserId);

                //UAT-1812:
                Presenter.SaveSeriesRejectedItemStatusHistory(lstExceptionRejectionContract);
            }

            Presenter.SendMailOnComplianceStatusChange();
            //UAT3805
            Presenter.SendItemDocNotificationToAgencyUser();
        }

        private List<Tuple<int, string, Boolean>> CheckRestrictedFileTypeExist(bool IsRestrictedTypeDocExist)
        {
            var tupleList = new List<Tuple<int, string, Boolean>>();

            for (int i = 0; i < pnlItems.Controls.Count; i++)
            {
                if (pnlItems.Controls[i] is ItemDataEditMode)
                {
                    //currentItemDataCount1 = currentItemDataCount1 + 1;
                    // Use only items for which status != Incompelete, in the radiobutton selected
                    ItemDataEditMode _editModeControl = pnlItems.Controls[i] as ItemDataEditMode;

                    if (_editModeControl.IsRestrictedFileExist())
                    {
                        var tList = _editModeControl.IsRestrictionDocChecked(_editModeControl.IsDeleteChecked, _editModeControl.ComplianceItemId);
                        tupleList.AddRange(tList);
                    }
                }
                else if (pnlItems.Controls[i] is ItemDataExceptionMode)
                {
                    var _exceptionModeControl = (pnlItems.Controls[i] as ItemDataExceptionMode);
                    if (_exceptionModeControl.IsRestrictedFileExist())
                    {
                        var tList = _exceptionModeControl.IsRestrictionDocChecked(_exceptionModeControl.IsDeleteChecked, _exceptionModeControl.CurrentItemId);
                        tupleList.AddRange(tList);

                    }
                    //if (_exceptionModeControl.IsRestrictedFileExist())
                    //{
                    //    //_exceptionModeControl.SetValidationMessage("Please remove the non-supported document(s).");
                    //    IsRestrictedTypeDocExist = true;
                    //    break;
                    //}
                }
            }

            return tupleList;
        }

        private void SetUIValidationMessage(Int32 _controlCount, Dictionary<Int32, String> _dicValidationMessages)
        {
            for (int i = 0; i < _controlCount; i++)
            {
                if (pnlItems.Controls[i] is ItemDataEditMode)
                {
                    String _validationMessage = "Item could not be validated";
                    ItemDataEditMode _editModeControl = pnlItems.Controls[i] as ItemDataEditMode;

                    KeyValuePair<Int32, String> _keyValuePair = _dicValidationMessages.Where(vm => vm.Key == _editModeControl.ComplianceItemId).FirstOrDefault();

                    if (!_keyValuePair.IsNullOrEmpty())
                        _validationMessage = _keyValuePair.Value;

                    _editModeControl.SetValidationMessage(_validationMessage);
                }
            }
        }
        /// <summary>
        ///  Get data related to XML of 'HandleAssignment' SP in 'Queue Framework'
        /// </summary>
        /// <param name="_editModeControl"></param>
        /// <param name="itemToDelete"></param>
        /// <returns></returns>
        private static ApplicantComplianceItemData GetItemData(ItemDataEditMode _editModeControl, Boolean itemToDelete = false)
        {
            ApplicantComplianceItemData _itemData = new ApplicantComplianceItemData();

            _itemData.ApplicantComplianceItemID = _editModeControl.ApplicantItemDataId;
            _itemData.StatusID = Convert.ToInt32(_editModeControl.VerificationData[0].ItemComplianceStatusId);
            _itemData.CurrentStatusCode = _editModeControl.CurrentItemStatus;
            _itemData.ReconciliationReviewCount = _editModeControl.ReconciliationReviewCount;
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
                _itemData.ReconciliationReviewCount = _editModeControl.ReconciliationReviewCount;
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
        private String CheckItemType(List<ApplicantComplianceItemData> lstNextReviewRecord, List<ApplicantComplianceItemData> lstNextQueueRecord,
                                     List<ApplicantComplianceItemData> lstEscalationRecord, List<ApplicantComplianceItemData> lstIncompleteSameQueue,
                                     List<ApplicantComplianceItemData> lstNoStatusChanged, List<ApplicantComplianceItemData> lstEscalatedRecords,
                                     List<ApplicantComplianceItemData> lstVerificationCompleted, Int32 complianceItemId,
                                     List<ApplicantComplianceItemData> lstItemSelectedForRandomReview, List<ApplicantComplianceItemData> lstItemNeedToSendForRandomReview,
                                     List<ApplicantComplianceItemData> lstItemUnderRandomReviewOverridenByClntAdmn)
        {
            String _actionType = String.Empty;
            if (lstItemSelectedForRandomReview.Where(x => x.ComplianceItemID == complianceItemId).Any())
                _actionType = lkpQueueActionType.Random_Review_Required.GetStringValue();

            if (lstItemNeedToSendForRandomReview.Where(x => x.ComplianceItemID == complianceItemId).Any())
                _actionType = lkpQueueActionType.SendFor_Random_Review_Required.GetStringValue();

            else if (lstItemUnderRandomReviewOverridenByClntAdmn.Where(x => x.ComplianceItemID == complianceItemId).Any())
                _actionType = lkpQueueActionType.Random_Review_OverRidden_Clntadmn.GetStringValue();

            else if (lstNextReviewRecord.Where(x => x.ComplianceItemID == complianceItemId).Any()
                || lstIncompleteSameQueue.Where(x => x.ComplianceItemID == complianceItemId).Any())
                _actionType = lkpQueueActionType.Next_Level_Review_Required.GetStringValue();

            else if (lstNextQueueRecord.Where(x => x.ComplianceItemID == complianceItemId).Any()
                || lstEscalatedRecords.Where(x => x.ComplianceItemID == complianceItemId).Any()
                || (!lstVerificationCompleted.IsNullOrEmpty() && lstVerificationCompleted.Where(x => x.ComplianceItemID == complianceItemId).Any()))
                _actionType = lkpQueueActionType.Proceed_To_Next_Queue.GetStringValue();

            else if (lstEscalationRecord.Where(x => x.ComplianceItemID == complianceItemId).Any())
                _actionType = lkpQueueActionType.Escalation_Required.GetStringValue();

            else if (lstNoStatusChanged.Where(x => x.ComplianceItemID == complianceItemId).Any())
                _actionType = lkpQueueActionType.No_Status_Changed.GetStringValue();

            return _actionType;
        }

        protected string GetImageUrl(string status, String CategoryExceptionStatusCode)
        {
            string url = "";

            if (status.IsNullOrEmpty() || status == ApplicantCategoryComplianceStatus.Incomplete.GetStringValue())
            {
                url = ResolveUrl("~/Resources/Mod/Compliance/icons/no16.png");
            }
            else if (!(String.IsNullOrEmpty(CategoryExceptionStatusCode)) && CategoryExceptionStatusCode == "AAAD" && status == ApplicantCategoryComplianceStatus.Approved.GetStringValue())
            {
                url = ResolveUrl("~/Resources/Mod/Compliance/icons/yesx16.png");
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
          List<ApplicantComplianceItemData> lstIncompleteSameQueue, Int32 complainceItemID, String status, List<ApplicantComplianceItemData> lstItemUnderRandomReview)
        {
            String successMessage = String.Empty;
            string reviewNotSubmittedStatus = ReconciliationMatchingStatus.Reviewed_Only.GetStringValue();
            string reviewNotMatchedStatus = ReconciliationMatchingStatus.Not_Matched.GetStringValue();
            if (lstNextReviewRecord.Where(condition => condition.ComplianceItemID == complainceItemID).Any()
                || lstEscalationRecord.Where(condition => condition.ComplianceItemID == complainceItemID).Any()
                || lstIncompleteSameQueue.Where(condition => condition.ComplianceItemID == complainceItemID).Any()
                 || lstItemUnderRandomReview.Where(condition => condition.ComplianceItemID == complainceItemID
                                            && (condition.ReconciliationMatchingStatus == reviewNotSubmittedStatus
                                             || condition.ReconciliationMatchingStatus == reviewNotMatchedStatus)).Any())
            {
                successMessage = "Your changes were saved with status " + status + ".";
            }
            //ApplicantComplianceItemData nextReviewRecord = lstNextReviewRecord.Where(condition => condition.ComplianceItemID == complainceItemID).FirstOrDefault();
            //if (nextReviewRecord.IsNotNull())
            //{
            //    sucessMessage = "Your changes were saved with status " + status + ".";
            //}
            //else
            //{
            //    ApplicantComplianceItemData escalationReviewRecord = lstEscalationRecord.Where(condition => condition.ComplianceItemID == complainceItemID).FirstOrDefault();
            //    if (escalationReviewRecord.IsNotNull())
            //    {
            //        sucessMessage = "Your changes were saved with status " + status + ".";
            //    }
            //}
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
            //else
            //{
            //    _applicantComplianceItemData = lstIncompleteDifferentQueue.Where(x => x.ComplianceItemID == complianceItemId).FirstOrDefault();
            //    _applicantComplianceItemData.ApplicantComplianceItemID = applicantComplianceItemId;
            //    return String.Empty;
            //}
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
            txtAdminNotes.Text = String.Empty;
            List<ApplicantDocumentMappingData> _lstDocumentMappingList = new List<ApplicantDocumentMappingData>();
            if (!applicantDataCatException.IsNullOrEmpty() && IsCategoryLevelException)
            {
                String _tenantTypeCode = Presenter.GetTenantType();
                CategoryExceptionControlMode = CheckControlMode(applicantDataCatException, _tenantTypeCode);
                if (CategoryExceptionControlMode == VerificationDataMode.Exception || CategoryExceptionControlMode == VerificationDataMode.ReadOnly)
                {
                    pnlCategoryException.Visible = true;

                    //UAT:2267:-
                    litCategoryExceptionSubmissionDate.Text = applicantDataCatException.SubmissionDate.HasValue ? applicantDataCatException.SubmissionDate.Value.ToString("M/d/yyyy hh:mm:ss tt") + "(MT)" : string.Empty;

                    if (applicantDataCatException.CatExpirationDate.IsNullOrEmpty())
                    {
                        dpExpiDateEditMode.MinDate = DateTime.Now.AddDays(1); ;//Set min date of Expiration Date.
                    }
                    else if (Convert.ToDateTime(Convert.ToDateTime(applicantDataCatException.CatExpirationDate).ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                    {
                        dpExpiDateEditMode.MinDate = Convert.ToDateTime(applicantDataCatException.CatExpirationDate);
                    }
                    else
                    {
                        dpExpiDateEditMode.MinDate = DateTime.Now.AddDays(1);
                    }
                    //dpExpiDateEditMode.MinDate = DateTime.Now.AddDays(1);
                    dpExpiDateEditMode.SelectedDate = applicantDataCatException.CatExpirationDate;
                    dpExpiDateReadOnly.SelectedDate = applicantDataCatException.CatExpirationDate;
                    hdnExpirationDate.Value = applicantDataCatException.CatExpirationDate.IsNullOrEmpty() ? String.Empty : Convert.ToString(applicantDataCatException.CatExpirationDate);
                    //UAT-819: WB: Category Exception enhancements
                    litExceptionReason.Text = applicantDataCatException.ExceptionReason.HtmlEncode();

                    //UAT -2807
                    if (CurrentViewContext.IsDefaultTenant)
                    {
                        txtVerificationComments.Text = applicantDataCatException.VerificationComments;
                    }
                    else
                    {
                        txtVerificationComments.Text = applicantDataCatException.VerificationCommentsWithInitials;
                    }

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
                    if ((applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPLIED.GetStringValue() || applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPROVED.GetStringValue()) && CategoryExceptionControlMode == VerificationDataMode.Exception)
                    {
                        BindCategoryExceptionAction();
                        HideShowCategoryExceptionControl(false);
                        if (applicantDataCatException.CatComplianceStatusCode == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue() || applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPROVED.GetStringValue())
                        {
                            rbtnActions.SelectedValue = ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue();
                            //rbtnActions.SelectedItem.Enabled = false;
                            //litCurrentCategoryStatus.Text = applicantDataCatException.CatComplianceStatus;
                            //UAT-959 
                            litCategoryExceptionStatus.Text = "Approved With Exception";
                        }
                        else
                        {
                            //Changes as per UAT-819 WB: Category Exception enhancements
                            //litCurrentCategoryStatus.Text = "Pending Review"; //"Applied For Exception";
                            //UAT-959 
                            litCategoryExceptionStatus.Text = "Applied For Exception";
                        }
                    }
                    else
                    {
                        //litCurrentCategoryStatus.Text = applicantDataCatException.CatComplianceStatus;
                        //UAT-959
                        if (applicantDataCatException.CatComplianceStatusCode == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue() || applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPROVED.GetStringValue())
                        {
                            litCategoryExceptionStatus.Text = "Approved With Exception";
                        }
                        else
                        {
                            litCategoryExceptionStatus.Text = "Applied For Exception";
                        }
                        if (applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_REJECTED.GetStringValue() && CategoryExceptionControlMode == VerificationDataMode.Exception)
                        {
                            BindCategoryExceptionAction();
                            HideShowCategoryExceptionControl(false);
                            rbtnActions.SelectedValue = ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue();
                            //rbtnActions.SelectedItem.Enabled = false;
                            //UAT-959
                            litCategoryExceptionStatus.Text = "Exception Rejected";
                        }
                        if (isRebindDataAfterSave && CategoryExceptionControlMode != VerificationDataMode.ReadOnly)
                            HideShowCategoryExceptionControl(false);
                    }
                    if ((applicantDataCatException.CatComplianceStatusCode == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue() || applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPROVED.GetStringValue()) && CategoryExceptionControlMode != VerificationDataMode.ReadOnly)
                    {
                        divExpiDateEditMode.Style["Display"] = "block";
                        divExpiDateReadOnly.Style["Display"] = "none";
                    }
                    else if ((applicantDataCatException.CatComplianceStatusCode == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue() || applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPROVED.GetStringValue()) && CategoryExceptionControlMode == VerificationDataMode.ReadOnly)
                    {
                        divExpiDateReadOnly.Style["Display"] = "block";
                        divExpiDateEditMode.Style["Display"] = "none";
                    }
                    else
                    {
                        dpExpiDateEditMode.SelectedDate = null;
                        divExpiDateEditMode.Style["Display"] = "none";
                        divExpiDateReadOnly.Style["Display"] = "none";
                    }
                    List<Int32?> _lstApplicantComplianceItemIds = new List<int?>();
                    _lstApplicantComplianceItemIds.Add(applicantDataCatException.ApplicantCompItemId);
                    _xmlApplicantComplianceItemDataIds = GenerateItemIdsXML(_lstApplicantComplianceItemIds);
                    if (!String.IsNullOrEmpty(_xmlApplicantComplianceItemDataIds))
                    {
                        _lstDocumentMappingList = Presenter.GetApplicantDocumentsData(_xmlApplicantComplianceItemDataIds, String.Empty);
                    }
                    ucVerificationDetailsDocumentConrol.PackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionID_Global;
                    ucVerificationDetailsDocumentConrol.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
                    ucVerificationDetailsDocumentConrol.ApplicantId = CurrentViewContext.SelectedApplicantId_Global;
                    ucVerificationDetailsDocumentConrol.ItemDataId = applicantDataCatException.ApplicantCompItemId.HasValue ?
                        applicantDataCatException.ApplicantCompItemId.Value : 0;
                    ucVerificationDetailsDocumentConrol.ComplianceItemId = applicantDataCatException.ComplianceItemId;
                    ucVerificationDetailsDocumentConrol.IsReadOnly = false;
                    ucVerificationDetailsDocumentConrol.lstExceptionDocumentDocumentMaps = FilterExceptionDocumentMapping(_lstDocumentMappingList, applicantDataCatException.ApplicantCompItemId.HasValue ?
                        applicantDataCatException.ApplicantCompItemId.Value : 0);
                    ucVerificationDetailsDocumentConrol.lstApplicantDocument = CurrentViewContext.lstApplicantDocument;

                    //------------------------------------------Un- Assigned Documents-----------------------------
                    ucVerificationDetailsUnassignedDocumentConrol.PackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionID_Global;
                    ucVerificationDetailsUnassignedDocumentConrol.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
                    ucVerificationDetailsUnassignedDocumentConrol.ApplicantId = CurrentViewContext.SelectedApplicantId_Global;
                    ucVerificationDetailsUnassignedDocumentConrol.ItemDataId = 0;
                    ucVerificationDetailsUnassignedDocumentConrol.ComplianceItemId = 0;
                    ucVerificationDetailsUnassignedDocumentConrol.IsReadOnly = false;
                    hdfCatRejectionCodeException.Value = String.Empty;
                    ucVerificationDetailsUnassignedDocumentConrol.ComplianceCategoryId = CurrentViewContext.SelectedComplianceCategoryId_Global;
                    ucVerificationDetailsUnassignedDocumentConrol.lstApplicantDocument = CurrentViewContext.lstApplicantDocument;
                    //--------------------------------------------------------------------------------
                    hdfCatRejectionCodeException.Value = ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue();
                    rbtnActions.Attributes.Add("exActionItemId", Convert.ToString(applicantDataCatException.ComplianceItemId));
                    txtAdminNotes.Attributes.Add("exNoteItemId", Convert.ToString(applicantDataCatException.ComplianceItemId));
                    txtAdminNotes.Attributes.Add("exCurrentStatus", Convert.ToString(applicantDataCatException.ItemComplianceStatusCode));

                    //UAT-850:WB: As an admin, I should be able to Delete a category exception/exception request
                    IsDeleteApplicableForCatException(_tenantTypeCode, CategoryExceptionControlMode);
                }
            }
            else
            {
                ucVerificationDetailsDocumentConrol.PackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionID_Global;
                ucVerificationDetailsDocumentConrol.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
                ucVerificationDetailsDocumentConrol.ApplicantId = CurrentViewContext.SelectedApplicantId_Global;
                ucVerificationDetailsDocumentConrol.ItemDataId = 0;
                ucVerificationDetailsDocumentConrol.ComplianceItemId = 0;
                ucVerificationDetailsDocumentConrol.IsReadOnly = false;
                hdfCatRejectionCodeException.Value = String.Empty;


                //------------------------------------------Un- Assigned Documents-----------------------------
                ucVerificationDetailsUnassignedDocumentConrol.PackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionID_Global;
                ucVerificationDetailsUnassignedDocumentConrol.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
                ucVerificationDetailsUnassignedDocumentConrol.ApplicantId = CurrentViewContext.SelectedApplicantId_Global;
                ucVerificationDetailsUnassignedDocumentConrol.ItemDataId = 0;
                ucVerificationDetailsUnassignedDocumentConrol.ComplianceItemId = 0;
                ucVerificationDetailsUnassignedDocumentConrol.IsReadOnly = false;
                hdfCatRejectionCodeException.Value = String.Empty;
                ucVerificationDetailsUnassignedDocumentConrol.ComplianceCategoryId = CurrentViewContext.SelectedComplianceCategoryId_Global;
                ucVerificationDetailsUnassignedDocumentConrol.lstApplicantDocument = CurrentViewContext.lstApplicantDocument;
                //--------------------------------------------------------------------------------
            }
        }

        /// <summary>
        /// Bind the actions Radio buttons with Approved and Rejected datasource forCategory exception
        /// </summary>
        private void BindCategoryExceptionAction(Boolean isCategoryOverride = false)
        {
            if (!(rbtnActions.Items.Count > 0))
            {
                //rbtnActions.Items.Add(new ListItem { Text = VerificationDataActions.PENDING_REVIEW_EXCEPTION.GetStringValue(), Value = ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue() });
                rbtnActions.Items.Add(new ListItem { Text = VerificationDataActions.APPROVED_EXCEPTION.GetStringValue() + "   ", Value = ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue() });
                rbtnActions.Items.Add(new ListItem { Text = VerificationDataActions.DECLINED_EXCEPTION.GetStringValue(), Value = ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue() });
            }
            if (isCategoryOverride && !(rbtnActionsCatOverride.Items.Count > 0))
            {
                rbtnActionsCatOverride.Items.Add(new ListItem { Text = "Default", Value = lkpCategoryExceptionStatus.DEFAULT.GetStringValue() });
                rbtnActionsCatOverride.Items.Add(new ListItem { Text = VerificationDataActions.APPROVED_BY_OVERRIDE.GetStringValue(), Value = lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue() });
            }
        }

        /// <summary>
        /// Method to check the control mode (i.e. ReadOnly,Exception)
        /// </summary>
        /// <param name="applicantDataCatException">Category Exception onject</param>
        /// <returns></returns>
        private VerificationDataMode CheckControlMode(ApplicantItemVerificationData applicantDataCatException, String _tenantTypeCode)
        {
            String _currentItemStatus = applicantDataCatException.ItemComplianceStatusCode;
            Int32? _itemDataId = applicantDataCatException.ApplicantCompItemId;
            Int32? _reviewerTenantId = applicantDataCatException.ReviewerTenantId;
            Int32? _assignedToUserId = applicantDataCatException.AssignedToUserId;
            String _subscriptionMobilityStatusCode = applicantDataCatException.SubscriptionMobilityStatusCode;
            Boolean _isEscalatedItem = false;

            // If item is not Incomplete
            //if (_itemDataId.IsNotNull() && _itemDataId > AppConsts.NONE)
            //{
            //    String _escalationCode = applicantDataCatException.EscalationCode;
            //    if (!String.IsNullOrEmpty(_escalationCode) && _escalationCode == lkpQueueEscalationType.Escalated.GetStringValue())
            //        _isEscalatedItem = true;
            //}
            VerificationDataMode controlMode = GetControlModeForCategoryLevelException(_itemDataId, _currentItemStatus, _assignedToUserId,
                        _reviewerTenantId, _subscriptionMobilityStatusCode, _tenantTypeCode, Convert.ToInt32(applicantDataCatException.ComplianceItemId), _isEscalatedItem);

            return controlMode;

        }

        /// <summary>
        /// Check whether the current Item control should be ReadOnly, Data-Entry or Exception type
        /// </summary>
        /// <param name="applicantItemDataId"></param>
        /// <param name="currentItemStatus"></param>
        /// <param name="assignedToUserId"></param>
        /// <param name="reviewerTenantId"></param>
        /// <param name="subscriptionMobilityStatusCode"></param>
        /// <param name="_tenantTypeCode"></param>
        /// <param name="complianceItemId"></param>
        /// <returns></returns>
        private VerificationDataMode GetControlModeForCategoryLevelException(Int32? applicantItemDataId, String currentItemStatus, Int32? assignedToUserId,
            Int32? reviewerTenantId, String subscriptionMobilityStatusCode, String _tenantTypeCode, Int32 complianceItemId, Boolean isEscalatedItem)
        {
            if (CurrentViewContext.WorkQueue == WorkQueueType.UserWorkQueue && assignedToUserId.IsNotNull() && assignedToUserId != CurrentLoggedInUserId)
                return VerificationDataMode.ReadOnly;

            //Case of Admin
            if (CurrentViewContext.IsDefaultTenant)
            {
                if (IsReadOnlyForAdmin(applicantItemDataId, currentItemStatus, subscriptionMobilityStatusCode, complianceItemId, isEscalatedItem))
                    return VerificationDataMode.ReadOnly;
                else
                {
                    return VerificationDataMode.Exception;
                }

            }
            else if (_tenantTypeCode.ToLower() == TenantType.Institution.GetStringValue().ToLower()) // Case of Client Admin
            {
                if (IsReadOnlyForClientAdmin(applicantItemDataId, currentItemStatus, subscriptionMobilityStatusCode, complianceItemId, isEscalatedItem))
                    return VerificationDataMode.ReadOnly;
                else
                    return VerificationDataMode.Exception;
            }
            else if (_tenantTypeCode.ToLower() == TenantType.Compliance_Reviewer.GetStringValue().ToLower() && this.IsFullPermissionForVerification) // Case of Third Party
            {
                return VerificationDataMode.DataEntry;
            }

            return VerificationDataMode.ReadOnly;
        }

        /// <summary>
        /// Show Hide UI controls for Category Level Exception
        /// </summary>
        /// <param name="isReadOnly">isReadOnly</param>
        private void HideShowCategoryExceptionControl(Boolean isReadOnly)
        {
            //divReadOnly.Visible = isReadOnly;
            //divExpiDateReadOnly.Visible = isReadOnly;
            divEditMode.Visible = !isReadOnly;
            dvAdminActions.Visible = !isReadOnly;
            //UAT-850:WB: As an admin, I should be able to Delete a category exception/exception request
            chkDeleteCatException.Visible = !isReadOnly;
            //divExpiDateEditMode.Visible = !isReadOnly;
            if (isReadOnly)
            {
                divExpiDateReadOnly.Style["Display"] = "block";
                divExpiDateEditMode.Style["Display"] = "none";
            }
            else
            {
                divExpiDateReadOnly.Style["Display"] = "none";
                divExpiDateEditMode.Style["Display"] = "block";
            }
        }
        #endregion

        #region UAT-845 Creation of admin override function (verification details).
        private void CategoryOverride(ApplicantItemVerificationData applicantDataCatException, Boolean isRebindDataAfterSave = false)
        {
            if (!applicantDataCatException.IsNullOrEmpty() && !IsCategoryLevelException)
            {
                pnlCategoryOverride.Visible = true;
                if (applicantDataCatException.CatExpirationDate.IsNullOrEmpty())
                {
                    dpExpteCatOverrideEditMode.MinDate = DateTime.Now.AddDays(1);//Set min date of Expiration Date.
                }
                else if ((Convert.ToDateTime(Convert.ToDateTime(applicantDataCatException.CatExpirationDate).ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString())) && applicantDataCatException.CatExceptionStatusCode != lkpCategoryExceptionStatus.EXCEPTION_EXPIRED.GetStringValue())
                {
                    dpExpteCatOverrideEditMode.MinDate = Convert.ToDateTime(applicantDataCatException.CatExpirationDate);
                }
                else
                {
                    dpExpteCatOverrideEditMode.MinDate = DateTime.Now.AddDays(1);
                }
                if (applicantDataCatException.CatExceptionStatusCode != lkpCategoryExceptionStatus.EXCEPTION_EXPIRED.GetStringValue())
                {
                    dpExpteCatOverrideEditMode.SelectedDate = applicantDataCatException.CatExpirationDate;
                    //dpExpteCatOverrideReadOnly.SelectedDate = applicantDataCatException.CatExpirationDate;
                    hdnExpirationDate.Value = applicantDataCatException.CatExpirationDate.IsNullOrEmpty() ? String.Empty : Convert.ToString(applicantDataCatException.CatExpirationDate);
                }
                //HideShowCategoryOverrideControls(true);

                if (applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue())
                {
                    BindCategoryExceptionAction(true);
                    rbtnActionsCatOverride.SelectedValue = lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue();
                    chkDeleteOverideStatus.Visible = true;
                    //UAT-1832  Change text status as "Approved Override" 
                    litCurrentCategoryVerSts.Text = "Approved Override";
                    dvAdminActionsCatOverride.Visible = true;
                    divExpiDateCategoryOvrrideEditMode.Style["Display"] = "block";
                }
                else
                {
                    if (applicantDataCatException.CatExceptionStatusCode == lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE_DISABLE.GetStringValue())
                    {
                        chkDeleteOverideStatus.Visible = true;
                        //UAT-1832  Change text status as "Approved Override" 
                        litCurrentCategoryVerSts.Text = "Override in place";
                        dvAdminActionsCatOverride.Visible = true;
                        divExpiDateCategoryOvrrideEditMode.Style["Display"] = "block";
                    }
                    else if (applicantDataCatException.CatComplianceStatus.IsNullOrEmpty())
                    {
                        litCurrentCategoryVerSts.Text = "Incomplete";
                    }

                    else
                    {
                        litCurrentCategoryVerSts.Text = applicantDataCatException.CatComplianceStatus;
                    }
                    BindCategoryExceptionAction(true);
                    dvAdminActionsCatOverride.Visible = true;
                    if (rbtnActionsCatOverride.SelectedValue.IsNullOrEmpty())
                    {
                        rbtnActionsCatOverride.SelectedValue = lkpCategoryExceptionStatus.DEFAULT.GetStringValue();
                    }
                    if (dvAdminActionsCatOverride.Visible && rbtnActionsCatOverride.SelectedValue == lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue())
                    {
                        chkDeleteOverideStatus.Visible = true;
                        divExpiDateCategoryOvrrideEditMode.Style["Display"] = "block";
                    }
                    else
                    {
                        divExpiDateCategoryOvrrideEditMode.Style["Display"] = "none";
                    }
                }
                //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
                SetControlsForVerPermission();

                //UAT-2547
                txtOverrideNotes.Text = applicantDataCatException.CatOverrideNotes;
            }
        }

        private void HideShowCategoryOverrideControls(Boolean isReadOnly)
        {
            dvAdminActionsCatOverride.Visible = !isReadOnly;
            if (isReadOnly)
            {
                //divExpiDateCategoryOvrrideReadOnly.Style["Display"] = "block";
                divExpiDateCategoryOvrrideEditMode.Style["Display"] = "none";
            }
            else
            {
                divExpiDateCategoryOvrrideEditMode.Style["Display"] = "block";
                //divExpiDateCategoryOvrrideReadOnly.Style["Display"] = "none";
            }
        }

        //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
        private void SetControlsForVerPermission()
        {
            if (!this.IsFullPermissionForVerification)
            {
                rbtnActionsCatOverride.Items.FindByValue(lkpCategoryExceptionStatus.DEFAULT.GetStringValue()).Enabled = false;
                rbtnActionsCatOverride.Items.FindByValue(lkpCategoryExceptionStatus.APPROVED_BY_OVERRIDE.GetStringValue()).Enabled = false;
                //UAT-2547
                txtOverrideNotes.Enabled = false;

                if (divExpiDateCategoryOvrrideEditMode.Style["Display"] == "block")
                    dpExpteCatOverrideEditMode.Enabled = false;
            }
        }
        #endregion

        #region UAT-850: WB: As an admin, I should be able to Delete a category exception/exception request
        /// <summary>
        /// Is chkDeleteItem visible to delete items
        /// </summary>
        /// <param name="isDeleteApplicable"></param>
        /// <param name="complianceItemId"></param>
        private void IsDeleteApplicableForCatException(String tenantTypeCode, VerificationDataMode controlMode, Boolean isDeleteApplicable = true, Int32 complianceItemId = 0)
        {
            if (isDeleteApplicable && controlMode != VerificationDataMode.ReadOnly)
            {
                //if TenantType is Compliance_Reviewer i.e. Third Party
                if (tenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue())
                {
                    chkDeleteCatException.Visible = false;
                    return;
                }
                chkDeleteCatException.Visible = true;
            }
            //else
            //{
            //    if (complianceItemId == this.CurrentItemId)
            //        chkDeleteCatException.Visible = true;
            //}
        }
        #endregion

        private void UpdateReconciliationStatus(List<ApplicantComplianceItemData> lstItemUnderRandomReview,
                                                                    ApplicantComplianceItemData itemDataSaved
                                                                    , List<ApplicantComplianceItemData> lstItemsWithZeroReconciliationReviewCount
                                                                    , List<ApplicantComplianceItemData> lstItemNeedToSendForRandomReview
                                                                    , List<ApplicantComplianceItemData> lstItemUnderRandomReviewOverridenByClntAdmn)
        {
            if (!itemDataSaved.ReconciliationMatchingStatus.IsNullOrEmpty())
            {
                var itemData = lstItemUnderRandomReview.FirstOrDefault(cond => cond.ComplianceItemID == itemDataSaved.ComplianceItemID);
                if (itemData.IsNotNull())
                    itemData.ReconciliationMatchingStatus = itemDataSaved.ReconciliationMatchingStatus;
            }
            if (!itemDataSaved.ReconciliationMatchingStatus.IsNullOrEmpty())
            {
                var itemData = lstItemsWithZeroReconciliationReviewCount.FirstOrDefault(cond => cond.ComplianceItemID == itemDataSaved.ComplianceItemID);
                if (itemData.IsNotNull())
                    itemData.ReconciliationMatchingStatus = itemDataSaved.ReconciliationMatchingStatus;

                var itemDataSendForRandomReview = lstItemNeedToSendForRandomReview.FirstOrDefault(cond => cond.ComplianceItemID == itemDataSaved.ComplianceItemID);
                if (itemDataSendForRandomReview.IsNotNull())
                    itemDataSendForRandomReview.ReconciliationMatchingStatus = itemDataSaved.ReconciliationMatchingStatus;

                var item = lstItemUnderRandomReviewOverridenByClntAdmn.FirstOrDefault(cond => cond.ComplianceItemID == itemDataSaved.ComplianceItemID);
                if (item.IsNotNull())
                    item.ReconciliationMatchingStatus = itemDataSaved.ReconciliationMatchingStatus;
            }
        }

        #region UAT-3951: Addition of option to use preset ADB Admin rejection notes
        private void BindRejectionReasons()
        {
            cmbRejectionReason.DataSource = CurrentViewContext.ListRejectionReasons;
            cmbRejectionReason.DataBind();
        }

        protected void cmbRejectionReason_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            try
            {
                Entity.RejectionReason rejectionReasonObj = (Entity.RejectionReason)e.Item.DataItem;
                if (!rejectionReasonObj.IsNullOrEmpty())
                {
                    e.Item.Attributes["RR_ReasonText"] = rejectionReasonObj.RR_ReasonText;
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

        private void ShowHideRejectionReasonControl()
        {
            if (IsDefaultTenant)
            {
                if (String.Compare(rbtnActions.SelectedValue, NotApprovedStatusCode, true) == AppConsts.NONE)
                {
                    dvRejectionReason.Style["display"] = "block";
                }
                else
                {
                    dvRejectionReason.Style["display"] = "none";
                }
            }
        }
        #endregion
    }
}

