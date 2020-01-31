<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomFormPage.aspx.cs" MasterPageFile="~/Shared/ChildPage.master"
    Inherits="CoreWeb.BkgOperations.Views.CustomFormPage" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnlCustomFormReview" runat="server"></asp:Panel>
    <asp:Panel ID="pnlCustomFormLoad" runat="server"></asp:Panel>
    <asp:HiddenField runat="server" ID="hdnGroupidandIntanceNumberMain" />
    <asp:HiddenField ID="hdnHiddenPanelsMain" runat="server" />
</asp:Content>
