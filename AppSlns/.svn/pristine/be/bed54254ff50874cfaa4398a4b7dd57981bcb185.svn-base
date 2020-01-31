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
using System.Web.UI;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class VerificationMainPage : Page, IVerificationMainPageView
    {
        private VerificationMainPagePresenter _presenter=new VerificationMainPagePresenter();

        #region Variables

        #region Private Variables

        private Int32 _tenantId;
        private Int32 _selectedTenantId;
        private String _viewType;
        delegate void ReLoadDataItemPanel(Dictionary<string, string> args);
        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public IVerificationMainPageView CurrentViewContext
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
                ucVerificationItemDataPanel.lstApplicantDocument = value;
                // ucVerificationDocumentPanel.lstApplicantDocument = value;
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


        #region Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                AddUpdateMethodToDelegate();
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                rprxAdminView.ResolveClientUrl("~/Default/colors.css");
                prxMP.ResolveClientUrl("~/Mod/Shared/AppMaster.js");
                prxMP.ResolveClientUrl("~/Compliance/verification-main.js");
                ReLoadDataItemPanel _reload = new VerificationMainPage.ReLoadDataItemPanel(LoadDataItemPanel);
                //Set method reference to a user control delegate
                this.ucVerificationApplicantPanel.ReLoadDataItemPanel = _reload;
                BasePage basePage = base.Page as BasePage;
            }
            catch (SysXException ex)
            {
                //base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                //base.LogError(ex);
                //base.ShowErrorMessage(ex.Message);
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
                SetPageDataAndLayout(!this.IsPostBack);
            }
            Presenter.OnViewLoaded();


            if (Request.Form.Count > 0)
            {
                var requestParameter = Request.Form.GetValues("hdnScrollClassValue");
                String scrollValues = requestParameter.IsNotNull() ? requestParameter[0] : "";
                //hdnClassName.Value = scrollValues;
            }


            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
            HandleCategoryPreviousNext();
            //base.SetPageTitle("Verification Details");

            //if (this.WorkQueue.IsNotNull() && (this.WorkQueue == WorkQueueType.DataItemSearch || this.WorkQueue == WorkQueueType.AssigneeDataItemSearch || this.WorkQueue == WorkQueueType.ComplianceSearch))
            //    lnkGoBack.InnerText = "Back to Search";
            lstApplicantDocument = lstApplicantDocument;
            //RoutePageBack(true);
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
            String requestParameter = Request.QueryString["args"];
            if (Request.Form.Count > 0)
            {
                requestParameter = Request.Form.GetValues("queryParam")[0];
            }
            args.ToDecryptedQueryString(requestParameter);

            if (args.IsNotNull() && args.Count() > 0)
            {

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
        }


        private void AddUpdateMethodToDelegate()
        {
            if (HttpContext.Current.Items["UpdateDocumentList"] == null)
            {
                var a = new CoreWeb.BaseUserControl.UpdateDocumentList(SetUpdateDocumentList);
                HttpContext.Current.Items["UpdateDocumentList"] = a;
            }
            else
            {
                var b = (CoreWeb.BaseUserControl.UpdateDocumentList)HttpContext.Current.Items["UpdateDocumentList"];
                b += new CoreWeb.BaseUserControl.UpdateDocumentList(SetUpdateDocumentList);
                HttpContext.Current.Items["UpdateDocumentList"] = b;
            }
        }

        #endregion

        
        public VerificationMainPagePresenter Presenter
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

        public void SetUpdateDocumentList(List<ApplicantDocuments> updatedList)
        {
            CurrentViewContext.lstApplicantDocument = updatedList;
        }

        public void LoadApplicantPanel(bool IsFirstTime)
        {
            //Seting properties of usercontrol Applicant Panel            
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
            //Presenter.GetApplicantDocuments();
            SetItemScreenIds();
            //LoadDocumentPanel();
            if (this.IsPostBack)
            {
                ucVerificationItemDataPanel.LoadItems();
            }
            ucVerificationItemDataPanel.IsException = IsException;
            if (this.WorkQueue == WorkQueueType.ComplianceSearch)
            {
                //lnkApplicantView.Visible = true;
                lnkToShowApplicantScreen();
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
                                                                    {"PackageSubscriptionId",CurrentViewContext.SelectedPackageSubscriptionID_Global.ToString()} 
                                                                 };
            string url = String.Format(@"/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            //lnkApplicantView.HRef = url;
        }
    }
}