<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ChildPage.master" AutoEventWireup="true" CodeBehind="ComplianceSearchNotesPopUp.aspx.cs" Inherits="CoreWeb.ComplianceOperations.Pages.ComplianceSearchNotesPopUp" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Resources/Mod/Dashboard/Styles/bootstrap.min.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Resources/Mod/Dashboard/Styles/font-awesome.min.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css") %>" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        textarea#ctl00_MainContent_txtNotes {
            height: 150px !Important;
            line-height: 20px !Important;
        }

        .riSingle {
            display: inline-block;
            white-space: normal;
            text-align: left;
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
    </style>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <p class="header-color">
                    <span class="cptn">Compliance Search Note</span>
                </p>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <label class="cptn" for="<%= txtNotes.ClientID %>" id="lblNotes">Enter Notes<span class="reqd">*</span> </label>
                <infs:WclTextBox TextMode="MultiLine" ID="txtNotes" MaxLength="1000" runat="server" Height="300px" Width="100%"  CssClass="form-control borderTextArea" EnableAriaSupport="true"></infs:WclTextBox>
                <div class="vldx">
                    <asp:RequiredFieldValidator runat="server" ID="rfvNotes" ControlToValidate="txtNotes" role="alert"
                        Display="Dynamic" ValidationGroup="saveNotes" CssClass="errmsg" SetFocusOnError="true"
                        Text="Note is required." />
                </div>
            </div>
        </div>
        ` 
        <div class="row">
            <div class="col-md-12" style="height: 15px">
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <infsu:CommandBar ID="fsucFeatureActionList" runat="server" AutoPostbackButtons="Save" OnSaveClick="fsucFeatureClientStatus_SaveClick" DisplayButtons="Save,Cancel" CancelButtonText="Close"
                    ButtonPosition="Right" CauseValidationOnCancel="false" OnCancelClientClick="ClosePopup" ValidationGroup="saveNotes" ButtonSkin="Silk" UseAutoSkinMode="false" />
            </div>
        </div>

        <div class="row">
            <div class="msgbox">
                <asp:Label ID="Label1" runat="server" CssClass="info">
                </asp:Label>
            </div>
        </div>

    </div>
    <script src="../../Resources/Mod/Shared/ApplyNewIcons.js" type="text/javascript"></script>
    <script type="text/javascript">
        var tabKey = 9;
        // To close the popup.
        function ClosePopup() {
            top.$window.get_radManager().getActiveWindow().close();
        }

        function pageLoad() {

            $jQuery("[id$=txtNotes]").focus();

           

            //For accessibility, we need to prevent focus to go outside after tabbing on last link
            $jQuery("a,button,:input:not([type=hidden]),[tabindex='0']").last().keydown(function (e) {
                if (e.keyCode == tabKey && !e.shiftKey) {
                    e.preventDefault();
                    $jQuery("#lblNotes").focus();
                }
            });

            if ($jQuery('#MsgBox').css('display') != 'none') {
                $jQuery("#lblError").attr("tabindex", 0).focus();
                $jQuery('#lblError').on("keydown", function (e) {
                    if (e.shiftKey && e.keyCode == tabKey) {
                        e.preventDefault();
                        $jQuery("[id$=<%= fsucFeatureActionList.CancelButton.ClientID %>]").focus();
                    }
                });
            }
            //For accessibility, we need to prevent focus to go outside after shift tab on firstmost element
            $jQuery(document).on("keydown", "[id$=txtNotes]", function (e) {
                if (e.shiftKey && e.keyCode == tabKey) {
                    e.preventDefault();
                    $jQuery("[id$=<%= fsucFeatureActionList.CancelButton.ClientID %>]").focus();
                }
            });
        }

    </script>

</asp:Content>

