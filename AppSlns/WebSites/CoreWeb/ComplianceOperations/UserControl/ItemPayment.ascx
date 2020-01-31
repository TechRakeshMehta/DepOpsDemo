<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemPayment.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ItemPayment" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="PaymentInstructions" Src="~/ComplianceOperations/UserControl/PaymentInstructions.ascx" %>

<infs:WclResourceManagerProxy ID="rmpOrderReview" runat="server">
    <%--<infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/OrderReview.js" ResourceType="JavaScript" />--%>
</infs:WclResourceManagerProxy>
<style type="text/css">
    .sxRushOrder {
        padding-top: 0px !important;
    }
</style>
<link href="../../App_Themes/Default/core.css" rel="stylesheet" />
<script>
    function openCmbBoxOnTab(sender, e) {
        //console.log(e.get_domEvent().keyCode);
        var code = e.get_domEvent().keyCode
        if (code != 9) {
            if (!sender.get_dropDownVisible())
                sender.showDropDown();
        }
    }
    // To close the popup.
    function ClosePopup() {
        top.$window.get_radManager().getActiveWindow().close();
    }
</script>
<div class="section">
    <div class="content">
        <div class="sbsection">
            <h1 class="sbhdr">Payment Detail</h1>
            <div class="sbcontent" id="divPaymentDetailSubContent" runat="server">
                <div class="sxform auto">
                    <asp:Panel runat="server" CssClass="sxpnl" ID="Panel2">
                        <div class="sxro sx3co" id="dvPrice" runat="server" style="display: block;">
                            <div class='sxlb'>
                                <span class='cptn'>Total Price</span>
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
        <div class="sxform auto">
            <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPP">
                <div class='sxro sx3co'>
                    <div class='sxlb' title="Package Name">
                        <span class='cptn'>Payment Name</span>
                    </div>
                    <div class='sxlm'>
                        <asp:Label runat="server" ID="lblItemName"></asp:Label>
                    </div>
                    <asp:Panel ID="pnlPaymentType" runat="server">
                        <div class='sxlb' id="dvPaymentTypelb" title="Select a payment method">
                            <span class='cptn'>Payment Type</span>
                        </div>
                        <div class='sxlm' id="dvPaymentTypelm">
                            <infs:WclComboBox ID="cmbPaymentModes" Enabled="false" runat="server" OnSelectedIndexChanged="cmbPaymentModes_SelectedIndexChanged"
                                Filter="StartsWith" OnClientKeyPressing="openCmbBoxOnTab" AutoPostBack="false">
                            </infs:WclComboBox>

                        </div>
                    </asp:Panel>
                    <asp:HiddenField ID="hdfPkgId" runat="server" Value='0' />
                    <asp:HiddenField ID="hdfPkgSubscriptionId" runat="server" Value='0' />
                    <div class='sxroend'>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <asp:Panel ID="pnlInstructions" runat="server">
    </asp:Panel>

    <div class="content" runat="server" id="dvUserAgreement" visible="false">
        <div class="sbsection">
            <h1 class="sbhdr">User Agreement</h1>
            <div class="sbcontent">
                <div class="sxform auto">
                    <asp:Panel runat="server" CssClass="sxpnl" ID="pnlUserAgreement">
                        <div class='sxro sx3co'>
                            <div class='m4spn'>
                                <asp:Literal ID="litText" runat="server"></asp:Literal>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class='sxro sx3co'>
                    <div class='m4spn' style="padding: 10px; text-align: center">
                        <asp:CheckBox runat="server" ID="chkAccept" ValidationGroup="grpPaymentModes" Text="I have read the User Agreement and accept it."
                            AutoPostBack="false" />
                        <div class='vldx'>
                            <asp:Label ID="lblValidationMsg" CssClass="lblValidationMsg errmsg" runat="server"> </asp:Label>
                        </div>
                    </div>

                    <div class='sxroend'>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="Center">
        <infsu:CommandBar ID="cmdbarSubmit" runat="server" ButtonPosition="Center">
            <ExtraCommandButtons>
                <infs:WclButton ID="btnCancelOrder" runat="server" Text="Cancel Payment" UseSubmitBehavior="false"
                    AutoPostBack="true" OnClick="btnCancelOrder_Click">
                    <Icon PrimaryIconCssClass="rbCancel" />
                </infs:WclButton>
                <infs:WclButton ID="btnProceedPayment" runat="server" Text="Proceed Payment" UseSubmitBehavior="false" ValidationGroup="grpPaymentModes"
                    AutoPostBack="true" OnClick="btnProceedPayment_Click">
                    <Icon PrimaryIconCssClass="rbNext" />
                </infs:WclButton>
            </ExtraCommandButtons>
        </infsu:CommandBar>
    </div>
</div>
<asp:HiddenField ID="hdnCredtPymntOptnId" runat="server" />
<asp:HiddenField ID="hdnIsOrderCreated" runat="server" Value='false' />
