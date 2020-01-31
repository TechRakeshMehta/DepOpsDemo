<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.OrderHistory" CodeBehind="OrderHistory.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/OrderHistory.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    .breakword {
        word-break: break-all;
    }

    .PdfLink {
        float: right;
        width: 180px;
        padding: 0px;
        margin: 0px;
    }

    .PdfImage {
        width: 16px;
        height: 16px;
        float: left;
        padding: 0px;
        margin: 0px;
    }

    .diplayHidden {
        display: none;
    }

    .btn-back {
        padding-left: 1px;
    }
</style>
<div onclick="setPostBackSourceOH(event, this);">
    <%--<asp:UpdatePanel ID="pnlErrorSchuduleInv" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="msgbox" id="pageMsgBoxSchuduleInv" style="overflow-y: auto; max-height: 400px">
                <asp:Label CssClass="info" EnableViewState="false" runat="server" ID="lblError"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>--%>
    <div id="pnlErrorSchuduleInv">
        <div class="msgbox" id="pageMsgBoxSchuduleInv" style="overflow-y: auto; max-height: 400px">
            <asp:Label CssClass="info" EnableViewState="false" runat="server" ID="lblError"></asp:Label>
        </div>
    </div>
    <asp:TextBox ID="hdnPostbacksource" class="postbacksource" runat="server" Style="display: none;" />
    <div class="section">
        <h1 class="mhdr" style="width: 93%; float: left;">
            <%=Resources.Language.ODRHISTORY %>
            <%--Order History--%>
        </h1>
        <div style="float: left; width: 6%">
            <infs:WclButton ID="btnGotoHome" Width="100%" CssClass="btn-back" AutoPostBack="true" Text="Back" Visible="false" runat="server" OnClick="btnGotoHome_Click"></infs:WclButton>
        </div>
        <div class="content" style="width: 100%">
            <div id="dvSubscription" runat="server" class="sxform auto">
                <infs:WclGrid runat="server" ID="grdOrderHistory" AutoSkinMode="True" CellSpacing="0"
                    EnableDefaultFeatures="True" ShowAllExportButtons="false" ShowExtraButtons="true" ShowClearFiltersButton="false"
                    GridLines="None" OnNeedDataSource="grdOrderHistory_NeedDataSource"
                    OnItemDataBound="grdOrderHistory_ItemDataBound" OnPreRender="grdOrderHistory_PreRender"
                    OnItemCommand="grdOrderHistory_ItemCommand">
                    <ExportSettings ExportOnlyData="True" IgnorePaging="True" OpenInNewWindow="True"
                        HideStructureColumns="true" Pdf-PageWidth="297mm" Pdf-PageHeight="210mm" Pdf-PageLeftMargin="20mm"
                        Pdf-PageRightMargin="20mm">
                        <Excel AutoFitImages="true" />
                    </ExportSettings>
                    <ClientSettings EnableRowHoverStyle="true">
                        <ClientEvents OnRowDblClick="grd_rwDbClick" />
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView CommandItemDisplay="Top" DataKeyNames="OrderId,SubscriptionOptionID,OrderNumber,PaymentTypeCode,OrderStatusCode"
                        AllowPaging="false" PageSize="50" AutoGenerateColumns="False" AllowSorting="false"
                        AllowFilteringByColumn="false">
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="false"
                            ShowExportToPdfButton="false" ShowExportToCsvButton="false" ShowExportToWordButton="false" ShowRefreshButton="false" />
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <%--HeaderText="Order Number"--%>
                            <telerik:GridBoundColumn DataField="OrderNumber" FilterControlAltText="Filter OrderId column"
                                HeaderText="<% $Resources:Language, ODRNUMBER %>"
                                SortExpression="OrderNumber" UniqueName="OrderId">
                            </telerik:GridBoundColumn>
                            <%--HeaderText="Order Date"--%>
                            <telerik:GridBoundColumn DataField="OrderDate" FilterControlAltText="Filter OrderDate column"
                                HeaderText="<% $Resources:Language, ODRDATE %>"
                                SortExpression="OrderDate" UniqueName="OrderDate" DataFormatString="{0:MM/dd/yyyy hh:mm tt}">
                            </telerik:GridBoundColumn>
                            <%--HeaderText="Institution Hierarchy"--%>
                            <telerik:GridBoundColumn DataField="InstituteHierarchy" FilterControlAltText="Filter InstituteHierarchy column"
                                HeaderText="<% $Resources:Language, INSTHIERARCHY %>"
                                SortExpression="InstituteHierarchy" UniqueName="InstituteHierarchy">
                            </telerik:GridBoundColumn>
                            <%--HeaderText="Payment Type"--%>
                            <telerik:GridBoundColumn DataField="PaymentType" FilterControlAltText="Filter PaymentType column"
                                HeaderText="<% $Resources:Language, PAYMENTTYPE %>"
                                SortExpression="PaymentType" UniqueName="PaymentType">
                            </telerik:GridBoundColumn>
                            <%--HeaderText="Amount"--%>
                            <telerik:GridBoundColumn DataField="Amount" FilterControlAltText="Filter Amount column"
                                DataFormatString="{0:c}"
                                HeaderText="<% $Resources:Language, AMOUNT %>"
                                SortExpression="Amount" UniqueName="Amount">
                            </telerik:GridBoundColumn>
                            <%--HeaderText="Payment Status"--%>
                            <telerik:GridBoundColumn DataField="OrderStatusName" FilterControlAltText="Filter OrderStatusName column"
                                HeaderText="<% $Resources:Language, PAYMENTSTATUS %>"
                                SortExpression="OrderStatusName" UniqueName="OrderStatusName">
                            </telerik:GridBoundColumn>
                            <%--HeaderText="Order Status"--%>
                            <telerik:GridBoundColumn DataField="BkgOrderStatus" FilterControlAltText="Filter BkgOrderStatus column" Display="false"
                                HeaderText="<% $Resources:Language, ODRSTATUS %>"
                                SortExpression="BkgOrderStatus" UniqueName="BkgOrderStatus">
                            </telerik:GridBoundColumn>
                            <%--HeaderText="Reschedule Appointment"--%>
                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    
                                    <asp:LinkButton ID="lnkRescheduleAppointment" Visible="false" runat="server" oid='<%#Eval("OrderId")%>' CommandName="Reschedule Appointment" Text="<% $Resources:Language, RSCHDLAPPNMNT %>"></asp:LinkButton>
                                    <%-- <asp:HiddenField ID="hdnfOrderID" runat="server" Value='<%#Eval("OrderId") %>' />--%>
                                    <%--OnClientClick="return openOrderPayment(this);--%>
                                </ItemTemplate>
                                <HeaderStyle CssClass="tplcohdr" />
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkModifyShipping" runat="server" Text="<% $Resources:Language, MODIFYSHIPPING %>" Visible="false"
                                        CommandName="ModifyShipping"></asp:LinkButton>
                                    <%--   CommandName="ModifyShipping" OnClientClick="return openModifyShippingPopup(this);"></asp:LinkButton>     --%>
                                    <asp:HiddenField ID="hdnfModifyShippingOrderID" runat="server" Value='<%#Eval("OrderId") %>' />
                                    <asp:HiddenField ID="hdnIfNeworderClick" runat="server" Value="False" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="tplcohdr" />
                            </telerik:GridTemplateColumn>

                            <%--HeaderText="Background Screening"--%>
                            <telerik:GridTemplateColumn
                                HeaderText="<% $Resources:Language, BKGSCREENING %>"
                                Visible="false" UniqueName="ViewCompletionReport">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnOrderCompletion" runat="server" Visible="false" Text="View Result"
                                        OnClientClick="return openReportWithOrderID(this);"></asp:LinkButton>
                                    <asp:HiddenField ID="hdnfOrderID" runat="server" Value='<%#Eval("OrderId") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="tplcohdr" />
                            </telerik:GridTemplateColumn>

                            <%--HeaderText="E-Drug Form"--%>
                            <telerik:GridTemplateColumn
                                HeaderText="<% $Resources:Language, EDRUGFORM %>"
                                Visible="false" UniqueName="ViewEDSLink">
                                <HeaderStyle Width="110" />
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%--<a runat="server" visible="false" id="ancDownldEDS" title="Click here to download the E Drug authorization form">E-Drug Form</a>--%>

                                    <%--ToolTip="Click here to download the E Drug authorization form"--%>
                                    <asp:LinkButton Visible="false" Text="E-Drug Form" ToolTip="<% $Resources:Language, DWNLDEDRUGFORM %>" runat="server" ID="lnkDownldEDS"></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--HeaderText="Service Form(s)"--%>
                            <telerik:GridTemplateColumn ItemStyle-CssClass="breakword"
                                HeaderText="<% $Resources:Language, SERVICEFORMS %>"
                                ItemStyle-Width="200px" Visible="false"
                                UniqueName="ServiceForms">

                                <ItemTemplate>
                                    <asp:Repeater ID="rptServiceForms" runat="server" OnItemDataBound="rptServiceForms_ItemDataBound">
                                        <ItemTemplate>
                                            <div style="text-align: Left;">
                                                <%-- <asp:Image ID="imgServiceGroupPDF" runat="server" ImageUrl='<%# ImagePath + "/pdf.gif" %>'
                                                    AlternateText="PDF" Visible="true" CssClass="hlink" />--%>
                                                <asp:ImageButton ID="imgPDF" AlternateText="PDF" CssClass="hlink" runat="server" Visible="true" ImageUrl='<%# ImagePath + "/pdf.gif" %>' />
                                                <%--Text="Service Form"--%>
                                                <asp:LinkButton Visible="true"
                                                    Text="<% $Resources:Language, SERVICEFORM %>"
                                                    runat="server" CssClass="PdfLink" ID="lnkDownldSvcFrm"></asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                        <SeparatorTemplate>
                                            <div style="clear: both;">
                                            </div>
                                        </SeparatorTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <%--<telerik:GridButtonColumn ButtonType="LinkButton" CommandName="Cancel" Text="Cancel"
                                UniqueName="CancelColumn"  >
                                <HeaderStyle CssClass="tplcohdr" />
                            </telerik:GridButtonColumn>--%>
                            <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="CancelColumn" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <%--   <telerik:RadButton ID="btnCancel" ButtonType="LinkButton" CommandName="Cancel" AutoPostBack="false" OnClientClicking="confirmtext()"
                                        runat="server" Text="Cancel"  BackColor="Transparent" Font-Underline="true" BorderStyle="None" ForeColor="Black">
                                    </telerik:RadButton>--%>
                                    <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel"
                                        CommandName="Cancel"></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="ViewDetail" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <%--Text="View Details"--%>
                                    <telerik:RadButton ID="btnViewDetails" ButtonType="LinkButton" CommandName="ViewDetail"
                                        Text="<% $Resources:Language, VIEWDETAILS %>"
                                        runat="server" BackColor="Transparent" Font-Underline="true" BorderStyle="None" ForeColor="Black">
                                    </telerik:RadButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="" UniqueName="UnArchiveRequestColumn">
                                <ItemTemplate>
                                    <%--Text="Send Un-archive Request"--%>
                                    <asp:LinkButton ID="lnkbtnUnArchiveRequest" runat="server" Visible="false"
                                        Text="<% $Resources:Language, SENDUNACHVREQ %>"
                                        CommandName="UnArchiveRequest"></asp:LinkButton>
                                    <%--Text="Un-archive request sent"--%>
                                    <asp:Label ID="lblUnArchiverequestSent"
                                        Text="<% $Resources:Language, UNACHVREQSENT %>"
                                        Visible="false" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="tplcohdr" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="CompleteOrder" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnCompleteModifyshipping" runat="server" Visible="false"
                                        Text="<% $Resources:Language, CMPLTPYMNT %>"
                                        CommandName="CompleteModifyshipping"></asp:LinkButton>
                                    <%--Text="Complete Your Order"--%>
                                    <asp:LinkButton ID="lbtnCompleteOrder" runat="server" Visible="false"
                                        Text="<% $Resources:Language, CMPLTORDER %>"
                                        CommandName="CompleteOrder"></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="">
                                <ItemTemplate>

                                    <asp:LinkButton ID="lbtnPlaceRushOrder" runat="server" Visible="false"
                                        Text="Place Rush Order"
                                        CommandName="PlaceRushOrder"></asp:LinkButton>
                                    <%--Text="Rush Order Placed"--%>
                                    <asp:Label ID="lblRushOrderPlaced"
                                        Text="<% $Resources:Language, RUSHODRPLCD %>"
                                        Visible="false" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="tplcohdr" />
                            </telerik:GridTemplateColumn>

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
                            <%--<telerik:GridTemplateColumn HeaderText="Auto Renewal">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnAutoRenewal" CssClass="autoRenewalLink" runat="server" Visible="true" Text='<%#!Convert.ToBoolean(Eval("AutomaticRenewalTurnedOff")) ? "ON" : "OFF" %>'
                                    OnClientClick="return ResetAutoRenewalStatus(this);" ToolTip='<%#!Convert.ToBoolean(Eval("AutomaticRenewalTurnedOff")) ? "Click to Turn Off Auto Renewal" : "Click to Turn On Auto Renewal" %>'>
                                </asp:LinkButton>
                                <asp:HiddenField ID="hfOrderID" runat="server" Value='<%#Eval("OrderId") %>' />
                            </ItemTemplate>
                            <HeaderStyle CssClass="tplcohdr" />
                        </telerik:GridTemplateColumn>--%>

                            <telerik:GridTemplateColumn HeaderText="" UniqueName="PrintReciept" Visible="false">
                                <ItemTemplate>
                                    <%--Text="Print Receipt"--%>
                                    <asp:LinkButton ID="lbtnOrderSummary" runat="server" Visible="false"
                                        Text="<% $Resources:Language, PRINTRECPT %>"
                                        OnClientClick="return openSummaryWithOrderID(this);"></asp:LinkButton>
                                    <asp:HiddenField ID="hdnfSummaryOrderID" runat="server" Value='<%#Eval("OrderId") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="tplcohdr" />
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
    </div>
</div>
<asp:HiddenField ID="hfTenantId" runat="server" />
<asp:HiddenField ID="hdfOrderID" runat="server" />
<asp:HiddenField ID="hdfPassport" runat="server" />
<asp:HiddenField ID="hdfFingerPrint" runat="server" />
<asp:HiddenField ID="hdnPrintReceiptPopupText" runat="server" Value="<%$Resources:Language,PRINTRECPT %>" />
<infs:WclButton ID="btnUpdateOrderDetails" CssClass="diplayHidden" AutoPostBack="true" runat="server" OnClick="btnUpdateOrderDetails_Click" />

<script>
    var _openPrintReceiptWinTitle = "<%=Resources.Language.PRINTRECPT %>";

</script>
