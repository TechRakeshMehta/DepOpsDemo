<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PopupMaster.master" AutoEventWireup="true" CodeBehind="ViewDocumentPopup.aspx.cs" Inherits="CoreWeb.ComplianceAdministration.Views.ViewDocumentPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <link href="../../Resources/Mod/Signature/CanvasLayout.css" rel="stylesheet" />
    <style type="text/css">
        .dvCommon label
        {
            font-size: 15px;
            color: red;
            font-weight: bold;
        }

        .borderGreen
        {
            border: 4px solid green;
        }

        .fontSize15
        {
            font-size: 15px;
            font-weight: bold;
        }

        .centerDiv
        {
            width: 509px;
            margin: 0 auto;
        }

        .signHere
        {
            width: 255px;
            float: left;
            text-align: left;
            color: #8C1921;
        }

        .clearSign
        {
            width: 254px;
            float: right;
            text-align: right;
        }
    </style>
    <infs:WclResourceManagerProxy runat="server" ID="rmpManagePopup">
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/ViewDocument.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>

    <div class="section">
        <div style="width: 100%">
            <div style="width: 90%; margin: 0 auto; height: 400px; margin-top: 0px">
                <%-- <h1>
        Disclaimer / Limitation of Liability</h1>--%>
                <div id="dvPdfDocViewer" runat="server" style="height: 130%;">
                    <asp:Label ID="lblMessage" runat="server"> </asp:Label>
                    <iframe id="iframePdfDocViewer" runat="server" width="100%" height="100%"></iframe>
                </div>
                <div style="padding-top: 10px; text-align: center;" runat="server">
                    <div id="signatureDiv" runat="server" class="sigPad dvCommon" visible="true">
                        <div>
                            <%--  <ul>
                                <li class="clearButton"><a href="#clear">Clear Signature</a></li>
                            </ul>--%>
                            <div class="centerDiv">
                                <div class="signHere fontSize15 dvHeading">Sign Here</div>
                                <div class="clearSign"><a class="fontSize15 clearButton" href="#clear">Clear Signature</a></div>
                                <div style="clear: both"></div>
                            </div>
                            <div>
                                <canvas id="signature" runat="server" class="pad borderGreen" width="500" height="150"></canvas>
                                <%--  UAT-1078 WB: Add a Icon to the login screens to view the create account help video--%>
                                <%-- <p><a href="#" onclick="openRadWin()">Click Here for Help Video</a></p>--%>
                                <input type="hidden" name="output" class="output" runat="server" id="hdnOutput" />
                            </div>
                            <asp:Label ID="lblErrorSig" CssClass="errmsg" Text="Please Sign the document." runat="server" Visible="false" />
                        </div>
                    </div>
                </div>
                <div style="float: right; padding-top: 5px;">
                    <infs:WclButton ID="btnSavePdf" Text="Save" OnClientClicked="btnSavePdfClick" runat="server" OnClick="btnSavePdf_Click"></infs:WclButton>
                    <infs:WclButton ID="btnCancel" Text="Close" OnClientClicked="returnToParent" OnClick="btnCancel_Click" runat="server"></infs:WclButton>
                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript" src="../../Resources/Mod/Signature/jquery.signaturepad.js"></script>
    <script type="text/javascript">
        function pageLoad() {        
            if ($jQuery('[id$=signature]').length == 0) {
                $jQuery('[id$=dvPdfDocViewer]').css('height', '130%');
            }

            if (!isCanvasSupported()) {
                $jQuery("[id$=lblSigUnSupport]")[0].innerHTML = "Form signature is not compatible with your browser. Please use another browser, such as Chrome or Firefox, or upgrade your current browser. If you are unable to either upgrade or use a different browser, please contact American Databank at 1-800-200-0853. Thank you.";
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

        function btnSavePdfClick() {            
            $jQuery("[id$=iframePdfDocViewer]").contents().find('#Button1').click();
        }
    </script>
    <script type="text/javascript" src="../../Resources/Mod/Signature/json2.min.js"></script>

    <asp:HiddenField ID="hfIsRequiredToView" runat="server" />
    <asp:HiddenField ID="hfIsSignatureRequired" runat="server" />
    <asp:HiddenField ID="hfTemporaryApplicantDocPath" runat="server" />
    <asp:HiddenField ID="hdnTenantId" runat="server" />
    <asp:HiddenField ID="hdnIsDocViewed" runat="server" />
    <asp:HiddenField ID="hdnFileName" runat="server" />
    <asp:HiddenField ID="hdnAppDocumentID" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="server">
</asp:Content>
