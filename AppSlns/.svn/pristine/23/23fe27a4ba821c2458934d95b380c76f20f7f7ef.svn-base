<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.OrderLineItems" CodeBehind="OrderLineItems.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<div id="divBkgSvcBreakdwnFees" runat="server" visible="false">
    <div class="row" runat="server" id="divOLTHeader">
        <div class="col-md-12">
            <h2 class="heading"><% =Resources.Language.ORDERSRVCITEMS%></h2>
        </div>
    </div>
    <div class="row">
        <div id="divgrdOrderSvcLnePrceInfo" runat="server">

            <infs:WclGrid runat="server" ID="grdOrderServiceLinePriceInfo" AutoGenerateColumns="false"
                AllowSorting="false" AutoSkinMode="True" CellSpacing="0" ShowFooter="true"
                GridLines="Both" ShowAllExportButtons="False" EnableDefaultFeatures="false" OnItemDataBound="grdOrderServiceLinePriceInfo_ItemDataBound">
                <MasterTableView CommandItemDisplay="Top" AllowFilteringByColumn="false" PagerStyle-Font-Bold="true">
                    <CommandItemSettings ShowAddNewRecordButton="false"
                        ShowExportToExcelButton="False" ShowExportToPdfButton="False" ShowExportToCsvButton="False"
                        ShowRefreshButton="False" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="OrderName"
                            HeaderText="<% $Resources:Language, SERVICENAME%>" SortExpression="OrderName" UniqueName="OrderName">
                            <ItemStyle Wrap="true" Width="28%" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Price"
                            HeaderText="<% $Resources:Language, BASEPRICENEW%>" SortExpression="Price" UniqueName="Price" DataFormatString="{0:c}"
                            HeaderStyle-Width="12%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <FooterStyle Font-Bold="true" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ServiceCode"
                            HeaderText="ServiceCode" SortExpression="ServiceCode" UniqueName="ServiceCode"  Display="false">
                            <ItemStyle Wrap="true" Width="28%" />
                        </telerik:GridBoundColumn>
                        

                        <telerik:GridBoundColumn DataField="Quantity"
                            HeaderText="<% $Resources:Language, FINGPRNTCRDCOPIES%>" SortExpression="Quantity" UniqueName="Quantity"
                            HeaderStyle-Width="12%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <FooterStyle Font-Bold="true" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="FCAdditionalPrice"
                            HeaderText="<% $Resources:Language, PRICEPERADDCOPY%>" SortExpression="FCAdditionalPrice" UniqueName="FCAdditionalPrice"
                            HeaderStyle-Width="12%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <FooterStyle Font-Bold="true" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PPQuantity" Visible="false"
                            HeaderText="<% $Resources:Language, PASSPORTPHOTOSETCOPIES%>" SortExpression="PPQuantity" UniqueName="PPQuantity"
                            HeaderStyle-Width="12%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <FooterStyle Font-Bold="true" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="PPAdditionalPrice" Visible="false"
                            HeaderText="<% $Resources:Language, PRICEPERADDSETNEW%>" SortExpression="PPAdditionalPrice" UniqueName="PPAdditionalPrice"
                            HeaderStyle-Width="12%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <FooterStyle Font-Bold="true" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Amount" Aggregate="Sum" FooterText="Total Net Price:$"
                            HeaderText="<% $Resources:Language, NETPRICENEW%>" SortExpression="Amount" UniqueName="Amount" DataFormatString="{0:c}"
                            HeaderStyle-Width="12%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                            <FooterStyle Font-Bold="true" />
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </infs:WclGrid>
        </div>
    </div>
</div>
