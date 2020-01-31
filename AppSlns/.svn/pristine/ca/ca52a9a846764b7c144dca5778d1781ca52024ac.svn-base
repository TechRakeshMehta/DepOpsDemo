using System;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using INTSOF.Utils;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;

namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public partial class RequirementRowControl : BaseUserControl, IRequirementRowControlView
    {
        public event EventHandler IsFileUploadApplicable;
        public event EventHandler IsViewDocumnetApplicable;
        public event EventHandler IsViewVideoApplicable;

        #region Variables

        #region Private Variables

        private RequirementRowControlPresenter _presenter = new RequirementRowControlPresenter();

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        public RequirementRowControlPresenter Presenter
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

        public IRequirementRowControlView CurrentViewContext
        {
            get { return this; }
        }

        public List<RequirementFieldContract> RequirementItemFields
        {
            get;
            set;
        }

        public Int32 NoOfFieldsPerRow
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the Master compliance Item (Client database), present in the ApplicantComplianceItem Entity. 
        /// </summary>
        public Int32 ItemId
        {
            get;
            set;
        }

        public Int32 TenantId
        {
            get;
            set;
        }

        public List<ApplicantRequirementFieldDataContract> ApplicantRequirementFieldData
        {
            get;
            set;
        }

        public Int32 ApplicantRequirementItemId
        {
            get;
            set;
        }

        //Implemented code for UAT-708
        public Int32 CategoryId
        {
            get;
            set;
        }

        public List<RequirementObjectTreeContract> LstRequirementObjTreeProperty { get; set; }

        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();

            CurrentViewContext.RequirementItemFields = RequirementItemFields;

            System.Web.UI.HtmlControls.HtmlGenericControl divGeneric = new System.Web.UI.HtmlControls.HtmlGenericControl();
            if (NoOfFieldsPerRow == 2)
            {
                divGeneric = div2Control;
            }

            foreach (var fields in CurrentViewContext.RequirementItemFields)
            {
                RequirementAttributeControl attributeControl = (RequirementAttributeControl)Page.LoadControl("~\\ApplicantRotationRequirement\\UserControl\\RequirementAttributeControl.ascx");
                attributeControl.RequirementFieldContract = fields;
                attributeControl.ItemId = CurrentViewContext.ItemId;
                attributeControl.TenantId = CurrentViewContext.TenantId;

                attributeControl.IsFileUploadApplicable -= new EventHandler(RowControl_IsFileUploadApplicable);
                attributeControl.IsFileUploadApplicable += new EventHandler(RowControl_IsFileUploadApplicable);

                attributeControl.IsViewDocumnetApplicable -= new EventHandler(RowControl_IsViewDocumnetApplicable);
                attributeControl.IsViewDocumnetApplicable += new EventHandler(RowControl_IsViewDocumnetApplicable);

                attributeControl.IsViewVideoApplicable -= new EventHandler(RowControl_IsViewVideoApplicable);
                attributeControl.IsViewVideoApplicable += new EventHandler(RowControl_IsViewVideoApplicable);
                attributeControl.LstRequirementObjTreeProperty = LstRequirementObjTreeProperty;

                if (CurrentViewContext.ApplicantRequirementFieldData.IsNotNull() && CurrentViewContext.ApplicantRequirementFieldData.Count() > 0)
                {
                    attributeControl.ApplicantFieldData = CurrentViewContext.ApplicantRequirementFieldData.Where(
                        attr => attr.RequirementItemDataID == CurrentViewContext.ApplicantRequirementItemId
                            && attr.RequirementFieldID == fields.RequirementFieldID).FirstOrDefault();
                }
                divGeneric.Controls.Add(attributeControl);
            }
        }

        void RowControl_IsFileUploadApplicable(object sender, EventArgs e)
        {
            if (IsFileUploadApplicable.IsNotNull())
            {
                IsFileUploadApplicable(this, e);
            }
        }

        void RowControl_IsViewDocumnetApplicable(object sender, EventArgs e)
        {
            if (IsViewDocumnetApplicable.IsNotNull())
            {
                IsViewDocumnetApplicable(this, e);
            }
        }

        void RowControl_IsViewVideoApplicable(object sender, EventArgs e)
        {
            if (IsViewVideoApplicable.IsNotNull())
            {
                IsViewVideoApplicable(this, e);
            }
        }

        #endregion

        #region Methods

        #region Private Methods

        #endregion

        #region Public Methods

        #endregion

        #endregion
    }
}

