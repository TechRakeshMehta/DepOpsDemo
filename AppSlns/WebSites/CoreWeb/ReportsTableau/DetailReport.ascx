<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailReport.ascx.cs"
    Inherits="CoreWeb.ReportsTableau.Views.DetailReport" %>

<div style="margin: 50px;">
</div>

<script>   

    function BindReport(url) {
        $jQuery('[id$=ifrPage]', $jQuery(parent.theForm)).attr('src', url);
    }


    function GenerateToken(url) {
        $jQuery('[id$=ifrPage]', $jQuery(parent.theForm)).attr('src', url);
    }
</script>


