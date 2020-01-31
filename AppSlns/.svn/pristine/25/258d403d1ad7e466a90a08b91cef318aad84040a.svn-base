using System;
using System.Linq;
using INTSOF.Utils;
using CoreWeb.Shell;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Web.Configuration;


namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public partial class RequirementAttributeControl : BaseUserControl, IRequirementAttributeControlView
    {
        #region Variables

        public event EventHandler IsFileUploadApplicable;
        public event EventHandler IsViewDocumnetApplicable;
        public event EventHandler IsViewVideoApplicable;

        #region Private Variables

        private RequirementAttributeControlPresenter _presenter = new RequirementAttributeControlPresenter();
        private IRequirementAttributeControlView _currentViewContext;

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        public RequirementAttributeControlPresenter Presenter
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

        public RequirementFieldContract RequirementFieldContract
        {
            get;
            set;
        }

        public IRequirementAttributeControlView CurrentViewContext
        {
            get
            {
                if (_currentViewContext.IsNull())
                    _currentViewContext = new RequirementAttributeControl();
                return _currentViewContext;
            }
        }

        public Int32 ItemId
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public List<ApplicantDocumentContract> ApplicantDocuments
        {
            get;
            set;
        }

        public Int32 TenantId
        {
            get;
            set;
        }

        public Int32 ApplicantComplianceAttributeID
        {
            get;
            set;
        }

        public ApplicantRequirementFieldDataContract ApplicantFieldData
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
            pnlControls.Controls.Clear();

            if (CurrentViewContext.RequirementFieldContract.IsNull())
            {
                CurrentViewContext.RequirementFieldContract = RequirementFieldContract;
                CurrentViewContext.ApplicantFieldData = ApplicantFieldData;

                CreateDynamicControl(CurrentViewContext.RequirementFieldContract);
                litLabel.Text = CurrentViewContext.RequirementFieldContract.RequirementFieldName.HtmlEncode();
                litLabel.Text = "<span class='cptn'>" + litLabel.Text + "</span>";
                //WclToolTip tltpMain = new WclToolTip();
                //dvMain.ID = dvMain.ID + CurrentViewContext.RequirementFieldContract.RequirementItemFieldID;
                //tltpMain.ID = "tltp_" + CurrentViewContext.RequirementFieldContract.RequirementItemFieldID;
                //tltpMain.Text = CurrentViewContext.RequirementFieldContract.RequirementFieldDescription;
                //tltpMain.Visible = tltpMain.Text.Trim().IsNullOrEmpty() ? false : false;
                //tltpMain.TargetControlID = dvMain.ID;
                //tltpMain.RelativeTo = Telerik.Web.UI.ToolTipRelativeDisplay.Mouse;
                //tltpMain.Position = Telerik.Web.UI.ToolTipPosition.TopRight;
                //tltpMain.Width = Unit.Pixel(300);
                //pnlControls.Controls.Add(tltpMain);
            }
        }

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// Generate the dynamic attribute
        /// </summary>
        /// <param name="clientItemAttribute">Attribute for which control is to be generated</param>
        private void CreateDynamicControl(RequirementFieldContract requirementField)
        {
            dvAttributeControl.Attributes.CssStyle.Value = null; //Fixed UI Issue When item have only signature control After changing to another item then UI gets distorted.
            String dataTypeCode = requirementField.RequirementFieldData.RequirementFieldDataTypeCode.ToLower().Trim();
            ItemId = requirementField.RequirementItemID;
            String validationMsg = CurrentViewContext.RequirementFieldContract.RequirementFieldName + " is required.";
            Boolean isFieldRequired = false;
            Int32 reqObjectTreeId = AppConsts.NONE;
            String attObjectTypeCode = LCObjectType.ComplianceATR.GetStringValue();
            String objectAttributeTypeCode = ObjectAttribute.REQUIRED.GetStringValue();
            Boolean isEnabled = requirementField.AttributeTypeCode.Equals(AppConsts.REQUIREMENT_FIELD_ATTRIBUTE_TYPE_CALCULATED_CODE) ? false : true;

            if (LstRequirementObjTreeProperty.IsNotNull() && isEnabled)
            {
                var fieldProperty = LstRequirementObjTreeProperty.FirstOrDefault(cond => cond.ObjectID == requirementField.RequirementFieldID
                                                                                && cond.ObjectTypeCode == attObjectTypeCode
                                                                                && cond.ObjectAttributeTypeCode == objectAttributeTypeCode);

                if (fieldProperty.IsNotNull())
                {
                    reqObjectTreeId = fieldProperty.RequirementObjectTreeID;
                    if (fieldProperty.ObjectAttributeValue.Trim().ToLower() == "true")
                    {
                        isFieldRequired = true;
                    }
                }
            }

            if (dataTypeCode == RequirementFieldDataType.DATE.GetStringValue().ToLower().Trim())
            {
                WclDatePicker dPicker = new WclDatePicker();
                dPicker.ToolTip = requirementField.RequirementFieldDescription;
                dPicker.DateInput.DateFormat = "MM-dd-yyyy";
                dPicker.ID = "dp_" + requirementField.RequirementItemFieldID;
                //dPicker.Attributes.Add("onclick", "HideInstructionMessageDiv('" + requirementField.RequirementItemFieldID + "');");
                //dPicker.ClientEvents.OnPopupClosing = "HideInstructionMessageDiv";
                dPicker.MinDate = Convert.ToDateTime("01-01-1900");
                dPicker.DateInput.EmptyMessage = isEnabled ? "Select a date" : string.Empty;
                dPicker.Enabled = isEnabled;

                HiddenField hdnCIA_ID = new HiddenField();
                hdnCIA_ID.ID = "hdnCIA_ID";
                hdnCIA_ID.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                hdnCIA_ID.Value = Convert.ToString(requirementField.RequirementItemFieldID);

                HiddenField hdnAlreadyEnteredDate = new HiddenField();
                hdnAlreadyEnteredDate.ID = String.Format("hdnAlreadyEnteredDate_{0}", requirementField.RequirementItemFieldID);
                hdnAlreadyEnteredDate.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                if (CurrentViewContext.ApplicantFieldData.IsNotNull() && !CurrentViewContext.ApplicantFieldData.FieldValue.IsNullOrEmpty())
                    hdnAlreadyEnteredDate.Value = CurrentViewContext.ApplicantFieldData.FieldValue;
                pnlControls.Controls.Add(hdnAlreadyEnteredDate);


                pnlControls.Controls.Add(hdnCIA_ID);
                pnlControls.Controls.Add(dPicker);

                if (isFieldRequired)
                {
                    ApplyRequiredFieldValidator(requirementField.RequirementItemFieldID, dPicker.ID, validationMsg, dataTypeCode);
                }
                // UAT-4380
                if (requirementField.IsEditableByApplicant.HasValue && requirementField.IsEditableByApplicant.Value.Equals(false))
                {
                    dPicker.DateInput.EmptyMessage = String.Empty;
                    dPicker.Enabled = requirementField.IsEditableByApplicant.Value;
                }
                else
                {
                    dPicker.DateInput.EmptyMessage = "Select a date";
                    dPicker.Enabled = requirementField.IsEditableByApplicant.Value;
                }
                //END UAT-4380
                #region BIND DATA IF IN EDIT MODE

                try
                {
                    if (CurrentViewContext.ApplicantFieldData.IsNotNull() && !CurrentViewContext.ApplicantFieldData.FieldValue.IsNullOrEmpty())
                        dPicker.SelectedDate = Convert.ToDateTime(CurrentViewContext.ApplicantFieldData.FieldValue);
                    else
                        dPicker.Clear();
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }

                #endregion
            }
            else if (dataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue().ToLower().Trim())
            {
                WclComboBox cmbOptions = new WclComboBox();
                cmbOptions.ID = "cmbOptions_" + requirementField.RequirementItemFieldID;
                List<RequirementFieldOptionsData> lst = requirementField.RequirementFieldData.LstRequirementFieldOptions.ToList();

                foreach (var fieldOption in lst)
                {
                    cmbOptions.Items.Add(new RadComboBoxItem(fieldOption.OptionText, fieldOption.OptionValue));
                }
                cmbOptions.Items.Insert(0, new RadComboBoxItem(AppConsts.COMBOBOX_ITEM_SELECT, AppConsts.ZERO));
                cmbOptions.ToolTip = requirementField.RequirementFieldDescription;
                pnlControls.Controls.Add(cmbOptions);
                if (isFieldRequired)
                {
                    ApplyRequiredFieldValidator(requirementField.RequirementItemFieldID, cmbOptions.ID, validationMsg, dataTypeCode);
                }
                cmbOptions.Enabled = isEnabled;
                //END UAT-4380
                if (requirementField.IsEditableByApplicant.HasValue)
                    cmbOptions.Enabled = requirementField.IsEditableByApplicant.Value;
                //END UAT-4380
                #region BIND DATA IF IN EDIT MODE

                try
                {
                    if (CurrentViewContext.ApplicantFieldData.IsNotNull())
                        cmbOptions.SelectedValue = CurrentViewContext.ApplicantFieldData.FieldValue;
                    else if (!cmbOptions.SelectedValue.IsNotNull() && !(int.Parse(cmbOptions.SelectedValue) > AppConsts.NONE))
                        cmbOptions.SelectedValue = AppConsts.ZERO;
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }

                #endregion
            }
            else if (dataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue().ToLower().Trim())
            {
                WclComboBox cmbFileUpload = new WclComboBox();
                cmbFileUpload.ID = "cmbFileUpload_" + requirementField.RequirementItemFieldID;
                //Start UAT-4900
                cmbFileUpload.CssClass = "cmbFileUpload";
                //End UAT-4900
                cmbFileUpload.ToolTip = requirementField.RequirementFieldDescription;
                cmbFileUpload.CheckBoxes = true;
                cmbFileUpload.EmptyMessage = "-- SELECT --";
                Presenter.GetDocuments();
                //Start UAT-4900
                List<Int32> lstDisableViewDocumentID = new List<Int32>();
                //End UAT-4900
                List<Int32> lstMappedDocuments = null;
                #region BIND DATA IF IN EDIT MODE
                try
                {
                    if (CurrentViewContext.ApplicantFieldData.IsNotNull())
                    {
                        lstMappedDocuments = new List<Int32>();

                        //Start UAT-4900
                        if (!CurrentViewContext.ApplicantFieldData.LstApplicantFieldDocumentMapping.IsNullOrEmpty())
                        {
                            lstMappedDocuments = CurrentViewContext.ApplicantFieldData.LstApplicantFieldDocumentMapping.Select(doc => doc.ApplicantDocumentId).ToList();

                            lstDisableViewDocumentID = CurrentViewContext.ApplicantFieldData.LstApplicantFieldDocumentMapping
                                                          .Where(c => c.IsDisabled)
                                                          .Select(sel => sel.ApplicantDocumentId).ToList();
                        }
                        //End UAT-4900
                    }
                    foreach (var document in CurrentViewContext.ApplicantDocuments)
                    {
                        Boolean _isMapped = false;
                        //Start UAT-4900
                        Boolean _isEnabled = true;
                        //End UAT-4900
                        if (CurrentViewContext.ApplicantFieldData.IsNotNull())
                            if (lstMappedDocuments.IsNotNull() && lstMappedDocuments.Count > 0)
                            {
                                _isMapped = lstMappedDocuments.Where(docId => docId == document.ApplicantDocumentId).Any();
                            }

                        //Start UAT-4900
                        if (!lstDisableViewDocumentID.IsNullOrEmpty() && lstDisableViewDocumentID.Count > AppConsts.NONE
                                && lstDisableViewDocumentID.Contains(document.ApplicantDocumentId))
                        {
                            _isEnabled = false;
                        }
                        //End UAT-4900

                        RadComboBoxItem item = new RadComboBoxItem { Text = document.FileName, Value = Convert.ToString(document.ApplicantDocumentId), Checked = _isMapped };
                        item.Attributes["desc"] = document.Description;

                        //Start UAT-4900
                        if (!_isEnabled)
                        {
                            item.Enabled = false;
                        }
                        //End UAT-4900

                        cmbFileUpload.Items.Add(item);
                    }
                    IsFileUploadApplicable(this, new EventArgs());

                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }
                #endregion

                //Attaching client side event handler for combobox upload event
                //This handler should be decalard in global scope of the script
                cmbFileUpload.OnClientLoad = "efn_atrFileUpdOnLoad";
                cmbFileUpload.Enabled = isEnabled;
                // UAT-4380
                if (requirementField.IsEditableByApplicant.HasValue)
                    cmbFileUpload.Enabled = requirementField.IsEditableByApplicant.Value;
                //END UAT-4380
                pnlControls.Controls.Add(cmbFileUpload);

                if (isFieldRequired)
                {
                    ApplyCustomFieldValidator(requirementField.RequirementItemFieldID, cmbFileUpload.ID, validationMsg, dataTypeCode);
                }
            }
            else if (dataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue().ToLower().Trim())
            {

                HiddenField hdnRIF_ID = new HiddenField();
                hdnRIF_ID.ID = "hdnRIF_ID";
                hdnRIF_ID.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                hdnRIF_ID.Value = Convert.ToString(requirementField.RequirementItemFieldID);
                pnlControls.Controls.Add(hdnRIF_ID);

                HiddenField hdfDocFileName = AddHiddenField(requirementField.RequirementItemFieldID, "hdfDocFileName");
                hdfDocFileName.Value = String.Empty;
                pnlControls.Controls.Add(hdfDocFileName);

                Int32 applicantDocumentId = AppConsts.NONE;
                if (CurrentViewContext.ApplicantFieldData.IsNotNull() && !CurrentViewContext.ApplicantFieldData.FieldValue.IsNullOrEmpty()
                    && CurrentViewContext.ApplicantFieldData.LstApplicantFieldDocumentMapping.IsNotNull())
                {
                    var signedAppDoc = CurrentViewContext.ApplicantFieldData.LstApplicantFieldDocumentMapping.FirstOrDefault(x => x.DocumentType == DocumentType.REQUIREMENT_FIELD_VIEW_DOCUMENT.GetStringValue());
                    applicantDocumentId = signedAppDoc.IsNotNull() ? signedAppDoc.ApplicantDocumentId : AppConsts.NONE;
                }
                LinkButton linkbutton = new LinkButton();
                linkbutton.ToolTip = requirementField.RequirementFieldDescription;
                linkbutton.ID = "lnkBtnDoc_" + requirementField.RequirementItemFieldID;
                if (requirementField.RequirementFieldData.FieldViewDocumentData.IsNotNull())
                {
                    linkbutton.OnClientClick = "ViewDocument('" + requirementField.RequirementFieldData.FieldViewDocumentData.ClientSystemDocumentID +
                                                             "','" + requirementField.RequirementFieldID + "','" + reqObjectTreeId +
                                                             "','" + requirementField.RequirementItemFieldID + "','" + applicantDocumentId + "');";
                }
                linkbutton.Attributes.Add("onclick", "return false;");
                linkbutton.Attributes.Add("FieldType", dataTypeCode);
                linkbutton.Attributes.Add("ReqItemFieldId", Convert.ToString(requirementField.RequirementItemFieldID));
                if (requirementField.RequirementFieldData.IsNotNull() && requirementField.RequirementFieldData.FieldViewDocumentData.IsNotNull())
                {
                    linkbutton.Text = requirementField.RequirementFieldData.FieldViewDocumentData.DocumentFileName;
                }
                linkbutton.Enabled = isEnabled;

                //END UAT-4380
                if (requirementField.IsEditableByApplicant.HasValue && requirementField.IsEditableByApplicant.Value.Equals(false))
                {
                    linkbutton.Enabled = requirementField.IsEditableByApplicant.Value;
                    linkbutton.OnClientClick = "";
                }
                else
                {
                    linkbutton.Enabled = requirementField.IsEditableByApplicant.Value;
                    linkbutton.OnClientClick = "ViewDocument('" + requirementField.RequirementFieldData.FieldViewDocumentData.ClientSystemDocumentID +
                                                             "','" + requirementField.RequirementFieldID + "','" + reqObjectTreeId +
                                                             "','" + requirementField.RequirementItemFieldID + "','" + applicantDocumentId + "');";
                }
                //END UAT-4380
                pnlControls.Controls.Add(linkbutton);

                HiddenField hdfViewedDocPath = AddHiddenField(requirementField.RequirementItemFieldID, "hdfViewedDocPath");
                hdfViewedDocPath.Value = String.Empty;
                pnlControls.Controls.Add(hdfViewedDocPath);

                HiddenField hdfIsViewDocumentRequired = AddHiddenField(requirementField.RequirementItemFieldID, "hdfIsViewDocumentRequired");
                if (isFieldRequired)
                {
                    ApplyCustomFieldValidator(requirementField.RequirementItemFieldID, linkbutton.ID, validationMsg, dataTypeCode);
                    hdfIsViewDocumentRequired.Value = "1";//"1" for true and "2" for false
                                                          //IsViewDocumnetApplicable(this, new EventArgs());
                }
                pnlControls.Controls.Add(hdfIsViewDocumentRequired);

                HiddenField hdfIsDocumentViewed = AddHiddenField(requirementField.RequirementItemFieldID, "hdfIsDocumentViewed");
                if (CurrentViewContext.ApplicantFieldData.IsNotNull() && !CurrentViewContext.ApplicantFieldData.FieldValue.IsNullOrEmpty())
                {
                    hdfIsDocumentViewed.Value = CurrentViewContext.ApplicantFieldData.FieldValue;
                }
                pnlControls.Controls.Add(hdfIsDocumentViewed);
            }
            else if (dataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue().ToLower().Trim())
            {
                LinkButton linkbutton = new LinkButton();
                linkbutton.ToolTip = requirementField.RequirementFieldDescription;
                linkbutton.ID = "lnkBtnVideo_" + requirementField.RequirementItemFieldID;
                if (requirementField.RequirementFieldData.VideoFieldData.IsNotNull())
                {
                    linkbutton.OnClientClick = "ViewVideo('" + requirementField.RequirementFieldData.VideoFieldData.RequirementFieldVideoID + "','" + reqObjectTreeId +
                                                           "','" + requirementField.RequirementItemFieldID + "');";
                }
                linkbutton.Attributes.Add("onclick", "return false;");
                linkbutton.Attributes.Add("FieldType", dataTypeCode);
                linkbutton.Attributes.Add("ReqItemFieldId", Convert.ToString(requirementField.RequirementItemFieldID));
                //Applied the class for giving it a hyperlink look                                
                linkbutton.CssClass = "lnkButtonAsHyperlink";
                if (requirementField.RequirementFieldData.IsNotNull() && requirementField.RequirementFieldData.VideoFieldData.IsNotNull())
                {
                    linkbutton.Text = requirementField.RequirementFieldData.VideoFieldData.VideoName;
                }

                linkbutton.Enabled = isEnabled;

                HiddenField hdnRIF_ID = new HiddenField();
                hdnRIF_ID.ID = "hdnRIF_ID";
                hdnRIF_ID.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                hdnRIF_ID.Value = Convert.ToString(requirementField.RequirementItemFieldID);

                HiddenField hdfIsVideoViewed = AddHiddenField(requirementField.RequirementItemFieldID, "hdfIsVideoViewed");
                HiddenField hdfVideoViewedTime = AddHiddenField(requirementField.RequirementItemFieldID, "hdfVideoViewedTime");
                HiddenField hdfIsViewVideoRequired = AddHiddenField(requirementField.RequirementItemFieldID, "hdfIsViewVideoRequired");

                #region Add Hidden Fields to panel
                pnlControls.Controls.Add(hdfIsVideoViewed);
                pnlControls.Controls.Add(hdfVideoViewedTime);
                pnlControls.Controls.Add(hdfIsViewVideoRequired);
                #endregion
                pnlControls.Controls.Add(hdnRIF_ID);
                pnlControls.Controls.Add(linkbutton);

                if (isFieldRequired)
                {
                    ApplyCustomFieldValidator(requirementField.RequirementItemFieldID, linkbutton.ID, validationMsg, dataTypeCode);
                    hdfIsViewVideoRequired.Value = "1";//1 for true and 0 for false
                                                       //IsViewVideoApplicable(this, new EventArgs());
                }
                if (CurrentViewContext.ApplicantFieldData.IsNotNull() && !CurrentViewContext.ApplicantFieldData.FieldValue.IsNullOrEmpty())
                {
                    linkbutton.CssClass = "";
                    hdfIsVideoViewed.Value = "1";
                    hdfVideoViewedTime.Value = CurrentViewContext.ApplicantFieldData.FieldValue;
                }
                //UAT-4380
                if (requirementField.IsEditableByApplicant.HasValue && requirementField.IsEditableByApplicant.Value.Equals(false))
                {
                    linkbutton.CssClass = "";
                    linkbutton.Enabled = requirementField.IsEditableByApplicant.Value;
                    linkbutton.OnClientClick = "";
                }
                else
                {
                    linkbutton.CssClass = "lnkButtonAsHyperlink";
                    linkbutton.Enabled = requirementField.IsEditableByApplicant.Value;
                    linkbutton.OnClientClick = "ViewVideo('" + requirementField.RequirementFieldData.VideoFieldData.RequirementFieldVideoID + "','" + reqObjectTreeId +
                                                           "','" + requirementField.RequirementItemFieldID + "');";
                }
                //END UAT-4380
            }

            else if (dataTypeCode == RequirementFieldDataType.TEXT.GetStringValue().ToLower().Trim())
            {
                WclTextBox txtTextType = new WclTextBox();
                txtTextType.ID = "txtTextType_" + requirementField.RequirementItemFieldID;
                if (requirementField.RequirementFieldMaxLength.IsNull() || requirementField.RequirementFieldMaxLength <= 0) //Max length setting
                    txtTextType.MaxLength = 50;
                else
                    txtTextType.MaxLength = Convert.ToInt32(requirementField.RequirementFieldMaxLength);

                txtTextType.Enabled = isEnabled;
                txtTextType.ToolTip = requirementField.RequirementFieldDescription;
                pnlControls.Controls.Add(txtTextType);

                if (isFieldRequired)
                {
                    ApplyRequiredFieldValidator(requirementField.RequirementItemFieldID, txtTextType.ID, validationMsg, dataTypeCode);
                }
                // UAT-4380
                if (requirementField.IsEditableByApplicant.HasValue)
                    txtTextType.Enabled = requirementField.IsEditableByApplicant.Value;
                //END UAT-4380
                #region BIND DATA IF IN EDIT MODE

                try
                {
                    if (CurrentViewContext.ApplicantFieldData.IsNotNull())
                    {
                        txtTextType.Text = CurrentViewContext.ApplicantFieldData.FieldValue;
                    }
                    else
                        txtTextType.Text = String.Empty;
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }

                #endregion
            }

            //Signature - Attribute Type
            else if (dataTypeCode == RequirementFieldDataType.SIGNATURE.GetStringValue().ToLower().Trim())
            {
                dvAttributeControl.Attributes.Add("style", "border-right-width: 0px;width:415px !important;");
                HtmlGenericControl mainDiv = new HtmlGenericControl("DIV");
                mainDiv.ID = "dvSignature_" + requirementField.RequirementItemFieldID;
                mainDiv.Attributes.Add("class", "sigPad");
                mainDiv.Attributes.Add("FieldType", dataTypeCode);
                mainDiv.Attributes.Add("ReqItemFieldId", requirementField.RequirementItemFieldID.ToString());

                if (!isEnabled)
                    mainDiv.Style.Add("Disabled", "true");
                // UAT-4380
                if (isEnabled && requirementField.IsEditableByApplicant.HasValue && requirementField.IsEditableByApplicant.Value.Equals(false))
                    mainDiv.Style.Add("Disabled", "true");
                else
                    mainDiv.Style.Remove("Disabled");
                //END UAT-4380
                HtmlGenericControl canvas = new HtmlGenericControl("canvas");
                canvas.ID = "signature";
                canvas.Attributes.Add("class", "pad borderTextArea");
                canvas.Attributes.Add("style", "border: 2px solid green;");

                HtmlInputHidden hdnOutput = new HtmlInputHidden();
                hdnOutput.ID = "hiddenOutput_" + requirementField.RequirementItemFieldID;
                hdnOutput.Attributes.Add("class", "output");
                hdnOutput.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                hdnOutput.EnableViewState = true;

                HtmlGenericControl lnkClear = new HtmlGenericControl("a");
                lnkClear.Attributes.Add("href", "#clear");
                lnkClear.InnerText = "Clear Signature";
                lnkClear.Attributes.Add("class", "clearButton");
                lnkClear.Attributes.Add("style", "font-size: 13px;color: blue;float:right");
                pnlControls.Controls.Add(mainDiv);

                CustomValidator cstValidator = null;

                if (isFieldRequired)
                {
                    cstValidator = ApplyCustomFieldValidator(requirementField.RequirementItemFieldID, hdnOutput.ID, validationMsg, dataTypeCode);
                }
                else
                    cstValidator = ApplyCustomFieldValidator(requirementField.RequirementItemFieldID, hdnOutput.ID, validationMsg, dataTypeCode, true);
                #region BIND DATA IF IN EDIT MODE

                try
                {
                    if (CurrentViewContext.ApplicantFieldData.IsNotNull())
                    {
                        if (!CurrentViewContext.ApplicantFieldData.Signature.IsNullOrEmpty())
                        {
                            HtmlImage img = new HtmlImage();
                            var base64Data = Convert.ToBase64String(CurrentViewContext.ApplicantFieldData.Signature.ToArray());
                            img.Src = "data:image/gif;base64," + base64Data;
                            img.Width = 300;
                            img.Height = 150;
                            mainDiv.Controls.Add(img);

                            canvas.Visible = false;
                            lnkClear.Visible = false;

                            if (!cstValidator.IsNullOrEmpty())
                                cstValidator.Enabled = false;
                        }
                        else
                        {
                            canvas.Visible = true;
                            lnkClear.Visible = true;
                        }
                    }
                    else
                    {
                        canvas.Visible = true;
                        lnkClear.Visible = true;
                    }
                    // UAT-4380
                    if (requirementField.IsEditableByApplicant.HasValue)
                    {
                        canvas.Visible = requirementField.IsEditableByApplicant.Value;
                        lnkClear.Visible = requirementField.IsEditableByApplicant.Value;
                    }
                    //END UAT-4380
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }

                mainDiv.Controls.Add(hdnOutput);
                mainDiv.Controls.Add(canvas);
                mainDiv.Controls.Add(lnkClear);
                #endregion
            }
            if (CurrentViewContext.ApplicantFieldData.IsNotNull())
            {
                hdfApplicantFieldDataId.Value = Convert.ToString(CurrentViewContext.ApplicantFieldData.ApplicantReqFieldDataID);
            }
        }

        private void ApplyRequiredFieldValidator(Int32 fieldId, String controlId, String errorMessage, String dataTypeCode)
        {
            RequiredFieldValidator rfValidator = new RequiredFieldValidator();
            rfValidator.ID = "rf_" + fieldId.ToString();
            rfValidator.Display = ValidatorDisplay.Dynamic;
            rfValidator.ControlToValidate = controlId;
            rfValidator.ErrorMessage = errorMessage;
            rfValidator.CssClass = "errmsg";
            rfValidator.ValidationGroup = "vGroup";
            if (dataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue().ToLower().Trim())
            {
                rfValidator.InitialValue = AppConsts.COMBOBOX_ITEM_SELECT;
            }
            pnlControls.Controls.Add(rfValidator);
        }

        private CustomValidator ApplyCustomFieldValidator(Int32 fieldId, String controlId, String errorMessage, String dataTypeCode, Boolean checkOnlyMinLengthSignature = false)
        {
            CustomValidator cstValidator = new CustomValidator();
            cstValidator.ID = "cst_" + fieldId.ToString();
            cstValidator.Display = ValidatorDisplay.Dynamic;
            cstValidator.ErrorMessage = errorMessage;
            cstValidator.CssClass = "errmsg";
            cstValidator.ValidationGroup = "vGroup";
            cstValidator.EnableClientScript = true;
            cstValidator.ValidateEmptyText = true;
            cstValidator.Attributes.Add("ReqItemFieldId", Convert.ToString(fieldId));
            if (dataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue().ToLower().Trim())
            {
                cstValidator.ControlToValidate = controlId;
                cstValidator.ClientValidationFunction = "ValidateUploadDocument";
                pnlControls.Controls.Add(cstValidator);
            }
            else if (dataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue().ToLower().Trim())
            {
                cstValidator.ClientValidationFunction = "ValidateViewDocument";
                divValidator.Controls.Add(cstValidator);
            }
            else if (dataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue().ToLower().Trim())
            {
                cstValidator.ClientValidationFunction = "ValidateViewVideo";
                divValidator.Controls.Add(cstValidator);
            }
            else if (dataTypeCode == RequirementFieldDataType.SIGNATURE.GetStringValue().ToLower().Trim())
            {
                String ClientValidationFunction = String.Empty;
                if (checkOnlyMinLengthSignature)
                    ClientValidationFunction = "CheckMinLengthSignature";
                else
                    ClientValidationFunction = "ValidateSignature";


                cstValidator.ClientValidationFunction = ClientValidationFunction;
                divValidator.Controls.Add(cstValidator);
            }

            return cstValidator;
        }

        private HiddenField GetHiddenField(String hdfName)
        {
            HiddenField hdf = null;
            Control ctl = this.Parent;
            while (true)
            {
                hdf = (HiddenField)ctl.FindControl(hdfName);
                if (hdf.IsNull())
                {
                    if (ctl.Parent == null)
                        return hdf;
                    ctl = ctl.Parent;
                    continue;
                }
                return hdf;
            }
        }

        private HiddenField AddHiddenField(Int32 reqItemfieldId, String controlId)
        {
            HiddenField hdnField = new HiddenField();
            hdnField.ID = controlId + "_" + reqItemfieldId;
            hdnField.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            hdnField.Value = "0";
            return hdnField;
        }

        #endregion

        #region Public Methods

        #endregion

        #endregion
    }
}

