<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Shell.Views.EmploymentDisclosure" StylesheetTheme="NoTheme" MasterPageFile="~/Shared/PublicPageMaster.master" CodeBehind="EmploymentDisclosure.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeadContent" runat="Server">
    <title>Employment Disclosure Form</title>
    <link href="Resources/Mod/ClientAdmin/EmploymentDisclosure.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="DefaultContent" runat="Server">
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
            #checkboxBig input[type='checkbox']

        {
            width: 20px;
            height: 17px;
            outline: #ccc 2px solid;
            -moz-appearance: checkbox;
            outline-offset: -2px;
            margin-top: 1px;
        vertical-align:middle;
        }

        }
    </style>
    <div class="dvHeading">
        <h2 class="heading">Employment Disclosure Form</h2>
    </div>
    <div class="frmRptVwr">
        <iframe id="iframeEmpDisclosure" runat="server" height="100%" width="100%"></iframe>
    </div>
    <div class="dvCommon sigPad" style="font-size: 11px;">
        <div style="padding: 10px; text-align: center; font-size: 15px; color: red; font-weight: bold;" id="checkboxBig">
            <asp:CheckBox runat="server" ID="chkAccept" Text="I have read and agree to the above disclosure form." AutoPostBack="false" />
        </div>
    </div>
    <div id="dvError" class="dvCommon" style="font-size: 10px; color: red; display: none">
        <asp:Label ID="lblAcceptError" runat="server" Text="Please agree disclosure form."></asp:Label>
    </div>
    <div class="dvCommon">
        <infs:WclButton ID="btnSubmit" runat="server" Text="Proceed" OnClick="btnSubmit_Click" OnClientClicking="ValidateForm">
        </infs:WclButton>
        <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click">
        </infs:WclButton>
    </div>


    <asp:Literal ID="litFooter" runat="server" Visible="false"></asp:Literal>

    <script type="text/javascript">

        $jQuery(document).ready(function () {
            if ($jQuery(".client-logo") != undefined) {
                $jQuery(".client-logo").css('display', 'none');
            }
        })

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
    </script>
</asp:Content>
