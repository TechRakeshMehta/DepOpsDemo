using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class SeriesDoseRowControl : BaseUserControl, ISeriesDoseRowControlView
    {
        #region Private Variables

        private Delegate _delAddRow;
        private Delegate _delRemoveRow;

        private Delegate _delMoveUpRow;
        private Delegate _delMoveDownRow;

        private SeriesDoseRowControlPresenter _presenter = new SeriesDoseRowControlPresenter();

        #endregion

        #region Properties

        #region Public Properties

        public SeriesDoseRowControlPresenter Presenter
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

        /// <summary>
        /// List of all the Attributes i.e. Attributes of the Series as well as the normal Items
        /// </summary>
        public List<SeriesAttributeContract> _lstSeriesAttributeContract
        {
            get;
            set;
        }

        public Int32 ItemSeriesItemOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the Row Control is for Series or normal Items.
        /// </summary>
        public Boolean IsSeriesLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Data related to all the Items in the Series. 
        /// Helps to identify the which ComplianceAttributeID is to be set as selected in Edit Mode, for the combobox.
        /// </summary>
        public List<SeriesItemContract> lstSeriesItemContract
        {
            get;
            set;
        }

        /// <summary>
        /// ComplianceItemId for which the Row is being generated. Will be NULL for Series level.
        /// </summary>
        public Int32? CIId
        {
            get;
            set;
        }

        /// <summary>
        /// ItemSeriesItemID of the current ComplianceItem, for which the Row is being generated. Will be NULL for Series level.
        /// </summary>
        public Int32? ItemSeriesItemId
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the Current LoggedInUser
        /// </summary>
        Int32 ISeriesDoseRowControlView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        /// <summary>
        /// List of Possible status types after shuffling of items
        /// </summary>
        public List<lkpItemStatusPostDataShuffle> lstStatusTypes
        {
            get;
            set;
        }

        public Delegate DelegateAddRow
        {
            set
            {
                _delAddRow = value;
            }
        }

        public Delegate DelegateRemoveRow
        {
            set
            {
                _delRemoveRow = value;
            }
        }

        public Delegate DelegateMoveUpRow
        {
            set
            {
                _delMoveUpRow = value;
            }
        }

        public Delegate DelegateMoveDownRow
        {
            set
            {
                _delMoveDownRow = value;
            }
        }

        public Int32 SeriesId
        {
            get;
            set;
        }

        public Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// UniqueIdentifier for the Item Rows, sepecially for Temporarily added rows.
        /// </summary>
        public Guid UniqueIdentifier
        {
            get;
            set;
        }

        /// <summary>
        ///  
        /// </summary>
        public Boolean DisplayMoveDownButton
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        private String _seriesDoseAttributeControlIdPrefix
        {
            get
            {
                return "SeriesDoseAttributeControl_" + this.ItemSeriesItemId;
            }
        }

        private String _itemStatusComboIdPrefix
        {
            get
            {
                return "comboStatus_" + this.ItemSeriesItemId;
            }
        }

        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<String, String> _dicHtmlAttributes = new Dictionary<String, String>();

            _dicHtmlAttributes = new Dictionary<String, String>();

            if (IsSeriesLevel)
            {
                _dicHtmlAttributes.Add("style", "border: solid 1px Black;width:15px;text-align:center;height:35px;background-color:#C6C5C5");
            }
            else
            {
                _dicHtmlAttributes.Add("style", "border: solid 1px Black;width:15px;text-align:center;height:35px");
            }

            //var _rowSNoCol1 = GenerateHTML("td", _dicHtmlAttributes, this.ItemSeriesItemOrder == AppConsts.NONE ? String.Empty : Convert.ToString(this.ItemSeriesItemOrder));
            var _rowSNoCol1 = GenerateHTML("td", _dicHtmlAttributes, String.Empty);
            Literal _litDisplayOrder = new Literal();
            _litDisplayOrder.Text = this.ItemSeriesItemOrder == AppConsts.NONE ? String.Empty : Convert.ToString(this.ItemSeriesItemOrder);
            _litDisplayOrder.ID = "litSDisplayOrder_" + this.UniqueIdentifier;
            _rowSNoCol1.Controls.Add(_litDisplayOrder);
            pnlRowContainer.Controls.Add(_rowSNoCol1);

            _dicHtmlAttributes = new Dictionary<String, String>();
            if (IsSeriesLevel)
            {
                _dicHtmlAttributes.Add("style", "border: solid 1px Black;height:35px;background-color:#C6C5C5;text-align:center");
            }
            else
            {
                _dicHtmlAttributes.Add("style", "border: solid 1px Black;height:30px;padding: 10px");
            }
            var _rowBlankCol2 = GenerateHTML("td", _dicHtmlAttributes, IsSeriesLevel ? lstSeriesItemContract.First().ItemSeriesName : lstSeriesItemContract.First().CmpItemName);

            pnlRowContainer.Controls.Add(_rowBlankCol2);

            _dicHtmlAttributes.Add("align", "center");
            _dicHtmlAttributes.Add("width", "90px");

            // List of attributes which were selected in the Series creation.
            // Column level loop must be based on this only, so that the attribtues for
            // both Series and Items rows are generated in sync with each other.
            List<SeriesAttributeContract> _lstSeriesAttribute = _lstSeriesAttributeContract.Where(attr => attr.IsSeriesAttribute == true)
                                                                                           .OrderByDescending(attr => attr.IsKeyAttribute)
                                                                                           .ThenBy(attr => attr.CmpAttributeId).ToList();

            if (IsSeriesLevel)
            {
                GenerateSeriesColumns(_dicHtmlAttributes, _lstSeriesAttribute);
            }
            else
            {
                GenerateItemColumns(_dicHtmlAttributes, _lstSeriesAttribute);
            }
        }

        /// <summary>
        /// Event to Move the Item Down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _btnDown_Click(object sender, EventArgs e)
        {
            object[] _objParameters = new object[1];
            _objParameters[0] = this.UniqueIdentifier;
            _delMoveDownRow.DynamicInvoke(_objParameters);
        }

        /// <summary>
        /// Event to Move the Item Up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _btnUp_Click(object sender, EventArgs e)
        {
            object[] _objParameters = new object[1];
            _objParameters[0] = this.UniqueIdentifier;
            _delMoveUpRow.DynamicInvoke(_objParameters);
        }

        /// <summary>
        /// Event to Remove the Item from table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _btnRemove_Click(object sender, EventArgs e)
        {
            object[] _objParameters = new object[1];
            _objParameters[0] = this.UniqueIdentifier;
            _delRemoveRow.DynamicInvoke(_objParameters);
        }

        /// <summary>
        /// Event to Add new item to the table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _btnAdd_Click(object sender, EventArgs e)
        {
            object[] _objParameters = new object[1];
            _objParameters[0] = this.CIId;
            _delAddRow.DynamicInvoke(_objParameters);
        }

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// Generate columns for the normal Item row.
        /// </summary>
        /// <param name="_dicHtmlAttributes"></param>
        /// <param name="_lstSeriesAttribute"></param>
        private void GenerateItemColumns(Dictionary<String, String> _dicHtmlAttributes, List<SeriesAttributeContract> _lstSeriesAttribute)
        {
            foreach (var attribute in _lstSeriesAttribute)
            {
                var _itemOtherAttributeCol4 = GenerateHTML("td", _dicHtmlAttributes, String.Empty);
                Control seriesOtherAttributeControl = Page.LoadControl("~/ComplianceAdministration/UserControl/SeriesDoseAttributeControl.ascx");

                (seriesOtherAttributeControl as SeriesDoseAttributeControl).ID = _seriesDoseAttributeControlIdPrefix + attribute.CmpAttributeId;
                (seriesOtherAttributeControl as SeriesDoseAttributeControl)._lstAttributes = _lstSeriesAttributeContract.Where(attr => attr.IsKeyAttribute == false
                                                                                                                                    && attr.IsSeriesAttribute == false
                                                                                                                                    && attr.CmpItemId == CIId).ToList();
                (seriesOtherAttributeControl as SeriesDoseAttributeControl).ItemSeriesAttributeId = attribute.ItemSeriesAttributeId;

                var _currentMappedAttribute = lstSeriesItemContract.Where(sic => sic.ItemSeriesItemId == this.ItemSeriesItemId && sic.ISAM_ItemSeriesAttrId == attribute.ItemSeriesAttributeId).FirstOrDefault();

                if (_currentMappedAttribute.IsNotNull())
                {
                    (seriesOtherAttributeControl as SeriesDoseAttributeControl).SelectedAttributeId = _currentMappedAttribute.SelectedAttributeId;
                }

                _itemOtherAttributeCol4.Controls.Add(seriesOtherAttributeControl);
                pnlRowContainer.Controls.Add(_itemOtherAttributeCol4);
            }

            var _itemStatusCol5 = GenerateHTML("td", _dicHtmlAttributes, String.Empty);

            WclComboBox _comboStatus = new WclComboBox();
            _comboStatus.DataSource = lstStatusTypes;
            _comboStatus.DataTextField = "ISPDS_Name";
            _comboStatus.DataValueField = "ISPDS_Code";
            _comboStatus.DataBind();
            _comboStatus.ID = _itemStatusComboIdPrefix;

            if (this.ItemSeriesItemId != AppConsts.NONE)
            {
                _comboStatus.SelectedValue = lstSeriesItemContract.Where(sic => sic.ItemSeriesItemId == this.ItemSeriesItemId).First().PostShuffleStatusCode; ;
            }
            _itemStatusCol5.Controls.Add(_comboStatus);

            pnlRowContainer.Controls.Add(_itemStatusCol5);

            var _itemActionCol6 = GenerateHTML("td", _dicHtmlAttributes, String.Empty);

            WclButton _btnAdd = GenerateAddButton();
            _itemActionCol6.Controls.Add(_btnAdd);

            WclButton _btnRemove = GenerateRemoveButton();
            _itemActionCol6.Controls.Add(_btnRemove);

            GenerateUpDownButtons(_itemActionCol6);

            pnlRowContainer.Controls.Add(_itemActionCol6);
        }

        /// <summary>
        /// Generate the AddItem Button
        /// </summary>
        /// <returns></returns>
        private WclButton GenerateRemoveButton()
        {
            var _enableDelete = lstSeriesItemContract.Where(sic => sic.CmpItemId == this.CIId)
                                                 .GroupBy(sic => sic.UniqueIdentifier)
                                                 .Count() > AppConsts.ONE
                                                ? true : false;

            WclButton _btnRemove = new WclButton();
            _btnRemove.Text = String.Empty;
            _btnRemove.Icon.PrimaryIconCssClass = "rbPrimaryIcon rbCancel";
            _btnRemove.BorderStyle = BorderStyle.None;
            _btnRemove.ButtonType = RadButtonType.LinkButton;
            _btnRemove.Click += _btnRemove_Click;
            _btnRemove.Width = 15;
            _btnRemove.Visible = _enableDelete;
            _btnRemove.ID = "btnRemove_" + this.CIId;
            return _btnRemove;
        }

        /// <summary>
        /// Generate the RemoveItem Button
        /// </summary>
        /// <returns></returns>
        private WclButton GenerateAddButton()
        {
            WclButton _btnAdd = new WclButton();
            _btnAdd.Text = String.Empty;
            _btnAdd.Click += _btnAdd_Click;
            _btnAdd.Icon.PrimaryIconCssClass = "rbPrimaryIcon rbAdd";
            _btnAdd.BorderStyle = BorderStyle.None;
            _btnAdd.ButtonType = RadButtonType.LinkButton;
            _btnAdd.Width = 15;
            return _btnAdd;
        }

        /// <summary>
        /// Generate the Up/Down links
        /// </summary>
        /// <param name="actionColumn"></param>
        private void GenerateUpDownButtons(HtmlGenericControl actionColumn)
        {
            WclButton _btnUp = new WclButton();
            _btnUp.Icon.PrimaryIconCssClass = "rbPrimaryIcon upArrow";
            _btnUp.BorderStyle = BorderStyle.None;
            _btnUp.ButtonType = RadButtonType.LinkButton;
            _btnUp.Click += _btnUp_Click;
            _btnUp.Width = 15;
            _btnUp.Visible = this.ItemSeriesItemOrder == AppConsts.ONE ? false : true;
            _btnUp.ID = "_btnUp_" + this.UniqueIdentifier;

            actionColumn.Controls.Add(_btnUp);

            WclButton _btnDown = new WclButton();
            _btnDown.Icon.PrimaryIconCssClass = "rbPrimaryIcon downArrow";
            _btnDown.BorderStyle = BorderStyle.None;
            _btnDown.ButtonType = RadButtonType.LinkButton;
            _btnDown.Click += _btnDown_Click;
            _btnDown.Width = 15;
            _btnDown.ID = "_btnDown_" + this.UniqueIdentifier;

            // Add the 'Move Down' button if this is NOT the last row to be generated
            _btnDown.Visible = DisplayMoveDownButton;
            actionColumn.Controls.Add(_btnDown);
        }

        /// <summary>
        /// Generate Columns for the Series Row.
        /// </summary>
        /// <param name="_dicHtmlAttributes"></param>
        /// <param name="_lstSeriesAttribute"></param>
        private void GenerateSeriesColumns(Dictionary<String, String> _dicHtmlAttributes, List<SeriesAttributeContract> _lstSeriesAttribute)
        {
            var _seriesKeyAttributeCol3 = GenerateHTML("td", _dicHtmlAttributes, _lstSeriesAttributeContract.Where(att => att.IsKeyAttribute && att.IsSeriesAttribute).First().CmpAttributeName);

            foreach (var attribute in _lstSeriesAttribute)
            {

                var _seriesAttributeCol4 = GenerateHTML("td", _dicHtmlAttributes, String.Empty);
                Control seriesAttributeControl = Page.LoadControl("~/ComplianceAdministration/UserControl/SeriesDoseAttributeControl.ascx");

                (seriesAttributeControl as SeriesDoseAttributeControl).IsSeriesLevel = true;
                (seriesAttributeControl as SeriesDoseAttributeControl)._lstAttributes = _lstSeriesAttribute;
                (seriesAttributeControl as SeriesDoseAttributeControl).KeyAttributeName = attribute.CmpAttributeName;
                (seriesAttributeControl as SeriesDoseAttributeControl).ItemSeriesAttributeId = attribute.ItemSeriesAttributeId;
                _seriesAttributeCol4.Controls.Add(seriesAttributeControl);
                pnlRowContainer.Controls.Add(_seriesAttributeCol4);
            }

            var _sreiesStatusCol5 = GenerateHTML("td", _dicHtmlAttributes, String.Empty);
            pnlRowContainer.Controls.Add(_sreiesStatusCol5);

            var _seriesActionCol6 = GenerateHTML("td", _dicHtmlAttributes, String.Empty);
            pnlRowContainer.Controls.Add(_seriesActionCol6);
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
        #endregion

        #region Public Methods

        /// <summary>
        /// Get data of all the attributes of the Row
        /// </summary>
        /// <returns></returns>
        public List<SeriesItemContract> GetRowData(Int32 seriesId, Int32 displayOrder)
        {
            List<SeriesItemContract> _lstRowData = new List<SeriesItemContract>();

            var _lstAttributes = _lstSeriesAttributeContract.Where(attr => attr.IsSeriesAttribute == true).ToList();
            var _statusCombo = pnlRowContainer.FindServerControlRecursively(_itemStatusComboIdPrefix);

            Int32 _retainStatusId = lstStatusTypes.Where(st => st.ISPDS_Code == ItemStatusPostDataShuffle.RETAIN.GetStringValue()).First().ISPDS_ID;
            Int32 _pendingReviewStatusId = lstStatusTypes.Where(st => st.ISPDS_Code == ItemStatusPostDataShuffle.PENDING_REVIEW.GetStringValue()).First().ISPDS_ID;

            foreach (var attr in _lstAttributes.OrderByDescending(attr => attr.IsKeyAttribute).ThenBy(attr => attr.CmpAttributeId).ToList())
            {
                var _attributeControl = pnlRowContainer.FindServerControlRecursively(_seriesDoseAttributeControlIdPrefix + attr.CmpAttributeId);

                if (_attributeControl.IsNotNull() && _statusCombo.IsNotNull())
                {
                    var _cmbAttributes = (_attributeControl as SeriesDoseAttributeControl).FindServerControlRecursively("combo_" + attr.ItemSeriesAttributeId);

                    if (_cmbAttributes.IsNotNull() && (_cmbAttributes as WclComboBox).SelectedValue != AppConsts.ZERO)
                    {
                        var _currentMappedAttribute = lstSeriesItemContract.Where(sic => sic.ItemSeriesItemId == this.ItemSeriesItemId && sic.ISAM_ItemSeriesAttrId == attr.ItemSeriesAttributeId).FirstOrDefault();

                        _lstRowData.Add(new SeriesItemContract
                        {
                            CmpItemId = Convert.ToInt32(this.CIId),
                            UniqueIdentifier = this.UniqueIdentifier,
                            ItemSeriesId = seriesId,
                            ItemSeriesItemOrder = displayOrder,
                            ItemSeriesItemId = Convert.ToInt32(this.ItemSeriesItemId),
                            SelectedAttributeId = Convert.ToInt32((_cmbAttributes as WclComboBox).SelectedValue),
                            ISAM_ItemSeriesAttrId = attr.ItemSeriesAttributeId,
                            PostShuffleStatusId = (_statusCombo as WclComboBox).SelectedValue == ItemStatusPostDataShuffle.RETAIN.GetStringValue()
                                                                ? _retainStatusId
                                                                : _pendingReviewStatusId,
                            ItemSeriesAttributeMapId = _currentMappedAttribute.IsNotNull() ? _currentMappedAttribute.ItemSeriesAttributeMapId : AppConsts.NONE
                        });
                    }
                }
            }
            return _lstRowData;
        }

        #endregion

        #endregion
    }
}