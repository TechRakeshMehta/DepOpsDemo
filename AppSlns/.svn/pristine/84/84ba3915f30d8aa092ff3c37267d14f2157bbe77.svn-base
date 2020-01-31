<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgencyHierarchyMappedNodes.ascx.cs" Inherits="CoreWeb.AgencyHierarchy.Views.AgencyHierarchyMappedNodes" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxClinicalRotationMappingPopup">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<script type="text/javascript">
   
    function RefreshHierarchyTree() {
        var btn = $jQuery('[id$=btnUpdateTree]', $jQuery(parent.theForm));
        btn.click();
    }
</script>
<script src="../Resources/Mod/Dashboard/Scripts/bootstrap.min.js" type="text/javascript"></script>
<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <h2 class="header-color">
                <asp:Label ID="lblRotation" Text="Mapped Nodes" runat="server" />
            </h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="msgbox">
                <asp:Label ID="lblMsg" runat="server" CssClass="info"></asp:Label>
            </div>
        </div>
    </div>
    <div class="row">
        <infs:WclGrid runat="server" ID="grdMappedNodes" Width="100%" CssClass="gridhover" AllowCustomPaging="false"
            AutoGenerateColumns="False" AllowSorting="true" AllowFilteringByColumn="false"
            AutoSkinMode="true" CellSpacing="0" GridLines="Both" ShowAllExportButtons="false"
            OnNeedDataSource="grdMappedNodes_NeedDataSource" OnDeleteCommand="grdMappedNodes_DeleteCommand" 
            ShowClearFiltersButton="false" OnRowDrop="grdMappedNodes_RowDrop">
            <ClientSettings EnableRowHoverStyle="true" AllowRowsDragDrop="true" AllowAutoScrollOnDragDrop="true">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <ExportSettings Pdf-PageWidth="450mm" Pdf-PageHeight="230mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm" OpenInNewWindow="true" HideStructureColumns="false"
                ExportOnlyData="true" IgnorePaging="true">
            </ExportSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="AgencyHierarchyID"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" AddNewRecordText="Add New" ShowExportToCsvButton="false"
                    ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToWordButton="false" />
                <RowIndicatorColumn Visible="true" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="true" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="AgencyHierarchyLabel" HeaderText="Name" SortExpression="AgencyHierarchyLabel"
                        UniqueName="AgencyHierarchyLabel" HeaderTooltip="This column displays the node name for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Description" HeaderText="Description" SortExpression="Description"
                        UniqueName="Description" HeaderTooltip="This column displays the node description for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="DisplayOrder" HeaderText="Display Order" SortExpression="DisplayOrder"
                        UniqueName="DisplayOrder" HeaderTooltip="This column displays the node display order for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/Resources/Mod/Dashboard/images/CancelGrid.gif"
                        CommandName="Delete" ConfirmText="Are you sure you want to delete this Hierarchy?"
                        Text="Delete" UniqueName="DeleteColumn">
                        <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
                    </telerik:GridButtonColumn>
                </Columns>
                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
            </MasterTableView>
            <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
</div>
