<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RenewOrderPopup.aspx.cs"
    Inherits="CoreWeb.ComplianceOperations.Views.RenewOrderPopup" MasterPageFile="~/Shared/PopupMaster.master" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="uc" TagName="PackageSubscription" Src="~/ComplianceOperations/PackageSubscription.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MessageContent" runat="server">
    <script type="text/javascript">

        //Function to redirect to parent 
        function CloseRenewOrderPopup(url) {
            var oArg = {};
            oArg.Action = "Submit";
            oArg.Url = url;
            //oWnd.remove_close(CloseRenewOrderPopup);
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

        $jQuery(document).ready(function () {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            if (oWindow != undefined && oWindow != null) { 
                oWindow.SetTitle("Renew Existing Tracking Subscription");
            }
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PoupContent" runat="server">

    <infs:WclResourceManagerProxy runat="server" ID="rmpManagePopup">
        <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"
            ResourceType="JavaScript" />
    </infs:WclResourceManagerProxy>

    <uc:PackageSubscription ID="ucPackageSubscription" runat="server"></uc:PackageSubscription>

</asp:Content>


