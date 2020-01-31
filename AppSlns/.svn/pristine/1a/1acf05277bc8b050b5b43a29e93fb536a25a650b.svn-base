<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.PaypalPaymentSubmission" Title="Paypal Payment Submission" Codebehind="PaypalPaymentSubmission.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="frmPaypalPaymentSubmission" runat="server" method='post'>
    <input id="hdnInitialValue" type="hidden" value="Initial Value" runat="server" />
    <input type="hidden" runat="server" name="cmd" id="cmd" />
    <input type="hidden" runat="server" name="business" id="business" />
    <input type='hidden' runat="server" name='amount' id='amount' />
    <input type='hidden' runat="server" name='invoice' id='invoice' />
    <input type='hidden' runat="server" name='return' id='return' />
    <input type='hidden' runat="server" name='notify_url' id='notify_url' />
    <input type="hidden" runat="server" name='rm' id='rm' />
    <div style="text-align: center; width: 50%; margin: 100px">
        <h3>
            Redirecting, please wait...!!!</h3>
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    document.getElementById("frmPaypalPaymentSubmission").submit();
</script>
