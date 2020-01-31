<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequirementDetails.ascx.cs" Inherits="CoreWeb.ClinicalRotation.UserControl.RequirementDetails" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<%@ Register TagPrefix="uc" Src="~/ClinicalRotation/UserControl/RequirementItemDataPanel.ascx" TagName="ItemControl" %>
<%@ Register TagPrefix="uc" Src="~/ClinicalRotation/UserControl/RequirementApplicantDetailPanel.ascx" TagName="ApplicantPanel" %>
<%@ Register TagPrefix="uc" Src="~/ClinicalRotation/UserControl/RequirementDocumentControl.ascx" TagName="DocumentPanel" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxAdminView">
    <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/kendo.default.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Dashboard/Styles/kendo.common.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/verification.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/ClinicalRotation/ReqVerification-main.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<%--<infs:WclResourceManagerProxy runat="server" ID="manageUploadDocument">
    <infs:LinkedResource Path="~/Resources/Mod/ClinicalRotation/RequirementVerificationDetail.js"
        ResourceType="JavaScript" />
     <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/verification.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/ClinicalRotation/RotationMemberSearch.js"
        ResourceType="JavaScript" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css"
        ResourceType="StyleSheet" />
</infs:WclResourceManagerProxy>--%>
<div class="msgbox" id="pageMsgBox2">
    <asp:Label CssClass="info" EnableViewState="false" runat="server" ID="lblPendingItems"></asp:Label>
</div>
<style type="text/css">
    #dvSharedUserBreadcrumb {
        display: none;
    }
</style>
<infs:WclSplitter ID="sptrAdminView" runat="server" LiveResize="true" Orientation="Horizontal"
    Height="100%" Width="100%" BorderSize="0" BorderWidth="0" ResizeWithParentPane="true">
    <infs:WclPane ID="pnToolbar" runat="server" MinHeight="50" Height="50" Width="100%"
        Scrolling="None" Collapsed="false">
        <div id="modwrapo">
            <div id="modwrapi">
                <div id="breadcmb">
                    <infsu:breadcrumb ID="breadcrum" runat="server" />
                </div>
                <div id="modhdr">
                    <h1>
                        <asp:Label Text="Requirement" runat="server" ID="lblModHdr" />&nbsp;<asp:Label Text="Verification Details"
                            runat="server" ID="lblPageHdr" CssClass="phdr" /></h1>
                </div>
            </div>
            <div id="modcmd_bar">
                <div id="vermod_cmds">
                    <a href="javascript:void" id="btnSendMsg" runat="server" onclick="openPopUp()">Send a Message</a>&nbsp;&nbsp;
                    <a runat="server" id="lnkGoBack" onclick="Page.showProgress('Processing...');">Back to Queues</a>
                </div>
            </div>
        </div>
    </infs:WclPane>

    <infs:WclPane ID="pnLower" runat="server" Scrolling="None" Height="100%" Width="100%">
        <infs:WclSplitter ID="sptrCategoryView" runat="server" LiveResize="true" Orientation="Vertical"
            BorderSize="0" BorderWidth="0" ResizeWithParentPane="true"
            OnClientLoaded="e_mnSptrLoaded" OnClientResized="e_mnSptrResized">
            <infs:WclPane ID="pnLeft" runat="server" Height="100%" Width="270" MinWidth="270"
                Scrolling="None" PersistScrollPosition="true" Collapsed="false" CssClass="pn-container">
                <uc:ApplicantPanel ID="ucApplicantPanel" runat="server"></uc:ApplicantPanel>
            </infs:WclPane>
            <infs:WclSplitBar ID="WclSplitBar1" runat="server" CollapseMode="forward">
            </infs:WclSplitBar>
            <infs:WclPane ID="pnMiddle" runat="server" Scrolling="None" Width="590" MinWidth="590" TabIndex="0"
                PersistScrollPosition="true" CssClass="pn-container pn-datapanel">
                <uc:ItemControl ID="ucItemControl" runat="server"></uc:ItemControl>
            </infs:WclPane>
            <infs:WclSplitBar ID="WclSplitBar2" runat="server" CollapseMode="Backward">
            </infs:WclSplitBar>
            <infs:WclPane ID="pnRight" runat="server" Scrolling="None" Width="100%" CssClass="pn-container"
                PersistScrollPosition="true">
                <uc:DocumentPanel ID="ucDocumentPanel" runat="server"></uc:DocumentPanel>
            </infs:WclPane>
        </infs:WclSplitter>
    </infs:WclPane>
</infs:WclSplitter>

<asp:HiddenField runat="server" ID="hdnFirstCatagoryID" Value="0" />
<asp:HiddenField runat="server" ID="hdnFirstDocumentID" Value="0" />
<asp:HiddenField runat="server" ID="hdnPrevCatagoryID" Value="0" />
<asp:HiddenField runat="server" ID="hdnNextCatagoryID" Value="0" />
<asp:HiddenField runat="server" ID="hdnApplicantReqItemID" Value="0" />
<asp:HiddenField runat="server" ID="hdnPageType" Value="" />

