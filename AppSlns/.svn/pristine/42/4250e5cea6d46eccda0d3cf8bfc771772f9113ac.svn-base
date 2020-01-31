<%@ Page Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceOperations.Views.VerificationMainPage" Codebehind="VerificationMainPage.aspx.cs" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="~/ComplianceOperations/UserControl/VerificationApplicantPanel.ascx" TagName="VerificationApplicantPanel"
    TagPrefix="uc1" %>
<%@ Register Src="~/ComplianceOperations/UserControl/VerificationItemDataPanel.ascx" TagName="VerificationItemDataPanel"
    TagPrefix="uc2" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height:100%">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="sptmManager" runat="server"></telerik:RadScriptManager>
        <infs:WclResourceManager runat="server" ID="rsrMgrpage" EnableBrowserSupport="true" SupportCssPath="~/Resources/Generic/Client"
            SupportScriptPath="~/Resources/Generic/Client" AllowTheming="true"
            ThemesFolder="~/Resources/Themes" ThemeFiles="colors.css"
            EnableWindowCheck="true" SupportedBrowsers="ie7,ie8,ie9" SupportedEngines="trident"
            EnableEngineSupport="true" />
        <infs:WclResourceManagerProxy runat="server" ID="rprxAdminView">
            <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/verification.css" ResourceType="StyleSheet" />
            <infs:LinkedResource ResourceType="StyleSheet" Path="~/Resources/Themes/Default/colors.css" />
            <infs:LinkedResource ResourceType="StyleSheet" Path="~/App_Themes/Default/core.css" />
        </infs:WclResourceManagerProxy>
        <div>
            <infs:WclSplitter ID="sptrMain" runat="server" LiveResize="true" Orientation="Horizontal"
                Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
                <infs:WclPane ID="pnLower" runat="server" Scrolling="None" Width="100%">
                    <infs:WclSplitter ID="sptrCategoryView" runat="server" LiveResize="true" Orientation="Vertical"
                        Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true"
                        OnClientLoaded="e_mnSptrLoaded" OnClientResized="e_mnSptrResized">
                        <infs:WclPane ID="pnLeft" runat="server" Height="100%" Width="270" MinWidth="270"
                            Scrolling="None" PersistScrollPosition="true" Collapsed="false" CssClass="pn-container">
                            <uc1:VerificationApplicantPanel ID="ucVerificationApplicantPanel" runat="server" />
                        </infs:WclPane>
                        <infs:WclSplitBar ID="WclSplitBar1" runat="server" CollapseMode="forward">
                        </infs:WclSplitBar>
                        <infs:WclPane ID="pnMiddle" runat="server" Scrolling="None" Width="590" MinWidth="590" TabIndex="0"
                            PersistScrollPosition="true" CssClass="pn-container pn-datapanel">
                            <uc2:VerificationItemDataPanel ID="ucVerificationItemDataPanel" runat="server" />
                        </infs:WclPane>
                    </infs:WclSplitter>
                </infs:WclPane>
            </infs:WclSplitter>
        </div>
        <infs:WclResourceManagerProxy runat="server" ID="prxMP">
            <infs:LinkedResource Path="~/Resources/Generic/app.min.js" ResourceType="JavaScript" />
            <infs:LinkedResource Path="~/Resources/Mod/Compliance/verification-main.js" ResourceType="JavaScript" />
            <infs:LinkedResource Path="~/Resources/Mod/Shared/AppMaster.js" ResourceType="JavaScript" />
            <infs:LinkedResource Path="~/Resources/Generic/page.min.js" ResourceType="JavaScript" />
        </infs:WclResourceManagerProxy>
    </form>
</bod>
</html>
