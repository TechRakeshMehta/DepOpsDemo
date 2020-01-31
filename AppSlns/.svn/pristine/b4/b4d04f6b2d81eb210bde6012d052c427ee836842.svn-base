using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Collections.Generic;
using System.Web;
using INTSOF.Utils;
using System.Linq;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class VerificationDocumentControlReadOnlyMode : BaseUserControl, IVerificationDocumentControlReadOnlyModeView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private variables
        private VerificationDocumentControlReadOnlyModePresenter _presenter = new VerificationDocumentControlReadOnlyModePresenter();

        #endregion

        #endregion

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            hdnTenantIdReadOnly.Value = SelectedTenantId_Global.ToString();
            //set document Status ID as Merging Completed
            hdnMergingCompletedDocmntStatusID.Value = Convert.ToString(Presenter.GetDocumentStatusID());
            BindDocumentList();
            Presenter.OnViewLoaded();
        }
        #endregion

        #region Presenter object

        public VerificationDocumentControlReadOnlyModePresenter Presenter
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

        #region Methods

        #region Public Methods

        #endregion

        #region Private methods

        /// <summary>
        /// Bind repeater for the read only data.
        /// </summary>
        private void BindDocumentList()
        {
            List<ApplicantDocuments> applicantDocument = new List<ApplicantDocuments>();
            applicantDocument = Presenter.GetItemRelatedDocument();

            Presenter.GetScreeningDocumentTypeId();
            var _screeningDocTypeAttrCode = ComplianceAttributeDatatypes.Screening_Document.GetStringValue();

            List<Int32> _lstScreeningTypeDocuments = applicantDocument.Where(ad => ad.DocumentType == this.ScreeningDocTypeId)
                                                              .Select(ad => ad.ApplicantDocumentId)
                                                              .ToList();

            List<Int32> _lstUnMappedDocuments = new List<Int32>();

            if (!_lstScreeningTypeDocuments.IsNullOrEmpty())
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

            rptrDocuments.DataSource = applicantDocument.Where(ad => !_lstUnMappedDocuments.Any(umdoc => umdoc == ad.ApplicantDocumentId))
                                                        .ToList();
            rptrDocuments.DataBind();
        }

        #endregion

        #endregion

        #region Properties

        #region public properties
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

        public Int32 ItemDataId
        {
            get { return (Int32)(ViewState["ItemDataId"] == null ? 0 : ViewState["ItemDataId"]); }
            set { ViewState["ItemDataId"] = value; }
        }

        public Boolean IsException { get; set; }

        /// <summary>
        /// Tenant id
        /// </summary>
        public Int32 SelectedTenantId_Global
        {
            get
            {
                if (!ViewState["SelectedTenantIdDocRO"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedTenantIdDocRO"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedTenantIdDocRO"].IsNullOrEmpty())
                    ViewState["SelectedTenantIdDocRO"] = value;
            }
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

        /// <summary>
        /// Mapped documents from the Read Only control
        /// </summary>
        public List<ApplicantComplianceDocumentMap> lstApplicantComplianceDocumentMaps
        {
            get;
            set;
        }

        #endregion

        #region private properties
        #endregion

        #endregion
    }
}

