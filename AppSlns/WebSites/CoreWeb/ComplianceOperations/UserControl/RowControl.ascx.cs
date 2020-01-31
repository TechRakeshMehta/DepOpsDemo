using System;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class RowControl : BaseUserControl, IRowControlView
    {
        public event EventHandler IsFileUploadApplicable;

        #region Variables

        #region Private Variables

        private RowControlPresenter _presenter = new RowControlPresenter();

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        public RowControlPresenter Presenter
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
        public Boolean IsItemSeries
        {
            get;
            set;
        }

        public IRowControlView CurrentViewContext
        {
            get { return this; }
        }

        public List<ComplianceItemAttribute> ClientItemAttributes
        {
            get;
            set;
        }

        public Int32 NoOfAttributesPerRow
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

        public List<ApplicantComplianceAttributeData> ApplicantAttributeData
        {
            get;
            set;
        }

        public Int32 ApplicantComplianceItemId
        {
            get;
            set;
        }

        //Implemented code for UAT-708
        public Int32 PackageId
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

        //UAT-4067
        public List<String> lstAllowedExtensions
        {
            get
            {
                if (!ViewState["lstAllowedExtensions"].IsNullOrEmpty())
                    return (List<String>)(ViewState["lstAllowedExtensions"]);
                return new List<String>();
            }
            set
            {
                ViewState["lstAllowedExtensions"] = value;
            }
        }

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

            CurrentViewContext.ClientItemAttributes = ClientItemAttributes;

            System.Web.UI.HtmlControls.HtmlGenericControl divGeneric = new System.Web.UI.HtmlControls.HtmlGenericControl();
            if (NoOfAttributesPerRow == 2)
            {
                divGeneric = div2Control;
            }

            Int32 count = 0;
            foreach (var attribute in CurrentViewContext.ClientItemAttributes)
            {
                count = count + 1;
                //Implemented code for UAT-708
                if (attribute.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower().Trim() == ComplianceAttributeDatatypes.Date.GetStringValue().ToLower().Trim())
                {
                    var prevAttribute = CurrentViewContext.ClientItemAttributes.FirstOrDefault();
                    divInstructionTextMain.ID = "divInstructionTextMain_" + attribute.CIA_ID;

                    HiddenField hdnCIA_ID = new HiddenField();
                    hdnCIA_ID.ID = "hdnInstBoxCIA_ID_" + attribute.CIA_ID;
                    hdnCIA_ID.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    hdnCIA_ID.Value = divInstructionTextMain.ID;
                    divInstructionTextControl.Controls.Add(hdnCIA_ID);
                    if (count == 2)
                    {
                        HiddenField tempHdnField = divInstructionTextControl.FindControl("hdnInstBoxCIA_ID_" + prevAttribute.CIA_ID) as HiddenField;
                        if (tempHdnField.IsNotNull())
                            tempHdnField.Value = divInstructionTextMain.ID;
                    }
                    if (count == 2 && prevAttribute.ComplianceAttribute.lkpComplianceAttributeDatatype.Code.ToLower().Trim() != ComplianceAttributeDatatypes.Date.GetStringValue().ToLower().Trim())
                    {
                        System.Web.UI.HtmlControls.HtmlGenericControl divInstTextLabel = new System.Web.UI.HtmlControls.HtmlGenericControl();
                        divInstTextLabel.ID = "divInstTextLabel_" + prevAttribute.CIA_ID;
                        divInstTextLabel.Attributes.Add("class", "sxlb sxlbHideBackground");
                        divInstructionTextControl.Controls.Add(divInstTextLabel);

                        System.Web.UI.HtmlControls.HtmlGenericControl divInsTextPrev = new System.Web.UI.HtmlControls.HtmlGenericControl();
                        divInsTextPrev.ID = "divInsTextPrev_" + prevAttribute.CIA_ID;
                        divInsTextPrev.Attributes.Add("class", "sxlm");
                        //divInstructionTextMain.Style["Display"] = "block";
                        divInstructionTextControl.Controls.Add(divInsTextPrev);
                    }
                    String instructionText = Presenter.GetInstructionText(attribute.CIA_AttributeID, attribute.CIA_ID);
                    System.Web.UI.HtmlControls.HtmlGenericControl divInscructionTextLabel = new System.Web.UI.HtmlControls.HtmlGenericControl();
                    divInscructionTextLabel.ID = "divInscructionTextLabel_" + attribute.CIA_ID;
                    divInscructionTextLabel.Attributes.Add("class", "sxlb sxlbHideBackground");
                    divInstructionTextControl.Controls.Add(divInscructionTextLabel);

                    System.Web.UI.HtmlControls.HtmlGenericControl divInscructionText = new System.Web.UI.HtmlControls.HtmlGenericControl();
                    divInscructionText.ID = "divInscructionText_" + attribute.CIA_ID;
                    divInscructionText.Attributes.Add("class", "sxlm");

                    if (!instructionText.IsEmpty())
                    {
                        System.Web.UI.HtmlControls.HtmlGenericControl divInsTemp = new System.Web.UI.HtmlControls.HtmlGenericControl();
                        divInsTemp.ID = "divInsTemp" + attribute.CIA_ID;
                        divInsTemp.Attributes.Add("class", "instBox");
                        divInsTemp.InnerText = instructionText;
                        divInsTemp.Attributes.Add("onclick", "HideInstructionMessageDiv('" + attribute.CIA_ID + "');");
                        divInscructionText.Controls.Add(divInsTemp);
                        divInstructionTextMain.Style["Display"] = "block";
                    }
                    divInstructionTextControl.Controls.Add(divInscructionText);

                }

                System.Web.UI.Control attributeControl = Page.LoadControl("~\\ComplianceOperations\\UserControl\\AttributeControl.ascx");
                (attributeControl as AttributeControl).ClientItemAttributes = attribute;
                //UAt-3806
                (attributeControl as AttributeControl).lstIsEditableBy = Presenter.GetEditableBiesByCategoryId();
                (attributeControl as AttributeControl).IsItemSeries = CurrentViewContext.IsItemSeries;
                (attributeControl as AttributeControl).ItemId = CurrentViewContext.ItemId;
                (attributeControl as AttributeControl).TenantId = CurrentViewContext.TenantId;

                (attributeControl as AttributeControl).IsFileUploadApplicable -= new EventHandler(RowControl_IsFileUploadApplicable);
                (attributeControl as AttributeControl).IsFileUploadApplicable += new EventHandler(RowControl_IsFileUploadApplicable);

                if (CurrentViewContext.ApplicantAttributeData.IsNotNull() && CurrentViewContext.ApplicantAttributeData.Count() > 0)
                {
                    (attributeControl as AttributeControl).ApplicantAttributeData = CurrentViewContext.ApplicantAttributeData.Where(
                        attr => attr.ApplicantComplianceItemID == CurrentViewContext.ApplicantComplianceItemId
                            && attr.ComplianceAttributeID == attribute.CIA_AttributeID && !attr.IsDeleted).FirstOrDefault();
                }

                //UAT-4067
               // (attributeControl as AttributeControl).lstAllowedExtensions = CurrentViewContext.lstAllowedExtensions;
                //END
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

        #endregion

        #region Methods

        #region Private Methods

        #endregion

        #region Public Methods

        #endregion

        #endregion
    }
}

