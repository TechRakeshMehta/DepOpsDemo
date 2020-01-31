<%--<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryDataReportByComplioId.ascx.cs" Inherits="CoreWeb.Reports.CategoryDataReportByComplioId" %>--%>

<%@ Control Language="C#" AutoEventWireup="true"%>
<%@ Register TagPrefix="infsu" TagName="ReportViewer" Src="~/Reports/UserControl/ReportViewer.ascx" %>
<%@ Register TagPrefix="cdr" TagName="ReportViewer" Src="~/Reports/UserControl/CategoryDataReportByComplioID.ascx" %>


<infsu:ReportViewer runat="server" ID="ctlReportViewer" ReportCode="CDRBCI" ShowSearchParameterPanel="true" />
<%--<cdr:ReportViewer runat="server" ID="rptViewer" />--%>