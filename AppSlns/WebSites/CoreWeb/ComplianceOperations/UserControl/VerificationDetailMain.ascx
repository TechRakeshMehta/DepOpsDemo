<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ComplianceOperations.Views.VerificationDetailMain" Codebehind="VerificationDetailMain.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="VerificationDocumentPanel.ascx" TagName="VerificationDocumentPanel"
    TagPrefix="uc3" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxAdminView">
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/verification.css" ResourceType="StyleSheet" />
    <infs:LinkedResource ResourceType="StyleSheet" Path="~/Resources/Mod/Shared/public_pages/core.css" />
</infs:WclResourceManagerProxy>
<infs:WclResourceManagerProxy runat="server" ID="prxMP">
    <infs:LinkedResource Path="~/Resources/Generic/app.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Shared/AppMaster.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Generic/page.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/verification-main.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<infs:WclSplitter ID="sptrAdminView" runat="server" LiveResize="true" Orientation="Horizontal" Height="100%"
    Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
    <infs:WclPane ID="pnToolbar" runat="server" MinHeight="0" Height="1" Width="100%"
        Scrolling="None" Collapsed="false">
        <div id="modcmd_bar">
            <div id="vermod_cmds">
                <a href="javascript:void" onclick="openPopUp()">Send a Message</a>&nbsp;&nbsp;
                    <a runat="server" id="lnkGoBack" onclick="Page.showProgress('Processing...');">Back to Queues</a>
                &nbsp;&nbsp;
                       <a runat="server" id="lnkApplicantView" onclick="Page.showProgress('Processing...');" visible="false">Go to Applicant View</a>

            </div>
        </div>
    </infs:WclPane>

    <infs:WclPane ID="pnLower" runat="server" Scrolling="None" Width="100%">
        <infs:WclSplitter ID="sptrCategoryView" runat="server" LiveResize="true" Orientation="Vertical"
            Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true"
            OnClientLoaded="e_mnSptrLoaded" OnClientResized="e_mnSptrResized">
            <infs:WclPane ID="pnLeftMain" runat="server" Width="870" MinWidth="270"
                Scrolling="None" PersistScrollPosition="true" Collapsed="false">
                <iframe id="ifrVerificationDetail" runat="server" style="height: 100%; width: 100%"></iframe>
            </infs:WclPane>
            <infs:WclSplitBar ID="WclSplitBar2" runat="server" CollapseMode="forward">
            </infs:WclSplitBar>
            <infs:WclPane ID="pnRight" runat="server" Scrolling="None" Width="100%" CssClass="pn-container"
                PersistScrollPosition="true">
                <uc3:VerificationDocumentPanel ID="ucVerificationDocumentPanel" runat="server" />
            </infs:WclPane>
        </infs:WclSplitter>
    </infs:WclPane>

</infs:WclSplitter>


