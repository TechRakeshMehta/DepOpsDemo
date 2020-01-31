<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.CustomAttributeControlSearchMultipleNodes" CodeBehind="CustomAttributeControlSearchMultipleNodes.ascx.cs" %>

<asp:Panel ID="pnlControlsMode" runat="server">
    <div class='form-group col-md-3'>
        <asp:Label ID="lblLabel" runat="server" CssClass="cptn"></asp:Label><span class="reqd" id="required" visible="false" runat="server">*</span>
        <div id="divControlMode" runat="server">
        </div>
    </div>
</asp:Panel>
<asp:Label ID="lblHdfIdentity" runat="server" Style="display: none"></asp:Label>
<asp:HiddenField ID="hdfCAVId" runat="server" />
<asp:HiddenField ID="hdfIdentity" runat="server" />


