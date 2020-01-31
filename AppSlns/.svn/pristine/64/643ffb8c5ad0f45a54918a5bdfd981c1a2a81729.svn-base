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
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RequirementVerificationReadOnlyItemControl : BaseUserControl, IRequirementVerificationReadOnlyItemControlView
    {
        #region Variables

        //UAT-1470 :As a student, there should be a way to close out of the video once you open it.
        private RequirementVerificationReadOnlyItemControlPresenter _presenter = new RequirementVerificationReadOnlyItemControlPresenter();

        #endregion;

        #region Properties

        /// <summary>
        /// RequirementItemID
        /// </summary>
        Int32 IRequirementVerificationReadOnlyItemControlView.ItemId { get; set; }

        /// <summary>
        /// ApplicantRequirementItemDataID
        /// </summary>
        Int32 IRequirementVerificationReadOnlyItemControlView.ApplReqItemDataId { get; set; }

        /// <summary>
        /// Represents the Item level data
        /// </summary>
        List<RequirementVerificationDetailContract> IRequirementVerificationReadOnlyItemControlView.lstItemLevelData
        {
            get;
            set;
        }

        /// <summary>
        /// List for 'lkpRequirementItemStatus' entity
        /// </summary>
        List<RequirementItemStatusContract> IRequirementVerificationReadOnlyItemControlView.lstReqItemStatusTypes
        {
            get;
            set;
        }

        /// <summary>
        /// SelectedTenantId
        /// </summary>
        Int32 IRequirementVerificationReadOnlyItemControlView.SelectedTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// ID Prefix for the Field Control
        /// </summary>
        String IRequirementVerificationReadOnlyItemControlView.FieldControlIdPrefix
        {
            get
            {
                return "ucFieldControl_" + CurrentViewContext.lstItemLevelData[0].CatId + "_" + CurrentViewContext.ItemId + "_";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        String IRequirementVerificationReadOnlyItemControlView.FieldControlIdGenerator
        {
            get
            {
                return "_" + CurrentViewContext.lstItemLevelData.First().CatId + "_" + CurrentViewContext.ItemId + "_";
            }
        }


        /// <summary>
        /// Represents the current Context
        /// </summary>
        public IRequirementVerificationReadOnlyItemControlView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #region UAT-1470 :As a student, there should be a way to close out of the video once you open it.
        /// <summary>
        /// PackageID
        /// </summary>
        Int32 IRequirementVerificationReadOnlyItemControlView.PackageID { get; set; }

        /// <summary>
        /// CategoryID
        /// </summary>
        Int32 IRequirementVerificationReadOnlyItemControlView.CategoryID { get; set; }

        List<RequirementObjectTreeContract> IRequirementVerificationReadOnlyItemControlView.LstRequirementObjTreeProperty { get; set; }
        public RequirementVerificationReadOnlyItemControlPresenter Presenter
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
        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentViewContext.lstItemLevelData.IsNullOrEmpty())
            {
                var _currenItemData = CurrentViewContext.lstItemLevelData.First();
                litItemName.Text = _currenItemData.ItemName.HtmlEncode();
                txtRejectionreason.Text = _currenItemData.RejectionReason;
                CurrentViewContext.ApplReqItemDataId = _currenItemData.ApplReqItemDataId;
                imgItemStatus.ImageUrl = GetImagePathItemStatus(_currenItemData.ItemStatusCode);
                imgItemStatus.ToolTip = _currenItemData.ItemStatusName;

                txtRejectionreason.Attributes.Add("noteItemId", Convert.ToString(_currenItemData.ItemId));
                rbtnListStatus.Attributes.Add("actionItemId", Convert.ToString(_currenItemData.ItemId));
                rbtnListStatus.Attributes.Add("catId", Convert.ToString(CurrentViewContext.lstItemLevelData.First().CatId));
                rbtnListStatus.Attributes.Add("itmId", Convert.ToString(CurrentViewContext.ItemId));

                #region UAT-3077:(1 of ?) Initial Analysis and begin Dev: Pay per submission item type (CC only) for Tracking and Rotation
                if (_currenItemData.ItemAmount.IsNotNull())
                {
                    decimal itemAmount = _currenItemData.ItemAmount.Value;


                    litItemAmount.Text = "$" + itemAmount.ToString("0.00");
                }
                litItemPaymentStatus.Text = _currenItemData.ItemPaymentStatus;

                if (_currenItemData.IsItemPaymentPaid && _currenItemData.PaidItemAmount.HasValue)
                    litItemPaymentStatus.Text += string.Concat(" ($", _currenItemData.PaidItemAmount.Value.ToString("0.00"), ")");

                if (_currenItemData.IsPaymentTypeItem)
                {
                    divItemPaymentPanel.Style["display"] = "block";
                }
                else
                {
                    divItemPaymentPanel.Style["display"] = "none";
                }
                #endregion
            }

            GenerateFields();

            if (!this.IsPostBack)
            {
                BindItemStatus(CurrentViewContext.lstReqItemStatusTypes);
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods


        private void GenerateFields()
        {
            Int32 _attributesPerRow = 3;
            String uploadDocTypeCode = RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue();

            // List of current Fields to be added into the control
            List<RequirementVerificationDetailContract> lstDstctFields = CurrentViewContext.lstItemLevelData.Where(cond => cond.FieldDataTypeCode != uploadDocTypeCode).OrderBy(vdd => vdd.RequirementItemDisplayOrder).DistinctBy(vdd => vdd.FieldId).ToList();
            List<RequirementVerificationDetailContract> lstDocTypeField = CurrentViewContext.lstItemLevelData.Where(cond => cond.FieldDataTypeCode == uploadDocTypeCode).OrderBy(vdd => vdd.RequirementItemDisplayOrder).DistinctBy(vdd => vdd.FieldId).ToList();

            foreach (RequirementVerificationDetailContract docTypeField in lstDocTypeField)
            {
                lstDstctFields.Add(docTypeField);
            }

            List<RequirementVerificationDetailContract> lstFieldsToAdd = lstDstctFields;

            // Maintains list of attributes that are already added
            List<Int32> lstAddedFieldIds = new List<Int32>();

            int _attributesAdded = 0;
            for (int i = 1; i <= Math.Ceiling(Convert.ToDecimal(lstDstctFields.Count()) / _attributesPerRow); i++)
            {
                if (_attributesAdded == _attributesPerRow)
                {
                    _attributesAdded = 0;
                    lstFieldsToAdd = new List<RequirementVerificationDetailContract>();

                    foreach (var att in lstDstctFields)
                    {
                        if (!lstAddedFieldIds.Contains(att.FieldId))
                        {
                            lstFieldsToAdd.Add(att);
                        }
                    }
                }
                lstFieldsToAdd = lstFieldsToAdd.Take(_attributesPerRow).ToList();
                lstAddedFieldIds.AddRange(lstFieldsToAdd.Select(att => att.FieldId));
                GenerateFieldRow(lstFieldsToAdd.Select(sel => sel.FieldId).ToList());
                _attributesAdded += _attributesPerRow;
            }
        }

        /// <summary>
        /// Generate the Field controls for the current item.
        /// </summary>
        private void GenerateFieldRow(List<Int32> _distinctFields)
        {
            var _comboTypeCode = RequirementFieldDataType.OPTIONS.GetStringValue();
            var _uploadDocTypeCode = RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue();

            System.Web.UI.HtmlControls.HtmlGenericControl dicRow = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            dicRow.Attributes.Add("class", "col-md-12");

            System.Web.UI.HtmlControls.HtmlGenericControl dicRow1 = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            dicRow1.Attributes.Add("class", "row");
            dicRow.Controls.Add(dicRow1);

            foreach (var fieldId in _distinctFields)
            {
                var _lstcurrentFieldData = CurrentViewContext.lstItemLevelData.Where(vdd => vdd.FieldId == fieldId).ToList();


                System.Web.UI.Control _fieldControl = Page.LoadControl("~/ClinicalRotation/UserControl/RequirementVerificationReadOnlyFieldControl.ascx");
                (_fieldControl as IRequirementVerificationReadOnlyFieldControlView).FieldData = CurrentViewContext.lstItemLevelData.Where(vdd => vdd.FieldId == fieldId).First();
                (_fieldControl as IRequirementVerificationReadOnlyFieldControlView).SelectedTenantId = CurrentViewContext.SelectedTenantId;
                (_fieldControl as RequirementVerificationReadOnlyFieldControl).ID = CurrentViewContext.FieldControlIdPrefix + fieldId;

                if (_lstcurrentFieldData.First().FieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue())
                {
                    #region Get Combobox Options

                    var _dic = new Dictionary<String, String>();

                    foreach (var comboItem in _lstcurrentFieldData)
                    {
                        _dic.Add(comboItem.OptionValue, comboItem.OptionText);
                    }
                    (_fieldControl as IRequirementVerificationReadOnlyFieldControlView).dicComboData = _dic;

                    #endregion
                }
                else if (_lstcurrentFieldData.First().FieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue())
                {
                    #region Get documents for the DocumentUploadType Field

                    var lstDocuments = new List<Tuple<Int32, String, String>>();

                    if (_lstcurrentFieldData.Any(dt => dt.ApplDocId > AppConsts.NONE))
                    {
                        foreach (var doc in _lstcurrentFieldData)
                        {
                            var _tplDocument = new Tuple<Int32, String, String>(doc.ApplDocId, doc.FieldDocName, doc.FieldDocPath);
                            lstDocuments.Add(_tplDocument);
                        }
                    }
                    (_fieldControl as IRequirementVerificationReadOnlyFieldControlView).lstDocuments = lstDocuments;

                    #endregion
                }
                else if (_lstcurrentFieldData.First().FieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
                {
                    #region UAT-1470 :As a student, there should be a way to close out of the video once you open it.
                    Int32 reqObjectTreeId = AppConsts.NONE;
                    String attObjectTypeCode = LCObjectType.ComplianceATR.GetStringValue();
                    String objectAttributeTypeCode = ObjectAttribute.REQUIRED.GetStringValue();
                    String boxOpentime = String.Empty;
                    if (CurrentViewContext.LstRequirementObjTreeProperty.IsNotNull())
                    {
                        var fieldProperty = CurrentViewContext.LstRequirementObjTreeProperty.FirstOrDefault(cond => cond.ObjectID == fieldId
                                                                                        && cond.ObjectTypeCode == attObjectTypeCode
                                                                                        && cond.ObjectAttributeTypeCode == objectAttributeTypeCode);

                        if (fieldProperty.IsNotNull())
                        {
                            reqObjectTreeId = fieldProperty.RequirementObjectTreeID;
                            boxOpentime = Presenter.GetObjectAttrProperties(reqObjectTreeId);
                        }
                    }

                    (_fieldControl as IRequirementVerificationReadOnlyFieldControlView).VideoRequiredOpenTime = boxOpentime;

                    #endregion

                }
                dicRow.Controls.Add(_fieldControl);
            }

            //System.Web.UI.HtmlControls.HtmlGenericControl dicRowEnd = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            //dicRowEnd.Attributes.Add("class", "sxroend");
            //dicRow.Controls.Add(dicRowEnd);
            divFieldContainer.Controls.Add(dicRow);
        }

        /// <summary>
        /// Bind the ItemStatus radio-buttons and enable/disable them based on condition
        /// </summary>
        /// <param name="value"></param>
        private void BindItemStatus(List<RequirementItemStatusContract> value)
        {
            var _lkpItemStatus = value.Where(ris => ris.ReqItemstatusCode != RequirementItemStatus.SUBMITTED.GetStringValue()).ToList();

            foreach (var itemStatus in _lkpItemStatus)
            {
                var _isEnabled = false;
                var _itemStsText = String.Empty;

                if (itemStatus.ReqItemstatusCode == RequirementItemStatus.NOT_APPROVED.GetStringValue())
                {
                    _itemStsText = "Rejected";
                }
                else
                {
                    _itemStsText = itemStatus.ReqItemstatusName;
                }
                rbtnListStatus.Items.Add(new ListItem
                {
                    Text = _itemStsText,
                    Value = itemStatus.ReqItemstatusCode,
                    Enabled = _isEnabled
                });
            }
            rbtnListStatus.SelectedValue = CurrentViewContext.lstItemLevelData.First().ItemStatusCode;
        }

        /// <summary>
        /// Get the ImageUrl for the ItemStatus icone
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private string GetImagePathItemStatus(string status)
        {
            if (status == RequirementItemStatus.INCOMPLETE.GetStringValue()
             || status == RequirementItemStatus.NOT_APPROVED.GetStringValue()
             || status == RequirementItemStatus.EXPIRED.GetStringValue())
            {
                return ResolveUrl("~/Resources/Mod/Compliance/icons/no16.png");
            }
            else if (status == RequirementItemStatus.APPROVED.GetStringValue())
            {
                return ResolveUrl("~/Resources/Mod/Compliance/icons/yes16.png");
            }
            else if (status == RequirementItemStatus.PENDING_REVIEW.GetStringValue())
            {
                return ResolveUrl("~/Resources/Mod/Compliance/icons/attn16.png");
            }
            return ResolveUrl("~/Resources/Mod/Compliance/icons/no16.png");
        }
        #endregion

        #endregion
    }
}