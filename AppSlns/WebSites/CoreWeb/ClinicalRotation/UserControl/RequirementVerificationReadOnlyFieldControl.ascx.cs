using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.ClinicalRotation.Views;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RequirementVerificationReadOnlyFieldControl : BaseUserControl, IRequirementVerificationReadOnlyFieldControlView
    {
        /// <summary>
        /// Represents the Field level data
        /// </summary>
        RequirementVerificationDetailContract IRequirementVerificationReadOnlyFieldControlView.FieldData
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the current Context
        /// </summary>
        public IRequirementVerificationReadOnlyFieldControlView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Data of the Combobox Field type
        /// </summary>
        Dictionary<String, String> IRequirementVerificationReadOnlyFieldControlView.dicComboData
        {
            get;
            set;
        }

        /// <summary>
        /// List of Documents uplaoded in File Upload type Field. I1 is ApplicantDocumentID, It is FileName, I3 is DocumentPath.
        /// </summary>
        List<Tuple<Int32, String, String>> IRequirementVerificationReadOnlyFieldControlView.lstDocuments
        {
            get;
            set;
        }

        /// <summary>
        /// SelectedTenantId
        /// </summary>
        Int32 IRequirementVerificationReadOnlyFieldControlView.SelectedTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        String IRequirementVerificationReadOnlyFieldControlView.ControlIdGenerator
        {
            get
            {
                return "_" + CurrentViewContext.FieldData.CatId + "_" + CurrentViewContext.FieldData.ItemId + "_";
            }
        }

        #region UAT-1470 :As a student, there should be a way to close out of the video once you open it.
        /// <summary>
        /// VideoRequiredOpenTime
        /// </summary>
        String IRequirementVerificationReadOnlyFieldControlView.VideoRequiredOpenTime
        {
            get;
            set;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentViewContext.FieldData.IsNotNull())
            {
                litFieldName.Text = CurrentViewContext.FieldData.FieldName.HtmlEncode();
                hdfApplicantFieldDataId.Value = Convert.ToString(CurrentViewContext.FieldData.ApplReqFieldDataId);
                hdfFieldId.Value = Convert.ToString(CurrentViewContext.FieldData.FieldId);
                hdfFieldTypeCode.Value = CurrentViewContext.FieldData.FieldDataTypeCode;
                GenerateFieldControl();
                hdfDocType.Value = DocumentViewerDocType.ROTATION_DOCUMENT_PDF.GetStringValue();
            }
        }

        #region Private Methods

        /// <summary>
        /// Generate the actual Field control
        /// </summary>
        private void GenerateFieldControl()
        {
            var _fieldData = CurrentViewContext.FieldData;

            if (_fieldData.FieldDataTypeCode == RequirementFieldDataType.DATE.GetStringValue())
            {
                WclDatePicker _datePicker = new WclDatePicker();
                _datePicker.ID = "dp" + CurrentViewContext.ControlIdGenerator + _fieldData.FieldId;
                _datePicker.MinDate = new DateTime(1900, 01, 01);

                if (_fieldData.ApplReqFieldDataId != AppConsts.NONE && !String.IsNullOrEmpty(_fieldData.FieldDataValue))
                {
                    _datePicker.SelectedDate = Convert.ToDateTime(_fieldData.FieldDataValue).Date;
                }

                if (_fieldData.IsFieldRequired)
                {
                    RequiredFieldValidator _rfv = new RequiredFieldValidator();
                    _rfv.ID = "rfv" + CurrentViewContext.ControlIdGenerator + _fieldData.FieldId;
                    _rfv.ControlToValidate = _datePicker.ID;
                    _rfv.ErrorMessage = CurrentViewContext.FieldData.FieldName + " is required.";
                    _rfv.CssClass = "errmsg";
                    pnlValidation.Controls.Add(_rfv);
                }

                _datePicker.Enabled = false;
                _datePicker.CssClass = "form-control";
                _datePicker.Width = new Unit("100%");
                pnlFieldControl.Controls.Add(_datePicker);
            }
            else if (_fieldData.FieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue())
            {
                WclComboBox _combo = new WclComboBox();
                _combo.ID = "combo" + CurrentViewContext.ControlIdGenerator + _fieldData.FieldId;
                BindComboBox(_combo);

                if (_fieldData.ApplReqFieldDataId != AppConsts.NONE && !String.IsNullOrEmpty(_fieldData.FieldDataValue))
                {
                    _combo.SelectedValue = Convert.ToString(_fieldData.FieldDataValue);
                }

                if (_fieldData.IsFieldRequired)
                {
                    RequiredFieldValidator _rfv = new RequiredFieldValidator();
                    _rfv.ID = "rfv" + CurrentViewContext.ControlIdGenerator + _fieldData.FieldId;
                    _rfv.ControlToValidate = _combo.ID;
                    _rfv.ErrorMessage = CurrentViewContext.FieldData.FieldName + " is required.";
                    _rfv.CssClass = "errmsg";
                    _rfv.InitialValue = AppConsts.COMBOBOX_ITEM_SELECT;
                    pnlValidation.Controls.Add(_rfv);
                }
                _combo.Enabled = false;
                _combo.CssClass = "form-control";
                _combo.Width = new Unit("100%");
                _combo.Skin = "Silk";
                _combo.AutoSkinMode = false;
                pnlFieldControl.Controls.Add(_combo);
            }
            else if (_fieldData.FieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
            {
                var _document = "<a href='#' onclick='ViewDocument(" + CurrentViewContext.SelectedTenantId + "," + _fieldData.ApplDocId + ")' >" + _fieldData.FieldDocName + "</a>";
                HtmlGenericControl divViewDocument = new HtmlGenericControl("div");
                divViewDocument.ID = "txt" + CurrentViewContext.ControlIdGenerator + _fieldData.FieldId;
                divViewDocument.InnerHtml = (_fieldData.FieldDataValue.IsNullOrEmpty() || _fieldData.FieldDataValue == "0"
                                ? AppConsts.NO
                                : AppConsts.YES + " (" + _document + ")");
                divViewDocument.Attributes.Add("class", "form-control");
                divViewDocument.Attributes.Add("Width", "100%");
                pnlFieldControl.Controls.Add(divViewDocument);
                pnlValidation.Controls.Add(GenerateDummyControl());
            }
            else if (_fieldData.FieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
            {
                WclTextBox txtViewVideo = new WclTextBox();
                txtViewVideo.ID = "txt" + CurrentViewContext.ControlIdGenerator + _fieldData.FieldId;
                txtViewVideo.Text = _fieldData.FieldDataValue.IsNullOrEmpty() || _fieldData.FieldDataValue == "0"
                                    //UAT-1470 :As a student, there should be a way to close out of the video once you open it.
                                     || Convert.ToInt32(_fieldData.FieldDataValue) < Convert.ToInt32(CurrentViewContext.VideoRequiredOpenTime)
                                    ? AppConsts.NO
                                    : AppConsts.YES + " (" + _fieldData.FieldDataValue + " seconds)";

                txtViewVideo.ReadOnly = true;
                txtViewVideo.CssClass = "form-control";
                txtViewVideo.Width = new Unit("100%");
                pnlFieldControl.Controls.Add(txtViewVideo);
                pnlValidation.Controls.Add(GenerateDummyControl());
            }
            else if (_fieldData.FieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue())
            {
                if (CurrentViewContext.lstDocuments.IsNullOrEmpty())
                {
                    rptDocuments.Visible = false;
                    WclTextBox txtNoDoc = new WclTextBox();
                    txtNoDoc.ID = "txt" + CurrentViewContext.ControlIdGenerator + _fieldData.FieldId;
                    txtNoDoc.Text = "No Document uploaded";
                    txtNoDoc.ReadOnly = true;
                    txtNoDoc.CssClass = "form-control";
                    txtNoDoc.Width = new Unit("100%");
                    pnlFieldControl.Controls.Add(txtNoDoc);
                }
                else
                {
                    rptDocuments.DataSource = CurrentViewContext.lstDocuments;
                    rptDocuments.DataBind();
                    rptDocuments.Visible = true;
                }
                //pnlValidation.Controls.Add(GenerateDummyControl());
            }
            else if (_fieldData.FieldDataTypeCode == RequirementFieldDataType.SIGNATURE.GetStringValue())
            {
                string value = "No";
                if (!string.IsNullOrEmpty(_fieldData.FieldDataValue) && _fieldData.FieldDataValue.ToLower() == "true")
                    value = "Yes";

                WclTextBox txtField = new WclTextBox();
                txtField.ID = "txt" + CurrentViewContext.ControlIdGenerator + _fieldData.FieldId;
                txtField.Text = value;
                txtField.MaxLength = Convert.ToInt32(_fieldData.FieldMaxLength);
                txtField.Enabled = false;
                txtField.Width = new Unit("100%");
                pnlFieldControl.Controls.Add(txtField);
            }
        }

        private WclTextBox GenerateDummyControl()
        {
            var textbox = new WclTextBox
            {
                Text = String.Empty,
                Enabled = false,

            };

            textbox.Attributes.Add("style", "display:none");
            return textbox;
        }

        /// <summary>
        /// Bind combobox for ComboType Field
        /// </summary>
        /// <param name="comboBox"></param>
        private void BindComboBox(WclComboBox comboBox)
        {
            comboBox.Items.Add(new RadComboBoxItem
            {
                Text = AppConsts.COMBOBOX_ITEM_SELECT,
                Value = AppConsts.ZERO
            });

            foreach (var comboItem in CurrentViewContext.dicComboData)
            {
                comboBox.Items.Add(new RadComboBoxItem
                {
                    Text = comboItem.Value,
                    Value = comboItem.Key
                });
            }
        }

        #endregion
    }
}