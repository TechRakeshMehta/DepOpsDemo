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
    public partial class ReconciliationDetail : BaseUserControl, IReconciliationDetailView
    {
        private ReconciliationDetailPresenter _presenter = new ReconciliationDetailPresenter();

        #region Variables

        #region Private Variables

        private Int32 _tenantId;
        private Int32 _selectedTenantId;
        private String _viewType;
        delegate void ReLoadDataItemPanel(Dictionary<string, string> args);
        #endregion

        #endregion

        #region Properties

        public ReconciliationDetailPresenter Presenter
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

        public IReconciliationDetailView CurrentViewContext
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

        public String ActionType
        {
            get;
            set;
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
		public Int32 ComplianceItemReconciliationDataID_Global
		{
			get
			{
				if (ViewState["ComplianceItemReconciliationDataID"].IsNotNull())
					return (Int32)(ViewState["ComplianceItemReconciliationDataID"]);
				else
					return 0;
			}

			set { ViewState["ComplianceItemReconciliationDataID"] = value; }
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
                ucItemDataPanel.lstApplicantDocument = value;
                ucDocumentPanel.lstApplicantDocument = value;
                ucApplicantPanel.lstApplicantDocument = value;
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


        //UAT-3744
        public Boolean IsReturnedFromVerificationDetails
        {
            get
            {
                if (!ViewState["IsReturnedFromVerificationDetails"].IsNull())
                {
                    return (Boolean)ViewState["IsReturnedFromVerificationDetails"];
                }
                return false;
            }
            set
            {
                ViewState["IsReturnedFromVerificationDetails"] = value;
            }
        }
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
                //AddUpdateMethodToDelegate();
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = "Reconciliation Details";
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
            // to set focus on "ucItemDataPanel" middle pane
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
                LoadDataItemPanel(args);
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

            lnkGoBack.InnerText = "Back to Search";
            lstApplicantDocument = lstApplicantDocument;
            RoutePageBack(true);

            //UAT-3744
            if (this.IsReturnedFromVerificationDetails)
            {
                String selectedInstitutionIds = String.Empty;
                List<DataReconciliationQueueContract> lstDataReconciliationQueueContract = new List<DataReconciliationQueueContract>();
                DataReconciliationQueueContract dataReconciliationQueueFilters = (DataReconciliationQueueContract)SysXWebSiteUtils.SessionService.GetCustomData("Data_Reconciliation_Queue");
                if (!dataReconciliationQueueFilters.IsNullOrEmpty())
                {
                    selectedInstitutionIds = String.Join(",", dataReconciliationQueueFilters.selectedTenantIds);
                    if (!selectedInstitutionIds.IsNullOrEmpty())
                        lstDataReconciliationQueueContract = Presenter.GetNextActiveReconciledItem(selectedInstitutionIds, this.ComplianceItemReconciliationDataID_Global);
                    if (!lstDataReconciliationQueueContract.IsNullOrEmpty())
                    {
                        String nextItemURL = RedirectToReconciliationDetailScreen(lstDataReconciliationQueueContract.FirstOrDefault());
                        Response.Redirect(nextItemURL, true);
                    }
                    else
                    {
                        var queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "SelectedTenantId", Convert.ToString(this.TenantId_Global) },
                                                                    { "Child", @"~\ComplianceOperations\DataReconciliationQueue.ascx"},
                                                                    {"PackageId",this.PackageId.ToString()},
                                                                    {"CategoryId",this.CategoryId.ToString()},
                                                                 };

                        String _url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                        Response.Redirect(_url, true);

                    }
                }
            }

            #region Commented Code
            //var _isItemDeletedFromReconciliationQueue = Convert.ToBoolean(Session["IsItemDeletedFromReconciliationQueue"]);
            //var _lstNextPrevReconiciliationItem = (List<DataReconciliationQueueContract>)(Session["CurrentReconciliationIds"]);
            //DataReconciliationQueueContract nextReconciliationData = null;
            //if (!_lstNextPrevReconiciliationItem.IsNullOrEmpty())
            //{
            //    nextReconciliationData = _lstNextPrevReconiciliationItem.LastOrDefault();
            //}
            //if (_isItemDeletedFromReconciliationQueue == true && !nextReconciliationData.IsNullOrEmpty() && _lstNextPrevReconiciliationItem.Count >= AppConsts.TWO && nextReconciliationData.ApplicantComplianceItemId != ItemDataId_Global)
            //{
            //    (Session["CurrentReconciliationIds"]) = null;
            //    Session["IsItemDeletedFromReconciliationQueue"] = false;
            //    string nextItemURL = RedirectToReconciliationDetailScreen(nextReconciliationData);
            //    Response.Redirect(nextItemURL, true);
            //}
            ////UAT-3744
            //else if (_isItemDeletedFromReconciliationQueue == true)
            //{
            //   
            //    //DataReconciliationQueueContract firstReconciliationData = null;
            //    //if (!_lstNextPrevReconiciliationItem.IsNullOrEmpty())
            //    //{
            //    //    firstReconciliationData = _lstNextPrevReconiciliationItem.FirstOrDefault();
            //    //}
            //    //if (!firstReconciliationData.IsNullOrEmpty() && firstReconciliationData.ApplicantComplianceItemId != ItemDataId_Global)
            //    //{
            //    //    (Session["CurrentReconciliationIds"]) = null;
            //    //    Session["IsItemDeletedFromReconciliationQueue"] = false;
            //    //    string firstItemURL = RedirectToReconciliationDetailScreen(firstReconciliationData);
            //    //    Response.Redirect(firstItemURL, true);
            //    //} 
            //   

            //    String selectedInstitutionIds = String.Empty;
            //    List<DataReconciliationQueueContract> lstDataReconciliationQueueContract = new List<DataReconciliationQueueContract>();

            //    DataReconciliationQueueContract dataReconciliationQueueFilters = (DataReconciliationQueueContract)SysXWebSiteUtils.SessionService.GetCustomData("Data_Reconciliation_Queue");
            //    if (!dataReconciliationQueueFilters.IsNullOrEmpty())
            //    {
            //        selectedInstitutionIds = String.Join(",", dataReconciliationQueueFilters.selectedTenantIds);
            //        if (!selectedInstitutionIds.IsNullOrEmpty())
            //            //Get All pending records of reconciliation queue from database.
            //            lstDataReconciliationQueueContract = Presenter.GetAllReconciliationQueueData(selectedInstitutionIds);
            //        if (!lstDataReconciliationQueueContract.IsNullOrEmpty())
            //        {

            //        }
            //    }
            //}

            //if (!_lstNextPrevReconiciliationItem.IsNullOrEmpty() && _lstNextPrevReconiciliationItem.Count == AppConsts.ONE)
            ////   || (!nextReconciliationData.IsNullOrEmpty() && nextReconciliationData.ApplicantComplianceItemId == ItemDataId_Global))
            //{
            //    var queryString = new Dictionary<String, String>
            //                                                     {
            //                                                        { "SelectedTenantId", Convert.ToString(this.TenantId_Global) },
            //                                                        { "Child", @"~\ComplianceOperations\DataReconciliationQueue.ascx"},
            //                                                        {"PackageId",this.PackageId.ToString()},
            //                                                        {"CategoryId",this.CategoryId.ToString()},
            //                                                     };

            //    String _url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            //    Response.Redirect(_url, true);
            //}
            #endregion
        }


        protected void Page_Init(object sender, EventArgs e)
        {

        }

        private void HandleCategoryPreviousNext()
        {
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
                if (args.ContainsKey("TenantId"))
                {
                    this.TenantId_Global = Convert.ToInt32(args["TenantId"]);
                    //ViewState["TenantId"] = SelectedTenantId;
                }
                if (args.ContainsKey("ItemDataId"))
                {
                    ItemDataId_Global = Convert.ToInt32(args["ItemDataId"]);
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
                if (args.ContainsKey("ApplicantId"))
                {
                    CurrentApplicantId_Global = Convert.ToInt32(args["ApplicantId"]);
                }

            }
            if (args.ContainsKey("ItemDataId"))
            {
                ItemDataId_Global = Convert.ToInt32(args["ItemDataId"]);
            }
            if (args.ContainsKey("PackageId"))
            {
                PackageId = Convert.ToInt32(args["PackageId"]);
            }
            if (args.ContainsKey("CategoryId"))
            {
                CategoryId = Convert.ToInt32(args["CategoryId"]);
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
            if (args.ContainsKey("TenantId"))
            {
                this.TenantId_Global = Convert.ToInt32(args["TenantId"]);
            }
            if (args.ContainsKey("PackageSubscriptionId"))
            {
                this.SelectedPackageSubscriptionID_Global = Convert.ToInt32(args["PackageSubscriptionId"]);
            }
            if (args.ContainsKey("ComplianceItemReconciliationDataID"))
            {
                this.ComplianceItemReconciliationDataID_Global = Convert.ToInt32(args["ComplianceItemReconciliationDataID"]);
            }
            //UAT-3744
            if (args.ContainsKey("IsReturnedFromVerificationDetails"))
            {
                this.IsReturnedFromVerificationDetails = Convert.ToBoolean(args["IsReturnedFromVerificationDetails"]);
            }
        }


        //private void AddUpdateMethodToDelegate()
        //{
        //    if (HttpContext.Current.Items["UpdateDocumentList"] == null)
        //    {
        //        del = new UpdateDocumentList(SetUpdateDocumentList);
        //        HttpContext.Current.Items["UpdateDocumentList"] = del;
        //    }
        //    else
        //    {
        //        del = (UpdateDocumentList)HttpContext.Current.Items["UpdateDocumentList"];
        //        del += new UpdateDocumentList(SetUpdateDocumentList);
        //        HttpContext.Current.Items["UpdateDocumentList"] = del;
        //    }
        //}

        #endregion

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	


        //public void SetUpdateDocumentList(List<ApplicantDocuments> updatedList)
        //{
        //    CurrentViewContext.lstApplicantDocument = updatedList;
        //}

        public void LoadApplicantPanel(bool IsFirstTime)
        {
            //Seting properties of usercontrol Applicant Panel            
            Presenter.GetApplicantDocuments();
            ucApplicantPanel.SelectedComplianceCategoryId_Global = this.SelectedComplianceCategoryId_Global;
            ucApplicantPanel.CategoryId = (IsFirstTime) ? this.CategoryId : ucApplicantPanel.CategoryId;
            ucApplicantPanel.ItemDataId_Global = (IsFirstTime) ? this.ItemDataId_Global : ucApplicantPanel.ItemDataId_Global;
            ucApplicantPanel.TenantId_Global = this.TenantId_Global;
            ucApplicantPanel.PackageId = (IsFirstTime) ? this.PackageId : ucApplicantPanel.PackageId;
            ucApplicantPanel.SelectedPackageSubscriptionID_Global = (IsFirstTime) ? this.SelectedPackageSubscriptionID_Global : ucApplicantPanel.SelectedPackageSubscriptionID_Global;
            ucApplicantPanel.OrganizationUserData = this.OrganizationData;
            ucApplicantPanel.CurrentApplicantId_Global = this.CurrentApplicantId_Global;
            if (this.IsPostBack)
            {
                ucApplicantPanel.SetPageDataAndLayout(true);
            }
        }

        public void LoadDataItemPanel(Dictionary<string, string> args)
        {
            this.CurrentApplicantId_Global = (args.ContainsKey("CurrentApplicantId_Global")) ? Convert.ToInt32(args["CurrentApplicantId_Global"]) : this.CurrentApplicantId_Global;
            this.CurrentCompliancePackageId_Global = (args.ContainsKey("PackageId")) ? Convert.ToInt32(args["PackageId"]) : this.PackageId;
            this.SelectedPackageSubscriptionID_Global = (args.ContainsKey("SelectedPackageSubscriptionID_Global")) ? Convert.ToInt32(args["SelectedPackageSubscriptionID_Global"]) : this.SelectedPackageSubscriptionID_Global;
            this.CurrentTenantId_Global = (args.ContainsKey("CurrentTenantId_Global")) ? Convert.ToInt32(args["CurrentTenantId_Global"]) : this.CurrentTenantId_Global;
            this.ItemDataId_Global = (args.ContainsKey("ItemDataId_Global")) ? Convert.ToInt32(args["ItemDataId_Global"]) : this.ItemDataId_Global;
            this.SelectedComplianceCategoryId_Global = (args.ContainsKey("SelectedComplianceCategoryId_Global")) ? Convert.ToInt32(args["SelectedComplianceCategoryId_Global"]) : this.SelectedComplianceCategoryId_Global;
            this.TenantId_Global = (args.ContainsKey("TenantId_Global")) ? Convert.ToInt32(args["TenantId_Global"]) : this.TenantId_Global;
            SetItemScreenIds();
            LoadDocumentPanel();
            if (this.IsPostBack)
            {
                ucItemDataPanel.LoadItems();
            }
            ucItemDataPanel.IsException = IsException;
        }

        void SetItemScreenIds()
        {
            ucItemDataPanel.CurrentTenantId_Global = this.CurrentTenantId_Global;
            ucItemDataPanel.SelectedTenantId_Global = this.TenantId_Global;
            ucItemDataPanel.CurrentPackageSubscriptionID_Global = this.SelectedPackageSubscriptionID_Global;
            ucItemDataPanel.SelectedCompliancePackageId_Global = this.CurrentCompliancePackageId_Global;
            ucItemDataPanel.SelectedComplianceCategoryId_Global = this.SelectedComplianceCategoryId_Global;
            ucItemDataPanel.SelectedApplicantId_Global = this.CurrentApplicantId_Global;
            ucItemDataPanel.CurrentLoggedInUserName_Global = this.LoggedInUserName_Global;
            ucItemDataPanel.ItemDataId_Global = this.ItemDataId_Global;
            ucItemDataPanel.PackageId_Global = this.PackageId;
            ucItemDataPanel.CategoryId_Global = this.CategoryId;
            ucItemDataPanel.lstApplicantDocument = this.lstApplicantDocument;
            ucItemDataPanel.LoggedInUserInitials_Global = this.LoggedInUserInitials_Global;
        }

        public void LoadDocumentPanel()
        {
            ucDocumentPanel.SelectedComplianceCategoryId_Global = this.SelectedComplianceCategoryId_Global;
            ucDocumentPanel.ItemDataId = this.ItemDataId_Global;
            ucDocumentPanel.SelectedTenantId = this.TenantId_Global;
            ucDocumentPanel.PackageSubscriptionId = this.SelectedPackageSubscriptionID_Global;
            ucDocumentPanel.OrganizationUserId = this.CurrentApplicantId_Global;
            ucDocumentPanel.lstApplicantDocument = this.lstApplicantDocument;
            if (IsPostBack)
                ucDocumentPanel.BindData();

        }

        /// <summary>
        /// To route Page Back
        /// </summary>
        /// <param name="showSuccessMessage"></param>
        private void RoutePageBack(bool showSuccessMessage, String successMessage = null)
        {
            Dictionary<String, String> queryString = null;


            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(this.TenantId_Global) },
                                                                    { "Child", @"~\ComplianceOperations\DataReconciliationQueue.ascx"},
                                                                    {"PackageId",this.PackageId.ToString()},
                                                                    {"CategoryId",this.CategoryId.ToString()},
                                                                 };

            String _url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            lnkGoBack.HRef = _url;
			
			Dictionary<String, String> queryStringVerificationDetailView = new Dictionary<String, String>();
			queryStringVerificationDetailView = new Dictionary<String, String>
																{
																{ "TenantId", Convert.ToString(this.TenantId_Global) },
																{ "Child", ChildControls.VerificationDetailsNew},
																{ "ItemDataId", Convert.ToString(this.ItemDataId_Global)},
																{"IsException","false"},
																{"WorkQueueType",WorkQueueType.ReconciliationDetail.ToString()},
																{"PackageId",this.PackageId.ToString()},
																{"CategoryId",this.CategoryId.ToString()},
																{"SelectedPackageSubscriptionId",Convert.ToString(this.SelectedPackageSubscriptionID_Global)},
																{"SelectedComplianceCategoryId",this.CategoryId.ToString()},
																{"ApplicantId",Convert.ToString(this.CurrentApplicantId_Global)},
																{"ComplianceItemReconciliationDataID",Convert.ToString(this.ComplianceItemReconciliationDataID_Global)},
																};
			string url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryStringVerificationDetailView.ToEncryptedQueryString());
			lnkVerificationDetailView.HRef = url;
		

		}

        private String RedirectToReconciliationDetailScreen(DataReconciliationQueueContract ReconcilictionData)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                    {
                                                        { "TenantId",Convert.ToString(ReconcilictionData.TenantId)},
                                                        { "Child", @"~\ComplianceOperations\UserControl\ReconciliationDetail.ascx"},
                                                        { "ItemDataId", Convert.ToString(ReconcilictionData.ApplicantComplianceItemId)},
                                                        {"PackageId",Convert.ToString(ReconcilictionData.PackageID)},
                                                        {"CategoryId",Convert.ToString(ReconcilictionData.CategoryID)},
                                                        {"SelectedComplianceCategoryId",Convert.ToString(ReconcilictionData.CategoryID)},
                                                        {"SelectedPackageSubscriptionId",Convert.ToString(ReconcilictionData.PackageSubscriptionID)},
                                                        {"ApplicantId",Convert.ToString(ReconcilictionData.ApplicantId)},
                                                        {"ComplianceItemReconciliationDataID",Convert.ToString(ReconcilictionData.FlatComplianceItemReconciliationDataID)},
                                                        //{"institutionIds",CurrentViewContext.SelectedInstitutionIds}
                                                    };
            return String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
        }
    }
}

