<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceAdministration.Views.CustomAttributeProfileLoader" CodeBehind="CustomAttributeProfileLoader.ascx.cs" %>
<%@ Register TagName="RowControl" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/CustomAttributeRowProfileControl.ascx" %>
<%@ Register TagName="AttributeControl" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/CustomAttributeProfileControl.ascx" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<h2 class="subhead">
        <asp:Literal ID="litTitle" runat="server"></asp:Literal>
    </h2>
<asp:Panel ID="pnlRows" runat="server">
</asp:Panel>

