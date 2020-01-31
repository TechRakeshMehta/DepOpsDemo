using System;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using System.Collections.Generic;
using System.Linq;
using INTSOF.Utils;
using System.Web;
using System.Web.UI;
using System.Web.Configuration;
using System.Data.Entity.Core.Objects;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.ComplianceOperations.Views;
using Business.RepoManagers;
using Telerik.Web.UI;
using System.Text;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class VerificationItemDataPanel : BaseUserControl, IVerificationItemDataPanelView
    {
        public delegate void CategoryChanged(object sender, EventArgs e);
        public event CategoryChanged CategoryNextClick;
        public event CategoryChanged CategoryPreviousClick;

        public delegate void PendingReviewCategoryChanged(object sender, EventArgs e);
        public event PendingReviewCategoryChanged PendingReviewCategoryNextClick;
        public event PendingReviewCategoryChanged PendingReviewCategoryPreviousClick;

        #region Variables

        #region Private Variables
        private Boolean? _isAdminLoggedIn = null;
        private VerificationItemDataPanelPresenter _presenter = new VerificationItemDataPanelPresenter();

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        public VerificationItemDataPanelPresenter Presenter
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

        public IVerificationItemDataPanelView CurrentViewContext
        {
            get { return this; }
        }

        public List<ApplicantItemVerificationData> lst
        {
            get;
            set;
        }

        public List<ApplicantDocuments> lstApplicantDocument
        {
            get;
            set;
        }

        public String viewType
        {
            get
            {
                if (ViewState["viewType"] != null)
                    return (String)ViewState["viewType"];
                return null;
            }
            set
            {
                ViewState["viewType"] = value;
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
                if (!ViewState["CurrentTenantId"].IsNullOrEmpty())
                    return (Int32)(ViewState["CurrentTenantId"]);
                else
                    return 0;
            }
            set
            {
                ViewState["CurrentTenantId"] = value;
            }
        }

        public Boolean IsEscalationRecords
        {
            get
            {
                if (ViewState["IsEscalationRecords"] != null)
                    return Convert.ToBoolean(ViewState["IsEscalationRecords"]);
                return false;
            }
            set
            {
                ViewState["IsEscalationRecords"] = value;
            }
        }

        public Int32 SelectedTenantId_Global
        {
            get
            {
                if (!ViewState["SelectedTenantId"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedTenantId"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedTenantId"].IsNullOrEmpty())
                    ViewState["SelectedTenantId"] = value;
            }
        }

        public Int32 SelectedCompliancePackageId_Global
        {
            get
            {
                if (!ViewState["SelectedCompliancePackageId"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedCompliancePackageId"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedCompliancePackageId"].IsNullOrEmpty())
                    ViewState["SelectedCompliancePackageId"] = value;
            }
        }

        public Int32 SelectedComplianceCategoryId_Global
        {
            get
            {
                if (!ViewState["SelectedComplianceCategoryId"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedComplianceCategoryId"]);
                else
                    return 0;
            }
            set
            {
                ViewState["SelectedComplianceCategoryId"] = value;
            }
        }

        public Int32 SelectedApplicantId_Global
        {
            get
            {
                if (!ViewState["SelectedApplicantId"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedApplicantId"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedApplicantId"].IsNullOrEmpty())
                    ViewState["SelectedApplicantId"] = value;
            }
        }

        public String CurrentLoggedInUserName_Global
        {
            get
            {
                if (!ViewState["CurrentLoggedInUserName"].IsNullOrEmpty())
                    return (String)(ViewState["CurrentLoggedInUserName"]);
                else
                    return String.Empty;
            }
            set
            {
                if (ViewState["CurrentLoggedInUserName"].IsNullOrEmpty())
                    ViewState["CurrentLoggedInUserName"] = value;
            }
        }

        public Int32 AssignedToVerUser
        {
            get
            {
                if (!ViewState["AssignedToVerUser"].IsNullOrEmpty())
                    return (Int32)(ViewState["AssignedToVerUser"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["AssignedToVerUser"].IsNullOrEmpty())
                    ViewState["AssignedToVerUser"] = value;
            }
        }

        public Int32 CurrentPackageSubscriptionID_Global
        {
            get { return (Int32)(ViewState["CurrentPackageSubscriptionID"]); }
            set { ViewState["CurrentPackageSubscriptionID"] = value; }
        }

        public Boolean IsException
        {
            get { return (Boolean)(ViewState["IsException_Data"]); }
            set { ViewState["IsException_Data"] = value; }
        }

        public WorkQueueType WorkQueue
        {
            get;
            set;
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
            get
            {
                if (!ViewState["IncludeIncompleteItems"].IsNullOrEmpty())
                    return (Boolean)(ViewState["IncludeIncompleteItems"]);
                else
                    return false;
            }
            set
            {
                if (ViewState["IncludeIncompleteItems"].IsNullOrEmpty())
                    ViewState["IncludeIncompleteItems"] = value;
            }
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
            get
            {
                if (!ViewState["ItemDataId"].IsNullOrEmpty())
                    return (Int32)(ViewState["ItemDataId"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["ItemDataId"].IsNullOrEmpty())
                    ViewState["ItemDataId"] = value;
            }
        }

        public Int32 PackageId_Global
        {
            get
            {
                if (!ViewState["PackageId"].IsNullOrEmpty())
                    return (Int32)(ViewState["PackageId"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["PackageId"].IsNullOrEmpty())
                    ViewState["PackageId"] = value;
            }
        }

        public Int32 CategoryId_Global
        {
            get
            {
                if (!ViewState["CategoryId"].IsNullOrEmpty())
                    return (Int32)(ViewState["CategoryId"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["CategoryId"].IsNullOrEmpty())
                    ViewState["CategoryId"] = value;
            }
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

        //UAT 537 Verification Details Screen "go to Next Pending for Review Category" should save data and button text change.
        public Boolean IsDataSavedSuccessfully
        {
            get;
            set;
        }

        //UAT-613
        public String UserId
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user != null)
                {
                    return Convert.ToString(user.UserId);
                }
                return String.Empty;
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

        #region UAT-2460

        /// <summary>
        ///get and  set package id .
        /// </summary>
        public Int32 PrevPackageSubscriptionID
        {
            get { return Convert.ToInt32(Session["PrevPackageSubscriptionID"] ?? "0"); }
            set { Session["PrevPackageSubscriptionID"] = value; }
        }
        public Int32 NextPackageSubscriptionID
        {
            get { return Convert.ToInt32(Session["NextPackageSubscriptionID"] ?? "0"); }
            set { Session["NextPackageSubscriptionID"] = value; }
        }

        public Boolean IsNextSubscriptionRequired
        {
            get { return Convert.ToBoolean(ViewState["IsNextSubscriptionRequired"] ?? false); }
            set { ViewState["IsNextSubscriptionRequired"] = value; }
        }
        public Boolean IsPreviousSubscriptionRequired
        {
            get { return Convert.ToBoolean(ViewState["IsPreviousSubscriptionRequired"] ?? false); }
            set { ViewState["IsPreviousSubscriptionRequired"] = value; }
        }

        public String SelectedArchiveStateCode
        {
            get
            {
                if (ViewState["SelectedArchiveStateCode"] != null)
                    return Convert.ToString(ViewState["SelectedArchiveStateCode"]);
                return String.Empty;
            }
            set
            {
                ViewState["SelectedArchiveStateCode"] = value;
            }
        }
        #endregion

        #region UAT-3744
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


        public List<RuleSet> RuleSetList
        {
            get;
            set;

        }

        public Boolean IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.HasValue ? _isAdminLoggedIn.Value : true;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }
        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            ShowDismissBlock();
            ShowDockUnDockAlert();
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                GetCategoryData();
                #region UAT:719 Check Exceptions turned off for a Category/Item

                Boolean isExceptionAlloed = Presenter.IsAllowExceptionOnCategory();
                if (isExceptionAlloed)
                {
                    imageExceptionOff.Visible = true;
                }

                //UAT-3599
                Boolean isSDEdisabled = Presenter.IsStudentDataEntryEnable();
                if (isSDEdisabled)
                {
                    imageSDEdisabled.Visible = true;
                }

                #endregion
                //  List<Int32?> lstItemDataId = CurrentViewContext.lst.Select(x =>x.ApplAttributeDataId).Distinct().ToList();
                SaveUpdateUtilityFeatureUsage();
                hdnExplanatoryNoteState.Value = Presenter.GetExplanatoryNoteState(UserId); //Set hidden field with explanatory note state value-UAT-613.
                hdnUserId.Value = UserId;

                if (CurrentViewContext.WorkQueue != WorkQueueType.AssignmentWorkQueue
                    && CurrentViewContext.WorkQueue != WorkQueueType.ExceptionAssignmentWorkQueue
                    && CurrentViewContext.WorkQueue != WorkQueueType.ComplianceSearch
                    && CurrentViewContext.WorkQueue != WorkQueueType.UserWorkQueue
                    && CurrentViewContext.WorkQueue != WorkQueueType.EsclationUserWorkQueue)
                {
                    btnNext.Text = "Save and Next Pending Category";
                    btnNext.ToolTip = "Click here to save and go to the next Category that is in Pending for Review status for this student";
                    btnPrevious.Text = "Save and Previous Category";
                    btnPrevious.ToolTip = "Click here to save and go to the previous category for this Student";
                }
            }
            Presenter.OnViewLoaded();

            viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            Dictionary<String, String> args = new Dictionary<string, string>();
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey("WorkQueueType"))
            {
                CurrentViewContext.WorkQueue = (WorkQueueType)Enum.Parse(typeof(WorkQueueType), args["WorkQueueType"].ToString(), true);
            }

            if (args.ContainsKey("UserMessage") && !this.IsPostBack)
            {
                String _message = Convert.ToString(args["UserMessage"]);
                if (!String.IsNullOrEmpty(_message))
                {
                    if (_message.LastIndexOf(',') > 0) // Errors are received as CSV from the DataLoader.cs section
                    {
                        lblMessage.Text = _message.Substring(0, _message.LastIndexOf(','));
                        lblMessage.CssClass = "error";
                    }
                    else // No "," in the success message received from the DataLoader.cs section
                    {
                        lblMessage.Text = _message;
                        lblMessage.CssClass = "sucs";
                    }
                }
            }
            else
            {
                lblMessage.Text = String.Empty;
                //Presenter.GetApplicantDocuments();
                LoadItems();
                ManageNextPrevious();
                ManageNextPreviousNavigations();
                hiddenuploader.MaxFileSize = Convert.ToInt32(WebConfigurationManager.AppSettings[AppConsts.MAXIMUM_FILE_SIZE]);
                // to show the track about Last Updated
                //Int32 CategoryID = CurrentViewContext.lst.Select(x => x.ComplianceCatId).Distinct().FirstOrDefault();
                //List<CatUpdatedByLatestInfo> latestInfo = Presenter.GetCatUpdatedByLatestInfo(CategoryID);
                //if (latestInfo.IsNotNull() && latestInfo.Count() > 0 && latestInfo.FirstOrDefault().IsApplicant == false)
                //{
                //    lbllastupdatedby.Text = latestInfo.FirstOrDefault().ApplicantFirstName.ToString() + " " + latestInfo.FirstOrDefault().ApplicantLastname + " on " + latestInfo.FirstOrDefault().ModifiedDate;
                //}
                //else
                //{
                //    lbllastupdatedby.Text = "Never";
                //}

            }
        }

        private void GetCategoryData()
        {
            ComplianceCategory cmpCategory = Presenter.GetCategoryData();


            litAdminExplanation.Text = cmpCategory.Description;
            //litApplicantExplanation.Text = cmpCategory.ExpNotes + GenerateExplanatoryNotesLink(cmpCategory.SampleDocFormURL);

            litApplicantExplanation.Text = cmpCategory.ExpNotes + GenerateExplanatoryNotesLinkList(cmpCategory);
            lblSelectedCategoryName.Text = (cmpCategory.CategoryName).HtmlEncode();
            if (IsAdminLoggedIn == true)
            {
                String ruleAppliedDate = Presenter.GetCategoryRuleExecutionDate(); //UAT-3566
                if (!ruleAppliedDate.IsNullOrEmpty())
                    spnRuleAppliedDate.Text = "(" + ruleAppliedDate + ")";
            }

            //UAT-1519: Add the ability to include the Category Explanatory note in the Item rejected email notification.
            hdnCatExplanatoryNotes.Value = cmpCategory.ExpNotes;
            hdnCatMoreInfoURL.Value = DocumentUrlLinkListForRejectedEmail(cmpCategory);

        }

        private String GenerateExplanatoryNotesLinkList(ComplianceCategory cmpCategory)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var docUrl in cmpCategory.ComplianceCategoryDocUrls)
            {
                if (!docUrl.SampleDocFormURL.IsNullOrEmpty())
                {
                    String linkText = docUrl.SampleDocFormURLLabel.IsNullOrEmpty() ? AppConsts.CATEGORY_EXP_NOTES_LINK_TEXT : docUrl.SampleDocFormURLLabel;
                    sb.Append("<br /><a href=\"" + docUrl.SampleDocFormURL + "\" onclick=\"\" target=\"_blank\");'>" + linkText + "</a>");
                }

            }

            return sb.ToString();
        }

        private String DocumentUrlLinkListForRejectedEmail(ComplianceCategory cmpCategory)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var docUrl in cmpCategory.ComplianceCategoryDocUrls)
            {
                if (!docUrl.SampleDocFormURL.IsNullOrEmpty())
                    sb.Append("<br /><a href=\"" + docUrl.SampleDocFormURL + "\" onclick=\"\" target=\"_blank\");'>" + docUrl.SampleDocFormURL + "</a>");
            }

            return sb.ToString();
        }

        private void ManageNextPreviousNavigations()
        {
            //UAT-3293
            if (this.WorkQueue == WorkQueueType.ReconciliationQueue || this.WorkQueue == WorkQueueType.ReconciliationDetail)
            {
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;

                imgNextCategory.Enabled = false;
                imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";

                imgPreviousCategory.Enabled = false;
                imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
            }
            String prevQueryString = ComplianceOperationsVerifications.GetVerificationDetailScreenQueryString(this.SelectedTenantId_Global,
                  this.ItemDataId_Global,
                  this.WorkQueue,
                  this.PackageId_Global,
                  this.CategoryId_Global,
                  this.UserGroupId,
                  this.IncludeIncompleteItems_Global,
                  this.CurrentPackageSubscriptionID_Global,
                  this.PreviousComplianceCategoryId_Global,
                  ChildControls.VerificationDetailsNew,
                  this.viewType,
                  this.ShowOnlyRushOrders, IsException, SelectedApplicantId_Global, VerificationDetailActionType.CategoryPreviousNextTop.GetStringValue(), IsEscalationRecords, this.SelectedArchiveStateCode, this.SelectedComplianceItemReconciliationDataID);

            String prevHrefValue = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", viewType, prevQueryString.ToString());
            if (imgPreviousCategory.Enabled)
                lnkBtnPreviousCategory.HRef = prevHrefValue;
            else
                lnkBtnPreviousCategory.HRef = String.Empty;

            //if (btnPrevCat.Enabled)
            //    btnPrevCat.Attributes.Add("Url", prevHrefValue);
            //else
            //    btnPrevCat.Attributes.Add("Url", String.Empty);

            if (!IsPreviousSubscriptionRequired)
            {
                hdnBtnPrevious.Value = String.Format("Default.aspx?ucid={0}&args={1}", viewType, prevQueryString.ToString()); ;
            }

            //Image Next Link Click
            String nextQueryString = ComplianceOperationsVerifications.GetVerificationDetailScreenQueryString(this.SelectedTenantId_Global,
                  this.ItemDataId_Global,
                  this.WorkQueue,
                  this.PackageId_Global,
                  this.CategoryId_Global,
                  this.UserGroupId,
                  this.IncludeIncompleteItems_Global,
                  this.CurrentPackageSubscriptionID_Global,
                  this.NextComplianceCategoryId_Global,
                  ChildControls.VerificationDetailsNew,
                  this.viewType,
                  this.ShowOnlyRushOrders, IsException, SelectedApplicantId_Global, VerificationDetailActionType.CategoryPreviousNextTop.GetStringValue(), IsEscalationRecords, this.SelectedArchiveStateCode, this.SelectedComplianceItemReconciliationDataID);

            String nextHrefValue = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", viewType, nextQueryString.ToString());

            if (imgNextCategory.Enabled)
                lnkBtnNextCategory.HRef = nextHrefValue;
            else
                lnkBtnNextCategory.HRef = String.Empty;

            //if (btnNextCat.Enabled)
            //    btnNextCat.Attributes.Add("Url", nextHrefValue);
            //else
            //    btnNextCat.Attributes.Add("Url", String.Empty);

            /*lnkBtnNextCategory.HRef = nextHrefValue;*/
            //Next Button at bottom Click
            if (!IsNextSubscriptionRequired)
            {
                hdnBtnNext.Value = String.Format("Default.aspx?ucid={0}&args={1}", viewType, nextQueryString.ToString()); ;
            }
        }

        private void ManageNextPrevious()
        {
            btnNext.Enabled = (this.NextComplianceCategoryId_Global > 0);
            btnPrevious.Enabled = (this.PreviousComplianceCategoryId_Global > 0);

            imgNextCategory.Enabled = (this.NextComplianceCategoryId_Global > 0);
            imgNextCategory.ImageUrl = (this.NextComplianceCategoryId_Global > 0) ? "~/Resources/Mod/Compliance/images/right-arrow-Blue.png" :
              "~/Resources/Mod/Compliance/images/right-arrowd.png";

            imgPreviousCategory.Enabled = (this.PreviousComplianceCategoryId_Global > 0);
            imgPreviousCategory.ImageUrl = (this.PreviousComplianceCategoryId_Global > 0) ? "~/Resources/Mod/Compliance/images/left-arrow-blue.png" :
              "~/Resources/Mod/Compliance/images/left-arrowd.png";

            //btnNextCat.Enabled = (this.NextComplianceCategoryId_Global > 0);
            ////btnNextCat.Image.ImageUrl = (this.NextComplianceCategoryId_Global > 0) 
            ////                            ? "~/Resources/Mod/Compliance/images/right-arrow.png" 
            ////                            : "~/Resources/Mod/Compliance/images/right-arrowd.png";

            //btnPrevCat.Enabled = (this.PreviousComplianceCategoryId_Global > 0);
            ////btnPrevCat.Image.ImageUrl =  (this.PreviousComplianceCategoryId_Global > 0) 
            ////                            ? "~/Resources/Mod/Compliance/images/left-arrow.png" 
            ////                            : "~/Resources/Mod/Compliance/images/left-arrowd.png";

            //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
            if (btnNext.Enabled && !this.IsFullPermissionForVerification)
            {
                btnNext.Enabled = false;
            }
            //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
            if (btnPrevious.Enabled && !this.IsFullPermissionForVerification)
            {
                btnPrevious.Enabled = false;
            }
            SetNextPreviousSubscription();
        }

        private void ucVerificationItemDataPanel_DataSavedClick(object sender, EventArgs e)
        {
            //LoadItems();
        }

        protected void btnSaveCategoryNotes_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.UpdateApplicantCategoryNotes(txtCategoryNotes.Text, Convert.ToInt32(hdfApplicantComplianceCategoryId.Value));
                lblMessage.Text = "Category notes updated successfully.";
                lblMessage.CssClass = "error";
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                lblMessage.CssClass = "error";
                lblMessage.Text = ex.ToString();
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                lblMessage.CssClass = "error";
                lblMessage.Text = ex.ToString();
            }
        }

        protected void imgPreviousCategory_Click(object sender, EventArgs e)
        {
            if (this.PreviousComplianceCategoryId_Global > AppConsts.NONE)
            {
                this.SelectedComplianceCategoryId_Global = this.PreviousComplianceCategoryId_Global;

                ComplianceOperationsVerifications.RedirectToVerificationDetailScreen(this.SelectedTenantId_Global,
                             this.ItemDataId_Global,
                             this.WorkQueue,
                             this.PackageId_Global,
                             this.CategoryId_Global,
                             this.UserGroupId,
                             this.IncludeIncompleteItems_Global,
                             this.CurrentPackageSubscriptionID_Global,
                             this.SelectedComplianceCategoryId_Global,
                             ChildControls.VerificationDetailsNew,
                             this.viewType, this.ShowOnlyRushOrders, IsException, IsEscalationRecords, this.SelectedComplianceItemReconciliationDataID);

            }
        }

        protected void imgNextCategory_Click(object sender, EventArgs e)
        {
            if (this.NextComplianceCategoryId_Global > AppConsts.NONE)
            {
                this.SelectedComplianceCategoryId_Global = this.NextComplianceCategoryId_Global;

                ComplianceOperationsVerifications.RedirectToVerificationDetailScreen(this.SelectedTenantId_Global,
                             this.ItemDataId_Global,
                             this.WorkQueue,
                             this.PackageId_Global,
                             this.CategoryId_Global,
                             this.UserGroupId,
                             this.IncludeIncompleteItems_Global,
                             this.CurrentPackageSubscriptionID_Global,
                             this.SelectedComplianceCategoryId_Global,
                             ChildControls.VerificationDetailsNew,
                             this.viewType, this.ShowOnlyRushOrders, IsException, IsEscalationRecords, this.SelectedComplianceItemReconciliationDataID);

            }
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            if (this.PreviousComplianceCategoryId_Global > AppConsts.NONE)
            {
                this.SelectedComplianceCategoryId_Global = this.PreviousComplianceCategoryId_Global;

                ComplianceOperationsVerifications.RedirectToVerificationDetailScreen(this.SelectedTenantId_Global,
                             this.ItemDataId_Global,
                             this.WorkQueue,
                             this.PackageId_Global,
                             this.CategoryId_Global,
                             this.UserGroupId,
                             this.IncludeIncompleteItems_Global,
                             this.CurrentPackageSubscriptionID_Global,
                             this.SelectedComplianceCategoryId_Global,
                             ChildControls.VerificationDetailsNew,
                             this.viewType, this.ShowOnlyRushOrders, IsException, IsEscalationRecords, this.SelectedComplianceItemReconciliationDataID);

            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            //UAT 537
            SaveCurrentCatagoryData(false); //false is used to prevent entry in the UtilityFeatureUsage table 
            if (IsDataSavedSuccessfully)
            {
                if (IsNextSubscriptionRequired)
                {
                    Response.Redirect(hdnNextSubscriptionURL.Value);
                }
                else
                {
                    Response.Redirect(hdnBtnNext.Value);
                }
            }


            //if (this.NextComplianceCategoryId_Global > AppConsts.NONE)
            //{
            //    this.SelectedComplianceCategoryId_Global = this.NextComplianceCategoryId_Global;
            //    ComplianceOperationsVerifications.RedirectToVerificationDetailScreen(this.SelectedTenantId_Global,
            //                 this.ItemDataId_Global,
            //                 this.WorkQueue,
            //                 this.PackageId_Global,
            //                 this.CategoryId_Global,
            //                 this.UserGroupId,
            //                 this.IncludeIncompleteItems_Global,
            //                 this.CurrentPackageSubscriptionID_Global,
            //                 this.SelectedComplianceCategoryId_Global,
            //                 ChildControls.VerificationDetailsNew,
            //                 this.viewType, this.ShowOnlyRushOrders, IsException);
            //}
        }

        #endregion

        #region Methods

        #region Private Methods

        #endregion

        #region Public Methods

        protected void Save(object sender, EventArgs e)
        {
            SaveCurrentCatagoryData(true);
        }

        private void SaveCurrentCatagoryData(Boolean isUpdateUtilityFeatureUsage)
        {
            Int32 _controlCount = pnlLoader.Controls.Count;

            for (int i = 0; i < _controlCount; i++)
            {
                if (pnlLoader.Controls[i] is ItemDataLoader)
                {
                    ItemDataLoader _itemDataLoader = pnlLoader.Controls[i] as ItemDataLoader;
                    //UAT-1519: Add the ability to include the Category Explanatory note in the Item rejected email notification.
                    //Added hdnCatExplanatoryNote parameter
                    _itemDataLoader.Save(hdnCatExplanatoryNotes.Value, hdnCatMoreInfoURL.Value);
                    //UAT 537 Verification Details Screen "go to Next Pending for Review Category" should save data and button text change.
                    IsDataSavedSuccessfully = _itemDataLoader.IsDataSavedSuccessfully;
                    if (IsDataSavedSuccessfully)
                    {
                        _itemDataLoader.LoadVerificationDetailsUnassignedDocumentConrol();
                    }
                }
            }
            if (isUpdateUtilityFeatureUsage)
            {
                Presenter.SaveUpdateUtilityFeatureUsage(Convert.ToBoolean(hdnIsSaveClicked.Value), UtilityFeatures.Ctrl_Save.GetStringValue());
                hdnIsSaveClicked.Value = true.ToString();
            }

            ShowDismissBlock(); //Set the visibility of Alert Notofication Block

            // to set focus on "ucVerificationItemDataPanel" middle pane
            String key = "CategoryPanel";
            if (HttpContext.Current.Items[key].IsNotNull())
            {
                INTERSOFT.WEB.UI.WebControls.WclSplitter categoryPanel = (INTERSOFT.WEB.UI.WebControls.WclSplitter)HttpContext.Current.Items[key];
                if (!categoryPanel.IsNullOrEmpty())
                    Page.FindControl(categoryPanel.UniqueID).Focus();
                else
                    this.Parent.Parent.Focus();
            }
        }

        /// <summary>
        /// Method for Hiding and Showing of the Alert Notification
        /// </summary>
        private void ShowDismissBlock()
        {
            if (IsFullPermissionForVerification)
            {
                if (Session["RemindLater"].IsNull())
                {
                    Entity.UtilityFeatureUsage utilityFeatureUsage = ComplianceDataManager.GetIgnoreAlertStatus(CurrentLoggedInUserId, UtilityFeatures.Ctrl_Save.GetStringValue());
                    if (utilityFeatureUsage.IsNotNull() && utilityFeatureUsage.UFU_IgnoreAlert == false && utilityFeatureUsage.UFU_Count > 4)
                    {
                        divCtrlSave.Visible = true;
                    }
                    else
                    {
                        divCtrlSave.Visible = false;
                    }
                }
                else if (Session["RemindLater"].IsNotNull() && Session["RemindLater"].ToString() == "True")
                {
                    divCtrlSave.Visible = false;
                }
            }
            else
            {
                divCtrlSave.Visible = false;
            }
        }

        public void LoadItems()
        {
            Control itemLoader;
            Presenter.GetComplianceItemData();
            pnlLoader.Controls.Clear();
            // List<Int32?> lstItemDataId = CurrentViewContext.lst.Where(x=>x.ApplAttributeDataId.HasValue).Select(x => x.ApplAttributeDataId ).Distinct().ToList();
            //stItemDataId.Remove(null);
            itemLoader = Page.LoadControl("~/ComplianceOperations/UserControl/ItemDataLoader.ascx");

            (itemLoader as ItemDataLoader).lst = CurrentViewContext.lst;
            (itemLoader as ItemDataLoader).SelectedCompliancePackageId_Global = CurrentViewContext.SelectedCompliancePackageId_Global;
            (itemLoader as ItemDataLoader).SelectedComplianceCategoryId_Global = CurrentViewContext.SelectedComplianceCategoryId_Global;
            (itemLoader as ItemDataLoader).SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
            (itemLoader as ItemDataLoader).CurrentLoggedInUserName_Global = CurrentViewContext.CurrentLoggedInUserName_Global;
            (itemLoader as ItemDataLoader).NextComplianceCategoryId_Global = CurrentViewContext.NextComplianceCategoryId_Global;
            (itemLoader as ItemDataLoader).PreviousComplianceCategoryId_Global = CurrentViewContext.PreviousComplianceCategoryId_Global;
            (itemLoader as ItemDataLoader).CurrentTenantId_Global = CurrentViewContext.CurrentTenantId_Global;
            (itemLoader as ItemDataLoader).CurrentPackageSubscriptionID_Global = CurrentViewContext.CurrentPackageSubscriptionID_Global;
            (itemLoader as ItemDataLoader).IncludeIncompleteItems_Global = CurrentViewContext.IncludeIncompleteItems_Global;
            (itemLoader as ItemDataLoader).ItemDataId_Global = CurrentViewContext.ItemDataId_Global;
            (itemLoader as ItemDataLoader).SelectedApplicantId_Global = CurrentViewContext.SelectedApplicantId_Global;
            (itemLoader as ItemDataLoader).WorkQueue = CurrentViewContext.WorkQueue;
            (itemLoader as ItemDataLoader).PackageId_Global = CurrentViewContext.PackageId_Global;
            (itemLoader as ItemDataLoader).CategoryId_Global = CurrentViewContext.CategoryId_Global;
            (itemLoader as ItemDataLoader).ShowOnlyRushOrders = CurrentViewContext.ShowOnlyRushOrders;
            (itemLoader as ItemDataLoader).IsException = IsException;
            (itemLoader as ItemDataLoader).UserGroupId = CurrentViewContext.UserGroupId;
            (itemLoader as ItemDataLoader).lstApplicantDocument = CurrentViewContext.lstApplicantDocument;
            (itemLoader as ItemDataLoader).btnSave = this.btnSave;
            (itemLoader as ItemDataLoader).LoggedInUserInitials_Global = CurrentViewContext.LoggedInUserInitials_Global;
            (itemLoader as ItemDataLoader).IsFullPermissionForVerification = this.IsFullPermissionForVerification;
            (itemLoader as ItemDataLoader).SelectedComplianceItemReconciliationDataID = this.SelectedComplianceItemReconciliationDataID;//UAT-3744

            if (lst.IsNotNull() && lst.Count() > 0)
            {
                if (lst[0].CatExceptionStatusCode == "AAAD")
                {
                    litCategoryStatus.Text = "Approved Override";
                }
                else
                {
                    litCategoryStatus.Text = lst[0].CatComplianceStatus;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(lst[0].ApplicantCompCatId)) && !IsPostBack)
                {
                    txtCategoryNotes.Text = lst[0].ApplicantCatNotes;
                    hdfApplicantComplianceCategoryId.Value = Convert.ToString(lst[0].ApplicantCompCatId);
                }
                //pnlCategoryLevel.Visible = !lst.Where(data => data.ExceptionReason.IsNullOrEmpty()).Any();
                //cbarCategory.Visible = pnlCategoryLevel.Visible;
            }

            cbarCategory.Visible = pnlCategoryLevel.Visible = false;

            (itemLoader as ItemDataLoader).DataSavedClick -= new ItemDataLoader.DataSaved(ucVerificationItemDataPanel_DataSavedClick);
            (itemLoader as ItemDataLoader).DataSavedClick += new ItemDataLoader.DataSaved(ucVerificationItemDataPanel_DataSavedClick);
            pnlLoader.Controls.Add(itemLoader);
        }

        /// <summary>
        /// Method for Hiding and Showing of the Alert Notification for dock undock
        /// </summary>
        private void ShowDockUnDockAlert()
        {
            //hdnCurrentLoggedInUserId.Value = CurrentLoggedInUserId.ToString();
            if (Session["RemindLaterDockUnDock"].IsNull())
            {
                Entity.UtilityFeatureUsage utilityFeatureUsage = Presenter.GetIgnoreAlertStatus();
                if (utilityFeatureUsage.IsNotNull() && utilityFeatureUsage.UFU_IgnoreAlert == false && utilityFeatureUsage.UFU_Count > 4)
                {
                    divDockUnDock.Visible = true;
                }
                else
                {
                    divDockUnDock.Visible = false;
                }
            }
            else if (Session["RemindLaterDockUnDock"].IsNotNull() && Session["RemindLaterDockUnDock"].ToString() == "True")
            {
                divDockUnDock.Visible = false;
            }
        }

        private void SaveUpdateUtilityFeatureUsage()
        {
            if (HttpContext.Current.Session["IsNewSession"].IsNull())
            {
                if (Presenter.SaveUpdateUtilityFeatureUsage(true, UtilityFeatures.Dock_UnDock.GetStringValue()))
                {
                    HttpContext.Current.Session["IsNewSession"] = false;
                    ShowDockUnDockAlert();
                }
            }
        }

        #endregion



        #endregion

        /// <summary>
        /// NOT IN USE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrevCat_Click(object sender, EventArgs e)
        {
            //String url = btnPrevCat.Attributes["Url"];
            //if (!url.IsNullOrEmpty())
            //{
            //    Response.Redirect(url);
            //}
        }

        /// <summary>
        /// NOT IN USE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNextCat_Click(object sender, EventArgs e)
        {
            //String url = btnNextCat.Attributes["Url"];
            //if (!url.IsNullOrEmpty())
            //{
            //    Response.Redirect(url);
            //}
        }

        /// <summary>
        /// NOT IN USE - Event of "Previous category" button at bottom of the Middle Panel. Fix for UAT-1767.
        /// Again in USE -UAT-2075, Also handling fix of UAT-1767.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPreviousCat_Click(object sender, EventArgs e)
        {
            SaveCurrentCatagoryData(false); //false is used to prevent entry in the UtilityFeatureUsage table 
            if (IsDataSavedSuccessfully)
            {
                if (IsPreviousSubscriptionRequired)
                {
                    Response.Redirect(hdnPreviousSubscriptionURL.Value);
                }
                else
                {
                    //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "btnPreviousClick();", true);
                    Response.Redirect(hdnBtnPrevious.Value);
                }
            }
        }

        private void SetNextPreviousSubscription()
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "GetNextPreviousURL();", true);

            if (!btnNext.Enabled && NextPackageSubscriptionID > AppConsts.NONE)
            {
                btnNext.Enabled = true;
                IsNextSubscriptionRequired = true;
            }
            if (!btnPrevious.Enabled && PrevPackageSubscriptionID > AppConsts.NONE)
            {
                btnPrevious.Enabled = true;
                IsPreviousSubscriptionRequired = true;
            }
        }
    }
}

