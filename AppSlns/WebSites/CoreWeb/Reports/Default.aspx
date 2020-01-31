<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/DefaultMaster.master" AutoEventWireup="true" StylesheetTheme="NoTheme" Inherits="CoreWeb.Reports.Views.ReportingDefault" Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadContent" Runat="Server">
    <style>
         body {margin:0;padding:0;}
    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="DefaultContent" Runat="Server">
		<asp:PlaceHolder runat="server" ID="phDynamic"></asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>

