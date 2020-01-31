<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="SelectBuisnessChannel" StylesheetTheme="NoTheme" MasterPageFile="~/Shared/PublicPageMaster.master" Codebehind="SelectBuisnessChannel.aspx.cs" %>

<%@ Register Src="IntsofSecurityModel/SelectBuisnessChannel.ascx" TagName="SelectBuisnessChannel"
    TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeadContent" runat="Server">
    <title>Complio :: Select Business Channel</title>    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="DefaultContent" runat="Server">
    <h1 class="page_header" tabindex="0">
        Select Business Channel</h1>
    <div class="content">
        <uc1:SelectBuisnessChannel ID="SelectBuisnessChannel1" runat="server" />
    </div>
    <asp:Literal ID="litFooter" runat="server" Visible="false"></asp:Literal>
</asp:Content>
