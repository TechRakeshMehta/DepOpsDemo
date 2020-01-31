<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PopupMaster.master" AutoEventWireup="true" Inherits="Student_UserControls_ViewDocument" Codebehind="ViewDocument.aspx.cs" %>

<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="Server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxName1">
    <infs:LinkedResource Path="~/Resources/Mod/Student/compliances.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
    <asp:Image ImageUrl="~/Resources/Mod/Student/doc1.jpg" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="Server">
    <infsu:CommandBar ID="fsucCmdBar1" runat="server" DefaultPanel="pnlName1" CancelButtonText="Close" DisplayButtons="Save,Cancel" ButtonPosition="Center" OnCancelClientClick="fnPopupClose"/>
</asp:Content>
