<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Messaging.Views.TransferRules"
    Title="Message Rules" MasterPageFile="~/Shared/PopupMaster.master" Codebehind="TransferRules.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="RulesGrid" Src="~/Messaging/Pages/TransferRulesGrid.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="Server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxName1">
        <infs:LinkedResource Path="~/Resources/Generic/ref.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Messaging/TransferRules.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Generic/popup.min.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <div class="section">
        <h1 class="mhdr">
            Manage Message Receiving Rules</h1>
        <div class="content">
            <div class="swrap">
                <infsu:RulesGrid ID="RulesGrid" runat="server" />
            </div>
            <div class="gclr">
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="Server">
    <infsu:CommandBar ID="fsucCmdBar1" runat="server" DefaultPanel="pnlName1" DisplayButtons="Cancel"
        OnCancelClientClick="returnToParent" ButtonPosition="Center" />
</asp:Content>
