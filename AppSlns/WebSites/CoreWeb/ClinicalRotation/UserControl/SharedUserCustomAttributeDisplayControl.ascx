<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ClinicalRotation.Views.SharedUserCustomAttributeDisplayControl" CodeBehind="SharedUserCustomAttributeDisplayControl.ascx.cs" %>
<asp:Panel ID="pnlControlsMode" runat="server" Visible="false">
    <div class="form-group col-md-3 col-sm-3">
        <asp:Label ID="lblLabel" runat="server" CssClass="cptn"></asp:Label><span class="reqd"
            id="required" visible="false" runat="server">*</span>
        <div id="divControlMode" runat="server">
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="pnlLabelsMode" runat="server" Visible="false">
    <div class="form-group col-md-3">
        <span>
            <asp:Label ID="lblLabelMode" runat="server"></asp:Label>:&nbsp;
        </span><span style="font-weight: bold">
            <asp:Label ID="lblAttributeValueLabelMode" runat="server"></asp:Label></span>
    </div>
</asp:Panel>
<asp:Panel ID="pnlReadOnlyLabels" runat="server">
    <div class="form-group col-md-3">
        <asp:Label ID="lblReadOnly" runat="server" CssClass="cptn"></asp:Label>
        <asp:Label ID="lblAttributeValueReadOnlyMode" CssClass="ronly" runat="server"></asp:Label>
    </div>
</asp:Panel>
<%--<div class='sxlm' id="divLabelMode" runat="server" visible="false">
</div>--%>
<asp:HiddenField ID="hdfCAVId" runat="server" />
<asp:HiddenField ID="hdfIdentity" runat="server" />
<asp:HiddenField ID="hdfCAId" runat="server" />

<asp:Label ID="lblCAVId" runat="server" Visible="false" ></asp:Label>
<asp:Label ID="lblIdentity" runat="server" Visible="false"></asp:Label>
<asp:Label ID="lblCAId" runat="server" Visible="false"></asp:Label>
