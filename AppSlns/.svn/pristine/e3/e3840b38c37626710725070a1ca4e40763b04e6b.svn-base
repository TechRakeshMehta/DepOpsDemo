<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceLevelDetailsForOrder.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.ServiceLevelDetailsForOrder" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<style>
    .hlink
    {
        cursor: pointer;
    }
</style>

<div class="section">
    <h1 class="mhdr">Service Details
    </h1>
    <div class="content">
        <div class="sxform auto">
            <div class="sxpnl">
                <div class="swrap">
                    <infs:WclGrid runat="server" ID="grdPackage" AllowPaging="false" PageSize="10"
                        AutoGenerateColumns="False" AllowSorting="false" GridLines="Both" ShowAllExportButtons="False" AllowFilteringByColumn="false"
                        OnNeedDataSource="grdPackage_NeedDataSource" ShowClearFiltersButton="false" EnableDefaultFeatures="false" OnItemCommand="grdPackage_ItemCommand">
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
                                        OnNeedDataSource="grdServiceGrp_NeedDataSource" OnItemDataBound="grdServiceGrp_ItemDataBound"
                                        ShowClearFiltersButton="false" EnableDefaultFeatures="false" OnItemCommand="grdServiceGrp_ItemCommand">
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
                                                        <asp:HyperLink ID="hlPackageGroupDocument" runat="server" onclick="openReportWithServiceGroupID(this);"
                                                            Visible="true" Target="_blank" CssClass="hlink">
                                                            <asp:Image ID="imgServiceGroupPDF" runat="server" ImageUrl='<%# ImagePath + "/pdf.gif" %>'
                                                                AlternateText="PDF" Visible="true" />
                                                        </asp:HyperLink>
                                                        <asp:Image ID="imgStatusServiceGrp" runat="server" ImageUrl='<%# ImagePath + "/blank.gif" %>' Visible="true" />
                                                        <asp:Label runat="server" ID="lblServiceGroupName" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ServiceGroupName")) ) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnfServiceGroupID" runat="server" Value='<%#Eval("ServiceGroupID") %>' />
                                                         <asp:HiddenField ID="hdnfBkgPkgSvcGrpID" runat="server" Value ='<%#Eval("PackageServiceGroupID") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="ServiceGroupStatus"
                                                    HeaderText="Status" UniqueName="ServiceGroupStatus">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ServiceGroupCompletionDate"
                                                    HeaderText="Completion Date" UniqueName="ServiceGroupCompletionDate">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                            <NestedViewTemplate>
                                                <div class="swrap">
                                                    <infs:WclGrid runat="server" ID="grdServices" AllowPaging="false"
                                                        PageSize="10" AutoGenerateColumns="False" AllowSorting="false" GridLines="Both" ShowClearFiltersButton="false"
                                                        OnNeedDataSource="grdServices_NeedDataSource" OnItemDataBound="grdServices_ItemDataBound"
                                                        ShowAllExportButtons="false" AllowFilteringByColumn="false" PagerStyle-ShowPagerText="false" EnableDefaultFeatures="false">
                                                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="PackageID,ServiceGroupID,ServiceID,PackageServiceGroupID"
                                                            AllowFilteringByColumn="false" HierarchyDefaultExpanded="true">
                                                            <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false"
                                                                ShowExportToCsvButton="false" ShowExportToExcelButton="false" ShowExportToPdfButton="false" />
                                                            <Columns>
                                                                <telerik:GridTemplateColumn DataField="ServiceName"
                                                                    HeaderText="Service Name" UniqueName="ServiceName" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgStatus" runat="server" ImageUrl='<%# ImagePath + "/blank.gif" %>' Visible="true" />
                                                                        <asp:Label runat="server" ID="lblLineItemName" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("ServiceName")) ) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridBoundColumn DataField="ServiceStatus"
                                                                    HeaderText="Status" UniqueName="ServiceStatus" HeaderStyle-Width="200px">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="ServiceForms"
                                                                    HeaderText="Service Forms" UniqueName="ServiceForms">
                                                                </telerik:GridBoundColumn>
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
                <div class="gclr">
                </div>
            </div>
        </div>
    </div>
</div>
<asp:HiddenField ID="hfTenantId" runat="server" />
<asp:HiddenField ID="hfOrderID" runat="server" />

<script type="text/javascript">
    var winopen = false;
    function openReportWithServiceGroupID(sender) {
        var btnID = sender.id;
        //var containerID = btnID.substr(0, btnID.indexOf("btnNotificationPdf"));
        var TenantId = $jQuery("#<%= hfTenantId.ClientID %>").val()
        var hfOrderID = $jQuery("#<%= hfOrderID.ClientID %>").val();
        var containerID = btnID.substr(0, btnID.indexOf("hlPackageGroupDocument"));
        var hdnfServiceGroupID = $jQuery("[id$=" + containerID + "hdnfServiceGroupID]").val();
        var hdnfBkgPkgSvcGrpID = $jQuery("[id$=" + containerID + "hdnfBkgPkgSvcGrpID]").val();
        var documentType = "ReportDocument";
        var reportType = "OrderCompletion";
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var composeScreenWindowName = "Filterd Report Detail";
        var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID=" + hfOrderID + "&DocumentType=" + documentType + "&ServiceGroupID=" + hdnfServiceGroupID + "&ReportType=" + reportType + "&tenantId=" + TenantId + "&BkgPkgSvcGrpID=" + hdnfBkgPkgSvcGrpID);
        var win = $window.createPopup(url, { size: "800,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
        winopen = true;
        return false;
    }

    function openReportWithOrderID(sender) {
        var btnID = sender.id;
        var TenantId = $jQuery("#<%= hfTenantId.ClientID %>").val()
        var hfOrderID = $jQuery("#<%= hfOrderID.ClientID %>").val();
        var documentType = "ReportDocument";
        var reportType = "OrderCompletion";
        var composeScreenWindowName = "Report Detail";
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/BkgOperations/Pages/ServiceFormViewer.aspx?OrderID=" + hfOrderID + "&DocumentType=" + documentType + "&ReportType=" + reportType + "&tenantId=" + TenantId);
        var win = $window.createPopup(url, { size: "800,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
        winopen = true;
        return false;
    }

    function OnClientClose(oWnd, args) {
        oWnd.get_contentFrame().src = ''; //This is added for fixing pop-up close issue in Safari browser.
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            winopen = false;
        }
    }
</script>
