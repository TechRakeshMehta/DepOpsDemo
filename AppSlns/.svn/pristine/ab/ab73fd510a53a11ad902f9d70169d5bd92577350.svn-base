<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.ItemsListing" CodeBehind="ItemsListing.ascx.cs" %>
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
<infs:WclGrid runat="server" ID="grdItems" AllowPaging="True" AutoGenerateColumns="False"
    OnItemCommand="grdItems_ItemCommand" OnNeedDataSource="grdItems_NeedDatasource" OnItemDataBound="grdItems_ItemDataBound"
    AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
    EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false"
    GridLines="None" EnableLinqExpressions="false">
    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
    </ExportSettings>
    <ClientSettings EnableRowHoverStyle="true">
        <Selecting AllowRowSelect="true"></Selecting>
    </ClientSettings>
    <MasterTableView CommandItemDisplay="Top" DataKeyNames="CCI_ID" AllowFilteringByColumn="True">
        <CommandItemSettings ShowAddNewRecordButton="false" AddNewRecordText="Add new Item" />
        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
        </RowIndicatorColumn>
        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
        </ExpandCollapseColumn>
        <Columns>
            <telerik:GridBoundColumn DataField="ComplianceItem.Name" FilterControlAltText="Filter ItemName column"
                HeaderText="Item Name" SortExpression="ComplianceItem.Name" UniqueName="ComplianceItem.Name">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="ComplianceItem.ItemLabel" FilterControlAltText="Filter ItemLabel column"
                HeaderText="Item Label" SortExpression="ComplianceItem.ItemLabel" UniqueName="ComplianceItem.ItemLabel">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="ComplianceItem.ScreenLabel" FilterControlAltText="Filter ScreenLabel column"
                HeaderText="Screen Label" SortExpression="ComplianceItem.ScreenLabel" UniqueName="ComplianceItem.ScreenLabel">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="ComplianceItem.Description" ItemStyle-CssClass="breakword" FilterControlAltText="Filter Description column"
                HeaderText="Description" SortExpression="ComplianceItem.Description" UniqueName="ComplianceItem.Description">
            </telerik:GridBoundColumn>
          <%--  <telerik:GridBoundColumn DataField="ComplianceItem.Details" ItemStyle-CssClass="breakword" FilterControlAltText="Filter Details column"
                HeaderText="Details" SortExpression="ComplianceItem.Details" UniqueName="ComplianceItem.Details">
            </telerik:GridBoundColumn>--%>
            <telerik:GridBoundColumn DataField="ComplianceItem.TenantName" FilterControlAltText="Filter TenantName column"
                HeaderText="Tenant" SortExpression="ComplianceItem.TenantName" UniqueName="TenantName">
            </telerik:GridBoundColumn>
            <telerik:GridDateTimeColumn DataField="ComplianceItem.EffectiveDate" FilterControlAltText="Filter EffectiveDate column" DataFormatString="{0:MM/dd/yyyy}"
                HeaderText="Effective Date" SortExpression="ComplianceItem.EffectiveDate" UniqueName="EffectiveDate" DataType="System.DateTime">
            </telerik:GridDateTimeColumn>

            <telerik:GridTemplateColumn DataField="ComplianceItem.IsActive" FilterControlAltText="Filter IsActive column" DataType="System.Boolean"
                HeaderText="Is Active" SortExpression="ComplianceItem.IsActive" UniqueName="IsActive">
                <ItemTemplate>
                    <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("ComplianceItem.IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                    <asp:HiddenField ID="hdnfComplianceItemID" runat="server" Value='<%#Eval("ComplianceItem.ComplianceItemID")%>' />
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridBoundColumn DataField="CCI_DisplayOrder" FilterControlAltText="Filter Display Order column" DataType="System.Int32"
                HeaderText="Display Order" SortExpression="CCI_DisplayOrder" UniqueName="DisplayOrder">
            </telerik:GridBoundColumn>
            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete"
                Text="Delete" UniqueName="DeleteColumn">
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

