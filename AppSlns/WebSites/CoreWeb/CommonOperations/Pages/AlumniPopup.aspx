<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlumniPopup.aspx.cs" Inherits="CoreWeb.CommonOperations.Pages.AlumniPopup" MasterPageFile="~/Shared/PopupMaster.master" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">
    <infs:WclResourceManagerProxy runat="server" ID="rprxAlumniPopup">
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
        <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />

    </infs:WclResourceManagerProxy>
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <h2 class="header-color">
                    <asp:Label ID="lblHeader" runat="server" Text="Complio Alumni Access"></asp:Label>
                </h2>
            </div>
        </div>
        <div class="row bgLightGreen">
            <asp:Panel ID="pnlAlumni" runat="server">
                <div class='col-md-12'>
                    <div class='form-group col-md-3'>
                        <div class='setfontbold setfontitalic'>
                            <label id="lblText" class="form-control" runat="server">Complio would like to preserve your Immunization data for future use. Do you want to activate Complio alumni access?</label>
                            <%--<asp:Literal ID="litHTML" runat="server"></asp:Literal>--%>
                        </div>
                    </div>
                </div>

            </asp:Panel>
            <div class="col-md-12">
                <div class="row text-center">
                    <infsu:CommandBar ID="cmdAlumni" runat="server" DisplayButtons="Save,Cancel,Extra" AutoPostbackButtons="Save,Cancel,Extra" ValidationGroup="grpverification"
                        ButtonPosition="Center" SaveButtonText="Activate" CancelButtonText="Dismiss" ExtraButtonText="Dismiss Forever" UseAutoSkinMode="false" ButtonSkin="Silk"
                        OnSaveClick="fsucCommandBar_ActivateClick" OnCancelClick="fsucCommandBar_DismissClick" OnExtraClick="fsucCommandBar_DismissForeverClick">
                    </infsu:CommandBar>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function closePopUp() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;

            var oArg = {};

            oWindow.Close(oArg);
        } 
    </script>
</asp:Content>


