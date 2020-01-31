<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientProfilePage.aspx.cs" Inherits="CoreWeb.SystemSetUp.Pages.ClientProfilePage"
    MasterPageFile="~/Shared/ChildPage.master" %>

<%@ Register Src="~/SearchUI/UserControl/ClientProfile.ascx" TagPrefix="ucClientProfile" TagName="ClientProfile" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ucClientProfile:ClientProfile runat="server" ID="ucClientProfile" />
</asp:Content>
