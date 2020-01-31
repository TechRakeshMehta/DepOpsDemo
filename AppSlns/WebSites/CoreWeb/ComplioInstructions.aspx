<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PublicPageMaster.master" AutoEventWireup="true" 
   StylesheetTheme="NoTheme"  CodeBehind="ComplioInstructions.aspx.cs" Inherits="CoreWeb.ComplioInstructions" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeadContent" runat="Server">
    <title>Complio :: New to Complio</title>
     <style type="text/css">
        
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="DefaultContent" runat="Server">
    <h1 class="page_header">New to Complio?
    </h1>
    <p class="page_ins">
         Account creation must be completed through your institution's unique website. 
        Please check the information provided to you, or contact your institution for the proper URL. 
    </p>
    <infs:WclButton runat="server" ID="btnBack" Text="Cancel" OnClick="btnBack_Click" 
        CssClass="button">
    </infs:WclButton>
</asp:Content>
