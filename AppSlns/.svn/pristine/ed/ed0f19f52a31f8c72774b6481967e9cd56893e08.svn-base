<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicantRequiredDocumentation.ascx.cs"
    Inherits="CoreWeb.ComplianceOperations.Views.ApplicantRequiredDocumentation" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ComplianceOperations/UserControl/RequiredDocumentationLoader.ascx" TagPrefix="infsu" TagName="RequiredDocumentationLoader" %>


<link href="../../Resources/Mod/Signature/CanvasLayout.css" rel="stylesheet" />
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
        -moz-appearance: checkbox;
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
        vertical-align: middle;
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
<script src="../../Resources/Mod/Signature/jquery.signaturepad.js"></script>
<script type="text/javascript">

    function pageLoad() {
        if (!isCanvasSupported()) {
            $jQuery("[id$=lblSigUnSupport]")[0].innerHTML = "Required Documentation form signature is not compatible with your browser. Please use another browser, such as Chrome or Firefox, or upgrade your current browser. If you are unable to either upgrade or use a different browser, please contact American Databank at 1-800-200-0853. Thank you.";
            $jQuery("[id$=signatureDiv]")[0].style("display", "none");
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

    //$jQuery("[id$=lnkbtnViewForm").click(function () { openPopUp(); });
    function openPopUp() {
        $jQuery("[id$=iframePdfDocViewer]").hide();
        var composeScreenWindowName = "Disclosure Form";
        var systemDocumentId = $jQuery("[id$=hdnSystemDocumentId]").val();;
        var documentType = "DisclosureDocument";
        var tenantId = $jQuery("[id$=hdnTenantId]").val();;
        var isApplicantDocument = $jQuery("[id$=hdnIsApplicantDocument]").val();
        var applicantDocumentId = $jQuery("[id$=hdnAppDocId]").val();
        var applicantDocumentType = $jQuery("[id$=hdnDocumentType]").val();
        var url = null;
        if (isApplicantDocument == 'false') {
            url = $page.url.create("~/ComplianceOperations/Pages/FormViewer.aspx?TenantId=" + tenantId + "&systemDocumentId=" + systemDocumentId + "&DocumentType=" + documentType + "&IsApplicantDocument=" + isApplicantDocument);
        }
        else {
            url = $page.url.create("~/ComplianceOperations/Pages/FormViewer.aspx?TenantId=" + tenantId + "&documentId=" + applicantDocumentId + "&DocumentType=" + applicantDocumentType + "&IsApplicantDocument=" + isApplicantDocument);
        }
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Resize, name: composeScreenWindowName, onclose: OnClientClose });
    }

    function isCanvasSupported() {

        var elem = document.createElement('canvas');
        return !!(elem.getContext && elem.getContext('2d'));
    }

    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        $jQuery("[id$=iframePdfDocViewer]").show();
    }

    function CheckIfAllDocsAcceptedAndSigned(sender) {      
        //var len = $jQuery("[id$=rptAdditionalDocuments]").contents().length;
        //for (var i = 0, i = i + 1; i < len;) {
        //    var iframePdfDocViewer = $jQuery("[id$=rptAdditionalDocuments]").contents()[i].find('#iframePdfDocViewer');
        //    iframePdfDocViewer.contents().find('#Button1').click();
        //}       
        //callDocumentViewerSaveButton();
        sender.set_autoPostBack(false);
        var AcceptanceValidationClear = false;
        var lstCheckBox = $jQuery(".dandrDisclosureCheckbox");

        if (lstCheckBox != undefined && lstCheckBox.length > 0) {
            for (i = 0; i < lstCheckBox.length; i++) {
                if ($jQuery(lstCheckBox[i]).children(":first").prop('checked') != true) {
                    var lblValidationMsg = $jQuery(".lblValidationMsg");
                    $jQuery(".lblValidationMsg").text("Please agree and accept required documentation form(s).")
                    return;
                }
            }
            AcceptanceValidationClear = true;
        }

        var signatureValidationClear = false;
        var sig = $jQuery("[id$=hiddenOutput]").val();
        if (sig != undefined && sig != "") { signatureValidationClear = true; }
        else {
            var lblValidationMsg = $jQuery(".lblValidationMsg");
            lblValidationMsg.text("Please sign the required documentation form(s).");
            return;
        }
        if ((signatureValidationClear) && (AcceptanceValidationClear)) {
            var lblValidationMsg = $jQuery(".lblValidationMsg");
            lblValidationMsg.text("");
            //sender.set_autoPostBack(true);
        }
        //debugger;
        var dataString = "hiddenOutput : '" + sig + "'";
        $jQuery.ajax({
            type: "POST",
            url: "Default.aspx/IsShortSignature",
            data: "{ " + dataString + " }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                //debugger;
                if (response.d != true) {
                    lblValidationMsg.text("");
                    var iFrames = $jQuery("[id^=DefaultContent_ucDynamicControl_ucRequiredDocumentationLoader_rptAdditionalDocuments_iframePdfDocViewer]");
                    var len = iFrames.length;
                    var i;
                    for (i = 0; i < len; i++) {
                        var IFrameId = iFrames[i].id;
                        $jQuery("[id$=" + IFrameId + "]").contents().find('#Button1').click();
                    }                    
                    sender.set_autoPostBack(true);
                }
                else {
                    lblValidationMsg.text("Provided text does not qualify as valid Signature.Please provide valid Signature.");
                    return;
                }
            },
            failure: function (response) {
                //debugger;

            }
        });
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
<asp:Panel ID="pnlMain1" runat="server" Height="100%" Width="100%">
    <div class="section">
        <div style="width: 100%">
            <div style="width: 80%; margin: 0 auto; margin-top: 10px">
                <asp:UpdatePanel ID="pnlError" runat="server">
                    <ContentTemplate>
                        <div class="msgbox1" id="dvInfo" runat="server" style="display: none;">
                            <asp:Label CssClass="info" EnableViewState="false" runat="server" ID="lblInfoMsg" Width="93%" Font-Size="Larger"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div>
                    <h1 style="float: left; padding-right: 5px;">Required Documentation</h1>
                    <h4 style="color: black; padding-top: 11px;">
                        <asp:Label ID="lblSignMessage" runat="server">(Scroll down for Signature box)
                        </asp:Label>
                    </h4>
                </div>
                <infsu:RequiredDocumentationLoader runat="server" ID="ucRequiredDocumentationLoader" />

                <div runat="server" id="divPdfDocViewer" style="height: 500px; z-index: -100">
                    <asp:Label ID="lblMessage" runat="server"> </asp:Label>
                    <iframe id="iframePdfDocViewer" runat="server" width="100%" height="100%"></iframe>
                </div>
                <div style="padding-top: 31px; text-align: center">
                    <div style="padding: 10px; text-align: center" id="checkboxBig" class="dvCommon">
                        <asp:CheckBox runat="server" ID="chkAccept" Text="I have read and agree to the above Required Documentation." AutoPostBack="false" CssClass="dandrDisclosureCheckbox" />
                    </div>
                    <span id="lblSigUnSupport" class="errmsg"></span>
                    <asp:Label ID="lblValidationMsg" CssClass="lblValidationMsg errmsg" runat="server"> </asp:Label>
                    <div id="signatureDiv" runat="server" class="sigPad" visible="true">
                        <div runat="server" id="dvLocation" visible="false" style="margin: auto; text-align: center;">
                            <div>
                                <span class="cptn">Location</span>
                            </div>
                            <div>
                                <infs:WclTextBox runat="server" ID="txtLocation"></infs:WclTextBox>
                            </div>
                        </div>

                        <div class="centerDiv">
                            <%--  <ul>
                                <li class="clearButton"><a href="#clear">Clear Signature</a></li>
                            </ul>--%>
                            <div class="centerDiv">
                                <div class="signHere fontSize15 dvHeading" style="margin-bottom: 3%">Sign Here</div>
                                <div class="clearSign"><a class="fontSize15 clearButton" href="#clear">Clear Signature</a> &nbsp;</div>
                                <div style="clear: both"></div>
                            </div>
                            <div class="centerDiv">
                                <canvas id="signature" runat="server" class="pad borderGreen" width="500" height="150"></canvas>
                                <input type="hidden" name="output" class="output" runat="server" id="hiddenOutput" />
                                <%--<div id="imgBtnVideo" title="Click Here for Help Video." onclick="openRadWin()"><div class="videoTextAlign"> Video Tutorial </div></div>--%>
                            </div>
                            <asp:Label ID="lblErrorChkBox" CssClass="errmsg" Text="Please select all the CheckBoxes." runat="server" Visible="false" /><br />
                            <asp:Label ID="lblErrorSig" CssClass="errmsg" Text="Please Sign the document." runat="server" Visible="false" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="cmdButtonMinSize">
                <infsu:CommandBar ButtonPosition="Center" ID="fsucCmdBar1" runat="server" DisplayButtons="Submit,Clear"
                    AutoPostbackButtons="Submit,Clear" SubmitButtonText="Restart Order" ClearButtonText="Accept" OnClearClientClick="CheckIfAllDocsAcceptedAndSigned"
                DefaultPanel="pnlMain1" DefaultPanelButton="Clear" OnClearClick="fsucCmdBar1_SubmitClick" SaveButtonIconClass="rbCancel"
                    SubmitButtonIconClass="rbPrevious" OnSubmitClick="fsucCmdBar1_ExtraClick">
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
<asp:HiddenField ID="hdnIsApplicantDocument" runat="server" Value="false" />
<asp:HiddenField ID="hdnTenantId" runat="server" Value="" />
<asp:HiddenField ID="hdnSystemDocumentId" runat="server" Value="" />
<input type="hidden" runat="server" id="hdnAppDocId" />
<input type="hidden" runat="server" id="hdnDocumentType" />