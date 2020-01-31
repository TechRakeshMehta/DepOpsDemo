#region Namespaces

#region System Defined

using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using System.Web.Configuration;
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
using System.Threading;
using CoreWeb.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ExceptionDetail : BaseUserControl, IExceptionDetailView
    {
        #region Variables

        #region Private Variables

        // private Int32 _itemId;
        private ExceptionDetailPresenter _presenter=new ExceptionDetailPresenter();
        private Int32 _tenantId;
        // private Int32 _selectedTenantId;
        private String _viewType;
        private CustomPagingArgsContract _exceptionGridCustomPaging = null;
        #endregion

        #endregion

        #region Properties

        public ExceptionDetailPresenter Presenter
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

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>

        public IExceptionDetailView CurrentViewContext
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
                {
                    //_tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }


        public ApplicantComplianceItemData ApplicantComplianceItem
        {
            get;
            set;
        }

        public Int32 ItemDataId
        {
            get { return (Int32)(ViewState["ItemDataId"]); }
            set { ViewState["ItemDataId"] = value; }
        }

        public Int32 ApplicantComplianceCategoryID
        {
            get { return Convert.ToInt32(ViewState["ApplicantComplianceCategoryID"]); }
            set { ViewState["ApplicantComplianceCategoryID"] = value; }
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

        public List<ApplicantComplianceItemData> lstApplicantComplianceItems
        {
            get;
            set;
        }

        public Entity.OrganizationUser OrganizationUserData
        {
            get
            {
                if (!ViewState["OrganizationUserData"].IsNull())
                {
                    return (Entity.OrganizationUser)(ViewState["OrganizationUserData"]);
                }
                return new Entity.OrganizationUser();
            }
            set { ViewState["OrganizationUserData"] = value; }
        }

        public Entity.Tenant TenantData { get; set; }

        public Int32 CurrentStatusId { get; set; }

        public String CurrentStatusCode { get; set; }

        public String Comments { get; set; }

        public List<Int32> ListOfIdToRemoveDocument { get; set; }

        public List<ExceptionDocumentMapping> GetExceptionDocumentMappingList { get; set; }

        public List<ComplianceItem> itemIdList { get; set; }

        public Int32 SelectedItemId { get; set; }

        public List<Int32> ApplicantDocumnetIds { get; set; }
        public List<Int32> ListOfIdToAddDocument { get; set; }

        /// <summary>
        /// Property to set the value of workQueueType to return back on Queue.
        /// </summary>
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

        /// <summary>
        /// Set the path of Queue to return back.
        /// </summary>
        public String WorkQueuePath
        {
            get
            {
                if (this.WorkQueue == WorkQueueType.AssignmentWorkQueue)
                    return ChildControls.VerificationQueue;
                return ChildControls.UserWorkQueue;
            }
        }

        /// <summary>
        /// set package id for return back to queue.
        /// </summary>
        public Int32 PackageId
        {
            get
            {
                if (!ViewState["PackageId"].IsNull())
                {
                    return (Int32)ViewState["PackageId"];
                }
                return 0;
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
                if (!ViewState["CategoryId"].IsNull())
                {
                    return (Int32)ViewState["CategoryId"];
                }
                return 0;

            }
            set
            {
                ViewState["CategoryId"] = value;
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

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        public Dictionary<Int32, Boolean> SelectedItems
        {
            get
            {
                if (!ViewState["SelectedItems"].IsNull())
                {
                    return ViewState["SelectedItems"] as Dictionary<Int32, Boolean>;
                }
                return new Dictionary<Int32, Boolean>();
            }
            set
            {
                ViewState["SelectedItems"] = value;
            }
        }

        public Int32 NextItemDataId
        {
            get;
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract ExceptionGridCustomPaging
        {
            get
            {
                if (_exceptionGridCustomPaging.IsNull())
                {
                    //var serializer = new XmlSerializer(typeof(CustomPagingArgsContract));
                    //TextReader reader = new StringReader(Convert.ToString(Session[AppConsts.EXCEPTION_QUEUE_SESSION_KEY]));
                    _exceptionGridCustomPaging = (CustomPagingArgsContract)(Session[AppConsts.EXCEPTION_QUEUE_SESSION_KEY]);
                }
                return _exceptionGridCustomPaging;
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

        #endregion

        #region Events

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                //base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = "Exception Detail";
                cbbuttons.SubmitButton.ValidationGroup = "grpFormSubmit";
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
                if (!this.IsPostBack)
                {

                    Dictionary<String, String> args = new Dictionary<String, String>();

                    if (!Request.QueryString["args"].IsNull())
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
                        if (args.ContainsKey("AssignedToExpUser"))
                        {
                            AssignesToUser = Convert.ToInt32(args["AssignedToExpUser"]);
                        }
                        if (args.ContainsKey("IncludeIncompleteItems"))
                        {
                            IncludeIncompleteItems = Convert.ToBoolean(args["IncludeIncompleteItems"]);
                        }

                    }
                    Presenter.OnViewInitialized();
                    BindTexBoxes();
                    BindItemDropDown();
                    if (!(uploadControl.UploadedFiles.Count > 0))
                    {
                        HideButton();
                    }
                    //Set Module Title
                    BasePage basePage = base.Page as BasePage;
                    if (basePage != null)
                    {
                        //basePage.HideTitleBars();
                        Label lblModHdr = basePage.Form.FindControl("lblModHdr") as Label;
                        lblModHdr.Text = String.Empty;
                    }

                }
                uploadControl.MaxFileSize = Convert.ToInt32(WebConfigurationManager.AppSettings[AppConsts.MAXIMUM_FILE_SIZE]);
                if (!IsIframeSrcRetain)
                    iframe.Attributes.Add("src", "");
                Presenter.OnViewLoaded();
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

        #region Button Events

        /// <summary>
        /// Approve
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarApprove_Click(Object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.CurrentStatusCode = ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue();
                GetListOfIDToRemoveDocument();
                //CurrentViewContext.ListOfIdToRemoveDocument = GetListOfIDToRemoveDocument();
                CurrentViewContext.SelectedItemId = Convert.ToInt32(cmbItemName.SelectedValue);
                Comments = txtComments.Text;
                Presenter.getNextRecordData();
                if (Presenter.UpdateAndSaveItemData())
                {
                    Presenter.evaluatePostSubmitRules();
                    if (CurrentViewContext.NextItemDataId > 0)
                    {
                        ResetOldData();
                        CurrentViewContext.ItemDataId = CurrentViewContext.NextItemDataId;
                        Presenter.getExceptionData();
                        BindTexBoxes();
                        BindItemDropDown();
                        base.HideErrorMessage();
                        if (!(uploadControl.UploadedFiles.Count > 0))
                        {
                            HideButton();
                        }
                        ResetOldData();
                        // grdDocuments.Rebind();
                    }
                    else
                    {
                        RedirectToQueue(true);
                    }
                }
                else
                {
                    base.ShowErrorMessage("Some error occured.Please try again");
                }
                //grdDocuments.Rebind();
                //lblErrMsg.ShowMessage("The record has been approved successfully.", MessageType.SuccessMessage);
                //hdnIsRedirect.Value = ReturnURL;
                //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "fnShowMessage();", true);
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
        /// Reject Alert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReject_Click(Object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.CurrentStatusCode = ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue();
                GetListOfIDToRemoveDocument();
                CurrentViewContext.SelectedItemId = Convert.ToInt32(cmbItemName.SelectedValue);
                Comments = txtComments.Text;
                Presenter.getNextRecordData();
                if (Presenter.UpdateAndSaveItemData())
                {
                    base.ShowSuccessMessage("Updated successfully.");
                    if (CurrentViewContext.NextItemDataId > 0)
                    {
                        ResetOldData();
                        CurrentViewContext.ItemDataId = CurrentViewContext.NextItemDataId;
                        Presenter.getExceptionData();
                        BindTexBoxes();
                        BindItemDropDown();
                        base.HideErrorMessage();
                        if (!(uploadControl.UploadedFiles.Count > 0))
                        {
                            HideButton();
                        }

                        //grdDocuments.Rebind();
                    }
                    else
                    {
                        RedirectToQueue(true);
                    }
                }
                else
                {
                    base.ShowErrorMessage("Some error occured.Please try again");
                }
                grdDocuments.Rebind();
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
                RedirectToQueue(false);
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
        /// Save uploaded files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(Object sender, EventArgs e)
        {
            try
            {
                UploadDocuments();
                HideButton();
            }
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
        /// Cancel upload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelUpload_Click(Object sender, EventArgs e)
        {
            try
            {
                uploadControl.UploadedFiles.Clear();
                HideButton();
            }
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

        #endregion

        #region Grid Events
        protected void grdDocuments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    ApplicantDocument docs = e.Item.DataItem as ApplicantDocument;
                    CheckBox chkRemove = e.Item.FindControl("chkRemove") as CheckBox;
                    if (docs.DocumentPath != null)
                    {
                        if (ApplicantDocumnetIds.Contains(docs.ApplicantDocumentID))
                        {
                            chkRemove.Checked = true;
                        }
                        //Dictionary for selected items to check the chekboxes.
                        Dictionary<Int32, Boolean> selectedItems = CurrentViewContext.SelectedItems;
                        if (selectedItems.ContainsKey(Convert.ToInt32(docs.ApplicantDocumentID)))
                        {
                            chkRemove.Checked = selectedItems[docs.ApplicantDocumentID];
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

        protected void grdDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                grdDocuments.DataSource = Presenter.BindDocumentGrid();
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

        #region CheckBox Events
        protected void chkSelectItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                if (checkBox.IsNull())
                {
                    return;
                }
                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                Dictionary<Int32, Boolean> selectedItems = CurrentViewContext.SelectedItems;
                Int32 applicantDocumentId = (Int32)dataItem.GetDataKeyValue("ApplicantDocumentID");

                if (dataItem.FindControl("chkRemove").IsNotNull())
                {
                    isChecked = ((CheckBox)dataItem.FindControl("chkRemove")).Checked;
                }
                if (selectedItems.ContainsKey(applicantDocumentId))
                {
                    selectedItems[applicantDocumentId] = isChecked;
                }
                else
                {
                    selectedItems.Add(applicantDocumentId, isChecked);
                }

                CurrentViewContext.SelectedItems = selectedItems;
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
        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// To route Page Back
        /// </summary>
        /// <param name="showSuccessMessage"></param>
        private void RedirectToQueue(bool ShowSuccessMessage)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "SelectedTenantId", Convert.ToString(SelectedTenantId) },
                                                                    { "Child", WorkQueuePath},
                                                                    {"PackageId",PackageId.ToString()},
                                                                    {"CategoryId",CategoryId.ToString()},
                                                                    {"AssignedToExpUser",AssignesToUser.ToString()},
                                                                    {"IncludeIncompleteItems",IncludeIncompleteItems.ToString()}
                                                                 };
            string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        /// <summary>
        /// Method to bind the textboxes on screen.
        /// </summary>
        private void BindTexBoxes()
        {
            txtApplicantName.Text = OrganizationUserData.FirstName + " " + OrganizationUserData.LastName;
            txtDOB.Text = OrganizationUserData.DOB.HasValue? Convert.ToDateTime(OrganizationUserData.DOB.Value).ToShortDateString():String.Empty;
            txtGender.Text = OrganizationUserData.Gender == 1 ? Gender.Male.GetStringValue() : Gender.Female.GetStringValue();
            txtEmail.Text = OrganizationUserData.aspnet_Users.aspnet_Membership.Email;
            txtPhone.Text = OrganizationUserData.PhoneNumber;
            txtTenantName.Text = TenantData.TenantName;
            txtExpReason.Text = ApplicantComplianceItem.ExceptionReason;
            txtVerificationComments.Text = ApplicantComplianceItem.VerificationComments;
            txtComments.Text = String.Empty;
        }

        /// <summary>
        /// Method to bind the Item Dropdown with the itemslist corresponding to current categoryid and tenantid
        /// </summary>
        private void BindItemDropDown()
        {
            cmbItemName.DataSource = itemIdList;
            cmbItemName.DataBind();
        }

        /// <summary>
        /// Method to get the list of ids to remove the document
        /// </summary>
        /// <returns>List of Ids</returns>
        private void GetListOfIDToRemoveDocument()
        {
            Presenter.BindDocumentGrid();
            List<Int32> addDocumentIdList = new List<Int32>();
            List<Int32> removeDocumentIdList = new List<Int32>();
            List<Int32> listOfIdToRemoveMapping = new List<Int32>();

            foreach (GridDataItem item in grdDocuments.Items)
            {
                CheckBox chkRemove = item.FindControl("chkRemove") as CheckBox;

                if (chkRemove.IsNotNull())
                {
                    if (chkRemove.Checked)
                    {
                        listOfIdToRemoveMapping.Add(Convert.ToInt32(item.GetDataKeyValue("ApplicantDocumentID")));
                    }
                }
            }

            CurrentViewContext.ListOfIdToAddDocument = listOfIdToRemoveMapping.Where(x => !ApplicantDocumnetIds.Any(i => i == x)).ToList();
            CurrentViewContext.ListOfIdToRemoveDocument = ApplicantDocumnetIds.Where(y => !listOfIdToRemoveMapping.Any(cond => cond == y)).ToList();
        }

        /// <summary>
        /// Method to hide buttons
        /// </summary>
        private void HideButton()
        {
            btnCancelUpload.Style.Add("display", "none");
            btnUpload.Style.Add("display", "none");
        }

        /// <summary>
        /// To save the uploaded files.
        /// </summary>
        private void UploadDocuments()
        {
            List<ApplicantDocument> applicantDocumentList = new List<ApplicantDocument>();
            foreach (UploadedFile item in uploadControl.UploadedFiles)
            {
                String filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
                if (filePath.IsNullOrEmpty())
                {
                    base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config", null);
                    break;
                }
                if (!filePath.EndsWith("\\"))
                {
                    filePath += "\\";
                }

                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                String fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                string newPath = Path.Combine(filePath, fileName);
                applicantDocumentList.Add(new ApplicantDocument()
                {
                    OrganizationUserID = OrganizationUserData.OrganizationUserID,
                    FileName = item.FileName,
                    Size = item.ContentLength,
                    DocumentPath = newPath,
                    CreatedByID = CurrentLoggedInUserId,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false
                });

                item.SaveAs(filePath + fileName);
            }

            if (applicantDocumentList != null && applicantDocumentList.Count > 0)
            {
                if (_presenter.SaveApplicantUploadedDocuments(applicantDocumentList))
                {
                    base.ShowSuccessMessage("Document uploaded successfully.");
                }
                else
                {
                    base.ShowErrorMessage("Some error occured.Please try again");
                }
                grdDocuments.Rebind();
            }
        }
        private void ResetOldData()
        {
            CurrentViewContext.SelectedItems = null;
        }
        #endregion
        #endregion
    }
}

