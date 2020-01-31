<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.Reports.Views.ReportViewer"
    CodeBehind="ReportViewer.ascx.cs" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<style>
    .description {
        width: 60%;
    }

    .definition {
        font-size: small;
    }

    .msgbox {
        display: none;
        border-color: black !important;
        border-width: 10px !important;
    }

        .msgbox .error {
            display: block;
            padding: 20px;
            padding: 15px 10px 20px 53px;
            border-width: 1px;
            margin: 10px;
            color: #ff8c08 !important;
            background-image: url('../../Resources/Themes/Default/images/error.png');
            background-color: #fffef0;
            background-position: 10px 8px;
            background-repeat: no-repeat;
            border-color: #ff8c08;
        }

        .msgbox .info {
            display: block;
            padding: 20px;
            padding: 15px 10px 20px 53px;
            border-width: 1px;
            margin: 10px;
            color: #3071cd !important;
            background-image: url('../../Resources/Themes/Default/images/info.png');
            background-color: #fffef0;
            background-position: 10px 8px;
            background-repeat: no-repeat;
        }

        .msgbox .sucs {
            display: block;
            padding: 20px;
            padding: 15px 10px 20px 53px;
            border-width: 1px !important;
            margin: 10px;
            color: green !important;
            background-image: url('../../Resources/Themes/Default/images/success.png');
            background-color: #fffef0 !important;
            background-position: 10px 8px;
            background-repeat: no-repeat;
            border-color: green !important;
            border-style: double !important;
        }

    .buttonHidden {
        display: none;
    }

    .colps {
        padding-left: 16px !important;
    }

    #pageoutwr {
        width: 100%;
        height: 100%;
        overflow: visible;
        -webkit-box-sizing: content-box!important;
        font-size: 11px;
        position: relative;
    }

    .auto .RadInput, .auto .riTextBox {
        width: 100%!important;
    }

    body {
        font-family: Helvetia, Arial, sans-serif!important;
    }

    span.errmsg {
        font-size: 90%;
    }

    .reqd {
        font-size: 11px;
        font-family: Helvetia, Arial, sans-serif!important;
    }

    .section, .tabvw {
        padding: 10px;
        margin-bottom: 10px;
    }

        .section .mhdr {
            padding: 0 0 8px 0;
            margin: 0 !important;
            font-size: 133% !important;
            cursor: pointer;
        }

        /*A class that represents the container for section contents*/
        .section .content {
            margin: 0 0 10px;
            overflow-x: auto;
            overflow-y: hidden;
            position: relative;
        }

    .sxform {
        background-color: #DEDEDE;
    }

    .riTextBox {
        height: 22px;
        padding-left: 3px !important;
        -webkit-box-sizing: border-box;
        -moz-box-sizing: border-box;
        box-sizing: border-box;
    }

    .sxpnl .sxro .sxlb, .sxpnl .sxro .sxlm, .sxpnl .sxtable td {
        border-style: solid;
    }

    .sxpnl {
        padding: 1px 0 1px 5px;
        margin: 0;
        -webkit-box-sizing: border-box;
        -moz-box-sizing: border-box;
        box-sizing: border-box;
    }

    h1, h2, h3, h4, h5, h6 {
        position: relative;
        font-weight: 700;
        word-spacing: 2px;
    }

    .sxpnl .sxro {
        clear: both;
        margin: 5px 0;
        width: 100%;
        min-height: 19px;
    }

        .sxpnl .sxro .sxlb, .sxpnl .sxro .sxlm {
            float: left;
            overflow: hidden;
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            box-sizing: border-box;
            vertical-align: top;
            text-align: left;
        }

    .sxro .sxroend {
        clear: both;
        margin: 0;
        padding: 0;
        height: 0px !important;
        overflow: hidden;
    }

    .sxpnl .sxro .sxlb {
        margin: 0;
        border-right-width: 5px;
        padding: 3px 5px 4px;
        min-height: 15px;
    }

    .sx3co .sxlb {
        width: 15%;
    }

    .sx3co .sxlm {
        width: 18.33%;
    }

    .sx2co .sxlb {
        width: 20%;
    }

    .sx2co .sxlm {
        width: 30%;
    }

    .sx2co .sxco {
        width: 50%;
    }


    .cptn:after {
        content: ':';
    }

    .btnSave {
        padding-top: 2px;
    }

    .sx3co .m2spn {
        width: 51.67%;
    }

    label {
        font-weight: normal !important;
    }

    input[type='radio'] + label {
        font-size: 12px;
    }

    input[type="checkbox"] + label {
        vertical-align: top !important;
    }

    input[type="checkbox"] {
        margin-top: 2px;
    }
</style>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="container-fluid">
    <div runat="server" id="dvSearchPanel">
        <h2 class="header-color">Save Search Criteria
        </h2>
        <div class="row">
            <div class="col-md-12">
                <div class="msgbox">
                    <asp:Label ID="lblMessage" runat="server">
                    </asp:Label>
                </div>
            </div>
        </div>
        <div class="row ">
            <asp:Panel runat="server" ID="pnlParameters">
                <div class="col-md-12">
                    <div class="row bgLightGreen">
                        <div class='form-group col-md-3'>
                            <asp:Label ID="lblName" runat="server" Text="Name" CssClass="cptn"></asp:Label><span
                                class="reqd">*</span>
                            <infs:WclTextBox runat="server" ID="txtName" MaxLength="100" Width="100%" CssClass="form-control">
                            </infs:WclTextBox>
                            <div class='vldx'>
                                <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                                    class="errmsg" Display="Dynamic" ErrorMessage="Name is required." ValidationGroup='grpSave' />
                            </div>
                        </div>
                        <div class='form-group col-md-3'>
                            <asp:Label ID="lblDescription" runat="server" Text="Description" CssClass="cptn"></asp:Label>
                            <infs:WclTextBox runat="server" ID="txtDescription" MaxLength="500" Width="100%"
                                CssClass="form-control">
                            </infs:WclTextBox>
                        </div>
                        <div class="form-group col-md-3">
                            <span class="">&nbsp;</span>
                            <infsu:CommandBar ID="fsucCmdBarParameter" runat="server" DefaultPanel="pnlParameters"
                                DisplayButtons="Save" AutoPostbackButtons="Save" SaveButtonText="Save"
                                ValidationGroup="grpSave" OnSaveClick="fsucCmdBarParameter_SaveClick" SaveButtonIconClass="rbSave"
                                UseAutoSkinMode="false" ButtonSkin="Silk" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    
         <div class="msgbox" id="pageMsgBox">
                        <asp:Label CssClass="info" runat="server" ID="lblError"></asp:Label>
                    </div>
    <div class="row">&nbsp;</div>
    <div id="divDescription" runat="server" class='description'>
        <asp:Label ID="lblRepDescription" Visible="false" runat="server" Text="Report Definition" CssClass="cptn"></asp:Label>
        <span id="repDescription" runat="server" class="definition"></span>
    </div>

    <div id="dvSpace" runat="server" class="row">&nbsp;</div>

    <div style="margin: 0 auto;">


        <rsweb:ReportViewer ID="ReportViewer1" OnLoad="ReportViewer1_Load" runat="server" Height="100%" Width="100%">
        </rsweb:ReportViewer>

    </div>
</div>
<asp:Button ID="btnDoPostBack" runat="server" CssClass="buttonHidden" OnClick="btnDoPostBack_Click" />
<script type="text/javascript">
    //UAT-721 28-Aug-2014
    //The ID which is used is depends on the version of SSRS. To find the ID for used below description.
    //Open a SSRS Report in Chrome, press F12 to open the Chrome DevTools window.
    //Press Ctrl-F to search for the <div> tag with and id containing the words ReportArea.
    //To confirm you had found it you should be able to see the <div> element with id containing VisibleReportContent above it.
    //Click on its parent <div> with the id that looks like this “ctl09” then look for the overflow style which is currently set to Auto. 
    //function pageLoad() {
    //    var element = $jQuery('[id$="ctl09"]');
    //    if (element) {
    //        element.css('overflow', 'visible');
    //    }
    //}
    function RefreshPage() {
        var btn = $jQuery('[id$="btnDoPostBack"]');
        btn.click();
    }

    function pageLoad() {
        $jQuery(".rbPrimaryIcon.rbSave").removeClass().addClass("fa fa-floppy-o");
    }
</script>

<style>
    .msgbox {
        border-color:none !Important;
        border-width:0px !Important;
    }
</style>