<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.ShotSeriesListing" CodeBehind="ShotSeriesListing.ascx.cs" %>
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

<infs:WclGrid runat="server" ID="grdShotSeries" AllowPaging="True" AutoGenerateColumns="False"
    OnItemCommand="grdShotSeries_ItemCommand" OnNeedDataSource="grdShotSeries_NeedDatasource"
    AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
    EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false"
    GridLines="None" EnableLinqExpressions="false">
    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
    </ExportSettings>
    <ClientSettings EnableRowHoverStyle="true">
        <Selecting AllowRowSelect="true"></Selecting>
    </ClientSettings>
    <MasterTableView CommandItemDisplay="Top" DataKeyNames="IS_ID" AllowFilteringByColumn="True">
        <CommandItemSettings ShowAddNewRecordButton="false" />
        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
        </RowIndicatorColumn>
        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
        </ExpandCollapseColumn>
        <Columns>
            <telerik:GridBoundColumn DataField="IS_Name" FilterControlAltText="Filter ItemName column"
                HeaderText="Series Name" SortExpression="IS_Name" UniqueName="IS_Name">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="IS_Label" FilterControlAltText="Filter ItemLabel column"
                HeaderText="Series Label" SortExpression="IS_Label" UniqueName="IS_Label">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="IS_Description" ItemStyle-CssClass="breakword" FilterControlAltText="Filter Description column"
                HeaderText="Description" SortExpression="IS_Description" UniqueName="IS_Description">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="IS_Details" ItemStyle-CssClass="breakword" FilterControlAltText="Filter Details column"
                HeaderText="Details" SortExpression="IS_Details" UniqueName="IS_Details">
            </telerik:GridBoundColumn>
            <telerik:GridTemplateColumn DataField="IS_IsActive" FilterControlAltText="Filter IsActive column" DataType="System.Boolean"
                HeaderText="Is Active" SortExpression="IS_IsActive" UniqueName="IsActive">
                <ItemTemplate>
                    <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("IS_IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                Text="Delete" UniqueName="DeleteColumn" ConfirmText="Are you sure you want to delete this record?">
                <HeaderStyle CssClass="tplcohdr" />
                <ItemStyle CssClass="MyImageButton" HorizontalAlign="Center" />
            </telerik:GridButtonColumn>
        </Columns>
        <EditFormSettings EditFormType="Template">
            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
            </EditColumn>
        </EditFormSettings>
        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat=" {4} {5} Item(s) in {1} page(s)" />
    </MasterTableView>
    <PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
    <FilterMenu EnableImageSprites="False">
    </FilterMenu>
</infs:WclGrid>

