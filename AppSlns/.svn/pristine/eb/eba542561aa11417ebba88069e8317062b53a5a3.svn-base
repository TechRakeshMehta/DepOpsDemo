<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceOperations.Views.ReportEmploymentDisclosure" StylesheetTheme="NoTheme" CodeBehind="ReportEmploymentDisclosure.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">--%>
<html>
<head id="Head1" runat="server">
    <title>Employment Disclosure Form</title>

    <style type="text/css">
        #imgBtnVideo
        {
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
            width: 515px;
            margin: 0 auto;
            position: relative;
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

        #checkboxBig input[type='checkbox']
        {
            width: 20px;
            height: 17px;
            -moz-appearance: checkbox; /* Firefox */
        }

        @-moz-document url-prefix()
        {
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
    </style>
</head>
<body>
    <form id="frmMain" runat="server">
        <link href="<%= ResolveUrl("~/App_Themes/Default/core.css") %>" rel="stylesheet" type="text/css" />
        <link href="<%= ResolveUrl("~/Resources/Themes/Default/colors.css") %>" rel="stylesheet" type="text/css" />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Path="~/Resources/Mod/ApplicantRotationRequirement/ViewVideo.js" />
            </Scripts>
        </asp:ScriptManager>

        <div class="section mhdr" id="secDiv" tabindex="1">
            <h2 class="heading">Employment Disclosure Form</h2>
        </div>
        <div class="section">

            <div id="dvPdfDocViewer" runat="server" style="height: 70%;" tabindex="0">
                <iframe id="iframeEmpDisclosure" runat="server" width="100%" height="100%"></iframe>
            </div>
            <div style="height: 30%;">
                <div class="dvCommon sigPad" style="font-size: 11px;">
                    <div style="padding: 10px; text-align: center; font-size: 15px; color: red; font-weight: bold;" id="checkboxBig">
                        <asp:CheckBox runat="server" ID="chkAccept" Text="I have read and agree to the above disclosure form." AutoPostBack="false" />
                    </div>
                </div>
                <div id="dvError" class="dvCommon" style="font-size: 14px; color: red; display: none; text-align: center;">
                    <asp:Label ID="lblAcceptError" role="alert" runat="server" Text="Please agree disclosure form."></asp:Label>
                </div>
                <div style="text-align: center;" class="dvCommon">
                    <infs:WclButton ID="btnSubmit" runat="server" Text="Proceed" OnClick="btnSubmit_Click" OnClientClicking="ValidateForm">
                    </infs:WclButton>
                    <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click">
                    </infs:WclButton>
                </div>

                <asp:Literal ID="litFooter" runat="server" Visible="false"></asp:Literal>
            </div>
        </div>
        <asp:HiddenField ID="hdnTenantId" runat="server" />
        <asp:HiddenField ID="hdnDocumentId" runat="server" />
        <asp:HiddenField ID="hdnOrderId" runat="server" />
        <asp:HiddenField ID="hdnPopupType" runat="server" />
        <asp:HiddenField ID="hdnServiceGroupID" runat="server" />
        <asp:HiddenField ID="hdnBkgPkgSvcGrpID" runat="server" />

        <script type="text/javascript">
            var tabKey = 9;
            $jQuery(document).ready(function () {
                //For accessibility, we need to prevent focus to go outside after tabbing on last link
                $jQuery("[id$=btnCancel]").on("keydown", function (e) {  
                    if (e.keyCode == tabKey && !e.shiftKey) {
                        e.preventDefault();
                        $jQuery("[id$=dvPdfDocViewer]").focus();
                    }

                })
                //For accessibility, we need to prevent focus to go outside after shift tab on firstmost element
                $jQuery("[id$=dvPdfDocViewer]").on("keydown", function (e) {
                   
                    if (e.shiftKey && e.keyCode == tabKey) {                       
                        e.preventDefault();
                        $jQuery("[id$=btnCancel]").focus();
                    }

                 })

            });

            function pageLoad() {
                if ($jQuery('.relpos').length > 0) {
                    $jQuery('.relpos').css('height', '100%');
                }
                $jQuery("[id$=secDiv]").focus();
            }
            function ValidateForm(sender, args) {
                if ($jQuery("[id$=chkAccept]") != undefined && $jQuery("[id$=chkAccept]").length > 0) {
                    if (!$jQuery("[id$=chkAccept]")[0].checked) {
                        $jQuery("[id$=dvError]")[0].style.display = "block"
                        args.set_cancel(true);
                    }
                    else {
                        $jQuery("[id$=dvError]")[0].style.display = "none"
                    }
                }
            }

            //function to get current popup window
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function ClosePopup(status) {
                var oWnd = GetRadWindow();
                var oArg = {};
                oArg.Action = status;
                oArg.TenantId = $jQuery("[id$=hdnTenantId]")[0].value;
                oArg.DocumentId = $jQuery("[id$=hdnDocumentId]")[0].value;
                oArg.OrderId = $jQuery("[id$=hdnOrderId]")[0].value;
                oArg.PopupType = $jQuery("[id$=hdnPopupType]")[0].value; //used in BkgOrderSearch screen
                oArg.ServiceGroupID = $jQuery("[id$=hdnServiceGroupID]")[0].value; //used in BkgOrderSearch screen
                oArg.BkgPkgSvcGrpID = $jQuery("[id$=hdnBkgPkgSvcGrpID]")[0].value; // used in bkgOrderSearch Screen
                if (oArg)
                    oWnd.Close(oArg);
                else
                    oWnd.Close();
                return false;
            }


        </script>
    </form>
    <%--</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CommandContent" runat="server">
</asp:Content>--%>
</body>
</html>
