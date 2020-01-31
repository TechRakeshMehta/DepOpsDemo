<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceAdministration.Views.ShotSeriesShuffleTest"
    Title="ShotSeriesShuffleTest" MasterPageFile="~/Shared/NewChildPage.master" CodeBehind="ShotSeriesShuffleTest.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="IsActiveToggle" Src="~/Shared/Controls/IsActiveToggle.ascx" %>
<%@ Register Src="~/ComplianceAdministration/UserControl/ShotSeriesShuffleRuleTest.ascx" TagPrefix="infsu" TagName="ShotSeriesShuffleRuleTest" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .sxcbar .RadButton .rbPrimary {
            padding-left: 16px;
        }

        .BorderStyleThick {
            border: 1.5px solid gray !important;
            border-radius: 5px !important;
            padding: 8px !important;
            margin-bottom: 8px !important;
        }

        .headerStyle {
            font-size: medium;
            font-weight: bold;
        }

        .tableHeader {
            border-bottom: solid 1px;
            border-right: solid 1px;
            border-top: solid 1px;
            border-image: none;
            height: 35px;
            text-align: center;
            background-color: rgb(198, 197, 197);
            font-size: medium;
            font-weight: bold;
        }

            .tableHeader:first-child {
                border-bottom: solid 1px;
                border-right: solid 1px;
                border-left: solid 1px;
                border-top: solid 1px;
                border-image: none;
                height: 35px;
                text-align: center;
                background-color: rgb(198, 197, 197);
                font-size: medium;
                font-weight: bold;
            }

        .tableCell {
            border-bottom: solid 1px;
            border-right: solid 1px;
            border-image: none;
            height: 35px;
            text-align: center;
            /*width:100%;*/
        }
    </style>
    <infs:WclResourceManagerProxy runat="server" ID="rsmpCpages">
        <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/AdminDataEntry.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/verification.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>
    <script type="text/javascript">
     

        function handleRadComboUI() {
            $jQuery('div[id$=paneTop]').on("scroll", function () {
                $jQuery.each($jQuery('.RadComboBox').toArray(), function (index, obj) {
                    $find(obj.id).hideDropDown();
                });
            });
        }

        function handleRadPickerUI() {
            $jQuery('div[id$=paneTop]').on("scroll", function () {
                $jQuery.each($jQuery('.RadCalendarPopup').toArray(), function (index, obj) {
                    obj.style.display = "none";
                });
            });
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        //$page.showAlertMessage = function (msg, msgtype, overriderErrorPanel) {
        //    /// <summary>Shows message box on the page</summary>
        //    /// <param name="msg" type="String">Message to be displayed</param>
        //    /// <param name="msgtype" type="$page.msgTypes">Type of message box</param>
        //    var win = GetRadWindow();
        //    if (win) {
        //        win.restore();
        //    }
        //    if (typeof (msg) == "undefined") return;
        //    var c = typeof (msgtype) != "undefined" ? msgtype : "";
        //    if ($jQuery(".no_error_panel").length > 0 || overriderErrorPanel) {
        //        $jQuery("#MsgBox").children("span").text(msg).attr("class", msgtype);
        //        if (c == 'sucs') {
        //            c = "Success";
        //        }
        //        else (c = c.toUpperCase());

        //        $jQuery("#pnlError").hide();

        //        $window.showDialog($jQuery("#MsgBox").clone().show(), { closeBtn: { autoclose: true, text: "Ok" } }, 400, c);
        //    }
        //    else {
        //        $jQuery("#MsgBox").fadeIn().children("span").text(msg).attr("class", msgtype);
        //    }

        //}

        $page.showAlertMessage = function (msg, msgtype, headerText) {
            var win = GetRadWindow();
            if (win) {
                win.restore();
            }
            if (typeof (msg) == "undefined") return;
            var c = typeof (msgtype) != "undefined" ? msgtype : "";
            $jQuery("#MsgBox").children("span")[0].innerHTML = msg;
            $jQuery("#MsgBox").children("span").attr("class", msgtype);
            c = headerText;

            $jQuery("[id$=pnlError]").hide();

            $window.showDialog($jQuery("#MsgBox").clone().show(), { closeBtn: { autoclose: true, text: "Ok" } }, 500, c);
        }

    </script>
    <script src="~/Resources/Mod/Dashboard/Scripts/bootstrap.min.js" type="text/javascript"></script>

    <div class="container-fluid">
        <div class="col-md-12">
            <div class="row">

                <p class="header-color">
                    <asp:Label ID="lblHeader" Text="Shot Series Shuffle Test" runat="server"></asp:Label>
                </p>
            </div>

            <div class="BorderStyleThick">
                <div class="container-fluid">
                    <div class="row">
                        <p class="header-color">
                            <asp:Label ID="lblSeries" Text="Series Item(s)" runat="server"></asp:Label>
                        </p>
                        <div class="col-md-12">
                            <asp:Repeater ID="rptShotSeriesData" runat="server" OnItemDataBound="rptShotSeriesData_ItemDataBound">
                                <HeaderTemplate>
                                    <table width="100%" cellpadding="0" cellspacing="0" style="height: 100%; border: none;">
                                        <tr>
                                            <asp:Repeater runat="server" ID="rptHeader">
                                                <ItemTemplate>
                                                    <th class="tableHeader">
                                                        <asp:Label ID="lbAttributeName" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CmpAttributeName")))%>' runat="server" CssClass="form-control">
                                                        </asp:Label>
                                                    </th>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td class="tableCell" style="border-left: solid 1px;">
                                            <asp:Label ID="lblItemSeriesItemID" Text='<%#Eval("CmpItemSeriesItemId")%>' runat="server" CssClass="form-control" Visible="false">
                                            </asp:Label>
                                            <asp:Label ID="lbItemName" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CmpItemName")))%>' runat="server" CssClass="form-control">
                                            </asp:Label>
                                        </td>
                                        <td class="tableCell">
                                            <infs:WclComboBox ID="ddlItemStatus" runat="server" AutoPostBack="false" DataTextField="Name" Skin="Silk" AutoSkinMode="false"
                                                DataValueField="ItemComplianceStatusID" CheckBoxes="false" Filter="None" OnClientKeyPressing="openCmbBoxOnTab" CssClass="form-control">
                                            </infs:WclComboBox>
                                        </td>
                                        <asp:Repeater runat="server" ID="rptColumn" OnItemDataBound="rptColumn_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemSeriesAttributeID" Text='<%#Eval("ItemSeriesAttributeID")%>' runat="server" CssClass="form-control" Visible="false">
                                                </asp:Label>
                                                <td class="tableCell">
                                                    <infs:WclDatePicker ID="dtPicker" runat="server" Visible="false" CssClass="form-control"
                                                        DateInput-EmptyMessage="Select a date">
                                                    </infs:WclDatePicker>
                                                    <infs:WclTextBox ID="txtBox" runat="server" Visible="false" CssClass="form-control">
                                                    </infs:WclTextBox>
                                                    <infs:WclNumericTextBox ID="numericTextBox" runat="server" Visible="false" CssClass="form-control">
                                                    </infs:WclNumericTextBox>
                                                    <div id="optionComboDiv">
                                                        <infs:WclComboBox ID="optionCombo" runat="server" Visible="false"
                                                            CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                                        </infs:WclComboBox>
                                                    </div>
                                                </td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>

            </div>

            <div class="row">
            </div>
            <div class="BorderStyleThick">
                <div class="row">
                    <div class="container-fluid">
                        <p class="header-color">
                            <asp:Label ID="Label1" Text="Compliance Package(s)" runat="server"></asp:Label>
                        </p>
                        <div class="col-md-12">
                            <div class="row bgLightGreen">
                                <div class='form-group col-md-3 col-sm-3'>
                                    <infs:WclComboBox ID="cmbPackage" runat="server"
                                        DataValueField="CompliancePackageID" DataTextField="PackageName" Filter="None" OnSelectedIndexChanged="cmbPackage_SelectedIndexChanged"
                                        AutoPostBack="true" Width="100%" CssClass="form-control" Skin="Silk" AutoSkinMode="false">
                                    </infs:WclComboBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
            </div>

            <infsu:ShotSeriesShuffleRuleTest runat="server" ID="ShotSeriesShuffleRuleTest" />
            <div class="row">
                <div class="container-fluid">
                    <div class="col-md-12" style="position: relative; bottom: 2px;">
                        <infsu:CommandBar ButtonPosition="Center" ID="cmdbarShuffle" runat="server" DisplayButtons="Submit" ValidationGroup="grpFrmSubmit" AutoPostbackButtons="Submit"
                            SubmitButtonIconClass="rbOk" SubmitButtonText="Test Shuffling" ButtonSkin="Silk" UseAutoSkinMode="false" OnSubmitClick="cmdbarShuffle_ExtraClick">
                        </infsu:CommandBar>
                    </div>
                </div>
            </div>

            <div class="BorderStyleThick" id="divShuffledData" runat="server" visible="false">
                <div class="container-fluid">
                    <div class="row">
                        <p class="header-color">
                            <asp:Label ID="lblShuffledData" Text="Shuffled Item(s)" runat="server"></asp:Label>
                        </p>
                        <div class="col-md-12">
                            <asp:Repeater ID="rptShuffledData" runat="server" OnItemDataBound="rptShuffledData_ItemDataBound">
                                <HeaderTemplate>
                                    <table width="100%" cellpadding="0" cellspacing="0" style="height: 100%; border: none;">
                                        <tr>
                                            <asp:Repeater runat="server" ID="rptShuffledHeader">
                                                <ItemTemplate>
                                                    <th class="tableHeader">
                                                        <asp:Label ID="lbAttributeName" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CmpAttributeName")))%>' runat="server" CssClass="form-control">
                                                        </asp:Label>
                                                    </th>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td class="tableCell" style="border-left: solid 1px;">
                                            <asp:Label ID="lblItemSeriesItemID" Text='<%#Eval("CmpItemSeriesItemId")%>' runat="server" CssClass="form-control" Visible="false">
                                            </asp:Label>
                                            <asp:Label ID="lbItemName" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CmpItemName")))%>' runat="server" CssClass="form-control">
                                            </asp:Label>
                                        </td>
                                        <td class="tableCell">
                                            <asp:Label ID="lblItemStatus" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("NewStatusName")))%>' runat="server" CssClass="form-control">
                                            </asp:Label>
                                        </td>
                                        <asp:Repeater runat="server" ID="rptShuffledColumn">
                                            <ItemTemplate>
                                                <td class="tableCell">
                                                    <asp:Label ID="lbAttributeValue" Text='<%#INTSOF.Utils.Extensions.HtmlEncode(Convert.ToString(Eval("CmpAttributeValue")))%>' runat="server" CssClass="form-control">
                                                    </asp:Label>
                                                </td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>

                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
