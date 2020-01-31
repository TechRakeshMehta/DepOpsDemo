<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.CategoryListing" Codebehind="CategoryListing.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblTitle" runat="server" Text="Categories"></asp:Label>
    </h1>
    <div class="content">
        <div class="swrap">
            <infs:WclGrid runat="server" ID="grdCategory" AllowPaging="True" AutoGenerateColumns="False"
                AllowSorting="True" AllowFilteringByColumn="True" AutoSkinMode="True" CellSpacing="0"
                EnableDefaultFeatures="true" ShowAllExportButtons="false" ShowExtraButtons="false"
                GridLines="None" OnNeedDataSource="grdCategory_NeedDataSource" OnDeleteCommand="grdCategory_DeleteCommand">
                <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True">
                </ExportSettings>
                <ClientSettings EnableRowHoverStyle="true">
                    <Selecting AllowRowSelect="true"></Selecting>
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="ComplianceCategoryID" AllowFilteringByColumn="true">
                    <CommandItemSettings ShowAddNewRecordButton="false" AddNewRecordText="Add New Category" />
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="CategoryName" FilterControlAltText="Filter CategoryName column"
                            HeaderText="Category Name" SortExpression="CategoryName" UniqueName="CategoryName" AllowFiltering="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CategoryLabel" FilterControlAltText="Filter CategoryLabel column"
                            HeaderText="Category Label" SortExpression="CategoryLabel" UniqueName="CategoryLabel" AllowFiltering="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ScreenLabel" FilterControlAltText="Filter ScreenLabel column"
                            HeaderText="Screen Label" SortExpression="ScreenLabel" UniqueName="ScreenLabel" AllowFiltering="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                            HeaderText="Description" SortExpression="Description" UniqueName="Description" AllowFiltering="true">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="ExplanatoryNotes" FilterControlAltText="Filter ExplanatoryNotes column"
                            HeaderText="Explanatory Notes" SortExpression="ExplanatoryNotes" UniqueName="ExplanatoryNotes">
                        </telerik:GridBoundColumn>--%>
                       <%-- <telerik:GridBoundColumn DataField="IsActive" FilterControlAltText="Filter IsActive column"
                            HeaderText="Is Active" SortExpression="IsActive" UniqueName="IsActive">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridTemplateColumn DataField="IsActive" FilterControlAltText="Filter IsActive column" DataType="System.Decimal"
                            HeaderText="Is Active" SortExpression="IsActive" UniqueName="IsActive">
                            <ItemTemplate>
                                <asp:Label ID="IsActive" runat="server" Text='<%# Convert.ToBoolean(Eval("IsActive"))== true ? Convert.ToString("Yes") :Convert.ToString("No") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridBoundColumn DataField="DisplayOrder" FilterControlAltText="Filter DisplayOrder column" DataType="System.Int32"
                            HeaderText="Display Order" SortExpression="DisplayOrder" UniqueName="DisplayOrder" AllowFiltering="true">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TenantName" FilterControlAltText="Filter TenantName column"
                                HeaderText="Tenant" SortExpression="TenantName" UniqueName="TenantName" AllowFiltering="true">
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
        </div>
        <div class="gclr">
        </div>
    </div>
</div>
