using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using NLog;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class DataEntryAttribute : BaseUserControl
    {

        /// <summary>
        /// List is used to Cover the case of Options type Attribute
        /// </summary>
        public List<AdminDataEntryUIContract> AttributeUIContract
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the ComplianceItemId to which the Attribute belongs to
        /// </summary>
        public Int32 ItemId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the ComplianceCategoryId to which the Attribute belongs to
        /// </summary>
        public Int32 CatId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the ComplianceAttributeId
        /// </summary>
        public Int32 AttrId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the ApplicantComplianceAttributeDataID
        /// </summary>
        public Int32 AttrDataId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the AttributeGroupId
        /// </summary>
        public Int32 AttributeGroupId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the AttributeDataType
        /// </summary>
        public String AttributeDataType
        {
            get;
            set;
        }

        /// <summary>
        /// Suffix of the Id's generated for the Controls
        /// </summary>
        private String ControlIdSuffix
        {
            get
            {
                if (IsItemSeries)
                {
                    return this.CatId + "_" + this.ItemId + "_" + this.AttrId + "_" + this.AttributeGroupId + "_" + this.ItemSeriesId + "_" + this.AttributeDataType;
                }
                else
                {
                    return this.CatId + "_" + this.ItemId + "_" + this.AttrId + "_" + this.AttributeGroupId + "_" + this.AttributeDataType;
                }
            }
        }

        #region UAT-1608
        public Boolean IsItemSeries
        {
            get;
            set;
        }

        public Int32 ItemSeriesId
        {
            get;
            set;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            var _currentAttribute = AttributeUIContract.First();
            this.AttrId = _currentAttribute.AttrId;
            hdfAttrId.Value = Convert.ToString(_currentAttribute.AttrId);
            this.AttrDataId = _currentAttribute.AttrDataId;
            this.AttributeDataType = _currentAttribute.AttrDataType;

            hdfAttrGrpId.Value = Convert.ToString(this.AttributeGroupId);
            hdfAttrDataType.Value = this.AttributeDataType;

            var _generatedControl = GenerateServerControl(this.AttributeDataType, _currentAttribute.AttrDataId, _currentAttribute.IsReadOnly, _currentAttribute.ComplianceAttributeTypeCode);

            // Case of File Upload control - UAT 1252
            if (_generatedControl.IsNotNull())
            {
                pnlAttributes.Controls.Add(_generatedControl);
            }

            #region UAT-1608
            var _generatedHiddenField = AddHiddenField(_currentAttribute.AttrDataId);
            if (_generatedHiddenField.IsNotNull())
            {
                pnlAttributes.Controls.Add(_generatedHiddenField);
            }
            #endregion
        }

        /// <summary>
        /// Generate the Control, bsaed on the DataTypeCode
        /// </summary>
        /// <param name="dataTypeCode"></param>
        /// <param name="catId"></param>
        /// <param name="itemId"></param>
        /// <param name="attrId"></param>
        /// <param name="attrDataId"></param>
        /// <returns></returns>
        private Control GenerateServerControl(String dataTypeCode, Int32 attrDataId, Boolean isReadOnly, string ComplianceAttributeTypeCode)
        {
            var _attributeValue = String.Empty;
            if (attrDataId > AppConsts.NONE)
            {
                _attributeValue = this.AttributeUIContract.Where(uic => uic.AttrDataId == attrDataId).First().AttrValue;
            }
            if (dataTypeCode == ComplianceAttributeDatatypes.Text.GetStringValue())
            {
                WclTextBox _txtBox = new WclTextBox();
                _txtBox.ID = "txt_" + this.ControlIdSuffix;
                _txtBox.Text = _attributeValue;
                _txtBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                //UAT-1608:UAT-1608: Admin data entry screen[Shot Series Changes]
                //_txtBox.Enabled = !isReadOnly;
                _txtBox.Enabled = false;
                _txtBox.Attributes.Add("cattr", "cattr_" + this.CatId + "_" + this.ItemId);
                _txtBox.Attributes.Add("ctrlType", dataTypeCode);
                _txtBox.Attributes.Add("ComplianceAttrType", ComplianceAttributeTypeCode);
                return _txtBox;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Numeric.GetStringValue())
            {
                WclNumericTextBox _numericTxtBox = new WclNumericTextBox();
                _numericTxtBox.ID = "numericTxt_" + this.ControlIdSuffix;
                _numericTxtBox.Text = _attributeValue;
                _numericTxtBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                //UAT-1608:UAT-1608: Admin data entry screen[Shot Series Changes]
                //_numericTxtBox.Enabled = !isReadOnly;

                _numericTxtBox.Enabled = false;

                _numericTxtBox.Attributes.Add("cattr", "cattr_" + this.CatId + "_" + this.ItemId);
                _numericTxtBox.Attributes.Add("ctrlType", dataTypeCode);
                _numericTxtBox.Attributes.Add("ComplianceAttrType", ComplianceAttributeTypeCode);
                return _numericTxtBox;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Date.GetStringValue())
            {
                WclDatePicker _datePicker = new WclDatePicker();
                _datePicker.MinDate = Convert.ToDateTime("01-01-1900");
                _datePicker.ID = "datePicker_" + this.ControlIdSuffix;
                if (!String.IsNullOrEmpty(_attributeValue))
                    _datePicker.SelectedDate = Convert.ToDateTime(_attributeValue);

                //UAT-1608:UAT-1608: Admin data entry screen[Shot Series Changes]
                //_datePicker.Enabled = !isReadOnly;
                _datePicker.Enabled = false;
                
                _datePicker.Attributes.Add("cattr", "cattr_" + this.CatId + "_" + this.ItemId);
                _datePicker.Attributes.Add("ctrlType", dataTypeCode);
                _datePicker.Attributes.Add("ComplianceAttrType", ComplianceAttributeTypeCode);
                return _datePicker;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Options.GetStringValue())
            {
                //WclComboBox _comboBox = new WclComboBox();
                //   _comboBox.ID = "comboBox_" + this.ControlIdSuffix;
                //   _comboBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                WclDropDownList _comboBox = new WclDropDownList();
                _comboBox.ID = "comboBox_" + this.ControlIdSuffix;
                _comboBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                BindCombobox(_comboBox, _attributeValue);
                //UAT-1608:UAT-1608: Admin data entry screen[Shot Series Changes]
               // _comboBox.Enabled = !isReadOnly;

                _comboBox.Enabled = false;
                base.LogDataEntry(String.Format("Options Control generated on Page Load with ID as: {0} and Attribute Value as: {1}", _comboBox.ID, _attributeValue));
                
                _comboBox.Attributes.Add("cattr", "cattr_" + this.CatId + "_" + this.ItemId);
                _comboBox.Attributes.Add("ctrlType", dataTypeCode);
                _comboBox.Attributes.Add("ComplianceAttrType", ComplianceAttributeTypeCode);
                return _comboBox;
            }
            return null;
        }

        /// <summary>
        /// Bind the combobox for the Options type attribute and set the Selected Value for it
        /// </summary>
        /// <param name="cmbToBind"></param>
        /// <param name="attrId"></param>
        /// <param name="selectedValue"></param>
        private void BindCombobox(WclDropDownList cmbToBind, String selectedValue)
        {
            var _lstCurrentAttributeOptions = this.AttributeUIContract.Where(uic => uic.AttrId == this.AttrId)
                    .Select(uic => new
                    {
                        OptionText = uic.AttrOptionText,
                        OptionValue = uic.AttrOptionValue
                    }
                    ).ToList();

            //cmbToBind.Items.Add(new RadComboBoxItem
            //{
            //    Text = AppConsts.COMBOBOX_ITEM_SELECT,
            //    Value = AppConsts.ZERO
            //});

            //foreach (var optn in _lst)
            //{
            //    cmbToBind.Items.Add(new RadComboBoxItem
            //    {
            //        Text = optn.OptionText,
            //        Value = optn.OptionValue
            //    });
            //}

            List<Entity.ClientEntity.ComplianceAttributeOption> _lstAttributeOptions = new List<Entity.ClientEntity.ComplianceAttributeOption>();

            _lstAttributeOptions.Add(new Entity.ClientEntity.ComplianceAttributeOption
            {
                OptionText = AppConsts.COMBOBOX_ITEM_SELECT,
                OptionValue = AppConsts.ZERO
            });

            foreach (var optn in _lstCurrentAttributeOptions)
            {
                _lstAttributeOptions.Add(new Entity.ClientEntity.ComplianceAttributeOption
                {
                    OptionText = optn.OptionText,
                    OptionValue = optn.OptionValue
                });
            }

            cmbToBind.DataSource = _lstAttributeOptions;
            cmbToBind.DataTextField = "OptionText";
            cmbToBind.DataValueField = "OptionValue";
            cmbToBind.DataBind();

            cmbToBind.SelectedValue = selectedValue;
        }

        /// <summary>
        /// Get the Attribute value, based on the Data type
        /// </summary>
        /// <param name="dataTypeCode"></param>
        /// <returns></returns>
        private String GetAttributeValue(String dataTypeCode)
        {
            var _attributeValue = String.Empty;

            if (dataTypeCode == ComplianceAttributeDatatypes.Text.GetStringValue())
            {
                var _txtBox = pnlAttributes.FindServerControlRecursively("txt_" + this.ControlIdSuffix) as WclTextBox;
                _attributeValue = _txtBox.Text;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Numeric.GetStringValue())
            {
                var _numericTxtBox = pnlAttributes.FindServerControlRecursively("numericTxt_" + this.ControlIdSuffix) as WclNumericTextBox;
                _attributeValue = _numericTxtBox.Text;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Date.GetStringValue())
            {
                var _datePicker = pnlAttributes.FindServerControlRecursively("datePicker_" + this.ControlIdSuffix) as WclDatePicker;
                _attributeValue = Convert.ToString(_datePicker.SelectedDate);
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Options.GetStringValue())
            {
                var _comboBox = pnlAttributes.FindServerControlRecursively("comboBox_" + this.ControlIdSuffix) as WclDropDownList;
                _attributeValue = _comboBox.SelectedValue;

                base.LogDataEntry(String.Format("Options Control Value for Save Process with ID as: {0} and Attribute Value as: {1}", _comboBox.ID, _attributeValue));
            }
            return _attributeValue;
        }

        /// <summary>
        /// Gets the Data related to the current attribute
        /// </summary>
        /// <returns></returns>
        public ApplicantCmplncAttrData GetAttributeData()
        {
            var _attributeData = new ApplicantCmplncAttrData();
            _attributeData.AttrId = Convert.ToInt32(this.AttrId);
            _attributeData.AcadId = Convert.ToInt32(this.AttrDataId);
            _attributeData.AttrTypeCode = this.AttributeUIContract.First().AttrDataType;
            _attributeData.AttrValue = GetAttributeValue(_attributeData.AttrTypeCode);
            _attributeData.AttrGroupId = String.IsNullOrEmpty(hdfAttrGrpId.Value) ? AppConsts.NONE : Convert.ToInt32(hdfAttrGrpId.Value);
            return _attributeData;
        }

        #region UAT-1608
        private HiddenField AddHiddenField(Int32 attrDataId)
        {
            var _attributeValue = String.Empty;
            if (attrDataId > AppConsts.NONE)
            {
                _attributeValue = this.AttributeUIContract.Where(uic => uic.AttrDataId == attrDataId).First().AttrValue;
            }
            HiddenField hdnField = new HiddenField();
            hdnField.ID = "hdfEnteredData" + "_" + this.ControlIdSuffix;
            hdnField.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            hdnField.Value = _attributeValue;
            return hdnField;
        }

        public Boolean IsValueChanged(String previousValueControlId, String newValue)
        {
            Boolean isValueChanged = true;
            HiddenField hdnField = pnlAttributes.FindServerControlRecursively(previousValueControlId + "_" + this.ControlIdSuffix) as HiddenField;
            if (!hdnField.IsNullOrEmpty())
            {
                String prevValue= hdnField.Value;
                if (this.AttributeDataType == ComplianceAttributeDatatypes.Date.GetStringValue() && !prevValue.IsNullOrEmpty())
                {
                    prevValue = Convert.ToDateTime(prevValue).ToShortDateString();
                    newValue = Convert.ToDateTime(newValue).ToShortDateString();
                }
                else if (this.AttributeDataType == ComplianceAttributeDatatypes.Options.GetStringValue())
                {
                    prevValue = prevValue.IsNullOrEmpty() ? AppConsts.ZERO : prevValue;
                }
                if (prevValue.Trim() == newValue.Trim())
                {
                    isValueChanged = false;
                }
            }
            return isValueChanged;
        }
        #endregion
    }
}