﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequirementVerificationDocViewer.aspx.cs" Inherits="CoreWeb.ClinicalRotation.Pages.RequirementVerificationDocViewer" %>


<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Assembly="RadPdf" Namespace="RadPdf.Web.UI" TagPrefix="radPdf" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <style type="text/css">
        .cat_highlight
        {
            color: red !important;
        }

        body, html, form, #box_content, #dvPdfDocuViewer
        {
            width: 100%;
            height: 100%;
            padding: 0;
            margin: 0;
            overflow: hidden;
        }

        .no_disp
        {
            display: none;
        }

        .toolbox
        {
            background: none repeat scroll 0 0 #FFFFFF;
            border: 1px solid #000000;
            border-radius: 4px;
            opacity: 1;
            padding: 5px;
            position: absolute;
            right: 2px;
            top: 0;
            box-shadow: 2px 2px 10px 0px;
        }

        .info
        {
            background-color: #fffef0;
            background-image: url("/Resources/Themes/Default/images/info.png");
            background-position: 10px 8px;
            background-repeat: no-repeat;
            color: #3071cd !important;
        }

        .shw_dv
        {
            display: block !important;
        }
    </style>
    <form id="form1" runat="server">
        <link href="<%= ResolveUrl("~/App_Themes/Default/core.css") %>" rel="stylesheet" type="text/css" />
        <link href="<%= ResolveUrl("~/Resources/Themes/Default/colors.css") %>" rel="stylesheet" type="text/css" />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Path="~/Resources/Mod/ClinicalRotation/RequirementDocumentViewer.js" />
            </Scripts>
        </asp:ScriptManager>
        <div class="toolbox">
            <asp:HiddenField runat="server" ID="hdnApplicantIdCurrent" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnTenantIdCurrent" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnDocIDDocViewer" runat="server" ClientIDMode="Static" />
            <infs:WclButton runat="server" AutoPostBack="false" ID="btnDock" OnClientClicked="dock_wndvwr" Text="Dock"></infs:WclButton>
            <infs:WclButton runat="server" AutoPostBack="true" ID="btnReloadPdfVwr" OnClick="btnReloadPdfVwr_Click" OnClientClicked="reload_page" Text="Reload"></infs:WclButton>
            <infs:WclButton runat="server" AutoPostBack="false" ID="btnRotatePdfPage" OnClientClicked="rotate_page" Text="Rotate"></infs:WclButton>
            <%--<infs:WclButton runat="server" AutoPostBack="true" ID="btnReloadPdfVwr" OnClick="btnReloadPdfVwr_Click" OnClientClicked="reload_page" Text="Reload"></infs:WclButton>
            <infs:WclButton runat="server" AutoPostBack="false" ID="btnRotatePdfPage" OnClientClicked="rotate_page"OnClick="btnReloadPdfVwr_Click" Text="Rotate"></infs:WclButton>--%>
        </div>
        <div id="dvMsgBox" runat="server">
            <asp:Label ID="lblPdfMessage" Visible="false" runat="server" Style="display: block; padding: 20px; padding: 15px 10px 20px 53px; border-width: 1px; margin: 10px;">                             
            </asp:Label>
        </div>
        <div id="dvPdfDocuViewer" runat="server">
            <asp:HiddenField runat="server" ID="hdnUnifiedDoc_value" />

            <radPdf:PdfWebControl ID="PdfWebControl1" runat="server" Height="100%"
                Width="100%" OnClientLoad="e_wcload();"
                HideBottomBar="false"
                HideThumbnails="true"
                HideBookmarks="true"
                CollapseTools="true"
                HideSearchText="True"
                HideEditMenu="true"
                HideObjectPropertiesBar="true"
                HideToolsPageTab="true"
                HideToolsAnnotateTab="true"
                HideToolsInsertTab="true"
                HideSideBar="true"
                HideToolsMenu="true"
                ViewerPageLayoutDefault="SinglePageContinuous" />
        </div>
    </form>
</body>
</html>