#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using System.Collections;

#endregion

#region Application Specific

using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.CommonControls.Views;
using INTSOF.Utils.Consts;
using INTERSOFT.WEB.UI.WebControls;
//using Microsoft.Practices.CompositeWeb.Web.UI;
using Entity.ClientEntity;
using CoreWeb.Shell;
using System.Xml.Serialization;
using System.IO;
using System.Web;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion


namespace CoreWeb.ComplianceOperations.Views
{
    public partial class VerificationDetails : BaseUserControl, IVerificationDetailsView
    {
        private VerificationDetailsPresenter _presenter = new VerificationDetailsPresenter();

        #region Variables

        #region Private Variables

        private Int32 _tenantId;
        private Int32 _selectedTenantId;
        private String _viewType;
        delegate void ReLoadDataItemPanel(Dictionary<string, string> args);
        #endregion

        #endregion

        #region Properties

        public VerificationDetailsPresenter Presenter
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

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>

        public IVerificationDetailsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }



        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
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

        public Boolean IsEscalationRecords
        {
            get
            {
                if (Session["IsEscalationRecords"] != null)
                    return Convert.ToBoolean(ViewState["IsEscalationRecords"]);
                return false;
            }
            set
            {
                ViewState["IsEscalationRecords"] = value;
            }
        }

        public String WorkQueuePath
        {
            get
            {
                if (this.WorkQueue == WorkQueueType.AssignmentWorkQueue && !IsException)
                    return ChildControls.VerificationQueue;
                else if (this.WorkQueue == WorkQueueType.AssignmentWorkQueue && IsException)
                    return ChildControls.ExceptionVerificationQueue;
                else if (this.WorkQueue == WorkQueueType.UserWorkQueue && IsException)
                    return ChildControls.ExceptionUserWorkQueue;
                else if (this.WorkQueue == WorkQueueType.DataItemSearch)
                    return ChildControls.DatatItemSearch;
                else if (this.WorkQueue == WorkQueueType.AssigneeDataItemSearch)
                    return ChildControls.AssigneeDatatItemSearch;
                else if (this.WorkQueue == WorkQueueType.ComplianceSearch)
                    return ChildControls.ComplianceSearchControl;
                else if (this.WorkQueue == WorkQueueType.EsclationAssignmentWorkQueue && !IsException)
                    return ChildControls.EsclationAssignmentQueue;
                else if (this.WorkQueue == WorkQueueType.EsclationAssignmentWorkQueue && IsException)
                    return ChildControls.EsclationExceptionAssignmentQueue;
                else if (this.WorkQueue == WorkQueueType.EsclationUserWorkQueue && !IsException)
                    return ChildControls.EsclationUserQueue;
                else if (this.WorkQueue == WorkQueueType.EsclationUserWorkQueue && IsException)
                    return ChildControls.EsclationExceptionUserQueue;
                else if (this.WorkQueue == WorkQueueType.ReconciliationQueue)
                    return ChildControls.ReconciliationQueue;
                return ChildControls.UserWorkQueue;
            }
        }


        public string UIInputException { get; set; }



        /// <summary>
        /// set package id for return back to queue.
        /// </summary>
        public Int32 PackageId
        {
            get
            {
                return Convert.ToInt32(ViewState["PackageId"] ?? "0");
            }
            set
            {
                ViewState["PackageId"] = value;
            }
        }

        /// <summary>
        /// set Category id for return back to queue.
        /// </summary>
        public Int32 CategoryId
        {
            get
            {
                return Convert.ToInt32(ViewState["CategoryId"] ?? "0");
            }
            set
            {
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


        public Int32 ReviewerUserId
        {
            get
            {
                if (ViewState["ReviewerUserId"] != null)
                    return (Int32)(ViewState["ReviewerUserId"]);
                return 0;
            }
            set
            {
                ViewState["ReviewerUserId"] = value;
            }
        }

        /// <summary>
        /// set IncludeIncompleteItems for return back to queue.
        /// </summary>
        public Boolean IncludeIncompleteItems
        {
            get
            {
                if (!ViewState["IncludeIncompleteItems"].IsNull())
                {
                    return (Boolean)ViewState["IncludeIncompleteItems"];
                }
                return false;
            }
            set
            {
                ViewState["IncludeIncompleteItems"] = value;
            }
        }

        /// <summary>
        /// set ShowOnlyRushOrders for return back to queue.
        /// </summary>
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




        public String ApplicantFirstName
        {
            get
            {
                if (!ViewState["ApplicantFirstName"].IsNull())
                {
                    return (String)ViewState["ApplicantFirstName"];
                }
                return String.Empty;
            }
            set
            {
                ViewState["ApplicantFirstName"] = value;
            }
        }

        public String ApplicantLastName
        {
            get
            {
                if (!ViewState["ApplicantLastName"].IsNull())
                {
                    return (String)ViewState["ApplicantLastName"];
                }
                return String.Empty;
            }
            set
            {
                ViewState["ApplicantLastName"] = value;
            }
        }

        public DateTime? DateOfBirth
        {
            get
            {
                if (!ViewState["DateOfBirth"].IsNull())
                {
                    return (DateTime)ViewState["DateOfBirth"];
                }
                return null;
            }
            set
            {
                ViewState["DateOfBirth"] = value;
            }
        }

        public String ItemLabel
        {
            get
            {
                if (!ViewState["ItemLabel"].IsNull())
                {
                    return (String)ViewState["ItemLabel"];
                }
                return String.Empty;
            }
            set
            {
                ViewState["ItemLabel"] = value;
            }
        }

        //UAT-5063
        public DateTime? SubmissionDateFrom
        {
            get
            {
                if (!ViewState["SubmissionDateFrom"].IsNull())
                {
                    return (DateTime)ViewState["SubmissionDateFrom"];
                }
                return null;
            }
            set
            {
                ViewState["SubmissionDateFrom"] = value;
            }
        }

        public DateTime? SubmissionDateTo
        {
            get
            {
                if (!ViewState["SubmissionDateTo"].IsNull())
                {
                    return (DateTime)ViewState["SubmissionDateTo"];
                }
                return null;
            }
            set
            {
                ViewState["SubmissionDateTo"] = value;
            }
        }

        public String AssignedUser
        {
            get
            {
                if (!ViewState["AssignedUser"].IsNull())
                {
                    return (String)ViewState["AssignedUser"];
                }
                return String.Empty;
            }
            set
            {
                ViewState["AssignedUser"] = value;
            }
        }

        public Int32 ProgramId
        {
            get
            {
                if (!ViewState["ProgramId"].IsNull())
                {
                    return (Int32)ViewState["ProgramId"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["ProgramId"] = value;
            }
        }

        public Entity.OrganizationUser OrganizationData { get; set; }

        public String SelectedItemComplianceStatusId
        {
            get
            {
                if (!ViewState["SelectedItemComplianceStatusId"].IsNull())
                {
                    return (String)ViewState["SelectedItemComplianceStatusId"];
                }
                return String.Empty;
            }
            set
            {
                ViewState["SelectedItemComplianceStatusId"] = value;
            }
        }

        public String ActionType
        {
            get;
            set;
        }
        public String SelectedArchiveStateCode
        {
            get
            {
                if (!Session["SelectedArchiveStateCode"].IsNull())
                {
                    return (String)Session["SelectedArchiveStateCode"];
                }
                return String.Empty;
            }
            set
            {
                Session["SelectedArchiveStateCode"] = value;
            }
        }

        public String UserID
        {
            get
            {
                if (!ViewState["UserId"].IsNull())
                {
                    return Convert.ToString(ViewState["UserId"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["UserId"] = value;
            }
        }

        #endregion

        #region "Global Properties"

        public Int32 TenantId_Global
        {
            get { return (Int32)(ViewState["TenantId"]); }
            set { ViewState["TenantId"] = value; }
        }

        public Boolean IsException
        {
            get { return Convert.ToBoolean(ViewState["IsException_record"]); }
            set { ViewState["IsException_record"] = value; }
        }

        /// <summary>
        ///get and  set Applicant id 
        /// </summary>
        public Int32 CurrentApplicantId_Global
        {
            get
            {
                try
                {
                  
                    return (Int32)(ViewState["CurrentApplicantId"]);
                }
                catch (SysXException ex)
                {
                    String ErrorMessage = "GET EVENT Tenant ID: -" + CurrentViewContext.CurrentTenantId_Global + ",Selected Applicant ID:-" + CurrentViewContext.CurrentApplicantId_Global.ToString() + "Subscription ID:-, " +
                    CurrentViewContext.SelectedPackageSubscriptionID_Global + " || "+ex.Message;
                    ex.Source = ErrorMessage;
                    base.LogError(ErrorMessage, ex);
                    base.ShowErrorMessage(""+ErrorMessage);
                    throw ex;
                }
                catch (System.Exception ex)
                {
                    //base.LogError(ex);
                    String ErrorMessage = "GET EVENT Tenant ID: -" + CurrentViewContext.CurrentTenantId_Global + ",Selected Applicant ID:-" + CurrentViewContext.CurrentApplicantId_Global.ToString() + "Subscription ID:-, " +
                    CurrentViewContext.SelectedPackageSubscriptionID_Global +" || " + ex.Message;
                    ex.Source = ErrorMessage;
                    base.LogError(ErrorMessage, ex);
                    base.ShowErrorMessage(ex.Message);
                    throw ex; 
                }
               
            }
            set
            {
                try
                {
                    ViewState["CurrentApplicantId"] = value;
                }
                catch (SysXException ex)
                {
                    String ErrorMessage = "SET EVENT Tenant ID: -" + CurrentViewContext.CurrentTenantId_Global + ",Selected Applicant ID:-" + CurrentViewContext.CurrentApplicantId_Global.ToString() + "Subscription ID:-, " +
                    CurrentViewContext.SelectedPackageSubscriptionID_Global + " || " + ex.Message;
                    ex.Source = ErrorMessage;
                    base.LogError(ErrorMessage, ex);
                    base.ShowErrorMessage(ex.Message);
                    throw ex;
                }
                catch (System.Exception ex)
                {
                    String ErrorMessage = "SET EVENT Selected Applicant ID:-" + CurrentViewContext.CurrentApplicantId_Global.ToString() + "Subscription ID:-, " +
                    CurrentViewContext.SelectedPackageSubscriptionID_Global + " || " + ex.Message;
                    ex.Source = ErrorMessage;
                    base.LogError(ErrorMessage, ex);
                    base.ShowErrorMessage(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        ///get and  set Category id 
        /// </summary>
        public Int32 SelectedComplianceCategoryId_Global
        {
            get { return Convert.ToInt32(ViewState["SelectedComplianceCategoryId"] ?? "0"); }
            set { ViewState["SelectedComplianceCategoryId"] = value; }
        }
        public Int32 PrevComplianceCategoryId_Global
        {
            get { return (Int32)(ViewState["PrevComplianceCategoryId_Global"]); }
            set { ViewState["PrevComplianceCategoryId_Global"] = value; }
        }

        public Int32 NextComplianceCategoryId_Global
        {
            get { return (Int32)(ViewState["NextComplianceCategoryId_Global"]); }
            set { ViewState["NextComplianceCategoryId_Global"] = value; }
        }
        public Int32 SelectedPackageSubscriptionID_Global
        {
            get
            {
                if (ViewState["CurrentPackageSubscriptionID"].IsNotNull())
                    return (Int32)(ViewState["CurrentPackageSubscriptionID"]);
                else
                    return 0;
            }

            set { ViewState["CurrentPackageSubscriptionID"] = value; }
        }

        /// <summary>
        ///get and  set package id .
        /// </summary>
        public Int32 CurrentCompliancePackageId_Global
        {
            get { return (Int32)(ViewState["CurrentCompliancePackageId"]); }
            set { ViewState["CurrentCompliancePackageId"] = value; }
        }

        public Int32 ItemDataId_Global
        {
            get { return Convert.ToInt32(ViewState["ItemDataId"] ?? "0"); }
            set { ViewState["ItemDataId"] = value; }
        }

		public Int32 ComplianceItemReconciliationDataID_Global
		{
			get { return Convert.ToInt32(ViewState["ComplianceItemReconciliationDataID"] ?? "0"); }
			set { ViewState["ComplianceItemReconciliationDataID"] = value; }
		}
		

		public Int32 CurrentTenantId_Global
        {
            get { return (Int32)(ViewState["CurrentTenantId"]); }
            set { ViewState["CurrentTenantId"] = value; }
        }


        public List<ApplicantDocuments> lstApplicantDocument
        {
            get
            {
                return (List<ApplicantDocuments>)ViewState["lstApplicantDocument_Details"];
            }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    value = new List<ApplicantDocuments>();
                }
                ViewState["lstApplicantDocument_Details"] = value;
                ucVerificationItemDataPanel.lstApplicantDocument = value;
                ucVerificationDocumentPanel.lstApplicantDocument = value;
                ucVerificationApplicantPanel.lstApplicantDocument = value;
            }
        }


        public string LoggedInUserName_Global
        {
            get
            {
                return (String)(ViewState["LoggedInUserName"]);
            }
            set
            {
                ViewState["LoggedInUserName"] = value;
            }
        }

        public String LoggedInUserInitials_Global
        {
            get
            {
                return (String)(ViewState["LoggedInUserInitials"]);
            }
            set
            {
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

        //public String AllowedFileExtensions_Global
        //{
        //    get
        //    {
        //        if (!ViewState["AllowedFileExtensions"].IsNull())
        //        {
        //            return Convert.ToString(ViewState["AllowedFileExtensions"]);
        //        }
        //        return String.Empty;
        //    }
        //    set
        //    {
        //        ViewState["AllowedFileExtensions"] = value;
        //    }
        //}
        #endregion

        #region Events

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (HttpContext.Current.Items["RefreshPage"].IsNotNull() && HttpContext.Current.Items["RefreshPage"].ToString() == "1")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "Reloadmiddlepanel();", true);
            }
        }
        /// <summary>
        /// OnInit event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                AddUpdateMethodToDelegate();
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

                ReLoadDataItemPanel _reload = new VerificationDetails.ReLoadDataItemPanel(LoadDataItemPanel);
                //Set method reference to a user control delegate
                this.ucVerificationApplicantPanel.ReLoadDataItemPanel = _reload;

                base.OnInit(e);
                base.Title = "Verification Details";
                BasePage basePage = base.Page as BasePage;
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
            // to set focus on "ucVerificationItemDataPanel" middle pane
            String key = "CategoryPanel";
            if (HttpContext.Current.Items[key].IsNull())
            {
                HttpContext.Current.Items.Add(key, sptrCategoryView);
            }

            if (!this.IsPostBack)
            {

                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    CaptureQuerystringParameters(args);

                }
                Presenter.GetTenant();
                //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
                Presenter.SetUserVerificationPermission();
                SetPageDataAndLayout(!this.IsPostBack);
            }
            Presenter.OnViewLoaded();
            if (Request.Form.Count > 0)
            {
                var requestParameter = Request.Form.GetValues("hdnScrollClassValue");
                String scrollValues = requestParameter.IsNotNull() ? requestParameter[0] : "";
                hdnClassName.Value = scrollValues;
            }


            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
            HandleCategoryPreviousNext();
            //base.SetPageTitle("Verification Details");

            if (this.WorkQueue.IsNotNull() && (this.WorkQueue == WorkQueueType.DataItemSearch || this.WorkQueue == WorkQueueType.AssigneeDataItemSearch
                || this.WorkQueue == WorkQueueType.ComplianceSearch || this.WorkQueue == WorkQueueType.ComprehensiveSearch))
                lnkGoBack.InnerText = "Back to Search";
            if (this.WorkQueue.IsNotNull() && this.WorkQueue == WorkQueueType.SupportPortalDetail)
                lnkGoBack.InnerText = "Back to Support Portal Detail";
            if (this.WorkQueue.IsNotNull() && this.WorkQueue == WorkQueueType.ReconciliationQueue)
                lnkGoBack.InnerText = "Back to Reconciliation Queue";
			if (this.WorkQueue.IsNotNull() && this.WorkQueue == WorkQueueType.ReconciliationDetail)
				lnkGoBack.InnerText = "Back to Reconciliation Detail";
            lstApplicantDocument = lstApplicantDocument;
            RoutePageBack(true);

        }


        protected void Page_Init(object sender, EventArgs e)
        {
            //var updatePanel = Page.Master.FindControl("UpdatePanel1") as System.Web.UI.UpdatePanel;
            //updatePanel.UpdateMode = System.Web.UI.UpdatePanelUpdateMode.Conditional;
            //updatePanel.ChildrenAsTriggers = false;
        }

        private void HandleCategoryPreviousNext()
        {
            ucVerificationItemDataPanel.CategoryNextClick -= new VerificationItemDataPanel.CategoryChanged(ucVerificationItemDataPanel_CategoryNextClickk);
            ucVerificationItemDataPanel.CategoryNextClick += new VerificationItemDataPanel.CategoryChanged(ucVerificationItemDataPanel_CategoryNextClickk);

            ucVerificationItemDataPanel.CategoryPreviousClick -= new VerificationItemDataPanel.CategoryChanged(ucVerificationItemDataPanel_CategoryPreviousClick);
            ucVerificationItemDataPanel.CategoryPreviousClick += new VerificationItemDataPanel.CategoryChanged(ucVerificationItemDataPanel_CategoryPreviousClick);
        }

        private void ucVerificationItemDataPanel_CategoryPreviousClick(object sender, EventArgs e)
        {
            if (this.PrevComplianceCategoryId_Global > 0)
            {
                this.SelectedComplianceCategoryId_Global = this.PrevComplianceCategoryId_Global;
                this.LoadApplicantPanel(false);
            }
        }

        private void ucVerificationItemDataPanel_CategoryNextClickk(object sender, EventArgs e)
        {
            if (this.NextComplianceCategoryId_Global > 0)
            {
                this.SelectedComplianceCategoryId_Global = this.NextComplianceCategoryId_Global;
                this.LoadApplicantPanel(false);
            }
        }

        #endregion

        #region "Methods"
        private void SetPageDataAndLayout(Boolean GetFreshData)
        {
            if (GetFreshData)
            {
                this._presenter.OnViewInitialized();
                LoadApplicantPanel(true);
            }
        }
        Dictionary<String, String> dicData;
        private void CaptureQuerystringParameters(Dictionary<String, String> args)
        {
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.IsNotNull() && args.Count() > 0)
            {
                if (args.ContainsKey("IsEscalationRecords"))
                {
                    this.IsEscalationRecords = Convert.ToBoolean(args.ContainsKey("IsEscalationRecords"));
                }

                if (args.ContainsKey("TenantId"))
                {
                    this.TenantId_Global = Convert.ToInt32(args["TenantId"]);
                    //ViewState["TenantId"] = SelectedTenantId;
                }
                if (args.ContainsKey("ItemDataId"))
                {
                    ItemDataId_Global = Convert.ToInt32(args["ItemDataId"]);
                }
                if (args.ContainsKey("WorkQueueType"))
                {
                    WorkQueue = (WorkQueueType)Enum.Parse(typeof(WorkQueueType), Convert.ToString(args["WorkQueueType"]), true);
                }
                if (args.ContainsKey("PackageId"))
                {
                    PackageId = Convert.ToInt32(args["PackageId"]);
                }
                if (args.ContainsKey("CategoryId"))
                {
                    CategoryId = Convert.ToInt32(args["CategoryId"]);
                }
                if (args.ContainsKey("SelectedPackageSubscriptionId"))
                {
                    this.SelectedPackageSubscriptionID_Global = Convert.ToInt32(args["SelectedPackageSubscriptionId"]);
                }
               
                
                if (args.ContainsKey("IsException"))
                {
                    IsException = Convert.ToBoolean(args["IsException"]);
                }

                if (args.ContainsKey("SelectedComplianceCategoryId"))
                {
                    this.SelectedComplianceCategoryId_Global = Convert.ToInt32(args["SelectedComplianceCategoryId"]);
                }
                else
                {
                    Session.Remove("PackageSubscriptionIdList");
                    Session.Remove("SubPageIndex");
                    Session.Remove("SubTotalPages");
                }
                if (!args.ContainsKey("IsPostBack"))
                {
                    Session.Remove("PackageSubscriptionIdList");
                    Session.Remove("SubPageIndex");
                    Session.Remove("SubTotalPages");
                }
                if (args.ContainsKey("UserGroupId"))
                {
                    UserGroupId = Convert.ToInt32(args["UserGroupId"]);
                }
                if (args.ContainsKey("ApplicantId"))
                {
                    CurrentApplicantId_Global = Convert.ToInt32(args["ApplicantId"]);
                }
                if (args.ContainsKey("ActionType") && args["ActionType"] == VerificationDetailActionType.SubscriptionPreviousNext.GetStringValue())
                {
                    CurrentApplicantId_Global = Presenter.GetApplicantIdForSubscription();
                    ActionType = args["ActionType"];
                }

                if (args.ContainsKey("SelectedArchiveStateCode"))
                {
                    SelectedArchiveStateCode = args["SelectedArchiveStateCode"];
                }

                if (args.ContainsKey("UserId"))
                {
                    UserID = Convert.ToString(args["UserId"]);
                }
				if (args.ContainsKey("ComplianceItemReconciliationDataID"))
				{
					ComplianceItemReconciliationDataID_Global = Convert.ToInt32(args["ComplianceItemReconciliationDataID"]);
				}
			}
            if (args.ContainsKey("ItemDataId"))
            {
                ItemDataId_Global = Convert.ToInt32(args["ItemDataId"]);
            }
            if (args.ContainsKey("WorkQueueType"))
            {
                WorkQueue = (WorkQueueType)Enum.Parse(typeof(WorkQueueType), Convert.ToString(args["WorkQueueType"]), true);
            }
            if (args.ContainsKey("PackageId"))
            {
                PackageId = Convert.ToInt32(args["PackageId"]);
            }
            if (args.ContainsKey("CategoryId"))
            {
                CategoryId = Convert.ToInt32(args["CategoryId"]);
            }
            if (args.ContainsKey("UserGroupId"))
            {
                UserGroupId = Convert.ToInt32(args["UserGroupId"]);
            }
            if (args.ContainsKey("ShowOnlyRushOrders"))
            {
                ShowOnlyRushOrders = Convert.ToBoolean(args["ShowOnlyRushOrders"]);
            }
            if (args.ContainsKey("IncludeIncompleteItems"))
            {
                IncludeIncompleteItems = Convert.ToBoolean(args["IncludeIncompleteItems"]);
            }
            if (args.ContainsKey("SelectedProgramStudyId"))
            {
                ProgramId = Convert.ToInt32(args["SelectedProgramStudyId"]);
            }
            if (args.ContainsKey("SelectedItemComplianceStatusId"))
            {
                CurrentViewContext.SelectedItemComplianceStatusId = Convert.ToString(args["SelectedItemComplianceStatusId"]);
            }
            if (args.ContainsKey("ApplicantFirstName"))
            {
                CurrentViewContext.ApplicantFirstName = args["ApplicantFirstName"];
            }
            if (args.ContainsKey("ApplicantLastName"))
            {
                CurrentViewContext.ApplicantLastName = args["ApplicantLastName"];
            }
            DateTime dob;
            if (args.ContainsKey("DateOfBirth") && DateTime.TryParse(args["DateOfBirth"], out dob))
            {
                CurrentViewContext.DateOfBirth = dob;
            }
            if (args.ContainsKey("ItemLabel"))
            {
                CurrentViewContext.ItemLabel = args["ItemLabel"];
            }
            //UAT-5063
            DateTime submissionDateFrom;
            if (args.ContainsKey("SubmissionDateFrom") && DateTime.TryParse(args["SubmissionDateFrom"], out submissionDateFrom))
            {
                CurrentViewContext.SubmissionDateFrom = submissionDateFrom;
            }
            DateTime submissionDateTo;
            if (args.ContainsKey("SubmissionDateTo") && DateTime.TryParse(args["SubmissionDateTo"], out submissionDateTo))
            {
                CurrentViewContext.SubmissionDateTo = submissionDateTo;
            }
            if (args.ContainsKey("AssignedUser"))
            {
                CurrentViewContext.AssignedUser = args["AssignedUser"];
            }
            if (args.ContainsKey("PageType") && args["PageType"].IsNotNull() && (args["PageType"] == "VerificationDetail"))
            {
                if (args.ContainsKey("TenantId"))
                {
                    this.TenantId_Global = Convert.ToInt32(args["TenantId"]);
                }
                if (args.ContainsKey("PackageSubscriptionId"))
                {
                    this.SelectedPackageSubscriptionID_Global = Convert.ToInt32(args["PackageSubscriptionId"]);
                }
                if (args.ContainsKey("WorkQueueType"))
                {
                    WorkQueue = (WorkQueueType)Enum.Parse(typeof(WorkQueueType), Convert.ToString(args["WorkQueueType"]), true);
                }
				
			}
            //UAT 4067
            //if (args.ContainsKey("allowedFileExtensions"))
            //{
            //    this.AllowedFileExtensions_Global = args["allowedFileExtensions"];
            //}
            //else
            //{
            //    dicData = (Dictionary<String, String>)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.COMPLIANCE_VERIFICATION_PARAMETERS);

            //    if (dicData.IsNotNull() && dicData.Count() > 0)
            //    {
            //        this.TenantId_Global = Convert.ToInt32(dicData["TenantId_Global"]);
            //        this.SelectedComplianceCategoryId_Global = this.CategoryId = Convert.ToInt32(dicData["SelectedComplianceCategoryId_Global"]);
            //        this.CurrentCompliancePackageId_Global = this.PackageId = Convert.ToInt32(dicData["CurrentCompliancePackageId_Global"]);
            //        this.ItemDataId_Global = Convert.ToInt32(dicData["ItemDataId_Global"]);
            //        this.SelectedPackageSubscriptionID_Global = Convert.ToInt32(dicData["SelectedPackageSubscriptionID_Global"]);
            //        this.CurrentApplicantId_Global = Convert.ToInt32(dicData["CurrentApplicantId_Global"]);
            //        this.NextComplianceCategoryId_Global = Convert.ToInt32(dicData["NextComplianceCategoryId_Global"]);
            //        this.PrevComplianceCategoryId_Global = Convert.ToInt32(dicData["PrevComplianceCategoryId_Global"]);
            //    }
            //}
        }


        private void AddUpdateMethodToDelegate()
        {
            if (HttpContext.Current.Items["UpdateDocumentList"] == null)
            {
                del = new UpdateDocumentList(SetUpdateDocumentList);
                HttpContext.Current.Items["UpdateDocumentList"] = del;
            }
            else
            {
                del = (UpdateDocumentList)HttpContext.Current.Items["UpdateDocumentList"];
                del += new UpdateDocumentList(SetUpdateDocumentList);
                HttpContext.Current.Items["UpdateDocumentList"] = del;
            }
        }

        #endregion

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	


        public void SetUpdateDocumentList(List<ApplicantDocuments> updatedList)
        {
            CurrentViewContext.lstApplicantDocument = updatedList;
        }

        public void LoadApplicantPanel(bool IsFirstTime)
        {
            //Seting properties of usercontrol Applicant Panel            
            Presenter.GetApplicantDocuments();
            ucVerificationApplicantPanel.SelectedComplianceCategoryId_Global = this.SelectedComplianceCategoryId_Global;
            ucVerificationApplicantPanel.WorkQueue = this.WorkQueue;
            ucVerificationApplicantPanel.viewType = this._viewType;
            ucVerificationApplicantPanel.CategoryId = (IsFirstTime) ? this.CategoryId : ucVerificationApplicantPanel.CategoryId;
            ucVerificationApplicantPanel.IncludeIncompleteItems = this.IncludeIncompleteItems;
            ucVerificationApplicantPanel.ItemDataId_Global = (IsFirstTime) ? this.ItemDataId_Global : ucVerificationApplicantPanel.ItemDataId_Global;
            ucVerificationApplicantPanel.TenantId_Global = this.TenantId_Global;
            ucVerificationApplicantPanel.PackageId = (IsFirstTime) ? this.PackageId : ucVerificationApplicantPanel.PackageId;
            ucVerificationApplicantPanel.IncludeIncompleteItems = this.IncludeIncompleteItems;
            ucVerificationApplicantPanel.ShowOnlyRushOrders = this.ShowOnlyRushOrders;
            ucVerificationApplicantPanel.SelectedPackageSubscriptionID_Global = (IsFirstTime) ? this.SelectedPackageSubscriptionID_Global : ucVerificationApplicantPanel.SelectedPackageSubscriptionID_Global;
            ucVerificationApplicantPanel.IsException = IsException;
            ucVerificationApplicantPanel.UserGroupId = (IsFirstTime) ? this.UserGroupId : ucVerificationApplicantPanel.UserGroupId;
            ucVerificationApplicantPanel.OrganizationUserData = this.OrganizationData;
            ucVerificationApplicantPanel.ActionType = ActionType;
            ucVerificationApplicantPanel.IsEscalationRecords = this.IsEscalationRecords;
            //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
            ucVerificationApplicantPanel.IsFullPermissionForVerification = CurrentViewContext.IsFullPermissionForVerification;
            //UAT-2460
            ucVerificationApplicantPanel.SelectedArchiveStateCode = this.SelectedArchiveStateCode;
            //UAT-3744
            ucVerificationApplicantPanel.SelectedComplianceItemReconciliationDataID = Convert.ToString(this.ComplianceItemReconciliationDataID_Global);
            //UAT 4067
           // ucVerificationApplicantPanel.AllowedFileExtensions = this.AllowedFileExtensions_Global;
            if (this.IsPostBack)
            {
                ucVerificationApplicantPanel.SetPageDataAndLayout(true);
            }
        }

        public void LoadDataItemPanel(Dictionary<string, string> args)
        {
            this.CurrentApplicantId_Global = (args.ContainsKey("CurrentApplicantId_Global")) ? Convert.ToInt32(args["CurrentApplicantId_Global"]) : this.CurrentApplicantId_Global;
            this.CurrentCompliancePackageId_Global = (args.ContainsKey("CurrentCompliancePackageId_Global")) ? Convert.ToInt32(args["CurrentCompliancePackageId_Global"]) : this.CurrentCompliancePackageId_Global;
            this.SelectedPackageSubscriptionID_Global = (args.ContainsKey("SelectedPackageSubscriptionID_Global")) ? Convert.ToInt32(args["SelectedPackageSubscriptionID_Global"]) : this.SelectedPackageSubscriptionID_Global;
            this.CurrentTenantId_Global = (args.ContainsKey("CurrentTenantId_Global")) ? Convert.ToInt32(args["CurrentTenantId_Global"]) : this.CurrentTenantId_Global;
            this.ItemDataId_Global = (args.ContainsKey("ItemDataId_Global")) ? Convert.ToInt32(args["ItemDataId_Global"]) : this.ItemDataId_Global;
            this.NextComplianceCategoryId_Global = (args.ContainsKey("NextComplianceCategoryId_Global")) ? Convert.ToInt32(args["NextComplianceCategoryId_Global"]) : this.NextComplianceCategoryId_Global;
            this.PrevComplianceCategoryId_Global = (args.ContainsKey("PrevComplianceCategoryId_Global")) ? Convert.ToInt32(args["PrevComplianceCategoryId_Global"]) : this.PrevComplianceCategoryId_Global;
            this.SelectedComplianceCategoryId_Global = (args.ContainsKey("SelectedComplianceCategoryId_Global")) ? Convert.ToInt32(args["SelectedComplianceCategoryId_Global"]) : this.SelectedComplianceCategoryId_Global;
            this.TenantId_Global = (args.ContainsKey("TenantId_Global")) ? Convert.ToInt32(args["TenantId_Global"]) : this.TenantId_Global;
            this.UserGroupId = (args.ContainsKey("UserGroupId")) ? Convert.ToInt32(args["UserGroupId"]) : this.UserGroupId;
            this.IsEscalationRecords = (args.ContainsKey("IsEscalationRecords")) ? Convert.ToBoolean(args["IsEscalationRecords"]) : this.IsEscalationRecords;
            this.ComplianceItemReconciliationDataID_Global = (args.ContainsKey("ComplianceItemReconciliationDataID_Global")) ? Convert.ToInt32(args["ComplianceItemReconciliationDataID_Global"]) : this.ItemDataId_Global;
          // this.AllowedFileExtensions_Global= (args.ContainsKey("allowedFileExtensions")) ? Convert.ToString(args["allowedFileExtensions"]) : this.AllowedFileExtensions_Global; //UAT 4067

            SetItemScreenIds();
            LoadDocumentPanel();
            if (this.IsPostBack)
            {
                ucVerificationItemDataPanel.LoadItems();
            }
            ucVerificationItemDataPanel.IsException = IsException;
            if (this.WorkQueue == WorkQueueType.ComplianceSearch)
            {
                lnkApplicantView.Visible = true;
                lnkToShowApplicantScreen();
            }
            if (this.WorkQueue == WorkQueueType.ComprehensiveSearch)
            {
                LnkGoToSubscrptnDetail.Visible = true;
                lnkToShowSubscrptnDetailScreen();
            }
			if (this.WorkQueue == WorkQueueType.ReconciliationDetail)
			{
				lnkToShowReconciliationDetailScreen();
			}
        }

        void SetItemScreenIds()
        {
            ucVerificationItemDataPanel.CurrentTenantId_Global = this.CurrentTenantId_Global;
            ucVerificationItemDataPanel.SelectedTenantId_Global = this.TenantId_Global;
            ucVerificationItemDataPanel.CurrentPackageSubscriptionID_Global = this.SelectedPackageSubscriptionID_Global;
            ucVerificationItemDataPanel.SelectedCompliancePackageId_Global = this.CurrentCompliancePackageId_Global;
            ucVerificationItemDataPanel.SelectedComplianceCategoryId_Global = this.SelectedComplianceCategoryId_Global;
            ucVerificationItemDataPanel.SelectedApplicantId_Global = this.CurrentApplicantId_Global;
            ucVerificationItemDataPanel.CurrentLoggedInUserName_Global = this.LoggedInUserName_Global;
            ucVerificationItemDataPanel.NextComplianceCategoryId_Global = this.NextComplianceCategoryId_Global;
            ucVerificationItemDataPanel.PreviousComplianceCategoryId_Global = this.PrevComplianceCategoryId_Global;
            ucVerificationItemDataPanel.ItemDataId_Global = this.ItemDataId_Global;
            ucVerificationItemDataPanel.IncludeIncompleteItems_Global = this.IncludeIncompleteItems;
            ucVerificationItemDataPanel.WorkQueue = this.WorkQueue;
            ucVerificationItemDataPanel.PackageId_Global = this.PackageId;
            ucVerificationItemDataPanel.CategoryId_Global = this.CategoryId;
            ucVerificationItemDataPanel.ShowOnlyRushOrders = this.ShowOnlyRushOrders;
            ucVerificationItemDataPanel.UserGroupId = this.UserGroupId;
            ucVerificationItemDataPanel.lstApplicantDocument = this.lstApplicantDocument;
            ucVerificationItemDataPanel.LoggedInUserInitials_Global = this.LoggedInUserInitials_Global;
            ucVerificationItemDataPanel.IsEscalationRecords = this.IsEscalationRecords;
            //UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
            ucVerificationItemDataPanel.IsFullPermissionForVerification = this.IsFullPermissionForVerification;
            ucVerificationItemDataPanel.SelectedArchiveStateCode = this.SelectedArchiveStateCode;
            //UAT-3744
            ucVerificationItemDataPanel.SelectedComplianceItemReconciliationDataID = Convert.ToString(this.ComplianceItemReconciliationDataID_Global);
            
        }

        public void LoadDocumentPanel()
        {
            ucVerificationDocumentPanel.SelectedComplianceCategoryId_Global = this.SelectedComplianceCategoryId_Global;
            ucVerificationDocumentPanel.ItemDataId = this.ItemDataId_Global;
            ucVerificationDocumentPanel.SelectedTenantId = this.TenantId_Global;
            ucVerificationDocumentPanel.PackageSubscriptionId = this.SelectedPackageSubscriptionID_Global;
            ucVerificationDocumentPanel.OrganizationUserId = this.CurrentApplicantId_Global;
            ucVerificationDocumentPanel.WorkQueue = Convert.ToString(this.WorkQueue);
            ucVerificationDocumentPanel.lstApplicantDocument = this.lstApplicantDocument;
			ucVerificationDocumentPanel.SelectedPackageId  = Convert.ToString(this.PackageId);
			ucVerificationDocumentPanel.SelectedItemDataId = Convert.ToString(this.ItemDataId_Global);
			ucVerificationDocumentPanel.SelectedComplianceItemReconciliationDataID = Convert.ToString(this.ComplianceItemReconciliationDataID_Global);
			//UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen
			ucVerificationDocumentPanel.IsFullPermissionForVerification = this.IsFullPermissionForVerification;
            ucVerificationDocumentPanel.IsException = IsException;

            if (IsPostBack)
                ucVerificationDocumentPanel.BindData();

        }

        private void lnkToShowApplicantScreen()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    {"TenantId", CurrentViewContext.TenantId_Global.ToString() },
                                                                    {"Child", ChildControls.SubscriptionDetail},
                                                                    {"WorkQueueType",WorkQueueType.ComplianceSearch.ToString()},
                                                                    {"PackageId", CurrentViewContext.CurrentCompliancePackageId_Global.ToString()},
                                                                    {"ApplicantId",Convert.ToString(CurrentApplicantId_Global)},
                                                                    {"PackageSubscriptionId",CurrentViewContext.SelectedPackageSubscriptionID_Global.ToString()},
                                                                    {"IsFullPermissionForVerification", IsFullPermissionForVerification.ToString()},
                                                                    {"SelectedArchiveStateCode", this.SelectedArchiveStateCode}
                                                                 };
            // string url = String.Format(@"/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            string url = String.Format(@"/Dashboard/Pages/ApplicantDashboardMain.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            lnkApplicantView.HRef = url;
        }

        /// <summary>
        /// To route Page Back
        /// </summary>
        /// <param name="showSuccessMessage"></param>
        private void RoutePageBack(bool showSuccessMessage, String successMessage = null)
        {
            Dictionary<String, String> queryString = null;

            if (this.WorkQueue == WorkQueueType.ComplianceSearch)
            {
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"TenantId",  Convert.ToString(this.TenantId_Global) },
                                                                    {"Child", this.WorkQueuePath},
                                                                 };
            }

            else if (CurrentViewContext.WorkQueue == WorkQueueType.AssignmentWorkQueue || CurrentViewContext.WorkQueue == WorkQueueType.UserWorkQueue)
            {
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "SelectedTenantId", Convert.ToString(this.TenantId_Global) },
                                                                    { "Child", this.WorkQueuePath},
                                                                    {"PackageId",this.PackageId.ToString()},
                                                                    {"CategoryId",this.CategoryId.ToString()},
                                                                    {"IncludeIncompleteItems",this.IncludeIncompleteItems.ToString()},
                                                                    {"ShowOnlyRushOrders",this.ShowOnlyRushOrders.ToString()},
                                                                    {"UserGroupId",UserGroupId.ToString()}
                                                                 };
            }
            else if (CurrentViewContext.WorkQueue == WorkQueueType.EsclationAssignmentWorkQueue || CurrentViewContext.WorkQueue == WorkQueueType.EsclationUserWorkQueue)
            {
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "SelectedTenantId", Convert.ToString(this.TenantId_Global) },
                                                                    { "Child", this.WorkQueuePath},
                                                                    {"PackageId",this.PackageId.ToString()},
                                                                    {"CategoryId",this.CategoryId.ToString()},
                                                                    {"IncludeIncompleteItems",this.IncludeIncompleteItems.ToString()},
                                                                    {"ShowOnlyRushOrders",this.ShowOnlyRushOrders.ToString()},
                                                                    {"UserGroupId",UserGroupId.ToString()}
                                                                 };
            }
            else if (CurrentViewContext.WorkQueue == WorkQueueType.ExceptionAssignmentWorkQueue || CurrentViewContext.WorkQueue == WorkQueueType.ExceptionUserWorkQueue)
            {
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "SelectedTenantId", Convert.ToString(this.TenantId_Global) },
                                                                    { "Child", this.WorkQueuePath},
                                                                    {"PackageId",this.PackageId.ToString()},
                                                                    {"CategoryId",this.CategoryId.ToString()},
                                                                    {"IncludeIncompleteItems",this.IncludeIncompleteItems.ToString()},
                                                                    {"ShowOnlyRushOrders",this.ShowOnlyRushOrders.ToString()},
                                                                    {"UserGroupId",UserGroupId.ToString()}
                                                                 };
            }
            else if (CurrentViewContext.WorkQueue == WorkQueueType.AssigneeDataItemSearch || CurrentViewContext.WorkQueue == WorkQueueType.DataItemSearch)
            {
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "SelectedTenantId", Convert.ToString(this.TenantId_Global) },
                                                                    { "Child", this.WorkQueuePath},
                                                                    {"PackageId",this.PackageId.ToString()},
                                                                    {"CategoryId",this.CategoryId.ToString()},
                                                                    {"IncludeIncompleteItems",this.IncludeIncompleteItems.ToString()},
                                                                    {"ShowOnlyRushOrders",this.ShowOnlyRushOrders.ToString()},
                                                                    {"SelectedProgramStudyId",Convert.ToString( this.ProgramId)},
                                                                    {"ApplicantFirstName",Convert.ToString( this.ApplicantFirstName)},
                                                                    {"ApplicantLastName",Convert.ToString( this.ApplicantLastName)},
                                                                    {"DateOfBirth",Convert.ToString( this.DateOfBirth)},
                                                                    {"ItemLabel",Convert.ToString( this.ItemLabel)},
                                                                    {"SelectedItemComplianceStatusId",Convert.ToString(SelectedItemComplianceStatusId)},
                                                                    {"SubmissionDateFrom",Convert.ToString( this.SubmissionDateFrom)},
                                                                    {"SubmissionDateTo",Convert.ToString( this.SubmissionDateTo)},
                                                                    {"AssignedUser",Convert.ToString( this.AssignedUser)},

                                                                 };
            }

            else if (CurrentViewContext.WorkQueue == WorkQueueType.ComprehensiveSearch)
            {
                queryString = new Dictionary<String, String>
                                                                 {

                                                                    {"Child", ChildControls.ApplicantComprehensiveSearchPage},
                                                                    {"PageType", WorkQueueType.ComprehensiveSearch.ToString()}
                                                                 };
            }
            else if (CurrentViewContext.WorkQueue == WorkQueueType.SupportPortalDetail)
            {
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", ChildControls.SupportPortalDetails},
                                                                    { "OrganizationUserId",Convert.ToString(CurrentApplicantId_Global)},
                                                                    {"TenantId",Convert.ToString(this.TenantId_Global)},
                                                                    {"UserId",UserID},
                                                                    { "PageType",CurrentViewContext.WorkQueue.ToString()}
                                                                  };
            }
            else if (CurrentViewContext.WorkQueue == WorkQueueType.ReconciliationQueue)
            {
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "SelectedTenantId", Convert.ToString(this.TenantId_Global) },
                                                                    { "Child", this.WorkQueuePath},
                                                                    {"PackageId",this.PackageId.ToString()},
                                                                    {"CategoryId",this.CategoryId.ToString()},
                                                                    //{"IncludeIncompleteItems",this.IncludeIncompleteItems.ToString()},
                                                                    //{"ShowOnlyRushOrders",this.ShowOnlyRushOrders.ToString()},
                                                                    //{"UserGroupId",UserGroupId.ToString()}
                                                                 };
            }
            if (CurrentViewContext.WorkQueue == WorkQueueType.ReconciliationDetail)
            {
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TenantId", Convert.ToString(this.TenantId_Global) },
                                                                    { "Child", ChildControls.ReconciliationDetail},
                                                                    { "ItemDataId", Convert.ToString(this.ItemDataId_Global) },
                                                                    {"IsException","false"},
                                                                    {"WorkQueueType",Convert.ToString(WorkQueueType.ReconciliationDetail)},
                                                                    {"PackageId",this.PackageId.ToString()},
                                                                    {"CategoryId",this.CategoryId.ToString()},
                                                                    {"SelectedPackageSubscriptionId",Convert.ToString(this.SelectedPackageSubscriptionID_Global)},
                                                                    {"SelectedComplianceCategoryId",this.CategoryId.ToString()},
                                                                    {"ApplicantId",Convert.ToString(this.CurrentApplicantId_Global)},
                                                                    {"ComplianceItemReconciliationDataID",Convert.ToString(this.ComplianceItemReconciliationDataID_Global)},
                                                                     //UAT-3744,
                                                                    {"IsReturnedFromVerificationDetails",Convert.ToString(true)}
                                                                 };


			}
			if (showSuccessMessage && successMessage != null)
            {
                queryString.Add("UpdatedStatus", successMessage);
            }
			String _url = string.Empty;
			if (CurrentViewContext.WorkQueue == WorkQueueType.ReconciliationDetail)
			{
				_url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
			}
			else
			{
				_url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
			}

			lnkGoBack.HRef = _url;
			//Response.Redirect(_url, true);
		}

        private void lnkToShowSubscrptnDetailScreen()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    {"TenantId", CurrentViewContext.TenantId_Global.ToString() },
                                                                    {"Child", ChildControls.ApplicantPortfolioDetailPage},
                                                                    {"OrganizationUserId",Convert.ToString(CurrentApplicantId_Global)},
                                                                   {"PageType", WorkQueueType.ComprehensiveSearch.ToString()}
                                                                 };


            string url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            LnkGoToSubscrptnDetail.HRef = url;
        }

		private void lnkToShowReconciliationDetailScreen()
		{
			var queryString = new Dictionary<String, String>
																 {
																	{ "TenantId",CurrentViewContext.TenantId_Global.ToString() },
																	{ "Child",  ChildControls.ReconciliationDetail},
																	{ "ItemDataId", Convert.ToString(this.ItemDataId_Global)},
																	{"WorkQueueType", WorkQueueType.ReconciliationDetail.ToString()},
																	{"PackageId",this.PackageId.ToString()},
																	{"CategoryId",this.CategoryId.ToString()},
																	{"SelectedPackageSubscriptionId",Convert.ToString(this.SelectedPackageSubscriptionID_Global)},
																	{"SelectedComplianceCategoryId",this.CategoryId.ToString()},
																	{"ApplicantId",Convert.ToString(this.CurrentApplicantId_Global)},
																	{"ComplianceItemReconciliationDataID",Convert.ToString(this.ComplianceItemReconciliationDataID_Global) },
                                                                    //UAT-3744,
                                                                    {"IsReturnedFromVerificationDetails",Convert.ToString(true)}
																 };
			String url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
			lnkGoBack.HRef = url;
		}
	}
}

