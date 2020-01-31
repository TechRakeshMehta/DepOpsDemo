using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;
using System.Xml;


namespace CoreWeb.ClinicalRotation.Views
{
    public partial class SharedUserCustomAttributeDisplayControl : BaseUserControl, ISharedUserCustomAttributeDisplayControlView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private SharedUserCustomAttributeDispalyControlPresenter _presenter = new SharedUserCustomAttributeDispalyControlPresenter();

        #endregion

        #endregion

        #region  Properties

        #region Public Properties


        public SharedUserCustomAttributeDispalyControlPresenter Presenter
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

        public CustomAttribteContract TypeCustomtAttribute
        {
            get;
            set;
        }

        /// <summary>
        /// Property to enable disable the controls
        /// </summary>
        public Boolean IsReadOnly { get; set; }

        /// <summary>
        /// Property to display the labels or actual controls
        /// </summary>
        public DisplayMode ControlDisplayMode { get; set; }


        public ISharedUserCustomAttributeDisplayControlView CurrentViewContext
        {
            get { return this; }
        }
        public Int32 SelectedRecordId
        {
            get;
            set;
        }

        public String ValidationGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Receive the attribute values and set to the controls, in Edit profile, in order flow
        /// </summary>
        public List<CustomAttribteContract> AttributeValues
        {
            get;
            set;
        }

        public String previousValues
        {
            get;
            set;
        }

        public Boolean IsSearchTypeControl
        {
            get;
            set;
        }

        public Boolean DoNotShowDefaultValues { get; set; }

        #endregion

        #region Private Properties

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

            if (this.ControlDisplayMode == DisplayMode.Labels)
            {
                CreateLabels();
                pnlLabelsMode.Visible = true;
                pnlControlsMode.Visible = false;
                pnlReadOnlyLabels.Visible = false;
            }
            else if (this.ControlDisplayMode == DisplayMode.Controls)
            {
                CreateDynamicControls();
                pnlLabelsMode.Visible = false;
                pnlControlsMode.Visible = true;
                pnlReadOnlyLabels.Visible = false;
            }
            else if (this.ControlDisplayMode == DisplayMode.ReadOnlyLabels)
            {
                CreateReadOnlyLabels();
                pnlReadOnlyLabels.Visible = true;
                pnlLabelsMode.Visible = false;
                pnlControlsMode.Visible = false;
            }
        }

        #endregion

        #region   Methods

        #region Public Methods

        /// <summary>
        /// UAT-1778: To Reset Custom Attribute
        /// </summary>
        public void ResetCustomAttribute()
        {
            hdfCAVId.Value = lblCAVId.Text;
            hdfIdentity.Value = lblIdentity.Text;
            hdfCAId.Value = lblCAId.Text;

            //Reset all types of server controls
            String dataTypeCode = CurrentViewContext.TypeCustomtAttribute.CustomAttributeDataTypeCode.ToLower().Trim();
            Int32 _customAttrId = CurrentViewContext.TypeCustomtAttribute.CustomAttributeId;
            if (dataTypeCode == CustomAttributeDatatype.Date.GetStringValue().ToLower().Trim())
            {
                var dp = divControlMode.FindServerControlRecursively("dp_" + _customAttrId) as WclDatePicker;
                if (dp.IsNotNull())
                    dp.SelectedDate = null;
            }
            else if (dataTypeCode == CustomAttributeDatatype.Text.GetStringValue().ToLower().Trim())
            {
                var txtTextType = divControlMode.FindServerControlRecursively("txtTextType_" + _customAttrId) as WclTextBox;
                if (txtTextType.IsNotNull())
                    txtTextType.Text = String.Empty;
            }
            else if (dataTypeCode == CustomAttributeDatatype.Numeric.GetStringValue().ToLower().Trim())
            {
                var txtNumericType = divControlMode.FindServerControlRecursively("txtNumericType_" + _customAttrId) as WclNumericTextBox;
                if (txtNumericType.IsNotNull())
                    txtNumericType.Text = String.Empty;
            }
            else if (dataTypeCode == CustomAttributeDatatype.Boolean.GetStringValue().ToLower().Trim())
            {
                var rbtnListType = divControlMode.FindServerControlRecursively("rbtnListType_" + _customAttrId) as RadioButtonList;
                if (rbtnListType.IsNotNull())
                    rbtnListType.SelectedValue = "0";
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generate the dynamic attribute control
        /// </summary>
        private void CreateDynamicControls()
        {
            divControlMode.Controls.Clear();
            String dataTypeCode = CurrentViewContext.TypeCustomtAttribute.CustomAttributeDataTypeCode.ToLower().Trim();
            String _caValue = CurrentViewContext.TypeCustomtAttribute.CustomAttributeValue;
            Int64 _mappingId = CurrentViewContext.TypeCustomtAttribute.CustomAttrMappingId;

            //UAT-1778
            Int32 _customAttrId = CurrentViewContext.TypeCustomtAttribute.CustomAttributeId;
            String value = String.Empty;
            if (this.IsSearchTypeControl)
                _mappingId = _customAttrId;

            if (!this.previousValues.IsNullOrEmpty())
            {
                value = GetValue(_mappingId);
            }

            Boolean _isRequired = Convert.ToBoolean(CurrentViewContext.TypeCustomtAttribute.CustomAttributeIsRequired);

            String _caType = lblLabel.Text = String.IsNullOrEmpty(CurrentViewContext.TypeCustomtAttribute.CustomAttributeLabel)
             ? CurrentViewContext.TypeCustomtAttribute.CustomAttributeName.HtmlEncode() : CurrentViewContext.TypeCustomtAttribute.CustomAttributeLabel.HtmlEncode();

            //UAT-1778
            if (this.IsSearchTypeControl)
                _isRequired = false;

            if (_isRequired)
            {
                required.Visible = true;
            }

            lblIdentity.Text = hdfIdentity.Value = Convert.ToString(CurrentViewContext.TypeCustomtAttribute.CustomAttrMappingId);

            if (AttributeValues.IsNotNull() && AttributeValues.Count > 0)
            {
                CustomAttribteContract typeCustomAttributes = AttributeValues.Where(av => av.CustomAttrMappingId == _mappingId).FirstOrDefault();
                if (typeCustomAttributes.IsNotNull())
                    _caValue = typeCustomAttributes.CustomAttributeValue;
            }
            //UAT-1778
            if (this.IsSearchTypeControl)
                _caValue = value;

            if (dataTypeCode == CustomAttributeDatatype.Date.GetStringValue().ToLower().Trim())
            {
                WclDatePicker dPicker = new WclDatePicker();
                dPicker.DateInput.DateFormat = "MM-dd-yyyy";
                dPicker.ID = "dp_" + _mappingId;
                dPicker.DateInput.EmptyMessage = "Select a date";
                dPicker.MinDate = Convert.ToDateTime("01-01-1900");
                dPicker.Enabled = !this.IsReadOnly;
                dPicker.CssClass = "form-control";
                dPicker.Width = new Unit("100%");
                divControlMode.Controls.Add(dPicker);

                if (_isRequired)
                {
                    ApplyRequiredField(_mappingId, dPicker.ID, _caType);
                }

                try
                {
                    if (CurrentViewContext.TypeCustomtAttribute.IsNotNull() && !String.IsNullOrEmpty(_caValue))
                    {
                        dPicker.SelectedDate = Convert.ToDateTime(_caValue);
                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }
            }
            else if (dataTypeCode == CustomAttributeDatatype.Text.GetStringValue().ToLower().Trim())
            {
                WclTextBox txtTextType = new WclTextBox();
                txtTextType.ID = "txtTextType_" + _mappingId;
                txtTextType.Enabled = !this.IsReadOnly;
                if (CurrentViewContext.TypeCustomtAttribute.MaxLength.IsNull() || CurrentViewContext.TypeCustomtAttribute.MaxLength <= 0)
                    txtTextType.MaxLength = 50;
                else
                    txtTextType.MaxLength = Convert.ToInt32(CurrentViewContext.TypeCustomtAttribute.MaxLength);

                txtTextType.CssClass = "form-control";
                txtTextType.Width = new Unit("100%");
                divControlMode.Controls.Add(txtTextType);
                if (_isRequired)
                    ApplyRequiredField(_mappingId, txtTextType.ID, _caType);

                try
                {
                    if (CurrentViewContext.CurrentViewContext.TypeCustomtAttribute.IsNotNull() && !String.IsNullOrEmpty(_caValue))
                    {
                        txtTextType.Text = _caValue;
                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }
            }
            else if (dataTypeCode == CustomAttributeDatatype.Numeric.GetStringValue().ToLower().Trim())
            {
                WclNumericTextBox txtNumeric = new WclNumericTextBox();
                txtNumeric.ID = "txtNumericType_" + _mappingId;
                txtNumeric.Enabled = !this.IsReadOnly;
                txtNumeric.CssClass = "form-control";
                txtNumeric.Width = new Unit("100%");
                divControlMode.Controls.Add(txtNumeric);
                if (_isRequired)
                {
                    ApplyRequiredField(_mappingId, txtNumeric.ID, _caType);
                }

                try
                {
                    if (CurrentViewContext.TypeCustomtAttribute.IsNotNull() && !String.IsNullOrEmpty(_caValue))
                    {
                        txtNumeric.Text = _caValue;
                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }
            }
            else if (dataTypeCode == CustomAttributeDatatype.Boolean.GetStringValue().ToLower().Trim())
            {
                RadioButtonList rbtnList = new RadioButtonList();
                rbtnList.RepeatDirection = RepeatDirection.Horizontal;

                rbtnList.Items.Add(new ListItem { Text = "Yes", Value = "1" });
                rbtnList.Items.Add(new ListItem { Text = "No", Value = "2" });
                rbtnList.ID = "rbtnListType_" + _mappingId;
                rbtnList.Enabled = !this.IsReadOnly;
                rbtnList.CssClass = "radio_list";
                rbtnList.Width = new Unit("100%");
                divControlMode.Controls.Add(rbtnList);

                if (_isRequired)
                {
                    ApplyRequiredField(_mappingId, rbtnList.ID, _caType);
                }
                else
                {
                    if (rbtnList.Items.FindByText("NA").IsNull())
                    {
                        if (DoNotShowDefaultValues)
                            rbtnList.Items.Add(new ListItem { Text = "NA", Value = "0", Selected = false });
                        else
                            rbtnList.Items.Add(new ListItem { Text = "NA", Value = "0", Selected = true });
                    }
                }

                try
                {
                    if (CurrentViewContext.TypeCustomtAttribute.IsNotNull() && !String.IsNullOrEmpty(_caValue))
                    {
                        if (String.IsNullOrEmpty(_caValue))
                            rbtnList.SelectedValue = "0";
                        else
                            rbtnList.SelectedValue = _caValue;
                    }
                }
                catch (SysXException ex)
                {
                    base.LogError(ex);
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                }
            }

            if (CurrentViewContext.TypeCustomtAttribute.IsNotNull()
                //&& !String.IsNullOrEmpty(CurrentViewContext.TypeCustomtAttribute.CAValue)
                && CurrentViewContext.TypeCustomtAttribute.CustomAttrValueId.IsNotNull())
            {
                lblCAVId.Text = hdfCAVId.Value = Convert.ToString(CurrentViewContext.TypeCustomtAttribute.CustomAttrValueId);
            }
            if (CurrentViewContext.TypeCustomtAttribute.IsNotNull())
            {
                lblCAId.Text = hdfCAId.Value = Convert.ToString(CurrentViewContext.TypeCustomtAttribute.CustomAttributeId);
            }
        }

        /// <summary>
        /// Generate the dynamic attribute labels for special CSS of Order confirmation page
        /// </summary>
        private void CreateLabels()
        {
            String _caValue = CurrentViewContext.TypeCustomtAttribute.CustomAttributeValue;
            Int32 _camId = CurrentViewContext.TypeCustomtAttribute.CustomAttrMappingId;
            String _caType = lblLabelMode.Text = String.IsNullOrEmpty(CurrentViewContext.TypeCustomtAttribute.CustomAttributeLabel)
           ? CurrentViewContext.TypeCustomtAttribute.CustomAttributeName.HtmlEncode() : CurrentViewContext.TypeCustomtAttribute.CustomAttributeLabel.HtmlEncode();
            //lblLabelMode.Text += ":";
            String _caDataTypeCode = CurrentViewContext.TypeCustomtAttribute.CustomAttributeDataTypeCode;
            try
            {
                if (_caDataTypeCode.ToLower().Trim() != CustomAttributeDatatype.Boolean.GetStringValue().ToLower().Trim())
                {
                    lblAttributeValueLabelMode.Text = String.IsNullOrEmpty(_caValue) ? String.Empty : _caValue;
                }
                else
                {
                    if (String.IsNullOrEmpty(_caValue) || _caValue == "0")
                        lblAttributeValueLabelMode.Text = "NA";
                    else if (_caValue == "1")
                        lblAttributeValueLabelMode.Text = "Yes";
                    else
                        lblAttributeValueLabelMode.Text = "No";
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
            }
        }

        /// <summary>
        /// Generate the dynamic attribute for Read only mode with ronly CSS for labels
        /// </summary>
        private void CreateReadOnlyLabels()
        {
            String _caValue = CurrentViewContext.TypeCustomtAttribute.CustomAttributeValue;
            Int32 _camId = CurrentViewContext.TypeCustomtAttribute.CustomAttrMappingId;
            String _caType = lblReadOnly.Text = String.IsNullOrEmpty(CurrentViewContext.TypeCustomtAttribute.CustomAttributeLabel) ? CurrentViewContext.TypeCustomtAttribute.CustomAttributeName.HtmlEncode() : CurrentViewContext.TypeCustomtAttribute.CustomAttributeLabel.HtmlEncode();

            String _caDataTypeCode = CurrentViewContext.TypeCustomtAttribute.CustomAttributeDataTypeCode;
            try
            {
                if (_caDataTypeCode.ToLower().Trim() != CustomAttributeDatatype.Boolean.GetStringValue().ToLower().Trim())
                    lblAttributeValueReadOnlyMode.Text = String.IsNullOrEmpty(_caValue) ? String.Empty : _caValue;
                else
                {
                    if (String.IsNullOrEmpty(_caValue) || _caValue == "0")
                        lblAttributeValueReadOnlyMode.Text = "NA";
                    else if (_caValue == "1")
                        lblAttributeValueReadOnlyMode.Text = "Yes";
                    else
                        lblAttributeValueReadOnlyMode.Text = "No";
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
            }
        }

        private void ApplyRequiredField(Int64 mappingId, String controlId, String errorMessage)
        {
            RequiredFieldValidator rfValidator = new RequiredFieldValidator();
            rfValidator.ID = "rf_" + controlId;
            rfValidator.Display = ValidatorDisplay.Dynamic;
            rfValidator.ControlToValidate = controlId;
            rfValidator.ErrorMessage = errorMessage + " is required.";
            rfValidator.CssClass = "errmsg";

            if (!String.IsNullOrEmpty(this.ValidationGroup))
                rfValidator.ValidationGroup = this.ValidationGroup;

            System.Web.UI.HtmlControls.HtmlGenericControl div = new System.Web.UI.HtmlControls.HtmlGenericControl();
            div.Attributes.Add("class", "vldx");
            div.Controls.Add(rfValidator);
            divControlMode.Controls.Add(div);
        }

        private String GetValue(Int64 mappingId)
        {
            XmlDataSource xml = new XmlDataSource();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(this.previousValues);
            XmlNodeList nodes = doc.SelectNodes("CustomAttributes/CustomAttribute");
            foreach (XmlNode node in nodes)
            {

                if (node.ChildNodes[3].ChildNodes[0].Value.ToString() == mappingId.ToString())
                {
                    return node.ChildNodes[1].ChildNodes[0].Value.ToString();
                }
            }
            return String.Empty;
        }

        #endregion

        #endregion
    }
}

