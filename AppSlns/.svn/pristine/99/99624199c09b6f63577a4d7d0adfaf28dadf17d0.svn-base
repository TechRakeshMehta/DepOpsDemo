if (Telerik.Web.UI.RadAsyncUpload != null && Telerik.Web.UI.RadAsyncUpload != undefined) {
    Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; };
    Telerik.Web.UI.RadAsyncUpload.Modules.Silverlight.isAvailable = function () { return false; };
}


var selectedFileIndex;
function onClientFileSelected(sender, args) {
    selectedFileIndex = args.get_rowIndex();
}

function onFileUploaded(sender, args) {
    if (sender.getUploadedFiles() != "") {
        showHideButton(true);
    }
}

function onFileRemoved(sender, args) {
    if (sender.getUploadedFiles() == "") {
        showHideButton(false)
    }
}

function showHideButton(visible) {
    clearDocumentsHiddenFields();
    $jQuery("[id$=divDocName]").hide();
    if ($jQuery("[id$=divExistingDoc]").length > 0 && $jQuery("[id$=hdnIsDocumentChanged]").length > 0 && $jQuery("[id$=hdnIsDocumentChanged]").val() == "true") {
        $jQuery("[id$=divExistingDoc]").hide();
    }
    if (visible) {
        $jQuery("[id$=divPreviewAndSaveDocument]").show();
    }
    else {
        $jQuery("[id$=divPreviewAndSaveDocument]").hide();
    }
}

function clearDocumentsHiddenFields() {
    $jQuery("[id$=hdnPDFAcroFields]").val("");
    $jQuery("[id$=hdnIsDocumentSaved]").val("");
    $jQuery("[id$= hdnDocumentFileName]").val("");
}

function onFileUploadedZeroSize(sender, args) {
    clearDocumentsHiddenFields();
    var fileSize = args.get_fileInfo().ContentLength;
    //Added minimum file size check regarding UAT-862: WB: As a student or an admin, I should not be allowed to upload documents with a size of 0
    if (fileSize > 0) {
        if (sender.getUploadedFiles() != "") {

            if ($jQuery("[id$=hdnIsDocumentChanged]") != undefined && $jQuery("[id$=hdnIsDocumentChanged]").length > 0) {
                $jQuery("[id$=hdnIsDocumentChanged]").val("true");
            }
            showHideButton(true);
        }
        else {
            sender.deleteFileInputAt(selectedFileIndex);
            sender.updateClientState();
            alert("File size should be more than 0 byte.");
            return;
        }
    }
}

function OnSelectedVideoRadioButton(event) {
    var id = "[id$=" + event.id + "]";
    var rdButtonListId = "#" + event.id + " input[type=radio]:checked";
    var selectedvalue = $jQuery(rdButtonListId)[0].value;
    if (selectedvalue == 'true') {
        $jQuery("[id$=divVideoDuration]").show();
        EnableValidator($jQuery("[id$=rfvVideoSeconds]")[0].id);
        EnableValidator($jQuery("[id$=rfvVideoMinutes]")[0].id);
    }
    else {
        $jQuery("[id$=divVideoDuration]").hide();
        findRadControlByServerId("txtVideoSeconds").clear();
        findRadControlByServerId("txtVideoMinutes").clear();
        DisableValidator($jQuery("[id$=rfvVideoSeconds]")[0].id);
        DisableValidator($jQuery("[id$=rfvVideoMinutes]")[0].id);
    }
}

function findRadControlByServerId(Id) {
    var buttonId = $find($jQuery("[id$=" + Id + "]").attr('id'));
    return buttonId;
}

function EnableValidator(id) {
    if ($jQuery('#' + id)[0] != undefined) {
        ValidatorEnable($jQuery('#' + id)[0], true);
        $jQuery('#' + id).hide();
    }
}

function DisableValidator(id) {
    if ($jQuery('#' + id)[0] != undefined) {
        ValidatorEnable($jQuery('#' + id)[0], false);
    }
}

function show_progress_OnSubmit() {
    Page.showProgress('Processing...');
}



//Function to redirect to parent 
function returnToParent() {
    var oArg = {};
    oArg.Action = "Submit";
    var oWnd = GetRadWindow();
    oWnd.Close(oArg);
}

//function to get current popup window
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

var winopen = false;
function OpenAddAnotherItemPopup() {
    var popupWindowName = "Add Another Item";
    var fromScreenName = "Manage Rotation Expiry";
    var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    winopen = true;
    var url = $page.url.create("~/RotationPackages/Pages/AddAnotherItemPopup.aspx?SelectedTenantID=" + tenantID);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "500,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName }
       );
    return false;
}

function OpenAddAnotherFieldPopup() {
    var popupWindowName = "Add Another Field";
    var fromScreenName = "Manage Rotation Field";
    var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    winopen = true;
    var url = $page.url.create("~/RotationPackages/Pages/AddAnotherFieldPopup.aspx?SelectedTenantID=" + tenantID);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "600,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName }
       );
    return false;
}

function OpenDocumentPreviewWindowPopup() {
    var popupWindowName = "Preview Document Window";
    var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    winopen = true;
    var url = $page.url.create("~/RotationPackages/Pages/PreviewDocumentWindowPage.aspx?SelectedTenantID=" + tenantID);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName, onclose: OnDocumentPreviewWindowClose });
    return false;
}

function OpenEditDocumentPreviewWindowPopup() {
    var popupWindowName = "Preview Edit Document Window";
    var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    winopen = true;
    var url = "RotationPackages/Pages/PreviewDocumentWindowPage.aspx?SelectedTenantID=" + tenantID;
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName, onclose: OnDocumentPreviewWindowClose });
    return false;
}

//Function to close popup window
function OnDocumentPreviewWindowClose(oWnd, args) {
    var arg = args.get_argument();
    if (arg.IsDocumentSaved == false) {
        showHideButton(false);
        clearDocumentsHiddenFields();
    }
    oWnd.remove_close(OnDocumentPreviewWindowClose);
    if (arg) {
        $jQuery("[id$=hdnPDFAcroFields]").val(arg.pdfAcroFields);
        $jQuery("[id$=hdnIsDocumentSaved]").val(arg.IsDocumentSaved);
        if ($jQuery("[id$=divExistingDoc]").length > 0 && $jQuery("[id$=hdnIsDocumentChanged]").length > 0 && $jQuery("[id$=hdnIsDocumentChanged]").val() == "true") {
            $jQuery("[id$=divExistingDoc]").hide();
        }
    }
    winopen = false;

}

function OnClientClose(oWnd, args) {
    oWnd.get_contentFrame().src = ''; //This is added for fixing pop-up close issue in Safari browser.
    oWnd.remove_close(OnClientClose);
    if (winopen) {
        var arg = args.get_argument();
        if (arg) {;
        }
        winopen = false;
    }
}

function ClosePopupAndRedirectToTargetPage(url) {
    top.$window.get_radManager().getActiveWindow().close();
    winopen = false;
    window.top.parent.location.href = url;
}

function ShowAddCategoryField(sender, args) {
    $jQuery("[id$=divAddCategory]").show();
}

function ShowAddExpiryField(sender, args) {
    if ($jQuery("[id$=hdnIsExistingItem]").val() == "true") {
        $jQuery("[id$=divAddItem]").show();
    }
    else {
        $jQuery("[id$=divAddExpiry]").show();
    }

}

function ShowAddItemField(sender, args) {
    $jQuery("[id$=divAddItem]").show();
}

function OnSelectedExpiryTypeRadioButton(event) {
    //var id = "[id$=" + event.id + "]";
    //var rdButtonListId = "#" + event.id + " input[type=radio]:checked";
    //var selectedvalue = $jQuery(rdButtonListId)[0].value;
    if (event.value == 'AAAF') {
        $jQuery("[id$=divExpiresIn]").show();
        $jQuery("[id$=divExpiresOn]").hide();
        EnableValidator($jQuery("[id$=rfvExpirationValue]")[0].id);
        EnableValidator($jQuery("[id$=rfvExpirationValueType]")[0].id);
        EnableValidator($jQuery("[id$=rfvDateTypeFields]")[0].id);
        DisableValidator($jQuery("[id$=rfvExpiresOn]")[0].id);
        $jQuery("[id$=divExpiresConditionally]").hide();
        DisableValidator($jQuery("[id$=rfvdpkrExpiresOnCond]")[0].id);
        //DisableValidator($jQuery("[id$=cstExprieStart]")[0].id);
        DisableValidator($jQuery("[id$=rfvdpExprieStart]")[0].id);
        DisableValidator($jQuery("[id$=rfvdpExprieEnd]")[0].id);
    }
    else if (event.value == 'AAAJ')
    {
        //UAT-2165
        $jQuery("[id$=divExpiresIn]").hide();
        $jQuery("[id$=divExpiresOn]").hide();
        DisableValidator($jQuery("[id$=rfvExpiresOn]")[0].id);
        DisableValidator($jQuery("[id$=rfvExpirationValue]")[0].id);
        DisableValidator($jQuery("[id$=rfvExpirationValueType]")[0].id);
        DisableValidator($jQuery("[id$=rfvDateTypeFields]")[0].id);
        $jQuery("[id$=divExpiresConditionally]").show();
        EnableValidator($jQuery("[id$=rfvdpkrExpiresOnCond]")[0].id);
        //EnableValidator($jQuery("[id$=cstExprieStart]")[0].id);
        EnableValidator($jQuery("[id$=rfvdpExprieStart]")[0].id);
        EnableValidator($jQuery("[id$=rfvdpExprieEnd]")[0].id);
    }
    else
    {
        $jQuery("[id$=divExpiresIn]").hide();
        $jQuery("[id$=divExpiresOn]").show();
        EnableValidator($jQuery("[id$=rfvExpiresOn]")[0].id);
        DisableValidator($jQuery("[id$=rfvExpirationValue]")[0].id);
        DisableValidator($jQuery("[id$=rfvExpirationValueType]")[0].id);
        DisableValidator($jQuery("[id$=rfvDateTypeFields]")[0].id);
        $jQuery("[id$=divExpiresConditionally]").hide();
        DisableValidator($jQuery("[id$=rfvdpkrExpiresOnCond]")[0].id);
        //DisableValidator($jQuery("[id$=cstExprieStart]")[0].id);
        DisableValidator($jQuery("[id$=rfvdpExprieStart]")[0].id);
        DisableValidator($jQuery("[id$=rfvdpExprieEnd]")[0].id);
    }
}

//UAT-2165
function ValdateFrmToExpDate(sender, args) {
    var startDate = $jQuery("[id$=dpExprieStart]")[0].control.get_selectedDate();
    var endDate = $jQuery("[id$=dpExprieEnd]")[0].control.get_selectedDate();
    if (endDate != null && startDate == null) {
        sender.innerText = 'Expiration Start Date is required.'
        args.IsValid = false;
    }
    if (startDate != null && endDate == null) {
        sender.innerText = 'Expiration End Date is required.'
        args.IsValid = false;
    }
}
//UAT-2165
function ShowHelpText(sender, args) {
    //debugger;
    var dpStartDate = $jQuery("[id$=dpExprieStart]")[0];
    var dpEndDate = $jQuery("[id$=dpExprieEnd]")[0];
    var dpExpireDate = $jQuery("[id$=dpkrExpiresOnCond]")[0];
    if (dpStartDate != null && dpEndDate != null && dpExpireDate != null) {
        var startDate = dpStartDate.control.get_selectedDate();
        var endDate = dpEndDate.control.get_selectedDate();
        var ExpireDate = dpExpireDate.control.get_selectedDate();
        var ExpInRange = $jQuery("[id$=lblInRange]");
        var ExpNotInRange = $jQuery("[id$=lblNotInRange]");
        var dvShowHelpText = $jQuery("[id$=divShowHelpText]");
        var cmbDataField = $jQuery("[id$=cmbDateTypeFieldForExpCond]");
        var lblDataFieldName = $jQuery("[id$=lblDataField]");
        if (startDate != null && endDate != null && ExpireDate != null && cmbDataField.val() != '--SELECT--') {
            dvShowHelpText.show();
            lblDataFieldName.text(cmbDataField.val());
            ExpNotInRange.text(ExpireDate.format("MM/dd"));
            ExpInRange.text(ExpireDate.format("MM/dd"));
        }
        else {
            dvShowHelpText.hide();
        }
    }
}

function OpenEditRequirementCategoryPopup() {
    var popupWindowName = "Edit Category";
    var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    winopen = true;
    var url = $page.url.create("~/RotationPackages/Pages/EditRequirementCategory.aspx?SelectedTenantID=" + tenantID);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName }
       );
    return false;
}

function OpenEditRequirementItemPopup() {
    var popupWindowName = "Edit Item";
    var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    winopen = true;
    var url = $page.url.create("~/RotationPackages/Pages/EditRequirementItem.aspx?SelectedTenantID=" + tenantID);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName }
       );
    return false;
}

function OpenEditRequirementFieldPopup() {
    var popupWindowName = "Edit Field";
    var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    winopen = true;
    var url = $page.url.create("~/RotationPackages/Pages/EditRequirementField.aspx?SelectedTenantID=" + tenantID);
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName }
       );
    return false;
}

function OpenEditRequirementPackagePopup() {
    var popupWindowName = "Edit Package";
    var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    winopen = true;
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var url = $page.url.create("~/RotationPackages/Pages/EditRequirementPackage.aspx?SelectedTenantID=" + tenantID);
    var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName }
       );
    return false;
}

function ViewVideo() {
    var popupWindowName = "View Video Window";
    var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    winopen = true;
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var url = $page.url.create("~/ApplicantRotationRequirement/Pages/SharedUserViewVideoPopup.aspx?TenantID=" + tenantID + "&IsFromAdmin=True");
    var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName });
    return false;
}

function ViewVideoFromPopup() {
    var popupWindowName = "View Video Window";
    var tenantID = $jQuery("[id$=hdnSelectedTenantID]").val();
    winopen = true;
    //UAT-2364
    var popupHeight = $jQuery(window).height() * (100 / 100);

    var url = "ApplicantRotationRequirement/Pages/SharedUserViewVideoPopup.aspx?TenantID=" + tenantID + "&IsFromAdmin=True";
    var win = $window.createPopup(url, { size: "900,"+popupHeight, behaviors: Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Modal, name: popupWindowName });
    return false;
}


////Function to close popup window
//function OnEditWindowClose(oWnd, args) {
//    oWnd.remove_close(OnClientClose);
//    var grid = $find($jQuery("[id$=grdCategory]")[0].id);
//    var MasterTable = grid.get_masterTableView();
//    MasterTable.rebind();
//    winopen = false;
//}