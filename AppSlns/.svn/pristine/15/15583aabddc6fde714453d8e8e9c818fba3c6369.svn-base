<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CoreWeb.ComplianceOperations.Views.ReconciliationItemDataPanel" CodeBehind="ReconciliationItemDataPanel.ascx.cs" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register TagPrefix="infsu" TagName="ItemDataLoader" Src="ReconciliationItemDataLoader.ascx" %>
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
            <asp:Label ID="lblSelectedCategoryName" runat="server" />
            <asp:Image ID="imageExceptionOff" ImageUrl="~/Resources/Mod/Compliance/icons/ExceptionsOffIcon.png" Visible="false" Style="vertical-align: text-bottom;"
                runat="server" ToolTip="Turned off Exception on this Category" />
             <asp:Image ID="imageSDEdisabled" ImageUrl="~/Resources/Mod/Compliance/icons/ExceptionsOffIcon-D.png" Visible="false" Style="vertical-align: text-bottom;"
                runat="server" ToolTip="Turned off Student Data Entry on this Category" />
        </div>
    </div>
    <div class="msgbox" id="dvMsgBox">
        <asp:Label ID="lblMessage" runat="server">                             
        </asp:Label>
    </div>
    <%--<div runat="server" id="divCtrlSave" class="msgbox" visible="false">
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
    </div>--%>
    <%--<div runat="server" id="divDockUnDock" class="msgbox" visible="false">
        <div class="info">
            <span id="spnMsgDock">Dock/Undock feature available for document view .</span>--%>
    <%--<div style="float: right">
                <infs:WclButton ID="btnRemindLaterDock" runat="server" Text="Remind Me Later" OnClientClicking="HideNotificationForDock"
                    AutoPostBack="false">
                </infs:WclButton>
                <infs:WclButton ID="btnDismissDock" runat="server" Text="Dismiss Forever" OnClientClicking="UpdateIgnoreAlertForDock"
                    AutoPostBack="false">
                </infs:WclButton>
            </div>--%>
    <%--  </div>
    </div>--%>
    <div class="section">
        <asp:HiddenField ID="hdnExplanatoryNoteState" runat="server" />
        <asp:HiddenField ID="hdnUserId" runat="server" />
        <asp:HiddenField ID="hdnIsExplanatoryNoteClosed" runat="server" />
        <asp:HiddenField ID="hdnCatExplanatoryNotes" ValidateRequestMode="Disabled" runat="server" />
        <asp:HiddenField ID="hdnCategoryMoreInfoURL" ValidateRequestMode="Disabled" runat="server" />

        <h2 class="header-color category">Category Information
            <div class="sec_cmds">
                <span class="bar_icon ihelp" title="View explanatory notes"></span>
            </div>
        </h2>
        <div class="content">
            <div class="tab-block">
                <div class="tabs">
                    <%--<span class="tab1 focused">Admin's Explanation</span> 
                    <span class="tab2">Applicant's RECO Explanation</span>--%>
                    <span class="tab2 focused">Requirement's Explanation</span>
                    <span class="tab1">Review Standard Explanation</span>                     
                </div>  
                <div class="tab-content tab1">
                    <div class="bullet">
                        <asp:Literal ID="litAdminExplanation" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="tab-content tab2 focused">
                    <div class="bullet">
                        <asp:Literal ID="litApplicantExplanation" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
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
    </div>
    <asp:Panel ID="pnlLoader" runat="server" EnableViewState="true">
    </asp:Panel>
    <asp:HiddenField ID="hdnBtnPrevious" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnBtnNext" runat="server" ClientIDMode="Static" />
</div>
<div class="vdatapn-botom fixed_box">
    <div>
        <infs:WclButton runat="server" ID="saveButton" OnClick="Save" OnClientClicked="verifyRejection" Style="display: none;"></infs:WclButton>
        <infsu:CommandBar ID="btnSave" runat="server" DisplayButtons="Save, Submit" ButtonPosition="Left"
            Visible="false" SaveButtonText="Save All Changes" OnSaveClick="Save" OnSaveClientClick="verifyRejection"
            OnSubmitClick="btnSave_SubmitClick" OnSubmitClientClick="verifyRejection" SubmitButtonText="Save and Next" SubmitButtonIconClass="rbNext">
        </infsu:CommandBar>
        <asp:HiddenField ID="hdnIsSaveClicked" runat="server" Value="true" ClientIDMode="Static" />
    </div>
</div>
<script type="text/javascript" language="javascript">
    function btnPreviousClick(s, e) {
        window.location.href = $jQuery("input[type=hidden][id=hdnBtnPrevious]").val();
        return false;
    }

    function btnNextClick(s, e) {
       // debugger;
        window.location.href = $jQuery("input[type=hidden][id=hdnBtnNext]").val();
        return false;
    }
</script>
