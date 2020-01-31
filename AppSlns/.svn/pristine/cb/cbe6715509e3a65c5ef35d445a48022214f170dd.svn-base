<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.CategoryPackageListing" CodeBehind="CategoryPackageListing.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<style>
    .breakword
    {
        word-break: break-all;
    }

        .breakword ul
        {
            margin-left: 10px;
            padding-left: 10px !important;
        }

        .breakword li
        {
            list-style-position: inside;
            list-style: disc;
        }

        .breakword ol
        {
            list-style-type: decimal;
            margin-left: 10px;
            padding-left: 10px;
        }

            .breakword ol li
            {
                list-style: decimal;
            }
</style>

<infs:WclGrid runat="server" ID="grdPackage" AllowPaging="True" AutoGenerateColumns="False"
    AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
    EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false"
    OnNeedDataSource="grdPackage_NeedDataSource1">
    <ClientSettings EnableRowHoverStyle="true">
        <Selecting AllowRowSelect="true"></Selecting>
    </ClientSettings>
    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
        HideStructureColumns="true">
    </ExportSettings>
    <MasterTableView CommandItemDisplay="Top" DataKeyNames="CompliancePackageID">
        <CommandItemSettings ShowAddNewRecordButton="false" />
        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
        </RowIndicatorColumn>
        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
        </ExpandCollapseColumn>
        <Columns>
            <telerik:GridBoundColumn DataField="PackageName" FilterControlAltText="Filter PackageName column"
                HeaderText="Package Name" SortExpression="PackageName" UniqueName="PackageName">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="PackageLabel" FilterControlAltText="Filter PackageLabel column"
                HeaderText="Package Label" SortExpression="PackageLabel" UniqueName="PackageLabel">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                HeaderText="Description" SortExpression="Description" UniqueName="Description">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="ScreenLabel" FilterControlAltText="Filter ScreenLabel column"
                HeaderText="Screen Label" SortExpression="ScreenLabel" UniqueName="ScreenLabel">
            </telerik:GridBoundColumn>
            <telerik:GridTemplateColumn DataField="IsActive" FilterControlAltText="Filter IsActive column" DataType="System.Boolean"
                HeaderText="Is Active" SortExpression="IsActive" UniqueName="IsActive">
                <ItemTemplate>
                    <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
        </Columns>
        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
    </MasterTableView>
    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
    <FilterMenu EnableImageSprites="False">
    </FilterMenu>
</infs:WclGrid>


