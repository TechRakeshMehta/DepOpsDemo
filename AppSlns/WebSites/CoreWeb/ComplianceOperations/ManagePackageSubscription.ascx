<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ManagePackageSubscription" Codebehind="ManagePackageSubscription.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="section">
    <h1 class="mhdr">
        <asp:Label ID="lblHeader" Text="Update Package" runat="server" /></h1>
    <div class="content">
        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlSubscription">
            <!-- Insert Rows here -->
            <div class="swrap">
                <infs:WclGrid runat="server" ID="grdSubscription" AllowPaging="True" EnableDefaultFeatures="false"
                    PageSize="10" AutoGenerateColumns="false" AllowSorting="True" GridLines="Both"
                    EnableLinqExpressions="false" no OnItemDataBound="grdSubscription_ItemDataBound"
                    OnNeedDataSource="grdSubscription_NeedDataSource">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView EnableColumnsViewState="false" DataKeyNames="CompliancePackageID">
                        <Columns>
                            <telerik:GridBoundColumn DataField="PackageName" HeaderText="Package Name" SortExpression="PackageName"
                                UniqueName="PackageName" AllowFiltering="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ManagePackageData">
                                <ItemTemplate>
                                    <a id="ancManagePackageData" runat="server">Enter Data</a>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PagerTextFormat="{4} {5} Item(s) in {1} page(s)" />
                    </MasterTableView>
                </infs:WclGrid>
                <%--</infs:SysXAjaxPanel>--%>
            </div>
            <div class="gclr">
            </div>
        </asp:Panel>
    </div>
</div>
