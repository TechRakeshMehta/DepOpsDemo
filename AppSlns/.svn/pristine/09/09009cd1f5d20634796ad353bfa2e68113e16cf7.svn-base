<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminEntryCustomFormHtml.ascx.cs" Inherits="CoreWeb.AdminEntryPortal.Views.AdminEntryCustomFormHtml" %>
<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<%@ Register TagPrefix="infsu" TagName="CommandBar" Src="~/Shared/Controls/CommandBar.ascx" %>
<infs:WclResourceManagerProxy runat="server" ID="rprxSetUp">
    <infs:LinkedResource Path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/BkgOperations/BkgOperations.js" ResourceType="JavaScript" />
    <infs:LinkedResource Path="~/Resources/Mod/BkgOperations/BkgOperationForSupplemental.js" ResourceType="JavaScript" />
</infs:WclResourceManagerProxy>
<style>
    .hlink {
        cursor: pointer;
    }

    #DefaultContent_ucDynamicControl_ctl00_dvPhone span.RadInput {
        width: 89% !important;
    }

    #DefaultContent_ucDynamicControl_ctl00_dvPhone .rbToggleCheckbox {
        padding-left: 0px !important;
        vertical-align: top;
        padding: 0px !important;
    }

    .ChckBox {
        padding-left: 0px !important;
        vertical-align: top;
        padding: 0px !important;
    }

    #DefaultContent_ucDynamicControl_ctl00_dvPhone .vldx {
        /*margin-left: -180px !important;*/
    }

    .myControl .RadInput {
        width: 90% !important;
    }

    .myControl input[type="checkbox"] {
        margin-top: 4px;
        position: absolute;
        right: 10px;
        top: 2px;
    }

    .myControl {
        position: relative;
    }

    /*[SS]:[10/30/2018]: Added below css for the visibility of disabled dropdown on mobile device.*/
    .RadComboBox_Outlook .rcbDisabled .rcbInput {
        color: #4D4D4D !important;
    }
</style>

<infs:WclButton ID="cmdbarEdit" Text="Edit" runat="server" OnClick="CmdBarEditCustomForm_Click" Style="display: none;" />
<asp:Panel ID="pnlRendercustomForm" runat="server">
    <asp:HyperLink ID="lnkEDSDocumentTemp" runat="server" Enabled="true" Text="Electronic Drug Screen Report"
        Visible="false" Font-Underline="true" BackColor="Transparent" BorderStyle="None" ForeColor="Black" Target="_blank" onclick="openEDSDocumentByOrderID(this)" CssClass="hlink">
    </asp:HyperLink>
</asp:Panel>
<div id="hiddenInstance" runat="server">
    <asp:HiddenField ID="hdnInstanceId" runat="server" />
</div>
<asp:HiddenField ID="hdnGroupId" runat="server" />
<asp:HiddenField ID="hdnCurrentFormId" runat="server" />
<asp:HiddenField ID="hdnHiddenDropDownIds" runat="server" />
<asp:HiddenField ID="hdnHiddenTextBoxIds" runat="server" />
<asp:HiddenField ID="hdnServiceItemId" runat="server" />
<asp:HiddenField ID="hdnServiceId" runat="server" />
<asp:HiddenField ID="hdnPackageServiceId" runat="server" />

<asp:HiddenField ID="hdfTenantId" runat="server" />
<asp:HiddenField ID="hdfOrderID" runat="server" />
<asp:HiddenField ID="hdfDocumentType" runat="server" />
<asp:HiddenField ID="hdnDecisionFieldId" Value="" runat="server" />
<asp:HiddenField ID="hdnIsLocationServiceTenant" Value="" runat="server" />
<asp:HiddenField ID="hdnIFSelectedValueBlank" Value="" runat="server" />
<div id="CommandBar" runat="server">
    <infsu:CommandBar ID="fsucCmdBar1" runat="server" ButtonPosition="Center" DisplayButtons="Save"
        AutoPostbackButtons="Save" SaveButtonIconClass="rbAdd" OnSaveClientClick="SetInstanceId"
        SaveButtonText="Add New" OnSaveClick="CmdBarRestart_Click">
    </infsu:CommandBar>
</div>
<script type="text/javascript">
    //UAT-2063:Combine the screens to add new Alias and add new locations
    var isFocusSet = false;
    var winopen = false;
    function openEDSDocumentByOrderID() {
        var tenantId = $jQuery("#<%= hdfTenantId.ClientID %>").val()
        var orderId = $jQuery("#<%= hdfOrderID.ClientID %>").val();
        var documentType = $jQuery("#<%= hdfDocumentType.ClientID %>").val();
        var composeScreenWindowName = "Electronic Drug Screening Form";
        //UAT-2364
        var popupHeight = $jQuery(window).height() * (100 / 100);

        var url = $page.url.create("~/AdminEntryPortal/Pages/AdminEntryServiceFormViewer.aspx?OrderID=" + orderId + "&DocumentType=" + documentType + "&tenantId=" + tenantId);
        var win = $window.createPopup(url, { size: "800," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: composeScreenWindowName, onclose: OnClientClose });
        winopen = true;
        return false;
    }

    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            winopen = false;
        }
    }

    // Add method to do click and remove colaspe of accordion mhdr class
    function stopColapseCustomForm(customFormId, btnID) {
        //debugger;
        $jQuery("#divCustomFormMhdr" + customFormId).removeClass("mhdr");
        $jQuery("#divCustomFormMhdr" + customFormId).addClass("newMhdr");
        $find(btnID).click();
    }

    $jQuery(document).ready(function () {

        //$jQuery("#ctl00_DefaultContent_ucDynamicControl_ctl00_txtPlain_Text_1_2_25").on("focus", function () {
        //  //  alert();
        //});

        if ($jQuery("[doFocustxt$='focus']").length > 0) {
            doFocusTextBox();
        }
        if ($jQuery("[doFocusCmb$='focus']").length > 0) {
            doFocusComboBox();
        }
    });
    //UAT-2074 : When I hit “Add New” button, I shouldn’t have to click into State (for location) or Alias boxes.
    function doFocusComboBox() {
        //debugger;
        if ($jQuery("[doFocusCmb$='focus']").length > 0 && !isFocusSet) {
            var id = $jQuery($jQuery("[doFocusCmb$='focus']").last())[0].id;
            $jQuery($jQuery("#" + id + " input[type=text]")[0]).focus();
            $jQuery("[doFocusCmb$= 'focus']").removeAttr('doFocusCmb');
        }
    }
    //UAT-2074 : When I hit “Add New” button, I shouldn’t have to click into State (for location) or Alias boxes.
    function doFocusTextBox() {
        if ($jQuery("[doFocustxt$='focus']").length > 0) {
            isFocusSet = true;
            var id = $jQuery($jQuery("[doFocustxt$='focus']").last())[0].id;
            $jQuery($jQuery("#" + id)[0]).focus();
            $jQuery("[doFocustxt$= 'focus']").removeAttr('doFocustxt');
        }
        //UAT-2063:Combine the screens to add new Alias and add new locations
        else { isFocusSet = false; }
    }

    function onLocationBlur(sender, args) {
        //if (sender.get_highlightedItem() != null && (sender.get_originalText() != null && sender.get_originalText() != sender.get_highlightedItem().get_text()))
        //    sender.get_highlightedItem().select();
        //else
        //    sender.set_text(""); 
        if (sender._value == '' && !sender._checkBoxes) {
            sender.trackChanges();
            sender.clearSelection();
            sender.set_text('');
            sender.commitChanges();
        }
    }

    //UAT-3663-- Added clientDropdownClosing 
    var isSecondAttampt = false;
    function clientDropdownClosing(sender, args) {
        //  debugger;
        if (sender._text != "" && sender._value == "" && !isSecondAttampt) {
            isSecondAttampt = true;
            var text = sender._text;
            $('#<%=hdnIFSelectedValueBlank.ClientID %>').val(text);
            sender.set_text("");
        } else if ($('#<%=hdnIFSelectedValueBlank.ClientID %>').val() != "" && sender._value == "" && isSecondAttampt) {
            sender._value = $('#<%=hdnIFSelectedValueBlank.ClientID %>').val();
            $('#<%=hdnIFSelectedValueBlank.ClientID %>').val("");
            isSecondAttampt = false;

            sender.trackChanges();
            sender.commitChanges();
        }
    }


    //UAT-2448
    function openPopUp() {
        var popupWindowName = "Country Identification Details";
        winopen = true;
        var popupHeight = $jQuery(window).height() * (100 / 100);
        var url = $page.url.create("~/AdminEntryPortal/Pages/AdminEntryCountryIdentificationDetails.aspx");
        var win = $window.createPopup(url, { size: "500," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move, name: popupWindowName, onclose: OnClientClose });
        return false;
    }

    function OnClientClose(oWnd, args) {
        oWnd.remove_close(OnClientClose);
        if (winopen) {
            var arg = args.get_argument();
            if (arg) {
            }
            winopen = false;
        }
    }

    // UAT-2447
    function MaskedUnmaskedPhone(ID, MaskedTextBoxId, PlainTextBoxID, revPlainTextBox, revMaskedPhoneID, regPlainTextID) {
        //debugger;
        if (ID != undefined && ID != '') {
            var chckedBox = $jQuery("[id$=" + ID + "]")[0];
            var isDisicionFieldEnabled = chckedBox.getAttribute('isEnabled');
            if (chckedBox.checked) {
                //debugger;
                var MaskedTextBox = $jQuery("[id$=" + MaskedTextBoxId + "]")[0];
                MaskedTextBox.style.display = "none";
                var PlainTextBoxValidator = ($jQuery("[id$=" + revPlainTextBox + "]")[0]);
                if (PlainTextBoxValidator != "" && PlainTextBoxValidator != undefined && (isDisicionFieldEnabled == undefined || isDisicionFieldEnabled == "true")) {
                    ValidatorEnable(PlainTextBoxValidator, true);
                    //PlainTextBoxValidator.hide();
                    $jQuery("[id$=" + revPlainTextBox + "]").hide();
                }
                var MaskedtextBoxValidator = ($jQuery("[id$=" + revMaskedPhoneID + "]")[0]);
                if (MaskedtextBoxValidator != "" && MaskedtextBoxValidator != undefined) {
                    ValidatorEnable(MaskedtextBoxValidator, false);
                }
                var PlainTExtBoxRegularExp = ($jQuery("[id$=" + regPlainTextID + "]")[0]);
                if (PlainTExtBoxRegularExp != "" && PlainTExtBoxRegularExp != undefined && (isDisicionFieldEnabled == undefined || isDisicionFieldEnabled == "true")) {
                    ValidatorEnable(PlainTExtBoxRegularExp, true);
                    $jQuery("[id$=" + regPlainTextID + "]").hide();
                }

                var PlainTextBox = $jQuery("[id$=" + PlainTextBoxID + "]")[0];
                PlainTextBox.style.display = "block";
            }
            else {
                var MaskedTextBox = $jQuery("[id$=" + MaskedTextBoxId + "]")[0];
                MaskedTextBox.style.display = "block";
                var PlainTextBox = $jQuery("[id$=" + PlainTextBoxID + "]")[0];
                PlainTextBox.style.display = "none";
                var PlainTextBoxValidator = ($jQuery("[id$=" + revPlainTextBox + "]")[0]);
                if (PlainTextBoxValidator != "" && PlainTextBoxValidator != undefined) {
                    ValidatorEnable(PlainTextBoxValidator, false);
                }
                var MaskedtextBoxValidator = ($jQuery("[id$=" + revMaskedPhoneID + "]")[0]);
                if (MaskedtextBoxValidator != "" && MaskedtextBoxValidator != undefined && (isDisicionFieldEnabled == undefined || isDisicionFieldEnabled == "true")) {
                    ValidatorEnable(MaskedtextBoxValidator, true);
                    //MaskedtextBoxValidator.hide();
                    $jQuery("[id$=" + revMaskedPhoneID + "]").hide();
                }
                var PlainTExtBoxRegularExp = ($jQuery("[id$=" + regPlainTextID + "]")[0]);
                if (PlainTExtBoxRegularExp != "" && PlainTExtBoxRegularExp != undefined) {
                    ValidatorEnable(PlainTExtBoxRegularExp, false);
                }
            }
        }
    }

    //UAT-2447
    function ShowHidePhoneOnLoad(instanceid, isEnabled) {
        $jQuery(".dvPhone").each(function (i) {
            i = i + 1;
            var chkInternationalPhone = $jQuery("[id$=dvPhone_" + i + "] input[type='checkbox']")[0];
            var CurrentPhoneId = $jQuery("[id$=dvPhone_" + i + "]");
            if (CurrentPhoneId != undefined && CurrentPhoneId.length > 0) {
                var CurrentinstanceID = $jQuery("[id$=dvPhone_" + i + "] input[type='text']")[1].id.split('Text')[1].split('_')[1];
                var AtrributeGroupMappingId = $jQuery("[id$=dvPhone_" + i + "] input[type='text']")[1].id.split('Text')[1].split('_')[3];
                if (chkInternationalPhone == undefined)
                    return;
                if (instanceid != undefined && isEnabled != undefined && instanceid == CurrentinstanceID) {
                    $jQuery("[id$=dvPhone_" + i + "] input[type='checkbox']").attr('isEnabled', isEnabled);
                }
                if (!chkInternationalPhone.checked) {
                    var MaskedPhoneElement = $jQuery("[id$=dvMaskedText_" + i + "]")[0];
                    MaskedPhoneElement.style.display = "block";

                    var PalinPhoneElement = $jQuery("[id$=dvPlainText_" + i + "]")[0];
                    PalinPhoneElement.style.display = "none";

                    if (CurrentinstanceID != undefined && CurrentinstanceID != '' && AtrributeGroupMappingId != undefined && AtrributeGroupMappingId != '') {
                        var refPlaintextBoxID = "rfv_Text_Plain_" + CurrentinstanceID + "_" + AtrributeGroupMappingId;

                        var reqplainText = $jQuery("[id$=dvPhone_" + i + "]").find("[id$=" + refPlaintextBoxID + "]")[0]
                        if (reqplainText != undefined) {
                            ValidatorEnable(reqplainText, false);
                        }
                    }

                }
                else {
                    var MaskedPhoneElement = $jQuery("[id$=dvMaskedText_" + i + "]")[0];
                    MaskedPhoneElement.style.display = "none";

                    var PalinPhoneElement = $jQuery("[id$=dvPlainText_" + i + "]")[0];
                    PalinPhoneElement.style.display = "block";

                    if (CurrentinstanceID != undefined && CurrentinstanceID != '' && AtrributeGroupMappingId != undefined && AtrributeGroupMappingId != '') {
                        var refMaskedtextBox = "rfv_Text_" + CurrentinstanceID + "_" + AtrributeGroupMappingId;
                        var reqMaskText = $jQuery("[id$=dvPhone_" + i + "]").find("[id$=" + refMaskedtextBox + "]")[0];
                        if (reqMaskText != undefined) {
                            ValidatorEnable(reqMaskText, false);
                        }
                    }
                }
            }
        });
    }

    function OpenCustomForm(url) {

        var popupHeight = $jQuery(window).height() * (110 / 100);
        var win = $window.createPopup(url, { size: "1000," + popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, onclose: OnClose }
            , function () {
                this.set_title("Applicant Details");
            });
    }

    function OnClose(oWnd, args) {

        oWnd.remove_close(OnClose);
        //var arg = args.get_argument();
        //if (arg) {
        //    if (arg.IsStatusSaved) {
        var btnRefeshPage = $jQuery("[id$=btnRefeshPage]", $jQuery(window.parent.document));
        btnRefeshPage[0].click();
        //    }
        //}
    }

    function ShowProgressBar() {
        Page.showProgress('Processing...');
    }

    function ClearDropDownIfNoValueSelected(sender, args) {
        if (sender._value == '' && !sender._checkBoxes) {
            sender.trackChanges();
            sender.clearSelection();
            sender.set_text('');
            sender.commitChanges();
        }
    }

</script>
