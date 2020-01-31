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
    public partial class RequirementVerificationFieldControl : BaseUserControl, IRequirementVerificationFieldControl
    {
        /// <summary>
        /// Represents the Field level data
        /// </summary>
        RequirementVerificationDetailContract IRequirementVerificationFieldControl.FieldData
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the current Context
        /// </summary>
        public IRequirementVerificationFieldControl CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Data of the Combobox Field type
        /// </summary>
        Dictionary<String, String> IRequirementVerificationFieldControl.dicComboData
        {
            get;
            set;
        }

        /// <summary>
        /// List of Documents uplaoded in File Upload type Field. I1 is ApplicantDocumentID, It is FileName, I3 is DocumentPath.
        /// </summary>
        List<Tuple<Int32, String, String>> IRequirementVerificationFieldControl.lstDocuments
        {
            get;
            set;
        }

        /// <summary>
        /// SelectedTenantId
        /// </summary>
        Int32 IRequirementVerificationFieldControl.SelectedTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        String IRequirementVerificationFieldControl.ControlIdGenerator
        {
            get
            {
                return "_" + CurrentViewContext.FieldData.CatId + "_" + CurrentViewContext.FieldData.ItemId + "_";
            }
        }

        public Boolean IsItemEditable
        { get; set; }
        #region UAT-1470 :As a student, there should be a way to close out of the video once you open it.
        /// <summary>
        /// VideoRequiredOpenTime
        /// </summary>
        String IRequirementVerificationFieldControl.VideoRequiredOpenTime
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the screen from which the screen was opened
        /// </summary>
        String IRequirementVerificationFieldControl.ControlUseType
        {
            get;
            set;
        }

        string IRequirementVerificationFieldControl.EntityPermissionName
        {
            get;
            set;
        }

        #endregion

        #region UAT-4368
        public Boolean IsClientAdminLoggedIn
        {
            get;
            set;
        }
        public Boolean IsAdminLoggedIn
        {
            get;
            set;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentViewContext.FieldData.IsNotNull())
            {
                litFieldName.Text = CurrentViewContext.FieldData.FieldName;
                hdfApplicantFieldDataId.Value = Convert.ToString(CurrentViewContext.FieldData.ApplReqFieldDataId);
                hdfFieldId.Value = Convert.ToString(CurrentViewContext.FieldData.FieldId);
                hdfFieldTypeCode.Value = CurrentViewContext.FieldData.FieldDataTypeCode;
                GenerateFieldControl();
                hdfDocType.Value = DocumentViewerDocType.ROTATION_DOCUMENT_PDF.GetStringValue();

                //System.Web.UI.Control _fieldControl = Page.LoadControl("~/ClinicalRotation/UserControl/RequirementVerificationFieldControl.ascx");
                //UAT 2371
                //(_fieldControl as IRequirementVerificationDetailsDocumentConrolView).EntityPermissionName = CurrentViewContext.EntityPermissionName;

            }
        }

        #region Private Methods

        /// <summary>
        /// Generate the actual Field control
        /// </summary>
        private void GenerateFieldControl()
        {
            var _fieldData = CurrentViewContext.FieldData;

            var _isEnabled = _fieldData.FieldAttributeTypeCode.Equals(AppConsts.REQUIREMENT_FIELD_ATTRIBUTE_TYPE_CALCULATED_CODE) ? false : true;

            //if (!IsItemEditable)
            //{
            //    _isEnabled = false;
            //}

            if (IsAdminLoggedIn)
            {
                if (!_fieldData.IsFieldEditableByAdmin)
                {
                    _isEnabled = false;
                }
                
            }
            if (IsClientAdminLoggedIn)
            {
                if (!_fieldData.IsFieldEditableByClientAdmin)
                {
                    _isEnabled = false;
                }               
            }

            //if (CurrentViewContext.ControlUseType == AppConsts.ROTATION_MEMBER_SEARCH_USE_TYPE_CODE.ToLower().Trim())
            //{
            //    _isEnabled = false;
            //}

            if (_fieldData.FieldDataTypeCode == RequirementFieldDataType.DATE.GetStringValue())
            {
                WclDatePicker _datePicker = new WclDatePicker();
                _datePicker.ID = "dp" + CurrentViewContext.ControlIdGenerator + _fieldData.FieldId;
                _datePicker.Enabled = _isEnabled;
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
                    _rfv.Display = ValidatorDisplay.Dynamic;
                    pnlValidation.Controls.Add(_rfv);
                }

                _datePicker.Width = new Unit("100%");

                pnlFieldControl.Controls.Add(_datePicker);
            }
            else if (_fieldData.FieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue())
            {
                WclComboBox _combo = new WclComboBox();
                _combo.ID = "combo" + CurrentViewContext.ControlIdGenerator + _fieldData.FieldId;
                _combo.Enabled = _isEnabled;

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
                    _rfv.Display = ValidatorDisplay.Dynamic;
                    _rfv.InitialValue = AppConsts.COMBOBOX_ITEM_SELECT;
                    pnlValidation.Controls.Add(_rfv);
                }

                _combo.Width = new Unit("100%");
                _combo.AutoSkinMode = false;
                pnlFieldControl.Controls.Add(_combo);
            }
            else if (_fieldData.FieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
            {
                var _document = "<a href='#' onclick='ViewDocItemClickedToView(" + CurrentViewContext.SelectedTenantId + "," + _fieldData.ApplDocId + ")' >" + _fieldData.FieldDocName + "</a>";
                HtmlGenericControl divViewDocument = new HtmlGenericControl("div");
                divViewDocument.ID = "txt" + CurrentViewContext.ControlIdGenerator + _fieldData.FieldId;
                if (_fieldData.FieldDataValue == "0" && _fieldData.ItemStatusCode == RequirementItemStatus.EXPIRED.GetStringValue())
                {
                    divViewDocument.InnerHtml = VerificationDataActions.EXPIRED.GetStringValue();
                }
                else if (_fieldData.FieldDataValue.IsNullOrEmpty() || _fieldData.FieldDataValue == "0")
                {
                    divViewDocument.InnerHtml = AppConsts.NO;
                }
                else
                {
                    divViewDocument.InnerHtml = AppConsts.YES + " (" + _document + ")";
                }

                //divViewDocument.InnerHtml = (_fieldData.FieldDataValue.IsNullOrEmpty() || _fieldData.FieldDataValue == "0"
                //                ? AppConsts.NO
                //                : AppConsts.YES + " (" + _document + ")");
                divViewDocument.Attributes.Add("Width", "100%");
                pnlFieldControl.Controls.Add(divViewDocument);
                pnlValidation.Controls.Add(GenerateDummyControl());
            }
            else if (_fieldData.FieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
            {
                WclTextBox txtViewVideo = new WclTextBox();
                txtViewVideo.ID = "txt" + CurrentViewContext.ControlIdGenerator + _fieldData.FieldId;
                txtViewVideo.Text = _fieldData.FieldDataValue.IsNullOrEmpty() || _fieldData.FieldDataValue == "0"
                                     //UAT-1470:As a student, there should be a way to close out of the video once you open it.
                                     || Convert.ToInt32(_fieldData.FieldDataValue) < Convert.ToInt32(CurrentViewContext.VideoRequiredOpenTime)
                                    ? AppConsts.NO
                                    : AppConsts.YES + " (" + _fieldData.FieldDataValue + " seconds)";

                txtViewVideo.ReadOnly = true;
                txtViewVideo.Width = new Unit("100%");
                pnlFieldControl.Controls.Add(txtViewVideo);
                pnlValidation.Controls.Add(GenerateDummyControl());
            }

            else if (_fieldData.FieldDataTypeCode == RequirementFieldDataType.TEXT.GetStringValue())
            {
                WclTextBox txtField = new WclTextBox();
                txtField.ID = "txt" + CurrentViewContext.ControlIdGenerator + _fieldData.FieldId;
                txtField.Text = Convert.ToString(_fieldData.FieldDataValue);
                txtField.MaxLength = Convert.ToInt32(_fieldData.FieldMaxLength);
                txtField.Width = new Unit("100%");
                txtField.Enabled = _isEnabled;
                pnlFieldControl.Controls.Add(txtField);
                if (_fieldData.IsFieldRequired)
                {
                    RequiredFieldValidator _rfv = new RequiredFieldValidator();
                    _rfv.ID = "rfv" + CurrentViewContext.ControlIdGenerator + _fieldData.FieldId;
                    _rfv.ControlToValidate = txtField.ID;
                    _rfv.ErrorMessage = CurrentViewContext.FieldData.FieldName + " is required.";
                    _rfv.CssClass = "errmsg";
                    _rfv.Display = ValidatorDisplay.Dynamic;
                    pnlValidation.Controls.Add(_rfv);
                }

                // pnlValidation.Controls.Add(GenerateDummyControl());
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
            //UAT-2224: Admin access to upload/associate documents on rotation package items.
            //else if (_fieldData.FieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue())
            //{
            //    if (CurrentViewContext.lstDocuments.IsNullOrEmpty())
            //    {
            //        rptDocuments.Visible = false;
            //        WclTextBox txtNoDoc = new WclTextBox();
            //        txtNoDoc.ID = "txt" + CurrentViewContext.ControlIdGenerator + _fieldData.FieldId;
            //        txtNoDoc.Text = "No Document uploaded";
            //        txtNoDoc.ReadOnly = true; 
            //        txtNoDoc.Width = new Unit("100%");
            //        pnlFieldControl.Controls.Add(txtNoDoc);
            //    }
            //    else
            //    {
            //        rptDocuments.DataSource = CurrentViewContext.lstDocuments;
            //        rptDocuments.DataBind();
            //        rptDocuments.Visible = true;
            //    }
            //    //pnlValidation.Controls.Add(GenerateDummyControl());
            //}
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