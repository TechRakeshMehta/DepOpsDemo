<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminEntryCustomFormLoad.ascx.cs" Inherits="CoreWeb.AdminEntryPortal.Views.AdminEntryCustomFormLoad" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<%@ Register Src="AdminEntryCustomFormHtml.ascx" TagName="CustomFormHtlm" TagPrefix="uc1" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>

<style type="text/css">
    #dvNextBtnStyleInSpanish.nextBtnStyleInSpanish .rbNext + .rbPrimary {
        padding-right: 20px !important;
    }
</style>

<asp:Panel ID="pnlLoader" runat="server">
</asp:Panel>
<div style="margin-top: 17px;" class="cmdButtonMinSize">
    <div id="dvNextBtnStyleInSpanish" class="nextBtnStyleInSpanish">
        <infsu:CommandBar ID="fsucCmdBar1" runat="server" ButtonPosition="Center" DisplayButtons="Clear,Submit,Extra" DefaultPanel="pnlLoader" DefaultPanelButton="Clear"
            AutoPostbackButtons="Clear,Submit" CauseValidationOnCancel="true" SaveButtonText="Cancel" ExtraButtonText="Save"
            ExtraButtonIconClass="rbSave" OnExtraClientClick="ValidatePage" OnExtraClick="fsucCmdBar1_ExtraClick"
            SubmitButtonText="<%$ Resources:Language, PREVIOUS %>" SubmitButtonIconClass="rbPrevious" ClearButtonIconClass="rbNext"
            ClearButtonText="<%$ Resources:Language, NEXT %>" OnClearClientClick="ValidatePage"
            OnSubmitClick="CmdBarRestart_Click" OnClearClick="CmdBarSave_Click" SaveButtonIconClass="rbCancel" OnSaveClick="fsucCmdBar1_SubmitClick">
            <ExtraCommandButtons>
                <infs:WclButton ID="btnCancelOrder" runat="server" UseSubmitBehavior="false" CssClass="margin-2 cancelposition"
                    AutoPostBack="true" OnClick="fsucCmdBar1_SubmitClick">
                    <Icon PrimaryIconCssClass="rbCancel" />
                </infs:WclButton>
            </ExtraCommandButtons>
        </infsu:CommandBar>
    </div>
</div>
<asp:HiddenField runat="server" ID="hdnFormId" />
<asp:HiddenField runat="server" ID="hdnInstanceCount" />
<asp:HiddenField ID="hdnHiddenPanels" runat="server" />
<asp:HiddenField runat="server" ID="hdnGroupidandIntanceNumber" />
<asp:HiddenField runat="server" ID="hdnEDrugScreenCustomFormId" />
<asp:HiddenField runat="server" ID="hdnIsedcisionField" />
<asp:HiddenField runat="server" ID="hdnEDrugScreenAttributeGrupId" />
<asp:HiddenField runat="server" ID="hdnCurrentEmployerDecisionField" />
<asp:HiddenField runat="server" ID="hdnIsAdminCreateOrder" Value="false" />
<asp:HiddenField ID="hdnLanguageCode" Value="AAAA" runat="server" />
<script type="text/javascript">
    function ValidatePage(s, e) {
        var validation = $jQuery(".errmsg");
        if (validation.length > 0) {
            var isValid = false;
            var groupIds = $jQuery("[id$=hdnGroupId]");
            if (groupIds.length > 0) {
                for (i = 0; i < groupIds.length; i++) {
                    isValid = Page_ClientValidate('submitForm' + groupIds[i].value);
                    if (!isValid) { return isValid; }
                }
            }
            if (isValid && s != undefined && e != undefined) {
                DelayButtonClick(s, e);
            }
            return isValid;
        }
        if (s != undefined && e != undefined) {
            DelayButtonClick(s, e);
        }
        return true;
    }

    var submitclicked = false;
    function DelayButtonClick(s, e) {
        if (submitclicked == false) {
            submitclicked = true;
            s.set_autoPostBack(false);
            window.setTimeout(function () {
                s.set_autoPostBack(true);
                s.click();
                submitclicked = false;
            }, 200, s);
        }
    }

    function RefeshPage() {
        var oWnd = GetRadWindow();
        var oArg = {};
        oArg.IsStatusSaved = true;
        oWnd.Close(oWnd, oArg);
    }

    function GetRadWindow() {
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }

    function pageLoad() {
        // debugger;
        var LanguageCode = $jQuery("[id$=hdnLanguageCode]").val();
        if (LanguageCode == 'AAAA')
            $jQuery("[id$=dvNextBtnStyleInSpanish]").removeClass("nextBtnStyleInSpanish");
        if (LanguageCode == 'AAAB')
            $jQuery("[id$=dvNextBtnStyleInSpanish]").addClass("nextBtnStyleInSpanish");
    }
</script>
