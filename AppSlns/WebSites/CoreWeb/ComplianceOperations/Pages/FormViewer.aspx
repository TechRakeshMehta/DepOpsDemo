<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormViewer.aspx.cs" Inherits="CoreWeb.ComplianceOperations.Pages.FormViewer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var height = $(window).height() * (90 / 100);
            $("#iframePdfDocViewer").height(height);
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <iframe id="iframePdfDocViewer" runat="server" width="100%"></iframe>
            </div>
        </div>
    </form>
</body>
</html>
