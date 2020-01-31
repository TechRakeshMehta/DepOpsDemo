/// <reference path="../../Generic/ref.js" />

$jQuery(document).ready(function () {
    if ($jQuery("[id$=rdbSpecifyAuthentication]").length > 0) {
        $jQuery("[id$=rdbSpecifyAuthentication]").off('change');
        $jQuery("[id$=rdbSpecifyAuthentication]").on('change', function () {
            var selectedValue = $jQuery("[id$=rdbSpecifyAuthentication]").find(":checked").val();
            var txtPhoneNumberIsRequired = false;

            if (selectedValue == 'AAAB') {
                txtPhoneNumberIsRequired = true;
            }
            else {
                if ($jQuery("[id$=rdbTextNotification]").length > 0) {
                    var selectedTextNotificationValue = $jQuery("[id$=rdbTextNotification]").find(":checked").val();

                    if (selectedTextNotificationValue == "True") {
                        txtPhoneNumberIsRequired = true;
                    }
                }
            }

            if (txtPhoneNumberIsRequired) {
                ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], true);
                $jQuery('[id$=spnPhoneNumberReq').show();
            }
            else {
                ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], false);
                $jQuery('[id$=spnPhoneNumberReq').hide();
            }

            $jQuery("[id$=hdnrdbSpecifyAuthenticationCalculatedValue]").val(selectedValue);
        });
    }
});


function OnKeyPress(sender, args) {
    var re = /^[0-9\-\:\b/]$/;
    args.set_cancel(!re.test(args.get_keyCharacter()));
}
function checkCheckBox(source, args) {
    //debugger;
    //var options = document.getElementById("chkBoxList");
    //    var options = $("[id*=chkBoxList] input:checked");
    //var options = $jQuery(".chkBoxList");
    var options = $jQuery(".chkBoxList input:checked");

    for (var i = 0; i < options.length; i++) {
        if (options[i].checked) {
            args.IsValid = true;
            return false;
        }
    }
    args.IsValid = false;
}
var selectedFileIndex;

function onClientFileSelected(sender, args) {
    selectedFileIndex = args.get_rowIndex();
}

function CompareEmail(sender, args) {
    args.IsValid = false;
    var cstValidator = sender.id;
    if (cstValidator == 'cstVConfirmPrimaryEmail' || cstValidator == 'CustomValidator1') {
        var txtEmail = $jQuery("[id$=txtPrimaryEmail]");
        var txtConfirm = $jQuery("[id$=txtConfrimPrimayEmail]");
    }
    else if (cstValidator == 'cstVConfirmSecEmail' || cstValidator == 'CustomValidator2') {
        var txtEmail = $jQuery("[id$=txtSecondaryEmail]");
        var txtConfirm = $jQuery("[id$=txtConfirmSecEmail]");
    }
    if (txtEmail[0].value.toLowerCase() == txtConfirm[0].value.toLowerCase())
        args.IsValid = true;
}

function onFileUploaded(sender, args) {
    var fileSize = args.get_fileInfo().ContentLength;
    //Added minimum file size check regarding UAT-862: WB: As a student or an admin, I should not be allowed to upload documents with a size of 0
    if (fileSize > 0) {
        if (sender.getUploadedFiles() != "") {
            $jQuery("[id$=btnUpload]").click();
        }
    }
    else {
        sender.deleteFileInputAt(selectedFileIndex);
        sender.updateClientState();
        alert("File size should be more than 0 byte.");
        return;
    }
}

var upl_OnClientValidationFailed = function (s, a) {
    var error = false;
    var errorMsg = "";

    var extn = a.get_fileName().substring(a.get_fileName().lastIndexOf('.') + 1, a.get_fileName().length);

    if (a.get_fileName().lastIndexOf('.') != -1) {
        if (s.get_allowedFileExtensions().indexOf(extn) == -1) {
            error = true;
            errorMsg = "! Error: Unsupported File Format";
        }
        else {
            error = true;
            errorMsg = "! Error: File size exceeds 5MB";
        }
    }
    else {
        error = true;
        errorMsg = "! Error: Unrecognized File Format";
    }

    if (error) {
        var row = a.get_row();
        smsg = document.createElement("span");

        smsg.innerHTML = errorMsg;
        smsg.setAttribute("class", "ruFileWrap");
        smsg.setAttribute("style", "color:red;padding-left:10px;");

        row.appendChild(smsg);
    }
}
function EnableDisableLicenseValidators(sender, args) {
    var rfvLicenseState = $jQuery("[id$=rfvLicenseState]");
    var rfvtxtLicenseNO = $jQuery("[id$=rfvtxtLicenseNO]");
    var spnLicenseState = $jQuery("[id$=spnLicenseState]");
    var spnLicenseNumber = $jQuery("[id$=spnLicenseNumber]");
    var cmbState = $jQuery("[id$=cmbState]");
    var txtLicenseNO = $jQuery("input[id$=txtLicenseNO]");
    if (sender._checked) {
        if (rfvLicenseState.length > 0 && spnLicenseState.length > 0) {
            EnableValidator($jQuery("[id$=rfvLicenseState]")[0].id);
            $jQuery("[id$=spnLicenseState]")[0].style.display = "inline";
        }
        if (rfvtxtLicenseNO.length > 0 && spnLicenseNumber.length > 0) {
            EnableValidator($jQuery("[id$=rfvtxtLicenseNO]")[0].id);
            $jQuery("[id$=spnLicenseNumber]")[0].style.display = "inline";
        }
        if (cmbState.length > 0) {
            $find(cmbState.attr("id")).enable();
        }
        if (txtLicenseNO.length > 0) {
            $find(txtLicenseNO.attr("id")).clear();
            $find(txtLicenseNO.attr("id")).enable();
        }
    }
    else {
        if (rfvLicenseState.length > 0 && spnLicenseState.length > 0) {
            DisableValidator($jQuery("[id$=rfvLicenseState]")[0].id);
            $jQuery("[id$=spnLicenseState]")[0].style.display = "none";
        }
        if (rfvtxtLicenseNO.length > 0 && spnLicenseNumber.length > 0) {
            DisableValidator($jQuery("[id$=rfvtxtLicenseNO]")[0].id);
            $jQuery("[id$=spnLicenseNumber]")[0].style.display = "none";
        }
        if (cmbState.length > 0) {
            $find(cmbState.attr("id")).disable();
            $find(cmbState.attr("id")).clearSelection();
        }
        if (txtLicenseNO.length > 0) {
            $find(txtLicenseNO.attr("id")).set_textBoxValue('N/A');
            $find(txtLicenseNO.attr("id")).disable();
        }
    }
}

// Code:: Validator Enabled::
function EnableValidator(id) {
    if ($jQuery('#' + id)[0] != undefined) {
        ValidatorEnable($jQuery('#' + id)[0], true);
        $jQuery('#' + id).hide();
    }
}

//Code:: Validator Disabled ::
function DisableValidator(id) {
    if ($jQuery('#' + id)[0] != undefined) {
        ValidatorEnable($jQuery('#' + id)[0], false);
    }
}

function setPostBackSourceEP() {
    $jQuery('.postbacksource').val('EP');
    window.DashboardChildClick = 1;
}

/*this code is used to delay execution of button click event so that it allows time for execution of focus lost event of dropdown controls*/
var submitclicked = false;
function DelayButtonClick(s, e) {
    if (submitclicked == false) {
        submitclicked = true;
        s.set_autoPostBack(false);
        if (ConfirmSubmit(s, e)) {
            submitclicked = false;
            return;
        }
        else {
            submitclicked = false
            s.set_autoPostBack(true);
            return true;
        }
        return false;
    }
    else return true;
}

function ConfirmSubmit(s, e) {
   
    var IsLocationTenant = $jQuery("[id$=hdnIsLocationTenant]").val();
    var txtNewFirstName = $jQuery("[id$=txtNewFirstName]").val();
    var txtNewLastName = $jQuery("[id$=txtNewLastName]").val();
    var txtNewMiddleName = $jQuery("[id$=txtNewMiddleName]").val();
    //   var txtNewSuffix = $jQuery("[id$=txtAliasNewSuffix]").val();

    // if (txtNewSuffix == 'Enter Suffix' || txtNewSuffix == 'Ingrese un sufijo') { txtNewSuffix = '' }
    if (IsLocationTenant.toLowerCase() == "true" && Page_IsValid && txtNewLastName != undefined && txtNewFirstName != undefined && txtNewMiddleName != undefined &&
        (txtNewFirstName.trim() != '' || txtNewMiddleName.trim() != '' || txtNewLastName.trim() != '')) {//|| txtNewSuffix.length > 0)) {
        $window.showDialog($jQuery(".confirmProfileSave").clone().show(), {
            approvebtn: {
                autoclose: true, text: "Ignore and Continue", click: function (s, e) {
                    $jQuery("[id$=txtNewFirstName]").val('');
                    $jQuery("[id$=txtNewLastName]").val('');
                    $jQuery("[id$=txtNewMiddleName]").val('');
                    $jQuery("[id$=txtAliasNewSuffix]").val('');
                    window.setTimeout(function () {
                        $jQuery("[id$=btnProfilehide").trigger('click');
                        submitclicked = false;
                    }, 100, s);
                }
            }, closeBtn: {
                autoclose: true, text: "Cancel", click: function (s, e) {
                }
            }
        }, 475, 'Alert');
        return true;
    }
    else {
        return false;
    }
}

function OnUploadedZeroSizeFileCheck(sender, args) {
    var fileSize = args.get_fileInfo().ContentLength;
    //Added minimum file size check regarding UAT-862: WB: As a student or an admin, I should not be allowed to upload documents with a size of 0
    if (fileSize <= 0) {
        sender.deleteFileInputAt(selectedFileIndex);
        sender.updateClientState();
        alert("File size should be more than 0 byte.");
        return;
    }
}

function ShowCallBackMessage(docMessage) {
    if (docMessage != '') {
        alert(docMessage);
    }
}


//function ValidateUsername() {
//    //debugger;
//    //if ($jQuery("[id$=hdnCurrentPageName]") != undefined && $jQuery("[id$=hdnCurrentPageName]").length > 0 && $jQuery("[id$=hdnCurrentPageName]")[0].value == "EditProfile") {
//        $jQuery("[id$=dvAccountInfo]").trigger("click");
//        var hdnIsValidatedCurrent = $jQuery("[id$=hdnIsValidatedCurrent]")[0];
//        if (hdnIsValidatedCurrent != undefined && hdnIsValidatedCurrent.value == "1") {
//            if ($jQuery("[id$=lblUserNameMessage]") != undefined && $jQuery("[id$=lblUserNameMessage]").length > 0) {
//                $jQuery("[id$=lblUserNameMessage]")[0].textContent = "";
//            }
//        }
//        if ($jQuery("[id$=txtUsername]").length > 0) {
//            var userName = $jQuery("[id$=txtUsername]")[0].value;
//            if (userName != undefined) {

//                if (userName == "") {
//                    $jQuery("[id$=lblUserNameMessage]")[0].style.color = "red";
//                    $jQuery("[id$=lblUserNameMessage]")[0].textContent = "Username is required.";
//                    $jQuery("[id$=hdnIsValidatedCurrent]")[0].value = "1";
//                    window.location.hash = '#dvMain';
//                    return false;
//                }
//                else if (userName != "" && !userName.match(/^[\.\@a-zA-Z0-9_-]{4,50}$/)) {
//                    $jQuery("[id$=lblUserNameMessage]")[0].style.color = "red";
//                    $jQuery("[id$=lblUserNameMessage]")[0].textContent = "Invalid username. Must have at least 4 chars (A-Z a-z 0-9 . _ - @).";
//                    $jQuery("[id$=hdnIsValidatedCurrent]")[0].value = "1";
//                    window.location.hash = '#dvMain';
//                    return false;
//                }
//            }
//        }
//    //}
//}


function AutoFillSSN(sender, args) {
    if (args.get_checked()) {
        setPostBackSourceEP();
        $find($jQuery("[id$=txtSSN]")[0].id).set_value('111111111');
    }
    else {
        $find($jQuery("[id$=txtSSN]")[0].id).set_value('')
    }
}

function onMVRStateBlur(sender, args) {
    if (sender.get_highlightedItem() != null && (sender.get_originalText() != null && sender.get_originalText() != sender.get_highlightedItem().get_text()))
        sender.get_highlightedItem().select();
    else
        sender.set_text("");
}

//UAT-1578 : Addition of SMS notification
function HideShowPhoneNumber(sender) {
    var rdbTextMessageID = sender.id;
    var isFromApplicantProfile = $jQuery("[id$=hdnIsFromApplicantProfile]").length > 0 ? true : false;

    $jQuery("[id$=hdnIsConfirmMsgVisible]").val('0');
    var rdbTextMessageValue = $jQuery("[id$=" + rdbTextMessageID + "]").find('input:radio:checked')[0].value;

    if (rdbTextMessageValue == "True") {
        ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], true);
        $jQuery('[id$=spnPhoneNumberReq').show();
        $jQuery('[id$=rfvPhoneNumber]').hide();

        if (isFromApplicantProfile) {
            $jQuery("[id$=divHideShowPhoneNumber]")[0].style.display = "block";
        }
    }
    else {
        //$jQuery("[id$=divHideShowPhoneNumber]")[0].style.display = "none";
        //$find($jQuery("[id$=txtPhoneNumber]")[0].id).set_value('');

        if ($jQuery("[id$=rdbSpecifyAuthentication]").length > 0) {

            var selectedValueOfTwoFactor = $jQuery("[id$=rdbSpecifyAuthentication]").find(":checked").val();

            if (selectedValueOfTwoFactor == 'AAAB') {
                ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], true);
                $jQuery('[id$=spnPhoneNumberReq').show();
            }
            else {
                ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], false);
                $jQuery('[id$=spnPhoneNumberReq').hide();
            }
        }
        else {
            ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], false);
            $jQuery('[id$=spnPhoneNumberReq').hide();
        }

        if (isFromApplicantProfile) {
            $jQuery("[id$=divHideShowPhoneNumber]")[0].style.display = "none";
        }

        //var ConfirmationStatusComtrol = $jQuery('[id$=divConfirmationStatus]')[0];
        //if (ConfirmationStatusComtrol != null && ConfirmationStatusComtrol != undefined) {
        //    ConfirmationStatusComtrol.style.display = "none";
        //}
    }
}

function MiddleNameEnableDisable(ID) {
    var IsLocationTenant = $jQuery("[id$=hdnIsLocationTenant]")[0].value;
    var hdnIFYOUDONTHAVEMIDDLENAME = $jQuery("[id$=hdnIFYOUDONTHAVEMIDDLENAME]")[0].value;
    var middlename = $find($jQuery("[id$=txtMiddleName]")[0].id);

    if (ID.checked) {
        //UAT_2169:Send Middle Name and Email address to clearstar in Complio
        var noMiddleNameText = $jQuery("[id$=hdnNoMiddleNameText]")[0].value;

        if (IsLocationTenant.toLowerCase() == "true") {
            $find($jQuery("[id$=txtMiddleName]")[0].id).set_value();

            middlename._element.setAttribute("Placeholder", "")
            middlename._element.setAttribute("title", "");

            ValidatorEnable($jQuery('[id$=revMiddleName]')[0], false);
            $jQuery('[id$=revMiddleName]').hide();
            var RegularExpressionValidator4 = $jQuery('[id$=RegularExpressionValidator4]')[0];
            if (RegularExpressionValidator4 != null && RegularExpressionValidator4 != '') {
                ValidatorEnable(RegularExpressionValidator4, false);
            }

            $find($jQuery("[id$=txtMiddleName]")[0].id).disable();
            ValidatorEnable($jQuery('[id$=rfvMiddleName]')[0], false);
            $jQuery('[id$=spnMiddleName]').hide();

            var RequiredFieldValidator7 = $jQuery('[id$=RequiredFieldValidator7]')[0];
            if (RequiredFieldValidator7 != null && RequiredFieldValidator7 != '') {
                ValidatorEnable(RequiredFieldValidator7, false);
            }
        }
        else {
            $find($jQuery("[id$=txtMiddleName]")[0].id).set_value(noMiddleNameText);
            $find($jQuery("[id$=txtMiddleName]")[0].id).disable();
            ValidatorEnable($jQuery('[id$=rfvMiddleName]')[0], false);
            $jQuery('[id$=spnMiddleName]').hide();

            var RequiredFieldValidator7 = $jQuery('[id$=RequiredFieldValidator7]')[0];
            if (RequiredFieldValidator7 != null && RequiredFieldValidator7 != '') {
                ValidatorEnable(RequiredFieldValidator7, false);
            }
        }
    }
    else {
        if (IsLocationTenant.toLowerCase() == "true") {
            $find($jQuery("[id$=txtMiddleName]")[0].id).enable();
            $find($jQuery("[id$=txtMiddleName]")[0].id).set_value('');

            middlename._element.setAttribute("Placeholder", hdnIFYOUDONTHAVEMIDDLENAME);
            middlename._element.setAttribute("title", hdnIFYOUDONTHAVEMIDDLENAME);

            ValidatorEnable($jQuery('[id$=revMiddleName]')[0], true);
            $jQuery('[id$=revMiddleName]').hide();
            var RegularExpressionValidator4 = $jQuery('[id$=RegularExpressionValidator4]')[0];
            if (RegularExpressionValidator4 != null && RegularExpressionValidator4 != '') {
                ValidatorEnable(RegularExpressionValidator4, true);
            }
            var RequiredFieldValidator7 = $jQuery('[id$=RequiredFieldValidator7]')[0];
            if (RequiredFieldValidator7 != null && RequiredFieldValidator7 != '') {
                ValidatorEnable(RequiredFieldValidator7, true);
            }
            ValidatorEnable($jQuery('[id$=rfvMiddleName]')[0], true);
            $jQuery('[id$=rfvMiddleName]').hide();
            $jQuery('[id$=spnMiddleName]').show();
        }
        else {
            $find($jQuery("[id$=txtMiddleName]")[0].id).enable();
            ValidatorEnable($jQuery('[id$=rfvMiddleName]')[0], true);
            $jQuery('[id$=rfvMiddleName]').hide();
            $jQuery('[id$=spnMiddleName]').show();

            var RequiredFieldValidator7 = $jQuery('[id$=RequiredFieldValidator7]')[0];
            if (RequiredFieldValidator7 != null && RequiredFieldValidator7 != '') {
                ValidatorEnable(RequiredFieldValidator7, true);
            }
        }
    }
}

function AliasMiddleNameEnableDisable(ID) {

    var IsLocationTenant = $jQuery("[id$=hdnIsPersonAliasLocationTenant]")[0].value;
    var middlename = $find($jQuery("[id$=txtNewMiddleName]")[0].id);
    var hdnIFYOUDONTHAVEMIDDLENAME = $jQuery("[id$=hdnIFYOUDONTHAVEMIDDLENAME]")[0].value;
    if (ID.checked) {
        //UAT_2169:Send Middle Name and Email address to clearstar in Complio
        //  var rfvMiddle = $jQuery("[id$=revNewMiddleName]")[0].css('display', 'block');        
        var noMiddleNameText = $jQuery("[id$=hdnNoMiddleNameText]")[0].value;
        if (IsLocationTenant.toLowerCase() == "true") {
            $find($jQuery("[id$=txtNewMiddleName]")[0].id).set_value();

            middlename._element.setAttribute("Placeholder", "");
            middlename._element.setAttribute("title", "");

            ValidatorEnable($jQuery("[id$=revNewMiddleName]")[0], false);
            $find($jQuery("[id$=txtNewMiddleName]")[0].id).disable();
            ValidatorEnable($jQuery("[id$=rfvNewMiddleName]")[0], false);
        }
        else {
            $find($jQuery("[id$=txtNewMiddleName]")[0].id).set_value(noMiddleNameText);
            $find($jQuery("[id$=txtNewMiddleName]")[0].id).disable();
            ValidatorEnable($jQuery("[id$=rfvNewMiddleName]")[0], false);
        }
    }
    else {
        $find($jQuery("[id$=txtNewMiddleName]")[0].id).set_value('');

        middlename._element.setAttribute("Placeholder", hdnIFYOUDONTHAVEMIDDLENAME);
        middlename._element.setAttribute("title", hdnIFYOUDONTHAVEMIDDLENAME);

        $find($jQuery("[id$=txtNewMiddleName]")[0].id).enable();
        ValidatorEnable($jQuery("[id$=rfvNewMiddleName]")[0], true);
        $jQuery("[id$=rfvNewMiddleName]").hide();
        if (IsLocationTenant.toLowerCase() == "true") {
            ValidatorEnable($jQuery("[id$=revNewMiddleName]")[0], true);
            $jQuery("[id$=revNewMiddleName]").hide();
        }

        //$jQuery('[id$=spnMiddleName]').show();
    }
}


function ValidateVerifyAlias(ID) {
    var IsLocationTenant = $jQuery("[id$=hdnIsPersonAliasLocationTenant]")[0].value;

    if (IsLocationTenant.toLowerCase() == "true") {
        if ($jQuery("[id$=chkMiddleNameRequired]")[1].checked) {
            ValidatorEnable($jQuery("[id$=rfvNewMiddleName]")[0], false);
        }
    }
}

function AliasMiddleNameEnableDisableForRepeater(ID) {

    var IsLocationTenant = $jQuery("[id$=hdnIsPersonAliasLocationTenant]")[0].value;
    //var txtMiddleName = $jQuery("[id$=" + ID.id + "]").closest('.sxro').next().find(".mddName").find(".helloAni");
    var hdnIFYOUDONTHAVEMIDDLENAME = $jQuery("[id$=hdnIFYOUDONTHAVEMIDDLENAME]")[0].value;
    var txtMiddleName = $jQuery("[id$=" + ID.id + "]").parentsUntil("[id$=divMiddleNameCheckBoxRepeater]").prev('.aliasDiv').find(".mddName").find(".helloAni");
    //var rfvValidator = $jQuery("[id$=" + ID.id + "]").closest('.sxro').next().find(".vlMiddelName").find(".errmsg");
    var rfvValidator = $jQuery("[id$=" + ID.id + "]").parentsUntil("[id$=divMiddleNameCheckBoxRepeater]").prev('.aliasDiv').find(".vlMiddelName").find(".errmsg");
    if (ID.checked) {
        //UAT_2169:Send Middle Name and Email address to clearstar in Complio
        var noMiddleNameText = $jQuery("[id$=hdnNoMiddleNameText]")[0].value;
        if (IsLocationTenant.toLowerCase() == "true") {
            $find(txtMiddleName[0].id).set_value();
            $find(txtMiddleName[0].id)._element.setAttribute("Placeholder", "");
            $find(txtMiddleName[0].id)._element.setAttribute("title", "");
        }
        else {
            $find(txtMiddleName[0].id).set_value(noMiddleNameText);
        }
        $find(txtMiddleName[0].id).disable();
        ValidatorEnable(rfvValidator[0], false);
        //$jQuery('[id$=spnMiddleName]').hide();
    }
    else {
        if (txtMiddleName[0] != undefined) {
            $find(txtMiddleName[0].id).set_value('');

            $find(txtMiddleName[0].id)._element.setAttribute("Placeholder", hdnIFYOUDONTHAVEMIDDLENAME);
            $find(txtMiddleName[0].id)._element.setAttribute("title", hdnIFYOUDONTHAVEMIDDLENAME);

            $find(txtMiddleName[0].id).enable();
        }
        ValidatorEnable(rfvValidator[0], true);
        rfvValidator.hide();
    }
    //$jQuery('[id$=spnMiddleName]').show();
}


//UAT-2276:Regression testing and performance optimization
function CollapseExpandSMSNotificationPanel(sender) {
    if ($(sender).hasClass("colps")) {
        $jQuery("[id$=hdnIsCollapsed]").val(false);
        $jQuery("[id$=hdnSMSDataAvailableForSave]").val(true);
        var hdnIsSMSDataFetchedFromDB = $jQuery("[id$=hdnIsSMSDataFetchedFromDB]");
        if (hdnIsSMSDataFetchedFromDB.val() == "false") {
            Page.showProgress("Processing...");
            var organizationUserId = $jQuery("[id$=hdnorganizationUserId]").val();
            var currentLoggedInUserId = $jQuery("[id$=hdncurrentLoggedInUserId]").val();
            $jQuery.ajax({
                type: "POST",
                url: '/SearchUI/Default.aspx/GetAndUpdateSMSNotificationData',
                data: "{'organizationUserId': '" + organizationUserId + "', currentLoggedInUserId: '" + currentLoggedInUserId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (Result) {
                    if (Result != undefined) {
                        $jQuery("[id$=hdnIsSMSDataFetchedFromDB]").val(true);
                        HideShowSMSControls(Result.d);
                    } return false;
                },
                error: function (Result) {
                }
            });
        }
    }
    else {
        $jQuery("[id$=hdnIsCollapsed]").val(true);
    }
}

//UAT-2276:Regression testing and performance optimization
function HideShowSMSControls(result) {

    var hdnIsConfirmMsgVisible = $jQuery("[id$=hdnIsConfirmMsgVisible]");
    //var lblConfirmationStatus = $jQuery("[id$=lblConfirmationStatus]");
    //var divHideShowPhoneNumber = $jQuery("[id$=divHideShowPhoneNumber]");
    //var divConfirmationStatus = $jQuery("[id$=divConfirmationStatus]");
    var hdnIsReceiveTextNotification = $jQuery("[id$=hdnIsReceiveTextNotification]");
    var rfvPhoneNumber = $jQuery("[id$=rfvPhoneNumber]");
    var txtPhoneNumber = $jQuery("[id$=txtPhoneNumber]");

    hdnIsReceiveTextNotification.val(result.IsReceiveTextNotification);

    $find(txtPhoneNumber[0].id)._setMask = "(###)-####-###"
    $find(txtPhoneNumber[0].id).set_value(result.PhoneNumber == null ? "" : result.PhoneNumber);

    if (result.IsReceiveTextNotification == true) {

        //lblConfirmationStatus.text(result.ConfirmationStatus);
        //divHideShowPhoneNumber[0].style["display"] = "block";
        //divConfirmationStatus[0].style["display"] = "block";

        hdnIsConfirmMsgVisible.val("1");
        $jQuery('[id$=spnPhoneNumberReq').show();
        ValidatorEnable(rfvPhoneNumber[0], true);
    }
    else {

        //lblConfirmationStatus.val("");
        //divHideShowPhoneNumber[0].style["display"] = "none";
        //divConfirmationStatus[0].style["display"] = "none";

        var selectedValueOfTwoFactor = $jQuery("[id$=rdbSpecifyAuthentication]").find(":checked").val();
        if (selectedValueOfTwoFactor == 'AAAB') {
            ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], true);
            $jQuery('[id$=spnPhoneNumberReq').show();
        }
        else {
            ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], false);
            $jQuery('[id$=spnPhoneNumberReq').hide();
        }

        hdnIsConfirmMsgVisible.val("0");
        ValidatorEnable(rfvPhoneNumber[0], false);
    }

    var rdbTextNotification = $jQuery("[id$=rdbTextNotification]");

    if (result.IsReceiveTextNotification == true) {
        rdbTextNotification.find('input:radio')[0].checked = true
    }
    else {
        rdbTextNotification.find('input:radio')[1].checked = true
    }
    Page.hideProgress();
}

//UAT-2447
function MaskedUnmaskedPhone(ID) {
    if (ID.checked) {
        //$jQuery("[id$=dvMaskedPrimaryPhone]").css('visibility', 'hidden');
        $jQuery("[id$=dvMaskedPrimaryPhone]").css('display', 'none');
        $jQuery("[id$=dvUnmaskedPrimaryPhone]").css('display', 'block');
        ValidatorEnable($jQuery('[id$=rfvTxtMobile]')[0], false);
        ValidatorEnable($jQuery('[id$=revTxtMobile]')[0], false);
        ValidatorEnable($jQuery('[id$=rfvTxtMobileUnmasked]')[0], true);
        ValidatorEnable($jQuery('[id$=revTxtMobilePrmyNonMasking]')[0], true);
        $jQuery("[id$=rfvTxtMobileUnmasked]").hide();
        $jQuery("[id$=revTxtMobilePrmyNonMasking]").hide();
    }
    else {
        $jQuery("[id$=dvMaskedPrimaryPhone]").css('display', 'block');
        $jQuery("[id$=dvUnmaskedPrimaryPhone]").css('display', 'none');
        ValidatorEnable($jQuery('[id$=rfvTxtMobileUnmasked]')[0], false);
        ValidatorEnable($jQuery('[id$=revTxtMobilePrmyNonMasking]')[0], false);
        ValidatorEnable($jQuery('[id$=rfvTxtMobile]')[0], true);
        ValidatorEnable($jQuery('[id$=revTxtMobile]')[0], true);
        $jQuery("[id$=rfvTxtMobile]").hide();
        $jQuery("[id$=revTxtMobile]").hide();
    }
}
//UAT-2447
function MaskUnmaskSecoundaryPhone(ID) {
    if (ID.checked) {
        $jQuery("[id$=dvMaskedSecondaryPhone]").css('display', 'none');
        $jQuery("[id$=dvUnMaskedSecondaryPhone]").css('display', 'block');
        ValidatorEnable($jQuery('[id$=revTxtUnmaskedSecondaryPhone]')[0], true);
        $jQuery("[id$=revTxtUnmaskedSecondary]").hide();
    }
    else {
        $jQuery("[id$=dvMaskedSecondaryPhone]").css('display', 'block');
        $jQuery("[id$=dvUnMaskedSecondaryPhone]").css('display', 'none');
        ValidatorEnable($jQuery('[id$=revTxtUnmaskedSecondaryPhone]')[0], false);
        $jQuery("[id$=revTxtUnmaskedSecondary]").hide();
    }
}

function CheckPhoneNumberRequiredStatus() {

    var selectedValue = $jQuery("[id$=rdbSpecifyAuthentication]").find(":checked").val();
    var txtPhoneNumberIsRequired = false;

    if (selectedValue == 'AAAB') {
        txtPhoneNumberIsRequired = true;
    }
    else {
        if ($jQuery("[id$=rdbTextNotification]").length > 0) {
            var selectedTextNotificationValue = $jQuery("[id$=rdbTextNotification]").find(":checked").val();

            if (selectedTextNotificationValue == "True") {
                txtPhoneNumberIsRequired = true;
            }
        }
    }

    if (txtPhoneNumberIsRequired) {
        ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], true);
        $jQuery('[id$=spnPhoneNumberReq').show();
    }
    else {
        ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], false);
        $jQuery('[id$=spnPhoneNumberReq').hide();
    }
}
//UAT-3084:Popup to resubmit rejected items on student portfolio screen after an alias has been added
function NavigateToRejectedItemListSubmission(orgUserId, tenantId) {
    // debugger;
    var popupWindowName = "Complio";
    var url = $page.url.create("~/ComplianceOperations/Pages/RejectedItemListSubmissionPopup.aspx?orgUserId=" + orgUserId + "&TenantId=" + tenantId);
    //var popupHeight = $jQuery(window).height() * (90 / 100);
    var win = $window.createPopup(url,
        {
            size: "1200," + "670",
            behaviors: Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Maximize, onclose: CloseRejectedSubmissionPopup
        }, function () {
            this.set_title(popupWindowName);
        });
}

function CloseRejectedSubmissionPopup(oWnd, args) {
    oWnd.remove_close(CloseRejectedSubmissionPopup);
}