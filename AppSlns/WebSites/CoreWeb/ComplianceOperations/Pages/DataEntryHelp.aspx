<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceOperations.Views.DataEntryHelp"
    Title="DataEntryHelp" MasterPageFile="~/Shared/PopupMaster.master" Codebehind="DataEntryHelp.aspx.cs" %>
    <%@ Register Src="~/ComplianceOperations/UserControl/ApplicantDataEntryHelp.ascx" TagName="ApplicantDataEntryHelp"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">

<uc1:ApplicantDataEntryHelp ID="ApplicantDataEntryHelp1" runat="server" />

</asp:Content>
