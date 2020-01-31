<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.OnlinePaymentSubmission" Title="Online Payment Submission" CodeBehind="OnlinePaymentSubmission.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%--<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" method='post'>
        <input id="HiddenValue" type="hidden" value="Initial Value" runat="server" />
        <input type='hidden' runat="server" name='x_login' id='x_login' />
        <input type='hidden' runat="server" name='x_amount' id='x_amount' />
        <input type='hidden' runat="server" name='x_description' id='x_description' />
        <input type='hidden' runat="server" name='x_invoice_num' id='x_invoice_num' />
        <input type='hidden' runat="server" name='x_fp_sequence' id='x_fp_sequence' />
        <input type='hidden' runat="server" name='x_fp_timestamp' id='x_fp_timestamp' />
        <input type='hidden' runat="server" name='x_fp_hash' id='x_fp_hash' />
        <input type='hidden' runat="server" name='x_test_request' id='x_test_request' />
        <input type='hidden' runat="server" name='x_method' id='x_method' />
         <!--Billing Information--> 
        <%--<input type='hidden' runat="server" name='x_cust_id' id='x_cust_id'/>--%>
        <input type='hidden' runat="server" name='x_first_name' id='x_first_name'/>
        <input type='hidden' runat="server" name='x_last_name' id='x_last_name' />
        <input type='hidden' runat="server" name='x_company' id='x_company' />
        <input type='hidden' runat="server" name='x_address' id='x_address' />
        <input type='hidden' runat="server" name='x_city' id='x_city' />
        <input type='hidden' runat="server" name='x_state' id='x_state' />
        <input type='hidden' runat="server" name='x_zip' id='x_zip' />
        <input type='hidden' runat="server" name='x_country' id='x_country' />
        <input type='hidden' runat="server" name='x_email' id='x_email' />
        <input type='hidden' runat="server" name='x_phone' id='x_phone' />
        <input type='hidden' runat="server" name='x_fax' id='x_fax' />
        <input type='hidden' name='x_show_form' value='PAYMENT_FORM' />              
        <div style="text-align: center; width: 50%; margin: 100px">
            <h3>Redirecting, please wait...!!!</h3>
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    window.history.forward();
    document.getElementById("form1").submit();
</script>
