<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequirementItemDataPanel.ascx.cs" Inherits="CoreWeb.ClinicalRotation.Views.RequirementItemDataPanel" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>

<infs:WclResourceManagerProxy runat="server" ID="rprxAdminView">
    <infs:LinkedResource Path="~/Resources/Mod/ClinicalRotation/RequirementVerificationDetail.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>

<style type="text/css">
    .vdatapn-botom {
        bottom: 0;
        height: 37px;
        position: relative;
        width: inherit;
        padding-top: 2px;
        background: url(../../Resources/Mod/Compliance/images/cmdbar.png) repeat-x scroll 0 0 rgba(0, 0, 0, 0);
    }

    .fixed_box {
        position: absolute;
        width: 100%;
        *position: relative;
    }

    .vdatapn-top {
        overflow-y: auto;
    }

    .header-color {
        color: #8C1921 !important;
    }
</style>

<div class="vdatapn-top border-box scroll-box">
    <div class="framebar" title="Category Name">
        <div class="title cat-icon">
            <asp:Label ID="lblCategoryName" runat="server" />
            <asp:Image ID="imageExceptionOff" ImageUrl="~/Resources/Mod/Compliance/icons/ExceptionsOffIcon.png" Visible="false" Style="vertical-align: text-bottom;"
                runat="server" ToolTip="Turned off Exception on this Category" />
             <asp:Image ID="imageSDEdisabled" ImageUrl="~/Resources/Mod/Compliance/icons/ExceptionsOffIcon-D.png" Visible="false" Style="vertical-align: text-bottom;"
                runat="server" ToolTip="Turned off Student Data Entry on this Category" />
            <asp:Image ID="imageADEdisabled" ImageUrl="~/Resources/Mod/RotationPackages/icon/ExceptionsOffIcon-DataMovement.png" Visible="false" Style="vertical-align: text-bottom;"
                runat="server" ToolTip="Turned off Data Movement on this category" />
        </div>
        <div class="commands">
            <a onclick="openInPageFrame(this)" id="lnkBtnPreviousCategory" runat="server">
                <asp:Image ID="imgPreviousCategory" ImageUrl="~/Resources/Mod/Compliance/images/left-arrow-blue.png"
                    runat="server" ToolTip="Go to Previous Category" />
            </a><a onclick="openInPageFrame(this)" id="lnkBtnNextCategory" runat="server">
                <asp:Image ID="imgNextCategory" ImageUrl="~/Resources/Mod/Compliance/images/right-arrow-Blue.png"
                    runat="server" ToolTip="Go to Next Category" />
            </a>
        </div>
    </div>
    <div class="msgbox" id="dvMsgBox">
        <asp:Label ID="lblMessage" runat="server">                             
        </asp:Label>
    </div>
    <div class="section catInfoContainer">

        <h2 class="header-color category">Category Information
            <div class="sec_cmds" onclick="ShowHideNotes(this);">
                <span class="bar_icon ihelp" title="View explanatory notes"></span>
            </div>
        </h2>
        <div class="showHideContent">
            <div class="tab-block" style="display: block;">
                <div class="tabs">
                    <%--<span class="tab1 focused">Admin's Explanation</span>
                    <span class="tab2">Applicant's ROT Explanation</span>--%>
                    
                    <span class="tab2 focused">Requirement's Explanation</span>
                    <span class="tab1">Review Standard Explanation</span>
                </div>
                <div class="tab-content tab1">
                    <div class="bullet">
                        <asp:Literal ID="litCategoryDesc" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="tab-content tab2 focused">
                    <div class="bullet">
                        <asp:Literal ID="litExplanatoryNotes" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <div class="section content">
        <div class="sxform auto">
            <div class="sxpnl">
                <div class='sxro sx1co'>
                    <div class='sxlb' title="The current verification status for this Item">
                        <span class="cptn">Current Status</span>
                    </div>
                    <div class='sxlm'>
                        <span class="ronly">
                            <asp:Literal ID="litCategoryStatus" runat="server"></asp:Literal></span>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlLoader" runat="server">
    </asp:Panel>
</div>
<div class="vdatapn-botom fixed_box">
    <div class="left">
        <infsu:CommandBar ID="cmdBar" runat="server" ButtonPosition="Center" DisplayButtons="Save"
            AutoPostbackButtons="Submit,Save,Cancel,Clear,Extra" OnSubmitClick="cmdBar_SubmitClick" OnSaveClientClick="validateItemRejectionReason"
            SaveButtonIconClass="rbSave" SubmitButtonText="Save all and Next" OnSubmitClientClick="validateItemRejectionReason" OnSaveClick="cmdBar_SaveClick"
            SaveButtonText="Save all Changes">
        </infsu:CommandBar>
    </div>
    <div style="width: 444px;" class="right">
        <infsu:CommandBar ID="CommandBar1" runat="server" DefaultPanel="pnlName1" DisplayButtons="None"
            ButtonPosition="Right">
            <ExtraCommandButtons>
                <infs:WclButton ID="btnPrevious" runat="server" Text="Save and Previous" OnClick="cmdBar_CancelClick" OnClientClicked="validateItemRejectionReason"
                    AutoPostBack="true" ToolTip="Click here to save and go to the previous category">
                    <Icon PrimaryIconCssClass="rbPrevious" />
                </infs:WclButton>
                <infs:WclButton ID="btnNext" runat="server" Text="Save and Next"
                    OnClientClicked="validateItemRejectionReason" AutoPostBack="true" OnClick="cmdBar_ExtraClick"
                    ToolTip="Click here to save and go to the next Category that is in Pending for Review status">
                    <Icon PrimaryIconCssClass="rbNext" />
                </infs:WclButton>
            </ExtraCommandButtons>
        </infsu:CommandBar>
    </div>



</div>

<script type="text/javascript">
    $jQuery(document).ready(function () {
        $jQuery('.showHideContent').slideUp();
    });

    function ShowHideNotes(sender) {
        $jQuery(sender).parents('.catInfoContainer').find('span').toggleClass('help_on');
        $jQuery(sender).parents('.catInfoContainer').find('.showHideContent').toggle('slow');
    }

    function ChangeApplicantPanelData(RequirmenetSubId, tenantId, clinicalRotationId)
    {
        //debugger;
        $jQuery.ajax({
            type: "POST",
            url: "Default.aspx/GetCategoryUpdatedUrl",
            data: "{'RequirementSubscriptionId':'" + RequirmenetSubId + "','TenantId':'" + tenantId + "','ClinicalRotationId':'" + clinicalRotationId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                //debugger;
                var liCatItems = $jQuery("[id$=lstCategories]").find("li");
                $jQuery(liCatItems).each(function () {
                    //debugger;
                    var CategoryId = $jQuery(this).find("a")[0].attributes["CategoryID"].value;
                    var originalImgUrl = $jQuery(this).find('[id$=imgStatus]').attr('src');
                    var imgStatus = $jQuery(this).find('[id$=imgStatus]');

                    response.d.forEach(function (i) {
                        //debugger;
                        var newImgUrl = i.Item2;
                        var newCatImageTooltip = i.Item3;
                        if (i.Item1 == CategoryId) {
                            if (originalImgUrl != newImgUrl) {
                                imgStatus.attr('src', newImgUrl);
                                imgStatus.attr('title', newCatImageTooltip);
                            }
                        }
                    });
                });
            },
            failure: function (response) {
                //debugger;
            }
        });
    }

</script>
