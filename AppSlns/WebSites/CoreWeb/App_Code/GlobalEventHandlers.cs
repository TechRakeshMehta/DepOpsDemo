using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using System.Web.UI.WebControls;
using Business.RepoManagers;

namespace CoreWeb
{
    /// <summary>
    /// Summary description for GlobalEventHandlers
    /// </summary>
    public class GlobalEventHandlers
    {
        #region Global Event Handles

        #region WclGrid Global Event Handlers

        public static void Grid_PreRender(object sender, EventArgs e)
        {
            ShowHideColumns(sender);
            WclGrid grd = sender as WclGrid;
            grd.FilterMenu.OnClientShown = "FocusMenu";
            if (!Extensions.HTMLEncodeOverride && Extensions.IsHtmlEncodingActivated)
            {
                EncodeCellsForRowCommand(grd);
                DecodeGrdCellDataOnExportSinglePage(sender);
            }
        }

        public static void Grid_Load(object sender, EventArgs e)
        {
            //Code to maintain page size of all the grid through the application
            //RadGrid grd = sender as RadGrid;
            WclGrid grd = sender as WclGrid;
            if (grd != null)
            {
                if (grd.Page.Session["GRID_PAGE_SIZE"] != null)
                {
                    int _savedPageSize = (int)grd.Page.Session["GRID_PAGE_SIZE"];
                    if (grd.MasterTableView.PageSize != _savedPageSize)
                    {
                        grd.MasterTableView.PageSize = _savedPageSize;
                    }
                }
                grd.PageSize = grd.MasterTableView.PageSize;
                foreach (GridColumn column in grd.Columns)
                {
                    if (column.ColumnType == "GridDateTimeColumn" || column.DataType == typeof(System.DateTime) || column.ColumnType == "GridButtonColumn")
                    {
                        //Do nothing
                    }
                    else if (column.ColumnType == "GridTemplateColumn" || column.DataType.ToString() == "System.Boolean")
                    {
                        if (column.CurrentFilterFunction == GridKnownFunction.NoFilter)
                        {
                            column.AutoPostBackOnFilter = true;
                            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                        }
                    }
                    else if (column.DataType == typeof(System.Int32) || column.ColumnType == "GridNumericColumn")
                    {
                        if (column.CurrentFilterFunction == GridKnownFunction.NoFilter)
                        {
                            column.AutoPostBackOnFilter = true;
                            column.CurrentFilterFunction = GridKnownFunction.EqualTo;
                        }
                    }
                    else
                    {
                        if (column.CurrentFilterFunction == GridKnownFunction.NoFilter)
                        {
                            column.AutoPostBackOnFilter = true;
                            column.CurrentFilterFunction = GridKnownFunction.Contains;
                        }
                    }

                    var wclGridDataObjectObj = grd.WclGridDataObject as GridObjectDataContainer;
                    if (Extensions.IsHtmlEncodingActivated && (wclGridDataObjectObj.IsNullOrEmpty() || !wclGridDataObjectObj.ColumnsToSkipEncoding.Contains(column.UniqueName)) && !Extensions.HTMLEncodeOverride)
                    {
                        EncodeGridBoundColumn(column);
                    }
                }
                grd.MasterTableView.PagerStyle.Position = GridPagerPosition.TopAndBottom;
            }
        }

        public static void Grid_PageSizeChanged(object source, GridPageSizeChangedEventArgs e)
        {
            //Code to maintain page size of all the grid through the application
            RadGrid grd = source as RadGrid;
            if (grd != null)
            {
                grd.Page.Session["GRID_PAGE_SIZE"] = e.NewPageSize;
            }
        }

        /// <summary>
        /// Remove On Change Event from all databound column from grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Grid_ItemCreated(object sender, GridItemEventArgs e)
        {
            RadGrid grd = sender as RadGrid;
            if (e.Item is GridFilteringItem)
            {
                GridFilteringItem filterItem = e.Item as GridFilteringItem;
                if (filterItem.OwnerTableView.DetailTableIndex.Equals(AppConsts.NONE) && grd.MasterTableView.DetailTables.Count > AppConsts.NONE) // For Child grid If Exist.
                {
                    GridTableView Detailgrd = grd.MasterTableView.DetailTables[AppConsts.NONE] as GridTableView;
                    foreach (GridColumn clm in Detailgrd.Columns)
                    {
                        if (filterItem[clm.UniqueName].Controls.Count > AppConsts.NONE && clm.ColumnType.Equals("GridBoundColumn"))
                        {
                            TextBox txtBoxFilter = filterItem[clm.UniqueName].Controls[AppConsts.NONE] as TextBox;
                            if (txtBoxFilter != null)
                            {
                                txtBoxFilter.Attributes.Remove("onchange");
                            }
                        }
                    }
                }
                else //By defualt :: Run This Code for ::   Grid:: 
                {
                    foreach (GridColumn clm in grd.Columns)
                    {
                        if (filterItem[clm.UniqueName].Controls.Count > AppConsts.NONE && clm.ColumnType.Equals("GridBoundColumn"))
                        {
                            TextBox txtBoxFilter = filterItem[clm.UniqueName].Controls[AppConsts.NONE] as TextBox;
                            if (txtBoxFilter != null)
                            {
                                txtBoxFilter.Attributes.Remove("onchange");
                            }
                        }
                        if (filterItem[clm.UniqueName].Controls.Count > AppConsts.NONE && clm.ColumnType.Equals("GridNumericColumn"))
                        {
                            RadNumericTextBox txtBoxFilter = filterItem[clm.UniqueName].Controls[AppConsts.NONE] as RadNumericTextBox;
                            if (txtBoxFilter != null)
                            {
                                txtBoxFilter.Attributes.Remove("onchange");
                            }
                        }
                        if (filterItem[clm.UniqueName].Controls.Count > AppConsts.NONE && clm.ColumnType.Equals("GridDateTimeColumn"))
                        {
                            RadDatePicker datePickerFilter = filterItem[clm.UniqueName].Controls[AppConsts.NONE] as RadDatePicker;
                            if (datePickerFilter != null)
                            {
                                datePickerFilter.Attributes.Remove("onchange");
                            }

                        }

                        if (filterItem[clm.UniqueName].Controls.Count > AppConsts.NONE && clm.ColumnType.Equals("GridTemplateColumn"))
                        {
                            TextBox txtBoxFilter = filterItem[clm.UniqueName].Controls[AppConsts.NONE] as TextBox;
                            if (txtBoxFilter != null)
                            {
                                txtBoxFilter.Attributes.Remove("onchange");
                            }
                        }
                    }
                }
            }
        }

        public static void Grid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (!Extensions.HTMLEncodeOverride && Extensions.IsHtmlEncodingActivated)
            {
                WclGrid grd = InitializeGridDataContainer(sender);
                GridDataItem dataItem = e.Item as GridDataItem;
                CreateListOfCellsForDecoding(grd, dataItem);
                SetCsvExportAttribute(sender, e);
            }
        }

        public static void Grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (!Extensions.HTMLEncodeOverride && Extensions.IsHtmlEncodingActivated && IsHtmlEventFromSenderGrid(sender, e))
            {
                DecodeCellsForRowCommand(sender, e);
            }
        }

        #endregion

        #region WclDropdown Global Event Handlers

        /// <summary>
        /// Fixed height of all dropdown 220 pixel for 10 itmes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void RadComboBox_PreRender(object sender, EventArgs e)
        {
            WclComboBox comboBox = sender as WclComboBox;
            if (comboBox != null)
            {
                var data = ((Telerik.Web.UI.RadComboBox)(sender)).UniqueID;
                //comboBox.Sort = RadComboBoxSort.Ascending;
                if (comboBox.Items.Count > AppConsts.TEN)
                {
                    comboBox.MaxHeight = AppConsts.TWOHUNDREDTWENTYTWO;
                }
            }
        }

        /// <summary>
        /// Fixed height of all dropdown 220 pixel for 10 itmes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void RadDropDownList_PreRender(object sender, EventArgs e)
        {
            WclDropDownList ddlDropDownList = sender as WclDropDownList;
            if (ddlDropDownList != null)
            {
                var data = ((WclDropDownList)(sender)).UniqueID;
                if (ddlDropDownList.Items.Count > AppConsts.TEN)
                {
                    ddlDropDownList.DropDownHeight = AppConsts.TWOHUNDREDTWENTYTWO;
                }
            }
        }
        #endregion

        #region WclTreeList/WclTreeView Global Event Handlers

        public static void TreeView_Load(object sender, EventArgs e)
        {
            if (Extensions.IsHtmlEncodingActivated)
            {
                WclTreeView treeView = sender as WclTreeView;
                if (!Convert.ToBoolean(treeView.Attributes["SkipTreeViewEncoding"]))
                {
                    treeView.EnableNodeTextHtmlEncoding = true;
                }
            }
        }

        public static void TreeList_Load(object sender, EventArgs e)
        {
            if (Extensions.IsHtmlEncodingActivated)
            {
                WclTreeList treeList = sender as WclTreeList;
                foreach (TreeListColumn column in treeList.Columns)
                {
                    if (column.ColumnType == "TreeListBoundColumn")
                    {
                        TreeListBoundColumn col = column as TreeListBoundColumn;
                        col.HtmlEncode = true;
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Properties 

        #endregion

        #region Methods

        #region Private Methods
        private static void ShowHideColumns(object sender)
        {
            #region UAT-2264: grid customization
            WclGrid grd = sender as WclGrid;

            List<String> lstPreHiddenColumns = new List<String>();
            String GridCode = String.Empty;

            String preHiddenColumns = String.Empty;
            if (!grd.Attributes["PreHiddenColumns"].IsNullOrEmpty())
            {
                preHiddenColumns = grd.Attributes["PreHiddenColumns"];
                lstPreHiddenColumns = preHiddenColumns.Split(',').ToList();
            }
            if (!grd.Attributes["GridCode"].IsNullOrEmpty())
            {
                GridCode = grd.Attributes["GridCode"];
            }
            List<String> columnsToHide = new List<String>();
            //String GridID = grd.ID;
            //String GridCode = String.Empty;
            //var fieldInfo = Screen.grdRotations.GetType().GetField(GridID);
            //if (!fieldInfo.IsNullOrEmpty())
            //{
            //    StringValueAttribute[] data = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            //    GridCode = data.Length > 0 ? data[0].StringValue : String.Empty;
            if (!GridCode.IsNullOrEmpty())
            {
                Int32 CurrentLoggedInUserID = CoreWeb.Shell.SysXWebSiteUtils.SessionService.OrganizationUserId;
                columnsToHide = SecurityManager.GetScreenColumnsToHide(GridCode, CurrentLoggedInUserID);

                String columnListToHide = String.Join(",", columnsToHide);

                //if(!columnListToHide.IsNullOrEmpty())
                ((INTERSOFT.WEB.UI.WebControls.WclGrid)(grd)).ColumnsToBeHiden = columnListToHide;

                foreach (GridColumn column in grd.Columns)
                {
                    String ColumnUniqueName = column.UniqueName;
                    if ((!lstPreHiddenColumns.IsNullOrEmpty() && lstPreHiddenColumns.Contains(ColumnUniqueName))
                        || (!columnsToHide.IsNullOrEmpty() && columnsToHide.Contains(ColumnUniqueName)))
                    {
                        column.Display = false;
                    }
                    else
                    {
                        column.Display = true;
                    }
                }

                if (!grd.Attributes["ExportingColumnsNotInGrid"].IsNullOrEmpty())
                {
                    List<String> lstExportingColumns = new List<String>();
                    String exportingColumns = String.Empty;

                    exportingColumns = grd.Attributes["ExportingColumnsNotInGrid"];
                    lstExportingColumns = exportingColumns.Split(',').ToList();
                    foreach (var column in lstExportingColumns)
                    {
                        if (lstPreHiddenColumns.Contains(column))
                        {
                            lstPreHiddenColumns.Remove(column);
                        }
                    }
                    preHiddenColumns = String.Join(",", lstPreHiddenColumns);
                }

                if (!lstPreHiddenColumns.IsNullOrEmpty())
                {
                    if (grd.ColumnsToBeHiden.IsNullOrEmpty())
                    {
                        grd.ColumnsToBeHiden = preHiddenColumns;
                    }
                    else
                    {
                        grd.ColumnsToBeHiden = grd.ColumnsToBeHiden + "," + preHiddenColumns;
                    }
                }
                grd.UpdateExportColumnList(grd.ColumnsToBeHiden);
            }
            #endregion
        }

        #region HTML Encoding Methods

        private static void EncodeGridBoundColumn(GridColumn column)
        {
            if (column.ColumnType == "GridBoundColumn")
            {
                if (((GridBoundColumn)column).DataFormatString.IsNullOrEmpty())
                {
                    ((GridBoundColumn)column).HtmlEncode = true;
                }
            }
        }

        private static void SetCsvExportAttribute(object sender, GridCommandEventArgs e)
        {
            WclGrid grd = sender as WclGrid;
            if (e.CommandName.IsNullOrEmpty() && e.Item is GridCommandItem)
            {
                if (grd != null)
                {
                    WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                    WclComboBox cmbExportPage = e.Item.FindControl("cmbExportPage") as WclComboBox;
                    if (cmbExportFormat != null && cmbExportFormat.SelectedValue == "Csv")
                    {
                        //CSV Single Page Export Case
                        if (cmbExportPage != null && cmbExportPage.SelectedValue != null && cmbExportPage.SelectedValue == "SINGLEPAGE")
                        {
                            Int32 attrKeyCount = grd.Attributes.Keys.Count;
                            String[] attributeKeys = new String[attrKeyCount];
                            grd.Attributes.Keys.CopyTo(attributeKeys, 0);
                            if (!attributeKeys.Any(x => x == "IsCsvSinglePageExport"))
                                grd.Attributes.Add("IsCsvSinglePageExport", "true");
                            else
                                grd.Attributes["IsCsvSinglePageExport"] = "true";
                        }
                        //All page Export Case
                        else
                        {
                            foreach (GridColumn column in grd.Columns)
                            {
                                if (column.ColumnType == "GridBoundColumn")
                                {
                                    GridBoundColumn col = column as GridBoundColumn;
                                    col.HtmlEncode = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void DecodeGrdCellDataOnExportSinglePage(object sender)
        {
            WclGrid grd = sender as WclGrid;
            if (grd != null && Convert.ToBoolean(grd.Attributes["IsCsvSinglePageExport"]))
            {
                foreach (GridColumn column in grd.Columns)
                {
                    if (column.ColumnType == "GridBoundColumn")
                    {
                        for (int i = 0; i < grd.Items.Count; i++)
                        {
                            grd.Items[i][column.UniqueName].Text = grd.Items[i][column.UniqueName].Text.HtmlDecode();
                        }
                    }
                }
                grd.Attributes["IsCsvSinglePageExport"] = "false";
            }
        }

        private static void CreateListOfCellsForDecoding(WclGrid grd, GridDataItem dataItem)
        {
            if (grd != null && !dataItem.IsNullOrEmpty())
            {
                foreach (GridColumn column in grd.Columns)
                {
                    if (column.ColumnType == "GridBoundColumn")
                    {
                        GridBoundColumn col = column as GridBoundColumn;
                        if (col.HtmlEncode && !col.ReadOnly && col.Visible)
                        {
                            GridCellForDecodeOnRowCommand grdItemToEncodeDecodeObj = new GridCellForDecodeOnRowCommand();
                            grdItemToEncodeDecodeObj.RowIndex = dataItem.ItemIndex;
                            grdItemToEncodeDecodeObj.columnUniqueName = col.UniqueName;
                            grdItemToEncodeDecodeObj.isDecoded = false;
                            (grd.WclGridDataObject as GridObjectDataContainer).GridCellListForDecodeOnRowCommand.Add(grdItemToEncodeDecodeObj);
                        }
                    }
                }
            }
        }

        private static WclGrid InitializeGridDataContainer(object sender)
        {
            WclGrid grd = sender as WclGrid;
            if (grd.WclGridDataObject == null)//grd.WclGridDataObject
            {
                grd.WclGridDataObject = new GridObjectDataContainer();
            }
            return grd;
        }

        private static void DecodeCellsForRowCommand(object sender, GridItemEventArgs e)
        {
            WclGrid grd = sender as WclGrid;

            if (grd != null)
            {
                GridObjectDataContainer wclGridDataObjectObj = grd.WclGridDataObject as GridObjectDataContainer;
                if (wclGridDataObjectObj.IsNullOrEmpty() || wclGridDataObjectObj.GridCellListForDecodeOnRowCommand.IsNullOrEmpty())
                    return;
                var gi = e.Item as GridEditableItem;
                if (!gi.IsNullOrEmpty())
                {
                    var EncodedCells = wclGridDataObjectObj.GridCellListForDecodeOnRowCommand.Where(item => item.RowIndex == gi.ItemIndex && !item.isDecoded).ToList();
                    if (!EncodedCells.IsNullOrEmpty())
                    {
                        foreach (var cell in EncodedCells)
                        {
                            gi[cell.columnUniqueName].Text = (gi[cell.columnUniqueName].Text).HtmlDecode();
                            cell.isDecoded = true;
                        }
                    }

                }

            }
        }

        private static void EncodeCellsForRowCommand(WclGrid grd)
        {
            if (grd != null )
            {
                GridObjectDataContainer wclGridDataObj = grd.WclGridDataObject as GridObjectDataContainer;
                if (wclGridDataObj.IsNullOrEmpty() || wclGridDataObj.GridCellListForDecodeOnRowCommand.IsNullOrEmpty())
                    return;

                var DecodedCells = wclGridDataObj.GridCellListForDecodeOnRowCommand.Where(item => item.isDecoded).ToList();
                if (!DecodedCells.IsNullOrEmpty())
                    foreach (var cell in DecodedCells)
                    {
                        if (grd.Items.Count <= cell.RowIndex) continue;// skip the encoded cell if it is no longer in range
                        var gi = grd.Items[cell.RowIndex] as GridEditableItem;
                        gi[cell.columnUniqueName].Text = (gi[cell.columnUniqueName].Text).HtmlEncode();
                        cell.isDecoded = false;
                    }
            }
            grd.WclGridDataObject = null;
        }

        private static Boolean IsHtmlEventFromSenderGrid(object sender, GridItemEventArgs e)
        {
            WclGrid grd = sender as WclGrid;
            if (grd.GetType().Name == e.Item.Parent.NamingContainer.Parent.GetType().Name)
            {
                return true;
            }
            return false;
        }

        #endregion

        #endregion

        #endregion
    }

    #region Contract Classes for Global Events Handlers

    public class GridCellForDecodeOnRowCommand
    {
        public Int32 RowIndex { get; set; }
        public String columnUniqueName { get; set; }
        /// <summary>
        /// True means item has been explicitly decoded because of row command being fired. Should be Encoded back before returning HTML response.
        /// </summary>
        public Boolean isDecoded { get; set; }
    }
    public class GridObjectDataContainer
    {
        private List<GridCellForDecodeOnRowCommand> _GridCellListForDecodeOnRowCommand;
        private List<String> _columnsToSkipEncoding;

        public GridObjectDataContainer()
        {
            _columnsToSkipEncoding = new List<string>();
            _GridCellListForDecodeOnRowCommand = new List<GridCellForDecodeOnRowCommand>();
        }

        public List<GridCellForDecodeOnRowCommand> GridCellListForDecodeOnRowCommand
        {
            get
            {
                return _GridCellListForDecodeOnRowCommand;
            }
            set
            {
                _GridCellListForDecodeOnRowCommand = value;
            }
        }
        public List<String> ColumnsToSkipEncoding
        {
            get
            {
                return _columnsToSkipEncoding;
            }
            set
            {
            }
        }
    }
    #endregion
}