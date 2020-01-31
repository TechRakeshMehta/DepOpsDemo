<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Shell.Views.Redirect"
    Title="Redirect" EnableViewState="false" EnableEventValidation="false" EnableViewStateMac="false" Codebehind="Redirect.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" action="">
    <div style="margin: 50px; border: solid 1px; width: 90%; font-size: 13px" align="center">
        <h2>
            <asp:Label ID="lblRedirectTitle" runat="server" Text="Payment Status"></asp:Label>
        </h2>
        <br />
        <fieldset>
            <table style="line-height: 28px">
                <tr>
                    <td>
                        <asp:Literal ID="litResponseMessage" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="submit" id="btnProceed" runat="server" value="Proceed" style="width: 250px" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <script language="javascript" type="text/javascript">
        //window.history.forward();
    </script>
    </form>
</body>
</html>
