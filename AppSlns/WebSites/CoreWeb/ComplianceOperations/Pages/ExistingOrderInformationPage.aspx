<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PopupMaster.master" AutoEventWireup="true"
    CodeBehind="ExistingOrderInformationPage.aspx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ExistingOrderInformationPage" %>


<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
    <style type="text/css">
        .setfontbold strong, .setfontbold b {
            font-weight: bold !important;
        }

        .setfontitalic em, .setfontitalic i {
            font-style: italic !important;
        }

        .sxpnl {
            padding-right: 5px;
        }

        h2.page_header {
            border-bottom: 1px solid;
            color: #8C1921;
            font-family: Arial;
            font-size: 20px;
        }

        .page_ins {
            padding: 10px;
            font-size: 13px;
        }
    </style>
    <script type="text/javascript">

        $page.add_pageLoad(function () {
            $jQuery(".rbPrimaryIcon.existingOrder").removeClass();
            $jQuery(".rbPrimaryIcon.cancelexistingOrder").removeClass();
        });
        function closeExisitngOrderInformationClosePopUpEvent(isIgnoreExistingOrderInformation) {
            var hdnExisitngOrderInformationIgnore = $jQuery("[id$=hdnExisitngOrderInformationIgnore]");
            
            if (hdnExisitngOrderInformationIgnore.length > 0) {
                if (isIgnoreExistingOrderInformation || isIgnoreExistingOrderInformation == "1")
                    hdnExisitngOrderInformationIgnore.val(1);
                else
                    hdnExisitngOrderInformationIgnore.val(0);
            }
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;

            var oArg = {};
            oArg.hdnExisitngOrderInformationIgnore = hdnExisitngOrderInformationIgnore;
            oWindow.Close(oArg);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" style="height: 900px; width: 900px" ContentPlaceHolderID="PoupContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxExistingOrderInformationPopup">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

    </infs:WclResourceManagerProxy>
    <div class="container-fluid">
        <div class="row bgLightGreen">
            <div class="col-md-12">
                <h2 class="header-color">
                    <asp:Label ID="lblHeader" runat="server" Text=""></asp:Label>
                </h2>
            </div>
        </div>

        <div class="row bgLightGreen">
            <div class="col-md-12">
                <div class="">
                    <div class="">
                        <div class="sxform auto">
                            <p class="page_ins">
                                <asp:Label runat="server" ID="lblOrderInfo" Text=""></asp:Label>
                            </p>
                        </div>
                        <br />
                    </div>
                </div>
            </div>
            <div class="col-md-12" style="margin-top: 1%">
                <div class="row text-center">
                    <infsu:CommandBar ID="cbExistingOrderInformation" runat="server" DisplayButtons="Save,Cancel" AutoPostbackButtons="Save,Cancel"
                        OnCancelClick="cbExistingOrderInformation_CancelClick" OnSaveClick="cbExistingOrderInformation_SaveClick" SaveButtonIconClass="existingOrder" CancelButtonIconClass="cancelexistingOrder"
                        ButtonPosition="Center" SaveButtonText="Ignore" CancelButtonText="Ok" UseAutoSkinMode="false" ButtonSkin="Silk">
                    </infsu:CommandBar>
                </div>
            </div>
        </div>

    </div>
    <asp:HiddenField runat="server" ID="hdnParentDiv" />
    <asp:HiddenField runat="server" ID="hdnExisitngOrderInformationIgnore" Value="0" />
</asp:Content>
