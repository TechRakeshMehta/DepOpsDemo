<%@ Page Language="C#" MaintainScrollPositionOnPostback="true"  AutoEventWireup="true" CodeBehind="CompliancePackageDetail.aspx.cs"   MasterPageFile="~/Shared/PopupMaster.master" Inherits="CoreWeb.ClinicalRotation.Pages.CompliancePackageDetail" %>

<%@ Register Src="~/ComplianceOperations/UserControl/CompliancePackageDetails.ascx" TagPrefix="uc1" TagName="CompliancePackageDetails" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>

<asp:Content ID="content" ContentPlaceHolderID="PoupContent" runat="Server">
    <%--<div style="overflow: auto; max-height: 550px;">--%>
    <asp:Panel runat="server" ID="pnl"></asp:Panel>
    <%--</div>--%>
</asp:Content>
