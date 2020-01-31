<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PackagePaymentOptions.ascx.cs" Inherits="CoreWeb.ComplianceAdministration.UserControl.PackagePaymentOptions" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<h1 class="mhdr">
    <asp:Label ID="lblPkgPaymentOptns" runat="server" Text="Package Payment Options"></asp:Label>
</h1>
<div class="content">
    <div class="sxform auto">
        <%--<h2 style="padding-left:2px">
            <asp:Label ID="lblPrimaryPricePaymentOption" runat="server" Text="Primary Price Payment Options" CssClass="cptn"></asp:Label>
        </h2>--%>
        <asp:Panel ID="pnlNode" CssClass="sxpnl" runat="server">
            <div class='sxro sx3co'>
                <div class='sxlb'>
                    <span class="cptn">Payment Option</span>
                </div>
                <div class='sxlm m3spn'>
                    <asp:CheckBoxList ID="chkPaymentOption" RepeatDirection="Horizontal" runat="server">
                    </asp:CheckBoxList>
                </div>
                <div class='sxroend'>
                </div>
            </div>
            <div id="dvApprovalRequired" runat="server">
                <div class='sxro sx3co'>
                    <div class='sxlb'>
                        <span class="cptn">Is School Approval Required for Credit Card</span>
                    </div>
                    <div class='sxlm'>
                        <%--<asp:RadioButtonList ID="rbtnApprovalRequiredBeforePayment" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Not Specified" Value="3" Selected="True"></asp:ListItem>
                        </asp:RadioButtonList>--%>
                        <asp:RadioButtonList ID="rbtnApprovalRequiredBeforePayment" runat="server" RepeatDirection="Horizontal"
                            DataTextField="PA_Name" DataValueField="PA_ID" AutoPostBack="false">
                        </asp:RadioButtonList>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <%--<div class="sxform auto">
        <h2>
            <asp:Label ID="lblAdditionalPricePaymentOption" runat="server" Text=" Additional Price Payment Options" CssClass="cptn"></asp:Label>
        </h2>
        <asp:Panel ID="pnlAdditionNode" CssClass="sxpnl" runat="server">
            <div class='sxro sx3co'>
                <div class='sxlb'>
                    <span class="cptn">Payment Option</span>
                </div>
                <div class='sxlm m3spn'>
                    <asp:RadioButtonList ID="rbtnAdditionalPricePaymentOption" RepeatDirection="Horizontal" runat="server" DataTextField="Name" DataValueField="PaymentOptionID">
                    </asp:RadioButtonList>
                </div>
                <div class='sxroend'>
                </div>
            </div>
        </asp:Panel>
    </div>--%>
</div>
