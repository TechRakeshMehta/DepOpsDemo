<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdminEntryPortal_UserControl_AdminEntryApplicantDisclaimer" CodeBehind="AdminEntryApplicantDisclaimer.ascx.cs" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<link href="../../Resources/Mod/Signature/CanvasLayout.css" rel="stylesheet" />

<%--<style type="text/css">
     .RadWindow {
        z-index: 200000 !important;
     }
    </style>--%>
<style type="text/css">
    #imgBtnVideo {
        background-image: url("/../../Resources/Mod/WebSite/video_icon_red.png");
        background-size: 32px 32px;
        height: 32px;
        margin: auto;
        right: -29px;
        /*top: 0;*/
        bottom: 0;
        width: 32px;
        cursor: pointer;
        /*display: inline-block;*/
        position: absolute;
    }

    .dvCommon label {
        font-size: 15px;
        color: red;
        font-weight: bold;
    }

    .borderGreen {
        border: 4px solid green;
    }

    .fontSize15 {
        font-size: 15px;
        font-weight: bold;
    }

    .centerDiv {
        width: 515px;
        margin: 0 auto;
        position: relative;
    }

    .signHere {
        width: 255px;
        float: left;
        text-align: left;
        color: #8C1921;
    }

    .clearSign {
        width: 254px;
        float: right;
        text-align: right;
    }

    #checkboxBig input[type='checkbox'] {
        width: 20px;
        height: 17px;
        -moz-appearance: checkbox; /* Firefox */
    }

    .videoTextAlign {
        position: absolute;
        color: #8C1921;
        font-size: 15px;
        bottom: 0px;
        font-weight: bold;
        right: -106px;
        width: 107px;
    }

    @-moz-document url-prefix() {
        #checkboxBig input[type='checkbox'];

    {
        width: 20px;
        height: 17px;
        outline: #ccc 2px solid;
        -moz-appearance: checkbox;
        outline-offset: -2px;
        margin-top: 1px;
    }

    }

    .msgbox1 .info {
        /*display: block;*/
        padding: 15px 10px 20px 53px;
        border-width: 1px;
        margin: 10px;
    }

    .msgbox1 .info {
        color: #3071cd !important;
        background-image: url('../../Resources/Themes/Default/images/info.png');
        background-color: #fffef0;
        background-position: 10px 8px;
        background-repeat: no-repeat;
        font: 300;
    }
</style>

<asp:Panel ID="pnlDisclaimerMain" runat="server" Width="100%" Height="100%">
    <div class="section">
        <div style="width: 100%">
            <div style="width: 80%; margin: 0 auto; margin-top: 30px">
                <asp:UpdatePanel ID="pnlError" runat="server">
                    <ContentTemplate>
                        <div class="msgbox1" id="dvInfo" runat="server" style="display: none;">
                            <asp:Label CssClass="info" EnableViewState="false" runat="server" ID="lblInfoMsg" Width="93%" Font-Size="Larger"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%-- <h1>
        Disclaimer / Limitation of Liability</h1>--%>
                <%--<h5 style="color:black;padding-top:15px;"><asp:Label ID="lblSignMessage" runat="server">(Scroll down for Signature box.)</asp:Label></h5>--%>
                <div>
                    <h4 style="color: black; padding-bottom: 5px;">
                        <asp:Label ID="lblSignMessage" runat="server">(Scroll down for Signature box)
                        </asp:Label>
                    </h4>
                </div>
                <div style="height: 500px;">
                    <asp:Label ID="lblMessage" runat="server"> </asp:Label>
                    <iframe id="iframePdfDocViewer" runat="server" width="100%" height="100%"></iframe>
                </div>

                <div style="padding-top: 30px; text-align: center">
                    <div style="padding: 10px; text-align: center; font-size: 15px; color: red; font-weight: bold;" id="checkboxBig">
                        <asp:CheckBox runat="server" ID="chkAccept" Text="I Agree" AutoPostBack="false" />
                    </div>
                    <span id="lblSigUnSupport" class="errmsg"></span>
                    <div id="signatureDiv" runat="server" class="sigPad dvCommon" visible="true">
                        <div>
                            <%-- <ul>
                                <li class="clearButton"><a href="#clear">Clear Signature</a></li>
                            </ul>--%>
                            <div class="centerDiv">
                                <div class="signHere fontSize15 dvHeading" style="margin-bottom: 3%">Sign Here</div>
                                <div class="clearSign"><a class="fontSize15 clearButton" href="#clear">Clear Signature</a> &nbsp;</div>
                                <div style="clear: both"></div>
                            </div>
                            <div class="centerDiv">
                                <canvas id="signature" runat="server" class="pad borderGreen" width="500" height="150"></canvas>
                                <%--  UAT-1078 WB: Add a Icon to the login screens to view the create account help video--%>
                                <%--<p><a href="#" onclick="openRadWin()">Click Here for Help Video</a></p>--%>
                                <input type="hidden" name="output" class="output" runat="server" id="hiddenOutput" />
                                <div id="imgBtnVideo" title="Click Here for Help Video." style="margin-right: -3%" onclick="openRadWin()">
                                    <div class="videoTextAlign">Video Tutorial</div>
                                </div>
                            </div>

                            <asp:Label ID="lblErrorChkBox" CssClass="errmsg" Text="Please select 'I Agree'." runat="server" Visible="false" /><br />
                            <asp:Label ID="lblErrorSig" CssClass="errmsg" Text="Please Sign the document." runat="server" Visible="false" />
                        </div>
                    </div>
                </div>

            </div>
            <div class="cmdButtonMinSize">
                <infsu:CommandBar ButtonPosition="Center" ID="fsucCmdBar1" runat="server" DisplayButtons="Submit,Clear"
                    AutoPostbackButtons="Submit,Clear" SubmitButtonText="Restart Order" ClearButtonText="Next"
                    DefaultPanel="pnlDisclaimerMain" DefaultPanelButton="Clear" OnClearClick="fsucCmdBar1_SubmitClick" SubmitButtonIconClass="rbPrevious"
                    OnSubmitClick="fsucCmdBar1_ExtraClick">
                    <ExtraCommandButtons>
                        <infs:WclButton ID="btnCancelOrder" runat="server" Text="Cancel" UseSubmitBehavior="false" CssClass="margin-2 cancelposition"
                            AutoPostBack="true" OnClick="fsucCmdBar1_CancelClick">
                            <Icon PrimaryIconCssClass="rbCancel" />
                        </infs:WclButton>
                    </ExtraCommandButtons>
                </infsu:CommandBar>
            </div>
        </div>
    </div>
</asp:Panel>
<script src="../../Resources/Mod/Signature/jquery.signaturepad.js"></script>
<script type="text/javascript">
    function openRadWin() {
        var url = $page.url.create("~/AdbVideos.aspx?VideoCode=ESHV");
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var win = $window.createPopup(url, {
            size: "800," + popupHeight,
            behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move
        },
            function () {
                this.set_title("American Databank | Signing Forms");
                this.set_destroyOnClose(true);
                this.set_status("");
                //this.set_overlay(true);
            });
    }

    function pageLoad() {
        // var options = { penWidth: 2, drawOnly: true, lineWidth: 0, validateFields: true }
        // $jQuery('.sigPad').signaturePad(options);
        //debugger;

        //UAT-1078 WB: Add a Icon to the login screens to view the create account help video -- Open Video popup


        if (!isCanvasSupported()) {
            $jQuery("[id$=lblSigUnSupport]")[0].innerHTML = "Disclosure form signature is not compatible with your browser. Please use another browser, such as Chrome or Firefox, or upgrade your current browser. If you are unable to either upgrade or use a different browser, please contact American Databank at 1-800-200-0853. Thank you.";
        }
        else {
            var sig = $jQuery("[id$=hiddenOutput]").val();
            if (sig != undefined && sig != "") {
                $jQuery(document).ready(function () {
                    $jQuery('.sigPad').signaturePad({ lineWidth: 0, drawOnly: true }).regenerate(sig);
                });
            }
            else {
                var options = { penWidth: 2, drawOnly: true, lineWidth: 0, validateFields: true }
                $jQuery('.sigPad').signaturePad(options);
            }
        }
    }

    function isCanvasSupported() {

        var elem = document.createElement('canvas');
        return !!(elem.getContext && elem.getContext('2d'));
    }

    //UAT-3182:-Addition of alert pop to display alerting students of the ability to scroll down the page on the document screens in the order process

    setTimeout(function () {
        //debugger;
        ScrollAlertPopUp();
    }, 120000);

    function ScrollAlertPopUp() {
        //debugger;
        $jQuery("[id$=dvInfo]")[0].style.display = "block";
        $jQuery("[id$=lblInfoMsg]").text("Please use the scroll bar on the far right to scroll down to the signature box. The document scroll bar to the immediate right is only to read through the document.");
        $jQuery("[id$=lblInfoMsg]").show();
    }

</script>
<script src="../../Resources/Mod/Signature/json2.min.js"></script>
