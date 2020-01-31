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

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class VerificationDetailsDocumentConrol : BaseUserControl, IVerificationDetailsDocumentConrolView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private variables
        private VerificationDetailsDocumentConrolPresenter _presenter = new VerificationDetailsDocumentConrolPresenter();

        Int32 _disableViewDocumentID = AppConsts.NONE;
        Boolean _isDocuementToDisabled = false;

        //UAT-2768
        Boolean _isDocumentAssignedOnly = false;
        Boolean _isUpdateSessionList = true;
        Int32 _totatDocumentMapped = 0;
        Int32 _ItemDataID = 0;
        Boolean _isValidAssignUnAssignDocument = false;

        #endregion

        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                Presenter.OnViewInitialized();
                //UAT 1740
                if (!Presenter.IsDefaultTenant)
                {
                    hdnIsEdPrevAcpt.Value = Convert.ToString(Presenter.IsEDFormPreviouslyAccepted()).ToLower();
                    hdnEmployementDiscTypeCode.Value = DislkpDocumentType.EMPLOYMENT_DISCLOSURE_FORM.GetStringValue();
                    hdnOrgUsrId.Value = Convert.ToString(CurrentLoggedUserID);
                }
            }
            Presenter.GetScreeningDocumentTypeId();
            GetAllowedFileExtensions();
            AddUpdateMethodToDelegate();
            /*_presenter.ItemHasFileAttribute();*/
            Presenter.getData();
            DisplayUploadControl();
            hdnTenantIdInDocument.Value = SelectedTenantId_Global.ToString();
            //set document Status ID as Merging Completed
            hdnMergingCompletedDocStatusID.Value = Convert.ToString(Presenter.GetDocumentStatusID());
            Presenter.OnViewLoaded();
            if (IsReadOnly)
            {
                uploadControl.Enabled = false;
            }
            uploadControl.MaxFileSize = Convert.ToInt32(WebConfigurationManager.AppSettings[AppConsts.MAXIMUM_FILE_SIZE]);
            SetFocusOnParent();

            //UAT-4067
            //Dictionary<String, String> arguments = new Dictionary<String, String>();
            //if (Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT] != null)
            //{
            //    arguments.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
            //    if (arguments.ContainsKey("allowedFileExtensions"))
            //    {
            //        String allowedFileExtensions = arguments["allowedFileExtensions"];
            //        String[] allowedExtensions = allowedFileExtensions.Split(',');
            //        uploadControl.AllowedFileExtensions = allowedExtensions;
            //    })
            //}
            //if (!AllowedFileExtensions.IsNullOrEmpty())
            //{
            //String[] allowedExtensions = AllowedFileExtensions;
            String[] allowedFileExtensions = AllowedFileExtensions.IsNullOrEmpty() ? String.Empty.Split(',') : AllowedFileExtensions.ToArray();
            uploadControl.AllowedFileExtensions = allowedFileExtensions;
            //}
        }
        #endregion

        #region Presenter object

        public VerificationDetailsDocumentConrolPresenter Presenter
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
        #endregion

        #region Properties

        #region public properties
        public Int32 CurrentLoggedUserID
        {
            get { return base.CurrentUserId; }
        }

        public Int32 PackageSubscriptionId
        {
            get { return (Int32)(ViewState["PackageSubscriptionId"]); }
            set { ViewState["PackageSubscriptionId"] = value; }
        }


        public List<ApplicantDocuments> lstApplicantDocument
        {
            get;
            set;
        }

        /// <summary>
        /// Contains the Assignment Properties of the current item
        /// </summary>
        public List<ListItemAssignmentProperties> lstAssignmentProperties
        {
            get;
            set;
        }

        /// <summary>
        ///get and  set Applicant id 
        /// </summary>
        public Int32 ApplicantId
        {
            get { return (Int32)(ViewState["ApplicantId"]); }
            set { ViewState["ApplicantId"] = value; }
        }

        public Int32 ComplianceItemId
        {
            get { return (Int32)(ViewState["ComplianceItemIdDocument"]); }
            set { ViewState["ComplianceItemIdDocument"] = value; }
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
                if (ViewState["SelectedTenantIdDoc"].IsNullOrEmpty())
                    ViewState["SelectedTenantIdDoc"] = value;
            }
        }

        /// <summary>
        /// ItemData Id for which the data is displayed on the screen.
        /// </summary>
        public Int32 ItemDataId
        {
            get { return Convert.ToInt32(ViewState["ItemDataId"]); }
            set { ViewState["ItemDataId"] = value; }
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

        /// <summary>
        /// Identify the Type of use of VerificationDetailsDocumnetControl.ascx - enum DocumentControlType
        /// </summary>
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

        /// <summary>
        /// CurrentLoggedInUserId
        /// </summary>
        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
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

        /// <summary>
        /// Checks for the data is for exception or normal data entry
        /// </summary>
        public Boolean IsException { get; set; }

        /// <summary>
        /// lstApplicantComplianceDocumentMaps
        /// </summary>
        public List<ApplicantComplianceDocumentMap> lstApplicantComplianceDocumentMaps { get; set; }

        /// <summary>
        /// list of documents
        /// </summary>
        public List<ApplicantDocument> ToSaveApplicantUploadedDocuments
        {
            get;
            set;
        }

        /// <summary>
        /// list of ApplicantComplianceAttributeData data
        /// </summary>
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


            }
        }

        /// <summary>
        /// ApplicantComplianceItemData
        /// </summary>
        public ApplicantComplianceItemData ApplicantComplianceItemData
        {
            get
            {
                if (ViewState["lstApplicantComplianceItemData"] != null)
                    return (ApplicantComplianceItemData)(ViewState["lstApplicantComplianceItemData"]);
                return null;
            }
            set
            {
                ViewState["lstApplicantComplianceItemData"] = value;


            }
        }

        /// <summary>
        /// list of ExceptionDocumentDocumentMaps data
        /// </summary>
        public List<ExceptionDocumentMapping> lstExceptionDocumentDocumentMaps { get; set; }

        Int32 organizationUserID = 0;
        /// <summary>
        /// Organization user id
        /// </summary>
        public Int32 OrganiztionUserID
        {
            get { return organizationUserID == 0 ? base.CurrentUserId : organizationUserID; }
            set { organizationUserID = value; }

        }

        public int CompliancePackageId
        {
            get { return (Int32)(ViewState["CompliancePackageId"]); }
            set { ViewState["CompliancePackageId"] = value; }
        }

        public int ComplianceCategoryId
        {
            get { return (Int32)(ViewState["ComplianceCategoryId"]); }
            set { ViewState["ComplianceCategoryId"] = value; }
        }

        public int ComplianceAttributeId
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ViewState["ComplianceAttributeId"])))
                    return Convert.ToInt32(ViewState["ComplianceAttributeId"]);
                return AppConsts.NONE;
            }
            set { ViewState["ComplianceAttributeId"] = value; }
        }

        public Boolean IsReadOnly
        {
            get { return (Boolean)(ViewState["IsReadOnly"]); }
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

        /// <summary>
        /// UAT1738 - Remove the Documents from Document panel, attached to "Screening Document Type" attribute.
        /// </summary>
        public Int32 ScreeningDocTypeId
        {
            get
            {
                return Convert.ToInt32(hdnScrDocTypeId.Value);
            }
            set
            {
                hdnScrDocTypeId.Value = Convert.ToString(value);
            }
        }

        #region UAT-1049:Admin Data Entry
        public Int16 DataEntryDocNewStatusId
        {
            get
            {
                if (ViewState["DataEntryDocNewStatusId"] != null)
                    return Convert.ToInt16(ViewState["DataEntryDocNewStatusId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["DataEntryDocNewStatusId"] = value;
            }
        }
        #endregion


        #region UAT-4067
        //public String AllowedExtensions
        //{
        //    get { return (String)(ViewState["AllowedExtensions"]); }
        //    set { ViewState["AllowedExtensions"] = value; }
        //}
        //public Boolean IsAllowedFileExtensionEnable
        //{
        //    get;
        //    set;
        //}
        //public Boolean IsRestrictedFileExists
        //{
        //    get
        //    {
        //        if (ViewState["IsRestrictedFileExists"].IsNotNull())
        //        {
        //            return (Boolean)(ViewState["IsRestrictedFileExists"]);
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    set
        //    {
        //        ViewState["IsRestrictedFileExists"] = value;
        //    }
        //}

        #region UAT-4067
        public List<Int32> selectedNodeIDs
        {
            get
            {
                if (!ViewState["selectedNodeIDs"].IsNull())
                {
                    return (ViewState["selectedNodeIDs"]) as List<Int32>;
                }
                return new List<Int32>();
            }
            set { ViewState["selectedNodeIDs"] = value; }
        }

        public List<String> AllowedFileExtensions
        {
            get
            {
                if (!ViewState["allowedFileExtensions"].IsNull())
                {
                    return (ViewState["allowedFileExtensions"]) as List<String>;
                }
                return new List<String>();
            }
            set { ViewState["allowedFileExtensions"] = value; }
        }

        #endregion-4067
        #endregion


        /// <summary>
        /// Delegates for the uploader.
        /// </summary>
        public delegate void UploadDelegate();

        public event UploadDelegate OnCompletedUpload;

        private Boolean IsUIValidationApplicable { get { return Convert.ToBoolean(ConfigurationManager.AppSettings["IsUIValidationApplicable"]); } }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// Fo new entry in ExceptionDocumentMapping instance is returned specific to the document id.
        /// </summary>
        /// <param name="applicantDocumentId">Documen id</param>
        /// <returns>ExceptionDocumentMapping instance</returns>
        private ExceptionDocumentMapping InstanceOfDocumentMapException(Int32 applicantDocumentId)
        {
            return new ExceptionDocumentMapping()
            {
                ApplicantComplianceItemID = ItemDataId,
                ApplicantDocumentID = applicantDocumentId,
                IsDeleted = false,
                CreatedBy = CurrentLoggedInUserId,
                CreatedOn = DateTime.Now
            };

        }

        /// <summary>
        /// Fo new entry in ApplicantComplianceDocumentMap instance is returned specific to the document id.
        /// </summary>
        /// <param name="applicantDocumentId">Documen id</param>
        /// <returns>ApplicantComplianceDocumentMap instance</returns>
        private ApplicantComplianceDocumentMap InstanceOfDocumentMap(Int32 applicantDocumentId)
        {
            String FileUploadTypecode = ComplianceAttributeDatatypes.FileUpload.GetStringValue();
            //String ViewDocumentTypecode = ComplianceAttributeDatatypes.View_Document.GetStringValue();

            if (!this.IsIncompleteItem) // If not incomplete
            {
                var _fileTypeAttribute = lstApplicantComplianceAttributeData
                                        .Where(x => x.ComplianceAttribute != null // Document type attribute is added as new attribute, so null check is required, as it is not having any Navigation property
                                            && x.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == FileUploadTypecode.ToLower()).FirstOrDefault();

                if (!_fileTypeAttribute.IsNullOrEmpty()) // If File type attribute is already added  or View Docuemnt Type attribute
                    return new ApplicantComplianceDocumentMap()
                    {
                        ApplicantComplianceAttributeID = lstApplicantComplianceAttributeData
                                                            .FirstOrDefault(x => x.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == FileUploadTypecode.ToLower()
                                                            ).ApplicantComplianceAttributeID,
                        ApplicantDocumentID = applicantDocumentId,
                        IsDeleted = false,
                        CreatedByID = CurrentLoggedInUserId,
                        CreatedOn = DateTime.Now,
                        IsIncompleteItem = false
                    };
                else // If File type attribute is added for first time
                {
                    return new ApplicantComplianceDocumentMap()
                    {
                        ApplicantComplianceAttributeID = AppConsts.NONE, //Id of the Applicant Compliance AttributeId
                        ApplicantDocumentID = applicantDocumentId,
                        IsDeleted = false,
                        CreatedByID = CurrentLoggedInUserId,
                        CreatedOn = DateTime.Now,
                        IsIncompleteItem = true
                    };
                }
            }
            else
            {
                return new ApplicantComplianceDocumentMap()
                {
                    ApplicantComplianceAttributeID = AppConsts.NONE, //Id of the Applicant Compliance AttributeId
                    ApplicantDocumentID = applicantDocumentId,
                    IsDeleted = false,
                    CreatedByID = CurrentLoggedInUserId,
                    CreatedOn = DateTime.Now,
                    IsIncompleteItem = true
                };
            }

        }

        /// <summary>
        /// Checks whether o show this control on screen or not.
        /// </summary>
        private void DisplayUploadControl()
        {
            if (!IsException)
            {
                String FileUploadTypecode = ComplianceAttributeDatatypes.FileUpload.GetStringValue();

                if ((lstApplicantComplianceAttributeData.IsNullOrEmpty() || !lstApplicantComplianceAttributeData.Any(cond => cond.AttributeTypeCode == FileUploadTypecode))
                    && !this.IsFileUploadApplicable)
                /* This case cannot be used as we can have a situation when we don;t have any entry for DOC type attribute 
                 * record in the ApplicantComplianceItemDataId but DOC type attribute existis in the Item */
                //    ||
                //(!lstApplicantComplianceAttributeData.IsNullOrEmpty() && lstApplicantComplianceAttributeData.Where(x => x.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower() == ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower() && !x.IsDeleted).FirstOrDefault().IsNull())
                //)
                {
                    if (IsViewDocApplicable)
                    {
                        //dvBrowse.Style.Add("display", "none");
                        //divAssignDocCls.Style.Add("display", "none");
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
                }

            }
            else
            {
                BindDocumentList(false);
            }
        }

        /// <summary>
        /// Bind the repeaters on the scree.
        /// </summary>
        private void BindDocumentList(Boolean refreshData = true)
        {
            if (refreshData)
            {
                Presenter.getData();
            }
            List<ApplicantDocuments> applicantDocument = new List<ApplicantDocuments>();
            applicantDocument = Presenter.GetItemRelatedDocument();

            var _screeningDocTypeAttrCode = ComplianceAttributeDatatypes.Screening_Document.GetStringValue();

            List<Int32> _lstScreeningTypeDocuments = applicantDocument.Where(ad => ad.DocumentType == this.ScreeningDocTypeId)
                                                              .Select(ad => ad.ApplicantDocumentId)
                                                              .ToList();

            List<Int32> _lstUnMappedDocuments = new List<Int32>();

            if (!_lstScreeningTypeDocuments.IsNullOrEmpty())
            {
                // For Exception case, 'Screening' Document can not get added to display list. For normal Items,
                // 'applicantDocument' list is being used with repeater, which also contains the 'Screening Documnents'
                if (!IsException)
                {
                    if (lstApplicantComplianceDocumentMaps.IsNull())
                    {
                        // This will allow execution of the NEXT statement even when 'lstApplicantComplianceDocumentMaps' is Empty
                        // i.e. No document was mapped with the current item. So evaluation of 'lstApplicantComplianceDocumentMaps' will not 
                        // give 'NULL' reference exception.
                        lstApplicantComplianceDocumentMaps = new List<ApplicantComplianceDocumentMap>();
                    }

                    // Add Screening Documents to "Removal List", which are part of Item, but not attached with attribute "File Upload" type.
                    _lstUnMappedDocuments.AddRange(_lstScreeningTypeDocuments.Where(docId => !lstApplicantComplianceDocumentMaps
                                                                             .Any(acdm => acdm.ApplicantDocumentID == docId))
                                         .ToList());
                }
            }


            rptrDocuments.DataSource = applicantDocument.Where(ad => !_lstUnMappedDocuments.Any(umdoc => umdoc == ad.ApplicantDocumentId)).ToList()
                                                        .DistinctBy(ad => ad.ApplicantDocumentId)
                                                        .ToList();
            rptrDocuments.DataBind();
            List<AllApplicantDocuments> allApplicantDocuments = new List<AllApplicantDocuments>();

            if (lstApplicantDocument.IsNotNull() && lstApplicantDocument.Count > 0 && !_isDocumentAssignedOnly)//UAT-2768
            {
                ApplicantDocuments viewTypeDoc = lstApplicantDocument.Where(x => x.IsViewDocType == AppConsts.ONE && x.ItemID == ItemDataId).FirstOrDefault();
                foreach (var item in lstApplicantDocument.DistinctBy(x => x.ApplicantDocumentId))
                {
                    //UAT 1582 Completed "View Document" should automatically associate with FileUpload attribute upon completion. 
                    if (viewTypeDoc.IsNotNull() && viewTypeDoc.ApplicantDocumentId == item.ApplicantDocumentId)
                    {
                        allApplicantDocuments.Add(new AllApplicantDocuments(viewTypeDoc.ApplicantDocumentId, viewTypeDoc.DocumentName, IsCheckedDocument(viewTypeDoc.ApplicantDocumentId), viewTypeDoc.IsViewDocType, viewTypeDoc.ItemID));
                    }
                    else
                    {
                        String extension = System.IO.Path.GetExtension(item.DocumentName);
                        //(rptrAllDocuments.FindControl("lblErrorMessage") as Label).Text = "unsuppted";


                        allApplicantDocuments.Add(new AllApplicantDocuments(item.ApplicantDocumentId, item.DocumentName, IsCheckedDocument(item.ApplicantDocumentId), item.IsViewDocType, item.ItemID));

                    }
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
            if (IsException)
            {
                if (lstExceptionDocumentDocumentMaps.IsNotNull())
                    return lstExceptionDocumentDocumentMaps.FirstOrDefault(x => x.ApplicantDocumentID == applicationDocumentId) == null ? false : true;
            }

            else
            {
                if (lstApplicantComplianceDocumentMaps.IsNotNull())
                    return lstApplicantComplianceDocumentMaps.FirstOrDefault(x => x.ApplicantDocumentID == applicationDocumentId) == null ? false : true;
            }
            return false;
        }

        /// <summary>
        /// This updates the mapping of the document with the item.
        /// </summary>
        private void UpdateApplicantComplianceDocumentMaps(Boolean isUIValidationPassed, Boolean isDocumentAssignedOnly = false, Boolean isNotApprovedStatus = false)
        {
            //This list adds the new mapping in ApplicantComplianceDocumentMap table. 
            List<ApplicantComplianceDocumentMap> ToAddDocumentMap = new List<ApplicantComplianceDocumentMap>();
            //This list adds the new mapping in ExceptionDocumentMapping table.
            List<ExceptionDocumentMapping> ToAddDocumentMapException = new List<ExceptionDocumentMapping>();
            //This deletes the mapping fom ExceptionDocumentMapping or ApplicantComplianceDocumentMap table.
            List<Int32> ToDeleteApplicantComplianceDocumentMapIDs = new List<Int32>();
            ApplicantComplianceDocumentMap documentMap = null;
            ExceptionDocumentMapping exdocumentMap = null;

            foreach (RepeaterItem item in rptrAllDocuments.Items)
            {
                //Int32 applicantDocumentId = Convert.ToInt32(item.GetDataKeyValue("ApplicantDocumentID"));
                var chkIsMapped = (CheckBox)item.FindControl("chkIsMapped");
                var hdnDocumentId = (HiddenField)item.FindControl("hdnDocumentId");

                Int32 applicantDocumentId = Convert.ToInt32(hdnDocumentId.Value);
                if (IsException)
                {
                    exdocumentMap = lstExceptionDocumentDocumentMaps.FirstOrDefault(x => x.ApplicantDocumentID == applicantDocumentId);
                    if (exdocumentMap == null && chkIsMapped.Checked)
                    {
                        ToAddDocumentMapException.Add(InstanceOfDocumentMapException(applicantDocumentId));
                    }
                    else if (exdocumentMap != null && !chkIsMapped.Checked)
                    {
                        ToDeleteApplicantComplianceDocumentMapIDs.Add(exdocumentMap.ExceptionDocumentMappingID);
                    }
                }
                else
                {
                    if (lstApplicantComplianceDocumentMaps.IsNotNull())
                        documentMap = lstApplicantComplianceDocumentMaps.FirstOrDefault(x => x.ApplicantDocumentID == applicantDocumentId);

                    if (documentMap == null && chkIsMapped.Checked)            // Add new document
                    {
                        ToAddDocumentMap.Add(InstanceOfDocumentMap(applicantDocumentId));
                    }
                    else if (documentMap != null && !chkIsMapped.Checked)
                    {
                        ToDeleteApplicantComplianceDocumentMapIDs.Add(documentMap.ApplicantComplianceDocumentMapID);
                    }
                }
            }

            //UAT-2768
            if (isDocumentAssignedOnly && ToDeleteApplicantComplianceDocumentMapIDs.Count > AppConsts.NONE && !isNotApprovedStatus)
            {
                _isValidAssignUnAssignDocument = isUIValidationPassed = ValidateAssignUnAssignDocument(true);
                if (!isUIValidationPassed && _ItemDataID == AppConsts.NONE)
                {
                    _ItemDataID = -1;
                }
            }

            // Update assign/un-assign only if validation is passed
            if ((isUIValidationPassed && !IsException) || IsException)
            {
                String _fileUploadTypecode = ComplianceAttributeDatatypes.FileUpload.GetStringValue();
                String _viewDocTypecode = ComplianceAttributeDatatypes.View_Document.GetStringValue();

                ApplicantComplianceAttributeData _fileTypeAttribute = null;
                ApplicantComplianceAttributeData _viewDocTypeAttribute = null;


                if (!lstApplicantComplianceAttributeData.IsNullOrEmpty())
                {
                    _fileTypeAttribute = lstApplicantComplianceAttributeData
                                            .Where(x => x.ComplianceAttribute != null // Document type attribute is added as new attribute, so null check is required
                                                && x.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower()
                                            == _fileUploadTypecode.ToLower()).FirstOrDefault();
                    _viewDocTypeAttribute = lstApplicantComplianceAttributeData
                                           .Where(x => x.ComplianceAttribute != null // Document type attribute is added as new attribute, so null check is required
                                               && x.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower()
                                           == _viewDocTypecode.ToLower()).FirstOrDefault();

                }
                // ONLY THIS PROPERTY CANNOT BE USED AS A PARAMTER FOR INCOMPLETE ITEM IDENTIFICATION.
                // Case when ONLY documents are assigned by admin for any item, also remains as Incomplete item status.
                // So item data gets inserted and in that case also 'AssignUnAssignIncompleteItemDocuments' is executed
                // So this case is handled inside the 'AssignUnAssignIncompleteItemDocuments' method

                if (ToAddDocumentMap.Count > AppConsts.NONE || ToAddDocumentMapException.Count > AppConsts.NONE || ToDeleteApplicantComplianceDocumentMapIDs.Count > AppConsts.NONE)
                {

                    if ((!this.IsIncompleteItem && !lstApplicantComplianceAttributeData.IsNullOrEmpty() && (!_fileTypeAttribute.IsNullOrEmpty() || !_viewDocTypeAttribute.IsNullOrEmpty()))
                       || IsException)
                    {
                        // Handles the cases when : 
                        // 1. Documents are getting added for second time or so on, when other attributes have been already added
                        // 2. Exception documents are being assigned/un-assigned 
                        Presenter.AssignUnAssignItemDocuments(ToAddDocumentMap, ToAddDocumentMapException, ToDeleteApplicantComplianceDocumentMapIDs, IsException, CurrentLoggedInUserId);

                        //UAT-2618:
                        Presenter.UpdateIsDocumentAssociated();
                    }
                    else
                    {
                        // NO EXCEPTION ITEM is handled by this. Handles the cases when :
                        // 1. If documents are being added for Incomplete items, either first or second time, without any data added for any other Attribute
                        // 2. Documents are being assigned to an item for the first time, when other attributes have been already added. 
                        //    This is because, this method internally manages to ADD NEW data in ApplicantComplianceAttributeData, which above condition method cannot do.
                        Presenter.AssignUnAssignIncompleteItemDocuments(ToAddDocumentMap, ToDeleteApplicantComplianceDocumentMapIDs, CurrentLoggedInUserId); // No Exception for Incomplete Items by ADMINs

                        //UAT-2618:
                        Presenter.UpdateIsDocumentAssociated();
                    }

                    if (!ToAddDocumentMap.IsNullOrEmpty() && !ToAddDocumentMap.FirstOrDefault().ApplicantComplianceAttributeData.IsNullOrEmpty())
                    {
                        _ItemDataID = ToAddDocumentMap.FirstOrDefault().ApplicantComplianceAttributeData.ApplicantComplianceItemID;
                    }
                    if (_ItemDataID <= 0 && !ToAddDocumentMapException.IsNullOrEmpty() && !ToAddDocumentMapException.FirstOrDefault().ApplicantComplianceItemData.IsNullOrEmpty())
                    {
                        _ItemDataID = ToAddDocumentMapException.FirstOrDefault().ApplicantComplianceItemData.ApplicantComplianceItemID;
                    }
                    if (_ItemDataID == AppConsts.NONE && ToDeleteApplicantComplianceDocumentMapIDs.Count > AppConsts.NONE)
                    {
                        _ItemDataID = -1;
                    }
                }

            }
        }

        private void AddUpdateMethodToDelegate()
        {
            if (HttpContext.Current.Items["UpdateDocumentList"] == null)
            {
                del = new UpdateDocumentList(UpdateDocumentList);
                HttpContext.Current.Items["UpdateDocumentList"] = del;
            }
            else
            {
                del = (UpdateDocumentList)HttpContext.Current.Items["UpdateDocumentList"];
                del += new UpdateDocumentList(UpdateDocumentList);
                HttpContext.Current.Items["UpdateDocumentList"] = del;
            }
        }

        public void UpdateDocumentList(List<ApplicantDocuments> lstUpdatedApplicantDocument)
        {
            lstApplicantDocument = lstUpdatedApplicantDocument;
            BindDocumentList();
        }

        private Boolean ValidateNewDocumentUpload(Int32 documentCount)
        {
            documentCount = documentCount + rptrDocuments.Items.Count;

            String _uiValidationResult = String.Empty;
            if (IsException || !this.IsUIValidationApplicable)
                return true;

            _uiValidationResult = Presenter.ValidateDocumentMappingRules(documentCount);

            if (!String.IsNullOrEmpty(_uiValidationResult))
            {
                lblMessage.Text = _uiValidationResult;
                lblMessage.CssClass = "error";
                return false;
            }
            else
            {
                lblMessage.Text = String.Empty;
                return true;
            }
        }

        private Boolean ValidateAssignUnAssignDocument(Boolean isNeedToShowValidationMsg = true)
        {
            String _uiValidationResult = String.Empty;

            if (IsException || !this.IsUIValidationApplicable)
                return true;

            Int32 _totalmappedDocumentCount = 0;
            foreach (RepeaterItem item in rptrAllDocuments.Items)
            {
                var chkIsMapped = (CheckBox)item.FindControl("chkIsMapped");
                if (chkIsMapped.Checked)
                    _totalmappedDocumentCount++;
            }
            //UAT-2768:
            _totatDocumentMapped = _totalmappedDocumentCount;
            _uiValidationResult = Presenter.ValidateDocumentMappingRules(_totalmappedDocumentCount);

            if (!String.IsNullOrEmpty(_uiValidationResult))
            {
                if (isNeedToShowValidationMsg)
                {
                    lblMessage.Text = _uiValidationResult;
                    lblMessage.CssClass = "error";
                }
                return false;
            }
            else
            {
                lblMessage.Text = String.Empty;
                return true;
            }
        }

        private Boolean ValidateRemoveDocument()
        {
            String _uiValidationResult = String.Empty;
            if (IsException || !this.IsUIValidationApplicable)
                return true;

            _uiValidationResult = Presenter.ValidateDocumentMappingRules(rptrDocuments.Items.Count - 1);

            if (!String.IsNullOrEmpty(_uiValidationResult))
            {
                lblMessage.Text = _uiValidationResult;
                lblMessage.CssClass = "error";
                return false;
            }
            else
            {
                lblMessage.Text = String.Empty;
                return true;
            }
        }

        /// <summary>
        /// Call the multiCast delegate to update the list of documents.
        /// </summary>
        public void UpdateNewDocumentList()
        {
            Presenter.GetApplicantDocuments();
            if (HttpContext.Current.Items["UpdateDocumentList"] != null && _isUpdateSessionList)//UAT-2768
            {
                del = (UpdateDocumentList)HttpContext.Current.Items["UpdateDocumentList"];
                del(lstApplicantDocument);
            }

            Presenter.UpdateMappingList();
            BindDocumentList();
            ShowHideDeleteCheckBox(false, ComplianceItemId);
            //_presenter.CallParallelTaskPdfConversionMerging();

        }

        /// <summary>
        /// Call the multiCast delegate to update the list of documents.
        /// </summary>
        public void ShowHideDeleteCheckBox(Boolean isDeleteApplicable, Int32 complianceItemId)
        {
            if (HttpContext.Current.Items["ShowDeleteCheckBox"] != null)
            {
                showChkBox = (ShowDeleteCheckBox)HttpContext.Current.Items["ShowDeleteCheckBox"];
                showChkBox(isDeleteApplicable, complianceItemId);
            }
            BindDocumentList();
        }

        private void GetAllowedFileExtensions()
        {
            #region UAT-4067
            Presenter.GetSelectedNodeIDBySubscriptionID(SelectedTenantId_Global, PackageSubscriptionId);
            Presenter.GetAllowedFileExtensions();
            #endregion
            //Dictionary<String, String> arguments = new Dictionary<String, String>();
            //if (Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT] != null)
            //{
            //    arguments.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
            //    if (arguments.ContainsKey("allowedFileExtensions"))
            //    {
            //        AllowedExtensions = arguments["allowedFileExtensions"];
            //        //String[] allowedExtensions = allowedFileExtensions.Split(',');

            //    }
            //    else
            //    {
            //        AllowedExtensions = AppConsts.NOT_AVAILABLE;
            //    }
            //}

        }
        #endregion

        #region Public Methods

        public void RebindAfterSave(Boolean isReadOnlyAfterSave)
        {
            //lnkbtnViewAll.Visible = false;
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

        /// <summary>
        /// It upload and maps the document added by the user.
        /// </summary> 
        public void UploadAllDocuments()
        {
            if (uploadControl.UploadedFiles.Count > 0 && ValidateNewDocumentUpload(uploadControl.UploadedFiles.Count))
            {
                String filePath = String.Empty;
                ApplicantDocument applicantDocument = null;
                Boolean aWSUseS3 = false;
                String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];

                //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                StringBuilder corruptedFileMessage = new StringBuilder();

                if (!WebConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                {
                    aWSUseS3 = Convert.ToBoolean(WebConfigurationManager.AppSettings["AWSUseS3"]);
                }
                if (tempFilePath.IsNullOrEmpty())
                {
                    base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                    return;
                }
                if (!tempFilePath.EndsWith(@"\"))
                {
                    tempFilePath += @"\";
                }
                tempFilePath += "Tenant(" + SelectedTenantId_Global.ToString() + @")\";

                if (!Directory.Exists(tempFilePath))
                    Directory.CreateDirectory(tempFilePath);

                //Check whether use AWS S3, true if need to use
                if (aWSUseS3 == false)
                {
                    if (filePath.IsNullOrEmpty())
                    {
                        base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", new Exception());
                        return;
                    }
                    if (!filePath.EndsWith("\\"))
                    {
                        filePath += "\\";
                    }
                    filePath += "Tenant(" + SelectedTenantId_Global.ToString() + @")\";

                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);
                }
                foreach (UploadedFile item in uploadControl.UploadedFiles)
                {
                    applicantDocument = new ApplicantDocument();
                    String fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    //Save file
                    String newTempFilePath = Path.Combine(tempFilePath, fileName);
                    item.SaveAs(newTempFilePath);
                    if (ToSaveApplicantUploadedDocuments == null)
                    {
                        ToSaveApplicantUploadedDocuments = new List<ApplicantDocument>();
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
                            base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", new Exception());
                            return;
                        }
                        if (!filePath.EndsWith("//"))
                        {
                            filePath += "//";
                        }

                        //AWS code to save document to S3 location
                        AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                        String destFolder = filePath + "Tenant(" + SelectedTenantId_Global.ToString() + @")/";
                        String returnFilePath = objAmazonS3.SaveDocument(newTempFilePath, fileName, destFolder);
                        //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                        if (returnFilePath.IsNullOrEmpty())
                        {
                            corruptedFileMessage.Append("Your file " + item.FileName + " is not uploaded. \\n");
                            continue;
                        }
                        applicantDocument.DocumentPath = returnFilePath; //Path.Combine(destFolder, fileName);
                    }
                    try
                    {
                        if (!String.IsNullOrEmpty(newTempFilePath))
                            File.Delete(newTempFilePath);
                    }
                    catch (Exception) { }
                    applicantDocument.OrganizationUserID = ApplicantId;
                    applicantDocument.FileName = item.FileName;
                    applicantDocument.Size = item.ContentLength;
                    applicantDocument.CreatedByID = CurrentLoggedUserID;
                    applicantDocument.CreatedOn = DateTime.Now;
                    applicantDocument.IsDeleted = false;
                    applicantDocument.DataEntryDocumentStatusID = DataEntryDocNewStatusId; //Set Data Entry Document Status in applicant Document [UAT-1049:Admin Data Entry]

                    ToSaveApplicantUploadedDocuments.Add(applicantDocument);
                }
                String newFilePath = String.Empty;
                if (aWSUseS3 == false)
                {
                    newFilePath = filePath;
                }
                else
                {
                    if (filePath.IsNullOrEmpty())
                    {
                        base.LogError("Please provide path for " + AppConsts.APPLICANT_FILE_LOCATION + " in config.", new Exception());
                        return;
                    }
                    if (!filePath.EndsWith("//"))
                    {
                        filePath += "//";
                    }
                    newFilePath = filePath + "Tenant(" + SelectedTenantId_Global.ToString() + @")/";
                }
                foreach (var item in ToSaveApplicantUploadedDocuments)
                {
                    Int32 applicantDocumentId = Presenter.AddApplicantDocument(item);
                    //For the string MMDDYYYY_MMSS
                    String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss");
                    String newFileName = newFilePath + "UD_" + SelectedTenantId_Global.ToString() + "_" + applicantDocumentId.ToString() + "_" + date + Path.GetExtension(item.FileName);

                    if (_presenter.UpdateDocumentPath(newFileName, item.DocumentPath, applicantDocumentId, item.OrganizationUserID.Value))
                    {
                        ApplicantComplianceDocumentMap ToAddDocumentMap = null;
                        ExceptionDocumentMapping ToAddDocumentMapException = null;

                        List<Int32> ToDeleteApplicantComplianceDocumentMapIDs = new List<Int32>();
                        if (IsException)
                        {
                            ToAddDocumentMapException = new ExceptionDocumentMapping();
                            ToAddDocumentMapException = InstanceOfDocumentMapException(applicantDocumentId);
                        }
                        else
                        {
                            ToAddDocumentMap = new ApplicantComplianceDocumentMap();
                            ToAddDocumentMap = InstanceOfDocumentMap(applicantDocumentId);
                        }
                        Presenter.UpdateApplicantComplianceNewDocumentMaps(ToAddDocumentMap, ToAddDocumentMapException);
                        //UAT-2618:
                        Presenter.UpdateIsDocumentAssociated();
                    }
                }

                if (ToSaveApplicantUploadedDocuments.IsNotNull() && ToSaveApplicantUploadedDocuments.Count() > 0)
                {
                    Presenter.CallParallelTaskPdfConversionMerging();
                }

                if (OnCompletedUpload != null)
                    OnCompletedUpload();
                //UAT-862 :- WB: As a student or an admin, I should not be allowed to upload documents with a size of 0 
                if ((corruptedFileMessage.Length > 0 && !(corruptedFileMessage.ToString().IsNullOrEmpty())))
                {
                    corruptedFileMessage.Append("Please again upload these documents .");
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowCallBackMessage('" + corruptedFileMessage.ToString() + "');", true);
                }
            }
        }


        /// <summary>
        /// It is used to set focus on "ucVerificationItemDataPanel" middle pane.
        /// </summary> 
        public void SetFocusOnParent()
        {
            String key = "CategoryPanel";
            if (HttpContext.Current.Items[key].IsNotNull())
            {
                INTERSOFT.WEB.UI.WebControls.WclSplitter categoryPanel = (INTERSOFT.WEB.UI.WebControls.WclSplitter)HttpContext.Current.Items[key];
                if (categoryPanel.IsNullOrEmpty())
                    this.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Focus(); // to set focus on "ucVerificationItemDataPanel" middle pane
                else
                {
                    Page.FindControl(categoryPanel.UniqueID).Focus(); // to set focus on "ucVerificationItemDataPanel" middle pane
                }
            }
        }

        #endregion
        #endregion

        #region Events

        /// <summary>
        /// Button click to save all the new added documents.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar1_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                UploadAllDocuments();
                UpdateNewDocumentList();
                SetFocusOnParent();
                //BindDocumentList();
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
        protected void comandSaveMapping_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                UpdateApplicantComplianceDocumentMaps(ValidateAssignUnAssignDocument());
                UpdateNewDocumentList();

                //BindDocumentList();
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
        /// Removes the document mapping with the item.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rptrDocuments_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            Boolean _isValidated = true;
            if (e.CommandName == "remove")
            {
                Int32 applicantDocumentId = Convert.ToInt32(e.CommandArgument);
                Int32 updateId = 0;
                if (IsException)
                {
                    updateId = lstExceptionDocumentDocumentMaps.FirstOrDefault(x => x.ApplicantDocumentID == applicantDocumentId) == null ? 0 :
                        lstExceptionDocumentDocumentMaps.FirstOrDefault(x => x.ApplicantDocumentID == applicantDocumentId).ExceptionDocumentMappingID;
                }
                else
                {
                    updateId = lstApplicantComplianceDocumentMaps.FirstOrDefault(x => x.ApplicantDocumentID == applicantDocumentId) == null ? 0 :
                        lstApplicantComplianceDocumentMaps.FirstOrDefault(x => x.ApplicantDocumentID == applicantDocumentId).ApplicantComplianceDocumentMapID;
                }
                _isValidated = ValidateRemoveDocument();
                if (_isValidated)
                    Presenter.RemoveMapping(updateId, CurrentLoggedInUserId, IsException);

                SetFocusOnParent();
            }
            UpdateNewDocumentList();
            hdnIsRestrictedFileTypeChecked.Value = "false";
            foreach (var index in lstApplicantDocument)
            {
                String FileType = index.DocumentName.Split(',').LastOrDefault();
                if (AllowedFileExtensions.Contains(FileType.ToLower()) && AllowedFileExtensions.Contains(FileType.ToUpper()))
                {
                    hdnIsRestrictedFileTypeChecked.Value = "true";
                    break;
                }
            }

            //BindDocumentList();
            //UpdateApplicantComplianceDocumentMaps(_isValidated);
        }

        protected void rptrAllDocuments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (IsReadOnly)
            {
                CheckBox chkIsMapped = e.Item.FindControl("chkIsMapped") as CheckBox;
                chkIsMapped.Enabled = false;
                comandSaveMapping.SaveButton.Enabled = false;
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                AllApplicantDocuments applicantDoc = (AllApplicantDocuments)e.Item.DataItem;
                //UAT 1582 Completed "View Document" should automatically associate with FileUpload attribute upon completion. 
                if (Convert.ToBoolean(applicantDoc.IsViewDocType) && Convert.ToInt32(applicantDoc.ItemId) == ItemDataId)
                {
                    CheckBox chkIsMapped = e.Item.FindControl("chkIsMapped") as CheckBox;
                    chkIsMapped.Enabled = false;

                }

                String extenesion = System.IO.Path.GetExtension(applicantDoc.DocumentName);
                extenesion = extenesion.Remove(".");
                if (!AllowedFileExtensions.IsNullOrEmpty())
                {
                    if (!AllowedFileExtensions.IsNullOrEmpty() && AllowedFileExtensions.Count > AppConsts.NONE && !AllowedFileExtensions.Contains(extenesion.ToLower()) && !AllowedFileExtensions.Contains(extenesion.ToUpper()))
                    {
                        CheckBox chkIsMapped = e.Item.FindControl("chkIsMapped") as CheckBox;
                        Label lblErrorMessage = e.Item.FindControl("lblErrorMessage") as Label;
                        lblErrorMessage.Text = "!Error: Unsupported File Format";

                        if (chkIsMapped.Checked)
                        {
                            hdnIsRestrictedFileTypeChecked.Value = "true";
                        }
                    }
                }
                else
                {
                    Label lblErrorMessage = e.Item.FindControl("lblErrorMessage") as Label;
                    lblErrorMessage.Text = "!Error: Unsupported File Format";
                    hdnIsRestrictedFileTypeChecked.Value = "true";

                }
            }
        }

        protected void rptrDocuments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (IsReadOnly)
            {
                LinkButton lnkBtnRemove = e.Item.FindControl("lnkbtnDelete") as LinkButton;
                lnkBtnRemove.Enabled = false;
            }
            //UAT 1582 Completed "View Document" should automatically associate with FileUpload attribute upon completion. 
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ApplicantDocuments applicantDoc = (ApplicantDocuments)e.Item.DataItem;

                if (Convert.ToBoolean(applicantDoc.IsViewDocType) && Convert.ToInt32(applicantDoc.ItemID) == ItemDataId)
                {
                    LinkButton lnkBtnRemove = e.Item.FindControl("lnkbtnDelete") as LinkButton;
                    Literal litSymbol2 = e.Item.FindControl("litSymbol2") as Literal;
                    lnkBtnRemove.Visible = false;
                    litSymbol2.Visible = false;
                }
            }
        }

        #region UAT-2768:
        public Dictionary<String, String> AssignDocument(Boolean isUpdateSessionList, Boolean needToFireUIRule, Boolean isUpdateOnlySessionList, Boolean isNotApprovedStatus = false)
        {
            Boolean isValidAssignUnAssignDocument = true;
            _isValidAssignUnAssignDocument = isValidAssignUnAssignDocument;
            Dictionary<String, String> dicAsisgnDocumentSave = new Dictionary<String, String>();

            if (!IsReadOnly)
            {
                _isDocumentAssignedOnly = true;
                _isUpdateSessionList = isUpdateSessionList;
                if (!isUpdateOnlySessionList)
                {
                    if ((IsFileUploadApplicable && !IsException) || IsException)
                    {

                        isValidAssignUnAssignDocument = needToFireUIRule ? ValidateAssignUnAssignDocument(false) : true;
                        _isValidAssignUnAssignDocument = isValidAssignUnAssignDocument;
                        if (IsAnyDocumentAssigned(isValidAssignUnAssignDocument))
                        {
                            UpdateApplicantComplianceDocumentMaps(isValidAssignUnAssignDocument, true, isNotApprovedStatus);
                        }
                    }
                }
                UpdateNewDocumentList();
            }
            dicAsisgnDocumentSave.Add("ValidAssignUnAssignDocument", _isValidAssignUnAssignDocument.ToString());
            dicAsisgnDocumentSave.Add("ItemDataID", _ItemDataID.ToString());

            return dicAsisgnDocumentSave;
        }

        private Boolean IsAnyDocumentAssigned(Boolean isValidAssignUnAssignDocument)
        {
            Boolean isAnyDocumentAssigned = false;

            if (IsException || isValidAssignUnAssignDocument)
            {
                isAnyDocumentAssigned = true;
            }
            else if (!isValidAssignUnAssignDocument)
            {
                if (_totatDocumentMapped > AppConsts.NONE)
                {
                    isAnyDocumentAssigned = true;
                }
            }
            return isAnyDocumentAssigned;
        }
        #endregion

        #endregion

        protected void chkIsMapped_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

