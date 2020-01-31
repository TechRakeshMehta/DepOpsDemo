<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminEntryCustomFormPage.aspx.cs" MasterPageFile="~/Shared/ChildPage.master"
    Inherits="CoreWeb.AdminEntryPortal.Views.AdminEntryCustomFormPage" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnlCustomFormReview" runat="server"></asp:Panel>
    <asp:Panel ID="pnlCustomFormLoad" runat="server"></asp:Panel>
    <asp:HiddenField runat="server" ID="hdnGroupidandIntanceNumberMain" />
    <asp:HiddenField ID="hdnHiddenPanelsMain" runat="server" />
</asp:Content>
