<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.CustomAttributeLoader" Codebehind="CustomAttributeLoader.ascx.cs" %>
<%@ Register TagName="RowControl" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/CustomAttributeRowControl.ascx" %>
<%@ Register TagName="AttributeControl" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/CustomAttributeControl.ascx" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div class="section" runat="server" id="divSection">
    <h2 class="mhdr">
        <asp:Literal ID="litTitle" runat="server"></asp:Literal>
    </h2>
    <div class="content">
        <div id="divForm" runat="server">
            <asp:Panel ID="pnlRows" runat="server">
            </asp:Panel>
        </div>
    </div>
</div>
