<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Messaging.Views.TransferRulesGrid" Codebehind="TransferRulesGrid.ascx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<infs:WclGrid runat="server" ID="grdRules" AllowPaging="True" AutoGenerateColumns="False"
    AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
    EnableDefaultFeatures="True" ShowAllExportButtons="false" ShowExtraButtons="False" GridLines="Both"
    OnNeedDataSource="grdRules_NeedDataSource" OnItemCommand="grdRules_ItemCommand">
    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
     Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm" Pdf-PageRightMargin="20mm">
    </ExportSettings>
    <ClientSettings EnableRowHoverStyle="true">
        <Selecting AllowRowSelect="true"></Selecting>
    </ClientSettings>
    <MasterTableView CommandItemDisplay="Top" DataKeyNames="MessageRuleID">
        <CommandItemSettings ShowAddNewRecordButton="true" AddNewRecordText="Add Rule" ShowExportToCsvButton="false"
                        ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false"/>
        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
        </RowIndicatorColumn>
        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
        </ExpandCollapseColumn>
        <Columns>
            <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Name column"
                HeaderText="Rule Description">
            </telerik:GridBoundColumn>
            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                <HeaderStyle CssClass="tplcohdr" />
                <ItemStyle CssClass="MyImageButton" />
            </telerik:GridEditCommandColumn>
            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmText="Are you sure you want to delete this Rule?"
                Text="Delete" UniqueName="DeleteColumn">
                <HeaderStyle Width="30px" />
            </telerik:GridButtonColumn>
        </Columns>
        <EditFormSettings EditFormType="WebUserControl" UserControlName="TransferRulesMaintenanceForm.ascx">
            <EditColumn UniqueName="EditCommandColumn" ButtonType="ImageButton">
                <HeaderStyle Width="30px" />
                <ItemStyle CssClass="MyImageButton" />
            </EditColumn>
        </EditFormSettings>
        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
    </MasterTableView>
    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
    <FilterMenu EnableImageSprites="False">
    </FilterMenu>
</infs:WclGrid>
