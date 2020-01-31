<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.IntsofSecurityModel.Views.SelectBuisnessChannel" Codebehind="SelectBuisnessChannel.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<infs:WclResourceManagerProxy ID="LoginSysXResourceManager" runat="server">
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/LoginPage.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="wrapper">
    <asp:Panel ID="pnlSelectBuisnessChannel" DefaultButton="btnProceed" runat="server">
        <span class="flb">
<%--            <asp:Label ID="lblSxBlocks" runat="server" Text="Business Channel" AssociatedControlID="cmbSxBlocks_Input"
                CssClass="cptn"></asp:Label>--%>
            <label class="cptn" id="lblSxBlocks" for="<%= cmbSxBlocks.ClientID %>_Input">Business Channel</label>
        </span> 
        <infs:WclComboBox ID="cmbSxBlocks" runat="server" DataValueField="SysXBlockId" DataTextField="Name"
            Width="180px" MarkFirstMatch="true" Style="z-index: 7002;" />
    </asp:Panel>
    <br />
    <br />
    <infs:WclButton ID="btnProceed" runat="server" Text="Proceed" OnClick="btnProceed_Click">
    </infs:WclButton>
    <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click">
    </infs:WclButton>
</div>
<script type="text/javascript">
    $jQuery("[id$=cmbSxBlocks]").attr("role", "combobox");</script>
