using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using System.Linq;
using Telerik.Web.UI;
using System.Web;
using System.Configuration;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.UI;
using Business.RepoManagers;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ReconciliationItemDataEditMode : BaseUserControl, IReconciliationItemDataEditModeView
    {
        #region Variables

        #region Private Variables

        private ReconciliationItemDataEditModePresenter _presenter = new ReconciliationItemDataEditModePresenter();

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties
        private Boolean ReloadingDataAfterSave = false;
        private Boolean TextChangedExecuted = false;

        #endregion

        #region Public Properties


        public ReconciliationItemDataEditModePresenter Presenter
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

        public IReconciliationItemDataEditModeView CurrentViewContext
        {
            get { return this; }
        }

        public List<ApplicantItemVerificationData> VerificationData
        {
            get;
            set;
        }

        public List<ReconciliationDetailsDataContract> lstReconciliationDetailsData { get; set; }

        /// <summary>
        /// Status of Item, when screen loads
        /// </summary>
        public String CurrentItemStatus
        {
            get { return hdfItemStatus.Value; }
        }

        //public String NewItemStatus
        //{
        //    get { return rbtnListAction.SelectedValue; }
        //}

        public String ItemMovementStatusCode
        {
            get;
            set;
        }

        public Int32 AttemptedItemStatusId
        {
            get { return _presenter.GetNewStatusId(rbtnListAction.SelectedValue); }
        }

        public Int32 CurrentStatusId
        {
            get;
            set;
        }


        public Int32 ApplicantItemDataId
        {
            get
            {
                return Convert.ToInt32(ViewState["ApplicantItemDataId"]);
            }
            set
            {
                ViewState["ApplicantItemDataId"] = value;
            }
        }

        public List<ApplicantDocuments> lstApplicantDocument
        {
            get
            {
                List<ApplicantDocuments> lstApplicantDocumentObj = new List<ApplicantDocuments>();
                if (ViewState["lstApplicantDocument"].IsNotNull())
                {
                    lstApplicantDocumentObj = (List<INTSOF.UI.Contract.ComplianceOperation.ApplicantDocuments>)ViewState["lstApplicantDocument"];
                }
                return lstApplicantDocumentObj;
            }
            set
            {
                ViewState["lstApplicantDocument"] = ucVerificationDocumentControlReadOnlyMode.lstApplicantDocument = value;
            }
        }

        public Int32 ApplicantCategoryDataId
        {
            get
            {
                return Convert.ToInt32(ViewState["ApplicantCategoryDataId"]);
            }
            set
            {
                ViewState["ApplicantCategoryDataId"] = value;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        public String Comments
        {
            get;
            set;
        }

        public String ItemComplianceStatusCode
        {
            get;
            set;
        }

        public Int32 ComplianceItemId
        {
            get;
            set;
        }

        public Int32 ComplianceCategoryId
        {
            get;
            set;
        }

        public String UIInputException
        {
            get;
            set;
        }

        public Int32 CurrentPackageSubscriptionId
        {
            get { return (Int32)(ViewState["CurrentPackageSubscriptionIdEM"]); }
            set { ViewState["CurrentPackageSubscriptionIdEM"] = value; }
        }

        /// <summary>
        /// If read only after save then disable the controls
        /// </summary>
        public Boolean IsReadOnlyAfterSave { get; set; }

        /// <summary>
        /// Identify if the controls is rebind after the save is done
        /// </summary>
        public Boolean IsRebindAfterSave { get; set; }

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

        public Boolean IsAdminReviewRequired
        {
            get { return Convert.ToBoolean((ViewState["IsAdminReviewRequired"])); }
            set { ViewState["IsAdminReviewRequired"] = value; }
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

        public List<ApplicantComplianceAttributeData> lstApplicantComplianceAttributeData
        {
            get;
            set;
        }

        public Int32 CurrentTenantId_Global
        {
            get
            {
                if (!ViewState["CurrentTenantIdEM"].IsNullOrEmpty())
                    return (Int32)(ViewState["CurrentTenantIdEM"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["CurrentTenantIdEM"].IsNullOrEmpty())
                    ViewState["CurrentTenantIdEM"] = value;
            }
        }

        public Int32 SelectedTenantId_Global
        {
            get
            {
                if (!ViewState["SelectedTenantIdEM"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedTenantIdEM"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedTenantIdEM"].IsNullOrEmpty())
                    ViewState["SelectedTenantIdEM"] = value;
            }
        }

        public Int32 SelectedApplicantId_Global
        {
            get
            {
                if (!ViewState["SelectedApplicantIdEM"].IsNullOrEmpty())
                    return (Int32)(ViewState["SelectedApplicantIdEM"]);
                else
                    return 0;
            }
            set
            {
                if (ViewState["SelectedApplicantIdEM"].IsNullOrEmpty())
                    ViewState["SelectedApplicantIdEM"] = value;
            }
        }

        public String CurrentLoggedInUserName_Global
        {
            get
            {
                if (!ViewState["CurrentLoggedInUserNameEM"].IsNullOrEmpty())
                    return (String)(ViewState["CurrentLoggedInUserNameEM"]);
                else
                    return String.Empty;
            }
            set
            {
                if (ViewState["CurrentLoggedInUserNameEM"].IsNullOrEmpty())
                    ViewState["CurrentLoggedInUserNameEM"] = value;
            }
        }

        /// <summary>
        /// Status of Item selected for Save
        /// </summary>
        public String AttemptedItemStatus
        {
            get { return rbtnListAction.SelectedValue; }
            set { value = rbtnListAction.SelectedValue; }
        }

        public Int32 TPReviewerUserId
        {
            get
            {
                if (!String.IsNullOrEmpty(hdfTPReviewerUserId.Value))
                    return Convert.ToInt32(hdfTPReviewerUserId.Value);
                return 0;
            }
            set
            {
                hdfTPReviewerUserId.Value = Convert.ToString(value);
            }
        }

        public Int32? ReviewerTenantId
        {
            get
            {
                if (!String.IsNullOrEmpty(hdfReviewerTenantId.Value))
                    return Convert.ToInt32(hdfReviewerTenantId.Value);
                return null;
            }
            set
            {
                hdfReviewerTenantId.Value = Convert.ToString(value);
            }
        }

        public Int16 ReviewerTypeId
        {
            get
            {
                if (!String.IsNullOrEmpty(hdfReviewerTypeId.Value))
                    return Convert.ToInt16(hdfReviewerTypeId.Value);
                return 0;
            }
            set
            {
                hdfReviewerTypeId.Value = Convert.ToString(value);
            }
        }

        public Int32 PackageId
        {
            get;
            set;
        }

        public Int32 SelectedCompliancePackageId_Global
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedCompliancePackageIdEM"]);
            }
            set
            {
                ViewState["SelectedCompliancePackageIdEM"] = value;
            }
        }

        public Int32 SelectedComplianceCategoryId_Global
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedComplianceCategoryIdEM"]);
            }
            set
            {
                ViewState["SelectedComplianceCategoryIdEM"] = value;
            }
        }

        /// <summary>
        /// Contains the Assignment Properties of the current item
        /// </summary>
        public List<ListItemAssignmentProperties> lstAssignmentProperties
        {
            get;
            set;
        }

        public Boolean IsFileUpoloadApplicable
        {
            get
            {
                if (!String.IsNullOrEmpty(hdfFileUploadExists.Value))
                    return Convert.ToBoolean(hdfFileUploadExists.Value);

                return false;
            }
            set
            {
                hdfFileUploadExists.Value = Convert.ToString(value);
            }
        }

        public Int32? FileUpoladAttributeId
        {
            get
            {
                if (!String.IsNullOrEmpty(hdfFileUploadAttributeId.Value))
                    return Convert.ToInt32(hdfFileUploadAttributeId.Value);
                return AppConsts.NONE;
            }
            set
            {
                hdfFileUploadAttributeId.Value = Convert.ToString(value);
            }
        }

        /// <summary>
        /// Used from Item Data Loader screen
        /// </summary>
        public Boolean IsIncompleteSelected
        {
            get
            {
                if ((rbtnListAction.SelectedValue == ApplicantItemComplianceStatus.Incomplete.GetStringValue()))
                    return true;
                return false;
            }
        }

        public Boolean IsItmEditableByApplcnt { get; set; } //UAT-3599

        private Dictionary<Object, Object> dropdownValues = new Dictionary<Object, Object>();
        /// <summary>
        /// Get the Mapped documents from the Loader control and pass on to the Document control
        /// </summary>
        public List<ApplicantComplianceDocumentMap> lstApplicantComplianceDocumentMaps { get; set; }

        /// <summary>
        /// Stores the Tenant Type code of the current logged in user 
        /// </summary>
        public String CurrentTenantTypeCode { get; set; }

        public Int32? UnifiedDocumentStartPageID
        {
            get;
            set;
        }

        public String ComplianceItemName { get { return litItemName.Text.HtmlDecode(); } }

        /// <summary>
        /// Status of Item selected for Save
        /// </summary>
        public String NewItemStatusText
        {
            get { return rbtnListAction.SelectedItem.Text; }
        }

        /// <summary>
        /// Status of Item when page loads
        /// </summary>
        public String CurrentItemStatusText
        {
            get { return litStatus.Text; }
        }

        public String EscalatedCode
        {
            get
            {
                return CurrentViewContext.VerificationData[0].EscalationCode;
            }
        }

        public Int32? IncompleteItemNewStatusId
        {
            get;
            set;
        }

        public String IncompleteItemNewStatusCode
        {
            get;
            set;
        }
        public String ExceptionComments
        {
            get { return txtVerificationComments.Text; }
        }

        public String AdminComments
        {
            get { return txtAdminNote.Text; }
        }

        public String LoggedInUserInitials_Global
        {
            get
            {
                if (!ViewState["LoggedInUserInitials"].IsNullOrEmpty())
                    return (String)(ViewState["LoggedInUserInitials"]);
                else
                    return String.Empty;
            }
            set
            {
                if (ViewState["LoggedInUserInitials"].IsNullOrEmpty())
                    ViewState["LoggedInUserInitials"] = value;
            }
        }

        public String StatusComments
        {
            get;
            set;
        }
        public Boolean IsAdminLoggedIn { get; set; }
        //2528
        public Boolean IsUiRulesViolate { get; set; }

        //UAT-2807
        String IReconciliationItemDataEditModeView.VerificationCommentsWithInitials
        {
            get;
            set;
        }

        #region UAT-3951:Rejection Reason
        public String NotApprovedStatusCode
        {
            get
            {
                return ApplicantItemComplianceStatus.Not_Approved.GetStringValue();
            }
        }

        public List<Entity.RejectionReason> ListRejectionReasons { get; set; }

        List<Int32> IReconciliationItemDataEditModeView.SelectedRejectionReasonIds
        {
            get
            {
                if (!hdnSelectedRejectionReasonIDs.Value.IsNullOrEmpty())
                {
                    return hdnSelectedRejectionReasonIDs.Value.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                }
                return null;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Presenter.OnViewLoaded();
            IsUiRulesViolate = false;
            LoadItemDetails();
            CurrentViewContext.ComplianceItemId = CurrentViewContext.VerificationData[0].ComplianceItemId;
            CurrentViewContext.ApplicantItemDataId = CurrentViewContext.VerificationData[0].ApplicantCompItemId.IsNotNull()
                ? Convert.ToInt32(CurrentViewContext.VerificationData[0].ApplicantCompItemId)
                : AppConsts.NONE;

            CurrentViewContext.ApplicantCategoryDataId = CurrentViewContext.VerificationData[0].ApplicantCompCatId.IsNotNull()
                ? Convert.ToInt32(CurrentViewContext.VerificationData[0].ApplicantCompCatId)
                : AppConsts.NONE;

            this.CurrentStatusId = !CurrentViewContext.VerificationData[0].ItemComplianceStatusId.IsNullOrEmpty()
                ? Convert.ToInt32(CurrentViewContext.VerificationData[0].ItemComplianceStatusId)
                : AppConsts.NONE;


            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            DisplayActions();
            SetDocumentControlProperties();
            if (!CurrentViewContext.VerificationData.IsNullOrEmpty())
            {
                ApplicantItemVerificationData _verificationData = CurrentViewContext.VerificationData
                    .Where(x =>
                        x.ComplianceAttributeId != null &&
                        x.AttributeTypeCode.ToLower().Trim() ==
                        ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower().Trim()).FirstOrDefault();

                ApplicantItemVerificationData _verificationDataViewDoc = CurrentViewContext.VerificationData
                                   .Where(x =>
                                       x.ComplianceAttributeId != null &&
                                       x.AttributeTypeCode.ToLower().Trim() ==
                                       ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower().Trim()).FirstOrDefault();
                if (_verificationData.IsNotNull()) // If File upload type attribute exists
                {
                    this.FileUpoladAttributeId = _verificationData.ComplianceAttributeId;
                }
            }
            hdfRejectionCodeItem.Value = ApplicantItemComplianceStatus.Not_Approved.GetStringValue();

            //UAT-3951:
            BindRejectionReasons();
            ShowHideRejectionReasonControl();

        }

        /// <summary>
        /// This function get executed if there is change in the drop down
        /// value. The changed value is added in a text box, which is added 
        /// with visibility false.
        /// The purpose of this function to set the selected value of the dropdown 
        /// corresponding to value selected previously. This is to maintain the value of dropdown after multiple post back.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void textchanged(object sender, EventArgs e)
        {
            if (!TextChangedExecuted)
            {
                dropdownValues.ForEach(item =>
                {
                    (item.Key as WclComboBox).SelectedValue = (item.Value as TextBox).Text;
                });
                //This is to prevent the multiple execution of this allocation. 
                //As change in multiple dropdown will lead to execution of this function multiple time.
                TextChangedExecuted = true;
            }
        }

        private void SetDocumentControlProperties()
        {
            ucVerificationDocumentControlReadOnlyMode.ItemDataId = CurrentViewContext.ApplicantItemDataId;
            ucVerificationDocumentControlReadOnlyMode.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
            ucVerificationDocumentControlReadOnlyMode.PackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionId;
            ucVerificationDocumentControlReadOnlyMode.lstApplicantComplianceDocumentMaps = this.lstApplicantComplianceDocumentMaps;
        }

        protected void rpAttributes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CurrentViewContext.ComplianceItemId = CurrentViewContext.VerificationData[0].ComplianceItemId;

            ApplicantItemVerificationData attributeData = (ApplicantItemVerificationData)e.Item.DataItem;

            String dataTypeCode = attributeData.AttributeTypeCode.ToLower().Trim();

            // Need to check is AttributeId is NULL from SP

            Boolean _isAttributeEditable = attributeData.AttTypeCode.ToLower() == ComplianceAttributeType.Manual.GetStringValue().ToLower();// -- STEP 3. Hide the next line of code to  run this
            HtmlGenericControl divMainDiv = (HtmlGenericControl)e.Item.FindControl("divMainDiv");

            WclTextBox txtDataType = (WclTextBox)e.Item.FindControl("txtDataType");
            Literal litLabel = (Literal)e.Item.FindControl("litLabel");
            txtDataType.Text = dataTypeCode;
            Boolean isAttributeValueDifferent = false;

            if (dataTypeCode == ComplianceAttributeDatatypes.Date.GetStringValue().ToLower().Trim())
            {
                WclDatePicker dtPicker = (WclDatePicker)e.Item.FindControl("dtPicker");
                dtPicker.Visible = true;
                if (_isAttributeEditable && !this.IsReadOnlyAfterSave)
                {
                    List<ReconciliationDetailsDataContract> lstReconciliationData = CurrentViewContext.lstReconciliationDetailsData
                                           .Where(att => att.AttributeDataTypeCode.IsNotNull()
                                            && att.AttributeDataTypeCode.ToLower() == dataTypeCode
                                            && att.ComplianceAttributeID == Convert.ToInt32(attributeData.ComplianceAttributeId))
                                            .OrderBy(cond => cond.ApplicantComplianceReconciliationDataID).ToList();
                    String currentValue = attributeData.AttributeValue;
                    if (!currentValue.IsNullOrEmpty())
                    {
                        currentValue = Convert.ToDateTime(currentValue).ToString("MM/dd/yyyy");
                    }
                    #region Check if all values are same
                    if (!lstReconciliationData.IsNullOrEmpty())
                    {
                        List<String> lstValues = lstReconciliationData.Select(cond => cond.AttributeValue).ToList();
                        String updatedValue = lstValues.FirstOrDefault();
                        if (!updatedValue.IsNullOrEmpty())
                        {
                            updatedValue = Convert.ToDateTime(updatedValue).ToString("MM/dd/yyyy");
                        }
                        isAttributeValueDifferent = (lstValues.Distinct().Count() > 1) || (!updatedValue.Equals(currentValue));
                    }
                    #endregion
                    litLabel.Text = "(Reconciled) " + (attributeData.AttributeLabel.IsNullOrEmpty() ? attributeData.AttributeName : attributeData.AttributeLabel).HtmlEncode();
                    GenerateControlForAttribute(divMainDiv
                        , "(Submitted) " + (attributeData.AttributeLabel.IsNullOrEmpty() ? attributeData.AttributeName : attributeData.AttributeLabel)
                            , currentValue, isAttributeValueDifferent);


                    foreach (ReconciliationDetailsDataContract attribute in lstReconciliationData)
                    {
                        GenerateControlForAttribute(divMainDiv, attribute.AttributeName,
                            attribute.AttributeValue.IsNullOrEmpty() ? String.Empty : Convert.ToDateTime(attribute.AttributeValue).ToString("MM/dd/yyyy")
                            , isAttributeValueDifferent);
                    }
                }
                else if (!this.IsReadOnlyAfterSave)
                {
                    divMainDiv.Visible = false;
                }

                if (!String.IsNullOrEmpty(attributeData.AttributeValue) && !DateTime.MinValue.ToShortDateString().Equals(attributeData.AttributeValue) && !isAttributeValueDifferent)
                    dtPicker.SelectedDate = Convert.ToDateTime(attributeData.AttributeValue);
                else
                    dtPicker.Clear();

                // Manage the Editably property, after rebind, on Save
                if (this.IsRebindAfterSave)
                    dtPicker.Enabled = (_isAttributeEditable && !this.IsReadOnlyAfterSave) ? true : false;
                else
                    dtPicker.Enabled = _isAttributeEditable ? true : false;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Text.GetStringValue().ToLower().Trim())
            {
                WclTextBox txtBox = (WclTextBox)e.Item.FindControl("txtBox");
                txtBox.MaxLength = attributeData.MaximumCharacters == null ? 50 : Convert.ToInt32(attributeData.MaximumCharacters);
                txtBox.Visible = true;
                if (_isAttributeEditable && !this.IsReadOnlyAfterSave)
                {
                    litLabel.Text = "Reconciled " + (attributeData.AttributeLabel.IsNullOrEmpty() ? attributeData.AttributeName : attributeData.AttributeLabel).HtmlEncode();
                    List<ReconciliationDetailsDataContract> lstReconciliationData = CurrentViewContext.lstReconciliationDetailsData
                                          .Where(att => att.AttributeDataTypeCode.IsNotNull()
                                           && att.AttributeDataTypeCode.ToLower() == dataTypeCode
                                           && att.ComplianceAttributeID == Convert.ToInt32(attributeData.ComplianceAttributeId))
                                           .OrderBy(cond => cond.ApplicantComplianceReconciliationDataID).ToList();
                    #region Check if all values are same
                    if (!lstReconciliationData.IsNullOrEmpty())
                    {
                        List<String> lstValues = lstReconciliationData.Select(cond => cond.AttributeValue).ToList();
                        isAttributeValueDifferent = (lstValues.Distinct().Count() > 1) || (lstValues.FirstOrDefault() != attributeData.AttributeValue);
                    }
                    #endregion
                    GenerateControlForAttribute(divMainDiv
                        , "(Submitted) " + (attributeData.AttributeLabel.IsNullOrEmpty() ? attributeData.AttributeName : attributeData.AttributeLabel)
                        , attributeData.AttributeValue, isAttributeValueDifferent);
                    foreach (ReconciliationDetailsDataContract attribute in lstReconciliationData)
                    {
                        GenerateControlForAttribute(divMainDiv, attribute.AttributeName, attribute.AttributeValue, isAttributeValueDifferent);
                    }
                }
                else if (!this.IsReadOnlyAfterSave)
                {
                    divMainDiv.Visible = false;
                }
                if (!isAttributeValueDifferent)
                {
                    txtBox.Text = attributeData.AttributeValue;
                }
                else
                {
                    txtBox.Text = String.Empty;
                }

                // Manage the Editably property, after rebind, on Save
                if (this.IsRebindAfterSave)
                    txtBox.Enabled = (_isAttributeEditable && !this.IsReadOnlyAfterSave) ? true : false;
                else
                    txtBox.Enabled = _isAttributeEditable ? true : false;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Numeric.GetStringValue().ToLower().Trim())
            {
                WclNumericTextBox numericTextBox = (WclNumericTextBox)e.Item.FindControl("numericTextBox");
                numericTextBox.Visible = true;
                if (_isAttributeEditable && !this.IsReadOnlyAfterSave)
                {
                    litLabel.Text = "Reconciled " + (attributeData.AttributeLabel.IsNullOrEmpty() ? attributeData.AttributeName : attributeData.AttributeLabel).HtmlEncode();
                    List<ReconciliationDetailsDataContract> lstReconciliationData = CurrentViewContext.lstReconciliationDetailsData
                                         .Where(att => att.AttributeDataTypeCode.IsNotNull()
                                          && att.AttributeDataTypeCode.ToLower() == dataTypeCode
                                          && att.ComplianceAttributeID == Convert.ToInt32(attributeData.ComplianceAttributeId))
                                          .OrderBy(cond => cond.ApplicantComplianceReconciliationDataID).ToList();
                    #region Check if all values are same
                    if (!lstReconciliationData.IsNullOrEmpty())
                    {
                        List<String> lstValues = lstReconciliationData.Select(cond => cond.AttributeValue).ToList();
                        isAttributeValueDifferent = (lstValues.Distinct().Count() > 1) || (lstValues.FirstOrDefault() != attributeData.AttributeValue);
                    }
                    #endregion
                    GenerateControlForAttribute(divMainDiv
                        , "(Submitted) " + (attributeData.AttributeLabel.IsNullOrEmpty() ? attributeData.AttributeName : attributeData.AttributeLabel)
                        , attributeData.AttributeValue, isAttributeValueDifferent
                        );
                    foreach (ReconciliationDetailsDataContract attribute in lstReconciliationData)
                    {
                        GenerateControlForAttribute(divMainDiv, attribute.AttributeName, attribute.AttributeValue, isAttributeValueDifferent);
                    }
                }
                else if (!this.IsReadOnlyAfterSave)
                {
                    divMainDiv.Visible = false;
                }
                if (!isAttributeValueDifferent)
                {
                    numericTextBox.Text = attributeData.AttributeValue;
                }
                else
                {
                    numericTextBox.Text = String.Empty;
                }

                if (this.IsRebindAfterSave)
                    numericTextBox.Enabled = (_isAttributeEditable && !this.IsReadOnlyAfterSave) ? true : false;
                else
                    numericTextBox.Enabled = _isAttributeEditable ? true : false;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Options.GetStringValue().ToLower().Trim())
            {
                String _optionsCode = ComplianceAttributeDatatypes.Options.GetStringValue();

                var _cmbData = CurrentViewContext.VerificationData
                       .Where(attrOption => attrOption.ComplianceAttributeId == attributeData.ComplianceAttributeId
                        && attrOption.AttributeTypeCode == _optionsCode)
                       .ToList().Select(optn => new { optn.OptionText, optn.OptionValue });


                WclComboBox optionCombo = (WclComboBox)e.Item.FindControl("optionCombo");

                TextBox userValuefield = (TextBox)e.Item.FindControl("hdnoptionComboValue");
                foreach (var attributeOption in _cmbData)
                    optionCombo.Items.Add(new RadComboBoxItem(attributeOption.OptionText, attributeOption.OptionValue));

                optionCombo.Items.Insert(0, new RadComboBoxItem(AppConsts.COMBOBOX_ITEM_SELECT, AppConsts.ZERO));
                if (_isAttributeEditable && !this.IsReadOnlyAfterSave)
                {
                    litLabel.Text = "Reconciled " + (attributeData.AttributeLabel.IsNullOrEmpty() ? attributeData.AttributeName : attributeData.AttributeLabel).HtmlEncode();
                    List<ReconciliationDetailsDataContract> lstReconciliationData = CurrentViewContext.lstReconciliationDetailsData
                                         .Where(att => att.AttributeDataTypeCode.IsNotNull()
                                          && att.AttributeDataTypeCode.ToLower() == dataTypeCode
                                          && att.ComplianceAttributeID == Convert.ToInt32(attributeData.ComplianceAttributeId))
                                          .OrderBy(cond => cond.ApplicantComplianceReconciliationDataID).ToList();
                    #region Check if all values are same
                    if (!lstReconciliationData.IsNullOrEmpty())
                    {
                        List<String> lstValues = lstReconciliationData.Select(cond => cond.AttributeValue).ToList();
                        isAttributeValueDifferent = (lstValues.Distinct().Count() > 1) || (lstValues.FirstOrDefault() != attributeData.AttributeValue);
                    }
                    #endregion
                    String optionTextSubmitted = String.Empty;
                    if (_cmbData.Where(cond => cond.OptionValue == attributeData.AttributeValue).Any())
                    {
                        optionTextSubmitted = _cmbData.Where(cond => cond.OptionValue == attributeData.AttributeValue).FirstOrDefault().OptionText;
                    }
                    GenerateControlForAttribute(divMainDiv
                        , "(Submitted) " + (attributeData.AttributeLabel.IsNullOrEmpty() ? attributeData.AttributeName : attributeData.AttributeLabel)
                        , optionTextSubmitted, isAttributeValueDifferent);
                    foreach (ReconciliationDetailsDataContract attribute in lstReconciliationData)
                    {
                        String optionText = String.Empty;
                        if (_cmbData.Where(cond => cond.OptionValue == attribute.AttributeValue).Any())
                        {
                            optionText = _cmbData.Where(cond => cond.OptionValue == attribute.AttributeValue).FirstOrDefault().OptionText;
                        }
                        GenerateControlForAttribute(divMainDiv, attribute.AttributeName, optionText, isAttributeValueDifferent);
                    }
                }
                else if (!this.IsReadOnlyAfterSave)
                {
                    divMainDiv.Visible = false;
                }
                //This check for the very first time the page is loaded and if save all changes is executed.
                if (ReloadingDataAfterSave || (!Page.IsPostBack))
                {
                    if (!isAttributeValueDifferent)
                    {
                        //To set the initial value of selected dropdown value in text box if there is some value.
                        userValuefield.Text = attributeData.AttributeValue;
                    }
                    else
                    {
                        userValuefield.Text = String.Empty;
                    }
                }
                if (Page.IsPostBack)
                {
                    //We dont get value of the text box here so we add the object here to get the value later.
                    dropdownValues.Add(optionCombo, userValuefield);
                }
                optionCombo.DataBind();
                optionCombo.Visible = true;
                if (!isAttributeValueDifferent)
                {
                    optionCombo.SelectedValue = attributeData.AttributeValue;
                }
                else
                {
                    optionCombo.SelectedValue = AppConsts.ZERO;
                }

                if (this.IsRebindAfterSave)
                    optionCombo.Enabled = (_isAttributeEditable && !this.IsReadOnlyAfterSave) ? true : false;
                else
                    optionCombo.Enabled = _isAttributeEditable ? true : false;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower().Trim())
            {
                Label lblViewDoc = (Label)e.Item.FindControl("lblViewDoc");
                if (!lblViewDoc.IsNull() && attributeData.ApplAttributeDataId.HasValue && attributeData.ApplAttributeDataId > AppConsts.NONE)
                {
                    lblViewDoc.Visible = true;
                    if (attributeData.AttributeValue == AppConsts.ZERO)
                    {
                        lblViewDoc.Text = AppConsts.NO;
                    }
                    else if (attributeData.AttributeValue == AppConsts.ONE.ToString())
                    {
                        lblViewDoc.Text = AppConsts.YES;
                    }
                    else
                    {
                        lblViewDoc.Text = "N/A";
                    }
                }
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Screening_Document.GetStringValue().ToLower().Trim())
            {
                var _screeningDocTypeCode = DocumentType.SCREENING_DOCUMENT_ATTRIBUTE_TYPE_DOCUMENT.GetStringValue();
                HtmlGenericControl _divScreeningDocuments = (HtmlGenericControl)e.Item.FindControl("divScreeningDocuments");
                var _lstScreeningDocuments = CurrentViewContext.VerificationData.Where(vd => vd.ComplianceAttributeId == attributeData.ComplianceAttributeId
                                             && vd.ScreeningDocTypeCode == _screeningDocTypeCode).ToList();

                StringBuilder _sbScreeningDocumentLinks = new StringBuilder();
                foreach (var document in _lstScreeningDocuments)
                {
                    LinkButton lnkBtn = new LinkButton();
                    lnkBtn.ID = "lnkBtn_" + attributeData.ComplianceAttributeId;
                    lnkBtn.OnClientClick = "ViewScreeningDocument(" + CurrentViewContext.SelectedTenantId_Global + ", " + document.ScreeningDocumentId + ");";
                    lnkBtn.Attributes.Add("onclick", "return false;");

                    lnkBtn.Text = document.ScreeningDocumentName;
                    _divScreeningDocuments.Controls.Add(lnkBtn);
                    _divScreeningDocuments.Controls.Add(new LiteralControl("<br />"));
                }
            }
        }

        #endregion

        #region Dynamic Control Generation

        private void GenerateControlForAttribute(HtmlGenericControl divMainDiv, String name, String value, Boolean isDifferentValue)
        {
            HtmlGenericControl controlInColumn = null;
            controlInColumn = GenerateColumnView();
            controlInColumn = GenerateControl(name, value, controlInColumn, isDifferentValue);
            AddNextLineDiv(controlInColumn);
            divMainDiv.Controls.Add(controlInColumn);
        }
        /// <summary>
        /// Generate a new row
        /// </summary>
        /// <param name="columnNumber">Number of column per row</param>
        /// <returns></returns>
        private HtmlGenericControl GenerateColumnView()
        {
            HtmlGenericControl column = new HtmlGenericControl("div");
            column.Attributes.Add("class", "sxro sx1co");
            return column;

        }

        /// <summary>
        /// Add relevant space between tweo row.
        /// </summary>
        /// <param name="parentControl"></param>
        /// <returns>parentControl</returns>
        private HtmlGenericControl AddNextLineDiv(HtmlGenericControl parentControl)
        {
            String className = "sxroend";
            HtmlGenericControl nextLineDiv = new HtmlGenericControl("div");
            nextLineDiv.Attributes.Add("class", className);
            parentControl.Controls.Add(nextLineDiv);
            return parentControl;
        }

        /// <summary>
        /// Main function that creates a control nas per their data type.
        /// </summary>
        /// <param name="parameter">Attribute data to be created.</param>
        /// <param name="parentControl"></param>
        /// <returns></returns>
        private HtmlGenericControl GenerateControl(String name, String value, HtmlGenericControl parentControl, Boolean isDifferentValue)
        {
            HtmlGenericControl lableDiv = CreateLabelForTheControl(name);
            parentControl.Controls.Add(lableDiv);
            HtmlGenericControl controlDiv = CreateControlForTheForm(value, isDifferentValue);
            parentControl.Controls.Add(controlDiv);
            return parentControl;
        }

        /// <summary>
        /// This method create lable for the control.
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="currentInstanceId"></param>
        /// <returns></returns>
        private HtmlGenericControl CreateLabelForTheControl(String name)
        {
            HtmlGenericControl lableDiv = new HtmlGenericControl("div");
            lableDiv.Attributes.Add("class", "sxlb");
            Label attributeLable = new Label();
            attributeLable.Attributes.Add("class", "cptn");
            attributeLable.ID = "lbl" + Guid.NewGuid().ToString();
            attributeLable.Text = name;
            lableDiv.Controls.Add(attributeLable);
            return lableDiv;
        }

        /// <summary>
        /// this method creates control corresponding to different data type.
        /// </summary>
        /// <param name="attributesForCustomForm"></param>
        /// <param name="currentInstanceId"></param>
        /// <returns></returns>
        private HtmlGenericControl CreateControlForTheForm(String value, Boolean isDifferentValue)
        {
            HtmlGenericControl controlDiv = new HtmlGenericControl("div");
            #region Commented Code
            /*
            switch (attributesForCustomForm.AttributeDataTypeCode)
            {
                case "ADTTEX":
                    WclTextBox textBox = new WclTextBox();
                    textBox.ID = "txt_" + attributesForCustomForm.AttributeDataTypeCode + "_" + attributesForCustomForm.ComplianceAttributeID + attributesForCustomForm.ReviewerID.ToString();
                    textBox.Style.Add("display", "block");
                    textBox.Enabled = false;
                    textBox.Text = attributesForCustomForm.AttributeValue;
                    controlDiv.Controls.Add(textBox);
                    break;
                case "ADTNUM":
                    WclNumericTextBox txtNumeric = new WclNumericTextBox();
                    txtNumeric.ID = "txtNumericType_" + attributesForCustomForm.AttributeDataTypeCode + "_" + attributesForCustomForm.ComplianceAttributeID + attributesForCustomForm.ReviewerID.ToString();
                    txtNumeric.Style.Add("display", "block");
                    txtNumeric.Enabled = false;
                    txtNumeric.Text = attributesForCustomForm.AttributeValue;
                    controlDiv.Controls.Add(txtNumeric);
                    break;
                case "ADTDAT":
                    WclDatePicker dPicker = new WclDatePicker();
                    dPicker.DateInput.DateFormat = "MM-dd-yyyy";
                    dPicker.ID = "dp_" + attributesForCustomForm.AttributeDataTypeCode + "_" + attributesForCustomForm.ComplianceAttributeID + attributesForCustomForm.ReviewerID.ToString();
                    dPicker.DateInput.EmptyMessage = "Select a date";
                    dPicker.Style.Add("display", "block");
                    dPicker.SelectedDate = Convert.ToDateTime(attributesForCustomForm.AttributeValue);
                    dPicker.Enabled = false;
                    controlDiv.Controls.Add(dPicker);
                    break;
                case "ADTOPT":
                    var _cmbData = CurrentViewContext.VerificationData
                       .Where(attrOption => attrOption.ComplianceAttributeId == attributesForCustomForm.ComplianceAttributeID
                        && attrOption.AttributeTypeCode == "ADTOPT")
                       .ToList().Select(optn => new { optn.OptionText, optn.OptionValue });
                    WclComboBox optionCombo = new WclComboBox();
                    optionCombo.ID = "dropDown_" + attributesForCustomForm.AttributeDataTypeCode + "_" + attributesForCustomForm.ComplianceAttributeID + attributesForCustomForm.ReviewerID.ToString();
                    foreach (var attributeOption in _cmbData)
                        optionCombo.Items.Add(new RadComboBoxItem(attributeOption.OptionText, attributeOption.OptionValue));
                    optionCombo.Items.Insert(0, new RadComboBoxItem(AppConsts.COMBOBOX_ITEM_SELECT, AppConsts.ZERO));
                    optionCombo.SelectedValue = attributesForCustomForm.AttributeValue;
                    optionCombo.Enabled = false;
                    controlDiv.Controls.Add(optionCombo);
                    break;
                default:

                    break;
            }
             * */
            #endregion
            if (isDifferentValue)
            {
                controlDiv.Attributes.Add("class", "sxlm literalClassYellow");
            }
            else
            {
                controlDiv.Attributes.Add("class", "sxlm literalClassGreen");
            }
            Literal literal = new Literal();
            literal.ID = Guid.NewGuid().ToString();
            literal.Text = value;
            controlDiv.Controls.Add(literal);
            return controlDiv;
        }

        #endregion


        #region Methods

        #region Private Methods

        /// <summary>
        /// Set the details of the Item being loaded
        /// </summary>
        private void LoadItemDetails()
        {
            // UC-2 In case No item data is entred by the applicant, then will it load the details properly
            if (VerificationData.IsNotNull())
            {
                litItemName.Text = CurrentViewContext.VerificationData[0].ItemName.HtmlEncode();
                IsAdminLoggedIn = (Presenter.IsDefaultTenant);//UAT-2807
                //IsAdminLoggedIn = (SecurityManager.DefaultTenantID == CurrentTenantId_Global);
                //if (IsAdminLoggedIn)
                //{
                //    if (CurrentViewContext.VerificationData[0].IsUiRulesViolate)
                //    {
                //        dvDetailPanel.Attributes.Add("Style", "background-color:#ff6666 !important");
                //    }
                //    else
                //    {
                //        dvDetailPanel.Attributes.Remove("Style");
                //    }
                //}

                #region UAT 725: Add submission time and date to verification details screen for each item

                lblSubmissionDate.Text = Convert.ToString(CurrentViewContext.VerificationData[0].SubmissionDate);

                if (lblSubmissionDate.Text != "")  //UAT-3371
                {
                    lblSubmissionDate.Text += "(MT)";
                }

                #endregion

                #region UAT:719 Check Exceptions turned off for a Category/Item
                Boolean isExceptionAllowed = Presenter.IsAllowExceptionOnCategory();
                if (isExceptionAllowed)
                {
                    imageExceptionOff.Visible = true;
                }
                #endregion
                //UAT-3599
                Boolean isSDEdisabled = Presenter.IsStudentDataEntryEnable();
                if (isSDEdisabled)
                {
                    imageSDEdisabled.Visible = true;
                }
                #region UAT:3614 Icon on Three Panel Screen if Item is set to approval required: No
                ListItemAssignmentProperties assignmentProperty = this.lstAssignmentProperties.FirstOrDefault();
                if (IsAdminLoggedIn)
                {
                    imageAutoApprove.Visible = assignmentProperty.ApprovalRequired.HasValue ? !(assignmentProperty.ApprovalRequired.Value) : false;
                }
                #endregion
                lnkItemName.Attributes.Add("UnifiedDocumentStartPageID", (UnifiedDocumentStartPageID != null ? Convert.ToString(UnifiedDocumentStartPageID) : "0"));

                if (String.IsNullOrEmpty((VerificationData[0].ItemComplianceStatusCode)))
                {
                    litStatus.Text = VerificationDataActions.INCOMPLETE.GetStringValue();
                }
                else if (VerificationData[0].ItemComplianceStatusCode.ToLower() == ApplicantItemComplianceStatus.Pending_Review.GetStringValue().ToLower()
                    || VerificationData[0].ItemComplianceStatusCode.ToLower() == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue().ToLower()
                    || VerificationData[0].ItemComplianceStatusCode.ToLower() == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue().ToLower())
                    litStatus.Text = CurrentViewContext.VerificationData[0].ItemComplianceStatusDescription;
                else
                    litStatus.Text = CurrentViewContext.VerificationData[0].ItemComplianceStatus;

                List<ReconciliationDetailsDataContract> lstReconciliationDetailsDataContract = lstReconciliationDetailsData
                 .GroupBy(attr => attr.ApplicantComplianceReconciliationDataID) // Group by to avoid Repition of attributes in case of the OPTIONS type attribute
                 .Select(attr => attr.First())
                 .OrderBy(o => o.ApplicantComplianceReconciliationDataID)
                 .ToList();

                if (!this.IsReadOnlyAfterSave)
                {
                    #region Check if all values are same
                    Boolean isAttributeValueDifferent = false;
                    if (!lstReconciliationDetailsDataContract.IsNullOrEmpty())
                    {
                        List<String> lstValues = lstReconciliationDetailsDataContract.Select(cond => cond.ItemStatusCode).ToList();
                        //UAT-2026
                        //isAttributeValueDifferent = (lstValues.Distinct().Count() > 1) || (lstValues.FirstOrDefault().ToLower() != VerificationData[0].ItemComplianceStatusCode.ToLower());
                        isAttributeValueDifferent = (lstValues.Distinct().Count() > 1);
                    }
                    #endregion

                    foreach (ReconciliationDetailsDataContract item in lstReconciliationDetailsDataContract)
                    {
                        String statusName = VerificationDataActions.INCOMPLETE.GetStringValue();
                        if (String.IsNullOrEmpty((item.ItemStatusCode.ToLower())))
                        {
                            statusName = VerificationDataActions.INCOMPLETE.GetStringValue();
                        }
                        else if (item.ItemStatusCode.ToLower() == ApplicantItemComplianceStatus.Pending_Review.GetStringValue().ToLower()
                            || item.ItemStatusCode.ToLower() == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue().ToLower()
                            || item.ItemStatusCode.ToLower() == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue().ToLower())
                            statusName = item.ItemComplianceStatusDescription;
                        else
                            statusName = item.ItemComplianceStatus;
                        GenerateControlForAttribute(divStatus, "(" + item.ReviewerName + ") Status", statusName, isAttributeValueDifferent);
                    }
                }
                else
                {
                    divStatus.Visible = false;
                }

                litApplicantNotes.Text = String.IsNullOrEmpty(CurrentViewContext.VerificationData[0].ApplicantItemNotes) ?
                    " <span class='no-data'>No Data</span>"
                    : CurrentViewContext.VerificationData[0].ApplicantItemNotes;

                //hdnUrl.Value = sampleDocFormURL;
                CurrentViewContext.ItemComplianceStatusCode = CurrentViewContext.VerificationData[0].ItemComplianceStatusCode;
                CurrentViewContext.PackageId = CurrentViewContext.SelectedCompliancePackageId_Global;

                if (IsAdminLoggedIn) //UAT-2807
                {
                    txtVerificationComments.Text = CurrentViewContext.VerificationData[0].VerificationComments;

                }
                else
                {
                    txtVerificationComments.Text = CurrentViewContext.VerificationData[0].VerificationCommentsWithInitials;
                }

                rbtnListAction.Attributes.Add("actionItemId", Convert.ToString(CurrentViewContext.VerificationData[0].ComplianceItemId));
                txtAdminNote.Attributes.Add("noteItemId", Convert.ToString(CurrentViewContext.VerificationData[0].ComplianceItemId));

                txtAdminNote.Attributes.Add("currentStatus", Convert.ToString(CurrentViewContext.VerificationData[0].ItemComplianceStatusCode));

                // Data entry for Incomplete items as well
                List<ApplicantItemVerificationData> lstAttributes = VerificationData
                  .GroupBy(attr => attr.ComplianceAttributeId) // Group by to avoid Repition of attributes in case of the OPTIONS type attribute
                  .Select(attr => attr.First())
                  .Where(att => !att.AttributeTypeCode.IsNullOrEmpty()
                      && att.AttributeTypeCode.ToLower() != ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower())
                  .ToList<ApplicantItemVerificationData>();

                // string sampleDocFormURL = VerificationData[0].SampleDocFormURL;
                //Only display hyperlink if sampleDocFromUrl available in VerificationData
                string[] sampleDocFormURLs = VerificationData[0].SampleDocFormURL.Split(',');
                string[] sampleDocFormDisplayURLLabels = VerificationData[0].SampleDocFormDisplayURLLabel.Split(',');
                StringBuilder sb = new StringBuilder();
                string SampleDocLink = string.Empty;
                for (int i = 0; i < sampleDocFormURLs.Count(); i++)
                {
                    if (!sampleDocFormURLs[i].IsNullOrEmpty())
                    {
                        sb.Append("<br /><a href=\"" + sampleDocFormURLs[i] + "\" onclick=\"\" target=\"_blank\");'>" +
                            (sampleDocFormDisplayURLLabels[i].IsNullOrEmpty()? "View Sample Document" : "View " + sampleDocFormDisplayURLLabels[i]) +"</a>");
                    }
                }
                //foreach (var sampleDocFormURL in sampleDocFormURLs)
                //{
                //    //Only display hyperlink if sampleDocFromUrl available
                //    if (!sampleDocFormURL.IsNullOrEmpty())
                //    {
                //        sb.Append("<br /><a href=\"" + sampleDocFormURL + "\" onclick=\"\" target=\"_blank\");'>View Sample Doc</a>");
                //    }

                //}
                SampleDocLink = sb.ToString();
                ucExplanationDescription.ApplicantExplanation = CurrentViewContext.VerificationData[0].ItemExpNotes;
                ucExplanationDescription.AdminExplanation = CurrentViewContext.VerificationData[0].ItemDescription + @SampleDocLink;

                rpAttributes.DataSource = lstAttributes;
                rpAttributes.DataBind();

                hdfItemStatus.Value = VerificationData[0].ItemComplianceStatusCode;
                hdfComplianceItemId.Value = Convert.ToString(VerificationData[0].ComplianceItemId);

                String _divType = ComplianceVerificationDetailsContract.GetItemDivType(CurrentViewContext.ItemComplianceStatusCode);
                divEditMode.Attributes.Add("divType", _divType);

                //#region Get Reconciled Doc Related Data

                //List<Int32> lstReconciledDocs = new List<int>();

                //List<String> lstReconciledData = lstReconciliationDetailsData.GroupBy(attr => attr.ReviewerID)
                //                                                                                         .Select(attr => attr.First().ApplicantDocumentIDs)
                //                                                                                         .ToList();
                //foreach (String csvDocIds in lstReconciledData)
                //{
                //    if (!csvDocIds.IsNullOrEmpty())
                //    {
                //        lstReconciledDocs.AddRange(csvDocIds.Split(',').Select(Int32.Parse).ToList());
                //    }
                //}
                //lstReconciledDocs = lstReconciledDocs.Distinct().ToList();
                //#endregion

                #region UAT-722
                String unifiedDocPageMapping = String.Empty;
                //UAT-1538
                String appDocumentIds = String.Empty;
                var currentItemUnifiedDocList = lstApplicantDocument.Where(cond => cond.ComplianceItemID == CurrentViewContext.VerificationData[0].ComplianceItemId
                    //|| lstReconciledDocs.Contains(cond.ApplicantDocumentId)
                    ).ToList();
                if (currentItemUnifiedDocList.IsNotNull() && currentItemUnifiedDocList.Count() > 0)
                {
                    Int16 counter = 0;
                    foreach (var record in currentItemUnifiedDocList)
                    {
                        if (counter == AppConsts.NONE)
                        {
                            unifiedDocPageMapping = record.UnifiedDocumentStartPageID + "-" + record.UnifiedDocumentEndPageID;
                        }
                        else
                        {
                            unifiedDocPageMapping = unifiedDocPageMapping + "," + record.UnifiedDocumentStartPageID + "-" + record.UnifiedDocumentEndPageID;
                        }
                        appDocumentIds = appDocumentIds + "," + record.ApplicantDocumentId;
                        counter++;
                    }
                }
                lnkItemName.Attributes.Add("UnifiedDocPageMapping", Convert.ToString(unifiedDocPageMapping));

                //UAT-1538:
                lnkItemName.Attributes.Add("appDocumentIds", Convert.ToString(appDocumentIds));
                #endregion
            }
        }

        /// <summary>
        /// Display the Radio buttons for possible Admin actions
        /// </summary>
        private void DisplayActions()
        {
            ListItemAssignmentProperties assignmentProperty = this.lstAssignmentProperties.FirstOrDefault();

            // Clear the items so that re-load does not adds duplicate items
            rbtnListAction.Items.Clear();

            if (!assignmentProperty.IsNullOrEmpty())
            {
                CurrentViewContext.ReviewerTenantId = Convert.ToInt32(assignmentProperty.ReviewerTenantId);
                CurrentViewContext.TPReviewerUserId = Convert.ToInt32(assignmentProperty.ThirdPartyReviewerUserId);

                if (assignmentProperty.ApprovalRequired == true)
                {
                    var clientAdmin = this.lstAssignmentProperties
                                         .FirstOrDefault(x => x.ReviewerTypeCode.IsNotNull() && x.ReviewerTypeCode == LkpReviewerType.ClientAdmin);

                    if (clientAdmin.IsNull())
                        CurrentViewContext.ReviewerTypeId = 0;
                    else
                        CurrentViewContext.ReviewerTypeId = Convert.ToInt16(clientAdmin.ReviewerTypeId);
                }
                if (_presenter.IsDefaultTenant)
                {
                    SetStatus();
                    if (!this.lstAssignmentProperties.IsNullOrEmpty() && this.lstAssignmentProperties
                                                                      .Any(revType => revType.ReviewerTypeCode.IsNotNull() && revType.ReviewerTypeCode
                                                                      .Equals(ReviewerType.Admin.GetStringValue())))
                        rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.PENDING_REVIEW.GetStringValue(), Value = ApplicantItemComplianceStatus.Pending_Review.GetStringValue() });

                    if (!this.lstAssignmentProperties.IsNullOrEmpty() && this.lstAssignmentProperties
                                                                      .Any(revType => revType.ReviewerTypeCode.IsNotNull() && revType.ReviewerTypeCode
                                                                      .Equals(ReviewerType.Client_Admin.GetStringValue())))
                    {
                        //rbtnListAction.Items.Clear();
                        rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.SEND_FOR_CLIENT_REVIEW.GetStringValue(), Value = ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue() });

                    }
                    else if (!this.lstAssignmentProperties.IsNullOrEmpty()
                    && this.lstAssignmentProperties.Any(revType => revType.ReviewerTypeCode.IsNotNull() && revType.ReviewerTypeCode.Equals(ReviewerType.Admin.GetStringValue()))
                    && !this.lstAssignmentProperties.Any(revType => revType.ReviewerTypeCode.IsNotNull() && revType.ReviewerTypeCode.Equals(ReviewerType.Client_Admin.GetStringValue()))
                    && assignmentProperty.ThirdPartyReviewerUserId != null
                    && VerificationData[0].ReviewerTenantId.IsNull()
                    && VerificationData[0].AssignedToUserId.IsNull())
                    {
                        //rbtnListAction.Items.Clear();
                        rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.FURTHER_REVIEW_THIRD_PARTY.GetStringValue(), Value = ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue() });
                    }
                    else if (VerificationData[0].ReviewerTenantId.IsNotNull() || (VerificationData[0].ReviewerTypeCode.IsNotNull()
                        && VerificationData[0].ReviewerTypeCode.ToLower() == ReviewerType.Client_Admin.GetStringValue().ToLower()))
                    {
                        //rbtnListAction.Items.Clear();
                        rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.SEND_FOR_CLIENT_REVIEW.GetStringValue(), Value = ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue() });
                        //rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.PENDING_REVIEW.GetStringValue(), Value = ApplicantItemComplianceStatus.Pending_Review.GetStringValue(), Enabled = false });
                    }
                    rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.APPROVED.GetStringValue(), Value = ApplicantItemComplianceStatus.Approved.GetStringValue() });
                    rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.DECLINED.GetStringValue(), Value = ApplicantItemComplianceStatus.Not_Approved.GetStringValue() });
                    //Added Expired status because admin can edit expired items also.(UAT-505)
                    rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.EXPIRED.GetStringValue(), Value = ApplicantItemComplianceStatus.Expired.GetStringValue() });//UAT-505 
                    rbtnListAction.Items.FindByValue(ApplicantItemComplianceStatus.Expired.GetStringValue()).Enabled = false;//Disabled the expired status 
                }

                //[BS]: Implemented UAT-527 As an ADB admin, I should not have the option to "Approve" items for packages/categories that are setup for joint review.
                if (_presenter.IsDefaultTenant)
                {
                    RemoveApprovedAction(rbtnListAction, lstAssignmentProperties);
                }

                if (rbtnListAction.SelectedValue.IsNullOrEmpty())
                    rbtnListAction.SelectedValue = hdfItemStatus.Value;
            }
        }

        /// <summary>
        /// Remove the 'Approved' action from Action RadioButtonList if Reviewer Type is both 'Admin' and 'Client Admin'.
        /// </summary>
        /// <param name="rbtnListAction">RadioButtonList from which Action is to be removed.</param>
        /// <param name="lstAssignmentProperties">List of ItemAssignmentProperties</param>
        private static void RemoveApprovedAction(RadioButtonList rbtnListAction, List<ListItemAssignmentProperties> lstAssignmentProperties)
        {
            if (lstAssignmentProperties.IsNotNull() && lstAssignmentProperties.Any(revType => revType.ReviewerTypeCode == ReviewerType.Admin.GetStringValue())
                    && lstAssignmentProperties.Any(revType => revType.ReviewerTypeCode == ReviewerType.Client_Admin.GetStringValue()))
            {
                if (rbtnListAction.Items.FindByText(VerificationDataActions.APPROVED.GetStringValue()).IsNotNull())
                {
                    rbtnListAction.Items.FindByText(VerificationDataActions.APPROVED.GetStringValue()).Attributes.Add("Style", "Display:none;");//Hide Approved status in case of ADB_Admin for UAT-505 
                }
            }
        }

        private void SetStatus()
        {
            if ((_presenter.IsDefaultTenant || this.CurrentTenantTypeCode == TenantType.Institution.GetStringValue())
                  && (String.IsNullOrEmpty(CurrentViewContext.ItemComplianceStatusCode) || CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Incomplete.GetStringValue())))
            {
                rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.INCOMPLETE.GetStringValue(), Value = ApplicantItemComplianceStatus.Incomplete.GetStringValue(), Selected = true });
            }
        }

        #endregion

        #region Public Methods

        public void RebindItemDataEditModeControlsData(List<ApplicantItemVerificationData> applicantItmVerificationData, Boolean isReadOnlyControlsAfterSave)
        {
            this.IsRebindAfterSave = true;
            this.IsReadOnlyAfterSave = isReadOnlyControlsAfterSave;

            txtAdminNote.Text = String.Empty;
            CurrentViewContext.VerificationData = applicantItmVerificationData;
            ReloadingDataAfterSave = true;
            LoadItemDetails();
            DisplayActions();
            if (isReadOnlyControlsAfterSave)
            {
                dvApplicantNotes.Visible = false;
                spnCommentsHistory.InnerText = "Comments:";
                dvCombined.Visible = false;
            }
            //UAT-3951
            ShowHideRejectionReasonControl();
        }

        /// <summary>
        /// Validate the complete item data
        /// </summary>
        /// <param name="lstCompleteData"></param>
        /// <returns></returns>
        public Boolean ValidateData(List<ApplicantComplianceAttributeData> lstCompleteData)
        {
            // DO NOT VALIDATE IN CASE STATUS IS INCOMPLETE
            if (rbtnListAction.SelectedValue == ApplicantItemComplianceStatus.Incomplete.GetStringValue())// || lstCompleteData.IsNullOrEmpty() || lstCompleteData.Count() == 0)
                return true;

            GetApplicantItemData();
            Presenter.ValidateApplicantData(new List<ApplicantComplianceItemData>(), lstCompleteData);

            if (String.IsNullOrEmpty(CurrentViewContext.UIInputException))
                return true;
            else
            {
                lblMessage.Text = CurrentViewContext.UIInputException;
                lblMessage.CssClass = "error";
                return false;
            }
        }

        public void SetValidationMessage(String validationMessage)
        {
            lblMessage.Text = validationMessage;
            lblMessage.CssClass = "error";
        }

        public void SetSuccessMessage(String sucessMessage)
        {
            lblMessage.Text = sucessMessage;
            lblMessage.CssClass = "sucs";
        }

        /// <summary>
        /// Get the complete data for the Verification details screen as well.
        /// </summary>
        public List<ApplicantComplianceAttributeData> GetApplicantItemData()
        {
            lstApplicantComplianceAttributeData = new List<ApplicantComplianceAttributeData>();
            // DO NOT VALIDATE IN CASE STATUS IS INCOMPLETE
            if (rbtnListAction.SelectedValue != ApplicantItemComplianceStatus.Incomplete.GetStringValue())
            {
                for (int i = 0; i < rpAttributes.Items.Count; i++)
                {
                    String _attributeValue = String.Empty;
                    String _attrbuteDataTypeCode = ((WclTextBox)rpAttributes.Items[i].FindControl("txtDataType")).Text;

                    if (_attrbuteDataTypeCode.ToLower() != ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower() && _attrbuteDataTypeCode.ToLower() != ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower())
                    {
                        Int32 _attributeDataId = AppConsts.NONE;

                        HiddenField _hdfAttributeDataId = (HiddenField)rpAttributes.Items[i].FindControl("hdfAttributeDataId");

                        if (!String.IsNullOrEmpty(_hdfAttributeDataId.Value))
                            _attributeDataId = Convert.ToInt32(_hdfAttributeDataId.Value);

                        Int32 _attributeId = Convert.ToInt32(((HiddenField)rpAttributes.Items[i].FindControl("hdfAttributeId")).Value);

                        if (_attrbuteDataTypeCode == ComplianceAttributeDatatypes.Date.GetStringValue().ToLower().Trim())
                        {
                            _attributeValue = (rpAttributes.Items[i].FindControl("dtPicker") as WclDatePicker).SelectedDate.IsNullOrEmpty() ? String.Empty : Convert.ToDateTime((rpAttributes.Items[i].FindControl("dtPicker") as WclDatePicker).SelectedDate).ToShortDateString();
                        }
                        else if (_attrbuteDataTypeCode == ComplianceAttributeDatatypes.Text.GetStringValue().ToLower().Trim())
                        {
                            _attributeValue = (rpAttributes.Items[i].FindControl("txtBox") as WclTextBox).Text;
                        }
                        else if (_attrbuteDataTypeCode == ComplianceAttributeDatatypes.Numeric.GetStringValue().ToLower().Trim())
                        {
                            _attributeValue = (rpAttributes.Items[i].FindControl("numericTextBox") as WclNumericTextBox).Text;
                        }
                        else if (_attrbuteDataTypeCode == ComplianceAttributeDatatypes.Options.GetStringValue().ToLower().Trim())
                        {
                            _attributeValue = (rpAttributes.Items[i].FindControl("optionCombo") as WclComboBox).SelectedValue;
                        }
                        lstApplicantComplianceAttributeData.Add(new ApplicantComplianceAttributeData
                        {
                            AttributeValue = _attributeValue,
                            ApplicantComplianceAttributeID = _attributeDataId,
                            ComplianceAttributeID = _attributeId,
                            ApplicantComplianceItemID = CurrentViewContext.ApplicantItemDataId, // This can be ZERO as Incomplete items can now be added from Verification Details Screen
                            ComplianceItemId = Convert.ToInt32(hdfComplianceItemId.Value),
                        });
                    }
                }
            }
            return lstApplicantComplianceAttributeData;
        }

        public String Save(out ApplicantComplianceItemData _applicantItemData, String recordActionType)
        {
            try
            {
                _applicantItemData = null;
                CurrentViewContext.ItemMovementStatusCode = String.Empty;
                // DO NOT SAVE/UPDATE IN CASE STATUS IS INCOMPLETE
                if (!String.IsNullOrEmpty(rbtnListAction.SelectedValue) && ((rbtnListAction.SelectedValue != ApplicantItemComplianceStatus.Incomplete.GetStringValue())
                    || (rbtnListAction.SelectedValue == ApplicantItemComplianceStatus.Incomplete.GetStringValue() && !ApplicantItemDataId.IsNullOrEmpty() && ApplicantItemDataId != AppConsts.NONE)))
                {
                    CurrentViewContext.AttemptedItemStatus = rbtnListAction.SelectedValue;

                    String _newStatus = GetNewSatatus(recordActionType);
                    // if (rbtnListAction.SelectedValue == ApplicantItemComplianceStatus.Approved.GetStringValue())
                    if (_newStatus == ApplicantItemComplianceStatus.Approved.GetStringValue())
                    {
                        CurrentViewContext.ReviewerTenantId = AppConsts.NONE; // To stop display in Third Party Listing
                        CurrentViewContext.TPReviewerUserId = AppConsts.NONE; // To stop display in TP User Listing
                        CurrentViewContext.ReviewerTypeId = AppConsts.NONE;  // To stop display in Client Admin or Admin Listing
                    }
                    else if (_newStatus == ApplicantItemComplianceStatus.Not_Approved.GetStringValue())
                    {
                        // Clear the movement status, if Rejected
                        CurrentViewContext.ReviewerTenantId = AppConsts.NONE;
                        CurrentViewContext.TPReviewerUserId = AppConsts.NONE;
                        CurrentViewContext.ReviewerTypeId = AppConsts.NONE;
                    }
                    else if (_newStatus == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue())
                    {
                        CurrentViewContext.ReviewerTenantId = AppConsts.NONE;
                    }
                    else if (_newStatus == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue()) // From both Admin and Client admin
                    {
                        CurrentViewContext.ReviewerTypeId = AppConsts.NONE;
                    }
                    else if (_newStatus == ApplicantItemComplianceStatus.Pending_Review.GetStringValue())
                    {
                        CurrentViewContext.ReviewerTenantId = AppConsts.NONE;
                        CurrentViewContext.TPReviewerUserId = AppConsts.NONE;
                    }


                    if (!(txtAdminNote.Text.IsNullOrEmpty()))
                    {
                        CurrentViewContext.Comments = Environment.NewLine + "[" + CurrentViewContext.CurrentLoggedInUserName_Global + " " + DateTime.Now.Date.ToShortDateString() + "] " + txtAdminNote.Text;
                        CurrentViewContext.StatusComments = Environment.NewLine + "[" + CurrentViewContext.LoggedInUserInitials_Global + " " + DateTime.Now.Date.ToShortDateString() + "] " + txtAdminNote.Text;
                        CurrentViewContext.VerificationCommentsWithInitials = Environment.NewLine + "[" + CurrentViewContext.LoggedInUserInitials_Global + "]" + txtAdminNote.Text; //UAT 2807
                    }
                    else
                    {
                        CurrentViewContext.Comments = String.Empty;
                        CurrentViewContext.StatusComments = String.Empty;
                    }

                    _applicantItemData = _presenter.SaveApplicantData(this.CurrentTenantTypeCode, recordActionType);
                    return String.Empty;
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (SysXException ex)
            {
                _applicantItemData = null;
                base.LogError(ex);
                return litItemName.Text + " could not be saved";
            }
            catch (Exception ex)
            {
                _applicantItemData = null;
                base.LogError(ex);
                return litItemName.Text + " could not be saved";
            }
        }

        /// <summary>
        /// Returns the new status of the item that should be considered, based on the action type
        /// </summary>
        /// <param name="recordActionType"></param>
        /// <returns></returns>
        private String GetNewSatatus(String recordActionType)
        {
            String _newStatusToUse = String.Empty;

            if (recordActionType == lkpQueueActionType.Next_Level_Review_Required.GetStringValue()
                || recordActionType == lkpQueueActionType.Escalation_Required.GetStringValue())
            {
                _newStatusToUse = CurrentViewContext.ItemComplianceStatusCode;
            }
            else if (recordActionType == lkpQueueActionType.Proceed_To_Next_Queue.GetStringValue()
                || recordActionType == lkpQueueActionType.No_Status_Changed.GetStringValue())
            {
                _newStatusToUse = rbtnListAction.SelectedValue;
            }
            return _newStatusToUse;
        }


        #endregion

        #endregion

        #region UAT-3951: Addition of option to use preset ADB Admin rejection notes
        private void BindRejectionReasons()
        {
            cmbRejectionReason.DataSource = CurrentViewContext.ListRejectionReasons;
            cmbRejectionReason.DataBind();
        }

        protected void cmbRejectionReason_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            try
            {
                Entity.RejectionReason rejectionReasonObj = (Entity.RejectionReason)e.Item.DataItem;
                if (!rejectionReasonObj.IsNullOrEmpty())
                {
                    e.Item.Attributes["RR_ReasonText"] = rejectionReasonObj.RR_ReasonText;
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

        private void ShowHideRejectionReasonControl()
        {
            if (IsAdminLoggedIn)
            {
                if (String.Compare(rbtnListAction.SelectedValue, NotApprovedStatusCode, true) == AppConsts.NONE)
                {
                    dvRejectionReason.Style["display"] = "block";
                }
                else
                {
                    dvRejectionReason.Style["display"] = "none";
                }
            }
        }
        #endregion

    }
}

