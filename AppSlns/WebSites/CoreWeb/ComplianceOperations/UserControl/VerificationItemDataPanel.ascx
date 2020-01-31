<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.VerificationItemDataPanel" CodeBehind="VerificationItemDataPanel.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ItemDataLoader" Src="ItemDataLoader.ascx" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>
<script type="text/javascript">

    Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
    Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };


</script>
<style type="text/css">
    .bullet ul {
        margin-left: 10px;
        padding-left: 10px !important;
    }

    .bullet li {
        list-style-position: inside;
        list-style: disc;
    }

    .bullet ol {
        list-style-type: decimal;
        margin-left: 10px;
        padding-left: 10px;
    }

        .bullet ol li {
            list-style: decimal;
        }
            .header-color {
 color: #8C1921 !important;
}
</style>
<infs:WclResourceManagerProxy runat="server" ID="manageUploadDocument">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceDataEntry/Scripts/UploadDocuments.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div style="display: none;">
    <infs:WclAsyncUpload runat="server" ID="hiddenuploader" HideFileInput="true" Skin="Hay"
        AllowedFileExtensions="ods,xls,xlsx,csv,png,jpg,jpeg,jpe,bmp,gif,tif,tiff,docx,doc,rtf,pdf,ODT,TXT,ODS,XLS,XLSX,CSV,PNG,JPG,JPEG,JPE,BMP,GIF,TIF,TIFF,DOCX,DOC,RTF,PDF,ODT,TXT"
        MultipleFileSelection="Automatic" OnClientValidationFailed="upl_OnClientValidationFailed">
        <Localization Select="Browse" />
    </infs:WclAsyncUpload>
</div>
<infs:WclResourceManagerProxy ID="rmpOrderReview" runat="server">
    <infs:LinkedResource Path="~/Resources/Mod/ComplianceOperations/ItemDataVerification.js"
        ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<div class="vdatapn-top border-box scroll-box">
    <div class="framebar" title="Category Name">
        <div class="title cat-icon">
            <asp:Label ID="lblSelectedCategoryName" runat="server" /><asp:Label id="spnRuleAppliedDate" runat="server" tooltip="Business Rule Applied Date" />
            <asp:Image ID="imageExceptionOff" ImageUrl="~/Resources/Mod/Compliance/icons/ExceptionsOffIcon.png" Visible="false" Style="vertical-align: text-bottom;"
                runat="server" ToolTip="Turned off Exception on this Category" />
             <asp:Image ID="imageSDEdisabled" ImageUrl="~/Resources/Mod/Compliance/icons/ExceptionsOffIcon-D.png" Visible="false" Style="vertical-align: text-bottom;"
                runat="server" ToolTip="Turned off Student Data Entry on this Category" />
        </div>
        <div class="commands">
            <!--Code changed by [BS] Image instaed of ImageButton. Performance improvement task-->
            <a onclick="openInPageFrame(this)" id="lnkBtnPreviousCategory" runat="server">
                <asp:Image ID="imgPreviousCategory" ImageUrl="~/Resources/Mod/Compliance/images/left-arrow-blue.png"
                    runat="server" ToolTip="Go to Previous Category" />
            </a><a onclick="openInPageFrame(this)" id="lnkBtnNextCategory" runat="server">
                <asp:Image ID="imgNextCategory" ImageUrl="~/Resources/Mod/Compliance/images/right-arrow-Blue.png"
                    runat="server" ToolTip="Go to Next Category" />
            </a>

            <%-- <infs:WclButton AutoPostBack="true" ButtonType="LinkButton" runat="server" OnClick="btnPrevCat_Click" ID="btnPrevCat" Height="19px" Width="20px"
                ToolTip="Go to Previous Category">
                <Image EnableImageButton="true" DisabledImageUrl="~/Resources/Mod/Compliance/images/left-arrowd.png"
                    ImageUrl="~/Resources/Mod/Compliance/images/left-arrow.png" />
            </infs:WclButton>
            <infs:WclButton AutoPostBack="true" ButtonType="LinkButton" runat="server" OnClick="btnNextCat_Click" ID="btnNextCat" Height="19px" Width="20px"
                ToolTip="Go to Next Category">
                <Image EnableImageButton="true" DisabledImageUrl="~/Resources/Mod/Compliance/images/right-arrowd.png"
                    ImageUrl="~/Resources/Mod/Compliance/images/right-arrow.png" />
            </infs:WclButton>--%>
        </div>
    </div>
    <div class="msgbox" id="dvMsgBox">
        <asp:Label ID="lblMessage" runat="server">                             
        </asp:Label>
    </div>
    <div runat="server" id="divCtrlSave" class="msgbox" visible="false">
        <div class="info">
            <span id="spnMsg">You can also save data by pressing Ctrl+S. </span>
            <div style="float: right">
                <infs:WclButton ID="btnRemindLater" runat="server" Text="Remind Me Later" OnClientClicking="HideNotification"
                    AutoPostBack="false">
                </infs:WclButton>
                <infs:WclButton ID="btnDismiss" runat="server" Text="Dismiss Forever" OnClientClicking="UpdateIgnoreAlert"
                    AutoPostBack="false">
                </infs:WclButton>
            </div>
        </div>
        <%--<asp:Button ID="btnDismiss" runat="server" OnClientClick="Update(); return false;"
            Text="Dismiss Forever" />--%>
        <%-- <infs:WclButton ID="btnDismiss" runat="server" OnClientClick="Update()" Text="Dismiss Forever" AutoPostBack="false" />--%>
    </div>
    <div runat="server" id="divDockUnDock" class="msgbox" visible="false">
        <div class="info">
            <span id="spnMsgDock">Dock/Undock feature available for document view .</span>
            <div style="float: right">
                <infs:WclButton ID="btnRemindLaterDock" runat="server" Text="Remind Me Later" OnClientClicking="HideNotificationForDock"
                    AutoPostBack="false">
                </infs:WclButton>
                <infs:WclButton ID="btnDismissDock" runat="server" Text="Dismiss Forever" OnClientClicking="UpdateIgnoreAlertForDock"
                    AutoPostBack="false">
                </infs:WclButton>
            </div>
        </div>
    </div>
    <%--<div class="filterbar">
    <div class="filter-inuse">
        Showing all items.</div>
    <span class="bar_icon" title="Filter Items"></span><span class="bar_cmds hid"><a
        href="#" onclick="manageItems(1)" title="View all the items of the category">Show
        all requirements</a> | <a href="#" onclick="manageItems(2)" title="View only items filled by the applicant">
            Show filled requirements</a> | <a href="#" onclick="manageItems(3)" title="View only items pending for review">
                Show only Pending for review</a></span>
</div>--%>
    <div class="section">
        <asp:HiddenField ID="hdnExplanatoryNoteState" runat="server" />
        <asp:HiddenField ID="hdnUserId" runat="server" />
        <asp:HiddenField ID="hdnIsExplanatoryNoteClosed" runat="server" />
        <asp:HiddenField ID="hdnCatExplanatoryNotes" ValidateRequestMode="Disabled" runat="server" />
        <asp:HiddenField ID="hdnCatMoreInfoURL" ValidateRequestMode="Disabled" runat="server" />

        <h2 class="header-color category">Category Information
            <div class="sec_cmds">
                <span class="bar_icon ihelp" title="View explanatory notes"></span>
            </div>
        </h2>
        <div class="content">
            <div class="tab-block">
                <div class="tabs">
                    <%--<span class="tab1 focused">Admin's Explanation</span> 
                    <span class="tab2">Applicant's VIDP Explanation</span>--%>
                    
                    <span class="tab2 focused">Requirement's Explanation</span> 
                    <span class="tab1">Review Standard Explanation</span> 
                </div>
                <div class="tab-content tab1">
                    <div class="bullet">
                        <asp:Literal ID="litAdminExplanation"  runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="tab-content tab2 focused">
                    <div class="bullet">
                        <asp:Literal ID="litApplicantExplanation" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
            <%-- <asp:Panel ID="pnlCategoryupdateOn" CssClass="sxform auto" runat="server">
            <div class="sxpnl">
                <div class='sxro sx1co'>
                    <div class='sxlb' title="The Name of the Admin and time that this category was last updated">
                        <span class="cptn">Last Updated</span>
                    </div>
                    <div class="dvlbllastupdatedby">
                        <span class="ronly" id="spnlbllastupdatedby">
                            <asp:Literal ID="lbllastupdatedby" runat="server"></asp:Literal></span>
                    </div>
                    <div class='sxroend'>
                    </div>
                </div>
            </div>
        </asp:Panel>--%>
            <asp:Panel ID="pnlCategoryLevel" CssClass="sxform auto" runat="server">
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
                    <div class='sxro monly'>
                        <div class='sxlb'>
                            <span class="cptn">Comments</span>
                        </div>
                        <infs:WclTextBox runat="server" ID="txtCategoryNotes" TextMode="MultiLine" Height="50px">
                        </infs:WclTextBox>
                        <asp:HiddenField ID="hdfApplicantComplianceCategoryId" runat="server" />
                        <div class='sxroend'>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <infsu:CommandBar ID="cbarCategory" runat="server" DisplayButtons="None" DefaultPanel="pnlCategoryLevel">
                <ExtraCommandButtons>
                    <infs:WclButton runat="server" ID="btnSaveCategoryNotes" OnClick="btnSaveCategoryNotes_Click"
                        Text="Save">
                        <Icon PrimaryIconCssClass="rbSave" />
                    </infs:WclButton>
                </ExtraCommandButtons>
            </infsu:CommandBar>
        </div>
        <%--<asp:Panel ID="pnlCategoryLevel" runat="server">
        <div class="formblock auto">
            <table border="0" cellpadding="0" cellspacing="0" class="form">
                <tr>
                    <td class="lbl">
                        Current Status :
                    </td>
                    <td class="elm">
                        <b>
                            <asp:Literal ID="litCategoryStatus" runat="server"></asp:Literal>
                        </b>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="lbl aligntop">
                        Write Note
                    </td>
                    <td class="elm" colspan="2">
                        <infs:WclTextBox runat="server" ID="txtCategoryNotes" TextMode="MultiLine" Height="50px">
                        </infs:WclTextBox>
                        <asp:HiddenField ID="hdfApplicantComplianceCategoryId" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="cmdbar">
            <infs:WclButton runat="server" ID="btnSaveCategoryNotes" OnClick="btnSaveCategoryNotes_Click"
                Text="Save">
                <Icon PrimaryIconCssClass="rbSave" />
            </infs:WclButton>
        </div>
    </asp:Panel>--%>
    </div>
    <asp:Panel ID="pnlLoader" runat="server" EnableViewState="true">
    </asp:Panel>
    <%--<infsu:ItemDataLoader
ID="ucItemDataLoader" runat="server"></infsu:ItemDataLoader>--%>
    <asp:HiddenField ID="hdnBtnPrevious" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnBtnNext" runat="server" ClientIDMode="Static" />
</div>
<div class="vdatapn-botom fixed_box">
    <div class="left">
        <infs:WclButton runat="server" ID="saveButton" OnClick="Save" OnClientClicked="verifyRejection" Style="display: none;"></infs:WclButton>
        <infsu:CommandBar ID="btnSave" runat="server" DisplayButtons="Save" ButtonPosition="Left"
            Visible="false" SaveButtonText="Save All Changes" OnSaveClick="Save" OnSaveClientClick="verifyRejection">
        </infsu:CommandBar>
        <asp:HiddenField ID="hdnIsSaveClicked" runat="server" Value="true" ClientIDMode="Static" />
    </div>
    <div style="width: 444px;" class="right">
        <infsu:CommandBar ID="CommandBar1" runat="server" DefaultPanel="pnlName1" DisplayButtons="None"
            ButtonPosition="Right">
            <ExtraCommandButtons>
                <infs:WclButton ID="btnPrevious" runat="server" Text="Save and Previous" OnClick="btnPreviousCat_Click" OnClientClicked="verifyRejection"
                    AutoPostBack="true" ToolTip="Click here to save and go to the previous category">
                    <Icon PrimaryIconCssClass="rbPrevious" />
                </infs:WclButton>
                <infs:WclButton ID="btnNext" runat="server" Text="Save and Next"
                    OnClientClicked="verifyRejection" AutoPostBack="true" OnClick="btnNext_Click" ToolTip="Click here to save and go to the next Category that is in Pending for Review status">
                    <Icon PrimaryIconCssClass="rbNext" />
                </infs:WclButton>
            </ExtraCommandButtons>
        </infsu:CommandBar>
    </div>
</div>
<a id="ankReloadPage" href="#" style="display:none"></a>
<asp:HiddenField ID="hdnNextSubscriptionURL" runat="server" />
<asp:HiddenField ID="hdnPreviousSubscriptionURL" runat="server" />
<script type="text/javascript" lang="javascript">

    function GetNextPreviousURL()
    {
        //debugger;
        if ($jQuery("[id$=lnkBtnNextApp]")[0] != undefined && $jQuery("[id$=lnkBtnPrevApp]")[0] != undefined) {
            var lnkBtnNextApp = $jQuery("[id$=lnkBtnNextApp]")[0].href;
            var lnkBtnPrevApp = $jQuery("[id$=lnkBtnPrevApp]")[0].href;
            $jQuery("[id$=hdnNextSubscriptionURL]").val(lnkBtnNextApp);
            $jQuery("[id$=hdnPreviousSubscriptionURL]").val(lnkBtnPrevApp);
        }
        else {
            var url = $jQuery("[id$=hdnPreviousSubscriptionURLApplicant]").val();
            $jQuery("[id$=hdnPreviousSubscriptionURL]").val(url);
            var url = $jQuery("[id$=hdnNextSubscriptionURLApplicant]").val();
            $jQuery("[id$=hdnNextSubscriptionURL]").val(url);
        }
    }

    function btnPreviousClick(s, e) {
        var url = $jQuery("input[type=hidden][id=hdnBtnPrevious]").val();
        openURLInPageFrame(url);
        return false;
    }

    function btnNextClick(s, e) {
        window.location.href = $jQuery("input[type=hidden][id=hdnBtnNext]").val();
        return false;
    }

    // Used for updating the alert notification status for Ctrl+ S functionality
    function UpdateIgnoreAlert() {
        var dataString = "organizationUserId : '" + '<%= CurrentLoggedInUserId %>' + "',ignoreAlert : '" + "true" + "'";
        var urltoPost = "/ComplianceOperations/Default.aspx/UpdateUtilityFeature";
        var control = $jQuery("[id$=divCtrlSave]").css("display", "none");

        //var control = $jQuery("[id$=divCtrlSave]").css("display", "none");
        $jQuery.ajax
         (
          {
              type: "POST",
              url: urltoPost,
              data: "{ " + dataString + " }",
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (data) {
                  var fileIdentifier = data.d;
              }
          });
    }

    //Used for temporary hiding of the notification message
    function HideNotification() {

        //var div = $jQuery("[id$=divCtrlSave]").css("display", "none");
        var urltoPost = "/ComplianceOperations/Default.aspx/RemindLater";
        var control = $jQuery("[id$=divCtrlSave]").css("display", "none");
        var dataString = "ignoreAlert : '" + "true" + "'";
        $jQuery.ajax
         (
          {
              type: "POST",
              url: urltoPost,
              data: "{ " + dataString + " }",
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (data) {
                  var fileIdentifier = data.d;
              }
          });
        return false;
    }

    // Used for updating the alert notification status for Dock/UnDock functionality
    function UpdateIgnoreAlertForDock() {
        var urltoPost = "/ComplianceOperations/Default.aspx/UpdateUtilityFeatureForDockUnDock";
        var dataString = "organizationUserId : '" + '<%= CurrentLoggedInUserId %>' + "',ignoreAlert : '" + "true" + "'";
        var control = $jQuery("[id$=divDockUnDock]").css("display", "none");
        $jQuery.ajax
         (
          {
              type: "POST",
              url: urltoPost,
              data: "{ " + dataString + " }",
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (data) {
                  var fileIdentifier = data.d;
              }
          });
    }

    //Used for temporary hiding of the notification message
    function HideNotificationForDock() {

        var div = $jQuery("[id$=divDockUnDock]").css("display", "none");
        var urltoPost = "/ComplianceOperations/Default.aspx/RemindLaterForDockUnDock";
        var dataString = "ignoreAlert : '" + "true" + "'";
        $jQuery.ajax
         (
          {
              type: "POST",
              url: urltoPost,
              data: "{ " + dataString + " }",
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (data) {
                  var fileIdentifier = data.d;
              }
          });
        return false;
    }

    function ReloadPage()
    {
        var url = location.href;
        var outeranchor = $jQuery("[id$=ankReloadPage]");
        outeranchor.attr('href', url);
        outeranchor[0].click();
    }
</script>
