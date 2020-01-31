using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceOperations.Views;
using CoreWeb.Shell;
using CoreWeb.Shell.Views;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using Telerik.Web.UI;
using System.Drawing;
using System.Drawing.Drawing2D;
using Newtonsoft.Json;
using Business.RepoManagers;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public partial class RotationRequirementDataEntry : BaseUserControl, IRotationRequirementDataEntryView
    {
        #region Private Variables
        private RotationRequirementDataEntryPresenter _presenter = new RotationRequirementDataEntryPresenter();
        private Boolean _isEnterRequirementClick = false;
        private Boolean _isValFailForDeletion = false;
        Int32 organizationUserID = 0;
        #endregion

        #region Properties
        #region Public Properties

        public RotationRequirementDataEntryPresenter Presenter
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

        public IRotationRequirementDataEntryView CurrentViewContext
        {
            get
            {
                return this;
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

        public Int32 SelectedTenantId
        {
            get
            {
                if (!ViewState["SelectedTenantId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SelectedTenantId"]);
                }
                return 0;
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        public Int32 ClinicalRotationID
        {
            get
            {
                if (!ViewState["ClinicalRotationID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["ClinicalRotationID"]);
                }
                return 0;
            }
            set
            {
                ViewState["ClinicalRotationID"] = value;
            }
        }

        public Int32 RequirementPackageSubscriptionID
        {
            get
            {
                if (!ViewState["RequirementPackageSubscriptionID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["RequirementPackageSubscriptionID"]);
                }
                return 0;
            }
            set
            {
                ViewState["RequirementPackageSubscriptionID"] = value;
            }
        }

        public String ControlUseType
        {
            get
            {
                if (ViewState["ControlUseType"] != null)
                    return (Convert.ToString(ViewState["ControlUseType"]));
                return String.Empty;
            }
            set
            {
                ViewState["ControlUseType"] = value;
            }
        }
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
        //UAT-2040
        public String ClinicalRotationIDs
        {
            get
            {
                if (!ViewState["ClinicalRotationIDs"].IsNull())
                {
                    return Convert.ToString(ViewState["ClinicalRotationIDs"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["ClinicalRotationIDs"] = value;
            }
        }
        //UAT-2040
        public List<ClinicalRotationDetailContract> lstclinicalRotationDetailContract
        {
            get;
            set;
        }


        public String RequirementCategoryURL
        { get; set; }

        //UAT-3161
        public String RequirementCategoryURLLabel
        { get; set; }

        public Boolean IsDisplayMultipleRotationDetails
        {
            get;
            set;
        }

        public List<RequirementItemPaymentContract> ItemPaymentList
        {
            get
            {
                if (ViewState["ItemPaymentList"].IsNotNull())
                    return ViewState["ItemPaymentList"] as List<RequirementItemPaymentContract>;
                return new List<RequirementItemPaymentContract>();
            }
            set
            {
                ViewState["ItemPaymentList"] = value;
            }
        }

        public List<RequirementExpiringItemListContract> lstRequirementExpiringItem
        {
            get
            {
                if (ViewState["lstRequirementExpiringItem"].IsNotNull())
                    return ViewState["lstRequirementExpiringItem"] as List<RequirementExpiringItemListContract>;
                return new List<RequirementExpiringItemListContract>();
            }
            set
            {
                ViewState["lstRequirementExpiringItem"] = value;
            }
        }

        //ExpiringReqItemList
        public List<Int32> ExpiringReqItemList { get; set; }
        #endregion

        #region Context Properties

        ClinicalRotationDetailContract IRotationRequirementDataEntryView.ClinicalRotationDetails
        {
            get;
            set;
        }

        RequirementPackageContract IRotationRequirementDataEntryView.RotationPackageDetail
        {
            get
            {
                if (ViewState["RotationPackageDetail"].IsNotNull())
                    return ViewState["RotationPackageDetail"] as RequirementPackageContract;
                return new RequirementPackageContract();
            }
            set
            {
                ViewState["RotationPackageDetail"] = value;
            }
        }

        RequirementPackageSubscriptionContract IRotationRequirementDataEntryView.RotationSubscriptionDetail
        {
            get
            {
                if (ViewState["RotationSubscriptionDetail"].IsNotNull())
                    return ViewState["RotationSubscriptionDetail"] as RequirementPackageSubscriptionContract;
                return new RequirementPackageSubscriptionContract();
            }
            set
            {
                ViewState["RotationSubscriptionDetail"] = value;
            }
        }

        Int32 IRotationRequirementDataEntryView.RequirementCategoryDataId
        {
            get
            {
                if (ViewState["RequirementCategoryDataId"].IsNotNull())
                    return Convert.ToInt32(ViewState["RequirementCategoryDataId"]);
                return 0;
            }
            set
            {
                ViewState["RequirementCategoryDataId"] = Convert.ToString(value);
            }
        }

        Int32 IRotationRequirementDataEntryView.RequirementCategoryId
        {
            get
            {
                if (ViewState["RequirementCategoryId"].IsNotNull())
                    return Convert.ToInt32(ViewState["RequirementCategoryId"]);
                return 0;
            }
            set
            {
                ViewState["RequirementCategoryId"] = Convert.ToString(value);
            }
        }


        Int32 IRotationRequirementDataEntryView.RequirementItemDataId
        {
            get
            {
                if (ViewState["RequirementItemDataId"].IsNotNull())
                    return Convert.ToInt32(ViewState["RequirementItemDataId"]);
                return 0;
            }
            set
            {
                ViewState["RequirementItemDataId"] = Convert.ToString(value);
            }
        }

        Int32 IRotationRequirementDataEntryView.RequirementItemId
        {
            get
            {
                if (ViewState["RequirementItemId"].IsNotNull())
                    return Convert.ToInt32(ViewState["RequirementItemId"]);
                return 0;
            }
            set
            {
                ViewState["RequirementItemId"] = Convert.ToString(value);
            }
        }

        Int32 IRotationRequirementDataEntryView.RequirementPackageId
        {
            get
            {
                if (ViewState["RequirementPackageId"].IsNotNull())
                    return Convert.ToInt32(ViewState["RequirementPackageId"]);
                return 0;
            }
            set
            {
                ViewState["RequirementPackageId"] = hdRequirementPackageId.Value = Convert.ToString(value);
            }
        }

        List<RequirementItemContract> IRotationRequirementDataEntryView.lstAvailableItems
        {
            get;
            set;
        }

        Boolean IRotationRequirementDataEntryView.IsUIValidationApplicable
        {
            get;
            set;
        }
        String IRotationRequirementDataEntryView.UIValidationErrors
        {
            get;
            set;
        }

        Int32 IRotationRequirementDataEntryView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Dictionary<Int32, String> IRotationRequirementDataEntryView.SavedApplicantDocuments
        {
            get;
            set;
        }

        Boolean IRotationRequirementDataEntryView.IsClinicalRotationExpired
        {
            get
            {
                if (!ViewState["IsClinicalRotationExpired"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsClinicalRotationExpired"]);
                }
                return false;
            }
            set
            {
                ViewState["IsClinicalRotationExpired"] = value;
            }
        }

        /// <summary>
        /// Organization user id
        /// </summary>
        public Int32 OrganiztionUserID
        {
            get { return organizationUserID == 0 ? base.CurrentUserId : organizationUserID; }
            set { organizationUserID = value; }

        }

        /// <summary>
        /// UAT 1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        /// </summary>
        Int32 IRotationRequirementDataEntryView.OrgUsrID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                }
                else
                {
                    return CurrentViewContext.CurrentLoggedInUserId;
                }
            }
        }

        String IRotationRequirementDataEntryView.RequirementCategoryName { get; set; }

        //Changes related to bud ID:15048
        String IRotationRequirementDataEntryView.ItemPreviousStatsCode { get; set; }

        /// <summary>
        /// Identify if the screen is opened from the Rotation Members listing screen.
        /// </summary>
        public Boolean IsOpenInReadOnlyMode
        {
            get
            {
                if (ViewState["IsOpenInReadOnlyMode"] != null)
                {
                    return (Convert.ToBoolean(ViewState["IsOpenInReadOnlyMode"]));
                }
                return false;
            }
            set
            {
                ViewState["IsOpenInReadOnlyMode"] = value;
            }
        }

        public bool IsApplicantDropped
        {
            get
            {
                if (!ViewState["IsApplicantDropped"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsApplicantDropped"]);
                }
                return false;
            }
            set
            {
                ViewState["IsApplicantDropped"] = value;
            }
        }

        public Int32 ApplicantID
        {
            get
            {
                if (ViewState["ApplicantID"].IsNotNull())
                    return Convert.ToInt32(ViewState["ApplicantID"]);
                return 0;
            }
            set
            {
                ViewState["ApplicantID"] = value;
            }
        }

        String IRotationRequirementDataEntryView.QuizConfigSetting
        {
            get
            {
                if (!ViewState["QuizConfigSetting"].IsNull())
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

        Boolean IRotationRequirementDataEntryView.IsOptionalCategoryClientSettingEnabled { get; set; }

        //UAT-3737
        public Boolean IsIntructorPreceptorPkg
        {
            get
            {
                if (ViewState["IsIntructorPreceptorPkg"] != null)
                {
                    return (Convert.ToBoolean(ViewState["IsIntructorPreceptorPkg"]));
                }
                return false;
            }
            set
            {
                ViewState["IsIntructorPreceptorPkg"] = value;
            }
        }
        public RequirementItemContract SelectedItemDetails
        {
            get
            {
                if (ViewState["SelectedItemDetails"].IsNotNull())
                    return ViewState["SelectedItemDetails"] as RequirementItemContract;
                return new RequirementItemContract();
            }
            set
            {
                ViewState["SelectedItemDetails"] = value;
            }
        }
        #region Data Contract Properties
        ApplicantRequirementItemDataContract IRotationRequirementDataEntryView.RequirementItemDataContract { get; set; }
        ApplicantRequirementCategoryDataContract IRotationRequirementDataEntryView.RequirementCategoryDataContract { get; set; }
        List<ApplicantRequirementFieldDataContract> IRotationRequirementDataEntryView.ApplicantFieldDataContractList { get; set; }
        Dictionary<Int32, Int32> IRotationRequirementDataEntryView.FieldDocuments { get; set; }

        List<ApplicantDocumentContract> IRotationRequirementDataEntryView.ToSaveApplicantUploadedDocuments { get; set; }
        Boolean IRotationRequirementDataEntryView.IsAppDataSavedSuccessfully { get; set; }

        Dictionary<Int32, ApplicantDocumentContract> IRotationRequirementDataEntryView.AppSignedDocumentDic { get; set; }

        #endregion

        #endregion

        #region UAT-2165:Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
        Dictionary<Int32, Boolean> IRotationRequirementDataEntryView.LstComplianceRqdCategoryMapping
        {
            get;
            set;
        }
        #endregion

        #region UAT-3532
        List<Int32> IRotationRequirementDataEntryView.ViewDocumentFieldDocumentList { get; set; }
        #endregion

        Boolean IRotationRequirementDataEntryView.IsAutoSubmit
        {
            get
            {
                if (!ViewState["IsAutoSubmit"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsAutoSubmit"]);
                return false;
            }
            set
            {
                ViewState["IsAutoSubmit"] = value;
            }
        }

        //UAt-4254
        List<RequirementCategoryDocUrl> IRotationRequirementDataEntryView.lstReqCatDocUrls
        {
            get
            {
                if (!ViewState["lstReqCatDocUrls"].IsNullOrEmpty())
                    return ViewState["lstReqCatDocUrls"] as List<RequirementCategoryDocUrl>;
                return null;
            }
            set
            {
                ViewState["lstReqCatDocUrls"] = value;
            }
        }

        //Start UAT-5062
        Boolean IRotationRequirementDataEntryView.IsUploadDocUpdated
        {
            get
            {
                if (!ViewState["IsUploadDocUpdated"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsUploadDocUpdated"]);
                return false;
            }
            set
            {
                ViewState["IsUploadDocUpdated"] = value;
            }
        }
        //End UAT-5062
        #endregion

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                CaptureQueryStringData();
                if (ControlUseType == AppConsts.SHARED_ROTATION_CONTROL_USE_TYPE_CODE)
                {
                    base.OnInit(e);
                    base.Title = "Rotation Requirement Data Entry";
                    if (!IsOpenInReadOnlyMode)
                    {
                        base.SetPageTitle("Rotation Requirement Data Entry");
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    //CurrentViewContext.ClinicalRotationID = 1032;

                    //CurrentViewContext.RequirementPackageSubscriptionID = 21;
                    //CurrentViewContext.SelectedTenantId = 104;
                    if (this.hdnPostbacksource.Text != "DE" && this.ControlUseType == AppConsts.DASHBOARD)
                    {
                        ResetControlsOnFalsePostback();
                    }
                    if (!this.IsPostBack || (!(this.hdnPostbacksource.Text == "DE") && this.ControlUseType == AppConsts.DASHBOARD))
                    {
                        Presenter.OnViewInitialized();
                        BindControls();
                        tlistRequirementData.ExpandAllItems();
                        LoadNotes();
                        //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowDataEntryTooltip();", true);

                        //UAT-1615: If I am a student using the view document for a requirement in a rotation package, I should be able to access a copy of the completed form.
                        hdfTenantId.Value = Convert.ToString(CurrentViewContext.SelectedTenantId);

                    }
                    LoadRotationDetailUC();
                    hdnOrganizationUserId.Value = Convert.ToString(CurrentViewContext.OrganiztionUserID);
                    if (this.ControlUseType != AppConsts.DASHBOARD)
                    {
                        /*UAT-2751*/
                        if (IsIntructorPreceptorPkg)
                        {
                            divInstructorName.Style.Add("display", "block");
                            lblInstructorName.InnerText = Presenter.GetInstructorNameByOrganizationUserId(ApplicantID);
                        }
                        else
                        {
                            divApplicantName.Style.Add("display", "block");
                            lblApplicantName.InnerText = Presenter.GetApplicantNameByApplicantId(ApplicantID, SelectedTenantId);
                        }
                        /*UAT-2751 End here*/
                    }
                    //UAT 4300 If "Complete Document" is only attribute for an item, item should auto-submit from student screen when document is completed
                    String controlName = this.Page.Request.Params["__EVENTTARGET"];
                    CurrentViewContext.IsAutoSubmit = false;
                    if (!String.IsNullOrEmpty(controlName) && controlName.Contains("btnAutoSubmit"))
                    {
                        btnAutoSubmit_Click(sender, e);
                        // tlistRequirementData_ItemCommand(sender, e);
                    }
                }

                if (IsOpenInReadOnlyMode)
                {
                    fsucCmdBar.Visible = false;
                    pnlRotationDetails.Visible = false;
                    dvRotationComplianceStatus.Visible = false;
                    //base.HideTitleBars();
                    tlistRequirementData.GetColumn("ItemDataColumn").Visible = false;
                }
                hdnSignatureMinLengh.Value = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings[INTSOF.Utils.AppConsts.MIN_PROFILE_SHARING_SIGN_LENGTH_KEY]);
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

        #region TreeList Events
        protected void tlistRequirementData_NeedDataSource(object sender, Telerik.Web.UI.TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                if (Visiblity.IsNullOrEmpty() || Visiblity == true)
                {
                    bool isNeedToHideDocumentLink = IsOpenInReadOnlyMode ? true : false;
                    Presenter.GetSubscriptionDetail();
                    Presenter.GetClientSettingByCode();
                    Presenter.GetQuizConfigurationSetting();

                    #region UAT-3458
                    List<Int32> expRequirementItemsList = lstRequirementExpiringItem.Where(cond => cond.ShowUpdateDelete
                        // && cond.ItemRequirementStatus != RequirementItemStatus.PENDING_REVIEW.GetStringValue()
                                                              && cond.ItemRequirementStatus != RequirementItemStatus.NOT_APPROVED.GetStringValue()
                                                              )
                                                       .Select(cond => cond.ARID_RequirementItemID).ToList();

                    ViewState["expRequirementItemsList"] = expRequirementItemsList;
                    #endregion

                    // List<RequirementItemPaymentContract>
                    ItemPaymentList = Presenter.GetItemPaymentDetail(CurrentViewContext.RotationSubscriptionDetail.RequirementPackageSubscriptionID);
                    RotationRequirementContract rotationRequirementContract = new RotationRequirementContract(CurrentViewContext.RotationPackageDetail,
                                                       CurrentViewContext.RotationSubscriptionDetail, new List<Int32>(), "Incomplete", CurrentViewContext.IsClinicalRotationExpired,
                                                       isNeedToHideDocumentLink, CurrentViewContext.LstComplianceRqdCategoryMapping, ItemPaymentList, CurrentViewContext.ClinicalRotationID, CurrentViewContext.IsOptionalCategoryClientSettingEnabled, CurrentViewContext.QuizConfigSetting, expRequirementItemsList);
                    IsApplicantDropped = Presenter.IsApplicantDropped();
                    if (IsOpenInReadOnlyMode || IsApplicantDropped)
                    {
                        rotationRequirementContract.RotationRequiremenUIContractList.ForEach(
                           cond =>
                           {
                               //cond.ShowAddException = false;
                               cond.ShowAddRequirement = false;
                               //cond.ShowExceptionEditDelete = false;
                               cond.ShowItemEditDelete = false;
                               cond.ShowItemDelete = false; //UAT-3511
                               //cond.ShowEnterData = false;
                           }
                           );
                        if (IsApplicantDropped)
                        {
                            btnSaveNotes.Visible = false;
                        }
                    }

                    #region UAT-2165:Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
                    var lstOptionalCategories = rotationRequirementContract.RotationRequiremenUIContractList.Where(cdc => cdc.IsComplianceRequired == false).ToList();

                    if (!lstOptionalCategories.IsNullOrEmpty())
                    {
                        lstOptionalCategories.Insert(0, new RotationRequirementUIContract()
                        {
                            RequirementCategory = new RequirementCategoryContract(),
                            ImgReviewStatus = String.Empty,
                            ReviewStatus = String.Empty,
                            ImageReviewStatusPath = String.Empty,
                            IsCategoryDataEntered = false,
                            IsParent = true,
                            RequirementItem = new RequirementItemContract(),
                            ParentNodeID = String.Empty,
                            Name = "Optional Compliance Category", //"Optional Requirement Category",  //UAT-4325
                            NodeID = AppConsts.DATA_ENTRY_OPTIONAL_REQUIREMENT_CATEGORY_NODE,
                            ShowAddRequirement = false,
                            ShowItemEditDelete = false,
                            ShowItemDelete = false //UAT-3511
                        });
                    }

                    var lstRequiredCategories = rotationRequirementContract.RotationRequiremenUIContractList.Where(cdc => cdc.IsComplianceRequired == true).ToList();

                    if (!lstRequiredCategories.IsNullOrEmpty())
                    {
                        lstRequiredCategories.Insert(0, new RotationRequirementUIContract()
                        {
                            RequirementCategory = new RequirementCategoryContract(),
                            ImgReviewStatus = String.Empty,
                            ReviewStatus = String.Empty,
                            ImageReviewStatusPath = String.Empty,
                            IsCategoryDataEntered = false,
                            IsParent = true,
                            RequirementItem = new RequirementItemContract(),
                            ParentNodeID = String.Empty,
                            Name = "Required Compliance Category", //"Required Requirement Category", //UAT-4325
                            NodeID = AppConsts.DATA_ENTRY_REQUIRED_REQUIREMENT_CATEGORY_NODE,
                            ShowAddRequirement = false,
                            ShowItemEditDelete = false,
                            ShowItemDelete = false //UAT-3511
                        });
                    }

                    List<RotationRequirementUIContract> lstCombinedData = new List<RotationRequirementUIContract>();

                    lstCombinedData.AddRange(lstRequiredCategories);
                    lstCombinedData.AddRange(lstOptionalCategories);

                    tlistRequirementData.DataSource = lstCombinedData;
                    #endregion

                    //List<RequirementCategoryContract> lstCombinedDatarequirementCategories = lstCombinedData.Select(x => x.RequirementCategory).ToList();
                    hdnDocLinkExist.Value = lstCombinedData.Any(cond => !cond.RequirementCategory.RequirementDocumentLink.IsNullOrEmpty()) && CurrentViewContext.RotationSubscriptionDetail.ApplicantRequirementCategoryData.IsNullOrEmpty() ? "true" : "false";


                    //tlistRequirementData.DataSource = rotationRequirementContract.RotationRequiremenUIContractList;

                    SetRotationComplianceStatus(CurrentViewContext.RotationSubscriptionDetail.RequirementPackageSubscriptionStatusCode);

                    //UAT-4254
                    if (!CurrentViewContext.RequirementCategoryId.IsNullOrEmpty() && CurrentViewContext.RequirementCategoryId > AppConsts.NONE)
                    {
                        Presenter.GetCurrentCategoryDocUrls();
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

        protected void tlistRequirementData_ItemCommand(object sender, Telerik.Web.UI.TreeListCommandEventArgs e)
        {
            try
            {
                String docMessage = String.Empty;

                if (e.CommandName == RadTreeList.InitInsertCommandName)
                {
                    CurrentViewContext.RequirementCategoryId = ParseNodeId(Convert.ToString((e.Item as TreeListDataItem).GetDataKeyValue("NodeID")));
                    CurrentViewContext.RequirementItemId = 0;
                    SetHiddenProperties();
                    WclButton btnCancel = e.Item.FindControl("btnCancel") as WclButton;
                    if (btnCancel != null)
                        btnCancel.Visible = false;
                }
                else if (e.CommandName == RadTreeList.PerformInsertCommandName || e.CommandName == RadTreeList.UpdateCommandName)
                {
                    TreeListEditFormItem editForm = e.Item as TreeListEditFormItem;
                    Control ucItemForm = editForm.FindControl("itemForm") as System.Web.UI.Control;
                    WclComboBox ddItems = editForm.FindControl("cmbRequirement") as WclComboBox;
                    Dictionary<Int32, String> SignedDocPath = new Dictionary<Int32, String>();
                    Panel pnlForm = ucItemForm.FindControl("pnlForm") as Panel;
                    Panel pnlMessage = ucItemForm.FindControl("pnlMessage") as Panel;
                    Panel pnlControls = ucItemForm.FindControl("pnl") as Panel;

                    HiddenField hdfApplicantReqItemDataId =
                       pnlForm.FindControl("hdfApplicantReqItemDataId") as HiddenField;

                    // hdfReqItemDataId.Value = hdfApplicantReqItemDataId.Value;
                    //Changes related to bud ID:15048
                    HiddenField hdnItemStatusCode = pnlForm.FindControl("hdnItemStatusCode") as HiddenField;
                    if (!hdnItemStatusCode.IsNullOrEmpty())
                    {
                        CurrentViewContext.ItemPreviousStatsCode = hdnItemStatusCode.Value;
                    }

                    Int32 appRequirementItemDataID = String.IsNullOrEmpty(hdfApplicantReqItemDataId.Value)
                             ? AppConsts.NONE
                             : Convert.ToInt32(hdfApplicantReqItemDataId.Value);
                    // To check Status of item during applicant update his data.
                    Boolean isItemSeries = false;
                    Int32 reqItemId = GetReqItemID(editForm, ddItems, ref isItemSeries);
                    Int32 comItemId = ParseNodeId(Convert.ToString(editForm.ParentItem.GetDataKeyValue("NodeID")));
                    //Commented for CR Implementation
                    //if (applicantComplianceItemId > 0 && Presenter.IsItemStatusApproved(Convert.ToInt32(applicantComplianceItemId))
                    //    )
                    //{
                    //    base.ShowInfoMessage("This Compliance Item has already been approved. Approved Compliance Item cannot be updated.");
                    //    return;
                    //}

                    #region UAT-3458
                    Boolean IfAnydateUpdatedForExpiredItem = false;
                    Boolean ifAnyManualDateAttrisPresent = false;
                    List<Int32> expRequirementItemsList = new List<Int32>();
                    if (ViewState["expRequirementItemsList"] != null)
                    {
                        expRequirementItemsList = (List<Int32>)ViewState["expRequirementItemsList"];
                    }

                    //if (expRequirementItemsList.Contains(comItemId)
                    //      && IfAnydateUpdatedForExpiredItem == false && ifAnyManualDateAttrisPresent)
                    //{
                    //    //base.ShowInfoMessage("You cannot re-submit an expiring item without changing Date. Please update Date to reflect the date on your documentation.");
                    //    //InitializeSignaturePad();
                    //    e.Canceled = true;
                    //    //return;
                    //}
                    #endregion

                    #region SET THE REQUIREMENT CATEGORY DATA TO SAVE-UPDATE

                    CurrentViewContext.RequirementCategoryDataContract = new ApplicantRequirementCategoryDataContract();
                    //CurrentViewContext.RequirementCategoryDataContract.PackageSubscriptionId =
                    //    CurrentViewContext.PackageSubscriptionId;
                    CurrentViewContext.RequirementCategoryDataContract.RequirementCategoryID =
                        CurrentViewContext.RequirementCategoryId;
                    CurrentViewContext.RequirementCategoryDataContract.RequirementCategoryStatusCode =
                        RequirementCategoryStatus.INCOMPLETE.GetStringValue();

                    #endregion

                    #region SET THE REQUIREMENT ITEM DATA TO SAVE-UPDATE

                    CurrentViewContext.RequirementItemDataContract = new ApplicantRequirementItemDataContract();
                    CurrentViewContext.RequirementItemDataContract.RequirementItemDataID = appRequirementItemDataID;

                    // This dropdown value will be available in case of InsertItemTemplate. 
                    // In case InertItemTemplate the 'else' condition returns the ComplianceCategoryId
                    // In case EditItemTemplate the 'else' condition returns the ComplianceItemId
                    if (ddItems.SelectedValue != AppConsts.ZERO)
                        CurrentViewContext.RequirementItemDataContract.RequirementItemID =
                            CurrentViewContext.RequirementItemId = Convert.ToInt32(ddItems.SelectedValue);
                    else
                        CurrentViewContext.RequirementItemDataContract.RequirementItemID =
                           CurrentViewContext.RequirementItemId =
                           Convert.ToInt32(
                               ParseNodeId(Convert.ToString(editForm.ParentItem.GetDataKeyValue("NodeID"))));

                    if (!CurrentViewContext.RotationPackageDetail.IsNullOrEmpty()
                            && CurrentViewContext.RotationPackageDetail.IsNewPackage)
                        CurrentViewContext.RequirementItemDataContract.RequirementItemStatusCode = RequirementItemStatus.PENDING_REVIEW.GetStringValue();
                    else
                        CurrentViewContext.RequirementItemDataContract.RequirementItemStatusCode = RequirementItemStatus.SUBMITTED.GetStringValue();

                    #endregion

                    #region  SET THE REQUIREMENT FIELD DATA TO SAVE-UPDATE

                    CurrentViewContext.ApplicantFieldDataContractList = new List<ApplicantRequirementFieldDataContract>();
                    //Boolean IsDcoumentType = false;
                    WclComboBox cmbDocuments = null;
                    WclAsyncUpload uploaderItemDocuments = (ucItemForm.FindControl("fupItemData") as WclAsyncUpload);
                    if (uploaderItemDocuments.IsNotNull() && uploaderItemDocuments.Visible)
                    {
                        docMessage = UploadAllDocuments(uploaderItemDocuments);
                        Int32 _itemId = CurrentViewContext.RequirementItemId;

                        if (CurrentViewContext.SavedApplicantDocuments.IsNotNull())
                        {
                            if (CurrentViewContext.FieldDocuments == null)
                                CurrentViewContext.FieldDocuments = new Dictionary<Int32, Int32>();

                            foreach (var documentId in CurrentViewContext.SavedApplicantDocuments)
                            {
                                CurrentViewContext.FieldDocuments.Add(Convert.ToInt32(documentId.Key), AppConsts.NONE);
                            }
                        }
                    }

                    foreach (Control rowControl in pnlControls.Controls)
                    {
                        #region GET THE ROW LEVEL USER CONTROL

                        if (rowControl.GetType().BaseType == typeof(RequirementRowControl))
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
                                        #region GET THE FIELD LEVEL USER CONTROL

                                        if (ctrl.GetType().BaseType == typeof(RequirementAttributeControl))
                                        {
                                            Panel attrPanel =
                                                ((ctrl as RequirementAttributeControl).FindControl("pnlControls") as Panel);
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

                                                    String fieldDataType = String.Empty;
                                                    Int32 reqItemFieldId = AppConsts.NONE;
                                                    if (baseControlType == typeof(WclComboBox))
                                                    {
                                                        if (!(ctrlType as WclComboBox).CheckBoxes)
                                                        {
                                                            attributeValue = (ctrlType as WclComboBox).SelectedValue;
                                                            AddDataToList(ctrl, attributeValue,
                                                                          RequirementFieldDataType.OPTIONS
                                                                                                      .GetStringValue
                                                                              ());
                                                        }
                                                        else
                                                        {
                                                            if (CurrentViewContext.FieldDocuments == null)
                                                                CurrentViewContext.FieldDocuments = new Dictionary<Int32, Int32>();
                                                            cmbDocuments = (ctrlType as WclComboBox);
                                                            foreach (var checkedDocument in (ctrlType as WclComboBox).CheckedItems)
                                                            {

                                                                CurrentViewContext.FieldDocuments.Add(
                                                                    Convert.ToInt32(checkedDocument.Value),
                                                                    (ctrl as RequirementAttributeControl).RequirementFieldContract.RequirementItemID);
                                                            }
                                                            AddDataToList(ctrl, Convert.ToString(CurrentViewContext.FieldDocuments.Count()), RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue());
                                                        }
                                                    }
                                                    else if (baseControlType == typeof(WclDatePicker))
                                                    {
                                                        if ((ctrlType as WclDatePicker).SelectedDate == null)
                                                            attributeValue = String.Empty;
                                                        else
                                                            attributeValue = Convert.ToDateTime((ctrlType as WclDatePicker).SelectedDate).ToShortDateString();

                                                        #region UAT-3458
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
                                                            ifAnyManualDateAttrisPresent = true;

                                                        #endregion

                                                        AddDataToList(ctrl, attributeValue,
                                                                      RequirementFieldDataType.DATE
                                                                                                  .GetStringValue());
                                                    }
                                                    else if (baseControlType == typeof(WclTextBox))
                                                    {
                                                        if ((ctrlType as WclTextBox).Text == null)
                                                            attributeValue = String.Empty;
                                                        else
                                                            attributeValue = (ctrlType as WclTextBox).Text;

                                                        AddDataToList(ctrl, attributeValue,
                                                                      RequirementFieldDataType.TEXT
                                                                                                  .GetStringValue());
                                                    }
                                                    else if (baseControlType == typeof(System.Web.UI.HtmlControls.HtmlGenericControl))
                                                    {
                                                        fieldDataType = (ctrlType as System.Web.UI.HtmlControls.HtmlGenericControl).Attributes["FieldType"];
                                                        reqItemFieldId = Convert.ToInt32((ctrlType as System.Web.UI.HtmlControls.HtmlGenericControl).Attributes["ReqItemFieldId"]);

                                                        HtmlInputHidden hdnOutput = attrPanel.Controls[0].FindControl(string.Concat("hiddenOutput_", reqItemFieldId.ToString())) as System.Web.UI.HtmlControls.HtmlInputHidden;
                                                        HtmlGenericControl canvas = attrPanel.Controls[0].FindControl("signature") as System.Web.UI.HtmlControls.HtmlGenericControl;

                                                        if (fieldDataType == RequirementFieldDataType.SIGNATURE.GetStringValue().ToLower())
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

                                                            AddDataToList(ctrl, attributeValue, RequirementFieldDataType.SIGNATURE.GetStringValue(), signature);
                                                        }
                                                    }
                                                    else if (baseControlType == typeof(LinkButton))
                                                    {
                                                        fieldDataType = (ctrlType as LinkButton).Attributes["FieldType"];
                                                        reqItemFieldId = Convert.ToInt32((ctrlType as LinkButton).Attributes["ReqItemFieldId"]);
                                                        if (fieldDataType == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue().ToLower().Trim())
                                                        {
                                                            String signedDocPath = GetHiddenFieldValue(ctrl, reqItemFieldId, "hdfViewedDocPath");
                                                            String fileName = GetHiddenFieldValue(ctrl, reqItemFieldId, "hdfDocFileName");
                                                            attributeValue = GetHiddenFieldValue(ctrl, reqItemFieldId, "hdfIsDocumentViewed");

                                                            /*If document uploaded successsfully then docMessage is null or empty,If docMessage is null/Empty 
                                                              then only saved the Signed Document.
                                                              --> Bug ID:9888:-ADB: Rotation Data Entry: Exception error displays while data entry if Applicant browse same
                                                                  document which is already in ‘Upload Document’ field drop down then selects same document to upload 
                                                                  and tries to Submit data*/
                                                            if (!signedDocPath.IsNullOrEmpty() && docMessage.IsNullOrEmpty())
                                                            {
                                                                Int32 reqFieldId = (ctrl as RequirementAttributeControl).RequirementFieldContract.RequirementFieldID;
                                                                SignedDocPath.Add(reqFieldId, signedDocPath);
                                                                AddSignedDocumentToContract(signedDocPath, reqFieldId, fileName);
                                                            }
                                                            AddDataToList(ctrl, attributeValue, RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue());
                                                        }
                                                        else if (fieldDataType == RequirementFieldDataType.VIEW_VIDEO.GetStringValue().ToLower().Trim())
                                                        {
                                                            attributeValue = GetHiddenFieldValue(ctrl, reqItemFieldId, "hdfVideoViewedTime");
                                                            AddDataToList(ctrl, attributeValue, RequirementFieldDataType.VIEW_VIDEO.GetStringValue());
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

                    //Add the newly selecetd documents to the list, if the UI validation blocks the save
                    if (cmbDocuments.IsNotNull() && CurrentViewContext.SavedApplicantDocuments.IsNotNull())
                    {
                        foreach (var document in CurrentViewContext.SavedApplicantDocuments)
                        {
                            cmbDocuments.Items.Add(new RadComboBoxItem { Text = document.Value, Value = Convert.ToString(document.Key), Checked = true });
                        }
                    }


                    #region UAT-3458
                    if (expRequirementItemsList.Contains(reqItemId)
                          && IfAnydateUpdatedForExpiredItem == false && ifAnyManualDateAttrisPresent)
                    {
                        base.ShowInfoMessage("You cannot re-submit an expiring item without changing Date. Please update Date to reflect the date on your documentation.");
                        e.Canceled = true;
                        return;
                    }
                    #endregion

                    //UAT-2366
                    Boolean isSuccess = false;
                    String errorMsg = String.Empty;
                    if (docMessage.IsNullOrEmpty())
                    {
                        #region UAT-3083
                        //Int32 RequirementItemDataId = AppConsts.NONE;
                        //INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract itemPaymentContract = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_PARKING_CART) as INTSOF.UI.Contract.ComplianceOperation.ItemPaymentContract;
                        //if (!itemPaymentContract.IsNullOrEmpty())
                        //{
                        //    if (itemPaymentContract.ItemID == CurrentViewContext.RequirementItemId)
                        //    {
                        //        if (itemPaymentContract.ItemDataId > AppConsts.NONE)
                        //        {
                        //            RequirementItemDataId = itemPaymentContract.ItemDataId;
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    RequirementItemDataId = CurrentViewContext.RequirementItemDataId;
                        //}

                        var res = Presenter.CheckItemPayment(appRequirementItemDataID, CurrentViewContext.RequirementItemId);
                        if (res.Item1)
                        {
                            Dictionary<Boolean, String> dicResponse = Presenter.SaveAppRequirementFieldData();
                            isSuccess = !dicResponse.Keys.FirstOrDefault();
                            errorMsg = dicResponse.Values.FirstOrDefault();
                        }
                        else
                        {
                            Label lblError = pnlMessage.FindControl("lblError") as Label;
                            lblError.Text = "Payment is required for this item.";
                            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "initializeSignaturePad();", true);
                            e.Canceled = true;
                            return;
                        }
                        #endregion

                    }
                    else
                    {
                        e.Canceled = true;
                        return;
                    }

                    if (!isSuccess)
                    {
                        Label lblError = pnlMessage.FindControl("lblError") as Label;
                        lblError.Text = errorMsg;
                        e.Canceled = true;
                        if (!SignedDocPath.IsNullOrEmpty())
                        {
                            //Start UAT-5062
                            CurrentViewContext.IsUploadDocUpdated = true;
                            //End UAT-5062
                            Int32 key = SignedDocPath.Select(con => con.Key).FirstOrDefault();
                            Boolean aWSUseS3 = false;
                            if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                            {
                                aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                            }
                            String DestinationFilePath = SignedDocPath.GetValue(key);
                            String FilePath = CurrentViewContext.AppSignedDocumentDic.GetValue(key).DocumentPath;

                            if (!aWSUseS3)
                            {
                                File.Copy(FilePath, DestinationFilePath);
                            }
                            else
                            {
                                //AWS code to save document to S3 location
                                AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                                Byte[] fileBytes = objAmazonS3.RetrieveDocument(FilePath);

                                File.WriteAllBytes(DestinationFilePath, fileBytes);
                            }
                        }
                    }
                    else
                    {
                        if (CurrentViewContext.IsAppDataSavedSuccessfully)
                        {
                            base.ShowSuccessMessage("Requirement data saved successfully.");

                            //Start Bug Id-24881
                            #region CLEAR THE RELATED ID's
                            foreach (Control rowControl in pnlControls.Controls)
                            {
                                #region GET THE ROW LEVEL USER CONTROL

                                if (rowControl.GetType().BaseType == typeof(SharedUserRequirementRowControl))
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
                                                #region GET THE FIELD LEVEL USER CONTROL
                                                if (ctrl.GetType().BaseType == typeof(SharedUserRequirementAttributeControl))
                                                {
                                                    Panel attrPanel =
                                                        ((ctrl as SharedUserRequirementAttributeControl).FindControl("pnlControls") as Panel);
                                                    if (attrPanel.IsNotNull())
                                                    {
                                                        foreach (var ctrlType in attrPanel.Controls)
                                                        {
                                                            #region GET THE ACTUAL VALUES BASED ON THE CONTROL TYPE

                                                            Type baseControlType = ctrlType.GetType();
                                                            String fieldDataType = String.Empty;
                                                            Int32 reqItemFieldId = AppConsts.NONE;

                                                            if (baseControlType == typeof(LinkButton))
                                                            {
                                                                fieldDataType = (ctrlType as LinkButton).Attributes["FieldType"];
                                                                reqItemFieldId = Convert.ToInt32((ctrlType as LinkButton).Attributes["ReqItemFieldId"]);
                                                                if (fieldDataType == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue().ToLower().Trim())
                                                                {
                                                                    String signedDocPath = GetHiddenFieldValue(ctrl, reqItemFieldId, "hdfViewedDocPath");
                                                                    if (!signedDocPath.IsNullOrEmpty())
                                                                    {
                                                                        DeleteTemporaryFile(signedDocPath);
                                                                    }
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

                            // UAT-3598	As an applicant, I should see a popup alerting me if i have submitted something that expires on submission.
                            ApplicantRequirementParameterContract applicantRequirementParameterContract = new ApplicantRequirementParameterContract();
                            applicantRequirementParameterContract.TenantId = CurrentViewContext.SelectedTenantId;
                            applicantRequirementParameterContract.RequirementPkgSubscriptionId = CurrentViewContext.RequirementPackageSubscriptionID;
                            applicantRequirementParameterContract.RequirementItemId = CurrentViewContext.RequirementItemId;
                            applicantRequirementParameterContract.RequirementCategoryId = CurrentViewContext.RequirementCategoryId;
                            ApplicantRequirementItemDataContract applicantRequirementItemDataContract = ApplicantRequirementManager.GetApplicantRequirementItemData(applicantRequirementParameterContract, CurrentViewContext.CurrentLoggedInUserId);
                            if (!applicantRequirementItemDataContract.IsNullOrEmpty())
                            {
                                if (applicantRequirementItemDataContract.RequirementItemStatusCode == RequirementItemStatus.EXPIRED.GetStringValue())
                                {
                                    lblExpiryItemMsg.Text = "<p style='color:#14892c'>ATTENTION - You've submitted an already <b>Expired</b> item, which may mean your information is not up to date. Please check the date to make sure it is accurate. Otherwise, you may need to update your requirement with more up to date information.</p>";
                                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ItemExpiredAlertPopUp();", true);
                                }
                            }

                            #region CLEAR THE RELATED ID's

                            CurrentViewContext.RequirementCategoryId = 0;
                            CurrentViewContext.RequirementItemId = 0;
                            ViewState[AppConsts.APPLICANT_COMPLIANCE_CATEGORY_DATA_ID_VIEW_STATE] = String.Empty;

                            #endregion
                            tlistRequirementData.Rebind();
                            /*UAT-2722 */
                            var expandedIndexes = new TreeListHierarchyIndex[tlistRequirementData.ExpandedIndexes.Count];
                            tlistRequirementData.ExpandedIndexes.AddRange(expandedIndexes);

                            for (int i = 0; i < tlistRequirementData.Items.Count; i++)
                            {
                                for (int j = 0; j < tlistRequirementData.Items[i].ChildItems.Count; j++)
                                {
                                    var a = ParseNodeId(((INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation.RotationRequirementUIContract)((tlistRequirementData.Items[i].ChildItems[j].DataItem))).NodeID);
                                    if (a == comItemId)
                                    {
                                        tlistRequirementData.Items[i].ChildItems[j].Expanded = true;
                                        break;
                                    }
                                }
                            }
                            /*UAT-2722 end here*/
                        }
                        else
                        {
                            uploaderItemDocuments.PostbackTriggers = null;
                            Label lblError = pnlMessage.FindControl("lblError") as Label;
                            lblError.Text = "Some error has occurred.Please try again.";
                            e.Canceled = true;
                        }
                    }
                }
                else if (e.CommandName == RadTreeList.EditCommandName)
                {
                    // Command is fired when user clicks on 'Update Requirements'. So parent of the current node is the ComplianceCategoryId(Master)
                    CurrentViewContext.RequirementCategoryId = ParseNodeId(Convert.ToString((e.Item as TreeListDataItem).ParentItem.GetDataKeyValue("NodeID")));

                    // Command is fired when user clicks on 'Update Requirements'. So id of the current node is the ItemId(Master)
                    CurrentViewContext.RequirementItemId = ParseNodeId(Convert.ToString((e.Item as TreeListDataItem).GetDataKeyValue("NodeID")));
                    SetHiddenProperties();
                }
                else if (e.CommandName == RadTreeList.DeleteCommandName)
                {
                    //TreeListEditFormItem editForm = e.Item as TreeListEditFormItem;
                    //Control ucItemForm = editForm.FindControl("itemForm") as System.Web.UI.Control;
                    //Panel pnlMessage = ucItemForm.FindControl("pnlMessage") as Panel;
                    //Panel pnlForm = ucItemForm.FindControl("pnlForm") as Panel;
                    Int32 ApplicantReqItemDataId = Convert.ToInt32(e.CommandArgument);



                    Int32 requirementCategoryId = ParseNodeId(Convert.ToString((e.Item as TreeListDataItem).ParentItem.GetDataKeyValue("NodeID")));
                    Int32 requirementItemId = ParseNodeId(Convert.ToString((e.Item as TreeListDataItem).GetDataKeyValue("NodeID")));
                    //TODO:Commented for clinical Rotation
                    //HiddenField hdnItemStatusCode = pnlForm.FindControl("hdnItemStatusCode") as HiddenField;
                    //if (!hdnItemStatusCode.IsNullOrEmpty())
                    //{
                    //    CurrentViewContext.ItemPreviousStatsCode = hdnItemStatusCode.Value;
                    if (Convert.ToInt32(ApplicantReqItemDataId) > 0 && Presenter.IsItemStatusApproved(requirementItemId, requirementCategoryId, ApplicantReqItemDataId))
                    {
                        //  Label lblError = pnlMessage.FindControl("lblError") as Label;
                        //lblError.Text = "This Compliance Item has already been approved. Approved requirement Item cannot be deleted.";
                        base.ShowInfoMessage("This Requirement Item has already been approved. Approved Requirement Item cannot be deleted.");
                        e.Canceled = true;
                    }
                    else
                        Presenter.DeleteAppRequirementItemFieldData(ApplicantReqItemDataId, requirementCategoryId, requirementItemId);
                }
                else if (e.CommandName == RadTreeList.CancelCommandName)
                {
                    TreeListEditFormItem editForm = e.Item as TreeListEditFormItem;
                    Control ucItemForm = editForm.FindControl("itemForm") as System.Web.UI.Control;
                    Panel pnlControls = ucItemForm.FindControl("pnl") as Panel;
                    foreach (Control rowControl in pnlControls.Controls)
                    {
                        #region GET THE ROW LEVEL USER CONTROL

                        if (rowControl.GetType().BaseType == typeof(RequirementRowControl))
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
                                        #region GET THE FIELD LEVEL USER CONTROL

                                        if (ctrl.GetType().BaseType == typeof(RequirementAttributeControl))
                                        {
                                            Panel attrPanel =
                                                ((ctrl as RequirementAttributeControl).FindControl("pnlControls") as Panel);
                                            if (attrPanel.IsNotNull())
                                            {
                                                foreach (var ctrlType in attrPanel.Controls)
                                                {
                                                    #region GET THE ACTUAL VALUES BASED ON THE CONTROL TYPE

                                                    Type baseControlType = ctrlType.GetType();
                                                    String fieldDataType = String.Empty;
                                                    Int32 reqItemFieldId = AppConsts.NONE;

                                                    if (baseControlType == typeof(LinkButton))
                                                    {
                                                        fieldDataType = (ctrlType as LinkButton).Attributes["FieldType"];
                                                        reqItemFieldId = Convert.ToInt32((ctrlType as LinkButton).Attributes["ReqItemFieldId"]);
                                                        if (fieldDataType == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue().ToLower().Trim())
                                                        {
                                                            String signedDocPath = GetHiddenFieldValue(ctrl, reqItemFieldId, "hdfViewedDocPath");
                                                            if (!signedDocPath.IsNullOrEmpty())
                                                            {
                                                                DeleteTemporaryFile(signedDocPath);
                                                            }
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

                }
                else if (e.CommandName == "ExpandAll")
                {
                    tlistRequirementData.ExpandAllItems();

                }
                else if (e.CommandName == "CollapseAll")
                {
                    tlistRequirementData.CollapseAllItems();

                }
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "initializeSignaturePad();", true);
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


        protected void tlistRequirementData_ItemDataBound(object sender, Telerik.Web.UI.TreeListItemDataBoundEventArgs e)
        {
            try
            {
                if (e.Item is TreeListEditableItem && (e.Item as TreeListEditableItem).IsInEditMode)
                {
                    TreeListEditableItem editableItem = (TreeListEditableItem)e.Item;
                    RequirementItemForm itemForm = editableItem.FindControl("itemForm") as RequirementItemForm;
                    Control cmdBar = editableItem.FindControl("cmdBar") as Control;
                    RotationRequirementUIContract rotReqContract = editableItem.DataItem as RotationRequirementUIContract;
                    WclButton btnCancel = editableItem.FindControl("btnCancel") as WclButton;

                    if (!itemForm.IsNullOrEmpty())
                    {
                        ManageControlDisplay(itemForm as RequirementItemForm, null, editableItem, cmdBar);

                        if (!rotReqContract.IsNullOrEmpty())
                        {
                            if (!rotReqContract.RequirementItem.IsNullOrEmpty() && !rotReqContract.RequirementItem.LstRequirementField.IsNullOrEmpty())
                            {
                                ShowCommandBar(cmdBar, true);
                                btnCancel.Visible = false;
                            }
                            else
                            {
                                ShowCommandBar(cmdBar, false);
                                btnCancel.Visible = true;
                                btnCancel.ToolTip = "Click to close";
                                btnCancel.Text = "Close";
                            }
                        }
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

        protected void tlistRequirementData_ItemCreated(object sender, Telerik.Web.UI.TreeListItemCreatedEventArgs e)
        {
            try
            {
                if (e.Item is TreeListEditableItem && (e.Item as TreeListEditableItem).IsInEditMode)
                {
                    TreeListEditFormItem item = e.Item as TreeListEditFormItem;
                    if (item == null)
                    {
                        return;
                    }
                    WclComboBox cmbRequirement = item.FindControl("cmbRequirement") as WclComboBox;
                    HtmlGenericControl divSelectRequirement = item.FindControl("divSelectRequirement") as HtmlGenericControl;
                    HtmlGenericControl dvAddNewRequirement = item.FindControl("dvAddNewRequirement") as HtmlGenericControl;

                    SetHiddenProperties();
                    if (cmbRequirement.IsNotNull())
                    {
                        #region UAT-3458
                        List<Int32> expRequirementItemsList = new List<Int32>();
                        if (ViewState["expRequirementItemsList"] != null)
                        {
                            expRequirementItemsList = (List<Int32>)ViewState["expRequirementItemsList"];
                        }
                        CurrentViewContext.ExpiringReqItemList = expRequirementItemsList;
                        #endregion
                        Presenter.GetItemsAvailableForDataEntry(ExpiringReqItemList);
                        if (_isEnterRequirementClick && (CurrentViewContext.lstAvailableItems == null || CurrentViewContext.lstAvailableItems.Count == AppConsts.NONE))
                        {
                            divSelectRequirement.Visible = false;

                        }
                        if (CurrentViewContext.lstAvailableItems.IsNotNull())
                        {
                            CurrentViewContext.lstAvailableItems.Insert(AppConsts.NONE, new RequirementItemContract { RequirementItemName = AppConsts.COMBOBOX_ITEM_SELECT, RequirementItemID = AppConsts.NONE, AllowItemDataEntry = true });
                        }

                        //UAT-4300
                        if (!CurrentViewContext.IsAutoSubmit.IsNullOrEmpty() && CurrentViewContext.IsAutoSubmit)
                        {
                            dvAddNewRequirement.Visible = false;
                        }
                        //END
                        //UAT 3792 Ability to turn off applicant editibility on rotation items/categories
                        cmbRequirement.DataSource = CurrentViewContext.lstAvailableItems.Where(X => X.AllowItemDataEntry == true).ToList();
                        cmbRequirement.DataBind();
                        cmbRequirement.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(cmbRequirement_SelectedIndexChanged);
                        //UAT-2449
                        if (hidEditForm.Value == "0")
                        {
                            if (CurrentViewContext.lstAvailableItems.IsNotNull() && CurrentViewContext.lstAvailableItems.Count == 2)
                            {
                                // cmbRequirement.SelectedIndex = 2;
                                cmbRequirement.SelectedValue = CurrentViewContext.lstAvailableItems[1].RequirementItemID.ToString();
                                RadComboBoxSelectedIndexChangedEventArgs rargs = null;
                                RequirementItemForm itemForm = item.FindControl("itemForm") as RequirementItemForm;
                                if (!itemForm.IsNullOrEmpty())
                                {
                                    itemForm.LoadControl();
                                }
                                cmbRequirement_SelectedIndexChanged(cmbRequirement, rargs);
                            }
                        }

                    }
                    #region BIND THE CATEGORY TEXT AND ITS EXPLANATORY NOTES

                    Label lblForm = item.FindControl("lblForm") as Label;

                    if (lblForm.IsNotNull())
                    {
                        //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes).
                        String catExplanatoryNotes = Presenter.GetCategoryExplanatoryNotes();
                        if (!CurrentViewContext.RequirementCategoryName.IsNullOrEmpty())
                        {

                            StringBuilder sb = new StringBuilder();
                            if (!catExplanatoryNotes.IsNullOrEmpty())
                            {
                                sb.Append(catExplanatoryNotes);
                                sb.Append("&nbsp");
                            }
                            //Commented in UAT-4254
                            //if (!CurrentViewContext.RequirementCategoryURL.IsNullOrEmpty())
                            //{
                            //    String reqCatUrlLabel = CurrentViewContext.RequirementCategoryURLLabel.IsNullOrEmpty() ? "More Information" : CurrentViewContext.RequirementCategoryURLLabel; // UAT-3161
                            //    sb.Append("<br /><a href=\"" + CurrentViewContext.RequirementCategoryURL + "\" onclick=\"\" target=\"_blank\");'>" + "\r<p>" + reqCatUrlLabel + "&nbsp" + "</a>");
                            //}

                            //Added in UAT-4254
                            if (!CurrentViewContext.lstReqCatDocUrls.IsNullOrEmpty() && CurrentViewContext.lstReqCatDocUrls.Count > AppConsts.NONE)
                            {
                                foreach (RequirementCategoryDocUrl catUrl in CurrentViewContext.lstReqCatDocUrls)
                                {
                                    String reqCatUrlLabel = catUrl.RequirementCatDocUrlLabel.IsNullOrEmpty() ? "More Information" : catUrl.RequirementCatDocUrlLabel;
                                    sb.Append("&nbsp");
                                    sb.Append("<br /><a href=\"" + catUrl.RequirementCatDocUrl + "\" onclick=\"\" target=\"_blank\");'>" + "\r<p>" + reqCatUrlLabel + "&nbsp" + "</a>");
                                }
                            }

                            catExplanatoryNotes = sb.ToString();
                            lblForm.Text = String.Format("<span class='expl-title'>{0}</span><span class='expl-dur'>: </span>{1}",
                               CurrentViewContext.RequirementCategoryName,
                               catExplanatoryNotes);
                        }
                        else
                        {
                            lblForm.Text = String.Empty;
                        }
                    }

                    #endregion

                    #region MANAGE THE SHOW/HIDE OF THE COMMAND BAR

                    Control ucCommandBar = item.FindControl("cmdBar") as Control;
                    if (ucCommandBar.IsNotNull() && CurrentViewContext.RequirementItemId > 0)
                    {
                        // Show command bar only if the case is edit mode. Add mode is managed through the dropdownlist
                        ShowCommandBar(ucCommandBar, true);
                        (ucCommandBar as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to save and submit the entered data for this requirement";
                        (ucCommandBar as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
                    }

                    #endregion

                    //HtmlGenericControl dvOrderDetail = item.FindControl("dvOrderDetail") as HtmlGenericControl;
                    //if (!_isEnterRequirementClick && CurrentViewContext.lstAvailableItems.Count > 1 && dvOrderDetail.IsNotNull() && CurrentViewContext.RequirementItemId > 0) 
                    //{
                    //    TreeListEditableItem editableItem = (TreeListEditableItem)e.Item;
                    //    dvOrderDetail.Visible = true;
                    //    dvOrderDetail.InnerHtml = "<table class='tbl-data' style='width: 40%;'>" + ((RotationRequirementUIContract)(editableItem.DataItem)).FieldHtmlPaymentItem + " </table>";
                    //}

                    var panelpnlEntryForm = (Panel)item.FindControl("pnlEntryForm");

                    //Added one more check of hideEditForm for deletion of ItemData and _isValFailForDeletion to show the info message 
                    //In case deletion of approved and expired item
                    if (hidEditForm.Value == "2") // 2 - Deletion of the Item Data
                    {
                        panelpnlEntryForm.Visible = false;
                        if (_isValFailForDeletion)
                        {
                            base.ShowInfoMessage("This Compliance Item has already been approved. Approved Compliance Item cannot be deleted.");
                        }
                    }

                    //UAT 2899 As an applicant, I should be able to see the category more information link for rotation categories even after all items have been submitted. 
                    if (hidEditForm.Value == "-1")
                    {
                        var pnlName1 = (Panel)item.FindControl("pnlName1");
                        if (pnlName1.IsNotNull())
                        {
                            pnlName1.Visible = false;
                        }
                        Label lblEDTFormHdr = item.FindControl("lblEDTFormHdr") as Label;
                        if (lblEDTFormHdr.IsNotNull())
                        {
                            lblEDTFormHdr.Visible = false;
                        }
                        Control ucCommandBar1 = item.FindControl("cmdBar") as Control;
                        if (ucCommandBar1.IsNotNull())
                        {
                            ShowCommandBar(ucCommandBar, true);
                            (ucCommandBar as CoreWeb.Shell.Views.CommandBar).SaveButton.Visible = false;
                            (ucCommandBar as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";
                        }
                        hidEditForm.Value = String.Empty;
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

        protected void btnViewReq_Command(object sender, CommandEventArgs e)
        {
            hidEditForm.Value = "-1";
        }
        protected void btnAutoSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedItemDetails != null && CurrentViewContext.SelectedItemDetails.IsPaymentType == false && CurrentViewContext.SelectedItemDetails.LstRequirementField.Count == AppConsts.ONE && CurrentViewContext.SelectedItemDetails.LstRequirementField.FirstOrDefault().RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
                {
                    Int32 appRequirementItemDataID = String.IsNullOrEmpty(hdfReqItemDataId.Value)
                               ? AppConsts.NONE
                               : Convert.ToInt32(hdfReqItemDataId.Value);

                    #region SET THE REQUIREMENT CATEGORY DATA TO SAVE-UPDATE

                    CurrentViewContext.RequirementCategoryDataContract = new ApplicantRequirementCategoryDataContract();
                    CurrentViewContext.RequirementCategoryDataContract.RequirementCategoryID =
                        CurrentViewContext.RequirementCategoryId;
                    CurrentViewContext.RequirementCategoryDataContract.RequirementCategoryStatusCode =
                        RequirementCategoryStatus.INCOMPLETE.GetStringValue();

                    #endregion

                    #region SET THE REQUIREMENT ITEM DATA TO SAVE-UPDATE

                    CurrentViewContext.RequirementItemDataContract = new ApplicantRequirementItemDataContract();
                    CurrentViewContext.RequirementItemDataContract.RequirementItemDataID = appRequirementItemDataID;

                    if (CurrentViewContext.SelectedItemDetails != null)
                        CurrentViewContext.RequirementItemDataContract.RequirementItemID =
                            CurrentViewContext.RequirementItemId = CurrentViewContext.SelectedItemDetails.RequirementItemID;

                    if (!CurrentViewContext.RotationPackageDetail.IsNullOrEmpty()
                            && CurrentViewContext.RotationPackageDetail.IsNewPackage)
                        CurrentViewContext.RequirementItemDataContract.RequirementItemStatusCode = RequirementItemStatus.PENDING_REVIEW.GetStringValue();
                    else
                        CurrentViewContext.RequirementItemDataContract.RequirementItemStatusCode = RequirementItemStatus.SUBMITTED.GetStringValue();

                    #endregion

                    #region  SET THE REQUIREMENT FIELD DATA TO SAVE-UPDATE

                    CurrentViewContext.ApplicantFieldDataContractList = new List<ApplicantRequirementFieldDataContract>();
                    Int32 reqFieldId = CurrentViewContext.SelectedItemDetails.LstRequirementField.FirstOrDefault().RequirementFieldID;
                    String fieldDataType = CurrentViewContext.SelectedItemDetails.LstRequirementField.FirstOrDefault().RequirementFieldData.RequirementFieldDataTypeCode;
                    Dictionary<Int32, String> SignedDocPath = new Dictionary<Int32, String>();

                    if (fieldDataType.ToLower() == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue().ToLower().Trim())
                    {
                        String signedDocPath = hdfReqViewedDocPath.Value;
                        String fileName = hdfReqDocFileName.Value;
                        String attributeValue = hdfIsReqDocViewed.Value;

                        if (!signedDocPath.IsNullOrEmpty())
                        {
                            SignedDocPath.Add(reqFieldId, signedDocPath);
                            AddSignedDocumentToContract(signedDocPath, reqFieldId, fileName, "btnAutoSubmit"); //Bug Id-24881
                        }

                        byte[] signature = null;

                        Int32 applicantReqFieldDataID = _presenter.GetApplicantRequirementFieldData(appRequirementItemDataID, reqFieldId);  //CurrentViewContext.ApplicantFieldDataContractList.IsNotNull() && CurrentViewContext.ApplicantFieldDataContractList.Count >0 ? CurrentViewContext.ApplicantFieldDataContractList.FirstOrDefault().ApplicantReqFieldDataID : AppConsts.NONE;

                        CurrentViewContext.ApplicantFieldDataContractList.Add(new ApplicantRequirementFieldDataContract
                        {
                            ApplicantReqFieldDataID = applicantReqFieldDataID,
                            RequirementItemDataID = CurrentViewContext.RequirementItemId,
                            RequirementFieldID = reqFieldId,
                            FieldValue = attributeValue,
                            FieldDataTypeCode = fieldDataType,
                            Signature = signature
                        });
                    }

                    #endregion

                    Boolean isSuccess = false;
                    String errorMsg = String.Empty;
                    Dictionary<Boolean, String> dicResponse = Presenter.SaveAppRequirementFieldData();
                    isSuccess = !dicResponse.Keys.FirstOrDefault();
                    errorMsg = dicResponse.Values.FirstOrDefault();
                    CurrentViewContext.IsAutoSubmit = isSuccess;
                    if (!isSuccess)
                    {
                        if (!SignedDocPath.IsNullOrEmpty())
                        {

                            Int32 key = SignedDocPath.Select(con => con.Key).FirstOrDefault();
                            Boolean aWSUseS3 = false;
                            if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                            {
                                aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                            }
                            String DestinationFilePath = SignedDocPath.GetValue(key);
                            String FilePath = CurrentViewContext.AppSignedDocumentDic.GetValue(key).DocumentPath;

                            if (!aWSUseS3)
                            {
                                File.Copy(FilePath, DestinationFilePath);
                            }
                            else
                            {
                                //AWS code to save document to S3 location
                                AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                                Byte[] fileBytes = objAmazonS3.RetrieveDocument(FilePath);

                                File.WriteAllBytes(DestinationFilePath, fileBytes);
                            }
                        }
                    }
                    else
                    {
                        if (CurrentViewContext.IsAppDataSavedSuccessfully)
                        {
                            #region CLEAR THE RELATED ID's
                            //CurrentViewContext.RequirementCategoryId = 0;
                            //CurrentViewContext.RequirementItemId = 0;
                            // ViewState[AppConsts.APPLICANT_COMPLIANCE_CATEGORY_DATA_ID_VIEW_STATE] = String.Empty;

                            hdfIsReqDocViewed.Value = String.Empty;
                            hdfReqViewedDocPath.Value = String.Empty;
                            hdfReqDocFileName.Value = String.Empty;
                            hdnfReqIsAutoSubmitTriggerForItem.Value = String.Empty;
                            hdfReqItemDataId.Value = String.Empty;
                            #endregion
                            tlistRequirementData.Rebind();

                            var expandedIndexes = new TreeListHierarchyIndex[tlistRequirementData.ExpandedIndexes.Count];
                            tlistRequirementData.ExpandedIndexes.AddRange(expandedIndexes);

                            for (int i = 0; i < tlistRequirementData.Items.Count; i++)
                            {
                                for (int j = 0; j < tlistRequirementData.Items[i].ChildItems.Count; j++)
                                {
                                    var a = ParseNodeId(((INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation.RotationRequirementUIContract)((tlistRequirementData.Items[i].ChildItems[j].DataItem))).NodeID);
                                    if (a == CurrentViewContext.RequirementCategoryId)
                                    {
                                        tlistRequirementData.Items[i].ChildItems[j].Expanded = true;
                                        break;
                                    }
                                }
                            }
                            ShowAlertMessage("Requirement data submitted successfully.", MessageType.SuccessMessage, "Success");
                        }
                        else
                        {
                            //ShowAlertMessage("Some error has occurred.Please try again.", MessageType.Error, "Validation Message(s)");
                        }
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
        #endregion

        #region DropDown Events

        void cmbRequirement_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

            //Clear the controls when 'SELECT' option is selected
            TreeListEditFormInsertItem insertItem = (sender as WclComboBox).NamingContainer as TreeListEditFormInsertItem;
            Control ucItemForm = insertItem.FindControl("itemForm") as System.Web.UI.Control;
            Panel pnlMessage = ucItemForm.FindControl("pnlMessage") as Panel;
            // Panel pnlItemInfo = insertItem.FindControl("pnlItemInfo") as Panel;

            Label lblError = pnlMessage.FindControl("lblError") as Label;
            lblError.Text = String.Empty;
            Control cmdBar = insertItem.FindControl("cmdBar") as Control;
            WclButton btnCancel = insertItem.FindControl("btnCancel") as WclButton;
            if ((sender as WclComboBox).SelectedValue == AppConsts.ZERO)
            {
                insertItem.FindControl("itemForm").Controls.Clear();

                ShowCommandBar(cmdBar, false);
                // pnlItemInfo.Visible = false;
            }
            else
            {
                //UAT-2195: Remove  "Fill in the form for..." text in rotation packages

                // pnlItemInfo.Visible = true;
                // Label lblItemName = insertItem.FindControl("lblItemName") as Label;
                //  lblItemName.Text = (sender as WclComboBox).SelectedItem.Text;
                // Added Custom ToolTip to the Item
                //   HtmlGenericControl ItemToolTip = insertItem.FindControl("ItemToolTipCustom") as HtmlGenericControl;
                //UAT 401 -Add a field for Items to configure "Details" during student Order Process
                // ItemToolTip.InnerHtml = Presenter.GetItemDescription(Convert.ToInt32((sender as WclComboBox).SelectedValue));  
                //Commented Code for clinical Rotation
                //ItemToolTip.InnerHtml = Presenter.GetItemDetails(Convert.ToInt32((sender as WclComboBox).SelectedValue));
                ShowCommandBar(cmdBar, true);
                (cmdBar as CoreWeb.Shell.Views.CommandBar).SaveButton.ToolTip = "Click to save and submit the entered data for this requirement";
                (cmdBar as CoreWeb.Shell.Views.CommandBar).CancelButton.ToolTip = "Click to cancel. Any data entered will not be saved";

            }
            ManageControlDisplay(ucItemForm as RequirementItemForm, insertItem, null, cmdBar);
            #region UAT-3077
            //TODO: Check the item is payment type and has no attribute
            var ItemDetails = CurrentViewContext.lstAvailableItems.Where(d => d.RequirementItemID == Convert.ToInt32((sender as WclComboBox).SelectedValue)).FirstOrDefault();

            //UAT 4300 If "Complete Document" is only attribute for an item, item should auto-submit from student screen when document is completed
            CurrentViewContext.SelectedItemDetails = ItemDetails;
            if (!CurrentViewContext.SelectedItemDetails.IsNullOrEmpty())
            {
                if (CurrentViewContext.SelectedItemDetails.IsPaymentType == false && CurrentViewContext.SelectedItemDetails.LstRequirementField.IsNotNull() && CurrentViewContext.SelectedItemDetails.LstRequirementField.Count == AppConsts.ONE && CurrentViewContext.SelectedItemDetails.LstRequirementField.FirstOrDefault().RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
                {
                    hdnfReqIsAutoSubmitTriggerForItem.Value = "true";
                }
                else
                {
                    hdnfReqIsAutoSubmitTriggerForItem.Value = "false";
                }
            }
            if (!ItemDetails.IsNullOrEmpty())
            {
                if (ItemDetails.LstRequirementField.IsNullOrEmpty() || ItemDetails.LstRequirementField.Count == AppConsts.NONE)
                {
                    ShowCommandBar(cmdBar, false);
                    btnCancel.Visible = true;
                    btnCancel.ToolTip = "Click to close";
                    btnCancel.Text = "Close";
                }
                else
                {
                    btnCancel.Visible = false;
                }
            }
            #endregion

            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "initializeSignaturePad();", true);
        }
        #endregion

        #region Button Events

        protected void fsucCmdBar_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                ReturnToManageInvitation();
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

        //UAT-1523: Addition a notes box for each rotation for the student to input information
        protected void btnSaveNotes_Click(object sender, EventArgs e)
        {
            try
            {
                if (Presenter.UpdateRequirementPackageSubscriptionNotes(txtNotes.Text))
                {
                    CurrentViewContext.RotationSubscriptionDetail.Notes = txtNotes.Text;
                    String successMessage = "Notes Saved Successfully.";
                    base.ShowSuccessMessage(successMessage);
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

        #region Private Methods

        /// <summary>
        /// To bind controls
        /// </summary>
        private void BindControls()
        {
            BindRotationDetail();
        }
        /// <summary>
        /// Bind Rotation Details section controls
        /// </summary>
        private void BindRotationDetail()
        {
            Presenter.BindRotationDetail();
            CurrentViewContext.IsClinicalRotationExpired = CurrentViewContext.ClinicalRotationDetails.EndDate.HasValue ?
                                                           CurrentViewContext.ClinicalRotationDetails.EndDate.Value.Date < DateTime.Now.Date ? true : false : false;
        }

        private void LoadRotationDetailUC()
        {
            Control ucRotationDetails = Page.LoadControl(AppConsts.ROTATION_DETAILS_CONTROL);
            (ucRotationDetails as CoreWeb.CommonControls.Views.IRotationDetails).TenantId = CurrentViewContext.SelectedTenantId;
            (ucRotationDetails as CoreWeb.CommonControls.Views.IRotationDetails).ClinicalRotationId = CurrentViewContext.ClinicalRotationID;
            hdnClinicalRotationID.Value = CurrentViewContext.ClinicalRotationID > AppConsts.NONE ? CurrentViewContext.ClinicalRotationID.ToString() : AppConsts.ZERO;
            (ucRotationDetails as CoreWeb.CommonControls.Views.IRotationDetails).IsRestrictToLoadFresshData = true;
            if (!this.IsPostBack)
            {
                (ucRotationDetails as CoreWeb.CommonControls.Views.IRotationDetails).ClinicalRotationDetails = CurrentViewContext.ClinicalRotationDetails;

            }
            pnlRotationDetails.Controls.Add(ucRotationDetails);
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
                    return Convert.ToInt32(idElements[idElements.Length - 1]);
            }
            return 0;
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
        /// Manage the display of the Upload documents and save button hide/show while data entry for items
        /// </summary>
        /// <param name="ucItemForm">Will be always available</param>
        /// <param name="tlEditFormInsertItem">Will be AVAILABLE during NEW ITEM INSERTION MODE, NOT in UPDATE MODE</param>
        /// <param name="tlEditableItem">Will be AVAILABLE during ITEM UPDATE MODE, NOT in INSERT MODE</param>
        ///<param name="cmdBar"></param>
        private void ManageControlDisplay(RequirementItemForm ucItemForm, TreeListEditFormInsertItem tlEditFormInsertItem, TreeListEditableItem tlEditableItem, Control cmdBar)
        {
            (cmdBar as CommandBar).SaveButton.ValidationGroup = "vGroup";
            (cmdBar as CommandBar).SaveButton.CausesValidation = true;
            Boolean IsUploadDocEditable = false;
            //(cmdBar as CommandBar).SaveButton.ClientIDMode = ClientIDMode.Static;
            //Shifted to attribute control
            //hdfIsViewDocumentRequired.Value = ucItemForm.IsViewDocumentRequired ? Convert.ToString(AppConsts.ONE) : Convert.ToString(AppConsts.NONE);
            //hdfIsViewVideoRequired.Value = ucItemForm.IsViewVideoRequired ? Convert.ToString(AppConsts.ONE) : Convert.ToString(AppConsts.NONE);
            //var reqItem = CurrentViewContext.RotationPackageDetail.LstRequirementCategory.Select(cond => cond.LstRequirementItem.Select(cond2 => cond2.LstRequirementField.Where(x => x.AttributeTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue()).Select(select => select.IsEditableByApplicant.Value))).ToList();
            //UAT-4380
            if (!CurrentViewContext.RotationPackageDetail.LstRequirementCategory.IsNullOrEmpty())
            {
                List<RequirementCategoryContract> lstRequirementCategories = CurrentViewContext.RotationPackageDetail.LstRequirementCategory.Where(b => b.RequirementCategoryID == CurrentViewContext.RequirementCategoryId).ToList();

                if (lstRequirementCategories.IsNotNull() && lstRequirementCategories.Count > AppConsts.NONE)
                {
                    foreach (var category in lstRequirementCategories)
                    {
                        if (category.LstRequirementItem.IsNotNull() && category.LstRequirementItem.Count > AppConsts.NONE)
                        {
                            foreach (var item in category.LstRequirementItem)
                            {
                                if (item.RequirementItemID.Equals(ucItemForm.RequirementItem.RequirementItemID))
                                {
                                    if (item.LstRequirementField.IsNotNull() && item.LstRequirementField.Count > AppConsts.NONE)
                                    {
                                        foreach (var field in item.LstRequirementField)
                                        {
                                            if (field.RequirementFieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue())
                                            {
                                                if (field.IsEditableByApplicant.Value)
                                                {
                                                    IsUploadDocEditable = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (ucItemForm.IsNotNull() && tlEditFormInsertItem.IsNotNull() && tlEditableItem == null)
            {
                Panel pnlItemDocumentUpload = ucItemForm.FindControl("pnlItemDocumentUpload") as Panel;
                if (pnlItemDocumentUpload != null)
                {
                    if (ucItemForm.IsFileUploadRequired)
                    {
                        WclAsyncUpload wclAsyncUpload = ucItemForm.FindControl("fupItemData") as WclAsyncUpload;
                        (ucItemForm.FindControl("pnlItemDocumentUpload") as Panel).Visible = true;
                        HtmlInputButton btnDropZone = (ucItemForm.FindControl("btnDropZone") as HtmlInputButton);
                        if (!IsUploadDocEditable)
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
                        (ucItemForm.FindControl("pnlItemDocumentUpload") as Panel).Visible = false;

                    //Commented below code because validators are used to validate view document and view video fields.
                    //if (ucItemForm.IsViewVideoRequired || ucItemForm.IsViewDocumentRequired)
                    //    (cmdBar as CommandBar).SaveButton.Enabled = false;
                    //else
                    //    (cmdBar as CommandBar).SaveButton.Enabled = true;

                }
            }
            else if (ucItemForm.IsNotNull() && tlEditableItem.IsNotNull() && tlEditFormInsertItem == null) // Will be fired in Item edit as well as update mode 
            {
                Panel pnlItemDocumentUpload = ucItemForm.FindControl("pnlItemDocumentUpload") as Panel;
                if (pnlItemDocumentUpload != null)
                {
                    if (ucItemForm.IsFileUploadRequired)
                    {
                        Panel panel = (ucItemForm.FindControl("pnlItemDocumentUpload") as Panel);
                        panel.Visible = true;
                        WclAsyncUpload wclAsyncUpload = ucItemForm.FindControl("fupItemData") as WclAsyncUpload;
                        HtmlInputButton btnDropZone = (pnlItemDocumentUpload.FindControl("btnDropZone") as HtmlInputButton);
                        if (!IsUploadDocEditable)
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
                        (ucItemForm.FindControl("pnlItemDocumentUpload") as Panel).Visible = false;

                    //Commented below code because validators are used to validate view document and view video fields.
                    //if (ucItemForm.IsViewVideoRequired || ucItemForm.IsViewDocumentRequired)
                    //    (cmdBar as CommandBar).SaveButton.Enabled = false;
                    //else
                    //    (cmdBar as CommandBar).SaveButton.Enabled = true;
                }
                //UAT 4300  If "Complete Document" is only attribute for an item, item should auto-submit from student screen when document is completed
                CurrentViewContext.SelectedItemDetails = ucItemForm.RequirementItem;

                if (!CurrentViewContext.SelectedItemDetails.IsNullOrEmpty())
                {
                    Panel pnlForm = ucItemForm.FindControl("pnlForm") as Panel;

                    HiddenField hdfApplicantReqItemDataId =
                                         pnlForm.FindControl("hdfApplicantReqItemDataId") as HiddenField;
                    hdfReqItemDataId.Value = hdfApplicantReqItemDataId.Value;

                    if (CurrentViewContext.SelectedItemDetails.IsPaymentType == false && CurrentViewContext.SelectedItemDetails.LstRequirementField.IsNotNull() && CurrentViewContext.SelectedItemDetails.LstRequirementField.Count == AppConsts.ONE && CurrentViewContext.SelectedItemDetails.LstRequirementField.FirstOrDefault().RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
                    {
                        hdnfReqIsAutoSubmitTriggerForItem.Value = "true";
                    }
                    else
                    {
                        hdnfReqIsAutoSubmitTriggerForItem.Value = "false";
                    }
                }
            }
        }

        private void SetHiddenProperties()
        {
            hdfRequirementItemId.Value = Convert.ToString(CurrentViewContext.RequirementItemId);
            hdfRequirementPkgSubscriptionID.Value = Convert.ToString(CurrentViewContext.RequirementPackageSubscriptionID);
            hdfTenantId.Value = Convert.ToString(CurrentViewContext.SelectedTenantId);
            hdRequirementCategoryId.Value = Convert.ToString(CurrentViewContext.RequirementCategoryId);
            hdnOrganizationUserId.Value = Convert.ToString(CurrentViewContext.OrganiztionUserID);
            //  hdnRequirementPackageName.Value = Convert.ToString(CurrentViewContext.
        }

        /// <summary>
        /// Add data of the Fields into list, both add and update mode
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="attributeValue">Value of the attribute</param>
        private void AddDataToList(object ctrl, String attributeValue, String fieldTypeCode, byte[] signature = null)
        {
            // Get the 'ApplicantComplianceAttributeId' while updating 
            Control ucFields = (ctrl as RequirementAttributeControl) as Control;
            Control hdfAttribute = FindControlRecursive(ucFields, "hdfApplicantFieldDataId");

            CurrentViewContext.ApplicantFieldDataContractList.Add(new ApplicantRequirementFieldDataContract
            {
                ApplicantReqFieldDataID = String.IsNullOrEmpty((hdfAttribute as HiddenField).Value) ? AppConsts.NONE : Convert.ToInt32((hdfAttribute as HiddenField).Value),
                RequirementItemDataID = CurrentViewContext.RequirementItemId,
                RequirementFieldID = (ctrl as RequirementAttributeControl).RequirementFieldContract.RequirementFieldID,
                FieldValue = attributeValue,
                FieldDataTypeCode = fieldTypeCode,
                Signature = signature
            });
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
        /// Finds a control nested within another control or possibly further down in the hierarchy.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private Control FindControlRecursive(Control root, String id)
        {
            if (!root.IsNull())
            {
                Control controlFound = root.FindControl(id);

                if (!controlFound.IsNull())
                {
                    return controlFound;
                }

                foreach (Control control in root.Controls)
                {
                    controlFound = FindControlRecursive(control, id);

                    if (!controlFound.IsNull())
                    {
                        return controlFound;
                    }
                }
            }
            return null;
        }

        //private void CaptureQueryStringData()
        //{
        //    Dictionary<String, String> args = new Dictionary<String, String>();
        //    if (!Request.QueryString["args"].IsNull())
        //    {
        //        args.ToDecryptedQueryString(Request.QueryString["args"]);
        //        if (args.ContainsKey("ClinicalRotationID"))
        //        {
        //            CurrentViewContext.ClinicalRotationID = Convert.ToInt32(args["ClinicalRotationID"]);
        //        }
        //        if (args.ContainsKey("SelectedTenantId"))
        //        {
        //            CurrentViewContext.SelectedTenantId = Convert.ToInt32(Request["SelectedTenantId"]);
        //        }
        //        if (args.ContainsKey("RequirementPackageSubscriptionID"))
        //        {
        //            CurrentViewContext.RequirementPackageSubscriptionID = Convert.ToInt32(Request["RequirementPackageSubscriptionID"]);
        //        }
        //    }
        //}

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
            tempFilePath += "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")\";

            if (!Directory.Exists(tempFilePath))
                Directory.CreateDirectory(tempFilePath);

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
                filePath += "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")\";

                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
            }

            StringBuilder docMessage = new StringBuilder();
            //Not allowed to upload document of size 0.
            StringBuilder corruptedFileMessage = new StringBuilder();
            Boolean isCorruptedFileUploaded = false;
            foreach (UploadedFile item in uploadControl.UploadedFiles)
            {
                String fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                //Save file
                String newTempFilePath = Path.Combine(tempFilePath, fileName);

                item.SaveAs(newTempFilePath);

                if (CurrentViewContext.ToSaveApplicantUploadedDocuments == null)
                {
                    CurrentViewContext.ToSaveApplicantUploadedDocuments = new List<ApplicantDocumentContract>();
                }
                ApplicantDocumentContract applicantDocument = new ApplicantDocumentContract();

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
                    String destFolder = filePath + "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")/";
                    String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, fileName, destFolder);
                    //Message for 0 size file upload
                    if (returnFilePath.IsNullOrEmpty())
                    {
                        isCorruptedFileUploaded = true;
                        corruptedFileMessage.Append("Your file " + item.FileName + " is not uploaded. \\n");
                        continue;
                    }
                    applicantDocument.DocumentPath = returnFilePath;
                }
                try
                {
                    if (!String.IsNullOrEmpty(newTempFilePath))
                        File.Delete(newTempFilePath);
                }
                catch (Exception) { }

                applicantDocument.FileName = item.FileName;
                applicantDocument.Size = item.ContentLength;
                applicantDocument.DocumentType = DocumentType.REQUIREMENT_FIELD_UPLOAD_DOCUMENT.GetStringValue();
                applicantDocument.DataEntryDocumentStatusCode = DataEntryDocumentStatus.COMPLETE.GetStringValue();

                CurrentViewContext.ToSaveApplicantUploadedDocuments.Add(applicantDocument);

            }
            if (CurrentViewContext.ToSaveApplicantUploadedDocuments != null && CurrentViewContext.ToSaveApplicantUploadedDocuments.Count > 0)
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
                    newFilePath = filePath + "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")/";
                }
                Presenter.AddApplicantUploadedDocuments(newFilePath);
                //Convert applicant uploaded document to PDF
                Presenter.CallParallelTaskPdfConversion();
            }

            //Restrict to upload duplicate document
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

        /// <summary>
        /// Used for handling dashboard postbacks.
        /// </summary>
        private void ResetControlsOnFalsePostback()
        {
            tlistRequirementData.IsItemInserted = false;
            tlistRequirementData.InsertIndexes.Clear();
        }

        /// <summary>
        /// Set the compliance status of the package based on the data
        /// </summary>
        /// <param name="status">Status to set</param>
        /// <param name="code">Look up code to devied the image for the status</param>
        private void SetRotationComplianceStatus(String code)
        {
            if (code.ToLower() == RequirementPackageStatus.REQUIREMENT_NOT_COMPLIANT.GetStringValue().ToLower())
            {
                imgPackageComplianceStatus.ImageUrl = AppConsts.PACKAGE_NON_COMPLIANCE_IMAGE_URL;
                lblComplianceStatus.ForeColor = System.Drawing.Color.Red;
                lblComplianceStatus.Text = "Not Compliant";
            }
            else
            {
                imgPackageComplianceStatus.ImageUrl = AppConsts.PACKAGE_COMPLIANCE_IMAGE_URL;
                lblComplianceStatus.ForeColor = System.Drawing.Color.Green;
                lblComplianceStatus.Text = "Compliant";
            }
        }

        /// <summary>
        /// Add data of the Fields into list, both add and update mode
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="attributeValue">Value of the attribute</param>
        private String GetHiddenFieldValue(object ctrl, Int32 reqItemFieldId, String controlId)
        {
            Control ucFields = (ctrl as RequirementAttributeControl) as Control;
            Control hiddenControl = FindControlRecursive(ucFields, controlId + "_" + reqItemFieldId);
            if (hiddenControl.IsNotNull())
            {
                return (hiddenControl as HiddenField).Value;
            }
            return String.Empty;
        }

        private String AddSignedDocumentToContract(String signedTempFilePath, Int32 reqFieldId, String signedFileName, String source = "")
        {
            String filePath = String.Empty;
            String fileName = String.Empty;
            Boolean aWSUseS3 = false;
            filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
            if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
            {
                aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
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
                filePath += "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")\";

                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
            }
            ApplicantDocumentContract signedApplicantDocument = new ApplicantDocumentContract();

            fileName = Guid.NewGuid().ToString() + Path.GetExtension(signedFileName);
            FileInfo fileInfo = new FileInfo(signedTempFilePath);
            var fileSize = fileInfo.Length;

            //Check whether use AWS S3, true if need to use
            if (aWSUseS3 == false)
            {
                //Move file to other location
                String destFilePath = Path.Combine(filePath, fileName);
                File.Copy(signedTempFilePath, destFilePath);
                signedApplicantDocument.DocumentPath = destFilePath;
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
                String destFolder = filePath + "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")/";
                String returnFilePath = objAmazonS3.SaveDocument(signedTempFilePath, fileName, destFolder);

                signedApplicantDocument.DocumentPath = returnFilePath;
            }
            //Start Bug Id-24881
            if (source == "btnAutoSubmit")
            {
                try
                {
                    DeleteTemporaryFile(signedTempFilePath);
                }
                catch (Exception) { }
            }
            //End Bug Id-24881

            signedApplicantDocument.FileName = signedFileName;
            signedApplicantDocument.Size = Convert.ToInt32(fileSize);
            signedApplicantDocument.DocumentType = DocumentType.REQUIREMENT_FIELD_VIEW_DOCUMENT.GetStringValue();
            signedApplicantDocument.DataEntryDocumentStatusCode = DataEntryDocumentStatus.COMPLETE.GetStringValue();

            if (CurrentViewContext.AppSignedDocumentDic.IsNull())
            {
                CurrentViewContext.AppSignedDocumentDic = new Dictionary<Int32, ApplicantDocumentContract>();
            }

            CurrentViewContext.AppSignedDocumentDic.Add(reqFieldId, signedApplicantDocument);
            if (CurrentViewContext.ToSaveApplicantUploadedDocuments == null)//UAT-3532
            {
                CurrentViewContext.ToSaveApplicantUploadedDocuments = new List<ApplicantDocumentContract>();
            }
            CurrentViewContext.ToSaveApplicantUploadedDocuments.Add(signedApplicantDocument); //UAT-3532

            return String.Empty;
        }

        private void DeleteTemporaryFile(String signedTempFilePath)
        {
            try
            {
                if (!String.IsNullOrEmpty(signedTempFilePath))
                {
                    File.Delete(signedTempFilePath);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Gets the data from the Query string and assign to properties
        /// </summary>
        private void CaptureQueryStringData()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT] != null)
            {
                args.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
            }
            if (args.ContainsKey(ProfileSharingQryString.SelectedTenantId))
            {
                CurrentViewContext.SelectedTenantId = Convert.ToInt32(args[ProfileSharingQryString.SelectedTenantId]);
            }
            if (args.ContainsKey(ProfileSharingQryString.ReqPkgSubscriptionId))
            {
                CurrentViewContext.RequirementPackageSubscriptionID = Convert.ToInt32(args[ProfileSharingQryString.ReqPkgSubscriptionId]);
            }
            if (args.ContainsKey(ProfileSharingQryString.RotationId))
            {
                CurrentViewContext.ClinicalRotationID = Convert.ToInt32(args[ProfileSharingQryString.RotationId]);
                hdnClinicalRotationID.Value = args[ProfileSharingQryString.RotationId];
            }
            if (args.ContainsKey(ProfileSharingQryString.Visibility))
            {
                Visiblity = Convert.ToString(args[ProfileSharingQryString.Visibility]).ToLower() == "true" ? true : false;
            }
            if (args.ContainsKey(ProfileSharingQryString.IsOpenInReadOnlyMode))
            {
                IsOpenInReadOnlyMode = Convert.ToBoolean(args[ProfileSharingQryString.IsOpenInReadOnlyMode]);
            }
            if (args.ContainsKey(ProfileSharingQryString.ControlUseType))
            {
                ControlUseType = Convert.ToString(args[ProfileSharingQryString.ControlUseType]);
                fsucCmdBar.Visible = Convert.ToString(args[ProfileSharingQryString.ControlUseType]) == AppConsts.SHARED_ROTATION_CONTROL_USE_TYPE_CODE ? true : false;
                //UAT-1523: Addition a notes box for each rotation for the student to input information
                divMainNotes.Visible = !fsucCmdBar.Visible;
            }//ApplicantId
            if (args.ContainsKey("ApplicantId"))
            {
                ApplicantID = args.ContainsKey("ApplicantId") ? Convert.ToInt32(args["ApplicantId"]) : AppConsts.NONE;
            }
            //UAT-3737
            if (args.ContainsKey("IsIntructorPreceptorPkg"))
            {
                IsIntructorPreceptorPkg = args.ContainsKey("IsIntructorPreceptorPkg") ? Convert.ToBoolean(args["IsIntructorPreceptorPkg"]) : false;
            }
        }

        /// <summary>
        /// Method to return manage invitation screen.
        /// </summary>
        private void ReturnToManageInvitation()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                     { "SelectedTenantID", Convert.ToString(CurrentViewContext.SelectedTenantId) },
                                                                    { "Child", ChildControls.SharedUserDashboard}
                                                                 };
            Response.Redirect(String.Format("~/ProfileSharing/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
        }

        //UAT-1523: Addition a notes box for each rotation for the student to input information
        private void LoadNotes()
        {
            txtNotes.Text = CurrentViewContext.RotationSubscriptionDetail.Notes;
        }

        private void ShowMessage(String strMessage, MessageType msgType)
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
                                                , "$page.showAlertMessage('" + strMessage.ToString() + "','" + msgClass + "',true);", true);
        }

        private Int32 GetReqItemID(TreeListEditFormItem editForm, WclComboBox ddItems, ref Boolean isItemSeries)
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
        private Int32 GetLatestReqItemID(WclComboBox ddItems, ref Boolean isItemSeries)
        {
            if (!ddItems.SelectedValue.IsNullOrEmpty() && ddItems.SelectedValue != AppConsts.ZERO)
            {
                isItemSeries = Convert.ToBoolean(ddItems.SelectedItem.Attributes["IsItemSeries"]);
                return !isItemSeries ? Convert.ToInt32(ddItems.SelectedValue) : GetItemIDFromDropDown(ddItems.SelectedValue);
            }
            return AppConsts.NONE;
            //else
            //{
            //    return Convert.ToInt32(ParseNodeId(Convert.ToString(editForm.ParentItem.GetDataKeyValue("NodeID"))));
            //}
        }

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
        #endregion
    }
    #endregion
}