<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.RushOrderReview" CodeBehind="RushOrderReview.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy ID="rmpOrderReview" runat="server">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/OrderReview.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="section">
    <asp:Panel ID="pnlRushOrderReview" runat="server">
        <h1 class="mhdr">Rush Order Review
        </h1>
        <div class="content">
            <div class="section">
                <h1 class="mhdr">Order Detail
                </h1>
                <div class="content">
                    <div class="sxform auto">
                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlOrderDetail">
                            <div class='sxro sx3co'>
                                <div class='sxlb '>
                                    <span class='cptn'>Institution Hierarchy</span>
                                </div>
                                <div class='sxlm m3spn'>
                                    <asp:Label ID="lblInstitutionHierarchy" runat="server" CssClass="ronly">
                                    </asp:Label>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'>Order Number</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblOrderNumber" CssClass="ronly" runat="server"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Package</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblPackage" runat="server" CssClass="ronly">
                                    </asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Package Detail</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblPackageDescription" CssClass="ronly" runat="server"></asp:Label>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <div class="section">
                <h1 class="mhdr">Personal Detail
                </h1>
                <div class="content">
                    <div class="sxform auto">
                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPersonalDetail">
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'>First Name</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblFirstName" CssClass="ronly" runat="server"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Last Name</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblLastName" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Date of Birth</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblDateOfBirth" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                            <div class='sxro sx3co'>
                                <div class='sxlb'>
                                    <span class='cptn'>Email Address</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblEmail" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxlb'>
                                    <span class='cptn'>Phone Number</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblPhone" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxroend'>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <div class="section">
                <h1 class="mhdr">Payment Detail
                </h1>
                <div class="content">
                    <div class="sxform auto">
                        <asp:Panel runat="server" CssClass="sxpnl" ID="pnlPaymentDetail">
                            <div class='sxro sx3co' style="height: 35px">
                                <div class='sxlb'>
                                    <span class='cptn'>Rush Order Price</span>
                                </div>
                                <div class='sxlm'>
                                    <asp:Label ID="lblRushOrderPrice" runat="server" CssClass="ronly"></asp:Label>
                                </div>
                                <div class='sxlb' title="Select a payment method">
                                    <span class='cptn'>Payment Type</span>
                                </div>
                                <div class='sxlm'>
                                    <infs:WclComboBox ID="cmbPaymentOptions" runat="server" DataTextField="Name" DataValueField="PaymentOptionID"
                                        AutoPostBack="true" OnSelectedIndexChanged="cmbPaymentOptions_SelectedIndexChanged">
                                    </infs:WclComboBox>
                                </div>
                                <%--<div class='sxlb' id="dvBillingAddress" runat="server" visible="false">
                                <span class="cptn">Use Billing Address same as Account Address</span>
                            </div>
                            <div class='sxlm'>
                                <infs:WclButton runat="server" ID="chkBillingAddress" ToggleType="CheckBox" ButtonType="ToggleButton" Visible="false"
                                    AutoPostBack="false" Checked="true">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="Yes" Value="True" />
                                        <telerik:RadButtonToggleState Text="No" Value="False" />
                                    </ToggleStates>
                                </infs:WclButton>
                            </div>--%>
                            </div>
                            <div class='sxroend'>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
        <div class="section" runat="server" id="divPaymentInstruction" visible="false">
            <h1 class="mhdr">Payment Instruction</h1>
            <div class="content">
                <div class="sxform auto">
                    <p style="font-weight: bold;">
                        <%--    <asp:Label runat="server" ID="lblPaymentInstruction"></asp:Label>--%>
                        <asp:Literal ID="litPaymentInstruction"
                            runat="server"></asp:Literal>
                    </p>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdnDeptPrgPackageSubscriptionId" runat="server" />
        <infsu:CommandBar ButtonPosition="Center" ID="cmdbarSubmit" runat="server" DisplayButtons="Save,Submit,Clear" DefaultPanel="pnlRushOrderReview" DefaultPanelButton="Clear"
            ClearButtonText="Submit Rush Order" AutoPostbackButtons="Save,Submit" OnClearClientClick="SubmitRushOrder" ClearButtonIconClass="rbNext"
            OnSubmitClick="btnSave_Click" SubmitButtonText="Go To Dashboard" SubmitButtonIconClass="rbPrevious" OnSaveClick="cmdbarSubmit_CancelClick" SaveButtonText="Cancel Order" SaveButtonIconClass="rbCancel">
        </infsu:CommandBar>
    </asp:Panel>
</div>

