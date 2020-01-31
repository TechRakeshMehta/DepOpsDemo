<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BkgPackageServiceDetails.aspx.cs" Title="Service Details" MasterPageFile="~/Shared/PopupMaster.master"
    Inherits="CoreWeb.BkgSetup.Views.BkgPackageServiceDetails" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<%@ Register TagPrefix="infsu" TagName="BkgOrderServiceLinePriceInfo" Src="~/BkgOperations/UserControl/BkgOrderServiceLinePriceInfo.ascx" %>
<%@ Register TagPrefix="infsu" TagName="BkgOrderServiceGroups" Src="~/BkgOperations/UserControl/BkgOrderServiceGroups.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <div class="container-fluid">
        <div class="section">
            <div class="row">
                <div class="col-md-12">
                    <h2 class="header-color" style="font-size: 25px !important;">Background Package</h2>
                </div>
            </div>

            <div class="content">
                <%-- <infs:WclTreeList ID="tlBkgPkgServiceDetails" runat="server" 
                ParentDataKeyNames="ParentNodeID,ParentDataID" 
                OnNeedDataSource="tlBkgPkgServiceDetails_NeedDataSource" 
                OnItemCreated="tlBkgPkgServiceDetails_ItemCreated" AutoGenerateColumns="false"
                 OnPreRender="tlBkgPkgServiceDetails_PreRender">
                <Columns>
                    <telerik:TreeListBoundColumn DataField="Value" UniqueName="Value" HeaderText="Display Name" />
                    <telerik:TreeListBoundColumn DataField="Description" UniqueName="Description" HeaderText="Description" />
                </Columns>
            </infs:WclTreeList>--%>


                <infs:WclGrid runat="server" ID="grdBkgPkgServiceDetails" AllowPaging="false" PageSize="10"
                    AutoGenerateColumns="False" AllowSorting="false" GridLines="Both" ShowAllExportButtons="False" AllowFilteringByColumn="false"
                    OnNeedDataSource="grdBkgPkgServiceDetails_NeedDataSource" OnItemCommand="grdBkgPkgServiceDetails_ItemCommand" ShowClearFiltersButton="false" EnableDefaultFeatures="false">
                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="PackageID" AllowFilteringByColumn="false" HierarchyDefaultExpanded="true">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                            ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"
                            ShowExportToWordButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridBoundColumn DataField="PackageName" UniqueName="PackageName"
                                HeaderText="Package Name">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <NestedViewTemplate>
                            <div class="swrap">
                                <infs:WclGrid runat="server" ID="grdServiceGrp" AllowPaging="false" PageSize="10"
                                    AutoGenerateColumns="False" AllowSorting="false" GridLines="Both" ShowAllExportButtons="False" AllowFilteringByColumn="false"
                                    OnNeedDataSource="grdServiceGrp_NeedDataSource" OnItemCommand="grdServiceGrp_ItemCommand"
                                    ShowClearFiltersButton="false" EnableDefaultFeatures="false">
                                    <ClientSettings EnableRowHoverStyle="true">
                                        <Selecting AllowRowSelect="true"></Selecting>
                                    </ClientSettings>
                                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="ServiceGroupID,PackageServiceGroupID" AllowFilteringByColumn="false" HierarchyDefaultExpanded="true">
                                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                                            ShowExportToExcelButton="false" ShowExportToPdfButton="false" ShowExportToCsvButton="false"
                                            ShowExportToWordButton="false"></CommandItemSettings>
                                        <Columns>
                                            <telerik:GridTemplateColumn DataField="ServiceGroupName" UniqueName="ServiceGroupName"
                                                HeaderText="Service Group Name">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblServiceGroupName" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ServiceGroupName")) )%>'></asp:Label>
                                                    <%--<asp:HiddenField ID="hdnfServiceGroupID" runat="server" Value='<%#Eval("ServiceGroupID") %>' />--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                        <NestedViewTemplate>
                                            <div class="swrap">
                                                <infs:WclGrid runat="server" ID="grdServices" AllowPaging="false"
                                                    PageSize="10" AutoGenerateColumns="False" AllowSorting="false" GridLines="Both" ShowClearFiltersButton="false"
                                                    OnNeedDataSource="grdServices_NeedDataSource"
                                                    ShowAllExportButtons="false" AllowFilteringByColumn="false" PagerStyle-ShowPagerText="false" EnableDefaultFeatures="false">
                                                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="PackageID,ServiceGroupID,ServiceID"
                                                        AllowFilteringByColumn="false" HierarchyDefaultExpanded="true">
                                                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                                                            ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn DataField="ServiceName"
                                                                HeaderText="Service Name" UniqueName="ServiceName" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblLineItemName" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ServiceName")) )%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </infs:WclGrid>
                                            </div>
                                        </NestedViewTemplate>
                                    </MasterTableView>
                                </infs:WclGrid>
                            </div>
                        </NestedViewTemplate>
                    </MasterTableView>
                </infs:WclGrid>
            </div>


            <%--<h1 class="mhdr">Price Detail</h1>--%>

            <div class="row">
                <div class="col-md-12">
                    <h2 class="header-color" style="font-size: 25px !important;">Price Detail</h2>
                    <infsu:BkgOrderServiceLinePriceInfo ID="ucBkgOrderServiceLinePriceInfo" runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <h1 class="header-color" style="font-size: 25px !important;">Service Line Item Details</h1>
                    <infsu:BkgOrderServiceLinePriceInfo ID="BkgOrderServiceLinePriceInfo1" runat="server" />
                </div>
            </div>
            <infsu:BkgOrderServiceGroups ID="ucBkgOrderServiceGroups" runat="server" />
        </div>
    </div>

</asp:Content>
