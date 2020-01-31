/// <reference path="../../Generic/ref.js" />

$jQuery(document).ready(function () {

    CheckDOBForLocationTenant();
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

function pageLoad() {
    var LanguageCode = $jQuery("[id$=hdnLanguageCode]").val();
    if (LanguageCode == 'AAAA')
        $jQuery("[id$=dvNextBtnStyleInSpanish]").removeClass("nextBtnStyleInSpanish");
    if (LanguageCode == 'AAAB')
        $jQuery("[id$=dvNextBtnStyleInSpanish]").addClass("nextBtnStyleInSpanish");
}

function OnKeyPress(sender, args) {
    var re = /^[0-9\-\:\b/]$/;
    args.set_cancel(!re.test(args.get_keyCharacter()));
}

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

var submitclicked = false;
var openGoogleRecaptcha = false;


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
    var txtNewFirstName = $jQuery("[id$=txtNewFirstName]").val();
    var txtNewLastName = $jQuery("[id$=txtNewLastName]").val();
    var txtNewMiddleName = $jQuery("[id$=txtNewMiddleName]").val();
    //   var txtNewSuffix = $jQuery("[id$=txtAliasNewSuffix]").val();
    // if (txtNewSuffix == 'Enter Suffix' || txtNewSuffix == 'Ingrese un sufijo') { txtNewSuffix = '' }
    if (Page_IsValid && txtNewLastName != undefined && txtNewFirstName != undefined && txtNewMiddleName != undefined &&
        (txtNewFirstName.trim() != '' || txtNewMiddleName.trim() != '' || txtNewLastName.trim() != '')) {//|| txtNewSuffix.length > 0)) {
        $window.showDialog($jQuery(".confirmProfileSave").clone().show(), {
            approvebtn: {
                autoclose: true, text: "Ignore and Continue", click: function (s, e) {
                    debugger;
                    $jQuery("[id$=txtNewFirstName]").val('');
                    $jQuery("[id$=txtNewLastName]").val('');
                    $jQuery("[id$=txtNewMiddleName]").val('');
                    //$jQuery("[id$=txtAliasNewSuffix]").val('');
                    window.setTimeout(function () {
                        $jQuery("[id$=btnhide]").trigger('click');
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

function MiddleNameEnableDisable(ID) {
    var hdnIFYOUDONTHAVEMIDDLENAME = $jQuery("[id$=hdnIFYOUDONTHAVEMIDDLENAME]")[0].value;
    var middlename = $find($jQuery("[id$=txtMiddleName]")[0].id);

    if (ID.checked) {
        //UAT_2169:Send Middle Name and Email address to clearstar in Complio
        var noMiddleNameText = $jQuery("[id$=hdnNoMiddleNameText]")[0].value;
        $find($jQuery("[id$=txtMiddleName]")[0].id).set_value(noMiddleNameText);
        middlename._element.setAttribute("Placeholder", "")
        middlename._element.setAttribute("title", "");
        //ValidatorEnable($jQuery('[id$=revMiddleName]')[0], false);
        //$jQuery('[id$=revMiddleName]').hide();
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
        $find($jQuery("[id$=txtMiddleName]")[0].id).enable();
        $find($jQuery("[id$=txtMiddleName]")[0].id).set_value('');

        middlename._element.setAttribute("Placeholder", hdnIFYOUDONTHAVEMIDDLENAME);
        middlename._element.setAttribute("title", hdnIFYOUDONTHAVEMIDDLENAME);

        //ValidatorEnable($jQuery('[id$=revMiddleName]')[0], true);
        //$jQuery('[id$=revMiddleName]').hide();
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
}

function MaskedUnmaskedPhone(ID) {
    if (ID.checked) {
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

//function HideShowPhoneNumber(sender) {
//    var rdbTextMessageID = sender.id;
//    var isFromApplicantProfile = $jQuery("[id$=hdnIsFromApplicantProfile]").length > 0 ? true : false;

//    $jQuery("[id$=hdnIsConfirmMsgVisible]").val('0');
//    var rdbTextMessageValue = $jQuery("[id$=" + rdbTextMessageID + "]").find('input:radio:checked')[0].value;

//    if (rdbTextMessageValue == "True") {
//        ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], true);
//        $jQuery('[id$=spnPhoneNumberReq').show();
//        $jQuery('[id$=rfvPhoneNumber]').hide();

//        if (isFromApplicantProfile) {
//            $jQuery("[id$=divHideShowPhoneNumber]")[0].style.display = "block";
//        }
//    }
//    else {
//        if ($jQuery("[id$=rdbSpecifyAuthentication]").length > 0) {

//            var selectedValueOfTwoFactor = $jQuery("[id$=rdbSpecifyAuthentication]").find(":checked").val();

//            if (selectedValueOfTwoFactor == 'AAAB') {
//                ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], true);
//                $jQuery('[id$=spnPhoneNumberReq').show();
//            }
//            else {
//                ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], false);
//                $jQuery('[id$=spnPhoneNumberReq').hide();
//            }
//        }
//        else {
//            ValidatorEnable($jQuery('[id$=rfvPhoneNumber]')[0], false);
//            $jQuery('[id$=spnPhoneNumberReq').hide();
//        }

//        if (isFromApplicantProfile) {
//            $jQuery("[id$=divHideShowPhoneNumber]")[0].style.display = "none";
//        }
//    }
//}

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

function CheckDOBForLocationTenant() {
    Telerik.Web.UI.RadDateInput.prototype.parseDate = function (value, baseDate) {
        try {
            var tokens;
            var lexer = new Telerik.Web.UI.DateParsing.DateTimeLexer(this.get_dateFormatInfo());
            try {
                tokens = lexer.GetTokens(value);
            } catch (e) {

                return value;
            }

            var parser = new Telerik.Web.UI.DateParsing.DateTimeParser(this.get_dateFormatInfo().TimeInputOnly);
            var entry = parser.Parse(tokens);
            baseDate = this._getParsingBaseDate(baseDate);
            var date = entry.Evaluate(baseDate, this.get_dateFormatInfo());
            if (date.getDate() != entry.Second || date.getMonth() + 1 != entry.First || date.getFullYear() != entry.Third) {
                $jQuery("[id$=lblDOBLocationError]").removeClass("HideError");
                $jQuery("[id$=lblDOBLocationError]").addClass("ShowError");
                return null;
            }
            else {
                $jQuery("[id$=lblDOBLocationError]").removeClass("ShowError");
                $jQuery("[id$=lblDOBLocationError]").addClass("HideError");
            }

            return date;
        }
        catch (parseError) {
            if (parseError.isDateParseException) {
            }
            else {
                throw parseError;
            }
        }
    }
}

function onMVRStateBlur(sender, args) {
    //debugger;
    if (sender.get_highlightedItem() != null && (sender.get_originalText() != null && sender.get_originalText() != sender.get_highlightedItem().get_text()))
        sender.get_highlightedItem().select();
    else
        sender.set_text("");
}
