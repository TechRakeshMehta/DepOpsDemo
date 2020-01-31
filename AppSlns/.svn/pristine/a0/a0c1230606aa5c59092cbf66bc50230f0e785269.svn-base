using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceOperations.Views;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class DataEntryItem : BaseUserControl, IDataEntryItemView
    {
        /// <summary>
        /// Data which belong to the particular Item of the category
        /// </summary>
        public List<AdminDataEntryUIContract> ItemUIContract
        {
            get;
            set;
        }

        /// <summary>
        /// ComplianceItemId of the item being generated
        /// </summary>
        public Int32 ItemId
        {
            get;
            set;
        }

        /// <summary>
        /// ComplianceItemName of the item biong generated
        /// </summary>
        public String ItemName
        {
            get;
            set;
        }

        /// <summary>
        /// ComplianceCategoryId of the item to which the Item belongs to
        /// </summary>
        public Int32 CatId
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
                    return this.CatId + "_" + this.ItemId + "_" + this.ItemSeriesId;
                }
                else
                {
                    return this.CatId + "_" + this.ItemId;
                }
            }
        }

        public Boolean IfItemExpiryRule
        {
            get;
            set;
        }

        /// <summary>
        /// Master list of the Attribute, 
        /// based on which the main header was generated
        /// </summary>
        public List<AdminDataEntryUIContract> DistinctAttributes
        {
            get;
            set;
        }

        //UAT 2591:Indicator for items that have expiration rules on the Admin Data Entry screen
        public Boolean ifDataEntryAllowed
        {
            get;
            set;
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
            Dictionary<String, String> _dicAttributes = new Dictionary<String, String>();

            #region UAT-1608:Admin data entry screen[Shot Series]
            Boolean isItemSeries = false;
            Boolean isReadOnly = false;
            Boolean isDataEnteredForSeries = false;
            Boolean isFileUploadAttrExist = false;
            Boolean isEnabledDocAssForItemSeries = false;

            IsItemSeries = isItemSeries = this.ItemUIContract.Any(cond => cond.IsItemSeries);
            isReadOnly = this.ItemUIContract.Any(cond => cond.IsReadOnly);
            isDataEnteredForSeries = this.ItemUIContract.Any(x => x.AttrDataId != 0 && x.ItemId == this.ItemId);
            String fileUploadAttributeTypeCode = ComplianceAttributeDatatypes.FileUpload.GetStringValue();
            isFileUploadAttrExist = this.ItemUIContract.Any(x => x.AttrDataType == fileUploadAttributeTypeCode && x.ItemId == this.ItemId);
            if (isDataEnteredForSeries && isFileUploadAttrExist)
            {
                isEnabledDocAssForItemSeries = true;
            }
            #endregion

            _dicAttributes.Add("class", "borderB borderR");
            _dicAttributes.Add("align", "center");
            var _chkCol = GenerateHTML("td", _dicAttributes, String.Empty);

            CheckBox _chk = new CheckBox();
            _chk.ID = "chkSwap_" + this.CatId + "_" + this.ItemId;
            _chk.Attributes.Add("class", "chkSelection");

            //UAT-1608: UAT-1608:Admin data entry screen[Shot Series]
            if (IsItemSeries)
            {
                _chk.ID = "chkSwap_" + this.CatId + "_" + this.ItemId + "_" + this.ItemSeriesId;
            }
            _chk.Enabled = (isItemSeries || isReadOnly || !ifDataEntryAllowed) ? false : true;

            _chkCol.Controls.Add(_chk);
            pnlContainer.Controls.Add(_chkCol);

            var _chkDocColumn = GenerateHTML("td", _dicAttributes, String.Empty);

            CheckBox _chkDocAssociation = new CheckBox();
            _chkDocAssociation.ID = "chkDocAssociation_" + this.CatId + "_" + this.ItemId;
            _chkDocAssociation.CssClass = "chkDocAssociation";
            //_chkDocAssociation.Attributes.Add("cattr", "cattr_" + this.CatId + "_" + this.ItemId);


            //UAT-1608: UAT-1608:Admin data entry screen[Shot Series]
            if (IsItemSeries)
            {
                _chkDocAssociation.ID = "chkDocAssociation_" + this.CatId + "_" + this.ItemId + "_" + this.ItemSeriesId;
                _chkDocAssociation.Attributes.Add("OnClick", "ManageControlsEditablity('cattr_" + this.CatId + "_" + this.ItemId + "_" + this.ItemSeriesId + "','" + _chkDocAssociation.ID + "');");
                _chk.Attributes.Add("OnClick", "ManageControlOnSwap('cattr_" + this.CatId + "_" + this.ItemId + "_" + this.ItemSeriesId + "','" + _chkDocAssociation.ID + "','" + _chk.ID + "');");
            }
            else
            {
                _chkDocAssociation.Attributes.Add("OnClick", "ManageControlsEditablity('cattr_" + this.CatId + "_" + this.ItemId + "','" + _chkDocAssociation.ID + "');");
                _chk.Attributes.Add("OnClick", "ManageControlOnSwap('cattr_" + this.CatId + "_" + this.ItemId + "','" + _chkDocAssociation.ID + "','" + _chk.ID + "');");
            }
            _chkDocAssociation.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            _chkDocAssociation.Enabled = ifDataEntryAllowed;
            //UAT 1722
            //_chkDocAssociation.Enabled = (isItemSeries && !isEnabledDocAssForItemSeries) ? false : true;

            _chkDocColumn.Controls.Add(_chkDocAssociation);

            pnlContainer.Controls.Add(_chkDocColumn);

            _dicAttributes = new Dictionary<String, String>();
            if (this.ItemUIContract.Any(cond => cond.IsCurrentDocAssociated))
            {
                _dicAttributes.Add("class", "borderB borderR item_highlight");
            }
            else
            {
                //TODO: If Required in future to show Item Series in different color.
                //if (IsItemSeries)
                //{
                //    _dicAttributes.Add("class", "borderB borderR itemSeries_highlight");
                //}
                //else
                //{
                //    _dicAttributes.Add("class", "borderB borderR");
                //}
                _dicAttributes.Add("class", "borderB borderR");
            }
            _dicAttributes.Add("align", "center");
            var _itemCol = GenerateHTML("td", _dicAttributes, this.ItemName);
            //UAT 2591:Indicator for items that have expiration rules on the Admin Data Entry screen
            if (IfItemExpiryRule)
            {
             var span = new HtmlGenericControl("span");
                            span.InnerHtml = " (E) ";
                            span.Attributes.Add("title", "This item has expiry rule.");
                            span.Style.Add("color","Blue");
                            span.Style.Add("font-weight", "bold");
                            _itemCol.Controls.Add(span);        
            }

            pnlContainer.Controls.Add(_itemCol);

            _dicAttributes = new Dictionary<String, String>();
            _dicAttributes.Add("class", "borderB borderR");
            _dicAttributes.Add("align", "center");
            var _itemStatus = GenerateHTML("td", _dicAttributes, this.ItemUIContract.First().OldItemStatus);
            pnlContainer.Controls.Add(_itemStatus);

            // Use the same List which was used to generated the headers in parent control
            foreach (var attr in this.DistinctAttributes)
            {
                GenerateItemAttributes(attr);
            }
            SetIds();
        }

        /// <summary>
        /// Set the Required Id's in the hidden fields, like CategoryId, ItemId, CategoryDataId, ItemDataId 
        /// </summary>
        private void SetIds()
        {
            hdfCatId.Value = Convert.ToString(this.CatId);
            hdfItemId.Value = Convert.ToString(this.ItemId);
            hdfNewStatusCode.Value = hdfOldStatusCode.Value = ItemUIContract.First().OldItemStatusCode;

            hdfCatDataId.Value = Convert.ToString(this.ItemUIContract.First().CatDataId);
            hdfItemDataId.Value = Convert.ToString(this.ItemUIContract.First().ItemDataId);
            #region UAT-1608
            hdfItemSeriesID.Value = Convert.ToString(this.ItemSeriesId);
            #endregion
        }


        /// <summary>
        /// Generate the Attribute level controls
        /// </summary>
        /// <param name="_dicAttributes"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        private void GenerateItemAttributes(AdminDataEntryUIContract attr)
        {
            Dictionary<String, String> _dicAttributes = new Dictionary<String, String>();
            _dicAttributes.Add("class", "borderB borderR");
            _dicAttributes.Add("align", "center");
            HtmlGenericControl _attrCol = GenerateHTML("td", _dicAttributes, String.Empty);

            // Set Attributeid to that of Attribute being navigated
            // If this were a Non-grouped attribute being used, then, 
            // '_attributeId' will become attribute ID, else
            // override with the AttributeId of the attribute which belongs to tha current Attribute group 
            // being navigated in the next condition below
            // This is the actual AttributeID
            var _attributeId = attr.AttrId;

            var _attributeGrpId = AppConsts.NONE;
            if (attr.IsGrouped)
            {
                // Get the Attribute for the Item, which belongs to the current Attribute group being used to create the cell
                var _groupedAttr = this.ItemUIContract.Where(uic => uic.AttrGroupId == attr.AttrGroupId
                                                                 && uic.ItemId == this.ItemId
                                                                 && uic.IsGrouped).FirstOrDefault();

                // Override the Attribute-Id & AttrGroupId, 
                // with that of a grouped attribute in the current Item being iterated,
                // if currently a Grouped attribute is being iterated
                if (_groupedAttr.IsNotNull())
                {
                    _attributeId = _groupedAttr.AttrId;
                    _attributeGrpId = _groupedAttr.AttrGroupId;
                }
            }

            var _attrType = this.ItemUIContract.Where(uic => uic.AttrId == _attributeId).FirstOrDefault();

            if (_attrType.IsNotNull())
            {
                System.Web.UI.Control _attrControl = Page.LoadControl("~/ComplianceOperations/UserControl/DataEntryAttribute.ascx");
                (_attrControl as DataEntryAttribute).AttributeUIContract = this.ItemUIContract.Where(uic => uic.CatId == this.CatId
                                                                                                    && uic.ItemId == this.ItemId
                                                                                                    && uic.AttrId == _attributeId).ToList();

                //&& (!this.IsItemSeries
                //   || (this.IsItemSeries
                //      && uic.ItemSeriesItemID == this.ItemSeriesItemId))

                if (!(_attrControl as DataEntryAttribute).AttributeUIContract.IsNullOrEmpty())
                {
                    _attrControl.ID = "ucDataEntryAttribute_" + this.ControlIdSuffix + "_" + _attributeId;
                    (_attrControl as DataEntryAttribute).CatId = this.CatId;
                    (_attrControl as DataEntryAttribute).ItemId = this.ItemId;
                    (_attrControl as DataEntryAttribute).AttributeGroupId = _attributeGrpId;
                    (_attrControl as DataEntryAttribute).ItemSeriesId = this.ItemSeriesId;
                    (_attrControl as DataEntryAttribute).IsItemSeries = this.IsItemSeries;
                    _attrCol.Controls.Add(_attrControl);
                }
            }

            pnlContainer.Controls.Add(_attrCol);
        }
        /// <summary>
        /// Generate HTML type control
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dicAttributes"></param>
        /// <param name="innerText"></param>
        /// <returns></returns>
        private HtmlGenericControl GenerateHTML(String htmlTagType, Dictionary<String, String> dicAttributes, String innerText)
        {
            HtmlGenericControl _htmlControl = new HtmlGenericControl(htmlTagType);

            foreach (var attr in dicAttributes)
            {
                _htmlControl.Attributes.Add(attr.Key, attr.Value);
            }
            _htmlControl.InnerText = innerText;
            return _htmlControl;
        }

        /// <summary>
        /// Gets the Data of the Item to be saved
        /// </summary>
        public ApplicantCmplncItemData GetItemDataToSave()
        {
            var chkDocAssociation = this.IsItemSeries ? (pnlContainer.FindControl("chkDocAssociation_" + this.CatId + "_" + this.ItemId + "_" + this.ItemSeriesId) as CheckBox)
                                                       : (pnlContainer.FindControl("chkDocAssociation_" + this.CatId + "_" + this.ItemId) as CheckBox);

            //UAT-1722: Pick the Item for Save Operation, only if the Checkbox for 'Update Item' as checked
            if (chkDocAssociation.IsNull() || !chkDocAssociation.Checked)
            {
                return null;
            }

            var _itemData = new ApplicantCmplncItemData();
            _itemData.ItmId = Convert.ToInt32(hdfItemId.Value);
            _itemData.AcidId = Convert.ToInt32(hdfItemDataId.Value);
            _itemData.OldStatusCode = hdfOldStatusCode.Value;
            _itemData.NewStatuscode = hdfNewStatusCode.Value;
            //UAT 2528
            _itemData.IsUiRulesViolate = false;

            #region UAT-1608

            _itemData.ItemSeriesID = Convert.ToInt32(hdfItemSeriesID.Value);

            #endregion

            _itemData.IsDocAssociationReq = chkDocAssociation.IsNotNull() && chkDocAssociation.Checked ? true : false;

            _itemData.SwappedItmId = String.IsNullOrEmpty(hdfSwappedItemId.Value) ? AppConsts.NONE : Convert.ToInt32(hdfSwappedItemId.Value);

            _itemData.IsItemSwapped = _itemData.SwappedItmId > AppConsts.NONE && _itemData.SwappedItmId != _itemData.ItmId
                                        ? true
                                        : false;

            var _lstDistinctAttr = this.ItemUIContract.Where(uic => uic.CatId == this.CatId
                                                    && uic.ItemId == this.ItemId)
                                                   .GroupBy(grp => grp.AttrId)
                                                   .Select(sel => sel.First())
                                                   .ToList();

            var _attrFilled = AppConsts.NONE;

            var _lstAttributes = new List<ApplicantCmplncAttrData>();

            #region UAT-1608

            Boolean isValueChanged = false;

            #endregion

            foreach (var attr in _lstDistinctAttr)
            {
                var _attrCtrl = pnlContainer.FindServerControlRecursively("ucDataEntryAttribute_" + this.ControlIdSuffix + "_" + attr.AttrId);
                if (_attrCtrl.IsNotNull() && _attrCtrl is DataEntryAttribute)
                {
                    var _currentAttributeData = (_attrCtrl as DataEntryAttribute).GetAttributeData();

                    #region UAT-1608
                    if (this.IsItemSeries && !isValueChanged)
                    {
                        isValueChanged = (_attrCtrl as DataEntryAttribute).IsValueChanged("hdfEnteredData", _currentAttributeData.AttrValue);
                    }
                    #endregion

                    _lstAttributes.Add(_currentAttributeData);
                    // If attribute is enetered
                    // OR attribute is of Options type and Value selected != 0
                    if (
                        (
                           !_currentAttributeData.AttrValue.IsNullOrEmpty() && (_attrCtrl as DataEntryAttribute).AttributeDataType != ComplianceAttributeDatatypes.Options.GetStringValue()
                        )
                        ||
                        (
                         _currentAttributeData.AttrValue != AppConsts.ZERO && (_attrCtrl as DataEntryAttribute).AttributeDataType == ComplianceAttributeDatatypes.Options.GetStringValue())
                        )
                        _attrFilled += 1;
                }
            }
            #region UAT-1608

            _itemData.IsDataChanged = isValueChanged;
            //_itemData.IsDataChanged = chkDocAssociation.Checked;

            #endregion

            _itemData.AttribuetFilledCount = _attrFilled;

            // UAT-1722: Admin Data Entry Screen enable data entry only when confirming with checkbox	
            // If any of the Attributes is filled, then add the data to the List
            //if (_attrFilled > 0 || (_attrFilled == AppConsts.NONE && _itemData.IsDocAssociationReq))
            //{
            _itemData.ApplicantCmplncAttrData = new List<ApplicantCmplncAttrData>();
            _itemData.ApplicantCmplncAttrData.AddRange(_lstAttributes);
            //}
            hdfSwappedItemId.Value = String.Empty;
            return _itemData;
        }
    }
}