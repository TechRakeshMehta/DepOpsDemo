<%@ Control Language="C#" AutoEventWireup="true" %>

<%@ Register TagPrefix="infsu" TagName="ReportViewer" Src="~/Reports/UserControl/ReportViewer.ascx" %>

<%@ Register TagPrefix="cdr" TagName="ReportViewer" Src="~/Reports/UserControl/ItemDataCountReport.ascx" %>

<infsu:ReportViewer runat="server" ID="ctlReportViewer" ReportCode="IDCR"  ShowSearchParameterPanel="true" />

<script type="text/javascript">
    $page.add_pageLoaded(function () {
        $jQuery("[id$=txtValue]").attr("autocomplete", "off");
    });
</script>

<%--<cdr:ReportViewer runat="server" ID="rptViewer" />--%>