<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="SharedUserViewVideoPopup.aspx.cs" Inherits="CoreWeb.ApplicantRotationRequirement.Pages.SharedUserViewVideoPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="Resources/Mod/Dashboard/Scripts/Kendo/jquery.min.js"></script>
</head>
<body onunload="OnViewVideoPopupWndClosed();">

    <style type="text/css">
        #frmVideos {
            height: 95%;
            width: 100%;
        }

        .dvClose {
            float: right;
            width: 100%;
            text-align: center;
        }
    </style>
    <form id="frmVideos" runat="server">
        <link href="<%= ResolveUrl("~/Resources/Mod/Dashboard/Styles/bootstrap.min.css") %>" rel="stylesheet" type="text/css" />
        <link href="<%= ResolveUrl("https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700") %>" rel="stylesheet" type="text/css" />
        <link href="<%= ResolveUrl("~/Resources/Mod/Dashboard/Styles/font-awesome.min.css") %>" rel="stylesheet" type="text/css" /> 
        <link href="<%= ResolveUrl("~/Resources/Mod/Dashboard/Styles/SharedUserDashboard.css") %>" rel="stylesheet" type="text/css" />
        <script src="../../Resources/Mod/Shared/ApplyNewIcons.js"></script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Path="~/Resources/Mod/ApplicantRotationRequirement/ViewVideo.js" />
            </Scripts>
        </asp:ScriptManager>

        <div id="dvVideoFrame" style="width: 100%; height: 92%;">
            <iframe id="iframeViewVideo" runat="server" width="100%" height="100%" frameborder="0" webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
              <div style="font-size: 13px; margin-left: 5px;color:red;">
                <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
            </div>
        </div>
        <div class="dvClose" id ="dvClose" runat="server"  style="margin-top: 3px;">
            <br />
            <infs:WclButton runat="server" ID="btnCloseViewVideo" AutoPostBack="false" OnClientClicked="OnViewVideoPopupWndClosed" Text="Close"
                Skin="Silk"  AutoSkinMode="false"></infs:WclButton>
        </div>
        <asp:HiddenField ID="hdnVideoUrl" runat="server" />
        <asp:HiddenField ID="hdnBoxStayOpenTime" runat="server" />
        <asp:HiddenField ID="hdnIsReqToOpen" runat="server" />
        <asp:HiddenField ID="hdnIsAdminRequested" runat="server" />
         <asp:HiddenField ID="hdnVideoRequiredOpenTime" runat="server" />
        <asp:HiddenField ID="hdnIsEditMode" runat="server" Value="false" />
        <asp:HiddenField ID="hdnVideoCurrentPlaybackTimed" runat="server" />
    </form>
</body>
</html>
