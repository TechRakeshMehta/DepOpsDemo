<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RenewalOrderOptions.ascx.cs"
    Inherits="CoreWeb.ComplianceOperations.Views.RenewalOrderOptions" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style type="text/css">
    .margin-2 {
        margin: 2px;
    }

    .cmdButtonMinSize .sxcbar .RadButton .rbNext {
        left: 60px !important;
    }

        .cmdButtonMinSize .sxcbar .RadButton .rbNext + input {
            padding-left: 0px !important;
        }

    .cancelposition {
        float: right;
        margin-right: 7vh;
    }
</style>
<div class="">
    <asp:Panel ID="pnlRenewOrderOptions" CssClass="section" runat="server">
        <h1 class="mhdr">Renewal Order Options</h1>
        <div class="content">
            <div class="sxform auto">
                <asp:Panel runat="server" CssClass="sxpnl" ID="pnl">
                    <div class='sxro sx1co'>
                        <div class='sxlb'>
                            <span>Are you renewing the same package or switching to a different one?</span>
                        </div>
                        <div class='sxlm m3spn'>
                            <asp:RadioButtonList ID="rbRenewalOrderOptions" runat="server" RepeatDirection="Horizontal"
                                CssClass="radio_list" AutoPostBack="false">
                                <asp:ListItem Enabled="true" Selected="True" Text="Renew Subscription" Value="renew"></asp:ListItem>
                               <%-- <asp:ListItem Enabled="true" Selected="False" Text="Change Subscription" Value="change"></asp:ListItem>--%>
                                <asp:ListItem Enabled="true" Selected="False" Text="Change Program" Value="change"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>

                </asp:Panel>
            </div>
        </div>
    </asp:Panel>
    <div class="cmdButtonMinSize">
        <infsu:CommandBar ID="cmdBar" runat="server" ButtonPosition="Center" DisplayButtons="Submit,Save" DefaultPanel="pnlMain" DefaultPanelButton="Submit"
            AutoPostbackButtons="Submit,Save" SubmitButtonText="Proceed" SaveButtonText="Cancel" SaveButtonIconClass="rbCancel"
            OnSubmitClick="btnProceed_Click" OnSaveClick="btnCancel_Click">
        </infsu:CommandBar>
    </div>

    <%--<div style="text-align: center; padding: 8px;">
        <infs:WclButton ID="btnProceed" runat="server" Text="Proceed" OnClick="btnProceed_Click" />
    </div>--%>
</div>
