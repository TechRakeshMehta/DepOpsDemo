<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManagePaymentProfile.aspx.cs" Inherits="CoreWeb.ComplianceOperations.Pages.ManagePaymentProfile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
<%--    <style type="text/css">
        #test
        {
            overflow-x: hidden;
            overflow-y: hidden;
        }
    </style>--%>

</head>
<body>
    <div>
        <iframe id="iframeAuthorizeNet" name="iframeAuthorizeNet" height="540" width="480"></iframe>
    </div>
    <%--<form id="formAuthorizeNetPage" method="post" action="https://test.authorize.net/profile/manage"
        target="iframeAuthorizeNet" style="display: none;"> action="https://test.authorize.net/profile/manage" --%>
    <form id="formAuthorizeNetPage" method="post" action="https://test.authorize.net/profile/addPayment"
        target="iframeAuthorizeNet">
        <input type='hidden' runat="server" name='token' id='token' />
        <input type='hidden' runat="server" name='paymentProfileId' id='paymentProfileId' />
        <input type='hidden' runat="server" name='shippingAddressId' id='shippingAddressId' />
    </form>
</body>
</html>

<script type="text/javascript">
    window.history.forward();
    document.getElementById("formAuthorizeNetPage").submit();
</script>
