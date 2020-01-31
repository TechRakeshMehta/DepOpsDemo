<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="ReconciliationDetail.ascx.cs" Inherits="CoreWeb.ComplianceOperations.Views.ReconciliationDetail" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<%@ Register Src="ReconcilicationApplicantPanel.ascx" TagName="ApplicantPanel"
    TagPrefix="uc1" %>
<%@ Register Src="ReconciliationItemDataPanel.ascx" TagName="ItemDataPanel"
    TagPrefix="uc2" %>
<%@ Register Src="ReconciliationDocumentPanel.ascx" TagName="DocumentPanel"
    TagPrefix="uc3" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>

<script type="text/javascript">
    //21/02/2014 Implemented CTRL+s for Save
    //debugger;
    var uAgent = navigator.userAgent;
    var verOffset = uAgent.indexOf("Firefox");
    if (verOffset != -1) {
        $jQuery(window).keypress(function (event) {
            if (!(event.which == 115 && event.ctrlKey) && !(event.which == 19)) return true;
            event.preventDefault();
        });
    }
    else {
        $jQuery(document).bind('keydown keypress', 'ctrl+s', function () {
            // For IE and Chrome
            var key = window.event.keyCode;
            return true;
        });
    }

    //[UAT-4276]
    $jQuery(document).ready(function () {
        $jQuery(".vdatapn-top.scroll-box").scroll(function () {
            $jQuery.each($jQuery('.RadComboBox').toArray(), function (index, obj) {
                $find(obj.id).hideDropDown();
            });
        });
    });   

</script>

<infs:WclResourceManagerProxy runat="server" ID="WclResourceManagerProxy1">
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/Styles/verification.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/verification-main.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
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
                        <asp:Label Text="Compliance" runat="server" ID="lblModHdr" />&nbsp;<asp:Label Text="Reconciliation Details"
                            runat="server" ID="lblPageHdr" CssClass="phdr" /></h1>
                </div>
            </div>
            <div id="modcmd_bar">
                <div id="vermod_cmds">
                     <a runat="server" id="lnkVerificationDetailView" onclick="Page.showProgress('Processing...');">Go to Verification Details</a>
                    &nbsp;&nbsp;
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
                <uc1:ApplicantPanel ID="ucApplicantPanel" runat="server" />
            </infs:WclPane>
            <infs:WclSplitBar ID="WclSplitBar1" runat="server" CollapseMode="forward">
            </infs:WclSplitBar>
            <infs:WclPane ID="pnMiddle" runat="server" Scrolling="None" Width="590" MinWidth="590" TabIndex="0"
                PersistScrollPosition="true" CssClass="pn-container pn-datapanel">
                <uc2:ItemDataPanel ID="ucItemDataPanel" runat="server" />
            </infs:WclPane>
            <infs:WclSplitBar ID="WclSplitBar2" runat="server" CollapseMode="Backward">
            </infs:WclSplitBar>
            <infs:WclPane ID="pnRight" runat="server" Scrolling="None" Width="100%" CssClass="pn-container"
                PersistScrollPosition="true">
                <uc3:DocumentPanel ID="ucDocumentPanel" runat="server" />
            </infs:WclPane>
        </infs:WclSplitter>
    </infs:WclPane>
</infs:WclSplitter>
<asp:HiddenField ID="hdnClassName" runat="server" />
<asp:HiddenField runat="server" ID="hdnNextItemURL" Value="" />