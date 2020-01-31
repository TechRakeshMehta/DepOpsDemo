<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.VerificationDetails" CodeBehind="VerificationDetails.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="VerificationApplicantPanel.ascx" TagName="VerificationApplicantPanel"
    TagPrefix="uc1" %>
<%@ Register Src="VerificationItemDataPanel.ascx" TagName="VerificationItemDataPanel"
    TagPrefix="uc2" %>
<%@ Register Src="VerificationDocumentPanel.ascx" TagName="VerificationDocumentPanel"
    TagPrefix="uc3" %>
<%@ Register Src="~/CommonControls/UserControl/BreadCrumb.ascx" TagName="breadcrumb"
    TagPrefix="infsu" %>
<script type="text/javascript">
    //21/02/2014 Implemented CTRL+s for Save
    //debugger;
    var uAgent = navigator.userAgent;
    var verOffset = uAgent.indexOf("Firefox");
    var isFullPermissionForVerification = '<%= IsFullPermissionForVerification %>';
    if (verOffset != -1) {
        $jQuery(window).keypress(function (event) {
            // For FireFox  
            //debugger;      
            if (!(event.which == 115 && event.ctrlKey) && !(event.which == 19)) return true;

            //alert("Ctrl-S pressed1");
            //Page.showProgress("Processing...");
            event.preventDefault();
            //if loggedin user have full verification permission then ctrl+ S works for saving data.[//UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen] 
            if (isFullPermissionForVerification.toLowerCase() != "false") {
                $jQuery("[id$=hdnIsSaveClicked]")[0].value = false;
                $jQuery.find("[id$=saveButton]")[0].click();
                return false;
            }
        });
    }
    else {
        $jQuery(document).bind('keydown keypress', 'ctrl+s', function () {
            // For IE and Chrome
            var key = window.event.keyCode;

            if (window.event.ctrlKey && String.fromCharCode(key).toLowerCase() == 's') {
                //alert("Ctrl-S pressed2");    
                // Page.showProgress("Processing...");//
                //debugger;
                //if loggedin user have full verification permission then ctrl+ S works for saving data.[//UAT-509 WB: Ability to limit admin access to read only on the ver details and applicant search details screen] 
                if (isFullPermissionForVerification.toLowerCase() != "false") {
                    $jQuery("[id$=hdnIsSaveClicked]")[0].value = false;
                    $jQuery.find("[id$=saveButton]")[0].click();
                    return false;
                }
            }
            else
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

<infs:WclResourceManagerProxy runat="server" ID="rprxAdminView">
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
                        <asp:Label Text="Compliance" runat="server" ID="lblModHdr" />&nbsp;<asp:Label Text="Verification Details"
                            runat="server" ID="lblPageHdr" CssClass="phdr" /></h1>
                </div>
            </div>
            <div id="modcmd_bar">
                <div id="vermod_cmds">
                    <a href="javascript:void" onclick="openPopUp()">Send a Message</a>&nbsp;&nbsp;
             <%--       <asp:LinkButton Text="Back to Queues" runat="server" ID="lnkGoBack" OnClick="lnkGoBack_click" />--%>
                    <a runat="server" id="lnkGoBack" onclick="Page.showProgress('Processing...');">Back to Queues</a>
                    &nbsp;&nbsp;
                       <a runat="server" id="lnkApplicantView" onclick="Page.showProgress('Processing...');" visible="false">Go to Applicant View</a>
                    &nbsp;&nbsp;
                       <a runat="server" id="LnkGoToSubscrptnDetail" onclick="Page.showProgress('Processing...');" visible="false">Go to Portfolio Detail</a>

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
                <uc1:VerificationApplicantPanel ID="ucVerificationApplicantPanel" runat="server" />
            </infs:WclPane>
            <infs:WclSplitBar ID="WclSplitBar1" runat="server" CollapseMode="forward">
            </infs:WclSplitBar>
            <infs:WclPane ID="pnMiddle" runat="server" Scrolling="None" Width="590" MinWidth="590" TabIndex="0"
                PersistScrollPosition="true" CssClass="pn-container pn-datapanel">
                <uc2:VerificationItemDataPanel ID="ucVerificationItemDataPanel" runat="server" />
            </infs:WclPane>
            <infs:WclSplitBar ID="WclSplitBar2" runat="server" CollapseMode="Backward">
            </infs:WclSplitBar>
            <infs:WclPane ID="pnRight" runat="server" Scrolling="None" Width="100%" CssClass="pn-container"
                PersistScrollPosition="true">
                <uc3:VerificationDocumentPanel ID="ucVerificationDocumentPanel" runat="server" />
            </infs:WclPane>
        </infs:WclSplitter>
    </infs:WclPane>
</infs:WclSplitter>
<asp:HiddenField ID="hdnClassName" runat="server" />
