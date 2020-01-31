<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BkgReportViewer.aspx.cs" Inherits="CoreWeb.BkgOperations.Pages.BkgReportViewer" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bkg Report</title>
</head>
<body>
    <style>
        .frmRptVwr
        {
            height: 95%!important;
            text-align: center;
            padding-top: 3px;
            padding-bottom: 3px;
        }

        .dv_btnGoToDB
        {
            text-align: right;
            padding-top: 3px;
            padding-right: 5px;
            margin: 5px;
        }
        #dvError h1
        {
            text-align:center;
            color:red !important;                                                
        }
    </style>
    <form id="form1" runat="server" style="height: 100%;">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <div class="dv_btnGoToDB">
                <infs:WclButton ID="btnGoToDashboard" runat="server" Text="Go To Dashboard" OnClick="btnGoToDashboard_Click"></infs:WclButton>
            </div>
            <div id="dvError" runat="server" visible="false">
                <h1>You are not authorized to view this page. Please contact System Administrator.</h1> <%--UAT-3231--%>
            </div>
        </div>
        <div class="frmRptVwr">
            <iframe id="iframeBkgReportViewer" runat="server" width="95%" height="95%"></iframe>
        </div>
    </form>
</body>
</html>
