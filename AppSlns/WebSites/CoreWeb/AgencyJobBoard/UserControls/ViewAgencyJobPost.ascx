<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewAgencyJobPost.ascx.cs" Inherits="CoreWeb.AgencyJobBoard.UserControls.Views.ViewAgencyJobPost" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>


<style type="text/css">
    .collapsed {
        height: 40px !important;
    }

    .customHeight {
        /*max-height: 82px !important;
        max-width: 200px !important;*/
        /*width: 175px !important;
        height: 73px !important;*/
    }

    .initName {
        font-size: 12px !important;
        line-height: 65px !important;
    }

    .msgbox {
        max-height: 400px !important;
        overflow: auto;
    }

    .largeFont {
        font-size: 18px;
    }

    .extraMargin {
        margin-bottom: 10px !important;
    }

    li {
        list-style: inherit !important;
        margin-left: 15px !important;
    }
</style>

<infs:WclResourceManagerProxy runat="server" ID="rprxEditProfile">
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/bootstrap.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://fonts.googleapis.com/css?family=Titillium+Web:400,600,700" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/font-awesome.min.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="../Resources/Mod/Dashboard/Styles/SharedUserDashboard.css" ResourceType="StyleSheet" />
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Compliance/ContentEditor.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/Applicant/editprofile.css" ResourceType="StyleSheet" />
   <infs:LinkedResource Path="~/Resources/Mod/Shared/ApplyNewIcons.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<div class="container-fluid">
    <div class="row">
        <div class="col-md-12">
            <div class="bx_image customHeight">
                <asp:Image runat="server" ID="imgCntrl" class="thumb" />
            </div>

        </div>
    </div>
    <div>
        <div class="row extraMargin" id="dvJobTitle" runat="server" visible="false" style="margin-bottom: 0px !important;">
            <div class="col-md-12">
                <asp:Label runat="server" ID="lblJobTitle" Font-Size="XX-Large"></asp:Label>
            </div>
        </div>
        <div class="row extraMargin" id="dvCompany" runat="server" visible="false" style="margin-top: -5px;">
            <div class="col-md-12">
                <asp:Label runat="server" ID="lblCompany" Font-Size="X-Large"></asp:Label>
                <asp:Label ID="lblHyphen" runat="server" Text="-" Visible="false"></asp:Label>
                <asp:Label runat="server" ID="lblLocation" Font-Size="X-Large"></asp:Label>
            </div>
        </div>
        <div class="row">&nbsp;</div>
        <div class="row extraMargin" id="dvInstruction" runat="server" visible="false" style="">
            <div class="col-md-12">
                <span id="spnInstruction" class="largeFont" runat="server"></span>
            </div>
        </div>
        <div class="row extraMargin" id="dvDescription" runat="server" visible="false" style="">
            <div class="col-md-12">
                <span runat="server" class="largeFont" id="spnDescription"></span>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnHowToApply" Value='<%# Eval("HowToApply") %>' />
    <infsu:CommandBar ID="fsucCmdBarAgencyDetails" DisplayButtons="Save,Submit" runat="server" GridMode="false" ButtonPosition="Left" SubmitButtonIconClass="rbRefresh"
        SaveButtonIconClass="rbSave" SaveButtonText="Apply for Job" OnSaveClientClick="HowtoApplyPopUp"
        UseAutoSkinMode="False" AutoPostbackButtons="Submit" OnSubmitClick="fsucCmdBarAgencyDetails_SubmitClick" SubmitButtonText="Back to Listing" ButtonSkin="Silk" />

    <div id="dialog-message" style="display: none;" title="How To Apply">
        <div class="msgbox scrollBar" style="display: block; opacity: 1; max-height: 400px !important; overflow: auto;">
            <span runat="server" id="spnHowToApply"></span>
        </div>
    </div>
</div>
<script type="text/javascript">
    function HowtoApplyPopUp() {
        //debugger;
        dialog = $window.showDialog($jQuery("#dialog-message").clone().show(), { closeBtn: { autoclose: true, text: "Ok" }, }, 500, "How To Apply");
    }

</script>
