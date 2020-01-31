<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewAgencyJobPost.ascx.cs" Inherits="CoreWeb.AgencyJobBoard.UserControls.Views.ViewAgencyJobPost" %>


<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxAdminView">
    <infs:LinkedResource ResourceType="StyleSheet" Path="~/Resources/Themes/Default/colors.css" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    .customHeight {
        /*max-height: 82px !important;
        max-width: 200px !important;*/
        /*width: 175px !important;
        height: 73px !important;*/
    }

    .largeFont {
        font-size: 18px;
    }

    .extraMargin {
        margin-bottom: 10px !important;
    }

    .rbSave {
        background-image: url("../../Resources/Mod/AgencyJobBoard/Images/JobBoard.png") !important;
        background-position: 0px !important;
        width: 16px !important;
    }

    li {
        list-style: inherit !important;
        margin-left: 15px !important;
    }
</style>

<asp:Panel ID="pnlMain" runat="server" Width="100%" Height="100%">
    <div class="section">
        <div class="sxform auto" style="background-color: white;">
            <asp:Panel ID="Panel1" runat="server" CssClass="sxpnl">
                <div class='sxro'>
                    <div class="bx_image customHeight">
                        <asp:Image runat="server" ID="imgCntrl" class="thumb" />
                    </div>
                </div>
                <div>
                    <%--style="background-color: #f1f1f1"--%>
                    <div class="sxro extraMargin" id="dvJobTitle" runat="server" visible="false" style="text-decoration-color: gray; margin-bottom: 0px !important;">
                        <asp:Label runat="server" ID="lblJobTitle" Font-Size="XX-Large" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="sxro extraMargin" id="dvCompany" runat="server" visible="false" style="margin-top: -5px;">
                        <asp:Label runat="server" ID="lblCompany" Font-Size="X-Large"></asp:Label>
                        <asp:Label ID="lblHyphen" runat="server" Text="-" Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lblLocation" Font-Size="X-Large"></asp:Label>
                    </div>
                    <div class="sxro extraMargin" id="dvInstruction" runat="server" visible="false" style="">
                        <span id="spnInstruction" class="largeFont" runat="server"></span>
                    </div>
                    <div class="sxro extraMargin" id="dvDescription" runat="server" visible="false" style="">
                        <span runat="server" class="largeFont" id="spnDescription"></span>
                    </div>
                </div>
                <infsu:CommandBar ID="fsucCmdBarAgencyDetails" DisplayButtons="Save,Submit" runat="server" GridMode="false" ButtonPosition="Left" SubmitButtonIconClass="rbPrevious"
                    SaveButtonIconClass="rbSave" SaveButtonText="Apply for Job" OnSaveClientClick="HowtoApplyPopUp"
                    UseAutoSkinMode="true" AutoPostbackButtons="Submit" OnSubmitClick="fsucCmdBarAgencyDetails_SubmitClick" SubmitButtonText="Back to Listing" />
                <asp:HiddenField runat="server" ID="hdnHowToApply" Value='<%# Eval("HowToApply") %>' />
            </asp:Panel>
        </div>
    </div>

    <div id="dialog-message" style="display: none;" title="How To Apply">
        <div class="msgbox" style="display: block; opacity: 1; max-height: 400px !important; overflow: auto;">
            <span runat="server" id="spnHowToApply"></span>
        </div>
    </div>
</asp:Panel>

<script type="text/javascript">
    function HowtoApplyPopUp() {
        dialog = $window.showDialog($jQuery("#dialog-message").clone().show(), { closeBtn: { autoclose: true, text: "Ok" }, }, 500, "How To Apply");
    }
</script>
