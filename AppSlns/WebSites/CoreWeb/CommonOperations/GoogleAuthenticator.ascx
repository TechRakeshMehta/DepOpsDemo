<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoogleAuthenticator.ascx.cs" Inherits="CoreWeb.CommonOperations.Views.GoogleAuthenticator" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<infs:WclResourceManagerProxy ID="LoginSysXResourceManager" runat="server">
    <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/LoginPage.js"
        ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/shared/changepassword.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/shared/changepwd.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>

<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
<script type="text/javascript">

    function ResendOTP() {
        $jQuery("#<%=btnResendOTP.ClientID %>").trigger('click');
    }

</script>

<style type="text/css">
    .errmsg {
        font-size: 90%;
        color: red;
    }

    .error {
        font-size: 100%;
        color: red;
    }
    .success {
    font-size:100%;
    color:green;
    }
</style>

<div id="dvGoogleAuth" runat="server" style="display: none;">
    <div class="note-box">
        <asp:Label CssClass="info" runat="server" ID="lblInstructions" Text="<%$ Resources:Language, PLZENTRVRFCTNCODEMSG %>"></asp:Label>
    </div>
    <div class="fgtfrm">
        <asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label>
        <div>
            &nbsp;
        </div>
        <asp:Panel ID="pnlverifyAuthentication" runat="server">
            <ul class="no-list-style no-box">
                <li id="ilOldPswd" runat="server">
                    <span class="flb">
                        <asp:Label ID="lblVerificationCode" runat="server" AssociatedControlID="txtVerificationCode"
                            Text="<%$ Resources:Language, VERIFICATIONCODE %>" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                    </span>
                    <infs:WclTextBox runat="server" ID="txtVerificationCode" Width="180px">
                    </infs:WclTextBox>
                    <asp:RequiredFieldValidator runat="server" ID="rfvVerificationCode" CssClass="errmsg" ErrorMessage="<%$ Resources:Language, VERIFICATIONCODEREQ %>" Display="Dynamic"
                        ControlToValidate="txtVerificationCode" ValidationGroup="grpValdVerificationCode" />
                </li>
            </ul>
        </asp:Panel>
        <div class="cmds">
            <infs:WclButton ID="btnVerify" runat="server" Text="<%$ Resources:Language, VERIFY %>" OnClick="btnUpdate_Click" ValidationGroup="grpValdVerificationCode">
            </infs:WclButton>
            <infs:WclButton ID="btnCancel" runat="server" Text="<%$ Resources:Language, CNCL %>" OnClick="btnCancel_Click">
            </infs:WclButton>
        </div>
    </div>
</div>
<div id="dvTextMessageAuth" runat="server" style="display: none;">
    <div class="note-box">
        <asp:Label CssClass="info" runat="server" ID="Label1" Text="<%$ Resources:Language, SNDTMPRYVRFCTNCODEMSG %>"></asp:Label>
    </div>
    <div class="fgtfrm">
        <asp:Label ID="lblTextAuthMessage" runat="server" CssClass="error"></asp:Label>
        <asp:Label ID="lblConfirmationMessage" runat="server" CssClass="success"></asp:Label>
        <div>
            &nbsp;
        </div>
        <asp:Panel ID="pnlverifyTextMessageAuth" runat="server">
            <ul class="no-list-style no-box">
                <li id="liTextMessage" runat="server">
                    <span class="flb">
                        <asp:Label ID="lblTextMessage" runat="server" AssociatedControlID="txtTextMessage"
                            Text="<%$ Resources:Language, VERIFICATIONCODE %>" CssClass="cptn"></asp:Label><span class="reqd">*</span>
                    </span>
                    <infs:WclTextBox runat="server" ID="txtTextMessage" Width="180px">
                    </infs:WclTextBox>
                    <i class="fa fa-repeat fa-lg"></i>&nbsp;<a id="lnkResendOTP" runat="server" href="#" onclick="ResendOTP();return false;"><%=Resources.Language.RESENDOTP%></a>
                    <asp:RequiredFieldValidator runat="server" ID="rfvTextMessage" CssClass="errmsg" ErrorMessage="<%$ Resources:Language, VERIFICATIONCODEREQ %>" Display="Dynamic"
                        ControlToValidate="txtTextMessage" ValidationGroup="grpTextMessageCode" />
                </li>
            </ul>
        </asp:Panel>
        <div class="cmds">
            <div style="display: none">
                <infs:WclButton ID="btnResendOTP" runat="server" Text="<%$ Resources:Language, RESENDOTP %>" OnClick="btnResendOTP_Click"></infs:WclButton>
            </div>
            <infs:WclButton ID="btnTextMessageverify" runat="server" Text="<%$ Resources:Language, VERIFY %>" OnClick="btnTextMessageverify_Click" ValidationGroup="grpTextMessageCode">
            </infs:WclButton>
            <infs:WclButton ID="btnTextMessageCancel" runat="server" Text="<%$ Resources:Language, CNCL %>" OnClick="btnCancel_Click">
            </infs:WclButton>
        </div>
    </div>

</div>

