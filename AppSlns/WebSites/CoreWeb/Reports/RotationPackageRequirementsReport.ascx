﻿<%@ Control Language="C#" AutoEventWireup="true" %>

<%@ Register TagPrefix="infsu" TagName="ReportViewer" Src="~/Reports/UserControl/ReportViewer.ascx" %> 

<infsu:ReportViewer runat="server" ID="ctlReportViewer" ReportCode="RPRR"  ShowSearchParameterPanel="false" />