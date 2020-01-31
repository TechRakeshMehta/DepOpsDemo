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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Text;
using Business.RepoManagers;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ItemDataEditMode : BaseUserControl, IItemDataEditModeView
    {
        #region Variables

        #region Private Variables

        private ItemDataEditModePresenter _presenter = new ItemDataEditModePresenter();
        private Int32 _editableCount = 0;

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


        public ItemDataEditModePresenter Presenter
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

        public IItemDataEditModeView CurrentViewContext
        {
            get { return this; }
        }

        public List<ApplicantItemVerificationData> VerificationData
        {
            get;
            set;
        }

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
                ViewState["lstApplicantDocument"] = ucVerificationDetailsDocumentConrol.lstApplicantDocument = value;
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

        public Boolean IsDeleteChecked
        {
            get { return chkDeleteItem.Checked; }
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
        /// Contains the data of all the Attributes of the current Item
        /// </summary>
        public List<ListItemEditableBies> lstEditableByData
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

        public String ComplianceItemName { get { return litItemName.Text; } }

        public Boolean IsItmEditableByApplcnt { get; set; } //UAT-3599

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

        //UAT-1056
        public Boolean IsItemAsisgnedToCurrentUser
        {
            get
            {
                if (!ViewState["IsItemAsisgnedToCurrentUser"].IsNullOrEmpty())
                {
                    return (Boolean)ViewState["IsItemAsisgnedToCurrentUser"];
                }
                return false;
            }
            set
            {
                if (ViewState["IsItemAsisgnedToCurrentUser"].IsNullOrEmpty())
                    ViewState["IsItemAsisgnedToCurrentUser"] = value;
            }
        }

        public Int32? ReconciliationReviewCount
        {
            get
            {
                if (!ViewState["ReconciliationReviewCount"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(ViewState["ReconciliationReviewCount"]);
                }
                return null;
            }
            set
            {
                ViewState["ReconciliationReviewCount"] = value;
            }
        }

        //UAT-2079:Do not trigger UI rules on verification details screen when only deleting an item's data
        public Boolean IsDataChanged { get; set; }
        //
        public Boolean IsAdminLoggedIn { get; set; }
        //2528
        public Boolean IsUiRulesViolate { get; set; }

        //UAT 2807
        String IItemDataEditModeView.VerificationCommentsWithInitials
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

        List<Int32> IItemDataEditModeView.SelectedRejectionReasonIds
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

        //public Boolean IsAdminLoggedIn 
        //{
        //    get { return Presenter.IsDefaultTenant; }
        //}

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

            CurrentViewContext.ReconciliationReviewCount = CurrentViewContext.VerificationData[0].ReconciliationReviewCount;
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            DisplayActions();
            SetDocumentControlProperties();
            HighlightAssignedItems();//UAT-1056 Need visual indicator of item assigned/not assigned
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
                //if (_verificationData.IsNotNull() && _verificationData.ComplianceAttributeId.IsNotNull())
                if (_verificationData.IsNotNull()) // If File upload type attribute exists
                {

                    this.FileUpoladAttributeId = _verificationData.ComplianceAttributeId;

                    // Check if editable ?
                    ucVerificationDetailsDocumentConrol.IsReadOnly = !IsAttributeEditable(Convert.ToInt32(_verificationData.ComplianceAttributeId));

                    if (_verificationData.ApplicantCompItemId.IsNotNull()) // If the data was already added for the Document attributes' item
                        ucVerificationDetailsDocumentConrol.ItemDataId = Convert.ToInt32(_verificationData.ApplicantCompItemId);
                    ucVerificationDetailsDocumentConrol.lstAssignmentProperties = this.lstAssignmentProperties;
                    ucVerificationDetailsDocumentConrol.ApplicantId = this.SelectedApplicantId_Global;
                    ucVerificationDetailsDocumentConrol.IsFileUploadApplicable = this.IsFileUpoloadApplicable = true;
                    ucVerificationDetailsDocumentConrol.ComplianceAttributeId = Convert.ToInt32(_verificationData.ComplianceAttributeId);
                    ucVerificationDetailsDocumentConrol.lstApplicantComplianceDocumentMaps = this.lstApplicantComplianceDocumentMaps;
                }
                else if (!_verificationDataViewDoc.IsNullOrEmpty())//if only view doc attribute present
                {
                    // Check if editable ?
                    ucVerificationDetailsDocumentConrol.IsReadOnly = !IsAttributeEditable(Convert.ToInt32(_verificationDataViewDoc.ComplianceAttributeId));
                    if (_verificationDataViewDoc.ApplicantCompItemId.IsNotNull()) // If the data was already added for the Document attributes' item
                        ucVerificationDetailsDocumentConrol.ItemDataId = Convert.ToInt32(_verificationDataViewDoc.ApplicantCompItemId);
                    ucVerificationDetailsDocumentConrol.lstApplicantComplianceDocumentMaps = this.lstApplicantComplianceDocumentMaps;
                    ucVerificationDetailsDocumentConrol.lstAssignmentProperties = this.lstAssignmentProperties;
                    ucVerificationDetailsDocumentConrol.ComplianceAttributeId = Convert.ToInt32(_verificationDataViewDoc.ComplianceAttributeId);
                    ucVerificationDetailsDocumentConrol.ApplicantId = this.SelectedApplicantId_Global;
                    ucVerificationDetailsDocumentConrol.IsFileUploadApplicable = false;
                    ucVerificationDetailsDocumentConrol.IsViewDocApplicable = true;
                }
                else
                {
                    ucVerificationDetailsDocumentConrol.IsReadOnly = false;
                    ucVerificationDetailsDocumentConrol.IsFileUploadApplicable = false;
                }

                #region UAT-3077:(1 of ?) Initial Analysis and begin Dev: Pay per submission item type (CC only) for Tracking and Rotation
                if (CurrentViewContext.VerificationData[0].ItemAmount.IsNotNull())
                {
                    decimal itemAmount = CurrentViewContext.VerificationData[0].ItemAmount.Value;
                    litItemAmount.Text = "$" + itemAmount.ToString("0.00");
                }

                litItemPaymentStatus.Text = CurrentViewContext.VerificationData[0].ItemPaymentStatus;

                if (!CurrentViewContext.VerificationData[0].IsNullOrEmpty()
                    && CurrentViewContext.VerificationData[0].IsItemPaymentPaid.HasValue
                    && CurrentViewContext.VerificationData[0].IsItemPaymentPaid.Value
                    && CurrentViewContext.VerificationData[0].PaidItemAmount.HasValue)
                {
                    litItemPaymentStatus.Text += string.Concat(" ($", CurrentViewContext.VerificationData[0].PaidItemAmount.Value.ToString("0.00"), ")"); ;
                }

                if (CurrentViewContext.VerificationData[0].IsPaymentTypeItem.Value)
                {
                    divItemPaymentPanel.Style["display"] = "block";
                }
                else
                {
                    divItemPaymentPanel.Style["display"] = "none";
                }
                #endregion
            }


            //showChkBox
            AddMethodToDelegate();


            hdfRejectionCodeItem.Value = ApplicantItemComplianceStatus.Not_Approved.GetStringValue();

            //Is chkDeleteItem visible to delete items
            IsDeleteApplicable();

            //UAT-3951:
            BindRejectionReasons();
            ShowHideRejectionReasonControl();
        }

        /// <summary>
        /// Method to Hightlight Assigned items
        /// </summary>
        private void HighlightAssignedItems()
        {
            if (CurrentViewContext.IsItemAsisgnedToCurrentUser)
            {
                dvlnkItemName.Attributes.Add("class", "highlightAssignedItem");
                dvDetailPanel.Attributes.Add("class", "highlightDetailBorder sxform auto");
            }
        }

        private void AddMethodToDelegate()
        {
            if (HttpContext.Current.Items["ShowDeleteCheckBox"] == null)
            {
                showChkBox = new ShowDeleteCheckBox(IsDeleteApplicable);
                HttpContext.Current.Items["ShowDeleteCheckBox"] = showChkBox;
            }
            else
            {
                showChkBox = (ShowDeleteCheckBox)HttpContext.Current.Items["ShowDeleteCheckBox"];
                showChkBox += new ShowDeleteCheckBox(IsDeleteApplicable);
                HttpContext.Current.Items["ShowDeleteCheckBox"] = showChkBox;
            }
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

            ucVerificationDetailsDocumentConrol.SelectedTenantId_Global = CurrentViewContext.SelectedTenantId_Global;
            ucVerificationDetailsDocumentConrol.ApplicantId = CurrentViewContext.SelectedApplicantId_Global;
            ucVerificationDetailsDocumentConrol.ComplianceItemId = CurrentViewContext.VerificationData[0].ComplianceItemId;
            ucVerificationDetailsDocumentConrol.PackageSubscriptionId = CurrentViewContext.CurrentPackageSubscriptionId;
            //ucVerificationDetailsDocumentConrol.lstApplicantDocument = CurrentViewContext.lstApplicantDocument;

            ucVerificationDetailsDocumentConrol.CompliancePackageId = CurrentViewContext.SelectedCompliancePackageId_Global;
            ucVerificationDetailsDocumentConrol.ComplianceCategoryId = CurrentViewContext.SelectedComplianceCategoryId_Global;

            if (rbtnListAction.SelectedValue == ApplicantItemComplianceStatus.Incomplete.GetStringValue())
                ucVerificationDetailsDocumentConrol.IsIncompleteItem = true;
            else
                ucVerificationDetailsDocumentConrol.IsIncompleteItem = false;
        }

        protected void rpAttributes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CurrentViewContext.ComplianceItemId = CurrentViewContext.VerificationData[0].ComplianceItemId;

            ApplicantItemVerificationData attributeData = (ApplicantItemVerificationData)e.Item.DataItem;

            String dataTypeCode = attributeData.AttributeTypeCode.ToLower().Trim();

            // Need to check is AttributeId is NULL from SP

            Boolean _isAttributeEditable = attributeData.AttTypeCode.ToLower() == ComplianceAttributeType.Manual.GetStringValue().ToLower()
                                       && IsAttributeEditable(Convert.ToInt32(attributeData.ComplianceAttributeId));// -- STEP 3. Hide the next line of code to  run this

            // Boolean _isAttributeEditable = attributeData.AttTypeCode.ToLower() == ComplianceAttributeType.Manual.GetStringValue().ToLower();

            WclTextBox txtDataType = (WclTextBox)e.Item.FindControl("txtDataType");
            txtDataType.Text = dataTypeCode;

            if (dataTypeCode == ComplianceAttributeDatatypes.Date.GetStringValue().ToLower().Trim())
            {
                WclDatePicker dtPicker = (WclDatePicker)e.Item.FindControl("dtPicker");
                if (!String.IsNullOrEmpty(attributeData.AttributeValue) && !DateTime.MinValue.ToShortDateString().Equals(attributeData.AttributeValue))
                    dtPicker.SelectedDate = Convert.ToDateTime(attributeData.AttributeValue);
                dtPicker.Visible = true;

                // Manage the Editably property, after rebind, on Save
                if (this.IsRebindAfterSave)
                    dtPicker.Enabled = (_isAttributeEditable && !this.IsReadOnlyAfterSave) ? true : false;
                else
                    dtPicker.Enabled = _isAttributeEditable ? true : false;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Text.GetStringValue().ToLower().Trim())
            {
                WclTextBox txtBox = (WclTextBox)e.Item.FindControl("txtBox");
                txtBox.Text = attributeData.AttributeValue;
                txtBox.MaxLength = attributeData.MaximumCharacters == null ? 50 : Convert.ToInt32(attributeData.MaximumCharacters);
                txtBox.Visible = true;

                // Manage the Editably property, after rebind, on Save
                if (this.IsRebindAfterSave)
                    txtBox.Enabled = (_isAttributeEditable && !this.IsReadOnlyAfterSave) ? true : false;
                else
                    txtBox.Enabled = _isAttributeEditable ? true : false;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Numeric.GetStringValue().ToLower().Trim())
            {
                WclNumericTextBox numericTextBox = (WclNumericTextBox)e.Item.FindControl("numericTextBox");
                numericTextBox.Text = attributeData.AttributeValue;
                numericTextBox.Visible = true;

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
                optionCombo.SelectedValue = attributeData.AttributeValue;
                //This check for the very first time the page is loaded and if save all changes is executed.
                if (ReloadingDataAfterSave || (!Page.IsPostBack))
                {
                    //To set the initial value of selected dropdown value in text box if there is some value.
                    userValuefield.Text = attributeData.AttributeValue;
                }
                if (Page.IsPostBack)
                {
                    //We dont get value of the text box here so we add the object here to get the value later.
                    dropdownValues.Add(optionCombo, userValuefield);
                }
                optionCombo.DataBind();
                optionCombo.Visible = true;

                if (this.IsRebindAfterSave)
                    optionCombo.Enabled = (_isAttributeEditable && !this.IsReadOnlyAfterSave) ? true : false;
                else
                    optionCombo.Enabled = _isAttributeEditable ? true : false;

                //WclComboBox optionCombo1 = (WclComboBox)e.Item.FindControl("optionCombo1");
                //optionCombo1.SelectedValue = attributeData.AttributeValue;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower().Trim())
            {
                Label lblViewDoc = (Label)e.Item.FindControl("lblViewDoc");
                if (!lblViewDoc.IsNull() && attributeData.ApplAttributeDataId.HasValue && attributeData.ApplAttributeDataId > AppConsts.NONE)
                {
                    lblViewDoc.Visible = true;
                    if (attributeData.AttributeValue == AppConsts.ZERO && attributeData.ItemComplianceStatusCode == ApplicantItemComplianceStatus.Expired.GetStringValue())
                    {
                        lblViewDoc.Text = VerificationDataActions.EXPIRED.GetStringValue();
                    }
                    else if (attributeData.AttributeValue == AppConsts.ZERO)
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
                    if (Presenter.IsDefaultTenant || Presenter.IsEDFormPreviouslyAccepted())
                    {
                        lnkBtn.OnClientClick = "ViewScreeningDocument(" + CurrentViewContext.SelectedTenantId_Global + ", " + document.ScreeningDocumentId + ");";
                    }
                    else
                    {
                        lnkBtn.OnClientClick = "OpenEmployerDisclosureDocument(" + CurrentViewContext.SelectedTenantId_Global + ", " + CurrentViewContext.CurrentLoggedInUserId + ", " + document.ScreeningDocumentId + ", '" + DislkpDocumentType.EMPLOYMENT_DISCLOSURE_FORM.GetStringValue() + "');";
                    }

                    lnkBtn.Attributes.Add("onclick", "return false;");

                    lnkBtn.Text = document.ScreeningDocumentName;
                    _divScreeningDocuments.Controls.Add(lnkBtn);
                    _divScreeningDocuments.Controls.Add(new LiteralControl("<br />"));
                }
            }

            else if (dataTypeCode == ComplianceAttributeDatatypes.Signature.GetStringValue().ToLower().Trim())
            {
                string value = "No";
                if (!string.IsNullOrEmpty(attributeData.AttributeValue) && attributeData.AttributeValue.ToLower() == "true")
                    value = "Yes";

                WclTextBox txtBox = (WclTextBox)e.Item.FindControl("txtBox");
                // txtBox.ID = "txt_" + attributeData.ComplianceAttributeId;
                txtBox.Text = value;
                txtBox.Visible = true;
                txtBox.Enabled = false;
            }
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
                //UAT 2528
                IsAdminLoggedIn = (Presenter.IsDefaultTenant);
                //IsAdminLoggedIn = (SecurityManager.DefaultTenantID == this.CurrentTenantId_Global);
                if (IsAdminLoggedIn)
                {
                    if (CurrentViewContext.VerificationData[0].IsUiRulesViolate)
                    {
                        dvDetailPanel.Attributes.Add("Style", "background-color:#ff6666 !important");
                    }
                    else
                    {
                        dvDetailPanel.Attributes.Remove("Style");
                    }
                }

                litItemName.Text = CurrentViewContext.VerificationData[0].ItemName.HtmlEncode();
                if (!IsItmEditableByApplcnt) //UAT-3599
                {
                    imageSDEdisabled.Visible = true;
                }

                #region UAT 725: Add submission time and date to verification details screen for each item
                if (CurrentViewContext.VerificationData[0].SubmissionDate.HasValue && CurrentViewContext.VerificationData[0].SubmissionDate.Value != new DateTime(1900, 1, 1))
                {
                    lblSubmissionDate.Text = Convert.ToString(CurrentViewContext.VerificationData[0].SubmissionDate) + "(MT)";

                }
                else
                {
                    lblSubmissionDate.Text = String.Empty;
                }
                #endregion

                #region UAT:719 Check Exceptions turned off for a Category/Item
                Boolean isExceptionAllowed = Presenter.IsAllowExceptionOnCategory();
                if (isExceptionAllowed)
                {
                    imageExceptionOff.Visible = true;
                }
                #endregion
                #region UAT:3614 Icon on Three Panel Screen if Item is set to approval required: No
                if (IsAdminLoggedIn)
                {
                    ListItemAssignmentProperties assignmentProperty = this.lstAssignmentProperties.FirstOrDefault();
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


                litApplicantNotes.Text = String.IsNullOrEmpty(CurrentViewContext.VerificationData[0].ApplicantItemNotes) ?
                    " <span class='no-data'>No Data</span>"
                    : CurrentViewContext.VerificationData[0].ApplicantItemNotes;

                //hdnUrl.Value = sampleDocFormURL;
                CurrentViewContext.ItemComplianceStatusCode = CurrentViewContext.VerificationData[0].ItemComplianceStatusCode;
                CurrentViewContext.PackageId = CurrentViewContext.SelectedCompliancePackageId_Global;
                //UAT - 2807
                if (IsAdminLoggedIn)
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

                //List<ApplicantItemVerificationData> lstAttributes = VerificationData.GroupBy(s => s.ApplAttributeDataId)
                //    .Select(attr => attr.First())
                //    .Where(att => att.ApplAttributeDataId.IsNotNull() && att.AttributeTypeCode.ToLower() != ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower() && att.ApplAttributeDataId.IsNotNull())
                //    .ToList<ApplicantItemVerificationData>();

                // Data entry for Incomplete items as well
                List<ApplicantItemVerificationData> lstAttributes = VerificationData
                  .GroupBy(attr => attr.ComplianceAttributeId) // Group by to avoid Repition of attributes in case of the OPTIONS type attribute
                  .Select(attr => attr.First())
                  .Where(att => att.AttributeTypeCode.IsNotNull() && att.AttributeTypeCode.ToLower() != ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower())
                  //.OrderBy(x => x.ComplianceAttributeId) //Sumit:  Done Order By in SP 
                  .ToList<ApplicantItemVerificationData>();

                string[] sampleDocFormURLs = VerificationData[0].SampleDocFormURL.Split(',');
                string[] sampleDocFormDisplayUrlLabels = VerificationData[0].SampleDocFormDisplayURLLabel.Split(',');//"..\\InstitutionImages\\Libertyuniversity\\Jellyfish.jpg";
                string SampleDocLink = string.Empty;
                StringBuilder sbDocumentLinks = new StringBuilder();
                for (int i = 0; i < sampleDocFormURLs.Count(); i++)
                {
                    if (!sampleDocFormURLs[i].IsNullOrEmpty())
                    {
                        sbDocumentLinks.Append("<br /><a href=\"" + sampleDocFormURLs[i] + "\" onclick=\"\" target=\"_blank\");'>" +
                            (sampleDocFormDisplayUrlLabels[i].IsNullOrEmpty() ? "View Sample Document" : "View " + sampleDocFormDisplayUrlLabels[i]) + "</a>");
                    }
                }
                //foreach (var sampleDocFormURL in sampleDocFormURLs)
                //{
                //    //Only display hyperlink if sampleDocFromUrl available in VerificationData
                //    if (!sampleDocFormURL.IsNullOrEmpty())
                //    {
                //        sbDocumentLinks.Append("<br /><a href=\"" + sampleDocFormURL + "\" onclick=\"\" target=\"_blank\");'>View Sample Doc1</a>");
                //    }
                //}
                SampleDocLink = sbDocumentLinks.ToString();

                ucExplanationDescription.ApplicantExplanation = CurrentViewContext.VerificationData[0].ItemExpNotes;
                ucExplanationDescription.AdminExplanation = CurrentViewContext.VerificationData[0].ItemDescription + @SampleDocLink;

                rpAttributes.DataSource = lstAttributes;
                rpAttributes.DataBind();

                hdfItemStatus.Value = VerificationData[0].ItemComplianceStatusCode;
                hdfComplianceItemId.Value = Convert.ToString(VerificationData[0].ComplianceItemId);
                hdfComplianceItemName.Value = Convert.ToString(VerificationData[0].ItemName);
                String _divType = ComplianceVerificationDetailsContract.GetItemDivType(CurrentViewContext.ItemComplianceStatusCode);
                divEditMode.Attributes.Add("divType", _divType);

                #region UAT-722
                String unifiedDocPageMapping = String.Empty;
                //UAT-1538
                String appDocumentIds = String.Empty;
                var currentItemUnifiedDocList = lstApplicantDocument.Where(cond => cond.ComplianceItemID == CurrentViewContext.VerificationData[0].ComplianceItemId).ToList();
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

                //UAT-3077
                if (CurrentViewContext.VerificationData[0].ItemAmount.IsNotNull())
                {
                    decimal itemAmount = CurrentViewContext.VerificationData[0].ItemAmount.Value;
                    litItemAmount.Text = "$" + itemAmount.ToString("0.00");
                }

                litItemPaymentStatus.Text = CurrentViewContext.VerificationData[0].ItemPaymentStatus.HtmlEncode();

                if (!CurrentViewContext.VerificationData[0].IsNullOrEmpty()
                    && CurrentViewContext.VerificationData[0].IsItemPaymentPaid.HasValue
                    && CurrentViewContext.VerificationData[0].IsItemPaymentPaid.Value
                    && CurrentViewContext.VerificationData[0].PaidItemAmount.HasValue)
                {
                    litItemPaymentStatus.Text += string.Concat(" ($", CurrentViewContext.VerificationData[0].PaidItemAmount.Value.ToString("0.00"), ")").HtmlEncode();
                }

                if (CurrentViewContext.VerificationData[0].IsPaymentTypeItem.Value)
                {
                    divItemPaymentPanel.Style["display"] = "block";
                }
                else
                {
                    divItemPaymentPanel.Style["display"] = "none";
                }
            }
        }

        /// <summary>
        /// Display the Radio buttons for possible Admin actions
        /// </summary>
        private void DisplayActionsOld()
        {
            ListItemAssignmentProperties assignmentProperty = this.lstAssignmentProperties.FirstOrDefault();
            SetProperties(assignmentProperty);

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
                //Commented Code for UAT-505 
                // &&
                //(
                //    String.IsNullOrEmpty(CurrentViewContext.ItemComplianceStatusCode) ||
                //    (CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Pending_Review.GetStringValue())) ||
                //    (CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue())) ||
                //    (CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue())) ||
                //    (CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Approved.GetStringValue())) ||
                //    (CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Not_Approved.GetStringValue())) ||   //UAT 373
                //    (CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue()))
                //    || CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Incomplete.GetStringValue()))
                //)
                {
                    /*rbtnListAction.Items.Clear();*/

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
                //else if client admin
                else if ((!Presenter.IsDefaultTenant
                    && this.CurrentTenantTypeCode == TenantType.Institution.GetStringValue())
                    //&& (!String.IsNullOrEmpty(CurrentViewContext.ItemComplianceStatusCode)UAT 505- commented out the code because client admin will also do data entry for incomplete items
                    //Commented this code regarding UAT-505
                    //|| CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue())
                    //|| CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Approved.GetStringValue())  //UAT 373
                    //|| CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Not_Approved.GetStringValue())  //UAT 373
                    //|| CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Incomplete.GetStringValue())
                    //UAT 472 client scenario
                    || ((!this.lstAssignmentProperties.Any(revType => revType.ReviewerTypeCode.Equals(ReviewerType.Admin.GetStringValue()))
                    && this.lstAssignmentProperties.Any(revType => revType.ReviewerTypeCode.Equals(ReviewerType.Client_Admin.GetStringValue()))
                    && assignmentProperty.ThirdPartyReviewerUserId == null)
                    ))
                {
                    SetStatus();

                    // Value Field is changed for the Client admin, but the text remains the same. To show the current status as selected
                    rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.PENDING_REVIEW.GetStringValue(), Value = ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue() });

                    if (Presenter.IsSendForThirdPartyReview(assignmentProperty))
                    {
                        rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.FURTHER_REVIEW_THIRD_PARTY.GetStringValue(), Value = ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue() });
                    }
                    rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.APPROVED.GetStringValue(), Value = ApplicantItemComplianceStatus.Approved.GetStringValue() });
                    rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.DECLINED.GetStringValue(), Value = ApplicantItemComplianceStatus.Not_Approved.GetStringValue() });
                    //Added Expired status because admin can edit expired items also.(UAT-505)
                    rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.EXPIRED.GetStringValue(), Value = ApplicantItemComplianceStatus.Expired.GetStringValue() });//UAT-505
                    rbtnListAction.Items.FindByValue(ApplicantItemComplianceStatus.Expired.GetStringValue()).Enabled = false;//Disabled the expired status 
                    rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.PENDING_REVIEW.GetStringValue(), Value = ApplicantItemComplianceStatus.Pending_Review.GetStringValue() });
                    rbtnListAction.Items.FindByValue(ApplicantItemComplianceStatus.Pending_Review.GetStringValue()).Attributes.Add("Style", "Display:none;");// to automativcally handle scenarios where client admin opens a record whcih is pending review for adb admin, and saves it without changing its status.
                }
                //else if Third Party/Compliance Reviewer
                else if (!Presenter.IsDefaultTenant
                    && this.CurrentTenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue()
                    && (String.IsNullOrEmpty(CurrentViewContext.ItemComplianceStatusCode) || CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue()))
                    && assignmentProperty.ReviewerTenantId == this.CurrentTenantId_Global
                    )
                {
                    // Value Field is changed for the Third Party, but the text remains the same. To show the current status as selected
                    rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.PENDING_REVIEW.GetStringValue(), Value = ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue() });

                    rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.APPROVED.GetStringValue(), Value = ApplicantItemComplianceStatus.Approved.GetStringValue() });
                    rbtnListAction.Items.Add(new ListItem { Text = VerificationDataActions.DECLINED.GetStringValue(), Value = ApplicantItemComplianceStatus.Not_Approved.GetStringValue() });
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

        private void DisplayActions()
        {
            ListItemAssignmentProperties assignmentProperty = this.lstAssignmentProperties.FirstOrDefault();
            SetProperties(assignmentProperty);

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
                    CurrentViewContext.IsAdminReviewRequired = this.lstAssignmentProperties
                        .Any(revType => revType.ReviewerTypeCode.ToLower() == LkpReviewerType.Admin.ToLower()); //UAT-4595
                }


                if (_presenter.IsDefaultTenant)
                {
                    SetStatus();

                    bool isPendingReviewRdnCreated = false;
                    bool isSendForClientReviewRdnCreated = false;

                    if (!this.lstAssignmentProperties.IsNullOrEmpty() && this.lstAssignmentProperties
                                                                      .Any(revType => revType.ReviewerTypeCode.IsNotNull() && revType.ReviewerTypeCode
                                                                      .Equals(ReviewerType.Admin.GetStringValue())))
                    {
                        AddItemsIntoRadioBtnList(VerificationDataActions.PENDING_REVIEW.GetStringValue(), ApplicantItemComplianceStatus.Pending_Review.GetStringValue());
                        isPendingReviewRdnCreated = true;
                    }


                    if (!this.lstAssignmentProperties.IsNullOrEmpty() && this.lstAssignmentProperties
                                                                      .Any(revType => revType.ReviewerTypeCode.IsNotNull() && revType.ReviewerTypeCode
                                                                      .Equals(ReviewerType.Client_Admin.GetStringValue())))
                    {
                        AddItemsIntoRadioBtnList(VerificationDataActions.SEND_FOR_CLIENT_REVIEW.GetStringValue(), ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue());
                        isSendForClientReviewRdnCreated = true;
                    }
                    else if (!this.lstAssignmentProperties.IsNullOrEmpty()
                    && this.lstAssignmentProperties.Any(revType => revType.ReviewerTypeCode.IsNotNull() && revType.ReviewerTypeCode.Equals(ReviewerType.Admin.GetStringValue()))
                    && !this.lstAssignmentProperties.Any(revType => revType.ReviewerTypeCode.IsNotNull() && revType.ReviewerTypeCode.Equals(ReviewerType.Client_Admin.GetStringValue()))
                    && assignmentProperty.ThirdPartyReviewerUserId != null
                    && VerificationData[0].ReviewerTenantId.IsNull()
                    && VerificationData[0].AssignedToUserId.IsNull())
                    {
                        AddItemsIntoRadioBtnList(VerificationDataActions.FURTHER_REVIEW_THIRD_PARTY.GetStringValue(), ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue());
                    }
                    else if (VerificationData[0].ReviewerTenantId.IsNotNull() || (VerificationData[0].ReviewerTypeCode.IsNotNull()
                        && VerificationData[0].ReviewerTypeCode.ToLower() == ReviewerType.Client_Admin.GetStringValue().ToLower()))
                    {
                        AddItemsIntoRadioBtnList(VerificationDataActions.SEND_FOR_CLIENT_REVIEW.GetStringValue(), ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue());
                        isSendForClientReviewRdnCreated = true;
                    }

                    AddItemsIntoRadioBtnList(VerificationDataActions.APPROVED.GetStringValue(), ApplicantItemComplianceStatus.Approved.GetStringValue());
                    AddItemsIntoRadioBtnList(VerificationDataActions.DECLINED.GetStringValue(), ApplicantItemComplianceStatus.Not_Approved.GetStringValue());
                    AddItemsIntoRadioBtnList(VerificationDataActions.EXPIRED.GetStringValue(), ApplicantItemComplianceStatus.Expired.GetStringValue(), true);//UAT-505 

                    if (!isPendingReviewRdnCreated)
                        AddItemsIntoRadioBtnList(VerificationDataActions.PENDING_REVIEW.GetStringValue(), ApplicantItemComplianceStatus.Pending_Review.GetStringValue(), false, true);

                    if (!isSendForClientReviewRdnCreated)
                        AddItemsIntoRadioBtnList(VerificationDataActions.SEND_FOR_CLIENT_REVIEW.GetStringValue(), ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue(), false, true);

                }
                else if ((!Presenter.IsDefaultTenant
                    && this.CurrentTenantTypeCode == TenantType.Institution.GetStringValue())
                    || ((!this.lstAssignmentProperties.Any(revType => revType.ReviewerTypeCode.Equals(ReviewerType.Admin.GetStringValue()))
                    && this.lstAssignmentProperties.Any(revType => revType.ReviewerTypeCode.Equals(ReviewerType.Client_Admin.GetStringValue()))
                    && assignmentProperty.ThirdPartyReviewerUserId == null)
                    ))
                {
                    SetStatus();
                    //UAT-4595
                    if (!this.lstAssignmentProperties.IsNullOrEmpty() && this.lstAssignmentProperties
                                                                      .Any(revType => revType.ReviewerTypeCode.IsNotNull() && revType.ReviewerTypeCode
                                                                      .Equals(ReviewerType.Client_Admin.GetStringValue())))
                    {
                        //UAT-4779
                        if (this.lstAssignmentProperties.Any(revType => revType.ReviewerTypeCode
                                                                      .Equals(ReviewerType.Client_Admin.GetStringValue())) && this.lstAssignmentProperties
                                                                      .Any(revType => revType.ReviewerTypeCode
                                                                      .Equals(ReviewerType.Admin.GetStringValue())) && ((String.IsNullOrEmpty(CurrentViewContext.ItemComplianceStatusCode) || CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Incomplete.GetStringValue()) ||
                                                                      (String.IsNullOrEmpty(CurrentViewContext.ItemComplianceStatusCode) ||CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Pending_Review.GetStringValue())))))
                        {
                            AddItemsIntoRadioBtnList(VerificationDataActions.PENDING_REVIEW.GetStringValue(), ApplicantItemComplianceStatus.Pending_Review.GetStringValue());
                        }
                        else
                        {
                            AddItemsIntoRadioBtnList(VerificationDataActions.PENDING_REVIEW.GetStringValue(), ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue());
                        }
                    }
                    else
                    {
                        AddItemsIntoRadioBtnList(VerificationDataActions.PENDING_REVIEW.GetStringValue(), ApplicantItemComplianceStatus.Pending_Review.GetStringValue());
                    }

                    if (Presenter.IsSendForThirdPartyReview(assignmentProperty))
                    {
                        AddItemsIntoRadioBtnList(VerificationDataActions.FURTHER_REVIEW_THIRD_PARTY.GetStringValue(), ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue());
                    }
                    AddItemsIntoRadioBtnList(VerificationDataActions.APPROVED.GetStringValue(), ApplicantItemComplianceStatus.Approved.GetStringValue());
                    AddItemsIntoRadioBtnList(VerificationDataActions.DECLINED.GetStringValue(), ApplicantItemComplianceStatus.Not_Approved.GetStringValue());
                    AddItemsIntoRadioBtnList(VerificationDataActions.EXPIRED.GetStringValue(), ApplicantItemComplianceStatus.Expired.GetStringValue(), true);//UAT-505
                    //AddItemsIntoRadioBtnList(VerificationDataActions.PENDING_REVIEW.GetStringValue(), ApplicantItemComplianceStatus.Pending_Review.GetStringValue(), false, true); UAT-4595
                }
                //else if Third Party/Compliance Reviewer
                else if (!Presenter.IsDefaultTenant
                    && this.CurrentTenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue()
                    && (String.IsNullOrEmpty(CurrentViewContext.ItemComplianceStatusCode) || CurrentViewContext.ItemComplianceStatusCode.Equals(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue()))
                    && assignmentProperty.ReviewerTenantId == this.CurrentTenantId_Global
                    )
                {
                    // Value Field is changed for the Third Party, but the text remains the same. To show the current status as selected
                    AddItemsIntoRadioBtnList(VerificationDataActions.PENDING_REVIEW.GetStringValue(), ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue());
                    AddItemsIntoRadioBtnList(VerificationDataActions.APPROVED.GetStringValue(), ApplicantItemComplianceStatus.Approved.GetStringValue());
                    AddItemsIntoRadioBtnList(VerificationDataActions.DECLINED.GetStringValue(), ApplicantItemComplianceStatus.Not_Approved.GetStringValue());
                }

                if (_presenter.IsDefaultTenant)
                {
                    RemoveApprovedAction(rbtnListAction, lstAssignmentProperties);
                }

                if (rbtnListAction.SelectedValue.IsNullOrEmpty())
                    rbtnListAction.SelectedValue = hdfItemStatus.Value;
            }
        }


        private void AddItemsIntoRadioBtnList(string text, string value, bool isDisabled = false, bool isHidden = false)
        {
            rbtnListAction.Items.Add(new ListItem { Text = text, Value = value });

            if (isDisabled)
                rbtnListAction.Items.FindByValue(value).Enabled = false;

            if (isHidden)
            {
                // to automativcally handle scenarios where client admin opens a record whcih is pending review for adb admin, and saves it without changing its status.
                rbtnListAction.Items.FindByValue(value).Attributes.Add("Style", "Display:none;");
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
                    //rbtnListAction.Items.Remove(rbtnListAction.Items.FindByText(VerificationDataActions.APPROVED.GetStringValue())); 
                    rbtnListAction.Items.FindByText(VerificationDataActions.APPROVED.GetStringValue()).Attributes.Add("Style", "Display:none;");//Hide Approved status in case of ADB_Admin for UAT-505 
                    //    new ListItem
                    //{
                    //    Text = VerificationDataActions.APPROVED.GetStringValue(),
                    //    Value = ApplicantItemComplianceStatus.Approved.GetStringValue()
                    //});
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
            //else
            //    rbtnListAction.SelectedValue = hdfItemStatus.Value;

        }
        /// <summary>
        /// Check if the attribute can be edited
        /// </summary>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        private Boolean IsAttributeEditable(Int32 attributeId)
        {
            String _attributeTypeCode = LCObjectType.ComplianceATR.GetStringValue();
            String _itemTypeCode = LCObjectType.ComplianceItem.GetStringValue();

            Boolean _isEditable = false;

            List<ListItemEditableBies> _attributeEditableByList = lstEditableByData.Where
                      (data => data.ComplianceAttributeId == attributeId).ToList();

            //  Code can be null in case Assignement Property is defined at the 
            // Package Level or not defined at all in the hierarchy
            if (_attributeEditableByList.IsNullOrEmpty() || _attributeEditableByList.Any(eb => String.IsNullOrEmpty(eb.EditableByCode)))
                _isEditable = false;

            else
            {
                // Check for ADB or Client Admin
                if (_presenter.IsDefaultTenant)
                {
                    if (this.lstEditableByData.Where(editableBy => editableBy.EditableByCode == LkpEditableBy.Admin
                                                     && editableBy.ComplianceAttributeId == attributeId).Any())
                        _isEditable = true;
                }
                else if
                  (!_presenter.IsDefaultTenant && this.CurrentTenantTypeCode == TenantType.Institution.GetStringValue())
                {
                    if (this.lstEditableByData
                        .Where(editableBy => editableBy.EditableByCode == LkpEditableBy.InstitutionAdmin
                              && editableBy.ComplianceAttributeId == attributeId).Any())
                        _isEditable = true;
                }
                else // Editable for Third party
                {
                    _isEditable = true;
                    _editableCount = 0;
                }
            }
            if (_isEditable)
                _editableCount++;

            return _isEditable;
        }

        private void SetProperties(ListItemAssignmentProperties assignmentProperty)
        {
            if (CurrentViewContext.SelectedTenantId_Global != Presenter.DefaultTenantId
                && CurrentViewContext.CurrentTenantId_Global == Presenter.DefaultTenantId)
            {
                if (assignmentProperty.ApprovalRequired == true)
                {
                    var clientAdmin = this.lstAssignmentProperties
                                           .FirstOrDefault(x => x.ReviewerTypeCode == LkpReviewerType.ClientAdmin);
                    if (clientAdmin.IsNull())
                        CurrentViewContext.ReviewerTypeId = 0;
                    else
                        CurrentViewContext.ReviewerTypeId = Convert.ToInt16(clientAdmin.ReviewerTypeId);

                    CurrentViewContext.IsAdminReviewRequired = this.lstAssignmentProperties
                        .Any(revType => revType.ReviewerTypeCode.ToLower() == LkpReviewerType.Admin.ToLower());
                }

                if (assignmentProperty.ThirdPartyReviewerUserId > 0)
                    CurrentViewContext.TPReviewerUserId = Convert.ToInt32(assignmentProperty.ThirdPartyReviewerUserId);
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
            //ucVerificationDetailsDocumentConrol.UpdateNewDocumentList();
            IsDeleteApplicable();
            DisplayActions();
            if (isReadOnlyControlsAfterSave)
            {
                dvApplicantNotes.Visible = false;
                spnCommentsHistory.InnerText = "Comments:";
                dvCombined.Visible = false;
                ucVerificationDetailsDocumentConrol.RebindAfterSave(isReadOnlyControlsAfterSave);
                chkDeleteItem.Visible = false;
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
            //UAT-2079:Do not trigger UI rules on verification details screen when only deleting an item's data
            Boolean isDataChanged = false;
            lstApplicantComplianceAttributeData = new List<ApplicantComplianceAttributeData>();

            for (int i = 0; i < rpAttributes.Items.Count; i++)
            {
                String _attributeValue = String.Empty;
                String _attrbuteDataTypeCode = ((WclTextBox)rpAttributes.Items[i].FindControl("txtDataType")).Text;
                //UAT-2079:Do not trigger UI rules on verification details screen when only deleting an item's data
                String existingAttributeValue = ((HiddenField)rpAttributes.Items[i].FindControl("hdnAttributeExistingData")).Value;

                if (_attrbuteDataTypeCode.ToLower() != ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower() && _attrbuteDataTypeCode.ToLower() != ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower() && _attrbuteDataTypeCode.ToLower() != ComplianceAttributeDatatypes.Signature.GetStringValue().ToLower())
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
                    //else if (_attrbuteDataTypeCode == ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower().Trim())
                    //{
                    //    _attributeValue = (rpAttributes.Items[i].FindControl("btnPreviewDoc") as LinkButton).Text.Trim();
                    //}
                    lstApplicantComplianceAttributeData.Add(new ApplicantComplianceAttributeData
                    {
                        AttributeValue = _attributeValue,
                        ApplicantComplianceAttributeID = _attributeDataId,
                        ComplianceAttributeID = _attributeId,
                        ApplicantComplianceItemID = CurrentViewContext.ApplicantItemDataId, // This can be ZERO as Incomplete items can now be added from Verification Details Screen
                        ComplianceItemId = Convert.ToInt32(hdfComplianceItemId.Value),
                    });
                    //UAT-3024:As an admin, I should be able to delete an item's data in the three-panel screen regardless of if UI rules are violated (this should only work if there are no other data updates on the save all changes click)
                    if (_attrbuteDataTypeCode == ComplianceAttributeDatatypes.Date.GetStringValue().ToLower().Trim())
                    {
                        _attributeValue = !String.IsNullOrEmpty(_attributeValue) ? Convert.ToDateTime(_attributeValue).ToString() : _attributeValue;
                        existingAttributeValue = !String.IsNullOrEmpty(existingAttributeValue) ? Convert.ToDateTime(existingAttributeValue).ToString() : existingAttributeValue;
                    }
                    //UAT-2079:Do not trigger UI rules on verification details screen when only deleting an item's data
                    if (!isDataChanged && (String.Compare(_attributeValue, existingAttributeValue, true) != AppConsts.NONE
                        || String.Compare(CurrentItemStatus, rbtnListAction.SelectedValue, true) != AppConsts.NONE))
                    {
                        isDataChanged = true;
                    }
                }
            }
            // DO NOT VALIDATE IN CASE STATUS IS INCOMPLETE
            //UAT-2525
            if (rbtnListAction.SelectedValue == ApplicantItemComplianceStatus.Incomplete.GetStringValue())
            {
                isDataChanged = false;
            }
            IsDataChanged = isDataChanged;
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
                    if (chkDeleteItem.Checked == true)
                    {
                        _presenter.DeleteApplicantItemAttrData();
                        ucVerificationDetailsDocumentConrol.UpdateNewDocumentList();

                        return String.Empty;
                    }
                    else
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
                            CurrentViewContext.Comments = "[" + CurrentViewContext.CurrentLoggedInUserName_Global + " " + DateTime.Now.Date.ToShortDateString() + "] " + txtAdminNote.Text + Environment.NewLine;
                            CurrentViewContext.StatusComments = Environment.NewLine + "[" + CurrentViewContext.LoggedInUserInitials_Global + " " + DateTime.Now.Date.ToShortDateString() + "] " + txtAdminNote.Text;
                            CurrentViewContext.VerificationCommentsWithInitials = "[" + CurrentViewContext.LoggedInUserInitials_Global + "] " + txtAdminNote.Text + Environment.NewLine;  //UAT 2807
                        }
                        else
                        {
                            CurrentViewContext.Comments = String.Empty;
                            CurrentViewContext.StatusComments = String.Empty;
                            CurrentViewContext.VerificationCommentsWithInitials = String.Empty; //UAT 2807
                        }

                        _applicantItemData = _presenter.SaveApplicantData(this.CurrentTenantTypeCode, recordActionType);
                        return String.Empty;
                    }
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

        /// <summary>
        /// Is chkDeleteItem visible to delete items
        /// </summary>
        private void IsDeleteApplicable(Boolean isDeleteApplicable = true, Int32 complianceItemId = 0)
        {
            if (isDeleteApplicable)
            {
                //if TenantType is Compliance_Reviewer i.e. Third Party
                //if (CurrentViewContext.Tenant.lkpTenantType.TenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue())
                if (CurrentTenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue())
                {
                    chkDeleteItem.Visible = false;
                    return;
                }

                ////UAT-3083
                if (CurrentViewContext.VerificationData[0].IsPaymentTypeItem.Value)
                {
                    if (!CurrentViewContext.VerificationData[0].ItemComplianceStatusCode.IsNullOrEmpty() && CurrentViewContext.VerificationData[0].ItemComplianceStatusCode != ApplicantItemComplianceStatus.Incomplete.GetStringValue())
                    {
                        chkDeleteItem.Visible = true;
                    }
                    else
                    {
                        chkDeleteItem.Visible = false;
                    }
                }
                else
                {
                    //if item is not editable Or Item is incomplete with no data in DB i.e. ApplicantItemDataId == 0
                    if (_editableCount == 0 || CurrentViewContext.ApplicantItemDataId == 0)
                    {
                        chkDeleteItem.Visible = false;
                    }
                    else
                    {
                        chkDeleteItem.Visible = true;
                    }
                }
            }
            else
            {
                if (complianceItemId == this.ComplianceItemId)
                    chkDeleteItem.Visible = true;
            }
        }

        #region UAT-3083
        private Boolean IsPaymentItemEditable()
        {
            Boolean isEditable = false;
            if (_presenter.IsDefaultTenant)
            {
                if (this.lstEditableByData.Where(editableBy => editableBy.EditableByCode == LkpEditableBy.Admin
                                                 && editableBy.ComplianceItemId == CurrentViewContext.ComplianceItemId).Any())
                    isEditable = true;
            }
            else if
              (!_presenter.IsDefaultTenant && this.CurrentTenantTypeCode == TenantType.Institution.GetStringValue())
            {
                if (this.lstEditableByData
                    .Where(editableBy => editableBy.EditableByCode == LkpEditableBy.InstitutionAdmin
                          && editableBy.ComplianceItemId == CurrentViewContext.ComplianceItemId).Any())
                    isEditable = true;
            }
            return isEditable;
        }
        #endregion

        #region UAT-2768:
        /// <summary>
        /// Method to Assign documents to map with the item.
        /// </summary>
        /// <param name="isUpdateSessionList"></param>
        /// <returns></returns>
        public Dictionary<String, String> AssignDocument(Boolean isUpdateSessionList, Boolean needToFireUIRule, Boolean isUpdateOnlySessionList, Boolean isNotApprovedStatus = false)
        {
            return ucVerificationDetailsDocumentConrol.AssignDocument(isUpdateSessionList, needToFireUIRule, isUpdateOnlySessionList, isNotApprovedStatus);
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

        public Boolean IsRestrictedFileExist()
        {
            HiddenField hdnIsRestrictedFileTypeChecked = ucVerificationDetailsDocumentConrol.FindControl("hdnIsRestrictedFileTypeChecked") as HiddenField;
            if (!hdnIsRestrictedFileTypeChecked.Value.IsNullOrEmpty())
                return Convert.ToBoolean(hdnIsRestrictedFileTypeChecked.Value);
            else
                return false;
        }


        public List<Tuple<int, string, Boolean>> IsRestrictionDocChecked(Boolean isDeleted, Int32 complianceItemId)
        {
            Repeater rptrAllDocuments = ucVerificationDetailsDocumentConrol.FindControl("rptrAllDocuments") as Repeater;

            var tupleList = new List<Tuple<int, string, Boolean>>();

            foreach (RepeaterItem item in rptrAllDocuments.Items)
            {
                CheckBox chkIsMapped = (CheckBox)item.FindControl("chkIsMapped");
                var hdnDocumentId = (HiddenField)item.FindControl("hdnDocumentId");

                if (chkIsMapped.Checked)
                {
                    Label lblErrorMessage = (Label)item.FindControl("lblErrorMessage");
                    if (!lblErrorMessage.Text.IsNullOrEmpty() && !isDeleted)
                    {
                        tupleList.Add(Tuple.Create(complianceItemId, lblErrorMessage.Text, isDeleted));
                    }
                }
            }

            return tupleList;
        }
    }
}

