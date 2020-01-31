<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="SharedUserRegistration.aspx.cs" 
    Inherits="CoreWeb.Shell.Views.SharedUserRegistration"
    Title="UserRegistration" StylesheetTheme="NoTheme"
    MasterPageFile="~/Shared/PublicPageMaster.master" %>

<%@ Register TagPrefix="uc" TagName="SharedUserAccount" Src="~/ProfileSharing/UserControl/SharedUserCreateAccount.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="DefaultContent" runat="Server">
    <uc:SharedUserAccount ID="SharedUserAccount" runat="server" />
</asp:Content>

