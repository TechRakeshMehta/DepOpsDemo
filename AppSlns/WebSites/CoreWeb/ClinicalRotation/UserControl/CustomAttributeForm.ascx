<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomAttributeForm.ascx.cs" Inherits="CoreWeb.ClinicalRotation.Views.CustomAttributeForm" %>

<%@ Register TagName="RowControl" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/CustomAttributeRowControl.ascx" %>
<%@ Register TagName="AttributeControl" TagPrefix="infsu" Src="~/ComplianceAdministration/UserControl/CustomAttributeControl.ascx" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<div runat="server" id="divSection">
    <h1 class="shdr">
        <asp:Literal ID="litTitle" runat="server"></asp:Literal>
    </h1>

    <div id="divForm" runat="server">
        <asp:Panel ID="pnlRows" runat="server">
        </asp:Panel>
    </div>

</div>




