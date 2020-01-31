<%@ Page Title="Default" Language="C#" MasterPageFile="~/Shared/DefaultMaster.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CoreWeb.AgencyHierarchy.Views.Default" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../Resources/Mod/Shared/ApplyNewIcons.js" type="text/javascript"></script>
    <asp:PlaceHolder runat="server" ID="phDynamic"></asp:PlaceHolder>
</asp:Content>
