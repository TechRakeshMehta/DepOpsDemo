#region Namespaces

#region System Defined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;

#endregion

#region Application Specific

using Telerik.Web.UI;
using System.Web.Configuration;
using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Configuration;
using System.Web.UI;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Data;
using CoreWeb.IntsofSecurityModel;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;

#endregion

#endregion

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RequirementVerificationDetailsDocumentConrol : BaseUserControl, IRequirementVerificationDetailsDocumentConrolView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private variables
        private RequirementVerificationDetailsDocumentConrolPresenter _presenter = new RequirementVerificationDetailsDocumentConrolPresenter();

        #endregion

        #endregion

        #region Properties

        #region Presenter object

        public RequirementVerificationDetailsDocumentConrolPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        #endregion

        #region public properties

        public IRequirementVerificationDetailsDocumentConrolView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        /// <summary>
        ///get and  set Applicant id 
        /// </summary>
        public Int32 ApplicantId
        {
            get { return (Int32)(ViewState["ApplicantId"]); }
            set { ViewState["ApplicantId"] = value; }
        }

        public Int32 RequirementItemId
        {
            get
            {
                if (!ViewState["RequirementItemIdDocument"].IsNullOrEmpty())
                    return (Int32)(ViewState["RequirementItemIdDocument"]);
                else
                    return 0;
            }

            set { ViewState["RequirementItemIdDocument"] = value; }
        }

        /// <summary>
        /// Tenant id
        /// </summary>
        public Int32 SelectedTenantId_Global
        {
            get
            {
                if (!ViewState["SelectedTenantIdDoc"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedTenantIdDoc"]);
                else
                    return 0;
            }
            set
            {
                ViewState["SelectedTenantIdDoc"] = value;
            }
        }

        /// <summary>
        /// ItemData Id for which the data is displayed on the screen.
        /// </summary>
        public Int32 RequirementItemDataId
        {
            get
            {
                if (!ViewState["RequirementItemDataId"].IsNullOrEmpty())
                    return (Int32)(ViewState["RequirementItemDataId"]);
                else
                    return 0;
            }
            set { ViewState["RequirementItemDataId"] = value; }
        }
        public Int32 RequirementCategoryDataId //UAT-3345
        {
            get
            {
                if (!ViewState["RequirementCategoryDataId"].IsNullOrEmpty())
                    return (Int32)(ViewState["RequirementCategoryDataId"]);
                else
                    return 0;
            }
            set { ViewState["RequirementCategoryDataId"] = value; }
        }
        public Int32 RequirementFieldDataId
        {
            get
            {
                if (!ViewState["RequirementFieldDataId"].IsNullOrEmpty())
                    return (Int32)(ViewState["RequirementFieldDataId"]);
                else
                    return 0;
            }
            set { ViewState["RequirementFieldDataId"] = value; }
        }


        public Boolean IsFileUploadApplicable
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ViewState["IsFileUploadApplicable"])))
                    return (Boolean)(ViewState["IsFileUploadApplicable"]);
                return false;
            }
            set { ViewState["IsFileUploadApplicable"] = value; }
        }

        public Boolean IsFieldRequired
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ViewState["IsFieldRequired"])))
                    return (Boolean)(ViewState["IsFieldRequired"]);
                return false;
            }
            set { ViewState["IsFieldRequired"] = value; }
        }

        public Int32 ViewApplDocId
        {
            get
            {
                if (!ViewState["ViewApplDocId"].IsNullOrEmpty())
                    return (Int32)(ViewState["ViewApplDocId"]);
                else
                    return 0;
            }
            set { ViewState["ViewApplDocId"] = value; }
        }

        public Boolean IsViewDocApplicable
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ViewState["IsViewDocApplicable"])))
                    return (Boolean)(ViewState["IsViewDocApplicable"]);
                return false;
            }
            set { ViewState["IsViewDocApplicable"] = value; }
        }

        public String DocumentControlType
        {
            get
            {
                return Convert.ToString(ViewState["DocumentControlType"]);
            }
            set
            {
                ViewState["DocumentControlType"] = value;
            }
        }

        public Int32 CurrentTenantId_Global
        {
            get
            {   //tenantId = Presenter.GetTenantId();
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user.IsNotNull())
                {
                    return user.TenantId.HasValue ? user.TenantId.Value : 0;
                }
                return 0;
            }
        }

        public List<ApplicantFieldDocumentMappingContract> lstApplicantRequirementDocuments { get; set; }

        public List<ApplicantDocumentContract> lstApplicantDocument
        {
            get;
            set;
        }

        public List<ApplicantFieldDocumentMappingContract> lstApplicantRequirementDocumentMaps { get; set; }

        /// <summary>
        /// list of documents
        /// </summary>
        public List<ApplicantDocumentContract> ToSaveApplicantUploadedDocuments
        {
            get;
            set;
        }

        /// <summary>
        /// list of ApplicantRequirementFieldData data
        /// </summary>
        public List<ApplicantRequirementFieldData> lstApplicantRequirementFieldData
        {
            get
            {
                if (ViewState["lstApplicantRequirementFieldData"] != null)
                    return (List<ApplicantRequirementFieldData>)(ViewState["lstApplicantRequirementFieldData"]);
                return null;
            }
            set
            {
                ViewState["lstApplicantRequirementFieldData"] = value;
            }
        }

        public ApplicantRequirementItemData ApplicantRequirementItemData
        {
            get
            {
                if (ViewState["ApplicantRequirementItemData"] != null)
                    return (ApplicantRequirementItemData)(ViewState["ApplicantRequirementItemData"]);
                return null;
            }
            set
            {
                ViewState["ApplicantRequirementItemData"] = value;


            }
        }

        Int32 organizationUserID = 0;
        /// <summary>
        /// Organization user id
        /// </summary>
        public Int32 OrganiztionUserID
        {
            get { return organizationUserID == 0 ? base.CurrentUserId : organizationUserID; }
            set { organizationUserID = value; }

        }

        public Int32 RequirementPackageSubscriptionId
        {
            get { return (Int32)(ViewState["RequirementPackageSubscriptionId"]); }
            set { ViewState["RequirementPackageSubscriptionId"] = value; }
        }

        public int RequirementCategoryId
        {
            get { return (Int32)(ViewState["RequirementCategoryId"]); }
            set { ViewState["RequirementCategoryId"] = value; }
        }

        public int RequirementFieldId
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ViewState["RequirementFieldId"])))
                    return Convert.ToInt32(ViewState["RequirementFieldId"]);
                return AppConsts.NONE;
            }
            set { ViewState["RequirementFieldId"] = value; }
        }

        public Boolean IsReadOnly
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ViewState["IsReadOnly"])))
                    return Convert.ToBoolean(ViewState["IsReadOnly"]);
                return false;
            }
            set { ViewState["IsReadOnly"] = value; }
        }

        public Boolean IsIncompleteItem
        {
            get { return Convert.ToBoolean((ViewState["IsIncompleteItem"])); }
            set { ViewState["IsIncompleteItem"] = value; }
        }
        String _errorMessage = String.Empty;

        public String ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                base.LogInfo(_errorMessage);
            }
        }

        public string EntityPermissionName
        {
            get;
            set;
        }

        public Boolean IsFileUploadControlExist
        {
            get
            {
                if (!ViewState["IsFileUploadControlExist"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsFileUploadControlExist"]);
                else
                    return false;
            }

            set { ViewState["IsFileUploadControlExist"] = value; }
        }

        /// <summary>
        /// Delegates for the uploader.
        /// </summary>
        public delegate void UploadDelegate();

        public event UploadDelegate OnCompletedUpload;
        public Boolean IsItemEditable { get; set; }

        #region UAT-4368
        public Boolean IsClientAdminLoggedIn
        {
            get;
            set;
        }
        public Boolean IsAdminLoggedIn
        {
            get;
            set;
        }
        public Boolean IsFieldEditableByAdmin { get; set; }
    
        public Boolean IsFieldEditableByApplicant { get; set; }

        public Boolean IsFieldEditableByClientAdmin { get; set; }
        #endregion
        #endregion


        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            AddUpdateMethodToDelegate();
            Presenter.GetData();
            DisplayUploadControl();
            hdnTenantIdInDocument.Value = SelectedTenantId_Global.ToString();
            Presenter.OnViewLoaded();
            if (IsReadOnly)
            {
                uploadControl.Enabled = false;
            }
            uploadControl.MaxFileSize = Convert.ToInt32(WebConfigurationManager.AppSettings[AppConsts.MAXIMUM_FILE_SIZE]);
            SetFocusOnParent();

            //UAT 2371
            if (!String.IsNullOrEmpty(this.EntityPermissionName) && Convert.ToString(this.EntityPermissionName).ToUpper() == "NONE")
            {
                hdnpermissionName.Value = "none";
                uploadControl.Enabled = false;
            }

        }

        /// <summary>
        /// Button click to save all the new added documents.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarMUser_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                UploadAllDocuments();
                UpdateNewDocumentList();
                SetFocusOnParent();
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
        /// Cancel click event for the new documents
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarMUser_CancelClick(object sender, EventArgs e)
        {
            try
            {
                EnableDisableAllValidations();
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
        /// Adds the mapping for the new document with the data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public Int32 SaveItemDocumentMappings()
        {
            Int32 resultCatdataId = 0;

            try
            {

                UpdateApplicantRequirementDocumentMaps();
                UpdateNewDocumentList();
                resultCatdataId = this.RequirementCategoryDataId;

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
            return resultCatdataId;
        }

        /// <summary>
        /// Cancel click event for mapping documents
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void comandSaveMapping_CancelClick(object sender, EventArgs e)
        {
            try
            {
                EnableDisableAllValidations();
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

        protected void rptrAllDocuments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (IsReadOnly)
            {
                CheckBox chkIsMapped = e.Item.FindControl("chkIsMapped") as CheckBox;
                chkIsMapped.Enabled = false;
                comandSaveMapping.SaveButton.Enabled = false;
            }
            //Code commentted for UAT 4380
            //if(!IsItemEditable)
            //{
            //    CheckBox chkIsMapped = e.Item.FindControl("chkIsMapped") as CheckBox;
            //    chkIsMapped.Enabled = false;
            //}
            //UAT 4380
            if (IsAdminLoggedIn)
            {
                if (!IsFieldEditableByAdmin)
                {
                      CheckBox chkIsMapped = e.Item.FindControl("chkIsMapped") as CheckBox;
                      chkIsMapped.Enabled = false;
                }
            }
            if (IsClientAdminLoggedIn)
            {
                if (!IsFieldEditableByClientAdmin)
                {
                    CheckBox chkIsMapped = e.Item.FindControl("chkIsMapped") as CheckBox;
                    chkIsMapped.Enabled = false;
                }
            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                AllApplicantDocuments applicantDoc = (AllApplicantDocuments)e.Item.DataItem;
                //UAT 1582 Completed "View Document" should automatically associate with FileUpload attribute upon completion. 
                CheckBox chkIsMapped = e.Item.FindControl("chkIsMapped") as CheckBox;
                if (Convert.ToBoolean(applicantDoc.IsViewDocType) && Convert.ToInt32(applicantDoc.ItemId) == RequirementItemDataId)
                {             
                    chkIsMapped.Enabled = false;
                }
                //Start UAT-5062
                if (lstApplicantRequirementDocumentMaps.IsNotNull() && lstApplicantRequirementDocumentMaps.Any(x => x.ApplicantDocumentId == Convert.ToInt32(applicantDoc.ApplicantDocumentId) && x.IsDisabled))
                    chkIsMapped.Enabled = false;
                //End UAT-5062
            }
        }

        protected void rptrDocuments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            LinkButton lnkBtnRemove = e.Item.FindControl("lnkbtnDelete") as LinkButton;
            if (!String.IsNullOrEmpty(this.EntityPermissionName) && (IsReadOnly || Convert.ToString(this.EntityPermissionName).ToUpper() == "NONE"))
            {
                lnkBtnRemove.Enabled = false;
            }
            //Code commentted for UAT 4380
            //if (!IsItemEditable)
            //{
            //    lnkBtnRemove.Enabled = false;
            //}
     
            //UAT 4380
            if (IsAdminLoggedIn)
            {
                if (!IsFieldEditableByAdmin)
                {
                    lnkBtnRemove.Enabled = false;
                }
            }
            if (IsClientAdminLoggedIn)
            {
                if (!IsFieldEditableByClientAdmin)
                {
                    lnkBtnRemove.Enabled = false;
                }
            }

            //Start UAT-4900
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ApplicantFieldDocumentMappingContract applicantDoc = (ApplicantFieldDocumentMappingContract)e.Item.DataItem;

                if (applicantDoc.IsNotNull() && Convert.ToBoolean(applicantDoc.IsDisabled))
                {
                    Literal litSymbol2 = e.Item.FindControl("litSymbol2") as Literal;
                    lnkBtnRemove.Visible = false;
                    litSymbol2.Visible = false;
                }
            }
            //End UAT-4900

            //UAT 1582 Completed "View Document" should automatically associate with FileUpload attribute upon completion. 
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    ApplicantDocumentContract applicantDoc = (ApplicantDocumentContract)e.Item.DataItem;

            //    if (Convert.ToBoolean(applicantDoc.IsViewDocType) && Convert.ToInt32(applicantDoc.ItemID) == RequirementItemDataId)
            //    {
            //        LinkButton lnkBtnRemove = e.Item.FindControl("lnkbtnDelete") as LinkButton;
            //        Literal litSymbol2 = e.Item.FindControl("litSymbol2") as Literal;
            //        lnkBtnRemove.Visible = false;
            //        litSymbol2.Visible = false;
            //    }
            //}
        }

        /// <summary>
        /// Removes the document mapping with the item.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rptrDocuments_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "remove")
            {
                Int32 applicantDocumentId = Convert.ToInt32(e.CommandArgument);
                Int32 updateId = 0;

                //If field is required and there is only one document uploaded then user cannot remove  the document.
                if (IsFieldRequired && lstApplicantRequirementDocumentMaps.Count() == AppConsts.ONE)
                {
                    ShowHideValidationMessage("Document is required.", true);
                    EnableDisableAllValidations();
                }
                else
                {
                    ShowHideValidationMessage(String.Empty, false);

                    updateId = lstApplicantRequirementDocumentMaps.FirstOrDefault(x => x.ApplicantDocumentId == applicantDocumentId) == null ? 0 :
                        lstApplicantRequirementDocumentMaps.FirstOrDefault(x => x.ApplicantDocumentId == applicantDocumentId).ApplicantRequirementDocumentMapId;

                    Dictionary<Boolean, String> result = Presenter.ValidateDocumentMappingRules(rptrDocuments.Items.Count - 1);
                    Boolean isSuccess = !result.Keys.FirstOrDefault();
                    if (isSuccess)
                    {
                        Presenter.RemoveMapping(updateId);

                        SetFocusOnParent();

                        UpdateNewDocumentList();
                    }
                    else
                    {
                        String errorMsg = result.Values.FirstOrDefault();
                        ShowHideValidationMessage(errorMsg, true);
                    }
                }
            }
            else if (e.CommandName == "print")
            {
                EnableDisableAllValidations();
            }
        }

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// Enable/Disable Validations
        /// </summary>
        private void EnableDisableAllValidations()
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "EnableDisableAllValidations();", true);
        }

        /// <summary>
        /// Checks whether o show this control on screen or not.
        /// </summary>
        private void DisplayUploadControl()
        {

            String _uploadDocumentTypecode = RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue();

            if ((lstApplicantRequirementFieldData.IsNullOrEmpty()
                || !lstApplicantRequirementFieldData.Any(x => x.RequirementField != null && x.RequirementField.lkpRequirementFieldDataType.RFDT_Code == _uploadDocumentTypecode))
                && !this.IsFileUploadApplicable)
            {
                if (IsViewDocApplicable)
                {
                    dvBrowse.Visible = false;
                    divAssignDocCls.Visible = false;
                    BindDocumentList(false);
                }
                else
                {
                    uploadControlDiv.Style.Add("display", "none");
                }
            }
            else
            {
                BindDocumentList(false);
                if (!String.IsNullOrEmpty(this.EntityPermissionName) && Convert.ToString(this.EntityPermissionName).ToUpper() == "NONE")
                {
                    uploadControl.Enabled = true;
                }
            }
            //Code Commentted for UAT 4380
            //if (!IsItemEditable)
            //{
            //    uploadControl.Enabled = false;
            //}

            //UAT 4380
            if (IsAdminLoggedIn)
            {
                if (!IsFieldEditableByAdmin)
                {
                    uploadControl.Enabled = false;
                }
            }
            if (IsClientAdminLoggedIn)
            {
                if (!IsFieldEditableByClientAdmin)
                {
                    uploadControl.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Bind the repeaters on the scree.
        /// </summary>
        private void BindDocumentList(Boolean refreshData = true)
        {
            if (refreshData)
            {
                Presenter.GetData();
            }
            List<ApplicantDocumentContract> applicantDocuments = new List<ApplicantDocumentContract>();
            //applicantDocument = Presenter.GetItemRelatedDocument();

            if (lstApplicantRequirementDocumentMaps.IsNull())
            {
                // This will allow execution of the NEXT statement even when 'lstApplicantRequirementDocumentMaps' is Empty
                // i.e. No document was mapped with the current item. So evaluation of 'lstApplicantRequirementDocumentMaps' will not 
                // give 'NULL' reference exception.
                lstApplicantRequirementDocumentMaps = new List<ApplicantFieldDocumentMappingContract>();
            }


            rptrDocuments.DataSource = lstApplicantRequirementDocumentMaps.DistinctBy(ad => ad.ApplicantDocumentId).ToList();
            rptrDocuments.DataBind();

            List<AllApplicantDocuments> allApplicantDocuments = new List<AllApplicantDocuments>();
            if (!lstApplicantDocument.IsNullOrEmpty())
            {
                //todo
                //ApplicantDocumentContract viewTypeDoc = lstApplicantDocument.Where(x => x.IsViewDocType == AppConsts.ONE && x.ItemID == RequirementItemDataId).FirstOrDefault();
                foreach (var item in lstApplicantDocument.DistinctBy(x => x.ApplicantDocumentId))
                {
                    //UAT 1582 Completed "View Document" should automatically associate with FileUpload attribute upon completion. 
                    //if (viewTypeDoc.IsNotNull() && viewTypeDoc.ApplicantDocumentId == item.ApplicantDocumentId)
                    //{
                    //    allApplicantDocuments.Add(new AllApplicantDocuments(viewTypeDoc.ApplicantDocumentId, viewTypeDoc.FileName, IsCheckedDocument(viewTypeDoc.ApplicantDocumentId), viewTypeDoc.IsViewDocType, viewTypeDoc.ItemID));
                    //}
                    //else
                    //{
                    //    allApplicantDocuments.Add(new AllApplicantDocuments(item.ApplicantDocumentId, item.FileName, IsCheckedDocument(item.ApplicantDocumentId), item.IsViewDocType, item.ItemID));
                    //}                                                                                                                                         //UAT-5062
                    allApplicantDocuments.Add(new AllApplicantDocuments(item.ApplicantDocumentId, item.FileName, IsCheckedDocument(item.ApplicantDocumentId)));
                }

                //Repeater for all the documents.
                rptrAllDocuments.DataSource = allApplicantDocuments;
                rptrAllDocuments.DataBind();

            }
        }

        /// <summary>
        /// Bind the check box in rptrAllDocuments repeater.
        /// It checks whether the current document is mapped or not.
        /// </summary>
        /// <param name="applicationDocumentId"></param>
        /// <returns></returns>
        private Boolean IsCheckedDocument(Int32 applicationDocumentId)
        {
            if (lstApplicantRequirementDocumentMaps.IsNotNull())
                return lstApplicantRequirementDocumentMaps.FirstOrDefault(x => x.ApplicantDocumentId == applicationDocumentId) == null ? false : true;

            return false;
        }

        /// <summary>
        /// This updates the mapping of the document with the item.
        /// </summary>
        private void UpdateApplicantRequirementDocumentMaps()
        {
            //This list adds the new mapping in ApplicantRequirementDocumentMap table. 
            List<ApplicantDocumentContract> ToAddDocumentMap = new List<ApplicantDocumentContract>();
            //This deletes the mapping from ApplicantRequirementDocumentMap table.
            List<Int32> toDeleteApplicantRequirementDocumentMapIDs = new List<Int32>();
            ApplicantFieldDocumentMappingContract documentMap = null;
            Boolean isUIValidationPassed = true;
            Boolean isDocumentMapped = false;

            foreach (RepeaterItem item in rptrAllDocuments.Items)
            {
                var chkIsMapped = (CheckBox)item.FindControl("chkIsMapped");
                var hdnDocumentId = (HiddenField)item.FindControl("hdnDocumentId");

                Int32 applicantDocumentId = Convert.ToInt32(hdnDocumentId.Value);

                if (lstApplicantRequirementDocumentMaps.IsNotNull())
                    documentMap = lstApplicantRequirementDocumentMaps.FirstOrDefault(x => x.ApplicantDocumentId == applicantDocumentId);

                if (documentMap == null && chkIsMapped.Checked)      // Add new document
                {
                    ApplicantDocumentContract applicantDocument = new ApplicantDocumentContract();
                    applicantDocument.ApplicantDocumentId = applicantDocumentId;

                    ToAddDocumentMap.Add(applicantDocument);
                }
                else if (documentMap != null && !chkIsMapped.Checked)
                {
                    toDeleteApplicantRequirementDocumentMapIDs.Add(documentMap.ApplicantRequirementDocumentMapId);
                }

                //Check if document mapped
                if (!isDocumentMapped)
                    isDocumentMapped = chkIsMapped.Checked;

            }

            if (IsFieldRequired && !isDocumentMapped)
            {
                //ShowHideValidationMessage("Document is required.", true); //UAT-3345

                isUIValidationPassed = false;
            }

            // Update assign/un-assign only if validation is passed
            if (isUIValidationPassed)
            {
                ShowHideValidationMessage(String.Empty, false);

                String _uploadDocumentTypecode = RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue();
                //String _viewDocTypecode = RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue();

                ApplicantRequirementFieldData _fileTypeAttribute = null;
                //ApplicantRequirementFieldData _viewDocTypeAttribute = null;

                if (!lstApplicantRequirementFieldData.IsNullOrEmpty())
                {
                    _fileTypeAttribute = lstApplicantRequirementFieldData
                                          .Where(x => x.RequirementField != null // Document type attribute is added as new attribute, so null check is required
                                          && x.RequirementField.lkpRequirementFieldDataType.RFDT_Code == _uploadDocumentTypecode).FirstOrDefault();
                    //_viewDocTypeAttribute = lstApplicantRequirementFieldData
                    //                         .Where(x => x.RequirementField != null // Document type attribute is added as new attribute, so null check is required
                    //                         && x.RequirementField.lkpRequirementFieldDataType.RFDT_Code == _viewDocTypecode).FirstOrDefault();

                }
                // ONLY THIS PROPERTY CANNOT BE USED AS A PARAMTER FOR INCOMPLETE ITEM IDENTIFICATION.
                // Case when ONLY documents are assigned by admin for any item, also remains as Incomplete item status.
                // So item data gets inserted and in that case also 'AssignUnAssignIncompleteItemDocuments' is executed
                // So this case is handled inside the 'AssignUnAssignIncompleteItemDocuments' method

                if ((!this.IsIncompleteItem && !lstApplicantRequirementFieldData.IsNullOrEmpty() && !_fileTypeAttribute.IsNullOrEmpty()))
                {
                    // Handles the cases when : 
                    // 1. Documents are getting added for second time or so on, when other attributes have been already added
                    Presenter.AssignUnAssignItemDocuments(ToAddDocumentMap, toDeleteApplicantRequirementDocumentMapIDs);
                }
                else
                {
                    // NO EXCEPTION ITEM is handled by this. Handles the cases when :
                    // 1. If documents are being added for Incomplete items, either first or second time, without any data added for any other fields
                    // 2. Documents are being assigned to an item for the first time, when other fields have been already added. 
                    //    This is because, this method internally manages to ADD NEW data in ApplicantRequirementFieldData, which above condition method cannot do.
                    var res = Presenter.AssignUnAssignIncompleteItemDocuments(ToAddDocumentMap, toDeleteApplicantRequirementDocumentMapIDs); // No Exception for Incomplete Items by ADMINs
                    RequirementCategoryDataId = res.Item1;
                    this.RequirementFieldDataId = res.Item3;
                    this.RequirementItemDataId = CurrentViewContext.RequirementItemDataId = res.Item2;
                }
            }
        }

        private void AddUpdateMethodToDelegate()
        {
            //todo
            if (HttpContext.Current.Items["UpdateDocumentList"] == null)
            {
                //del = new UpdateDocumentList(UpdateDocumentList);
                //HttpContext.Current.Items["UpdateDocumentList"] = del;
            }
            else
            {
                //del = (UpdateDocumentList)HttpContext.Current.Items["UpdateDocumentList"];
                //del += new UpdateDocumentList(UpdateDocumentList);
                //HttpContext.Current.Items["UpdateDocumentList"] = del;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Show/Hide validation message
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="visibility"></param>
        public void ShowHideValidationMessage(String msg, Boolean visibility)
        {
            lblMessage.Visible = visibility;
            lblMessage.Text = msg;
        }

        public void RebindAfterSave(Boolean isReadOnlyAfterSave)
        {
            uploadControlDiv.Attributes.Add("class", "hidedocs");
            uploadControl.Visible = false;

            if (isReadOnlyAfterSave)
            {
                for (int i = 0; i < rptrDocuments.Items.Count; i++)
                {
                    LinkButton lnkDelete = (rptrDocuments.Items[i].FindControl("lnkbtnDelete") as LinkButton);
                    if (lnkDelete.IsNotNull())
                        lnkDelete.Visible = false;

                    Literal litSymbol = (rptrDocuments.Items[i].FindControl("litSymbol") as Literal);
                    if (litSymbol.IsNotNull())
                        litSymbol.Visible = false;

                }
            }
        }

        public void UpdateDocumentList(List<ApplicantDocumentContract> lstUpdatedApplicantDocument)
        {
            lstApplicantDocument = lstUpdatedApplicantDocument;
            BindDocumentList();
        }


        /// <summary>
        /// Call the multiCast delegate to update the list of documents.
        /// </summary>
        public void UpdateNewDocumentList()
        {
            Presenter.GetApplicantDocuments();
            if (HttpContext.Current.Items["UpdateDocumentList"] != null)
            {
                //todo
                //del = (UpdateDocumentList)HttpContext.Current.Items["UpdateDocumentList"];
                //del(lstApplicantDocument);
            }
            if (CurrentViewContext.IsFileUploadControlExist) //For Payment type item no document mapping available
            {

                Presenter.UpdateMappingList();

                BindDocumentList();
                ShowHideDeleteCheckBox(false, RequirementItemId);

                //Get applicant documents by requirement package subscription id and category Id
                Int32 docId = 0;
                Presenter.GetRequirementApplicantDocumentsByCategoryId();
                var _applicantRequirementDocuments = CurrentViewContext.lstApplicantRequirementDocuments.FirstOrDefault(x => x.ApplicantDocumentId > AppConsts.NONE);
                if (!_applicantRequirementDocuments.IsNullOrEmpty())
                    docId = _applicantRequirementDocuments.ApplicantDocumentId;

                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ViewDocInPDFViewer('" + docId + "');", true);
                EnableDisableAllValidations();
            }
            //if (!CurrentViewContext.lstApplicantRequirementDocumentMaps.IsNullOrEmpty())
            //{
            //    docId = CurrentViewContext.lstApplicantRequirementDocumentMaps.FirstOrDefault().ApplicantDocumentId;
            //    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ViewDocInPDFViewer('" + docId + "');", true);
            //}
            //else
            //{
            //    if (CurrentViewContext.ViewApplDocId > AppConsts.NONE)
            //        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ViewDocInPDFViewer('" + CurrentViewContext.ViewApplDocId + "');", true);
            //    else
            //        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ViewDocInPDFViewer('" + docId + "');", true);
            //}
        }

        /// <summary>
        /// Call the multiCast delegate to update the list of documents.
        /// </summary>
        public void ShowHideDeleteCheckBox(Boolean isDeleteApplicable, Int32 requirementItemId)
        {
            if (HttpContext.Current.Items["ShowDeleteCheckBox"] != null)
            {
                showChkBox = (ShowDeleteCheckBox)HttpContext.Current.Items["ShowDeleteCheckBox"];
                showChkBox(isDeleteApplicable, requirementItemId);
            }
            BindDocumentList();
        }

        /// <summary>
        /// It upload and maps the document added by the user.
        /// </summary> 
        public void UploadAllDocuments()
        {
            if (uploadControl.UploadedFiles.Count > 0)
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
                }
                if (!tempFilePath.EndsWith(@"\"))
                {
                    tempFilePath += @"\";
                }
                tempFilePath += "Tenant(" + CurrentViewContext.SelectedTenantId_Global.ToString() + @")\";

                if (!Directory.Exists(tempFilePath))
                    Directory.CreateDirectory(tempFilePath);

                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    if (filePath.IsNullOrEmpty())
                    {
                        base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", null);
                    }

                    if (!filePath.EndsWith(@"\"))
                    {
                        filePath += @"\";
                    }
                    filePath += "Tenant(" + CurrentViewContext.SelectedTenantId_Global.ToString() + @")\";

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
                        }
                        if (!filePath.EndsWith(@"/"))
                        {
                            filePath += @"/";
                        }

                        //AWS code to save document to S3 location
                        AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                        String destFolder = filePath + "Tenant(" + CurrentViewContext.SelectedTenantId_Global.ToString() + @")/";
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
                        }
                        if (!filePath.EndsWith(@"/"))
                        {
                            filePath += @"/";
                        }
                        newFilePath = filePath + "Tenant(" + CurrentViewContext.SelectedTenantId_Global.ToString() + @")/";
                    }

                    //Add Applicant Uploaded Documents and mapping data
                    Presenter.AddApplicantUploadedDocuments();

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
                //return docMessage.ToString();
            }
        }


        /// <summary>
        /// It is used to set focus on "ucVerificationItemDataPanel" middle pane.
        /// </summary> 
        public void SetFocusOnParent()
        {
            //String key = "CategoryPanel";
            //if (HttpContext.Current.Items[key].IsNotNull())
            //{
            //    INTERSOFT.WEB.UI.WebControls.WclSplitter categoryPanel = (INTERSOFT.WEB.UI.WebControls.WclSplitter)HttpContext.Current.Items[key];
            //    if (categoryPanel.IsNullOrEmpty())
            //        this.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Focus(); // to set focus on "ucVerificationItemDataPanel" middle pane
            //    else
            //    {
            //        Page.FindControl(categoryPanel.UniqueID).Focus(); // to set focus on "ucVerificationItemDataPanel" middle pane
            //    }
            //}
        }

        #endregion

        #endregion




    }
}

