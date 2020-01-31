<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Shell.Views.Login" StylesheetTheme="NoTheme" CodeBehind="Login.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login :: Complio</title>
    <link href="Resources/Mod/Shared/login.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <%-- <style type="text/css">
        #imgBtnVideo {
            background-image: url("/Resources/Mod/WebSite/video_icon_red.png");
            background-size: 32px 32px;
            height: 32px;
            margin: auto;
            position: absolute;
            right: 190px;
            top: 0;
            width: 32px;
            cursor: pointer;
        }

        .rwStatusbarRow {
            display: none !important;
        }
    </style>--%>
    <style type="text/css">
        .BtnLanguage {
            width: 70% !important;
            background-color: #EFEFEF !important;
            text-align: center !important;
            border-radius: 10px !important;
            font-size: medium !important;
            font-style: normal !important;
            font-family: bold !important;
            color: #333 !important;
        }

        .dvLanguage {
            width: 10% !important;
            float: right !important;
        }
    </style>

    <form id="frmLogin" runat="server" defaultfocus="txtUserName">
        <asp:ScriptManager ID="LoginScriptManager" runat="server" ScriptMode="Release">
        </asp:ScriptManager>
        <infs:WclResourceManager ID="LoginSysXResourceManager" runat="server" SupportedBrowsers="ie7"
            SupportCssPath="~/Resources/Generic/client" SupportScriptPath="~/Resources/Generic/client"
            EnableBrowserSupport="true">
            <infs:LinkedResource Path="~/Resources/Mod/IntsofSecurityModel/Scripts/LoginPage.js"
                ResourceType="JavaScript" />
        </infs:WclResourceManager>
        <%--  <asp:Panel runat="server" CssClass="sxpnl" ID="pnlLogin">--%>
        <div class="base_wrap">
            <div class="col-md-12">
                <div class="row dvLanguage" id="dvLanguage" runat="server">
                    <div class="form-group col-md-3">
                        <infs:WclButton ID="btnLanguage" ControlName="btnLanguage" runat="server" AutoPostBack="true" Visible="true" CausesValidation="false" ButtonType="SkinnedButton"
                            CssClass="BtnLanguage" OnClick="btnLanguage_Click">
                        </infs:WclButton>
                    </div>
                </div>
            </div>

            <asp:HiddenField ID="hdnLanguageCode" runat="server" />
            <asp:PlaceHolder runat="server" ID="phDynamic"></asp:PlaceHolder>
        </div>
        <%--</asp:Panel>--%>
        <%--        <uc:CommonLogin ID="ucCommonLogin" runat="server" Visible="false" />
        <uc:ConfigurableLogin Id="ucConfigurableLogin" runat="server" Visible="false"/>--%>
    </form>
</body>
</html>
<script type="text/javascript">
    function ResendActivationCodeClicked(e) {
        e._enabled = false;
    }

    //AD: displays message
    $jQuery(document).ready(function () {

        if ($jQuery('[id$="hdnAccountVerificationPopup"]').val() == "SHOWPOPUP") {
            //Show Account Verification Popup
            OpenAccountVerificationPopup();
            $jQuery('[id$="hdnAccountVerificationPopup"]').val("");
        }
    });

    //UAT-1078 WB: Add a Icon to the login screens to view the create account help video -- Open Video popup
    function openRadWin() {
        var url = $page.url.create("~/AdbVideos.aspx?VideoCode=CAHV"); //Code AAAA - 
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (70 / 100);

        var win = $window.createPopup(url,
            {
                size: "800," + popupHeight,
                behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize
            },
            function () {
                this.set_title("American Databank | Creating an Account");
                this.set_destroyOnClose(true);
                this.set_status("");
                //this.set_modal(true);
                //this.set_overlay(true);
            });
    }


    function OpenAccountVerificationPopup() {
        var url = $page.url.create("~/AdditionalAccountVerification.aspx?UsrVerCode=" + $jQuery('[id$="hdnVerificationCode"]').val());
        var popupHeight = $jQuery(window).height() * (70 / 100);
        var win = $window.createPopup(url,
            {
                size: "900," + popupHeight,
                behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize, onclose: OnPopupClientClose
            },
            function () {
                this.set_title("American Databank | Verify Complio Account");
                this.set_destroyOnClose(true);
                this.set_status("");
                //this.set_modal(true);
                //this.set_overlay(true);
            });
    }


    function OnPopupClientClose(oWnd, args) {
        oWnd.remove_close(OnPopupClientClose);
        var arg = args.get_argument();
        if (arg) {
            if (arg.RedirectUrl != "") {
                $jQuery('[id$="lblVerificationMessage"]').text("Your account has been activated. Please login to continue.");
                $jQuery('.msgbox:last').fadeIn();
            }
        }
    }

    //function ManageButton(sender, args) {
    //    debugger;
    //    var languageCode;
    //    var language = sender._text;

    //    if (language != null && language.toLowerCase() == "spanish") {
    //        languageCode = 'AAAB';
    //        sender.set_text("English");
    //        sender.set_toolTip("Click for English");
    //    }
    //    if (language != null && language.toLowerCase() == "english") {
    //        languageCode = 'AAAA';
    //        sender.set_text("Spanish");
    //        sender.set_toolTip("Click for Spanish");
    //    }
    //    var hdnLanguageCode = $jQuery('[id$=hdnLanguageCode]');

    //    if (hdnLanguageCode != null && languageCode != null) {
    //        hdnLanguageCode.val(languageCode);
    //    }
    //}

</script>
