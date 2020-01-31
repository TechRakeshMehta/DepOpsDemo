<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageAgencyHierarchy.ascx.cs" Inherits="CoreWeb.AgencyHierarchy.Views.ManageAgencyHierarchy" %>


<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>


<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>


<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">Manage Agency Hierarchy
            </h2>
        </div>
    </div>

    <%--OnNeedDataSource="grdRotationPackage_NeedDataSource" OnItemCommand="grdRotationPackage_ItemCommand" OnDeleteCommand="grdRotationPackage_DeleteCommand"
            NonExportingColumns="EditCommandColumn,DeleteColumn" ShowClearFiltersButton="false" OnSortCommand="grdRotationPackage_SortCommand" OnItemDataBound="grdRotationPackage_ItemDataBound"--%>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdAgencyHierarchy" Width="100%" CssClass="gridhover" AllowCustomPaging="true"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="true"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false" OnInit="grdAgencyHierarchy_Init"
            OnNeedDataSource="grdAgencyHierarchy_NeedDataSource" OnItemCommand="grdAgencyHierarchy_ItemCommand" OnDeleteCommand="grdAgencyHierarchy_DeleteCommand"
            ShowClearFiltersButton="true" OnSortCommand="grdAgencyHierarchy_SortCommand">
            <ClientSettings EnableRowHoverStyle="true">
                <ClientEvents OnRowDblClick="grd_rwDbClick" />
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="AgencyHierarchyID"
                AllowFilteringByColumn="true">
                <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add New Hierarchy" ShowExportToCsvButton="false"
                    ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToWordButton="false" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="AgencyHierarchyLabel" HeaderText="Agency Hierarchy" SortExpression="AgencyHierarchyLabel"
                        UniqueName="AgencyHierarchyLabel" HeaderTooltip="This column displays the Agency Hierarchy Label for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="../Resources/Mod/Dashboard/images/CancelGrid.gif"
                        CommandName="Delete" ConfirmText="Are you sure you want to delete this Hierarchy?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="EditCommandColumn">
                        <ItemTemplate>
                            <div style="display: none">
                                <telerik:RadButton ID="btnEdit" ButtonType="LinkButton" CommandName="Edit"
                                    runat="server" Text="Edit">
                                </telerik:RadButton>
                            </div>
                            <itemtemplate>
                             <asp:ImageButton ID="impBtnEdit" ClientIDMode="Static" CssClass="abc" runat="server" ImageUrl="../Resources/Mod/Dashboard/images/editGrid.gif" AlternateText="Edit" CommandName="Edit"/>
                         </itemtemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
</div>

<script type="text/javascript">
  
    function grd_rwDbClick(s, e) {
        var _id = "btnEdit";
        var b = e.get_gridDataItem().findControl(_id);
        if (b && typeof (b.click) != "undefined") { b.click(); }
    }
</script>
<script src="../Resources/Mod/Dashboard/Scripts/bootstrap.min.js" type="text/javascript"></script>
