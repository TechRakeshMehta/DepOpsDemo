<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RotationStudentDetailPopup.aspx.cs" MasterPageFile="~/Shared/PopupMaster.master" Inherits="CoreWeb.ClinicalRotation.Pages.RotationStudentDetailPopup" %>

<%@ Register Src="~/ClinicalRotation/UserControl/RotationStudentDetailsPopup.ascx" TagPrefix="uc1" TagName="RotationStudentDetailsPopup" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>

<asp:Content ID="content" ContentPlaceHolderID="PoupContent" runat="Server">
    <asp:Panel runat="server" ID="pnl"></asp:Panel>
</asp:Content>
