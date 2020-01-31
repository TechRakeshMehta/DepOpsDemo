<%@ Page MasterPageFile="~/Shared/PopupMaster.master" Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceOperations.Views.ImmunizationSummaryReport"
 Codebehind="ImmunizationSummaryReport.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PoupContent" runat="server">
    <rsweb:ReportViewer ID="ssrsReport" runat="server" Height="100%" Width="100%" SizeToReportContent="true">
    </rsweb:ReportViewer>
</asp:Content>
