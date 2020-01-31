<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BkgOrderCustomForm.ascx.cs" Inherits="CoreWeb.BkgOperations.Views.BkgOrderCustomForm" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<infs:WclResourceManagerProxy ID="rmnprxyBkgOrderCustomForm" runat="server">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>
<style>
    .classImagePdf {
        background: url(../../images/medium/pdf.gif);
        background-position: 0 0;
        background-repeat: no-repeat;
        width: 20px;
        height: 20px;
    }
</style>

<div class="section">
    <div class="content">
        <asp:Panel ID="pnlLoader" runat="server">
        </asp:Panel>
    </div>
    <infs:WclButton runat="server" Style="display: none"></infs:WclButton>
</div>
