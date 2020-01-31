<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequirementDocumentControl.ascx.cs" Inherits="CoreWeb.ClinicalRotation.UserControl.RequirementDocumentControl" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ComplianceOperations/UserControl/PdfDocumentViewer.ascx" TagName="PdfDocumentViewer"
    TagPrefix="uc2" %>
<style type="text/css">
    .cat_highlight
    {
        color: red !important;
    }
</style>
<infs:WclResourceManagerProxy runat="server" ID="prxMP">
    <infs:LinkedResource Path="~/Resources/Mod/ClinicalRotation/RequirementDocumentPanel.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>


<asp:HiddenField runat="server" ID="hdnADEDocVwr" />
<infs:WclButton runat="server" AutoPostBack="false" ID="btnUndockPdfVwr" OnClientClicked="btnUndockClick" Text="UnDock" UseSubmitBehavior="false"></infs:WclButton>
<iframe id="iframePdfDocViewer" runat="server" width="100%" height="95%"></iframe>
