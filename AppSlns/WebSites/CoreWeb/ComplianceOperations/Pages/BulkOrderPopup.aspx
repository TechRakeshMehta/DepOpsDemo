<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BulkOrderPopup.aspx.cs"
    Inherits="CoreWeb.ComplianceOperations.Views.BulkOrderPopup" MasterPageFile="~/Shared/PopupMaster.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
    <script type="text/javascript">
        function SetPopupHeight() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            if (oWindow != undefined && oWindow != null) {
                oWindow.SetHeight(200);
            }
        }

        //Function to redirect to parent 
        function CloseBulkOrderPopup() {
            var url = $jQuery("[id$=hdnNavigationUrl]").val();
            var oArg = {};
            oArg.Action = "Submit";
            oArg.Url = url;
            var oWnd = GetRadWindow();
            oWnd.Close(oArg);
        }

        //function to get current popup window
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">

    <infs:WclResourceManagerProxy runat="server" ID="rmpManagePopup">
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"
            ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>


    <div class="section" style="padding-bottom: 5px; margin-bottom: 5px;">
        <asp:Panel ID="pnlInfoMessage" CssClass="sxpnl" runat="server">
            <div>
                <div style="font-weight: bold;" id="dvMessage" runat="server">
                    <asp:Label ID="lblMessage" runat="server" CssClass="info"></asp:Label>
                </div>
            </div>
            <div class="content">
                <div class="sxform auto">
                    <div class='sxro sxco'>
                        <div class='sxlm m3spn'>
                            <p>There is a pending order that needs to be completed.</p>
                        </div>
                        <div class='sxroend'>
                        </div>
                    </div>

                    <asp:Panel ID="pnlBulkOrderSummary" CssClass="sxpnl" runat="server">
                        <infsu:CommandBar ID="fsucBulkOrder" runat="server" ShowAsLinkButtons="false" DisplayButtons="Submit,Cancel"
                            AutoPostbackButtons="Submit,Cancel" SubmitButtonText="Complete Order" CancelButtonText="Dismiss for Now"
                            ButtonPosition="Right" DefaultPanel="pnlRegForm" OnSubmitClick="fsucBulkOrder_SubmitClick"
                            OnCancelClick="fsucBulkOrder_CancelClick" />
                    </asp:Panel>
                </div>
            </div>
        </asp:Panel>
    </div>
    <asp:HiddenField runat="server" ID="hdnNavigationUrl" />
</asp:Content>


