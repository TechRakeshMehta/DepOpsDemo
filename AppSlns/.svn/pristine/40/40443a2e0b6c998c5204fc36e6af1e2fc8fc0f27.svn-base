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

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ReconciliationItemDataReviewer : BaseUserControl, IReconciliationItemDataReviewerView
    {
        #region Variables

        #region Private Variables

        private ReconciliationItemDataReviewerPrsenter _presenter = new ReconciliationItemDataReviewerPrsenter();
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


        public ReconciliationItemDataReviewerPrsenter Presenter
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

        public IReconciliationItemDataReviewerView CurrentViewContext
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

        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Presenter.OnViewLoaded();
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
            }


            //showChkBox
            AddMethodToDelegate();


            hdfRejectionCodeItem.Value = ApplicantItemComplianceStatus.Not_Approved.GetStringValue();

            //Is chkDeleteItem visible to delete items
            IsDeleteApplicable();
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
            //else if (dataTypeCode == ComplianceAttributeDatatypes.View_Document.GetStringValue().ToLower().Trim())
            //{
            //    LinkButton btnPreviewDoc = (LinkButton)e.Item.FindControl("btnPreviewDoc");
            //    if (attributeData.ApplAttributeDataId.HasValue && attributeData.ApplAttributeDataId > AppConsts.NONE)
            //    {
            //        btnPreviewDoc.Text = attributeData.AttributeValue;
            //        btnPreviewDoc.Visible = true;

            //        if (this.IsRebindAfterSave)
            //            btnPreviewDoc.Enabled = (_isAttributeEditable && !this.IsReadOnlyAfterSave) ? true : false;
            //        else
            //            btnPreviewDoc.Enabled = _isAttributeEditable ? true : false;

            //        ViewDocumentDetailsContract docContract = Presenter.GetViewDocumentDetailContract(attributeData);

            //        Dictionary<String, String> requestSingleDocViewerArgs = new Dictionary<String, String>();
            //        requestSingleDocViewerArgs = new Dictionary<String, String>
            //                                                     { 
            //                                                        {"OrganizationUserId", AppConsts.ZERO },
            //                                                        {"SelectedTenantId", CurrentViewContext.SelectedTenantId_Global.ToString()},
            //                                                        {"SelectedCatUnifiedStartPageID",AppConsts.ZERO},
            //                                                        {"DocumentId","0"},
            //                                                        {"IsRequestAuth",Convert.ToString(AppConsts.TRUE)},
            //                                                        {"DocumentViewType",UtilityFeatures.View_Document.GetStringValue()},
            //                                                        {"DocumentPath",docContract.DocumentPath}
            //                                                     };
            //        btnPreviewDoc.Attributes.Add("onclick", "return false;");
            //        String _redirectUrl = String.Format(@"/ComplianceOperations/UnifiedPdfDocViewer.aspx?args={0}", requestSingleDocViewerArgs.ToEncryptedQueryString());
            //        btnPreviewDoc.OnClientClick = "GetSingleDocument_ViewDoc('" + _redirectUrl + "');";
            //    }}
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

                #region UAT 725: Add submission time and date to verification details screen for each item

                lblSubmissionDate.Text = Convert.ToString(CurrentViewContext.VerificationData[0].SubmissionDate);

                if (lblSubmissionDate.Text != "") //UAT-3371
                {
                    lblSubmissionDate.Text +="(MT)";
                }

                #endregion

                #region UAT:719 Check Exceptions turned off for a Category/Item
                Boolean isExceptionAllowed = Presenter.IsAllowExceptionOnCategory();
                if (isExceptionAllowed)
                {
                    imageExceptionOff.Visible = true;
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


                litApplicantNotes.Text = (String.IsNullOrEmpty(CurrentViewContext.VerificationData[0].ApplicantItemNotes) ?
                    " <span class='no-data'>No Data</span>"
                    : CurrentViewContext.VerificationData[0].ApplicantItemNotes);

                //hdnUrl.Value = sampleDocFormURL;
                CurrentViewContext.ItemComplianceStatusCode = CurrentViewContext.VerificationData[0].ItemComplianceStatusCode;
                CurrentViewContext.PackageId = CurrentViewContext.SelectedCompliancePackageId_Global;
                txtVerificationComments.Text = CurrentViewContext.VerificationData[0].VerificationComments;

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

                string sampleDocFormURL = VerificationData[0].SampleDocFormURL;
                string sampleDocFormDisplayUrlLabel = VerificationData[0].SampleDocFormDisplayURLLabel;//"..\\InstitutionImages\\Libertyuniversity\\Jellyfish.jpg";
                string SampleDocLink;
                //Only display hyperlink if sampleDocFromUrl available in VerificationData
                if (!sampleDocFormURL.IsNullOrEmpty())
                {
                    SampleDocLink = "<br /><a href=\"" + sampleDocFormURL + "\" onclick=\"\" target=\"_blank\");'>" +
                       (sampleDocFormDisplayUrlLabel.IsNullOrEmpty()? "View Sample Document" : "View "+ sampleDocFormDisplayUrlLabel) + "</a>";
                    //SampleDocLink = "<br /><a href='#' onclick='OpenSampleDocWindow(\"" + VerificationData[0].ItemName + "\");'>View Sample Doc</a>";
                    //ucExplanationDescription.SampleDocUrl = sampleDocFormURL;
                }
                else
                    SampleDocLink = String.Empty;

                ucExplanationDescription.ApplicantExplanation = CurrentViewContext.VerificationData[0].ItemExpNotes;
                ucExplanationDescription.AdminExplanation = CurrentViewContext.VerificationData[0].ItemDescription + @SampleDocLink;

                rpAttributes.DataSource = lstAttributes;
                rpAttributes.DataBind();

                hdfItemStatus.Value = VerificationData[0].ItemComplianceStatusCode;
                hdfComplianceItemId.Value = Convert.ToString(VerificationData[0].ComplianceItemId);

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
            }
        }

        /// <summary>
        /// Display the Radio buttons for possible Admin actions
        /// </summary>
        private void DisplayActions()
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
                            CurrentViewContext.Comments = Environment.NewLine + "[" + CurrentViewContext.CurrentLoggedInUserName_Global + " " + DateTime.Now.Date.ToShortDateString() + "] " + txtAdminNote.Text;
                            CurrentViewContext.StatusComments = Environment.NewLine + "[" + CurrentViewContext.LoggedInUserInitials_Global + " " + DateTime.Now.Date.ToShortDateString() + "] " + txtAdminNote.Text;
                        }
                        else
                        {
                            CurrentViewContext.Comments = String.Empty;
                            CurrentViewContext.StatusComments = String.Empty;
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
            else
            {
                if (complianceItemId == this.ComplianceItemId)
                    chkDeleteItem.Visible = true;
            }
        }

        #endregion

    }
}

