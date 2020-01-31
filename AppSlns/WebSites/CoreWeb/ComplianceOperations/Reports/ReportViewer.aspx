<%@ Page MasterPageFile="~/Shared/PopupMaster.master" Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ReportViewer" Src="~/Reports/UserControl/ReportViewer.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PoupContent" runat="server">
    <infsu:ReportViewer runat="server" ID="ctlReportViewer"
        ShowParameterPanel="false" SetPageTitle="false" SizeToReportContent="true" />

</asp:Content>
