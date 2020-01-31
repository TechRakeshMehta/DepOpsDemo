using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceAdministration.Views;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Telerik.Web.UI;
using CoreWeb.Shell;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class SeriesUnMappedAttributes : BaseUserControl, ISeriesUnMappedAttributesView
    {
        #region Variables

        #region Private Variables

        private SeriesUnMappedAttributesPresenter _presenter = new SeriesUnMappedAttributesPresenter();

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public SeriesUnMappedAttributesPresenter Presenter
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

        public ISeriesUnMappedAttributesView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 TenantId
        {
            get { return Convert.ToInt32(ViewState["TenantId"]); }
            set { ViewState["TenantId"] = value; }
        }

        public Int32 ItemSeriesId
        {
            get { return Convert.ToInt32(ViewState["ItemSeriesId"]); }
            set { ViewState["ItemSeriesId"] = value; }
        }

        Int32 ISeriesUnMappedAttributesView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        List<SeriesAttributeContract> ISeriesUnMappedAttributesView.UnMappedAttributesList
        {
            get;
            set;
        }

        String ISeriesUnMappedAttributesView.ControlIdSuffix
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region EventS

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }

            //Bind UnMapped Attributes and controls
            BindUnMappedAttributes();

            if (CurrentViewContext.UnMappedAttributesList.Any())
            {
                divAttributes.Visible = true;
            }

        }

        /// <summary>
        /// Save button click event to save attributes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar_SaveClick(object sender, EventArgs e)
        {
            List<SeriesAttributeContract> lstSeriesAttributeContract = GetAttributeValues();

            if (Presenter.SaveUnMappedAttributes(lstSeriesAttributeContract))
            {
                //base.ShowSuccessMessage("UnMapped Attributes saved successfully.");
                (this.Page as BaseWebPage).ShowSuccessMessage("Unmapped Attribute(s) saved successfully.");
            }
            else
            {
                (this.Page as BaseWebPage).ShowErrorInfoMessage("Some error has occured. Please contact administrator.");
            }
            BindUnMappedAttributes();
        }

        #region Repeater EventS

        /// <summary>
        /// Attributes repeater ItemDataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptUnMappedAttributes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                try
                {
                    HiddenField hdnItemID = e.Item.FindControl("hdnItemID") as HiddenField;
                    HiddenField hdnAttributeID = e.Item.FindControl("hdnAttributeID") as HiddenField;
                    HiddenField hdnAttrDatatypeCode = e.Item.FindControl("hdnAttrDatatypeCode") as HiddenField;
                    HiddenField hdnItemSeriesItemID = e.Item.FindControl("hdnItemSeriesItemID") as HiddenField;

                    if (hdnAttrDatatypeCode.IsNotNull() && hdnItemID.IsNotNull() && hdnAttributeID.IsNotNull())
                    {
                        var _generatedControl = GenerateServerControl(hdnAttrDatatypeCode.Value, hdnItemID.Value, hdnAttributeID.Value, hdnItemSeriesItemID.Value);
                        if (_generatedControl.IsNotNull())
                        {
                            Panel pnlAttributes = e.Item.FindControl("pnlAttributes") as Panel;
                            if (pnlAttributes.IsNotNull())
                            {
                                pnlAttributes.Controls.Add(_generatedControl);
                            }
                        }
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
        }


        #endregion

        #endregion

        #region Methods

        private void BindUnMappedAttributes()
        {
            Presenter.GetUnMappedAttributes();
            rptUnMappedAttributes.DataSource = CurrentViewContext.UnMappedAttributesList;
            rptUnMappedAttributes.DataBind();
        }

        /// <summary>
        /// Generate the Control, based on the DataTypeCode
        /// </summary>
        /// <param name="dataTypeCode"></param>
        /// <param name="itemId"></param>
        /// <param name="attrId"></param>
        /// <returns></returns>
        private Control GenerateServerControl(String dataTypeCode, String itemId, String attrId, String serieItemId)
        {
            CurrentViewContext.ControlIdSuffix = serieItemId + "_" + itemId + "_" + attrId + "_" + dataTypeCode;
            var _attributeValue = String.Empty;
            Int32 _itemId = 0;
            Int32 _attrId = 0;
            Int32 _serItemId = 0;

            if (!itemId.IsNullOrEmpty())
                _itemId = Convert.ToInt32(itemId);
            if (!attrId.IsNullOrEmpty())
                _attrId = Convert.ToInt32(attrId);
            if (!serieItemId.IsNullOrEmpty())
                _serItemId = Convert.ToInt32(serieItemId);

            if (_itemId > AppConsts.NONE && _attrId > AppConsts.NONE && _serItemId > AppConsts.NONE)
            {
                var unMappedAttribute = CurrentViewContext.UnMappedAttributesList.Where(x => x.CmpAttributeId == _attrId && x.CmpItemId == _itemId && x.CmpItemSeriesItemId == _serItemId).FirstOrDefault();
                if (!unMappedAttribute.IsNullOrEmpty())
                    _attributeValue = unMappedAttribute.CmpAttributeValue;
            }

            if (dataTypeCode == ComplianceAttributeDatatypes.Text.GetStringValue())
            {
                WclTextBox _txtBox = new WclTextBox();
                _txtBox.ID = "txt_" + CurrentViewContext.ControlIdSuffix;
                _txtBox.Text = _attributeValue;
                _txtBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                return _txtBox;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Numeric.GetStringValue())
            {
                WclNumericTextBox _numericTxtBox = new WclNumericTextBox();
                _numericTxtBox.ID = "numericTxt_" + CurrentViewContext.ControlIdSuffix;
                _numericTxtBox.Text = _attributeValue;
                _numericTxtBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                return _numericTxtBox;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Date.GetStringValue())
            {
                WclDatePicker _datePicker = new WclDatePicker();
                _datePicker.MinDate = Convert.ToDateTime("01-01-1900");
                _datePicker.ID = "datePicker_" + CurrentViewContext.ControlIdSuffix;
                if (!String.IsNullOrEmpty(_attributeValue))
                    _datePicker.SelectedDate = Convert.ToDateTime(_attributeValue);

                return _datePicker;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Options.GetStringValue())
            {
                WclDropDownList _comboBox = new WclDropDownList();
                _comboBox.ID = "comboBox_" + CurrentViewContext.ControlIdSuffix;
                _comboBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                BindCombobox(_comboBox, _attributeValue, _itemId, _attrId);

                base.LogDataEntry(String.Format("Options Control generated on Page Load with ID as: {0} and Attribute Value as: {1}", _comboBox.ID, _attributeValue));

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
        private void BindCombobox(WclDropDownList cmbToBind, String selectedValue, Int32 _itemId, Int32 _attrId)
        {
            List<ComplianceAttributeOption> _lstAttributeOptions = new List<ComplianceAttributeOption>();
            var currentAttributeOptions = CurrentViewContext.UnMappedAttributesList.FirstOrDefault(x => x.CmpItemId == _itemId && x.CmpAttributeId == _attrId);

            _lstAttributeOptions.Add(new Entity.ClientEntity.ComplianceAttributeOption
            {
                OptionText = AppConsts.COMBOBOX_ITEM_SELECT,
                OptionValue = AppConsts.ZERO
            });

            if (currentAttributeOptions.IsNotNull())
            {
                var currentAttributeOption = currentAttributeOptions.AttributeOptionList.Split(',');

                foreach (var optn in currentAttributeOption)
                {
                    var currentAttributeOptionText = optn.Split('-');

                    if (currentAttributeOptionText.IsNotNull())
                    {
                        _lstAttributeOptions.Add(new Entity.ClientEntity.ComplianceAttributeOption
                        {
                            OptionText = currentAttributeOptionText[0],
                            OptionValue = currentAttributeOptionText[1]
                        });
                    }
                }
            }

            cmbToBind.DataSource = _lstAttributeOptions;
            cmbToBind.DataTextField = "OptionText";
            cmbToBind.DataValueField = "OptionValue";
            cmbToBind.DataBind();

            cmbToBind.SelectedValue = selectedValue;
        }

        /// <summary>
        /// Get the Attribute values
        /// </summary>
        /// <returns></returns>
        private List<SeriesAttributeContract> GetAttributeValues()
        {
            List<SeriesAttributeContract> lstSeriesAttributeContract = new List<SeriesAttributeContract>();

            if (rptUnMappedAttributes.IsNotNull())
            {
                foreach (RepeaterItem item in rptUnMappedAttributes.Items)
                {
                    SeriesAttributeContract seriesAttributeContract = new SeriesAttributeContract();
                    HiddenField hdnItemID = item.FindControl("hdnItemID") as HiddenField;
                    HiddenField hdnAttributeID = item.FindControl("hdnAttributeID") as HiddenField;
                    HiddenField hdnAttrDatatypeCode = item.FindControl("hdnAttrDatatypeCode") as HiddenField;
                    HiddenField hdnItemSeriesItemID = item.FindControl("hdnItemSeriesItemID") as HiddenField;
                    HiddenField hdnItemSeriesItemAttributeValueID = item.FindControl("hdnItemSeriesItemAttributeValueID") as HiddenField;

                    if (hdnAttrDatatypeCode.IsNotNull() && hdnItemID.IsNotNull() && hdnAttributeID.IsNotNull() && hdnItemSeriesItemID.IsNotNull()
                        && hdnItemSeriesItemAttributeValueID.IsNotNull())
                    {
                        CurrentViewContext.ControlIdSuffix = hdnItemSeriesItemID.Value + "_" + hdnItemID.Value + "_" + hdnAttributeID.Value + "_" + hdnAttrDatatypeCode.Value;

                        seriesAttributeContract.CmpAttributeValue = GetAttributeValue(hdnAttrDatatypeCode.Value, item);
                        seriesAttributeContract.CmpAttributeDatatypeCode = hdnAttrDatatypeCode.Value;
                        seriesAttributeContract.CmpAttributeId = hdnAttributeID.Value.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(hdnAttributeID.Value);
                        seriesAttributeContract.CmpItemSeriesItemId = hdnItemSeriesItemID.Value.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(hdnItemSeriesItemID.Value);
                        seriesAttributeContract.CmpItemSeriesItemAttributeValueId = hdnItemSeriesItemAttributeValueID.Value.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(hdnItemSeriesItemAttributeValueID.Value);
                    }
                    lstSeriesAttributeContract.Add(seriesAttributeContract);
                }
            }
            return lstSeriesAttributeContract;
        }

        /// <summary>
        /// Get the Attribute value, based on the Data type
        /// </summary>
        /// <param name="dataTypeCode"></param>
        /// <returns></returns>
        private String GetAttributeValue(String dataTypeCode, RepeaterItem item)
        {
            var _attributeValue = String.Empty;

            if (dataTypeCode == ComplianceAttributeDatatypes.Text.GetStringValue())
            {
                var _txtBox = item.FindControl("txt_" + CurrentViewContext.ControlIdSuffix) as WclTextBox;
                _attributeValue = _txtBox.Text;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Numeric.GetStringValue())
            {
                var _numericTxtBox = item.FindControl("numericTxt_" + CurrentViewContext.ControlIdSuffix) as WclNumericTextBox;
                _attributeValue = _numericTxtBox.Text;
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Date.GetStringValue())
            {
                var _datePicker = item.FindControl("datePicker_" + CurrentViewContext.ControlIdSuffix) as WclDatePicker;
                _attributeValue = Convert.ToString(_datePicker.SelectedDate);
            }
            else if (dataTypeCode == ComplianceAttributeDatatypes.Options.GetStringValue())
            {
                var _comboBox = item.FindControl("comboBox_" + CurrentViewContext.ControlIdSuffix) as WclDropDownList;
                _attributeValue = _comboBox.SelectedValue;

                base.LogDataEntry(String.Format("Options Control Value for Save Process with ID as: {0} and Attribute Value as: {1}", _comboBox.ID, _attributeValue));
            }
            return _attributeValue;
        }

        #endregion
    }
}