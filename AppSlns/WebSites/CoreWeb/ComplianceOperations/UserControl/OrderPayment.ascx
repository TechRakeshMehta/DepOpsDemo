<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderPayment.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.OrderPayment" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="PaymentInstructions" Src="~/ComplianceOperations/UserControl/PaymentInstructions.ascx" %>

<infs:WclResourceManagerProxy ID="rmpOrderReview" runat="server">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/OrderReview.js?v=1" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style type="text/css">
    .sxRushOrder {
        padding-top: 0px !important;
    }

    .margin-2 {
        margin: 2px;
    }

    #dvNextBtnStyleInSpanish.nextBtnStyleInSpanish .rbNext + .rbPrimary {
        padding-right: 20px !important;
    }

    .paymentmodeapprovalmsg {
        color: #009900;
        font-size: inherit;
        font: inherit;
    }
    .lineHtCss {
        line-height: 1 !important
    }
</style>

<div class="section">
    <div class="content">
        <div class="sbsection">
            <h1 class="sbhdr"><%=Resources.Language.PRCHSDTL%></h1>
            <div class="sbcontent" id="divPaymentDetailSubContent" runat="server">
                <div class="sxform auto">
                    <asp:Panel runat="server" CssClass="sxpnl" ID="Panel2">

                        <div runat="server" id="dvNoCompliancePackage">
                            <div class='sxro sx3co' runat="server" id="divRushOrder">
                                <div runat="server" id="dvRushOrderSrvc">
                                    <div class='sxlb'>
                                        <span class='cptn'>Rush Order Service</span>
                                    </div>
                                    <div class='sxlm'>
                                        <asp:CheckBox ID="chkRushOrder" runat="server" AutoPostBack="true" OnCheckedChanged="chkRushOrder_CheckedChanged" />
                                    </div>
                                </div>
                                <div id="divRush" runat="server" visible="false">
                                    <div class='sxlb'>
                                        <span class='cptn'>Rush Order Price</span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" Enabled="false" ID="txtRushOrderPrice"
                                            runat="server" MinValue="0" InvalidStyleDuration="100">
                                            <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                                        </infs:WclNumericTextBox>
                                    </div>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                        </div>
                        <%-- <div class='sxro sx3co'>
                            <div class='sxlb' id="dvPaymentTypelb" title="Select a payment method">
                                <span class='cptn'>Payment Type</span><span class="reqd">*</span>
                            </div>
                            <div class='sxlm' id="dvPaymentTypelm">
                                <infs:WclComboBox ID="cmbPaymentModes" runat="server" DataTextField="Name" DataValueField="PaymentOptionID" EmptyMessage="--Select--"
                                    Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab" AutoPostBack="true" OnSelectedIndexChanged="cmbPaymentModes_SelectedIndexChanged" ValidationGroup="grpPaymentModes">
                                </infs:WclComboBox>
                                <div class='vldx'>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvPaymentModes" ControlToValidate="cmbPaymentModes" ValidationGroup="grpPaymentModes"
                                        class="errmsg" Display="Dynamic" ErrorMessage="Please select the payment type." />
                                </div>
                            </div>

                            <div class='sxroend'>
                            </div>
                        </div>--%>
                        <div class="sxro sx3co" id="dvPrice" runat="server" style="display: block;">
                            <div class='sxlb'>
                                <span class='cptn'><%=Resources.Language.TOTALPRICE%></span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" ReadOnly="true" ID="txtTotalPrice"
                                    runat="server" MinValue="0" InvalidStyleDuration="100">
                                    <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                                </infs:WclNumericTextBox>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>



    <div id="divMain">

        <asp:Repeater ID="rptPackages" runat="server" OnItemDataBound="rptPackages_ItemDataBound">
            <ItemTemplate>
                <div class="divPkg">
                    <div class="sxform auto">
                        <asp:Panel runat="server" CssClass="sxpnl lineHtCss" ID="pnlPP">
                            <div class='sxro sx3co'>
                                <%--<div class='sxlb' id="Div1" title="<%$ Resources:Language, PKGNAME %>" runat="server">--%>
                                <%--<div class='sxlb' id="Div1" title="Package Name" runat="server">--%>
                                <div class='sxlb' id="dvPackageName" runat="server">
                                    <span class='cptn' id="spnPackageName" runat="server"></span>
                                </div>
                                <%-- <div class='sxlb' id="dvOrderSelection" title="Order Selection" runat="server">
                                    <span class='cptn'>Order Selection</span>
                                </div>--%>
                                <div class='sxlm'>
                                    <%# Eval("PkgName") %>
                                </div>

                                <%--<div id="dvBasePrice" runat="server">
                                    <div class='sxlb'>
                                        <span class='cptn'>Base Price</span>
                                    </div>
                                    <div class='sxlm'>
                                        <%# Eval("BasePrice") %>
                                    </div>
                                </div>--%>

                                <asp:Panel ID="pnlPaymentType" runat="server">
                                    <div class='sxlb' id="div1" title="<%=Resources.Language.SELPAYMTHD %>">
                                        <%--<div class='sxlb' id="dvPaymentTypelb" title="Select a payment method">--%>
                                        <span class='cptn'><%=Resources.Language.PAYMENTTYPE%></span>
                                    </div>
                                    <div class='sxlm' id="dvPaymentTypelm">
                                        <infs:WclComboBox ID="cmbPaymentModes" runat="server" OnSelectedIndexChanged="_cmbPaymentModes_SelectedIndexChanged"
                                            Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab" AutoPostBack="true">
                                        </infs:WclComboBox>

                                    </div>
                                </asp:Panel>
                            </div>

                            <div class='sxro sx3co' id="dvPaymentByInst" runat="server" style="display: none">
                                <asp:Panel ID="pnlPaymentByInst" runat="server" Visible="false">
                                    <div class='sxlb'>
                                        <span class='cptn'><%=Resources.Language.PAIDBYINST%></span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" ReadOnly="true" ID="txtPaidByInst"
                                            runat="server" MinValue="0" InvalidStyleDuration="100">
                                            <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                                        </infs:WclNumericTextBox>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class='sxro sx3co' id="dvBalanceAmt" runat="server" style="display: none">
                                <asp:Panel ID="pnlBalanceAmt" runat="server" Visible="false">

                                    <div class='sxlb'>
                                        <span class='cptn'><%=Resources.Language.BALANCEAMT%></span>
                                    </div>
                                    <div class='sxlm'>
                                        <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" ReadOnly="true" ID="txtBalanceAmount"
                                            runat="server" MinValue="0" InvalidStyleDuration="100">
                                            <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                                        </infs:WclNumericTextBox>
                                    </div>
                                    <div id="dvBalanceAmount" runat="server">
                                        <div class='sxlb'>
                                            <span class='cptn'><%=Resources.Language.PAYMENTTYPE%></span>
                                        </div>
                                        <div class='sxlm' id="dvPaymentTypeBalanceAmt">
                                            <infs:WclComboBox ID="cmbPaymentModeBalanceAmt" runat="server" OnSelectedIndexChanged="_cmbPaymentModes_SelectedIndexChanged"
                                                Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab" AutoPostBack="true">
                                            </infs:WclComboBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <%--   <div class='sxro sx3co' id="dvAdditionalPrice" runat="server">
                                <div class=''></div>
                                <div class=''></div>
                                <div class='sxlb'>
                                    <span class='cptn'>Additional Price</span>
                                </div>
                                <div class='sxlm'>
                                    <%# Eval("AdditionalPrice") %>
                                </div>
                                <%--   <asp:Panel ID="pnlAdditionalPaymentOption" runat="server">
                                <div class='sxlb'>
                                    <span class='cptn'>Payment Type</span>
                                </div>
                                <div class='sxlm' id="dvPaymentType">
                                    <infs:WclTextBox runat="server" ID="txtPaymentType" ReadOnly="true" Text='<%# Eval("AdditionalPaymentOption")%>'></infs:WclTextBox>
                                </div>
                              
                            </div>--%>
                            <asp:HiddenField ID="hdfPkgId" runat="server" Value='<%# Eval("PkgId") %>' />
                            <asp:HiddenField ID="hdfIsBkgPkg" runat="server" Value='<%# Eval("IsBkgPkg") %>' />
                            <asp:HiddenField ID="hdnAdditionalPaymentOptionID" runat="server" Value='<%# Eval("AdditionalPaymentOptionID") %>' />
                            <asp:Label ID="lblIsApprovalReqdForPaymentModeMsg" runat="server" CssClass="paymentmodeapprovalmsg" Text="" />
                            <asp:Label ID="lblIsPaymentApprovalRequired" runat="server" Visible="true" />
                            <asp:HiddenField ID="hdnIsPaymentApprovalReq" runat="server" Value='<%# Eval("IsApprovalRequired") %>' />
                            <div class='sxroend'>
                            </div>


                        </asp:Panel>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <div runat="server" id="dvCommonBalAmt" visible="false">
            <div class="sxform auto sxpnl">
                <div class='sxro sx3co' id="dvPaymentByInst" runat="server">
                    <asp:Panel ID="pnlPaymentByInst" runat="server">
                        <div class='sxlb'>
                            <span class='cptn'><%=Resources.Language.PAIDBYINST%></span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" ReadOnly="true" ID="txtPaidByInst"
                                runat="server" MinValue="0" InvalidStyleDuration="100">
                                <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                            </infs:WclNumericTextBox>
                        </div>
                    </asp:Panel>
                </div>
                <div class='sxro sx3co' id="dvBalanceAmt" runat="server">
                    <asp:Panel ID="pnlBalanceAmt" runat="server">
                        <div class='sxlb'>
                            <span class='cptn'><%=Resources.Language.BALANCEAMT%></span>
                        </div>
                        <div class='sxlm'>
                            <infs:WclNumericTextBox ShowSpinButtons="false" Type="Currency" ReadOnly="true" ID="txtBalanceAmount"
                                runat="server" MinValue="0" InvalidStyleDuration="100">
                                <NumberFormat AllowRounding="true" DecimalDigits="2" DecimalSeparator="." GroupSizes="3" />
                            </infs:WclNumericTextBox>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>

        <div runat="server" id="dvCommonPaymentSelection" visible="false">
            <div class="sxform auto sxpnl">
                <div class='sxro sx3co'>
                    <asp:Panel class="pnlPaymentTypeCommon" ID="pnlPaymentTypeCommon" runat="server">
                        <div class='sxlb' id="divPaymentTypeCommonHeading" title="<%=Resources.Language.SELPAYMTHD %>">
                            <span class='cptn'><%=Resources.Language.PAYMENTTYPE%></span>
                        </div>
                        <div class='sxlm' id="dvPaymentTypeCommonlm">
                            <infs:WclComboBox class="cmbPaymentModesCommon" ID="cmbPaymentModesCommon" runat="server" OnSelectedIndexChanged="cmbPaymentModesCommon_SelectedIndexChanged"
                                Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab" AutoPostBack="true">
                            </infs:WclComboBox>
                        </div>
                    </asp:Panel>
                    <div class='sxlm' id="dvSplitPaymentTypeByPkg" runat="server">
                        <asp:CheckBox runat="server" ID="chkSplitPaymentTypeByPkg"
                            AutoPostBack="true" OnCheckedChanged="chkSplitPaymentTypeByPkg_CheckedChanged" />
                        <span class='cptn'><%= Resources.Language.SPLITPAYMENTTYPE %></span>
                    </div>
                    <%--<div class='sxlb'>
                        
                    </div>--%>
                </div>
            </div>
        </div>

    </div>
    <div class="sbsection" id="dvAdditionalPaymentTypes" runat="server">
        <h1 class="sbhdr"><%=Resources.Language.ADDPAYDTLS%></h1>
        <asp:Repeater ID="rptrAdtnlPayment" runat="server" OnItemDataBound="rptrAdtnlPayment_ItemDataBound">
            <ItemTemplate>
                <div class="divAdditionalPaymentPkg">
                    <div class="sxform auto">
                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlAdtnlPkg">
                            <div class='sxro sx3co'>
                                <%--<div class='sxlb' title="<%$ Resources:Language, PKGNAME %>">--%>
                                <div class='sxlb' title="Package Name">
                                    <span class='cptn'><%=Resources.Language.PKGNAME%></span>
                                </div>
                                <div class='sxlm'>
                                    <%# Eval("PackageLable")==String.Empty? Eval("PackageName"):Eval("PackageLable")%>
                                </div>
                                <asp:Panel ID="pnlAdditionalPaymentOption" runat="server">
                                    <div class='sxlb'>
                                        <span class='cptn'><%=Resources.Language.PAYMENTTYPE%></span>
                                    </div>
                                    <div class='sxlm' id="dvPaymentType">
                                        <infs:WclTextBox runat="server" ID="txtPaymentType" ReadOnly="true" Text='<%# Eval("AdditionalPaymentOption")%>'></infs:WclTextBox>
                                    </div>
                                </asp:Panel>
                            </div>
                            <asp:HiddenField ID="hdfPkgId" runat="server" Value='<%# Eval("PackageID") %>' />
                            <%-- <asp:HiddenField ID="hdfIsBkgPkg" runat="server" Value='<%# Eval("IsBkgPkg") %>' />--%>
                            <asp:HiddenField ID="hdnAdditionalPaymentOptionID" runat="server" Value='<%# Eval("AdditionalPaymentOptionID") %>' />
                            <div class='sxroend'>
                            </div>

                        </asp:Panel>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <asp:Panel ID="pnlInstructions" runat="server">
    </asp:Panel>

    <div class="content" runat="server" id="dvUserAgreement" visible="false">
        <div class="sbsection">
            <h1 class="sbhdr"><%=Resources.Language.USERAGRMNT%></h1>
            <div class="sbcontent">
                <div class="sxform auto">
                    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlUserAgreement">
                        <div class='sxro sx3co'>
                            <div class='m4spn'>
                                <%--<infs:WclTextBox runat="server" ID="txtUserAgreement" Text="" TextMode="MultiLine" Rows="10" Height="150px" Enabled="false">
                                </infs:WclTextBox>--%>
                                <asp:Literal ID="litText" runat="server"></asp:Literal>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class='sxro sx3co'>
                    <div class='m4spn' style="padding: 10px; text-align: center">
                        <asp:CheckBox runat="server" ID="chkAccept" Text="<%$ Resources:Language, HAVEREADUSERAGRMNT %>"
                            AutoPostBack="false" />
                        <div class='vldx'>
                            <asp:Label ID="lblValidationMsg" CssClass="lblValidationMsg errmsg" runat="server"> </asp:Label>
                            <asp:HiddenField ID="hdnValidationMsg" Value="<%$ Resources:Language, PLZREADUSERAGRMNT %>" runat="server" />
                        </div>
                    </div>

                    <div class='sxroend'>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="Center cmdButtonMinSize">
        <div id="dvNextBtnStyleInSpanish" class="nextBtnStyleInSpanish">
            <infsu:CommandBar ID="cmdbarSubmit" runat="server" ButtonPosition="Center">
                <ExtraCommandButtons>
                    <infs:WclButton ID="btnPrevious" runat="server" Text="<%$ Resources:Language, RSTRTORDR %>" UseSubmitBehavior="false" CssClass="margin-2"
                        AutoPostBack="true" OnClick="btnExtra_Click">
                        <Icon PrimaryIconCssClass="rbPrevious" />
                    </infs:WclButton>
                    <infs:WclButton ID="btnBackOrderReview" runat="server" Text="<%$ Resources:Language, BKTOORDRRVW %>" UseSubmitBehavior="false" CssClass="margin-2"
                        AutoPostBack="true" OnClick="btnSave_Click">
                        <Icon PrimaryIconCssClass="rbPrevious" />
                    </infs:WclButton>
                    <infs:WclButton ID="btnSubmitOrder" runat="server" Text="<%$ Resources:Language, SUBMIT %>" UseSubmitBehavior="false" ValidationGroup="grpPaymentModes" CssClass="margin-2"
                        AutoPostBack="false" OnClientClicked="submitOrder">
                        <Icon PrimaryIconCssClass="rbNext" />
                    </infs:WclButton>
                    <infs:WclButton ID="btnCancelOrder" runat="server" Text="<%$ Resources:Language, CNCL %>" UseSubmitBehavior="false" CssClass="cancelposition"
                        AutoPostBack="true" OnClick="btnCancel_Click">
                        <Icon PrimaryIconCssClass="rbCancel" />
                    </infs:WclButton>
                </ExtraCommandButtons>
            </infsu:CommandBar>
        </div>
    </div>
    <div id="ApprovalMsgPopup" title="Complio" runat="server" style="display: none">
        <p style="text-align: left;">Following package(s) will be processed after the approval from your school for the selected payment type.</p>
        <div>&nbsp;</div>
        <asp:Panel ID="pnlApprovalOrders" runat="server">
        </asp:Panel>
        <div>&nbsp;</div>
        <div>&nbsp;</div>
    </div>
</div>
<asp:HiddenField ID="hdnCredtPymntOptnId" runat="server" />
<asp:HiddenField ID="hdnLanguageCode" Value="AAAA" runat="server" />
<asp:HiddenField ID="hdnIsPaymentByInst" runat="server" Value="false" />
<asp:HiddenField ID="hdnPaymentByInstID" runat="server" />
<asp:HiddenField ID="hdnCreditCardPaymentModeApprovalCode" runat="server" />
<asp:HiddenField ID="hdnIsAnyOptionsApprovalReq" runat="server" Value="" />
<asp:HiddenField ID="hdnIsLocationServiceTenant" runat="server" />
<asp:HiddenField ID="hdnIsCommonPaymentSelection" runat="server" />
<script type="text/javascript">

    function pageLoad() {
        // debugger;
        var LanguageCode = $jQuery("[id$=hdnLanguageCode]").val();
        if (LanguageCode == 'AAAA')
            $jQuery("[id$=dvNextBtnStyleInSpanish]").removeClass("nextBtnStyleInSpanish");
        if (LanguageCode == 'AAAB')
            $jQuery("[id$=dvNextBtnStyleInSpanish]").addClass("nextBtnStyleInSpanish");
    }

</script>

