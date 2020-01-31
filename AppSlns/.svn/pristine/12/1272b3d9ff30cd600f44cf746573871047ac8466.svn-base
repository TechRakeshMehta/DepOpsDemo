<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequirementVerificationCategoryControl.ascx.cs"
    Inherits="CoreWeb.ClinicalRotation.Views.RequirementVerificationCategoryControl" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Src="~/ClinicalRotation/UserControl/RequirementVerificationItemControl.ascx"
    TagPrefix="uc" TagName="ItemControl" %>


<style type="text/css">
    .statusImg
    {
        padding-right: 7px;
        vertical-align: sub;
    }

    label
    {
        font-size: 11px !important;
    }

    .section:hover
    {
        background-color: none;
        background: none;
    }
</style>



<div class="container-fluid">
    <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-6">
                <div class="row h2text">
                    <asp:Image ID="imgCatStatus" runat="server" CssClass="statusImg" /><asp:Literal ID="litCatName"
                        runat="server"></asp:Literal>
                </div>
            </div>
            <%----UAT-1555--%>
            <div class="col-md-6">
                <div class="row text-right">
                    <div class="sec_cmds" onclick="ShowHideNotes(this);">
                        <span title="View explanatory notes" class="bar_icon ihelp"></span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">&nbsp;</div>
    <div class="showHideContent">
        <asp:Literal ID="litExplanatoryNotes" runat="server"></asp:Literal>
    </div>
    <asp:Panel ID="pnlItemContainer" runat="server">
    </asp:Panel>
</div>
<script type="text/javascript">
    $jQuery('.showHideContent').slideUp();
    function ShowHideNotes(sender) {
        $jQuery(sender).parents('.container-fluid').find('span').toggleClass('help_on');
        $jQuery(sender).parents('.container-fluid').find('.showHideContent').toggle('slow');
    }

</script>
