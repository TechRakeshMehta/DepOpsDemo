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
using System.Threading;
using System.Web.Configuration;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class VerificationDetail : BaseUserControl, IVerificationDetailView
    {
        #region Variables

        #region Private Variables

        private Int32 _itemId;
        private Int32 _attributesPerRow;
        private VerificationDetailPresenter _presenter=new VerificationDetailPresenter();
        //private ApplicantComplianceAttributeDataContract _viewContract;
        private Int32 _tenantId;
        private Int32 _selectedTenantId;
        private String _viewType;
        private CustomPagingArgsContract _verificationGridCustomPaging = null;
        private CustomPagingArgsContract _exceptionGridCustomPaging = null;

        #endregion

        #endregion

        #region Properties

        
        public VerificationDetailPresenter Presenter
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

        public IVerificationDetailView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Get or Set the Tenant ID
        /// </summary>
        public Int32 TenantId
        {
            get
            {
                if (_tenantId == 0)
                    _tenantId = Tenant.TenantID;
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        public Entity.Tenant Tenant
        {
            get
            {
                if (ViewState["Tenant"] == null)
                    ViewState["Tenant"] = Presenter.GetTenant();
                return (Entity.Tenant)ViewState["Tenant"];
            }
        }



        //public List<ComplianceAttribute> lstComplianceAttribute
        //{
        //    get
        //    {
        //        return CurrentViewContext.ClientComplianceItem.ComplianceItemAttributes.Select(x => x.ComplianceAttribute).ToList();
        //    }
        //}

        public ApplicantComplianceItemData ApplicantComplianceItem
        {
            get
            {
                if (ViewState["ApplicantComplianceItem"] != null)
                    return (ApplicantComplianceItemData)ViewState["ApplicantComplianceItem"];
                return null;
            }
            set
            {
                ViewState["ApplicantComplianceItem"] = value;
                StatusCode = value.lkpItemComplianceStatu != null ? value.lkpItemComplianceStatu.Code : String.Empty;
            }
        }

        public String StatusCode
        {
            get
            {
                if (ViewState["StatusCode"] != null)
                    return (String)ViewState["StatusCode"];
                return String.Empty;
            }
            set
            {
                ViewState["StatusCode"] = value;
            }
        }

        public Int32 ItemDataId
        {
            get { return (Int32)(ViewState["ItemDataId"]); }
            set { ViewState["ItemDataId"] = value; }
        }

        public String ItemName
        {
            get;
            set;
        }

        public Int32 SelectedTenantId
        {
            get { return (Int32)(ViewState["SelectedTenantId"]); }
            set { ViewState["SelectedTenantId"] = value; }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Dictionary<Int32, Int32> AttributeDocuments
        {
            get;
            set;
        }

        //public String AttributeHtml { get; set; }

        public List<ApplicantComplianceAttributeData> lstApplicantComplianceAttributeData
        {
            get
            {
                if (ViewState["lstApplicantComplianceAttributeData"] != null)
                    return (List<ApplicantComplianceAttributeData>)(ViewState["lstApplicantComplianceAttributeData"]);
                return null;
            }
            set
            {
                ViewState["lstApplicantComplianceAttributeData"] = value;
                documentSection.Visible = IsVisibleDocumentSection;

            }
        }

        public Boolean IsVisibleDocumentSection
        {
            get
            {
                if (lstApplicantComplianceAttributeData == null)
                    return false;
                return lstApplicantComplianceAttributeData.Any(x => x.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower() && !x.IsDeleted);
            }
        }

        public Int32? FileUploadAttributeId
        {
            get
            {
                if (lstApplicantComplianceAttributeData != null)
                {
                    ApplicantComplianceAttributeData attributeData = lstApplicantComplianceAttributeData.FirstOrDefault(x => x.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower() && !x.IsDeleted);
                    if (attributeData != null)
                        return attributeData.ApplicantComplianceAttributeID;
                }
                return null;
            }
        }

        public List<ApplicantComplianceDocumentMap> lstApplicantComplianceDocumentMaps
        {
            get
            {
                if (ViewState["lstApplicantComplianceDocumentMaps"] != null)
                    return (List<ApplicantComplianceDocumentMap>)(ViewState["lstApplicantComplianceDocumentMaps"]);
                return null;
            }
            set
            {
                ViewState["lstApplicantComplianceDocumentMaps"] = value;
            }
        }



        public Entity.OrganizationUser OrganizationUserData
        {
            get
            {
                if (ViewState["OrganizationUserData"] != null)
                    return (Entity.OrganizationUser)ViewState["OrganizationUserData"];
                return null;
            }
            set
            {
                ViewState["OrganizationUserData"] = value;
            }
        }

        public String OrganizationUserName
        {
            get { return (String)(ViewState["OrganizationUserName"]); }
            set { ViewState["OrganizationUserName"] = value; }
        }

        public Entity.Tenant TenantData { get; set; }

        public Int32 CurrentStatusId { get; set; }

        public String CurrentStatusCode { get; set; }

        public String Comments { get; set; }

        public Int16 ClientAdminId
        {
            get { return (Int16)(ViewState["ClientAdminId"]); }
            set { ViewState["ClientAdminId"] = value; }
        }

        public List<ApplicantDocument> lstApplicantDocument { get; set; }

        public Boolean MoveOnNextRec
        {
            get
            {
                return WorkQueue != WorkQueueType.DataItemSearch;
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
                if (this.WorkQueue == WorkQueueType.AssignmentWorkQueue)
                    return ChildControls.VerificationQueue;
                else if (this.WorkQueue == WorkQueueType.DataItemSearch)
                    return ChildControls.DatatItemSearch;
                else if (this.WorkQueue == WorkQueueType.AssigneeDataItemSearch)
                    return ChildControls.AssigneeDatatItemSearch;
                return ChildControls.UserWorkQueue;
            }
        }

        /// <summary>
        /// set AssignedToUser for return back to queue.
        /// </summary>
        public Int32 AssignesToUser
        {
            get
            {
                if (!ViewState["AssignesToUser"].IsNull())
                {
                    return (Int32)ViewState["AssignesToUser"];
                }
                return 0;
            }
            set
            {
                ViewState["AssignesToUser"] = value;
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

        public Int32 SelectedProgramStudyId
        {
            get
            {
                if (!ViewState["SelectedProgramStudyId"].IsNull())
                    return (Int32)ViewState["SelectedProgramStudyId"];
                return 0;
            }
            set
            {
                ViewState["SelectedProgramStudyId"] = value;
            }
        }

        public String ApplicantFirstName
        {
            get
            {
                if (!ViewState["ApplicantFirstName"].IsNull())
                    return (String)ViewState["ApplicantFirstName"];
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
                    return (String)ViewState["ApplicantLastName"];
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
                    return (DateTime)ViewState["DateOfBirth"];
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
                    return (String)ViewState["ItemLabel"];
                return String.Empty;
            }
            set
            {
                ViewState["ItemLabel"] = value;
            }
        }

        public String SelectedItemComplianceStatusId
        {
            get
            {
                if (!ViewState["SelectedItemComplianceStatusId"].IsNull())
                    return (String)ViewState["SelectedItemComplianceStatusId"];
                return String.Empty;
            }
            set
            {
                ViewState["SelectedItemComplianceStatusId"] = value;
            }
        }

        public string UIInputException { get; set; }

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


        /// <summary>
        /// set package id for return back to queue.
        /// </summary>
        public Int32 PackageId
        {
            get
            {
                return (Int32)ViewState["PackageId"];
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
                return (Int32)ViewState["CategoryId"];
            }
            set
            {
                ViewState["CategoryId"] = value;
            }
        }

        public Int32 NextItemDataId
        {
            get;
            set;
        }
        //public String Notes { get; set; }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract VerificationGridCustomPaging
        {
            get
            {
                if (_verificationGridCustomPaging.IsNull())
                {
                    var serializer = new XmlSerializer(typeof(CustomPagingArgsContract));
                    TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.VERIFICATION_QUEUE_SESSION_KEY]));
                    _verificationGridCustomPaging = (CustomPagingArgsContract)serializer.Deserialize(reader);
                }
                return _verificationGridCustomPaging;
            }
        }


        /// <summary>
        ///get and  set package id .
        /// </summary>
        public Int32 CompliancePackageId
        {
            get { return (Int32)(ViewState["CompliancePackageId"]); }
            set { ViewState["CompliancePackageId"] = value; }
        }

        /// <summary>
        ///get and  set Category id 
        /// </summary>
        public Int32 ComplianceCategoryId
        {
            get { return (Int32)(ViewState["ComplianceCategoryId"]); }
            set { ViewState["ComplianceCategoryId"] = value; }
        }

        /// <summary>
        ///get and  set Category id 
        /// </summary>
        public Int32 ComplianceItemId
        {
            get { return (Int32)(ViewState["ComplianceItemId"]); }
            set { ViewState["ComplianceItemId"] = value; }
        }

        /// <summary>
        ///get and  set Applicant id 
        /// </summary>
        public Int32 ApplicantId
        {
            get { return (Int32)(ViewState["ApplicantId"]); }
            set { ViewState["ApplicantId"] = value; }
        }

        Boolean isVisibleThirdPartyReviewButton = false;
        private Boolean IsVisibleThirdPartyReviewButton
        {
            get
            {
                if (ViewState["IsVisibleThirdPartyReviewButton"] == null)
                    ViewState["IsVisibleThirdPartyReviewButton"] = Presenter.IsSendForThirdPartyReview;
                return (Boolean)(ViewState["IsVisibleThirdPartyReviewButton"]);
            }
        }

        public String LastViewedDocumentPath
        {
            get
            {
                if (ViewState["LastViewedDocumentPath"] != null)
                    return (String)ViewState["LastViewedDocumentPath"];
                return String.Empty;
            }
            set
            {
                ViewState["LastViewedDocumentPath"] = value;
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

        public Boolean IsIframeSrcRetain
        {
            get
            {
                if (!String.IsNullOrEmpty(LastViewedDocumentPath))
                {
                    String extension = Path.GetExtension(LastViewedDocumentPath);
                    switch (extension)
                    {
                        case ".pdf":
                            return true;
                        case ".swf":
                            return true;
                        case ".gif":
                            return true;
                        case ".jpeg":
                        case ".jpg":
                            return true;
                        case ".png":
                            return true;
                        case ".txt":
                            return true;
                        default:
                            return false;
                    }
                }
                return false;
            }
        }

        private String GetContentType(String fileExtension)
        {
            switch (fileExtension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".swf":
                    return "application/x-shockwave-flash";
                case ".gif":
                    return "image/gif";
                case ".jpeg":
                case ".jpg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".txt":
                    return "text/plain";
                default:
                    return "application/octet-stream";
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// OnInit event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                cbbuttons.SaveButton.CausesValidation = false;
                cbbuttons.CancelButton.CausesValidation = false;
                // IsVisibleThirdPartyReviewButton;

                //base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];




                base.OnInit(e);
                base.Title = "Verification Detail";
                BasePage basePage = base.Page as BasePage;
                if (basePage != null)
                {
                    basePage.HideTitleBars();
                }
                //lblVerificationQueue.Text = base.Title;
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
        /// Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    CaptureQuerystringParameters(args);
                }

                //SelectedTenantId = ViewState["SelectedTenantId"].IsNull() ? SelectedTenantId : (Int32)(ViewState["SelectedTenantId"]);

            }
            SetPageDataAndLayout(!this.IsPostBack);
            ucUploadDocuments.OnCompletedUpload -= new UploadDocuments.UploadDelegate(ucUploadDocuments_OnCompletedUpload);
            ucUploadDocuments.OnCompletedUpload += new UploadDocuments.UploadDelegate(ucUploadDocuments_OnCompletedUpload);
            ucUploadDocuments.TenantID = SelectedTenantId;
            ucUploadDocuments.OrganiztionUserID = OrganizationUserData.OrganizationUserID;
            if (!IsIframeSrcRetain)
                iframe.Attributes.Add("src", "");
            Presenter.OnViewLoaded();

            hiddenuploader.MaxFileSize = Convert.ToInt32(WebConfigurationManager.AppSettings[AppConsts.MAXIMUM_FILE_SIZE]);
        }

        private void SetPageDataAndLayout(Boolean GetFreshData)
        {
            if (GetFreshData)
            {
                Presenter.OnViewInitialized();
                BindAttributes();
                grdDocuments.Rebind();
            }
            DisplayButtons();
        }

        private void CaptureQuerystringParameters(Dictionary<String, String> args)
        {
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey("SelectedTenantId"))
            {
                SelectedTenantId = Convert.ToInt32(args["SelectedTenantId"]);
                //ViewState["SelectedTenantId"] = SelectedTenantId;
            }

            if (args.ContainsKey("ItemDataId"))
            {
                ItemDataId = Convert.ToInt32(args["ItemDataId"]);
            }

            if (args.ContainsKey("WorkQueueType"))
            {
                WorkQueue = (WorkQueueType)Enum.Parse(typeof(WorkQueueType), args["WorkQueueType"].ToString(), true);
            }
            if (args.ContainsKey("PackageId"))
            {
                PackageId = Convert.ToInt32(args["PackageId"]);
            }
            if (args.ContainsKey("CategoryId"))
            {
                CategoryId = Convert.ToInt32(args["CategoryId"]);
            }
            if (args.ContainsKey("AssignedToVerUser"))
            {
                AssignesToUser = Convert.ToInt32(args["AssignedToVerUser"]);
            }
            if (args.ContainsKey("IncludeIncompleteItems"))
            {
                IncludeIncompleteItems = Convert.ToBoolean(args["IncludeIncompleteItems"]);
            }
            if (args.ContainsKey("SelectedProgramStudyId"))
            {
                SelectedProgramStudyId = Convert.ToInt32(args["SelectedProgramStudyId"]);
            }
            if (args.ContainsKey("ApplicantFirstName"))
            {
                ApplicantFirstName = args["ApplicantFirstName"];
            }
            if (args.ContainsKey("ApplicantLastName"))
            {
                ApplicantLastName = args["ApplicantLastName"];
            }
            DateTime dob;
            if (args.ContainsKey("DateOfBirth") && DateTime.TryParse(args["DateOfBirth"], out dob))
            {
                DateOfBirth = dob;
            }

            if (args.ContainsKey("ItemLabel"))
            {
                ItemLabel = args["ItemLabel"];
            }

            if (args.ContainsKey("SelectedItemComplianceStatusId"))
            {
                SelectedItemComplianceStatusId = args["SelectedItemComplianceStatusId"];
            }
        }

        protected void grdDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            Presenter.GetApplicantDocuments();
            if (CurrentViewContext.lstApplicantDocument != null && FileUploadAttributeId != null)
            {
                foreach (ApplicantDocument document in CurrentViewContext.lstApplicantDocument)
                {
                    document.IsMapped =
                        document.ApplicantComplianceDocumentMaps != null
                        && (document.ApplicantComplianceDocumentMaps.Count(x => !x.IsDeleted && x.ApplicantComplianceAttributeID == FileUploadAttributeId) > 0);
                }
            }
            grdDocuments.DataSource = CurrentViewContext.lstApplicantDocument.OrderByDescending(x => x.IsMapped);
        }

        /// <summary>
        /// Approve
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarApprove_Click(Object sender, EventArgs e)
        {
            try
            {
                if (!UpdateApplicantComplianceAttributeData())
                    return;

                UpdateApplicantComplianceDocumentMaps();

                if (!(txtCommentsNew.Text.IsNullOrEmpty()))
                {
                    Comments = Environment.NewLine + "[" + OrganizationUserName + " " + DateTime.Now.Date.ToShortDateString() + "] " + txtCommentsNew.Text;
                }
                else
                {
                    Comments = String.Empty;
                }

                if (ClientAdminId > 0)
                {//case Admin send to client
                    CurrentViewContext.CurrentStatusCode = ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue();

                    if (MoveOnNextRec)
                        Presenter.getNextRecordData();
                    UpdateItemDataStatus();
                    ShowNextPage("sent for client review");

                }
                else
                {
                    String status = String.Empty;
                    if (Presenter.IsDefaultTenant)
                    {
                        if (CurrentViewContext.ReviewerUserId > 0)
                        {
                            //case Admin send to third party 
                            CurrentViewContext.CurrentStatusCode = ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue();
                            status = "Further Review(Send To Third Party)";
                        }
                        else
                        {//case ADmin approve
                            CurrentViewContext.CurrentStatusCode = ApplicantItemComplianceStatus.Approved.GetStringValue();
                            status = "approved";
                        }

                    }
                    else if (!Presenter.IsDefaultTenant && !Presenter.IsThirdPartyTenant)
                    {//case client admin approve
                        CurrentViewContext.CurrentStatusCode = ApplicantItemComplianceStatus.Approved.GetStringValue();
                        status = "approved";
                    }
                    else if (Presenter.IsThirdPartyTenant)
                    { //case Third party approve
                        CurrentViewContext.CurrentStatusCode = ApplicantItemComplianceStatus.Approved.GetStringValue();
                        status = "approved";
                    }
                    if (MoveOnNextRec)
                        Presenter.getNextRecordData();
                    UpdateItemDataStatus();
                    ShowNextPage(status);
                }

            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        public void ShowNextPage(String Message)
        {
            if (MoveOnNextRec && CurrentViewContext.NextItemDataId > 0)
            {
                ResetProperties();
                CurrentViewContext.ItemDataId = CurrentViewContext.NextItemDataId;
                SetPageDataAndLayout(GetFreshData: true);
            }
            else
            {
                RoutePageBack(true, Message);
            }
        }


        /// <summary>
        /// Reject Alert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReject_Click(Object sender, EventArgs e)
        {
            try
            {
                if (!UpdateApplicantComplianceAttributeData())
                    return;
                UpdateApplicantComplianceDocumentMaps();

                CurrentViewContext.CurrentStatusCode = ApplicantItemComplianceStatus.Not_Approved.GetStringValue();
                if (!(txtCommentsNew.Text.IsNullOrEmpty()))
                {
                    Comments = Environment.NewLine + "[" + OrganizationUserName + " " + DateTime.Now.Date.ToShortDateString() + "] " + txtCommentsNew.Text;
                }
                else
                {
                    Comments = String.Empty;
                }

                if (MoveOnNextRec)
                    Presenter.getNextRecordData();
                UpdateItemDataStatus();
                ShowNextPage("rejected");
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }


        protected void CmdBarThirdPartyReview_Click(object sender, EventArgs e)
        {
            try
            {
                if (!UpdateApplicantComplianceAttributeData())
                    return;

                UpdateApplicantComplianceDocumentMaps();

                if (!(txtCommentsNew.Text.IsNullOrEmpty()))
                {
                    Comments = Environment.NewLine + "[" + OrganizationUserName + " " + DateTime.Now.Date.ToShortDateString() + "] " + txtCommentsNew.Text;
                }
                else
                {
                    Comments = String.Empty;
                }

                CurrentViewContext.CurrentStatusCode = ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue();
                if (MoveOnNextRec)
                    Presenter.getNextRecordData();
                UpdateItemDataStatus();
                ShowNextPage("Sent for third party review");
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }


        /// <summary>
        /// Cancel Alert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarCancel_Click(Object sender, EventArgs e)
        {

            try
            {
                RoutePageBack(false);

                //if (!ReturnURL.IsNullOrWhiteSpace())
                //{
                //    Response.Redirect(ReturnURL, false);
                //}
                //else
                //{
                //Response.Redirect(AppConsts.SYSX_DASHBOARD, false);
                //}
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }



        protected void grdAttributes_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                ApplicantComplianceAttributeData attributeData = (ApplicantComplianceAttributeData)e.Item.DataItem;

                String dataTypeCode = attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower().Trim();

                WclTextBox txtDataType = (WclTextBox)e.Item.FindControl("txtDataType");
                txtDataType.Text = dataTypeCode;

                if (dataTypeCode == ComplianceAttributeDatatypes.Date.GetStringValue().ToLower().Trim())
                {
                    WclDatePicker dtPicker = (WclDatePicker)e.Item.FindControl("dtPicker");
                    if (!String.IsNullOrEmpty(attributeData.AttributeValue) && !DateTime.MinValue.ToShortDateString().Equals(attributeData.AttributeValue))
                        dtPicker.SelectedDate = Convert.ToDateTime(attributeData.AttributeValue);
                    dtPicker.Visible = true;
                }
                else if (dataTypeCode == ComplianceAttributeDatatypes.Text.GetStringValue().ToLower().Trim())
                {
                    WclTextBox txtBox = (WclTextBox)e.Item.FindControl("txtBox");
                    txtBox.Text = attributeData.AttributeValue;
                    txtBox.MaxLength = attributeData.ComplianceAttribute.MaximumCharacters == null ? 50 : Convert.ToInt32(attributeData.ComplianceAttribute.MaximumCharacters);
                    txtBox.Visible = true;
                }
                else if (dataTypeCode == ComplianceAttributeDatatypes.Numeric.GetStringValue().ToLower().Trim())
                {
                    WclNumericTextBox numericTextBox = (WclNumericTextBox)e.Item.FindControl("numericTextBox");
                    numericTextBox.Text = attributeData.AttributeValue;
                    numericTextBox.Visible = true;
                }
                else if (dataTypeCode == ComplianceAttributeDatatypes.Options.GetStringValue().ToLower().Trim())
                {
                    WclComboBox optionCombo = (WclComboBox)e.Item.FindControl("optionCombo");
                    if (attributeData.ComplianceAttribute.ComplianceAttributeOptions != null)
                    {
                        List<ComplianceAttributeOption> lst = attributeData.ComplianceAttribute.ComplianceAttributeOptions.Where(opt => !opt.IsDeleted && opt.IsActive).ToList();

                        foreach (var attributeOption in lst)
                            optionCombo.Items.Add(new RadComboBoxItem(attributeOption.OptionText, attributeOption.OptionValue));
                    }

                    optionCombo.Items.Insert(0, new RadComboBoxItem(AppConsts.COMBOBOX_ITEM_SELECT, AppConsts.ZERO));
                    optionCombo.SelectedValue = attributeData.AttributeValue;
                    optionCombo.Visible = true;
                }

            }
        }

        protected void ucUploadDocuments_OnCompletedUpload()
        {
            if (ucUploadDocuments.ToSaveApplicantUploadedDocuments != null && ucUploadDocuments.ToSaveApplicantUploadedDocuments.Count > 0)
            {
                List<ApplicantComplianceDocumentMap> mappedDocument = new List<ApplicantComplianceDocumentMap>();
                foreach (ApplicantDocument document in ucUploadDocuments.ToSaveApplicantUploadedDocuments)
                    mappedDocument.Add(InstanceOfDocumentMap(document.ApplicantDocumentID));

                Presenter.UpdateApplicantComplianceDocumentMaps(mappedDocument, null);
                Presenter.GetApplicantComplianceData();
                grdDocuments.Rebind();
            }
        }

        protected void grdDocuments_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "DocumentViewer")
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                Int32 applicantDocumentId = Convert.ToInt32(dataItem.GetDataKeyValue("ApplicantDocumentID"));
                LastViewedDocumentPath = Convert.ToString(dataItem.GetDataKeyValue("DocumentPath"));

                iframe.Attributes.Add("src", String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?documentId={0}&tenantId={1}", applicantDocumentId, SelectedTenantId));
            }
        }

        #endregion

        #region Methods

        #region Private Methods




        private void BindAttributes()
        {
            List<ApplicantComplianceAttributeData> lstAttributeData = new List<ApplicantComplianceAttributeData>();
            if (lstApplicantComplianceAttributeData != null)
            {
                foreach (ApplicantComplianceAttributeData attributeData in lstApplicantComplianceAttributeData)
                {
                    if (attributeData.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower())
                        continue;
                    lstAttributeData.Add(attributeData);
                }
            }
            grdAttributes.DataSource = lstAttributeData;
            grdAttributes.DataBind();
            txtName.Text = OrganizationUserData.FirstName + " " + OrganizationUserData.LastName;
            txtDOB.Text = OrganizationUserData.DOB.HasValue? Convert.ToDateTime(OrganizationUserData.DOB.Value).ToShortDateString():String.Empty;
            txtGender.Text = OrganizationUserData.lkpGender != null ? OrganizationUserData.lkpGender.GenderName : String.Empty;
            txtPhone.Text = OrganizationUserData.PhoneNumber;
            txtEmail.Text = OrganizationUserData.aspnet_Users.aspnet_Membership.Email;
            txtItemName.Text = ApplicantComplianceItem.ComplianceItem.Name;
            txtTenantName.Text = TenantData.TenantName;
            txtComments.Text = ApplicantComplianceItem.VerificationComments;
            txtNotes.Text = ApplicantComplianceItem.Notes;
            txtCommentsNew.Text = String.Empty;
        }


        /// <summary>
        /// To update Item Data Status
        /// </summary>
        private void UpdateItemDataStatus()
        {
            Presenter.UpdateItemDataStatus();
        }

        private Boolean UpdateApplicantComplianceAttributeData()
        {
            if (lstApplicantComplianceAttributeData.IsNotNull())
            {
                foreach (GridDataItem item in grdAttributes.Items)
                {
                    if (item.GetDataKeyValue("ApplicantComplianceAttributeID") != null)
                    {
                        Int32 applicantComplianceAttributeID = Convert.ToInt32(item.GetDataKeyValue("ApplicantComplianceAttributeID"));

                        ApplicantComplianceAttributeData attributeData = lstApplicantComplianceAttributeData.FirstOrDefault(x => x.ApplicantComplianceAttributeID == applicantComplianceAttributeID);
                        if (attributeData != null)
                        {
                            String dataTypeCode = ((WclTextBox)item.FindControl("txtDataType")).Text;

                            if (dataTypeCode == ComplianceAttributeDatatypes.Date.GetStringValue().ToLower().Trim())
                            {
                                WclDatePicker dtPicker = (WclDatePicker)item.FindControl("dtPicker");
                                attributeData.AttributeValue = dtPicker.SelectedDate.ToString();

                            }
                            else if (dataTypeCode == ComplianceAttributeDatatypes.Text.GetStringValue().ToLower().Trim())
                            {
                                WclTextBox txtBox = (WclTextBox)item.FindControl("txtBox");
                                attributeData.AttributeValue = txtBox.Text;
                            }
                            else if (dataTypeCode == ComplianceAttributeDatatypes.Numeric.GetStringValue().ToLower().Trim())
                            {
                                WclNumericTextBox numericTextBox = (WclNumericTextBox)item.FindControl("numericTextBox");
                                attributeData.AttributeValue = numericTextBox.Text;
                            }
                            else if (dataTypeCode == ComplianceAttributeDatatypes.Options.GetStringValue().ToLower().Trim())
                            {
                                WclComboBox optionCombo = (WclComboBox)item.FindControl("optionCombo");
                                attributeData.AttributeValue = optionCombo.SelectedValue;
                            }
                            attributeData.ModifiedOn = DateTime.Now;
                            attributeData.ModifiedByID = CurrentLoggedInUserId;
                        }
                    }
                }
                Presenter.UpdateApplicantComplianceAttributeData();
                if (!String.IsNullOrEmpty(CurrentViewContext.UIInputException))
                {
                    lblError.Text = CurrentViewContext.UIInputException;
                    return false;
                }
                return true;
            }
            return true;
        }

        private void UpdateApplicantComplianceDocumentMaps()
        {
            if (!IsVisibleDocumentSection) return;
            List<ApplicantComplianceDocumentMap> ToAddDocumentMap = new List<ApplicantComplianceDocumentMap>();
            List<Int32> ToDeleteApplicantComplianceDocumentMapIDs = new List<Int32>();

            foreach (GridDataItem item in grdDocuments.Items)
            {
                Int32 applicantDocumentId = Convert.ToInt32(item.GetDataKeyValue("ApplicantDocumentID"));
                CheckBox chkDocument = (CheckBox)item.FindControl("chkDocument");

                ApplicantComplianceDocumentMap documentMap = lstApplicantComplianceDocumentMaps.FirstOrDefault(x => x.ApplicantDocumentID == applicantDocumentId);

                if (documentMap == null && chkDocument.Checked)            // Add new document
                    ToAddDocumentMap.Add(InstanceOfDocumentMap(applicantDocumentId));
                else if (documentMap != null && !chkDocument.Checked)
                    ToDeleteApplicantComplianceDocumentMapIDs.Add(documentMap.ApplicantComplianceDocumentMapID);
            }

            Presenter.UpdateApplicantComplianceDocumentMaps(ToAddDocumentMap, ToDeleteApplicantComplianceDocumentMapIDs);
        }

        private ApplicantComplianceDocumentMap InstanceOfDocumentMap(Int32 applicantDocumentId)
        {
            var applicantComplianceAttribute = lstApplicantComplianceAttributeData.Where(x => x.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower() && !x.IsDeleted).FirstOrDefault();

            return
                new ApplicantComplianceDocumentMap()
                {
                    ApplicantComplianceAttributeID = applicantComplianceAttribute.ApplicantComplianceAttributeID,
                    ApplicantDocumentID = applicantDocumentId,
                    IsDeleted = false,
                    CreatedByID = CurrentLoggedInUserId,
                    CreatedOn = DateTime.Now
                };
        }

        private void DisplayButtons()
        {

            cbbuttons.DisplayButtons = CommandBarButtons.Cancel;
            AssignmentProperty assignmentProperty = Presenter.GetAssignmentProperty();
            if (assignmentProperty != null)
            {
                List<lkpReviewerType> reviewerTypes = assignmentProperty.AssignmentPropertiesReviewers.Where(x => !x.IsDeleted).Select(x => x.lkpReviewerType).Where(x => !x.IsDeleted).ToList();
                //lkpReviewerType reviewerType = CurrentViewContext.ApplicantComplianceItem.lkpReviewerType;

                if (Presenter.IsDefaultTenant && StatusCode.Equals(ApplicantItemComplianceStatus.Pending_Review.GetStringValue()))
                {
                    cbbuttons.DisplayButtons = cbbuttons.DisplayButtons | CommandBarButtons.Submit; // Submit - Reject
                    if (reviewerTypes != null && reviewerTypes.Any(x => x.Code.Equals(ReviewerType.Admin.GetStringValue())))
                        cbbuttons.DisplayButtons = cbbuttons.DisplayButtons | CommandBarButtons.Save; // Save - Approve
                    if (reviewerTypes != null && reviewerTypes.Any(x => x.Code.Equals(ReviewerType.Client_Admin.GetStringValue())) && ClientAdminId > 0)
                    {
                        cbbuttons.DisplayButtons = cbbuttons.DisplayButtons | CommandBarButtons.Save;
                        cbbuttons.SaveButtonText = "Send for Client Review";

                    }
                    if (reviewerTypes != null && reviewerTypes.Any(x => x.Code.Equals(ReviewerType.Admin.GetStringValue())) && !reviewerTypes.Any(x => x.Code.Equals(ReviewerType.Client_Admin.GetStringValue())) && assignmentProperty.TPReviewerUserID != null)
                    {
                        cbbuttons.DisplayButtons = cbbuttons.DisplayButtons | CommandBarButtons.Save;
                        cbbuttons.SaveButtonText = "Further Review (Send To Third party)";
                    }
                }
                else if (!Presenter.IsDefaultTenant
                    && CurrentViewContext.Tenant.lkpTenantType.TenantTypeCode == TenantType.Institution.GetStringValue()
                    && StatusCode.Equals(ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue()))
                {
                    cbbuttons.DisplayButtons = cbbuttons.DisplayButtons | CommandBarButtons.Save | CommandBarButtons.Submit;
                    if (Presenter.IsSendForThirdPartyReview)
                        cbbuttons.DisplayButtons = cbbuttons.DisplayButtons | CommandBarButtons.Clear; // Send for third party
                }
                else if (!Presenter.IsDefaultTenant
                    && CurrentViewContext.Tenant.lkpTenantType.TenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue()
                    && StatusCode.Equals(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue())
                    && assignmentProperty.ReviewerTenantID == CurrentViewContext.TenantId
                    )
                {
                    cbbuttons.DisplayButtons = cbbuttons.DisplayButtons | CommandBarButtons.Save | CommandBarButtons.Submit;
                }
            }
            cbbuttons.ReloadButtonText();
        }


        /// <summary>
        /// To route Page Back
        /// </summary>
        /// <param name="showSuccessMessage"></param>
        private void RoutePageBack(bool showSuccessMessage, String successMessage = null)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(SelectedTenantId) },
                                                                    { "Child", WorkQueuePath},
                                                                    {"PackageId",PackageId.ToString()},
                                                                    {"CategoryId",CategoryId.ToString()},
                                                                    {"AssignedToVerUser",AssignesToUser.ToString()},
                                                                    {"IncludeIncompleteItems",IncludeIncompleteItems.ToString()},
                                                                    {"SelectedProgramStudyId",SelectedProgramStudyId.ToString()},
                                                                    {"ApplicantFirstName",ApplicantFirstName},
                                                                    {"ApplicantLastName",ApplicantLastName},
                                                                    {"DateOfBirth",DateOfBirth.ToString()},
                                                                    {"ItemLabel",ItemLabel},
                                                                    {"SelectedItemComplianceStatusId",SelectedItemComplianceStatusId}                                                                 
                                                                 };
            if (showSuccessMessage)
            {
                queryString.Add("UpdatedStatus", successMessage);
            }
            String url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        /// <summary>
        /// reset the properties while moving to next record
        /// </summary>
        private void ResetProperties()
        {
            iframe.Attributes.Add("src", "");
        }
        #endregion

        #endregion




    }
}

