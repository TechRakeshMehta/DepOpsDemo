<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAttestationDisclosure.aspx.cs" StylesheetTheme="NoTheme" MasterPageFile="~/Shared/PublicPageMaster.master" Inherits="CoreWeb.Shell.Views.UserAttestationDisclosure" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageHeadContent" runat="Server">
    <title>User Attestation Disclosure Form</title>
    <link href="Resources/Mod/ClientAdmin/EmploymentDisclosure.css" rel="stylesheet" />
    <style type="text/css">
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
            width: 509px;
            margin: 0 auto;
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
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="DefaultContent" runat="Server">
    <%--<script src="../../Resources/Mod/Signature/json2.min.js" type="text/javascript"></script>--%>
    <script src="../../Resources/Mod/Signature/jquery.signaturepad.js" type="text/javascript"></script>
    <script src="Resources/Mod/ClientAdmin/UserAttestationDisclosure.js" type="text/javascript"></script>
    <div class="dvHeading">
        <h2 class="heading">User Attestation Disclosure Form</h2>
    </div>
    <div class="frmRptVwr">
        <iframe tabindex="1" id="iframeAttestDisclosure" runat="server" height="100%" width="100%"></iframe>
    </div>
    <div class="dvCommon" style="padding-top: 20px;" id="checkboxBig">
        <asp:CheckBox TabIndex="2" runat="server" ID="chkAccept" Text="I have read and agree to the above disclosure form." AutoPostBack="false" />
    </div>
    <div id="signatureDiv" runat="server" class="sigPad dvCommon" visible="true">
        <div>
            <div class="centerDiv">
                <div class="signHere fontSize15 dvHeading">Sign Here</div>
                <div tabindex="3" class="clearSign"><a class="fontSize15 clearButton" href="#clear">Clear Signature</a></div>
                <div style="clear: both"></div>
            </div>
            <%-- <ul>
                <li class="clearButton" style="font-size: 10px;"><a href="#clear">Clear Signature</a></li>
            </ul>--%>
            <div style="width: 100%">
                <canvas id="signature" runat="server" class="pad borderGreen" width="500" height="150"></canvas>
                <input type="hidden" name="output" class="output" runat="server" id="hiddenOutput" />
            </div>
            <%--<asp:Label ID="lblErrorChkBox" CssClass="errmsg" Text="Please select all the CheckBoxes." runat="server" Visible="false" /><br />
            <asp:Label ID="lblErrorSig" CssClass="errmsg" Text="Please Sign the document." runat="server" Visible="false" />--%>
        </div>
    </div>
    <div id="dvError" class="dvCommon" style="font-size: 10px; color: red; display: none">
        <asp:Label role="alert" ID="lblAcceptError" runat="server" Text="Please agree disclosure form."></asp:Label>
    </div>
    <div id="dvSignError" class="dvCommon" style="font-size: 10px; color: red; display: none">
        <asp:Label role="alert" ID="lblSignError" CssClass="errmsg" Text="Please Sign the document." runat="server"></asp:Label>
    </div>
    <div id="dvSignQualifyError" runat="server" class="dvCommon" style="font-size: 10px; color: red;" visible="false">
        <asp:Label role="alert" ID="lblSignQualifyError" CssClass="errmsg" Text="Provided text does not qualify as valid Signature. Please provide valid Signature." runat="server"></asp:Label>
    </div>
    <div class="dvCommon">
        <infs:WclButton ID="btnSubmit" runat="server" Text="Proceed" Value="Accept" OnClick="btnSubmit_Click" OnClientClicking="ValidateForm">
        </infs:WclButton>
        <infs:WclButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click">
        </infs:WclButton>
    </div>
    <asp:Literal ID="litFooter" runat="server" Visible="false"></asp:Literal>
</asp:Content>
