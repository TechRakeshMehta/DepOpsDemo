<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.AdminEntryPortal.Views.AdminEntryPackageDetails" CodeBehind="AdminEntryPackageDetails.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<script>

    function stopColapseInPkgDetails(sender, args) {
        $jQuery("#divPkgDetailMhdr").removeClass("mhdr");
        $jQuery("#divPkgDetailMhdr").addClass("newMhdr");
    }
</script>

<div class="section">

    <div id="divPkgDetailMhdr" class="mhdr" style="position: relative; bottom: 2px;">
        <h1 id="headerPkgDetails" runat="server" style="font-size: 14px; padding-bottom: 2px;">Package Details</h1>
        <%--<div style="right: 20px; position: absolute; z-index: 99999999999; bottom: 20px;">
            <infsu:CommandBar ButtonPosition="Right" ID="cmdbarEditPackage" runat="server" DisplayButtons="Extra" OnExtraClientClick="stopColapseInPkgDetails"
                ExtraButtonText="<%$Resources:Language,CHNGPKGSLECTON %>" OnExtraClick="btnEditPackage_Click" AutoPostbackButtons="Extra">
            </infsu:CommandBar>
        </div>--%>
    </div>
    <div class="content">
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlBeforeSubmission">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class='cptn'><%=Resources.Language.INSTHIERARCHY%></span>
                    </div>
                    <div class='sxlm m3spn'>
                        <asp:Label ID="lblInstitutionHierarchy" runat="server" CssClass="ronly">
                        </asp:Label>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
                <div id="dvCompliancePkgs">
                    <asp:Repeater ID="rptCompliancePkgs" runat="server">
                        <ItemTemplate>
                            <div class='sxro sx3co' id="divCompliancePackage" runat="server" visible="false">

                                <div class='sxlb'>
                                    <span class='cptn'>Immunization Compliance Package</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblPackage" runat="server" CssClass="ronly">
                                    </asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Subscription Period</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblSubscription" runat="server" CssClass="ronly">
                                    </asp:Label>
                                </div>
                                <div id="dvCmplncPkgPrice" runat="server" style="display: block;">
                                    <div class='sxlb'>
                                        <span class='cptn'>Package Price</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" ReadOnly="true" ID="txtPrice"
                                            runat="server" MinValue="0" InvalidStyleDuration="100">
                                            <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                                        </infs:WclNumericTextBox>
                                    </div>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>

                            <div class='sxro sx3co' id="divCompliancePackage_ADMN" runat="server" visible="false">

                                <div class='sxlb'>
                                    <span class='cptn'>Administrative Compliance Package</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblPackage_ADMN" runat="server" CssClass="ronly">
                                    </asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Subscription Period</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblSubscription_ADMN" runat="server" CssClass="ronly">
                                    </asp:Label>
                                </div>
                                <div id="dvCmplncPkgPrice_ADMN" runat="server" style="display: block;">
                                    <div class='sxlb'>
                                        <span class='cptn'>Package Price</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" ReadOnly="true" ID="txtPrice_ADMN"
                                            runat="server" MinValue="0" InvalidStyleDuration="100">
                                            <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                                        </infs:WclNumericTextBox>
                                    </div>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>

                            <asp:HiddenField ID="hdfPackageId" runat="server" Value='<%# Eval("CompliancePackageID") %>' />
                            <asp:HiddenField ID="hdfDPPId" runat="server" Value='<%# Eval("DPP_Id") %>' />
                        </ItemTemplate>

                    </asp:Repeater>
                </div>
                <div id="divBackgroundPackage" runat="server" style="display: none">
                    <asp:Repeater ID="rptBackgroundPackages" runat="server" OnItemDataBound="rptBackgroundPackages_ItemDataBound">
                        <ItemTemplate>
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <asp:Label ID="lblOrderSelection" runat="server" CssClass="cptn"></asp:Label>
                                </div>
                                <div class='sxlm m2spn'>
                                    <asp:Label ID="lblBkgPackage" runat="server" CssClass="ronly" Text='<%#Eval("BPAName") %>'>
                                    </asp:Label>
                                </div>
                                <div id="dvElement" runat="server" style="display: none;">
                                    <div class='sxlb'>
                                        <span class='cptn'>Base Price</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" ReadOnly="true" ID="txtBkgPackagePrice"
                                            runat="server" MinValue="0" InvalidStyleDuration="100" Text='<%#Eval("BasePrice") %>'>
                                            <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                                        </infs:WclNumericTextBox>
                                    </div>
                                </div>

                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co' id="dvAdditionalPrice" style="display: none" runat="server" visible="false">
                                <div>
                                    <div class="sxlb" style="background-color: #dedede !important">
                                    </div>
                                    <div class="sxlm m2spn">
                                    </div>
                                    <div class='sxlb'>
                                        <span class='cptn'>Additional Price</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" ReadOnly="true" ID="txtAdditionalPrice"
                                            runat="server" MinValue="0" InvalidStyleDuration="100" Text='<%#Eval("AdditionalPrice") %>'>
                                            <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                                        </infs:WclNumericTextBox>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div id="divBkgSvcBreakdwnFees" style="display: none" runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <h2 class="heading">Additional Fee(s)</h2>
                        </div>
                    </div>
                    <div class="row">
                        <div id="divgrdOrderSvcLnePrceInfo" runat="server">
                            <infs:WclGrid runat="server" ID="grdOrderServiceLinePriceInfo" AutoGenerateColumns="false"
                                AllowSorting="false" AutoSkinMode="True" CellSpacing="0" ShowFooter="true"
                                GridLines="Both" ShowAllExportButtons="False" ShowExtraButtons="True" EnableDefaultFeatures="false">

                                <MasterTableView CommandItemDisplay="Top" AllowFilteringByColumn="false" PagerStyle-Font-Bold="true">
                                    <CommandItemSettings ShowAddNewRecordButton="false"
                                        ShowExportToExcelButton="False" ShowExportToPdfButton="False" ShowExportToCsvButton="False"
                                        ShowRefreshButton="False" />
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="BackgroundServiceName"
                                            HeaderText="Service Information" SortExpression="BackgroundServiceName" UniqueName="BackgroundServiceName">
                                            <ItemStyle Wrap="true" Width="40%" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Amount" Aggregate="Sum" FooterText="Total Amount: $"
                                            HeaderText="Amount" SortExpression="Amount" UniqueName="Amount" DataFormatString="{0:c}"
                                            HeaderStyle-Width="15%">
                                            <FooterStyle Font-Bold="true" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="AdjAmount" Aggregate="Sum" FooterText="Total Adj Amount: $"
                                            HeaderText="Adj Amount" SortExpression="AdjAmount" UniqueName="Amount" DataFormatString="{0:c}"
                                            HeaderStyle-Width="15%">
                                            <FooterStyle Font-Bold="true" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="NetAmount" Aggregate="Sum" FooterText="Total Net Amount: $"
                                            HeaderText="Net Amount" SortExpression="NetAmount" UniqueName="NetAmount" DataFormatString="{0:c}"
                                            HeaderStyle-Width="15%">
                                            <FooterStyle Font-Bold="true" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Description"
                                            HeaderText="Adjustment Information" SortExpression="Description" UniqueName="Description" HeaderStyle-Width="15%">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </infs:WclGrid>
                        </div>

                    </div>
                </div>
                <div id="dvTotalPrice" runat="server" style="display: none;">
                    <div class="sxro sx3co" id="dvPaymentByInst" runat="server" style="display:none">
                        <div class="sxlb">
                            <span class='cptn'><%=Resources.Language.PAYMENTBYINST%></span>
                        </div>
                        <div class="sxlm">
                            <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" ReadOnly="true" ID="txtPaymentByInst"
                                runat="server" MinValue="0" InvalidStyleDuration="100">
                                <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                            </infs:WclNumericTextBox>
                        </div>
                    </div>
                     <div class="sxro sx3co" id="dvBalanceAmount" runat="server" style="display:none">
                        <div class="sxlb">
                            <span class='cptn'><%=Resources.Language.BALANCEAMT%></span>
                        </div>
                        <div class="sxlm">
                            <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" ReadOnly="true" ID="txtBalanceAmount"
                                runat="server" MinValue="0" InvalidStyleDuration="100">
                                <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                            </infs:WclNumericTextBox>
                        </div>
                    </div>
                    <div class="sxro sx3co">
                        <div class="sxlb" style="background-color: #dedede !important">
                        </div>
                        <div class="sxlm m2spn">
                        </div>
                        <div class="sxlb">
                            <span class='cptn'><%=Resources.Language.TOTALPRICE%></span>
                        </div>
                        <div class="sxlm">
                            <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" ReadOnly="true" ID="txtTotalPrice"
                                runat="server" MinValue="0" InvalidStyleDuration="100">
                                <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                            </infs:WclNumericTextBox>
                        </div>
                        <div class="sxroend">
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</div>
