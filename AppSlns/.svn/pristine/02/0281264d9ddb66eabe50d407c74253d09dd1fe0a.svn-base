using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceAdministration.Views;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class SeriesDose : BaseUserControl, ISeriesDoseView
    {
        #region Variables

        #region Private Variables

        private SeriesDosePresenter _presenter = new SeriesDosePresenter();

        private delegate void DelegateAddNewRow(Int32 itemId);
        private delegate void DelegateRemoveRow(Guid uniqueIdentifier);

        private delegate void DelegateMoveUpRow(Guid uniqueIdentifier);
        private delegate void DelegateMoveDownRow(Guid uniqueIdentifier);

        /// <summary>
        /// Mapping Table generated
        /// </summary>
        private HtmlGenericControl _table;

        /// <summary>
        /// Attributes for table, rows and it's columns
        /// </summary>
        Dictionary<String, String> _dicHtmlAttributes;

        /// <summary>
        /// Key is the UniqueIdentifier and Value is the Display Order
        /// </summary>
        Dictionary<Guid, Int32> _dicList;

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Private Properties

        /// <summary>
        /// ID of the 'SeriesDose.ascx' control
        /// </summary>
        private String _seriesDoseRowControlIdPrefix
        {
            get
            {
                return "SeriesDoseRowControl_";
            }
        }

        #endregion

        #region Properties

        public SeriesDosePresenter Presenter
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

        public Int32 SeriesId { get; set; }

        public Int32 SelectedTenantId { get; set; }

        public ISeriesDoseView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        List<SeriesItemContract> ISeriesDoseView.lstSeriesItemContract
        {
            get
            {
                return Session[AppConsts.SHOTSERIES_SESSION_LISTITEMCONTRACT].IsNull()
                    ? new List<SeriesItemContract>()
                    : Session[AppConsts.SHOTSERIES_SESSION_LISTITEMCONTRACT] as List<SeriesItemContract>;
            }
            set
            {
                Session[AppConsts.SHOTSERIES_SESSION_LISTITEMCONTRACT] = value;
            }
        }

        List<SeriesAttributeContract> ISeriesDoseView.lstSeriesAttributeContract
        {
            get
            {
                return Session[AppConsts.SHOTSERIES_SESSION_LISTATTRIBUTESCONTRACT].IsNull()
                    ? new List<SeriesAttributeContract>()
                    : Session[AppConsts.SHOTSERIES_SESSION_LISTATTRIBUTESCONTRACT] as List<SeriesAttributeContract>;
            }
            set
            {
                Session[AppConsts.SHOTSERIES_SESSION_LISTATTRIBUTESCONTRACT] = value;
            }
        }

        /// <summary>
        /// List of Possible status types after shuffling of items
        /// </summary>
        List<lkpItemStatusPostDataShuffle> ISeriesDoseView.lstStatusTypes
        {
            get;
            set;
        }

        /// <summary>
        /// List to Save/Update the data in database
        /// </summary>
        List<SeriesItemContract> ISeriesDoseView.lstSeriesItemContractSaveUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the Current LoggedInUser
        /// </summary>
        Int32 ISeriesDoseView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            GenerateMappingTable();

            //Load Series UnMapped Attributes user control
            if (Presenter.CheckIfSeriesMappedAttrExist())
                LoadSeriesUnMappedAttributes();
        }

        /// <summary>
        /// Command bar event to save Mapping defined in the table
        /// </summary>
        protected void fsucCmdBarMapping_SaveClick(object sender, EventArgs e)
        {
            try
            {
                SaveData();
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

        public void SaveData()
        {
            CurrentViewContext.lstSeriesItemContractSaveUpdate = new List<SeriesItemContract>();
            List<Int32> _lstDistinctItemIds = CurrentViewContext.lstSeriesItemContract.OrderBy(x => x.ItemSeriesItemOrder).Select(itm => itm.ItemSeriesItemId).Distinct().ToList();

            foreach (var dicItem in _dicList)
            {
                var itemRowControl = pnlRows.FindServerControlRecursively(_seriesDoseRowControlIdPrefix + dicItem.Key);
                if (itemRowControl.IsNotNull())
                {
                    CurrentViewContext.lstSeriesItemContractSaveUpdate.AddRange((itemRowControl as SeriesDoseRowControl).GetRowData(CurrentViewContext.SeriesId, dicItem.Value));
                }
            }

            if (!CurrentViewContext.lstSeriesItemContractSaveUpdate.IsNullOrEmpty())
            {
                Presenter.SaveUpdateSeriesMapping();

                Session.Remove(AppConsts.SHOTSERIES_SESSION_LISTATTRIBUTESCONTRACT);
                Session.Remove(AppConsts.SHOTSERIES_SESSION_LISTITEMCONTRACT);

                pnlRows.Controls.Clear();

                // Rebind the UI so that newly generated ID's get assigned to the controls.
                GenerateMappingTable();
                (this.Page as BaseWebPage).ShowSuccessMessage("Attribute Mapping for Series saved successfully.");

                //Load Series UnMapped Attributes user control
                if (Presenter.CheckIfSeriesMappedAttrExist())
                    LoadSeriesUnMappedAttributes();
            }
            else
            {
                (this.Page as BaseWebPage).ShowInfoMessage("Please select mapping for atleast one Item.");
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// Generate the Mapping Table
        /// </summary>
        private void GenerateMappingTable()
        {
            if (Session[AppConsts.SHOTSERIES_SESSION_LISTITEMCONTRACT].IsNull())
            {
                Presenter.GetSeriesDetails();
            }

            Presenter.GetPostShuffleStatusList();

            #region Generate Table and Header row

            _dicHtmlAttributes = new Dictionary<String, String>();

            _dicHtmlAttributes.Add("width", "100%");
            _dicHtmlAttributes.Add("border", "0");
            _dicHtmlAttributes.Add("cellspacing", "0");
            _dicHtmlAttributes.Add("cellpadding", "0");
            _dicHtmlAttributes.Add("id", "tblMappingTable");
            _table = GenerateHTML("table", _dicHtmlAttributes, String.Empty);

            var _headerRow = GenerateHeader();
            _table.Controls.Add(_headerRow);

            #endregion

            #region Generate Series Row

            _dicHtmlAttributes = new Dictionary<String, String>();

            _dicHtmlAttributes.Add("width", "100%");
            _dicHtmlAttributes.Add("border", "0");
            _dicHtmlAttributes.Add("cellspacing", "0");
            _dicHtmlAttributes.Add("cellpadding", "0");
            GenerateSeriesRow();

            #endregion

            #region Generate Item Rows

            // Get Distinct List of Items from the Database list
            _dicList = GenerateBaseList();
            _dicHtmlAttributes.Add("style", "background-color:white");
            foreach (var dicItem in _dicList.OrderBy(dic => dic.Value).ToList())
            {
                var _currentItemSeriesItem = CurrentViewContext.lstSeriesItemContract.Where(isic => isic.UniqueIdentifier == dicItem.Key).First();

                AddRow(_currentItemSeriesItem.ItemSeriesItemId, _currentItemSeriesItem.CmpItemId, _currentItemSeriesItem.ItemSeriesItemOrder,
                       _currentItemSeriesItem.UniqueIdentifier);
            }

            #endregion

            pnlRows.Controls.Add(_table);
        }

        /// <summary>
        /// Get the Distinct dictionary list of the Items which are fetched from database as well as added temporarily from the screen.
        /// Key is the uniqueidentifier and Value is the Display Order.
        /// </summary>
        /// <returns></returns>
        private Dictionary<Guid, Int32> GenerateBaseList()
        {
            Dictionary<Guid, Int32> _dic = new Dictionary<Guid, Int32>();

            var _lstDB = CurrentViewContext.lstSeriesItemContract.Where(sic => sic.ItemSeriesItemId != AppConsts.NONE)
                                                                                    .OrderBy(sic => sic.ItemSeriesItemOrder)
                                                                                    .Select(itm => new { uid = itm.UniqueIdentifier, displayOrder = itm.ItemSeriesItemOrder })
                                                                                    .Distinct().ToList();

            foreach (var item in _lstDB)
            {
                _dic.Add(item.uid, item.displayOrder);
            }

            var _lstTemp = CurrentViewContext.lstSeriesItemContract.Where(x => x.ItemSeriesItemId == AppConsts.NONE)
                                                    .OrderBy(sic => sic.ItemSeriesItemOrder)
                                                    .Select(itm => new { uid = itm.UniqueIdentifier, displayOrder = itm.ItemSeriesItemOrder })
                                                    .ToList();

            foreach (var item in _lstTemp)
            {
                _dic.Add(item.uid, item.displayOrder);
            }
            return _dic;
        }

        /// <summary>
        /// Generate a new Row level control and add it to the table.
        /// </summary>
        /// <param name="isiId"></param>
        /// <param name="_itemId"></param>
        /// <param name="itemSeriesItemOrder"></param>
        /// <param name="uniqueIdentifier"></param>
        /// <param name="isTempRow"></param>
        private void AddRow(Int32 isiId, Int32 _itemId, Int32 itemSeriesItemOrder, Guid uniqueIdentifier)
        {
            _dicHtmlAttributes.Add("id", Convert.ToString("tr_" + uniqueIdentifier));
            _dicHtmlAttributes.Add("runat", "server");

            var _trItemRow = GenerateHTML("tr", _dicHtmlAttributes, String.Empty);
            Control itemRowControl = Page.LoadControl("~/ComplianceAdministration/UserControl/SeriesDoseRowControl.ascx");

            (itemRowControl as SeriesDoseRowControl).lstSeriesItemContract = CurrentViewContext.lstSeriesItemContract.Where(itm => itm.CmpItemId == _itemId).ToList();
            (itemRowControl as SeriesDoseRowControl).IsSeriesLevel = false;
            (itemRowControl as SeriesDoseRowControl)._lstSeriesAttributeContract = CurrentViewContext.lstSeriesAttributeContract;
            (itemRowControl as SeriesDoseRowControl).ItemSeriesItemOrder = itemSeriesItemOrder;
            (itemRowControl as SeriesDoseRowControl).CIId = _itemId;
            (itemRowControl as SeriesDoseRowControl).ItemSeriesItemId = isiId;
            (itemRowControl as SeriesDoseRowControl).ID = _seriesDoseRowControlIdPrefix + uniqueIdentifier;
            (itemRowControl as SeriesDoseRowControl).lstStatusTypes = CurrentViewContext.lstStatusTypes;
            (itemRowControl as SeriesDoseRowControl).SeriesId = CurrentViewContext.SeriesId;
            (itemRowControl as SeriesDoseRowControl).TenantId = CurrentViewContext.SelectedTenantId;
            (itemRowControl as SeriesDoseRowControl).UniqueIdentifier = uniqueIdentifier;
            (itemRowControl as SeriesDoseRowControl).DisplayMoveDownButton = itemSeriesItemOrder != CurrentViewContext.lstSeriesItemContract.Max(sic => sic.ItemSeriesItemOrder) ? true : false;

            DelegateAddNewRow _addRow = new DelegateAddNewRow(AddNewItemRow);
            (itemRowControl as SeriesDoseRowControl).DelegateAddRow = _addRow;

            DelegateRemoveRow _removeRow = new DelegateRemoveRow(RemoveItemRow);
            (itemRowControl as SeriesDoseRowControl).DelegateRemoveRow = _removeRow;

            DelegateMoveUpRow _moveUpRow = new DelegateMoveUpRow(MoveUpItemRow);
            (itemRowControl as SeriesDoseRowControl).DelegateMoveUpRow = _moveUpRow;

            DelegateMoveDownRow _moveDownRow = new DelegateMoveDownRow(MoveDownItemRow);
            (itemRowControl as SeriesDoseRowControl).DelegateMoveDownRow = _moveDownRow;

            _trItemRow.Controls.Add(itemRowControl);
            _table.Controls.Add(_trItemRow);

            _dicHtmlAttributes.Remove("id");
            _dicHtmlAttributes.Remove("runat");
        }

        /// <summary>
        /// Generate the Table Row for Series
        /// </summary>
        private void GenerateSeriesRow()
        {

            var _trSeries = GenerateHTML("tr", _dicHtmlAttributes, String.Empty);
            Control seriesRowControl = Page.LoadControl("~/ComplianceAdministration/UserControl/SeriesDoseRowControl.ascx");

            (seriesRowControl as SeriesDoseRowControl).lstSeriesItemContract = CurrentViewContext.lstSeriesItemContract;
            (seriesRowControl as SeriesDoseRowControl).IsSeriesLevel = true;
            (seriesRowControl as SeriesDoseRowControl)._lstSeriesAttributeContract = CurrentViewContext.lstSeriesAttributeContract;
            (seriesRowControl as SeriesDoseRowControl).CIId = null;
            (seriesRowControl as SeriesDoseRowControl).ID = _seriesDoseRowControlIdPrefix + CurrentViewContext.SeriesId;
            _trSeries.Controls.Add(seriesRowControl);

            _table.Controls.Add(_trSeries);
        }

        /// <summary>
        /// Generate the Table Header Row
        /// </summary>
        /// <param name="_dicHtmlAttributes"></param>
        /// <returns></returns>
        private HtmlGenericControl GenerateHeader()
        {
            Dictionary<String, String> _dicHtmlAttributes = new Dictionary<String, String>();
            var _headerRow = GenerateHTML("tr", _dicHtmlAttributes, String.Empty);

            _dicHtmlAttributes.Add("bgcolor", "#C6C5C5");
            _dicHtmlAttributes.Add("style", "border: solid 1px Black;width:15px;text-align:center");
            var _headerSNoCol1 = GenerateHTML("td", _dicHtmlAttributes, "#");

            var _headerBlankCol2 = GenerateHTML("td", _dicHtmlAttributes, "");
            var _headerKeyAttributeCol3 = GenerateHTML("td", _dicHtmlAttributes, "Key Attribute");
            var _headerStatusColumnCol5 = GenerateHTML("td", _dicHtmlAttributes, "Status after Data shuffle");
            var _headerActionsColumnCol6 = GenerateHTML("td", _dicHtmlAttributes, "Actions");


            _headerRow.Controls.Add(_headerSNoCol1);
            _headerRow.Controls.Add(_headerBlankCol2);
            _headerRow.Controls.Add(_headerKeyAttributeCol3);

            var _colSpan = CurrentViewContext.lstSeriesAttributeContract.Where(attr => attr.IsKeyAttribute == false && attr.IsSeriesAttribute == true).Count();

            _dicHtmlAttributes.Add("colspan", Convert.ToString(_colSpan));

            if (_colSpan > AppConsts.NONE)
            {
                var _headerOtherAttributesCol4 = GenerateHTML("td", _dicHtmlAttributes, "Other Attributes");
                _headerRow.Controls.Add(_headerOtherAttributesCol4);
            }
            _headerRow.Controls.Add(_headerStatusColumnCol5);
            _headerRow.Controls.Add(_headerActionsColumnCol6);

            return _headerRow;
        }

        /// <summary>
        /// Generate HTML type control
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dicAttributes"></param>
        /// <param name="innerText"></param>
        /// <returns></returns>
        private HtmlGenericControl GenerateHTML(String type, Dictionary<String, String> dicAttributes, String innerText)
        {
            var _htmlControl = new HtmlGenericControl(type);

            foreach (var attr in dicAttributes)
            {
                _htmlControl.Attributes.Add(attr.Key, attr.Value);
            }
            _htmlControl.InnerText = innerText;
            return _htmlControl;
        }

        /// <summary>
        /// Add the New Item Row when 'Add' is clicked from Row Actions.
        /// </summary>
        /// <param name="itemId"></param>
        private void AddNewItemRow(Int32 itemId)
        {
            var _maxOrderId = CurrentViewContext.lstSeriesItemContract.Max(sic => sic.ItemSeriesItemOrder);

            //Display the Down button for all the rows, before the new row get's added in the table
            foreach (var uid in CurrentViewContext.lstSeriesItemContract.Select(sic => sic.UniqueIdentifier).Distinct())
            {
                var _btnDown = pnlRows.FindServerControlRecursively("_btnDown_" + uid);
                if (_btnDown.IsNotNull())
                {
                    (_btnDown as INTERSOFT.WEB.UI.WebControls.WclButton).Visible = true;
                }
            }

            var _itemData = CurrentViewContext.lstSeriesItemContract.Where(sic => sic.CmpItemId == itemId).First();

            SeriesItemContract _seriesItemContract = new SeriesItemContract();
            _seriesItemContract.CmpItemId = itemId;
            _seriesItemContract.CmpItemName = _itemData.CmpItemName;
            _seriesItemContract.ItemSeriesName = _itemData.ItemSeriesName;
            _seriesItemContract.ItemSeriesItemOrder = _maxOrderId + 1;
            _seriesItemContract.ItemSeriesId = CurrentViewContext.SeriesId;
            _seriesItemContract.ItemSeriesAttributeMapId = AppConsts.NONE;
            _seriesItemContract.ItemSeriesItemId = AppConsts.NONE;
            _seriesItemContract.SelectedAttributeId = AppConsts.NONE;
            _seriesItemContract.ISAM_ItemSeriesAttrId = AppConsts.NONE;
            _seriesItemContract.PostShuffleStatusCode = String.Empty;
            _seriesItemContract.IsTempRow = true;
            _seriesItemContract.UniqueIdentifier = Guid.NewGuid();

            CurrentViewContext.lstSeriesItemContract.Add(_seriesItemContract);

            AddRow(AppConsts.NONE, itemId, _seriesItemContract.ItemSeriesItemOrder, _seriesItemContract.UniqueIdentifier);

            // Enabled Remove Button for already added Item, when a new type of it get's added
            var _btnRemove = pnlRows.FindServerControlRecursively("btnRemove_" + itemId);
            if (_btnRemove.IsNotNull())
            {
                (_btnRemove as INTERSOFT.WEB.UI.WebControls.WclButton).Visible = true;
            }
        }

        /// <summary>
        /// Remove the selected Row from the Mapping table
        /// </summary>
        /// <param name="uniqueIdentifier"></param>
        private void RemoveItemRow(Guid uniqueIdentifier)
        {
            // This 'CurrentViewContext.lstSeriesItemContract' contains multiple records for already saved items. So List is used
            var _lstItemsToRemove = CurrentViewContext.lstSeriesItemContract.Where(sic => sic.UniqueIdentifier == uniqueIdentifier).ToList();

            //Remove the selected Item from the list
            foreach (var item in _lstItemsToRemove)
            {
                CurrentViewContext.lstSeriesItemContract.Remove(item);
            }

            // Remove the selected item from the Table
            foreach (System.Web.UI.Control control in _table.Controls)
            {
                if (control.GetType() == typeof(System.Web.UI.HtmlControls.HtmlGenericControl) && (control as System.Web.UI.HtmlControls.HtmlGenericControl).TagName.ToLower() == "tr")
                {
                    if ((control as System.Web.UI.HtmlControls.HtmlGenericControl).Attributes["id"] == "tr_" + uniqueIdentifier)
                    {
                        _table.Controls.Remove(control);
                        break;
                    }
                }
            }

            // Enanled delete ONLY if same ComplianceItem with different UniqueIdentifier's is found.
            var _enableDelete = CurrentViewContext.lstSeriesItemContract.Where(sic => sic.CmpItemId == _lstItemsToRemove.First().CmpItemId)
                                                                        .GroupBy(sic => sic.UniqueIdentifier)
                                                                        .Count() > AppConsts.ONE ? true : false;

            var _btnRemove = pnlRows.FindServerControlRecursively("btnRemove_" + _lstItemsToRemove.First().CmpItemId);
            if (_btnRemove.IsNotNull())
            {
                (_btnRemove as INTERSOFT.WEB.UI.WebControls.WclButton).Visible = _enableDelete;
            }

            #region Update the display order of the Items, after removal.

            // Get the items for which the display order is to be updated 
            var _lstItemsToUpdate = CurrentViewContext.lstSeriesItemContract.Where(sic => sic.ItemSeriesItemOrder > _lstItemsToRemove.First().ItemSeriesItemOrder).ToList();

            _lstItemsToUpdate.ForEach(isc =>
            {
                isc.ItemSeriesItemOrder -= 1;
            });

            // Update the Display order of items, which are moved up, after deletion
            foreach (System.Web.UI.Control control in _table.Controls)
            {
                if (control.GetType() == typeof(System.Web.UI.HtmlControls.HtmlGenericControl) && (control as System.Web.UI.HtmlControls.HtmlGenericControl).TagName.ToLower() == "tr")
                {
                    var _ctrl = control as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (_ctrl.Attributes["id"].IsNotNull() && _ctrl.Attributes["id"].Contains("tr_"))
                    {
                        var _id = Convert.ToString(_ctrl.Attributes["id"]);
                        _id = _id.Substring(_id.IndexOf("_") + 1);

                        if (_lstItemsToUpdate.Any(sic => sic.UniqueIdentifier == Guid.Parse(_id)))
                        {
                            var _litDisplayOrder = _table.FindServerControlRecursively("litSDisplayOrder_" + Guid.Parse(_id)) as Literal;
                            _litDisplayOrder.Text = Convert.ToString(Convert.ToInt32(_litDisplayOrder.Text) - 1);
                        }
                    }
                }
            }
            #endregion

            var _maxOrderId = CurrentViewContext.lstSeriesItemContract.Max(sic => sic.ItemSeriesItemOrder);
            var _itemIdentifierMax = CurrentViewContext.lstSeriesItemContract.Where(sic => sic.ItemSeriesItemOrder == _maxOrderId).First().UniqueIdentifier;

            // Remove the 'Down Icon' for last record in the list.
            var _btnDown = pnlRows.FindServerControlRecursively("_btnDown_" + _itemIdentifierMax);
            if (_btnDown.IsNotNull())
            {
                (_btnDown as INTERSOFT.WEB.UI.WebControls.WclButton).Visible = false;
            }


            var _minOrderId = CurrentViewContext.lstSeriesItemContract.Min(sic => sic.ItemSeriesItemOrder);
            var _itemIdentifierMin = CurrentViewContext.lstSeriesItemContract.Where(sic => sic.ItemSeriesItemOrder == _minOrderId).First().UniqueIdentifier;

            // Remove the 'Up Icon' for TOP record in the list.
            var _btnUp = pnlRows.FindServerControlRecursively("_btnUp_" + _itemIdentifierMin);
            if (_btnUp.IsNotNull())
            {
                (_btnUp as INTERSOFT.WEB.UI.WebControls.WclButton).Visible = false;
            }
        }

        /// <summary>
        /// Change the Display order to Move the selected Row up in the Mapping table
        /// </summary>
        /// <param name="uniqueIdentifier"></param>
        private void MoveUpItemRow(Guid uniqueIdentifier)
        {
            MoveUpDown(uniqueIdentifier, true);
        }

        /// <summary>
        /// Change the Display order to Move the selected Row down in the Mapping table
        /// </summary>
        /// <param name="uniqueIdentifier"></param>
        private void MoveDownItemRow(Guid uniqueIdentifier)
        {
            MoveUpDown(uniqueIdentifier, false);
        }

        /// <summary>
        /// Update the Display order in the list and re-bind the table.
        /// </summary>
        /// <param name="uniqueIdentifier"></param>
        /// <param name="moveUp"></param>
        private void MoveUpDown(Guid uniqueIdentifier, Boolean moveUp)
        {
            // This 'CurrentViewContext.lstSeriesItemContract' contains multiple records for already saved items. So List is used
            var _lstItemToManage = CurrentViewContext.lstSeriesItemContract.Where(sic => sic.UniqueIdentifier == uniqueIdentifier).ToList();
            var _crntItemOrderId = _lstItemToManage.First().ItemSeriesItemOrder;

            if (moveUp)
            {
                // Get Items with DisplayOrder 1 Less then current items' display order
                var _lstItemToMoveDown = CurrentViewContext.lstSeriesItemContract.Where(sic => sic.ItemSeriesItemOrder == _crntItemOrderId - 1).ToList();

                // Update the Display order of current item to 1 less then current order
                _lstItemToManage.ForEach(isi =>
                {
                    isi.ItemSeriesItemOrder = isi.ItemSeriesItemOrder - 1;
                });

                // Increment the Display order of Item above current item by 1, to move below in list
                _lstItemToMoveDown.ForEach(isi =>
                {
                    isi.ItemSeriesItemOrder = isi.ItemSeriesItemOrder + 1;
                });
            }
            else
            {
                // Get Items with DisplayOrder 1 Less then current items' display order
                var _lstItemToMoveUp = CurrentViewContext.lstSeriesItemContract.Where(sic => sic.ItemSeriesItemOrder == _crntItemOrderId + 1).ToList();

                // Update the Display order of current item to 1 less then current order
                _lstItemToManage.ForEach(isi =>
                {
                    isi.ItemSeriesItemOrder = isi.ItemSeriesItemOrder + 1;
                });

                // Increment the Display order of Item above current item by 1, to move below in list
                _lstItemToMoveUp.ForEach(isi =>
                {
                    isi.ItemSeriesItemOrder = isi.ItemSeriesItemOrder - 1;
                });

            }
            // Clear panel and re-generate the table
            pnlRows.Controls.Clear();
            GenerateMappingTable();
        }

        /// <summary>
        /// Load Series UnMapped Attributes user control
        /// </summary>
        private void LoadSeriesUnMappedAttributes()
        {
            pnlSeriesUnMappedAttributes.Controls.Clear();
            Control seriesUnMappedAttributesControl = Page.LoadControl("~\\ComplianceAdministration\\UserControl\\SeriesUnMappedAttributes.ascx");

            (seriesUnMappedAttributesControl as SeriesUnMappedAttributes).ID = "ucSeriesUnMappedAttributes_" + CurrentViewContext.SeriesId;
            (seriesUnMappedAttributesControl as SeriesUnMappedAttributes).ItemSeriesId = CurrentViewContext.SeriesId;
            (seriesUnMappedAttributesControl as SeriesUnMappedAttributes).TenantId = CurrentViewContext.SelectedTenantId;
            pnlSeriesUnMappedAttributes.Controls.Add(seriesUnMappedAttributesControl);
        }

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveData();
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

        #endregion
    }
}