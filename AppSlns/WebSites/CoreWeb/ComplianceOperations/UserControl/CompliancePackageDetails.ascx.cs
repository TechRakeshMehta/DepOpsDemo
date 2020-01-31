#region System Defined

using System;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

#endregion

#region Application Specific

using DAL;

using INTSOF.Utils;
using Telerik.Web.UI;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Web.UI.HtmlControls;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;

using Business.ReportExecutionService;
using Business.RepoManagers;
using System.Drawing;
using System.Drawing.Drawing2D;
using Newtonsoft.Json;
using CoreWeb.ProfileSharing.Views;
#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class CompliancePackageDetails : BaseUserControl, ICompliancePackageDetailsView
    {
        #region Variables

        #region Private Variables

        private String _viewType;
        private Int32 _clientCompliancePackageID;
        private Entity.ClientEntity.PackageSubscription _subscription;
        private List<ListCategoryEditableBies> _listCategoryEditableBies = new List<ListCategoryEditableBies>();
        private CompliancePackageDetailsPresenter _presenter = new CompliancePackageDetailsPresenter();
        private Boolean _isEnterRequirementClick = false;
        private Boolean _isValFailForDeletion = false;
        private String _nonOptionalCatTreeListId = "tlistComplianceData";
        //UAT-1607
        private Int32 _catIDToShowUIValForSeriesItem = AppConsts.NONE;
        private Boolean _isApplyForExceptionClick = false;
        //UAT-3392
        private Boolean _isEnterData = false;
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        public bool IsUpdateButtonCall { get; set; }
        public bool IsApplyForExceptionCall { get; set; }
        public bool IsCancelButtonCall { get; set; }
        private int ApplicantComplianceItemId { get; set; }

        public Boolean IsAdminView
        {
            get { return String.IsNullOrEmpty(Convert.ToString(ViewState["_isAdminView"])) ? false : Convert.ToBoolean(ViewState["_isAdminView"]); }
            set { ViewState["_isAdminView"] = value; }
        }


        public String SubscriptionMobilityStatusCode
        {
            get
            {
                if (ViewState["SubscriptionMobilityStatusCode"].IsNotNull())
                {
                    return Convert.ToString(ViewState["SubscriptionMobilityStatusCode"]);
                }

                return String.Empty;
            }
            set
            {
                ViewState["SubscriptionMobilityStatusCode"] = value;
            }
        }
        public String TextBoxClientID
        {
            get
            {
                if (ViewState["TextBoxClientID"].IsNotNull())
                {
                    return Convert.ToString(ViewState["TextBoxClientID"]);
                }

                return String.Empty;
            }
            set
            {
                ViewState["TextBoxClientID"] = value;
            }
        }

        #region UAT-977: Additional work towards archive ability
        public Boolean IsArchivedSubscription { get; set; }
        public Boolean IsExpiredSubscription { get; set; }
        #endregion

        public Boolean IsGraduated { get; set; }

        public Boolean ArchivedGraduated { get; set; }
        #endregion

        #region Public Properties
        public Dictionary<Int32, String> SavedApplicantDocuments { get; set; }
        public List<ApplicantDocument> ApplicantUploadedDocuments { get; set; }

        public CompliancePackageDetailsPresenter Presenter
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

        protected Boolean IsEditing(object container)
        {
            if (container is TreeListEditFormInsertItem)
            {
                return false;
            }
            return true;
        }

        public Int32 TenantID
        {
            get
            {
                if (!String.IsNullOrEmpty(hdfTenantId.Value))
                {
                    return Convert.ToInt32(hdfTenantId.Value);
                }

                return 0;
            }
            set
            {
                hdfTenantId.Value = Convert.ToString(value);
            }
        }

        public Boolean IsItemSeries { get; set; }
        public Int32 ApplicantId
        {
            get;
            set;
        }

        public Int32 IsItemExpired
        {
            get
            {
                if (ViewState["IsItemExpired"] != null)
                {
                    return (Int32)(ViewState["IsItemExpired"]);
                }

                return AppConsts.NONE;
            }
            set
            {
                ViewState["IsItemExpired"] = value;
            }
        }

        //UAT-3806
        public List<ListItemEditableBies> lstEditableBy { get; set; }
        public Int32 ClientCompliancePackageID
        {
            get
            {
                return _clientCompliancePackageID;
            }
            set
            {
                _clientCompliancePackageID = value;
            }
        }

        public CompliancePackage ClientPackage
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// UAT 1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        /// </summary>
        public Int32 OrgUsrID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                }
                else
                {
                    return CurrentLoggedInUserId;
                }
            }
        }


        public Entity.ClientEntity.PackageSubscription Subscription
        {
            get
            {
                if (ViewState["PkgSubscription"] != null)
                {
                    return (Entity.ClientEntity.PackageSubscription)(ViewState["PkgSubscription"]);
                }

                return _subscription;
            }
            set
            {
                _subscription = value;
                String _status = _subscription == null || _subscription.lkpPackageComplianceStatu == null ? String.Empty : _subscription.lkpPackageComplianceStatu.Name;
                String _code = _subscription == null || _subscription.lkpPackageComplianceStatu == null ? String.Empty : _subscription.lkpPackageComplianceStatu.Code;
                SetCompliancestatus(_status, _code);
                ViewState["PkgSubscription"] = _subscription;
            }
        }

        public Int32 PackageId
        {
            get
            {
                if (!String.IsNullOrEmpty(hdfPackageId.Value))
                {
                    return Convert.ToInt32(hdfPackageId.Value);
                }

                return 0;
            }
            set
            {
                hdfPackageId.Value = Convert.ToString(value);
            }
        }

        private Int32 organizationUserID = 0;
        /// <summary>
        /// Organization user id
        /// </summary>
        public Int32 OrganiztionUserID
        {
            get { return organizationUserID == 0 ? base.CurrentUserId : organizationUserID; }
            set { organizationUserID = value; }

        }
        public List<ComplianceItem> lstAvailableItems
        {
            get;
            set;
        }

        public ComplianceCategory SelectedCategory
        {
            get;
            set;
        }

        public ICompliancePackageDetailsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        public String ControlUseType
        {
            get
            {
                if (ViewState["ControlUseType"] != null)
                {
                    return (Convert.ToString(ViewState["ControlUseType"]));
                }

                return String.Empty;
            }
            set
            {
                ViewState["ControlUseType"] = value;
            }
        }

        /// <summary>
        /// Id of the Master compliance Item (Client database), present in the ApplicantComplianceItem Entity. 
        /// </summary>
        public Int32 ItemId
        {
            get
            {
                if (!String.IsNullOrEmpty(hdfComplianceItemId.Value))
                {
                    return Convert.ToInt32(hdfComplianceItemId.Value);
                }

                return 0;
            }
            set
            {
                hdfComplianceItemId.Value = Convert.ToString(value);
            }
        }

        public Int32 PackageSubscriptionId
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString((ViewState[AppConsts.APPLICANT_PACKAGE_SUBSCRIPTION_ID_VIEW_STATE]))))
                {
                    return Convert.ToInt32(ViewState[AppConsts.APPLICANT_PACKAGE_SUBSCRIPTION_ID_VIEW_STATE]);
                }

                return 0;
            }
            set
            {
                ViewState[AppConsts.APPLICANT_PACKAGE_SUBSCRIPTION_ID_VIEW_STATE] = Convert.ToInt32(value);
            }
        }

        public Int32 AddedViewDocId
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString((ViewState["AddedViewDocId"]))))
                {
                    return Convert.ToInt32(ViewState["AddedViewDocId"]);
                }

                return 0;
            }
            set
            {
                ViewState["AddedViewDocId"] = Convert.ToInt32(value);
            }
        }

        public Int32 ComplianceStatusID
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString((ViewState[AppConsts.APPLICANT_COMPLIANCE_STATUS_ID_VIEW_STATE]))))
                {
                    return Convert.ToInt32(ViewState[AppConsts.APPLICANT_COMPLIANCE_STATUS_ID_VIEW_STATE]);
                }

                return 0;
            }
            set
            {
                ViewState[AppConsts.APPLICANT_COMPLIANCE_STATUS_ID_VIEW_STATE] = Convert.ToInt32(value);
            }
        }

        public String ComplianceStatus
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString((ViewState[AppConsts.APPLICANT_COMPLIANCE_STATUS_VIEW_STATE]))))
                {
                    return Convert.ToString(ViewState[AppConsts.APPLICANT_COMPLIANCE_STATUS_VIEW_STATE]);
                }

                return "";
            }
            set
            {
                ViewState[AppConsts.APPLICANT_COMPLIANCE_STATUS_VIEW_STATE] = value;
            }
        }

        public Int32 ComplianceCategoryId
        {
            get
            {
                if (!String.IsNullOrEmpty(hdfComplianceCategoryId.Value))
                {
                    return Convert.ToInt32(hdfComplianceCategoryId.Value);
                }

                return 0;
            }
            set
            {
                hdfComplianceCategoryId.Value = Convert.ToString(value);
            }
        }

        public ApplicantComplianceItemDataContract ItemDataContract
        {
            get;
            set;
        }

        public ApplicantComplianceCategoryDataContract CategoryDataContract
        {
            get;
            set;
        }

        public List<ApplicantComplianceAttributeDataContract> lstAttributesData
        {
            get;
            set;
        }

        public Dictionary<Int32, Int32> AttributeDocuments
        {
            get;
            set;
        }

        public Dictionary<Int32, Int32> ViewAttributeDocuments
        {
            get;
            set;
        }

        private String _errorMessage = String.Empty;

        public String ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                base.LogInfo(_errorMessage);
            }
        }
        public Int32 ApplicantComplianceCategoryDataId
        {
            get
            {
                if (ViewState[AppConsts.APPLICANT_COMPLIANCE_CATEGORY_DATA_ID_VIEW_STATE].IsNotNull())
                {
                    return Convert.ToInt32(ViewState[AppConsts.APPLICANT_COMPLIANCE_CATEGORY_DATA_ID_VIEW_STATE]);
                }

                return 0;
            }
            set
            {
                ViewState[AppConsts.APPLICANT_COMPLIANCE_CATEGORY_DATA_ID_VIEW_STATE] = Convert.ToString(value);
            }
        }

        public String UIValidationErrors
        {
            get;
            set;
        }

        public Boolean IsUIValidationApplicable
        {
            get;
            set;
        }

        public String WorkQueue
        {
            get
            {
                if (ViewState["WorkQueueType"].IsNotNull())
                {
                    return Convert.ToString(ViewState["WorkQueueType"]);
                }

                return String.Empty;
            }
            set
            {
                ViewState["WorkQueueType"] = value;
            }
        }

        public List<ExpiringItemList> lstExpiringItem
        {
            get;
            set;
        }

        public Int32 MaxFileSize
        {
            get
            {
                return Convert.ToInt32(WebConfigurationManager.AppSettings[AppConsts.MAXIMUM_FILE_SIZE]);
            }
        }


        public Boolean IsFileUploadExists
        {
            get
            {
                if (ViewState["IsFileUploadExists"].IsNotNull())
                {
                    return Convert.ToBoolean(ViewState["IsFileUploadExists"]);
                }

                return false;
            }
            set
            {
                ViewState["IsFileUploadExists"] = value;
            }
        }

        //UAT-4067
        public Boolean IsAllowedFileExtensionEnable
        {
            get;
            set;
        }

        public List<String> allowedFileExtensions
        {
            get
            {
                if (!ViewState["allowedFileExtensions"].IsNullOrEmpty())
                {
                    return (List<String>)(ViewState["allowedFileExtensions"]);
                }

                return new List<String>();
            }
            set
            {
                hdnAllowedExtension.Value = value.IsNullOrEmpty() ? String.Empty : String.Join(",", value);
                ViewState["allowedFileExtensions"] = value;
            }
        }

        #region UAT-1049:Admin Data Entry
        public Int16 DataEntryDocNewStatusId
        {
            get
            {
                if (ViewState["DataEntryDocNewStatusId"] != null)
                {
                    return Convert.ToInt16(ViewState["DataEntryDocNewStatusId"]);
                }

                return AppConsts.NONE;
            }
            set
            {
                ViewState["DataEntryDocNewStatusId"] = value;
            }
        }

        public Int16 DataEntryDocCompleteStatusId
        {
            get
            {
                if (ViewState["DataEntryDocCompleteStatusId"] != null)
                {
                    return Convert.ToInt16(ViewState["DataEntryDocCompleteStatusId"]);
                }

                return AppConsts.NONE;
            }
            set
            {
                ViewState["DataEntryDocCompleteStatusId"] = value;
            }
        }

        //public Int32 ViewDocumentTypeID
        //{
        //    get
        //    {
        //        if (ViewState["ViewDocumentTypeID"] != null)
        //            return Convert.ToInt32(ViewState["ViewDocumentTypeID"]);
        //        return AppConsts.NONE;
        //    }
        //    set
        //    {
        //        ViewState["ViewDocumentTypeID"] = value;
        //    }
        //}

        public Boolean IsItemSeriesSelected
        {
            get
            {
                if (ViewState["IsItemSeriesSelected"] != null)
                {
                    return (Convert.ToBoolean(ViewState["IsItemSeriesSelected"]));
                }

                return false;
            }
            set
            {
                ViewState["IsItemSeriesSelected"] = value;
            }
        }

        #endregion

        #region QUEUE MANAGEMENT
        public String PackageName
        {
            get
            {
                return lblPackageName.Text.HtmlDecode();

            }
        }

        public String ApplicantName
        {
            get
            {
                if (ViewState["ApplicantName"].IsNotNull())
                {
                    return Convert.ToString(ViewState["ApplicantName"]);
                }

                return String.Empty;
            }
            set
            {
                ViewState["ApplicantName"] = value;
            }
        }

        public Int32 ApplicantID
        {
            get
            {
                if (ViewState["ApplicantID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["ApplicantID"]);
                }

                return 0;
            }
            set
            {
                ViewState["ApplicantID"] = value;
            }
        }

        public Int32? RushOrderStatusId
        {
            get
            {
                if (ViewState["RushOrderStatusId"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["RushOrderStatusId"]);
                }

                return null;
            }
            set
            {
                ViewState["RushOrderStatusId"] = value;
            }
        }

        public Int32? HierarchyID
        {
            get
            {
                if (ViewState["HierarchyID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["HierarchyID"]);
                }

                return null;
            }
            set
            {
                ViewState["HierarchyID"] = value;
            }
        }

        public Int32? SelectedNodeID
        {
            get
            {
                if (ViewState["SelectedNodeID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["SelectedNodeID"]);
                }

                return null;
            }
            set
            {
                ViewState["SelectedNodeID"] = value;
                hdnSelectedNodeIds.Value = value.ToString();
            }
        }
        #endregion

        #region SEND NOTIFICATION FOR FIRST ITEM SUBMITT
        public String FirstName
        {
            get
            {
                if (ViewState["FirstName"].IsNotNull())
                {
                    return Convert.ToString(ViewState["FirstName"]);
                }

                return String.Empty;
            }
            set
            {
                ViewState["FirstName"] = value;
            }
        }
        public String LastName
        {
            get
            {
                if (ViewState["LastName"].IsNotNull())
                {
                    return Convert.ToString(ViewState["LastName"]);
                }

                return String.Empty;
            }
            set
            {
                ViewState["LastName"] = value;
            }
        }
        public String PrimaryEmailAddress
        {
            get
            {
                if (ViewState["PrimaryEmailAddress"].IsNotNull())
                {
                    return Convert.ToString(ViewState["PrimaryEmailAddress"]);
                }

                return String.Empty;
            }
            set
            {
                ViewState["PrimaryEmailAddress"] = value;
            }
        }
        #endregion

        public Boolean Visiblity
        {
            get
            {
                if (ViewState["Visiblity"] != null)
                {
                    return (Convert.ToBoolean(ViewState["Visiblity"]));
                }

                return true;
            }
            set
            {
                ViewState["Visiblity"] = value;
            }
        }

        public String ComplianceStatusText
        {
            get;
            set;
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

        //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
        public Boolean ReloadVerificationPermsFromDB
        {
            get
            {
                if (!ViewState["ReloadVerificationPermsFromDB"].IsNullOrEmpty())
                {
                    return (Boolean)(ViewState["ReloadVerificationPermsFromDB"]);
                }

                return false;
            }
            set
            {
                ViewState["ReloadVerificationPermsFromDB"] = value;
            }
        }

        #region UAT-1607:Student Data Entry Screen changes
        List<ItemSery> ICompliancePackageDetailsView.lstItemSeries { get; set; }
        public Boolean IsItemSeriesDataToSave { get; set; }

        List<Int32> ICompliancePackageDetailsView.ExpiringItemList { get; set; }
        #endregion

        #region UAT-1137:Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
        public List<String> lstExplanatoryNotes { get; set; }
        public List<ComplianceItem> lstNotAllowedDataEntryItems { get; set; }
        #endregion

        public Dictionary<Int32, Boolean> LstComplncRqdMapping
        {
            get
            {
                if (ViewState["LstComplncRqdMapping"].IsNotNull())
                {
                    return (Dictionary<Int32, Boolean>)(ViewState["LstComplncRqdMapping"]);
                }

                return new Dictionary<Int32, Boolean>();
            }
            set
            {
                ViewState["LstComplncRqdMapping"] = value;
            }
        }

        public Boolean IsCallingFromRotationDetailScreen
        {
            get;
            set;
        }
        public Boolean IsUiRulesViolate { get; set; }
        /// <summary>
        /// Dictionary contains the category id and its items included series items also.
        /// </summary>
        public Dictionary<Int32, List<Int32>> DicCategoryDataForItemSeriesRule { get; set; }

        //UAT-1811
        public List<INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail> ComplianceDetailList { get; set; }

        #region UAT-2028:Expired items should also show in the Enter Requirements item selection dropdown on the student screen
        List<Int32> ICompliancePackageDetailsView.lstMappedItems
        {
            get
            {
                if (ViewState["lstMappedItems"].IsNotNull())
                {
                    return (List<Int32>)(ViewState["lstMappedItems"]);
                }

                return new List<Int32>();
            }
            set
            {
                ViewState["lstMappedItems"] = value;
            }
        }
        #endregion

        #region UAT-2159 : Show Category Explanatory note as a mouseover on the category name on the student data entry screen.
        Dictionary<Int32, String> ICompliancePackageDetailsView.dicCatExplanatoryNotes { get; set; }
        #endregion

        public Boolean IsApplicant
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user.IsNotNull())
                {
                    return user.IsApplicant;
                }
                return false;
            }

        }

        public Boolean IsPackageActive
        { get; set; }

        #region UAT-3240
        Boolean ICompliancePackageDetailsView.IsDisabledBothCategoryAndItemExceptionsForTenant
        {
            get
            {
                if (ViewState["IsDisabledBothCategoryAndItemExceptionsForTenant"].IsNotNull())
                {
                    return Convert.ToBoolean(ViewState["IsDisabledBothCategoryAndItemExceptionsForTenant"]);
                }

                return false;
            }
            set
            {
                ViewState["IsDisabledBothCategoryAndItemExceptionsForTenant"] = value;
            }
        }

        #endregion

        String ICompliancePackageDetailsView.QuizConfigSetting
        {
            get
            {
                if (ViewState["QuizConfigSetting"].IsNotNull())
                {
                    return Convert.ToString(ViewState["QuizConfigSetting"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["QuizConfigSetting"] = value;
            }
        }

        #region UAT-3392

        Boolean ICompliancePackageDetailsView.IsItemEnterDataClick
        {
            get;
            set;
        }


        #endregion

        //UAT-3639
        public String DropzoneID
        {
            get { return this._dropZoneId; }
            set { _dropZoneId = value; }
        }
        private String _dropZoneId = "CompliancePkgDropZone" + DateTime.Now.Ticks.ToString();

        public ComplianceItem SelectedItemDetails
        {
            get
            {
                if (ViewState["SelectedItemDetails"] != null)
                {
                    return ViewState["SelectedItemDetails"] as ComplianceItem;
                }

                return new ComplianceItem();
            }
            set
            {
                ViewState["SelectedItemDetails"] = value;
            }
        }

        Boolean ICompliancePackageDetailsView.IsAutoSubmit
        {
            get
            {
                if (!ViewState["IsAutoSubmit"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsAutoSubmit"]);
                }

                return false;
            }
            set
            {
                ViewState["IsAutoSubmit"] = value;
            }

        }

        Int32 ICompliancePackageDetailsView.NodeID
        {
            get
            {
                if (!ViewState["NodeID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(ViewState["NodeID"]);
                }

                return AppConsts.NONE;
            }
            set
            {
                ViewState["NodeID"] = value;
            }

        }

        public List<ApplicantDocument> ToSaveApplicantUploadedDocuments { get; set; }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

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
                //change done for applicant dashboard redesign.
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    _viewType = Request.QueryString[AppConsts.UCID] == null ? String.Empty : Request.QueryString[AppConsts.UCID];
                    base.OnInit(e);

                    SetQueryStringValues();
                    if (!(this.ControlUseType == AppConsts.DASHBOARD))
                    {
                        if (String.IsNullOrEmpty(CurrentViewContext.WorkQueue))
                        {
                            base.Title = "Data Entry";
                        }
                        else
                        {
                            base.Title = "Applicant's View";
                        }
                        //lblManageTenant.Text = base.Title;
                    }

                    /*UAT-2751*/
                    if (!IsApplicant)
                    {
                        divApplicantName.Style.Add("display", "block");

                        lblApplicantName.InnerText = Presenter.GetApplicantNameByApplicantId(ApplicantID, TenantID).HtmlEncode();
                    }
                    /*UAT-2751 End here*/
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

        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            hdnCompliancePackageDetails.Value = "true";
            hdnEnterRequirement.Value = "";
            Page.MaintainScrollPositionOnPostBack = true;
            //change done for applicant dashboard redesign.
            if (Visiblity.IsNullOrEmpty() || Visiblity == true)
            {
                if (IsCallingFromRotationDetailScreen)
                {
                    //hdnPostbacksource.Text = "Other";
                    hdnNeedToHideDocumentAttributeSection.Value = "true";
                }

                if (this.hdnPostbacksource.Text != "DE" && this.ControlUseType == AppConsts.DASHBOARD)
                {
                    ResetControlsOnFalsePostback();
                }
                if (!this.IsPostBack || (!(this.hdnPostbacksource.Text == "DE") && this.ControlUseType == AppConsts.DASHBOARD) && !IsCallingFromRotationDetailScreen)
                {
                    Presenter.OnViewInitialized();
                    tlistComplianceData.ExpandAllItems();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowDataEntryTooltip();", true);
                    Presenter.IsDisabledBothCategoryAndItemExceptionsForTenant(CurrentViewContext.TenantID);//UAT-3240
                }


                Presenter.OnViewLoaded();
                if (!(this.ControlUseType == AppConsts.DASHBOARD))
                {
                    if (String.IsNullOrEmpty(CurrentViewContext.WorkQueue))
                    {
                        base.SetPageTitle("Data Entry");
                    }
                    else
                    {
                        base.SetPageTitle("Applicant's View");
                        base.Title = "Applicant's View";
                    }
                }
                if (this.ControlUseType == AppConsts.DASHBOARD)
                {
                    tblComplBar.Visible = false;
                }

                string _immunSumaryReportUrl = String.Empty;
                String myscript = "var tenantId=" + TenantID;
                myscript += "; _packageSubscriptionId='" + CurrentViewContext.PackageSubscriptionId + "'; var _rptUrl='" + _immunSumaryReportUrl + "';";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", myscript, true);

                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TenantId", Convert.ToString(CurrentViewContext.TenantID) },
                                                                    { "Child", ChildControls.ManageUploadDocuments}
                                                                 };
                if (this.IsAdminView)
                {
                    ancViewSubscription.Disabled = true;
                    ancUpload.Disabled = true;
                    ancViewSubscription.HRef = ancUpload.HRef = String.Empty;
                    if (this.ControlUseType != AppConsts.DASHBOARD)
                    {
                        lnkBackSearch.Visible = true;
                        lnkVerificationDetails.Visible = true;
                        SetUrlForVerificationDetails();
                        SetUrlForSearchScreen();
                    }
                }
                else
                {
                    ancViewSubscription.Disabled = false;
                    ancUpload.Disabled = false;
                    ancUpload.HRef = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                }
                dvComplianceStatus.Style.Add("display", "block");
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                dvComplianceStatus.RenderControl(htw);
                ComplianceStatusText = sw.GetStringBuilder().ToString();
                if (ControlUseType == AppConsts.DASHBOARD)
                {
                    dvComplianceStatus.Style.Add("display", "none");
                    dvPkgInfo.Style.Add("display", "none");

                }
                // UAT 4300 If "Complete Document" is only attribute for an item, item should auto-submit from student screen when document is completed
                String controlName = this.Page.Request.Params["__EVENTTARGET"];
                CurrentViewContext.IsAutoSubmit = false;
                if (!String.IsNullOrEmpty(controlName) && controlName.Contains("btnAutoSubmit"))
                {
                    btnAutoSubmit_Click(sender, e);
                    // tlistRequirementData_ItemCommand(sender, e);
                }
            }
            hdnSignatureMinLengh.Value = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings[INTSOF.Utils.AppConsts.MIN_PROFILE_SHARING_SIGN_LENGTH_KEY]);
            CurrentViewContext.IsItemEnterDataClick = false;
        }

        #endregion

        #region Treelist Events

        protected void tlistComplianceData_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                //change done for applicant dashboard redesign.
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {

                    String _incompleteItemName = String.Empty;
                    if (String.IsNullOrEmpty(CurrentViewContext.WorkQueue))
                    {
                        _incompleteItemName = Presenter.GetClientCompliancePackage();
                    }
                    else
                    {
                        _incompleteItemName = Presenter.GetClientCompliancePackageBySubscription();
                    }

                    if (CurrentViewContext.Subscription.lkpSubscriptionMobilityStatu.IsNotNull())
                    {
                        this.SubscriptionMobilityStatusCode = CurrentViewContext.Subscription.lkpSubscriptionMobilityStatu.Code;
                    }
                    else
                    {
                        this.SubscriptionMobilityStatusCode = null;
                    }

                    List<Int32> expItemList = lstExpiringItem.Where(cond => cond.ShowUpdateDelete == 1
                                                                    && cond.ItemComplianceStatus != ApplicantItemComplianceStatus.Pending_Review.GetStringValue()
                                                                    && cond.ItemComplianceStatus != ApplicantItemComplianceStatus.Not_Approved.GetStringValue()
                                                                    )
                                                             .Select(cond => cond.ComplianceItemID).ToList();

                    //UAT-1413:Add note to data entry screen when student has editable upcoming expiration item.
                    List<Int32> expiringPendingItemLst = lstExpiringItem.Where(cond => cond.ItemComplianceStatus == ApplicantItemComplianceStatus.Pending_Review.GetStringValue())
                                                                        .Select(cond => cond.ComplianceItemID).ToList();
                    //UAT-2028:
                    List<Int32> expiringNotApprdItemLst = lstExpiringItem.Where(cond => cond.ItemComplianceStatus == ApplicantItemComplianceStatus.Not_Approved.GetStringValue())
                                                                        .Select(cond => cond.ComplianceItemID).ToList();

                    ViewState["expItemList"] = expItemList;
                    ViewState["expiringPendingItemLst"] = expiringPendingItemLst;
                    ViewState["expiringNotApprdItemLst"] = expiringNotApprdItemLst;

                    List<ListCategoryEditableBies> listCategoryEditableBies = new List<ListCategoryEditableBies>();

                    listCategoryEditableBies = Presenter.GetEditableBiesByPackage();
                    _listCategoryEditableBies = listCategoryEditableBies;

                    #region UAT-1607:
                    List<ItemSeriesItemContract> lstItemSeriesItems = Presenter.GetItemSeriesItemForCategories();
                    #endregion

                    #region UAT-2413
                    String EnterRequirementText = Presenter.GetEnterRequirementsText();
                    if (EnterRequirementText != AppConsts.ENTER_REQUIREMENT_TEXT)
                    {
                        tltReqHelp.Visible = false;
                    }
                    else
                    {
                        tltReqHelp.Visible = true;
                    }
                    #endregion

                    bool isNeedToHideDocumentLink = IsCallingFromRotationDetailScreen ? true : false;
                    //TODO: GET All item payment for selected subscription
                    List<ItemPaymentContract> ItemPaymentList = Presenter.GetItemPaymentDetail(Subscription.PackageSubscriptionID);
                    //String OptionalCategoryClientSetting = Presenter.GetClientSettingsByCode(Setting.EXECUTE_COMPLIANCE_RULE_WHEN_OPTIONAL_CATEGORY_COMPLIANCE_RULE_MET.GetStringValue()); //commented for UAT 3683
                    String OptionalCategoryClientSetting = AppConsts.ZERO;
                    Boolean OptionalCategoryClientSettingBool = ComplianceDataManager.GetOptionalCategorySettingForNode(CurrentViewContext.TenantID, AppConsts.NONE, Subscription.PackageSubscriptionID, SubscriptionTypeCategorySetting.COMPLIANCE_PACKAGE.GetStringValue());
                    if (OptionalCategoryClientSettingBool)
                    {
                        OptionalCategoryClientSetting = AppConsts.STR_ONE;
                    }
                    else
                    {
                        OptionalCategoryClientSetting = AppConsts.ZERO;
                    }

                    Presenter.IsDisabledBothCategoryAndItemExceptionsForTenant(CurrentViewContext.TenantID);
                    Presenter.GetQuizConfigurationSetting(); //UAT 3299
                    ComplianceDetailsContract complianceDetailsContract = new ComplianceDetailsContract(ClientPackage, Subscription, expItemList, _incompleteItemName,
                                                                                                        listCategoryEditableBies, expiringPendingItemLst, lstItemSeriesItems,
                                                                                                        LstComplncRqdMapping, isNeedToHideDocumentLink, expiringNotApprdItemLst, CurrentViewContext.dicCatExplanatoryNotes, EnterRequirementText, ItemPaymentList, OptionalCategoryClientSetting, CurrentViewContext.IsDisabledBothCategoryAndItemExceptionsForTenant, _listCategoryEditableBies);

                    //UAT 3299
                    foreach (INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail item in complianceDetailsContract.ComplianceDetails.Where(cond => cond.IsQuizItem).ToList())
                    {
                        if (CurrentViewContext.QuizConfigSetting == AppConsts.STR_ONE)
                        {
                            item.ShowItemEditDelete = false;
                            item.ShowItemDelete = false; //UAT-3511
                        }
                        else
                        {
                            item.ShowItemEditDelete = true;
                            item.ShowItemDelete = true; //UAT-3511
                        }
                    }
                    //UAT-977:Additional work towards archive ability
                    SetArchiveProperties();
                    //Fixed issue: When admin and client admin navigates to Applicant View screen from compliance search screen, Update and Delete buttons should be in disabled mode.
                    HideCrudButtonsForApplicantView(complianceDetailsContract);

                    var _lstOptional = complianceDetailsContract.ComplianceDetails.Where(cdc => cdc.IsComplianceRequired == false).ToList();

                    if (!_lstOptional.IsNullOrEmpty())
                    {
                        if (IsCallingFromRotationDetailScreen || !CurrentViewContext.WorkQueue.IsNullOrEmpty() && CurrentViewContext.WorkQueue == WorkQueueType.ComplianceSearch.ToString())
                        {
                            _lstOptional.ForEach(x => { x.ShowApplyException = false; });

                        }
                        _lstOptional.Insert(0, new INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail()
                        {
                            Category = new ComplianceCategory { Description = String.Empty },
                            ImgReviewStatus = String.Empty,
                            ImageReviewStatus = String.Empty,
                            IsCategoryDataEntered = false,
                            IsItemAboutToExpire = false,
                            IsParent = true,
                            Item = new ComplianceItem(),
                            ParentNodeID = String.Empty,
                            ReviewStatus = String.Empty,
                            Name = "Optional Compliance Category",
                            NodeID = AppConsts.DATA_ENTRY_OPTIONAL_CATEGORY_NODE,
                            ShowAddException = false,
                            ShowAddRequirement = false,
                            ShowEnterData = false,
                            ShowExceptionEditDelete = false,
                            ShowItemEditDelete = false,
                            ShowExceptionAllTimeUpdate = false  //UAT-4926
                             ,
                            ShowItemDelete = false //UAT-3511
                        });
                    }

                    var _lstRequired = complianceDetailsContract.ComplianceDetails.Where(cdc => cdc.IsComplianceRequired == true).ToList();

                    if (!_lstRequired.IsNullOrEmpty())
                    {
                        if (IsCallingFromRotationDetailScreen || !CurrentViewContext.WorkQueue.IsNullOrEmpty() && CurrentViewContext.WorkQueue == WorkQueueType.ComplianceSearch.ToString())
                        {
                            _lstRequired.ForEach(x => { x.ShowApplyException = false; });

                        }
                        _lstRequired.Insert(0, new INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail()
                        {
                            Category = new ComplianceCategory { Description = String.Empty },
                            ImgReviewStatus = String.Empty,
                            ImageReviewStatus = String.Empty,
                            IsCategoryDataEntered = false,
                            IsItemAboutToExpire = false,
                            IsParent = true,
                            Item = new ComplianceItem(),
                            ParentNodeID = String.Empty,
                            ReviewStatus = String.Empty,
                            Name = "Required Compliance Category",
                            NodeID = AppConsts.DATA_ENTRY_REQUIRED_CATEGORY_NODE
                        });
                    }

                    List<INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail> _lstCombined = new List<INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail>();

                    _lstCombined.AddRange(_lstRequired);
                    _lstCombined.AddRange(_lstOptional);

                    //UAT-1811
                    ComplianceDetailList = _lstCombined;
                    tlistComplianceData.DataSource = _lstCombined;

                    //[SG]: UAT-1036 - (Add Compliance count to student dashboard)
                    if (complianceDetailsContract.ComplianceDetails != null)
                    {
                        lblComplianceCategoryStatus.Visible = true;
                        //Exceptionally Approved
                        //CATEGORY_EXCEPTIONALLY_APPROVED is added as on 'Applicant Data Entry screen' total count for 
                        //approved categories is fetched based on review status
                        int approvedCategoryCount = complianceDetailsContract.ComplianceDetails.Where(cd => cd.IsParent == true
                                                                                                      && (cd.ReviewStatusCode.Equals(ApplicantCategoryComplianceStatus.Approved.GetStringValue())
                                                                                                          || cd.ReviewStatusCode.Equals(ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue()))
                                                                                                      ).Count();
                        int totalCategoryCount = complianceDetailsContract.ComplianceDetails.Where(cd => cd.IsParent == true).Count();
                        lblComplianceCategoryStatus.Text = String.Format("({0}/{1} Compliant)", approvedCategoryCount, totalCategoryCount);
                    }

                    CurrentViewContext.PackageSubscriptionId = CurrentViewContext.Subscription.PackageSubscriptionID;
                    hdfPackageSubscriptionID.Value = Convert.ToString(CurrentViewContext.Subscription.PackageSubscriptionID);
                    CurrentViewContext.ComplianceStatusID = CurrentViewContext.Subscription.ComplianceStatusID.Value;
                    CurrentViewContext.ComplianceStatus = CurrentViewContext.Subscription.lkpPackageComplianceStatu.Name;
                    //Call a method to set the properties required for handles assignment
                    SetPropertiesForQueueManagement(CurrentViewContext.Subscription);

                    litDaysLeft.Text = Convert.ToDateTime(Subscription.ExpiryDate).Subtract(DateTime.Now).Days + " days left";

                    String packageArchivestatus = complianceDetailsContract.Subscription.lkpArchiveState.AS_Code.ToString();

                    if (packageArchivestatus == ArchiveState.Active.GetStringValue() && complianceDetailsContract.Subscription.ExpiryDate > DateTime.Now)
                    {
                        IsPackageActive = true;
                    }
                    else
                    {
                        IsPackageActive = false;
                    }
                }
                else
                {
                    lblComplianceCategoryStatus.Visible = false;
                }
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                return;

            }
        }

        protected void tlistComplianceData_ItemCreated(object sender, TreeListItemCreatedEventArgs e)
        {
            SetQueryStringValues();
            var txtEditTime = e.Item.FindControl("txtEditTime") as TextBox;
            if (txtEditTime != null && !TextBoxClientID.IsNullOrEmpty() &&   TextBoxClientID== txtEditTime.ClientID && IsUpdateButtonCall)
            {
                txtEditTime.Focus();
                txtEditTime.Text = "Item Created";
            }

            if (e.Item is TreeListEditableItem && (e.Item as TreeListEditableItem).IsInEditMode)
            {
                TreeListEditFormItem item = e.Item as TreeListEditFormItem;
                if (item == null)
                {
                    return;
                }

                #region BIND THE LIST OF AVAILABLE ITEMS FOR THE APPLICANT DATA ENTRY

                WclComboBox cmbRequirement = item.FindControl("cmbRequirement") as WclComboBox;

                //UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                HtmlGenericControl divExplanatoryNoteItems = item.FindControl("dvExplanatoryNotesItem") as HtmlGenericControl;
                HtmlGenericControl divSelectRequirement = item.FindControl("divSelectRequirement") as HtmlGenericControl;
                Repeater rptExplanatoryNotes = item.FindControl("rptExplanatoryNotes") as Repeater;
                WclButton btnCancelExpNotes = item.FindControl("btnCancelExpNotes") as WclButton;
                HtmlGenericControl dvApplyException = item.FindControl("dvApplyException") as HtmlGenericControl;
                Panel pnlApplyForException = item.FindControl("pnlName1") as Panel;
                HtmlGenericControl dvAddNewRequirement = item.FindControl("dvAddNewRequirement") as HtmlGenericControl;

                if (cmbRequirement.IsNotNull())
                {
                    #region UAT-1607
                    List<Int32> expItemList = new List<Int32>();
                    if (ViewState["expItemList"] != null)
                    {
                        expItemList = (List<Int32>)ViewState["expItemList"];
                    }
                    CurrentViewContext.ExpiringItemList = expItemList;
                    #endregion
                    //UAT-2028:
                    List<Int32> expPendingItemList = new List<Int32>();
                    List<Int32> expiringNotApprdItemLst = new List<Int32>();
                    List<Int32> finalExpItemList = new List<Int32>();
                    finalExpItemList.AddRange(CurrentViewContext.ExpiringItemList);
                    if (ViewState["expiringPendingItemLst"] != null)
                    {
                        expPendingItemList = (List<Int32>)ViewState["expiringPendingItemLst"];
                        finalExpItemList.AddRange(expPendingItemList);
                    }
                    if (ViewState["expiringNotApprdItemLst"] != null)
                    {
                        expiringNotApprdItemLst = (List<Int32>)ViewState["expiringNotApprdItemLst"];
                        finalExpItemList.AddRange(expiringNotApprdItemLst);
                    }
                    Presenter.GetClientComplianceItems(finalExpItemList);
                    //Hide in case if no item available for exception and data entry.
                    //UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                    if (_isEnterRequirementClick && (CurrentViewContext.lstAvailableItems == null || CurrentViewContext.lstAvailableItems.Count == AppConsts.NONE)
                        && CurrentViewContext.lstExplanatoryNotes.IsNotNull() && CurrentViewContext.lstExplanatoryNotes.Count > AppConsts.NONE)
                    {
                        divSelectRequirement.Visible = false;
                        dvApplyException.Style["Padding-Left"] = "5px";
                        pnlApplyForException.Style["min-height"] = "40px";
                    }

                    //UAT-4300
                    if (!CurrentViewContext.IsAutoSubmit.IsNullOrEmpty() && CurrentViewContext.IsAutoSubmit)
                    {
                        dvAddNewRequirement.Visible = false;
                    }
                    //END

                    //UAT-2609
                    var CurrentCategoryDetails = new INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail();
                    if (!tlistComplianceData.DataSource.IsNullOrEmpty())
                    {
                        CurrentCategoryDetails = (tlistComplianceData.DataSource as List<INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail>).Where(cond => cond.CategoryId == CurrentViewContext.ComplianceCategoryId).FirstOrDefault();
                    }
                    LinkButton btnAddException = pnlApplyForException.FindControl("btnAddException") as LinkButton;
                    if (!CurrentCategoryDetails.IsNullOrEmpty() && !btnAddException.IsNullOrEmpty())
                    {
                        if (CurrentCategoryDetails.HideExceptionLinkForApprovedCat)
                        {
                            btnAddException.Visible = false;
                        }
                        else
                        {
                            btnAddException.Visible = true;
                        }
                    }
                    else
                    {
                        btnAddException.Visible = true;
                    }

                    if (CurrentViewContext.lstAvailableItems.IsNotNull())
                    {
                        CurrentViewContext.lstAvailableItems.Insert(AppConsts.NONE, new ComplianceItem { ItemLabel = AppConsts.COMBOBOX_ITEM_SELECT, ComplianceItemID = AppConsts.NONE, CompItemID = AppConsts.ZERO });
                        foreach (var availableItem in lstAvailableItems)
                        {
                            availableItem.ItemLabel = !String.IsNullOrEmpty(availableItem.ItemLabel) ? availableItem.ItemLabel : availableItem.Name;
                        }
                    }

                    cmbRequirement.DataSource = CurrentViewContext.lstAvailableItems;
                    cmbRequirement.Items.Insert(AppConsts.NONE, new RadComboBoxItem { Text = AppConsts.COMBOBOX_ITEM_SELECT, Value = AppConsts.ZERO });
                    cmbRequirement.DataBind();

                    cmbRequirement.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cmbRequirement_SelectedIndexChanged);

                    //UAT-2453
                    Boolean IsEntryAllowed = false;
                    if (hidEditForm.Value == "0" && _isEnterRequirementClick)
                    {
                        if (CurrentViewContext.lstAvailableItems.IsNotNull() && CurrentViewContext.lstAvailableItems.Count == AppConsts.TWO)
                        {
                            cmbRequirement.SelectedValue = CurrentViewContext.lstAvailableItems[1].CompItemID.ToString();
                            cmbRequirement.SelectedItem.Attributes["IsItemSeries"] = Convert.ToString(CurrentViewContext.lstAvailableItems[1].IsItemSeries);
                            //RadComboBoxSelectedIndexChangedEventArgs rargs = null;
                            ItemForm itemForm = item.FindControl("itemForm") as ItemForm;
                            if (!itemForm.IsNullOrEmpty())
                            {
                                itemForm.LoadControl();
                                IsEntryAllowed = true;
                            }
                            cmbRequirement_SelectedIndexChanged(cmbRequirement, null);
                        }
                    }

                    //UAT-1264: WB: As a student, I should be able to enter the data for an item (or submit a new exception) when an item exception expires
                    if (hidEditForm.Value == "4")
                    {
                        cmbRequirement.SelectedValue = Convert.ToString(CurrentViewContext.ItemId);
                        cmbRequirement.Enabled = false;
                        cmbRequirement_SelectedIndexChanged(cmbRequirement, null);
                    }

                    #region UAT-1607: Student Data Entry Screen changes
                    cmbRequirement.ItemDataBound += new RadComboBoxItemEventHandler(cmbRequirement_ItemDataBound);
                    #endregion


                    //UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                    if (_isEnterRequirementClick && CurrentViewContext.lstExplanatoryNotes.IsNotNull() && CurrentViewContext.lstExplanatoryNotes.Count > AppConsts.NONE
                        && divExplanatoryNoteItems.IsNotNull() && rptExplanatoryNotes.IsNotNull() && !IsEntryAllowed)
                    {
                        divExplanatoryNoteItems.Visible = true;
                        rptExplanatoryNotes.DataSource = CurrentViewContext.lstExplanatoryNotes;
                        rptExplanatoryNotes.DataBind();
                        btnCancelExpNotes.Visible = true;
                    }
                    else
                    {
                        divExplanatoryNoteItems.Visible = false;
                    }
                }

                #endregion

                #region BIND THE CATEGORY TEXT AND ITS NOTES

                Label lblForm = item.FindControl("lblForm") as Label;
                if (lblForm.IsNotNull())
                {
                    if (CurrentViewContext.SelectedCategory.IsNotNull())
                    {
                        lblForm.Text = String.Format("<span class='expl-title'>{0}</span><span class='expl-dur'>: </span>{1}{2}",
                           CurrentViewContext.SelectedCategory.CategoryName,
                           CurrentViewContext.SelectedCategory.ExpNotes,
                           GenerateExplanatoryNotesLinkList(CurrentViewContext.SelectedCategory));
                    }
                    else
                    {
                        lblForm.Text = String.Empty;
                    }

                    Control itemControl = item.FindControl("itemForm") as Control;
                }

                #endregion

                #region MANAGE THE SHOW/HIDE OF THE COMMAND BAR

                Control ucCommandBar = item.FindControl("cmdBar") as Control;
                if (ucCommandBar.IsNotNull() && CurrentViewContext.ItemId > 0)
                {
                    var ItemDetails = Presenter.ItemDetails(CurrentViewContext.ItemId);
                    if (!ItemDetails.IsNullOrEmpty())
                    {
                        var complianceAttribiute = ItemDetails.ComplianceItemAttributes.Where(cond => !cond.CIA_IsDeleted).ToList();
                        if (complianceAttribiute.IsNullOrEmpty() || complianceAttribiute.Count == AppConsts.NONE)
                        {
                            ShowCommandBar(ucCommandBar, false);
                            btnCancelExpNotes.Visible = true;
                            btnCancelExpNotes.ToolTip = "Click to close";
                            btnCancelExpNotes.Text = "Close";
                        }
                        else
                        {
                            // Show command bar only if the case is edit mode. Add mode is managed through the dropdownlist
                            ShowCommandBar(ucCommandBar, true);
                            (ucCommandBar as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to save and submit the entered data for this requirement";
                            (ucCommandBar as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
                        }
                    }
                    else
                    {
                        // Show command bar only if the case is edit mode. Add mode is managed through the dropdownlist
                        ShowCommandBar(ucCommandBar, true);
                        (ucCommandBar as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to save and submit the entered data for this requirement";
                        (ucCommandBar as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
                    }
                }

                Control ucExceptionCommandBar = item.FindControl("fsucCmdBar1") as Control;
                if (ucExceptionCommandBar.IsNotNull())
                {
                    (ucExceptionCommandBar as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to save and submit the entered data for this requirement";
                    (ucExceptionCommandBar as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
                }

                #endregion

                #region Exception

                var cmbExceptionItems = item.FindControl("cmbExceptionItems") as WclComboBox;
                var cmbUploadDocument = item.FindControl("cmbUploadDocument") as WclComboBox;
                var applicantComplianceItem = item.FindControl("hdnApplicantComplianceItemId") as HiddenField;
                var complianceItemId = item.FindControl("hdnComplianceItemId") as HiddenField;
                IEnumerable<ExceptionDocumentMapping> applicationMappingData = null;
                if (hidEditForm.Value == "1")
                {
                    var uploadedDocument = Presenter.GetApplicantUploadedDocuments();
                    if (ApplicantComplianceItemId == 0)
                    {
                        foreach (var exceptionDocuments in uploadedDocument)
                        {
                            //cmbUploadDocument.Items.Add(new RadComboBoxItem
                            //{
                            //    Text = exceptionDocuments.FileName,
                            //    Value = exceptionDocuments.ApplicantDocumentID.ToString()
                            //});
                            // UAT-210 Adding description to each uploaded file and commented the above code.
                            RadComboBoxItem expitem = new RadComboBoxItem { Text = exceptionDocuments.FileName, Value = Convert.ToString(exceptionDocuments.ApplicantDocumentID) };
                            expitem.Attributes["desc"] = exceptionDocuments.Description;
                            cmbUploadDocument.Items.Add(expitem);
                        }
                    }
                    else
                    {
                        var exceptionComments = (WclTextBox)item.FindControl("txtExceptionComments");
                        var rdbCategoryLevel = (WclButton)item.FindControl("rdbItemLevel");
                        var rdbItemLevel = (WclButton)item.FindControl("rdbCategoryLevel");
                        //var pnlSupportingDocs = (HtmlGenericControl)item.FindControl("pnlSupportingDocs");
                        var uploadControl = (WclAsyncUpload)item.FindControl("uploadControl");
                        //UAT-3639
                        String[] dropzone = new String[] { "#" + this.DropzoneID };
                        uploadControl.DropZones = dropzone;
                        uploadControl.Localization.Select = "Hidden";

                        //UAT-4067
                        String[] allowedFileExtensions = CurrentViewContext.allowedFileExtensions.IsNullOrEmpty() ? String.Empty.Split(',') : CurrentViewContext.allowedFileExtensions.ToArray();
                        uploadControl.AllowedFileExtensions = allowedFileExtensions;

                        var applicantComplianceItemData = Presenter.GetApplicantComplianceItemData(ApplicantComplianceItemId);
                        applicantComplianceItem.Value = ApplicantComplianceItemId.ToString();
                        complianceItemId.Value = applicantComplianceItemData.ComplianceItemID.ToString();
                        rdbCategoryLevel.Visible = rdbItemLevel.Visible = false;
                        Control divApplyfor = (Control)item.FindControl("divApplyfor");
                        if (divApplyfor != null)
                        {
                            divApplyfor.Visible = false;
                        }
                        exceptionComments.Text = applicantComplianceItemData.ExceptionReason;
                        applicationMappingData = applicantComplianceItemData.ExceptionDocumentMappings;

                        foreach (var exceptionDocuments in uploadedDocument)
                        {
                            var isChecked = applicationMappingData.Any(cond =>
                                    !cond.IsDeleted &&
                                    cond.ApplicantDocumentID == exceptionDocuments.ApplicantDocumentID);
                            //cmbUploadDocument.Items.Add(new RadComboBoxItem
                            //{
                            //    Text = exceptionDocuments.FileName,
                            //    Checked = isChecked,
                            //    Value = exceptionDocuments.ApplicantDocumentID.ToString()
                            //});
                            // UAT-210 Adding description to each uploaded file and commented the above code. 
                            RadComboBoxItem expitem = new RadComboBoxItem { Text = exceptionDocuments.FileName, Value = Convert.ToString(exceptionDocuments.ApplicantDocumentID), Checked = isChecked, };
                            expitem.Attributes["desc"] = exceptionDocuments.Description;
                            cmbUploadDocument.Items.Add(expitem);
                        }

                        #region UAT-1864 : As an applicant, I should be able to preview documents in the document selection dropdown on the submit item screen.
                        List<RadComboBoxItem> cmb = cmbUploadDocument.CheckedItems.ToList();
                        ShowHideDocumentPreview(cmb, (item.FindControl("dvDocumentPreview") as HtmlGenericControl), (item.FindControl("pnlDocumentPreview") as Panel));
                        #endregion

                        //pnlSupportingDocs.Visible = true;
                        uploadControl.MaxFileSize = MaxFileSize;
                    }
                    //UAT-210
                    //Attaching client side event handler for combobox upload event
                    //This handler should be decalard in global scope of the script
                    cmbUploadDocument.OnClientLoad = "efn_atrFileUpdOnLoad";


                }
                //UAT-1607: Student Data Entry Screen changes
                //ignore item series from exception items.
                var availableItems = !CurrentViewContext.lstAvailableItems.IsNullOrEmpty() ? CurrentViewContext.lstAvailableItems.Where(cnd => !cnd.IsItemSeries)
                                                                                             : CurrentViewContext.lstAvailableItems;
                //UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                var lstNotAllowedItems = CurrentViewContext.lstNotAllowedDataEntryItems;
                var exceptionItems = new List<ComplianceItem>();

                if (availableItems != null)
                {
                    var hdnComplianceCategoryId = item.FindControl("hdnComplianceCategoryId") as HiddenField;
                    hdnComplianceCategoryId.Value = CurrentViewContext.ComplianceCategoryId.ToString();

                    exceptionItems.AddRange(availableItems);
                    exceptionItems.RemoveAt(0);
                }
                //UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                if (lstNotAllowedItems != null)
                {
                    exceptionItems.AddRange(lstNotAllowedItems);
                }
                exceptionItems = Presenter.GetOrderedComplianceItem(exceptionItems);
                //UAT-2028:Expired items should also show in the Enter Requirements item selection dropdown on the student screen
                //Commented below code on [03/08/2016] to resolved the "Apply For exception" ISSUE.
                //if (!Presenter.IsAnyItemPendingForDataEntry(lstNotAllowedItems, CurrentViewContext.lstAvailableItems))
                //{
                //    dvApplyException.Visible = false;
                //}
                if (exceptionItems != null)
                {
                    //UAT-2028:Expired items should also show in the Enter Requirements item selection dropdown on the student screen
                    exceptionItems = exceptionItems.Where(x => !CurrentViewContext.lstMappedItems.Contains(x.ComplianceItemID)).ToList();
                    foreach (var complianceItems in exceptionItems)
                    {
                        complianceItems.ItemLabel = !String.IsNullOrEmpty(complianceItems.ItemLabel) ? complianceItems.ItemLabel : complianceItems.Name;
                    }

                    //cmbExceptionItems.EmptyMessage = "--SELECT--";

                    //exceptionItems.Insert(0, new ComplianceItem { ComplianceItemID = AppConsts.NONE, Name = AppConsts.COMBOBOX_ITEM_SELECT });
                    cmbExceptionItems.DataSource = exceptionItems;
                    cmbExceptionItems.DataBind();
                    cmbExceptionItems.EmptyMessage = "--SELECT--";
                    //cmbExceptionItems.Items.Insert(0, new RadComboBoxItem { Value = AppConsts.ZERO, Text = AppConsts.COMBOBOX_ITEM_SELECT });

                }



                #endregion


                #region HIDE APPLY FOR EXCEPTION LINK

                if (IsExceptionNotAllowed() || CurrentViewContext.IsDisabledBothCategoryAndItemExceptionsForTenant)//UAT-3240
                {
                    LinkButton btnAddException = item.FindControl("btnAddException") as LinkButton;
                    if (btnAddException != null)
                    {
                        btnAddException.Visible = false;
                    }
                }
                #endregion

                var panelException = (Panel)item.FindControl("pnlExceptionForm");
                var panelpnlEntryForm = (Panel)item.FindControl("pnlEntryForm");

                //var pnlMessage = (Panel)item.FindControl("pnlMessage");

                ShowHidePanel(panelException, panelpnlEntryForm);
                //Added one more check of hideEditForm for deletion of ItemData and _isValFailForDeletion to show the info message 
                //InCase:[Deletion of Approved And expired item. [Bug id: 9245:]] 
                if ((hidEditForm.Value == "3" || hidEditForm.Value == "2")) // 3 - Deletion of the Exception // 2 - Deletion of the Item Data
                {
                    panelException.Visible = false;
                    panelpnlEntryForm.Visible = false;
                    //pnlMessage.Visible = false;
                    if (_isValFailForDeletion)
                    {
                        base.ShowInfoMessage("This Compliance Item has already been approved. Approved Compliance Item cannot be deleted.");
                    }
                }
                //UAT 3189 As a student, I should be able to apply for a category exception when all items in the category have been entered.   
                if (hdnApplyForException.Value == "1")
                {
                    hidEditForm.Value = "1";
                    panelException.Visible = true;
                    panelpnlEntryForm.Visible = false;
                    var pnlExceptionForm = (Panel)item.FindControl("pnlExceptionForm");
                    var cmbUploadDocument1 = item.FindControl("cmbUploadDocument") as WclComboBox;
                    //var applicantComplianceItem = pnlExceptionForm.FindControl("hdnApplicantComplianceItemId") as HiddenField;
                    //pnlEntryForm.Visible = false;
                    //pnlExceptionForm.Visible = true;
                    var uploadedDocument = Presenter.GetApplicantUploadedDocuments();
                    if (ApplicantComplianceItemId == 0)
                    {
                        foreach (var exceptionDocuments in uploadedDocument)
                        {
                            // UAT-210 Adding description to each uploaded file and commented the above code.
                            RadComboBoxItem expitem = new RadComboBoxItem { Text = exceptionDocuments.FileName, Value = Convert.ToString(exceptionDocuments.ApplicantDocumentID) };
                            expitem.Attributes["desc"] = exceptionDocuments.Description;
                            cmbUploadDocument1.Items.Add(expitem);
                        }
                    }
                    //UAT-210
                    //Attaching client side event handler for combobox upload event
                    //This handler should be decalard in global scope of the script
                    cmbUploadDocument1.OnClientLoad = "efn_atrFileUpdOnLoad";

                    if (Subscription != null)
                    {
                        if (Subscription.ApplicantComplianceCategoryDatas != null)
                        {
                            var rdbCategoryLevel = (WclButton)item.FindControl("rdbCategoryLevel");
                            var rdbItemLevel = (WclButton)item.FindControl("rdbItemLevel");
                            var pnlExceptionItems = (Panel)item.FindControl("pnlExceptionItems");
                            //var pnlSupportingDocs = (HtmlGenericControl)insertItem.FindControl("pnlSupportingDocs");
                            var hdnIsCategoryView = (HiddenField)item.FindControl("hdnIsCategoryView");
                            var uploadControl = (WclAsyncUpload)item.FindControl("uploadControl");

                            //#region UAT-3639
                            String[] dropzone = new String[] { "#" + this.DropzoneID };
                            uploadControl.DropZones = dropzone;
                            uploadControl.Localization.Select = "Hidden";
                            //#endRegion

                            //UAT-4067
                            String[] allowedFileExtensions = CurrentViewContext.allowedFileExtensions.IsNullOrEmpty() ? String.Empty.Split(',') : CurrentViewContext.allowedFileExtensions.ToArray();
                            uploadControl.AllowedFileExtensions = allowedFileExtensions;

                            String exceptionExpiredCode = lkpCategoryExceptionStatus.EXCEPTION_EXPIRED.GetStringValue();

                            ApplicantComplianceCategoryData appCmpCatData = Subscription.ApplicantComplianceCategoryDatas.FirstOrDefault(x => x.ComplianceCategoryID == CurrentViewContext.ComplianceCategoryId && !x.IsDeleted);
                            if (appCmpCatData.IsNotNull() && appCmpCatData.lkpCategoryExceptionStatu.IsNotNull() && appCmpCatData.lkpCategoryExceptionStatu.CES_Code != exceptionExpiredCode)
                            {
                                rdbCategoryLevel.Enabled = false;
                                uploadControl.MaxFileSize = MaxFileSize;
                                hdnIsCategoryView.Value = "0";
                                hidEditForm.Value = "0";
                            }
                            //UAT-3090
                            var cmbExceptionItemss = pnlExceptionItems.FindControl("cmbExceptionItems") as RadComboBox;
                            if (cmbExceptionItemss.IsNullOrEmpty() || cmbExceptionItemss.Items.Count == AppConsts.NONE)
                            {
                                rdbItemLevel.Visible = false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Manage the edit/update/init insert/Save of the data entry form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tlistComplianceData_ItemCommand(object sender, TreeListCommandEventArgs e)
        {
            String docMessage = String.Empty;
            ViewDocumentDetailsContract docContract = new ViewDocumentDetailsContract();
            if(e.CommandName==RadTreeList.CancelCommandName)
            {
                hdnEnterRequirement.Value = "Cancel";
                IsCancelButtonCall = true;
                IsUpdateButtonCall = true;
            }
            if (e.CommandName == RadTreeList.DeleteCommandName)
            {
                TextBoxClientID = string.Empty;
                ApplicantComplianceItemId = Convert.ToInt32(e.CommandArgument); // issue

                //CHANGED
                /*CurrentViewContext.ComplianceCategoryId = ParseNodeId(Convert.ToString((e.Item as TreeListDataItem).ParentItem.GetDataKeyValue("NodeID")));
                CurrentViewContext.ItemId = ParseNodeId(Convert.ToString((e.Item as TreeListDataItem).GetDataKeyValue("NodeID")));*/

                var _complianceCategoryId = ParseNodeId(Convert.ToString((e.Item as TreeListDataItem).ParentItem.GetDataKeyValue("NodeID")));
                var _complianceItemId = ParseNodeId(Convert.ToString((e.Item as TreeListDataItem).GetDataKeyValue("NodeID")));

                if (hidEditForm.Value == "3") // 3 - Deletion of the Exception
                {
                    if (Convert.ToInt32(ApplicantComplianceItemId) > 0 && Presenter.IsItemStatusApproved(Convert.ToInt32(ApplicantComplianceItemId)))
                    {
                        base.ShowInfoMessage("This Compliance Item has already been approved. Approved Compliance Item cannot be deleted.");
                        _isValFailForDeletion = true;
                        return;
                    }
                    //ApplicantComplianceItemId = Convert.ToInt16(e.CommandArgument);
                    //var id = Convert.ToInt16(ApplicantComplianceItemId);
                    Presenter.RemoveExceptionByApplicantComplianceItem(ApplicantComplianceItemId, _complianceItemId, _complianceCategoryId);
                }
                else if (hidEditForm.Value == "2") // 2 - Deletion of the Item Data                    {
                {
                    if (Convert.ToInt32(ApplicantComplianceItemId) > 0 && Presenter.IsItemStatusApproved(Convert.ToInt32(ApplicantComplianceItemId)))
                    {
                        base.ShowInfoMessage("This Compliance Item has already been approved. Approved Compliance Item cannot be deleted.");
                        _isValFailForDeletion = true;
                        return;
                    }
                    #region UAT-1607
                    Int32 itemSeriesId = Presenter.GetItemSeriesIDForItem(_complianceItemId, _complianceCategoryId);
                    Label lblSeriesErrorMsg = e.Item.FindControl("lblSeriesErrorMsg") as Label;
                    #endregion
                    Presenter.DeleteApplicantItemAttributeData(ApplicantComplianceItemId, _complianceCategoryId, _complianceItemId);
                    if (itemSeriesId > AppConsts.NONE)
                    {
                        Presenter.SuffleShotSeriesItemData(itemSeriesId, _complianceCategoryId);
                        if (!CurrentViewContext.UIValidationErrors.IsNullOrEmpty())
                        {
                            if (!lblSeriesErrorMsg.IsNullOrEmpty())
                            {
                                lblSeriesErrorMsg.Text = CurrentViewContext.UIValidationErrors.HtmlEncode();
                            }
                            _catIDToShowUIValForSeriesItem = _complianceCategoryId;
                            //e.Canceled = true;
                        }
                    }
                    #region Changes related to short series 'usp_Rule_EvaluateAdjustItemSeriesRules' SP.
                    Presenter.EvaluateAdjustItemSeriesRules();
                    #endregion
                }
            }
            else if (e.CommandName == RadTreeList.InitInsertCommandName)
            {
                Session["ViewDocumentDetailsContract"] = null;
                CurrentViewContext.AddedViewDocId = AppConsts.NONE;
                // Command is fired when user clicks on 'Enter Requirements'. So current node id is the ComplianceCategoryId(Master)
                CurrentViewContext.ComplianceCategoryId = ParseNodeId(Convert.ToString((e.Item as TreeListDataItem).GetDataKeyValue("NodeID")));
                TextBoxClientID = CurrentViewContext.ComplianceCategoryId+"_"+Convert.ToString((e.Item as TreeListDataItem).GetParentDataKeyValue("ParentNodeID"));
                //if (hidEditForm.Value == "1") // Exception Mode
                //{
                //    ApplicantComplianceItemId = ConvertToInt32(e.CommandArgument);
                //}
                //else
                // {
                CurrentViewContext.ItemId = 0;
                //UAT-1264: WB: As a student, I should be able to enter the data for an item (or submit a new exception) when an item exception expires
                if (hidEditForm.Value == "4") // Exception Mode
                {
                    CurrentViewContext.ComplianceCategoryId = ParseNodeId(Convert.ToString((e.Item as TreeListDataItem).ParentItem.GetDataKeyValue("NodeID")));
                    CurrentViewContext.ItemId = ParseNodeId(Convert.ToString((e.Item as TreeListDataItem).GetDataKeyValue("NodeID")));
                }
                //Reset the compliance item id in case the user selectes 'add requirement', immediately after clicking 'update requirement'.
                Presenter.GetComplianceCategoryDetails();

                hdnCategoryName.Value = String.IsNullOrEmpty(CurrentViewContext.SelectedCategory.CategoryLabel) ? CurrentViewContext.SelectedCategory.CategoryName : CurrentViewContext.SelectedCategory.CategoryLabel;
                hdnPackageName.Value = String.IsNullOrEmpty(CurrentViewContext.PackageName) ? String.Empty : CurrentViewContext.PackageName;
                //}

                //UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                _isEnterRequirementClick = true;
                //UAT 3189 As a student, I should be able to apply for a category exception when all items in the category have been entered.   
                var editForm = (e.Item as TreeListDataItem);
                var lnkApplyForException = editForm.FindControl("lnkApplyForException") as LinkButton;
                if (lnkApplyForException.IsNotNull() && lnkApplyForException.Visible)
                {
                    hdnApplyForException.Value = "1";
                }
                else
                {
                    hdnApplyForException.Value = String.Empty;
                }

                //CurrentViewContext.IsItemEnterDataClick = true;
                CurrentViewContext.NodeID = ParseNodeId(Convert.ToString((e.Item as TreeListDataItem).GetDataKeyValue("NodeID")));
            }

            else if (e.CommandName == RadTreeList.PerformInsertCommandName || e.CommandName == RadTreeList.UpdateCommandName)
            {

                //Check isRestrictedFile = true.
                hdnIsAnyRestrictedFileUploaded.Value = false.ToString();
                CheckFileTypeRestrictions(e.Item as TreeListEditFormItem);
                Boolean IsAnyRestrictedFileUploaded = Convert.ToBoolean(hdnIsAnyRestrictedFileUploaded.Value);
                if (IsAnyRestrictedFileUploaded)
                {
                    lblError.Text = "Please remove the non-supported document(s).";
                    e.Canceled = true;
                    return;
                }

                //UAT-1811
                List<Int32> lstComplianceItemId = new List<Int32>();
                Int32 complianceCategoryId = 0;
                if (e.CommandName == RadTreeList.UpdateCommandName)
                {
                    IsUpdateButtonCall = true;
                }

                if (hidEditForm.Value == "1") // Exception Mode
                {
                    //Exception mode

                    var editForm = e.Item as TreeListEditFormItem;
                    var applicantComplianceItem = editForm.FindControl("hdnApplicantComplianceItemId") as HiddenField;
                    var complianceItem = editForm.FindControl("hdnComplianceItemId") as HiddenField;
                    // To check Status of item during applicant update his data. 

                    if (Convert.ToInt32(applicantComplianceItem.Value) > 0 && Presenter.IsItemStatusApproved(Convert.ToInt32(applicantComplianceItem.Value))
                        && !IsEnableUpdateAllTimeForItem(Convert.ToInt32(complianceItem.Value)) //UAT-4926
                        )
                    {
                        base.ShowInfoMessage("This Compliance Item has already been approved. Approved Compliance Item cannot be updated.");
                        return;
                    }

                    var cmbExceptionItems = editForm.FindControl("cmbExceptionItems") as WclComboBox;
                    var cmbUploadDocument = editForm.FindControl("cmbUploadDocument") as WclComboBox;
                    var exceptionReason = editForm.FindControl("txtExceptionComments") as WclTextBox;
                    var applicantComplianceCategoryId = 0;
                    // check for category level 
                    var hdnIsCategoryView = (HiddenField)editForm.FindControl("hdnIsCategoryView");
                    //Changes as per UAT-523 WB: Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
                    Boolean isCategory = false;
                    if (hdnIsCategoryView.Value != "0")
                    {
                        isCategory = true;
                    }
                    //Change 
                    //if (exceptionReason.Text.Trim().Length < 10 && !isCategory)
                    if (exceptionReason.Text.Trim().Length < 10)
                    {
                        var lblExceptionComments = (Label)editForm.FindControl("lblExceptionComments");
                        lblExceptionComments.Text = "Minimum 10 characters required for reason for exception.";
                        InitializeSignaturePad();
                        e.Canceled = true;

                        var panelException = (Panel)editForm.FindControl("pnlExceptionForm");
                        var panelpnlEntryForm = (Panel)editForm.FindControl("pnlEntryForm");
                        ShowHidePanel(panelException, panelpnlEntryForm);
                        return;
                    }

                    Int32 comItemId = ParseNodeId(Convert.ToString(editForm.ParentItem.GetDataKeyValue("NodeID")));
                    CurrentViewContext.CategoryDataContract = new ApplicantComplianceCategoryDataContract
                    {
                        PackageSubscriptionId = CurrentViewContext.PackageSubscriptionId,
                        ComplianceCategoryId = CurrentViewContext.ComplianceCategoryId,
                        ReviewStatusTypeCode = ApplicantCategoryComplianceStatus.Incomplete.GetStringValue()
                    };

                    var exceptionMapping = cmbUploadDocument.CheckedItems.Select(item => new ExceptionDocumentMapping
                    {
                        ApplicantDocumentID = Convert.ToInt32(item.Value),
                    }).ToList();


                    //uploading the Document to Application 
                    var uploadControl = (WclAsyncUpload)editForm.FindControl("uploadControl");

                    //#region UAT-3639
                    String[] dropzone = new String[] { "#" + this.DropzoneID };
                    uploadControl.DropZones = dropzone;
                    uploadControl.Localization.Select = "Hidden";
                    //#endRegion

                    //UAT-4067
                    String[] allowedFileExtensions = CurrentViewContext.allowedFileExtensions.IsNullOrEmpty() ? String.Empty.Split(',') : CurrentViewContext.allowedFileExtensions.ToArray();
                    uploadControl.AllowedFileExtensions = allowedFileExtensions;

                    docMessage = UploadAllDocuments(uploadControl);

                    if (applicantComplianceItem.Value == "0")
                    {
                        Int32 tenantId = CurrentViewContext.TenantID;
                        //Contract to add mail content for Exception Applied.
                        List<ExceptionRejectionContract> lstExceptionRejectionContract = new List<ExceptionRejectionContract>();
                        String instituteUrl = Presenter.GetInstitutionUrl(tenantId);
                        String instituteName = Presenter.GetInstitutionName(tenantId);

                        //Get CategoryName
                        var categoryName = hdnCategoryName.Value;

                        //Get NodeHiearchy
                        String nodeHiearchy = Presenter.GetNodeHiearchy(CurrentViewContext.PackageSubscriptionId);

                        if (hdnIsCategoryView.Value == "0")
                        {
                            var chkItemList = (from item in cmbExceptionItems.CheckedItems
                                               where item.Checked
                                               select new { ChkValue = Convert.ToInt32(item.Value), ChkText = Convert.ToString(item.Text) });

                            //foreach (var complianceItemId in from item in cmbExceptionItems.CheckedItems
                            //                                 where item.Checked
                            //                                 select Convert.ToInt32(item.Value))
                            foreach (var item in chkItemList)
                            {
                                //Presenter.SaveExceptionData(applicantComplianceCategoryId, exceptionReason.Text, complianceItemId, exceptionMapping);
                                Presenter.SaveExceptionData(applicantComplianceCategoryId, exceptionReason.Text, item.ChkValue, exceptionMapping, isCategory);
                                ExceptionRejectionContract excRejContract = new ExceptionRejectionContract();
                                excRejContract.ApplicantName = CurrentViewContext.ApplicantName;
                                excRejContract.ApplicationUrl = instituteUrl;
                                excRejContract.UserFullName = "Administrative User";
                                excRejContract.ComplianceItemName = item.ChkText;
                                excRejContract.PackageName = CurrentViewContext.PackageName;
                                excRejContract.InstituteName = instituteName;
                                excRejContract.NodeHierarchy = nodeHiearchy;
                                excRejContract.CategoryName = categoryName;
                                excRejContract.HierarchyNodeID = CurrentViewContext.HierarchyID.HasValue ? CurrentViewContext.HierarchyID.Value : 0;
                                lstExceptionRejectionContract.Add(excRejContract);

                                //UAT-1811
                                lstComplianceItemId.Add(item.ChkValue);
                            }
                        }
                        else
                        {
                            //Saving for category level, for items only which can be edited by the applicant
                            var hdnComplianceCategoryId = editForm.FindControl("hdnComplianceCategoryId") as HiddenField;
                            CurrentViewContext.ComplianceCategoryId = Convert.ToInt32(hdnComplianceCategoryId.Value);

                            //New Code UAT-523 WB: Change "Category Exception" behavior to be an overall approval of the category, not just all Items filled in.
                            //isCategory = true;
                            Guid wholeCatGUID = new Guid(AppConsts.WHOLE_CATEGORY_GUID);
                            Presenter.GetWholeCategoryItemID(wholeCatGUID);
                            var availableItems = CurrentViewContext.lstAvailableItems;
                            foreach (var availableItem in availableItems)
                            {
                                Presenter.SaveExceptionData(applicantComplianceCategoryId, exceptionReason.Text,
                                    Convert.ToInt32(availableItem.ComplianceItemID), exceptionMapping, isCategory);
                                ExceptionRejectionContract excRejContract = new ExceptionRejectionContract();
                                excRejContract.ApplicantName = CurrentViewContext.ApplicantName;
                                excRejContract.ApplicationUrl = instituteUrl;
                                excRejContract.UserFullName = "Administrative User";
                                excRejContract.ComplianceItemName = availableItem.ItemLabel;
                                excRejContract.PackageName = CurrentViewContext.PackageName;
                                excRejContract.InstituteName = instituteName;
                                excRejContract.NodeHierarchy = nodeHiearchy;
                                excRejContract.CategoryName = categoryName;
                                excRejContract.HierarchyNodeID = CurrentViewContext.HierarchyID.HasValue ? CurrentViewContext.HierarchyID.Value : 0;
                                lstExceptionRejectionContract.Add(excRejContract);

                                //UAT-1811
                                lstComplianceItemId.Add(availableItem.ComplianceItemID);
                            }

                            //Old Code
                            //CurrentViewContext.ComplianceCategoryId = Convert.ToInt32(hdnComplianceCategoryId.Value);
                            //Presenter.GetClientComplianceItems();
                            //var availableItems = CurrentViewContext.lstAvailableItems;

                            //foreach (var availableItem in availableItems)
                            //{
                            //    Presenter.SaveExceptionData(applicantComplianceCategoryId, exceptionReason.Text,
                            //        Convert.ToInt32(availableItem.ComplianceItemID), exceptionMapping);
                            //    ExceptionRejectionContract excRejContract = new ExceptionRejectionContract();
                            //    excRejContract.ApplicantName = CurrentViewContext.ApplicantName;
                            //    excRejContract.ApplicationUrl = instituteUrl;
                            //    excRejContract.UserFullName = "Administrative User";
                            //    excRejContract.ComplianceItemName = availableItem.ItemLabel;
                            //    excRejContract.PackageName = CurrentViewContext.PackageName;
                            //    excRejContract.InstituteName = instituteName;
                            //    excRejContract.NodeHierarchy = nodeHiearchy;
                            //    excRejContract.CategoryName = categoryName;
                            //    excRejContract.HierarchyNodeID = CurrentViewContext.HierarchyID.HasValue ? CurrentViewContext.HierarchyID.Value : 0;

                            //    lstExceptionRejectionContract.Add(excRejContract);
                            //}
                        }
                        if (lstExceptionRejectionContract.IsNotNull() && lstExceptionRejectionContract.Count > 0)
                        {
                            Presenter.CallParallelTaskForMail(lstExceptionRejectionContract, isCategory);
                        }
                        //hdnCategoryName.Value = "";
                    }
                    else
                    {
                        var applcntcomplianceItemId = Convert.ToInt32(applicantComplianceItem.Value);
                        var complianceItemId = Convert.ToInt32(complianceItem.Value);
                        Presenter.UpdateExceptionData(applcntcomplianceItemId, exceptionReason.Text, exceptionMapping, complianceItemId);

                        //UAT-1811
                        lstComplianceItemId.Add(complianceItemId);
                    }
                    //UAT-4330
                    lblExceptionMsg.Text = "<p style='color:#14892c'>Please note - you have applied for an exception for this requirement. ALL exceptions are reviewed directly by your school administrators. The Complio team at American DataBank is unable to review exceptions, and therefore cannot provide a turnaround time for review.</p>";
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ExceptionAlertPopUp();", true);

                    #region Changes related to short series 'usp_Rule_EvaluateAdjustItemSeriesRules' SP.
                    Presenter.EvaluateAdjustItemSeriesRules();
                    #endregion
                    //mapping the Docuemnt 

                    //UAT-1811
                    complianceCategoryId = CurrentViewContext.ComplianceCategoryId;
                    tlistComplianceData.Rebind();

                    //UAT-285 on submitting the infomation corresponding to Item,catogry should be expanded.  
                    //tlistComplianceData.ExpandAllItems();

                    /*UAT-2722 */
                    var expandedIndexes = new TreeListHierarchyIndex[tlistComplianceData.ExpandedIndexes.Count];
                    tlistComplianceData.ExpandedIndexes.AddRange(expandedIndexes);

                    for (int i = 0; i < tlistComplianceData.Items.Count; i++)
                    {
                        for (int j = 0; j < tlistComplianceData.Items[i].ChildItems.Count; j++)
                        {
                            var a = ParseNodeId(((INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail)(((Telerik.Web.UI.TreeListEditableItem)(tlistComplianceData.Items[i].ChildItems[j])).DataItem)).NodeID);
                            if (a == comItemId)
                            {
                                tlistComplianceData.Items[i].ChildItems[j].Expanded = true;
                                break;
                            }
                        }
                    }
                    /*UAT-2722 end here*/


                    //_presenter.CallParallelTaskPdfConversionMerging();

                    //if (docMessage.IsNullOrEmpty())
                    //{
                    //    base.ShowSuccessMessage("Compliance exception data saved successfully.");
                    //}
                }
                else
                {
                    try
                    {
                        Boolean isItemSeries = false;
                        List<Int32> expItemList = new List<Int32>();
                        if (ViewState["expItemList"] != null)
                        {
                            expItemList = (List<Int32>)ViewState["expItemList"];
                        }

                        List<Int32> expiringPendingItemLst = new List<Int32>();
                        if (ViewState["expiringPendingItemLst"] != null)
                        {
                            expiringPendingItemLst = (List<Int32>)ViewState["expiringPendingItemLst"];
                        }
                        Boolean _isUIValidation;
                        String _uiValidation =
                            Convert.ToString(ConfigurationManager.AppSettings["IsUIValidationApplicable"]);
                        if (!String.IsNullOrEmpty(_uiValidation))
                        {
                            Boolean.TryParse(_uiValidation, out _isUIValidation);
                            CurrentViewContext.IsUIValidationApplicable = _isUIValidation;
                        }

                        var temps = CurrentViewContext.SelectedCategory;

                        TreeListEditFormItem editForm = e.Item as TreeListEditFormItem;
                        Control ucItemForm = editForm.FindControl("itemForm") as System.Web.UI.Control;
                        WclComboBox ddItems = editForm.FindControl("cmbRequirement") as WclComboBox;
                        String ParentNodeID = Convert.ToString(editForm.ParentItem.GetParentDataKeyValue("ParentNodeID"));
                        Int32 compItemId = Convert.ToInt32(ParseNodeId(Convert.ToString(editForm.ParentItem.GetDataKeyValue("NodeID"))));
                        if (!ddItems.IsNullOrEmpty() && Convert.ToInt32(ddItems.SelectedValue) > 0)
                            {
                            TextBoxClientID = ddItems.Text + "_" +ddItems.SelectedValue+"_"+ compItemId;
                            IsUpdateButtonCall = true;
                            }
                        Panel pnlForm = ucItemForm.FindControl("pnlForm") as Panel;

                        Panel pnlControls = ucItemForm.FindControl("pnl") as Panel;

                        HiddenField hdfApplicantItemDataId =
                           pnlForm.FindControl("hdfApplicantItemDataId") as HiddenField;

                        Int32 applicantComplianceItemId = String.IsNullOrEmpty(hdfApplicantItemDataId.Value)
                                 ? AppConsts.NONE
                                 : Convert.ToInt32(hdfApplicantItemDataId.Value);

                        //UAT 4300 If "Complete Document" is only attribute for an item, item should auto-submit from student screen when document is completed
                        HiddenField hdnItemCompliance = editForm.FindControl("hdnItemComplianceStatus") as HiddenField;
                        hdnItemComplianceReviewStatus.Value = hdnItemCompliance.Value;
                        // To check Status of item during applicant update his data.
                        //Int32 comItemId = Convert.ToInt32(ParseNodeId(Convert.ToString(editForm.ParentItem.GetDataKeyValue("NodeID"))));

                        //UAT-2143,Research Request | Complio | FIU | Student Attestation
                        Int32 comItemId = GetComplianceItemID(editForm, ddItems, ref isItemSeries);
                   

                        #region UAT-1607:Student Data Entry Screen changes
                        //Check item or series
                        SetItemSeriesProperty(ddItems);
                        #endregion

                        Boolean IsPaymentType = false;
                        if (!IsItemSeriesDataToSave) // This bit indicates that the current data is not item series data
                        {
                            IsPaymentType = Convert.ToBoolean(Presenter.ItemDetails(comItemId).IsPaymentType);
                        }

                        if (applicantComplianceItemId > 0 && Presenter.IsItemStatusApproved(Convert.ToInt32(applicantComplianceItemId))
                            && (!expItemList.Contains(comItemId)) && !IsPaymentType
                            && !IsEnableUpdateAllTimeForItem(comItemId) // UAT-4926
                            )
                        {
                            base.ShowInfoMessage("This Compliance Item has already been approved. Approved Compliance Item cannot be updated.");
                            return;
                        }



                        #region SET THE CATEGORY DATA TO SAVE-UPDATE

                        CurrentViewContext.CategoryDataContract = new ApplicantComplianceCategoryDataContract();
                        CurrentViewContext.CategoryDataContract.PackageSubscriptionId =
                            CurrentViewContext.PackageSubscriptionId;
                        CurrentViewContext.CategoryDataContract.ComplianceCategoryId =
                            CurrentViewContext.ComplianceCategoryId;
                        CurrentViewContext.CategoryDataContract.ReviewStatusTypeCode =
                            ApplicantCategoryComplianceStatus.Incomplete.GetStringValue();

                        #endregion

                        #region SET THE ITEM DATA TO SAVE-UPDATE

                        CurrentViewContext.ItemDataContract = new ApplicantComplianceItemDataContract();
                        //HiddenField hdfApplicantItemDataId =
                        //    pnlForm.FindControl("hdfApplicantItemDataId") as HiddenField;
                        CurrentViewContext.ItemDataContract.ApplicantComplianceItemId =
                            String.IsNullOrEmpty(hdfApplicantItemDataId.Value)
                                ? AppConsts.NONE
                                : Convert.ToInt32(hdfApplicantItemDataId.Value);
                        ;

                        // This dropdown value will be available in case of InsertItemTemplate. 
                        // In case InertItemTemplate the 'else' condition returns the ComplianceCategoryId
                        // In case EditItemTemplate the 'else' condition returns the ComplianceItemId
                        //UAT-1607 
                        //Boolean isItemSeries = false;
                        //if (!ddItems.SelectedValue.IsNullOrEmpty() && ddItems.SelectedValue != AppConsts.ZERO)
                        //{
                        //    isItemSeries = Convert.ToBoolean(ddItems.SelectedItem.Attributes["IsItemSeries"]);
                        //    CurrentViewContext.ItemDataContract.ComplianceItemId =
                        //        CurrentViewContext.ItemId = !isItemSeries ? Convert.ToInt32(ddItems.SelectedValue) : GetItemIDFromDropDown(ddItems.SelectedValue);

                        //}
                        //else
                        //{
                        //    CurrentViewContext.ItemDataContract.ComplianceItemId =
                        //       CurrentViewContext.ItemId = Convert.ToInt32(ParseNodeId(Convert.ToString(editForm.ParentItem.GetDataKeyValue("NodeID"))));
                        //}

                        //UAT-2143,Research Request | Complio | FIU | Student Attestation
                        CurrentViewContext.ItemDataContract.ComplianceItemId = CurrentViewContext.ItemId = GetComplianceItemID(editForm, ddItems, ref isItemSeries);

                        CurrentViewContext.IsItemSeriesSelected = isItemSeries;
                        CurrentViewContext.ItemDataContract.ReviewStatusTypeCode =
                            ApplicantItemComplianceStatus.Pending_Review.GetStringValue();
                        CurrentViewContext.ItemDataContract.Notes =
                            (pnlForm.FindControl("txtItemNotes") as WclTextBox).Text;

                        #endregion

                        #region  SET THE ATTRIBUTE DATA TO SAVE-UPDATE

                        lstAttributesData = new List<ApplicantComplianceAttributeDataContract>();

                        Boolean IfAnydateUpdatedForExpiredItem = false;
                        Boolean ifAnyManualDateAttrisPresent = false;//UAT-3095
                        //Boolean IsDcoumentType = false;
                        WclComboBox cmbDocuments = null;
                        WclAsyncUpload uploaderItemDocuments = (ucItemForm.FindControl("fupItemData") as WclAsyncUpload);
                        if (uploaderItemDocuments.IsNotNull() && uploaderItemDocuments.Visible)
                        {
                            docMessage = UploadAllDocuments(uploaderItemDocuments);
                            Int32 _itemId = CurrentViewContext.ItemId;

                            if (CurrentViewContext.SavedApplicantDocuments.IsNotNull())
                            {
                                if (CurrentViewContext.AttributeDocuments == null)
                                {
                                    CurrentViewContext.AttributeDocuments = new Dictionary<Int32, Int32>();
                                }

                                foreach (var documentId in CurrentViewContext.SavedApplicantDocuments)
                                {
                                    if (!CurrentViewContext.AttributeDocuments.ContainsKey(documentId.Key))
                                    {
                                        CurrentViewContext.AttributeDocuments.Add(Convert.ToInt32(documentId.Key), AppConsts.NONE);
                                    }
                                }
                            }
                        }

                        var isAttributeControlToSave = false;
                        var isAttributeDataChanged = false;

                        foreach (Control rowControl in pnlControls.Controls)
                        {
                            #region GET THE ROW LEVEL USER CONTROL

                            if (rowControl.GetType().BaseType == typeof(RowControl))
                            {
                                foreach (var attributeControl in rowControl.Controls)
                                {
                                    if (attributeControl.GetType().BaseType ==
                                        typeof(System.Web.UI.HtmlControls.HtmlContainerControl))
                                    {
                                        foreach (
                                            var ctrl in
                                                (attributeControl as System.Web.UI.HtmlControls.HtmlContainerControl)
                                                    .Controls)
                                        {
                                            #region GET THE ATTRIBUTE LEVEL USER CONTROL

                                            if (ctrl.GetType().BaseType == typeof(AttributeControl))
                                            {
                                                Panel attrPanel =
                                                    ((ctrl as AttributeControl).FindControl("pnlControls") as Panel);
                                                if (attrPanel.IsNotNull())
                                                {
                                                    DateTime? previousEnteredDate = null;
                                                    foreach (var ctrlType in attrPanel.Controls)
                                                    {
                                                        #region GET THE ACTUAL VALUES BASED ON THE CONTROL TYPE

                                                        Type baseControlType = ctrlType.GetType();
                                                        String attributeValue = String.Empty;

                                                        if (baseControlType == typeof(HiddenField) && (ctrlType as HiddenField).ID.Contains("hdnAlreadyEnteredDate_"))
                                                        {
                                                            previousEnteredDate = (ctrlType as HiddenField).Value != String.Empty ? Convert.ToDateTime((ctrlType as HiddenField).Value) : (DateTime?)null;
                                                        }

                                                        if (baseControlType == typeof(WclTextBox))
                                                        {
                                                            isAttributeControlToSave = true;
                                                            attributeValue = (ctrlType as WclTextBox).Text;
                                                            isAttributeDataChanged |= AddDataToList(ctrl, attributeValue,
                                                                          ComplianceAttributeDatatypes.Text
                                                                                                      .GetStringValue());
                                                        }
                                                        else if (baseControlType == typeof(WclNumericTextBox))
                                                        {
                                                            isAttributeControlToSave = true;
                                                            attributeValue = (ctrlType as WclNumericTextBox).Text;
                                                            isAttributeDataChanged |= AddDataToList(ctrl, attributeValue,
                                                                          ComplianceAttributeDatatypes.Numeric
                                                                                                      .GetStringValue());
                                                        }
                                                        else if (baseControlType == typeof(WclComboBox))
                                                        {
                                                            isAttributeControlToSave = true;
                                                            if (!(ctrlType as WclComboBox).CheckBoxes)
                                                            {
                                                                attributeValue = (ctrlType as WclComboBox).SelectedValue;
                                                                isAttributeDataChanged |= AddDataToList(ctrl, attributeValue,
                                                                              ComplianceAttributeDatatypes.Options
                                                                                                          .GetStringValue());
                                                            }
                                                            else
                                                            {
                                                                if (CurrentViewContext.AttributeDocuments == null)
                                                                {
                                                                    CurrentViewContext.AttributeDocuments = new Dictionary<Int32, Int32>();
                                                                }

                                                                cmbDocuments = (ctrlType as WclComboBox);
                                                                foreach (var checkedDocument in (ctrlType as WclComboBox).CheckedItems)
                                                                {
                                                                    if (!CurrentViewContext.AttributeDocuments.ContainsKey(Convert.ToInt32(checkedDocument.Value)))
                                                                    {
                                                                        CurrentViewContext.AttributeDocuments.Add(
                                                                        Convert.ToInt32(checkedDocument.Value),
                                                                        (ctrl as AttributeControl).ClientItemAttributes
                                                                                                  .ComplianceItem
                                                                                                  .ComplianceItemID);
                                                                    }

                                                                }
                                                                #region UAT-1864 : As an applicant, I should be able to preview documents in the document selection dropdown on the submit item screen
                                                                List<RadComboBoxItem> cmb = cmbDocuments.CheckedItems.ToList();
                                                                ShowHideDocumentPreview(cmb, (ucItemForm.FindControl("dvDocumentPreview") as HtmlGenericControl), (ucItemForm.FindControl("pnlDocumentPreview") as Panel));
                                                                #endregion
                                                                isAttributeDataChanged |= AddDataToList(ctrl, Convert.ToString(CurrentViewContext.AttributeDocuments.Count()), ComplianceAttributeDatatypes.Numeric.GetStringValue(), true);
                                                            }
                                                        }
                                                        else if (baseControlType == typeof(WclDatePicker))
                                                        {
                                                            isAttributeControlToSave = true;
                                                            if ((ctrlType as WclDatePicker).SelectedDate == null)
                                                            {
                                                                attributeValue = String.Empty;
                                                            }
                                                            else
                                                            {
                                                                attributeValue = Convert.ToDateTime((ctrlType as WclDatePicker).SelectedDate).ToShortDateString();
                                                            }

                                                            DateTime? newEntredDate = (ctrlType as WclDatePicker).SelectedDate;
                                                            if (!previousEnteredDate.IsNullOrEmpty() && !newEntredDate.IsNullOrEmpty())
                                                            {
                                                                if (newEntredDate > previousEnteredDate)
                                                                {
                                                                    IfAnydateUpdatedForExpiredItem = true;
                                                                }
                                                            }
                                                            if (previousEnteredDate.IsNullOrEmpty() && !newEntredDate.IsNullOrEmpty())
                                                            {
                                                                IfAnydateUpdatedForExpiredItem = true;
                                                            }
                                                            //UAT-3095
                                                            if ((ctrlType as WclDatePicker).Enabled)
                                                            {
                                                                ifAnyManualDateAttrisPresent = true;
                                                            }

                                                            isAttributeDataChanged |= AddDataToList(ctrl, attributeValue,
                                                                          ComplianceAttributeDatatypes.Date
                                                                                                      .GetStringValue());
                                                        }
                                                        else if (baseControlType == typeof(LinkButton))
                                                        {
                                                            if ((ctrlType as LinkButton).Attributes["DataTypeCode"] == ComplianceAttributeDatatypes.View_Document.GetStringValue())
                                                            {
                                                                docContract = Session["ViewDocumentDetailsContract"] as ViewDocumentDetailsContract;
                                                                if (!docContract.IsNullOrEmpty() && !docContract.DocumentPath.IsNullOrEmpty())
                                                                {
                                                                    Int32 docID = 0;
                                                                    if (docContract.IsNotNull())
                                                                    {
                                                                        docID = docContract.AddedViewDocId; //CurrentViewContext.AddedViewDocId;
                                                                    }
                                                                    //else
                                                                    //{
                                                                    //    docID = UploadViewDocument(docContract);
                                                                    //}
                                                                    if (docID > 0)
                                                                    {
                                                                        //UAT - 4913
                                                                        isAttributeDataChanged |= AddDataToList(ctrl, AppConsts.ONE.ToString(),
                                                                          ComplianceAttributeDatatypes.View_Document.GetStringValue());
                                                                        if (CurrentViewContext.ViewAttributeDocuments == null)
                                                                        {
                                                                            CurrentViewContext.ViewAttributeDocuments = new Dictionary<Int32, Int32>();
                                                                        }

                                                                        CurrentViewContext.ViewAttributeDocuments.Add(
                                                                       docID,
                                                                       (ctrl as AttributeControl).ClientItemAttributes
                                                                                                 .ComplianceItem
                                                                                                 .ComplianceItemID);
                                                                        if (CurrentViewContext.IsFileUploadExists)
                                                                        {
                                                                            if (CurrentViewContext.AttributeDocuments == null)
                                                                            {
                                                                                CurrentViewContext.AttributeDocuments = new Dictionary<Int32, Int32>();
                                                                            }
                                                                            if (!CurrentViewContext.AttributeDocuments.ContainsKey(docID))
                                                                            {
                                                                                CurrentViewContext.AttributeDocuments.Add(docID, (ctrl as AttributeControl).ClientItemAttributes.ComplianceItem.ComplianceItemID);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else if (e.CommandName == RadTreeList.UpdateCommandName)
                                                                {
                                                                    if ((ctrlType as LinkButton).Attributes["AppDocID"] == "0")
                                                                    {
                                                                        AddDataToList(ctrl, AppConsts.ZERO,
                                                                          ComplianceAttributeDatatypes.View_Document.GetStringValue());
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    AddDataToList(ctrl, AppConsts.ZERO,
                                                                          ComplianceAttributeDatatypes.View_Document.GetStringValue());
                                                                }
                                                            }
                                                            //UAT-1738:Create new attribute type for data-synced documents and update data sync procedure
                                                            else if (String.Compare((ctrlType as LinkButton).Attributes["DataTypeCode"],
                                                                                     ComplianceAttributeDatatypes.Screening_Document.GetStringValue(), true) == AppConsts.NONE)
                                                            {
                                                                if (((ctrlType as LinkButton).Attributes["AttributeValue"]).IsNullOrEmpty())
                                                                {
                                                                    AddDataToList(ctrl, AppConsts.ZERO,
                                                                         ComplianceAttributeDatatypes.Screening_Document.GetStringValue());
                                                                }
                                                            }
                                                        }
                                                        else if (baseControlType == typeof(System.Web.UI.HtmlControls.HtmlGenericControl))
                                                        {
                                                            string fieldDataType = (ctrlType as System.Web.UI.HtmlControls.HtmlGenericControl).Attributes["FieldType"];
                                                            Int32 itemFieldId = Convert.ToInt32((ctrlType as System.Web.UI.HtmlControls.HtmlGenericControl).Attributes["ItemFieldId"]);

                                                            HtmlInputHidden hdnOutput = attrPanel.Controls[0].FindControl(string.Concat("hiddenOutput_", itemFieldId.ToString())) as System.Web.UI.HtmlControls.HtmlInputHidden;
                                                            HtmlGenericControl canvas = attrPanel.Controls[0].FindControl("signature") as System.Web.UI.HtmlControls.HtmlGenericControl;

                                                            if (fieldDataType == ComplianceAttributeDatatypes.Signature.GetStringValue().ToLower())
                                                            {
                                                                byte[] signature = null;
                                                                attributeValue = "False";

                                                                if (hdnOutput.IsNotNull() && !hdnOutput.Value.IsNullOrEmpty())
                                                                {
                                                                    attributeValue = "True";
                                                                    signature = GetSignatureImageBuffer(hdnOutput.Value);
                                                                }
                                                                else if (!canvas.Visible)
                                                                {
                                                                    attributeValue = "True";
                                                                }

                                                                AddDataToList(ctrl, attributeValue, ComplianceAttributeDatatypes.Signature.GetStringValue(), false, signature);
                                                            }
                                                        }

                                                        #endregion
                                                    }
                                                }
                                            }

                                            #endregion
                                        }
                                    }
                                }
                            }

                            #endregion
                        }

                        #endregion

                        //variable to set the ResetBusinessProcess of handle assignment method.                        
                        HiddenField hdnItemComplianceStatus = (HiddenField)editForm.FindControl("hdnItemComplianceStatus");
                        Boolean isItemExpired = hdnItemComplianceStatus.Value == ApplicantItemComplianceStatus.Expired.ToString();
                        if (isItemExpired && IfAnydateUpdatedForExpiredItem == false && ifAnyManualDateAttrisPresent)
                        {
                            base.ShowInfoMessage("You cannot re-submit an expired item without changing Date. Please update Date to reflect the date on your documentation.");
                            InitializeSignaturePad();
                            e.Canceled = true;
                            return;
                        }
                        //UAT-1682 related changes.
                        else if ((expItemList.Contains(comItemId) || expiringPendingItemLst.Contains(comItemId))
                            && IfAnydateUpdatedForExpiredItem == false && ifAnyManualDateAttrisPresent)
                        {
                            base.ShowInfoMessage("You cannot re-submit an expiring item without changing Date. Please update Date to reflect the date on your documentation.");
                            InitializeSignaturePad();
                            e.Canceled = true;
                            return;
                        }

                        //UAT-3888
                        if (!string.IsNullOrWhiteSpace(hdnItemComplianceStatus.Value)
                            && hdnItemComplianceStatus.Value != ApplicantItemComplianceStatus.Incomplete.ToString()
                            && isAttributeControlToSave
                            && !isAttributeDataChanged)
                        {
                            base.ShowInfoMessage("You cannot submit the requirement without updating any field. Please update any field to reflect changes on your documentation.");
                            InitializeSignaturePad();
                            e.Canceled = true;
                            return;
                        }

                        if (IsItemSeriesDataToSave)
                        {
                            //UAT-1811
                            var shotSeriesItemIds = Presenter.SaveShotSeriesAttributeData();
                            lstComplianceItemId.AddRange(shotSeriesItemIds);
                        }
                        else
                        {
                            //Int32 ApplicantComplianceItemId = AppConsts.NONE;
                            //INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_PARKING_CART) as INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract;
                            //if (!itemPaymentContract.IsNullOrEmpty())
                            //{
                            //    if (itemPaymentContract.ItemID == CurrentViewContext.ItemDataContract.ComplianceItemId)
                            //    {
                            //        if (itemPaymentContract.ItemDataId > AppConsts.NONE)
                            //        {
                            //            ApplicantComplianceItemId = itemPaymentContract.ItemDataId;
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    ApplicantComplianceItemId = CurrentViewContext.CurrentViewContext.ItemDataContract.ApplicantComplianceItemId;
                            //}
                            var res = Presenter.CheckItemPayment(applicantComplianceItemId, CurrentViewContext.ItemDataContract.ComplianceItemId);
                            if (res.Item1)
                            {
                                Presenter.SaveApplicantComplianceAttributeData(isItemExpired);

                                // UAT-3598	As an applicant, I should see a popup alerting me if i have submitted something that expires on submission.
                                ApplicantComplianceItemData applicantComplianceItemData = ComplianceDataManager.GetApplicantData(CurrentViewContext.PackageId, CurrentViewContext.ComplianceCategoryId, CurrentViewContext.ItemDataContract.ComplianceItemId, CurrentViewContext.OrganiztionUserID, CurrentViewContext.TenantID);
                                if (applicantComplianceItemData.IsNotNull())
                                {
                                    List<lkpItemComplianceStatu> itemComplianceStatus = ComplianceDataManager.GetItemComplianceStatus(CurrentViewContext.TenantID);
                                    var compItemStatus = itemComplianceStatus.Where(cond => cond.ItemComplianceStatusID == applicantComplianceItemData.StatusID).First();
                                    if (String.IsNullOrEmpty(CurrentViewContext.UIValidationErrors) && compItemStatus.Code == ApplicantItemComplianceStatus.Expired.GetStringValue())
                                    {
                                        lblExpiryItemMsg.Text = "<p style='color:#14892c'>ATTENTION - You've submitted an already <b>Expired</b> item, which may mean your information is not up to date. Please check the date to make sure it is accurate. Otherwise, you may need to update your requirement with more up to date information.</p>";
                                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ItemExpiredAlertPopUp();", true);
                                    }
                                }

                                //UAT-1811
                                lstComplianceItemId.Add(CurrentViewContext.ItemId);
                            }
                            else
                            {
                                base.ShowErrorInfoMessage("Payment is required for this item.");
                                e.Canceled = true;
                                InitializeSignaturePad();
                                return;
                            }
                        }
                        #region Changes related to short series 'usp_Rule_EvaluateAdjustItemSeriesRules' SP.
                        Presenter.EvaluateAdjustItemSeriesRules();
                        #endregion
                        //if (IsDcoumentType)
                        //{
                        //    //_presenter.CallParallelTaskPdfConversionMerging();
                        //    if (!String.IsNullOrEmpty(ErrorMessage))
                        //    {
                        //        base.LogDebug(ErrorMessage);
                        //    }
                        //}
                        if (docContract.IsNotNull())
                        {
                            if (!cmbDocuments.IsNullOrEmpty())
                            {
                                RadComboBoxItem addedItemFromClientSide = cmbDocuments.FindItemByValue(Convert.ToString(docContract.AddedViewDocId.ToString()));
                                if (!addedItemFromClientSide.IsNullOrEmpty())
                                {
                                    addedItemFromClientSide.Enabled = false;
                                    addedItemFromClientSide.Checked = true;
                                }
                                else
                                {
                                    // Add the newly selecetd documents to the list, if the UI validation blocks the save
                                    if (cmbDocuments.IsNotNull() && CurrentViewContext.SavedApplicantDocuments.IsNotNull())
                                    {
                                        foreach (var document in CurrentViewContext.SavedApplicantDocuments)
                                        {
                                            cmbDocuments.Items.Add(new RadComboBoxItem { Text = document.Value, Value = Convert.ToString(document.Key), Checked = true });
                                            if (document.Key == docContract.AddedViewDocId)//do not check View doc type item
                                            {
                                                RadComboBoxItem addedItem = cmbDocuments.FindItemByValue(Convert.ToString(document.Key));
                                                addedItem.Enabled = false;
                                            }
                                        }
                                    }
                                    //if UI rule validator is fired again=>find view doc type document and disable it
                                    else if (cmbDocuments.IsNotNull() && CurrentViewContext.SavedApplicantDocuments.IsNullOrEmpty() && docContract.AddedViewDocId > 0)
                                    {
                                        RadComboBoxItem addedItem = cmbDocuments.FindItemByValue(Convert.ToString(docContract.AddedViewDocId.ToString()));
                                        if (!addedItem.IsNullOrEmpty())
                                        {
                                            addedItem.Enabled = false;
                                            addedItem.Checked = true;
                                        }
                                    }
                                }
                            }
                        }
                        if (String.IsNullOrEmpty(CurrentViewContext.UIValidationErrors))
                        {
                            base.ShowSuccessMessage("Compliance data saved successfully.");

                            //UAT-1811
                            complianceCategoryId = CurrentViewContext.ComplianceCategoryId;

                            #region CLEAR THE RELATED ID's

                            CurrentViewContext.ComplianceCategoryId = 0;
                            CurrentViewContext.ItemId = 0;
                            ViewState[AppConsts.APPLICANT_COMPLIANCE_CATEGORY_DATA_ID_VIEW_STATE] = String.Empty;

                            #endregion

                            tlistComplianceData.Rebind();
                            //UAT-285 on submitting the infomation corresponding to Item,catogry should be expanded. 
                            //tlistComplianceData.ExpandAllItems();

                            /*UAT-2722 */
                            var expandedIndexes = new TreeListHierarchyIndex[tlistComplianceData.ExpandedIndexes.Count];
                            tlistComplianceData.ExpandedIndexes.AddRange(expandedIndexes);

                            for (int i = 0; i < tlistComplianceData.Items.Count; i++)
                            {
                                for (int j = 0; j < tlistComplianceData.Items[i].ChildItems.Count; j++)
                                {
                                    //((INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail)(((Telerik.Web.UI.TreeListEditableItem)((new System.Collections.Generic.Mscorlib_CollectionDebugView<Telerik.Web.UI.TreeListDataItem>(tlistComplianceData.Items)).Items[0])).DataItem)).NodeID
                                    var a = ParseNodeId(((INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail)(((Telerik.Web.UI.TreeListEditableItem)(tlistComplianceData.Items[i].ChildItems[j])).DataItem)).NodeID);
                                    //ParseNodeId(((INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation.RotationRequirementUIContract)((tlistComplianceData.Items[i].ChildItems[j].DataItem))).NodeID);
                                    if (a == compItemId)
                                    {
                                        tlistComplianceData.Items[i].ChildItems[j].Expanded = true;
                                        break;
                                    }
                                }
                            }
                            /*UAT-2722 end here*/

                        }
                        else
                        {
                            uploaderItemDocuments.PostbackTriggers = null;
                            Label lblError = pnlForm.FindControl("lblError") as Label;
                            lblError.Text = UIValidationErrors;
                            InitializeSignaturePad();
                            e.Canceled = true;
                            return;
                        }
                    }
                    catch (SysXException ex)
                    {
                        base.LogError(ex);
                        if (IsItemSeriesDataToSave)
                        {
                            base.ShowInfoMessage("A system error has occurred, please try again. If the problem persists, please contact American Databank.");
                            return;
                        }
                        else
                        {
                            base.ShowInfoMessage("Compliance data could not be saved.");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        base.LogError(ex);
                        if (IsItemSeriesDataToSave)
                        {
                            base.ShowInfoMessage("A system error has occurred, please try again. If the problem persists, please contact American Databank.");
                            return;
                        }
                        else
                        {
                            base.ShowInfoMessage("Compliance data could not be saved.");
                            return;
                        }
                    }
                }

                //UAT-1811: Display a popup following item submission by applicant if category is not made pending review or approved.
                //String catName = hdnCategoryName.Value;
                List<String> lstItemName = new List<String>();
                if (complianceCategoryId == 0)
                {
                    complianceCategoryId = CurrentViewContext.ComplianceCategoryId;
                }

                var categoryDetails = ComplianceDetailList.Where(x => x.Category.ComplianceCategoryID == complianceCategoryId);
                foreach (var category in categoryDetails.Where(x => !x.Item.IsNullOrEmpty() && !lstComplianceItemId.IsNullOrEmpty()))
                {
                    if (lstComplianceItemId.Contains(category.Item.ComplianceItemID))
                    {
                        //lstItemName.Add(category.Item.Name);
                        if(!IsUpdateButtonCall)
                        {
                            TextBoxClientID = String.IsNullOrEmpty(category.Item.ItemLabel) ? category.Item.Name : category.Item.ItemLabel + "_" + category.NodeID+"_"+ category.Category.ComplianceCategoryID ;
                            IsUpdateButtonCall = true;
                        }
                        lstItemName.Add(String.IsNullOrEmpty(category.Item.ItemLabel) ? category.Item.Name : category.Item.ItemLabel);
                        lstComplianceItemId.Remove(category.Item.ComplianceItemID);
                    }
                }
                var categoryDetail = categoryDetails.FirstOrDefault(x => x.IsParent == true);
                //String catName = categoryDetail.Name;
                Boolean showPopup = !(categoryDetail.ReviewStatus.Equals(VerificationDataActions.PENDING_REVIEW.GetStringValue())
                                      || categoryDetail.ReviewStatus.Equals(VerificationDataActions.PENDING_REVIEW_EXCEPTION.GetStringValue())
                                      || categoryDetail.ReviewStatus.Equals(VerificationDataActions.APPROVED.GetStringValue())
                                      || categoryDetail.ReviewStatus.Equals(VerificationDataActions.APPROVED_EXCEPTION.GetStringValue())
                                      || categoryDetail.ReviewStatus.Equals(VerificationDataActions.CATEGORY_EXCEPTIONALLY_APPROVED.GetStringValue()));
                if (showPopup)
                {
                    //String catExplanatoryNote = Presenter.GetLargeContent(complianceCategoryId);
                    Presenter.GetComplianceCategoryDetails(complianceCategoryId);
                    String catName = (String.IsNullOrEmpty(CurrentViewContext.SelectedCategory.CategoryLabel) ? CurrentViewContext.SelectedCategory.CategoryName : CurrentViewContext.SelectedCategory.CategoryLabel);
                    String catExplanatoryNote = CurrentViewContext.SelectedCategory.ExpNotes;
                    String itemNames = string.Join(", ", lstItemName);
                    String msg = "Thank you for submitting " + itemNames + "!" + " Submission of additional items is required per the rules of your institution." +
                                 " Please see the requirements for " + catName + " Below: <br/> <br/>" + catExplanatoryNote +
                                 "<br/> <br/>" + "Please note that if multiple requirements information is on a document, the data for each requirement must be entered separately."; //UAT-3248
                    // base.ShowInfoMessage(msg);
                    lblMsg.Text = msg;
                    lblMsg.ForeColor = System.Drawing.Color.Navy;
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "nextStepPopUp();", true);
                    return;
                }

            }

            else if (e.CommandName == RadTreeList.EditCommandName)
            {
                Session["ViewDocumentDetailsContract"] = null;
                CurrentViewContext.AddedViewDocId = AppConsts.NONE;
                // Command is fired when user clicks on 'Update Requirements'. So parent of the current node is the ComplianceCategoryId(Master)
                CurrentViewContext.ComplianceCategoryId = ParseNodeId(Convert.ToString((e.Item as TreeListDataItem).ParentItem.GetDataKeyValue("NodeID")));

                // Command is fired when user clicks on 'Update Requirements'. So id of the current node is the ItemId(Master)
                CurrentViewContext.ItemId = ParseNodeId(Convert.ToString((e.Item as TreeListDataItem).GetDataKeyValue("NodeID")));

                string ParentNodeId = Convert.ToString((e.Item as TreeListDataItem).GetParentDataKeyValue("ParentNodeID"));

                var temp = e.Item as TreeListDataItem;
                TextBox txtEditTime = temp.FindControl("txtEditTime") as TextBox;
                if (txtEditTime != null && TextBoxClientID.IsNullOrEmpty())
                {
                    TextBoxClientID = Convert.ToString( CurrentViewContext.ItemId)+"_"+ ParentNodeId;
                    IsUpdateButtonCall = true;
                }

                //if (Presenter.IsItemStatusApproved(CurrentViewContext.ItemId)) { }

                //ItemPaymentContract itemPayment = Presenter.GetItemPaymentDetail(Subscription.PackageSubscriptionID).Where(cond => cond.ItemID == CurrentViewContext.ItemId).FirstOrDefault();

                //if (itemPayment.IsNotNull() && !itemPayment.IsNullOrEmpty())
                //{
                //    hdfInvoiceNumber.Value = Convert.ToString(itemPayment.invoiceNumber);
                //    hdfOrgUserProfileID.Value = Convert.ToString(itemPayment.OrganizationUserProfileID);
                //}


                if (hidEditForm.Value == "1") // Exception Mode
                {
                    ApplicantComplianceItemId = Convert.ToInt32(e.CommandArgument);
                }
            }
            else if (e.CommandName == "ExpandAll")
            {
                tlistComplianceData.ExpandAllItems();
            }
            else if (e.CommandName == "CollapseAll")
            {
                tlistComplianceData.CollapseAllItems();
            }
            else if (e.CommandName == "Cancel")
            {
                tlistComplianceData.ExpandAllItems();
            }

            InitializeSignaturePad();
        }

        protected void tlistComplianceData_ItemDataBound(object sender, TreeListItemDataBoundEventArgs e)
        {
            if (e.Item is TreeListEditableItem && (e.Item as TreeListEditableItem).IsInEditMode)
            {
                TreeListEditableItem editableItem = (TreeListEditableItem)e.Item;
                ItemForm itemForm = editableItem.FindControl("itemForm") as ItemForm;
                Control cmdBar = editableItem.FindControl("cmdBar") as Control;
                WclButton btnCancel = editableItem.FindControl("btnCancelExpNotes") as WclButton;
                TextBox txtEditTime = editableItem.FindControl("txtEditTime") as TextBox;
                INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail compDetail = editableItem.DataItem as INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail;
                LinkButton LinkButton = editableItem.FindControl("lnkApplyForException") as LinkButton;
                WclComboBox ddItems = editableItem.FindControl("cmbRequirement") as WclComboBox;
                //UAT 4300 If "Complete Document" is only attribute for an item, item should auto-submit from student screen when document is completed
                HiddenField hdnItemCompliance = editableItem.FindControl("hdnItemComplianceStatus") as HiddenField;
                if (hdnItemCompliance != null)
                {
                    hdnItemComplianceReviewStatus.Value = hdnItemCompliance.Value;
                }

                ManageFileUploadDisplay(itemForm as ItemForm, null, editableItem);
                if (!compDetail.IsNullOrEmpty() && !compDetail.Item.IsNullOrEmpty() && compDetail.Item.ComplianceItemID > AppConsts.NONE)
                {
                    var ItemDetails = Presenter.ItemDetails(compDetail.Item.ComplianceItemID);
                    if (!ItemDetails.IsNullOrEmpty() && !cmdBar.IsNullOrEmpty())
                    {
                        var complianceAttribiute = ItemDetails.ComplianceItemAttributes.Where(cond => !cond.CIA_IsDeleted && cond.ComplianceAttribute.IsActive).ToList();
                        if (complianceAttribiute.IsNullOrEmpty() || complianceAttribiute.Count == AppConsts.NONE)
                        {
                            ShowCommandBar(cmdBar, false);
                            btnCancel.Visible = true;
                            btnCancel.ToolTip = "Click to close";
                            btnCancel.Text = "Close";
                        }
                        else
                        {
                            ShowCommandBar(cmdBar, true);
                            btnCancel.Visible = false;
                        }
                    }
                }

                //UAT-4067
                if (!itemForm.IsNullOrEmpty())
                {
                    itemForm.IsAllowedFileExtensionEnable = true;
                    itemForm.AllowedExtensions = CurrentViewContext.allowedFileExtensions.IsNullOrEmpty() ? String.Empty : String.Join(",", CurrentViewContext.allowedFileExtensions);
                }
            }

         
            //UAT-846: UAT-977: Additional work towards archive ability
            if (e.Item is TreeListDataItem)
            {
                #region UAT:4665
                TreeListDataItem DataItem = (TreeListDataItem)e.Item;
                INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail compDetail = (INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail)DataItem.DataItem;

                int parentId = 0;
                if (compDetail != null && !compDetail.ParentNodeID.IsNullOrEmpty() && compDetail.ParentNodeID.Split('_').Length > 1)
                {
                    parentId = Convert.ToInt32(compDetail.ParentNodeID.Split('_')[1]);

                }
                if (IsCancelButtonCall && TextBoxClientID == compDetail.CategoryId + "_" + compDetail.ParentNodeID)
                {
                    TextBox txtEditTime = DataItem.FindControl("TxtCategory") as TextBox;
                    txtEditTime.Focus();

                }
                if (parentId > AppConsts.NONE && compDetail.Name + "_" + compDetail.NodeID + "_" + parentId == TextBoxClientID && IsUpdateButtonCall)
                {
                    TextBox txtEditTime = DataItem.FindControl("txtEditTime") as TextBox;
                    txtEditTime.Focus();
                }
                if (compDetail.NodeID + "_" + compDetail.ParentNodeID == TextBoxClientID && IsUpdateButtonCall)
                {
                    TextBox txtEditTime = DataItem.FindControl("txtEditTime") as TextBox;
                    txtEditTime.Focus();
                } 
                #endregion



                //UAT-1607
                //TreeListDataItem DataItem = (TreeListDataItem)e.Item;
                //INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail compDetail = (INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail)DataItem.DataItem;
                if (e.Item.FindControl("imgStatus").IsNotNull() && (IsExpiredSubscription || IsArchivedSubscription || IsGraduated || ArchivedGraduated))
                {
                    e.Item.FindControl("imgStatus").Visible = false;

                }
                
                #region UAT-1607
                if (!compDetail.IsNullOrEmpty() && compDetail.IsParent && compDetail.Category.ComplianceCategoryID == _catIDToShowUIValForSeriesItem)
                {
                    Label lblSeriesErrorMsg = e.Item.FindControl("lblSeriesErrorMsg") as Label;

                    if (!CurrentViewContext.UIValidationErrors.IsNullOrEmpty())
                    {
                        if (!lblSeriesErrorMsg.IsNullOrEmpty())
                        {
                            lblSeriesErrorMsg.Text = CurrentViewContext.UIValidationErrors.HtmlEncode();
                        }
                        CurrentViewContext.UIValidationErrors = String.Empty;
                    }
                }
                #endregion
            }

            //if (e.Item is TreeListDataItem)
            //{
            //    TreeListDataItem dataItem = e.Item as TreeListDataItem;
            //    HtmlGenericControl dvAddRequirement = dataItem.FindControl("Div2") as HtmlGenericControl ;
            //    dvAddRequirement.Visible= true;
            //}

            //UAT 2978
            if (e.Item is TreeListHeaderItem)
            {
                TreeListHeaderItem headeritem = e.Item as TreeListHeaderItem;
                /*UAT-3026 Updates to Applicant Requirements PDF*/
                //  LinkButton btnDownloadTrackingRequirementReport = headeritem.FindControl("btnDownloadTrackingRequirementReport") as LinkButton;

                WclButton btnDownloadTrackingRequirementReport = headeritem.FindControl("btnDownloadTrackingRequirementReport") as WclButton;
                /*UAT-3026 End here*/
                btnDownloadTrackingRequirementReport.Visible = IsPackageActive;
            }
        }

        #endregion

        #region Radio button Events

        /// <summary>
        /// Event of 'Items'  combobox listing for which the applicant can enter the data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbCategoryLevel_Click(object sender, EventArgs e)
        {
            var editForm = (sender as WclButton).NamingContainer as TreeListEditFormInsertItem;
            var pnlExceptionItems = (Panel)editForm.FindControl("pnlExceptionItems");
            var panelException = (Panel)editForm.FindControl("pnlExceptionForm");
            var panelpnlEntryForm = (Panel)editForm.FindControl("pnlEntryForm");
            var hdnIsCategoryView = (HiddenField)editForm.FindControl("hdnIsCategoryView");
            hdnIsCategoryView.Value = "1";
            //var pnlSupportingDocs = (HtmlGenericControl)editForm.FindControl("pnlSupportingDocs");
            //#region UAT-3639
            var uploadControl = (WclAsyncUpload)editForm.FindControl("uploadControl");
            String[] dropzone = new String[] { "#" + this.DropzoneID };
            uploadControl.DropZones = dropzone;
            uploadControl.Localization.Select = "Hidden";
            //endregion
            ShowHidePanel(panelException, panelpnlEntryForm);
            pnlExceptionItems.Visible = false;

            //UAT-1864 : As an applicant, I should be able to preview documents in the document selection dropdown on the submit item screen
            var cmbUploadDocument = (WclComboBox)editForm.FindControl("cmbUploadDocument");
            ShowHideDocumentPreview(cmbUploadDocument.CheckedItems.ToList(), (editForm.FindControl("dvDocumentPreview") as HtmlGenericControl), (editForm.FindControl("pnlDocumentPreview") as Panel));
            //pnlSupportingDocs.Visible = false;

            //UAT-4067
            String[] allowedFileExtensions = CurrentViewContext.allowedFileExtensions.IsNullOrEmpty() ? String.Empty.Split(',') : CurrentViewContext.allowedFileExtensions.ToArray();
            uploadControl.AllowedFileExtensions = allowedFileExtensions;
        }

        protected void rdbItemLevel_Click(object sender, EventArgs e)
        {
            var editForm = (sender as WclButton).NamingContainer as TreeListEditFormInsertItem;
            var pnlExceptionItems = (Panel)editForm.FindControl("pnlExceptionItems");
            var panelException = (Panel)editForm.FindControl("pnlExceptionForm");
            var panelpnlEntryForm = (Panel)editForm.FindControl("pnlEntryForm");
            //var pnlSupportingDocs = (HtmlGenericControl)editForm.FindControl("pnlSupportingDocs");
            var uploadControl = (WclAsyncUpload)editForm.FindControl("uploadControl");
            //#region UAT-3639
            String[] dropzone = new String[] { "#" + this.DropzoneID };
            uploadControl.DropZones = dropzone;
            uploadControl.Localization.Select = "Hidden";
            //#endRegion
            ShowHidePanel(panelException, panelpnlEntryForm);
            var hdnIsCategoryView = (HiddenField)editForm.FindControl("hdnIsCategoryView");
            hdnIsCategoryView.Value = "0";
            pnlExceptionItems.Visible = true;
            //pnlSupportingDocs.Visible = true;
            uploadControl.MaxFileSize = MaxFileSize;
            //UAT-1264: WB: As a student, I should be able to enter the data for an item (or submit a new exception) when an item exception expires
            var cmbExceptionItems = pnlExceptionItems.FindControl("cmbExceptionItems") as RadComboBox;
            var cmbRequirement = editForm.FindControl("cmbRequirement") as RadComboBox;
            if (CurrentViewContext.ItemId > AppConsts.NONE && !IsItemSeriesSelected)
            {
                var selectedItem = cmbExceptionItems.FindItemByValue(CurrentViewContext.ItemId.ToString());
                if (!selectedItem.IsNullOrEmpty())
                {
                    selectedItem.Checked = true;
                    cmbExceptionItems.Enabled = false;
                }
                else
                {
                    CurrentViewContext.IsItemEnterDataClick = true;
                }
            }
            //UAT-1864 : As an applicant, I should be able to preview documents in the document selection dropdown on the submit item screen
            var cmbUploadDocument = (WclComboBox)editForm.FindControl("cmbUploadDocument");
            ShowHideDocumentPreview(cmbUploadDocument.CheckedItems.ToList(), (editForm.FindControl("dvDocumentPreview") as HtmlGenericControl), (editForm.FindControl("pnlDocumentPreview") as Panel));

            //UAT-4067
            String[] allowedFileExtensions = CurrentViewContext.allowedFileExtensions.IsNullOrEmpty() ? String.Empty.Split(',') : CurrentViewContext.allowedFileExtensions.ToArray();
            uploadControl.AllowedFileExtensions = allowedFileExtensions;
        }

        #endregion

        #region Combobox Events

        private void cmbRequirement_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Session["ViewDocumentDetailsContract"] = null;
            CurrentViewContext.AddedViewDocId = AppConsts.NONE;
            //Clear the controls when 'SELECT' option is selected
            TreeListEditFormInsertItem insertItem = (sender as WclComboBox).NamingContainer as TreeListEditFormInsertItem;
            Control ucItemForm = insertItem.FindControl("itemForm") as System.Web.UI.Control;
            Panel pnlForm = ucItemForm.FindControl("pnlForm") as Panel;
            Panel pnlItemInfo = insertItem.FindControl("pnlItemInfo") as Panel;

            Label lblError = pnlForm.FindControl("lblError") as Label;
            lblError.Text = String.Empty;
            Control cmdBar = insertItem.FindControl("cmdBar") as Control;
            //UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
            WclButton btnCancelExpNotes = insertItem.FindControl("btnCancelExpNotes") as WclButton;

            HtmlGenericControl spnItemInfo = insertItem.FindControl("spnItemInfo") as HtmlGenericControl;
            HtmlGenericControl spnItemInfoData = insertItem.FindControl("spnItemInfoData") as HtmlGenericControl;
            (sender as WclComboBox).Focus();
            if ((sender as WclComboBox).SelectedValue == AppConsts.ZERO)
            {
                insertItem.FindControl("itemForm").Controls.Clear();
                ShowCommandBar(cmdBar, false);
                pnlItemInfo.Visible = false;
                //UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                if (btnCancelExpNotes.IsNotNull())
                {
                    btnCancelExpNotes.Visible = true;
                }

            }
            else
            {
                pnlItemInfo.Visible = true;
                Label lblItemName = insertItem.FindControl("lblItemName") as Label;
                lblItemName.Text = (sender as WclComboBox).SelectedItem.Text;
                spnItemInfo.Visible = true;
                spnItemInfoData.Visible = false;
                // Added Custom ToolTip to the Item
                HtmlGenericControl ItemToolTip = insertItem.FindControl("ItemToolTipCustom") as HtmlGenericControl;
                //UAT 401 -Add a field for Items to configure "Details" during student Order Process
                // ItemToolTip.InnerHtml = Presenter.GetItemDescription(Convert.ToInt32((sender as WclComboBox).SelectedValue));  
                #region UAT-1607:
                Boolean isItemSeries = Convert.ToBoolean((sender as WclComboBox).SelectedItem.Attributes["IsItemSeries"]);
                #endregion
                CurrentViewContext.IsItemSeries = isItemSeries;
                if (isItemSeries)
                {
                    //ItemToolTip.InnerHtml = Presenter.GetItemSeriesDetail(Convert.ToInt32((sender as WclComboBox).SelectedValue));
                    ItemToolTip.InnerHtml = Presenter.GetItemSeriesDetail(GetItemIDFromDropDown((sender as WclComboBox).SelectedValue));
                }
                else
                {
                    ItemToolTip.InnerHtml = Presenter.GetItemDetails(Convert.ToInt32((sender as WclComboBox).SelectedValue));
                }

                ShowCommandBar(cmdBar, true);
                (cmdBar as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to save and submit the entered data for this requirement";
                (cmdBar as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
                //UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                if (btnCancelExpNotes.IsNotNull())
                {
                    btnCancelExpNotes.Visible = false;
                }


            }

            //UAT-4067
            if (!ucItemForm.IsNullOrEmpty())
            {
                ItemForm ucForm = ucItemForm as ItemForm;
                if (!ucForm.IsNullOrEmpty())
                {
                    ucForm.IsAllowedFileExtensionEnable = true;
                    ucForm.AllowedExtensions = CurrentViewContext.allowedFileExtensions.IsNullOrEmpty() ? String.Empty : String.Join(",", CurrentViewContext.allowedFileExtensions);
                }
            }

            ManageFileUploadDisplay(ucItemForm as ItemForm, insertItem, null);

            #region UAT-3077
            //TODO: Check the item is payment type and has no attribute
            Int32 SelectedItemId;

            bool result = Int32.TryParse((sender as WclComboBox).SelectedValue, out SelectedItemId);
            if (result)
            {
                // HtmlGenericControl divExplanatoryNoteItems = insertItem.FindControl("dvExplanatoryNotesItem") as HtmlGenericControl;
                var ItemDetails = Presenter.ItemDetails(SelectedItemId);
                if (!ItemDetails.IsNullOrEmpty())
                {
                    var complianceAttribiute = ItemDetails.ComplianceItemAttributes.Where(cond => !cond.CIA_IsDeleted && cond.ComplianceAttribute.IsActive).ToList();
                    if (complianceAttribiute.IsNullOrEmpty() || complianceAttribiute.Count == AppConsts.NONE)
                    {
                        // divExplanatoryNoteItems.Visible = false;
                        ShowCommandBar(cmdBar, false);
                        btnCancelExpNotes.Visible = true;
                        btnCancelExpNotes.ToolTip = "Click to close";
                        btnCancelExpNotes.Text = "Close";
                    }
                    //UAT UAT-3622 "Complete the below fields for 'X item'" should only be displayed on student data entry screen if there is a view/complere document attribute on the item
                    if (!complianceAttribiute.IsNullOrEmpty() && complianceAttribiute.Count > AppConsts.NONE && complianceAttribiute.Any(x => x.ComplianceAttribute.lkpComplianceAttributeDatatype.Code == ComplianceAttributeDatatypes.View_Document.GetStringValue()))
                    {
                        spnItemInfo.Visible = false;
                        spnItemInfoData.Visible = true;
                        Label lblDocViewItemName = insertItem.FindControl("lblDocViewItemName") as Label;
                        lblDocViewItemName.Text = (sender as WclComboBox).SelectedItem.Text;
                    }
                }
                //UAT 4300 If "Complete Document" is only attribute for an item, item should auto-submit from student screen when document is completed
                CurrentViewContext.SelectedItemDetails = ItemDetails;

                if (!CurrentViewContext.SelectedItemDetails.IsNullOrEmpty())
                {
                    if (pnlForm.FindControl("txtItemNotes").IsNotNull())
                    {
                        hdnItemNotes.Value = (pnlForm.FindControl("txtItemNotes") as WclTextBox).Text;
                    }

                    List<ComplianceAttribute> lstComplianceAttributes = new List<ComplianceAttribute>();
                    String viewDocTypeCode = ComplianceAttributeDatatypes.View_Document.GetStringValue();
                    if (CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.IsNotNull())
                    {
                        lstComplianceAttributes = CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.Where(x => x.CIA_IsDeleted == false && x.CIA_IsActive == true).Select(sel => sel.ComplianceAttribute).ToList();
                    }

                    if (CurrentViewContext.SelectedItemDetails.IsPaymentType == false
                        && CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.IsNotNull()
                         && lstComplianceAttributes.Where(con => !con.IsDeleted && con.IsActive).Count() == AppConsts.ONE
                    && lstComplianceAttributes.Where(con => !con.IsDeleted && con.IsActive).FirstOrDefault().lkpComplianceAttributeDatatype.Code == viewDocTypeCode)
                    {
                        hdnfIsAutoSubmitTriggerForItem.Value = "true";
                    }
                    else
                    {
                        hdnfIsAutoSubmitTriggerForItem.Value = "false";
                    }
                }
            }
            #endregion

            InitializeSignaturePad();
        }

        #endregion

        #region Button Events

        protected void btnAddException_Click(object sender, EventArgs e)
        {

            TextBoxClientID = string.Empty;
            hdnEnterRequirement.Value = "ApplyException";
            IsApplyForExceptionCall = true;

            TreeListEditFormInsertItem insertItem = (sender as LinkButton).NamingContainer as TreeListEditFormInsertItem;
            Panel pnlExceptionForm = insertItem.FindControl("pnlExceptionForm") as Panel;
            Panel pnlEntryForm = insertItem.FindControl("pnlEntryForm") as Panel;
            // IEnumerable<ExceptionDocumentMapping> applicationMappingData = null;
            var cmbUploadDocument = pnlExceptionForm.FindControl("cmbUploadDocument") as WclComboBox;
            //var applicantComplianceItem = pnlExceptionForm.FindControl("hdnApplicantComplianceItemId") as HiddenField;
            pnlEntryForm.Visible = false;
            pnlExceptionForm.Visible = true;
            var uploadedDocument = Presenter.GetApplicantUploadedDocuments();
            if (ApplicantComplianceItemId == 0)
            {
                foreach (var exceptionDocuments in uploadedDocument)
                {
                    //cmbUploadDocument.Items.Add(new RadComboBoxItem
                    //{
                    //    Text = exceptionDocuments.FileName,
                    //    Value = exceptionDocuments.ApplicantDocumentID.ToString()
                    //});
                    // UAT-210 Adding description to each uploaded file and commented the above code.
                    RadComboBoxItem expitem = new RadComboBoxItem { Text = exceptionDocuments.FileName, Value = Convert.ToString(exceptionDocuments.ApplicantDocumentID) };
                    expitem.Attributes["desc"] = exceptionDocuments.Description;
                    cmbUploadDocument.Items.Add(expitem);
                }
            }
            //UAT-210
            //Attaching client side event handler for combobox upload event
            //This handler should be decalard in global scope of the script
            cmbUploadDocument.OnClientLoad = "efn_atrFileUpdOnLoad";

            if (Subscription != null)
            {
                if (Subscription.ApplicantComplianceCategoryDatas != null)
                {
                    var rdbCategoryLevel = (WclButton)insertItem.FindControl("rdbCategoryLevel");
                    var rdbItemLevel = (WclButton)insertItem.FindControl("rdbItemLevel");
                    var pnlExceptionItems = (Panel)insertItem.FindControl("pnlExceptionItems");
                    //var pnlSupportingDocs = (HtmlGenericControl)insertItem.FindControl("pnlSupportingDocs");
                    var hdnIsCategoryView = (HiddenField)insertItem.FindControl("hdnIsCategoryView");
                    var uploadControl = (WclAsyncUpload)insertItem.FindControl("uploadControl");

                    //#region UAT-3639
                    String[] dropzone = new String[] { "#" + this.DropzoneID };
                    uploadControl.DropZones = dropzone;
                    uploadControl.Localization.Select = "Hidden";
                    //#endRegion

                    //#region UAT-4067
                    String[] allowedFileExtensions = CurrentViewContext.allowedFileExtensions.IsNullOrEmpty() ? String.Empty.Split(',') : CurrentViewContext.allowedFileExtensions.ToArray();
                    uploadControl.AllowedFileExtensions = allowedFileExtensions;
                    //#endRegion


                    String exceptionExpiredCode = lkpCategoryExceptionStatus.EXCEPTION_EXPIRED.GetStringValue();

                    ApplicantComplianceCategoryData appCmpCatData = Subscription.ApplicantComplianceCategoryDatas.FirstOrDefault(x => x.ComplianceCategoryID == CurrentViewContext.ComplianceCategoryId && !x.IsDeleted);
                    if (appCmpCatData.IsNotNull() && appCmpCatData.lkpCategoryExceptionStatu.IsNotNull() && appCmpCatData.lkpCategoryExceptionStatu.CES_Code != exceptionExpiredCode)
                    {
                        rdbCategoryLevel.Enabled = false;
                        rdbItemLevel.Checked = true;
                        pnlExceptionItems.Visible = true;
                        //pnlSupportingDocs.Visible = true;
                        uploadControl.MaxFileSize = MaxFileSize;
                        hdnIsCategoryView.Value = "0";

                    }
                    if (CurrentViewContext.ItemId > AppConsts.NONE && !IsItemSeriesSelected)
                    {
                        rdbCategoryLevel.Enabled = false;
                        rdbCategoryLevel.Checked = false;
                        rdbItemLevel.Checked = true;
                        rdbItemLevel.Enabled = false;
                        rdbItemLevel_Click(rdbItemLevel, null);
                    }
                    //UAT-3090
                    var cmbExceptionItems = pnlExceptionItems.FindControl("cmbExceptionItems") as RadComboBox;
                    if (cmbExceptionItems.IsNullOrEmpty() || cmbExceptionItems.Items.Count == AppConsts.NONE)
                    {
                        rdbItemLevel.Visible = false;
                    }

                    if ((cmbExceptionItems.IsNullOrEmpty() || cmbExceptionItems.Items.Count == AppConsts.NONE) && CurrentViewContext.IsItemEnterDataClick)
                    {
                        if (!appCmpCatData.lkpCategoryExceptionStatu.IsNullOrEmpty()
                            && (appCmpCatData.lkpCategoryExceptionStatu.CES_Code == "AAAA"
                            || appCmpCatData.lkpCategoryExceptionStatu.CES_Code == "AAAB"
                            || appCmpCatData.lkpCategoryExceptionStatu.CES_Code == "AAAD"
                            || appCmpCatData.lkpCategoryExceptionStatu.CES_Code == "AAAE"
                            || appCmpCatData.lkpCategoryExceptionStatu.CES_Code == "AAAF"))
                        {
                            rdbCategoryLevel.Enabled = false;
                            rdbCategoryLevel.Checked = false;
                            rdbItemLevel_Click(rdbItemLevel, null);
                        }
                        else
                        {
                            rdbCategoryLevel.Enabled = true;
                            rdbCategoryLevel.Checked = true;
                            rdbCategoryLevel_Click(rdbCategoryLevel, null);

                        }
                    }

                    if ((!cmbExceptionItems.IsNullOrEmpty() && cmbExceptionItems.Items.Count > AppConsts.NONE) && CurrentViewContext.IsItemEnterDataClick)
                    {
                        if (CurrentViewContext.IsItemExpired == AppConsts.ONE && appCmpCatData.lkpCategoryExceptionStatu.IsNotNull() &&
                            (appCmpCatData.lkpCategoryExceptionStatu.CES_Code == "AAAA"
                            || appCmpCatData.lkpCategoryExceptionStatu.CES_Code == "AAAB"
                            || appCmpCatData.lkpCategoryExceptionStatu.CES_Code == "AAAD"
                            || appCmpCatData.lkpCategoryExceptionStatu.CES_Code == "AAAE"
                            || appCmpCatData.lkpCategoryExceptionStatu.CES_Code == "AAAF"))
                        {
                            rdbCategoryLevel.Enabled = false;
                            rdbCategoryLevel.Checked = false;
                            rdbItemLevel.Enabled = true;
                            rdbItemLevel.Checked = true;
                            rdbItemLevel_Click(rdbItemLevel, null);
                        }
                        else
                        {
                            rdbCategoryLevel.Enabled = true;
                            rdbCategoryLevel.Checked = true;
                            rdbItemLevel.Enabled = true;
                            rdbItemLevel.Checked = false;
                            rdbCategoryLevel_Click(rdbCategoryLevel, null);
                        }
                    }
                }
            }
        }

        #region UAT-1607: Student Data Entry Screen changes

        private void cmbRequirement_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            try
            {
                ComplianceItem compItemObject = (ComplianceItem)e.Item.DataItem;
                if (!compItemObject.IsNullOrEmpty())
                {
                    e.Item.Attributes["IsItemSeries"] = Convert.ToString(compItemObject.IsItemSeries);
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

        #region UAT-2143: Research Request | Complio | FIU | Student Attestation

        protected void btnDownloadTrackingRequirementReport_Click(object sender, EventArgs e)
        {
            /*UAT-3026*/
            //var editForm = (sender as LinkButton).NamingContainer as TreeListHeaderItem;
            var editForm = (sender as WclButton).NamingContainer as TreeListHeaderItem;
            /*UAT-3026 Ends here*/

            String reportName = "TrackingPackageRequirementsReport";
            String format = "pdf";
            ParameterValue[] parameters = new ParameterValue[3];

            parameters[0] = new ParameterValue();
            parameters[0].Name = "TenantID";
            parameters[0].Value = CurrentViewContext.TenantID.ToString();

            parameters[1] = new ParameterValue();
            parameters[1].Name = "OrganizationUserID";
            parameters[1].Value = IsApplicant ? CurrentViewContext.CurrentLoggedInUserId.ToString() : CurrentViewContext.ApplicantID.ToString();

            parameters[2] = new ParameterValue();
            parameters[2].Name = "PackageSubscriptionID";
            parameters[2].Value = CurrentViewContext.PackageSubscriptionId.ToString();

            byte[] reportContent = ReportManager.GetReportByteArrayFormat(reportName, parameters, format);

            String fileName = "RequirementExplanation_" + Guid.NewGuid() + ".pdf";
            FileStream _FileStream = null;

            try
            {
                String tempFilePath = ConfigurationManager.AppSettings["TemporaryFileLocation"];
                if (tempFilePath.IsNullOrEmpty())
                {
                    //base.LogError("Please provide path for TemporaryFileLocation in config.", new SystemException());
                    throw new SystemException("Please provide path for TemporaryFileLocation in config.");
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

                String newTempFilePath = Path.Combine(tempFilePath, fileName);
                _FileStream = new FileStream(newTempFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                _FileStream.Write(reportContent, 0, reportContent.Length);
                _FileStream.Close();
                HtmlIframe iframe = editForm.FindControl("iframe") as HtmlIframe;
                iframe.Src = "~/ComplianceOperations/UserControl/DoccumentDownload.aspx?FilePath=" + newTempFilePath + "&FileName=" + fileName + "&IsFileDownloadFromFilePath=" + "True&IsTempFileLoaction=True";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try { _FileStream.Close(); }
                catch (Exception) { }
            }

        }

        #endregion

        #region UAT-3077

        protected void btnRefeshPage_Click(object sender, EventArgs e)
        {
            ItemPaymentContract itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_PARKING_CART) as ItemPaymentContract;
            if (itemPaymentContract.IsNullOrEmpty())
            {
                Response.Redirect(AppConsts.DASHBOARD_URL, true);
            }
            else
            {
                if (itemPaymentContract.IsRequirementPackage)
                {
                    String menuId = "10";
                    Response.Redirect(AppConsts.DASHBOARD_URL + "?MenuId=" + menuId + "&ReqPkgSubscriptionId=" + itemPaymentContract.PkgSubscriptionId + "&ClinicalRotationId=" + itemPaymentContract.ClinicalRotationID, true);
                }
                Response.Redirect(AppConsts.DASHBOARD_URL + "?ItemPaymentPkgSubscriptionId=" + itemPaymentContract.PkgSubscriptionId, true);
            }
        }

        #endregion

        protected void btnAutoSubmit_Click(object sender, EventArgs e)
        {

            List<Int32> lstComplianceItemId = new List<Int32>();
            Int32 complianceCategoryId = 0;

            try
            {
                List<ComplianceAttribute> lstComplianceAttributes = new List<ComplianceAttribute>();
                String viewDocTypeCode = ComplianceAttributeDatatypes.View_Document.GetStringValue();

                if (CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.IsNotNull())
                {
                    lstComplianceAttributes = CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.Where(x => x.CIA_IsDeleted == false && x.CIA_IsActive == true).Select(sel => sel.ComplianceAttribute).ToList();
                }

                if (CurrentViewContext.SelectedItemDetails.IsPaymentType == false && CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.IsNotNull()
                    && lstComplianceAttributes.Where(con => !con.IsDeleted && con.IsActive).Count() == AppConsts.ONE
                   && lstComplianceAttributes.Where(con => !con.IsDeleted && con.IsActive).FirstOrDefault().lkpComplianceAttributeDatatype.Code == viewDocTypeCode)
                {

                    Boolean isItemSeries = false;
                    List<Int32> expItemList = new List<Int32>();
                    if (ViewState["expItemList"] != null)
                    {
                        expItemList = (List<Int32>)ViewState["expItemList"];
                    }

                    List<Int32> expiringPendingItemLst = new List<Int32>();
                    if (ViewState["expiringPendingItemLst"] != null)
                    {
                        expiringPendingItemLst = (List<Int32>)ViewState["expiringPendingItemLst"];
                    }

                    Boolean _isUIValidation;
                    String _uiValidation =
                        Convert.ToString(ConfigurationManager.AppSettings["IsUIValidationApplicable"]);
                    if (!String.IsNullOrEmpty(_uiValidation))
                    {
                        Boolean.TryParse(_uiValidation, out _isUIValidation);
                        CurrentViewContext.IsUIValidationApplicable = _isUIValidation;
                    }

                    Int32 applicantComplianceItemId = String.IsNullOrEmpty(hdfAppItemDataId.Value)
                             ? AppConsts.NONE
                             : Convert.ToInt32(hdfAppItemDataId.Value);

                    //UAT-2143,Research Request | Complio | FIU | Student Attestation
                    Int32 comItemId = CurrentViewContext.SelectedItemDetails.ComplianceItemID;
                    Int32 compItemId = CurrentViewContext.SelectedItemDetails.ComplianceItemID;

                    #region UAT-1607:Student Data Entry Screen changes
                    //Check item or series
                    #endregion
                    Boolean IsPaymentType = false;
                    IsItemSeriesDataToSave = CurrentViewContext.SelectedItemDetails.IsItemSeries;
                    IsPaymentType = CurrentViewContext.SelectedItemDetails.IsPaymentType.HasValue ? CurrentViewContext.SelectedItemDetails.IsPaymentType.Value : false;

                    if (applicantComplianceItemId > 0 && Presenter.IsItemStatusApproved(Convert.ToInt32(applicantComplianceItemId))
                        && (!expItemList.Contains(comItemId)) && !IsPaymentType)
                    {
                        return;
                    }

                    #region SET THE CATEGORY DATA TO SAVE-UPDATE

                    CurrentViewContext.CategoryDataContract = new ApplicantComplianceCategoryDataContract();
                    CurrentViewContext.CategoryDataContract.PackageSubscriptionId =
                        CurrentViewContext.PackageSubscriptionId;
                    CurrentViewContext.CategoryDataContract.ComplianceCategoryId =
                        CurrentViewContext.ComplianceCategoryId;
                    CurrentViewContext.CategoryDataContract.ReviewStatusTypeCode =
                        ApplicantCategoryComplianceStatus.Incomplete.GetStringValue();

                    #endregion

                    #region SET THE ITEM DATA TO SAVE-UPDATE

                    CurrentViewContext.ItemDataContract = new ApplicantComplianceItemDataContract();
                    //HiddenField hdfApplicantItemDataId =
                    //    pnlForm.FindControl("hdfApplicantItemDataId") as HiddenField;
                    CurrentViewContext.ItemDataContract.ApplicantComplianceItemId =
                        String.IsNullOrEmpty(hdfAppItemDataId.Value)
                            ? AppConsts.NONE
                            : Convert.ToInt32(hdfAppItemDataId.Value);


                    // This dropdown value will be available in case of InsertItemTemplate. 
                    // In case InertItemTemplate the 'else' condition returns the ComplianceCategoryId
                    // In case EditItemTemplate the 'else' condition returns the ComplianceItemId
                    //UAT-1607 
                    //Boolean isItemSeries = false;
                    //if (!ddItems.SelectedValue.IsNullOrEmpty() && ddItems.SelectedValue != AppConsts.ZERO)
                    //{
                    //    isItemSeries = Convert.ToBoolean(ddItems.SelectedItem.Attributes["IsItemSeries"]);
                    //    CurrentViewContext.ItemDataContract.ComplianceItemId =
                    //        CurrentViewContext.ItemId = !isItemSeries ? Convert.ToInt32(ddItems.SelectedValue) : GetItemIDFromDropDown(ddItems.SelectedValue);

                    //}
                    //else
                    //{
                    //    CurrentViewContext.ItemDataContract.ComplianceItemId =
                    //       CurrentViewContext.ItemId = Convert.ToInt32(ParseNodeId(Convert.ToString(editForm.ParentItem.GetDataKeyValue("NodeID"))));
                    //}

                    //UAT-2143,Research Request | Complio | FIU | Student Attestation
                    if (CurrentViewContext.SelectedItemDetails.IsItemSeries)
                    {
                        CurrentViewContext.ItemDataContract.ComplianceItemId = GetItemIDFromDropDown(CurrentViewContext.SelectedItemDetails.ComplianceItemID.ToString());
                    }
                    else
                    {
                        CurrentViewContext.ItemDataContract.ComplianceItemId = CurrentViewContext.SelectedItemDetails.ComplianceItemID;
                    }
                    CurrentViewContext.IsItemSeriesSelected = CurrentViewContext.SelectedItemDetails.IsItemSeries;
                    CurrentViewContext.ItemDataContract.ReviewStatusTypeCode =
                        ApplicantItemComplianceStatus.Pending_Review.GetStringValue();
                    CurrentViewContext.ItemDataContract.Notes =
                        hdnItemNotes.Value;

                    #endregion

                    #region  SET THE ATTRIBUTE DATA TO SAVE-UPDATE

                    lstAttributesData = new List<ApplicantComplianceAttributeDataContract>();

                    Boolean IfAnydateUpdatedForExpiredItem = false;
                    Boolean ifAnyManualDateAttrisPresent = false;//UAT-3095
                    //Boolean IsDcoumentType = false;

                    var isAttributeControlToSave = false;
                    var isAttributeDataChanged = false;

                    String dataTypeCode = String.Empty;
                    ViewDocumentDetailsContract docContract = new ViewDocumentDetailsContract();

                    //if (dataTypeCode == ComplianceAttributeDatatypes.View_Document.GetStringValue())
                    //{
                    Int32 ComplianceAttributeID = lstComplianceAttributes.Where(cond => !cond.IsDeleted && cond.IsActive && cond.lkpComplianceAttributeDatatype.Code == viewDocTypeCode).FirstOrDefault().ComplianceAttributeID;
                    //Int32 ComplianceAttributeID = CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.FirstOrDefault(x => x.CIA_IsDeleted == false && x.CIA_IsActive == true).ComplianceAttribute.ComplianceAttributeID;
                    Int32 ComplianceItemID = CurrentViewContext.SelectedItemDetails.ComplianceItemID;

                    docContract = Session["ViewDocumentDetailsContract"] as ViewDocumentDetailsContract;

                    if (!docContract.IsNullOrEmpty() && !docContract.DocumentPath.IsNullOrEmpty())
                    {
                        Int32 docID = 0;
                        if (docContract.IsNotNull())
                        {
                            docID = docContract.AddedViewDocId; //CurrentViewContext.AddedViewDocId;
                        }
                        //else
                        //{
                        //    docID = UploadViewDocument(docContract);
                        //}
                        if (docID > 0)
                        {
                            AddDataToViewDocAttributeToList(AppConsts.ONE.ToString(),
                              ComplianceAttributeDatatypes.View_Document.GetStringValue(), applicantComplianceItemId, ComplianceAttributeID);
                            if (CurrentViewContext.ViewAttributeDocuments == null)
                            {
                                CurrentViewContext.ViewAttributeDocuments = new Dictionary<Int32, Int32>();
                            }

                            CurrentViewContext.ViewAttributeDocuments.Add(
                           docID, ComplianceItemID);
                            if (CurrentViewContext.IsFileUploadExists)
                            {
                                if (CurrentViewContext.AttributeDocuments == null)
                                {
                                    CurrentViewContext.AttributeDocuments = new Dictionary<Int32, Int32>();
                                }
                                if (!CurrentViewContext.AttributeDocuments.ContainsKey(docID))
                                {
                                    CurrentViewContext.AttributeDocuments.Add(docID, ComplianceItemID);
                                }
                            }
                        }
                    }
                    else
                    {
                        AddDataToViewDocAttributeToList(AppConsts.ZERO,
                              ComplianceAttributeDatatypes.View_Document.GetStringValue(), applicantComplianceItemId, ComplianceAttributeID);
                    }
                    // }




                    #endregion

                    //variable to set the ResetBusinessProcess of handle assignment method.                        
                    Boolean isItemExpired = hdnItemComplianceReviewStatus.Value == ApplicantItemComplianceStatus.Expired.ToString();
                    if (isItemExpired && IfAnydateUpdatedForExpiredItem == false && ifAnyManualDateAttrisPresent)
                    {
                        return;
                    }
                    //UAT-1682 related changes.
                    else if ((expItemList.Contains(comItemId) || expiringPendingItemLst.Contains(comItemId))
                        && IfAnydateUpdatedForExpiredItem == false && ifAnyManualDateAttrisPresent)
                    {
                        return;
                    }

                    //UAT-3888
                    if (!string.IsNullOrWhiteSpace(hdnItemComplianceReviewStatus.Value)
                        && hdnItemComplianceReviewStatus.Value != ApplicantItemComplianceStatus.Incomplete.ToString()
                        && isAttributeControlToSave
                        && !isAttributeDataChanged)
                    {
                        return;
                    }

                    if (IsItemSeriesDataToSave)
                    {
                        //UAT-1811
                        var shotSeriesItemIds = Presenter.SaveShotSeriesAttributeData();
                        lstComplianceItemId.AddRange(shotSeriesItemIds);
                    }
                    else
                    {
                        Presenter.SaveApplicantComplianceAttributeData(isItemExpired);
                        lstComplianceItemId.Add(CurrentViewContext.ItemId);
                    }
                    #region Changes related to short series 'usp_Rule_EvaluateAdjustItemSeriesRules' SP.
                    Presenter.EvaluateAdjustItemSeriesRules();
                    #endregion

                    if (String.IsNullOrEmpty(CurrentViewContext.UIValidationErrors))
                    {
                        // base.ShowSuccessMessage("Compliance data saved successfully.");
                        //UAT-1811
                        complianceCategoryId = CurrentViewContext.ComplianceCategoryId;

                        #region CLEAR THE RELATED ID's

                        //  CurrentViewContext.ComplianceCategoryId = 0;
                        //  CurrentViewContext.ItemId = 0;
                        //  ViewState[AppConsts.APPLICANT_COMPLIANCE_CATEGORY_DATA_ID_VIEW_STATE] = String.Empty;
                        hdfAppItemDataId.Value = String.Empty;
                        hdfViewedDocPath.Value = String.Empty;
                        hdfDocFileName.Value = String.Empty;
                        hdfapplicantDocID.Value = String.Empty;
                        hdfDocViewed.Value = String.Empty;
                        hdnfIsAutoSubmitTriggerForItem.Value = String.Empty;
                        #endregion
                        CurrentViewContext.IsAutoSubmit = true;
                        tlistComplianceData.Rebind();

                        var expandedIndexes = new TreeListHierarchyIndex[tlistComplianceData.ExpandedIndexes.Count];
                        tlistComplianceData.ExpandedIndexes.AddRange(expandedIndexes);

                        for (int i = 0; i < tlistComplianceData.Items.Count; i++)
                        {
                            for (int j = 0; j < tlistComplianceData.Items[i].ChildItems.Count; j++)
                            {
                                var a = ParseNodeId(((INTSOF.UI.Contract.ComplianceManagement.ComplianceDetail)(((Telerik.Web.UI.TreeListEditableItem)(tlistComplianceData.Items[i].ChildItems[j])).DataItem)).NodeID);

                                if (a == CurrentViewContext.NodeID)
                                {
                                    tlistComplianceData.Items[i].ChildItems[j].Expanded = true;
                                    break;
                                }
                            }
                        }
                        ShowAlertMessage("Compliance data submitted successfully.", MessageType.SuccessMessage, "Success");
                    }
                    else
                    {
                        //ShowAlertMessage(CurrentViewContext.UIValidationErrors, MessageType.Error, "Validation Message(s)");
                        var expandedIndexes = new TreeListHierarchyIndex[tlistComplianceData.ExpandedIndexes.Count];
                        tlistComplianceData.ExpandedIndexes.AddRange(expandedIndexes);

                        for (int i = 0; i < tlistComplianceData.Items.Count; i++)
                        {

                            if (!tlistComplianceData.Items[i].InsertItem.IsNullOrEmpty())
                            {
                                TreeListEditFormInsertItem insertItem = tlistComplianceData.Items[i].InsertItem as TreeListEditFormInsertItem;
                                Control ucitemForm = null;
                                Panel pnlForm = null;
                                Label lblError = null;
                                if (!insertItem.IsNullOrEmpty())
                                {
                                    ucitemForm = insertItem.FindControl("itemForm") as Control;
                                }

                                if (!ucitemForm.IsNullOrEmpty())
                                {
                                    pnlForm = ucitemForm.FindControl("pnlForm") as Panel;
                                }

                                if (!pnlForm.IsNullOrEmpty())
                                {
                                    lblError = pnlForm.FindControl("lblError") as Label;
                                }

                                if (!lblError.IsNullOrEmpty())
                                {
                                    lblError.Text = CurrentViewContext.UIValidationErrors;
                                    break;
                                }
                            }
                        }
                    }

                    // InitializeSignaturePad();
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                if (IsItemSeriesDataToSave)
                {
                    base.ShowInfoMessage("A system error has occurred, please try again. If the problem persists, please contact American Databank.");
                    return;
                }
                else
                {
                    base.ShowInfoMessage("Compliance data could not be saved.");
                    return;
                }
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                if (IsItemSeriesDataToSave)
                {
                    base.ShowInfoMessage("A system error has occurred, please try again. If the problem persists, please contact American Databank.");
                    return;
                }
                else
                {
                    base.ShowInfoMessage("Compliance data could not be saved.");
                    return;
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        //Start UAT-3806
        private List<ListItemEditableBies> GetItemEditableByProperties(Int32 itemId)
        {
            return lstEditableBy
                       .Where(ap => ap.ComplianceItemId == itemId)
                       .ToList();
        }
        private Boolean IsAttributeEditable(Int32 attributeId, Int32 ItemId)
        {
            String _attributeTypeCode = LCObjectType.ComplianceATR.GetStringValue();
            String _itemTypeCode = LCObjectType.ComplianceItem.GetStringValue();
            List<ListItemEditableBies> lstItemEditableBy = GetItemEditableByProperties(ItemId);
            Boolean _isEditable = false;

            List<ListItemEditableBies> _attributeEditableByList = lstItemEditableBy.Where
                      (data => data.ComplianceAttributeId == attributeId).ToList();

            //  Code can be null in case Assignement Property is defined at the 
            // Package Level or not defined at all in the hierarchy
            if (_attributeEditableByList.IsNullOrEmpty() || _attributeEditableByList.Any(eb => String.IsNullOrEmpty(eb.EditableByCode)))
            {
                _isEditable = false;
            }
            else
            {
                // Check for Applicant
                if (lstItemEditableBy.Where(editableBy => editableBy.EditableByCode == LkpEditableBy.Applicant
                                                      && editableBy.ComplianceAttributeId == attributeId).Any())
                {
                    _isEditable = true;
                }
            }
            return _isEditable;
        }
        //END UAT
        /// <summary>
        /// Manage the display of the Upload documents while data entry for items
        /// </summary>
        /// <param name="ucItemForm">Will be always available</param>
        /// <param name="tlEditFormInsertItem">Will be AVAILABLE during NEW ITEM INSERTION MODE, NOT in UPDATE MODE</param>
        /// <param name="tlEditableItem">Will be AVAILABLE during ITEM UPDATE MODE, NOT in INSERT MODE</param>
        private void ManageFileUploadDisplay(ItemForm ucItemForm, TreeListEditFormInsertItem tlEditFormInsertItem, TreeListEditableItem tlEditableItem)
        {
            //UAT-3806
            Boolean _isAttributeEditable = true;
            //END UAT-3806
            if (ucItemForm.IsNotNull() && tlEditFormInsertItem.IsNotNull() && tlEditableItem == null)
            {
                //UAT-3806
                if (!ucItemForm.ClientItemAttributes.IsNullOrEmpty())
                {
                    CurrentViewContext.lstEditableBy = Presenter.GetEditableBiesByCategoryId();
                    Int32 complianceFileUploadTypeId = Presenter.GetComplianceFileUploadDataTypeID();
                    Int32 ComplianceManualDataType = Presenter.GetComplianceAttributeManualTypeID();
                    Int32 FileUploadAttributeID = ucItemForm.ClientItemAttributes.Where(cond => cond.ComplianceAttribute.ComplianceAttributeDatatypeID == complianceFileUploadTypeId && !cond.CIA_IsDeleted && !cond.ComplianceAttribute.IsDeleted).Select(select => select.ComplianceAttribute.ComplianceAttributeID).FirstOrDefault();
                    Int32 complianceItemID = ucItemForm.ClientItemAttributes.Select(x => x.CIA_ItemID).FirstOrDefault();
                    Boolean IsFileUploadManual = (ucItemForm.ClientItemAttributes.Where(cond => cond.ComplianceAttribute.ComplianceAttributeDatatypeID == complianceFileUploadTypeId
                                                    && !cond.CIA_IsDeleted && !cond.ComplianceAttribute.IsDeleted && cond.ComplianceAttribute.ComplianceAttributeTypeID == ComplianceManualDataType)).Count() > 0 ? true : false;
                    _isAttributeEditable = IsFileUploadManual
                                           && IsAttributeEditable(FileUploadAttributeID, complianceItemID);
                }
                WclAsyncUpload wclAsyncUpload = ucItemForm.FindControl("fupItemData") as WclAsyncUpload;
                HtmlInputButton btnDropZone = (ucItemForm.FindControl("btnDropZone") as HtmlInputButton);
                //END UAT
                Panel pnlItemDocumentUpload = ucItemForm.FindControl("pnlItemDocumentUpload") as Panel;

                if (pnlItemDocumentUpload != null)
                {
                    if (ucItemForm.IsFileUploadRequired)
                    {
                        (ucItemForm.FindControl("pnlItemDocumentUpload") as Panel).Visible = true;
                        if (!_isAttributeEditable && !CurrentViewContext.IsItemSeries)
                        {
                            wclAsyncUpload.Enabled = false;
                            if (!btnDropZone.IsNullOrEmpty())
                            {
                                btnDropZone.Style.Add("background-color", "white");
                                btnDropZone.Attributes.Remove("class");
                            }
                        }
                        else
                        {
                            wclAsyncUpload.Enabled = true;
                            if (!btnDropZone.IsNullOrEmpty())
                            {
                                btnDropZone.Attributes.Add("class", "issue-drop-zone__button");
                                btnDropZone.Style.Remove("background-color");
                            }
                        }
                    }
                    else
                    {
                        (ucItemForm.FindControl("pnlItemDocumentUpload") as Panel).Visible = false;
                    }
                }
            }
            else if (ucItemForm.IsNotNull() && tlEditableItem.IsNotNull() && tlEditFormInsertItem == null) // Will be fired in Item edit as well as update mode & Exception mode
            {
                //UAT-3806
                if (!ucItemForm.ClientItemAttributes.IsNullOrEmpty())
                {
                    CurrentViewContext.lstEditableBy = Presenter.GetEditableBiesByCategoryId();
                    Int32 complianceFileUploadTypeId = Presenter.GetComplianceFileUploadDataTypeID();
                    Int32 ComplianceManualDataType = Presenter.GetComplianceAttributeManualTypeID();
                    Int32 FileUploadAttributeID = ucItemForm.ClientItemAttributes.Where(cond => cond.ComplianceAttribute.ComplianceAttributeDatatypeID == complianceFileUploadTypeId && !cond.CIA_IsDeleted && !cond.ComplianceAttribute.IsDeleted).Select(select => select.ComplianceAttribute.ComplianceAttributeID).FirstOrDefault();
                    Int32 complianceItemID = ucItemForm.ClientItemAttributes.Select(x => x.CIA_ItemID).FirstOrDefault();
                    Boolean IsFileUploadManual = (ucItemForm.ClientItemAttributes.Where(cond => cond.ComplianceAttribute.ComplianceAttributeDatatypeID == complianceFileUploadTypeId
                                                    && !cond.CIA_IsDeleted && !cond.ComplianceAttribute.IsDeleted && cond.ComplianceAttribute.ComplianceAttributeTypeID == ComplianceManualDataType)).Count() > 0 ? true : false;
                    _isAttributeEditable = IsFileUploadManual
                                           && IsAttributeEditable(FileUploadAttributeID, complianceItemID);
                }
                WclAsyncUpload wclAsyncUpload = ucItemForm.FindControl("fupItemData") as WclAsyncUpload;
                HtmlInputButton btnDropZone = (ucItemForm.FindControl("btnDropZone") as HtmlInputButton);
                //END UAT
                Panel pnlItemDocumentUpload = ucItemForm.FindControl("pnlItemDocumentUpload") as Panel;
                if (pnlItemDocumentUpload != null)
                {
                    if (ucItemForm.IsFileUploadRequired)
                    {
                        (ucItemForm.FindControl("pnlItemDocumentUpload") as Panel).Visible = true;
                        if (!_isAttributeEditable && !CurrentViewContext.IsItemSeries)
                        {
                            wclAsyncUpload.Enabled = false;
                            if (!btnDropZone.IsNullOrEmpty())
                            {
                                btnDropZone.Style.Add("background-color", "white");
                                btnDropZone.Attributes.Remove("class");
                            }
                        }
                        else
                        {
                            wclAsyncUpload.Enabled = true;
                            if (!btnDropZone.IsNullOrEmpty())
                            {
                                btnDropZone.Attributes.Add("class", "issue-drop-zone__button");
                                btnDropZone.Style.Remove("background-color");
                            }
                        }
                    }
                    else
                    {
                        (ucItemForm.FindControl("pnlItemDocumentUpload") as Panel).Visible = false;
                    }
                }

                //UAT 4300  If "Complete Document" is only attribute for an item, item should auto-submit from student screen when document is completed
                CurrentViewContext.SelectedItemDetails = ucItemForm.ClientComplianceItem;

                if (!CurrentViewContext.SelectedItemDetails.IsNullOrEmpty())
                {
                    Panel pnlForm = ucItemForm.FindControl("pnlForm") as Panel;

                    HiddenField hdfApplicantItemDataId =
                                         pnlForm.FindControl("hdfApplicantItemDataId") as HiddenField;
                    hdfAppItemDataId.Value = hdfApplicantItemDataId.Value;

                    if (pnlForm.FindControl("txtItemNotes").IsNotNull())
                    {
                        hdnItemNotes.Value = (pnlForm.FindControl("txtItemNotes") as WclTextBox).Text;
                    }

                    List<ComplianceAttribute> lstComplianceAttributes = new List<ComplianceAttribute>();
                    String viewDocTypeCode = ComplianceAttributeDatatypes.View_Document.GetStringValue();

                    if (CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.IsNotNull())
                    {
                        lstComplianceAttributes = CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.Where(x => x.CIA_IsDeleted == false && x.CIA_IsActive == true).Select(sel => sel.ComplianceAttribute).ToList();
                    }

                    if (CurrentViewContext.SelectedItemDetails.IsPaymentType == false
                        && CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.IsNotNull()
                    && lstComplianceAttributes.Where(con => !con.IsDeleted && con.IsActive).Count() == AppConsts.ONE
                   && lstComplianceAttributes.Where(con => !con.IsDeleted && con.IsActive).FirstOrDefault().lkpComplianceAttributeDatatype.Code == viewDocTypeCode)
                    {
                        hdnfIsAutoSubmitTriggerForItem.Value = "true";
                    }
                    else
                    {
                        hdnfIsAutoSubmitTriggerForItem.Value = "false";
                    }
                }
            }
            if (ucItemForm.IsNotNull() && ucItemForm.IsFileUploadRequired)
            {
                IsFileUploadExists = true;
            }
        }

        private void SetQueryStringValues()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT] != null)
            {
                args.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
            }

            if (this.ControlUseType == AppConsts.DASHBOARD)
            {
                CurrentViewContext.TenantID = this.TenantID;
                CurrentViewContext.PackageSubscriptionId = this.PackageSubscriptionId;
                CurrentViewContext.ClientCompliancePackageID = this.ClientCompliancePackageID;
                CurrentViewContext.ApplicantID = this.ApplicantId;
            }
            else
            {
                CurrentViewContext.TenantID = args.ContainsKey("TenantId") ? Convert.ToInt32(args["TenantId"]) : AppConsts.NONE;
                CurrentViewContext.PackageId = _clientCompliancePackageID = args.ContainsKey("PackageId") ? Convert.ToInt32(args["PackageId"]) : AppConsts.NONE;
                ApplicantId = args.ContainsKey("ApplicantId") ? Convert.ToInt32(args["ApplicantId"]) : AppConsts.NONE;

            }
            //moved code for UAt-979
            if (args.ContainsKey("WorkQueueType") && args.ContainsKey("PackageSubscriptionId"))
            {
                CurrentViewContext.WorkQueue = Convert.ToString(args["WorkQueueType"]);
                CurrentViewContext.PackageSubscriptionId = Convert.ToInt32(args["PackageSubscriptionId"]);
                tlistComplianceData.Columns.FirstOrDefault(column => column.UniqueName == "ItemDataColumn").Visible = false;
                //tlistComplianceData.Columns.FirstOrDefault(column => column.UniqueName == "ExceptionDataColumn").Visible = false;
                //For UAT-307 changes commented above code. 
                this.IsAdminView = true;
            }
            if (!String.IsNullOrEmpty(this.SubscriptionMobilityStatusCode) && this.SubscriptionMobilityStatusCode == LkpSubscriptionMobilityStatus.MobilitySwitched && !(args.ContainsKey("WorkQueueType") && args.ContainsKey("PackageSubscriptionId")))
            {
                tlistComplianceData.Columns.FirstOrDefault(column => column.UniqueName == "ItemDataColumn").Visible = false;
                //tlistComplianceData.Columns.FirstOrDefault(column => column.UniqueName == "ExceptionDataColumn").Visible = false;
                //For UAT-307 changes commented above code.
            }
            //Added checks for expired and archived subscription [ UAT-977: Additional work towards archive ability]
            else if (!IsExpiredSubscription && !IsArchivedSubscription && !IsGraduated && !ArchivedGraduated)
            {
                tlistComplianceData.Columns.FirstOrDefault(column => column.UniqueName == "ItemDataColumn").Visible = true;
            }
        }

        private void ShowHidePanel(Panel panelException, Panel panelpnlEntryForm)
        {
            if (hidEditForm.Value == "1")
            {
                panelException.Visible = true;
                panelpnlEntryForm.Visible = false;
            }
            else
            {
                panelException.Visible = false;
                panelpnlEntryForm.Visible = true;
            }
        }

        /// <summary>
        /// Method to check Is exception for a category is allowed or not from assignment properties.
        /// </summary>
        /// <returns>Boolean</returns>
        private Boolean IsExceptionNotAllowed()
        {
            var assignmentProperty = _listCategoryEditableBies
                   .FirstOrDefault(editableBy => editableBy.CategoryId == CurrentViewContext.ComplianceCategoryId);
            if (assignmentProperty.IsNotNull())
            {
                return assignmentProperty.IsExceptionNotAllowed;
            }
            return false;

        }

        //UAT-3888 : return type chnaged
        /// <summary>
        /// Add data of the attributes into list, both add and update mode
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="attributeValue">Value of the attribute</param>
        private Boolean AddDataToList(object ctrl, String attributeValue, String attributeTypeCode, Boolean isfileUploadTypeAttribute = false, byte[] signature = null)
        {
            // Get the 'ApplicantComplianceAttributeId' while updating 
            Control ucAttributes = (ctrl as AttributeControl) as Control;
            Control hdfAttribute = ucAttributes.FindControlRecursive((ctrl as AttributeControl).hdfApplicantAttributeDataControlId);
            //Control hdfAttribute = ucAttributes.FindControlRecursive("hdfApplicantAttributeDataId");

            var hdnPreviousValue = ucAttributes.FindControlRecursive("hdnPreviousValue");
            var previousValue = "";
            if (hdnPreviousValue != null)
            {
                previousValue = (hdnPreviousValue as HiddenField).Value;
            }

            lstAttributesData.Add(new ApplicantComplianceAttributeDataContract
            {
                ApplicantComplianceAttributeId = String.IsNullOrEmpty((hdfAttribute as HiddenField).Value) ? AppConsts.NONE : Convert.ToInt32((hdfAttribute as HiddenField).Value),
                ApplicantComplianceItemId = CurrentViewContext.ItemId,
                ComplianceItemAttributeId = (ctrl as AttributeControl).ClientItemAttributes.CIA_AttributeID,
                AttributeValue = attributeValue,
                AttributeTypeCode = attributeTypeCode,
                //UAT-1607
                IsFileUploadTypeAttribute = isfileUploadTypeAttribute,
                Signature = signature
            });

            if (isfileUploadTypeAttribute)
            {
                List<Int32> previousIds = new List<int>();
                previousValue.Split(',').ForEach(pv =>
                {
                    Int32 temp = 0;
                    if (Int32.TryParse(pv, out temp))
                    {
                        previousIds.Add(temp);
                    }
                });
                List<Int32> currentIds = CurrentViewContext.AttributeDocuments
                    .Where(attD => attD.Value == CurrentViewContext.ItemId
                    || attD.Value == AppConsts.NONE)
                    .Select(attD => attD.Key)
                    .ToList();
                return currentIds.Except(previousIds).Any();
            }
            //else
            return previousValue != attributeValue;
        }
        //UAT-3888 : return type chnaged
        /// <summary>
        /// Add data of the attributes into list, both add and update mode
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="attributeValue">Value of the attribute</param>
        private Boolean AddDataToViewDocAttributeToList(String attributeValue, String attributeTypeCode, Int32 applicantItemDataId, Int32 complianceAttributeID, Boolean isfileUploadTypeAttribute = false, byte[] signature = null)
        {
            // Get the 'ApplicantComplianceAttributeId' while updating 
            //Control ucAttributes = (ctrl as AttributeControl) as Control;
            //Control hdfAttribute = ucAttributes.FindControlRecursive((ctrl as AttributeControl).hdfApplicantAttributeDataControlId);
            //Control hdfAttribute = ucAttributes.FindControlRecursive("hdfApplicantAttributeDataId");

            //  var hdnPreviousValue = ucAttributes.FindControlRecursive("hdnPreviousValue");
            List<ComplianceAttribute> lstComplianceAttribute = new List<ComplianceAttribute>();
            Int32 ApplAttributeDataId = AppConsts.NONE;

            if (!CurrentViewContext.SelectedItemDetails.IsNullOrEmpty() && !CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.IsNullOrEmpty())
            {
                lstComplianceAttribute = CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.Where(x => x.CIA_IsDeleted == false && x.CIA_IsActive == true).Select(sel => sel.ComplianceAttribute).ToList();
            }

            if (
                //CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.FirstOrDefault(x => x.CIA_IsDeleted == false && x.CIA_IsActive == true).ComplianceAttribute.ApplicantComplianceAttributeDatas.IsNotNull() 
                lstComplianceAttribute.Where(x => x.IsDeleted == false && x.IsActive == true && x.lkpComplianceAttributeDatatype.Code == attributeTypeCode).FirstOrDefault().ApplicantComplianceAttributeDatas.IsNotNull()
                    && lstComplianceAttribute.Where(x => x.IsDeleted == false && x.IsActive == true && x.lkpComplianceAttributeDatatype.Code == attributeTypeCode).FirstOrDefault().ApplicantComplianceAttributeDatas.Where(c => !c.IsDeleted).Count() > AppConsts.NONE
                    && lstComplianceAttribute.Where(x => x.IsDeleted == false && x.IsActive == true && x.lkpComplianceAttributeDatatype.Code == attributeTypeCode).FirstOrDefault().ApplicantComplianceAttributeDatas.Where(c => !c.IsDeleted).FirstOrDefault().ApplicantComplianceAttributeID > AppConsts.NONE)
            //   && CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.FirstOrDefault(x => x.CIA_IsDeleted == false && x.CIA_IsActive == true).ComplianceAttribute.ApplicantComplianceAttributeDatas.Count() > AppConsts.NONE
            //  && CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.FirstOrDefault(x => x.CIA_IsDeleted == false && x.CIA_IsActive == true).ComplianceAttribute.ApplicantComplianceAttributeDatas.FirstOrDefault().ApplicantComplianceAttributeID > AppConsts.NONE)
            {
                ApplAttributeDataId = CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.FirstOrDefault(x => x.CIA_IsDeleted == false && x.CIA_IsActive == true).ComplianceAttribute.ApplicantComplianceAttributeDatas.FirstOrDefault().ApplicantComplianceAttributeID;
            }
            String PreviousValue = String.Empty;
            if (attributeValue == AppConsts.ONE.ToString())
            {
                PreviousValue = _presenter.GetApplicantDocumentByApplAttrDataID(ApplAttributeDataId);
            }
            var previousValue = "";
            if (!PreviousValue.IsNullOrEmpty())
            {
                previousValue = PreviousValue;
            }

            Int32 applicantComplianceAttributeId = _presenter.GetApplicantComplianceAttributeData(applicantItemDataId, complianceAttributeID);

            lstAttributesData.Add(new ApplicantComplianceAttributeDataContract
            {
                ApplicantComplianceAttributeId = applicantComplianceAttributeId,
                ApplicantComplianceItemId = CurrentViewContext.SelectedItemDetails.ComplianceItemID,
                ComplianceItemAttributeId = lstComplianceAttribute.Where(x => x.IsDeleted == false && x.IsActive == true && x.lkpComplianceAttributeDatatype.Code == attributeTypeCode).FirstOrDefault().ComplianceAttributeID,//CurrentViewContext.SelectedItemDetails.ComplianceItemAttributes.FirstOrDefault().CIA_AttributeID,
                AttributeValue = attributeValue,
                AttributeTypeCode = attributeTypeCode,
                //UAT-1607
                IsFileUploadTypeAttribute = isfileUploadTypeAttribute,
                Signature = signature
            });

            if (isfileUploadTypeAttribute)
            {
                List<Int32> previousIds = new List<int>();
                previousValue.Split(',').ForEach(pv =>
                {
                    Int32 temp = 0;
                    if (Int32.TryParse(pv, out temp))
                    {
                        previousIds.Add(temp);
                    }
                });
                List<Int32> currentIds = CurrentViewContext.AttributeDocuments
                    .Where(attD => attD.Value == CurrentViewContext.ItemId
                    || attD.Value == AppConsts.NONE)
                    .Select(attD => attD.Key)
                    .ToList();
                return currentIds.Except(previousIds).Any();
            }
            //else
            return previousValue != attributeValue;
        }

        private byte[] GetSignatureImageBuffer(String jsonStr)
        {
            if (!string.IsNullOrEmpty(jsonStr))
            {
                System.Drawing.Bitmap signatureImage = SigJsonToImage(jsonStr);
                // Save out to memory and then to a file.
                MemoryStream mm = new MemoryStream();
                signatureImage.Save(mm, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] bufferSignature = mm.GetBuffer();
                //We dispose of all objects to make sure the files don't stay locked.
                signatureImage.Dispose();
                mm.Dispose();
                return bufferSignature;
            }
            return null;
        }

        private Bitmap SigJsonToImage(string json)
        {
            Color PenColor = Color.Black;
            Color Background = Color.White;
            int Height = 150;
            int Width = 300;
            int PenWidth = 2;
            //int FontSize = 24;

            Bitmap signatureImage = new Bitmap(Width, Height);
            signatureImage.MakeTransparent();
            using (Graphics signatureGraphic = Graphics.FromImage(signatureImage))
            {
                signatureGraphic.Clear(Background);
                signatureGraphic.SmoothingMode = SmoothingMode.AntiAlias;
                Pen pen = new Pen(PenColor, PenWidth);
                List<SignatureLine> lines = (List<SignatureLine>)JsonConvert.DeserializeObject(json ?? string.Empty, typeof(List<SignatureLine>));
                foreach (SignatureLine line in lines)
                {
                    signatureGraphic.DrawLine(pen, line.lx, line.ly, line.mx, line.my);
                }
            }
            return signatureImage;
        }

        /// <summary>
        /// Show hide command bar only when applicable
        /// </summary>
        /// <param name="cmbBarControl">Control using findcontrol</param>
        /// <param name="visibility">Visisbility to be set</param>
        private void ShowCommandBar(Control cmbBarControl, Boolean visibility)
        {
            cmbBarControl.Visible = visibility;
        }

        /// <summary>
        /// Parse the node id to extract the actual values.
        /// </summary>
        /// <param name="parseNodeId">Node Id to parse.</param>
        /// <returns>Actual id of the node</returns>
        private Int32 ParseNodeId(String parseNodeId)
        {
            if (!String.IsNullOrEmpty(parseNodeId))
            {
                String[] idElements = parseNodeId.Split('_');
                if (idElements.Length > 0)
                {
                    return Convert.ToInt32(idElements[idElements.Length - 1]);
                }
            }
            return 0;

        }

        /// <summary>
        /// Set the compliance status of the package based on the data
        /// </summary>
        /// <param name="status">Status to set</param>
        /// <param name="code">Look up code to devied the image for the status</param>
        private void SetCompliancestatus(String status, String code)
        {
            lblComplianceStatus.Text = status;
            lblPackageName.Text = (!String.IsNullOrEmpty(_subscription.CompliancePackage.PackageLabel) ? _subscription.CompliancePackage.PackageLabel : _subscription.CompliancePackage.PackageName).HtmlEncode();

            if (code.ToLower() == ApplicantPackageComplianceStatus.Not_Compliant.GetStringValue().ToLower())
            {
                imgPackageComplianceStatus.ImageUrl = AppConsts.PACKAGE_NON_COMPLIANCE_IMAGE_URL;
                lblComplianceStatus.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                imgPackageComplianceStatus.ImageUrl = AppConsts.PACKAGE_COMPLIANCE_IMAGE_URL;
                lblComplianceStatus.ForeColor = System.Drawing.Color.Green;
            }
        }

        private String UploadAllDocuments(WclAsyncUpload uploadControl)
        {
            String filePath = String.Empty;
            Boolean aWSUseS3 = false;
            String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
            filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
            if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
            {
                aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
            }
            if (tempFilePath.IsNullOrEmpty())
            {
                base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                return null;
            }
            if (!tempFilePath.EndsWith(@"\"))
            {
                tempFilePath += @"\";
            }
            tempFilePath += "Tenant(" + TenantID.ToString() + @")\";

            if (!Directory.Exists(tempFilePath))
            {
                Directory.CreateDirectory(tempFilePath);
            }

            //Check whether use AWS S3, true if need to use
            if (aWSUseS3 == false)
            {
                if (filePath.IsNullOrEmpty())
                {
                    base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                    return null;
                }

                if (!filePath.EndsWith(@"\"))
                {
                    filePath += @"\";
                }
                filePath += "Tenant(" + TenantID.ToString() + @")\";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
            }

            StringBuilder docMessage = new StringBuilder();
            //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
            StringBuilder corruptedFileMessage = new StringBuilder();
            Boolean isCorruptedFileUploaded = false;
            ToSaveApplicantUploadedDocuments = new List<ApplicantDocument>();
            foreach (UploadedFile item in uploadControl.UploadedFiles)
            {
                //String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                //String fileName = "UD_" + TenantID.ToString() + "_xxx_" + date + Path.GetExtension(item.FileName);
                String fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                //Save file
                String newTempFilePath = Path.Combine(tempFilePath, fileName);

                item.SaveAs(newTempFilePath);

                ApplicantDocument applicantDocument = new ApplicantDocument();

                //Code changes for UAT 531- As a student, I should not be able to upload a duplicate document.
                //Get original file bytes and check if same document is already uploaded
                byte[] fileBytes = File.ReadAllBytes(newTempFilePath);
                var documentName = Presenter.IsDocumentAlreadyUploaded(item.FileName, item.ContentLength, fileBytes);

                if (!documentName.IsNullOrEmpty())
                {
                    //docMessage.Append("You have already updated " + item.FileName + " document as " + documentName + ". <BR/>");
                    docMessage.Append("You have already updated " + item.FileName + " document as " + documentName + ". \\n");
                    continue;
                }

                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    //Move file to other location
                    String destFilePath = Path.Combine(filePath, fileName);
                    File.Copy(newTempFilePath, destFilePath);
                    applicantDocument.DocumentPath = destFilePath;
                }
                else
                {
                    if (filePath.IsNullOrEmpty())
                    {
                        base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                        return null;
                    }
                    if (!filePath.EndsWith(@"/"))
                    {
                        filePath += @"/";
                    }

                    //AWS code to save document to S3 location
                    AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                    String destFolder = filePath + "Tenant(" + TenantID.ToString() + @")/";
                    String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, fileName, destFolder);
                    //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                    if (returnFilePath.IsNullOrEmpty())
                    {
                        isCorruptedFileUploaded = true;
                        corruptedFileMessage.Append("Your file " + item.FileName + " is not uploaded. \\n");
                        continue;
                    }
                    applicantDocument.DocumentPath = returnFilePath; //Path.Combine(destFolder, fileName);
                }
                try
                {
                    if (!String.IsNullOrEmpty(newTempFilePath))
                    {
                        File.Delete(newTempFilePath);
                    }
                }
                catch (Exception) { }

                applicantDocument.OrganizationUserID = CurrentViewContext.CurrentLoggedInUserId;
                applicantDocument.FileName = item.FileName;
                applicantDocument.Size = item.ContentLength;
                applicantDocument.CreatedByID = CurrentViewContext.OrgUsrID; //CurrentViewContext.CurrentLoggedInUserId; UAT 1261
                applicantDocument.CreatedOn = DateTime.Now;
                applicantDocument.IsDeleted = false;
                applicantDocument.DataEntryDocumentStatusID = DataEntryDocNewStatusId;//Set Data Entry Document new status.[UAT-1049:Admin Data Entry]

                ToSaveApplicantUploadedDocuments.Add(applicantDocument);
            }
            if (ToSaveApplicantUploadedDocuments != null && ToSaveApplicantUploadedDocuments.Count > 0)
            {
                String newFilePath = String.Empty;
                if (aWSUseS3 == false)
                {
                    newFilePath = filePath;
                }
                else
                {
                    if (filePath.IsNullOrEmpty())
                    {
                        base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                        return null;
                    }
                    if (!filePath.EndsWith(@"/"))
                    {
                        filePath += @"/";
                    }
                    newFilePath = filePath + "Tenant(" + TenantID.ToString() + @")/";
                }
                Presenter.AddApplicantUploadedDocuments(newFilePath);
                //Convert and Merge uploaded documents
                Presenter.CallParallelTaskPdfConversionMerging();
            }

            //Code changes for UAT 531- As a student, I should not be able to upload a duplicate document.
            if (docMessage.Length > 0 && !(docMessage.ToString().IsNullOrEmpty()))
            {
                docMessage.Append("Please select these documents from the Document dropdown.");
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCallBackMessage('" + docMessage.ToString() + "');", true);
            }
            //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
            if (corruptedFileMessage.Length > 0 && !(corruptedFileMessage.ToString().IsNullOrEmpty()))
            {
                if (isCorruptedFileUploaded)
                {
                    corruptedFileMessage.Append("Please again upload these documents .");
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCallBackMessage('" + corruptedFileMessage.ToString() + "');", true);
                }
            }
            return docMessage.ToString();
        }

        //private Int32 UploadViewDocument(ViewDocumentDetailsContract docContract)
        //{
        //    String filePath = String.Empty;
        //    Boolean aWSUseS3 = false;
        //    String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
        //    filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
        //    if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
        //    {
        //        aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
        //    }
        //    if (tempFilePath.IsNullOrEmpty())
        //    {
        //        base.LogError("Please provide path for TemporaryFileLocation in config.", null);
        //        return 0;
        //    }
        //    if (!tempFilePath.EndsWith(@"\"))
        //    {
        //        tempFilePath += @"\";
        //    }
        //    tempFilePath += "Tenant(" + TenantID.ToString() + @")\";

        //    if (!Directory.Exists(tempFilePath))
        //        Directory.CreateDirectory(tempFilePath);

        //    //Check whether use AWS S3, true if need to use
        //    if (aWSUseS3 == false)
        //    {
        //        if (filePath.IsNullOrEmpty())
        //        {
        //            base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
        //            return 0;
        //        }

        //        if (!filePath.EndsWith(@"\"))
        //        {
        //            filePath += @"\";
        //        }
        //        filePath += "Tenant(" + TenantID.ToString() + @")\";

        //        if (!Directory.Exists(filePath))
        //            Directory.CreateDirectory(filePath);
        //    }

        //    StringBuilder docMessage = new StringBuilder();
        //    //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
        //    StringBuilder corruptedFileMessage = new StringBuilder();

        //    ToSaveApplicantUploadedDocuments = new List<ApplicantDocument>();
        //    ApplicantDocument applicantDocument = new ApplicantDocument();

        //    //Code changes for UAT 531- As a student, I should not be able to upload a duplicate document.
        //    //Get original file bytes and check if same document is already uploaded
        //    byte[] fileBytes = File.ReadAllBytes(docContract.DocumentPath);

        //    //Check whether use AWS S3, true if need to use
        //    if (aWSUseS3 == false)
        //    {
        //        //Move file to other location
        //        String destFilePath = Path.Combine(filePath, docContract.DocumentName);
        //        File.Copy(docContract.DocumentPath, destFilePath);
        //        applicantDocument.DocumentPath = destFilePath;
        //    }
        //    else
        //    {
        //        if (filePath.IsNullOrEmpty())
        //        {
        //            base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
        //            return 0;
        //        }
        //        if (!filePath.EndsWith(@"/"))
        //        {
        //            filePath += @"/";
        //        }

        //        //AWS code to save document to S3 location
        //        AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
        //        String destFolder = filePath + "Tenant(" + TenantID.ToString() + @")/";
        //        String returnFilePath = objAmazonS3.SaveDocument(docContract.DocumentPath, docContract.DocumentName, destFolder);
        //        applicantDocument.DocumentPath = returnFilePath;
        //    }


        //    applicantDocument.OrganizationUserID = CurrentViewContext.CurrentLoggedInUserId;
        //    applicantDocument.FileName = docContract.DocumentName;
        //    applicantDocument.Size = Convert.ToInt32(new FileInfo(docContract.DocumentPath).Length);
        //    applicantDocument.CreatedByID = CurrentViewContext.OrgUsrID; //CurrentViewContext.CurrentLoggedInUserId; UAT 1261
        //    applicantDocument.CreatedOn = DateTime.Now;
        //    applicantDocument.IsDeleted = false;
        //    applicantDocument.DataEntryDocumentStatusID = DataEntryDocCompleteStatusId;
        //    //applicantDocument.DocumentType = CurrentViewContext.ViewDocumentTypeID;

        //    ToSaveApplicantUploadedDocuments.Add(applicantDocument);
        //    try
        //    {
        //        if (!String.IsNullOrEmpty(docContract.DocumentPath))
        //            File.Delete(docContract.DocumentPath);
        //    }
        //    catch (Exception) { }
        //    if (ToSaveApplicantUploadedDocuments != null && ToSaveApplicantUploadedDocuments.Count > 0)
        //    {
        //        String newFilePath = String.Empty;
        //        if (aWSUseS3 == false)
        //        {
        //            newFilePath = filePath;
        //        }
        //        else
        //        {
        //            if (filePath.IsNullOrEmpty())
        //            {
        //                base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
        //                return 0;
        //            }
        //            if (!filePath.EndsWith(@"/"))
        //            {
        //                filePath += @"/";
        //            }
        //            newFilePath = filePath + "Tenant(" + TenantID.ToString() + @")/";
        //        }
        //        Int32 addedID = Presenter.AddApplicantViewDocuments(newFilePath);
        //        //Convert and Merge uploaded documents
        //        Presenter.CallParallelTaskPdfConversionMerging();
        //        return addedID;
        //    }

        //    return 0;
        //}

        /// <summary>
        /// Used for handling dashboard postbacks.
        /// </summary>
        private void ResetControlsOnFalsePostback()
        {
            tlistComplianceData.IsItemInserted = false;
            tlistComplianceData.InsertIndexes.Clear();
        }

        /// <summary>
        /// Genetate the link field for category explanatory notes - UAT 837
        /// </summary>
        /// <param name="sampleDocUrl"></param>
        /// <returns></returns>
        private String GenerateExplanatoryNotesLink(string sampleDocUrl, string sampleDocUrlLabel)
        {
            if (!String.IsNullOrEmpty(sampleDocUrl))
            {
                String linkText = sampleDocUrlLabel.IsNullOrEmpty() ? AppConsts.CATEGORY_EXP_NOTES_LINK_TEXT : sampleDocUrlLabel;// Changes in link text respective to UAT-3161.
                return "<br /><a href=\"" + sampleDocUrl + "\" onclick=\"\" target=\"_blank\");'>" + linkText + "</a>";
            }
            return String.Empty;
        }

        //Fixed issue: When admin and client admin navigates to Applicant View screen from compliance search screen, Update and Delete buttons should be in disabled mode.
        private void HideCrudButtonsForApplicantView(ComplianceDetailsContract complianceDetailsContract)
        {
            if ((!WorkQueue.IsNullOrEmpty() && WorkQueue == WorkQueueType.ComplianceSearch.ToString()))
            {
                complianceDetailsContract.ComplianceDetails.ForEach(
                       cond =>
                       {
                           cond.ShowAddException = false;
                           cond.ShowAddRequirement = false;
                           cond.ShowExceptionEditDelete = false;
                           cond.ShowExceptionAllTimeUpdate = false; // UAT- 4926
                           cond.ShowItemEditDelete = false;
                           cond.ShowItemDelete = false; //UAT-3511
                           cond.ShowEnterData = false;
                       }
                       );
            }
        }

        #region UAT-977: Additional work towards archive ability

        /// <summary>
        /// Method to set the IsExpiredSubscription and IsArchivedSubscription properties.
        /// </summary>
        private void SetArchiveProperties()
        {
            if (CurrentViewContext.Subscription.IsNotNull() && CurrentViewContext.Subscription.ExpiryDate.IsNotNull())
            {
                IsExpiredSubscription = CurrentViewContext.Subscription.ExpiryDate.Value.Date < DateTime.Now.Date ? true : false;
            }
            else
            {
                IsExpiredSubscription = false;
            }

            String archiveStateCode = String.Empty;
            if (CurrentViewContext.Subscription.IsNotNull() && CurrentViewContext.Subscription.lkpArchiveState.IsNotNull())
            {
                archiveStateCode = CurrentViewContext.Subscription.lkpArchiveState.AS_Code;
            }

            IsArchivedSubscription = archiveStateCode == ArchiveState.Archived.GetStringValue() ? true : false;

            IsGraduated = archiveStateCode == SubscriptionState.Graduated.GetStringValue() ? true : false;
            ArchivedGraduated = archiveStateCode == SubscriptionState.Archived_Graduated.GetStringValue() ? true : false;

            if (IsExpiredSubscription || IsArchivedSubscription || IsGraduated || ArchivedGraduated)
            {
                tlistComplianceData.GetColumn("ReviewStatus").Visible = false;
                tlistComplianceData.GetColumn("ItemDataColumn").Visible = false;
            }
            else
            {
                tlistComplianceData.GetColumn("ReviewStatus").Visible = true;
            }
        }

        #endregion

        #region UAT-1607: Student Data Entry Screen changes

        private void SetItemSeriesProperty(WclComboBox ddItems)
        {
            if (!ddItems.IsNullOrEmpty())
            {
                RadComboBoxItem item = ddItems.SelectedItem;
                if (!item.IsNullOrEmpty())
                {
                    IsItemSeriesDataToSave = Convert.ToBoolean(item.Attributes["IsItemSeries"]);
                }
            }
        }

        private Int32 GetItemIDFromDropDown(String selectedItemId)
        {
            if (!selectedItemId.IsNullOrEmpty())
            {
                var itemId = selectedItemId.Split('_');
                if (!itemId.IsNullOrEmpty())
                {
                    return Convert.ToInt32(itemId[1]);
                }
            }
            return AppConsts.NONE;
        }

        #endregion

        #region UAT-2143: Research Request | Complio | FIU | Student Attestation

        /// <summary>
        /// Return compliance item id.
        /// </summary>
        /// <param name="editForm"></param>
        /// <param name="ddItems"></param>
        /// <param name="isItemSeries"></param>
        /// <returns></returns>
        private Int32 GetComplianceItemID(TreeListEditFormItem editForm, WclComboBox ddItems, ref Boolean isItemSeries)
        {
            if (!ddItems.SelectedValue.IsNullOrEmpty() && ddItems.SelectedValue != AppConsts.ZERO)
            {
                isItemSeries = Convert.ToBoolean(ddItems.SelectedItem.Attributes["IsItemSeries"]);
                return !isItemSeries ? Convert.ToInt32(ddItems.SelectedValue) : GetItemIDFromDropDown(ddItems.SelectedValue);
            }
            else
            {
                return Convert.ToInt32(ParseNodeId(Convert.ToString(editForm.ParentItem.GetDataKeyValue("NodeID"))));
            }
        }

        #endregion

        private void ShowAlertMessage(String strMessage, MessageType msgType, String headerText)
        {
            String msgClass = "info";
            switch (msgType)
            {
                case MessageType.Error:
                    msgClass = "error";
                    break;
                case MessageType.Information:
                    msgClass = "info";
                    break;
                case MessageType.SuccessMessage:
                    msgClass = "sucs";
                    break;
                default:
                    msgClass = "info";
                    break;
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlertMessage"
                                                , "$page.showAlertMessageWithTitle('" + strMessage.ToString() + "','" + msgClass + "','" + headerText + "',true);", true);
        }

        private String GenerateExplanatoryNotesLinkList(ComplianceCategory cmpCategory)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var docUrl in cmpCategory.ComplianceCategoryDocUrls)
            {
                if (!docUrl.SampleDocFormURL.IsNullOrEmpty())
                {
                    sb.Append(GenerateExplanatoryNotesLink(docUrl.SampleDocFormURL, docUrl.SampleDocFormURLLabel));
                }
            }

            return sb.ToString();
        }

        private void SetUrlForVerificationDetails()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TenantId",Convert.ToString( CurrentViewContext.TenantID )},
                                                                    { "Child", ChildControls.VerificationDetailsNew},
                                                                    {"SelectedPackageSubscriptionId",Convert.ToString( CurrentViewContext.PackageSubscriptionId)},
                                                                    {"ApplicantId",Convert.ToString(ApplicantId)},
                                                                    {"WorkQueueType", CurrentViewContext.WorkQueue}
                                                                 };
            String url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            lnkVerificationDetails.HRef = url;
        }

        private void InitializeSignaturePad()
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "initializeSignaturePad();", true);
        }

        #region UAT-4067:- Method to check if the associated documents format is allowed.

        private void CheckFileTypeRestrictions(TreeListEditFormItem editformItem)
        {
            if (!editformItem.IsNullOrEmpty())
            {
                Control ucItemForm = editformItem.FindControl("itemForm") as System.Web.UI.Control;
                WclComboBox ddItems = editformItem.FindControl("cmbRequirement") as WclComboBox;

                Panel pnlForm = ucItemForm.FindControl("pnlForm") as Panel;
                Panel pnlControls = ucItemForm.FindControl("pnl") as Panel;
                WclComboBox cmbDocuments = null;

                foreach (Control rowControl in pnlControls.Controls)
                {
                    #region GET THE ROW LEVEL USER CONTROL

                    if (rowControl.GetType().BaseType == typeof(RowControl))
                    {
                        foreach (var attributeControl in rowControl.Controls)
                        {
                            if (attributeControl.GetType().BaseType ==
                                typeof(System.Web.UI.HtmlControls.HtmlContainerControl))
                            {
                                foreach (
                                    var ctrl in
                                        (attributeControl as System.Web.UI.HtmlControls.HtmlContainerControl)
                                            .Controls)
                                {
                                    #region GET THE ATTRIBUTE LEVEL USER CONTROL

                                    if (ctrl.GetType().BaseType == typeof(AttributeControl))
                                    {
                                        Panel attrPanel =
                                            ((ctrl as AttributeControl).FindControl("pnlControls") as Panel);
                                        if (attrPanel.IsNotNull())
                                        {
                                            foreach (var ctrlType in attrPanel.Controls)
                                            {
                                                #region GET THE ACTUAL VALUES BASED ON THE CONTROL TYPE

                                                Type baseControlType = ctrlType.GetType();
                                                String attributeValue = String.Empty;

                                                if (baseControlType == typeof(WclComboBox))
                                                {
                                                    //isAttributeControlToSave = true;
                                                    if (!(ctrlType as WclComboBox).CheckBoxes)
                                                    {
                                                        attributeValue = (ctrlType as WclComboBox).SelectedValue;
                                                    }
                                                    else
                                                    {
                                                        if (CurrentViewContext.AttributeDocuments == null)
                                                        {
                                                            CurrentViewContext.AttributeDocuments = new Dictionary<Int32, Int32>();
                                                        }

                                                        cmbDocuments = (ctrlType as WclComboBox);
                                                        foreach (var checkedDocument in (ctrlType as WclComboBox).CheckedItems)
                                                        {
                                                            if (!CurrentViewContext.AttributeDocuments.ContainsKey(Convert.ToInt32(checkedDocument.Value)))
                                                            {
                                                                CurrentViewContext.AttributeDocuments.Add(
                                                                Convert.ToInt32(checkedDocument.Value),
                                                                (ctrl as AttributeControl).ClientItemAttributes
                                                                                          .ComplianceItem
                                                                                          .ComplianceItemID);
                                                            }

                                                        }
                                                        #region UAT-1864 : As an applicant, I should be able to preview documents in the document selection dropdown on the submit item screen
                                                        List<RadComboBoxItem> cmb = cmbDocuments.CheckedItems.ToList();
                                                        ShowHideDocumentPreview(cmb, (ucItemForm.FindControl("dvDocumentPreview") as HtmlGenericControl), (ucItemForm.FindControl("pnlDocumentPreview") as Panel), true);
                                                        #endregion
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    #endregion

                    else
                    {

                        WclComboBox cmbUploadDocument = editformItem.FindControl("cmbUploadDocument") as WclComboBox;
                        if (cmbUploadDocument.IsNotNull() && !cmbUploadDocument.IsNullOrEmpty() && cmbUploadDocument.CheckedItems.Count > 0)
                        {
                            Panel pnlDocumentPreview = editformItem.FindControl("pnlDocumentPreview") as Panel;
                            HtmlGenericControl dvDocumentPreview = editformItem.FindControl("dvDocumentPreview") as HtmlGenericControl;

                            #region UAT-1864 : As an applicant, I should be able to preview documents in the document selection dropdown on the submit item screen.
                            List<RadComboBoxItem> cmb = cmbUploadDocument.CheckedItems.ToList();
                            ShowHideDocumentPreview(cmb, dvDocumentPreview, pnlDocumentPreview, true);
                            #endregion
                        }

                    }
                }
            }
        }

        #endregion

        #region UAT-4926      
        private Boolean IsEnableUpdateAllTimeForItem(Int32 itemId)
        {
            var lstCategories = Presenter.GetEditableBiesByPackage();
            var lstItems = lstCategories
                  .Where(isEnabled => isEnabled.CategoryId == CurrentViewContext.ComplianceCategoryId && isEnabled.ComplianceItemId == itemId).ToList();

            //UAT-2418:Student's are able to update item after rejection even when editable by does not include applicants. 
            //if (lstItems.IsNotNull() && lstItems.Any(eb => String.IsNullOrEmpty(eb.EditableByCode)))
            if (lstItems.IsNotNull() && lstItems.Count > 0)
            {
                if (lstItems.Any(enable => enable.IsEnableUpdateAllTime))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        #endregion

        #endregion

        #region Public Methods

        public void SetPropertiesForQueueManagement(Entity.ClientEntity.PackageSubscription packageSubscription)
        {
            ApplicantName = packageSubscription.OrganizationUser.LastName + ' ' + packageSubscription.OrganizationUser.FirstName;
            ApplicantID = packageSubscription.OrganizationUser.OrganizationUserID;
            RushOrderStatusId = packageSubscription.Order.RushOrderStatusID;
            HierarchyID = packageSubscription.Order.HierarchyNodeID;
            FirstName = packageSubscription.OrganizationUser.FirstName;
            LastName = packageSubscription.OrganizationUser.LastName;
            PrimaryEmailAddress = packageSubscription.OrganizationUser.PrimaryEmailAddress;
            SelectedNodeID = packageSubscription.Order.SelectedNodeID;
            //!String.IsNullOrEmpty(_subscription.CompliancePackage.PackageLabel) ? _subscription.CompliancePackage.PackageLabel : _subscription.CompliancePackage.PackageName;
        }

        #region UAT-1864 : As an applicant, I should be able to preview documents in the document selection dropdown on the submit item screen
        public void ShowHideDocumentPreview(List<RadComboBoxItem> cmb, HtmlGenericControl DivDocPreview, Panel pnlDocPreview, Boolean isDocRestrictionsCheck = false)
        {
            //UAT-4067
            List<ApplicantDocument> lstApplicantDocs = new List<ApplicantDocument>();
            //ENd UAT-4067
            if (cmb.Count > 0)
            {
                DivDocPreview.Style["display"] = "block";
            }
            else
            {
                DivDocPreview.Style["display"] = "none";
            }
            pnlDocPreview.Controls.Clear();

            //UAT-4067
            if (isDocRestrictionsCheck)
            {
                lstApplicantDocs = Presenter.GetApplicantUploadedDocuments();
            }
            //ENd UAT-4067

            foreach (var doc in cmb)
            {
                LinkButton lnkBtn = new LinkButton();
                Label nextLine = new Label();
                nextLine.Text = "<br/>";
                lnkBtn.ID = doc.Value;
                lnkBtn.Text = doc.Text;
                lnkBtn.OnClientClick = "OpenAddAnotherItemPopup(" + doc.Value + ")";
                lnkBtn.Attributes.Add("onclick", "return false;");
                pnlDocPreview.Controls.Add(lnkBtn);

                //UAT-4067//
                if (isDocRestrictionsCheck)
                {
                    String docPath = String.Empty;
                    String docExtension = String.Empty;

                    Int32 docId = Convert.ToInt32(doc.Value);

                    if (!lstApplicantDocs.IsNullOrEmpty())
                    {
                        docPath = lstApplicantDocs.Where(con => con.ApplicantDocumentID == docId && !con.IsDeleted).Select(Sel => Sel.DocumentPath).FirstOrDefault();
                        docExtension = System.IO.Path.GetExtension(docPath);
                        docExtension = docExtension.Remove(".");
                    }

                    if (CurrentViewContext.allowedFileExtensions.IsNotNull() && !docExtension.IsNullOrEmpty() && !CurrentViewContext.allowedFileExtensions.Contains(docExtension.ToLower()) && !CurrentViewContext.allowedFileExtensions.Contains(docExtension.ToUpper()))
                    {
                        Label lblFileErrorMsg = new Label();
                        lblFileErrorMsg.ID = "lblFileErrorMsg_" + doc.Value;
                        lblFileErrorMsg.Text = " ! Error: Unsupported File Format";
                        lblFileErrorMsg.ForeColor = System.Drawing.Color.Red;
                        pnlDocPreview.Controls.Add(lblFileErrorMsg);
                        hdnIsAnyRestrictedFileUploaded.Value = true.ToString().ToLower();
                    }
                }
                //END UAT-4067//
                pnlDocPreview.Controls.Add(nextLine);
            }
        }
        #endregion

        public void SetUrlForSearchScreen()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"Child",ChildControls.ComplianceSearchControl}
                                                                 };
            String url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            lnkBackSearch.HRef = url;
        }

        protected void btnAddReq_Click(object sender, EventArgs e)
        {
            TextBoxClientID = string.Empty;
            hdnEnterRequirement.Value = "true";
        }

        protected void btnUpdReq_Click(object sender, EventArgs e)
        {
            TextBoxClientID = string.Empty;
            hdnEnterRequirement.Value = "false";
        }

        protected void lnkbtnDeleteItem_Click(object sender, EventArgs e)
        {
            TextBoxClientID = string.Empty;
            hdnEnterRequirement.Value = "false";

        }

        protected void lnkApplyForException_Click(object sender, EventArgs e)
        {
            TextBoxClientID = string.Empty;
            hdnEnterRequirement.Value = "ApplyException";
            IsApplyForExceptionCall = true;
        }

        #endregion

        #endregion

        //Changes as per UAT-819 WB: Category Exception enhancements
        //protected void btnDeleteCatException_Click(object sender, EventArgs e)
        //{
        //    Presenter.DeleteCategoryException();
        //    tlistComplianceData.Rebind();
        //    //UAT-285 on submitting the infomation corresponding to Item,catogry should be expanded. 
        //    tlistComplianceData.ExpandAllItems();
        //}
    }
}

