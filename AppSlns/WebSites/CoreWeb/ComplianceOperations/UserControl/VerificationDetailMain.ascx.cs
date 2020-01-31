using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb;
using CoreWeb.ComplianceOperations.Views;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Microsoft.Practices.ObjectBuilder;


namespace CoreWeb.ComplianceOperations.Views
{
    public partial class VerificationDetailMain : BaseUserControl, IVerificationDetailMainView
    {

        private VerificationDetailMainPresenter _presenter=new VerificationDetailMainPresenter();

        #region Variables

        #region Private Variables

        private String _viewType;

        #endregion

        #endregion

        #region Events
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

                base.OnInit(e);
                base.Title = "Verification Details";
                base.SetPageTitle("Verification Details");
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
            if (!IsPostBack)
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    CaptureQuerystringParameters(args);
                }

                ifrVerificationDetail.Src = String.Format("~/ComplianceOperations/Pages/VerificationMainPage.aspx?ucid={0}&args={1}", _viewType, Request.QueryString["args"]);
                Presenter.GetApplicantDocuments();
                LoadDocumentPanel();
                SetBackUrl();
            }
            //This is to save the view state at the document panel.
            lstApplicantDocument = lstApplicantDocument;
        }

        #endregion

        #region Functions

        #region Private functions

        private void SetBackUrl()
        {
            if (this.WorkQueue.IsNotNull() && (this.WorkQueue == WorkQueueType.DataItemSearch || this.WorkQueue == WorkQueueType.AssigneeDataItemSearch || this.WorkQueue == WorkQueueType.ComplianceSearch))
                lnkGoBack.InnerText = "Back to Search";

            RoutePageBack(true);
        }

        private void CaptureQuerystringParameters(Dictionary<String, String> args)
        {
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.IsNotNull() && args.Count() > 0)
            {

                if (args.ContainsKey("TenantId"))
                {
                    this.TenantId_Global = Convert.ToInt32(args["TenantId"]);
                    //ViewState["TenantId"] = SelectedTenantId;
                }
                if (args.ContainsKey("ApplicantId"))
                {
                    this.CurrentApplicantId_Global = Convert.ToInt32(args["ApplicantId"]);
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


       
        #endregion

        #region public functions

        public void LoadDocumentPanel()
        {
            ucVerificationDocumentPanel.SelectedComplianceCategoryId_Global = this.SelectedComplianceCategoryId_Global;
            //ucVerificationDocumentPanel.ApplicantLastName = this.ApplicantLastName;
            //ucVerificationDocumentPanel.CategoryId = this.CategoryId;
            //ucVerificationDocumentPanel.DateOfBirth = this.DateOfBirth;
            ucVerificationDocumentPanel.ItemDataId = this.ItemDataId_Global;
            ucVerificationDocumentPanel.SelectedTenantId = this.TenantId_Global;
            ucVerificationDocumentPanel.PackageSubscriptionId = this.SelectedPackageSubscriptionID_Global;
            ucVerificationDocumentPanel.OrganizationUserId = this.CurrentApplicantId_Global;
            //ucVerificationDocumentPanel.TenantId = this.SelectedTenantId;
            ucVerificationDocumentPanel.WorkQueue = Convert.ToString(this.WorkQueue);
            ucVerificationDocumentPanel.lstApplicantDocument = this.lstApplicantDocument;

            if (IsPostBack)
                ucVerificationDocumentPanel.BindData();

        }

        /// <summary>
        /// To route Page Back
        /// </summary>
        /// <param name="showSuccessMessage"></param>
        public void RoutePageBack(bool showSuccessMessage, String successMessage = null)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();

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
                                                                    {"SelectedItemComplianceStatusId",Convert.ToString(SelectedItemComplianceStatusId)}
                                                                 };
            }

            if (showSuccessMessage && successMessage != null)
            {
                queryString.Add("UpdatedStatus", successMessage);
            }
            String _url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            lnkGoBack.HRef = _url;
            //Response.Redirect(_url, true);
        }

        public void lnkToShowApplicantScreen()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            //queryString = new Dictionary<String, String>
            //                                                     { 
            //                                                        {"TenantId", CurrentViewContext.TenantId_Global.ToString() },
            //                                                        {"Child", ChildControls.SubscriptionDetail},
            //                                                        {"WorkQueueType",WorkQueueType.ComplianceSearch.ToString()},
            //                                                        {"PackageId", CurrentViewContext.CurrentCompliancePackageId_Global.ToString()},
            //                                                        {"PackageSubscriptionId",CurrentViewContext.SelectedPackageSubscriptionID_Global.ToString()} 
            //                                                     };
            string url = String.Format(@"/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            lnkApplicantView.HRef = url;
        }


        #endregion

        #endregion

        #region Properties

        #region Public properties
        
        public VerificationDetailMainPresenter Presenter
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
        public IVerificationDetailMainView CurrentViewContext
        {
            get { return this; }
        }

        //public IVerificationMainPageView CurrentViewContext
        //{
        //    get
        //    {
        //        return this;
        //    }
        //}

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

        #endregion

        #region Global Properties

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
            get { return (Int32)(ViewState["CurrentApplicantId"]); }
            set { ViewState["CurrentApplicantId"] = value; }
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
                //cVerificationItemDataPanel.lstApplicantDocument = value;
                 ucVerificationDocumentPanel.lstApplicantDocument = value;
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

        #endregion



        #endregion
    }
}