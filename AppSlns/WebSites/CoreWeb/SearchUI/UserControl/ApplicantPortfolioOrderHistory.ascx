<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.Search.Views.ApplicantPortfolioOrderHistory" CodeBehind="ApplicantPortfolioOrderHistory.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="row">
    <div class='col-md-12'>
        <h2 class="header-color" tabindex="0">Order History
        </h2>
    </div>
</div>
<div class="row">
    <div id="dvSubscription" runat="server">
        <infs:WclGrid runat="server" ID="grdOrderHistory" AutoSkinMode="True" CellSpacing="0" EnableAriaSupport="true"
            EnableDefaultFeatures="True" ShowAllExportButtons="false" ShowExtraButtons="true" OnItemDataBound="grdOrderHistory_ItemDataBound"
            NonExportingColumns="EditCommandColumn" GridLines="None" OnNeedDataSource="grdOrderHistory_NeedDataSource"
            OnItemCommand="grdOrderHistory_ItemCommand">
            <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                HideStructureColumns="true" Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                Pdf-PageRightMargin="20mm">
                <Excel AutoFitImages="true" />
            </ExportSettings>
            <ClientSettings EnableRowHoverStyle="true" ClientEvents-OnGridCreated="onGridCreated">
                <Selecting AllowRowSelect="true"></Selecting>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrderId,SubscriptionOptionID"
                AllowPaging="false" PageSize="50" AutoGenerateColumns="False" AllowSorting="false"
                AllowFilteringByColumn="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="false"
                    ShowExportToPdfButton="false" ShowExportToCsvButton="false" ShowExportToWordButton="false"
                    ShowRefreshButton="false" />
                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                </RowIndicatorColumn>
                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="OrderNumber" FilterControlAltText="Filter OrderId column"
                        HeaderText="Order Number" SortExpression="OrderNumber" UniqueName="OrderId" HeaderTooltip="This column displays the order number for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="OrderDate" FilterControlAltText="Filter OrderDate column" HeaderTooltip="This column displays the Order Date for each order"
                        HeaderText="Order Date" SortExpression="OrderDate" UniqueName="OrderDate" DataFormatString="{0:MM/dd/yyyy hh:mm tt}">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="InstituteHierarchy" FilterControlAltText="Filter InstituteHierarchy column"
                        HeaderText="Institution Hierarchy" SortExpression="InstituteHierarchy" UniqueName="InstituteHierarchy" HeaderTooltip="This column displays the institution hierarchy for each record in the grid">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PaymentType" FilterControlAltText="Filter PaymentType column"
                        HeaderText="Payment Type" SortExpression="PaymentType" UniqueName="PaymentType" HeaderTooltip="This column displays the Payment Type for each order">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Amount" FilterControlAltText="Filter Amount column" HeaderTooltip="This column displays the price paid for each order"
                        DataFormatString="{0:c}" HeaderText="Amount" SortExpression="Amount" UniqueName="Amount">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="OrderStatusName" FilterControlAltText="Filter OrderStatusName column"
                        HeaderText="Status" SortExpression="OrderStatusName" UniqueName="OrderStatusName" HeaderTooltip="This column displays the current status for each order">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="OrderStatusCode" FilterControlAltText="Filter OrderStatusCode column"
                        HeaderText="OrderStatusCode" SortExpression="OrderStatusCode" UniqueName="OrderStatusCode"
                        Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="DaysLeftToExpire" FilterControlAltText="Filter DaysLeftToExpire column"
                        HeaderText="DaysLeftToExpire" SortExpression="DaysLeftToExpire" UniqueName="DaysLeftToExpire"
                        Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="RushOrderStatus" FilterControlAltText="Filter RushOrderStatus column"
                        HeaderText="RushOrderStatus" SortExpression="RushOrderStatus" UniqueName="RushOrderStatus"
                        Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SubscriptionOptionID" FilterControlAltText="Filter SubscriptionOptionID column"
                        HeaderText="SubscriptionOptionID" SortExpression="SubscriptionOptionID" UniqueName="SubscriptionOptionID"
                        Display="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="PackageSubscriptionArchiveCode" FilterControlAltText="Filter ArchiveState column"
                        HeaderText="Archive State" AllowSorting="false" UniqueName="ArchiveState">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="Cancellation Status" ItemStyle-Wrap="false" UniqueName="CancellationStatus" Display="true" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblNvrCncl" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CancellationStatus"))) %>'></asp:Label>
                            <telerik:RadButton ButtonType="LinkButton" runat="server" ID="btnCancelStatus" CommandName="InfoViewer" Text='<%# INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CancellationStatus"))) %>'>
                            </telerik:RadButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle AlwaysVisible="false" Visible="false" />
            </MasterTableView>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </infs:WclGrid>
    </div>
    <div class="gclr">
    </div>
</div>
<asp:HiddenField runat="server" ID="hdnPopupInfoHtml" />
<script type="text/javascript">

    function showInfoPopup() {
        var data = $jQuery("[id$=hdnPopupInfoHtml]").val();
        $window.showDialog(data, { btnClose: { autoclose: true, "text": "Close" }}, 500, "Cancelled Package(s)");
    };

    $jQuery(document).ready(function () {

        $jQuery("[id$=grdOrderHistory]").find("th").each(function (element) {
            if ($jQuery(this).text() != "" && $jQuery(this).text() != undefined && $jQuery(this).text().length > 1) {
                $jQuery(this).attr("tabindex", "0");
            }
        });
    });

</script>
