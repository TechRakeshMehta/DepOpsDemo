<%@ Control Language="C#" %>
<%@ Register TagPrefix="infsu" TagName="ReportViewer" Src="~/Reports/UserControl/ReportViewer.ascx" %>

<%@ Register TagPrefix="cdr" TagName="ReportViewer" Src="~/Reports/UserControl/ProfileCountReport.ascx" %>

<infsu:ReportViewer runat="server" ID="ctlReportViewer" ReportCode="AGUSERREP" ShowSearchParameterPanel="true" />

<script type="text/javascript">
    $page.add_pageLoaded(function () {
        $jQuery("[id$=txtValue]").attr("autocomplete", "off");
    });
</script>

<%-- Need to make this configurable --%>
<%--<cdr:ReportViewer runat="server" ID="rptViewer" />--%>
