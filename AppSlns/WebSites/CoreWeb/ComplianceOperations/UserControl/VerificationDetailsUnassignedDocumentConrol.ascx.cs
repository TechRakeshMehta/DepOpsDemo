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
    public partial class VerificationDetailsUnassignedDocumentConrol : BaseUserControl, IVerificationDetailsUnassignedDocumentConrolView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private variables
        private VerificationDetailsUnassignedDocumentControlPresenter _presenter = new VerificationDetailsUnassignedDocumentControlPresenter();

        Int32 _disableViewDocumentID = AppConsts.NONE;
        Boolean _isDocuementToDisabled = false;

        //UAT-2768
        Boolean _isDocumentAssignedOnly = false;
        Boolean _isUpdateSessionList = true;
        Int32 _totatDocumentMapped = 0;


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
          
            BindDocumentList(false);

            hdnTenantIdInDocument.Value = SelectedTenantId_Global.ToString();
            //set document Status ID as Merging Completed
            hdnMergingCompletedDocStatusID.Value = Convert.ToString(Presenter.GetDocumentStatusID());
            Presenter.OnViewLoaded();
            SetFocusOnParent();
        }
        #endregion

        #region Presenter object

        public VerificationDetailsUnassignedDocumentControlPresenter Presenter
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

        #region public 
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


        public Int16 DataEntryDocNewStatusId { get; set; }

        #region Private Properties

        #endregion

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// Bind the repeaters on the scree.
        /// </summary>
        public void BindDocumentList(Boolean refreshData = true)
        {
            Presenter.GetScreeningDocumentTypeId();

            if (refreshData)
            {
                Presenter.GetApplicantDocuments();
            }
            List<ApplicantDocuments> applicantDocument = new List<ApplicantDocuments>();

            List<ApplicantDocuments> applicantAssignedDocument = Presenter.GetItemRelatedDocument();

            List<Int32> ApplicantDocumentIds = applicantAssignedDocument.Select(x => x.ApplicantDocumentId).ToList();

            if (ApplicantDocumentIds.IsNotNull() && ApplicantDocumentIds.Count > 0)
            {
                applicantDocument = lstApplicantDocument.Where(x => !ApplicantDocumentIds.Contains(x.ApplicantDocumentId)).ToList();
            }
            else
            {
                applicantDocument = lstApplicantDocument;
            }

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


            List<ApplicantDocuments> applicantDocuments = applicantDocument.Where(ad => !_lstUnMappedDocuments.Any(umdoc => umdoc == ad.ApplicantDocumentId)).ToList()
                                                        .DistinctBy(ad => ad.ApplicantDocumentId)
                                                        .ToList();

            rptrDocuments.DataSource = applicantDocuments;
            rptrDocuments.DataBind();

        }
    
        #endregion

        #region Public Methods

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

    }
}

